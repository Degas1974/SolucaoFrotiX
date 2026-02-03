/* ****************************************************************************************
 * ⚡ ARQUIVO: recorrencia-logic.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Lógica de visibilidade e comportamento dos campos de recorrência no
 *                   modal de agendamento. 24 funções para controlar show/hide de campos
 *                   baseado em lstRecorrente ("Sim"/"Não") e lstPeriodos (Diário/Semanal/
 *                   Mensal/Variado), inicializar Syncfusion Calendar multiselect com
 *                   badge contador, configurar event handlers (change), carregar CLDR
 *                   + L10n PT-BR, limpar campos ao mudar tipo recorrência. Principais
 *                   fluxos: lstRecorrente="Sim" → show divPeriodo, lstPeriodos="Semanal"
 *                   → show lstDias + txtFinalRecorrencia, lstPeriodos="Variado" → show
 *                   calDatasSelecionadas (multiSelect Calendar) com badge laranja
 *                   (contador datas). Usa Syncfusion DropDownList, Calendar, DateTimePicker.
 * 📥 ENTRADAS     : Eventos change de Syncfusion components (lstRecorrente, lstPeriodos),
 *                   valores selecionados (RecorrenteId: "S"/"N", PeriodoId: "D"/"S"/"Q"/
 *                   "M"/"V"), valores Calendar (dates array), window.ignorarEventosRecorrencia
 *                   (boolean flag para skip handlers durante load edição)
 * 📤 SAÍDAS       : Void (side effects: DOM display changes, Calendar instance creation,
 *                   badge textContent updates, console.log debug), Calendar instance
 *                   (window.calendario), datasSelecionadas array (window.datasSelecionadas)
 * 🔗 CHAMADA POR  : main.js (DOMContentLoaded → setTimeout 1000ms → inicializarLogicaRecorrencia),
 *                   recorrencia.js (inicializarDropdownPeriodos), modal-viagem-novo.js
 *                   (inicializarCamposModal → set ignorarEventosRecorrencia flag),
 *                   Syncfusion change events (lstRecorrente, lstPeriodos, calDatasSelecionadas)
 * 🔄 CHAMA        : Syncfusion EJ2 API (ej2_instances[0], value setter, dataBind(), refresh(),
 *                   destroy(), appendTo(), Calendar constructor, DropDownList.change),
 *                   ej.base.Ajax (CLDR files load), ej.base.loadCldr, ej.base.setCulture,
 *                   ej.base.L10n.load, window.inicializarDropdownPeriodos (recorrencia.js),
 *                   window.inicializarLstDias/inicializarLstDiasMes (recorrencia.js popula
 *                   dataSource), Alerta.TratamentoErroComLinha, setTimeout (200ms/300ms
 *                   delays para render), setInterval (200ms × 10 tentativas retry),
 *                   jQuery ($(element).empty/append/css/data, $(window).on('resize')),
 *                   DOM API (getElementById, style.setProperty, classList.add/remove,
 *                   createElement, appendChild, querySelector, getComputedStyle)
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 Calendars (ej.calendars.Calendar, ej.base.L10n, ej.base.Ajax,
 *                   ej.base.loadCldr, ej.base.setCulture), Syncfusion DropDownList
 *                   (lstRecorrente, lstPeriodos, lstDias, lstDiasMes), Syncfusion Calendar
 *                   (calDatasSelecionadas com isMultiSelection: true), Syncfusion
 *                   DateTimePicker (txtFinalRecorrencia), jQuery (DOM manipulation,
 *                   events), Alerta (TratamentoErroComLinha), recorrencia.js
 *                   (inicializarDropdownPeriodos, inicializarLstDias, inicializarLstDiasMes),
 *                   DOM elements (divPeriodo, divDias, divDiaMes, divFinalRecorrencia,
 *                   calendarContainer, calDatasSelecionadas, badgeContadorDatas), CLDR
 *                   files (cldr/numberingSystems.json, ca-gregorian.json, numbers.json,
 *                   timeZoneNames.json, weekData.json, pt-BR.json para traduções)
 * 📝 OBSERVAÇÕES  : Arquivo de controle de UI (1395 linhas, 24 funções). 3 global variables:
 *                   window.calendario (Calendar instance, null inicial), window.datasSelecionadas
 *                   (Date array para multiselect), window.ignorarEventosRecorrencia
 *                   (boolean flag para evitar loops durante edição). Todas as funções
 *                   privadas (sem window.* export) exceto inicializarLogicaRecorrencia.
 *                   Try-catch em todas as funções principais com Alerta.TratamentoErroComLinha.
 *                   Console.log extensivo para debug (production-ready). Uso de style.setProperty
 *                   com '!important' para sobrescrever CSS (garantir visibilidade).
 *                   Retry pattern: setInterval 200ms × 10 tentativas para lstPeriodos
 *                   (aguarda render Syncfusion). Delays: setTimeout 100-300ms para aguardar
 *                   DOM completo. Badge: círculo laranja (#ff8c00) 35×35px, position
 *                   absolute, z-index 999999, top/left calculado dinamicamente. CLDR:
 *                   carregamento local via ej.base.Ajax (5 JSON files) + pt-BR.json
 *                   (traduções). Inicialização automática: DOMContentLoaded ou document.readyState
 *                   ready → setTimeout 1000ms → inicializarLogicaRecorrencia. Linhas
 *                   96-136: código duplicado (versão simplificada de inicializarLogicaRecorrencia,
 *                   não executado). Períodos suportados: D (Diário), S (Semanal), Q
 *                   (Quinzenal), M (Mensal), V (Dias Variados). Lógica de visibilidade:
 *                   switch/case + fallback por texto (toLowerCase + includes). Calendar
 *                   multiSelect: min=today (desabilita passado), renderDayCell hook.
 *
 * 📋 ÍNDICE DE FUNÇÕES (24 funções + 3 global variables + 1 auto-init):
 *
 * ┌─ GLOBAL VARIABLES ───────────────────────────────────────────────────┐
 * │ 1. window.calendario = null                                          │
 * │    → Syncfusion Calendar instance (multiSelect)                     │
 * │    → Criado em inicializarCalendarioSyncfusion()                    │
 * │                                                                       │
 * │ 2. window.datasSelecionadas = []                                     │
 * │    → Date array para armazenar datas selecionadas no Calendar       │
 * │    → Atualizado no Calendar.change event                            │
 * │                                                                       │
 * │ 3. window.ignorarEventosRecorrencia = false                          │
 * │    → Boolean flag para skip event handlers durante edição           │
 * │    → Setado true em modal-viagem-novo.inicializarCamposModal        │
 * │    → Evita loops ao preencher campos com dados de edição            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 1: INICIALIZAÇÃO E CONFIGURAÇÃO ─────────────────────────────┐
 * │ 4. window.inicializarLogicaRecorrencia()                             │
 * │    → Inicializa lógica completa de recorrência (entry point)        │
 * │    → returns void (side effect: configura handlers + defaults)      │
 * │    → Fluxo: (89 linhas)                                             │
 * │      1. console.log "Inicializando lógica de recorrência"           │
 * │      2. Se window.inicializarDropdownPeriodos exists:               │
 * │         - Call inicializarDropdownPeriodos() (recorrencia.js)       │
 * │      3. setTimeout 300ms: aguardar dropdown render                  │
 * │         a. esconderTodosCamposRecorrencia()                         │
 * │         b. setTimeout 200ms:                                         │
 * │            - Obter lstRecorrente.ej2_instances[0]                   │
 * │            - Find dataSource item "Não" (RecorrenteId="N")          │
 * │            - lstRecorrente.value = "N", dataBind()                  │
 * │         c. configurarEventHandlerRecorrente()                       │
 * │         d. configurarEventHandlerPeriodo()                          │
 * │      4. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: main.js DOMContentLoaded + 1000ms delay            │
 * │                                                                       │
 * │ 5. esconderTodosCamposRecorrencia()                                  │
 * │    → Esconde todos os campos exceto lstRecorrente                   │
 * │    → returns void (side effect: display='none' !important)          │
 * │    → Fluxo:                                                          │
 * │      1. Array de 5 IDs: divPeriodo, divDias, divDiaMes,            │
 * │         divFinalRecorrencia, calendarContainer                      │
 * │      2. forEach: setProperty('display', 'none', 'important')        │
 * │      3. console.log "Campos escondidos"                             │
 * │    → Uso típico: inicializarLogicaRecorrencia, aoMudarRecorrente("Não")│
 * │                                                                       │
 * │ 6. configurarEventHandlerRecorrente()                                │
 * │    → Configura change handler para lstRecorrente                    │
 * │    → returns void (side effect: lstRecorrente.change = aoMudarRecorrente)│
 * │    → Fluxo:                                                          │
 * │      1. getElementById("lstRecorrente")                             │
 * │      2. Obter ej2_instances[0]                                      │
 * │      3. lstRecorrente.change = aoMudarRecorrente                    │
 * │      4. console.log "Event handler configurado"                     │
 * │    → Chamado por: inicializarLogicaRecorrencia                      │
 * │                                                                       │
 * │ 7. configurarEventHandlerPeriodo()                                   │
 * │    → Configura change handler para lstPeriodos com retry            │
 * │    → returns void (side effect: lstPeriodos.change = aoMudarPeriodo)│
 * │    → Fluxo:                                                          │
 * │      1. setInterval 200ms (retry pattern):                          │
 * │         - tentativas++, max 10 tentativas                           │
 * │         - getElementById("lstPeriodos")                             │
 * │         - Se !exists ou !ej2_instances: continue retry              │
 * │         - Se encontrado: clearInterval                              │
 * │      2. lstPeriodos.change = null (remover anterior)                │
 * │      3. lstPeriodos.change = aoMudarPeriodo                         │
 * │      4. console.log "Event handler configurado"                     │
 * │    → Retry necessário: lstPeriodos criado dinamicamente após        │
 * │      inicializarDropdownPeriodos                                    │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 2: EVENT HANDLERS ───────────────────────────────────────────┐
 * │ 8. aoMudarRecorrente(args)                                           │
 * │    → Handler change de lstRecorrente (Sim/Não)                      │
 * │    → param args: Syncfusion change event args (value, itemData)     │
 * │    → returns void (side effect: show/hide divPeriodo)               │
 * │    → Fluxo: (77 linhas)                                             │
 * │      1. console.log debug completo (args, value, itemData)          │
 * │      2. Se ignorarEventosRecorrencia: return early                  │
 * │      3. Extrair valor: args.value || itemData?.RecorrenteId         │
 * │      4. Extrair descricao: itemData?.Descricao                      │
 * │      5. limparCamposRecorrenciaAoMudar()                            │
 * │      6. Verificar se "Sim": valor="S" || descricao="Sim"            │
 * │      7. Se Sim:                                                      │
 * │         a. divPeriodo.setProperty('display', 'block', '!important') │
 * │         b. Limpar lstPeriodos.value = null                          │
 * │         c. console.log "Mostrar lstPeriodo"                         │
 * │      8. Se Não:                                                      │
 * │         a. esconderTodosCamposRecorrencia()                         │
 * │      9. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: lstRecorrente.change event trigger                 │
 * │                                                                       │
 * │ 9. aoMudarPeriodo(args)                                              │
 * │    → Handler change de lstPeriodos (Diário/Semanal/Mensal/Variado) │
 * │    → param args: Syncfusion change event args                       │
 * │    → returns void (side effect: show campos específicos)            │
 * │    → Fluxo: (103 linhas)                                            │
 * │      1. console.log debug completo                                  │
 * │      2. Se ignorarEventosRecorrencia: return early                  │
 * │      3. Extrair valor: args.value || itemData?.PeriodoId            │
 * │      4. Extrair texto: itemData?.Text || itemData?.Periodo          │
 * │      5. esconderCamposEspecificosPeriodo()                          │
 * │      6. Switch valor:                                               │
 * │         - "D" (Diário): mostrarTxtFinalRecorrencia()                │
 * │         - "S"/"Q" (Semanal/Quinzenal): mostrarLstDias() +           │
 * │           mostrarTxtFinalRecorrencia()                              │
 * │         - "M" (Mensal): mostrarLstDiasMes() +                       │
 * │           mostrarTxtFinalRecorrencia()                              │
 * │         - "V" (Variado): mostrarCalendarioComBadge()                │
 * │         - default: fallback por texto.toLowerCase().includes()      │
 * │      7. console.log "aoMudarPeriodo concluído"                      │
 * │      8. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: lstPeriodos.change event trigger                   │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 3: CONTROLE DE VISIBILIDADE ─────────────────────────────────┐
 * │ 10. esconderCamposEspecificosPeriodo()                               │
 * │     → Esconde campos específicos de período                         │
 * │     → returns void (side effect: display='none' em 4 divs)          │
 * │     → Fluxo:                                                         │
 * │       1. body.classList.remove('modo-criacao-variada',              │
 * │          'modo-edicao-variada')                                     │
 * │       2. Array de 4 IDs: divDias, divDiaMes, divFinalRecorrencia,  │
 * │          calendarContainer                                          │
 * │       3. forEach: setProperty('display', 'none', 'important')       │
 * │     → Chamado por: aoMudarPeriodo antes de show campos              │
 * │                                                                       │
 * │ 11. mostrarTxtFinalRecorrencia()                                     │
 * │     → Mostra campo data final de recorrência                        │
 * │     → returns void (side effect: divFinalRecorrencia display=block) │
 * │     → Fluxo:                                                         │
 * │       1. getElementById("divFinalRecorrencia")                      │
 * │       2. setProperty('display', 'block', 'important')               │
 * │       3. console.log "txtFinalRecorrencia exibido"                  │
 * │     → Uso típico: aoMudarPeriodo → Diário/Semanal/Mensal            │
 * │                                                                       │
 * │ 12. mostrarLstDias()                                                 │
 * │     → Mostra campo multiselect de dias da semana                    │
 * │     → returns void (side effect: divDias display=block + populate)  │
 * │     → Fluxo:                                                         │
 * │       1. getElementById("divDias")                                  │
 * │       2. setProperty('display', 'block', 'important')               │
 * │       3. setTimeout 100ms:                                           │
 * │          - Call window.inicializarLstDias() (recorrencia.js)        │
 * │          - Popula dataSource com dias semana (Dom-Sáb)              │
 * │       4. try-catch: Alerta.TratamentoErroComLinha                   │
 * │     → Uso típico: aoMudarPeriodo → Semanal/Quinzenal                │
 * │                                                                       │
 * │ 13. mostrarLstDiasMes()                                              │
 * │     → Mostra campo multiselect de dias do mês                       │
 * │     → returns void (side effect: divDiaMes display=block + populate)│
 * │     → Fluxo:                                                         │
 * │       1. getElementById("divDiaMes")                                │
 * │       2. setProperty('display', 'block', 'important')               │
 * │       3. setTimeout 100ms:                                           │
 * │          - Call window.inicializarLstDiasMes() (recorrencia.js)     │
 * │          - Popula dataSource com dias 1-31                          │
 * │       4. try-catch: Alerta.TratamentoErroComLinha                   │
 * │     → Uso típico: aoMudarPeriodo → Mensal                           │
 * │                                                                       │
 * │ 14. mostrarCalendarioComBadge()                                      │
 * │     → Mostra calendário multiselect com badge contador              │
 * │     → returns void (side effect: calendarContainer display + init)  │
 * │     → Fluxo: (68 linhas)                                            │
 * │       1. console.log "Iniciando mostrarCalendarioComBadge"          │
 * │       2. Esconder outros campos (divDias, divDiaMes, divFinalRecorrencia)│
 * │       3. getElementById("calendarContainer")                        │
 * │       4. setProperty('display', 'block', 'important')               │
 * │       5. getElementById("calDatasSelecionadas")                     │
 * │       6. setProperty('display', 'block', 'important')               │
 * │       7. Se configurarLocalizacaoSyncfusion exists: call it         │
 * │       8. setTimeout 100ms: inicializarCalendarioSyncfusion()        │
 * │       9. console.log "mostrarCalendarioComBadge concluído"          │
 * │      10. try-catch: Alerta.TratamentoErroComLinha                   │
 * │     → Uso típico: aoMudarPeriodo → Dias Variados                    │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 4: CALENDÁRIO SYNCFUSION (MultiSelect) ──────────────────────┐
 * │ 15. inicializarCalendarioSyncfusion()                                │
 * │     → Cria instância Syncfusion Calendar multiselect com badge      │
 * │     → returns void (side effect: window.calendario instance + badge)│
 * │     → Fluxo: (81 linhas)                                            │
 * │       1. console.log "Inicializando calendário Syncfusion"          │
 * │       2. getElementById("calDatasSelecionadas")                     │
 * │       3. Se calendario exists: destroy() anterior                   │
 * │       4. $('#calDatasSelecionadas').empty() (limpar container)      │
 * │       5. Verificar ej.calendars.Calendar disponível                 │
 * │       6. Criar nova instância:                                      │
 * │          calendario = new ej.calendars.Calendar({                   │
 * │            value: new Date(),                                       │
 * │            isMultiSelection: true,                                  │
 * │            firstDayOfWeek: 0,                                       │
 * │            values: datasSelecionadas,                               │
 * │            locale: 'pt-BR',                                         │
 * │            format: 'dd/MM/yyyy',                                    │
 * │            change: function(args) {                                 │
 * │              datasSelecionadas = args.values || [];                 │
 * │              atualizarBadgeCalendario(datasSelecionadas.length);    │
 * │            }                                                         │
 * │          })                                                          │
 * │       7. calendario.appendTo('#calDatasSelecionadas')               │
 * │       8. calElement.style.display = 'block'                         │
 * │       9. setTimeout 200ms: criarBadgeVisual()                       │
 * │      10. try-catch: Alerta.TratamentoErroComLinha                   │
 * │     → Uso típico: mostrarCalendarioComBadge                         │
 * │                                                                       │
 * │ 16. inicializarCalendario()                                          │
 * │     → Versão alternativa de init Calendar (com min=today)           │
 * │     → returns void (side effect: Calendar instance com validation)  │
 * │     → Fluxo: (72 linhas)                                            │
 * │       1. getElementById("calDatasSelecionadas")                     │
 * │       2. Configurar L10n.load PT-BR                                 │
 * │       3. Criar Calendar:                                            │
 * │          - isMultiSelection: true                                   │
 * │          - values: []                                               │
 * │          - locale: 'pt-BR'                                          │
 * │          - min: new Date() (hoje)                                   │
 * │          - change: atualizarBadgeContador                           │
 * │          - renderDayCell: desabilitar datas passadas                │
 * │       4. calendar.appendTo(calElement)                              │
 * │       5. console.log "Calendário inicializado"                      │
 * │       6. try-catch: Alerta.TratamentoErroComLinha                   │
 * │     → Nota: função não usada atualmente (alternativa a inicializarCalendarioSyncfusion)│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 5: BADGE CONTADOR ───────────────────────────────────────────┐
 * │ 17. atualizarBadgeCalendario(quantidade)                             │
 * │     → Atualiza texto do badge com número de datas selecionadas      │
 * │     → param quantidade: int (número de datas)                       │
 * │     → returns void (side effect: badge textContent + animação)      │
 * │     → Fluxo:                                                         │
 * │       1. $('#badgeContadorDatas').text(quantidade)                  │
 * │       2. addClass('badge-pulse') (animação)                         │
 * │       3. setTimeout 300ms: removeClass('badge-pulse')               │
 * │       4. console.log "Badge atualizado"                             │
 * │     → Chamado por: Calendar.change event                            │
 * │                                                                       │
 * │ 18. criarBadgeVisual()                                               │
 * │     → Cria badge visual laranja no canto superior direito           │
 * │     → returns void (side effect: append badge ao calendarContainer) │
 * │     → Fluxo: (78 linhas)                                            │
 * │       1. console.log "Criando badge"                                │
 * │       2. $('#badgeContadorDatas').remove() (limpar anterior)        │
 * │       3. $('#calendarContainer').css({ position: 'relative',        │
 * │          overflow: 'visible' })                                     │
 * │       4. Criar badge div:                                           │
 * │          - id="badgeContadorDatas", text="0"                        │
 * │          - CSS: position absolute, width 35px, height 35px,         │
 * │            border-radius 50%, background #FF8C00, color white,      │
 * │            border 2px white, z-index 999999, font-size 14px,        │
 * │            box-shadow, transition 0.3s                              │
 * │       5. Hover effect: scale(1.15), box-shadow laranja              │
 * │       6. $('#calendarContainer').append(badge)                      │
 * │       7. setTimeout 100ms: posicionar badge dinâmico                │
 * │          - calPos = calElement.position()                           │
 * │          - badge.css({ top: calPos.top-18, left: calPos.left+calWidth-18 })│
 * │       8. console.log "Badge criado"                                 │
 * │     → Chamado por: inicializarCalendarioSyncfusion após 200ms       │
 * │                                                                       │
 * │ 19. criarBadgeContador()                                             │
 * │     → Versão alternativa de criar badge (sobre calendário)          │
 * │     → returns void (side effect: badge sobre calDatasSelecionadas)  │
 * │     → Fluxo: (59 linhas)                                            │
 * │       1. getElementById("calDatasSelecionadas")                     │
 * │       2. getElementById("badgeContadorDias") (verificar existe)     │
 * │       3. Se !exists: createElement("span")                          │
 * │          - id="badgeContadorDias", class="badge-contador-dias"      │
 * │          - CSS: position absolute, top -25px, right -25px, bg       │
 * │            #ff8c00, color white, border-radius 50%, width 45px,     │
 * │            height 45px, z-index 1000, border 3px white              │
 * │       4. calDatasSelecionadas.style.position = "relative"           │
 * │       5. calDatasSelecionadas.appendChild(badge)                    │
 * │       6. console.log "Badge criado sobre calendário"                │
 * │     → Nota: função não usada atualmente (alternativa a criarBadgeVisual)│
 * │                                                                       │
 * │ 20. posicionarBadge()                                                │
 * │     → Reposiciona badge quando janela redimensiona                  │
 * │     → returns void (side effect: badge CSS top/left update)         │
 * │     → Fluxo:                                                         │
 * │       1. calPos = $('#calDatasSelecionadas').offset()               │
 * │       2. calWidth = $('#calDatasSelecionadas').outerWidth()         │
 * │       3. $('#badgeContadorDatas').css({ position: 'fixed',          │
 * │          top: calPos.top+10, left: calPos.left+calWidth-45 })       │
 * │     → Event: $(window).on('resize', posicionarBadge)                │
 * │                                                                       │
 * │ 21. configurarAtualizacaoBadge()                                     │
 * │     → Configura atualização automática do badge no change           │
 * │     → returns void (side effect: intercepta Calendar.change)        │
 * │     → Fluxo: (42 linhas)                                            │
 * │       1. getElementById("calDatasSelecionadas")                     │
 * │       2. Obter ej2_instances[0]                                     │
 * │       3. Salvar changeOriginal                                      │
 * │       4. calendario.change = function(args) {                       │
 * │            if (changeOriginal) changeOriginal.call(calendario, args);│
 * │            atualizarBadgeContador();                                │
 * │          }                                                           │
 * │       5. console.log "Atualização de badge configurada"             │
 * │     → Nota: não usado atualmente (Calendar.change já definido em init)│
 * │                                                                       │
 * │ 22. atualizarBadgeContador()                                         │
 * │     → Versão alternativa de atualizar badge                         │
 * │     → returns void (side effect: badge textContent update)          │
 * │     → Fluxo:                                                         │
 * │       1. getElementById("badgeContadorDias")                        │
 * │       2. Obter calendario.values || []                              │
 * │       3. badge.textContent = datasSelecionadas.length.toString()    │
 * │       4. console.log "Badge atualizado"                             │
 * │     → Nota: função alternativa a atualizarBadgeCalendario           │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 6: CLDR E LOCALIZAÇÃO ───────────────────────────────────────┐
 * │ 23. carregarCLDRLocal()                                              │
 * │     → Carrega arquivos CLDR locais via ej.base.Ajax                │
 * │     → returns void (side effect: load 5 JSON files + aplicarCLDR)   │
 * │     → Fluxo: (62 linhas)                                            │
 * │       1. console.log "Carregando dados CLDR locais"                 │
 * │       2. Array de 5 URLs:                                           │
 * │          - cldr/numberingSystems.json                               │
 * │          - cldr/ca-gregorian.json                                   │
 * │          - cldr/numbers.json                                        │
 * │          - cldr/timeZoneNames.json                                  │
 * │          - cldr/weekData.json                                       │
 * │       3. forEach URL: new ej.base.Ajax(caminho, 'GET', true)        │
 * │       4. ajax.onSuccess:                                            │
 * │          - JSON.parse(response)                                     │
 * │          - dadosCarregados.push(dados)                              │
 * │          - carregamentosCompletos++                                 │
 * │          - Se todos completos: aplicarCLDR(dadosCarregados)         │
 * │       5. ajax.onFailure: continuar (não bloqueia)                   │
 * │       6. ajax.send()                                                │
 * │     → Nota: não chamado automaticamente (seria para CLDR completo)  │
 * │                                                                       │
 * │ 24. aplicarCLDR(dadosCarregados)                                     │
 * │     → Aplica dados CLDR no Syncfusion e carrega traduções           │
 * │     → param dadosCarregados: Array de objects CLDR                  │
 * │     → returns void (side effect: loadCldr + setCulture)             │
 * │     → Fluxo:                                                         │
 * │       1. console.log "Aplicando dados CLDR"                         │
 * │       2. Se dadosCarregados.length == 0:                            │
 * │          - setCulture('en-US'), inicializarCalendarioSyncfusion     │
 * │       3. Senão:                                                      │
 * │          - ej.base.loadCldr.apply(null, dadosCarregados)            │
 * │          - ej.base.setCulture('pt')                                 │
 * │          - carregarTraducoesPTBR()                                  │
 * │       4. try-catch: fallback en-US                                  │
 * │     → Chamado por: carregarCLDRLocal após carregar todos arquivos   │
 * │                                                                       │
 * │ 25. carregarTraducoesPTBR()                                          │
 * │     → Carrega arquivo pt-BR.json com traduções Syncfusion           │
 * │     → returns void (side effect: L10n.load + init Calendar)         │
 * │     → Fluxo:                                                         │
 * │       1. console.log "Carregando traduções pt-BR.json"              │
 * │       2. new ej.base.Ajax('cldr/pt-BR.json', 'GET', true)           │
 * │       3. ajax.onSuccess:                                            │
 * │          - JSON.parse(response)                                     │
 * │          - ej.base.L10n.load(traducoes)                             │
 * │          - inicializarCalendarioSyncfusion()                        │
 * │       4. ajax.onFailure: inicializarCalendarioSyncfusion (continuar)│
 * │     → Chamado por: aplicarCLDR após loadCldr                        │
 * │                                                                       │
 * │ 26. configurarLocalizacaoSyncfusion()                                │
 * │     → Configura locale PT-BR manualmente (sem CLDR completo)        │
 * │     → returns void (side effect: L10n.load + setCulture)            │
 * │     → Fluxo:                                                         │
 * │       1. ej.base.L10n.load({ 'pt-BR': { 'calendar': { today: 'Hoje' } } })│
 * │       2. ej.base.setCulture('pt-BR')                                │
 * │       3. ej.base.setCurrencyCode('BRL')                             │
 * │     → Chamado por: mostrarCalendarioComBadge antes de init Calendar │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 7: LIMPEZA DE CAMPOS ────────────────────────────────────────┐
 * │ 27. limparCamposRecorrenciaAoMudar()                                 │
 * │     → Limpa valores dos campos ao mudar lstRecorrente/lstPeriodos   │
 * │     → returns void (side effect: value=null em 5 campos)            │
 * │     → Fluxo: (74 linhas)                                            │
 * │       1. Limpar lstPeriodos.value = null, dataBind()                │
 * │       2. Limpar lstDias.value = [], dataBind()                      │
 * │       3. Limpar lstDiasMes.value = null, dataBind()                 │
 * │       4. Limpar txtFinalRecorrencia.value = null, dataBind()        │
 * │       5. Limpar calendario.values = [], dataBind()                  │
 * │       6. Resetar badgeContadorDias.textContent = "0"                │
 * │       7. try-catch: console.error                                   │
 * │     → Chamado por: aoMudarRecorrente, aoMudarPeriodo                │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ AUTO-INITIALIZATION ────────────────────────────────────────────────┐
 * │ DOMContentLoaded ou document.readyState ready:                       │
 * │   → setTimeout 1000ms → window.inicializarLogicaRecorrencia()       │
 * │ → Garante que Syncfusion components foram renderizados antes de     │
 * │   configurar event handlers e defaults                              │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO 1 - CRIAR AGENDAMENTO COM RECORRÊNCIA SEMANAL:
 * 1. Usuário abre modal → inicializarLogicaRecorrencia já executou (DOMContentLoaded)
 * 2. lstRecorrente exibido com valor default "Não"
 * 3. Todos os outros campos escondidos (esconderTodosCamposRecorrencia)
 * 4. Usuário muda lstRecorrente → "Sim"
 * 5. aoMudarRecorrente trigger → divPeriodo.display = 'block'
 * 6. lstPeriodos aparece (vazio)
 * 7. Usuário seleciona lstPeriodos → "Semanal"
 * 8. aoMudarPeriodo trigger → esconderCamposEspecificosPeriodo
 * 9. aoMudarPeriodo → mostrarLstDias() + mostrarTxtFinalRecorrencia()
 * 10. divDias.display = 'block', setTimeout 100ms → inicializarLstDias()
 * 11. lstDias populado com Dom-Sáb (7 options)
 * 12. divFinalRecorrencia.display = 'block'
 * 13. Usuário seleciona dias (Seg, Qua, Sex) + data final
 * 14. Salva agendamento → lstDias.value = [1, 3, 5]
 *
 * 🔄 FLUXO TÍPICO 2 - RECORRÊNCIA DIAS VARIADOS (Calendar MultiSelect):
 * 1. Usuário abre modal, lstRecorrente → "Sim"
 * 2. aoMudarRecorrente → divPeriodo aparece
 * 3. Usuário seleciona lstPeriodos → "Dias Variados"
 * 4. aoMudarPeriodo → mostrarCalendarioComBadge()
 * 5. esconderCamposEspecificosPeriodo (limpa outros campos)
 * 6. calendarContainer.display = 'block'
 * 7. calDatasSelecionadas.display = 'block'
 * 8. configurarLocalizacaoSyncfusion() → L10n PT-BR
 * 9. setTimeout 100ms → inicializarCalendarioSyncfusion()
 * 10. Destroy calendário anterior (se exists)
 * 11. Criar novo Calendar({ isMultiSelection: true, locale: 'pt-BR', change: ... })
 * 12. calendario.appendTo('#calDatasSelecionadas')
 * 13. setTimeout 200ms → criarBadgeVisual()
 * 14. Badge laranja criado (35×35px, z-index 999999, position absolute)
 * 15. Badge posicionado top-right do calendário (calPos.left + calWidth - 18)
 * 16. Badge textContent = "0" (nenhuma data selecionada)
 * 17. Usuário clica em datas no calendário
 * 18. Calendar.change event → datasSelecionadas = args.values
 * 19. atualizarBadgeCalendario(datasSelecionadas.length)
 * 20. Badge textContent atualizado (ex: "5")
 * 21. Badge animação pulse (addClass/removeClass badge-pulse)
 * 22. Usuário salva → calendario.values = [Date1, Date2, Date3, Date4, Date5]
 *
 * 🔄 FLUXO TÍPICO 3 - EDITAR AGENDAMENTO COM RECORRÊNCIA (Evitar Loops):
 * 1. modal-viagem-novo.inicializarCamposModal(dados) inicia
 * 2. Set window.ignorarEventosRecorrencia = true (ANTES de preencher)
 * 3. lstRecorrente.value = "S" (Sim) → aoMudarRecorrente NÃO executa (flag true)
 * 4. lstPeriodos.value = "M" (Mensal) → aoMudarPeriodo NÃO executa (flag true)
 * 5. Preencher outros campos (lstDiasMes, txtFinalRecorrencia)
 * 6. Show campos manualmente: divPeriodo, divDiaMes, divFinalRecorrencia
 * 7. Set window.ignorarEventosRecorrencia = false (DEPOIS de preencher)
 * 8. Agora mudanças do usuário disparam handlers normalmente
 *
 * 📌 ESTRUTURA CAMPOS RECORRÊNCIA (5 divs):
 * - divPeriodo: container para lstPeriodos (DropDownList: Diário, Semanal, Quinzenal, Mensal, Variado)
 * - divDias: container para lstDias (MultiSelect: Dom, Seg, Ter, Qua, Qui, Sex, Sáb)
 * - divDiaMes: container para lstDiasMes (MultiSelect: 1-31)
 * - divFinalRecorrencia: container para txtFinalRecorrencia (DateTimePicker: data limite)
 * - calendarContainer: container para calDatasSelecionadas (Calendar multiSelect)
 *
 * 📌 PERÍODOS SUPORTADOS (5 tipos):
 * - "D" (Diário): repete todos os dias → show apenas txtFinalRecorrencia
 * - "S" (Semanal): repete dias específicos da semana → show lstDias + txtFinalRecorrencia
 * - "Q" (Quinzenal): repete a cada 2 semanas → show lstDias + txtFinalRecorrencia
 * - "M" (Mensal): repete dias específicos do mês → show lstDiasMes + txtFinalRecorrencia
 * - "V" (Dias Variados): datas customizadas → show calDatasSelecionadas (Calendar multiSelect)
 *
 * 📌 BADGE VISUAL (2 implementações):
 * - criarBadgeVisual: badge sobre calendarContainer (método principal usado)
 *   - Posição: calculada dinamicamente (calPos.left + calWidth - 18)
 *   - Z-index: 999999 (sempre visível)
 *   - Animação: pulse ao atualizar (addClass → setTimeout → removeClass)
 * - criarBadgeContador: badge sobre calDatasSelecionadas (método alternativo)
 *   - Posição: absolute top -25px, right -25px (50% fora do calendário)
 *   - Z-index: 1000
 *
 * 📌 RETRY PATTERN (configurarEventHandlerPeriodo):
 * - setInterval 200ms, max 10 tentativas (total 2 segundos)
 * - Necessário porque lstPeriodos criado dinamicamente após inicializarDropdownPeriodos
 * - Se não encontrado após 10 tentativas: console.error + desiste
 *
 * 📌 CLDR E LOCALIZAÇÃO (3 métodos):
 * - carregarCLDRLocal: carrega 5 JSON files completos (numberingSystems, ca-gregorian, etc.)
 *   - Não usado atualmente (muito pesado)
 * - configurarLocalizacaoSyncfusion: método simples (L10n.load manual + setCulture)
 *   - Usado atualmente (suficiente para Calendar básico)
 * - carregarTraducoesPTBR: carrega pt-BR.json via Ajax
 *   - Complementar ao CLDR completo (também não usado)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Linhas 96-136: código duplicado simplificado de inicializarLogicaRecorrencia (não executado)
 * - Duas implementações de várias funções (Calendar, Badge, etc.) para flexibilidade
 * - Console.log extensivo facilita debug mas pode ser removido em produção
 * - style.setProperty com '!important' necessário para sobrescrever CSS Razor/Bootstrap
 * - setTimeout delays (100-300ms) necessários para aguardar render Syncfusion assíncrono
 * - jQuery usado para manipulação DOM (.$(), .css(), .append, .on) + Syncfusion native
 * - window.calendar global permite acesso externo (ex: modal-viagem-novo.criarAgendamentoNovo)
 * - ignorarEventosRecorrencia flag crítica para evitar loops infinitos em edição
 * - Badge contador: UX visual importante para feedback ao usuário (quantas datas selecionadas)
 * - Calendar multiSelect: isMultiSelection=true permite selecionar múltiplas datas (array)
 * - min=today em inicializarCalendario: valida datas futuras apenas (renderDayCell hook)
 *
 * 🔌 VERSÃO: 3.0 (refatorado após Lote 192, adiciona comprehensive header)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */

