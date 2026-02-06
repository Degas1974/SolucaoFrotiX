-- ============================================================================
-- FROTIX DB JOBS - SCRIPT DE JOBS DO SQL SERVER AGENT
-- ============================================================================
--
-- ARQUIVO: FrotiXDBJobs.sql
-- OBJETIVO: Criar Jobs agendados para execucao automatica no servidor
-- ULTIMA ATUALIZACAO: 23/01/2026
--
-- ============================================================================
-- IMPORTANTE:
-- ============================================================================
-- 1. Este script deve ser executado APENAS no SERVIDOR DE PRODUCAO
-- 2. Requer SQL Server Agent habilitado e em execucao
-- 3. Requer permissoes de sysadmin ou SQLAgentOperatorRole
-- 4. NAO executar em ambiente de desenvolvimento local
--
-- ============================================================================
-- JOBS INCLUIDOS:
-- ============================================================================
-- 1. DocGenerator_AutoScan - Varredura automatica de documentacao (diario 2AM)
-- 2. (Adicionar novos jobs conforme necessidade)
--
-- ============================================================================

USE msdb;
GO

PRINT '============================================================================'
PRINT 'FROTIX DB JOBS - Configuracao de Jobs do SQL Server Agent'
PRINT '============================================================================'
PRINT ''
PRINT 'Data de Execucao: ' + CONVERT(varchar, GETDATE(), 120)
PRINT ''

-- ============================================================================
-- JOB 1: DocGenerator_AutoScan
-- ============================================================================
-- Descricao: Varredura automatica de arquivos para detectar documentacao
--            desatualizada. Executa diariamente as 2:00 AM.
-- ============================================================================

PRINT '============================================================================'
PRINT 'JOB 1: DocGenerator_AutoScan'
PRINT '============================================================================'
PRINT ''

-- Remover job existente se houver (para recriar com configuracoes atualizadas)
IF EXISTS (SELECT 1 FROM msdb.dbo.sysjobs WHERE name = N'DocGenerator_AutoScan')
BEGIN
    EXEC msdb.dbo.sp_delete_job @job_name = N'DocGenerator_AutoScan', @delete_unused_schedule = 1;
    PRINT '- Job DocGenerator_AutoScan removido (sera recriado)';
END
GO

-- Criar Job
EXEC msdb.dbo.sp_add_job
    @job_name = N'DocGenerator_AutoScan',
    @enabled = 1,
    @notify_level_eventlog = 2,
    @notify_level_email = 0,
    @notify_level_netsend = 0,
    @notify_level_page = 0,
    @delete_level = 0,
    @description = N'Varredura automatica de arquivos para detectar documentacao desatualizada no sistema FrotiX. Marca arquivos como desatualizados apos 30 dias sem atualizacao.',
    @category_name = N'[Uncategorized (Local)]',
    @owner_login_name = N'sa';
GO

PRINT '+ Job DocGenerator_AutoScan criado';

-- Adicionar Step (Passo de Execucao)
EXEC msdb.dbo.sp_add_jobstep
    @job_name = N'DocGenerator_AutoScan',
    @step_name = N'Executar Varredura Automatica',
    @step_id = 1,
    @cmdexec_success_code = 0,
    @on_success_action = 1,  -- Quit with success
    @on_success_step_id = 0,
    @on_fail_action = 2,     -- Quit with failure
    @on_fail_step_id = 0,
    @retry_attempts = 3,
    @retry_interval = 5,     -- 5 minutos entre tentativas
    @os_run_priority = 0,
    @subsystem = N'TSQL',
    @command = N'EXEC DocGenerator.sp_RunAutoScan;',
    @database_name = N'FrotiX',
    @flags = 0;
GO

PRINT '+ Step "Executar Varredura Automatica" adicionado';

-- Criar Schedule (Agendamento)
EXEC msdb.dbo.sp_add_jobschedule
    @job_name = N'DocGenerator_AutoScan',
    @name = N'Diariamente as 02:00 AM',
    @enabled = 1,
    @freq_type = 4,              -- Diariamente
    @freq_interval = 1,          -- A cada 1 dia
    @freq_subday_type = 1,       -- Uma vez no horario especificado
    @freq_subday_interval = 0,
    @freq_relative_interval = 0,
    @freq_recurrence_factor = 0,
    @active_start_date = 20260123,  -- Data de inicio
    @active_end_date = 99991231,    -- Sem data de termino
    @active_start_time = 20000,     -- 02:00:00 AM (formato HHMMSS)
    @active_end_time = 235959;
