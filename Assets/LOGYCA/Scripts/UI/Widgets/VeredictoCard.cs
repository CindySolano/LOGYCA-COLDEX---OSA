using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card grande del frame 04 (slide 9):
    ///   - Chip "DECISIÓN ACERTADA" o "DECISIÓN DESACERTADA"
    ///   - Título de la opción elegida (lo que el usuario tocó)
    ///   - Bloque "PORQUÉ" con el texto narrativo de la opción
    ///   - 3 mini-deltas (OSA / INV / SOS) en la parte baja
    ///
    /// Los colores ya vienen en los sprites (card_feedback_acertado/desacertado,
    /// icono_check_teal, icono_x_rojo). El script solo cambia el sprite y el
    /// texto — no aplica tinting.
    /// </summary>
    public class VeredictoCard : MonoBehaviour
    {
        [Header("Chip / cabecera")]
        [SerializeField] private Image chipFondo;          // icono_check_teal o icono_x_rojo
        [SerializeField] private TMP_Text chipTexto;        // "DECISIÓN ACERTADA"/"DESACERTADA"
        [SerializeField] private Image fondoCard;           // card_feedback_acertado o _desacertado

        [Header("Sprites por estado (color ya horneado)")]
        [SerializeField] private Sprite chipAcertada;
        [SerializeField] private Sprite chipDesacertada;
        [SerializeField] private Sprite cardAcertada;
        [SerializeField] private Sprite cardDesacertada;

        [Header("Cuerpo")]
        [SerializeField] private TMP_Text textoTituloOpcion;  // lo que el usuario eligió (op.titulo)
        [SerializeField] private TMP_Text textoPorque;        // op.porque

        [Header("Mini deltas (OSA / INV / SOS)")]
        [SerializeField] private TMP_Text deltaOsa;
        [SerializeField] private TMP_Text deltaInv;
        [SerializeField] private TMP_Text deltaSos;

        public void Configurar(OpcionData op)
        {
            if (op == null) return;

            // Cabecera: sprite ya trae el color
            if (chipTexto != null) chipTexto.text = op.esAcertada ? "DECISIÓN ACERTADA" : "DECISIÓN DESACERTADA";
            if (chipFondo != null) chipFondo.sprite = op.esAcertada ? chipAcertada : chipDesacertada;
            if (fondoCard != null) fondoCard.sprite = op.esAcertada ? cardAcertada  : cardDesacertada;

            // Título de opción: lo que el usuario tocó
            if (textoTituloOpcion != null) textoTituloOpcion.text = $"\"{op.titulo}\"";
            if (textoPorque       != null) textoPorque.text       = op.porque;

            // Mini-deltas: solo formato, sin tinting (el TMP del prefab define los colores base)
            if (deltaOsa != null) deltaOsa.text = ConfiguracionCatalogo.FormatoDeltaPct(op.deltaOsaPct);
            if (deltaInv != null) deltaInv.text = op.deltaInvDescriptivo;
            if (deltaSos != null) deltaSos.text = ConfiguracionCatalogo.FormatoDeltaPct(op.deltaSosPct);
        }
    }
}
