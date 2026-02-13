/* ****************************************************************************************
 * ⚡ ARQUIVO: requisitante.service.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Service completo para gerenciamento de Requisitantes (solicitantes
 *                   de viagens) do sistema FrotiX. Combina SERVICE LAYER (RequisitanteService
 *                   class com 2 métodos CRUD: adicionar POST, listar GET) e UI LAYER
 *                   completa (Bootstrap Modal para cadastro, validações de campos, Syncfusion
 *                   DropDownTree para setores). Total de 17 funções: 2 service methods +
 *                   15 UI functions (modal open/close, form validation, field sanitization,
 *                   AJAX save). IIFE wrapper com "use strict". Debug logging extensivo.
 * 📥 ENTRADAS     : adicionar(dados: {Nome, Ponto, Ramal, Email, SetorSolicitanteId}),
 *                   listar() sem params, UI functions variadas (elementos DOM, eventos)
 * 📤 SAÍDAS       : adicionar retorna {success, message, requisitanteId}, listar retorna
 *                   Promise<{success, data: Array<{RequisitanteId, Requisitante}>}>, UI
 *                   functions manipulam DOM (show/hide modal, populate dropdowns, validate
 *                   fields, update Kendo ComboBox lstRequisitante)
 * 🔗 CHAMADA POR  : Event handlers (onSelectRequisitante em event-handlers.js), exibe-viagem.js
 *                   (ExibeViagem, preencherCamposParaEdicao), Bootstrap modal triggers
 *                   (data-bs-toggle="modal" em botão Novo Requisitante), controls-init.js
 *                   (inicializarEventHandlersControles), modal close events
 * 🔄 CHAMA        : ApiClient.post('/api/Viagem/AdicionarRequisitante'), $.ajax GET
 *                   (/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes), $.ajax POST
 *                   (salvarNovoRequisitante), criarErroAjax, Alerta.TratamentoErroComLinha,
 *                   Alerta.Alerta, Alerta.Warning, Alerta.Erro, AppToast.show, toastr,
 *                   bootstrap.Modal (show/hide/getInstance), Syncfusion DropDownTree
 *                   (new ej.dropdowns.DropDownTree, destroy, dataBind, value setter),
 *                   Kendo ComboBox (getRequisitanteCombo, setDataSource, value), DOM
 *                   methods (getElementById, querySelector, addEventListener, classList,
 *                   cloneNode, replaceChild), Array methods (map, push, sort, some, slice),
 *                   String methods (trim, toLowerCase, replace, substring, split, join),
 *                   JSON.stringify, parseInt, setTimeout, console logging
 * 📦 DEPENDÊNCIAS : ApiClient (window.ApiClient), jQuery ($.ajax), criarErroAjax
 *                   (ajax-helper.js), Alerta (frotix-core.js), AppToast/toastr (toast
 *                   notifications), Bootstrap 5 Modal (bootstrap.Modal, data-bs-toggle,
 *                   shown.bs.modal event), Syncfusion EJ2 DropDownTree (ej.dropdowns.
 *                   DropDownTree, getSyncfusionInstance bridge), Kendo UI ComboBox (lstRequisitante,
 *                   getRequisitanteCombo function), DOM elements (#txtPonto, #txtNome,
 *                   #txtRamal, #txtEmail, #ddtSetorNovoRequisitante, #hiddenSetorId,
 *                   #lstSetorRequisitanteAgendamento, #lstSetorRequisitanteEvento,
 *                   #txtRamalRequisitanteSF, #modalNovoRequisitante, #modalViagens,
 *                   #btnInserirRequisitante, #lstRequisitante), window.SETORES_DATA
 *                   (global array), Razor Pages endpoints (/Viagens/Upsert handler,
 *                   /api/Viagem/AdicionarRequisitante)
 * 📝 OBSERVAÇÕES  : Exporta window.RequisitanteService (singleton instance), window.
 *                   inicializarSistemaRequisitante, window.resetarSistemaRequisitante,
 *                   window.configurarBotaoNovoRequisitante, window.abrirFormularioCadastro
 *                   Requisitante, window.fecharFormularioCadastroRequisitante, window.
 *                   limparCamposCadastroRequisitante, window.salvarNovoRequisitante, window.
 *                   capturarDadosSetores, window.inicializarDropDownTreeModal (9 exports).
 *                   IIFE wrapper ((function(){ "use strict"; ... })()) com isolation. Debug
 *                   tracking: window.requisitanteServiceLoadCount incrementado em cada load.
 *                   Proteção contra dupla inicialização: window.requisitanteServiceInicializado
 *                   flag. Bootstrap Modal stacking: z-index 1060 para modal filho, 1059 para
 *                   backdrop. Syncfusion DropDownTree recriado em shown.bs.modal event (fix
 *                   popup rendering issue). Campo Ponto auto-prefixo "p_" (blur validation).
 *                   Campo Email auto-sufixo "@camara.leg.br" (blur validation). Campo Nome
 *                   auto Camel Case com conectores (de, da, do, das, dos, e) lowercase.
 *                   Campo Ramal: apenas números, 8 dígitos, começa com 1-9. Validação flag
 *                   estaValidando previne cliques durante validação (bloqueio seletivo).
 *                   Kendo ComboBox lstRequisitante atualizado após save (add + sort alfabético).
 *                   Auto-inicialização: DOMContentLoaded listener para inicializarDropDownTree
 *                   Modal. Pattern comum: clone button + replaceChild para remover listeners
 *                   antigos. Try-catch completo com TratamentoErroComLinha em todas as functions.
 *                   Console logging extremamente detalhado (🔄🆕✅❌⚠️📦🔍 emojis). Usa
 *                   ApiClient em adicionar, $.ajax direto em listar e salvarNovoRequisitante
 *                   (inconsistência de pattern). DropDownTree.value espera array ([value])
 *                   para seleção. Modal Bootstrap usa static backdrop + keyboard:false para
 *                   evitar fechamento acidental. Sanitização de nome: Unicode letters/numbers
 *                   only, max 80 chars. Ramal max 8 dígitos, Ponto max 50 chars (com "p_").
 *
 * 📋 ÍNDICE DE FUNÇÕES (17 funções: 2 service + 15 UI, 9 exports window.*):
 *
 * ┌─ CLASS RequisitanteService (SERVICE LAYER) ────────────────────────┐
 * │ constructor()                                                       │
 * │ → Inicializa this.api = window.ApiClient                           │
 * │ → Singleton instance: window.RequisitanteService                   │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ async adicionar(dados: Object) ───────────────────────────────────┐
 * │ → Adiciona novo requisitante via API POST                          │
 * │ → param dados: {Nome, Ponto, Ramal, Email, SetorSolicitanteId}    │
 * │ → returns Promise<{success, message, requisitanteId}> ou           │
 * │           {success: false, message/error}                          │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. await this.api.post('/api/Viagem/AdicionarRequisitante',dados)│
 * │   3. if response.success: return {success: true, message,           │
 * │      requisitanteId: response.requisitanteid}                       │
 * │   4. else: return {success: false, message || "Erro ao adicionar"}  │
 * │   5. catch: Alerta.TratamentoErroComLinha + return {success: false, │
 * │      error: error.message}                                          │
 * │ → Não lança exceção, sempre retorna objeto com success flag        │
 * │ → Usado internamente ou externamente para add requisitante         │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ async listar() ────────────────────────────────────────────────────┐
 * │ → Lista todos os requisitantes do sistema                          │
 * │ → returns Promise<{success: true, data: Array<{RequisitanteId,     │
 * │           Requisitante}>}> ou reject(erro)                          │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. return new Promise((resolve, reject) => {...})                 │
 * │   3. $.ajax GET /Viagens/Upsert?handler=AJAXPreencheListaRequisitantes│
 * │   4. success callback:                                              │
 * │      a. const requisitantes = res.data.map(item => ({               │
 * │           RequisitanteId: item.requisitanteId, Requisitante:        │
 * │           item.requisitante }))                                     │
 * │      b. resolve({success: true, data: requisitantes})               │
 * │   5. error callback:                                                │
 * │      a. const erro = criarErroAjax(jqXHR, textStatus, errorThrown,  │
 * │           this)                                                     │
 * │      b. Alerta.TratamentoErroComLinha("requisitante.service.js",    │
 * │           "listar", erro)                                           │
 * │      c. reject(erro)                                                │
 * │   6. catch outer: TratamentoErroComLinha + return {success: false,  │
 * │      error, data: []}                                               │
 * │ → Mapeia camelCase backend para PascalCase Syncfusion              │
 * │ → Pattern Promise wrapper around jQuery.ajax                       │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ UI LAYER (15 functions dentro de IIFE) ───────────────────────────┐
 * │                                                                     │
 * │ 1. capturarDadosSetores()                                           │
 * │    → Captura dados de setores de outros dropdowns já carregados    │
 * │    → Tenta lstSetorRequisitanteAgendamento via getSyncfusionInstance│
 * │      bridge, acessa .fields.dataSource                              │
 * │    → Fallback: lstSetorRequisitanteEvento                           │
 * │    → Popula window.SETORES_DATA (global array)                     │
 * │    → returns boolean (true se capturado, false se falhou)          │
 * │    → Console.log quantidade de itens capturados                     │
 * │                                                                     │
 * │ 2. inicializarSistemaRequisitante()                                 │
 * │    → Inicializa sistema de requisitante ao abrir modal             │
 * │    → Proteção: verifica window.requisitanteServiceInicializado flag │
 * │    → Marca flag como true imediatamente (previne race conditions)  │
 * │    → Chama configurarBotoesCadastroRequisitante()                   │
 * │    → Comentário: accordion code DESABILITADO, agora usa Bootstrap   │
 * │      Modal                                                          │
 * │    → Incrementa inicializacaoCount (debug)                          │
 * │    → Export: window.inicializarSistemaRequisitante                  │
 * │                                                                     │
 * │ 3. configurarBotaoNovoRequisitante()                                │
 * │    → Configura botão "Novo Requisitante" (toggle accordion - DEPRECATED)│
 * │    → Comentário: agora usa data-bs-toggle="modal" em HTML          │
 * │    → Clone button + replaceChild para remover listeners antigos    │
 * │    → Click listener com estaValidando e isProcessing flags         │
 * │    → Toggle sectionCadastroRequisitante display (none/block)        │
 * │    → Chama abrirFormularioCadastroRequisitante ou fechar           │
 * │    → Export: window.configurarBotaoNovoRequisitante                │
 * │                                                                     │
 * │ 4. abrirFormularioCadastroRequisitante()                            │
 * │    → Abre Bootstrap Modal #modalNovoRequisitante                    │
 * │    → Fluxo:                                                         │
 * │      1. limparCamposCadastroRequisitante()                          │
 * │      2. bootstrap.Modal.getInstance ou new bootstrap.Modal({        │
 * │         backdrop: 'static', keyboard: false })                      │
 * │      3. modalInstance.show()                                        │
 * │      4. setTimeout 150ms: ajusta z-index modal (1060) e backdrop    │
 * │         (1059) para stacking correto                                │
 * │      5. shown.bs.modal event listener:                              │
 * │         a. setTimeout 100ms                                         │
 * │         b. capturarDadosSetores() se window.SETORES_DATA vazio      │
 * │         c. Destroy old ddtSetorNovoRequisitante via bridge          │
 * │         d. new ej.dropdowns.DropDownTree com 8 event handlers       │
 * │            (open ajusta z-index 1060, select stopPropagation,       │
 * │             blur, close, created, dataBound)                        │
 * │         e. dropdown.appendTo(ddtSetor)                              │
 * │         f. removeEventListener após executar (once: true)           │
 * │    → Try-catch com TratamentoErroComLinha                           │
 * │    → Export: window.abrirFormularioCadastroRequisitante            │
 * │                                                                     │
 * │ 5. fecharFormularioCadastroRequisitante()                           │
 * │    → Fecha Bootstrap Modal via bootstrap.Modal.getInstance().hide() │
 * │    → Reset isProcessing flag                                        │
 * │    → Try-catch com TratamentoErroComLinha                           │
 * │    → Export: window.fecharFormularioCadastroRequisitante           │
 * │                                                                     │
 * │ 6. limparCamposCadastroRequisitante()                               │
 * │    → Limpa campos #txtPonto, #txtNome, #txtRamal, #txtEmail        │
 * │    → Limpa #ddtSetorNovoRequisitante via getSyncfusionInstance      │
 * │    → dataBind() para refresh                                        │
 * │    → Console logging detalhado (stack trace, dataSource length)     │
 * │    → Try-catch (sem throw, apenas console.error)                    │
 * │    → Export: window.limparCamposCadastroRequisitante               │
 * │                                                                     │
 * │ 7. configurarValidacaoPonto()                                       │
 * │    → Configura validação de campo #txtPonto                         │
 * │    → Clone + replaceChild para limpar listeners                     │
 * │    → blur event:                                                    │
 * │      a. trim valor                                                  │
 * │      b. if length > 50: Alerta.Warning + substring(0, 50)          │
 * │      c. if começa com "P_": converte para "p_" (lowercase)          │
 * │      d. if não começa com "p_": adiciona "p_" no início             │
 * │      e. verificar length > 50 novamente após adicionar "p_"         │
 * │      f. atualizar campo.value                                       │
 * │    → Try-catch com TratamentoErroComLinha                           │
 * │    → Console.log "✅ Validação de Ponto configurada"                │
 * │                                                                     │
 * │ 8. toCamelCase(str)                                                 │
 * │    → Converte string para Camel Case com conectores lowercase      │
 * │    → param str: string para converter                               │
 * │    → returns string em Camel Case                                   │
 * │    → Conectores: ['de', 'da', 'do', 'das', 'dos', 'e']             │
 * │    → Fluxo:                                                         │
 * │      1. toLowerCase()                                               │
 * │      2. split(' ')                                                  │
 * │      3. filter(palavra => palavra.length > 0)                       │
 * │      4. map((palavra, index) => {                                   │
 * │           if (index === 0 || !conectores.includes(palavra))         │
 * │             return palavra.charAt(0).toUpperCase() + palavra.slice(1)│
 * │           return palavra })                                         │
 * │      5. join(' ')                                                   │
 * │    → Exemplo: "MARIA DA SILVA" → "Maria da Silva"                  │
 * │                                                                     │
 * │ 9. sanitizeNomeCompleto(valor)                                      │
 * │    → Remove caracteres inválidos e limita a 80 caracteres          │
 * │    → param valor: string para sanitizar                             │
 * │    → returns string sanitizada                                      │
 * │    → Regex: /[^\p{L}\p{N} ]+/gu (remove tudo exceto Unicode letters,│
 * │      numbers, espaços)                                              │
 * │    → substring(0, 80) se exceder                                    │
 * │    → Usado em validação de Nome (input e blur events)              │
 * │                                                                     │
 * │ 10. configurarValidacoesRequisitante()                              │
 * │     → Configura validações de Ramal, Email, Nome                    │
 * │     → RAMAL:                                                        │
 * │       - input event: replace(/\D/g, ''), substring(0, 8)            │
 * │       - blur event: regex /^[1-9]\d{7}$/ (8 dígitos, começa 1-9)   │
 * │       - is-invalid class se não passar                              │
 * │     → EMAIL:                                                        │
 * │       - blur event: remove @camara.leg.br, remove @, remove chars   │
 * │         inválidos, adiciona @camara.leg.br obrigatório              │
 * │       - regex /^[a-z0-9._-]+@camara\.leg\.br$/ final                │
 * │       - input event: toLowerCase, limita 1 @                        │
 * │     → NOME:                                                         │
 * │       - input event: sanitizeNomeCompleto                           │
 * │       - blur event: sanitize + toCamelCase, valida não vazio        │
 * │     → Clone + replaceChild para todos os campos                     │
 * │     → Try-catch com TratamentoErroComLinha em cada listener         │
 * │                                                                     │
 * │ 11. configurarBotoesCadastroRequisitante()                          │
 * │     → Configura botões do formulário                                │
 * │     → Chama: configurarValidacaoPonto()                             │
 * │     → Chama: configurarValidacoesRequisitante()                     │
 * │     → Configura #btnInserirRequisitante click: salvarNovoRequisitante│
 * │     → Clone + replaceChild, preventDefault, stopPropagation,        │
 * │       stopImmediatePropagation, capture phase (true)                │
 * │     → Comentário: btnFecharAccordionRequisitante DESABILITADO (usa  │
 * │       data-bs-dismiss="modal" em HTML)                              │
 * │                                                                     │
 * │ 12. salvarNovoRequisitante()                                        │
 * │     → Salva novo requisitante via AJAX POST                         │
 * │     → Fluxo:                                                        │
 * │       1. Obter campos: txtPonto, txtNome, txtRamal, txtEmail,       │
 * │          hiddenSetorId (TreeView hidden field)                      │
 * │       2. Validações com estaValidando = true:                       │
 * │          a. Ponto obrigatório                                       │
 * │          b. Nome obrigatório                                        │
 * │          c. Ramal obrigatório                                       │
 * │          d. Setor obrigatório (hiddenSetorId.value)                 │
 * │          e. Se falha: setTimeout 2000ms reset flag, Alerta.Alerta,  │
 * │             focus, return                                           │
 * │       3. Montar objRequisitante: {Nome, Ponto, Ramal: parseInt,     │
 * │          Email, SetorSolicitanteId: toString}                       │
 * │       4. $.ajax POST /api/Viagem/AdicionarRequisitante              │
 * │       5. success callback:                                          │
 * │          a. if data.success: AppToast.show ou toastr.success        │
 * │          b. Atualizar Kendo ComboBox lstRequisitante:               │
 * │             - getRequisitanteCombo()                                │
 * │             - novoItem = {RequisitanteId, Requisitante: "nome - ponto"}│
 * │             - dataSource = comboRequisitante.dataSource.data()      │
 * │             - push(novoItem) se não existe                          │
 * │             - sort alfabético (localeCompare pt-BR)                 │
 * │             - setDataSource(dataSource)                             │
 * │             - value(requisitanteId)                                 │
 * │          c. Atualizar #txtRamalRequisitanteSF.value                 │
 * │          d. Atualizar lstSetorRequisitanteAgendamento via bridge    │
 * │             [0].value = [setorValue] (array), dataBind()            │
 * │          e. bootstrap.Modal.getInstance().hide()                    │
 * │          f. limparCamposCadastroRequisitante()                      │
 * │          else: AppToast/toastr/Alerta.Erro com mensagem             │
 * │       6. error callback: Alerta.Erro, TratamentoErroComLinha        │
 * │       7. catch outer: estaValidando = false, TratamentoErroComLinha │
 * │     → Try-catch completo com logging detalhado                      │
 * │     → Export: window.salvarNovoRequisitante                         │
 * │                                                                     │
 * │ 13. resetarSistemaRequisitante()                                    │
 * │     → Reseta sistema ao fechar modal                                │
 * │     → window.requisitanteServiceInicializado = false                │
 * │     → fecharFormularioCadastroRequisitante()                        │
 * │     → limparCamposCadastroRequisitante()                            │
 * │     → Disconnect window.__accordionObserver se existir              │
 * │     → Export: window.resetarSistemaRequisitante                     │
 * │                                                                     │
 * │ 14. inicializarDropDownTreeModal()                                  │
 * │     → Inicializa DropDownTree quando modal é exibido               │
 * │     → Listener shown.bs.modal em #modalNovoRequisitante            │
 * │     → Fluxo:                                                        │
 * │       1. getElementById("ddtSetorNovoRequisitante")                 │
 * │       2. if !window.SETORES_DATA: capturarDadosSetores()            │
 * │       3. if ainda vazio: setTimeout 500ms retry captura             │
 * │       4. criarDropDownTree(ddtSetor)                                │
 * │     → Auto-inicialização: DOMContentLoaded ou immediate se ready    │
 * │     → Export: window.inicializarDropDownTreeModal                   │
 * │                                                                     │
 * │ 15. criarDropDownTree(elemento)                                     │
 * │     → Cria DropDownTree no elemento fornecido                       │
 * │     → param elemento: DOM element para appendTo                     │
 * │     → Fluxo:                                                        │
 * │       1. if getSyncfusionInstance(id) exists: destroy()             │
 * │       2. new ej.dropdowns.DropDownTree({                            │
 * │          fields: {dataSource: SETORES_DATA, value: 'SetorSolicitanteId',│
 * │            text: 'Nome', parentValue: 'SetorPaiId', hasChildren:    │
 * │            'HasChild'},                                             │
 * │          allowFiltering: true, placeholder, sortOrder: 'Ascending', │
 * │          showCheckBox: false, filterType: 'Contains', popupHeight:  │
 * │          '200px', popupWidth: '100%', width: '100%',                │
 * │          created, dataBound callbacks })                            │
 * │       3. dropdown.appendTo(elemento)                                │
 * │     → Try-catch com TratamentoErroComLinha                          │
 * │     → Console.log quantidade de itens carregados                    │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO DE ADICIONAR REQUISITANTE:
 * 1. Usuário clica em botão "Novo Requisitante" (data-bs-toggle="modal"
 *    target="#modalNovoRequisitante")
 * 2. Bootstrap Modal abre #modalNovoRequisitante
 * 3. shown.bs.modal event dispara: DropDownTree é recriado (fix rendering)
 * 4. capturarDadosSetores() popula window.SETORES_DATA de outros dropdowns
 * 5. new ej.dropdowns.DropDownTree criado com SETORES_DATA, z-index 1060
 * 6. Usuário preenche: Ponto (auto "p_" prefix), Nome (auto Camel Case),
 *    Ramal (8 dígitos), Email (auto @camara.leg.br), Setor (TreeView)
 * 7. Usuário clica "Salvar" (#btnInserirRequisitante)
 * 8. salvarNovoRequisitante() executa:
 *    a. Validações com estaValidando flag (Alerta.Alerta se falha)
 *    b. $.ajax POST /api/Viagem/AdicionarRequisitante
 *    c. Success: atualiza Kendo ComboBox lstRequisitante (add + sort)
 *    d. Atualiza campos Ramal e Setor do formulário principal
 *    e. Fecha modal via bootstrap.Modal.getInstance().hide()
 *    f. limparCamposCadastroRequisitante()
 * 9. Novo requisitante aparece selecionado em lstRequisitante (Kendo)
 * 10. onSelectRequisitante handler dispara (event-handlers.js) para
 *     fazer 2 AJAX calls paralelos (buscar ramal e setor)
 *
 * 🔄 FLUXO DE VALIDAÇÃO DE CAMPOS:
 * - PONTO (blur): adiciona "p_" se não tem, converte "P_" → "p_", trunca 50 chars
 * - NOME (input): sanitizeNomeCompleto (Unicode letters/numbers/spaces, 80 chars)
 * - NOME (blur): toCamelCase (primeira palavra + não-conectores uppercase first letter)
 * - RAMAL (input): remove não-dígitos, limita 8 chars
 * - RAMAL (blur): regex /^[1-9]\d{7}$/, adiciona is-invalid se falha
 * - EMAIL (input): toLowerCase, remove chars inválidos, limita 1 @
 * - EMAIL (blur): remove @camara.leg.br existente, remove @, sanitiza, adiciona
 *   @camara.leg.br obrigatório, regex /^[a-z0-9._-]+@camara\.leg\.br$/
 * - SETOR: hiddenSetorId preenchido por TreeView, validado em salvar
 *
 * 🔄 FLUXO DE STACKING DE MODAIS (Bootstrap):
 * 1. Modal pai: #modalViagens (z-index padrão 1055, backdrop 1050)
 * 2. Modal filho: #modalNovoRequisitante
 * 3. abrirFormularioCadastroRequisitante():
 *    a. bootstrap.Modal({backdrop: 'static', keyboard: false})
 *    b. modalInstance.show()
 *    c. setTimeout 150ms:
 *       - modalElement.style.zIndex = '1060'
 *       - ultimo backdrop.style.zIndex = '1059'
 * 4. DropDownTree popup: open event ajusta args.popup.element.style.zIndex = '1060'
 * 5. Modal filho fecha: backdrop removido automaticamente, modal pai permanece aberto
 *
 * 📌 ESTRUTURA DE DADOS (adicionar):
 * Request:
 * {
 *   "Nome": "Maria da Silva",
 *   "Ponto": "p_12345",
 *   "Ramal": 12345678,
 *   "Email": "maria.silva@camara.leg.br",
 *   "SetorSolicitanteId": "42"
 * }
 *
 * Response:
 * {
 *   "success": true,
 *   "message": "Requisitante adicionado com sucesso",
 *   "requisitanteid": 123
 * }
 *
 * 📌 ESTRUTURA DE DADOS (listar):
 * Backend response:
 * {
 *   "data": [
 *     { "requisitanteId": 1, "requisitante": "João Silva - p_11111" },
 *     { "requisitanteId": 2, "requisitante": "Maria Santos - p_22222" }
 *   ]
 * }
 *
 * Após map (PascalCase):
 * [
 *   { "RequisitanteId": 1, "Requisitante": "João Silva - p_11111" },
 *   { "RequisitanteId": 2, "Requisitante": "Maria Santos - p_22222" }
 * ]
 *
 * 📌 DOM ELEMENTS DEPENDENCY (18 elements):
 * - #txtPonto: input text para Ponto do requisitante
 * - #txtNome: input text para Nome do requisitante
 * - #txtRamal: input text para Ramal (8 dígitos)
 * - #txtEmail: input text para Email (@camara.leg.br)
 * - #ddtSetorNovoRequisitante: Syncfusion DropDownTree para setor (modal)
 * - #hiddenSetorId: input hidden preenchido por TreeView com setor selecionado
 * - #lstSetorRequisitanteAgendamento: Syncfusion DropDownTree (form principal)
 * - #lstSetorRequisitanteEvento: Syncfusion DropDownTree (fallback para captura)
 * - #txtRamalRequisitanteSF: input text Ramal no form principal (atualizado após save)
 * - #modalNovoRequisitante: Bootstrap Modal para cadastro de requisitante
 * - #modalViagens: Bootstrap Modal pai (viagens)
 * - #btnInserirRequisitante: button Salvar no modal de requisitante
 * - #lstRequisitante: Kendo ComboBox para seleção de requisitante (form principal)
 * - #sectionCadastroRequisitante: section do formulário (usado em accordion DEPRECATED)
 * - #btnRequisitante: button "Novo Requisitante" (usado em accordion DEPRECATED)
 * - .modal-backdrop: Bootstrap backdrops (z-index ajustado)
 * - .swal2-container: SweetAlert container (permitido durante estaValidando)
 * - #accordionRequisitante: accordion container (DEPRECATED, agora usa modal)
 *
 * 📌 GLOBAL VARIABLES:
 * - window.RequisitanteService: singleton instance da classe
 * - window.SETORES_DATA: array de setores capturado de outros dropdowns
 * - window.requisitanteServiceInicializado: flag de inicialização (boolean)
 * - window.requisitanteServiceLoadCount: contador de cargas do script (debug)
 * - window.__accordionObserver: MutationObserver (DEPRECATED, accordion removido)
 * - window.globalClickListener: global click listener (DEPRECATED)
 * - estaValidando: flag interna para bloqueio durante validação (boolean)
 * - isProcessing: flag interna para evitar duplo clique (boolean)
 * - inicializacaoCount: contador interno de inicializações (debug)
 *
 * 📌 EXPORTS (9 window.* functions):
 * 1. window.RequisitanteService (singleton instance)
 * 2. window.inicializarSistemaRequisitante
 * 3. window.resetarSistemaRequisitante
 * 4. window.configurarBotaoNovoRequisitante
 * 5. window.abrirFormularioCadastroRequisitante
 * 6. window.fecharFormularioCadastroRequisitante
 * 7. window.limparCamposCadastroRequisitante
 * 8. window.salvarNovoRequisitante
 * 9. window.capturarDadosSetores
 * 10. window.inicializarDropDownTreeModal
 *
 * 📌 TRATAMENTO DE ERROS:
 * - Todos os 17 métodos/funções têm try-catch
 * - TratamentoErroComLinha em todos os catch blocks
 * - criarErroAjax em $.ajax error callbacks
 * - Alerta.Alerta/Warning/Erro para feedback ao usuário
 * - AppToast.show ou toastr.success/error para notificações
 * - Console logging extensivo com emojis (🔄🆕✅❌⚠️📦🔍)
 * - estaValidando flag com setTimeout 2000ms reset em caso de validação falha
 * - Silent return em algumas funções se elementos não existem
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE wrapper: (function(){ "use strict"; ... })() para isolation
 * - Debug tracking: window.requisitanteServiceLoadCount incrementado, timestamp logged
 * - Pattern inconsistency: ApiClient em adicionar, $.ajax em listar e salvar
 * - Clone + replaceChild pattern usado em todos os event listeners para remover
 *   listeners antigos (evita múltiplos binds)
 * - Capture phase (true) em alguns addEventListener para prioridade
 * - preventDefault, stopPropagation, stopImmediatePropagation em botões críticos
 * - Bootstrap Modal static backdrop + keyboard:false para evitar fechamento acidental
 * - z-index stacking: modal 1060, backdrop 1059, popup DropDownTree 1060
 * - Syncfusion DropDownTree recriado em shown.bs.modal (fix popup rendering issue
 *   quando control criado com display:none)
 * - DropDownTree.value espera array ([value]) para seleção, não string
 * - Kendo ComboBox usa setDataSource + value() methods (diferentes de Syncfusion)
 * - getRequisitanteCombo() function externa (não definida neste arquivo)
 * - Ramal parseInt sem radix (assume base 10)
 * - SetorSolicitanteId toString() para garantir string
 * - Sort alfabético com localeCompare('pt-BR') para ordem correta
 * - Regex Unicode: \p{L}\p{N} para suportar acentos em nomes
 * - Auto-inicialização DOMContentLoaded para inicializarDropDownTreeModal
 * - Comentários sobre código DESABILITADO (accordion antigo, botão fechar)
 * - Accordion code comentado (lines 212-268) para referência histórica
 * - MutationObserver desconectado em reset (window.__accordionObserver)
 * - Campo Email: domínio @camara.leg.br hardcoded e obrigatório
 * - Campo Ponto: prefixo "p_" hardcoded e obrigatório
 * - Campo Nome: conectores ['de', 'da', 'do', 'das', 'dos', 'e'] lowercase
 * - Validação de Ramal: começa com 1-9 (não aceita 0 como primeiro dígito)
 * - Console.log stack trace em limparCamposCadastroRequisitante (debug)
 * - Toast notifications: prefere AppToast.show, fallback toastr, fallback Alerta
 * - Alerta.Alerta usado para validações simples (não bloqueia workflow)
 * - Alerta.Warning usado para avisos não-críticos (truncamento)
 * - Alerta.Erro usado para erros AJAX críticos
 * - jQuery.ajax usado em vez de fetch (legacy pattern)
 * - contentType: "application/json; charset=utf-8" explícito em POST
 * - dataType: "json" para auto-parse response
 * - data: JSON.stringify para body serialization
 *
 * 🔌 VERSÃO: 1.2
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/* eslint-disable no-undef */
(function ()
{
    "use strict";

    // Debug: Rastrear cargas do arquivo
    window.requisitanteServiceLoadCount = (window.requisitanteServiceLoadCount || 0) + 1;
    console.log("🔄 requisitante_service.js CARREGADO - Carga #" + window.requisitanteServiceLoadCount);
    console.log("   Timestamp:", new Date().toISOString());

    // ------------------------------
    // Serviço (chamadas à API)
    // ------------------------------
    class RequisitanteService
    {
        constructor()
        {
            this.api = window.ApiClient;
        }

        /**
         * Adiciona novo requisitante
         * @param {Object} dados - Dados do requisitante
         * @returns {Promise<Object>} Resultado da operação
         */
        async adicionar(dados)
        {
            try
            {
                const response = await this.api.post('/api/Viagem/AdicionarRequisitante', dados);

                if (response.success)
                {
                    return {
                        success: true,
                        message: response.message,
                        requisitanteId: response.requisitanteid
                    };
                } else
                {
                    return {
                        success: false,
                        message: response.message || "Erro ao adicionar requisitante"
                    };
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("requisitante_service.js", "adicionar", error);
                return {
                    success: false,
                    error: error.message
                };
            }
        }

        /**
         * Lista requisitantes
         * @returns {Promise<{success:boolean,data:any[],error?:string}>}
         */
        async listar()
        {
            try
            {
                return new Promise((resolve, reject) =>
                {
                    $.ajax({
                        url: "/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes",
                        method: "GET",
                        datatype: "json",
                        success: function (res)
                        {
                            const requisitantes = res.data.map(item => ({
                                RequisitanteId: item.requisitanteId,
                                Requisitante: item.requisitante
                            }));

                            resolve({
                                success: true,
                                data: requisitantes
                            });
                        },
                        error: function (jqXHR, textStatus, errorThrown)
                        {
                            const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                            Alerta.TratamentoErroComLinha("requisitante.service.js", "listar", erro);
                            reject(erro);
                        }
                    });
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("requisitante.service.js", "listar", error);
                return {
                    success: false,
                    error: error.message,
                    data: []
                };
            }
        }
    }

    // Instância global do serviço
    window.RequisitanteService = new RequisitanteService();

    // Flag para prevenir fechamento durante validação
    let estaValidando = false;

    // Flag para evitar duplo clique no botão Novo Requisitante
    let isProcessing = false;

    // Contador de inicializações (debug)
    let inicializacaoCount = 0;


    // ===============================================================
    // CAPTURA DE DADOS DE SETORES DO VIEWDATA
    // ===============================================================

    /**
     * Captura dados de setores já carregados nos outros controles
     */
    function capturarDadosSetores()
    {
        try
        {
            // Tentar pegar dos controles já existentes (via bridge getSyncfusionInstance)
            const ddtSetorAgendamento = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;

            if (ddtSetorAgendamento)
            {
                const dados = ddtSetorAgendamento.fields?.dataSource;
                if (dados && dados.length > 0)
                {
                    window.SETORES_DATA = dados;
                    console.log(`✅ Dados de setores capturados: ${dados.length} itens`);
                    return true;
                }
            }

            // Tentar do lstSetorRequisitanteEvento (via bridge getSyncfusionInstance)
            const ddtSetorEvento = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteEvento") : null;
            if (ddtSetorEvento)
            {
                const dados = ddtSetorEvento.fields?.dataSource;
                if (dados && dados.length > 0)
                {
                    window.SETORES_DATA = dados;
                    console.log(`✅ Dados de setores capturados do evento: ${dados.length} itens`);
                    return true;
                }
            }

            console.warn("⚠️ Não foi possível capturar dados de setores");
            return false;

        } catch (error)
        {
            console.error("❌ Erro ao capturar dados de setores:", error);
            return false;
        }
    }

    // ===============================================================
    // SISTEMA DE REQUISITANTE - ACCORDION (UI)
    // ===============================================================

    /**
     * Inicializa o sistema de requisitante (chamar ao abrir o modal)
     */
    function inicializarSistemaRequisitante()
    {
        inicializacaoCount++;
        console.log(`🔄 inicializarSistemaRequisitante chamada (${inicializacaoCount}x)`);

        // PROTEÇÃO: Evitar múltiplas inicializações
        if (window.requisitanteServiceInicializado)
        {
            console.log("⚠️ Sistema já inicializado, ignorando chamada duplicada");
            return;
        }

        // Marca como inicializado IMEDIATAMENTE para evitar race conditions
        window.requisitanteServiceInicializado = true;
        console.log("📍 Marcado como inicializado. Próximas chamadas serão ignoradas.");

        // ⚠️ MODAL: Botão "Novo Requisitante" agora usa Bootstrap Modal (data-bs-toggle="modal")
        // Não precisamos mais interceptar o clique manualmente
        // configurarBotaoNovoRequisitante(); // <-- DESABILITADO: lógica de accordion removida

        // Configura botões do formulário de cadastro no modal
        configurarBotoesCadastroRequisitante();

        // ⚠️ ACCORDION REMOVIDO: Código global click listener era para accordion
        // Agora usamos modal, então não precisamos mais desse listener complexo
        /*
        // Remove listener global antigo (se existir)
        if (window.globalClickListener)
        {
            document.removeEventListener("click", window.globalClickListener, true);
            console.log("🗑️ Listener global antigo removido");
        }

        // Cria função nomeada para o listener global
        // BLOQUEIO SELETIVO: Apenas botão btnRequisitante e elementos do accordion
        window.globalClickListener = function (e)
        {
            if (!estaValidando) return;

            // Permitir cliques no SweetAlert
            if (e.target.closest('.swal2-container') ||
                e.target.classList.contains('swal2-container'))
            {
                return; // ✅ SweetAlert pode funcionar normalmente
            }

            // Bloquear apenas: btnRequisitante e elementos do accordion
            const btnRequisitante = document.getElementById('btnRequisitante');
            const accordionRequisitante = document.getElementById('accordionRequisitante');

            const clickedBtn = e.target === btnRequisitante ||
                (btnRequisitante && btnRequisitante.contains(e.target));

            const clickedAccordion = accordionRequisitante &&
                (e.target === accordionRequisitante ||
                    accordionRequisitante.contains(e.target));

            if (clickedBtn || clickedAccordion)
            {
                console.log("🛑 Click bloqueado durante validação no:",
                    clickedBtn ? "botão" : "accordion");
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
            }
        };

        // Adiciona listener global para prevenir fechamento durante validação
        document.addEventListener("click", window.globalClickListener, true);
        console.log("✅ Listener global adicionado");
        console.log("🔍 window.globalClickListener referência:", window.globalClickListener ? "EXISTE" : "NULL");
        console.log("🔍 Tipo:", typeof window.globalClickListener);
        */

        console.log("✅ Sistema de Requisitante inicializado!");
    }

    /**
     * Configura o botão "Novo Requisitante" (toggle)
     */
    function configurarBotaoNovoRequisitante()
    {
        console.log("🔧 Configurando botão Novo Requisitante...");
        const btnRequisitante = document.getElementById("btnRequisitante");

        if (!btnRequisitante)
        {
            console.error("❌ btnRequisitante NÃO ENCONTRADO no DOM!");
            return;
        }

        console.log("✅ btnRequisitante encontrado:", btnRequisitante);

        // Remove listeners anteriores clonando o botão
        const novoBotao = btnRequisitante.cloneNode(true);
        btnRequisitante.parentNode.replaceChild(novoBotao, btnRequisitante);

        // Adiciona listener (TOGGLE) - fase de captura
        novoBotao.addEventListener("click", function (e)
        {
            console.log("🖱️ ========================================");
            console.log("🖱️ CLIQUE NO btnRequisitante DETECTADO!");
            console.log("🖱️ ========================================");
            console.log("   - estaValidando:", estaValidando);
            console.log("   - isProcessing:", isProcessing);
            console.log("   - Event:", e);
            console.log("   - Target:", e.target);

            // Ignorar se está validando
            if (estaValidando)
            {
                console.log("⏸️ Validação em andamento, ignorando clique");
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                return false;
            }

            if (isProcessing)
            {
                console.log("⏸️ Já processando, ignorando clique");
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                return false;
            }

            isProcessing = true;

            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();

            const sectionCadastro = document.getElementById("sectionCadastroRequisitante");

            if (!sectionCadastro)
            {
                console.error("❌ sectionCadastroRequisitante NÃO ENCONTRADO!");
                isProcessing = false;
                return false;
            }

            console.log("✅ sectionCadastroRequisitante encontrado:", sectionCadastro);
            console.log("   - style.display atual:", sectionCadastro.style.display);

            // TOGGLE
            const estaOculto = (sectionCadastro.style.display === "none" || !sectionCadastro.style.display);
            console.log("   - estaOculto:", estaOculto);

            if (estaOculto)
            {
                console.log("🆕 ========================================");
                console.log("🆕 ABRINDO FORMULÁRIO DE REQUISITANTE");
                console.log("🆕 ========================================");
                abrirFormularioCadastroRequisitante();

                setTimeout(() =>
                {
                    isProcessing = false;
                }, 300);
            } else
            {
                console.log("➖ Fechando formulário de cadastro de requisitante");
                fecharFormularioCadastroRequisitante();
                setTimeout(() => { isProcessing = false; }, 300);
            }

            return false;
        }, true); // capture

        console.log("✅ Botão Novo Requisitante configurado (modo TOGGLE)");
    }

    /**
     * Abre o modal de cadastro de requisitante
     */
    function abrirFormularioCadastroRequisitante()
    {
        try
        {
            console.log("🆕 ABRINDO modal de requisitante...");

            // 1) Limpa campos antes de abrir
            limparCamposCadastroRequisitante();

            // 2) Abre o modal Bootstrap
            const modalElement = document.getElementById('modalNovoRequisitante');
            if (!modalElement) {
                console.error("❌ Modal modalNovoRequisitante não encontrado no DOM");
                return;
            }

            // Garantir que o modal pai (modalViagens) NÃO será fechado
            // Definir z-index do novo modal para ficar acima
            const modalViagens = document.getElementById('modalViagens');
            if (modalViagens) {
                console.log("🔓 Garantindo que modalViagens permanece aberto...");
                // Não fazer nada com modalViagens - deixar aberto
            }

            // Criar ou obter instância do modal
            let modalInstance = bootstrap.Modal.getInstance(modalElement);
            if (!modalInstance) {
                modalInstance = new bootstrap.Modal(modalElement, {
                    backdrop: 'static', // Backdrop estático para evitar fechar ao clicar fora acidentalmente
                    keyboard: false     // Evitar fechar com ESC para não fechar o pai junto
                });
            }

            // Abrir o modal
            modalInstance.show();
            
            // 🔥 CORREÇÃO DE Z-INDEX PARA MODAIS EMPILHADOS
            // O modal pai (modalViagens) tem z-index padrão (1055).
            // O novo modal precisa ser maior. E o backdrop dele também.
            setTimeout(() => {
                // Ajustar z-index do modal filho
                modalElement.style.zIndex = '1060';
                
                // Ajustar z-index do backdrop do modal filho (o último backdrop criado)
                const backdrops = document.querySelectorAll('.modal-backdrop');
                if (backdrops.length > 1) {
                    const ultimoBackdrop = backdrops[backdrops.length - 1];
                    ultimoBackdrop.style.zIndex = '1059'; // Acima do modal pai (1055), abaixo do filho (1060)
                }
            }, 150); // Pequeno delay para garantir que o Bootstrap criou o backdrop

            console.log("✅ Modal de Novo Requisitante aberto (Stacking corrigido)");

            // 3) CRÍTICO: Destruir e recriar ddtSetorNovoRequisitante após modal abrir
            // Syncfusion não renderiza popup corretamente quando controle é criado com display:none
            modalElement.addEventListener('shown.bs.modal', function inicializarDropdown() {
                setTimeout(() =>
                {
                    const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");

                if (!ddtSetor)
                {
                    console.error("❌ ddtSetorNovoRequisitante não encontrado no DOM");
                    return;
                }

                console.log("🔍 ddtSetorNovoRequisitante encontrado, iniciando recriação...");

                // Capturar dados de setores se ainda não existirem
                if (!window.SETORES_DATA || window.SETORES_DATA.length === 0)
                {
                    console.log("📦 Capturando dados de setores...");
                    const capturado = capturarDadosSetores();

                    if (!capturado || !window.SETORES_DATA || window.SETORES_DATA.length === 0)
                    {
                        console.error("❌ Não foi possível capturar dados de setores!");
                        console.error("   window.SETORES_DATA:", window.SETORES_DATA);
                        Alerta.Warning(
                            "Atenção",
                            "Não foi possível carregar a lista de setores. Por favor, recarregue a página.",
                            "OK"
                        );
                        return;
                    }
                }

                console.log(`📦 Dados de setores disponíveis: ${window.SETORES_DATA?.length || 0} itens`);

                // Destruir instância antiga se existir (via bridge getSyncfusionInstance)
                const ddtSetorInstanciaAntiga = window.getSyncfusionInstance ? window.getSyncfusionInstance("ddtSetorNovoRequisitante") : null;
                if (ddtSetorInstanciaAntiga)
                {
                    console.log("🗑️ Destruindo instância antiga de ddtSetorNovoRequisitante...");
                    try
                    {
                        ddtSetorInstanciaAntiga.destroy();
                    }
                    catch (error)
                    {
                        console.warn("⚠️ Erro ao destruir instância antiga:", error);
                    }
                }

                // Recriar o controle
                console.log("🔧 Recriando ddtSetorNovoRequisitante...");

                try
                {
                    const novoDropdown = new ej.dropdowns.DropDownTree({
                        fields: {
                            dataSource: window.SETORES_DATA || [],
                            value: 'SetorSolicitanteId',
                            text: 'Nome',
                            parentValue: 'SetorPaiId',
                            hasChildren: 'HasChild'
                        },
                        allowFiltering: true,
                        placeholder: 'Selecione o setor...',
                        sortOrder: 'Ascending',
                        showCheckBox: false,
                        filterType: 'Contains',
                        filterBarPlaceholder: 'Procurar...',
                        popupHeight: '200px',
                        popupWidth: '100%',

                        // 🔥 EVENTOS CRÍTICOS PARA GARANTIR BOA EXPERIÊNCIA NO MODAL
                        open: function(args) {
                            console.log("🔓 DropDownTree ABERTO (popup)");
                            // Garantir z-index correto do popup
                            if (args && args.popup && args.popup.element) {
                                args.popup.element.style.zIndex = '1060'; // Acima do modal (1055)
                            }
                        },

                        select: function(args) {
                            console.log("✅ Item SELECIONADO no DropDownTree:", args.nodeData?.text);
                            // Prevenir propagação que pode disparar fechamento
                            if (args.event) {
                                args.event.stopPropagation();
                            }
                        },

                        blur: function(args) {
                            console.log("👁️ DropDownTree BLUR (perdeu foco)");
                            // Não fechar accordion ao perder foco
                        },

                        close: function(args) {
                            console.log("🔒 DropDownTree FECHADO (popup)");
                            // Modal permanece aberto naturalmente - não precisa forçar reabertura
                        },

                        created: function() {
                            console.log("✅ DropDownTree CREATED disparado");
                        },

                        dataBound: function() {
                            console.log("✅ DropDownTree DATA BOUND disparado");
                            console.log(`   Total de itens: ${this.treeData?.length || 0}`);
                        }
                    });

                    novoDropdown.appendTo(ddtSetor);

                    console.log(`✅ ddtSetorNovoRequisitante recriado - ${window.SETORES_DATA?.length || 0} itens carregados`);
                    console.log("🔍 Instância criada:", novoDropdown);
                }
                catch (error)
                {
                    console.error("❌ Erro ao criar DropDownTree:", error);
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "abrirFormularioCadastroRequisitante - criar dropdown", error);
                }

                }, 100);

                // Remover listener após executar uma vez
                modalElement.removeEventListener('shown.bs.modal', inicializarDropdown);
            }, { once: true });

            console.log("✅ Modal de cadastro de requisitante sendo aberto");
        } catch (error)
        {
            console.error("❌ Erro ao abrir modal:", error);
            Alerta.TratamentoErroComLinha("requisitante.service.js", "abrirFormularioCadastroRequisitante", error);
        }
    }

    /**
     * Fecha o modal de cadastro de requisitante
     */
    function fecharFormularioCadastroRequisitante()
    {
        try
        {
            console.log("➖ Fechando modal de cadastro de requisitante");

            const modalElement = document.getElementById('modalNovoRequisitante');
            if (modalElement) {
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                if (modalInstance) {
                    modalInstance.hide();
                    console.log("✅ Modal fechado via Bootstrap");
                }
            }

            // Reset da flag de processamento
            isProcessing = false;

            console.log("✅ Modal fechado");
        } catch (error)
        {
            console.error("❌ Erro ao fechar modal:", error);
            Alerta.TratamentoErroComLinha("requisitante.service.js", "fecharFormularioCadastroRequisitante", error);
        }
    }

    /**
     * Limpa os campos do formulário de cadastro de requisitante
     */
    function limparCamposCadastroRequisitante()
    {
        try
        {
            console.log("🧹 Limpando campos do formulário de requisitante");
            console.log("   Stack trace:", new Error().stack);

            // Campos de texto simples
            const txtPonto = document.getElementById("txtPonto");
            const txtNome = document.getElementById("txtNome");
            const txtRamal = document.getElementById("txtRamal");
            const txtEmail = document.getElementById("txtEmail");

            if (txtPonto) txtPonto.value = "";
            if (txtNome) txtNome.value = "";
            if (txtRamal) txtRamal.value = "";
            if (txtEmail) txtEmail.value = "";

            // Dropdown de Setor
            const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");
            console.log("🔍 ddtSetorNovoRequisitante:", ddtSetor ? "encontrado" : "NÃO ENCONTRADO");

            if (ddtSetor)
            {
                // Obter instância via bridge getSyncfusionInstance
                const dropdown = window.getSyncfusionInstance ? window.getSyncfusionInstance("ddtSetorNovoRequisitante") : null;
                console.log("🔍 getSyncfusionInstance:", dropdown ? "existe" : "NÃO EXISTE");

                if (dropdown)
                {
                    console.log(`🔍 DataSource: ${dropdown.fields?.dataSource?.length || 0} itens`);
                    console.log("🔍 Campos configurados:", {
                        value: dropdown.fields.value,
                        text: dropdown.fields.text,
                        parentValue: dropdown.fields.parentValue,
                        hasChildren: dropdown.fields.hasChildren
                    });
                    console.log("🔍 Primeiros 3 itens:", dropdown.fields?.dataSource?.slice(0, 3));

                    dropdown.value = null;
                    dropdown.dataBind();
                    console.log("✅ ddtSetorNovoRequisitante limpo");
                } else
                {
                    console.warn("⚠️ ddtSetorNovoRequisitante não está inicializado");
                }
            }

            console.log("✅ Campos limpos");
        } catch (error)
        {
            console.error("❌ Erro ao limpar campos:", error);
        }
    }

    /**
     * Configura validação do campo Ponto
     */
    function configurarValidacaoPonto()
    {
        const txtPonto = document.getElementById("txtPonto");
        if (!txtPonto)
        {
            console.warn("⚠️ txtPonto não encontrado");
            return;
        }

        // Remove listeners anteriores
        const novoCampo = txtPonto.cloneNode(true);
        txtPonto.parentNode.replaceChild(novoCampo, txtPonto);

        // Adiciona validação no blur (lostfocus)
        novoCampo.addEventListener("blur", function(e)
        {
            try
            {
                let valor = novoCampo.value.trim();

                if (!valor)
                {
                    return; // Campo vazio, não valida
                }

                // Verificar tamanho máximo (50 caracteres conforme banco)
                if (valor.length > 50)
                {
                    Alerta.Warning(
                        "Atenção",
                        "O Ponto não pode ter mais de 50 caracteres. Será truncado.",
                        "OK"
                    );
                    valor = valor.substring(0, 50);
                }

                // Verificar se começa com "p_" (minúsculo)
                if (valor.toLowerCase().startsWith("p_"))
                {
                    // Se começa com P_ (maiúsculo), converter para p_
                    if (valor.startsWith("P_"))
                    {
                        valor = "p_" + valor.substring(2);
                        console.log("✅ P_ convertido para p_");
                    }
                    // Se já está correto (p_), não faz nada
                }
                else
                {
                    // Não começa com p_ nem P_ - adicionar p_
                    valor = "p_" + valor;
                    console.log("✅ p_ adicionado ao início");
                }

                // Verificar novamente tamanho após adicionar p_
                if (valor.length > 50)
                {
                    Alerta.Warning(
                        "Atenção",
                        "O Ponto não pode ter mais de 50 caracteres (incluindo 'p_'). Será truncado.",
                        "OK"
                    );
                    valor = valor.substring(0, 50);
                }

                // Atualizar campo
                novoCampo.value = valor;

            }
            catch (error)
            {
                console.error("❌ Erro na validação do Ponto:", error);
                Alerta.TratamentoErroComLinha("requisitante.service.js", "configurarValidacaoPonto", error);
            }
        });

        console.log("✅ Validação de Ponto configurada");
    }

    /**
     * Converte string para Camel Case
     * @param {string} str - String para converter
     * @returns {string} String em Camel Case
     */
    function toCamelCase(str)
    {
        const conectores = ['de', 'da', 'do', 'das', 'dos', 'e'];
        return str
            .toLowerCase()
            .split(' ')
            .filter(palavra => palavra.length > 0)
            .map((palavra, index) =>
            {
                // Primeira palavra sempre em Camel Case, demais verificar se é conector
                if (index === 0 || !conectores.includes(palavra)) {
                    return palavra.charAt(0).toUpperCase() + palavra.slice(1);
                }
                return palavra;
            })
            .join(' ');
    }

    /**
     * Remove caracteres inválidos do nome e limita a 80 caracteres
     * @param {string} valor - Valor para sanitizar
     * @returns {string} Valor sanitizado
     */
    function sanitizeNomeCompleto(valor)
    {
        // Remove tudo exceto letras Unicode, números e espaços
        let limpo = valor.replace(/[^\p{L}\p{N} ]+/gu, '');
        if (limpo.length > 80) {
            limpo = limpo.substring(0, 80);
        }
        return limpo;
    }

    /**
     * Configura validações de Email, Ramal e Nome (padrão Usuarios/Upsert)
     */
    function configurarValidacoesRequisitante()
    {
        // =====================================================
        // VALIDAÇÃO: Ramal - apenas números (máx 8 dígitos, começa com 1-9)
        // =====================================================
        const txtRamal = document.getElementById("txtRamal");
        if (txtRamal)
        {
            // Remove listeners anteriores
            const novoRamal = txtRamal.cloneNode(true);
            txtRamal.parentNode.replaceChild(novoRamal, txtRamal);

            novoRamal.addEventListener("input", function()
            {
                try
                {
                    let valor = novoRamal.value.replace(/\D/g, '');
                    valor = valor.substring(0, 8);
                    novoRamal.value = valor;
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtRamal.input", error);
                }
            });

            novoRamal.addEventListener("blur", function()
            {
                try
                {
                    const valor = novoRamal.value.trim();
                    const regex = /^[1-9]\d{7}$/; // 8 dígitos começando com 1-9

                    if (valor && !regex.test(valor))
                    {
                        novoRamal.classList.add('is-invalid');
                    }
                    else
                    {
                        novoRamal.classList.remove('is-invalid');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtRamal.blur", error);
                }
            });

            console.log("✅ Validação de Ramal configurada");
        }

        // =====================================================
        // VALIDAÇÃO: Email obrigatoriamente terminando em @camara.leg.br
        // =====================================================
        const txtEmail = document.getElementById("txtEmail");
        if (txtEmail)
        {
            // Remove listeners anteriores
            const novoEmail = txtEmail.cloneNode(true);
            txtEmail.parentNode.replaceChild(novoEmail, txtEmail);

            novoEmail.addEventListener("blur", function()
            {
                try
                {
                    let valor = novoEmail.value.trim().toLowerCase();

                    if (valor)
                    {
                        // Remove @camara.leg.br se já existir
                        valor = valor.replace(/@camara\.leg\.br$/i, '');
                        // Remove qualquer @ que possa existir
                        valor = valor.replace(/@/g, '');
                        // Remove caracteres inválidos (permite: letras, números, ponto, hífen, underscore)
                        valor = valor.replace(/[^a-z0-9._-]/g, '');

                        if (valor.length > 0)
                        {
                            // Adiciona domínio obrigatório
                            valor = valor + '@camara.leg.br';
                        }
                        else
                        {
                            valor = '';
                        }

                        novoEmail.value = valor;

                        // Valida formato final
                        const regex = /^[a-z0-9._-]+@camara\.leg\.br$/;
                        if (valor && !regex.test(valor))
                        {
                            novoEmail.classList.add('is-invalid');
                        }
                        else
                        {
                            novoEmail.classList.remove('is-invalid');
                        }
                    }
                    else
                    {
                        novoEmail.classList.add('is-invalid');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtEmail.blur", error);
                }
            });

            novoEmail.addEventListener("input", function()
            {
                try
                {
                    // Converte para minúsculo
                    let valor = novoEmail.value.toLowerCase();
                    // Remove tudo que não é letra, número, ponto, hífen, underscore ou @
                    valor = valor.replace(/[^a-z0-9._@-]/g, '');

                    // Limita a 1 @
                    const numArrobas = (valor.match(/@/g) || []).length;
                    if (numArrobas > 1)
                    {
                        const partes = valor.split('@');
                        valor = partes[0] + '@' + partes.slice(1).join('');
                    }

                    novoEmail.value = valor;
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtEmail.input", error);
                }
            });

            console.log("✅ Validação de Email configurada");
        }

        // =====================================================
        // VALIDAÇÃO: Nome obrigatório em Camel Case
        // =====================================================
        const txtNome = document.getElementById("txtNome");
        if (txtNome)
        {
            // Remove listeners anteriores
            const novoNome = txtNome.cloneNode(true);
            txtNome.parentNode.replaceChild(novoNome, txtNome);

            // INPUT: Remove caracteres inválidos e limita a 80 chars
            novoNome.addEventListener("input", function()
            {
                try
                {
                    novoNome.value = sanitizeNomeCompleto(novoNome.value);
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtNome.input", error);
                }
            });

            // BLUR: Converte para Camel Case e valida se não está vazio
            novoNome.addEventListener("blur", function()
            {
                try
                {
                    const valor = sanitizeNomeCompleto(novoNome.value.trim());
                    if (valor)
                    {
                        novoNome.value = toCamelCase(valor);
                        novoNome.classList.remove('is-invalid');
                    }
                    else
                    {
                        novoNome.classList.add('is-invalid');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtNome.blur", error);
                }
            });

            console.log("✅ Validação de Nome configurada");
        }
    }

    /**
     * Configura os botões do formulário de cadastro de requisitante
     */
    function configurarBotoesCadastroRequisitante()
    {
        // ===== CONFIGURAR VALIDAÇÃO DO CAMPO PONTO =====
        configurarValidacaoPonto();

        // ===== CONFIGURAR VALIDAÇÕES DE RAMAL, EMAIL E NOME =====
        configurarValidacoesRequisitante();

        // ===== BOTÃO SALVAR =====
        const btnSalvarRequisitante = document.getElementById("btnInserirRequisitante");
        if (btnSalvarRequisitante)
        {
            // Remove listeners anteriores
            const novoBotaoSalvar = btnSalvarRequisitante.cloneNode(true);
            btnSalvarRequisitante.parentNode.replaceChild(novoBotaoSalvar, btnSalvarRequisitante);

            // Adiciona novo listener
            novoBotaoSalvar.addEventListener("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                salvarNovoRequisitante();
            }, true);

            console.log("✅ Botão Salvar configurado");
        } else
        {
            console.warn("⚠️ btnInserirRequisitante não encontrado");
        }

        // ⚠️ MODAL: Botão "Cancelar Operação" no modal usa data-bs-dismiss="modal"
        // Não precisamos configurar listener manualmente - Bootstrap gerencia isso
        /*
        // ===== BOTÃO FECHAR =====
        const btnCancelarRequisitante = document.getElementById("btnFecharAccordionRequisitante");
        if (btnCancelarRequisitante)
        {
            // Remove listeners anteriores
            const novoBotaoFechar = btnCancelarRequisitante.cloneNode(true);
            btnCancelarRequisitante.parentNode.replaceChild(novoBotaoFechar, btnCancelarRequisitante);

            // Adiciona novo listener
            novoBotaoFechar.addEventListener("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                fecharFormularioCadastroRequisitante();
                limparCamposCadastroRequisitante();
            }, true);

            console.log("✅ Botão Fechar configurado");
        } else
        {
            console.warn("⚠️ btnFecharAccordionRequisitante não encontrado");
        }
        */

        console.log("✅ Botões configurados com estilos padrão");
    }

    /**
     * Salva o novo requisitante chamando a API via AJAX
     */
    function salvarNovoRequisitante()
    {
        try
        {
            console.log("💾 Iniciando salvamento de requisitante.");

            // ===== OBTER CAMPOS =====
            const txtPonto = document.getElementById("txtPonto");
            const txtNome = document.getElementById("txtNome");
            const txtRamal = document.getElementById("txtRamal");
            const txtEmail = document.getElementById("txtEmail");
            // ATUALIZADO: Usar campo oculto do TreeView em vez do DropDownTree antigo
            const hiddenSetorId = document.getElementById("hiddenSetorId");

            // ===== VALIDAÇÕES =====
            console.log("🔍 Iniciando validações - ativando flag estaValidando");
            estaValidando = true;

            if (!txtPonto || !txtPonto.value.trim())
            {
                console.log("❌ Validação falhou: Ponto obrigatório");

                // Agendar desativação da flag ANTES de mostrar alerta
                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Ponto)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Ponto é obrigatório!");
                if (txtPonto) txtPonto.focus();
                return;
            }

            if (!txtNome || !txtNome.value.trim())
            {
                console.log("❌ Validação falhou: Nome obrigatório");

                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Nome)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Nome é obrigatório!");
                if (txtNome) txtNome.focus();
                return;
            }

            if (!txtRamal || !txtRamal.value.trim())
            {
                console.log("❌ Validação falhou: Ramal obrigatório");

                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Ramal)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Ramal é obrigatório!");
                if (txtRamal) txtRamal.focus();
                return;
            }

            // ATUALIZADO: Obter valor do campo oculto preenchido pelo TreeView
            let setorValue = null;
            if (hiddenSetorId)
            {
                setorValue = hiddenSetorId.value;
                console.log("🔍 Validando hiddenSetorId (TreeView):");
                console.log("  - Valor:", setorValue);
            } else
            {
                console.error("❌ hiddenSetorId não encontrado no DOM!");
            }

            if (!setorValue || setorValue.trim() === "")
            {
                console.log("❌ Validação falhou: Setor obrigatório");

                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Setor)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Setor do Requisitante é obrigatório!");
                return;
            }

            // Validações passaram
            console.log("✅ Todas as validações passaram");
            estaValidando = false;

            // ===== MONTAR OBJETO =====
            const objRequisitante = {
                Nome: txtNome.value.trim(),
                Ponto: txtPonto.value.trim(),
                Ramal: parseInt(txtRamal.value.trim()),
                Email: txtEmail ? txtEmail.value.trim() : "",
                SetorSolicitanteId: setorValue.toString()
            };

            console.log("📦 Dados coletados:", objRequisitante);

            // ===== CHAMAR API VIA AJAX =====
            $.ajax({
                type: "POST",
                url: "/api/Viagem/AdicionarRequisitante",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(objRequisitante),
                success: function (data)
                {
                    try
                    {
                        if (data.success)
                        {
                            console.log("✅ Requisitante adicionado com sucesso!");
                            console.log("📦 Resposta da API:", data);

                            if (typeof AppToast !== 'undefined')
                            {
                                AppToast.show('Verde', data.message);
                            } else if (typeof toastr !== 'undefined')
                            {
                                toastr.success(data.message);
                            }

                            // ===== ATUALIZAR DROPDOWN lstRequisitante =====
                            const comboRequisitante = getRequisitanteCombo();
                            if (comboRequisitante)
                            {
                                const novoItem = {
                                    RequisitanteId: data.requisitanteid,
                                    Requisitante: txtNome.value.trim() + " - " + txtPonto.value.trim()
                                };

                                console.log("📦 Novo requisitante a ser adicionado:", novoItem);

                                // Obter dataSource atual (Telerik)
                                let dataSource = comboRequisitante.dataSource.data() || [];

                                if (!Array.isArray(dataSource))
                                {
                                    dataSource = [];
                                }

                                // Verificar se já existe
                                const jaExiste = dataSource.some(item => item.RequisitanteId === data.requisitanteid);

                                if (!jaExiste)
                                {
                                    // Adiciona o novo item
                                    dataSource.push(novoItem);
                                    console.log("📦 Novo item adicionado ao array");

                                    // Ordena alfabeticamente por nome do requisitante (case-insensitive)
                                    dataSource.sort((a, b) => {
                                        const nomeA = (a.Requisitante || '').toString().toLowerCase();
                                        const nomeB = (b.Requisitante || '').toString().toLowerCase();
                                        return nomeA.localeCompare(nomeB, 'pt-BR');
                                    });
                                    console.log("🔄 Lista ordenada alfabeticamente");

                                    // Atualiza dataSource (Telerik usa setDataSource)
                                    comboRequisitante.setDataSource(dataSource);

                                    console.log("✅ Lista atualizada e ordenada com sucesso");
                                }
                                else
                                {
                                    console.log("⚠️ Requisitante já existe na lista");
                                }

                                // Seleciona o novo requisitante (Telerik)
                                comboRequisitante.value(data.requisitanteid);

                                console.log("✅ Requisitante selecionado:", data.requisitanteid);
                            }

                            // ===== ATUALIZAR RAMAL =====
                            // txtRamalRequisitanteSF é um input HTML simples, não Syncfusion
                            const txtRamalRequisitanteSF = document.getElementById("txtRamalRequisitanteSF");
                            if (txtRamalRequisitanteSF)
                            {
                                txtRamalRequisitanteSF.value = txtRamal.value.trim();
                                console.log("✅ Campo Ramal atualizado:", txtRamal.value.trim());
                            }

                            // ===== ATUALIZAR SETOR (via bridge getSyncfusionInstance) =====
                            const comboSetor = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;
                            if (comboSetor)
                            {
                                console.log("🔍 Atualizando Setor:");
                                console.log("  - setorValue (closure):", setorValue);
                                console.log("  - Tipo:", typeof setorValue);

                                // DropDownTree espera array como value
                                comboSetor.value = [setorValue.toString()];
                                comboSetor.dataBind();
                                console.log("✅ Campo Setor atualizado para:", setorValue);
                            } else
                            {
                                console.error("❌ lstSetorRequisitanteAgendamento não encontrado ou não é Syncfusion");
                            }

                            // ===== FECHAR MODAL =====
                            const modalNovoRequisitante = bootstrap.Modal.getInstance(document.getElementById('modalNovoRequisitante'));
                            if (modalNovoRequisitante)
                            {
                                modalNovoRequisitante.hide();
                                console.log("✅ Modal fechado");
                            }
                            limparCamposCadastroRequisitante();

                        } else
                        {
                            console.error("❌ Erro ao adicionar requisitante:", data.message);

                            if (typeof AppToast !== 'undefined')
                            {
                                AppToast.show('Vermelho', data.message);
                            } else if (typeof toastr !== 'undefined')
                            {
                                toastr.error(data.message);
                            } else
                            {
                                Alerta.Erro("Atenção", data.message);
                            }
                        }
                    } catch (error)
                    {
                        console.error("❌ Erro no callback de sucesso:", error);
                        Alerta.TratamentoErroComLinha(
                            "requisitante_service.js",
                            "salvarNovoRequisitante.ajax.success",
                            error
                        );
                    }
                },
                error: function (jqXHR, textStatus, errorThrown)
                {
                    try
                    {
                        console.error("❌ Erro na requisição AJAX:", textStatus, errorThrown);
                        console.error("Resposta:", jqXHR.responseText);

                        Alerta.Erro("Atenção", "Erro ao adicionar requisitante. Verifique se já existe um requisitante com este ponto/nome!");

                        Alerta.TratamentoErroComLinha(
                            "requisitante_service.js",
                            "salvarNovoRequisitante.ajax.error",
                            new Error(textStatus + ": " + errorThrown)
                        );
                    } catch (error)
                    {
                        console.error("❌ Erro no callback de erro:", error);
                    }
                }
            });

        } catch (error)
        {
            estaValidando = false;
            console.error("❌ Erro ao salvar requisitante:", error);
            Alerta.TratamentoErroComLinha("requisitante_service.js", "salvarNovoRequisitante", error);
        }
    }

    /**
     * Reseta o sistema de requisitante ao fechar o modal
     * Permite que seja reinicializado na próxima abertura
     */
    function resetarSistemaRequisitante()
    {
        console.log("🔄 Resetando sistema de requisitante...");

        // Resetar flag de inicialização
        window.requisitanteServiceInicializado = false;

        // Fechar accordion se estiver aberto
        fecharFormularioCadastroRequisitante();

        // Limpar campos
        limparCamposCadastroRequisitante();

        // Desconectar MutationObserver se existir
        if (window.__accordionObserver)
        {
            window.__accordionObserver.disconnect();
            window.__accordionObserver = null;
        }

        console.log("✅ Sistema de requisitante resetado");
    }

    /**
     * Inicializa o DropDownTree quando o modal é exibido
     */
    function inicializarDropDownTreeModal()
    {
        console.log("🔧 Inicializando DropDownTree no modal...");

        const modalRequisitante = document.getElementById("modalNovoRequisitante");
        if (!modalRequisitante)
        {
            console.warn("⚠️ modalNovoRequisitante não encontrado");
            return;
        }

        // Listener para quando o modal for completamente exibido
        modalRequisitante.addEventListener('shown.bs.modal', function ()
        {
            console.log("📢 Modal mostrado - inicializando DropDownTree...");

            const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");
            if (!ddtSetor)
            {
                console.error("❌ ddtSetorNovoRequisitante não encontrado no DOM");
                return;
            }

            // Capturar dados de setores se ainda não existirem
            if (!window.SETORES_DATA || window.SETORES_DATA.length === 0)
            {
                console.log("📦 Capturando dados de setores do modal...");
                const capturado = capturarDadosSetores();

                if (!capturado || !window.SETORES_DATA || window.SETORES_DATA.length === 0)
                {
                    console.error("❌ Dados de setores não disponíveis!");
                    console.error("   Tentando aguardar carregamento da página...");

                    // Tentar novamente após 500ms
                    setTimeout(() =>
                    {
                        capturarDadosSetores();
                        if (window.SETORES_DATA && window.SETORES_DATA.length > 0)
                        {
                            console.log(`✅ Dados capturados após delay: ${window.SETORES_DATA.length} itens`);
                            criarDropDownTree(ddtSetor);
                        }
                        else
                        {
                            console.error("❌ Ainda não foi possível capturar dados de setores!");
                        }
                    }, 500);
                    return;
                }
            }

            console.log(`📦 Dados disponíveis: ${window.SETORES_DATA?.length || 0} itens`);
            criarDropDownTree(ddtSetor);
        });

        console.log("✅ Listener do modal configurado");
    }

    /**
     * Cria o DropDownTree no elemento fornecido
     */
    function criarDropDownTree(elemento)
    {
        try
        {
            console.log("🔧 Criando DropDownTree...");

            // Destruir instância antiga se existir (via bridge getSyncfusionInstance)
            const instanciaAntiga = (elemento.id && window.getSyncfusionInstance) ? window.getSyncfusionInstance(elemento.id) : null;
            if (instanciaAntiga)
            {
                console.log("🗑️ Destruindo instância antiga...");
                try
                {
                    instanciaAntiga.destroy();
                }
                catch (error)
                {
                    console.warn("⚠️ Erro ao destruir:", error);
                }
            }

            // Criar nova instância
            const dropdown = new ej.dropdowns.DropDownTree({
                fields: {
                    dataSource: window.SETORES_DATA || [],
                    value: 'SetorSolicitanteId',
                    text: 'Nome',
                    parentValue: 'SetorPaiId',
                    hasChildren: 'HasChild'
                },
                allowFiltering: true,
                placeholder: 'Selecione o setor...',
                sortOrder: 'Ascending',
                showCheckBox: false,
                filterType: 'Contains',
                filterBarPlaceholder: 'Procurar...',
                popupHeight: '200px',
                popupWidth: '100%',
                width: '100%',

                created: function ()
                {
                    console.log("✅ DropDownTree CREATED");
                },

                dataBound: function ()
                {
                    console.log("✅ DropDownTree DATA BOUND");
                    console.log(`   Itens carregados: ${this.treeData?.length || 0}`);
                }
            });

            dropdown.appendTo(elemento);
            console.log(`✅ DropDownTree criado com sucesso - ${window.SETORES_DATA?.length || 0} itens`);
        }
        catch (error)
        {
            console.error("❌ Erro ao criar DropDownTree:", error);
            Alerta.TratamentoErroComLinha("requisitante.service.js", "criarDropDownTree", error);
        }
    }

    // ===============================================================
    // EXPORTAR FUNÇÕES GLOBALMENTE
    // ===============================================================
    window.inicializarSistemaRequisitante = inicializarSistemaRequisitante;
    window.resetarSistemaRequisitante = resetarSistemaRequisitante;
    window.configurarBotaoNovoRequisitante = configurarBotaoNovoRequisitante;
    window.abrirFormularioCadastroRequisitante = abrirFormularioCadastroRequisitante;
    window.fecharFormularioCadastroRequisitante = fecharFormularioCadastroRequisitante;
    window.limparCamposCadastroRequisitante = limparCamposCadastroRequisitante;
    window.salvarNovoRequisitante = salvarNovoRequisitante;
    window.capturarDadosSetores = capturarDadosSetores;
    window.inicializarDropDownTreeModal = inicializarDropDownTreeModal;

    // ===============================================================
    // AUTO-INICIALIZAÇÃO
    // ===============================================================
    // Inicializar o listener do modal quando o DOM estiver pronto
    if (document.readyState === 'loading')
    {
        document.addEventListener('DOMContentLoaded', inicializarDropDownTreeModal);
    }
    else
    {
        inicializarDropDownTreeModal();
    }
})();
