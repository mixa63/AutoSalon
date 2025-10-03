using Common.Domain;

namespace Client
{
    /// <summary>
    /// Singleton klasa koja čuva podatke o trenutnoj sesiji korisnika.
    /// </summary>
    internal class Session
    {
        private static Session instance;

        /// <summary>
        /// Jedina instanca klase Session.
        /// </summary>
        internal static Session Instance => instance ??= new Session();

        /// <summary>
        /// Privatni konstruktor da se onemogući direktno instanciranje.
        /// </summary>
        private Session() { }

        /// <summary>
        /// Trenutno prijavljeni prodavac.
        /// </summary>
        internal Prodavac Prodavac { get; set; }

        /// <summary>
        /// Briše podatke o sesiji, npr. prilikom odjave korisnika.
        /// </summary>
        internal void Clear()
        {
            Prodavac = null;
        }
    }
}
