using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class Evento
    {
        public Evento(int eventoId, string equipo_Local, string equipo_Visitante, string dia, List<Mercado> mercados)
        {
            EventoId = eventoId;
            Equipo_Local = equipo_Local;
            Equipo_Visitante = equipo_Visitante;
            Dia = dia;
            Mercados = mercados;
        }

        public Evento() { }

        public int EventoId { get; set; }

        [Required]
        public string Equipo_Local { get; set; }

        [Required]
        public string Equipo_Visitante { get; set; }

        [Required]
        public string Dia { get; set; }

        public List<Mercado> Mercados { get; set; }

    }

    public class EventoDTO
    {
        public EventoDTO(string equipo_Local, string equipo_Visitante)
        {
            Equipo_Local = equipo_Local;
            Equipo_Visitante = equipo_Visitante;
        }

        public string Equipo_Local { get; set; }
        public string Equipo_Visitante { get; set; }

    }
}
