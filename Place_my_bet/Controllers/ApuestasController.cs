using Place_my_bet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Place_my_bet.Controllers
{
    public class ApuestasController : ApiController
    {
        //GET: api/Apuestas
        public IEnumerable<Apuesta> GetBetsComplete()
        {
            var repo = new ApuestasRepository();

            List<Apuesta> apuestas = repo.Retrieve();

            return apuestas;
        }


        //public IEnumerable<ApuestaDTO> Get()
        //{
        //    var repo = new ApuestasRepository();
        //    List<ApuestaDTO> apuestas = repo.RetrieveApuestaDTO();
        //    return apuestas;
        //}

        //GET api/Apuestas?email={email}
        public IEnumerable<Apuesta> GetBetsByEmail(string email)
        {
            var repo = new ApuestasRepository();

            List<Apuesta> apuestas = repo.RetrieveByEmail(email);

            return apuestas;
        }

        //GET api/Apuestas?mercadoId={mercadoId}
        public IEnumerable<Apuesta> GetBetsByMarket(int mercadoId)
        {
            var repo = new ApuestasRepository();

            List<Apuesta> apuestas = repo.RetrieveByMarket(mercadoId);

            return apuestas;
        }

        public IEnumerable<Apuesta> GetBetsByEvent(int eventoId)
        {
            var repo = new ApuestasRepository();

            List<Apuesta> apuestas = repo.RetrieveByEvent(eventoId);

            return apuestas;
        }

        public List<String> GetLabelDates(int dateId)
        {
            var repo = new ApuestasRepository();

            List<String> labelDates = repo.RetrieveDates();

            return labelDates;
        }

        public List<int> GetInfoBets(int quantityBet)
        {
            var repo = new ApuestasRepository();

            List<int> quantityBets = repo.RetrieveQuantityBetsByDate();

            return quantityBets;
        }

        //// GET: api/Apuestas/5
        //public Apuesta Get(int id)
        //{
        //    var repo = new ApuestasRepository();

        //    Apuesta apuesta = repo.RetrieveById(id);

        //    return apuesta;
        //}

        //POST: api/Apuestas
        public bool Post([FromBody] Apuesta apuesta)
        {
            var repo = new ApuestasRepository();

           bool inserting = repo.SavebyEntity(apuesta);

            return inserting;
        }

        //POST api/Apuestas?eventoId={eventoId}
        public bool PostNewMarket(int eventoId)
        {
            bool ok;

            var repo = new ApuestasRepository();

            ok = repo.SaveNewMarket(eventoId);

            return ok;
        }


        //GET: api/Apuestas?email={email}&idm={idm}
        //public IEnumerable<ApuestaEmailDTO> GetEventandBet(string email, int idm)
        //{
        //    var repo = new ApuestasRepository();

        //    List<ApuestaEmailDTO> apuestas = repo.RetrievebyEmailandMarketType(email, idm);

        //    return apuestas;
        //}

        //GET: api/Apuestas?ident={ident}&usu_mail={usu_mail}
        //[Authorize(Roles = "Admin")]
        //public IEnumerable<ApuestabyMarket> GetMarketBet(int ident, string usu_mail)
        //{
        //    var repo = new ApuestasRepository();


        //    List<ApuestabyMarket> apuestas = repo.RetrieveBetByTypeBetandEmail(ident, usu_mail);

        //    return apuestas;
        //}

        //POST: api/Apuestas
        //[Authorize(Roles ="Standard")]
        //public void Post([FromBody] Apuesta apuesta)
        //{
        //    var repo = new ApuestasRepository();

        //    repo.Save(apuesta);
        //}

        // PUT: api/Apuestas/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Apuestas/5
        public void Delete(int id)
        {
        }
    }
}
