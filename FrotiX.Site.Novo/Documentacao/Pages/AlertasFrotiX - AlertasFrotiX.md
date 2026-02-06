# Documentação: AlertasFrotiX - AlertasFrotiX

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
├── Pages/AlertasFrotiX/AlertasFrotiX.cshtml
├── Pages/AlertasFrotiX/AlertasFrotiX.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `AlertasFrotiX`
- **Página**: `AlertasFrotiX`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.AlertasFrotiX.IndexModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (0):
- **JS** (2):
  - `~/js/alertas/alertas_gestao.js`
  - `~/lib/microsoft-signalr/signalr.min.js`

### Observações detectadas
- Contém `@section ScriptsBlock`.
- Contém `@section HeadBlock`.
- Possível uso de DataTables (detectado por string).

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

## [13/01/2026 18:00] - Fase 3: Padronização btn-secondary → btn-vinho (CSS)

**Descrição**: Atualizados seletores CSS de `.btn-secondary` para `.btn-vinho` em estilos de modal.

**Problema Identificado**:
- Seletores CSS customizando `.btn-secondary` em modal de detalhes de alerta
- Inconsistência com padrão FrotiX que define `btn-vinho` para botões de fechar
- CSS aplicado a classe genérica Bootstrap em vez de classe FrotiX oficial

**Solução Implementada**:
- Linhas 282-292: Seletores CSS mudados de `#modalDetalhesAlerta .btn-secondary` para `#modalDetalhesAlerta .btn-vinho`
- Cores ajustadas para paleta vinho FrotiX (#722f37, #5a252c, #4a1f24)
- Alinhamento com diretrizes FrotiX: estilos customizados devem usar classes FrotiX

**Arquivos Afetados**:
- Pages/AlertasFrotiX/AlertasFrotiX.cshtml (linhas 282-292 - seção CSS)

**Impacto**:
- ✅ CSS aplicado à classe correta (btn-vinho)
- ✅ Consistência com outras correções da Fase 3
- ✅ Paleta de cores FrotiX respeitada

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.2

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:
- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**
