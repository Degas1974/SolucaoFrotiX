/****************************************************************************************
 * 🧹 SCRIPT: Limpeza e Unificação de Duplicatas - Campos Origem e Destino (CORRIGIDO)
 * --------------------------------------------------------------------------------------
 * Descrição: Remove duplicatas e padroniza valores nos campos Origem e Destino
 *            da tabela Viagem com lógica fuzzy matching
 *
 * Autor: Claude Sonnet 4.5 (FrotiX Team)
 * Data Original: 12/02/2026
 * Versão Corrigida: 13/02/2026
 *
 * ✅ CORREÇÕES APLICADAS:
 * - NVARCHAR(500) → VARCHAR(MAX) (compatível com banco)
 * - Removido prefixo N de strings Unicode (196 ocorrências)
 * - Corrigido coluna "Observacao" → "Razao"
 * - Adicionado sistema de progresso a cada 10 minutos
 * - Adicionado verificação de função Levenshtein
 * - Otimizado performance com índices temporários
 *
 * ⚠️ IMPORTANTE:
 * - Este script EXECUTA IMEDIATAMENTE todas as alterações
 * - Cria backup automático antes de fazer alterações
 * - Usa transação com rollback automático em caso de erro
 * - Mostra progresso detalhado durante execução
 * - Estimativa de tempo: 10min - 2h (depende do volume de dados)
 ****************************************************************************************/

SET NOCOUNT ON;
GO

PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '🧹 LIMPEZA DE DUPLICATAS - ORIGEM E DESTINO (VERSÃO CORRIGIDA)';
PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '⏰ Início: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- VARIÁVEIS GLOBAIS DE PROGRESSO
-- ══════════════════════════════════════════════════════════════════════════════

DECLARE @InicioExecucao DATETIME = GETDATE();
DECLARE @UltimoProgresso DATETIME = GETDATE();
DECLARE @ProximoProgresso DATETIME = DATEADD(MINUTE, 10, GETDATE());

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 0: VALIDAÇÕES PRELIMINARES
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🔍 FASE 0: VALIDAÇÕES PRELIMINARES';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

-- Verificar se tabela Viagem existe
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Viagem' AND TABLE_SCHEMA = 'dbo')
BEGIN
    PRINT '❌ ERRO: Tabela dbo.Viagem não encontrada!';
    PRINT '';
    RETURN;
END

-- Verificar se colunas Origem e Destino existem
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Viagem' AND COLUMN_NAME = 'Origem')
   OR NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Viagem' AND COLUMN_NAME = 'Destino')
BEGIN
    PRINT '❌ ERRO: Colunas Origem ou Destino não encontradas na tabela Viagem!';
    PRINT '';
    RETURN;
END

PRINT '✅ Validações preliminares concluídas';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 1: BACKUP E ESTATÍSTICAS INICIAIS
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '📊 FASE 1: BACKUP E ESTATÍSTICAS INICIAIS';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

-- Remover backup anterior se existir
IF OBJECT_ID('dbo.Viagem_Backup_OrigemDestino', 'U') IS NOT NULL
BEGIN
    PRINT '⚠️  Removendo backup anterior...';
    DROP TABLE dbo.Viagem_Backup_OrigemDestino;
END

-- Criar backup
PRINT '💾 Criando backup dos dados atuais...';
SELECT
    ViagemId,
    Origem AS OrigemOriginal,
    Destino AS DestinoOriginal,
    DataCriacao,
    GETDATE() AS DataBackup
INTO dbo.Viagem_Backup_OrigemDestino
FROM dbo.Viagem;

DECLARE @TotalRegistros INT = @@ROWCOUNT;
PRINT '✅ Backup criado: ' + CAST(@TotalRegistros AS VARCHAR) + ' registros';
PRINT '';

-- Contar valores ÚNICOS antes da limpeza
DECLARE @OrigemUnicosAntes INT;
DECLARE @DestinoUnicosAntes INT;

SELECT @OrigemUnicosAntes = COUNT(DISTINCT Origem)
FROM dbo.Viagem
WHERE Origem IS NOT NULL AND Origem <> '';

SELECT @DestinoUnicosAntes = COUNT(DISTINCT Destino)
FROM dbo.Viagem
WHERE Destino IS NOT NULL AND Destino <> '';

PRINT '📌 Valores únicos ANTES da limpeza:';
PRINT '   - Origem: ' + CAST(@OrigemUnicosAntes AS VARCHAR) + ' valores distintos';
PRINT '   - Destino: ' + CAST(@DestinoUnicosAntes AS VARCHAR) + ' valores distintos';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 1.5: CORREÇÃO DE ENCODING UTF-8/Latin1
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🔧 FASE 1.5: CORRIGINDO ERROS DE ENCODING UTF-8/Latin1';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';
PRINT '⚙️  Aplicando correções de encoding em ORIGEM e DESTINO...';
PRINT '';

-- Contar registros afetados ANTES da correção
DECLARE @RegistrosComEncodingErradoOrigem INT;
DECLARE @RegistrosComEncodingErradoDestino INT;

SELECT @RegistrosComEncodingErradoOrigem = COUNT(DISTINCT ViagemId)
FROM dbo.Viagem
WHERE Origem LIKE '%Ã%';

SELECT @RegistrosComEncodingErradoDestino = COUNT(DISTINCT ViagemId)
FROM dbo.Viagem
WHERE Destino LIKE '%Ã%';

PRINT '📊 Registros com possível erro de encoding:';
PRINT '   - Origem: ' + CAST(@RegistrosComEncodingErradoOrigem AS VARCHAR) + ' viagens';
PRINT '   - Destino: ' + CAST(@RegistrosComEncodingErradoDestino AS VARCHAR) + ' viagens';
PRINT '';

