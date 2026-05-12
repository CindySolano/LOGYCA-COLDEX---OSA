using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card del bloque "Tipo de relación con el proveedor" (.md sección 6.3).
    /// Barra de 4 segmentos horizontales + nombre del nivel + descripción.
    /// </summary>
    public class ColaboracionCard : MonoBehaviour
    {
        [SerializeField] private List<Image> segmentos = new List<Image>(4);
        [SerializeField] private TMP_Text textoBadgeNivel;
        [SerializeField] private TMP_Text textoNombre;
        [SerializeField] private TMP_Text textoDescripcion;

        [SerializeField] private Sprite spriteSegmentoActivo;
        [SerializeField] private Sprite spriteSegmentoInactivo;

        public void Configurar(int nivel)
        {
            nivel = Mathf.Clamp(nivel, 1, 4);
            var (nombre, desc) = ConfiguracionCatalogo.NivelColaboracion(nivel);

            if (textoBadgeNivel  != null) textoBadgeNivel.text  = $"{nivel} de 4";
            if (textoNombre      != null) textoNombre.text      = nombre;
            if (textoDescripcion != null) textoDescripcion.text = desc;

            for (int i = 0; i < segmentos.Count; i++)
            {
                if (segmentos[i] == null) continue;
                Sprite s = (i < nivel) ? spriteSegmentoActivo : spriteSegmentoInactivo;
                if (s != null) segmentos[i].sprite = s;
            }
        }
    }
}