window.calendario = null;
window.datasSelecionadas = [];
window.ignorarEventosRecorrencia = false;

/**
 * Inicializa a lógica de visibilidade dos campos de recorrência
 * Deve ser chamado após os controles Syncfusion estarem renderizados
 */
window.inicializarLogicaRecorrencia = function ()
{
    try
    {
        console.log("ðŸ”§ Inicializando lógica de recorrência...");

        // PRIMEIRO: Inicializar o dropdown de perí­odos (se ainda não foi)
        if (window.inicializarDropdownPeriodos)
        {
            console.log("ðŸ“‹ Inicializando dropdown de perí­odos...");
            window.inicializarDropdownPeriodos();
        }
        else
        {
            console.warn("âš ï¸ Função inicializarDropdownPeriodos não encontrada");
        }

        // Aguardar um pouco para garantir que o dropdown foi criado
        setTimeout(() =>
        {
            // Esconder todos os campos exceto lstRecorrente no início
            esconderTodosCamposRecorrencia();

            // SEGUNDO: Definir valor padrío "Não" para lstRecorrente
            setTimeout(() =>
            {
                const lstRecorrenteElement = document.getElementById("lstRecorrente");
                if (lstRecorrenteElement && lstRecorrenteElement.ej2_instances)
                {
                    const lstRecorrente = lstRecorrenteElement.ej2_instances[0];
                    if (lstRecorrente)
                    {
                        // Verificar qual valor usar para "Não"
                        console.log("ðŸ” DataSource de lstRecorrente:", lstRecorrente.dataSource);

                        // Tentar encontrar o item "Não"
                        const itemNao = lstRecorrente.dataSource?.find(item =>
                            item.Descricao === "Não" ||
                            item.Descricao === "Nao" ||
                            item.RecorrenteId === "N"
                        );

                        if (itemNao)
                        {
                            console.log("ðŸ“‹ Item 'Não' encontrado:", itemNao);
                            lstRecorrente.value = itemNao.RecorrenteId;
                            lstRecorrente.dataBind();
                            // lstRecorrente.refresh(); // Comentado - causa evento change indesejado
                            console.log("âœ… lstRecorrente definido como 'Não' (padrío)");
                        }
                        else
                        {
                            console.warn("âš ï¸ Item 'Não' não encontrado no dataSource");
                        }
                    }
                    else
                    {
                        console.warn("âš ï¸ Instância lstRecorrente não encontrada");
                    }
                }
                else
                {
                    console.warn("âš ï¸ lstRecorrente não encontrado no DOM");
                }
            }, 200);

            // TERCEIRO: Configurar event handlers
            configurarEventHandlerRecorrente();
            configurarEventHandlerPeriodo();

            console.log("âœ… Lógica de recorrência inicializada");

        }, 300);

    } catch (error)
    {
        console.error("âŒ Erro ao inicializar lógica de recorrência:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "inicializarLogicaRecorrencia", error);
        }
    }
};
{
    try
    {
        console.log("ðŸ”§ Inicializando lógica de recorrência...");

        // Esconder todos os campos exceto lstRecorrente no início
        esconderTodosCamposRecorrencia();

        // Definir valor padrío "Não" para lstRecorrente
        setTimeout(() =>
        {
            const lstRecorrenteElement = document.getElementById("lstRecorrente");
            if (lstRecorrenteElement && lstRecorrenteElement.ej2_instances)
            {
                const lstRecorrente = lstRecorrenteElement.ej2_instances[0];
                if (lstRecorrente)
                {
                    lstRecorrente.value = "N";
                    lstRecorrente.dataBind();
                    console.log("âœ… lstRecorrente definido como 'Não'");
                }
            }
        }, 100);

        // Configurar event handler para lstRecorrente
        configurarEventHandlerRecorrente();

        // Configurar event handler para lstPeriodos
        configurarEventHandlerPeriodo();

        console.log("âœ… Lógica de recorrência inicializada");

    } catch (error)
    {
        console.error("âŒ Erro ao inicializar lógica de recorrência:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "inicializarLogicaRecorrencia", error);
        }
    }
};

