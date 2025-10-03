using Common.Communication;
using Common.Domain;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;

namespace ServerApp
{
    /// <summary>
    /// Rukuje komunikacijom sa jednim klijentom.
    /// Prima zahteve, obrađuje ih i šalje odgovore.
    /// </summary>
    internal class ClientHandler
    {
        private JsonNetworkSerializer serializer;
        private Socket clientSocket;
        private Server server;
        private Controller controller;

        /// <summary>
        /// Kreira novi ClientHandler za datog klijenta.
        /// </summary>
        /// <param name="clientSocket">Soket klijenta preko kojeg se komunicira.</param>
        /// <param name="server">Referenca na server koji upravlja klijentima.</param>
        public ClientHandler(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            serializer = new JsonNetworkSerializer(clientSocket);
            controller = Controller.Instance;
        }

        /// <summary>
        /// Zatvara konekciju sa klijentom.
        /// </summary>
        internal void CloseSocket()
        {
            clientSocket.Close();
        }

        /// <summary>
        /// Petlja koja prima zahteve od klijenta, obrađuje ih i šalje odgovore.
        /// Zatvara konekciju i uklanja klijenta sa servera kada dođe do prekida komunikacije.
        /// </summary>
        internal void HandleRequest()
        {
            try
            {
                while (true)
                {
                    Request req = serializer.Receive<Request>();
                    Response r = ProcessRequest(req);
                    serializer.Send(r);
                }
            }
            catch (IOException)
            {
                Debug.WriteLine("Konekcija zatvorena od strane klijenta.");
            }
            catch (SocketException)
            {
                Debug.WriteLine("Komunikacija sa klijentom je prekinuta");
            }
            finally
            {
                if (clientSocket.Connected)
                {
                    CloseSocket();
                }
                server.RemoveClient(this);
            }
        }

        /// <summary>
        /// Obradjuje pojedinačni zahtev klijenta i vraća odgovor.
        /// </summary>
        /// <param name="req">Zahtev koji treba obraditi.</param>
        /// <returns>
        /// Response objekat koji sadrži rezultat operacije u polju <see cref="Response.Result"/> 
        /// ili poruku o grešci u polju <see cref="Response.ExceptionMessage"/>.
        /// </returns>
        private Response ProcessRequest(Request req)
        {
            switch (req.Operation)
            {
                case Operation.PrijaviProdavac:
                    req.Argument = serializer.ReadType<Prodavac>(req.Argument);
                    break;
                case Operation.KreirajKupac:
                case Operation.PretraziKupac:
                case Operation.VratiListuKupac:
                case Operation.PromeniKupac:
                case Operation.ObrisiKupac:
                    req.Argument = serializer.ReadKupacPolymorphic(req.Argument);
                    break;
                case Operation.KreirajUgovor:
                case Operation.PretraziUgovor:
                case Operation.VratiListuUgovor:
                case Operation.PromeniUgovor:
                    req.Argument = serializer.ReadType<Ugovor>(req.Argument);
                    break;

                case Operation.UbaciKvalifikacija:
                    req.Argument = serializer.ReadType<Kvalifikacija>(req.Argument);
                    break;

                default:
                    break;
            }
            Response r = controller.ProcessRequest(req);
            
            return r;
        }
    }
}