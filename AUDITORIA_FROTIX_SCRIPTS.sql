-- ============================================================
-- AUDITORIA FROTIX - SCRIPT CONSOLIDADO DE EXECUÇÃO
-- ============================================================
-- Data da auditoria: 06/02/2026
-- Banco de dados: Frotix (SQL Server 2022)
-- Total de ações: 26 scripts em 6 fases
--
-- IMPORTANTE:
--   - FAZER BACKUP COMPLETO ANTES DE EXECUTAR
--   - Se QUALQUER instrução falhar, ALL changes são revertidas (ROLLBACK)
--   - Scripts 5.1/5.2 (limpeza) estão COMENTADOS por segurança
--   - ONLINE = ON removido pois não é compatível com transação única
--   - Após sucesso, execute WITH CHECK CHECK CONSTRAINT para ativar
--     validação completa das FKs (ver final do arquivo)
-- ============================================================

USE Frotix;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON; -- Garante rollback automático em qualquer erro
GO

BEGIN TRY
    BEGIN TRANSACTION AuditoriaFrotix;

    PRINT '================================================================';
    PRINT 'AUDITORIA FROTIX - EXECUÇÃO CONSOLIDADA';
    PRINT 'Início: ' + CONVERT(VARCHAR(30), GETDATE(), 120);
    PRINT '================================================================';
    PRINT '';

    -- ==========================================================
    -- FASE 1 — CORREÇÕES ESTRUTURAIS CRÍTICAS
    -- ==========================================================
    PRINT '--- FASE 1: Correções Estruturais Críticas ---';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 1.1: Corrigir Fornecedor - Adicionar PRIMARY KEY
    -- Converte FornecedorId de NULL + UNIQUE INDEX para
    -- NOT NULL + PRIMARY KEY
    -- ----------------------------------------------------------
    PRINT '[1.1] Verificando Fornecedor.FornecedorId...';

    -- Verificar se há registros com FornecedorId NULL
    IF EXISTS (SELECT 1 FROM dbo.Fornecedor WHERE FornecedorId IS NULL)
    BEGIN
        RAISERROR('[1.1] BLOQUEIO: Existem registros com FornecedorId NULL na tabela Fornecedor. Corrija antes de continuar.', 16, 1);
    END

    -- Remover o índice único existente
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'KEY_Fornecedor_FornecedorId' AND object_id = OBJECT_ID('dbo.Fornecedor'))
        DROP INDEX KEY_Fornecedor_FornecedorId ON dbo.Fornecedor;

    -- Alterar coluna para NOT NULL
    ALTER TABLE dbo.Fornecedor ALTER COLUMN FornecedorId uniqueidentifier NOT NULL;

    -- Adicionar PRIMARY KEY
    ALTER TABLE dbo.Fornecedor ADD CONSTRAINT PK_Fornecedor_FornecedorId PRIMARY KEY CLUSTERED (FornecedorId);

    PRINT '[1.1] OK - Fornecedor: PRIMARY KEY criada com sucesso.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 1.2: Remover FKs duplicadas (WhatsApp)
    -- Remove FKs anônimas duplicadas que causam overhead
    -- de validação dupla em INSERT/UPDATE
    -- ----------------------------------------------------------
    PRINT '[1.2] Removendo FKs duplicadas WhatsApp...';

    -- WhatsAppMensagens: remover FK anônima para InstanciaId
    -- (manter a nomeada FK_WhatsAppMensagens_InstanciaId)
    DECLARE @fkName NVARCHAR(256);
    DECLARE @sql NVARCHAR(MAX);

    SELECT @fkName = fk.name
    FROM sys.foreign_keys fk
    JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    WHERE fk.parent_object_id = OBJECT_ID('dbo.WhatsAppMensagens')
      AND COL_NAME(fkc.parent_object_id, fkc.parent_column_id) = 'InstanciaId'
      AND fk.name LIKE 'FK__WhatsApp%'  -- FK anônima (gerada pelo SQL Server)
      AND fk.name <> 'FK_WhatsAppMensagens_InstanciaId'; -- Não é a nomeada

    IF @fkName IS NOT NULL
    BEGIN
        SET @sql = 'ALTER TABLE dbo.WhatsAppMensagens DROP CONSTRAINT [' + @fkName + ']';
        EXEC sp_executesql @sql;
        PRINT '[1.2] OK - WhatsAppMensagens: FK anônima [' + @fkName + '] removida.';
    END
    ELSE
        PRINT '[1.2] INFO - WhatsAppMensagens: FK anônima para InstanciaId não encontrada (já removida?).';

    -- WhatsAppFilaMensagens: remover FK anônima para MensagemId
    -- (manter a nomeada FK_WhatsAppFilaMensagens_MensagemId)
    SET @fkName = NULL;
    SET @sql = NULL;

    SELECT @fkName = fk.name
    FROM sys.foreign_keys fk
    JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    WHERE fk.parent_object_id = OBJECT_ID('dbo.WhatsAppFilaMensagens')
      AND COL_NAME(fkc.parent_object_id, fkc.parent_column_id) = 'MensagemId'
      AND fk.name LIKE 'FK__WhatsApp%'
      AND fk.name <> 'FK_WhatsAppFilaMensagens_MensagemId';

    IF @fkName IS NOT NULL
    BEGIN
        SET @sql = 'ALTER TABLE dbo.WhatsAppFilaMensagens DROP CONSTRAINT [' + @fkName + ']';
        EXEC sp_executesql @sql;
        PRINT '[1.2] OK - WhatsAppFilaMensagens: FK anônima [' + @fkName + '] removida.';
    END
    ELSE
        PRINT '[1.2] INFO - WhatsAppFilaMensagens: FK anônima para MensagemId não encontrada (já removida?).';

    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 1.3: Remover FKs duplicadas em MotoristaItensPendentes
    -- Mantém as versões com sufixo _Id, remove as genéricas
    -- ----------------------------------------------------------
    PRINT '[1.3] Removendo FKs duplicadas MotoristaItensPendentes...';

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_MotoristaItensPendentes_Motorista')
        ALTER TABLE dbo.MotoristaItensPendentes DROP CONSTRAINT FK_MotoristaItensPendentes_Motorista;

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_MotoristaItensPendentes_Viagem')
        ALTER TABLE dbo.MotoristaItensPendentes DROP CONSTRAINT FK_MotoristaItensPendentes_Viagem;

    PRINT '[1.3] OK - MotoristaItensPendentes: FKs duplicadas removidas.';
    PRINT '';

    -- ==========================================================
    -- FASE 2 — FOREIGN KEYS FALTANTES
    -- ==========================================================
    PRINT '--- FASE 2: Foreign Keys Faltantes ---';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.1: FK Viagem.UsuarioIdCriacao → AspNetUsers
    -- Apenas UsuarioIdCriacao (as demais 5 colunas de usuário
    -- ficam com validação via código para não impactar performance)
    -- ----------------------------------------------------------
    PRINT '[2.1] Criando FK Viagem.UsuarioIdCriacao...';

    ALTER TABLE dbo.Viagem WITH NOCHECK
      ADD CONSTRAINT FK_Viagem_UsuarioIdCriacao
      FOREIGN KEY (UsuarioIdCriacao) REFERENCES dbo.AspNetUsers (Id);

    PRINT '[2.1] OK - Viagem: FK UsuarioIdCriacao criada.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.2: FK Lavador.ContratoId → Contrato
    -- Manter consistência com Operador e Encarregado
    -- ----------------------------------------------------------
    PRINT '[2.2] Criando FK Lavador.ContratoId...';

    ALTER TABLE dbo.Lavador WITH NOCHECK
      ADD CONSTRAINT FK_Lavador_ContratoId
      FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);

    PRINT '[2.2] OK - Lavador: FK ContratoId criada.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.3: FK MovimentacaoEmpenhoMulta.EmpenhoMultaId
    -- ----------------------------------------------------------
    PRINT '[2.3] Criando FK MovimentacaoEmpenhoMulta.EmpenhoMultaId...';

    ALTER TABLE dbo.MovimentacaoEmpenhoMulta WITH NOCHECK
      ADD CONSTRAINT FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId
      FOREIGN KEY (EmpenhoMultaId) REFERENCES dbo.EmpenhoMulta (EmpenhoMultaId);

    PRINT '[2.3] OK - MovimentacaoEmpenhoMulta: FK EmpenhoMultaId criada.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.4: FK AlertasFrotiX.UsuarioCriadorId → AspNetUsers
    -- ----------------------------------------------------------
    PRINT '[2.4] Criando FK AlertasFrotiX.UsuarioCriadorId...';

    ALTER TABLE dbo.AlertasFrotiX WITH NOCHECK
      ADD CONSTRAINT FK_AlertasFrotiX_UsuarioCriadorId
      FOREIGN KEY (UsuarioCriadorId) REFERENCES dbo.AspNetUsers (Id);

    PRINT '[2.4] OK - AlertasFrotiX: FK UsuarioCriadorId criada.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.5: FKs Motorista → Unidade, Contrato, CondutorApoio
    -- ----------------------------------------------------------
    PRINT '[2.5] Criando 3 FKs em Motorista...';

    -- 2.5a: Motorista.UnidadeId → Unidade
    ALTER TABLE dbo.Motorista WITH NOCHECK
      ADD CONSTRAINT FK_Motorista_UnidadeId
      FOREIGN KEY (UnidadeId) REFERENCES dbo.Unidade (UnidadeId);

    -- 2.5b: Motorista.ContratoId → Contrato
    ALTER TABLE dbo.Motorista WITH NOCHECK
      ADD CONSTRAINT FK_Motorista_ContratoId
      FOREIGN KEY (ContratoId) REFERENCES dbo.Contrato (ContratoId);

    -- 2.5c: Motorista.CondutorId → CondutorApoio
    ALTER TABLE dbo.Motorista WITH NOCHECK
      ADD CONSTRAINT FK_Motorista_CondutorId
      FOREIGN KEY (CondutorId) REFERENCES dbo.CondutorApoio (CondutorId);

    PRINT '[2.5] OK - Motorista: 3 FKs criadas (UnidadeId, ContratoId, CondutorId).';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.6: FKs Manutencao → AspNetUsers (3 colunas de usuário)
    -- Apenas IdUsuarioAlteracao já possui FK, as outras 3 não
    -- ----------------------------------------------------------
    PRINT '[2.6] Criando 3 FKs de usuário em Manutencao...';

    -- 2.6a: Manutencao.IdUsuarioCriacao
    ALTER TABLE dbo.Manutencao WITH NOCHECK
      ADD CONSTRAINT FK_Manutencao_IdUsuarioCriacao
      FOREIGN KEY (IdUsuarioCriacao) REFERENCES dbo.AspNetUsers (Id);

    -- 2.6b: Manutencao.IdUsuarioFinalizacao
    ALTER TABLE dbo.Manutencao WITH NOCHECK
      ADD CONSTRAINT FK_Manutencao_IdUsuarioFinalizacao
      FOREIGN KEY (IdUsuarioFinalizacao) REFERENCES dbo.AspNetUsers (Id);

    -- 2.6c: Manutencao.IdUsuarioCancelamento
    ALTER TABLE dbo.Manutencao WITH NOCHECK
      ADD CONSTRAINT FK_Manutencao_IdUsuarioCancelamento
      FOREIGN KEY (IdUsuarioCancelamento) REFERENCES dbo.AspNetUsers (Id);

    PRINT '[2.6] OK - Manutencao: 3 FKs de usuario criadas.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 2.7: FK ItensManutencao.ViagemId → Viagem
    -- ----------------------------------------------------------
    PRINT '[2.7] Criando FK ItensManutencao.ViagemId...';

    ALTER TABLE dbo.ItensManutencao WITH NOCHECK
      ADD CONSTRAINT FK_ItensManutencao_ViagemId
      FOREIGN KEY (ViagemId) REFERENCES dbo.Viagem (ViagemId);

    PRINT '[2.7] OK - ItensManutencao: FK ViagemId criada.';
    PRINT '';

    -- ==========================================================
    -- FASE 3 — NOVOS ÍNDICES
    -- (ONLINE = ON removido pois não é compatível com transação)
    -- ==========================================================
    PRINT '--- FASE 3: Novos Índices ---';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.1: Índice cobrindo para Abastecimento Dashboard
    -- Cobre DataHora + agrupamento VeiculoId/MotoristaId/Tipo
    -- ----------------------------------------------------------
    PRINT '[3.1] Criando índice Abastecimento Dashboard...';

    CREATE NONCLUSTERED INDEX IX_Abastecimento_DataHora_Cobertura
    ON dbo.Abastecimento (DataHora DESC)
    INCLUDE (VeiculoId, MotoristaId, CombustivelId, ValorTotal, LitrosAbastecidos, ValorUnitario, TipoCombustivel, Categoria)
    WITH (FILLFACTOR = 90)
    ON [PRIMARY];

    PRINT '[3.1] OK - Abastecimento: Índice de cobertura Dashboard criado.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.2: Índice Abastecimento MotoristaId + DataHora
    -- Para queries do DashboardMotoristas
    -- ----------------------------------------------------------
    PRINT '[3.2] Criando índice Abastecimento MotoristaId + DataHora...';

    CREATE NONCLUSTERED INDEX IX_Abastecimento_MotoristaId_DataHora
    ON dbo.Abastecimento (MotoristaId, DataHora DESC)
    INCLUDE (ValorTotal, LitrosAbastecidos, CombustivelId)
    WHERE (MotoristaId IS NOT NULL)
    WITH (FILLFACTOR = 90)
    ON [PRIMARY];

    PRINT '[3.2] OK - Abastecimento: Índice MotoristaId + DataHora criado.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.3: Índice CorridasTaxiLeg.DataAgenda
    -- Tabela sem NENHUM índice além da PK
    -- ----------------------------------------------------------
    PRINT '[3.3] Criando índice CorridasTaxiLeg...';

    CREATE NONCLUSTERED INDEX IX_CorridasTaxiLeg_DataAgenda
    ON dbo.CorridasTaxiLeg (DataAgenda DESC)
    INCLUDE (CorridaId, Setor, Unidade, Valor, KmReal, QtdPassageiros)
    WITH (FILLFACTOR = 90)
    ON [PRIMARY];

    PRINT '[3.3] OK - CorridasTaxiLeg: Índice DataAgenda criado.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.4: Índice CorridasCanceladasTaxiLeg.DataAgenda
    -- Idem — zero índices além da PK
    -- ----------------------------------------------------------
    PRINT '[3.4] Criando índice CorridasCanceladasTaxiLeg...';

    CREATE NONCLUSTERED INDEX IX_CorridasCanceladasTaxiLeg_DataAgenda
    ON dbo.CorridasCanceladasTaxiLeg (DataAgenda DESC)
    INCLUDE (TipoCancelamento, MotivoCancelamento, TempoEspera)
    WITH (FILLFACTOR = 90)
    ON [PRIMARY];

    PRINT '[3.4] OK - CorridasCanceladasTaxiLeg: Índice DataAgenda criado.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.5: Índices MovimentacaoEmpenhoMulta
    -- Tabela sem índices — JOINs com Multa e EmpenhoMulta
    -- ----------------------------------------------------------
    PRINT '[3.5] Criando 2 índices MovimentacaoEmpenhoMulta...';

    CREATE NONCLUSTERED INDEX IX_MovimentacaoEmpenhoMulta_MultaId
    ON dbo.MovimentacaoEmpenhoMulta (MultaId)
    INCLUDE (EmpenhoMultaId, DataMovimentacao, Valor)
    ON [PRIMARY];

    CREATE NONCLUSTERED INDEX IX_MovimentacaoEmpenhoMulta_EmpenhoMultaId
    ON dbo.MovimentacaoEmpenhoMulta (EmpenhoMultaId)
    INCLUDE (MultaId, DataMovimentacao, Valor)
    ON [PRIMARY];

    PRINT '[3.5] OK - MovimentacaoEmpenhoMulta: 2 índices criados.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.6: Índices NotaFiscal (EmpenhoId e ContratoId)
    -- ViewEmpenhos faz LEFT JOIN sem índice dedicado
    -- ----------------------------------------------------------
    PRINT '[3.6] Criando 2 índices NotaFiscal...';

    CREATE NONCLUSTERED INDEX IX_NotaFiscal_EmpenhoId
    ON dbo.NotaFiscal (EmpenhoId)
    INCLUDE (NotaFiscalId, ValorNF, ValorGlosa, DataEmissao, ContratoId)
    WHERE (EmpenhoId IS NOT NULL)
    WITH (FILLFACTOR = 90)
    ON [PRIMARY];

    CREATE NONCLUSTERED INDEX IX_NotaFiscal_ContratoId_AnoMes
    ON dbo.NotaFiscal (ContratoId, AnoReferencia DESC, MesReferencia DESC)
    INCLUDE (NotaFiscalId, NumeroNF, ValorNF, EmpenhoId)
    WHERE (ContratoId IS NOT NULL)
    WITH (FILLFACTOR = 90)
    ON [PRIMARY];

    PRINT '[3.6] OK - NotaFiscal: 2 índices criados.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.7: Índice DocumentoContrato.ContratoId
    -- ----------------------------------------------------------
    PRINT '[3.7] Criando índice DocumentoContrato...';

    CREATE NONCLUSTERED INDEX IX_DocumentoContrato_ContratoId
    ON dbo.DocumentoContrato (ContratoId)
    INCLUDE (DocumentoContratoId, TipoDocumento, Descricao)
    ON [PRIMARY];

    PRINT '[3.7] OK - DocumentoContrato: Índice ContratoId criado.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 3.8: Índice Contatos.Nome (busca textual WhatsApp)
    -- ----------------------------------------------------------
    PRINT '[3.8] Criando índice Contatos.Nome...';

    CREATE NONCLUSTERED INDEX IX_Contatos_Nome
    ON dbo.Contatos (Nome)
    INCLUDE (Celular, Email, Ativo)
    WHERE (Ativo = 1)
    ON [PRIMARY];

    PRINT '[3.8] OK - Contatos: Índice Nome criado.';
    PRINT '';

    -- ==========================================================
    -- FASE 4 — REMOÇÃO DE ÍNDICES REDUNDANTES/SOBREPOSTOS
    -- ==========================================================
    PRINT '--- FASE 4: Remoção de Índices Redundantes ---';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 4.1: Remover índice duplicado em Viagem.EventoId
    -- IX_Viagem_EventoId_Custos e IX_Viagem_EventoId_Include_Custos
    -- são idênticos — mesmo chave, mesmos INCLUDEs, mesmo filtro
    -- ----------------------------------------------------------
    PRINT '[4.1] Removendo índice duplicado EventoId...';

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_EventoId_Include_Custos' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IX_Viagem_EventoId_Include_Custos ON dbo.Viagem;

    PRINT '[4.1] OK - Viagem: Índice duplicado EventoId removido.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 4.2: Remover índice duplicado em NoFichaVistoria
    -- IDX_Ficha e IDX_NoFichaVistoria indexam a mesma coluna
    -- ----------------------------------------------------------
    PRINT '[4.2] Removendo índice duplicado NoFichaVistoria...';

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_Ficha' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IDX_Ficha ON dbo.Viagem;

    PRINT '[4.2] OK - Viagem: IDX_Ficha removido (mantido IDX_NoFichaVistoria).';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 4.3: Remover índices redundantes em MotoristaId
    -- IDX_MotoristaId e IX_Viagem_MotoristaId_DataInicial são
    -- cobertos por IX_Viagem_MotoristaId
    -- ----------------------------------------------------------
    PRINT '[4.3] Removendo 2 índices redundantes MotoristaId...';

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_MotoristaId' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IDX_MotoristaId ON dbo.Viagem;

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Viagem_MotoristaId_DataInicial' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IX_Viagem_MotoristaId_DataInicial ON dbo.Viagem;

    PRINT '[4.3] OK - Viagem: 2 índices redundantes MotoristaId removidos.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 4.4: Remover índice redundante em VeiculoId
    -- IDX_VeiculoId é coberto por IX_Viagem_VeiculoId
    -- ----------------------------------------------------------
    PRINT '[4.4] Removendo índice redundante VeiculoId...';

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_VeiculoId' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IDX_VeiculoId ON dbo.Viagem;

    PRINT '[4.4] OK - Viagem: IDX_VeiculoId removido.';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 4.5: Remover índices simples SetorId e RequisitanteId
    -- Cobertos pelos compostos IX_Viagem_SetorSolicitanteId
    -- e IX_Viagem_RequisitanteId
    -- ----------------------------------------------------------
    PRINT '[4.5] Removendo índices simples SetorId e RequisitanteId...';

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_SetorId' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IDX_SetorId ON dbo.Viagem;

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IDX_Requistante' AND object_id = OBJECT_ID('dbo.Viagem'))
        DROP INDEX IDX_Requistante ON dbo.Viagem;

    PRINT '[4.5] OK - Viagem: Índices simples SetorId e RequisitanteId removidos.';
    PRINT '';

    -- ==========================================================
    -- FASE 5 — STORED PROCEDURE
    -- ==========================================================
    PRINT '--- FASE 5: Stored Procedure ---';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 6.1: SP para recálculo de estatísticas de multas
    -- CREATE OR ALTER PROCEDURE precisa de EXEC pois deve ser
    -- a primeira instrução em um batch
    -- ----------------------------------------------------------
    PRINT '[6.1] Criando SP sp_RecalcularEstatisticasMultasMotoristas...';

    EXEC sp_executesql N'
    CREATE OR ALTER PROCEDURE dbo.sp_RecalcularEstatisticasMultasMotoristas
        @Ano INT = NULL,
        @Mes INT = NULL
    AS
    BEGIN
        SET NOCOUNT ON;

        IF @Ano IS NULL SET @Ano = YEAR(GETDATE());
        IF @Mes IS NULL SET @Mes = MONTH(GETDATE());

        BEGIN TRY
            BEGIN TRANSACTION;

            UPDATE em
            SET
                em.TotalMultas = ISNULL(m.TotalMultas, 0),
                em.ValorTotalMultas = ISNULL(m.ValorTotal, 0),
                em.DataAtualizacao = GETDATE()
            FROM dbo.EstatisticaMotoristasMensal em
            LEFT JOIN (
                SELECT
                    MotoristaId,
                    YEAR(Data) AS Ano,
                    MONTH(Data) AS Mes,
                    COUNT(*) AS TotalMultas,
                    SUM(ISNULL(ValorAteVencimento, 0)) AS ValorTotal
                FROM dbo.Multa
                WHERE MotoristaId IS NOT NULL
                  AND YEAR(Data) = @Ano AND MONTH(Data) = @Mes
                GROUP BY MotoristaId, YEAR(Data), MONTH(Data)
            ) m ON em.MotoristaId = m.MotoristaId
               AND em.Ano = m.Ano AND em.Mes = m.Mes
            WHERE em.Ano = @Ano AND em.Mes = @Mes;

            COMMIT TRANSACTION;
            PRINT ''Estatísticas de multas recalculadas para '' + CAST(@Ano AS VARCHAR) + ''/'' + CAST(@Mes AS VARCHAR);
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            THROW;
        END CATCH
    END;';

    PRINT '[6.1] OK - SP sp_RecalcularEstatisticasMultasMotoristas criada.';
    PRINT '';

    -- ==========================================================
    -- FASE 6 — LIMPEZA (COMENTADO POR SEGURANÇA)
    -- Descomentar SOMENTE após verificação manual
    -- ==========================================================
    PRINT '--- FASE 6: Limpeza (comentado) ---';
    PRINT '';

    -- ----------------------------------------------------------
    -- SCRIPT 5.1: Remover tabelas de backup
    -- DESCOMENTE após verificar que não contêm dados úteis:
    --   SELECT * FROM dbo.Recurso_BACKUP;
    --   SELECT * FROM dbo.ControleAcesso_BACKUP;
    -- ----------------------------------------------------------
    -- DROP TABLE dbo.Recurso_BACKUP;
    -- DROP TABLE dbo.ControleAcesso_BACKUP;
    PRINT '[5.1] IGNORADO - Tabelas backup (comentado por segurança).';

    -- ----------------------------------------------------------
    -- SCRIPT 5.2: Remover views obsoletas
    -- DESCOMENTE após verificar que nenhum código referencia:
    --   "ViewMotoristas_original", "ViewCalculaMediana"
    -- ----------------------------------------------------------
    -- DROP VIEW dbo.ViewMotoristas_original;
    -- DROP VIEW dbo.[ViewCalculaMediana (backup)];
    PRINT '[5.2] IGNORADO - Views obsoletas (comentado por segurança).';
    PRINT '';

    -- ==========================================================
    -- COMMIT — Todas as alterações aplicadas com sucesso
    -- ==========================================================
    COMMIT TRANSACTION AuditoriaFrotix;

    PRINT '================================================================';
    PRINT 'SUCESSO! Todas as alterações foram aplicadas.';
    PRINT 'Fim: ' + CONVERT(VARCHAR(30), GETDATE(), 120);
    PRINT '================================================================';
    PRINT '';
    PRINT 'PRÓXIMOS PASSOS:';
    PRINT '1. Verificar integridade: DBCC CHECKDB(Frotix) WITH NO_INFOMSGS';
    PRINT '2. Atualizar estatísticas: EXEC sp_updatestats';
    PRINT '3. Para ativar validação completa das FKs (opcional):';
    PRINT '   Execute o bloco WITH CHECK CHECK abaixo separadamente.';

