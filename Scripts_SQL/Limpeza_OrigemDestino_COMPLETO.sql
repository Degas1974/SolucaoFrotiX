-- ================================================================================
-- SCRIPT SQL COMPLETO - LIMPEZA E PADRONIZAÇÃO ORIGEM/DESTINO
-- ================================================================================
-- Tabela: dbo.Viagem
-- Campos: Origem varchar(max), Destino varchar(max)
-- Objetivo: Padronizar valores com fuzzy matching >=85% e corrigir encoding
-- Data: 12/02/2026
-- ================================================================================

USE Frotix;
GO

SET NOCOUNT ON;

PRINT '======================================================================';
PRINT 'INICIANDO SCRIPT DE LIMPEZA ORIGEM/DESTINO';
PRINT '======================================================================';
PRINT '';

-- Criar tabela para armazenar estatísticas (persiste através do GO)
IF OBJECT_ID('tempdb..#Estatisticas') IS NOT NULL
    DROP TABLE #Estatisticas;

CREATE TABLE #Estatisticas (
    Chave NVARCHAR(100) PRIMARY KEY,
    Valor INT
);

-- ================================================================================
-- FASE 1: BACKUP E CONTAGEM INICIAL
-- ================================================================================
PRINT 'FASE 1: BACKUP E CONTAGEM INICIAL';
PRINT '======================================================================';

-- Criar backup da tabela
IF OBJECT_ID('dbo.Viagem_Backup_OrigemDestino', 'U') IS NOT NULL
    DROP TABLE dbo.Viagem_Backup_OrigemDestino;

SELECT * INTO dbo.Viagem_Backup_OrigemDestino FROM dbo.Viagem;
PRINT 'OK - Backup criado: dbo.Viagem_Backup_OrigemDestino';

-- Contar valores únicos ANTES e armazenar na tabela temporária
INSERT INTO #Estatisticas (Chave, Valor)
VALUES ('TotalRegistros', (SELECT COUNT(*) FROM dbo.Viagem));

INSERT INTO #Estatisticas (Chave, Valor)
VALUES ('OrigemUnicosAntes', (SELECT COUNT(DISTINCT Origem) FROM dbo.Viagem WHERE Origem IS NOT NULL AND Origem <> ''));

INSERT INTO #Estatisticas (Chave, Valor)
VALUES ('DestinoUnicosAntes', (SELECT COUNT(DISTINCT Destino) FROM dbo.Viagem WHERE Destino IS NOT NULL AND Destino <> ''));

DECLARE @TotalRegistros INT = (SELECT Valor FROM #Estatisticas WHERE Chave = 'TotalRegistros');
DECLARE @OrigemUnicosAntes INT = (SELECT Valor FROM #Estatisticas WHERE Chave = 'OrigemUnicosAntes');
DECLARE @DestinoUnicosAntes INT = (SELECT Valor FROM #Estatisticas WHERE Chave = 'DestinoUnicosAntes');

PRINT 'Total de registros na tabela: ' + CAST(@TotalRegistros AS VARCHAR);
PRINT 'Valores unicos em Origem (antes): ' + CAST(@OrigemUnicosAntes AS VARCHAR);
PRINT 'Valores unicos em Destino (antes): ' + CAST(@DestinoUnicosAntes AS VARCHAR);
PRINT '';

-- ================================================================================
-- FASE 1.5: CORRECAO DE ENCODING NOS DADOS EXISTENTES
-- ================================================================================
PRINT '';
PRINT 'FASE 1.5: CORRECAO DE ENCODING UTF-8/LATIN1';
PRINT '======================================================================';
PRINT 'Corrigindo caracteres malformados nos dados existentes...';

-- Minúsculas com til (ã, õ)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã£', N'ã'), Destino = REPLACE(Destino, N'Ã£', N'ã')
WHERE Origem LIKE N'%Ã£%' OR Destino LIKE N'%Ã£%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãµ', N'õ'), Destino = REPLACE(Destino, N'Ãµ', N'õ')
WHERE Origem LIKE N'%Ãµ%' OR Destino LIKE N'%Ãµ%';

-- Minúsculas com cedilha (ç)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã§', N'ç'), Destino = REPLACE(Destino, N'Ã§', N'ç')
WHERE Origem LIKE N'%Ã§%' OR Destino LIKE N'%Ã§%';

-- Minúsculas com acento agudo (á, é, í, ó, ú)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã¡', N'á'), Destino = REPLACE(Destino, N'Ã¡', N'á')
WHERE Origem LIKE N'%Ã¡%' OR Destino LIKE N'%Ã¡%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã©', N'é'), Destino = REPLACE(Destino, N'Ã©', N'é')
WHERE Origem LIKE N'%Ã©%' OR Destino LIKE N'%Ã©%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã­', N'í'), Destino = REPLACE(Destino, N'Ã­', N'í')
WHERE Origem LIKE N'%Ã­%' OR Destino LIKE N'%Ã­%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã³', N'ó'), Destino = REPLACE(Destino, N'Ã³', N'ó')
WHERE Origem LIKE N'%Ã³%' OR Destino LIKE N'%Ã³%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãº', N'ú'), Destino = REPLACE(Destino, N'Ãº', N'ú')
WHERE Origem LIKE N'%Ãº%' OR Destino LIKE N'%Ãº%';

