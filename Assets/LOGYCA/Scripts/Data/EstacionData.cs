using System.Collections.Generic;
using UnityEngine;

namespace LOGYCA.OSA.Data
{
    [CreateAssetMenu(fileName = "Estacion", menuName = "LOGYCA/Estación", order = 1)]
    public class EstacionData : ScriptableObject
    {
        [Header("Identificación")]
        public string id;
        public string nombre;             // "Frutería"
        public string subtitulo;          // "Rotación y mermas"
        public Sprite icono;
        public Color colorPrincipal = Color.white;

        [Header("Cámara virtual destino")]
        [Tooltip("ID que el CameraSequencer busca en su lista para hacer dolly al hotspot.")]
        public string camaraVirtualId;

        [Header("Contenido narrativo")]
        [TextArea(3, 6)] public string contexto;
        public string pregunta = "¿Qué deberías hacer primero?";

        [Header("Opciones (3)")]
        public List<OpcionData> opciones = new List<OpcionData>();
    }
}
