-- ============================================================================
-- FROTIX BD SCRIPTS - SCRIPT CONSOLIDADO DE MODIFICACOES DO BANCO DE DADOS
-- ============================================================================
--
-- ARQUIVO: FrotiXBDScripts.sql
-- OBJETIVO: Consolidar TODAS as alteracoes de banco de dados do projeto FrotiX
-- ULTIMA ATUALIZACAO: 23/01/2026
--
-- ============================================================================
-- REGRAS IMPORTANTES:
-- ============================================================================
-- 1. Este script DEVE ser executado em ambiente de DESENVOLVIMENTO primeiro
-- 2. SEMPRE fazer BACKUP do banco antes de executar em PRODUCAO
-- 3. Objetos sao recriados com DROP + CREATE (exceto TABELAS)
-- 4. Jobs estao em arquivo separado: FrotiXDBJobs.sql
--
-- POLITICA DE DROP:
-- ✅ DROP permitido: Views, Triggers, Stored Procedures, Functions, FKs, Indices
-- ❌ DROP NUNCA: Tabelas (usar ALTER TABLE para modificacoes)
--
-- ============================================================================
-- INDICE DE SECOES:
-- ============================================================================
-- SECAO 1: SCHEMAS
-- SECAO 2: TABELAS (CREATE IF NOT EXISTS - nunca DROP)
-- SECAO 3: COLUNAS NOVAS EM TABELAS EXISTENTES (ALTER TABLE)
-- SECAO 4: INDICES DE PERFORMANCE
-- SECAO 5: FOREIGN KEYS
-- SECAO 6: VIEWS
-- SECAO 7: STORED PROCEDURES
-- SECAO 8: TRIGGERS
-- SECAO 9: FUNCTIONS
-- ============================================================================

USE [FrotiX]
GO

PRINT '============================================================================'
PRINT 'FROTIX BD SCRIPTS - Execucao Iniciada'
PRINT '============================================================================'
PRINT ''
PRINT 'Data de Execucao: ' + CONVERT(varchar, GETDATE(), 120)
PRINT ''

-- ============================================================================
-- SECAO 1: SCHEMAS
-- ============================================================================

PRINT '============================================================================'
PRINT 'SECAO 1: SCHEMAS'
PRINT '============================================================================'
PRINT ''

-- Schema DocGenerator
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'DocGenerator')
BEGIN
    EXEC('CREATE SCHEMA DocGenerator');
    PRINT '+ Schema DocGenerator criado';
END
ELSE
    PRINT '= Schema DocGenerator ja existe';
GO

-- ============================================================================
-- SECAO 2: TABELAS (CREATE IF NOT EXISTS - NUNCA DROP!)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 2: TABELAS (CREATE IF NOT EXISTS)'
PRINT '============================================================================'
PRINT ''

-- DocGenerator.FileTracking
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FileTracking' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.FileTracking (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FilePath NVARCHAR(500) NOT NULL,
        FileHash NVARCHAR(64) NOT NULL,
        FileSize INT NOT NULL,
        LineCount INT NOT NULL,
        CharacterCount INT NOT NULL,
        LastModified DATETIME2 NOT NULL,
        LastDocumented DATETIME2 NULL,
        DocumentationVersion INT DEFAULT 1,
        NeedsUpdate BIT DEFAULT 0,
        UpdateReason NVARCHAR(200) NULL,
        CreatedAt DATETIME2 DEFAULT GETDATE(),
        UpdatedAt DATETIME2 DEFAULT GETDATE(),
        CONSTRAINT UQ_DocGenerator_FilePath UNIQUE (FilePath)
    );
    PRINT '+ Tabela DocGenerator.FileTracking criada';
END
ELSE
    PRINT '= Tabela DocGenerator.FileTracking ja existe';
GO

-- DocGenerator.FileChangeHistory
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FileChangeHistory' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.FileChangeHistory (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FilePath NVARCHAR(500) NOT NULL,
        OldHash NVARCHAR(64) NULL,
        NewHash NVARCHAR(64) NOT NULL,
        OldSize INT NULL,
        NewSize INT NOT NULL,
        ChangeType NVARCHAR(50) NOT NULL,
        ChangePercentage DECIMAL(5,2) NULL,
        DetectedAt DATETIME2 DEFAULT GETDATE(),
        INDEX IX_DocGen_FilePath (FilePath),
        INDEX IX_DocGen_DetectedAt (DetectedAt)
    );
    PRINT '+ Tabela DocGenerator.FileChangeHistory criada';
END
ELSE
    PRINT '= Tabela DocGenerator.FileChangeHistory ja existe';
GO

-- DocGenerator.DocumentationAlerts
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DocumentationAlerts' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.DocumentationAlerts (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FilePath NVARCHAR(500) NOT NULL,
        AlertType NVARCHAR(50) NOT NULL,
        AlertMessage NVARCHAR(500) NOT NULL,
        Priority INT DEFAULT 1,
        AssignedToUserId INT NULL,
        Status NVARCHAR(50) DEFAULT 'PENDING',
        CreatedAt DATETIME2 DEFAULT GETDATE(),
        ResolvedAt DATETIME2 NULL,
        INDEX IX_DocGen_Alert_FilePath (FilePath),
        INDEX IX_DocGen_Alert_Status (Status),
        INDEX IX_DocGen_Alert_Priority (Priority)
    );
    PRINT '+ Tabela DocGenerator.DocumentationAlerts criada';
