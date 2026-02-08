/*
╔══════════════════════════════════════════════════════════════════════════╗
║                                                                          ║
║  📚 SISTEMA DE CONTROLE DE CACHE - DOCGENERATOR                         ║
║                                                                          ║
║  Este script cria o sistema de rastreamento de mudanças nos arquivos    ║
║  para detectar quando a documentação precisa ser atualizada             ║
║                                                                          ║
╚══════════════════════════════════════════════════════════════════════════╝
*/

-- Criar schema DocGenerator se não existir
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'DocGenerator')
BEGIN
    EXEC('CREATE SCHEMA DocGenerator');
END
GO

-- Drop objetos existentes para recriar (desenvolvimento)
IF OBJECT_ID('DocGenerator.vw_FilesNeedingUpdate', 'V') IS NOT NULL
    DROP VIEW DocGenerator.vw_FilesNeedingUpdate;
GO

IF OBJECT_ID('DocGenerator.sp_DetectFileChanges', 'P') IS NOT NULL
    DROP PROCEDURE DocGenerator.sp_DetectFileChanges;
GO

IF OBJECT_ID('DocGenerator.sp_MarkAsDocumented', 'P') IS NOT NULL
    DROP PROCEDURE DocGenerator.sp_MarkAsDocumented;
GO

IF OBJECT_ID('DocGenerator.sp_RunAutoScan', 'P') IS NOT NULL
    DROP PROCEDURE DocGenerator.sp_RunAutoScan;
GO

-- Tabela principal de rastreamento de arquivos
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FileTracking' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.FileTracking (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FilePath NVARCHAR(500) NOT NULL,           -- Caminho relativo do arquivo
        FileHash NVARCHAR(64) NOT NULL,            -- Hash SHA256 do conteúdo
        FileSize INT NOT NULL,                     -- Tamanho em bytes
        LineCount INT NOT NULL,                    -- Número de linhas
        CharacterCount INT NOT NULL,               -- Número de caracteres
        LastModified DATETIME2 NOT NULL,           -- Última modificação
        LastDocumented DATETIME2 NULL,             -- Última vez que foi documentado
        DocumentationVersion INT DEFAULT 1,        -- Versão da documentação
        NeedsUpdate BIT DEFAULT 0,                 -- Precisa atualizar?
        UpdateReason NVARCHAR(200) NULL,           -- Razão da atualização
        CreatedAt DATETIME2 DEFAULT GETDATE(),
        UpdatedAt DATETIME2 DEFAULT GETDATE(),

        CONSTRAINT UQ_DocGenerator_FilePath UNIQUE (FilePath)
    );

    PRINT '✅ Tabela DocGenerator.FileTracking criada';
END
ELSE
BEGIN
    PRINT '⚠️ Tabela DocGenerator.FileTracking já existe';
END
GO

-- Tabela de histórico de mudanças
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FileChangeHistory' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.FileChangeHistory (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FilePath NVARCHAR(500) NOT NULL,
        OldHash NVARCHAR(64) NULL,
        NewHash NVARCHAR(64) NOT NULL,
        OldSize INT NULL,
        NewSize INT NOT NULL,
        ChangeType NVARCHAR(50) NOT NULL,          -- 'MODIFIED', 'ADDED', 'DELETED'
        ChangePercentage DECIMAL(5,2) NULL,        -- % de mudança
        DetectedAt DATETIME2 DEFAULT GETDATE(),

        INDEX IX_DocGen_FilePath (FilePath),
        INDEX IX_DocGen_DetectedAt (DetectedAt)
    );

    PRINT '✅ Tabela DocGenerator.FileChangeHistory criada';
END
ELSE
BEGIN
    PRINT '⚠️ Tabela DocGenerator.FileChangeHistory já existe';
END
GO

-- Tabela de alertas de documentação
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DocumentationAlerts' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.DocumentationAlerts (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FilePath NVARCHAR(500) NOT NULL,
        AlertType NVARCHAR(50) NOT NULL,           -- 'NEEDS_UPDATE', 'STALE', 'MISSING'
        AlertMessage NVARCHAR(500) NOT NULL,
        Priority INT DEFAULT 1,                    -- 1=Baixa, 2=Média, 3=Alta
        AssignedToUserId INT NULL,                 -- Usuário responsável
        Status NVARCHAR(50) DEFAULT 'PENDING',     -- 'PENDING', 'IN_PROGRESS', 'RESOLVED'
        CreatedAt DATETIME2 DEFAULT GETDATE(),
        ResolvedAt DATETIME2 NULL,

        INDEX IX_DocGen_Alert_FilePath (FilePath),
        INDEX IX_DocGen_Alert_Status (Status),
        INDEX IX_DocGen_Alert_Priority (Priority)
    );

    PRINT '✅ Tabela DocGenerator.DocumentationAlerts criada';
