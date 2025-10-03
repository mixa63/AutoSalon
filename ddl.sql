-- Brisanje triggera
IF OBJECT_ID('trg_CheckPopust','TR') IS NOT NULL
    DROP TRIGGER trg_CheckPopust;

IF OBJECT_ID('trg_UpdateUgovorTotals','TR') IS NOT NULL
    DROP TRIGGER trg_UpdateUgovorTotals;

-- Brisanje tabela
IF OBJECT_ID('StavkaUgovora','U') IS NOT NULL DROP TABLE StavkaUgovora;
IF OBJECT_ID('Ugovor','U') IS NOT NULL DROP TABLE Ugovor;
IF OBJECT_ID('PrKvalifikacija','U') IS NOT NULL DROP TABLE PrKvalifikacija;
IF OBJECT_ID('FizickoLice','U') IS NOT NULL DROP TABLE FizickoLice;
IF OBJECT_ID('PravnoLice','U') IS NOT NULL DROP TABLE PravnoLice;
IF OBJECT_ID('Automobil','U') IS NOT NULL DROP TABLE Automobil;
IF OBJECT_ID('Kupac','U') IS NOT NULL DROP TABLE Kupac;
IF OBJECT_ID('Kvalifikacija','U') IS NOT NULL DROP TABLE Kvalifikacija;
IF OBJECT_ID('Prodavac','U') IS NOT NULL DROP TABLE Prodavac;

-- Prodavac
CREATE TABLE Prodavac (
    idProdavac INT PRIMARY KEY CHECK (idProdavac > 0),
    ime NVARCHAR(50) NOT NULL,
    prezime NVARCHAR(50) NOT NULL,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(50) NOT NULL
);

-- Kvalifikacija
CREATE TABLE Kvalifikacija (
    idKvalifikacija INT IDENTITY(1,1) PRIMARY KEY,
    naziv NVARCHAR(50) NOT NULL,
    stepen NVARCHAR(50) NOT NULL
);

-- PrKvalifikacija
CREATE TABLE PrKvalifikacija (
    idProdavac INT NOT NULL,
    idKvalifikacija INT NOT NULL,
    datumIzdavanja DATE NOT NULL,
    PRIMARY KEY (idProdavac, idKvalifikacija),
    FOREIGN KEY (idProdavac) REFERENCES Prodavac(idProdavac) ON UPDATE CASCADE,
    FOREIGN KEY (idKvalifikacija) REFERENCES Kvalifikacija(idKvalifikacija) ON UPDATE CASCADE
);

-- Kupac
CREATE TABLE Kupac (
    idKupac INT IDENTITY(1,1) PRIMARY KEY,
    email NVARCHAR(100) NOT NULL
);

-- PravnoLice
CREATE TABLE PravnoLice (
    idKupac INT PRIMARY KEY,
    nazivFirme NVARCHAR(100) NOT NULL,
    pib BIGINT NOT NULL,
    maticniBroj NVARCHAR(20) NOT NULL,
    FOREIGN KEY (idKupac) REFERENCES Kupac(idKupac)
);

-- FizickoLice
CREATE TABLE FizickoLice (
    idKupac INT PRIMARY KEY,
    ime NVARCHAR(50) NOT NULL,
    prezime NVARCHAR(50) NOT NULL,
    telefon NVARCHAR(20) NOT NULL,
    jmbg NVARCHAR(13) NOT NULL,
    FOREIGN KEY (idKupac) REFERENCES Kupac(idKupac)
);

-- Automobil
CREATE TABLE Automobil (
    idAutomobil INT PRIMARY KEY CHECK (idAutomobil > 0),
    model NVARCHAR(50) NOT NULL,
    oprema NVARCHAR(50) NOT NULL,
    tipGoriva NVARCHAR(20) NOT NULL,
    boja NVARCHAR(20) NOT NULL,
    cena DECIMAL(18,2) NOT NULL CHECK (cena > 0)
);

-- Ugovor
CREATE TABLE Ugovor (
    idUgovor INT IDENTITY(1,1) PRIMARY KEY,
    datum DATE NOT NULL,
    brAutomobila INT NOT NULL DEFAULT 0,
    pdv DECIMAL(5,2) NOT NULL,
    iznosBezPDV DECIMAL(18,2) NOT NULL DEFAULT 0,
    iznosSaPDV DECIMAL(18,2) NOT NULL DEFAULT 0,
    idProdavac INT NOT NULL,
    idKupac INT NOT NULL,
    FOREIGN KEY (idProdavac) REFERENCES Prodavac(idProdavac),
    FOREIGN KEY (idKupac) REFERENCES Kupac(idKupac)
);

-- StavkaUgovora
CREATE TABLE StavkaUgovora (
    idUgovor INT NOT NULL,
    rb INT NOT NULL CHECK (rb > 0),
    cenaAutomobila DECIMAL(18,2) NOT NULL CHECK (cenaAutomobila > 0),
    popust DECIMAL(5,2) DEFAULT 0 CHECK (popust >= 0),
    iznos DECIMAL(18,2) NOT NULL,
    idAutomobil INT NOT NULL,
    PRIMARY KEY (idUgovor, rb),
    FOREIGN KEY (idUgovor) REFERENCES Ugovor(idUgovor),
    FOREIGN KEY (idAutomobil) REFERENCES Automobil(idAutomobil),
    CHECK (iznos = cenaAutomobila * (1 - popust))
);
