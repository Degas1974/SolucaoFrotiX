-- ============================================
-- Script de Atualização do Campo TemFichaVistoriaReal
-- ============================================
-- Data de Criação: 21/01/2026
-- Objetivo: Atualizar o campo TemFichaVistoriaReal em todos os registros existentes
--
-- Regras de Identificação:
-- • TemFichaVistoriaReal = 0 (false) → FichaAmarela (106.930 bytes)
-- • TemFichaVistoriaReal = 1 (true)  → Ficha de Vistoria real (qualquer outro tamanho)
-- • TemFichaVistoriaReal = NULL      → Sem ficha (FichaVistoria IS NULL)
--
-- Critério de Identificação:
-- • FichaAmarela tem EXATAMENTE 106.930 bytes (tamanho do arquivo FichaAmarelaNova.jpg)
-- • Identificado por: DATALENGTH(FichaVistoria) = 106930
-- • Hash MD5 da FichaAmarela: 2d1c1dc837e19e00550825b5a431d65b
-- ============================================

USE FrotiX
GO

PRINT ''
PRINT '=========================================='
PRINT '  ATUALIZAÇÃO CAMPO TemFichaVistoriaReal'
PRINT '=========================================='
PRINT ''

-- ============================================
-- PASSO 1: Verificar se a coluna existe
-- ============================================
PRINT '[1/4] Verificando se a coluna TemFichaVistoriaReal existe...'
PRINT ''

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Viagem')
      AND name = 'TemFichaVistoriaReal'
)
BEGIN
    PRINT '  ⚠️  ATENÇÃO: Coluna TemFichaVistoriaReal não existe!'
    PRINT '  ✅ Criando coluna TemFichaVistoriaReal (bit NULL)...'

    ALTER TABLE dbo.Viagem
    ADD TemFichaVistoriaReal bit NULL

    PRINT '  ✅ Coluna criada com sucesso!'
END
ELSE
BEGIN
    PRINT '  ✅ Coluna TemFichaVistoriaReal já existe.'
END

PRINT ''

-- ============================================
-- PASSO 2: Estatísticas ANTES da atualização
-- ============================================
PRINT '[2/4] Estatísticas ANTES da atualização:'
PRINT ''

DECLARE @TotalRegistros INT
DECLARE @ComFichaAmarela INT
DECLARE @ComFichaReal INT
DECLARE @SemFicha INT
DECLARE @TemFichaVistoriaRealNull INT
DECLARE @TemFichaVistoriaRealTrue INT
DECLARE @TemFichaVistoriaRealFalse INT

-- Total de registros
SELECT @TotalRegistros = COUNT(*) FROM dbo.Viagem
PRINT '  • Total de Viagens: ' + CAST(@TotalRegistros AS VARCHAR(10))

-- Registros com FichaAmarela (exatamente 106.930 bytes)
SELECT @ComFichaAmarela = COUNT(*)
FROM dbo.Viagem
WHERE DATALENGTH(FichaVistoria) = 106930
PRINT '  • Com FichaAmarela (106.930 bytes): ' + CAST(@ComFichaAmarela AS VARCHAR(10))

-- Registros com Ficha Real (outros tamanhos)
SELECT @ComFichaReal = COUNT(*)
FROM dbo.Viagem
WHERE FichaVistoria IS NOT NULL
  AND DATALENGTH(FichaVistoria) <> 106930
PRINT '  • Com Ficha de Vistoria REAL (outros tamanhos): ' + CAST(@ComFichaReal AS VARCHAR(10))

-- Registros sem ficha
SELECT @SemFicha = COUNT(*)
FROM dbo.Viagem
WHERE FichaVistoria IS NULL
PRINT '  • Sem Ficha (NULL): ' + CAST(@SemFicha AS VARCHAR(10))

PRINT ''
PRINT '  Estado atual do campo TemFichaVistoriaReal:'

SELECT @TemFichaVistoriaRealNull = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal IS NULL
PRINT '    - NULL: ' + CAST(@TemFichaVistoriaRealNull AS VARCHAR(10))

SELECT @TemFichaVistoriaRealTrue = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 1
PRINT '    - TRUE (1): ' + CAST(@TemFichaVistoriaRealTrue AS VARCHAR(10))

SELECT @TemFichaVistoriaRealFalse = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 0
PRINT '    - FALSE (0): ' + CAST(@TemFichaVistoriaRealFalse AS VARCHAR(10))

PRINT ''

-- ============================================
-- PASSO 3: Atualizar os registros
-- ============================================
PRINT '[3/4] Atualizando registros...'
PRINT ''

BEGIN TRANSACTION

