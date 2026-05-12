using TMPro;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card individual del HUD. Versión compacta: un solo TMP_Text con rich
    /// text que combina nombre + valor + flecha.
    ///
    /// Variables disponibles en la plantilla (string.Format):
    ///   {0} = nombre del indicador  ("OSA")
    ///   {1} = valor actual          ("4%" o "→ 0")
    ///   {2} = flecha de dirección   ("▲" / "▼" / "→")
    ///   {3} = valor anterior        ("0%")  (opcional)
    /// </summary>
    public class HUDIndicator : MonoBehaviour
    {
        [SerializeField] private IndicadorHud indicador = IndicadorHud.OSA;
        [SerializeField] private TMP_Text texto;

        [Tooltip("Plantilla rich-text. {0}=nombre · {1}=valor · {2}=flecha · {3}=anterior.\n\n" +
                 "Ejemplos:\n" +
                 "  \"{0}\\n<size=20>{1} {2}</size>\"\n" +
                 "  \"{0} {2}\\n<size=18>{1}</size>\\n<size=12>antes {3}</size>\"")]
        [TextArea(2, 5)]
        [SerializeField] private string plantilla = "{0}\n<size=20>{1} {2}</size>";

        public IndicadorHud Indicador => indicador;

        public void RenderizarDesde(HUDState state)
        {
            if (state == null || texto == null) return;

            string nombre = ConfiguracionCatalogo.NombreIndicador(indicador);
            string valor;
            string anterior;
            DeltaDir dir;

            switch (indicador)
            {
                case IndicadorHud.OSA:
                    valor    = $"{state.OsaActual}%";
                    anterior = $"{state.OsaAnterior}%";
                    dir      = state.UltimoOsaDir;
                    break;
                case IndicadorHud.INV:
                    valor    = state.InvActual;
                    anterior = state.InvAnterior;
                    dir      = state.UltimoInvDir;
                    break;
                case IndicadorHud.SOS:
                    valor    = $"{state.SosActual}%";
                    anterior = $"{state.SosAnterior}%";
                    dir      = state.UltimoSosDir;
                    break;
                default:
                    valor    = "";
                    anterior = "";
                    dir      = DeltaDir.Igual;
                    break;
            }

            string flecha = ConfiguracionCatalogo.FlechaDelta(dir);
            texto.richText = true;
            texto.text = string.Format(plantilla, nombre, valor, flecha, anterior);
        }
    }
}
