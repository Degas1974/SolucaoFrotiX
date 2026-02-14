-- ============================================================================
-- SQL PARA ADICIONAR FOREIGN KEYS FALTANTES DA TABELA AspNetUsers
-- ============================================================================
--
-- Este script adiciona Foreign Keys para garantir integridade referencial
-- entre AspNetUsers e todas as tabelas que armazenam IDs de usuários.
--
-- Data de Criação: 12/01/2026
-- Gerado por: Claude Code
--
-- IMPORTANTE:
-- - Execute este script em ambiente de DESENVOLVIMENTO primeiro
-- - Verifique se os dados existentes são consistentes antes de aplicar
-- - Algumas FKs podem falhar se houver dados órfãos (usuários inexistentes)
--
-- ============================================================================

USE [FrotiX]
GO

-- ===========================================================================
-- TABELA: Viagem
-- Campos sem FK: UsuarioIdCriacao, UsuarioIdFinalizacao
-- ===========================================================================

-- FK para UsuarioIdCriacao (Usuário que criou a viagem)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Viagem_UsuarioIdCriacao')
BEGIN
    PRINT 'Criando FK_Viagem_UsuarioIdCriacao...'

    -- Verificar se há dados órfãos
    IF EXISTS (
        SELECT 1 FROM Viagem v
        LEFT JOIN AspNetUsers u ON v.UsuarioIdCriacao = u.Id
        WHERE v.UsuarioIdCriacao IS NOT NULL AND u.Id IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem viagens com UsuarioIdCriacao inválido!'
        PRINT '   Execute a query abaixo para ver os registros problemáticos:'
        PRINT '   SELECT ViagemId, UsuarioIdCriacao FROM Viagem v'
        PRINT '   LEFT JOIN AspNetUsers u ON v.UsuarioIdCriacao = u.Id'
        PRINT '   WHERE v.UsuarioIdCriacao IS NOT NULL AND u.Id IS NULL'
        PRINT ''
        PRINT '   Considere corrigir os dados antes de criar a FK'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Viagem WITH NOCHECK
            ADD CONSTRAINT FK_Viagem_UsuarioIdCriacao
            FOREIGN KEY (UsuarioIdCriacao)
            REFERENCES dbo.AspNetUsers (Id)

        PRINT '✅ FK_Viagem_UsuarioIdCriacao criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Viagem_UsuarioIdCriacao já existe'
END
GO

-- FK para UsuarioIdFinalizacao (Usuário que finalizou a viagem)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Viagem_UsuarioIdFinalizacao')
BEGIN
    PRINT 'Criando FK_Viagem_UsuarioIdFinalizacao...'

    -- Verificar se há dados órfãos
    IF EXISTS (
        SELECT 1 FROM Viagem v
        LEFT JOIN AspNetUsers u ON v.UsuarioIdFinalizacao = u.Id
        WHERE v.UsuarioIdFinalizacao IS NOT NULL AND u.Id IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem viagens com UsuarioIdFinalizacao inválido!'
        PRINT '   Execute a query abaixo para ver os registros problemáticos:'
        PRINT '   SELECT ViagemId, UsuarioIdFinalizacao FROM Viagem v'
        PRINT '   LEFT JOIN AspNetUsers u ON v.UsuarioIdFinalizacao = u.Id'
        PRINT '   WHERE v.UsuarioIdFinalizacao IS NOT NULL AND u.Id IS NULL'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Viagem WITH NOCHECK
            ADD CONSTRAINT FK_Viagem_UsuarioIdFinalizacao
            FOREIGN KEY (UsuarioIdFinalizacao)
            REFERENCES dbo.AspNetUsers (Id)

        PRINT '✅ FK_Viagem_UsuarioIdFinalizacao criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Viagem_UsuarioIdFinalizacao já existe'
END
GO

-- ===========================================================================
-- TABELA: Manutencao
-- Campos sem FK: IdUsuarioCriacao, IdUsuarioFinalizacao, IdUsuarioCancelamento
-- (IdUsuarioAlteracao já tem FK conforme Frotix.sql linha 3867)
-- ===========================================================================

-- FK para IdUsuarioCriacao (Usuário que criou a manutenção)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCriacao')
BEGIN
    PRINT 'Criando FK_Manutencao_IdUsuarioCriacao...'

    -- Verificar se há dados órfãos
    IF EXISTS (
        SELECT 1 FROM Manutencao m
        LEFT JOIN AspNetUsers u ON m.IdUsuarioCriacao = u.Id
        WHERE m.IdUsuarioCriacao IS NOT NULL AND u.Id IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem manutenções com IdUsuarioCriacao inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Manutencao WITH NOCHECK
            ADD CONSTRAINT FK_Manutencao_IdUsuarioCriacao
            FOREIGN KEY (IdUsuarioCriacao)
            REFERENCES dbo.AspNetUsers (Id)

        PRINT '✅ FK_Manutencao_IdUsuarioCriacao criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Manutencao_IdUsuarioCriacao já existe'
END
GO

-- FK para IdUsuarioFinalizacao (Usuário que finalizou a manutenção)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioFinalizacao')
BEGIN
    PRINT 'Criando FK_Manutencao_IdUsuarioFinalizacao...'

    -- Verificar se há dados órfãos
    IF EXISTS (
        SELECT 1 FROM Manutencao m
        LEFT JOIN AspNetUsers u ON m.IdUsuarioFinalizacao = u.Id
        WHERE m.IdUsuarioFinalizacao IS NOT NULL AND u.Id IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem manutenções com IdUsuarioFinalizacao inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Manutencao WITH NOCHECK
            ADD CONSTRAINT FK_Manutencao_IdUsuarioFinalizacao
            FOREIGN KEY (IdUsuarioFinalizacao)
            REFERENCES dbo.AspNetUsers (Id)

        PRINT '✅ FK_Manutencao_IdUsuarioFinalizacao criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Manutencao_IdUsuarioFinalizacao já existe'
END
GO

-- FK para IdUsuarioCancelamento (Usuário que cancelou a manutenção)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCancelamento')
BEGIN
    PRINT 'Criando FK_Manutencao_IdUsuarioCancelamento...'

    -- Verificar se há dados órfãos
    IF EXISTS (
        SELECT 1 FROM Manutencao m
        LEFT JOIN AspNetUsers u ON m.IdUsuarioCancelamento = u.Id
        WHERE m.IdUsuarioCancelamento IS NOT NULL AND u.Id IS NULL
    )
    BEGIN
        PRINT '⚠️  AVISO: Existem manutenções com IdUsuarioCancelamento inválido!'
        PRINT ''
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Manutencao WITH NOCHECK
            ADD CONSTRAINT FK_Manutencao_IdUsuarioCancelamento
            FOREIGN KEY (IdUsuarioCancelamento)
            REFERENCES dbo.AspNetUsers (Id)

        PRINT '✅ FK_Manutencao_IdUsuarioCancelamento criada com sucesso!'
    END
END
ELSE
BEGIN
    PRINT '✓  FK_Manutencao_IdUsuarioCancelamento já existe'
END
GO

-- ===========================================================================
-- VERIFICAÇÃO FINAL
-- ===========================================================================

PRINT ''
PRINT '============================================================================'
PRINT 'VERIFICAÇÃO FINAL - FOREIGN KEYS DA AspNetUsers'
PRINT '============================================================================'
PRINT ''

-- Listar todas as FKs da AspNetUsers
SELECT
    fk.name AS 'Foreign Key',
    OBJECT_NAME(fk.parent_object_id) AS 'Tabela',
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS 'Coluna',
    'AspNetUsers' AS 'Tabela Referenciada',
    'Id' AS 'Coluna Referenciada'
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc
    ON fk.object_id = fkc.constraint_object_id
WHERE fk.referenced_object_id = OBJECT_ID('AspNetUsers')
ORDER BY OBJECT_NAME(fk.parent_object_id), fk.name

PRINT ''
PRINT '============================================================================'
PRINT 'SCRIPT CONCLUÍDO!'
PRINT '============================================================================'
PRINT ''
PRINT 'PRÓXIMOS PASSOS:'
PRINT '1. Verifique se há avisos sobre dados órfãos acima'
PRINT '2. Se houver, corrija os dados antes de aplicar em produção'
PRINT '3. Teste a exclusão de usuários para validar as FKs'
PRINT '4. Após validar, aplique em PRODUÇÃO com cautela'
PRINT ''
GO
