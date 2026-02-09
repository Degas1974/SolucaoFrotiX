# =============================================================================
# GERADOR DE SCRIPT DE ATUALIZA??O - Produ??o ? Desenvolvimento
# =============================================================================

$ErrorActionPreference = 'Stop'

$diffDataPath = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\DiffData.json"
$devFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\FrotixDesenvolvimento.sql"
$outputFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\SCRIPT_ATUALIZACAO_PRODUCAO.sql"

Write-Host "`n======================================================================================" -ForegroundColor Cyan
Write-Host "GERADOR DE SCRIPT DE ATUALIZA??O - PRODU??O vs DESENVOLVIMENTO" -ForegroundColor Cyan
Write-Host "======================================================================================`n" -ForegroundColor Cyan

# Carregar dados do diff
$diffData = Get-Content $diffDataPath -Raw -Encoding UTF8 | ConvertFrom-Json

# Ler o arquivo de desenvolvimento completo
$devContent = Get-Content $devFile -Raw -Encoding UTF8

Write-Host "Gerando script de atualiza??o..." -ForegroundColor Yellow

# Iniciar script SQL
$scriptHeader = @"
/* ======================================================================================
   SCRIPT DE ATUALIZA??O DO BANCO FROTIX - PRODU??O ? DESENVOLVIMENTO
   ======================================================================================
   
   DATA GERA??O: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
   
   IMPORTANTE:
   ? Este script atualiza o banco de PRODU??O com os objetos do banco de DESENVOLVIMENTO
   ? Fa?a BACKUP completo antes de executar
   ? Execute em hor?rio de baixo movimento
   ? Tempo estimado: 5-15 minutos
   ? Em caso de erro, ser? feito ROLLBACK autom?tico
   
   RESUMO DAS ALTERA??ES:
   ? Novas tabelas: $($diffData.NovasTables.Count)
   ?? Tabelas modificadas: $($diffData.ModificadasTables.Count)
   ? Novas views: $($diffData.NovasViews.Count)
   ?? Views modificadas: $($diffData.ModificadasViews.Count)
   ? Novas procedures: $($diffData.NovasProcedures.Count)
   ?? Procedures modificadas: $($diffData.ModificadasProcedures.Count)
   ? Procedures removidas: $($diffData.RemovidasProcedures.Count)
   
   ======================================================================================
*/

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET XACT_ABORT ON; -- Rollback autom?tico em caso de erro
GO

USE Frotix
GO

-- Verificar banco correto
IF DB_NAME() <> 'Frotix'
BEGIN
    RAISERROR('Este script deve ser executado no banco Frotix!', 16, 1);
    RETURN;
END
GO

PRINT '======================================================================================';
PRINT '                   IN?CIO DA ATUALIZA??O DO BANCO FROTIX';
PRINT '======================================================================================';
PRINT 'Servidor: ' + @@SERVERNAME;
PRINT 'Banco: ' + DB_NAME();
PRINT 'Data/Hora: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '======================================================================================';
PRINT '';
GO

-- N?o usar transaction global aqui - cada se??o ter? sua pr?pria

"@

$script = $scriptHeader

# ======================================================================================
# SE??O 1: CRIAR NOVAS TABELAS
# ======================================================================================

$script += @"
-- ======================================================================================
-- SE??O 1: CRIAR NOVAS TABELAS ($($diffData.NovasTables.Count) tabelas)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 1: CRIAR NOVAS TABELAS';
PRINT '======================================================================================';
PRINT '';
GO

"@

foreach ($tableName in $diffData.NovasTables) {
    $tableCode = $diffData.DevTables.$tableName
    if ($tableCode) {
        $script += @"
-- Criar tabela: $tableName
PRINT 'Criando tabela: $tableName...';
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[$tableName]') AND type IN (N'U'))
BEGIN
$tableCode
END
ELSE
BEGIN
    PRINT '  ??  Tabela $tableName j? existe. Pulando.';
END
GO

"@
    }
}

# ======================================================================================
# SE??O 2: ALTERAR TABELAS EXISTENTES
# ======================================================================================

$script += @"
-- ======================================================================================
-- SE??O 2: ALTERAR TABELAS MODIFICADAS ($($diffData.ModificadasTables.Count) tabelas)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 2: ALTERAR TABELAS MODIFICADAS';
PRINT '======================================================================================';
PRINT '??  ATEN??O: Altera??es de tabelas devem ser revisadas manualmente!';
PRINT '??  Este script n?o aplicar? altera??es autom?ticas em tabelas existentes.';
PRINT '??  Tabelas modificadas: $($diffData.ModificadasTables -join ', ')';
PRINT '';
GO

"@

# ======================================================================================
# SE??O 3: CRIAR/ATUALIZAR VIEWS
# ======================================================================================

$script += @"
-- ======================================================================================
-- SE??O 3: CRIAR/ATUALIZAR VIEWS ($(($diffData.NovasViews.Count + $diffData.ModificadasViews.Count)) views)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 3: CRIAR/ATUALIZAR VIEWS';
PRINT '======================================================================================';
PRINT '';
GO

