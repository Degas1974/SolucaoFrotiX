# Documentação: Multa - UpsertPenalidade

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
├── Pages/Multa/UpsertPenalidade.cshtml
├── Pages/Multa/UpsertPenalidade.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Multa`
- **Página**: `UpsertPenalidade`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.Multa.UpsertPenalidadeModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (2):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css`
  - `~/css/ftx-card-styled.css`
- **JS** (2):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js`
  - `~/js/cadastros/upsert_penalidade.js`

### Observações detectadas

- Contém `@section ScriptsBlock`.
- Contém `@section HeadBlock`.
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

- Pages/Multa/UpsertPenalidade.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

**Impacto**:

- ✅ Botão mantém cor rosada/vinho ao ser pressionado
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.2

---

## [12/01/2026 16:25] - Adicionado botão Voltar à Lista

**Descrição**: Adicionado botão "Voltar à Lista" no header da página, seguindo padrão FrotiX.

**Alteração no Header** (linhas 309-321):

```html
<div class="ftx-card-header d-flex justify-content-between align-items-center">
  <h2 class="titulo-paginas mb-0">
    <i class="fa-duotone fa-gavel"></i>
    Criar/Atualizar Penalidade
  </h2>
  <div class="ftx-card-actions">
    <a
      asp-page="/Multa/ListaPenalidade"
      class="btn btn-header-orange"
      data-ftx-loading
    >
      <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i>
      Voltar à Lista
    </a>
  </div>
</div>
```

**Arquivos Afetados**:

- `Pages/Multa/UpsertPenalidade.cshtml` (linhas 309-321)

**Impacto**: Melhoria de navegação com botão laranja no header

**Status**: ✅ **Concluído**

**Versão**: 0.2

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:

- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**
