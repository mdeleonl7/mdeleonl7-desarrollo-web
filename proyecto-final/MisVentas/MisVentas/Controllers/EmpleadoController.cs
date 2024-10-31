using APLICACION_RRHH.Entidad;
using APLICACION_RRHH.EntidadDto;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace APLICACION_RRHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpleadoController : Controller
    {
        private readonly string? conexionMySql;

        public EmpleadoController(IConfiguration conexion)
        {
            conexionMySql = conexion.GetConnectionString("conexion");
        }

        [HttpPost]
        public async Task<IActionResult> PostEmpleado(EmpleadoDto empleado)
        {
            try
            {
                string querySql = "INSERT INTO cliente (NombresCliente, ApellidosCliente, NIT, DireccionCliente, CategoriaCliente, EstadoCliente) " +
                                  "VALUES (@Nombre, @Apellido, @nit, @direccion_cliente, @categoria_cliente, @estado_cliente)";

                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();

                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", empleado.NombresCliente);
                        command.Parameters.AddWithValue("@Apellido", empleado.ApellidosCliente);
                        command.Parameters.AddWithValue("@nit", empleado.NIT);
                        command.Parameters.AddWithValue("@direccion_cliente", empleado.DireccionCliente);
                        command.Parameters.AddWithValue("@categoria_cliente", empleado.CategoriaCliente);
                        command.Parameters.AddWithValue("@estado_cliente", empleado.EstadoCliente);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { message = "Empleado creado exitosamente" });
            }
            catch (MySqlException me)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error al interactuar con la base de datos MySQL",
                    error = me.Message
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

        [HttpPut("{idEmpleado}")]
        public async Task<IActionResult> UpdateEmpleado(EmpleadoDto empleado, int idEmpleado)
        {
            string querySql = "UPDATE empleado SET " +
                "nombre = @nombre, " +
                "correo_electronico = @correo, " +
                "codigo_municipio = @codMunicipio, " +
                "telefono_casa = @telCasa, " +
                "dpi = @dpi, " +
                "telefono_personal = @telPersonal, " +
                "codigo_puesto = @codPuesto, " +
                "salario = @salario, " +
                "cod_jefe_inmediato = @codJefe, " +
                "fecha_nacimiento = @fechaNacimiento " +
                "WHERE codigo_empleado = @codEmpleado";
            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@nombre", empleado.Nombre);
                        command.Parameters.AddWithValue("@correo", empleado.CorreoElectronico);
                        command.Parameters.AddWithValue("@codMunicipio", empleado.CodigoMunicipio);
                        command.Parameters.AddWithValue("@telCasa", empleado.TelefonoCasa);
                        command.Parameters.AddWithValue("@dpi", empleado.Dpi);
                        command.Parameters.AddWithValue("@telPersonal", empleado.TelefonoPersonal);
                        command.Parameters.AddWithValue("@codPuesto", empleado.CodigoPuesto);
                        command.Parameters.AddWithValue("@salario", empleado.Salario);
                        command.Parameters.AddWithValue("@codJefe", empleado.CodJefeInmediato);
                        command.Parameters.AddWithValue("@fechaNacimiento", empleado.FechaNacimiento.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@codEmpleado", idEmpleado);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Empleado actualizado exitosamente" });
            }
            catch (MySqlException me)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error al interactuar con la base de datos MySQL",
                    error = me.Message
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

        [HttpGet]
        public async Task<IActionResult> GetEmpleados()
        {
            string querySql = @"
                SELECT e.codigo_empleado, e.nombre, e.correo_electronico, e.codigo_municipio, m.nombre AS NombreMunicipio,
                       e.telefono_casa, e.dpi, e.telefono_personal, e.codigo_puesto, pt.descripcion_puesto, 
                       e.salario, e.cod_jefe_inmediato, e.fecha_nacimiento, e.fecha_ingreso
                FROM empleado e
                INNER JOIN puestos_trabajo pt ON pt.codigo_puesto = e.codigo_puesto
                INNER JOIN municipio m ON m.codigo_municipio = e.codigo_municipio
                ORDER BY e.codigo_empleado";

            var resultados = new List<GetEmpladoDto>();

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();

                    using (var comando = new MySqlCommand(querySql, conexion))
                    {
                        using (var reader = await comando.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                resultados.Add(new GetEmpladoDto
                                {
                                    CodigoEmpleado = reader.IsDBNull("codigo_empleado") ? 0 : reader.GetInt32("codigo_empleado"),
                                    Nombre = reader.IsDBNull("nombre") ? "string" : reader.GetString("nombre"),
                                    CorreoElectronico = reader.IsDBNull("correo_electronico") ? null : reader.GetString("correo_electronico"),
                                    CodigoMunicipio = reader.IsDBNull("codigo_municipio") ? 0 : reader.GetInt32("codigo_municipio"),
                                    NombreMunicipio = reader.IsDBNull("NombreMunicipio") ? null : reader.GetString("NombreMunicipio"),
                                    TelefonoCasa = reader.IsDBNull("telefono_casa") ? null : reader.GetString("telefono_casa"),
                                    Dpi = reader.IsDBNull("dpi") ? 0 : reader.GetInt64("dpi"),
                                    TelefonoPersonal = reader.IsDBNull("telefono_personal") ? null : reader.GetString("telefono_personal"),
                                    CodigoPuesto = reader.IsDBNull("codigo_puesto") ? 0 : reader.GetInt32("codigo_puesto"),
                                    DescripcionPuesto = reader.IsDBNull("descripcion_puesto") ? null : reader.GetString("descripcion_puesto"),
                                    Salario = reader.GetDecimal("salario"),
                                    CodJefeInmediato = reader.IsDBNull("cod_jefe_inmediato") ? 0 : reader.GetInt32("cod_jefe_inmediato"),
                                    FechaNacimiento = reader.IsDBNull("fecha_nacimiento") ? DateTime.Now : reader.GetDateTime("fecha_nacimiento"),
                                    FechaIngreso = reader.IsDBNull("fecha_ingreso") ? DateTime.Now : reader.GetDateTime("fecha_ingreso")
                                });
                            }
                        }
                    }
                }

                return Ok(resultados);
            }
            catch (MySqlException me)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error en la base de datos MySQL",
                    error = me.Message
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
