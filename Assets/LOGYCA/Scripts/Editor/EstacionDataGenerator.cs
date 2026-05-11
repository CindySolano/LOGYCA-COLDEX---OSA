#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.EditorTools
{
    /// <summary>
    /// Genera los 5 ScriptableObjects de estaciones con el modelo simplificado
    /// (delta OSA / INV / SOS + porqué + label de zona). Cada estación tiene 3
    /// opciones inspiradas en la plantilla del PDF.
    ///
    /// Uso: menú LOGYCA → Generar estaciones (5 .asset)
    /// </summary>
    public static class EstacionDataGenerator
    {
        private const string CarpetaDestino = "Assets/LOGYCA/ScriptableObjects/Estaciones";

        [MenuItem("LOGYCA/Generar estaciones (5 .asset)")]
        public static void Generar()
        {
            EnsureDir(CarpetaDestino);
            LimpiarAssetsViejos();   // borra cualquier .asset previo (incl. 03_Panaderia si existe)
            CrearFruteria();
            CrearCarnes();
            CrearCaja();
            CrearAbarrotes();
            CrearBodega();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[LOGYCA] 5 estaciones generadas en " + CarpetaDestino);
        }

        private static void LimpiarAssetsViejos()
        {
            if (!Directory.Exists(CarpetaDestino)) return;
            foreach (var path in Directory.GetFiles(CarpetaDestino, "*.asset"))
            {
                var rel = path.Replace('\\', '/');
                AssetDatabase.DeleteAsset(rel);
            }
        }

        // -------- 1. FRUTERÍA (la del slide 8 / 9) --------
        private static void CrearFruteria()
        {
            var e = ScriptableObject.CreateInstance<EstacionData>();
            e.id = "fruteria";
            e.nombre = "Frutería";
            e.subtitulo = "Rotación y mermas";
            e.colorPrincipal = new Color(0.961f, 0.376f, 0.118f);
            e.camaraVirtualId = "fruteria";
            e.contexto = "La frutería tiene mermas del 12% esta semana. La fruta que se daña es la que está al fondo del exhibidor.";
            e.pregunta = "¿Qué deberías hacer primero?";

            e.opciones.Add(Op('A', "Rotar fruta: pasar la del fondo al frente",
                acertada: true, deltaOsa: +4, deltaSos: +2, invDir: DeltaDir.Igual, invDesc: "→ 0",
                labelZona: "FRUTERÍA ROTADA", animMerc: "Rotar", nivelCol: 1,
                porque: "Rotar la fruta protege OSA sin sacrificar inventario. La merma baja porque la fruta vieja sale antes de dañarse."));

            e.opciones.Add(Op('B', "Bajar el precio de la fruta del fondo",
                acertada: false, deltaOsa: -2, deltaSos: -1, invDir: DeltaDir.Baja, invDesc: "−1u",
                labelZona: "FRUTERÍA EN PROMOCIÓN", animMerc: "Promocion", nivelCol: 1,
                porque: "Resolviste hoy con descuento, pero la merma estructural sigue ahí y el margen se redujo."));

            e.opciones.Add(Op('C', "Pedir menos fruta al próximo despacho",
                acertada: false, deltaOsa: -6, deltaSos: -4, invDir: DeltaDir.Baja, invDesc: "−3u",
                labelZona: "FRUTERÍA SIN ATENDER", animMerc: "PedirMenos", nivelCol: 2,
                porque: "Pedir menos baja la merma pero también el OSA y el espacio en góndola. La competencia ocupa el aire que cediste."));

            Save(e, "01_Fruteria");
        }

        // -------- 2. CARNES Y LÁCTEOS --------
        private static void CrearCarnes()
        {
            var e = ScriptableObject.CreateInstance<EstacionData>();
            e.id = "carnes";
            e.nombre = "Carnes y lácteos";
            e.subtitulo = "Cadena de frío";
            e.colorPrincipal = new Color(0.768f, 0.282f, 0.282f);
            e.camaraVirtualId = "carnes";
            e.contexto = "Esta semana se dañó el 9% de las carnes empacadas. Y al mismo tiempo se acabó la leche entera 3 días seguidos. El proveedor entrega 2 veces por semana.";

            e.opciones.Add(Op('A', "Pedirle al proveedor que entregue 3 veces por semana",
                acertada: false, deltaOsa: +4, deltaSos: +1, invDir: DeltaDir.Baja, invDesc: "−2u",
                labelZona: "FRÍO REFORZADO · COSTO ALTO", animMerc: "Reorden", nivelCol: 2,
                porque: "Vendiste un poco más por menos quiebres, pero el camión extra se comió casi toda la ganancia."));

            e.opciones.Add(Op('B', "Trabajar en equipo con el proveedor: comparten datos y producción",
                acertada: true, deltaOsa: +6, deltaSos: +3, invDir: DeltaDir.Baja, invDesc: "−3u",
                labelZona: "FRÍO COLABORATIVO", animMerc: "Colaborar", nivelCol: 4,
                porque: "Vendiste más, gastaste menos en logística y casi no se dañó producto. La gestión conjunta paga la inversión."));

            e.opciones.Add(Op('C', "Seguir igual y rebajar las carnes próximas a vencerse",
                acertada: false, deltaOsa: -2, deltaSos: -1, invDir: DeltaDir.Igual, invDesc: "→ 0",
                labelZona: "FRÍO EN LIQUIDACIÓN", animMerc: "Rebajar", nivelCol: 1,
                porque: "Tapaste el hueco de hoy, pero rebajar precios te bajó la ganancia y la próxima semana vuelve a pasar."));

            Save(e, "02_Carnes");
        }

        // -------- 3. CAJA REGISTRADORA --------
        private static void CrearCaja()
        {
            var e = ScriptableObject.CreateInstance<EstacionData>();
            e.id = "caja";
            e.nombre = "Cajas";
            e.subtitulo = "Productos de impulso";
            e.colorPrincipal = new Color(0.498f, 0.467f, 0.867f);
            e.camaraVirtualId = "caja";
            e.contexto = "En las cajas tienes 24 productos de impulso: chocolatinas, chicles, pilas, revistas. Solo 8 de cada 10 veces el cliente los encuentra disponibles, porque sólo se reponen al cambio de turno. Estos productos son los que más margen dejan.";

            e.opciones.Add(Op('A', "Poner una persona dedicada a reponer las cajas cada 2 horas",
                acertada: false, deltaOsa: +4, deltaSos: +1, invDir: DeltaDir.Baja, invDesc: "−2u",
                labelZona: "REPONEDOR EN TURNO", animMerc: "Reponer", nivelCol: 1,
                porque: "Ganas mucho en impulso, pero el sueldo del reponedor se come parte de la utilidad y el proveedor queda fuera de la jugada."));

            e.opciones.Add(Op('B', "Que la marca líder gestione y reponga toda la zona de cajas",
                acertada: false, deltaOsa: +5, deltaSos: -2, invDir: DeltaDir.Baja, invDesc: "−3u",
                labelZona: "ZONA DELEGADA", animMerc: "Delegar", nivelCol: 2,
                porque: "Vendes más sin operar la zona, pero cediste el control de tu espacio más rentable. Es delegación, no colaboración."));

            e.opciones.Add(Op('C', "Comité conjunto cadena-proveedor con datos en tiempo real",
                acertada: true, deltaOsa: +6, deltaSos: +3, invDir: DeltaDir.Baja, invDesc: "−3u",
                labelZona: "COMITÉ ACTIVO", animMerc: "Colaborar", nivelCol: 4,
                porque: "Mantienes el control de la zona y sumás la inteligencia del proveedor. Ambos ven los datos y deciden juntos: mejor margen y aprendizaje compartido."));

            Save(e, "03_Caja");
        }

        // -------- 4. ABARROTES --------
        private static void CrearAbarrotes()
        {
            var e = ScriptableObject.CreateInstance<EstacionData>();
            e.id = "abarrotes";
            e.nombre = "Abarrotes";
            e.subtitulo = "Surtido y rotación";
            e.colorPrincipal = new Color(0.114f, 0.620f, 0.459f);
            e.camaraVirtualId = "abarrotes";
            e.contexto = "En la sección de aceites tienes 38 referencias. La plata invertida subió 18% este año y 6 referencias casi no se mueven.";

            e.opciones.Add(Op('A', "Sacar de la góndola las 6 referencias de baja rotación",
                acertada: false, deltaOsa: +1, deltaSos: -2, invDir: DeltaDir.Baja, invDesc: "−6u",
                labelZona: "ANAQUEL DEPURADO", animMerc: "Retirar", nivelCol: 1,
                porque: "Liberaste plata pero el cliente fiel que buscaba esas marcas se va molesto. El proveedor pierde su espacio."));

            e.opciones.Add(Op('B', "Pago por venta",
                acertada: false, deltaOsa: 0, deltaSos: 0, invDir: DeltaDir.Baja, invDesc: "−2u",
                labelZona: "RIESGO COMPARTIDO", animMerc: "Renegociar", nivelCol: 3,
                porque: "Liberas capital pero el proveedor sube el precio para cubrir el riesgo. Es un primer paso de confianza."));

            e.opciones.Add(Op('C', "Planear surtido juntos",
                acertada: true, deltaOsa: +2, deltaSos: +3, invDir: DeltaDir.Baja, invDesc: "−4u",
                labelZona: "SURTIDO CONJUNTO", animMerc: "Colaborar", nivelCol: 4,
                porque: "Ambos analizan, ambos deciden, ambos responden. Ese es uno de los pilares más maduros del OSA colaborativo."));

            Save(e, "04_Abarrotes");
        }

        // -------- 5. BODEGA --------
        private static void CrearBodega()
        {
            var e = ScriptableObject.CreateInstance<EstacionData>();
            e.id = "bodega";
            e.nombre = "Bodega";
            e.subtitulo = "Reposición";
            e.colorPrincipal = new Color(0.216f, 0.541f, 0.867f);
            e.camaraVirtualId = "bodega";
            e.contexto = "En el 7% de las veces que un cliente no encuentra un producto, ese producto sí está en bodega. El equipo repone en horarios fijos.";

            e.opciones.Add(Op('A', "Reposición por alerta cada hora",
                acertada: false, deltaOsa: +5, deltaSos: +1, invDir: DeltaDir.Igual, invDesc: "→ 0",
                labelZona: "REPOSICIÓN REACTIVA", animMerc: "Reponer", nivelCol: 1,
                porque: "Plata dormida que ahora se vende. Pero el proveedor se beneficia sin enterarse — la oportunidad colaborativa queda intacta."));

            e.opciones.Add(Op('B', "Reorganizar la bodega",
                acertada: false, deltaOsa: +3, deltaSos: 0, invDir: DeltaDir.Baja, invDesc: "−1u",
                labelZona: "BODEGA ORDENADA", animMerc: "Organizar", nivelCol: 1,
                porque: "Mejora rápida y barata, pero el problema de fondo (los horarios fijos de reposición) sigue ahí."));

            e.opciones.Add(Op('C', "Sincronizar bodega y producción del proveedor",
                acertada: true, deltaOsa: +6, deltaSos: +2, invDir: DeltaDir.Baja, invDesc: "−4u",
                labelZona: "BODEGA SINCRONIZADA", animMerc: "Colaborar", nivelCol: 4,
                porque: "Libera capital congelado y los dos jugando como un equipo capturan la rentabilidad combinada más alta."));

            Save(e, "05_Bodega");
        }

        // -------- helpers --------

        private static OpcionData Op(char letra, string titulo, bool acertada,
            int deltaOsa, int deltaSos, DeltaDir invDir, string invDesc,
            string labelZona, string animMerc, int nivelCol, string porque) => new OpcionData
        {
            letra = letra,
            titulo = titulo,
            esAcertada = acertada,
            deltaOsaPct = deltaOsa,
            deltaSosPct = deltaSos,
            deltaInvDir = invDir,
            deltaInvDescriptivo = invDesc,
            labelEstadoZona = labelZona,
            animacionMercaderista = animMerc,
            nivelColaboracion = nivelCol,
            porque = porque
        };

        private static void Save(EstacionData e, string nombre)
        {
            string path = $"{CarpetaDestino}/{nombre}.asset";
            AssetDatabase.CreateAsset(e, path);
        }

        private static void EnsureDir(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}
#endif
