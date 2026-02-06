-- ============================================
-- SCRIPT: Atualizar campo TemFichaVistoriaReal
-- Data: 22/01/2026
-- Autor: Claude Sonnet 4.5 / FrotiX Team
-- Descrição: Popula campo TemFichaVistoriaReal em registros existentes
--            baseado no conteúdo do campo FichaVistoria
-- ============================================
-- IMPORTANTE: Fazer backup do banco ANTES de executar!
-- BACKUP DATABASE FrotiX TO DISK = 'C:\Backup\FrotiX_PreUpdateTemFicha_22012026.bak';
-- ============================================

USE FrotiX;
GO

-- ════════════════════════════════════════════════════════════════
-- ETAPA 1: DIAGNÓSTICO - Verificar estado atual
-- ════════════════════════════════════════════════════════════════

PRINT '';
PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  DIAGNÓSTICO: Estado Atual do Campo TemFichaVistoriaReal    ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

-- Verificar se coluna existe
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Viagem'
      AND COLUMN_NAME = 'TemFichaVistoriaReal'
)
BEGIN
    PRINT '❌ ERRO: Coluna TemFichaVistoriaReal não existe!';
    PRINT '   Execute primeiro: Scripts/AddTemFichaVistoriaReal.sql';
    RAISERROR('Coluna TemFichaVistoriaReal não encontrada.', 16, 1);
    RETURN;
END
ELSE
BEGIN
    PRINT '✅ Coluna TemFichaVistoriaReal existe.';
END

-- Distribuição de valores
PRINT '';
PRINT '📊 Distribuição de valores:';
SELECT
    CASE WHEN TemFichaVistoriaReal IS NULL THEN 'NULL'
         WHEN TemFichaVistoriaReal = 1 THEN 'TRUE'
         ELSE 'FALSE'
    END AS TemFichaVistoriaReal,
    COUNT(*) AS Quantidade
FROM dbo.Viagem
GROUP BY TemFichaVistoriaReal
ORDER BY TemFichaVistoriaReal;

-- Casos problemáticos (FichaVistoria preenchida mas TemFichaVistoriaReal NULL)
DECLARE @RegistrosProblematicos INT;
SELECT @RegistrosProblematicos = COUNT(*)
FROM dbo.Viagem
WHERE FichaVistoria IS NOT NULL
  AND TemFichaVistoriaReal IS NULL;

IF (@RegistrosProblematicos > 0)
BEGIN
    PRINT '';
    PRINT '⚠️ ATENÇÃO: ' + CAST(@RegistrosProblematicos AS VARCHAR) + ' registros com FichaVistoria preenchida mas TemFichaVistoriaReal = NULL';
    PRINT '   Esses registros serão atualizados.';
END
ELSE
BEGIN
    PRINT '';
    PRINT '✅ Não há registros problemáticos.';
END

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';

-- ════════════════════════════════════════════════════════════════
-- ETAPA 2: ATUALIZAÇÃO - Popular TemFichaVistoriaReal
-- ════════════════════════════════════════════════════════════════

PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  ATUALIZAÇÃO: Populando TemFichaVistoriaReal                ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

BEGIN TRANSACTION;

DECLARE @TotalAtualizados INT = 0;
DECLARE @ComFichaReal INT = 0;
DECLARE @SemFichaReal INT = 0;
DECLARE @DataHoraInicio DATETIME = GETDATE();

-- ════════════════════════════════════════════════════════════════
-- 🔹 BLOCO: Atualizar registros COM ficha real
-- REGRA: FichaVistoria IS NOT NULL → TemFichaVistoriaReal = TRUE (1)
-- ════════════════════════════════════════════════════════════════

PRINT '🔄 Atualizando registros COM ficha real...';

UPDATE dbo.Viagem
SET TemFichaVistoriaReal = 1
WHERE FichaVistoria IS NOT NULL
  AND (TemFichaVistoriaReal IS NULL OR TemFichaVistoriaReal = 0);

SET @ComFichaReal = @@ROWCOUNT;
PRINT '✅ Registros COM ficha real atualizados: ' + CAST(@ComFichaReal AS VARCHAR);

-- ════════════════════════════════════════════════════════════════
-- 🔹 BLOCO: Atualizar registros SEM ficha real
-- REGRA: FichaVistoria IS NULL → TemFichaVistoriaReal = FALSE (0)
-- ════════════════════════════════════════════════════════════════

