-- ============================================================================
-- SQL PARA ADICIONAR FOREIGN KEYS FALTANTES - CORRIGIDO
-- ============================================================================
--
-- VERSÃO CORRIGIDA baseada nos erros encontrados na execução
--
-- Data de Criação: 12/01/2026
-- Gerado por: Claude Code
--
-- CORREÇÕES APLICADAS:
-- 1. WhatsAppInstancias.Id (não InstanciaId)
-- 2. WhatsAppContatos.Id (não ContatoId)
-- 3. WhatsAppMensagens.Id (não MensagemId)
-- 4. REMOVIDO: WhatsAppMensagens.UsuarioId → AspNetUsers.Id (tipos incompatíveis)
-- 5. REMOVIDO: WhatsAppWebhookLogs.InstanciaId (campo não existe)
-- 6. REMOVIDO: ItemVeiculoAta.VeiculoId (campo não existe)
--
-- IMPORTANTE:
-- - Execute este script em ambiente de DESENVOLVIMENTO primeiro
-- - Faça BACKUP do banco antes de executar em PRODUÇÃO
--
-- ============================================================================

USE [FrotiX]
GO

PRINT '============================================================================'
PRINT 'FOREIGN KEYS FALTANTES - IMPLEMENTAÇÃO CORRIGIDA'
PRINT '============================================================================'
PRINT ''
PRINT 'Data de Execução: ' + CONVERT(varchar, GETDATE(), 120)
PRINT ''

-- ============================================================================
-- PRIORIDADE ALTA (Tabelas Críticas)
-- ============================================================================
PRINT '============================================================================'
PRINT '[1/3] FOREIGN KEYS - PRIORIDADE ALTA'
PRINT '============================================================================'
PRINT ''

