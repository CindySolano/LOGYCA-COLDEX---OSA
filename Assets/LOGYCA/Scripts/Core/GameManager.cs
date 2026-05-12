using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Screens;
using LOGYCA.OSA.UI.Widgets;
using LOGYCA.OSA.CameraCtrl;
using LOGYCA.OSA.Audio;

namespace LOGYCA.OSA.Core
{
    /// <summary>
    /// Máquina de estados central con transiciones animadas.
    /// IrA() inicia una corrutina que primero hace FADE OUT del panel actual
    /// (bloqueando), luego setea el nuevo estado, y por último FADE IN del
    /// nuevo panel. Ninguna transición avanza hasta que el fade-out termina.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuración")]
        [SerializeField] private ExperienciaConfig config;

        [Header("Pantallas (hijas de Canvas_Pantallas → Panels)")]
        [SerializeField] private AttractScreen attractScreen;
        [SerializeField] private MapScreen mapScreen;
        [SerializeField] private SituationScreen situationScreen;
        [SerializeField] private FeedbackScreen feedbackScreen;
        [SerializeField] private SummaryScreen summaryScreen;

        [Header("HUD persistente (oculto durante toda la experiencia por ahora)")]
        [SerializeField] private GameObject hudRoot;
        [SerializeField] private HUDBar hudBar;

        [Header("Intro pre-Attract (CanvasIntroManager)")]
        [SerializeField] private IntroManager introManager;

        [Header("Audio ambiente (off en Intro/Attract · on durante el juego)")]
        [SerializeField] private AmbientAudio ambientAudio;

        [Header("Cámara, hotspots, mercaderistas")]
        [SerializeField] private CameraSequencer cameraSequencer;
        [SerializeField] private IdleTimer idleTimer;
        [SerializeField] private List<Hotspot3D> hotspots = new List<Hotspot3D>();
        [SerializeField] private List<MercaderistaSlot> mercaderistas = new List<MercaderistaSlot>();

        [System.Serializable]
        public class MercaderistaSlot
        {
            public string estacionId;
            public MercaderistaController mercaderista;
            public GondolaContenido gondolaContenido;
        }

        public AppState State { get; private set; }
        public EstacionData EstacionActual => RondaIndex >= 0 && RondaIndex < config.estaciones.Count
                                                ? config.estaciones[RondaIndex] : null;
        public OpcionData OpcionActual { get; private set; }
        public ExperienciaConfig Config => config;
        public HUDState Hud { get; private set; }
        public int RondaIndex { get; private set; } = -1;

