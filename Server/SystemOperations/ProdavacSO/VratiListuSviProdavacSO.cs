using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.ProdavacSO
{
    /// <summary>
    /// Sistemska operacija koja vraća listu svih <see cref="Prodavac"/> entiteta iz baze podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku za dohvat svih prodavaca.
    /// </summary>
    internal class VratiListuSviProdavacSO : SystemOperationBase
    {
        /// <summary>
        /// Rezultat operacije – lista svih prodavaca iz baze podataka.
        /// Ako u bazi ne postoji nijedan prodavac, vraća se prazna lista.
        /// </summary>
        internal List<Prodavac> Result { get; private set; }

        /// <summary>
        /// Izvršava konkretnu logiku operacije dohvaćanja svih prodavaca.
        /// </summary>
        /// <remarks>
        /// Metoda koristi broker metod <c>GetAll</c> koji vraća generičku listu entiteta,
        /// a zatim se ta lista konvertuje u listu tipa <see cref="Prodavac"/>.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Result = broker.GetAll(new Prodavac()).ConvertAll(x => (Prodavac)x);
        }
    }
}
