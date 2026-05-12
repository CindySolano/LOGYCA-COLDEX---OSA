using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Layout rico del .md sección 7.5. No hay distinción visual entre opción
    /// "acertada" o "desacertada": el resultado siempre se muestra igual, lo
    /// único que cambia son los KPIs y el texto de feedback según la opción.
    ///
    /// Bloques:
    ///   1. Chip / cabecera (texto y sprite fijos, editables desde Inspector)
    ///   2. Título de la opción elegida
    ///   3. Impacto operativo  → DeltaTable con 6 KPIs
    ///   4. Impacto en el bolsillo → DeltaTable con 2 KPIs
    ///   5. Impacto en el cliente  → ShopperCard (cara feliz/neutra/triste)
    ///   6. Tipo de relación con el proveedor → ColaboracionCard
    ///   7. Lo que pasó → bloques cadena | proveedor + veredicto destacado
    ///   8. Único botón "Continuar"
    /// </summary>
    public class FeedbackScreen : ScreenController
    {
        [Header("Cabecera (chip + título)")]
        [SerializeField] private Image chipFondo;          // sprite único, sin distinción
        [SerializeField] private TMP_Text chipTexto;        // texto fijo, editable abajo
        [SerializeField] private TMP_Text textoTituloOpcion;

        [Tooltip("Texto fijo del chip. Mismo para todas las opciones.")]
        [SerializeField] private string textoChip = "RESULTADO";

        [Header("Impactos numéricos")]
        [SerializeField] private DeltaTable deltaTable;
        [SerializeField] private ShopperCard shopperCard;
        [SerializeField] private ColaboracionCard colaboracionCard;

        [Header("Lo que pasó")]
        [SerializeField] private TMP_Text textoFeedbackCadena;
        [SerializeField] private TMP_Text textoFeedbackProveedor;
        [SerializeField] private TMP_Text textoVeredicto;

        [Header("Botón único")]
        [SerializeField] private Button botonContinuar;

        public void Mostrar(EstacionData estacion, OpcionData opcion)
        {
            Show();
            if (opcion == null) return;

            // 1. Cabecera
            if (chipTexto != null) chipTexto.text = textoChip;
            if (textoTituloOpcion != null) textoTituloOpcion.text = $"\"{opcion.titulo}\"";

            // 2-3. KPIs operativos + bolsillo (6 + 2 cards)
            deltaTable?.Renderizar(opcion.kpis);

            // 4. Shopper card
            foreach (var kpi in opcion.kpis)
                if (kpi.tipo == TipoKPI.SatisfaccionShopper)
                    shopperCard?.Configurar(kpi);

            // 5. Colaboración
            colaboracionCard?.Configurar(opcion.nivelColaboracion);

            // 6. Narrativa
            if (textoFeedbackCadena    != null) textoFeedbackCadena.text    = opcion.feedbackCadena;
            if (textoFeedbackProveedor != null) textoFeedbackProveedor.text = opcion.feedbackProveedor;
            if (textoVeredicto         != null) textoVeredicto.text         = opcion.veredicto;

            // 7. Botón
            if (botonContinuar != null)
            {
                botonContinuar.onClick.RemoveAllListeners();
                botonContinuar.onClick.AddListener(() => GameManager.Instance?.Continuar());
            }
        }
    }
}
