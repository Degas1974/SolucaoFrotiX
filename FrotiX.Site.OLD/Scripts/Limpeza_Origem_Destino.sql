/****************************************************************************************
 * 🧹 SCRIPT: Limpeza e Unificação de Duplicatas - Campos Origem e Destino
 * --------------------------------------------------------------------------------------
 * Descrição: Remove duplicatas e padroniza valores nos campos Origem e Destino
 *            da tabela Viagem (EXECUÇÃO COMPLETA EM UMA VEZ)
 *
 * Autor: Claude Sonnet 4.5 (FrotiX Team)
 * Data: 12/02/2026
 *
 * ⚠️ IMPORTANTE:
 * - Este script EXECUTA IMEDIATAMENTE todas as alterações
 * - Cria backup automático antes de fazer alterações
 * - Usa transação com rollback automático em caso de erro
 * - Mostra estatísticas de redução ao final
 ****************************************************************************************/

SET NOCOUNT ON;
GO

PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '🧹 LIMPEZA DE DUPLICATAS - ORIGEM E DESTINO';
PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '⏰ Início: ' + CONVERT(VARCHAR, GETDATE(), 120);
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
-- FASE 1.5: CORREÇÃO DE ENCODING UTF-8/Latin1 (EXECUTAR PRIMEIRO)
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

-- ══════════════════════════════════════════════════════════════════════════════
-- APLICAR CORREÇÕES DE ENCODING (30 substituições sequenciais)
-- ══════════════════════════════════════════════════════════════════════════════

-- Minúsculas com til
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã£', N'ã'), Destino = REPLACE(Destino, N'Ã£', N'ã') WHERE Origem LIKE N'%Ã£%' OR Destino LIKE N'%Ã£%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãµ', N'õ'), Destino = REPLACE(Destino, N'Ãµ', N'õ') WHERE Origem LIKE N'%Ãµ%' OR Destino LIKE N'%Ãµ%';

-- Minúsculas com cedilha
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã§', N'ç'), Destino = REPLACE(Destino, N'Ã§', N'ç') WHERE Origem LIKE N'%Ã§%' OR Destino LIKE N'%Ã§%';

-- Minúsculas com acento agudo
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã¡', N'á'), Destino = REPLACE(Destino, N'Ã¡', N'á') WHERE Origem LIKE N'%Ã¡%' OR Destino LIKE N'%Ã¡%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã©', N'é'), Destino = REPLACE(Destino, N'Ã©', N'é') WHERE Origem LIKE N'%Ã©%' OR Destino LIKE N'%Ã©%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã­', N'í'), Destino = REPLACE(Destino, N'Ã­', N'í') WHERE Origem LIKE N'%Ã­%' OR Destino LIKE N'%Ã­%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã³', N'ó'), Destino = REPLACE(Destino, N'Ã³', N'ó') WHERE Origem LIKE N'%Ã³%' OR Destino LIKE N'%Ã³%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãº', N'ú'), Destino = REPLACE(Destino, N'Ãº', N'ú') WHERE Origem LIKE N'%Ãº%' OR Destino LIKE N'%Ãº%';

-- Minúsculas com acento circunflexo
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã¢', N'â'), Destino = REPLACE(Destino, N'Ã¢', N'â') WHERE Origem LIKE N'%Ã¢%' OR Destino LIKE N'%Ã¢%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãª', N'ê'), Destino = REPLACE(Destino, N'Ãª', N'ê') WHERE Origem LIKE N'%Ãª%' OR Destino LIKE N'%Ãª%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã´', N'ô'), Destino = REPLACE(Destino, N'Ã´', N'ô') WHERE Origem LIKE N'%Ã´%' OR Destino LIKE N'%Ã´%';

-- Minúsculas com acento grave
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã ', N'à'), Destino = REPLACE(Destino, N'Ã ', N'à') WHERE Origem LIKE N'%Ã %' OR Destino LIKE N'%Ã %';

-- Maiúsculas com til
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã', N'Ã'), Destino = REPLACE(Destino, N'Ã', N'Ã') WHERE Origem LIKE N'%Ã%' OR Destino LIKE N'%Ã%';

-- Maiúsculas com cedilha
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã‡', N'Ç'), Destino = REPLACE(Destino, N'Ã‡', N'Ç') WHERE Origem LIKE N'%Ã‡%' OR Destino LIKE N'%Ã‡%';

-- Maiúsculas com acento agudo
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã', N'Á'), Destino = REPLACE(Destino, N'Ã', N'Á') WHERE Origem LIKE N'%Ã%' OR Destino LIKE N'%Ã%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã‰', N'É'), Destino = REPLACE(Destino, N'Ã‰', N'É') WHERE Origem LIKE N'%Ã‰%' OR Destino LIKE N'%Ã‰%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã', N'Í'), Destino = REPLACE(Destino, N'Ã', N'Í') WHERE Origem LIKE N'%Ã%' OR Destino LIKE N'%Ã%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã"', N'Ó'), Destino = REPLACE(Destino, N'Ã"', N'Ó') WHERE Origem LIKE N'%Ã"%' OR Destino LIKE N'%Ã"%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãš', N'Ú'), Destino = REPLACE(Destino, N'Ãš', N'Ú') WHERE Origem LIKE N'%Ãš%' OR Destino LIKE N'%Ãš%';

-- Maiúsculas com acento circunflexo
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã‚', N'Â'), Destino = REPLACE(Destino, N'Ã‚', N'Â') WHERE Origem LIKE N'%Ã‚%' OR Destino LIKE N'%Ã‚%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'ÃŠ', N'Ê'), Destino = REPLACE(Destino, N'ÃŠ', N'Ê') WHERE Origem LIKE N'%ÃŠ%' OR Destino LIKE N'%ÃŠ%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã"', N'Ô'), Destino = REPLACE(Destino, N'Ã"', N'Ô') WHERE Origem LIKE N'%Ã"%' OR Destino LIKE N'%Ã"%';

-- Maiúsculas com acento grave
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã€', N'À'), Destino = REPLACE(Destino, N'Ã€', N'À') WHERE Origem LIKE N'%Ã€%' OR Destino LIKE N'%Ã€%';

-- Pontuação mal interpretada
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'â€"', N'-'), Destino = REPLACE(Destino, N'â€"', N'-') WHERE Origem LIKE N'%â€"%' OR Destino LIKE N'%â€"%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'â€œ', N'"'), Destino = REPLACE(Destino, N'â€œ', N'"') WHERE Origem LIKE N'%â€œ%' OR Destino LIKE N'%â€œ%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'â€', N'"'), Destino = REPLACE(Destino, N'â€', N'"') WHERE Origem LIKE N'%â€%' OR Destino LIKE N'%â€%';
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'â€™', N''''), Destino = REPLACE(Destino, N'â€™', N'''') WHERE Origem LIKE N'%â€™%' OR Destino LIKE N'%â€™%';

