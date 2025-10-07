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
    public class PrKvalifikacijaTests
    {
        [Fact]
        public void Prodavac_SetWithValidValue_SetsProperty()
        {
            var prKvalifikacija = new PrKvalifikacija();
            var validProdavac = new Prodavac();
            prKvalifikacija.Prodavac = validProdavac;
            Assert.Equal(validProdavac, prKvalifikacija.Prodavac);
        }

        [Fact]
        public void Prodavac_SetWithNull_ThrowsArgumentNullException()
        {
            var prKvalifikacija = new PrKvalifikacija();
            Assert.Throws<ArgumentNullException>(() => prKvalifikacija.Prodavac = null);
        }

        [Fact]
        public void Kvalifikacija_SetWithValidValue_SetsProperty()
        {
            var prKvalifikacija = new PrKvalifikacija();
            var validKvalifikacija = new Kvalifikacija();
            prKvalifikacija.Kvalifikacija = validKvalifikacija;
            Assert.Equal(validKvalifikacija, prKvalifikacija.Kvalifikacija);
        }

        [Fact]
        public void Kvalifikacija_SetWithNull_ThrowsArgumentNullException()
        {
            var prKvalifikacija = new PrKvalifikacija();
            Assert.Throws<ArgumentNullException>(() => prKvalifikacija.Kvalifikacija = null);
        }

        [Fact]
        public void DatumIzdavanja_SetWithValidValue_SetsProperty()
        {
            var prKvalifikacija = new PrKvalifikacija();
            var validDatum = DateTime.Now;
            prKvalifikacija.DatumIzdavanja = validDatum;
            Assert.Equal(validDatum, prKvalifikacija.DatumIzdavanja);
        }

        [Fact]
        public void DatumIzdavanja_SetWithMinValue_ThrowsArgumentOutOfRangeException()
        {
            var prKvalifikacija = new PrKvalifikacija();
            Assert.Throws<ArgumentOutOfRangeException>(() => prKvalifikacija.DatumIzdavanja = DateTime.MinValue);
        }

        [Fact]
        public void GetInsertParameters_ShouldReturnCorrectParameters()
        {
            var prk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = 1 },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = 2 },
                DatumIzdavanja = new DateTime(2023, 5, 1)
            };

            var parameters = prk.GetInsertParameters();

            Assert.Equal(3, parameters.Count);

            Assert.Contains(parameters, p => p.ParameterName == "@idProdavac"
                && p.SqlDbType == SqlDbType.Int
                && (int)p.Value == 1);

            Assert.Contains(parameters, p => p.ParameterName == "@idKvalifikacija"
                && p.SqlDbType == SqlDbType.Int
                && (int)p.Value == 2);

            Assert.Contains(parameters, p => p.ParameterName == "@datumIzdavanja"
                && p.SqlDbType == SqlDbType.Date
                && (DateTime)p.Value == new DateTime(2023, 5, 1));
        }

        [Fact]
        public void GetUpdateParameters_ShouldReturnSameAsInsertParameters()
        {
            var prk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = 3 },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = 4 },
                DatumIzdavanja = new DateTime(2024, 1, 1)
            };

            var updateParams = prk.GetUpdateParameters();
            var insertParams = prk.GetInsertParameters();

            Assert.Equal(insertParams.Count, updateParams.Count);
            for (int i = 0; i < insertParams.Count; i++)
            {
                Assert.Equal(insertParams[i].ParameterName, updateParams[i].ParameterName);
                Assert.Equal(insertParams[i].Value, updateParams[i].Value);
            }
        }

        [Fact]
        public void GetPrimaryKeyParameters_ShouldReturnCorrectPKParams()
        {
            var prk = new PrKvalifikacija
            {
                Prodavac = new Prodavac { IdProdavac = 10 },
                Kvalifikacija = new Kvalifikacija { IdKvalifikacija = 20 }
            };

            var pkParams = prk.GetPrimaryKeyParameters();

            Assert.Equal(2, pkParams.Count);

            Assert.Contains(pkParams, p => p.ParameterName == "@idProdavac"
                && (int)p.Value == 10);

            Assert.Contains(pkParams, p => p.ParameterName == "@idKvalifikacija"
                && (int)p.Value == 20);
        }

        [Fact]
        public void ReadEntities_ShouldMapReaderToEntities()
        {
            var mockReader = new Mock<DbDataReader>();
            var sequence = new Queue<bool>(new[] { true, false });
            mockReader.Setup(r => r.Read()).Returns(() => sequence.Dequeue());

            mockReader.Setup(r => r["idProdavac"]).Returns(5);
            mockReader.Setup(r => r["idKvalifikacija"]).Returns(6);
            mockReader.Setup(r => r["datumIzdavanja"]).Returns(new DateTime(2022, 12, 31));

            var prk = new PrKvalifikacija();

            var result = prk.ReadEntities(mockReader.Object);

            Assert.Single(result);
            var entity = Assert.IsType<PrKvalifikacija>(result[0]);
            Assert.Equal(5, entity.Prodavac.IdProdavac);
            Assert.Equal(6, entity.Kvalifikacija.IdKvalifikacija);
            Assert.Equal(new DateTime(2022, 12, 31), entity.DatumIzdavanja);
        }

        [Theory]
        [InlineData(1, 2, "2023-05-01", "1=1 AND pk.idProdavac = @idProdavac AND pk.idKvalifikacija = @idKvalifikacija AND pk.datumIzdavanja = @datumIzdavanja", 3)]
        [InlineData(0, 2, "0001-01-01", "1=1 AND pk.idKvalifikacija = @idKvalifikacija", 1)]
        [InlineData(1, 0, "0001-01-01", "1=1 AND pk.idProdavac = @idProdavac", 1)]
        [InlineData(0, 0, "2024-01-01", "1=1 AND pk.datumIzdavanja = @datumIzdavanja", 1)]
        [InlineData(0, 0, "0001-01-01", "1=1", 0)]
        public void GetWhereClauseWithParameters_ShouldBuildCorrectClause(int idProdavac, int idKvalifikacija, string datum, string expectedClause, int expectedParamCount)
        {
            var prk = new PrKvalifikacija
            (
                new Prodavac(idProdavac,null,null,null,null),
                new Kvalifikacija(idKvalifikacija, null, null),
                DateTime.Parse(datum)
            );

            var (whereClause, parameters) = prk.GetWhereClauseWithParameters();

            Assert.Equal(expectedClause, whereClause);
            Assert.Equal(expectedParamCount, parameters.Count);
        }

    }
}
