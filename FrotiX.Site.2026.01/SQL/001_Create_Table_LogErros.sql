/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 SCRIPT SQL: Criação da Tabela LogErros                                                          ║
   ║ 📂 PROJETO: FrotiX - Sistema de Gestão de Frotas                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO:                                                                                        ║
   ║    Criar tabela LogErros para armazenar todos os logs do sistema (servidor e cliente)              ║
   ║    com índices otimizados para consultas rápidas e análises avançadas.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CARACTERÍSTICAS:                                                                                 ║
   ║    • BIGINT para suportar milhões de registros                                                     ║
   ║    • Índices otimizados para consultas por data, tipo, origem, usuário                             ║
   ║    • Campos para análise completa (arquivo, linha, stack, url, user agent)                         ║
   ║    • Suporte a logs do servidor (C#) e cliente (JavaScript/Console)                                ║
   ║    • Campos para correlação (usuário, URL, sessão)                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 VERSÃO: 1.0 | DATA: 31/01/2026 | AUTOR: Claude Code                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

-- ========================================================
-- 0. CONFIGURAR OPÇÕES NECESSÁRIAS
-- ========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

-- ========================================================
-- 1. VERIFICAR SE TABELA JÁ EXISTE
-- ========================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogErros]') AND type in (N'U'))
BEGIN
    PRINT '⚠️ Tabela LogErros já existe. Execute o script de DROP primeiro se deseja recriar.'
    -- RETURN -- Descomente para impedir execução se já existir
END
ELSE
BEGIN
    PRINT '✅ Criando tabela LogErros...'
END
GO

