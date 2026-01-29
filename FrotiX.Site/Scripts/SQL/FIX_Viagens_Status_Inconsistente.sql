-- ╔══════════════════════════════════════════════════════════════════════════════════════════╗
-- ║  SCRIPT: Correção de Status Inconsistente em Viagens                                     ║
-- ║  Data: 29/01/2026                                                                         ║
-- ║  Problema: Viagens com DataFinalizacao preenchida mas Status vazio/null                   ║
-- ║  Sintoma: Aparecem como "Cancelada" no grid mas "Aberta" na Ficha de Impressão            ║
-- ╚══════════════════════════════════════════════════════════════════════════════════════════╝

-- ================================================================================
-- PASSO 1: DIAGNÓSTICO - Identificar todas as viagens com status inconsistente
-- ================================================================================

PRINT '=== DIAGNÓSTICO DE VIAGENS COM STATUS INCONSISTENTE ==='
PRINT ''

-- Viagens finalizadas (tem DataFinalizacao) mas com Status vazio/NULL
SELECT 
    ViagemId,
    NoFichaVistoria,
    DataInicial,
    DataFinal,
    DataFinalizacao,
    DataCancelamento,
    Status,
    'PROBLEMA: Finalizada mas Status vazio' AS Diagnostico
FROM dbo.Viagem
WHERE DataFinalizacao IS NOT NULL 
  AND DataCancelamento IS NULL 
  AND (Status IS NULL OR Status = '' OR LTRIM(RTRIM(Status)) = '')
ORDER BY DataInicial DESC;

PRINT ''
PRINT 'Total de viagens finalizadas com Status inconsistente:'
SELECT COUNT(*) AS TotalInconsistentes
FROM dbo.Viagem
WHERE DataFinalizacao IS NOT NULL 
  AND DataCancelamento IS NULL 
  AND (Status IS NULL OR Status = '' OR LTRIM(RTRIM(Status)) = '');

-- ================================================================================
-- PASSO 2: CORREÇÃO EM MASSA - Atualizar Status para 'Realizada'
-- ================================================================================

PRINT ''
PRINT '=== INICIANDO CORREÇÃO EM MASSA ==='
PRINT ''

BEGIN TRANSACTION;

-- Atualiza todas as viagens finalizadas que estão com Status inconsistente
UPDATE dbo.Viagem
SET Status = 'Realizada'
WHERE DataFinalizacao IS NOT NULL 
  AND DataCancelamento IS NULL 
  AND (Status IS NULL OR Status = '' OR LTRIM(RTRIM(Status)) = '');

DECLARE @RowsAffected INT = @@ROWCOUNT;

PRINT 'Viagens atualizadas para Status = ''Realizada'': ' + CAST(@RowsAffected AS VARCHAR(10));

-- Verificação pós-correção
IF @RowsAffected > 0
BEGIN
    PRINT ''
    PRINT '=== VERIFICAÇÃO PÓS-CORREÇÃO ==='
    
    -- Confirma que não há mais inconsistências
    DECLARE @Remaining INT;
    SELECT @Remaining = COUNT(*)
    FROM dbo.Viagem
    WHERE DataFinalizacao IS NOT NULL 
      AND DataCancelamento IS NULL 
      AND (Status IS NULL OR Status = '' OR LTRIM(RTRIM(Status)) = '');
    
    IF @Remaining = 0
    BEGIN
        PRINT 'SUCESSO: Todas as inconsistências foram corrigidas!'
        COMMIT TRANSACTION;
        PRINT 'Transação CONFIRMADA (COMMIT)'
    END
    ELSE
    BEGIN
        PRINT 'AVISO: Ainda restam ' + CAST(@Remaining AS VARCHAR(10)) + ' registros inconsistentes.'
        ROLLBACK TRANSACTION;
        PRINT 'Transação REVERTIDA (ROLLBACK) - Verifique os dados manualmente.'
    END
END
ELSE
BEGIN
    PRINT 'Nenhuma viagem precisava de correção.'
    COMMIT TRANSACTION;
END

-- ================================================================================
-- PASSO 3: CORREÇÃO ESPECÍFICA (se necessário rodar apenas para uma viagem)
-- ================================================================================

/*
-- Descomente o bloco abaixo para corrigir UMA viagem específica:

UPDATE dbo.Viagem 
SET Status = 'Realizada' 
WHERE ViagemId = '05c5b591-af5c-428d-902d-3ba8d7304c34';

PRINT 'Viagem 05c5b591-af5c-428d-902d-3ba8d7304c34 atualizada para Status = Realizada';
*/

-- ================================================================================
-- PASSO 4: AUDITORIA - Verificar distribuição de status atual
-- ================================================================================

PRINT ''
PRINT '=== DISTRIBUIÇÃO ATUAL DE STATUS ==='

SELECT 
    ISNULL(Status, '(NULL)') AS Status,
    COUNT(*) AS Quantidade,
    CAST(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER() AS DECIMAL(5,2)) AS Percentual
FROM dbo.Viagem
GROUP BY Status
ORDER BY Quantidade DESC;

PRINT ''
PRINT '=== FIM DO SCRIPT ==='
GO
