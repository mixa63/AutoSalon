using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja ugovor koji se sklapa između kupca i prodavca.
    /// Mapira se na tabelu "Ugovor" u bazi podataka.
    /// </summary>
    public class Ugovor : IEntity
    {
        private bool isLoading = false;
        private DateTime _datum;
        private int _brAutomobila;
        private double _pdv;
        private double _iznosBezPDV;
        private double _iznosSaPDV;
        private Prodavac _prodavac;
        private Kupac _kupac;
        private List<StavkaUgovora> _stavke = new();

        /// <summary>Podrazumevani konstruktor.</summary>
        public Ugovor() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public Ugovor(int idUgovor, DateTime datum, int brAutomobila, double pdv, double iznosBezPDV, double iznosSaPDV, Prodavac prodavac, Kupac kupac, List<StavkaUgovora> stavke)
        {
            this.IdUgovor = idUgovor;
            this._datum = datum;
            this._brAutomobila = brAutomobila;
            this._pdv = pdv;
            this._iznosBezPDV = iznosBezPDV;
            this._iznosSaPDV = iznosSaPDV;
            this._prodavac = prodavac;
            this._kupac = kupac;
            this._stavke = stavke ?? new List<StavkaUgovora>();
        }

        /// <summary>
        /// Jedinstveni identifikator ugovora. Primarni ključ.
        /// </summary>
        public int IdUgovor { get; set; }

        /// <summary>
        /// Datum sklapanja ugovora. Ne sme biti podrazumevana vrednost (DateTime.MinValue).
        /// </summary>
        /// <value>Datum kada je ugovor sklopljen. Mora biti validan datum.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti DateTime.MinValue ili drugi nevalidan datum.</exception>
        public DateTime Datum
        {
            get => _datum;
            set
            {
                if (!isLoading)
                {
                    if (value == DateTime.MinValue)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Datum), "Datum ugovora mora biti validan datum.");
                    }
                }
                _datum = value;
            }
        }

        /// <summary>
        /// Broj automobila uključenih u ugovor. Mora biti veći od nule.
        /// </summary>
        /// <value>Pozitivan ceo broj koji predstavlja ukupan broj automobila u ugovoru.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti vrednost manja ili jednaka 0.</exception>
        public int BrAutomobila
        {
            get => _brAutomobila;
            set
            {
                if (!isLoading)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(BrAutomobila), "Broj automobila mora biti veći od nule.");
                    }
                }
                _brAutomobila = value;
            }
        }

        /// <summary>
        /// PDV. Mora biti nenegativan.
        /// </summary>
        /// <value>Decimalna vrednost koja predstavlja stopu PDV-a (npr. 0.2 za 20%).</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti negativna vrednost.</exception>
        public double PDV
        {
            get => _pdv;
            set
            {
                if (!isLoading)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(PDV), "PDV ne sme biti negativan.");
                    }
                }
                _pdv = value;
            }
        }

        /// <summary>
        /// Ukupan iznos bez PDV-a. Mora biti nenegativan.
        /// </summary>
        /// <value>Pozitivna decimalna vrednost koja predstavlja ukupan iznos ugovora bez PDV-a.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti negativna vrednost.</exception>
        public double IznosBezPDV
        {
            get => _iznosBezPDV;
            set
            {
                if (!isLoading)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(IznosBezPDV), "Iznos bez PDV-a ne sme biti negativan.");
                    }
                }
                _iznosBezPDV = value;
            }
        }

        /// <summary>
        /// Ukupan iznos sa PDV-om. Mora biti nenegativan.
        /// </summary>
        /// <value>Pozitivna decimalna vrednost koja predstavlja ukupan iznos ugovora sa uračunatim PDV-om.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti negativna vrednost.</exception>
        public double IznosSaPDV
        {
            get => _iznosSaPDV;
            set
            {
                if (!isLoading)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(IznosSaPDV), "Iznos sa PDV-om ne sme biti negativan.");
                    }
                }
                _iznosSaPDV = value;
            }
        }

        /// <summary>
        /// Prodavac koji sklapa ugovor. Ne sme biti null.
        /// </summary>
        /// <value>Instanca klase <see cref="Prodavac"/> koja predstavlja prodavca koji je sklopio ugovor.</value>
        /// <exception cref="ArgumentNullException">Baca se kada se pokuša postaviti null vrednost.</exception>
        public Prodavac Prodavac
        {
            get => _prodavac;
            set
            {
                if (!isLoading)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(Prodavac), "Prodavac ne sme biti null.");
                    }
                }
                _prodavac = value;
            }
        }

        /// <summary>
        /// Kupac koji sklapa ugovor. Ne sme biti null.
        /// </summary>
        /// <value>Instanca klase <see cref="Kupac"/> koja predstavlja kupca koji je sklopio ugovor.</value>
        /// <exception cref="ArgumentNullException">Baca se kada se pokuša postaviti null vrednost.</exception>
        public Kupac Kupac
        {
            get => _kupac;
            set
            {
                if (!isLoading)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(Kupac), "Kupac ne sme biti null.");
                    }
                }
                _kupac = value;
            }
        }

        /// <summary>
        /// Stavke ugovora koje predstavljaju automobile. Ne sme biti null.
        /// </summary>
        /// <value>Lista instanci klase <see cref="StavkaUgovora"/> koje predstavljaju pojedinačne stavke ugovora.</value>
        /// <exception cref="ArgumentNullException">Baca se kada se pokuša postaviti null vrednost.</exception>
        public List<StavkaUgovora> Stavke
        {
            get => _stavke;
            set
            {
                if (!isLoading)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(Stavke), "Lista stavki ne sme biti null.");
                    }
                }
                _stavke = value;
            }
        }

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
                        isLoading = true,
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
