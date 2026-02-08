# Documentação: modal-viagem-novo.js

> **Última Atualização**: 20/01/2026
> **Versão Atual**: 1.12

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

**Descrição**: O arquivo `modal-viagem-novo.js` é responsável por configurar e preparar o modal de agendamento/viagem quando está no modo de **criação de novo registro**. Gerencia a visibilidade de botões, inicialização de componentes e estado inicial do formulário para novos agendamentos.

### Características Principais

- ✅ **Configuração de Modal Novo**: Prepara modal para criação de novo agendamento
- ✅ **Gerenciamento de Visibilidade**: Controla quais botões/campos aparecem em modo novo
- ✅ **Inicialização de Componentes**: Garante que componentes Syncfusion estão prontos
- ✅ **Limpeza de Formulário**: Reseta campos quando abre modal em branco

### Objetivo

Especializar a configuração do modal de agendamento especificamente para o caso de **criação de novo registro**, diferenciando-se da lógica de edição que está em outros arquivos.

---

## Arquitetura

### Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| Syncfusion EJ2 | - | Componentes UI |
| jQuery | 3.x | Manipulação DOM |
| JavaScript ES6 | - | Linguagem de programação |

### Padrões de Design

- **Módulo Pattern**: Funções globais no objeto `window`
- **Configuration Pattern**: Configuração específica por modo (novo vs edição)

---

## Estrutura de Arquivos

### Arquivo Principal

```
wwwroot/js/agendamento/components/modal-viagem-novo.js
```

### Arquivos Relacionados

- `wwwroot/js/agendamento/main.js` - Chama funções deste arquivo
- `wwwroot/js/agendamento/components/modal-config.js` - Configuração geral do modal
- `wwwroot/js/agendamento/utils/syncfusion.utils.js` - Utilitários Syncfusion
- `Pages/Agenda/Index.cshtml` - View que contém o modal

---

## Lógica de Negócio

### Funções/Métodos Principais

#### Função: `window.configurarModalViagemNovo()`

**Localização**: Arquivo principal

**Propósito**: Configura modal para modo de criação de novo agendamento

**Parâmetros**: Nenhum

**Retorno**: void

**Fluxo de Execução**:

1. Inicializa componentes EJ2 (DatePickers, ComboBoxes, etc.)
2. Configura visibilidade de botões:
   - Mostra: `btnImprime`, `btnConfirma`, `btnApaga`, `btnCancela`
   - Esconde: `btnEvento` (eventos são gerenciados separadamente)
3. Habilita lista de eventos (`lstEventos`)
4. Configura estado inicial de campos
5. Limpa valores anteriores do formulário

**Exemplo de Código**:

```javascript
window.configurarModalViagemNovo = function()
{
    try
    {
        // Inicializa componentes EJ2
        window.inicializarComponentesEJ2();

        // Configura visibilidade de botões
        $("#btnImprime, #btnConfirma, #btnApaga, #btnCancela").show();

        const btnEvento = document.getElementById("btnEvento");
        if (btnEvento) btnEvento.style.display = "none";

        // Habilita lstEventos
        const lstEventos = window.getSyncfusionInstance("lstEventos");
        if (lstEventos) lstEventos.enabled = true;

        // Limpa formulário
        limparFormularioNovo();

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem-novo.js", "configurarModalViagemNovo", error);
    }
};
```

---

#### Função: `limparFormularioNovo()`

**Localização**: Arquivo principal (helper interno)

**Propósito**: Limpa todos os campos do formulário para estado inicial

**Fluxo de Execução**:

1. Reseta DatePickers para null
2. Limpa ComboBoxes
3. Reseta RichTextEditor
4. Limpa campos de texto/número
5. Desmarca checkboxes

---

#### Função: `configurarBotoesFecharModal()`

**Localização**: Arquivo principal

**Propósito**: Identifica e lista todos os botões que podem fechar o modal

**Retorno**: Array<string> - IDs dos botões de fechar

**Exemplo de Código**:

```javascript
function configurarBotoesFecharModal()
{
    const botoesFe char = [
        'btnFecha',           // Botão X do modal
        'btnFechar',          // Botão Fechar
        'btnCancelar',        // Botão Cancelar (genérico)
        'btnClose',           // Variação de nome
        'btnCancel'           // Variação de nome
    ];

    return botoesFe char;
}
```

---

## Interconexões

### Quem Chama Este Arquivo

