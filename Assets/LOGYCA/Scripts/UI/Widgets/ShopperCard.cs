using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card del bloque "Impacto en el cliente" (.md sección 6.2).
    /// Versión reducida para esta entrega: sin cara emoji, solo cambia el
    /// fondo y el texto de estado (SUBE / IGUAL / BAJA).
    /// </summary>
    public class ShopperCard : MonoBehaviour
    {
        [SerializeField] private Image fondo;
        [SerializeField] private TMP_Text textoEstado;

        [Header("Sprites de fondo por estado")]
        [SerializeField] private Sprite fondoSube;
        [SerializeField] private Sprite fondoIgual;
        [SerializeField] private Sprite fondoBaja;

        public void Configurar(KPIData kpi)
        {
            if (kpi == null || kpi.tipo != TipoKPI.SatisfaccionShopper) return;

            switch (kpi.direccion)
            {
                case Direccion.Sube:
                    SetFondo(fondoSube);
                    if (textoEstado != null) textoEstado.text = "SUBE";
                    break;
                case Direccion.Baja:
                    SetFondo(fondoBaja);
                    if (textoEstado != null) textoEstado.text = "BAJA";
                    break;
                default:
                    SetFondo(fondoIgual);
                    if (textoEstado != null) textoEstado.text = "IGUAL";
                    break;
            }
        }

        private void SetFondo(Sprite s)
        {
            if (fondo != null && s != null) fondo.sprite = s;
        }
    }
}
