namespace LOGYCA.OSA.Data
{
    /// <summary>
    /// Estado mutable del HUD. No es MonoBehaviour: el GameManager lo posee y
    /// los HUDIndicator lo leen. Para cada decisión se llama AplicarOpcion()
    /// que mueve OSA/INV/SOS y guarda el valor anterior para que la barra
    /// pueda mostrar "antes 92%" (slide 5).
    /// </summary>
    public class HUDState
    {
        public int OsaActual { get; private set; }
        public int OsaAnterior { get; private set; }
        public string InvActual { get; private set; }
        public string InvAnterior { get; private set; }
        public int SosActual { get; private set; }
        public int SosAnterior { get; private set; }

        public DeltaDir UltimoOsaDir { get; private set; } = DeltaDir.Igual;
        public DeltaDir UltimoInvDir { get; private set; } = DeltaDir.Igual;
        public DeltaDir UltimoSosDir { get; private set; } = DeltaDir.Igual;

        public bool Actualizado { get; private set; }

        public HUDState(int osaIni, string invIni, int sosIni)
        {
            OsaActual = OsaAnterior = osaIni;
            InvActual = InvAnterior = invIni;
            SosActual = SosAnterior = sosIni;
        }

        public void AplicarOpcion(OpcionData op)
        {
            if (op == null) return;

            OsaAnterior = OsaActual;
            InvAnterior = InvActual;
            SosAnterior = SosActual;

            OsaActual = UnityEngine.Mathf.Clamp(OsaActual + op.deltaOsaPct, 0, 100);
            UltimoOsaDir = Direccion(op.deltaOsaPct);

            SosActual = UnityEngine.Mathf.Clamp(SosActual + op.deltaSosPct, 0, 100);
            UltimoSosDir = Direccion(op.deltaSosPct);

            UltimoInvDir = op.deltaInvDir;
            InvActual    = string.IsNullOrEmpty(op.deltaInvDescriptivo) ? InvActual : op.deltaInvDescriptivo;

            Actualizado = true;
        }

        public void Reset(int osaIni, string invIni, int sosIni)
        {
            OsaActual = OsaAnterior = osaIni;
            InvActual = InvAnterior = invIni;
            SosActual = SosAnterior = sosIni;
            UltimoOsaDir = UltimoInvDir = UltimoSosDir = DeltaDir.Igual;
            Actualizado = false;
        }

        private static DeltaDir Direccion(int delta)
        {
            if (delta > 0) return DeltaDir.Sube;
            if (delta < 0) return DeltaDir.Baja;
            return DeltaDir.Igual;
        }
    }
}
