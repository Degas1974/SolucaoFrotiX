# Documentação: Manutencao - ControleLavagem

> **Última Atualização**: 14/01/2026
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
├── Pages/Manutencao/ControleLavagem.cshtml
├── Pages/Manutencao/ControleLavagem.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Manutencao`
- **Página**: `ControleLavagem`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Models.ViewViagens`

---

## Frontend

### Assets referenciados na página

- **CSS** (2):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css`
  - `https://kendo.cdn.telerik.com/2022.3.913/styles/kendo.default-ocean-blue.min.css`
- **JS** (4):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js`
  - `https://kendo.cdn.telerik.com/2022.1.412/js/jszip.min.js`
  - `https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js`
  - `https://kendo.cdn.telerik.com/2022.1.412/js/kendo.aspnetmvc.min.js`

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

## [14/01/2026 17:45] - Implementação do Modal de Espera Padrão FrotiX

**Descrição**:

- Implementado o modal de espera padrão FrotiX (`FtxSpin`) para exibição durante carregamento de dados
- Adicionado `FtxSpin.show("Carregando Lavagens")` nos seguintes momentos:
  - Carregamento inicial da página (`ListaTodasLavagens()`)
  - Mudança de data no filtro (`#txtData.change`)
  - Seleção de veículo (`VeiculosValueChange()`)
  - Seleção de motorista (`MotoristaValueChange()`)
  - Seleção de lavador (`LavadorValueChange()`)
- Adicionado `FtxSpin.hide()` no callback `drawCallback` do DataTable para esconder o modal ao terminar
- Adicionado tratamento de erro no ajax do DataTable para esconder o modal em caso de falha
- Removido código legado do plugin `LoadingScript`
- Atualizado subtexto padrão do `FtxSpin` global (`frotix.js`) para "Por favor, aguarde..."

**Arquivos Afetados**:

- `Pages/Manutencao/ControleLavagem.cshtml` - Implementação do FtxSpin
- `wwwroot/js/frotix.js` - Alterado subtexto padrão

**Impacto**:

- Modal de espera consistente com o padrão visual FrotiX (logo pulsando + barra de progresso)
- Feedback visual ao usuário durante carregamento de dados

**Status**: ✅ **Concluído**

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:
- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**
