using Microsoft.Data.Sqlite;
using Xunit;

namespace Common.Domain.Tests
{
    public class FizickoLiceTests
    {
        [Fact]
        public void FizickoLice_Properties_InitializedCorrectly()
        {
            // Arrange & Act
            var fizicko = new FizickoLice();

            // Assert
            Assert.Equal("FizickoLice", fizicko.TableName);
            Assert.Equal("fl", fizicko.TableAlias);
            Assert.Equal("fl.idKupac", fizicko.PrimaryKeyColumn);
            Assert.Null(fizicko.JoinTable);
            Assert.Null(fizicko.JoinCondition);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            // Arrange
            var fizicko = new FizickoLice
            {
                Kupac = new Kupac { IdKupac = 1 },
                Ime = "Petar",
                Prezime = "Petrovic",
                Telefon = "061111111",
                JMBG = "1234567890123"
            };

            // Act
            var parameters = fizicko.GetInsertParameters();

            // Assert
            Assert.Equal(5, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@ime" && (string)p.Value == "Petar");
            Assert.Contains(parameters, p => p.ParameterName == "@prezime" && (string)p.Value == "Petrovic");
            Assert.Contains(parameters, p => p.ParameterName == "@telefon" && (string)p.Value == "061111111");
            Assert.Contains(parameters, p => p.ParameterName == "@jmbg" && (string)p.Value == "1234567890123");
        }

        [Fact]
        public void GetUpdateParameters_IncludesPrimaryKey()
        {
            // Arrange
            var fizicko = new FizickoLice
            {
                Kupac = new Kupac { IdKupac = 1 },
                Ime = "Marko",
                Prezime = "Markovic",
                Telefon = "062222222",
                JMBG = "9876543210123"
            };

            // Act
            var parameters = fizicko.GetUpdateParameters();

            // Assert
            Assert.Equal(5, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 1);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdParameter()
        {
            // Arrange
            var fizicko = new FizickoLice
            {
                Kupac = new Kupac { IdKupac = 7 }
            };

            // Act
            var parameters = fizicko.GetPrimaryKeyParameters();

            // Assert
            var parameter = Assert.Single(parameters);
            Assert.Equal("@idKupac", parameter.ParameterName);
            Assert.Equal(7, parameter.Value);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsCorrectFizickaLica()
        {
            // Arrange
            SQLitePCL.Batteries.Init();
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE FizickoLice (
                idKupac INTEGER PRIMARY KEY,
                ime TEXT NOT NULL,
                prezime TEXT NOT NULL,
                telefon TEXT NOT NULL,
                jmbg TEXT NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO FizickoLice (idKupac, ime, prezime, telefon, jmbg)
            VALUES (1, 'Petar', 'Petrovic', '061111111', '1111111111111'),
                   (2, 'Jovan', 'Jovic', '062222222', '2222222222222')
            ";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM FizickoLice";

            using var reader = cmd.ExecuteReader();
            var fizicko = new FizickoLice();

            // Act
            var result = fizicko.ReadEntities(reader);

            // Assert
            Assert.Equal(2, result.Count);

            var first = result[0] as FizickoLice;
            Assert.Equal(1, first.Kupac.IdKupac);
            Assert.Equal("Petar", first.Ime);
            Assert.Equal("Petrovic", first.Prezime);
            Assert.Equal("061111111", first.Telefon);
            Assert.Equal("1111111111111", first.JMBG);

            var second = result[1] as FizickoLice;
            Assert.Equal(2, second.Kupac.IdKupac);
            Assert.Equal("Jovan", second.Ime);
        }

        [Theory]
        [InlineData(0, null, null, null, null, "1=1")]
        [InlineData(1, null, null, null, null, "1=1 AND fl.idKupac = @idKupac")]
        [InlineData(0, "Petar", null, null, null, "1=1 AND fl.ime LIKE @ime")]
        [InlineData(0, null, "Markovic", null, null, "1=1 AND fl.prezime LIKE @prezime")]
        [InlineData(0, null, null, "061111111", null, "1=1 AND fl.telefon = @telefon")]
        [InlineData(0, null, null, null, "1234567890123", "1=1 AND fl.jmbg = @jmbg")]
        [InlineData(3, "Jovan", "Jovic", "062222222", "2222222222222", "1=1 AND fl.idKupac = @idKupac AND fl.ime LIKE @ime AND fl.prezime LIKE @prezime AND fl.telefon = @telefon AND fl.jmbg = @jmbg")]
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(int idKupac, string ime, string prezime, string telefon, string jmbg, string expectedClause)
        {
            // Arrange
            var fizicko = new FizickoLice
            {
                Kupac = new Kupac { IdKupac = idKupac },
                Ime = ime,
                Prezime = prezime,
                Telefon = telefon,
                JMBG = jmbg
            };

            // Act
            var (actualClause, parameters) = fizicko.GetWhereClauseWithParameters();

            // Assert
            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (idKupac > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(ime)) expectedParamCount++;
            if (!string.IsNullOrEmpty(prezime)) expectedParamCount++;
            if (!string.IsNullOrEmpty(telefon)) expectedParamCount++;
            if (!string.IsNullOrEmpty(jmbg)) expectedParamCount++;

            Assert.Equal(expectedParamCount, parameters.Count);
        }
    }
}
