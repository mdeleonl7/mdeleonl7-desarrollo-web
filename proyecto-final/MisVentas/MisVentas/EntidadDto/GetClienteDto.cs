namespace MisVentas.EntidadDto
{
    public class GetClienteDto
    {
        public int CodigoCliente { get; set; }
        public string? NombresCliente { get; set; }
        public string? ApellidosCliente { get; set; }
        public string? NIT { get; set; }
        public string? DireccionCliente { get; set; }
        public string? CategoriaCliente { get; set; }
        public byte EstadoCliente { get; set; }
    }
}
