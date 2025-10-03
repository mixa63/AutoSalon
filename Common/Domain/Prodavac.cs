using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja prodavca u sistemu (osobu koja zaključuje ugovore i poseduje kvalifikacije).
    /// Mapira se na tabelu "Prodavac" u bazi podataka i implementira opšti <see cref="IEntity"/> interfejs.
    /// </summary>
    public class Prodavac : IEntity
    {
        /// <summary>Jedinstveni identifikator prodavca (primarni ključ).</summary>
        public int IdProdavac { get; set; }

        /// <summary>Ime prodavca.</summary>
        public string Ime { get; set; }

        /// <summary>Prezime prodavca.</summary>
        public string Prezime { get; set; }

        /// <summary>Korisničko ime (username) za prijavu prodavca.</summary>
        public string Username { get; set; }

        /// <summary>Lozinka prodavca.</summary>
        public string Password { get; set; }

        /// <inheritdoc/>
        public string TableName => "Prodavac";

        /// <inheritdoc/>
        public string TableAlias => "p";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "idProdavac";

        /// <inheritdoc/>
        public string SelectColumns =>
            $"{TableAlias}.idProdavac, {TableAlias}.ime, {TableAlias}.prezime, {TableAlias}.username, {TableAlias}.password";

        /// <inheritdoc/>
        public string InsertColumns => "ime, prezime, username, password";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@ime, @prezime, @username, @password";

        /// <inheritdoc/>
        public string UpdateSetClause => "ime = @ime, prezime = @prezime, username = @username, password = @password";

        /// <inheritdoc/>
        public string WhereCondition => $"{TableAlias}.idProdavac = @idProdavac";

        /// <inheritdoc/>
        public string? JoinTable => null;

        /// <summary>
        /// Generiše listu <see cref="SqlParameter"/> objekata za <c>INSERT</c> upit za tabelu Prodavac.
        /// </summary>
        /// <returns>Lista SQL parametara za INSERT: <c>@ime</c>, <c>@prezime</c>, <c>@username</c>, <c>@password</c>.</returns>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@ime", SqlDbType.NVarChar) { Value = Ime },
                new SqlParameter("@prezime", SqlDbType.NVarChar) { Value = Prezime },
                new SqlParameter("@username", SqlDbType.NVarChar) { Value = Username },
                new SqlParameter("@password", SqlDbType.NVarChar) { Value = Password }
            };
        }

        /// <summary>
        /// Generiše listu <see cref="SqlParameter"/> objekata za <c>UPDATE</c> upit.
        /// Uključuje sve insert parametre i na kraj dodaje parametar za primarni ključ.
        /// </summary>
        /// <returns>Lista SQL parametara za UPDATE.</returns>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            parameters.Add(new SqlParameter("@idProdavac", SqlDbType.Int) { Value = IdProdavac });
            return parameters;
        }

        /// <summary>
        /// Generiše listu parametara koji predstavljaju primarni ključ (WHERE uslov).
        /// </summary>
        /// <returns>Lista sa jednim parametrom: <c>@idProdavac</c>.</returns>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idProdavac", SqlDbType.Int) { Value = IdProdavac }
            };
        }

        /// <summary>
        /// Mapira rezultate iz <see cref="DbDataReader"/> u listu objekata tipa <see cref="Prodavac"/>.
        /// Ovaj metod očekuje kolone: <c>idProdavac, ime, prezime, username, password</c>.
        /// </summary>
        /// <param name="reader">Otvoreni <see cref="DbDataReader"/> koji sadrži redove iz upita.</param>
        /// <returns>Lista <see cref="IEntity"/> (konkretno <see cref="Prodavac"/>) popunjena iz reader-a.</returns>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Prodavac
                {
                    IdProdavac = Convert.ToInt32(reader["idProdavac"]),
                    Ime = reader["ime"]?.ToString(),
                    Prezime = reader["prezime"]?.ToString(),
                    Username = reader["username"]?.ToString(),
                    Password = reader["password"]?.ToString()
                });
            }
            return list;
        }

        /// <summary>
        /// Generiše SQL <c>WHERE</c> uslov i pripadajuće SQL parametre za pretragu/profiltriranje prodavaca.
        /// Pohranjuje početni uslov <c>1=1</c> i dodaje dodatne uslove za svako popunjeno svojstvo.
        /// </summary>
        /// <returns>
        /// Tuple gde:
        /// - <c>whereClause</c> je string koji sadrži SQL <c>WHERE</c> uslov (bez klauzule "WHERE"),
        /// - <c>parameters</c> je lista <see cref="SqlParameter"/> objekata koji odgovaraju placeholder-ima u <c>whereClause</c>.
        /// </returns>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdProdavac > 0)
            {
                whereClause += $" AND {TableAlias}.idProdavac = @idProdavac";
                parameters.Add(new SqlParameter("@idProdavac", SqlDbType.Int) { Value = IdProdavac });
            }

            if (!string.IsNullOrEmpty(Ime))
            {
                
                whereClause += $" AND {TableAlias}.ime LIKE @ime";
                parameters.Add(new SqlParameter("@ime", SqlDbType.NVarChar) { Value = $"%{Ime}%" });
            }

            if (!string.IsNullOrEmpty(Prezime))
            {
               
                whereClause += $" AND {TableAlias}.prezime LIKE @prezime";
                parameters.Add(new SqlParameter("@prezime", SqlDbType.NVarChar) { Value = $"%{Prezime}%" });
            }

            if (!string.IsNullOrEmpty(Username))
            {
               
                whereClause += $" AND {TableAlias}.username = @username";
                parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar) { Value = Username });
            }
            if (!string.IsNullOrEmpty(Password))
            {
                
                whereClause += $" AND {TableAlias}.password = @password";
                parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar) { Value = Password });
            }
            return (whereClause, parameters);
        }
    }
}