-- Tabela: Viagem
-- Campo: RecorrenciaViagemId (auto-referência para viagens recorrentes)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Viagem_RecorrenciaViagemId')
BEGIN
    PRINT 'Criando FK_Viagem_RecorrenciaViagemId...'

    -- Verificar se há dados órfãos
    IF EXISTS (
        SELECT 1 FROM Viagem v
        LEFT JOIN Viagem v2 ON v.RecorrenciaViagemId = v2.ViagemId
        WHERE v.RecorrenciaViagemId IS NOT NULL AND v2.ViagemId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem viagens com RecorrenciaViagemId inválido!'
        PRINT '   Execute a query abaixo para ver os registros problemáticos:'
        PRINT '   SELECT ViagemId, RecorrenciaViagemId FROM Viagem v'
        PRINT '   LEFT JOIN Viagem v2 ON v.RecorrenciaViagemId = v2.ViagemId'
        PRINT '   WHERE v.RecorrenciaViagemId IS NOT NULL AND v2.ViagemId IS NULL'
        PRINT ''
        PRINT '   Considere corrigir os dados antes de criar a FK'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Viagem WITH NOCHECK
            ADD CONSTRAINT FK_Viagem_RecorrenciaViagemId
            FOREIGN KEY (RecorrenciaViagemId)
            REFERENCES dbo.Viagem (ViagemId)

        PRINT '✅ FK_Viagem_RecorrenciaViagemId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Viagem_RecorrenciaViagemId já existe'
END
GO

PRINT ''
PRINT '============================================================================'
PRINT '[2/3] FOREIGN KEYS - PRIORIDADE MÉDIA'
PRINT '============================================================================'
PRINT ''

-- ============================================================================
-- Tabela: Motorista
-- ============================================================================

-- Campo: UnidadeId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Motorista_UnidadeId')
BEGIN
    PRINT 'Criando FK_Motorista_UnidadeId...'

    IF EXISTS (
        SELECT 1 FROM Motorista m
        LEFT JOIN Unidade u ON m.UnidadeId = u.UnidadeId
        WHERE m.UnidadeId IS NOT NULL AND u.UnidadeId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem motoristas com UnidadeId inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Motorista WITH NOCHECK
            ADD CONSTRAINT FK_Motorista_UnidadeId
            FOREIGN KEY (UnidadeId)
            REFERENCES dbo.Unidade (UnidadeId)

        PRINT '✅ FK_Motorista_UnidadeId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Motorista_UnidadeId já existe'
END
GO

-- Campo: CondutorId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Motorista_CondutorId')
BEGIN
    PRINT 'Criando FK_Motorista_CondutorId...'

    IF EXISTS (
        SELECT 1 FROM Motorista m
        LEFT JOIN CondutorApoio c ON m.CondutorId = c.CondutorId
        WHERE m.CondutorId IS NOT NULL AND c.CondutorId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem motoristas com CondutorId inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Motorista WITH NOCHECK
            ADD CONSTRAINT FK_Motorista_CondutorId
            FOREIGN KEY (CondutorId)
            REFERENCES dbo.CondutorApoio (CondutorId)

        PRINT '✅ FK_Motorista_CondutorId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Motorista_CondutorId já existe'
END
GO

-- ============================================================================
-- Tabela: Veiculo
-- ============================================================================

-- Campo: CombustivelId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Veiculo_CombustivelId')
BEGIN
    PRINT 'Criando FK_Veiculo_CombustivelId...'

    IF EXISTS (
        SELECT 1 FROM Veiculo v
        LEFT JOIN Combustivel c ON v.CombustivelId = c.CombustivelId
        WHERE v.CombustivelId IS NOT NULL AND c.CombustivelId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem veículos com CombustivelId inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Veiculo WITH NOCHECK
            ADD CONSTRAINT FK_Veiculo_CombustivelId
            FOREIGN KEY (CombustivelId)
            REFERENCES dbo.Combustivel (CombustivelId)

        PRINT '✅ FK_Veiculo_CombustivelId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Veiculo_CombustivelId já existe'
END
GO

-- Campo: ContratoId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Veiculo_ContratoId')
BEGIN
    PRINT 'Criando FK_Veiculo_ContratoId...'

    IF EXISTS (
        SELECT 1 FROM Veiculo v
        LEFT JOIN Contrato c ON v.ContratoId = c.ContratoId
        WHERE v.ContratoId IS NOT NULL AND c.ContratoId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem veículos com ContratoId inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Veiculo WITH NOCHECK
            ADD CONSTRAINT FK_Veiculo_ContratoId
            FOREIGN KEY (ContratoId)
            REFERENCES dbo.Contrato (ContratoId)

        PRINT '✅ FK_Veiculo_ContratoId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Veiculo_ContratoId já existe'
END
GO

-- Campo: AtaId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Veiculo_AtaId')
BEGIN
    PRINT 'Criando FK_Veiculo_AtaId...'

    IF EXISTS (
        SELECT 1 FROM Veiculo v
        LEFT JOIN AtaRegistroPrecos a ON v.AtaId = a.AtaId
        WHERE v.AtaId IS NOT NULL AND a.AtaId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem veículos com AtaId inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Veiculo WITH NOCHECK
            ADD CONSTRAINT FK_Veiculo_AtaId
            FOREIGN KEY (AtaId)
            REFERENCES dbo.AtaRegistroPrecos (AtaId)

        PRINT '✅ FK_Veiculo_AtaId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Veiculo_AtaId já existe'
END
GO

-- ============================================================================
-- Tabela: SetorSolicitante
-- ============================================================================

-- Campo: SetorPaiId (auto-referência para hierarquia)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_SetorSolicitante_SetorPaiId')
BEGIN
    PRINT 'Criando FK_SetorSolicitante_SetorPaiId...'

    IF EXISTS (
        SELECT 1 FROM SetorSolicitante s
        LEFT JOIN SetorSolicitante s2 ON s.SetorPaiId = s2.SetorSolicitanteId
        WHERE s.SetorPaiId IS NOT NULL AND s2.SetorSolicitanteId IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem setores com SetorPaiId inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.SetorSolicitante WITH NOCHECK
            ADD CONSTRAINT FK_SetorSolicitante_SetorPaiId
            FOREIGN KEY (SetorPaiId)
            REFERENCES dbo.SetorSolicitante (SetorSolicitanteId)

        PRINT '✅ FK_SetorSolicitante_SetorPaiId criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_SetorSolicitante_SetorPaiId já existe'
END
GO

PRINT ''
PRINT '============================================================================'
PRINT '[3/3] FOREIGN KEYS - PRIORIDADE BAIXA'
PRINT '============================================================================'
PRINT ''

-- ============================================================================
-- Tabela: WhatsAppMensagens
-- ============================================================================

-- CORRIGIDO: InstanciaId → WhatsAppInstancias.Id (não InstanciaId)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WhatsAppMensagens_InstanciaId')
BEGIN
    ALTER TABLE dbo.WhatsAppMensagens WITH NOCHECK
        ADD CONSTRAINT FK_WhatsAppMensagens_InstanciaId
        FOREIGN KEY (InstanciaId)
        REFERENCES dbo.WhatsAppInstancias (Id)  -- CORRIGIDO: Id (não InstanciaId)
    PRINT '✅ FK_WhatsAppMensagens_InstanciaId criada'
END
ELSE PRINT '✓  FK_WhatsAppMensagens_InstanciaId já existe'
GO

-- CORRIGIDO: ContatoId → WhatsAppContatos.Id (não ContatoId)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WhatsAppMensagens_ContatoId')
BEGIN
    ALTER TABLE dbo.WhatsAppMensagens WITH NOCHECK
        ADD CONSTRAINT FK_WhatsAppMensagens_ContatoId
        FOREIGN KEY (ContatoId)
        REFERENCES dbo.WhatsAppContatos (Id)  -- CORRIGIDO: Id (não ContatoId)
    PRINT '✅ FK_WhatsAppMensagens_ContatoId criada'
END
ELSE PRINT '✓  FK_WhatsAppMensagens_ContatoId já existe'
GO

-- REMOVIDO: WhatsAppMensagens.UsuarioId → AspNetUsers.Id
-- MOTIVO: Tipos incompatíveis (int vs nvarchar(450))

-- ============================================================================
-- Tabela: WhatsAppFilaMensagens
-- ============================================================================

-- CORRIGIDO: MensagemId → WhatsAppMensagens.Id (não MensagemId)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WhatsAppFilaMensagens_MensagemId')
BEGIN
    ALTER TABLE dbo.WhatsAppFilaMensagens WITH NOCHECK
        ADD CONSTRAINT FK_WhatsAppFilaMensagens_MensagemId
        FOREIGN KEY (MensagemId)
        REFERENCES dbo.WhatsAppMensagens (Id)  -- CORRIGIDO: Id (não MensagemId)
    PRINT '✅ FK_WhatsAppFilaMensagens_MensagemId criada'
END
ELSE PRINT '✓  FK_WhatsAppFilaMensagens_MensagemId já existe'
GO

-- ============================================================================
-- Tabela: WhatsAppWebhookLogs
-- ============================================================================

-- REMOVIDO: WhatsAppWebhookLogs.InstanciaId
-- MOTIVO: Campo InstanciaId não existe nesta tabela

-- ============================================================================
-- Tabela: ItemVeiculoAta
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItemVeiculoAta_RepactuacaoAtaId')
BEGIN
    ALTER TABLE dbo.ItemVeiculoAta WITH NOCHECK
        ADD CONSTRAINT FK_ItemVeiculoAta_RepactuacaoAtaId
        FOREIGN KEY (RepactuacaoAtaId)
        REFERENCES dbo.RepactuacaoAta (RepactuacaoAtaId)
    PRINT '✅ FK_ItemVeiculoAta_RepactuacaoAtaId criada'
END
ELSE PRINT '✓  FK_ItemVeiculoAta_RepactuacaoAtaId já existe'
GO

-- REMOVIDO: ItemVeiculoAta.VeiculoId
-- MOTIVO: Campo VeiculoId não existe nesta tabela
-- NOTA: A relação Veiculo ↔ Ata é feita pela tabela VeiculoAta (N-N)

-- ============================================================================
-- Tabela: NotaFiscal
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_NotaFiscal_VeiculoId')
BEGIN
    ALTER TABLE dbo.NotaFiscal WITH NOCHECK
        ADD CONSTRAINT FK_NotaFiscal_VeiculoId
        FOREIGN KEY (VeiculoId)
        REFERENCES dbo.Veiculo (VeiculoId)
    PRINT '✅ FK_NotaFiscal_VeiculoId criada'
END
ELSE PRINT '✓  FK_NotaFiscal_VeiculoId já existe'
GO

-- ============================================================================
-- Tabela: DocumentoContrato
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DocumentoContrato_ContratoId')
BEGIN
    ALTER TABLE dbo.DocumentoContrato WITH NOCHECK
        ADD CONSTRAINT FK_DocumentoContrato_ContratoId
        FOREIGN KEY (ContratoId)
        REFERENCES dbo.Contrato (ContratoId)
    PRINT '✅ FK_DocumentoContrato_ContratoId criada'
END
ELSE PRINT '✓  FK_DocumentoContrato_ContratoId já existe'
GO

-- ============================================================================
-- Tabela: LotacaoMotorista
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_LotacaoMotorista_MotoristaCoberturaId')
BEGIN
    ALTER TABLE dbo.LotacaoMotorista WITH NOCHECK
        ADD CONSTRAINT FK_LotacaoMotorista_MotoristaCoberturaId
        FOREIGN KEY (MotoristaCoberturaId)
        REFERENCES dbo.Motorista (MotoristaId)
    PRINT '✅ FK_LotacaoMotorista_MotoristaCoberturaId criada'
END
ELSE PRINT '✓  FK_LotacaoMotorista_MotoristaCoberturaId já existe'
GO

-- ============================================================================
-- Tabela: MovimentacaoEmpenhoMulta
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_MovimentacaoEmpenhoMulta_MultaId')
BEGIN
    ALTER TABLE dbo.MovimentacaoEmpenhoMulta WITH NOCHECK
        ADD CONSTRAINT FK_MovimentacaoEmpenhoMulta_MultaId
        FOREIGN KEY (MultaId)
        REFERENCES dbo.Multa (MultaId)
    PRINT '✅ FK_MovimentacaoEmpenhoMulta_MultaId criada'
END
ELSE PRINT '✓  FK_MovimentacaoEmpenhoMulta_MultaId já existe'
GO

-- ============================================================================
-- Tabela: ItensManutencao
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItensManutencao_MotoristaId')
BEGIN
    ALTER TABLE dbo.ItensManutencao WITH NOCHECK
        ADD CONSTRAINT FK_ItensManutencao_MotoristaId
        FOREIGN KEY (MotoristaId)
        REFERENCES dbo.Motorista (MotoristaId)
    PRINT '✅ FK_ItensManutencao_MotoristaId criada'
END
ELSE PRINT '✓  FK_ItensManutencao_MotoristaId já existe'
GO

-- ============================================================================
-- Tabela: Patrimonio
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Patrimonio_SetorConferenciaId')
BEGIN
    ALTER TABLE dbo.Patrimonio WITH NOCHECK
        ADD CONSTRAINT FK_Patrimonio_SetorConferenciaId
        FOREIGN KEY (SetorConferenciaId)
        REFERENCES dbo.SetorPatrimonial (SetorId)
    PRINT '✅ FK_Patrimonio_SetorConferenciaId criada'
END
ELSE PRINT '✓  FK_Patrimonio_SetorConferenciaId já existe'
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Patrimonio_SecaoConferenciaId')
BEGIN
    ALTER TABLE dbo.Patrimonio WITH NOCHECK
        ADD CONSTRAINT FK_Patrimonio_SecaoConferenciaId
        FOREIGN KEY (SecaoConferenciaId)
        REFERENCES dbo.SecaoPatrimonial (SecaoId)
    PRINT '✅ FK_Patrimonio_SecaoConferenciaId criada'
END
ELSE PRINT '✓  FK_Patrimonio_SecaoConferenciaId já existe'
GO

-- ============================================================================
-- Tabela: RepactuacaoServicos
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_RepactuacaoServicos_RepactuacaoContratoId')
BEGIN
    ALTER TABLE dbo.RepactuacaoServicos WITH NOCHECK
        ADD CONSTRAINT FK_RepactuacaoServicos_RepactuacaoContratoId
        FOREIGN KEY (RepactuacaoContratoId)
        REFERENCES dbo.RepactuacaoContrato (RepactuacaoContratoId)
    PRINT '✅ FK_RepactuacaoServicos_RepactuacaoContratoId criada'
END
ELSE PRINT '✓  FK_RepactuacaoServicos_RepactuacaoContratoId já existe'
GO

-- ============================================================================
-- VERIFICAÇÃO FINAL
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'VERIFICAÇÃO FINAL - FOREIGN KEYS CRIADAS'
PRINT '============================================================================'
PRINT ''

-- Listar todas as FKs criadas neste script
SELECT
    fk.name AS 'Foreign Key',
    OBJECT_NAME(fk.parent_object_id) AS 'Tabela',
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS 'Coluna',
    OBJECT_NAME(fk.referenced_object_id) AS 'Tabela Referenciada',
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS 'Coluna Referenciada'
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc
    ON fk.object_id = fkc.constraint_object_id
WHERE fk.name IN (
    'FK_Viagem_RecorrenciaViagemId',
    'FK_Motorista_UnidadeId',
    'FK_Motorista_CondutorId',
    'FK_Veiculo_CombustivelId',
    'FK_Veiculo_ContratoId',
    'FK_Veiculo_AtaId',
    'FK_SetorSolicitante_SetorPaiId',
    'FK_WhatsAppMensagens_InstanciaId',
    'FK_WhatsAppMensagens_ContatoId',
    'FK_WhatsAppFilaMensagens_MensagemId',
    'FK_ItemVeiculoAta_RepactuacaoAtaId',
    'FK_NotaFiscal_VeiculoId',
    'FK_DocumentoContrato_ContratoId',
    'FK_LotacaoMotorista_MotoristaCoberturaId',
    'FK_MovimentacaoEmpenhoMulta_MultaId',
    'FK_ItensManutencao_MotoristaId',
    'FK_Patrimonio_SetorConferenciaId',
    'FK_Patrimonio_SecaoConferenciaId',
    'FK_RepactuacaoServicos_RepactuacaoContratoId'
)
ORDER BY OBJECT_NAME(fk.parent_object_id), fk.name

PRINT ''
PRINT '============================================================================'
PRINT 'SCRIPT CONCLUÍDO!'
PRINT '============================================================================'
PRINT ''
PRINT 'FOREIGN KEYS REMOVIDAS DO SCRIPT ORIGINAL (Motivos):'
PRINT '1. WhatsAppMensagens.UsuarioId → AspNetUsers.Id'
PRINT '   MOTIVO: Tipos incompatíveis (int vs nvarchar(450))'
PRINT ''
PRINT '2. WhatsAppWebhookLogs.InstanciaId → WhatsAppInstancias.Id'
PRINT '   MOTIVO: Campo InstanciaId não existe em WhatsAppWebhookLogs'
PRINT ''
PRINT '3. ItemVeiculoAta.VeiculoId → Veiculo.VeiculoId'
PRINT '   MOTIVO: Campo VeiculoId não existe em ItemVeiculoAta'
PRINT '   NOTA: Relação Veiculo ↔ Ata é feita pela tabela VeiculoAta (N-N)'
PRINT ''
PRINT 'TOTAL DE FKs CRIADAS: 19 (de 23 planejadas)'
PRINT ''
PRINT 'PRÓXIMOS PASSOS:'
PRINT '1. Verifique se há avisos sobre dados órfãos acima'
PRINT '2. Execute o script SQL_Indices_Performance.sql para criar índices'
PRINT '3. Teste as funcionalidades do sistema para validar as FKs'
PRINT ''
GO