END
ELSE
    PRINT '= Tabela DocGenerator.DocumentationAlerts ja existe';
GO

-- DocGenerator.MonitoringConfig
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MonitoringConfig' AND schema_id = SCHEMA_ID('DocGenerator'))
BEGIN
    CREATE TABLE DocGenerator.MonitoringConfig (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ConfigKey NVARCHAR(100) NOT NULL UNIQUE,
        ConfigValue NVARCHAR(500) NOT NULL,
        Description NVARCHAR(500) NULL,
        UpdatedAt DATETIME2 DEFAULT GETDATE()
    );

    INSERT INTO DocGenerator.MonitoringConfig (ConfigKey, ConfigValue, Description) VALUES
    ('ChangeThreshold', '5.0', 'Percentual minimo de mudanca para exigir atualizacao (%)'),
    ('StaleDays', '30', 'Numero de dias para considerar documentacao desatualizada'),
    ('AutoScanInterval', '24', 'Intervalo em horas para varredura automatica'),
    ('NotifyUsers', 'true', 'Notificar usuarios sobre documentacao desatualizada'),
    ('HighPriorityThreshold', '20.0', 'Mudanca > X% = prioridade alta'),
    ('MediumPriorityThreshold', '10.0', 'Mudanca > X% = prioridade media');

    PRINT '+ Tabela DocGenerator.MonitoringConfig criada com dados iniciais';
END
ELSE
    PRINT '= Tabela DocGenerator.MonitoringConfig ja existe';
GO

-- ============================================================================
-- SECAO 3: COLUNAS NOVAS EM TABELAS EXISTENTES (ALTER TABLE)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 3: COLUNAS NOVAS (ALTER TABLE)'
PRINT '============================================================================'
PRINT ''

-- Viagem.TemFichaVistoriaReal (21/01/2026)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Viagem') AND name = 'TemFichaVistoriaReal')
BEGIN
    ALTER TABLE dbo.Viagem ADD TemFichaVistoriaReal BIT NULL;
    PRINT '+ Coluna Viagem.TemFichaVistoriaReal adicionada';

    -- Popula dados existentes
    UPDATE dbo.Viagem SET TemFichaVistoriaReal = 1 WHERE FichaVistoria IS NOT NULL AND TemFichaVistoriaReal IS NULL;
    UPDATE dbo.Viagem SET TemFichaVistoriaReal = 0 WHERE FichaVistoria IS NULL AND TemFichaVistoriaReal IS NULL;
    PRINT '  -> Dados populados automaticamente';
END
ELSE
    PRINT '= Coluna Viagem.TemFichaVistoriaReal ja existe';
GO

-- ============================================================================
-- SECAO 4: INDICES DE PERFORMANCE
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 4: INDICES DE PERFORMANCE'
PRINT '============================================================================'
PRINT ''

-- Indices DocGenerator
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DocGen_FileTracking_NeedsUpdate')
BEGIN
    CREATE INDEX IX_DocGen_FileTracking_NeedsUpdate ON DocGenerator.FileTracking (NeedsUpdate);
    PRINT '+ Indice IX_DocGen_FileTracking_NeedsUpdate criado';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DocGen_FileTracking_LastModified')
BEGIN
    CREATE INDEX IX_DocGen_FileTracking_LastModified ON DocGenerator.FileTracking (LastModified);
    PRINT '+ Indice IX_DocGen_FileTracking_LastModified criado';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DocGen_FileChangeHistory_FilePath_DetectedAt')
BEGIN
    CREATE INDEX IX_DocGen_FileChangeHistory_FilePath_DetectedAt ON DocGenerator.FileChangeHistory (FilePath, DetectedAt DESC);
    PRINT '+ Indice IX_DocGen_FileChangeHistory_FilePath_DetectedAt criado';
END
GO

-- Indices de Eventos
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Viagem_EventoId_Include_Custos' AND object_id = OBJECT_ID('dbo.Viagem'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Viagem_EventoId_Include_Custos ON dbo.Viagem (EventoId)
    INCLUDE (CustoCombustivel, CustoMotorista, CustoVeiculo, CustoOperador, CustoLavador) WHERE EventoId IS NOT NULL;
    PRINT '+ Indice IX_Viagem_EventoId_Include_Custos criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Evento_Nome' AND object_id = OBJECT_ID('dbo.Evento'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Evento_Nome ON dbo.Evento (Nome);
    PRINT '+ Indice IX_Evento_Nome criado';
END
GO

-- Indices Motorista/Veiculo
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Motorista_UnidadeId')
BEGIN
    CREATE INDEX IX_Motorista_UnidadeId ON dbo.Motorista (UnidadeId) INCLUDE (Nome, Status);
    PRINT '+ Indice IX_Motorista_UnidadeId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Veiculo_CombustivelId')
