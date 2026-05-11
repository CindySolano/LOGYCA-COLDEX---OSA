using UnityEngine;

namespace LOGYCA.OSA.Utils
{
    /// <summary>
    /// Pulso de escala/opacidad. Aplicable a hotspots y al texto "Toca para empezar".
    /// </summary>
    public class PulseAnimator : MonoBehaviour
    {
        [SerializeField] private float frecuenciaHz = 0.5f;
        [SerializeField] private float minScale = 0.9f;
        [SerializeField] private float maxScale = 1.1f;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float minAlpha = 0.6f;
        [SerializeField] private float maxAlpha = 1f;

        private Vector3 baseScale;

        private void Awake() => baseScale = transform.localScale;

        private void Update()
        {
            float t = (Mathf.Sin(Time.time * frecuenciaHz * Mathf.PI * 2f) + 1f) * 0.5f;
            float scale = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = baseScale * scale;
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
        }
    }
}