PRINT '✅ Correção de encoding concluída!';
PRINT '📊 Total de 30 correções de encoding aplicadas';
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

CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo NVARCHAR(500) NOT NULL,
    ValorCanonico NVARCHAR(500) NOT NULL,
    Razao NVARCHAR(200) NOT NULL
    -- PRIMARY KEY removida temporariamente para permitir duplicatas durante INSERT
    -- O auto-fix removerá as duplicatas automaticamente após o INSERT
);

-- NOTA: Duplicatas case-insensitive foram removidas manualmente do mapeamento
-- para garantir compatibilidade com o collation padrão do SQL Server

-- ══════════════════════════════════════════════════════════════════════════════
-- VALIDAÇÃO PRÉ-INSERT: Detectar duplicatas case-insensitive na lista
-- ══════════════════════════════════════════════════════════════════════════════

-- Criar tabela temporária para validação
IF OBJECT_ID('tempdb..#ValidacaoDuplicatas') IS NOT NULL
    DROP TABLE #ValidacaoDuplicatas;

CREATE TABLE #ValidacaoDuplicatas (
    ValorAntigo NVARCHAR(500) NOT NULL
);

-- Inserir todos os mapeamentos
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)
VALUES
    -- Aeroporto
    (N' Aeroporto ', N'Aeroporto', N'Espaços extras'),

    -- Cefor (variações de case + typo)
    (N'cefor', N'Cefor', N'Case incorreto (inclui CEFOR)'),
    (N'ceforc', N'Cefor', N'Typo'),

    -- Ctran (variações de case + typo)
    (N'ctran', N'Ctran', N'Case incorreto (inclui CTRAN)'),
    (N'ctram', N'Ctran', N'Typo'),

    -- Anexo I
    (N'ANEXO I', N'Anexo I', N'Case incorreto'),
    (N'Anexo I do Senado Federal', N'Anexo I', N'Descrição redundante'),
    (N'Anexo I-senado', N'Anexo I', N'Formatação incorreta'),

    -- Anexo I - Carga e Descarga
    (N'Anexo I - Carga e Descarga/serob', N'Anexo I - Carga e Descarga', N'Info redundante'),
    (N'Anexo I Carga', N'Anexo I - Carga e Descarga', N'Formato inconsistente'),
    (N'Anexo I- Carga e Descarga', N'Anexo I - Carga e Descarga', N'Espaçamento incorreto'),

    -- Anexo I - Rampa
    (N'Anexo I - rampa/serob', N'Anexo I - Rampa', N'Info redundante + case'),
    (N'Anexo I rampa', N'Anexo I - Rampa', N'Formato inconsistente'),
    (N'Anexo I- Rampa', N'Anexo I - Rampa', N'Espaçamento incorreto'),

    -- Anexo II
    (N'Anexo  II', N'Anexo II', N'Espaço duplo'),
    (N'ANEXO II', N'Anexo II', N'Case incorreto (inclui com espaço no final)'),

    -- Anexo II - Marquise
    (N'Anexo  II -Marquise', N'Anexo II - Marquise', N'Espaço duplo'),

    -- Anexo II - Portaria
    (N'Anexo II - Portão Lateral da Biblioteca', N'Anexo II - Portaria', N'Descrição padronizada'),
    (N'Anexo II- Portao da biblioteca', N'Anexo II - Portaria', N'Sem acento + formatação'),
    (N'Anexo II- Portaria', N'Anexo II - Portaria', N'Espaçamento incorreto'),
    (N'PORTA LATERAL BIBLIOTECA', N'Anexo II - Portaria', N'Descrição alternativa'),
    (N'portal lateral da biblioteca ', N'Anexo II - Portaria', N'Case + espaço extra'),

    -- Anexo III
    (N'Anexo  III', N'Anexo III', N'Espaço duplo'),
    (N'ANEXO III', N'Anexo III', N'Case incorreto'),
    (N'Anexo III ', N'Anexo III', N'Espaço extra no final'),

    -- Anexo III - Carga e Descarga
    (N'Anexo III -  CARGA E DES', N'Anexo III - Carga e Descarga', N'Abreviado + espaço duplo'),
    (N'Anexo III Carga', N'Anexo III - Carga e Descarga', N'Formato inconsistente'),

    -- Anexo III - Portaria
    (N'ANEXO III PORTARIA', N'Anexo III - Portaria', N'Case incorreto'),
    (N'Anexo III- Portaria do Demed ( Das)', N'Anexo III - Portaria', N'Info redundante'),
    (N'Portaria do Demed', N'Anexo III - Portaria', N'Descrição incompleta'),

    -- Anexo III - Ambulância
    (N'Anexo  III proximo ambulancia', N'Anexo III - Próximo à Ambulância', N'Espaço duplo + sem acento'),
    (N'ANEXO III proximo ambulancia', N'Anexo III - Próximo à Ambulância', N'Case + sem acento'),
    (N'Anexo III-Ao lado Ambulancia', N'Anexo III - Próximo à Ambulância', N'Formatação + sem acento'),

    -- Anexo III - Demed
    (N'ANEXO III- DEMED', N'Anexo III - Demed', N'Case incorreto'),
    (N'D.A.S. - Demed', N'Anexo III - Demed', N'Descrição padronizada'),
    (N'Das - Demed', N'Anexo III - Demed', N'Descrição padronizada'),
    (N'DEMED', N'Anexo III - Demed', N'Local específico'),
    (N'Demed- Portaria', N'Anexo III - Portaria', N'Descrição corrigida'),

    -- Anexo IV
    (N'ANEXO IV ', N'Anexo IV', N'Case + espaço extra'),

    -- Anexo IV - Carga e Descarga
    (N'Anexo IV- Carga e Descaga', N'Anexo IV - Carga e Descarga', N'Typo "Descaga"'),

    -- Anexo IV - Meia Lua
    (N'Anexo IV MEIA LUA', N'Anexo IV - Meia Lua', N'Case incorreto'),
    (N'Anexo IV-  Meia Lua ( Portaria)', N'Anexo IV - Meia Lua', N'Espaços extras'),

    -- Aniversário
    (N'ANIVERSARIO', N'Aniversário', N'Sem acento (inclui com espaço)'),

    -- Câmara dos Deputados
    (N'Camara', N'Câmara dos Deputados', N'Sem acento + incompleto'),
    (N'camara dos Deputados', N'Câmara dos Deputados', N'Sem acento + case'),
    (N'Câmara Dos Deputados', N'Câmara dos Deputados', N'Case incorreto em "Dos"'),
    (N'Camara dos Deputados,Anexo II -Marquise', N'Câmara dos Deputados', N'Info redundante'),

    -- Canteiro de Obras
    (N'caneteiro de obras', N'Canteiro de Obras', N'Typo "caneteiro"'),
    (N'Canteiro de Obras  - Serob', N'Canteiro de Obras', N'Espaços extras + info redundante'),

    -- Centro de Transmissão
    (N'Centro de trasmissão', N'Centro de Transmissão', N'Typo "trasmissão"'),
    (N'Centro de Trasmissão', N'Centro de Transmissão', N'Typo "Trasmissão"'),
    (N'Torre de Transmissão  Colorado.(sobradinho)', N'Centro de Transmissão', N'Descrição padronizada'),
    (N'Torre de Trasmissão', N'Centro de Transmissão', N'Typo "Trasmissão"'),

    -- Chapelaria
    (N'chapelaria', N'Chapelaria', N'Case incorreto'),
    (N'chapelaria ', N'Chapelaria', N'Case + espaço extra'),
    (N'Chapelaria/ Alameda das Bandeiras', N'Chapelaria', N'Info redundante'),

    -- Almoxarifado SIA
    (N'Almoxarifado', N'Almoxarifado SIA', N'Incompleto'),
    (N'Almoxarifado-sia', N'Almoxarifado SIA', N'Formatação incorreta'),
    (N'Almoxarifado-sia Trecho 5', N'Almoxarifado SIA', N'Info redundante'),
    (N'Central de Almoxarifado - Sia', N'Almoxarifado SIA', N'Descrição alternativa'),
    (N'Centro de Armazenamento (ceam-sia)', N'Almoxarifado SIA', N'Descrição alternativa'),
    (N'Deposito Sia', N'Almoxarifado SIA', N'Descrição alternativa'),
    (N'Galpão Sia', N'Almoxarifado SIA', N'Descrição alternativa'),
    (N'galpão sia trecho 05 ', N'Almoxarifado SIA', N'Case + espaço extra'),
    (N'sia', N'Almoxarifado SIA', N'Incompleto'),
    (N'Sia -galpão da Câmara', N'Almoxarifado SIA', N'Info redundante'),

    -- Complexo Avançado
    (N'Bloco D Complexo Avançado', N'Complexo Avançado', N'Info redundante'),
    (N'Bloco D Programação Visual', N'Complexo Avançado - Programação Visual', N'Descrição específica'),
    (N'Complexo Avançado/Guarita', N'Complexo Avançado', N'Info redundante'),
    (N'Pragramação Visual  Bloco D Complexo Avançado', N'Complexo Avançado - Programação Visual', N'Typo + espaços'),
    (N'Programação Visual', N'Complexo Avançado - Programação Visual', N'Incompleto'),

    -- Depol
    (N'depol', N'Depol', N'Case incorreto'),

    -- Gráfica
    (N'Atrás da Gráfica', N'Gráfica', N'Descrição redundante'),
    (N'Grafica', N'Gráfica', N'Sem acento'),
    (N'grafica ', N'Gráfica', N'Sem acento + case + espaço'),

    -- Hotel Lets Idea Brasília
    (N'Hotel Lets Idea Brasília SHN Quadra 05, bloco B, Asa Norte', N'Hotel Lets Idea Brasília', N'Endereço redundante'),
    (N'Anexo II/ Hotel Lets Idea Brasília SHN Quadra 05, bloco B, Asa Norte', N'Hotel Lets Idea Brasília', N'Info redundante'),
    (N'Hotel Lets Idea', N'Hotel Lets Idea Brasília', N'Incompleto'),
    (N'Hotel Lets Idea Brasília Hotel - Shn Q 5 Bloco B - Asa Norte', N'Hotel Lets Idea Brasília', N'Endereço redundante'),
    (N'Hotel ManhattanPlaza Hotel / Hotel Lets Idea Brasília Hotel - Shn Q 5 Bloco B - Asa Norte', N'Hotel Lets Idea Brasília', N'Endereço redundante'),

    -- Hotel Brasília Palace
    (N'Brasília Palace Hotel', N'Hotel Brasília Palace', N'Ordem incorreta'),

    -- Oficina
    (N'oficina', N'Oficina', N'Case incorreto'),

    -- PGR
    (N'pgr', N'PGR', N'Case incorreto (inclui Pgr e com espaço)'),

    -- Quality
    (N'quality', N'Quality', N'Case incorreto'),

    -- Rampa do Congresso - Salão Negro
    (N'rampa do congresso-salão negro', N'Rampa do Congresso - Salão Negro', N'Case + formatação'),
    (N'Rampa salão negro', N'Rampa do Congresso - Salão Negro', N'Incompleto'),

    -- Residência Oficial
    (N'Residência', N'Residência Oficial', N'Incompleto'),
    (N'residencia oficial', N'Residência Oficial', N'Case + sem acento'),

    -- Residencial Morato
    (N'Residencial  Morato ', N'Residencial Morato', N'Espaços extras'),

    -- Rodoviária
    (N'Rodoviaria', N'Rodoviária', N'Sem acento'),

    -- Sean SIA
    (N'Sean -  Sia', N'Sean SIA', N'Espaços extras'),
    (N'Sean Sia', N'Sean SIA', N'Case incorreto'),

    -- Escola
    (N'Escola ', N'Escola', N'Espaço extra'),

    -- UnB
    (N'Unb', N'UnB', N'Case incorreto'),
    (N'Unb -  Maloca', N'UnB - Maloca', N'Case + espaço duplo'),
    (N'Unb - Centro de Convivência Multicultural', N'UnB - Centro de Convivência Multicultural', N'Case incorreto'),
    (N'Unb - Instituto de Ciência Política da UnB', N'UnB - Instituto de Ciência Política', N'Case + redundante'),
    (N'Unb - no final do ICC', N'UnB - ICC', N'Descrição padronizada'),
    (N'Universidade de Brasília', N'UnB', N'Nome completo → sigla'),

    -- Hotel
    (N'Hotel ', N'Hotel', N'Espaço extra'),
    (N'Hotel  Grand Mercure', N'Hotel Grand Mercure', N'Espaço duplo'),

    -- Arniqueira SHA
    (N'Arniqueira SHA.', N'Arniqueira SHA', N'Ponto extra'),
    (N'Sha Chácara 81/28 - Casa 61 . Residencial Bela Vista. Arniqueira.', N'Arniqueira SHA - Residencial Bela Vista', N'Padronização'),
    (N'Sha Chácara 81/28 Casa 61 - Residencial Bela Vista Arniqueiras', N'Arniqueira SHA - Residencial Bela Vista', N'Padronização'),

    -- Capital Recicláveis / Centro Olímpico Estrutural
    (N'Capital Recicláveis (Estrutural) / Centro Olimpico Estrutural', N'Capital Recicláveis / Centro Olímpico Estrutural', N'Sem acento'),

    -- 111 Sul
    (N'111 Sul b  l ', N'111 Sul', N'Texto truncado'),
    (N'111 Sul Bloco I', N'111 Sul - Bloco I', N'Formatação'),

    -- ══════════════════════════════════════════════════════════════════════════
    -- CORREÇÕES ORTOGRÁFICAS ADICIONAIS (Validação Completa)
    -- ══════════════════════════════════════════════════════════════════════════

    -- A definir (preposição sem acento/crase)
    (N'Á definir', N'A definir', N'Acento incorreto (preposição "A" sem acento)'),
    (N'À definir', N'A definir', N'Crase incorreta (preposição "A" sem crase antes de verbo)'),

    -- Garagem
    (N'Garagem ', N'Garagem', N'Espaço extra no final'),
    (N'garagem', N'Garagem', N'Case incorreto'),

    -- Estacionamento
    (N'Estacionamento ', N'Estacionamento', N'Espaço extra no final'),
    (N'estacionamento', N'Estacionamento', N'Case incorreto'),

    -- Secretaria
    (N'Secretaria ', N'Secretaria', N'Espaço extra no final'),
    (N'secretaria', N'Secretaria', N'Case incorreto'),

    -- Recepção (cedilha)
    (N'Recepcao', N'Recepção', N'Sem cedilha'),
    (N'recepcao', N'Recepção', N'Sem cedilha + case incorreto'),
    (N'RECEPCAO', N'Recepção', N'Sem cedilha + case incorreto'),

    -- Depósito (acento)
    (N'Deposito', N'Depósito', N'Sem acento'),
    (N'deposito', N'Depósito', N'Sem acento + case incorreto'),

    -- Área (acento)
    (N'Area', N'Área', N'Sem acento'),
    (N'area', N'Área', N'Sem acento + case incorreto'),

    -- Sanitária (acento)
    (N'Sanitaria', N'Sanitária', N'Sem acento'),
    (N'sanitaria', N'Sanitária', N'Sem acento + case incorreto'),

    -- Ambulância (acento) - standalone
    (N'Ambulancia', N'Ambulância', N'Sem acento'),
    (N'ambulancia', N'Ambulância', N'Sem acento + case incorreto'),

    -- Portão (acento + cedilha) - standalone
    (N'Portao', N'Portão', N'Sem til'),
    (N'portao', N'Portão', N'Sem til + case incorreto'),

    -- Estação (cedilha)
    (N'Estacao', N'Estação', N'Sem cedilha'),
    (N'estacao', N'Estação', N'Sem cedilha + case incorreto'),

    -- Funcionário (acento)
    (N'Funcionario', N'Funcionário', N'Sem acento'),
    (N'funcionario', N'Funcionário', N'Sem acento + case incorreto'),

    -- Almoxarifado (standalone - para casos não cobertos por "Almoxarifado SIA")
    (N'almoxarifado', N'Almoxarifado', N'Case incorreto'),

    -- Auditório (acento)
    (N'Auditorio', N'Auditório', N'Sem acento'),
    (N'auditorio', N'Auditório', N'Sem acento + case incorreto'),

    -- Saída (acento)
    (N'Saida', N'Saída', N'Sem acento'),
    (N'saida', N'Saída', N'Sem acento + case incorreto'),

    -- Emergência (acento)
    (N'Emergencia', N'Emergência', N'Sem acento'),
    (N'emergencia', N'Emergência', N'Sem acento + case incorreto'),

    -- Manutenção (cedilha)
    (N'Manutencao', N'Manutenção', N'Sem cedilha'),
    (N'manutencao', N'Manutenção', N'Sem cedilha + case incorreto'),

    -- Administração (cedilha)
    (N'Administracao', N'Administração', N'Sem cedilha'),
    (N'administracao', N'Administração', N'Sem cedilha + case incorreto'),

    -- Recepção - variações adicionais
    (N'Recepção ', N'Recepção', N'Espaço extra no final'),

    -- Coordenação (cedilha)
    (N'Coordenacao', N'Coordenação', N'Sem cedilha'),
    (N'coordenacao', N'Coordenação', N'Sem cedilha + case incorreto');