END
ELSE
BEGIN
    PRINT '⚠️ Tabela DocGenerator.DocumentationAlerts já existe';
END
GO

-- Tabela de configuração de monitoramento
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MonitoringConfig' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.MonitoringConfig (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ConfigKey NVARCHAR(100) NOT NULL UNIQUE,
        ConfigValue NVARCHAR(500) NOT NULL,
        Description NVARCHAR(500) NULL,
        UpdatedAt DATETIME2 DEFAULT GETDATE()
    );

    -- Inserir configurações padrão
    INSERT INTO DocGenerator.MonitoringConfig (ConfigKey, ConfigValue, Description) VALUES
    ('ChangeThreshold', '5.0', 'Percentual mínimo de mudança para exigir atualização (%)'),
    ('StaleDays', '30', 'Número de dias para considerar documentação desatualizada'),
    ('AutoScanInterval', '24', 'Intervalo em horas para varredura automática'),
    ('NotifyUsers', 'true', 'Notificar usuários sobre documentação desatualizada'),
    ('HighPriorityThreshold', '20.0', 'Mudança > X% = prioridade alta'),
    ('MediumPriorityThreshold', '10.0', 'Mudança > X% = prioridade média');

    PRINT '✅ Tabela DocGenerator.MonitoringConfig criada com configurações padrão';
END
ELSE
BEGIN
    PRINT '⚠️ Tabela DocGenerator.MonitoringConfig já existe';
END
GO

-- Índices para performance (criar se não existem)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DocGen_FileTracking_NeedsUpdate')
BEGIN
    CREATE INDEX IX_DocGen_FileTracking_NeedsUpdate ON DocGenerator.FileTracking (NeedsUpdate);
    PRINT '✅ Índice IX_DocGen_FileTracking_NeedsUpdate criado';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DocGen_FileTracking_LastModified')
BEGIN
    CREATE INDEX IX_DocGen_FileTracking_LastModified ON DocGenerator.FileTracking (LastModified);
    PRINT '✅ Índice IX_DocGen_FileTracking_LastModified criado';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DocGen_FileChangeHistory_FilePath_DetectedAt')
BEGIN
    CREATE INDEX IX_DocGen_FileChangeHistory_FilePath_DetectedAt ON DocGenerator.FileChangeHistory (FilePath, DetectedAt DESC);
    PRINT '✅ Índice IX_DocGen_FileChangeHistory_FilePath_DetectedAt criado';
END
GO

-- View para arquivos que precisam de atualização
CREATE VIEW DocGenerator.vw_FilesNeedingUpdate AS
SELECT
    ft.FilePath,
    ft.FileSize,
    ft.LineCount,
    ft.CharacterCount,
    ft.LastModified,
    ft.LastDocumented,
    DATEDIFF(DAY, ft.LastDocumented, GETDATE()) AS DaysSinceDocumented,
    ft.UpdateReason,
    da.AlertType,
    da.Priority
FROM DocGenerator.FileTracking ft
LEFT JOIN DocGenerator.DocumentationAlerts da
    ON ft.FilePath = da.FilePath
    AND da.Status = 'PENDING'
WHERE ft.NeedsUpdate = 1
    OR (ft.LastDocumented IS NULL)
    OR (DATEDIFF(DAY, ft.LastDocumented, GETDATE()) > 30);
GO

PRINT '✅ View DocGenerator.vw_FilesNeedingUpdate criada';
GO

