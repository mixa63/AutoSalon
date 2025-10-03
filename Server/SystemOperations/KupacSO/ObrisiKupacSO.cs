using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.KupacSO
{
    /// <summary>
    /// Sistemska operacija koja briše postojeći <see cref="Kupac"/> iz baze podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku brisanja kupca.
    /// </summary>
    internal class ObrisiKupacSO : SystemOperationBase
    {
        /// <summary>
        /// Kupac koji treba da bude obrisan iz baze.
        /// </summary>
        private readonly Kupac kupac;

        /// <summary>
        /// Rezultat operacije.
        /// <c>true</c> ako je kupac uspešno obrisan, <c>false</c> inače.
        /// </summary>
        internal bool Result { get; private set; }

        /// <summary>
        /// Konstruktor koji inicijalizuje operaciju brisanja sa prosleđenim kupcem.
        /// </summary>
        /// <param name="kupac">Instanca <see cref="Kupac"/> koja će biti obrisana.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="kupac"/> null.</exception>
        public ObrisiKupacSO(Kupac kupac)
        {
            this.kupac = kupac ?? throw new ArgumentNullException(nameof(kupac), "Kupac ne može biti null.");
        }

        /// <summary>
        /// Izvršava konkretnu logiku brisanja kupca.
        /// </summary>
        /// <remarks>
        /// - Proverava da li kupac ima postojeće <see cref="Ugovor"/> zapise u bazi; ako ima, baca <see cref="InvalidOperationException"/>.<br/>
        /// - Briše specifične tipove kupca: <see cref="FizickoLice"/> ili <see cref="PravnoLice"/>.<br/>
        /// - Na kraju briše osnovni <see cref="Kupac"/> zapis iz baze.<br/>
        /// - Ako je brisanje uspešno, <see cref="Result"/> se postavlja na <c>true</c>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Ako kupac ima postojeće ugovore i ne može biti obrisan.</exception>
        protected override void ExecuteConcreteOperation()
        {
            var ugovori = broker.GetByCondition(new Ugovor { Kupac = kupac });

            if (ugovori.Count > 0)
            {
                throw new InvalidOperationException("Kupac ima postojeće ugovore i ne može biti obrisan.");
            }

            if (kupac is FizickoLice fl)
                broker.Delete(fl);
            else if (kupac is PravnoLice pl)
                broker.Delete(pl);

            broker.Delete(new Kupac { IdKupac = kupac.IdKupac });

            Result = true;
        }
    }
}
