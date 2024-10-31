namespace APLICACION_RRHH.EntidadDto
{
    public class GetEmpladoDto
    {
        public int? CodigoEmpleado { get; set; }
        public string? Nombre { get; set; }
        public string? CorreoElectronico { get; set; }
        public int CodigoMunicipio { get; set; }
        public string? NombreMunicipio { get; set; }
        public string? TelefonoCasa { get; set; }
        public long Dpi { get; set; }
        public string? TelefonoPersonal { get; set; }
        public int CodigoPuesto { get; set; }
        public string? DescripcionPuesto { get; set; }
        public decimal Salario { get; set; }
        public int? CodJefeInmediato { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
