using TMPro;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card "DATO + pregunta" del frame 03. Sprite: card_dato_bg.png para el
    /// chip "DATO" y card_pregunta_contenedor.png para el contenedor general.
    /// </summary>
    public class DatoCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text textoDato;
        [SerializeField] private TMP_Text textoPregunta;

        public void Configurar(EstacionData estacion)
        {
            if (estacion == null) return;
            if (textoDato != null) textoDato.text = estacion.contexto;
            if (textoPregunta != null) textoPregunta.text = estacion.pregunta;
        }
    }
}
