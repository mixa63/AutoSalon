using Common.Domain;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace Common.Communication
{
    /// <summary>
    /// Omogućava serijalizaciju i deserializaciju objekata u JSON formatu preko mrežnog soketa.
    /// Pruža metode za slanje i primanje podataka preko <see cref="Socket"/>.
    /// </summary>
    public class JsonNetworkSerializer
    {
        private readonly Socket s;
        private readonly NetworkStream stream;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        /// <summary>
        /// Kreira instancu <see cref="JsonNetworkSerializer"/> za dati soket.
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
        /// <param name="z">Objekat koji se šalje. Može biti bilo koji referentni tip.</param>
        public void Send(object z)
        {
            writer.WriteLine(JsonSerializer.Serialize(z));
        }

        /// <summary>
        /// Prima objekat u JSON formatu i deserializuje ga u dati tip.
        /// </summary>
        /// <typeparam name="T">Tip u koji će JSON biti deserializovan.</typeparam>
        /// <returns>Deserializovani objekat tipa <typeparamref name="T"/>.</returns>
        /// <exception cref="IOException">Baca se ako je veza sa soketom prekinuta.</exception>
        public T Receive<T>()
        {
            string json = reader.ReadLine();
            if (json == null)
            {
                throw new IOException("Veza sa klijentom je prekinuta.");
            }
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Deserializuje već primljeni JSON objekat u dati tip.
        /// </summary>
        /// <typeparam name="T">Tip u koji će podaci biti deserializovani. Mora biti referentni tip.</typeparam>
        /// <param name="podaci">JSON objekat koji treba deserializovati.</param>
        /// <returns>Deserializovani objekat tipa <typeparamref name="T"/> ili <c>default(T)</c> ako je ulaz <c>null</c>.</returns>
        public T ReadType<T>(object podaci)
        {
            if (podaci == null)
                return default(T);

            JsonElement element = (JsonElement)podaci;
            return element.Deserialize<T>();
        }

        /// <summary>
        /// Deserializuje <see cref="Kupac"/> objekat u polimorfnom obliku na osnovu tipa.
        /// </summary>
        /// <param name="podaci">JSON objekat koji sadrži informacije o kupcu.</param>
        /// <returns>
        /// Deserializovani <see cref="Kupac"/> ili njegov derivat (<see cref="FizickoLice"/> ili <see cref="PravnoLice"/>)
        /// na osnovu vrednosti <c>TableName</c> u JSON-u.
        /// Vraća <c>null</c> ako je ulaz <c>null</c>.
        /// </returns>
        public Kupac ReadKupacPolymorphic(object podaci)
        {
            if (podaci == null) return null;

            JsonElement element = (JsonElement)podaci;
            string tableName = element.GetProperty("TableName").GetString();

            if (tableName == "FizickoLice")
                return element.Deserialize<FizickoLice>();
            else if (tableName == "PravnoLice")
                return element.Deserialize<PravnoLice>();
            else
                return element.Deserialize<Kupac>();
        }
    }
}
