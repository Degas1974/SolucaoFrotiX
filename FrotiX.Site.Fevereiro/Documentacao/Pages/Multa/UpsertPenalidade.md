# Documentação: Multa - Upsert Penalidade

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Frontend](#frontend)

---

## Visão Geral

Página de criação e edição (Upsert) de penalidades de multas no sistema FrotiX.

### Características Principais
- ✅ **Criação de Penalidades**: Permite criar novas penalidades
- ✅ **Edição de Penalidades**: Permite atualizar penalidades existentes
- ✅ **Upload de PDF**: Upload do documento de penalidade em PDF
- ✅ **Gestão de Empenhos**: Associa penalidade a empenho do órgão autuante
- ✅ **Botão Voltar à Lista**: Navegação rápida para lista de penalidades

### Objetivo
Centralizar o cadastro de penalidades, facilitando o controle de multas na fase de penalidade.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| ASP.NET Core | 9.0 | Backend Razor Pages |
| Syncfusion EJ2 | 31.1.22 | Componentes UI |
| jQuery | 3.x | Manipulação DOM |
| Bootstrap | 5.3.8 | Layout responsivo |

### Padrões de Design
- **Razor Pages**: Padrão MVVM com PageModel
- **Repository Pattern**: Acesso a dados

---

## Estrutura de Arquivos

### Arquivo Principal
```
Pages/Multa/UpsertPenalidade.cshtml
```

### Arquivos Relacionados
- `Pages/Multa/UpsertPenalidade.cshtml.cs` - PageModel
- `Pages/Multa/ListaPenalidade.cshtml` - Lista de penalidades

---

## Lógica de Negócio

Similar à UpsertAutuacao, porém para a fase de Penalidade do processo de multas.

---

## Frontend

### Header com Botão Voltar
```html
<div class="ftx-card-header d-flex justify-content-between align-items-center">
    <h2 class="titulo-paginas mb-0">
        <i class="fa-duotone fa-gavel"></i>
        Criar/Atualizar Penalidade
    </h2>
    <div class="ftx-card-actions">
        <a asp-page="/Multa/ListaPenalidade" class="btn btn-header-orange" data-ftx-loading>
            <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i>
            Voltar à Lista
        </a>
    </div>
</div>
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [12/01/2026 - 16:25] - Adicionado botão Voltar à Lista

**Descrição**: Adicionado botão "Voltar à Lista" no header da página, seguindo padrão FrotiX.

**Arquivos Afetados**:
- `Pages/Multa/UpsertPenalidade.cshtml` (linhas 309-321)

**Impacto**: Melhoria de navegação com botão laranja no header

**Status**: ✅ **Concluído**

**Versão**: 1.0

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0
