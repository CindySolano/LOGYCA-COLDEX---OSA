using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Versión reducida del FeedbackScreen para esta entrega:
    ///   1. Chip / cabecera con icono dinámico de la estación
    ///   2. Título de la opción elegida + descripción
    ///   3. Impactos numéricos (DeltaTable + ShopperCard + ColaboracionCard)
    ///   4. Botón único "Continuar"
    ///
    /// El bloque "Lo que pasó" (cadena/proveedor/veredicto) está fuera de
    /// este entregable.
    /// </summary>
    public class FeedbackScreen : ScreenController
    {
        [Header("Cabecera (chip + título)")]
        [SerializeField] private Image chipFondo;
        [SerializeField] private TMP_Text chipTexto;
        [SerializeField] private TMP_Text textoTituloOpcion;
        [SerializeField] private TMP_Text textoDescripcion;
        [SerializeField] private Image iconoEstacion;

        [Tooltip("Texto fijo del chip. Mismo para todas las opciones.")]
        [SerializeField] private string textoChip = "RESULTADO";

        [Tooltip("Texto que aparece debajo del título de la opción.")]
        [SerializeField] private string textoDecisionRegistrada = "Decisión registrada.";

        [Header("Impactos numéricos")]
        [SerializeField] private DeltaTable deltaTable;
        [SerializeField] private ShopperCard shopperCard;
        [SerializeField] private ColaboracionCard colaboracionCard;

        [Header("Botón único")]
        [SerializeField] private Button botonContinuar;

        public void Mostrar(EstacionData estacion, OpcionData opcion)
        {
            if (opcion == null) return;

            // 1. Cabecera
            if (chipTexto != null) chipTexto.text = textoChip;
            if (textoTituloOpcion != null) textoTituloOpcion.text = $"\"{opcion.titulo}\"";
            if (textoDescripcion != null) textoDescripcion.text = textoDecisionRegistrada;

            // Icono dinámico según la estación
            if (iconoEstacion != null && estacion != null && estacion.icono != null)
            {
                iconoEstacion.sprite = estacion.icono;
                iconoEstacion.enabled = true;
            }

            // 2. Impactos numéricos
            deltaTable?.Renderizar(opcion.kpis);

            foreach (var kpi in opcion.kpis)
                if (kpi.tipo == TipoKPI.SatisfaccionShopper)
                    shopperCard?.Configurar(kpi);

            colaboracionCard?.Configurar(opcion.nivelColaboracion);

            // 3. Botón
            if (botonContinuar != null)
            {
                botonContinuar.onClick.RemoveAllListeners();
                botonContinuar.onClick.AddListener(() => GameManager.Instance?.Continuar());
            }
        }
    }
}