PRINT '🔄 Atualizando registros SEM ficha real...';

UPDATE dbo.Viagem
SET TemFichaVistoriaReal = 0
WHERE FichaVistoria IS NULL
  AND (TemFichaVistoriaReal IS NULL OR TemFichaVistoriaReal = 1);

SET @SemFichaReal = @@ROWCOUNT;
PRINT '✅ Registros SEM ficha real atualizados: ' + CAST(@SemFichaReal AS VARCHAR);

-- Total
SET @TotalAtualizados = @ComFichaReal + @SemFichaReal;
PRINT '';
PRINT '📊 Total de registros atualizados: ' + CAST(@TotalAtualizados AS VARCHAR);

-- Tempo de execução
DECLARE @DataHoraFim DATETIME = GETDATE();
DECLARE @TempoExecucao INT = DATEDIFF(SECOND, @DataHoraInicio, @DataHoraFim);
PRINT '⏱️ Tempo de execução: ' + CAST(@TempoExecucao AS VARCHAR) + ' segundos';

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';

-- ════════════════════════════════════════════════════════════════
-- ETAPA 3: VALIDAÇÃO - Verificar integridade
-- ════════════════════════════════════════════════════════════════

PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  VALIDAÇÃO: Verificando Integridade dos Dados               ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

-- Verificar se ainda há NULLs (não deveria!)
DECLARE @RegistrosNULL INT;
SELECT @RegistrosNULL = COUNT(*)
FROM dbo.Viagem
WHERE TemFichaVistoriaReal IS NULL;

IF (@RegistrosNULL > 0)
BEGIN
    PRINT '❌ ERRO: Ainda existem ' + CAST(@RegistrosNULL AS VARCHAR) + ' registros com TemFichaVistoriaReal = NULL!';
    PRINT '   Rollback será executado.';
    ROLLBACK TRANSACTION;
    RAISERROR('Atualização incompleta. Verificar dados.', 16, 1);
    RETURN;
END
ELSE
BEGIN
    PRINT '✅ Não há registros com TemFichaVistoriaReal = NULL.';
END

-- Verificar incoerências (FichaVistoria preenchida mas flag = FALSE)
DECLARE @Incoerencias1 INT;
SELECT @Incoerencias1 = COUNT(*)
FROM dbo.Viagem
WHERE FichaVistoria IS NOT NULL
  AND TemFichaVistoriaReal = 0;

IF (@Incoerencias1 > 0)
BEGIN
    PRINT '❌ ERRO: ' + CAST(@Incoerencias1 AS VARCHAR) + ' registros com FichaVistoria preenchida mas TemFichaVistoriaReal = FALSE!';
    ROLLBACK TRANSACTION;
    RAISERROR('Incoerência detectada: FichaVistoria preenchida mas flag = FALSE.', 16, 1);
    RETURN;
END
ELSE
BEGIN
    PRINT '✅ Não há incoerências (FichaVistoria vs TemFichaVistoriaReal).';
END

-- Verificar incoerências reversas (FichaVistoria NULL mas flag = TRUE)
DECLARE @Incoerencias2 INT;
SELECT @Incoerencias2 = COUNT(*)
FROM dbo.Viagem
WHERE FichaVistoria IS NULL
  AND TemFichaVistoriaReal = 1;

IF (@Incoerencias2 > 0)
BEGIN
    PRINT '❌ ERRO: ' + CAST(@Incoerencias2 AS VARCHAR) + ' registros com FichaVistoria NULL mas TemFichaVistoriaReal = TRUE!';
    ROLLBACK TRANSACTION;
    RAISERROR('Incoerência detectada: FichaVistoria NULL mas flag = TRUE.', 16, 1);
    RETURN;
END
ELSE
BEGIN
    PRINT '✅ Não há incoerências reversas.';
END

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';

-- ════════════════════════════════════════════════════════════════
-- ETAPA 4: ESTATÍSTICAS FINAIS
-- ════════════════════════════════════════════════════════════════

PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  ESTATÍSTICAS FINAIS                                         ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

DECLARE @TotalViagens INT;
DECLARE @TotalComFicha INT;
DECLARE @TotalSemFicha INT;
DECLARE @PercentualComFicha DECIMAL(5,2);

