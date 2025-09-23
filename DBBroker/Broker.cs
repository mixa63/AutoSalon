using Microsoft.Data.SqlClient;
using Common.Domain;
using System.Data.Common;

namespace DBBroker
{
    /// <summary>
    /// Glavna broker klasa zadužena za komunikaciju sa bazom podataka.
    /// Pruža CRUD operacije i druge usluge za rad sa IEntity objektima.
    /// </summary>
    public class Broker
    {
        private DBConnection connection;

        /// <summary>
        /// Konstruktor koji inicijalizuje broker sa zadatim connection string-om.
        /// </summary>
        /// <param name="connectionString">Connection string za bazu podataka</param>
        public Broker(string connectionString)
        {
            connection = new DBConnection(connectionString);
        }

        /// <summary>
        /// Otvara konekciju ka bazi podataka.
        /// </summary>
        public void OpenConnection()
        {
            connection.OpenConnection();
        }

        /// <summary>
        /// Zatvara konekciju ka bazi podataka.
        /// </summary>
        public void CloseConnection()
        {
            connection.CloseConnection();
        }

        /// <summary>
        /// Počinje novu transakciju.
        /// </summary>
        public void BeginTransaction()
        {
            connection.BeginTransaction();
        }

        /// <summary>
        /// Potvrđuje transakciju.
        /// </summary>
        public void Commit()
        {
            connection.Commit();
        }

        /// <summary>
        /// Poništava transakciju.
        /// </summary>
        public void Rollback()
        {
            connection.Rollback();
        }

        /// <summary>
        /// Dodaje novi entitet u bazu podataka.
        /// </summary>
        /// <param name="entity">Entitet za čuvanje</param>
        public void Add(IEntity entity)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"INSERT INTO {entity.TableName} ({entity.InsertColumns}) VALUES ({entity.InsertValuesPlaceholders})";

            foreach (SqlParameter param in entity.GetInsertParameters())
            {
                cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        /// <summary>
        /// Dodaje novi entitet i vraća generisani ID.
        /// </summary>
        /// <param name="entity">Entitet za čuvanje</param>
        /// <returns>Generisani ID novog zapisa</returns>
        public int AddWithReturnId(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            // Izdvojiti čisto ime primarnog ključa (bez aliasa)
            string pkColumn = entity.PrimaryKeyColumn;
            if (pkColumn.Contains('.'))
                pkColumn = pkColumn.Split('.')[1];

            cmd.CommandText = $"INSERT INTO {entity.TableName} ({entity.InsertColumns}) OUTPUT INSERTED.{pkColumn} VALUES ({entity.InsertValuesPlaceholders})";

            foreach (SqlParameter param in entity.GetInsertParameters())
            {
                cmd.Parameters.Add(param);
            }

            int id = (int)cmd.ExecuteScalar();
            return id;
        }

        /// <summary>
        /// Ažurira postojeći entitet u bazi podataka.
        /// </summary>
        /// <param name="entity">Entitet za ažuriranje</param>
        public void Update(IEntity entity)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"UPDATE {entity.TableName} SET {entity.UpdateSetClause} WHERE {entity.WhereCondition}";

            foreach (SqlParameter param in entity.GetUpdateParameters())
            {
                if (!cmd.Parameters.Contains(param.ParameterName))
                    cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        /// <summary>
        /// Briše entitet iz baze podataka.
        /// </summary>
        /// <param name="entity">Entitet za brisanje</param>
        public void Delete(IEntity entity)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM {entity.TableName} WHERE {entity.WhereCondition}";

            foreach (SqlParameter param in entity.GetPrimaryKeyParameters())
            {
                cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        /// <summary>
        /// Vraća sve zapise za dati entitet.
        /// </summary>
        /// <param name="entity">Entitet za pretragu</param>
        /// <returns>Lista entiteta</returns>
        public List<IEntity> GetAll(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            string joinClause = "";
            if (!string.IsNullOrEmpty(entity.JoinTable) && !string.IsNullOrEmpty(entity.JoinCondition))
            {
                joinClause = $" JOIN {entity.JoinTable} ON {entity.JoinCondition}";
            }

            cmd.CommandText = $"SELECT {entity.SelectColumns} FROM {entity.TableName} {entity.TableAlias}{joinClause}";

            using DbDataReader reader = cmd.ExecuteReader();
            return entity.ReadEntities(reader);
        }

        /// <summary>
        /// Vraća entitete na osnovu zadatih uslova.
        /// </summary>
        /// <param name="entity">Entitet sa popunjenim kriterijumima za pretragu</param>
        /// <returns>Lista pronađenih entiteta</returns>
        public List<IEntity> GetByCondition(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            var (whereClause, parameters) = entity.GetWhereClauseWithParameters();

            string joinClause = "";
            if (!string.IsNullOrEmpty(entity.JoinTable) && !string.IsNullOrEmpty(entity.JoinCondition))
            {
                joinClause = $" JOIN {entity.JoinTable} ON {entity.JoinCondition}";
            }

            cmd.CommandText = $"SELECT {entity.SelectColumns} FROM {entity.TableName} {entity.TableAlias}{joinClause} WHERE {whereClause}";

            foreach (SqlParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            using DbDataReader reader = cmd.ExecuteReader();
            return entity.ReadEntities(reader);
        }

        /// <summary>
        /// Vraća prvi ID za dati entitet (za sortirane upite).
        /// </summary>
        /// <param name="entity">Entitet za pretragu</param>
        /// <returns>Prvi ID ili -1 ako nije pronađen</returns>
        public int GetFirstId(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            // Izdvojiti čisto ime primarnog ključa (bez aliasa)
            string pkColumn = entity.PrimaryKeyColumn;
            if (pkColumn.Contains('.'))
                pkColumn = pkColumn.Split('.')[1];

            cmd.CommandText = $"SELECT TOP 1 {pkColumn} FROM {entity.TableName} ORDER BY {pkColumn}";

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }

            return -1;
        }
    }
}