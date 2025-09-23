using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Common.Domain.Tests
{
    public class KvalifikacijaTests
    {
        [Fact]
        public void Kvalifikacija_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var kvalifikacija = new Kvalifikacija();

            // Assert
            Assert.Equal("Kvalifikacija", kvalifikacija.TableName);
            Assert.Equal("k", kvalifikacija.TableAlias);
            Assert.Equal("idKvalifikacija", kvalifikacija.PrimaryKeyColumn);
            Assert.Null(kvalifikacija.JoinTable);
            Assert.Null(kvalifikacija.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var kvalifikacija = new Kvalifikacija
            {
                Naziv = "Sertifikat prodaje",
                Stepen = "Napredni"
            };

            // Act
            var parameters = kvalifikacija.GetInsertParameters();

            // Assert
            Assert.Equal(2, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@naziv" && p.Value.ToString() == "Sertifikat prodaje");
            Assert.Contains(parameters, p => p.ParameterName == "@stepen" && p.Value.ToString() == "Napredni");
        }

        [Fact]
        public void GetUpdateParameters_IncludesPrimaryKey()
        {
            // Arrange
            var kvalifikacija = new Kvalifikacija
            {
                IdKvalifikacija = 1,
                Naziv = "Sertifikat prodaje",
                Stepen = "Osnovni"
            };

            // Act
            var parameters = kvalifikacija.GetUpdateParameters();

            // Assert
            Assert.Equal(3, parameters.Count); // 2 insert + 1 PK
            Assert.Contains(parameters, p => p.ParameterName == "@idKvalifikacija" && (int)p.Value == 1);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdParameter()
        {
            // Arrange
            var kvalifikacija = new Kvalifikacija { IdKvalifikacija = 5 };

            // Act
            var parameters = kvalifikacija.GetPrimaryKeyParameters();

            // Assert
            var parameter = Assert.Single(parameters);
            Assert.Equal("@idKvalifikacija", parameter.ParameterName);
            Assert.Equal(5, parameter.Value);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectKvalifikacije()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE Kvalifikacija (
                idKvalifikacija INTEGER PRIMARY KEY,
                naziv TEXT NOT NULL,
                stepen TEXT NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO Kvalifikacija (naziv, stepen)
            VALUES ('Sertifikat prodaje', 'Osnovni'),
                   ('Napredni kurs', 'Napredni')
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Kvalifikacija";

            using var reader = cmd.ExecuteReader();
            var kvalifikacija = new Kvalifikacija();

            // Act
            var result = kvalifikacija.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as Kvalifikacija;
            Assert.Equal(1, first.IdKvalifikacija);
            Assert.Equal("Sertifikat prodaje", first.Naziv);
            Assert.Equal("Osnovni", first.Stepen);

            var second = result[1] as Kvalifikacija;
            Assert.Equal(2, second.IdKvalifikacija);
            Assert.Equal("Napredni kurs", second.Naziv);
            Assert.Equal("Napredni", second.Stepen);
        }

        [Theory]
        [InlineData(0, null, null, "1=1")] // Bez filtera
        [InlineData(5, null, null, "1=1 AND k.idKvalifikacija = @idKvalifikacija")] // Samo ID
        [InlineData(0, "Sertifikat", null, "1=1 AND k.naziv LIKE @naziv")] // Samo naziv
        [InlineData(0, null, "Napredni", "1=1 AND k.stepen = @stepen")] // Samo stepen
        [InlineData(3, "Sertifikat", "Osnovni",
            "1=1 AND k.idKvalifikacija = @idKvalifikacija AND k.naziv LIKE @naziv AND k.stepen = @stepen")] // Svi filteri
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(
            int id, string naziv, string stepen, string expectedClause)
        {
            // Arrange
            var kvalifikacija = new Kvalifikacija
            {
                IdKvalifikacija = id,
                Naziv = naziv,
                Stepen = stepen
            };

            // Act
            var (actualClause, parameters) = kvalifikacija.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (id > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(naziv)) expectedParamCount++;
            if (!string.IsNullOrEmpty(stepen)) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }

        [Fact]
        public void SelectColumns_ReturnsCorrectFormat()
        {
            // Arrange
            var kvalifikacija = new Kvalifikacija();

            // Act
            var result = kvalifikacija.SelectColumns;

            // Assert
            Assert.Equal("k.idKvalifikacija, k.naziv, k.stepen", result);
        }

        [Fact]
        public void InsertColumnsAndPlaceholders_ReturnCorrectValues()
        {
            // Arrange
            var kvalifikacija = new Kvalifikacija();

            // Act & Assert
            Assert.Equal("naziv, stepen", kvalifikacija.InsertColumns);
            Assert.Equal("@naziv, @stepen", kvalifikacija.InsertValuesPlaceholders);
        }
    }
}
