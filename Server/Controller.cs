using Common.Communication;
using Common.Domain;
using ServerApp.SystemOperations;
using ServerApp.SystemOperations.AutomobilSO;
using ServerApp.SystemOperations.KupacSO;
using ServerApp.SystemOperations.KvalifikacijaSO;
using ServerApp.SystemOperations.ProdavacSO;
using ServerApp.SystemOperations.UgovorSO;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerApp
{
    /// <summary>
    /// Kontroler koji upravlja sistemskim operacijama na osnovu zahteva od klijenta.
    /// </summary>
    internal class Controller
    {
        private static readonly Lazy<Controller> instance = new Lazy<Controller>(() => new Controller());

        /// <summary>
        /// Privatni konstruktor da se onemogući direktno instanciranje.
        /// </summary>
        private Controller() { }

        /// <summary>
        /// Jedina instanca Controller klase (Singleton).
        /// </summary>
        public static Controller Instance => instance.Value;

        /// <summary>
        /// Obradjuje zahtev i vraća odgovor sa rezultatom sistemske operacije.
        /// </summary>
        /// <param name="request">Zahtev od klijenta koji sadrži operaciju i argument.</param>
        /// <returns>
        /// <see cref="Response"/> objekat koji sadrži rezultat operacije u polju 
        /// <see cref="Response.Result"/> ili poruku o grešci u polju <see cref="Response.ExceptionMessage"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Kada je prosleđena nepoznata ili nepodržana operacija.
        /// </exception>

        internal Response ProcessRequest(Request request)
        {
            Response response = new Response();

            try
            {
                switch (request.Operation)
                {
                    #region Ugovor Operations
                    case Operation.KreirajUgovor:
                        var kreirajUgovorSO = new KreirajUgovorSO((Ugovor)request.Argument);
                        kreirajUgovorSO.ExecuteTemplate();
                        response.Result = kreirajUgovorSO.Result;
                        break;

                    case Operation.PretraziUgovor:
                        var pretraziUgovorSO = new PretraziUgovorSO((Ugovor)request.Argument);
                        pretraziUgovorSO.ExecuteTemplate();
                        response.Result = pretraziUgovorSO.Result;
                        break;

                    case Operation.VratiListuUgovor:
                        var vratiListuUgovorSO = new VratiListuUgovorSO((Ugovor)request.Argument);
                        vratiListuUgovorSO.ExecuteTemplate();
                        response.Result = vratiListuUgovorSO.Result;
                        break;

                    case Operation.PromeniUgovor:
                        var promeniUgovorSO = new PromeniUgovorSO((Ugovor)request.Argument);
                        promeniUgovorSO.ExecuteTemplate();
                        response.Result = promeniUgovorSO.Result;
                        break;
                    #endregion

                    #region Kupac Operations
                    case Operation.KreirajKupac:
                        var kreirajKupacSO = new KreirajKupacSO((Kupac)request.Argument);
                        kreirajKupacSO.ExecuteTemplate();
                        response.Result = kreirajKupacSO.Result;
                        break;

                    case Operation.PretraziKupac:
                        var pretraziKupacSO = new PretraziKupacSO((Kupac)request.Argument);
                        pretraziKupacSO.ExecuteTemplate();
                        response.Result = pretraziKupacSO.Result;
                        break;

                    case Operation.VratiListuKupac:
                        var vratiListuKupacSO = new VratiListuKupacSO((Kupac)request.Argument);
                        vratiListuKupacSO.ExecuteTemplate();
                        response.Result = vratiListuKupacSO.Result;
                        break;

                    case Operation.PromeniKupac:
                        var promeniKupacSO = new PromeniKupacSO((Kupac)request.Argument);
                        promeniKupacSO.ExecuteTemplate();
                        response.Result = promeniKupacSO.Result;
                        break;

                    case Operation.ObrisiKupac:
                        var obrisiKupacSO = new ObrisiKupacSO((Kupac)request.Argument);
                        obrisiKupacSO.ExecuteTemplate();
                        response.Result = obrisiKupacSO.Result;
                        break;
                    #endregion

                    #region Kvalifikacija Operations
                    case Operation.UbaciKvalifikacija:
                        var ubaciKvalifikacijaSO = new UbaciKvalifikacijaSO((Kvalifikacija)request.Argument);
                        ubaciKvalifikacijaSO.ExecuteTemplate();
                        response.Result = ubaciKvalifikacijaSO.Result;
                        break;

                    case Operation.VratiListuSviKvalifikacija:
                        var vratiListuSviKvalifikacijaSO = new VratiListuSviKvalifikacijaSO();
                        vratiListuSviKvalifikacijaSO.ExecuteTemplate();
                        response.Result = vratiListuSviKvalifikacijaSO.Result;
                        break;
                    #endregion

                    #region Prodavac Operations
                    case Operation.PrijaviProdavac:
                        var prodavacLogin = (Prodavac)request.Argument;
                        var prijaviProdavacSO = new PrijaviProdavacSO(prodavacLogin.Username, prodavacLogin.Password);
                        prijaviProdavacSO.ExecuteTemplate();
                        if (prijaviProdavacSO.Result == null)
                        {
                            throw new Exception("Korisničko ime i šifra nisu ispravni.");
                        }
                        response.Result = prijaviProdavacSO.Result;
                        break;

                    case Operation.VratiListuSviProdavac:
                        var vratiListuSviProdavacSO = new VratiListuSviProdavacSO();
                        vratiListuSviProdavacSO.ExecuteTemplate();
                        response.Result = vratiListuSviProdavacSO.Result;
                        break;
                    #endregion

                    #region List Operations (bez parametara)
                    case Operation.VratiListuSviKupac:
                        var vratiListuSviKupacSO = new VratiListuSviKupacSO();
                        vratiListuSviKupacSO.ExecuteTemplate();
                        response.Result = vratiListuSviKupacSO.Result;
                        break;

                    case Operation.VratiListuSviAutomobil:
                        var vratiListuSviAutomobilSO = new VratiListuSviAutomobilSO();
                        vratiListuSviAutomobilSO.ExecuteTemplate();
                        response.Result = vratiListuSviAutomobilSO.Result;
                        break;
                    #endregion

                    case Operation.None:
                    default:
                        throw new ArgumentException($"Nepoznata operacija: {request.Operation}");
                }

            }
            catch (Exception ex)
            {
                response.ExceptionMessage = ex.Message;
            }

            return response;
        }
    }
}