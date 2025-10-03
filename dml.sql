
DELETE FROM StavkaUgovora;
DELETE FROM Ugovor;
DELETE FROM PrKvalifikacija;
DELETE FROM FizickoLice;
DELETE FROM PravnoLice;
DELETE FROM Automobil;
DELETE FROM Kupac;
DELETE FROM Kvalifikacija;
DELETE FROM Prodavac;
DBCC CHECKIDENT ('Ugovor', RESEED, 0);
DBCC CHECKIDENT ('Kupac', RESEED, 0);
DBCC CHECKIDENT ('Kvalifikacija', RESEED, 0);

-- Prodavci
INSERT INTO Prodavac VALUES 
(1,'Pera','Peric','pera','passpera'),
(2,'Mika','Mikic','mika','passmika'),
(3,'Jovan','Jovic','jovan','passjovan'),
(4,'Marko','Markovic','marko','passmarko'),
(5,'Luka','Lukic','luka','passluka');

-- Kvalifikacije
INSERT INTO Kvalifikacija VALUES 
('Osnovna prodajna obuka','Osnovni'),
('Obuka komunikacionih veština','Osnovni'),
('CRM i upravljanje klijentima','Srednji'),
('Obuka za menadžment prodaje','Srednji'),
('Napredna obuka za korporativnu komunikaciju','Napredni'),
('Napredni trening iz pregovaračkih veština','Napredni'),
('Obuka za prezentaciju i zastupanje brenda','Napredni');

-- PrKvalifikacija
INSERT INTO PrKvalifikacija VALUES 
(1,1,'2023-01-15'),
(1,2,'2022-06-20'),
(2,1,'2023-03-10'),
(3,2,'2022-11-05'),
(4,1,'2023-07-01');

-- Kupci
INSERT INTO Kupac VALUES 
('kupac1@test.com'),
('kupac2@email.com'),
('kupac3@test.com'),
('musterija4@email.com'),
('musterija5@test.com');

-- Pravna lica
INSERT INTO PravnoLice VALUES 
(1,'Firma1',123456789,'MB123'),
(3,'Firma3',987654321,'MB321');

-- Fizicka lica
INSERT INTO FizickoLice VALUES 
(2,'Ana','Anic','060111222','1234567890123'),
(4,'Ivan','Ivic','060333444','3210987654321'),
(5,'Zika','Zikic','060444555','4321098765432');

-- Automobili
INSERT INTO Automobil VALUES
(1,'Golf','Osnovna','benzin','Crvena',15000),
(2,'Tesla','Premium','elektricni','Plava',40000),
(3,'BMW','Standard','dizel','Crna',25000),
(4,'Audi','Premium','benzin','Siva',30000),
(5,'Nissan Leaf','Standard','elektricni','Bela',35000),
(6,'Mercedes','Luxury','dizel','Srebrna',50000),
(7,'Fiat','Osnovna','benzin','Zuta',12000),
(8,'Opel','Standard','dizel','Plava',18000);

-- Ugovori
-- PDV = 0.2
INSERT INTO Ugovor (datum, pdv, idProdavac, idKupac) VALUES
(GETDATE(),0.2,1,1),
(GETDATE(),0.2,2,2),
(GETDATE(),0.2,3,3),
(GETDATE(),0.2,4,4),
(GETDATE(),0.2,5,2),
(GETDATE(),0.2,1,3);

-- Stavke ugovora
-- Ugovor 1 (kupac 1, prodavac 1)
INSERT INTO StavkaUgovora VALUES (1,1,15000,0,15000,1);
INSERT INTO StavkaUgovora VALUES (1,2,40000,0.1,36000,2);

-- Ugovor 2 (kupac 2, prodavac 2)
INSERT INTO StavkaUgovora VALUES (2,1,25000,0,25000,3);
INSERT INTO StavkaUgovora VALUES (2,2,30000,0,30000,4);

-- Ugovor 3 (kupac 3, prodavac 3)
INSERT INTO StavkaUgovora VALUES (3,1,35000,0.1,31500,5);

-- Ugovor 4 (kupac 4, prodavac 4)
INSERT INTO StavkaUgovora VALUES (4,1,50000,0,50000,6);

-- Ugovor 5 (kupac 2, prodavac 5)
INSERT INTO StavkaUgovora VALUES (5,1,15000,0,15000,7);
INSERT INTO StavkaUgovora VALUES (5,2,18000,0,18000,8);

-- Ugovor 6 (kupac 3, prodavac 1)
INSERT INTO StavkaUgovora VALUES (6,1,40000,0.1,36000,2);
INSERT INTO StavkaUgovora VALUES (6,2,25000,0,25000,3);
