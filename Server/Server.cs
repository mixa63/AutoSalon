using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    /// <summary>
    /// Predstavlja TCP server koji prihvata više klijenata
    /// i upravlja njihovim konekcijama preko klase <see cref="ClientHandler"/>.
    /// </summary>
    internal class Server
    {
        /// <summary>
        /// Indikator da li je server trenutno pokrenut.
        /// </summary>
        private volatile bool isRunning = false;

        /// <summary>
        /// Socket koji služi za slušanje dolaznih konekcija.
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Lista aktivnih klijenata povezanih sa ovim serverom.
        /// </summary>
        private List<ClientHandler> handlers = new List<ClientHandler>();

        /// <summary>
        /// Događaj koji se pokreće kada se promeni broj povezanih klijenata.
        /// Prosleđuje trenutni broj klijenata kao argument.
        /// </summary>
        internal event Action<int>? ClientsCountChanged;

        /// <summary>
        /// Objekt za sinhronizaciju pri pristupu listi klijenata.
        /// </summary>
        private object _lock = new object();

        /// <summary>
        /// Pokreće događaj <see cref="ClientsCountChanged"/> sa trenutnim brojem klijenata.
        /// </summary>
        private void OnClientsCountChanged()
        {
            var handler = ClientsCountChanged;
            handler?.Invoke(handlers.Count);
        }

        /// <summary>
        /// Kreira novu instancu servera i inicijalizuje socket.
        /// </summary>
        internal Server()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Pokreće server koristeći IP i port definisan u <see cref="ConfigManager.AppConfig"/>.
        /// Počinje da sluša dolazne konekcije na posebnoj pozadinskoj niti.
        /// </summary>
        internal void Start()
        {
            if (isRunning) return;
            isRunning = true;

            IPEndPoint endPoint = new IPEndPoint(
                IPAddress.Parse(ConfigManager.AppConfig.Ip),
                ConfigManager.AppConfig.Port
            );

            socket.Bind(endPoint);
            socket.Listen(5);

            Thread thread = new Thread(AcceptClient);
            thread.Start();
        }

        /// <summary>
        /// Petlja koja prihvata dolazne konekcije i kreira <see cref="ClientHandler"/> za svakog klijenta.
        /// </summary>
        internal void AcceptClient()
        {
            try
            {
                while (isRunning)
                {
                    Socket clientSocket = socket.Accept();
                    ClientHandler handler = new ClientHandler(clientSocket, this);
                    AddClient(handler);
                    Thread clientThread = new Thread(handler.HandleRequest);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Zaustavlja server, zatvara sve konekcije i oslobađa resurse.
        /// </summary>
        internal void Stop()
        {
            if (!isRunning) return;
            isRunning = false;

            List<ClientHandler> copy;

            lock (_lock)
            {
                copy = new List<ClientHandler>(handlers);
            }

            foreach (ClientHandler handler in copy)
            {
                handler.CloseSocket();
            }

            handlers.Clear();

            socket?.Close();
            socket = null;
        }

        /// <summary>
        /// Dodaje novog klijenta u listu aktivnih klijenata i pokreće događaj promene broja klijenata.
        /// </summary>
        /// <param name="clientHandler">Handler novog klijenta.</param>
        internal void AddClient(ClientHandler clientHandler)
        {
            lock (_lock)
            {
                handlers.Add(clientHandler);
                OnClientsCountChanged();
            }
        }

        /// <summary>
        /// Uklanja klijenta iz liste aktivnih klijenata i pokreće događaj promene broja klijenata.
        /// </summary>
        /// <param name="clientHandler">Handler klijenta koji se uklanja.</param>
        internal void RemoveClient(ClientHandler clientHandler)
        {
            lock (_lock)
            {
                handlers.Remove(clientHandler);
                OnClientsCountChanged();
            }
        }
    }
}
