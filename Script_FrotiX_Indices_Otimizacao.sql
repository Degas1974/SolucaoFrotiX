-- ============================================================================
-- ANÁLISE E OTIMIZAÇÃO DE ÍNDICES - BANCO FROTIX
-- Data: 29/11/2025
-- ============================================================================
-- Este script foi gerado após análise completa de:
-- 1. Índices existentes no banco de dados
-- 2. Padrões de consulta nos Controllers (.Where, .OrderBy, .GetFirstOrDefault)
-- 3. Frequência de uso de cada padrão
-- 4. Views mais acessadas
-- ============================================================================

USE Frotix
GO

SET NOCOUNT ON
GO

PRINT '============================================================================'
PRINT '=== ANÁLISE DE ÍNDICES - BANCO FROTIX ==='
PRINT '=== Data: ' + CONVERT(VARCHAR, GETDATE(), 120)
PRINT '============================================================================'
GO

-- ============================================================================
-- PARTE 1: ANÁLISE DOS ÍNDICES EXISTENTES
-- ============================================================================

PRINT ''
PRINT '=== PARTE 1: ÍNDICES EXISTENTES ==='
PRINT ''

-- Listar todos os índices existentes
SELECT 
    t.name AS Tabela,
    i.name AS NomeIndice,
    i.type_desc AS Tipo,
    CASE WHEN i.is_unique = 1 THEN 'Sim' ELSE 'Nao' END AS Unico,
    STUFF((
        SELECT ', ' + c.name + CASE WHEN ic.is_descending_key = 1 THEN ' DESC' ELSE '' END
        FROM sys.index_columns ic
        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS Colunas,
    i.[fill_factor] AS [FillFactor]
FROM sys.indexes i
JOIN sys.tables t ON i.object_id = t.object_id
WHERE t.is_ms_shipped = 0
    AND i.type > 0
    AND t.name IN ('Viagem', 'Veiculo', 'Motorista', 'Multa', 'Manutencao', 
                   'Abastecimento', 'Evento', 'Empenho', 'LotacaoMotorista',
                   'ItensManutencao', 'AlertasFrotiX', 'AlertasUsuario', 'Lavagem')
ORDER BY t.name, i.name;
GO

-- ============================================================================
-- PARTE 2: ÍNDICES RECOMENDADOS - TABELA VIAGEM
-- ============================================================================
-- Padrões de consulta identificados:
-- 1. DataInicial + Range (Dashboard) - JÁ TEM
-- 2. VeiculoId + StatusAgendamento - FALTA
-- 3. MotoristaId + StatusAgendamento - FALTA
-- 4. EventoId + Status - FALTA
-- 5. Finalidade + StatusAgendamento - FALTA
-- 6. SetorSolicitanteId + DataInicial - FALTA
-- 7. RequisitanteId + DataInicial - FALTA
-- ============================================================================

PRINT ''
PRINT '=== PARTE 2: ÍNDICES PARA TABELA VIAGEM ==='

-- Índice 1: VeiculoId + StatusAgendamento (muito usado em CustosViagem, ViagemEvento)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_VeiculoId_StatusAgendamento' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_VeiculoId_StatusAgendamento...'
    CREATE NONCLUSTERED INDEX IX_Viagem_VeiculoId_StatusAgendamento
    ON Viagem (VeiculoId, StatusAgendamento)
    INCLUDE (DataInicial, DataFinal, Status, MotoristaId, CustoCombustivel, CustoMotorista)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_VeiculoId_StatusAgendamento criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_VeiculoId_StatusAgendamento já existe'
GO

-- Índice 2: MotoristaId + StatusAgendamento (muito usado em CustosViagem, ViagemEvento)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_MotoristaId_StatusAgendamento' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_MotoristaId_StatusAgendamento...'
    CREATE NONCLUSTERED INDEX IX_Viagem_MotoristaId_StatusAgendamento
    ON Viagem (MotoristaId, StatusAgendamento)
    INCLUDE (DataInicial, DataFinal, Status, VeiculoId, CustoCombustivel, CustoMotorista)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_MotoristaId_StatusAgendamento criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_MotoristaId_StatusAgendamento já existe'
GO

-- Índice 3: EventoId + Status (usado em ViagemController para eventos)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_EventoId_Status' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_EventoId_Status...'
    CREATE NONCLUSTERED INDEX IX_Viagem_EventoId_Status
    ON Viagem (EventoId, Status)
    INCLUDE (DataInicial, DataFinal, VeiculoId, MotoristaId, NoFichaVistoria, CustoCombustivel)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_EventoId_Status criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_EventoId_Status já existe'
GO

-- Índice 4: Finalidade + StatusAgendamento (usado para filtrar eventos na Agenda)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_Finalidade_StatusAgendamento' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_Finalidade_StatusAgendamento...'
    CREATE NONCLUSTERED INDEX IX_Viagem_Finalidade_StatusAgendamento
    ON Viagem (Finalidade, StatusAgendamento)
    INCLUDE (DataInicial, DataFinal, Status, VeiculoId, MotoristaId, EventoId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_Finalidade_StatusAgendamento criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_Finalidade_StatusAgendamento já existe'
GO

-- Índice 5: SetorSolicitanteId + DataInicial (Dashboard por Setor)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_SetorSolicitanteId_DataInicial' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_SetorSolicitanteId_DataInicial...'
    CREATE NONCLUSTERED INDEX IX_Viagem_SetorSolicitanteId_DataInicial
    ON Viagem (SetorSolicitanteId, DataInicial)
    INCLUDE (Status, StatusAgendamento, CustoCombustivel, CustoMotorista, VeiculoId, MotoristaId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_SetorSolicitanteId_DataInicial criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_SetorSolicitanteId_DataInicial já existe'
GO

-- Índice 6: RequisitanteId + DataInicial (Dashboard por Requisitante)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_RequisitanteId_DataInicial' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_RequisitanteId_DataInicial...'
    CREATE NONCLUSTERED INDEX IX_Viagem_RequisitanteId_DataInicial
    ON Viagem (RequisitanteId, DataInicial)
    INCLUDE (Status, StatusAgendamento, CustoCombustivel, CustoMotorista, VeiculoId, MotoristaId, SetorSolicitanteId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_RequisitanteId_DataInicial criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_RequisitanteId_DataInicial já existe'
GO

-- Índice 7: ItemManutencaoId (usado em Ocorrências para verificar viagens com manutenção)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_ItemManutencaoId_Completo' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_ItemManutencaoId_Completo...'
    CREATE NONCLUSTERED INDEX IX_Viagem_ItemManutencaoId_Completo
    ON Viagem (ItemManutencaoId)
    INCLUDE (ViagemId, VeiculoId, DataInicial, Status, StatusOcorrencia, ResumoOcorrencia)
    WHERE ItemManutencaoId IS NOT NULL
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_ItemManutencaoId_Completo criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_ItemManutencaoId_Completo já existe'
GO

-- Índice 8: StatusOcorrencia + DataFinal (Ocorrências - muito usado)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_StatusOcorrencia_DataFinal' AND object_id = OBJECT_ID('Viagem'))
BEGIN
    PRINT 'Criando IX_Viagem_StatusOcorrencia_DataFinal...'
    CREATE NONCLUSTERED INDEX IX_Viagem_StatusOcorrencia_DataFinal
    ON Viagem (StatusOcorrencia, DataFinal DESC)
    INCLUDE (VeiculoId, MotoristaId, ResumoOcorrencia, NoFichaVistoria, DataInicial)
    WHERE ResumoOcorrencia IS NOT NULL AND ResumoOcorrencia <> ''
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Viagem_StatusOcorrencia_DataFinal criado com sucesso'
END
ELSE
    PRINT 'IX_Viagem_StatusOcorrencia_DataFinal já existe'
GO

-- ============================================================================
-- PARTE 3: ÍNDICES RECOMENDADOS - TABELA MULTA
-- ============================================================================
-- Padrões de consulta identificados:
-- 1. MotoristaId - filtro comum
-- 2. VeiculoId - filtro comum  
-- 3. Status - filtro comum
-- 4. OrgaoAutuanteId - filtro comum
-- 5. TipoMultaId - filtro comum
-- 6. Data - ordenacao
-- ============================================================================

PRINT ''
PRINT '=== PARTE 3: ÍNDICES PARA TABELA MULTA ==='

-- Índice 1: MotoristaId (filtro muito usado)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Multa_MotoristaId' AND object_id = OBJECT_ID('Multa'))
BEGIN
    PRINT 'Criando IX_Multa_MotoristaId...'
    CREATE NONCLUSTERED INDEX IX_Multa_MotoristaId
    ON Multa (MotoristaId)
    INCLUDE (VeiculoId, Status, Data, ValorAteVencimento, NumInfracao)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Multa_MotoristaId criado com sucesso'
END
ELSE
    PRINT 'IX_Multa_MotoristaId já existe'
GO

-- Índice 2: VeiculoId (filtro muito usado)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Multa_VeiculoId' AND object_id = OBJECT_ID('Multa'))
BEGIN
    PRINT 'Criando IX_Multa_VeiculoId...'
    CREATE NONCLUSTERED INDEX IX_Multa_VeiculoId
    ON Multa (VeiculoId)
    INCLUDE (MotoristaId, Status, Data, ValorAteVencimento, NumInfracao)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Multa_VeiculoId criado com sucesso'
END
ELSE
    PRINT 'IX_Multa_VeiculoId já existe'
GO

-- Índice 3: Status + Data (filtro + ordenacao)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Multa_Status_Data' AND object_id = OBJECT_ID('Multa'))
BEGIN
    PRINT 'Criando IX_Multa_Status_Data...'
    CREATE NONCLUSTERED INDEX IX_Multa_Status_Data
    ON Multa (Status, Data DESC)
    INCLUDE (VeiculoId, MotoristaId, ValorAteVencimento, NumInfracao, Fase)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Multa_Status_Data criado com sucesso'
END
ELSE
    PRINT 'IX_Multa_Status_Data já existe'
GO

-- Índice 4: OrgaoAutuanteId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Multa_OrgaoAutuanteId' AND object_id = OBJECT_ID('Multa'))
BEGIN
    PRINT 'Criando IX_Multa_OrgaoAutuanteId...'
    CREATE NONCLUSTERED INDEX IX_Multa_OrgaoAutuanteId
    ON Multa (OrgaoAutuanteId)
    INCLUDE (Status, Data, ValorAteVencimento)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Multa_OrgaoAutuanteId criado com sucesso'
END
ELSE
    PRINT 'IX_Multa_OrgaoAutuanteId já existe'
GO

-- Índice 5: TipoMultaId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Multa_TipoMultaId' AND object_id = OBJECT_ID('Multa'))
BEGIN
    PRINT 'Criando IX_Multa_TipoMultaId...'
    CREATE NONCLUSTERED INDEX IX_Multa_TipoMultaId
    ON Multa (TipoMultaId)
    INCLUDE (Status, Data, ValorAteVencimento)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Multa_TipoMultaId criado com sucesso'
END
ELSE
    PRINT 'IX_Multa_TipoMultaId já existe'
GO

-- Índice 6: Composto para listagem geral com filtros multiplos
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Multa_Data_Completo' AND object_id = OBJECT_ID('Multa'))
BEGIN
    PRINT 'Criando IX_Multa_Data_Completo...'
    CREATE NONCLUSTERED INDEX IX_Multa_Data_Completo
    ON Multa (Data DESC)
    INCLUDE (MultaId, VeiculoId, MotoristaId, Status, Fase, ValorAteVencimento, 
             OrgaoAutuanteId, TipoMultaId, NumInfracao, AutuacaoPDF, PenalidadePDF, ComprovantePDF)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Multa_Data_Completo criado com sucesso'
END
ELSE
    PRINT 'IX_Multa_Data_Completo já existe'
GO

-- ============================================================================
-- PARTE 4: ÍNDICES RECOMENDADOS - TABELA MANUTENCAO
-- ============================================================================

PRINT ''
PRINT '=== PARTE 4: ÍNDICES PARA TABELA MANUTENCAO ==='

-- Índice 1: VeiculoId + DataSolicitacao (muito usado)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Manutencao_VeiculoId_DataSolicitacao' AND object_id = OBJECT_ID('Manutencao'))
BEGIN
    PRINT 'Criando IX_Manutencao_VeiculoId_DataSolicitacao...'
    CREATE NONCLUSTERED INDEX IX_Manutencao_VeiculoId_DataSolicitacao
    ON Manutencao (VeiculoId, DataSolicitacao DESC)
    INCLUDE (ManutencaoId, StatusOS, ResumoOS, DataDevolucao, DataFinalizacao)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Manutencao_VeiculoId_DataSolicitacao criado com sucesso'
END
ELSE
    PRINT 'IX_Manutencao_VeiculoId_DataSolicitacao já existe'
GO

-- Índice 2: StatusOS (filtro comum)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Manutencao_StatusOS' AND object_id = OBJECT_ID('Manutencao'))
BEGIN
    PRINT 'Criando IX_Manutencao_StatusOS...'
    CREATE NONCLUSTERED INDEX IX_Manutencao_StatusOS
    ON Manutencao (StatusOS)
    INCLUDE (VeiculoId, DataSolicitacao, ResumoOS)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Manutencao_StatusOS criado com sucesso'
END
ELSE
    PRINT 'IX_Manutencao_StatusOS já existe'
GO

-- Índice 3: DataSolicitacao DESC (ordenação principal na listagem)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Manutencao_DataSolicitacao' AND object_id = OBJECT_ID('Manutencao'))
BEGIN
    PRINT 'Criando IX_Manutencao_DataSolicitacao...'
    CREATE NONCLUSTERED INDEX IX_Manutencao_DataSolicitacao
    ON Manutencao (DataSolicitacao DESC)
    INCLUDE (ManutencaoId, VeiculoId, StatusOS, ResumoOS, VeiculoReservaId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Manutencao_DataSolicitacao criado com sucesso'
END
ELSE
    PRINT 'IX_Manutencao_DataSolicitacao já existe'
GO

-- ============================================================================
-- PARTE 5: ÍNDICES RECOMENDADOS - TABELA ITENSMANUTENCAO
-- ============================================================================

PRINT ''
PRINT '=== PARTE 5: ÍNDICES PARA TABELA ITENSMANUTENCAO ==='

-- Índice 1: ManutencaoId (FK muito usada)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ItensManutencao_ManutencaoId' AND object_id = OBJECT_ID('ItensManutencao'))
BEGIN
    PRINT 'Criando IX_ItensManutencao_ManutencaoId...'
    CREATE NONCLUSTERED INDEX IX_ItensManutencao_ManutencaoId
    ON ItensManutencao (ManutencaoId)
    INCLUDE (ItemManutencaoId, ViagemId, Descricao)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_ItensManutencao_ManutencaoId criado com sucesso'
END
ELSE
    PRINT 'IX_ItensManutencao_ManutencaoId já existe'
GO

-- Índice 2: ViagemId (FK para vincular com Viagem)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ItensManutencao_ViagemId' AND object_id = OBJECT_ID('ItensManutencao'))
BEGIN
    PRINT 'Criando IX_ItensManutencao_ViagemId...'
    CREATE NONCLUSTERED INDEX IX_ItensManutencao_ViagemId
    ON ItensManutencao (ViagemId)
    INCLUDE (ItemManutencaoId, ManutencaoId, Descricao)
    WHERE ViagemId IS NOT NULL
    WITH (FILLFACTOR = 90);
    PRINT 'IX_ItensManutencao_ViagemId criado com sucesso'
END
ELSE
    PRINT 'IX_ItensManutencao_ViagemId já existe'
GO

-- ============================================================================
-- PARTE 6: ÍNDICES RECOMENDADOS - TABELA EMPENHO
-- ============================================================================

PRINT ''
PRINT '=== PARTE 6: ÍNDICES PARA TABELA EMPENHO ==='

-- Índice 1: ContratoId (muito usado em NotaFiscalController)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Empenho_ContratoId' AND object_id = OBJECT_ID('Empenho'))
BEGIN
    PRINT 'Criando IX_Empenho_ContratoId...'
    CREATE NONCLUSTERED INDEX IX_Empenho_ContratoId
    ON Empenho (ContratoId)
    INCLUDE (EmpenhoId, NotaEmpenho, SaldoInicial, DataEmissao, AtaId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Empenho_ContratoId criado com sucesso'
END
ELSE
    PRINT 'IX_Empenho_ContratoId já existe'
GO

-- Índice 2: AtaId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Empenho_AtaId' AND object_id = OBJECT_ID('Empenho'))
BEGIN
    PRINT 'Criando IX_Empenho_AtaId...'
    CREATE NONCLUSTERED INDEX IX_Empenho_AtaId
    ON Empenho (AtaId)
    INCLUDE (EmpenhoId, NotaEmpenho, SaldoInicial, DataEmissao, ContratoId)
    WHERE AtaId IS NOT NULL
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Empenho_AtaId criado com sucesso'
END
ELSE
    PRINT 'IX_Empenho_AtaId já existe'
GO

-- ============================================================================
-- PARTE 7: ÍNDICES RECOMENDADOS - TABELA LOTACAOMOTORISTA
-- ============================================================================

PRINT ''
PRINT '=== PARTE 7: ÍNDICES PARA TABELA LOTACAOMOTORISTA ==='

-- Índice 1: MotoristaId + UnidadeId (muito usado em UnidadeController)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LotacaoMotorista_MotoristaId_UnidadeId' AND object_id = OBJECT_ID('LotacaoMotorista'))
BEGIN
    PRINT 'Criando IX_LotacaoMotorista_MotoristaId_UnidadeId...'
    CREATE NONCLUSTERED INDEX IX_LotacaoMotorista_MotoristaId_UnidadeId
    ON LotacaoMotorista (MotoristaId, UnidadeId)
    INCLUDE (LotacaoMotoristaId, DataInicio, DataFim, Lotado)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_LotacaoMotorista_MotoristaId_UnidadeId criado com sucesso'
END
ELSE
    PRINT 'IX_LotacaoMotorista_MotoristaId_UnidadeId já existe'
GO

-- Índice 2: UnidadeId (busca por Unidade)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LotacaoMotorista_UnidadeId' AND object_id = OBJECT_ID('LotacaoMotorista'))
BEGIN
    PRINT 'Criando IX_LotacaoMotorista_UnidadeId...'
    CREATE NONCLUSTERED INDEX IX_LotacaoMotorista_UnidadeId
    ON LotacaoMotorista (UnidadeId)
    INCLUDE (LotacaoMotoristaId, MotoristaId, DataInicio, DataFim, Lotado)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_LotacaoMotorista_UnidadeId criado com sucesso'
END
ELSE
    PRINT 'IX_LotacaoMotorista_UnidadeId já existe'
GO

-- ============================================================================
-- PARTE 8: ÍNDICES RECOMENDADOS - TABELA EVENTO
-- ============================================================================

PRINT ''
PRINT '=== PARTE 8: ÍNDICES PARA TABELA EVENTO ==='

-- Índice 1: SetorSolicitanteId + DataInicial (Dashboard de Eventos)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Evento_SetorSolicitanteId_DataInicial' AND object_id = OBJECT_ID('Evento'))
BEGIN
    PRINT 'Criando IX_Evento_SetorSolicitanteId_DataInicial...'
    CREATE NONCLUSTERED INDEX IX_Evento_SetorSolicitanteId_DataInicial
    ON Evento (SetorSolicitanteId, DataInicial)
    INCLUDE (EventoId, Nome, DataFinal, QtdParticipantes, RequisitanteId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Evento_SetorSolicitanteId_DataInicial criado com sucesso'
END
ELSE
    PRINT 'IX_Evento_SetorSolicitanteId_DataInicial já existe'
GO

-- Índice 2: DataInicial (Dashboard range de datas)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Evento_DataInicial' AND object_id = OBJECT_ID('Evento'))
BEGIN
    PRINT 'Criando IX_Evento_DataInicial...'
    CREATE NONCLUSTERED INDEX IX_Evento_DataInicial
    ON Evento (DataInicial)
    INCLUDE (EventoId, Nome, DataFinal, SetorSolicitanteId, QtdParticipantes)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Evento_DataInicial criado com sucesso'
END
ELSE
    PRINT 'IX_Evento_DataInicial já existe'
GO

-- Índice 3: Nome (busca por nome de evento)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Evento_Nome' AND object_id = OBJECT_ID('Evento'))
BEGIN
    PRINT 'Criando IX_Evento_Nome...'
    CREATE NONCLUSTERED INDEX IX_Evento_Nome
    ON Evento (Nome)
    INCLUDE (EventoId, DataInicial, DataFinal)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Evento_Nome criado com sucesso'
END
ELSE
    PRINT 'IX_Evento_Nome já existe'
GO

-- ============================================================================
-- PARTE 9: ÍNDICES RECOMENDADOS - TABELA LAVAGEM
-- ============================================================================

PRINT ''
PRINT '=== PARTE 9: ÍNDICES PARA TABELA LAVAGEM ==='

-- Índice 1: VeiculoId + Data (listagem de lavagens por veículo)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Lavagem_VeiculoId_Data' AND object_id = OBJECT_ID('Lavagem'))
BEGIN
    PRINT 'Criando IX_Lavagem_VeiculoId_Data...'
    CREATE NONCLUSTERED INDEX IX_Lavagem_VeiculoId_Data
    ON Lavagem (VeiculoId, Data DESC)
    INCLUDE (LavagemId, MotoristaId, Horario)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Lavagem_VeiculoId_Data criado com sucesso'
END
ELSE
    PRINT 'IX_Lavagem_VeiculoId_Data já existe'
GO

-- Índice 2: MotoristaId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Lavagem_MotoristaId' AND object_id = OBJECT_ID('Lavagem'))
BEGIN
    PRINT 'Criando IX_Lavagem_MotoristaId...'
    CREATE NONCLUSTERED INDEX IX_Lavagem_MotoristaId
    ON Lavagem (MotoristaId)
    INCLUDE (LavagemId, VeiculoId, Data, Horario)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Lavagem_MotoristaId criado com sucesso'
END
ELSE
    PRINT 'IX_Lavagem_MotoristaId já existe'
GO

-- ============================================================================
-- PARTE 10: ÍNDICES RECOMENDADOS - TABELA VIAGESTESTATISTICA
-- ============================================================================

PRINT ''
PRINT '=== PARTE 10: ÍNDICES PARA TABELA VIAGEMESTATISTICA ==='

-- Índice 1: DataReferencia (muito usado em Dashboard para range)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ViagemEstatistica_DataReferencia_Custos' AND object_id = OBJECT_ID('ViagemEstatistica'))
BEGIN
    PRINT 'Criando IX_ViagemEstatistica_DataReferencia_Custos...'
    CREATE NONCLUSTERED INDEX IX_ViagemEstatistica_DataReferencia_Custos
    ON ViagemEstatistica (DataReferencia)
    INCLUDE (TotalViagens, QuilometragemTotal, CustoTotal, CustoCombustivel, CustoMotorista)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_ViagemEstatistica_DataReferencia_Custos criado com sucesso'
END
ELSE
    PRINT 'IX_ViagemEstatistica_DataReferencia_Custos já existe'
GO

-- ============================================================================
-- PARTE 11: ÍNDICES PARA TABELA VEICULO
-- ============================================================================

PRINT ''
PRINT '=== PARTE 11: ÍNDICES PARA TABELA VEICULO ==='

-- Índice 1: ContratoId (busca veículos por contrato)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Veiculo_ContratoId' AND object_id = OBJECT_ID('Veiculo'))
BEGIN
    PRINT 'Criando IX_Veiculo_ContratoId...'
    CREATE NONCLUSTERED INDEX IX_Veiculo_ContratoId
    ON Veiculo (ContratoId)
    INCLUDE (VeiculoId, Placa, Status, MarcaId, ModeloId, ItemVeiculoId)
    WHERE ContratoId IS NOT NULL
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Veiculo_ContratoId criado com sucesso'
END
ELSE
    PRINT 'IX_Veiculo_ContratoId já existe'
GO

-- Índice 2: UnidadeId + Status (busca veículos ativos por unidade)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Veiculo_UnidadeId_Status' AND object_id = OBJECT_ID('Veiculo'))
BEGIN
    PRINT 'Criando IX_Veiculo_UnidadeId_Status...'
    CREATE NONCLUSTERED INDEX IX_Veiculo_UnidadeId_Status
    ON Veiculo (UnidadeId, Status)
    INCLUDE (VeiculoId, Placa, MarcaId, ModeloId, ContratoId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Veiculo_UnidadeId_Status criado com sucesso'
END
ELSE
    PRINT 'IX_Veiculo_UnidadeId_Status já existe'
GO

-- Índice 3: Placa (busca direta por placa)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Veiculo_Placa' AND object_id = OBJECT_ID('Veiculo'))
BEGIN
    PRINT 'Criando IX_Veiculo_Placa...'
    CREATE NONCLUSTERED INDEX IX_Veiculo_Placa
    ON Veiculo (Placa)
    INCLUDE (VeiculoId, Status, MarcaId, ModeloId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Veiculo_Placa criado com sucesso'
END
ELSE
    PRINT 'IX_Veiculo_Placa já existe'
GO

-- ============================================================================
-- PARTE 12: ÍNDICES PARA TABELA MOTORISTA
-- ============================================================================

PRINT ''
PRINT '=== PARTE 12: ÍNDICES PARA TABELA MOTORISTA ==='

-- Índice 1: ContratoId + Status (motoristas terceirizados ativos)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Motorista_ContratoId_Status' AND object_id = OBJECT_ID('Motorista'))
BEGIN
    PRINT 'Criando IX_Motorista_ContratoId_Status...'
    CREATE NONCLUSTERED INDEX IX_Motorista_ContratoId_Status
    ON Motorista (ContratoId, Status)
    INCLUDE (MotoristaId, Nome, Ponto, TipoCondutor)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Motorista_ContratoId_Status criado com sucesso'
END
ELSE
    PRINT 'IX_Motorista_ContratoId_Status já existe'
GO

-- Índice 2: Status + Nome (listagem de motoristas ativos ordenados)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Motorista_Status_Nome' AND object_id = OBJECT_ID('Motorista'))
BEGIN
    PRINT 'Criando IX_Motorista_Status_Nome...'
    CREATE NONCLUSTERED INDEX IX_Motorista_Status_Nome
    ON Motorista (Status, Nome)
    INCLUDE (MotoristaId, Ponto, TipoCondutor, ContratoId)
    WITH (FILLFACTOR = 90);
    PRINT 'IX_Motorista_Status_Nome criado com sucesso'
END
ELSE
    PRINT 'IX_Motorista_Status_Nome já existe'
GO

-- ============================================================================
-- PARTE 13: ATUALIZAR ESTATÍSTICAS
-- ============================================================================

PRINT ''
PRINT '=== PARTE 13: ATUALIZANDO ESTATÍSTICAS ==='

-- Atualizar estatísticas das tabelas principais
UPDATE STATISTICS Viagem WITH FULLSCAN;
PRINT 'Estatísticas de Viagem atualizadas'

UPDATE STATISTICS Multa WITH FULLSCAN;
PRINT 'Estatísticas de Multa atualizadas'

UPDATE STATISTICS Manutencao WITH FULLSCAN;
PRINT 'Estatísticas de Manutencao atualizadas'

UPDATE STATISTICS Veiculo WITH FULLSCAN;
PRINT 'Estatísticas de Veiculo atualizadas'

UPDATE STATISTICS Motorista WITH FULLSCAN;
PRINT 'Estatísticas de Motorista atualizadas'

UPDATE STATISTICS Evento WITH FULLSCAN;
PRINT 'Estatísticas de Evento atualizadas'

UPDATE STATISTICS AlertasFrotiX WITH FULLSCAN;
PRINT 'Estatísticas de AlertasFrotiX atualizadas'

UPDATE STATISTICS AlertasUsuario WITH FULLSCAN;
PRINT 'Estatísticas de AlertasUsuario atualizadas'
GO

-- ============================================================================
-- PARTE 14: VERIFICAÇÃO FINAL - LISTAR TODOS OS ÍNDICES CRIADOS
-- ============================================================================

PRINT ''
PRINT '=== PARTE 14: VERIFICAÇÃO FINAL ==='

SELECT 
    t.name AS Tabela,
    i.name AS NomeIndice,
    i.type_desc AS Tipo,
    CASE WHEN i.is_unique = 1 THEN 'Sim' ELSE 'Nao' END AS Unico,
    STUFF((
        SELECT ', ' + c.name
        FROM sys.index_columns ic
        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS ColunasChave,
    STUFF((
        SELECT ', ' + c.name
        FROM sys.index_columns ic
        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS ColunasIncluidas
FROM sys.indexes i
JOIN sys.tables t ON i.object_id = t.object_id
WHERE t.is_ms_shipped = 0
    AND i.type > 0
    AND i.name LIKE 'IX_%'
    AND t.name IN ('Viagem', 'Veiculo', 'Motorista', 'Multa', 'Manutencao', 
                   'Evento', 'Empenho', 'LotacaoMotorista', 'ItensManutencao',
                   'Lavagem', 'ViagemEstatistica')
ORDER BY t.name, i.name;
GO

PRINT ''
PRINT '============================================================================'
PRINT '=== SCRIPT FINALIZADO COM SUCESSO ==='
PRINT '=== Total de índices novos: ~30 ==='
PRINT '=== Data: ' + CONVERT(VARCHAR, GETDATE(), 120)
PRINT '============================================================================'
GO

-- ============================================================================
-- RESUMO DOS ÍNDICES CRIADOS
-- ============================================================================
/*
TABELA VIAGEM (8 índices novos):
1. IX_Viagem_VeiculoId_StatusAgendamento - Filtro por veículo + agendamento
2. IX_Viagem_MotoristaId_StatusAgendamento - Filtro por motorista + agendamento
3. IX_Viagem_EventoId_Status - Viagens de eventos
4. IX_Viagem_Finalidade_StatusAgendamento - Filtro por finalidade
5. IX_Viagem_SetorSolicitanteId_DataInicial - Dashboard por setor
6. IX_Viagem_RequisitanteId_DataInicial - Dashboard por requisitante
7. IX_Viagem_ItemManutencaoId_Completo - Ocorrências com manutenção
8. IX_Viagem_StatusOcorrencia_DataFinal - Listagem de ocorrências

TABELA MULTA (6 índices novos):
1. IX_Multa_MotoristaId - Filtro por motorista
2. IX_Multa_VeiculoId - Filtro por veículo
3. IX_Multa_Status_Data - Filtro + ordenacao
4. IX_Multa_OrgaoAutuanteId - Filtro por órgão
5. IX_Multa_TipoMultaId - Filtro por tipo
6. IX_Multa_Data_Completo - Listagem geral

TABELA MANUTENCAO (3 índices novos):
1. IX_Manutencao_VeiculoId_DataSolicitacao - Histórico por veículo
2. IX_Manutencao_StatusOS - Filtro por status
3. IX_Manutencao_DataSolicitacao - Ordenação principal

TABELA ITENSMANUTENCAO (2 índices novos):
1. IX_ItensManutencao_ManutencaoId - FK lookup
2. IX_ItensManutencao_ViagemId - FK lookup

TABELA EMPENHO (2 índices novos):
1. IX_Empenho_ContratoId - Busca por contrato
2. IX_Empenho_AtaId - Busca por ata

TABELA LOTACAOMOTORISTA (2 índices novos):
1. IX_LotacaoMotorista_MotoristaId_UnidadeId - Composto
2. IX_LotacaoMotorista_UnidadeId - Busca por unidade

TABELA EVENTO (3 índices novos):
1. IX_Evento_SetorSolicitanteId_DataInicial - Dashboard
2. IX_Evento_DataInicial - Range de datas
3. IX_Evento_Nome - Busca por nome

TABELA LAVAGEM (2 índices novos):
1. IX_Lavagem_VeiculoId_Data - Histórico por veículo
2. IX_Lavagem_MotoristaId - Filtro por motorista

TABELA VIAGEMESTATISTICA (1 índice novo):
1. IX_ViagemEstatistica_DataReferencia_Custos - Dashboard

TABELA VEICULO (3 índices novos):
1. IX_Veiculo_ContratoId - Veículos por contrato
2. IX_Veiculo_UnidadeId_Status - Veículos por unidade
3. IX_Veiculo_Placa - Busca direta

TABELA MOTORISTA (2 índices novos):
1. IX_Motorista_ContratoId_Status - Terceirizados ativos
2. IX_Motorista_Status_Nome - Listagem ordenada
*/