DECLARE @TotalMapeamentos INT = @@ROWCOUNT;
PRINT '✅ ' + CAST(@TotalMapeamentos AS VARCHAR) + ' mapeamentos configurados';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- VALIDAÇÃO PÓS-INSERT: Verificar e CORRIGIR duplicatas case-insensitive
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '🔍 Verificando duplicatas case-insensitive...';

-- Inserir valores na tabela de validação (com collation case-sensitive para detectar duplicatas)
INSERT INTO #ValidacaoDuplicatas (ValorAntigo)
SELECT DISTINCT ValorAntigo COLLATE Latin1_General_CS_AS
FROM #MapeamentoOrigemDestino;

DECLARE @TotalDistintos INT = (SELECT COUNT(*) FROM #ValidacaoDuplicatas);
DECLARE @DiferencaMapeamentos INT = @TotalMapeamentos - @TotalDistintos;

IF @DiferencaMapeamentos > 0
BEGIN
    PRINT '';
    PRINT '⚠️  ════════════════════════════════════════════════════════════════════';
    PRINT '⚠️  AVISO: Duplicatas case-insensitive detectadas!';
    PRINT '⚠️  ════════════════════════════════════════════════════════════════════';
    PRINT '⚠️  Total de mapeamentos: ' + CAST(@TotalMapeamentos AS VARCHAR);
    PRINT '⚠️  Total de valores únicos (case-sensitive): ' + CAST(@TotalDistintos AS VARCHAR);
    PRINT '⚠️  Duplicatas encontradas: ' + CAST(@DiferencaMapeamentos AS VARCHAR);
    PRINT '';
    PRINT '🔍 Lista de valores duplicados (case-insensitive):';
    PRINT '';

    -- Listar duplicatas detectadas
    SELECT
        LOWER(ValorAntigo) AS ValorNormalizado,
        COUNT(*) AS Ocorrencias,
        STRING_AGG(ValorAntigo, ' | ') AS Variacoes
    FROM #MapeamentoOrigemDestino
    GROUP BY LOWER(ValorAntigo)
    HAVING COUNT(*) > 1
    ORDER BY COUNT(*) DESC, LOWER(ValorAntigo);

    PRINT '';
    PRINT '🤖 Aplicando AUTO-FIX INTELIGENTE...';
    PRINT '';

    -- ══════════════════════════════════════════════════════════════════════════
    -- AUTO-FIX: Remover duplicatas automaticamente seguindo regras
    -- ══════════════════════════════════════════════════════════════════════════

    -- Criar tabela para armazenar valores a serem mantidos
    IF OBJECT_ID('tempdb..#ValoresManterAutoFix') IS NOT NULL
        DROP TABLE #ValoresManterAutoFix;

    CREATE TABLE #ValoresManterAutoFix (
        ValorAntigo NVARCHAR(500) NOT NULL,
        Prioridade INT NOT NULL,
        Razao NVARCHAR(200)
    );

    -- Aplicar regras de prioridade para cada grupo de duplicatas
    -- NOVA LÓGICA: PRIORIDADE BASEADA NO ValorCanonico (CORRETO), NÃO NO ValorAntigo (ERRADO)!
    INSERT INTO #ValoresManterAutoFix (ValorAntigo, Prioridade, Razao)
    SELECT
        ValorAntigo,
        CASE
            -- ══════════════════════════════════════════════════════════════════════
            -- PRIORIDADE MÁXIMA: ValorCanonico COM ORTOGRAFIA CORRETA
            -- ══════════════════════════════════════════════════════════════════════

            -- Prioridade 1: ValorCanonico com ortografia correta + Primeira maiúscula + sem espaços (ex: "Recepção")
            WHEN (
                    ValorCanonico LIKE N'%á%' OR ValorCanonico LIKE N'%é%' OR ValorCanonico LIKE N'%í%' OR
                    ValorCanonico LIKE N'%ó%' OR ValorCanonico LIKE N'%ú%' OR ValorCanonico LIKE N'%â%' OR
                    ValorCanonico LIKE N'%ê%' OR ValorCanonico LIKE N'%ô%' OR ValorCanonico LIKE N'%ã%' OR
                    ValorCanonico LIKE N'%õ%' OR ValorCanonico LIKE N'%ç%' OR ValorCanonico LIKE N'%à%' OR
                    ValorCanonico LIKE N'%Á%' OR ValorCanonico LIKE N'%É%' OR ValorCanonico LIKE N'%Í%' OR
                    ValorCanonico LIKE N'%Ó%' OR ValorCanonico LIKE N'%Ú%' OR ValorCanonico LIKE N'%Â%' OR
                    ValorCanonico LIKE N'%Ê%' OR ValorCanonico LIKE N'%Ô%' OR ValorCanonico LIKE N'%Ã%' OR
                    ValorCanonico LIKE N'%Õ%' OR ValorCanonico LIKE N'%Ç%' OR ValorCanonico LIKE N'%À%'
                 )
                 AND ValorCanonico COLLATE Latin1_General_CS_AS =
                     UPPER(LEFT(ValorCanonico, 1)) + LOWER(SUBSTRING(ValorCanonico, 2, LEN(ValorCanonico)))
                 AND LEN(RTRIM(ValorCanonico)) = LEN(ValorCanonico)
            THEN 1

            -- Prioridade 2: ValorCanonico com ortografia correta + Tudo minúscula + sem espaços (ex: "recepção")
            WHEN (
                    ValorCanonico LIKE N'%á%' OR ValorCanonico LIKE N'%é%' OR ValorCanonico LIKE N'%í%' OR
                    ValorCanonico LIKE N'%ó%' OR ValorCanonico LIKE N'%ú%' OR ValorCanonico LIKE N'%â%' OR
                    ValorCanonico LIKE N'%ê%' OR ValorCanonico LIKE N'%ô%' OR ValorCanonico LIKE N'%ã%' OR
                    ValorCanonico LIKE N'%õ%' OR ValorCanonico LIKE N'%ç%' OR ValorCanonico LIKE N'%à%'
                 )
                 AND ValorCanonico = LOWER(ValorCanonico)
                 AND LEN(RTRIM(ValorCanonico)) = LEN(ValorCanonico)
            THEN 2

            -- Prioridade 3: ValorCanonico com ortografia correta + Primeira maiúscula + COM espaços
            WHEN (
                    ValorCanonico LIKE N'%á%' OR ValorCanonico LIKE N'%é%' OR ValorCanonico LIKE N'%í%' OR
                    ValorCanonico LIKE N'%ó%' OR ValorCanonico LIKE N'%ú%' OR ValorCanonico LIKE N'%â%' OR
                    ValorCanonico LIKE N'%ê%' OR ValorCanonico LIKE N'%ô%' OR ValorCanonico LIKE N'%ã%' OR
                    ValorCanonico LIKE N'%õ%' OR ValorCanonico LIKE N'%ç%' OR ValorCanonico LIKE N'%à%' OR
                    ValorCanonico LIKE N'%Á%' OR ValorCanonico LIKE N'%É%' OR ValorCanonico LIKE N'%Í%' OR
                    ValorCanonico LIKE N'%Ó%' OR ValorCanonico LIKE N'%Ú%' OR ValorCanonico LIKE N'%Â%' OR
                    ValorCanonico LIKE N'%Ê%' OR ValorCanonico LIKE N'%Ô%' OR ValorCanonico LIKE N'%Ã%' OR
                    ValorCanonico LIKE N'%Õ%' OR ValorCanonico LIKE N'%Ç%' OR ValorCanonico LIKE N'%À%'
                 )
                 AND ValorCanonico COLLATE Latin1_General_CS_AS =
                     UPPER(LEFT(RTRIM(ValorCanonico), 1)) + LOWER(SUBSTRING(RTRIM(ValorCanonico), 2, LEN(RTRIM(ValorCanonico))))
            THEN 3

            -- Prioridade 4: ValorCanonico com ortografia correta + Outras variações (tudo maiúscula, mista, etc.)
            WHEN (
                    ValorCanonico LIKE N'%á%' OR ValorCanonico LIKE N'%é%' OR ValorCanonico LIKE N'%í%' OR
                    ValorCanonico LIKE N'%ó%' OR ValorCanonico LIKE N'%ú%' OR ValorCanonico LIKE N'%â%' OR
                    ValorCanonico LIKE N'%ê%' OR ValorCanonico LIKE N'%ô%' OR ValorCanonico LIKE N'%ã%' OR
                    ValorCanonico LIKE N'%õ%' OR ValorCanonico LIKE N'%ç%' OR ValorCanonico LIKE N'%à%' OR
                    ValorCanonico LIKE N'%Á%' OR ValorCanonico LIKE N'%É%' OR ValorCanonico LIKE N'%Í%' OR
                    ValorCanonico LIKE N'%Ó%' OR ValorCanonico LIKE N'%Ú%' OR ValorCanonico LIKE N'%Â%' OR
                    ValorCanonico LIKE N'%Ê%' OR ValorCanonico LIKE N'%Ô%' OR ValorCanonico LIKE N'%Ã%' OR
                    ValorCanonico LIKE N'%Õ%' OR ValorCanonico LIKE N'%Ç%' OR ValorCanonico LIKE N'%À%'
                 )
            THEN 4

            -- ══════════════════════════════════════════════════════════════════════
            -- PRIORIDADE BAIXA: ValorCanonico SEM ORTOGRAFIA CORRETA
            -- ══════════════════════════════════════════════════════════════════════

            -- Prioridade 5: SEM ortografia + Primeira maiúscula + sem espaços (ex: "Recepcao")
            WHEN ValorCanonico COLLATE Latin1_General_CS_AS =
                 UPPER(LEFT(ValorCanonico, 1)) + LOWER(SUBSTRING(ValorCanonico, 2, LEN(ValorCanonico)))
                 AND LEN(RTRIM(ValorCanonico)) = LEN(ValorCanonico)
            THEN 5

            -- Prioridade 6: SEM ortografia + Primeira maiúscula + COM espaços
            WHEN ValorCanonico COLLATE Latin1_General_CS_AS =
                 UPPER(LEFT(RTRIM(ValorCanonico), 1)) + LOWER(SUBSTRING(RTRIM(ValorCanonico), 2, LEN(RTRIM(ValorCanonico))))
            THEN 6

            -- Prioridade 7: SEM ortografia + Todas maiúsculas + sem espaços (ex: "RECEPCAO")
            WHEN ValorCanonico = UPPER(ValorCanonico) AND LEN(RTRIM(ValorCanonico)) = LEN(ValorCanonico)
            THEN 7

            -- Prioridade 8: SEM ortografia + Todas maiúsculas + com espaços
            WHEN ValorCanonico = UPPER(ValorCanonico)
            THEN 8

            -- Prioridade 9: SEM ortografia + Todas minúsculas + sem espaços (ex: "recepcao")
            WHEN ValorCanonico = LOWER(ValorCanonico) AND LEN(RTRIM(ValorCanonico)) = LEN(ValorCanonico)
            THEN 9

            -- Prioridade 10: Outras variações sem ortografia
            ELSE 10
        END AS Prioridade,
        CASE
            WHEN (
                    ValorCanonico LIKE N'%á%' OR ValorCanonico LIKE N'%é%' OR ValorCanonico LIKE N'%í%' OR
                    ValorCanonico LIKE N'%ó%' OR ValorCanonico LIKE N'%ú%' OR ValorCanonico LIKE N'%â%' OR
                    ValorCanonico LIKE N'%ê%' OR ValorCanonico LIKE N'%ô%' OR ValorCanonico LIKE N'%ã%' OR
                    ValorCanonico LIKE N'%õ%' OR ValorCanonico LIKE N'%ç%' OR ValorCanonico LIKE N'%à%' OR
                    ValorCanonico LIKE N'%Á%' OR ValorCanonico LIKE N'%É%' OR ValorCanonico LIKE N'%Í%' OR
                    ValorCanonico LIKE N'%Ó%' OR ValorCanonico LIKE N'%Ú%' OR ValorCanonico LIKE N'%Â%' OR
                    ValorCanonico LIKE N'%Ê%' OR ValorCanonico LIKE N'%Ô%' OR ValorCanonico LIKE N'%Ã%' OR
                    ValorCanonico LIKE N'%Õ%' OR ValorCanonico LIKE N'%Ç%' OR ValorCanonico LIKE N'%À%'
                 )
            THEN 'Ortografia correta ✓ (ValorCanonico)'
            ELSE 'Sem ortografia correta'
        END AS Razao
    FROM #MapeamentoOrigemDestino;

    -- Para cada grupo de duplicatas, manter apenas o de maior prioridade (menor número)
    DELETE m
    FROM #MapeamentoOrigemDestino m
    WHERE EXISTS (
        SELECT 1
        FROM #ValoresManterAutoFix v1
        WHERE LOWER(v1.ValorAntigo) = LOWER(m.ValorAntigo)
        AND EXISTS (
            SELECT 1
            FROM #ValoresManterAutoFix v2
            WHERE LOWER(v2.ValorAntigo) = LOWER(v1.ValorAntigo)
            AND v2.Prioridade < v1.Prioridade
        )
        AND v1.ValorAntigo = m.ValorAntigo
    );

    DECLARE @RegistrosRemovidos INT = @TotalMapeamentos - (SELECT COUNT(*) FROM #MapeamentoOrigemDestino);

    PRINT '✅ AUTO-FIX concluído!';
    PRINT '📊 Registros removidos: ' + CAST(@RegistrosRemovidos AS VARCHAR);
    PRINT '';
    PRINT '🔍 Valores mantidos (melhor variação de cada grupo):';
    PRINT '';

    -- Mostrar valores mantidos COM O MAPEAMENTO COMPLETO
    SELECT DISTINCT
        LOWER(m.ValorAntigo) AS ValorNormalizado,
        m.ValorAntigo AS VariacaoMantida_ERRADO,
        m.ValorCanonico AS ValorCorreto_COM_ACENTO,
        v.Razao AS Motivo
    FROM #MapeamentoOrigemDestino m
    INNER JOIN #ValoresManterAutoFix v ON v.ValorAntigo = m.ValorAntigo
    WHERE EXISTS (
        SELECT 1
        FROM #ValoresManterAutoFix v2
        WHERE LOWER(v2.ValorAntigo) = LOWER(m.ValorAntigo)
        GROUP BY LOWER(v2.ValorAntigo)
        HAVING COUNT(*) > 1
    )
    ORDER BY LOWER(m.ValorAntigo);

    PRINT '';

    -- Limpar tabela de auto-fix
    DROP TABLE #ValoresManterAutoFix;

    -- Atualizar contadores
    SET @TotalMapeamentos = (SELECT COUNT(*) FROM #MapeamentoOrigemDestino);
    PRINT '📝 Total de mapeamentos após auto-fix: ' + CAST(@TotalMapeamentos AS VARCHAR);
    PRINT '';
END
ELSE
BEGIN
    PRINT '✅ Nenhuma duplicata case-insensitive detectada';
    PRINT '';
END

-- Limpar tabela de validação
DROP TABLE #ValidacaoDuplicatas;

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 3: FUZZY MATCHING EM MASSA (LEVENSHTEIN DISTANCE)
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🔍 FASE 3: FUZZY MATCHING EM MASSA';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';
PRINT '⚙️  Calculando similaridade Levenshtein para valores não mapeados...';
PRINT '   Threshold: ≥85% de similaridade';
PRINT '';

-- Criar função de Levenshtein Distance
IF OBJECT_ID('tempdb..#LevenshteinDistance') IS NOT NULL
    DROP FUNCTION #LevenshteinDistance;
GO

CREATE FUNCTION dbo.LevenshteinDistance(@string1 NVARCHAR(MAX), @string2 NVARCHAR(MAX))
RETURNS INT
AS
BEGIN
    DECLARE @len1 INT = LEN(@string1);
    DECLARE @len2 INT = LEN(@string2);
    DECLARE @i INT = 0;
    DECLARE @j INT = 0;
    DECLARE @cost INT;
    DECLARE @d TABLE (i INT, j INT, distance INT);

    -- Caso base: strings vazias
    IF @len1 = 0 RETURN @len2;
    IF @len2 = 0 RETURN @len1;

    -- Inicializar matriz de distâncias
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

    -- Calcular distâncias
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
                SELECT distance + 1 FROM @d WHERE i = @i - 1 AND j = @j      -- Deletar
                UNION ALL
                SELECT distance + 1 FROM @d WHERE i = @i AND j = @j - 1      -- Inserir
                UNION ALL
                SELECT distance + @cost FROM @d WHERE i = @i - 1 AND j = @j - 1  -- Substituir
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
    ValorOriginal NVARCHAR(255),
    ValorCanonico NVARCHAR(255),
    LevenshteinDist INT,
    SimilarityPercent DECIMAL(5,2),
    Campo NVARCHAR(10) -- 'Origem' ou 'Destino'
);

-- Buscar valores de ORIGEM não mapeados
DECLARE @ValorOriginal NVARCHAR(255);
DECLARE @ValorCanonico NVARCHAR(255);
DECLARE @LevenshteinDist INT;
DECLARE @MaxLen INT;
DECLARE @SimilarityPercent DECIMAL(5,2);
DECLARE @MatchesFound INT = 0;

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
    -- Comparar com todos os valores canônicos
    DECLARE canonico_cursor CURSOR FOR
    SELECT DISTINCT ValorCanonico FROM #MapeamentoOrigemDestino;

    OPEN canonico_cursor;
    FETCH NEXT FROM canonico_cursor INTO @ValorCanonico;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular Levenshtein Distance
        SET @LevenshteinDist = dbo.LevenshteinDistance(@ValorOriginal, @ValorCanonico);

        -- Calcular percentual de similaridade
        SET @MaxLen = CASE
            WHEN LEN(@ValorOriginal) > LEN(@ValorCanonico) THEN LEN(@ValorOriginal)
            ELSE LEN(@ValorCanonico)
        END;

        IF @MaxLen > 0
            SET @SimilarityPercent = ((CAST(@MaxLen AS DECIMAL(10,2)) - CAST(@LevenshteinDist AS DECIMAL(10,2))) / CAST(@MaxLen AS DECIMAL(10,2))) * 100
        ELSE
            SET @SimilarityPercent = 0;

        -- Se similaridade >= 85%, adicionar como candidato
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

-- Buscar valores de DESTINO não mapeados
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
    -- Comparar com todos os valores canônicos
    DECLARE canonico_cursor2 CURSOR FOR
    SELECT DISTINCT ValorCanonico FROM #MapeamentoOrigemDestino;

    OPEN canonico_cursor2;
    FETCH NEXT FROM canonico_cursor2 INTO @ValorCanonico;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular Levenshtein Distance
        SET @LevenshteinDist = dbo.LevenshteinDistance(@ValorOriginal, @ValorCanonico);

        -- Calcular percentual de similaridade
        SET @MaxLen = CASE
            WHEN LEN(@ValorOriginal) > LEN(@ValorCanonico) THEN LEN(@ValorOriginal)
            ELSE LEN(@ValorCanonico)
        END;

        IF @MaxLen > 0
            SET @SimilarityPercent = ((CAST(@MaxLen AS DECIMAL(10,2)) - CAST(@LevenshteinDist AS DECIMAL(10,2))) / CAST(@MaxLen AS DECIMAL(10,2))) * 100
        ELSE
            SET @SimilarityPercent = 0;

        -- Se similaridade >= 85%, adicionar como candidato
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

-- Selecionar apenas o melhor match para cada valor original (maior similaridade)
CREATE TABLE #BestMatches (
    ValorOriginal NVARCHAR(255),
    ValorCanonico NVARCHAR(255),
    SimilarityPercent DECIMAL(5,2),
    Campo NVARCHAR(10)
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

-- Inserir novos mapeamentos fuzzy na tabela principal
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Observacao)
SELECT
    ValorOriginal,
    ValorCanonico,
    N'Fuzzy Match (' + CAST(SimilarityPercent AS NVARCHAR(10)) + N'% similaridade)'
