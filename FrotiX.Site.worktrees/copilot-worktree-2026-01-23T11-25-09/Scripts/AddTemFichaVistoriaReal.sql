-- ============================================
-- SCRIPT: Adicionar campo TemFichaVistoriaReal
-- Data: 21/01/2026
-- Descrição: Campo para indicar se a viagem possui
--            ficha de vistoria real (não padrão/amarelinha)
-- ============================================

-- 1. Adicionar coluna na tabela Viagem
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Viagem')
    AND name = 'TemFichaVistoriaReal'
)
BEGIN
    ALTER TABLE dbo.Viagem
    ADD TemFichaVistoriaReal BIT NULL;

    PRINT '✅ Coluna TemFichaVistoriaReal adicionada com sucesso!';
END
ELSE
BEGIN
    PRINT '⚠️ Coluna TemFichaVistoriaReal já existe.';
END
GO

-- 2. (Opcional) Atualizar registros existentes baseado em NoFichaVistoria
-- Considera como "ficha real" se NoFichaVistoria > 0 E FichaVistoria não é NULL
-- Você pode ajustar esta lógica conforme necessário

/*
UPDATE dbo.Viagem
SET TemFichaVistoriaReal = CASE
    WHEN NoFichaVistoria > 0 AND FichaVistoria IS NOT NULL THEN 1
    ELSE 0
END
WHERE TemFichaVistoriaReal IS NULL;

PRINT '✅ Registros existentes atualizados!';
*/

-- 3. Verificar resultado
SELECT
    COUNT(*) AS TotalViagens,
    SUM(CASE WHEN TemFichaVistoriaReal = 1 THEN 1 ELSE 0 END) AS ComFichaReal,
    SUM(CASE WHEN TemFichaVistoriaReal = 0 THEN 1 ELSE 0 END) AS SemFichaReal,
    SUM(CASE WHEN TemFichaVistoriaReal IS NULL THEN 1 ELSE 0 END) AS NaoDefinido
FROM dbo.Viagem;
