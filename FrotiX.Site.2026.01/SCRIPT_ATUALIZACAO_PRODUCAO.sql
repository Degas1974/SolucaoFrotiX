/* ======================================================================================
   SCRIPT DE ATUALIZA??O DO BANCO FROTIX - PRODU??O ? DESENVOLVIMENTO
   ======================================================================================
   
   DATA GERA??O: 09/02/2026 08:38:25
   
   IMPORTANTE:
   ? Este script atualiza o banco de PRODU??O com os objetos do banco de DESENVOLVIMENTO
   ? Fa?a BACKUP completo antes de executar
   ? Execute em hor?rio de baixo movimento
   ? Tempo estimado: 5-15 minutos
   ? Em caso de erro, ser? feito ROLLBACK autom?tico
   
   RESUMO DAS ALTERA??ES:
   ? Novas tabelas: 31
   ?? Tabelas modificadas: 5
   ? Novas views: 2
   ?? Views modificadas: 65
   ? Novas procedures: 20
   ?? Procedures modificadas: 1
   ? Procedures removidas: 1
   
   ======================================================================================
*/

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET XACT_ABORT ON; -- Rollback autom?tico em caso de erro
GO

USE Frotix
GO

-- Verificar banco correto
IF DB_NAME() <> 'Frotix'
BEGIN
    RAISERROR('Este script deve ser executado no banco Frotix!', 16, 1);
    RETURN;
END
GO

PRINT '======================================================================================';
PRINT '                   IN?CIO DA ATUALIZA??O DO BANCO FROTIX';
PRINT '======================================================================================';
PRINT 'Servidor: ' + @@SERVERNAME;
PRINT 'Banco: ' + DB_NAME();
PRINT 'Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '======================================================================================';
PRINT '';
GO

-- N?o usar transaction global aqui - cada se??o ter? sua pr?pria
-- ======================================================================================
-- SE??O 1: CRIAR NOVAS TABELAS (31 tabelas)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 1: CRIAR NOVAS TABELAS';
PRINT '======================================================================================';
PRINT '';
GO
-- Criar tabela: AnosDisponiveisAbastecimento
PRINT 'Criando tabela: AnosDisponiveisAbastecimento...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AnosDisponiveisAbastecimento]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.AnosDisponiveisAbastecimento (
  Ano int NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Ano)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela AnosDisponiveisAbastecimento j? existe. Pulando.';
END
GO
-- Criar tabela: AnosDisponiveisVeiculo
PRINT 'Criando tabela: AnosDisponiveisVeiculo...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AnosDisponiveisVeiculo]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.AnosDisponiveisVeiculo (
  Ano int NOT NULL,
  TotalViagens int NOT NULL DEFAULT (0),
  TotalAbastecimentos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Ano)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela AnosDisponiveisVeiculo j? existe. Pulando.';
END
GO
-- Criar tabela: ControleAcesso_BACKUP
PRINT 'Criando tabela: ControleAcesso_BACKUP...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControleAcesso_BACKUP]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.ControleAcesso_BACKUP (
  UsuarioId nvarchar(450) NOT NULL,
  RecursoId uniqueidentifier NOT NULL,
  Acesso bit NOT NULL
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela ControleAcesso_BACKUP j? existe. Pulando.';
END
GO
-- Criar schema: DocGenerator
PRINT 'Criando schema: DocGenerator...';
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'DocGenerator')
BEGIN
    EXEC('CREATE SCHEMA DocGenerator');
    PRINT '  ✓ Schema DocGenerator criado.';
END
ELSE
BEGIN
    PRINT '  ℹ️  Schema DocGenerator já existe.';
END
GO

-- Criar tabela: DocGenerator.DocumentationAlerts
PRINT 'Criando tabela: DocGenerator.DocumentationAlerts...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DocGenerator].[DocumentationAlerts]') AND type IN (N'U'))
BEGIN
CREATE TABLE DocGenerator.DocumentationAlerts (
  Id int IDENTITY,
  FilePath nvarchar(500) NOT NULL,
  AlertType nvarchar(50) NOT NULL,
  AlertMessage nvarchar(500) NOT NULL,
  Priority int NULL DEFAULT (1),
  AssignedToUserId int NULL,
  Status nvarchar(50) NULL DEFAULT ('PENDING'),
  CreatedAt datetime2 NULL DEFAULT (getdate()),
  ResolvedAt datetime2 NULL,
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
    PRINT '  ✓ Tabela DocGenerator.DocumentationAlerts criada.';
END
ELSE
BEGIN
    PRINT '  ℹ️  Tabela DocGenerator.DocumentationAlerts já existe. Pulando.';
END
GO

-- Criar tabela: DocGenerator.FileTracking
PRINT 'Criando tabela: DocGenerator.FileTracking...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DocGenerator].[FileTracking]') AND type IN (N'U'))
BEGIN
CREATE TABLE DocGenerator.FileTracking (
  Id int IDENTITY,
  FilePath nvarchar(500) NOT NULL,
  FileHash nvarchar(64) NOT NULL,
  FileSize int NOT NULL,
  LineCount int NOT NULL,
  CharacterCount int NOT NULL,
  LastModified datetime2 NOT NULL,
  LastDocumented datetime2 NULL,
  DocumentationVersion int NULL DEFAULT (1),
  NeedsUpdate bit NULL DEFAULT (0),
  UpdateReason nvarchar(200) NULL,
  CreatedAt datetime2 NULL DEFAULT (getdate()),
  UpdatedAt datetime2 NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_DocGenerator_FilePath UNIQUE (FilePath)
)
ON [PRIMARY];

CREATE INDEX IX_DocGen_FileTracking_LastModified
  ON DocGenerator.FileTracking (LastModified)
  ON [PRIMARY];

    PRINT '  ✓ Tabela DocGenerator.FileTracking criada.';
END
ELSE
BEGIN
    PRINT '  ℹ️  Tabela DocGenerator.FileTracking já existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaAbastecimentoCategoria
PRINT 'Criando tabela: EstatisticaAbastecimentoCategoria...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaAbastecimentoCategoria]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaAbastecimentoCategoria (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  Categoria nvarchar(100) NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatAbastCat UNIQUE (Ano, Mes, Categoria)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaAbastecimentoCategoria j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaAbastecimentoCombustivel
PRINT 'Criando tabela: EstatisticaAbastecimentoCombustivel...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaAbastecimentoCombustivel]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaAbastecimentoCombustivel (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  TipoCombustivel nvarchar(100) NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NULL DEFAULT (0),
  MediaValorLitro decimal(18, 4) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatAbastComb UNIQUE (Ano, Mes, TipoCombustivel)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaAbastecimentoCombustivel j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaAbastecimentoMensal
PRINT 'Criando tabela: EstatisticaAbastecimentoMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaAbastecimentoMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaAbastecimentoMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatAbastMensal UNIQUE (Ano, Mes)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaAbastecimentoMensal j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaAbastecimentoTipoVeiculo
PRINT 'Criando tabela: EstatisticaAbastecimentoTipoVeiculo...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaAbastecimentoTipoVeiculo]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaAbastecimentoTipoVeiculo (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  TipoVeiculo nvarchar(100) NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatAbastTipo UNIQUE (Ano, Mes, TipoVeiculo)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaAbastecimentoTipoVeiculo j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaAbastecimentoVeiculo
PRINT 'Criando tabela: EstatisticaAbastecimentoVeiculo...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaAbastecimentoVeiculo]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaAbastecimentoVeiculo (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  VeiculoId uniqueidentifier NOT NULL,
  Placa nvarchar(20) NULL,
  TipoVeiculo nvarchar(100) NULL,
  Categoria nvarchar(100) NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatAbastVeiculo UNIQUE (Ano, VeiculoId)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaAbastecimentoVeiculo j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaAbastecimentoVeiculoMensal
PRINT 'Criando tabela: EstatisticaAbastecimentoVeiculoMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaAbastecimentoVeiculoMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaAbastecimentoVeiculoMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  VeiculoId uniqueidentifier NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatAbastVeiculoMes UNIQUE (Ano, Mes, VeiculoId)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaAbastecimentoVeiculoMensal j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaGeralMensal
PRINT 'Criando tabela: EstatisticaGeralMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaGeralMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaGeralMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  TotalMotoristas int NULL DEFAULT (0),
  MotoristasAtivos int NULL DEFAULT (0),
  MotoristasInativos int NULL DEFAULT (0),
  Efetivos int NULL DEFAULT (0),
  Feristas int NULL DEFAULT (0),
  Cobertura int NULL DEFAULT (0),
  TotalViagens int NULL DEFAULT (0),
  KmTotal decimal(18, 2) NULL DEFAULT (0),
  HorasTotais decimal(18, 2) NULL DEFAULT (0),
  TotalMultas int NULL DEFAULT (0),
  ValorTotalMultas decimal(18, 2) NULL DEFAULT (0),
  TotalAbastecimentos int NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatGeralMensal UNIQUE (Ano, Mes)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaGeralMensal j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaMotoristasMensal
PRINT 'Criando tabela: EstatisticaMotoristasMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaMotoristasMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaMotoristasMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  MotoristaId uniqueidentifier NOT NULL,
  Ano int NOT NULL,
  Mes int NOT NULL,
  TotalViagens int NULL DEFAULT (0),
  KmTotal decimal(18, 2) NULL DEFAULT (0),
  MinutosTotais int NULL DEFAULT (0),
  TotalMultas int NULL DEFAULT (0),
  ValorTotalMultas decimal(18, 2) NULL DEFAULT (0),
  TotalAbastecimentos int NULL DEFAULT (0),
  LitrosTotais decimal(18, 2) NULL DEFAULT (0),
  ValorTotalAbastecimentos decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatMotMensal UNIQUE (MotoristaId, Ano, Mes)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaMotoristasMensal j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoAnoFabricacao
PRINT 'Criando tabela: EstatisticaVeiculoAnoFabricacao...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoAnoFabricacao]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoAnoFabricacao (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  AnoFabricacao int NOT NULL,
  TotalVeiculos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicAnoFab UNIQUE (AnoFabricacao)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoAnoFabricacao j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoCategoria
PRINT 'Criando tabela: EstatisticaVeiculoCategoria...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoCategoria]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoCategoria (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Categoria nvarchar(100) NOT NULL,
  TotalVeiculos int NOT NULL DEFAULT (0),
  VeiculosAtivos int NOT NULL DEFAULT (0),
  VeiculosProprios int NOT NULL DEFAULT (0),
  VeiculosLocados int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicCategoria UNIQUE (Categoria)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoCategoria j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoCombustivel
PRINT 'Criando tabela: EstatisticaVeiculoCombustivel...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoCombustivel]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoCombustivel (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Combustivel nvarchar(100) NOT NULL,
  TotalVeiculos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicCombustivel UNIQUE (Combustivel)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoCombustivel j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoGeral
PRINT 'Criando tabela: EstatisticaVeiculoGeral...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoGeral]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoGeral (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  TotalVeiculos int NOT NULL DEFAULT (0),
  VeiculosAtivos int NOT NULL DEFAULT (0),
  VeiculosInativos int NOT NULL DEFAULT (0),
  VeiculosProprios int NOT NULL DEFAULT (0),
  VeiculosLocados int NOT NULL DEFAULT (0),
  IdadeMediaAnos decimal(10, 2) NOT NULL DEFAULT (0),
  KmMedioRodado decimal(18, 2) NOT NULL DEFAULT (0),
  ValorMensalLocacao decimal(18, 2) NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoGeral j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoModelo
PRINT 'Criando tabela: EstatisticaVeiculoModelo...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoModelo]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoModelo (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  ModeloId uniqueidentifier NULL,
  Modelo nvarchar(100) NOT NULL,
  TotalVeiculos int NOT NULL DEFAULT (0),
  VeiculosAtivos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicModelo UNIQUE (Modelo)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoModelo j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoRankingConsumo
PRINT 'Criando tabela: EstatisticaVeiculoRankingConsumo...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoRankingConsumo]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoRankingConsumo (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  VeiculoId uniqueidentifier NOT NULL,
  Placa nvarchar(20) NULL,
  Modelo nvarchar(100) NULL,
  KmRodado decimal(18, 2) NOT NULL DEFAULT (0),
  LitrosAbastecidos decimal(18, 2) NOT NULL DEFAULT (0),
  ConsumoKmPorLitro decimal(10, 2) NOT NULL DEFAULT (0),
  TotalAbastecimentos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicRankConsumo UNIQUE (Ano, VeiculoId)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoRankingConsumo j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoRankingKm
PRINT 'Criando tabela: EstatisticaVeiculoRankingKm...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoRankingKm]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoRankingKm (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  VeiculoId uniqueidentifier NOT NULL,
  Placa nvarchar(20) NULL,
  Modelo nvarchar(100) NULL,
  KmRodado decimal(18, 2) NOT NULL DEFAULT (0),
  TotalViagens int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicRankKm UNIQUE (Ano, VeiculoId)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoRankingKm j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoRankingLitros
PRINT 'Criando tabela: EstatisticaVeiculoRankingLitros...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoRankingLitros]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoRankingLitros (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  VeiculoId uniqueidentifier NOT NULL,
  Placa nvarchar(20) NULL,
  Modelo nvarchar(100) NULL,
  LitrosAbastecidos decimal(18, 2) NOT NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NOT NULL DEFAULT (0),
  TotalAbastecimentos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicRankLitros UNIQUE (Ano, VeiculoId)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoRankingLitros j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoStatus
PRINT 'Criando tabela: EstatisticaVeiculoStatus...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoStatus]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoStatus (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Status nvarchar(50) NOT NULL,
  TotalVeiculos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicStatus UNIQUE (Status)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoStatus j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoUnidade
PRINT 'Criando tabela: EstatisticaVeiculoUnidade...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoUnidade]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoUnidade (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  UnidadeId uniqueidentifier NULL,
  Unidade nvarchar(200) NOT NULL,
  TotalVeiculos int NOT NULL DEFAULT (0),
  VeiculosAtivos int NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicUnidade UNIQUE (Unidade)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoUnidade j? existe. Pulando.';
END
GO
-- Criar tabela: EstatisticaVeiculoUsoMensal
PRINT 'Criando tabela: EstatisticaVeiculoUsoMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstatisticaVeiculoUsoMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EstatisticaVeiculoUsoMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  TotalViagens int NOT NULL DEFAULT (0),
  KmTotalRodado decimal(18, 2) NOT NULL DEFAULT (0),
  TotalAbastecimentos int NOT NULL DEFAULT (0),
  LitrosTotal decimal(18, 2) NOT NULL DEFAULT (0),
  ValorAbastecimento decimal(18, 2) NOT NULL DEFAULT (0),
  ConsumoMedio decimal(10, 2) NOT NULL DEFAULT (0),
  DataAtualizacao datetime NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_EstatVeicUsoMensal UNIQUE (Ano, Mes)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EstatisticaVeiculoUsoMensal j? existe. Pulando.';
END
GO
-- Criar tabela: EvolucaoViagensDiaria
PRINT 'Criando tabela: EvolucaoViagensDiaria...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EvolucaoViagensDiaria]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.EvolucaoViagensDiaria (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Data date NOT NULL,
  MotoristaId uniqueidentifier NULL,
  TotalViagens int NULL DEFAULT (0),
  KmTotal decimal(18, 2) NULL DEFAULT (0),
  MinutosTotais int NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela EvolucaoViagensDiaria j? existe. Pulando.';
END
GO
-- Criar tabela: HeatmapAbastecimentoMensal
PRINT 'Criando tabela: HeatmapAbastecimentoMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HeatmapAbastecimentoMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.HeatmapAbastecimentoMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  VeiculoId uniqueidentifier NULL,
  TipoVeiculo nvarchar(100) NULL,
  DiaSemana int NOT NULL,
  Hora int NOT NULL,
  TotalAbastecimentos int NULL DEFAULT (0),
  ValorTotal decimal(18, 2) NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela HeatmapAbastecimentoMensal j? existe. Pulando.';
END
GO
-- Criar tabela: HeatmapViagensMensal
PRINT 'Criando tabela: HeatmapViagensMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HeatmapViagensMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.HeatmapViagensMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  MotoristaId uniqueidentifier NULL,
  DiaSemana int NOT NULL,
  Hora int NOT NULL,
  TotalViagens int NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela HeatmapViagensMensal j? existe. Pulando.';
END
GO
-- Criar tabela: LogErros
PRINT 'Criando tabela: LogErros...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogErros]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.LogErros (
  LogErroId bigint IDENTITY,
  DataHora datetime2(3) NOT NULL DEFAULT (getdate()),
  Tipo nvarchar(50) NOT NULL,
  Origem nvarchar(20) NOT NULL,
  Nivel nvarchar(20) NULL,
  Categoria nvarchar(100) NULL,
  Mensagem nvarchar(max) NOT NULL,
  MensagemCurta AS (case when len([Mensagem])>(200) then left([Mensagem],(200))+'...' else [Mensagem] end) PERSISTED,
  Arquivo nvarchar(500) NULL,
  Metodo nvarchar(200) NULL,
  Linha int NULL,
  Coluna int NULL,
  ExceptionType nvarchar(200) NULL,
  ExceptionMessage nvarchar(max) NULL,
  StackTrace nvarchar(max) NULL,
  InnerException nvarchar(max) NULL,
  Url nvarchar(1000) NULL,
  HttpMethod nvarchar(10) NULL,
  StatusCode int NULL,
  UserAgent nvarchar(500) NULL,
  IpAddress nvarchar(45) NULL,
  Usuario nvarchar(100) NULL,
  SessionId nvarchar(100) NULL,
  DadosAdicionais nvarchar(max) NULL,
  Resolvido bit NOT NULL DEFAULT (0),
  DataResolucao datetime2(3) NULL,
  ResolvidoPor nvarchar(100) NULL,
  Observacoes nvarchar(max) NULL,
  HashErro AS (CONVERT([nvarchar](64),hashbytes('SHA2_256',concat(isnull([Tipo],''),'|',isnull([Arquivo],''),'|',isnull(CONVERT([nvarchar](10),[Linha]),'0'),'|',left(isnull([Mensagem],''),(200)))),(2))) PERSISTED,
  CriadoEm datetime2(3) NOT NULL DEFAULT (getdate()),
  CONSTRAINT PK_LogErros PRIMARY KEY CLUSTERED (LogErroId DESC)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela LogErros j? existe. Pulando.';
END
GO
-- Criar tabela: RankingMotoristasMensal
PRINT 'Criando tabela: RankingMotoristasMensal...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RankingMotoristasMensal]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.RankingMotoristasMensal (
  Id uniqueidentifier NOT NULL DEFAULT (newid()),
  Ano int NOT NULL,
  Mes int NOT NULL,
  TipoRanking varchar(50) NOT NULL,
  Posicao int NOT NULL,
  MotoristaId uniqueidentifier NOT NULL,
  NomeMotorista nvarchar(200) NULL,
  TipoMotorista nvarchar(50) NULL,
  ValorPrincipal decimal(18, 2) NULL DEFAULT (0),
  ValorSecundario decimal(18, 2) NULL DEFAULT (0),
  ValorTerciario decimal(18, 2) NULL DEFAULT (0),
  ValorQuaternario int NULL DEFAULT (0),
  DataAtualizacao datetime NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT UQ_RankingMot UNIQUE (Ano, Mes, TipoRanking, Posicao)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela RankingMotoristasMensal j? existe. Pulando.';
END
GO
-- Criar tabela: Recurso_BACKUP
PRINT 'Criando tabela: Recurso_BACKUP...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Recurso_BACKUP]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.Recurso_BACKUP (
  RecursoId uniqueidentifier NOT NULL,
  Nome nvarchar(200) NOT NULL,
  Descricao varchar(250) NULL,
  NomeMenu nvarchar(200) NOT NULL,
  Ordem float NOT NULL,
  ParentId uniqueidentifier NULL,
  Icon nvarchar(200) NOT NULL,
  Href nvarchar(500) NOT NULL,
  Ativo bit NOT NULL,
  Nivel int NOT NULL,
  HasChild bit NOT NULL
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela Recurso_BACKUP j? existe. Pulando.';
END
GO
-- Criar tabela: RepactuacaoVeiculo
PRINT 'Criando tabela: RepactuacaoVeiculo...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RepactuacaoVeiculo]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.RepactuacaoVeiculo (
  RepactuacaoVeiculoId uniqueidentifier NOT NULL DEFAULT (newid()),
  RepactuacaoContratoId uniqueidentifier NOT NULL,
  VeiculoId uniqueidentifier NOT NULL,
  Valor float NULL,
  Observacao nvarchar(500) NULL,
  CONSTRAINT PK_RepactuacaoVeiculo PRIMARY KEY CLUSTERED (RepactuacaoVeiculoId)
)
ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela RepactuacaoVeiculo j? existe. Pulando.';
END
GO
-- Criar tabela: SchemaChangeLog
PRINT 'Criando tabela: SchemaChangeLog...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SchemaChangeLog]') AND type IN (N'U'))
BEGIN
CREATE TABLE dbo.SchemaChangeLog (
  Id bigint IDENTITY,
  RunId uniqueidentifier NULL,
  LoggedAt datetime2(3) NOT NULL DEFAULT (sysdatetime()),
  LoginName sysname NULL,
  UserName sysname NULL,
  EventType nvarchar(128) NULL,
  SchemaName sysname NULL,
  ObjectName sysname NULL,
  ObjectType nvarchar(128) NULL,
  TSqlCommand nvarchar(max) NULL,
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
END
ELSE
BEGIN
    PRINT '  ??  Tabela SchemaChangeLog j? existe. Pulando.';
END
GO
-- ======================================================================================
-- SE??O 2: ALTERAR TABELAS MODIFICADAS (5 tabelas)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 2: ALTERAR TABELAS MODIFICADAS';
PRINT '======================================================================================';
PRINT '??  ATEN??O: Altera??es de tabelas devem ser revisadas manualmente!';
PRINT '??  Este script n?o aplicar? altera??es autom?ticas em tabelas existentes.';
PRINT '??  Tabelas modificadas: Viagem, Manutencao, Recurso, Lavagem, Fornecedor';
PRINT '';
GO
-- ======================================================================================
-- SE??O 3: CRIAR/ATUALIZAR VIEWS (67 views)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 3: CRIAR/ATUALIZAR VIEWS';
PRINT '======================================================================================';
PRINT '';
GO
-- View: DocGenerator
PRINT 'Criando/atualizando view: DocGenerator...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'DocGenerator')
    DROP VIEW [dbo].[DocGenerator]
GO

CREATE OR ALTER VIEW DocGenerator.vw_FilesNeedingUpdate AS
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
-- View: vw_RankingMotoristasPorPeriodo
PRINT 'Criando/atualizando view: vw_RankingMotoristasPorPeriodo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_RankingMotoristasPorPeriodo')
    DROP VIEW [dbo].[vw_RankingMotoristasPorPeriodo]
GO

CREATE OR ALTER VIEW dbo.vw_RankingMotoristasPorPeriodo
AS
WITH ViagensPorMotorista AS (
    SELECT
        YEAR(v.DataInicial) AS Ano,
        MONTH(v.DataInicial) AS Mes,
        v.MotoristaId,
        m.Nome AS NomeMotorista,
        COUNT(*) AS TotalViagens,
        SUM(
            CASE
                WHEN v.KmInicial IS NOT NULL
                     AND v.KmFinal IS NOT NULL
                     AND v.KmFinal >= v.KmInicial
                     AND (v.KmFinal - v.KmInicial) <= 2000
                THEN v.KmFinal - v.KmInicial
                ELSE 0
            END
        ) AS KmTotal
    FROM Viagem v
    INNER JOIN Motorista m ON v.MotoristaId = m.MotoristaId
    WHERE v.MotoristaId IS NOT NULL
      AND v.DataInicial IS NOT NULL
    GROUP BY YEAR(v.DataInicial), MONTH(v.DataInicial), v.MotoristaId, m.Nome
)
SELECT
    Ano,
    Mes,
    MotoristaId,
    NomeMotorista,
    TotalViagens,
    KmTotal,
    ROW_NUMBER() OVER (PARTITION BY Ano, Mes ORDER BY TotalViagens DESC, KmTotal DESC) AS PosicaoViagens,
    ROW_NUMBER() OVER (PARTITION BY Ano, Mes ORDER BY KmTotal DESC, TotalViagens DESC) AS PosicaoKm
