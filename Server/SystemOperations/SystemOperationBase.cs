using DBBroker;
using ServerApp;

namespace ServerApp.SystemOperations
{
    /// <summary>
    /// Apstraktna bazna klasa koja definise zajednicki template za izvrsavanje sistemskih operacija.
    /// Obezbedjuje upravljanje konekcijom i transakcijama ka bazi podataka.
    /// </summary>
    internal abstract class SystemOperationBase
    {
        /// <summary>
        /// Broker za rad sa bazom podataka.
        /// </summary>
        protected IBroker broker;

        /// <summary>
        /// Konstruktor koji inicijalizuje broker sa connection string-om iz ConfigManager-a.
        /// </summary>
        internal SystemOperationBase()
        {
            broker = new Broker(ConfigManager.AppConfig.ConnectionString);
        }

        /// <summary>
        /// Template metod koji upravlja celokupnim tokom izvrsavanja sistemske operacije.
        /// Obuhvata otvaranje konekcije, pokretanje transakcije, izvrsavanje konkretne operacije,
        /// potvrdu ili ponistavanje transakcije i zatvaranje konekcije.
        /// </summary>
        /// <exception cref="Exception">Baca izuzetak u slucaju greske tokom izvrsavanja operacije.</exception>
        internal void ExecuteTemplate()
        {
            try
            {
                broker.OpenConnection();
                broker.BeginTransaction();

                ExecuteConcreteOperation();

                broker.Commit();
            }
            catch (Exception ex)
            {
                broker.Rollback();
                throw;
            }
            finally
            {
                broker.CloseConnection();
            }
        }

        /// <summary>
        /// Apstraktni metod koji konkretne klase moraju da implementiraju.
        /// Sadrzi specifichnu logiku odredjene sistemske operacije.
        /// </summary>
        protected abstract void ExecuteConcreteOperation();
    }
}