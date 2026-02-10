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
 * Obtém instância Syncfusion de um elemento
 * param {string} id - ID do elemento
 * returns {Object|null} Instância Syncfusion ou null
 */
window.getSyncfusionInstance = function (id)
{
    try
    {
        const el = document.getElementById(id);
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
 * Obtém primeiro valor de um componente Syncfusion
 * param {Object} inst - Instância Syncfusion
 * returns {*} Primeiro valor ou null
 */
window.getSfValue0 = function (inst)
{
    try
    {
        if (!inst) return null;
        const v = inst.value;
        if (Array.isArray(v)) return v.length ? v[0] : null;
        return v ?? null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "getSfValue0", error);
        return null;
    }
};

/**
 * Limpa tooltips globais Syncfusion
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
                document.querySelectorAll(".e-tooltip-wrap").forEach(t =>
                {
                    try
                    {
                        t.remove();
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_remove", error);
                    }
                });

                document.querySelectorAll(".e-control.e-tooltip").forEach(el =>
                {
                    try
                    {
                        const instance = el.ej2_instances?.[0];
                        if (instance?.destroy) instance.destroy();
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_destroy", error);
                    }
                });

                document.querySelectorAll("[title]").forEach(el =>
                {
                    try
                    {
                        el.removeAttribute("title");
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_removeAttr", error);
                    }
                });

                $('[data-bs-toggle="tooltip"]').tooltip("dispose");
                $(".tooltip").remove();
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
 * Rebuilda lista de períodos
 */
window.rebuildLstPeriodos = function ()
{
    try
    {
        new ej.dropdowns.DropDownList({
            dataSource: window.dataPeriodos || [],
            fields: {
                value: "PeriodoId",
                text: "Periodo"
            },
            placeholder: "Selecione o período",
            allowFiltering: true,
            showClearButton: true,
            sortOrder: "Ascending"
        }).appendTo("#lstPeriodos");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "rebuildLstPeriodos", error);
    }
};

/**
 * Inicializa tooltips Syncfusion em modal
 */
