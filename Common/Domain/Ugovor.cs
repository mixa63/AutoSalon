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
        /// Prodavac koji sklapa ugovor.
        /// </summary>
        public Prodavac Prodavac { get; set; }

        /// <summary>
        /// Kupac koji sklapa ugovor.
        /// </summary>
        public Kupac Kupac { get; set; }

        /// <summary>
        /// Stavke ugovora koje predstavljaju automobile.
        /// </summary>
        public List<StavkaUgovora> Stavke { get; set; } = new();

        /// <inheritdoc/>
        public string TableName => "Ugovor";

        /// <inheritdoc/>
        public string TableAlias => "u";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "u.idUgovor";

        /// <inheritdoc/>
        public string SelectColumns =>
            "u.idUgovor, u.datum, u.brAutomobila, u.pdv, u.iznosBezPDV, u.iznosSaPDV, " +
            "u.idProdavac, u.idKupac, " +
            "p.ime AS ProdavacIme, k.email AS KupacEmail, " +
            "a.model AS AutomobilModel";

        /// <inheritdoc/>
        public string InsertColumns => "datum, brAutomobila, pdv, iznosBezPDV, iznosSaPDV, idProdavac, idKupac";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@datum, @brAutomobila, @pdv, @iznosBezPDV, @iznosSaPDV, @idProdavac, @idKupac";

        /// <inheritdoc/>
        public string UpdateSetClause =>
            "datum = @datum, brAutomobila = @brAutomobila, pdv = @pdv, iznosBezPDV = @iznosBezPDV, " +
            "iznosSaPDV = @iznosSaPDV, idProdavac = @idProdavac, idKupac = @idKupac";

        /// <inheritdoc/>
        public string WhereCondition => "idUgovor = @idUgovor";

        /// <inheritdoc/>
        public string? JoinTable => 
            "JOIN Prodavac p ON u.idProdavac = p.idProdavac " +
            "JOIN Kupac k ON u.idKupac = k.idKupac " +
            "LEFT JOIN StavkaUgovora s ON u.idUgovor = s.idUgovor " +
            "LEFT JOIN Automobil a ON s.idAutomobil = a.idAutomobil";


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
                new SqlParameter("@idProdavac", SqlDbType.Int) { Value = Prodavac.IdProdavac },
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = Kupac.IdKupac }
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
            var ugovoriDict = new Dictionary<int, Ugovor>();

            while (reader.Read())
            {
                int idUgovor = Convert.ToInt32(reader["idUgovor"]);

                if (!ugovoriDict.ContainsKey(idUgovor))
                {
                    ugovoriDict[idUgovor] = new Ugovor
                    {
                        IdUgovor = idUgovor,
                        Datum = Convert.ToDateTime(reader["datum"]),
                        BrAutomobila = Convert.ToInt32(reader["brAutomobila"]),
                        PDV = Convert.ToDouble(reader["pdv"]),
                        IznosBezPDV = Convert.ToDouble(reader["iznosBezPDV"]),
                        IznosSaPDV = Convert.ToDouble(reader["iznosSaPDV"]),
                        Prodavac = new Prodavac
                        {
                            IdProdavac = Convert.ToInt32(reader["idProdavac"]),
                            Ime = reader["ProdavacIme"].ToString()
                        },
                        Kupac = new Kupac
                        {
                            IdKupac = Convert.ToInt32(reader["idKupac"]),
                            Email = reader["KupacEmail"].ToString()
                        }
                    };
                }

            }

            return ugovoriDict.Values.Cast<IEntity>().ToList();
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
            if (Prodavac != null)
            {
                if (!string.IsNullOrEmpty(Prodavac.Ime))
                {
                    whereClause += " AND p.ime LIKE @prodavacIme";
                    parameters.Add(new SqlParameter("@prodavacIme", $"%{Prodavac.Ime}%"));
                }
            }

            if (Kupac != null)
            {
                if (Kupac.IdKupac > 0)
                {
                    whereClause += " AND u.IdKupac = @idKupac";
                    parameters.Add(new SqlParameter("@idKupac", Kupac.IdKupac));
                }
                if (!string.IsNullOrEmpty(Kupac.Email))
                {
                    whereClause += " AND k.email LIKE @kupacEmail";
                    parameters.Add(new SqlParameter("@kupacEmail", $"%{Kupac.Email}%"));
                }
            }


            if (!string.IsNullOrWhiteSpace(Stavke?.Count > 0 ? Stavke[0].Automobil.Model : null))
            {
                whereClause += @" AND EXISTS (
                        SELECT 1 
                        FROM StavkaUgovora s2
                        JOIN Automobil a2 ON s2.idAutomobil = a2.idAutomobil
                        WHERE s2.idUgovor = u.idUgovor
                        AND a2.model LIKE @automobilModel
                     )";

                parameters.Add(new SqlParameter("@automobilModel", $"%{Stavke[0].Automobil.Model}%"));
            }

            return (whereClause, parameters);
        }
    }
}
