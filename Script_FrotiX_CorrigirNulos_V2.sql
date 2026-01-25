-- ============================================================================
-- SCRIPT COMPLETO: Correção de NULLs no Banco FrotiX (CORRIGIDO V2)
-- Data: 29/11/2025
-- ============================================================================
-- ESTRATÉGIA: Usar apenas DEFAULT CONSTRAINTS (SEM triggers novos)
-- MOTIVO: Entity Framework usa OUTPUT que conflita com triggers
-- ============================================================================

USE Frotix
GO

SET NOCOUNT ON
GO

PRINT '============================================================================'
PRINT '=== INICIANDO CORREÇÃO DE NULLs NO BANCO FROTIX ==='
PRINT '=== Data: ' + CONVERT(VARCHAR, GETDATE(), 120)
PRINT '============================================================================'
GO

-- ============================================================================
-- PARTE 1: REMOVER TRIGGERS PROBLEMÁTICOS (se existirem)
-- ============================================================================

PRINT ''
PRINT '=== PARTE 1: REMOVENDO TRIGGERS DE PREENCHIMENTO DE NULOS ==='

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_AlertasFrotiX_PreencherNulos')
BEGIN
    DROP TRIGGER TR_AlertasFrotiX_PreencherNulos;
    PRINT 'TR_AlertasFrotiX_PreencherNulos removido'
END

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_AlertasUsuario_PreencherNulos')
BEGIN
    DROP TRIGGER TR_AlertasUsuario_PreencherNulos;
    PRINT 'TR_AlertasUsuario_PreencherNulos removido'
END

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Viagem_PreencherNulos')
BEGIN
    DROP TRIGGER TR_Viagem_PreencherNulos;
    PRINT 'TR_Viagem_PreencherNulos removido'
END

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Manutencao_PreencherNulos')
BEGIN
    DROP TRIGGER TR_Manutencao_PreencherNulos;
    PRINT 'TR_Manutencao_PreencherNulos removido'
END

PRINT 'Triggers de preenchimento removidos (se existiam)'
GO

-- ============================================================================
-- PARTE 2: FUNÇÃO AUXILIAR PARA VERIFICAR SE COLUNA JÁ TEM DEFAULT
-- ============================================================================

IF OBJECT_ID('dbo.fn_ColunaTemDefault', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_ColunaTemDefault
GO

CREATE FUNCTION dbo.fn_ColunaTemDefault(@Tabela NVARCHAR(128), @Coluna NVARCHAR(128))
RETURNS BIT
AS
BEGIN
    DECLARE @Resultado BIT = 0
    
    IF EXISTS (
        SELECT 1 
        FROM sys.default_constraints dc 
        JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
        WHERE dc.parent_object_id = OBJECT_ID(@Tabela) AND c.name = @Coluna
    )
        SET @Resultado = 1
    
    RETURN @Resultado
END
GO

-- ============================================================================
-- PARTE 3: TABELA AlertasFrotiX - Adicionar DEFAULTs faltantes
-- ============================================================================

PRINT ''
PRINT '=== PARTE 3: AlertasFrotiX - DEFAULTS ==='

IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Recorrente') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Recorrente2 DEFAULT 'N' FOR Recorrente;

IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Monday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Monday2 DEFAULT 0 FOR Monday;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Tuesday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Tuesday2 DEFAULT 0 FOR Tuesday;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Wednesday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Wednesday2 DEFAULT 0 FOR Wednesday;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Thursday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Thursday2 DEFAULT 0 FOR Thursday;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Friday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Friday2 DEFAULT 0 FOR Friday;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Saturday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Saturday2 DEFAULT 0 FOR Saturday;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'Sunday') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Sunday2 DEFAULT 0 FOR Sunday;

IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'DiasSemana') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_DiasSemana2 DEFAULT '' FOR DiasSemana;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'DatasSelecionadas') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_DatasSelecionadas2 DEFAULT '' FOR DatasSelecionadas;

IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'DiaMesRecorrencia') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_DiaMesRecorrencia2 DEFAULT 0 FOR DiaMesRecorrencia;

IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'DesativadoPor') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_DesativadoPor2 DEFAULT '' FOR DesativadoPor;
IF dbo.fn_ColunaTemDefault('AlertasFrotiX', 'MotivoDesativacao') = 0
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_MotivoDesativacao2 DEFAULT '' FOR MotivoDesativacao;

PRINT 'AlertasFrotiX: Defaults verificados/adicionados'
GO

