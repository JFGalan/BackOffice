using Place_my_bet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Place_my_bet.Controllers
{
    public class CuentasController : ApiController
    {
        // GET: api/Cuentas
        public IEnumerable<Cuenta> Get()
        {
            var repo = new CuentasRepository();

            List<Cuenta> cuentas = repo.Retrieve();

            return cuentas;
        }

        // GET: api/Cuentas/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Cuentas
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Cuentas/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Cuentas/5
        public void Delete(int id)
        {
        }
    }
}