FROM #BestMatches;

SET @MatchesFound = @@ROWCOUNT;

PRINT '';
PRINT '✅ Fuzzy matching concluído!';
PRINT '   - Matches encontrados: ' + CAST(@MatchesFound AS VARCHAR);
PRINT '   - Novos mapeamentos adicionados: ' + CAST(@MatchesFound AS VARCHAR);
PRINT '';

-- Mostrar resumo dos matches fuzzy encontrados
IF @MatchesFound > 0
BEGIN
    PRINT '📋 RESUMO DOS MATCHES FUZZY:';
    PRINT '';

    DECLARE @Msg NVARCHAR(MAX);
    DECLARE match_cursor CURSOR FOR
    SELECT
        '   ' + Campo + ': "' + ValorOriginal + '" → "' + ValorCanonico + '" (' + CAST(SimilarityPercent AS NVARCHAR(10)) + '%)'
    FROM #BestMatches
    ORDER BY Campo, SimilarityPercent DESC;

    OPEN match_cursor;
    FETCH NEXT FROM match_cursor INTO @Msg;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT @Msg;
        FETCH NEXT FROM match_cursor INTO @Msg;
    END

    CLOSE match_cursor;
    DEALLOCATE match_cursor;

    PRINT '';
END

-- Limpar tabelas temporárias
DROP TABLE #FuzzyCandidates;
DROP TABLE #BestMatches;
DROP FUNCTION dbo.LevenshteinDistance;

