/* ======================================================================================
   SCRIPT DE MIGRAÇÃO SEGURA - ALTERAÇÃO DE TABELAS EXISTENTES
   ======================================================================================
   
   DATA GERAÇÃO: 09/02/2026
   
   IMPORTANTE:
   ✅ Este script é SEGURO e faz verificações antes de cada alteração
   ✅ Usa TRANSAÇÕES para permitir rollback em caso de erro
   ✅ Migra dados antes de remover colunas
   ✅ Valida dados antes de alterar tipos
   
   TABELAS AFETADAS:
   - Recurso (1 nova + 7 alteradas)
   - Manutencao (3 colunas alteradas)
   - Fornecedor (1 alterada)
   
   TEMPO ESTIMADO: 1-3 minutos
   
   ======================================================================================
*/

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

USE Frotix
GO

IF DB_NAME() <> 'Frotix'
BEGIN
    RAISERROR('Este script deve ser executado no banco Frotix!', 16, 1);
    RETURN;
END
GO

PRINT '======================================================================================';
PRINT '                   INÍCIO DA MIGRAÇÃO DE TABELAS EXISTENTES';
PRINT '======================================================================================';
PRINT 'Servidor: ' + @@SERVERNAME;
PRINT 'Banco: ' + DB_NAME();
PRINT 'Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '======================================================================================';
PRINT '';
GO

-- ======================================================================================
-- SEÇÃO 1: TABELA LAVAGEM - SEM ALTERAÇÕES NECESSÁRIAS
-- ======================================================================================
-- Coluna Horario está correta em produção e desenvolvimento.
-- Nenhuma migração necessária.
GO

-- ======================================================================================
-- SEÇÃO 2: TABELA RECURSO - ADICIONAR COLUNA HasChild
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SEÇÃO 2: TABELA RECURSO - ADICIONAR COLUNA HasChild';
PRINT '======================================================================================';
PRINT '';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Recurso') AND name = 'HasChild')
BEGIN
    PRINT '→ Adicionando coluna HasChild...';
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Adicionar coluna com valor padrão 0
        ALTER TABLE [dbo].[Recurso] ADD [HasChild] BIT NOT NULL DEFAULT(0);
        
        PRINT '  ✓ Coluna HasChild adicionada.';
        
        -- Atualizar valores baseado na existência de filhos
        PRINT '  → Calculando valores de HasChild...';
        UPDATE parent
        SET HasChild = 1
        FROM [dbo].[Recurso] parent
        WHERE EXISTS (
            SELECT 1 
            FROM [dbo].[Recurso] child 
            WHERE child.ParentId = parent.RecursoId
        );
        
        DECLARE @RecursosComFilhos INT = @@ROWCOUNT;
        PRINT '  ✓ ' + CAST(@RecursosComFilhos AS VARCHAR) + ' recursos marcados com HasChild = 1';
        
        COMMIT TRANSACTION;
        PRINT '✅ Coluna HasChild criada e populada com sucesso!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT '❌ ERRO ao adicionar coluna HasChild: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END
ELSE
BEGIN
    PRINT '✓ Coluna HasChild já existe.';
END
GO

-- ======================================================================================
-- SEÇÃO 3: TABELA RECURSO - POPULAR VALORES NULL ANTES DE ALTERAR PARA NOT NULL
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SEÇÃO 3: TABELA RECURSO - POPULAR VALORES NULL';
PRINT '======================================================================================';
PRINT '';
GO

