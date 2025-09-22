using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Communication
{
    /// <summary>
    /// Omogućava serijalizaciju i deserializaciju objekata u JSON formatu preko mrežnog soketa.
    /// </summary>
    public class JsonNetworkSerializer
    {
        private readonly Socket s;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;

        /// <summary>
        /// Kreira instancu JsonNetworkSerializer-a za dati soket.
        /// </summary>
        /// <param name="s">Soket preko kojeg se komunicira.</param>
        public JsonNetworkSerializer(Socket s)
        {
            this.s = s;
            stream = new NetworkStream(s);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };
        }

        /// <summary>
        /// Šalje objekat preko toka u JSON formatu.
        /// </summary>
        /// <param name="z">Objekat koji se šalje.</param>
        public void Send(object z)
        {
            writer.WriteLine(JsonSerializer.Serialize(z));
        }

        /// <summary>
        /// Prima objekat u JSON formatu i deserializuje ga u dati tip.
        /// </summary>
        /// <typeparam name="T">Tip u koji će JSON biti deserializovan.</typeparam>
        /// <returns>Deserializovani objekat tipa T.</returns>
        public T Receive<T>()
        {
            string json = reader.ReadLine();
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Deserializuje objekat koji je već primljen u JSON formatu u dati tip.
        /// </summary>
        /// <typeparam name="T">Tip u koji će podaci biti deserializovani. Mora biti referentni tip.</typeparam>
        /// <param name="podaci">Objekat koji treba deserializovati.</param>
        /// <returns>Deserializovani objekat tipa T, ili null ako je ulazni objekat null.</returns>
        public T ReadType<T>(object podaci) where T : class
        {
            return podaci == null ? null : JsonSerializer.Deserialize<T>((JsonElement)podaci);
        }

        /// <summary>
        /// Zatvara sve tokove i mrežni stream.
        /// </summary>
        public void Close()
        {
            stream.Close();
            reader.Close();
            writer.Close();
        }
    }
}