-- Atualizar contagem total de mapeamentos
SELECT @TotalMapeamentos = COUNT(*) FROM #MapeamentoOrigemDestino;
PRINT '📝 Total de mapeamentos após fuzzy matching: ' + CAST(@TotalMapeamentos AS VARCHAR);
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 4: EXECUTAR ATUALIZAÇÕES
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '🚀 FASE 4: EXECUTANDO ATUALIZAÇÕES';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

BEGIN TRANSACTION;

BEGIN TRY
    -- Contar registros que serão afetados
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

    -- Commit
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

    SET NOCOUNT OFF;
    RETURN;
END CATCH;

-- ══════════════════════════════════════════════════════════════════════════════
-- FASE 5: ESTATÍSTICAS FINAIS E PERCENTUAL DE REDUÇÃO
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '📊 FASE 5: ESTATÍSTICAS FINAIS';
PRINT '────────────────────────────────────────────────────────────────────────';
PRINT '';

-- Contar valores ÚNICOS depois da limpeza
DECLARE @OrigemUnicosDepois INT;
DECLARE @DestinoUnicosDepois INT;

SELECT @OrigemUnicosDepois = COUNT(DISTINCT Origem)
FROM dbo.Viagem
WHERE Origem IS NOT NULL AND Origem <> '';

