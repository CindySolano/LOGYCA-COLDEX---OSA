using System.Collections;
using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;
using LOGYCA.OSA.Utils;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Pasos 02 + 03 del slide 4 fusionados. Mientras la cámara hace dolly suave,
    /// mostramos el label "MERCADERISTA EN HOTSPOT". Al terminar el dolly,
    /// fade-in de la DatoCard + DecisionScreen (3 botones).
    ///
    /// Para el camino "Probar otra decisión" desde Feedback, usar MostrarSinFade().
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
            Configurar(estacion);

            if (datoCardGroup != null) datoCardGroup.alpha = 0f;
            decisionScreenInline?.Ocultar();

            StopAllCoroutines();
            StartCoroutine(SecuenciaDollyYFade(estacion));
        }

        public void MostrarSinFade(EstacionData estacion)
        {
            Show();
            if (estacion == null) return;
            Configurar(estacion);

            if (datoCardGroup != null) { datoCardGroup.alpha = 1f; datoCardGroup.interactable = true; datoCardGroup.blocksRaycasts = true; }
            decisionScreenInline?.Mostrar(estacion);
        }

        private void Configurar(EstacionData estacion)
        {
            labelMercaderista?.Configurar(HotspotLabel.Tipo.Mercaderista, "MERCADERISTA EN HOTSPOT");
            datoCard?.Configurar(estacion);
        }

        private IEnumerator SecuenciaDollyYFade(EstacionData estacion)
        {
            float dolly = GameManager.Instance?.Config?.dollyInSeconds ?? 1.6f;
            float fade  = GameManager.Instance?.Config?.fadeInSeconds  ?? 0.25f;

            yield return new WaitForSeconds(dolly);
            yield return FadeAnimator.FadeIn(datoCardGroup, fade);

            decisionScreenInline?.Mostrar(estacion);
        }
    }
}
