using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Common.Domain.Tests
{
    public class AutomobilTests
    {
        [Fact]
        public void Automobil_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var automobil = new Automobil();

            // Assert
            Assert.Equal("Automobil", automobil.TableName);
            Assert.Equal("a", automobil.TableAlias);
            Assert.Equal("idAutomobil", automobil.PrimaryKeyColumn);
            Assert.Null(automobil.JoinTable);
            Assert.Null(automobil.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var automobil = new Automobil
            {
                Model = "Golf 8",
                Oprema = "Full",
                TipGoriva = "Dizel",
                Boja = "Crna",
                Cena = 25000.0
            };

            // Act
            var parameters = automobil.GetInsertParameters();

            // Assert
            Assert.Equal(5, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@model" && p.Value.ToString() == "Golf 8");
            Assert.Contains(parameters, p => p.ParameterName == "@oprema" && p.Value.ToString() == "Full");
            Assert.Contains(parameters, p => p.ParameterName == "@tipGoriva" && p.Value.ToString() == "Dizel");
            Assert.Contains(parameters, p => p.ParameterName == "@boja" && p.Value.ToString() == "Crna");
            Assert.Contains(parameters, p => p.ParameterName == "@cena" && (double)p.Value == 25000.0);
        }

        [Fact]
        public void GetUpdateParameters_IncludesPrimaryKey()
        {
            // Arrange
            var automobil = new Automobil
            {
                IdAutomobil = 1,
                Model = "Passat",
                Oprema = "Comfort",
                TipGoriva = "Benzin",
                Boja = "Bela",
                Cena = 30000.0
            };

            // Act
            var parameters = automobil.GetUpdateParameters();

            // Assert
            Assert.Equal(6, parameters.Count); // 5 insert + 1 primary key
            Assert.Contains(parameters, p => p.ParameterName == "@idAutomobil" && (int)p.Value == 1);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdParameter()
        {
            // Arrange
            var automobil = new Automobil { IdAutomobil = 5 };

            // Act
            var parameters = automobil.GetPrimaryKeyParameters();

            // Assert
            var parameter = Assert.Single(parameters);
            Assert.Equal("@idAutomobil", parameter.ParameterName);
            Assert.Equal(5, parameter.Value);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectAutomobili()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE Automobil (
                idAutomobil INTEGER PRIMARY KEY,
                model TEXT NOT NULL,
                oprema TEXT NOT NULL,
                tipGoriva TEXT NOT NULL,
                boja TEXT NOT NULL,
                cena REAL NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO Automobil (model, oprema, tipGoriva, boja, cena)
            VALUES ('Audi A4', 'Premium', 'Dizel', 'Siva', 35000),
                   ('BMW 320', 'Sport', 'Benzin', 'Plava', 40000)
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Automobil";

            using var reader = cmd.ExecuteReader();
            var automobil = new Automobil();

            // Act
            var result = automobil.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as Automobil;
            Assert.Equal(1, first.IdAutomobil);
            Assert.Equal("Audi A4", first.Model);
            Assert.Equal("Premium", first.Oprema);
            Assert.Equal("Dizel", first.TipGoriva);
            Assert.Equal("Siva", first.Boja);
            Assert.Equal(35000, first.Cena);

            var second = result[1] as Automobil;
            Assert.Equal(2, second.IdAutomobil);
            Assert.Equal("BMW 320", second.Model);
        }


        [Theory]
        [InlineData(0, null, null, null, 0, "1=1")] // Bez filtera
        [InlineData(5, null, null, null, 0, "1=1 AND a.idAutomobil = @idAutomobil")] // Samo ID
        [InlineData(0, "Golf", null, null, 0, "1=1 AND a.model LIKE @model")] // Samo model
        [InlineData(0, null, "Dizel", null, 0, "1=1 AND a.tipGoriva = @tipGoriva")] // Samo tip goriva
        [InlineData(0, null, null, "Crna", 0, "1=1 AND a.boja = @boja")] // Samo boja
        [InlineData(0, null, null, null, 20000, "1=1 AND a.cena <= @cena")] // Samo cena
        [InlineData(3, "Passat", "Benzin", "Bela", 30000,
            "1=1 AND a.idAutomobil = @idAutomobil AND a.model LIKE @model AND a.tipGoriva = @tipGoriva AND a.boja = @boja AND a.cena <= @cena")] // Svi filteri
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(
            int id, string model, string tipGoriva, string boja, double cena, string expectedClause)
        {
            // Arrange
            var automobil = new Automobil
            {
                IdAutomobil = id,
                Model = model,
                TipGoriva = tipGoriva,
                Boja = boja,
                Cena = cena
            };

            // Act
            var (actualClause, parameters) = automobil.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            // Proveravamo broj parametara
            int expectedParamCount = 0;
            if (id > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(model)) expectedParamCount++;
            if (!string.IsNullOrEmpty(tipGoriva)) expectedParamCount++;
            if (!string.IsNullOrEmpty(boja)) expectedParamCount++;
            if (cena > 0) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }

        [Fact]
        public void SelectColumns_ReturnsCorrectFormat()
        {
            // Arrange
            var automobil = new Automobil();

            // Act
            var result = automobil.SelectColumns;

            // Assert
            Assert.Equal("a.idAutomobil, a.model, a.oprema, a.tipGoriva, a.boja, a.cena", result);
        }

        [Fact]
        public void InsertColumnsAndPlaceholders_ReturnCorrectValues()
        {
            // Arrange
            var automobil = new Automobil();

            // Act & Assert
            Assert.Equal("model, oprema, tipGoriva, boja, cena", automobil.InsertColumns);
            Assert.Equal("@model, @oprema, @tipGoriva, @boja, @cena", automobil.InsertValuesPlaceholders);
        }
    }
}