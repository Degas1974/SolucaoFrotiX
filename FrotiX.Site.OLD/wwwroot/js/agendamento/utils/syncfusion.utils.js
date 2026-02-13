/* ****************************************************************************************
 * ⚡ ARQUIVO: syncfusion.utils.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Utilitários completos para componentes Syncfusion EJ2: 10 funções
 *                   para obter instâncias, manipular valores, limpar tooltips globais,
 *                   rebuild dropdowns, inicializar tooltips em modais, configurar paste
 *                   de imagens em RichTextEditor (clipboard API + FileReader + base64),
 *                   configuração completa de localização PT-BR (L10n com 140+ strings
 *                   traduzidas para RichTextEditor + Calendar, setCulture, loadCldr com
 *                   CLDR data para meses e dias em português), callbacks globais RTE
 *                   (onCreate, toolbarClick com XSRF token, onDateChange). Todas as
 *                   funções exportadas para window.* com try-catch completo.
 * 📥 ENTRADAS     : getSyncfusionInstance(id: string), getSfValue0(inst: Object),
 *                   limpaTooltipsGlobais(timeout?: number default 200ms),
 *                   rebuildLstPeriodos() sem params, initializeModalTooltips() sem params,
 *                   setupRTEImagePaste(rteId: string), configurarLocalizacaoSyncfusion()
 *                   sem params, onCreate() callback this context, toolbarClick(e: Event),
 *                   onDateChange(args: {values: Array})
 * 📤 SAÍDAS       : getSyncfusionInstance retorna Syncfusion instance ou null, getSfValue0
 *                   retorna primeiro valor (primitivo ou array[0]) ou null, outras funções
 *                   manipulam DOM/global state (limpaTooltipsGlobais remove elementos,
 *                   rebuildLstPeriodos cria novo DropDownList, initializeModalTooltips
 *                   cria Tooltip instances, setupRTEImagePaste adiciona event listener,
 *                   configurarLocalizacaoSyncfusion configura ej.base.L10n/setCulture/
 *                   loadCldr, onCreate seta window.defaultRTE, toolbarClick adiciona XSRF
 *                   header, onDateChange seta window.selectedDates)
 * 🔗 CHAMADA POR  : exibe-viagem.js, controls-init.js, event-handlers.js, formatters.js,
 *                   calendario.js, validacao.js, dialogs.js, main.js (qualquer código que
 *                   manipule Syncfusion components), Syncfusion RTE callbacks (onCreate,
 *                   toolbarClick via toolbar config, onDateChange via Calendar change event),
 *                   modal open events (initializeModalTooltips), RTE initialization
 *                   (setupRTEImagePaste), app startup (configurarLocalizacaoSyncfusion)
 * 🔄 CHAMA        : document.getElementById, document.querySelectorAll, Array.isArray,
 *                   Array.forEach, element.remove, element.removeAttribute, ej.dropdowns.
 *                   DropDownList (new), ej.popups.Tooltip (new), FileReader (new +
 *                   readAsDataURL), Blob.getAsFile, String.split, String.indexOf,
 *                   setTimeout, ej.base.L10n.load, ej.base.setCulture, ej.base.loadCldr,
 *                   jQuery ($('[data-bs-toggle="tooltip"]').tooltip("dispose"), $(".tooltip").
 *                   remove()), console methods, Alerta.TratamentoErroComLinha, Syncfusion
 *                   instance methods (destroy, dataBind, executeCommand, appendTo),
 *                   XMLHttpRequest.setRequestHeader (via args.currentRequest)
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (ej.dropdowns.DropDownList, ej.popups.Tooltip, ej.base.L10n,
 *                   ej.base.setCulture, ej.base.loadCldr, ej2_instances array), jQuery
 *                   ($.tooltip, $.remove), FileReader API (native browser), Clipboard API
 *                   (event.clipboardData.items), Alerta.TratamentoErroComLinha (frotix-core.js),
 *                   DOM elements (dynamic via getElementById/querySelectorAll: #lstPeriodos,
 *                   [data-ejtip], RichTextEditor elements, .e-tooltip-wrap, .e-control.e-tooltip,
 *                   [title], .tooltip, #rte_upload, input[name="__RequestVerificationToken"]),
 *                   window.dataPeriodos (global array), window.defaultRTE (global reference),
 *                   window.selectedDates (global array)
 * 📝 OBSERVAÇÕES  : Exporta 10 window.* functions: getSyncfusionInstance, getSfValue0,
 *                   limpaTooltipsGlobais, rebuildLstPeriodos, initializeModalTooltips,
 *                   setupRTEImagePaste, configurarLocalizacaoSyncfusion, onCreate,
 *                   toolbarClick, onDateChange. Try-catch em todas as funções com
 *                   TratamentoErroComLinha. Nested try-catch em limpaTooltipsGlobais
 *                   (timeout + 3 inner forEach loops). Nullish coalescing (??) em getSfValue0.
 *                   Optional chaining (?.) em múltiplas funções. setTimeout wrapper em
 *                   limpaTooltipsGlobais com delay configurável (default 200ms).
 *                   setupRTEImagePaste usa paste event listener para capturar imagens do
 *                   clipboard (FileReader.readAsDataURL → base64 → insertHTML via
 *                   executeCommand). configurarLocalizacaoSyncfusion tem 140+ strings PT-BR
 *                   traduzidas para RichTextEditor (toolbar items, dialogs, tables, images,
 *                   links, formats, alignments) + Calendar ("Hoje"). CLDR data hardcoded
 *                   inline (ptBRCldr object com structure CLDR 36 para meses abreviados/wide
 *                   e dias abreviados/wide). onCreate callback armazena this context em
 *                   window.defaultRTE (usado para acesso global ao RTE). toolbarClick adiciona
 *                   XSRF-TOKEN header ao upload de imagens (anti-CSRF protection). onDateChange
 *                   armazena args.values em window.selectedDates (múltiplas datas selecionadas).
 *                   rebuildLstPeriodos destrói implicitamente instância antiga ao appendTo
 *                   (Syncfusion behavior). jQuery tooltip disposal necessário para Bootstrap
 *                   tooltips remnants. e-tooltip-wrap e .tooltip são classes Syncfusion e
 *                   Bootstrap respectivamente. Image paste: detecta item.type.indexOf("image"),
 *                   cria data URL, inserta via executeCommand('insertHTML'). Break após
 *                   primeira imagem encontrada (não processa múltiplas).
 *
 * 📋 ÍNDICE DE FUNÇÕES (10 functions window.*):
 *
 * ┌─ window.getSyncfusionInstance(id) ─────────────────────────────────┐
 * │ → Obtém instância Syncfusion de elemento por ID                    │
 * │ → param id: string, ID do elemento DOM                             │
 * │ → returns Object|null: ej2_instances[0] ou null se não existir     │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. const el = document.getElementById(id)                         │
 * │   3. if el && Array.isArray(el.ej2_instances) &&                    │
 * │      el.ej2_instances.length > 0 && el.ej2_instances[0]:           │
 * │        return el.ej2_instances[0]                                   │
 * │   4. return null                                                    │
 * │   5. catch: Alerta.TratamentoErroComLinha + return null            │
 * │ → Safe accessor para Syncfusion instances (evita undefined errors) │
 * │ → Verifica array, length e [0] explicitamente                      │
 * │ → Usado extensivamente em todos os arquivos do sistema             │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.getSfValue0(inst) ──────────────────────────────────────────┐
 * │ → Obtém primeiro valor de componente Syncfusion                    │
 * │ → param inst: Object, instância Syncfusion (ej2_instances[0])      │
 * │ → returns *: primeiro valor ou null                                 │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. if !inst: return null                                          │
 * │   3. const v = inst.value                                           │
 * │   4. if Array.isArray(v): return v.length ? v[0] : null             │
 * │   5. return v ?? null (nullish coalescing)                          │
 * │   6. catch: Alerta.TratamentoErroComLinha + return null            │
 * │ → Normaliza value (pode ser primitivo ou array dependendo do       │
 * │   component type: DropDownList=primitivo, MultiSelect/DropDownTree │
 * │   com checkbox=array)                                               │
 * │ → Sempre retorna primeiro valor ou null (nunca array, undefined)   │
 * │ → Usado quando código espera single value de component multi-value │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.limpaTooltipsGlobais(timeout) ──────────────────────────────┐
 * │ → Limpa todos os tooltips Syncfusion e Bootstrap do DOM            │
 * │ → param timeout: number opcional, delay em ms (default 200)        │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch outer                                                │
 * │   2. setTimeout(() => {                                             │
 * │      a. try-catch inner para timeout body                           │
 * │      b. querySelectorAll(".e-tooltip-wrap").forEach(t => {          │
 * │           try-catch: t.remove() })                                  │
 * │      c. querySelectorAll(".e-control.e-tooltip").forEach(el => {    │
 * │           try-catch: el.ej2_instances?.[0]?.destroy() })            │
 * │      d. querySelectorAll("[title]").forEach(el => {                 │
 * │           try-catch: el.removeAttribute("title") })                 │
 * │      e. $('[data-bs-toggle="tooltip"]').tooltip("dispose")          │
 * │      f. $(".tooltip").remove()                                      │
 * │   }, timeout)                                                       │
 * │   3. catch outer: Alerta.TratamentoErroComLinha                     │
 * │ → 5 operações de limpeza (3 Syncfusion + 2 Bootstrap)              │
 * │ → Nested try-catch protege contra erros individuais (continua loop)│
 * │ → Optional chaining (?.) para safe destroy call                     │
 * │ → Usado ao fechar modais para evitar tooltips persistentes         │
 * │ → setTimeout delay permite tooltips animarem close antes de remover│
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.rebuildLstPeriodos() ───────────────────────────────────────┐
 * │ → Reconstrói DropDownList de períodos (#lstPeriodos)               │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. new ej.dropdowns.DropDownList({                                │
 * │      dataSource: window.dataPeriodos || [],                         │
 * │      fields: { value: "PeriodoId", text: "Periodo" },               │
 * │      placeholder: "Selecione o período",                            │
 * │      allowFiltering: true,                                          │
 * │      showClearButton: true,                                         │
 * │      sortOrder: "Ascending"                                         │
 * │   }).appendTo("#lstPeriodos")                                       │
 * │   3. catch: Alerta.TratamentoErroComLinha                           │
 * │ → Assume window.dataPeriodos populado (global array)                │
 * │ → appendTo destrói instância antiga automaticamente (Syncfusion)    │
 * │ → Usado quando dataPeriodos é atualizado dinamicamente             │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.initializeModalTooltips() ──────────────────────────────────┐
 * │ → Inicializa tooltips Syncfusion em elementos com data-ejtip       │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch outer                                                │
 * │   2. const tooltipElements = querySelectorAll('[data-ejtip]')       │
 * │   3. tooltipElements.forEach(element => {                           │
 * │      a. try-catch inner                                             │
 * │      b. new ej.popups.Tooltip({ target: element })                  │
 * │   })                                                                │
 * │   4. catch outer: Alerta.TratamentoErroComLinha                     │
 * │ → Cria Tooltip instance para cada elemento [data-ejtip]            │
 * │ → Nested try-catch permite continuar se um tooltip falhar          │
 * │ → Usado em modal shown events para inicializar tooltips            │
 * │ → Sincroniza com elementos adicionados dinamicamente ao DOM        │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.setupRTEImagePaste(rteId) ──────────────────────────────────┐
 * │ → Configura RichTextEditor para aceitar paste de imagens clipboard │
 * │ → param rteId: string, ID do elemento RichTextEditor               │
 * │ → returns void (silent return se RTE não existir)                   │
 * │ → Fluxo:                                                            │
 * │   1. try-catch outer                                                │
 * │   2. const rteDescricao = getElementById(rteId)                     │
 * │   3. if !rteDescricao || !ej2_instances[0]: return                  │
 * │   4. const rte = rteDescricao.ej2_instances[0]                      │
 * │   5. rte.element.addEventListener("paste", function(event) {        │
 * │      a. try-catch inner                                             │
 * │      b. const clipboardData = event.clipboardData                   │
 * │      c. if clipboardData && clipboardData.items:                    │
 * │         - for (let i = 0; i < items.length; i++):                   │
 * │           * if item.type.indexOf("image") !== -1:                   │
 * │             - const blob = item.getAsFile()                         │
 * │             - const reader = new FileReader()                       │
 * │             - reader.onloadend = function() {                       │
 * │                 try-catch:                                          │
 * │                   const base64 = reader.result.split(",")[1]        │
 * │                   const html = `<img src="data:image/png;base64,    │
 * │                     ${base64}" />`                                  │
 * │                   rte.executeCommand('insertHTML', html)            │
 * │               }                                                     │
 * │             - reader.readAsDataURL(blob)                            │
 * │             - break (primeira imagem apenas)                        │
 * │   })                                                                │
 * │   6. catch outer: Alerta.TratamentoErroComLinha                     │
 * │ → Clipboard API: event.clipboardData.items iterado                  │
 * │ → FileReader.readAsDataURL converte blob para data URL             │
 * │ → reader.result format: "data:image/png;base64,iVBOR..."           │
 * │ → split(",")[1] extrai base64 puro (sem prefix)                    │
 * │ → executeCommand('insertHTML') inserta imagem no cursor position   │
 * │ → Break após primeira imagem (não múltiplas simultaneamente)       │
 * │ → Hardcoded data:image/png (mesmo se JPEG/GIF clipboard)           │
 * │ → Usado em exibe-viagem.js para rteDescricao                       │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.configurarLocalizacaoSyncfusion() ──────────────────────────┐
 * │ → Configura localização PT-BR completa para Syncfusion             │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. const L10n = ej.base.L10n                                      │
 * │   3. L10n.load({                                                    │
 * │      pt: { calendar: { today: "Hoje" } },                           │
 * │      "pt-BR": {                                                     │
 * │        calendar: { today: "Hoje" },                                 │
 * │        richtexteditor: { ... 140+ strings traduzidas ... }          │
 * │      }                                                              │
 * │   })                                                                │
 * │   4. if ej.base && ej.base.setCulture:                              │
 * │        ej.base.setCulture('pt-BR')                                  │
 * │   5. if ej.base && ej.base.loadCldr:                                │
 * │        const ptBRCldr = { ... CLDR data structure ... }             │
 * │        ej.base.loadCldr(ptBRCldr)                                   │
 * │   6. catch: Alerta.TratamentoErroComLinha                           │
 * │ → L10n.load traduz strings UI dos componentes                       │
 * │ → setCulture('pt-BR') ativa cultura portuguesa (formatting)         │
 * │ → loadCldr carrega CLDR data para meses/dias em português          │
 * │ → ptBRCldr object structure: main.pt-BR.dates.calendars.gregorian  │
 * │   com months (abbreviated: jan-dez, wide: janeiro-dezembro) e      │
 * │   days (abbreviated: dom-sáb, wide: domingo-sábado)                │
 * │ → RichTextEditor translations: toolbar items (bold, italic, etc),  │
 * │   dialogs (insert link, insert image, insert table), alignment,    │
 * │   formats, paste options, font names                                │
 * │ → Hardcoded inline (não carrega de arquivo externo)                │
 * │ → Chamado no startup da aplicação (main.js ou similar)             │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.onCreate() (RTE callback) ───────────────────────────────────┐
 * │ → Callback onCreate do RichTextEditor (armazena referência global) │
 * │ → Context: this = instância RichTextEditor                          │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. window.defaultRTE = this                                       │
 * │   3. catch: Alerta.TratamentoErroComLinha                           │
 * │ → Configurado via RichTextEditor created callback                   │
 * │ → window.defaultRTE usado para acesso global ao RTE instance       │
 * │ → Permite manipulação do RTE de qualquer parte do código           │
 * │ → Mantido para compatibilidade (legacy pattern)                    │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.toolbarClick(e) (RTE callback) ──────────────────────────────┐
 * │ → Callback toolbarClick do RichTextEditor (adiciona XSRF token)    │
 * │ → param e: Event, toolbar click event object com e.item.id          │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch outer                                                │
 * │   2. if e.item.id == "rte_toolbar_Image":                           │
 * │      a. const element = getElementById("rte_upload")                │
 * │      b. if element && ej2_instances[0]:                             │
 * │         - element.ej2_instances[0].uploading = function(args) {     │
 * │             try-catch inner:                                        │
 * │               args.currentRequest.setRequestHeader("XSRF-TOKEN",    │
 * │                 document.getElementsByName("__RequestVerificationToken")│
 * │                   [0].value)                                        │
 * │           }                                                         │
 * │   3. catch outer: Alerta.TratamentoErroComLinha                     │
 * │ → Intercepta click em botão Image da toolbar                       │
 * │ → Adiciona anti-CSRF token ao upload request                        │
 * │ → __RequestVerificationToken é input hidden do ASP.NET             │
 * │ → currentRequest é XMLHttpRequest do uploader Syncfusion           │
 * │ → setRequestHeader adiciona custom header XSRF-TOKEN               │
 * │ → Backend valida token para prevenir CSRF attacks                  │
 * │ → Configurado via RichTextEditor toolbarClick callback             │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.onDateChange(args) (Calendar callback) ──────────────────────┐
 * │ → Callback change do Calendar (armazena datas selecionadas)        │
 * │ → param args: Object, change event args com args.values array      │
 * │ → returns void                                                      │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. window.selectedDates = args.values                             │
 * │   3. catch: Alerta.TratamentoErroComLinha                           │
 * │ → Armazena múltiplas datas selecionadas (multi-select Calendar)    │
 * │ → window.selectedDates usado por código externo para obter seleção │
 * │ → args.values é array de Date objects                               │
 * │ → Configurado via Calendar change callback                          │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * 📌 RICHTEXTEDITOR PT-BR TRANSLATIONS (140+ strings):
 * Categorias:
 * - Toolbar items: bold, italic, underline, strikethrough, fontName, fontSize,
 *   fontColor, backgroundColor, alignments, lists, indent/outdent, undo/redo,
 *   superscript/subscript, formats, clearFormat, fullscreen
 * - Link dialog: createLink, insertLink, editLink, removeLink, linkText, linkWebUrl,
 *   linkTitle, linkTooltipLabel, linkOpenInNewWindow, linkHeader
 * - Image dialog: image, imageHeader, imageUrl, imageAlternateText, imageCaption,
 *   imageSizeHeader, imageHeight, imageWidth, imageUploadMessage, imageDeviceUploadMessage,
 *   browse, imageInsertLinkHeader, editImageHeader, imageDisplayDropDown*
 * - Table: inserttablebtn, createTable, removeTable, tableHeader, tableWidth,
 *   cellpadding, cellspacing, columns, rows, tableRows, tableColumns, tableCellHorizontalAlign,
 *   tableCellVerticalAlign, tableCellBackground, tableEditProperties, insertColumn*,
 *   deleteColumn, insertRow*, deleteRow, tableEditHeader, TableHeadingText, TableColText,
 *   tableVerticalAlignDropDown*, tableStylesDropDown*
 * - Paste: pasteFormat, pasteFormatContent, plainText, cleanFormat, keepFormat
 * - Formats dropdown: formatsDropDownParagraph, formatsDropDownCode, formatsDropDownQuotation,
 *   formatsDropDownHeading1-4
 * - Font names: fontNameSegoeUI, fontNameArial, fontNameGeorgia, fontNameImpact,
 *   fontNameTahoma, fontNameTimesNewRoman, fontNameVerdana
 * - Misc: sourcecode, preview, print, styles, lowerCase, upperCase, textPlaceholder
 *
 * 📌 CLDR DATA STRUCTURE (PT-BR):
 * {
 *   "main": {
 *     "pt-BR": {
 *       "identity": { "version": { "_cldrVersion": "36" }, "language": "pt" },
 *       "dates": {
 *         "calendars": {
 *           "gregorian": {
 *             "months": {
 *               "format": {
 *                 "abbreviated": { "1": "jan", ..., "12": "dez" },
 *                 "wide": { "1": "janeiro", ..., "12": "dezembro" }
 *               }
 *             },
 *             "days": {
 *               "format": {
 *                 "abbreviated": { "sun": "dom", ..., "sat": "sáb" },
 *                 "wide": { "sun": "domingo", ..., "sat": "sábado" }
 *               }
 *             }
 *           }
 *         }
 *       }
 *     }
 *   }
 * }
 *
 * 📌 USAGE PATTERNS:
 * - getSyncfusionInstance: const dropdown = getSyncfusionInstance("lstFinalidade");
 *   if (dropdown) { dropdown.value = 5; dropdown.dataBind(); }
 * - getSfValue0: const firstValue = getSfValue0(multiselectInstance); // sempre primitivo
 * - limpaTooltipsGlobais: $('#modalViagens').on('hidden.bs.modal', () =>
 *   limpaTooltipsGlobais(200));
 * - rebuildLstPeriodos: window.dataPeriodos = newData; rebuildLstPeriodos();
 * - initializeModalTooltips: $('#modalViagens').on('shown.bs.modal',
 *   initializeModalTooltips);
 * - setupRTEImagePaste: setupRTEImagePaste("rteDescricao"); // no RTE init
 * - configurarLocalizacaoSyncfusion: $(document).ready(() =>
 *   configurarLocalizacaoSyncfusion());
 * - onCreate: new RichTextEditor({ created: onCreate, ... })
 * - toolbarClick: new RichTextEditor({ toolbarClick: toolbarClick, ... })
 * - onDateChange: new Calendar({ change: onDateChange, ... })
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Todas as 10 funções exportadas para window (global scope)
 * - Try-catch em 100% das funções (10/10)
 * - Nested try-catch em 4 funções (limpaTooltipsGlobais, initializeModalTooltips,
 *   setupRTEImagePaste, toolbarClick)
 * - Optional chaining (?.) usado em 7 funções para safe property access
 * - Nullish coalescing (??) usado em getSfValue0
 * - Array || [] fallback em rebuildLstPeriodos
 * - Silent returns (sem throw) em várias funções se elementos não existem
 * - FileReader async pattern (onloadend callback) em setupRTEImagePaste
 * - Clipboard API não suportado em browsers antigos (graceful degradation via try-catch)
 * - CLDR data hardcoded (alternativa: carregar de arquivo .json externo)
 * - L10n.load pode ser chamado múltiplas vezes (merge de translations)
 * - setCulture afeta formatting de datas/números globalmente
 * - loadCldr necessário para nomes de meses/dias (não apenas numbers)
 * - Syncfusion ej2_instances sempre array (pode ter múltiplas instances no mesmo element,
 *   mas geralmente [0] é o único)
 * - jQuery usado apenas para Bootstrap tooltip disposal (legacy)
 * - .e-tooltip-wrap é container HTML do Syncfusion Tooltip
 * - .e-control.e-tooltip é instância Syncfusion Tooltip (tem ej2_instances)
 * - [title] attribute pode causar native browser tooltips (removeAttribute limpa)
 * - XSRF-TOKEN header name customizado (padrão seria X-XSRF-TOKEN ou X-CSRF-TOKEN)
 * - __RequestVerificationToken input hidden gerado por @Html.AntiForgeryToken()
 * - Image paste hardcoded para PNG (data:image/png) mesmo se clipboard tem JPEG/GIF
 * - executeCommand('insertHTML') é method genérico RTE para inserção de HTML arbitrário
 * - reader.result é data URL completo, split(",")[1] remove prefix "data:image/...;base64,"
 * - Break após primeira imagem clipboard (não itera restantes se múltiplas copiadas)
 * - Calendar args.values é array mesmo em single-select mode (consistency)
 * - window.defaultRTE, window.selectedDates, window.dataPeriodos são global state
 *   (não ideal, mas pattern comum em aplicação legacy)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Obtém instância Kendo UI widget de um elemento (compatibilidade com getSyncfusionInstance)
 * Tenta todos os tipos de widget Kendo conhecidos no FrotiX.
 * param {string} id - ID do elemento
 * returns {Object|null} Instância wrapper Kendo ou null
 */