/**
 * Esconde todos os campos de recorrência exceto lstRecorrente
 */
function esconderTodosCamposRecorrencia()
{
    try
    {
        const camposParaEsconder = [
            "divPeriodo",
            "divDias",
            "divDiaMes",
            "divFinalRecorrencia",
            "calendarContainer"
        ];

        camposParaEsconder.forEach(id =>
        {
            const elemento = document.getElementById(id);
            if (elemento)
            {
                // Usar setProperty com important para sobrescrever CSS
                elemento.style.setProperty('display', 'none', 'important');
            }
        });

        console.log("âœ… Todos os campos de recorrência escondidos (exceto lstRecorrente)");

    } catch (error)
    {
        console.error("âŒ Erro ao esconder campos:", error);
    }
}

/**
 * Configura o event handler para o dropdown lstRecorrente
 */
function configurarEventHandlerRecorrente()
{
    try
    {
        const lstRecorrenteElement = document.getElementById("lstRecorrente");

        if (!lstRecorrenteElement || !lstRecorrenteElement.ej2_instances)
        {
            console.warn("âš ï¸ lstRecorrente não encontrado");
            return;
        }

        const lstRecorrente = lstRecorrenteElement.ej2_instances[0];

        if (!lstRecorrente)
        {
            console.warn("âš ï¸ Instância lstRecorrente não encontrada");
            return;
        }

        // Configurar evento de mudança
        lstRecorrente.change = function (args)
        {
            aoMudarRecorrente(args);
        };

        console.log("âœ… Event handler lstRecorrente configurado");

    } catch (error)
    {
        console.error("âŒ Erro ao configurar event handler recorrente:", error);
    }
}