UPDATE AlertasFrotiX SET
    Recorrente = ISNULL(Recorrente, 'N'),
    Monday = ISNULL(Monday, 0),
    Tuesday = ISNULL(Tuesday, 0),
    Wednesday = ISNULL(Wednesday, 0),
    Thursday = ISNULL(Thursday, 0),
    Friday = ISNULL(Friday, 0),
    Saturday = ISNULL(Saturday, 0),
    Sunday = ISNULL(Sunday, 0),
    DiasSemana = ISNULL(DiasSemana, ''),
    DatasSelecionadas = ISNULL(DatasSelecionadas, ''),
    DesativadoPor = ISNULL(DesativadoPor, ''),
    MotivoDesativacao = ISNULL(MotivoDesativacao, '')
WHERE Recorrente IS NULL OR Monday IS NULL OR Tuesday IS NULL OR Wednesday IS NULL
    OR Thursday IS NULL OR Friday IS NULL OR Saturday IS NULL OR Sunday IS NULL
    OR DiasSemana IS NULL OR DatasSelecionadas IS NULL
    OR DesativadoPor IS NULL OR MotivoDesativacao IS NULL;

PRINT 'AlertasFrotiX: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 4: TABELA AlertasUsuario - Adicionar DEFAULTs faltantes
-- ============================================================================

PRINT ''
PRINT '=== PARTE 4: AlertasUsuario - DEFAULTS ==='

IF dbo.fn_ColunaTemDefault('AlertasUsuario', 'Lido') = 0
    ALTER TABLE AlertasUsuario ADD CONSTRAINT DF_AlertasUsuario_Lido2 DEFAULT 0 FOR Lido;
IF dbo.fn_ColunaTemDefault('AlertasUsuario', 'Notificado') = 0
    ALTER TABLE AlertasUsuario ADD CONSTRAINT DF_AlertasUsuario_Notificado2 DEFAULT 0 FOR Notificado;
IF dbo.fn_ColunaTemDefault('AlertasUsuario', 'Apagado') = 0
    ALTER TABLE AlertasUsuario ADD CONSTRAINT DF_AlertasUsuario_Apagado2 DEFAULT 0 FOR Apagado;

PRINT 'AlertasUsuario: Defaults verificados/adicionados'
GO

UPDATE AlertasUsuario SET
    Lido = ISNULL(Lido, 0),
    Notificado = ISNULL(Notificado, 0),
    Apagado = ISNULL(Apagado, 0)
WHERE Lido IS NULL OR Notificado IS NULL OR Apagado IS NULL;

PRINT 'AlertasUsuario: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 5: TABELA Manutencao - Adicionar DEFAULTs faltantes
-- ============================================================================

PRINT ''
PRINT '=== PARTE 5: Manutencao - DEFAULTS ==='

IF dbo.fn_ColunaTemDefault('Manutencao', 'ResumoOS') = 0
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_ResumoOS2 DEFAULT '' FOR ResumoOS;
IF dbo.fn_ColunaTemDefault('Manutencao', 'StatusOS') = 0
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_StatusOS2 DEFAULT '' FOR StatusOS;
IF dbo.fn_ColunaTemDefault('Manutencao', 'ReservaEnviado') = 0
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_ReservaEnviado2 DEFAULT 0 FOR ReservaEnviado;
IF dbo.fn_ColunaTemDefault('Manutencao', 'ManutencaoPreventiva') = 0
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_ManutencaoPreventiva2 DEFAULT 0 FOR ManutencaoPreventiva;
IF dbo.fn_ColunaTemDefault('Manutencao', 'QuilometragemManutencao') = 0
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_QuilometragemManutencao2 DEFAULT 0 FOR QuilometragemManutencao;

PRINT 'Manutencao: Defaults verificados/adicionados'
GO

UPDATE Manutencao SET
    ResumoOS = ISNULL(ResumoOS, ''),
    StatusOS = ISNULL(StatusOS, ''),
    ReservaEnviado = ISNULL(ReservaEnviado, 0),
    ManutencaoPreventiva = ISNULL(ManutencaoPreventiva, 0),
    QuilometragemManutencao = ISNULL(QuilometragemManutencao, 0)
WHERE ResumoOS IS NULL OR StatusOS IS NULL OR ReservaEnviado IS NULL
    OR ManutencaoPreventiva IS NULL OR QuilometragemManutencao IS NULL;

