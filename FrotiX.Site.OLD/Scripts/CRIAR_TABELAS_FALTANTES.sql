/****************************************************************************************
 * 🔧 SCRIPT: Criar Tabelas Faltantes no Banco FrotiX
 * --------------------------------------------------------------------------------------
 * Descrição: Cria as 3 tabelas que estão definidas em Frotix.sql mas ausentes no banco
 *
 * Autor: Claude Sonnet 4.5 (FrotiX Team)
 * Data: 13/02/2026
 *
 * ⚠️ TABELAS QUE SERÃO CRIADAS:
 * 1. Contrato (CRÍTICO - sistema de contratos)
 * 2. AspNetUsers (CRÍTICO - autenticação Identity)
 * 3. AnosDisponiveisAbastecimento (MÉDIO - dashboards)
 *
 * ⚠️ IMPORTANTE:
 * - Execute ESTE script ANTES do SINCRONIZAR_BANCO_COM_MODELOS_V2.sql
 * - Script extraído diretamente do Frotix.sql (linhas 1415, 3588, 11922)
 * - Inclui índices e foreign keys
 ****************************************************************************************/

USE Frotix;
GO

SET NOCOUNT ON;
GO

PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '🔧 CRIANDO TABELAS FALTANTES NO BANCO FROTIX';
PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '⏰ Início: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- VALIDAÇÕES PRELIMINARES
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '🔍 Verificando quais tabelas estão ausentes...';
PRINT '';

DECLARE @FaltaContrato BIT = 0;
DECLARE @FaltaAspNetUsers BIT = 0;
DECLARE @FaltaAnosDisponiveisAbastecimento BIT = 0;

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Contrato' AND TABLE_SCHEMA = 'dbo')
BEGIN
    SET @FaltaContrato = 1;
    PRINT '❌ Tabela ausente: Contrato';
END
ELSE
    PRINT '✅ Tabela existe: Contrato';

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AspNetUsers' AND TABLE_SCHEMA = 'dbo')
BEGIN
    SET @FaltaAspNetUsers = 1;
    PRINT '❌ Tabela ausente: AspNetUsers';
END
ELSE
    PRINT '✅ Tabela existe: AspNetUsers';

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AnosDisponiveisAbastecimento' AND TABLE_SCHEMA = 'dbo')
BEGIN
    SET @FaltaAnosDisponiveisAbastecimento = 1;
    PRINT '❌ Tabela ausente: AnosDisponiveisAbastecimento';
END
ELSE
    PRINT '✅ Tabela existe: AnosDisponiveisAbastecimento';

PRINT '';

IF @FaltaContrato = 0 AND @FaltaAspNetUsers = 0 AND @FaltaAnosDisponiveisAbastecimento = 0
BEGIN
    PRINT '✅ Todas as tabelas já existem! Nada a fazer.';
    PRINT '';
    PRINT '════════════════════════════════════════════════════════════════════════════════';
    RETURN;
END

PRINT '🔧 Iniciando criação das tabelas faltantes...';
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- TABELA 1: Contrato (se ausente)
-- ═══════════════════════════════════════════════════════════════════════════════════

IF @FaltaContrato = 1
BEGIN
    PRINT '────────────────────────────────────────────────────────────────────────────';
    PRINT '📋 Criando tabela: Contrato';
    PRINT '────────────────────────────────────────────────────────────────────────────';
    PRINT '';

    CREATE TABLE dbo.Contrato (
      ContratoId uniqueidentifier NOT NULL DEFAULT (newid()),
      NumeroContrato varchar(10) NULL,
      AnoContrato varchar(10) NULL,
      Vigencia int NULL,
      Prorrogacao int NULL,
      AnoProcesso int NULL,
      NumeroProcesso varchar(50) NULL,
      Objeto varchar(max) NULL,
      Valor float NULL,
      TipoContrato varchar(50) NULL,
      DataInicio date NULL,
      DataFim date NULL,
      ContratoOperadores bit NULL DEFAULT (0),
      ContratoMotoristas bit NULL DEFAULT (0),
      ContratoLavadores bit NULL DEFAULT (0),
      FornecedorId uniqueidentifier NULL,
      Status bit NULL DEFAULT (1),
      UsuarioIdAlteracao nvarchar(100) NULL,
      DataAlteracao datetime NULL,
      CustoMensalOperador float NULL,
      CustoMensalMotorista float NULL,
      CustoMensalLavador float NULL,
      QuantidadeOperador int NULL,
      QuantidadeMotorista int NULL,
      QuantidadeLavador int NULL,
      CustoMensalEncarregado float NULL,
      QuantidadeEncarregado int NULL,
      ContratoEncarregados bit NULL,
      DataRepactuacao date NULL,
      CONSTRAINT PK_Contrato_ContratoId PRIMARY KEY CLUSTERED (ContratoId)
    )
    ON [PRIMARY]
    TEXTIMAGE_ON [PRIMARY];

    PRINT '✅ Tabela criada: Contrato';

    -- Criar índice
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Contrato')
    BEGIN
        CREATE INDEX IX_Contrato_TipoContrato_Flags
          ON dbo.Contrato (TipoContrato, DataInicio DESC)
          INCLUDE (ContratoId, ContratoOperadores, ContratoLavadores)
          WHERE ([TipoContrato]='Terceirização')
          ON [PRIMARY];

        PRINT '✅ Índice criado: IX_Contrato_TipoContrato_Flags';
    END

    -- Criar FK (se Fornecedor existe)
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Fornecedor' AND TABLE_SCHEMA = 'dbo')
    BEGIN
        ALTER TABLE dbo.Contrato
          ADD CONSTRAINT FK_Contrato_Fornecedor FOREIGN KEY (FornecedorId) REFERENCES dbo.Fornecedor (FornecedorId);

        PRINT '✅ Foreign Key criada: FK_Contrato_Fornecedor';
    END
    ELSE
    BEGIN
        PRINT '⚠️  FK não criada: Tabela Fornecedor não encontrada';
    END

    PRINT '';
