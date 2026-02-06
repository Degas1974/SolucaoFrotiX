# Documentação: main.js (Agendamento)

> **Última Atualização**: 18/01/2026
> **Versão Atual**: 1.3

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Interconexões](#interconexões)

---

## Visão Geral

**Descrição**: O arquivo `main.js` é o ponto de entrada principal do módulo de Agendamento da Agenda de Viagens. É responsável por inicializar todos os componentes Syncfusion, configurar event handlers dos botões do modal, gerenciar o calendário FullCalendar e orquestrar a interação entre todos os submódulos.

### Características Principais
- ✅ **Inicialização do FullCalendar**: Configura calendário de agenda com eventos via AJAX
- ✅ **Configuração de Componentes Syncfusion**: Inicializa DatePickers, ComboBoxes, DropDownTrees, etc.
- ✅ **Event Handlers de Botões**: Configura clicks de Confirmar, Apagar, Viagem, Imprimir e Fechar
- ✅ **Gerenciamento de Modal**: Controla abertura/fechamento e limpeza do modal de agendamento
- ✅ **Localização pt-BR**: Chama configuração de localização Syncfusion

### Objetivo
Centralizar a inicialização e coordenação de todos os componentes e funcionalidades do módulo de Agendamento, servindo como "maestro" que orquestra a interação entre modal, calendário e submódulos.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| FullCalendar | 6.x | Calendário de agenda |
| Syncfusion EJ2 | - | Componentes UI (DatePicker, ComboBox, etc.) |
| jQuery | 3.x | Manipulação DOM e AJAX |
| Bootstrap 5 | 5.x | Modal e componentes UI |

### Padrões de Design
- **Módulo Pattern**: Funções agrupadas por responsabilidade
- **Event-Driven**: Event handlers para interações do usuário
- **Async/Await**: Operações assíncronas para AJAX

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/agendamento/main.js
```

### Arquivos Relacionados
- `wwwroot/js/agendamento/components/*.js` - Submódulos especializados (exibe-viagem, evento, etc.)
- `wwwroot/js/agendamento/utils/*.js` - Utilitários (syncfusion.utils, etc.)
- `wwwroot/js/agendamento/services/*.js` - Serviços (requisitante.service, etc.)
- `Pages/Agenda/Index.cshtml` - View que referencia este arquivo

---

## Lógica de Negócio

### Funções/Métodos Principais

#### Função: `InitializeCalendar(URL)`
**Localização**: Início do arquivo

**Propósito**: Inicializa o calendário FullCalendar com configurações pt-BR e eventos via AJAX

**Parâmetros**:
- `URL` (string): URL base do servidor (não utilizada diretamente)

**Retorno**: void

**Fluxo de Execução**:
1. Obtém elemento `#agenda`
2. Cria instância `FullCalendar.Calendar` com configurações:
   - Locale pt-BR
   - Views (mensal, semanal, diário)
   - Eventos carregados de `/api/Agenda/CarregaViagens`
   - Event handlers (click, dateClick, drop, resize)
3. Renderiza calendário

---

#### Função: `configurarBotaoConfirmar()`
**Localização**: Meio do arquivo

**Propósito**: Configura event handler do botão "Confirmar" (salvar viagem/agendamento)

**Fluxo de Execução**:
1. Adiciona click handler em `#btnConfirma`
2. Valida dados do formulário
3. Chama `window.salvarViagem()` (definida em outro arquivo)
4. Fecha modal e atualiza calendário em caso de sucesso

---

#### Função: `configurarBotaoApagar()`
**Localização**: Meio do arquivo

**Propósito**: Configura event handler do botão "Apagar" (excluir agendamento)

**Fluxo de Execução**:
1. Adiciona click handler em `#btnApaga`
2. Exibe confirmação via `Alerta.Confirmar()`
3. Se confirmado:
   - Se recorrente: pergunta se apaga todos ou apenas atual
   - Chama API para apagar via AJAX
4. Atualiza calendário e fecha modal

---

#### Função: `configurarBotaoViagem()`
**Localização**: Meio do arquivo

**Propósito**: Configura event handler do botão "Transformar em Viagem" (converte agendamento em viagem)

**Fluxo de Execução**:
1. Adiciona click handler em `#btnViagem`
2. Exibe confirmação
3. Chama `window.transformarEmViagem()`
4. Atualiza calendário

---

#### Função: `configurarBotaoImprimir()`
**Localização**: Meio do arquivo

**Propósito**: Configura event handler do botão "Imprimir" (gera relatório)

**Fluxo de Execução**:
1. Adiciona click handler em `#btnImprime`
2. Obtém ID da viagem
3. Abre janela com relatório via `/Viagens/Imprimir?id=...`

---

#### Função: `configurarBotõesFechar()`
**Localização**: Meio do arquivo

**Propósito**: Configura event handlers de todos os botões que fecham o modal

**Fluxo de Execução**:
1. Identifica todos os seletores de botões de fechar (`#btnFecha`, `[data-bs-dismiss="modal"]`, etc.)
2. Adiciona click handler que:
   - Limpa tooltips globais
   - Fecha modal via Bootstrap
   - Limpa formulário

---

#### Função: `inicializarComponentesEJ2()`
**Localização**: Fim do arquivo

**Propósito**: Inicializa todos os componentes Syncfusion EJ2 do modal

**Fluxo de Execução**:
1. Inicializa DatePickers (txtDataInicial, txtHoraInicial, etc.)
2. Inicializa ComboBoxes (lstVeiculo, lstMotorista, etc.)
3. Inicializa DropDownTree (lstUnidades, lstTipoUnidades, etc.)
4. Inicializa RichTextEditor (rteDescricao)
5. Configura event handlers de mudança de valor

---

#### Função: `$(document).ready()`
**Localização**: Fim do arquivo

**Propósito**: Função de inicialização executada quando DOM está pronto

**Fluxo de Execução**:
1. Configura localização Syncfusion pt-BR
2. Inicializa calendário FullCalendar
3. Inicializa componentes EJ2
4. Configura event handlers de botões
5. Configura validações
6. Prepara modal para uso

---

## Interconexões

### Quem Chama Este Arquivo
- **`Pages/Agenda/Index.cshtml`**: Referencia este arquivo na seção de scripts

### O Que Este Arquivo Chama
- **`syncfusion.utils.js`**: `configurarLocalizacaoSyncfusion()`
- **`exibe-viagem.js`**: `window.exibirViagem()`, `window.mostrarCamposViagem()`
- **`evento.js`**: Funções de gerenciamento de eventos
- **`modal-viagem-novo.js`**: `window.configurarModalViagemNovo()`
- **`alerta.js`**: `Alerta.Confirmar()`, `Alerta.TratamentoErroComLinha()`
- **`global-toast.js`**: `AppToast.show()`
- **APIs**: `/api/Agenda/CarregaViagens`, `/api/Viagem/Salvar`, `/api/Viagem/Apagar`

### Fluxo de Dados
```
Usuário interage com Calendário/Modal
    ↓
main.js captura evento
    ↓
Chama submódulo especializado (exibe-viagem.js, etc.)
    ↓
Submódulo processa lógica
    ↓
AJAX para API
    ↓
Retorno atualiza calendário/modal
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [18/01/2026 - 00:30] - Otimização: Exclusão em Lote de Agendamentos Recorrentes

**Descrição**: Substituído loop de múltiplas requisições individuais por uma única chamada ao novo endpoint `ApagaAgendamentosRecorrentes` ao clicar "Apagar Todos" nos agendamentos recorrentes.

**Problema**:
- Código anterior fazia N requisições HTTP (uma por agendamento recorrente)
- Usava delay artificial de 200ms entre cada delete
- Para 10 agendamentos = 10 requests + 2 segundos de delay
- Possibilidade de erro 500 por violação de integridade referencial

**Solução**:

**ANTES (linhas 637-651)**:
```javascript
const agendamentosRecorrentes = await window.obterAgendamentosRecorrentes(recorrenciaId);
for (const agendamento of agendamentosRecorrentes)
{
    await window.excluirAgendamento(agendamento.viagemId);
    await window.delay(200); // Delay artificial
}
```

**DEPOIS (linhas 637-661)**:
```javascript
// Chamar novo endpoint que deleta todos de uma vez (mais eficiente e sem erro de FK)
const result = await window.AgendamentoService.excluirRecorrentes(recorrenciaId);

if (result.success)
{
    AppToast.show("Verde", result.message || "Todos foram excluídos!", 3000);
}
else
{
    Alerta.Erro("Erro ao Excluir", result.message || result.error, "OK");
}
```

**Mudanças**:
- ❌ Removido loop `for (const agendamento of agendamentosRecorrentes)`
- ❌ Removido `await window.excluirAgendamento()` individual
- ❌ Removido `await window.delay(200)` artificial
- ✅ Adicionado chamada única a `AgendamentoService.excluirRecorrentes()`
- ✅ Adicionado tratamento de erro com `Alerta.Erro()`

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 637-661)
- Backend: `Controllers/AgendaController.cs` (novo endpoint)
- Service: `agendamento.service.js` (novo método)

**Impacto**:
- ✅ Performance 10x melhor (1 request vs N requests)
- ✅ Sem delays artificiais
- ✅ Resolução de erro 500 (FK tratada no backend)
- ✅ Transação atômica no backend (tudo ou nada)

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.3

---

## [17/01/2026 23:15] - CRÍTICO: Modal de Espera Movido para Após Validações

**Descrição**: Movido `FtxSpin.show()` para DEPOIS de todas as validações de campos e validação IA, impedindo que o modal bloqueie alertas de erro.

**Problema CRÍTICO**:
- Modal de espera aparecia ANTES das validações de campos
- Quando havia erro (ex: Requisitante ausente), SweetAlert ficava ABAIXO do modal
- Usuário não conseguia clicar no alerta de erro
- Única solução era sair da página

**Fluxo Anterior (INCORRETO)**:
```javascript
$btn.prop("disabled", true);
FtxSpin.show("Gravando Agendamento(s)");  // ❌ ANTES DAS VALIDAÇÕES

const validado = await window.ValidaCampos(viagemId);
if (!validado) {
    return;  // ❌ Modal ainda visível, alerta bloqueado
}
```

**Fluxo Corrigido**:
```javascript
$btn.prop("disabled", true);

// 1. Validação de campos
const validado = await window.ValidaCampos(viagemId);
if (!validado) {
    $btn.prop("disabled", false);  // ✅ Re-habilita botão
    return;  // ✅ Modal não foi exibido, alerta funciona
}

// 2. Validação IA
if (isRegistraViagem) {
    const iaValida = await window.validarFinalizacaoConsolidadaIA({...});
    if (!iaValida) {
        $btn.prop("disabled", false);  // ✅ Re-habilita botão
        return;  // ✅ Modal não foi exibido, alerta funciona
    }
}

// ✅ AGORA SIM: Todas validações OK, pode exibir modal
FtxSpin.show("Gravando Agendamento(s)");
```

**Alterações** (linhas 107-155):
- Removido `FtxSpin.show()` da linha 110 (ANTES das validações)
- Adicionado `FtxSpin.show()` na linha 155 (DEPOIS de todas validações)
- Adicionado `$btn.prop("disabled", false)` em todos os `return` de validação falha

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 107-155)

**Impacto**:
- **Alertas de erro agora são clicáveis** - não ficam mais bloqueados pelo modal
- **UX drasticamente melhorada** - usuário consegue ler e fechar alertas
- **Botão re-habilitado corretamente** ao falhar validação
- **Modal só aparece quando gravação vai realmente acontecer**

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.2

---

## [13/01/2026 05:35] - Desabilita Funções Duplicadas de Accordion (Migração para Modal)

**Descrição**: Comentadas funções `configurarAccordions()` e `configurarInserirRequisitante()` que tentavam manipular elementos do accordion removido, causando erro JavaScript.

**Problema Crítico**:
- Erro ao carregar página: `Cannot read properties of null (reading 'contains')`
- Arquivo: `main.js`, Método: `accordion_clickOutside`, Linha: 1746
- Causa: Código tentava acessar `document.getElementById("accordionRequisitante")` que não existe mais no DOM

**Contexto**:
- HTML dos accordions de Requisitante e Evento foram removidos (commit anterior)
- main.js ainda tinha código tentando manipular esses elementos
- requisitante.service.js já gerencia toda lógica do modal de Requisitante

**Alterações Aplicadas**:

### 1. Função `configurarAccordions()` Comentada (linhas 1670-1762)
**Problemas**:
- Linha 1743: `const accordionElement = document.getElementById("accordionRequisitante");` retorna `null`
- Linha 1746: `accordionElement.contains(event.target)` → erro (null.contains)
- Tentava manipular `$("#accordionRequisitante")` inexistente

**Solução**: Função inteira comentada com aviso `⚠️ ACCORDION REMOVIDO`

### 2. Função `configurarInserirRequisitante()` Comentada (linhas 1768-1847)
**Problemas**:
- Duplicava lógica do `requisitante.service.js`
- Linha 1799: Usava ID errado `ddtSetorRequisitante` (correto é `ddtSetorNovoRequisitante`)
- Linha 1825: Tentava fechar accordion com `$("#accordionRequisitante").slideUp(300)`

**Solução**: Função inteira comentada, lógica já existe em `requisitante.service.js`

### 3. Chamadas Comentadas na Inicialização (linhas 2037, 2044)
```javascript
// ANTES:
configurarAccordions();
configurarInserirRequisitante();
configurarInserirEvento();

// DEPOIS:
// ⚠️ DESABILITADO: Accordions removidos, migrado 100% para modais Bootstrap
// configurarAccordions();

// ⚠️ DESABILITADO: Lógica movida para requisitante.service.js (modal Bootstrap)
// configurarInserirRequisitante();
configurarInserirEvento(); // ← Mantida (modal de evento funciona)
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (3 blocos comentados)
- `Documentacao/JavaScript/main.md` (v1.0 → v1.1)

**Impacto**:
- **ERRO RESOLVIDO**: Aplicação carrega sem erros JavaScript
- **Simplificação**: ~170 linhas de código accordion desabilitadas
- **Sem duplicação**: Lógica de Requisitante centralizada em `requisitante.service.js`
- **Modal funcional**: Bootstrap gerencia modais sem código customizado complexo

**Status**: ✅ **Concluído**

**Versão**: 1.1

---

## [12/01/2026 - 19:45] - Refatoração: Remoção de função configurarBotaoCancelar()

**Descrição**: Removida função `configurarBotaoCancelar()` completa e sua chamada na inicialização, pois o botão `btnCancela` foi removido do modal.

**Contexto**: O botão "Cancelar" (btnCancela) foi excluído do modal por ser redundante com "Cancelar Operação" (btnFecha). A função que configurava seu comportamento (73 linhas) foi removida.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (função removida + chamada removida)

**Mudanças Aplicadas**:

**1. Função removida (linhas 661-733)**:
```javascript
// ANTES:
function configurarBotaoCancelar()
{
    // ... 73 linhas de código
    // Configurava click handler para cancelar agendamento
    // Permitia cancelar individual ou todos recorrentes
}

// DEPOIS:
// (função completamente removida)
```

**2. Chamada removida na inicialização**:
```javascript
// ANTES:
configurarBotaoConfirmar();
configurarBotaoViagem();
configurarBotaoApagar();
configurarBotaoCancelar();  // ← Removida
configurarBotaoImprimir();

// DEPOIS:
configurarBotaoConfirmar();
configurarBotaoViagem();
configurarBotaoApagar();
configurarBotaoImprimir();
```

**Impacto**:
- ✅ Código mais limpo sem função obsoleta
- ✅ 73 linhas de código removidas
- ✅ Nenhum erro ao inicializar event handlers
- ⚠️ Funcionalidade de "cancelar agendamento" (status → Cancelada) foi removida - se necessário, implementar em outro local

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

---

## [12/01/2026] - Criação: Documentação inicial

**Descrição**: Criada documentação inicial do arquivo `main.js`.

**Status**: ✅ **Concluído**

**Responsável**: Sistema de Documentação FrotiX

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0