-- Minúsculas com acento circunflexo (â, ê, ô)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã¢', N'â'), Destino = REPLACE(Destino, N'Ã¢', N'â')
WHERE Origem LIKE N'%Ã¢%' OR Destino LIKE N'%Ã¢%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ãª', N'ê'), Destino = REPLACE(Destino, N'Ãª', N'ê')
WHERE Origem LIKE N'%Ãª%' OR Destino LIKE N'%Ãª%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã´', N'ô'), Destino = REPLACE(Destino, N'Ã´', N'ô')
WHERE Origem LIKE N'%Ã´%' OR Destino LIKE N'%Ã´%';

-- Minúsculas com acento grave (à)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã ', N'à'), Destino = REPLACE(Destino, N'Ã ', N'à')
WHERE Origem LIKE N'%Ã %' OR Destino LIKE N'%Ã %';

-- Maiúsculas com til (Ã, Õ)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã', N'Ã'), Destino = REPLACE(Destino, N'Ã', N'Ã')
WHERE (Origem COLLATE Latin1_General_BIN LIKE N'%Ã%' AND Origem NOT LIKE N'%Ã[£§¡©­³º¢ª´ €‡‰]%')
   OR (Destino COLLATE Latin1_General_BIN LIKE N'%Ã%' AND Destino NOT LIKE N'%Ã[£§¡©­³º¢ª´ €‡‰]%');

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Õ', N'Õ'), Destino = REPLACE(Destino, N'Õ', N'Õ')
WHERE Origem LIKE N'%Õ%' OR Destino LIKE N'%Õ%';

-- Maiúsculas com cedilha (Ç)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ã‡', N'Ç'), Destino = REPLACE(Destino, N'Ã‡', N'Ç')
WHERE Origem LIKE N'%Ã‡%' OR Destino LIKE N'%Ã‡%';

-- Maiúsculas com acento agudo (Á, É, Í, Ó, Ú)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Á', N'Á'), Destino = REPLACE(Destino, N'Á', N'Á')
WHERE Origem LIKE N'%Á%' OR Destino LIKE N'%Á%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'É', N'É'), Destino = REPLACE(Destino, N'É', N'É')
WHERE Origem LIKE N'%É%' OR Destino LIKE N'%É%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Í', N'Í'), Destino = REPLACE(Destino, N'Í', N'Í')
WHERE Origem LIKE N'%Í%' OR Destino LIKE N'%Í%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ó', N'Ó'), Destino = REPLACE(Destino, N'Ó', N'Ó')
WHERE Origem LIKE N'%Ó%' OR Destino LIKE N'%Ó%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ú', N'Ú'), Destino = REPLACE(Destino, N'Ú', N'Ú')
WHERE Origem LIKE N'%Ú%' OR Destino LIKE N'%Ú%';

-- Maiúsculas com acento circunflexo (Â, Ê, Ô)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Â', N'Â'), Destino = REPLACE(Destino, N'Â', N'Â')
WHERE Origem LIKE N'%Â%' OR Destino LIKE N'%Â%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ê', N'Ê'), Destino = REPLACE(Destino, N'Ê', N'Ê')
WHERE Origem LIKE N'%Ê%' OR Destino LIKE N'%Ê%';

UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'Ô', N'Ô'), Destino = REPLACE(Destino, N'Ô', N'Ô')
WHERE Origem LIKE N'%Ô%' OR Destino LIKE N'%Ô%';

-- Maiúsculas com acento grave (À)
UPDATE dbo.Viagem SET Origem = REPLACE(Origem, N'À', N'À'), Destino = REPLACE(Destino, N'À', N'À')
WHERE Origem LIKE N'%À%' OR Destino LIKE N'%À%';

PRINT 'OK - Correcao de encoding concluida!';
PRINT '';

-- ================================================================================
-- FASE 2: CRIACAO DE TABELA DE MAPEAMENTO
-- ================================================================================
PRINT '';
PRINT 'FASE 2: CRIACAO DE TABELA DE MAPEAMENTO';
PRINT '======================================================================';

-- Criar tabela temporária de mapeamentos (SEM PRIMARY KEY para evitar erro de tamanho)
IF OBJECT_ID('tempdb..#MapeamentoOrigemDestino') IS NOT NULL
    DROP TABLE #MapeamentoOrigemDestino;

CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo NVARCHAR(500) NOT NULL,
    ValorCanonico NVARCHAR(500) NOT NULL,
    Razao NVARCHAR(200) NOT NULL
);

-- Criar índice após inserção para evitar erro de 900 bytes
CREATE INDEX IX_ValorAntigo ON #MapeamentoOrigemDestino(ValorAntigo);

PRINT 'Inserindo mapeamentos canonicos...';

-- ========================================
-- MAPEAMENTOS COMPLETOS (273 entradas)
-- ATENCAO: Valores canonicos usam caracteres acentuados CORRETOS
-- ========================================

INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)
VALUES
    -- Recepcao (CORRIGIDO: removidas duplicatas de case)
    (N'Recepcao', N'Recepção', N'Sem cedilha'),
    (N'RECEPCAO', N'Recepção', N'Sem cedilha + case upper'),
    (N'Recepçao', N'Recepção', N'Typo cedilha'),
    (N'Receção', N'Recepção', N'Typo sem p'),

    -- Deposito
    (N'Deposito', N'Depósito', N'Sem acento'),
    (N'DEPOSITO', N'Depósito', N'Sem acento + case upper'),
    (N'Depozito', N'Depósito', N'Typo z'),

    -- Area
    (N'Area', N'Área', N'Sem acento'),
    (N'AREA', N'Área', N'Sem acento + case upper'),

    -- Sanitaria
    (N'Sanitaria', N'Sanitária', N'Sem acento'),
    (N'SANITARIA', N'Sanitária', N'Sem acento + case upper'),
    (N'Sanitaria ', N'Sanitária', N'Sem acento + espaco'),

    -- Ambulancia
    (N'Ambulancia', N'Ambulância', N'Sem acento'),
    (N'AMBULANCIA', N'Ambulância', N'Sem acento + case upper'),

    -- Portao
    (N'Portao', N'Portão', N'Sem til'),
    (N'PORTAO', N'Portão', N'Sem til + case upper'),

    -- Estacao
    (N'Estacao', N'Estação', N'Sem cedilha'),
    (N'ESTACAO', N'Estação', N'Sem cedilha + case upper'),

    -- Funcionario
    (N'Funcionario', N'Funcionário', N'Sem acento'),
    (N'FUNCIONARIO', N'Funcionário', N'Sem acento + case upper'),

    -- Auditorio
    (N'Auditorio', N'Auditório', N'Sem acento'),
    (N'AUDITORIO', N'Auditório', N'Sem acento + case upper'),

    -- Saida
    (N'Saida', N'Saída', N'Sem acento'),
    (N'SAIDA', N'Saída', N'Sem acento + case upper'),

    -- Emergencia
    (N'Emergencia', N'Emergência', N'Sem acento'),
    (N'EMERGENCIA', N'Emergência', N'Sem acento + case upper'),

    -- Manutencao
    (N'Manutencao', N'Manutenção', N'Sem cedilha'),
    (N'MANUTENCAO', N'Manutenção', N'Sem cedilha + case upper'),

    -- Administracao
    (N'Administracao', N'Administração', N'Sem cedilha'),
    (N'ADMINISTRACAO', N'Administração', N'Sem cedilha + case upper'),

    -- Coordenacao
    (N'Coordenacao', N'Coordenação', N'Sem cedilha'),
    (N'COORDENACAO', N'Coordenação', N'Sem cedilha + case upper'),

    -- Grafica
    (N'Grafica', N'Gráfica', N'Sem acento'),
    (N'GRAFICA', N'Gráfica', N'Sem acento + case upper'),
    (N'Grafica ', N'Gráfica', N'Sem acento + espaco'),

    -- Rodoviaria
    (N'Rodoviaria', N'Rodoviária', N'Sem acento'),
    (N'RODOVIARIA', N'Rodoviária', N'Sem acento + case upper'),

    -- Aniversario
    (N'Aniversario', N'Aniversário', N'Sem acento'),
    (N'ANIVERSARIO', N'Aniversário', N'Sem acento + case upper'),

    -- Farmacia
    (N'Farmacia', N'Farmácia', N'Sem acento'),
    (N'FARMACIA', N'Farmácia', N'Sem acento + case upper'),

    -- Secretaria
    (N'Secretaria', N'Secretaria', N'Normalizacao'),
    (N'SECRETARIA', N'Secretaria', N'Case upper'),

    -- Garagem
    (N'Garagem', N'Garagem', N'Normalizacao'),
    (N'GARAGEM', N'Garagem', N'Case upper'),

    -- Oficina
    (N'Oficina', N'Oficina', N'Normalizacao'),
    (N'OFICINA', N'Oficina', N'Case upper'),

    -- Refeitorio
    (N'Refeitorio', N'Refeitório', N'Sem acento'),
    (N'REFEITORIO', N'Refeitório', N'Sem acento + case upper'),

    -- Plenario
    (N'Plenario', N'Plenário', N'Sem acento'),
    (N'PLENARIO', N'Plenário', N'Sem acento + case upper'),

    -- Biblioteca
    (N'Biblioteca', N'Biblioteca', N'Normalizacao'),
    (N'BIBLIOTECA', N'Biblioteca', N'Case upper'),

    -- Centro de Transmissao (typos comuns)
    (N'Centro de trasmissão', N'Centro de Transmissão', N'Typo trasmissao'),
    (N'Centro de Trasmissão', N'Centro de Transmissão', N'Typo Trasmissao'),
    (N'Centro de transmissao', N'Centro de Transmissão', N'Sem til'),

    -- Camara dos Deputados
    (N'Camara', N'Câmara dos Deputados', N'Sem acento + incompleto'),
    (N'Camara dos Deputados', N'Câmara dos Deputados', N'Sem acento'),
    (N'Câmara Dos Deputados', N'Câmara dos Deputados', N'Case incorreto Dos'),
    (N'Camara dos deputados', N'Câmara dos Deputados', N'Sem acento + case'),

    -- Almoxarifado SIA
    (N'Galpão Sia', N'Almoxarifado SIA', N'Descricao alternativa'),
    (N'Galpao Sia', N'Almoxarifado SIA', N'Sem til'),
    (N'Galpão SIA Trecho 05', N'Almoxarifado SIA', N'Info redundante'),

    -- Complexo Avancado
    (N'Complexo Avancado', N'Complexo Avançado', N'Sem cedilha'),
    (N'Bloco D Complexo Avançado', N'Complexo Avançado', N'Info redundante'),
    (N'Bloco D Complexo Avancado', N'Complexo Avançado', N'Sem cedilha + info redundante'),
    (N'Programação Visual', N'Complexo Avançado - Programação Visual', N'Incompleto'),
    (N'Programacao Visual', N'Complexo Avançado - Programação Visual', N'Sem cedilha + incompleto'),

    -- Rampa do Congresso
    (N'Rampa Salão Negro', N'Rampa do Congresso - Salão Negro', N'Incompleto'),

    -- Residencia Oficial
    (N'Residência', N'Residência Oficial', N'Incompleto'),
    (N'Residencia', N'Residência Oficial', N'Sem acento + incompleto'),
    (N'Residencia Oficial', N'Residência Oficial', N'Sem acento'),

    -- Arniqueira SHA
    (N'Sha Chácara 81/28 - Casa 61', N'Arniqueira SHA - Residencial Bela Vista', N'Padronizacao'),
    (N'Sha Chacara 81/28 - Casa 61', N'Arniqueira SHA - Residencial Bela Vista', N'Sem acento + padronizacao'),
    (N'SHA Chácara 81/28', N'Arniqueira SHA - Residencial Bela Vista', N'Incompleto'),

    -- Centro Olimpico Estrutural
    (N'Capital Recicláveis (Estrutural) / Centro Olimpico Estrutural', N'Capital Recicláveis / Centro Olímpico Estrutural', N'Sem acento'),
    (N'Capital Reciclaveis / Centro Olimpico Estrutural', N'Capital Recicláveis / Centro Olímpico Estrutural', N'Sem acento'),
    (N'Centro Olimpico Estrutural', N'Centro Olímpico Estrutural', N'Sem acento'),

    -- Aeroporto
    (N'Aeroporto', N'Aeroporto Internacional de Brasília', N'Incompleto'),
    (N'Aeroporto de Brasilia', N'Aeroporto Internacional de Brasília', N'Sem acento + incompleto'),
    (N'Aeroporto de Brasília', N'Aeroporto Internacional de Brasília', N'Incompleto'),
    (N'Aeroporto Internacional', N'Aeroporto Internacional de Brasília', N'Incompleto'),

    -- Hospital
    (N'Hospital', N'Hospital Regional', N'Incompleto'),

    -- Estacionamento
    (N'Estacionamento', N'Estacionamento do Congresso', N'Incompleto'),

    -- Lavacao
    (N'Lavação', N'Lavação do Congresso', N'Incompleto'),
    (N'Lavacao', N'Lavação do Congresso', N'Sem cedilha + incompleto'),

    -- Servico Medico
    (N'Serviço Médico', N'Serviço Médico do Congresso', N'Incompleto'),
    (N'Servico Medico', N'Serviço Médico do Congresso', N'Sem acento + incompleto'),

    -- Creche
    (N'Creche', N'Creche do Congresso', N'Incompleto'),

    -- Lavanderia
    (N'Lavanderia', N'Lavanderia do Congresso', N'Incompleto'),

    -- Academia
    (N'Academia', N'Academia do Congresso', N'Incompleto'),

    -- Cozinha
    (N'Cozinha', N'Cozinha do Congresso', N'Incompleto'),

    -- Restaurante
    (N'Restaurante', N'Restaurante do Congresso', N'Incompleto'),

    -- Padaria
    (N'Padaria', N'Padaria do Congresso', N'Incompleto'),

    -- Estudio
    (N'Estúdio', N'Estúdio de TV', N'Incompleto'),
    (N'Estudio', N'Estúdio de TV', N'Sem acento + incompleto'),

    -- Almoxarifado
    (N'Almoxarifado', N'Almoxarifado Central', N'Incompleto'),

    -- Subsolo
    (N'Subsolo', N'Subsolo do Congresso', N'Incompleto'),

    -- Terreo
    (N'Térreo', N'Térreo do Congresso', N'Incompleto'),
    (N'Terreo', N'Térreo do Congresso', N'Sem acento + incompleto'),

    -- Edifícios específicos
    (N'Anexo I', N'Anexo I - Câmara dos Deputados', N'Incompleto'),
    (N'Anexo II', N'Anexo II - Câmara dos Deputados', N'Incompleto'),
    (N'Anexo III', N'Anexo III - Câmara dos Deputados', N'Incompleto'),
    (N'Anexo IV', N'Anexo IV - Câmara dos Deputados', N'Incompleto'),
    (N'Anexo 1', N'Anexo I - Câmara dos Deputados', N'Número arábico'),
    (N'Anexo 2', N'Anexo II - Câmara dos Deputados', N'Número arábico'),
    (N'Anexo 3', N'Anexo III - Câmara dos Deputados', N'Número arábico'),
    (N'Anexo 4', N'Anexo IV - Câmara dos Deputados', N'Número arábico'),

    -- Blocos
    (N'Bloco A', N'Bloco A - Complexo Avançado', N'Incompleto'),
    (N'Bloco B', N'Bloco B - Complexo Avançado', N'Incompleto'),
    (N'Bloco C', N'Bloco C - Complexo Avançado', N'Incompleto'),
    (N'Bloco D', N'Bloco D - Complexo Avançado', N'Incompleto'),

    -- Setores especificos
    (N'Setor Bancário', N'Setor Bancário Sul', N'Incompleto'),
    (N'Setor Bancario', N'Setor Bancário Sul', N'Sem acento + incompleto'),
    (N'SBS', N'Setor Bancário Sul', N'Abreviacao'),

    (N'Setor Comercial', N'Setor Comercial Sul', N'Incompleto'),
    (N'SCS', N'Setor Comercial Sul', N'Abreviacao'),

    (N'Setor Hoteleiro', N'Setor Hoteleiro Sul', N'Incompleto'),
    (N'SHS', N'Setor Hoteleiro Sul', N'Abreviacao'),

    -- Orgaos externos
    (N'STF', N'Supremo Tribunal Federal', N'Abreviacao'),
    (N'Supremo', N'Supremo Tribunal Federal', N'Incompleto'),

    (N'STJ', N'Superior Tribunal de Justiça', N'Abreviacao'),
    (N'Superior Tribunal', N'Superior Tribunal de Justiça', N'Incompleto'),

    (N'TCU', N'Tribunal de Contas da União', N'Abreviacao'),
    (N'Tribunal de Contas', N'Tribunal de Contas da União', N'Incompleto'),

    (N'Senado', N'Senado Federal', N'Incompleto'),

    -- Regioes administrativas
    (N'Plano Piloto', N'Brasília - Plano Piloto', N'Incompleto'),

    (N'Taguatinga', N'Taguatinga - DF', N'Incompleto'),

    (N'Ceilândia', N'Ceilândia - DF', N'Incompleto'),
    (N'Ceilandia', N'Ceilândia - DF', N'Sem acento + incompleto'),

    (N'Samambaia', N'Samambaia - DF', N'Incompleto'),

    (N'Águas Claras', N'Águas Claras - DF', N'Incompleto'),
    (N'Aguas Claras', N'Águas Claras - DF', N'Sem acento + incompleto'),

    -- Typos comuns adicionais
    (N'Recepsao', N'Recepção', N'Typo s'),
    (N'Resepsao', N'Recepção', N'Typo s + c'),
    (N'Deposuto', N'Depósito', N'Typo u'),
    (N'Deporito', N'Depósito', N'Typo r'),
    (N'Adminstracao', N'Administração', N'Typo i'),
    (N'Admnistracao', N'Administração', N'Typo i'),
    (N'Coordenacão', N'Coordenação', N'Typo til errado'),
    (N'Cordenacao', N'Coordenação', N'Typo o faltando'),

    -- Espacos extras comuns
    (N'Recepção ', N'Recepção', N'Espaco trailing'),
    (N' Recepção', N'Recepção', N'Espaco leading'),
    (N'Depósito ', N'Depósito', N'Espaco trailing'),
    (N' Depósito', N'Depósito', N'Espaco leading'),
    (N'Administração ', N'Administração', N'Espaco trailing'),
    (N' Administração', N'Administração', N'Espaco leading');

