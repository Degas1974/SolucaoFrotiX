#!/usr/bin/env python3
"""
═══════════════════════════════════════════════════════════════════════════
  SUPERVISOR INTELIGENTE - DOCUMENTAÇÃO FROTIX
═══════════════════════════════════════════════════════════════════════════

Objetivo: Gerenciar automaticamente o processo completo de documentação

Funcionalidades:
- ✅ Monitora progresso em tempo real
- ✅ Determina próximos arquivos a documentar
- ✅ Gera prompts específicos para cada lote
- ✅ Faz commits automáticos nos marcos (15%, 20%, 25%, etc.)
- ✅ Cria FimProcesso-<timestamp>.md ao atingir 100%
- ✅ Gerencia erros e retentativas
- ✅ Fornece relatórios detalhados

Uso:
    python3 supervisor-inteligente.py
"""

import os
import re
import time
import subprocess
from datetime import datetime
from pathlib import Path
from typing import List, Tuple, Dict

# Configurações
DOC_FILE = "FrotiX.Site/DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md"
TOTAL_ARQUIVOS = 924
TAMANHO_LOTE = 10
MILESTONES = [15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100]

# Cores ANSI
class Color:
    RED = '\033[0;31m'
    GREEN = '\033[0;32m'
    YELLOW = '\033[1;33m'
    BLUE = '\033[0;34m'
    CYAN = '\033[0;36m'
    MAGENTA = '\033[0;35m'
    BOLD = '\033[1m'
    RESET = '\033[0m'

def contar_documentados() -> int:
    """Conta quantos arquivos já foram documentados"""
    try:
        if not os.path.exists(DOC_FILE):
            return 0

        with open(DOC_FILE, 'r', encoding='utf-8') as f:
            content = f.read()
            # Contar linhas com [x]
            return len(re.findall(r'^\- \[x\]', content, re.MULTILINE))
    except Exception as e:
        print(f"{Color.RED}Erro ao contar documentados: {e}{Color.RESET}")
        return 0

def calcular_percentual(atual: int) -> float:
    """Calcula percentual de progresso"""
    return round((atual * 100) / TOTAL_ARQUIVOS, 2)

def proximo_marco(percentual: float) -> int:
    """Retorna o próximo marco a ser atingido"""
    for marco in MILESTONES:
        if percentual < marco:
            return marco
    return 100

def atingiu_marco(percentual: float, ultimo_commit: float) -> int:
    """Verifica se atingiu um novo marco"""
    for marco in MILESTONES:
        if percentual >= marco and ultimo_commit < marco:
            return marco
    return 0

def listar_controllers_pendentes() -> List[str]:
    """Lista Controllers que ainda não foram documentados"""
    controllers_dir = Path("FrotiX.Site/Controllers")

    if not controllers_dir.exists():
        print(f"{Color.RED}Diretório Controllers não encontrado!{Color.RESET}")
        return []

    # Listar todos os .cs no diretório Controllers
    todos = []
    for arquivo in sorted(controllers_dir.glob("*.cs")):
        todos.append(arquivo.name)

    # Verificar quais já foram documentados
    try:
        with open(DOC_FILE, 'r', encoding='utf-8') as f:
            content = f.read()
            documentados = re.findall(r'\- \[x\] /FrotiX\.Site/Controllers/([^\s]+)', content)
    except Exception as e:
        print(f"{Color.YELLOW}Aviso: não conseguiu ler documentados - {e}{Color.RESET}")
        documentados = []

    # Retornar apenas os pendentes
    pendentes = [c for c in todos if c not in documentados]
    return pendentes

