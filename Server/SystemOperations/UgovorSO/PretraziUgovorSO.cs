using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.SystemOperations.UgovorSO
{
    /// <summary>
    /// Sistemska operacija za pretragu pojedinačnog <see cref="Ugovor"/> zapisa u bazi prema zadatom kriterijumu.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku pretrage jednog ugovora,
    /// zajedno sa njegovim stavkama i pridruženim automobilima.
    /// </summary>
    internal class PretraziUgovorSO : SystemOperationBase
    {
        /// <summary>
        /// Kriterijum za filtriranje ugovora pri pretrazi.
        /// </summary>
        private readonly Ugovor criteria;

        /// <summary>
        /// Rezultat operacije.
        /// Nakon uspešnog izvršenja sadrži prvi pronađeni <see cref="Ugovor"/> koji zadovoljava kriterijum,
        /// zajedno sa učitanim <see cref="StavkaUgovora"/> i pridruženim <see cref="Automobil"/> objektima.
        /// Ako ne postoji nijedan ugovor koji zadovoljava kriterijum, rezultat je <c>null</c>.
        /// </summary>
        internal Ugovor Result { get; private set; }

        /// <summary>
        /// Inicijalizuje novu instancu operacije za pretragu ugovora.
        /// </summary>
        /// <param name="criteria">Instanca <see cref="Ugovor"/> sa vrednostima za filtriranje pretrage.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="criteria"/> null.</exception>
        public PretraziUgovorSO(Ugovor criteria)
        {
            this.criteria = criteria ?? throw new ArgumentNullException(nameof(criteria), "Kriterijum ne može biti null.");
        }

        /// <summary>
        /// Izvršava konkretnu logiku pretrage ugovora.
        /// </summary>
        /// <remarks>
        /// - Dohvata sve ugovore koji zadovoljavaju zadati kriterijum i uzima prvi pomoću <c>FirstOrDefault()</c>.<br/>
        /// - Ako nijedan ugovor ne odgovara, <see cref="Result"/> se postavlja na <c>null</c>.<br/>
        /// - Ako je ugovor pronađen, učitavaju se njegove <see cref="StavkaUgovora"/>.<br/>
        /// - Za svaku stavku dodatno se učitava kompletan <see cref="Automobil"/> iz baze na osnovu Id-ja.<br/>
        /// - Na kraju se kompletan ugovor sa svim stavkama i automobilima smešta u <see cref="Result"/>.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            var ugovori = broker.GetByCondition(criteria).ConvertAll(x => (Ugovor)x);

            var ugovor = ugovori.FirstOrDefault();
            if (ugovor == null)
            {
                Result = null;
                return;
            }

            var stavkeCriteria = new StavkaUgovora { Ugovor = new Ugovor { IdUgovor = ugovor.IdUgovor } };
            var stavke = broker.GetByCondition(stavkeCriteria).ConvertAll(x => (StavkaUgovora)x);

            foreach (var stavka in stavke)
            {
                var autoCriteria = new Automobil { IdAutomobil = stavka.Automobil.IdAutomobil };
                stavka.Automobil = (Automobil)broker.GetByCondition(autoCriteria).FirstOrDefault();
            }

            ugovor.Stavke = stavke;
            Result = ugovor;
        }
    }
}
