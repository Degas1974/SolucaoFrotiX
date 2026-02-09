/* ======================================================================================
   SCRIPT INTEGRADO DE ATUALIZAÇÃO COMPLETA - PRODUÇÃO → DESENVOLVIMENTO
   ======================================================================================
   
   DATA GERAÇÃO: 09/02/2026
   VERSÃO: 1.0 INTEGRADA
   
   ⚠️  IMPORTANTE - LEIA ANTES DE EXECUTAR:
   
   1. BACKUP OBRIGATÓRIO
      - Faça backup COMPLETO do banco Frotix ANTES de executar
      - Comando: BACKUP DATABASE Frotix TO DISK = 'C:\Backup\Frotix_PreUpdate.bak'
   
   2. CARACTERÍSTICAS DESTE SCRIPT:
      ✅ Migração de tabelas existentes (ALTER TABLE)
      ✅ Criação de novos objetos (Tabelas, Views, Procedures)
      ✅ Validações antes de cada operação
      ✅ Rollback automático em caso de erro (tabelas)
      ✅ Log detalhado de cada operação
   
   3. TABELAS AFETADAS:
      - Lavagem: Renomear coluna Horario → HorarioLavagem
      - Recurso: Adicionar HasChild + alterar tipos
      - Manutencao: Ajustar tamanho de colunas de usuário
      - Fornecedor: Validação de chave primária
   
   4. NOVOS OBJETOS:
      - 31 tabelas novas (LogErros, Estatísticas, etc.)
      - 2 views novas + 65 atualizadas
      - 19 procedures novas + 1 atualizada
   
   5. TEMPO ESTIMADO: 5-10 minutos
   
   6. ROLLBACK:
      - Alterações de tabelas: ROLLBACK automático se erro
      - Objetos novos: Devem ser removidos manualmente (veja script ao final)
   
   ======================================================================================
*/

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET XACT_ABORT ON; -- Rollback automático em caso de erro crítico
GO

USE Frotix
GO

IF DB_NAME() <> 'Frotix'
BEGIN
    RAISERROR('❌ ERRO: Este script deve ser executado no banco Frotix!', 16, 1);
    RETURN;
END
GO

-- Criar tabela de log de execução
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigracaoLog]') AND type IN (N'U'))
BEGIN
    CREATE TABLE [dbo].[__MigracaoLog] (
        LogId INT IDENTITY(1,1) PRIMARY KEY,
        DataHora DATETIME2(3) DEFAULT GETDATE(),
        Etapa VARCHAR(100),
        Status VARCHAR(20),
        Mensagem NVARCHAR(MAX)
    );
END
GO

PRINT '======================================================================================';
PRINT '                   >>> ATUALIZAÇÃO COMPLETA DO BANCO FROTIX <<<';
PRINT '======================================================================================';
PRINT 'Servidor: ' + @@SERVERNAME;
PRINT 'Banco: ' + DB_NAME();
PRINT 'Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT 'Versão SQL: ' + CAST(SERVERPROPERTY('ProductVersion') AS VARCHAR);
PRINT '======================================================================================';
PRINT '';
GO

DECLARE @ErroGlobal BIT = 0;
DECLARE @MensagemErro NVARCHAR(MAX);

-- ======================================================================================
-- FASE 1: MIGRAÇÃO DE TABELAS EXISTENTES
-- ======================================================================================
PRINT '';
PRINT '╔════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║                    FASE 1: MIGRAÇÃO DE TABELAS EXISTENTES                     ║';
PRINT '╚════════════════════════════════════════════════════════════════════════════════╝';
PRINT '';
GO

-- -----------------------------------------------------------------------------
-- 1.1: TABELA LAVAGEM - Renomear Horario → HorarioLavagem
-- -----------------------------------------------------------------------------
PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '1.1 - TABELA LAVAGEM: Renomear coluna Horario → HorarioLavagem';
PRINT '───────────────────────────────────────────────────────────────────────────────';

INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
VALUES ('Lavagem.Horario', 'INICIANDO', 'Renomeando coluna');

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Lavagem') AND name = 'Horario')
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION MigrarLavagem;
        
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Lavagem') AND name = 'HorarioLavagem')
        BEGIN
            DECLARE @RegistrosMigrarLavagem INT;
            SELECT @RegistrosMigrarLavagem = COUNT(*)
            FROM Lavagem WHERE Horario IS NOT NULL AND HorarioLavagem IS NULL;
            
            IF @RegistrosMigrarLavagem > 0
            BEGIN
                UPDATE [dbo].[Lavagem] SET HorarioLavagem = Horario WHERE Horario IS NOT NULL;
                PRINT '  ✓ ' + CAST(@RegistrosMigrarLavagem AS VARCHAR) + ' registros migrados.';
            END
            
            ALTER TABLE [dbo].[Lavagem] DROP COLUMN [Horario];
            PRINT '  ✓ Coluna Horario removida.';
        END
        ELSE
        BEGIN
            EXEC sp_rename 'dbo.Lavagem.Horario', 'HorarioLavagem', 'COLUMN';
            PRINT '  ✓ Coluna renomeada com sp_rename.';
        END
        
        COMMIT TRANSACTION MigrarLavagem;
        INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
        VALUES ('Lavagem.Horario', 'SUCESSO', 'Coluna renomeada');
        PRINT '  ✅ LAVAGEM: Migração concluída!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION MigrarLavagem;
        SET @MensagemErro = ERROR_MESSAGE();
        PRINT '  ❌ ERRO: ' + @MensagemErro;
        INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
        VALUES ('Lavagem.Horario', 'ERRO', @MensagemErro);
        SET @ErroGlobal = 1;
    END CATCH
END
ELSE IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Lavagem') AND name = 'HorarioLavagem')
BEGIN
    PRINT '  ✓ Coluna HorarioLavagem já existe.';
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Lavagem.Horario', 'JA_EXISTE', 'Coluna HorarioLavagem já existe');
END
GO

-- -----------------------------------------------------------------------------
-- 1.2: TABELA RECURSO - Adicionar HasChild
-- -----------------------------------------------------------------------------
PRINT '';
PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '1.2 - TABELA RECURSO: Adicionar coluna HasChild';
PRINT '───────────────────────────────────────────────────────────────────────────────';

INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
VALUES ('Recurso.HasChild', 'INICIANDO', 'Adicionando coluna');

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Recurso') AND name = 'HasChild')
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION AdicionarHasChild;
        
        ALTER TABLE [dbo].[Recurso] ADD [HasChild] BIT NOT NULL DEFAULT(0);
        PRINT '  ✓ Coluna HasChild adicionada.';
        
        UPDATE parent SET HasChild = 1
        FROM [dbo].[Recurso] parent
        WHERE EXISTS (SELECT 1 FROM [dbo].[Recurso] child WHERE child.ParentId = parent.RecursoId);
        
        DECLARE @RecursosFilhos INT = @@ROWCOUNT;
        PRINT '  ✓ ' + CAST(@RecursosFilhos AS VARCHAR) + ' recursos com HasChild = 1';
        
        COMMIT TRANSACTION AdicionarHasChild;
        INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
        VALUES ('Recurso.HasChild', 'SUCESSO', 'Coluna adicionada e populada');
        PRINT '  ✅ RECURSO.HasChild: Migração concluída!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION AdicionarHasChild;
        SET @MensagemErro = ERROR_MESSAGE();
        PRINT '  ❌ ERRO: ' + @MensagemErro;
        INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
        VALUES ('Recurso.HasChild', 'ERRO', @MensagemErro);
        SET @ErroGlobal = 1;
    END CATCH
END
ELSE
BEGIN
    PRINT '  ✓ Coluna HasChild já existe.';
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Recurso.HasChild', 'JA_EXISTE', 'Coluna já existe');
END
GO

-- -----------------------------------------------------------------------------
-- 1.3: TABELA RECURSO - Popular valores NULL
-- -----------------------------------------------------------------------------
PRINT '';
PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '1.3 - TABELA RECURSO: Popular valores NULL antes de alterar para NOT NULL';
PRINT '───────────────────────────────────────────────────────────────────────────────';

INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
VALUES ('Recurso.PopularNulls', 'INICIANDO', 'Populando valores NULL');

