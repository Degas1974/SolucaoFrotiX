# Documentação: orgaoautuante.js

> **Última Atualização**: 22/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Funções Principais](#funções-principais)
4. [Event Handlers](#event-handlers)
5. [Dependências](#dependências)

---

## Visão Geral

Este arquivo JavaScript gerencia o **CRUD de Órgãos Autuantes** no módulo de Multas do FrotiX. Órgãos autuantes são as entidades responsáveis pela emissão de multas (DETRAN, PRF, DMAE, PM, BPTran, etc.).

### Características Principais

- ✅ Listagem de órgãos autuantes cadastrados
- ✅ Exclusão com confirmação via Alerta.Confirmar
- ✅ Integração com endpoints API
- ✅ Tratamento de erros padrão FrotiX

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── wwwroot/js/cadastros/orgaoautuante.js        ← Este arquivo
├── Pages/Multa/UpsertOrgaoAutuante.cshtml       ← Página Razor associada
├── Pages/Multa/UpsertOrgaoAutuante.cshtml.cs    ← PageModel
```

### Informações de Roteamento

- **Módulo**: Multa
- **Página**: UpsertOrgaoAutuante
- **Arquivo JavaScript**: `~/js/cadastros/orgaoautuante.js`
- **Endpoint API**: `/api/orgaoautuante`

---

## Funções Principais

### loadList()

Carrega lista de órgãos autuantes cadastrados via AJAX.

**Fluxo:**

1. Faz requisição GET para `/api/orgaoautuante`
2. Popula tabela/lista com órgãos retornados
3. Configura event handlers para botões de ação

**Tratamento de Erro:**

- Try-catch com `Alerta.TratamentoErroComLinha`

---

## Event Handlers

### btn-delete (click)

Exclui órgão autuante com confirmação.

**Fluxo:**

1. Obtém ID do órgão via `data-id`
2. Exibe confirmação via `Alerta.Confirmar`
   - Título: "Confirmar Exclusão"
   - Mensagem: "Deseja realmente excluir este órgão autuante?"
3. Se confirmado, faz AJAX DELETE para `/api/orgaoautuante/{id}`
4. Em caso de sucesso:
   - Exibe `Alerta.Sucesso`
   - Recarrega lista
5. Em caso de erro:
   - Exibe `Alerta.Erro` com detalhes

**Tratamento de Erro:**

- Try-catch com `Alerta.TratamentoErroComLinha`

---

## Dependências

| Biblioteca  | Versão | Uso                     |
| ----------- | ------ | ----------------------- |
| jQuery      | 3.x    | AJAX, manipulação DOM   |
| SweetAlert2 | -      | Via Alerta.\* (interop) |

---

## Endpoints Utilizados

| Método | Endpoint                  | Descrição             |
| ------ | ------------------------- | --------------------- |
| GET    | `/api/orgaoautuante`      | Lista todos os órgãos |
| DELETE | `/api/orgaoautuante/{id}` | Exclui órgão por ID   |

---

## Padrões FrotiX Aplicados

- ✅ Try-catch em todas as funções com `Alerta.TratamentoErroComLinha`
- ✅ Confirmações via `Alerta.Confirmar` (não `confirm()`)
- ✅ Mensagens via `Alerta.Sucesso` e `Alerta.Erro`
- ✅ Ícones FontAwesome Duotone

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026] - Documentação JSDoc Completa

**Descrição**: Adicionada documentação JSDoc padrão FrotiX completa ao arquivo.

**Arquivos Afetados**:

- wwwroot/js/cadastros/orgaoautuante.js

**Mudanças**:

- ✅ Cabeçalho JSDoc com box visual FrotiX e metadados
- ✅ Comentários inline explicativos na função loadList()
- ✅ Documentação do event handler btn-delete
- ✅ Documentação do fluxo AJAX de exclusão

**Motivo**: Conformidade com padrão de documentação FrotiX

**Impacto**: Apenas documentação, sem alteração funcional

**Status**: ✅ **Concluído**

**Responsável**: Claude Opus 4.5

**Versão**: 1.0
