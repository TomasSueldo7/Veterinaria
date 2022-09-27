using Grupo4_PAVI_Veterinaria.Datos;
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
            bool resultado = EmpleadosBD.AgregarEmpleadoABD(emp); //Los agrego a la BD
            if (resultado)
            {
                MessageBox.Show("Empleado agregado con éxito");
                cargarGrillaEmpleados();
                LimpiarCampos();
                
            }
            else
            {
                MessageBox.Show("Error al agregar Empleado");
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
            btnActualizar.Enabled = false;
            btnBaja.Enabled = false;
            LimpiarCampos();
        }

        private void cargarGrillaEmpleados()
        {
            try
            {
                gdrEmpleados.DataSource = EmpleadosBD.ObtenerGrillaEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la grilla de Empleados");
            }
        }

        private void cargarComboTipoDoc()
        {
            try
            {
                cmbTipoDoc.DataSource = EmpleadosBD.ObtenerTipoDoc();
                cmbTipoDoc.DisplayMember = "Nombre"; //Cada opcion del combo me lo mostras de la columna Nombre
                cmbTipoDoc.ValueMember = "Id_TipoDoc"; //Cada opcion del combo su valor esta dado por su id
                cmbTipoDoc.SelectedIndex = -1; //Deja por defecto nada seleccionado

        }
            catch (Exception ex)
            {

            MessageBox.Show("Error al cargar los tipos de Documentos");
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
            btnActualizar.Enabled = false;
            btnBaja.Enabled = false;
        }

        private void gdrEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex; //Me indica en que fila estoy parado
            btnActualizar.Enabled = true;
            btnBaja.Enabled = true;
            DataGridViewRow filaSeleccionada = gdrEmpleados.Rows[indice];
            string documento = filaSeleccionada.Cells["Documento"].Value.ToString();
            Empleado emp = EmpleadosBD.ObtenerEmpleado(documento);
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

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Empleado emp = ObtenerDatosEmpleado();
            bool resultado = EmpleadosBD.ModificarEmpleado(emp);
            if (resultado)
            {
                MessageBox.Show("Empleado actualizado con éxito");
                LimpiarCampos();
                cargarGrillaEmpleados();
                cargarComboTipoDoc();
            }
            
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {

            Empleado emp = ObtenerDatosEmpleado(); //Obtengo los datos de los txt
            bool resultado = EmpleadosBD.EliminarEmpleadoABD(emp); //Los agrego a la BD
            if (resultado)
            {
                MessageBox.Show("Empleado eliminado con éxito");
                cargarGrillaEmpleados();
                LimpiarCampos();

            }
            else
            {
                MessageBox.Show("Error al eliminar empleado");
            }
        }
    }
}
