# =================================================================================
# CONSTRUTOR DE SCRIPT INTEGRADO FINAL
# =================================================================================

$ErrorActionPreference = 'Stop'

Write-Host "`n====================================================================================" -ForegroundColor Cyan
Write-Host "  CONSTRUTOR DE SCRIPT SQL INTEGRADO FINAL COM ROLLBACK" -ForegroundColor Cyan
Write-Host "====================================================================================`n" -ForegroundColor Cyan

$fase1File = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\MIGRACAO_SEGURA_TABELAS.sql"
$fase2File = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\SCRIPT_ATUALIZACAO_PRODUCAO.sql"
$outputFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\ATUALIZACAO_COMPLETA_COM_ROLLBACK.sql"

Write-Host "Lendo arquivos..." -ForegroundColor Yellow

# Ler Fase 1 (migração de tabelas)
$fase1Content = Get-Content $fase1File -Raw -Encoding UTF8

# Ler Fase 2 (criação de objetos)
$fase2Content = Get-Content $fase2File -Raw -Encoding UTF8

# Extrair apenas as seções relevantes de Fase 2 (remover cabeçalho duplicado)
$fase2StartIndex = $fase2Content.IndexOf("-- ======================================================================================")
$fase2StartIndex2 = $fase2Content.IndexOf("-- SE", $fase2StartIndex + 1)
if ($fase2StartIndex2 -gt 0) {
    $fase2Body = $fase2Content.Substring($fase2StartIndex2)
} else {
    $fase2Body = $fase2Content
}

# Construir script integrado
$scriptFinal = @"
/* ======================================================================================
   SCRIPT INTEGRADO COM ROLLBACK - ATUALIZAÇÃO COMPLETA FROTIX  
   ======================================================================================
   
   ⚠️⚠️⚠️  BACKUP OBRIGATÓRIO ANTES DE EXECUTAR  ⚠️⚠️⚠️
   
   DATA GERAÇÃO: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
   
   ESTE SCRIPT CONTÉM:
   - FASE 1: Migração de 5 tabelas existentes (ALTER TABLE) ✅ COM ROLLBACK
   - FASE 2: Criação de 31 tabelas + 67 views + 20 procedures ⚠️  SEM ROLLBACK (DDL)
   
   TEMPO ESTIMADO: 5-10 minutos
   
   ======================================================================================
*/

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
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

PRINT '';
PRINT '===================================================================================';
PRINT '                >>> ATUALIZACAO COMPLETA DO BANCO FROTIX <<<';
PRINT '===================================================================================';
PRINT '  Servidor: ' + @@SERVERNAME;
PRINT '  Banco: ' + DB_NAME();
PRINT '  Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '===================================================================================';
PRINT '';
GO

-- ======================================================================================
-- FASE 1: MIGRAÇÃO DE TABELAS EXISTENTES (COM TRANSAÇÕES E ROLLBACK)
-- ======================================================================================

$fase1Content

-- ======================================================================================
-- VERIFICAÇÃO ANTES DA FASE 2
-- ======================================================================================

IF EXISTS (SELECT 1 FROM [dbo].[__MigracaoLog] WHERE Status = 'ERRO')
BEGIN
    PRINT '';
    PRINT '===================================================================================';
    PRINT '  ERRO DETECTADO NA FASE 1 - FASE 2 NAO SERA EXECUTADA';
    PRINT '  Verifique o log: SELECT * FROM __MigracaoLog WHERE Status = ''ERRO''';
    PRINT '===================================================================================';
    RAISERROR('Execução abortada devido a erros na Fase 1', 16, 1);
    RETURN;
END
GO

-- ======================================================================================
-- FASE 2: CRIAÇÃO DE NOVOS OBJETOS
-- ======================================================================================

PRINT '';
PRINT '===================================================================================';
PRINT '                    FASE 2: CRIACAO DE NOVOS OBJETOS';
PRINT '  Esta fase criara 31 tabelas + 67 views + 20 procedures (~3-5 min)';
PRINT '===================================================================================';
PRINT '';
GO

$fase2Body

-- ======================================================================================
-- VALIDAÇÃO FINAL E RELATÓRIO
-- ======================================================================================

PRINT '';
PRINT '===================================================================================';
PRINT '                         ATUALIZACAO CONCLUIDA!';
PRINT '===================================================================================';
PRINT '';

SELECT Etapa, Status, COUNT(*) as Quantidade
FROM [dbo].[__MigracaoLog]
GROUP BY Etapa, Status
ORDER BY Etapa, Status;

