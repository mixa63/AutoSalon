using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using Common.Communication;

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
        /// <returns>Odgovor sa rezultatom ili porukom o grešci.</returns>-
        private Response ProcessRequest(Request req)
        {
            Response r = new Response();
            try
            {
                switch (req.Operation)
                {
                    case Operation.None:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                r.ExceptionMessage = ex.Message;
            }
            return r;
        }
    }
}