def commit_marco(marco: int, arquivos: int) -> bool:
    """Faz commit de um marco específico"""
    print(f"\n{Color.CYAN}{'═' * 60}{Color.RESET}")
    print(f"{Color.GREEN}{Color.BOLD}🎯 MARCO ATINGIDO: {marco}% ({arquivos} arquivos){Color.RESET}")
    print(f"{Color.CYAN}{'═' * 60}{Color.RESET}\n")

    try:
        # Adicionar arquivos
        subprocess.run(['git', 'add', 'FrotiX.Site/Controllers/', 'FrotiX.Site/Areas/', DOC_FILE],
                      check=False, capture_output=True)

        # Criar mensagem de commit
        commit_msg = f"""docs: documentação FrotiX - Marco {marco}%

Progresso: {arquivos}/{TOTAL_ARQUIVOS} arquivos documentados

✅ Cards ASCII adicionados
✅ Try-catch implementados
✅ Comentários [DOC] em lógica complexa
✅ DocumentacaoIntracodigo.md atualizado

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>"""

        # Fazer commit
        result = subprocess.run(['git', 'commit', '-m', commit_msg],
                               capture_output=True, text=True)

        if result.returncode == 0:
            print(f"{Color.GREEN}✅ Commit criado com sucesso{Color.RESET}")

            # Tentar push
            push_result = subprocess.run(['git', 'push'], capture_output=True, text=True)
            if push_result.returncode == 0:
                print(f"{Color.GREEN}✅ Push realizado com sucesso{Color.RESET}")
            else:
                print(f"{Color.YELLOW}⚠️  Push falhou (pode estar offline){Color.RESET}")

            return True
        else:
            if "nothing to commit" in result.stdout or "nothing to commit" in result.stderr:
                print(f"{Color.YELLOW}⚠️  Nenhuma mudança para commitar{Color.RESET}")
            else:
                print(f"{Color.RED}❌ Erro ao criar commit: {result.stderr}{Color.RESET}")
            return False

    except Exception as e:
        print(f"{Color.RED}❌ Erro ao fazer commit: {e}{Color.RESET}")
        return False

def criar_arquivo_conclusao():
    """Cria arquivo de conclusão quando atinge 100%"""
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    arquivo = f"FimProcesso-{timestamp}.md"

    # Obter estatísticas de commits
    try:
        log_result = subprocess.run(
            ['git', 'log', '--oneline', '--grep=docs: documentação FrotiX'],
            capture_output=True, text=True
        )
        commits_log = log_result.stdout.strip()
    except:
        commits_log = "Não foi possível obter histórico de commits"

    content = f"""# 🎉 DOCUMENTAÇÃO FROTIX CONCLUÍDA

**Data de Conclusão**: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}
**Total de Arquivos**: {TOTAL_ARQUIVOS}
**Progresso**: 100% ✅

---

## 📊 Estatísticas Finais

| Métrica | Valor |
|---------|-------|
| Arquivos Documentados | {TOTAL_ARQUIVOS} |
| Cards ASCII Adicionados | ~{TOTAL_ARQUIVOS} |
| Try-Catch Implementados | ~{TOTAL_ARQUIVOS * 3} (estimativa) |
| Comentários [DOC] | ~{TOTAL_ARQUIVOS * 5} (estimativa) |
| Linhas Documentadas | ~{TOTAL_ARQUIVOS * 50} (estimativa) |

---

## ✅ Padrões Aplicados

- ✅ Card ASCII no topo de cada arquivo
- ✅ Card de documentação para Controllers/Classes
- ✅ Card para cada método público
- ✅ Comentários [DOC] em lógica complexa
- ✅ Try-catch com TempData["Erro"] (C#)
- ✅ Try-catch com alerta.erro() (JavaScript)
- ✅ DocumentacaoIntracodigo.md atualizado

---

## 📂 Diretórios Documentados

- ✅ Analises
- ✅ Areas/Authorization
- ✅ Areas/Identity
- ✅ Controllers
- ✅ Data
- ✅ EndPoints
- ✅ Extensions
- ✅ Filters
- ✅ Helpers
- ✅ Hubs
- ✅ Infrastructure
- ✅ Logging
- ✅ Middlewares
- ✅ Models
- ✅ Pages
- ✅ Properties
- ✅ Repository
- ✅ Services
- ✅ Settings
- ✅ Tools

---

## 🎯 Commits Realizados

```
{commits_log}
```

---

## 📝 Arquivo de Log Completo

Consulte: `FrotiX.Site/DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md`

---

**Processo supervisionado automaticamente por**: `supervisor-inteligente.py`
**Arquiteto Responsável**: Claude Sonnet 4.5
**Status**: ✅ CONCLUÍDO
"""

    with open(arquivo, 'w', encoding='utf-8') as f:
        f.write(content)

    print(f"{Color.GREEN}✅ Arquivo de conclusão criado: {arquivo}{Color.RESET}")

    # Commitar arquivo de conclusão
    try:
        subprocess.run(['git', 'add', arquivo], check=True)
        subprocess.run(['git', 'commit', '-m',
                       f'docs: processo de documentação FrotiX concluído\n\n'
                       f'🎉 100% dos arquivos documentados ({TOTAL_ARQUIVOS}/{TOTAL_ARQUIVOS})\n\n'
                       f'Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>'],
                      check=True)
        subprocess.run(['git', 'push'], check=False)
    except Exception as e:
        print(f"{Color.YELLOW}⚠️  Erro ao commitar arquivo de conclusão: {e}{Color.RESET}")

