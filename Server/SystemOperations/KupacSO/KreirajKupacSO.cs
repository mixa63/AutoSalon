using Common.Domain;
using System;

namespace ServerApp.SystemOperations.KupacSO
{
    /// <summary>
    /// Sistemska operacija koja kreira novog <see cref="Kupac"/> u bazi podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku za unos kupca.
    /// </summary>
    internal class KreirajKupacSO : SystemOperationBase
    {
        /// <summary>
        /// Kupac koji treba da bude ubačen u bazu podataka.
        /// </summary>
        private readonly Kupac kupac;

        /// <summary>
        /// Rezultat operacije – ubačeni kupac sa generisanim identifikatorom <c>IdKupac</c>.
        /// </summary>
        internal Kupac Result { get; private set; }

        /// <summary>
        /// Konstruktor koji inicijalizuje sistemsku operaciju sa prosleđenim kupcem.
        /// </summary>
        /// <param name="kupac">Instanca <see cref="Kupac"/> koja će biti ubačena u bazu.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="kupac"/> null.</exception>
        public KreirajKupacSO(Kupac kupac)
        {
            this.kupac = kupac ?? throw new ArgumentNullException(nameof(kupac), "Kupac ne može biti null");
        }

        /// <summary>
        /// Izvršava konkretnu logiku operacije kreiranja kupca.
        /// </summary>
        /// <remarks>
        /// - Kreira novu instancu <see cref="Kupac"/> sa default vrednostima (npr. prazan <c>Email</c>).<br/>
        /// - Koristi broker metod <c>AddWithReturnId</c> da sačuva kupca u bazi i dobije generisani <c>IdKupac</c>.<br/>
        /// - Rezultat se čuva u <see cref="Result"/>.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Kupac noviKupac = new Kupac(0, "");
            noviKupac.IdKupac = broker.AddWithReturnId(noviKupac);
            Result = noviKupac;
        }
    }
}
