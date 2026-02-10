# Documentação: Manutencao - ControleLavagem

> **Última Atualização**: 03/02/2026
> **Versão Atual**: 0.3

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

Tela responsável por registrar lavagens de veículos e filtrar lavagens por veículo, motorista, lavador e data.

### Características Principais
- ✅ Registro de lavagem com data + hora única (sem hora fim)
- ✅ Filtros por veículo, motorista, lavador e data

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

**Principais endpoints consumidos:**
- `GET /api/Manutencao/ListaLavagens`
- `GET /api/Manutencao/ListaLavagemVeiculos`
- `GET /api/Manutencao/ListaLavagemMotoristas`
- `GET /api/Manutencao/ListaLavagemLavadores`
- `POST /api/Manutencao/InsereLavagem`
- `POST /api/Manutencao/InsereLavadoresLavagem`
- `POST /api/Manutencao/ApagaLavagem`

**Payload de inserção (InsereLavagem):**
```json
{
  "Data": "2026-02-03",
  "HorarioLavagem": "08:30",
  "VeiculoId": "GUID",
  "MotoristaId": "GUID"
}
```

---

## Validações

**Frontend (JS):**
- Data obrigatória (`#txtDataLavagem`)
- Hora da lavagem obrigatória (`#txtHoraLavagem`)
- Veículo obrigatório (`#lstVeiculoLavagem`)
- Motorista obrigatório (`#lstMotoristaLavagem`)
- Ao menos 1 lavador obrigatório (`#lstLavadores`)

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

## [03/02/2026 10:00] - Hora Única da Lavagem

**Descrição**:
- Removido campo de Hora Fim (UI e validações)
- Unificado horário da lavagem em `HorarioLavagem`
- DataTable passa a consumir campo `Horario`

**Arquivos Afetados**:

- `Pages/Manutencao/ControleLavagem.cshtml`
- `Controllers/ManutencaoController.cs`
- `Models/Cadastros/Lavagem.cs`
- `Models/Views/ViewLavagem.cs`

**Impacto**:

- Registro simplificado de lavagens
- Correção de erro no DataTable (campo `Horario`)

**Status**: ✅ **Concluído**

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:
- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**
