# ╔══════════════════════════════════════════════════════════════════╗
# ║  FrotiX - Script de Instalação para Nova Máquina               ║
# ║  Instala extensão + configurações globais do VS Code            ║
# ║  Versão: 1.0.0 | Data: 10/02/2026                              ║
# ╚══════════════════════════════════════════════════════════════════╝

param(
    [switch]$Regular  # Usar VS Code regular ao invés do Insiders
)

$ErrorActionPreference = "Stop"

# ─────────────────────────────────────────────────────────────────
# DETECÇÃO DE AMBIENTE
# ─────────────────────────────────────────────────────────────────

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

if ($Regular) {
    $CodeCmd = "code"
    $SettingsDir = "$env:APPDATA\Code\User"
    $CodeName = "VS Code"
} else {
    $CodeCmd = "code-insiders"
    $SettingsDir = "$env:APPDATA\Code - Insiders\User"
    $CodeName = "VS Code Insiders"
}

$ContinueDir = "$env:USERPROFILE\.continue"
$BackupDir = "$ScriptDir\backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
$Timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# ─────────────────────────────────────────────────────────────────
# FUNÇÕES
# ─────────────────────────────────────────────────────────────────

function Write-Step($msg) {
    Write-Host "`n▶ $msg" -ForegroundColor Cyan
}

function Write-Ok($msg) {
    Write-Host "  ✅ $msg" -ForegroundColor Green
}

function Write-Warn($msg) {
    Write-Host "  ⚠️  $msg" -ForegroundColor Yellow
}

function Write-Err($msg) {
    Write-Host "  ❌ $msg" -ForegroundColor Red
}

function Backup-File($filePath, $label) {
    if (Test-Path $filePath) {
        if (-not (Test-Path $BackupDir)) {
            New-Item -ItemType Directory -Path $BackupDir -Force | Out-Null
        }
        Copy-Item $filePath "$BackupDir\$label"
        Write-Ok "Backup: $label"
    }
}

# ─────────────────────────────────────────────────────────────────
# BANNER
# ─────────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "╔══════════════════════════════════════════════════════╗" -ForegroundColor Magenta
Write-Host "║  FrotiX - Instalação de Configurações               ║" -ForegroundColor Magenta
Write-Host "║  Alvo: $($CodeName.PadRight(44))║" -ForegroundColor Magenta
Write-Host "║  Data: $($Timestamp.PadRight(44))║" -ForegroundColor Magenta
Write-Host "╚══════════════════════════════════════════════════════╝" -ForegroundColor Magenta

# ─────────────────────────────────────────────────────────────────
# VERIFICAÇÃO DE PRÉ-REQUISITOS
# ─────────────────────────────────────────────────────────────────

Write-Step "Verificando pré-requisitos..."

# Verificar VS Code
try {
    $codeVersion = & $CodeCmd --version 2>&1 | Select-Object -First 1
    Write-Ok "$CodeName encontrado: $codeVersion"
} catch {
    Write-Err "$CodeName não encontrado! Instale antes de continuar."
    Write-Host "  Download: https://code.visualstudio.com/insiders/" -ForegroundColor Gray
    exit 1
}

# Verificar se diretório User existe
if (-not (Test-Path $SettingsDir)) {
    New-Item -ItemType Directory -Path $SettingsDir -Force | Out-Null
    Write-Warn "Diretório $SettingsDir criado (não existia)"
}

# Verificar arquivos do pacote
$vsixFile = "$ScriptDir\frotix-conversa-manager-1.0.0.vsix"
if (-not (Test-Path $vsixFile)) {
    Write-Err "Arquivo .vsix não encontrado em: $vsixFile"
    exit 1
}
Write-Ok "Pacote .vsix encontrado"

# ─────────────────────────────────────────────────────────────────
# PASSO 1: INSTALAR EXTENSÃO
# ─────────────────────────────────────────────────────────────────

Write-Step "Instalando extensão FrotiX Conversa Manager..."

try {
    & $CodeCmd --install-extension $vsixFile --force 2>&1 | Out-Null
    Write-Ok "Extensão instalada com sucesso"
} catch {
    Write-Err "Falha ao instalar extensão: $_"
}

# ─────────────────────────────────────────────────────────────────
# PASSO 2: KEYBINDINGS GLOBAIS
# ─────────────────────────────────────────────────────────────────

Write-Step "Configurando keybindings globais..."

$keybindingsTarget = "$SettingsDir\keybindings.json"
$keybindingsSource = "$ScriptDir\global-keybindings.json"

if (Test-Path $keybindingsSource) {
    Backup-File $keybindingsTarget "keybindings.json.bak"
    Copy-Item $keybindingsSource $keybindingsTarget -Force
    Write-Ok "keybindings.json atualizado"
} else {
    Write-Warn "global-keybindings.json não encontrado, pulando..."
}

