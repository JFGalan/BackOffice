using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class Apuesta
    {

        public Apuesta(int apuestaId, string tipo_Apuesta, double tipo_Cuota, double dinero_Apostado, DateTime fecha, int mercadoId, Mercado mercado, string usuarioId, Usuario usuario)
        {
            ApuestaId = apuestaId;
            Tipo_Apuesta = tipo_Apuesta;
            Tipo_Cuota = tipo_Cuota;
            Dinero_Apostado = dinero_Apostado;
            Fecha = fecha;
            MercadoId = mercadoId;
            Mercado = mercado;
            UsuarioId = usuarioId;
            Usuario = usuario;
        }

        public Apuesta(int apuestaId, string tipo_Apuesta, double tipo_Cuota, double dinero_Apostado, DateTime fecha, int mercadoId, string usuarioId)
        {
            ApuestaId = apuestaId;
            Tipo_Apuesta = tipo_Apuesta;
            Tipo_Cuota = tipo_Cuota;
            Dinero_Apostado = dinero_Apostado;
            Fecha = fecha;
            MercadoId = mercadoId;
            UsuarioId = usuarioId;
        }

        public Apuesta() { }

        public int ApuestaId { get; set; }
        [Required]
        public string Tipo_Apuesta { get; set; }
        public double Tipo_Cuota { get; set; }
        public double Dinero_Apostado { get; set; }
        public DateTime Fecha { get; set; }
        public int MercadoId { get; set; } //Entidad Dependiente
        public Mercado Mercado { get; set; }
        [Required]
        public string UsuarioId { get; set; } //Entidad Dependiente
        public Usuario Usuario { get; set; }

    }

    public class ApuestaDTO
    {
        public ApuestaDTO(string usuarioId, int eventoId, string tipo_Apuesta, double tipo_Cuota, double dinero_Apostado)
        {
            UsuarioId = usuarioId;
            EventoId = eventoId;
            Tipo_Apuesta = tipo_Apuesta;
            Tipo_Cuota = tipo_Cuota;
            Dinero_Apostado = dinero_Apostado;
        }

        public string UsuarioId { get; set; }
        public int EventoId { get; set; }
        public string Tipo_Apuesta { get; set; }
        public double Tipo_Cuota { get; set; }
        public double Dinero_Apostado { get; set; }
    }

    public class ApuestaDTO2
    {
        public ApuestaDTO2()
        {

        }

        public List<DateTime> Fecha { get; set; }
        public int Cantidad_Apuestas { get; set; }
    }
}