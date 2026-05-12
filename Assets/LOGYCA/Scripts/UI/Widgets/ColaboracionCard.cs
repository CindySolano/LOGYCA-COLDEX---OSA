using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Card del bloque "Nivel de colaboración" (.md sección 6.3).
    /// Versión reducida para esta entrega:
    ///   - Texto del badge ("2 de 4")
    ///   - Texto del nombre del nivel ("Colaboración básica")
    ///   - Lista de 4 segmentos (Images): se enable/disable según el nivel
    ///     (sin sprite swap — la apariencia visual la define cada Image en el prefab)
    /// </summary>
    public class ColaboracionCard : MonoBehaviour
    {
        [SerializeField] private List<Image> segmentos = new List<Image>(4);
        [SerializeField] private TMP_Text textoBadgeNivel;
        [SerializeField] private TMP_Text textoNombre;

        public void Configurar(int nivel)
        {
            nivel = Mathf.Clamp(nivel, 1, 4);
            var (nombre, _) = ConfiguracionCatalogo.NivelColaboracion(nivel);

            if (textoBadgeNivel != null) textoBadgeNivel.text = $"{nivel} de 4";
            if (textoNombre     != null) textoNombre.text     = nombre;

            // Solo enable/disable de la Image: el visual de "activo" vs
            // "inactivo" se resuelve en el prefab (ej: dos circles overlapped,
            // o un Image que se oculta cuando i >= nivel).
            for (int i = 0; i < segmentos.Count; i++)
                if (segmentos[i] != null)
                    segmentos[i].enabled = (i < nivel);
        }
    }
}
