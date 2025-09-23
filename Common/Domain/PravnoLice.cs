using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja kupca koji je pravno lice (firma).
    /// Sadrži dodatne informacije vezane za firmu.
    /// </summary>
    public class PravnoLice : IEntity
    {
        /// <summary>
        /// Referenca na generičkog kupca.
        /// </summary>
        public Kupac Kupac { get; set; }

        /// <summary>
        /// Naziv firme.
        /// </summary>
        public string NazivFirme { get; set; }

        /// <summary>
        /// PIB firme.
        /// </summary>
        public int PIB { get; set; }

        /// <summary>
        /// Matični broj firme.
        /// </summary>
        public string MaticniBroj { get; set; }

        /// <inheritdoc/>
        public string TableName => "PravnoLice";

        /// <inheritdoc/>
        public string TableAlias => "pl";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "pl.idKupac";

        /// <inheritdoc/>
        public string SelectColumns => "pl.idKupac, pl.nazivFirme, pl.pib, pl.maticniBroj";

        /// <inheritdoc/>
        public string InsertColumns => "idKupac, nazivFirme, pib, maticniBroj";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@idKupac, @nazivFirme, @pib, @maticniBroj";

        /// <inheritdoc/>
        public string UpdateSetClause => "nazivFirme = @nazivFirme, pib = @pib, maticniBroj = @maticniBroj";

        /// <inheritdoc/>
        public string WhereCondition => "pl.idKupac = @idKupac";

        /// <inheritdoc/>
        public string? JoinTable => null;

        /// <inheritdoc/>
        public string? JoinCondition => null;

        /// <inheritdoc/>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = Kupac.IdKupac },
                new SqlParameter("@nazivFirme", SqlDbType.NVarChar) { Value = NazivFirme },
                new SqlParameter("@pib", SqlDbType.Int) { Value = PIB },
                new SqlParameter("@maticniBroj", SqlDbType.NVarChar) { Value = MaticniBroj }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetUpdateParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@nazivFirme", SqlDbType.NVarChar) { Value = NazivFirme },
                new SqlParameter("@pib", SqlDbType.Int) { Value = PIB },
                new SqlParameter("@maticniBroj", SqlDbType.NVarChar) { Value = MaticniBroj },
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = Kupac.IdKupac }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = Kupac.IdKupac }
            };
        }

        /// <inheritdoc/>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var pravnaLica = new List<IEntity>();
            while (reader.Read())
            {
                pravnaLica.Add(new PravnoLice
                {
                    Kupac = new Kupac { IdKupac = Convert.ToInt32(reader["idKupac"]) },
                    NazivFirme = reader["nazivFirme"].ToString(),
                    PIB = Convert.ToInt32(reader["pib"]),
                    MaticniBroj = reader["maticniBroj"].ToString()
                });
            }
            return pravnaLica;
        }

        /// <inheritdoc/>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (Kupac != null && Kupac.IdKupac > 0)
            {
                whereClause += " AND pl.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", Kupac.IdKupac));
            }

            if (!string.IsNullOrEmpty(NazivFirme))
            {
                whereClause += " AND pl.nazivFirme LIKE @nazivFirme";
                parameters.Add(new SqlParameter("@nazivFirme", $"%{NazivFirme}%"));
            }

            if (PIB > 0)
            {
                whereClause += " AND pl.pib = @pib";
                parameters.Add(new SqlParameter("@pib", PIB));
            }

            if (!string.IsNullOrEmpty(MaticniBroj))
            {
                whereClause += " AND pl.maticniBroj = @maticniBroj";
                parameters.Add(new SqlParameter("@maticniBroj", MaticniBroj));
            }

            return (whereClause, parameters);
        }

    }
}
