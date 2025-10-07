using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja domenski entitet Automobil koji se koristi u sistemu za praćenje prodaje automobila.
    /// Mapira se na tabelu "Automobil" u bazi podataka i implementira IEntity interfejs.
    /// </summary>
    public class Automobil : IEntity
    {
        private bool isLoading = false;
        private string _model;
        private string _oprema;
        private string _tipGoriva;
        private string _boja;
        private double _cena;

        /// <summary>Podrazumevani konstruktor.</summary>
        public Automobil() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public Automobil(int idAutomobil, string model, string oprema, string tipGoriva, string boja, double cena)
        {
            this.IdAutomobil = idAutomobil;
            this._model = model;
            this._oprema = oprema;
            this._tipGoriva = tipGoriva;
            this._boja = boja;
            this._cena = cena;
        }

        /// <summary>
        /// Jedinstveni identifikator automobila. Primarni ključ u bazi podataka.
        /// </summary>
        public int IdAutomobil { get; set; }

        /// <summary>
        /// Model automobila. Ne sme biti prazan string ili null.
        /// </summary>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string Model
        {
            get => _model;
            set
            {
                if(!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Model automobila ne sme biti prazan string.", nameof(Model));
                    }
                }
                _model = value;
            }
        }

        /// <summary>
        /// Opis opreme automobila. Ne sme biti prazan string ili null.
        /// </summary>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string Oprema 
        {
            get => _oprema;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Oprema automobila ne sme biti prazan string.", nameof(Oprema));
                    }
                }
                _oprema = value;
            }
        }

        /// <summary>
        /// Tip goriva koje automobil koristi. Ne sme biti prazan string ili null.
        /// </summary>
        /// <value>String koji označava vrstu goriva (npr. "benzin", "dizel", "električni").</value>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string TipGoriva
        {
            get => _tipGoriva;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Tip goriva automobila ne sme biti prazan string.", nameof(TipGoriva));
                    }
                }
                _tipGoriva = value;
            }
        }

        /// <summary>
        /// Boja automobila. Ne sme biti prazan string ili null.
        /// </summary>
        /// <value>String koji predstavlja boju automobila (npr. "crvena", "plava", "siva").</value>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string Boja
        {
            get => _boja;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Boja automobila ne sme biti prazan string.", nameof(Boja));
                    }
                }
                _boja = value;
            }
        }

        /// <summary>
        /// Cena automobila. Ne sme biti negativan broj.
        /// </summary>
        /// <value>Pozitivna decimalna vrednost koja predstavlja cenu automobila u valuti.</value>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti negativna vrednost.</exception>
        public double Cena
        {
            get => _cena;
            set
            {
                if (!isLoading)
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Cena automobila ne sme biti negativna.", nameof(Cena));
                    }
                }
                _cena = value;
            }
        }

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
                    isLoading = true,
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