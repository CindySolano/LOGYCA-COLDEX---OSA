using System;
using UnityEngine;

namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Un KPI individual de una opción. Tres atributos: tipo (qué se mide),
    /// dirección (sube/baja/igual/no aplica) y descripción textual del cambio.
    /// </summary>
    [Serializable]
    public class KPIData
    {
        public TipoKPI tipo;
        public Direccion direccion;
        [TextArea(1, 2)] public string valorDescriptivo;
    }
}