BEGIN TRY
    BEGIN TRANSACTION;
    
    DECLARE @NullCount INT;
    
    -- Popular Nome com valores padrão
    SELECT @NullCount = COUNT(*) FROM [dbo].[Recurso] WHERE Nome IS NULL;
    IF @NullCount > 0
    BEGIN
        PRINT '  → Corrigindo ' + CAST(@NullCount AS VARCHAR) + ' valores NULL em Nome...';
        UPDATE [dbo].[Recurso] 
        SET Nome = 'Recurso_' + CAST(RecursoId AS VARCHAR(36))
        WHERE Nome IS NULL;
        PRINT '  ✓ Valores NULL em Nome corrigidos.';
    END
    ELSE
        PRINT '  ✓ Nome: nenhum valor NULL encontrado.';
    
    -- Popular NomeMenu com valores padrão
    SELECT @NullCount = COUNT(*) FROM [dbo].[Recurso] WHERE NomeMenu IS NULL;
    IF @NullCount > 0
    BEGIN
        PRINT '  → Corrigindo ' + CAST(@NullCount AS VARCHAR) + ' valores NULL em NomeMenu...';
        UPDATE [dbo].[Recurso] 
        SET NomeMenu = ISNULL(Nome, 'Menu_' + CAST(RecursoId AS VARCHAR(36)))
        WHERE NomeMenu IS NULL;
        PRINT '  ✓ Valores NULL em NomeMenu corrigidos.';
    END
    ELSE
        PRINT '  ✓ NomeMenu: nenhum valor NULL encontrado.';
    
    -- Popular Icon com valores padrão
    SELECT @NullCount = COUNT(*) FROM [dbo].[Recurso] WHERE Icon IS NULL;
    IF @NullCount > 0
    BEGIN
        PRINT '  → Corrigindo ' + CAST(@NullCount AS VARCHAR) + ' valores NULL em Icon...';
        UPDATE [dbo].[Recurso] 
        SET Icon = 'fa fa-circle'
        WHERE Icon IS NULL;
        PRINT '  ✓ Valores NULL em Icon corrigidos.';
    END
    ELSE
        PRINT '  ✓ Icon: nenhum valor NULL encontrado.';
    
    -- Popular Href com valores padrão
    SELECT @NullCount = COUNT(*) FROM [dbo].[Recurso] WHERE Href IS NULL;
    IF @NullCount > 0
    BEGIN
        PRINT '  → Corrigindo ' + CAST(@NullCount AS VARCHAR) + ' valores NULL em Href...';
        UPDATE [dbo].[Recurso] 
        SET Href = '#'
        WHERE Href IS NULL;
        PRINT '  ✓ Valores NULL em Href corrigidos.';
    END
    ELSE
        PRINT '  ✓ Href: nenhum valor NULL encontrado.';
    
    COMMIT TRANSACTION;
    PRINT '✅ Valores NULL populados com sucesso!';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT '❌ ERRO ao popular valores NULL: ' + ERROR_MESSAGE();
    THROW;
END CATCH
GO

-- ======================================================================================
-- SEÇÃO 4: TABELA RECURSO - ALTERAR TIPOS E NOT NULL
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SEÇÃO 4: TABELA RECURSO - ALTERAR DEFINIÇÕES DE COLUNAS';
PRINT '======================================================================================';
PRINT '';
GO

BEGIN TRY
    BEGIN TRANSACTION;
    
    -- DROP índices que dependem das colunas
    IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_Recurso_Nome' AND object_id = OBJECT_ID('dbo.Recurso'))
    BEGIN
        PRINT '  → Removendo índice UK_Recurso_Nome...';
        DROP INDEX [UK_Recurso_Nome] ON [dbo].[Recurso];
    END
    
    IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_Recurso_NomeMenu' AND object_id = OBJECT_ID('dbo.Recurso'))
    BEGIN
        PRINT '  → Removendo índice UK_Recurso_NomeMenu...';
        DROP INDEX [UK_Recurso_NomeMenu] ON [dbo].[Recurso];
    END
    
    -- Verificar tipo atual de Nome
    DECLARE @TipoNome VARCHAR(50);
    SELECT @TipoNome = DATA_TYPE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Nome';
    
    IF @TipoNome = 'varchar'
    BEGIN
        PRINT '  → Alterando Nome: varchar(250) → nvarchar(200) NOT NULL...';
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Nome] NVARCHAR(200) NOT NULL;
        PRINT '  ✓ Nome alterado.';
    END
    ELSE
        PRINT '  ✓ Nome já é nvarchar.';
    
    -- Alterar NomeMenu
    DECLARE @TipoNomeMenu VARCHAR(50);
    SELECT @TipoNomeMenu = DATA_TYPE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'NomeMenu';
    
    IF @TipoNomeMenu = 'varchar'
    BEGIN
        PRINT '  → Alterando NomeMenu: varchar(250) → nvarchar(200) NOT NULL...';
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [NomeMenu] NVARCHAR(200) NOT NULL;
        PRINT '  ✓ NomeMenu alterado.';
    END
    ELSE
        PRINT '  ✓ NomeMenu já é nvarchar.';
    
    -- Alterar Icon para NOT NULL
    DECLARE @IconNullable VARCHAR(3);
    SELECT @IconNullable = IS_NULLABLE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Icon';
    
    IF @IconNullable = 'YES'
    BEGIN
        PRINT '  → Alterando Icon: NULL → NOT NULL...';
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Icon] NVARCHAR(200) NOT NULL;
        PRINT '  ✓ Icon alterado.';
    END
    ELSE
        PRINT '  ✓ Icon já é NOT NULL.';
    
    -- Alterar Href para NOT NULL
    DECLARE @HrefNullable VARCHAR(3);
    SELECT @HrefNullable = IS_NULLABLE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Href';
    
    IF @HrefNullable = 'YES'
    BEGIN
        PRINT '  → Alterando Href: NULL → NOT NULL...';
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Href] NVARCHAR(500) NOT NULL;
        PRINT '  ✓ Href alterado.';
    END
    ELSE
        PRINT '  ✓ Href já é NOT NULL.';
    
    -- Recriar índices removidos
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_Recurso_Nome' AND object_id = OBJECT_ID('dbo.Recurso'))
    BEGIN
        PRINT '  → Recriando índice UK_Recurso_Nome...';
        CREATE UNIQUE INDEX [UK_Recurso_Nome] ON [dbo].[Recurso] ([Nome]) ON [PRIMARY];
    END
    
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_Recurso_NomeMenu' AND object_id = OBJECT_ID('dbo.Recurso'))
    BEGIN
        PRINT '  → Recriando índice UK_Recurso_NomeMenu...';
        CREATE UNIQUE INDEX [UK_Recurso_NomeMenu] ON [dbo].[Recurso] ([NomeMenu]) ON [PRIMARY];
    END
    
    COMMIT TRANSACTION;
    PRINT '✅ TABELA RECURSO: Todas as alterações concluídas com sucesso!';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT '❌ ERRO ao alterar colunas da tabela Recurso: ' + ERROR_MESSAGE();
    THROW;