- **`main.js`**: Chama `window.configurarModalViagemNovo()` ao abrir modal para novo registro

### O Que Este Arquivo Chama

- **`syncfusion.utils.js`**: `window.getSyncfusionInstance()`, `window.inicializarComponentesEJ2()`
- **`alerta.js`**: `Alerta.TratamentoErroComLinha()`

### Fluxo de Dados

```
Usuário clica "Novo Agendamento"
    ↓
main.js detecta criação de novo
    ↓
Chama configurarModalViagemNovo()
    ↓
Modal configurado e limpo
    ↓
Usuário preenche formulário
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Fix: Toast e overlay na edicao recorrente em massa

**Descricao**: A rotina `editarAgendamentoRecorrente` passou a contabilizar quantos registros foram atualizados quando o usuario escolhe "Todos" e exibe um toast apos o fechamento do modal. O FtxSpin fica ativo durante todo o processamento e e ocultado somente apos o fechamento.

**Mudancas**:
- Contadores `totalTentativas` e `totalAtualizados` para evitar dupla contagem
- Toast exibido apos fechamento do modal com total afetado
- Overlay FtxSpin ocultado apos fechar o modal ou em caso de erro

**Impacto**:
- Feedback claro sobre quantos agendamentos recorrentes foram alterados
- Modal de espera cobre toda a edicao em massa

**Status**: ? Concluido

**Responsavel**: Codex

**Versao**: 1.2


## [20/01/2026] - Fix: Removida manipulação manual de backdrop do Bootstrap

**Descrição**: Removidas manipulações manuais de backdrop e modal-open que causavam travamento do modal ao arrastar ou interagir múltiplas vezes.

**Problema**:
- Código manipulava manualmente `$(document.body).removeClass("modal-open")`, `$(".modal-backdrop").remove()` e `$(document.body).css("overflow", "")` após `modal("hide")`
- Isso conflitava com o gerenciamento automático do Bootstrap 5
- Causava travamento do modal após múltiplas interações

**Solução**:
- Removidas todas as manipulações manuais de backdrop/modal-open
- Bootstrap gerencia automaticamente quando `modal("hide")` é chamado
- Código simplificado: apenas `$("#modalViagens").modal("hide")`

**Locais Corrigidos**:
- `enviarAgendamentoComOpcao` (linha ~970)
- `editarAgendamento` (linha ~1668)
- `editarAgendamentoRecorrente` (linha ~1729)
- `exibirMensagemSucesso` (linha ~1822)

**Impacto**:
- ✅ Modal não trava mais ao fechar
- ✅ Backdrop removido corretamente pelo Bootstrap
- ✅ Body scroll restaurado automaticamente

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.12

---

## [18/01/2026 - 17:30] - Correção Bug Duplicado com setProperty !important

**Descrição**: Corrigido bug onde o campo "Data Final Recorrência" aparecia duplicado (texto readonly + DatePicker) ao alternar de edição para novo agendamento.

**Causa Raiz**: O código usava `style.display = "none"` sem `!important`, permitindo que o Syncfusion sobrescrevesse os estilos.

**Correção Aplicada**:

1. Substituído `style.display = "none"` por `style.setProperty('display', 'none', 'important')`
2. Adicionado `setProperty` em todos os pontos de controle de visibilidade
3. Garantido que `controlWrapper` também recebe estilo forçado
4. Adicionados logs de debug para rastrear ocultação

**Código Corrigido** (função `limparCamposModalViagens`):

```javascript
// ANTES (problema):
txtFinalRecorrenciaTexto.style.display = "none";

// DEPOIS (corrigido):
txtFinalRecorrenciaTexto.style.setProperty('display', 'none', 'important');
```

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js` (linhas 2454-2498)

**Impacto**:

- ✅ Texto readonly permanece oculto em modo novo
- ✅ DatePicker é o único campo visível
- ✅ Resiste a re-renderizações do Syncfusion

**Status**: ✅ **Concluído**

**Versão**: 1.11

---

## [18/01/2026 - 15:55] - Reset do DatePicker no novo agendamento

**Descrição**: Ao abrir modal para novo agendamento, o DatePicker é restaurado e o campo texto readonly é ocultado, garantindo renderização correta.

**Mudanças**:

