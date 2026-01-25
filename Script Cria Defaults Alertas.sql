-- ============================================================================
-- SCRIPT: Correção de NULLs SEM TRIGGERS (compatível com Entity Framework)
-- Data: 25/11/2025
-- Problema: EF usa OUTPUT que não funciona com triggers
-- Solução: Usar DEFAULT constraints + UPDATE inicial
-- ============================================================================

-- ============================================================================
-- PARTE 1: REMOVER TRIGGERS EXISTENTES
-- ============================================================================

PRINT '=== REMOVENDO TRIGGERS ==='

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_AlertasFrotiX_PreencherNulos')
BEGIN
    DROP TRIGGER TR_AlertasFrotiX_PreencherNulos;
    PRINT 'TR_AlertasFrotiX_PreencherNulos removido'
END

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_AlertasUsuario_PreencherNulos')
BEGIN
    DROP TRIGGER TR_AlertasUsuario_PreencherNulos;
    PRINT 'TR_AlertasUsuario_PreencherNulos removido'
END

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Viagem_PreencherNulos')
BEGIN
    DROP TRIGGER TR_Viagem_PreencherNulos;
    PRINT 'TR_Viagem_PreencherNulos removido'
END

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Manutencao_PreencherNulos')
BEGIN
    DROP TRIGGER TR_Manutencao_PreencherNulos;
    PRINT 'TR_Manutencao_PreencherNulos removido'
END

PRINT '=== TRIGGERS REMOVIDOS ==='
GO

-- ============================================================================
-- PARTE 2: ADICIONAR DEFAULT CONSTRAINTS NA TABELA AlertasFrotiX
-- (Verifica se a COLUNA já tem default, não apenas o nome)
-- ============================================================================

PRINT '=== ADICIONANDO DEFAULTS EM AlertasFrotiX ==='

-- Recorrente
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Recorrente')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Recorrente DEFAULT 'N' FOR Recorrente;
    PRINT 'Default Recorrente adicionado'
END
ELSE PRINT 'Recorrente já tem default'

-- Monday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Monday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Monday DEFAULT 0 FOR Monday;
    PRINT 'Default Monday adicionado'
END
ELSE PRINT 'Monday já tem default'

-- Tuesday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Tuesday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Tuesday DEFAULT 0 FOR Tuesday;
    PRINT 'Default Tuesday adicionado'
END
ELSE PRINT 'Tuesday já tem default'

-- Wednesday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Wednesday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Wednesday DEFAULT 0 FOR Wednesday;
    PRINT 'Default Wednesday adicionado'
END
ELSE PRINT 'Wednesday já tem default'

-- Thursday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Thursday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Thursday DEFAULT 0 FOR Thursday;
    PRINT 'Default Thursday adicionado'
END
ELSE PRINT 'Thursday já tem default'

-- Friday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Friday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Friday DEFAULT 0 FOR Friday;
    PRINT 'Default Friday adicionado'
END
ELSE PRINT 'Friday já tem default'

-- Saturday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Saturday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Saturday DEFAULT 0 FOR Saturday;
    PRINT 'Default Saturday adicionado'
END
ELSE PRINT 'Saturday já tem default'

-- Sunday
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'Sunday')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_Sunday DEFAULT 0 FOR Sunday;
    PRINT 'Default Sunday adicionado'
END
ELSE PRINT 'Sunday já tem default'

-- DiasSemana
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'DiasSemana')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_DiasSemana DEFAULT '' FOR DiasSemana;
    PRINT 'Default DiasSemana adicionado'
END
ELSE PRINT 'DiasSemana já tem default'

-- DatasSelecionadas
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasFrotiX') AND c.name = 'DatasSelecionadas')
BEGIN
    ALTER TABLE AlertasFrotiX ADD CONSTRAINT DF_AlertasFrotiX_DatasSelecionadas DEFAULT '' FOR DatasSelecionadas;
    PRINT 'Default DatasSelecionadas adicionado'
END
ELSE PRINT 'DatasSelecionadas já tem default'

