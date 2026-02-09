/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 SCRIPT SQL: Adicionar Tabela LogErros ao Script Principal FrotiX.sql                      ║
   ║ 📂 PROJETO: FrotiX - Sistema de Gestão de Frotas                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO:                                                                                        ║
   ║    Script para ser adicionado ao final do arquivo FrotiX.sql                                       ║
   ║    Cria a tabela LogErros com todos os índices e estatísticas                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 VERSÃO: 1.0 | DATA: 09/02/2026 | AUTOR: Claude Code                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

-- ================================================================================================
-- TABELA DE LOGS DE ERROS (LogErros)
-- Adicionado em: 09/02/2026
-- Descrição: Armazena logs de erros do servidor e cliente para análise e dashboard
-- ================================================================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

--
-- Create table [dbo].[LogErros]
--
PRINT (N'Create table [dbo].[LogErros]')
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogErros]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LogErros] (
        -- ====== IDENTIFICAÇÃO ======
        [LogErroId] BIGINT IDENTITY(1,1) NOT NULL,
        [DataHora] DATETIME2(3) NOT NULL DEFAULT GETDATE(),

        -- ====== CLASSIFICAÇÃO ======
        [Tipo] NVARCHAR(50) NOT NULL,
        [Origem] NVARCHAR(20) NOT NULL,
        [Nivel] NVARCHAR(20) NULL,
        [Categoria] NVARCHAR(100) NULL,

        -- ====== MENSAGEM E DETALHES ======
        [Mensagem] NVARCHAR(MAX) NOT NULL,
        [MensagemCurta] AS (
            CASE
                WHEN LEN([Mensagem]) > 200
                THEN LEFT([Mensagem], 200) + '...'
                ELSE [Mensagem]
            END
        ) PERSISTED,

        -- ====== LOCALIZAÇÃO DO ERRO ======
        [Arquivo] NVARCHAR(500) NULL,
        [Metodo] NVARCHAR(200) NULL,
        [Linha] INT NULL,
        [Coluna] INT NULL,

        -- ====== EXCEÇÃO ======
        [ExceptionType] NVARCHAR(200) NULL,
        [ExceptionMessage] NVARCHAR(MAX) NULL,
        [StackTrace] NVARCHAR(MAX) NULL,
        [InnerException] NVARCHAR(MAX) NULL,

        -- ====== CONTEXTO HTTP ======
        [Url] NVARCHAR(1000) NULL,
        [HttpMethod] NVARCHAR(10) NULL,
        [StatusCode] INT NULL,
        [UserAgent] NVARCHAR(500) NULL,
        [IpAddress] NVARCHAR(45) NULL,

        -- ====== USUÁRIO E SESSÃO ======
        [Usuario] NVARCHAR(100) NULL,
        [SessionId] NVARCHAR(100) NULL,

        -- ====== DADOS ADICIONAIS ======
        [DadosAdicionais] NVARCHAR(MAX) NULL,

        -- ====== RESOLUÇÃO ======
        [Resolvido] BIT NOT NULL DEFAULT 0,
        [DataResolucao] DATETIME2(3) NULL,
        [ResolvidoPor] NVARCHAR(100) NULL,
        [Observacoes] NVARCHAR(MAX) NULL,

        -- ====== AGRUPAMENTO ======
        [HashErro] AS (
            CONVERT(NVARCHAR(64),
                HASHBYTES('SHA2_256',
                    CONCAT(
                        ISNULL([Tipo], ''), '|',
                        ISNULL([Arquivo], ''), '|',
                        ISNULL(CAST([Linha] AS NVARCHAR(10)), '0'), '|',
                        LEFT(ISNULL([Mensagem], ''), 200)
                    )
                ), 2)
        ) PERSISTED,

        -- ====== AUDITORIA ======
        [CriadoEm] DATETIME2(3) NOT NULL DEFAULT GETDATE(),

        CONSTRAINT [PK_LogErros] PRIMARY KEY CLUSTERED ([LogErroId] DESC)
    )
    ON [PRIMARY]
    TEXTIMAGE_ON [PRIMARY]
    
    PRINT '✅ Tabela LogErros criada com sucesso'
END
ELSE
BEGIN
    PRINT '⚠️ Tabela LogErros já existe'
END
GO

--
-- Create indexes on table [dbo].[LogErros]
--
PRINT (N'Create indexes on table [dbo].[LogErros]')
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_DataHora' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_DataHora]
    ON [dbo].[LogErros]([DataHora] DESC)
    INCLUDE ([Tipo], [Origem], [Nivel], [MensagemCurta], [Usuario])
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Tipo' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_Tipo]
    ON [dbo].[LogErros]([Tipo], [DataHora] DESC)
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Origem' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_Origem]
    ON [dbo].[LogErros]([Origem], [DataHora] DESC)
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Usuario' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_Usuario]
    ON [dbo].[LogErros]([Usuario], [DataHora] DESC)
    WHERE [Usuario] IS NOT NULL
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Url' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_Url]
    ON [dbo].[LogErros]([Url], [Tipo])
    INCLUDE ([DataHora])
    WHERE [Url] IS NOT NULL
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_HashErro' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_HashErro]
    ON [dbo].[LogErros]([HashErro], [DataHora] DESC)
    INCLUDE ([Mensagem], [Arquivo], [Linha])
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Resolvido' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_Resolvido]
    ON [dbo].[LogErros]([Resolvido], [Tipo], [DataHora] DESC)
    WHERE [Resolvido] = 0
    ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Dashboard' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE NONCLUSTERED INDEX [IX_LogErros_Dashboard]
    ON [dbo].[LogErros]([Tipo], [Origem], [DataHora] DESC)
    INCLUDE ([Usuario], [Url])
    ON [PRIMARY]
GO

--
-- Create statistics on table [dbo].[LogErros]
--
PRINT (N'Create statistics on table [dbo].[LogErros]')
GO
IF NOT EXISTS (SELECT * FROM sys.stats WHERE name = 'STAT_LogErros_TipoOrigem' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE STATISTICS [STAT_LogErros_TipoOrigem]
    ON [dbo].[LogErros]([Tipo], [Origem])
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE name = 'STAT_LogErros_DataHoraTipo' AND object_id = OBJECT_ID('dbo.LogErros'))
    CREATE STATISTICS [STAT_LogErros_DataHoraTipo]
    ON [dbo].[LogErros]([DataHora], [Tipo])
GO

PRINT '✅ Tabela LogErros configurada com sucesso (9 índices + 2 estatísticas)'
GO

PRINT ''
PRINT '═══════════════════════════════════════════════════════════════════════════════════'
PRINT '   INSTRUÇÕES PARA ADICIONAR AO ARQUIVO FrotiX.sql:'
PRINT '═══════════════════════════════════════════════════════════════════════════════════'
PRINT '   1. Abra o arquivo FrotiX.sql'
PRINT '   2. Vá até o FINAL do arquivo (após a última linha)'
PRINT '   3. Cole todo o conteúdo deste arquivo (002_Add_LogErros_To_FrotiX.sql)'
PRINT '   4. Salve o arquivo FrotiX.sql'
PRINT ''
PRINT '   ⚠️ IMPORTANTE: Este script já foi executado no banco de desenvolvimento!'
PRINT '   ⚠️ Para produção, use o script: PRODUCAO_Add_Table_LogErros.sql'
PRINT '═══════════════════════════════════════════════════════════════════════════════════'
