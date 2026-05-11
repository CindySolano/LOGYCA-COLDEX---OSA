using UnityEngine;
using UnityEngine.EventSystems;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Hotspot anclado al mundo 3D. Tiene 3 estados visuales:
    ///   - INACTIVO   : el grupo todavía no toca este hotspot, anillo apagado
    ///   - PROXIMO    : este es el "go" para la siguiente decisión — anillo
    ///                  pulsando + label "PRÓXIMA" + escala mayor en hover
    ///   - VISITADO   : ya pasó la ronda, anillo apagado y check teal sobre él
    ///
    /// Sólo dispara la transición cuando está en estado PROXIMO (eso es lo que
    /// "da el go" a la cámara y a la mercaderista).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Hotspot3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum Estado { Inactivo, Proximo, Visitado }

        [Header("Datos")]
        public EstacionData estacion;

        [Header("Visuales")]
        [SerializeField] private GameObject anilloPulsante;     // hotspot_naranja.png en Quad
        [SerializeField] private GameObject labelProxima;        // pill PRÓXIMA
        [SerializeField] private GameObject iconoCheck;          // icono_check_teal
        [SerializeField] private Transform raizEscalado;         // se anima en hover

        [Header("Hover (afordance de clic)")]
        [SerializeField] private float escalaIdle = 1f;
        [SerializeField] private float escalaHover = 1.18f;
        [SerializeField] private float velocidadEscalado = 8f;

        [Header("Billboard")]
        [SerializeField] private Camera worldCamera;
        [SerializeField] private bool mantenerTamanoEnPantalla = true;
        [SerializeField] private float factorTamanoPantalla = 0.05f;

        public Estado EstadoActual { get; private set; } = Estado.Inactivo;
        private bool hover;

        private void Awake()
        {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
            if (worldCamera == null) worldCamera = Camera.main;
        }

        public void SetEstado(Estado nuevo)
        {
            EstadoActual = nuevo;
            if (anilloPulsante != null) anilloPulsante.SetActive(nuevo == Estado.Proximo);
            if (labelProxima  != null) labelProxima.SetActive(nuevo == Estado.Proximo);
            if (iconoCheck    != null) iconoCheck.SetActive(nuevo == Estado.Visitado);
        }

        public bool EsClickeable => EstadoActual == Estado.Proximo && estacion != null;

        public void OnPointerEnter(PointerEventData _) => hover = EsClickeable;
        public void OnPointerExit (PointerEventData _) => hover = false;

        public void OnPointerClick(PointerEventData _)
        {
            if (!EsClickeable || GameManager.Instance == null) return;
            GameManager.Instance.SeleccionarEstacion(estacion);
        }

        private void LateUpdate()
        {
            if (raizEscalado != null)
            {
                float target = (hover ? escalaHover : escalaIdle);
                raizEscalado.localScale = Vector3.Lerp(
                    raizEscalado.localScale, Vector3.one * target,
                    Time.deltaTime * velocidadEscalado);
            }

            if (worldCamera != null)
            {
                var dir = transform.position - worldCamera.transform.position;
                if (dir.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                if (mantenerTamanoEnPantalla)
                {
                    float dist = Vector3.Distance(transform.position, worldCamera.transform.position);
                    transform.localScale = Vector3.one * dist * factorTamanoPantalla;
                }
            }
        }
    }
}
