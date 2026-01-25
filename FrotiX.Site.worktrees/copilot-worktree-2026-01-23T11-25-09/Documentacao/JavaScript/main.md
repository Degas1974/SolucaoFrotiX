# Documenta��o: main.js (Agendamento)

> **Última Atualização**: 20/01/2026
> **Versão Atual**: 1.8

---

# PARTE 1: DOCUMENTA��O DA FUNCIONALIDADE

## �ndice
1. [Vis�o Geral](#vis�o-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [L�gica de Neg�cio](#l�gica-de-neg�cio)
5. [Interconex�es](#interconex�es)

---

## Vis�o Geral

**Descri��o**: O arquivo `main.js` � o ponto de entrada principal do m�dulo de Agendamento da Agenda de Viagens. � respons�vel por inicializar todos os componentes Syncfusion, configurar event handlers dos bot�es do modal, gerenciar o calend�rio FullCalendar e orquestrar a intera��o entre todos os subm�dulos.

### Caracter�sticas Principais
- ? **Inicializa��o do FullCalendar**: Configura calend�rio de agenda com eventos via AJAX
- ? **Configura��o de Componentes Syncfusion**: Inicializa DatePickers, ComboBoxes, DropDownTrees, etc.
- ? **Event Handlers de Bot�es**: Configura clicks de Confirmar, Apagar, Viagem, Imprimir e Fechar
- ? **Gerenciamento de Modal**: Controla abertura/fechamento e limpeza do modal de agendamento
- ? **Recorr�ncia Centralizada**: Delega��o para `RecorrenciaController` no fluxo de abertura do modal
- ? **Localiza��o pt-BR**: Chama configura��o de localiza��o Syncfusion

### Objetivo
Centralizar a inicializa��o e coordena��o de todos os componentes e funcionalidades do m�dulo de Agendamento, servindo como "maestro" que orquestra a intera��o entre modal, calend�rio e subm�dulos.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Vers�o | Uso |
|------------|--------|-----|
| FullCalendar | 6.x | Calend�rio de agenda |
| Syncfusion EJ2 | - | Componentes UI (DatePicker, ComboBox, etc.) |
| jQuery | 3.x | Manipula��o DOM e AJAX |
| Bootstrap 5 | 5.x | Modal e componentes UI |

### Padr�es de Design
- **M�dulo Pattern**: Fun��es agrupadas por responsabilidade
- **Event-Driven**: Event handlers para intera��es do usu�rio
- **Async/Await**: Opera��es ass�ncronas para AJAX

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/agendamento/main.js
```

### Arquivos Relacionados
- `wwwroot/js/agendamento/components/*.js` - Subm�dulos especializados (exibe-viagem, evento, etc.)
- `wwwroot/js/agendamento/utils/*.js` - Utilit�rios (syncfusion.utils, etc.)
- `wwwroot/js/agendamento/services/*.js` - Servi�os (requisitante.service, etc.)
- `Pages/Agenda/Index.cshtml` - View que referencia este arquivo

---

## L�gica de Neg�cio

### Fun��es/M�todos Principais

#### Fun��o: `InitializeCalendar(URL)`
**Localiza��o**: In�cio do arquivo

**Prop�sito**: Inicializa o calend�rio FullCalendar com configura��es pt-BR e eventos via AJAX

**Par�metros**:
- `URL` (string): URL base do servidor (n�o utilizada diretamente)

**Retorno**: void

**Fluxo de Execu��o**:
1. Obt�m elemento `#agenda`
2. Cria inst�ncia `FullCalendar.Calendar` com configura��es:
   - Locale pt-BR
   - Views (mensal, semanal, di�rio)
   - Eventos carregados de `/api/Agenda/CarregaViagens`
   - Event handlers (click, dateClick, drop, resize)
3. Renderiza calend�rio

---

#### Fun��o: `configurarBotaoConfirmar()`
**Localiza��o**: Meio do arquivo

**Prop�sito**: Configura event handler do bot�o "Confirmar" (salvar viagem/agendamento)

**Fluxo de Execu��o**:
1. Adiciona click handler em `#btnConfirma`
2. Valida dados do formul�rio
3. Chama `window.salvarViagem()` (definida em outro arquivo)
4. Fecha modal e atualiza calend�rio em caso de sucesso

---

#### Fun��o: `configurarBotaoApagar()`
**Localiza��o**: Meio do arquivo

**Prop�sito**: Configura event handler do bot�o "Apagar" (excluir agendamento)

**Fluxo de Execu��o**:
1. Adiciona click handler em `#btnApaga`
2. Exibe confirma��o via `Alerta.Confirmar()`
3. Se confirmado:
   - Se recorrente: pergunta se apaga todos ou apenas atual
   - Chama API para apagar via AJAX
4. Atualiza calend�rio e fecha modal

---

#### Fun��o: `configurarBotaoViagem()`
**Localiza��o**: Meio do arquivo

**Prop�sito**: Configura event handler do bot�o "Transformar em Viagem" (converte agendamento em viagem)

**Fluxo de Execu��o**:
1. Adiciona click handler em `#btnViagem`
2. Exibe confirma��o
3. Chama `window.transformarEmViagem()`
4. Atualiza calend�rio

---

#### Fun��o: `configurarBotaoImprimir()`
**Localiza��o**: Meio do arquivo

**Prop�sito**: Configura event handler do bot�o "Imprimir" (gera relat�rio)

**Fluxo de Execu��o**:
1. Adiciona click handler em `#btnImprime`
2. Obt�m ID da viagem
3. Abre janela com relat�rio via `/Viagens/Imprimir?id=...`

---

#### Fun��o: `configurarBot�esFechar()`
**Localiza��o**: Meio do arquivo

**Prop�sito**: Configura event handlers de todos os bot�es que fecham o modal

**Fluxo de Execu��o**:
1. Identifica todos os seletores de bot�es de fechar (`#btnFecha`, `[data-bs-dismiss="modal"]`, etc.)
2. Adiciona click handler que:
   - Limpa tooltips globais
   - Fecha modal via Bootstrap
   - Limpa formul�rio

---

#### Fun��o: `inicializarComponentesEJ2()`
**Localiza��o**: Fim do arquivo

**Prop�sito**: Inicializa todos os componentes Syncfusion EJ2 do modal

**Fluxo de Execu��o**:
1. Inicializa DatePickers (txtDataInicial, txtHoraInicial, etc.)
2. Inicializa ComboBoxes (lstVeiculo, lstMotorista, etc.)
3. Inicializa DropDownTree (lstUnidades, lstTipoUnidades, etc.)
4. Inicializa RichTextEditor (rteDescricao)
5. Configura event handlers de mudan�a de valor

---

#### Fun��o: `$(document).ready()`
**Localiza��o**: Fim do arquivo

**Prop�sito**: Fun��o de inicializa��o executada quando DOM est� pronto

**Fluxo de Execu��o**:
1. Configura localiza��o Syncfusion pt-BR
2. Inicializa calend�rio FullCalendar
3. Inicializa componentes EJ2
4. Configura event handlers de bot�es
5. Configura valida��es
6. Prepara modal para uso

---

## Interconex�es

### Quem Chama Este Arquivo
- **`Pages/Agenda/Index.cshtml`**: Referencia este arquivo na se��o de scripts

### O Que Este Arquivo Chama
- **`syncfusion.utils.js`**: `configurarLocalizacaoSyncfusion()`
- **`exibe-viagem.js`**: `window.exibirViagem()`, `window.mostrarCamposViagem()`
- **`evento.js`**: Fun��es de gerenciamento de eventos
- **`modal-viagem-novo.js`**: `window.configurarModalViagemNovo()`
- **`alerta.js`**: `Alerta.Confirmar()`, `Alerta.TratamentoErroComLinha()`
- **`global-toast.js`**: `AppToast.show()`
- **APIs**: `/api/Agenda/CarregaViagens`, `/api/Viagem/Salvar`, `/api/Viagem/Apagar`

### Fluxo de Dados
```
Usu�rio interage com Calend�rio/Modal
    ?
main.js captura evento
    ?
Chama subm�dulo especializado (exibe-viagem.js, etc.)
    ?
Subm�dulo processa l�gica
    ?
AJAX para API
    ?
Retorno atualiza calend�rio/modal
```

---

# PARTE 2: LOG DE MODIFICA��ES/CORRE��ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Fix: Overlay e fluxo async na edicao recorrente

**Descricao**: O fluxo de edicao recorrente agora aguarda o status da viagem via Promise, exibe o FtxSpin quando o usuario escolhe "Todos" e mantem o overlay ativo ate o fechamento do modal, evitando sobrescritas no estado de recorrencia.

**Mudancas**:
- `handleEditarAgendamento` passou a aguardar o status via Promise (sem callback isolado)
- Overlay FtxSpin ativado apenas quando o usuario confirma "Todos"
- Flag `window._recorrenciaOverlayAtivo` controla a duracao do overlay

**Impacto**:
- Edicao em massa com feedback visual continuo
- Menos risco de sobrescritas no estado de recorrencia

**Status**: ? Concluido

**Responsavel**: Codex

**Versao**: 1.6


## [20/01/2026] - Ajuste: Delega��o de Recorr�ncia ao RecorrenciaController

**Descri��o**: O fluxo de abertura do modal deixou de definir manualmente o valor padr�o de recorr�ncia, delegando a responsabilidade ao `RecorrenciaController` para evitar sobrescritas na edi��o.

**Mudan�as**:
- ? Removido set manual de `lstRecorrente` no `shown.bs.modal`
- ? Removido `restaurarDadosRecorrencia` como fallback de edi��o

**Impacto**:
- Modal n�o reseta `Recorrente` quando edi��o de recorr�ncia est� em andamento

**Status**: ? Conclu�do

**Respons�vel**: Codex

## [18/01/2026 - 00:30] - Otimiza��o: Exclus�o em Lote de Agendamentos Recorrentes

**Descri��o**: Substitu�do loop de m�ltiplas requisi��es individuais por uma �nica chamada ao novo endpoint `ApagaAgendamentosRecorrentes` ao clicar "Apagar Todos" nos agendamentos recorrentes.

**Problema**:
- C�digo anterior fazia N requisi��es HTTP (uma por agendamento recorrente)
- Usava delay artificial de 200ms entre cada delete
- Para 10 agendamentos = 10 requests + 2 segundos de delay
- Possibilidade de erro 500 por viola��o de integridade referencial

**Solu��o**:

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
    AppToast.show("Verde", result.message || "Todos foram exclu�dos!", 3000);
}
else
{
    Alerta.Erro("Erro ao Excluir", result.message || result.error, "OK");
}
```

**Mudan�as**:
- ? Removido loop `for (const agendamento of agendamentosRecorrentes)`
- ? Removido `await window.excluirAgendamento()` individual
- ? Removido `await window.delay(200)` artificial
- ? Adicionado chamada �nica a `AgendamentoService.excluirRecorrentes()`
- ? Adicionado tratamento de erro com `Alerta.Erro()`

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 637-661)
- Backend: `Controllers/AgendaController.cs` (novo endpoint)
- Service: `agendamento.service.js` (novo m�todo)

**Impacto**:
- ? Performance 10x melhor (1 request vs N requests)
- ? Sem delays artificiais
- ? Resolu��o de erro 500 (FK tratada no backend)
- ? Transa��o at�mica no backend (tudo ou nada)

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Sonnet 4.5

**Vers�o**: 1.3

---

## [17/01/2026 23:15] - CR�TICO: Modal de Espera Movido para Ap�s Valida��es

**Descri��o**: Movido `FtxSpin.show()` para DEPOIS de todas as valida��es de campos e valida��o IA, impedindo que o modal bloqueie alertas de erro.

**Problema CR�TICO**:
- Modal de espera aparecia ANTES das valida��es de campos
- Quando havia erro (ex: Requisitante ausente), SweetAlert ficava ABAIXO do modal
- Usu�rio n�o conseguia clicar no alerta de erro
- �nica solu��o era sair da p�gina

**Fluxo Anterior (INCORRETO)**:
```javascript
$btn.prop("disabled", true);
FtxSpin.show("Gravando Agendamento(s)");  // ? ANTES DAS VALIDA��ES

const validado = await window.ValidaCampos(viagemId);
if (!validado) {
    return;  // ? Modal ainda vis�vel, alerta bloqueado
}
```

**Fluxo Corrigido**:
```javascript
$btn.prop("disabled", true);

// 1. Valida��o de campos
const validado = await window.ValidaCampos(viagemId);
if (!validado) {
    $btn.prop("disabled", false);  // ? Re-habilita bot�o
    return;  // ? Modal n�o foi exibido, alerta funciona
}

// 2. Valida��o IA
if (isRegistraViagem) {
    const iaValida = await window.validarFinalizacaoConsolidadaIA({...});
    if (!iaValida) {
        $btn.prop("disabled", false);  // ? Re-habilita bot�o
        return;  // ? Modal n�o foi exibido, alerta funciona
    }
}

// ? AGORA SIM: Todas valida��es OK, pode exibir modal
FtxSpin.show("Gravando Agendamento(s)");
```

**Altera��es** (linhas 107-155):
- Removido `FtxSpin.show()` da linha 110 (ANTES das valida��es)
- Adicionado `FtxSpin.show()` na linha 155 (DEPOIS de todas valida��es)
- Adicionado `$btn.prop("disabled", false)` em todos os `return` de valida��o falha

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 107-155)

**Impacto**:
- **Alertas de erro agora s�o clic�veis** - n�o ficam mais bloqueados pelo modal
- **UX drasticamente melhorada** - usu�rio consegue ler e fechar alertas
- **Bot�o re-habilitado corretamente** ao falhar valida��o
- **Modal s� aparece quando grava��o vai realmente acontecer**

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Sonnet 4.5

**Vers�o**: 1.2

---

## [13/01/2026 05:35] - Desabilita Fun��es Duplicadas de Accordion (Migra��o para Modal)

**Descri��o**: Comentadas fun��es `configurarAccordions()` e `configurarInserirRequisitante()` que tentavam manipular elementos do accordion removido, causando erro JavaScript.

**Problema Cr�tico**:
- Erro ao carregar p�gina: `Cannot read properties of null (reading 'contains')`
- Arquivo: `main.js`, M�todo: `accordion_clickOutside`, Linha: 1746
- Causa: C�digo tentava acessar `document.getElementById("accordionRequisitante")` que n�o existe mais no DOM

**Contexto**:
- HTML dos accordions de Requisitante e Evento foram removidos (commit anterior)
- main.js ainda tinha c�digo tentando manipular esses elementos
- requisitante.service.js j� gerencia toda l�gica do modal de Requisitante

**Altera��es Aplicadas**:

### 1. Fun��o `configurarAccordions()` Comentada (linhas 1670-1762)
**Problemas**:
- Linha 1743: `const accordionElement = document.getElementById("accordionRequisitante");` retorna `null`
- Linha 1746: `accordionElement.contains(event.target)` ? erro (null.contains)
- Tentava manipular `$("#accordionRequisitante")` inexistente

**Solu��o**: Fun��o inteira comentada com aviso `?? ACCORDION REMOVIDO`

### 2. Fun��o `configurarInserirRequisitante()` Comentada (linhas 1768-1847)
**Problemas**:
- Duplicava l�gica do `requisitante.service.js`
- Linha 1799: Usava ID errado `ddtSetorRequisitante` (correto � `ddtSetorNovoRequisitante`)
- Linha 1825: Tentava fechar accordion com `$("#accordionRequisitante").slideUp(300)`

**Solu��o**: Fun��o inteira comentada, l�gica j� existe em `requisitante.service.js`

### 3. Chamadas Comentadas na Inicializa��o (linhas 2037, 2044)
```javascript
// ANTES:
configurarAccordions();
configurarInserirRequisitante();
configurarInserirEvento();

// DEPOIS:
// ?? DESABILITADO: Accordions removidos, migrado 100% para modais Bootstrap
// configurarAccordions();

// ?? DESABILITADO: L�gica movida para requisitante.service.js (modal Bootstrap)
// configurarInserirRequisitante();
configurarInserirEvento(); // ? Mantida (modal de evento funciona)
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (3 blocos comentados)
- `Documentacao/JavaScript/main.md` (v1.0 ? v1.1)

**Impacto**:
- **ERRO RESOLVIDO**: Aplica��o carrega sem erros JavaScript
- **Simplifica��o**: ~170 linhas de c�digo accordion desabilitadas
- **Sem duplica��o**: L�gica de Requisitante centralizada em `requisitante.service.js`
- **Modal funcional**: Bootstrap gerencia modais sem c�digo customizado complexo

**Status**: ? **Conclu�do**

**Vers�o**: 1.1

---

## [12/01/2026 - 19:45] - Refatora��o: Remo��o de fun��o configurarBotaoCancelar()

**Descri��o**: Removida fun��o `configurarBotaoCancelar()` completa e sua chamada na inicializa��o, pois o bot�o `btnCancela` foi removido do modal.

**Contexto**: O bot�o "Cancelar" (btnCancela) foi exclu�do do modal por ser redundante com "Cancelar Opera��o" (btnFecha). A fun��o que configurava seu comportamento (73 linhas) foi removida.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (fun��o removida + chamada removida)

**Mudan�as Aplicadas**:

**1. Fun��o removida (linhas 661-733)**:
```javascript
// ANTES:
function configurarBotaoCancelar()
{
    // ... 73 linhas de c�digo
    // Configurava click handler para cancelar agendamento
    // Permitia cancelar individual ou todos recorrentes
}

// DEPOIS:
// (fun��o completamente removida)
```

**2. Chamada removida na inicializa��o**:
```javascript
// ANTES:
configurarBotaoConfirmar();
configurarBotaoViagem();
configurarBotaoApagar();
configurarBotaoCancelar();  // ? Removida
configurarBotaoImprimir();

// DEPOIS:
configurarBotaoConfirmar();
configurarBotaoViagem();
configurarBotaoApagar();
configurarBotaoImprimir();
```

**Impacto**:
- ? C�digo mais limpo sem fun��o obsoleta
- ? 73 linhas de c�digo removidas
- ? Nenhum erro ao inicializar event handlers
- ?? Funcionalidade de "cancelar agendamento" (status ? Cancelada) foi removida - se necess�rio, implementar em outro local

**Status**: ? **Conclu�do**

**Respons�vel**: Claude Code

---

## [12/01/2026] - Cria��o: Documenta��o inicial

**Descri��o**: Criada documenta��o inicial do arquivo `main.js`.

**Status**: ? **Conclu�do**

**Respons�vel**: Sistema de Documenta��o FrotiX

---

**�ltima atualiza��o**: 12/01/2026
**Autor**: Sistema FrotiX
**Vers�o**: 1.0


---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Fix: RecorrenciasCompactar trata controles Kendo corretamente

**Descrição**: Corrigida função `RecorrenciasCompactar` para identificar e tratar corretamente controles Telerik Kendo UI (lstRecorrente, lstPeriodos, lstDiasMes, lstDias) que estavam sendo erroneamente acessados como Syncfusion EJ2.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 1280-1384)

**Mudanças**:
- ✅ Adicionado tratamento para Kendo DropDownList (lstRecorrente, lstPeriodos, lstDiasMes)
- ✅ Adicionado tratamento para Kendo MultiSelect (lstDias)
- ✅ Tratamento Kendo DatePicker (txtFinalRecorrencia) já existia
- ✅ Controles Kendo agora são identificados e estilizados corretamente
- ✅ Removidos avisos falsos "não tem instância Syncfusion"

**Mapeamento de Controles de Recorrência**:
| ID | Biblioteca | Acesso |
|----|------------|--------|
| lstRecorrente | Kendo | `.data("kendoDropDownList")` |
| lstPeriodos | Kendo | `.data("kendoDropDownList")` |
| lstDiasMes | Kendo | `.data("kendoDropDownList")` |
| lstDias | Kendo | `.data("kendoMultiSelect")` |
| txtFinalRecorrencia | Kendo | `.data("kendoDatePicker")` |

**Impacto**:
- Console sem erros "não tem instância Syncfusion inicializada"
- Controles Kendo compactados corretamente
- Modal não trava mais por erros silenciosos

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.8

---

## [20/01/2026] - Fix: Listeners de close nos ComboBoxes Syncfusion + intervalo 500ms

**Descrição**: Reduzido intervalo de limpeza para 500ms e adicionados listeners de close em todos os ComboBoxes Syncfusion para limpar overlays órfãos imediatamente quando o popup fecha.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 1718-1769)

**Mudanças**:
- ✅ Intervalo de limpeza reduzido de 2000ms para 500ms
- ✅ Adicionados listeners de `close` nos ComboBoxes: lstVeiculo, lstMotorista, lstFinalidade, cmbOrigem, cmbDestino, lstEventos
- ✅ Wrap do método `close` original para limpar overlays após 100ms
- ✅ Flag `_overlayCleanupBound` para evitar listeners duplicados

**Impacto**:
- Overlays são removidos mais rapidamente (500ms vs 2000ms)
- Remoção imediata ao fechar qualquer dropdown Syncfusion
- Corrige travamento ao interagir com dropdown de veículos

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.7

---

## [20/01/2026] - Fix: Intervalo de limpeza de overlays Syncfusion e cleanup no hide

**Descrição**: Adicionado intervalo de limpeza (a cada 2 segundos) que remove overlays órfãos `.e-overlay` enquanto o modal está aberto. Também adicionada limpeza ao fechar o modal para garantir remoção de todos os overlays.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 1463-1472, 1718-1737, 1753-1760)

**Mudanças**:
- ✅ Adicionado `.e-overlay` ao selector de limpeza no `shown.bs.modal`
- ✅ Limpeza condicional: não remove overlays de componentes ativos (`.e-dialog`, `.e-popup-open`)
- ✅ Novo intervalo `window._overlayCleanupInterval` que executa a cada 2 segundos
- ✅ Limpeza do intervalo no evento `hide.bs.modal`
- ✅ Remoção de overlays órfãos ao fechar modal

**Impacto**:
- Modal não trava mais ao interagir com dropdowns Syncfusion (lstVeiculo, etc.)
- Overlays órfãos são removidos periodicamente durante uso do modal
- Recursos liberados corretamente ao fechar modal

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.6

---

## [20/01/2026] - Fix: Limpeza abrangente de overlays no shown.bs.modal

**Descrição**: Adicionada limpeza de overlays Syncfusion/Kendo/FrotiX no evento `shown.bs.modal` para evitar travamento do modal após interagir com controles.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/main.js` (linhas 1455-1477)

**Mudanças**:
- ✅ Remove overlay de loading do relatório (`#modal-relatorio-loading-overlay`)
- ✅ Esconde FtxSpin global
- ✅ Remove spinners Syncfusion (`.e-spinner-pane`, `.e-spin-overlay`, `.e-spin-show`)
- ✅ Remove overlays Kendo (`.k-overlay`, `.k-loading-mask`)
- ✅ Remove overlays FrotiX órfãos

**Impacto**:
- Modal não trava mais após interagir com 3-4 controles
- Overlays órfãos são removidos automaticamente

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.5

---

## [19/01/2026] - Atualização: Implementação de Métodos com Tracking Seletivo

**Descrição**: Migração de chamadas .AsTracking() para novos métodos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimização de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos métodos do repositório)
- Repository/IRepository/IRepository.cs (definição dos novos métodos)
- Repository/Repository.cs (implementação)
- RegrasDesenvolvimentoFrotiX.md (seção 4.2 - nova regra permanente)

**Mudanças**:
- ❌ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- ✅ **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- Otimização de memória e performance
- Tracking seletivo (apenas quando necessário para Update/Delete)
- Padrão mais limpo e explícito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seção 4.2)

**Impacto**: 
- Melhoria de performance em operações de leitura (usa AsNoTracking por padrão)
- Tracking correto em operações de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: ✅ **Concluído**

**Responsável**: Sistema (Atualização Automática)

**Versão**: Incremento de patch