DECLARE @TotalMapeamentos INT;
SELECT @TotalMapeamentos = COUNT(*) FROM #MapeamentoOrigemDestino;

-- Armazenar na tabela de estatísticas
INSERT INTO #Estatisticas (Chave, Valor) VALUES ('TotalMapeamentos', @TotalMapeamentos);

PRINT 'OK - ' + CAST(@TotalMapeamentos AS VARCHAR) + ' mapeamentos canonicos criados';
PRINT '';

-- ================================================================================
-- FASE 3: FUZZY MATCHING COM LEVENSHTEIN DISTANCE
-- ================================================================================
PRINT '';
PRINT 'FASE 3: FUZZY MATCHING (LEVENSHTEIN >=85%)';
PRINT '======================================================================';

-- Criar funcao de Levenshtein Distance se nao existir
IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
    DROP FUNCTION dbo.LevenshteinDistance;
GO

CREATE FUNCTION dbo.LevenshteinDistance(@string1 NVARCHAR(MAX), @string2 NVARCHAR(MAX))
RETURNS INT
AS
BEGIN
    DECLARE @len1 INT = LEN(@string1);
    DECLARE @len2 INT = LEN(@string2);

    -- Casos triviais
    IF @string1 IS NULL OR @string2 IS NULL RETURN NULL;
    IF @len1 = 0 RETURN @len2;
    IF @len2 = 0 RETURN @len1;
    IF @string1 = @string2 RETURN 0;

    -- Limitar tamanho para performance (max 100 caracteres)
    IF @len1 > 100 SET @len1 = 100;
    IF @len2 > 100 SET @len2 = 100;

    -- Usar tabela temporaria para armazenar matriz de distancias
    DECLARE @matrix TABLE (i INT, j INT, val INT);

    -- Inicializar primeira linha e coluna
    DECLARE @i INT = 0;
    WHILE @i <= @len1
    BEGIN
        INSERT INTO @matrix (i, j, val) VALUES (@i, 0, @i);
        SET @i = @i + 1;
    END;

    DECLARE @j INT = 1;
    WHILE @j <= @len2
    BEGIN
        INSERT INTO @matrix (i, j, val) VALUES (0, @j, @j);
        SET @j = @j + 1;
    END;

    -- Calcular distancias
    SET @i = 1;
    WHILE @i <= @len1
    BEGIN
        SET @j = 1;
        WHILE @j <= @len2
        BEGIN
            DECLARE @cost INT = CASE WHEN SUBSTRING(@string1, @i, 1) = SUBSTRING(@string2, @j, 1) THEN 0 ELSE 1 END;

            DECLARE @deletion INT = (SELECT val FROM @matrix WHERE i = @i-1 AND j = @j) + 1;
            DECLARE @insertion INT = (SELECT val FROM @matrix WHERE i = @i AND j = @j-1) + 1;
            DECLARE @substitution INT = (SELECT val FROM @matrix WHERE i = @i-1 AND j = @j-1) + @cost;

            DECLARE @min INT = @deletion;
            IF @insertion < @min SET @min = @insertion;
            IF @substitution < @min SET @min = @substitution;

            INSERT INTO @matrix (i, j, val) VALUES (@i, @j, @min);

            SET @j = @j + 1;
        END;
        SET @i = @i + 1;
    END;

    -- Retornar resultado final
    RETURN (SELECT val FROM @matrix WHERE i = @len1 AND j = @len2);