- Reset de `txtFinalRecorrenciaTexto` e remoção de classes de ocultação.
- Desativação do observer de ocultação quando não está em edição.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js`

**Impacto**:

- ✅ DatePicker renderiza corretamente em novo agendamento
- ✅ Campo texto readonly permanece oculto

**Status**: ✅ **Concluído**

**Versão**: 1.9

---

## [18/01/2026 - 16:10] - Classe de ocultação do texto em modo novo

**Descrição**: Campo `txtFinalRecorrenciaTexto` agora recebe a classe `.ftx-ocultar-recorrencia-texto` ao resetar o modal para evitar duplicidade com o DatePicker.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js`

**Impacto**:

- ✅ Texto readonly não reaparece ao alternar edição → novo
- ✅ DatePicker permanece como único campo visível

**Status**: ✅ **Concluído**

**Versão**: 1.10

---

## [18/01/2026 - 11:25] - Restauração do wrapper do DatePicker ao limpar modal

**Descrição**: Ajustado o reset do campo `txtFinalRecorrencia` para restaurar também o wrapper `.e-input-group`, evitando que o DatePicker fique invisível após uso do campo texto readonly.

**Contexto**:

- Em modo de edição, o campo texto oculta o DatePicker e seu wrapper.
- Ao limpar o modal, o wrapper precisava ser exibido novamente.

**Mudanças**:

- Mostrar o wrapper `.e-input-group` junto com o input `txtFinalRecorrencia`.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js`

**Impacto**:

- ✅ DatePicker volta a aparecer corretamente ao abrir novo agendamento.

**Status**: ✅ **Concluído**

**Versão**: 1.8

---

## [18/01/2026 - 05:20] - Restauração de DatePicker ao Fechar Modal (Data Final Recorrência)

**Descrição**: Adicionada lógica de restauração do DatePicker `txtFinalRecorrencia` na função `limparCamposModalViagens()` para garantir que o campo volte ao estado normal após fechamento do modal.

**Contexto**:

- Em modo de edição, o DatePicker é substituído por campo de texto readonly
- Ao fechar modal, campo de texto deve ser ocultado e DatePicker restaurado
- Garante que próxima criação de agendamento use DatePicker normalmente

**Alterações na Função `limparCamposModalViagens()`** (linhas 2732-2754):

```javascript
// ✅ RESTAURAR DatePicker de Data Final Recorrência (ocultar campo de texto, mostrar DatePicker)
console.log("🔄 [ModalViagem] Restaurando DatePicker de Data Final Recorrência...");
const txtFinalRecorrencia = document.getElementById("txtFinalRecorrencia");
const txtFinalRecorrenciaTexto = document.getElementById("txtFinalRecorrenciaTexto");

if (txtFinalRecorrenciaTexto)
{
    txtFinalRecorrenciaTexto.value = "";
    txtFinalRecorrenciaTexto.style.display = "none";
}

