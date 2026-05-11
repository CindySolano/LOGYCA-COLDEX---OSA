using System;
using UnityEngine;

namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Una de las 3 opciones de decisión de la estación. Modelo simple alineado
    /// con las slides 8 y 9: la opción mueve los 3 indicadores del HUD y dispara
    /// un veredicto narrativo (porqué).
    /// </summary>
    [Serializable]
    public class OpcionData
    {
        [Header("Identificación")]
        public char letra = 'A';
        [TextArea(2, 4)] public string titulo;

        [Header("Veredicto")]
        public bool esAcertada;

        [Header("Impacto en el HUD")]
        [Tooltip("Cambio porcentual sobre OSA (ej. +4 = sube de 92% a 96%).")]
        public int deltaOsaPct;

        [Tooltip("Cambio en INV expresado en string (ej. \"→ 0\", \"-3u\", \"OK\").")]
        public string deltaInvDescriptivo = "→ 0";
        public DeltaDir deltaInvDir = DeltaDir.Igual;

        [Tooltip("Cambio porcentual sobre SOS (ej. +2 = sube de 38% a 40%).")]
        public int deltaSosPct;

        [Header("Estado de la zona (frame de feedback)")]
        [Tooltip("Texto del label sobre la góndola al ver el resultado, ej. \"FRUTERÍA ROTADA\".")]
        public string labelEstadoZona;

        [Header("Mercaderista")]
        [Tooltip("Trigger del Animator de la mercaderista al ejecutar la decisión (vacío = sin animación).")]
        public string animacionMercaderista;

        [Header("Narrativa de feedback")]
        [TextArea(3, 6)] public string porque;

        [Header("Niveles (analytics / summary)")]
        [Range(1, 4)] public int nivelColaboracion = 1;
    }
}
