using APLICACION_RRHH.Entidad;
using APLICACION_RRHH.EntidadDto;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Swashbuckle.AspNetCore.Swagger;
using System.Data;

namespace APLICACION_RRHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NominaController : Controller
    {
        private readonly string? conexionOracle;

        public NominaController(IConfiguration conexion)
        {
            conexionOracle = conexion.GetConnectionString("conexion");
        }

        //consulta nomina
        [HttpGet]
        public async Task<IActionResult> GetNomina()
        {

            string querySql = "select n.id_nomina, n.cod_empleado, n.periodo_trabajando, n.fecha_pago, n.sueldo_base, n.sueldo_neto, " +
                             "n.bonificacion_incentivo, n.igss, n.codigo_puesto, " +
                             "((n.sueldo_base + n.bonificacion_incentivo - n.igss - n.sueldo_neto)) as Descuento, " +
                             "pt.descripcion_puesto " +
                             "from nomina n " +
                             "inner join puestos_trabajo pt on pt.codigo_puesto = n.codigo_puesto";


            var resultados = new List<NominaDto>();

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
                                // Se aseguran los tipos de dato esperados
                                resultados.Add(new NominaDto
                                {
                                    IdNomina = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("ID_NOMINA"))),
                                    CodEmpleado = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("COD_EMPLEADO"))),
                                    PeriodoTrabajando = reader.GetString(reader.GetOrdinal("PERIODO_TRABAJANDO")),
                                    FechaPago = reader.GetDateTime(reader.GetOrdinal("FECHA_PAGO")),
                                    SueldoBase = reader.GetDecimal(reader.GetOrdinal("SUELDO_BASE")),
                                    SueldoNeto = reader.GetDecimal(reader.GetOrdinal("SUELDO_NETO")),
                                    BonificacionIncentivo = reader.GetDecimal(reader.GetOrdinal("BONIFICACION_INCENTIVO")),
                                    Igss = reader.GetDecimal(reader.GetOrdinal("IGSS")),
                                    CodigoPuesto = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("CODIGO_PUESTO"))),
                                    Descuento = reader.GetDecimal(reader.GetOrdinal("Descuento")),
                                    DescripcionPuesto = reader.GetString(reader.GetOrdinal("DESCRIPCION_PUESTO"))
                                });
                            }
                        }
                    }
                }
                return Ok(resultados);
            }
            catch (OracleException oe) // Manejo específico para errores de Oracle
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
        //eliminacion de nomina 
        [HttpDelete]
        public async Task<IActionResult> DeleteNomina(int idNomina)
        {
            string queryDeleteDetalle = "delete detalle_nomina_descuentos where id_nomina = :id";
            string queryDeleteNomina = "delete nomina where id_nomina = :id";
            try
            {
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();


                    using (var comandoDetalle = new OracleCommand(queryDeleteDetalle, conexion))
                    {
                        comandoDetalle.Parameters.Add(new OracleParameter("id", idNomina));
                        await comandoDetalle.ExecuteNonQueryAsync();
                    }
                    await conexion.CloseAsync();
                }
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();

                    using (var comandoNomina = new OracleCommand(queryDeleteNomina, conexion))
                    {
                        comandoNomina.Parameters.Add(new OracleParameter("id", idNomina));
                        await comandoNomina.ExecuteNonQueryAsync();
                    }
                    await conexion.CloseAsync();
                }

                return Ok("Eliminado Con exito");
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

        //Registro de nomina 
        [HttpPost]
        public async Task<IActionResult> RegistroEncabezado(SpNominaDtp nominaDto)
        {
            string sqlQuery = "SP_INSERTAR_NOMINA";
            string sqlQueryDetNomina = "SP_INSERTAR_DETALLE_NOMINA_DESCUENTO";

            try
            {
                using (var conexion = new OracleConnection(conexionOracle))
                {
                    await conexion.OpenAsync();
                    OracleTransaction transaction = conexion.BeginTransaction();  // Iniciar transacción

                    try
                    {
                        // Insertar encabezado de nómina
                        using (var comando = new OracleCommand(sqlQuery, conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Transaction = transaction;

                            comando.Parameters.Add(new OracleParameter("P_COD_EMPLEADO", OracleDbType.Int32)).Value = nominaDto.CodEmpleado;
                            comando.Parameters.Add(new OracleParameter("P_PERIODO_TRABAJANDO", OracleDbType.Varchar2)).Value = nominaDto.PeriodoTrabajando;
                            comando.Parameters.Add(new OracleParameter("P_FECHA_PAGO", OracleDbType.Date)).Value = nominaDto.FechaPago;
                            comando.Parameters.Add(new OracleParameter("P_SUELDO_BASE", OracleDbType.Decimal)).Value = nominaDto.SueldoBase;
                            comando.Parameters.Add(new OracleParameter("P_BONIFICACION_INCENTIVO", OracleDbType.Decimal)).Value = nominaDto.BonificacionIncentivo;

                            await comando.ExecuteNonQueryAsync();
                        }
     
                        // Iterar sobre la lista de detalle de nómina para insertar cada registro
                        foreach (var detalle in nominaDto.detalleNominaDtos)
                        {
                            if (detalle.idDescuento != 0)
                            {
                                //para obtener el ultimo id nomina
                                string? queryIdNomina = "select n.id_nomina  from nomina n order by  id_nomina desc FETCH FIRST ROW ONLY";
                                int idNominaDet = 0;
                                using (var comandoId = new OracleCommand(queryIdNomina, conexion))
                                {
                                    using (var reader = await comandoId.ExecuteReaderAsync())
                                    {
                                        
                                        if (await reader.ReadAsync())
                                        {
                                            idNominaDet = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("ID_NOMINA")));
                                        }
                                    }
                                }

            
                                using (var comandoDet = new OracleCommand(sqlQueryDetNomina, conexion))
                                {
                                    comandoDet.CommandType = CommandType.StoredProcedure;
                                    comandoDet.Transaction = transaction;

                                    comandoDet.Parameters.Add(new OracleParameter("P_ID_NOMINA", OracleDbType.Int32)).Value = idNominaDet;
                                    comandoDet.Parameters.Add(new OracleParameter("P_ID_DESCUENTO_SALARIO", OracleDbType.Int32)).Value = detalle.idDescuento;
                                    comandoDet.Parameters.Add(new OracleParameter("P_CANTIDAD", OracleDbType.Decimal)).Value = detalle.cantidad;

                                    await comandoDet.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        transaction.Commit();
                        return Ok("Registro de nómina insertado correctamente con sus detalles");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return StatusCode(StatusCodes.Status500InternalServerError, new
                        {
                            message = "Error al insertar el registro de nómina",
                            error = ex.Message
                        });
                    }
                }
            }
            catch (OracleException oe)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error en la base de datos Oracle",
                    error = oe.Message
                });
            }


        }


    }
}
