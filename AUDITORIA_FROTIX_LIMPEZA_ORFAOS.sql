-- ============================================================
-- AUDITORIA FROTIX - LIMPEZA DE ÓRFÃOS + TRIGGERS DE PREVENÇÃO
-- ============================================================
-- Data: 07/02/2026
-- Banco: Frotix (SQL Server 2022)
-- Pré-requisito: Executar AUDITORIA_FROTIX_SCRIPTS.sql ANTES
--
-- IMPORTANTE:
--   - FAZER BACKUP ANTES DE EXECUTAR
--   - Usuário padrão: 0687751d-ba4d-498b-bd81-3c726f7296d2
--   - Campos de usuário órfãos → redirecionados para o padrão
--   - Campos de entidade órfãos → SET NULL
--   - Após limpeza, FKs ativadas com WITH CHECK (trusted)
--   - Triggers impedem futuros órfãos
-- ============================================================

USE Frotix;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- ============================================================
-- VARIÁVEL GLOBAL: Usuário padrão para redirecionamento
-- ============================================================
DECLARE @UsuarioPadrao NVARCHAR(450) = N'0687751d-ba4d-498b-bd81-3c726f7296d2';

-- Validar que o usuário padrão existe
IF NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers WHERE Id = @UsuarioPadrao)
BEGIN
    RAISERROR('BLOQUEIO: Usuário padrão %s NÃO existe em AspNetUsers. Corrija antes de continuar.', 16, 1, @UsuarioPadrao);
    RETURN;
END

PRINT '================================================================';
PRINT 'LIMPEZA DE ÓRFÃOS - INÍCIO';
PRINT 'Usuário padrão: ' + @UsuarioPadrao;
PRINT 'Data: ' + CONVERT(VARCHAR(30), GETDATE(), 120);
PRINT '================================================================';
PRINT '';

DECLARE @count INT;

-- ==========================================================
-- PARTE 0 — DESABILITAR TRIGGER trg_Motorista_FillNulls_OnChange
-- A SP usp_PreencheNulos_Motorista seta GUIDs vazios 
-- (00000000-0000-0000-0000-000000000000) em UnidadeId, 
-- ContratoId, CondutorId — viola as novas FKs.
-- Desabilitamos a trigger ANTES da limpeza e depois a
-- substituímos por uma versão corrigida.
-- ==========================================================
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'trg_Motorista_FillNulls_OnChange')
BEGIN
    DISABLE TRIGGER dbo.trg_Motorista_FillNulls_OnChange ON dbo.Motorista;
    PRINT '[0.1] trg_Motorista_FillNulls_OnChange DESABILITADA temporariamente.';
END
PRINT '';

-- ==========================================================
-- PARTE 0B — DESABILITAR TRIGGERS DA VIAGEM
-- Evita loops durante atualizações de limpeza (minutos/custos)
-- ==========================================================
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'tr_Viagem_CalculaMinutos')
BEGIN
  DISABLE TRIGGER dbo.tr_Viagem_CalculaMinutos ON dbo.Viagem;
  PRINT '[0.2] tr_Viagem_CalculaMinutos DESABILITADA temporariamente.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'TR_Viagem_NormalizarMinutos')
BEGIN
  DISABLE TRIGGER dbo.TR_Viagem_NormalizarMinutos ON dbo.Viagem;
  PRINT '[0.3] TR_Viagem_NormalizarMinutos DESABILITADA temporariamente.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'tr_Viagem_CalculaCustos')
BEGIN
  DISABLE TRIGGER dbo.tr_Viagem_CalculaCustos ON dbo.Viagem;
  PRINT '[0.4] tr_Viagem_CalculaCustos DESABILITADA temporariamente.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'trg_Viagem_AtualizarEstatisticasMotoristas')
BEGIN
  DISABLE TRIGGER dbo.trg_Viagem_AtualizarEstatisticasMotoristas ON dbo.Viagem;
  PRINT '[0.5] trg_Viagem_AtualizarEstatisticasMotoristas DESABILITADA temporariamente.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'trg_Viagem_AtualizarEstatisticasVeiculo')
BEGIN
  DISABLE TRIGGER dbo.trg_Viagem_AtualizarEstatisticasVeiculo ON dbo.Viagem;
  PRINT '[0.6] trg_Viagem_AtualizarEstatisticasVeiculo DESABILITADA temporariamente.';
END
PRINT '';

