using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja ugovor koji se sklapa između kupca i prodavca.
    /// Mapira se na tabelu "Ugovor" u bazi podataka.
    /// </summary>
    public class Ugovor : IEntity
    {
        /// <summary>
        /// Jedinstveni identifikator ugovora. Primarni ključ.
        /// </summary>
        public int IdUgovor { get; set; }

        /// <summary>
        /// Datum sklapanja ugovora.
        /// </summary>
        public DateTime Datum { get; set; }

        /// <summary>
        /// Broj automobila uključenih u ugovor.
        /// </summary>
        public int BrAutomobila { get; set; }

        /// <summary>
        /// PDV.
        /// </summary>
        public double PDV { get; set; }

        /// <summary>
        /// Ukupan iznos bez PDV-a.
        /// </summary>
        public double IznosBezPDV { get; set; }

        /// <summary>
        /// Ukupan iznos sa PDV-om.
        /// </summary>
        public double IznosSaPDV { get; set; }

        /// <summary>
        /// ID prodavca koji sklapa ugovor.
        /// </summary>
        public int IdProdavac { get; set; }

        /// <summary>
        /// ID kupca koji sklapa ugovor.
        /// </summary>
        public int IdKupac { get; set; }

        /// <inheritdoc/>
        public string TableName => "Ugovor";

        /// <inheritdoc/>
        public string TableAlias => "u";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "u.idUgovor";

        /// <inheritdoc/>
        public string SelectColumns =>
            "u.idUgovor, u.datum, u.brAutomobila, u.pdv, u.iznosBezPDV, u.iznosSaPDV, u.idProdavac, u.idKupac";

        /// <inheritdoc/>
        public string InsertColumns => "datum, brAutomobila, pdv, iznosBezPDV, iznosSaPDV, idProdavac, idKupac";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@datum, @brAutomobila, @pdv, @iznosBezPDV, @iznosSaPDV, @idProdavac, @idKupac";

        /// <inheritdoc/>
        public string UpdateSetClause =>
            "datum = @datum, brAutomobila = @brAutomobila, pdv = @pdv, iznosBezPDV = @iznosBezPDV, " +
            "iznosSaPDV = @iznosSaPDV, idProdavac = @idProdavac, idKupac = @idKupac";

        /// <inheritdoc/>
        public string WhereCondition => "u.idUgovor = @idUgovor";

        /// <inheritdoc/>
        public string? JoinTable => null;

        /// <inheritdoc/>
        public string? JoinCondition => null;

        /// <inheritdoc/>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@datum", SqlDbType.Date) { Value = Datum },
                new SqlParameter("@brAutomobila", SqlDbType.Int) { Value = BrAutomobila },
                new SqlParameter("@pdv", SqlDbType.Float) { Value = PDV },
                new SqlParameter("@iznosBezPDV", SqlDbType.Float) { Value = IznosBezPDV },
                new SqlParameter("@iznosSaPDV", SqlDbType.Float) { Value = IznosSaPDV },
                new SqlParameter("@idProdavac", SqlDbType.Int) { Value = IdProdavac },
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac }
            };
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idUgovor", SqlDbType.Int) { Value = IdUgovor });
            return parameters;
        }

        /// <inheritdoc/>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idUgovor", SqlDbType.Int) { Value = IdUgovor }
            };
        }

        /// <inheritdoc/>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var ugovori = new List<IEntity>();
            while (reader.Read())
            {
                ugovori.Add(new Ugovor
                {
                    IdUgovor = Convert.ToInt32(reader["idUgovor"]),
                    Datum = Convert.ToDateTime(reader["datum"]),
                    BrAutomobila = Convert.ToInt32(reader["brAutomobila"]),
                    PDV = Convert.ToDouble(reader["pdv"]),
                    IznosBezPDV = Convert.ToDouble(reader["iznosBezPDV"]),
                    IznosSaPDV = Convert.ToDouble(reader["iznosSaPDV"]),
                    IdProdavac = Convert.ToInt32(reader["idProdavac"]),
                    IdKupac = Convert.ToInt32(reader["idKupac"])
                });
            }
            return ugovori;
        }

        /// <inheritdoc/>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdUgovor > 0)
            {
                whereClause += " AND u.idUgovor = @idUgovor";
                parameters.Add(new SqlParameter("@idUgovor", IdUgovor));
            }

            if (Datum != DateTime.MinValue)
            {
                whereClause += " AND u.datum = @datum";
                parameters.Add(new SqlParameter("@datum", Datum));
            }

            if (BrAutomobila > 0)
            {
                whereClause += " AND u.brAutomobila = @brAutomobila";
                parameters.Add(new SqlParameter("@brAutomobila", BrAutomobila));
            }

            if (PDV > 0)
            {
                whereClause += " AND u.pdv <= @pdv";
                parameters.Add(new SqlParameter("@pdv", PDV));
            }

            if (IznosBezPDV > 0)
            {
                whereClause += " AND u.iznosBezPDV <= @iznosBezPDV";
                parameters.Add(new SqlParameter("@iznosBezPDV", IznosBezPDV));
            }

            if (IznosSaPDV > 0)
            {
                whereClause += " AND u.iznosSaPDV <= @iznosSaPDV";
                parameters.Add(new SqlParameter("@iznosSaPDV", IznosSaPDV));
            }

            if (IdProdavac > 0)
            {
                whereClause += " AND u.idProdavac = @idProdavac";
                parameters.Add(new SqlParameter("@idProdavac", IdProdavac));
            }

            if (IdKupac > 0)
            {
                whereClause += " AND u.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            return (whereClause, parameters);
        }
    }
}
