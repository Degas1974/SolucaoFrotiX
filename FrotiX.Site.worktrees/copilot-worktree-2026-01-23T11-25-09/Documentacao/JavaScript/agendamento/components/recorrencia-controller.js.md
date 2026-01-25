# Documentação: recorrencia-controller.js (Agendamento)

> **Última Atualização**: 20/01/2026 06:10
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

**Descrição**: O arquivo `recorrencia-controller.js` centraliza toda a escrita dos campos de recorrência do modal de agendamento. Ele atua como **single source of truth**, garantindo que nenhum outro módulo defina valores diretamente nos campos `lstRecorrente`, `lstPeriodos`, `lstDias`, `lstDiasMes`, `txtFinalRecorrencia` e calendário.

### Objetivo

- Blindar o fluxo de recorrência contra sobrescritas tardias
- Manter um único ponto de escrita para cada campo
- Permitir inicialização estável tanto em **novo agendamento** quanto em **edição**

---

## Funções Principais

### 1. `definirCampoRecorrencia(campo, valor, opcoes)`

**Propósito**: Aplicar valores de recorrência de maneira centralizada e segura.

```javascript
window.RecorrenciaController.definirCampoRecorrencia("periodo", "S", {
    habilitar: false,
    contexto: "edicao-semanal",
    silenciarEventos: true
});
```

**Detalhamento técnico**:

- **Localiza o controle** usando `obterInstanciaSyncfusion()`
- **Garante datasource** com `inicializarLstDias` / `inicializarDropdownPeriodos`
- **Aplica o valor** e bloqueia eventos quando necessário

---

### 2. `prepararNovo()`

**Propósito**: Configurar o modal para novo agendamento com recorrência limpa.

```javascript
if (window.RecorrenciaController?.prepararNovo) {
    window.RecorrenciaController.prepararNovo();
}
```

**Detalhamento técnico**:

- Define `Recorrente = N`
- Limpa período/dias/data final
- Exibe o card de recorrência em modo editável

---

### 3. `aplicarEdicao(objViagem)`

**Propósito**: Aplicar recorrência ao editar um agendamento existente.

```javascript
if (window.RecorrenciaController?.aplicarEdicao) {
    window.RecorrenciaController.aplicarEdicao(objViagem);
}
```

**Detalhamento técnico**:

- Detecta se o agendamento é recorrente
- Aplica intervalo correto (D, S, Q, M, V)
- Bloqueia campos para evitar sobrescritas

---

## Interações com Outros Arquivos

- `exibe-viagem.js` → chama `prepararNovo()` e `aplicarEdicao(objViagem)`
- `recorrencia-logic.js` → chama `limparCampos()` e `definirCampoRecorrencia()`
- `main.js` → deixa de definir valores padrão manualmente

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Criação inicial do controlador

**Descrição**: Implementação do `RecorrenciaController` para centralizar a escrita dos campos de recorrência e eliminar sobrescritas na edição.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/recorrencia-controller.js`
- `wwwroot/js/agendamento/components/exibe-viagem.js`
- `wwwroot/js/agendamento/components/recorrencia-logic.js`
- `wwwroot/js/agendamento/main.js`

**Status**: ✅ Concluído

**Responsável**: Codex