PRINT '';
PRINT 'PROXIMOS PASSOS:';
PRINT '  1. Reinicie a aplicacao FrotiX';
PRINT '  2. Teste as funcionalidades principais';
PRINT '  3. Monitore logs de erro';
PRINT '';
PRINT 'Para remover tabela de log: DROP TABLE [dbo].[__MigracaoLog];';
PRINT '';
GO

-- ======================================================================================
-- SCRIPT DE ROLLBACK MANUAL (USAR APENAS SE NECESSÁRIO)
-- ======================================================================================
/*
-- Reverter Fase 1 (ALTER TABLE)
EXEC sp_rename 'dbo.Lavagem.HorarioLavagem', 'Horario', 'COLUMN';
ALTER TABLE [dbo].[Recurso] DROP COLUMN [HasChild];
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Nome] VARCHAR(250) NULL;
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [NomeMenu] VARCHAR(250) NULL;
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Icon] NVARCHAR(200) NULL;
ALTER TABLE [dbo].[Recurso] ALTER COLUMN [Href] NVARCHAR(500) NULL;
ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCancelamento] NVARCHAR(450) NULL;
ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioCriacao] NVARCHAR(450) NULL;
ALTER TABLE [dbo].[Manutencao] ALTER COLUMN [IdUsuarioFinalizacao] NVARCHAR(450) NULL;

-- Remover objetos da Fase 2 (se necessário)
DROP TABLE IF EXISTS [dbo].[LogErros];
DROP TABLE IF EXISTS [dbo].[EstatisticaMotoristasMensal];
DROP TABLE IF EXISTS [dbo].[EstatisticaVeiculoGeral];
-- [... adicionar mais conforme necessário ...]

PRINT 'Rollback manual executado.';
*/
"@

Write-Host "Salvando script final..." -ForegroundColor Yellow
$scriptFinal | Out-File $outputFile -Encoding UTF8

$fileSize = (Get-Item $outputFile).Length / 1MB
$lineCount = (Get-Content $outputFile | Measure-Object -Line).Lines

Write-Host ""
Write-Host "✅ SCRIPT INTEGRADO CRIADO COM SUCESSO!" -ForegroundColor Green
Write-Host ""
Write-Host "Arquivo: ATUALIZACAO_COMPLETA_COM_ROLLBACK.sql" -ForegroundColor Cyan
Write-Host "Tamanho: $("{0:N2}" -f $fileSize) MB" -ForegroundColor Gray
Write-Host "Linhas: $("{0:N0}" -f $lineCount)" -ForegroundColor Gray
Write-Host ""
Write-Host "===========================================================================" -ForegroundColor White
Write-Host "                          COMO EXECUTAR" -ForegroundColor White
Write-Host "===========================================================================" -ForegroundColor White
Write-Host ""
Write-Host "  1. BACKUP DO BANCO (OBRIGATORIO):" -ForegroundColor Yellow
Write-Host "     BACKUP DATABASE Frotix TO DISK = 'C:\Backup\Frotix_PreUpdate.bak'" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. EXECUTAR VIA SSMS:" -ForegroundColor Yellow
Write-Host "     - Abrir arquivo no SQL Server Management Studio" -ForegroundColor Gray
Write-Host "     - Conectar no servidor CTRAN01" -ForegroundColor Gray
Write-Host "     - Selecionar banco Frotix" -ForegroundColor Gray
Write-Host "     - Executar (F5)" -ForegroundColor Gray
Write-Host ""
Write-Host "  3. EXECUTAR VIA SQLCMD (LINHA DE COMANDO):" -ForegroundColor Yellow
Write-Host "     sqlcmd -S CTRAN01 -d Frotix -i ATUALIZACAO_COMPLETA_COM_ROLLBACK.sql" -ForegroundColor Gray
Write-Host ""
Write-Host "===========================================================================" -ForegroundColor White
Write-Host "  CARACTERISTICAS DO SCRIPT:" -ForegroundColor White
Write-Host "   OK FASE 1: Rollback automatico em caso de erro" -ForegroundColor Green
Write-Host "   AVISO FASE 2: Rollback manual necessario (veja final do script)" -ForegroundColor Yellow
Write-Host "   OK Log completo: Tabela __MigracaoLog" -ForegroundColor Green
Write-Host "   Tempo estimado: 5-10 minutos" -ForegroundColor Cyan
Write-Host "===========================================================================" -ForegroundColor White
Write-Host ""
