# Documentação: modal-config.js

> **Última Atualização**: 20/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

**Descrição**: Arquivo de configuração do modal de agendamento/viagem. Define títulos, ícones e cores para cada tipo de modal, além de funções utilitárias para garantir funcionamento correto dos botões de fechar.

### Características Principais

- ✅ **Configuração de Títulos**: Define HTML para cada tipo de modal (novo, editar, cancelado, viagem aberta, etc.)
- ✅ **Ícones Duotone**: Usa Font Awesome Duotone com cores customizadas
- ✅ **Fonte Outfit**: Títulos com fonte Outfit e estilo personalizado
- ✅ **Botões de Fechar**: Garante que botões de fechar nunca sejam desabilitados

---

## Estrutura do Arquivo

### Constante `TITLE_STYLE`
Estilo inline aplicado a todos os títulos do modal.

### Objeto `window.ModalConfig`
Configuração de títulos para cada tipo de modal:

| Tipo | Descrição | Ícone |
|------|-----------|-------|
| NOVO_AGENDAMENTO | Criar novo agendamento | `fa-calendar-lines-pen` (verde) |
| EDITAR_AGENDAMENTO | Editar agendamento existente | `fa-calendar-lines-pen` (azul) |
| AGENDAMENTO_CANCELADO | Agendamento cancelado | `fa-calendar-xmark` (vermelho) |
| VIAGEM_ABERTA | Viagem em andamento | `fa-suitcase-rolling` |
| VIAGEM_REALIZADA | Viagem finalizada (readonly) | `fa-suitcase-rolling` |
| VIAGEM_CANCELADA | Viagem cancelada (readonly) | `fa-suitcase-rolling` |
| TRANSFORMAR_VIAGEM | Transformar agendamento em viagem | `fa-calendar-lines-pen` (azul) |

---

## Funções Principais

### `window.setModalTitle(tipo, statusTexto)`
Define o título do modal com base no tipo.

**Parâmetros**:
- `tipo` (string): Chave do ModalConfig
- `statusTexto` (string, opcional): Texto adicional de status

### `window.resetModal()`
Reseta o modal para estado inicial (NOVO_AGENDAMENTO).

### `window.garantirBotoesFechaHabilitados()`
Garante que todos os botões de fechar estejam habilitados.

---

## Interconexões

### Quem Chama Este Arquivo
- `exibe-viagem.js` → Chama `setModalTitle()` ao abrir modal
- `main.js` → Registra listener para `garantirBotoesFechaHabilitados()`

### O Que Este Arquivo Chama
- `limparCamposModalViagens()` → Em `resetModal()`
- `inicializarCamposModal()` → Em `resetModal()`
- `Alerta.TratamentoErroComLinha()` → Para tratamento de erros

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Fix: Removido setInterval que causava travamento

**Descrição**: Removido `setInterval` de 1 segundo que rodava indefinidamente, causando travamento do modal ao arrastar ou interagir múltiplas vezes.

**Problema**:
- `setInterval(window.garantirBotoesFechaHabilitados, 1000)` rodava a cada segundo
- Manipulava DOM continuamente mesmo sem necessidade
- Conflitava com arraste do modal e outros eventos

**Solução**:
```javascript
// ANTES (problema):
setInterval(window.garantirBotoesFechaHabilitados, 1000);

// DEPOIS (corrigido):
// Executar apenas uma vez ao carregar
window.garantirBotoesFechaHabilitados();

// Executar quando modal abrir (registrado uma vez)
if (!window._garantirBotoesListenerRegistrado) {
    $('#modalViagens').on('shown.bs.modal', function() {
        window.garantirBotoesFechaHabilitados();
    });
    window._garantirBotoesListenerRegistrado = true;
}
```

**Impacto**:
- ✅ Modal não trava mais ao arrastar
- ✅ Botões de fechar ainda são garantidos quando modal abre
- ✅ Sem polling desnecessário

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.0

---

**Última atualização**: 20/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0