        private Coroutine transicionActiva;
        private readonly List<int> nivelesElegidos = new List<int>();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            ResetHud();
            if (idleTimer != null)
            {
                idleTimer.timeoutSeconds = config != null ? config.idleTimeoutSeconds : 60f;
                idleTimer.OnIdle = ReiniciarExperiencia;
            }
        }

        private void Start() => IrA(AppState.Intro);

        // ────────────────── API pública ──────────────────

        public void IrA(AppState nuevoEstado)
        {
            if (transicionActiva != null) StopCoroutine(transicionActiva);
            transicionActiva = StartCoroutine(TransicionarA(nuevoEstado));
        }

        public void EmpezarExperiencia()
        {
            RondaIndex = 0;
            IrA(AppState.MapSelection);
        }

        public void SeleccionarEstacion(EstacionData estacion)
        {
            Debug.Log($"[GameManager] SeleccionarEstacion · estacion={estacion?.id ?? "null"} · ronda={RondaIndex} · esperada={(RondaIndex >= 0 && RondaIndex < config.estaciones.Count ? config.estaciones[RondaIndex].id : "fuera de rango")}");
            if (estacion == null) return;
            if (RondaIndex < 0 || RondaIndex >= config.estaciones.Count) { Debug.LogWarning("[GameManager] RondaIndex fuera de rango"); return; }
            if (config.estaciones[RondaIndex] != estacion) { Debug.LogWarning($"[GameManager] estacion {estacion.id} no es la ronda actual ({config.estaciones[RondaIndex].id})"); return; }
            IrA(AppState.Situation);
        }

        public void SeleccionarOpcion(OpcionData opcion)
        {
            OpcionActual = opcion;
            nivelesElegidos.Add(opcion.nivelColaboracion);
            IrA(AppState.Feedback);
        }

        public void Continuar()
        {
            if (transicionActiva != null) StopCoroutine(transicionActiva);
            transicionActiva = StartCoroutine(EjecutarContinuar());
        }

        /// <summary>
        /// Secuencia bloqueante al tocar "Continuar":
        ///   1. Marca hotspot como visitado
        ///   2. AGUARDA el fade out del Panel_Feedback (no puede continuar antes)
        ///   3. Pull-out de cámara + ocultar mercaderistas
        ///   4. Espera a que termine el blend del pull-out
        ///   5. Restaura blend a velocidad de dolly-in
        ///   6. Avanza ronda → IrA(MapSelection) o IrA(Summary)
        /// </summary>
        private IEnumerator EjecutarContinuar()
        {
            var actual = HotspotDeRonda(RondaIndex);
            actual?.SetEstado(Hotspot3D.Estado.Visitado);

            // 2. Bloqueante: fade out del feedback ANTES de mover la cámara.
            if (feedbackScreen != null && feedbackScreen.IsVisible)
                yield return feedbackScreen.FadeOut();

            // 3. Recién ahora pull-out de cámara.
            if (config != null) cameraSequencer?.AjustarBlend(config.pullOutSeconds);
            cameraSequencer?.IrAVistaGeneral();
            OcultarMercaderistas();

            // 4. Esperar a que la cámara llegue.
            float pullOut = config != null ? config.pullOutSeconds : 1.4f;
            yield return new WaitForSeconds(pullOut);

            // 5. Restaurar blend para el próximo dolly-in.
            if (config != null) cameraSequencer?.AjustarBlend(config.dollyInSeconds);

            // 6. Avanzar ronda.
            RondaIndex++;
            transicionActiva = null;
            if (RondaIndex >= config.estaciones.Count)
                IrA(AppState.Summary);
            else
                IrA(AppState.MapSelection);
        }

        public void ReiniciarExperiencia()
        {
            RondaIndex = -1;
            nivelesElegidos.Clear();
            OpcionActual = null;
            ResetHud();
            ResetHotspots();
            ApagarTodosLosContenidos();
            OcultarMercaderistas();
            IrA(AppState.Intro);  // reset arranca desde el Intro, no desde Attract
        }

        // ────────────────── Coroutine principal ──────────────────

        private IEnumerator TransicionarA(AppState nuevoEstado)
        {
            // 1. Fade out del panel visible (bloquea hasta terminar)
            yield return FadeOutPanelVisible();

            // 2. Establecer estado
            State = nuevoEstado;

            // HUD oculto durante toda la experiencia por el momento.
            if (hudRoot != null) hudRoot.SetActive(false);

            // Idle timer: NO se trackea en Intro (es la pantalla de bienvenida).
            //             SÍ se trackea en el resto.
            if (idleTimer != null)
            {
                if (nuevoEstado == AppState.Intro) idleTimer.StopTracking();
                else idleTimer.StartTracking();
            }

            // CanvasIntroManager: solo visible en Intro. En cualquier otro estado, off.
            if (introManager != null && nuevoEstado != AppState.Intro)
                introManager.gameObject.SetActive(false);

            // Audio ambiente: off en Intro/Attract · on durante el juego.
            if (ambientAudio != null)
            {
                bool enJuego = nuevoEstado != AppState.Intro && nuevoEstado != AppState.Attract;
                if (enJuego) ambientAudio.Reproducir();
                else         ambientAudio.Detener();
            }

            // 3. Setup específico + fade in del nuevo panel
            switch (nuevoEstado)
            {
                case AppState.Intro:
                    // Reset duro de todo el estado de juego.
                    RondaIndex = -1;
                    ResetHud();
                    ResetHotspots();
                    OcultarMercaderistas();
                    ApagarTodosLosContenidos();
                    // El IntroManager maneja sus propios fades y cámara internamente.
                    // Restaurar visual state al inicial (intro panel visible, cameraIntro on, walls on).
                    if (introManager != null) introManager.Reiniciar();
                    break;

                case AppState.Attract:
                    // Actualmente no se usa (el Intro lo reemplaza). Lo dejo por
                    // compatibilidad: si en algún momento se vuelve a la pantalla
                    // de Toca para empezar, ya está cableado.
                    RondaIndex = -1;
                    ResetHud();
                    ResetHotspots();
                    OcultarMercaderistas();
                    ApagarTodosLosContenidos();
                    cameraSequencer?.IrAVistaAttract();
                    if (attractScreen != null) yield return attractScreen.FadeIn();
                    break;

                case AppState.MapSelection:
                    if (RondaIndex < 0) RondaIndex = 0;
                    cameraSequencer?.IrAVistaGeneral();
                    OcultarMercaderistas();
                    ConfigurarHotspotsParaRonda();
                    RefrescarHud();
                    // sin panel que mostrar — la vista es la escena 3D
                    break;

                case AppState.Situation:
                    yield return TransicionHaciaSituation();
                    break;

                case AppState.Feedback:
                    yield return EjecutarFeedback();
                    break;

                case AppState.Summary:
                    cameraSequencer?.IrAVistaGeneral();
                    if (summaryScreen != null)
                    {
                        summaryScreen.Mostrar(nivelesElegidos, Hud);
                        yield return summaryScreen.FadeIn();
                    }
                    break;
            }

            transicionActiva = null;
        }

        /// <summary>
        /// Fade-out de cualquier panel actualmente visible. Bloquea hasta que
        /// el alpha llegue a 0 y el GameObject quede inactivo.
        /// </summary>
        private IEnumerator FadeOutPanelVisible()
        {
            if (attractScreen   != null && attractScreen.IsVisible)   yield return attractScreen.FadeOut();
            if (mapScreen       != null && mapScreen.IsVisible)       yield return mapScreen.FadeOut();
            if (situationScreen != null && situationScreen.IsVisible) yield return situationScreen.FadeOut();
            if (feedbackScreen  != null && feedbackScreen.IsVisible)  yield return feedbackScreen.FadeOut();
            if (summaryScreen   != null && summaryScreen.IsVisible)   yield return summaryScreen.FadeOut();
        }

        // ────────────────── Transición Situation ──────────────────

        private IEnumerator TransicionHaciaSituation()
        {
            var estacion = EstacionActual;
            int totalRondas = config != null ? config.estaciones.Count : 0;
            int rondaUsuario = RondaIndex + 1;

            Debug.Log($"[GameManager] TransicionHaciaSituation · estacion={estacion?.id} · camaraVirtualId={estacion?.camaraVirtualId}");

            OcultarHotspots();

            // Panel_Map fade in (mientras la cámara hace dolly)
            if (mapScreen != null)
            {
                mapScreen.MostrarTransicion(rondaUsuario, totalRondas, estacion);
                StartCoroutine(mapScreen.FadeIn());
            }
            else Debug.LogError("[GameManager] mapScreen es NULL");

            // Mover cámara
            if (cameraSequencer != null) cameraSequencer.IrAEstacion(estacion);
            else Debug.LogError("[GameManager] cameraSequencer es NULL");

            // Mercaderista visible + apagar góndola contenido durante la pregunta
            var slot = BuscarMercaderistaSlot(estacion?.id);
            if (slot == null)
                Debug.LogWarning($"[GameManager] No hay MercaderistaSlot para id='{estacion?.id}' (tamaño={mercaderistas.Count})");
            else
            {
                if (slot.mercaderista == null) Debug.LogWarning($"[GameManager] Slot id={slot.estacionId} sin MercaderistaController");
                else { slot.mercaderista.Mostrar(); Debug.Log($"[GameManager] Mercaderista de {slot.estacionId} → Mostrar()"); }

                if (slot.gondolaContenido == null) Debug.LogWarning($"[GameManager] Slot id={slot.estacionId} sin GondolaContenido");
                else slot.gondolaContenido.Apagar();
            }

            // Esperar dolly (la cámara llega al hotspot)
            float dolly = config != null ? config.dollyInSeconds : 1.6f;
            yield return new WaitForSeconds(dolly);

            // Panel_Map fade out → Panel_Situation fade in
            if (mapScreen != null) yield return mapScreen.FadeOut();
            if (situationScreen != null)
            {
                situationScreen.Mostrar(estacion);
                yield return situationScreen.FadeIn();
            }
        }

        // ────────────────── Feedback ──────────────────

        private IEnumerator EjecutarFeedback()
        {
            // 1) Mercaderista ejecuta la acción
            var slot = BuscarMercaderistaSlot(EstacionActual?.id);
            float dur = config != null ? config.duracionAccionMercaderista : 1.2f;
            if (slot != null && slot.mercaderista != null && OpcionActual != null)
                yield return slot.mercaderista.EjecutarAccion(OpcionActual.animacionMercaderista, dur);

            // 2) Objeto contenido se enciende al terminar de responder
            slot?.gondolaContenido?.Encender();

            // 3) HUD se anima con los deltas
            Hud.AplicarOpcion(OpcionActual);
            RefrescarHud();

            // 4) Panel_Feedback con fade in
            if (feedbackScreen != null)
            {
                feedbackScreen.Mostrar(EstacionActual, OpcionActual);
                yield return feedbackScreen.FadeIn();
            }
        }

        // ────────────────── Hotspots ──────────────────

        private void ConfigurarHotspotsParaRonda()
        {
            EstacionData esp = (RondaIndex >= 0 && RondaIndex < config.estaciones.Count)
                ? config.estaciones[RondaIndex] : null;
            Debug.Log($"[GameManager] ConfigurarHotspotsParaRonda · ronda={RondaIndex} · hotspots.Count={hotspots.Count} · estacionEsperada={esp?.id ?? "null"}");
            if (esp == null) return;

            foreach (var h in hotspots)
            {
                if (h == null) { Debug.LogWarning("[GameManager] hotspot null en la lista"); continue; }
                if (h.estacion == null) { Debug.LogWarning($"[GameManager] {h.name} sin EstacionData asignado"); continue; }
                h.Show();
                if (h.estacion.id == esp.id) h.SetEstado(Hotspot3D.Estado.Activo);
                else if (YaVisitada(h.estacion.id)) h.SetEstado(Hotspot3D.Estado.Visitado);
                else h.SetEstado(Hotspot3D.Estado.Dormido);
            }
        }

        private void ResetHotspots()
        {
            foreach (var h in hotspots)
            {
                if (h == null) continue;
                h.Show();
                h.SetEstado(Hotspot3D.Estado.Dormido);
            }
        }

        private void OcultarHotspots()
        {
            foreach (var h in hotspots) h?.Hide();
        }

        private bool YaVisitada(string id)
        {
            for (int i = 0; i < RondaIndex; i++)
                if (i >= 0 && i < config.estaciones.Count && config.estaciones[i].id == id)
                    return true;
            return false;
        }

        private Hotspot3D HotspotDeRonda(int rondaIdx)
        {
            if (rondaIdx < 0 || rondaIdx >= config.estaciones.Count) return null;
            var id = config.estaciones[rondaIdx].id;
            return hotspots.Find(h => h != null && h.estacion != null && h.estacion.id == id);
        }

        // ────────────────── Mercaderistas / contenido ──────────────────

        private MercaderistaSlot BuscarMercaderistaSlot(string id) =>
            string.IsNullOrEmpty(id) ? null : mercaderistas.Find(m => m != null && m.estacionId == id);

        private void OcultarMercaderistas()
        {
            foreach (var s in mercaderistas) s?.mercaderista?.Ocultar();
        }

        private void ApagarTodosLosContenidos()
        {
            foreach (var s in mercaderistas) s?.gondolaContenido?.Apagar();
        }

        // ────────────────── HUD ──────────────────

        private void ResetHud()
        {
            if (config == null) { Hud = new HUDState(0, "OK", 0); return; }
            if (Hud == null) Hud = new HUDState(config.osaInicial, config.invInicial, config.sosInicial);
            else Hud.Reset(config.osaInicial, config.invInicial, config.sosInicial);
            RefrescarHud();
        }

        private void RefrescarHud() => hudBar?.Renderizar(Hud);
    }
}
