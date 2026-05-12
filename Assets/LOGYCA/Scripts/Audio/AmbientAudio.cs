using System.Collections;
using UnityEngine;

namespace LOGYCA.OSA.Audio
{
    /// <summary>
    /// Controla el audio ambiente de la experiencia.
    /// El GameManager llama Reproducir() al entrar a estados de juego
    /// (MapSelection / Situation / Feedback / Summary) y Detener() al
    /// volver a Intro / Attract. Maneja fade in/out automáticamente
    /// para que la transición sea suave.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AmbientAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        [Header("Volumen y fades")]
        [Tooltip("Volumen máximo cuando suena (0 = silencio, 1 = full).")]
        [Range(0f, 1f)] [SerializeField] private float volumenMax = 0.6f;
        [Tooltip("Tiempo en segundos del fade-in al iniciar el audio.")]
        [Range(0f, 5f)] [SerializeField] private float fadeInSeconds = 1.5f;
        [Tooltip("Tiempo en segundos del fade-out al detener el audio.")]
        [Range(0f, 5f)] [SerializeField] private float fadeOutSeconds = 1.0f;

        private Coroutine fadeActivo;
        private bool sonando;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.loop = true;
                audioSource.playOnAwake = false;
            }
        }

        private void Awake()
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null) return;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 0f;
        }

        /// <summary>Arranca el audio (idempotente — si ya está sonando, no hace nada).</summary>
        public void Reproducir()
        {
            if (audioSource == null || audioSource.clip == null) return;
            if (sonando) return;
            sonando = true;

            if (fadeActivo != null) StopCoroutine(fadeActivo);
            if (!audioSource.isPlaying) audioSource.Play();
            fadeActivo = StartCoroutine(Fade(audioSource.volume, volumenMax, fadeInSeconds));
        }

        /// <summary>Apaga el audio con fade-out (idempotente).</summary>
        public void Detener()
        {
            if (audioSource == null) return;
            if (!sonando) return;
            sonando = false;

            if (fadeActivo != null) StopCoroutine(fadeActivo);
            fadeActivo = StartCoroutine(DetenerSuave());
        }

        private IEnumerator DetenerSuave()
        {
            yield return Fade(audioSource.volume, 0f, fadeOutSeconds);
            audioSource.Stop();
            fadeActivo = null;
        }

        private IEnumerator Fade(float from, float to, float duration)
        {
            float t = 0f;
            float dur = Mathf.Max(0.01f, duration);
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                audioSource.volume = Mathf.Lerp(from, to, t / dur);
                yield return null;
            }
            audioSource.volume = to;
            if (Mathf.Approximately(to, 0f)) fadeActivo = null;
        }
    }
}
