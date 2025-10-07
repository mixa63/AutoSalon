using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.SystemOperations.KupacSO
{
    /// <summary>
    /// Sistemska operacija koja pretražuje jednog <see cref="Kupac"/> iz baze podataka prema zadatom kriterijumu.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku pretrage kupca.
    /// </summary>
    internal class PretraziKupacSO : SystemOperationBase
    {
        /// <summary>
        /// Kriterijum za filtriranje kupca pri pretrazi.
        /// </summary>
        private readonly Kupac criteria;

        /// <summary>
        /// Rezultat operacije – pronađeni kupac koji zadovoljava kriterijum ili <c>null</c> ako ne postoji.
        /// </summary>
        internal Kupac Result { get; private set; }

        /// <summary>
        /// Konstruktor koji inicijalizuje operaciju sa kriterijumom za pretragu kupca.
        /// </summary>
        /// <param name="criteria">Instanca <see cref="Kupac"/> koja sadrži vrednosti za filtriranje pretrage.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="criteria"/> null.</exception>
        public PretraziKupacSO(Kupac criteria)
        {
            this.criteria = criteria ?? throw new ArgumentNullException(nameof(criteria), "Kriterijum ne može biti null");
        }

        /// <summary>
        /// Izvršava konkretnu logiku pretrage kupca.
        /// </summary>
        /// <remarks>
        /// - Prvo pokušava da pronađe <see cref="FizickoLice"/> koji odgovara kriterijumu.<br/>
        /// - Ako ne pronađe fizičko lice, pokušava da pronađe <see cref="PravnoLice"/>.<br/>
        /// - Ako ni pravno lice nije pronađeno, koristi osnovni <see cref="Kupac"/> kriterijum.<br/>
        /// - Rezultat se čuva u <see cref="Result"/> i može biti <c>null</c> ako ne postoji nijedan kupac koji zadovoljava kriterijum.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            var fizickoLiceCriteria = new FizickoLice { IdKupac = criteria.IdKupac };
            var pronadjenaFizickaLica = broker.GetByCondition(fizickoLiceCriteria).ConvertAll(x => (FizickoLice)x);

            if (pronadjenaFizickaLica != null && pronadjenaFizickaLica.Count > 0)
            {
                Result = pronadjenaFizickaLica.FirstOrDefault();
                return;
            }

            var pravnoLiceCriteria = new PravnoLice { IdKupac = criteria.IdKupac };
            var pronadjenaPravnaLica = broker.GetByCondition(pravnoLiceCriteria).ConvertAll(x => (PravnoLice)x);

            if (pronadjenaPravnaLica != null && pronadjenaPravnaLica.Count > 0)
            {
                Result = pronadjenaPravnaLica.FirstOrDefault();
                return;
            }

            var pronadjeniKupci = broker.GetByCondition(criteria).ConvertAll(x => (Kupac)x);
            Result = pronadjeniKupci.FirstOrDefault();
        }
    }
}
