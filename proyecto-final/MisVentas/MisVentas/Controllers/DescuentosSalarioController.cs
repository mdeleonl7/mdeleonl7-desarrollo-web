using APLICACION_RRHH.EntidadDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace APLICACION_RRHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DescuentosSalarioController : Controller
    {
        private readonly string? conexionOracle;

        public DescuentosSalarioController(IConfiguration conexion)
        {
            conexionOracle = conexion.GetConnectionString("conexion");
        }

        [HttpGet]
        public async Task<IActionResult> GetDescuentosSalario()
        {
            var resultados = new List<DescuentoSalarioDto>();

            try
            {
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();

                    // Configura el comando para llamar al procedimiento almacenado
                    using (var comando = new OracleCommand("ObtenerDescuentosSalario", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        // Parámetro de salida: cursor
                        comando.Parameters.Add("cur_DescuentosSalario", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        // Ejecuta el procedimiento y obtén el cursor
                        using (var reader = await comando.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Lee los datos del cursor y los asigna al DTO
                                resultados.Add(new DescuentoSalarioDto
                                {
                                    IdDescuentoSalario = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("ID_DESCUENTO_SALARIO"))),
                                    DescripcionDescuento = reader.GetString(reader.GetOrdinal("DESCRIPCION_DESCUENTO"))
                                });
                            }
                        }
                    }
                }
                return Ok(resultados);
            }
            catch (OracleException oe)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error en la base de datos Oracle",
                    error = oe.Message
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Ocurrió un error en el servidor",
                    error = e.Message
                });
            }
        }



    }
}
