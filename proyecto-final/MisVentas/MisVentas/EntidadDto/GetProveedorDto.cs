﻿namespace MisVentas.EntidadDto
{
    public class GetProveedorDto
    {
        public int CodigoProveedor { get; set; }
        public string? NombreProveedor { get; set; }
        public string? TelefonoProveedor { get; set; }
        public string? DireccionProveedor { get; set; }
        public byte EstadoProveedor { get; set; }
    }
}
