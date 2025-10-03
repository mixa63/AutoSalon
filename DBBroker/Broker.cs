using Microsoft.Data.SqlClient;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DBBroker
{
    /// <summary>
    /// Glavna broker klasa zadužena za komunikaciju sa bazom podataka.
    /// Pruža CRUD operacije i druge metode za rad sa <see cref="IEntity"/> objektima.
    /// </summary>
    public class Broker : IBroker
    {
        private readonly DBConnection connection;

        /// <summary>
        /// Konstruktor koji inicijalizuje broker sa zadatim connection string-om.
        /// </summary>
        /// <param name="connectionString">Connection string za bazu podataka.</param>
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
        /// Počinje novu transakciju nad bazom.
        /// </summary>
        public void BeginTransaction()
        {
            connection.BeginTransaction();
        }

        /// <summary>
        /// Potvrđuje aktivnu transakciju.
        /// </summary>
        public void Commit()
        {
            connection.Commit();
        }

        /// <summary>
        /// Poništava aktivnu transakciju.
        /// </summary>
        public void Rollback()
        {
            connection.Rollback();
        }

        /// <summary>
        /// Dodaje novi entitet u bazu podataka.
        /// </summary>
        /// <param name="entity">Entitet koji treba dodati.</param>
        public void Add(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"INSERT INTO {entity.TableName} ({entity.InsertColumns}) VALUES ({entity.InsertValuesPlaceholders})";

            foreach (SqlParameter param in entity.GetInsertParameters())
            {
                cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Dodaje novi entitet u bazu i vraća generisani ID primarnog ključa.
        /// </summary>
        /// <param name="entity">Entitet koji treba dodati.</param>
        /// <returns>Generisani ID novog zapisa u bazi.</returns>
        public int AddWithReturnId(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            // Izdvajanje čiste kolone primarnog ključa
            string pkColumn = entity.PrimaryKeyColumn.Contains(".")
                ? entity.PrimaryKeyColumn.Split('.')[1]
                : entity.PrimaryKeyColumn;

            cmd.CommandText = $"INSERT INTO {entity.TableName} ({entity.InsertColumns}) " +
                              $"OUTPUT INSERTED.{pkColumn} VALUES ({entity.InsertValuesPlaceholders})";

            foreach (SqlParameter param in entity.GetInsertParameters())
            {
                cmd.Parameters.Add(param);
            }

            return (int)cmd.ExecuteScalar();
        }

        /// <summary>
        /// Ažurira postojeći entitet u bazi podataka.
        /// </summary>
        /// <param name="entity">Entitet koji treba ažurirati.</param>
        public void Update(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"UPDATE {entity.TableName} SET {entity.UpdateSetClause} WHERE {entity.WhereCondition}";

            foreach (SqlParameter param in entity.GetUpdateParameters())
            {
                if (!cmd.Parameters.Contains(param.ParameterName))
                    cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Briše entitet iz baze podataka na osnovu primarnog ključa.
        /// </summary>
        /// <param name="entity">Entitet koji treba obrisati.</param>
        public void Delete(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM {entity.TableName} WHERE {entity.WhereCondition}";

            foreach (SqlParameter param in entity.GetPrimaryKeyParameters())
            {
                cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Vraća sve zapise iz baze za dati entitet.
        /// </summary>
        /// <param name="entity">Entitet za pretragu.</param>
        /// <returns>Lista svih entiteta tipa <see cref="IEntity"/>.</returns>
        public List<IEntity> GetAll(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            string joinClause = string.IsNullOrEmpty(entity.JoinTable) ? "" : $" {entity.JoinTable}";

            cmd.CommandText = $"SELECT {entity.SelectColumns} FROM {entity.TableName} {entity.TableAlias}{joinClause}";

            using DbDataReader reader = cmd.ExecuteReader();
            return entity.ReadEntities(reader);
        }

        /// <summary>
        /// Vraća entitete iz baze koji zadovoljavaju zadate kriterijume.
        /// </summary>
        /// <param name="entity">Entitet sa postavljenim kriterijumima.</param>
        /// <returns>Lista pronađenih entiteta.</returns>
        public List<IEntity> GetByCondition(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            var (whereClause, parameters) = entity.GetWhereClauseWithParameters();
            string joinClause = string.IsNullOrEmpty(entity.JoinTable) ? "" : $" {entity.JoinTable}";

            cmd.CommandText = $"SELECT {entity.SelectColumns} FROM {entity.TableName} {entity.TableAlias}{joinClause} WHERE {whereClause}";

            foreach (SqlParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            using DbDataReader reader = cmd.ExecuteReader();
            return entity.ReadEntities(reader);
        }

        /// <summary>
        /// Vraća prvi primarni ključ za dati entitet, ili -1 ako entitet ne postoji.
        /// </summary>
        /// <param name="entity">Entitet za pretragu.</param>
        /// <returns>Prvi ID ili -1 ako nije pronađen.</returns>
        public int GetFirstId(IEntity entity)
        {
            using SqlCommand cmd = connection.CreateCommand();

            string pkColumn = entity.PrimaryKeyColumn.Contains(".")
                ? entity.PrimaryKeyColumn.Split('.')[1]
                : entity.PrimaryKeyColumn;

            cmd.CommandText = $"SELECT TOP 1 {pkColumn} FROM {entity.TableName} ORDER BY {pkColumn}";

            object result = cmd.ExecuteScalar();

            return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : -1;
        }
    }
}
