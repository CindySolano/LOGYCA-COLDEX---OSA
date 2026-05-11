using System.Collections.Generic;
using TMPro;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Barra superior persistente. Contenedor de 3 HUDIndicator (OSA, INV, SOS)
    /// + label "HUD" / "Actualizado" a la izquierda. La actualiza el GameManager
    /// pasándole un HUDState ya mutado.
    /// </summary>
    public class HUDBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text labelHud;          // "HUD"
        [SerializeField] private TMP_Text labelEstado;        // "Siempre visible" → "Actualizado"
        [SerializeField] private List<HUDIndicator> indicadores = new List<HUDIndicator>(3);

        public void Renderizar(HUDState state)
        {
            if (labelHud != null)    labelHud.text = "HUD";
            if (labelEstado != null) labelEstado.text = state != null && state.Actualizado ? "Actualizado" : "Siempre visible";

            foreach (var ind in indicadores)
                if (ind != null) ind.RenderizarDesde(state);
        }
    }
}
