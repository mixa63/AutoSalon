using Microsoft.Data.Sqlite;
using Xunit;

namespace Common.Domain.Tests
{
    public class PravnoLiceTests
    {
        [Fact]
        public void PravnoLice_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var pravno = new PravnoLice();

            // Assert
            Assert.Equal("PravnoLice", pravno.TableName);
            Assert.Equal("pl", pravno.TableAlias);
            Assert.Equal("pl.idKupac", pravno.PrimaryKeyColumn);
            Assert.Null(pravno.JoinTable);
            Assert.Null(pravno.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var pravno = new PravnoLice
            {
                Kupac = new Kupac { IdKupac = 1 },
                NazivFirme = "Firma DOO",
                PIB = 123456,
                MaticniBroj = "987654321"
            };

            // Act
            var parameters = pravno.GetInsertParameters();

            // Assert
            Assert.Equal(4, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@nazivFirme" && (string)p.Value == "Firma DOO");
            Assert.Contains(parameters, p => p.ParameterName == "@pib" && (int)p.Value == 123456);
            Assert.Contains(parameters, p => p.ParameterName == "@maticniBroj" && (string)p.Value == "987654321");
        }

        [Fact]
        public void GetUpdateParameters_IncludesPrimaryKey()
        {
            // Arrange
            var pravno = new PravnoLice
            {
                Kupac = new Kupac { IdKupac = 1 },
                NazivFirme = "UpdateFirma",
                PIB = 654321,
                MaticniBroj = "111222333"
            };

            // Act
            var parameters = pravno.GetUpdateParameters();

            // Assert
            Assert.Equal(4, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 1);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdParameter()
        {
            // Arrange
            var pravno = new PravnoLice
            {
                Kupac = new Kupac { IdKupac = 5 }
            };

            // Act
            var parameters = pravno.GetPrimaryKeyParameters();

            // Assert
            var parameter = Assert.Single(parameters);
            Assert.Equal("@idKupac", parameter.ParameterName);
            Assert.Equal(5, parameter.Value);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectPravnaLica()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE PravnoLice (
                idKupac INTEGER PRIMARY KEY,
                nazivFirme TEXT NOT NULL,
                pib INTEGER NOT NULL,
                maticniBroj TEXT NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO PravnoLice (idKupac, nazivFirme, pib, maticniBroj)
            VALUES (1, 'Firma1', 123, 'MB1'),
                   (2, 'Firma2', 456, 'MB2')
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM PravnoLice";

            using var reader = cmd.ExecuteReader();
            var pravno = new PravnoLice();

            // Act
            var result = pravno.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as PravnoLice;
            Assert.Equal(1, first.Kupac.IdKupac);
            Assert.Equal("Firma1", first.NazivFirme);
            Assert.Equal(123, first.PIB);
            Assert.Equal("MB1", first.MaticniBroj);

            var second = result[1] as PravnoLice;
            Assert.Equal(2, second.Kupac.IdKupac);
            Assert.Equal("Firma2", second.NazivFirme);
        }

        [Theory]
        [InlineData(0, null, 0, null, "1=1")]
        [InlineData(1, null, 0, null, "1=1 AND pl.idKupac = @idKupac")]
        [InlineData(0, "FirmaX", 0, null, "1=1 AND pl.nazivFirme LIKE @nazivFirme")]
        [InlineData(0, null, 123456, null, "1=1 AND pl.pib = @pib")]
        [InlineData(0, null, 0, "MB123", "1=1 AND pl.maticniBroj = @maticniBroj")]
        [InlineData(2, "FirmaY", 654321, "MB987", "1=1 AND pl.idKupac = @idKupac AND pl.nazivFirme LIKE @nazivFirme AND pl.pib = @pib AND pl.maticniBroj = @maticniBroj")]
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(int idKupac, string naziv, int pib, string maticni, string expectedClause)
        {
            // Arrange
            var pravno = new PravnoLice
            {
                Kupac = new Kupac { IdKupac = idKupac },
                NazivFirme = naziv,
                PIB = pib,
                MaticniBroj = maticni
            };

            // Act
            var (actualClause, parameters) = pravno.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (idKupac > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(naziv)) expectedParamCount++;
            if (pib > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(maticni)) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }
    }
}