FROM ViagensPorMotorista;
-- Create or alter view [dbo].[ViewViagensAgendaTodosMeses]
--;
GO
-- View: ViewMotoristasMulta
PRINT 'Criando/atualizando view: ViewMotoristasMulta...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristasMulta')
    DROP VIEW [dbo].[ViewMotoristasMulta]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristasMulta 
AS SELECT TOP 1000000
  Motorista.Nome
 ,SUM(Multa.ValorPago) AS 'Valor Pago'
 ,SUM(Multa.ValorPosVencimento) AS 'Valor a Pagar'
FROM dbo.Multa
INNER JOIN dbo.Motorista
  ON Multa.MotoristaId = Motorista.MotoristaId
GROUP BY Motorista.Nome
ORDER BY SUM(Multa.ValorPosVencimento) DESC
-- Create or alter view [dbo].[ViewEmpenhoMulta]
--;
GO
-- View: ViewPatrimonioConferencia
PRINT 'Criando/atualizando view: ViewPatrimonioConferencia...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewPatrimonioConferencia')
    DROP VIEW [dbo].[ViewPatrimonioConferencia]
GO

CREATE OR ALTER VIEW dbo.ViewPatrimonioConferencia 
AS SELECT
  Patrimonio.PatrimonioId
 ,Patrimonio.NPR
 ,Patrimonio.Descricao
 ,Patrimonio.LocalizacaoAtual
 ,Patrimonio.Marca
 ,Patrimonio.Modelo
 ,SetorPatrimonial.NomeSetor
 ,SecaoPatrimonial.NomeSecao
 ,Patrimonio.Status
 ,Patrimonio.StatusConferencia
 ,Patrimonio.LocalizacaoConferencia
 ,Patrimonio.SetorConferenciaId
 ,Patrimonio.SecaoConferenciaId
 ,Patrimonio.Situacao
FROM dbo.Patrimonio
INNER JOIN dbo.SetorPatrimonial
  ON Patrimonio.SetorId = SetorPatrimonial.SetorId
INNER JOIN dbo.SecaoPatrimonial
  ON Patrimonio.SecaoId = SecaoPatrimonial.SecaoId
    AND SecaoPatrimonial.SetorId = SetorPatrimonial.SetorId
-- Create table [dbo].[MovimentacaoPatrimonio]
--;
GO
-- View: ViewNoFichaVistoria
PRINT 'Criando/atualizando view: ViewNoFichaVistoria...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewNoFichaVistoria')
    DROP VIEW [dbo].[ViewNoFichaVistoria]
GO

CREATE OR ALTER VIEW dbo.ViewNoFichaVistoria 
AS SELECT NoFichaVistoria FROM Viagem
-- Create or alter view [dbo].[ViewFichaViagem]
--;
GO
-- View: ViewCustosFinalidade
PRINT 'Criando/atualizando view: ViewCustosFinalidade...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustosFinalidade')
    DROP VIEW [dbo].[ViewCustosFinalidade]
GO

CREATE OR ALTER VIEW dbo.ViewCustosFinalidade 
AS SELECT
  Viagem.Finalidade
 ,FORMAT(SUM(Viagem.CustoCombustivel), 'N2', 'pt-BR') AS Combustivel
 ,FORMAT(SUM(Viagem.CustoMotorista), 'N2', 'pt-BR') AS Motorista
 ,FORMAT(SUM(Viagem.CustoVeiculo), 'N2', 'pt-BR') AS Veiculo
 ,FORMAT(SUM(Viagem.KmFinal - Viagem.KmInicial), 'N2', 'pt-BR') AS Quilometragem
FROM dbo.Viagem
WHERE Viagem.Status = 'Realizada'
AND Viagem.Finalidade NOT LIKE 'Manutenção'
AND Viagem.Finalidade NOT LIKE 'Devolução à Locadora'
AND MONTH(Viagem.DataInicial) = 12
GROUP BY Viagem.Finalidade
-- Create table [dbo].[OcorrenciaViagem]
--;
GO
-- View: ViewAlocacaoMotorista
PRINT 'Criando/atualizando view: ViewAlocacaoMotorista...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAlocacaoMotorista')
    DROP VIEW [dbo].[ViewAlocacaoMotorista]
GO

CREATE OR ALTER VIEW dbo.ViewAlocacaoMotorista 
AS SELECT DISTINCT
  COUNT(Motorista.MotoristaId) AS Motoristas
 ,Unidade.QtdMotoristas
 ,(COUNT(Motorista.MotoristaId) - Unidade.QtdMotoristas) AS Deficit
 ,Unidade.Descricao
FROM dbo.Motorista
INNER JOIN dbo.Unidade
  ON Motorista.UnidadeId = Unidade.UnidadeId
WHERE Motorista.Status = 1 AND Motorista.TipoCondutor = 'Terceirizado'
GROUP BY Unidade.QtdMotoristas
        ,Unidade.Descricao
-- Create table [dbo].[ViagensEconomildo]
--;
GO
-- View: ViewFluxoEconomildo
PRINT 'Criando/atualizando view: ViewFluxoEconomildo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewFluxoEconomildo')
    DROP VIEW [dbo].[ViewFluxoEconomildo]
GO

CREATE OR ALTER VIEW dbo.ViewFluxoEconomildo 
AS SELECT TOP 10000000
  ViagensEconomildo.Data
 ,Motorista.Nome AS NomeMotorista
 ,Motorista.TipoCondutor
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,ViagensEconomildo.VIagemEconomildoId
 ,ViagensEconomildo.VeiculoId
 ,ViagensEconomildo.MotoristaId
 ,ViagensEconomildo.MOB
 ,ViagensEconomildo.Responsavel
 ,ViagensEconomildo.IdaVolta
 ,ViagensEconomildo.HoraInicio
 ,ViagensEconomildo.HoraFim
 ,ViagensEconomildo.QtdPassageiros
FROM dbo.Veiculo
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
INNER JOIN dbo.ViagensEconomildo
  ON ViagensEconomildo.VeiculoId = Veiculo.VeiculoId
INNER JOIN dbo.Motorista
  ON ViagensEconomildo.MotoristaId = Motorista.MotoristaId
ORDER BY Data DESC, MOB, HoraInicio DESC
-- Create or alter view [dbo].[ViewMotoristaFluxo]
--;
GO
-- View: ViewEmpenhos
PRINT 'Criando/atualizando view: ViewEmpenhos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewEmpenhos')
    DROP VIEW [dbo].[ViewEmpenhos]
GO

CREATE OR ALTER VIEW dbo.ViewEmpenhos 
AS -- =============================================
-- ViewEmpenhos - Versão sem NULLs
-- Todos os campos retornam valores padrão em vez de NULL
-- =============================================

SELECT
    -- GUID sempre tem valor (é PK, nunca é NULL)
    Empenho.EmpenhoId,
    
    -- Strings: ISNULL retorna string vazia
    ISNULL(Empenho.NotaEmpenho, '') AS NotaEmpenho,
    
    -- Datas: ISNULL retorna data padrão (ou pode manter NULL se preferir)
    Empenho.DataEmissao,
    Empenho.VigenciaInicial,
    Empenho.VigenciaFinal,
    
    -- Inteiros: ISNULL retorna 0
    ISNULL(Empenho.AnoVigencia, 0) AS AnoVigencia,
    
    -- Floats: ISNULL retorna 0
    ISNULL(Empenho.SaldoInicial, 0) AS SaldoInicial,
    ISNULL(Empenho.SaldoFinal, 0) AS SaldoFinal,
    
    -- GUIDs que podem ser NULL: ISNULL retorna GUID vazio
    ISNULL(Empenho.ContratoId, '00000000-0000-0000-0000-000000000000') AS ContratoId,
    ISNULL(Empenho.AtaId, '00000000-0000-0000-0000-000000000000') AS AtaId,
    
    -- Agregações: ISNULL para garantir 0 quando não há registros relacionados
    ISNULL(COUNT(DISTINCT MovimentacaoEmpenho.MovimentacaoId), 0) AS Movimentacoes,
    ISNULL(SUM(MovimentacaoEmpenho.Valor), 0) AS SaldoMovimentacao,
    ISNULL(SUM(NotaFiscal.ValorNF - ISNULL(NotaFiscal.ValorGlosa, 0)), 0) AS SaldoNotas

FROM dbo.Empenho

LEFT OUTER JOIN dbo.MovimentacaoEmpenho
    ON Empenho.EmpenhoId = MovimentacaoEmpenho.EmpenhoId

LEFT OUTER JOIN dbo.NotaFiscal
    ON Empenho.EmpenhoId = NotaFiscal.EmpenhoId

GROUP BY 
    Empenho.EmpenhoId, 
    Empenho.NotaEmpenho, 
    Empenho.DataEmissao, 
    Empenho.AnoVigencia, 
    Empenho.VigenciaInicial, 
    Empenho.VigenciaFinal, 
    Empenho.SaldoInicial, 
    Empenho.SaldoFinal, 
    Empenho.ContratoId, 
    Empenho.AtaId
-- Create table [dbo].[EstatisticaVeiculoUnidade]
--;
GO
-- View: ViewControleAcesso
PRINT 'Criando/atualizando view: ViewControleAcesso...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewControleAcesso')
    DROP VIEW [dbo].[ViewControleAcesso]
GO

CREATE OR ALTER VIEW dbo.ViewControleAcesso 
AS SELECT TOP 100000000
  ControleAcesso.UsuarioId
 ,Recurso.RecursoId
 ,ControleAcesso.Acesso
 ,Recurso.Nome
 ,Recurso.Descricao
 ,Recurso.Ordem
 ,AspNetUsers.NomeCompleto
 ,(ControleAcesso.UsuarioId + '|' + CONVERT(NVARCHAR(MAX), ControleAcesso.RecursoId)) AS IDS
FROM dbo.ControleAcesso
INNER JOIN dbo.AspNetUsers
  ON ControleAcesso.UsuarioId = AspNetUsers.Id
INNER JOIN dbo.Recurso
  ON ControleAcesso.RecursoId = Recurso.RecursoId
ORDER BY Recurso.Ordem
-- Create table [dbo].[AspNetUserRoles]
--;
GO
-- View: ViewManutencao_Ultima
PRINT 'Criando/atualizando view: ViewManutencao_Ultima...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewManutencao_Ultima')
    DROP VIEW [dbo].[ViewManutencao_Ultima]
GO

CREATE OR ALTER VIEW dbo.ViewManutencao_Ultima 
AS SELECT 
    -- Campo inicial solicitado
    ISNULL(v.Placa,'') + ' - ' + ISNULL(ma.DescricaoMarca,'') + '/' + ISNULL(m.DescricaoModelo,'') AS PlacaDescricao,

    -- ===== Chaves e referência ao contrato (para filtrar na LINQ) =====
    vc.ContratoId,

    -- ===== Manutenção =====
    b.ManutencaoId,
    b.NumOS,
    b.ResumoOS,

    -- strings formatadas (dd/MM/yyyy)
    CONVERT(CHAR(10), b.DataSolicitacao, 103)                                       AS DataSolicitacao,
    CONVERT(CHAR(10), b.DataDisponibilidade, 103)                                   AS DataDisponibilidade,
    CONVERT(CHAR(10), b.DataRecolhimento, 103)                                      AS DataRecolhimento,
    CONVERT(CHAR(10), b.DataRecebimentoReserva, 103)                                AS DataRecebimentoReserva,
    CONVERT(CHAR(10), b.DataDevolucaoReserva, 103)                                  AS DataDevolucaoReserva,
    CONVERT(CHAR(10), b.DataEntrega, 103)                                           AS DataEntrega,
    CONVERT(CHAR(10), b.DataDevolucao, 103)                                         AS DataDevolucao,

    -- datas cruas
    b.DataSolicitacao                                                               AS DataSolicitacaoRaw,
    b.DataDevolucao                                                                 AS DataDevolucaoRaw,

    b.StatusOS,
    b.VeiculoId,

    -- Derivado (Marca/Modelo) e informações auxiliares
    ma.DescricaoMarca + '/' + m.DescricaoModelo                                     AS DescricaoVeiculo,
    u.Sigla                                                                          AS Sigla,
    c.Descricao                                                                      AS CombustivelDescricao,
    v.Placa                                                                          AS Placa,

    -- Reserva como texto
    CASE
        WHEN b.ReservaEnviado = 1 THEN 'Enviado'
        WHEN b.ReservaEnviado = 0 THEN 'Ausente'
        ELSE ''
    END                                                                              AS Reserva,

    -- ===== Planilha de Glosa (Item do contrato, via Veiculo.ItemVeiculoId) =====
    ivc.Descricao,
    ivc.Quantidade,
    ivc.ValorUnitario,

    -- DiasGlosa (via APPLY)
    d.DiasCalc                                                                       AS DiasGlosa,

    -- ValorGlosa (trata NULLs e usa DECIMAL p/ precisão)
    CAST(
        d.DiasCalc
        * COALESCE(CAST(ivc.ValorUnitario AS DECIMAL(18,2)), 0)
        AS DECIMAL(18,2)
    )                                                                                AS ValorGlosa,

    -- Dias (0 quando sem devolução, senão diff Solicitação->Devolução)
    CASE
        WHEN b.DataDevolucao IS NULL THEN 0
        ELSE DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END                                                                              AS Dias,

    -- Atributos de UI
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS Habilitado,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'fa-regular fa-lock' ELSE 'far fa-flag-checkered' END                 AS Icon,

    -- Item do contrato
    ivc.NumItem,

    -- Campos "montados"
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'disabled' ELSE '' END                                              AS HabilitadoEditar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END           AS OpacityEditar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'Visualizar Manutenção' ELSE 'Edita a Ordem de Serviço!' END        AS OpacityTooltipEditarEditar,

    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'disabled' ELSE '' END                                              AS HabilitadoBaixar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS ModalBaixarAttrs,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END           AS OpacityBaixar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'Desabilitado' ELSE 'Fecha a Ordem de Serviço!' END                 AS Tooltip,

    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'disabled' ELSE '' END                                              AS HabilitadoCancelar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END           AS OpacityCancelar,
    CASE
        WHEN b.StatusOS = 'Cancelada' THEN 'Manutenção Cancelada'
        WHEN b.StatusOS = 'Fechada'   THEN 'OS Fechada/Baixada'
        ELSE 'Cancelar Manutenção'
    END                                                                                                                      AS TooltipCancelar

FROM dbo.Manutencao               AS b
LEFT JOIN dbo.Veiculo             AS v   ON v.VeiculoId       = b.VeiculoId
LEFT JOIN dbo.VeiculoContrato     AS vc  ON vc.VeiculoId      = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo       AS m   ON m.ModeloId        = v.ModeloId
LEFT JOIN dbo.MarcaVeiculo        AS ma  ON ma.MarcaId        = v.MarcaId
LEFT JOIN dbo.Combustivel         AS c   ON c.CombustivelId   = v.CombustivelId
LEFT JOIN dbo.Unidade             AS u   ON u.UnidadeId       = v.UnidadeId
LEFT JOIN dbo.ItemVeiculoContrato AS ivc ON ivc.ItemVeiculoId = v.ItemVeiculoId
CROSS APPLY (
  SELECT DiasCalc =
    CASE
      WHEN b.DataDevolucao IS NULL THEN 1
      WHEN b.DataEntrega  IS NOT NULL THEN DATEDIFF(DAY, b.DataEntrega,  b.DataDevolucao)
      ELSE                               DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END
) AS d

--WHERE
--    v.Status = 1
--    AND b.DataDevolucao IS NOT NULL
--    AND DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao) > 0
-- Create or alter view [dbo].[ViewManutencao_2025.11.04]
--;
GO
-- View: ViewVeiculos_Original
PRINT 'Criando/atualizando view: ViewVeiculos_Original...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewVeiculos_Original')
    DROP VIEW [dbo].[ViewVeiculos_Original]
GO

CREATE OR ALTER VIEW dbo.ViewVeiculos_Original 
AS SELECT
  Veiculo.VeiculoId
 ,Veiculo.Placa
 ,LEFT(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'), LEN(SUBSTRING(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'), CHARINDEX(' ', REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'), CHARINDEX(' ', REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'))), LEN(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.')))) - 2) AS Quilometragem
 ,MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS MarcaModelo
 ,Unidade.Sigla
 ,Combustivel.Descricao
 ,CASE
    WHEN CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2)) IS NULL THEN 0
    ELSE CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2))
  END AS Consumo
 ,CASE
    WHEN ViewContratoFornecedor_Veiculos.ContratoVeiculo IS NOT NULL THEN ViewContratoFornecedor_Veiculos.ContratoVeiculo
    WHEN ViewAtaFornecedor.AtaVeiculo IS NOT NULL THEN ViewAtaFornecedor.AtaVeiculo + ' <b>(Ata)</b> '
    ELSE '<b>(Veículo Próprio)</b>'
  END AS OrigemVeiculo
 ,CONVERT(VARCHAR, Veiculo.DataAlteracao, 103) AS DataAlteracao
 ,AspNetUsers.NomeCompleto
 ,CASE
    WHEN Veiculo.Reserva = 0 THEN 'Efetivo'
    ELSE 'Reserva'
  END AS VeiculoReserva
 ,Veiculo.Status
 ,Veiculo.CombustivelId
 ,ROW_NUMBER() OVER (ORDER BY Veiculo.Placa) AS RowNum
 ,ViewContratoFornecedor_Veiculos.ContratoId
 ,ViewAtaFornecedor.AtaId
 ,ViewContratoFornecedor_Veiculos.ContratoVeiculo
 ,ViewAtaFornecedor.AtaVeiculo
 ,Veiculo.ValorMensal
FROM dbo.Abastecimento
RIGHT OUTER JOIN dbo.Veiculo
  ON Abastecimento.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
LEFT OUTER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
LEFT OUTER JOIN dbo.Unidade
  ON Unidade.UnidadeId = Veiculo.UnidadeId
LEFT OUTER JOIN dbo.Combustivel
  ON Combustivel.CombustivelId = Veiculo.CombustivelId
LEFT OUTER JOIN dbo.ViewContratoFornecedor_Veiculos
  ON ViewContratoFornecedor_Veiculos.ContratoId = Veiculo.ContratoId
LEFT OUTER JOIN dbo.ViewAtaFornecedor
  ON ViewAtaFornecedor.AtaId = Veiculo.AtaId
INNER JOIN dbo.AspNetUsers
  ON AspNetUsers.Id = Veiculo.UsuarioIdAlteracao
GROUP BY Veiculo.VeiculoId
        ,Veiculo.Placa
        ,Veiculo.Quilometragem
        ,MarcaVeiculo.DescricaoMarca
        ,ModeloVeiculo.DescricaoModelo
        ,Unidade.Sigla
        ,Combustivel.Descricao
        ,ViewContratoFornecedor_Veiculos.ContratoId
        ,Veiculo.DataAlteracao
        ,AspNetUsers.NomeCompleto
        ,Veiculo.Reserva
        ,Veiculo.Status
        ,Veiculo.CombustivelId
        ,ViewAtaFornecedor.AtaId
        ,ViewContratoFornecedor_Veiculos.ContratoVeiculo
        ,ViewAtaFornecedor.AtaVeiculo
        ,Veiculo.ValorMensal
-- Create or alter view [dbo].[ViewVeiculos]
--;
GO
-- View: ViewCalculaMediana
PRINT 'Criando/atualizando view: ViewCalculaMediana...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCalculaMediana')
    DROP VIEW [dbo].[ViewCalculaMediana]
GO

CREATE OR ALTER VIEW dbo.[ViewCalculaMediana (backup)] 
AS SELECT x.TempoEspera 
FROM   (SELECT TempoEspera, 
               Count(1) OVER (partition BY 'A')        AS TotalRows, 
               Row_number() OVER (ORDER BY TempoEspera ASC) AS AmountOrder 
        FROM   CorridasCanceladasTaxiLeg ft) x 
WHERE  x.AmountOrder = Round(x.TotalRows / 2.0, 0)  
-- Create table [dbo].[ControleAcesso_BACKUP]
--;
GO
-- View: ViewProcuraFicha
PRINT 'Criando/atualizando view: ViewProcuraFicha...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewProcuraFicha')
    DROP VIEW [dbo].[ViewProcuraFicha]
GO

CREATE OR ALTER VIEW dbo.ViewProcuraFicha 
AS SELECT 
MotoristaId, 
VeiculoId, 
DataInicial, 
DataFinal, 
(FORMAT(DATEPART(hour, HoraInicio), '00') + ':' + FORMAT(DATEPART(minute, HoraInicio), '00')) AS HoraInicio,
(FORMAT(DATEPART(hour, HoraFim), '00') + ':' + FORMAT(DATEPART(minute, HoraFim), '00')) AS HoraFim,
NoFichaVistoria
FROM
Viagem
WHERE Status NOT LIKE 'Cancelada' AND StatusAgendamento = 0
-- Create or alter view [dbo].[ViewPendenciasManutencao]
--;
GO
-- View: ViewVeiculoCompleto
PRINT 'Criando/atualizando view: ViewVeiculoCompleto...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewVeiculoCompleto')
    DROP VIEW [dbo].[ViewVeiculoCompleto]
GO

CREATE OR ALTER VIEW dbo.ViewVeiculoCompleto 
AS SELECT
    v.VeiculoId,
    v.Status,
    ISNULL(v.Placa, '') AS Placa,
    ISNULL(v.Placa, '') + ' - ' +
        ISNULL(m.DescricaoMarca, 'Sem Marca') + '/' +
        ISNULL(md.DescricaoModelo, 'Sem Modelo') AS VeiculoCompleto
FROM
    dbo.Veiculo v
LEFT JOIN
    dbo.MarcaVeiculo m ON v.MarcaId = m.MarcaId
LEFT JOIN
    dbo.ModeloVeiculo md ON v.ModeloId = md.ModeloId
WHERE v.Status = 1
-- Create or alter view [dbo].[ViewExisteItemContrato]
--;
GO
-- View: ViewAbastecimentosPBI
PRINT 'Criando/atualizando view: ViewAbastecimentosPBI...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAbastecimentosPBI')
    DROP VIEW [dbo].[ViewAbastecimentosPBI]
GO

CREATE OR ALTER VIEW dbo.ViewAbastecimentosPBI 
AS SELECT
  Abastecimento.AbastecimentoId
 ,ViewAbastecimentosItensPBI.VeiculoID
 ,CASE
    WHEN ViewAbastecimentosItensPBI.Descricao IS NULL THEN 'Veículo Próprio'
    ELSE ViewAbastecimentosItensPBI.Descricao
  END AS Descricao
 ,ViewAbastecimentosItensPBI.Categoria
 ,Abastecimento.Litros
 ,Abastecimento.ValorUnitario
 ,Abastecimento.KmRodado
 ,CONVERT(VARCHAR, Abastecimento.DataHora, 103) AS DataAbastecimento
 ,FORMAT(Abastecimento.DataHora, 'HH:mm') AS HoraAbastecimento
 ,Combustivel.Descricao AS DescricaoCombustivel
FROM dbo.ViewAbastecimentosItensPBI
RIGHT OUTER JOIN dbo.Abastecimento
  ON ViewAbastecimentosItensPBI.VeiculoID = Abastecimento.VeiculoId
INNER JOIN dbo.Combustivel
  ON Abastecimento.CombustivelId = Combustivel.CombustivelId
-- Create or alter function [dbo].[fn_CalculaMinutosUteis]
--;
GO
-- View: ViewOcorrenciasViagem
PRINT 'Criando/atualizando view: ViewOcorrenciasViagem...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewOcorrenciasViagem')
    DROP VIEW [dbo].[ViewOcorrenciasViagem]
GO

