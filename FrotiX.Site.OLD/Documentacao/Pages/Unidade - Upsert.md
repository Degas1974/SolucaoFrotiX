# Documentação: Unidade - Upsert (Criação e Edição)

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura da Interface](#estrutura-da-interface)
4. [Lógica de Frontend (JavaScript)](#lógica-de-frontend-javascript)
5. [Endpoints API](#endpoints-api)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página de **Upsert de Unidade** (`Pages/Unidade/Upsert.cshtml`) permite cadastrar ou editar unidades usuárias. O formulário é detalhado, permitindo registrar até três contatos diferentes para a mesma unidade, além de sua categorização.

### Características Principais

- ✅ **Múltiplos Contatos**: Campos para Nome, Ponto e Ramal repetidos para 3 contatos.
- ✅ **Categorização**: Dropdown para definir a categoria (Presidência, DG, etc.).
- ✅ **Capacidade**: Campo numérico para quantidade de motoristas.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Unidade/
│       └── Upsert.cshtml            # View do Formulário
│
├── Controllers/
│   └── UnidadeController.cs         # Controller (Submit)
```

### Tecnologias Utilizadas

| Tecnologia                   | Uso          |
| ---------------------------- | ------------ |
| **ASP.NET Core Razor Pages** | Renderização |
| **Bootstrap 5**              | Layout       |

---

## Estrutura da Interface

O formulário é organizado em linhas (Rows) usando o grid do Bootstrap.

```html
<div class="row">
  <div class="col-2">
    <label>Sigla</label>
    <input asp-for="UnidadeObj.Sigla" />
  </div>
  <div class="col-5">
    <label>Descrição</label>
    <input asp-for="UnidadeObj.Descricao" />
  </div>
  <!-- ... -->
</div>

<!-- Contato 1 -->
<div class="row">
  <div class="col-6">
    <label>Contato</label>
    <input asp-for="UnidadeObj.PrimeiroContato" />
  </div>
  <!-- ... -->
</div>
```

---

## Lógica de Frontend (JavaScript)

Esta página utiliza principalmente HTML/Razor nativo com validação via jQuery Validate Unobtrusive (padrão ASP.NET Core). Não há scripts complexos dedicados.

---

## Endpoints API

### POST `/Unidade/Upsert` (Handler)

Processa o formulário. O objeto `UnidadeObj` é preenchido automaticamente.

---

## Troubleshooting

### Erro "Sigla já existe"

**Causa**: Validação de duplicidade no banco de dados (provavelmente no Controller ou Service).
**Solução**: Utilize uma sigla única.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026 17:15] - Auditoria Global: Campos Obrigatórios (.label-required)

**Descrição**: Adicionado asterisco vermelho em labels de campos mandatórios identificados via lógica de validação (Back/Front).

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

- Pages/Unidade/Upsert.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

**Impacto**:

- ✅ Botão mantém cor rosada/vinho ao ser pressionado
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.1

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial do formulário de Upsert de Unidades.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0
