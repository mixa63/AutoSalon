CREATE TRIGGER trg_SetPopustAndIznos
ON StavkaUgovora
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
    SET 
        popust = CASE 
                    WHEN a.tipGoriva = 'elektricni' THEN 0.1
                    ELSE 0
                 END,
        iznos = s.cenaAutomobila * (1 - 
                   CASE 
                       WHEN a.tipGoriva = 'elektricni' THEN 0.1
                       ELSE 0
                   END)
    FROM StavkaUgovora s
    JOIN inserted i ON s.idUgovor = i.idUgovor AND s.rb = i.rb
    JOIN Automobil a ON i.idAutomobil = a.idAutomobil;
END;

