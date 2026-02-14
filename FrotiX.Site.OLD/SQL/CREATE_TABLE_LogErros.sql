/* ****************************************************************************************
 * ⚡ SCRIPT SQL: Criação da Tabela LogErros
 * --------------------------------------------------------------------------------------
 * 📂 PROJETO: FrotiX - Sistema de Gestão de Frotas
 * 🎯 OBJETIVO: Criar tabela LogErros para armazenar logs do servidor (C#) e cliente (JS)
 *              com índices otimizados para consultas rápidas e análises avançadas.
 *
 * 📋 CARACTERÍSTICAS:
 *    • BIGINT para suportar milhões de registros
 *    • Índices otimizados para consultas por data, tipo, origem, usuário
 *    • Campos computados: MensagemCurta (200 chars) e HashErro (SHA256)
 *    • Suporte completo a logs de servidor (C#) e cliente (JavaScript/Console)
 *    • Campos para correlação: usuário, URL, sessão, IP
 *
 * 📝 VERSÃO: 2.0
 * 📅 DATA: 14/02/2026
 * 👤 AUTOR: Claude Code
 *
 * 🔧 BASEADO EM: Models/LogErro.cs
 **************************************************************************************** */

USE [Frotix];
GO

-- ========================================================
-- 1. VERIFICAR SE TABELA JÁ EXISTE
-- ========================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogErros]') AND type in (N'U'))
BEGIN
    PRINT '⚠️ ATENÇÃO: Tabela [LogErros] já existe no banco de dados.';
    PRINT '   Para recriar a tabela, execute primeiro o script de DROP.';
    PRINT '   Script interrompido para evitar perda de dados.';
    RETURN; -- Interrompe execução
END
ELSE
BEGIN
    PRINT '✅ Iniciando criação da tabela [LogErros]...';
END
GO

-- ========================================================
-- 2. CRIAR TABELA LogErros
-- ========================================================
PRINT '📊 Criando estrutura da tabela...';

