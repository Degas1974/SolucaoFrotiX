# Documentação: Viagens - UpsertEvento

> **Última Atualização**: 16/01/2026
> **Versão Atual**: 0.9

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
├── Pages/Viagens/UpsertEvento.cshtml
├── Pages/Viagens/UpsertEvento.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Viagens`
- **Página**: `UpsertEvento`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.Viagens.UpsertEventoModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (2):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css`
  - `~/css/ftx-card-styled.css`
- **JS** (2):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js`
  - `~/js/cadastros/eventoupsert.js`

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

## [16/01/2026] - Aplicação de Trim + NaturalStringComparer em método de Requisitantes

**Descrição**:
Aplicado padrão de Trim + NaturalStringComparer em 1 método que carrega lista de requisitantes:
- `PreencheListaRequisitantes()` - Método de preenchimento no OnGet

**Padrão Aplicado**:
1. Query banco sem orderBy (melhor performance)
2. `.ToList()` para materializar em memória
3. `.Select()` com `.Trim()` para remover espaços iniciais/finais
4. `.OrderBy()` com `NaturalStringComparer` (números antes de letras, case-insensitive, pt-BR)

**Motivo**:
- Remover espaços em branco que causam desordenação alfabética
- Garantir ordenação natural (001, 002, 003, ..., A, B, C)
- Consistência com padrão aplicado em todo o sistema

**Arquivos Afetados**:
- `Pages/Viagens/UpsertEvento.cshtml.cs` (1 método)

**Status**: ✅ **Concluído**

**Versão**: 0.9

---

## [14/01/2026 01:00] - Reforço do CSS de borda para lstRequisitanteEvento

**Descrição**:

- Ampliado o seletor CSS para cobrir todas as estruturas possíveis do Syncfusion DropdownTree
- Adicionados seletores para `.e-control`, `.e-ddt`, `.e-ddt-wrapper` e variações
- Evita borda dupla removendo borda dos elementos internos (`.e-input`)

**Arquivos Afetados**:

- `Pages/Viagens/UpsertEvento.cshtml` (CSS linhas 265-283)

**Status**: ✅ **Concluído**

**Versão**: 0.8

---

## [14/01/2026 00:55] - Botão Confirmar Desassociação alterado para btn-azul

**Descrição**:

- Botão "Confirmar Desassociação" alterado de `btn-vinho` para `btn-azul`
- Padrão FrotiX: ações de confirmação usam azul, ações de cancelar/fechar usam vinho

**Arquivos Afetados**:

- `Pages/Viagens/UpsertEvento.cshtml` (linha 1101)

**Status**: ✅ **Concluído**

**Versão**: 0.7

---

## [14/01/2026 00:50] - Correções no Modal de Desassociação de Viagem

**Descrição**:

- **lstNovaFinalidade**: Adicionado ao CSS fix para não cortar o item selecionado (mesmo problema do lstStatus)
- **Botão Cancelar**: Substituído `fa-xmark` por `fa-circle-xmark` (X com círculo), conforme padrão FrotiX

**Arquivos Afetados**:

- `Pages/Viagens/UpsertEvento.cshtml` (CSS linha 274, botão linha 1098)

**Impacto**: Visual - campo de finalidade e botão cancelar agora exibem corretamente

**Status**: ✅ **Concluído**

**Versão**: 0.6

---

## [14/01/2026 00:45] - Correção visual dos campos Requisitante e Status

**Descrição**:

- **lstRequisitanteEvento**: Adicionado CSS para garantir bordas superior e inferior visíveis no DropdownTree Syncfusion
- **lstStatus**: Removido `max-height: 40px` que estava cortando o item selecionado ao meio; agora usa `height: auto` com `min-height: 40px`

**Problema Identificado**:

- Lista de Requisitante sem borda superior/inferior (conflito entre CSS genérico e Syncfusion)
- Lista de Status cortando metade do item selecionado (max-height forçado)

**Arquivos Afetados**:

- `Pages/Viagens/UpsertEvento.cshtml` (CSS inline, linhas 263-280)

**Impacto**: Visual - campos de seleção agora exibem corretamente

**Status**: ✅ **Concluído**

**Versão**: 0.5

---

## [14/01/2026 00:30] - Correção do ícone do botão Fechar no Modal de Detalhamento de Custos

**Descrição**:

- Corrigido ícone do botão "Fechar" no Modal de Detalhamento de Custos
- Substituído `fa-rotate-left` (ícone de voltar) por `fa-circle-xmark` (X com círculo)
- Padrão FrotiX: botões de fechar modal usam X envolto no círculo

**Arquivos Afetados**:

- `Pages/Viagens/UpsertEvento.cshtml` (linha 1004)

**Impacto**: Visual - padronização de ícones em modais

**Status**: ✅ **Concluído**

**Versão**: 0.4

---

## [13/01/2026 18:00] - Fase 3: Padronização btn-secondary → btn-vinho

**Descrição**: Substituída classe Bootstrap genérica `btn-secondary` por `btn-vinho` (padrão FrotiX oficial) em botão de modal.

**Problema Identificado**:
- Uso de classe Bootstrap genérica `btn-secondary` em modal de cancelar
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de fechar/cancelar
- Falta de padronização visual em todo o sistema

**Solução Implementada**:
- Modal "Desassociar Evento" (linha 1077): botão "Cancelar" mudado de `btn-secondary` para `btn-vinho`
- Alinhamento com diretrizes FrotiX: botões de fechar/cancelar SEMPRE usam `btn-vinho`
- Consistência com outras 8 correções aplicadas em todo o sistema (Fase 3)

**Arquivos Afetados**:
- Pages/Viagens/UpsertEvento.cshtml (linha 1077)

**Impacto**:
- ✅ Botão mantém cor vinho consistente ao pressionar
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.3

---

## [13/01/2026 15:30] - Padronização: Substituição de btn-ftx-fechar por btn-vinho

**Descrição**: Substituída classe `btn-ftx-fechar` por `btn-vinho` em botões de cancelar/fechar operação.

**Problema Identificado**:
- Classe `btn-ftx-fechar` não tinha `background-color` definido no estado `:active`
- Botões ficavam BRANCOS ao serem pressionados (em vez de manter cor rosada/vinho)
- Comportamento visual inconsistente com padrão FrotiX

**Solução Implementada**:
- Todos os botões cancelar/fechar padronizados para usar classe `.btn-vinho`
- Classe `.btn-vinho` já possui `background-color: #4a1f24` no estado `:active`
- Garantia de cor rosada/vinho ao pressionar botão

**Arquivos Afetados**:
- Pages/Viagens/UpsertEvento.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

**Impacto**:
- ✅ Botão mantém cor rosada/vinho ao ser pressionado
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