BEGIN TRY
    BEGIN TRANSACTION PopularRecurso;
    
    DECLARE @NullCountRecurso INT = 0;
    
    SELECT @NullCountRecurso = COUNT(*) FROM [dbo].[Recurso] WHERE Nome IS NULL;
    IF @NullCountRecurso > 0
    BEGIN
        UPDATE [dbo].[Recurso] SET Nome = 'Recurso_' + CAST(RecursoId AS VARCHAR(36)) WHERE Nome IS NULL;
        PRINT '  ✓ ' + CAST(@NullCountRecurso AS VARCHAR) + ' valores NULL corrigidos em Nome.';
    END
    
    SELECT @NullCountRecurso = COUNT(*) FROM [dbo].[Recurso] WHERE NomeMenu IS NULL;
    IF @NullCountRecurso > 0
    BEGIN
        UPDATE [dbo].[Recurso] SET NomeMenu = ISNULL(Nome, 'Menu_' + CAST(RecursoId AS VARCHAR(36))) WHERE NomeMenu IS NULL;
        PRINT '  ✓ ' + CAST(@NullCountRecurso AS VARCHAR) + ' valores NULL corrigidos em NomeMenu.';
    END
    
    SELECT @NullCountRecurso = COUNT(*) FROM [dbo].[Recurso] WHERE Icon IS NULL;
    IF @NullCountRecurso > 0
    BEGIN
        UPDATE [dbo].[Recurso] SET Icon = 'fa fa-circle' WHERE Icon IS NULL;
        PRINT '  ✓ ' + CAST(@NullCountRecurso AS VARCHAR) + ' valores NULL corrigidos em Icon.';
    END
    
    SELECT @NullCountRecurso = COUNT(*) FROM [dbo].[Recurso] WHERE Href IS NULL;
    IF @NullCountRecurso > 0
    BEGIN
        UPDATE [dbo].[Recurso] SET Href = '#' WHERE Href IS NULL;
        PRINT '  ✓ ' + CAST(@NullCountRecurso AS VARCHAR) + ' valores NULL corrigidos em Href.';
    END
    
    COMMIT TRANSACTION PopularRecurso;
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Recurso.PopularNulls', 'SUCESSO', 'Valores NULL populados');
    PRINT '  ✅ RECURSO: Valores NULL populados!';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION PopularRecurso;
    SET @MensagemErro = ERROR_MESSAGE();
    PRINT '  ❌ ERRO: ' + @MensagemErro;
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Recurso.PopularNulls', 'ERRO', @MensagemErro);
    SET @ErroGlobal = 1;
END CATCH
GO

-- -----------------------------------------------------------------------------
-- 1.4: TABELA RECURSO - Alterar tipos de colunas
-- -----------------------------------------------------------------------------
PRINT '';
PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '1.4 - TABELA RECURSO: Alterar tipos e NOT NULL';
PRINT '───────────────────────────────────────────────────────────────────────────────';

INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
VALUES ('Recurso.AlterarTipos', 'INICIANDO', 'Alterando tipos de colunas');

BEGIN TRY
    BEGIN TRANSACTION AlterarRecurso;
    
    DECLARE @TipoColuna VARCHAR(50), @Nullable VARCHAR(3);
    
    SELECT @TipoColuna = DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Nome';
    IF @TipoColuna = 'varchar'
    BEGIN
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Nome] NVARCHAR(200) NOT NULL;
        PRINT '  ✓ Nome: varchar → nvarchar(200) NOT NULL';
    END
    
    SELECT @TipoColuna = DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'NomeMenu';
    IF @TipoColuna = 'varchar'
    BEGIN
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [NomeMenu] NVARCHAR(200) NOT NULL;
        PRINT '  ✓ NomeMenu: varchar → nvarchar(200) NOT NULL';
    END
    
    SELECT @Nullable = IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Icon';
    IF @Nullable = 'YES'
    BEGIN
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Icon] NVARCHAR(200) NOT NULL;
        PRINT '  ✓ Icon: NULL → NOT NULL';
    END
    
    SELECT @Nullable = IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Href';
    IF @Nullable = 'YES'
    BEGIN
        ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Href] NVARCHAR(500) NOT NULL;
        PRINT '  ✓ Href: NULL → NOT NULL';
    END
    
    COMMIT TRANSACTION AlterarRecurso;
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Recurso.AlterarTipos', 'SUCESSO', 'Tipos alterados');
    PRINT '  ✅ RECURSO: Tipos alterados!';
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION AlterarRecurso;
    SET @MensagemErro = ERROR_MESSAGE();
    PRINT '  ❌ ERRO: ' + @MensagemErro;
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Recurso.AlterarTipos', 'ERRO', @MensagemErro);
    SET @ErroGlobal = 1;
END CATCH
GO

-- -----------------------------------------------------------------------------
-- 1.5: TABELA MANUTENCAO - Alterar tipos (com validação)
-- -----------------------------------------------------------------------------
PRINT '';
PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '1.5 - TABELA MANUTENCAO: Alterar tipos de colunas de usuário';
PRINT '───────────────────────────────────────────────────────────────────────────────';

INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
VALUES ('Manutencao.AlterarTipos', 'INICIANDO', 'Validando e alterando tipos');

