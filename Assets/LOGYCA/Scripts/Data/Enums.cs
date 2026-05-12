namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Dirección visual de un delta del HUD. ▲ Sube · ▼ Baja · → Igual.
    /// </summary>
    public enum DeltaDir { Sube, Baja, Igual }

    /// <summary>Los 3 indicadores siempre visibles (slide 5).</summary>
    public enum IndicadorHud { OSA, INV, SOS }

    /// <summary>Los 9 KPIs detallados del feedback (sección 5.2 del .md).</summary>
    public enum TipoKPI
    {
        DisponibilidadGondola,
        DiasInventario,
        EspacioGondola,
        TiempoEntrega,
        CostoTransporte,
        CostoOperativo,
        Ventas,
        MargenGanancia,
        SatisfaccionShopper
    }

    /// <summary>Dirección del KPI individual (separada de DeltaDir para no confundir).</summary>
    public enum Direccion
    {
        Sube,
        Baja,
        Igual,
        NoAplica
    }

    /// <summary>Bloques del grid de feedback (sección 7.5 del .md).</summary>
    public enum CategoriaKPI
    {
        Operativo,  // 6 KPIs
        Bolsillo,   // 2 KPIs (Ventas, Margen)
        Cliente     // 1 KPI (Shopper)
    }
}