def gerar_prompt_lote(arquivos: List[str]) -> str:
    """Gera prompt específico para documentar um lote de arquivos"""
    arquivos_str = "\n".join([f"- {a}" for a in arquivos])

    prompt = f"""Documentar o próximo lote de {len(arquivos)} Controllers do FrotiX.

Arquivos a documentar (em ordem):
{arquivos_str}

Para cada arquivo:
1. Adicionar card ASCII no topo
2. Adicionar card de Controller
3. Adicionar card para cada método público
4. Adicionar comentários [DOC] em lógica complexa
5. Adicionar try-catch onde faltar (com TempData["Erro"])
6. Atualizar DocumentacaoIntracodigo.md marcando como [x]

Padrão de documentação:
- Card ASCII: ╔══════╗ com emojis 🎯📥📤🔗🔄📦
- Try-catch C#: TempData["Erro"] = $"Erro: {{ex.Message}}"
- Comentários: // [DOC] Explicação clara
- Atualizar contadores em DocumentacaoIntracodigo.md

Não fazer commit - apenas documentar."""

    return prompt

def mostrar_estatisticas(atual: int, percentual: float):
    """Mostra estatísticas de progresso"""
    proximo = proximo_marco(percentual)
    arquivos_para_proximo = int((proximo * TOTAL_ARQUIVOS / 100) - atual)

    print(f"\n{Color.BLUE}{'═' * 60}{Color.RESET}")
    print(f"{Color.BOLD}  📊 ESTATÍSTICAS DE PROGRESSO{Color.RESET}")
    print(f"{Color.BLUE}{'═' * 60}{Color.RESET}")
    print(f"  Arquivos documentados: {Color.GREEN}{atual}{Color.RESET}/{TOTAL_ARQUIVOS}")
    print(f"  Progresso atual: {Color.GREEN}{percentual}%{Color.RESET}")
    print(f"  Próximo marco: {Color.YELLOW}{proximo}%{Color.RESET}")
    print(f"  Faltam para próximo marco: {Color.YELLOW}{arquivos_para_proximo}{Color.RESET} arquivos")
    print(f"{Color.BLUE}{'═' * 60}{Color.RESET}\n")

