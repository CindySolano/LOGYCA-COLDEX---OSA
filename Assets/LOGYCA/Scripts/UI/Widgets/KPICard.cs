using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card de un KPI individual (.md sección 6.1).
    ///
    /// Iconografía:
    ///   - Sube + favorable     → flecha arriba derecha · verde
    ///   - Sube + desfavorable  → flecha arriba derecha · rojo
    ///   - Baja + favorable     → flecha abajo derecha · verde
    ///   - Baja + desfavorable  → flecha abajo derecha · rojo
    ///   - Igual / No aplica    → raya horizontal · gris
    /// </summary>
    public class KPICard : MonoBehaviour
    {
        [SerializeField] private Image iconoFlecha;
        [SerializeField] private TMP_Text textoNombre;
        [SerializeField] private TMP_Text textoValor;

        [Header("Sprites de flecha")]
        [SerializeField] private Sprite spriteArribaFavorable;
        [SerializeField] private Sprite spriteArribaDesfavorable;
        [SerializeField] private Sprite spriteAbajoFavorable;
        [SerializeField] private Sprite spriteAbajoDesfavorable;
        [SerializeField] private Sprite spriteRaya;

        public void Configurar(KPIData kpi)
        {
            if (kpi == null) return;
            if (textoNombre != null) textoNombre.text = ConfiguracionCatalogo.NombreLegible(kpi.tipo);
            if (textoValor  != null) textoValor.text  = kpi.valorDescriptivo;

            bool subirEsBueno = ConfiguracionCatalogo.EsFavorableSubir(kpi.tipo);
            Sprite sprite = ElegirSprite(kpi.direccion, subirEsBueno);
            if (iconoFlecha != null && sprite != null) iconoFlecha.sprite = sprite;
        }

        private Sprite ElegirSprite(Direccion dir, bool subirEsBueno) => dir switch
        {
            Direccion.Sube => subirEsBueno ? spriteArribaFavorable : spriteArribaDesfavorable,
            Direccion.Baja => subirEsBueno ? spriteAbajoDesfavorable : spriteAbajoFavorable,
            _ => spriteRaya
        };
    }
}