CREATE OR ALTER VIEW dbo.ViewOcorrenciasViagem 
AS SELECT 
    oc.OcorrenciaViagemId,
    oc.ViagemId,
    oc.VeiculoId,
    oc.MotoristaId,
    ISNULL(oc.Resumo, '') AS Resumo,
    ISNULL(oc.Descricao, '') AS Descricao,
    ISNULL(oc.ImagemOcorrencia, '') AS ImagemOcorrencia,
    ISNULL(oc.Status, 'Aberta') AS Status,
    oc.DataCriacao,
    oc.DataBaixa,
    ISNULL(oc.UsuarioCriacao, '') AS UsuarioCriacao,
    ISNULL(oc.UsuarioBaixa, '') AS UsuarioBaixa,
    oc.ItemManutencaoId,
    ISNULL(oc.Observacoes, '') AS Observacoes,
    vi.DataInicial,
    vi.DataFinal,
    vi.HoraInicio,
    vi.HoraFim,
    vi.NoFichaVistoria,
    ISNULL(vi.Origem, '') AS Origem,
    ISNULL(vi.Destino, '') AS Destino,
    ISNULL(vi.Finalidade, '') AS FinalidadeViagem,
    ISNULL(vi.Status, '') AS StatusViagem,
    ISNULL(ve.Placa, '') AS Placa,
    ISNULL(ma.DescricaoMarca, '') AS DescricaoMarca,
    ISNULL(mo.DescricaoModelo, '') AS DescricaoModelo,
    ISNULL(CONCAT(ve.Placa, ' - ', ma.DescricaoMarca, '/', mo.DescricaoModelo), '') AS VeiculoCompleto,
    ISNULL(CONCAT(ma.DescricaoMarca, '/', mo.DescricaoModelo), '') AS MarcaModelo,
    ISNULL(mt.Nome, '') AS NomeMotorista,
    CAST('' AS VARCHAR(1)) AS FotoMotorista,  -- Campo vazio como placeholder
    DATEDIFF(DAY, oc.DataCriacao, GETDATE()) AS DiasEmAberto,
    CASE 
        WHEN oc.Status = 'Baixada' THEN 'Resolvida'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN 'Crítica'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN 'Alta'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN 'Média'
        ELSE 'Normal'
    END AS Urgencia,
    CASE 
        WHEN oc.Status = 'Baixada' THEN '#28a745'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN '#dc3545'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN '#ffc107'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN '#17a2b8'
        ELSE '#6c757d'
    END AS CorUrgencia
FROM dbo.OcorrenciaViagem oc
    LEFT JOIN dbo.Viagem vi ON oc.ViagemId = vi.ViagemId
    LEFT JOIN dbo.Veiculo ve ON oc.VeiculoId = ve.VeiculoId
    LEFT JOIN dbo.MarcaVeiculo ma ON ve.MarcaId = ma.MarcaId
    LEFT JOIN dbo.ModeloVeiculo mo ON ve.ModeloId = mo.ModeloId
    LEFT JOIN dbo.Motorista mt ON oc.MotoristaId = mt.MotoristaId
-- Create or alter view [dbo].[ViewOcorrenciasAbertasVeiculo]
--;
GO
-- View: ViewAbastecimentosPBIAta
PRINT 'Criando/atualizando view: ViewAbastecimentosPBIAta...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAbastecimentosPBIAta')
    DROP VIEW [dbo].[ViewAbastecimentosPBIAta]
GO

CREATE OR ALTER VIEW dbo.ViewAbastecimentosPBIAta 
AS SELECT
  Veiculo.VeiculoId
 ,Veiculo.Placa
 ,Veiculo.Categoria
 ,ItemVeiculoAta.Descricao
FROM dbo.VeiculoAta
INNER JOIN dbo.Veiculo
  ON VeiculoAta.VeiculoId = Veiculo.VeiculoId
INNER JOIN dbo.ItemVeiculoAta
  ON Veiculo.ItemVeiculoAtaId = ItemVeiculoAta.ItemVeiculoAtaId
INNER JOIN dbo.RepactuacaoAta
  ON VeiculoAta.AtaId = RepactuacaoAta.AtaId
-- Create or alter view [dbo].[ViewAbastecimentosItensPBI]
--;
GO
-- View: ViewVeiculos
PRINT 'Criando/atualizando view: ViewVeiculos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewVeiculos')
    DROP VIEW [dbo].[ViewVeiculos]
GO

CREATE OR ALTER VIEW dbo.ViewVeiculos 
AS SELECT
  Veiculo.VeiculoId
 ,Veiculo.Placa
 ,Economildo
 ,Quilometragem
 ,Veiculo.Categoria
 ,Veiculo.Placa +  ' (' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo + ')' AS VeiculoCompleto
 ,MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS MarcaModelo
 ,Unidade.Sigla
 ,Combustivel.Descricao
 ,CASE
    WHEN CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2)) IS NULL THEN 0
    ELSE CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2))
  END AS Consumo
 ,CASE
    WHEN ViewContratoFornecedor_Veiculos.ContratoVeiculo IS NOT NULL THEN ViewContratoFornecedor_Veiculos.ContratoVeiculo
    WHEN ViewAtaFornecedor.AtaVeiculo IS NOT NULL THEN ViewAtaFornecedor.AtaVeiculo + ' <b>(Ata)</b> '
    WHEN Veiculo.VeiculoProprio = 1 THEN '<b><i>(Veículo Próprio)</i></b>'
    ELSE '<b><i style="color: #dc3545;">(Sem Contrato/Ata)</i></b>'
  END AS OrigemVeiculo
 ,CONVERT(VARCHAR, Veiculo.DataAlteracao, 103) AS DataAlteracao
 ,AspNetUsers.NomeCompleto
 ,CASE
    WHEN Veiculo.Reserva = 0 THEN 'Efetivo'
    ELSE 'Reserva'
  END AS VeiculoReserva
 ,Veiculo.Status
 ,Veiculo.CombustivelId
 ,ROW_NUMBER() OVER (ORDER BY Veiculo.Placa) AS RowNum
 ,ViewContratoFornecedor_Veiculos.ContratoId
 ,ViewAtaFornecedor.AtaId
 ,ViewContratoFornecedor_Veiculos.ContratoVeiculo
 ,ViewAtaFornecedor.AtaVeiculo
 ,Veiculo.VeiculoProprio
 ,Veiculo.ItemVeiculoAtaId
 ,Veiculo.ItemVeiculoId
 ,Veiculo.ValorMensal
FROM dbo.Abastecimento
RIGHT OUTER JOIN dbo.Veiculo
  ON Abastecimento.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
LEFT OUTER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
LEFT OUTER JOIN dbo.Unidade
  ON Unidade.UnidadeId = Veiculo.UnidadeId
LEFT OUTER JOIN dbo.Combustivel
  ON Combustivel.CombustivelId = Veiculo.CombustivelId
LEFT OUTER JOIN dbo.ViewContratoFornecedor_Veiculos
  ON ViewContratoFornecedor_Veiculos.ContratoId = Veiculo.ContratoId
LEFT OUTER JOIN dbo.ViewAtaFornecedor
  ON ViewAtaFornecedor.AtaId = Veiculo.AtaId
INNER JOIN dbo.AspNetUsers
  ON AspNetUsers.Id = Veiculo.UsuarioIdAlteracao
GROUP BY Veiculo.VeiculoId
        ,Veiculo.Placa
        ,Veiculo.Quilometragem
        ,MarcaVeiculo.DescricaoMarca
        ,ModeloVeiculo.DescricaoModelo
        ,Unidade.Sigla
        ,Combustivel.Descricao
        ,ViewContratoFornecedor_Veiculos.ContratoId
        ,Veiculo.DataAlteracao
        ,AspNetUsers.NomeCompleto
        ,Veiculo.Reserva
        ,Veiculo.Status
        ,Veiculo.CombustivelId
        ,ViewAtaFornecedor.AtaId
        ,ViewContratoFornecedor_Veiculos.ContratoVeiculo
        ,ViewAtaFornecedor.AtaVeiculo
        ,Veiculo.VeiculoProprio
        ,Veiculo.ItemVeiculoAtaId
        ,Veiculo.ItemVeiculoId
        ,Veiculo.ValorMensal
        ,Economildo
        ,Veiculo.Categoria
-- Create or alter view [dbo].[ViewManutencao.OLD]
--;
GO
-- View: ViewVeiculosManutencaoReserva
PRINT 'Criando/atualizando view: ViewVeiculosManutencaoReserva...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewVeiculosManutencaoReserva')
    DROP VIEW [dbo].[ViewVeiculosManutencaoReserva]
GO

CREATE OR ALTER VIEW dbo.ViewVeiculosManutencaoReserva 
AS SELECT TOP 1000
    v.VeiculoId,
    (v.Placa + ' - ' + ma.DescricaoMarca + '/' + m.DescricaoModelo) AS Descricao
FROM 
    dbo.Veiculo v
INNER JOIN 
    dbo.ModeloVeiculo m ON v.ModeloId = m.ModeloId
INNER JOIN 
    dbo.MarcaVeiculo ma ON v.MarcaId = ma.MarcaId
WHERE 
    v.Status = 1 AND v.Reserva = 1
ORDER BY 
    Descricao
-- Create or alter view [dbo].[ViewVeiculosManutencao]
--;
GO
-- View: ViewAbastecimentosPBIProprio
PRINT 'Criando/atualizando view: ViewAbastecimentosPBIProprio...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAbastecimentosPBIProprio')
    DROP VIEW [dbo].[ViewAbastecimentosPBIProprio]
GO

CREATE OR ALTER VIEW dbo.ViewAbastecimentosPBIProprio 
AS SELECT
  Veiculo.VeiculoId
 ,MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS Descricao
 ,Veiculo.Categoria
FROM dbo.Veiculo
LEFT OUTER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
LEFT OUTER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
WHERE VeiculoProprio = 1
GROUP BY Veiculo.VeiculoId
        ,Veiculo.Categoria
        ,MarcaVeiculo.DescricaoMarca
        ,ModeloVeiculo.DescricaoModelo
-- Create table [dbo].[VeiculoPadraoViagem]
--;
GO
-- View: ViewMotoristasViagem
PRINT 'Criando/atualizando view: ViewMotoristasViagem...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristasViagem')
    DROP VIEW [dbo].[ViewMotoristasViagem]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristasViagem 
AS SELECT TOP 10000
    m.MotoristaId,
    m.Nome,
    m.TipoCondutor,
    m.Status,
    m.Foto,
    CAST(m.Nome + ' (' + m.TipoCondutor + ')' AS NVARCHAR(300)) AS MotoristaCondutor
FROM dbo.Motorista AS m
WHERE m.Status = 1
ORDER BY m.Nome
-- Create or alter view [dbo].[ViewMotoristas_Ultimo]
--;
GO
-- View: ViewAtaFornecedor
PRINT 'Criando/atualizando view: ViewAtaFornecedor...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAtaFornecedor')
    DROP VIEW [dbo].[ViewAtaFornecedor]
GO

CREATE OR ALTER VIEW dbo.ViewAtaFornecedor 
AS SELECT
  AtaRegistroPrecos.AtaId
  ,(AtaRegistroPrecos.AnoAta+ '/' + AtaRegistroPrecos.NumeroAta + ' - ' + Fornecedor.DescricaoFornecedor) AS AtaVeiculo

FROM dbo.Fornecedor
INNER JOIN dbo.AtaRegistroPrecos
  ON Fornecedor.FornecedorId = AtaRegistroPrecos.FornecedorId
-- Create table [dbo].[VeiculoAta]
--;
GO
-- View: ViewMultas
PRINT 'Criando/atualizando view: ViewMultas...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMultas')
    DROP VIEW [dbo].[ViewMultas]
GO

CREATE OR ALTER VIEW dbo.ViewMultas 
AS SELECT TOP 100000
  Multa.MultaId
 ,Multa.NumInfracao
 ,Multa.Data AS DataOriginal
 ,CONVERT(VARCHAR, Multa.Data, 103) AS Data
 ,FORMAT(Multa.Hora, 'HH:mm') AS Hora
 ,(Motorista.Nome + ' - (' + Motorista.TipoCondutor + ')') AS Nome
 ,Motorista.MotoristaId
 ,CASE
    WHEN Motorista.Celular02 IS NULL THEN Motorista.Celular01
    ELSE (Motorista.Celular01 + ' / ' + Motorista.Celular02) 
  END AS Telefone
 ,Veiculo.Placa
 ,Veiculo.VeiculoId
 ,OrgaoAutuante.Sigla
 ,OrgaoAutuante.OrgaoAutuanteId
 ,Multa.Localizacao
 ,TipoMulta.Artigo
 ,CONVERT(VARCHAR, Multa.Vencimento, 103) AS Vencimento
 ,Multa.AutuacaoPDF
 ,Multa.PenalidadePDF
 ,Multa.ValorAteVencimento
 ,Multa.ComprovantePDF
 ,Multa.ValorPosVencimento
 ,Multa.ProcessoEDoc
 ,Multa.Status
 ,Multa.Fase
 ,TipoMulta.Descricao
 ,Multa.Observacao
 ,Multa.Paga
 ,Multa.ValorPago
 ,Multa.TipoMultaId
 ,CONVERT(VARCHAR, Multa.DataPagamento, 103) AS DataPagamento
FROM dbo.Multa
LEFT OUTER JOIN dbo.Motorista
  ON Multa.MotoristaId = Motorista.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
  ON Multa.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.OrgaoAutuante
  ON Multa.OrgaoAutuanteId = OrgaoAutuante.OrgaoAutuanteId
LEFT OUTER JOIN dbo.TipoMulta
  ON Multa.TipoMultaId = TipoMulta.TipoMultaId
LEFT OUTER JOIN dbo.EmpenhoMulta
  ON Multa.EmpenhoMultaId = EmpenhoMulta.EmpenhoMultaId
    AND EmpenhoMulta.OrgaoAutuanteId = OrgaoAutuante.OrgaoAutuanteId
ORDER BY DataOriginal DESC
-- Create or alter view [dbo].[ViewMotoristasMulta]
--;
GO
-- View: ViewSabrina
PRINT 'Criando/atualizando view: ViewSabrina...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewSabrina')
    DROP VIEW [dbo].[ViewSabrina]
GO

CREATE OR ALTER VIEW dbo.ViewSabrina 
AS SELECT
  Combustivel.Descricao
 ,Veiculo.Placa
 ,Abastecimento.Litros
 ,Abastecimento.ValorUnitario
 ,Abastecimento.DataHora
FROM dbo.Abastecimento
INNER JOIN dbo.Combustivel
  ON Abastecimento.CombustivelId = Combustivel.CombustivelId
INNER JOIN dbo.Veiculo
  ON Abastecimento.VeiculoId = Veiculo.VeiculoId
-- Create or alter view [dbo].[ViewMediaConsumo]
--;
GO
-- View: ViewVeiculosManutencao
PRINT 'Criando/atualizando view: ViewVeiculosManutencao...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewVeiculosManutencao')
    DROP VIEW [dbo].[ViewVeiculosManutencao]
GO

CREATE OR ALTER VIEW dbo.ViewVeiculosManutencao 
AS SELECT TOP 1000
    v.VeiculoId,
    (v.Placa + ' - ' + ma.DescricaoMarca + '/' + m.DescricaoModelo) AS Descricao
FROM 
    dbo.Veiculo v
INNER JOIN 
    dbo.ModeloVeiculo m ON v.ModeloId = m.ModeloId
INNER JOIN 
    dbo.MarcaVeiculo ma ON v.MarcaId = ma.MarcaId
WHERE 
    v.Status = 1 -- BIT: TRUE
ORDER BY 
    Descricao
-- Create or alter view [dbo].[ViewVeiculoCompleto]
--;
GO
-- View: ViewEventos
PRINT 'Criando/atualizando view: ViewEventos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewEventos')
    DROP VIEW [dbo].[ViewEventos]
GO

CREATE OR ALTER VIEW dbo.ViewEventos 
AS SELECT 
    e.EventoId,
    e.Nome,
    e.Descricao,
    e.QtdParticipantes,
    e.DataInicial,
    e.DataFinal,
    r.Nome AS NomeRequisitante,
    s.Nome AS NomeSetor,
    s.Sigla AS SiglaSetor,
    ISNULL(cv.CustoViagem, 0) AS CustoViagem,
    e.Status
FROM dbo.Evento e
LEFT JOIN dbo.Requisitante r 
    ON e.RequisitanteId = r.RequisitanteId
LEFT JOIN dbo.SetorSolicitante s 
    ON e.SetorSolicitanteId = s.SetorSolicitanteId
-- ✅ SUBQUERY AGREGADA: Executa 1x para TODOS os eventos
LEFT JOIN (
    SELECT 
        EventoId,
        ROUND(
            SUM(
                ISNULL(CustoCombustivel, 0) + 
                ISNULL(CustoMotorista, 0) + 
                ISNULL(CustoVeiculo, 0) + 
                ISNULL(CustoOperador, 0) + 
                ISNULL(CustoLavador, 0)
            ), 2
        ) AS CustoViagem
    FROM dbo.Viagem
    WHERE EventoId IS NOT NULL
    GROUP BY EventoId
) cv ON e.EventoId = cv.EventoId
-- Create or alter view [dbo].[ViewCustosViagem]
--;
GO
-- View: ViewCustoAbastecimento
PRINT 'Criando/atualizando view: ViewCustoAbastecimento...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustoAbastecimento')
    DROP VIEW [dbo].[ViewCustoAbastecimento]
GO

CREATE OR ALTER VIEW dbo.ViewCustoAbastecimento 
AS SELECT
  Combustivel.Descricao
 ,DAY(Abastecimento.DataHora) AS Dia
 ,SUM(Abastecimento.Litros * Abastecimento.ValorUnitario) AS Valor
FROM dbo.Abastecimento
INNER JOIN dbo.Combustivel
  ON Abastecimento.CombustivelId = Combustivel.CombustivelId
WHERE MONTH(Abastecimento.DataHora) = 07
AND YEAR(Abastecimento.DataHora) = 2022
GROUP BY Combustivel.Descricao
        ,Abastecimento.DataHora
-- Create or alter view [dbo].[ViewAbastecimentosPBI]
--;
GO
-- View: ViewMotoristas
PRINT 'Criando/atualizando view: ViewMotoristas...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristas')
    DROP VIEW [dbo].[ViewMotoristas]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristas 
AS SELECT TOP 10000
  Motorista.MotoristaId
 ,Motorista.Nome
 ,Motorista.Ponto
 ,Motorista.CNH
 ,Motorista.CategoriaCNH
 ,Motorista.Celular01
 ,Motorista.Status
 ,Unidade.Sigla
 ,Contrato.AnoContrato
 ,Contrato.NumeroContrato
 ,Fornecedor.DescricaoFornecedor
 ,AspNetUsers.NomeCompleto
 ,Motorista.DataAlteracao
 ,Motorista.ContratoId
 ,Motorista.TipoCondutor 
 ,Motorista.Foto
 ,Motorista.EfetivoFerista   
,Motorista.Nome + ' (' + Motorista.TipoCondutor + ')' AS MotoristaCondutor
 ,ROW_NUMBER() OVER (ORDER BY Motorista.Nome) AS RowNum
FROM dbo.Motorista
LEFT OUTER JOIN dbo.Contrato
  ON Motorista.ContratoId = Contrato.ContratoId
LEFT OUTER JOIN dbo.Fornecedor
  ON Contrato.FornecedorId = Fornecedor.FornecedorId
INNER JOIN dbo.AspNetUsers
  ON Motorista.UsuarioIdAlteracao = AspNetUsers.Id
LEFT OUTER JOIN dbo.Unidade
  ON Motorista.UnidadeId = Unidade.UnidadeId
ORDER BY Motorista.Nome
-- Create or alter view [dbo].[ViewItensManutencao]
--;
GO
-- View: ViewMotoristas_original
PRINT 'Criando/atualizando view: ViewMotoristas_original...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristas_original')
    DROP VIEW [dbo].[ViewMotoristas_original]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristas_original 
AS SELECT
  Motorista.MotoristaId
 ,Motorista.Nome
 ,Motorista.Ponto
 ,Motorista.CNH
 ,Motorista.CategoriaCNH
 ,Motorista.Celular01
 ,Motorista.Status
 ,Unidade.Sigla
 ,Contrato.AnoContrato
 ,Contrato.NumeroContrato
 ,Fornecedor.DescricaoFornecedor
 ,AspNetUsers.NomeCompleto
 ,Motorista.DataAlteracao
 ,Motorista.ContratoId
 ,Motorista.CondutorId
 ,Motorista.Foto
 ,CondutorApoio.Descricao
 ,CASE
    WHEN CondutorApoio.Descricao IS NULL THEN (Motorista.Nome + '(Terceirizado)') 
    ELSE Motorista.Nome + '(' + CondutorApoio.Descricao + ')' 
    END AS MotoristaCondutor
  ,ROW_NUMBER() OVER (ORDER BY Motorista.Nome) AS RowNum
FROM dbo.Motorista
LEFT OUTER JOIN dbo.Contrato
  ON Motorista.ContratoId = Contrato.ContratoId
LEFT OUTER JOIN dbo.Fornecedor
  ON Contrato.FornecedorId = Fornecedor.FornecedorId
INNER JOIN dbo.AspNetUsers
  ON Motorista.UsuarioIdAlteracao = AspNetUsers.Id
LEFT OUTER JOIN dbo.Unidade
  ON Motorista.UnidadeId = Unidade.UnidadeId
LEFT OUTER JOIN dbo.CondutorApoio
  ON Motorista.CondutorId = CondutorApoio.CondutorId
-- Create or alter view [dbo].[ViewMotoristas]
--;
GO
-- View: ViewCustosMotoristas_Individual
PRINT 'Criando/atualizando view: ViewCustosMotoristas_Individual...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustosMotoristas_Individual')
    DROP VIEW [dbo].[ViewCustosMotoristas_Individual]
GO

CREATE OR ALTER VIEW dbo.ViewCustosMotoristas_Individual 
AS SELECT
  Viagem.NoFichaVistoria
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
 ,CONVERT(VARCHAR, Viagem.DataFinal, 103) AS DataFinal
 ,FORMAT(Viagem.KmFinal - Viagem.KmInicial, 'N2', 'pt-BR') AS Quilometragem
FROM dbo.Viagem
INNER JOIN dbo.Motorista
  ON Viagem.MotoristaId = Motorista.MotoristaId
WHERE Viagem.Status = 'Realizada'
AND Viagem.Finalidade NOT LIKE 'Manutenção'
AND Viagem.Finalidade NOT LIKE 'Devolução à Locadora'
AND Motorista.TipoCondutor = 'Terceirizado'
AND Motorista.MotoristaId = '00a5ece9-1735-47c0-5eb5-08d97c53518f'
AND MONTH(Viagem.DataInicial) = 06
-- Create or alter view [dbo].[ViewCustosMotoristas]
--;
GO
-- View: ViewLotacoes
PRINT 'Criando/atualizando view: ViewLotacoes...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewLotacoes')
    DROP VIEW [dbo].[ViewLotacoes]
GO

CREATE OR ALTER VIEW dbo.ViewLotacoes 
AS SELECT
  Unidade.Categoria AS NomeCategoria
 ,Unidade.Descricao + ' ('+ Unidade.Sigla + ')' AS Unidade
 ,Motorista.Nome AS Motorista
 ,CONVERT(VARCHAR, LotacaoMotorista.DataInicio, 103) AS DataInicio
 ,LotacaoMotorista.LotacaoMotoristaId
 ,LotacaoMotorista.Lotado
 ,Motorista.MotoristaId
 ,Unidade.UnidadeId
FROM dbo.LotacaoMotorista
INNER JOIN dbo.Unidade
  ON LotacaoMotorista.UnidadeId = Unidade.UnidadeId
INNER JOIN dbo.Motorista
  ON LotacaoMotorista.MotoristaId = Motorista.MotoristaId
-- Create or alter view [dbo].[ViewLotacaoMotorista]
--;
GO
-- View: ViewSetores
PRINT 'Criando/atualizando view: ViewSetores...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewSetores')
    DROP VIEW [dbo].[ViewSetores]
GO

CREATE OR ALTER VIEW dbo.ViewSetores 
AS SELECT
  SetorSolicitante.SetorSolicitanteId
  ,CASE
    WHEN SetorSolicitante.Sigla IS NULL THEN (SetorSolicitante.Nome) 
    ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')' 
    END AS Nome
 ,SetorSolicitante.SetorPaiId
 ,SetorSolicitante.Status
FROM dbo.SetorSolicitante
WHERE SetorSolicitante.Status = 1
-- Create table [dbo].[Requisitante]
--;
GO
-- View: ViewViagens
PRINT 'Criando/atualizando view: ViewViagens...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewViagens')
    DROP VIEW [dbo].[ViewViagens]
GO

