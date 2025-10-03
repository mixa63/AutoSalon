using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.KupacSO
{
    /// <summary>
    /// Sistemska operacija koja vraća listu <see cref="Kupac"/> entiteta iz baze podataka prema zadatom kriterijumu.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku pretrage kupaca.
    /// </summary>
    internal class VratiListuKupacSO : SystemOperationBase
    {
        /// <summary>
        /// Kriterijum za filtriranje kupaca pri pretrazi.
        /// </summary>
        private readonly Kupac criteria;

        /// <summary>
        /// Rezultat operacije – lista kupaca koja zadovoljava kriterijum.
        /// Ako ne postoji nijedan kupac koji zadovoljava kriterijum, vraća se prazna lista.
        /// </summary>
        internal List<Kupac> Result { get; private set; }

        /// <summary>
        /// Konstruktor koji inicijalizuje operaciju sa kriterijumom za pretragu kupaca.
        /// </summary>
        /// <param name="criteria">Instanca <see cref="Kupac"/> koja sadrži vrednosti za filtriranje pretrage.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="criteria"/> null.</exception>
        public VratiListuKupacSO(Kupac criteria)
        {
            this.criteria = criteria ?? throw new ArgumentNullException(nameof(criteria), "Kriterijum ne može biti null");
        }

        /// <summary>
        /// Izvršava konkretnu logiku pretrage kupaca prema kriterijumu.
        /// </summary>
        /// <remarks>
        /// - Koristi broker metod <c>GetByCondition</c> koji vraća generičku listu entiteta.<br/>
        /// - Svaki element liste se konvertuje u tip <see cref="Kupac"/> i čuva u <see cref="Result"/>.<br/>
        /// - Ako nema kupaca koji zadovoljavaju kriterijum, <see cref="Result"/> će biti prazna lista.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Result = broker.GetByCondition(criteria).ConvertAll(x => (Kupac)x);
        }
    }
}