window.getSyncfusionInstance = function (id)
{
    try
    {
        const $el = $("#" + id);
        if (!$el.length) return null;

        // Mapeamento de tipos de widget Kendo por ID (known controls)
        const kendoWidgetTypes = [
            "kendoDropDownList",
            "kendoComboBox",
            "kendoMultiSelect",
            "kendoDatePicker",
            "kendoTimePicker",
            "kendoDateTimePicker",
            "kendoNumericTextBox",
            "kendoTreeView",
            "kendoEditor",
            "kendoGrid",
            "kendoUpload"
        ];

        for (var i = 0; i < kendoWidgetTypes.length; i++)
        {
            var widget = $el.data(kendoWidgetTypes[i]);
            if (widget) return widget;
        }

        // Fallback: checar se elemento ainda tem ej2_instances (compat layer do kendo-editor-helper)
        const el = $el[0];
        if (el && Array.isArray(el.ej2_instances) && el.ej2_instances.length > 0 && el.ej2_instances[0])
        {
            return el.ej2_instances[0];
        }

        return null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "getSyncfusionInstance", error);
        return null;
    }
};

/**
 * Obtém primeiro valor de um componente (Kendo ou Syncfusion compat)
 * param {Object} inst - Instância widget (Kendo ou Syncfusion compat)
 * returns {*} Primeiro valor ou null
 */