CREATE OR ALTER VIEW dbo.ViewViagens 
AS SELECT
    Viagem.ViagemId,
    Viagem.DataInicial,
    Viagem.DataFinal,
    Viagem.HoraInicio,
    Viagem.HoraFim,
    Viagem.Descricao,
    Viagem.Status,
    Viagem.KmInicial,
    Viagem.KmFinal,
    Viagem.CombustivelInicial,
    Viagem.CombustivelFinal,
    Viagem.MotoristaId,
    Viagem.VeiculoId,
    Viagem.ResumoOcorrencia,
    Viagem.DescricaoOcorrencia,
    Viagem.StatusOcorrencia,
    Viagem.StatusDocumento,
    Viagem.StatusCartaoAbastecimento,
    Viagem.StatusAgendamento,
    Viagem.NoFichaVistoria,
    Viagem.Finalidade,
    Veiculo.Placa,
    Motorista.CNH,
    Motorista.Foto,
    Motorista.Ponto,
    convert(nvarchar(36), Viagem.ViagemId) AS ViagemIDStr,
    CASE
        WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
        ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
    END AS NomeRequisitante,
    CASE
        WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
        ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
    END AS NomeSetor,
    Requisitante.RequisitanteId,
    SetorSolicitante.SetorSolicitanteId,
    Motorista.Nome AS NomeMotorista,
    '(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo,
    Veiculo.UnidadeId,
    ROW_NUMBER() OVER (ORDER BY Viagem.DataInicial, Viagem.HoraInicio) AS RowNum,
    Veiculo.CombustivelId,
    Viagem.DescricaoSolucaoOcorrencia,
    Viagem.FichaVistoria,
    Viagem.Origem,
    Viagem.Destino,
    Viagem.Minutos,
    Viagem.NomeEvento,
    Viagem.RamalRequisitante,
    Viagem.ImagemOcorrencia,
    Viagem.ItemManutencaoId,
    ROUND((Viagem.CustoCombustivel + Viagem.CustoMotorista + Viagem.CustoVeiculo + Viagem.CustoOperador + Viagem.CustoLavador), 2) AS CustoViagem,
    Viagem.EventoId,

    -- NOVAS COLUNAS: Lógica de cor do evento
    CorEvento = CASE
        WHEN Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada' AND Viagem.Finalidade = 'Evento' THEN '#E99B63'
        WHEN Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada' THEN '#29B3FF'
        WHEN Viagem.Status = 'Cancelada' THEN '#E34234'
        WHEN Viagem.Status = 'Aberta' AND ISNULL(Viagem.StatusAgendamento, 0) = 0 THEN '#FFD774'
        WHEN Viagem.Status = 'Realizada' AND ISNULL(Viagem.FoiAgendamento, 0) = 1 THEN '#52688F'
        WHEN Viagem.Status = 'Realizada' THEN '#75B390'
        WHEN Viagem.Finalidade = 'Evento' THEN '#E99B63'
        ELSE '#29B3FF'
    END,

    CorTexto = CASE
        WHEN (Viagem.StatusAgendamento = 1 AND Viagem.Status <> 'Cancelada')
          OR (Viagem.Status = 'Realizada')
          OR (Viagem.Finalidade = 'Evento')
          OR (Viagem.Status = 'Cancelada')
          THEN 'white'
        WHEN (Viagem.Status = 'Aberta' AND ISNULL(Viagem.StatusAgendamento, 0) = 0)
          THEN '#2B6670'
        ELSE 'white'
    END,

    -- Descrição Montada
    DescricaoMontada = 
        (
            ISNULL(Motorista.Nome, '(Motorista Não Identificado)')
            + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veículo') + ')'
            + CASE 
                WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                THEN ' - ' + Viagem.Descricao
                ELSE ''
              END
        ),

    -- Descrição especial para eventos (seguindo a mesma lógica do C#)
    DescricaoEvento =
        CASE
            WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL AND Viagem.Status = 'Cancelada'
                THEN 'Evento CANCELADO: ' + Viagem.NomeEvento + ' / '
                    + ISNULL(Motorista.Nome, '(Motorista Não Identificado)')
                    + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veículo') + ')'
                    + CASE WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                        THEN ' - ' + Viagem.Descricao
                        ELSE '' END
            WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL
                THEN 'Evento: ' + Viagem.NomeEvento + ' / '
                    + ISNULL(Motorista.Nome, '(Motorista Não Identificado)')
                    + ' - (' + ISNULL(Veiculo.Placa, 'Sem Veículo') + ')'
                    + CASE WHEN Viagem.Descricao IS NOT NULL AND LEN(LTRIM(RTRIM(Viagem.Descricao))) > 0
                        THEN ' - ' + Viagem.Descricao
                        ELSE '' END
            ELSE NULL
        END,

    -- Título do Evento
    Titulo = 
        CASE
            WHEN Viagem.Finalidade = 'Evento' AND Viagem.NomeEvento IS NOT NULL THEN 'Evento : ' + Viagem.NomeEvento
            ELSE Viagem.Finalidade
        END

FROM dbo.SetorSolicitante
RIGHT OUTER JOIN dbo.Viagem
    ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
LEFT OUTER JOIN dbo.Requisitante
    ON Requisitante.RequisitanteId = Viagem.RequisitanteId
LEFT OUTER JOIN dbo.Motorista
    ON Motorista.MotoristaId = Viagem.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
    ON Veiculo.VeiculoId = Viagem.VeiculoId
INNER JOIN dbo.ModeloVeiculo
    ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
    ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
-- Create or alter view [dbo].[ViewProcuraFicha]
--;
GO
-- View: ViewItensManutencao
PRINT 'Criando/atualizando view: ViewItensManutencao...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewItensManutencao')
    DROP VIEW [dbo].[ViewItensManutencao]
GO

CREATE OR ALTER VIEW dbo.ViewItensManutencao 
AS SELECT
   im.ItemManutencaoId,
   im.ManutencaoId,
   im.TipoItem,
   im.NumFicha,
   CONVERT(varchar(10), im.DataItem, 103) AS DataItem,  -- dd/MM/yyyy
   im.Resumo,
   im.Descricao,
   im.Status,
   im.ImagemOcorrencia,
   m.Nome AS NomeMotorista,
   im.MotoristaId,
   im.ViagemId
FROM dbo.ItensManutencao AS im
LEFT JOIN dbo.Motorista  AS m
  ON m.MotoristaId = im.MotoristaId
-- Create or alter view [dbo].[ViewAlocacaoMotorista]
--;
GO
-- View: ViewExisteItemContrato
PRINT 'Criando/atualizando view: ViewExisteItemContrato...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewExisteItemContrato')
    DROP VIEW [dbo].[ViewExisteItemContrato]
GO

CREATE OR ALTER VIEW dbo.ViewExisteItemContrato 
AS SELECT
  ItemVeiculoContrato.ItemVeiculoId
 ,ItemVeiculoContrato.NumItem
 ,ItemVeiculoContrato.Descricao
 ,ItemVeiculoContrato.Quantidade
 ,ItemVeiculoContrato.ValorUnitario AS ValUnitario
 ,ItemVeiculoContrato.RepactuacaoContratoId
 ,(SELECT DISTINCT ItemVeiculoId FROM Veiculo WHERE Veiculo.ItemVeiculoId = ItemVeiculoContrato.ItemVeiculoId) AS ExisteVeiculo
FROM dbo.ItemVeiculoContrato
-- Create or alter view [dbo].[ViewAbastecimentosPBIProprio]
--;
GO
-- View: ViewMediaConsumo
PRINT 'Criando/atualizando view: ViewMediaConsumo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMediaConsumo')
    DROP VIEW [dbo].[ViewMediaConsumo]
GO

CREATE OR ALTER VIEW dbo.ViewMediaConsumo 
AS SELECT
  Abastecimento.VeiculoId
 ,CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2)) AS ConsumoGeral
FROM dbo.Abastecimento
GROUP BY Abastecimento.VeiculoId
-- Create or alter view [dbo].[ViewAbastecimentos]
--;
GO
-- View: ViewAbastecimentosPBIContrato
PRINT 'Criando/atualizando view: ViewAbastecimentosPBIContrato...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAbastecimentosPBIContrato')
    DROP VIEW [dbo].[ViewAbastecimentosPBIContrato]
GO

CREATE OR ALTER VIEW dbo.ViewAbastecimentosPBIContrato 
AS SELECT
  Veiculo.VeiculoId
 ,Veiculo.Placa
 ,Veiculo.Categoria
 ,ItemVeiculoContrato.Descricao
FROM dbo.VeiculoContrato
INNER JOIN dbo.Veiculo
  ON VeiculoContrato.VeiculoId = Veiculo.VeiculoId
INNER JOIN dbo.ItemVeiculoContrato
  ON Veiculo.ItemVeiculoId = ItemVeiculoContrato.ItemVeiculoId
INNER JOIN dbo.RepactuacaoContrato
  ON ItemVeiculoContrato.RepactuacaoContratoId = RepactuacaoContrato.RepactuacaoContratoId
    AND VeiculoContrato.ContratoId = RepactuacaoContrato.ContratoId
-- Create table [dbo].[RepactuacaoVeiculo]
--;
GO
-- View: ViewCustosViagem
PRINT 'Criando/atualizando view: ViewCustosViagem...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustosViagem')
    DROP VIEW [dbo].[ViewCustosViagem]
GO

CREATE OR ALTER VIEW dbo.ViewCustosViagem 
AS SELECT
  Viagem.NoFichaVistoria
 ,Viagem.ViagemId
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
 ,CONVERT(VARCHAR, Viagem.DataFinal, 103) AS DataFinal
 ,FORMAT(Viagem.HoraInicio, 'HH:mm') AS HoraInicio
 ,FORMAT(Viagem.HoraFim, 'HH:mm') AS HoraFim
 ,Viagem.Finalidade
 ,Viagem.Status
 ,Viagem.KmInicial
 ,Viagem.KmFinal
 ,Viagem.KmFinal - Viagem.KmInicial AS Quilometragem
 ,FORMAT(Viagem.CustoMotorista, 'N2', 'pt-BR') AS CustoMotorista
 ,FORMAT(Viagem.CustoCombustivel, 'N2', 'pt-BR') AS CustoCombustivel
 ,FORMAT(Viagem.CustoVeiculo, 'N2', 'pt-BR') AS CustoVeiculo
 ,CONVERT(varchar, CAST(Viagem.CustoMotorista AS money), 1) AS CustoMotoristaFormatado
 ,Motorista.Nome AS NomeMotorista
 ,Motorista.MotoristaId
 ,Viagem.VeiculoId
 ,Viagem.StatusAgendamento
 ,Viagem.SetorSolicitanteId 
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,ROW_NUMBER() OVER (ORDER BY Viagem.DataInicial, Viagem.HoraInicio) AS RowNum
FROM dbo.Viagem
LEFT OUTER JOIN dbo.Motorista
  ON Motorista.MotoristaId = Viagem.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
  ON Veiculo.VeiculoId = Viagem.VeiculoId
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
WHERE Viagem.Status = 'Realizada'
AND Viagem.Finalidade NOT LIKE 'Manutenção'
AND Viagem.Finalidade NOT LIKE 'Devolução à Locadora'
-- Create or alter view [dbo].[ViewCustosVeiculos]
--;
GO
-- View: ViewManutencao
PRINT 'Criando/atualizando view: ViewManutencao...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewManutencao')
    DROP VIEW [dbo].[ViewManutencao]
GO

CREATE OR ALTER VIEW dbo.[ViewManutencao.OLD] 
AS SELECT
  Manutencao.ManutencaoId
 ,Manutencao.NumOS
 ,CONVERT(VARCHAR, Manutencao.DataSolicitacao, 103) AS DataSolicitacao
 ,Manutencao.ResumoOS
 ,CONVERT(VARCHAR, Manutencao.DataRecolhimento, 103) AS DataRecolhimento
 ,CONVERT(VARCHAR, Manutencao.DataRecebimentoReserva, 103) AS DataRecebimentoReserva
 ,CONVERT(VARCHAR, Manutencao.DataDevolucaoReserva, 103) AS DataDevolucaoReserva
 ,CONVERT(VARCHAR, Manutencao.DataEntrega, 103) AS DataEntrega
 ,Manutencao.StatusOS
 ,Manutencao.VeiculoId
 ,'(' + ViewVeiculos.Placa + ') ' + ViewVeiculos.MarcaModelo AS DescricaoVeiculo
 ,CASE
    WHEN Manutencao.ReservaEnviado IS NULL THEN ''
    WHEN Manutencao.ReservaEnviado = 1 THEN 'Enviado'
    ELSE 'Ausente'
  END AS Reserva
 ,CASE
    WHEN Manutencao.DataDevolucao IS NULL THEN '01/01/2001'
    ELSE CONVERT(VARCHAR, Manutencao.DataDevolucao, 103)
  END AS DataDevolucao
 ,CASE
    WHEN Manutencao.DataDevolucao IS NULL THEN '01'
    WHEN Manutencao.DataEntrega IS NOT NULL THEN DATEDIFF(DAY, Manutencao.DataEntrega, Manutencao.DataDevolucao)
    ELSE DATEDIFF(DAY, Manutencao.DataSolicitacao, Manutencao.DataDevolucao)
  END AS DiasGlosa
 ,ItemVeiculoContrato.NumItem
FROM dbo.ViewVeiculos
RIGHT OUTER JOIN dbo.Manutencao
  ON ViewVeiculos.VeiculoId = Manutencao.VeiculoId
LEFT OUTER JOIN dbo.ItemVeiculoContrato
  ON ItemVeiculoContrato.ItemVeiculoId = ViewVeiculos.ItemVeiculoId
-- Create or alter view [dbo].[ViewTeste]
--;
GO
-- View: ViewAbastecimentosItensPBI
PRINT 'Criando/atualizando view: ViewAbastecimentosItensPBI...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAbastecimentosItensPBI')
    DROP VIEW [dbo].[ViewAbastecimentosItensPBI]
GO

CREATE OR ALTER VIEW dbo.ViewAbastecimentosItensPBI 
AS SELECT
  ViewAbastecimentosPBIAta.VeiculoID,
  ViewAbastecimentosPBIAta.Descricao,
  ViewAbastecimentosPBIAta.Categoria
FROM dbo.ViewAbastecimentosPBIAta

UNION

SELECT
  ViewAbastecimentosPBIContrato.VeiculoID,
  ViewAbastecimentosPBIContrato.Descricao,
  ViewAbastecimentosPBIContrato.Categoria 
FROM dbo.ViewAbastecimentosPBIContrato
  
UNION 

SELECT
  ViewAbastecimentosPBIProprio.VeiculoID,
  ViewAbastecimentosPBIProprio.Descricao,
  ViewAbastecimentosPBIProprio.Categoria 
FROM dbo.ViewAbastecimentosPBIProprio
-- Create table [dbo].[Empenho]
--;
GO
-- View: ViewLotacaoMotorista
PRINT 'Criando/atualizando view: ViewLotacaoMotorista...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewLotacaoMotorista')
    DROP VIEW [dbo].[ViewLotacaoMotorista]
GO

CREATE OR ALTER VIEW dbo.ViewLotacaoMotorista 
AS SELECT TOP 1000000
  Unidade.UnidadeId
 ,LotacaoMotorista.LotacaoMotoristaId
 ,Motorista.MotoristaId
 ,LotacaoMotorista.Lotado
 ,LotacaoMotorista.Motivo
 ,Unidade.Descricao AS Unidade
 ,CONVERT(VARCHAR, LotacaoMotorista.DataInicio, 103) AS DataInicial
 ,CONVERT(VARCHAR, LotacaoMotorista.DataFim, 103) AS DataFim
 ,Motorista_1.Nome AS MotoristaCobertura
FROM dbo.LotacaoMotorista
LEFT OUTER JOIN dbo.Unidade
  ON LotacaoMotorista.UnidadeId = Unidade.UnidadeId
INNER JOIN dbo.Motorista
  ON LotacaoMotorista.MotoristaId = Motorista.MotoristaId
LEFT OUTER JOIN dbo.Motorista Motorista_1
  ON Motorista_1.MotoristaId = LotacaoMotorista.MotoristaCoberturaId
ORDER BY LotacaoMotorista.DataInicio DESC
-- Create table [dbo].[Lavagem]
--;
GO
-- View: ViewViagensAgenda
PRINT 'Criando/atualizando view: ViewViagensAgenda...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewViagensAgenda')
    DROP VIEW [dbo].[ViewViagensAgenda]
GO

CREATE OR ALTER VIEW dbo.ViewViagensAgenda
WITH SCHEMABINDING
AS
-- ================================================================
-- VIEW: ViewViagensAgenda
-- Descrição: View para alimentar o calendário FullCalendar
-- Atualizado: Cores correspondentes aos Modais do sistema
-- Última alteração: 18/01/2026 - Adicionado campo Descricao
-- ================================================================

SELECT
    -- ============================================================
    -- IDENTIFICADORES
    -- ============================================================
    v.ViagemId,
    v.MotoristaId,
    v.VeiculoId,
    v.EventoId,

    -- ============================================================
    -- DATAS E HORÁRIOS
    -- ============================================================
    v.DataInicial,
    v.HoraInicio,
    v.DataFinal,
    v.HoraFim,

    -- Campo Start para FullCalendar (DATETIME combinando data + hora)
    CONVERT(
        DATETIME,
        CONVERT(VARCHAR(10), v.DataInicial, 120) + ' ' + CONVERT(VARCHAR(8), v.HoraInicio, 108),
        120
    ) AS [Start],

    -- Campo End para FullCalendar
    v.HoraFim AS [End],

    -- ============================================================
    -- STATUS E FLAGS
    -- ============================================================
    v.Status,
    v.StatusAgendamento,
    v.FoiAgendamento,
    v.Finalidade,

    -- ============================================================
    -- INFORMAÇÕES DO EVENTO
    -- ============================================================
    v.NomeEvento,
    e.Nome AS NomeEventoFull,

    -- ============================================================
    -- INFORMAÇÕES DE MOTORISTA E VEÍCULO
    -- ============================================================
    m.Nome AS NomeMotorista,
    vec.Placa,

    -- ============================================================
    -- TÍTULO DO EVENTO NA AGENDA
    -- ============================================================
    Titulo = CASE
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome
        ELSE v.Finalidade
    END,

    -- ============================================================
    -- COR DO EVENTO - Correspondente aos Headers dos Modais
    -- ============================================================
    -- Prioridade: 1. Evento, 2. Cancelada, 3. Realizada, 4. Agendada, 5. Aberta
    CorEvento = CASE
        -- EVENTO (Finalidade = 'Evento') - Bege claro #A39481 (20% mais claro)
        WHEN v.Finalidade = 'Evento'
            THEN '#A39481'

        -- CANCELADA - Vinho #722F37
        WHEN v.Status = 'Cancelada'
            THEN '#722F37'

        -- REALIZADA - Azul Petróleo #154c62
        WHEN v.Status = 'Realizada'
            THEN '#154c62'

        -- AGENDADA (Status = 'Agendada' OU Status = 'Aberta' com StatusAgendamento = 1) - Laranja #FFA726
        WHEN v.Status = 'Agendada'
            THEN '#FFA726'
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 1
            THEN '#FFA726'

        -- ABERTA (viagem já transformada) - Verde Militar #3d5c3d
        WHEN v.Status = 'Aberta' AND ISNULL(CAST(v.StatusAgendamento AS INT), 0) = 0
            THEN '#476b47'

        -- DEFAULT - Cinza
        ELSE '#6c757d'
    END,

    -- ============================================================
    -- COR DO TEXTO - Branco para todas (fundos escuros)
    -- ============================================================
    CorTexto = '#FFFFFF',

    -- ============================================================
    -- DESCRIÇÃO GENÉRICA (sem HTML)
    -- ============================================================
    DescricaoMontada =
        ISNULL(m.Nome, '(Motorista Não Informado)')
        + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
        + CASE
            WHEN v.DescricaoSemFormato IS NOT NULL
            THEN ' - ' +
                REPLACE(
                    REPLACE(
                        REPLACE(
                            CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                            CHAR(13), ''
                        ),
                        CHAR(10), ' '
                    ),
                    CHAR(9), ' '
                )
            ELSE ''
        END,

    -- ============================================================
    -- DESCRIÇÃO ESPECIAL PARA EVENTOS
    -- ============================================================
    DescricaoEvento = CASE
        -- Evento Cancelado
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL AND v.Status = 'Cancelada'
            THEN 'Evento CANCELADO: ' + e.Nome + ' / '
                + ISNULL(m.Nome, '(Motorista Não Identificado)')
                + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
                + CASE
                    WHEN v.DescricaoSemFormato IS NOT NULL
                    THEN ' - ' +
                        REPLACE(
                            REPLACE(
                                REPLACE(
                                    CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                                    CHAR(13), ''
                                ),
                                CHAR(10), ' '
                            ),
                            CHAR(9), ' '
                        )
                    ELSE ''
                END

        -- Evento Ativo
        WHEN v.Finalidade = 'Evento' AND e.Nome IS NOT NULL
            THEN 'Evento: ' + e.Nome + ' / '
                + ISNULL(m.Nome, '(Motorista Não Identificado)')
                + ' - (' + ISNULL(vec.Placa, 'Sem Veículo') + ')'
                + CASE
                    WHEN v.DescricaoSemFormato IS NOT NULL
                    THEN ' - ' +
                        REPLACE(
                            REPLACE(
                                REPLACE(
                                    CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                                    CHAR(13), ''
                                ),
                                CHAR(10), ' '
                            ),
                            CHAR(9), ' '
                        )
                    ELSE ''
                END

        -- Não é evento
        ELSE NULL
    END,

    -- ============================================================
    -- DESCRIÇÃO PURA (NOVO CAMPO - 18/01/2026)
    -- Apenas a descrição da viagem, sem motorista/placa
    -- Para uso em tooltips customizadas
    -- ============================================================
    Descricao = CASE
        WHEN v.DescricaoSemFormato IS NOT NULL
        THEN
            REPLACE(
                REPLACE(
                    REPLACE(
                        CAST(v.DescricaoSemFormato AS VARCHAR(MAX)),
                        CHAR(13), ''
                    ),
                    CHAR(10), ' '
                ),
                CHAR(9), ' '
            )
        ELSE NULL
    END

FROM dbo.Viagem v WITH (NOLOCK)
LEFT JOIN dbo.Motorista m WITH (NOLOCK) ON v.MotoristaId = m.MotoristaId
LEFT JOIN dbo.Veiculo vec WITH (NOLOCK) ON v.VeiculoId = vec.VeiculoId
LEFT JOIN dbo.Evento e WITH (NOLOCK) ON v.EventoId = e.EventoId

WHERE v.DataInicial IS NOT NULL
  AND v.HoraInicio IS NOT NULL
GO
-- View: ViewContratoFornecedor
PRINT 'Criando/atualizando view: ViewContratoFornecedor...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewContratoFornecedor')
    DROP VIEW [dbo].[ViewContratoFornecedor]
GO

CREATE OR ALTER VIEW dbo.ViewContratoFornecedor 
AS SELECT TOP 100000
  Contrato.ContratoId
 ,Contrato.AnoContrato + '/' + Contrato.NumeroContrato + ' - ' + Fornecedor.DescricaoFornecedor AS Descricao
 ,Contrato.TipoContrato
FROM dbo.Fornecedor
INNER JOIN dbo.Contrato
  ON Fornecedor.FornecedorId = Contrato.FornecedorId
ORDER BY Contrato.AnoContrato DESC
-- Create table [dbo].[RepactuacaoContrato]
--;
GO
-- View: ViewTeste
PRINT 'Criando/atualizando view: ViewTeste...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewTeste')
    DROP VIEW [dbo].[ViewTeste]
GO

CREATE OR ALTER VIEW dbo.ViewTeste 
AS SELECT
  Abastecimento.AbastecimentoId
 ,Abastecimento.Litros
 ,Abastecimento.ValorUnitario
 ,Abastecimento.DataHora
 ,Abastecimento.KmRodado
 ,Abastecimento.Hodometro
 ,Veiculo.Placa
 ,ModeloVeiculo.DescricaoModelo
 ,MarcaVeiculo.DescricaoMarca
 ,Combustivel.Descricao
 ,Motorista.Nome
 ,Unidade.Sigla
FROM dbo.Abastecimento
INNER JOIN dbo.Veiculo
  ON Abastecimento.VeiculoId = Veiculo.VeiculoId
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
INNER JOIN dbo.Combustivel
  ON Abastecimento.CombustivelId = Combustivel.CombustivelId
INNER JOIN dbo.Motorista
  ON Abastecimento.MotoristaId = Motorista.MotoristaId
LEFT OUTER JOIN dbo.Unidade
  ON Unidade.UnidadeId = Motorista.UnidadeId
