using System.Data.SqlClient;
using System.Data;
using Grupo4_PAVI_Veterinaria.formularios;

namespace Grupo4_PAVI_Veterinaria
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Equals("") || txtPassword.Text.Equals(""))
            {
                MessageBox.Show("Ingrese nombre de usuario y contraseña");

            }
            else
            {
                string nombreDeUsuario = txtUsuario.Text;
                string contra = txtPassword.Text;
                bool resultado = false;
                try
                {
                    resultado = ValidarUsuario(nombreDeUsuario, contra);
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al consultar el usuario");
                }

                if (resultado)
                {
                    PrincipalForm ventana = new PrincipalForm();
                    ventana.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario Inexistente");
                }
            }
        }
        private bool ValidarUsuario(string nombreDeUsuario, string password)
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