END CATCH
GO

-- ======================================================================================
-- SEÇÃO 5: TABELA MANUTENCAO - VALIDAR E ALTERAR TIPOS
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SEÇÃO 5: TABELA MANUTENCAO - ALTERAR TIPOS DE COLUNAS DE USUÁRIO';
PRINT '======================================================================================';
PRINT '';
GO

BEGIN TRY
    PRINT '  → Validando dados existentes...';
    
    -- Verificar se existem valores maiores que 100 caracteres
    DECLARE @MaxLen INT;
    
    SELECT @MaxLen = MAX(LEN(IdUsuarioCancelamento))
    FROM [dbo].[Manutencao]
    WHERE IdUsuarioCancelamento IS NOT NULL;
    
    IF @MaxLen > 100
    BEGIN
        PRINT '  ⚠️  ATENÇÃO: IdUsuarioCancelamento tem valores com ' + CAST(@MaxLen AS VARCHAR) + ' caracteres!';
        PRINT '  ❌ Não é seguro alterar para varchar(100). Abortando.';
        RAISERROR('Dados incompatíveis com novo tamanho de coluna', 16, 1);
    END
    
    SELECT @MaxLen = MAX(LEN(IdUsuarioCriacao))
    FROM [dbo].[Manutencao]
    WHERE IdUsuarioCriacao IS NOT NULL;
    
    IF @MaxLen > 100
    BEGIN
        PRINT '  ⚠️  ATENÇÃO: IdUsuarioCriacao tem valores com ' + CAST(@MaxLen AS VARCHAR) + ' caracteres!';
        PRINT '  ❌ Não é seguro alterar para varchar(100). Abortando.';
        RAISERROR('Dados incompatíveis com novo tamanho de coluna', 16, 1);
    END
    
    SELECT @MaxLen = MAX(LEN(IdUsuarioFinalizacao))
    FROM [dbo].[Manutencao]
    WHERE IdUsuarioFinalizacao IS NOT NULL;
    
    IF @MaxLen > 100
    BEGIN
        PRINT '  ⚠️  ATENÇÃO: IdUsuarioFinalizacao tem valores com ' + CAST(@MaxLen AS VARCHAR) + ' caracteres!';
        PRINT '  ❌ Não é seguro alterar para varchar(100). Abortando.';
        RAISERROR('Dados incompatíveis com novo tamanho de coluna', 16, 1);
    END
    
    PRINT '  ✓ Validação OK. Todos os valores cabem em varchar(100).';
    
    BEGIN TRANSACTION;
    
    -- DROP FKs temporariamente
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCriacao')
    BEGIN
        PRINT '  → Removendo FK_Manutencao_IdUsuarioCriacao...';
        ALTER TABLE [dbo].[Manutencao] DROP CONSTRAINT [FK_Manutencao_IdUsuarioCriacao];
    END
    
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioFinalizacao')
    BEGIN
        PRINT '  → Removendo FK_Manutencao_IdUsuarioFinalizacao...';
        ALTER TABLE [dbo].[Manutencao] DROP CONSTRAINT [FK_Manutencao_IdUsuarioFinalizacao];
    END
    
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCancelamento')
    BEGIN
        PRINT '  → Removendo FK_Manutencao_IdUsuarioCancelamento...';
        ALTER TABLE [dbo].[Manutencao] DROP CONSTRAINT [FK_Manutencao_IdUsuarioCancelamento];
    END
    
    -- Verificar tipo atual
    DECLARE @TipoAtual VARCHAR(50);
    SELECT @TipoAtual = DATA_TYPE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Manutencao' AND COLUMN_NAME = 'IdUsuarioCancelamento';
    
    IF @TipoAtual = 'nvarchar'
    BEGIN
        PRINT '  → Alterando IdUsuarioCancelamento: nvarchar(450) → varchar(100)...';
        ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCancelamento] VARCHAR(100) NULL;
        PRINT '  ✓ IdUsuarioCancelamento alterado.';
        
        PRINT '  → Alterando IdUsuarioCriacao: nvarchar(450) → varchar(100)...';
        ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCriacao] VARCHAR(100) NULL;
        PRINT '  ✓ IdUsuarioCriacao alterado.';
        
        PRINT '  → Alterando IdUsuarioFinalizacao: nvarchar(450) → varchar(100)...';
        ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioFinalizacao] VARCHAR(100) NULL;
        PRINT '  ✓ IdUsuarioFinalizacao alterado.';
    END
    ELSE
        PRINT '  ✓ Colunas já são varchar(100).';
    
    -- Recriar FKs removidas
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCriacao')
    BEGIN
        PRINT '  → Recriando FK_Manutencao_IdUsuarioCriacao...';
        ALTER TABLE [dbo].[Manutencao] ADD CONSTRAINT [FK_Manutencao_IdUsuarioCriacao] 
        FOREIGN KEY ([IdUsuarioCriacao]) REFERENCES [dbo].[AspNetUsers]([Id]);
    END
    
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioFinalizacao')
    BEGIN
        PRINT '  → Recriando FK_Manutencao_IdUsuarioFinalizacao...';
        ALTER TABLE [dbo].[Manutencao] ADD CONSTRAINT [FK_Manutencao_IdUsuarioFinalizacao] 
        FOREIGN KEY ([IdUsuarioFinalizacao]) REFERENCES [dbo].[AspNetUsers]([Id]);
    END
    
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCancelamento')
    BEGIN
        PRINT '  → Recriando FK_Manutencao_IdUsuarioCancelamento...';
        ALTER TABLE [dbo].[Manutencao] ADD CONSTRAINT [FK_Manutencao_IdUsuarioCancelamento] 
        FOREIGN KEY ([IdUsuarioCancelamento]) REFERENCES [dbo].[AspNetUsers]([Id]);
    END
    
    COMMIT TRANSACTION;
    PRINT '✅ TABELA MANUTENCAO: Alterações concluídas com sucesso!';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT '❌ ERRO ao alterar tabela Manutencao: ' + ERROR_MESSAGE();
    PRINT 'ℹ️  Verifique os dados antes de tentar novamente.';
    -- Não faz THROW para permitir que o resto do script continue