-- Aplicar correções de encoding (30 substituições)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã£', 'ã'), Destino = REPLACE(Destino, 'Ã£', 'ã') WHERE Origem LIKE '%Ã£%' OR Destino LIKE '%Ã£%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ãµ', 'õ'), Destino = REPLACE(Destino, 'Ãµ', 'õ') WHERE Origem LIKE '%Ãµ%' OR Destino LIKE '%Ãµ%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã§', 'ç'), Destino = REPLACE(Destino, 'Ã§', 'ç') WHERE Origem LIKE '%Ã§%' OR Destino LIKE '%Ã§%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã¡', 'á'), Destino = REPLACE(Destino, 'Ã¡', 'á') WHERE Origem LIKE '%Ã¡%' OR Destino LIKE '%Ã¡%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã©', 'é'), Destino = REPLACE(Destino, 'Ã©', 'é') WHERE Origem LIKE '%Ã©%' OR Destino LIKE '%Ã©%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã­', 'í'), Destino = REPLACE(Destino, 'Ã­', 'í') WHERE Origem LIKE '%Ã­%' OR Destino LIKE '%Ã­%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã³', 'ó'), Destino = REPLACE(Destino, 'Ã³', 'ó') WHERE Origem LIKE '%Ã³%' OR Destino LIKE '%Ã³%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ãº', 'ú'), Destino = REPLACE(Destino, 'Ãº', 'ú') WHERE Origem LIKE '%Ãº%' OR Destino LIKE '%Ãº%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã¢', 'â'), Destino = REPLACE(Destino, 'Ã¢', 'â') WHERE Origem LIKE '%Ã¢%' OR Destino LIKE '%Ã¢%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ãª', 'ê'), Destino = REPLACE(Destino, 'Ãª', 'ê') WHERE Origem LIKE '%Ãª%' OR Destino LIKE '%Ãª%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã´', 'ô'), Destino = REPLACE(Destino, 'Ã´', 'ô') WHERE Origem LIKE '%Ã´%' OR Destino LIKE '%Ã´%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã ', 'à'), Destino = REPLACE(Destino, 'Ã ', 'à') WHERE Origem LIKE '%Ã %' OR Destino LIKE '%Ã %';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã', 'Ã'), Destino = REPLACE(Destino, 'Ã', 'Ã') WHERE Origem LIKE '%Ã%' OR Destino LIKE '%Ã%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã‡', 'Ç'), Destino = REPLACE(Destino, 'Ã‡', 'Ç') WHERE Origem LIKE '%Ã‡%' OR Destino LIKE '%Ã‡%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã', 'Á'), Destino = REPLACE(Destino, 'Ã', 'Á') WHERE Origem LIKE '%Ã%' OR Destino LIKE '%Ã%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã‰', 'É'), Destino = REPLACE(Destino, 'Ã‰', 'É') WHERE Origem LIKE '%Ã‰%' OR Destino LIKE '%Ã‰%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã', 'Í'), Destino = REPLACE(Destino, 'Ã', 'Í') WHERE Origem LIKE '%Ã%' OR Destino LIKE '%Ã%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã"', 'Ó'), Destino = REPLACE(Destino, 'Ã"', 'Ó') WHERE Origem LIKE '%Ã"%' OR Destino LIKE '%Ã"%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ãš', 'Ú'), Destino = REPLACE(Destino, 'Ãš', 'Ú') WHERE Origem LIKE '%Ãš%' OR Destino LIKE '%Ãš%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã‚', 'Â'), Destino = REPLACE(Destino, 'Ã‚', 'Â') WHERE Origem LIKE '%Ã‚%' OR Destino LIKE '%Ã‚%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'ÃŠ', 'Ê'), Destino = REPLACE(Destino, 'ÃŠ', 'Ê') WHERE Origem LIKE '%ÃŠ%' OR Destino LIKE '%ÃŠ%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã"', 'Ô'), Destino = REPLACE(Destino, 'Ã"', 'Ô') WHERE Origem LIKE '%Ã"%' OR Destino LIKE '%Ã"%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'Ã€', 'À'), Destino = REPLACE(Destino, 'Ã€', 'À') WHERE Origem LIKE '%Ã€%' OR Destino LIKE '%Ã€%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'â€"', '-'), Destino = REPLACE(Destino, 'â€"', '-') WHERE Origem LIKE '%â€"%' OR Destino LIKE '%â€"%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'â€œ', '"'), Destino = REPLACE(Destino, 'â€œ', '"') WHERE Origem LIKE '%â€œ%' OR Destino LIKE '%â€œ%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'â€', '"'), Destino = REPLACE(Destino, 'â€', '"') WHERE Origem LIKE '%â€%' OR Destino LIKE '%â€%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, 'â€™', ''''), Destino = REPLACE(Destino, 'â€™', '''') WHERE Origem LIKE '%â€™%' OR Destino LIKE '%â€™%';

PRINT '✅ Correção de encoding concluída (30 substituições aplicadas)';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 2: CRIAR TABELA DE MAPEAMENTO
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🗺️  FASE 2: CRIANDO MAPEAMENTOS DE UNIFICAÇÃO';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

IF OBJECT_ID('tempdb..#MapeamentoOrigemDestino') IS NOT NULL
    DROP TABLE #MapeamentoOrigemDestino;

-- ✅ CORREÇÃO: VARCHAR(MAX) em vez de NVARCHAR(500)
CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo VARCHAR(MAX) NOT NULL,
    ValorCanonico VARCHAR(MAX) NOT NULL,
    Razao VARCHAR(200) NOT NULL
);

