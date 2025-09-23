using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Common.Domain.Tests
{
    public class ProdavacTests
    {
        [Fact]
        public void Prodavac_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var prodavac = new Prodavac();

            // Assert
            Assert.Equal("Prodavac", prodavac.TableName);
            Assert.Equal("p", prodavac.TableAlias);
            Assert.Equal("idProdavac", prodavac.PrimaryKeyColumn);
            Assert.Null(prodavac.JoinTable);
            Assert.Null(prodavac.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var prodavac = new Prodavac
            {
                Ime = "Petar",
                Prezime = "Petrović",
                Username = "ppetar",
                Password = "lozinka123"
            };

            // Act
            var parameters = prodavac.GetInsertParameters();

            // Assert
            Assert.Equal(4, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@ime" && (string)p.Value == "Petar");
            Assert.Contains(parameters, p => p.ParameterName == "@prezime" && (string)p.Value == "Petrović");
            Assert.Contains(parameters, p => p.ParameterName == "@username" && (string)p.Value == "ppetar");
            Assert.Contains(parameters, p => p.ParameterName == "@password" && (string)p.Value == "lozinka123");
        }

        [Fact]
        public void GetUpdateParameters_IncludesPrimaryKey()
        {
            // Arrange
            var prodavac = new Prodavac
            {
                IdProdavac = 10,
                Ime = "Marko",
                Prezime = "Marković",
                Username = "mmarko",
                Password = "12345"
            };

            // Act
            var parameters = prodavac.GetUpdateParameters();

            // Assert
            Assert.Equal(5, parameters.Count); // 4 insert + 1 PK
            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac" && (int)p.Value == 10);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdParameter()
        {
            // Arrange
            var prodavac = new Prodavac { IdProdavac = 7 };

            // Act
            var parameters = prodavac.GetPrimaryKeyParameters();

            // Assert
            var parameter = Assert.Single(parameters);
            Assert.Equal("@idProdavac", parameter.ParameterName);
            Assert.Equal(7, parameter.Value);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectProdavci()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE Prodavac (
                idProdavac INTEGER PRIMARY KEY,
                ime TEXT NOT NULL,
                prezime TEXT NOT NULL,
                username TEXT NOT NULL,
                password TEXT NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO Prodavac (ime, prezime, username, password)
            VALUES ('Milan', 'Milić', 'mmilic', 'pass1'),
                   ('Ana', 'Anić', 'aanic', 'pass2')
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Prodavac";

            using var reader = cmd.ExecuteReader();
            var prodavac = new Prodavac();

            // Act
            var result = prodavac.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as Prodavac;
            Assert.Equal(1, first.IdProdavac);
            Assert.Equal("Milan", first.Ime);
            Assert.Equal("Milić", first.Prezime);
            Assert.Equal("mmilic", first.Username);
            Assert.Equal("pass1", first.Password);

            var second = result[1] as Prodavac;
            Assert.Equal(2, second.IdProdavac);
            Assert.Equal("Ana", second.Ime);
        }

        [Theory]
        [InlineData(0, null, null, null, "1=1")] // Bez filtera
        [InlineData(3, null, null, null, "1=1 AND p.idProdavac = @idProdavac")] // Samo ID
        [InlineData(0, "Milan", null, null, "1=1 AND p.ime LIKE @ime")] // Samo ime
        [InlineData(0, null, "Milić", null, "1=1 AND p.prezime LIKE @prezime")] // Samo prezime
        [InlineData(0, null, null, "mmilic", "1=1 AND p.username = @username")] // Samo username
        [InlineData(2, "Ana", "Anić", "aanic",
            "1=1 AND p.idProdavac = @idProdavac AND p.ime LIKE @ime AND p.prezime LIKE @prezime AND p.username = @username")] // Svi filteri
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(
            int id, string ime, string prezime, string username, string expectedClause)
        {
            // Arrange
            var prodavac = new Prodavac
            {
                IdProdavac = id,
                Ime = ime,
                Prezime = prezime,
                Username = username
            };

            // Act
            var (actualClause, parameters) = prodavac.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (id > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(ime)) expectedParamCount++;
            if (!string.IsNullOrEmpty(prezime)) expectedParamCount++;
            if (!string.IsNullOrEmpty(username)) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }

        [Fact]
        public void SelectColumns_ReturnsCorrectFormat()
        {
            // Arrange
            var prodavac = new Prodavac();

            // Act
            var result = prodavac.SelectColumns;

            // Assert
            Assert.Equal("p.idProdavac, p.ime, p.prezime, p.username, p.password", result);
        }

        [Fact]
        public void InsertColumnsAndPlaceholders_ReturnCorrectValues()
        {
            // Arrange
            var prodavac = new Prodavac();

            // Act & Assert
            Assert.Equal("ime, prezime, username, password", prodavac.InsertColumns);
            Assert.Equal("@ime, @prezime, @username, @password", prodavac.InsertValuesPlaceholders);
        }
    }
}
