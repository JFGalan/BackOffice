using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Place_my_bet.Models
{
    public class CuentasRepository
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

        #region Devolución de todos los elementos con entityFrameWork

        /// <summary>
        /// Método que nos devolverá una lista de tipo Cuenta con todas las cuentas.
        /// </summary>
        /// <returns></returns>

        internal List<Cuenta> Retrieve()
        {
            List<Cuenta> cuentas = new List<Cuenta>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                cuentas = context.Cuentas.ToList();
            }

            return cuentas;
        }
        #endregion


    }
}