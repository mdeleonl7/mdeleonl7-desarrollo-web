using APLICACION_RRHH.EntidadDto;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace APLICACION_RRHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PuestoController : Controller
    {
        private readonly string? conexionOracle;

        public PuestoController(IConfiguration conexion)
        {
            conexionOracle = conexion.GetConnectionString("conexion");
        }
        // Consulta puestos de trabajo
        [HttpGet]
        public async Task<IActionResult> GetPuestosTrabajo()
        {
            string querySql = "SELECT CODIGO_PUESTO, DESCRIPCION_PUESTO, SUELDO_MIN, SUELDO_MAX, CODIGO_DEPTO_TRABAJO FROM PUESTOS_TRABAJO";

            var resultados = new List<PuestosTrabajoDto>();

            try
            {
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();

                    using (var comando = new OracleCommand(querySql, conexion))
                    {
                        using (var reader = await comando.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                resultados.Add(new PuestosTrabajoDto
                                {
                                    CodigoPuesto = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("CODIGO_PUESTO"))),
                                    DescripcionPuesto = reader.GetString(reader.GetOrdinal("DESCRIPCION_PUESTO")),
                                    SueldoMin = reader.GetDecimal(reader.GetOrdinal("SUELDO_MIN")),
                                    SueldoMax = reader.GetDecimal(reader.GetOrdinal("SUELDO_MAX")),
                                    CodigoDeptoTrabajo = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("CODIGO_DEPTO_TRABAJO")))
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
