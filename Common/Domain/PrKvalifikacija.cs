using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja asocijaciju između prodavca i kvalifikacije.
    /// Svaka instanca predstavlja da određeni prodavac poseduje određenu kvalifikaciju, dobijenu određenog datuma.
    /// Mapira se na tabelu "PrKvalifikacija" u bazi podataka.
    /// </summary>
    public class PrKvalifikacija : IEntity
    {
        private bool isLoading = false;
        private Prodavac _prodavac;
        private Kvalifikacija _kvalifikacija;
        private DateTime _datumIzdavanja;

        /// <summary>Podrazumevani konstruktor.</summary>
        public PrKvalifikacija() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public PrKvalifikacija(Prodavac prodavac, Kvalifikacija kvalifikacija, DateTime datumIzdavanja)
        {
            this._prodavac = prodavac;
            this._kvalifikacija = kvalifikacija;
            this._datumIzdavanja = datumIzdavanja;
        }

        /// <summary>
        /// Prodavac koji poseduje kvalifikaciju. Ne sme biti null.
        /// </summary>
        /// <value>Instanca klase <see cref="Prodavac"/> koja predstavlja prodavca kojem je dodeljena kvalifikacija.</value>
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
        /// Kvalifikacija koju prodavac poseduje. Ne sme biti null.
        /// </summary>
        /// <value>Instanca klase <see cref="Kvalifikacija"/> koja predstavlja kvalifikaciju dodeljenu prodavcu.</value>
        /// <exception cref="ArgumentNullException">Baca se kada se pokuša postaviti null vrednost.</exception>
        public Kvalifikacija Kvalifikacija
        {
            get => _kvalifikacija;
            set
            {
                if (!isLoading)
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(Kvalifikacija), "Kvalifikacija ne sme biti null.");
                    }
                }
                _kvalifikacija = value;
            }
        }

        /// <summary>
        /// Datum izdavanja kvalifikacije prodavcu. Ne sme biti podrazumevana vrednost (DateTime.MinValue).
        /// </summary>
        /// <value>Datum kada je kvalifikacija izdata prodavcu. Mora biti validan datum.</value>
        /// <exception cref="ArgumentOutOfRangeException">Baca se kada se pokuša postaviti DateTime.MinValue ili drugi nevalidan datum.</exception>
        public DateTime DatumIzdavanja
        {
            get => _datumIzdavanja;
            set
            {
                if (!isLoading)
                {
                    if (value == DateTime.MinValue)
                    {
                        throw new ArgumentOutOfRangeException(nameof(DatumIzdavanja), "Datum izdavanja mora biti validan datum.");
                    }
                }
                _datumIzdavanja = value;
            }
        }

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
        public string? JoinTable => "JOIN Kvalifikacija k ON " +
            $"{TableAlias}.idKvalifikacija = k.idKvalifikacija";


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
            return parameters;
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
                    isLoading = true,
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