END

-- ═══════════════════════════════════════════════════════════════════════════════════
-- TABELA 2: AspNetUsers (se ausente)
-- ═══════════════════════════════════════════════════════════════════════════════════

IF @FaltaAspNetUsers = 1
BEGIN
    PRINT '────────────────────────────────────────────────────────────────────────────';
    PRINT '📋 Criando tabela: AspNetUsers';
    PRINT '────────────────────────────────────────────────────────────────────────────';
    PRINT '';

    CREATE TABLE dbo.AspNetUsers (
      Id nvarchar(450) NOT NULL,
      UserName nvarchar(256) NULL,
      NormalizedUserName nvarchar(256) NULL,
      Email nvarchar(256) NULL,
      NormalizedEmail nvarchar(256) NULL,
      EmailConfirmed bit NOT NULL,
      PasswordHash nvarchar(max) NULL,
      SecurityStamp nvarchar(max) NULL,
      ConcurrencyStamp nvarchar(max) NULL,
      PhoneNumber nvarchar(max) NULL,
      PhoneNumberConfirmed bit NOT NULL,
      TwoFactorEnabled bit NOT NULL,
      LockoutEnd datetimeoffset NULL,
      LockoutEnabled bit NOT NULL,
      AccessFailedCount int NOT NULL,
      Discriminator nvarchar(max) NOT NULL,
      NomeCompleto nvarchar(max) NULL,
      SetorId uniqueidentifier NULL,
      FuncaoId uniqueidentifier NULL,
      UnidadeId uniqueidentifier NULL,
      CONSTRAINT PK_AspNetUsers PRIMARY KEY CLUSTERED (Id)
    )
    ON [PRIMARY]
    TEXTIMAGE_ON [PRIMARY];

    PRINT '✅ Tabela criada: AspNetUsers';

    -- Criar índices
    CREATE UNIQUE INDEX UserNameIndex
      ON dbo.AspNetUsers (NormalizedUserName)
      WHERE ([NormalizedUserName] IS NOT NULL)
      ON [PRIMARY];

    PRINT '✅ Índice criado: UserNameIndex';

    CREATE INDEX EmailIndex
      ON dbo.AspNetUsers (NormalizedEmail)
      ON [PRIMARY];

    PRINT '✅ Índice criado: EmailIndex';

    PRINT '';
END

-- ═══════════════════════════════════════════════════════════════════════════════════
-- TABELA 3: AnosDisponiveisAbastecimento (se ausente)
-- ═══════════════════════════════════════════════════════════════════════════════════

IF @FaltaAnosDisponiveisAbastecimento = 1
BEGIN
    PRINT '────────────────────────────────────────────────────────────────────────────';
    PRINT '📋 Criando tabela: AnosDisponiveisAbastecimento';
    PRINT '────────────────────────────────────────────────────────────────────────────';
    PRINT '';

    CREATE TABLE dbo.AnosDisponiveisAbastecimento (
      Ano int NOT NULL,
      TotalAbastecimentos int NULL,
      PrimeiraData datetime2 NULL,
      UltimaData datetime2 NULL,
      DataUltimaAtualizacao datetime2 NULL,
      CONSTRAINT PK_AnosDisponiveisAbastecimento PRIMARY KEY CLUSTERED (Ano)
    )
    ON [PRIMARY];

    PRINT '✅ Tabela criada: AnosDisponiveisAbastecimento';

    -- Criar índice
    CREATE INDEX IX_AnosDisponiveisAbastecimento_UltimaData
      ON dbo.AnosDisponiveisAbastecimento (UltimaData DESC)
      INCLUDE (Ano, TotalAbastecimentos)
      ON [PRIMARY];

    PRINT '✅ Índice criado: IX_AnosDisponiveisAbastecimento_UltimaData';

    PRINT '';
END

-- ═══════════════════════════════════════════════════════════════════════════════════
-- RESUMO FINAL
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '✅ CRIAÇÃO DE TABELAS CONCLUÍDA!';
PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '📊 Resumo:';

IF @FaltaContrato = 1
    PRINT '   ✅ Contrato: Criada com sucesso';
ELSE
    PRINT '   ⚪ Contrato: Já existia (não foi criada)';

IF @FaltaAspNetUsers = 1
    PRINT '   ✅ AspNetUsers: Criada com sucesso';
ELSE
    PRINT '   ⚪ AspNetUsers: Já existia (não foi criada)';

IF @FaltaAnosDisponiveisAbastecimento = 1
    PRINT '   ✅ AnosDisponiveisAbastecimento: Criada com sucesso';
ELSE
    PRINT '   ⚪ AnosDisponiveisAbastecimento: Já existia (não foi criada)';

PRINT '';
PRINT '📋 Próximos passos:';
PRINT '   1. Executar: SINCRONIZAR_BANCO_COM_MODELOS_V2.sql';
PRINT '   2. Corrigir modelos C# conforme ACOES_MODELOS_CSHARP_POS_SINCRONIZACAO.md';
PRINT '   3. Executar: Limpeza_Origem_Destino_CORRIGIDO.sql (opcional)';
PRINT '';
PRINT '⏰ Fim: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';
PRINT '════════════════════════════════════════════════════════════════════════════════';

SET NOCOUNT OFF;
GO
