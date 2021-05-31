using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class Usuario
    {

        public Usuario() { }

        public Usuario(string emailId, string nombre, string apellidos, int edad, string fecha_Registro, Cuenta cuenta, List<Apuesta> apuestas)
        {
            EmailId = emailId;
            Nombre = nombre;
            Apellidos = apellidos;
            Edad = edad;
            Fecha_Registro = fecha_Registro;
            Cuenta = cuenta;
            Apuestas = apuestas;
        }

        [Key]
        public string EmailId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }
        public int Edad { get; set; }
        public string Fecha_Registro { get; set; }
        public Cuenta Cuenta { get; set; }
        public List<Apuesta> Apuestas { get; set; }
   
    }

    public class UsuarioDTO
    {
        public UsuarioDTO(string emailId, string nombre, string apellidos, int edad)
        {
            EmailId = emailId;
            Nombre = nombre;
            Apellidos = apellidos;
            Edad = edad;
            
        }

        public string EmailId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Edad { get; set; }
        public double Cantidad_Total_Apostada { get; set; }
    }
}