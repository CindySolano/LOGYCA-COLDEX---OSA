using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Screens;
using LOGYCA.OSA.UI.Widgets;
using LOGYCA.OSA.CameraCtrl;

namespace LOGYCA.OSA.Core
{
    /// <summary>
    /// Flujo secuencial:
    ///   Attract → [Ronda 1 → Ronda 2 → ... → Ronda 5] → Summary
    ///
    /// Estados visuales por ronda:
    ///   - Idle:        hotspot de la ronda actual está Activo (pulsa, alpha 100)
    ///                  los demás están Dormidos (alpha bajo, no clickeables)
    ///   - Transición:  click en hotspot → Panel_Map visible "Acercando cámara al hotspot X"
    ///                  cámara hace dolly suave (1.6 s) · mercaderista entra a Idle
    ///   - Pregunta:    Panel_Map se apaga, aparece Panel_Situación con DATO + 3 botones
    ///   - Acción:      click en opción → mercaderista trigger anim · gondolaContenido
    ///                  se enciende si esAcertada · HUD se anima
    ///   - Feedback:    Panel_Feedback con 9 KPIs + cadena/proveedor/veredicto
    ///                  Único botón "Continuar"
    ///   - Continuar:   apaga hotspot actual (queda Visitado) · cámara pull-out ·
    ///                  prende el siguiente hotspot (Activo) · loop o Summary
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

        [Header("HUD persistente")]
        [SerializeField] private GameObject hudRoot;
        [SerializeField] private HUDBar hudBar;

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

        private void Start() => IrA(AppState.Attract);

        // -------- API pública --------

        public void IrA(AppState nuevoEstado)
        {
            State = nuevoEstado;
            OcultarPantallas();
            if (hudRoot != null) hudRoot.SetActive(nuevoEstado != AppState.Attract);

            if (idleTimer != null)
            {
                if (nuevoEstado == AppState.Attract) idleTimer.StopTracking();
                else idleTimer.StartTracking();
            }

            switch (nuevoEstado)
            {
                case AppState.Attract:
                    RondaIndex = -1;
                    ResetHud();
                    ResetHotspots();
                    OcultarMercaderistas();
                    cameraSequencer?.IrAVistaAttract();
                    attractScreen?.Show();
                    break;

                case AppState.MapSelection:
                    // primer entrada al loop: arrancar ronda 1
                    if (RondaIndex < 0) RondaIndex = 0;
                    cameraSequencer?.IrAVistaGeneral();
                    OcultarMercaderistas();
                    ConfigurarHotspotsParaRonda();
                    RefrescarHud();
                    break;

                case AppState.Situation:
                    // transición de cámara + Panel_Map visible mientras dura el dolly
                    StartCoroutine(TransicionHaciaSituation());
                    break;

                case AppState.Feedback:
                    StartCoroutine(EjecutarFeedback());
                    break;

                case AppState.Summary:
                    summaryScreen?.Mostrar(nivelesElegidos, Hud);
                    cameraSequencer?.IrAVistaGeneral();
                    break;
            }
        }

        /// <summary>Llamado por Attract al primer toque.</summary>
        public void EmpezarExperiencia()
        {
            RondaIndex = 0;
            IrA(AppState.MapSelection);
        }

        /// <summary>Llamado por Hotspot3D cuando se hace click sobre el Activo.</summary>
        public void SeleccionarEstacion(EstacionData estacion)
        {
            Debug.Log($"[GameManager] SeleccionarEstacion · estacion={estacion?.id ?? "null"} · ronda={RondaIndex} · esperada={(RondaIndex >= 0 && RondaIndex < config.estaciones.Count ? config.estaciones[RondaIndex].id : "fuera de rango")}");
            if (estacion == null) return;
            if (RondaIndex < 0 || RondaIndex >= config.estaciones.Count) { Debug.LogWarning("[GameManager] RondaIndex fuera de rango"); return; }
            if (config.estaciones[RondaIndex] != estacion) { Debug.LogWarning($"[GameManager] estacion {estacion.id} no es la ronda actual ({config.estaciones[RondaIndex].id})"); return; }
            IrA(AppState.Situation);
        }

        /// <summary>Llamado por DecisionScreen cuando el usuario toca una opción.</summary>
        public void SeleccionarOpcion(OpcionData opcion)
        {
            OpcionActual = opcion;
            nivelesElegidos.Add(opcion.nivelColaboracion);
            IrA(AppState.Feedback);
        }

        /// <summary>Llamado por el botón "Continuar" del Panel_Feedback.</summary>
        public void Continuar()
        {
            // marca el hotspot actual como Visitado
            var actual = HotspotDeRonda(RondaIndex);
            actual?.SetEstado(Hotspot3D.Estado.Visitado);

            // pull-out a vista general
            if (config != null) cameraSequencer?.AjustarBlend(config.pullOutSeconds);
            cameraSequencer?.IrAVistaGeneral();
            OcultarMercaderistas();
            ApagarObjetoContenido(EstacionActual?.id);
            if (config != null) cameraSequencer?.AjustarBlend(config.dollyInSeconds);

            RondaIndex++;
            if (RondaIndex >= config.estaciones.Count)
            {
                IrA(AppState.Summary);
            }
            else
            {
                // arrancar la siguiente ronda: prender el siguiente hotspot
                ConfigurarHotspotsParaRonda();
                IrA(AppState.MapSelection);
            }
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
            IrA(AppState.Attract);
        }

