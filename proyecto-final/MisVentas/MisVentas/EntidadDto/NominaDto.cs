namespace APLICACION_RRHH.Entidad
{
    public class NominaDto
    {
        public int IdNomina { get; set; }

        public int CodEmpleado { get; set; }

        public string? PeriodoTrabajando { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal SueldoBase { get; set; }

        public decimal SueldoNeto { get; set; }

        public decimal BonificacionIncentivo { get; set; }

        public decimal Igss { get; set; }

        public int CodigoPuesto { get; set; }
        public decimal Descuento { get; set; }
        public string? DescripcionPuesto { get; set; }
    }
}