def supervisionar():
    """Função principal de supervisão"""
    print(f"\n{Color.CYAN}{'═' * 60}{Color.RESET}")
    print(f"{Color.BOLD}{Color.CYAN}  🚀 SUPERVISOR INTELIGENTE - DOCUMENTAÇÃO FROTIX{Color.RESET}")
    print(f"{Color.CYAN}{'═' * 60}{Color.RESET}\n")

    ultimo_commit_percentual = 10.0  # Último commit foi em 10%
    iteracao = 1

    while True:
        print(f"\n{Color.MAGENTA}{'─' * 60}{Color.RESET}")
        print(f"{Color.MAGENTA}Iteração #{iteracao} - {datetime.now().strftime('%H:%M:%S')}{Color.RESET}")
        print(f"{Color.MAGENTA}{'─' * 60}{Color.RESET}")

        # Contar progresso atual
        atual = contar_documentados()
        percentual = calcular_percentual(atual)

        # Mostrar estatísticas
        mostrar_estatisticas(atual, percentual)

        # Verificar se completou 100%
        if atual >= TOTAL_ARQUIVOS:
            print(f"\n{Color.GREEN}{'═' * 60}{Color.RESET}")
            print(f"{Color.GREEN}{Color.BOLD}  🎉 DOCUMENTAÇÃO 100% CONCLUÍDA!{Color.RESET}")
            print(f"{Color.GREEN}{'═' * 60}{Color.RESET}\n")

            # Commit final se necessário
            if ultimo_commit_percentual < 100:
                commit_marco(100, atual)

            # Criar arquivo de conclusão
            criar_arquivo_conclusao()

            print(f"\n{Color.GREEN}✅ Processo finalizado com sucesso!{Color.RESET}")
            print(f"{Color.GREEN}✅ Arquivo FimProcesso criado{Color.RESET}\n")
            break

        # Verificar se atingiu um marco
        marco = atingiu_marco(percentual, ultimo_commit_percentual)
        if marco > 0:
            commit_marco(marco, atual)
            ultimo_commit_percentual = float(marco)

        # Listar próximos arquivos pendentes
        pendentes = listar_controllers_pendentes()

        if len(pendentes) == 0:
            print(f"{Color.YELLOW}⚠️  Nenhum Controller pendente encontrado.{Color.RESET}")
            print(f"{Color.YELLOW}   Pode estar processando outros diretórios.{Color.RESET}")
            print(f"{Color.YELLOW}   Aguardando 30 segundos...{Color.RESET}")
            time.sleep(30)
            iteracao += 1
            continue

        # Determinar próximo lote
        proximo_lote = pendentes[:TAMANHO_LOTE]
        print(f"\n{Color.CYAN}📋 Próximo lote ({len(proximo_lote)} arquivos):{Color.RESET}")
        for i, arquivo in enumerate(proximo_lote, 1):
            print(f"  {i}. {arquivo}")

        # Gerar prompt
        prompt = gerar_prompt_lote(proximo_lote)

        print(f"\n{Color.YELLOW}📝 Prompt gerado (pronto para copiar):{Color.RESET}")
        print(f"{Color.BLUE}{'─' * 60}{Color.RESET}")
        print(prompt)
        print(f"{Color.BLUE}{'─' * 60}{Color.RESET}")

        print(f"\n{Color.YELLOW}⏳ Aguarde o agente processar este lote...{Color.RESET}")
        print(f"{Color.YELLOW}   Verificando novamente em 60 segundos...{Color.RESET}")

        # Aguardar antes da próxima verificação
        time.sleep(60)
        iteracao += 1

def main():
    """Função principal"""
    # Verificar se está no diretório correto
    if not os.path.exists(DOC_FILE):
        print(f"{Color.RED}❌ Erro: Arquivo {DOC_FILE} não encontrado!{Color.RESET}")
        print(f"{Color.RED}   Execute este script no diretório raiz do projeto FrotiX{Color.RESET}")
        return 1

    try:
        supervisionar()
        return 0
    except KeyboardInterrupt:
        print(f"\n\n{Color.YELLOW}⚠️  Supervisão interrompida pelo usuário{Color.RESET}")
        return 0
    except Exception as e:
        print(f"\n{Color.RED}❌ Erro fatal: {e}{Color.RESET}")
        import traceback
        traceback.print_exc()
        return 1

if __name__ == "__main__":
    exit(main())
