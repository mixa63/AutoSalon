using Common.Domain;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Xunit;

namespace Tests.Common.Domain
{
    public class ProdavacTests
    {
        [Fact]
        public void GetInsertParameters_ShouldReturnCorrectParameters()
        {
            var prodavac = new Prodavac
            {
                Ime = "Marko",
                Prezime = "Markovic",
                Username = "marko123",
                Password = "pass123"
            };

            var parameters = prodavac.GetInsertParameters();

            Assert.Equal(4, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@ime" && (string)p.Value == "Marko" && p.SqlDbType == SqlDbType.NVarChar);
            Assert.Contains(parameters, p => p.ParameterName == "@prezime" && (string)p.Value == "Markovic" && p.SqlDbType == SqlDbType.NVarChar);
            Assert.Contains(parameters, p => p.ParameterName == "@username" && (string)p.Value == "marko123" && p.SqlDbType == SqlDbType.NVarChar);
            Assert.Contains(parameters, p => p.ParameterName == "@password" && (string)p.Value == "pass123" && p.SqlDbType == SqlDbType.NVarChar);
        }

        [Fact]
        public void GetUpdateParameters_ShouldIncludeIdProdavac()
        {
            var prodavac = new Prodavac
            {
                IdProdavac = 5,
                Ime = "Ana",
                Prezime = "Anic",
                Username = "ana123",
                Password = "pwd123"
            };

            var parameters = prodavac.GetUpdateParameters();

            Assert.Equal(5, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac" && (int)p.Value == 5 && p.SqlDbType == SqlDbType.Int);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ShouldReturnIdProdavacParameter()
        {
            var prodavac = new Prodavac { IdProdavac = 10 };

            var pkParams = prodavac.GetPrimaryKeyParameters();

            Assert.Single(pkParams);
            Assert.Equal("@idProdavac", pkParams[0].ParameterName);
            Assert.Equal(10, pkParams[0].Value);
            Assert.Equal(SqlDbType.Int, pkParams[0].SqlDbType);
        }

        [Fact]
        public void ReadEntities_ShouldMapReaderToProdavacList()
        {
            var mockReader = new Mock<DbDataReader>();
            var sequence = new Queue<bool>(new[] { true, false });
            mockReader.Setup(r => r.Read()).Returns(() => sequence.Dequeue());

            mockReader.Setup(r => r["idProdavac"]).Returns(1);
            mockReader.Setup(r => r["ime"]).Returns("Petar");
            mockReader.Setup(r => r["prezime"]).Returns("Petrovic");
            mockReader.Setup(r => r["username"]).Returns("petar123");
            mockReader.Setup(r => r["password"]).Returns("pwdPetar");

            var prodavac = new Prodavac();

            var result = prodavac.ReadEntities(mockReader.Object);

            Assert.Single(result);
            var entity = Assert.IsType<Prodavac>(result[0]);
            Assert.Equal(1, entity.IdProdavac);
            Assert.Equal("Petar", entity.Ime);
            Assert.Equal("Petrovic", entity.Prezime);
            Assert.Equal("petar123", entity.Username);
            Assert.Equal("pwdPetar", entity.Password);
        }

        [Theory]
        [InlineData(0, "Mika", "", "", "", "1=1 AND p.ime LIKE @ime", 1)]
        [InlineData(1, "", "Mikic", "", "", "1=1 AND p.idProdavac = @idProdavac AND p.prezime LIKE @prezime", 2)]
        [InlineData(2, "Ana", "Anic", "ana123", "pwd123", "1=1 AND p.idProdavac = @idProdavac AND p.ime LIKE @ime AND p.prezime LIKE @prezime AND p.username = @username AND p.password = @password", 5)]
        [InlineData(3, "", "", "userX", "", "1=1 AND p.idProdavac = @idProdavac AND p.username = @username", 2)]
        [InlineData(0, "", "", "", "", "1=1", 0)]
        public void GetWhereClauseWithParameters_ShouldBuildCorrectClause(int idProdavac, string ime, string prezime, string username, string password, string expectedClause, int expectedParamCount)
        {
            var prodavac = new Prodavac
            {
                IdProdavac = idProdavac,
                Ime = ime,
                Prezime = prezime,
                Username = username,
                Password = password
            };

            var (whereClause, parameters) = prodavac.GetWhereClauseWithParameters();

            Assert.Equal(expectedClause, whereClause);
            Assert.Equal(expectedParamCount, parameters.Count);
        }

    }
}
