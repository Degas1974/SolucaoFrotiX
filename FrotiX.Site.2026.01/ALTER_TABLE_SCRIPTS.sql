/* ======================================================================================
   SCRIPTS ALTER TABLE - ATUALIZACAO DE TABELAS MODIFICADAS
   ======================================================================================
   
   DATA GERACAO: 09/02/2026 08:40:00
   
   IMPORTANTE:
   - Revise TODOS os scripts antes de executar
   - Scripts comentados (--) requerem ATENCAO ESPECIAL
   - Faca BACKUP antes de alterar estrutura de tabelas com dados
   
   ======================================================================================
*/

USE Frotix
GO

-- ========================================
-- TABELA: Manutencao
-- ========================================

PRINT 'Processando: Manutencao.IdUsuarioCancelamento...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Manutencao] ALTER COLUMN IdUsuarioCancelamento varchar(100) NULL;
GO

PRINT 'Processando: Manutencao.IdUsuarioCriacao...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Manutencao] ALTER COLUMN IdUsuarioCriacao varchar(100) NULL;
GO

PRINT 'Processando: Manutencao.IdUsuarioFinalizacao...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Manutencao] ALTER COLUMN IdUsuarioFinalizacao varchar(100) NULL;
GO


-- ========================================
-- TABELA: Recurso
-- ========================================

PRINT 'Processando: Recurso.HasChild...';
GO

ALTER TABLE [dbo].[Recurso] ADD HasChild bit NOT NULL;
GO

PRINT 'Processando: Recurso.RecursoId...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN RecursoId uniqueidentifier NOT NULL;
GO

PRINT 'Processando: Recurso.Nome...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN Nome nvarchar(200) NOT NULL;
GO

PRINT 'Processando: Recurso.NomeMenu...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN NomeMenu nvarchar(200) NOT NULL;
GO

PRINT 'Processando: Recurso.Icon...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN Icon nvarchar(200) NOT NULL;
GO

PRINT 'Processando: Recurso.Href...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN Href nvarchar(500) NOT NULL;
GO

PRINT 'Processando: Recurso.Ativo...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN Ativo bit NOT NULL;
GO

PRINT 'Processando: Recurso.Nivel...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Recurso] ALTER COLUMN Nivel int NOT NULL;
GO


-- ========================================
-- TABELA: Lavagem
-- ========================================

PRINT 'Processando: Lavagem.HorarioLavagem...';
GO

ALTER TABLE [dbo].[Lavagem] ADD HorarioLavagem datetime NULL;
GO

PRINT 'Processando: Lavagem.Horario...';
GO

-- ATENCAO: Remover coluna com dados! Verificar antes de executar
-- ALTER TABLE [dbo].[Lavagem] DROP COLUMN [Horario];
GO


-- ========================================
-- TABELA: Fornecedor
-- ========================================

PRINT 'Processando: Fornecedor.FornecedorId...';
GO

-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade
-- ALTER TABLE [dbo].[Fornecedor] ALTER COLUMN FornecedorId uniqueidentifier NULL DEFAULT (newid());
GO


