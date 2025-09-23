using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Common.Domain.Tests
{
    public class UgovorTests
    {
        [Fact]
        public void Ugovor_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var ugovor = new Ugovor();

            // Assert
            Assert.Equal("Ugovor", ugovor.TableName);
            Assert.Equal("u", ugovor.TableAlias);
            Assert.Equal("u.idUgovor", ugovor.PrimaryKeyColumn);
            Assert.Null(ugovor.JoinTable);
            Assert.Null(ugovor.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var ugovor = new Ugovor
            {
                Datum = new DateTime(2025, 9, 23),
                BrAutomobila = 3,
                PDV = 0.2,
                IznosBezPDV = 100000,
                IznosSaPDV = 120000,
                IdProdavac = 1,
                IdKupac = 2
            };

            // Act
            var parameters = ugovor.GetInsertParameters();

            // Assert
            Assert.Equal(7, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@datum" && (DateTime)p.Value == new DateTime(2025, 9, 23));
            Assert.Contains(parameters, p => p.ParameterName == "@brAutomobila" && (int)p.Value == 3);
            Assert.Contains(parameters, p => p.ParameterName == "@pdv" && (double)p.Value == 0.2);
            Assert.Contains(parameters, p => p.ParameterName == "@iznosBezPDV" && (double)p.Value == 100000);
            Assert.Contains(parameters, p => p.ParameterName == "@iznosSaPDV" && (double)p.Value == 120000);
            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 2);
        }

        [Fact]
        public void GetUpdateParameters_IncludesPrimaryKey()
        {
            // Arrange
            var ugovor = new Ugovor
            {
                IdUgovor = 5,
                Datum = DateTime.Today,
                BrAutomobila = 2,
                PDV = 0.1,
                IznosBezPDV = 50000,
                IznosSaPDV = 55000,
                IdProdavac = 1,
                IdKupac = 2
            };

            // Act
            var parameters = ugovor.GetUpdateParameters();

            // Assert
            Assert.Equal(8, parameters.Count); // 7 insert + 1 primary key
            Assert.Contains(parameters, p => p.ParameterName == "@idUgovor" && (int)p.Value == 5);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdUgovorParameter()
        {
            // Arrange
            var ugovor = new Ugovor { IdUgovor = 10 };

            // Act
            var parameters = ugovor.GetPrimaryKeyParameters();

            // Assert
            var parameter = Assert.Single(parameters);
            Assert.Equal("@idUgovor", parameter.ParameterName);
            Assert.Equal(10, parameter.Value);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectUgovori()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE Ugovor (
                idUgovor INTEGER PRIMARY KEY,
                datum TEXT NOT NULL,
                brAutomobila INTEGER NOT NULL,
                pdv REAL NOT NULL,
                iznosBezPDV REAL NOT NULL,
                iznosSaPDV REAL NOT NULL,
                idProdavac INTEGER NOT NULL,
                idKupac INTEGER NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO Ugovor (datum, brAutomobila, pdv, iznosBezPDV, iznosSaPDV, idProdavac, idKupac)
            VALUES ('2025-09-23', 2, 0.2, 50000, 60000, 1, 2),
                   ('2025-09-24', 1, 0.1, 30000, 33000, 1, 3)
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ugovor";

            using var reader = cmd.ExecuteReader();
            var ugovor = new Ugovor();

            // Act
            var result = ugovor.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as Ugovor;
            Assert.Equal(1, first.IdUgovor);
            Assert.Equal(new DateTime(2025, 9, 23), first.Datum);
            Assert.Equal(2, first.BrAutomobila);
            Assert.Equal(0.2, first.PDV);
            Assert.Equal(50000, first.IznosBezPDV);
            Assert.Equal(60000, first.IznosSaPDV);
            Assert.Equal(1, first.IdProdavac);
            Assert.Equal(2, first.IdKupac);

            var second = result[1] as Ugovor;
            Assert.Equal(2, second.IdUgovor);
            Assert.Equal(new DateTime(2025, 9, 24), second.Datum);
        }

        [Theory]
        [InlineData(0, "0001-01-01", 0, 0, 0, 0, 0, 0, "1=1")] // Bez filtera
        [InlineData(5, "0001-01-01", 0, 0, 0, 0, 0, 0, "1=1 AND u.idUgovor = @idUgovor")] // Samo idUgovor
        [InlineData(0, "2025-09-23", 0, 0, 0, 0, 0, 0, "1=1 AND u.datum = @datum")] // Samo datum
        [InlineData(0, "0001-01-01", 2, 0, 0, 0, 0, 0, "1=1 AND u.brAutomobila = @brAutomobila")] // Samo brAutomobila
        [InlineData(0, "0001-01-01", 0, 0.2, 0, 0, 0, 0, "1=1 AND u.pdv <= @pdv")] // Samo PDV
        [InlineData(0, "0001-01-01", 0, 0, 50000, 0, 0, 0, "1=1 AND u.iznosBezPDV <= @iznosBezPDV")] // Samo iznosBezPDV
        [InlineData(0, "0001-01-01", 0, 0, 0, 60000, 0, 0, "1=1 AND u.iznosSaPDV <= @iznosSaPDV")] // Samo iznosSaPDV
        [InlineData(0, "0001-01-01", 0, 0, 0, 0, 1, 0, "1=1 AND u.idProdavac = @idProdavac")] // Samo idProdavac
        [InlineData(0, "0001-01-01", 0, 0, 0, 0, 0, 2, "1=1 AND u.idKupac = @idKupac")] // Samo idKupac
        [InlineData(5, "2025-09-23", 2, 0.2, 50000, 60000, 1, 2,
            "1=1 AND u.idUgovor = @idUgovor AND u.datum = @datum AND u.brAutomobila = @brAutomobila AND u.pdv <= @pdv AND u.iznosBezPDV <= @iznosBezPDV AND u.iznosSaPDV <= @iznosSaPDV AND u.idProdavac = @idProdavac AND u.idKupac = @idKupac")]
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(
            int idUgovor, string datumStr, int brAutomobila, double pdv, double iznosBezPDV, double iznosSaPDV,
            int idProdavac, int idKupac, string expectedClause)
        {
            // Arrange
            DateTime datum = DateTime.Parse(datumStr);
            var ugovor = new Ugovor
            {
                IdUgovor = idUgovor,
                Datum = datum,
                BrAutomobila = brAutomobila,
                PDV = pdv,
                IznosBezPDV = iznosBezPDV,
                IznosSaPDV = iznosSaPDV,
                IdProdavac = idProdavac,
                IdKupac = idKupac
            };

            // Act
            var (actualClause, parameters) = ugovor.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            // Provera broja parametara
            int expectedParamCount = 0;
            if (idUgovor > 0) expectedParamCount++;
            if (datum != DateTime.MinValue) expectedParamCount++;
            if (brAutomobila > 0) expectedParamCount++;
            if (pdv > 0) expectedParamCount++;
            if (iznosBezPDV > 0) expectedParamCount++;
            if (iznosSaPDV > 0) expectedParamCount++;
            if (idProdavac > 0) expectedParamCount++;
            if (idKupac > 0) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }

        [Fact]
        public void SelectColumns_ReturnsCorrectFormat()
        {
            // Arrange
            var ugovor = new Ugovor();

            // Act
            var result = ugovor.SelectColumns;

            // Assert
            Assert.Equal("u.idUgovor, u.datum, u.brAutomobila, u.pdv, u.iznosBezPDV, u.iznosSaPDV, u.idProdavac, u.idKupac", result);
        }

        [Fact]
        public void InsertColumnsAndPlaceholders_ReturnCorrectValues()
        {
            // Arrange
            var ugovor = new Ugovor();

            // Act & Assert
            Assert.Equal("datum, brAutomobila, pdv, iznosBezPDV, iznosSaPDV, idProdavac, idKupac", ugovor.InsertColumns);
            Assert.Equal("@datum, @brAutomobila, @pdv, @iznosBezPDV, @iznosSaPDV, @idProdavac, @idKupac", ugovor.InsertValuesPlaceholders);
        }
    }
}
