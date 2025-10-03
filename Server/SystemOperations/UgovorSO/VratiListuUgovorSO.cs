using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.UgovorSO
{
    /// <summary>
    /// Sistemska operacija za vraćanje liste <see cref="Ugovor"/> zapisa iz baze podataka prema zadatom kriterijumu.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku pretrage više ugovora.
    /// </summary>
    internal class VratiListuUgovorSO : SystemOperationBase
    {
        /// <summary>
        /// Kriterijum za filtriranje ugovora pri pretrazi.
        /// </summary>
        private readonly Ugovor criteria;

        /// <summary>
        /// Rezultat operacije.
        /// Nakon uspešnog izvršenja sadrži listu svih <see cref="Ugovor"/> objekata koji zadovoljavaju kriterijum.
        /// Ako nijedan ugovor ne odgovara kriterijumu, lista će biti prazna.
        /// </summary>
        internal List<Ugovor> Result { get; private set; }

        /// <summary>
        /// Inicijalizuje novu instancu operacije za pretragu liste ugovora.
        /// </summary>
        /// <param name="criteria">Instanca <see cref="Ugovor"/> sa vrednostima za filtriranje pretrage.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="criteria"/> null.</exception>
        public VratiListuUgovorSO(Ugovor criteria)
        {
            this.criteria = criteria ?? throw new ArgumentNullException(nameof(criteria), "Kriterijum ne može biti null.");
        }

        /// <summary>
        /// Izvršava konkretnu logiku pretrage liste ugovora.
        /// </summary>
        /// <remarks>
        /// - Koristi broker metodu <c>GetByCondition</c> da pronađe sve ugovore koji odgovaraju zadatom kriterijumu.<br/>
        /// - Rezultat se konvertuje u listu tipa <see cref="Ugovor"/> i čuva u <see cref="Result"/>.<br/>
        /// - Ako ne postoji nijedan ugovor koji zadovoljava kriterijum, <see cref="Result"/> će biti prazna lista.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Result = broker.GetByCondition(criteria).ConvertAll(x => (Ugovor)x);
        }
    }
}