-- Create or alter view [dbo].[ViewSabrina]
--;
GO
-- View: ViewContratoFornecedor_Veiculos
PRINT 'Criando/atualizando view: ViewContratoFornecedor_Veiculos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewContratoFornecedor_Veiculos')
    DROP VIEW [dbo].[ViewContratoFornecedor_Veiculos]
GO

CREATE OR ALTER VIEW dbo.ViewContratoFornecedor_Veiculos 
AS SELECT TOP 100000
  Contrato.ContratoId
 ,Contrato.AnoContrato + '/' + Contrato.NumeroContrato + ' - ' + Fornecedor.DescricaoFornecedor AS ContratoVeiculo
 ,Contrato.TipoContrato
FROM dbo.Fornecedor
INNER JOIN dbo.Contrato
  ON Fornecedor.FornecedorId = Contrato.FornecedorId
ORDER BY Contrato.AnoContrato DESC
-- Create or alter view [dbo].[ViewContratoFornecedor]
--;
GO
-- View: ViewVeiculos_Ultima
PRINT 'Criando/atualizando view: ViewVeiculos_Ultima...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewVeiculos_Ultima')
    DROP VIEW [dbo].[ViewVeiculos_Ultima]
GO

CREATE OR ALTER VIEW dbo.ViewVeiculos_Ultima 
AS SELECT
  Veiculo.VeiculoId
 ,Veiculo.Placa
 ,LEFT(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'), LEN(SUBSTRING(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'), CHARINDEX(' ', REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'), CHARINDEX(' ', REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.'))), LEN(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR, CAST(Veiculo.Quilometragem AS MONEY), 1), ',', '_'), '.', ','), '_', '.')))) - 2) AS Quilometragem
 ,MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS MarcaModelo
 ,Unidade.Sigla
 ,Combustivel.Descricao
 ,CASE
    WHEN CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2)) IS NULL THEN 0
    ELSE CAST(ROUND(AVG(Abastecimento.KmRodado / Abastecimento.Litros), 2) AS DEC(10, 2))
  END AS Consumo
 ,CASE
    WHEN ViewContratoFornecedor_Veiculos.ContratoVeiculo IS NOT NULL THEN ViewContratoFornecedor_Veiculos.ContratoVeiculo
    WHEN ViewAtaFornecedor.AtaVeiculo IS NOT NULL THEN ViewAtaFornecedor.AtaVeiculo + ' <b>(Ata)</b> '
    ELSE '<b>(Veículo Próprio)</b>'
  END AS OrigemVeiculo
 ,CONVERT(VARCHAR, Veiculo.DataAlteracao, 103) AS DataAlteracao
 ,AspNetUsers.NomeCompleto
 ,CASE
    WHEN Veiculo.Reserva = 0 THEN 'Efetivo'
    ELSE 'Reserva'
  END AS VeiculoReserva
 ,Veiculo.Status
 ,Veiculo.CombustivelId
 ,ROW_NUMBER() OVER (ORDER BY Veiculo.Placa) AS RowNum
 ,ViewContratoFornecedor_Veiculos.ContratoId
 ,ViewAtaFornecedor.AtaId
 ,ViewContratoFornecedor_Veiculos.ContratoVeiculo
 ,ViewAtaFornecedor.AtaVeiculo
 ,Veiculo.VeiculoProprio
 ,Veiculo.ItemVeiculoAtaId
 ,Veiculo.ItemVeiculoId
 ,Veiculo.ValorMensal
FROM dbo.Abastecimento
RIGHT OUTER JOIN dbo.Veiculo
  ON Abastecimento.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
LEFT OUTER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
LEFT OUTER JOIN dbo.Unidade
  ON Unidade.UnidadeId = Veiculo.UnidadeId
LEFT OUTER JOIN dbo.Combustivel
  ON Combustivel.CombustivelId = Veiculo.CombustivelId
LEFT OUTER JOIN dbo.ViewContratoFornecedor_Veiculos
  ON ViewContratoFornecedor_Veiculos.ContratoId = Veiculo.ContratoId
LEFT OUTER JOIN dbo.ViewAtaFornecedor
  ON ViewAtaFornecedor.AtaId = Veiculo.AtaId
INNER JOIN dbo.AspNetUsers
  ON AspNetUsers.Id = Veiculo.UsuarioIdAlteracao
GROUP BY Veiculo.VeiculoId
        ,Veiculo.Placa
        ,Veiculo.Quilometragem
        ,MarcaVeiculo.DescricaoMarca
        ,ModeloVeiculo.DescricaoModelo
        ,Unidade.Sigla
        ,Combustivel.Descricao
        ,ViewContratoFornecedor_Veiculos.ContratoId
        ,Veiculo.DataAlteracao
        ,AspNetUsers.NomeCompleto
        ,Veiculo.Reserva
        ,Veiculo.Status
        ,Veiculo.CombustivelId
        ,ViewAtaFornecedor.AtaId
        ,ViewContratoFornecedor_Veiculos.ContratoVeiculo
        ,ViewAtaFornecedor.AtaVeiculo
        ,Veiculo.VeiculoProprio
        ,Veiculo.ItemVeiculoAtaId
        ,Veiculo.ItemVeiculoId
        ,Veiculo.ValorMensal
-- Create or alter view [dbo].[ViewVeiculos_Original]
--;
GO
-- View: ViewManutencao_2025
PRINT 'Criando/atualizando view: ViewManutencao_2025...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewManutencao_2025')
    DROP VIEW [dbo].[ViewManutencao_2025]
GO

CREATE OR ALTER VIEW dbo.[ViewManutencao_2025.11.04] 
AS SELECT 
    -- Campo inicial solicitado
    ISNULL(v.Placa,'') + ' - ' + ISNULL(ma.DescricaoMarca,'') + '/' + ISNULL(m.DescricaoModelo,'') AS PlacaDescricao,

    -- ===== Chaves e referência ao contrato (para filtrar na LINQ) =====
    vc.ContratoId,

    -- ===== Manutenção =====
    b.ManutencaoId,
    b.NumOS,
    b.ResumoOS,

    -- strings formatadas (dd/MM/yyyy)
    CONVERT(CHAR(10), b.DataSolicitacao,     103) AS DataSolicitacao,
    CONVERT(CHAR(10), b.DataDisponibilidade, 103) AS DataDisponibilidade,
    CONVERT(CHAR(10), b.DataRecolhimento,    103) AS DataRecolhimento,
    CONVERT(CHAR(10), b.DataRecebimentoReserva, 103) AS DataRecebimentoReserva,
    CONVERT(CHAR(10), b.DataDevolucaoReserva,   103) AS DataDevolucaoReserva,
    CONVERT(CHAR(10), b.DataEntrega,         103) AS DataEntrega,
    CONVERT(CHAR(10), b.DataDevolucao,       103) AS DataDevolucao,

    -- datas cruas
    b.DataSolicitacao AS DataSolicitacaoRaw,
    b.DataDevolucao   AS DataDevolucaoRaw,

    b.StatusOS,
    b.VeiculoId,

    -- Derivado (Marca/Modelo) e informações auxiliares
    ma.DescricaoMarca + '/' + m.DescricaoModelo AS DescricaoVeiculo,
    u.Sigla                                    AS Sigla,
    c.Descricao                                AS CombustivelDescricao,
    v.Placa                                    AS Placa,

    -- Reserva em texto (compatibilidade)
    CASE
        WHEN b.ReservaEnviado = 1 THEN 'Enviado'
        WHEN b.ReservaEnviado = 0 THEN 'Ausente'
        ELSE ''
    END AS Reserva,

    -- Novo: ReservaEnviado (Sim/Não)
    CASE WHEN b.ReservaEnviado = 1 THEN 'Sim' ELSE 'Não' END AS ReservaEnviado,

    b.VeiculoReservaId,

    -- ===== Planilha de Glosa (Item do contrato, via Veiculo.ItemVeiculoId) =====
    ivc.Descricao,
    ivc.Quantidade,
    ivc.ValorUnitario,

    -- DiasGlosa (via APPLY)
    d.DiasCalc AS DiasGlosa,

    -- ValorGlosa (trata NULLs e usa DECIMAL p/ precisão)
    CAST(
        d.DiasCalc * COALESCE(CAST(ivc.ValorUnitario AS DECIMAL(18,2)), 0)
        AS DECIMAL(18,2)
    ) AS ValorGlosa,

    -- Dias (0 quando sem devolução, senão diff Solicitação->Devolução)
    CASE
        WHEN b.DataDevolucao IS NULL THEN 0
        ELSE DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END AS Dias,

    -- Atributos de UI
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS Habilitado,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'fa-regular fa-lock' ELSE 'far fa-flag-checkered' END                 AS Icon,

    -- Item do contrato
    ivc.NumItem,

    -- Campos "montados"
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'disabled' ELSE '' END                                              AS HabilitadoEditar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END           AS OpacityEditar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'Visualizar Manutenção' ELSE 'Edita a Ordem de Serviço!' END        AS OpacityTooltipEditarEditar,

    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'disabled' ELSE '' END                                              AS HabilitadoBaixar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN '' ELSE 'data-toggle=''modal'' data-target=''#modalManutencao''' END AS ModalBaixarAttrs,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END           AS OpacityBaixar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'Desabilitado' ELSE 'Fecha a Ordem de Serviço!' END                 AS Tooltip,

    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'disabled' ELSE '' END                                              AS HabilitadoCancelar,
    CASE WHEN b.StatusOS IN ('Fechada','Cancelada') THEN 'opacity:0.3; pointer-events:none;' ELSE 'opacity:1;' END           AS OpacityCancelar,
    CASE
        WHEN b.StatusOS = 'Cancelada' THEN 'Manutenção Cancelada'
        WHEN b.StatusOS = 'Fechada'   THEN 'OS Fechada/Baixada'
        ELSE 'Cancelar Manutenção'
    END AS TooltipCancelar,

    -- ========= Novos campos no formato solicitado =========
    -- Veiculo principal: "Placa - (Marca/Modelo)"
    ISNULL(v.Placa,'') + ' - (' + ISNULL(ma.DescricaoMarca,'') + '/' + ISNULL(m.DescricaoModelo,'') + ')' AS Veiculo,

    -- Carro de reserva (quando houver): "Placa - (Marca/Modelo)"
    CASE
        WHEN b.VeiculoReservaId IS NULL THEN NULL
        ELSE ISNULL(vr.Placa,'') + ' - (' + ISNULL(mar.DescricaoMarca,'') + '/' + ISNULL(mr.DescricaoModelo,'') + ')'
    END AS CarroReserva

FROM dbo.Manutencao               AS b
LEFT JOIN dbo.Veiculo             AS v   ON v.VeiculoId       = b.VeiculoId
LEFT JOIN dbo.VeiculoContrato     AS vc  ON vc.VeiculoId      = v.VeiculoId
LEFT JOIN dbo.ModeloVeiculo       AS m   ON m.ModeloId        = v.ModeloId
LEFT JOIN dbo.MarcaVeiculo        AS ma  ON ma.MarcaId        = v.MarcaId
LEFT JOIN dbo.Combustivel         AS c   ON c.CombustivelId   = v.CombustivelId
LEFT JOIN dbo.Unidade             AS u   ON u.UnidadeId       = v.UnidadeId
LEFT JOIN dbo.ItemVeiculoContrato AS ivc ON ivc.ItemVeiculoId = v.ItemVeiculoId

-- === Inclusão pedida: veículo de reserva ===
LEFT JOIN dbo.Veiculo             AS vr  ON vr.VeiculoId      = b.VeiculoReservaId
LEFT JOIN dbo.MarcaVeiculo        AS mar ON mar.MarcaId       = vr.MarcaId
LEFT JOIN dbo.ModeloVeiculo       AS mr  ON mr.ModeloId       = vr.ModeloId

CROSS APPLY (
  SELECT DiasCalc =
    CASE
      WHEN b.DataDevolucao IS NULL THEN 1
      WHEN b.DataEntrega  IS NOT NULL THEN DATEDIFF(DAY, b.DataEntrega,  b.DataDevolucao)
      ELSE                               DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao)
    END
) AS d

--WHERE
--    v.Status = 1
--    AND b.DataDevolucao IS NOT NULL
--    AND DATEDIFF(DAY, b.DataSolicitacao, b.DataDevolucao) > 0
-- Create or alter view [dbo].[ViewManutencao]
--;
GO
-- View: ViewOcorrencia
PRINT 'Criando/atualizando view: ViewOcorrencia...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewOcorrencia')
    DROP VIEW [dbo].[ViewOcorrencia]
GO

CREATE OR ALTER VIEW dbo.ViewOcorrencia 
WITH SCHEMABINDING
AS SELECT
  Viagem.VeiculoId
 ,Viagem.ViagemId
 ,Viagem.NoFichaVistoria
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
 ,Motorista.Nome AS NomeMotorista
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,Viagem.ResumoOcorrencia
 ,Viagem.StatusOcorrencia
 ,Viagem.MotoristaId
 ,Viagem.ImagemOcorrencia
 ,Viagem.ItemManutencaoId
 ,Viagem.DescricaoOcorrencia
 ,Viagem.DescricaoSolucaoOcorrencia
FROM dbo.Viagem
INNER JOIN dbo.Motorista
  ON Motorista.MotoristaId = Viagem.MotoristaId
INNER JOIN dbo.Veiculo
  ON Veiculo.VeiculoId = Viagem.VeiculoId
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
-- Create index [UK_ViewOcorrencia_ViagemId] on view [dbo].[ViewOcorrencia]
--;
GO
-- View: ViewEmpenhoMulta
PRINT 'Criando/atualizando view: ViewEmpenhoMulta...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewEmpenhoMulta')
    DROP VIEW [dbo].[ViewEmpenhoMulta]
GO

CREATE OR ALTER VIEW dbo.ViewEmpenhoMulta 
AS SELECT
  EmpenhoMulta.EmpenhoMultaId,
  EmpenhoMulta.NotaEmpenho,
  EmpenhoMulta.AnoVigencia,
  EmpenhoMulta.SaldoInicial,
  EmpenhoMulta.SaldoAtual,
  EmpenhoMulta.OrgaoAutuanteId,
  COUNT(DISTINCT MovimentacaoEmpenhoMulta.MovimentacaoId) AS Movimentacoes,
  SUM(DISTINCT MovimentacaoEmpenhoMulta.Valor) AS SaldoMovimentacao,
  SUM(DISTINCT Multa.ValorPago) AS SaldoMultas
FROM dbo.EmpenhoMulta
LEFT OUTER JOIN dbo.MovimentacaoEmpenhoMulta
  ON EmpenhoMulta.EmpenhoMultaId = MovimentacaoEmpenhoMulta.EmpenhoMultaId 
LEFT OUTER JOIN dbo.Multa
  ON EmpenhoMulta.EmpenhoMultaId = Multa.EmpenhoMultaId
GROUP BY EmpenhoMulta.EmpenhoMultaId, EmpenhoMulta.NotaEmpenho, EmpenhoMulta.AnoVigencia, EmpenhoMulta.SaldoInicial, EmpenhoMulta.SaldoAtual, EmpenhoMulta.OrgaoAutuanteId
-- Create or alter view [dbo].[ViewEdocMultas]
--;
GO
-- View: ViewRequisitantes
PRINT 'Criando/atualizando view: ViewRequisitantes...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewRequisitantes')
    DROP VIEW [dbo].[ViewRequisitantes]
GO

CREATE OR ALTER VIEW dbo.ViewRequisitantes 
AS SELECT
  Requisitante.RequisitanteId
 ,CASE WHEN Requisitante.Ponto IS NULL AND SetorSolicitante.Sigla IS NULL THEN Requisitante.Nome + '  - (' + SetorSolicitante.Nome + ')' 
       WHEN Requisitante.Ponto IS NOT NULL AND SetorSolicitante.Sigla IS NULL THEN Requisitante.Nome  + ' (' + Requisitante.Ponto + ')' + ' -  (' + SetorSolicitante.Nome + ')' 
       WHEN Requisitante.Ponto IS NOT NULL AND SetorSolicitante.Sigla IS NOT NULL THEN Requisitante.Nome  + ' (' + Requisitante.Ponto + ')' + ' -  (' + SetorSolicitante.Sigla + ')' 
  END AS Requisitante
 ,Requisitante.Status
 ,SetorSolicitante.Nome
 ,SetorSolicitante.Sigla
FROM dbo.Requisitante
INNER JOIN dbo.SetorSolicitante
  ON Requisitante.SetorSolicitanteId = SetorSolicitante.SetorSolicitanteId
WHERE Requisitante.Status = 1
-- Create table [dbo].[Evento]
--;
GO
-- View: ViewManutencaoAberta_Base
PRINT 'Criando/atualizando view: ViewManutencaoAberta_Base...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewManutencaoAberta_Base')
    DROP VIEW [dbo].[ViewManutencaoAberta_Base]
GO

CREATE OR ALTER VIEW dbo.ViewManutencaoAberta_Base 
AS SELECT
    m.ManutencaoId,
    m.NumOS,
    m.DataSolicitacao,
    m.ResumoOS,
    m.DataRecolhimento,
    m.DataRecebimentoReserva,
    m.DataDevolucaoReserva,
    m.DataDisponibilidade,
    m.DataEntrega,
    m.StatusOS,
    m.VeiculoId,
    v.ItemVeiculoId,  -- leva para a UI fazer LEFT JOIN
    -- Concatenação determinística para descrição:
    '(' + v.Placa + ') ' + ma.DescricaoMarca + ' / ' + mo.DescricaoModelo AS DescricaoVeiculo,
    CAST(ISNULL(m.ReservaEnviado, 0) AS bit) AS ReservaEnviado,
    m.DataDevolucao,
    -- Cálculo determinístico/preciso:
    DATEDIFF(DAY, ISNULL(m.DataEntrega, m.DataSolicitacao), ISNULL(m.DataDevolucao, m.DataSolicitacao)) AS DiasGlosa
FROM dbo.Manutencao      AS m
JOIN dbo.Veiculo         AS v  ON m.VeiculoId = v.VeiculoId
JOIN dbo.ModeloVeiculo   AS mo ON v.ModeloId  = mo.ModeloId       -- ajuste se o nome for diferente
JOIN dbo.MarcaVeiculo    AS ma ON mo.MarcaId  = ma.MarcaId
WHERE m.StatusOS = 'Aberta'
-- Create or alter view [dbo].[ViewManutencao.ATUAL]
--;
GO
-- View: ViewMotoristaFluxo
PRINT 'Criando/atualizando view: ViewMotoristaFluxo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristaFluxo')
    DROP VIEW [dbo].[ViewMotoristaFluxo]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristaFluxo 
AS SELECT DISTINCT
  convert(nvarchar(50), ViewFluxoEconomildo.MotoristaId) AS MotoristaId,
  ViewFluxoEconomildo.NomeMotorista
FROM dbo.ViewFluxoEconomildo
-- Create table [dbo].[RankingMotoristasMensal]
--;
GO
-- View: ViewEdocMultas
PRINT 'Criando/atualizando view: ViewEdocMultas...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewEdocMultas')
    DROP VIEW [dbo].[ViewEdocMultas]
GO

CREATE OR ALTER VIEW dbo.ViewEdocMultas 
AS SELECT
  Multa.MultaId
 ,Multa.NumInfracao
 ,CONVERT(VARCHAR, Multa.Data, 103) AS Data
 ,FORMAT(Multa.Hora, 'HH:mm') AS Hora
 ,Motorista.Nome
 ,Motorista.MotoristaId
 ,CASE
    WHEN Motorista.Celular02 IS NULL THEN Motorista.Celular01
    ELSE Motorista.Celular01 + ' / ' + Motorista.Celular02
  END AS Telefone
 ,Veiculo.Placa
 ,Veiculo.VeiculoId
 ,OrgaoAutuante.Sigla
 ,OrgaoAutuante.OrgaoAutuanteId
 ,Multa.Localizacao
 ,TipoMulta.Artigo
 ,CONVERT(VARCHAR, Multa.Vencimento, 103) AS Vencimento
 ,Multa.ValorAteVencimento
 ,Multa.ValorPosVencimento
 ,Multa.ProcessoEDoc
 ,Multa.Status
 ,Multa.Fase
 ,TipoMulta.Descricao
 ,Multa.Observacao
 ,Multa.Paga
 ,Motorista.Ponto
 ,Contrato.AnoContrato + '/' + Contrato.NumeroContrato AS NumContrato
 ,CASE
    WHEN Multa.Paga = -1 THEN 'Pago'
  END AS Pago
 ,Fornecedor.DescricaoFornecedor
FROM dbo.Multa
LEFT OUTER JOIN dbo.Motorista
  ON Multa.MotoristaId = Motorista.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
  ON Multa.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.OrgaoAutuante
  ON Multa.OrgaoAutuanteId = OrgaoAutuante.OrgaoAutuanteId
LEFT OUTER JOIN dbo.TipoMulta
  ON Multa.TipoMultaId = TipoMulta.TipoMultaId
LEFT OUTER JOIN dbo.EmpenhoMulta
  ON Multa.EmpenhoMultaId = EmpenhoMulta.EmpenhoMultaId
    AND EmpenhoMulta.OrgaoAutuanteId = OrgaoAutuante.OrgaoAutuanteId
LEFT OUTER JOIN dbo.Contrato
  ON Multa.ContratoMotoristaId = Contrato.ContratoId
LEFT OUTER JOIN dbo.Fornecedor
  ON Contrato.FornecedorId = Fornecedor.FornecedorId
-- Create table [dbo].[Abastecimento]
--;
GO
-- View: ViewCustosMotoristas
PRINT 'Criando/atualizando view: ViewCustosMotoristas...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustosMotoristas')
    DROP VIEW [dbo].[ViewCustosMotoristas]
GO

CREATE OR ALTER VIEW dbo.ViewCustosMotoristas 
AS SELECT
  Motorista.Nome
 ,FORMAT(SUM(Viagem.CustoCombustivel), 'N2', 'pt-BR') AS Combustivel
 ,FORMAT(SUM(Viagem.CustoMotorista), 'N2', 'pt-BR') AS Motorista
 ,FORMAT(SUM(Viagem.CustoVeiculo), 'N2', 'pt-BR') AS Veiculo
 ,FORMAT(SUM(Viagem.KmFinal - Viagem.KmInicial), 'N2', 'pt-BR') AS Quilometragem
FROM dbo.Viagem
INNER JOIN dbo.Motorista
  ON Viagem.MotoristaId = Motorista.MotoristaId
WHERE Viagem.Status = 'Realizada'
AND Viagem.Finalidade NOT LIKE 'Manutenção'
AND Viagem.Finalidade NOT LIKE 'Devolução à Locadora'
AND Motorista.TipoCondutor = 'Terceirizado'
AND MONTH(Viagem.DataInicial) = 05
GROUP BY Motorista.Nome
-- Create or alter view [dbo].[ViewCustosFinalidade]
--;
GO
-- View: ViewViagens_Sergio
PRINT 'Criando/atualizando view: ViewViagens_Sergio...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewViagens_Sergio')
    DROP VIEW [dbo].[ViewViagens_Sergio]
GO

CREATE OR ALTER VIEW dbo.ViewViagens_Sergio 
AS SELECT
  Viagem.ViagemId
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
 ,FORMAT(Viagem.HoraInicio, 'HH:mm') AS HoraInicio
 ,Viagem.Descricao
 ,Viagem.KmInicial
 ,Viagem.KmFinal
 ,Viagem.CombustivelInicial
 ,Viagem.CombustivelFinal
 ,Viagem.NoFichaVistoria
 ,Viagem.Finalidade
,CASE
    WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
    ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
  END AS NomeRequisitante
 ,CASE
    WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
    ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
  END AS NomeSetor
 ,Motorista.Nome AS NomeMotorista
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,Veiculo.CombustivelId
 ,Viagem.FichaVistoria
 ,Viagem.Origem
 ,Viagem.Destino
 ,Viagem.Minutos
 ,Viagem.NomeEvento
FROM dbo.SetorSolicitante
RIGHT OUTER JOIN dbo.Viagem
  ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
LEFT OUTER JOIN dbo.Requisitante
  ON Requisitante.RequisitanteId = Viagem.RequisitanteId
