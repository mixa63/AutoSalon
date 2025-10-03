using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.KupacSO
{
    /// <summary>
    /// Sistemska operacija koja vraća listu svih <see cref="Kupac"/> entiteta iz baze podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku dohvaćanja svih kupaca.
    /// </summary>
    internal class VratiListuSviKupacSO : SystemOperationBase
    {
        /// <summary>
        /// Rezultat operacije – lista svih kupaca iz baze podataka.
        /// Ako u bazi ne postoji nijedan kupac, vraća se prazna lista.
        /// </summary>
        internal List<Kupac> Result { get; private set; }

        /// <summary>
        /// Izvršava konkretnu logiku dohvaćanja svih kupaca.
        /// </summary>
        /// <remarks>
        /// - Koristi broker metod <c>GetAll</c> koji vraća sve entitete tipa <see cref="Kupac"/>.<br/>
        /// - Rezultat se konvertuje u listu tipa <see cref="Kupac"/> i čuva u <see cref="Result"/>.<br/>
        /// - Ako nema nijednog kupca u bazi, <see cref="Result"/> će biti prazna lista.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Result = broker.GetAll(new Kupac()).ConvertAll(x => (Kupac)x);
        }
    }
}
