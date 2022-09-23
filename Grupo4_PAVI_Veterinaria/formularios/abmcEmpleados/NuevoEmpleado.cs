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
            Empleado emp= ObtenerDatosEmpleado();
            bool resultado = AgregarEmpleadoABD(emp);
            if (resultado)
            {
                MessageBox.Show("Persona agregada con éxito");
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
            LimpiarCampos();
            cargarComboTipoDoc();
            txtNombre.Focus();
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
    }
}