SELECT @TotalViagens = COUNT(*) FROM dbo.Viagem;
SELECT @TotalComFicha = COUNT(*) FROM dbo.Viagem WHERE TemFichaVistoriaReal = 1;
SELECT @TotalSemFicha = COUNT(*) FROM dbo.Viagem WHERE TemFichaVistoriaReal = 0;

IF (@TotalViagens > 0)
BEGIN
    SET @PercentualComFicha = CAST(@TotalComFicha AS DECIMAL(10,2)) / CAST(@TotalViagens AS DECIMAL(10,2)) * 100;
END
ELSE
BEGIN
    SET @PercentualComFicha = 0;
END

PRINT '📊 Total de Viagens: ' + CAST(@TotalViagens AS VARCHAR);
PRINT '✅ Com Ficha Real: ' + CAST(@TotalComFicha AS VARCHAR) + ' (' + CAST(@PercentualComFicha AS VARCHAR(10)) + '%)';
PRINT '❌ Sem Ficha Real: ' + CAST(@TotalSemFicha AS VARCHAR) + ' (' + CAST((100 - @PercentualComFicha) AS VARCHAR(10)) + '%)';

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';

-- ════════════════════════════════════════════════════════════════
-- ETAPA 5: COMMIT
-- ════════════════════════════════════════════════════════════════

PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  COMMIT: Confirmando Transação                               ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

COMMIT TRANSACTION;

PRINT '🎉 TRANSAÇÃO CONFIRMADA COM SUCESSO!';
PRINT '';
PRINT '✅ Campo TemFichaVistoriaReal atualizado em todos os registros.';
PRINT '✅ Integridade dos dados validada.';
PRINT '✅ Pronto para uso na aplicação.';

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';

-- ════════════════════════════════════════════════════════════════
-- ETAPA 6: EXEMPLOS PARA TESTE
-- ════════════════════════════════════════════════════════════════

PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  EXEMPLOS PARA TESTE NA INTERFACE                            ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

PRINT 'Top 5 viagens COM ficha real (use para testar botão ATIVO):';
PRINT '';

SELECT TOP 5
    v.ViagemId,
    v.NoFichaVistoria,
    CONVERT(VARCHAR, v.DataInicial, 103) AS DataInicial,
    ISNULL(ve.Placa, 'N/A') AS Placa,
    ISNULL(m.Nome, 'N/A') AS Motorista,
    ISNULL(v.Destino, 'N/A') AS Destino,
    v.TemFichaVistoriaReal
FROM dbo.Viagem v
LEFT JOIN dbo.Veiculo ve ON v.VeiculoId = ve.VeiculoId
LEFT JOIN dbo.Motorista m ON v.MotoristaId = m.MotoristaId
WHERE v.TemFichaVistoriaReal = 1
ORDER BY v.DataCriacao DESC;

PRINT '';
PRINT 'Top 5 viagens SEM ficha real (use para testar botão BLOQUEADO):';
PRINT '';

SELECT TOP 5
    v.ViagemId,
    v.NoFichaVistoria,
    CONVERT(VARCHAR, v.DataInicial, 103) AS DataInicial,
    ISNULL(ve.Placa, 'N/A') AS Placa,
    ISNULL(m.Nome, 'N/A') AS Motorista,
    ISNULL(v.Destino, 'N/A') AS Destino,
    v.TemFichaVistoriaReal
FROM dbo.Viagem v
LEFT JOIN dbo.Veiculo ve ON v.VeiculoId = ve.VeiculoId
LEFT JOIN dbo.Motorista m ON v.MotoristaId = m.MotoristaId
WHERE v.TemFichaVistoriaReal = 0
ORDER BY v.DataCriacao DESC;

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '🏁 SCRIPT FINALIZADO COM SUCESSO!';
PRINT '📅 Data/Hora: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';
PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  PRÓXIMOS PASSOS:                                            ║';
PRINT '║  1. Testar na interface de Agenda                            ║';
PRINT '║  2. Verificar botão em viagens COM ficha (laranja, ativo)    ║';
PRINT '║  3. Verificar botão em viagens SEM ficha (cinza, bloqueado)  ║';
PRINT '║  4. Atualizar documentação                                   ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

GO
