-- Verificar e criar tabela LogErros
SET NOCOUNT ON;

IF OBJECT_ID('dbo.LogErros', 'U') IS NULL
BEGIN
    PRINT 'Tabela LogErros NAO existe. Criando...'
END
ELSE
BEGIN
    PRINT 'Tabela LogErros JA existe!'
    SELECT TOP 1 * FROM dbo.LogErros ORDER BY LogErroId DESC
END
GO
