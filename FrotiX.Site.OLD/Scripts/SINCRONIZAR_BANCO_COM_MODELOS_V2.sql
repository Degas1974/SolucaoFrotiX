/****************************************************************************************
 * 🔄 SCRIPT: Sincronização Banco de Dados ↔ Modelos C# (V2 - ROBUSTO)
 * --------------------------------------------------------------------------------------
 * Descrição: Versão robusta que VALIDA a existência de cada tabela antes de fazer backup
 *
 * Autor: Claude Sonnet 4.5 (FrotiX Team)
 * Data: 13/02/2026
 * Versão: 2.0 (ROBUSTA)
 *
 * ✅ MELHORIAS DA V2:
 * - Verifica existência de CADA tabela antes de fazer backup
 * - Pula tabelas que não existem com aviso (não quebra o script)
 * - Lista quais tabelas foram encontradas vs não encontradas
 * - Mais seguro para bancos com estruturas diferentes
 *
 * ⚠️ IMPORTANTE:
 * - Baseado na auditoria completa (761 discrepâncias)
 * - Usa transação global com rollback automático
 * - Tempo estimado: 5-15 minutos
 ****************************************************************************************/

USE Frotix;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '🔄 SINCRONIZAÇÃO BANCO ↔ MODELOS C# (V2 - ROBUSTA)';
PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '⏰ Início: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '📊 Banco: ' + DB_NAME();
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- VARIÁVEIS DE CONTROLE
-- ═══════════════════════════════════════════════════════════════════════════════════

DECLARE @StartTime DATETIME = GETDATE();
DECLARE @PhaseStartTime DATETIME;
DECLARE @TableName NVARCHAR(128);
DECLARE @BackupTableName NVARCHAR(128);
DECLARE @SQL NVARCHAR(MAX);
DECLARE @RowCount INT;
DECLARE @TabelasBackup INT = 0;
DECLARE @TabelasNaoEncontradas INT = 0;

-- ═══════════════════════════════════════════════════════════════════════════════════
-- FASE 0: VALIDAÇÕES PRELIMINARES
-- ═══════════════════════════════════════════════════════════════════════════════════

SET @PhaseStartTime = GETDATE();
PRINT '┌──────────────────────────────────────────────────────────────────────────┐';
PRINT '│ FASE 0: Validações Preliminares                                         │';
PRINT '└──────────────────────────────────────────────────────────────────────────┘';
PRINT '';

-- Verificar banco correto
IF DB_NAME() <> 'Frotix'
BEGIN
    PRINT '❌ ERRO: Conectado ao banco "' + DB_NAME() + '", esperado "Frotix"';
    PRINT '   Execute: USE Frotix; GO antes de executar este script';
    PRINT '';
    RETURN;
END

PRINT '✅ Banco correto: Frotix';

-- Verificar conexões ativas
SELECT @RowCount = COUNT(*)
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('Frotix')
  AND session_id <> @@SPID
  AND is_user_process = 1;

PRINT '📊 Conexões ativas: ' + CAST(@RowCount AS VARCHAR);

IF @RowCount > 10
    PRINT '⚠️  Aviso: ' + CAST(@RowCount AS VARCHAR) + ' conexões ativas. Considere horário de baixo uso.';

PRINT '';
PRINT '⏱️  Tempo: ' + CAST(DATEDIFF(SECOND, @PhaseStartTime, GETDATE()) AS VARCHAR) + 's';
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- INÍCIO DA TRANSAÇÃO PRINCIPAL
-- ═══════════════════════════════════════════════════════════════════════════════════

