# Documentação: Viagens - GestaoFluxo

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
├── Pages/Viagens/GestaoFluxo.cshtml
├── Pages/Viagens/GestaoFluxo.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Viagens`
- **Página**: `GestaoFluxo`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.Viagens.GestaoFluxoModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (1):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css`
- **JS** (1):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js`

### Observações detectadas
- Contém `@section ScriptsBlock`.
- Contém `@section HeadBlock`.
- Possível uso de DataTables (detectado por string).
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

**Descrição**: Substituída classe Bootstrap genérica `btn-secondary` por `btn-vinho` (padrão FrotiX oficial) em botão de limpar filtros.

**Problema Identificado**:
- Uso de classe Bootstrap genérica `btn-secondary` em botão de limpar filtros
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de cancelar/limpar
- Falta de padronização visual em todo o sistema

**Solução Implementada**:
- Botão "Limpar" filtros (linha 657): mudado de `btn-secondary` para `btn-vinho`
- Limpar filtros é semanticamente uma ação de "cancelar/resetar" estado
- Alinhamento com diretrizes FrotiX: ações de cancelar/limpar usam `btn-vinho`
- Consistência com outras 8 correções aplicadas em todo o sistema (Fase 3)

**Arquivos Afetados**:
- Pages/Viagens/GestaoFluxo.cshtml (linha 657)

**Impacto**:
- ✅ Botão mantém cor vinho consistente ao pressionar
- ✅ Semântica visual correta (limpar = cancelar filtros)
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
