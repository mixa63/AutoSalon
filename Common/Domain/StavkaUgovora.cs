using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja stavku ugovora koja povezuje ugovor sa automobilima.
    /// Svaka stavka predstavlja jedan automobil.
    /// Mapira se na tabelu "StavkaUgovora" u bazi podataka.
    /// </summary>
    public class StavkaUgovora : IEntity
    {
        private bool isLoading = false;
        private Ugovor _ugovor;
        private int _rb;
        private Automobil _automobil;
        private double _popust;
        private double _cenaAutomobila;
        private double _iznos;

        /// <summary>Podrazumevani konstruktor.</summary>
        public StavkaUgovora() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public StavkaUgovora(Ugovor ugovor, int rb, Automobil automobil, double popust, double cenaAutomobila, double iznos)
        {
            this._ugovor = ugovor;
            this._rb = rb;
            this._automobil = automobil;
            this._popust = popust;
            this._cenaAutomobila = cenaAutomobila;
            this._iznos = iznos;
        }

        /// <summary>
        /// Ugovor kojem stavka pripada. Ne sme biti null.
        /// </summary>
        /// <value>Instanca klase <see cref="Ugovor"/> kojoj pripada stavka.</value>
        /// <exception cref="ArgumentNullException">Baca se kada se pokuša postaviti null vrednost.</exception>
        public Ugovor Ugovor
        {
            get => _ugovor;
            set
            {
                if (!isLoading)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(Ugovor), "Ugovor ne sme biti null.");
                    }
                }
                _ugovor = value;
            }
        }

        /// <summary>
        /// Redni broj stavke unutar ugovora. Mora biti veći od nule.
        /// </summary>
        /// <value>Pozitivan ceo broj koji predstavlja redni broj stavke u okviru ugovora.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti vrednost manja ili jednaka 0.</exception>
        public int Rb
        {
            get => _rb;
            set
            {
                if (!isLoading)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Rb), "Redni broj stavke mora biti veći od nule.");
                    }
                }
                _rb = value;
            }
        }

        /// <summary>
        /// Referenca na automobil koji je predmet stavke. Ne sme biti null.
        /// </summary>
        /// <value>Instanca klase <see cref="Automobil"/> koja predstavlja automobil u stavci ugovora.</value>
        /// <exception cref="ArgumentNullException">Baca se kada se pokuša postaviti null vrednost.</exception>
        public Automobil Automobil
        {
            get => _automobil;
            set
            {
                if (!isLoading)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(Automobil), "Automobil ne sme biti null.");
                    }
                }
                _automobil = value;
            }
        }

        /// <summary>
        /// Popust na automobil koji je predmet stavke. Mora biti između 0 i 1.
        /// </summary>
        /// <value>Decimalna vrednost između 0 i 1 koja predstavlja popust (npr. 0.1 za 10% popusta).</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti vrednost manja od 0 ili veća od 1.</exception>
        public double Popust
        {
            get => _popust;
            set
            {
                if (!isLoading)
                {
                    if (value < 0 || value > 1)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Popust), "Popust mora biti između 0 i 1.");
                    }
                }
                _popust = value;
            }
        }

        /// <summary>
        /// Cena automobila koji je predmet stavke. Mora biti nenegativna.
        /// </summary>
        /// <value>Pozitivna decimalna vrednost koja predstavlja cenu automobila pre popusta.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti negativna vrednost.</exception>
        public double CenaAutomobila
        {
            get => _cenaAutomobila;
            set
            {
                if (!isLoading)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(CenaAutomobila), "Cena automobila ne sme biti negativna.");
                    }
                }
                _cenaAutomobila = value;
            }
        }

        /// <summary>
        /// Iznos stavke sa popustom. Mora biti nenegativan.
        /// </summary>
        /// <value>Pozitivna decimalna vrednost koja predstavlja konačan iznos stavke nakon primene popusta.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti negativna vrednost.</exception>
        public double Iznos
        {
            get => _iznos;
            set
            {
                if (!isLoading)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Iznos), "Iznos stavke ne sme biti negativan.");
                    }
                }
                _iznos = value;
            }
        }

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
                    isLoading = true,
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