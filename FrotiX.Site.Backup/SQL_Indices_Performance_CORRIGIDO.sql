-- ============================================================================
-- SQL PARA CRIAR ÍNDICES DE PERFORMANCE - CORRIGIDO
-- ============================================================================
--
-- VERSÃO CORRIGIDA baseada nos erros encontrados na execução
--
-- Data de Criação: 12/01/2026
-- Gerado por: Claude Code
--
-- CORREÇÕES APLICADAS:
-- 1. Multa.Data (não DataMulta)
-- 2. Multa: Removidos índices com ValorMulta (campo não existe)
-- 3. ItemVeiculoAta: Removido índice VeiculoId (campo não existe)
--
-- IMPORTANTE:
-- - Execute este script em ambiente de DESENVOLVIMENTO primeiro
-- - Monitore o uso de índices após criação
-- - Faça BACKUP do banco antes de executar em PRODUÇÃO
--
-- ============================================================================

USE [FrotiX]
GO

PRINT '============================================================================'
PRINT 'ÍNDICES DE PERFORMANCE - IMPLEMENTAÇÃO CORRIGIDA'
PRINT '============================================================================'
PRINT ''
PRINT 'Data de Execução: ' + CONVERT(varchar, GETDATE(), 120)
PRINT ''

-- ============================================================================
-- PRIORIDADE ALTA (Tabelas Mais Consultadas)
-- ============================================================================
PRINT '============================================================================'
PRINT '[1/3] ÍNDICES - PRIORIDADE ALTA'
PRINT '============================================================================'
PRINT ''

-- ============================================================================
-- Tabela: Motorista
-- ============================================================================

-- Índice para consultas por Unidade
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Motorista_UnidadeId')
BEGIN
    PRINT 'Criando índice IX_Motorista_UnidadeId...'
    CREATE INDEX IX_Motorista_UnidadeId
    ON dbo.Motorista (UnidadeId)
    INCLUDE (Nome, Status);
    PRINT '✅ Índice criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Motorista_UnidadeId já existe'
END
GO

-- ============================================================================
-- Tabela: Veiculo
-- ============================================================================

-- Índice para consultas por tipo de combustível
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Veiculo_CombustivelId')
BEGIN
    PRINT 'Criando índice IX_Veiculo_CombustivelId...'
    CREATE INDEX IX_Veiculo_CombustivelId
    ON dbo.Veiculo (CombustivelId)
    INCLUDE (Placa, Status);
    PRINT '✅ Índice criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Veiculo_CombustivelId já existe'
END
GO

-- Índice para consultas por Ata
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Veiculo_AtaId')
BEGIN
    PRINT 'Criando índice IX_Veiculo_AtaId...'
    CREATE INDEX IX_Veiculo_AtaId
    ON dbo.Veiculo (AtaId)
    INCLUDE (Placa, Status);
    PRINT '✅ Índice criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Veiculo_AtaId já existe'
END
GO

PRINT ''
PRINT '============================================================================'
PRINT '[2/3] ÍNDICES - PRIORIDADE MÉDIA'
PRINT '============================================================================'
PRINT ''

-- ============================================================================
-- Tabela: Manutencao
-- ============================================================================

-- Índice filtrado para veículos reserva
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Manutencao_VeiculoReservaId')
BEGIN
    PRINT 'Criando índice IX_Manutencao_VeiculoReservaId...'
    CREATE INDEX IX_Manutencao_VeiculoReservaId
    ON dbo.Manutencao (VeiculoReservaId)
    WHERE VeiculoReservaId IS NOT NULL;
    PRINT '✅ Índice filtrado criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Manutencao_VeiculoReservaId já existe'
END
GO

-- ============================================================================
-- Tabela: Multa
-- ============================================================================

-- CORRIGIDO: Campo é 'Data' (não 'DataMulta')
-- REMOVIDO: ValorMulta (campo não existe - usar ValorAteVencimento ou ValorPago)

