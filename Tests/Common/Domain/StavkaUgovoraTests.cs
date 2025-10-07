using Common.Domain;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Xunit;

namespace Tests.Common.Domain
{
    public class StavkaUgovoraTests
    {
        [Fact]
        public void Ugovor_SetWithValidValue_SetsProperty()
        {
            var stavka = new StavkaUgovora();
            var validUgovor = new Ugovor();
            stavka.Ugovor = validUgovor;
            Assert.Equal(validUgovor, stavka.Ugovor);
        }

        [Fact]
        public void Ugovor_SetWithNull_ThrowsArgumentNullException()
        {
            var stavka = new StavkaUgovora();
            Assert.Throws<ArgumentNullException>(() => stavka.Ugovor = null);
        }

        [Fact]
        public void Rb_SetWithValidValue_SetsProperty()
        {
            var stavka = new StavkaUgovora();
            const int validRb = 1;
            stavka.Rb = validRb;
            Assert.Equal(validRb, stavka.Rb);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Rb_SetWithInvalidValue_ThrowsArgumentOutOfRangeException(int invalidRb)
        {
            var stavka = new StavkaUgovora();
            Assert.Throws<ArgumentOutOfRangeException>(() => stavka.Rb = invalidRb);
        }

        [Fact]
        public void Automobil_SetWithValidValue_SetsProperty()
        {
            var stavka = new StavkaUgovora();
            var validAutomobil = new Automobil();
            stavka.Automobil = validAutomobil;
            Assert.Equal(validAutomobil, stavka.Automobil);
        }

        [Fact]
        public void Automobil_SetWithNull_ThrowsArgumentNullException()
        {
            var stavka = new StavkaUgovora();
            Assert.Throws<ArgumentNullException>(() => stavka.Automobil = null);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.1)]
        [InlineData(1)]
        public void Popust_SetWithValidValue_SetsProperty(double validPopust)
        {
            var stavka = new StavkaUgovora();
            stavka.Popust = validPopust;
            Assert.Equal(validPopust, stavka.Popust);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        public void Popust_SetWithInvalidValue_ThrowsArgumentOutOfRangeException(double invalidPopust)
        {
            var stavka = new StavkaUgovora();
            Assert.Throws<ArgumentOutOfRangeException>(() => stavka.Popust = invalidPopust);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10000)]
        public void CenaAutomobila_SetWithValidValue_SetsProperty(double validCena)
        {
            var stavka = new StavkaUgovora();
            stavka.CenaAutomobila = validCena;
            Assert.Equal(validCena, stavka.CenaAutomobila);
        }

        [Fact]
        public void CenaAutomobila_SetWithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            var stavka = new StavkaUgovora();
            Assert.Throws<ArgumentOutOfRangeException>(() => stavka.CenaAutomobila = -1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9000)]
        public void Iznos_SetWithValidValue_SetsProperty(double validIznos)
        {
            var stavka = new StavkaUgovora();
            stavka.Iznos = validIznos;
            Assert.Equal(validIznos, stavka.Iznos);
        }

        [Fact]
        public void Iznos_SetWithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            var stavka = new StavkaUgovora();
            Assert.Throws<ArgumentOutOfRangeException>(() => stavka.Iznos = -1);
        }

        [Fact]
        public void GetInsertParameters_ShouldReturnCorrectParameters()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = 1 },
                Rb = 2,
                Automobil = new Automobil { IdAutomobil = 3 },
                CenaAutomobila = 10000,
                Popust = 0.1,
                Iznos = 9500
            };

            var parameters = stavka.GetInsertParameters();

            Assert.Equal(6, parameters.Count);
            Assert.Contains(parameters, p => p.ParameterName == "@idUgovor" && (int)p.Value == 1);
            Assert.Contains(parameters, p => p.ParameterName == "@rb" && (int)p.Value == 2);
            Assert.Contains(parameters, p => p.ParameterName == "@idAutomobil" && (int)p.Value == 3);
            Assert.Contains(parameters, p => p.ParameterName == "@cenaAutomobila" && (double)p.Value == 10000);
            Assert.Contains(parameters, p => p.ParameterName == "@popust" && (double)p.Value == 0.1);
            Assert.Contains(parameters, p => p.ParameterName == "@iznos" && (double)p.Value == 9500);
        }

        [Fact]
        public void GetUpdateParameters_ShouldReturnCorrectParameters()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = 1 },
                Rb = 2,
                Automobil = new Automobil { IdAutomobil = 3 },
                CenaAutomobila = 10000,
                Popust = 0.1,
                Iznos = 9500
            };

            var parameters = stavka.GetUpdateParameters();

            Assert.Equal(6, parameters.Count);
            Assert.Equal("@idUgovor", parameters[0].ParameterName);
            Assert.Equal(1, parameters[0].Value);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ShouldReturnCorrectParameters()
        {
            var stavka = new StavkaUgovora
            {
                Ugovor = new Ugovor { IdUgovor = 1 },
                Rb = 2
            };

            var pkParams = stavka.GetPrimaryKeyParameters();

            Assert.Equal(2, pkParams.Count);
            Assert.Contains(pkParams, p => p.ParameterName == "@idUgovor" && (int)p.Value == 1);
            Assert.Contains(pkParams, p => p.ParameterName == "@rb" && (int)p.Value == 2);
        }

        [Fact]
        public void ReadEntities_ShouldMapReaderToStavkaUgovoraList()
        {
            var mockReader = new Mock<DbDataReader>();
            var sequence = new Queue<bool>(new[] { true, false });
            mockReader.Setup(r => r.Read()).Returns(() => sequence.Dequeue());

            mockReader.Setup(r => r["idUgovor"]).Returns(1);
            mockReader.Setup(r => r["rb"]).Returns(2);
            mockReader.Setup(r => r["idAutomobil"]).Returns(3);
            mockReader.Setup(r => r["cenaAutomobila"]).Returns(10000.0);
            mockReader.Setup(r => r["popust"]).Returns(500.0);
            mockReader.Setup(r => r["iznos"]).Returns(9500.0);

            var stavka = new StavkaUgovora();

            var result = stavka.ReadEntities(mockReader.Object);

            Assert.Single(result);
            var entity = Assert.IsType<StavkaUgovora>(result[0]);
            Assert.Equal(1, entity.Ugovor.IdUgovor);
            Assert.Equal(2, entity.Rb);
            Assert.Equal(3, entity.Automobil.IdAutomobil);
            Assert.Equal(10000, entity.CenaAutomobila);
            Assert.Equal(500, entity.Popust);
            Assert.Equal(9500, entity.Iznos);
        }

        [Theory]
        [InlineData(0, 0, "1=1", 0)]
        [InlineData(1, 0, "1=1 AND su.idUgovor = @idUgovor", 1)]
        [InlineData(1, 2, "1=1 AND su.idUgovor = @idUgovor AND su.rb = @rb", 2)]
        public void GetWhereClauseWithParameters_ShouldBuildCorrectClause(int idUgovor, int rb, string expectedClause, int expectedParamCount)
        {
            var stavka = new StavkaUgovora
            (
                new Ugovor { IdUgovor = idUgovor },
                rb,
                null,
                0,
                0,
                0 
            );

            var (whereClause, parameters) = stavka.GetWhereClauseWithParameters();

            Assert.Equal(expectedClause, whereClause);
            Assert.Equal(expectedParamCount, parameters.Count);
            Assert.All(parameters, p => Assert.IsType<SqlParameter>(p));
        }

    }
}
