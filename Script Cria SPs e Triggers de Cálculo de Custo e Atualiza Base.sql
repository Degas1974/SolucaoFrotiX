-- ============================================================================
-- FROTIX - SCRIPT COMPLETO DE INSTALAÇÃO E RECÁLCULO
-- ============================================================================
-- Este script instala todas as funções, stored procedures, trigger e
-- executa o recálculo de toda a base de dados.
-- ============================================================================
-- FÓRMULAS IMPLEMENTADAS:
-- 
-- CustoCombustivel = KmRodado × (ValorUnitarioAbastecimento / ConsumoMedioVeiculo)
-- CustoVeiculo     = (ValorUnitarioItem / 30 / 24 / 60) × Minutos
-- CustoMotorista   = CustoMensalMotorista × (Minutos / 13200)
-- CustoLavador     = (CustoMensalLavador × QtdLavadores) / MediaMensalViagens
-- CustoOperador    = (CustoMensalOperador × QtdOperadores) / MediaMensalViagens
-- ============================================================================

USE [FrotiX]
GO

PRINT '============================================================================'
PRINT 'INICIANDO INSTALAÇÃO...'
PRINT '============================================================================'
GO

-- ============================================================================
-- 1. FUNÇÃO: Calcula o Consumo Médio do Veículo (Km/Litro)
-- ============================================================================
IF OBJECT_ID('dbo.fn_CalculaConsumoVeiculo', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculaConsumoVeiculo
GO

CREATE FUNCTION dbo.fn_CalculaConsumoVeiculo
(
    @VeiculoId UNIQUEIDENTIFIER
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @Consumo FLOAT

    SELECT @Consumo = 
        CASE 
            WHEN SUM(Litros) > 0 THEN SUM(KmRodado) / SUM(Litros)
            ELSE NULL
        END
    FROM Abastecimento
    WHERE VeiculoId = @VeiculoId
      AND Litros > 0
      AND KmRodado > 0

    RETURN @Consumo
END
GO

PRINT '✓ fn_CalculaConsumoVeiculo criada'
GO

-- ============================================================================
-- 2. FUNÇÃO: Obtém o Valor do Combustível mais próximo da data da viagem
-- ============================================================================
IF OBJECT_ID('dbo.fn_GetValorCombustivelProximo', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetValorCombustivelProximo
GO

CREATE FUNCTION dbo.fn_GetValorCombustivelProximo
(
    @VeiculoId UNIQUEIDENTIFIER,
    @DataViagem DATETIME
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @ValorUnitario FLOAT

    SELECT TOP 1 @ValorUnitario = ValorUnitario
    FROM Abastecimento
    WHERE VeiculoId = @VeiculoId
      AND ValorUnitario > 0
    ORDER BY ABS(DATEDIFF(MINUTE, DataHora, @DataViagem))

    RETURN @ValorUnitario
END
GO

PRINT '✓ fn_GetValorCombustivelProximo criada'
GO

-- ============================================================================
-- 3. FUNÇÃO: Calcula a Média Mensal de Viagens (para Lavador e Operador)
-- ============================================================================
IF OBJECT_ID('dbo.fn_CalculaMediaMensalViagens', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculaMediaMensalViagens
GO

CREATE FUNCTION dbo.fn_CalculaMediaMensalViagens
(
    @DataViagem DATETIME
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @TotalViagens INT
    DECLARE @PrimeiraViagem DATETIME
    DECLARE @TotalDias INT
    DECLARE @MediaDiaria FLOAT
    DECLARE @MediaMensal FLOAT

    SELECT @TotalViagens = COUNT(*),
           @PrimeiraViagem = MIN(DataInicial)
    FROM Viagem
    WHERE DataInicial < @DataViagem
      AND Status = 'Realizada'

    IF @TotalViagens = 0 OR @PrimeiraViagem IS NULL
        RETURN 1.0

    SET @TotalDias = DATEDIFF(DAY, @PrimeiraViagem, @DataViagem)
    
    IF @TotalDias <= 0
        SET @TotalDias = 1

    SET @MediaDiaria = CAST(@TotalViagens AS FLOAT) / CAST(@TotalDias AS FLOAT)
    SET @MediaMensal = @MediaDiaria * 30.0

    IF @MediaMensal < 0.1
        SET @MediaMensal = 0.1

    RETURN @MediaMensal
END
GO

PRINT '✓ fn_CalculaMediaMensalViagens criada'
GO

-- ============================================================================
-- 4. STORED PROCEDURE: Calcula todos os custos de uma viagem
-- ============================================================================
IF OBJECT_ID('dbo.sp_CalculaCustosViagem', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CalculaCustosViagem
GO

CREATE PROCEDURE dbo.sp_CalculaCustosViagem
    @ViagemId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @VeiculoId UNIQUEIDENTIFIER
    DECLARE @MotoristaId UNIQUEIDENTIFIER
    DECLARE @DataInicial DATETIME
    DECLARE @KmInicial INT
    DECLARE @KmFinal INT
    DECLARE @KmRodado INT
    DECLARE @Minutos INT
    DECLARE @Status VARCHAR(50)

    -- Custos calculados
    DECLARE @CustoCombustivel FLOAT = 0
    DECLARE @CustoVeiculo FLOAT = 0
    DECLARE @CustoMotorista FLOAT = 0
    DECLARE @CustoLavador FLOAT = 0
    DECLARE @CustoOperador FLOAT = 0

    -- Variáveis auxiliares
    DECLARE @ConsumoVeiculo FLOAT
    DECLARE @ValorCombustivel FLOAT
    DECLARE @ValorUnitarioVeiculo FLOAT
    DECLARE @CustoMensalMotorista FLOAT
    DECLARE @CustoMensalLavador FLOAT
    DECLARE @CustoMensalOperador FLOAT
    DECLARE @MediaMensalViagens FLOAT
    DECLARE @ItemVeiculoId UNIQUEIDENTIFIER
    DECLARE @ContratoMotoristaId UNIQUEIDENTIFIER
    DECLARE @QuantidadeLavador INT
    DECLARE @QuantidadeOperador INT

    -- ========================================================================
    -- Busca dados da viagem
    -- ========================================================================
    SELECT 
        @VeiculoId = VeiculoId,
        @MotoristaId = MotoristaId,
        @DataInicial = DataInicial,
        @KmInicial = ISNULL(KmInicial, 0),
        @KmFinal = ISNULL(KmFinal, 0),
        @Minutos = ISNULL(Minutos, 0),
        @Status = Status
    FROM Viagem
    WHERE ViagemId = @ViagemId

    IF @VeiculoId IS NULL
        RETURN

    SET @KmRodado = @KmFinal - @KmInicial
    IF @KmRodado < 0
        SET @KmRodado = 0

    -- ========================================================================
    -- 1. CUSTO COMBUSTÍVEL
    -- Fórmula: KmRodado × (ValorUnitarioAbastecimento / ConsumoMedioVeiculo)
    -- ========================================================================
    IF @KmRodado > 0 AND @VeiculoId IS NOT NULL
    BEGIN
        SET @ConsumoVeiculo = dbo.fn_CalculaConsumoVeiculo(@VeiculoId)
        SET @ValorCombustivel = dbo.fn_GetValorCombustivelProximo(@VeiculoId, @DataInicial)

        IF @ConsumoVeiculo IS NOT NULL AND @ConsumoVeiculo > 0 AND @ValorCombustivel IS NOT NULL
        BEGIN
            SET @CustoCombustivel = @KmRodado * (@ValorCombustivel / @ConsumoVeiculo)
        END
    END

    -- ========================================================================
    -- 2. CUSTO VEÍCULO
    -- Fórmula: (ValorUnitarioItem / 30 / 24 / 60) × Minutos
    -- ========================================================================
    IF @Minutos > 0 AND @VeiculoId IS NOT NULL
    BEGIN
        SELECT @ItemVeiculoId = ItemVeiculoId
        FROM Veiculo
        WHERE VeiculoId = @VeiculoId

        IF @ItemVeiculoId IS NOT NULL
        BEGIN
            SELECT @ValorUnitarioVeiculo = ValorUnitario
            FROM ItemVeiculoContrato
            WHERE ItemVeiculoId = @ItemVeiculoId

            IF @ValorUnitarioVeiculo IS NOT NULL AND @ValorUnitarioVeiculo > 0
            BEGIN
                SET @CustoVeiculo = (@ValorUnitarioVeiculo / 30.0 / 24.0 / 60.0) * @Minutos
            END
        END
    END

    -- ========================================================================
    -- 3. CUSTO MOTORISTA
    -- Fórmula: CustoMensalMotorista × (Minutos / 13200)
    -- ========================================================================
    IF @Minutos > 0 AND @MotoristaId IS NOT NULL
    BEGIN
        SELECT @ContratoMotoristaId = ContratoId
        FROM Motorista
        WHERE MotoristaId = @MotoristaId

        IF @ContratoMotoristaId IS NOT NULL
        BEGIN
            SELECT @CustoMensalMotorista = CustoMensalMotorista
            FROM Contrato
            WHERE ContratoId = @ContratoMotoristaId
              AND Status = 1

            IF @CustoMensalMotorista IS NOT NULL AND @CustoMensalMotorista > 0
            BEGIN
                SET @CustoMotorista = @CustoMensalMotorista * (CAST(@Minutos AS FLOAT) / 13200.0)
            END
        END
    END

    -- ========================================================================
    -- 4. CUSTO LAVADOR
    -- Fórmula: (CustoMensalLavador × QuantidadeLavador) / MediaMensalViagens
    -- ========================================================================
    IF @DataInicial IS NOT NULL
    BEGIN
        SELECT @CustoMensalLavador = CustoMensalLavador,
               @QuantidadeLavador = ISNULL(QuantidadeLavador, 1)
        FROM Contrato
        WHERE ContratoLavadores = 1
          AND Status = 1

        IF @CustoMensalLavador IS NOT NULL AND @CustoMensalLavador > 0
        BEGIN
            SET @MediaMensalViagens = dbo.fn_CalculaMediaMensalViagens(@DataInicial)
            SET @CustoLavador = (@CustoMensalLavador * @QuantidadeLavador) / @MediaMensalViagens
        END
    END

    -- ========================================================================
    -- 5. CUSTO OPERADOR
    -- Fórmula: (CustoMensalOperador × QuantidadeOperador) / MediaMensalViagens
    -- ========================================================================
    IF @DataInicial IS NOT NULL
    BEGIN
        SELECT @CustoMensalOperador = CustoMensalOperador,
               @QuantidadeOperador = ISNULL(QuantidadeOperador, 1)
        FROM Contrato
        WHERE ContratoOperadores = 1
          AND Status = 1

        IF @CustoMensalOperador IS NOT NULL AND @CustoMensalOperador > 0
        BEGIN
            IF @MediaMensalViagens IS NULL OR @MediaMensalViagens = 0
                SET @MediaMensalViagens = dbo.fn_CalculaMediaMensalViagens(@DataInicial)
            
            SET @CustoOperador = (@CustoMensalOperador * @QuantidadeOperador) / @MediaMensalViagens
        END
    END

    -- ========================================================================
    -- Atualiza a viagem com os custos calculados
    -- ========================================================================
    UPDATE Viagem
    SET CustoCombustivel = ROUND(@CustoCombustivel, 2),
        CustoVeiculo = ROUND(@CustoVeiculo, 2),
        CustoMotorista = ROUND(@CustoMotorista, 2),
        CustoLavador = ROUND(@CustoLavador, 2),
        CustoOperador = ROUND(@CustoOperador, 2)
    WHERE ViagemId = @ViagemId

END
GO

PRINT '✓ sp_CalculaCustosViagem criada'
GO

-- ============================================================================
-- 5. STORED PROCEDURE AUXILIAR: Recalcula custos de todas as viagens
-- ============================================================================
IF OBJECT_ID('dbo.sp_RecalculaTodosOsCustos', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RecalculaTodosOsCustos
GO

CREATE PROCEDURE dbo.sp_RecalculaTodosOsCustos
    @DataInicio DATETIME = NULL,
    @DataFim DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @ViagemId UNIQUEIDENTIFIER
    DECLARE @Total INT
    DECLARE @Contador INT = 0

    SELECT @Total = COUNT(*)
    FROM Viagem
    WHERE (@DataInicio IS NULL OR DataInicial >= @DataInicio)
      AND (@DataFim IS NULL OR DataInicial <= @DataFim)

    PRINT 'Iniciando recálculo de ' + CAST(@Total AS VARCHAR) + ' viagens...'

    DECLARE viagem_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT ViagemId
        FROM Viagem
        WHERE (@DataInicio IS NULL OR DataInicial >= @DataInicio)
          AND (@DataFim IS NULL OR DataInicial <= @DataFim)
        ORDER BY DataInicial

    OPEN viagem_cursor
    FETCH NEXT FROM viagem_cursor INTO @ViagemId

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Contador = @Contador + 1
        EXEC dbo.sp_CalculaCustosViagem @ViagemId

        IF @Contador % 500 = 0
            PRINT 'Processadas ' + CAST(@Contador AS VARCHAR) + ' de ' + CAST(@Total AS VARCHAR) + ' viagens...'

        FETCH NEXT FROM viagem_cursor INTO @ViagemId
    END

    CLOSE viagem_cursor
    DEALLOCATE viagem_cursor

    PRINT 'Recálculo concluído! Total processado: ' + CAST(@Contador AS VARCHAR) + ' viagens.'
END
GO

PRINT '✓ sp_RecalculaTodosOsCustos criada'
GO

-- ============================================================================
-- 6. TRIGGER: Dispara o cálculo de custos em INSERT e UPDATE
-- ============================================================================
IF OBJECT_ID('dbo.tr_Viagem_CalculaCustos', 'TR') IS NOT NULL
    DROP TRIGGER dbo.tr_Viagem_CalculaCustos
GO

CREATE TRIGGER dbo.tr_Viagem_CalculaCustos
ON dbo.Viagem
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @ViagemId UNIQUEIDENTIFIER

    DECLARE viagem_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT i.ViagemId
        FROM inserted i
        LEFT JOIN deleted d ON i.ViagemId = d.ViagemId
        WHERE 
            d.ViagemId IS NULL
            OR ISNULL(i.VeiculoId, NEWID()) <> ISNULL(d.VeiculoId, NEWID())
            OR ISNULL(i.MotoristaId, NEWID()) <> ISNULL(d.MotoristaId, NEWID())
            OR ISNULL(i.DataInicial, '1900-01-01') <> ISNULL(d.DataInicial, '1900-01-01')
            OR ISNULL(i.KmInicial, -1) <> ISNULL(d.KmInicial, -1)
            OR ISNULL(i.KmFinal, -1) <> ISNULL(d.KmFinal, -1)
            OR ISNULL(i.Minutos, -1) <> ISNULL(d.Minutos, -1)
            OR ISNULL(i.Status, '') <> ISNULL(d.Status, '')

    OPEN viagem_cursor
    FETCH NEXT FROM viagem_cursor INTO @ViagemId

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.sp_CalculaCustosViagem @ViagemId
        FETCH NEXT FROM viagem_cursor INTO @ViagemId
    END

    CLOSE viagem_cursor
    DEALLOCATE viagem_cursor

END
GO

PRINT '✓ tr_Viagem_CalculaCustos criado'
GO

PRINT ''
PRINT '============================================================================'
PRINT 'INSTALAÇÃO CONCLUÍDA!'
PRINT '============================================================================'
PRINT ''
PRINT 'Objetos criados:'
PRINT '  - fn_CalculaConsumoVeiculo'
PRINT '  - fn_GetValorCombustivelProximo'
PRINT '  - fn_CalculaMediaMensalViagens'
PRINT '  - sp_CalculaCustosViagem'
PRINT '  - sp_RecalculaTodosOsCustos'
PRINT '  - tr_Viagem_CalculaCustos'
PRINT ''
PRINT '============================================================================'
PRINT 'INICIANDO RECÁLCULO DE TODA A BASE...'
PRINT '============================================================================'
GO

-- ============================================================================
-- 7. EXECUTA RECÁLCULO DE TODA A BASE
-- ============================================================================
EXEC dbo.sp_RecalculaTodosOsCustos
GO

PRINT ''
PRINT '============================================================================'
PRINT 'PROCESSO FINALIZADO COM SUCESSO!'
PRINT '============================================================================'
GO
