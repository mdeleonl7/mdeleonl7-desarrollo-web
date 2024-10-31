namespace APLICACION_RRHH.EntidadDto
{
    public class SpNominaDtp
    {

        public int CodEmpleado { get; set; }

        public string? PeriodoTrabajando { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal SueldoBase { get; set; }


        public decimal BonificacionIncentivo { get; set; }

        public List<DetalleNominaDto>? detalleNominaDtos { get; set; }


    }
}
