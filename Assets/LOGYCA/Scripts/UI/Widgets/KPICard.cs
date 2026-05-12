using TMPro;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card de un KPI individual.
    /// Versión reducida para esta entrega: sin flechas/iconos direccionales.
    /// El script solo escribe el nombre del KPI y el valor descriptivo.
    /// El estilo visual (color, ícono fijo, etc.) se define en el prefab.
    /// </summary>
    public class KPICard : MonoBehaviour
    {
        [SerializeField] private TMP_Text textoNombre;
        [SerializeField] private TMP_Text textoValor;

        public void Configurar(KPIData kpi)
        {
            if (kpi == null) return;
            if (textoNombre != null) textoNombre.text = ConfiguracionCatalogo.NombreLegible(kpi.tipo);
            if (textoValor  != null) textoValor.text  = kpi.valorDescriptivo;
        }
    }
}