BEGIN TRY
    BEGIN TRANSACTION LimpezaOrfaos;

    -- ==========================================================
    -- PARTE 0.5 — LIMPAR GUIDs VAZIOS NA Motorista
    -- A SP antiga setava 00000000-... que não existe nas tabelas
    -- referenciadas. Converter para NULL antes de ativar FKs.
    -- ==========================================================
    PRINT '--- PARTE 0.5: Limpando GUIDs vazios (00000000-...) ---';
    PRINT '';
    
    DECLARE @guidVazio UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';

    UPDATE dbo.Motorista SET UnidadeId = NULL
    WHERE UnidadeId = @guidVazio;
    PRINT '[0.5.1] Motorista.UnidadeId: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' GUIDs vazios → NULL.';

    UPDATE dbo.Motorista SET ContratoId = NULL
    WHERE ContratoId = @guidVazio;
    PRINT '[0.5.2] Motorista.ContratoId: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' GUIDs vazios → NULL.';

    UPDATE dbo.Motorista SET CondutorId = NULL
    WHERE CondutorId = @guidVazio;
    PRINT '[0.5.3] Motorista.CondutorId: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' GUIDs vazios → NULL.';

    PRINT '';

    -- ==========================================================
    -- PARTE 1 — ÓRFÃOS DE USUÁRIO → Redirecionar para padrão
    -- Regra: Se o campo tem dados mas o ID não existe em
    --        AspNetUsers, redireciona para o usuário padrão.
    -- ==========================================================
    PRINT '--- PARTE 1: Redirecionando órfãos de usuário ---';
    PRINT '';

    -- 1.0 Normalizar strings vazias → NULL (evita violação de FK)
    UPDATE dbo.Viagem
    SET UsuarioIdCriacao = NULL
    WHERE UsuarioIdCriacao IS NOT NULL
      AND LTRIM(RTRIM(UsuarioIdCriacao)) = '';
    SET @count = @@ROWCOUNT;
    PRINT '[1.0] Viagem.UsuarioIdCriacao: ' + CAST(@count AS VARCHAR) + ' vazios → NULL.';

    UPDATE dbo.Manutencao
    SET IdUsuarioCriacao = NULL
    WHERE IdUsuarioCriacao IS NOT NULL
      AND LTRIM(RTRIM(IdUsuarioCriacao)) = '';
    SET @count = @@ROWCOUNT;
    PRINT '[1.0] Manutencao.IdUsuarioCriacao: ' + CAST(@count AS VARCHAR) + ' vazios → NULL.';

    UPDATE dbo.Manutencao
    SET IdUsuarioFinalizacao = NULL
    WHERE IdUsuarioFinalizacao IS NOT NULL
      AND LTRIM(RTRIM(IdUsuarioFinalizacao)) = '';
    SET @count = @@ROWCOUNT;
    PRINT '[1.0] Manutencao.IdUsuarioFinalizacao: ' + CAST(@count AS VARCHAR) + ' vazios → NULL.';

    UPDATE dbo.Manutencao
    SET IdUsuarioCancelamento = NULL
    WHERE IdUsuarioCancelamento IS NOT NULL
      AND LTRIM(RTRIM(IdUsuarioCancelamento)) = '';
    SET @count = @@ROWCOUNT;
    PRINT '[1.0] Manutencao.IdUsuarioCancelamento: ' + CAST(@count AS VARCHAR) + ' vazios → NULL.';

    UPDATE dbo.AlertasFrotiX
    SET UsuarioCriadorId = NULL
    WHERE UsuarioCriadorId IS NOT NULL
      AND LTRIM(RTRIM(UsuarioCriadorId)) = '';
    SET @count = @@ROWCOUNT;
    PRINT '[1.0] AlertasFrotiX.UsuarioCriadorId: ' + CAST(@count AS VARCHAR) + ' vazios → NULL.';

    -- 1.1 Viagem.UsuarioIdCriacao
    UPDATE dbo.Viagem
    SET UsuarioIdCriacao = @UsuarioPadrao
    WHERE UsuarioIdCriacao IS NOT NULL
      AND UsuarioIdCriacao <> ''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = UsuarioIdCriacao);
    SET @count = @@ROWCOUNT;
    PRINT '[1.1] Viagem.UsuarioIdCriacao: ' + CAST(@count AS VARCHAR) + ' órfãos redirecionados.';

    -- 1.2 Manutencao.IdUsuarioCriacao
    UPDATE dbo.Manutencao
    SET IdUsuarioCriacao = @UsuarioPadrao
    WHERE IdUsuarioCriacao IS NOT NULL
      AND IdUsuarioCriacao <> ''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = IdUsuarioCriacao);
    SET @count = @@ROWCOUNT;
    PRINT '[1.2] Manutencao.IdUsuarioCriacao: ' + CAST(@count AS VARCHAR) + ' órfãos redirecionados.';

    -- 1.3 Manutencao.IdUsuarioFinalizacao
    UPDATE dbo.Manutencao
    SET IdUsuarioFinalizacao = @UsuarioPadrao
    WHERE IdUsuarioFinalizacao IS NOT NULL
      AND IdUsuarioFinalizacao <> ''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = IdUsuarioFinalizacao);
    SET @count = @@ROWCOUNT;
    PRINT '[1.3] Manutencao.IdUsuarioFinalizacao: ' + CAST(@count AS VARCHAR) + ' órfãos redirecionados.';

    -- 1.4 Manutencao.IdUsuarioCancelamento
    UPDATE dbo.Manutencao
    SET IdUsuarioCancelamento = @UsuarioPadrao
    WHERE IdUsuarioCancelamento IS NOT NULL
      AND IdUsuarioCancelamento <> ''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = IdUsuarioCancelamento);
    SET @count = @@ROWCOUNT;
    PRINT '[1.4] Manutencao.IdUsuarioCancelamento: ' + CAST(@count AS VARCHAR) + ' órfãos redirecionados.';

    -- 1.5 AlertasFrotiX.UsuarioCriadorId
    UPDATE dbo.AlertasFrotiX
    SET UsuarioCriadorId = @UsuarioPadrao
    WHERE UsuarioCriadorId IS NOT NULL
      AND UsuarioCriadorId <> ''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = UsuarioCriadorId);
    SET @count = @@ROWCOUNT;
    PRINT '[1.5] AlertasFrotiX.UsuarioCriadorId: ' + CAST(@count AS VARCHAR) + ' órfãos redirecionados.';

    PRINT '';

    -- ==========================================================
    -- PARTE 2 — ÓRFÃOS DE ENTIDADE → SET NULL
    -- Regra: Se a FK aponta para registro inexistente, limpa
    --        com NULL (campos opcionais por design).
    -- ==========================================================
    PRINT '--- PARTE 2: Limpando órfãos de entidade (SET NULL) ---';
    PRINT '';

    -- 2.1 Lavador.ContratoId
    UPDATE dbo.Lavador
    SET ContratoId = NULL
    WHERE ContratoId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = Lavador.ContratoId);
    SET @count = @@ROWCOUNT;
    PRINT '[2.1] Lavador.ContratoId: ' + CAST(@count AS VARCHAR) + ' órfãos → NULL.';

    -- 2.2 MovimentacaoEmpenhoMulta.EmpenhoMultaId
    UPDATE dbo.MovimentacaoEmpenhoMulta
    SET EmpenhoMultaId = NULL
    WHERE EmpenhoMultaId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.EmpenhoMulta e WHERE e.EmpenhoMultaId = MovimentacaoEmpenhoMulta.EmpenhoMultaId);
    SET @count = @@ROWCOUNT;
    PRINT '[2.2] MovimentacaoEmpenhoMulta.EmpenhoMultaId: ' + CAST(@count AS VARCHAR) + ' órfãos → NULL.';

    -- 2.3 Motorista.UnidadeId
    UPDATE dbo.Motorista
    SET UnidadeId = NULL
    WHERE UnidadeId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Unidade u WHERE u.UnidadeId = Motorista.UnidadeId);
    SET @count = @@ROWCOUNT;
    PRINT '[2.3] Motorista.UnidadeId: ' + CAST(@count AS VARCHAR) + ' órfãos → NULL.';

    -- 2.4 Motorista.ContratoId
    UPDATE dbo.Motorista
    SET ContratoId = NULL
    WHERE ContratoId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = Motorista.ContratoId);
    SET @count = @@ROWCOUNT;
    PRINT '[2.4] Motorista.ContratoId: ' + CAST(@count AS VARCHAR) + ' órfãos → NULL.';

    -- 2.5 Motorista.CondutorId
    UPDATE dbo.Motorista
    SET CondutorId = NULL
    WHERE CondutorId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.CondutorApoio c WHERE c.CondutorId = Motorista.CondutorId);
    SET @count = @@ROWCOUNT;
    PRINT '[2.5] Motorista.CondutorId: ' + CAST(@count AS VARCHAR) + ' órfãos → NULL.';

    -- 2.6 ItensManutencao.ViagemId
    UPDATE dbo.ItensManutencao
    SET ViagemId = NULL
    WHERE ViagemId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Viagem v WHERE v.ViagemId = ItensManutencao.ViagemId);
    SET @count = @@ROWCOUNT;
    PRINT '[2.6] ItensManutencao.ViagemId: ' + CAST(@count AS VARCHAR) + ' órfãos → NULL.';

    PRINT '';

    -- ==========================================================
    -- PARTE 3 — ATIVAR FKs COM CHECK (Trusted)
    -- Agora que não há mais órfãos, podemos ativar a validação
    -- completa em todas as FKs criadas com NOCHECK.
    -- ==========================================================
    PRINT '--- PARTE 3: Ativando FKs com WITH CHECK ---';
    PRINT '';

    -- FKs de Usuário
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Viagem_UsuarioIdCriacao' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Viagem WITH CHECK CHECK CONSTRAINT FK_Viagem_UsuarioIdCriacao;
        PRINT '[3.1] FK_Viagem_UsuarioIdCriacao → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCriacao' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Manutencao WITH CHECK CHECK CONSTRAINT FK_Manutencao_IdUsuarioCriacao;
        PRINT '[3.2] FK_Manutencao_IdUsuarioCriacao → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioFinalizacao' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Manutencao WITH CHECK CHECK CONSTRAINT FK_Manutencao_IdUsuarioFinalizacao;
        PRINT '[3.3] FK_Manutencao_IdUsuarioFinalizacao → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Manutencao_IdUsuarioCancelamento' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Manutencao WITH CHECK CHECK CONSTRAINT FK_Manutencao_IdUsuarioCancelamento;
        PRINT '[3.4] FK_Manutencao_IdUsuarioCancelamento → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_AlertasFrotiX_UsuarioCriadorId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.AlertasFrotiX WITH CHECK CHECK CONSTRAINT FK_AlertasFrotiX_UsuarioCriadorId;
        PRINT '[3.5] FK_AlertasFrotiX_UsuarioCriadorId → TRUSTED';
    END

    -- FKs de Entidade
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Lavador_ContratoId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Lavador WITH CHECK CHECK CONSTRAINT FK_Lavador_ContratoId;
        PRINT '[3.6] FK_Lavador_ContratoId → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.MovimentacaoEmpenhoMulta WITH CHECK CHECK CONSTRAINT FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId;
        PRINT '[3.7] FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Motorista_UnidadeId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Motorista WITH CHECK CHECK CONSTRAINT FK_Motorista_UnidadeId;
        PRINT '[3.8] FK_Motorista_UnidadeId → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Motorista_ContratoId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Motorista WITH CHECK CHECK CONSTRAINT FK_Motorista_ContratoId;
        PRINT '[3.9] FK_Motorista_ContratoId → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Motorista_CondutorId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.Motorista WITH CHECK CHECK CONSTRAINT FK_Motorista_CondutorId;
        PRINT '[3.10] FK_Motorista_CondutorId → TRUSTED';
    END

    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ItensManutencao_ViagemId' AND is_not_trusted = 1)
    BEGIN
        ALTER TABLE dbo.ItensManutencao WITH CHECK CHECK CONSTRAINT FK_ItensManutencao_ViagemId;
        PRINT '[3.11] FK_ItensManutencao_ViagemId → TRUSTED';
    END

    PRINT '';

    -- ==========================================================
    -- COMMIT
    -- ==========================================================
    COMMIT TRANSACTION LimpezaOrfaos;

    PRINT '================================================================';
    PRINT 'LIMPEZA CONCLUÍDA COM SUCESSO!';
    PRINT 'Fim: ' + CONVERT(VARCHAR(30), GETDATE(), 120);
    PRINT '================================================================';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION LimpezaOrfaos;

    PRINT '';
    PRINT '================================================================';
    PRINT 'ERRO! ROLLBACK — NENHUMA ALTERAÇÃO FOI MANTIDA';
    PRINT '================================================================';
    PRINT 'Mensagem: ' + ERROR_MESSAGE();
    PRINT 'Linha: ' + CAST(ERROR_LINE() AS VARCHAR);
    PRINT '================================================================';
    THROW;