-- Índice para relatórios por contrato de motorista
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Multa_ContratoMotoristaId')
BEGIN
    PRINT 'Criando índice IX_Multa_ContratoMotoristaId...'
    CREATE INDEX IX_Multa_ContratoMotoristaId
    ON dbo.Multa (ContratoMotoristaId)
    INCLUDE (Data, ValorAteVencimento);  -- CORRIGIDO: Data, ValorAteVencimento
    PRINT '✅ Índice criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Multa_ContratoMotoristaId já existe'
END
GO

-- Índice para relatórios por contrato de veículo
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Multa_ContratoVeiculoId')
BEGIN
    PRINT 'Criando índice IX_Multa_ContratoVeiculoId...'
    CREATE INDEX IX_Multa_ContratoVeiculoId
    ON dbo.Multa (ContratoVeiculoId)
    INCLUDE (Data, ValorAteVencimento);  -- CORRIGIDO: Data, ValorAteVencimento
    PRINT '✅ Índice criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Multa_ContratoVeiculoId já existe'
END
GO

-- Índice para filtros por empenho
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Multa_EmpenhoMultaId')
BEGIN
    PRINT 'Criando índice IX_Multa_EmpenhoMultaId...'
    CREATE INDEX IX_Multa_EmpenhoMultaId
    ON dbo.Multa (EmpenhoMultaId);
    PRINT '✅ Índice criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_Multa_EmpenhoMultaId já existe'
END
GO

-- ============================================================================
-- Tabela: SetorSolicitante
-- ============================================================================

-- Índice filtrado para hierarquia de setores
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SetorSolicitante_SetorPaiId')
BEGIN
    PRINT 'Criando índice IX_SetorSolicitante_SetorPaiId...'
    CREATE INDEX IX_SetorSolicitante_SetorPaiId
    ON dbo.SetorSolicitante (SetorPaiId)
    WHERE SetorPaiId IS NOT NULL;
    PRINT '✅ Índice filtrado criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_SetorSolicitante_SetorPaiId já existe'
END
GO

-- ============================================================================
-- Tabela: LotacaoMotorista
-- ============================================================================

-- Índice filtrado para motoristas de cobertura
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LotacaoMotorista_MotoristaCoberturaId')
BEGIN
    PRINT 'Criando índice IX_LotacaoMotorista_MotoristaCoberturaId...'
    CREATE INDEX IX_LotacaoMotorista_MotoristaCoberturaId
    ON dbo.LotacaoMotorista (MotoristaCoberturaId)
    WHERE MotoristaCoberturaId IS NOT NULL;
    PRINT '✅ Índice filtrado criado com sucesso!'
END
ELSE
BEGIN
    PRINT '✓  Índice IX_LotacaoMotorista_MotoristaCoberturaId já existe'
END
GO

PRINT ''
PRINT '============================================================================'
PRINT '[3/3] ÍNDICES - PRIORIDADE BAIXA'
PRINT '============================================================================'
PRINT ''

-- ============================================================================
-- Tabela: NotaFiscal
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NotaFiscal_VeiculoId')
BEGIN
    CREATE INDEX IX_NotaFiscal_VeiculoId
    ON dbo.NotaFiscal (VeiculoId)
    WHERE VeiculoId IS NOT NULL;
    PRINT '✅ IX_NotaFiscal_VeiculoId criado'
END
ELSE PRINT '✓  IX_NotaFiscal_VeiculoId já existe'
GO

-- ============================================================================
-- Tabela: ItensManutencao
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ItensManutencao_MotoristaId')
BEGIN
    CREATE INDEX IX_ItensManutencao_MotoristaId
    ON dbo.ItensManutencao (MotoristaId)
    WHERE MotoristaId IS NOT NULL;
    PRINT '✅ IX_ItensManutencao_MotoristaId criado'
END
ELSE PRINT '✓  IX_ItensManutencao_MotoristaId já existe'
GO

-- ============================================================================
-- Tabela: WhatsAppMensagens
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppMensagens_InstanciaId')
BEGIN
    CREATE INDEX IX_WhatsAppMensagens_InstanciaId
    ON dbo.WhatsAppMensagens (InstanciaId);
    PRINT '✅ IX_WhatsAppMensagens_InstanciaId criado'
