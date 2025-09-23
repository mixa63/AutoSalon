using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja asocijaciju između prodavca i kvalifikacije.
    /// Svaka instanca predstavlja da određeni prodavac poseduje određenu kvalifikaciju, dobijenu određenog datuma.
    /// Mapira se na tabelu "PrKvalifikacija" u bazi podataka.
    /// </summary>
    public class PrKvalifikacija : IEntity
    {
        /// <summary>
        /// Prodavac koji poseduje kvalifikaciju.
        /// </summary>
        public Prodavac Prodavac { get; set; }

        /// <summary>
        /// Kvalifikacija koju prodavac poseduje.
        /// </summary>
        public Kvalifikacija Kvalifikacija { get; set; }

        /// <summary>
        /// Datum izdavanja kvalifikacije prodavcu.
        /// </summary>
        public DateTime DatumIzdavanja { get; set; }

        /// <inheritdoc/>
        public string TableName => "PrKvalifikacija";

        /// <inheritdoc/>
        public string TableAlias => "pk";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "pk.idProdavac, pk.idKvalifikacija";

        /// <inheritdoc/>
        public string SelectColumns =>
            $"{TableAlias}.idProdavac, {TableAlias}.idKvalifikacija, {TableAlias}.datumIzdavanja";

        /// <inheritdoc/>
        public string InsertColumns => "idProdavac, idKvalifikacija, datumIzdavanja";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@idProdavac, @idKvalifikacija, @datumIzdavanja";

        /// <inheritdoc/>
        public string UpdateSetClause => "idProdavac = @idProdavac, idKvalifikacija = @idKvalifikacija, datumIzdavanja = @datumIzdavanja";

        /// <inheritdoc/>
        public string WhereCondition => $"{TableAlias}.idProdavac = @idProdavac AND {TableAlias}.idKvalifikacija = @idKvalifikacija";

        /// <inheritdoc/>
        public string? JoinTable => "Kvalifikacija k";

        /// <inheritdoc/>
        public string? JoinCondition => $"{TableAlias}.idKvalifikacija = k.idKvalifikacija";

        /// <inheritdoc/>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idProdavac", SqlDbType.Int) { Value = Prodavac.IdProdavac },
                new SqlParameter("@idKvalifikacija", SqlDbType.Int) { Value = Kvalifikacija.IdKvalifikacija },
                new SqlParameter("@datumIzdavanja", SqlDbType.Date) { Value = DatumIzdavanja }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            return parameters; // IdProdavac i IdKvalifikacija su primarni ključevi i već su uključeni
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idProdavac", SqlDbType.Int) { Value = Prodavac.IdProdavac },
                new SqlParameter("@idKvalifikacija", SqlDbType.Int) { Value = Kvalifikacija.IdKvalifikacija }
            };
        }

        /// <inheritdoc/>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new PrKvalifikacija
                {
                    Prodavac = new Prodavac { IdProdavac = Convert.ToInt32(reader["idProdavac"]) },
                    Kvalifikacija = new Kvalifikacija { IdKvalifikacija = Convert.ToInt32(reader["idKvalifikacija"]) },
                    DatumIzdavanja = Convert.ToDateTime(reader["datumIzdavanja"])
                });
            }
            return list;
        }

        /// <inheritdoc/>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (Prodavac.IdProdavac > 0)
            {
                whereClause += $" AND {TableAlias}.idProdavac = @idProdavac";
                parameters.Add(new SqlParameter("@idProdavac", Prodavac.IdProdavac));
            }

            if (Kvalifikacija.IdKvalifikacija > 0)
            {
                whereClause += $" AND {TableAlias}.idKvalifikacija = @idKvalifikacija";
                parameters.Add(new SqlParameter("@idKvalifikacija", Kvalifikacija.IdKvalifikacija));
            }

            if (DatumIzdavanja != DateTime.MinValue)
            {
                whereClause += $" AND {TableAlias}.datumIzdavanja = @datumIzdavanja";
                parameters.Add(new SqlParameter("@datumIzdavanja", DatumIzdavanja));
            }

            return (whereClause, parameters);
        }
    }
}