-- Stored Procedure para detectar mudanças
CREATE PROCEDURE DocGenerator.sp_DetectFileChanges
    @FilePath NVARCHAR(500),
    @NewHash NVARCHAR(64),
    @NewSize INT,
    @NewLineCount INT,
    @NewCharCount INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OldHash NVARCHAR(64),
            @OldSize INT,
            @ChangePercentage DECIMAL(5,2),
            @ChangeType NVARCHAR(50),
            @NeedsUpdate BIT = 0,
            @UpdateReason NVARCHAR(200);

    -- Verificar se arquivo já existe no tracking
    SELECT @OldHash = FileHash, @OldSize = FileSize
    FROM DocGenerator.FileTracking
    WHERE FilePath = @FilePath;

    IF @OldHash IS NULL
    BEGIN
        -- Arquivo novo
        SET @ChangeType = 'ADDED';
        SET @NeedsUpdate = 1;
        SET @UpdateReason = 'Arquivo novo detectado';

        INSERT INTO DocGenerator.FileTracking
            (FilePath, FileHash, FileSize, LineCount, CharacterCount, LastModified, NeedsUpdate, UpdateReason)
        VALUES
            (@FilePath, @NewHash, @NewSize, @NewLineCount, @NewCharCount, GETDATE(), 1, @UpdateReason);
    END
    ELSE IF @OldHash != @NewHash
    BEGIN
        -- Arquivo modificado
        SET @ChangeType = 'MODIFIED';

        -- Calcular percentual de mudança (baseado no tamanho)
        SET @ChangePercentage = ABS(@NewSize - @OldSize) * 100.0 / NULLIF(@OldSize, 0);

        -- Verificar se precisa atualizar baseado no threshold
        DECLARE @Threshold DECIMAL(5,2);
        SELECT @Threshold = CAST(ConfigValue AS DECIMAL(5,2))
        FROM DocGenerator.MonitoringConfig
        WHERE ConfigKey = 'ChangeThreshold';

        IF @ChangePercentage >= ISNULL(@Threshold, 5.0)
        BEGIN
            SET @NeedsUpdate = 1;
            SET @UpdateReason = CONCAT('Mudança de ', FORMAT(@ChangePercentage, 'N2'), '% detectada');

            -- Determinar prioridade do alerta
            DECLARE @Priority INT = 1;
            DECLARE @HighThreshold DECIMAL(5,2), @MediumThreshold DECIMAL(5,2);

            SELECT @HighThreshold = CAST(ConfigValue AS DECIMAL(5,2))
            FROM DocGenerator.MonitoringConfig WHERE ConfigKey = 'HighPriorityThreshold';

            SELECT @MediumThreshold = CAST(ConfigValue AS DECIMAL(5,2))
            FROM DocGenerator.MonitoringConfig WHERE ConfigKey = 'MediumPriorityThreshold';

            IF @ChangePercentage >= ISNULL(@HighThreshold, 20.0)
                SET @Priority = 3;
            ELSE IF @ChangePercentage >= ISNULL(@MediumThreshold, 10.0)
                SET @Priority = 2;

            -- Criar alerta
            INSERT INTO DocGenerator.DocumentationAlerts
                (FilePath, AlertType, AlertMessage, Priority)
            VALUES
                (@FilePath, 'NEEDS_UPDATE', @UpdateReason, @Priority);
        END

        -- Atualizar tracking
        UPDATE DocGenerator.FileTracking
        SET FileHash = @NewHash,
            FileSize = @NewSize,
            LineCount = @NewLineCount,
            CharacterCount = @NewCharCount,
            LastModified = GETDATE(),
            NeedsUpdate = @NeedsUpdate,
            UpdateReason = @UpdateReason,
            UpdatedAt = GETDATE()
        WHERE FilePath = @FilePath;
    END

    -- Registrar no histórico
    INSERT INTO DocGenerator.FileChangeHistory
        (FilePath, OldHash, NewHash, OldSize, NewSize, ChangeType, ChangePercentage)
    VALUES
        (@FilePath, @OldHash, @NewHash, @OldSize, @NewSize, @ChangeType, @ChangePercentage);

    SELECT @NeedsUpdate AS NeedsUpdate, @UpdateReason AS UpdateReason;
END
GO

PRINT '✅ Stored Procedure DocGenerator.sp_DetectFileChanges criada';
GO

