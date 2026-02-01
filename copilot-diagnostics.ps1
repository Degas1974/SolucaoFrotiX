# =============================================================================
# SCRIPT DE DIAGNÓSTICO DO GITHUB COPILOT
# =============================================================================
# Este script coleta informações detalhadas sobre o estado do GitHub Copilot
# =============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  DIAGNÓSTICO DO GITHUB COPILOT" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Função para formatar tamanho
function Format-FileSize {
    param([long]$Size)
    if ($Size -gt 1GB) { return "{0:N2} GB" -f ($Size / 1GB) }
    elseif ($Size -gt 1MB) { return "{0:N2} MB" -f ($Size / 1MB) }
    elseif ($Size -gt 1KB) { return "{0:N2} KB" -f ($Size / 1KB) }
    else { return "$Size bytes" }
}

# 1. Informações do Sistema
Write-Host "[1] INFORMAÇÕES DO SISTEMA" -ForegroundColor Green
Write-Host "Data/Hora: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor White
Write-Host "Sistema Operacional: $([System.Environment]::OSVersion.VersionString)" -ForegroundColor White
Write-Host "Usuário: $env:USERNAME" -ForegroundColor White
Write-Host ""

# 2. Versão do VS Code
Write-Host "[2] VS CODE" -ForegroundColor Green
try {
    $vscodeVersion = & code --version 2>&1
    Write-Host "Versão instalada:" -ForegroundColor White
    $vscodeVersion | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
} catch {
    Write-Host "  Erro ao obter versão do VS Code" -ForegroundColor Red
}

# Verificar se está rodando
$vscodeProcess = Get-Process -Name "Code" -ErrorAction SilentlyContinue
if ($vscodeProcess) {
    Write-Host "Status: EM EXECUÇÃO (PID: $($vscodeProcess.Id -join ', '))" -ForegroundColor Yellow
} else {
    Write-Host "Status: NÃO ESTÁ RODANDO" -ForegroundColor Gray
}
Write-Host ""

# 3. Extensões do Copilot
Write-Host "[3] EXTENSÕES DO GITHUB COPILOT" -ForegroundColor Green
$copilotExtensions = & code --list-extensions --show-versions 2>&1 | Select-String -Pattern "copilot"

if ($copilotExtensions) {
    foreach ($ext in $copilotExtensions) {
        Write-Host "  ✓ $ext" -ForegroundColor Green
    }
} else {
    Write-Host "  ✗ Nenhuma extensão do Copilot instalada!" -ForegroundColor Red
}
Write-Host ""

# 4. Extensões de AI/Chat (possíveis conflitos)
Write-Host "[4] OUTRAS EXTENSÕES DE AI/CHAT" -ForegroundColor Yellow
$aiExtensions = & code --list-extensions --show-versions 2>&1 | Select-String -Pattern "gpt|gemini|ai|claude|chat|openai"

if ($aiExtensions) {
    Write-Host "  ATENÇÃO: Múltiplas extensões de AI detectadas!" -ForegroundColor Red
    foreach ($ext in $aiExtensions) {
        Write-Host "  - $ext" -ForegroundColor Yellow
    }
    Write-Host "`n  Recomendação: Considere desabilitar extensões não essenciais" -ForegroundColor Cyan
} else {
    Write-Host "  Nenhuma outra extensão de AI detectada" -ForegroundColor Green
}
Write-Host ""

# 5. Configurações do Copilot
Write-Host "[5] CONFIGURAÇÕES DO COPILOT" -ForegroundColor Green
$settingsPath = "$env:APPDATA\Code\User\settings.json"

if (Test-Path $settingsPath) {
    $settings = Get-Content $settingsPath -Raw | ConvertFrom-Json
    $copilotSettings = $settings.PSObject.Properties | Where-Object {
        $_.Name -like "*copilot*"
    }

    if ($copilotSettings) {
        Write-Host "  Configurações encontradas:" -ForegroundColor White
        foreach ($setting in $copilotSettings) {
            Write-Host "    $($setting.Name): $($setting.Value)" -ForegroundColor Gray
        }
    } else {
        Write-Host "  Nenhuma configuração específica do Copilot encontrada" -ForegroundColor Yellow
    }
} else {
    Write-Host "  ✗ settings.json não encontrado!" -ForegroundColor Red
}
Write-Host ""

# 6. Cache e Armazenamento
Write-Host "[6] CACHE E ARMAZENAMENTO" -ForegroundColor Green

$storagePaths = @{
    "GlobalStorage (copilot)" = "$env:APPDATA\Code\User\globalStorage\github.copilot"
    "GlobalStorage (copilot-chat)" = "$env:APPDATA\Code\User\globalStorage\github.copilot-chat"
    "Cached VSIXs" = "$env:APPDATA\Code\CachedExtensionVSIXs"
    "Extensions (.vscode)" = "$env:USERPROFILE\.vscode\extensions"
}

foreach ($name in $storagePaths.Keys) {
    $path = $storagePaths[$name]
    if ($name -like "*copilot*") {
        $items = Get-Item $path -ErrorAction SilentlyContinue
    } else {
        $items = Get-ChildItem $path -Filter "*copilot*" -ErrorAction SilentlyContinue
    }

    if ($items) {
        $size = 0
        foreach ($item in $items) {
            if ($item.PSIsContainer) {
                $size += (Get-ChildItem -Path $item.FullName -Recurse -File -ErrorAction SilentlyContinue |
                         Measure-Object -Property Length -Sum).Sum
            } else {
                $size += $item.Length
            }
        }
        Write-Host "  $name :" -NoNewline -ForegroundColor White
        Write-Host " $(Format-FileSize $size)" -ForegroundColor Cyan
    } else {
        Write-Host "  $name :" -NoNewline -ForegroundColor White
        Write-Host " Não encontrado" -ForegroundColor Gray
    }
}
Write-Host ""