BEGIN
    CREATE INDEX IX_Veiculo_CombustivelId ON dbo.Veiculo (CombustivelId) INCLUDE (Placa, Status);
    PRINT '+ Indice IX_Veiculo_CombustivelId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Veiculo_AtaId')
BEGIN
    CREATE INDEX IX_Veiculo_AtaId ON dbo.Veiculo (AtaId) INCLUDE (Placa, Status);
    PRINT '+ Indice IX_Veiculo_AtaId criado';
END
GO

-- Indices Manutencao/Multa
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Manutencao_VeiculoReservaId')
BEGIN
    CREATE INDEX IX_Manutencao_VeiculoReservaId ON dbo.Manutencao (VeiculoReservaId) WHERE VeiculoReservaId IS NOT NULL;
    PRINT '+ Indice IX_Manutencao_VeiculoReservaId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Multa_ContratoMotoristaId')
BEGIN
    CREATE INDEX IX_Multa_ContratoMotoristaId ON dbo.Multa (ContratoMotoristaId) INCLUDE (Data, ValorAteVencimento);
    PRINT '+ Indice IX_Multa_ContratoMotoristaId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Multa_ContratoVeiculoId')
BEGIN
    CREATE INDEX IX_Multa_ContratoVeiculoId ON dbo.Multa (ContratoVeiculoId) INCLUDE (Data, ValorAteVencimento);
    PRINT '+ Indice IX_Multa_ContratoVeiculoId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Multa_EmpenhoMultaId')
BEGIN
    CREATE INDEX IX_Multa_EmpenhoMultaId ON dbo.Multa (EmpenhoMultaId);
    PRINT '+ Indice IX_Multa_EmpenhoMultaId criado';
END
GO

-- Indices SetorSolicitante/LotacaoMotorista
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SetorSolicitante_SetorPaiId')
BEGIN
    CREATE INDEX IX_SetorSolicitante_SetorPaiId ON dbo.SetorSolicitante (SetorPaiId) WHERE SetorPaiId IS NOT NULL;
    PRINT '+ Indice IX_SetorSolicitante_SetorPaiId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LotacaoMotorista_MotoristaCoberturaId')
BEGIN
    CREATE INDEX IX_LotacaoMotorista_MotoristaCoberturaId ON dbo.LotacaoMotorista (MotoristaCoberturaId) WHERE MotoristaCoberturaId IS NOT NULL;
    PRINT '+ Indice IX_LotacaoMotorista_MotoristaCoberturaId criado';
END
GO

-- Indices NotaFiscal/ItensManutencao
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotaFiscal_VeiculoId')
BEGIN
    CREATE INDEX IX_NotaFiscal_VeiculoId ON dbo.NotaFiscal (VeiculoId) WHERE VeiculoId IS NOT NULL;
    PRINT '+ Indice IX_NotaFiscal_VeiculoId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ItensManutencao_MotoristaId')
BEGIN
    CREATE INDEX IX_ItensManutencao_MotoristaId ON dbo.ItensManutencao (MotoristaId) WHERE MotoristaId IS NOT NULL;
    PRINT '+ Indice IX_ItensManutencao_MotoristaId criado';
END
GO

-- Indices WhatsApp
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppMensagens_InstanciaId')
BEGIN
    CREATE INDEX IX_WhatsAppMensagens_InstanciaId ON dbo.WhatsAppMensagens (InstanciaId);
    PRINT '+ Indice IX_WhatsAppMensagens_InstanciaId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppMensagens_ContatoId')
BEGIN
    CREATE INDEX IX_WhatsAppMensagens_ContatoId ON dbo.WhatsAppMensagens (ContatoId);
    PRINT '+ Indice IX_WhatsAppMensagens_ContatoId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppMensagens_UsuarioId')
BEGIN
    CREATE INDEX IX_WhatsAppMensagens_UsuarioId ON dbo.WhatsAppMensagens (UsuarioId);
    PRINT '+ Indice IX_WhatsAppMensagens_UsuarioId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppFilaMensagens_MensagemId')
BEGIN
    CREATE INDEX IX_WhatsAppFilaMensagens_MensagemId ON dbo.WhatsAppFilaMensagens (MensagemId);
    PRINT '+ Indice IX_WhatsAppFilaMensagens_MensagemId criado';
END
GO

-- Indices Patrimonio
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Patrimonio_SetorConferenciaId')
BEGIN
    CREATE INDEX IX_Patrimonio_SetorConferenciaId ON dbo.Patrimonio (SetorConferenciaId) WHERE SetorConferenciaId IS NOT NULL;
    PRINT '+ Indice IX_Patrimonio_SetorConferenciaId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Patrimonio_SecaoConferenciaId')
BEGIN
    CREATE INDEX IX_Patrimonio_SecaoConferenciaId ON dbo.Patrimonio (SecaoConferenciaId) WHERE SecaoConferenciaId IS NOT NULL;
    PRINT '+ Indice IX_Patrimonio_SecaoConferenciaId criado';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ItemVeiculoAta_RepactuacaoAtaId')
