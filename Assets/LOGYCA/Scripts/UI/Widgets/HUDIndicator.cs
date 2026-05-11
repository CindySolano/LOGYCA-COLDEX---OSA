using TMPro;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card individual de OSA, INV o SOS para la barra superior.
    /// Replica el bloque de la imagen del HUD del PDF:
    ///   "OSA · 96% ▲    antes 92%"
    ///
    /// El fondo (hud_bar_bg.png) y la sombra/forma vienen del sprite.
    /// Aquí sólo se actualizan los textos.
    /// </summary>
    public class HUDIndicator : MonoBehaviour
    {
        [SerializeField] private IndicadorHud indicador = IndicadorHud.OSA;
        [SerializeField] private TMP_Text textoValor;       // "OSA · 96%"
        [SerializeField] private TMP_Text textoFlecha;      // ▲ ▼ →
        [SerializeField] private TMP_Text textoAnterior;    // "antes 92%"

        public IndicadorHud Indicador => indicador;

        public void RenderizarDesde(HUDState state)
        {
            if (state == null) return;

            switch (indicador)
            {
                case IndicadorHud.OSA:
                    Pintar($"{state.OsaActual}%", state.UltimoOsaDir, $"antes {state.OsaAnterior}%");
                    break;
                case IndicadorHud.INV:
                    Pintar(state.InvActual, state.UltimoInvDir, $"antes {state.InvAnterior}");
                    break;
                case IndicadorHud.SOS:
                    Pintar($"{state.SosActual}%", state.UltimoSosDir, $"antes {state.SosAnterior}%");
                    break;
            }
        }

        private void Pintar(string valor, DeltaDir dir, string anterior)
        {
            string nombre = ConfiguracionCatalogo.NombreIndicador(indicador);
            if (textoValor    != null) textoValor.text    = $"{nombre} · {valor}";
            if (textoFlecha   != null) textoFlecha.text   = ConfiguracionCatalogo.FlechaDelta(dir);
            if (textoAnterior != null) textoAnterior.text = anterior;
        }
    }
}