/**
 * Handler executado quando lstRecorrente muda
 */
function aoMudarRecorrente(args)
{
    try
    {
        console.log("ðŸ”„ lstRecorrente mudou - DEBUG COMPLETO:");
        console.log("   - args completo:", args);
        console.log("   - args.value:", args.value);
        console.log("   - args.itemData:", args.itemData);
        console.log("   - args.itemData?.RecorrenteId:", args.itemData?.RecorrenteId);
        console.log("   - args.itemData?.Descricao:", args.itemData?.Descricao);

        // ADICIONAR VERIFICAÇÃO DA FLAG
        if (window.ignorarEventosRecorrencia)
        {
            console.log("ðŸ“Œ Ignorando evento de recorrente (carregando dados)");
            return;
        }

        // Tentar múltiplas formas de pegar o valor
        const valor = args.value || args.itemData?.RecorrenteId || args.itemData?.Value;
        const descricao = args.itemData?.Descricao || args.itemData?.Text || "";

        console.log("   - Valor extraÃ­do:", valor);
        console.log("   - Descrição extraÃ­da:", descricao);

        const divPeriodo = document.getElementById("divPeriodo");
        console.log("   - divPeriodo existe?", divPeriodo ? "SIM" : "NÃO");

        // Limpar campos antes de mostrar/esconder
        limparCamposRecorrenciaAoMudar();

        // Verificar se é "Sim" de várias formas possíveis
        const ehSim = valor === "S" ||
            valor === "Sim" ||
            descricao === "Sim" ||
            descricao.toLowerCase() === "sim";

        console.log("   - Ã‰ SIM?", ehSim);

        if (ehSim) // Sim
        {
            console.log("   âœ… Selecionou SIM - Mostrar lstPeriodo");

            if (divPeriodo)
            {
                console.log("   â†’ Aplicando display:block no divPeriodo...");
                // Usar setProperty com important para sobrescrever CSS
                divPeriodo.style.setProperty('display', 'block', 'important');
                console.log("   â†’ Display aplicado. Valor atual:", window.getComputedStyle(divPeriodo).display);

                // Limpar valor do lstPeriodos
                const lstPeriodosElement = document.getElementById("lstPeriodos");
                if (lstPeriodosElement && lstPeriodosElement.ej2_instances)
                {
                    const lstPeriodos = lstPeriodosElement.ej2_instances[0];
                    if (lstPeriodos)
                    {
                        lstPeriodos.value = null;
                        lstPeriodos.dataBind();
                    }
                }
            }
            else
            {
                console.error("   âŒ divPeriodo NÃO FOI ENCONTRADO!");
            }
        }
        else // Não
        {
            console.log("   âŒ Selecionou NÃO - Esconder todos os campos");
            esconderTodosCamposRecorrencia();
        }

    } catch (error)
    {
        console.error("âŒ Erro em aoMudarRecorrente:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "aoMudarRecorrente", error);
        }
    }
}

