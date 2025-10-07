using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Moq;
using Common.Domain;

namespace Tests.Common.Domain;

public class FizickoLiceTests
{
    [Fact]
    public void Ime_SetWithValidValue_SetsProperty()
    {
        var fizickoLice = new FizickoLice();
        const string validIme = "Pera";
        fizickoLice.Ime = validIme;
        Assert.Equal(validIme, fizickoLice.Ime);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Ime_SetWithInvalidValue_ThrowsArgumentException(string invalidIme)
    {
        var fizickoLice = new FizickoLice();
        Assert.Throws<ArgumentException>(() => fizickoLice.Ime = invalidIme);
    }

    [Fact]
    public void Prezime_SetWithValidValue_SetsProperty()
    {
        var fizickoLice = new FizickoLice();
        const string validPrezime = "Peric";
        fizickoLice.Prezime = validPrezime;
        Assert.Equal(validPrezime, fizickoLice.Prezime);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Prezime_SetWithInvalidValue_ThrowsArgumentException(string invalidPrezime)
    {
        var fizickoLice = new FizickoLice();
        Assert.Throws<ArgumentException>(() => fizickoLice.Prezime = invalidPrezime);
    }

    [Fact]
    public void Telefon_SetWithValidValue_SetsProperty()
    {
        var fizickoLice = new FizickoLice();
        const string validTelefon = "064123456";
        fizickoLice.Telefon = validTelefon;
        Assert.Equal(validTelefon, fizickoLice.Telefon);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Telefon_SetWithInvalidValue_ThrowsArgumentException(string invalidTelefon)
    {
        var fizickoLice = new FizickoLice();
        Assert.Throws<ArgumentException>(() => fizickoLice.Telefon = invalidTelefon);
    }

    [Fact]
    public void JMBG_SetWithValidValue_SetsProperty()
    {
        var fizickoLice = new FizickoLice();
        const string validJMBG = "1234567890123";
        fizickoLice.JMBG = validJMBG;
        Assert.Equal(validJMBG, fizickoLice.JMBG);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789012")]
    [InlineData("12345678901234")]
    [InlineData("123456789012a")]
    public void JMBG_SetWithInvalidValue_ThrowsArgumentException(string invalidJMBG)
    {
        var fizickoLice = new FizickoLice();
        Assert.Throws<ArgumentException>(() => fizickoLice.JMBG = invalidJMBG);
    }

    [Fact]
    public void GetInsertParameters_ShouldReturnCorrectParameters()
    {
        var fl = new FizickoLice
        {
            IdKupac = 1,
            Ime = "Petar",
            Prezime = "Petrovic",
            Telefon = "061234567",
            JMBG = "1234567890123"
        };

        var parameters = fl.GetInsertParameters();

        Assert.Equal(5, parameters.Count);
        Assert.Contains(parameters, p => p.ParameterName == "@ime" && (string)p.Value == "Petar");
        Assert.Contains(parameters, p => p.ParameterName == "@jmbg" && (string)p.Value == "1234567890123");
        Assert.Contains(parameters, p => p.ParameterName == "@prezime" && (string)p.Value == "Petrovic");
        Assert.Contains(parameters, p => p.ParameterName == "@telefon" && (string)p.Value == "061234567");
    }

    [Fact]
    public void GetUpdateParameters_ShouldIncludePrimaryKey()
    {
        var fl = new FizickoLice
        {
            IdKupac = 2,
            Ime = "Milan",
            Prezime = "Miletic",
            Telefon = "064111222",
            JMBG = "9876543210987"
        };

        var parameters = fl.GetUpdateParameters();

        Assert.Equal(5, parameters.Count);
        Assert.Contains(parameters, p => p.ParameterName == "@idKupac" && (int)p.Value == 2);
    }

    [Fact]
    public void GetPrimaryKeyParameters_ShouldReturnSingleParameter()
    {
        var fl = new FizickoLice { IdKupac = 3 };

        var parameters = fl.GetPrimaryKeyParameters();

        Assert.Single(parameters);
        Assert.Equal("@idKupac", parameters[0].ParameterName);
        Assert.Equal(3, parameters[0].Value);
    }

    [Theory]
    [InlineData(1, null, null, null, null, "fl.idKupac = @idKupac", 1)]
    [InlineData(0, "Jovan", null, null, null, "fl.ime LIKE @ime", 1)]
    [InlineData(0, null, "Markovic", null, null, "fl.prezime LIKE @prezime", 1)]
    [InlineData(0, null, null, "060123456", null, "fl.telefon = @telefon", 1)]
    [InlineData(0, null, "Markovic", null, "1234567890000", "fl.prezime LIKE @prezime AND fl.jmbg = @jmbg", 2)]
    public void GetWhereClauseWithParameters_VariousInputs_ShouldGenerateCorrectClause(
        int id, string ime, string prezime, string telefon, string jmbg,
        string expectedCondition, int expectedParamCount)
    {
        var fl = new FizickoLice(id, null, ime, prezime, telefon, jmbg);
        

        var (where, parameters) = fl.GetWhereClauseWithParameters();

        Assert.Contains(expectedCondition, where);
        Assert.Equal(expectedParamCount, parameters.Count);
    }

    [Fact]
    public void ReadEntities_ShouldMapFromDataReader()
    {
        var mockReader = new Mock<DbDataReader>();
        var readCount = 0;

        mockReader.Setup(r => r.Read()).Returns(() => readCount++ == 0);

        mockReader.Setup(r => r["idKupac"]).Returns(5);
        mockReader.Setup(r => r["email"]).Returns("pera@mail.com");
        mockReader.Setup(r => r["ime"]).Returns("Pera");
        mockReader.Setup(r => r["prezime"]).Returns("Peric");
        mockReader.Setup(r => r["telefon"]).Returns("065999888");
        mockReader.Setup(r => r["jmbg"]).Returns("1112223334445");

        var fl = new FizickoLice();

        var result = fl.ReadEntities(mockReader.Object);

        var entity = Assert.Single(result) as FizickoLice;
        Assert.NotNull(entity);
        Assert.Equal(5, entity.IdKupac);
        Assert.Equal("pera@mail.com", entity.Email);
        Assert.Equal("Pera", entity.Ime);
        Assert.Equal("Peric", entity.Prezime);
        Assert.Equal("065999888", entity.Telefon);
        Assert.Equal("1112223334445", entity.JMBG);
    }
}
