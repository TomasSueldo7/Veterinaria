using Grupo4_PAVI_Veterinaria.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grupo4_PAVI_Veterinaria.formularios.abmcEmpleados
{
    public partial class NuevoEmpleado : Form
    {
        public NuevoEmpleado()
        {
            InitializeComponent();
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Empleado emp= ObtenerDatosEmpleado(); //Obtengo los datos de los txt
            bool resultado = AgregarEmpleadoABD(emp); //Los agrego a la BD
            if (resultado)
            {
                MessageBox.Show("Persona agregada con éxito");
                cargarGrillaEmpleados();
                LimpiarCampos();
                
            }
            else
            {
                MessageBox.Show("Error al agregar Persona");
            }
        }
        private Empleado ObtenerDatosEmpleado()
        {
            Empleado emp = new Empleado();
            emp.NombreEmpleado = txtNombre.Text.Trim();
            emp.ApellidoEmpleado = txtApellido.Text.Trim();
            emp.FechaNacimientoEmpleado = DateTime.Parse(txtFechaNacimiento.Text);
            emp.FechaIngresoEmpleado = DateTime.Parse(txtFechaIngreso.Text);
            emp.DocumentoEmpleado = txtNroDocumento.Text.Trim();
            emp.MatriculaEmpleado = txtMatricula.Text.Trim();
            emp.IdTipoDocumentoEmpleado = (int)cmbTipoDoc.SelectedValue;
            emp.ActivoEmpleado = 1;

            return emp;
        }

        private void NuevoEmpleado_Load(object sender, EventArgs e)
        {
            cargarComboTipoDoc();
            cargarGrillaEmpleados();
            btnModificar.Enabled = false;
            LimpiarCampos();
        }

        private void cargarGrillaEmpleados()
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

                gdrEmpleados.DataSource = tabla;


            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al cargar la grilla de Empleados");
            }
            finally
            {
                cn.Close();
            }

        }
    

        private void cargarComboTipoDoc()
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
                    cmbTipoDoc.DataSource = tabla;
                    cmbTipoDoc.DisplayMember = "Nombre"; //Cada opcion del combo me lo mostras de la columna Nombre
                    cmbTipoDoc.ValueMember = "Id_Tipo_doc"; //Cada opcion del combo su valor esta dado por su id
                    cmbTipoDoc.SelectedIndex = -1; //Deja por defecto nada seleccionado

            }
                catch (Exception ex)
                {

                MessageBox.Show("Error al cargar los tipos de Documentos");
                }
                finally
                {
                    cn.Close();
                }                
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtMatricula.Text = "";
            cmbTipoDoc.Text = "";
            txtNroDocumento.Text = "";
            txtFechaIngreso.Text = "";
            txtFechaNacimiento.Text = "";
            txtNombre.Focus();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
        private bool AgregarEmpleadoABD(Empleado emp)
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

                MessageBox.Show("Error al crear Usuario");
            }
            finally
            {
                cn.Close();
            }
            return resultado;
        }

        private void gdrEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex; //Me indica en que fila estoy parado
            btnModificar.Enabled = true;
            DataGridViewRow filaSeleccionada = gdrEmpleados.Rows[indice];
            string documento = filaSeleccionada.Cells["Documento"].Value.ToString();
            Empleado emp = ObtenerEmpleado(documento);
            LimpiarCampos();
            cargarEmpleado(emp);
            
        }

        private void cargarEmpleado(Empleado emp)
        {
            txtNombre.Text = emp.NombreEmpleado;
            txtApellido.Text = emp.ApellidoEmpleado;
            DateTime fecha = emp.FechaNacimientoEmpleado;
            string dia = "";
            string mes = "";
            string año = "";
            dia = fecha.Date.Day.ToString();
            if (dia.Length == 1)
            {
                dia = "0" + dia;
            }
            mes = fecha.Date.Month.ToString();
            if (mes.Length == 1)
            {
                mes = "0" + mes;
            }
            año = fecha.Date.Year.ToString();
            txtFechaNacimiento.Text = dia + mes + año;
            cmbTipoDoc.SelectedValue = emp.IdTipoDocumentoEmpleado;
            txtNroDocumento.Text = emp.DocumentoEmpleado;
            txtMatricula.Text = emp.MatriculaEmpleado;
            DateTime fecha2 = emp.FechaIngresoEmpleado;
            string dia2 = "";
            string mes2 = "";
            string año2 = "";
            dia2 = fecha2.Date.Day.ToString();
            if (dia2.Length == 1)
            {
                dia2 = "0" + dia2;
            }
            mes2 = fecha2.Date.Month.ToString();
            if (mes2.Length == 1)
            {
                mes2 = "0" + mes2;
            }
            año2 = fecha2.Date.Year.ToString();
            txtFechaIngreso.Text = dia2 + mes2 + año2;
        }

        private Empleado ObtenerEmpleado(string? documento)
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

                MessageBox.Show("Error al cargar Usuario");
            }
            finally
            {
                cn.Close();
            }
            return emp;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Empleado emp = ObtenerDatosEmpleado();
            bool resultado = ModificarEmpleado(emp);
            if (resultado)
            {
                MessageBox.Show("Empleado actualizado con éxito");
                LimpiarCampos();
                cargarGrillaEmpleados();
                cargarComboTipoDoc();
            }
            
        }

        private bool ModificarEmpleado(Empleado emp)
        {
            bool resultado = false;
            string cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["CadenaBD"];
            SqlConnection cn = new SqlConnection(cadenaConexion);

            try
            {
                SqlCommand cmd = new SqlCommand(); 
                string consulta = " UPDATE Empleados SET Nombre = @nombre, Apellido = @apellido, Fecha_nacimiento = @fechanacimiento, Tipo_doc = @idtipodocumento, Nro_doc = @nrodocumento, Fecha_ingreso = @fechaingreso, Matricula = @matricula "+
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
