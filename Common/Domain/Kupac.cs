using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja generičkog kupca u sistemu.
    /// Ovo je zajednički entitet za <see cref="PravnoLice"/> i <see cref="FizickoLice"/>.
    /// Sadrži osnovne informacije koje su zajedničke za oba tipa.
    /// </summary>
    public class Kupac : IEntity
    {
        /// <summary>
        /// Jedinstveni identifikator kupca. Primarni ključ.
        /// </summary>
        public int IdKupac { get; set; }

        /// <summary>
        /// Email adresa kupca.
        /// </summary>
        public string Email { get; set; }

        /// <inheritdoc/>
        public string TableName => "Kupac";

        /// <inheritdoc/>
        public string TableAlias => "k";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "k.idKupac";

        /// <inheritdoc/>
        public string SelectColumns => "k.idKupac, k.email";

        /// <inheritdoc/>
        public string InsertColumns => "email";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@email";

        /// <inheritdoc/>
        public string UpdateSetClause => "email = @email";

        /// <inheritdoc/>
        public string WhereCondition => "k.idKupac = @idKupac";

        /// <inheritdoc/>
        public string? JoinTable => null;

        /// <inheritdoc/>
        public string? JoinCondition => null;

        /// <inheritdoc/>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@email", SqlDbType.NVarChar) { Value = Email }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac });
            return parameters;
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac }
            };
        }

        /// <inheritdoc/>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var kupci = new List<IEntity>();
            while (reader.Read())
            {
                kupci.Add(new Kupac
                {
                    IdKupac = Convert.ToInt32(reader["idKupac"]),
                    Email = reader["email"].ToString()
                });
            }
            return kupci;
        }

        /// <inheritdoc/>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKupac > 0)
            {
                whereClause += " AND k.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                whereClause += " AND k.email = @email";
                parameters.Add(new SqlParameter("@email", Email));
            }

            return (whereClause, parameters);
        }
    }
}
