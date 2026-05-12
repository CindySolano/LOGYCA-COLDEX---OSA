using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card grande del bloque "Impacto en el cliente" (.md sección 6.2).
    /// Cara feliz / neutra / triste según la dirección de SatisfaccionShopper.
    /// El color del fondo viene del sprite — el script solo swappea sprites.
    /// </summary>
    public class ShopperCard : MonoBehaviour
    {
        [SerializeField] private Image fondo;
        [SerializeField] private Image cara;
        [SerializeField] private TMP_Text textoEstado;

        [Header("Sprites por estado")]
        [SerializeField] private Sprite fondoSube;
        [SerializeField] private Sprite fondoIgual;
        [SerializeField] private Sprite fondoBaja;
        [SerializeField] private Sprite caraFeliz;
        [SerializeField] private Sprite caraNeutra;
        [SerializeField] private Sprite caraTriste;

        public void Configurar(KPIData kpi)
        {
            if (kpi == null || kpi.tipo != TipoKPI.SatisfaccionShopper) return;

            switch (kpi.direccion)
            {
                case Direccion.Sube:
                    SetSprite(fondoSube, caraFeliz);
                    if (textoEstado != null) textoEstado.text = "SUBE";
                    break;
                case Direccion.Baja:
                    SetSprite(fondoBaja, caraTriste);
                    if (textoEstado != null) textoEstado.text = "BAJA";
                    break;
                default:
                    SetSprite(fondoIgual, caraNeutra);
                    if (textoEstado != null) textoEstado.text = "IGUAL";
                    break;
            }
        }

        private void SetSprite(Sprite f, Sprite c)
        {
            if (fondo != null && f != null) fondo.sprite = f;
            if (cara  != null && c != null) cara.sprite  = c;
        }
    }
}
