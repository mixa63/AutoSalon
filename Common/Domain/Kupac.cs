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
        public virtual string TableName => "Kupac";

        /// <inheritdoc/>
        public virtual string TableAlias => "k";

        /// <inheritdoc/>
        public virtual string PrimaryKeyColumn => "k.idKupac";

        /// <inheritdoc/>
        public virtual string SelectColumns => "k.idKupac, k.email";

        /// <inheritdoc/>
        public virtual string InsertColumns => "email";

        /// <inheritdoc/>
        public virtual string InsertValuesPlaceholders => "@email";

        /// <inheritdoc/>
        public virtual string UpdateSetClause => "email = @email";

        /// <inheritdoc/>
        public virtual string WhereCondition => "idKupac = @idKupac";

        /// <inheritdoc/>
        public virtual string? JoinTable => null;

        /// <inheritdoc/>
        public virtual List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@email", SqlDbType.NVarChar) { Value = Email }
            };
        }

        /// <inheritdoc/>
        public virtual List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac });
            return parameters;
        }

        /// <inheritdoc/>
        public virtual List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac }
            };
        }

        /// <inheritdoc/>
        public virtual List<IEntity> ReadEntities(DbDataReader reader)
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
        public virtual (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
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
                whereClause += " AND k.email LIKE @email";
                parameters.Add(new SqlParameter("@email", $"%{Email}%"));
            }

            return (whereClause, parameters);
        }
    }
}