/**
 * Configura o event handler para o dropdown lstPeriodos
 */
function configurarEventHandlerPeriodo()
{
    try
    {
        console.log("ðŸ”§ Tentando configurar event handler de lstPeriodos...");

        // Tentar várias vezes até encontrar o controle
        let tentativas = 0;
        const maxTentativas = 10;

        const intervalo = setInterval(() =>
        {
            tentativas++;
            console.log(`   â†’ Tentativa ${tentativas}/${maxTentativas}...`);

            const lstPeriodosElement = document.getElementById("lstPeriodos");

            if (!lstPeriodosElement)
            {
                console.warn(`   âš ï¸ lstPeriodos não encontrado (tentativa ${tentativas})`);
                if (tentativas >= maxTentativas)
                {
                    clearInterval(intervalo);
                    console.error("   âŒ lstPeriodos não encontrado após todas tentativas");
                }
                return;
            }

            if (!lstPeriodosElement.ej2_instances || !lstPeriodosElement.ej2_instances[0])
            {
                console.warn(`   âš ï¸ lstPeriodos não inicializado ainda (tentativa ${tentativas})`);
                if (tentativas >= maxTentativas)
                {
                    clearInterval(intervalo);
                    console.error("   âŒ lstPeriodos não inicializado após todas tentativas");
                }
                return;
            }

            // Encontrou! Configurar o evento
            clearInterval(intervalo);

            const lstPeriodos = lstPeriodosElement.ej2_instances[0];

            console.log("   âœ… lstPeriodos encontrado! Configurando evento...");
            console.log("   ðŸ“‹ DataSource atual:", lstPeriodos.dataSource);

            // Remover evento anterior se existir
            lstPeriodos.change = null;

            // Configurar novo evento de mudança
            lstPeriodos.change = function (args)
            {
                console.log("ðŸŽ¯ EVENT HANDLER CHAMADO! lstPeriodos mudou!");
                aoMudarPeriodo(args);
            };

            console.log("   âœ… Event handler lstPeriodos configurado com sucesso!");

        }, 200); // Tentar a cada 200ms

    } catch (error)
    {
        console.error("âŒ Erro ao configurar event handler perí­odo:", error);
    }
}

/**
 * Handler executado quando lstPeriodos muda
 */
