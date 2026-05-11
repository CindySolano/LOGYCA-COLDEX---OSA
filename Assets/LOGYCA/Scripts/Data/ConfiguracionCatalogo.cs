namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Helpers de presentación. El modelo simplificado del HUD elimina los 9
    /// KPIs originales; aquí quedan solo helpers de colaboración (resumen) y
    /// formato de deltas.
    /// </summary>
    public static class ConfiguracionCatalogo
    {
        public static (string nombre, string descripcion) NivelColaboracion(int nivel)
        {
            switch (nivel)
            {
                case 1: return ("Cada quien por su lado",  "Decides solo, el proveedor solo recibe el pedido");
                case 2: return ("Coordinación básica",     "Negocian un cambio puntual, pero cada uno sigue por su cuenta");
                case 3: return ("Datos compartidos",       "Le abres tus números al proveedor para que decida mejor");
                case 4: return ("Gestión conjunta",        "Comparten datos, decisiones, riesgos y beneficios");
                default: return ("", "");
            }
        }

        public static string PerfilDecisor(float promedioColaboracion)
        {
            if (promedioColaboracion <= 1.5f) return "Lobo solitario";
            if (promedioColaboracion <= 2.5f) return "Operador clásico";
            if (promedioColaboracion <= 3.4f) return "Colaborador";
            return "Jugador en equipo";
        }

        public static string FormatoDeltaPct(int delta)
        {
            if (delta > 0) return $"+{delta}%";
            if (delta < 0) return $"{delta}%";
            return "→ 0";
        }

        public static string FlechaDelta(DeltaDir dir)
        {
            switch (dir)
            {
                case DeltaDir.Sube: return "▲";
                case DeltaDir.Baja: return "▼";
                default:            return "→";
            }
        }

        public static string NombreIndicador(IndicadorHud i)
        {
            switch (i)
            {
                case IndicadorHud.OSA: return "OSA";
                case IndicadorHud.INV: return "INV";
                case IndicadorHud.SOS: return "SOS";
                default: return i.ToString();
            }
        }

        public static string NombreLargoIndicador(IndicadorHud i)
        {
            switch (i)
            {
                case IndicadorHud.OSA: return "On Shelf Availability";
                case IndicadorHud.INV: return "Inventarios";
                case IndicadorHud.SOS: return "Share of Shelf";
                default: return i.ToString();
            }
        }
    }
}