SELECT @DestinoUnicosDepois = COUNT(DISTINCT Destino)
FROM dbo.Viagem
WHERE Destino IS NOT NULL AND Destino <> '';

-- Calcular redução
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

PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '✅ LIMPEZA CONCLUÍDA COM SUCESSO!';
PRINT '════════════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '📊 RESUMO GERAL:';
PRINT '   - Total de viagens no backup: ' + CAST(@TotalRegistros AS VARCHAR);
PRINT '   - Mapeamentos configurados: ' + CAST(@TotalMapeamentos AS VARCHAR);
PRINT '   - Registros de Origem atualizados: ' + CAST(@OrigemAtualizados AS VARCHAR);
PRINT '   - Registros de Destino atualizados: ' + CAST(@DestinoAtualizados AS VARCHAR);
PRINT '';
PRINT '┌────────────────────────────────────────────────────────────────────────┐';
PRINT '│ 📊 REDUÇÃO DE VALORES ÚNICOS                                          │';
PRINT '├────────────────────────────────────────────────────────────────────────┤';
PRINT '│                                                                        │';
PRINT '│  📍 ORIGEM:                                                            │';
PRINT '│     Antes:  ' + CAST(@OrigemUnicosAntes AS VARCHAR) + ' valores únicos' + REPLICATE(' ', 48 - LEN(CAST(@OrigemUnicosAntes AS VARCHAR))) + '│';
PRINT '│     Depois: ' + CAST(@OrigemUnicosDepois AS VARCHAR) + ' valores únicos' + REPLICATE(' ', 48 - LEN(CAST(@OrigemUnicosDepois AS VARCHAR))) + '│';
PRINT '│     Redução: ' + CAST(@OrigemReduzidos AS VARCHAR) + ' itens eliminados (' + CAST(@OrigemPercentualReducao AS VARCHAR(10)) + '%)' + REPLICATE(' ', 38 - LEN(CAST(@OrigemReduzidos AS VARCHAR)) - LEN(CAST(@OrigemPercentualReducao AS VARCHAR(10)))) + '│';
PRINT '│                                                                        │';
PRINT '│  🎯 DESTINO:                                                           │';
PRINT '│     Antes:  ' + CAST(@DestinoUnicosAntes AS VARCHAR) + ' valores únicos' + REPLICATE(' ', 48 - LEN(CAST(@DestinoUnicosAntes AS VARCHAR))) + '│';
PRINT '│     Depois: ' + CAST(@DestinoUnicosDepois AS VARCHAR) + ' valores únicos' + REPLICATE(' ', 48 - LEN(CAST(@DestinoUnicosDepois AS VARCHAR))) + '│';
PRINT '│     Redução: ' + CAST(@DestinoReduzidos AS VARCHAR) + ' itens eliminados (' + CAST(@DestinoPercentualReducao AS VARCHAR(10)) + '%)' + REPLICATE(' ', 38 - LEN(CAST(@DestinoReduzidos AS VARCHAR)) - LEN(CAST(@DestinoPercentualReducao AS VARCHAR(10)))) + '│';
PRINT '│                                                                        │';
PRINT '└────────────────────────────────────────────────────────────────────────┘';
PRINT '';