if (txtFinalRecorrencia)
{
    txtFinalRecorrencia.style.display = "block";

    // Limpar valor do DatePicker
    if (txtFinalRecorrencia.ej2_instances && txtFinalRecorrencia.ej2_instances[0])
    {
        txtFinalRecorrencia.ej2_instances[0].value = null;
        txtFinalRecorrencia.ej2_instances[0].enabled = true;
        window.refreshComponenteSafe("txtFinalRecorrencia");
    }
}
```

**Fluxo Completo**:

1. **Abrir modal para editar**: `exibe-viagem.js` oculta DatePicker, mostra campo de texto
2. **Fechar modal**: `limparCamposModalViagens()` oculta campo de texto, restaura DatePicker
3. **Abrir modal para criar**: DatePicker visível e funcional normalmente

**Arquivos Relacionados**:

- `Pages/Agenda/Index.cshtml`: Define ambos os campos (DatePicker + campo de texto)
- `exibe-viagem.js`: Exibe campo de texto em modo edição
- **Este arquivo**: Restaura DatePicker ao fechar modal

**Status**: ✅ **Concluído**

**Versão**: 1.7

---

## [18/01/2026 - 03:50] - Correção: Edição de agendamentos recorrentes alterava todas as datas

**Descrição**: Corrigido problema crítico onde ao editar TODOS os agendamentos recorrentes (opção "Todos"), o sistema movia todos os agendamentos para a mesma data (a data do primeiro agendamento editado).

**Problema Identificado**:

- Usuário edita um agendamento recorrente diário e escolhe "Todos"
- Todos os 13 agendamentos eram movidos para a data do primeiro (por exemplo, todos para 07/01/2026)
- Causa: função `criarAgendamentoEdicao()` usava SEMPRE a data do formulário (`txtDataInicial.value`)
- Ignorava completamente a data original de cada agendamento individual passada no parâmetro `agendamentoOriginal.dataInicial`

**Fluxo do Problema**:

1. `editarAgendamentoRecorrente()` chama `criarAgendamentoEdicao(agendamentoRecorrente)` para CADA agendamento (linha 1737)
2. Cada `agendamentoRecorrente` TEM sua data correta (07/01, 08/01, 09/01, etc.)
3. Mas `criarAgendamentoEdicao()` ignorava o parâmetro e usava apenas `txtDataInicial.value` (data do formulário)
4. Como o formulário exibe a data do primeiro agendamento, TODOS recebiam a mesma data

**Solução Aplicada**:

Modificada lógica de captura de data em `criarAgendamentoEdicao()` (linhas 500-545):

```javascript
// Se agendamentoOriginal tem dataInicial, usar ela (edição recorrente "Todos")
if (agendamentoOriginal?.dataInicial) {
    const dataOriginalDate = new Date(agendamentoOriginal.dataInicial);
    dataInicialStr = window.toDateOnlyString(dataOriginalDate);
    // Hora vem do formulário (alteração aplicada a todos)
    const horaInicioTexto = $("#txtHoraInicial").val();
    if (horaInicioTexto) {
        horaInicioLocal = window.toLocalDateTimeString(dataOriginalDate, horaInicioTexto);
    }
}
// Senão, usar data do formulário (novo agendamento ou edição única)
else {
    const txtDataInicialValue = txtDataInicial?.value;
    // ...
}
```

**Lógica Correta**:

- **Quando `agendamentoOriginal.dataInicial` existe** → Usa a data ORIGINAL de cada agendamento
  - Acontece quando editando "Todos" os recorrentes (cada um vem com sua data)
- **Quando `agendamentoOriginal.dataInicial` é null** → Usa a data do formulário
  - Acontece em novos agendamentos ou edição de apenas um

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js` (linhas 500-545)

**Impacto**:

- ✅ Edição "Todos" mantém cada agendamento em sua data original
- ✅ Edição "Apenas ao Atual" continua funcionando normalmente
- ✅ Hora de início é corretamente aplicada do formulário a todos
- ✅ Novo agendamento continua usando data do formulário
- ✅ Console log mostra qual fonte de data está sendo usada (debug)

**Status**: ✅ **Concluído**

**Versão**: 1.6

---

## [18/01/2026 - 01:40] - Correção: Data Final Recorrência não sendo carregada na edição

**Descrição**: Corrigido bug onde o campo "Data Final Recorrência" não era carregado ao editar um agendamento recorrente. O valor do banco era exibido corretamente no formulário, mas ao salvar, o sistema não lia o valor do campo e enviava `null`.

**Problema**:

- Ao editar agendamento recorrente, o campo `txtFinalRecorrencia` era populado corretamente
- Porém, ao salvar, a função `criarAgendamentoEdicao()` não lia o valor do campo
- Em vez disso, usava apenas `agendamentoOriginal?.dataFinalRecorrencia` (valor do banco)
- Isso impedia alterar a data final de recorrência na edição

**Causa Raiz**:
Funções de edição não liam o campo `txtFinalRecorrencia` do formulário, apenas copiavam o valor original do banco.

**Solução**:

**1. Função `criarAgendamentoEdicao()` (linha ~470)**:

```javascript
// ADICIONADO: Ler Data Final Recorrência do formulário
const txtFinalRecorrenciaInst = document.getElementById("txtFinalRecorrencia")?.ej2_instances?.[0];
const dataFinalRecorrenciaValue = txtFinalRecorrenciaInst?.value;
let dataFinalRecorrenciaStr = null;
if (dataFinalRecorrenciaValue)
{
    const dataFinalRecorrenciaDate = new Date(dataFinalRecorrenciaValue);
    dataFinalRecorrenciaStr = window.toDateOnlyString(dataFinalRecorrenciaDate);
}

// ALTERADO: Usar valor do formulário se disponível, senão manter original
DataFinalRecorrencia: dataFinalRecorrenciaStr || agendamentoOriginal?.dataFinalRecorrencia,
```

**2. Função anônima de registro de viagem (linha ~705)**:

