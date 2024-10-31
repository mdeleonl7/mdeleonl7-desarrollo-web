using Microsoft.AspNetCore.Mvc;
using MisVentas.EntidadDto;
using MySql.Data.MySqlClient;
using System.Data;

namespace MisVentas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : Controller
    {
        private readonly string? conexionMySql;

        public ProductoController(IConfiguration conexion)
        {
            conexionMySql = conexion.GetConnectionString("conexion");
        }

        [HttpPost]
        public async Task<IActionResult> PostProducto(ProductoDto producto)
        {
            try
            {
                string querySql = "INSERT INTO producto (Descripcion, CodigoProveedor, FechaVencimiento, UbicacionFisica, ExistenciaMinima) " +
                                    "VALUES (@Nombre, @Codigo, @Fecha, @Ubicacion, @Existencia)";

                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();

                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", producto.Descripcion);
                        command.Parameters.AddWithValue("@Codigo", producto.CodigoProveedor);
                        command.Parameters.AddWithValue("@Fecha", producto.FechaVencimiento);
                        command.Parameters.AddWithValue("@Ubicacion", producto.UbicacionFisica);
                        command.Parameters.AddWithValue("@Existencia", producto.ExistenciaMinima);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { message = "Producto creado exitosamente" });
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

        [HttpPut("{idProducto}")]
        public async Task<IActionResult> UpdateProducto(ProductoDto Producto, int idProducto)
        {
            string querySql = "UPDATE Producto SET " +
                "Descripcion = @Nombre, " +
                "CodigoProveedor = @Codigo, " +
                "FechaVencimiento = @Fecha, " +
                "UbicacionFisica = @Ubicacion " +
                "ExistenciaMinima = @Existencia " +
                "WHERE CodigoProducto = @idProducto";

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", Producto.Descripcion);
                        command.Parameters.AddWithValue("@Codigo", Producto.CodigoProveedor);
                        command.Parameters.AddWithValue("@Fecha", Producto.FechaVencimiento);
                        command.Parameters.AddWithValue("@Ubicacion", Producto.UbicacionFisica);
                        command.Parameters.AddWithValue("@Existencia", Producto.ExistenciaMinima);
                        command.Parameters.AddWithValue("@idProducto", idProducto);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Producto actualizado exitosamente" });
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


        [HttpDelete("{idProducto}")]
        public async Task<IActionResult> DeleteProducto(int idProducto)
        {
            string querySql = "UPDATE Producto SET " +
                "EstadoProducto = @Estado " +
                "WHERE CodigoProducto = @idProducto";

            try
            {
                using (var conexion = new MySqlConnection(conexionMySql))
                {
                    await conexion.OpenAsync();
                    using (var command = new MySqlCommand(querySql, conexion))
                    {
                        command.Parameters.AddWithValue("@Estado", 0);
                        command.Parameters.AddWithValue("@idProducto", idProducto);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Ok(new { message = "Producto eliminado exitosamente" });
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
        public async Task<IActionResult> GetProductoes()
        {
            string querySql = "SELECT CodigoProducto, Descripcion, CodigoProveedor, FechaVencimiento, UbicacionFisica, ExistenciaMinima FROM Producto";

            var resultados = new List<GetProductoDto>();

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
                                resultados.Add(new GetProductoDto
                                {
                                    CodigoProducto = reader.IsDBNull("CodigoProducto") ? 0 : reader.GetInt32("CodigoProducto"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? string.Empty : reader.GetString("Descripcion"),
                                    CodigoProveedor = reader.IsDBNull("CodigoProveedor") ? 0 : reader.GetInt32("CodigoProveedor"),
                                    FechaVencimiento = reader.IsDBNull("FechaVencimiento") ? DateTime.Today : reader.GetDateTime("FechaVencimiento"),
                                    UbicacionFisica = reader.IsDBNull("UbicacionFisica") ? string.Empty : reader.GetString("UbicacionFisica"),
                                    ExistenciaMinima = reader.IsDBNull("ExistenciaMinima") ? 1 : reader.GetInt32("ExistenciaMinima")
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
