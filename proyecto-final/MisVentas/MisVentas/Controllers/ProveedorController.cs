using MisVentas.EntidadDto;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace MisVentas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProveedorController : Controller
    {
        private readonly string? conexionMySql;

        public ProveedorController(IConfiguration conexion)
        {
            conexionMySql = conexion.GetConnectionString("conexion");
        }

        [HttpPost]
        public async Task<IActionResult> PostProveedor(ProveedorDto proveedor)
        {
            try
            {
                string querySql = "INSERT INTO proveedor (NombreProveedor, TelefonoProveedor, DireccionProveedor) " +
                                    "VALUES (@Nombre, @Telefono, @Direccion)";

                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();

                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", proveedor.NombreProveedor);
                        command.Parameters.AddWithValue("@Telefono", proveedor.TelefonoProveedor);
                        command.Parameters.AddWithValue("@Direccion", proveedor.DireccionProveedor);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { message = "Proveedor creado exitosamente" });
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

        [HttpPut("{idProveedor}")]
        public async Task<IActionResult> UpdateProveedor(ProveedorDto Proveedor, int idProveedor)
        {
            string querySql = "UPDATE Proveedor SET " +
                "NombreProveedor = @Nombre, " +
                "TelefonoProveedor = @Telefono, " +
                "DireccionProveedor = @Direccion, " +
                "EstadoProveedor = @Estado " +
                "WHERE CodigoProveedor = @idProveedor";

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", Proveedor.NombreProveedor);
                        command.Parameters.AddWithValue("@Telefono", Proveedor.TelefonoProveedor);
                        command.Parameters.AddWithValue("@Direccion", Proveedor.DireccionProveedor);
                        command.Parameters.AddWithValue("@Estado", Proveedor.EstadoProveedor);
                        command.Parameters.AddWithValue("@idProveedor", idProveedor);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Proveedor actualizado exitosamente" });
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


        [HttpDelete("{idProveedor}")]
        public async Task<IActionResult> DeleteProveedor(int idProveedor)
        {
            string querySql = "UPDATE Proveedor SET " +
                "EstadoProveedor = @Estado " +
                "WHERE CodigoProveedor = @idProveedor";

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Estado", 0);
                        command.Parameters.AddWithValue("@idProveedor", idProveedor);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Proveedor eliminado exitosamente" });
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
        public async Task<IActionResult> GetProveedores()
        {
            string querySql = "SELECT CodigoProveedor, NombreProveedor, TelefonoProveedor, DireccionProveedor, EstadoProveedor FROM Proveedor";

            var resultados = new List<GetProveedorDto>();

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
                                resultados.Add(new GetProveedorDto
                                {
                                    CodigoProveedor = reader.IsDBNull("CodigoProveedor") ? 0 : reader.GetInt32("CodigoProveedor"),
                                    NombreProveedor = reader.IsDBNull("NombreProveedor") ? string.Empty : reader.GetString("NombreProveedor"),
                                    TelefonoProveedor = reader.IsDBNull("TelefonoProveedor") ? string.Empty : reader.GetString("TelefonoProveedor"),
                                    DireccionProveedor = reader.IsDBNull("DireccionProveedor") ? string.Empty : reader.GetString("DireccionProveedor"),
                                    EstadoProveedor = reader.IsDBNull("EstadoProveedor") ? (byte)0 : (reader.GetBoolean("EstadoProveedor") ? (byte)1 : (byte)0)
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