        // -------- corrutinas internas --------

        private IEnumerator TransicionHaciaSituation()
        {
            var estacion = EstacionActual;
            int totalRondas = config != null ? config.estaciones.Count : 0;
            int rondaUsuario = RondaIndex + 1;

            Debug.Log($"[GameManager] TransicionHaciaSituation · estacion={estacion?.id} · camaraVirtualId={estacion?.camaraVirtualId}");

            // Apagar los 5 hotspots para dejar la vista limpia mientras dura la ronda
            OcultarHotspots();

            // Panel_Map visible MIENTRAS la cámara se mueve
            if (mapScreen != null) mapScreen.MostrarTransicion(rondaUsuario, totalRondas, estacion);
            else Debug.LogError("[GameManager] mapScreen es NULL — arrastra el Panel_Map en el Inspector");

            // mover cámara
            if (cameraSequencer != null) cameraSequencer.IrAEstacion(estacion);
            else Debug.LogError("[GameManager] cameraSequencer es NULL — arrastra el Managers/CameraSequencer en el Inspector");

            // mercaderista aparece en Idle
            var slot = BuscarMercaderistaSlot(estacion?.id);
            if (slot == null)
                Debug.LogWarning($"[GameManager] No hay MercaderistaSlot para id='{estacion?.id}'. Verifica el array Mercaderistas del GameManager (tamaño={mercaderistas.Count})");
            else
            {
                if (slot.mercaderista == null) Debug.LogWarning($"[GameManager] Slot id={slot.estacionId} sin MercaderistaController asignado");
                else { slot.mercaderista.Mostrar(); Debug.Log($"[GameManager] Mercaderista de {slot.estacionId} → Mostrar()"); }

                if (slot.gondolaContenido == null) Debug.LogWarning($"[GameManager] Slot id={slot.estacionId} sin GondolaContenido asignado");
                else slot.gondolaContenido.Apagar();
            }

            // esperar dolly
            float dolly = config != null ? config.dollyInSeconds : 1.6f;
            yield return new WaitForSeconds(dolly);

            // ocultar Panel_Map → mostrar Panel_Situación con DATO + 3 botones
            mapScreen?.Hide();
            situationScreen?.Mostrar(estacion);
        }

        private IEnumerator EjecutarFeedback()
        {
            // 1) mercaderista ejecuta la acción (trigger "Trabajando" u otro)
            var slot = BuscarMercaderistaSlot(EstacionActual?.id);
            float dur = config != null ? config.duracionAccionMercaderista : 1.2f;
            if (slot != null && slot.mercaderista != null && OpcionActual != null)
                yield return slot.mercaderista.EjecutarAccion(OpcionActual.animacionMercaderista, dur);

            // 2) "objeto contenido" SIEMPRE se enciende (sin importar la opción elegida)
            slot?.gondolaContenido?.Encender();

            // 3) HUD se anima con los deltas de la opción
            Hud.AplicarOpcion(OpcionActual);
            RefrescarHud();

            // 4) Panel_Feedback con todo el detalle
            feedbackScreen?.Mostrar(EstacionActual, OpcionActual);
        }

        // -------- hotspots --------

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
                h.Show();  // asegurarse que el GameObject esté activo antes de cambiar estado
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

        /// <summary>Esconde TODOS los hotspots (GameObject.SetActive(false)). Se usa al entrar a Situation/Feedback.</summary>
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

        // -------- mercaderistas / contenido --------

        private MercaderistaSlot BuscarMercaderistaSlot(string id) =>
            string.IsNullOrEmpty(id) ? null : mercaderistas.Find(m => m != null && m.estacionId == id);

        private void OcultarMercaderistas()
        {
            foreach (var s in mercaderistas) s?.mercaderista?.Ocultar();
        }

        private void ApagarObjetoContenido(string id)
        {
            var slot = BuscarMercaderistaSlot(id);
            slot?.gondolaContenido?.Apagar();
        }

        private void ApagarTodosLosContenidos()
        {
            foreach (var s in mercaderistas) s?.gondolaContenido?.Apagar();
        }

        // -------- HUD --------

        private readonly List<int> nivelesElegidos = new List<int>();

        private void ResetHud()
        {
            if (config == null) { Hud = new HUDState(0, "OK", 0); return; }
            if (Hud == null) Hud = new HUDState(config.osaInicial, config.invInicial, config.sosInicial);
            else Hud.Reset(config.osaInicial, config.invInicial, config.sosInicial);
            RefrescarHud();
        }

        private void RefrescarHud() => hudBar?.Renderizar(Hud);

        // -------- pantallas --------

        private void OcultarPantallas()
        {
            attractScreen?.Hide();
            mapScreen?.Hide();
            situationScreen?.Hide();
            feedbackScreen?.Hide();
            summaryScreen?.Hide();
        }
    }
}