BEGIN TRY
    -- Atualizar todos os registros de uma vez usando CASE
    UPDATE dbo.Viagem
    SET TemFichaVistoriaReal = CASE
        -- FichaAmarela: exatamente 106.930 bytes
        WHEN DATALENGTH(FichaVistoria) = 106930 THEN 0
        -- Ficha Real: qualquer outro tamanho (não NULL e diferente de 106.930)
        WHEN FichaVistoria IS NOT NULL AND DATALENGTH(FichaVistoria) <> 106930 THEN 1
        -- Sem ficha: NULL
        ELSE NULL
    END
    WHERE TemFichaVistoriaReal IS NULL
       OR TemFichaVistoriaReal <> CASE
            WHEN DATALENGTH(FichaVistoria) = 106930 THEN 0
            WHEN FichaVistoria IS NOT NULL AND DATALENGTH(FichaVistoria) <> 106930 THEN 1
            ELSE NULL
        END

    DECLARE @RowsAffected INT = @@ROWCOUNT
    PRINT '  ✅ Total de ' + CAST(@RowsAffected AS VARCHAR(10)) + ' registros atualizados!'

    COMMIT TRANSACTION
    PRINT ''
    PRINT '  ✅ TRANSAÇÃO COMMITADA COM SUCESSO!'

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT ''
    PRINT '  ❌ ERRO AO ATUALIZAR REGISTROS!'
    PRINT '  ❌ Mensagem de Erro: ' + ERROR_MESSAGE()
    PRINT '  ❌ Linha do Erro: ' + CAST(ERROR_LINE() AS VARCHAR(10))
    PRINT '  ❌ TRANSAÇÃO REVERTIDA (ROLLBACK)'
    PRINT ''

    -- Re-throw the error
    THROW;
END CATCH

PRINT ''

-- ============================================
-- PASSO 4: Estatísticas DEPOIS da atualização
-- ============================================
PRINT '[4/4] Estatísticas DEPOIS da atualização:'
PRINT ''

SELECT @TemFichaVistoriaRealNull = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal IS NULL
PRINT '  • TemFichaVistoriaReal = NULL: ' + CAST(@TemFichaVistoriaRealNull AS VARCHAR(10))

SELECT @TemFichaVistoriaRealTrue = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 1
PRINT '  • TemFichaVistoriaReal = TRUE (1): ' + CAST(@TemFichaVistoriaRealTrue AS VARCHAR(10))

SELECT @TemFichaVistoriaRealFalse = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 0
PRINT '  • TemFichaVistoriaReal = FALSE (0): ' + CAST(@TemFichaVistoriaRealFalse AS VARCHAR(10))

PRINT ''
PRINT '=========================================='
PRINT '  ATUALIZAÇÃO CONCLUÍDA COM SUCESSO!'
PRINT '=========================================='
PRINT ''
PRINT 'RESUMO:'
PRINT '  • Total de viagens processadas: ' + CAST(@TotalRegistros AS VARCHAR(10))
PRINT '  • Com Ficha Real (TRUE): ' + CAST(@TemFichaVistoriaRealTrue AS VARCHAR(10))
PRINT '  • Com FichaAmarela (FALSE): ' + CAST(@TemFichaVistoriaRealFalse AS VARCHAR(10))
PRINT '  • Sem Ficha (NULL): ' + CAST(@TemFichaVistoriaRealNull AS VARCHAR(10))
PRINT ''
PRINT 'PRÓXIMOS PASSOS:'
PRINT '  1. Abra o Visual Studio'
PRINT '  2. Build > Rebuild Solution'
PRINT '  3. Pressione F5'
PRINT '  4. No navegador: Ctrl+Shift+Delete > Limpar cache'
PRINT '  5. OU simplesmente: Ctrl+F5 (hard refresh)'
PRINT ''
PRINT 'IMPORTANTE: Os botões de Ficha de Vistoria agora aparecerão!'
PRINT ''

-- ============================================
-- VALIDAÇÃO: Mostrar alguns exemplos
-- ============================================
PRINT '=========================================='
PRINT '  EXEMPLOS DE REGISTROS ATUALIZADOS'
PRINT '=========================================='
PRINT ''

PRINT 'Primeiros 5 registros com Ficha REAL (TRUE):'
SELECT TOP 5
    ViagemId,
    NoFichaVistoria,
    CASE
        WHEN FichaVistoria IS NULL THEN 'NULL'
        ELSE 'Imagem (' + CAST(DATALENGTH(FichaVistoria) AS VARCHAR(20)) + ' bytes)'
    END AS FichaVistoriaStatus,
    TemFichaVistoriaReal,
    CONVERT(VARCHAR(10), DataInicial, 103) AS DataInicial,
    Status
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 1
ORDER BY DataInicial DESC

PRINT ''
PRINT 'Primeiros 5 registros com FichaAmarela (FALSE):'
SELECT TOP 5
    ViagemId,
    NoFichaVistoria,
    CASE
        WHEN FichaVistoria IS NULL THEN 'NULL'
        ELSE 'Imagem (' + CAST(DATALENGTH(FichaVistoria) AS VARCHAR(20)) + ' bytes)'
    END AS FichaVistoriaStatus,
    TemFichaVistoriaReal,
    CONVERT(VARCHAR(10), DataInicial, 103) AS DataInicial,
    Status
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 0
ORDER BY DataInicial DESC

PRINT ''
PRINT '=========================================='
PRINT '  VERIFICAÇÃO DE INTEGRIDADE'
PRINT '=========================================='
PRINT ''

-- Verificar se há registros com tamanho suspeito
PRINT 'Verificando se há registros com tamanhos anormais...'
SELECT
    DATALENGTH(FichaVistoria) AS TamanhoBytes,
    COUNT(*) AS Quantidade
FROM dbo.Viagem
WHERE FichaVistoria IS NOT NULL
GROUP BY DATALENGTH(FichaVistoria)
ORDER BY Quantidade DESC

PRINT ''
PRINT '=========================================='
PRINT '  SCRIPT FINALIZADO'
PRINT '=========================================='
GO