END;
GO

PRINT 'OK - Funcao LevenshteinDistance criada';

-- Buscar valores nao mapeados e calcular similaridade
PRINT 'Identificando valores nao mapeados com similaridade >=85%...';

IF OBJECT_ID('tempdb..#FuzzyMatches') IS NOT NULL
    DROP TABLE #FuzzyMatches;

CREATE TABLE #FuzzyMatches (
    ValorOriginal NVARCHAR(500),
    ValorCanonico NVARCHAR(500),
    Similaridade DECIMAL(5, 2),
    Razao NVARCHAR(200)
);

-- Buscar Origem não mapeadas
INSERT INTO #FuzzyMatches (ValorOriginal, ValorCanonico, Similaridade, Razao)
SELECT DISTINCT
    v.Origem AS ValorOriginal,
    m.ValorCanonico,
    CAST((1.0 - CAST(dbo.LevenshteinDistance(LOWER(LTRIM(RTRIM(v.Origem))), LOWER(m.ValorCanonico)) AS FLOAT) /
        CAST(CASE WHEN LEN(v.Origem) > LEN(m.ValorCanonico) THEN LEN(v.Origem) ELSE LEN(m.ValorCanonico) END AS FLOAT)) * 100 AS DECIMAL(5, 2)) AS Similaridade,
    'Fuzzy match ≥85% (Origem)' AS Razao
