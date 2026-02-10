#!/bin/bash

echo "============================================"
echo "  VERIFICAÇÃO DE CONFIGURAÇÃO DAS IAs"
echo "  Projeto FrotiX - $(date '+%d/%m/%Y %H:%M')"
echo "============================================"
echo ""

# Cores para output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Contador de resultados
total=0
passed=0
failed=0

# Função para verificar arquivo
check_file() {
    local file=$1
    local name=$2
    total=$((total + 1))

    if [ -f "$file" ]; then
        echo -e "${GREEN}✅${NC} $name: $file"
        passed=$((passed + 1))
    else
        echo -e "${RED}❌${NC} $name: $file (não encontrado)"
        failed=$((failed + 1))
    fi
}

# Função para verificar mensagem de confirmação
check_confirmation() {
    local file=$1
    local ia_name=$2
    local search_term=$3

    if [ -f "$file" ]; then
        if grep -q "$search_term" "$file"; then
            echo -e "   ${GREEN}→${NC} Confirmação visual presente"
            return 0
        else
            echo -e "   ${YELLOW}⚠${NC} Confirmação visual ausente"
            return 1
        fi
    fi
    return 1
}

echo "📁 VERIFICANDO ARQUIVOS PRINCIPAIS..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

# Verificar arquivos principais
check_file "RegrasDesenvolvimentoFrotiX.md" "Regras Unificadas"
check_file "FrotiX.sql" "Estrutura do Banco"
echo ""

echo "🤖 VERIFICANDO CONFIGURAÇÃO DAS IAs..."
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

# Claude Code
echo "1. CLAUDE CODE"
check_file "CLAUDE.md" "   Arquivo principal"
check_confirmation "CLAUDE.md" "Claude Code" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
echo ""

# Cursor AI
echo "2. CURSOR AI"
check_file ".cursor/rules.md" "   Arquivo de regras"
check_confirmation ".cursor/rules.md" "Cursor AI" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
check_file ".cursorrules" "   Arquivo legado"
echo ""

# GitHub Copilot
echo "3. GITHUB COPILOT"
check_file ".github/copilot-instructions.md" "   Instruções principais"
check_confirmation ".github/copilot-instructions.md" "Copilot" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
check_file ".vscode/settings.json" "   Settings VS Code"
check_confirmation ".vscode/settings.json" "Copilot Settings" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
check_file ".copilot-instructions.md" "   Instruções raiz"
echo ""

# Continue
echo "4. CONTINUE"
check_file ".continue/config.json" "   Configuração"
check_confirmation ".continue/config.json" "Continue Config" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
check_file ".continue/rules.md" "   Arquivo de regras"
check_confirmation ".continue/rules.md" "Continue Rules" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
check_file ".continue/rules/frotix-rules.md" "   Regras específicas"
check_confirmation ".continue/rules/frotix-rules.md" "Continue Frotix Rules" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
echo ""

# Gemini
echo "5. GEMINI / GENIE AI"
check_file ".gemini/instructions.md" "   Instruções"
check_confirmation ".gemini/instructions.md" "Gemini" "CONFIRMAÇÃO VISUAL OBRIGATÓRIA"
check_file ".gemini/settings.json" "   Settings"
echo ""

echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo "📊 RESUMO"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo -e "Total de verificações: $total"
echo -e "${GREEN}Passou: $passed${NC}"
echo -e "${RED}Falhou: $failed${NC}"
echo ""

if [ $failed -eq 0 ]; then
    echo -e "${GREEN}✅ TODAS AS VERIFICAÇÕES PASSARAM!${NC}"
    echo ""
    echo "🎯 PRÓXIMOS PASSOS:"
    echo "   1. Testar cada IA abrindo um novo chat"
    echo "   2. Verificar se a mensagem de confirmação visual aparece"
    echo "   3. Perguntar sobre as regras críticas do projeto"
    exit 0
else
    echo -e "${RED}❌ ALGUMAS VERIFICAÇÕES FALHARAM${NC}"
    echo ""
    echo "Revise os arquivos marcados como ausentes acima."
    exit 1
fi
