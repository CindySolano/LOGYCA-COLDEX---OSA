using System.Collections.Generic;
using UnityEngine;

namespace LOGYCA.OSA.Data
{
    [CreateAssetMenu(fileName = "ExperienciaConfig", menuName = "LOGYCA/Experiencia Config", order = 0)]
    public class ExperienciaConfig : ScriptableObject
    {
        [Header("Estaciones (orden de aparición — 5)")]
        public List<EstacionData> estaciones = new List<EstacionData>();

        [Header("Tiempos (segundos)")]
        public float idleTimeoutSeconds = 60f;
        [Tooltip("Duración del blend de Cinemachine al hacer dolly al hotspot (frame 02).")]
        public float dollyInSeconds = 1.6f;
        [Tooltip("Duración del blend al volver a la vista general (frame 05).")]
        public float pullOutSeconds = 1.4f;
        [Tooltip("Fade-in de la card DATO + pregunta tras llegar al hotspot.")]
        public float fadeInSeconds = 0.25f;
        [Tooltip("Tiempo que la mercaderista actúa antes de mostrar el feedback.")]
        public float duracionAccionMercaderista = 1.2f;

        [Header("HUD valores iniciales (slide 5)")]
        [Range(0, 100)] public int osaInicial = 92;
        public string invInicial = "OK";
        [Range(0, 100)] public int sosInicial = 38;

        [Header("Paleta global")]
        public Color naranjaLogyca = new Color(0.961f, 0.376f, 0.118f); // #F5601E
        public Color tealLogyca    = new Color(0.157f, 0.667f, 0.580f); // #28AA94
        public Color rojoOscuro    = new Color(0.804f, 0.255f, 0.255f);
        public Color verdeFavorable = new Color(0.231f, 0.427f, 0.067f);
        public Color textoPrincipal = new Color(0.275f, 0.275f, 0.255f);
    }
}
