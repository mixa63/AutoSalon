using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.AutomobilSO
{
    /// <summary>
    /// Sistemska operacija koja vraća listu svih <see cref="Automobil"/> entiteta iz baze podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku dohvaćanja svih automobila.
    /// </summary>
    internal class VratiListuSviAutomobilSO : SystemOperationBase
    {
        /// <summary>
        /// Rezultat operacije – lista svih automobila iz baze podataka.
        /// Ako u bazi ne postoji nijedan automobil, vraća se prazna lista.
        /// </summary>
        internal List<Automobil> Result { get; private set; }

        /// <summary>
        /// Izvršava konkretnu logiku dohvaćanja svih automobila.
        /// </summary>
        /// <remarks>
        /// - Koristi broker metod <c>GetAll</c> koji vraća sve entitete tipa <see cref="Automobil"/>.<br/>
        /// - Rezultat se konvertuje u listu tipa <see cref="Automobil"/> i čuva u <see cref="Result"/>.<br/>
        /// - Ako nema nijednog automobila u bazi, <see cref="Result"/> će biti prazna lista.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Result = broker.GetAll(new Automobil()).ConvertAll(x => (Automobil)x);
        }
    }
}
