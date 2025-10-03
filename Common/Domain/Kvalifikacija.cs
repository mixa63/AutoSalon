using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Predstavlja kvalifikaciju koja može pripadati prodavcu.
    /// Mapira se na tabelu "Kvalifikacija" u bazi podataka i implementira {@link IEntity} interfejs.
    /// </summary>
    public class Kvalifikacija : IEntity
    {
        /// <summary>
        /// Jedinstveni identifikator kvalifikacije. Primarni ključ u bazi podataka.
        /// </summary>
        public int IdKvalifikacija { get; set; }

        /// <summary>
        /// Naziv kvalifikacije.
        /// </summary>
        public string Naziv { get; set; }

        /// <summary>
        /// Stepen ili nivo kvalifikacije ("Osnovni", "Srednji", "Napredni").
        /// </summary>
        public string Stepen { get; set; }

        /// <inheritdoc/>
        public string TableName => "Kvalifikacija";

        /// <inheritdoc/>
        public string TableAlias => "k";

        /// <inheritdoc/>
        public string PrimaryKeyColumn => "idKvalifikacija";

        /// <inheritdoc/>
        public string SelectColumns => $"{TableAlias}.idKvalifikacija, {TableAlias}.naziv, {TableAlias}.stepen";

        /// <inheritdoc/>
        public string InsertColumns => "naziv, stepen";

        /// <inheritdoc/>
        public string InsertValuesPlaceholders => "@naziv, @stepen";

        /// <inheritdoc/>
        public string UpdateSetClause => "naziv = @naziv, stepen = @stepen";

        /// <inheritdoc/>
        public string WhereCondition => $"{TableAlias}.idKvalifikacija = @idKvalifikacija";

        /// <inheritdoc/>
        public string? JoinTable => null;

        /// <summary>
        /// Kreira <see cref="SqlParameter"/> listu za INSERT upit.
        /// </summary>
        /// <returns>Lista parametara: <c>@naziv</c>, <c>@stepen</c>.</returns>
        public List<SqlParameter> GetInsertParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@naziv", SqlDbType.NVarChar) { Value = Naziv ?? (object)DBNull.Value },
                new SqlParameter("@stepen", SqlDbType.NVarChar) { Value = Stepen ?? (object)DBNull.Value }
            };
        }

        /// <summary>
        /// Kreira <see cref="SqlParameter"/> listu za UPDATE upit (uključuje i primarni ključ).
        /// </summary>
        /// <returns>Lista parametara za UPDATE.</returns>
        public List<SqlParameter> GetUpdateParameters()
        {
            var parameters = GetInsertParameters();
            // Dodajemo primarni ključ na kraj, tako da poziv koji očekuje insert-parametre + PK radi jednostavno
            parameters.Add(new SqlParameter("@idKvalifikacija", SqlDbType.Int) { Value = IdKvalifikacija });
            return parameters;
        }

        /// <summary>
        /// Kreira parametar(e) za WHERE uslov zasnovan na primarnom ključu.
        /// </summary>
        /// <returns>Lista sa jednim parametrom: <c>@idKvalifikacija</c>.</returns>
        public List<SqlParameter> GetPrimaryKeyParameters()
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@idKvalifikacija", SqlDbType.Int) { Value = IdKvalifikacija }
            };
        }

        /// <summary>
        /// Mapira rezultate iz <see cref="DbDataReader"/> u listu objekata tipa <see cref="Kvalifikacija"/>.
        /// Koristi <see cref="Convert"/> kako bi podržao različite reader-implementacije (SqlClient, Sqlite, ...).
        /// </summary>
        /// <param name="reader">Reader koji sadrži kolone: idKvalifikacija, naziv, stepen.</param>
        /// <returns>Lista entiteta <see cref="IEntity"/> (konkretno <see cref="Kvalifikacija"/>).</returns>
        public List<IEntity> ReadEntities(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Kvalifikacija
                {
                    IdKvalifikacija = Convert.ToInt32(reader["idKvalifikacija"]),
                    Naziv = reader["naziv"]?.ToString() ?? string.Empty,
                    Stepen = reader["stepen"]?.ToString() ?? string.Empty
                });
            }
            return list;
        }

        /// <summary>
        /// Generički metod koji kreira SQL WHERE uslov sa parametrima na osnovu popunjenih svojstava entiteta.
        /// <para>
        /// Pravilo: 
        /// - Ako je <c>IdKvalifikacija &gt; 0</c> doda se uslov po id-u,
        /// - Ako je <c>Naziv</c> postavljen doda se LIKE uslov (<c>naziv LIKE @naziv</c>),
        /// - Ako je <c>Stepen</c> postavljen doda se tačno poklapanje (<c>stepen = @stepen</c>).
        /// </para>
        /// </summary>
        /// <returns>
        /// Tuple gde je:
        /// - <c>whereClause</c> SQL WHERE uslov (počinje sa <c>1=1</c>),
        /// - <c>parameters</c> lista <see cref="SqlParameter"/> koji odgovaraju placeholder-ima u uslovu.
        /// </returns>
        public (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters()
        {
            var parameters = new List<SqlParameter>();
            var whereClause = "1=1";

            if (IdKvalifikacija > 0)
            {
                whereClause += $" AND {TableAlias}.idKvalifikacija = @idKvalifikacija";
                parameters.Add(new SqlParameter("@idKvalifikacija", SqlDbType.Int) { Value = IdKvalifikacija });
            }

            if (!string.IsNullOrEmpty(Naziv))
            {
                whereClause += $" AND {TableAlias}.naziv LIKE @naziv";
                parameters.Add(new SqlParameter("@naziv", SqlDbType.NVarChar) { Value = $"%{Naziv}%" });
            }

            if (!string.IsNullOrEmpty(Stepen))
            {
                whereClause += $" AND {TableAlias}.stepen = @stepen";
                parameters.Add(new SqlParameter("@stepen", SqlDbType.NVarChar) { Value = Stepen });
            }

            return (whereClause, parameters);
        }
    }
}
