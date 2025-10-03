using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja domenski entitet Automobil koji se koristi u sistemu za praćenje prodaje automobila.
    /// Mapira se na tabelu "Automobil" u bazi podataka i implementira IEntity interfejs.
    /// </summary>
    public class Automobil : IEntity
    {
        /// <summary>
        /// Jedinstveni identifikator automobila. Primarni ključ u bazi podataka.
        /// </summary>
        public int IdAutomobil { get; set; }

        /// <summary>
        /// Model automobila.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Opis opreme automobila.
        /// </summary>
        public string Oprema { get; set; }

        /// <summary>
        /// Tip goriva koje automobil koristi (benzin, dizel, električni).
        /// </summary>
        public string TipGoriva { get; set; }

        /// <summary>
        /// Boja automobila.
        /// </summary>
        public string Boja { get; set; }

        /// <summary>
        /// Cena automobila.
        /// </summary>
        public double Cena { get; set; }

        /// <inheritdoc/>
        public string TableName => "Automobil";

        /// <inheritdoc/>
        public string TableAlias => "a";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "idAutomobil";

        /// <inheritdoc/>
        public string SelectColumns =>
            $"{TableAlias}.idAutomobil, {TableAlias}.model, {TableAlias}.oprema, " +
            $"{TableAlias}.tipGoriva, {TableAlias}.boja, {TableAlias}.cena";

        /// <inheritdoc/>
        public string InsertColumns => "model, oprema, tipGoriva, boja, cena";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@model, @oprema, @tipGoriva, @boja, @cena";

        /// <inheritdoc/>
        public string UpdateSetClause =>
            "model = @model, oprema = @oprema, tipGoriva = @tipGoriva, " +
            "boja = @boja, cena = @cena";

        /// <inheritdoc/>
        public string WhereCondition => $"{TableAlias}.idAutomobil = @idAutomobil";

        /// <inheritdoc/>
        public string? JoinTable => null;

        /// <inheritdoc/>
        public string? JoinCondition => null;

        /// <inheritdoc/>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@model", SqlDbType.NVarChar) { Value = Model },
                new SqlParameter("@oprema", SqlDbType.NVarChar) { Value = Oprema },
                new SqlParameter("@tipGoriva", SqlDbType.NVarChar) { Value = TipGoriva },
                new SqlParameter("@boja", SqlDbType.NVarChar) { Value = Boja },
                new SqlParameter("@cena", SqlDbType.Float) { Value = Cena }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idAutomobil", IdAutomobil));
            return parameters;
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idAutomobil", IdAutomobil)
            };
        }

        /// <inheritdoc/>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var automobili = new List<IEntity>();
            while (reader.Read())
            {
                automobili.Add(new Automobil
                {
                    IdAutomobil = Convert.ToInt32(reader["idAutomobil"]),
                    Model = reader["model"].ToString(),
                    Oprema = reader["oprema"].ToString(),
                    TipGoriva = reader["tipGoriva"].ToString(),
                    Boja = reader["boja"].ToString(),
                    Cena = Convert.ToDouble(reader["cena"])
                });
            }
            return automobili;
        }

        /// <inheritdoc/>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdAutomobil > 0)
            {
                whereClause += $" AND {TableAlias}.idAutomobil = @idAutomobil";
                parameters.Add(new SqlParameter("@idAutomobil", IdAutomobil));
            }

            if (!string.IsNullOrEmpty(Model))
            {
                whereClause += $" AND {TableAlias}.model LIKE @model";
                parameters.Add(new SqlParameter("@model", $"%{Model}%"));
            }

            if (!string.IsNullOrEmpty(TipGoriva))
            {
                whereClause += $" AND {TableAlias}.tipGoriva = @tipGoriva";
                parameters.Add(new SqlParameter("@tipGoriva", TipGoriva));
            }

            if (!string.IsNullOrEmpty(Boja))
            {
                whereClause += $" AND {TableAlias}.boja = @boja";
                parameters.Add(new SqlParameter("@boja", Boja));
            }

            if (Cena > 0)
            {
                whereClause += $" AND {TableAlias}.cena <= @cena";
                parameters.Add(new SqlParameter("@cena", Cena));
            }

            return (whereClause, parameters);
        }
    }
}