CREATE TRIGGER trg_UpdateUgovorTotals
ON StavkaUgovora
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idUgovor INT;

    -- Ako INSERT/UPDATE
    IF EXISTS(SELECT * FROM inserted)
        SELECT TOP 1 @idUgovor = idUgovor FROM inserted;
    ELSE
        -- Ako DELETE
        SELECT TOP 1 @idUgovor = idUgovor FROM deleted;

    IF @idUgovor IS NOT NULL
    BEGIN
        DECLARE @totalBr INT, @totalIznos DECIMAL(18,2), @pdv DECIMAL(5,2);

        SELECT @totalBr = COUNT(*), @totalIznos = ISNULL(SUM(iznos),0)
        FROM StavkaUgovora
        WHERE idUgovor = @idUgovor;

        SELECT @pdv = pdv FROM Ugovor WHERE idUgovor = @idUgovor;

        UPDATE Ugovor
        SET brAutomobila = @totalBr,
            iznosBezPDV = @totalIznos,
            iznosSaPDV = @totalIznos * (1 + @pdv)
        WHERE idUgovor = @idUgovor;
    END
END;