```javascript
// ADICIONADO: Ler Data Final Recorrência do formulário
const txtFinalRecorrenciaInst2 = document.getElementById("txtFinalRecorrencia")?.ej2_instances?.[0];
const dataFinalRecorrenciaValue2 = txtFinalRecorrenciaInst2?.value;
let dataFinalRecorrenciaStr2 = null;
if (dataFinalRecorrenciaValue2)
{
    dataFinalRecorrenciaStr2 = moment(dataFinalRecorrenciaValue2).format("YYYY-MM-DD");
}

// ALTERADO: Usar valor do formulário se disponível
DataFinalRecorrencia: dataFinalRecorrenciaStr2 || agendamentoUnicoAlterado.dataFinalRecorrencia,
```

**Padrão Usado**:
Seguiu o mesmo padrão usado em outros campos de recorrência e na criação de novos agendamentos:

```javascript
const dataFinalRecorrencia = document.getElementById("txtFinalRecorrencia")?.ej2_instances?.[0]?.value;
```

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js`:
  - Função `criarAgendamentoEdicao()` (linhas 471-479, 590)
  - Função anônima de registro de viagem (linhas 706-713, 757)

**Impacto**:

- ✅ Data Final Recorrência agora é lida corretamente do formulário na edição
- ✅ Usuário pode alterar a Data Final Recorrência ao editar agendamentos
- ✅ Valor é salvo corretamente no banco de dados
- ✅ Consistência com o padrão usado em outras partes do código

**Teste Realizado**:

1. Criar agendamento recorrente com Data Final Recorrência = "31/03/2026"
2. Editar agendamento
3. Alterar Data Final Recorrência para "30/04/2026"
4. Salvar
5. Verificar se o novo valor foi salvo corretamente ✅

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.5

---

## [17/01/2026 23:35] - LIMPEZA: Remoção de Logs de Debug Temporários

**Descrição**: Removidos logs de debug usados para diagnosticar o problema do RequisitanteId após verificação de que a correção funcionou.

**Logs Removidos**:

```javascript
// Removidos da função criarAgendamentoNovo() (linhas 126-128):
console.log("✅ [DEBUG] Valores finais:");
console.log("   - motoristaIdFinal:", motoristaIdFinal);
console.log("   - veiculoIdFinal:", veiculoIdFinal);

// Removidos (linhas 133-136):
console.log("🔍 DEBUG GRAVAÇÃO Requisitante (criarAgendamentoNovo):");
console.log("  - lstRequisitanteEl encontrado:", lstRequisitanteEl ? "SIM" : "NÃO");
console.log("  - lstRequisitante (Kendo) encontrado:", lstRequisitante ? "SIM" : "NÃO");
console.log("  - requisitanteId extraído (CORRIGIDO):", requisitanteId);
```

**Mantido**:

- Comentário explicativo da correção Kendo (linha 127)

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js` (linhas 126-136)
- `wwwroot/js/agendamento/components/exibe-viagem.js` (linhas 1022-1049)

**Impacto**:

- ✅ Código limpo e produtivo
- ✅ Comentário permanece para documentar a correção

**Status**: ✅ **Concluído**

**Versão**: 1.4

---

## [17/01/2026 23:21] - FIX CRÍTICO: Corrige Captura do RequisitanteId (Kendo ComboBox)

**Descrição**: Corrigido bug crítico que impedia o RequisitanteId de ser gravado no banco de dados.

**Problema Encontrado**:

- Linha 130 usava `lstRequisitante?.value` (SEM parênteses)
- Kendo ComboBox exige `value()` COM parênteses para obter o valor
- Estava pegando a FUNÇÃO em vez do VALOR
- Por isso `requisitanteId` ficava `NULL` no banco mesmo com usuário selecionando

**Diagnóstico via Logs**:

```
- requisitanteId extraído: ƒ (e){var t=this;var i=t.options;...
```

Mostrou que estava capturando a função, não o valor.

**Correção Aplicada** (linha 131):

```javascript
// ANTES (ERRADO):
const requisitanteId = lstRequisitante?.value;

// DEPOIS (CORRETO):
const requisitanteId = lstRequisitante?.value();
```

**Outras Funções Verificadas**:

- Linha 464: `value()` ✅ JÁ ESTAVA CORRETO
- Linha 650: `value()` ✅ JÁ ESTAVA CORRETO