END TRY
BEGIN CATCH
    -- ROLLBACK TOTAL — Nenhuma alteração é mantida
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION AuditoriaFrotix;

    PRINT '';
    PRINT '================================================================';
    PRINT 'ERRO! ROLLBACK COMPLETO REALIZADO — NENHUMA ALTERAÇÃO FOI MANTIDA';
    PRINT '================================================================';
    PRINT 'Mensagem: ' + ERROR_MESSAGE();
    PRINT 'Severidade: ' + CAST(ERROR_SEVERITY() AS VARCHAR);
    PRINT 'Estado: ' + CAST(ERROR_STATE() AS VARCHAR);
    PRINT 'Linha: ' + CAST(ERROR_LINE() AS VARCHAR);
    PRINT 'Procedimento: ' + ISNULL(ERROR_PROCEDURE(), 'Script principal');
    PRINT '================================================================';

    -- Re-lançar o erro para que o SSMS/aplicação também capture
    THROW;
END CATCH
GO

-- ============================================================
-- BLOCO OPCIONAL: Ativar validação completa das FKs
-- Executar SEPARADAMENTE após confirmar que os dados estão OK
-- Isso faz o SQL Server validar TODOS os registros existentes
-- contra as novas FKs (pode demorar dependendo do volume)
-- ============================================================
/*
PRINT 'Ativando validação completa das FKs...';

ALTER TABLE dbo.Viagem WITH CHECK CHECK CONSTRAINT FK_Viagem_UsuarioIdCriacao;
ALTER TABLE dbo.Lavador WITH CHECK CHECK CONSTRAINT FK_Lavador_ContratoId;
ALTER TABLE dbo.MovimentacaoEmpenhoMulta WITH CHECK CHECK CONSTRAINT FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId;
ALTER TABLE dbo.AlertasFrotiX WITH CHECK CHECK CONSTRAINT FK_AlertasFrotiX_UsuarioCriadorId;
ALTER TABLE dbo.Motorista WITH CHECK CHECK CONSTRAINT FK_Motorista_UnidadeId;
ALTER TABLE dbo.Motorista WITH CHECK CHECK CONSTRAINT FK_Motorista_ContratoId;
ALTER TABLE dbo.Motorista WITH CHECK CHECK CONSTRAINT FK_Motorista_CondutorId;
ALTER TABLE dbo.Manutencao WITH CHECK CHECK CONSTRAINT FK_Manutencao_IdUsuarioCriacao;
ALTER TABLE dbo.Manutencao WITH CHECK CHECK CONSTRAINT FK_Manutencao_IdUsuarioFinalizacao;
ALTER TABLE dbo.Manutencao WITH CHECK CHECK CONSTRAINT FK_Manutencao_IdUsuarioCancelamento;
ALTER TABLE dbo.ItensManutencao WITH CHECK CHECK CONSTRAINT FK_ItensManutencao_ViagemId;

PRINT 'Todas as FKs agora validam dados existentes (trusted).';
*/
