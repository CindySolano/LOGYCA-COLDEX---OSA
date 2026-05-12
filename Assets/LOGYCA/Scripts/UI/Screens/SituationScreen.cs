using System.Collections;
using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;
using LOGYCA.OSA.Utils;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Aparece DESPUÉS de que la cámara llegó al hotspot (el GameManager
    /// ya esperó el dolly). Hace fade-in de la DatoCard y prende la
    /// DecisionScreen inline con los 3 botones.
    /// </summary>
    public class SituationScreen : ScreenController
    {
        [SerializeField] private HotspotLabel labelMercaderista;
        [SerializeField] private DatoCard datoCard;
        [SerializeField] private CanvasGroup datoCardGroup;
        [SerializeField] private DecisionScreen decisionScreenInline;

        public void Mostrar(EstacionData estacion)
        {
            Show();
            if (estacion == null) return;

            labelMercaderista?.Configurar(HotspotLabel.Tipo.Mercaderista, "MERCADERISTA EN HOTSPOT");
            datoCard?.Configurar(estacion);

            if (datoCardGroup != null) datoCardGroup.alpha = 0f;
            decisionScreenInline?.Ocultar();

            StopAllCoroutines();
            StartCoroutine(FadeYBotones(estacion));
        }

        private IEnumerator FadeYBotones(EstacionData estacion)
        {
            float fade = GameManager.Instance?.Config?.fadeInSeconds ?? 0.25f;
            yield return FadeAnimator.FadeIn(datoCardGroup, fade);
            decisionScreenInline?.Mostrar(estacion);
        }
    }
}
