using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo4_PAVI_Veterinaria.Datos
{
    internal class UsuariosBD
    {
        public static bool ValidarUsuario(string nombreDeUsuario, string password)
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                bool resultado = false;
                SqlCommand cmd = new SqlCommand();


                string consulta = "SELECT * FROM Usuarios WHERE Usuario LIKE @nombreDeUsuario AND Contra LIKE @password";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("nombreDeUsuario", nombreDeUsuario);
                cmd.Parameters.AddWithValue("password", password);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;

                DataTable tabla = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                if (tabla.Rows.Count == 1)
                {
                    resultado = true;
                }

                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }

        }
    }
}
