using Place_my_bet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Place_my_bet.Controllers
{
    public class MercadosController : ApiController
    {
        //// GET: api/Mercados
        //public IEnumerable<Mercado> Get()
        //{
        //    var repo = new MercadosRepository();

        //    List<Mercado> mercados = repo.Retrieve();

        //    return mercados;
        //}

        // GET: api/Mercados
        public IEnumerable<MercadoDTO> Get()
        {
            var repo = new MercadosRepository();

            List<MercadoDTO> mercados = repo.RetrieveMercadoDTO();

            return mercados;
        }

        // GET: api/Mercados/5
        public Mercado Get(int id)
        {
            var repo = new MercadosRepository();

            Mercado mercados = repo.RetrieveById(id);

            return mercados;
        }

        //POST: api/Mercados
        public void Post([FromBody] Mercado mercado)
        {
            var repo = new MercadosRepository();

            repo.Save(mercado);
        }


        //// POST: api/Mercados
        //public void Post([FromBody]Mercado mercado)
        //{
        //    var repo = new MercadosRepository();

        //    repo.Save(mercado);
        //}

        // PUT: api/Mercados/5
        public int Put(int idMercado, bool blockOrUnblock)
        {
            var repo = new MercadosRepository();

            int update = repo.BlockAndUnblockMarket(idMercado, blockOrUnblock);

            return update;
        }

        // DELETE: api/Mercados/5
        public void Delete(int id)
        {
        }
    }
}
