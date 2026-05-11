using System.Collections;
using UnityEngine;

namespace LOGYCA.OSA.Utils
{
    /// <summary>
    /// Helpers de fade-in/out para CanvasGroup. Útil para la transición FADE IN
    /// del frame 03 (DATO + pregunta).
    /// </summary>
    public class FadeAnimator : MonoBehaviour
    {
        public static IEnumerator FadeIn(CanvasGroup cg, float duration)
        {
            if (cg == null) yield break;
            cg.alpha = 0f;
            cg.gameObject.SetActive(true);
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Clamp01(t / duration);
                yield return null;
            }
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        public static IEnumerator FadeOut(CanvasGroup cg, float duration)
        {
            if (cg == null) yield break;
            float t = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                cg.alpha = 1f - Mathf.Clamp01(t / duration);
                yield return null;
            }
            cg.alpha = 0f;
            cg.gameObject.SetActive(false);
        }
    }
}
