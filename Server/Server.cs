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
    internal class Server
    {
        private volatile bool isRunning = false;

        private Socket socket;
        private List<ClientHandler> handlers = new List<ClientHandler>();

        internal event Action<int>? ClientsCountChanged;
        private object _lock = new object();

        private void OnClientsCountChanged()
        {
            var handler = ClientsCountChanged;
            handler?.Invoke(handlers.Count);
        }

        internal Server()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        internal void Start()
        {
            if (isRunning) return;
            isRunning = true;

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);

            socket.Bind(endPoint);
            socket.Listen(5);

            Thread thread = new Thread(AcceptClient);
            thread.Start();
        }

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

        internal void AddClient(ClientHandler clientHandler)
        {
            lock (_lock)
            {
                handlers.Add(clientHandler);
                OnClientsCountChanged();
            }
        }

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