BEGIN
    CREATE INDEX IX_ItemVeiculoAta_RepactuacaoAtaId ON dbo.ItemVeiculoAta (RepactuacaoAtaId) WHERE RepactuacaoAtaId IS NOT NULL;
    PRINT '+ Indice IX_ItemVeiculoAta_RepactuacaoAtaId criado';
END
GO

-- ============================================================================
-- SECAO 5: FOREIGN KEYS (DROP + CREATE)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 5: FOREIGN KEYS'
PRINT '============================================================================'
PRINT ''

-- FK Viagem.UsuarioIdCriacao
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Viagem_UsuarioIdCriacao')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Viagem v LEFT JOIN AspNetUsers u ON v.UsuarioIdCriacao = u.Id WHERE v.UsuarioIdCriacao IS NOT NULL AND u.Id IS NULL)
    BEGIN
        ALTER TABLE dbo.Viagem WITH NOCHECK ADD CONSTRAINT FK_Viagem_UsuarioIdCriacao FOREIGN KEY (UsuarioIdCriacao) REFERENCES dbo.AspNetUsers (Id);
        PRINT '+ FK_Viagem_UsuarioIdCriacao criada';
    END
    ELSE PRINT '! FK_Viagem_UsuarioIdCriacao - dados orfaos detectados';
END
GO

-- FK Viagem.UsuarioIdFinalizacao
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Viagem_UsuarioIdFinalizacao')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Viagem v LEFT JOIN AspNetUsers u ON v.UsuarioIdFinalizacao = u.Id WHERE v.UsuarioIdFinalizacao IS NOT NULL AND u.Id IS NULL)
    BEGIN
        ALTER TABLE dbo.Viagem WITH NOCHECK ADD CONSTRAINT FK_Viagem_UsuarioIdFinalizacao FOREIGN KEY (UsuarioIdFinalizacao) REFERENCES dbo.AspNetUsers (Id);
        PRINT '+ FK_Viagem_UsuarioIdFinalizacao criada';
    END
    ELSE PRINT '! FK_Viagem_UsuarioIdFinalizacao - dados orfaos detectados';
END
GO

-- FK Viagem.RecorrenciaViagemId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Viagem_RecorrenciaViagemId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Viagem v LEFT JOIN Viagem v2 ON v.RecorrenciaViagemId = v2.ViagemId WHERE v.RecorrenciaViagemId IS NOT NULL AND v2.ViagemId IS NULL)
    BEGIN
        ALTER TABLE dbo.Viagem WITH NOCHECK ADD CONSTRAINT FK_Viagem_RecorrenciaViagemId FOREIGN KEY (RecorrenciaViagemId) REFERENCES dbo.Viagem (ViagemId);
        PRINT '+ FK_Viagem_RecorrenciaViagemId criada';
    END
    ELSE PRINT '! FK_Viagem_RecorrenciaViagemId - dados orfaos detectados';
END
GO

-- FK Manutencao
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCriacao')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Manutencao m LEFT JOIN AspNetUsers u ON m.IdUsuarioCriacao = u.Id WHERE m.IdUsuarioCriacao IS NOT NULL AND u.Id IS NULL)
    BEGIN
        ALTER TABLE dbo.Manutencao WITH NOCHECK ADD CONSTRAINT FK_Manutencao_IdUsuarioCriacao FOREIGN KEY (IdUsuarioCriacao) REFERENCES dbo.AspNetUsers (Id);
        PRINT '+ FK_Manutencao_IdUsuarioCriacao criada';
    END
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioFinalizacao')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Manutencao m LEFT JOIN AspNetUsers u ON m.IdUsuarioFinalizacao = u.Id WHERE m.IdUsuarioFinalizacao IS NOT NULL AND u.Id IS NULL)
    BEGIN
        ALTER TABLE dbo.Manutencao WITH NOCHECK ADD CONSTRAINT FK_Manutencao_IdUsuarioFinalizacao FOREIGN KEY (IdUsuarioFinalizacao) REFERENCES dbo.AspNetUsers (Id);
        PRINT '+ FK_Manutencao_IdUsuarioFinalizacao criada';
    END
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCancelamento')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Manutencao m LEFT JOIN AspNetUsers u ON m.IdUsuarioCancelamento = u.Id WHERE m.IdUsuarioCancelamento IS NOT NULL AND u.Id IS NULL)
    BEGIN
        ALTER TABLE dbo.Manutencao WITH NOCHECK ADD CONSTRAINT FK_Manutencao_IdUsuarioCancelamento FOREIGN KEY (IdUsuarioCancelamento) REFERENCES dbo.AspNetUsers (Id);
        PRINT '+ FK_Manutencao_IdUsuarioCancelamento criada';
    END
END
GO

-- FK Motorista
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Motorista_UnidadeId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Motorista m LEFT JOIN Unidade u ON m.UnidadeId = u.UnidadeId WHERE m.UnidadeId IS NOT NULL AND u.UnidadeId IS NULL)
    BEGIN
        ALTER TABLE dbo.Motorista WITH NOCHECK ADD CONSTRAINT FK_Motorista_UnidadeId FOREIGN KEY (UnidadeId) REFERENCES dbo.Unidade (UnidadeId);
        PRINT '+ FK_Motorista_UnidadeId criada';
    END
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Motorista_CondutorId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Motorista m LEFT JOIN CondutorApoio c ON m.CondutorId = c.CondutorId WHERE m.CondutorId IS NOT NULL AND c.CondutorId IS NULL)
    BEGIN
        ALTER TABLE dbo.Motorista WITH NOCHECK ADD CONSTRAINT FK_Motorista_CondutorId FOREIGN KEY (CondutorId) REFERENCES dbo.CondutorApoio (CondutorId);
        PRINT '+ FK_Motorista_CondutorId criada';
    END