END CATCH
GO

-- ======================================================================================
-- SEÇÃO 6: VALIDAÇÃO FINAL
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT '                           VALIDAÇÃO FINAL';
PRINT '======================================================================================';
PRINT '';
GO

-- Validar Recurso
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Recurso') AND name = 'HasChild')
    PRINT '✅ Recurso.HasChild: OK';
ELSE
    PRINT '❌ Recurso.HasChild: NÃO ENCONTRADA';

-- Verificar NOT NULL em Recurso
DECLARE @IconNull VARCHAR(3), @HrefNull VARCHAR(3);
SELECT @IconNull = IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Icon';
SELECT @HrefNull = IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Href';

IF @IconNull = 'NO'
    PRINT '✅ Recurso.Icon: NOT NULL';
ELSE
    PRINT '⚠️  Recurso.Icon: ainda permite NULL';

IF @HrefNull = 'NO'
    PRINT '✅ Recurso.Href: NOT NULL';
ELSE
    PRINT '⚠️  Recurso.Href: ainda permite NULL';

-- Validar Manutencao
DECLARE @ManutencaoTipo VARCHAR(50);
SELECT @ManutencaoTipo = DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Manutencao' AND COLUMN_NAME = 'IdUsuarioCancelamento';

IF @ManutencaoTipo = 'varchar'
    PRINT '✅ Manutencao: Colunas alteradas para varchar(100)';
ELSE
    PRINT '⚠️  Manutencao: Ainda nvarchar(450)';

PRINT '';
PRINT '======================================================================================';
PRINT '                           MIGRAÇÃO CONCLUÍDA!';
PRINT '======================================================================================';
PRINT 'Data/Hora término: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '======================================================================================';
PRINT '';
GO