END CATCH
GO

-- ============================================================
-- PARTE 3B — REABILITAR TRIGGERS DA VIAGEM
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'tr_Viagem_CalculaMinutos')
BEGIN
  ENABLE TRIGGER dbo.tr_Viagem_CalculaMinutos ON dbo.Viagem;
  PRINT '[3B.1] tr_Viagem_CalculaMinutos REABILITADA.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'TR_Viagem_NormalizarMinutos')
BEGIN
  ENABLE TRIGGER dbo.TR_Viagem_NormalizarMinutos ON dbo.Viagem;
  PRINT '[3B.2] TR_Viagem_NormalizarMinutos REABILITADA.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'tr_Viagem_CalculaCustos')
BEGIN
  ENABLE TRIGGER dbo.tr_Viagem_CalculaCustos ON dbo.Viagem;
  PRINT '[3B.3] tr_Viagem_CalculaCustos REABILITADA.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'trg_Viagem_AtualizarEstatisticasMotoristas')
BEGIN
  ENABLE TRIGGER dbo.trg_Viagem_AtualizarEstatisticasMotoristas ON dbo.Viagem;
  PRINT '[3B.4] trg_Viagem_AtualizarEstatisticasMotoristas REABILITADA.';
END
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'trg_Viagem_AtualizarEstatisticasVeiculo')
BEGIN
  ENABLE TRIGGER dbo.trg_Viagem_AtualizarEstatisticasVeiculo ON dbo.Viagem;
  PRINT '[3B.5] trg_Viagem_AtualizarEstatisticasVeiculo REABILITADA.';
