using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Common.Domain
{
    /// <summary>
    /// Opšti interfejs koji definiše ponašanje svih domenskih entiteta u sistemu.
    /// Omogućava CRUD operacije, rad sa SQL parametrima i mapiranje iz baze.
    /// </summary>
    public interface IEntity
    {
        /// <summary>Naziv baze tabele koja odgovara entitetu.</summary>
        string TableName { get; }

        /// <summary>Alias tabele koji se koristi u SQL upitima.</summary>
        string TableAlias { get; }

        /// <summary>Lista kolona koje se selektuju u SQL upitima.</summary>
        string SelectColumns { get; }

        /// <summary>Naziv primarnog ključa tabele.</summary>
        string PrimaryKeyColumn { get; }

        /// <summary>Lista kolona za <c>INSERT</c> upit.</summary>
        string InsertColumns { get; }

        /// <summary>Placeholders (npr. <c>@Kolona1</c>, <c>@Kolona2</c>) za <c>INSERT</c> upit.</summary>
        string InsertValuesPlaceholders { get; }

        /// <summary>Deo SQL upita koji se koristi za <c>UPDATE</c> naredbu (<c>SET kolona=@vrednost</c>).</summary>
        string UpdateSetClause { get; }

        /// <summary>Uslov <c>WHERE</c> koji se koristi za primarni ključ.</summary>
        string WhereCondition { get; }

        /// <summary>Naziv spoljneg entiteta sa kojim se spaja (opciono).</summary>
        string? JoinTable { get; }

        /// <summary>Uslov spajanja za <c>JOIN</c> upite (opciono).</summary>
        string? JoinCondition { get; }

        /// <summary>
        /// Kreira listu parametara za <c>INSERT</c> operaciju.
        /// </summary>
        /// <returns>Lista <see cref="SqlParameter"/> objekata za <c>INSERT</c> upit.</returns>
        List<SqlParameter> GetInsertParameters();

        /// <summary>
        /// Kreira listu parametara za <c>UPDATE</c> operaciju.
        /// </summary>
        /// <returns>Lista <see cref="SqlParameter"/> objekata za <c>UPDATE</c> upit.</returns>
        List<SqlParameter> GetUpdateParameters();

        /// <summary>
        /// Kreira parametre za <c>WHERE</c> uslov na osnovu primarnog ključa.
        /// </summary>
        /// <returns>Lista <see cref="SqlParameter"/> objekata za primarni ključ.</returns>
        List<SqlParameter> GetPrimaryKeyParameters();

        /// <summary>
        /// Mapira rezultate iz <see cref="SqlDataReader"/> u listu entiteta.
        /// </summary>
        /// <param name="reader">Objekat <see cref="SqlDataReader"/> sa podacima iz baze.</param>
        /// <returns>Lista <c>IEntity</c> objekata popunjenih iz baze.</returns>
        List<IEntity> ReadEntities(SqlDataReader reader);

        /// <summary>
        /// Generički metod koji kreira SQL <c>WHERE</c> uslov sa pripadajućim parametrima.
        /// </summary>
        /// <returns>
        /// Tuple gde je:
        /// - <c>whereClause</c> string koji sadrži SQL <c>WHERE</c> uslov,
        /// - <c>parameters</c> lista <see cref="SqlParameter"/> objekata koji odgovaraju placeholder-ima u <c>whereClause</c>.
        /// </returns>
        (string whereClause, List<SqlParameter> parameters) GetWhereClauseWithParameters();
    }
}
