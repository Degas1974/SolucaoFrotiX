-- ============================================================================
-- SCRIPT: Correção de campos NULL nas tabelas Viagem e Manutencao
-- Data: 25/11/2025
-- Descrição: Usa DEFAULT constraints (sem triggers - compatível com EF)
-- NOTA: Campos de Data são IGNORADOS (permanecem NULL quando aplicável)
-- ============================================================================

-- ============================================================================
-- PARTE 1: REMOVER TRIGGERS SE EXISTIREM
-- ============================================================================

PRINT '=== REMOVENDO TRIGGERS ANTIGOS ==='

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
GO

-- ============================================================================
-- PARTE 2: ADICIONAR DEFAULT CONSTRAINTS EM MANUTENCAO
-- ============================================================================

PRINT '=== ADICIONANDO DEFAULTS EM MANUTENCAO ==='

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('Manutencao') AND c.name = 'ResumoOS')
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_ResumoOS DEFAULT '' FOR ResumoOS;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('Manutencao') AND c.name = 'StatusOS')
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_StatusOS DEFAULT '' FOR StatusOS;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('Manutencao') AND c.name = 'ReservaEnviado')
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_ReservaEnviado DEFAULT 0 FOR ReservaEnviado;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('Manutencao') AND c.name = 'ManutencaoPreventiva')
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_ManutencaoPreventiva DEFAULT 0 FOR ManutencaoPreventiva;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('Manutencao') AND c.name = 'QuilometragemManutencao')
    ALTER TABLE Manutencao ADD CONSTRAINT DF_Manutencao_QuilometragemManutencao DEFAULT 0 FOR QuilometragemManutencao;

PRINT 'Defaults Manutencao verificados'
GO

-- ============================================================================
-- PARTE 3: ADICIONAR DEFAULT CONSTRAINTS EM VIAGEM
-- ============================================================================

PRINT '=== ADICIONANDO DEFAULTS EM VIAGEM ==='

-- Campos VARCHAR
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CombustivelInicial')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CombustivelInicial DEFAULT '' FOR CombustivelInicial;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CombustivelFinal')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CombustivelFinal DEFAULT '' FOR CombustivelFinal;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Descricao')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Descricao DEFAULT '' FOR Descricao;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'RamalRequisitante')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_RamalRequisitante DEFAULT '' FOR RamalRequisitante;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Finalidade')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Finalidade DEFAULT '' FOR Finalidade;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Status')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Status DEFAULT '' FOR Status;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'ResumoOcorrencia')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_ResumoOcorrencia DEFAULT '' FOR ResumoOcorrencia;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DescricaoOcorrencia')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DescricaoOcorrencia DEFAULT '' FOR DescricaoOcorrencia;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'StatusOcorrencia')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusOcorrencia DEFAULT '' FOR StatusOcorrencia;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'StatusDocumento')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusDocumento DEFAULT '' FOR StatusDocumento;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'StatusCartaoAbastecimento')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusCartaoAbastecimento DEFAULT '' FOR StatusCartaoAbastecimento;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Origem')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Origem DEFAULT '' FOR Origem;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Destino')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Destino DEFAULT '' FOR Destino;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DescricaoSemFormato')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DescricaoSemFormato DEFAULT '' FOR DescricaoSemFormato;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'NomeEvento')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_NomeEvento DEFAULT '' FOR NomeEvento;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DescricaoSolucaoOcorrencia')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DescricaoSolucaoOcorrencia DEFAULT '' FOR DescricaoSolucaoOcorrencia;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Recorrente')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Recorrente DEFAULT '' FOR Recorrente;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Intervalo')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Intervalo DEFAULT '' FOR Intervalo;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'AgendamentoTMP')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_AgendamentoTMP DEFAULT '' FOR AgendamentoTMP;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DatasSelecionadas')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DatasSelecionadas DEFAULT '' FOR DatasSelecionadas;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Rubrica')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Rubrica DEFAULT '' FOR Rubrica;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DanoAvaria')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DanoAvaria DEFAULT '' FOR DanoAvaria;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'StatusDocumentoFinal')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusDocumentoFinal DEFAULT '' FOR StatusDocumentoFinal;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'StatusCartaoAbastecimentoFinal')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_StatusCartaoAbastecimentoFinal DEFAULT '' FOR StatusCartaoAbastecimentoFinal;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'RubricaFinal')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_RubricaFinal DEFAULT '' FOR RubricaFinal;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DanoAvariaFinal')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DanoAvariaFinal DEFAULT '' FOR DanoAvariaFinal;

