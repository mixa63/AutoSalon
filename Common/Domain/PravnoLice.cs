using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja kupca koji je pravno lice.
    /// Sadrži dodatne informacije vezane za firmu.
    /// </summary>
    public class PravnoLice : Kupac
    {
        /// <summary>
        /// Naziv firme.
        /// </summary>
        public string NazivFirme { get; set; }

        /// <summary>
        /// PIB firme.
        /// </summary>
        public String PIB { get; set; }

        /// <summary>
        /// Matični broj firme.
        /// </summary>
        public string MaticniBroj { get; set; }

        /// <inheritdoc/>
        public override string TableName => "PravnoLice";

        /// <inheritdoc/>
        public override string TableAlias => "pl";

        /// <inheritdoc/>
        public override string PrimaryKeyColumn => "pl.idKupac";

        /// <inheritdoc/>
        public override string SelectColumns => "pl.idKupac, pl.nazivFirme, pl.pib, pl.maticniBroj, k.email";

        /// <inheritdoc/>
        public override string InsertColumns => "idKupac, nazivFirme, pib, maticniBroj";

        /// <inheritdoc/>
        public override string InsertValuesPlaceholders => "@idKupac, @nazivFirme, @pib, @maticniBroj";

        /// <inheritdoc/>
        public override string UpdateSetClause => "nazivFirme = @nazivFirme, pib = @pib, maticniBroj = @maticniBroj";

        /// <inheritdoc/>
        public override string WhereCondition => "idKupac = @idKupac";

        /// <inheritdoc/>
        public override string? JoinTable => "INNER JOIN Kupac k ON pl.idKupac = k.idKupac";


        /// <inheritdoc/>
        public override List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac },
                new SqlParameter("@nazivFirme", SqlDbType.NVarChar) { Value = NazivFirme },
                new SqlParameter("@pib", SqlDbType.NVarChar) { Value = PIB },
                new SqlParameter("@maticniBroj", SqlDbType.NVarChar) { Value = MaticniBroj }
            };
        }

        /// <inheritdoc/>
        public override List<SqlParameter> GetUpdateParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@nazivFirme", SqlDbType.NVarChar) { Value = NazivFirme },
                new SqlParameter("@pib", SqlDbType.NVarChar) { Value = PIB },
                new SqlParameter("@maticniBroj", SqlDbType.NVarChar) { Value = MaticniBroj },
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac }
            };
        }

        /// <inheritdoc/>
        public override List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac }
            };
        }

        /// <inheritdoc/>
        public override List<IEntity> ReadEntities(DbDataReader reader)
        {
            var pravnaLica = new List<IEntity>();
            while (reader.Read())
            {
                pravnaLica.Add(new PravnoLice
                {
                    IdKupac = Convert.ToInt32(reader["idKupac"]),
                    Email = reader["email"].ToString(),
                    NazivFirme = reader["nazivFirme"].ToString(),
                    PIB = reader["pib"].ToString(),
                    MaticniBroj = reader["maticniBroj"].ToString()
                });
            }
            return pravnaLica;
        }

        /// <inheritdoc/>
        public override (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKupac > 0)
            {
                whereClause += " AND pl.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            if (!string.IsNullOrEmpty(NazivFirme))
            {
                whereClause += " AND pl.nazivFirme LIKE @nazivFirme";
                parameters.Add(new SqlParameter("@nazivFirme", $"%{NazivFirme}%"));
            }

            if (!string.IsNullOrEmpty(PIB))
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
