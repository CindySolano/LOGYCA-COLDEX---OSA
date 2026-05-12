using UnityEngine;
using UnityEngine.EventSystems;
using LOGYCA.OSA.Core;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Hotspot 3D anclado al mundo. Construido con un SpriteRenderer (3D-space)
    /// y opcionalmente un ParticleSystem hijo que actúa como afordance de clic.
    ///
    /// Estados:
    ///   - Activo  : alpha 100% del sprite + partículas ON + clickeable
    ///   - Dormido : alpha bajo (default 0.25) + partículas OFF + NO clickeable
    ///   - Visitado: alpha medio (default 0.5)  + partículas OFF + NO clickeable
    ///
    /// Requiere un Collider para que IPointerClickHandler funcione vía Physics
    /// Raycaster en la Main Camera.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Hotspot3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum Estado { Dormido, Activo, Visitado }

        [Header("Datos")]
        public EstacionData estacion;

        [Header("Componentes (auto-resolved si están vacíos)")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ParticleSystem particulas;

        [Header("Alpha por estado (sobre el color del SpriteRenderer)")]
        [Range(0f, 1f)] [SerializeField] private float alphaActivo  = 1f;
        [Range(0f, 1f)] [SerializeField] private float alphaDormido = 0.25f;
        [Range(0f, 1f)] [SerializeField] private float alphaVisitado = 0.5f;

        [Header("Hover (solo cuando está Activo)")]
        [SerializeField] private float escalaIdle   = 1f;
        [SerializeField] private float escalaHover  = 1.15f;
        [SerializeField] private float velocidadEscalado = 8f;

        public Estado EstadoActual { get; private set; } = Estado.Dormido;
        private bool hover;
        private Vector3 baseScale;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (particulas == null)     particulas     = GetComponentInChildren<ParticleSystem>();
            baseScale = transform.localScale;
            SetEstado(Estado.Dormido);
        }

        public void SetEstado(Estado nuevo)
        {
            EstadoActual = nuevo;
            Debug.Log($"[Hotspot {name}] estado → {nuevo} (estacion={estacion?.id ?? "null"})", this);

            // Alpha
            if (spriteRenderer != null)
            {
                var c = spriteRenderer.color;
                c.a = nuevo switch
                {
                    Estado.Activo   => alphaActivo,
                    Estado.Visitado => alphaVisitado,
                    _               => alphaDormido
                };
                spriteRenderer.color = c;
            }

            // Partículas: solo cuando está Activo
            if (particulas != null)
            {
                if (nuevo == Estado.Activo)
                {
                    if (!particulas.isPlaying) particulas.Play(true);
                }
                else
                {
                    if (particulas.isPlaying) particulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }

        public bool EsClickeable => EstadoActual == Estado.Activo && estacion != null;

        /// <summary>Apaga el GameObject completo — se usa durante Situation/Feedback para sacarlo de cámara.</summary>
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        [ContextMenu("Test → Activo")]
        private void TestActivo() => SetEstado(Estado.Activo);
        [ContextMenu("Test → Dormido")]
        private void TestDormido() => SetEstado(Estado.Dormido);
        [ContextMenu("Test → Visitado")]
        private void TestVisitado() => SetEstado(Estado.Visitado);

        public void OnPointerEnter(PointerEventData _)
        {
            Debug.Log($"[Hotspot {name}] OnPointerEnter · EsClickeable={EsClickeable} · Estado={EstadoActual}", this);
            hover = EsClickeable;
        }

        public void OnPointerExit(PointerEventData _)
        {
            Debug.Log($"[Hotspot {name}] OnPointerExit", this);
            hover = false;
        }

        public void OnPointerClick(PointerEventData _)
        {
            Debug.Log($"[Hotspot {name}] OnPointerClick · EsClickeable={EsClickeable} · Estado={EstadoActual} · GM={(GameManager.Instance != null ? "ok" : "NULL")}", this);
            if (!EsClickeable)
            {
                Debug.LogWarning($"[Hotspot {name}] click ignorado: no es clickeable (estado={EstadoActual})", this);
                return;
            }
            if (GameManager.Instance == null)
            {
                Debug.LogError($"[Hotspot {name}] click ignorado: GameManager.Instance es null", this);
                return;
            }
            GameManager.Instance.SeleccionarEstacion(estacion);
        }

        private void LateUpdate()
        {
            // Hover scale solo en Activo
            if (EstadoActual == Estado.Activo)
            {
                float target = hover ? escalaHover : escalaIdle;
                transform.localScale = Vector3.Lerp(transform.localScale,
                    baseScale * target, Time.deltaTime * velocidadEscalado);
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale,
                    baseScale, Time.deltaTime * velocidadEscalado);
            }
        }
    }
}
