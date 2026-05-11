using TMPro;
using UnityEngine;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Etiqueta tipo "pill" sobre la escena 3D. Cubre los dos casos de la
    /// referencia: PRÓXIMA (fondo naranja) y MERCADERISTA EN HOTSPOT (teal).
    /// </summary>
    public class HotspotLabel : MonoBehaviour
    {
        public enum Tipo { Proxima, Mercaderista }

        [SerializeField] private Tipo tipo = Tipo.Proxima;
        [SerializeField] private TMP_Text texto;
        [SerializeField] private UnityEngine.UI.Image fondo;
        [SerializeField] private Sprite fondoProxima;     // label_proxima_naranja_pill.png
        [SerializeField] private Sprite fondoMercaderista; // label_hotspot_teal_pill.png

        public void Configurar(Tipo tipo, string textoLabel)
        {
            this.tipo = tipo;
            if (texto != null) texto.text = textoLabel;
            if (fondo != null) fondo.sprite = tipo == Tipo.Proxima ? fondoProxima : fondoMercaderista;
        }
    }
}
