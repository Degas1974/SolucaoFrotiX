# Documentação: Contrato - ItensContrato

> **Última Atualização**: 15/01/2026
> **Versão Atual**: 0.4

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
├── Pages/Contrato/ItensContrato.cshtml
├── Pages/Contrato/ItensContrato.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Contrato`
- **Página**: `ItensContrato`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.Contrato.ItensContratoModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (0):
- **JS** (1):
  - `~/js/cadastros/itenscontrato.js`

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

## [15/01/2026 14:30] - Toggle de Status para equipes terceirizadas

**Descrição**: Os badges de status de Encarregados, Operadores, Motoristas e Lavadores passaram a ser clicáveis e agora alternam entre Ativo/Inativo via endpoints dedicados. O visual foi alinhado com os botões de status padrão já usados em Veículos (classes `btn btn-verde` e `fundo-cinza`).

**Problema Identificado**:
- Status das equipes terceirizadas era apenas informativo (sem clique)
- Badges de status não utilizavam o mesmo padrão visual dos demais itens

**Solução Implementada**:
- Adicionados handlers de clique no `itenscontrato.js` para cada tipo
- Render dos badges atualizado com links e classes padrão
- Integração direta com os endpoints `UpdateStatus*` de cada cadastro

**Arquivos Afetados**:
- `wwwroot/js/cadastros/itenscontrato.js`

**Impacto**:
- ✅ Alternância rápida de status sem sair da página
- ✅ Consistência visual com o padrão FrotiX

**Status**: ✅ **Concluído**

**Responsável**: GitHub Copilot

**Versão**: 0.4

## [13/01/2026 18:00] - Fase 3: Padronização btn-secondary → btn-vinho

**Descrição**: Substituída classe Bootstrap genérica `btn-secondary` por `btn-vinho` (padrão FrotiX oficial) em 5 modais CRUD.

**Problema Identificado**:
- Uso de classe Bootstrap genérica `btn-secondary` em botões de cancelar de modais
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de fechar/cancelar
- Falta de padronização visual em todo o sistema

**Solução Implementada**:
- Modal "Adicionar Veículo" (linha 949): botão "Cancelar" mudado de `btn-secondary` para `btn-vinho`
- Modal "Adicionar Encarregado" (linha 979): botão "Cancelar" mudado de `btn-secondary` para `btn-vinho`
- Modal "Adicionar Operador" (linha 1009): botão "Cancelar" mudado de `btn-secondary` para `btn-vinho`
- Modal "Adicionar Motorista" (linha 1039): botão "Cancelar" mudado de `btn-secondary` para `btn-vinho`
- Modal "Adicionar Lavador" (linha 1069): botão "Cancelar" mudado de `btn-secondary` para `btn-vinho`
- Alinhamento com diretrizes FrotiX: botões de fechar/cancelar SEMPRE usam `btn-vinho`
- Consistência com outras 4 correções aplicadas em todo o sistema (Fase 3)

**Arquivos Afetados**:
- Pages/Contrato/ItensContrato.cshtml (linhas 949, 979, 1009, 1039, 1069)

**Impacto**:
- ✅ Botões mantêm cor vinho consistente ao pressionar
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema
- ✅ 5 modais CRUD padronizados

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.3

---

## [13/01/2026 17:15] - Padronização: Correção btn-verde → btn-azul

**Descrição**: Corrigidos botões que usavam `btn-verde` incorretamente para ações neutras de salvar/gravar.

**Problema Identificado**:
- Botões de "Salvar/Gravar" em modais CRUD estavam usando `btn-verde`
- `btn-verde` deve ser usado apenas para ações positivas (Importar, Processar, Aprovar, Aplicar)
- Botões de salvar genérico devem usar `btn-azul` (ações neutras)

**Solução Implementada**:
- Substituídas classes `btn-verde` por `btn-azul` em botões de salvar/gravar
- Alinhamento com diretrizes oficiais FrotiX (v2.7)

**Arquivos Afetados**:
- `Pages/Contrato/ItensContrato.cshtml` (linhas 952, 982, 1012, 1042, 1072)
  - btnSalvarVeiculo: `btn-verde` → `btn-azul`
  - btnSalvarEncarregado: `btn-verde` → `btn-azul`
  - btnSalvarOperador: `btn-verde` → `btn-azul`
  - btnSalvarMotorista: `btn-verde` → `btn-azul`
  - btnSalvarLavador: `btn-verde` → `btn-azul`

**Diretrizes FrotiX:**
- ✅ `btn-azul` para: Salvar/Editar/Inserir/Atualizar ações neutras
- ✅ `btn-verde` para: Importar/Processar/Aprovar/Aplicar/Confirmar ações positivas
- ✅ `btn-vinho` para: Cancelar/Fechar/Excluir

**Impacto**:
- ✅ Uso consistente de cores conforme tipo de ação
- ✅ Sistema 100% alinhado com padrão FrotiX

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.1 → 0.2

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:
- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**
