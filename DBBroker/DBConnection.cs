using Microsoft.Data.SqlClient;

namespace DBBroker
{
    /// <summary>
    /// Klasa zadužena za upravljanje konekcijom ka bazi podataka i transakcijama.
    /// Obezbeđuje osnovne funkcije za otvaranje, zatvaranje i rad sa transakcijama.
    /// </summary>
    public class DBConnection
    {
        private SqlConnection connection;
        private SqlTransaction transaction;

        /// <summary>
        /// Konstruktor koji inicijalizuje konekciju sa zadatim connection string-om.
        /// </summary>
        /// <param name="connectionString">Connection string za bazu podataka</param>
        public DBConnection(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Otvara konekciju ka bazi podataka.
        /// </summary>
        public void OpenConnection()
        {
            connection?.Open();
        }

        /// <summary>
        /// Zatvara konekciju ka bazi podataka.
        /// </summary>
        public void CloseConnection()
        {
            connection?.Close();
        }

        /// <summary>
        /// Počinje novu transakciju.
        /// </summary>
        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        /// <summary>
        /// Potvrđuje transakciju.
        /// </summary>
        public void Commit()
        {
            transaction?.Commit();
        }

        /// <summary>
        /// Poništava transakciju.
        /// </summary>
        public void Rollback()
        {
            transaction?.Rollback();
        }

        /// <summary>
        /// Kreira SqlCommand objekat vezan za trenutnu konekciju i transakciju.
        /// </summary>
        /// <returns>Novi SqlCommand objekat</returns>
        public SqlCommand CreateCommand()
        {
            return new SqlCommand("", connection, transaction);
        }
    }
}