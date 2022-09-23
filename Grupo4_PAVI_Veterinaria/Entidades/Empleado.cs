using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo4_PAVI_Veterinaria.Entidades
{
    internal class Empleado
    {
        private string Documento;
        private string Apellido;
        private string Nombre;
        private DateTime FechaNacimiento;
        private DateTime FechaIngreso;
        private int IdTipoDocumento;
        private string Matricula;
        private int Activo;

        public Empleado(string Doc, string ape, string nom)
        {
            this.Documento = Doc;
            this.Apellido = ape;
            this.Nombre = nom;
        }

        public Empleado()
        {

        }

        public string DocumentoEmpleado
        {
            get => Documento;
            set => Documento = value;
        }

        public string MatriculaEmpleado
        {
            get => Matricula;
            set => Matricula = value;
        }
        public int IdTipoDocumentoEmpleado
        {
            get => IdTipoDocumento;
            set => IdTipoDocumento = value;
        }
        public int ActivoEmpleado
        {
            get => Activo;
            set => Activo = value;
        }

        public DateTime FechaNacimientoEmpleado
        {
            get => FechaNacimiento;
            set => FechaNacimiento = value;
        }
        public DateTime FechaIngresoEmpleado
        {
            get => FechaIngreso;
            set => FechaIngreso = value;
        }
        public string ApellidoEmpleado
        {
            get => Apellido;
            set => Apellido = value;

        }
        public string NombreEmpleado
        {
            get => Nombre;
            set => Nombre = value;

        }

    }
}
