using System.Collections.Generic;
using UnityEngine;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.UI.Widgets;

namespace LOGYCA.OSA.UI.Screens
{
    /// <summary>
    /// Frame 01 de la referencia (slide 8). El Canvas en sí casi no tiene
    /// elementos: la mayoría son hotspots 3D anclados al mundo. Este script
    /// recorre los hotspots de la escena y les asigna estado:
    ///   - Visitado  → check teal
    ///   - Próximo   → anillo pulsando + label PRÓXIMA (es el clickeable)
    ///   - Inactivo  → apagado
    /// </summary>
    public class MapScreen : ScreenController
    {
        [SerializeField] private List<Hotspot3D> hotspots = new List<Hotspot3D>();
        [SerializeField] private RondaCounter rondaCounter;

        public void Mostrar(HashSet<string> visitadas, int rondaCero)
        {
            Show();

            int total = hotspots.Count;
            int rondaActual = Mathf.Min(rondaCero + 1, Mathf.Max(1, total));
            rondaCounter?.Configurar(rondaActual, total, "acercando cámara al hotspot");

            // primer hotspot no visitado en orden de la lista = PROXIMO
            Hotspot3D proximo = null;
            foreach (var h in hotspots)
            {
                if (h == null || h.estacion == null) continue;
                bool ya = visitadas != null && visitadas.Contains(h.estacion.id);
                if (ya) h.SetEstado(Hotspot3D.Estado.Visitado);
                else if (proximo == null) proximo = h;
                else h.SetEstado(Hotspot3D.Estado.Inactivo);
            }
            proximo?.SetEstado(Hotspot3D.Estado.Proximo);
        }
    }
}