window.getSfValue0 = function (inst)
{
    try
    {
        if (!inst) return null;
        // Kendo widgets usam .value() como método, Syncfusion compat usa .value como propriedade
        var v = (typeof inst.value === 'function') ? inst.value() : inst.value;
        if (Array.isArray(v)) return v.length ? v[0] : null;
        return v ?? null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "getSfValue0", error);
        return null;
    }
};

/**
 * Limpa tooltips globais (Syncfusion remnants + Bootstrap)
 * param {number} timeout - Timeout em ms
 */
window.limpaTooltipsGlobais = function (timeout = 200)
{
    try
    {
        setTimeout(() =>
        {
            try
            {
                // Limpar tooltips Syncfusion remanescentes (se houver)
                document.querySelectorAll(".e-tooltip-wrap").forEach(t =>
                {
                    try { t.remove(); } catch (e) { /* silenciar */ }
                });

                // Limpar atributos title que geram tooltips nativos
                document.querySelectorAll("[title]").forEach(el =>
                {
                    try { el.removeAttribute("title"); } catch (e) { /* silenciar */ }
                });

                // Limpar tooltips Bootstrap (jQuery)
                try {
                    $('[data-bs-toggle="tooltip"]').tooltip("dispose");
                    $(".tooltip").remove();
                } catch (e) { /* silenciar */ }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_timeout", error);
            }
        }, timeout);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais", error);
    }
};

