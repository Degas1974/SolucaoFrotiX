/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 SCRIPT SQL PRODUÇÃO: Criar Tabela LogErros no Banco FrotiX                                 ║
   ║ 📂 PROJETO: FrotiX - Sistema de Gestão de Frotas                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO:                                                                                        ║
   ║    Script SEGURO para executar no BANCO DE PRODUÇÃO                                                ║
   ║    Cria a tabela LogErros apenas se ela NÃO existir                                                ║
   ║    Adiciona índices e estatísticas de forma incremental                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ IMPORTANTE:                                                                                      ║
   ║    • Faça backup do banco ANTES de executar                                                        ║
   ║    • Execute em horário de baixo uso (madrugada)                                                   ║
   ║    • Monitore o log de execução                                                                    ║
   ║    • Tempo estimado: 30 segundos - 2 minutos (depende do tamanho do banco)                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 VERSÃO: 1.0 | DATA: 09/02/2026 | AUTOR: Claude Code                                             ║
   ║ 🔧 EXECUTAR EM: Servidor CTRAN01, Banco Frotix (PRODUÇÃO)                                          ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

-- ========================================================
-- CONFIGURAÇÕES E VALIDAÇÕES PRÉ-EXECUÇÃO
-- ========================================================
SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET XACT_ABORT ON; -- Rollback automático em caso de erro
GO

-- Verificar que estamos no banco correto
IF DB_NAME() <> N'Frotix'
BEGIN
    RAISERROR('❌ ERRO: Este script deve ser executado no banco Frotix!', 16, 1);
    RETURN;
END
GO

PRINT '╔════════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║                   INÍCIO DA EXECUÇÃO - CRIAÇÃO TABELA LogErros                      ║';
PRINT '╠════════════════════════════════════════════════════════════════════════════════════╣';
PRINT '║  Servidor: ' + @@SERVERNAME;
PRINT '║  Banco de dados: ' + DB_NAME();
PRINT '║  Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '║  Versão SQL Server: ' + CAST(SERVERPROPERTY('ProductVersion') AS VARCHAR);
PRINT '╚════════════════════════════════════════════════════════════════════════════════════╝';
PRINT '';
GO

-- ========================================================
-- 1. CRIAR TABELA LogErros (SE NÃO EXISTIR)
-- ========================================================
PRINT '📋 ETAPA 1/4: Verificando e criando tabela LogErros...'
PRINT ''
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogErros]') AND type in (N'U'))
BEGIN
    PRINT '  → Tabela LogErros NÃO existe. Criando...';
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
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
        TEXTIMAGE_ON [PRIMARY];
        
        COMMIT TRANSACTION;
        PRINT '  ✅ Tabela LogErros criada com sucesso!';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT '  ❌ ERRO ao criar tabela: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
ELSE
BEGIN
    PRINT '  ⚠️ Tabela LogErros JÁ EXISTE. Pulando criação.';
END
GO

PRINT '';
PRINT '────────────────────────────────────────────────────────────────────────────────────';
PRINT '';
GO

-- ========================================================
-- 2. CRIAR ÍNDICES
-- ========================================================
PRINT '📊 ETAPA 2/4: Criando índices otimizados...'
PRINT ''
GO

-- Índice: DataHora
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_DataHora' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_DataHora...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_DataHora]
    ON [dbo].[LogErros]([DataHora] DESC)
    INCLUDE ([Tipo], [Origem], [Nivel], [MensagemCurta], [Usuario])
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_DataHora já existe';
GO

-- Índice: Tipo
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Tipo' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_Tipo...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_Tipo]
    ON [dbo].[LogErros]([Tipo], [DataHora] DESC)
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_Tipo já existe';
GO

-- Índice: Origem
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Origem' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_Origem...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_Origem]
    ON [dbo].[LogErros]([Origem], [DataHora] DESC)
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_Origem já existe';
GO

-- Índice: Usuario
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Usuario' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_Usuario...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_Usuario]
    ON [dbo].[LogErros]([Usuario], [DataHora] DESC)
    WHERE [Usuario] IS NOT NULL
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_Usuario já existe';
GO

-- Índice: Url
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Url' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_Url...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_Url]
    ON [dbo].[LogErros]([Url], [Tipo])
    INCLUDE ([DataHora])
    WHERE [Url] IS NOT NULL
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_Url já existe';
GO

-- Índice: HashErro
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_HashErro' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_HashErro...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_HashErro]
    ON [dbo].[LogErros]([HashErro], [DataHora] DESC)
    INCLUDE ([Mensagem], [Arquivo], [Linha])
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_HashErro já existe';
GO

-- Índice: Resolvido
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Resolvido' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_Resolvido...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_Resolvido]
    ON [dbo].[LogErros]([Resolvido], [Tipo], [DataHora] DESC)
    WHERE [Resolvido] = 0
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_Resolvido já existe';
GO