# 7. Logs Recentes
Write-Host "[7] LOGS RECENTES (ERROS)" -ForegroundColor Green
$logsPath = "$env:APPDATA\Code\logs"
$recentLogs = Get-ChildItem -Path $logsPath -Directory |
              Sort-Object CreationTime -Descending |
              Select-Object -First 1

if ($recentLogs) {
    Write-Host "  Última sessão: $($recentLogs.Name)" -ForegroundColor White

    $copilotLogs = Get-ChildItem -Path $recentLogs.FullName -Recurse -Filter "*copilot*.log" -ErrorAction SilentlyContinue

    if ($copilotLogs) {
        foreach ($log in $copilotLogs | Select-Object -First 2) {
            Write-Host "`n  Arquivo: $($log.Name)" -ForegroundColor Cyan
            $errors = Get-Content $log.FullName -Tail 20 | Select-String -Pattern "\[error\]"

            if ($errors) {
                Write-Host "  Últimos erros encontrados:" -ForegroundColor Yellow
                $errors | Select-Object -First 5 | ForEach-Object {
                    Write-Host "    $_" -ForegroundColor Red
                }
            } else {
                Write-Host "  ✓ Nenhum erro recente" -ForegroundColor Green
            }
        }
    } else {
        Write-Host "  Nenhum log do Copilot encontrado" -ForegroundColor Gray
    }
} else {
    Write-Host "  ✗ Nenhum log encontrado!" -ForegroundColor Red
}
Write-Host ""

# 8. Autenticação
Write-Host "[8] STATUS DE AUTENTICAÇÃO" -ForegroundColor Green
$authFiles = @(
    "$env:APPDATA\Code\User\globalStorage\github.copilot\versions",
    "$env:APPDATA\Code\User\globalStorage\github.copilot-chat"
)

$authenticated = $false
foreach ($path in $authFiles) {
    if (Test-Path $path) {
        $authenticated = $true
        break
    }
}

if ($authenticated) {
    Write-Host "  ✓ Dados de autenticação encontrados" -ForegroundColor Green

    # Tentar encontrar informações do usuário nos logs
    if ($recentLogs) {
        $copilotLogs = Get-ChildItem -Path $recentLogs.FullName -Recurse -Filter "*copilot*.log" -ErrorAction SilentlyContinue
        if ($copilotLogs) {
            $userInfo = Get-Content $copilotLogs[0].FullName | Select-String -Pattern "Logged in as" | Select-Object -Last 1
            if ($userInfo) {
                Write-Host "  $userInfo" -ForegroundColor White
            }

            $tokenInfo = Get-Content $copilotLogs[0].FullName | Select-String -Pattern "sku:" | Select-Object -Last 1
            if ($tokenInfo) {
                Write-Host "  $tokenInfo" -ForegroundColor White
            }
        }
    }
} else {
    Write-Host "  ✗ Não autenticado ou dados não encontrados" -ForegroundColor Red
    Write-Host "  Ação necessária: Faça login no GitHub Copilot" -ForegroundColor Yellow
}
Write-Host ""

# Resumo e Recomendações
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  RESUMO E RECOMENDAÇÕES" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$issues = @()

# Verificar múltiplas extensões AI
if ($aiExtensions.Count -gt 2) {
    $issues += "Múltiplas extensões de AI detectadas - pode causar conflitos"
}

# Verificar autenticação
if (-not $authenticated) {
    $issues += "Autenticação não detectada - faça login no GitHub Copilot"
}

# Verificar tamanho do cache
$copilotChatPath = "$env:APPDATA\Code\User\globalStorage\github.copilot-chat"
if (Test-Path $copilotChatPath) {
    $cacheSize = (Get-ChildItem -Path $copilotChatPath -Recurse -File |
                  Measure-Object -Property Length -Sum).Sum
    if ($cacheSize -gt 50MB) {
        $issues += "Cache grande detectado ($(Format-FileSize $cacheSize)) - considere limpeza"
    }
}

if ($issues.Count -gt 0) {
    Write-Host "`nProblemas detectados:" -ForegroundColor Red
    $i = 1
    foreach ($issue in $issues) {
        Write-Host "  $i. $issue" -ForegroundColor Yellow
        $i++
    }

    Write-Host "`nSoluções sugeridas:" -ForegroundColor Green
    Write-Host "  1. Execute: .\copilot-cleanup.ps1 (limpeza de cache)" -ForegroundColor White
    Write-Host "  2. Execute: .\copilot-full-reset.ps1 (reset completo)" -ForegroundColor White
    Write-Host "  3. Desabilite extensões de AI não essenciais" -ForegroundColor White
} else {
    Write-Host "✓ Nenhum problema crítico detectado!" -ForegroundColor Green
}

Write-Host ""
Write-Host "Diagnóstico completo salvo em clipboard (pressione Ctrl+V para colar)" -ForegroundColor Cyan
Write-Host ""

# Gerar relatório para clipboard
$report = @"
=== DIAGNÓSTICO GITHUB COPILOT - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') ===

VS Code: $($vscodeVersion -join ' | ')
Extensões Copilot: $($copilotExtensions.Count)
Outras extensões AI: $($aiExtensions.Count)
Autenticado: $authenticated

Problemas: $($issues.Count)
$(if ($issues.Count -gt 0) { $issues | ForEach-Object { "- $_" } } else { "Nenhum" })
"@

$report | Set-Clipboard