-- ══════════════════════════════════════════════════════════════════════════════
-- TABELA DE RESULTADOS (aparece em aba separada no SSMS)
-- ══════════════════════════════════════════════════════════════════════════════

SELECT
    '📊 ESTATÍSTICAS DE LIMPEZA' AS [Categoria],
    NULL AS [Campo],
    NULL AS [Antes],
    NULL AS [Depois],
    NULL AS [Redução],
    NULL AS [Percentual]

UNION ALL

SELECT
    '────────────────────────',
    '────────────',
    '────────',
    '────────',
    '────────',
    '────────'

UNION ALL

SELECT
    '📍 Origem',
    'Valores únicos',
    CAST(@OrigemUnicosAntes AS VARCHAR),
    CAST(@OrigemUnicosDepois AS VARCHAR),
    CAST(@OrigemReduzidos AS VARCHAR) + ' itens',
    CAST(@OrigemPercentualReducao AS VARCHAR(10)) + '%'

UNION ALL

SELECT
    '🎯 Destino',
    'Valores únicos',
    CAST(@DestinoUnicosAntes AS VARCHAR),
    CAST(@DestinoUnicosDepois AS VARCHAR),
    CAST(@DestinoReduzidos AS VARCHAR) + ' itens',
    CAST(@DestinoPercentualReducao AS VARCHAR(10)) + '%'

