#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.EditorTools
{
    /// <summary>
    /// Genera los 5 ScriptableObjects con el contenido COMPLETO del .md
    /// (9 KPIs + cadena/proveedor/veredicto + colaboración) más la capa HUD
    /// (deltas OSA / INV / SOS) que se usa para la barra superior.
    ///
    /// Estaciones del piloto: Frutería, Carnes, Cajas, Abarrotes, Bodega.
    /// </summary>
    public static class EstacionDataGenerator
    {
        private const string CarpetaDestino = "Assets/LOGYCA/ScriptableObjects/Estaciones";

        [MenuItem("LOGYCA/Generar estaciones (5 .asset)")]
        public static void Generar()
        {
            EnsureDir(CarpetaDestino);
            LimpiarAssetsViejos();
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
                AssetDatabase.DeleteAsset(path.Replace('\\', '/'));
        }

        // ====================================================================
        //                             FRUTERÍA
        // ====================================================================
        private static void CrearFruteria()
        {
            var e = NewEstacion("fruteria", "Frutería", "Frutería", "Rotación y mermas",
                                new Color(0.961f, 0.376f, 0.118f),
                                "La frutería tiene mermas del 12% esta semana. La fruta que se daña es la que está al fondo del exhibidor.");

            e.opciones.Add(Op('A', "Rotar fruta: pasar la del fondo al frente", acertada: true, nivelCol: 1,
                deltaOsa: +4, deltaSos: +2, invDir: DeltaDir.Igual, invDesc: "→ 0", anim: "Rotar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "+4%"),
                    K(TipoKPI.DiasInventario,        Direccion.Igual,"Igual"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "+2%"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+3%"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+5%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Solución simple, sin costo, mejora la rotación natural y baja la merma. La cadena resuelve sola el problema.",
                proveedor: "No se entera del problema ni participa de la solución. Sigue despachando igual.",
                veredicto: "Buena solución operativa, pero la oportunidad colaborativa queda sobre la mesa."));

            e.opciones.Add(Op('B', "Bajar el precio de la fruta del fondo", acertada: false, nivelCol: 1,
                deltaOsa: +2, deltaSos: 0, invDir: DeltaDir.Baja, invDesc: "-10u", anim: "Promocion",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "+2%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "-10 unidades"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "+5%"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Igual,"Casi igual (vendes lo mismo más barato)"),
                    K(TipoKPI.MargenGanancia,        Direccion.Baja, "-6%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Igual,"Igual"),
                },
                cadena:    "Tapaste el hueco de hoy con descuentos, pero rebajar precios te bajó la ganancia.",
                proveedor: "No se entera de la merma. La próxima semana se repite la misma historia.",
                veredicto: "Pierden los dos, solo que el proveedor todavía no lo sabe. El problema estructural sigue intacto."));

            e.opciones.Add(Op('C', "Pedir menos fruta al próximo despacho", acertada: false, nivelCol: 2,
                deltaOsa: -6, deltaSos: -4, invDir: DeltaDir.Baja, invDesc: "-3u", anim: "PedirMenos",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Baja, "-6%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "-3 unidades"),
                    K(TipoKPI.EspacioGondola,        Direccion.Baja, "-4%"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Baja, "-5%"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Baja, "-4% (más quiebres)"),
                    K(TipoKPI.MargenGanancia,        Direccion.Baja, "-3%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Baja, "Baja"),
                },
                cadena:    "Pedir menos baja la merma pero también baja el OSA y el espacio en góndola. La cadena pierde ventas y deja de captar a clientes que buscan fruta.",
                proveedor: "Recibe un pedido menor sin contexto: se queda con inventario que tendrá que colocar en otro lado.",
                veredicto: "Solución reactiva que crea más problemas. La operación parece más eficiente pero el cliente y la venta sufren."));

            Save(e, "01_Fruteria");
        }

        // ====================================================================
        //                              CARNES
        // ====================================================================
        private static void CrearCarnes()
        {
            var e = NewEstacion("carnes", "Carnes y lácteos", "Carnicería", "Cadena de frío",
                                new Color(0.768f, 0.282f, 0.282f),
                                "Esta semana se dañó el 9% de las carnes empacadas porque se vencieron antes de venderse. Y al mismo tiempo, hubo 3 días en los que se acabó la leche entera de 1 litro y los clientes no la encontraron. El proveedor entrega 2 veces por semana.");

            e.opciones.Add(Op('A', "Pedirle al proveedor que entregue 3 veces por semana", acertada: false, nivelCol: 2,
                deltaOsa: +4, deltaSos: +1, invDir: DeltaDir.Baja, invDesc: "-2u", anim: "Reorden",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 91% a 95%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 7 a 5 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Igual,"Igual"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Baja, "Baja de 4 a 3 días"),
                    K(TipoKPI.CostoTransporte,       Direccion.Sube, "+30%"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+5% (menos quiebres)"),
                    K(TipoKPI.MargenGanancia,        Direccion.Igual,"Casi igual (lo que ganas se va en transporte)"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Vendiste un poco más por menos quiebres, pero el camión extra se comió casi toda la ganancia. Resolviste hoy, pero a un costo alto.",
                proveedor: "Está poniendo más camiones y planeando más despachos sin recibir nada extra a cambio. La carga operativa subió, su margen también se apretó.",
                veredicto: "La cadena gana poquito, el proveedor pierde. Esto no es sostenible: tarde o temprano él te subirá el precio o pedirá renegociar."));

            e.opciones.Add(Op('B', "Trabajar en equipo con el proveedor: comparten datos, inventario y producción", acertada: true, nivelCol: 4,
                deltaOsa: +6, deltaSos: +3, invDir: DeltaDir.Baja, invDesc: "-3u", anim: "Colaborar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 91% a 97%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 7 a 5 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Igual,"Igual"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Baja, "Baja de 4 a 2 días"),
                    K(TipoKPI.CostoTransporte,       Direccion.Sube, "+12%"),
                    K(TipoKPI.CostoOperativo,        Direccion.Sube, "+3% (reuniones y software compartido)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+8% (mejor surtido y menos merma)"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+10%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Vendiste más, gastaste menos en logística, casi no se dañó producto y el cliente siempre encuentra lo que busca. Hay que abrir información y dedicar tiempo, pero el resultado lo justifica.",
                proveedor: "Planea su producción con datos reales y participa en las decisiones de pedido. Despacha lo justo, hace menos viajes en vacío, reduce devoluciones por vencimiento.",
                veredicto: "Ganan los dos. Cuando ambos comparten información Y deciden juntos, el sistema completo se vuelve más eficiente y la torta crece para ambos."));

            e.opciones.Add(Op('C', "Seguir igual y rebajar las carnes próximas a vencerse", acertada: false, nivelCol: 1,
                deltaOsa: -2, deltaSos: -1, invDir: DeltaDir.Igual, invDesc: "→ 0", anim: "Rebajar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Baja, "Baja de 91% a 89%"),
                    K(TipoKPI.DiasInventario,        Direccion.Igual,"Igual, 7 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Baja, "-1 cara"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Igual,"Casi igual"),
                    K(TipoKPI.MargenGanancia,        Direccion.Baja, "-6% (descuentos comen la ganancia)"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Igual,"Igual"),
                },
                cadena:    "Tapaste el hueco de hoy, pero rebajar precios te bajó la ganancia y la próxima semana vuelve a pasar.",
                proveedor: "No se enteró de la merma ni del problema. Va a seguir despachando igual.",
                veredicto: "Pierden los dos, solo que el proveedor todavía no lo sabe. El problema estructural sigue intacto."));

            Save(e, "02_Carnes");
        }

        // ====================================================================
        //                                CAJAS
        // ====================================================================
        private static void CrearCaja()
        {
            var e = NewEstacion("caja", "Cajas", "Cajas registradoras", "Productos de impulso",
                                new Color(0.498f, 0.467f, 0.867f),
                                "En las cajas tienes 24 productos de impulso: chocolatinas, chicles, pilas, revistas. Solo 8 de cada 10 veces el cliente los encuentra disponibles, porque sólo se reponen al cambio de turno. Estos productos son los que más margen dejan.");

            e.opciones.Add(Op('A', "Poner una persona dedicada a reponer las cajas cada 2 horas", acertada: false, nivelCol: 1,
                deltaOsa: +4, deltaSos: +1, invDir: DeltaDir.Baja, invDesc: "-2u", anim: "Reponer",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 82% a 93%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 6 a 4 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Igual,"Igual"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Sube, "+11% (sueldo del reponedor)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+15% en impulso"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+18%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "La cadena gana mucho porque el impulso es de los productos con más margen. El sueldo del reponedor se paga solo con las ventas extras.",
                proveedor: "Vende un poco más, pero sin enterarse. Para él es una venta normal, no hay nada distinto en la relación.",
                veredicto: "La cadena resuelve sola y gana. El proveedor se beneficia indirectamente pero queda fuera de la jugada."));

            e.opciones.Add(Op('B', "Que la marca líder gestione y reponga toda la zona de cajas (delegación)", acertada: false, nivelCol: 2,
                deltaOsa: +5, deltaSos: -2, invDir: DeltaDir.Baja, invDesc: "-3u", anim: "Delegar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 82% a 96%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 6 a 3 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "Optimizado por la marca"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Baja, "Reposición diaria"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Asumido por el proveedor"),
                    K(TipoKPI.CostoOperativo,        Direccion.Baja, "-8% (lo asume el proveedor)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+22% en impulso"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+25%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "No tienes que preocuparte por las cajas: la marca repone, decide variedad y asume el riesgo. La cadena cobra por el espacio y por la venta. Pero pierde control de la zona más rentable.",
                proveedor: "Tiene control total de la mejor zona de la tienda. Decide qué exhibir según ventas reales y captura todo el valor.",
                veredicto: "Esto no es colaboración, es delegación. Funciona pero es transaccional. Si la marca cambia de estrategia, la cadena queda expuesta."));

            e.opciones.Add(Op('C', "Comité conjunto cadena-proveedor con datos en tiempo real", acertada: true, nivelCol: 4,
                deltaOsa: +6, deltaSos: +3, invDir: DeltaDir.Baja, invDesc: "-3u", anim: "Colaborar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 82% a 94%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 6 a 3 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "Optimizado por datos"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Baja, "Reposición ajustada"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Sube, "+5% (comité y software)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+19% en impulso"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+21%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Mantienes el control de la zona y sumás la inteligencia del proveedor. Ambos ven los datos en tiempo real y ajustan juntos.",
                proveedor: "Participa en decisiones reales, no solo despacha. Aporta conocimiento de categoría y aprende del comportamiento de compra en esta tienda.",
                veredicto: "Ganan los dos sin que ninguno pierda control. Esta es la diferencia entre delegar (B) y colaborar (C)."));

            Save(e, "03_Caja");
        }

        // ====================================================================
        //                              ABARROTES
        // ====================================================================
        private static void CrearAbarrotes()
        {
            var e = NewEstacion("abarrotes", "Abarrotes", "Abarrotes", "Surtido y rotación",
                                new Color(0.114f, 0.620f, 0.459f),
                                "En la sección de aceites tienes 38 referencias. La plata invertida en este inventario subió 18% este año y 6 referencias se venden menos de 2 unidades por semana. El cliente sí encuentra lo que busca, pero estás cargando con mucho inventario que no se mueve.");

            e.opciones.Add(Op('A', "Sacar de la góndola las 6 referencias que casi no se venden", acertada: false, nivelCol: 1,
                deltaOsa: +1, deltaSos: -2, invDir: DeltaDir.Baja, invDesc: "-6 refs", anim: "Retirar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 96% a 97%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 45 a 32 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "+30% para las que sí se venden"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Baja, "-4%"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+3% (las top tienen más visibilidad)"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+11%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Baja, "Baja"),
                },
                cadena:    "Liberaste plata y los productos top se ven mejor. Vendes un poco más. Pero el cliente fiel que buscaba esa marca específica se va molesto.",
                proveedor: "Las marcas que sacaste pierden su espacio. Su producto deja de existir en tu tienda sin oportunidad de defenderse.",
                veredicto: "La cadena gana en margen, el proveedor pierde categóricamente y el cliente se va incómodo."));

            e.opciones.Add(Op('B', "Acordar con el proveedor que solo le pagas cuando vendes el producto", acertada: false, nivelCol: 3,
                deltaOsa: 0, deltaSos: 0, invDir: DeltaDir.Baja, invDesc: "-2u", anim: "Renegociar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Igual,"Igual, 96%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 45 a 38 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Igual,"Igual"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Igual,"Igual"),
                    K(TipoKPI.Ventas,                Direccion.Igual,"Igual"),
                    K(TipoKPI.MargenGanancia,        Direccion.Baja, "-3% (el proveedor sube precio)"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Igual,"Igual"),
                },
                cadena:    "Mantienes todas las referencias y liberas plata congelada. Pero el proveedor te sube el precio para cubrir el riesgo.",
                proveedor: "Ahora carga con el riesgo del inventario sin vender. Sube el precio para protegerse, pero gana visibilidad de qué se vende y dónde.",
                veredicto: "Empiezan a colaborar, pero todavía no es un pacto equilibrado. Es un primer paso de confianza, no el destino final."));

            e.opciones.Add(Op('C', "Planear el surtido juntos cada mes con datos compartidos", acertada: true, nivelCol: 4,
                deltaOsa: +2, deltaSos: +3, invDir: DeltaDir.Baja, invDesc: "-4u", anim: "Colaborar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 96% a 98%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 45 a 28 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "Optimizado por categoría"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Baja, "Baja de 7 a 4 días"),
                    K(TipoKPI.CostoTransporte,       Direccion.Baja, "-12%"),
                    K(TipoKPI.CostoOperativo,        Direccion.Sube, "+3% (reuniones mensuales)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+9%"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+19%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "La cadena aporta datos de comportamiento del shopper, el proveedor aporta conocimiento de su categoría. Las decisiones son compartidas, así que ambos las defienden y las ejecutan.",
                proveedor: "Participa en las decisiones de qué se queda y qué sale. Optimiza su portafolio, reduce devoluciones, baja costos y vende más.",
                veredicto: "Ganan los dos al máximo. Esto es planeación conjunta de surtido: ambos analizan, ambos deciden, ambos responden."));

            Save(e, "04_Abarrotes");
        }

        // ====================================================================
        //                                BODEGA
        // ====================================================================
        private static void CrearBodega()
        {
            var e = NewEstacion("bodega", "Bodega", "Bodega", "Reposición y trastienda",
                                new Color(0.216f, 0.541f, 0.867f),
                                "Una revisión muestra algo curioso: en el 7% de las veces que un cliente no encuentra un producto, ese producto SÍ está en la bodega de la tienda, solo que nadie lo subió a la góndola. El equipo repone en horarios fijos (3 veces al día) y por costumbre, no mirando qué se está acabando.");

            e.opciones.Add(Op('A', "Reposición por alerta cada hora", acertada: false, nivelCol: 1,
                deltaOsa: +5, deltaSos: +1, invDir: DeltaDir.Igual, invDesc: "→ 0", anim: "Reponer",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 90% a 95%"),
                    K(TipoKPI.DiasInventario,        Direccion.Igual,"Igual"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "Siempre se ve llena"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"No aplica"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Sube, "+9% (mano de obra extra)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+9%"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+16%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Resolviste el problema sin comprar nada nuevo: el inventario que ya tenías ahora sí llega al cliente. Plata dormida convertida en ventas.",
                proveedor: "Vende más porque la cadena exhibe más, pero no se enteró de nada. Para él es solo más volumen.",
                veredicto: "La cadena gana mucho con un cambio interno. El proveedor se beneficia sin saberlo. Buena decisión, pero deja la oportunidad colaborativa intacta."));

            e.opciones.Add(Op('B', "Reorganizar la bodega: lo que más se vende cerca de la puerta", acertada: false, nivelCol: 1,
                deltaOsa: +3, deltaSos: 0, invDir: DeltaDir.Baja, invDesc: "-1u", anim: "Organizar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 90% a 93%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 14 a 12 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Igual,"Igual"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoTransporte,       Direccion.Igual,"Igual"),
                    K(TipoKPI.CostoOperativo,        Direccion.Baja, "-3%"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+4%"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+6%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Ahora se reabastece más rápido y se cometen menos errores. Inversión mínima, mejora rápida.",
                proveedor: "Sin cambios. Sigue despachando como siempre y vendiendo lo mismo.",
                veredicto: "Ganancia rápida y barata, pero sin colaboración. El problema de fondo (horarios fijos) sigue ahí."));

            e.opciones.Add(Op('C', "Sincronizar bodega y producción del proveedor", acertada: true, nivelCol: 4,
                deltaOsa: +6, deltaSos: +2, invDir: DeltaDir.Baja, invDesc: "-4u", anim: "Colaborar",
                kpis: new[]
                {
                    K(TipoKPI.DisponibilidadGondola, Direccion.Sube, "Sube de 90% a 96%"),
                    K(TipoKPI.DiasInventario,        Direccion.Baja, "Baja de 14 a 6 días"),
                    K(TipoKPI.EspacioGondola,        Direccion.Sube, "Optimizado"),
                    K(TipoKPI.TiempoEntrega,         Direccion.Baja, "Baja de 4 a 2 días"),
                    K(TipoKPI.CostoTransporte,       Direccion.Sube, "+10%"),
                    K(TipoKPI.CostoOperativo,        Direccion.Sube, "+6% (software y gestión conjunta)"),
                    K(TipoKPI.Ventas,                Direccion.Sube, "+12%"),
                    K(TipoKPI.MargenGanancia,        Direccion.Sube, "+18%"),
                    K(TipoKPI.SatisfaccionShopper,   Direccion.Sube, "Sube"),
                },
                cadena:    "Libera espacio de bodega que se vuelve venta. Reduce capital congelado a la mitad. La inversión está en software y gestión.",
                proveedor: "Planifica producción con datos reales, optimiza rutas, reduce devoluciones. Comparte el riesgo de quiebre y el beneficio.",
                veredicto: "Ganan los dos jugando como un equipo. Requiere confianza y tecnología, pero la rentabilidad combinada es la más alta."));

            Save(e, "05_Bodega");
        }

        // ============================ helpers ============================

        private static EstacionData NewEstacion(string id, string nombre, string nombreLargo,
            string subtitulo, Color color, string contexto)
        {
            var e = ScriptableObject.CreateInstance<EstacionData>();
            e.id = id;
            e.nombre = nombre;
            e.nombreLargo = nombreLargo;
            e.subtitulo = subtitulo;
            e.colorPrincipal = color;
            e.camaraVirtualId = id;
            e.contexto = contexto;
            e.pregunta = "¿Qué deberías hacer primero?";
            return e;
        }

        private static OpcionData Op(char letra, string titulo, bool acertada, int nivelCol,
            int deltaOsa, int deltaSos, DeltaDir invDir, string invDesc, string anim,
            KPIData[] kpis, string cadena, string proveedor, string veredicto)
            => new OpcionData
            {
                letra = letra,
                titulo = titulo,
                esAcertada = acertada,
                deltaOsaPct = deltaOsa,
                deltaSosPct = deltaSos,
                deltaInvDir = invDir,
                deltaInvDescriptivo = invDesc,
                animacionMercaderista = anim,
                kpis = new List<KPIData>(kpis),
                nivelColaboracion = nivelCol,
                feedbackCadena = cadena,
                feedbackProveedor = proveedor,
                veredicto = veredicto
            };

        private static KPIData K(TipoKPI tipo, Direccion dir, string desc) =>
            new KPIData { tipo = tipo, direccion = dir, valorDescriptivo = desc };

        private static void Save(EstacionData e, string nombre)
        {
            AssetDatabase.CreateAsset(e, $"{CarpetaDestino}/{nombre}.asset");
        }

        private static void EnsureDir(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}
#endif