/**
 * Rebuilda lista de períodos (migrado para Kendo DropDownList)
 */
window.rebuildLstPeriodos = function ()
{
    try
    {
        var existente = $("#lstPeriodos").data("kendoDropDownList");
        if (existente) existente.destroy();

        $("#lstPeriodos").kendoDropDownList({
            dataSource: window.dataPeriodos || [],
            dataTextField: "periodo",
            dataValueField: "periodoId",
            optionLabel: "Selecione o período",
            filter: "contains",
            height: 200
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "rebuildLstPeriodos", error);
    }
};

/**
 * Inicializa tooltips em modal (refresh Syncfusion ejTooltip global)
 */
window.initializeModalTooltips = function ()
{
    try
    {
        // Tooltips FrotiX usam data-ejtip com Syncfusion global (ejTooltip)
        if (window.ejTooltip) {
            window.ejTooltip.refresh();
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "initializeModalTooltips", error);
    }
};

/**
 * Configura RichTextEditor (Kendo Editor) para paste de imagens
 * param {string} rteId - ID do elemento textarea do editor
 */
window.setupRTEImagePaste = function (rteId)
{
    try
    {
        var editor = $("#" + rteId).data("kendoEditor");
        if (!editor) {
            console.warn('[syncfusion.utils] setupRTEImagePaste: Kendo Editor não encontrado para #' + rteId);
            return;
        }

        // Kendo Editor: body é o contentEditable iframe body
        var editorBody = $(editor.body);
        if (!editorBody.length) return;

        editorBody.on("paste", function (event)
        {
            try
            {
                var clipboardData = event.originalEvent ? event.originalEvent.clipboardData : event.clipboardData;

                if (clipboardData && clipboardData.items)
                {
                    var items = clipboardData.items;

                    for (var i = 0; i < items.length; i++)
                    {
                        var item = items[i];

                        if (item.type.indexOf("image") !== -1)
                        {
                            var blob = item.getAsFile();
                            var reader = new FileReader();

                            reader.onloadend = function ()
                            {
                                try
                                {
                                    var base64Image = reader.result.split(",")[1];
                                    var pastedHtml = '<img src="data:image/png;base64,' + base64Image + '" />';
                                    editor.exec("inserthtml", { value: pastedHtml });
                                } catch (error)
                                {
                                    Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupRTEImagePaste_onloadend", error);
                                }
                            };

                            reader.readAsDataURL(blob);
                            break;
                        }
                    }
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupRTEImagePaste_paste", error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupRTEImagePaste", error);
    }
};

/**
 * [MIGRADO] Configuração de localização Syncfusion - NÃO mais necessário.
 * Kendo UI pt-BR já é configurado globalmente via _ScriptsBasePlugins.cshtml
 * (kendo.culture.pt-BR.min.js + kendo.messages.pt-BR.min.js).
 * Mantido como no-op para compatibilidade com callers existentes.
 */
window.configurarLocalizacaoSyncfusion = function ()
{
    try
    {
        console.log('[syncfusion.utils] configurarLocalizacaoSyncfusion: Kendo UI pt-BR já configurado globalmente. Nenhuma ação necessária.');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "configurarLocalizacaoSyncfusion", error);
    }
};

/**
 * [MIGRADO] Callbacks globais do RTE - adaptados para Kendo Editor
 * onCreate: armazena referência global ao Kendo Editor
 */
window.onCreate = function ()
{
    try
    {
        // Kendo Editor: obter instância via jQuery
        var editor = $("#rte").data("kendoEditor") || $("#rteDescricao").data("kendoEditor");
        if (editor) {
            window.defaultRTE = editor;
        } else {
            // Fallback: 'this' pode ser a instância se chamado como callback
            window.defaultRTE = this;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "onCreate", error);
    }
};

/**
 * [MIGRADO] toolbarClick - Kendo Editor não precisa de XSRF manual
 * (Kendo Upload já suporta antiForgeryToken via Upload.async config)
 * Mantido como no-op para compatibilidade.
 */
window.toolbarClick = function (e)
{
    try
    {
        // Kendo Editor toolbar click - sem ação necessária para XSRF
        // (Kendo Upload gerencia CSRF automaticamente via kendo.antiForgeryTokens())
        console.log('[syncfusion.utils] toolbarClick: Kendo Editor - sem ação XSRF necessária');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "toolbarClick", error);
    }
};

/**
 * Callback de mudança de data (calendário) - mantido para compat
 */
window.onDateChange = function (args)
{
    try
    {
        window.selectedDates = args.values || args.value || [];
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "onDateChange", error);
    }
};

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * BRIDGE: getKendoWidget(id) - Obtém widget Kendo por ID do elemento
 * ═══════════════════════════════════════════════════════════════════════════
 * Helper central para migração Syncfusion→Kendo.
 * Retorna o widget Kendo associado ao elemento, ou null.
 *
 * param {string} id - ID do elemento DOM
 * returns {Object|null} Widget Kendo ou null
 */
window.getKendoWidget = function (id)
{
    try
    {
        return window.getSyncfusionInstance(id); // Reutiliza lógica já migrada
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "getKendoWidget", error);
        return null;
    }
};

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * BRIDGE: Compatibilidade ej2_instances → Kendo
 * ═══════════════════════════════════════════════════════════════════════════
 * Intercepta acesso a el.ej2_instances para retornar wrapper Kendo compatível.
 * Isso permite que código legado como:
 *   document.getElementById("lstVeiculo").ej2_instances[0].value
 * funcione com widgets Kendo sem alteração.
 *
 * IMPORTANTE: Apenas para período de transição! Código novo deve usar
 * getSyncfusionInstance(id) ou $(el).data("kendoXxx").
 */
