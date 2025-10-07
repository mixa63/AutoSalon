using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
using Common.Domain;

namespace Tests.Common.Domain;

public class KvalifikacijaTests
{

    [Fact]
    public void Naziv_SetWithValidValue_SetsProperty()
    {
        var kvalifikacija = new Kvalifikacija();
        const string validNaziv = "Napredni prodavac";
        kvalifikacija.Naziv = validNaziv;
        Assert.Equal(validNaziv, kvalifikacija.Naziv);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Naziv_SetWithInvalidValue_ThrowsArgumentException(string invalidNaziv)
    {
        var kvalifikacija = new Kvalifikacija();
        Assert.Throws<ArgumentException>(() => kvalifikacija.Naziv = invalidNaziv);
    }

    [Fact]
    public void Stepen_SetWithValidValue_SetsProperty()
    {
        var kvalifikacija = new Kvalifikacija();
        const string validStepen = "III";
        kvalifikacija.Stepen = validStepen;
        Assert.Equal(validStepen, kvalifikacija.Stepen);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Stepen_SetWithInvalidValue_ThrowsArgumentException(string invalidStepen)
    {
        var kvalifikacija = new Kvalifikacija();
        Assert.Throws<ArgumentException>(() => kvalifikacija.Stepen = invalidStepen);
    }

    [Fact]
    public void GetInsertParameters_ShouldReturnCorrectParameters()
    {
        var k = new Kvalifikacija
        {
            Naziv = "Menadžment",
            Stepen = "Napredni"
        };

        var parameters = k.GetInsertParameters();

        Assert.Equal(2, parameters.Count);
        Assert.Contains(parameters, p => p.ParameterName == "@naziv" && (string)p.Value == "Menadžment");
        Assert.Contains(parameters, p => p.ParameterName == "@stepen" && (string)p.Value == "Napredni");
    }

    [Fact]
    public void GetUpdateParameters_ShouldIncludePrimaryKey()
    {
        var k = new Kvalifikacija
        {
            IdKvalifikacija = 5,
            Naziv = "Prodaja",
            Stepen = "Srednji"
        };

        var parameters = k.GetUpdateParameters();

        Assert.Equal(3, parameters.Count);
        Assert.Contains(parameters, p => p.ParameterName == "@idKvalifikacija" && (int)p.Value == 5);
    }

    [Fact]
    public void GetPrimaryKeyParameters_ShouldReturnSingleParameter()
    {
        var k = new Kvalifikacija { IdKvalifikacija = 10 };

        var parameters = k.GetPrimaryKeyParameters();

        Assert.Single(parameters);
        Assert.Equal("@idKvalifikacija", parameters[0].ParameterName);
        Assert.Equal(10, parameters[0].Value);
    }

    [Theory]
    [InlineData(1, null, null, "k.idKvalifikacija = @idKvalifikacija", 1)]
    [InlineData(0, "IT", null, "k.naziv LIKE @naziv", 1)]
    [InlineData(0, null, "Osnovni", "k.stepen = @stepen", 1)]
    [InlineData(3, "Ekonomija", "Napredni", "k.idKvalifikacija = @idKvalifikacija AND k.naziv LIKE @naziv AND k.stepen = @stepen", 3)]
    public void GetWhereClauseWithParameters_VariousInputs_ShouldGenerateCorrectClause(
        int id, string naziv, string stepen,
        string expectedCondition, int expectedParamCount)
    {
        var k = new Kvalifikacija(id, naziv, stepen);

        var (where, parameters) = k.GetWhereClauseWithParameters();

        Assert.Contains(expectedCondition, where);
        Assert.Equal(expectedParamCount, parameters.Count);
    }

    [Fact]
    public void ReadEntities_ShouldMapFromDataReader()
    {
        var mockReader = new Mock<DbDataReader>();
        var readCount = 0;

        mockReader.Setup(r => r.Read()).Returns(() => readCount++ == 0);

        mockReader.Setup(r => r["idKvalifikacija"]).Returns(7);
        mockReader.Setup(r => r["naziv"]).Returns("Marketing");
        mockReader.Setup(r => r["stepen"]).Returns("Srednji");

        var kvalifikacija = new Kvalifikacija();

        var result = kvalifikacija.ReadEntities(mockReader.Object);

        var entity = Assert.Single(result) as Kvalifikacija;
        Assert.NotNull(entity);
        Assert.Equal(7, entity.IdKvalifikacija);
        Assert.Equal("Marketing", entity.Naziv);
        Assert.Equal("Srednji", entity.Stepen);
    }
}
