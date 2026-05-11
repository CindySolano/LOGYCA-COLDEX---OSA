using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.CameraCtrl
{
    /// <summary>
    /// Secuenciador de cámara basado en Cinemachine 3. Garantiza el "movimiento
    /// super suave" exigido por el brief: mete el blend en el CinemachineBrain
    /// con curva EaseInOut y duración configurable, y la transición se dispara
    /// simplemente subiendo la prioridad de la cámara virtual destino.
    ///
    /// Setup en escena:
    ///   - Main Camera con CinemachineBrain.
    ///   - 1 CinemachineCamera "VistaAttract"  (lejana, aspirational)
    ///   - 1 CinemachineCamera "VistaGeneral"  (mall completo, frame 01)
    ///   - 1 CinemachineCamera por estación, encuadrando la góndola+mercaderista
    /// </summary>
    public class CameraSequencer : MonoBehaviour
    {
        [System.Serializable]
        public class CameraSlot
        {
            public string id;
            public CinemachineCamera virtualCamera;
        }

        [Header("Cámara principal")]
        [SerializeField] private CinemachineBrain brain;

        [Header("Cámaras virtuales")]
        [SerializeField] private CameraSlot vistaAttract;
        [SerializeField] private CameraSlot vistaGeneral;
        [SerializeField] private List<CameraSlot> camarasEstacion = new List<CameraSlot>();

        [Header("Blends")]
        [Tooltip("Duración por defecto del blend (segundos). Sobreescribe el del CinemachineBrain.")]
        [SerializeField, Range(0.2f, 4f)] private float blendDefaultSeconds = 1.6f;
        [Tooltip("Estilo de la curva. EaseInOut da la sensación 'super suave' del brief.")]
        [SerializeField] private CinemachineBlendDefinition.Styles estilo = CinemachineBlendDefinition.Styles.EaseInOut;

        private const int PriorityActive = 30;
        private const int PriorityIdle   = 0;

        private void Awake()
        {
            AjustarBlend(blendDefaultSeconds);
        }

        public void IrAVistaAttract() => Activar(vistaAttract);
        public void IrAVistaGeneral() => Activar(vistaGeneral);

        public void IrAEstacion(EstacionData estacion)
        {
            if (estacion == null) { IrAVistaGeneral(); return; }
            var slot = camarasEstacion.Find(c => c != null && c.id == estacion.camaraVirtualId);
            Activar(slot ?? vistaGeneral);
        }

        /// <summary>Permite override puntual del blend (por ejemplo: pull-out más rápido).</summary>
        public void AjustarBlend(float seconds)
        {
            if (brain == null) brain = Camera.main != null ? Camera.main.GetComponent<CinemachineBrain>() : null;
            if (brain == null) return;
            brain.DefaultBlend = new CinemachineBlendDefinition(estilo, Mathf.Max(0.05f, seconds));
        }

        private void Activar(CameraSlot slot)
        {
            if (slot == null) return;
            DesactivarTodas();
            if (slot.virtualCamera != null) slot.virtualCamera.Priority = PriorityActive;
        }

        private void DesactivarTodas()
        {
            SetIdle(vistaAttract);
            SetIdle(vistaGeneral);
            foreach (var c in camarasEstacion) SetIdle(c);
        }

        private static void SetIdle(CameraSlot slot)
        {
            if (slot != null && slot.virtualCamera != null) slot.virtualCamera.Priority = PriorityIdle;
        }
    }
}
