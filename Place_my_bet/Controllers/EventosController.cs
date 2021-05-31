using Place_my_bet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Place_my_bet.Controllers
{
    public class EventosController : ApiController
    {
        // GET: api/Eventos
        public IEnumerable<Evento> Get()
        {
            var repo = new EventosRepository();

            //List<Eventos> eventos = repo.Retrieve();

            List<Evento> eventos = repo.Retrieve();

            return eventos;
        }

        // GET: api/Eventos
        //public IEnumerable<EventoDTO> Get()
        //{
        //    var repo = new EventosRepository();

        //    //List<Eventos> eventos = repo.Retrieve();

        //    List<EventoDTO> eventos = repo.RetrieveDTO();

        //    return eventos;
        //}

        //GET:api/Eventos?tipo_Ap={tipo_Ap}&idM={idM}
        public IEnumerable<Mercado> GetIDandBetType(double tipo_Ap, int idM)
        {
            var repo = new EventosRepository();

            //List<Eventos> eventos = repo.Retrieve();

            List<Mercado> mercados = repo.RetrievebyIDandBetType(tipo_Ap, idM);

            return mercados;
        }

        //// GET: api/Eventos?day={day}
        //public IEnumerable<EventoDTO> GetDay(string day)
        //{
        //    var repo = new EventosRepository();

        //    //List<Eventos> eventos = repo.Retrieve();

        //    List<EventoDTO> eventos = repo.RetrieveDTObyDay(day);

        //    return eventos;
        //}

        // GET: api/Eventos?day={day}&day2={day2}
        //public IEnumerable<EventoDTO> GetDates(string day, string day2)
        //{
        //    var repo = new EventosRepository();

        //    //List<Eventos> eventos = repo.Retrieve();

        //    List<EventoDTO> eventos = repo.RetrieveDTObyTwoDates(day, day2);

        //    return eventos;
        //}

        //// GET:api/Eventos?localTeam={localTeam}
        //public IEnumerable<EventoDTO> GetLocalTeam(string localTeam)
        //{
        //    var repo = new EventosRepository();

        //    //List<Eventos> eventos = repo.Retrieve();

        //    List<EventoDTO> eventos = repo.RetrieveDTObyLocalTeam(localTeam);

        //    return eventos;
        //}

        ////GET api/Eventos?foreignTeam={foreignTeam}
        //public IEnumerable<EventoDTO> GetForeignTeam(string foreignTeam)
        //{
        //    var repo = new EventosRepository();

        //    //List<Eventos> eventos = repo.Retrieve();

        //    List<EventoDTO> eventos = repo.RetrieveDTObyForeingTeam(foreignTeam);

        //    return eventos;
        //}

        // GET: api/Eventos/5
        public Evento GetEventById(int id)
        {
            var repo = new EventosRepository();

            //List<Eventos> eventos = repo.Retrieve();

           Evento evento = repo.RetrieveById(id);

            return evento;
        }

        //GET api/Eventos?dateAndHour={dateAndHour}
        public List<Evento> GetEventByDate(string dateAndHour)
        {
            var repo = new EventosRepository();

            //List<Eventos> eventos = repo.Retrieve();

            List<Evento> evento = repo.RetrieveByDate(dateAndHour);

            return evento;
        }

        public List<Evento> GetEventByTeam(string team)
        {
            var repo = new EventosRepository();

            //List<Eventos> eventos = repo.Retrieve();

            List<Evento> evento = repo.RetrieveByTeam(team);

            return evento;
        }

        // POST: api/Eventos
        //public void Post([FromBody] EventoDTO evento)
        //{
        //    var repo = new EventosRepository();

        //    repo.Save(evento);
        //}

        //POST: api/Eventos
        public void Post([FromBody] Evento evento)
        {
            var repo = new EventosRepository();

            repo.Save(evento);
        }

        // PUT: api/Eventos/5
        public void Put(int id, string equipo_Local, string equipo_Visitante)
        {
            var repo = new EventosRepository();

            repo.UpdateEvento(id, equipo_Local, equipo_Visitante);
        }

        public bool PutEventoDate(int idEvento, string fecha)
        {
            var repo = new EventosRepository();

            bool updated = repo.UpdateEventoByIdAndDate(idEvento, fecha);

            return updated;
        }

        // DELETE: api/Eventos/5
        public bool Delete(int id)
        {
            var repo = new EventosRepository();

           bool deleted = repo.DeleteEvento(id);

            return deleted;
        }
    }
}
