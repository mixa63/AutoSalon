using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Common.Domain.Tests
{
    public class PrKvalifikacijaTests
    {
        [Fact]
        public void PrKvalifikacija_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var pk = new PrKvalifikacija
            {
                Prodavac = new Prodavac(),
                Kvalifikacija = new Kvalifikacija()
            };

            // Assert
            Assert.Equal("PrKvalifikacija", pk.TableName);
            Assert.Equal("pk", pk.TableAlias);
            Assert.Equal("pk.idProdavac, pk.idKvalifikacija", pk.PrimaryKeyColumn);
            Assert.Equal("Kvalifikacija k", pk.JoinTable);
            Assert.Equal("pk.idKvalifikacija = k.idKvalifikacija", pk.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var pk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = 1 },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = 2 },
                DatumIzdavanja = new DateTime(2025, 9, 23)
            };

            // Act
            var parameters = pk.GetInsertParameters();

            // Assert
            Assert.Equal(3, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@idKvalifikacija" && (int)p.Value == 2);
            Assert.Contains(parameters, p => p.ParameterName == "@datumIzdavanja" && (DateTime)p.Value == new DateTime(2025, 9, 23));
        }

        [Fact]
        public void GetUpdateParameters_ReturnsSameAsInsert()
        {
            // Arrange
            var pk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = 1 },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = 2 },
                DatumIzdavanja = new DateTime(2025, 9, 23)
            };

            // Act
            var parameters = pk.GetUpdateParameters();

            // Assert
            Assert.Equal(3, parameters.Count);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsBothKeys()
        {
            // Arrange
            var pk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = 1 },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = 2 }
            };

            // Act
            var parameters = pk.GetPrimaryKeyParameters();

            // Assert
            Assert.Equal(2, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@idKvalifikacija" && (int)p.Value == 2);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectPrKvalifikacija()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE PrKvalifikacija (
                idProdavac INTEGER NOT NULL,
                idKvalifikacija INTEGER NOT NULL,
                datumIzdavanja DATE NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO PrKvalifikacija (idProdavac, idKvalifikacija, datumIzdavanja)
            VALUES (1, 10, '2025-09-23'),
                   (2, 20, '2025-09-24')
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM PrKvalifikacija";

            using var reader = cmd.ExecuteReader();
            var pk = new PrKvalifikacija();

            // Act
            var result = pk.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as PrKvalifikacija;
            Assert.Equal(1, first.Prodavac.IdProdavac);
            Assert.Equal(10, first.Kvalifikacija.IdKvalifikacija);
            Assert.Equal(new DateTime(2025, 9, 23), first.DatumIzdavanja);

            var second = result[1] as PrKvalifikacija;
            Assert.Equal(2, second.Prodavac.IdProdavac);
            Assert.Equal(20, second.Kvalifikacija.IdKvalifikacija);
            Assert.Equal(new DateTime(2025, 9, 24), second.DatumIzdavanja);
        }

        [Theory]
        [InlineData(0, 0, "0001-01-01", "1=1")] // Bez filtera
        [InlineData(1, 0, "0001-01-01", "1=1 AND pk.idProdavac = @idProdavac")] // Samo idProdavac
        [InlineData(0, 2, "0001-01-01", "1=1 AND pk.idKvalifikacija = @idKvalifikacija")] // Samo idKvalifikacija
        [InlineData(0, 0, "2025-09-23", "1=1 AND pk.datumIzdavanja = @datumIzdavanja")] // Samo datum
        [InlineData(1, 2, "2025-09-23", "1=1 AND pk.idProdavac = @idProdavac AND pk.idKvalifikacija = @idKvalifikacija AND pk.datumIzdavanja = @datumIzdavanja")] // Svi filteri
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(
            int idProdavac, int idKvalifikacija, string datumString, string expectedClause)
        {
            // Arrange
            var datum = DateTime.Parse(datumString);
            var pk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = idProdavac },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = idKvalifikacija },
                DatumIzdavanja = datum
            };

            // Act
            var (actualClause, parameters) = pk.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (idProdavac > 0) expectedParamCount++;
            if (idKvalifikacija > 0) expectedParamCount++;
            if (datum != DateTime.MinValue) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }

        [Fact]
        public void SelectColumns_ReturnsCorrectFormat()
        {
            // Arrange
            var pk = new PrKvalifikacija();

            // Act
            var result = pk.SelectColumns;

            // Assert
            Assert.Equal("pk.idProdavac, pk.idKvalifikacija, pk.datumIzdavanja", result);
        }

        [Fact]
        public void InsertColumnsAndPlaceholders_ReturnCorrectValues()
        {
            // Arrange
            var pk = new PrKvalifikacija();

            // Act & Assert
            Assert.Equal("idProdavac, idKvalifikacija, datumIzdavanja", pk.InsertColumns);
            Assert.Equal("@idProdavac, @idKvalifikacija, @datumIzdavanja", pk.InsertValuesPlaceholders);
        }
    }
}
