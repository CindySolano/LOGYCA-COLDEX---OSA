namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Helpers de presentación: nombre legible de KPI, categoría, regla de
    /// interpretación (sube = bueno/malo), niveles de colaboración, etc.
    /// </summary>
    public static class ConfiguracionCatalogo
    {
        // --------- KPI ---------

        public static bool EsFavorableSubir(TipoKPI tipo) => tipo switch
        {
            TipoKPI.DisponibilidadGondola or
            TipoKPI.EspacioGondola or
            TipoKPI.Ventas or
            TipoKPI.MargenGanancia or
            TipoKPI.SatisfaccionShopper => true,
            _ => false
        };

        public static string NombreLegible(TipoKPI tipo) => tipo switch
        {
            TipoKPI.DisponibilidadGondola => "Disponibilidad en góndola",
            TipoKPI.DiasInventario        => "Días de inventario",
            TipoKPI.EspacioGondola        => "Espacio en góndola",
            TipoKPI.TiempoEntrega         => "Tiempo de entrega",
            TipoKPI.CostoTransporte       => "Costo de transporte",
            TipoKPI.CostoOperativo        => "Costo operativo",
            TipoKPI.Ventas                => "Ventas",
            TipoKPI.MargenGanancia        => "Margen de ganancia",
            TipoKPI.SatisfaccionShopper   => "Satisfacción del shopper",
            _ => tipo.ToString()
        };

        public static CategoriaKPI Categoria(TipoKPI tipo)
        {
            if (tipo == TipoKPI.Ventas || tipo == TipoKPI.MargenGanancia) return CategoriaKPI.Bolsillo;
            if (tipo == TipoKPI.SatisfaccionShopper) return CategoriaKPI.Cliente;
            return CategoriaKPI.Operativo;
        }

        // --------- Colaboración ---------

        public static (string nombre, string descripcion) NivelColaboracion(int nivel) => nivel switch
        {
            1 => ("Cada quien por su lado", "Decides solo, el proveedor solo recibe el pedido"),
            2 => ("Coordinación básica",    "Negocian un cambio puntual, pero cada uno sigue por su cuenta"),
            3 => ("Datos compartidos",      "Le abres tus números al proveedor para que decida mejor"),
            4 => ("Gestión conjunta",       "Comparten datos, decisiones, riesgos y beneficios"),
            _ => ("", "")
        };

        public static string PerfilDecisor(float promedio)
        {
            if (promedio <= 1.5f) return "Lobo solitario";
            if (promedio <= 2.5f) return "Operador clásico";
            if (promedio <= 3.4f) return "Colaborador";
            return "Jugador en equipo";
        }

        // --------- HUD ---------

        public static string NombreIndicador(IndicadorHud i) => i switch
        {
            IndicadorHud.OSA => "OSA",
            IndicadorHud.INV => "INV",
            IndicadorHud.SOS => "SOS",
            _ => i.ToString()
        };

        public static string NombreLargoIndicador(IndicadorHud i) => i switch
        {
            IndicadorHud.OSA => "On Shelf Availability",
            IndicadorHud.INV => "Inventarios",
            IndicadorHud.SOS => "Share of Shelf",
            _ => i.ToString()
        };

        public static string FlechaDelta(DeltaDir dir) => dir switch
        {
            DeltaDir.Sube => "▲",
            DeltaDir.Baja => "▼",
            _ => "→"
        };

        public static string FormatoDeltaPct(int delta)
        {
            if (delta > 0) return $"+{delta}%";
            if (delta < 0) return $"{delta}%";
            return "→ 0";
        }
    }
}
