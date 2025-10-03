using Common.Domain;
using System;
using System.Collections.Generic;

namespace ServerApp.SystemOperations.KvalifikacijaSO
{
    /// <summary>
    /// Sistemska operacija koja vraća listu svih <see cref="Kvalifikacija"/> entiteta iz baze podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku za dohvat svih kvalifikacija.
    /// </summary>
    internal class VratiListuSviKvalifikacijaSO : SystemOperationBase
    {
        /// <summary>
        /// Rezultat operacije – lista svih kvalifikacija iz baze podataka.
        /// Ako u bazi ne postoji nijedna kvalifikacija, vraća se prazna lista.
        /// </summary>
        internal List<Kvalifikacija> Result { get; private set; }

        /// <summary>
        /// Izvršava konkretnu logiku operacije dohvaćanja svih kvalifikacija.
        /// </summary>
        /// <remarks>
        /// Metoda koristi broker metod <c>GetAll</c> koji vraća generičku listu entiteta,
        /// a zatim konvertuje svaki element u instancu tipa <see cref="Kvalifikacija"/>.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            Result = broker.GetAll(new Kvalifikacija()).ConvertAll(x => (Kvalifikacija)x);
        }
    }
}
