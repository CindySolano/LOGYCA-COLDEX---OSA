using TMPro;
using UnityEngine;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Pill inferior del frame 01.
    /// Formato: "<b>Ronda 2 de 5</b> | Acercando cámara al hotspot"
    /// Aprovecha el rich-text de TextMeshPro para el bold del prefijo.
    /// </summary>
    public class RondaCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text texto;

        public void Configurar(int rondaActual, int total, string detalle = null)
        {
            if (texto == null) return;
            string sufijo = string.IsNullOrEmpty(detalle) ? "" : $" | {detalle}";
            texto.richText = true;
            texto.text = $"<b>Ronda {rondaActual} de {total}</b>{sufijo}";
        }
    }
}