-- Stored Procedure para marcar como documentado
CREATE PROCEDURE DocGenerator.sp_MarkAsDocumented
    @FilePath NVARCHAR(500),
    @DocumentationVersion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DocGenerator.FileTracking
    SET LastDocumented = GETDATE(),
        NeedsUpdate = 0,
        UpdateReason = NULL,
        DocumentationVersion = ISNULL(@DocumentationVersion, DocumentationVersion + 1),
        UpdatedAt = GETDATE()
    WHERE FilePath = @FilePath;

    -- Marcar alertas como resolvidos
    UPDATE DocGenerator.DocumentationAlerts
    SET Status = 'RESOLVED',
        ResolvedAt = GETDATE()
    WHERE FilePath = @FilePath
        AND Status = 'PENDING';

    PRINT 'Arquivo marcado como documentado: ' + @FilePath;
END
GO

PRINT '✅ Stored Procedure DocGenerator.sp_MarkAsDocumented criada';
GO

-- Stored Procedure para varredura automática
CREATE PROCEDURE DocGenerator.sp_RunAutoScan
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StaleDays INT;
    SELECT @StaleDays = CAST(ConfigValue AS INT)
    FROM DocGenerator.MonitoringConfig
    WHERE ConfigKey = 'StaleDays';

    -- Marcar arquivos desatualizados
    UPDATE ft
    SET ft.NeedsUpdate = 1,
        ft.UpdateReason = CONCAT('Documentação desatualizada (',
                                DATEDIFF(DAY, ft.LastDocumented, GETDATE()),
                                ' dias)')
    FROM DocGenerator.FileTracking ft
    WHERE ft.LastDocumented IS NOT NULL
        AND DATEDIFF(DAY, ft.LastDocumented, GETDATE()) > ISNULL(@StaleDays, 30)
        AND ft.NeedsUpdate = 0;

    -- Criar alertas para arquivos desatualizados
    INSERT INTO DocGenerator.DocumentationAlerts (FilePath, AlertType, AlertMessage, Priority)
    SELECT
        ft.FilePath,
        'STALE',
        CONCAT('Documentação desatualizada há ',
              DATEDIFF(DAY, ft.LastDocumented, GETDATE()),
              ' dias'),
        2
    FROM DocGenerator.FileTracking ft
    WHERE ft.NeedsUpdate = 1
        AND ft.UpdateReason LIKE '%desatualizada%'
        AND NOT EXISTS (
            SELECT 1 FROM DocGenerator.DocumentationAlerts da
            WHERE da.FilePath = ft.FilePath AND da.Status = 'PENDING'
        );

    PRINT 'Varredura automática concluída. ' +
          CAST(@@ROWCOUNT AS NVARCHAR) + ' arquivos marcados como desatualizados.';
END
GO

PRINT '✅ Stored Procedure DocGenerator.sp_RunAutoScan criada';
GO

-- Resumo final
PRINT '';
PRINT '════════════════════════════════════════════════════════════════════════════';
PRINT '✅ SISTEMA DE CONTROLE DE CACHE CRIADO COM SUCESSO!';
PRINT '════════════════════════════════════════════════════════════════════════════';
PRINT '📊 Tabelas: FileTracking, FileChangeHistory, DocumentationAlerts, MonitoringConfig';
PRINT '🔄 Stored Procedures: sp_DetectFileChanges, sp_MarkAsDocumented, sp_RunAutoScan';
PRINT '👁️ View: vw_FilesNeedingUpdate';
PRINT '';
PRINT '⏰ Para agendar varredura automática, descomente o bloco de Job no final';
PRINT '════════════════════════════════════════════════════════════════════════════';
GO

/*
-- Job do SQL Server para varredura automática (executa diariamente às 2:00 AM)
-- Descomentar e executar separadamente se desejar

USE msdb;
GO

EXEC dbo.sp_add_job
    @job_name = N'DocGenerator_AutoScan',
    @enabled = 1,
    @description = N'Varredura automática de arquivos para atualização de documentação';

EXEC sp_add_jobstep
    @job_name = N'DocGenerator_AutoScan',
    @step_name = N'Executar varredura',
    @subsystem = N'TSQL',
    @command = N'EXEC DocGenerator.sp_RunAutoScan;',
    @database_name = N'FrotiX';

EXEC sp_add_jobschedule
    @job_name = N'DocGenerator_AutoScan',
    @schedule_name = N'Diariamente 2AM',
    @freq_type = 4, -- Diariamente
    @freq_interval = 1, -- Todos os dias
    @active_start_time = 20000; -- 2:00:00 AM

EXEC sp_add_jobserver
    @job_name = N'DocGenerator_AutoScan';
*/