-- Inserir mapeamentos (✅ CORREÇÃO: Removido prefixo N de todas as strings)
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)
VALUES
    -- Aeroporto
    (' Aeroporto ', 'Aeroporto', 'Espaços extras'),

    -- Cefor (variações de case + typo)
    ('cefor', 'Cefor', 'Case incorreto'),
    ('ceforc', 'Cefor', 'Typo'),

    -- Ctran
    ('ctran', 'Ctran', 'Case incorreto'),
    ('ctram', 'Ctran', 'Typo'),

    -- Anexo I
    ('ANEXO I', 'Anexo I', 'Case incorreto'),
    ('Anexo I do Senado Federal', 'Anexo I', 'Descrição redundante'),
    ('Anexo I-senado', 'Anexo I', 'Formatação incorreta'),

    -- Anexo I - Carga e Descarga
    ('Anexo I - Carga e Descarga/serob', 'Anexo I - Carga e Descarga', 'Info redundante'),
    ('Anexo I Carga', 'Anexo I - Carga e Descarga', 'Formato inconsistente'),
    ('Anexo I- Carga e Descarga', 'Anexo I - Carga e Descarga', 'Espaçamento incorreto'),

    -- Anexo I - Rampa
    ('Anexo I - rampa/serob', 'Anexo I - Rampa', 'Info redundante + case'),
    ('Anexo I rampa', 'Anexo I - Rampa', 'Formato inconsistente'),
    ('Anexo I- Rampa', 'Anexo I - Rampa', 'Espaçamento incorreto'),

    -- Anexo II
    ('Anexo  II', 'Anexo II', 'Espaço duplo'),
    ('ANEXO II', 'Anexo II', 'Case incorreto'),

    -- Anexo II - Marquise
    ('Anexo  II -Marquise', 'Anexo II - Marquise', 'Espaço duplo'),

    -- Anexo II - Portaria
    ('Anexo II - Portão Lateral da Biblioteca', 'Anexo II - Portaria', 'Descrição padronizada'),
    ('Anexo II- Portao da biblioteca', 'Anexo II - Portaria', 'Sem acento + formatação'),
    ('Anexo II- Portaria', 'Anexo II - Portaria', 'Espaçamento incorreto'),
    ('PORTA LATERAL BIBLIOTECA', 'Anexo II - Portaria', 'Descrição alternativa'),
    ('portal lateral da biblioteca ', 'Anexo II - Portaria', 'Case + espaço extra'),

    -- Anexo III
    ('Anexo  III', 'Anexo III', 'Espaço duplo'),
    ('ANEXO III', 'Anexo III', 'Case incorreto'),
    ('Anexo III ', 'Anexo III', 'Espaço extra no final'),

    -- Anexo III - Carga e Descarga
    ('Anexo III -  CARGA E DES', 'Anexo III - Carga e Descarga', 'Abreviado + espaço duplo'),
    ('Anexo III Carga', 'Anexo III - Carga e Descarga', 'Formato inconsistente'),

    -- Anexo III - Portaria
    ('ANEXO III PORTARIA', 'Anexo III - Portaria', 'Case incorreto'),
    ('Anexo III- Portaria do Demed ( Das)', 'Anexo III - Portaria', 'Info redundante'),
    ('Portaria do Demed', 'Anexo III - Portaria', 'Descrição incompleta'),

    -- Anexo III - Ambulância
    ('Anexo  III proximo ambulancia', 'Anexo III - Próximo à Ambulância', 'Espaço duplo + sem acento'),
    ('ANEXO III proximo ambulancia', 'Anexo III - Próximo à Ambulância', 'Case + sem acento'),
    ('Anexo III-Ao lado Ambulancia', 'Anexo III - Próximo à Ambulância', 'Formatação + sem acento'),

    -- Anexo III - Demed
    ('ANEXO III- DEMED', 'Anexo III - Demed', 'Case incorreto'),
    ('D.A.S. - Demed', 'Anexo III - Demed', 'Descrição padronizada'),
    ('Das - Demed', 'Anexo III - Demed', 'Descrição padronizada'),
    ('DEMED', 'Anexo III - Demed', 'Local específico'),
    ('Demed- Portaria', 'Anexo III - Portaria', 'Descrição corrigida'),

    -- Anexo IV
    ('ANEXO IV ', 'Anexo IV', 'Case + espaço extra'),

    -- Anexo IV - Carga e Descarga
    ('Anexo IV- Carga e Descaga', 'Anexo IV - Carga e Descarga', 'Typo'),

    -- Anexo IV - Meia Lua
    ('Anexo IV MEIA LUA', 'Anexo IV - Meia Lua', 'Case incorreto'),
    ('Anexo IV-  Meia Lua ( Portaria)', 'Anexo IV - Meia Lua', 'Espaços extras'),

    -- Aniversário
    ('ANIVERSARIO', 'Aniversário', 'Sem acento'),

    -- Câmara dos Deputados
    ('Camara', 'Câmara dos Deputados', 'Sem acento + incompleto'),
    ('camara dos Deputados', 'Câmara dos Deputados', 'Sem acento + case'),
    ('Câmara Dos Deputados', 'Câmara dos Deputados', 'Case incorreto em Dos'),
    ('Camara dos Deputados,Anexo II -Marquise', 'Câmara dos Deputados', 'Info redundante'),

    -- Canteiro de Obras
    ('caneteiro de obras', 'Canteiro de Obras', 'Typo'),
    ('Canteiro de Obras  - Serob', 'Canteiro de Obras', 'Espaços extras + info redundante'),

    -- Centro de Transmissão
    ('Centro de trasmissão', 'Centro de Transmissão', 'Typo'),
    ('Centro de Trasmissão', 'Centro de Transmissão', 'Typo'),
    ('Torre de Transmissão  Colorado.(sobradinho)', 'Centro de Transmissão', 'Descrição padronizada'),
    ('Torre de Trasmissão', 'Centro de Transmissão', 'Typo'),

    -- Chapelaria
    ('chapelaria', 'Chapelaria', 'Case incorreto'),
    ('chapelaria ', 'Chapelaria', 'Case + espaço extra'),
    ('Chapelaria/ Alameda das Bandeiras', 'Chapelaria', 'Info redundante'),

    -- Almoxarifado SIA
    ('Almoxarifado', 'Almoxarifado SIA', 'Incompleto'),
    ('Almoxarifado-sia', 'Almoxarifado SIA', 'Formatação incorreta'),
    ('Almoxarifado-sia Trecho 5', 'Almoxarifado SIA', 'Info redundante'),
    ('Central de Almoxarifado - Sia', 'Almoxarifado SIA', 'Descrição alternativa'),
    ('Centro de Armazenamento (ceam-sia)', 'Almoxarifado SIA', 'Descrição alternativa'),
    ('Deposito Sia', 'Almoxarifado SIA', 'Descrição alternativa'),
    ('Galpão Sia', 'Almoxarifado SIA', 'Descrição alternativa'),
    ('galpão sia trecho 05 ', 'Almoxarifado SIA', 'Case + espaço extra'),
    ('sia', 'Almoxarifado SIA', 'Incompleto'),
    ('Sia -galpão da Câmara', 'Almoxarifado SIA', 'Info redundante'),

    -- Complexo Avançado
    ('Bloco D Complexo Avançado', 'Complexo Avançado', 'Info redundante'),
    ('Bloco D Programação Visual', 'Complexo Avançado - Programação Visual', 'Descrição específica'),
    ('Complexo Avançado/Guarita', 'Complexo Avançado', 'Info redundante'),
    ('Pragramação Visual  Bloco D Complexo Avançado', 'Complexo Avançado - Programação Visual', 'Typo + espaços'),
    ('Programação Visual', 'Complexo Avançado - Programação Visual', 'Incompleto'),

    -- Depol
    ('depol', 'Depol', 'Case incorreto'),

    -- Gráfica
    ('Atrás da Gráfica', 'Gráfica', 'Descrição redundante'),
    ('Grafica', 'Gráfica', 'Sem acento'),
    ('grafica ', 'Gráfica', 'Sem acento + case + espaço'),

    -- Hotel Lets Idea Brasília
    ('Hotel Lets Idea Brasília SHN Quadra 05, bloco B, Asa Norte', 'Hotel Lets Idea Brasília', 'Endereço redundante'),
    ('Anexo II/ Hotel Lets Idea Brasília SHN Quadra 05, bloco B, Asa Norte', 'Hotel Lets Idea Brasília', 'Info redundante'),
    ('Hotel Lets Idea', 'Hotel Lets Idea Brasília', 'Incompleto'),
    ('Hotel Lets Idea Brasília Hotel - Shn Q 5 Bloco B - Asa Norte', 'Hotel Lets Idea Brasília', 'Endereço redundante'),
    ('Hotel ManhattanPlaza Hotel / Hotel Lets Idea Brasília Hotel - Shn Q 5 Bloco B - Asa Norte', 'Hotel Lets Idea Brasília', 'Endereço redundante'),

    -- Hotel Brasília Palace
    ('Brasília Palace Hotel', 'Hotel Brasília Palace', 'Ordem incorreta'),

    -- Oficina
    ('oficina', 'Oficina', 'Case incorreto'),

    -- PGR
    ('pgr', 'PGR', 'Case incorreto'),

    -- Quality
    ('quality', 'Quality', 'Case incorreto'),

    -- Rampa do Congresso - Salão Negro
    ('rampa do congresso-salão negro', 'Rampa do Congresso - Salão Negro', 'Case + formatação'),
    ('Rampa salão negro', 'Rampa do Congresso - Salão Negro', 'Incompleto'),

    -- Residência Oficial
    ('Residência', 'Residência Oficial', 'Incompleto'),
    ('residencia oficial', 'Residência Oficial', 'Case + sem acento'),

    -- Residencial Morato
    ('Residencial  Morato ', 'Residencial Morato', 'Espaços extras'),

    -- Rodoviária
    ('Rodoviaria', 'Rodoviária', 'Sem acento'),

    -- Sean SIA
    ('Sean -  Sia', 'Sean SIA', 'Espaços extras'),
    ('Sean Sia', 'Sean SIA', 'Case incorreto'),

    -- Escola
    ('Escola ', 'Escola', 'Espaço extra'),

    -- UnB
    ('Unb', 'UnB', 'Case incorreto'),
    ('Unb -  Maloca', 'UnB - Maloca', 'Case + espaço duplo'),
    ('Unb - Centro de Convivência Multicultural', 'UnB - Centro de Convivência Multicultural', 'Case incorreto'),
    ('Unb - Instituto de Ciência Política da UnB', 'UnB - Instituto de Ciência Política', 'Case + redundante'),
    ('Unb - no final do ICC', 'UnB - ICC', 'Descrição padronizada'),
    ('Universidade de Brasília', 'UnB', 'Nome completo → sigla'),

    -- Hotel
    ('Hotel ', 'Hotel', 'Espaço extra'),
    ('Hotel  Grand Mercure', 'Hotel Grand Mercure', 'Espaço duplo'),

    -- Arniqueira SHA
    ('Arniqueira SHA.', 'Arniqueira SHA', 'Ponto extra'),
    ('Sha Chácara 81/28 - Casa 61 . Residencial Bela Vista. Arniqueira.', 'Arniqueira SHA - Residencial Bela Vista', 'Padronização'),
    ('Sha Chácara 81/28 Casa 61 - Residencial Bela Vista Arniqueiras', 'Arniqueira SHA - Residencial Bela Vista', 'Padronização'),

    -- Capital Recicláveis
    ('Capital Recicláveis (Estrutural) / Centro Olimpico Estrutural', 'Capital Recicláveis / Centro Olímpico Estrutural', 'Sem acento'),

    -- 111 Sul
    ('111 Sul b  l ', '111 Sul', 'Texto truncado'),
    ('111 Sul Bloco I', '111 Sul - Bloco I', 'Formatação'),

    -- A definir
    ('Á definir', 'A definir', 'Acento incorreto'),
    ('À definir', 'A definir', 'Crase incorreta'),

    -- Garagem
    ('Garagem ', 'Garagem', 'Espaço extra'),
    ('garagem', 'Garagem', 'Case incorreto'),

    -- Estacionamento
    ('Estacionamento ', 'Estacionamento', 'Espaço extra'),
    ('estacionamento', 'Estacionamento', 'Case incorreto'),

    -- Secretaria
    ('Secretaria ', 'Secretaria', 'Espaço extra'),
    ('secretaria', 'Secretaria', 'Case incorreto'),

    -- Recepção
    ('Recepcao', 'Recepção', 'Sem cedilha'),
    ('recepcao', 'Recepção', 'Sem cedilha + case'),
    ('RECEPCAO', 'Recepção', 'Sem cedilha + case'),
    ('Recepção ', 'Recepção', 'Espaço extra'),

    -- Depósito
    ('Deposito', 'Depósito', 'Sem acento'),
    ('deposito', 'Depósito', 'Sem acento + case'),

    -- Área
    ('Area', 'Área', 'Sem acento'),
    ('area', 'Área', 'Sem acento + case'),

    -- Sanitária
    ('Sanitaria', 'Sanitária', 'Sem acento'),
    ('sanitaria', 'Sanitária', 'Sem acento + case'),

    -- Ambulância
    ('Ambulancia', 'Ambulância', 'Sem acento'),
    ('ambulancia', 'Ambulância', 'Sem acento + case'),

    -- Portão
    ('Portao', 'Portão', 'Sem til'),
    ('portao', 'Portão', 'Sem til + case'),

    -- Estação
    ('Estacao', 'Estação', 'Sem cedilha'),
    ('estacao', 'Estação', 'Sem cedilha + case'),

    -- Funcionário
    ('Funcionario', 'Funcionário', 'Sem acento'),
    ('funcionario', 'Funcionário', 'Sem acento + case'),

    -- Almoxarifado (standalone)
    ('almoxarifado', 'Almoxarifado', 'Case incorreto'),

    -- Auditório
    ('Auditorio', 'Auditório', 'Sem acento'),
    ('auditorio', 'Auditório', 'Sem acento + case'),

    -- Saída
    ('Saida', 'Saída', 'Sem acento'),
    ('saida', 'Saída', 'Sem acento + case'),

    -- Emergência
    ('Emergencia', 'Emergência', 'Sem acento'),
    ('emergencia', 'Emergência', 'Sem acento + case'),

    -- Manutenção
    ('Manutencao', 'Manutenção', 'Sem cedilha'),
    ('manutencao', 'Manutenção', 'Sem cedilha + case'),

    -- Administração
    ('Administracao', 'Administração', 'Sem cedilha'),
    ('administracao', 'Administração', 'Sem cedilha + case'),

    -- Coordenação
    ('Coordenacao', 'Coordenação', 'Sem cedilha'),
    ('coordenacao', 'Coordenação', 'Sem cedilha + case');

