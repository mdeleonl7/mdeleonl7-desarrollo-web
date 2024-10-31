using MisVentas.EntidadDto;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using MySqlX.XDevAPI;

namespace MisVentas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private readonly string? conexionMySql;

        public ClienteController(IConfiguration conexion)
        {
            conexionMySql = conexion.GetConnectionString("conexion");
        }

        [HttpPost]
        public async Task<IActionResult> PostCliente(ClienteDto cliente)
        {
            try
            {
                string querySql = "INSERT INTO cliente (NombresCliente, ApellidosCliente, NIT, DireccionCliente, CategoriaCliente) " +
                                  "VALUES (@Nombre, @Apellido, @nit, @direccion_cliente, @categoria_cliente)";

                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();

                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", cliente.NombresCliente);
                        command.Parameters.AddWithValue("@Apellido", cliente.ApellidosCliente);
                        command.Parameters.AddWithValue("@nit", cliente.NIT);
                        command.Parameters.AddWithValue("@direccion_cliente", cliente.DireccionCliente);
                        command.Parameters.AddWithValue("@categoria_cliente", cliente.CategoriaCliente);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { message = "Cliente creado exitosamente" });
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

        [HttpPut("{idCliente}")]
        public async Task<IActionResult> UpdateCliente(ClienteDto cliente, int idCliente)
        {
            string querySql = "UPDATE cliente SET " +
                "NombresCliente = @Nombre, " +
                "ApellidosCliente = @Apellido, " +
                "NIT = @nit, " +
                "DireccionCliente = @direccion_cliente, " +
                "CategoriaCliente = @categoria_cliente, " +
                "EstadoCliente = @estado_cliente " +
                "WHERE CodigoCliente = @idCliente";

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", cliente.NombresCliente);
                        command.Parameters.AddWithValue("@Apellido", cliente.ApellidosCliente);
                        command.Parameters.AddWithValue("@nit", cliente.NIT);
                        command.Parameters.AddWithValue("@direccion_cliente", cliente.DireccionCliente);
                        command.Parameters.AddWithValue("@categoria_cliente", cliente.CategoriaCliente);
                        command.Parameters.AddWithValue("@estado_cliente", cliente.EstadoCliente);
                        command.Parameters.AddWithValue("@idCliente", idCliente);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Cliente actualizado exitosamente" });
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


        [HttpDelete("{idCliente}")]
        public async Task<IActionResult> DeleteCliente(int idCliente)
        {
            string querySql = "UPDATE cliente SET " +
                "EstadoCliente = @Estado " +
                "WHERE CodigoCliente = @idCliente";

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Estado", 0);
                        command.Parameters.AddWithValue("@idCliente", idCliente);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Cliente actualizado exitosamente" });
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
        public async Task<IActionResult> GetClientes()
        {
            string querySql = "SELECT CodigoCliente, NombresCliente, ApellidosCliente, NIT, DireccionCliente, CategoriaCliente, EstadoCliente FROM cliente";

            var resultados = new List<GetClienteDto>();

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
                                resultados.Add(new GetClienteDto
                                {
                                    CodigoCliente = reader.IsDBNull("CodigoCliente") ? 0 : reader.GetInt32("CodigoCliente"),
                                    NombresCliente = reader.IsDBNull("NombresCliente") ? string.Empty : reader.GetString("NombresCliente"),
                                    ApellidosCliente = reader.IsDBNull("ApellidosCliente") ? string.Empty : reader.GetString("ApellidosCliente"),
                                    NIT = reader.IsDBNull("NIT") ? string.Empty : reader.GetString("NIT"),
                                    DireccionCliente = reader.IsDBNull("DireccionCliente") ? string.Empty : reader.GetString("DireccionCliente"),
                                    CategoriaCliente = reader.IsDBNull("CategoriaCliente") ? string.Empty : reader.GetString("CategoriaCliente"),
                                    EstadoCliente = reader.IsDBNull("EstadoCliente") ? (byte)0 : (reader.GetBoolean("EstadoCliente") ? (byte)1 : (byte)0)
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
