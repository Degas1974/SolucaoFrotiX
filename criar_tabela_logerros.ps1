# Script para criar tabela LogErros
Write-Output "================================================================================"
Write-Output "CRIACAO DA TABELA LogErros - VERSAO CORRIGIDA"
Write-Output "================================================================================"

$sqlFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\SQL\001_Create_Table_LogErros.sql"

Write-Output "`nExecutando script SQL..."
Write-Output "Arquivo: $sqlFile"
Write-Output ""

try {
    # Executar SQL e capturar output
    $output = & sqlcmd -S localhost -d Frotix -E -i $sqlFile 2>&1
    
    # Exibir todo o output
    $output | ForEach-Object {
        Write-Output $_
    }
    
    Write-Output "`n================================================================================"
    Write-Output "VERIFICANDO SE A TABELA FOI CRIADA..."
    Write-Output "================================================================================"
    
    # Verificar se tabela existe
    $checkOutput = & sqlcmd -S localhost -d Frotix -E -Q "SELECT CASE WHEN OBJECT_ID('dbo.LogErros', 'U') IS NOT NULL THEN '*** SUCESSO: TABELA LogErros CRIADA ***' ELSE '*** ERRO: TABELA NAO EXISTE ***' END AS Status" -W -h -1 2>&1
    
    Write-Output ""
    $checkOutput | ForEach-Object { Write-Output $_ }
    Write-Output ""
    
} catch {
    Write-Output "`nERRO CRITICO:"
    Write-Output $_.Exception.Message
}

Write-Output "`n================================================================================"
Write-Output "PRESSIONE QUALQUER TECLA PARA FECHAR"
Write-Output "================================================================================"
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