function aoMudarPeriodo(args)
{
    try
    {
        console.log("ðŸ”„ lstPeriodos mudou - DEBUG COMPLETO:");
        console.log("   - args completo:", args);
        console.log("   - args.value:", args.value);
        console.log("   - args.itemData:", args.itemData);

        // ADICIONAR VERIFICAÇÃO DA FLAG
        if (window.ignorarEventosRecorrencia)
        {
            console.log("ðŸ“Œ Ignorando evento de perí­odo (carregando dados)");
            return;
        }

        // Tentar múltiplas formas de pegar o valor
        const valor = args.value || args.itemData?.Value || args.itemData?.PeriodoId;
        const texto = args.itemData?.Text || args.itemData?.Periodo || "";

        console.log("   ðŸ“‹ Valor extraÃ­do:", valor);
        console.log("   ðŸ“‹ Texto extraÃ­do:", texto);

        // Esconder todos os campos especí­ficos primeiro
        console.log("   ðŸ§¹ Escondendo campos especí­ficos...");
        esconderCamposEspecificosPeriodo();

        // Mostrar campos baseado no perí­odo selecionado
        console.log("   ðŸ” Verificando qual perí­odo foi selecionado...");

        switch (valor)
        {
            case "D": // Diário
                console.log("   âž¡ï¸ Perí­odo: DIÃRIO - Mostrar apenas txtFinalRecorrencia");
                mostrarTxtFinalRecorrencia();
                break;

            case "S": // Semanal
            case "Q": // Quinzenal
                console.log("   âž¡ï¸ Perí­odo: SEMANAL/QUINZENAL - Mostrar lstDias + txtFinalRecorrencia");
                mostrarLstDias();
                mostrarTxtFinalRecorrencia();
                break;

            case "M": // Mensal
                console.log("   âž¡ï¸ Perí­odo: MENSAL - Mostrar lstDiasMes + txtFinalRecorrencia");
                mostrarLstDiasMes();
                mostrarTxtFinalRecorrencia();
                break;

            case "V": // Dias Variados
                console.log("   âž¡ï¸ Perí­odo: DIAS VARIADOS - Mostrar calendário com badge");
                mostrarCalendarioComBadge();
                break;

            default:
                console.log("   âš ï¸ Perí­odo não reconhecido:", valor, texto);
                console.log("   ðŸ’¡ Tentando pelo texto...");

                // Tentar pelo texto se o valor não for reconhecido
                const textoLower = texto.toLowerCase();

                if (textoLower.includes("diário") || textoLower.includes("diario"))
                {
                    console.log("   âž¡ï¸ Detectado pelo texto: DIÃRIO");
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("semanal"))
                {
                    console.log("   âž¡ï¸ Detectado pelo texto: SEMANAL");
                    mostrarLstDias();
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("quinzenal"))
                {
                    console.log("   âž¡ï¸ Detectado pelo texto: QUINZENAL");
                    mostrarLstDias();
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("mensal"))
                {
                    console.log("   âž¡ï¸ Detectado pelo texto: MENSAL");
                    mostrarLstDiasMes();
                    mostrarTxtFinalRecorrencia();
                }
                else if (textoLower.includes("variado") || textoLower.includes("variada"))
                {
                    console.log("   âž¡ï¸ Detectado pelo texto: DIAS VARIADOS");
                    mostrarCalendarioComBadge();
                }
                else
                {
                    console.error("   âŒ Perí­odo não pôde ser identificado!");
                }
                break;
        }

        console.log("   âœ… aoMudarPeriodo concluÃ­do");

    } catch (error)
    {
        console.error("âŒ Erro em aoMudarPeriodo:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "aoMudarPeriodo", error);
        }
    }
}

/**
 * Esconde campos especí­ficos de perí­odo
 */
function esconderCamposEspecificosPeriodo()
{
    // Remover classes de modo de recorrência variada
    document.body.classList.remove('modo-criacao-variada');
    document.body.classList.remove('modo-edicao-variada');

    const campos = [
        "divDias",
        "divDiaMes",
        "divFinalRecorrencia",
        "calendarContainer"
    ];

    campos.forEach(id =>
    {
        const elemento = document.getElementById(id);
        if (elemento)
        {
            // Usar setProperty com important para sobrescrever CSS
            elemento.style.setProperty('display', 'none', 'important');
        }
    });
}

/**
 * Mostra o campo txtFinalRecorrencia
 */
function mostrarTxtFinalRecorrencia()
{
    const divFinalRecorrencia = document.getElementById("divFinalRecorrencia");
    if (divFinalRecorrencia)
    {
        // Usar setProperty com important para sobrescrever CSS
        divFinalRecorrencia.style.setProperty('display', 'block', 'important');
        console.log("   âœ… txtFinalRecorrencia exibido");
    }
}

/**
 * Mostra o campo lstDias (multiselect de dias da semana)
 * ✅ CORRIGIDO: Agora chama inicialização para popular o dataSource
 */
