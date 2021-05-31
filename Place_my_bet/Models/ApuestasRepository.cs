using Microsoft.Ajax.Utilities;
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
    public class ApuestasRepository
    {
        #region Conexión BBDD

        /// <summary>
        /// Método usado para conectarnos a la Base de datos.
        /// </summary>
        /// <returns>Devuelve la conexión</returns>

        /*private MySqlConnection Connect()
        {
            string conString = "Server=127.0.0.1;Port=3306;Database=place_my_bet;Uid=root;password=;SslMode=none";

            MySqlConnection con = new MySqlConnection(conString);

            return con;

        }*/
        #endregion

        #region Devolución de todos los elementos

        /// <summary>
        /// Método que devuelve toda una lista de apuestas de nuestra base de datos.
        /// En sí, en este método nos indicada que se nos debe de devolver la apuesta en formato lista y además,
        /// que se nos incluya el mercado al que va dirigida esa apuesta.
        /// </summary>
        /// <returns>Devuelve la lista de apuestas</returns>

        internal List<Apuesta> Retrieve()
        {

            List<Apuesta> apuestas = new List<Apuesta>();

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                apuestas = context.Apuestas.Include(p => p.Mercado).ToList();
            }

            return apuestas;
        }

        #endregion

        #region Devolución de Apuestas por ID con entity framework

        /// <summary>
        /// En este método lo que estamos haciendo es que, nosotros le paso un int que representará el id
        /// de apuesta que queremos recuperar. Usamos el context y, además, le decimos que 'Where' , es decir,
        /// donde s, que es nuestra nueva variable sea s.ApuestaID y esta sea exactamente igual al id 
        /// introducido. Realmente, se está buscando una apuesta y se está comparando con el id introducido.
        /// ¿Cómo se hace? Mediante el 'FirtsOrDefault'. Esto significa el primero por defecto, por tanto, está
        /// haciendo eso, buscar aquella apuesta que tenga un id y que me de el primer resultado encontrado. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Nos devolverá la apuesta por id.</returns>

        internal Apuesta RetrieveById(int id)
        {
            Apuesta apuesta;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                apuesta = context.Apuestas
                    .Where(s => s.ApuestaId == id)
                    .FirstOrDefault();

            }
            
            return apuesta;
        }

        #endregion

        #region Inserción de apuestas con entity framework
        /// <summary>
        /// Método mediante el cual insertamos la apuesta a nuestra base de datos.
        /// En primer lugar, recibiremos nuestra apuesta por postman.
        /// Luego de ello, comprobaremos qué tipo de apuesta es, es decir, si es UNDER u es OVER.
        /// Cuando sepamos eso llamaremos a nuestro método para obtener su respectiva cuota.
        /// Estos pueden ser: obtainsUnderCuote u obtainsOverCuote.
        /// Luego a la apuesta introducida le daremos el valor de la cuota que hemos obtenido en nuestro método.
        /// Después de ello, haremos un context.Apuestas.Add(apuesta) para añadir nuestra apuesta a nuestra BBDD y
        /// la guardaremos con context.SaveChanges();
        /// Luego de ello, nos declararemos una variable de tipo Mercado y le daremos un mercado que se 
        /// corresponda con el MercadoId de la apuesta.
        /// Finalmente, haremos un update de Mercado mediante el método updateMercado y a este le pasaremos
        /// dos variables, nuestra apuesta y el mercado obtenido.
        /// </summary>
        /// <param name="apuesta"></param>

        internal bool SavebyEntity(Apuesta apuesta)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();
            Mercado mercado = RetrieveMercado(apuesta.MercadoId);
            bool saved = false;

            if(mercado != null)
            {
                if(mercado.Bloqueado != true)
                {
                    saved = true;

                    apuesta.Fecha = DateTime.Now;

                    double cuota = 0;

                    if (apuesta.Tipo_Apuesta == "UNDER")
                    {
                        cuota = obtainsUnderCuote(apuesta.MercadoId);
                    }
                    else
                    {
                        cuota = obtainsOverCuote(apuesta.MercadoId);

                    }

                    apuesta.Tipo_Cuota = cuota;

                    context.Apuestas.Add(apuesta);
                    context.SaveChanges();

                    updateMercado(apuesta, mercado);
                }
                
            }

            return saved;
        }

        #endregion

        #region Update Mercado con entity Framework
        /// <summary>
        /// Este método se encargará de actualizar un mercado mediante el uso de dos variables:
        /// La primera variable de tipo Apuesta llamada apuesta y la segunda variable de tipo Mercado llamada
        /// mercado.
        /// 
        /// Recibe ambas variables, luego comprobaremos qué tipo de apuesta hemos realizado, es decir, si es
        /// UNDER u OVER. 
        /// 
        /// Una vez hecho eso, obtendremos el dinero que vamos a insertar en UNDER mediante el método
        /// newQuantityMoney_Under, al cual le pasaremos la apuesta y el mercado.Dinero_Under, es decir, 
        /// le pasamos la apuesta la cual tendrá el dinero apostado y el mercado.Dinero_Under para saber cuánto
        /// dinero en UNDER hay.
        /// 
        /// Además, hará lo mismo con OVER. Dependiendo de qué tipo de apuesta se realice, se efectuará antes
        /// la obtención del dinero respecto a uno u otro.
        /// Después de ello se hará un update de ese mercado mediante context.Mercados.Update y luego, para
        /// guardarlos, un context.SaveChanges();
        /// 
        /// Una vez ya hemos guardado nuestro mercado lo que haremos será obtener la probabilidad de OVER y de 
        /// UNDER. Esto lo obtendremos pasándole por parametro una variable de tipo Apuesta llamada apuesta y
        /// dos variables de tipo double llamada dinero_Under y dinero_Over. Estas dos se irán alternando en 
        /// posición dependiendo del tipo de apuesta realizada, es decir, una se producirá antes que la otra
        /// dependiendo de si el tipo de apuesta es OVER o UNDER.
        /// 
        /// Luego obtendremos la cuota de este mercado mediante dos métodos: ObteinsNewCuotaUnder y ObteinsNewCuotaOver.
        /// En el primero se introducirá la apuesta y el mercado.Dinero_Under y en el segundo la apuesta y el mercado.Dinero_Over.
        /// Como se ha comentado con anterioridad, esto variará dependiendo del tipo de apuesta insertada, pues puede producirse
        /// antes u otro y viceversa. 
        /// 
        /// Luego de ello, se producirá un context.Mercados.Update(mercado); para actualizar otra vez el mercado y 
        /// un context.SaveChanges(); para guardar los cambios. 
        /// </summary>
        /// <param name="apuesta"></param>
        /// <param name="mercado"></param>

        internal void updateMercado(Apuesta apuesta, Mercado mercado)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            double probability_Under = 0;
            double probability_Over = 0;
            double cuota_Under = 0;
            double cuota_Over = 0;
            double dinero_Under = 0;
            double dinero_Over = 0;

            if (apuesta.Tipo_Apuesta == "UNDER")
            {
                dinero_Under = newQuantityMoney_Under(apuesta, mercado.Dinero_Under);
                dinero_Over = newQuantityMoney_Over(apuesta, mercado.Dinero_Over);

                mercado.Dinero_Under = dinero_Under;
                mercado.Dinero_Over = dinero_Over;

                context.Mercados.Update(mercado);
                context.SaveChanges();

                probability_Under = obtainsProbabilityUnder(apuesta, dinero_Under, dinero_Over);
                probability_Over = obtainsProbabilityOver(apuesta, dinero_Over, dinero_Under);
                cuota_Under = ObteinsNewCuotaUnder(probability_Under);
                cuota_Over = ObteinsNewCuotaOver(probability_Over);


                mercado.Cuota_Under = cuota_Under;
                mercado.Cuota_Over = cuota_Over;

            }
            else
            {
                dinero_Over = newQuantityMoney_Over(apuesta, mercado.Dinero_Over);
                dinero_Under = newQuantityMoney_Under(apuesta, mercado.Dinero_Under);

                mercado.Dinero_Over = dinero_Over;
                mercado.Dinero_Under = dinero_Under;

                context.Mercados.Update(mercado);
                context.SaveChanges();

                probability_Over = obtainsProbabilityOver(apuesta, dinero_Over, dinero_Under);
                probability_Under = obtainsProbabilityUnder(apuesta, dinero_Under, dinero_Over);

                cuota_Over = ObteinsNewCuotaOver(probability_Over);
                cuota_Under = ObteinsNewCuotaUnder(probability_Under);

                mercado.Cuota_Over = cuota_Over;
                mercado.Cuota_Under = cuota_Under;
            }

            context.Mercados.Update(mercado);
            context.SaveChanges();

        }

        #endregion

        #region Obtención de cuota Under con entity framework
        /// <summary>
        /// Método que devuelve la cuota Under.
        /// A este método le pasaremos el idMerc, es decir, una variable de tipo int que representará el id
        /// de mercado.
        /// 
        /// Una vez obtenida nos creamos una variable de tipo Mercado y con ella obtendremos un mercado 
        /// mediante el uso del context.
        /// 
        /// Aquí tendremos que mostrar qué tipo de directrices se ha utilzado: 
        /// mercado = context.Mercados ==> Dame el mercado.
        /// .Where(s => s.MercadoId == idMerc) ==> Donde s sea s.MercadoId y este sea exactamente igual a idMerc, que es la variable int introducida
        /// .FirstOrDefault(); ==> Además, que sea el primero o el por defecto que se refiera a la variable anterior. 
        /// </summary>
        /// <param name="idMerc"></param>
        /// <returns>Nos devolverá mercado.Cuota_Under</returns>
        /// 
        internal double obtainsUnderCuote(int idMerc)
        {
            Mercado mercado;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                mercado = context.Mercados
                    .Where(s => s.MercadoId == idMerc)
                    .FirstOrDefault();
            }

            return mercado.Cuota_Under;
        }

        #endregion

        #region Obtencion de cuota Over con entity framework
        /// <summary>
        /// Método que devuelve la cuota Over.
        /// A este método le pasaremos el idMerc, es decir, una variable de tipo int que representará el id
        /// de mercado.
        /// Una vez obtenida nos creamos una variable de tipo Mercado y con ella obtendremos un mercado 
        /// mediante el uso del context.
        /// Aquí tendremos que mostrar qué tipo de directrices se ha utilzado: 
        /// mercado = context.Mercados ==> Dame el mercado.
        /// .Where(s => s.MercadoId == idMerc) ==> Donde s sea s.MercadoId y este sea exactamente igual a idMerc, que es la variable int introducida
        /// .FirstOrDefault(); ==> Además, que sea el primero o el por defecto que se refiera a la variable anterior. 
        /// </summary>
        /// <param name="idMerc"></param>
        /// <returns>Nos devolverá mercado.Cuota_Over</returns>
        /// 
        internal double obtainsOverCuote(double idMerc)
        {
            Mercado mercado;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {

                mercado = context.Mercados
                    .Where(s => s.MercadoId == idMerc)
                    .FirstOrDefault();
            }

            return mercado.Cuota_Over;
        }

        #endregion

        #region Probabilidad Under con entity framework

        /// <summary>
        /// Método que nos devolverá la probabilidad de UNDER.
        /// Este recibe tres variables: Una variable de tipo Apuesta llamada bet y dos variables de tipo double
        /// m_Dinero_Under y m_Dinero_Over.
        /// Con ello, declararemos una varible de tipo double llamada probability y esta será el resultado de
        /// la división de m_Dinero_Under entre la suma de m_Dinero_Under + m_Dinero_Over.
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="m_Dinero_Under"></param>
        /// <param name="m_Dinero_Over"></param>
        /// <returns>Se retornará la probabilidad de UNDER.</returns>

        internal double obtainsProbabilityUnder(Apuesta bet, double m_Dinero_Under, double m_Dinero_Over)
        {

            double probability = m_Dinero_Under / (m_Dinero_Under + m_Dinero_Over);

            return probability;
        }
        #endregion

        #region Probabilidad Over con entity framework

        /// <summary>
        /// Método que nos devolverá la probabilidad de UNDER.
        /// Este recibe tres variables: Una variable de tipo Apuesta llamada bet y dos variables de tipo double
        /// m_Dinero_Over y m_Dinero_Under.
        /// Con ello, declararemos una varible de tipo double llamada probability y esta será el resultado de
        /// la división de m_Dinero_Over entre la suma de m_Dinero_Over + m_Dinero_Under.
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="m_Dinero_Over"></param>
        /// <param name="m_Dinero_Under"></param>
        /// <returns>Devolveremos la probabilidad de OVER</returns>

        internal double obtainsProbabilityOver(Apuesta bet, double m_Dinero_Over, double m_Dinero_Under)
        {


            double probability = m_Dinero_Over / (m_Dinero_Over + m_Dinero_Under);

            return probability;
        }
        #endregion

        #region ObteinsNewCuotaUnder con entity framework

        /// <summary>
        /// Método que nos devuelve la nueva cuota de UNDER en el mercado que hemos indicado.
        /// A este método le pasamos por variable la probabilidad de UNDER.
        /// Asimismo, nos declaramos una constante que será la cuota fija y una variable que será
        /// la nueva cuota de under.
        /// Esta cuota obtendrá el resultante de los cálculos que se deben de realizar.
        /// </summary>
        /// <param name="probability"></param>
        /// <returns>Nos retornará la NUEVA cuota de UNDER</returns>

        internal double ObteinsNewCuotaUnder(double probability)
        {
            const double cuotaFija = 0.95;

            double cuota_Under = Math.Round(((1 / probability) * cuotaFija), 2, MidpointRounding.AwayFromZero);

            return cuota_Under;
        }

        #endregion

        #region obtenisNewCuotaOver con entity framework

        /// <summary>
        /// Método que nos devuelve la nueva cuota de OVER en el mercado que hemos indicado.
        /// A este método le pasamos por variable la probabilidad de OVER.
        /// Asimismo, nos declaramos una constante que será la cuota fija y una variable que será
        /// la nueva cuota de over.
        /// Esta cuota obtendrá el resultante de los cálculos que se deben de realizar.
        /// </summary>
        /// <param name="probability"></param>
        /// <returns>Nos retornará la NUEVA cuota de OVER</returns>

        internal double ObteinsNewCuotaOver(double probability)
        {
            const double cuotaFija = 0.95;

            double cuota_Over = Math.Round(((1 / probability) * cuotaFija), 2, MidpointRounding.AwayFromZero);

            return cuota_Over;
        }

        #endregion

        #region newQuantityMoney_Under

        /// <summary>
        /// Método que se encarga de devolver y realizar la nueva cantidad de dinero que habrá en el mercado designado en UNDER.
        /// Este método obtendrá una variable de tipo Apuesta y una variable de tipo double llamada dinero_Under_m.
        /// En ella comprobaremos si la apuesta introducida es de tipo UNDER. Si lo es, dineros que dinero_Under será igual a
        /// el dinero apostado en la apuesta + el dinero que de dinero_Under_m. 
        /// Sino es el caso, el dinero en Under será igual al dinero de dinero_Under_m.
        /// </summary>
        /// <param name="apuesta"></param>
        /// <param name="dinero_Under_m"></param>
        /// <returns>Devuelve el dinero que hay en mercado en UNDER</returns>

        internal double newQuantityMoney_Under(Apuesta apuesta, double dinero_Under_m)
        {
            double dinero_Under = dinero_Under_m;

            if (apuesta.Tipo_Apuesta.ToUpper() == "UNDER")
            {
                dinero_Under = apuesta.Dinero_Apostado + dinero_Under_m;
            }

            return dinero_Under;
        }

        #endregion

        #region newQuantityMoney_Over

        /// <summary>
        /// Método que se encarga de devolver y realizar la nueva cantidad de dinero que habrá en el mercado designado en OVER.
        /// Este método obtendrá una variable de tipo Apuesta y una variable de tipo double llamada dinero_Over_m.
        /// En ella comprobaremos si la apuesta introducida es de tipo OVER. Si lo es,  dinero_Over será igual a
        /// el dinero apostado en la apuesta + el dinero que de dinero_Over_m. 
        /// Sino es el caso, el dinero en Over será igual al dinero de dinero_Over_m. 
        /// </summary>
        /// <param name="apuesta"></param>
        /// <param name="dinero_Over_m"></param>
        /// <returns>Devuelve el dinero que hay en mercado en OVER</returns>

        internal double newQuantityMoney_Over(Apuesta apuesta, double dinero_Over_m)
        {

            double dinero_Over = dinero_Over_m;

            if (apuesta.Tipo_Apuesta.ToUpper() == "OVER")
            {
                dinero_Over = apuesta.Dinero_Apostado + dinero_Over_m;
            }

            return dinero_Over;
        }

        #endregion

        #region Obtener Mercado con entity framework

        /// <summary>
        /// Este método nos devolverá un mercado en concreto.
        /// Este método obtendrá una variable de tipo int llamada id, representando el id de Mercado. 
        /// Nos declaramos una variable de tipo Mercado.
        /// Luego, utilizaremos un PlaceMyBetContext. Con ello diremos lo siguiente:
        /// mercado = context.Mercados ==> es decir, mercado es igual al context de Mercados
        /// .Where(s => s.MercadoId == id) ==> es decir, donde s sea representativo de s.MercadoId y este sea exactamente igual al id
        /// introducido.
        /// .FirstOrDefault(); ==> es decir, que una vez ya le indicamos que tiene que ser exactamente igual al id introducido, esta parte
        /// se encargará de devolvernos exactamente el primero por defecto que represente a ese id, sin fijarse en los demás. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Nos devolverá el mercado que nosotros queremos. </returns>

        internal Mercado RetrieveMercado(int id)
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

        #region Convertir Apuesta en ApuestaDTO

        /// <summary>
        /// Método que recibe una variable de tipo Apuesta llamada apuesta y la moldea para otorgar una variable de tipo
        /// ApuestaDTO.
        /// 
        /// Para ello se definirá una variable de tipo PlaceMyBetContext y una variable de tipo Apuesta.
        /// 
        /// Luego, diremos que nuestra Apuesta llamada bet será igual a context.Apuestas
        /// y además, .Include(ap => ap.Mercado) ==> es decir, que en ap tenga ap.Mercado, es decir, que me incluya el mercado
        /// adjunto a esa apuesta.
        /// Luego, con .FirstOrDefault(ap => ap.ApuestaId == apuesta.ApuestaId); ==> estaremos diciendo que nos otorguen aquella
        /// apuesta cuyo id sea el primero por defecto exactamente igual al id de la apuesta pasada por variable.
        /// 
        /// Finalmente, una vez obtengamos esta apuesta, la convertiremos en una ApuestaDTO, introduciendo los parametros 
        /// recogidos en nuestra apuesta, que ha sido llamada mediante el context. 
        /// 
        /// El único punto a tener en cuenta es bet.Mercado.EventoId ==> Esto sucede porque en Apuesta tenemos una variable de 
        /// tipo Mercado y si mos dirigimos a la clase Mercado, veremos que existe un campo llamado EventoId.
        /// Esto es necesario para otorgarle a ApuestaDTO el evento adherido del mercado que representará esa apuesta. 
        /// </summary>
        /// <param name="apuesta"></param>
        /// <returns></returns>

        private static ApuestaDTO ToDTO(Apuesta apuesta)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            //Apuesta bet recibe un context de apuestas que inlcuya un mercado y que este mercado tenga como característica que la apuesta que queremos recuperar sea exactamente igual a la que pasamos por parametro.
            Apuesta bet = context.Apuestas
                .Include(ap => ap.Mercado)
                .FirstOrDefault(ap => ap.ApuestaId == apuesta.ApuestaId);

            return new ApuestaDTO(bet.UsuarioId, bet.Mercado.EventoId, bet.Tipo_Apuesta, bet.Tipo_Cuota, bet.Dinero_Apostado);
        }

        #endregion

        #region Devolver una ApuestaDTO

        /// <summary>
        /// Método que devuelve una ApuestaDTO.
        /// 
        /// En sí, nos declaramos una lista de apuestas DTO y un PlaceMyBetContext.
        /// 
        /// Posteriormente, indicamos que apuestas será igual a context de Apuestas, es decir,
        /// apuestas = context.Apuestas y que realizaremos una selección en tanto en cuanto que ap sea
        /// un ap modificado por el método ToDTO, el cual se encarga de convertir una apuesta de nuestrta BBDD
        /// en una apuestaDTO. Esto se muestra así ==> .Select(ap => ToDTO(ap)).
        /// Finalmente, se utilizará el .ToList(); para decir que nos devuelva una lista de ello, es decir,
        /// que nos devuelva todas las apuestas modificadas con el DTO. 
        /// </summary>
        /// <returns>Devolución de una lista de apuestasDTO.</returns>

        internal List<ApuestaDTO> RetrieveApuestaDTO()
        {
            List<ApuestaDTO> apuestas = new List<ApuestaDTO>();

            PlaceMyBetContext context = new PlaceMyBetContext();

            apuestas = context.Apuestas
                .Select(ap => ToDTO(ap))
                .ToList();

            return apuestas;
        }

        #endregion

        #region Devolver Apuesta por email

        internal List<Apuesta> RetrieveByEmail(string email)
        {
            List<Apuesta> apuesta;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                apuesta = context.Apuestas
                    .Include(p => p.Mercado)
                    .Include(p => p.Mercado.Evento)
                    .Where(p => p.UsuarioId == email)
                    .ToList();
            }

            return apuesta;
        }

        #endregion

        #region Devolver Apuesta por Mercado

        internal List<Apuesta> RetrieveByMarket(int mercadoId)
        {
            List<Apuesta> apuesta;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                apuesta = context.Apuestas
                    .Include(p => p.Mercado)
                    .Where(p => p.MercadoId == mercadoId)
                    .ToList();
            }

            return apuesta;
        }

        #endregion

        #region Devolver Apuesta por Mercado

        internal List<Apuesta> RetrieveByEvent(int eventoId)
        {
            List<Apuesta> apuesta;

            using (PlaceMyBetContext context = new PlaceMyBetContext())
            {
                apuesta = context.Apuestas
                    .Include(p => p.Mercado).ThenInclude(p => p.Evento)
                    .Where(p => p.Mercado.EventoId == eventoId)
                    .ToList();
            }

            return apuesta;
        }

        #endregion

        #region Insert Mercado desde Apuesta

        internal bool SaveNewMarket(int idEvento)
        {
            PlaceMyBetContext context = new PlaceMyBetContext();
            Evento evento = new Evento();

            evento = context.Eventos
                    .Where(s => s.EventoId == idEvento)
                    .FirstOrDefault();


            if(evento != null)
            {
                introduceFirtsMarket(idEvento);

                introduceSecondMarket(idEvento);

                introduceThirdMarket(idEvento);

                return true;
            }
            else
            {
                return false;
            }

        }

        internal void introduceFirtsMarket(int idEvento)
        {
            Mercado mercado = new Mercado();

            PlaceMyBetContext context = new PlaceMyBetContext();

            mercado.EventoId = idEvento;
            mercado.Tipo_over_under = 1.5;
            mercado.Cuota_Over = 1.9;
            mercado.Cuota_Under = 1.9;
            mercado.Dinero_Over = 100;
            mercado.Dinero_Under = 100;

            context.Mercados.Add(mercado);
            context.SaveChanges();

        }

        internal void introduceSecondMarket(int idEvento)
        {
            Mercado mercado = new Mercado();

            PlaceMyBetContext context = new PlaceMyBetContext();

            mercado.EventoId = idEvento;
            mercado.Tipo_over_under = 2.5;
            mercado.Cuota_Over = 1.9;
            mercado.Cuota_Under = 1.9;
            mercado.Dinero_Over = 100;
            mercado.Dinero_Under = 100;

            context.Mercados.Add(mercado);
            context.SaveChanges();
        }

        internal void introduceThirdMarket(int idEvento)
        {
            Mercado mercado = new Mercado();

            PlaceMyBetContext context = new PlaceMyBetContext();

            mercado.EventoId = idEvento;
            mercado.Tipo_over_under = 3.5;
            mercado.Cuota_Over = 1.9;
            mercado.Cuota_Under = 1.9;
            mercado.Dinero_Over = 100;
            mercado.Dinero_Under = 100;

            context.Mercados.Add(mercado);
            context.SaveChanges();
        }

        #endregion

        internal List<String> RetrieveDates()
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            List<String> dates = context.Apuestas.Select(ap => ap.Fecha.ToShortDateString()).DistinctBy(ap => ap).ToList();

            return dates;
        }

        internal List<int> RetrieveQuantityBetsByDate()
        {
            PlaceMyBetContext context = new PlaceMyBetContext();

            List<int> quantityBets = new List<int>();

            List<String> dates = RetrieveDates();

            List<String> allDates = context.Apuestas.Select(ap => ap.Fecha.ToShortDateString()).ToList();

           foreach (String date in dates)
           {
                int qBet = allDates.Count(bet => bet == date);
                quantityBets.Add(qBet);
           }

            return quantityBets;
        }

        //============================================================================================================================//

        #region Devolución de todos los elementos DTO

        /// <summary>
        /// Método que devuelve una lista con unos parámetrtos prefijados. En este caso es una lista de ApuestaDTO
        /// </summary>
        /// <returns>Devuelve una lista de ApuestaDTO</returns>

        internal List<Apuesta> RetrievebyDTO()
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            // MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            //command.CommandText = "SELECT apuesta.Tipo_Cuota, apuesta.Dinero_Apostado, apuesta.Fecha, apuesta.Email_Usuario, apuesta.Tipo_Apuesta, mercado.Tipo_over_under FROM apuesta INNER JOIN mercado WHERE apuesta.idMercado = mercado.IdMercado;";

            try
            {
                //con.Open();

                // MySqlDataReader res = command.ExecuteReader();

                Apuesta apu = null;

                List<Apuesta> apuestas = new List<Apuesta>();

                //while (res.Read())
                //{
                //    apu = new ApuestaDTO(res.GetString(3), res.GetDouble(5), res.GetDouble(0), res.GetDouble(1), Convert.ToDateTime(res.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss")), res.GetString(4));

                //    apuestas.Add(apu);
                //}

                //con.Close();

                return apuestas;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Se ha producido un error de conexión");
                return null;
            }
        }

        #endregion

        #region Inserción de datos a la BBDD

        /// <summary>
        /// Método que insertará una apuesta en nuestra BBDD. Además, se calcularán la probabilidad, la inserción del dinero a OVER o Under y el cambio de Cuota de Over o Under.
        /// </summary>
        /// <param name="ap">Representará el objeto apuesta que utilizaremos para todos y cada uno de los procesos que hagamos.</param>

        internal void Save(Apuesta ap)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            ap.Tipo_Cuota = devolverCuota(ap.MercadoId, ap.Tipo_Apuesta);

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            DateTime date = DateTime.Now;
            string actualDate;
            actualDate = date.ToString("yyyy-MM-dd HH:mm:ss");

            //command.CommandText = "INSERT INTO apuesta (apuesta.idMercado, apuesta.Tipo_Apuesta, apuesta.Tipo_Cuota, apuesta.Dinero_Apostado, apuesta.Fecha, apuesta.Email_Usuario) VALUES ('" + ap.IdMercado + "','" + ap.Tipo_Apuesta + "','" + ap.Tipo_Cuota + "','" + ap.Dinero_Apostado + "','" + actualDate + "','" + ap.Email_Usuario + "');";


            //Debug.WriteLine("comando: " + command.CommandText);

            double probUnder = 0;
            double probOver = 0;

            try
            {
                //con.Open();
                //command.ExecuteNonQuery();
                //con.Close();
                if (ap.Tipo_Apuesta == "UNDER")
                {
                    probUnder = ProbabilidadUnder(ap);
                    probOver = ProbabilidadOver(ap);
                }
                else
                {
                    probOver = ProbabilidadOver(ap);
                    probUnder = ProbabilidadUnder(ap);
                }

                CuotaOver(ap, probOver);
                CuotaUnder(ap, probUnder);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Se ha producido un error de conexión");
            }
        }

        #endregion

        #region Devolución apuestas y eventos por usuario y por tipo de Mercado.

        //internal List<ApuestaEmailDTO> RetrievebyEmailandMarketType(string email, int idm)
        //{
        //    CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
        //    culInfo.NumberFormat.NumberDecimalSeparator = ".";
        //    culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        //    culInfo.NumberFormat.PercentDecimalSeparator = ".";
        //    culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        //    System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

        //    //MySqlConnection con = Connect();

        //    //MySqlCommand command = con.CreateCommand();

        //    //command.CommandText = "SELECT evento.idEvento, apuesta.Tipo_Apuesta, apuesta.Tipo_Cuota, apuesta.Dinero_Apostado FROM apuesta  INNER JOIN mercado ON mercado.IdMercado = apuesta.idMercado INNER JOIN evento ON evento.idEvento = mercado.Id_Evento WHERE apuesta.Email_Usuario = @A AND apuesta.idMercado = @A2;";

        //    //command.Parameters.AddWithValue("@A", email);

        //    //command.Parameters.AddWithValue("@A2", idm);

        //    try
        //    {
        //        //con.Open();

        //        //MySqlDataReader res = command.ExecuteReader();

        //        ApuestaEmailDTO apu = null;

        //        List<ApuestaEmailDTO> apuestas = new List<ApuestaEmailDTO>();

        //        //while (res.Read())
        //        //{
        //        //    apu = new ApuestaEmailDTO(res.GetInt32(0), res.GetString(1), res.GetDouble(2), res.GetDouble(3));

        //        //    apuestas.Add(apu);
        //        //}

        //        //con.Close();

        //        return apuestas;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("Se ha producido un error de conexión");
        //        return null;
        //    }
        //}

        #endregion

        #region Devolución apuestas por tipo Mercado y Email de usuario.

        //internal List<ApuestabyMarket> RetrieveBetByTypeBetandEmail(int idm, string email)
        //{
        //    CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
        //    culInfo.NumberFormat.NumberDecimalSeparator = ".";
        //    culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        //    culInfo.NumberFormat.PercentDecimalSeparator = ".";
        //    culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        //    System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

        //    //MySqlConnection con = Connect();

        //    //MySqlCommand command = con.CreateCommand();

        //    //command.CommandText = "SELECT mercado.Tipo_over_under, apuesta.Tipo_Apuesta, apuesta.Tipo_Cuota, apuesta.Dinero_Apostado FROM apuesta  INNER JOIN mercado ON mercado.IdMercado = apuesta.idMercado INNER JOIN evento ON evento.idEvento = mercado.Id_Evento WHERE apuesta.idMercado = @A AND apuesta.Email_Usuario = @A2;";

        //    //command.Parameters.AddWithValue("@A", idm);

        //    //command.Parameters.AddWithValue("@A2", email);

        //    try
        //    {
        //        //con.Open();

        //        //MySqlDataReader res = command.ExecuteReader();

        //        ApuestabyMarket apu = null;

        //        List<ApuestabyMarket> apuestas = new List<ApuestabyMarket>();

        //        //while (res.Read())
        //        //{
        //        //    apu = new ApuestabyMarket(res.GetDouble(0), res.GetString(1), res.GetDouble(2), res.GetDouble(3));

        //        //    apuestas.Add(apu);
        //        //}

        //        //con.Close();

        //        return apuestas;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("Se ha producido un error de conexión");
        //        return null;
        //    }
        //}

        #endregion

        #region Obtención Cuota

        /// <summary>
        /// Método mediante el cual obtendremos la cuota de nuestra apuesta.
        /// </summary>
        /// <param name="idmerc">Representa el idMercado y lo usaremos para buscar nuestro mercado</param>
        /// <param name="ov_Und">Representa la apuesta, es decir,  representa el tipo de apuesta: OVER o UNDER</param>
        /// <returns></returns>

        internal double devolverCuota(int idmerc, string ov_Und)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            double cuota = 0;

            //if (ov_Und.ToUpper() == "UNDER")
            //{
            //    command.CommandText = "select * from mercado where IdMercado = @A";

            //    command.Parameters.AddWithValue("@A", idmerc);
            //}
            //else
            //{
            //    command.CommandText = "select * from mercado where IdMercado = @A";

            //    command.Parameters.AddWithValue("@A", idmerc);
            //}

            try
            {
                //    con.Open();

                //    MySqlDataReader res = command.ExecuteReader();

                //    Mercado merc = null;

                //    while (res.Read())
                //    {
                //        merc = new Mercado(res.GetInt32(0), res.GetDouble(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetInt32(6));

                //    }

                //    con.Close();


                //    if (ov_Und.ToUpper() == "UNDER")
                //    {
                //        cuota = merc.Cuota_Under;
                //    }
                //    else
                //    {
                //        cuota = merc.Cuota_Over;

                //    }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Se ha producido un error de conexión");

            }

            return cuota;
        }

        #endregion

        #region Probabilidad de Under

        /// <summary>
        /// Método que nos calculará la cuota que tendremos que actualizar en mercados. 
        /// Para ello crearemos un objeto mercados y realizaremos una llamada a la BBDD para que nos dé la información pertinente.
        /// Luego usaremos esa información para realizar un update del dinero que hemos apostado y unirlo con el existente en la BBDD.
        /// Finalmente, devolveremos la probabilidad.
        /// </summary>
        /// <param name="apuesta">Objeto que usaremos para realizar todos los cálculos y gestiones pertinentes</param>
        /// <returns>Devolveremos la información de la probabilidad</returns>

        internal double ProbabilidadUnder(Apuesta apuesta)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            Mercado merc = ObtenerMercado(apuesta.MercadoId);

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            double dinero_Under = merc.Dinero_Under;

            if (apuesta.Tipo_Apuesta.ToUpper() == "UNDER")
            {
                dinero_Under = apuesta.Dinero_Apostado + merc.Dinero_Under;
            }

            double probability = dinero_Under / (dinero_Under + merc.Dinero_Over);

            //if (apuesta.Tipo_Apuesta.ToUpper() == "UNDER")
            //{
            //    command.CommandText = "UPDATE mercado SET mercado.Dinero_Under = @A3 WHERE mercado.IdMercado = @A2;";

            //    command.Parameters.AddWithValue("@A2", apuesta.IdMercado);

            //    command.Parameters.AddWithValue("@A3", dinero_Under);

            //    try
            //    {
            //        con.Open();
            //        command.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    catch (MySqlException e)
            //    {
            //        Debug.WriteLine("Se ha producido un error de conexión");
            //    }

            //}

            return probability;

        }

        #endregion

        #region Probabilidad de Over

        /// <summary>
        /// Método que nos calculará la cuota que tendremos que actualizar en mercados. 
        /// Para ello crearemos un objeto mercados y realizaremos una llamada a la BBDD para que nos dé la información pertinente.
        /// Luego usaremos esa información para realizar un update del dinero que hemos apostado y unirlo con el existente en la BBDD.
        /// Finalmente, devolveremos la probabilidad.
        /// </summary>
        /// <param name="apuesta">Objeto que usaremos para realizar todos los cálculos y gestiones pertinentes</param>
        /// <returns>Devolveremos la información de la probabilidad</returns>

        internal double ProbabilidadOver(Apuesta apuesta)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            Mercado merc = ObtenerMercado(apuesta.MercadoId);

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            double dinero_Over = merc.Dinero_Over;

            if (apuesta.Tipo_Apuesta.ToUpper() == "OVER")
            {
                dinero_Over = apuesta.Dinero_Apostado + merc.Dinero_Over;
            }

            double probability = dinero_Over / (dinero_Over + merc.Dinero_Under);

            if (apuesta.Tipo_Apuesta.ToUpper() == "OVER")
            {
                //command.CommandText = "UPDATE mercado SET mercado.Dinero_Over = @A3 WHERE mercado.IdMercado = @A2;";

                //command.Parameters.AddWithValue("@A2", apuesta.IdMercado);

                //command.Parameters.AddWithValue("@A3", dinero_Over);

                //try
                //{
                //    con.Open();
                //    command.ExecuteNonQuery();
                //    con.Close();
                //}
                //catch (MySqlException e)
                //{
                //    Debug.WriteLine("Se ha producido un error de conexión");
                //}
            }

            return probability;

        }

        #endregion

        #region Cuota Over

        /// <summary>
        /// Método mediante el cual recalcularemos la cuota de Under u Over.
        /// </summary>
        /// <param name="apuesta">Objeto de tipo Apuesta.</param>
        /// <param name="probabilidad">Variable de tipo double que obtendremos de efectuar el método probabilidad.</param>

        public void CuotaOver(Apuesta apuesta, double probabilidadOver)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            const double cuotaFija = 0.95;


            double cuotaOver = Math.Round(((1 / probabilidadOver) * cuotaFija), 2, MidpointRounding.AwayFromZero);

            //command.CommandText = "UPDATE mercado SET mercado.Cuota_Over = '" + cuotaOver + "' WHERE mercado.IdMercado = '" + apuesta.IdMercado + "';";

            //try
            //{
            //    con.Open();
            //    command.ExecuteNonQuery();
            //    con.Close();
            //}
            //catch (MySqlException e)
            //{
            //    Debug.WriteLine("Se ha producido un error de conexión");
            //}

        }

        #endregion

        #region Cuota Over

        /// <summary>
        /// Método mediante el cual recalcularemos la cuota de Under u Over.
        /// </summary>
        /// <param name="apuesta">Objeto de tipo Apuesta.</param>
        /// <param name="probabilidad">Variable de tipo double que obtendremos de efectuar el método probabilidad.</param>

        public void CuotaUnder(Apuesta apuesta, double probabilidadUnder)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            const double cuotaFija = 0.95;


            double cuotaUnder = Math.Round(((1 / probabilidadUnder) * cuotaFija), 2, MidpointRounding.AwayFromZero);

            //command.CommandText = "UPDATE mercado SET mercado.Cuota_Under = '" + cuotaUnder + "' WHERE mercado.IdMercado = '" + apuesta.IdMercado + "';";

            //try
            //{
            //    con.Open();
            //    command.ExecuteNonQuery();
            //    con.Close();
            //}
            //catch (MySqlException e)
            //{
            //    Debug.WriteLine("Se ha producido un error de conexión");
            //}

        }

        #endregion

        #region Obtención cuota Under

        internal double devolverCuotaUnder(int idmerc)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            double cuota = 0;

            //command.CommandText = "select * from mercado where IdMercado = @A";

            //command.Parameters.AddWithValue("@A", idmerc);


            //try
            //{
            //    con.Open();

            //    MySqlDataReader res = command.ExecuteReader();

            //    Mercado merc = null;

            //    while (res.Read())
            //    {
            //        merc = new Mercado(res.GetInt32(0), res.GetDouble(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetInt32(6));

            //    }

            //    con.Close();

            //   cuota = merc.Cuota_Under;

            //}
            //catch (MySqlException e)
            //{
            //    Debug.WriteLine("Se ha producido un error de conexión");

            //}

            return cuota;
        }

        #endregion

        #region Obtención Cuota Over

        /// <summary>
        /// Método mediante el cual obtendremos la cuota de nuestra apuesta.
        /// </summary>
        /// <param name="idmerc">Representa el idMercado y lo usaremos para buscar nuestro mercado</param>
        /// <param name="ov_Und">Representa la apuesta, es decir,  representa el tipo de apuesta: OVER o UNDER</param>
        /// <returns></returns>

        internal double devolverCuotaOver(int idmerc)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            double cuota = 0;

            //command.CommandText = "select * from mercado where IdMercado = @A";

            //command.Parameters.AddWithValue("@A", idmerc);


            //try
            //{
            //    con.Open();

            //    MySqlDataReader res = command.ExecuteReader();

            //    Mercado merc = null;

            //    while (res.Read())
            //    {
            //        merc = new Mercado(res.GetInt32(0), res.GetDouble(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetInt32(6));

            //    }

            //    con.Close();
            //    cuota = merc.Cuota_Over;

            //}
            //catch (MySqlException e)
            //{
            //    Debug.WriteLine("Se ha producido un error de conexión");

            //}

            return cuota;
        }

        #endregion

        #region Objeto Mercado

        internal Mercado ObtenerMercado(int idMercado)
        {
            CultureInfo culInfo = new System.Globalization.CultureInfo("es-ES");
            culInfo.NumberFormat.NumberDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            culInfo.NumberFormat.PercentDecimalSeparator = ".";
            culInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culInfo;

            //MySqlConnection con = Connect();

            //MySqlCommand command = con.CreateCommand();

            //command.CommandText = "select * from mercado WHERE mercado.IdMercado = @A";

            //command.Parameters.AddWithValue("@A", idMercado);

            Mercado merc = null;

            //try
            //{
            //    con.Open();

            //    MySqlDataReader res = command.ExecuteReader();

            //    while (res.Read())
            //    {
            //        merc = new Mercado(res.GetInt32(0), res.GetDouble(1), res.GetDouble(2), res.GetDouble(3), res.GetDouble(4), res.GetDouble(5), res.GetInt32(6));

            //    }

            //    con.Close();

            //}
            //catch (MySqlException e)
            //{
            //    Debug.WriteLine("Se ha producido un error de conexión");
            //}

            return merc;
        }

        #endregion


    }
}