-- Índice: Dashboard
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LogErros_Dashboard' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando índice IX_LogErros_Dashboard...';
    CREATE NONCLUSTERED INDEX [IX_LogErros_Dashboard]
    ON [dbo].[LogErros]([Tipo], [Origem], [DataHora] DESC)
    INCLUDE ([Usuario], [Url])
    ON [PRIMARY];
    PRINT '  ✅ Índice criado';
END
ELSE
    PRINT '  ⚠️ Índice IX_LogErros_Dashboard já existe';
GO

PRINT '';
PRINT '────────────────────────────────────────────────────────────────────────────────────';
PRINT '';
GO

-- ========================================================
-- 3. CRIAR ESTATÍSTICAS
-- ========================================================
PRINT '📈 ETAPA 3/4: Criando estatísticas...'
PRINT ''
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE name = 'STAT_LogErros_TipoOrigem' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando estatística STAT_LogErros_TipoOrigem...';
    CREATE STATISTICS [STAT_LogErros_TipoOrigem]
    ON [dbo].[LogErros]([Tipo], [Origem]);
    PRINT '  ✅ Estatística criada';
END
ELSE
    PRINT '  ⚠️ Estatística STAT_LogErros_TipoOrigem já existe';
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE name = 'STAT_LogErros_DataHoraTipo' AND object_id = OBJECT_ID('dbo.LogErros'))
BEGIN
    PRINT '  → Criando estatística STAT_LogErros_DataHoraTipo...';
    CREATE STATISTICS [STAT_LogErros_DataHoraTipo]
    ON [dbo].[LogErros]([DataHora], [Tipo]);
    PRINT '  ✅ Estatística criada';
END
ELSE
    PRINT '  ⚠️ Estatística STAT_LogErros_DataHoraTipo já existe';
GO

PRINT '';
PRINT '────────────────────────────────────────────────────────────────────────────────────';
PRINT '';
GO

-- ========================================================
-- 4. VALIDAÇÃO FINAL E RESUMO
-- ========================================================
PRINT '🔍 ETAPA 4/4: Validação final e resumo...'
PRINT ''
GO

DECLARE @TabelaExiste BIT = 0;
DECLARE @TotalIndices INT = 0;
DECLARE @TotalEstatisticas INT = 0;
DECLARE @TotalRegistros BIGINT = 0;

-- Verificar tabela
IF OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL
    SET @TabelaExiste = 1;

-- Contar índices
SELECT @TotalIndices = COUNT(*)
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.LogErros')
    AND index_id > 0;

-- Contar estatísticas
SELECT @TotalEstatisticas = COUNT(*)
FROM sys.stats
WHERE object_id = OBJECT_ID('dbo.LogErros')
    AND name LIKE 'STAT_LogErros%';

-- Contar registros
IF @TabelaExiste = 1
    SELECT @TotalRegistros = COUNT(*) FROM dbo.LogErros;

-- Exibir resumo
PRINT '╔════════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║                           RESUMO DA EXECUÇÃO                                         ║';
PRINT '╠════════════════════════════════════════════════════════════════════════════════════╣';
PRINT '║  Tabela LogErros: ' + CASE WHEN @TabelaExiste = 1 THEN '✅ EXISTE' ELSE '❌ NÃO CRIADA' END;
PRINT '║  Total de Índices: ' + CAST(@TotalIndices AS VARCHAR(10)) + ' (esperado: 9)';
PRINT '║  Total de Estatísticas: ' + CAST(@TotalEstatisticas AS VARCHAR(10)) + ' (esperado: 2)';
PRINT '║  Total de Registros: ' + CAST(@TotalRegistros AS VARCHAR(20));
PRINT '╠════════════════════════════════════════════════════════════════════════════════════╣';

IF @TabelaExiste = 1 AND @TotalIndices >= 9 AND @TotalEstatisticas >= 2
BEGIN
    PRINT '║  STATUS: ✅ SUCESSO - Tabela configurada corretamente!                            ║';
    PRINT '╚════════════════════════════════════════════════════════════════════════════════════╝';
    PRINT '';
    PRINT '🎉 SCRIPT EXECUTADO COM SUCESSO!';
    PRINT '';
    PRINT '📌 PRÓXIMOS PASSOS:';
    PRINT '   1. Reiniciar a aplicação FrotiX para ativar o sistema de logs';
    PRINT '   2. Monitorar os primeiros logs no dashboard: /Administracao/LogErros';
    PRINT '   3. Configurar rotina de limpeza automática de logs antigos (opcional)';
END
ELSE
BEGIN
    PRINT '║  STATUS: ⚠️ ATENÇÃO - Verificar configuração!                                   ║';
    PRINT '╚════════════════════════════════════════════════════════════════════════════════════╝';
    PRINT '';
    PRINT '⚠️ ATENÇÃO: Nem todos os objetos foram criados corretamente.';
    PRINT '   Revise as mensagens acima para identificar o problema.';
END

PRINT '';
PRINT '🕐 Data/Hora término: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '';
GO

-- ========================================================
-- FIM DO SCRIPT
-- ========================================================