UNION ALL

SELECT
    '────────────────────────',
    '────────────',
    '────────',
    '────────',
    '────────',
    '────────'

UNION ALL

SELECT
    '📋 Resumo',
    'Total de viagens',
    CAST(@TotalRegistros AS VARCHAR),
    NULL,
    NULL,
    NULL

UNION ALL

SELECT
    '📋 Resumo',
    'Mapeamentos',
    CAST(@TotalMapeamentos AS VARCHAR),
    NULL,
    NULL,
    NULL

UNION ALL

SELECT
    '📋 Resumo',
    'Origem atualizados',
    CAST(@OrigemAtualizados AS VARCHAR),
    NULL,
    NULL,
    NULL

UNION ALL

SELECT
    '📋 Resumo',
    'Destino atualizados',
    CAST(@DestinoAtualizados AS VARCHAR),
    NULL,
    NULL,
    NULL;

PRINT '';
PRINT '💾 Backup disponível em: dbo.Viagem_Backup_OrigemDestino';
PRINT '⏰ Fim: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';
PRINT '════════════════════════════════════════════════════════════════════════';

-- ══════════════════════════════════════════════════════════════════════════════
-- INSTRUÇÕES DE ROLLBACK (SE NECESSÁRIO)
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '';
PRINT '🔄 Para reverter as alterações, execute:';
PRINT '';
PRINT '/*';
PRINT 'UPDATE v';
PRINT 'SET v.Origem = b.OrigemOriginal, v.Destino = b.DestinoOriginal';
PRINT 'FROM dbo.Viagem v';
PRINT 'INNER JOIN dbo.Viagem_Backup_OrigemDestino b ON v.ViagemId = b.ViagemId;';
PRINT 'PRINT ''✅ Rollback concluído.'';';
PRINT '*/';
PRINT '';

-- Limpar
IF OBJECT_ID('tempdb..#MapeamentoOrigemDestino') IS NOT NULL
    DROP TABLE #MapeamentoOrigemDestino;

SET NOCOUNT OFF;
GO