END
ELSE PRINT '✓  IX_WhatsAppMensagens_InstanciaId já existe'
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppMensagens_ContatoId')
BEGIN
    CREATE INDEX IX_WhatsAppMensagens_ContatoId
    ON dbo.WhatsAppMensagens (ContatoId);
    PRINT '✅ IX_WhatsAppMensagens_ContatoId criado'
END
ELSE PRINT '✓  IX_WhatsAppMensagens_ContatoId já existe'
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppMensagens_UsuarioId')
BEGIN
    CREATE INDEX IX_WhatsAppMensagens_UsuarioId
    ON dbo.WhatsAppMensagens (UsuarioId);
    PRINT '✅ IX_WhatsAppMensagens_UsuarioId criado'
END
ELSE PRINT '✓  IX_WhatsAppMensagens_UsuarioId já existe'
GO

-- ============================================================================
-- Tabela: WhatsAppFilaMensagens
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WhatsAppFilaMensagens_MensagemId')
BEGIN
    CREATE INDEX IX_WhatsAppFilaMensagens_MensagemId
    ON dbo.WhatsAppFilaMensagens (MensagemId);
    PRINT '✅ IX_WhatsAppFilaMensagens_MensagemId criado'
END
ELSE PRINT '✓  IX_WhatsAppFilaMensagens_MensagemId já existe'
GO

-- ============================================================================
-- Tabela: Patrimonio
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Patrimonio_SetorConferenciaId')
BEGIN
    CREATE INDEX IX_Patrimonio_SetorConferenciaId
    ON dbo.Patrimonio (SetorConferenciaId)
    WHERE SetorConferenciaId IS NOT NULL;
    PRINT '✅ IX_Patrimonio_SetorConferenciaId criado'
END
ELSE PRINT '✓  IX_Patrimonio_SetorConferenciaId já existe'
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Patrimonio_SecaoConferenciaId')
BEGIN
    CREATE INDEX IX_Patrimonio_SecaoConferenciaId
    ON dbo.Patrimonio (SecaoConferenciaId)
    WHERE SecaoConferenciaId IS NOT NULL;
    PRINT '✅ IX_Patrimonio_SecaoConferenciaId criado'
END
ELSE PRINT '✓  IX_Patrimonio_SecaoConferenciaId já existe'
GO

-- ============================================================================
-- Tabela: ItemVeiculoAta
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ItemVeiculoAta_RepactuacaoAtaId')
BEGIN
    CREATE INDEX IX_ItemVeiculoAta_RepactuacaoAtaId
    ON dbo.ItemVeiculoAta (RepactuacaoAtaId)
    WHERE RepactuacaoAtaId IS NOT NULL;
    PRINT '✅ IX_ItemVeiculoAta_RepactuacaoAtaId criado'
END
ELSE PRINT '✓  IX_ItemVeiculoAta_RepactuacaoAtaId já existe'
GO

-- REMOVIDO: ItemVeiculoAta.VeiculoId
-- MOTIVO: Campo VeiculoId não existe nesta tabela

-- ============================================================================
-- VERIFICAÇÃO E MONITORAMENTO
-- ============================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'VERIFICAÇÃO DE ÍNDICES CRIADOS'
PRINT '============================================================================'
PRINT ''

