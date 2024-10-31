using APLICACION_RRHH.EntidadDto;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace APLICACION_RRHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MunicipioController : Controller
    {
        private readonly string? conexionOracle;

        public MunicipioController(IConfiguration conexion)
        {
            conexionOracle = conexion.GetConnectionString("conexion");
        }

        [HttpGet("{codigoDepartamento}")]
        public async Task<IActionResult> GetMunicipiosPorDepartamento(int codigoDepartamento)
        {
            var resultados = new List<MunicipioDto>();

            try
            {
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();

                    using (var comando = new OracleCommand("ObtenerMunicipiosPorDepartamento", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        comando.Parameters.Add("p_CodigoDepartamento", OracleDbType.Int32).Value = codigoDepartamento;

                        // Parámetro de salida (cursor)
                        comando.Parameters.Add("cur_Municipios", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (var reader = await comando.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                resultados.Add(new MunicipioDto
                                {
                                    CodigoMunicipio = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("CODIGO_MUNICIPIO"))),
                                    Nombre = reader.GetString(reader.GetOrdinal("NOMBRE")),
                                    CodigoDepartamento = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("CODIGO_DEPARTAMENTO")))
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
