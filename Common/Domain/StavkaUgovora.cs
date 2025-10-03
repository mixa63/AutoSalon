using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja stavku ugovora koja povezuje ugovor sa automobilima.
    /// Svaka stavka predstavlja jedan automobil.
    /// Mapira se na tabelu "StavkaUgovora" u bazi podataka.
    /// </summary>
    public class StavkaUgovora : IEntity
    {
        /// <summary>
        /// Ugovor kojem stavka pripada.
        /// </summary>
        public Ugovor Ugovor { get; set; }

        /// <summary>
        /// Redni broj stavke unutar ugovora.
        /// </summary>
        public int Rb { get; set; }
     
        /// <summary>
        /// Referenca na automobil koji je predmet stavke.
        /// </summary>
        public Automobil Automobil { get; set; }

        /// <summary>
        /// Popust na automobil koji je predmet stavke.
        /// </summary>
        public double Popust { get; set; }

        /// <summary>
        /// Cena automobila koji je predmet stavke.
        /// </summary>
        public double CenaAutomobila { get; set; }

        /// <summary>
        /// Iznos stavke sa popustom.
        /// </summary>
        public double Iznos { get; set; }

        /// <inheritdoc/>
        public string TableName => "StavkaUgovora";

        /// <inheritdoc/>
        public string TableAlias => "su";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => $"su.idUgovor, su.rb";

        /// <inheritdoc/>
        public string SelectColumns =>
            $"{TableAlias}.idUgovor, {TableAlias}.rb, {TableAlias}.idAutomobil, {TableAlias}.cenaAutomobila, {TableAlias}.popust, {TableAlias}.iznos";

        /// <inheritdoc/>
        public string InsertColumns => "idUgovor, rb, cenaAutomobila, popust, idAutomobil, iznos";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@idUgovor, @rb, @cenaAutomobila, @popust, @idAutomobil, @iznos";

        /// <inheritdoc/>
        public string UpdateSetClause =>
            "idUgovor = @idUgovor, rb = @rb, idAutomobil = @idAutomobil, cenaAutomobila = @cenaAutomobila, popust = @popust,  iznos = @iznos";

        /// <inheritdoc/>
        public string WhereCondition => $"idUgovor = @idUgovor AND rb = @rb";

        /// <inheritdoc/>
        public string? JoinTable => "JOIN Automobil a ON " +
            $"{TableAlias}.idAutomobil = a.idAutomobil";

        /// <inheritdoc/>
        public string? JoinCondition => $"{TableAlias}.idAutomobil = a.idAutomobil";

        /// <inheritdoc/>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idUgovor", SqlDbType.Int) { Value = Ugovor.IdUgovor },
                new SqlParameter("@rb", SqlDbType.Int) { Value = Rb },
                new SqlParameter("@idAutomobil", SqlDbType.Int) { Value = Automobil.IdAutomobil },
                new SqlParameter("@cenaAutomobila", SqlDbType.Float) { Value = CenaAutomobila },
                new SqlParameter("@popust", SqlDbType.Float) { Value = Popust },
                new SqlParameter("@iznos", SqlDbType.Float) { Value = Iznos }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idUgovor", SqlDbType.Int) { Value = Ugovor.IdUgovor });
            return parameters;
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idUgovor", SqlDbType.Int) { Value = Ugovor.IdUgovor },
                new SqlParameter("@rb", SqlDbType.Int) { Value = Rb }
            };
        }

        /// <inheritdoc/>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var stavke = new List<IEntity>();
            while (reader.Read())
            {
                stavke.Add(new StavkaUgovora
                {
                    Ugovor = new Ugovor { IdUgovor = Convert.ToInt32(reader["idUgovor"]) },
                    Rb = Convert.ToInt32(reader["rb"]),
                    Automobil = new Automobil{ IdAutomobil = Convert.ToInt32(reader["idAutomobil"]) },
                    CenaAutomobila = Convert.ToDouble(reader["cenaAutomobila"]),
                    Popust = Convert.ToDouble(reader["popust"]),
                    Iznos = Convert.ToDouble(reader["iznos"])
                });
            }
            return stavke;
        }

        /// <inheritdoc/>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (Ugovor.IdUgovor > 0)
            {
                whereClause += $" AND {TableAlias}.idUgovor = @idUgovor";
                parameters.Add(new SqlParameter("@idUgovor", Ugovor.IdUgovor));
            }

            if (Rb > 0)
            {
                whereClause += $" AND {TableAlias}.rb = @rb";
                parameters.Add(new SqlParameter("@rb", Rb));
            }

            return (whereClause, parameters);
        }
    }
}