LEFT OUTER JOIN dbo.Motorista
  ON Motorista.MotoristaId = Viagem.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
  ON Veiculo.VeiculoId = Viagem.VeiculoId
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
-- Create or alter view [dbo].[ViewViagens]
--;
GO
-- View: ViewFichaViagem
PRINT 'Criando/atualizando view: ViewFichaViagem...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewFichaViagem')
    DROP VIEW [dbo].[ViewFichaViagem]
GO

CREATE OR ALTER VIEW dbo.ViewFichaViagem 
AS SELECT
  CONVERT(NVARCHAR(100), Viagem.ViagemId) AS ViagemId
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS DataInicial
 ,CONVERT(VARCHAR, Viagem.DataFinal, 103) AS DataFinal
 ,FORMAT(Viagem.HoraInicio, 'HH:mm') AS HoraInicio
 ,FORMAT(Viagem.HoraFim, 'HH:mm') AS HoraFim
 ,Viagem.DescricaoSemFormato
 ,Viagem.KmInicial
 ,Viagem.KmFinal
 ,Viagem.ResumoOcorrencia
 ,Viagem.DescricaoOcorrencia
 ,Viagem.StatusOcorrencia
 ,Viagem.NoFichaVistoria
 ,Viagem.Finalidade
 ,Viagem.Origem
 ,Viagem.Destino
 ,Viagem.Status
 ,Viagem.EventoId
 ,Viagem.StatusAgendamento
 ,CASE
    WHEN Requisitante.Ramal IS NULL THEN Requisitante.Nome
    ELSE Requisitante.Nome + ' - (' + CONVERT(VARCHAR, Requisitante.Ramal) + ')'
  END AS NomeRequisitante
 ,CASE
    WHEN SetorSolicitante.Sigla IS NULL THEN SetorSolicitante.Nome
    ELSE SetorSolicitante.Nome + ' - (' + SetorSolicitante.Sigla + ')'
  END AS NomeSetor
 ,Motorista.Nome AS NomeMotorista
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,Veiculo.UnidadeId
 ,Evento.Nome AS NomeEvento
 ,Viagem.FichaVistoria
 ,Viagem.Minutos
 ,Viagem.Descricao
 ,Viagem.CombustivelInicial
 ,Viagem.CombustivelFinal
 ,Viagem.RamalRequisitante
FROM dbo.SetorSolicitante
LEFT OUTER JOIN dbo.Viagem
  ON SetorSolicitante.SetorSolicitanteId = Viagem.SetorSolicitanteId
LEFT OUTER JOIN dbo.Requisitante
  ON Requisitante.RequisitanteId = Viagem.RequisitanteId
LEFT OUTER JOIN dbo.Motorista
  ON Motorista.MotoristaId = Viagem.MotoristaId
LEFT OUTER JOIN dbo.Veiculo
  ON Veiculo.VeiculoId = Viagem.VeiculoId
LEFT OUTER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
LEFT OUTER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
LEFT OUTER JOIN dbo.Evento
  ON Viagem.EventoId = Evento.EventoId
-- Create or alter view [dbo].[ViewEventos]
--;
GO
-- View: ViewPendenciasManutencao
PRINT 'Criando/atualizando view: ViewPendenciasManutencao...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewPendenciasManutencao')
    DROP VIEW [dbo].[ViewPendenciasManutencao]
GO

CREATE OR ALTER VIEW dbo.ViewPendenciasManutencao 
AS SELECT
  ItensManutencao.ItemManutencaoId
 ,ItensManutencao.ManutencaoId
 ,ItensManutencao.TipoItem
 ,ItensManutencao.NumFicha
 ,CONVERT(VARCHAR, ItensManutencao.DataItem, 103) AS DataItem
 ,ItensManutencao.Resumo
 ,ItensManutencao.Descricao
 ,ItensManutencao.Status
 ,ItensManutencao.MotoristaId
 ,ItensManutencao.ViagemId
 ,ItensManutencao.ImagemOcorrencia
 ,Motorista.Nome
 ,Manutencao.VeiculoId
FROM dbo.ItensManutencao
LEFT OUTER JOIN dbo.Motorista
  ON ItensManutencao.MotoristaId = Motorista.MotoristaId
LEFT OUTER JOIN dbo.Viagem
  ON ItensManutencao.ViagemId = Viagem.ViagemId
INNER JOIN dbo.Manutencao
  ON ItensManutencao.ManutencaoId = Manutencao.ManutencaoId
-- Create or alter view [dbo].[ViewOcorrencia]
--;
GO
-- View: ViewMotoristasViagem_Ultimo
PRINT 'Criando/atualizando view: ViewMotoristasViagem_Ultimo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristasViagem_Ultimo')
    DROP VIEW [dbo].[ViewMotoristasViagem_Ultimo]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristasViagem_Ultimo 
AS SELECT TOP 10000
    m.MotoristaId,
    m.Nome,
    m.TipoCondutor,
    m.Status,
    m.Foto,
    CAST(m.Nome + ' (' + m.TipoCondutor + ')' AS NVARCHAR(300)) AS MotoristaCondutor
FROM dbo.Motorista AS m
WHERE m.Status = 1
ORDER BY m.Nome
-- Create or alter view [dbo].[ViewMotoristasViagem]
--;
GO
-- View: ViewOcorrenciasAbertasVeiculo
PRINT 'Criando/atualizando view: ViewOcorrenciasAbertasVeiculo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewOcorrenciasAbertasVeiculo')
    DROP VIEW [dbo].[ViewOcorrenciasAbertasVeiculo]
GO

CREATE OR ALTER VIEW dbo.ViewOcorrenciasAbertasVeiculo
AS
SELECT 
    oc.OcorrenciaViagemId,
    oc.ViagemId,
    oc.VeiculoId,
    oc.MotoristaId,
    oc.Resumo,
    oc.Descricao,
    oc.ImagemOcorrencia,
    oc.DataCriacao,
    oc.UsuarioCriacao,
    
    -- Dados do Veículo
    ve.Placa,
    ma.DescricaoMarca,
    mo.DescricaoModelo,
    CONCAT(ve.Placa, ' - ', ma.DescricaoMarca, '/', mo.DescricaoModelo) AS VeiculoCompleto,
    
    -- Dados da Viagem de Origem
    vi.DataInicial AS DataViagem,
    vi.NoFichaVistoria,
    
    -- Dados do Motorista que registrou
    mt.Nome AS NomeMotorista,
    
    -- Campos de Urgência
    DATEDIFF(DAY, oc.DataCriacao, GETDATE()) AS DiasEmAberto,
    
    CASE 
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN 'Crítica'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN 'Alta'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN 'Média'
        ELSE 'Normal'
    END AS Urgencia,
    
    CASE 
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 30 THEN '#dc3545'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 15 THEN '#ffc107'
        WHEN DATEDIFF(DAY, oc.DataCriacao, GETDATE()) > 7 THEN '#17a2b8'
        ELSE '#6c757d'
    END AS CorUrgencia

FROM dbo.OcorrenciaViagem oc
    INNER JOIN dbo.Viagem vi ON oc.ViagemId = vi.ViagemId
    INNER JOIN dbo.Veiculo ve ON oc.VeiculoId = ve.VeiculoId
    INNER JOIN dbo.MarcaVeiculo ma ON ve.MarcaId = ma.MarcaId
    INNER JOIN dbo.ModeloVeiculo mo ON ve.ModeloId = mo.ModeloId
    LEFT JOIN dbo.Motorista mt ON oc.MotoristaId = mt.MotoristaId

WHERE oc.Status = 'Aberta'
-- Create table [dbo].[AlertasFrotiX]
--;
GO
-- View: ViewLavagem
PRINT 'Criando/atualizando view: ViewLavagem...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewLavagem')
    DROP VIEW [dbo].[ViewLavagem]
GO

CREATE OR ALTER VIEW dbo.ViewLavagem 
AS SELECT
  Lavagem.LavagemId
  ,Lavagem.VeiculoId
  ,Lavagem.MotoristaId
 ,CONVERT(VARCHAR, Lavagem.Data, 103) AS Data
 ,CONVERT(VARCHAR, Lavagem.HorarioInicio, 8) AS Horario
 ,CONVERT(VARCHAR, Lavagem.HorarioInicio, 8) AS HorarioInicio
 ,CONVERT(VARCHAR, Lavagem.HorarioFim, 8) AS HorarioFim
 ,DATEDIFF(MINUTE, Lavagem.HorarioInicio, Lavagem.HorarioFim) AS DuracaoMinutos
 ,STRING_AGG(Lavador.Nome, ',') AS Lavadores
 ,STRING_AGG(convert(nvarchar(50),Lavador.LavadorId), ',') AS LavadoresId
 ,'(' + Veiculo.Placa + ') - ' + MarcaVeiculo.DescricaoMarca + '/' + ModeloVeiculo.DescricaoModelo AS DescricaoVeiculo
 ,Motorista.Nome
FROM dbo.LavadoresLavagem
LEFT OUTER JOIN dbo.Lavador
  ON LavadoresLavagem.LavadorId = Lavador.LavadorId
RIGHT OUTER JOIN dbo.Lavagem
  ON LavadoresLavagem.LavagemId = Lavagem.LavagemId
LEFT OUTER JOIN dbo.Veiculo
  ON Lavagem.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
LEFT OUTER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
LEFT OUTER JOIN dbo.Motorista
  ON Lavagem.MotoristaId = Motorista.MotoristaId
GROUP BY Lavagem.LavagemId
        ,Lavagem.VeiculoId
        ,Lavagem.MotoristaId
        ,Lavagem.Data
        ,Lavagem.HorarioInicio
        ,HorarioFim
        ,Veiculo.Placa
        ,MarcaVeiculo.DescricaoMarca
        ,ModeloVeiculo.DescricaoModelo
        ,Motorista.Nome
-- Create table [dbo].[EstatisticaMotoristasMensal]
--;
GO
-- View: ViewAlertasAtivos
PRINT 'Criando/atualizando view: ViewAlertasAtivos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAlertasAtivos')
    DROP VIEW [dbo].[ViewAlertasAtivos]
GO

CREATE OR ALTER VIEW dbo.ViewAlertasAtivos 
AS SELECT 
    a.AlertasFrotiXId,
    a.Titulo,
    a.Descricao,
    a.TipoAlerta,
    CASE a.TipoAlerta
        WHEN 1 THEN 'Agendamento'
        WHEN 2 THEN 'Manutenção'
        WHEN 3 THEN 'Motorista'
        WHEN 4 THEN 'Veículo'
        WHEN 5 THEN 'Anúncio'
        ELSE 'Diversos'
    END AS TipoAlertaTexto,
    a.Prioridade,
    CASE a.Prioridade
        WHEN 1 THEN 'Baixa'
        WHEN 2 THEN 'Média'
        WHEN 3 THEN 'Alta'
    END AS PrioridadeTexto,
    a.DataInsercao,
    a.DataExibicao,
    a.DataExpiracao,
    a.ViagemId,
    a.ManutencaoId,
    a.MotoristaId,
    a.VeiculoId,
    a.TipoExibicao,
    a.HorarioExibicao,
    a.UsuarioCriadorId,
    a.Ativo,
    au.UsuarioId,
    au.Lido,
    au.DataLeitura,
    au.Notificado,
    u.UserName,
    u.Email
FROM AlertasFrotiX a
INNER JOIN AlertasUsuario au ON a.AlertasFrotiXId = au.AlertasFrotiXId
INNER JOIN AspNetUsers u ON au.UsuarioId = u.Id
WHERE a.Ativo = 1
-- Create user [frotix]
--;
GO
-- View: ViewCustosVeiculos
PRINT 'Criando/atualizando view: ViewCustosVeiculos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustosVeiculos')
    DROP VIEW [dbo].[ViewCustosVeiculos]
GO

CREATE OR ALTER VIEW dbo.ViewCustosVeiculos 
AS SELECT
  ItemVeiculoContrato.Descricao
 ,FORMAT(SUM(Viagem.CustoCombustivel), 'N2', 'pt-BR') AS Combustivel
 ,FORMAT(SUM(Viagem.CustoMotorista), 'N2', 'pt-BR') AS Motorista
 ,FORMAT(SUM(Viagem.CustoVeiculo), 'N2', 'pt-BR') AS Veiculo
 ,FORMAT(SUM(Viagem.KmFinal - Viagem.KmInicial), 'N2', 'pt-BR') AS Quilometragem
FROM dbo.Viagem
LEFT OUTER JOIN dbo.Veiculo
  ON Viagem.VeiculoId = Veiculo.VeiculoId
LEFT OUTER JOIN dbo.ItemVeiculoContrato
  ON Veiculo.ItemVeiculoId = ItemVeiculoContrato.ItemVeiculoId
WHERE Viagem.Status = 'Realizada'
AND Viagem.Finalidade NOT LIKE 'Manutenção'
AND Viagem.Finalidade NOT LIKE 'Devolução à Locadora'
AND MONTH(Viagem.DataInicial) = 12
GROUP BY ItemVeiculoContrato.Descricao
-- Create or alter view [dbo].[ViewCustosTemporaria]
--;
GO
-- View: ViewCustosTemporaria
PRINT 'Criando/atualizando view: ViewCustosTemporaria...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewCustosTemporaria')
    DROP VIEW [dbo].[ViewCustosTemporaria]
GO

CREATE OR ALTER VIEW dbo.ViewCustosTemporaria 
AS SELECT
 Viagem.NoFichaVistoria
 ,SetorSolicitante.Sigla
 ,SetorSolicitante.Nome
 ,CONVERT(VARCHAR, Viagem.DataInicial, 103) AS 'Data Inicial'
 ,CONVERT(VARCHAR, Viagem.DataFinal, 103) AS 'Data Final'
 ,FORMAT((Viagem.HoraFim - Viagem.HoraInicio), 'HH:mm') AS Duração
 ,(Viagem.KmFinal - Viagem.KmInicial) AS Quilometragem
 ,CONVERT(DECIMAL(10,2),Viagem.CustoCombustivel) AS Combustivel
 ,CONVERT(DECIMAL(10,2),Viagem.CustoMotorista) AS Motorista
 ,Viagem.Finalidade 
FROM dbo.Viagem
INNER JOIN dbo.SetorSolicitante
  ON Viagem.SetorSolicitanteId = SetorSolicitante.SetorSolicitanteId
WHERE SetorSolicitante.Sigla = 'CTRAN' AND Viagem.Finalidade NOT LIKE 'Manutenção' AND Viagem.Finalidade NOT LIKE 'Devolução à Locadora'
-- Create or alter view [dbo].[ViewCustosMotoristas_Individual]
--;
GO
-- View: ViewMotoristas_Ultimo
PRINT 'Criando/atualizando view: ViewMotoristas_Ultimo...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewMotoristas_Ultimo')
    DROP VIEW [dbo].[ViewMotoristas_Ultimo]
GO

CREATE OR ALTER VIEW dbo.ViewMotoristas_Ultimo 
AS SELECT TOP 10000
  Motorista.MotoristaId
 ,Motorista.Nome
 ,Motorista.Ponto
 ,Motorista.CNH
 ,Motorista.CategoriaCNH
 ,Motorista.Celular01
 ,Motorista.Status
 ,Unidade.Sigla
 ,Contrato.AnoContrato
 ,Contrato.NumeroContrato
 ,Fornecedor.DescricaoFornecedor
 ,AspNetUsers.NomeCompleto
 ,Motorista.DataAlteracao
 ,Motorista.ContratoId
 ,Motorista.TipoCondutor 
 ,Motorista.Foto
 ,Motorista.EfetivoFerista   
,Motorista.Nome + ' (' + Motorista.TipoCondutor + ')' AS MotoristaCondutor
 ,ROW_NUMBER() OVER (ORDER BY Motorista.Nome) AS RowNum
FROM dbo.Motorista
LEFT OUTER JOIN dbo.Contrato
  ON Motorista.ContratoId = Contrato.ContratoId
LEFT OUTER JOIN dbo.Fornecedor
  ON Contrato.FornecedorId = Fornecedor.FornecedorId
INNER JOIN dbo.AspNetUsers
  ON Motorista.UsuarioIdAlteracao = AspNetUsers.Id
LEFT OUTER JOIN dbo.Unidade
  ON Motorista.UnidadeId = Unidade.UnidadeId
ORDER BY Motorista.Nome
-- Create or alter view [dbo].[ViewMotoristas_original]
--;
GO
-- View: ViewViagensAgendaTodosMeses
PRINT 'Criando/atualizando view: ViewViagensAgendaTodosMeses...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewViagensAgendaTodosMeses')
    DROP VIEW [dbo].[ViewViagensAgendaTodosMeses]
GO

CREATE OR ALTER VIEW dbo.ViewViagensAgendaTodosMeses 
AS SELECT
  Viagem.DataInicial
 ,Viagem.HoraInicio
 ,Viagem.MotoristaId
 ,Viagem.VeiculoId
 ,Viagem.Descricao
 ,Viagem.Status
 ,Viagem.ViagemId
 ,Viagem.Finalidade
 ,Viagem.StatusAgendamento
 ,Viagem.NomeEvento
FROM dbo.Viagem
-- Create or alter view [dbo].[ViewViagensAgenda]
--;
GO
-- View: ViewAbastecimentos
PRINT 'Criando/atualizando view: ViewAbastecimentos...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'ViewAbastecimentos')
    DROP VIEW [dbo].[ViewAbastecimentos]
GO

CREATE OR ALTER VIEW dbo.ViewAbastecimentos 
AS SELECT TOP (100) PERCENT
  Abastecimento.AbastecimentoId
 ,Abastecimento.DataHora
 ,CONVERT(VARCHAR, Abastecimento.DataHora, 103) AS Data
 ,FORMAT(Abastecimento.DataHora, 'HH:mm') AS Hora
 ,Veiculo.Placa
 ,MarcaVeiculo.DescricaoMarca + ' / ' + ModeloVeiculo.DescricaoModelo AS TipoVeiculo
 ,Motorista.Nome
 ,Combustivel.Descricao AS TipoCombustivel
 ,Unidade.Sigla
 ,FORMAT(Abastecimento.Litros, 'N2') AS Litros
 ,FORMAT(Abastecimento.ValorUnitario, 'N2') AS ValorUnitario
 ,FORMAT(Abastecimento.Litros * Abastecimento.ValorUnitario, 'N2') AS ValorTotal
 ,FORMAT(Abastecimento.KmRodado / Abastecimento.Litros, 'N2') AS Consumo
 ,ViewMediaConsumo.ConsumoGeral
 ,Abastecimento.KmRodado
 ,Veiculo.VeiculoId
 ,Combustivel.CombustivelId
 ,Unidade.UnidadeId
 ,Motorista.MotoristaId
  ,ROW_NUMBER() OVER (ORDER BY Abastecimento.DataHora DESC) AS RowNum
 ,Motorista.Nome + ' (' + Motorista.TipoCondutor + ')' AS MotoristaCondutor
FROM dbo.Abastecimento
INNER JOIN dbo.Veiculo
  ON Abastecimento.VeiculoId = Veiculo.VeiculoId
INNER JOIN dbo.ModeloVeiculo
  ON Veiculo.ModeloId = ModeloVeiculo.ModeloId
INNER JOIN dbo.MarcaVeiculo
  ON ModeloVeiculo.MarcaId = MarcaVeiculo.MarcaId
INNER JOIN dbo.Motorista
  ON Abastecimento.MotoristaId = Motorista.MotoristaId
INNER JOIN dbo.Combustivel
  ON Abastecimento.CombustivelId = Combustivel.CombustivelId
INNER JOIN dbo.Unidade
  ON Motorista.UnidadeId = Unidade.UnidadeId
LEFT OUTER JOIN dbo.ViewMediaConsumo
  ON ViewMediaConsumo.VeiculoId = Abastecimento.VeiculoId
LEFT OUTER JOIN dbo.CondutorApoio
  ON CondutorApoio.CondutorId = Motorista.CondutorId
-- Create or alter view [dbo].[ViewCustoAbastecimento]
--;
GO
-- ======================================================================================
-- SE??O 4: CRIAR/ATUALIZAR PROCEDURES (21 procedures)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 4: CRIAR/ATUALIZAR PROCEDURES';
PRINT '======================================================================================';
PRINT '';
GO
-- Procedure: DocGenerator
PRINT 'Criando/atualizando procedure: DocGenerator...';
GO

CREATE OR ALTER PROCEDURE DocGenerator.sp_DetectFileChanges
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
-- Procedure: sp_AtualizarEstatisticasAbastecimentosMesAtual
PRINT 'Criando/atualizando procedure: sp_AtualizarEstatisticasAbastecimentosMesAtual...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_AtualizarEstatisticasAbastecimentosMesAtual
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ano INT = YEAR(GETDATE());
    DECLARE @Mes INT = MONTH(GETDATE());

    -- Recalcula mês atual
    EXEC sp_RecalcularEstatisticasAbastecimentos @Ano, @Mes;
    EXEC sp_RecalcularEstatisticasAbastecimentosAnuais @Ano;

    -- Recalcula também mês anterior
    IF @Mes = 1
    BEGIN
        SET @Ano = @Ano - 1;
        SET @Mes = 12;
    END
    ELSE
        SET @Mes = @Mes - 1;

    EXEC sp_RecalcularEstatisticasAbastecimentos @Ano, @Mes;

    PRINT 'Estatísticas de abastecimentos do mês atual e anterior atualizadas!';
END
GO
-- Procedure: sp_AtualizarEstatisticasMesAtual
PRINT 'Criando/atualizando procedure: sp_AtualizarEstatisticasMesAtual...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_AtualizarEstatisticasMesAtual
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ano INT = YEAR(GETDATE());
    DECLARE @Mes INT = MONTH(GETDATE());

    -- Recalcula mês atual
    EXEC sp_RecalcularEstatisticasMotoristas @Ano, @Mes;

    -- Recalcula também mês anterior
    IF @Mes = 1
    BEGIN
        SET @Ano = @Ano - 1;
        SET @Mes = 12;
    END
    ELSE
        SET @Mes = @Mes - 1;

    EXEC sp_RecalcularEstatisticasMotoristas @Ano, @Mes;

    PRINT 'Estatísticas do mês atual e anterior atualizadas!';
END
GO
-- Procedure: sp_AtualizarEstatisticasVeiculosMesAtual
PRINT 'Criando/atualizando procedure: sp_AtualizarEstatisticasVeiculosMesAtual...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_AtualizarEstatisticasVeiculosMesAtual
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ano INT = YEAR(GETDATE());
    DECLARE @Mes INT = MONTH(GETDATE());

    -- Atualiza snapshot da frota
    EXEC sp_RecalcularEstatisticasVeiculoGeral;
    EXEC sp_RecalcularEstatisticasVeiculoCategoria;
    EXEC sp_RecalcularEstatisticasVeiculoStatus;
    EXEC sp_RecalcularEstatisticasVeiculoModelo;
    EXEC sp_RecalcularEstatisticasVeiculoCombustivel;
    EXEC sp_RecalcularEstatisticasVeiculoUnidade;
    EXEC sp_RecalcularEstatisticasVeiculoAnoFabricacao;

    -- Atualiza mês atual
    EXEC sp_RecalcularEstatisticasVeiculoUsoMensal @Ano, @Mes;
    EXEC sp_RecalcularRankingsVeiculoAnual @Ano;

    -- Atualiza mês anterior
    IF @Mes = 1
    BEGIN
        SET @Ano = @Ano - 1;
        SET @Mes = 12;
    END
    ELSE
        SET @Mes = @Mes - 1;

    EXEC sp_RecalcularEstatisticasVeiculoUsoMensal @Ano, @Mes;

    PRINT 'Estatísticas de veículos do mês atual e anterior atualizadas!';