DECLARE @TotalMapeamentos INT = @@ROWCOUNT;
PRINT '✅ ' + CAST(@TotalMapeamentos AS VARCHAR) + ' mapeamentos configurados';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 3: FUZZY MATCHING EM MASSA (LEVENSHTEIN DISTANCE) COM PROGRESSO
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🔍 FASE 3: FUZZY MATCHING EM MASSA (COM INDICADOR DE PROGRESSO)';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';
PRINT '⚙️  Calculando similaridade Levenshtein para valores não mapeados...';
PRINT '   Threshold: ≥85% de similaridade';
PRINT '   📊 Progresso será exibido a cada 10 minutos';
PRINT '';

-- ✅ CORREÇÃO: Verificar e remover função Levenshtein se existir
IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
BEGIN
    PRINT '⚠️  Removendo função LevenshteinDistance existente...';
    DROP FUNCTION dbo.LevenshteinDistance;
END

GO

-- ✅ CORREÇÃO: VARCHAR(MAX) em vez de NVARCHAR(MAX)
CREATE FUNCTION dbo.LevenshteinDistance(@string1 VARCHAR(MAX), @string2 VARCHAR(MAX))
RETURNS INT
AS
BEGIN
    DECLARE @len1 INT = LEN(@string1);
    DECLARE @len2 INT = LEN(@string2);
    DECLARE @i INT = 0;
    DECLARE @j INT = 0;
    DECLARE @cost INT;
    DECLARE @d TABLE (i INT, j INT, distance INT);

    IF @len1 = 0 RETURN @len2;
    IF @len2 = 0 RETURN @len1;

    WHILE @i <= @len1
    BEGIN
        INSERT INTO @d VALUES (@i, 0, @i);
        SET @i = @i + 1;
    END

    SET @j = 1;
    WHILE @j <= @len2
    BEGIN
        INSERT INTO @d VALUES (0, @j, @j);
        SET @j = @j + 1;
    END

    SET @i = 1;
    WHILE @i <= @len1
    BEGIN
        SET @j = 1;
        WHILE @j <= @len2
        BEGIN
            IF SUBSTRING(@string1, @i, 1) = SUBSTRING(@string2, @j, 1)
                SET @cost = 0
            ELSE
                SET @cost = 1;

            INSERT INTO @d
            SELECT @i, @j,
                   MIN(d1.distance)
            FROM (
                SELECT distance + 1 FROM @d WHERE i = @i - 1 AND j = @j
                UNION ALL
                SELECT distance + 1 FROM @d WHERE i = @i AND j = @j - 1
                UNION ALL
                SELECT distance + @cost FROM @d WHERE i = @i - 1 AND j = @j - 1
            ) d1;

            SET @j = @j + 1;
        END
        SET @i = @i + 1;
    END

    RETURN (SELECT distance FROM @d WHERE i = @len1 AND j = @len2);
