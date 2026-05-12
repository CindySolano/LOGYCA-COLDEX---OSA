using TMPro;
using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Panel transitorio durante el movimiento de cámara.
    /// MostrarTransicion solo configura el texto — el GameObject lo activa el
    /// GameManager vía FadeIn() de ScreenController.
    /// </summary>
    public class MapScreen : ScreenController
    {
        [Header("Referencia al texto")]
        [SerializeField] private TMP_Text textoRonda;

        [Header("Plantilla del texto")]
        [Tooltip("{0}=ronda actual · {1}=total · {2}=nombre largo de la estación")]
        [TextArea(2, 4)]
        [SerializeField] private string plantilla = "<b>Ronda {0} de {1}</b> | Acercando cámara al hotspot {2}";

        public void MostrarTransicion(int rondaActual, int total, EstacionData destino)
        {
            if (textoRonda == null) return;
            string nombreDestino = destino != null ? ObtenerNombre(destino) : "";
            textoRonda.richText = true;
            textoRonda.text = string.Format(plantilla, rondaActual, total, nombreDestino);
        }

        private static string ObtenerNombre(EstacionData e)
        {
            return !string.IsNullOrEmpty(e.nombreLargo) ? e.nombreLargo : e.nombre;
        }
    }
}
