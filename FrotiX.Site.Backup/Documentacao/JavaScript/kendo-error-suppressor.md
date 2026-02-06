# Documentacao: kendo-error-suppressor.js

> **Ultima Atualizacao**: 23/01/2026
> **Versao Atual**: 1.1

---

# PARTE 1: DOCUMENTACAO DA FUNCIONALIDADE

## Indice
1. [Visao Geral](#visao-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Logica de Negocio](#logica-de-negocio)
5. [Interconexoes](#interconexoes)
6. [Troubleshooting](#troubleshooting)

---

## Visao Geral

**Descricao**: Este arquivo e um supressor de erros que intercepta e silencia erros especificos de bibliotecas de terceiros (Kendo UI e Syncfusion) que ocorrem durante o carregamento da pagina ou ao fechar modais, mas que nao afetam a funcionalidade do sistema.

### Caracteristicas Principais
- Intercepta `console.error` para filtrar erros conhecidos
- Intercepta `window.onerror` para capturar erros globais
- Intercepta `unhandledrejection` para capturar Promises rejeitadas
- Silencia erros de formatacao do Syncfusion (percentSign, currencySign)
- Silencia erros do Kendo UI (collapsible, toggle)

### Objetivo
Evitar que erros inofensivos de bibliotecas de terceiros poluam o console e causem confusao para desenvolvedores e usuarios. Esses erros geralmente ocorrem devido a timing de carregamento de dados CLDR e nao afetam a experiencia do usuario.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versao | Uso |
|------------|--------|-----|
| JavaScript | ES6+ | Interceptacao de erros |

### Padroes de Design
- IIFE (Immediately Invoked Function Expression) para isolamento de escopo
- Decorator Pattern para sobrescrever funcoes nativas

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/kendo-error-suppressor.js
```

### Arquivos Relacionados
- `Pages/Shared/_ScriptsBasePlugins.cshtml` - Carrega este script PRIMEIRO

---

## Logica de Negocio

### Funcao: `isSyncfusionFormatError()`

**Proposito**: Verifica se uma mensagem de erro e relacionada a formatacao do Syncfusion

**Parametros**:
- `msg` (string): Mensagem de erro a ser verificada

**Retorno**: boolean - true se for erro de formatacao

**Codigo**:
```javascript
function isSyncfusionFormatError(msg) {
    const lowerMsg = msg.toLowerCase();
    return syncfusionFormatErrors.some(err => lowerMsg.includes(err));
}
```

### Interceptador: `console.error`

**Proposito**: Substitui a funcao nativa para filtrar erros especificos

**Fluxo de Execucao**:
1. Recebe argumentos do erro
2. Verifica se e erro do Kendo (collapsible, toggle)
3. Verifica se e erro de formatacao Syncfusion
4. Se for erro conhecido, exibe apenas warning
5. Caso contrario, chama `console.error` original

### Interceptador: `window.onerror`

**Proposito**: Captura erros globais nao tratados

**Parametros**:
- `message`: Mensagem de erro
- `source`: URL do arquivo fonte
- `lineno`: Numero da linha
- `colno`: Numero da coluna
- `error`: Objeto de erro

**Retorno**:
- `true` - Previne propagacao (erro suprimido)
- `false` - Permite propagacao normal

### Interceptador: `unhandledrejection`

**Proposito**: Captura Promises rejeitadas nao tratadas

**Fluxo**:
1. Verifica se a razao da rejeicao e erro de formatacao
2. Se for, chama `event.preventDefault()` para suprimir

---

## Interconexoes

### Quem Carrega Este Arquivo
- `Pages/Shared/_ScriptsBasePlugins.cshtml` (linha 5) - **DEVE SER O PRIMEIRO SCRIPT**

### Bibliotecas Afetadas
- **Kendo UI**: Erros de `collapsible` e `toggle`
- **Syncfusion EJ2**: Erros de `percentSign`, `currencySign`, `format options`

### Ordem de Carregamento Critica
```
1. kendo-error-suppressor.js  <-- PRIMEIRO (este arquivo)
2. jQuery
3. SignalR
4. Kendo UI
5. Syncfusion EJ2
6. ... demais scripts
```

---

## Troubleshooting

### Problema: Erro ainda aparece no console

**Sintoma**: Mesmo com o supressor, alguns erros ainda aparecem

**Causa**: O script nao esta sendo carregado primeiro

**Solucao**: Verificar em `_ScriptsBasePlugins.cshtml` se este arquivo esta na linha 5 (primeiro script)

### Problema: Erro de formatacao ao fechar modal

**Sintoma**: `TypeError: Cannot read properties of undefined (reading 'percentSign')`

**Causa**: Syncfusion tenta formatar numeros antes do CLDR estar carregado

**Solucao**: Este supressor ja trata esse erro. Se persistir, verificar se o script esta sendo carregado corretamente.

---

# PARTE 2: LOG DE MODIFICACOES/CORRECOES

---

## [23/01/2026] - Adiciona suporte a erros Syncfusion

**Descricao**: Expandido para tambem suprimir erros de formatacao do Syncfusion (percentSign, currencySign) que ocorrem ao fechar modais quando o CLDR ainda nao esta totalmente carregado.

**Arquivos Afetados**:
- `wwwroot/js/kendo-error-suppressor.js` (linhas 1-85)

**Impacto**: Usuarios nao verao mais erros de formatacao no console ao fechar modais

**Status**: Concluido

**Versao**: 1.1

---

## [??/??/2025] - Versao inicial

**Descricao**: Criacao do supressor para erros do Kendo UI (collapsible, toggle)

**Status**: Concluido

**Versao**: 1.0

---

## Historico de Versoes

| Versao | Data | Descricao |
|--------|------|-----------|
| 1.0 | ??/??/2025 | Versao inicial - Kendo UI |
| 1.1 | 23/01/2026 | Adiciona suporte a erros Syncfusion |

---

**Ultima atualizacao**: 23/01/2026
**Autor**: Sistema FrotiX
**Versao**: 1.1
