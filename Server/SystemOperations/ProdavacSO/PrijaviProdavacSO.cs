using Common.Domain;
using System;
using System.Linq;

namespace ServerApp.SystemOperations.ProdavacSO
{
    /// <summary>
    /// Sistemska operacija za prijavu <see cref="Prodavac"/> korisnika na osnovu korisničkog imena i lozinke.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku autentifikacije prodavca.
    /// </summary>
    internal class PrijaviProdavacSO : SystemOperationBase
    {
        /// <summary>
        /// Korisničko ime prodavca.
        /// </summary>
        private readonly string username;

        /// <summary>
        /// Lozinka prodavca.
        /// </summary>
        private readonly string password;

        /// <summary>
        /// Rezultat operacije.
        /// Nakon uspešne prijave sadrži instancu <see cref="Prodavac"/> iz baze podataka.
        /// Ako korisničko ime i lozinka nisu ispravni, rezultat će biti <c>null</c>.
        /// </summary>
        internal Prodavac Result { get; private set; }

        /// <summary>
        /// Inicijalizuje novu instancu operacije za prijavu prodavca.
        /// </summary>
        /// <param name="username">Korisničko ime prodavca.</param>
        /// <param name="password">Lozinka prodavca.</param>
        public PrijaviProdavacSO(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Izvršava konkretnu logiku prijave prodavca.
        /// </summary>
        /// <remarks>
        /// - Ako <paramref name="username"/> ili <paramref name="password"/> nisu uneti, baca izuzetak.<br/>
        /// - Koristi broker metod <c>GetByCondition</c> za pretragu prodavca po kredencijalima.<br/>
        /// - Ako pronađe odgovarajućeg prodavca, čuva ga u <see cref="Result"/>; u suprotnom <see cref="Result"/> je <c>null</c>.
        /// </remarks>
        /// <exception cref="Exception">Ako nije uneto korisničko ime ili lozinka.</exception>
        protected override void ExecuteConcreteOperation()
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Morate uneti korisničko ime i lozinku.");
            }

            var criteria = new Prodavac { Username = username, Password = password };
            Result = broker.GetByCondition(criteria).OfType<Prodavac>().FirstOrDefault();
        }
    }
}