-- Listar todos os índices criados neste script
SELECT
    OBJECT_NAME(i.object_id) AS 'Tabela',
    i.name AS 'Índice',
    i.type_desc AS 'Tipo',
    i.is_unique AS 'Único',
    STUFF((
        SELECT ', ' + COL_NAME(ic.object_id, ic.column_id)
        FROM sys.index_columns ic
        WHERE ic.object_id = i.object_id
        AND ic.index_id = i.index_id
        AND ic.is_included_column = 0
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS 'Colunas Chave',
    STUFF((
        SELECT ', ' + COL_NAME(ic.object_id, ic.column_id)
        FROM sys.index_columns ic
        WHERE ic.object_id = i.object_id
        AND ic.index_id = i.index_id
        AND ic.is_included_column = 1
        ORDER BY ic.index_column_id
        FOR XML PATH('')
    ), 1, 2, '') AS 'Colunas Incluídas'
FROM sys.indexes i
WHERE i.name IN (
    'IX_Motorista_UnidadeId',
    'IX_Veiculo_CombustivelId',
    'IX_Veiculo_AtaId',
    'IX_Manutencao_VeiculoReservaId',
    'IX_Multa_ContratoMotoristaId',
    'IX_Multa_ContratoVeiculoId',
    'IX_Multa_EmpenhoMultaId',
    'IX_SetorSolicitante_SetorPaiId',
    'IX_LotacaoMotorista_MotoristaCoberturaId',
    'IX_NotaFiscal_VeiculoId',
    'IX_ItensManutencao_MotoristaId',
    'IX_WhatsAppMensagens_InstanciaId',
    'IX_WhatsAppMensagens_ContatoId',
    'IX_WhatsAppMensagens_UsuarioId',
    'IX_WhatsAppFilaMensagens_MensagemId',
    'IX_Patrimonio_SetorConferenciaId',
    'IX_Patrimonio_SecaoConferenciaId',
    'IX_ItemVeiculoAta_RepactuacaoAtaId'
)
ORDER BY OBJECT_NAME(i.object_id), i.name

PRINT ''
PRINT '============================================================================'
PRINT 'SCRIPT CONCLUÍDO!'
PRINT '============================================================================'
PRINT ''
PRINT 'CORREÇÕES APLICADAS:'
PRINT '1. Multa: Alterado DataMulta → Data'
PRINT '2. Multa: Alterado ValorMulta → ValorAteVencimento (nos índices compostos)'
PRINT '3. ItemVeiculoAta: Removido índice VeiculoId (campo não existe)'
PRINT ''
PRINT 'TOTAL DE ÍNDICES CRIADOS: 18 (de 24 planejados)'
PRINT ''
PRINT 'PRÓXIMOS PASSOS:'
PRINT '1. Execute queries de teste para validar a performance'
PRINT '2. Monitore o uso dos índices (query abaixo)'
PRINT '3. Remova índices não utilizados após 30 dias de monitoramento'
PRINT '4. Execute DBCC UPDATESTATISTICS periodicamente'
PRINT ''
PRINT '-- Query para monitorar uso de índices (executar após 7-30 dias):'
PRINT 'SELECT'
PRINT '    OBJECT_NAME(s.object_id) AS TableName,'
PRINT '    i.name AS IndexName,'
PRINT '    s.user_seeks AS Buscas,'
PRINT '    s.user_scans AS Varreduras,'
PRINT '    s.user_lookups AS Lookups,'
PRINT '    s.user_updates AS Atualizacoes,'
PRINT '    s.last_user_seek AS UltimaBusca,'
PRINT '    s.last_user_scan AS UltimaVarredura'
PRINT 'FROM sys.dm_db_index_usage_stats s'
PRINT 'INNER JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id'
PRINT 'WHERE OBJECTPROPERTY(s.object_id, ''IsUserTable'') = 1'
PRINT 'AND i.name LIKE ''IX_%'''
PRINT 'ORDER BY (s.user_seeks + s.user_scans + s.user_lookups) DESC;'
PRINT ''
GO

-- ============================================================================
-- ANÁLISE DE FRAGMENTAÇÃO (Executar manualmente quando necessário)
-- ============================================================================

-- Descomente para executar análise de fragmentação
/*
SELECT
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.index_type_desc AS IndexType,
    ips.avg_fragmentation_in_percent AS Fragmentacao,
    ips.page_count AS Paginas
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'SAMPLED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 10  -- Apenas índices com mais de 10% de fragmentação
AND ips.page_count > 100  -- Apenas índices com mais de 100 páginas
ORDER BY ips.avg_fragmentation_in_percent DESC;

-- Se fragmentação > 30%, considere REBUILD
-- Se fragmentação entre 10-30%, considere REORGANIZE
*/
GO
