#!/bin/bash

cd /mnt/d/FrotiX/SolucaoFrotiX

echo "📂 Criando arquivos de configuração das IAs..."

mkdir -p .claude .continue .github .gemini

cp templates/.claude/project-rules.md .claude/
cp templates/.continue/rules.md .continue/
cp templates/.github/copilot-instructions.md .github/
cp templates/.gemini/instructions.md .gemini/

echo "✅ Arquivos criados!"
ls -la .claude/ .continue/ .github/ .gemini/