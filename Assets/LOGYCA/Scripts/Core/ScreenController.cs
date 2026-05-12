using System.Collections;
using UnityEngine;

namespace LOGYCA.OSA.Core
{
    /// <summary>
    /// Base de cada panel. Expone:
    ///   - Show()/Hide()           → cambio instantáneo (init, reset)
    ///   - FadeIn()/FadeOut()      → coroutines suaves con SmoothStep
    ///                                que el GameManager aguarda con yield return
    /// </summary>
    public abstract class ScreenController : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        [Header("Fade")]
        [Tooltip("Duración del fade-in al aparecer el panel.")]
        [Range(0.05f, 2f)] [SerializeField] private float fadeInSeconds = 0.25f;

        [Tooltip("Duración del fade-out al desaparecer el panel.")]
        [Range(0.05f, 2f)] [SerializeField] private float fadeOutSeconds = 0.25f;

        public bool IsVisible =>
            gameObject.activeSelf && canvasGroup != null && canvasGroup.alpha > 0.01f;

        // ── instantáneo ──

        public virtual void Show()
        {
            gameObject.SetActive(true);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void Hide()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            gameObject.SetActive(false);
        }

        // ── animado ──

        public IEnumerator FadeIn()
        {
            gameObject.SetActive(true);
            if (canvasGroup == null) yield break;

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float t = 0f;
            float dur = Mathf.Max(0.01f, fadeInSeconds);
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, t / dur);
                yield return null;
            }
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public IEnumerator FadeOut()
        {
            if (canvasGroup == null || !gameObject.activeSelf)
            {
                if (gameObject.activeSelf) gameObject.SetActive(false);
                yield break;
            }
            if (canvasGroup.alpha < 0.01f)
            {
                gameObject.SetActive(false);
                yield break;
            }

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float startAlpha = canvasGroup.alpha;
            float t = 0f;
            float dur = Mathf.Max(0.01f, fadeOutSeconds);
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.SmoothStep(startAlpha, 0f, t / dur);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    }
}