window.initializeModalTooltips = function ()
{
    try
    {
        const tooltipElements = document.querySelectorAll('[data-ejtip]');
        tooltipElements.forEach(function (element)
        {
            try
            {
                new ej.popups.Tooltip({
                    target: element
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("syncfusion.utils.js", "initializeModalTooltips_forEach", error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "initializeModalTooltips", error);
    }
};

/**
 * Configura RichTextEditor para paste de imagens
 * param {string} rteId - ID do RichTextEditor
 */
window.setupRTEImagePaste = function (rteId)
{
    try
    {
        const rteDescricao = document.getElementById(rteId);
        if (!rteDescricao || !rteDescricao.ej2_instances || !rteDescricao.ej2_instances[0])
        {
            return;
        }

        const rte = rteDescricao.ej2_instances[0];

        rte.element.addEventListener("paste", function (event)
        {
            try
            {
                const clipboardData = event.clipboardData;

                if (clipboardData && clipboardData.items)
                {
                    const items = clipboardData.items;

                    for (let i = 0; i < items.length; i++)
                    {
                        const item = items[i];

                        if (item.type.indexOf("image") !== -1)
                        {
                            const blob = item.getAsFile();
                            const reader = new FileReader();

                            reader.onloadend = function ()
                            {
                                try
                                {
                                    const base64Image = reader.result.split(",")[1];
                                    const pastedHtml = `<img src="data:image/png;base64,${base64Image}" />`;
                                    rte.executeCommand('insertHTML', pastedHtml);
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
 * Configuração global de localização Syncfusion para PT-BR
 */
window.configurarLocalizacaoSyncfusion = function ()
{
    try
    {
        // Configurar L10n (textos dos componentes)
        const L10n = ej.base.L10n;
        L10n.load({
            pt: {
                calendar: {
                    today: "Hoje"
                }
            },
            "pt-BR": {
                calendar: {
                    today: "Hoje"
                },
                richtexteditor: {
                    alignments: "Alinhamentos",
                    justifyLeft: "Alinhar à Esquerda",
                    justifyCenter: "Centralizar",
                    justifyRight: "Alinhar à Direita",
                    justifyFull: "Justificar",
                    fontName: "Nome da Fonte",
                    fontSize: "Tamanho da Fonte",
                    fontColor: "Cor da Fonte",
                    backgroundColor: "Cor de Fundo",
                    bold: "Negrito",
                    italic: "Itálico",
                    underline: "Sublinhado",
                    strikethrough: "Tachado",
                    clearFormat: "Limpa Formatação",
                    clearAll: "Limpa Tudo",
                    cut: "Cortar",
                    copy: "Copiar",
                    paste: "Colar",
                    unorderedList: "Lista com Marcadores",
                    orderedList: "Lista Numerada",
                    indent: "Aumentar Identação",
                    outdent: "Diminuir Identação",
                    undo: "Desfazer",
                    redo: "Refazer",
                    superscript: "Sobrescrito",
                    subscript: "Subscrito",
                    createLink: "Inserir Link",
                    openLink: "Abrir Link",
                    editLink: "Editar Link",
                    removeLink: "Remover Link",
                    image: "Inserir Imagem",
                    replace: "Substituir",
                    align: "Alinhar",
                    caption: "Título da Imagem",
                    remove: "Remover",
                    insertLink: "Inserir Link",
                    display: "Exibir",
                    altText: "Texto Alternativo",
                    dimension: "Mudar Tamanho",
                    fullscreen: "Maximizar",
                    maximize: "Maximizar",
                    minimize: "Minimizar",
                    lowerCase: "Caixa Baixa",
                    upperCase: "Caixa Alta",
                    print: "Imprimir",
                    formats: "Formatos",
                    sourcecode: "Visualizar Código",
                    preview: "Exibir",
                    viewside: "ViewSide",
                    insertCode: "Inserir Código",
                    linkText: "Exibir Texto",
                    linkTooltipLabel: "Título",
                    linkWebUrl: "Endereço Web",
                    linkTitle: "Entre com um título",
                    linkurl: "http://exemplo.com",
                    linkOpenInNewWindow: "Abrir Link em Nova Janela",
                    linkHeader: "Inserir Link",
                    dialogInsert: "Inserir",
                    dialogCancel: "Cancelar",
                    dialogUpdate: "Atualizar",
                    imageHeader: "Inserir Imagem",
                    imageLinkHeader: "Você pode proporcionar um link da web",
                    mdimageLink: "Favor proporcionar uma URL para sua imagem",
                    imageUploadMessage: "Solte a imagem aqui ou busque para o upload",
                    imageDeviceUploadMessage: "Clique aqui para o upload",
                    imageAlternateText: "Texto Alternativo",
                    alternateHeader: "Texto Alternativo",
                    browse: "Procurar",
                    imageUrl: "http://exemplo.com/imagem.png",
                    imageCaption: "Título",
                    imageSizeHeader: "Tamanho da Imagem",
                    imageHeight: "Altura",
                    imageWidth: "Largura",
                    textPlaceholder: "Entre com um Texto",
                    inserttablebtn: "Inserir Tabela",
                    tabledialogHeader: "Inserir Tabela",
                    tableWidth: "Largura",
                    cellpadding: "Espaçamento de célula",
                    cellspacing: "Espaçamento de célula",
                    columns: "Número de colunas",
                    rows: "Número de linhas",
                    tableRows: "Linhas da Tabela",
                    tableColumns: "Colunas da Tabela",
                    tableCellHorizontalAlign: "Alinhamento Horizontal da Célular",
                    tableCellVerticalAlign: "Alinhamento Vertical da Célular",
                    createTable: "Criar Tabela",
                    removeTable: "Remover Tabela",
                    tableHeader: "Cabeçalho da Tabela",
                    tableRemove: "Remover Tabela",
                    tableCellBackground: "Cor de Fundo da Célula",
                    tableEditProperties: "Editar Propriedades da Tabela",
                    styles: "Estilos",
                    insertColumnLeft: "Inserir Coluna à Esquerda",
                    insertColumnRight: "Inserir Coluna à Direita",
                    deleteColumn: "Remover Coluna",
                    insertRowBefore: "Inserir Linha Acima",
                    insertRowAfter: "Inserir Linha Abaixo",
                    deleteRow: "Remover Linha",
                    tableEditHeader: "Editar Tabela",
                    TableHeadingText: "Cabeçalho",
                    TableColText: "Coluna",
                    imageInsertLinkHeader: "Inserir Link",
                    editImageHeader: "Editar Imagem",
                    alignmentsDropDownLeft: "Alinhar Esquerda",
                    alignmentsDropDownCenter: "Alinhar Centro",
                    alignmentsDropDownRight: "Alinhar Direita",
                    alignmentsDropDownJustify: "Alinhar Justificar",
                    imageDisplayDropDownInline: "Na Linha",
                    imageDisplayDropDownBreak: "Quebrar",
                    tableInsertRowDropDownBefore: "Inserir linha acima",
                    tableInsertRowDropDownAfter: "Inserir linha abaixo",
                    tableInsertRowDropDownDelete: "Deletar linha",
                    tableInsertColumnDropDownLeft: "Inserir coluna esquerda",
                    tableInsertColumnDropDownRight: "Inserir coluna direita",
                    tableInsertColumnDropDownDelete: "Deletar coluna",
                    tableVerticalAlignDropDownTop: "Alinhar Topo",
                    tableVerticalAlignDropDownMiddle: "Alinhar Meio",
                    tableVerticalAlignDropDownBottom: "Alinhar Inferior",
                    tableStylesDropDownDashedBorder: "Bordas Tracejadas",
                    tableStylesDropDownAlternateRows: "Linhas Alternadas",
                    pasteFormat: "Formato de Colagem",
                    pasteFormatContent: "Escolha o formato que deseja colar.",
                    plainText: "Texto Sem Formatação",
                    cleanFormat: "Limpar",
                    keepFormat: "Manter",
                    formatsDropDownParagraph: "Parágrafo",
                    formatsDropDownCode: "Código",
                    formatsDropDownQuotation: "Citação",
                    formatsDropDownHeading1: "Cabeçalho 1",
                    formatsDropDownHeading2: "Cabeçalho 2",
                    formatsDropDownHeading3: "Cabeçalho 3",
                    formatsDropDownHeading4: "Cabeçalho 4",
                    fontNameSegoeUI: "SegoeUI",
                    fontNameArial: "Arial",
                    fontNameGeorgia: "Georgia",
                    fontNameImpact: "Impact",
                    fontNameTahoma: "Tahoma",
                    fontNameTimesNewRoman: "Times New Roman",
                    fontNameVerdana: "Verdana"
                }
            }
        });

        // Configurar cultura pt-BR (para nomes de meses e dias)
        if (ej.base && ej.base.setCulture)
        {
            ej.base.setCulture('pt-BR');
        }

        // Carregar dados CLDR para português
        if (ej.base && ej.base.loadCldr)
        {
            const ptBRCldr = {
                "main": {
                    "pt-BR": {
                        "identity": {
                            "version": {
                                "_cldrVersion": "36"
                            },
                            "language": "pt"
                        },
                        "dates": {
                            "calendars": {
                                "gregorian": {
                                    "months": {
                                        "format": {
                                            "abbreviated": {
                                                "1": "jan",
                                                "2": "fev",
                                                "3": "mar",
                                                "4": "abr",
                                                "5": "mai",
                                                "6": "jun",
                                                "7": "jul",
                                                "8": "ago",
                                                "9": "set",
                                                "10": "out",
                                                "11": "nov",
                                                "12": "dez"
                                            },
                                            "wide": {
                                                "1": "janeiro",
                                                "2": "fevereiro",
                                                "3": "março",
                                                "4": "abril",
                                                "5": "maio",
                                                "6": "junho",
                                                "7": "julho",
                                                "8": "agosto",
                                                "9": "setembro",
                                                "10": "outubro",
                                                "11": "novembro",
                                                "12": "dezembro"
                                            }
                                        }
                                    },
                                    "days": {
                                        "format": {
                                            "abbreviated": {
                                                "sun": "dom",
                                                "mon": "seg",
                                                "tue": "ter",
                                                "wed": "qua",
                                                "thu": "qui",
                                                "fri": "sex",
                                                "sat": "sáb"
                                            },
                                            "wide": {
                                                "sun": "domingo",
                                                "mon": "segunda",
                                                "tue": "terça",
                                                "wed": "quarta",
                                                "thu": "quinta",
                                                "fri": "sexta",
                                                "sat": "sábado"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            ej.base.loadCldr(ptBRCldr);
        }

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "configurarLocalizacaoSyncfusion", error);
    }
};

/**
 * Callbacks globais do RTE (mantidos para compatibilidade)
 */
window.onCreate = function ()
{
    try
    {
        window.defaultRTE = this;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "onCreate", error);
    }
};

window.toolbarClick = function (e)
{
    try
    {
        if (e.item.id == "rte_toolbar_Image")
        {
            const element = document.getElementById("rte_upload");
            if (element && element.ej2_instances && element.ej2_instances[0])
            {
                element.ej2_instances[0].uploading = function (args)
                {
                    try
                    {
                        args.currentRequest.setRequestHeader("XSRF-TOKEN", document.getElementsByName("__RequestVerificationToken")[0].value);
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "toolbarClick_uploading", error);
                    }
                };
            }
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "toolbarClick", error);
    }
};

/**
 * Callback de mudança de data (calendário)
 */
window.onDateChange = function (args)
{
    try
    {
        window.selectedDates = args.values;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "onDateChange", error);
    }
};