END
GO

-- FK Veiculo
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Veiculo_CombustivelId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Veiculo v LEFT JOIN Combustivel c ON v.CombustivelId = c.CombustivelId WHERE v.CombustivelId IS NOT NULL AND c.CombustivelId IS NULL)
    BEGIN
        ALTER TABLE dbo.Veiculo WITH NOCHECK ADD CONSTRAINT FK_Veiculo_CombustivelId FOREIGN KEY (CombustivelId) REFERENCES dbo.Combustivel (CombustivelId);
        PRINT '+ FK_Veiculo_CombustivelId criada';
    END
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Veiculo_ContratoId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Veiculo v LEFT JOIN Contrato c ON v.ContratoId = c.ContratoId WHERE v.ContratoId IS NOT NULL AND c.ContratoId IS NULL)
    BEGIN
        ALTER TABLE dbo.Veiculo WITH NOCHECK ADD CONSTRAINT FK_Veiculo_ContratoId FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);
        PRINT '+ FK_Veiculo_ContratoId criada';
    END
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Veiculo_AtaId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Veiculo v LEFT JOIN AtaRegistroPrecos a ON v.AtaId = a.AtaId WHERE v.AtaId IS NOT NULL AND a.AtaId IS NULL)
    BEGIN
        ALTER TABLE dbo.Veiculo WITH NOCHECK ADD CONSTRAINT FK_Veiculo_AtaId FOREIGN KEY (AtaId) REFERENCES dbo.AtaRegistroPrecos (AtaId);
        PRINT '+ FK_Veiculo_AtaId criada';
    END
END
GO

-- FK SetorSolicitante
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SetorSolicitante_SetorPaiId')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM SetorSolicitante s LEFT JOIN SetorSolicitante s2 ON s.SetorPaiId = s2.SetorSolicitanteId WHERE s.SetorPaiId IS NOT NULL AND s2.SetorSolicitanteId IS NULL)
    BEGIN
        ALTER TABLE dbo.SetorSolicitante WITH NOCHECK ADD CONSTRAINT FK_SetorSolicitante_SetorPaiId FOREIGN KEY (SetorPaiId) REFERENCES dbo.SetorSolicitante (SetorSolicitanteId);
        PRINT '+ FK_SetorSolicitante_SetorPaiId criada';
    END
END
GO

-- FK WhatsApp
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WhatsAppMensagens_InstanciaId')
BEGIN
    ALTER TABLE dbo.WhatsAppMensagens WITH NOCHECK ADD CONSTRAINT FK_WhatsAppMensagens_InstanciaId FOREIGN KEY (InstanciaId) REFERENCES dbo.WhatsAppInstancias (Id);
    PRINT '+ FK_WhatsAppMensagens_InstanciaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WhatsAppMensagens_ContatoId')
BEGIN
    ALTER TABLE dbo.WhatsAppMensagens WITH NOCHECK ADD CONSTRAINT FK_WhatsAppMensagens_ContatoId FOREIGN KEY (ContatoId) REFERENCES dbo.WhatsAppContatos (Id);
    PRINT '+ FK_WhatsAppMensagens_ContatoId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WhatsAppFilaMensagens_MensagemId')
BEGIN
    ALTER TABLE dbo.WhatsAppFilaMensagens WITH NOCHECK ADD CONSTRAINT FK_WhatsAppFilaMensagens_MensagemId FOREIGN KEY (MensagemId) REFERENCES dbo.WhatsAppMensagens (Id);
    PRINT '+ FK_WhatsAppFilaMensagens_MensagemId criada';
END
GO

-- FK Outras Tabelas
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItemVeiculoAta_RepactuacaoAtaId')
BEGIN
    ALTER TABLE dbo.ItemVeiculoAta WITH NOCHECK ADD CONSTRAINT FK_ItemVeiculoAta_RepactuacaoAtaId FOREIGN KEY (RepactuacaoAtaId) REFERENCES dbo.RepactuacaoAta (RepactuacaoAtaId);
    PRINT '+ FK_ItemVeiculoAta_RepactuacaoAtaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_NotaFiscal_VeiculoId')
BEGIN
    ALTER TABLE dbo.NotaFiscal WITH NOCHECK ADD CONSTRAINT FK_NotaFiscal_VeiculoId FOREIGN KEY (VeiculoId) REFERENCES dbo.Veiculo (VeiculoId);
    PRINT '+ FK_NotaFiscal_VeiculoId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DocumentoContrato_ContratoId')
BEGIN
    ALTER TABLE dbo.DocumentoContrato WITH NOCHECK ADD CONSTRAINT FK_DocumentoContrato_ContratoId FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);
    PRINT '+ FK_DocumentoContrato_ContratoId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_LotacaoMotorista_MotoristaCoberturaId')
