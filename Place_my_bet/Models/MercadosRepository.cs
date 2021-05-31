using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class MercadosRepository
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
        /// Método que devuelve toda una lista de Mercado de nuestra base de datos.
        /// Nos declararemos una variablde tipo Mercado, la cual será una lista y
        /// además, nos declararemos una variable de tipo PlaceMyBetContext.
        /// 
        /// La variable de tipo mercado será igual a context.Mercados y, ademas, que 
        /// esta incluya al Evento en sí, mediante p, es decir, dentro del include,
        /// nos declaramos una variable, p y está incluirá p.Evento. .Include(p => p.Evento).
        /// .ToList(); ==> Esto hará que de todo ello, nos devuelvan una lista.
        /// </summary>
        /// <returns>Devuelve la lista de Mercado</returns>

        internal List<Mercado> Retrieve()
        {
            List<Mercado> mercados = new List<Mercado>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                mercados = context.Mercados
                    .Include(p => p.Evento)
                    .ToList();
            }

            return mercados;
        }
        #endregion

        #region Devolución de una lista de MercadoDTO 

        /// <summary>
        /// Método que nos devuelve una lista de MercadoDTO.
        /// 
        /// Declararemos una variable que será una lista de MercadoDTO y una variable de tipo PlaceMyBetContext.
        /// 
        /// Luego diremos que nuestra lista de MercadoDTO, es decir, evento será igual a context.Mercados, 
        /// es decir, eventos = context.Eventos.
        /// Además, diremos que queremos seleccionar aquellos eventos y convertirlos en ToDTO, es decir,
        /// .Select(p => ToDTO(p)) ==> p sería la variable que declararemos internamente y que modificare-
        /// mos en DTO.
        /// Finalmente, con el .ToList(); ==> Diremos que de todo ello se nos otorgue una lista.
        /// </summary>
        /// <returns>Devolución de lista de MercadoDTO</returns>

        internal List<MercadoDTO> RetrieveMercadoDTO()
        {
            List<MercadoDTO> mercados = new List<MercadoDTO>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                mercados = context.Mercados
                    .Select(p => ToDTO(p))
                    .ToList();
            }

            return mercados;
        }

        #endregion

        #region Transformación de Mercado en MercadoDTO

        /// <summary>
        /// Método que modifica un mercado en un MercadoDTO.
        /// 
        /// Este método recibe un mercado y lo modifica para retornar un MercadoDTO.
        /// </summary>
        /// <param name="mercado"></param>
        /// <returns>Nos retorna un mercado modificado a DTO</returns>

        static internal MercadoDTO ToDTO(Mercado mercado)
        {
            return new MercadoDTO(mercado.Tipo_over_under, mercado.Cuota_Over, mercado.Cuota_Under);
        }

        #endregion

        #region Devolución de Mercados por ID

        /// <summary>
        /// Método que nos devuelve un mercado en concreto mediante un ID.
        /// 
        /// Este método recibe una variable de tipo int que representará el id de mercado.
        /// 
        /// Luego de ello nos haremos una variablde de tipo mercado y otra de PlaceMyBetContext.
        /// 
        /// Con todo ello, mercado será igual a context.Mercados y está recibirá aquel mercado
        /// donde s.MercadoId sea igual al id introducido, .Where(s => s.MercadoId == id), es decir,
        /// en este caso dentro del Where hacemos que s se refiera a s.MercadoId y este lo compara
        /// con el id introducido. 
        /// 
        /// Finalmente, con .FirstOrDefault(); ==> Nos devolverá el mercado que sea el primero 
        /// por defecto que sea exactamente igual al id introducido. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        internal Mercado RetrieveById(int id)
        {
            Mercado mercado;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                mercado = context.Mercados
                    .Where(s => s.MercadoId == id)
                    .FirstOrDefault();

            }

            return mercado;
        }

        #endregion

        #region Inserción de un mercado mediante entity framework

        /// <summary>
        /// Método que se encarga de insertar mercados dentro de la BBDD mediante el entity Framework.
        /// 
        /// En sí, recibirá una variable de tipo Mercado.
        /// 
        /// Luego haremos un context.Mercados.Add(merc); ==> es decir, que lo añadiremos a la BBDD.
        /// Finalmente, haremos un context.SaveChanges(); ==> es decir, guardarmos los cambios. 
        /// </summary>
        /// <param name="merc"></param>

        internal void Save(Mercado merc)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            context.Mercados.Add(merc);
            context.SaveChanges();
        }

        #endregion

       

        internal int BlockAndUnblockMarket(int id, bool block)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();
            Mercado mercado = RetrieveById(id);
            int blocked = 0;

            if (mercado != null)
            {
                if(mercado.Bloqueado == true)
                {
                    blocked = 2;
                }
                else
                {
                    mercado.Bloqueado = block;
                    context.Mercados.Update(mercado);
                    context.SaveChanges();
                    blocked = 1;
                }
              
            }

            return blocked;
        }

        //====================================================================================//

        #region Devolución de todos los elementos mediante DTO

        /// <summary>
        /// Método que devuelve una lista con unos parámetrtos prefijados. En este caso es una lista de MercadoDTO
        /// </summary>
        /// <returns>Devuelve una lista de MercadoDTO</returns>

        internal List<Mercado> RetrievebyDTO()
        {
            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            //command.CommandText = "select * from mercado";

            try
            {
                //con.Open();

                //MySqlDataReader res = command.ExecuteReader();

                Mercado merc = null;

                List<Mercado> mercados = null;

                // List<MercadoDTO> mercados = new List<MercadoDTO>();

                //while (res.Read())
                //{
                //    merc = new MercadoDTO(res.GetDouble(1), res.GetDouble(2), res.GetDouble(3));

                //    mercados.Add(merc);
                //}

                //con.Close();

                return mercados;
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