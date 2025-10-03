using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja kupca koji je fizičko lice.
    /// Sadrži lične podatke osobe.
    /// </summary>
    public class FizickoLice : Kupac
    {
        /// <summary>
        /// Ime fizičkog lica.
        /// </summary>
        public string Ime { get; set; }

        /// <summary>
        /// Prezime fizičkog lica.
        /// </summary>
        public string Prezime { get; set; }

        /// <summary>
        /// Broj telefona.
        /// </summary>
        public string Telefon { get; set; }

        /// <summary>
        /// Jedinstveni matični broj građana (JMBG).
        /// </summary>
        public string JMBG { get; set; }

        /// <inheritdoc/>
        public override string TableName => "FizickoLice";

        /// <inheritdoc/>
        public override string TableAlias => "fl";

        /// <inheritdoc/>
        public override string PrimaryKeyColumn => "fl.idKupac";

        /// <inheritdoc/>
        public override string SelectColumns => "fl.idKupac, fl.ime, fl.prezime, fl.telefon, fl.jmbg, k.email";

        /// <inheritdoc/>
        public override string InsertColumns => "idKupac, ime, prezime, telefon, jmbg";

        /// <inheritdoc/>
        public override string InsertValuesPlaceholders => "@idKupac, @ime, @prezime, @telefon, @jmbg";

        /// <inheritdoc/>
        public override string UpdateSetClause => "ime = @ime, prezime = @prezime, telefon = @telefon, jmbg = @jmbg";

        /// <inheritdoc/>
        public override string WhereCondition => "idKupac = @idKupac";

        /// <inheritdoc/>
        public override string? JoinTable => "INNER JOIN Kupac k ON fl.idKupac = k.idKupac";

        /// <inheritdoc/>
        public override List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac },
                new SqlParameter("@ime", SqlDbType.NVarChar) { Value = Ime },
                new SqlParameter("@prezime", SqlDbType.NVarChar) { Value = Prezime },
                new SqlParameter("@telefon", SqlDbType.NVarChar) { Value = Telefon },
                new SqlParameter("@jmbg", SqlDbType.NVarChar) { Value = JMBG }
            };
        }

        /// <inheritdoc/>
        public override List<SqlParameter> GetUpdateParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@ime", SqlDbType.NVarChar) { Value = Ime },
                new SqlParameter("@prezime", SqlDbType.NVarChar) { Value = Prezime },
                new SqlParameter("@telefon", SqlDbType.NVarChar) { Value = Telefon },
                new SqlParameter("@jmbg", SqlDbType.NVarChar) { Value = JMBG },
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
            var fizickaLica = new List<IEntity>();
            while (reader.Read())
            {
                fizickaLica.Add(new FizickoLice
                {
                    IdKupac = Convert.ToInt32(reader["idKupac"]),
                    Email = reader["email"].ToString(),
                    Ime = reader["ime"].ToString(),
                    Prezime = reader["prezime"].ToString(),
                    Telefon = reader["telefon"].ToString(),
                    JMBG = reader["jmbg"].ToString()
                });
            }
            return fizickaLica;
        }

        /// <inheritdoc/>
        public override (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKupac > 0)
            {
                whereClause += " AND fl.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            if (!string.IsNullOrEmpty(Ime))
            {
                whereClause += " AND fl.ime LIKE @ime";
                parameters.Add(new SqlParameter("@ime", $"%{Ime}%"));
            }

            if (!string.IsNullOrEmpty(Prezime))
            {
                whereClause += " AND fl.prezime LIKE @prezime";
                parameters.Add(new SqlParameter("@prezime", $"%{Prezime}%"));
            }

            if (!string.IsNullOrEmpty(Telefon))
            {
                whereClause += " AND fl.telefon = @telefon";
                parameters.Add(new SqlParameter("@telefon", $"%{Telefon}%"));
            }

            if (!string.IsNullOrEmpty(JMBG))
            {
                whereClause += " AND fl.jmbg = @jmbg";
                parameters.Add(new SqlParameter("@jmbg", JMBG));
            }

            return (whereClause, parameters);
        }


    }
}