PRINT 'Manutencao: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 6: TABELA Viagem - Verificar DEFAULTs (já tem muitos)
-- ============================================================================

PRINT ''
PRINT '=== PARTE 6: Viagem - DEFAULTS ==='

-- Campos VARCHAR
IF dbo.fn_ColunaTemDefault('Viagem', 'CombustivelInicial') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CombustivelInicial2 DEFAULT '' FOR CombustivelInicial;
IF dbo.fn_ColunaTemDefault('Viagem', 'CombustivelFinal') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CombustivelFinal2 DEFAULT '' FOR CombustivelFinal;
IF dbo.fn_ColunaTemDefault('Viagem', 'Descricao') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Descricao2 DEFAULT '' FOR Descricao;
IF dbo.fn_ColunaTemDefault('Viagem', 'RamalRequisitante') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_RamalRequisitante2 DEFAULT '' FOR RamalRequisitante;
IF dbo.fn_ColunaTemDefault('Viagem', 'Finalidade') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Finalidade2 DEFAULT '' FOR Finalidade;
IF dbo.fn_ColunaTemDefault('Viagem', 'Status') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Status2 DEFAULT '' FOR Status;
IF dbo.fn_ColunaTemDefault('Viagem', 'ResumoOcorrencia') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_ResumoOcorrencia2 DEFAULT '' FOR ResumoOcorrencia;
IF dbo.fn_ColunaTemDefault('Viagem', 'DescricaoOcorrencia') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DescricaoOcorrencia2 DEFAULT '' FOR DescricaoOcorrencia;
IF dbo.fn_ColunaTemDefault('Viagem', 'StatusOcorrencia') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusOcorrencia2 DEFAULT '' FOR StatusOcorrencia;
IF dbo.fn_ColunaTemDefault('Viagem', 'StatusDocumento') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusDocumento2 DEFAULT '' FOR StatusDocumento;
IF dbo.fn_ColunaTemDefault('Viagem', 'StatusCartaoAbastecimento') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusCartaoAbastecimento2 DEFAULT '' FOR StatusCartaoAbastecimento;
IF dbo.fn_ColunaTemDefault('Viagem', 'Origem') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Origem2 DEFAULT '' FOR Origem;
IF dbo.fn_ColunaTemDefault('Viagem', 'Destino') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Destino2 DEFAULT '' FOR Destino;
IF dbo.fn_ColunaTemDefault('Viagem', 'DescricaoSemFormato') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DescricaoSemFormato2 DEFAULT '' FOR DescricaoSemFormato;
IF dbo.fn_ColunaTemDefault('Viagem', 'NomeEvento') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_NomeEvento2 DEFAULT '' FOR NomeEvento;
IF dbo.fn_ColunaTemDefault('Viagem', 'DescricaoSolucaoOcorrencia') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DescricaoSolucaoOcorrencia2 DEFAULT '' FOR DescricaoSolucaoOcorrencia;
IF dbo.fn_ColunaTemDefault('Viagem', 'Recorrente') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Recorrente2 DEFAULT '' FOR Recorrente;
IF dbo.fn_ColunaTemDefault('Viagem', 'Intervalo') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Intervalo2 DEFAULT '' FOR Intervalo;
IF dbo.fn_ColunaTemDefault('Viagem', 'AgendamentoTMP') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_AgendamentoTMP2 DEFAULT '' FOR AgendamentoTMP;
IF dbo.fn_ColunaTemDefault('Viagem', 'DatasSelecionadas') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DatasSelecionadas2 DEFAULT '' FOR DatasSelecionadas;
IF dbo.fn_ColunaTemDefault('Viagem', 'Rubrica') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Rubrica2 DEFAULT '' FOR Rubrica;
IF dbo.fn_ColunaTemDefault('Viagem', 'DanoAvaria') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DanoAvaria2 DEFAULT '' FOR DanoAvaria;
IF dbo.fn_ColunaTemDefault('Viagem', 'StatusDocumentoFinal') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusDocumentoFinal2 DEFAULT '' FOR StatusDocumentoFinal;
IF dbo.fn_ColunaTemDefault('Viagem', 'StatusCartaoAbastecimentoFinal') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusCartaoAbastecimentoFinal2 DEFAULT '' FOR StatusCartaoAbastecimentoFinal;
IF dbo.fn_ColunaTemDefault('Viagem', 'RubricaFinal') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_RubricaFinal2 DEFAULT '' FOR RubricaFinal;
IF dbo.fn_ColunaTemDefault('Viagem', 'DanoAvariaFinal') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DanoAvariaFinal2 DEFAULT '' FOR DanoAvariaFinal;

