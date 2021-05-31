using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class EventosRepository
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
        /// Método que devuelve toda una lista de eventos de nuestra base de datos.
        /// </summary>
        /// <returns>Devuelve la lista de apuestas</returns>

        internal List<Evento> Retrieve()
        {
            List<Evento> eventos = new List<Evento>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                eventos = context.Eventos.ToList();
            }

            return eventos;
        }

        #endregion

        #region Inserción de Eventos CON ENTITY FRAMEWORK

        /// <summary>
        /// Método que se encarga de insertar eventos dentro de la BBDD mediante el entity Framework.
        /// 
        /// En sí, recibirá una variable de tipo Evento.
        /// 
        /// Esta variable recibirá un cambio, es decir, diremos que la variable de tipo Evento en su 
        /// propiedad Dia será igual a la fecha que otorgaremos mediante una variable llamada fecha.
        /// 
        /// Luego haremos un context.Eventos.Add(ev); ==> es decir, que lo añadiremos a la BBDD.
        /// Finalmente, haremos un context.SaveChanges(); ==> es decir, guardarmos los cambios. 
        /// </summary>
        /// <param name="ev"></param>

        internal void Save(Evento ev)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            context.Eventos.Add(ev);
            context.SaveChanges();
        }

        #endregion

        #region Devolución de todos los elementos mediante DTO

        /// <summary>
        /// Método que nos devolverá una lista de EventoDTO.
        /// 
        /// Declararemos una variable que será una lista de EventoDTO y una variable de tipo PlaceMyBetContext.
        /// 
        /// Luego diremos que nuestra lista de EventosDTO, es decir, evento será igual a context.Eventos, 
        /// es decir, eventos = context.Eventos.
        /// Además, diremos que queremos seleccionar aquellos eventos y convertirlos en ToDTO, es decir,
        /// .Select(p => ToDTO(p)) ==> p sería la variable que declararemos internamente y que modificare-
        /// mos en DTO.
        /// Finalmente, con el .ToList(); ==> Diremos que de todo ello se nos otorgue una lista.
        /// </summary>
        /// <returns>Finalmente, devolveremos la lista de EventoDTO.</returns>
        /// 

        internal List<EventoDTO> RetrieveDTO()
        {
            List<EventoDTO> eventos = new List<EventoDTO>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                 eventos = context.Eventos
                    .Select(p => ToDTO(p))
                    .ToList();
            }

            return eventos;
        }

        #endregion

        #region Realise TO DTO

        /// <summary>
        /// Método que modifica un evento en un EventoDTO.
        /// 
        /// Este método recibe un evento y lo modifica para retornar un eventoDTO.
        /// </summary>
        /// <param name="e"></param>
        /// <returns>retorna un eventoDTO</returns>
         

        static internal EventoDTO ToDTO(Evento e)
        {
            return new EventoDTO(e.Equipo_Local, e.Equipo_Visitante);
        }

        #endregion

        #region Update Evento con entity framework

        /// <summary>
        /// Método que realiza un update de evento mediante entity framework.
        /// 
        /// Este  método obtiene tres variables, la primera de ellas de tipo int representando
        /// al id de Evento, la segunda de ellas de tipo string representando al equipo local y 
        /// la tercera de ellas de tipo string representando al equipo visitante.
        /// 
        /// Luego de ello, nos declaramos una variable de tipo Evento y esta obtendrá del método
        /// RetrieveById el evento en concreto del id introducido, ya que este método recibe la 
        /// variable de tipo int que representa el id de evento. 
        /// 
        /// Luego nos definimos una variable de tipo PlaceMyBetContext y otorgamos a la variable de 
        /// tipo mercado los nuevos equipos, tanto visitante como local que queremos cambiar.
        /// 
        /// Finalmente, hacemos un context.Eventos.Update(evento) ==> es decir, que hacemos un update
        /// de ese evento en concreto con los parametros cambiados y con un context.SaveChanges, guardamos
        /// el cambio producido. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipo_Local"></param>
        /// <param name="equipo_Visitante"></param>

        internal void UpdateEvento(int id, string equipo_Local, string equipo_Visitante)
        {
            Evento evento = RetrieveById(id);

            //Evento evento = context.Eventos.FirstOrDefault(a => a.EnventoId == id); Es lo mismo que lo anterior y sin la necesidad de hacer un método. Más corto y eficiente.

            PlaceMyBetContext context = new PlaceMyBetContext();

            evento.Equipo_Local = equipo_Local;
            evento.Equipo_Visitante = equipo_Visitante;

            context.Eventos.Update(evento);
            context.SaveChanges();
        }

        #endregion

        #region Delete de eventos con entity Framework

        /// <summary>
        /// Método que elemina un evento insertándole o pasándole un id.
        /// En este caso  nuestro evento obtiene una variable de tipo int que representará el id
        /// del evento que queremos eliminar.
        /// 
        /// Luego se declarará un PlaceMyBetContext y una variable de tipo Evento.
        /// 
        /// Esta variable de tipo evento será igual a context.Eventos, es decir, que será igual a 
        /// la obtención que otorgue context, pero el tipo de Eventos.
        /// 
        /// Además, que sea aquel que sea el primero por defecto que sea exactamente igual a Id introducido,
        /// es decir, .FirstOrDefault(a => a.EventoId == id); ==> Esto significa que queremos que se
        /// elimine el primer evento cuyo id sea igual al id introducido. 
        /// Realizamos un context.Eventos.Remove(evento); ==> Eliminamos el evento de la BBDD.
        /// Finalmente, hacemos un context.SaveChanges(); ==> es decir, guardamos lo realizado. 
        /// </summary>
        /// <param name="id"></param>
        /// 
        internal bool DeleteEvento(int id)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            Evento evento = context.Eventos
                .FirstOrDefault(a => a.EventoId == id);

            bool okDelete = false;

            if(evento != null)
            {
                context.Eventos.Remove(evento);
                context.SaveChanges();
                okDelete = true;
            }

            return okDelete;
        }

        #endregion

        #region Devolución de Evento mediante ID en concreto con entity Framework

        /// <summary>
        /// Método que nos devuelve un evento en concreto mediante un ID.
        /// 
        /// Este método recibe una variable de tipo int que representará el id de evento.
        /// 
        /// Luego de ello nos haremos una variablde de tipo evento y otra de PlaceMyBetContext.
        /// 
        /// Con todo ello, evento será igual a context.Eventos y está recibirá aquel evento
        /// donde s.EventoId sea igual al id introducido, .Where(s => s.EventoId == id), es decir,
        /// en este caso dentro del Where hacemos que s se refiera a s.EventoId y este lo compara
        /// con el id introducido. 
        /// 
        /// Finalmente, con .FirstOrDefault(); ==> Nos devolverá el evento que sea el primero 
        /// por defecto que sea exactamente igual al id introducido. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Devolución del evento por id</returns>

        internal Evento RetrieveById(int id)
        {
            Evento evento;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                evento = context.Eventos
                    .Where(s => s.EventoId == id)
                    .FirstOrDefault();

            }

            return evento;
        }

        #endregion

        internal List<Evento> RetrieveByDate(string dateEvent)
        {
            List<Evento> evento;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                evento = context.Eventos
                    .Where(s => s.Dia == dateEvent)
                    .ToList();

            }

            return evento;
        }

        internal List<Evento> RetrieveByTeam(string team)
        {
            List<Evento> evento;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                evento = context.Eventos
                    .Where(s => s.Equipo_Local == team || s.Equipo_Visitante == team)
                    .ToList();

            }

            return evento;
        }

        internal bool UpdateEventoByIdAndDate(int id, string fecha)
        {
            Evento evento = RetrieveById(id);

            //Evento evento = context.Eventos.FirstOrDefault(a => a.EnventoId == id); Es lo mismo que lo anterior y sin la necesidad de hacer un método. Más corto y eficiente.

            PlaceMyBetContext context = new PlaceMyBetContext();

            bool okUpdate = false;
            
            if(evento != null)
            {
                evento.Dia = fecha;
                context.Eventos.Update(evento);
                context.SaveChanges();

                okUpdate = true;
            }

            return okUpdate;
        }

        //======================================================================================//

        #region Devolución de todos los elementos con DTO

        /// <summary>
        /// Método que devolverá los elementos DTO de EventosDTO especificados. Pues extraemos solo los nombres de los dos equipos y las fechas.
        /// </summary>
        /// <returns>Devuelve una lista de EventoDTO</returns>

        //internal List<Evento> RetrieveDTO()
        //{
        //    //MySqlConnection con = Connect();

        //    //MySqlCommand command = con.CreateCommand();

        //    //command.CommandText = "select * from evento";

        //    try
        //    {
        //        //con.Open();

        //        //MySqlDataReader res = command.ExecuteReader();

        //        Evento ev = null;

        //        List<Evento> eventos = null;

        //        //List<EventoDTO> eventos = new List<EventoDTO>();

        //        //while (res.Read())
        //        //{
        //        //    ev = new EventoDTO(res.GetString(1), res.GetString(2), res.GetString(3));

        //        //    eventos.Add(ev);
        //        //}

        //        //con.Close();

        //        return eventos;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("Se ha producido un error de conexión");
        //        return null;
        //    }
        //}

        #endregion

        #region Devolución de mercados en concreto

        internal List<Mercado> RetrievebyIDandBetType(double tipo_Ap, int idMer)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            //command.CommandText = "SELECT * FROM mercado WHERE mercado.Tipo_over_under = @A AND mercado.Id_Evento = @A2;";

            //command.Parameters.AddWithValue("@A", tipo_Ap);

            //command.Parameters.AddWithValue("@A2", idMer);

            try
            {
                //con.Open();

                //MySqlDataReader res = command.ExecuteReader();

                Mercado merc = null;
                List<Mercado> mercado = null;
                //List<Mercado> mercado = new List<Mercado>();

                //while (res.Read())
                //{
                //    merc = new Mercado(res.GetInt32(0), res.GetDouble(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetInt32(6));

                //    mercado.Add(merc);
                //}

                //con.Close();

                return mercado;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Se ha producido un error de conexión");
                return null;
            }
        }

        #endregion

    }
}