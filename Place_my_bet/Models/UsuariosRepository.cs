using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class UsuariosRepository
    {
        #region Conexión BBDD

        /// <summary>
        /// Método usado para conectarnos a la Base de datos.
        /// </summary>
        /// <returns>Devuelve la conexión</returns>

        //private MySqlConnection Connect()
        //{
        //    string conString = "Server=127.0.0.1;Port=3306;Database=place_my_bet;Uid=root;password=;SslMode=none";

        //    MySqlConnection con = new MySqlConnection(conString);

        //    return con;

        //}
        #endregion

        #region Devolución de todos los elementos

        /// <summary>
        /// Método que devuelve toda una lista de usuarios de nuestra base de datos.
        /// </summary>
        /// <returns>Devuelve la lista de usuarios</returns>

        internal List<Usuario> Retrieve()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                usuarios = context.Usuarios.ToList();
            }

            return usuarios;
        }
        #endregion

        #region Devolución por email en concreto de un Usuario

        /// <summary>
        /// Método que retornará un Usuario modificado con un DTO.
        /// 
        /// En sí, este método recibe un string email, que le será pasado por parámetro mediante postman.
        /// Luego, nos realizamos una variable de tipo PlaceMyBetContext y otra de tipo Usuario.
        /// La de tipo usuario recibirá que es igual a context.Usuarios ==> es decir, que la variable de
        /// tipo Usuario será igual a la variable de context con su propiedad Usuarios, siendo esta quien
        /// nos devuelve información de la BBDD afín a las propiedades de la variable usuario de tipo Usuario.
        /// 
        /// Además,  usará el include, para incluirle un objeto de tipo Apuestas. Pero esto se puede hacer,
        /// porque en nuestra clase Usuario ya existe una clase de tipo Apuesta, siendo esta, realmente, una
        /// lista de Apuestas, por lo que, realmente, lo que nos está dando es una lista de Apuestas, no una
        /// única apuesta. 
        /// 
        /// Luego, incorporaríamos el FirstOrDefault, que sería quien se encargaría de otorgarnos el usuario, el cual será
        /// el primero por defecto.
        /// 
        /// Posteriormente nos declararemos una variable de tipo double para usarla de acumulador y
        /// luego de ello,sumaremos la cantidad apostada de todas las apuestas en cantidad_Dinero.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Retornamos un UsuarioDTO.</returns>

        internal Usuario RetrieveByEmail(string email)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Usuario usuario = context.Usuarios
                .Include(usu => usu.Apuestas)
                .FirstOrDefault(usu => usu.EmailId == email);


            return usuario;
        }

        #endregion

        #region Devolución por nombre en concreto de un Usuario

        /// <summary>
        /// Método que retornará un Usuario modificado con un DTO.
        /// 
        /// En sí, este método recibe un string email, que le será pasado por parámetro mediante postman.
        /// Luego, nos realizamos una variable de tipo PlaceMyBetContext y otra de tipo Usuario.
        /// La de tipo usuario recibirá que es igual a context.Usuarios ==> es decir, que la variable de
        /// tipo Usuario será igual a la variable de context con su propiedad Usuarios, siendo esta quien
        /// nos devuelve información de la BBDD afín a las propiedades de la variable usuario de tipo Usuario.
        /// 
        /// Además,  usará el include, para incluirle un objeto de tipo Apuestas. Pero esto se puede hacer,
        /// porque en nuestra clase Usuario ya existe una clase de tipo Apuesta, siendo esta, realmente, una
        /// lista de Apuestas, por lo que, realmente, lo que nos está dando es una lista de Apuestas, no una
        /// única apuesta. 
        /// 
        /// Luego, incorporaríamos el FirstOrDefault, que sería quien se encargaría de otorgarnos el usuario, el cual será
        /// el primero por defecto.
        /// 
        /// Posteriormente nos declararemos una variable de tipo double para usarla de acumulador y
        /// luego de ello,sumaremos la cantidad apostada de todas las apuestas en cantidad_Dinero.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Retornamos un UsuarioDTO.</returns>

        internal Usuario RetrieveByName(string name)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Usuario usuario = context.Usuarios
                .Include(usu => usu.Apuestas)
                .FirstOrDefault(usu => usu.Nombre == name);


            return usuario;
        }

        #endregion

        #region Devolución por nombre en concreto de un Usuario

        /// <summary>
        /// Método que retornará un Usuario modificado con un DTO.
        /// 
        /// En sí, este método recibe un string email, que le será pasado por parámetro mediante postman.
        /// Luego, nos realizamos una variable de tipo PlaceMyBetContext y otra de tipo Usuario.
        /// La de tipo usuario recibirá que es igual a context.Usuarios ==> es decir, que la variable de
        /// tipo Usuario será igual a la variable de context con su propiedad Usuarios, siendo esta quien
        /// nos devuelve información de la BBDD afín a las propiedades de la variable usuario de tipo Usuario.
        /// 
        /// Además,  usará el include, para incluirle un objeto de tipo Apuestas. Pero esto se puede hacer,
        /// porque en nuestra clase Usuario ya existe una clase de tipo Apuesta, siendo esta, realmente, una
        /// lista de Apuestas, por lo que, realmente, lo que nos está dando es una lista de Apuestas, no una
        /// única apuesta. 
        /// 
        /// Luego, incorporaríamos el FirstOrDefault, que sería quien se encargaría de otorgarnos el usuario, el cual será
        /// el primero por defecto.
        /// 
        /// Posteriormente nos declararemos una variable de tipo double para usarla de acumulador y
        /// luego de ello,sumaremos la cantidad apostada de todas las apuestas en cantidad_Dinero.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Retornamos un UsuarioDTO.</returns>

        internal Usuario RetrieveBySurName(string surname)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Usuario usuario = context.Usuarios
                .Include(usu => usu.Apuestas)
                .FirstOrDefault(usu => usu.Apellidos == surname);


            return usuario;
        }

        #endregion

        #region Conversión de Usuario a UsuarioDTO

        /// <summary>
        /// Método que transforma un Usuario en un UsuarioDTO.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Devuelve un UsuarioDTO</returns>

        private static UsuarioDTO ToDTO(Usuario user)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Usuario usuario = context.Usuarios
                .Include(usu => usu.Apuestas)
                .FirstOrDefault(usu => usu.EmailId == user.EmailId);

            double cantidad_Dinero = 0;


            for (int i = 0; i < usuario.Apuestas.Count; i++)
            {
                cantidad_Dinero += usuario.Apuestas[i].Dinero_Apostado;
            }


            return new UsuarioDTO(usuario.EmailId, usuario.Nombre, usuario.Apellidos, usuario.Edad);
        }

        #endregion

        #region Devolución de UsuariosDTO

        /// <summary>
        /// Devolución de usuarios por DTO.
        /// </summary>
        /// <returns></returns>

        internal List<UsuarioDTO> RetrieveDTO()
        {
            List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                usuarios = context.Usuarios
                    .Select(usu => ToDTO(usu))
                    .ToList();
            }

            return usuarios;
        }

        #endregion

        #region Inserción de un Usuario a la BBDD con entityFramework

        internal void Save(Usuario usuario)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            context.Usuarios.Add(usuario);
            context.SaveChanges();
        }

        #endregion

        #region Actualizar Usuario

        internal void UpdateUsuario(string email, string nombre, string apellidos, int edad)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Usuario usuario = context.Usuarios.FirstOrDefault(usu => usu.EmailId == email);

            usuario.Nombre = nombre;
            usuario.Apellidos = apellidos;
            usuario.Edad = edad;

            context.Usuarios.Update(usuario);
            context.SaveChanges();
        }

        #endregion

        #region Elilminar Usuario

        internal bool DeleteUsuario(string email)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Usuario usuario = context.Usuarios.Where(usu => usu.EmailId == email)
                .FirstOrDefault();

            bool userOk = false;

            if(usuario != null)
            {
                userOk = true;
                context.Usuarios.Remove(usuario);
                context.SaveChanges();
            }

            return userOk;
        }

        #endregion

        internal List<String> RetrieveDates()
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            List<String> dates = context.Usuarios.Select(ap => ap.Fecha_Registro).DistinctBy(ap => ap).ToList();

            return dates;
        }

        internal List<int> RetrieveQuantityUsersByDate()
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            List<int> quantityBets = new List<int>();

            List<String> dates = RetrieveDates();

            List<String> allDates = context.Usuarios.Select(ap => ap.Fecha_Registro).ToList();

            foreach (String date in dates)
            {
                int qBet = allDates.Count(userDate => userDate == date);
                quantityBets.Add(qBet);
            }

            return quantityBets;
        }
    }
}