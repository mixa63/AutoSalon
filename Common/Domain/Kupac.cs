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
    /// Predstavlja generičkog kupca u sistemu.
    /// Ovo je zajednički entitet za <see cref="PravnoLice"/> i <see cref="FizickoLice"/>.
    /// Sadrži osnovne informacije koje su zajedničke za oba tipa.
    /// </summary>
    public class Kupac : IEntity
    {
        private bool isLoading = false;
        private string _email;

        /// <summary>Podrazumevani konstruktor.</summary>
        public Kupac() { }

        /// <summary>Konstruktor za JSON deserializaciju.</summary>
        [JsonConstructor]
        public Kupac(int idKupac, string email)
        {
            this.IdKupac = idKupac;
            this._email = email;
        }

        /// <summary>
        /// Jedinstveni identifikator kupca. Primarni ključ.
        /// </summary>
        public int IdKupac { get; set; }

        /// <summary>
        /// Email adresa kupca. Ne sme biti prazna ili null i mora biti u ispravnom formatu.
        /// </summary>
        /// <value>String koji predstavlja email adresu u validnom formatu (npr. "ime@domen.rs").</value>
        /// <exception cref="ArgumentException">Baca se u sledećim slučajevima:
        /// - Kada se pokuša postaviti prazan string ili null
        /// - Kada email adresa nije u validnom formatu (nedostaje @ simbol ili domen)</exception>
        public string Email
        {
            get => _email;
            set
            {
                if (!isLoading)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Email adresa kupca ne sme biti prazna.", nameof(Email));
                    }

                    // NOVA PROVERA: Validacija formata
                    if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        throw new ArgumentException("Email adresa nije u ispravnom formatu.", nameof(Email));
                    }
                }
                _email = value;
            }
        }

        /// <inheritdoc/>
        public virtual string TableName => "Kupac";

        /// <inheritdoc/>
        public virtual string TableAlias => "k";

        /// <inheritdoc/>
        public virtual string PrimaryKeyColumn => "k.idKupac";

        /// <inheritdoc/>
        public virtual string SelectColumns => "k.idKupac, k.email";

        /// <inheritdoc/>
        public virtual string InsertColumns => "email";

        /// <inheritdoc/>
        public virtual string InsertValuesPlaceholders => "@email";

        /// <inheritdoc/>
        public virtual string UpdateSetClause => "email = @email";

        /// <inheritdoc/>
        public virtual string WhereCondition => "idKupac = @idKupac";

        /// <inheritdoc/>
        public virtual string? JoinTable => null;

        /// <inheritdoc/>
        public virtual List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@email", SqlDbType.NVarChar) { Value = Email }
            };
        }

        /// <inheritdoc/>
        public virtual List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac });
            return parameters;
        }

        /// <inheritdoc/>
        public virtual List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKupac", SqlDbType.Int) { Value = IdKupac }
            };
        }

        /// <inheritdoc/>
        public virtual List<IEntity> ReadEntities(DbDataReader reader)
        {
            var kupci = new List<IEntity>();
            while (reader.Read())
            {
                kupci.Add(new Kupac
                {
                    isLoading = true,
                    IdKupac = Convert.ToInt32(reader["idKupac"]),
                    Email = reader["email"].ToString()
                });
            }
            return kupci;
        }

        /// <inheritdoc/>
        public virtual (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKupac > 0)
            {
                whereClause += " AND k.idKupac = @idKupac";
                parameters.Add(new SqlParameter("@idKupac", IdKupac));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                whereClause += " AND k.email LIKE @email";
                parameters.Add(new SqlParameter("@email", $"%{Email}%"));
            }

            return (whereClause, parameters);
        }
    }
}
