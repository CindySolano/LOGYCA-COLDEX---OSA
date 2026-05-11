using System.Collections;
using UnityEngine;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Controla la mercaderista del frame 02-04 de las slides:
    ///   - Aparece (Show) frente a la góndola al llegar la cámara.
    ///   - Hace una animación (TriggerAccion) tras la decisión.
    ///   - Desaparece al volver al mapa.
    ///
    /// Setup: GameObject con un Animator que tenga triggers nombrados como los
    /// strings que vengan en OpcionData.animacionMercaderista (p.ej. "Rotar",
    /// "PedirMenos", "Idle").
    /// </summary>
    public class MercaderistaController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string triggerIdle = "Idle";
        [SerializeField] private float fadeAparicionSeconds = 0.3f;

        private CanvasGroup cg; // opcional para fade de un sprite si fuera 2D

        public void Mostrar()
        {
            gameObject.SetActive(true);
            if (animator != null && !string.IsNullOrEmpty(triggerIdle))
                animator.SetTrigger(triggerIdle);
        }

        public void Ocultar() => gameObject.SetActive(false);

        public IEnumerator EjecutarAccion(string trigger, float duracion)
        {
            if (animator != null && !string.IsNullOrEmpty(trigger))
                animator.SetTrigger(trigger);
            yield return new WaitForSeconds(duracion);
        }
    }
}