-- Campos INT
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'KmInicial')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_KmInicial DEFAULT 0 FOR KmInicial;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'KmFinal')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_KmFinal DEFAULT 0 FOR KmFinal;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'NoFichaVistoria')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_NoFichaVistoria DEFAULT 0 FOR NoFichaVistoria;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Minutos')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Minutos DEFAULT 0 FOR Minutos;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'DiaMesRecorrencia')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_DiaMesRecorrencia DEFAULT 0 FOR DiaMesRecorrencia;

-- Campos FLOAT (Custos)
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CustoCombustivel')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoCombustivel DEFAULT 0 FOR CustoCombustivel;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CustoMotorista')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoMotorista DEFAULT 0 FOR CustoMotorista;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CustoVeiculo')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoVeiculo DEFAULT 0 FOR CustoVeiculo;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CustoOperador')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoOperador DEFAULT 0 FOR CustoOperador;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'CustoLavador')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_CustoLavador DEFAULT 0 FOR CustoLavador;

-- Campos BIT
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Monday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Monday DEFAULT 0 FOR Monday;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Tuesday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Tuesday DEFAULT 0 FOR Tuesday;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Wednesday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Wednesday DEFAULT 0 FOR Wednesday;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Thursday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Thursday DEFAULT 0 FOR Thursday;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Friday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Friday DEFAULT 0 FOR Friday;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Saturday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Saturday DEFAULT 0 FOR Saturday;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id WHERE dc.parent_object_id = OBJECT_ID('Viagem') AND c.name = 'Sunday')
    ALTER TABLE Viagem ADD CONSTRAINT DF_Viagem_Sunday DEFAULT 0 FOR Sunday;

PRINT 'Defaults Viagem verificados'
GO

-- ============================================================================
-- PARTE 4: ATUALIZAR REGISTROS EXISTENTES
-- ============================================================================

PRINT '=== ATUALIZANDO MANUTENCAO ==='

UPDATE Manutencao
SET 
    ResumoOS = ISNULL(ResumoOS, ''),
    StatusOS = ISNULL(StatusOS, ''),
    ReservaEnviado = ISNULL(ReservaEnviado, 0),
    ManutencaoPreventiva = ISNULL(ManutencaoPreventiva, 0),
    QuilometragemManutencao = ISNULL(QuilometragemManutencao, 0)
WHERE 
    ResumoOS IS NULL OR StatusOS IS NULL
    OR ReservaEnviado IS NULL OR ManutencaoPreventiva IS NULL
    OR QuilometragemManutencao IS NULL;

PRINT 'Manutencao atualizados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
GO

PRINT '=== ATUALIZANDO VIAGEM ==='

UPDATE Viagem
SET 
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
    Sunday = ISNULL(Sunday, 0);

PRINT 'Viagem atualizados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
GO

-- ============================================================================
-- PARTE 5: VERIFICAÇÃO FINAL
-- ============================================================================

PRINT '=== VERIFICAÇÃO FINAL ==='

SELECT 'Manutencao' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN ResumoOS IS NULL THEN 1 ELSE 0 END) AS ResumoOSNull,
    SUM(CASE WHEN StatusOS IS NULL THEN 1 ELSE 0 END) AS StatusOSNull
FROM Manutencao;

SELECT 'Viagem' AS Tabela, COUNT(*) AS Total,
    SUM(CASE WHEN Status IS NULL THEN 1 ELSE 0 END) AS StatusNull,
    SUM(CASE WHEN Origem IS NULL THEN 1 ELSE 0 END) AS OrigemNull,
    SUM(CASE WHEN Monday IS NULL THEN 1 ELSE 0 END) AS MondayNull
FROM Viagem;

PRINT '=== SCRIPT FINALIZADO ==='
GO