END
GO
-- Procedure: sp_RecalcularEstatisticasAbastecimentos
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasAbastecimentos...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasAbastecimentos
    @Ano INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DataInicio DATE = DATEFROMPARTS(@Ano, @Mes, 1);
    DECLARE @DataFim DATE = EOMONTH(@DataInicio);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- =====================================================
        -- 9.1 Estatísticas Gerais do Mês
        -- =====================================================

        DELETE FROM EstatisticaAbastecimentoMensal WHERE Ano = @Ano AND Mes = @Mes;

        INSERT INTO EstatisticaAbastecimentoMensal (Ano, Mes, TotalAbastecimentos, ValorTotal, LitrosTotal, DataAtualizacao)
        SELECT
            @Ano,
            @Mes,
            COUNT(*),
            SUM(ISNULL(Litros, 0) * ISNULL(ValorUnitario, 0)),
            SUM(ISNULL(Litros, 0)),
            GETDATE()
        FROM Abastecimento
        WHERE DataHora >= @DataInicio AND DataHora < DATEADD(DAY, 1, @DataFim);

        -- =====================================================
        -- 9.2 Estatísticas por Combustível
        -- =====================================================

        DELETE FROM EstatisticaAbastecimentoCombustivel WHERE Ano = @Ano AND Mes = @Mes;

        INSERT INTO EstatisticaAbastecimentoCombustivel (Ano, Mes, TipoCombustivel, TotalAbastecimentos, ValorTotal, LitrosTotal, MediaValorLitro, DataAtualizacao)
        SELECT
            @Ano,
            @Mes,
            ISNULL(c.Descricao, 'Não Informado'),
            COUNT(*),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            SUM(ISNULL(a.Litros, 0)),
            AVG(ISNULL(a.ValorUnitario, 0)),
            GETDATE()
        FROM Abastecimento a
        LEFT JOIN Combustivel c ON a.CombustivelId = c.CombustivelId
        WHERE a.DataHora >= @DataInicio AND a.DataHora < DATEADD(DAY, 1, @DataFim)
        GROUP BY c.Descricao;

        -- =====================================================
        -- 9.3 Estatísticas por Categoria de Veículo
        -- =====================================================

        DELETE FROM EstatisticaAbastecimentoCategoria WHERE Ano = @Ano AND Mes = @Mes;

        INSERT INTO EstatisticaAbastecimentoCategoria (Ano, Mes, Categoria, TotalAbastecimentos, ValorTotal, LitrosTotal, DataAtualizacao)
        SELECT
            @Ano,
            @Mes,
            ISNULL(v.Categoria, 'Sem Categoria'),
            COUNT(*),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            SUM(ISNULL(a.Litros, 0)),
            GETDATE()
        FROM Abastecimento a
        LEFT JOIN Veiculo v ON a.VeiculoId = v.VeiculoId
        WHERE a.DataHora >= @DataInicio AND a.DataHora < DATEADD(DAY, 1, @DataFim)
        GROUP BY v.Categoria;

        -- =====================================================
        -- 9.4 Estatísticas por Tipo/Modelo de Veículo
        -- =====================================================

        DELETE FROM EstatisticaAbastecimentoTipoVeiculo WHERE Ano = @Ano AND Mes = @Mes;

        INSERT INTO EstatisticaAbastecimentoTipoVeiculo (Ano, Mes, TipoVeiculo, TotalAbastecimentos, ValorTotal, LitrosTotal, DataAtualizacao)
        SELECT
            @Ano,
            @Mes,
            ISNULL(m.DescricaoModelo, 'Não Informado'),
            COUNT(*),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            SUM(ISNULL(a.Litros, 0)),
            GETDATE()
        FROM Abastecimento a
        LEFT JOIN Veiculo v ON a.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE a.DataHora >= @DataInicio AND a.DataHora < DATEADD(DAY, 1, @DataFim)
        GROUP BY m.DescricaoModelo;

        -- =====================================================
        -- 9.5 Estatísticas por Veículo Mensal
        -- =====================================================

        DELETE FROM EstatisticaAbastecimentoVeiculoMensal WHERE Ano = @Ano AND Mes = @Mes;

        INSERT INTO EstatisticaAbastecimentoVeiculoMensal (Ano, Mes, VeiculoId, TotalAbastecimentos, ValorTotal, LitrosTotal, DataAtualizacao)
        SELECT
            @Ano,
            @Mes,
            a.VeiculoId,
            COUNT(*),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            SUM(ISNULL(a.Litros, 0)),
            GETDATE()
        FROM Abastecimento a
        WHERE a.DataHora >= @DataInicio AND a.DataHora < DATEADD(DAY, 1, @DataFim)
          AND a.VeiculoId IS NOT NULL AND a.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY a.VeiculoId;

        -- =====================================================
        -- 9.6 Heatmap Geral (todos os veículos)
        -- =====================================================

        DELETE FROM HeatmapAbastecimentoMensal
        WHERE Ano = @Ano AND Mes = @Mes AND VeiculoId IS NULL AND TipoVeiculo IS NULL;

        INSERT INTO HeatmapAbastecimentoMensal (Ano, Mes, VeiculoId, TipoVeiculo, DiaSemana, Hora, TotalAbastecimentos, ValorTotal, DataAtualizacao)
        SELECT
            @Ano, @Mes, NULL, NULL,
            DATEPART(WEEKDAY, DataHora) - 1, -- 0=Domingo
            DATEPART(HOUR, DataHora),
            COUNT(*),
            SUM(ISNULL(Litros, 0) * ISNULL(ValorUnitario, 0)),
            GETDATE()
        FROM Abastecimento
        WHERE DataHora >= @DataInicio AND DataHora < DATEADD(DAY, 1, @DataFim)
        GROUP BY DATEPART(WEEKDAY, DataHora), DATEPART(HOUR, DataHora);

        COMMIT TRANSACTION;

        PRINT 'Estatísticas de abastecimentos do mês ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + ' recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasAbastecimentosAnuais
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasAbastecimentosAnuais...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasAbastecimentosAnuais
    @Ano INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Recalcula estatísticas anuais por veículo (para ranking)
        DELETE FROM EstatisticaAbastecimentoVeiculo WHERE Ano = @Ano;

        INSERT INTO EstatisticaAbastecimentoVeiculo (Ano, VeiculoId, Placa, TipoVeiculo, Categoria, TotalAbastecimentos, ValorTotal, LitrosTotal, DataAtualizacao)
        SELECT
            @Ano,
            a.VeiculoId,
            v.Placa,
            m.DescricaoModelo,
            v.Categoria,
            COUNT(*),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            SUM(ISNULL(a.Litros, 0)),
            GETDATE()
        FROM Abastecimento a
        INNER JOIN Veiculo v ON a.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE YEAR(a.DataHora) = @Ano
          AND a.VeiculoId IS NOT NULL AND a.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY a.VeiculoId, v.Placa, m.DescricaoModelo, v.Categoria;

        -- Atualiza lista de anos disponíveis
        DELETE FROM AnosDisponiveisAbastecimento WHERE Ano = @Ano;

        INSERT INTO AnosDisponiveisAbastecimento (Ano, TotalAbastecimentos, DataAtualizacao)
        SELECT @Ano, COUNT(*), GETDATE()
        FROM Abastecimento
        WHERE YEAR(DataHora) = @Ano;

        COMMIT TRANSACTION;

        PRINT 'Estatísticas anuais de abastecimentos do ano ' + CAST(@Ano AS VARCHAR) + ' recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasMotoristas
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasMotoristas...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasMotoristas
      @Ano INT,
      @Mes INT
  AS
  BEGIN
      SET NOCOUNT ON;

      DECLARE @DataInicio DATE = DATEFROMPARTS(@Ano, @Mes, 1);
      DECLARE @DataFim DATE = EOMONTH(@DataInicio);
      DECLARE @Hoje DATE = GETDATE();

      BEGIN TRY
          BEGIN TRANSACTION;

          -- Limpa dados existentes do mês
          DELETE FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes;

          -- Insere estatísticas de viagens por motorista
          INSERT INTO EstatisticaMotoristasMensal (MotoristaId, Ano, Mes, TotalViagens, KmTotal, MinutosTotais, DataAtualizacao)
          SELECT
              v.MotoristaId,
              @Ano,
              @Mes,
              COUNT(*),
              SUM(CASE
                  WHEN v.KmInicial IS NOT NULL AND v.KmFinal IS NOT NULL
                       AND v.KmFinal >= v.KmInicial
                       AND (v.KmFinal - v.KmInicial) <= 2000
                  THEN v.KmFinal - v.KmInicial
                  ELSE 0
              END),
              SUM(ISNULL(v.Minutos, 0)),
              GETDATE()
          FROM Viagem v
          WHERE v.MotoristaId IS NOT NULL
            AND v.DataInicial >= @DataInicio
            AND v.DataInicial < DATEADD(DAY, 1, @DataFim)
          GROUP BY v.MotoristaId;

          -- Atualiza multas por motorista
          UPDATE e
          SET e.TotalMultas = ISNULL(m.Total, 0),
              e.ValorTotalMultas = ISNULL(m.Valor, 0),
              e.DataAtualizacao = GETDATE()
          FROM EstatisticaMotoristasMensal e
          LEFT JOIN (
              SELECT MotoristaId, COUNT(*) as Total, SUM(ISNULL(ValorAteVencimento, 0)) as Valor
              FROM Multa
              WHERE Data >= @DataInicio AND Data < DATEADD(DAY, 1, @DataFim)
                AND MotoristaId IS NOT NULL
              GROUP BY MotoristaId
          ) m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes;

          -- Insere motoristas que só têm multas (sem viagens)
          INSERT INTO EstatisticaMotoristasMensal (MotoristaId, Ano, Mes, TotalMultas, ValorTotalMultas, DataAtualizacao)
          SELECT
              m.MotoristaId,
              @Ano,
              @Mes,
              COUNT(*),
              SUM(ISNULL(m.ValorAteVencimento, 0)),
              GETDATE()
          FROM Multa m
          WHERE m.MotoristaId IS NOT NULL
            AND m.Data >= @DataInicio
            AND m.Data < DATEADD(DAY, 1, @DataFim)
            AND NOT EXISTS (
                SELECT 1 FROM EstatisticaMotoristasMensal e
                WHERE e.MotoristaId = m.MotoristaId AND e.Ano = @Ano AND e.Mes = @Mes
            )
          GROUP BY m.MotoristaId;

          -- Atualiza abastecimentos por motorista
          UPDATE e
          SET e.TotalAbastecimentos = ISNULL(a.Total, 0),
              e.LitrosTotais = ISNULL(a.Litros, 0),
              e.ValorTotalAbastecimentos = ISNULL(a.Valor, 0),
              e.DataAtualizacao = GETDATE()
          FROM EstatisticaMotoristasMensal e
          LEFT JOIN (
              SELECT MotoristaId, COUNT(*) as Total,
                     SUM(ISNULL(Litros, 0)) as Litros,
                     SUM(ISNULL(Litros, 0) * ISNULL(ValorUnitario, 0)) as Valor
              FROM Abastecimento
              WHERE DataHora >= @DataInicio AND DataHora < DATEADD(DAY, 1, @DataFim)
                AND MotoristaId IS NOT NULL AND MotoristaId <> '00000000-0000-0000-0000-000000000000'
              GROUP BY MotoristaId
          ) a ON e.MotoristaId = a.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes;

          -- Estatísticas Gerais do Mês
          DELETE FROM EstatisticaGeralMensal WHERE Ano = @Ano AND Mes = @Mes;

          INSERT INTO EstatisticaGeralMensal (
              Ano, Mes,
              TotalMotoristas, MotoristasAtivos, MotoristasInativos,
              Efetivos, Feristas, Cobertura,
              TotalViagens, KmTotal, HorasTotais,
              TotalMultas, ValorTotalMultas,
              TotalAbastecimentos,
              DataAtualizacao
          )
          SELECT
              @Ano,
              @Mes,
              (SELECT COUNT(*) FROM Motorista),
              (SELECT COUNT(*) FROM Motorista WHERE Status = 1),
              (SELECT COUNT(*) FROM Motorista WHERE Status = 0),
              (SELECT COUNT(*) FROM Motorista WHERE Status = 1 AND (EfetivoFerista = 'Efetivo' OR EfetivoFerista IS NULL OR EfetivoFerista = '')),    
              (SELECT COUNT(*) FROM Motorista WHERE Status = 1 AND EfetivoFerista = 'Ferista'),
              (SELECT COUNT(*) FROM Motorista WHERE Status = 1 AND EfetivoFerista = 'Cobertura'),
              ISNULL((SELECT SUM(TotalViagens) FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes), 0),
              ISNULL((SELECT SUM(KmTotal) FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes), 0),
              ISNULL((SELECT SUM(MinutosTotais) / 60.0 FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes), 0),
              ISNULL((SELECT SUM(TotalMultas) FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes), 0),
              ISNULL((SELECT SUM(ValorTotalMultas) FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes), 0),
              ISNULL((SELECT SUM(TotalAbastecimentos) FROM EstatisticaMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes), 0),
              GETDATE();

          -- Rankings Top 10
          DELETE FROM RankingMotoristasMensal WHERE Ano = @Ano AND Mes = @Mes;

          -- Top 10 por Viagens
          INSERT INTO RankingMotoristasMensal (Ano, Mes, TipoRanking, Posicao, MotoristaId, NomeMotorista, TipoMotorista, ValorPrincipal, DataAtualizacao)
          SELECT TOP 10
              @Ano, @Mes, 'VIAGENS', ROW_NUMBER() OVER (ORDER BY e.TotalViagens DESC),
              e.MotoristaId, m.Nome, ISNULL(m.EfetivoFerista, 'Efetivo'), e.TotalViagens, GETDATE()
          FROM EstatisticaMotoristasMensal e
          INNER JOIN Motorista m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes AND e.TotalViagens > 0
          ORDER BY e.TotalViagens DESC;

          -- Top 10 por KM
          INSERT INTO RankingMotoristasMensal (Ano, Mes, TipoRanking, Posicao, MotoristaId, NomeMotorista, TipoMotorista, ValorPrincipal, DataAtualizacao)
          SELECT TOP 10
              @Ano, @Mes, 'KM', ROW_NUMBER() OVER (ORDER BY e.KmTotal DESC),
              e.MotoristaId, m.Nome, ISNULL(m.EfetivoFerista, 'Efetivo'), e.KmTotal, GETDATE()
          FROM EstatisticaMotoristasMensal e
          INNER JOIN Motorista m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes AND e.KmTotal > 0
          ORDER BY e.KmTotal DESC;

          -- Top 10 por Horas
          INSERT INTO RankingMotoristasMensal (Ano, Mes, TipoRanking, Posicao, MotoristaId, NomeMotorista, TipoMotorista, ValorPrincipal, DataAtualizacao)
          SELECT TOP 10
              @Ano, @Mes, 'HORAS', ROW_NUMBER() OVER (ORDER BY e.MinutosTotais DESC),
              e.MotoristaId, m.Nome, ISNULL(m.EfetivoFerista, 'Efetivo'), ROUND(e.MinutosTotais / 60.0, 1), GETDATE()
          FROM EstatisticaMotoristasMensal e
          INNER JOIN Motorista m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes AND e.MinutosTotais > 0
          ORDER BY e.MinutosTotais DESC;

          -- Top 10 por Abastecimentos
          INSERT INTO RankingMotoristasMensal (Ano, Mes, TipoRanking, Posicao, MotoristaId, NomeMotorista, TipoMotorista, ValorPrincipal, DataAtualizacao)
          SELECT TOP 10
              @Ano, @Mes, 'ABASTECIMENTOS', ROW_NUMBER() OVER (ORDER BY e.TotalAbastecimentos DESC),
              e.MotoristaId, m.Nome, ISNULL(m.EfetivoFerista, 'Efetivo'), e.TotalAbastecimentos, GETDATE()
          FROM EstatisticaMotoristasMensal e
          INNER JOIN Motorista m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes AND e.TotalAbastecimentos > 0
          ORDER BY e.TotalAbastecimentos DESC;

          -- Top 10 por Multas
          INSERT INTO RankingMotoristasMensal (Ano, Mes, TipoRanking, Posicao, MotoristaId, NomeMotorista, TipoMotorista, ValorPrincipal, ValorSecundario, DataAtualizacao)
          SELECT TOP 10
              @Ano, @Mes, 'MULTAS', ROW_NUMBER() OVER (ORDER BY e.TotalMultas DESC),
              e.MotoristaId, m.Nome, ISNULL(m.EfetivoFerista, 'Efetivo'), e.TotalMultas, e.ValorTotalMultas, GETDATE()
          FROM EstatisticaMotoristasMensal e
          INNER JOIN Motorista m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes AND e.TotalMultas > 0
          ORDER BY e.TotalMultas DESC;

          -- Top 10 Performance
          INSERT INTO RankingMotoristasMensal (Ano, Mes, TipoRanking, Posicao, MotoristaId, NomeMotorista, TipoMotorista, ValorPrincipal, ValorSecundario, ValorTerciario, ValorQuaternario, DataAtualizacao)
          SELECT TOP 10
              @Ano, @Mes, 'PERFORMANCE', ROW_NUMBER() OVER (ORDER BY e.TotalViagens DESC),
              e.MotoristaId, m.Nome, ISNULL(m.EfetivoFerista, 'Efetivo'),
              e.TotalViagens, e.KmTotal, ROUND(e.MinutosTotais / 60.0, 1), e.TotalMultas, GETDATE()
          FROM EstatisticaMotoristasMensal e
          INNER JOIN Motorista m ON e.MotoristaId = m.MotoristaId
          WHERE e.Ano = @Ano AND e.Mes = @Mes AND e.TotalViagens > 0
          ORDER BY e.TotalViagens DESC;

          -- =====================================================
          -- HEATMAP - CORRIGIDO: Usar HoraInicio para extrair a hora
          -- =====================================================
          DELETE FROM HeatmapViagensMensal WHERE Ano = @Ano AND Mes = @Mes AND MotoristaId IS NULL;

          INSERT INTO HeatmapViagensMensal (Ano, Mes, MotoristaId, DiaSemana, Hora, TotalViagens, DataAtualizacao)
          SELECT
              @Ano, @Mes, NULL,
              DATEPART(WEEKDAY, v.HoraInicio) - 1,  -- CORRIGIDO: HoraInicio
              DATEPART(HOUR, v.HoraInicio),          -- CORRIGIDO: HoraInicio
              COUNT(*),
              GETDATE()
          FROM Viagem v
          WHERE v.MotoristaId IS NOT NULL
            AND v.DataInicial >= @DataInicio
            AND v.DataInicial < DATEADD(DAY, 1, @DataFim)
            AND v.HoraInicio IS NOT NULL
          GROUP BY DATEPART(WEEKDAY, v.HoraInicio), DATEPART(HOUR, v.HoraInicio);

          -- Evolução Diária de Viagens (Todos os motoristas)
          DELETE FROM EvolucaoViagensDiaria
          WHERE Data >= @DataInicio AND Data <= @DataFim AND MotoristaId IS NULL;

          INSERT INTO EvolucaoViagensDiaria (Data, MotoristaId, TotalViagens, KmTotal, MinutosTotais, DataAtualizacao)
          SELECT
              CAST(v.DataInicial AS DATE),
              NULL,
              COUNT(*),
              SUM(CASE
                  WHEN v.KmInicial IS NOT NULL AND v.KmFinal IS NOT NULL
                       AND v.KmFinal >= v.KmInicial
                       AND (v.KmFinal - v.KmInicial) <= 2000
                  THEN v.KmFinal - v.KmInicial
                  ELSE 0
              END),
              SUM(ISNULL(v.Minutos, 0)),
              GETDATE()
          FROM Viagem v
          WHERE v.MotoristaId IS NOT NULL
            AND v.DataInicial >= @DataInicio
            AND v.DataInicial < DATEADD(DAY, 1, @DataFim)
          GROUP BY CAST(v.DataInicial AS DATE);

          COMMIT TRANSACTION;
          PRINT 'Estatísticas do mês ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + ' recalculadas com sucesso.';

      END TRY
      BEGIN CATCH
          ROLLBACK TRANSACTION;
          THROW;
      END CATCH
  END
