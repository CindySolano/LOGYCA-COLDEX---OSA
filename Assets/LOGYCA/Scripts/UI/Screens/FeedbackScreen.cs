using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Frame 04 (slide 9). La cámara sigue en el hotspot. El estado visual de
    /// la góndola lo cambia GondolaState en el mundo 3D. Aquí solo mostramos:
    ///   - VeredictoCard con título de opción + porqué + 3 mini-deltas
    ///   - Botones "Volver al mapa" y "Probar otra decisión"
    /// </summary>
    public class FeedbackScreen : ScreenController
    {
        [SerializeField] private VeredictoCard veredictoCard;
        [SerializeField] private GameObject botonVolver;
        [SerializeField] private GameObject botonProbarOtra;

        public void Mostrar(EstacionData estacion, OpcionData opcion)
        {
            Show();
            if (opcion == null) return;

            veredictoCard?.Configurar(opcion);

            if (botonVolver != null)     botonVolver.SetActive(true);
            if (botonProbarOtra != null) botonProbarOtra.SetActive(true);
        }

        public void OnVolverPressed()     => GameManager.Instance?.VolverAlMapa();
        public void OnProbarOtraPressed() => GameManager.Instance?.ProbarOtraDecision();
    }
}