BEGIN
    ALTER TABLE dbo.LotacaoMotorista WITH NOCHECK ADD CONSTRAINT FK_LotacaoMotorista_MotoristaCoberturaId FOREIGN KEY (MotoristaCoberturaId) REFERENCES dbo.Motorista (MotoristaId);
    PRINT '+ FK_LotacaoMotorista_MotoristaCoberturaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_MovimentacaoEmpenhoMulta_MultaId')
BEGIN
    ALTER TABLE dbo.MovimentacaoEmpenhoMulta WITH NOCHECK ADD CONSTRAINT FK_MovimentacaoEmpenhoMulta_MultaId FOREIGN KEY (MultaId) REFERENCES dbo.Multa (MultaId);
    PRINT '+ FK_MovimentacaoEmpenhoMulta_MultaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItensManutencao_MotoristaId')
BEGIN
    ALTER TABLE dbo.ItensManutencao WITH NOCHECK ADD CONSTRAINT FK_ItensManutencao_MotoristaId FOREIGN KEY (MotoristaId) REFERENCES dbo.Motorista (MotoristaId);
    PRINT '+ FK_ItensManutencao_MotoristaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Patrimonio_SetorConferenciaId')
BEGIN
    ALTER TABLE dbo.Patrimonio WITH NOCHECK ADD CONSTRAINT FK_Patrimonio_SetorConferenciaId FOREIGN KEY (SetorConferenciaId) REFERENCES dbo.SetorPatrimonial (SetorId);
    PRINT '+ FK_Patrimonio_SetorConferenciaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Patrimonio_SecaoConferenciaId')
BEGIN
    ALTER TABLE dbo.Patrimonio WITH NOCHECK ADD CONSTRAINT FK_Patrimonio_SecaoConferenciaId FOREIGN KEY (SecaoConferenciaId) REFERENCES dbo.SecaoPatrimonial (SecaoId);
    PRINT '+ FK_Patrimonio_SecaoConferenciaId criada';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_RepactuacaoServicos_RepactuacaoContratoId')
BEGIN
    ALTER TABLE dbo.RepactuacaoServicos WITH NOCHECK ADD CONSTRAINT FK_RepactuacaoServicos_RepactuacaoContratoId FOREIGN KEY (RepactuacaoContratoId) REFERENCES dbo.RepactuacaoContrato (RepactuacaoContratoId);
    PRINT '+ FK_RepactuacaoServicos_RepactuacaoContratoId criada';
END
GO

-- ============================================================================
-- SECAO 6: VIEWS (DROP + CREATE)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 6: VIEWS (DROP + CREATE)'
PRINT '============================================================================'
PRINT ''

-- DROP e CREATE DocGenerator.vw_FilesNeedingUpdate
IF OBJECT_ID('DocGenerator.vw_FilesNeedingUpdate', 'V') IS NOT NULL
    DROP VIEW DocGenerator.vw_FilesNeedingUpdate;
GO

CREATE VIEW DocGenerator.vw_FilesNeedingUpdate AS
SELECT
    ft.FilePath, ft.FileSize, ft.LineCount, ft.CharacterCount,
    ft.LastModified, ft.LastDocumented,
    DATEDIFF(DAY, ft.LastDocumented, GETDATE()) AS DaysSinceDocumented,
    ft.UpdateReason, da.AlertType, da.Priority
FROM DocGenerator.FileTracking ft
LEFT JOIN DocGenerator.DocumentationAlerts da ON ft.FilePath = da.FilePath AND da.Status = 'PENDING'
WHERE ft.NeedsUpdate = 1 OR ft.LastDocumented IS NULL OR DATEDIFF(DAY, ft.LastDocumented, GETDATE()) > 30;
GO
PRINT '+ View DocGenerator.vw_FilesNeedingUpdate criada';
GO

-- DROP e CREATE ViewViagensAgenda
IF OBJECT_ID('dbo.ViewViagensAgenda', 'V') IS NOT NULL
    DROP VIEW dbo.ViewViagensAgenda;
GO

