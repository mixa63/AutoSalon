using System;
using System.Collections.Generic;
using System.Data.Common;
using Moq;
using Common.Domain;

namespace Tests.Common.Domain;

public class AutomobilTests
{
    [Fact]
    public void GetInsertParameters_ShouldReturnCorrectParameters()
    {
        var auto = new Automobil
        {
            Model = "Audi A4",
            Oprema = "Full",
            TipGoriva = "Benzin",
            Boja = "Crna",
            Cena = 25000
        };

        var parameters = auto.GetInsertParameters();

        Assert.Equal(5, parameters.Count);
        Assert.Contains(parameters, p => p.ParameterName == "@model" && (string)p.Value == "Audi A4");
        Assert.Contains(parameters, p => p.ParameterName == "@tipGoriva" && (string)p.Value == "Benzin");
        Assert.Contains(parameters, p => p.ParameterName == "@oprema" && (string)p.Value == "Full");
        Assert.Contains(parameters, p => p.ParameterName == "@boja" && (string)p.Value == "Crna");
        Assert.Contains(parameters, p => p.ParameterName == "@cena" && (double)p.Value == 25000);
    }

    [Fact]
    public void GetUpdateParameters_ShouldIncludePrimaryKey()
    {
        var auto = new Automobil
        {
            IdAutomobil = 10,
            Model = "BMW",
            Oprema = "Sport",
            TipGoriva = "Dizel",
            Boja = "Plava",
            Cena = 30000
        };

        var parameters = auto.GetUpdateParameters();

        Assert.Equal(6, parameters.Count);
        Assert.Contains(parameters, p => p.ParameterName == "@idAutomobil" && (int)p.Value == 10);
    }

    [Fact]
    public void GetPrimaryKeyParameters_ShouldReturnSingleParameter()
    {
        var auto = new Automobil { IdAutomobil = 5 };

        var parameters = auto.GetPrimaryKeyParameters();

        Assert.Single(parameters);
        Assert.Equal("@idAutomobil", parameters[0].ParameterName);
        Assert.Equal(5, parameters[0].Value);
    }

    [Theory]
    [InlineData(1, null, null, null, 0, "idAutomobil = @idAutomobil", 1)]
    [InlineData(0, "Golf", null, null, 0, "model LIKE @model", 1)]
    [InlineData(0, null, "Dizel", null, 0, "tipGoriva = @tipGoriva", 1)]
    [InlineData(0, null, null, "Crvena", 0, "boja = @boja", 1)]
    [InlineData(0, null, null, null, 20000, "cena <= @cena", 1)]
    [InlineData(0, null, "Benzin", null, 10000, "a.tipGoriva = @tipGoriva AND a.cena <= @cena", 2)]
    public void GetWhereClauseWithParameters_VariousInputs_ShouldGenerateCorrectClause(
        int id, string model, string tipGoriva, string boja, double cena,
        string expectedCondition, int expectedParamCount)
    {
        var auto = new Automobil
        {
            IdAutomobil = id,
            Model = model,
            TipGoriva = tipGoriva,
            Boja = boja,
            Cena = cena
        };

        var (where, parameters) = auto.GetWhereClauseWithParameters();

        Assert.Contains(expectedCondition, where);
        Assert.Equal(expectedParamCount, parameters.Count);
    }

    [Fact]
    public void ReadEntities_ShouldMapFromDataReader()
    {
        var mockReader = new Mock<DbDataReader>();
        var readCount = 0;

        mockReader.Setup(r => r.Read()).Returns(() => readCount++ == 0);

        mockReader.Setup(r => r["idAutomobil"]).Returns(1);
        mockReader.Setup(r => r["model"]).Returns("Tesla Model S");
        mockReader.Setup(r => r["oprema"]).Returns("Premium");
        mockReader.Setup(r => r["tipGoriva"]).Returns("Elektricni");
        mockReader.Setup(r => r["boja"]).Returns("Bela");
        mockReader.Setup(r => r["cena"]).Returns(80000);

        var auto = new Automobil();

        var result = auto.ReadEntities(mockReader.Object);

        var entity = Assert.Single(result) as Automobil;
        Assert.NotNull(entity);
        Assert.Equal(1, entity.IdAutomobil);
        Assert.Equal("Tesla Model S", entity.Model);
        Assert.Equal("Premium", entity.Oprema);
        Assert.Equal("Elektricni", entity.TipGoriva);
        Assert.Equal("Bela", entity.Boja);
        Assert.Equal(80000, entity.Cena);
    }
}
