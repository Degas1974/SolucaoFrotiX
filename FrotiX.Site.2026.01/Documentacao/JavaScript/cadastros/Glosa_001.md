# Documentação: Glosa_001.js

> **Última Atualização**: 22/01/2026
> **Versão Atual**: 1.1

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

Este arquivo JavaScript gerencia a funcionalidade de **Glosas Contratuais** no módulo de Manutenção do FrotiX. Glosas são mecanismos de dedução financeira aplicados quando há inconformidades operacionais em contratos de terceirização, locação ou serviços.

### Características Principais

- ✅ Gestão de três tipos de contratos: Terceirização, Locação e Serviços
- ✅ Carregamento dinâmico de contratos via DataManager Syncfusion
- ✅ Tabelas de veículos e notas fiscais por contrato selecionado
- ✅ Operações de remover motorista, operador, lavador e veículo de contratos
- ✅ Integração com endpoints API para operações CRUD

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── wwwroot/js/cadastros/Glosa_001.js    ← Este arquivo
├── Pages/Manutencao/Glosa.cshtml        ← Página Razor associada
├── Pages/Manutencao/Glosa.cshtml.cs     ← PageModel
```

### Informações de Roteamento

- **Módulo**: Manutenção
- **Página**: Glosa
- **Arquivo JavaScript**: `~/js/cadastros/Glosa_001.js`

---

## Funções Principais

### loadListaContratos()

Carrega a lista de contratos conforme o tipo selecionado (Terceirização, Locação, Serviços).

**Fluxo:**

1. Obtém ID do tipo de contrato do dropdown
2. Configura DataManager Syncfusion com URL do endpoint
3. Popula dropdown de contratos com dados retornados

### loadTblVeiculos()

Carrega tabela de veículos associados ao contrato selecionado.

**Parâmetros:**

- Contrato selecionado (via dropdown)
- Tipo de contrato

**Retorno:**

- Popula DataTable com veículos do contrato

### loadTblNotas()

Carrega tabela de notas fiscais relacionadas ao contrato.

**Parâmetros:**

- Contrato selecionado
- Período (mês/ano) de referência

---

## Event Handlers

### btn-remover-motorista-contrato

Remove associação de motorista com contrato de terceirização.

- Usa Alerta.Confirmar para confirmação
- Faz AJAX POST para `/api/glosa/remover-motorista`

### btn-remover-operador-contrato

Remove associação de operador com contrato.

- Usa Alerta.Confirmar para confirmação
- Faz AJAX POST para `/api/glosa/remover-operador`

### btn-remover-lavador-contrato

Remove associação de lavador com contrato de lavagem.

- Usa Alerta.Confirmar para confirmação
- Faz AJAX POST para `/api/glosa/remover-lavador`

### btn-remover-veiculo-contrato

Remove associação de veículo com contrato.

- Usa Alerta.Confirmar para confirmação
- Faz AJAX POST para `/api/glosa/remover-veiculo`

---

## Dependências

| Biblioteca     | Versão | Uso                         |
| -------------- | ------ | --------------------------- |
| jQuery         | 3.x    | AJAX, manipulação DOM       |
| Syncfusion EJ2 | 29.x   | DropDownList, DataManager   |
| DataTables     | 2.x    | Tabelas de veículos e notas |
| SweetAlert2    | -      | Via Alerta.\* (interop)     |

---

## Padrões FrotiX Aplicados

- ✅ Try-catch em todas as funções com `Alerta.TratamentoErroComLinha`
- ✅ Confirmações via `Alerta.Confirmar` (não `confirm()`)
- ✅ Ícones FontAwesome Duotone
- ✅ Loading overlay durante operações assíncronas

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026] - Documentação JSDoc Completa

**Descrição**: Adicionada documentação JSDoc padrão FrotiX completa ao arquivo.

**Arquivos Afetados**:

- wwwroot/js/cadastros/Glosa_001.js

**Mudanças**:

- ✅ Cabeçalho JSDoc com box visual FrotiX e metadados
- ✅ Comentários inline explicativos em todas as funções
- ✅ Documentação de event handlers
- ✅ Corrigido referências incorretas de "Glosa\_&lt;num&gt;.js" para "Glosa_001.js"

**Motivo**: Conformidade com padrão de documentação FrotiX

**Impacto**: Apenas documentação, sem alteração funcional

**Status**: ✅ **Concluído**

**Responsável**: Claude Opus 4.5

**Versão**: 1.1
