using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using APLICACION_RRHH.EntidadDto;

namespace APLICACION_RRHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : Controller
    {
        private readonly string? conexionOracle;

        public DepartamentoController(IConfiguration conexion)
        {
            conexionOracle = conexion.GetConnectionString("conexion");
        }


        [HttpGet]
        public async Task<IActionResult> GetDepartamentos()
        {
            var resultados = new List<DepartamentoDto>();

            try
            {
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();

                    // Configura el comando para llamar al procedimiento almacenado
                    using (var comando = new OracleCommand("ObtenerDepartamentos", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        // Parámetro de salida: cursor
                        comando.Parameters.Add("cur_Departamentos", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        // Ejecuta el procedimiento y obtén el cursor
                        using (var reader = await comando.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Lee los datos del cursor y los asigna al DTO
                                resultados.Add(new DepartamentoDto
                                {
                                    CodigoDepartamento = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("CODIGO_DEPARTAMENTO"))),
                                    Nombre = reader.GetString(reader.GetOrdinal("NOMBRE"))
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
