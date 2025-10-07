using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja kupca koji je fizičko lice.
    /// Sadrži lične podatke osobe.
    /// </summary>
    public class FizickoLice : Kupac
    {
        private bool isLoading = false;
        private string _ime;
        private string _prezime;
        private string _telefon;
        private string _jmbg;

        /// <summary>Podrazumevani konstruktor.</summary>
        public FizickoLice() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public FizickoLice(int idKupac, string email, string ime, string prezime, string telefon, string jmbg) : base(idKupac, email)
        {
            this._ime = ime;
            this._prezime = prezime;
            this._telefon = telefon;
            this._jmbg = jmbg;
        }

        /// <summary>
        /// Ime fizičkog lica. Ne sme biti prazno ili null.
        /// </summary>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string Ime
        {
            get => _ime;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Ime fizičkog lica ne sme biti prazno.", nameof(Ime));
                    }
                }
                _ime = value;
            }
        }

        /// <summary>
        /// Prezime fizičkog lica. Ne sme biti prazno ili null.
        /// </summary>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string Prezime
        {
            get => _prezime;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Prezime fizičkog lica ne sme biti prazno.", nameof(Prezime));
                    }
                }
                _prezime = value;
            }
        }

        /// <summary>
        /// Broj telefona. Ne sme biti prazan string ili null.
        /// </summary>
        /// <value>String koji predstavlja broj telefona kupca u proizvoljnom formatu.</value>
        /// <exception cref="ArgumentException">Baca se kada se pokuša postaviti prazan string, null ili string koji se sastoji samo od belina.</exception>
        public string Telefon
        {
            get => _telefon;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Telefon ne sme biti prazan string ili null.", nameof(Telefon));
                    }
                }
                _telefon = value;
            }
        }

        /// <summary>
        /// Jedinstveni matični broj građana (JMBG). Ne sme biti prazan ili null i mora biti sastavljen samo od brojeva.
        /// </summary>
        /// <value>String od tačno 13 cifara koji predstavlja jedinstveni matični broj građana.</value>
        /// <exception cref="ArgumentException">Baca se u sledećim slučajevima:
        /// - Kada se pokuša postaviti prazan string ili null
        /// - Kada string sadrži ne-numeričke karaktere
        /// - Kada string nema tačno 13 karaktera</exception>
        public string JMBG
        {
            get => _jmbg;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("JMBG ne sme biti prazan.", nameof(JMBG));
                    }

                    
                    if (!Regex.IsMatch(value, @"^\d+$"))
                    {
                        throw new ArgumentException("JMBG mora sadržati samo numeričke vrednosti (brojeve).", nameof(JMBG));
                    }

                    
                    if (value.Length != 13)
                    {
                         throw new ArgumentException("JMBG mora imati tačno 13 cifara.", nameof(JMBG));
                    }
                }
                _jmbg = value;
            }
        }

        /// <inheritdoc/>
        public override string TableName => "FizickoLice";

        /// <inheritdoc/>
        public override string TableAlias => "fl";

        /// <inheritdoc/>
        public override string PrimaryKeyColumn => "fl.idKupac";

        /// <inheritdoc/>
        public override string SelectColumns => "fl.idKupac, fl.ime, fl.prezime, fl.telefon, fl.jmbg, k.email";

        /// <inheritdoc/>
        public override string InsertColumns => "idKupac, ime, prezime, telefon, jmbg";

        /// <inheritdoc/>
        public override string InsertValuesPlaceholders => "@idKupac, @ime, @prezime, @telefon, @jmbg";

        /// <inheritdoc/>
        public override string UpdateSetClause => "ime = @ime, prezime = @prezime, telefon = @telefon, jmbg = @jmbg";

        /// <inheritdoc/>
        public override string WhereCondition => "idKupac = @idKupac";

        /// <inheritdoc/>
        public override string? JoinTable => "INNER JOIN Kupac k ON fl.idKupac = k.idKupac";

        /// <inheritdoc/>
        public override List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac },
                new SqlParameter("@ime", SqlDbType.NVarChar) { Value = Ime },
                new SqlParameter("@prezime", SqlDbType.NVarChar) { Value = Prezime },
                new SqlParameter("@telefon", SqlDbType.NVarChar) { Value = Telefon },
                new SqlParameter("@jmbg", SqlDbType.NVarChar) { Value = JMBG }
            };
        }

        /// <inheritdoc/>
        public override List<SqlParameter> GetUpdateParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@ime", SqlDbType.NVarChar) { Value = Ime },
                new SqlParameter("@prezime", SqlDbType.NVarChar) { Value = Prezime },
                new SqlParameter("@telefon", SqlDbType.NVarChar) { Value = Telefon },
                new SqlParameter("@jmbg", SqlDbType.NVarChar) { Value = JMBG },
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
            var fizickaLica = new List<IEntity>();
            while (reader.Read())
            {
                fizickaLica.Add(new FizickoLice
                {
                    isLoading = true,
                    IdKupac = Convert.ToInt32(reader["idKupac"]),
                    Email = reader["email"].ToString(),
                    Ime = reader["ime"].ToString(),
                    Prezime = reader["prezime"].ToString(),
                    Telefon = reader["telefon"].ToString(),
                    JMBG = reader["jmbg"].ToString()
                });
            }
            return fizickaLica;
        }

        /// <inheritdoc/>
        public override (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKupac > 0)
            {
                whereClause += " AND fl.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            if (!string.IsNullOrEmpty(Ime))
            {
                whereClause += " AND fl.ime LIKE @ime";
                parameters.Add(new SqlParameter("@ime", $"%{Ime}%"));
            }

            if (!string.IsNullOrEmpty(Prezime))
            {
                whereClause += " AND fl.prezime LIKE @prezime";
                parameters.Add(new SqlParameter("@prezime", $"%{Prezime}%"));
            }

            if (!string.IsNullOrEmpty(Telefon))
            {
                whereClause += " AND fl.telefon = @telefon";
                parameters.Add(new SqlParameter("@telefon", $"%{Telefon}%"));
            }

            if (!string.IsNullOrEmpty(JMBG))
            {
                whereClause += " AND fl.jmbg = @jmbg";
                parameters.Add(new SqlParameter("@jmbg", JMBG));
            }

            return (whereClause, parameters);
        }


    }
}
