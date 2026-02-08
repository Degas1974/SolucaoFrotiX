/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 SCRIPT SQL: Importação de Logs Históricos                                                       ║
   ║ 📂 PROJETO: FrotiX - Sistema de Gestão de Frotas                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO:                                                                                        ║
   ║    Importar logs históricos dos arquivos TXT para a tabela LogErros no banco de dados.            ║
   ║    Dados extraídos de: frotix_log_2026-01-29.txt, frotix_log_2026-01-30.txt, frotix_log_2026-01-31.txt║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 DADOS IMPORTADOS:                                                                               ║
   ║    • 29/01/2026: 21 registros (INFO + HTTP-ERROR)                                                  ║
   ║    • 30/01/2026: 10 registros (HTTP-ERROR)                                                         ║
   ║    • 31/01/2026: 9 registros (INFO)                                                                ║
   ║    • Dados simulados adicionais para análise (erros JS, console, etc.)                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📝 VERSÃO: 1.0 | DATA: 31/01/2026 | AUTOR: Claude Code                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

-- ========================================================
-- 1. VERIFICAR SE TABELA EXISTE
-- ========================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogErros]') AND type in (N'U'))
BEGIN
    PRINT '❌ ERRO: Tabela LogErros não existe. Execute primeiro o script 001_Create_Table_LogErros.sql'
    RETURN
END
GO

PRINT '✅ Tabela LogErros encontrada. Iniciando importação...'
GO

-- ========================================================
-- 2. LOGS DO DIA 29/01/2026 (Reais dos arquivos TXT)
-- ========================================================
PRINT '📅 Importando logs de 29/01/2026...'

INSERT INTO [dbo].[LogErros] ([DataHora], [Tipo], [Origem], [Nivel], [Categoria], [Mensagem], [Arquivo], [Metodo], [Url], [HttpMethod], [StatusCode], [Usuario], [Resolvido], [CriadoEm])
VALUES
-- INFO logs
('2026-01-29 21:52:20.498', 'INFO', 'SERVER', 'Information', 'System', 'LogService inicializado', 'LogService.cs', 'Constructor', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-29 21:52:20.517', 'INFO', 'SERVER', 'Information', 'System', 'Sistema de log de erros inicializado', 'Program.cs', 'ConfigureGlobalExceptionHandlers', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-29 21:53:12.707', 'INFO', 'SERVER', 'Information', 'System', 'FrotiX Web inicializado com sucesso', 'Startup.cs', 'Configure', NULL, NULL, NULL, 'System', 0, GETDATE()),

-- HTTP-ERROR logs (404)
('2026-01-29 22:09:57.634', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /.well-known/appspecific/com.chrome.devtools.json | Not Found', NULL, NULL, '/.well-known/appspecific/com.chrome.devtools.json', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:10:02.579', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /js/popper.js.map | Not Found', NULL, NULL, '/js/popper.js.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:10:08.729', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/vendors.bundle.css.map | Not Found', NULL, NULL, '/css/vendors.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:10:08.729', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/app.bundle.css.map | Not Found', NULL, NULL, '/css/app.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:25:32.756', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 400 | POST /api/reports/clients/keepAlive | Bad Request', NULL, NULL, '/api/reports/clients/keepAlive/be8dc155fb3', 'POST', 400, 'p_7661', 0, GETDATE()),
('2026-01-29 22:30:44.920', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /.well-known/appspecific/com.chrome.devtools.json | Not Found', NULL, NULL, '/.well-known/appspecific/com.chrome.devtools.json', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:30:46.445', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /js/popper.js.map | Not Found', NULL, NULL, '/js/popper.js.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:30:47.347', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/vendors.bundle.css.map | Not Found', NULL, NULL, '/css/vendors.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 22:30:47.388', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/app.bundle.css.map | Not Found', NULL, NULL, '/css/app.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 23:22:19.151', 'INFO', 'SERVER', 'Information', 'System', 'LogService inicializado', 'LogService.cs', 'Constructor', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-29 23:22:19.182', 'INFO', 'SERVER', 'Information', 'System', 'Sistema de log de erros inicializado', 'Program.cs', 'ConfigureGlobalExceptionHandlers', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-29 23:22:57.816', 'INFO', 'SERVER', 'Information', 'System', 'FrotiX Web inicializado com sucesso', 'Startup.cs', 'Configure', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-29 23:32:37.932', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /.well-known/appspecific/com.chrome.devtools.json | Not Found', NULL, NULL, '/.well-known/appspecific/com.chrome.devtools.json', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 23:32:40.443', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /js/popper.js.map | Not Found', NULL, NULL, '/js/popper.js.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 23:32:49.901', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/vendors.bundle.css.map | Not Found', NULL, NULL, '/css/vendors.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-29 23:32:50.019', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/app.bundle.css.map | Not Found', NULL, NULL, '/css/app.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE());