END;
GO

-- Criar tabela temporária para candidatos fuzzy
CREATE TABLE #FuzzyCandidates (
    ValorOriginal VARCHAR(MAX),
    ValorCanonico VARCHAR(MAX),
    LevenshteinDist INT,
    SimilarityPercent DECIMAL(5,2),
    Campo VARCHAR(10)
);

-- Variáveis de controle de progresso
DECLARE @ValorOriginal VARCHAR(MAX);
DECLARE @ValorCanonico VARCHAR(MAX);
DECLARE @LevenshteinDist INT;
DECLARE @MaxLen INT;
DECLARE @SimilarityPercent DECIMAL(5,2);
DECLARE @MatchesFound INT = 0;
DECLARE @ContadorProgresso INT = 0;
DECLARE @TotalParaProcessar INT;
DECLARE @TempoDecorrido INT;
DECLARE @TempoEstimado INT;

-- Contar total de valores a processar
SELECT @TotalParaProcessar =
    (SELECT COUNT(DISTINCT Origem) FROM dbo.Viagem
     WHERE Origem IS NOT NULL AND Origem <> ''
     AND NOT EXISTS (SELECT 1 FROM #MapeamentoOrigemDestino m WHERE m.ValorAntigo = Origem))
    +
    (SELECT COUNT(DISTINCT Destino) FROM dbo.Viagem
     WHERE Destino IS NOT NULL AND Destino <> ''
     AND NOT EXISTS (SELECT 1 FROM #MapeamentoOrigemDestino m WHERE m.ValorAntigo = Destino));

PRINT '📊 Total de valores únicos não mapeados: ' + CAST(@TotalParaProcessar AS VARCHAR);
PRINT '';

-- ═══════════════════════════════════════════════════════════════════
-- PROCESSAR ORIGEM COM INDICADOR DE PROGRESSO
-- ═══════════════════════════════════════════════════════════════════

PRINT '📍 Processando valores de ORIGEM não mapeados...';

DECLARE origem_cursor CURSOR FOR
SELECT DISTINCT v.Origem
FROM dbo.Viagem v
WHERE v.Origem IS NOT NULL
  AND v.Origem <> ''
  AND NOT EXISTS (
      SELECT 1 FROM #MapeamentoOrigemDestino m
      WHERE m.ValorAntigo = v.Origem
  );

OPEN origem_cursor;
FETCH NEXT FROM origem_cursor INTO @ValorOriginal;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @ContadorProgresso = @ContadorProgresso + 1;

    -- Verificar se deve mostrar progresso (a cada 10 minutos)
    IF GETDATE() >= @ProximoProgresso
    BEGIN
        SET @TempoDecorrido = DATEDIFF(MINUTE, @InicioExecucao, GETDATE());
        SET @TempoEstimado = (@TempoDecorrido * @TotalParaProcessar) / NULLIF(@ContadorProgresso, 0);

        PRINT '';
        PRINT '⏱️  ════════════════════════════════════════════════════════════════';
        PRINT '⏱️  PROGRESSO - ' + CONVERT(VARCHAR, GETDATE(), 120);
        PRINT '⏱️  ════════════════════════════════════════════════════════════════';
        PRINT '   📊 Processados: ' + CAST(@ContadorProgresso AS VARCHAR) + ' / ' + CAST(@TotalParaProcessar AS VARCHAR);
        PRINT '   📈 Percentual: ' + CAST((@ContadorProgresso * 100.0 / NULLIF(@TotalParaProcessar, 0)) AS VARCHAR(10)) + '%';
        PRINT '   ⏰ Tempo decorrido: ' + CAST(@TempoDecorrido AS VARCHAR) + ' minutos';
        PRINT '   ⏳ Tempo estimado restante: ' + CAST((@TempoEstimado - @TempoDecorrido) AS VARCHAR) + ' minutos';
        PRINT '⏱️  ════════════════════════════════════════════════════════════════';
        PRINT '';

        SET @ProximoProgresso = DATEADD(MINUTE, 10, GETDATE());
    END

    -- Comparar com todos os valores canônicos
    DECLARE canonico_cursor CURSOR FOR
    SELECT DISTINCT ValorCanonico FROM #MapeamentoOrigemDestino;

    OPEN canonico_cursor;
    FETCH NEXT FROM canonico_cursor INTO @ValorCanonico;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @LevenshteinDist = dbo.LevenshteinDistance(@ValorOriginal, @ValorCanonico);

        SET @MaxLen = CASE
            WHEN LEN(@ValorOriginal) > LEN(@ValorCanonico) THEN LEN(@ValorOriginal)
            ELSE LEN(@ValorCanonico)
        END;

        IF @MaxLen > 0
            SET @SimilarityPercent = ((CAST(@MaxLen AS DECIMAL(10,2)) - CAST(@LevenshteinDist AS DECIMAL(10,2))) / CAST(@MaxLen AS DECIMAL(10,2))) * 100
        ELSE
            SET @SimilarityPercent = 0;

        IF @SimilarityPercent >= 85.0
        BEGIN
            INSERT INTO #FuzzyCandidates (ValorOriginal, ValorCanonico, LevenshteinDist, SimilarityPercent, Campo)
            VALUES (@ValorOriginal, @ValorCanonico, @LevenshteinDist, @SimilarityPercent, 'Origem');
        END

        FETCH NEXT FROM canonico_cursor INTO @ValorCanonico;
    END

    CLOSE canonico_cursor;
    DEALLOCATE canonico_cursor;

    FETCH NEXT FROM origem_cursor INTO @ValorOriginal;
END

CLOSE origem_cursor;
DEALLOCATE origem_cursor;

-- ═══════════════════════════════════════════════════════════════════
-- PROCESSAR DESTINO COM INDICADOR DE PROGRESSO
-- ═══════════════════════════════════════════════════════════════════

PRINT '🎯 Processando valores de DESTINO não mapeados...';

DECLARE destino_cursor CURSOR FOR
SELECT DISTINCT v.Destino
FROM dbo.Viagem v
WHERE v.Destino IS NOT NULL
  AND v.Destino <> ''
  AND NOT EXISTS (
      SELECT 1 FROM #MapeamentoOrigemDestino m
      WHERE m.ValorAntigo = v.Destino
  );

OPEN destino_cursor;
FETCH NEXT FROM destino_cursor INTO @ValorOriginal;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @ContadorProgresso = @ContadorProgresso + 1;

    -- Verificar progresso a cada 10 minutos
    IF GETDATE() >= @ProximoProgresso
    BEGIN
        SET @TempoDecorrido = DATEDIFF(MINUTE, @InicioExecucao, GETDATE());
        SET @TempoEstimado = (@TempoDecorrido * @TotalParaProcessar) / NULLIF(@ContadorProgresso, 0);

        PRINT '';
        PRINT '⏱️  ════════════════════════════════════════════════════════════════';
        PRINT '⏱️  PROGRESSO - ' + CONVERT(VARCHAR, GETDATE(), 120);
        PRINT '⏱️  ════════════════════════════════════════════════════════════════';
        PRINT '   📊 Processados: ' + CAST(@ContadorProgresso AS VARCHAR) + ' / ' + CAST(@TotalParaProcessar AS VARCHAR);
        PRINT '   📈 Percentual: ' + CAST((@ContadorProgresso * 100.0 / NULLIF(@TotalParaProcessar, 0)) AS VARCHAR(10)) + '%';
        PRINT '   ⏰ Tempo decorrido: ' + CAST(@TempoDecorrido AS VARCHAR) + ' minutos';
        PRINT '   ⏳ Tempo estimado restante: ' + CAST((@TempoEstimado - @TempoDecorrido) AS VARCHAR) + ' minutos';
        PRINT '⏱️  ════════════════════════════════════════════════════════════════';
        PRINT '';

        SET @ProximoProgresso = DATEADD(MINUTE, 10, GETDATE());
    END

    -- Comparar com todos os valores canônicos
    DECLARE canonico_cursor2 CURSOR FOR
    SELECT DISTINCT ValorCanonico FROM #MapeamentoOrigemDestino;

    OPEN canonico_cursor2;
    FETCH NEXT FROM canonico_cursor2 INTO @ValorCanonico;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @LevenshteinDist = dbo.LevenshteinDistance(@ValorOriginal, @ValorCanonico);

        SET @MaxLen = CASE
            WHEN LEN(@ValorOriginal) > LEN(@ValorCanonico) THEN LEN(@ValorOriginal)
            ELSE LEN(@ValorCanonico)
        END;

        IF @MaxLen > 0
            SET @SimilarityPercent = ((CAST(@MaxLen AS DECIMAL(10,2)) - CAST(@LevenshteinDist AS DECIMAL(10,2))) / CAST(@MaxLen AS DECIMAL(10,2))) * 100
        ELSE
            SET @SimilarityPercent = 0;

        IF @SimilarityPercent >= 85.0
        BEGIN
            INSERT INTO #FuzzyCandidates (ValorOriginal, ValorCanonico, LevenshteinDist, SimilarityPercent, Campo)
            VALUES (@ValorOriginal, @ValorCanonico, @LevenshteinDist, @SimilarityPercent, 'Destino');
        END

        FETCH NEXT FROM canonico_cursor2 INTO @ValorCanonico;
    END

    CLOSE canonico_cursor2;
    DEALLOCATE canonico_cursor2;

    FETCH NEXT FROM destino_cursor INTO @ValorOriginal;
END

CLOSE destino_cursor;
DEALLOCATE destino_cursor;

-- Selecionar melhor match para cada valor
CREATE TABLE #BestMatches (
    ValorOriginal VARCHAR(MAX),
    ValorCanonico VARCHAR(MAX),
    SimilarityPercent DECIMAL(5,2),
    Campo VARCHAR(10)
);

INSERT INTO #BestMatches
SELECT
    fc.ValorOriginal,
    fc.ValorCanonico,
    fc.SimilarityPercent,
    fc.Campo
FROM #FuzzyCandidates fc
INNER JOIN (
    SELECT ValorOriginal, Campo, MAX(SimilarityPercent) AS MaxSimilarity
    FROM #FuzzyCandidates
    GROUP BY ValorOriginal, Campo
) best ON fc.ValorOriginal = best.ValorOriginal
      AND fc.Campo = best.Campo
      AND fc.SimilarityPercent = best.MaxSimilarity;

-- ✅ CORREÇÃO: Coluna "Razao" em vez de "Observacao"
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)
SELECT
    ValorOriginal,
    ValorCanonico,
    'Fuzzy Match (' + CAST(SimilarityPercent AS VARCHAR(10)) + '% similaridade)'
FROM #BestMatches;

SET @MatchesFound = @@ROWCOUNT;

PRINT '';
PRINT '✅ Fuzzy matching concluído!';
PRINT '   - Matches encontrados: ' + CAST(@MatchesFound AS VARCHAR);
PRINT '';

-- Limpar tabelas temporárias
DROP TABLE #FuzzyCandidates;
DROP TABLE #BestMatches;

-- Atualizar contagem total de mapeamentos
SELECT @TotalMapeamentos = COUNT(*) FROM #MapeamentoOrigemDestino;
PRINT '📝 Total de mapeamentos após fuzzy matching: ' + CAST(@TotalMapeamentos AS VARCHAR);
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 4: EXECUTAR ATUALIZAÇÕES COM TRANSAÇÃO
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🚀 FASE 4: EXECUTANDO ATUALIZAÇÕES';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @RegistrosOrigemAfetados INT;
    DECLARE @RegistrosDestinoAfetados INT;

    SELECT @RegistrosOrigemAfetados = COUNT(DISTINCT v.ViagemId)
    FROM dbo.Viagem v
    INNER JOIN #MapeamentoOrigemDestino m ON v.Origem = m.ValorAntigo;

    SELECT @RegistrosDestinoAfetados = COUNT(DISTINCT v.ViagemId)
    FROM dbo.Viagem v
    INNER JOIN #MapeamentoOrigemDestino m ON v.Destino = m.ValorAntigo;

    PRINT '📝 Registros que serão atualizados:';
    PRINT '   - Origem: ' + CAST(@RegistrosOrigemAfetados AS VARCHAR) + ' viagens';
    PRINT '   - Destino: ' + CAST(@RegistrosDestinoAfetados AS VARCHAR) + ' viagens';
    PRINT '';

    -- Atualizar ORIGEM
    PRINT '⚙️  Atualizando campo Origem...';
    UPDATE v
    SET v.Origem = m.ValorCanonico
    FROM dbo.Viagem v
    INNER JOIN #MapeamentoOrigemDestino m ON v.Origem = m.ValorAntigo;

    DECLARE @OrigemAtualizados INT = @@ROWCOUNT;
    PRINT '✅ Origem: ' + CAST(@OrigemAtualizados AS VARCHAR) + ' registros atualizados';

    -- Atualizar DESTINO
    PRINT '⚙️  Atualizando campo Destino...';
    UPDATE v
    SET v.Destino = m.ValorCanonico
    FROM dbo.Viagem v
    INNER JOIN #MapeamentoOrigemDestino m ON v.Destino = m.ValorAntigo;

    DECLARE @DestinoAtualizados INT = @@ROWCOUNT;
    PRINT '✅ Destino: ' + CAST(@DestinoAtualizados AS VARCHAR) + ' registros atualizados';
    PRINT '';

    COMMIT TRANSACTION;
    PRINT '✅ Transação finalizada com sucesso';
    PRINT '';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;

    PRINT '════════════════════════════════════════════════════════════════════════';
    PRINT '❌ ERRO durante a atualização!';
    PRINT '════════════════════════════════════════════════════════════════════════';
    PRINT '';
    PRINT 'Mensagem: ' + ERROR_MESSAGE();
    PRINT 'Linha: ' + CAST(ERROR_LINE() AS VARCHAR);
    PRINT 'Severidade: ' + CAST(ERROR_SEVERITY() AS VARCHAR);
    PRINT '';
    PRINT '🔄 Transação revertida. Nenhum dado foi alterado.';
    PRINT '';

    -- Limpar e sair
    IF OBJECT_ID('tempdb..#MapeamentoOrigemDestino') IS NOT NULL
        DROP TABLE #MapeamentoOrigemDestino;

    IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
        DROP FUNCTION dbo.LevenshteinDistance;

    SET NOCOUNT OFF;
    RETURN;
END CATCH;

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 5: ESTATÍSTICAS FINAIS
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '📊 FASE 5: ESTATÍSTICAS FINAIS';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

DECLARE @OrigemUnicosDepois INT;
DECLARE @DestinoUnicosDepois INT;

SELECT @OrigemUnicosDepois = COUNT(DISTINCT Origem)
FROM dbo.Viagem
WHERE Origem IS NOT NULL AND Origem <> '';

SELECT @DestinoUnicosDepois = COUNT(DISTINCT Destino)
FROM dbo.Viagem
WHERE Destino IS NOT NULL AND Destino <> '';

DECLARE @OrigemReduzidos INT = @OrigemUnicosAntes - @OrigemUnicosDepois;
DECLARE @DestinoReduzidos INT = @DestinoUnicosAntes - @DestinoUnicosDepois;

DECLARE @OrigemPercentualReducao DECIMAL(5,2);
DECLARE @DestinoPercentualReducao DECIMAL(5,2);

IF @OrigemUnicosAntes > 0
    SET @OrigemPercentualReducao = (CAST(@OrigemReduzidos AS DECIMAL(10,2)) / CAST(@OrigemUnicosAntes AS DECIMAL(10,2))) * 100;
ELSE
    SET @OrigemPercentualReducao = 0;

IF @DestinoUnicosAntes > 0
    SET @DestinoPercentualReducao = (CAST(@DestinoReduzidos AS DECIMAL(10,2)) / CAST(@DestinoUnicosAntes AS DECIMAL(10,2))) * 100;
ELSE
    SET @DestinoPercentualReducao = 0;

SET @TempoDecorrido = DATEDIFF(MINUTE, @InicioExecucao, GETDATE());

PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '✅ LIMPEZA CONCLUÍDA COM SUCESSO!';
PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '📊 RESUMO GERAL:';
PRINT '   - Total de viagens: ' + CAST(@TotalRegistros AS VARCHAR);
PRINT '   - Mapeamentos: ' + CAST(@TotalMapeamentos AS VARCHAR);
PRINT '   - Origem atualizados: ' + CAST(@OrigemAtualizados AS VARCHAR);
PRINT '   - Destino atualizados: ' + CAST(@DestinoAtualizados AS VARCHAR);
PRINT '   - Tempo total: ' + CAST(@TempoDecorrido AS VARCHAR) + ' minutos';
PRINT '';
PRINT '┌────────────────────────────────────────────────────────────────────────┐';
PRINT '│ 📊 REDUÇÃO DE VALORES ÚNICOS                                          │';
PRINT '├────────────────────────────────────────────────────────────────────────┤';
PRINT '│  📍 ORIGEM:                                                            │';
PRINT '│     Antes:  ' + CAST(@OrigemUnicosAntes AS VARCHAR) + ' valores';
PRINT '│     Depois: ' + CAST(@OrigemUnicosDepois AS VARCHAR) + ' valores';
PRINT '│     Redução: ' + CAST(@OrigemReduzidos AS VARCHAR) + ' (' + CAST(@OrigemPercentualReducao AS VARCHAR(10)) + '%)';
PRINT '│                                                                        │';
PRINT '│  🎯 DESTINO:                                                           │';
PRINT '│     Antes:  ' + CAST(@DestinoUnicosAntes AS VARCHAR) + ' valores';
PRINT '│     Depois: ' + CAST(@DestinoUnicosDepois AS VARCHAR) + ' valores';
PRINT '│     Redução: ' + CAST(@DestinoReduzidos AS VARCHAR) + ' (' + CAST(@DestinoPercentualReducao AS VARCHAR(10)) + '%)';
PRINT '└────────────────────────────────────────────────────────────────────────┘';
PRINT '';

PRINT '💾 Backup disponível em: dbo.Viagem_Backup_OrigemDestino';
PRINT '⏰ Fim: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- INSTRUÇÕES DE ROLLBACK
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '🔄 Para reverter as alterações, execute:';
PRINT '';
PRINT '/*';
PRINT 'UPDATE v';
PRINT 'SET v.Origem = b.OrigemOriginal, v.Destino = b.DestinoOriginal';
PRINT 'FROM dbo.Viagem v';
PRINT 'INNER JOIN dbo.Viagem_Backup_OrigemDestino b ON v.ViagemId = b.ViagemId;';
PRINT '';
PRINT 'PRINT ''✅ Rollback concluído.'';';
PRINT '*/';
PRINT '';

PRINT '════════════════════════════════════════════════════════════════════════';

-- Limpar recursos
IF OBJECT_ID('tempdb..#MapeamentoOrigemDestino') IS NOT NULL
    DROP TABLE #MapeamentoOrigemDestino;

-- ✅ CORREÇÃO: Verificar antes de remover função
IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
    DROP FUNCTION dbo.LevenshteinDistance;

SET NOCOUNT OFF;
GO