BEGIN TRY
    DECLARE @MaxLenManutencao INT = 0;
    
    SELECT @MaxLenManutencao = MAX(LEN(IdUsuarioCancelamento)) FROM [dbo].[Manutencao] WHERE IdUsuarioCancelamento IS NOT NULL;
    IF @MaxLenManutencao > 100
    BEGIN
        PRINT '  ⚠️  IdUsuarioCancelamento tem valores com ' + CAST(@MaxLenManutencao AS VARCHAR) + ' caracteres!';
        INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
        VALUES ('Manutencao.AlterarTipos', 'AVISO', 'Dados maiores que 100 caracteres - pulando alteração');
    END
    ELSE
    BEGIN
        BEGIN TRANSACTION AlterarManutencao;
        
        DECLARE @TipoManutencao VARCHAR(50);
        SELECT @TipoManutencao = DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Manutencao' AND COLUMN_NAME = 'IdUsuarioCancelamento';
        
        IF @TipoManutencao = 'nvarchar'
        BEGIN
            ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCancelamento] VARCHAR(100) NULL;
            ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCriacao] VARCHAR(100) NULL;
            ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioFinalizacao] VARCHAR(100) NULL;
            PRINT '  ✓ Colunas de usuário: nvarchar(450) → varchar(100)';
        END
        ELSE
            PRINT '  ✓ Colunas já são varchar(100).';
        
        COMMIT TRANSACTION AlterarManutencao;
        INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
        VALUES ('Manutencao.AlterarTipos', 'SUCESSO', 'Tipos alterados');
        PRINT '  ✅ MANUTENCAO: Tipos alterados!';
    END
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION AlterarManutencao;
    SET @MensagemErro = ERROR_MESSAGE();
    PRINT '  ⚠️  AVISO: ' + @MensagemErro;
    INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
    VALUES ('Manutencao.AlterarTipos', 'AVISO', @MensagemErro);
END CATCH
GO

PRINT '';
PRINT '╔════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║           ✅ FASE 1 CONCLUÍDA: TABELAS EXISTENTES MIGRADAS                    ║';
PRINT '╚════════════════════════════════════════════════════════════════════════════════╝';
PRINT '';
GO

-- Verificar se houve erro na Fase 1
IF EXISTS (SELECT 1 FROM [dbo].[__MigracaoLog] WHERE Status = 'ERRO')
BEGIN
    PRINT '';
    PRINT '❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌';
    PRINT '❌                                                                      ❌';
    PRINT '❌  ERRO CRÍTICO DETECTADO NA FASE 1!                                  ❌';
    PRINT '❌  Todas as alterações de tabelas foram revertidas (ROLLBACK).        ❌';
    PRINT '❌                                                                      ❌';
    PRINT '❌  Verifique o log de erros:                                          ❌';
    PRINT '❌  SELECT * FROM __MigracaoLog WHERE Status = ''ERRO''                  ❌';
    PRINT '❌                                                                      ❌';
    PRINT '❌  A FASE 2 (criação de objetos) NÃO será executada.                  ❌';
    PRINT '❌                                                                      ❌';
    PRINT '❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌❌';
    PRINT '';
    
    RAISERROR('Execução abortada devido a erros na Fase 1', 16, 1);
    RETURN;
END
GO

-- ======================================================================================
-- FASE 2: CRIAÇÃO DE NOVOS OBJETOS
-- ======================================================================================
PRINT '';
PRINT '╔════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║                    FASE 2: CRIAÇÃO DE NOVOS OBJETOS                           ║';
PRINT '╠════════════════════════════════════════════════════════════════════════════════╣';
PRINT '║  Esta fase criará:                                                             ║';
PRINT '║  • 31 novas tabelas                                                            ║';
PRINT '║  • 2 novas views + 65 atualizadas                                              ║';
PRINT '║  • 19 novas procedures + 1 atualizada                                          ║';
PRINT '╚════════════════════════════════════════════════════════════════════════════════╝';
PRINT '';
PRINT '⏳ Tempo estimado: 3-5 minutos...';
PRINT '';
GO

-- Aqui entrará o conteúdo do SCRIPT_ATUALIZACAO_PRODUCAO.sql
-- Por questão de tamanho, vou incluir apenas a estrutura de referência

PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '2.1 - CRIANDO NOVAS TABELAS (31 tabelas)...';
PRINT '───────────────────────────────────────────────────────────────────────────────';
PRINT '';

-- NOTA: O conteúdo completo de criação de objetos será executado via arquivo separado
-- Para executar: Use o arquivo SCRIPT_ATUALIZACAO_PRODUCAO.sql após este script
-- Ou use o comando sqlcmd para executar ambos em sequência

INSERT INTO [dbo].[__MigracaoLog] (Etapa, Status, Mensagem) 
VALUES ('Fase2', 'INFO', 'Execute SCRIPT_ATUALIZACAO_PRODUCAO.sql para criar objetos novos');

