# Documentação: JavaScript - upsert_autuacao.js

> **Última Atualização**: 16/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO TÉCNICA

## Visão Geral

Script responsável pela interatividade, formatação e validação do formulário de Multas (Autuações).

## Funcionalidades Principais

### 1. Formatação de Texto (Title Case)

- Aplica Title Case automaticamente nos campos de Número da Infração e Localização.
- Garante que a primeira letra de cada palavra seja maiúscula.

### 2. Validação Chronológica de Datas

- Função `validarOrdemDatas(campoId)` garante que:
  - Data da Infração < Data da Notificação
  - Data da Notificação < Data Limite de Defesa
  - Data Limite de Defesa < Data do Pagamento (se houver)

### 3. Integração com Alertas FrotiX

- Utiliza a biblioteca `Alerta` para exibir mensagens de erro padronizadas.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

---

## [16/01/2026 17:20] - Criação e Refatoração de Validações

**Descrição**: Refatoração completa para suportar as novas regras de negócio do módulo de Multas.

**Mudanças**:

- Implementação de Title Case para campos de texto.
- Implementação de lógica de datas encadeadas.
- Migração para o padrão de alertas FrotiX.


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