-- Campos INT
IF dbo.fn_ColunaTemDefault('Viagem', 'KmInicial') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_KmInicial2 DEFAULT 0 FOR KmInicial;
IF dbo.fn_ColunaTemDefault('Viagem', 'KmFinal') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_KmFinal2 DEFAULT 0 FOR KmFinal;
IF dbo.fn_ColunaTemDefault('Viagem', 'NoFichaVistoria') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_NoFichaVistoria2 DEFAULT 0 FOR NoFichaVistoria;
IF dbo.fn_ColunaTemDefault('Viagem', 'Minutos') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Minutos2 DEFAULT 0 FOR Minutos;
IF dbo.fn_ColunaTemDefault('Viagem', 'DiaMesRecorrencia') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DiaMesRecorrencia2 DEFAULT 0 FOR DiaMesRecorrencia;

-- Campos FLOAT (Custos)
IF dbo.fn_ColunaTemDefault('Viagem', 'CustoCombustivel') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoCombustivel2 DEFAULT 0 FOR CustoCombustivel;
IF dbo.fn_ColunaTemDefault('Viagem', 'CustoMotorista') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoMotorista2 DEFAULT 0 FOR CustoMotorista;
IF dbo.fn_ColunaTemDefault('Viagem', 'CustoVeiculo') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoVeiculo2 DEFAULT 0 FOR CustoVeiculo;
IF dbo.fn_ColunaTemDefault('Viagem', 'CustoOperador') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoOperador2 DEFAULT 0 FOR CustoOperador;
IF dbo.fn_ColunaTemDefault('Viagem', 'CustoLavador') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoLavador2 DEFAULT 0 FOR CustoLavador;

