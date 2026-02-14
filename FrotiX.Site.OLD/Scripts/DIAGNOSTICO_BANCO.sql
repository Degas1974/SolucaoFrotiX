/****************************************************************************************
 * 🔍 SCRIPT: Diagnóstico Completo do Banco de Dados
 * --------------------------------------------------------------------------------------
 * Descrição: Identifica qual banco está conectado e lista todas as tabelas
 *
 * Autor: Claude Sonnet 4.5 (FrotiX Team)
 * Data: 13/02/2026
 ****************************************************************************************/

SET NOCOUNT ON;
GO

PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '🔍 DIAGNÓSTICO COMPLETO DO BANCO DE DADOS';
PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- INFORMAÇÕES DE CONEXÃO
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '📊 INFORMAÇÕES DE CONEXÃO:';
PRINT '   - Servidor: ' + @@SERVERNAME;
PRINT '   - Banco conectado: ' + DB_NAME();
PRINT '   - Usuário: ' + SYSTEM_USER;
PRINT '   - Session ID: ' + CAST(@@SPID AS VARCHAR);
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- LISTAR TODOS OS BANCOS DISPONÍVEIS
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '📁 BANCOS DE DADOS DISPONÍVEIS:';
PRINT '';

SELECT
    '   ' +
    CASE WHEN name = DB_NAME() THEN '👉 ' ELSE '   ' END +
    name +
    CASE WHEN name = DB_NAME() THEN ' (CONECTADO AGORA)' ELSE '' END AS [Banco de Dados]
FROM sys.databases
WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')
ORDER BY name;

PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- CONTAR TABELAS NO BANCO ATUAL
-- ═══════════════════════════════════════════════════════════════════════════════════

DECLARE @TotalTabelas INT;

SELECT @TotalTabelas = COUNT(*)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
  AND TABLE_SCHEMA = 'dbo';

PRINT '📊 TABELAS NO BANCO ATUAL (' + DB_NAME() + '):';
PRINT '   Total de tabelas: ' + CAST(@TotalTabelas AS VARCHAR);
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- VERIFICAR TABELAS ESPECÍFICAS (AS 9 DO BACKUP)
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '🔍 VERIFICANDO TABELAS ESPECÍFICAS:';
PRINT '';

CREATE TABLE #TabelasVerificacao (
    NomeTabela VARCHAR(100),
    Existe BIT,
    QtdRegistros INT
);

INSERT INTO #TabelasVerificacao (NomeTabela, Existe, QtdRegistros)
VALUES
    ('Abastecimento', 0, 0),
    ('AbastecimentoPendente', 0, 0),
    ('AlertasFrotiX', 0, 0),
    ('AlertasUsuario', 0, 0),
    ('AnosDisponiveisAbastecimento', 0, 0),
    ('AspNetUsers', 0, 0),
    ('AtaRegistroPrecos', 0, 0),
    ('Combustivel', 0, 0),
    ('Contrato', 0, 0);

-- Verificar existência
UPDATE t
SET t.Existe = 1
FROM #TabelasVerificacao t
WHERE EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = t.NomeTabela
      AND TABLE_SCHEMA = 'dbo'
);

-- Contar registros (apenas tabelas que existem)
DECLARE @TabelaNome VARCHAR(100);
DECLARE @SQL NVARCHAR(MAX);
DECLARE @Count INT;

DECLARE tabela_cursor CURSOR FOR
SELECT NomeTabela FROM #TabelasVerificacao WHERE Existe = 1;

OPEN tabela_cursor;
FETCH NEXT FROM tabela_cursor INTO @TabelaNome;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'SELECT @Count = COUNT(*) FROM dbo.' + QUOTENAME(@TabelaNome);
    EXEC sp_executesql @SQL, N'@Count INT OUTPUT', @Count OUTPUT;

    UPDATE #TabelasVerificacao
    SET QtdRegistros = @Count
    WHERE NomeTabela = @TabelaNome;

    FETCH NEXT FROM tabela_cursor INTO @TabelaNome;
END

CLOSE tabela_cursor;
DEALLOCATE tabela_cursor;

-- Mostrar resultado
SELECT
    CASE
        WHEN Existe = 1 THEN '✅ '
        ELSE '❌ '
    END + NomeTabela AS [Tabela],
    CASE
        WHEN Existe = 1 THEN 'SIM'
        ELSE 'NÃO'
    END AS [Existe],
    CASE
        WHEN Existe = 1 THEN CAST(QtdRegistros AS VARCHAR) + ' registros'
        ELSE '-'
    END AS [Quantidade]
FROM #TabelasVerificacao
ORDER BY NomeTabela;

PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- LISTAR TODAS AS TABELAS DO BANCO ATUAL
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '📋 TODAS AS TABELAS NO BANCO ' + DB_NAME() + ':';
PRINT '';

SELECT
    ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS [#],
    'dbo.' + TABLE_NAME AS [Nome Completo da Tabela]
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
  AND TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME;

PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- VERIFICAR SE HÁ OUTRO BANCO COM NOME SIMILAR
-- ═══════════════════════════════════════════════════════════════════════════════════

PRINT '🔍 VERIFICANDO BANCOS COM NOMES SIMILARES:';
PRINT '';

SELECT
    '   ' + name AS [Bancos com "Frotix" no nome]
FROM sys.databases
WHERE name LIKE '%Frotix%'
   OR name LIKE '%FrotiX%'
   OR name LIKE '%frotix%'
ORDER BY name;

PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════════
-- RESUMO E RECOMENDAÇÕES
-- ═══════════════════════════════════════════════════════════════════════════════════

DECLARE @TabelasEncontradas INT;
SELECT @TabelasEncontradas = COUNT(*) FROM #TabelasVerificacao WHERE Existe = 1;

PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '📊 RESUMO DO DIAGNÓSTICO';
PRINT '════════════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '✅ Banco conectado: ' + DB_NAME();
PRINT '✅ Total de tabelas no banco: ' + CAST(@TotalTabelas AS VARCHAR);
PRINT '✅ Tabelas encontradas (das 9 verificadas): ' + CAST(@TabelasEncontradas AS VARCHAR) + '/9';
PRINT '';

IF @TabelasEncontradas = 9
BEGIN
    PRINT '✅ PERFEITO! Todas as 9 tabelas foram encontradas.';
    PRINT '';
    PRINT '📋 Próximo passo:';
    PRINT '   Execute: SINCRONIZAR_BANCO_COM_MODELOS_V2.sql';
    PRINT '   (deve encontrar 9/9 tabelas agora)';
END
ELSE
BEGIN
    DECLARE @TabelasFaltando INT = 9 - @TabelasEncontradas;
    PRINT '⚠️  ATENÇÃO: ' + CAST(@TabelasFaltando AS VARCHAR) + ' tabelas não foram encontradas.';
    PRINT '';
    PRINT '📋 Tabelas faltando:';

    SELECT '   ❌ ' + NomeTabela AS [Tabelas Ausentes]
    FROM #TabelasVerificacao
    WHERE Existe = 0;

    PRINT '';
    PRINT '📋 Próximo passo:';
    PRINT '   1. Verifique se está conectado ao banco correto';
    PRINT '   2. Se sim, execute: CRIAR_TABELAS_FALTANTES.sql';
    PRINT '   3. Depois execute: SINCRONIZAR_BANCO_COM_MODELOS_V2.sql';
END

PRINT '';
PRINT '════════════════════════════════════════════════════════════════════════════════';

-- Limpar
DROP TABLE #TabelasVerificacao;

SET NOCOUNT OFF;
GO
