using Common.Domain;
using Microsoft.Data.SqlClient;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Xunit;

namespace Tests.Common.Domain
{
    public class PravnoLiceTests
    {
        [Fact]
        public void NazivFirme_SetWithValidValue_SetsProperty()
        {
            var pravnoLice = new PravnoLice();
            const string validNaziv = "Firma d.o.o.";
            pravnoLice.NazivFirme = validNaziv;
            Assert.Equal(validNaziv, pravnoLice.NazivFirme);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NazivFirme_SetWithInvalidValue_ThrowsArgumentException(string invalidNaziv)
        {
            var pravnoLice = new PravnoLice();
            Assert.Throws<ArgumentException>(() => pravnoLice.NazivFirme = invalidNaziv);
        }

        [Fact]
        public void PIB_SetWithValidValue_SetsProperty()
        {
            var pravnoLice = new PravnoLice();
            const string validPIB = "123456789";
            pravnoLice.PIB = validPIB;
            Assert.Equal(validPIB, pravnoLice.PIB);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void PIB_SetWithInvalidValue_ThrowsArgumentException(string invalidPIB)
        {
            var pravnoLice = new PravnoLice();
            Assert.Throws<ArgumentException>(() => pravnoLice.PIB = invalidPIB);
        }

        [Fact]
        public void MaticniBroj_SetWithValidValue_SetsProperty()
        {
            var pravnoLice = new PravnoLice();
            const string validMaticniBroj = "98765432";
            pravnoLice.MaticniBroj = validMaticniBroj;
            Assert.Equal(validMaticniBroj, pravnoLice.MaticniBroj);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void MaticniBroj_SetWithInvalidValue_ThrowsArgumentException(string invalidMaticniBroj)
        {
            var pravnoLice = new PravnoLice();
            Assert.Throws<ArgumentException>(() => pravnoLice.MaticniBroj = invalidMaticniBroj);
        }

        [Fact]
        public void GetInsertParameters_ShouldReturnCorrectParameters()
        {
            var pl = new PravnoLice
            {
                IdKupac = 1,
                NazivFirme = "TestFirma",
                PIB = "123456789",
                MaticniBroj = "987654321"
            };

            var parameters = pl.GetInsertParameters();

            Assert.Equal(4, parameters.Count);

            Assert.Contains(parameters, p => p.ParameterName == "@idKupac"
                && p.SqlDbType == SqlDbType.Int
                && (int)p.Value == 1);

            Assert.Contains(parameters, p => p.ParameterName == "@nazivFirme"
                && p.SqlDbType == SqlDbType.NVarChar
                && (string)p.Value == "TestFirma");

            Assert.Contains(parameters, p => p.ParameterName == "@pib"
                && p.SqlDbType == SqlDbType.NVarChar
                && (string)p.Value == "123456789");

            Assert.Contains(parameters, p => p.ParameterName == "@maticniBroj"
                && p.SqlDbType == SqlDbType.NVarChar
                && (string)p.Value == "987654321");
        }

        [Fact]
        public void GetUpdateParameters_ShouldReturnCorrectParameters()
        {
            var pl = new PravnoLice
            {
                IdKupac = 2,
                NazivFirme = "FirmaX",
                PIB = "PIB999",
                MaticniBroj = "MB123"
            };

            var parameters = pl.GetUpdateParameters();

            Assert.Equal(4, parameters.Count);

            Assert.Contains(parameters, p => p.ParameterName == "@nazivFirme"
                && p.SqlDbType == SqlDbType.NVarChar
                && (string)p.Value == "FirmaX");

            Assert.Contains(parameters, p => p.ParameterName == "@pib"
                && p.SqlDbType == SqlDbType.NVarChar
                && (string)p.Value == "PIB999");

            Assert.Contains(parameters, p => p.ParameterName == "@maticniBroj"
                && p.SqlDbType == SqlDbType.NVarChar
                && (string)p.Value == "MB123");

            Assert.Contains(parameters, p => p.ParameterName == "@idKupac"
                && p.SqlDbType == SqlDbType.Int
                && (int)p.Value == 2);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ShouldReturnIdKupacParameter()
        {
            var pl = new PravnoLice { IdKupac = 5 };

            var parameters = pl.GetPrimaryKeyParameters();

            Assert.Single(parameters);
            Assert.Equal("@idKupac", parameters[0].ParameterName);
            Assert.Equal(SqlDbType.Int, parameters[0].SqlDbType);
            Assert.Equal(5, parameters[0].Value);
        }

        [Fact]
        public void ReadEntities_ShouldReturnListOfPravnoLice()
        {
            var mockReader = new Mock<DbDataReader>();
            var sequence = new Queue<bool>(new[] { true, false });
            mockReader.Setup(r => r.Read()).Returns(() => sequence.Dequeue());

            mockReader.Setup(r => r["idKupac"]).Returns(10);
            mockReader.Setup(r => r["email"]).Returns("firma@mail.com");
            mockReader.Setup(r => r["nazivFirme"]).Returns("FirmaTest");
            mockReader.Setup(r => r["pib"]).Returns("PIB123");
            mockReader.Setup(r => r["maticniBroj"]).Returns("MB999");

            var pl = new PravnoLice();

            var result = pl.ReadEntities(mockReader.Object);

            Assert.Single(result);
            var entity = Assert.IsType<PravnoLice>(result[0]);
            Assert.Equal(10, entity.IdKupac);
            Assert.Equal("firma@mail.com", entity.Email);
            Assert.Equal("FirmaTest", entity.NazivFirme);
            Assert.Equal("PIB123", entity.PIB);
            Assert.Equal("MB999", entity.MaticniBroj);
        }

        [Theory]
        [InlineData(0, "FirmaTest", "PIB123", "MB999", "1=1 AND pl.nazivFirme LIKE @nazivFirme AND pl.pib = @pib AND pl.maticniBroj = @maticniBroj", 3)]
        [InlineData(5, "", "", "", "1=1 AND pl.idKupac = @idKupac", 1)]
        [InlineData(7, "FirmaY", "", "", "1=1 AND pl.idKupac = @idKupac AND pl.nazivFirme LIKE @nazivFirme", 2)]
        [InlineData(0, "", "PIB555", "", "1=1 AND pl.pib = @pib", 1)]
        [InlineData(0, "", "", "MB777", "1=1 AND pl.maticniBroj = @maticniBroj", 1)]
        [InlineData(0, "", "", "", "1=1", 0)]
        public void GetWhereClauseWithParameters_ShouldBuildCorrectClause(int idKupac, string nazivFirme, string pib, string maticniBroj, string expectedClause, int expectedParamCount)
        {
            var pl = new PravnoLice(idKupac, null, nazivFirme, pib, maticniBroj);

            var (whereClause, parameters) = pl.GetWhereClauseWithParameters();
             
            Assert.Equal(expectedClause, whereClause);
            Assert.Equal(expectedParamCount, parameters.Count);
        }

    }
}