END
PRINT '';

-- ============================================================
-- PARTE 4 — CORRIGIR SP + TRIGGERS DE PREVENÇÃO
-- Executados fora da transação (DDL não participa de TXN)
-- ============================================================

PRINT '';
PRINT '--- PARTE 4A: Corrigindo SP usp_PreencheNulos_Motorista ---';
PRINT '    (Remove GUIDs vazios → mantém NULL para FKs)';
PRINT '';

-- ----------------------------------------------------------
-- 4A.1 — Corrigir SP para NÃO setar GUIDs vazios
-- Os campos UnidadeId, ContratoId e CondutorId devem 
-- permanecer NULL quando não houver valor (FK exige)
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER PROCEDURE dbo.usp_PreencheNulos_Motorista
    @Keys dbo.MotoristaKeyList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE M
       SET
         -- strings
         CPF               = ISNULL(M.CPF,               ''''),
         CNH               = ISNULL(M.CNH,               ''''),
         CategoriaCNH      = ISNULL(M.CategoriaCNH,      ''''),
         Celular01         = ISNULL(M.Celular01,          ''''),
         Celular02         = ISNULL(M.Celular02,          ''''),
         OrigemIndicacao   = ISNULL(M.OrigemIndicacao,    ''''),
         UsuarioIdAlteracao= ISNULL(M.UsuarioIdAlteracao, ''''),
         TipoCondutor      = ISNULL(M.TipoCondutor,      ''''),
         EfetivoFerista    = ISNULL(M.EfetivoFerista,     ''''),

         -- datas
         DataNascimento    = ISNULL(M.DataNascimento,     CONVERT(date,''19000101'')),
         DataVencimentoCNH = ISNULL(M.DataVencimentoCNH,  CONVERT(date,''19000101'')),
         DataIngresso      = ISNULL(M.DataIngresso,       CONVERT(date,''19000101'')),
         DataAlteracao     = ISNULL(M.DataAlteracao,      GETDATE()),

         -- numéricos/bit
         Status            = ISNULL(M.Status, 0),
         CodMotoristaQCard = ISNULL(M.CodMotoristaQCard, 0)

         -- GUIDs de FK: NÃO preencher com 00000000-..., MANTER NULL
         -- UnidadeId, ContratoId, CondutorId permanecem como estão
    FROM dbo.Motorista AS M
    INNER JOIN @Keys      AS K ON K.MotoristaId = M.MotoristaId
    WHERE
         M.CPF                IS NULL OR
         M.CNH                IS NULL OR
         M.CategoriaCNH       IS NULL OR
         M.Celular01          IS NULL OR
         M.Celular02          IS NULL OR
         M.OrigemIndicacao    IS NULL OR
         M.UsuarioIdAlteracao IS NULL OR
         M.TipoCondutor       IS NULL OR
         M.EfetivoFerista     IS NULL OR

         M.DataNascimento     IS NULL OR
         M.DataVencimentoCNH  IS NULL OR
         M.DataIngresso       IS NULL OR
         M.DataAlteracao      IS NULL OR

         M.Status             IS NULL OR
         M.CodMotoristaQCard  IS NULL;
END;';

PRINT '[4A.1] OK - usp_PreencheNulos_Motorista corrigida (sem GUIDs vazios).';
PRINT '';

-- ----------------------------------------------------------
-- 4A.2 — Reabilitar trigger original (agora chama SP corrigida)
-- ----------------------------------------------------------
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'trg_Motorista_FillNulls_OnChange')
BEGIN
    ENABLE TRIGGER dbo.trg_Motorista_FillNulls_OnChange ON dbo.Motorista;
    PRINT '[4A.2] OK - trg_Motorista_FillNulls_OnChange REABILITADA (agora usa SP corrigida).';
END
PRINT '';

PRINT '--- PARTE 4B: Criando Triggers de Validação de FK ---';
PRINT '';

-- ----------------------------------------------------------
-- 4B.0 Corrigir TR_Viagem_NormalizarMinutos existente
--      (adiciona proteção anti-recursão que faltava)
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.TR_Viagem_NormalizarMinutos
ON dbo.Viagem
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    UPDATE v
    SET Minutos = 
        CASE 
            WHEN i.Status <> ''Realizada'' THEN 0
            WHEN i.Status = ''Realizada'' AND ISNULL(i.Minutos, 0) > 0 THEN i.Minutos
            WHEN i.Status = ''Realizada'' 
                 AND i.DataInicial IS NOT NULL 
                 AND i.DataFinal IS NOT NULL 
                 AND i.HoraInicio IS NOT NULL 
                 AND i.HoraFim IS NOT NULL
            THEN ABS(DATEDIFF(MINUTE, 
                    DATEADD(MINUTE, 
                        DATEPART(HOUR, i.HoraInicio) * 60 + DATEPART(MINUTE, i.HoraInicio),
                        CAST(CAST(i.DataInicial AS DATE) AS DATETIME)),
                    DATEADD(MINUTE, 
                        DATEPART(HOUR, i.HoraFim) * 60 + DATEPART(MINUTE, i.HoraFim),
                        CAST(CAST(i.DataFinal AS DATE) AS DATETIME))))
            ELSE 0
        END
    FROM dbo.Viagem v
    INNER JOIN inserted i ON v.ViagemId = i.ViagemId
    WHERE v.Minutos IS NULL 
       OR v.Minutos < 0
       OR (i.Status <> ''Realizada'' AND v.Minutos <> 0)
       OR (i.Status = ''Realizada'' AND v.Minutos = 0 
           AND i.DataInicial IS NOT NULL 
           AND i.DataFinal IS NOT NULL 
           AND i.HoraInicio IS NOT NULL 
           AND i.HoraFim IS NOT NULL);
END;';

PRINT '[4B.0] OK - TR_Viagem_NormalizarMinutos corrigida (anti-recursão adicionada).';

-- ----------------------------------------------------------
-- 4B.1 Trigger: Viagem — Protege UsuarioIdCriacao
--      Só dispara no INSERT ou quando UsuarioIdCriacao muda
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_Viagem_ValidaUsuarios
ON dbo.Viagem
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    DECLARE @UsuarioPadrao NVARCHAR(450) = N''0687751d-ba4d-498b-bd81-3c726f7296d2'';

    -- Normaliza vazio → NULL
    UPDATE v
    SET v.UsuarioIdCriacao = NULL
    FROM dbo.Viagem v
    INNER JOIN inserted i ON v.ViagemId = i.ViagemId
    WHERE v.UsuarioIdCriacao IS NOT NULL
      AND LTRIM(RTRIM(v.UsuarioIdCriacao)) = '''';

    UPDATE v
    SET v.UsuarioIdCriacao = @UsuarioPadrao
    FROM dbo.Viagem v
    INNER JOIN inserted i ON v.ViagemId = i.ViagemId
    WHERE v.UsuarioIdCriacao IS NOT NULL
      AND v.UsuarioIdCriacao <> ''''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = v.UsuarioIdCriacao);
END;';

PRINT '[4B.1] OK - trg_Viagem_ValidaUsuarios criada (com anti-recursão).';

-- ----------------------------------------------------------
-- 4B.2 Trigger: Manutencao — Protege 3 colunas de usuário
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_Manutencao_ValidaUsuarios
ON dbo.Manutencao
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    DECLARE @UsuarioPadrao NVARCHAR(450) = N''0687751d-ba4d-498b-bd81-3c726f7296d2'';

    -- Normaliza vazios → NULL
    UPDATE m
    SET m.IdUsuarioCriacao = NULL
    FROM dbo.Manutencao m
    INNER JOIN inserted i ON m.ManutencaoId = i.ManutencaoId
    WHERE m.IdUsuarioCriacao IS NOT NULL
      AND LTRIM(RTRIM(m.IdUsuarioCriacao)) = '''';

    UPDATE m
    SET m.IdUsuarioFinalizacao = NULL
    FROM dbo.Manutencao m
    INNER JOIN inserted i ON m.ManutencaoId = i.ManutencaoId
    WHERE m.IdUsuarioFinalizacao IS NOT NULL
      AND LTRIM(RTRIM(m.IdUsuarioFinalizacao)) = '''';

    UPDATE m
    SET m.IdUsuarioCancelamento = NULL
    FROM dbo.Manutencao m
    INNER JOIN inserted i ON m.ManutencaoId = i.ManutencaoId
    WHERE m.IdUsuarioCancelamento IS NOT NULL
      AND LTRIM(RTRIM(m.IdUsuarioCancelamento)) = '''';

    -- Corrige IdUsuarioCriacao inválido
    UPDATE m
    SET m.IdUsuarioCriacao = @UsuarioPadrao
    FROM dbo.Manutencao m
    INNER JOIN inserted i ON m.ManutencaoId = i.ManutencaoId
    WHERE m.IdUsuarioCriacao IS NOT NULL
      AND m.IdUsuarioCriacao <> ''''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioCriacao);

    -- Corrige IdUsuarioFinalizacao inválido
    UPDATE m
    SET m.IdUsuarioFinalizacao = @UsuarioPadrao
    FROM dbo.Manutencao m
    INNER JOIN inserted i ON m.ManutencaoId = i.ManutencaoId
    WHERE m.IdUsuarioFinalizacao IS NOT NULL
      AND m.IdUsuarioFinalizacao <> ''''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioFinalizacao);

    -- Corrige IdUsuarioCancelamento inválido
    UPDATE m
    SET m.IdUsuarioCancelamento = @UsuarioPadrao
    FROM dbo.Manutencao m
    INNER JOIN inserted i ON m.ManutencaoId = i.ManutencaoId
    WHERE m.IdUsuarioCancelamento IS NOT NULL
      AND m.IdUsuarioCancelamento <> ''''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioCancelamento);
END;';

PRINT '[4B.2] OK - trg_Manutencao_ValidaUsuarios criada.';

-- ----------------------------------------------------------
-- 4B.3 Trigger: AlertasFrotiX — Protege UsuarioCriadorId
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_AlertasFrotiX_ValidaUsuarios
ON dbo.AlertasFrotiX
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    DECLARE @UsuarioPadrao NVARCHAR(450) = N''0687751d-ba4d-498b-bd81-3c726f7296d2'';

  -- Normaliza vazio → NULL
  UPDATE a
  SET a.UsuarioCriadorId = NULL
  FROM dbo.AlertasFrotiX a
  INNER JOIN inserted i ON a.AlertasFrotiXId = i.AlertasFrotiXId
  WHERE a.UsuarioCriadorId IS NOT NULL
    AND LTRIM(RTRIM(a.UsuarioCriadorId)) = '''';

    -- Corrige UsuarioCriadorId inválido
    UPDATE a
    SET a.UsuarioCriadorId = @UsuarioPadrao
    FROM dbo.AlertasFrotiX a
    INNER JOIN inserted i ON a.AlertasFrotiXId = i.AlertasFrotiXId
    WHERE a.UsuarioCriadorId IS NOT NULL
      AND a.UsuarioCriadorId <> ''''
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = a.UsuarioCriadorId);
END;';

PRINT '[4B.3] OK - trg_AlertasFrotiX_ValidaUsuarios criada.';

-- ----------------------------------------------------------
-- 4B.4 Trigger: Motorista — Protege UnidadeId, ContratoId, CondutorId
-- Se a referência não existir, seta NULL (campo opcional)
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_Motorista_ValidaReferencias
ON dbo.Motorista
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    -- Corrige UnidadeId inválido → NULL
    UPDATE m
    SET m.UnidadeId = NULL
    FROM dbo.Motorista m
    INNER JOIN inserted i ON m.MotoristaId = i.MotoristaId
    WHERE m.UnidadeId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Unidade u WHERE u.UnidadeId = m.UnidadeId);

    -- Corrige ContratoId inválido → NULL
    UPDATE m
    SET m.ContratoId = NULL
    FROM dbo.Motorista m
    INNER JOIN inserted i ON m.MotoristaId = i.MotoristaId
    WHERE m.ContratoId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = m.ContratoId);

    -- Corrige CondutorId inválido → NULL
    UPDATE m
    SET m.CondutorId = NULL
    FROM dbo.Motorista m
    INNER JOIN inserted i ON m.MotoristaId = i.MotoristaId
    WHERE m.CondutorId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.CondutorApoio c WHERE c.CondutorId = m.CondutorId);
END;';

PRINT '[4B.4] OK - trg_Motorista_ValidaReferencias criada.';

-- ----------------------------------------------------------
-- 4B.5 Trigger: Lavador — Protege ContratoId
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_Lavador_ValidaReferencias
ON dbo.Lavador
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    -- Corrige ContratoId inválido → NULL
    UPDATE l
    SET l.ContratoId = NULL
    FROM dbo.Lavador l
    INNER JOIN inserted i ON l.LavadorId = i.LavadorId
    WHERE l.ContratoId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = l.ContratoId);
END;';

PRINT '[4B.5] OK - trg_Lavador_ValidaReferencias criada.';

-- ----------------------------------------------------------
-- 4B.6 Trigger: ItensManutencao — Protege ViagemId
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_ItensManutencao_ValidaReferencias
ON dbo.ItensManutencao
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    -- Corrige ViagemId inválido → NULL
    UPDATE im
    SET im.ViagemId = NULL
    FROM dbo.ItensManutencao im
    INNER JOIN inserted i ON im.ItemManutencaoId = i.ItemManutencaoId
    WHERE im.ViagemId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.Viagem v WHERE v.ViagemId = im.ViagemId);
END;';

PRINT '[4B.6] OK - trg_ItensManutencao_ValidaReferencias criada.';

-- ----------------------------------------------------------
-- 4B.7 Trigger: MovimentacaoEmpenhoMulta — Protege EmpenhoMultaId
-- ----------------------------------------------------------
EXEC sp_executesql N'
CREATE OR ALTER TRIGGER dbo.trg_MovimentacaoEmpenhoMulta_ValidaReferencias
ON dbo.MovimentacaoEmpenhoMulta
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Proteção anti-recursão: se já estamos dentro de outro trigger, sair
    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    -- Corrige EmpenhoMultaId inválido → NULL
    UPDATE mem
    SET mem.EmpenhoMultaId = NULL
    FROM dbo.MovimentacaoEmpenhoMulta mem
    INNER JOIN inserted i ON mem.MovimentacaoId = i.MovimentacaoId
    WHERE mem.EmpenhoMultaId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.EmpenhoMulta e WHERE e.EmpenhoMultaId = mem.EmpenhoMultaId);
END;';

PRINT '[4B.7] OK - trg_MovimentacaoEmpenhoMulta_ValidaReferencias criada.';

PRINT '';
PRINT '================================================================';
PRINT 'TODAS AS TRIGGERS CRIADAS COM SUCESSO!';
PRINT '================================================================';
PRINT '';
PRINT 'RESUMO:';
PRINT '  - SP usp_PreencheNulos_Motorista CORRIGIDA (não seta mais GUIDs vazios)';
PRINT '  - trg_Motorista_FillNulls_OnChange REABILITADA (usa SP corrigida)';
PRINT '  - 7 triggers de validação de FK criadas';
PRINT '  - Campos de usuário: redirecionam para 0687751d-ba4d-498b-bd81-3c726f7296d2';
PRINT '  - Campos de entidade: SET NULL se referência inválida';
PRINT '  - FKs agora são TRUSTED (WITH CHECK)';
PRINT '';
PRINT 'COMPORTAMENTO FUTURO:';
PRINT '  INSERT/UPDATE com ID de usuário inexistente → auto-corrige para padrão';
PRINT '  INSERT/UPDATE com ContratoId/UnidadeId inexistente → auto-corrige para NULL';
PRINT '  A FK continua existindo e validando APÓS o trigger corrigir';
GO

-- ============================================================
-- VERIFICAÇÃO FINAL: Confirmar que não restam órfãos
-- ============================================================
PRINT '';
PRINT '=== VERIFICAÇÃO FINAL: Registros órfãos restantes ===';
PRINT '';

SELECT 'Viagem.UsuarioIdCriacao' AS FK, COUNT(*) AS Orfaos
FROM dbo.Viagem v
WHERE v.UsuarioIdCriacao IS NOT NULL AND v.UsuarioIdCriacao <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = v.UsuarioIdCriacao);

SELECT 'Manutencao.IdUsuarioCriacao' AS FK, COUNT(*) AS Orfaos
FROM dbo.Manutencao m
WHERE m.IdUsuarioCriacao IS NOT NULL AND m.IdUsuarioCriacao <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioCriacao);

SELECT 'Manutencao.IdUsuarioFinalizacao' AS FK, COUNT(*) AS Orfaos
FROM dbo.Manutencao m
WHERE m.IdUsuarioFinalizacao IS NOT NULL AND m.IdUsuarioFinalizacao <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioFinalizacao);

SELECT 'Manutencao.IdUsuarioCancelamento' AS FK, COUNT(*) AS Orfaos
FROM dbo.Manutencao m
WHERE m.IdUsuarioCancelamento IS NOT NULL AND m.IdUsuarioCancelamento <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = m.IdUsuarioCancelamento);

SELECT 'AlertasFrotiX.UsuarioCriadorId' AS FK, COUNT(*) AS Orfaos
FROM dbo.AlertasFrotiX a
WHERE a.UsuarioCriadorId IS NOT NULL AND a.UsuarioCriadorId <> ''
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = a.UsuarioCriadorId);

SELECT 'Motorista.UnidadeId' AS FK, COUNT(*) AS Orfaos
FROM dbo.Motorista m
WHERE m.UnidadeId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Unidade u WHERE u.UnidadeId = m.UnidadeId);

SELECT 'Motorista.ContratoId' AS FK, COUNT(*) AS Orfaos
FROM dbo.Motorista m
WHERE m.ContratoId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = m.ContratoId);

SELECT 'Motorista.CondutorId' AS FK, COUNT(*) AS Orfaos
FROM dbo.Motorista m
WHERE m.CondutorId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.CondutorApoio c WHERE c.CondutorId = m.CondutorId);

SELECT 'Lavador.ContratoId' AS FK, COUNT(*) AS Orfaos
FROM dbo.Lavador l
WHERE l.ContratoId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Contrato c WHERE c.ContratoId = l.ContratoId);

SELECT 'MovimentacaoEmpenhoMulta.EmpenhoMultaId' AS FK, COUNT(*) AS Orfaos
FROM dbo.MovimentacaoEmpenhoMulta m
WHERE m.EmpenhoMultaId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.EmpenhoMulta e WHERE e.EmpenhoMultaId = m.EmpenhoMultaId);

SELECT 'ItensManutencao.ViagemId' AS FK, COUNT(*) AS Orfaos
FROM dbo.ItensManutencao i
WHERE i.ViagemId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Viagem v WHERE v.ViagemId = i.ViagemId);

PRINT '';
PRINT '=== Se todos Orfaos = 0, a limpeza foi bem-sucedida! ===';
GO

-- ============================================================
-- VERIFICAÇÃO: Status das FKs (trusted/untrusted)
-- ============================================================
PRINT '';
PRINT '=== STATUS DAS FKs (is_not_trusted = 0 significa TRUSTED) ===';

SELECT
    fk.name AS FK,
    OBJECT_NAME(fk.parent_object_id) AS Tabela,
    fk.is_not_trusted AS NaoConfiavel,
    CASE WHEN fk.is_not_trusted = 0 THEN 'TRUSTED' ELSE 'UNTRUSTED' END AS Status
FROM sys.foreign_keys fk
WHERE fk.name IN (
    'FK_Viagem_UsuarioIdCriacao',
    'FK_Lavador_ContratoId',
    'FK_MovimentacaoEmpenhoMulta_EmpenhoMultaId',
    'FK_AlertasFrotiX_UsuarioCriadorId',
    'FK_Motorista_UnidadeId', 'FK_Motorista_ContratoId', 'FK_Motorista_CondutorId',
    'FK_Manutencao_IdUsuarioCriacao', 'FK_Manutencao_IdUsuarioFinalizacao', 'FK_Manutencao_IdUsuarioCancelamento',
    'FK_ItensManutencao_ViagemId'
)
ORDER BY Tabela, fk.name;
GO
