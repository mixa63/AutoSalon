using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
using Common.Domain;

namespace Tests.Common.Domain
{
    public class KupacTests
    {
        [Fact]
        public void GetInsertParameters_ShouldReturnCorrectParameters()
        {
            var kupac = new Kupac { Email = "pera@example.com" };

            var parameters = kupac.GetInsertParameters();

            Assert.Single(parameters);
            Assert.Equal("@email", parameters[0].ParameterName);
            Assert.Equal("pera@example.com", parameters[0].Value);
        }

        [Fact]
        public void GetUpdateParameters_ShouldIncludePrimaryKey()
        {
            var kupac = new Kupac { IdKupac = 10, Email = "mika@example.com" };

            var parameters = kupac.GetUpdateParameters();

            Assert.Equal(2, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 10);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ShouldReturnSingleParameter()
        {
            var kupac = new Kupac { IdKupac = 5 };

            var parameters = kupac.GetPrimaryKeyParameters();

            Assert.Single(parameters);
            Assert.Equal("@idKupac", parameters[0].ParameterName);
            Assert.Equal(5, parameters[0].Value);
        }

        [Theory]
        [InlineData(1, null, "k.idKupac = @idKupac", 1)]
        [InlineData(0, "test@mail.com", "k.email LIKE @email", 1)]
        [InlineData(2, "some@mail.com", "k.idKupac = @idKupac AND k.email LIKE @email", 2)]
        public void GetWhereClauseWithParameters_VariousInputs_ShouldGenerateCorrectClause(
            int idKupac, string email, string expectedCondition, int expectedParamCount)
        {
            var kupac = new Kupac
            {
                IdKupac = idKupac,
                Email = email
            };

            var (where, parameters) = kupac.GetWhereClauseWithParameters();

            Assert.Contains(expectedCondition, where);
            Assert.Equal(expectedParamCount, parameters.Count);
        }

        [Fact]
        public void ReadEntities_ShouldMapFromDataReader()
        {
            var mockReader = new Mock<DbDataReader>();
            var readCount = 0;

            mockReader.Setup(r => r.Read()).Returns(() => readCount++ == 0);

            mockReader.Setup(r => r["idKupac"]).Returns(7);
            mockReader.Setup(r => r["email"]).Returns("kupac@example.com");

            var kupac = new Kupac();

            var result = kupac.ReadEntities(mockReader.Object);

            var entity = Assert.Single(result) as Kupac;
            Assert.NotNull(entity);
            Assert.Equal(7, entity.IdKupac);
            Assert.Equal("kupac@example.com", entity.Email);
        }
    }
}