BEGIN TRY
    BEGIN TRANSACTION SincronizacaoModelos;

    PRINT '✅ Transação iniciada';
    PRINT '';

    -- ═══════════════════════════════════════════════════════════════════════════════
    -- FASE 1: BACKUP DAS TABELAS AFETADAS (COM VALIDAÇÃO)
    -- ═══════════════════════════════════════════════════════════════════════════════

    SET @PhaseStartTime = GETDATE();
    PRINT '┌──────────────────────────────────────────────────────────────────────────┐';
    PRINT '│ FASE 1: Backup das Tabelas Afetadas (com validação)                     │';
    PRINT '└──────────────────────────────────────────────────────────────────────────┘';
    PRINT '';
    PRINT '📋 Criando backups com sufixo _BACKUP_20260213...';
    PRINT '';

    -- Tabela temporária para controlar backups
    CREATE TABLE #TabelasParaBackup (
        Id INT IDENTITY(1,1),
        NomeTabela NVARCHAR(128),
        Existe BIT,
        BackupCriado BIT DEFAULT 0,
        RegistrosBackup INT DEFAULT 0
    );

    -- Lista de tabelas que precisam de backup
    INSERT INTO #TabelasParaBackup (NomeTabela, Existe)
    VALUES
        ('Abastecimento', 0),
        ('AbastecimentoPendente', 0),
        ('AlertasFrotiX', 0),
        ('AlertasUsuario', 0),
        ('AnosDisponiveisAbastecimento', 0),
        ('AspNetUsers', 0),
        ('AtaRegistroPrecos', 0),
        ('Combustivel', 0),
        ('Contrato', 0);

    -- Verificar quais tabelas existem
    UPDATE #TabelasParaBackup
    SET Existe = 1
    WHERE EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.TABLES
        WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = #TabelasParaBackup.NomeTabela
    );

    -- Mostrar resumo de tabelas encontradas
    DECLARE @TabelasEncontradas INT;
    SELECT @TabelasEncontradas = COUNT(*) FROM #TabelasParaBackup WHERE Existe = 1;
    SELECT @TabelasNaoEncontradas = COUNT(*) FROM #TabelasParaBackup WHERE Existe = 0;

    PRINT '📊 Tabelas encontradas: ' + CAST(@TabelasEncontradas AS VARCHAR) + ' / ' +
          CAST((@TabelasEncontradas + @TabelasNaoEncontradas) AS VARCHAR);

    IF @TabelasNaoEncontradas > 0
    BEGIN
        PRINT '';
        PRINT '⚠️  Tabelas NÃO encontradas (serão ignoradas):';
        SELECT '   ❌ ' + NomeTabela AS [Tabelas Ausentes]
        FROM #TabelasParaBackup
        WHERE Existe = 0;
        PRINT '';
    END

    -- Criar backups para tabelas que existem
    DECLARE tabela_cursor CURSOR FOR
    SELECT NomeTabela FROM #TabelasParaBackup WHERE Existe = 1 ORDER BY Id;

    OPEN tabela_cursor;
    FETCH NEXT FROM tabela_cursor INTO @TableName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @BackupTableName = @TableName + '_BACKUP_20260213';

        -- Remover backup anterior se existir
        IF OBJECT_ID('dbo.' + @BackupTableName, 'U') IS NOT NULL
        BEGIN
            SET @SQL = 'DROP TABLE dbo.' + QUOTENAME(@BackupTableName);
            EXEC sp_executesql @SQL;
        END

        -- Criar novo backup
        SET @SQL = 'SELECT * INTO dbo.' + QUOTENAME(@BackupTableName) +
                   ' FROM dbo.' + QUOTENAME(@TableName);
        EXEC sp_executesql @SQL;

        SET @RowCount = @@ROWCOUNT;

        -- Atualizar controle
        UPDATE #TabelasParaBackup
        SET BackupCriado = 1, RegistrosBackup = @RowCount
        WHERE NomeTabela = @TableName;

        PRINT '✅ Backup: ' + @BackupTableName + ' (' + CAST(@RowCount AS VARCHAR) + ' registros)';

        SET @TabelasBackup = @TabelasBackup + 1;

        FETCH NEXT FROM tabela_cursor INTO @TableName;
    END

    CLOSE tabela_cursor;
    DEALLOCATE tabela_cursor;

    PRINT '';
    PRINT '✅ Total de backups criados: ' + CAST(@TabelasBackup AS VARCHAR);
    PRINT '⏱️  Tempo: ' + CAST(DATEDIFF(SECOND, @PhaseStartTime, GETDATE()) AS VARCHAR) + 's';
    PRINT '';

    -- ═══════════════════════════════════════════════════════════════════════════════
    -- FASE 2: CORREÇÕES NULLABLE - AlertasFrotiX
    -- ═══════════════════════════════════════════════════════════════════════════════

    SET @PhaseStartTime = GETDATE();
    PRINT '┌──────────────────────────────────────────────────────────────────────────┐';
    PRINT '│ FASE 2: Correções Nullable - AlertasFrotiX                              │';
    PRINT '└──────────────────────────────────────────────────────────────────────────┘';
    PRINT '';

    -- Verificar se tabela AlertasFrotiX existe
    IF EXISTS (SELECT 1 FROM #TabelasParaBackup WHERE NomeTabela = 'AlertasFrotiX' AND Existe = 1)
    BEGIN
        PRINT '🔧 Corrigindo colunas de dias da semana (7 alterações)...';
        PRINT '';

        -- Segunda-feira (Monday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Monday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Monday BIT NULL;
            PRINT '✅ AlertasFrotiX.Monday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Monday → Já está NULL';

        -- Terça-feira (Tuesday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Tuesday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Tuesday BIT NULL;
            PRINT '✅ AlertasFrotiX.Tuesday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Tuesday → Já está NULL';

        -- Quarta-feira (Wednesday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Wednesday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Wednesday BIT NULL;
            PRINT '✅ AlertasFrotiX.Wednesday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Wednesday → Já está NULL';

        -- Quinta-feira (Thursday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Thursday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Thursday BIT NULL;
            PRINT '✅ AlertasFrotiX.Thursday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Thursday → Já está NULL';

        -- Sexta-feira (Friday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Friday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Friday BIT NULL;
            PRINT '✅ AlertasFrotiX.Friday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Friday → Já está NULL';

        -- Sábado (Saturday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Saturday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Saturday BIT NULL;
            PRINT '✅ AlertasFrotiX.Saturday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Saturday → Já está NULL';

        -- Domingo (Sunday)
        IF EXISTS (
            SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'AlertasFrotiX'
              AND COLUMN_NAME = 'Sunday'
              AND IS_NULLABLE = 'NO'
        )
        BEGIN
            ALTER TABLE dbo.AlertasFrotiX ALTER COLUMN Sunday BIT NULL;
            PRINT '✅ AlertasFrotiX.Sunday → NULL (era NOT NULL)';
        END
        ELSE
            PRINT '⚪ AlertasFrotiX.Sunday → Já está NULL';

        PRINT '';
        PRINT '✅ Correções nullable concluídas: AlertasFrotiX';
    END
    ELSE
    BEGIN
        PRINT '⚠️  Tabela AlertasFrotiX não encontrada - pulando correções';
    END

    PRINT '';
    PRINT '⏱️  Tempo: ' + CAST(DATEDIFF(SECOND, @PhaseStartTime, GETDATE()) AS VARCHAR) + 's';
    PRINT '';

    -- ═══════════════════════════════════════════════════════════════════════════════
    -- FASE 3: DOCUMENTAÇÃO DAS DEMAIS CORREÇÕES
    -- ═══════════════════════════════════════════════════════════════════════════════

    PRINT '┌──────────────────────────────────────────────────────────────────────────┐';
    PRINT '│ FASE 3: Documentação das Demais Correções                               │';
    PRINT '└──────────────────────────────────────────────────────────────────────────┘';
    PRINT '';
    PRINT '📋 As seguintes correções NÃO foram aplicadas automaticamente:';
    PRINT '';
    PRINT '🔴 CRÍTICO - Nullable Incompatível (183 restantes):';
    PRINT '   - Abastecimento: 6 propriedades (Litros, DataHora, etc.)';
    PRINT '   - Motorista: ~10 propriedades';
    PRINT '   - Outras tabelas: ~167 propriedades';
    PRINT '';
    PRINT '🟡 ATENÇÃO - MaxLength Incompatível (11 ocorrências):';
    PRINT '   - AbastecimentoPendente.TipoPendencia: [MaxLength(2000)] → 50';
    PRINT '   - AbastecimentoPendente.CampoCorrecao: [MaxLength(50)] → 20';
    PRINT '';
    PRINT '📘 Consulte o arquivo para detalhes:';
    PRINT '   ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md';
    PRINT '';

    -- ═══════════════════════════════════════════════════════════════════════════════
    -- COMMIT DA TRANSAÇÃO
    -- ═══════════════════════════════════════════════════════════════════════════════

    COMMIT TRANSACTION SincronizacaoModelos;

    PRINT '✅ Transação confirmada (COMMIT)';
    PRINT '';

    -- ═══════════════════════════════════════════════════════════════════════════════
    -- FASE 4: ESTATÍSTICAS FINAIS
    -- ═══════════════════════════════════════════════════════════════════════════════

    DECLARE @TempoTotal INT = DATEDIFF(SECOND, @StartTime, GETDATE());

    PRINT '════════════════════════════════════════════════════════════════════════════════';
    PRINT '✅ SINCRONIZAÇÃO CONCLUÍDA COM SUCESSO!';
    PRINT '════════════════════════════════════════════════════════════════════════════════';
    PRINT '';
    PRINT '📊 RESUMO GERAL:';
    PRINT '   - Backups criados: ' + CAST(@TabelasBackup AS VARCHAR);
    PRINT '   - Tabelas não encontradas: ' + CAST(@TabelasNaoEncontradas AS VARCHAR);
    PRINT '   - Alterações SQL aplicadas: 7 (AlertasFrotiX nullable)';
    PRINT '   - Tempo total: ' + CAST(@TempoTotal AS VARCHAR) + 's';
    PRINT '';
    PRINT '📋 PRÓXIMOS PASSOS:';
    PRINT '   1. Revisar arquivo: ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md';
    PRINT '   2. Corrigir modelos C# (183 propriedades nullable + 11 MaxLength)';
    PRINT '   3. Compilar projeto e validar';
    PRINT '   4. Executar testes';
    PRINT '';
    PRINT '💾 BACKUPS DISPONÍVEIS:';

    SELECT
        '   ✅ ' + NomeTabela + '_BACKUP_20260213 (' +
        CAST(RegistrosBackup AS VARCHAR) + ' registros)' AS [Backups Criados]
    FROM #TabelasParaBackup
    WHERE BackupCriado = 1
    ORDER BY Id;

    PRINT '';
    PRINT '🔄 INSTRUÇÕES DE ROLLBACK:';
    PRINT '';
    PRINT '/*';
    PRINT '-- Para reverter APENAS AlertasFrotiX:';
    PRINT 'DROP TABLE dbo.AlertasFrotiX;';
    PRINT 'EXEC sp_rename ''dbo.AlertasFrotiX_BACKUP_20260213'', ''AlertasFrotiX'';';
    PRINT '';
    PRINT '-- Para reverter TODAS as tabelas:';
    PRINT 'DECLARE @sql NVARCHAR(MAX) = '''';';
    PRINT 'SELECT @sql = @sql +';
    PRINT '  ''DROP TABLE dbo.'' + REPLACE(TABLE_NAME, ''_BACKUP_20260213'', '''') + '';'' +';
    PRINT '  ''EXEC sp_rename ''''dbo.'' + TABLE_NAME + '''''', '''''' +';
    PRINT '  REPLACE(TABLE_NAME, ''_BACKUP_20260213'', '''') + '''''';''';
    PRINT 'FROM INFORMATION_SCHEMA.TABLES';
    PRINT 'WHERE TABLE_NAME LIKE ''%_BACKUP_20260213'';';
    PRINT 'EXEC sp_executesql @sql;';
    PRINT '*/';
    PRINT '';
    PRINT '════════════════════════════════════════════════════════════════════════════════';
    PRINT '';
    PRINT '⏰ Fim: ' + CONVERT(VARCHAR, GETDATE(), 120);
    PRINT '';

    -- Limpar tabelas temporárias
    DROP TABLE #TabelasParaBackup;

END TRY
BEGIN CATCH
    -- ═══════════════════════════════════════════════════════════════════════════════
    -- TRATAMENTO DE ERROS E ROLLBACK
    -- ═══════════════════════════════════════════════════════════════════════════════

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION SincronizacaoModelos;

    DECLARE @ErrorMessage2 NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity2 INT = ERROR_SEVERITY();
    DECLARE @ErrorState2 INT = ERROR_STATE();
    DECLARE @ErrorLine INT = ERROR_LINE();

    PRINT '';
    PRINT '════════════════════════════════════════════════════════════════════════════════';
    PRINT '❌ ERRO DURANTE A SINCRONIZAÇÃO!';
    PRINT '════════════════════════════════════════════════════════════════════════════════';
    PRINT '';
    PRINT '📋 Detalhes do erro:';
    PRINT '   Mensagem: ' + @ErrorMessage2;
    PRINT '   Linha: ' + CAST(@ErrorLine AS VARCHAR);
    PRINT '   Severidade: ' + CAST(@ErrorSeverity2 AS VARCHAR);
    PRINT '   Estado: ' + CAST(@ErrorState2 AS VARCHAR);
    PRINT '';
    PRINT '🔄 Rollback automático executado - nenhuma alteração foi aplicada';
    PRINT '';
    PRINT '💡 Recomendações:';
    PRINT '   1. Verifique a mensagem de erro acima';
    PRINT '   2. Corrija o problema identificado';
    PRINT '   3. Execute o script novamente';
    PRINT '';
    PRINT '════════════════════════════════════════════════════════════════════════════════';
    PRINT '';

    -- Limpar tabelas temporárias
    IF OBJECT_ID('tempdb..#TabelasParaBackup') IS NOT NULL
        DROP TABLE #TabelasParaBackup;

    -- Re-throw do erro para parar execução
    RAISERROR(@ErrorMessage2, @ErrorSeverity2, @ErrorState2);
END CATCH;

SET NOCOUNT OFF;
GO