GO

-- ============================================================================
-- PARTE 3: ADICIONAR DEFAULT CONSTRAINTS NA TABELA AlertasUsuario
-- ============================================================================

PRINT '=== ADICIONANDO DEFAULTS EM AlertasUsuario ==='

-- Lido
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasUsuario') AND c.name = 'Lido')
BEGIN
    ALTER TABLE AlertasUsuario ADD CONSTRAINT DF_AlertasUsuario_Lido DEFAULT 0 FOR Lido;
    PRINT 'Default Lido adicionado'
END
ELSE PRINT 'Lido já tem default'

-- Notificado
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasUsuario') AND c.name = 'Notificado')
BEGIN
    ALTER TABLE AlertasUsuario ADD CONSTRAINT DF_AlertasUsuario_Notificado DEFAULT 0 FOR Notificado;
    PRINT 'Default Notificado adicionado'
END
ELSE PRINT 'Notificado já tem default'

-- Apagado
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc 
               JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
               WHERE dc.parent_object_id = OBJECT_ID('AlertasUsuario') AND c.name = 'Apagado')
BEGIN
    ALTER TABLE AlertasUsuario ADD CONSTRAINT DF_AlertasUsuario_Apagado DEFAULT 0 FOR Apagado;
    PRINT 'Default Apagado adicionado'
END
ELSE PRINT 'Apagado já tem default'

GO

-- ============================================================================
-- PARTE 4: ATUALIZAR REGISTROS EXISTENTES
-- ============================================================================

PRINT '=== ATUALIZANDO REGISTROS EXISTENTES ==='

-- AlertasFrotiX
UPDATE AlertasFrotiX
SET 
    Recorrente = ISNULL(Recorrente, 'N'),
    Monday = ISNULL(Monday, 0),
    Tuesday = ISNULL(Tuesday, 0),
    Wednesday = ISNULL(Wednesday, 0),
    Thursday = ISNULL(Thursday, 0),
    Friday = ISNULL(Friday, 0),
    Saturday = ISNULL(Saturday, 0),
    Sunday = ISNULL(Sunday, 0),
    DiasSemana = ISNULL(DiasSemana, ''),
    DatasSelecionadas = ISNULL(DatasSelecionadas, '')
WHERE 
    Recorrente IS NULL 
    OR Monday IS NULL OR Tuesday IS NULL OR Wednesday IS NULL
    OR Thursday IS NULL OR Friday IS NULL OR Saturday IS NULL OR Sunday IS NULL
    OR DiasSemana IS NULL OR DatasSelecionadas IS NULL;

PRINT 'AlertasFrotiX atualizados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- AlertasUsuario
UPDATE AlertasUsuario
SET 
    Lido = ISNULL(Lido, 0),
    Notificado = ISNULL(Notificado, 0),
    Apagado = ISNULL(Apagado, 0)
WHERE 
    Lido IS NULL OR Notificado IS NULL OR Apagado IS NULL;

PRINT 'AlertasUsuario atualizados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

GO

-- ============================================================================
-- PARTE 5: VERIFICAÇÃO FINAL
-- ============================================================================

PRINT '=== VERIFICAÇÃO FINAL ==='

SELECT 'AlertasFrotiX' AS Tabela,
    COUNT(*) AS Total,
    SUM(CASE WHEN Recorrente IS NULL THEN 1 ELSE 0 END) AS RecorrenteNull,
    SUM(CASE WHEN Monday IS NULL THEN 1 ELSE 0 END) AS MondayNull
FROM AlertasFrotiX;

SELECT 'AlertasUsuario' AS Tabela,
    COUNT(*) AS Total,
    SUM(CASE WHEN Lido IS NULL THEN 1 ELSE 0 END) AS LidoNull,
    SUM(CASE WHEN Notificado IS NULL THEN 1 ELSE 0 END) AS NotificadoNull
FROM AlertasUsuario;

PRINT '=== SCRIPT FINALIZADO COM SUCESSO ==='
GO