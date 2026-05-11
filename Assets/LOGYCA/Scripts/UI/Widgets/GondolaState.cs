using UnityEngine;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Una góndola tiene 3 estados visuales (slide 9):
    ///   - Neutral:    estado base antes de decidir
    ///   - Atendido:   resultado tras decisión acertada (frutería rotada)
    ///   - SinAtender: resultado tras decisión errada (frutería sin atender)
    ///
    /// Cada estado es un GameObject hijo. Sólo uno está activo a la vez.
    /// El ZoneSelector se enciende junto con el estado correspondiente.
    /// </summary>
    public class GondolaState : MonoBehaviour
    {
        [SerializeField] private GameObject estadoNeutral;
        [SerializeField] private GameObject estadoAtendido;
        [SerializeField] private GameObject estadoSinAtender;

        public void MostrarNeutral()    { Set(estadoNeutral,    estadoAtendido, estadoSinAtender); }
        public void MostrarAtendido()   { Set(estadoAtendido,   estadoNeutral,  estadoSinAtender); }
        public void MostrarSinAtender() { Set(estadoSinAtender, estadoNeutral,  estadoAtendido);   }

        private static void Set(GameObject visible, params GameObject[] ocultos)
        {
            if (visible != null) visible.SetActive(true);
            foreach (var go in ocultos)
                if (go != null) go.SetActive(false);
        }
    }
}
