/* ****************************************************************************************
 * ⚡ ARQUIVO: calendario-config.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Configuração e tradução PT-BR para Syncfusion Calendar. 3 funções
 *                   para localizar calendário: configurar L10n (ej.base.L10n.load com
 *                   traduções 'Hoje', 'Selecione uma data'), criar instância Calendar com
 *                   firstDayOfWeek=0 (domingo), traduzir manualmente headers DOM (setTimeout
 *                   100ms para aguardar renderização). Abordagem híbrida: L10n oficial +
 *                   manipulação DOM manual para headers (dias semana: Dom-Sáb, meses:
 *                   Janeiro-Dezembro). Exporta window.CalendarioConfig object com 3 métodos.
 * 📥 ENTRADAS     : configurarCalendarioPtBR() sem params, criarCalendario(elementoId:
 *                   string, dataInicial?: Date), traduzirCalendario(elementoId: string)
 * 📤 SAÍDAS       : configurarCalendarioPtBR retorna void (side effect: L10n.load),
 *                   criarCalendario retorna Syncfusion Calendar instance, traduzirCalendario
 *                   retorna void (side effect: DOM textContent updates)
 * 🔗 CHAMADA POR  : main.js (startup: CalendarioConfig.configurar()), components (calendario.js,
 *                   exibe-viagem.js: CalendarioConfig.criar para criar instância Calendar),
 *                   Syncfusion Calendar callbacks (created, navigated events chamam
 *                   CalendarioConfig.traduzir automaticamente)
 * 🔄 CHAMA        : ej.base.L10n.load (Syncfusion localization), ej.calendars.Calendar
 *                   constructor (new Calendar({ options })), calendar.appendTo (Syncfusion
 *                   method), setTimeout (100ms delay), document.getElementById,
 *                   element.querySelectorAll ('.e-calendar th' para headers, '.e-title'
 *                   para mês/ano), Array.forEach, element.textContent setter,
 *                   element.getAttribute('aria-label')
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 Calendars (ej.base.L10n, ej.calendars.Calendar, ej.base
 *                   existence check), DOM elements (dynamic via elementoId parameter),
 *                   Syncfusion CSS classes (.e-calendar, .e-title, th headers)
 * 📝 OBSERVAÇÕES  : Exporta window.CalendarioConfig = { configurar, criar, traduzir } (3
 *                   aliases para funções). Sem try-catch (funções simples). Safe check:
 *                   typeof ej !== 'undefined' antes de L10n.load. setTimeout necessário
 *                   porque Syncfusion renderiza assíncronamente (100ms suficiente). Manual
 *                   translation via DOM: alternativa a CLDR completo (mais simples, menos
 *                   robusto). firstDayOfWeek=0 hardcoded (domingo, padrão brasileiro).
 *                   dayHeaderFormat='Short' usa abreviações (Dom, Seg, Ter). Meses array
 *                   definido mas não usado atualmente (linha 50-52, commented logic linha
 *                   71-78 não funcional). created + navigated callbacks garantem tradução
 *                   após render inicial e após navegação mês/ano. querySelectorAll('.e-calendar
 *                   th') seleciona 7 headers (dias semana). Index check (index < diasSemana.length)
 *                   previne erro se mais headers encontrados.
 *
 * 📋 ÍNDICE DE FUNÇÕES (3 funções globais, 1 export object):
 *
 * ┌─ configurarCalendarioPtBR() ────────────────────────────────────────┐
 * │ → Configura traduções L10n para Syncfusion Calendar e DatePicker   │
 * │ → returns void                                                       │
 * │ → Fluxo:                                                             │
 * │   1. if typeof ej !== 'undefined' && ej.base && ej.base.L10n:       │
 * │      a. ej.base.L10n.load({                                          │
 * │           'pt-BR': {                                                 │
 * │             'calendar': { today: 'Hoje' },                           │
 * │             'datepicker': { placeholder: 'Selecione uma data',      │
 * │                             today: 'Hoje' }                          │
 * │           }                                                          │
 * │         })                                                           │
 * │ → Safe check: verifica ej existence antes de load                   │
 * │ → Traduz 2 componentes: calendar e datepicker                       │
 * │ → Strings traduzidas: today, placeholder                             │
 * │ → Chamada típica: CalendarioConfig.configurar() no app startup      │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ criarCalendario(elementoId, dataInicial) ──────────────────────────┐
 * │ → Cria instância Syncfusion Calendar com PT-BR manual translation   │
 * │ → param elementoId: string, ID do elemento DOM (sem #)              │
 * │ → param dataInicial: Date opcional, valor inicial (default new Date)│
 * │ → returns ej.calendars.Calendar instance                             │
 * │ → Fluxo:                                                             │
 * │   1. var calendario = new ej.calendars.Calendar({                   │
 * │        value: dataInicial || new Date(),                             │
 * │        firstDayOfWeek: 0,  // Domingo                                │
 * │        dayHeaderFormat: 'Short',  // Dom, Seg, Ter                   │
 * │        created: function() { traduzirCalendario(elementoId) },       │
 * │        navigated: function() { traduzirCalendario(elementoId) }      │
 * │      })                                                              │
 * │   2. calendario.appendTo('#' + elementoId)                           │
 * │   3. return calendario                                               │
 * │ → Callbacks: created dispara após render inicial, navigated após    │
 * │   mudança de mês/ano (ambos chamam traduzirCalendario)              │
 * │ → firstDayOfWeek=0: domingo (padrão BR, diferente de ISO 8601       │
 * │   que usa segunda=1)                                                 │
 * │ → dayHeaderFormat='Short': usa abreviações (Dom vs Domingo)         │
 * │ → appendTo: monta calendar no DOM element #elementoId               │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ traduzirCalendario(elementoId) ────────────────────────────────────┐
 * │ → Traduz manualmente headers DOM do calendar (dias semana)          │
 * │ → param elementoId: string, ID do elemento DOM                      │
 * │ → returns void (side effect: atualiza textContent)                   │
 * │ → Fluxo:                                                             │
 * │   1. var diasSemana = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex',    │
 * │      'Sáb']                                                          │
 * │   2. var meses = ['Janeiro', ..., 'Dezembro'] (não usado atualmente)│
 * │   3. setTimeout(() => {                                              │
 * │        a. var elemento = getElementById(elementoId)                  │
 * │        b. if elemento:                                               │
 * │           - var headers = querySelectorAll('.e-calendar th')         │
 * │           - headers.forEach((header, index) => {                     │
 * │               if (index < diasSemana.length):                        │
 * │                 header.textContent = diasSemana[index]               │
 * │             })                                                       │
 * │           - var titulo = querySelector('.e-title')                   │
 * │           - if titulo: getAttribute('aria-label') (não usado)        │
 * │      }, 100)                                                         │
 * │ → setTimeout 100ms: aguarda Syncfusion renderizar DOM completo      │
 * │ → .e-calendar th: seleciona headers <th> de dias semana (7 elementos)│
 * │ → .e-title: seleciona elemento título mês/ano (lógica comentada)    │
 * │ → Meses array definido mas não aplicado (línhas 71-78 incompletas)  │
 * │ → Index guard: previne erro se headers.length > diasSemana.length   │
 * │ → Abordagem manual: necessária porque L10n não cobre day headers    │
 * │   completamente em algumas versões Syncfusion                       │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EXPORT window.CalendarioConfig ────────────────────────────────────┐
 * │ window.CalendarioConfig = {                                          │
 * │   configurar: configurarCalendarioPtBR,                              │
 * │   criar: criarCalendario,                                            │
 * │   traduzir: traduzirCalendario                                       │
 * │ }                                                                    │
 * │ → Object com 3 métodos públicos                                      │
 * │ → Uso: CalendarioConfig.configurar()                                 │
 * │ →      const cal = CalendarioConfig.criar('meuCalendario')           │
 * │ →      CalendarioConfig.traduzir('meuCalendario')                    │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO DE USO:
 * 1. App startup (main.js): CalendarioConfig.configurar() → carrega L10n PT-BR
 * 2. Criar calendar (exibe-viagem.js): const cal = CalendarioConfig.criar('calendario1',
 *    new Date('2026-02-01'))
 * 3. Syncfusion renderiza calendar → dispara created callback
 * 4. created callback → traduzirCalendario('calendario1')
 * 5. setTimeout 100ms → querySelectorAll headers → textContent = ['Dom', 'Seg', ...]
 * 6. Calendar exibido com headers em PT-BR
 * 7. Usuário navega para próximo mês → dispara navigated callback
 * 8. navigated → traduzirCalendario novamente → headers re-traduzidos
 *
 * 📌 DIAS SEMANA ARRAY (7 elementos):
 * ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb']
 * → Abreviações de 3 letras (padrão brasileiro)
 * → Index 0 = Domingo (firstDayOfWeek=0)
 * → Aplicado via textContent em <th> elements
 *
 * 📌 MESES ARRAY (12 elementos - NÃO USADO):
 * ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
 *  'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro']
 * → Nomes completos (não abreviados)
 * → Definido mas não aplicado (linhas 71-78 comentadas/incompletas)
 * → Para traduzir meses: precisaria CLDR completo ou lógica adicional
 *
 * 📌 SYNCFUSION CSS CLASSES:
 * - .e-calendar: container principal do calendar
 * - .e-calendar th: headers de dias semana (7 elementos <th>)
 * - .e-title: elemento título mostrando "Mês Ano" (ex: "February 2026")
 * - aria-label: atributo accessibility com data formatada
 *
 * 📌 SYNCFUSION CALENDAR OPTIONS:
 * - value: Date inicial exibido (default today)
 * - firstDayOfWeek: 0=Domingo, 1=Segunda, ..., 6=Sábado
 * - dayHeaderFormat: 'Short' (abreviado), 'Narrow' (1 letra), 'Wide' (completo)
 * - created: callback após render inicial
 * - navigated: callback após navegação mês/ano (arrows, dropdown)
 *
 * 📌 TIMING (setTimeout 100ms):
 * - Necessário porque Syncfusion renderiza DOM assíncronamente
 * - 100ms geralmente suficiente (pode precisar ajuste em sistemas lentos)
 * - Alternativa: usar requestAnimationFrame ou MutationObserver
 * - created/navigated callbacks disparam antes de DOM completo (daí setTimeout)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Sem try-catch (funções simples, erros não críticos)
 * - Safe check: typeof ej !== 'undefined' previne erro se Syncfusion não carregado
 * - Manual translation: workaround para limitações L10n Syncfusion (nem todos strings
 *   traduzíveis via L10n.load)
 * - CLDR vs Manual: CLDR seria abordagem oficial (loadCldr com CLDR JSON completo),
 *   mas requer arquivo grande e configuração complexa; manual é mais simples
 * - firstDayOfWeek=0 brasileiro: diferente de Europa/ISO (segunda=1), mesmo padrão
 *   que calendários físicos brasileiros
 * - var keyword: código usa var (pre-ES6), poderia migrar para const/let
 * - Function keyword: function() callbacks (pre-ES6), poderiam ser arrow functions
 * - querySelectorAll + forEach: pattern ES5+ (funciona em browsers modernos)
 * - elemento existence check: if (elemento) previne erro se ID não encontrado
 * - headers.forEach index check: previne crash se Syncfusion mudar estrutura DOM
 * - meses array: preparado para feature futura (traduzir título mês/ano), não
 *   implementado completamente (linhas 71-78 incompletas)
 * - aria-label: atributo acessibilidade, não usado atualmente mas poderia ser
 *   fonte para parsing de mês/ano
 * - textContent vs innerHTML: textContent mais seguro (não injeta HTML), suficiente
 *   para texto simples
 * - Calendar instance: retornado de criarCalendario para permitir manipulação
 *   externa (ex: cal.value = new Date(), cal.destroy())
 * - appendTo('#' + elementoId): concatenação manual, assume elementoId sem #
 * - Global export: window.CalendarioConfig torna funções acessíveis globalmente
 *   (não usa ES6 modules export)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

// Tradução manual para português do Brasil
function configurarCalendarioPtBR()
{
    // Definir traduções
    if (typeof ej !== 'undefined' && ej.base && ej.base.L10n)
    {
        ej.base.L10n.load({
            'pt-BR': {
                'calendar': {
                    today: 'Hoje'
                },
                'datepicker': {
                    placeholder: 'Selecione uma data',
                    today: 'Hoje'
                }
            }
        });
    }
}

function criarCalendario(elementoId, dataInicial)
{
    // Criar calendário com nomes em português manualmente
    var calendario = new ej.calendars.Calendar({
        value: dataInicial || new Date(),
        firstDayOfWeek: 0, // Domingo
        // Nomes dos dias da semana em português
        dayHeaderFormat: 'Short',
        // Eventos para traduzir manualmente
        created: function ()
        {
            traduzirCalendario(elementoId);
        },
        navigated: function ()
        {
            traduzirCalendario(elementoId);
        }
    });

    calendario.appendTo('#' + elementoId);
    return calendario;
}

function traduzirCalendario(elementoId)
{
    // Traduzir dias da semana
    var diasSemana = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'];
    var meses = [
        'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
    ];

    setTimeout(function ()
    {
        // Traduzir cabeçalho dos dias
        var elemento = document.getElementById(elementoId);
        if (elemento)
        {
            var headers = elemento.querySelectorAll('.e-calendar th');
            headers.forEach(function (header, index)
            {
                if (index < diasSemana.length)
                {
                    header.textContent = diasSemana[index];
                }
            });

            // Traduzir o título do mês/ano
            var titulo = elemento.querySelector('.e-title');
            if (titulo)
            {
                var textoOriginal = titulo.textContent;
                // Tentar identificar e traduzir o mês
                var dataAtual = titulo.getAttribute('aria-label');
                // Esta é uma abordagem básica, você pode melhorar conforme necessário
            }
        }
    }, 100);
}

// Expor funções globalmente
window.CalendarioConfig = {
    configurar: configurarCalendarioPtBR,
    criar: criarCalendario,
    traduzir: traduzirCalendario
};
