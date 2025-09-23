using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Xunit;

namespace Common.Domain.Tests
{
    public class KupacTests
    {
        [Fact]
        public void Kupac_Properties_InitializedCorrectly()
        {
            var kupac = new Kupac();
            Assert.Equal("Kupac", kupac.TableName);
            Assert.Equal("k", kupac.TableAlias);
            Assert.Equal("k.idKupac", kupac.PrimaryKeyColumn);
        }

        [Fact]
        public void GetInsertParameters_ReturnsCorrectParameters()
        {
            var kupac = new Kupac { Email = "test@mail.com" };
            var parameters = kupac.GetInsertParameters();

            Assert.Single(parameters);
            Assert.Contains(parameters, p => p.ParameterName == "@email" && (string)p.Value == "test@mail.com");
        }

        [Fact]
        public void GetUpdateParameters_ReturnsCorrectParameters()
        {
            var kupac = new Kupac { IdKupac = 1, Email = "update@mail.com" };
            var parameters = kupac.GetUpdateParameters();

            Assert.Equal(2, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@email" && (string)p.Value == "update@mail.com");
        }

        [Fact]
        public void GetPrimaryKeyParameters_ReturnsIdKupac()
        {
            var kupac = new Kupac { IdKupac = 1 };
            var parameters = kupac.GetPrimaryKeyParameters();

            Assert.Single(parameters);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 1);
        }

        [Fact]
        public void ReadEntities_WithSQLiteInMemory_ReturnsKupci()
        {
            SQLitePCL.Batteries.Init();

            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText =
            @"
            CREATE TABLE Kupac (
                idKupac INTEGER NOT NULL,
                email TEXT NOT NULL
            )";
            createTableCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"INSERT INTO Kupac (idKupac, email) VALUES (1, 'kupac1@mail.com'), (2, 'kupac2@mail.com')";
            insertCmd.ExecuteNonQuery();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Kupac";

            using var reader = cmd.ExecuteReader();
            var result = new Kupac().ReadEntities(reader);

            Assert.Equal(2, result.Count);

            var first = result[0] as Kupac;
            Assert.Equal(1, first.IdKupac);
            Assert.Equal("kupac1@mail.com", first.Email);

            var second = result[1] as Kupac;
            Assert.Equal(2, second.IdKupac);
            Assert.Equal("kupac2@mail.com", second.Email);
        }

        [Theory]
        [InlineData(0, null, "1=1")]
        [InlineData(1, null, "1=1 AND k.idKupac = @idKupac")]
        [InlineData(0, "kupac@mail.com", "1=1 AND k.email = @email")]
        [InlineData(5, "kupac@mail.com", "1=1 AND k.idKupac = @idKupac AND k.email = @email")]
        public void GetWhereClauseWithParameters_ReturnsCorrectClause(int idKupac, string email, string expectedClause)
        {
            var kupac = new Kupac { IdKupac = idKupac, Email = email };
            var (actualClause, parameters) = kupac.GetWhereClauseWithParameters();

            Assert.Equal(expectedClause, actualClause);

            int expectedParamCount = 0;
            if (idKupac > 0) expectedParamCount++;
            if (!string.IsNullOrEmpty(email)) expectedParamCount++;
            Assert.Equal(expectedParamCount, parameters.Count);
        }
    }
}