CREATE TABLE [dbo].[LogErros] (
    -- ====== IDENTIFICAÇÃO ======
    [LogErroId] BIGINT IDENTITY(1,1) NOT NULL,
    [DataHora] DATETIME2(3) NOT NULL DEFAULT GETDATE(), -- Precisão de milissegundos

    -- ====== CLASSIFICAÇÃO ======
    [Tipo] NVARCHAR(50) NOT NULL,           -- ERROR, WARN, INFO, ERROR-JS, CONSOLE-INFO, HTTP-ERROR, etc.
    [Origem] NVARCHAR(20) NOT NULL,         -- SERVER, CLIENT
    [Nivel] NVARCHAR(20) NULL,              -- Critical, Error, Warning, Information, Debug
    [Categoria] NVARCHAR(100) NULL,         -- Controller, Service, Page, JavaScript, Console, etc.

    -- ====== MENSAGEM E DETALHES ======
    [Mensagem] NVARCHAR(MAX) NOT NULL,

    -- Campo computado: Mensagem curta (200 caracteres + "...")
    [MensagemCurta] AS (
        CASE
            WHEN LEN([Mensagem]) > 200
            THEN LEFT([Mensagem], 200) + '...'
            ELSE [Mensagem]
        END
    ) PERSISTED,                             -- PERSISTED para criar índice sobre o campo

    -- ====== LOCALIZAÇÃO DO ERRO (CÓDIGO) ======
    [Arquivo] NVARCHAR(500) NULL,           -- Arquivo onde o erro ocorreu (VeiculoController.cs, app.js)
    [Metodo] NVARCHAR(200) NULL,            -- Método/Função onde ocorreu
    [Linha] INT NULL,                       -- Número da linha
    [Coluna] INT NULL,                      -- Número da coluna (para JavaScript)

    -- ====== EXCEÇÃO (PARA ERROS DO SERVIDOR C#) ======
    [ExceptionType] NVARCHAR(200) NULL,     -- Tipo da exceção (NullReferenceException, etc.)
    [ExceptionMessage] NVARCHAR(MAX) NULL,  -- Mensagem da exceção
    [StackTrace] NVARCHAR(MAX) NULL,        -- Stack trace completo
    [InnerException] NVARCHAR(MAX) NULL,    -- Inner exception (se houver)

    -- ====== CONTEXTO HTTP ======
    [Url] NVARCHAR(1000) NULL,              -- URL onde o erro ocorreu
    [HttpMethod] NVARCHAR(10) NULL,         -- GET, POST, PUT, DELETE, etc.
    [StatusCode] INT NULL,                  -- Status HTTP (404, 500, etc.)
    [UserAgent] NVARCHAR(500) NULL,         -- User Agent (navegador/dispositivo)
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
    -- Campo computado: Hash SHA256 para agrupar erros similares
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

    -- ====== CHAVE PRIMÁRIA ======
    CONSTRAINT [PK_LogErros] PRIMARY KEY CLUSTERED ([LogErroId] DESC) -- DESC: logs recentes primeiro
);
GO

PRINT '✅ Tabela [LogErros] criada com sucesso!';
GO

-- ========================================================
-- 3. CRIAR ÍNDICES OTIMIZADOS
-- ========================================================
PRINT '📊 Criando índices otimizados...';

-- Índice 1: Consultas por data (mais comum no dashboard)
CREATE NONCLUSTERED INDEX [IX_LogErros_DataHora]
ON [dbo].[LogErros]([DataHora] DESC)
INCLUDE ([Tipo], [Origem], [Nivel], [MensagemCurta], [Usuario]);
GO

-- Índice 2: Filtros por tipo de log
CREATE NONCLUSTERED INDEX [IX_LogErros_Tipo]
ON [dbo].[LogErros]([Tipo], [DataHora] DESC);
GO

-- Índice 3: Filtros por origem (SERVER/CLIENT)
CREATE NONCLUSTERED INDEX [IX_LogErros_Origem]
ON [dbo].[LogErros]([Origem], [DataHora] DESC);
GO

-- Índice 4: Consultas por usuário (apenas registros com usuário)
CREATE NONCLUSTERED INDEX [IX_LogErros_Usuario]
ON [dbo].[LogErros]([Usuario], [DataHora] DESC)
INCLUDE ([Tipo], [Mensagem])
WHERE [Usuario] IS NOT NULL;
GO

-- Índice 5: Análise de páginas/URLs com mais erros
CREATE NONCLUSTERED INDEX [IX_LogErros_Url]
ON [dbo].[LogErros]([Url], [Tipo])
INCLUDE ([DataHora], [Usuario])
WHERE [Url] IS NOT NULL;
GO

-- Índice 6: Agrupamento de erros similares (por hash)
CREATE NONCLUSTERED INDEX [IX_LogErros_HashErro]
ON [dbo].[LogErros]([HashErro], [DataHora] DESC)
INCLUDE ([Mensagem], [Arquivo], [Linha], [Tipo]);
GO

-- Índice 7: Erros não resolvidos (filtro importante)
CREATE NONCLUSTERED INDEX [IX_LogErros_Resolvido]
ON [dbo].[LogErros]([Resolvido], [Tipo], [DataHora] DESC)
INCLUDE ([Mensagem], [Usuario])
WHERE [Resolvido] = 0;
GO

-- Índice 8: Dashboard (combinação tipo + origem + data)
CREATE NONCLUSTERED INDEX [IX_LogErros_Dashboard]
ON [dbo].[LogErros]([Tipo], [Origem], [DataHora] DESC)
INCLUDE ([Usuario], [Url], [MensagemCurta]);
GO

-- Índice 9: Análise por arquivo (código fonte)
CREATE NONCLUSTERED INDEX [IX_LogErros_Arquivo]
ON [dbo].[LogErros]([Arquivo], [DataHora] DESC)
INCLUDE ([Linha], [Metodo], [Tipo])
WHERE [Arquivo] IS NOT NULL;
GO

-- Índice 10: Erros HTTP (por status code)
CREATE NONCLUSTERED INDEX [IX_LogErros_StatusCode]
ON [dbo].[LogErros]([StatusCode], [DataHora] DESC)
INCLUDE ([Url], [HttpMethod])
WHERE [StatusCode] IS NOT NULL;
GO

PRINT '✅ 10 índices criados com sucesso!';
GO

-- ========================================================
-- 4. CRIAR ESTATÍSTICAS ADICIONAIS
-- ========================================================
PRINT '📈 Criando estatísticas adicionais...';

CREATE STATISTICS [STAT_LogErros_TipoOrigem]
ON [dbo].[LogErros]([Tipo], [Origem]);
GO

CREATE STATISTICS [STAT_LogErros_DataHoraTipo]
ON [dbo].[LogErros]([DataHora], [Tipo]);
GO

CREATE STATISTICS [STAT_LogErros_NivelCategoria]
ON [dbo].[LogErros]([Nivel], [Categoria]);
GO

PRINT '✅ Estatísticas criadas com sucesso!';
GO

-- ========================================================
-- 5. ADICIONAR COMENTÁRIOS (EXTENDED PROPERTIES)
-- ========================================================
PRINT '📝 Adicionando documentação na tabela...';

-- Comentário na tabela
EXEC sys.sp_addextendedproperty
    @name=N'MS_Description',
    @value=N'Tabela de logs de erros do sistema FrotiX. Armazena logs do servidor (C#) e cliente (JavaScript/Console) para auditoria, análise e dashboards. Suporta milhões de registros com índices otimizados.',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros';
GO

-- Comentários nas colunas principais
EXEC sys.sp_addextendedproperty
    @name=N'MS_Description', @value=N'ID único do log (chave primária, auto-incremento)',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'LogErroId';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description', @value=N'Data e hora do erro (precisão de milissegundos)',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'DataHora';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description', @value=N'Tipo do log: ERROR, WARN, INFO, ERROR-JS, CONSOLE-INFO, CONSOLE-ERROR, HTTP-ERROR, etc.',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'Tipo';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description', @value=N'Origem do log: SERVER (C#/ASP.NET) ou CLIENT (JavaScript/Console)',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'Origem';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description', @value=N'Mensagem curta do erro (200 caracteres + ...). Campo COMPUTADO automaticamente.',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'MensagemCurta';
GO

EXEC sys.sp_addextendedproperty
    @name=N'MS_Description', @value=N'Hash SHA256 para agrupar erros similares (mesmo tipo, arquivo, linha, mensagem). Campo COMPUTADO automaticamente.',
    @level0type=N'SCHEMA', @level0name=N'dbo',
    @level1type=N'TABLE', @level1name=N'LogErros',
    @level2type=N'COLUMN', @level2name=N'HashErro';
GO

PRINT '✅ Documentação adicionada com sucesso!';
GO

-- ========================================================
-- 6. VERIFICAR CRIAÇÃO E EXIBIR RESUMO
-- ========================================================
PRINT '';
PRINT '╔════════════════════════════════════════════════════════════════╗';
PRINT '║          ✅ TABELA LogErros CRIADA COM SUCESSO!                ║';
PRINT '╚════════════════════════════════════════════════════════════════╝';
PRINT '';

-- Resumo da tabela criada
SELECT
    'LogErros' AS [Tabela],
    COUNT(*) AS [Total de Índices],
    (SELECT COUNT(*) FROM sys.columns WHERE object_id = OBJECT_ID('dbo.LogErros')) AS [Total de Colunas]
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.LogErros')
    AND index_id > 0;
GO

-- Listar todos os índices criados
PRINT '';
PRINT '📊 Índices criados:';
SELECT
    i.name AS [Nome do Índice],
    CASE i.type
        WHEN 1 THEN 'CLUSTERED'
        WHEN 2 THEN 'NONCLUSTERED'
        ELSE 'OTHER'
    END AS [Tipo],
    CASE
        WHEN i.is_unique = 1 THEN 'Sim'
        ELSE 'Não'
    END AS [Único]
FROM sys.indexes i
WHERE i.object_id = OBJECT_ID('dbo.LogErros')
    AND i.index_id > 0
ORDER BY i.name;
GO

PRINT '';
PRINT '🎉 Script executado com 100% de sucesso!';
PRINT '';
PRINT '📌 PRÓXIMOS PASSOS:';
PRINT '   1. ✅ Tabela criada no banco de dados';
PRINT '   2. 🔧 Reabilite o console-interceptor.js no _Layout.cshtml (linha 832)';
PRINT '   3. 🔄 Reinicie a aplicação (Ctrl+F5 no Visual Studio)';
PRINT '   4. 🧪 Teste a Agenda - os logs agora serão gravados no banco';
PRINT '';
PRINT '💡 DICA: Use o dashboard de LogErros para monitorar erros em tempo real!';
GO