GO

PRINT '+ Schedule "Diariamente as 02:00 AM" criado';

-- Associar Job ao Servidor Local
EXEC msdb.dbo.sp_add_jobserver
    @job_name = N'DocGenerator_AutoScan',
    @server_name = N'(LOCAL)';
GO

PRINT '+ Job associado ao servidor local';
PRINT ''

-- ============================================================================
-- JOB 2: (Template para novos jobs)
-- ============================================================================
-- Descomentar e configurar conforme necessidade
-- ============================================================================

/*
-- Exemplo de novo job:

IF EXISTS (SELECT 1 FROM msdb.dbo.sysjobs WHERE name = N'FrotiX_NomeDoJob')
BEGIN
    EXEC msdb.dbo.sp_delete_job @job_name = N'FrotiX_NomeDoJob', @delete_unused_schedule = 1;
END
GO

EXEC msdb.dbo.sp_add_job
    @job_name = N'FrotiX_NomeDoJob',
    @enabled = 1,
    @description = N'Descricao do job',
    @category_name = N'[Uncategorized (Local)]',
    @owner_login_name = N'sa';
GO

EXEC msdb.dbo.sp_add_jobstep
    @job_name = N'FrotiX_NomeDoJob',
    @step_name = N'Executar Procedimento',
    @step_id = 1,
    @subsystem = N'TSQL',
    @command = N'EXEC dbo.NomeDaProcedure;',
    @database_name = N'FrotiX';
GO

EXEC msdb.dbo.sp_add_jobschedule
    @job_name = N'FrotiX_NomeDoJob',
    @name = N'Nome do Schedule',
    @enabled = 1,
    @freq_type = 4,              -- 4=Diario, 8=Semanal, 16=Mensal
    @freq_interval = 1,
    @active_start_time = 30000;  -- 03:00:00 AM
GO

EXEC msdb.dbo.sp_add_jobserver
    @job_name = N'FrotiX_NomeDoJob',
    @server_name = N'(LOCAL)';
GO
*/

-- ============================================================================
-- VERIFICACAO FINAL
-- ============================================================================

PRINT '============================================================================'
PRINT 'VERIFICACAO FINAL - Jobs Criados'
PRINT '============================================================================'
PRINT ''

SELECT
    j.name AS [Nome do Job],
    j.enabled AS [Habilitado],
    j.description AS [Descricao],
    s.name AS [Schedule],
    CASE s.freq_type
        WHEN 1 THEN 'Uma vez'
        WHEN 4 THEN 'Diariamente'
        WHEN 8 THEN 'Semanalmente'
        WHEN 16 THEN 'Mensalmente'
        ELSE 'Outro'
    END AS [Frequencia],
    STUFF(RIGHT('000000' + CAST(s.active_start_time AS VARCHAR(6)), 6), 3, 0, ':') AS [Horario]
FROM msdb.dbo.sysjobs j
INNER JOIN msdb.dbo.sysjobschedules js ON j.job_id = js.job_id
INNER JOIN msdb.dbo.sysschedules s ON js.schedule_id = s.schedule_id
WHERE j.name LIKE 'DocGenerator%' OR j.name LIKE 'FrotiX%'
ORDER BY j.name;

PRINT ''
PRINT '============================================================================'
PRINT 'FROTIX DB JOBS - Configuracao Concluida'
PRINT '============================================================================'
PRINT ''
PRINT 'PROXIMOS PASSOS:'
PRINT '1. Verificar se SQL Server Agent esta em execucao'
PRINT '2. Testar execucao manual: EXEC msdb.dbo.sp_start_job @job_name = ''DocGenerator_AutoScan'''
PRINT '3. Monitorar historico: SELECT * FROM msdb.dbo.sysjobhistory WHERE job_id = (SELECT job_id FROM msdb.dbo.sysjobs WHERE name = ''DocGenerator_AutoScan'')'
PRINT ''
PRINT 'Data/Hora de Conclusao: ' + CONVERT(VARCHAR, GETDATE(), 120)
PRINT '============================================================================'
GO
