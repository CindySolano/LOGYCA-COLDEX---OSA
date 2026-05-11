namespace LOGYCA.OSA.Core
{
    /// <summary>
    /// Estados visibles del flujo. "Decision" no existe como estado propio:
    /// los 3 botones viven dentro del Panel_Situation y se re-muestran
    /// (sin animación de entrada) cuando el usuario pulsa "Probar otra".
    /// </summary>
    public enum AppState
    {
        Attract,
        MapSelection,
        Situation,
        Feedback,
        Summary
    }
}
