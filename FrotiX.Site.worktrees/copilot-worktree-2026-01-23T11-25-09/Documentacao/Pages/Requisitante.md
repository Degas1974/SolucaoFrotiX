# Documentação: Requisitante (Funcionalidade)

> **Última Atualização**: 06/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Listagem (Index)](#listagem-index)
4. [Cadastro/Edição (Upsert)](#cadastroedicao-upsert)
5. [Endpoints API](#endpoints-api)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

O módulo de **Requisitantes** gerencia os usuários ou colaboradores que podem solicitar viagens ou serviços no sistema. Eles são vinculados a um **Setor** (Centro de Custo).

### Características Principais
- ✅ **CRUD Simples**: Gerenciamento de Nome, Ponto, Ramal, E-mail e Setor.
- ✅ **Vínculo com Setor**: Uso de DropdownTree para selecionar o setor em uma estrutura hierárquica.
- ✅ **Status**: Controle de Ativo/Inativo.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Requisitante/
│       ├── Index.cshtml             # Listagem
│       └── Upsert.cshtml            # Formulário
│
├── Controllers/
│   └── RequisitanteController.cs    # Endpoints API
│
├── wwwroot/
│   ├── js/
│   │   └── cadastros/
│   │       └── requisitante.js      # Lógica JS
```

### Tecnologias Utilizadas
| Tecnologia | Uso |
|------------|-----|
| **Syncfusion DropdownTree** | Seleção de Setores Hierárquicos |
| **jQuery DataTables** | Grid de listagem |
| **ASP.NET Core Razor Pages** | Renderização |

---

## Listagem (Index)

A página `Index.cshtml` exibe a lista de requisitantes.

### Estrutura da Tabela
A tabela consome a API `/api/Requisitante`.

**Colunas:**
1. Ponto (Matrícula)
2. Nome
3. Ramal
4. Setor (Nome do Setor vinculado)
5. Status (Badge Ativo/Inativo)
6. Ação (Editar)

---

## Cadastro/Edição (Upsert)

A página `Upsert.cshtml` contém o formulário.

### Campos do Formulário
- **Ponto**: Identificador único (Matrícula).
- **Nome**: Nome completo.
- **Ramal**: Telefone interno.
- **Email**: Contato principal.
- **Setor Solicitante**: Componente `ejs-dropdowntree` que carrega a árvore de setores.

### Lógica de Setor (DropdownTree)
O componente Syncfusion é configurado para permitir a seleção de setores filhos.

```javascript
// Pré-seleção no Upsert (JS Inline)
var setorId = "@Model.RequisitanteObj.Requisitante.SetorSolicitanteId";
if (setorId && setorId !== emptyGuid) {
    document.getElementById("ddtree").ej2_instances[0].value = [setorId];
}
```

---

## Endpoints API

### GET `/api/Requisitante`
Retorna a lista de requisitantes.

### POST `/Requisitante/Upsert` (Handler)
Processa a criação ou atualização.

---

## Troubleshooting

### Dropdown de Setor vazio
**Causa**: Falha ao carregar a árvore de setores no `ViewData["dataSource"]`.
**Solução**: Verifique o `OnGet` do `UpsertModel` e se a tabela de Setores está populada.

### Erro ao salvar Requisitante
**Causa**: Ponto duplicado ou E-mail inválido (se houver validação).
**Solução**: Verifique as mensagens de validação (`asp-validation-for`).

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial do módulo de Requisitantes.

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