# ─────────────────────────────────────────────────────────────────
# PASSO 3: CONFIGURAÇÕES DAS IAs (MERGE NO SETTINGS.JSON)
# ─────────────────────────────────────────────────────────────────

Write-Step "Mesclando configurações das IAs no settings.json..."

$settingsTarget = "$SettingsDir\settings.json"
$iaSettingsSource = "$ScriptDir\global-ia-settings.jsonc"

if (Test-Path $iaSettingsSource) {
    Backup-File $settingsTarget "settings.json.bak"

    # Ler settings existente ou criar novo
    if (Test-Path $settingsTarget) {
        $settingsContent = Get-Content $settingsTarget -Raw -Encoding UTF8
    } else {
        $settingsContent = "{}"
    }

    # Ler as configurações das IAs (remover comentários JSONC)
    $iaContent = Get-Content $iaSettingsSource -Raw -Encoding UTF8
    $iaClean = $iaContent -replace '//.*$', '' -replace '/\*[\s\S]*?\*/', ''

    try {
        $settingsObj = $settingsContent | ConvertFrom-Json
        $iaObj = $iaClean | ConvertFrom-Json

        # Mesclar cada propriedade das IAs no settings
        $iaObj.PSObject.Properties | ForEach-Object {
            $settingsObj | Add-Member -NotePropertyName $_.Name -NotePropertyValue $_.Value -Force
        }

        # Salvar com formatação
        $settingsObj | ConvertTo-Json -Depth 10 | Set-Content $settingsTarget -Encoding UTF8
        Write-Ok "settings.json atualizado com configurações das 5 IAs"
    } catch {
        Write-Err "Falha ao mesclar settings.json: $_"
        Write-Warn "Copie manualmente as configurações de global-ia-settings.jsonc"
    }
} else {
    Write-Warn "global-ia-settings.jsonc não encontrado, pulando..."
}

# ─────────────────────────────────────────────────────────────────
# PASSO 4: CONTINUE CONFIG
# ─────────────────────────────────────────────────────────────────

Write-Step "Configurando Continue..."

$continueTarget = "$ContinueDir\config.json"
$continueSource = "$ScriptDir\continue-config.json"

if (Test-Path $continueSource) {
    if (-not (Test-Path $ContinueDir)) {
        New-Item -ItemType Directory -Path $ContinueDir -Force | Out-Null
    }
    Backup-File $continueTarget "continue-config.json.bak"
    Copy-Item $continueSource $continueTarget -Force

    # Aviso sobre API key
    Write-Ok "config.json do Continue instalado"
    Write-Warn "ATENÇÃO: Edite $continueTarget e substitua 'SUBSTITUIR_PELA_SUA_API_KEY' pela sua chave real"
} else {
    Write-Warn "continue-config.json não encontrado, pulando..."
}

# ─────────────────────────────────────────────────────────────────
# RESUMO FINAL
# ─────────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "╔══════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║  INSTALAÇÃO CONCLUÍDA                                ║" -ForegroundColor Green
Write-Host "╚══════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

if (Test-Path $BackupDir) {
    Write-Host "  📦 Backups salvos em:" -ForegroundColor Gray
    Write-Host "     $BackupDir" -ForegroundColor Gray
}

Write-Host ""
Write-Host "  📋 O que foi instalado:" -ForegroundColor White
Write-Host "     ✅ Extensão FrotiX Conversa Manager (.vsix)" -ForegroundColor White
Write-Host "     ✅ Keybindings globais (Ctrl+Alt+C/S/F e atalhos de painéis)" -ForegroundColor White
Write-Host "     ✅ Custom instructions das 5 IAs (settings.json)" -ForegroundColor White
Write-Host "     ✅ Continue /doq command (config.json)" -ForegroundColor White

Write-Host ""
Write-Host "  ⚡ Próximos passos:" -ForegroundColor Yellow
Write-Host "     1. Recarregue o $CodeName (Ctrl+Shift+P → 'Reload Window')" -ForegroundColor Yellow
Write-Host "     2. Edite a API key do Continue se necessário" -ForegroundColor Yellow
Write-Host "     3. Clone o repositório FrotiX (git clone ...)" -ForegroundColor Yellow
Write-Host "     4. Abra o workspace e teste: Ctrl+Alt+C" -ForegroundColor Yellow

Write-Host ""
Write-Host "  🎹 Atalhos FrotiX Conversa Manager:" -ForegroundColor Cyan
Write-Host "     Ctrl+Alt+C = Nova Conversa" -ForegroundColor Cyan
Write-Host "     Ctrl+Alt+S = Checkpoint (salvar progresso)" -ForegroundColor Cyan
Write-Host "     Ctrl+Alt+F = Finalizar conversa" -ForegroundColor Cyan
Write-Host ""