function mostrarLstDias()
{
    try
    {
        const divDias = document.getElementById("divDias");
        if (divDias)
        {
            // Usar setProperty com important para sobrescrever CSS
            divDias.style.setProperty('display', 'block', 'important');
            console.log("   ✅ lstDias container exibido");

            // ✅ CRÍTICO: Chamar inicialização para popular os dias da semana
            setTimeout(() =>
            {
                if (typeof window.inicializarLstDias === 'function')
                {
                    const sucesso = window.inicializarLstDias();
                    if (sucesso)
                    {
                        console.log("   ✅ lstDias populado com dias da semana");
                    }
                    else
                    {
                        console.warn("   ⚠️ lstDias não pôde ser populado (controle não renderizado)");
                    }
                }
                else
                {
                    console.error("   ❌ Função window.inicializarLstDias não encontrada!");
                }
            }, 100); // Pequeno delay para garantir renderização
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-logic.js", "mostrarLstDias", error);
    }
}

/**
 * Mostra o campo lstDiasMes
 * ✅ CORRIGIDO: Agora chama inicialização para popular o dataSource
 */
function mostrarLstDiasMes()
{
    try
    {
        const divDiaMes = document.getElementById("divDiaMes");
        if (divDiaMes)
        {
            // Usar setProperty com important para sobrescrever CSS
            divDiaMes.style.setProperty('display', 'block', 'important');
            console.log("   ✅ lstDiasMes container exibido");

            // ✅ CRÍTICO: Chamar inicialização para popular os dias do mês
            setTimeout(() =>
            {
                if (typeof window.inicializarLstDiasMes === 'function')
                {
                    const sucesso = window.inicializarLstDiasMes();
                    if (sucesso)
                    {
                        console.log("   ✅ lstDiasMes populado com dias do mês");
                    }
                    else
                    {
                        console.warn("   ⚠️ lstDiasMes não pôde ser populado (controle não renderizado)");
                    }
                }
                else
                {
                    console.error("   ❌ Função window.inicializarLstDiasMes não encontrada!");
                }
            }, 100); // Pequeno delay para garantir renderização
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-logic.js", "mostrarLstDiasMes", error);
    }
}

var datasSelecionadas = []; // Array para armazenar datas selecionadas

function inicializarCalendarioSyncfusion()
{
    try
    {
        console.log("🔧 Inicializando calendário Syncfusion...");

        // Verificar se o elemento existe
        const calElement = document.getElementById('calDatasSelecionadas');
        if (!calElement)
        {
            console.error("❌ Elemento calDatasSelecionadas não encontrado!");
            return;
        }

        console.log("✅ Elemento calDatasSelecionadas encontrado");

        // Destruir calendário anterior se existir
        if (calendario)
        {
            console.log("♻️ Destruindo calendário anterior");
            try
            {
                calendario.destroy();
            } catch (e)
            {
                console.warn("⚠️ Erro ao destruir calendário anterior:", e);
            }
        }

        // Limpar o container
        $('#calDatasSelecionadas').empty();
        console.log("🧹 Container limpo");

        // Verificar se Syncfusion está disponível
        if (typeof ej === 'undefined' || !ej.calendars || !ej.calendars.Calendar)
        {
            console.error("❌ Syncfusion Calendar não está disponível!");
            return;
        }

        console.log("✅ Syncfusion Calendar disponível");

        // Criar novo calendário com seleção múltipla
        calendario = new ej.calendars.Calendar({
            value: new Date(),
            isMultiSelection: true,
            firstDayOfWeek: 0,
            values: datasSelecionadas,
            locale: 'pt-BR',
            format: 'dd/MM/yyyy',
            change: function (args)
            {
                datasSelecionadas = args.values || [];
                console.log("📅 Datas selecionadas:", datasSelecionadas);
                console.log("📊 Total de datas:", datasSelecionadas.length);

                // Atualizar badge com contador
                atualizarBadgeCalendario(datasSelecionadas.length);
            }
        });

        console.log("📅 Instância do calendário criada");

        // Anexar ao elemento
        calendario.appendTo('#calDatasSelecionadas');
        console.log("✅ Calendário Syncfusion anexado ao DOM");

        // Forçar exibição do elemento
        calElement.style.display = 'block';
        calElement.style.visibility = 'visible';

        console.log("✅ Calendário Syncfusion inicializado com sucesso!");

        // CRIAR BADGE APÓS o calendário ser renderizado
        setTimeout(function ()
        {
            criarBadgeVisual();
        }, 200);

    } catch (error)
    {
        console.error("❌ Erro ao inicializar calendário:", error);
        Alerta.TratamentoErroComLinha("recorrencia-logic.js", "inicializarCalendarioSyncfusion", error);
    }
}


function atualizarBadgeCalendario(quantidade)
{
    // Atualizar o texto do badge
    $('#badgeContadorDatas').text(quantidade);

    // Adicionar animação de pulse quando houver mudança
    $('#badgeContadorDatas').addClass('badge-pulse');
    setTimeout(function ()
    {
        $('#badgeContadorDatas').removeClass('badge-pulse');
    }, 300);

    console.log("ðŸ·ï¸ Badge atualizado:", quantidade);
}

/**
 * Mostra o calendário com badge para contagem de dias
 */
function mostrarCalendarioComBadge()
{
    try
    {
        console.log("📅 Iniciando mostrarCalendarioComBadge()");

        // Esconder outros containers primeiro
        const camposParaEsconder = ["divDias", "divDiaMes", "divFinalRecorrencia"];
        camposParaEsconder.forEach(id =>
        {
            const elemento = document.getElementById(id);
            if (elemento)
            {
                elemento.style.setProperty('display', 'none', 'important');
            }
        });

        console.log("✅ Outros campos escondidos");

        // Verificar se o container do calendário existe
        const calendarContainer = document.getElementById("calendarContainer");
        if (!calendarContainer)
        {
            console.error("❌ Elemento calendarContainer não encontrado!");
            return;
        }

        console.log("✅ Container do calendário encontrado");

        // Mostrar container do calendário com !important
        calendarContainer.style.setProperty('display', 'block', 'important');
        calendarContainer.style.setProperty('visibility', 'visible', 'important');
        console.log("✅ Container do calendário exibido");

        // Verificar se o elemento interno existe
        const calDatasSelecionadas = document.getElementById("calDatasSelecionadas");
        if (!calDatasSelecionadas)
        {
            console.error("❌ Elemento calDatasSelecionadas não encontrado!");
            return;
        }

        console.log("✅ Elemento calDatasSelecionadas encontrado");

        // Garantir que o elemento interno também está visível
        calDatasSelecionadas.style.setProperty('display', 'block', 'important');
        calDatasSelecionadas.style.setProperty('visibility', 'visible', 'important');

        // Configurar localização ANTES de inicializar
        if (typeof configurarLocalizacaoSyncfusion === 'function')
        {
            configurarLocalizacaoSyncfusion();
            console.log("✅ Localização configurada");
        }

        // Aguardar um pouco para garantir que o DOM está pronto
        setTimeout(() =>
        {
            // Inicializar o calendário Syncfusion
            inicializarCalendarioSyncfusion();
            console.log("✅ Calendário inicializado");
        }, 100);

        console.log("✅ mostrarCalendarioComBadge concluído");

    } catch (error)
    {
        console.error("❌ Erro em mostrarCalendarioComBadge:", error);
        Alerta.TratamentoErroComLinha("recorrencia-logic.js", "mostrarCalendarioComBadge", error);
    }
}


/**
 * Cria o badge visual no canto superior direito do calendário
 */
function criarBadgeVisual()
{
    console.log("ðŸ·ï¸ Criando badge...");

    // Remover badge antigo
    $('#badgeContadorDatas').remove();

    // Garantir que o container tenha position relative
    $('#calendarContainer').css({
        'position': 'relative',
        'overflow': 'visible' // â† IMPORTANTE: permitir que o badge saia do container
    });

    // Criar badge
    var badge = $('<div id="badgeContadorDatas">0</div>').css({
        'position': 'absolute',
        'width': '35px',
        'height': '35px',
        'border-radius': '50%',
        'background-color': '#FF8C00',
        'color': 'white',
        'border': '2px solid white',
        'display': 'flex',
        'align-items': 'center',
        'justify-content': 'center',
        'font-size': '14px',
        'font-weight': 'bold',
        'font-family': 'Arial, sans-serif',
        'box-shadow': '0 2px 8px rgba(0, 0, 0, 0.3)',
        'z-index': '999999', // â† Z-index altí­ssimo
        'transition': 'all 0.3s ease',
        'cursor': 'default'
    });

    // Efeito hover
    badge.hover(
        function ()
        {
            $(this).css({
                'transform': 'scale(1.15)',
                'box-shadow': '0 4px 12px rgba(255, 140, 0, 0.5)'
            });
        },
        function ()
        {
            $(this).css({
                'transform': 'scale(1)',
                'box-shadow': '0 2px 8px rgba(0, 0, 0, 0.3)'
            });
        }
    );

    // Adicionar badge ao container pai
    $('#calendarContainer').append(badge);

    // Aguardar o calendário renderizar completamente
    setTimeout(function ()
    {
        // Pegar a posição do calendário dentro do container
        var calElement = $('#calDatasSelecionadas');
        if (calElement.length > 0)
        {
            var calPos = calElement.position();
            var calWidth = calElement.outerWidth();

            // Posicionar badge na quina superior direita do calendário
            badge.css({
                'top': (calPos.top - 18) + 'px',
                'left': (calPos.left + calWidth - 18) + 'px'
            });

            console.log("âœ… Badge posicionado em:", {
                top: (calPos.top - 18) + 'px',
                left: (calPos.left + calWidth - 18) + 'px'
            });
        }
    }, 100);

    console.log("âœ… Badge criado!");
}

function posicionarBadge()
{
    var calPos = $('#calDatasSelecionadas').offset();
    var calWidth = $('#calDatasSelecionadas').outerWidth();

    $('#badgeContadorDatas').css({
        'position': 'fixed',
        'top': calPos.top + 10 + 'px',
        'left': (calPos.left + calWidth - 45) + 'px'
    });
}

// Reposicionar ao redimensionar janela
$(window).on('resize', posicionarBadge);

/**
 * Carrega dados CLDR dos arquivos locais
 */
function carregarCLDRLocal()
{
    console.log("ðŸŒ Carregando dados CLDR locais...");

    // Caminhos dos arquivos CLDR locais
    var cldrUrls = [
        'cldr/numberingSystems.json',
        'cldr/ca-gregorian.json',
        'cldr/numbers.json',
        'cldr/timeZoneNames.json',
        'cldr/weekData.json'
    ];

    var dadosCarregados = [];
    var carregamentosCompletos = 0;
    var totalArquivos = cldrUrls.length;

    // Função para carregar cada arquivo
    cldrUrls.forEach(function (caminho)
    {
        var ajax = new ej.base.Ajax(caminho, 'GET', true);

        ajax.onSuccess = function (response)
        {
            console.log("âœ… Arquivo carregado:", caminho);

            try
            {
                // Tentar fazer parse do JSON
                var dados = JSON.parse(response);
                dadosCarregados.push(dados);
                console.log("âœ… Parse bem-sucedido:", caminho);
            } catch (erro)
            {
                console.error("âŒ Erro ao fazer parse do JSON:", caminho);
                console.error("Erro detalhado:", erro.message);
                console.log("Conteíºdo recebido:", response.substring(0, 200)); // Primeiros 200 caracteres
            }

            carregamentosCompletos++;

            // Quando todos os arquivos forem carregados
            if (carregamentosCompletos === totalArquivos)
            {
                console.log("âœ… Total de arquivos processados:", dadosCarregados.length);
                aplicarCLDR(dadosCarregados);
            }
        };

        ajax.onFailure = function (error)
        {
            console.error("âŒ Erro ao carregar arquivo:", caminho, error);
            carregamentosCompletos++;

            // Continuar mesmo com erro
            if (carregamentosCompletos === totalArquivos)
            {
                aplicarCLDR(dadosCarregados);
            }
        };

        ajax.send();
    });
}
/**
 * Aplica os dados CLDR e carrega traduções
 */
function aplicarCLDR(dadosCarregados)
{
    console.log("ðŸ”§ Aplicando dados CLDR...");
    console.log("ðŸ“Š Arquivos carregados com sucesso:", dadosCarregados.length);

    // Verificar se temos dados para carregar
    if (dadosCarregados.length === 0)
    {
        console.error("âŒ Nenhum arquivo CLDR foi carregado corretamente!");
        console.log("âš ï¸ Usando configuração padrío en-US");
        ej.base.setCulture('en-US');
        inicializarCalendarioSyncfusion();
        return;
    }

    try
    {
        // Carregar dados no Syncfusion
        ej.base.loadCldr.apply(null, dadosCarregados);
        console.log("âœ… Dados CLDR aplicados com sucesso");

        // Definir cultura portuguesa
        ej.base.setCulture('pt');
        console.log("âœ… Cultura definida para 'pt'");

        // Carregar arquivo de tradução pt-BR.json
        carregarTraducoesPTBR();

    } catch (erro)
    {
        console.error("âŒ Erro ao aplicar CLDR:", erro);
        console.log("âš ï¸ Usando configuração padrío en-US");
        ej.base.setCulture('en-US');
        inicializarCalendarioSyncfusion();
    }
}

/**
 * Carrega arquivo de tradução pt-BR.json local
 */
function carregarTraducoesPTBR()
{
    console.log("ðŸ”¤ Carregando traduções pt-BR.json...");

    var ajax = new ej.base.Ajax('cldr/pt-BR.json', 'GET', true);

    ajax.onSuccess = function (response)
    {
        try
        {
            console.log("âœ… Traduções pt-BR carregadas");

            // Fazer parse do JSON
            var traducoes = JSON.parse(response);

            // Carregar traduções
            ej.base.L10n.load(traducoes);
            console.log("âœ… Traduções aplicadas com sucesso");

        } catch (erro)
        {
            console.error("âŒ Erro ao fazer parse do pt-BR.json:", erro.message);
            console.log("Conteíºdo recebido:", response.substring(0, 200));
        }

        // Inicializar calendário (com ou sem traduções)
        inicializarCalendarioSyncfusion();
    };

    ajax.onFailure = function (error)
    {
        console.warn("âš ï¸ Erro ao carregar pt-BR.json:", error);
        console.log("âš ï¸ Continuando sem traduções da interface...");

        // Continuar mesmo sem traduções
        inicializarCalendarioSyncfusion();
    };

    ajax.send();
}

/**
 * Configura a localização pt-BR no Syncfusion
 */
function configurarLocalizacaoSyncfusion()
{
    // Definir locale pt-BR
    ej.base.L10n.load({
        'pt-BR': {
            'calendar': {
                today: 'Hoje'
            }
        }
    });

    // Configurar cultura padrío
    ej.base.setCulture('pt-BR');
    ej.base.setCurrencyCode('BRL');
}

/**
 * Inicializa o calendário Syncfusion de seleção múltipla
 */
function inicializarCalendario()
{
    try
    {
        const calElement = document.getElementById("calDatasSelecionadas");

        if (!calElement)
        {
            console.error("âŒ Elemento calDatasSelecionadas não encontrado");
            return;
        }

        console.log("ðŸ”§ Criando instância do Calendar Syncfusion...");

        // Configurar locale português se ainda não foi
        if (ej.base && ej.base.L10n && ej.base.L10n.load)
        {
            ej.base.L10n.load({
                'pt-BR': {
                    'calendar': {
                        today: 'Hoje'
                    }
                }
            });
        }

        // Criar instância do Calendar com seleção múltipla
        const calendar = new ej.calendars.Calendar({
            // Permitir seleção múltipla
            isMultiSelection: true,

            // Valores iniciais vazios
            values: [],

            // Locale português
            locale: 'pt-BR',

            // Data mÃ­nima: hoje
            min: new Date(),

            // Evento de mudança
            change: function (args)
            {
                console.log("ðŸ“… Datas selecionadas:", args.values);
                atualizarBadgeContador();
            },

            // Renderização de células
            renderDayCell: function (args)
            {
                // Desabilitar datas passadas
                const hoje = new Date();
                hoje.setHours(0, 0, 0, 0);

                if (args.date < hoje)
                {
                    args.isDisabled = true;
                }
            }
        });

        // Anexar ao elemento
        calendar.appendTo(calElement);

        console.log("âœ… Calendário inicializado com sucesso!");
        console.log("   ðŸ“‹ Tipo:", calendar.getModuleName());

    } catch (error)
    {
        console.error("âŒ Erro ao inicializar calendário:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-logic.js", "inicializarCalendario", error);
        }
    }
}

/**
 * Cria o badge contador de dias selecionados
 */
function criarBadgeContador()
{
    try
    {
        // O badge deve ficar SOBRE O CALENDÁRIO, não sobre o container
        const calDatasSelecionadas = document.getElementById("calDatasSelecionadas");

        if (!calDatasSelecionadas)
        {
            console.warn("âš ï¸ Elemento calDatasSelecionadas não encontrado");
            return;
        }

        // Verificar se o badge já existe
        let badge = document.getElementById("badgeContadorDias");

        if (!badge)
        {
            // Criar novo badge
            badge = document.createElement("span");
            badge.id = "badgeContadorDias";
            badge.className = "badge-contador-dias";
            badge.textContent = "0";

            // Estilizar o badge
            badge.style.position = "absolute";
            badge.style.top = "-25px"; // Mais fora! (55% fora do calendário)
            badge.style.right = "-25px"; // Mais fora! (55% fora do calendário)
            badge.style.backgroundColor = "#ff8c00"; // Laranja
            badge.style.color = "white";
            badge.style.borderRadius = "50%";
            badge.style.width = "45px";
            badge.style.height = "45px";
            badge.style.display = "flex";
            badge.style.alignItems = "center";
            badge.style.justifyContent = "center";
            badge.style.fontSize = "18px";
            badge.style.fontWeight = "bold";
            badge.style.zIndex = "1000";
            badge.style.boxShadow = "0 2px 8px rgba(0, 0, 0, 0.3)";
            badge.style.border = "3px solid white"; // Borda branca para destacar

            // Posicionar o calDatasSelecionadas como relative
            calDatasSelecionadas.style.position = "relative";

            // Adicionar o badge AO CALENDÁRIO (não ao container)
            calDatasSelecionadas.appendChild(badge);

            console.log("   âœ… Badge contador criado e posicionado sobre o calendário");
        }
        else
        {
            // Resetar contador se já existe
            badge.textContent = "0";
            console.log("   âœ… Badge resetado");
        }

    } catch (error)
    {
        console.error("âŒ Erro ao criar badge:", error);
    }
}

/**
 * Configura atualização automática do badge
 */
function configurarAtualizacaoBadge()
{
    try
    {
        const calDatasSelecionadasElement = document.getElementById("calDatasSelecionadas");

        if (!calDatasSelecionadasElement)
        {
            console.warn("âš ï¸ Elemento calDatasSelecionadas não encontrado no DOM");
            return;
        }

        if (!calDatasSelecionadasElement.ej2_instances || !calDatasSelecionadasElement.ej2_instances[0])
        {
            console.warn("âš ï¸ Calendário calDatasSelecionadas não está inicializado");
            console.log("ðŸ’¡ Isso é normal se o calendário ainda não foi renderizado");
            return;
        }

        const calendario = calDatasSelecionadasElement.ej2_instances[0];

        console.log("âœ… Calendário encontrado! Tipo:", calendario.getModuleName());

        // Interceptar o evento de mudança do calendário
        const changeOriginal = calendario.change;

        calendario.change = function (args)
        {
            // Executar função original se existir
            if (changeOriginal)
            {
                changeOriginal.call(calendario, args);
            }

            // Atualizar o badge
            atualizarBadgeContador();
        };

        console.log("   âœ… Atualização de badge configurada");

    } catch (error)
    {
        console.error("âŒ Erro ao configurar atualização de badge:", error);
    }
}

/**
 * Atualiza o número no badge de contador
 */
function atualizarBadgeContador()
{
    try
    {
        const badge = document.getElementById("badgeContadorDias");
        const calDatasSelecionadasElement = document.getElementById("calDatasSelecionadas");

        if (!badge)
        {
            console.warn("âš ï¸ Badge não encontrado");
            return;
        }

        if (!calDatasSelecionadasElement || !calDatasSelecionadasElement.ej2_instances)
        {
            console.warn("âš ï¸ Calendário não encontrado para atualizar badge");
            badge.textContent = "0";
            return;
        }

        const calendario = calDatasSelecionadasElement.ej2_instances[0];

        if (!calendario)
        {
            badge.textContent = "0";
            return;
        }

        // Contar datas selecionadas
        const datasSelecionadas = calendario.values || [];
        const quantidade = datasSelecionadas.length;

        // Atualizar badge
        badge.textContent = quantidade.toString();

        console.log(`   ðŸ“Š Badge atualizado: ${quantidade} dias selecionados`);

    } catch (error)
    {
        console.error("âŒ Erro ao atualizar badge:", error);
    }
}

/**
 * Limpa valores dos campos ao mudar lstRecorrente
 */
function limparCamposRecorrenciaAoMudar()
{
    try
    {
        // Limpar lstPeriodos
        const lstPeriodosElement = document.getElementById("lstPeriodos");
        if (lstPeriodosElement && lstPeriodosElement.ej2_instances)
        {
            const lstPeriodos = lstPeriodosElement.ej2_instances[0];
            if (lstPeriodos)
            {
                lstPeriodos.value = null;
                lstPeriodos.dataBind();
            }
        }

        // Limpar lstDias
        const lstDiasElement = document.getElementById("lstDias");
        if (lstDiasElement && lstDiasElement.ej2_instances)
        {
            const lstDias = lstDiasElement.ej2_instances[0];
            if (lstDias)
            {
                lstDias.value = [];
                lstDias.dataBind();
            }
        }

        // Limpar lstDiasMes
        const lstDiasMesElement = document.getElementById("lstDiasMes");
        if (lstDiasMesElement && lstDiasMesElement.ej2_instances)
        {
            const lstDiasMes = lstDiasMesElement.ej2_instances[0];
            if (lstDiasMes)
            {
                lstDiasMes.value = null;
                lstDiasMes.dataBind();
            }
        }

        // Limpar txtFinalRecorrencia
        window.setKendoDateValue("txtFinalRecorrencia", null);

        // Limpar calendário
        const calDatasSelecionadasElement = document.getElementById("calDatasSelecionadas");
        if (calDatasSelecionadasElement && calDatasSelecionadasElement.ej2_instances)
        {
            const calendario = calDatasSelecionadasElement.ej2_instances[0];
            if (calendario)
            {
                calendario.values = [];
                calendario.dataBind();
            }
        }

        // Resetar badge
        const badge = document.getElementById("badgeContadorDias");
        if (badge)
        {
            badge.textContent = "0";
        }

    } catch (error)
    {
        console.error("âŒ Erro ao limpar campos:", error);
    }
}

// ====================================================================
// INICIALIZAÇÃO AUTOMÃTICA
// ====================================================================

// Chamar inicialização quando o documento estiver pronto
if (document.readyState === 'loading')
{
    document.addEventListener('DOMContentLoaded', () =>
    {
        // Aguardar um pouco para garantir que os controles Syncfusion foram renderizados
        setTimeout(() =>
        {
            window.inicializarLogicaRecorrencia();
        }, 1000);
    });
}
else
{
    // Documento já carregado
    setTimeout(() =>
    {
        window.inicializarLogicaRecorrencia();
    }, 1000);
}
