# Documentação: listaautuacao.js

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

Este arquivo JavaScript gerencia a **Lista de Autuações** no módulo de Multas do FrotiX. Autuações são multas em fase inicial de processamento, antes de se tornarem penalidades.

### Características Principais

- ✅ Listagem completa de autuações via DataTables
- ✅ Filtros por data, status, veículo, órgão autuante
- ✅ Exportação para Excel e PDF via DataTables Buttons
- ✅ Visualização de PDFs de autuação via Syncfusion PDF Viewer
- ✅ Alteração de status e forma de pagamento
- ✅ Exclusão de autuações com confirmação

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── wwwroot/js/cadastros/listaautuacao.js   ← Este arquivo
├── Pages/Multa/ListaAutuacao.cshtml        ← Página Razor associada
├── Pages/Multa/ListaAutuacao.cshtml.cs     ← PageModel
```

### Informações de Roteamento

- **Módulo**: Multa
- **Página**: ListaAutuacao
- **Arquivo JavaScript**: `~/js/cadastros/listaautuacao.js`

---

## Funções Principais

### ListaTodasNotificacoes()

Carrega todas as autuações do período selecionado via AJAX.

**Fluxo:**

1. Mostra loading overlay
2. Coleta filtros (datas, status, veículo)
3. Faz requisição AJAX para `/api/autuacao/listar`
4. Popula DataTable com resultados
5. Esconde loading overlay

### mostrarLoadingAutuacao()

Exibe overlay de loading com blur de fundo durante operações.

**Características:**

- Background escuro semi-transparente
- Logo FrotiX pulsante
- Texto "Processando..."

### esconderLoadingAutuacao()

Remove overlay de loading após conclusão da operação.

### exibirPDFAutuacao(autuacaoId)

Carrega e exibe PDF da autuação no viewer Syncfusion.

**Parâmetros:**

- `autuacaoId` (Guid): ID da autuação

**Fluxo:**

1. Obtém URL do PDF via AJAX
2. Carrega no Syncfusion PDF Viewer
3. Exibe modal com visualização

---

## Event Handlers

### btn-status (click)

Abre modal para alterar status da autuação.

- Carrega status atual
- Lista opções de status disponíveis
- Envia alteração via AJAX

### btn-pagamento (click)

Abre modal para alterar forma de pagamento.

- Lista formas de pagamento disponíveis
- Envia alteração via AJAX

### btn-apagar (click)

Exclui autuação com confirmação.

- Usa Alerta.Confirmar para confirmar exclusão
- Faz AJAX DELETE para `/api/autuacao/{id}`
- Recarrega tabela após sucesso

### btn-exibe-autuacao (click)

Exibe PDF da autuação no viewer.

- Obtém ID da autuação via data-attribute
- Chama exibirPDFAutuacao()

---

## Dependências

| Biblioteca                | Versão | Uso                     |
| ------------------------- | ------ | ----------------------- |
| jQuery                    | 3.x    | AJAX, manipulação DOM   |
| DataTables                | 2.x    | Tabela principal        |
| DataTables Buttons        | 3.x    | Exportação Excel/PDF    |
| DTBetterErrors            | -      | Tratamento de erros     |
| Syncfusion EJ2 PDF Viewer | 29.x   | Visualização de PDFs    |
| SweetAlert2               | -      | Via Alerta.\* (interop) |

---

## Padrões FrotiX Aplicados

- ✅ Try-catch em todas as funções com `Alerta.TratamentoErroComLinha`
- ✅ Confirmações via `Alerta.Confirmar` (não `confirm()`)
- ✅ Ícones FontAwesome Duotone
- ✅ Loading overlay FrotiX durante operações assíncronas

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026] - Documentação JSDoc Completa

**Descrição**: Adicionada documentação JSDoc padrão FrotiX completa ao arquivo.

**Arquivos Afetados**:

- wwwroot/js/cadastros/listaautuacao.js

**Mudanças**:

- ✅ Cabeçalho JSDoc com box visual FrotiX e metadados
- ✅ Comentários inline explicativos em todas as funções
- ✅ Documentação de event handlers

**Motivo**: Conformidade com padrão de documentação FrotiX

**Impacto**: Apenas documentação, sem alteração funcional

**Status**: ✅ **Concluído**

**Responsável**: Claude Opus 4.5

**Versão**: 1.0
