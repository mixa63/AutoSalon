using Common.Domain;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Xunit;

namespace Tests.Common.Domain
{
    public class UgovorTests
    {
        [Fact]
        public void Datum_SetWithValidValue_SetsProperty()
        {
            var ugovor = new Ugovor();
            var validDatum = DateTime.Now;
            ugovor.Datum = validDatum;
            Assert.Equal(validDatum, ugovor.Datum);
        }

        [Fact]
        public void Datum_SetWithMinValue_ThrowsArgumentOutOfRangeException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentOutOfRangeException>(() => ugovor.Datum = DateTime.MinValue);
        }

        [Fact]
        public void BrAutomobila_SetWithValidValue_SetsProperty()
        {
            var ugovor = new Ugovor();
            const int validBr = 5;
            ugovor.BrAutomobila = validBr;
            Assert.Equal(validBr, ugovor.BrAutomobila);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void BrAutomobila_SetWithInvalidValue_ThrowsArgumentOutOfRangeException(int invalidBr)
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentOutOfRangeException>(() => ugovor.BrAutomobila = invalidBr);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.2)]
        public void PDV_SetWithValidValue_SetsProperty(double validPdv)
        {
            var ugovor = new Ugovor();
            ugovor.PDV = validPdv;
            Assert.Equal(validPdv, ugovor.PDV);
        }

        [Fact]
        public void PDV_SetWithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentOutOfRangeException>(() => ugovor.PDV = -0.1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50000)]
        public void IznosBezPDV_SetWithValidValue_SetsProperty(double validIznos)
        {
            var ugovor = new Ugovor();
            ugovor.IznosBezPDV = validIznos;
            Assert.Equal(validIznos, ugovor.IznosBezPDV);
        }

        [Fact]
        public void IznosBezPDV_SetWithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentOutOfRangeException>(() => ugovor.IznosBezPDV = -1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(60000)]
        public void IznosSaPDV_SetWithValidValue_SetsProperty(double validIznos)
        {
            var ugovor = new Ugovor();
            ugovor.IznosSaPDV = validIznos;
            Assert.Equal(validIznos, ugovor.IznosSaPDV);
        }

        [Fact]
        public void IznosSaPDV_SetWithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentOutOfRangeException>(() => ugovor.IznosSaPDV = -1);
        }

        [Fact]
        public void Prodavac_SetWithValidValue_SetsProperty()
        {
            var ugovor = new Ugovor();
            var validProdavac = new Prodavac();
            ugovor.Prodavac = validProdavac;
            Assert.Equal(validProdavac, ugovor.Prodavac);
        }

        [Fact]
        public void Prodavac_SetWithNull_ThrowsArgumentNullException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentNullException>(() => ugovor.Prodavac = null);
        }

        [Fact]
        public void Kupac_SetWithValidValue_SetsProperty()
        {
            var ugovor = new Ugovor();
            var validKupac = new Kupac();
            ugovor.Kupac = validKupac;
            Assert.Equal(validKupac, ugovor.Kupac);
        }

        [Fact]
        public void Kupac_SetWithNull_ThrowsArgumentNullException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentNullException>(() => ugovor.Kupac = null);
        }

        [Fact]
        public void Stavke_SetWithValidValue_SetsProperty()
        {
            var ugovor = new Ugovor();
            var validStavke = new List<StavkaUgovora>();
            ugovor.Stavke = validStavke;
            Assert.Equal(validStavke, ugovor.Stavke);
        }

        [Fact]
        public void Stavke_SetWithNull_ThrowsArgumentNullException()
        {
            var ugovor = new Ugovor();
            Assert.Throws<ArgumentNullException>(() => ugovor.Stavke = null);
        }

        [Fact]
        public void GetInsertParameters_ShouldReturnCorrectSqlParameters()
        {
            var ugovor = new Ugovor
            {
                Datum = new DateTime(2025, 10, 3),
                BrAutomobila = 2,
                PDV = 20,
                IznosBezPDV = 1000,
                IznosSaPDV = 1200,
                Prodavac = new Prodavac { IdProdavac = 1 },
                Kupac = new Kupac { IdKupac = 2 }
            };

            var parameters = ugovor.GetInsertParameters();

            Assert.Contains(parameters, p => p.ParameterName == "@datum" && (DateTime)p.Value == ugovor.Datum);
            Assert.Contains(parameters, p => p.ParameterName == "@brAutomobila" && (int)p.Value == ugovor.BrAutomobila);
            Assert.Contains(parameters, p => p.ParameterName == "@pdv" && (double)p.Value == ugovor.PDV);
            Assert.Contains(parameters, p => p.ParameterName == "@iznosBezPDV" && (double)p.Value == ugovor.IznosBezPDV);
            Assert.Contains(parameters, p => p.ParameterName == "@iznosSaPDV" && (double)p.Value == ugovor.IznosSaPDV);
            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac" && (int)p.Value == ugovor.Prodavac.IdProdavac);
            Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == ugovor.Kupac.IdKupac);
        }

        [Fact]
        public void GetUpdateParameters_ShouldIncludeIdUgovor()
        {
            var ugovor = new Ugovor
            {
                IdUgovor = 5,
                Datum = DateTime.Today,
                BrAutomobila = 1,
                PDV = 15,
                IznosBezPDV = 500,
                IznosSaPDV = 575,
                Prodavac = new Prodavac { IdProdavac = 1 },
                Kupac = new Kupac { IdKupac = 2 }
            };

            var parameters = ugovor.GetUpdateParameters();

            Assert.Contains(parameters, p => p.ParameterName == "@idUgovor" && (int)p.Value == ugovor.IdUgovor);
        }

        [Fact]
        public void GetPrimaryKeyParameters_ShouldReturnIdUgovor()
        {
            var ugovor = new Ugovor { IdUgovor = 10 };
            var parameters = ugovor.GetPrimaryKeyParameters();
            Assert.Single(parameters);
            Assert.Equal("@idUgovor", parameters[0].ParameterName);
            Assert.Equal(ugovor.IdUgovor, parameters[0].Value);
        }

        [Theory]
        [InlineData(0, "", "", "", 0, 0, 0, 0, "1=1", 0)]
        [InlineData(1, "", "", "", 0, 0, 0, 0, "1=1 AND u.idUgovor = @idUgovor", 1)]
        [InlineData(0, "2025-10-03", "", "", 0, 0, 0, 0, "1=1 AND u.datum = @datum", 1)]
        [InlineData(0, "", "Milan", "", 0, 0, 0, 0, "1=1 AND p.ime LIKE @prodavacIme", 1)]
        [InlineData(0, "", "", "kupac@mail.com", 0, 0, 0, 0, "1=1 AND k.email LIKE @kupacEmail", 1)]
        [InlineData(0, "", "", "", 2, 0, 0, 0, "1=1 AND u.brAutomobila = @brAutomobila", 1)]
        [InlineData(0, "", "", "", 0, 20, 0, 0, "1=1 AND u.pdv <= @pdv", 1)]
        [InlineData(0, "", "", "", 0, 0, 1000, 0, "1=1 AND u.iznosBezPDV <= @iznosBezPDV", 1)]
        [InlineData(0, "", "", "", 0, 0, 0, 1200, "1=1 AND u.iznosSaPDV <= @iznosSaPDV", 1)]
        [InlineData(0, "", "", "", 0, 0, 0, 0, "1=1 AND EXISTS", 1, "Golf")]
        [InlineData(2, "", "Ana", "ana@mail.com", 1, 25, 500, 600,"1=1 AND u.idUgovor = @idUgovor AND u.brAutomobila = @brAutomobila AND u.pdv <= @pdv AND u.iznosBezPDV <= @iznosBezPDV AND u.iznosSaPDV <= @iznosSaPDV AND p.ime LIKE @prodavacIme AND k.email LIKE @kupacEmail AND EXISTS", 8, "Astra")]
        public void GetWhereClauseWithParameters_ShouldBuildCorrectWhereClause(
        int idUgovor,
        string datumStr,
        string prodavacIme,
        string kupacEmail,
        int brAutomobila,
        double pdv,
        double iznosBezPdv,
        double iznosSaPdv,
        string expectedContains,
        int expectedParamCount,
        string modelAutomobila = "")
        {
            DateTime datum = string.IsNullOrEmpty(datumStr) ? DateTime.MinValue : DateTime.Parse(datumStr);

            var ugovor = new Ugovor
            (
                idUgovor,
                datum,
                brAutomobila,
                pdv,
                iznosBezPdv,
                iznosSaPdv,
                new Prodavac(-1, prodavacIme, null, null, null),
                new Kupac(-1,kupacEmail),
                string.IsNullOrEmpty(modelAutomobila)
                    ? new List<StavkaUgovora>()
                    : new List<StavkaUgovora> { new StavkaUgovora { Automobil = new Automobil { Model = modelAutomobila } } }
            );

            var (whereClause, parameters) = ugovor.GetWhereClauseWithParameters();

            Assert.Contains(expectedContains, whereClause);
            Assert.Equal(expectedParamCount, parameters.Count);
        }


        [Fact]
        public void ReadEntities_ShouldMapReaderToUgovorObjects()
        {
            var mockReader = new Mock<DbDataReader>();
            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);
            mockReader.Setup(r => r["idUgovor"]).Returns(1);
            mockReader.Setup(r => r["datum"]).Returns(new DateTime(2025, 10, 3));
            mockReader.Setup(r => r["brAutomobila"]).Returns(2);
            mockReader.Setup(r => r["pdv"]).Returns(20.0);
            mockReader.Setup(r => r["iznosBezPDV"]).Returns(1000.0);
            mockReader.Setup(r => r["iznosSaPDV"]).Returns(1200.0);
            mockReader.Setup(r => r["idProdavac"]).Returns(1);
            mockReader.Setup(r => r["ProdavacIme"]).Returns("Marko");
            mockReader.Setup(r => r["idKupac"]).Returns(2);
            mockReader.Setup(r => r["KupacEmail"]).Returns("kupac@email.com");

            var ugovor = new Ugovor();

            var result = ugovor.ReadEntities(mockReader.Object);

            var first = Assert.Single(result) as Ugovor;
            Assert.NotNull(first);
            Assert.Equal(1, first.IdUgovor);
            Assert.Equal(2, first.BrAutomobila);
            Assert.Equal("Marko", first.Prodavac.Ime);
            Assert.Equal("kupac@email.com", first.Kupac.Email);
        }
    }
}