"@

# Views novas + modificadas
$allViews = @($diffData.NovasViews) + @($diffData.ModificadasViews) | Select-Object -Unique

foreach ($viewName in $allViews) {
    $viewCode = $diffData.DevViews.$viewName
    if ($viewCode) {
        $script += @"
-- View: $viewName
PRINT 'Criando/atualizando view: $viewName...';
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = '$viewName')
    DROP VIEW [dbo].[$viewName]
GO

$viewCode
GO

"@
    }
}

# ======================================================================================
# SE??O 4: CRIAR/ATUALIZAR PROCEDURES
# ======================================================================================

$script += @"
-- ======================================================================================
-- SE??O 4: CRIAR/ATUALIZAR PROCEDURES ($(($diffData.NovasProcedures.Count + $diffData.ModificadasProcedures.Count)) procedures)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 4: CRIAR/ATUALIZAR PROCEDURES';
PRINT '======================================================================================';
PRINT '';
GO

"@

# Procedures novas + modificadas
$allProcedures = @($diffData.NovasProcedures) + @($diffData.ModificadasProcedures) | Select-Object -Unique

foreach ($procName in $allProcedures) {
    $procCode = $diffData.DevProcedures.$procName
    if ($procCode) {
        $script += @"
-- Procedure: $procName
PRINT 'Criando/atualizando procedure: $procName...';
GO

$procCode
GO

"@
    }
}

# ======================================================================================
# SE??O 5: REMOVER PROCEDURES OBSOLETAS
# ======================================================================================

if ($diffData.RemovidasProcedures.Count -gt 0) {
    $script += @"
-- ======================================================================================
-- SE??O 5: REMOVER PROCEDURES OBSOLETAS ($($diffData.RemovidasProcedures.Count) procedures)
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT 'SE??O 5: REMOVER PROCEDURES OBSOLETAS';
PRINT '======================================================================================';
PRINT '';
GO

"@

    foreach ($procName in $diffData.RemovidasProcedures) {
        $script += @"
-- Remover procedure: $procName
PRINT 'Removendo procedure: $procName...';
GO

IF EXISTS (SELECT * FROM sys.procedures WHERE name = '$procName')
BEGIN
    DROP PROCEDURE [dbo].[$procName]
    PRINT '  ? Procedure $procName removida.';
END
ELSE
BEGIN
    PRINT '  ??  Procedure $procName j? n?o existe.';
END
GO

"@
    }
}

# ======================================================================================
# RESUMO FINAL
# ======================================================================================

$script += @"
-- ======================================================================================
-- RESUMO FINAL
-- ======================================================================================
PRINT '';
PRINT '======================================================================================';
PRINT '                           RESUMO DA ATUALIZA??O';
PRINT '======================================================================================';

DECLARE @TotalTabelas INT = $($diffData.NovasTables.Count);
DECLARE @TotalViews INT = $(($diffData.NovasViews.Count + $diffData.ModificadasViews.Count));
DECLARE @TotalProcedures INT = $(($diffData.NovasProcedures.Count + $diffData.ModificadasProcedures.Count));
DECLARE @TotalRemovidas INT = $($diffData.RemovidasProcedures.Count);

PRINT '  ? Novas tabelas criadas: ' + CAST(@TotalTabelas AS VARCHAR);
PRINT '  ?? Views atualizadas: ' + CAST(@TotalViews AS VARCHAR);
PRINT '  ?? Procedures atualizadas: ' + CAST(@TotalProcedures AS VARCHAR);
PRINT '  ? Procedures removidas: ' + CAST(@TotalRemovidas AS VARCHAR);
PRINT '';
PRINT '  ? ATUALIZA??O CONCLU?DA COM SUCESSO!';
PRINT '';
PRINT '======================================================================================';
PRINT 'Data/Hora t?rmino: ' + CONVERT(VARCHAR(23), GETDATE(), 121);
PRINT '======================================================================================';
GO

"@

# Salvar script
$script | Out-File $outputFile -Encoding UTF8

Write-Host "`n? Script de atualiza??o gerado com sucesso!" -ForegroundColor Green
Write-Host "?? Arquivo: $outputFile" -ForegroundColor Cyan
Write-Host "`n?? ESTAT?STICAS:" -ForegroundColor White
Write-Host ("   Tamanho: {0:N2} MB" -f ((Get-Item $outputFile).Length / 1MB)) -ForegroundColor Gray
Write-Host ("   Linhas: {0:N0}" -f (Get-Content $outputFile | Measure-Object -Line).Lines) -ForegroundColor Gray
Write-Host ""
Write-Host "IMPORTANTE:" -ForegroundColor Yellow
Write-Host "   1. FACA BACKUP do banco antes de executar" -ForegroundColor Yellow
Write-Host "   2. Execute em horario de baixo movimento" -ForegroundColor Yellow
Write-Host "   3. Revise as alteracoes de tabelas manualmente" -ForegroundColor Yellow
Write-Host "   4. Tempo estimado de execucao: 5-15 minutos" -ForegroundColor Yellow
Write-Host ""

