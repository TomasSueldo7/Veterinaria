using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo4_PAVI_Veterinaria.Entidades
{
    internal class Perro
    {
        private int nro_HC;
        private string nombre;
        private float peso;
        private float altura;
        private DateTime fecha_nac;
        private int id_dueño;
        private int id_raza;


        public Perro()
        {

        }

        public Perro(int nro_HC, string nombre, float peso, float altura, DateTime fecha_nac, int id_dueño, int id_raza)
        {
            this.nro_HC = nro_HC;
            this.nombre = nombre;
            this.peso = peso;
            this.altura = altura;
            this.fecha_nac = fecha_nac;
            this.id_dueño = id_dueño;
            this.id_raza = id_raza;
        }

        public int Nro_HC
        {
            get => nro_HC;
            set => nro_HC = value;
        }

        public string Nombre
        {
            get => nombre; 
            set => nombre = value;
        }

        public float Peso
        {
            get => peso; 
            set => peso = value;
        }

        public float Altura
        {
            get => altura; set => altura = value;   
        }

        public DateTime FechaNacimiento
        {
            get => fecha_nac; set => fecha_nac = value; 
        }

        public int Id_dueño
        {
            get => id_dueño; set => id_dueño = value;   
        }

        public int Id_raza
        {
            get => id_raza; set => id_raza = value; 
        }
    }
}