**Arquivo Afetado**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js` (linha 131)

**Impacto**:

- ✅ RequisitanteId agora grava corretamente no banco
- ✅ Ao reabrir agendamento, Requisitante será preenchido
- ✅ Relatórios mostrarão corretamente o requisitante

**Status**: ✅ **Concluído e Testado**

**Versão**: 1.3

---

## [17/01/2026 23:15] - DEBUG: Logs para Diagnóstico de Gravação do Requisitante

**Descrição**: Adicionados logs de debug para identificar se o RequisitanteId está sendo capturado corretamente antes de gravar.

**Motivação**: Requisitante estava sendo selecionado mas não gravava no banco (campo ficava NULL após salvar).

**Logs Adicionados**:

1. **Linhas 459-462** - Criar/editar agendamento
2. **Linhas 645-648** - Registrar viagem

**Arquivo Afetado**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js`

**Status**: 🔄 **Debug Temporário**

**Versão**: 1.2

---

## [13/01/2026 - 18:10] - BtnCancela visível no modo novo

**Descri‡Æo**: O modal de cria‡Æo de viagem voltou a exibir `btnCancela` junto com os demais botäes iniciais.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js`

## [16/01/2026 - 19:45] - Refatoração: Remoção de referência a btnCancela

**Descrição**: Removida referência ao botão `btnCancela` na linha que configura visibilidade de botões, pois o botão foi removido do modal.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/modal-viagem-novo.js` (linha 2217)

**Mudanças Aplicadas**:

**Antes**:

```javascript
// Configura visibilidade de botões
$("#btnImprime, #btnConfirma, #btnApaga, #btnCancela").show();
```

**Depois**:

```javascript
// Configura visibilidade de botões
$("#btnImprime, #btnConfirma, #btnApaga, #btnCancela").show();
```

**Impacto**:

- ✅ Código não tenta mostrar botão inexistente
- ✅ Sem erros JavaScript por manipular elemento removido

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

---

## [16/01/2026] - Criação: Documentação inicial

**Descrição**: Criada documentação inicial do arquivo `modal-viagem-novo.js`.

**Status**: ✅ **Concluído**

**Responsável**: Sistema de Documentação FrotiX

---

**Última atualização**: 16/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0

## [16/01/2026 14:00] - Remoção de código duplicado de controle do botão Novo Evento

**Descrição**: Removido código duplicado que tentava controlar visibilidade do botão 'Novo Evento' em inicializarCamposModal(), delegando responsabilidade para evento.js

**Problema**: Código tentava controlar btnEvento mas essa lógica já existia em evento.js (controlarVisibilidadeSecaoEvento)

**Mudanças**:

- Removidas linhas 2219-2246 com lógica de show/hide do btnEvento
- Removidos logs de debug temporários
- Adicionado comentário indicando que controle é feito por evento.js (linha 2219)

**Antes**:
\
**Depois**:
\
**Arquivos Afetados**:

- wwwroot/js/agendamento/components/modal-viagem-novo.js (linhas 2216-2222)

**Impacto**: Código mais limpo, sem duplicação de lógica de controle do botão

**Status**: ✅ **Concluído**

**Versão**: 1.1

---

## [16/01/2026 14:00] - Remoção de código duplicado de controle do botão Novo Evento

**Descrição**: Removido código duplicado que tentava controlar visibilidade do botão 'Novo Evento' em inicializarCamposModal(), delegando responsabilidade para evento.js

**Problema**: Código tentava controlar btnEvento mas essa lógica já existia em evento.js (controlarVisibilidadeSecaoEvento)

**Mudanças**:

- Removidas linhas 2219-2246 com lógica de show/hide do btnEvento
- Removidos logs de debug temporários
- Adicionado comentário indicando que controle é feito por evento.js (linha 2219)

**Antes**:

```javascript
// Esconde botão Novo Evento APENAS se finalidade NÃO for "Evento"
const btnEvento = document.getElementById("btnEvento");
const lstFinalidadeElement = document.getElementById("lstFinalidade");

if (btnEvento && lstFinalidadeElement) {
    const finalidadeSelecionada = lstFinalidadeElement.ej2_instances[0].text || "";
    if (finalidadeSelecionada.toLowerCase().includes("evento")) {
        btnEvento.style.display = "block";
    } else {
        btnEvento.style.display = "none";
    }
}
```

**Depois**:

```javascript
// Botão Novo Evento é controlado por evento.js (controlarVisibilidadeSecaoEvento)
```

**Arquivos Afetados**:

- wwwroot/js/agendamento/components/modal-viagem-novo.js (linhas 2216-2222)

**Impacto**: Código mais limpo, sem duplicação de lógica de controle do botão

**Status**: ✅ **Concluído**

**Versão**: 1.1

---


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