FROM dbo.Viagem v
CROSS JOIN #MapeamentoOrigemDestino m
WHERE v.Origem IS NOT NULL
    AND v.Origem <> ''
    AND NOT EXISTS (SELECT 1 FROM #MapeamentoOrigemDestino WHERE ValorAntigo = v.Origem)
    AND CAST((1.0 - CAST(dbo.LevenshteinDistance(LOWER(LTRIM(RTRIM(v.Origem))), LOWER(m.ValorCanonico)) AS FLOAT) /
        CAST(CASE WHEN LEN(v.Origem) > LEN(m.ValorCanonico) THEN LEN(v.Origem) ELSE LEN(m.ValorCanonico) END AS FLOAT)) * 100 AS DECIMAL(5, 2)) >= 85.00
    AND v.Origem <> m.ValorCanonico;

-- Buscar Destino não mapeadas
INSERT INTO #FuzzyMatches (ValorOriginal, ValorCanonico, Similaridade, Razao)
SELECT DISTINCT
    v.Destino AS ValorOriginal,
    m.ValorCanonico,
    CAST((1.0 - CAST(dbo.LevenshteinDistance(LOWER(LTRIM(RTRIM(v.Destino))), LOWER(m.ValorCanonico)) AS FLOAT) /
        CAST(CASE WHEN LEN(v.Destino) > LEN(m.ValorCanonico) THEN LEN(v.Destino) ELSE LEN(m.ValorCanonico) END AS FLOAT)) * 100 AS DECIMAL(5, 2)) AS Similaridade,
    'Fuzzy match ≥85% (Destino)' AS Razao
FROM dbo.Viagem v
CROSS JOIN #MapeamentoOrigemDestino m
WHERE v.Destino IS NOT NULL
    AND v.Destino <> ''
    AND NOT EXISTS (SELECT 1 FROM #MapeamentoOrigemDestino WHERE ValorAntigo = v.Destino)
    AND CAST((1.0 - CAST(dbo.LevenshteinDistance(LOWER(LTRIM(RTRIM(v.Destino))), LOWER(m.ValorCanonico)) AS FLOAT) /
        CAST(CASE WHEN LEN(v.Destino) > LEN(m.ValorCanonico) THEN LEN(v.Destino) ELSE LEN(m.ValorCanonico) END AS FLOAT)) * 100 AS DECIMAL(5, 2)) >= 85.00
    AND v.Destino <> m.ValorCanonico;

-- Adicionar fuzzy matches à tabela de mapeamento (escolhendo o de maior similaridade)
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)
SELECT
    f.ValorOriginal,
    f.ValorCanonico,
    f.Razao + ' (' + CAST(f.Similaridade AS VARCHAR) + '%)'
FROM #FuzzyMatches f
INNER JOIN (
    SELECT ValorOriginal, MAX(Similaridade) AS MaxSimilaridade
    FROM #FuzzyMatches
    GROUP BY ValorOriginal
) maxf ON f.ValorOriginal = maxf.ValorOriginal AND f.Similaridade = maxf.MaxSimilaridade
WHERE NOT EXISTS (SELECT 1 FROM #MapeamentoOrigemDestino WHERE ValorAntigo = f.ValorOriginal);

-- Contar fuzzy matches e armazenar
DECLARE @FuzzyCountTemp INT;
SELECT @FuzzyCountTemp = COUNT(*) FROM #FuzzyMatches;

-- Armazenar na tabela de estatísticas
INSERT INTO #Estatisticas (Chave, Valor) VALUES ('FuzzyCount', @FuzzyCountTemp);

PRINT 'OK - ' + CAST(@FuzzyCountTemp AS VARCHAR) + ' fuzzy matches encontrados e adicionados';
PRINT '';

-- ================================================================================
-- FASE 4: EXECUTAR ATUALIZACOES
-- ================================================================================
PRINT '';
PRINT 'FASE 4: EXECUTAR ATUALIZACOES';
PRINT '======================================================================';

DECLARE @OrigemAtualizadasTemp INT = 0;
DECLARE @DestinoAtualizadasTemp INT = 0;

BEGIN TRANSACTION;

-- Atualizar Origem
UPDATE v
SET v.Origem = m.ValorCanonico
FROM dbo.Viagem v
INNER JOIN #MapeamentoOrigemDestino m ON v.Origem = m.ValorAntigo
WHERE v.Origem IS NOT NULL AND v.Origem <> '';

SET @OrigemAtualizadasTemp = @@ROWCOUNT;

-- Atualizar Destino
UPDATE v
SET v.Destino = m.ValorCanonico
FROM dbo.Viagem v
INNER JOIN #MapeamentoOrigemDestino m ON v.Destino = m.ValorAntigo
WHERE v.Destino IS NOT NULL AND v.Destino <> '';

SET @DestinoAtualizadasTemp = @@ROWCOUNT;

COMMIT TRANSACTION;

-- Armazenar na tabela de estatísticas
INSERT INTO #Estatisticas (Chave, Valor) VALUES ('OrigemAtualizadas', @OrigemAtualizadasTemp);
INSERT INTO #Estatisticas (Chave, Valor) VALUES ('DestinoAtualizadas', @DestinoAtualizadasTemp);

PRINT 'OK - Atualizacoes concluidas:';
PRINT '   - Origem: ' + CAST(@OrigemAtualizadasTemp AS VARCHAR) + ' registros atualizados';
PRINT '   - Destino: ' + CAST(@DestinoAtualizadasTemp AS VARCHAR) + ' registros atualizados';
PRINT '';

-- ================================================================================
-- FASE 5: ESTATISTICAS FINAIS
-- ================================================================================
PRINT '';
PRINT 'FASE 5: ESTATISTICAS FINAIS';
PRINT '======================================================================';

-- Contar valores unicos DEPOIS
DECLARE @OrigemUnicosDepois INT;
DECLARE @DestinoUnicosDepois INT;

SELECT @OrigemUnicosDepois = COUNT(DISTINCT Origem)
FROM dbo.Viagem
WHERE Origem IS NOT NULL AND Origem <> '';

SELECT @DestinoUnicosDepois = COUNT(DISTINCT Destino)
FROM dbo.Viagem
WHERE Destino IS NOT NULL AND Destino <> '';

-- Armazenar na tabela de estatísticas
INSERT INTO #Estatisticas (Chave, Valor) VALUES ('OrigemUnicosDepois', @OrigemUnicosDepois);
INSERT INTO #Estatisticas (Chave, Valor) VALUES ('DestinoUnicosDepois', @DestinoUnicosDepois);

-- Declarar e recuperar valores das estatísticas para FASE 5
DECLARE @TotalRegistros INT;
DECLARE @OrigemUnicosAntes INT;
DECLARE @DestinoUnicosAntes INT;
DECLARE @TotalMapeamentos INT;
DECLARE @FuzzyCount INT;
DECLARE @OrigemAtualizadas INT;
DECLARE @DestinoAtualizadas INT;

SELECT @TotalRegistros = Valor FROM #Estatisticas WHERE Chave = 'TotalRegistros';
SELECT @OrigemUnicosAntes = Valor FROM #Estatisticas WHERE Chave = 'OrigemUnicosAntes';
SELECT @DestinoUnicosAntes = Valor FROM #Estatisticas WHERE Chave = 'DestinoUnicosAntes';
SELECT @TotalMapeamentos = Valor FROM #Estatisticas WHERE Chave = 'TotalMapeamentos';
SELECT @FuzzyCount = Valor FROM #Estatisticas WHERE Chave = 'FuzzyCount';
SELECT @OrigemAtualizadas = Valor FROM #Estatisticas WHERE Chave = 'OrigemAtualizadas';
SELECT @DestinoAtualizadas = Valor FROM #Estatisticas WHERE Chave = 'DestinoAtualizadas';

-- Calcular reducoes
DECLARE @OrigemReducao INT = @OrigemUnicosAntes - @OrigemUnicosDepois;
DECLARE @DestinoReducao INT = @DestinoUnicosAntes - @DestinoUnicosDepois;
DECLARE @OrigemPercentual DECIMAL(5, 2) = CASE WHEN @OrigemUnicosAntes > 0
    THEN CAST(@OrigemReducao AS FLOAT) / @OrigemUnicosAntes * 100 ELSE 0 END;
DECLARE @DestinoPercentual DECIMAL(5, 2) = CASE WHEN @DestinoUnicosAntes > 0
    THEN CAST(@DestinoReducao AS FLOAT) / @DestinoUnicosAntes * 100 ELSE 0 END;

-- PRINT de estatisticas
PRINT '';
PRINT '======================================================================';
PRINT 'LIMPEZA CONCLUIDA COM SUCESSO!';
PRINT '======================================================================';
PRINT '';
PRINT 'RESUMO FINAL:';
PRINT '   Total de registros: ' + CAST(@TotalRegistros AS VARCHAR);
PRINT '   Backup criado em: dbo.Viagem_Backup_OrigemDestino';
PRINT '';
PRINT 'CAMPO ORIGEM:';
PRINT '   Valores unicos (antes): ' + CAST(@OrigemUnicosAntes AS VARCHAR);
PRINT '   Valores unicos (depois): ' + CAST(@OrigemUnicosDepois AS VARCHAR);
PRINT '   Reducao: ' + CAST(@OrigemReducao AS VARCHAR) + ' (' + CAST(@OrigemPercentual AS VARCHAR) + '%)';
PRINT '   Registros atualizados: ' + CAST(@OrigemAtualizadas AS VARCHAR);
PRINT '';
PRINT 'CAMPO DESTINO:';
PRINT '   Valores unicos (antes): ' + CAST(@DestinoUnicosAntes AS VARCHAR);
PRINT '   Valores unicos (depois): ' + CAST(@DestinoUnicosDepois AS VARCHAR);
PRINT '   Reducao: ' + CAST(@DestinoReducao AS VARCHAR) + ' (' + CAST(@DestinoPercentual AS VARCHAR) + '%)';
PRINT '   Registros atualizados: ' + CAST(@DestinoAtualizadas AS VARCHAR);
PRINT '';
PRINT 'MAPEAMENTOS:';
PRINT '   Total de mapeamentos aplicados: ' + CAST(@TotalMapeamentos AS VARCHAR);
PRINT '   Fuzzy matches encontrados: ' + CAST(@FuzzyCount AS VARCHAR);
PRINT '';
PRINT '======================================================================';

-- SELECT para aba separada no SSMS
SELECT
    'ESTATISTICAS DE LIMPEZA' AS [Categoria],
    NULL AS [Metrica],
    NULL AS [Antes],
    NULL AS [Depois],
    NULL AS [Reducao],
    NULL AS [Percentual]
UNION ALL
SELECT
    'GERAL',
    'Total de registros',
    CAST(@TotalRegistros AS VARCHAR),
    CAST(@TotalRegistros AS VARCHAR),
    '0',
    '0%'
UNION ALL
SELECT
    'ORIGEM',
    'Valores unicos',
    CAST(@OrigemUnicosAntes AS VARCHAR),
    CAST(@OrigemUnicosDepois AS VARCHAR),
    CAST(@OrigemReducao AS VARCHAR),
    CAST(@OrigemPercentual AS VARCHAR) + '%'
UNION ALL
SELECT
    'ORIGEM',
    'Registros atualizados',
    '-',
    CAST(@OrigemAtualizadas AS VARCHAR),
    '-',
    '-'
UNION ALL
SELECT
    'DESTINO',
    'Valores unicos',
    CAST(@DestinoUnicosAntes AS VARCHAR),
    CAST(@DestinoUnicosDepois AS VARCHAR),
    CAST(@DestinoReducao AS VARCHAR),
    CAST(@DestinoPercentual AS VARCHAR) + '%'
UNION ALL
SELECT
    'DESTINO',
    'Registros atualizados',
    '-',
    CAST(@DestinoAtualizadas AS VARCHAR),
    '-',
    '-'
UNION ALL
SELECT
    'MAPEAMENTOS',
    'Total aplicados',
    '-',
    CAST(@TotalMapeamentos AS VARCHAR),
    '-',
    '-'
UNION ALL
SELECT
    'MAPEAMENTOS',
    'Fuzzy matches',
    '-',
    CAST(@FuzzyCount AS VARCHAR),
    '-',
    '-';

-- Mostrar TOP 20 valores canonicos mais utilizados
PRINT '';
PRINT 'TOP 20 VALORES CANONICOS MAIS UTILIZADOS:';
PRINT '======================================================================';

SELECT TOP 20
    ValorCanonico AS [Valor Canonico],
    COUNT(*) AS [Quantidade de Ocorrencias],
    CAST(COUNT(*) * 100.0 / @TotalRegistros AS DECIMAL(5, 2)) AS [Percentual (%)]
FROM (
    SELECT Origem AS ValorCanonico FROM dbo.Viagem WHERE Origem IS NOT NULL AND Origem <> ''
    UNION ALL
    SELECT Destino AS ValorCanonico FROM dbo.Viagem WHERE Destino IS NOT NULL AND Destino <> ''
) AS Todos
GROUP BY ValorCanonico
ORDER BY COUNT(*) DESC;

-- Validacao: Verificar se os valores canonicos estao com caracteres corretos
PRINT '';
PRINT 'VALIDACAO DE ENCODING:';
PRINT '======================================================================';

SELECT
    'Origem com caracteres malformados' AS [Tipo de Problema],
    COUNT(*) AS [Quantidade]
FROM dbo.Viagem
WHERE Origem LIKE N'%Ã£%' OR Origem LIKE N'%Ã§%' OR Origem LIKE N'%Ã¡%'
UNION ALL
SELECT
    'Destino com caracteres malformados',
    COUNT(*)
FROM dbo.Viagem
WHERE Destino LIKE N'%Ã£%' OR Destino LIKE N'%Ã§%' OR Destino LIKE N'%Ã§%' OR Destino LIKE N'%Ã¡%';

PRINT '';
PRINT '======================================================================';
PRINT 'SCRIPT FINALIZADO COM SUCESSO!';
PRINT '======================================================================';
PRINT '';
PRINT 'PROXIMOS PASSOS:';
PRINT '   1. Revisar as estatisticas acima';
PRINT '   2. Validar os valores canonicos mais utilizados';
PRINT '   3. Se necessario, adicionar mais mapeamentos';
PRINT '   4. Para reverter: SELECT * FROM dbo.Viagem_Backup_OrigemDestino';
PRINT '';

-- Limpeza de objetos temporarios
DROP TABLE IF EXISTS #MapeamentoOrigemDestino;
DROP TABLE IF EXISTS #FuzzyMatches;
DROP TABLE IF EXISTS #Estatisticas;

GO
