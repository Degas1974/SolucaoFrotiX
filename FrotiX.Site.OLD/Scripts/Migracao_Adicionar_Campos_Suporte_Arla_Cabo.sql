/* ****************************************************************************************
 * ⚡ SCRIPT: Migracao_Adicionar_Campos_Suporte_Arla_Cabo.sql
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Adicionar campos faltantes de documentos/itens à tabela Viagem
 *
 * 📋 DESCRIÇÃO    : Este script adiciona 6 novos campos booleanos (bit) à tabela Viagem:
 *                   - ArlaEntregue / ArlaDevolvido
 *                   - CaboEntregue / CaboDevolvido
 *                   - SuporteIntegro / SuporteDefeituoso
 *
 * 📅 DATA CRIAÇÃO : 12/02/2026
 *
 * 👤 AUTOR        : Claude Sonnet 4.5
 *
 * 🔄 VERSÃO       : 1.0
 *
 * ⚠️ IMPORTANTE   : Execute este script no banco de dados FrotiX
 *                   ANTES de fazer deploy da aplicação atualizada.
 **************************************************************************************** */

USE [FrotiX]
GO

BEGIN TRANSACTION

BEGIN TRY
    PRINT '==========================================================================='
    PRINT 'INÍCIO DA MIGRAÇÃO: Adicionando campos Arla, Cabo e Suporte'
    PRINT '==========================================================================='
    PRINT ''

    -- ═══════════════════════════════════════════════════════════════════════════
    -- 1. VERIFICAR SE AS COLUNAS JÁ EXISTEM (Proteção contra re-execução)
    -- ═══════════════════════════════════════════════════════════════════════════

    DECLARE @ArlaEntregueExists BIT = 0
    DECLARE @CaboEntregueExists BIT = 0
    DECLARE @SuporteIntegroExists BIT = 0

    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'ArlaEntregue')
        SET @ArlaEntregueExists = 1

    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'CaboEntregue')
        SET @CaboEntregueExists = 1

    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Viagem') AND name = 'SuporteIntegro')
        SET @SuporteIntegroExists = 1

    -- ═══════════════════════════════════════════════════════════════════════════
    -- 2. ADICIONAR CAMPOS: ARLA ENTREGUE E DEVOLVIDO
    -- ═══════════════════════════════════════════════════════════════════════════

    IF @ArlaEntregueExists = 0
    BEGIN
        PRINT 'Adicionando campo: ArlaEntregue...'
        ALTER TABLE dbo.Viagem
        ADD ArlaEntregue bit NULL DEFAULT (0)

        PRINT 'Adicionando campo: ArlaDevolvido...'
        ALTER TABLE dbo.Viagem
        ADD ArlaDevolvido bit NULL DEFAULT (0)

        PRINT '✓ Campos Arla adicionados com sucesso!'
        PRINT ''
    END
    ELSE
    BEGIN
        PRINT '⚠ Campos Arla já existem - pulando...'
        PRINT ''
    END

    -- ═══════════════════════════════════════════════════════════════════════════
    -- 3. ADICIONAR CAMPOS: CABO ENTREGUE E DEVOLVIDO
    -- ═══════════════════════════════════════════════════════════════════════════

    IF @CaboEntregueExists = 0
    BEGIN
        PRINT 'Adicionando campo: CaboEntregue...'
        ALTER TABLE dbo.Viagem
        ADD CaboEntregue bit NULL DEFAULT (0)

        PRINT 'Adicionando campo: CaboDevolvido...'
        ALTER TABLE dbo.Viagem
        ADD CaboDevolvido bit NULL DEFAULT (0)

        PRINT '✓ Campos Cabo adicionados com sucesso!'
        PRINT ''
    END
    ELSE
    BEGIN
        PRINT '⚠ Campos Cabo já existem - pulando...'
        PRINT ''
    END

    -- ═══════════════════════════════════════════════════════════════════════════
    -- 4. ADICIONAR CAMPOS: SUPORTE ÍNTEGRO E DEFEITUOSO
    -- ═══════════════════════════════════════════════════════════════════════════

    IF @SuporteIntegroExists = 0
    BEGIN
        PRINT 'Adicionando campo: SuporteIntegro...'
        ALTER TABLE dbo.Viagem
        ADD SuporteIntegro bit NULL DEFAULT (0)

        PRINT 'Adicionando campo: SuporteDefeituoso...'
        ALTER TABLE dbo.Viagem
        ADD SuporteDefeituoso bit NULL DEFAULT (0)

        PRINT '✓ Campos Suporte adicionados com sucesso!'
        PRINT ''
    END
    ELSE
    BEGIN
        PRINT '⚠ Campos Suporte já existem - pulando...'
        PRINT ''
    END

    -- ═══════════════════════════════════════════════════════════════════════════
    -- 5. COMMIT DA TRANSAÇÃO
    -- ═══════════════════════════════════════════════════════════════════════════

    COMMIT TRANSACTION

    PRINT ''
    PRINT '==========================================================================='
    PRINT '✓ MIGRAÇÃO CONCLUÍDA COM SUCESSO!'
    PRINT '==========================================================================='
    PRINT ''
    PRINT 'Campos adicionados à tabela Viagem:'
    PRINT '  - ArlaEntregue (bit, default 0)'
    PRINT '  - ArlaDevolvido (bit, default 0)'
    PRINT '  - CaboEntregue (bit, default 0)'
    PRINT '  - CaboDevolvido (bit, default 0)'
    PRINT '  - SuporteIntegro (bit, default 0)'
    PRINT '  - SuporteDefeituoso (bit, default 0)'
    PRINT ''
    PRINT 'A aplicação pode ser atualizada agora.'
    PRINT '==========================================================================='

END TRY
BEGIN CATCH
    -- ═══════════════════════════════════════════════════════════════════════════
    -- TRATAMENTO DE ERRO: ROLLBACK E MENSAGEM
    -- ═══════════════════════════════════════════════════════════════════════════

    ROLLBACK TRANSACTION

    PRINT ''
    PRINT '==========================================================================='
    PRINT '✗ ERRO NA MIGRAÇÃO!'
    PRINT '==========================================================================='
    PRINT 'Erro: ' + ERROR_MESSAGE()
    PRINT 'Linha: ' + CAST(ERROR_LINE() AS VARCHAR(10))
    PRINT 'Procedure: ' + ISNULL(ERROR_PROCEDURE(), 'N/A')
    PRINT ''
    PRINT 'A transação foi revertida (ROLLBACK).'
    PRINT 'Nenhuma alteração foi feita no banco de dados.'
    PRINT '==========================================================================='

    -- Retornar erro ao cliente
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
    DECLARE @ErrorState INT = ERROR_STATE()

    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
GO
