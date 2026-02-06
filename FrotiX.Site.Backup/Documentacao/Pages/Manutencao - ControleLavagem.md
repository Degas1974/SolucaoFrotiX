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


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

## [21/01/2026] - PadronizaÃ§Ã£o de Nomenclatura

**DescriÃ§Ã£o**: Renomeada coluna "AÃ§Ã£o" para "AÃ§Ãµes" no cabeÃ§alho do DataTable para padronizaÃ§Ã£o do sistema

**Arquivos Afetados**:
- Arquivo .cshtml correspondente

**Impacto**: AlteraÃ§Ã£o cosmÃ©tica, sem impacto funcional

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema

**VersÃ£o**: Atual

---

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
