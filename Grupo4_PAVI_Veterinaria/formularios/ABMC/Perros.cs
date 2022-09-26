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

namespace Grupo4_PAVI_Veterinaria.formularios.abmcPerros
{
    public partial class Perros : Form
    {
        public Perros()
        {
            InitializeComponent();
        }

        private void Perros_Load(object sender, EventArgs e)
        {
            btnActualizar.Enabled = false;
            CargarCombosRazas();
            CargarCombosDueños();
            CargarGrilla();
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtFecha.Text = "";
            txtAltura.Text = "";
            txtPeso.Text = "";
            cmbDueño.SelectedIndex = -1;
            cmbRaza.SelectedIndex = -1;
        }

        private void CargarGrilla()
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

                gdr_perros.DataSource = tabla;
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

        private void CargarCombosRazas()
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

                cmbRaza.DataSource = tabla;
                cmbRaza.DisplayMember = "Denominacion";
                cmbRaza.ValueMember = "Id_raza";
                cmbRaza.SelectedIndex = -1;
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

        private void CargarCombosDueños()
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

                cmbDueño.DataSource = tabla;
                cmbDueño.DisplayMember = "Nombre";
                cmbDueño.ValueMember = "Id_dueño";
                cmbDueño.SelectedIndex = -1;
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

        private Perro ObtenerDatosPerro()
        {
            Perro p = new Perro();
            p.Nombre = txtNombre.Text.Trim();
            p.FechaNacimiento = DateTime.Parse(txtFecha.Text);
            p.Id_raza = (int)cmbRaza.SelectedValue;
            p.Id_dueño = (int)cmbDueño.SelectedValue;
            p.Peso = float.Parse(txtPeso.Text.Trim());
            p.Altura = float.Parse(txtAltura.Text.Trim());
            return p;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            Perro p = ObtenerDatosPerro();
            bool resultado = AgregarPersonaBD(p);
            if (resultado)
            {
                MessageBox.Show("Perro agregado con éxito.");
                LimpiarCampos();
                CargarCombosRazas();
                CargarCombosDueños();
                CargarGrilla();
            }
            else
            {
                MessageBox.Show("Error al registrar.");
            }
        }

        private bool AgregarPersonaBD(Perro per)
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

        private void gdr_perros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex;
            btnActualizar.Enabled = true;
            DataGridViewRow fila = gdr_perros.Rows[indice];

            string nro_Hc = fila.Cells["Nro_HC"].Value.ToString();

            Perro p = ObtenerPerro(nro_Hc);
            LimpiarCampos();
            CargarCampos(p);
        }

        private Perro ObtenerPerro(string nroHC)
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

        private void CargarCampos(Perro p)
        {
            txtNombre.Text = p.Nombre;
            txtPeso.Text = p.Peso.ToString();
            txtAltura.Text = p.Altura.ToString();
            DateTime fecha = p.FechaNacimiento;
            string dia = fecha.Date.Day.ToString();
            string mes = fecha.Date.Month.ToString();
            string año = fecha.Date.Year.ToString();
            if (dia.Length == 1)
            {
                dia = "0" + dia;
            }
            if (mes.Length == 1)
            {
                mes = "0" + mes;
            }
            txtFecha.Text = dia + mes + año;
            cmbRaza.SelectedValue = p.Id_raza;
            cmbDueño.SelectedValue = p.Id_dueño;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Perro p = ObtenerDatosPerro();
            bool resultado = ActualizarPerro(p);
            if (resultado)
            {
                MessageBox.Show("Perro actualizado con éxito.");
                LimpiarCampos();
                CargarCombosRazas();
                CargarCombosDueños();
                CargarGrilla();
            }
            else
            {
                MessageBox.Show("Error al actualizar.");
            }
        }

        private bool ActualizarPerro(Perro p)
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
