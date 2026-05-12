using System.Collections.Generic;
using TMPro;
using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Cierre del juego (slide 11). Muestra el HUD final + perfil de decisor +
    /// reflexión guiada (texto fijo) + botón reiniciar.
    /// </summary>
    public class SummaryScreen : ScreenController
    {
        [SerializeField] private HUDBar hudBarFinal;
        [SerializeField] private TMP_Text textoPerfil;
        [SerializeField] private TMP_Text textoPromedio;
        [SerializeField] private TMP_Text textoReflexion;

        public void Mostrar(List<int> niveles, HUDState estadoFinal)
        {
            hudBarFinal?.Renderizar(estadoFinal);

            if (niveles != null && niveles.Count > 0)
            {
                float promedio = 0f;
                foreach (var n in niveles) promedio += n;
                promedio /= niveles.Count;

                if (textoPerfil   != null) textoPerfil.text   = ConfiguracionCatalogo.PerfilDecisor(promedio).ToUpper();
                if (textoPromedio != null) textoPromedio.text = $"Promedio nivel de colaboración: {promedio:F1}";
            }

            if (textoReflexion != null)
                textoReflexion.text = "No se trata del indicador correcto, sino de saber qué se cede al priorizar otro.";
        }

        public void OnReiniciarPressed() => GameManager.Instance?.ReiniciarExperiencia();
    }
}
