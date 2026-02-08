# Documentação: Requisitante - Upsert (Criação e Edição)

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

A página de **Upsert de Requisitante** (`Pages/Requisitante/Upsert.cshtml`) permite cadastrar novos requisitantes ou editar os existentes. O ponto principal é a seleção do **Setor Solicitante**, que utiliza um componente de árvore hierárquica.

### Características Principais
- ✅ **Formulário Simples**: Campos de identificação e contato.
- ✅ **Seleção Hierárquica**: Componente `Dropdowntree` para escolher o setor na estrutura organizacional.
- ✅ **Status**: Checkbox simples para ativar/desativar.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Requisitante/
│       └── Upsert.cshtml            # View do Formulário
│
├── Controllers/
│   └── RequisitanteController.cs    # Controller (Submit)
```

### Tecnologias Utilizadas
| Tecnologia | Uso |
|------------|-----|
| **ASP.NET Core Razor Pages** | Renderização |
| **Syncfusion Dropdowntree** | Seleção de Setor |
| **Bootstrap 5** | Layout |

---

## Estrutura da Interface

### Componente de Setor (Syncfusion)
O `ejs-dropdowntree` é configurado para consumir uma fonte de dados hierárquica (Id, Texto, ParentId).

```html
<ejs-dropdowntree id="ddtree"
                  ejs-for="@Model.RequisitanteObj.Requisitante.SetorSolicitanteId">
    <e-dropdowntree-fields dataSource="@ViewData["dataSource"]"
                           value="SetorSolicitanteId"
                           text="Nome"
                           parentValue="SetorPaiId"
                           hasChildren="HasChild">
    </e-dropdowntree-fields>
</ejs-dropdowntree>
```

---

## Lógica de Frontend (JavaScript)

Script inline para pré-selecionar o valor no componente Syncfusion durante a edição (devido a peculiaridades de renderização do componente).

```javascript
$(document).ready(function () {
    var setorId = "@Model.RequisitanteObj.Requisitante.SetorSolicitanteId";
    // Se existir setor e não for GUID vazio
    if (setorId && setorId !== "00000000-0000-0000-0000-000000000000") {
        // Define o valor no componente
        document.getElementById("ddtree").ej2_instances[0].value = [setorId];
    }
});
```

---

## Endpoints API

### POST `/Requisitante/Upsert` (Handler)
Processa o formulário. O `RequisitanteObj` é populado automaticamente pelo Model Binding do ASP.NET Core.

---

## Troubleshooting

### Árvore de setores não carrega
**Causa**: `ViewData["dataSource"]` nulo ou formato de dados incorreto (falta de `HasChild`).
**Solução**: Verifique o método `OnGet` e a query que popula a lista de setores.

### Valor do setor não salva
**Causa**: O componente `dropdowntree` pode não estar bindado corretamente ao input hidden do form.
**Solução**: O atributo `ejs-for` deve gerar o `name` correto (`RequisitanteObj.Requisitante.SetorSolicitanteId`).

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
- Pages/Requisitante/Upsert.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

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
Documentação inicial do formulário de Upsert de Requisitantes.

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
