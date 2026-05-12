using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Pasos 02 + 03 del slide 4 fusionados. Solo configura los datos —
    /// el fade-in del panel completo lo maneja GameManager vía FadeIn().
    /// </summary>
    public class SituationScreen : ScreenController
    {
        [SerializeField] private HotspotLabel labelMercaderista;
        [SerializeField] private DatoCard datoCard;
        [SerializeField] private DecisionScreen decisionScreenInline;

        public void Mostrar(EstacionData estacion)
        {
            if (estacion == null) return;
            labelMercaderista?.Configurar(HotspotLabel.Tipo.Mercaderista, "MERCADERISTA EN HOTSPOT");
            datoCard?.Configurar(estacion);
            decisionScreenInline?.Mostrar(estacion);
        }
    }
}
