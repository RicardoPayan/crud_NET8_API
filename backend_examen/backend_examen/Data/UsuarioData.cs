using backend_examen.Models;
using System.Data;
using System.Data.SqlClient;
using BCrypt.Net;
namespace backend_examen.Data

{
    public class UsuarioData
    {
        private readonly string conexion;

        public UsuarioData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL");
        }

        public async Task<List<Usuario>> Lista()
        {
            List<Usuario> lista = new List<Usuario>();

            using (var conn = new SqlConnection(conexion))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spObtenerUsuarios", conn);
                cmd.CommandType = CommandType.Text;

                using (var reading = await cmd.ExecuteReaderAsync())
                {
                    while (await reading.ReadAsync())
                    {
                        lista.Add(new Usuario
                        {
                            idUsuario = Convert.ToInt32(reading["id"]),
                            Nombre = reading["nombre"].ToString(),
                            Direccion = reading["direccion"].ToString(),
                            Telefono = reading["telefono"].ToString(),
                            CodigoPostal = reading["codigoPostal"].ToString(),
                            Tipo = Convert.ToInt32(reading["tipo"]),
                            Estado = reading["estado"].ToString(),
                            Ciudad  = reading["ciudad"].ToString(),
                            Login = reading["login"].ToString(),
                            Password = reading["password"].ToString(),

                        });
                    }
                }
            }
            return lista;
        }


        public async Task<Usuario> Obtener(int Id)
        {
            Usuario objeto = new Usuario();

            using (var conn = new SqlConnection(conexion))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("ObtenerUsuarioPorID", conn);
                cmd.Parameters.AddWithValue("@UserID", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reading = await cmd.ExecuteReaderAsync())
                {
                    while (await reading.ReadAsync())
                    {
                        objeto = new Usuario
                        {
                            idUsuario = Convert.ToInt32(reading["id"]),
                            Nombre = reading["nombre"].ToString(),
                            Direccion = reading["direccion"].ToString(),
                            Telefono = reading["telefono"].ToString(),
                            CodigoPostal = reading["codigoPostal"].ToString(),
                            Tipo = Convert.ToInt32(reading["tipo"]),
                            Estado = reading["estado"].ToString(),
                            Ciudad = reading["ciudad"].ToString(),
                            Login = reading["login"].ToString(),
                            Password = reading["password"].ToString(),

                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Usuario objeto)
        {
            bool respuesta = true;

            //Encriptar contraseña
            objeto.Password = BCrypt.Net.BCrypt.HashPassword(objeto.Password);

            using (var conn = new SqlConnection(conexion))
            {
                
                SqlCommand cmd = new SqlCommand("spAgregarUsuario", conn);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);
                cmd.Parameters.AddWithValue("@telefono", objeto.Telefono);
                cmd.Parameters.AddWithValue("@codigoPostal", objeto.CodigoPostal);
                cmd.Parameters.AddWithValue("@tipo", objeto.Tipo);
                cmd.Parameters.AddWithValue("@estado", objeto.Estado);
                cmd.Parameters.AddWithValue("@ciudad", objeto.Ciudad);
                cmd.Parameters.AddWithValue("@login", objeto.Login);
                cmd.Parameters.AddWithValue("@password", objeto.Password);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }

                
            }
            return respuesta;
        }


        public async Task<bool> Editar(Usuario objeto)
        {
            bool respuesta = true;

            //Encriptar contraseña
            objeto.Password = BCrypt.Net.BCrypt.HashPassword(objeto.Password);

            using (var conn = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("spActualizarUsuario", conn);
                cmd.Parameters.AddWithValue("@id", objeto.idUsuario);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);
                cmd.Parameters.AddWithValue("@telefono", objeto.Telefono);
                cmd.Parameters.AddWithValue("@codigoPostal", objeto.CodigoPostal);
                cmd.Parameters.AddWithValue("@tipo", objeto.Tipo);
                cmd.Parameters.AddWithValue("@estado", objeto.Estado);
                cmd.Parameters.AddWithValue("@ciudad", objeto.Ciudad);
                cmd.Parameters.AddWithValue("@login", objeto.Login);
                cmd.Parameters.AddWithValue("@password", objeto.Password);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conn.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }


            }
            return respuesta;
        }


        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;

            using (var conn = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("spEliminarUsuario", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                

                try
                {
                    await conn.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }


            }
            return respuesta;
        }

        public async Task<bool> VerificarUsuarioLogin(string login, string password)
        {
            string passwordEncriptada = null;

            using (var conn = new SqlConnection(conexion))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("spObtenerPasswordPorLogin", conn);
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        passwordEncriptada = reader["Password"].ToString();
                    }
                }
            }

            if (string.IsNullOrEmpty(passwordEncriptada))
            {
                return false; // Usuario no encontrado
            }

            return BCrypt.Net.BCrypt.Verify(password, passwordEncriptada);
        }

    }
}
