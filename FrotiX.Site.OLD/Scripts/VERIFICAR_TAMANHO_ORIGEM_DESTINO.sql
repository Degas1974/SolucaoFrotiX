-- ══════════════════════════════════════════════════════════════════════════════
-- SCRIPT: Verificação de Tamanho dos Campos Origem e Destino
-- ══════════════════════════════════════════════════════════════════════════════
-- Descrição: Verifica se há valores com mais de 500 caracteres antes de executar
--            o script Limpeza_Origem_Destino.sql
--
-- Autor: Claude Sonnet 4.5 (FrotiX Team)
-- Data: 13/02/2026
-- ══════════════════════════════════════════════════════════════════════════════

SET NOCOUNT ON;
GO

PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '🔍 VERIFICAÇÃO DE TAMANHO - CAMPOS ORIGEM E DESTINO';
PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- 1. VERIFICAR VALORES COM > 500 CARACTERES
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '📊 Verificando valores com mais de 500 caracteres...';
PRINT '';

DECLARE @OrigemMaior500 INT;
DECLARE @DestinoMaior500 INT;

SELECT @OrigemMaior500 = COUNT(*)
FROM dbo.Viagem
WHERE LEN(Origem) > 500;

SELECT @DestinoMaior500 = COUNT(*)
FROM dbo.Viagem
WHERE LEN(Destino) > 500;

PRINT '📍 ORIGEM:';
PRINT '   - Registros com > 500 caracteres: ' + CAST(@OrigemMaior500 AS VARCHAR);

IF @OrigemMaior500 > 0
BEGIN
    PRINT '';
    PRINT '⚠️  ATENÇÃO: Encontrados valores que serão truncados!';
    PRINT '';
    PRINT 'Valores maiores que 500 caracteres:';

    SELECT TOP 10
        ViagemId,
        LEN(Origem) AS TamanhoOrigem,
        LEFT(Origem, 100) + '...' AS PreviaOrigem
    FROM dbo.Viagem
    WHERE LEN(Origem) > 500
    ORDER BY LEN(Origem) DESC;
END
ELSE
    PRINT '   ✅ Nenhum problema encontrado';

PRINT '';
PRINT '🎯 DESTINO:';
PRINT '   - Registros com > 500 caracteres: ' + CAST(@DestinoMaior500 AS VARCHAR);

IF @DestinoMaior500 > 0
BEGIN
    PRINT '';
    PRINT '⚠️  ATENÇÃO: Encontrados valores que serão truncados!';
    PRINT '';
    PRINT 'Valores maiores que 500 caracteres:';

    SELECT TOP 10
        ViagemId,
        LEN(Destino) AS TamanhoDestino,
        LEFT(Destino, 100) + '...' AS PreviaDestino
    FROM dbo.Viagem
    WHERE LEN(Destino) > 500
    ORDER BY LEN(Destino) DESC;
END
ELSE
    PRINT '   ✅ Nenhum problema encontrado';

PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- 2. ESTATÍSTICAS GERAIS
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '📊 ESTATÍSTICAS GERAIS';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

DECLARE @TotalViagens INT;
DECLARE @OrigemNaoVazio INT;
DECLARE @DestinoNaoVazio INT;
DECLARE @OrigemMaxLen INT;
DECLARE @DestinoMaxLen INT;
DECLARE @OrigemUnicos INT;
DECLARE @DestinoUnicos INT;

SELECT @TotalViagens = COUNT(*) FROM dbo.Viagem;
SELECT @OrigemNaoVazio = COUNT(*) FROM dbo.Viagem WHERE Origem IS NOT NULL AND Origem <> '';
SELECT @DestinoNaoVazio = COUNT(*) FROM dbo.Viagem WHERE Destino IS NOT NULL AND Destino <> '';
SELECT @OrigemMaxLen = MAX(LEN(Origem)) FROM dbo.Viagem WHERE Origem IS NOT NULL;
SELECT @DestinoMaxLen = MAX(LEN(Destino)) FROM dbo.Viagem WHERE Destino IS NOT NULL;
SELECT @OrigemUnicos = COUNT(DISTINCT Origem) FROM dbo.Viagem WHERE Origem IS NOT NULL AND Origem <> '';
SELECT @DestinoUnicos = COUNT(DISTINCT Destino) FROM dbo.Viagem WHERE Destino IS NOT NULL AND Destino <> '';

PRINT 'Total de viagens: ' + CAST(@TotalViagens AS VARCHAR);
PRINT '';
PRINT '📍 ORIGEM:';
PRINT '   - Registros não vazios: ' + CAST(@OrigemNaoVazio AS VARCHAR);
PRINT '   - Valores únicos: ' + CAST(@OrigemUnicos AS VARCHAR);
PRINT '   - Maior comprimento: ' + CAST(@OrigemMaxLen AS VARCHAR) + ' caracteres';
PRINT '';
PRINT '🎯 DESTINO:';
PRINT '   - Registros não vazios: ' + CAST(@DestinoNaoVazio AS VARCHAR);
PRINT '   - Valores únicos: ' + CAST(@DestinoUnicos AS VARCHAR);
PRINT '   - Maior comprimento: ' + CAST(@DestinoMaxLen AS VARCHAR) + ' caracteres';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- 3. VERIFICAR TIPO DE DADOS (varchar vs nvarchar)
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🔧 VERIFICAÇÃO DE TIPO DE DADOS';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

SELECT
    c.COLUMN_NAME AS Campo,
    c.DATA_TYPE AS TipoDados,
    c.CHARACTER_MAXIMUM_LENGTH AS TamanhoMaximo,
    c.IS_NULLABLE AS Nullable
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_NAME = 'Viagem'
  AND c.COLUMN_NAME IN ('Origem', 'Destino')
ORDER BY c.COLUMN_NAME;

PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- 4. CONCLUSÃO
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '📋 CONCLUSÃO';
PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '';

IF @OrigemMaior500 > 0 OR @DestinoMaior500 > 0
BEGIN
    PRINT '❌ NÃO EXECUTE O SCRIPT Limpeza_Origem_Destino.sql SEM AJUSTES!';
    PRINT '';
    PRINT '⚠️  Motivo: Existem valores com mais de 500 caracteres que serão';
    PRINT '   truncados pela tabela temporária #MapeamentoOrigemDestino.';
    PRINT '';
    PRINT '🔧 Ação recomendada:';
    PRINT '   1. Alterar NVARCHAR(500) para VARCHAR(MAX) nas linhas 173-174';
    PRINT '   2. Remover prefixo N das strings (ex: N''Aeroporto'' → ''Aeroporto'')';
    PRINT '   3. Corrigir linha 940: Observacao → Razao';
END
ELSE
BEGIN
    PRINT '✅ VERIFICAÇÃO APROVADA!';
    PRINT '';
    PRINT '📌 Próximos passos:';
    PRINT '   1. Revisar os mapeamentos no script';
    PRINT '   2. Fazer backup manual do banco (opcional, além do backup automático)';
    PRINT '   3. Executar em HORÁRIO DE BAIXO USO (script usa cursors - lento!)';
    PRINT '   4. Corrigir linha 940: Observacao → Razao';
    PRINT '';
    PRINT '⚠️  Observações:';
    PRINT '   - O script usa VARCHAR, mas seus campos são VARCHAR(MAX)';
    PRINT '   - Há uso de NVARCHAR e prefixo N (Unicode) desnecessário';
    PRINT '   - Fuzzy matching usa cursors (performance pode ser lenta)';
END

PRINT '';
PRINT '════════════════════════════════════════════════════════════════════════';

SET NOCOUNT OFF;
GO
