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
    /// Máquina de estados central. Implementa los 5 pasos del slide 4:
    ///   01 Vista general → 02 Acercamiento → 03 Dato + pregunta →
    ///   04 Decide y feedback (mercaderista actúa, HUD se anima) →
    ///   05 Vuelve al mapa (pull-out)
    ///
    /// Decision no es un panel propio: los 3 botones viven dentro de Panel_Situation.
    /// "Probar otra decisión" llama a SituationScreen.MostrarSinFade() para reusar
    /// el mismo panel sin re-animar la entrada.
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

        [Header("HUD persistente (vive fuera del stack de paneles)")]
        [SerializeField] private GameObject hudRoot;
        [SerializeField] private HUDBar hudBar;

        [Header("Cámara y temporizador")]
        [SerializeField] private CameraSequencer cameraSequencer;
        [SerializeField] private IdleTimer idleTimer;

        [Header("Mercaderista por estación (mapeo por id)")]
        [SerializeField] private List<MercaderistaSlot> mercaderistas = new List<MercaderistaSlot>();

        [System.Serializable]
        public class MercaderistaSlot
        {
            public string estacionId;
            public MercaderistaController mercaderista;
            public GondolaState gondolaState;
        }

        public AppState State { get; private set; }
        public EstacionData EstacionActual { get; private set; }
        public OpcionData OpcionActual { get; private set; }
        public ExperienciaConfig Config => config;
        public HUDState Hud { get; private set; }

        private readonly HashSet<string> visitadas = new HashSet<string>();
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

        private void Start() => IrA(AppState.Attract);

        public void IrA(AppState nuevoEstado)
        {
            State = nuevoEstado;
            OcultarPantallas();

            // HUD: oculto sólo en Attract
            if (hudRoot != null) hudRoot.SetActive(nuevoEstado != AppState.Attract);

            if (idleTimer != null)
            {
                if (nuevoEstado == AppState.Attract) idleTimer.StopTracking();
                else idleTimer.StartTracking();
            }

            switch (nuevoEstado)
            {
                case AppState.Attract:
                    ResetHud();
                    cameraSequencer?.IrAVistaAttract();
                    attractScreen?.Show();
                    break;

                case AppState.MapSelection:                  // paso 01
                    cameraSequencer?.IrAVistaGeneral();
                    OcultarMercaderistas();
                    mapScreen?.Mostrar(visitadas, nivelesElegidos.Count);
                    RefrescarHud();
                    break;

                case AppState.Situation:                     // pasos 02 + 03
                    cameraSequencer?.IrAEstacion(EstacionActual);
                    MostrarMercaderista(EstacionActual?.id);
                    situationScreen?.Mostrar(EstacionActual);
                    RefrescarHud();
                    break;

                case AppState.Feedback:                      // paso 04
                    StartCoroutine(EjecutarFeedback());
                    break;

                case AppState.Summary:
                    summaryScreen?.Mostrar(nivelesElegidos, Hud);
                    cameraSequencer?.IrAVistaGeneral();
                    break;
            }
        }

        public void SeleccionarEstacion(EstacionData estacion)
        {
            EstacionActual = estacion;
            IrA(AppState.Situation);
        }

        public void SeleccionarOpcion(OpcionData opcion)
        {
            OpcionActual = opcion;
            nivelesElegidos.Add(opcion.nivelColaboracion);
            if (EstacionActual != null) visitadas.Add(EstacionActual.id);
            IrA(AppState.Feedback);
        }

        private IEnumerator EjecutarFeedback()
        {
            var slot = BuscarMercaderistaSlot(EstacionActual?.id);
            float dur = config != null ? config.duracionAccionMercaderista : 1.2f;
            if (slot != null && slot.mercaderista != null && OpcionActual != null)
                yield return slot.mercaderista.EjecutarAccion(OpcionActual.animacionMercaderista, dur);

            if (slot != null && slot.gondolaState != null && OpcionActual != null)
            {
                if (OpcionActual.esAcertada) slot.gondolaState.MostrarAtendido();
                else                          slot.gondolaState.MostrarSinAtender();
            }

            Hud.AplicarOpcion(OpcionActual);
            RefrescarHud();

            feedbackScreen?.Mostrar(EstacionActual, OpcionActual);
        }

        public void VolverAlMapa()
        {
            // paso 05: pull-out (blend más corto que el dolly de entrada)
            if (config != null) cameraSequencer?.AjustarBlend(config.pullOutSeconds);
            ResetGondolaActual();
            if (config != null && visitadas.Count >= config.estaciones.Count)
                IrA(AppState.Summary);
            else
                IrA(AppState.MapSelection);
            if (config != null) cameraSequencer?.AjustarBlend(config.dollyInSeconds);
        }

        /// <summary>
        /// "Probar otra decisión" desde Feedback: re-muestra el Panel_Situation
        /// (mismo panel donde viven los 3 botones) pero sin re-animar el fade
        /// de la DatoCard. La cámara se queda en la estación, sólo cambia el UI.
        /// </summary>
        public void ProbarOtraDecision()
        {
            State = AppState.Situation;
            OcultarPantallas();
            if (hudRoot != null) hudRoot.SetActive(true);
            ResetGondolaActual();
            situationScreen?.MostrarSinFade(EstacionActual);
            RefrescarHud();
        }

        public void ReiniciarExperiencia()
        {
            visitadas.Clear();
            nivelesElegidos.Clear();
            EstacionActual = null;
            OpcionActual = null;
            ResetHud();
            OcultarMercaderistas();
            foreach (var s in mercaderistas) s?.gondolaState?.MostrarNeutral();
            IrA(AppState.Attract);
        }

        // -------- helpers --------

        private void ResetHud()
        {
            if (config == null) { Hud = new HUDState(0, "OK", 0); return; }
            if (Hud == null) Hud = new HUDState(config.osaInicial, config.invInicial, config.sosInicial);
            else Hud.Reset(config.osaInicial, config.invInicial, config.sosInicial);
            RefrescarHud();
        }

        private void RefrescarHud() => hudBar?.Renderizar(Hud);

        private void OcultarPantallas()
        {
            attractScreen?.Hide();
            mapScreen?.Hide();
            situationScreen?.Hide();
            feedbackScreen?.Hide();
            summaryScreen?.Hide();
        }

        private MercaderistaSlot BuscarMercaderistaSlot(string id) =>
            string.IsNullOrEmpty(id) ? null : mercaderistas.Find(m => m != null && m.estacionId == id);

        private void MostrarMercaderista(string id)
        {
            OcultarMercaderistas();
            var slot = BuscarMercaderistaSlot(id);
            if (slot == null) return;
            slot.mercaderista?.Mostrar();
            slot.gondolaState?.MostrarNeutral();
        }

        private void OcultarMercaderistas()
        {
            foreach (var s in mercaderistas) s?.mercaderista?.Ocultar();
        }

        private void ResetGondolaActual()
        {
            var slot = BuscarMercaderistaSlot(EstacionActual?.id);
            slot?.gondolaState?.MostrarNeutral();
        }
    }
}
