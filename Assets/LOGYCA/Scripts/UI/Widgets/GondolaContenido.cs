using UnityEngine;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Maneja el GameObject "objeto contenido" de cada góndola: arranca apagado
    /// (durante la pregunta) y se enciende SIEMPRE al pasar al feedback,
    /// sin importar la opción elegida — sólo cambian los KPIs y el texto.
    /// </summary>
    public class GondolaContenido : MonoBehaviour
    {
        [SerializeField] private GameObject objetoContenido;

        public void Apagar()
        {
            if (objetoContenido != null) objetoContenido.SetActive(false);
        }

        public void Encender()
        {
            if (objetoContenido != null) objetoContenido.SetActive(true);
        }
    }
}