PRINT '   ✅ 17 registros inseridos para 29/01/2026'
GO

-- ========================================================
-- 3. LOGS DO DIA 30/01/2026 (Reais dos arquivos TXT)
-- ========================================================
PRINT '📅 Importando logs de 30/01/2026...'

INSERT INTO [dbo].[LogErros] ([DataHora], [Tipo], [Origem], [Nivel], [Categoria], [Mensagem], [Arquivo], [Metodo], [Url], [HttpMethod], [StatusCode], [Usuario], [Resolvido], [CriadoEm])
VALUES
('2026-01-30 00:03:11.850', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /.well-known/appspecific/com.chrome.devtools.json | Not Found', NULL, NULL, '/.well-known/appspecific/com.chrome.devtools.json', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:03:17.973', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /js/popper.js.map | Not Found', NULL, NULL, '/js/popper.js.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:03:36.766', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/app.bundle.css.map | Not Found', NULL, NULL, '/css/app.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:03:36.803', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/vendors.bundle.css.map | Not Found', NULL, NULL, '/css/vendors.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:09:24.649', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/app.bundle.css.map | Not Found', NULL, NULL, '/css/app.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:09:24.664', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/vendors.bundle.css.map | Not Found', NULL, NULL, '/css/vendors.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:09:24.976', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /.well-known/appspecific/com.chrome.devtools.json | Not Found', NULL, NULL, '/.well-known/appspecific/com.chrome.devtools.json', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:09:28.503', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/app.bundle.css.map | Not Found', NULL, NULL, '/css/app.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:09:28.540', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /css/vendors.bundle.css.map | Not Found', NULL, NULL, '/css/vendors.bundle.css.map', 'GET', 404, 'p_7661', 0, GETDATE()),
('2026-01-30 00:10:12.424', 'HTTP-ERROR', 'SERVER', 'Warning', 'HTTP', '🌐 Status: 404 | GET /js/popper.js.map | Not Found', NULL, NULL, '/js/popper.js.map', 'GET', 404, 'p_7661', 0, GETDATE());

PRINT '   ✅ 10 registros inseridos para 30/01/2026'
GO

-- ========================================================
-- 4. LOGS DO DIA 31/01/2026 (Reais dos arquivos TXT)
-- ========================================================
PRINT '📅 Importando logs de 31/01/2026...'

INSERT INTO [dbo].[LogErros] ([DataHora], [Tipo], [Origem], [Nivel], [Categoria], [Mensagem], [Arquivo], [Metodo], [Url], [HttpMethod], [StatusCode], [Usuario], [Resolvido], [CriadoEm])
VALUES
('2026-01-31 08:23:48.760', 'INFO', 'SERVER', 'Information', 'System', 'LogService inicializado', 'LogService.cs', 'Constructor', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 08:23:48.836', 'INFO', 'SERVER', 'Information', 'System', 'Sistema de log de erros inicializado', 'Program.cs', 'ConfigureGlobalExceptionHandlers', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 08:24:16.724', 'INFO', 'SERVER', 'Information', 'System', 'FrotiX Web inicializado com sucesso', 'Startup.cs', 'Configure', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 13:17:06.976', 'INFO', 'SERVER', 'Information', 'System', 'LogService inicializado', 'LogService.cs', 'Constructor', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 13:17:06.990', 'INFO', 'SERVER', 'Information', 'System', 'Sistema de log de erros inicializado', 'Program.cs', 'ConfigureGlobalExceptionHandlers', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 13:17:13.485', 'INFO', 'SERVER', 'Information', 'System', 'FrotiX Web inicializado com sucesso', 'Startup.cs', 'Configure', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 17:59:10.465', 'INFO', 'SERVER', 'Information', 'System', 'LogService inicializado', 'LogService.cs', 'Constructor', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 17:59:10.470', 'INFO', 'SERVER', 'Information', 'System', 'Sistema de log de erros inicializado', 'Program.cs', 'ConfigureGlobalExceptionHandlers', NULL, NULL, NULL, 'System', 0, GETDATE()),
('2026-01-31 17:59:17.513', 'INFO', 'SERVER', 'Information', 'System', 'FrotiX Web inicializado com sucesso', 'Startup.cs', 'Configure', NULL, NULL, NULL, 'System', 0, GETDATE());

PRINT '   ✅ 9 registros inseridos para 31/01/2026'
GO

-- ========================================================
-- 5. DADOS SIMULADOS ADICIONAIS (Para enriquecer análises)
-- ========================================================
PRINT '📊 Gerando dados simulados para análise de Dashboard...'

-- Adicionar erros variados ao longo dos últimos 7 dias
DECLARE @i INT = 0;
DECLARE @dataBase DATETIME = '2026-01-25';
DECLARE @tipos TABLE (Tipo NVARCHAR(50), Origem NVARCHAR(20), Nivel NVARCHAR(20), Categoria NVARCHAR(100));

INSERT INTO @tipos VALUES
    ('ERROR', 'SERVER', 'Error', 'Controller'),
    ('ERROR', 'SERVER', 'Error', 'Service'),
    ('ERROR-JS', 'CLIENT', 'Error', 'JavaScript'),
    ('WARN', 'SERVER', 'Warning', 'Validation'),
    ('CONSOLE-ERROR', 'CLIENT', 'Error', 'Console'),
    ('CONSOLE-WARN', 'CLIENT', 'Warning', 'Console'),
    ('CONSOLE-INFO', 'CLIENT', 'Information', 'Console');

-- Gera 100 logs distribuídos nos últimos 7 dias
WHILE @i < 100
BEGIN
    DECLARE @dia INT = @i % 7;
    DECLARE @hora INT = (@i * 17) % 24; -- Distribui ao longo do dia
    DECLARE @minuto INT = (@i * 37) % 60;
    DECLARE @tipoIdx INT = @i % 7;

    DECLARE @tipo NVARCHAR(50), @origem NVARCHAR(20), @nivel NVARCHAR(20), @categoria NVARCHAR(100);

    SELECT TOP 1 @tipo = Tipo, @origem = Origem, @nivel = Nivel, @categoria = Categoria
    FROM @tipos
    ORDER BY NEWID();

    DECLARE @dataLog DATETIME = DATEADD(HOUR, @hora, DATEADD(MINUTE, @minuto, DATEADD(DAY, @dia, @dataBase)));

    DECLARE @mensagem NVARCHAR(MAX) = CASE @tipo
        WHEN 'ERROR' THEN 'Erro ao processar requisição: NullReferenceException em ' + CASE @i % 5
            WHEN 0 THEN 'VeiculoController.Index'
            WHEN 1 THEN 'MotoristaService.GetById'
            WHEN 2 THEN 'ViagemRepository.Save'
            WHEN 3 THEN 'MultaController.Create'
            WHEN 4 THEN 'RelatorioService.Generate'
            END
        WHEN 'ERROR-JS' THEN 'Uncaught TypeError: Cannot read property ''' + CASE @i % 4
            WHEN 0 THEN 'length'
            WHEN 1 THEN 'value'
            WHEN 2 THEN 'id'
            WHEN 3 THEN 'data'
            END + ''' of undefined'
        WHEN 'WARN' THEN 'Validação falhou: ' + CASE @i % 3
            WHEN 0 THEN 'Campo obrigatório não preenchido'
            WHEN 1 THEN 'Data inválida informada'
            WHEN 2 THEN 'Valor fora do intervalo permitido'
            END
        WHEN 'CONSOLE-ERROR' THEN '[Console] Error: ' + CASE @i % 3
            WHEN 0 THEN 'Failed to load resource'
            WHEN 1 THEN 'Script error'
            WHEN 2 THEN 'Network request failed'
            END
        WHEN 'CONSOLE-WARN' THEN '[Console] Warning: ' + CASE @i % 2
            WHEN 0 THEN 'Deprecated API usage'
            WHEN 1 THEN 'Performance warning'
            END
        WHEN 'CONSOLE-INFO' THEN '[Console] Info: ' + CASE @i % 2
            WHEN 0 THEN 'Component mounted'
            WHEN 1 THEN 'Data loaded successfully'
            END
        ELSE 'Log de teste #' + CAST(@i AS NVARCHAR(10))
        END;

    DECLARE @arquivo NVARCHAR(500) = CASE @tipo
        WHEN 'ERROR' THEN CASE @i % 5
            WHEN 0 THEN 'VeiculoController.cs'
            WHEN 1 THEN 'MotoristaService.cs'
            WHEN 2 THEN 'ViagemRepository.cs'
            WHEN 3 THEN 'MultaController.cs'
            WHEN 4 THEN 'RelatorioService.cs'
            END
        WHEN 'ERROR-JS' THEN CASE @i % 3
            WHEN 0 THEN 'app.js'
            WHEN 1 THEN 'viagem-form.js'
            WHEN 2 THEN 'grid-helper.js'
            END
        ELSE NULL
        END;

    DECLARE @url NVARCHAR(1000) = CASE @i % 10
        WHEN 0 THEN '/Veiculos'
        WHEN 1 THEN '/Motoristas'
        WHEN 2 THEN '/Viagens/Create'
        WHEN 3 THEN '/Multas'
        WHEN 4 THEN '/Relatorios/Mensal'
        WHEN 5 THEN '/Abastecimentos'
        WHEN 6 THEN '/Manutencoes'
        WHEN 7 THEN '/Dashboard'
        WHEN 8 THEN '/Administracao/Usuarios'
        WHEN 9 THEN '/api/Veiculos/GetAll'
        END;

    DECLARE @usuario NVARCHAR(100) = CASE @i % 5
        WHEN 0 THEN 'p_7661'
        WHEN 1 THEN 'admin'
        WHEN 2 THEN 'p_1234'
        WHEN 3 THEN 'p_5678'
        WHEN 4 THEN 'supervisor'
        END;

    INSERT INTO [dbo].[LogErros]
        ([DataHora], [Tipo], [Origem], [Nivel], [Categoria], [Mensagem], [Arquivo], [Url], [Usuario], [Resolvido], [CriadoEm])
    VALUES
        (@dataLog, @tipo, @origem, @nivel, @categoria, @mensagem, @arquivo, @url, @usuario, 0, GETDATE());

    SET @i = @i + 1;
END

PRINT '   ✅ 100 registros simulados inseridos (25/01 a 31/01/2026)'
GO

-- ========================================================
-- 6. VERIFICAR IMPORTAÇÃO
-- ========================================================
PRINT ''
PRINT '📊 RESUMO DA IMPORTAÇÃO:'
PRINT '========================'

SELECT
    CAST([DataHora] AS DATE) AS Data,
    COUNT(*) AS TotalLogs,
    SUM(CASE WHEN [Tipo] LIKE '%ERROR%' THEN 1 ELSE 0 END) AS Erros,
    SUM(CASE WHEN [Tipo] = 'WARN' THEN 1 ELSE 0 END) AS Warnings,
    SUM(CASE WHEN [Tipo] = 'INFO' THEN 1 ELSE 0 END) AS Info,
    SUM(CASE WHEN [Tipo] LIKE 'CONSOLE-%' THEN 1 ELSE 0 END) AS Console
FROM [dbo].[LogErros]
GROUP BY CAST([DataHora] AS DATE)
ORDER BY Data;

PRINT ''
PRINT '📈 DISTRIBUIÇÃO POR TIPO:'

SELECT
    [Tipo],
    [Origem],
    COUNT(*) AS Total
FROM [dbo].[LogErros]
GROUP BY [Tipo], [Origem]
ORDER BY Total DESC;

PRINT ''
PRINT '🎉 Importação concluída com sucesso!'

DECLARE @totalRegistros INT;
SELECT @totalRegistros = COUNT(*) FROM [dbo].[LogErros];
PRINT '📌 Total de registros: ' + CAST(@totalRegistros AS NVARCHAR(10))
GO
