using Common.Communication;
using Common.Domain;
using System.Net.Sockets;
using System.Text.Json;

namespace Client
{
    public class Communication
    {
        private static Communication instance;
        private JsonNetworkSerializer serializer;
        private Socket socket;

        private Communication() { }

        /// <summary>
        /// Jedina instanca Communication klase.
        /// </summary>
        public static Communication Instance
        {
            get
            {
                if (instance == null)
                    instance = new Communication();
                return instance;
            }
        }

        /// <summary>
        /// Povezuje se sa serverom i inicijalizuje serializer.
        /// </summary>
        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ConfigManager.AppConfig.Ip, ConfigManager.AppConfig.Port);
            serializer = new JsonNetworkSerializer(socket);
        }

        /// <summary>
        /// Šalje zahtev serveru i vraća odgovor deserializovan u tip T.
        /// </summary>
        /// <typeparam name="T">Tip očekivanog rezultata.</typeparam>
        /// <param name="op">Operacija koju šaljemo serveru.</param>
        /// <param name="arg">Argument operacije (opcionalno).</param>
        /// <returns>Rezultat operacije tipa T.</returns>
        /// <exception cref="Exception">Ako server vrati poruku o grešci.</exception>
        public T SendRequest<T>(Operation op, object arg = null)
        {
            var req = new Request { Operation = op, Argument = arg };
            serializer.Send(req);
            var resp = serializer.Receive<Response>();

            if (!string.IsNullOrEmpty(resp.ExceptionMessage))
                throw new Exception(resp.ExceptionMessage);

            return serializer.ReadType<T>(resp.Result);
        }

        /// <summary>
        /// Specijalizovana metoda za pretragu kupca koja vraća ispravan polimorfni tip.
        /// </summary>
        /// <param name="searchKupac">Kriterijum za pretragu kupca.</param>
        /// <returns>Instanca Kupac, FizickoLice ili PravnoLice.</returns>
        public Kupac PretraziKupca(object searchKupac)
        {
            var req = new Request { Operation = Operation.PretraziKupac, Argument = searchKupac };
            serializer.Send(req);
            var resp = serializer.Receive<Response>();

            if (!string.IsNullOrEmpty(resp.ExceptionMessage))
                throw new Exception(resp.ExceptionMessage);

            return serializer.ReadKupacPolymorphic(resp.Result);
        }

        /// <summary>
        /// Zatvara konekciju sa serverom i oslobađa resurse.
        /// </summary>
        public void Close()
        {
            try { socket?.Shutdown(SocketShutdown.Both); } catch { }
            try { socket?.Close(); } catch { }
            socket = null;
            serializer = null;
        }

    }
}
