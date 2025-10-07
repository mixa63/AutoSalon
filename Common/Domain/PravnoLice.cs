using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja kupca koji je pravno lice.
    /// Sadrži dodatne informacije vezane za firmu.
    /// </summary>
    public class PravnoLice : Kupac
    {
        private bool isLoading = false;
        private string _nazivFirme;
        private string _pib;
        private string _maticniBroj;

        /// <summary>Podrazumevani konstruktor.</summary>
        public PravnoLice() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public PravnoLice(int idKupac, string email, string nazivFirme, string pib, string maticniBroj) : base(idKupac, email)
        {
            this._nazivFirme = nazivFirme;
            this._pib = pib;
            this._maticniBroj = maticniBroj;
        }
        /// <summary>
        /// Naziv firme. Ne sme biti prazan string ili null.
        /// </summary>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string NazivFirme
        {
            get => _nazivFirme;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Naziv firme ne sme biti prazan string.", nameof(NazivFirme));
                    }
                }
                _nazivFirme = value;
            }
        }

        /// <summary>
        /// PIB firme. Ne sme biti prazan string ili null.
        /// </summary>
        /// <value>String koji predstavlja Poreski identifikacioni broj firme.</value>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public String PIB
        {
            get => _pib;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("PIB ne sme biti prazan string.", nameof(PIB));
                    }
                }
                _pib = value;
            }
        }

        /// <summary>
        /// Matični broj firme. Ne sme biti prazan string ili null.
        /// </summary>
        /// <value>String koji predstavlja matični broj firme prema registru.</value>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string MaticniBroj
        {
            get => _maticniBroj;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Maticni broj ne sme biti prazan string.", nameof(MaticniBroj));
                    }
                }
                _maticniBroj = value;
            }
        }

        /// <inheritdoc/>
        public override string TableName => "PravnoLice";

        /// <inheritdoc/>
        public override string TableAlias => "pl";

        /// <inheritdoc/>
        public override string PrimaryKeyColumn => "pl.idKupac";

        /// <inheritdoc/>
        public override string SelectColumns => "pl.idKupac, pl.nazivFirme, pl.pib, pl.maticniBroj, k.email";

        /// <inheritdoc/>
        public override string InsertColumns => "idKupac, nazivFirme, pib, maticniBroj";

        /// <inheritdoc/>
        public override string InsertValuesPlaceholders => "@idKupac, @nazivFirme, @pib, @maticniBroj";

        /// <inheritdoc/>
        public override string UpdateSetClause => "nazivFirme = @nazivFirme, pib = @pib, maticniBroj = @maticniBroj";

        /// <inheritdoc/>
        public override string WhereCondition => "idKupac = @idKupac";

        /// <inheritdoc/>
        public override string? JoinTable => "INNER JOIN Kupac k ON pl.idKupac = k.idKupac";


        /// <inheritdoc/>
        public override List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac },
                new SqlParameter("@nazivFirme", SqlDbType.NVarChar) { Value = NazivFirme },
                new SqlParameter("@pib", SqlDbType.NVarChar) { Value = PIB },
                new SqlParameter("@maticniBroj", SqlDbType.NVarChar) { Value = MaticniBroj }
            };
        }

        /// <inheritdoc/>
        public override List<SqlParameter> GetUpdateParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@nazivFirme", SqlDbType.NVarChar) { Value = NazivFirme },
                new SqlParameter("@pib", SqlDbType.NVarChar) { Value = PIB },
                new SqlParameter("@maticniBroj", SqlDbType.NVarChar) { Value = MaticniBroj },
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
            var pravnaLica = new List<IEntity>();
            while (reader.Read())
            {
                pravnaLica.Add(new PravnoLice
                {
                    isLoading = true,
                    IdKupac = Convert.ToInt32(reader["idKupac"]),
                    Email = reader["email"].ToString(),
                    NazivFirme = reader["nazivFirme"].ToString(),
                    PIB = reader["pib"].ToString(),
                    MaticniBroj = reader["maticniBroj"].ToString()
                });
            }
            return pravnaLica;
        }

        /// <inheritdoc/>
        public override (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKupac > 0)
            {
                whereClause += " AND pl.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            if (!string.IsNullOrEmpty(NazivFirme))
            {
                whereClause += " AND pl.nazivFirme LIKE @nazivFirme";
                parameters.Add(new SqlParameter("@nazivFirme", $"%{NazivFirme}%"));
            }

            if (!string.IsNullOrEmpty(PIB))
            {
                whereClause += " AND pl.pib = @pib";
                parameters.Add(new SqlParameter("@pib", PIB));
            }

            if (!string.IsNullOrEmpty(MaticniBroj))
            {
                whereClause += " AND pl.maticniBroj = @maticniBroj";
                parameters.Add(new SqlParameter("@maticniBroj", MaticniBroj));
            }

            return (whereClause, parameters);
        }

    }
}
