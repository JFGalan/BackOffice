using Place_my_bet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Place_my_bet.Controllers
{
    public class UsuariosController : ApiController
    {
        // GET: api/Usuarios
        public IEnumerable<Usuario> Get()
        {
            var repo = new UsuariosRepository();

            List<Usuario> usuarios = repo.Retrieve();

            return usuarios;
        }

        //// GET: api/Usuarios
        //public List<UsuarioDTO> Get()
        //{
        //    var repo = new UsuariosRepository();

        //    List<UsuarioDTO> usuarios = repo.RetrieveDTO();

        //    return usuarios;
        //}

        //// GET: api/Usuarios/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //GET api/Usuarios?email={email}
        public Usuario GetUser(string email)
        {
            var repo = new UsuariosRepository();

            Usuario usuario = repo.RetrieveByEmail(email);

            return usuario;
        }

        //GET api/Usuarios?name={name}
        public Usuario GetUserName(string name)
        {
            var repo = new UsuariosRepository();

            Usuario usuario = repo.RetrieveByName(name);

            return usuario;
        }

        //GET api/Usuarios?surname={surname}
        public Usuario GetUserSurName(string surname)
        {
            var repo = new UsuariosRepository();

            Usuario usuario = repo.RetrieveBySurName(surname);

            return usuario;
        }

        public List<String> GetLabelDates(int dateId)
        {
            var repo = new UsuariosRepository();

            List<String> labelDates = repo.RetrieveDates();

            return labelDates;
        }

        public List<int> GetInfoBets(int countUsers)
        {
            var repo = new UsuariosRepository();

            List<int> quantityUsers = repo.RetrieveQuantityUsersByDate();

            return quantityUsers;
        }


        // POST: api/Usuarios
        public void Post([FromBody] Usuario usuario)
        {
            var repo = new UsuariosRepository();

            repo.Save(usuario);
        }

        // PUT: api/Usuarios/5
        public void Put(string email, string nombre, string apellidos, int edad)
        {
            var repo = new UsuariosRepository();

            repo.UpdateUsuario(email, nombre, apellidos, edad);
        }

        // DELETE: api/Usuarios/5
        public bool Delete(string emailId)
        {
            var repo = new UsuariosRepository();

           bool confirmDelete = repo.DeleteUsuario(emailId);

            return confirmDelete;
        }
    }
}
