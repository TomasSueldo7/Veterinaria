using Grupo4_PAVI_Veterinaria.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo4_PAVI_Veterinaria.Datos
{
    internal class PerrosBD
    {
        public static DataTable ObtenerGrilla()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();

                //string consulta1 = "SELECT Nro_HC, Nombre, Fecha_nacimiento, Id_raza, Id_owner, Peso, Altura FROM Perros";

                string consulta = "SELECT P.Nro_HC, P.Nombre, P.Fecha_nacimiento, R.Denominacion, D.Nombre as Dueño, P.Peso, P.Altura " +
                   "FROM Perros P JOIN Dueños D ON P.Id_owner = D.Id_dueño JOIN Razas R ON P.Id_raza = R.Id_raza";

                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;

                DataTable tabla = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                return tabla;
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

        public static DataTable ObtenerRazas()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();


                string consulta = "SELECT * FROM Razas";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;

                DataTable tabla = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                return tabla;
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

        public static DataTable ObtenerDueños()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();


                string consulta = "SELECT * FROM Dueños";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;

                DataTable tabla = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                return tabla;
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

        public static bool AgregarPerroBD(Perro per)
        {
            //FALTA VALIDAR QUE NO EXISTA EL USUARIO
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();

                string consulta = "INSERT INTO Perros (Nombre, Fecha_nacimiento, Id_raza, Id_owner, Peso, Altura) " +
                    "VALUES (@nombre, @fechaNac, @idRaza, @idDueño, @peso, @altura)";
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("nombre", per.Nombre);
                cmd.Parameters.AddWithValue("fechaNac", per.FechaNacimiento);
                cmd.Parameters.AddWithValue("idRaza", per.Id_raza);
                cmd.Parameters.AddWithValue("idDueño", per.Id_dueño);
                cmd.Parameters.AddWithValue("peso", per.Peso);
                cmd.Parameters.AddWithValue("altura", per.Altura);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                resultado = true;
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

        public static Perro ObtenerPerro(string nroHC)
        {
            Perro p = new Perro();
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();


                string consulta = "SELECT * FROM Perros WHERE Nro_HC LIKE @nroHC";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nroHC", nroHC);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr != null && dr.Read())
                {
                    p.Nro_HC = int.Parse(dr["Nro_HC"].ToString());
                    p.Nombre = dr["Nombre"].ToString();
                    p.FechaNacimiento = DateTime.Parse(dr["Fecha_nacimiento"].ToString());
                    p.Peso = float.Parse(dr["Peso"].ToString());
                    p.Altura = float.Parse(dr["Altura"].ToString());
                    p.Id_dueño = int.Parse(dr["Id_owner"].ToString());
                    p.Id_raza = int.Parse(dr["Id_raza"].ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }
            return p;
        }

        public static bool ActualizarPerro(Perro p)
        {

            //FALTA VALIDAR QUE NO EXISTA EL USUARIO
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();


                string actualizacion = "UPDATE Perros SET Nombre = @nombre, Fecha_nacimiento = @fechaNac, Id_raza = @idRaza, Id_owner = @idDueño, Peso = @peso, " +
                    "Altura = @altura WHERE Nro_HC LIKE @nroHC";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("nroHC", p.Nro_HC);
                cmd.Parameters.AddWithValue("nombre", p.Nombre);
                cmd.Parameters.AddWithValue("fechaNac", p.FechaNacimiento);
                cmd.Parameters.AddWithValue("idRaza", p.Id_raza);
                cmd.Parameters.AddWithValue("idDueño", p.Id_dueño);
                cmd.Parameters.AddWithValue("peso", p.Peso);
                cmd.Parameters.AddWithValue("altura", p.Altura);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = actualizacion;

                cn.Open();
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                resultado = true;
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
