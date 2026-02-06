# Documentação: recorrencia-logic.js (Agendamento)

> **Última Atualização**: 20/01/2026 06:10
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

**Descrição**: O arquivo `recorrencia-logic.js` controla a **visibilidade** e os **eventos de UI** dos campos de recorrência. Ele exibe/oculta seções conforme o usuário seleciona `Recorrente` e `Período`, e agora trabalha em conjunto com o `RecorrenciaController` para evitar sobrescritas de valores.

### Objetivo

- Garantir visibilidade correta para cada tipo de recorrência
- Manter eventos de UI desacoplados da escrita dos campos
- Integrar com o controlador central (`recorrencia-controller.js`)

---

## Principais Ajustes Recentes

### Integração com RecorrenciaController

```javascript
if (window.RecorrenciaController?.definirCampoRecorrencia) {
    window.RecorrenciaController.definirCampoRecorrencia("periodo", null, {
        habilitar: true,
        contexto: "aoMudarRecorrente",
        silenciarEventos: true
    });
}
```

**Detalhamento**:

- A lógica de limpeza de valores é delegada ao controller
- Evita duplicidade de funções definindo o mesmo campo
- O arquivo permanece responsável apenas por **visibilidade** e **eventos**

---

## Interações com Outros Arquivos

- `recorrencia-controller.js` → Escrita centralizada dos campos
- `exibe-viagem.js` → Usa o controller para aplicar edição
- `main.js` → Chama `window.inicializarLogicaRecorrencia()`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Ajuste: integração com RecorrenciaController

**Descrição**: Removido reset automático de `lstRecorrente` e delegada a limpeza de campos para o controlador central.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/recorrencia-logic.js`
- `wwwroot/js/agendamento/components/recorrencia-controller.js`

**Status**: ✅ Concluído

**Responsável**: Codex
