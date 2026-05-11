namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Dirección visual de un delta del HUD. Determina la flecha que se pinta:
    /// ▲ Sube · ▼ Baja · → Igual.
    /// </summary>
    public enum DeltaDir { Sube, Baja, Igual }

    /// <summary>
    /// Identifica los 3 indicadores del HUD (slide 5).
    /// </summary>
    public enum IndicadorHud { OSA, INV, SOS }
}
