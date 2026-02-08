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