GO
-- Procedure: sp_RecalcularEstatisticasMotoristaUnico
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasMotoristaUnico...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasMotoristaUnico
      @MotoristaId UNIQUEIDENTIFIER,
      @Ano INT,
      @Mes INT
  AS
  BEGIN
      SET NOCOUNT ON;

      DECLARE @DataInicio DATE = DATEFROMPARTS(@Ano, @Mes, 1);
      DECLARE @DataFim DATE = EOMONTH(@DataInicio);

      BEGIN TRY
          DELETE FROM EstatisticaMotoristasMensal
          WHERE MotoristaId = @MotoristaId AND Ano = @Ano AND Mes = @Mes;

          INSERT INTO EstatisticaMotoristasMensal (MotoristaId, Ano, Mes, TotalViagens, KmTotal, MinutosTotais, DataAtualizacao)
          SELECT
              @MotoristaId,
              @Ano,
              @Mes,
              COUNT(*),
              SUM(CASE
                  WHEN v.KmInicial IS NOT NULL AND v.KmFinal IS NOT NULL
                       AND v.KmFinal >= v.KmInicial
                       AND (v.KmFinal - v.KmInicial) <= 2000
                  THEN v.KmFinal - v.KmInicial
                  ELSE 0
              END),
              SUM(ISNULL(v.Minutos, 0)),
              GETDATE()
          FROM Viagem v
          WHERE v.MotoristaId = @MotoristaId
            AND v.DataInicial >= @DataInicio
            AND v.DataInicial < DATEADD(DAY, 1, @DataFim);

          IF @@ROWCOUNT = 0
          BEGIN
              INSERT INTO EstatisticaMotoristasMensal (MotoristaId, Ano, Mes, DataAtualizacao)
              VALUES (@MotoristaId, @Ano, @Mes, GETDATE());
          END

          UPDATE e
          SET e.TotalMultas = ISNULL(m.Total, 0),
              e.ValorTotalMultas = ISNULL(m.Valor, 0),
              e.DataAtualizacao = GETDATE()
          FROM EstatisticaMotoristasMensal e
          LEFT JOIN (
              SELECT COUNT(*) as Total, SUM(ISNULL(ValorAteVencimento, 0)) as Valor
              FROM Multa
              WHERE MotoristaId = @MotoristaId
                AND Data >= @DataInicio AND Data < DATEADD(DAY, 1, @DataFim)
          ) m ON 1=1
          WHERE e.MotoristaId = @MotoristaId AND e.Ano = @Ano AND e.Mes = @Mes;

          UPDATE e
          SET e.TotalAbastecimentos = ISNULL(a.Total, 0),
              e.LitrosTotais = ISNULL(a.Litros, 0),
              e.ValorTotalAbastecimentos = ISNULL(a.Valor, 0),
              e.DataAtualizacao = GETDATE()
          FROM EstatisticaMotoristasMensal e
          LEFT JOIN (
              SELECT COUNT(*) as Total,
                     SUM(ISNULL(Litros, 0)) as Litros,
                     SUM(ISNULL(Litros, 0) * ISNULL(ValorUnitario, 0)) as Valor
              FROM Abastecimento
              WHERE MotoristaId = @MotoristaId
                AND DataHora >= @DataInicio AND DataHora < DATEADD(DAY, 1, @DataFim)
          ) a ON 1=1
          WHERE e.MotoristaId = @MotoristaId AND e.Ano = @Ano AND e.Mes = @Mes;

          -- HEATMAP - CORRIGIDO: Usar HoraInicio
          DELETE FROM HeatmapViagensMensal
          WHERE Ano = @Ano AND Mes = @Mes AND MotoristaId = @MotoristaId;

          INSERT INTO HeatmapViagensMensal (Ano, Mes, MotoristaId, DiaSemana, Hora, TotalViagens, DataAtualizacao)
          SELECT
              @Ano, @Mes, @MotoristaId,
              DATEPART(WEEKDAY, v.HoraInicio) - 1,  -- CORRIGIDO: HoraInicio
              DATEPART(HOUR, v.HoraInicio),          -- CORRIGIDO: HoraInicio
              COUNT(*),
              GETDATE()
          FROM Viagem v
          WHERE v.MotoristaId = @MotoristaId
            AND v.DataInicial >= @DataInicio
            AND v.DataInicial < DATEADD(DAY, 1, @DataFim)
            AND v.HoraInicio IS NOT NULL
          GROUP BY DATEPART(WEEKDAY, v.HoraInicio), DATEPART(HOUR, v.HoraInicio);

          DELETE FROM EvolucaoViagensDiaria
          WHERE Data >= @DataInicio AND Data <= @DataFim AND MotoristaId = @MotoristaId;

          INSERT INTO EvolucaoViagensDiaria (Data, MotoristaId, TotalViagens, KmTotal, MinutosTotais, DataAtualizacao)
          SELECT
              CAST(v.DataInicial AS DATE),
              @MotoristaId,
              COUNT(*),
              SUM(CASE
                  WHEN v.KmInicial IS NOT NULL AND v.KmFinal IS NOT NULL
                       AND v.KmFinal >= v.KmInicial
                       AND (v.KmFinal - v.KmInicial) <= 2000
                  THEN v.KmFinal - v.KmInicial
                  ELSE 0
              END),
              SUM(ISNULL(v.Minutos, 0)),
              GETDATE()
          FROM Viagem v
          WHERE v.MotoristaId = @MotoristaId
            AND v.DataInicial >= @DataInicio
            AND v.DataInicial < DATEADD(DAY, 1, @DataFim)
          GROUP BY CAST(v.DataInicial AS DATE);

      END TRY
      BEGIN CATCH
          THROW;
      END CATCH
  END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoAnoFabricacao
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoAnoFabricacao...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoAnoFabricacao
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoAnoFabricacao;

        INSERT INTO EstatisticaVeiculoAnoFabricacao (AnoFabricacao, TotalVeiculos, DataAtualizacao)
        SELECT
            ISNULL(AnoFabricacao, 0),
            COUNT(*),
            GETDATE()
        FROM Veiculo
        WHERE AnoFabricacao IS NOT NULL AND AnoFabricacao > 0
        GROUP BY AnoFabricacao;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por ano de fabricação recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoCategoria
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoCategoria...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoCategoria
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoCategoria;

        INSERT INTO EstatisticaVeiculoCategoria (
            Categoria, TotalVeiculos, VeiculosAtivos,
            VeiculosProprios, VeiculosLocados, DataAtualizacao
        )
        SELECT
            ISNULL(Categoria, 'Sem Categoria'),
            COUNT(*),
            SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END),
            SUM(CASE WHEN VeiculoProprio = 1 THEN 1 ELSE 0 END),
            SUM(CASE WHEN VeiculoProprio = 0 THEN 1 ELSE 0 END),
            GETDATE()
        FROM Veiculo
        GROUP BY Categoria;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por categoria recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoCombustivel
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoCombustivel...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoCombustivel
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoCombustivel;

        INSERT INTO EstatisticaVeiculoCombustivel (Combustivel, TotalVeiculos, DataAtualizacao)
        SELECT
            ISNULL(c.Descricao, 'Não Informado'),
            COUNT(*),
            GETDATE()
        FROM Veiculo v
        LEFT JOIN Combustivel c ON v.CombustivelId = c.CombustivelId
        GROUP BY c.Descricao; -- Já está correto, agrupa apenas pela descrição

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por combustível recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoGeral
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoGeral...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoGeral
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Limpa tabela
        DELETE FROM EstatisticaVeiculoGeral;

        -- Insere estatísticas gerais
        INSERT INTO EstatisticaVeiculoGeral (
            TotalVeiculos, VeiculosAtivos, VeiculosInativos,
            VeiculosProprios, VeiculosLocados, IdadeMediaAnos,
            KmMedioRodado, ValorMensalLocacao, DataAtualizacao
        )
        SELECT
            COUNT(*),
            SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END),
            SUM(CASE WHEN Status = 0 THEN 1 ELSE 0 END),
            SUM(CASE WHEN VeiculoProprio = 1 THEN 1 ELSE 0 END),
            SUM(CASE WHEN VeiculoProprio = 0 THEN 1 ELSE 0 END),
            ISNULL(AVG(CAST(YEAR(GETDATE()) - AnoFabricacao AS DECIMAL(10,2))), 0),
            ISNULL(AVG(CAST(Quilometragem AS DECIMAL(18,2))), 0),
            ISNULL(SUM(CASE WHEN VeiculoProprio = 0 THEN ISNULL(ValorMensal, 0) ELSE 0 END), 0),
            GETDATE()
        FROM Veiculo;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas gerais de veículos recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoModelo
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoModelo...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoModelo
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoModelo;

        INSERT INTO EstatisticaVeiculoModelo (
            ModeloId, Modelo, TotalVeiculos, VeiculosAtivos, DataAtualizacao
        )
        SELECT
            MIN(v.ModeloId), -- Pega o primeiro ModeloId encontrado para cada nome de modelo
            ISNULL(m.DescricaoModelo, 'Não Informado'),
            COUNT(*),
            SUM(CASE WHEN v.Status = 1 THEN 1 ELSE 0 END),
            GETDATE()
        FROM Veiculo v
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        GROUP BY m.DescricaoModelo; -- Agrupa apenas pelo nome do modelo

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por modelo recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoStatus
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoStatus...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoStatus
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoStatus;

        INSERT INTO EstatisticaVeiculoStatus (Status, TotalVeiculos, DataAtualizacao)
        SELECT
            CASE WHEN Status = 1 THEN 'Ativo' ELSE 'Inativo' END,
            COUNT(*),
            GETDATE()
        FROM Veiculo
        GROUP BY Status;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por status recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoUnidade
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoUnidade...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoUnidade
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoUnidade;

        INSERT INTO EstatisticaVeiculoUnidade (
            UnidadeId, Unidade, TotalVeiculos, VeiculosAtivos, DataAtualizacao
        )
        SELECT
            MIN(v.UnidadeId), -- Pega o primeiro UnidadeId encontrado para cada sigla
            ISNULL(u.Sigla, 'Sem Unidade'),
            COUNT(*),
            SUM(CASE WHEN v.Status = 1 THEN 1 ELSE 0 END),
            GETDATE()
        FROM Veiculo v
        LEFT JOIN Unidade u ON v.UnidadeId = u.UnidadeId
        GROUP BY u.Sigla; -- Agrupa apenas pela sigla da unidade

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por unidade recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularEstatisticasVeiculoUsoMensal
PRINT 'Criando/atualizando procedure: sp_RecalcularEstatisticasVeiculoUsoMensal...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoUsoMensal
    @Ano INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @DataInicio DATE = DATEFROMPARTS(@Ano, @Mes, 1);
        DECLARE @DataFim DATE = EOMONTH(@DataInicio);

        DELETE FROM EstatisticaVeiculoUsoMensal WHERE Ano = @Ano AND Mes = @Mes;

        -- Viagens do mês
        DECLARE @TotalViagens INT = 0;
        DECLARE @KmTotal DECIMAL(18,2) = 0;

        SELECT
            @TotalViagens = COUNT(*),
            @KmTotal = ISNULL(SUM(ISNULL(KmFinal, 0) - ISNULL(KmInicial, 0)), 0)
        FROM Viagem
        WHERE DataInicial >= @DataInicio AND DataInicial < DATEADD(DAY, 1, @DataFim);

        -- Abastecimentos do mês
        DECLARE @TotalAbastecimentos INT = 0;
        DECLARE @LitrosTotal DECIMAL(18,2) = 0;
        DECLARE @ValorAbastecimento DECIMAL(18,2) = 0;

        SELECT
            @TotalAbastecimentos = COUNT(*),
            @LitrosTotal = ISNULL(SUM(ISNULL(Litros, 0)), 0),
            @ValorAbastecimento = ISNULL(SUM(ISNULL(Litros, 0) * ISNULL(ValorUnitario, 0)), 0)
        FROM Abastecimento
        WHERE DataHora >= @DataInicio AND DataHora < DATEADD(DAY, 1, @DataFim);

        -- Consumo médio (km/l)
        DECLARE @ConsumoMedio DECIMAL(10,2) = 0;
        IF @LitrosTotal > 0
            SET @ConsumoMedio = @KmTotal / @LitrosTotal;

        INSERT INTO EstatisticaVeiculoUsoMensal (
            Ano, Mes, TotalViagens, KmTotalRodado,
            TotalAbastecimentos, LitrosTotal, ValorAbastecimento,
            ConsumoMedio, DataAtualizacao
        )
        VALUES (
            @Ano, @Mes, @TotalViagens, @KmTotal,
            @TotalAbastecimentos, @LitrosTotal, @ValorAbastecimento,
            @ConsumoMedio, GETDATE()
        );

        COMMIT TRANSACTION;
        PRINT 'Estatísticas de uso do mês ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + ' recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularRankingsVeiculoAnual
PRINT 'Criando/atualizando procedure: sp_RecalcularRankingsVeiculoAnual...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularRankingsVeiculoAnual
    @Ano INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- =====================================================
        -- Ranking por KM Rodado
        -- =====================================================
        DELETE FROM EstatisticaVeiculoRankingKm WHERE Ano = @Ano;

        INSERT INTO EstatisticaVeiculoRankingKm (
            Ano, VeiculoId, Placa, Modelo, KmRodado, TotalViagens, DataAtualizacao
        )
        SELECT
            @Ano,
            vi.VeiculoId,
            v.Placa,
            m.DescricaoModelo,
            SUM(ISNULL(vi.KmFinal, 0) - ISNULL(vi.KmInicial, 0)),
            COUNT(*),
            GETDATE()
        FROM Viagem vi
        INNER JOIN Veiculo v ON vi.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE YEAR(vi.DataInicial) = @Ano
          AND vi.VeiculoId IS NOT NULL AND vi.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY vi.VeiculoId, v.Placa, m.DescricaoModelo;

        -- =====================================================
        -- Ranking por Litros Abastecidos
        -- =====================================================
        DELETE FROM EstatisticaVeiculoRankingLitros WHERE Ano = @Ano;

        INSERT INTO EstatisticaVeiculoRankingLitros (
            Ano, VeiculoId, Placa, Modelo, LitrosAbastecidos, ValorTotal, TotalAbastecimentos, DataAtualizacao
        )
        SELECT
            @Ano,
            a.VeiculoId,
            v.Placa,
            m.DescricaoModelo,
            SUM(ISNULL(a.Litros, 0)),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            COUNT(*),
            GETDATE()
        FROM Abastecimento a
        INNER JOIN Veiculo v ON a.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE YEAR(a.DataHora) = @Ano
          AND a.VeiculoId IS NOT NULL AND a.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY a.VeiculoId, v.Placa, m.DescricaoModelo;

        -- =====================================================
        -- Ranking por Consumo (km/l)
        -- =====================================================
        DELETE FROM EstatisticaVeiculoRankingConsumo WHERE Ano = @Ano;

        -- Primeiro, obter KM por veículo
        WITH KmPorVeiculo AS (
            SELECT
                VeiculoId,
                SUM(ISNULL(KmFinal, 0) - ISNULL(KmInicial, 0)) AS KmRodado
            FROM Viagem
            WHERE YEAR(DataInicial) = @Ano
              AND VeiculoId IS NOT NULL AND VeiculoId <> '00000000-0000-0000-0000-000000000000'
            GROUP BY VeiculoId
        ),
        LitrosPorVeiculo AS (
            SELECT
                VeiculoId,
                SUM(ISNULL(Litros, 0)) AS LitrosAbastecidos,
                COUNT(*) AS TotalAbastecimentos
            FROM Abastecimento
            WHERE YEAR(DataHora) = @Ano
              AND VeiculoId IS NOT NULL AND VeiculoId <> '00000000-0000-0000-0000-000000000000'
            GROUP BY VeiculoId
        )
        INSERT INTO EstatisticaVeiculoRankingConsumo (
            Ano, VeiculoId, Placa, Modelo, KmRodado, LitrosAbastecidos,
            ConsumoKmPorLitro, TotalAbastecimentos, DataAtualizacao
        )
        SELECT
            @Ano,
            COALESCE(k.VeiculoId, l.VeiculoId),
            v.Placa,
            m.DescricaoModelo,
            ISNULL(k.KmRodado, 0),
            ISNULL(l.LitrosAbastecidos, 0),
            CASE WHEN ISNULL(l.LitrosAbastecidos, 0) > 0
                 THEN ROUND(ISNULL(k.KmRodado, 0) / l.LitrosAbastecidos, 2)
                 ELSE 0 END,
            ISNULL(l.TotalAbastecimentos, 0),
            GETDATE()
        FROM KmPorVeiculo k
        FULL OUTER JOIN LitrosPorVeiculo l ON k.VeiculoId = l.VeiculoId
        INNER JOIN Veiculo v ON COALESCE(k.VeiculoId, l.VeiculoId) = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE ISNULL(k.KmRodado, 0) > 0 OR ISNULL(l.LitrosAbastecidos, 0) > 0;

        -- =====================================================
        -- Atualizar Anos Disponíveis
        -- =====================================================
        DELETE FROM AnosDisponiveisVeiculo WHERE Ano = @Ano;

        INSERT INTO AnosDisponiveisVeiculo (Ano, TotalViagens, TotalAbastecimentos, DataAtualizacao)
        SELECT
            @Ano,
            (SELECT COUNT(*) FROM Viagem WHERE YEAR(DataInicial) = @Ano),
            (SELECT COUNT(*) FROM Abastecimento WHERE YEAR(DataHora) = @Ano),
            GETDATE();

        COMMIT TRANSACTION;
        PRINT 'Rankings anuais de veículos do ano ' + CAST(@Ano AS VARCHAR) + ' recalculados com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
-- Procedure: sp_RecalcularTodasEstatisticasAbastecimentos
PRINT 'Criando/atualizando procedure: sp_RecalcularTodasEstatisticasAbastecimentos...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularTodasEstatisticasAbastecimentos
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AnoMes TABLE (Ano INT, Mes INT);

    -- Busca todos os anos/meses com abastecimentos
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataHora), MONTH(DataHora)
    FROM Abastecimento
    WHERE DataHora IS NOT NULL
    ORDER BY YEAR(DataHora), MONTH(DataHora);

    DECLARE @Ano INT, @Mes INT;
    DECLARE cur CURSOR FOR SELECT Ano, Mes FROM @AnoMes ORDER BY Ano, Mes;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Ano, @Mes;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processando ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasAbastecimentos @Ano, @Mes;
        FETCH NEXT FROM cur INTO @Ano, @Mes;
    END

    CLOSE cur;
    DEALLOCATE cur;

    -- Recalcula estatísticas anuais
    DECLARE @Anos TABLE (Ano INT);
    INSERT INTO @Anos SELECT DISTINCT Ano FROM @AnoMes;

    DECLARE cur2 CURSOR FOR SELECT Ano FROM @Anos ORDER BY Ano;
    OPEN cur2;
    FETCH NEXT FROM cur2 INTO @Ano;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processando estatísticas anuais de ' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasAbastecimentosAnuais @Ano;
        FETCH NEXT FROM cur2 INTO @Ano;
    END

    CLOSE cur2;
    DEALLOCATE cur2;

    PRINT 'Todas as estatísticas de abastecimentos foram recalculadas!';
END
GO
-- Procedure: sp_RecalcularTodasEstatisticasMotoristas
PRINT 'Criando/atualizando procedure: sp_RecalcularTodasEstatisticasMotoristas...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularTodasEstatisticasMotoristas
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AnoMes TABLE (Ano INT, Mes INT);

    -- Busca todos os anos/meses com viagens
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataInicial), MONTH(DataInicial)
    FROM Viagem
    WHERE DataInicial IS NOT NULL AND MotoristaId IS NOT NULL
    ORDER BY YEAR(DataInicial), MONTH(DataInicial);

    -- Adiciona meses de multas que não estão em viagens
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(Data), MONTH(Data)
    FROM Multa
    WHERE Data IS NOT NULL
      AND MotoristaId IS NOT NULL
      AND NOT EXISTS (
          SELECT 1 FROM @AnoMes am
          WHERE am.Ano = YEAR(Data) AND am.Mes = MONTH(Data)
      );

    -- Adiciona meses de abastecimentos que não estão nas anteriores
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataHora), MONTH(DataHora)
    FROM Abastecimento
    WHERE DataHora IS NOT NULL
      AND MotoristaId IS NOT NULL
      AND MotoristaId <> '00000000-0000-0000-0000-000000000000'
      AND NOT EXISTS (
          SELECT 1 FROM @AnoMes am
          WHERE am.Ano = YEAR(DataHora) AND am.Mes = MONTH(DataHora)
      );

    DECLARE @Ano INT, @Mes INT;
    DECLARE cur CURSOR FOR SELECT Ano, Mes FROM @AnoMes ORDER BY Ano, Mes;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Ano, @Mes;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processando ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasMotoristas @Ano, @Mes;
        FETCH NEXT FROM cur INTO @Ano, @Mes;
    END

    CLOSE cur;
    DEALLOCATE cur;

    PRINT 'Todas as estatísticas foram recalculadas!';
END
GO
-- Procedure: sp_RecalcularTodasEstatisticasVeiculos
PRINT 'Criando/atualizando procedure: sp_RecalcularTodasEstatisticasVeiculos...';
GO

CREATE OR ALTER PROCEDURE dbo.sp_RecalcularTodasEstatisticasVeiculos
AS
BEGIN
    SET NOCOUNT ON;

    PRINT 'Iniciando recálculo de todas as estatísticas de veículos...';
    PRINT '';

    -- Estatísticas da frota (snapshot atual)
    PRINT '1. Recalculando estatísticas gerais da frota...';
    EXEC sp_RecalcularEstatisticasVeiculoGeral;

    PRINT '2. Recalculando estatísticas por categoria...';
    EXEC sp_RecalcularEstatisticasVeiculoCategoria;

    PRINT '3. Recalculando estatísticas por status...';
    EXEC sp_RecalcularEstatisticasVeiculoStatus;

    PRINT '4. Recalculando estatísticas por modelo...';
    EXEC sp_RecalcularEstatisticasVeiculoModelo;

    PRINT '5. Recalculando estatísticas por combustível...';
    EXEC sp_RecalcularEstatisticasVeiculoCombustivel;

    PRINT '6. Recalculando estatísticas por unidade...';
    EXEC sp_RecalcularEstatisticasVeiculoUnidade;

    PRINT '7. Recalculando estatísticas por ano de fabricação...';
    EXEC sp_RecalcularEstatisticasVeiculoAnoFabricacao;

    -- Estatísticas de uso (por ano/mês)
    PRINT '';
    PRINT '8. Recalculando estatísticas de uso mensal...';

    DECLARE @AnoMes TABLE (Ano INT, Mes INT);

    -- Anos/meses de viagens
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataInicial), MONTH(DataInicial)
    FROM Viagem
    WHERE DataInicial IS NOT NULL;

    -- Anos/meses de abastecimentos (sem duplicar)
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataHora), MONTH(DataHora)
    FROM Abastecimento
    WHERE DataHora IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM @AnoMes am WHERE am.Ano = YEAR(DataHora) AND am.Mes = MONTH(DataHora));

    DECLARE @Ano INT, @Mes INT;
    DECLARE cur CURSOR FOR SELECT DISTINCT Ano, Mes FROM @AnoMes ORDER BY Ano, Mes;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Ano, @Mes;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT '   Processando ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasVeiculoUsoMensal @Ano, @Mes;
        FETCH NEXT FROM cur INTO @Ano, @Mes;
    END

    CLOSE cur;
    DEALLOCATE cur;

    -- Rankings anuais
    PRINT '';
    PRINT '9. Recalculando rankings anuais...';

    DECLARE @Anos TABLE (Ano INT);
    INSERT INTO @Anos SELECT DISTINCT Ano FROM @AnoMes;

    DECLARE cur2 CURSOR FOR SELECT Ano FROM @Anos ORDER BY Ano;
    OPEN cur2;
    FETCH NEXT FROM cur2 INTO @Ano;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT '   Processando rankings de ' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularRankingsVeiculoAnual @Ano;
        FETCH NEXT FROM cur2 INTO @Ano;
    END

    CLOSE cur2;
    DEALLOCATE cur2;

    PRINT '';
    PRINT '=====================================================';
    PRINT 'Todas as estatísticas de veículos foram recalculadas!';
    PRINT '=====================================================';
END
GO
-- Procedure: usp_PreencheNulos_Motorista
PRINT 'Criando/atualizando procedure: usp_PreencheNulos_Motorista...';
GO

CREATE OR ALTER PROCEDURE dbo.usp_PreencheNulos_Motorista
    @Keys dbo.MotoristaKeyList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- Atualiza somente colunas NULL e somente linhas passadas em @Keys
    UPDATE M
       SET
         -- strings
         CPF               = ISNULL(M.CPF,               ''),
         CNH               = ISNULL(M.CNH,               ''),
         CategoriaCNH      = ISNULL(M.CategoriaCNH,      ''),
         Celular01         = ISNULL(M.Celular01,         ''),
         Celular02         = ISNULL(M.Celular02,         ''),
         OrigemIndicacao   = ISNULL(M.OrigemIndicacao,   ''),
         UsuarioIdAlteracao= ISNULL(M.UsuarioIdAlteracao,''),
         TipoCondutor      = ISNULL(M.TipoCondutor,      ''),
         EfetivoFerista    = ISNULL(M.EfetivoFerista,    ''),

         -- datas
         DataNascimento    = ISNULL(M.DataNascimento,     CONVERT(date,'19000101')),
         DataVencimentoCNH = ISNULL(M.DataVencimentoCNH,  CONVERT(date,'19000101')),
         DataIngresso      = ISNULL(M.DataIngresso,       CONVERT(date,'19000101')),
         DataAlteracao     = ISNULL(M.DataAlteracao,      GETDATE()),

         -- binário (mantenho excluído por padrão)
         -- Foto           = ISNULL(M.Foto,        0x),
         -- CNHDigital     = ISNULL(M.CNHDigital,  0x),

         -- numéricos/bit
         Status            = ISNULL(M.Status, 0),
         CodMotoristaQCard = ISNULL(M.CodMotoristaQCard, 0),

         -- GUIDs
         UnidadeId         = ISNULL(M.UnidadeId, CONVERT(uniqueidentifier,'00000000-0000-0000-0000-000000000000')),
         ContratoId        = ISNULL(M.ContratoId,CONVERT(uniqueidentifier,'00000000-0000-0000-0000-000000000000')),
         CondutorId        = ISNULL(M.CondutorId,CONVERT(uniqueidentifier,'00000000-0000-0000-0000-000000000000'))
    FROM dbo.Motorista AS M
    INNER JOIN @Keys      AS K ON K.MotoristaId = M.MotoristaId
    WHERE
         M.CPF                IS NULL OR
         M.CNH                IS NULL OR
         M.CategoriaCNH       IS NULL OR
         M.Celular01          IS NULL OR
         M.Celular02          IS NULL OR
         M.OrigemIndicacao    IS NULL OR
         M.UsuarioIdAlteracao IS NULL OR
         M.TipoCondutor       IS NULL OR
         M.EfetivoFerista     IS NULL OR

         M.DataNascimento     IS NULL OR
         M.DataVencimentoCNH  IS NULL OR
         M.DataIngresso       IS NULL OR
         M.DataAlteracao      IS NULL OR

         -- M.Foto            IS NULL OR
         -- M.CNHDigital      IS NULL OR

         M.Status             IS NULL OR
         M.CodMotoristaQCard  IS NULL OR

         M.UnidadeId          IS NULL OR
         M.ContratoId         IS NULL OR
         M.CondutorId         IS NULL;
END
GO
-- ======================================================================================
-- SE??O 5: REMOVER PROCEDURES OBSOLETAS (1 procedures)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 5: REMOVER PROCEDURES OBSOLETAS';
PRINT '======================================================================================';
PRINT '';
GO
-- Remover procedure: sp_RecalcularEstatisticasMultasMotoristas
PRINT 'Removendo procedure: sp_RecalcularEstatisticasMultasMotoristas...';
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_RecalcularEstatisticasMultasMotoristas')
BEGIN
    DROP PROCEDURE [dbo].[sp_RecalcularEstatisticasMultasMotoristas]
    PRINT '  ? Procedure sp_RecalcularEstatisticasMultasMotoristas removida.';
END
ELSE
BEGIN
    PRINT '  ??  Procedure sp_RecalcularEstatisticasMultasMotoristas j? n?o existe.';
END
GO
-- ======================================================================================
-- RESUMO FINAL
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT '                           RESUMO DA ATUALIZA??O';
PRINT '======================================================================================';

DECLARE @TotalTabelas INT = 31;
DECLARE @TotalViews INT = 67;
DECLARE @TotalProcedures INT = 21;
DECLARE @TotalRemovidas INT = 1;

PRINT '  ? Novas tabelas criadas: ' + CAST(@TotalTabelas AS VARCHAR);
PRINT '  ?? Views atualizadas: ' + CAST(@TotalViews AS VARCHAR);
PRINT '  ?? Procedures atualizadas: ' + CAST(@TotalProcedures AS VARCHAR);
PRINT '  ? Procedures removidas: ' + CAST(@TotalRemovidas AS VARCHAR);
PRINT '';
PRINT '  ? ATUALIZA??O CONCLU?DA COM SUCESSO!';
PRINT '';
PRINT '======================================================================================';
PRINT 'Data/Hora t?rmino: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '======================================================================================';
GO

