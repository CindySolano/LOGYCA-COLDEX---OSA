using UnityEngine;
using UnityEngine.InputSystem;

namespace LOGYCA.OSA.Core
{
    /// <summary>
    /// Detecta inactividad (touch / mouse / teclado) y dispara un callback.
    /// Usa el nuevo Input System de Unity 17.
    /// </summary>
    public class IdleTimer : MonoBehaviour
    {
        public float timeoutSeconds = 60f;
        public System.Action OnIdle;

        private float lastInteractionTime;
        private bool active;

        public void StartTracking()
        {
            lastInteractionTime = Time.unscaledTime;
            active = true;
        }

        public void StopTracking() => active = false;

        public void Pulse() => lastInteractionTime = Time.unscaledTime;

        private void Update()
        {
            if (!active) return;

            if (HuboInteraccion())
                lastInteractionTime = Time.unscaledTime;

            if (Time.unscaledTime - lastInteractionTime > timeoutSeconds)
            {
                active = false;
                OnIdle?.Invoke();
            }
        }

        private static bool HuboInteraccion()
        {
            var ts = Touchscreen.current;
            if (ts != null && ts.primaryTouch.press.isPressed) return true;

            var mouse = Mouse.current;
            if (mouse != null && (mouse.leftButton.isPressed || mouse.delta.ReadValue().sqrMagnitude > 1f)) return true;

            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.anyKey.isPressed) return true;

            return false;
        }
    }
}
