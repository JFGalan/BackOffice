using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class Mercado
    { 

        public Mercado() { }

        public Mercado(int mercadoId, double tipo_over_under, double cuota_Over, double cuota_Under, double dinero_Over, double dinero_Under, bool bloqueado, int eventoId, Evento evento, List<Apuesta> apuestas)
        {
            MercadoId = mercadoId;
            Tipo_over_under = tipo_over_under;
            Cuota_Over = cuota_Over;
            Cuota_Under = cuota_Under;
            Dinero_Over = dinero_Over;
            Dinero_Under = dinero_Under;
            Bloqueado = bloqueado;
            EventoId = eventoId;
            Evento = evento;
            Apuestas = apuestas;
        }

        public int MercadoId { get; set; }
        public double Tipo_over_under { get; set; }
        public double Cuota_Over { get; set; }
        public double Cuota_Under { get; set; }
        public double Dinero_Over { get; set; }
        public double Dinero_Under { get; set; }
        public bool Bloqueado { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
        public List<Apuesta> Apuestas { get; set; }

    }

    public class MercadoDTO
    {
        public MercadoDTO(double tipo_over_under, double cuota_Over, double cuota_Under)
        {
            Tipo_over_under = tipo_over_under;
            Cuota_Over = cuota_Over;
            Cuota_Under = cuota_Under;
        }

        public double Tipo_over_under { get; set; }
        public double Cuota_Over { get; set; }
        public double Cuota_Under { get; set; }
    }
}