/* ======================================================================================
   SCRIPT MASTER DE ATUALIZAÇÃO COMPLETA - PRODUÇÃO → DESENVOLVIMENTO
   ======================================================================================
   
   VERSÃO: 2.0 FINAL INTEGRADA
   DATA: 09/02/2026
   
   ⚠️⚠️⚠️  LEIA ATENTAMENTE ANTES DE EXECUTAR  ⚠️⚠️⚠️
   
   ESTE É O SCRIPT COMPLETO E DEFINITIVO QUE CONTÉM:
   
   ═══════════════════════════════════════════════════════════════════════════════
   FASE 1: MIGRAÇÃO DE TABELAS EXISTENTES (ALTER TABLE)
   ═══════════════════════════════════════════════════════════════════════════════
   • Lavagem: Renomear Horario → HorarioLavagem  
   • Recurso: Adicionar HasChild + alterar tipos (varchar → nvarchar, NULL → NOT NULL)
   • Manutencao: Ajustar tamanho de colunas (nvarchar(450) → varchar(100))
   
   ✅ ROLLBACK AUTOMÁTICO em caso de erro nesta fase
   ✅ Transações por operação para segurança máxima
   ✅ Tabela de log: __MigracaoLog para rastreamento
   
   ═══════════════════════════════════════════════════════════════════════════════
   FASE 2: CRIAÇÃO DE NOVOS OBJETOS
   ═══════════════════════════════════════════════════════════════════════════════
   • 31 NOVAS TABELAS:
     - LogErros
     - Estatísticas (Motoristas, Veículos, Abastecimentos)
     - Rankings (Consumo, Km, Litros)
     - Heatmaps (Viagens, Abastecimentos mensais)
     - DocGenerator, SchemaChangeLog, RepactuacaoVeiculo, etc.
   
   • 67 VIEWS (2 novas + 65 atualizadas):
     - ViewMotoristasMulta, ViewEventos, ViewViagensAgenda, etc.
   
   • 20 STORED PROCEDURES (19 novas + 1 atualizada):
     - sp_RecalcularEstatisticasMotoristas
     - sp_RecalcularEstatisticasVeiculos  
     - sp_RecalcularEstatisticasAbastecimentos
     - etc.
   
   ⚠️  NOTA: Objetos novos NÃO usam transação (limitação DDL do SQL Server)
   ⚠️  Rollback manual necessário se cancelar Fase 2 (veja script ao final)
   
   ═══════════════════════════════════════════════════════════════════════════════
   PRÉ-REQUISITOS OBRIGATÓRIOS
   ═══════════════════════════════════════════════════════════════════════════════
   ☑️  1. BACKUP COMPLETO do banco Frotix
   ☑️  2. Executar em horário de baixo movimento (madrugada recomendada)
   ☑️  3. Conexão estável com servidor (não usar VPN instável)
   ☑️  4. Permissões: db_ddladmin + db_datawriter
   ☑️  5. Espaço em disco: ~500MB livres
   
   ═══════════════════════════════════════════════════════════════════════════════
   TEMPO ESTIMADO: 5-10 minutos
   ═══════════════════════════════════════════════════════════════════════════════
   
   ======================================================================================
*/

-- Configurações do SQL Server
SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET XACT_ABORT ON; -- Abort transaction on error
GO

USE Frotix
GO

-- V validation banco correto
IF DB_NAME() <> 'Frotix'
BEGIN
    RAISERROR('❌ ERRO: Este script deve ser executado no banco Frotix!', 16, 1);
    RETURN;
END
GO

PRINT '';
PRINT '╔══════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║                                                                                  ║';
PRINT '║           >>> ATUALIZAÇÃO COMPLETA DO BANCO FROTIX PRODUÇÃO <<<                 ║';
PRINT '║                                                                                  ║';
PRINT '╠══════════════════════════════════════════════════════════════════════════════════╣';
PRINT '║  Servidor: ' + @@SERVERNAME;
PRINT '║  Banco: ' + DB_NAME();
PRINT '║  Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '║  Versão SQL: ' + CAST(SERVERPROPERTY('ProductVersion') AS VARCHAR);
PRINT '║  Usuário: ' + SYSTEM_USER;
PRINT '╚══════════════════════════════════════════════════════════════════════════════════╝';
PRINT '';

-- Pausa para confirmação (comentar para execução automatizada)
-- WAITFOR DELAY '00:00:05';
GO

-- Criar tabela de log se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigracaoLog]') AND type IN (N'U'))
BEGIN
    CREATE TABLE [dbo].[__MigracaoLog] (
        LogId INT IDENTITY(1,1) PRIMARY KEY,
        DataHora DATETIME2(3) DEFAULT GETDATE(),
        Etapa VARCHAR(100),
        Status VARCHAR(20),
        Mensagem NVARCHAR(MAX),
        DuracaoMs INT NULL
    );
    PRINT '✓ Tabela de log __MigracaoLog criada.';
END
ELSE
BEGIN
    PRINT '✓ Tabela de log __MigracaoLog já existe.';
END
GO

DECLARE @InicioMigracao DATETIME2(3) = GETDATE();
GO