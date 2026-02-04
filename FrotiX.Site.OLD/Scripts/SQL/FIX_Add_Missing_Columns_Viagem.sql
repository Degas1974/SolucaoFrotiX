/* ****************************************************************************************
 * 🔧 SCRIPT: FIX_Add_Missing_Columns_Viagem.sql
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO: Adicionar 4 colunas faltantes na tabela Viagem
 *
 * ❌ PROBLEMA: Modelo C# tem 4 propriedades que não existem no banco:
 *              - CartaoAbastecimentoDevolvido
 *              - CartaoAbastecimentoEntregue
 *              - DocumentoDevolvido
 *              - DocumentoEntregue
 *
 * ✅ SOLUÇÃO: Adicionar as colunas com valores padrão (0 = false)
 *
 * 📅 DATA: 2026-02-14
 * 🔖 VERSÃO: 1.0
 **************************************************************************************** */

USE Frotix;
GO

PRINT '========================================';
PRINT '🔧 Iniciando correção de colunas...';
PRINT '========================================';

-- ================================================================
-- VERIFICAR SE AS COLUNAS JÁ EXISTEM
-- ================================================================

-- Verificar DocumentoEntregue
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Viagem')
    AND name = 'DocumentoEntregue'
)
BEGIN
    PRINT '➕ Adicionando coluna: DocumentoEntregue';
    ALTER TABLE dbo.Viagem
    ADD DocumentoEntregue bit NULL DEFAULT (0);
    PRINT '✅ DocumentoEntregue adicionada';
END
ELSE
BEGIN
    PRINT '✔️ DocumentoEntregue já existe';
END
GO

-- Verificar DocumentoDevolvido
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Viagem')
    AND name = 'DocumentoDevolvido'
)
BEGIN
    PRINT '➕ Adicionando coluna: DocumentoDevolvido';
    ALTER TABLE dbo.Viagem
    ADD DocumentoDevolvido bit NULL DEFAULT (0);
    PRINT '✅ DocumentoDevolvido adicionada';
END
ELSE
BEGIN
    PRINT '✔️ DocumentoDevolvido já existe';
END
GO

-- Verificar CartaoAbastecimentoEntregue
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Viagem')
    AND name = 'CartaoAbastecimentoEntregue'
)
BEGIN
    PRINT '➕ Adicionando coluna: CartaoAbastecimentoEntregue';
    ALTER TABLE dbo.Viagem
    ADD CartaoAbastecimentoEntregue bit NULL DEFAULT (0);
    PRINT '✅ CartaoAbastecimentoEntregue adicionada';
END
ELSE
BEGIN
    PRINT '✔️ CartaoAbastecimentoEntregue já existe';
END
GO

-- Verificar CartaoAbastecimentoDevolvido
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Viagem')
    AND name = 'CartaoAbastecimentoDevolvido'
)
BEGIN
    PRINT '➕ Adicionando coluna: CartaoAbastecimentoDevolvido';
    ALTER TABLE dbo.Viagem
    ADD CartaoAbastecimentoDevolvido bit NULL DEFAULT (0);
    PRINT '✅ CartaoAbastecimentoDevolvido adicionada';
END
ELSE
BEGIN
    PRINT '✔️ CartaoAbastecimentoDevolvido já existe';
END
GO

-- ================================================================
-- VERIFICAR RESULTADO
-- ================================================================

PRINT '';
PRINT '========================================';
PRINT '📊 Verificando colunas adicionadas...';
PRINT '========================================';

SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Viagem'
AND COLUMN_NAME IN (
    'DocumentoEntregue',
    'DocumentoDevolvido',
    'CartaoAbastecimentoEntregue',
    'CartaoAbastecimentoDevolvido'
)
ORDER BY COLUMN_NAME;

PRINT '';
PRINT '========================================';
PRINT '✅ Script executado com sucesso!';
PRINT '========================================';
GO
