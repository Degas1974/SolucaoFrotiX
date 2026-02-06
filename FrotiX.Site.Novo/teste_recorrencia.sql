"-- Script de teste para verificar a funcionalidade de alertas recorrentes
-- Execute após testar a criação de alertas recorrentes via interface

-- 1. Verificar estrutura da tabela
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'AlertasFrotiX' 
AND COLUMN_NAME LIKE '%Recorrencia%' 
   OR COLUMN_NAME LIKE '%Recorrente%'
   OR COLUMN_NAME IN ('Intervalo', 'DataFinalRecorrencia', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday', 'DiaMesRecorrencia', 'DatasSelecionadas')
ORDER BY ORDINAL_POSITION;

-- 2. Verificar constraints
SELECT 
    tc.CONSTRAINT_NAME, 
    tc.CONSTRAINT_TYPE,
    cc.CHECK_CLAUSE
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
LEFT JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc ON tc.CONSTRAINT_NAME = cc.CONSTRAINT_NAME
WHERE tc.TABLE_NAME = 'AlertasFrotiX' 
AND tc.CONSTRAINT_NAME LIKE 'CK_AlertasFrotiX_%';

-- 3. Exemplo de inserção manual (para teste)
DECLARE @RecorrenciaAlertaId UNIQUEIDENTIFIER = NEWID();

-- Inserir alertas recorrentes (exemplo: tipo 8 - Dias Variados)
INSERT INTO AlertasFrotiX (
    AlertasFrotiXId,
    Titulo,
    Descricao,
    TipoAlerta,
    Prioridade,
    TipoExibicao,
    DataExibicao,
    HorarioExibicao,
    DataExpiracao,
    DiasSemana,
    DiaMesRecorrencia,
    DataInsercao,
    UsuarioCriadorId,
    Ativo,
    Recorrente,
    RecorrenciaAlertaId,
    Intervalo,
    DataFinalRecorrencia,
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday,
    DatasSelecionadas
)
VALUES 
    (NEWID(), 'Teste Recorrente 1', 'Descrição teste', 1, 1, 8, '2024-01-15', '09:00', '2024-12-31', NULL, NULL, GETDATE(), 'teste', 1, 'S', @RecorrenciaAlertaId, '8', '2024-12-31', 0,0,0,0,0,0,0, '2024-01-15,2024-01-20,2024-01-25'),
    (NEWID(), 'Teste Recorrente 2', 'Descrição teste', 1, 1, 8, '2024-01-20', '09:00', '2024-12-31', NULL, NULL, GETDATE(), 'teste', 1, 'S', @RecorrenciaAlertaId, '8', '2024-12-31', 0,0,0,0,0,0,0, '2024-01-15,2024-01-20,2024-01-25'),
    (NEWID(), 'Teste Recorrente 3', 'Descrição teste', 1, 1, 8, '2024-01-25', '09:00', '2024-12-31', NULL, NULL, GETDATE(), 'teste', 1, 'S', @RecorrenciaAlertaId, '8', '2024-12-31', 0,0,0,0,0,0,0, '2024-01-15,2024-01-20,2024-01-25');

-- 4. Verificar alertas inseridos
SELECT 
    AlertasFrotiXId,
    Titulo,
    TipoExibicao,
    DataExibicao,
    Recorrente,
    RecorrenciaAlertaId,
    Intervalo,
    DataFinalRecorrencia,
    DatasSelecionadas
FROM AlertasFrotiX 
WHERE RecorrenciaAlertaId = @RecorrenciaAlertaId
ORDER BY DataExibicao;

-- 5. Comparar com estrutura da tabela Viagem (agendamentos)
SELECT 'Viagem' as Tabela, COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Viagem' 
AND COLUMN_NAME LIKE '%Recorrencia%' 
   OR COLUMN_NAME LIKE '%Recorrente%'
   OR COLUMN_NAME IN ('Intervalo', 'DatasSelecionadas', 'DiaMesRecorrencia')
UNION ALL
SELECT 'AlertasFrotiX' as Tabela, COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'AlertasFrotiX' 
AND COLUMN_NAME LIKE '%Recorrencia%' 
   OR COLUMN_NAME LIKE '%Recorrente%'
   OR COLUMN_NAME IN ('Intervalo', 'DataFinalRecorrencia', 'DiaMesRecorrencia', 'DatasSelecionadas', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday')
ORDER BY Tabela, COLUMN_NAME;"