CREATE VIEW dbo.ViewViagensAgenda AS
SELECT
    v.ViagemId, v.MotoristaId, v.VeiculoId, v.EventoId,
    v.DataInicial, v.HoraInicio, v.DataFinal, v.HoraFim,
    CASE WHEN v.DataInicial IS NOT NULL AND v.HoraInicio IS NOT NULL
        THEN DATEADD(SECOND, DATEDIFF(SECOND, 0, CAST(v.HoraInicio AS TIME)), CAST(v.DataInicial AS DATETIME))
        ELSE v.DataInicial END AS [Start],
    v.HoraFim AS [End],
    v.Status, v.StatusAgendamento, v.FoiAgendamento, v.Finalidade,
    v.NomeEvento, e.Nome AS NomeEventoFull,
    m.Nome AS NomeMotorista, vec.Placa,
    CASE WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL THEN 'Evento: ' + e.Nome ELSE v.Finalidade END AS Titulo,
    CASE
        WHEN v.Finalidade = 'Evento' THEN '#A39481'
        WHEN v.Status = 'Cancelada' THEN '#722F37'
        WHEN v.Status = 'Realizada' THEN '#154c62'
        WHEN v.Status = 'Agendada' THEN '#FFA726'
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 1 THEN '#FFA726'
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 0 THEN '#476b47'
        ELSE '#6c757d'
    END AS CorEvento,
    '#FFFFFF' AS CorTexto,
    ISNULL(m.Nome, '(Motorista Nao Informado)') + ' - (' + ISNULL(vec.Placa, 'Sem Veiculo') + ')' +
        CASE WHEN v.DescricaoSemFormato IS NOT NULL AND LEN(CAST(v.DescricaoSemFormato AS NVARCHAR(MAX))) > 0
            THEN ' - ' + CAST(v.DescricaoSemFormato AS NVARCHAR(500)) ELSE '' END AS DescricaoMontada,
    CASE
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL AND v.Status = 'Cancelada'
            THEN 'Evento CANCELADO: ' + e.Nome + ' / ' + ISNULL(m.Nome, '(Motorista Nao Identificado)') + ' - (' + ISNULL(vec.Placa, 'Sem Veiculo') + ')'
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome + ' / ' + ISNULL(m.Nome, '(Motorista Nao Identificado)') + ' - (' + ISNULL(vec.Placa, 'Sem Veiculo') + ')'
        ELSE NULL
    END AS DescricaoEvento,
    CASE WHEN v.DescricaoSemFormato IS NOT NULL AND LEN(CAST(v.DescricaoSemFormato AS NVARCHAR(MAX))) > 0
        THEN CAST(v.DescricaoSemFormato AS NVARCHAR(500)) ELSE NULL END AS Descricao
FROM dbo.Viagem v
LEFT JOIN dbo.Motorista m ON v.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo vec ON v.VeiculoId = vec.VeiculoId
LEFT JOIN dbo.Evento e ON v.EventoId = e.EventoId
WHERE v.DataInicial IS NOT NULL AND v.HoraInicio IS NOT NULL;
GO
PRINT '+ View dbo.ViewViagensAgenda criada';
GO

-- ============================================================================
-- SECAO 7: STORED PROCEDURES (DROP + CREATE)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 7: STORED PROCEDURES (DROP + CREATE)'
PRINT '============================================================================'
PRINT ''

-- DROP e CREATE DocGenerator.sp_DetectFileChanges
IF OBJECT_ID('DocGenerator.sp_DetectFileChanges', 'P') IS NOT NULL
    DROP PROCEDURE DocGenerator.sp_DetectFileChanges;
GO

CREATE PROCEDURE DocGenerator.sp_DetectFileChanges
    @FilePath NVARCHAR(500), @NewHash NVARCHAR(64), @NewSize INT, @NewLineCount INT, @NewCharCount INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @OldHash NVARCHAR(64), @OldSize INT, @ChangePercentage DECIMAL(5,2), @ChangeType NVARCHAR(50), @NeedsUpdate BIT = 0, @UpdateReason NVARCHAR(200);

    SELECT @OldHash = FileHash, @OldSize = FileSize FROM DocGenerator.FileTracking WHERE FilePath = @FilePath;

    IF @OldHash IS NULL
    BEGIN
        SET @ChangeType = 'ADDED'; SET @NeedsUpdate = 1; SET @UpdateReason = 'Arquivo novo detectado';
        INSERT INTO DocGenerator.FileTracking (FilePath, FileHash, FileSize, LineCount, CharacterCount, LastModified, NeedsUpdate, UpdateReason)
        VALUES (@FilePath, @NewHash, @NewSize, @NewLineCount, @NewCharCount, GETDATE(), 1, @UpdateReason);
    END
    ELSE IF @OldHash != @NewHash
    BEGIN
        SET @ChangeType = 'MODIFIED';
        SET @ChangePercentage = ABS(@NewSize - @OldSize) * 100.0 / NULLIF(@OldSize, 0);

        DECLARE @Threshold DECIMAL(5,2);
        SELECT @Threshold = CAST(ConfigValue AS DECIMAL(5,2)) FROM DocGenerator.MonitoringConfig WHERE ConfigKey = 'ChangeThreshold';

        IF @ChangePercentage >= ISNULL(@Threshold, 5.0)
        BEGIN
            SET @NeedsUpdate = 1; SET @UpdateReason = CONCAT('Mudanca de ', FORMAT(@ChangePercentage, 'N2'), '% detectada');
            DECLARE @Priority INT = 1, @HighThreshold DECIMAL(5,2), @MediumThreshold DECIMAL(5,2);
            SELECT @HighThreshold = CAST(ConfigValue AS DECIMAL(5,2)) FROM DocGenerator.MonitoringConfig WHERE ConfigKey = 'HighPriorityThreshold';
            SELECT @MediumThreshold = CAST(ConfigValue AS DECIMAL(5,2)) FROM DocGenerator.MonitoringConfig WHERE ConfigKey = 'MediumPriorityThreshold';
            IF @ChangePercentage >= ISNULL(@HighThreshold, 20.0) SET @Priority = 3;
            ELSE IF @ChangePercentage >= ISNULL(@MediumThreshold, 10.0) SET @Priority = 2;
            INSERT INTO DocGenerator.DocumentationAlerts (FilePath, AlertType, AlertMessage, Priority) VALUES (@FilePath, 'NEEDS_UPDATE', @UpdateReason, @Priority);
        END

        UPDATE DocGenerator.FileTracking SET FileHash = @NewHash, FileSize = @NewSize, LineCount = @NewLineCount, CharacterCount = @NewCharCount,
            LastModified = GETDATE(), NeedsUpdate = @NeedsUpdate, UpdateReason = @UpdateReason, UpdatedAt = GETDATE() WHERE FilePath = @FilePath;
    END

    INSERT INTO DocGenerator.FileChangeHistory (FilePath, OldHash, NewHash, OldSize, NewSize, ChangeType, ChangePercentage)
    VALUES (@FilePath, @OldHash, @NewHash, @OldSize, @NewSize, @ChangeType, @ChangePercentage);

    SELECT @NeedsUpdate AS NeedsUpdate, @UpdateReason AS UpdateReason;
