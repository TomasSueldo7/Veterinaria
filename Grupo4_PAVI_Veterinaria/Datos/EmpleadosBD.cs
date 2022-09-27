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
    internal class EmpleadosBD
    {
        public static DataTable ObtenerGrillaEmpleados()
        {
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {

                SqlCommand cmd = new SqlCommand();

                string consulta = "SELECT Nombre, Apellido, Nro_Doc " +
                    "FROM Empleados ";

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
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }

        }

        public static DataTable ObtenerTipoDoc()
        {

            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            try
            {

                SqlCommand cmd = new SqlCommand();

                string consulta = "SELECT * " +
                    "FROM Tipo_Doc";

                cmd.Parameters.Clear(); //Limpiar todos los tipos de parámetros que puede llegar a tener

                cmd.CommandType = CommandType.Text; //El commandtype es .text, es decir yo escribo a mano la consulta
                cmd.CommandText = consulta; //cual es la consulta q quiero ejecutar

                cn.Open();
                cmd.Connection = cn;

                DataTable tabla = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);

                return tabla;

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                cn.Close();
            }
        }
        public static bool EliminarEmpleadoABD(Empleado emp)
        {
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);

            try
            {
                SqlCommand cmd = new SqlCommand();
                string consulta = "DELETE FROM Empleados where Nro_Doc like @documento";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@documento", emp.DocumentoEmpleado);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                resultado = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cn.Close();
            }
            return resultado;
        }

        public static bool AgregarEmpleadoABD(Empleado emp)
        {
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);

            try
            {
                SqlCommand cmd = new SqlCommand();
                string consulta = "INSERT INTO Empleados(Nombre, Apellido, Fecha_nacimiento, Tipo_doc, Nro_Doc, Fecha_ingreso, Matricula, Activo ) VALUES(@nombre, @apellido, @fechanacimiento, @tipodoc, @nrodoc, @fechaingreso, @matricula, @activo) ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nombre", emp.NombreEmpleado);
                cmd.Parameters.AddWithValue("@apellido", emp.ApellidoEmpleado);
                cmd.Parameters.AddWithValue("@fechanacimiento", emp.FechaNacimientoEmpleado);
                cmd.Parameters.AddWithValue("@tipodoc", emp.IdTipoDocumentoEmpleado);
                cmd.Parameters.AddWithValue("@nrodoc", emp.DocumentoEmpleado);
                cmd.Parameters.AddWithValue("@fechaingreso", emp.FechaIngresoEmpleado);
                cmd.Parameters.AddWithValue("@matricula", emp.MatriculaEmpleado);
                cmd.Parameters.AddWithValue("@activo", emp.ActivoEmpleado);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                resultado = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cn.Close();
            }
            return resultado;
        }

        public static Empleado ObtenerEmpleado(string? documento)
        {
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);
            Empleado emp = new Empleado();

            try
            {
                SqlCommand cmd = new SqlCommand();
                string consulta = "SELECT * FROM Empleados where Nro_Doc like @documento";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@documento", documento);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;
                SqlDataReader dr = cmd.ExecuteReader(); //obtener resultado
                if (dr != null && dr.Read()) //pregunto si la consulta sql devolvio un resultado o no, el dr.read me dice que carga al dr con el
                                             //primer valor que tiene que leer del resultado de esa fila que me devolvio la consulta sql
                {
                    emp.NombreEmpleado = dr["Nombre"].ToString();
                    emp.ApellidoEmpleado = dr["Apellido"].ToString();
                    emp.FechaNacimientoEmpleado = DateTime.Parse(dr["Fecha_nacimiento"].ToString());
                    emp.IdTipoDocumentoEmpleado = int.Parse(dr["Tipo_doc"].ToString());
                    emp.DocumentoEmpleado = dr["Nro_Doc"].ToString();
                    emp.MatriculaEmpleado = dr["Matricula"].ToString();
                    emp.FechaIngresoEmpleado = DateTime.Parse(dr["Fecha_ingreso"].ToString());

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
            return emp;
        }

        public static bool ModificarEmpleado(Empleado emp)
        {
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);

            try
            {
                SqlCommand cmd = new SqlCommand();
                string consulta = " UPDATE Empleados SET Nombre = @nombre, Apellido = @apellido, Fecha_nacimiento = @fechanacimiento, Tipo_doc = @idtipodocumento, Nro_doc = @nrodocumento, Fecha_ingreso = @fechaingreso, Matricula = @matricula " +
                    "WHERE Nro_doc like @nrodocumento";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nombre", emp.NombreEmpleado);
                cmd.Parameters.AddWithValue("@apellido", emp.ApellidoEmpleado);
                cmd.Parameters.AddWithValue("@fechanacimiento", emp.FechaNacimientoEmpleado);
                cmd.Parameters.AddWithValue("@idtipodocumento", emp.IdTipoDocumentoEmpleado);
                cmd.Parameters.AddWithValue("@nrodocumento", emp.DocumentoEmpleado);
                cmd.Parameters.AddWithValue("@fechaingreso", emp.FechaIngresoEmpleado);
                cmd.Parameters.AddWithValue("@matricula", emp.MatriculaEmpleado);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = consulta;

                cn.Open();
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                resultado = true;
            }
            catch (Exception)
            {

                MessageBox.Show("Error al actualizar empleado");
            }
            finally
            {
                cn.Close();
            }




            return resultado;
        }
    }
}
