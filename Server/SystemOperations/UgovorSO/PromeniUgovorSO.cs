using Common.Domain;
using System;
using System.Linq;

namespace ServerApp.SystemOperations.UgovorSO
{
    /// <summary>
    /// Sistemska operacija za izmenu postojećeg <see cref="Ugovor"/> zapisa u bazi podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku ažuriranja ugovora
    /// zajedno sa njegovim stavkama.
    /// </summary>
    internal class PromeniUgovorSO : SystemOperationBase
    {
        /// <summary>
        /// Ugovor koji treba da bude ažuriran u bazi.
        /// </summary>
        private readonly Ugovor ugovor;

        /// <summary>
        /// Rezultat operacije.
        /// Nakon uspešnog izvršenja sadrži <c>true</c>, što označava da je ažuriranje završeno bez greške.
        /// </summary>
        internal bool Result { get; private set; }

        /// <summary>
        /// Inicijalizuje novu instancu operacije za izmenu ugovora.
        /// </summary>
        /// <param name="ugovor">Instanca <see cref="Ugovor"/> koja će biti ažurirana u bazi podataka.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="ugovor"/> null.</exception>
        public PromeniUgovorSO(Ugovor ugovor)
        {
            this.ugovor = ugovor ?? throw new ArgumentNullException(nameof(ugovor), "Ugovor ne može biti null.");
        }

        /// <summary>
        /// Izvršava konkretnu logiku izmene ugovora i njegovih stavki.
        /// </summary>
        /// <remarks>
        /// - Prvo ažurira osnovne podatke ugovora.<br/>
        /// - Briše sve postojeće stavke povezane sa ugovorom.<br/>
        /// - Dodaje nove stavke iz kolekcije <see cref="Ugovor.Stavke"/> i postavlja redni broj (<c>Rb</c>) za svaku.<br/>
        /// - Nakon izvršenja, <see cref="Result"/> je <c>true</c>.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            // Azuriranje osnovnog ugovora
            broker.Update(ugovor);

            // Brisanje svih postojećih stavki za ugovor
            var sveStavkeZaUgovor = new StavkaUgovora { Ugovor = ugovor };
            var stavke = broker.GetByCondition(sveStavkeZaUgovor);
            foreach (StavkaUgovora s in stavke.Cast<StavkaUgovora>())
            {
                broker.Delete(s);
            }

            // Dodavanje novih stavki sa rednim brojem
            int rb = 1;
            foreach (var stavka in ugovor.Stavke)
            {
                stavka.Rb = rb++;
                stavka.Ugovor = ugovor;
                broker.Add(stavka);
            }

            Result = true;
        }
    }
}