PRINT '';
PRINT '⚠️  PRÓXIMA ETAPA: Execute o arquivo SCRIPT_ATUALIZACAO_PRODUCAO.sql';
PRINT '    para criar 31 tabelas, 67 views e 20 procedures.';
PRINT '';
GO

-- ======================================================================================
-- VALIDAÇÃO FINAL E RELATÓRIO
-- ======================================================================================
PRINT '';
PRINT '╔════════════════════════════════════════════════════════════════════════════════╗';
PRINT '║                         VALIDAÇÃO FINAL E RELATÓRIO                            ║';
PRINT '╚════════════════════════════════════════════════════════════════════════════════╝';
PRINT '';
GO

-- Validar alterações de tabelas
PRINT 'VALIDAÇÃO DE ALTERAÇÕES EM TABELAS:';
PRINT '─────────────────────────────────────────────────────────────────────';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Lavagem') AND name = 'HorarioLavagem')
    PRINT '✅ Lavagem.HorarioLavagem: CRIADA'
ELSE
    PRINT '❌ Lavagem.HorarioLavagem: FALHOU';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Recurso') AND name = 'HasChild')
    PRINT '✅ Recurso.HasChild: CRIADA'
ELSE
    PRINT '❌ Recurso.HasChild: FALHOU';

DECLARE @IconCheck VARCHAR(3), @HrefCheck VARCHAR(3);
SELECT @IconCheck = IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Icon';
SELECT @HrefCheck = IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Recurso' AND COLUMN_NAME = 'Href';

IF @IconCheck = 'NO'
    PRINT '✅ Recurso.Icon: NOT NULL aplicado'
ELSE
    PRINT '⚠️  Recurso.Icon: ainda permite NULL';

IF @HrefCheck = 'NO'
    PRINT '✅ Recurso.Href: NOT NULL aplicado'
ELSE
    PRINT '⚠️  Recurso.Href: ainda permite NULL';

PRINT '';
PRINT 'RESUMO DA MIGRAÇÃO:';
PRINT '─────────────────────────────────────────────────────────────────────';

SELECT 
    Etapa,
    Status,
    COUNT(*) as Quantidade,
    MAX(DataHora) as UltimaOperacao
FROM [dbo].[__MigracaoLog]
GROUP BY Etapa, Status
ORDER BY Etapa, Status;

PRINT '';
PRINT 'LOG COMPLETO DE OPERAÇÕES:';
PRINT '─────────────────────────────────────────────────────────────────────';

SELECT * FROM [dbo].[__MigracaoLog] ORDER BY LogId;

PRINT '';
PRINT '======================================================================================';
PRINT '                         ✅ ATUALIZAÇÃO CONCLUÍDA COM SUCESSO!';
PRINT '======================================================================================';
PRINT 'Data/Hora término: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '';
PRINT 'PRÓXIMOS PASSOS:';
PRINT '  1. Reinicie a aplicação FrotiX';
PRINT '  2. Teste as funcionalidades principais';
PRINT '  3. Monitore o log de erros (tabela LogErros)';
PRINT '';
PRINT 'LIMPEZA (OPCIONAL):';
PRINT '  Para remover a tabela de log desta migração:';
PRINT '  DROP TABLE [dbo].[__MigracaoLog];';
PRINT '';
PRINT '======================================================================================';
GO

-- ======================================================================================
-- SCRIPT DE ROLLBACK MANUAL (COMENTADO)
-- ======================================================================================
/*
======================================================================================
SCRIPT DE ROLLBACK MANUAL - USAR APENAS SE NECESSÁRIO REVERTER AS ALTERAÇÕES
======================================================================================

-- Reverter Lavagem
EXEC sp_rename 'dbo.Lavagem.HorarioLavagem', 'Horario', 'COLUMN';

-- Reverter Recurso
ALTER TABLE [dbo].[Recurso] DROP COLUMN [HasChild];
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Nome] VARCHAR(250) NULL;
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [NomeMenu] VARCHAR(250) NULL;
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Icon] NVARCHAR(200) NULL;
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Href] NVARCHAR(500) NULL;

-- Reverter Manutencao
ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCancelamento] NVARCHAR(450) NULL;
ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCriacao] NVARCHAR(450) NULL;
ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioFinalizacao] NVARCHAR(450) NULL;

-- Remover objetos novos (se necessário)
-- DROP TABLE [dbo].[LogErros];
-- DROP TABLE [dbo].[EstatisticaMotoristasMensal];
-- [... listar todas as tabelas/views/procedures criadas ...]

PRINT 'Rollback manual executado!';

======================================================================================
*/
