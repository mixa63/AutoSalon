using Common.Domain;
using System;

namespace ServerApp.SystemOperations.KvalifikacijaSO
{
    /// <summary>
    /// Sistemska operacija koja ubacuje novu <see cref="Kvalifikacija"/> u bazu podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku za unos kvalifikacije.
    /// </summary>
    internal class UbaciKvalifikacijaSO : SystemOperationBase
    {
        /// <summary>
        /// Kvalifikacija koja treba da bude ubačena u bazu podataka.
        /// </summary>
        private readonly Kvalifikacija kvalifikacija;

        /// <summary>
        /// Rezultat operacije – ubačena kvalifikacija sa dodeljenim identifikatorom.
        /// </summary>
        internal Kvalifikacija Result { get; private set; }

        /// <summary>
        /// Konstruktor koji inicijalizuje sistemsku operaciju sa prosleđenom kvalifikacijom.
        /// </summary>
        /// <param name="kvalifikacija">Instanca <see cref="Kvalifikacija"/> koja će biti ubačena u bazu.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="kvalifikacija"/> null.</exception>
        public UbaciKvalifikacijaSO(Kvalifikacija kvalifikacija)
        {
            this.kvalifikacija = kvalifikacija ?? throw new ArgumentNullException(nameof(kvalifikacija), "Kvalifikacija ne može biti null");
        }

        /// <summary>
        /// Izvršava konkretnu logiku operacije ubacivanja kvalifikacije.
        /// </summary>
        /// <remarks>
        /// Metoda koristi broker metod <c>AddWithReturnId</c> koji vraća novogenerisani identifikator,
        /// a zatim kreira novu instancu <see cref="Kvalifikacija"/> sa tim Id-jem i dodeljuje je kao rezultat.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            int id = broker.AddWithReturnId(kvalifikacija);
            Result = new Kvalifikacija { IdKvalifikacija = id };
        }
    }
}
