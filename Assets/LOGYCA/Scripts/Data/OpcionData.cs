using System;
using System.Collections.Generic;
using UnityEngine;

namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Una opción de decisión de la estación. Tiene dos capas de información:
    ///
    /// 1) Capa HUD (slide 5): 3 deltas sobre OSA, INV, SOS — alimentan la
    ///    barra superior persistente.
    /// 2) Capa feedback rico (.md sección 7.5): 9 KPIs detallados + 4 niveles
    ///    de colaboración + 3 bloques narrativos (cadena, proveedor, veredicto).
    /// </summary>
    [Serializable]
    public class OpcionData
    {
        [Header("Identificación")]
        public char letra = 'A';
        [TextArea(2, 4)] public string titulo;

        [Header("Veredicto")]
        public bool esAcertada;

        [Header("Capa HUD (OSA / INV / SOS)")]
        [Tooltip("Cambio porcentual sobre OSA (ej. +4 = sube de 92% a 96%).")]
        public int deltaOsaPct;
        [Tooltip("Cambio en INV expresado como string (ej. \"→ 0\", \"-3u\", \"OK\").")]
        public string deltaInvDescriptivo = "→ 0";
        public DeltaDir deltaInvDir = DeltaDir.Igual;
        [Tooltip("Cambio porcentual sobre SOS (ej. +2 = sube de 38% a 40%).")]
        public int deltaSosPct;

        [Header("Capa feedback rico — 9 KPIs (.md sección 7.5)")]
        public List<KPIData> kpis = new List<KPIData>();

        [Header("Capa feedback rico — Colaboración")]
        [Range(1, 4)] public int nivelColaboracion = 1;

        [Header("Capa feedback rico — Narrativa")]
        [TextArea(3, 6)] public string feedbackCadena;
        [TextArea(3, 6)] public string feedbackProveedor;
        [TextArea(3, 6)] public string veredicto;

        [Header("Mercaderista")]
        [Tooltip("Trigger del Animator de la mercaderista al ejecutar la decisión.")]
        public string animacionMercaderista = "Trabajando";
    }
}