-- Campos BIT
IF dbo.fn_ColunaTemDefault('Viagem', 'Monday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Monday2 DEFAULT 0 FOR Monday;
IF dbo.fn_ColunaTemDefault('Viagem', 'Tuesday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Tuesday2 DEFAULT 0 FOR Tuesday;
IF dbo.fn_ColunaTemDefault('Viagem', 'Wednesday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Wednesday2 DEFAULT 0 FOR Wednesday;
IF dbo.fn_ColunaTemDefault('Viagem', 'Thursday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Thursday2 DEFAULT 0 FOR Thursday;
IF dbo.fn_ColunaTemDefault('Viagem', 'Friday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Friday2 DEFAULT 0 FOR Friday;
IF dbo.fn_ColunaTemDefault('Viagem', 'Saturday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Saturday2 DEFAULT 0 FOR Saturday;
IF dbo.fn_ColunaTemDefault('Viagem', 'Sunday') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Sunday2 DEFAULT 0 FOR Sunday;
IF dbo.fn_ColunaTemDefault('Viagem', 'StatusAgendamento') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusAgendamento2 DEFAULT 0 FOR StatusAgendamento;
IF dbo.fn_ColunaTemDefault('Viagem', 'FoiAgendamento') = 0
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_FoiAgendamento2 DEFAULT 0 FOR FoiAgendamento;

PRINT 'Viagem: Defaults verificados/adicionados'
GO

PRINT 'Viagem: Atualizando registros existentes...'
GO

UPDATE Viagem SET
    CombustivelInicial = ISNULL(CombustivelInicial, ''),
    CombustivelFinal = ISNULL(CombustivelFinal, ''),
    Descricao = ISNULL(Descricao, ''),
    RamalRequisitante = ISNULL(RamalRequisitante, ''),
    Finalidade = ISNULL(Finalidade, ''),
    Status = ISNULL(Status, ''),
    ResumoOcorrencia = ISNULL(ResumoOcorrencia, ''),
    DescricaoOcorrencia = ISNULL(DescricaoOcorrencia, ''),
    StatusOcorrencia = ISNULL(StatusOcorrencia, ''),
    StatusDocumento = ISNULL(StatusDocumento, ''),
    StatusCartaoAbastecimento = ISNULL(StatusCartaoAbastecimento, ''),
    Origem = ISNULL(Origem, ''),
    Destino = ISNULL(Destino, ''),
    DescricaoSemFormato = ISNULL(DescricaoSemFormato, ''),
    NomeEvento = ISNULL(NomeEvento, ''),
    DescricaoSolucaoOcorrencia = ISNULL(DescricaoSolucaoOcorrencia, ''),
    ImagemOcorrencia = ISNULL(ImagemOcorrencia, 'semimagem.jpg'),
    Recorrente = ISNULL(Recorrente, ''),
    Intervalo = ISNULL(Intervalo, ''),
    AgendamentoTMP = ISNULL(AgendamentoTMP, ''),
    DatasSelecionadas = ISNULL(DatasSelecionadas, ''),
    Rubrica = ISNULL(Rubrica, ''),
    DanoAvaria = ISNULL(DanoAvaria, ''),
    StatusDocumentoFinal = ISNULL(StatusDocumentoFinal, ''),
    StatusCartaoAbastecimentoFinal = ISNULL(StatusCartaoAbastecimentoFinal, ''),
    RubricaFinal = ISNULL(RubricaFinal, ''),
    DanoAvariaFinal = ISNULL(DanoAvariaFinal, ''),
    KmInicial = ISNULL(KmInicial, 0),
    KmFinal = ISNULL(KmFinal, 0),
    NoFichaVistoria = ISNULL(NoFichaVistoria, 0),
    Minutos = ISNULL(Minutos, 0),
    DiaMesRecorrencia = ISNULL(DiaMesRecorrencia, 0),
    CustoCombustivel = ISNULL(CustoCombustivel, 0),
    CustoMotorista = ISNULL(CustoMotorista, 0),
    CustoVeiculo = ISNULL(CustoVeiculo, 0),
    CustoOperador = ISNULL(CustoOperador, 0),
    CustoLavador = ISNULL(CustoLavador, 0),
    Monday = ISNULL(Monday, 0),
    Tuesday = ISNULL(Tuesday, 0),
    Wednesday = ISNULL(Wednesday, 0),
    Thursday = ISNULL(Thursday, 0),
    Friday = ISNULL(Friday, 0),
    Saturday = ISNULL(Saturday, 0),
    Sunday = ISNULL(Sunday, 0),
    StatusAgendamento = ISNULL(StatusAgendamento, 0),
    FoiAgendamento = ISNULL(FoiAgendamento, 0);

PRINT 'Viagem: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 7: TABELA Motorista - Verificar DEFAULTs
-- ============================================================================

PRINT ''
PRINT '=== PARTE 7: Motorista - DEFAULTS ==='

IF dbo.fn_ColunaTemDefault('Motorista', 'Nome') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_Nome2 DEFAULT '' FOR Nome;
IF dbo.fn_ColunaTemDefault('Motorista', 'Ponto') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_Ponto2 DEFAULT '' FOR Ponto;
IF dbo.fn_ColunaTemDefault('Motorista', 'CPF') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_CPF2 DEFAULT '' FOR CPF;
IF dbo.fn_ColunaTemDefault('Motorista', 'CNH') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_CNH2 DEFAULT '' FOR CNH;
IF dbo.fn_ColunaTemDefault('Motorista', 'CategoriaCNH') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_CategoriaCNH2 DEFAULT '' FOR CategoriaCNH;
IF dbo.fn_ColunaTemDefault('Motorista', 'Celular01') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_Celular012 DEFAULT '' FOR Celular01;
IF dbo.fn_ColunaTemDefault('Motorista', 'Celular02') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_Celular022 DEFAULT '' FOR Celular02;
IF dbo.fn_ColunaTemDefault('Motorista', 'Status') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_Status2 DEFAULT 1 FOR Status;
IF dbo.fn_ColunaTemDefault('Motorista', 'TipoCondutor') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_TipoCondutor2 DEFAULT '' FOR TipoCondutor;
IF dbo.fn_ColunaTemDefault('Motorista', 'EfetivoFerista') = 0
    ALTER TABLE Motorista ADD CONSTRAINT DF_Motorista_EfetivoFerista2 DEFAULT '' FOR EfetivoFerista;

PRINT 'Motorista: Defaults verificados/adicionados'
GO

UPDATE Motorista SET
    Nome = ISNULL(Nome, ''),
    Ponto = ISNULL(Ponto, ''),
    CPF = ISNULL(CPF, ''),
    CNH = ISNULL(CNH, ''),
    CategoriaCNH = ISNULL(CategoriaCNH, ''),
    Celular01 = ISNULL(Celular01, ''),
    Celular02 = ISNULL(Celular02, ''),
    TipoCondutor = ISNULL(TipoCondutor, ''),
    EfetivoFerista = ISNULL(EfetivoFerista, '')
WHERE Nome IS NULL OR Ponto IS NULL OR CPF IS NULL OR CNH IS NULL 
    OR CategoriaCNH IS NULL OR Celular01 IS NULL OR Celular02 IS NULL
    OR TipoCondutor IS NULL OR EfetivoFerista IS NULL;

PRINT 'Motorista: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 8: TABELA Veiculo - DEFAULTS (colunas corretas)
-- ============================================================================

PRINT ''
PRINT '=== PARTE 8: Veiculo - DEFAULTS ==='

-- Colunas que EXISTEM na tabela Veiculo:
-- Placa (NOT NULL), Quilometragem, Renavam, PlacaVinculada, AnoFabricacao, AnoModelo,
-- Reserva, VeiculoProprio, Status, Patrimonio, Categoria, Economildo, ValorMensal

IF dbo.fn_ColunaTemDefault('Veiculo', 'Renavam') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Renavam2 DEFAULT '' FOR Renavam;
IF dbo.fn_ColunaTemDefault('Veiculo', 'PlacaVinculada') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_PlacaVinculada2 DEFAULT '' FOR PlacaVinculada;
IF dbo.fn_ColunaTemDefault('Veiculo', 'Patrimonio') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Patrimonio2 DEFAULT '' FOR Patrimonio;
IF dbo.fn_ColunaTemDefault('Veiculo', 'Categoria') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Categoria2 DEFAULT '' FOR Categoria;
IF dbo.fn_ColunaTemDefault('Veiculo', 'Quilometragem') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Quilometragem2 DEFAULT 0 FOR Quilometragem;
IF dbo.fn_ColunaTemDefault('Veiculo', 'AnoFabricacao') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_AnoFabricacao2 DEFAULT 0 FOR AnoFabricacao;
IF dbo.fn_ColunaTemDefault('Veiculo', 'AnoModelo') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_AnoModelo2 DEFAULT 0 FOR AnoModelo;
IF dbo.fn_ColunaTemDefault('Veiculo', 'Reserva') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Reserva2 DEFAULT 0 FOR Reserva;
IF dbo.fn_ColunaTemDefault('Veiculo', 'VeiculoProprio') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_VeiculoProprio2 DEFAULT 0 FOR VeiculoProprio;
IF dbo.fn_ColunaTemDefault('Veiculo', 'Status') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Status2 DEFAULT 1 FOR Status;
IF dbo.fn_ColunaTemDefault('Veiculo', 'Economildo') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_Economildo2 DEFAULT 0 FOR Economildo;
IF dbo.fn_ColunaTemDefault('Veiculo', 'ValorMensal') = 0
    ALTER TABLE Veiculo ADD CONSTRAINT DF_Veiculo_ValorMensal2 DEFAULT 0 FOR ValorMensal;

PRINT 'Veiculo: Defaults verificados/adicionados'
GO

UPDATE Veiculo SET
    Renavam = ISNULL(Renavam, ''),
    PlacaVinculada = ISNULL(PlacaVinculada, ''),
    Patrimonio = ISNULL(Patrimonio, ''),
    Categoria = ISNULL(Categoria, ''),
    Quilometragem = ISNULL(Quilometragem, 0),
    AnoFabricacao = ISNULL(AnoFabricacao, 0),
    AnoModelo = ISNULL(AnoModelo, 0),
    Reserva = ISNULL(Reserva, 0),
    VeiculoProprio = ISNULL(VeiculoProprio, 0),
    Economildo = ISNULL(Economildo, 0),
    ValorMensal = ISNULL(ValorMensal, 0)
WHERE Renavam IS NULL OR PlacaVinculada IS NULL OR Patrimonio IS NULL OR Categoria IS NULL
    OR Quilometragem IS NULL OR AnoFabricacao IS NULL OR AnoModelo IS NULL
    OR Reserva IS NULL OR VeiculoProprio IS NULL OR Economildo IS NULL OR ValorMensal IS NULL;

PRINT 'Veiculo: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 9: TABELA Multa - DEFAULTS
-- ============================================================================

PRINT ''
PRINT '=== PARTE 9: Multa - DEFAULTS ==='

IF dbo.fn_ColunaTemDefault('Multa', 'Observacao') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_Observacao2 DEFAULT '' FOR Observacao;
IF dbo.fn_ColunaTemDefault('Multa', 'AutuacaoPDF') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_AutuacaoPDF2 DEFAULT '' FOR AutuacaoPDF;
IF dbo.fn_ColunaTemDefault('Multa', 'PenalidadePDF') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_PenalidadePDF2 DEFAULT '' FOR PenalidadePDF;
IF dbo.fn_ColunaTemDefault('Multa', 'ComprovantePDF') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_ComprovantePDF2 DEFAULT '' FOR ComprovantePDF;
IF dbo.fn_ColunaTemDefault('Multa', 'Fase') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_Fase2 DEFAULT '' FOR Fase;
IF dbo.fn_ColunaTemDefault('Multa', 'ProcessoEDoc') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_ProcessoEDoc2 DEFAULT '' FOR ProcessoEDoc;
IF dbo.fn_ColunaTemDefault('Multa', 'Status') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_Status2 DEFAULT '' FOR Status;
IF dbo.fn_ColunaTemDefault('Multa', 'FormaPagamento') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_FormaPagamento2 DEFAULT '' FOR FormaPagamento;
IF dbo.fn_ColunaTemDefault('Multa', 'ValorAteVencimento') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_ValorAteVencimento2 DEFAULT 0 FOR ValorAteVencimento;
IF dbo.fn_ColunaTemDefault('Multa', 'ValorPosVencimento') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_ValorPosVencimento2 DEFAULT 0 FOR ValorPosVencimento;
IF dbo.fn_ColunaTemDefault('Multa', 'ValorPago') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_ValorPago2 DEFAULT 0 FOR ValorPago;
IF dbo.fn_ColunaTemDefault('Multa', 'NoFichaVistoria') = 0
    ALTER TABLE Multa ADD CONSTRAINT DF_Multa_NoFichaVistoria2 DEFAULT 0 FOR NoFichaVistoria;

PRINT 'Multa: Defaults verificados/adicionados'
GO

UPDATE Multa SET
    Observacao = ISNULL(Observacao, ''),
    AutuacaoPDF = ISNULL(AutuacaoPDF, ''),
    PenalidadePDF = ISNULL(PenalidadePDF, ''),
    ComprovantePDF = ISNULL(ComprovantePDF, ''),
    Fase = ISNULL(Fase, ''),
    ProcessoEDoc = ISNULL(ProcessoEDoc, ''),
    Status = ISNULL(Status, ''),
    FormaPagamento = ISNULL(FormaPagamento, ''),
    ValorAteVencimento = ISNULL(ValorAteVencimento, 0),
    ValorPosVencimento = ISNULL(ValorPosVencimento, 0),
    ValorPago = ISNULL(ValorPago, 0),
    NoFichaVistoria = ISNULL(NoFichaVistoria, 0)
WHERE Observacao IS NULL OR AutuacaoPDF IS NULL OR PenalidadePDF IS NULL
    OR ComprovantePDF IS NULL OR Fase IS NULL OR ProcessoEDoc IS NULL
    OR Status IS NULL OR FormaPagamento IS NULL
    OR ValorAteVencimento IS NULL OR ValorPosVencimento IS NULL 
    OR ValorPago IS NULL OR NoFichaVistoria IS NULL;

PRINT 'Multa: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 10: TABELAS Lavador e Operador - DEFAULTS
-- ============================================================================

PRINT ''
PRINT '=== PARTE 10: Lavador e Operador - DEFAULTS ==='

-- Lavador
IF dbo.fn_ColunaTemDefault('Lavador', 'Nome') = 0
    ALTER TABLE Lavador ADD CONSTRAINT DF_Lavador_Nome2 DEFAULT '' FOR Nome;
IF dbo.fn_ColunaTemDefault('Lavador', 'Ponto') = 0
    ALTER TABLE Lavador ADD CONSTRAINT DF_Lavador_Ponto2 DEFAULT '' FOR Ponto;
IF dbo.fn_ColunaTemDefault('Lavador', 'CPF') = 0
    ALTER TABLE Lavador ADD CONSTRAINT DF_Lavador_CPF2 DEFAULT '' FOR CPF;
IF dbo.fn_ColunaTemDefault('Lavador', 'Celular01') = 0
    ALTER TABLE Lavador ADD CONSTRAINT DF_Lavador_Celular012 DEFAULT '' FOR Celular01;
IF dbo.fn_ColunaTemDefault('Lavador', 'Celular02') = 0
    ALTER TABLE Lavador ADD CONSTRAINT DF_Lavador_Celular022 DEFAULT '' FOR Celular02;

PRINT 'Lavador: Defaults verificados'
GO

UPDATE Lavador SET
    Nome = ISNULL(Nome, ''),
    Ponto = ISNULL(Ponto, ''),
    CPF = ISNULL(CPF, ''),
    Celular01 = ISNULL(Celular01, ''),
    Celular02 = ISNULL(Celular02, '')
WHERE Nome IS NULL OR Ponto IS NULL OR CPF IS NULL 
    OR Celular01 IS NULL OR Celular02 IS NULL;

PRINT 'Lavador: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- Operador
IF dbo.fn_ColunaTemDefault('Operador', 'Nome') = 0
    ALTER TABLE Operador ADD CONSTRAINT DF_Operador_Nome2 DEFAULT '' FOR Nome;
IF dbo.fn_ColunaTemDefault('Operador', 'Ponto') = 0
    ALTER TABLE Operador ADD CONSTRAINT DF_Operador_Ponto2 DEFAULT '' FOR Ponto;
IF dbo.fn_ColunaTemDefault('Operador', 'CPF') = 0
    ALTER TABLE Operador ADD CONSTRAINT DF_Operador_CPF2 DEFAULT '' FOR CPF;
IF dbo.fn_ColunaTemDefault('Operador', 'Celular01') = 0
    ALTER TABLE Operador ADD CONSTRAINT DF_Operador_Celular012 DEFAULT '' FOR Celular01;
IF dbo.fn_ColunaTemDefault('Operador', 'Celular02') = 0
    ALTER TABLE Operador ADD CONSTRAINT DF_Operador_Celular022 DEFAULT '' FOR Celular02;

PRINT 'Operador: Defaults verificados'
GO

UPDATE Operador SET
    Nome = ISNULL(Nome, ''),
    Ponto = ISNULL(Ponto, ''),
    CPF = ISNULL(CPF, ''),
    Celular01 = ISNULL(Celular01, ''),
    Celular02 = ISNULL(Celular02, '')
WHERE Nome IS NULL OR Ponto IS NULL OR CPF IS NULL 
    OR Celular01 IS NULL OR Celular02 IS NULL;

PRINT 'Operador: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' registros atualizados'
GO

-- ============================================================================
-- PARTE 11: LIMPEZA - Remover função auxiliar
-- ============================================================================

DROP FUNCTION dbo.fn_ColunaTemDefault
GO

-- ============================================================================
-- PARTE 12: VERIFICAÇÃO FINAL
-- ============================================================================

PRINT ''
PRINT '=== VERIFICAÇÃO FINAL ==='

SELECT 'AlertasFrotiX' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN Recorrente IS NULL THEN 1 ELSE 0 END) AS RecorrenteNull,
    SUM(CASE WHEN Monday IS NULL THEN 1 ELSE 0 END) AS MondayNull,
    SUM(CASE WHEN DiasSemana IS NULL THEN 1 ELSE 0 END) AS DiasSemanaNull
FROM AlertasFrotiX;

SELECT 'AlertasUsuario' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN Lido IS NULL THEN 1 ELSE 0 END) AS LidoNull,
    SUM(CASE WHEN Notificado IS NULL THEN 1 ELSE 0 END) AS NotificadoNull,
    SUM(CASE WHEN Apagado IS NULL THEN 1 ELSE 0 END) AS ApagadoNull
FROM AlertasUsuario;

SELECT 'Viagem' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN Status IS NULL THEN 1 ELSE 0 END) AS StatusNull,
    SUM(CASE WHEN Monday IS NULL THEN 1 ELSE 0 END) AS MondayNull,
    SUM(CASE WHEN CustoCombustivel IS NULL THEN 1 ELSE 0 END) AS CustoNull
FROM Viagem;

SELECT 'Manutencao' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN ResumoOS IS NULL THEN 1 ELSE 0 END) AS ResumoOSNull,
    SUM(CASE WHEN StatusOS IS NULL THEN 1 ELSE 0 END) AS StatusOSNull
FROM Manutencao;

SELECT 'Veiculo' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN Renavam IS NULL THEN 1 ELSE 0 END) AS RenavamNull,
    SUM(CASE WHEN Economildo IS NULL THEN 1 ELSE 0 END) AS EconomildoNull
FROM Veiculo;

PRINT ''
PRINT '============================================================================'
PRINT '=== SCRIPT FINALIZADO COM SUCESSO ==='
PRINT '=== Data: ' + CONVERT(VARCHAR, GETDATE(), 120)
PRINT '============================================================================'
GO
