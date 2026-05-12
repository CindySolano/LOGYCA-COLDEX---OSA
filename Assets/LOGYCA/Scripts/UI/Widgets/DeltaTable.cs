using System.Collections.Generic;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Instancia KPICards en dos contenedores: "Impacto operativo" (6 KPIs) e
    /// "Impacto en el bolsillo" (2 KPIs). El KPI de SatisfaccionShopper se
    /// envía a ShopperCard, no a este componente.
    /// </summary>
    public class DeltaTable : MonoBehaviour
    {
        [SerializeField] private Transform contenedorOperativos;
        [SerializeField] private Transform contenedorBolsillo;
        [SerializeField] private GameObject prefabKPICard;

        public void Renderizar(IEnumerable<KPIData> kpis)
        {
            Limpiar(contenedorOperativos);
            Limpiar(contenedorBolsillo);
            if (prefabKPICard == null) return;

            foreach (var kpi in kpis)
            {
                var cat = ConfiguracionCatalogo.Categoria(kpi.tipo);
                if (cat == CategoriaKPI.Cliente) continue;

                Transform parent = cat == CategoriaKPI.Bolsillo ? contenedorBolsillo : contenedorOperativos;
                if (parent == null) continue;

                var card = Instantiate(prefabKPICard, parent);
                var kpiCard = card.GetComponent<KPICard>();
                if (kpiCard != null) kpiCard.Configurar(kpi);
            }
        }

        private static void Limpiar(Transform t)
        {
            if (t == null) return;
            for (int i = t.childCount - 1; i >= 0; i--) Destroy(t.GetChild(i).gameObject);
        }
    }
}