window.setupEj2InstancesBridge = function ()
{
    try
    {
        var controlIds = [
            "lstFinalidade", "lstMotorista", "lstVeiculo", "lstRequisitante",
            "lstRequisitanteEvento", "lstEventos", "lstRecorrente", "lstDias",
            "lstDiasMes", "lstPeriodos", "lstSetorRequisitanteAgendamento",
            "ddtCombustivelInicial", "ddtCombustivelFinal",
            "ddtSetorRequisitante",
            "txtDataInicial", "txtDataFinal", "txtHoraInicial", "txtHoraFinal",
            "txtDataInicioEvento", "txtDataFimEvento", "txtFinalRecorrencia",
            "txtDataInicialEvento", "txtDataFinalEvento",
            "txtDuracao", "txtQtdParticipantesEvento", "txtQtdParticipantesEventoCadastro",
            "cmbOrigem", "cmbDestino",
            "rte", "rteDescricao"
        ];

        controlIds.forEach(function (id) {
            try {
                var el = document.getElementById(id);
                if (!el) return;

                // Pular se já tem ej2_instances (ex: kendo-editor-helper compat layer)
                if (el.ej2_instances && Array.isArray(el.ej2_instances)) return;

                // Definir getter lazy para ej2_instances que retorna wrapper Kendo
                Object.defineProperty(el, 'ej2_instances', {
                    get: function () {
                        var widget = window.getSyncfusionInstance(id);
                        if (!widget) return null;

                        // Criar wrapper compatível com API Syncfusion
                        var wrapper = {
                            // value: getter/setter que mapeia para API Kendo
                            get value() {
                                try {
                                    return (typeof widget.value === 'function') ? widget.value() : widget.value;
                                } catch (e) { return null; }
                            },
                            set value(v) {
                                try {
                                    if (typeof widget.value === 'function') {
                                        widget.value(v);
                                    }
                                } catch (e) { /* silenciar */ }
                            },
                            // text: getter/setter
                            get text() {
                                try {
                                    return (typeof widget.text === 'function') ? widget.text() : (widget.text || '');
                                } catch (e) { return ''; }
                            },
                            set text(v) {
                                try {
                                    if (typeof widget.text === 'function') widget.text(v);
                                } catch (e) { /* silenciar */ }
                            },
                            // enabled: getter/setter
                            get enabled() {
                                try {
                                    return widget.options ? widget.options.enable !== false : true;
                                } catch (e) { return true; }
                            },
                            set enabled(v) {
                                try {
                                    if (typeof widget.enable === 'function') widget.enable(v);
                                } catch (e) { /* silenciar */ }
                            },
                            // dataBind: no-op (Kendo atualiza automaticamente)
                            dataBind: function () {
                                // Kendo widgets atualizam automaticamente - no-op
                            },
                            // refresh: mapeia para Kendo
                            refresh: function () {
                                try {
                                    if (typeof widget.refresh === 'function') widget.refresh();
                                } catch (e) { /* silenciar */ }
                            },
                            // destroy
                            destroy: function () {
                                try {
                                    if (typeof widget.destroy === 'function') widget.destroy();
                                } catch (e) { /* silenciar */ }
                            },
                            // addItem: para ComboBox/DropDownList
                            addItem: function (item) {
                                try {
                                    if (widget.dataSource && typeof widget.dataSource.add === 'function') {
                                        widget.dataSource.add(item);
                                    }
                                } catch (e) { /* silenciar */ }
                            },
                            // dataSource getter/setter
                            get dataSource() {
                                try {
                                    return widget.dataSource;
                                } catch (e) { return null; }
                            },
                            set dataSource(v) {
                                try {
                                    if (widget.setDataSource) {
                                        widget.setDataSource(v);
                                    } else if (widget.dataSource) {
                                        widget.dataSource.data(v);
                                    }
                                } catch (e) { /* silenciar */ }
                            },
                            // fields: para compat com ej2 fields.dataSource
                            fields: {
                                get dataSource() {
                                    try {
                                        return widget.dataSource ? widget.dataSource.data() : [];
                                    } catch (e) { return []; }
                                },
                                set dataSource(v) {
                                    try {
                                        if (widget.dataSource) widget.dataSource.data(v);
                                    } catch (e) { /* silenciar */ }
                                }
                            },
                            // selectedNodes: para TreeView compat
                            get selectedNodes() {
                                try {
                                    if (typeof widget.select === 'function') {
                                        var node = widget.select();
                                        if (node && node.length) {
                                            var item = widget.dataItem(node);
                                            return item ? [item.id || item.setorSolicitanteId] : [];
                                        }
                                    }
                                    return [];
                                } catch (e) { return []; }
                            },
                            set selectedNodes(v) {
                                try {
                                    if (Array.isArray(v) && v.length === 0 && typeof widget.select === 'function') {
                                        widget.select($());
                                    }
                                } catch (e) { /* silenciar */ }
                            },
                            // Kendo widget reference
                            _kendoWidget: widget
                        };

                        return [wrapper];
                    },
                    configurable: true,
                    enumerable: false
                });
            } catch (e) {
                // Silenciar - elemento pode não existir na página atual
            }
        });

        console.log('[syncfusion.utils] ✅ Bridge ej2_instances→Kendo configurada para ' + controlIds.length + ' controles');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupEj2InstancesBridge", error);
    }
};

// Configurar bridge automaticamente quando DOM estiver pronto e Kendo inicializado
$(document).ready(function () {
    // Esperar Kendo widgets serem inicializados (após ScriptsBlock)
    setTimeout(function () {
        window.setupEj2InstancesBridge();
    }, 1000);
});
