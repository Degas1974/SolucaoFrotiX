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
