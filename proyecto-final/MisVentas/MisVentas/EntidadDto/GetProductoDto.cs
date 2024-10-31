namespace MisVentas.EntidadDto
{
    public class GetProductoDto
    {
        public int CodigoProducto { get; set; }
        public string? Descripcion { get; set; }
        public int? CodigoProveedor { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string? UbicacionFisica { get; set; }
        public int? ExistenciaMinima { get; set; }
    }
}
