using UnityEngine;

namespace LOGYCA.OSA.Core
{
    /// <summary>
    /// Base liviana para cada pantalla / canvas. Centraliza Show/Hide y notifica
    /// al GameManager cuando se completa una transición.
    /// </summary>
    public abstract class ScreenController : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        public bool IsVisible => canvasGroup != null && canvasGroup.alpha > 0.01f;

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
    }
}
