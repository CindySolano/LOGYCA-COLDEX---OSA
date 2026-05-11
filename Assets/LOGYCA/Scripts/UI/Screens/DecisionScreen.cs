using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Sub-componente de Panel_Situation. NO es un panel propio: vive dentro de
    /// DatoCardGroup y sólo expone el bloque de 3 OptionButton.
    ///
    /// El SituationScreen lo apaga/enciende junto con la card.
    /// </summary>
    public class DecisionScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text textoPregunta;
        [SerializeField] private List<OptionButton> botones = new List<OptionButton>(3);
        [SerializeField] private CanvasGroup canvasGroup;

        public void Mostrar(EstacionData estacion)
        {
            gameObject.SetActive(true);
            if (canvasGroup != null) { canvasGroup.alpha = 1f; canvasGroup.interactable = true; canvasGroup.blocksRaycasts = true; }
            if (estacion == null) return;

            if (textoPregunta != null) textoPregunta.text = estacion.pregunta;

            for (int i = 0; i < botones.Count; i++)
            {
                if (botones[i] == null) continue;
                if (i < estacion.opciones.Count)
                {
                    botones[i].gameObject.SetActive(true);
                    var opcion = estacion.opciones[i];
                    botones[i].Configurar(opcion, () => GameManager.Instance?.SeleccionarOpcion(opcion));
                }
                else
                {
                    botones[i].gameObject.SetActive(false);
                }
            }
        }

        public void Ocultar()
        {
            if (canvasGroup != null) { canvasGroup.alpha = 0f; canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false; }
            gameObject.SetActive(false);
        }
    }
}