-- ========================================================
-- 2. CRIAR TABELA LogErros
-- ========================================================
CREATE TABLE [dbo].[LogErros] (
    -- ====== IDENTIFICAÇÃO ======
    [LogErroId] BIGINT IDENTITY(1,1) NOT NULL,
    [DataHora] DATETIME2(3) NOT NULL DEFAULT GETDATE(), -- Precisão de milissegundos

    -- ====== CLASSIFICAÇÃO ======
    [Tipo] NVARCHAR(50) NOT NULL,           -- ERROR, WARN, INFO, ERROR-JS, CONSOLE-INFO, HTTP-ERROR, etc.
    [Origem] NVARCHAR(20) NOT NULL,         -- SERVER, CLIENT
    [Nivel] NVARCHAR(20) NULL,              -- Critical, Error, Warning, Information, Debug
    [Categoria] NVARCHAR(100) NULL,         -- Controller, Service, Page, JavaScript, etc.

    -- ====== MENSAGEM E DETALHES ======
    [Mensagem] NVARCHAR(MAX) NOT NULL,
    [MensagemCurta] AS (
        CASE
            WHEN LEN([Mensagem]) > 200
            THEN LEFT([Mensagem], 200) + '...'
            ELSE [Mensagem]
        END
    ) PERSISTED,                             -- Campo computado para buscas rápidas

    -- ====== LOCALIZAÇÃO DO ERRO (CÓDIGO) ======
    [Arquivo] NVARCHAR(500) NULL,           -- Arquivo onde o erro ocorreu
    [Metodo] NVARCHAR(200) NULL,            -- Método/Função onde ocorreu
    [Linha] INT NULL,                       -- Número da linha
    [Coluna] INT NULL,                      -- Número da coluna (para JS)

    -- ====== EXCEÇÃO (PARA ERROS DO SERVIDOR) ======
    [ExceptionType] NVARCHAR(200) NULL,     -- Tipo da exceção (ex: NullReferenceException)
    [ExceptionMessage] NVARCHAR(MAX) NULL,  -- Mensagem da exceção
    [StackTrace] NVARCHAR(MAX) NULL,        -- Stack trace completo
    [InnerException] NVARCHAR(MAX) NULL,    -- Inner exception (se houver)

    -- ====== CONTEXTO HTTP ======
    [Url] NVARCHAR(1000) NULL,              -- URL onde o erro ocorreu
    [HttpMethod] NVARCHAR(10) NULL,         -- GET, POST, PUT, DELETE, etc.
    [StatusCode] INT NULL,                  -- Status HTTP (para erros HTTP)
    [UserAgent] NVARCHAR(500) NULL,         -- User Agent (navegador)
    [IpAddress] NVARCHAR(45) NULL,          -- IP do usuário (suporta IPv6)

    -- ====== USUÁRIO E SESSÃO ======
    [Usuario] NVARCHAR(100) NULL,           -- Nome/email do usuário logado
    [SessionId] NVARCHAR(100) NULL,         -- ID da sessão (para correlacionar erros)

    -- ====== DADOS ADICIONAIS (JSON) ======
    [DadosAdicionais] NVARCHAR(MAX) NULL,   -- JSON com dados extras (formulários, estado, etc.)

    -- ====== RESOLUÇÃO ======
    [Resolvido] BIT NOT NULL DEFAULT 0,     -- Se o erro foi resolvido
    [DataResolucao] DATETIME2(3) NULL,      -- Quando foi resolvido
    [ResolvidoPor] NVARCHAR(100) NULL,      -- Quem resolveu
    [Observacoes] NVARCHAR(MAX) NULL,       -- Notas sobre a resolução

    -- ====== AGRUPAMENTO (PARA ANÁLISE) ======
    [HashErro] AS (                          -- Hash para agrupar erros similares
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

    -- ====== CHAVE PRIMÁRIA ======
    CONSTRAINT [PK_LogErros] PRIMARY KEY CLUSTERED ([LogErroId] DESC) -- DESC para logs mais recentes primeiro
);
GO

-- ========================================================
-- 3. CRIAR ÍNDICES OTIMIZADOS
-- ========================================================
PRINT '📊 Criando índices otimizados...'

-- Índice para consultas por data (mais comum)
CREATE NONCLUSTERED INDEX [IX_LogErros_DataHora]
ON [dbo].[LogErros]([DataHora] DESC)
INCLUDE ([Tipo], [Origem], [Nivel], [MensagemCurta], [Usuario]);
GO

-- Índice para filtros por tipo
CREATE NONCLUSTERED INDEX [IX_LogErros_Tipo]
ON [dbo].[LogErros]([Tipo], [DataHora] DESC);
GO

-- Índice para filtros por origem (servidor/cliente)
CREATE NONCLUSTERED INDEX [IX_LogErros_Origem]
ON [dbo].[LogErros]([Origem], [DataHora] DESC);
GO

-- Índice para consultas por usuário
CREATE NONCLUSTERED INDEX [IX_LogErros_Usuario]
ON [dbo].[LogErros]([Usuario], [DataHora] DESC)
WHERE [Usuario] IS NOT NULL;
GO

-- Índice para análise de páginas com mais erros
CREATE NONCLUSTERED INDEX [IX_LogErros_Url]
ON [dbo].[LogErros]([Url], [Tipo])
INCLUDE ([DataHora])
WHERE [Url] IS NOT NULL;
GO

-- Índice para agrupamento de erros similares
CREATE NONCLUSTERED INDEX [IX_LogErros_HashErro]
ON [dbo].[LogErros]([HashErro], [DataHora] DESC)
INCLUDE ([Mensagem], [Arquivo], [Linha]);
GO

-- Índice para erros não resolvidos
CREATE NONCLUSTERED INDEX [IX_LogErros_Resolvido]
ON [dbo].[LogErros]([Resolvido], [Tipo], [DataHora] DESC)
WHERE [Resolvido] = 0;
GO

-- Índice composto para dashboard (tipo + origem + data)
CREATE NONCLUSTERED INDEX [IX_LogErros_Dashboard]
ON [dbo].[LogErros]([Tipo], [Origem], [DataHora] DESC)
INCLUDE ([Usuario], [Url]);
GO

-- ========================================================
-- 4. CRIAR ESTATÍSTICAS ADICIONAIS
-- ========================================================
PRINT '📈 Criando estatísticas adicionais...'

CREATE STATISTICS [STAT_LogErros_TipoOrigem]
ON [dbo].[LogErros]([Tipo], [Origem]);
GO

CREATE STATISTICS [STAT_LogErros_DataHoraTipo]
ON [dbo].[LogErros]([DataHora], [Tipo]);
GO

-- ========================================================
-- 5. ADICIONAR COMENTÁRIOS EXTENDED PROPERTIES
-- ========================================================
PRINT '📝 Adicionando comentários na tabela...'

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description',
    @value=N'Tabela de logs de erros do sistema FrotiX (servidor e cliente). Armazena todos os erros, warnings e informações para análise e dashboard.' ,
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description',
    @value=N'Tipo do log: ERROR, WARN, INFO, ERROR-JS, CONSOLE-INFO, CONSOLE-ERROR, HTTP-ERROR, etc.' ,
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'Tipo';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description',
    @value=N'Origem do log: SERVER (C#/ASP.NET) ou CLIENT (JavaScript/Console)' ,
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'Origem';
GO

-- ========================================================
-- 6. VERIFICAR CRIAÇÃO
-- ========================================================
PRINT '✅ Tabela LogErros criada com sucesso!'
PRINT ''
PRINT '📊 Resumo:'
SELECT
    'LogErros' AS Tabela,
    COUNT(*) AS TotalIndices
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.LogErros')
    AND index_id > 0;
GO

PRINT ''
PRINT '🎉 Script executado com sucesso!'
PRINT '📌 Próximos passos:'
PRINT '   1. Executar Migration no EF Core (Add-Migration AddLogErrosTable)'
PRINT '   2. Atualizar LogService para gravar no banco'
PRINT '   3. Criar Repository ILogRepository'
GO