END
GO
PRINT '+ Stored Procedure DocGenerator.sp_DetectFileChanges criada';
GO

-- DROP e CREATE DocGenerator.sp_MarkAsDocumented
IF OBJECT_ID('DocGenerator.sp_MarkAsDocumented', 'P') IS NOT NULL
    DROP PROCEDURE DocGenerator.sp_MarkAsDocumented;
GO

CREATE PROCEDURE DocGenerator.sp_MarkAsDocumented @FilePath NVARCHAR(500), @DocumentationVersion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DocGenerator.FileTracking SET LastDocumented = GETDATE(), NeedsUpdate = 0, UpdateReason = NULL,
        DocumentationVersion = ISNULL(@DocumentationVersion, DocumentationVersion + 1), UpdatedAt = GETDATE() WHERE FilePath = @FilePath;
    UPDATE DocGenerator.DocumentationAlerts SET Status = 'RESOLVED', ResolvedAt = GETDATE() WHERE FilePath = @FilePath AND Status = 'PENDING';
END
GO
PRINT '+ Stored Procedure DocGenerator.sp_MarkAsDocumented criada';
GO

-- DROP e CREATE DocGenerator.sp_RunAutoScan
IF OBJECT_ID('DocGenerator.sp_RunAutoScan', 'P') IS NOT NULL
    DROP PROCEDURE DocGenerator.sp_RunAutoScan;
GO

CREATE PROCEDURE DocGenerator.sp_RunAutoScan
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StaleDays INT;
    SELECT @StaleDays = CAST(ConfigValue AS INT) FROM DocGenerator.MonitoringConfig WHERE ConfigKey = 'StaleDays';

    UPDATE ft SET ft.NeedsUpdate = 1, ft.UpdateReason = CONCAT('Documentacao desatualizada (', DATEDIFF(DAY, ft.LastDocumented, GETDATE()), ' dias)')
    FROM DocGenerator.FileTracking ft
    WHERE ft.LastDocumented IS NOT NULL AND DATEDIFF(DAY, ft.LastDocumented, GETDATE()) > ISNULL(@StaleDays, 30) AND ft.NeedsUpdate = 0;

    INSERT INTO DocGenerator.DocumentationAlerts (FilePath, AlertType, AlertMessage, Priority)
    SELECT ft.FilePath, 'STALE', CONCAT('Documentacao desatualizada ha ', DATEDIFF(DAY, ft.LastDocumented, GETDATE()), ' dias'), 2
    FROM DocGenerator.FileTracking ft
    WHERE ft.NeedsUpdate = 1 AND ft.UpdateReason LIKE '%desatualizada%'
        AND NOT EXISTS (SELECT 1 FROM DocGenerator.DocumentationAlerts da WHERE da.FilePath = ft.FilePath AND da.Status = 'PENDING');
END
GO
PRINT '+ Stored Procedure DocGenerator.sp_RunAutoScan criada';
GO

-- ============================================================================
-- SECAO 8: TRIGGERS (DROP + CREATE)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 8: TRIGGERS (DROP + CREATE)'
PRINT '============================================================================'
PRINT ''

-- (Adicionar triggers conforme necessario)
PRINT '= Nenhum trigger definido nesta versao';
GO

-- ============================================================================
-- SECAO 9: FUNCTIONS (DROP + CREATE)
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'SECAO 9: FUNCTIONS (DROP + CREATE)'
PRINT '============================================================================'
PRINT ''

-- (Adicionar functions conforme necessario)
PRINT '= Nenhuma function definida nesta versao';
GO

-- ============================================================================
-- FINALIZACAO
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'FROTIX BD SCRIPTS - Execucao Concluida'
PRINT '============================================================================'
PRINT ''
PRINT 'Data/Hora de Conclusao: ' + CONVERT(VARCHAR, GETDATE(), 120)
PRINT ''
PRINT 'NOTA: Jobs estao em arquivo separado (FrotiXDBJobs.sql)'
PRINT '      Execute no servidor de producao apenas.'
PRINT '============================================================================'
GO
