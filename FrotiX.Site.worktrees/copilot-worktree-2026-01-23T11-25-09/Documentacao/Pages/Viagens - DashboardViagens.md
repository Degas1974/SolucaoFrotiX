# Documentação: Viagens - DashboardViagens

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 0.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Frontend](#frontend)
4. [Endpoints API](#endpoints-api)
5. [Validações](#validações)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

> **TODO**: Descrever o objetivo da página e as principais ações do usuário.

### Características Principais
- ✅ **TODO**

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/Viagens/DashboardViagens.cshtml
├── Pages/Viagens/DashboardViagens.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Viagens`
- **Página**: `DashboardViagens`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.Viagens.DashboardViagensModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (0):
- **JS** (9):
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-base/dist/global/ej2-base.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-charts/dist/global/ej2-charts.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-compression/dist/global/ej2-compression.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-data/dist/global/ej2-data.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-file-utils/dist/global/ej2-file-utils.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-pdf-export/dist/global/ej2-pdf-export.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-svg-base/dist/global/ej2-svg-base.min.js`
  - `https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js`
  - `~/js/dashboards/dashboard-viagens.js`

### Observações detectadas
- Contém `@section ScriptsBlock`.
- Possível uso de componentes Syncfusion EJ2 (detectado por tags `ejs-*`).

---

## Endpoints API

> **TODO**: Listar endpoints consumidos pela página e incluir trechos reais de código do Controller/Handler quando aplicável.

---

## Validações

> **TODO**: Listar validações do frontend e backend (com trechos reais do código).

---

## Troubleshooting

> **TODO**: Problemas comuns, sintomas, causa e solução.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026 18:00] - Fase 3: Padronização btn-secondary → btn-vinho

**Descrição**: Substituída classe Bootstrap genérica `btn-secondary` por `btn-vinho` (padrão FrotiX oficial) em modal de detalhes.

**Problema Identificado**:
- Uso de classe Bootstrap genérica `btn-secondary` em modal de fechar
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de fechar/cancelar
- Falta de padronização visual em todo o sistema

**Solução Implementada**:
- Modal "Detalhes da Viagem" (linha 965): botão "Fechar" mudado de `btn-secondary` para `btn-vinho`
- Alinhamento com diretrizes FrotiX: botões de fechar/cancelar SEMPRE usam `btn-vinho`
- Consistência com outras 8 correções aplicadas em todo o sistema (Fase 3)

**Arquivos Afetados**:
- Pages/Viagens/DashboardViagens.cshtml (linha 965)

**Impacto**:
- ✅ Botão mantém cor vinho consistente ao pressionar
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.2

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:
- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**


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
