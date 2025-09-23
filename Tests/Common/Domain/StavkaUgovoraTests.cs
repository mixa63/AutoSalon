using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Common.Domain.Tests
{
    public class StavkaUgovoraTests
    {
        [Fact]
        public void StavkaUgovora_Properties_InitializedCorrectly()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor(),
                Automobil = new Automobil()
            };

            Assert.Equal("StavkaUgovora", stavka.TableName);
            Assert.Equal("su", stavka.TableAlias);
            Assert.Equal("su.idUgovor, su.rb", stavka.PrimaryKeyColumn);
            Assert.Equal("Automobil a", stavka.JoinTable);
            Assert.Equal("su.idAutomobil = a.idAutomobil", stavka.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = 1 },
                Rb = 2,
                Automobil = new Automobil { IdAutomobil = 10 },
                CenaAutomobila = 35000,
                Popust = 5000,
                Iznos = 30000
            };

            var parameters = stavka.GetInsertParameters();
            Assert.Equal(5, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@rb" && (int)p.Value == 2);
            Assert.Contains(parameters, p => p.ParameterName == "@idAutomobil" && (int)p.Value == 10);
            Assert.Contains(parameters, p => p.ParameterName == "@cenaAutomobila" && (double)p.Value == 35000);
            Assert.Contains(parameters, p => p.ParameterName == "@popust" && (double)p.Value == 5000);
            Assert.Contains(parameters, p => p.ParameterName == "@iznos" && (double)p.Value == 30000);
        }

        [Fact]
        public void GetUpdateParameters_IncludesUgovorId()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = 1 },
                Rb = 2,
                Automobil = new Automobil { IdAutomobil = 10 },
                CenaAutomobila = 35000,
                Popust = 5000,
                Iznos = 30000
            };

            var parameters = stavka.GetUpdateParameters();
            Assert.Equal(6, parameters.Count); // 5 insert + idUgovor
            Assert.Contains(parameters, p => p.ParameterName == "@idUgovor" && (int)p.Value == 1);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsBothKeys()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = 1 },
                Rb = 2
            };

            var parameters = stavka.GetPrimaryKeyParameters();
            Assert.Equal(2, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idUgovor" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@rb" && (int)p.Value == 2);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectStavke()
        {
            SQLitePCL.Batteries.Init();

            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE StavkaUgovora (
                idUgovor INTEGER NOT NULL,
                rb INTEGER NOT NULL,
                idAutomobil INTEGER NOT NULL,
                cenaAutomobila REAL NOT NULL,
                popust REAL NOT NULL,
                iznos REAL NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO StavkaUgovora (idUgovor, rb, idAutomobil, cenaAutomobila, popust, iznos)
            VALUES (1, 1, 101, 35000, 0.2, 30000),
                   (1, 2, 102, 40000, 0.2, 36000)
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM StavkaUgovora";

            using var reader = cmd.ExecuteReader();
            var stavka = new StavkaUgovora();

            var result = stavka.ReadEntities(reader);
            Assert.Equal(2, result.Count);

            var first = result[0] as StavkaUgovora;
            Assert.Equal(1, first.Ugovor.IdUgovor);
            Assert.Equal(1, first.Rb);
            Assert.Equal(101, first.Automobil.IdAutomobil);
            Assert.Equal(35000, first.CenaAutomobila);
            Assert.Equal(0.2, first.Popust);
            Assert.Equal(30000, first.Iznos);

            var second = result[1] as StavkaUgovora;
            Assert.Equal(1, second.Ugovor.IdUgovor);
            Assert.Equal(2, second.Rb);
            Assert.Equal(102, second.Automobil.IdAutomobil);
            Assert.Equal(40000, second.CenaAutomobila);
            Assert.Equal(0.2, second.Popust);
            Assert.Equal(36000, second.Iznos);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, "1=1")] // Bez filtera
        [InlineData(1, 0, 0, 0, 0, 0, "1=1 AND su.idUgovor = @idUgovor")] // Samo idUgovor
        [InlineData(0, 2, 0, 0, 0, 0, "1=1 AND su.rb = @rb")] // Samo rb
        [InlineData(0, 0, 101, 0, 0, 0, "1=1 AND su.idAutomobil = @idAutomobil")] // Samo idAutomobil
        [InlineData(0, 0, 0, 35000, 0, 0, "1=1 AND su.cenaAutomobila <= @cenaAutomobila")] // Samo cenaAutomobila
        [InlineData(0, 0, 0, 0, 0.2, 0, "1=1 AND su.popust <= @popust")] // Samo popust
        [InlineData(0, 0, 0, 0, 0, 30000, "1=1 AND su.iznos <= @iznos")] // Samo iznos
        [InlineData(1, 2, 101, 35000, 5000, 30000,
            "1=1 AND su.idUgovor = @idUgovor AND su.rb = @rb AND su.idAutomobil = @idAutomobil AND su.cenaAutomobila <= @cenaAutomobila AND su.popust <= @popust AND su.iznos <= @iznos")] // Svi filteri
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(
            int idUgovor, int rb, int idAutomobil, double cena, double popust, double iznos, string expectedClause)
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = idUgovor },
                Rb = rb,
                Automobil = new Automobil { IdAutomobil = idAutomobil },
                CenaAutomobila = cena,
                Popust = popust,
                Iznos = iznos
            };

            var (actualClause, parameters) = stavka.GetWhereClauseWithParameters();
            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (idUgovor > 0) expectedParamCount++;
            if (rb > 0) expectedParamCount++;
            if (idAutomobil > 0) expectedParamCount++;
            if (cena > 0) expectedParamCount++;
            if (popust > 0) expectedParamCount++;
            if (iznos > 0) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }
    }
}
