using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using LOGYCA.OSA.Core;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Pantalla de espera. Cualquier toque la cierra y arranca el flujo.
    /// </summary>
    public class AttractScreen : ScreenController, IPointerClickHandler
    {
        [SerializeField] private TMP_Text textoTitulo;
        [SerializeField] private TMP_Text textoTocaParaEmpezar;

        private void Reset()
        {
            if (textoTitulo != null) textoTitulo.text = "MUEVE EL DATO,\nMUEVE LA GÓNDOLA";
            if (textoTocaParaEmpezar != null) textoTocaParaEmpezar.text = "Toca para empezar";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameManager.Instance?.EmpezarExperiencia();
        }
    }
}
