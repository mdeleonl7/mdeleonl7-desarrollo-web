namespace APLICACION_RRHH.EntidadDto
{
    public class PuestosTrabajoDto
    {
        public int CodigoPuesto { get; set; }
        public string? DescripcionPuesto { get; set; }
        public decimal SueldoMin { get; set; }
        public decimal SueldoMax { get; set; }
        public int CodigoDeptoTrabajo { get; set; }
    }
}
