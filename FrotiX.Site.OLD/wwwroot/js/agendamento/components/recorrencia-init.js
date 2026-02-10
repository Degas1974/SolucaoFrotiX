/* ****************************************************************************************
 * ⚡ ARQUIVO: recorrencia-init.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicialização e população de controles Syncfusion de recorrência.
 *                   6 funções para popular dataSource de DropDownList (lstDias 0-6 Dom-Sáb,
 *                   lstDiasMes 1-31, lstPeriodos 5 tipos) e DateTimePicker (txtFinalRecorrencia
 *                   min=hoje). Criar/destruir instâncias Syncfusion EJ2 DropDownList para
 *                   lstPeriodos dinamicamente. Retry pattern para aguardar Syncfusion carregar
 *                   (setTimeout 200ms recursivo). Auto-init com setTimeout 500ms se Syncfusion
 *                   já carregado. Todas funções exportadas como window.* para chamada externa
 *                   (recorrencia-logic.js, modal-viagem-novo.js).
 * 📥 ENTRADAS     : Nenhuma entrada direta (funções sem parâmetros), usa DOM elements
 *                   (lstDias, lstDiasMes, lstPeriodos, txtFinalRecorrencia) e ej.dropdowns.DropDownList
 *                   constructor, checks de ej2_instances existence
 * 📤 SAÍDAS       : Boolean (inicializarLstDias/lstDiasMes/txtFinalRecorrencia: true success,
 *                   false falha), void (inicializarControlesRecorrencia, inicializarDropdownPeriodos,
 *                   rebuildLstPeriodos side effects: dataSource population, instance creation),
 *                   console.log debug (emoji prefixes ✅⚠️❌)
 * 🔗 CHAMADA POR  : recorrencia-logic.js (mostrarLstDias/mostrarLstDiasMes → inicializarLstDias/inicializarLstDiasMes),
 *                   recorrencia-logic.js (inicializarLogicaRecorrencia → inicializarDropdownPeriodos),
 *                   modal-viagem-novo.js (aoAbrirModalViagem → inicializarControlesRecorrencia),
 *                   auto-init (setTimeout 500ms se ej.dropdowns carregado)
 * 🔄 CHAMA        : Syncfusion EJ2 API (getElementById().ej2_instances[0], .dataSource setter,
 *                   .dataBind(), .destroy(), new ej.dropdowns.DropDownList(), .appendTo()),
 *                   Alerta.TratamentoErroComLinha, console.log/warn (debug com emojis),
 *                   setTimeout (200ms retry inicializarDropdownPeriodos, 500ms auto-init),
 *                   Date() constructor (hoje para min txtFinalRecorrencia)
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 DropDownList (ej.dropdowns.DropDownList, ej2_instances),
 *                   Syncfusion DateTimePicker (txtFinalRecorrencia.min setter), Alerta
 *                   (TratamentoErroComLinha), DOM elements (lstDias, lstDiasMes, lstPeriodos,
 *                   txtFinalRecorrencia ids), recorrencia-logic.js (chama inicializar* functions)
 * 📝 OBSERVAÇÕES  : 6 funções window.* exports: inicializarControlesRecorrencia (wrapper dos
 *                   3 inicializar), inicializarLstDiasMes (1-31 array), inicializarLstDias
 *                   (Dom-Sáb array), inicializarTxtFinalRecorrencia (min=hoje), inicializarDropdownPeriodos
 *                   (criar DropDownList com 5 períodos), rebuildLstPeriodos (wrapper destroy
 *                   + recreate). Try-catch completo em todas as funções com Alerta.TratamentoErroComLinha.
 *                   Console.log com emoji prefixes: 🔧 (init), ✅ (success), ⚠️ (warning),
 *                   ❌ (error), 🗑️ (destroy), 🔄 (rebuild), ℹ️ (info), 📊 (data). Safe checks:
 *                   if (!element) return false, if (!ej2_instances) return false, if
 *                   (dataSource.length > 0) skip (já populado). Retry pattern: setTimeout
 *                   200ms recursivo em inicializarDropdownPeriodos se Syncfusion não carregado.
 *                   Auto-init: if (ej.dropdowns) setTimeout 500ms → inicializarDropdownPeriodos.
 *                   lstPeriodos.change callback: removido (linha 239 comentada), substituído
 *                   por recorrencia-logic.js configurarEventHandlerPeriodo. DropDownList config:
 *                   fields={text:'Periodo', value:'PeriodoId'}, placeholder, popupHeight=200px,
 *                   floatLabelType='Never', cssClass='e-outline', width=100%. Períodos (5
 *                   tipos): D (Diário), S (Semanal), Q (Quinzenal), M (Mensal), V (Dias
 *                   Variados). Dias semana (7 items): 0=Domingo, 1=Segunda, 2=Terça, 3=Quarta,
 *                   4=Quinta, 5=Sexta, 6=Sábado. Dias mês (31 items): 1-31 com Value=i,
 *                   Text=i.toString(). txtFinalRecorrencia.min: garante que usuário não
 *                   seleciona data passada (validação client-side).
 *
 * 📋 ÍNDICE DE FUNÇÕES (6 exports window.*):
 *
 * ┌─ FUNÇÃO WRAPPER (1 função) ─────────────────────────────────────────┐
 * │ 1. window.inicializarControlesRecorrencia()                          │
 * │    → Wrapper que chama todos os 3 inicializar* em sequência         │
 * │    → returns void (side effect: chama 3 funções)                    │
 * │    → Fluxo:                                                          │
 * │      1. console.log "Inicializando controles de recorrência"        │
 * │      2. window.inicializarLstDiasMes()                              │
 * │      3. window.inicializarLstDias()                                 │
 * │      4. window.inicializarTxtFinalRecorrencia()                     │
 * │      5. console.log "Controles inicializados"                       │
 * │      6. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: modal-viagem-novo.aoAbrirModalViagem               │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ POPULADORES DE DATASOURCE (3 funções) ─────────────────────────────┐
 * │ 2. window.inicializarLstDiasMes()                                    │
 * │    → Popula lstDiasMes com dias 1-31                                │
 * │    → returns boolean: true success, false falha/não renderizado     │
 * │    → Fluxo:                                                          │
 * │      1. getElementById("lstDiasMes")                                │
 * │      2. Verificar ej2_instances[0] exists (se não: return false)    │
 * │      3. Verificar se já populado (dataSource.length > 0): skip      │
 * │      4. Criar array: for i=1 to 31: { Value: i, Text: i.toString() }│
 * │      5. lstDiasMesObj.dataSource = diasDoMes                        │
 * │      6. lstDiasMesObj.dataBind()                                    │
 * │      7. console.log "lstDiasMes populado com 31 dias"               │
 * │      8. return true                                                 │
 * │      9. try-catch: Alerta.TratamentoErroComLinha + return false     │
 * │    → Chamado por: inicializarControlesRecorrencia,                  │
 * │      recorrencia-logic.mostrarLstDiasMes                            │
 * │                                                                       │
 * │ 3. window.inicializarLstDias()                                       │
 * │    → Popula lstDias com dias da semana (Dom-Sáb)                    │
 * │    → returns boolean: true success, false falha                     │
 * │    → Fluxo:                                                          │
 * │      1. getElementById("lstDias")                                   │
 * │      2. Verificar ej2_instances[0] exists (se não: return false)    │
 * │      3. Verificar se já populado: skip                              │
 * │      4. Criar array diasDaSemana (7 items):                         │
 * │         - { Value: 0, Text: "Domingo" }                             │
 * │         - { Value: 1, Text: "Segunda" }                             │
 * │         - { Value: 2, Text: "Terça" }                               │
 * │         - { Value: 3, Text: "Quarta" }                              │
 * │         - { Value: 4, Text: "Quinta" }                              │
 * │         - { Value: 5, Text: "Sexta" }                               │
 * │         - { Value: 6, Text: "Sábado" }                              │
 * │      5. lstDiasObj.dataSource = diasDaSemana                        │
 * │      6. lstDiasObj.dataBind()                                       │
 * │      7. console.log "lstDias populado com dias da semana"           │
 * │      8. return true                                                 │
 * │      9. try-catch: Alerta.TratamentoErroComLinha + return false     │
 * │    → Chamado por: inicializarControlesRecorrencia,                  │
 * │      recorrencia-logic.mostrarLstDias                               │
 * │                                                                       │
 * │ 4. window.inicializarTxtFinalRecorrencia()                          │
 * │    → Configura txtFinalRecorrencia (min=hoje)                       │
 * │    → returns boolean: true success, false falha                     │
 * │    → Fluxo:                                                          │
 * │      1. getElementById("txtFinalRecorrencia")                       │
 * │      2. Verificar ej2_instances[0] exists (se não: return false)    │
 * │      3. Obter hoje = new Date()                                     │
 * │      4. txtFinalRecorrenciaObj.min = hoje                           │
 * │      5. console.log "txtFinalRecorrencia configurado"               │
 * │      6. return true                                                 │
 * │      7. try-catch: Alerta.TratamentoErroComLinha + return false     │
 * │    → Chamado por: inicializarControlesRecorrencia                   │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CRIADOR DE DROPDOWNLIST (2 funções) ───────────────────────────────┐
 * │ 5. window.inicializarDropdownPeriodos()                              │
 * │    → Cria/recria DropDownList lstPeriodos com 5 períodos            │
 * │    → returns void (side effect: DropDownList instance criada)       │
 * │    → Fluxo: (67 linhas)                                             │
 * │      1. console.log "Inicializando dropdown de períodos"            │
 * │      2. Verificar ej.dropdowns.DropDownList carregado:              │
 * │         - Se não: setTimeout(inicializarDropdownPeriodos, 200) retry│
 * │      3. getElementById("lstPeriodos")                               │
 * │      4. Destruir instância anterior se exists:                      │
 * │         - lstPeriodosElement.ej2_instances[0].destroy()             │
 * │      5. Criar array periodos (5 items):                             │
 * │         - { PeriodoId: "D", Periodo: "Diário" }                     │
 * │         - { PeriodoId: "S", Periodo: "Semanal" }                    │
 * │         - { PeriodoId: "Q", Periodo: "Quinzenal" }                  │
 * │         - { PeriodoId: "M", Periodo: "Mensal" }                     │
 * │         - { PeriodoId: "V", Periodo: "Dias Variados" }              │
 * │      6. Criar dropdownPeriodos = new ej.dropdowns.DropDownList({    │
 * │         dataSource: periodos,                                       │
 * │         fields: { text: 'Periodo', value: 'PeriodoId' },            │
 * │         placeholder: 'Selecione o período...',                      │
 * │         popupHeight: '200px',                                       │
 * │         floatLabelType: 'Never',                                    │
 * │         cssClass: 'e-outline',                                      │
 * │         width: '100%'                                               │
 * │       })                                                             │
 * │      7. dropdownPeriodos.appendTo(lstPeriodosElement)               │
 * │      8. console.log "Dropdown inicializado" + "Total períodos: 5"   │
 * │      9. try-catch: console.error + Alerta.TratamentoErroComLinha    │
 * │    → Nota: change callback removido (linha 239), substituído por    │
 * │      recorrencia-logic.configurarEventHandlerPeriodo               │
 * │    → Chamado por: recorrencia-logic.inicializarLogicaRecorrencia,  │
 * │      auto-init setTimeout                                           │
 * │                                                                       │
 * │ 6. window.rebuildLstPeriodos()                                       │
 * │    → Wrapper para rebuild completo de lstPeriodos                   │
 * │    → returns void (side effect: chama inicializarDropdownPeriodos)  │
 * │    → Fluxo:                                                          │
 * │      1. console.log "Reconstruindo dropdown de períodos"            │
 * │      2. window.inicializarDropdownPeriodos()                        │
 * │      3. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: reset completo do dropdown                         │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ AUTO-INITIALIZATION ────────────────────────────────────────────────┐
 * │ if (typeof ej !== 'undefined' && ej.dropdowns && ej.dropdowns.DropDownList)│
 * │   console.log "Syncfusion DropDownList disponível"                  │
 * │   setTimeout 500ms:                                                  │
 * │     try:                                                             │
 * │       if (getElementById("lstPeriodos"))                            │
 * │         window.inicializarDropdownPeriodos()                        │
 * │     catch: Alerta.TratamentoErroComLinha("auto-init")               │
 * │ else                                                                  │
 * │   console.warn "Syncfusion ainda não carregado, aguardando"         │
 * │ → Executa apenas se Syncfusion já carregado no momento do script    │
 * │ → Delay 500ms para garantir que DOM está pronto                     │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO 1 - INICIALIZAR NA ABERTURA DO MODAL:
 * 1. Usuário clica "Novo Agendamento" → modal Bootstrap show
 * 2. modal-viagem-novo.aoAbrirModalViagem trigger
 * 3. aoAbrirModalViagem → inicializarControlesRecorrencia()
 * 4. inicializarLstDiasMes():
 *    - getElementById("lstDiasMes") → obter element
 *    - Verificar ej2_instances[0] → existe (Razor já renderizou)
 *    - Criar array 1-31: [{ Value: 1, Text: "1" }, ..., { Value: 31, Text: "31" }]
 *    - lstDiasMesObj.dataSource = diasDoMes
 *    - lstDiasMesObj.dataBind()
 *    - return true
 * 5. inicializarLstDias():
 *    - Similar, array Dom-Sáb
 *    - return true
 * 6. inicializarTxtFinalRecorrencia():
 *    - txtFinalRecorrenciaObj.min = new Date()
 *    - return true
 * 7. console.log "Controles inicializados"
 * 8. Controles prontos para uso
 *
 * 🔄 FLUXO TÍPICO 2 - LAZY INIT AO MOSTRAR CAMPO:
 * 1. Usuário seleciona lstPeriodos="Mensal"
 * 2. recorrencia-logic.aoMudarPeriodo trigger
 * 3. aoMudarPeriodo → mostrarLstDiasMes()
 * 4. mostrarLstDiasMes → divDiaMes.display='block'
 * 5. setTimeout 100ms → inicializarLstDiasMes()
 * 6. inicializarLstDiasMes: verificar dataSource
 *    - Se length > 0: console.log "já populado" + return true (skip)
 *    - Se length == 0: popular + dataBind + return true
 * 7. Campo exibido com 31 dias disponíveis
 *
 * 🔄 FLUXO TÍPICO 3 - CRIAR LSTPERIODOS DINAMICAMENTE:
 * 1. Page load → script recorrencia-init.js executa
 * 2. Auto-init: verificar ej.dropdowns.DropDownList carregado
 * 3. Se carregado: setTimeout 500ms → inicializarDropdownPeriodos()
 * 4. inicializarDropdownPeriodos:
 *    - Verificar ej.dropdowns existe (se não: setTimeout 200ms retry)
 *    - getElementById("lstPeriodos") → obter <input> vazio
 *    - Destruir instance anterior se exists (safety)
 *    - Criar array periodos (5 items: D, S, Q, M, V)
 *    - new ej.dropdowns.DropDownList({ dataSource, fields, ... })
 *    - dropdownPeriodos.appendTo(lstPeriodosElement)
 *    - console.log "Dropdown inicializado com 5 períodos"
 * 5. lstPeriodos agora é DropDownList funcional
 * 6. recorrencia-logic.configurarEventHandlerPeriodo → adicionar change listener
 *
 * 📌 DATASOURCES (3 arrays):
 * - lstDiasMes (31 items): [{ Value: 1, Text: "1" }, { Value: 2, Text: "2" }, ..., { Value: 31, Text: "31" }]
 * - lstDias (7 items): [{ Value: 0, Text: "Domingo" }, { Value: 1, Text: "Segunda" }, ..., { Value: 6, Text: "Sábado" }]
 * - lstPeriodos (5 items): [
 *     { PeriodoId: "D", Periodo: "Diário" },
 *     { PeriodoId: "S", Periodo: "Semanal" },
 *     { PeriodoId: "Q", Periodo: "Quinzenal" },
 *     { PeriodoId: "M", Periodo: "Mensal" },
 *     { PeriodoId: "V", Periodo: "Dias Variados" }
 *   ]
 *
 * 📌 SAFE CHECKS (4 níveis):
 * 1. Element exists: if (!element) return false
 * 2. ej2_instances exists: if (!element.ej2_instances) return false
 * 3. Instance exists: if (!element.ej2_instances[0]) return false
 * 4. Already populated: if (dataSource.length > 0) skip (performance)
 *
 * 📌 RETRY PATTERN (inicializarDropdownPeriodos):
 * - Verifica: typeof ej !== 'undefined' && ej.dropdowns && ej.dropdowns.DropDownList
 * - Se não carregado: setTimeout(inicializarDropdownPeriodos, 200) → retry recursivo
 * - Sem limite de tentativas (assume que Syncfusion eventualmente carrega)
 * - Alternativa: recorrencia-logic usa setInterval com max 10 tentativas
 *
 * 📌 SYNCFUSION DROPDOWNLIST CONFIG (lstPeriodos):
 * - dataSource: array periodos (5 items)
 * - fields: { text: 'Periodo', value: 'PeriodoId' } → mapeia props
 * - placeholder: 'Selecione o período...' → texto quando vazio
 * - popupHeight: '200px' → altura do dropdown popup
 * - floatLabelType: 'Never' → label não flutua
 * - cssClass: 'e-outline' → estilo outline (borda sem preenchimento)
 * - width: '100%' → ocupa largura completa do container
 * - change: removido → recorrencia-logic.configurarEventHandlerPeriodo adiciona depois
 *
 * 📌 TXTFINALRECORRENCIA.MIN:
 * - min: new Date() → data mínima selecionável é hoje
 * - Validação client-side: usuário não pode escolher data passada
 * - Backend também valida (redundância segurança)
 * - Format: DateTimePicker usa format="dd/MM/yyyy" (padrão PT-BR)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Todas as 6 funções exportadas como window.* (scope global)
 * - Try-catch em todas as funções com Alerta.TratamentoErroComLinha
 * - Console.log com emoji prefixes para facilitar debug visual
 * - Safe checks previnem crashes se Syncfusion não renderizou ainda
 * - Skip se dataSource.length > 0: evita repopular (performance)
 * - Return boolean: permite caller verificar se init foi bem-sucedido
 * - inicializarControlesRecorrencia: conveniente wrapper para init completo
 * - rebuildLstPeriodos: útil para reset após erros
 * - Auto-init: apenas lstPeriodos (outros dependem de lazy init)
 * - lstPeriodos único que precisa destroy+recreate (outros apenas populam dataSource)
 * - Linha 239 comentada: change callback removido (conflito com recorrencia-logic)
 * - Delay 500ms auto-init: garante que DOM está pronto (DOMContentLoaded já disparou)
 * - Retry 200ms: mais rápido que auto-init (espera menos entre tentativas)
 *
 * 🔌 VERSÃO: 2.0 (refatorado após Lote 193, adiciona comprehensive header)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */
window.inicializarControlesRecorrencia = function ()
{
    try
    {
        console.log("🔧 Inicializando controles de recorrência...");

        // Inicializar cada controle
        window.inicializarLstDiasMes();
        window.inicializarLstDias();
        window.inicializarTxtFinalRecorrencia();

        console.log("✅ Controles de recorrência inicializados");

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarControlesRecorrencia", error);
    }
};

/**
 * Inicializa o dropdown de dias do mês (1-31)
 * ✅ EXPOSTA GLOBALMENTE para poder ser chamada de outros lugares
 */
window.inicializarLstDiasMes = function ()
{
    try
    {
        const lstDiasMesElement = document.getElementById("lstDiasMes");

        if (!lstDiasMesElement)
        {
            console.warn("⚠️ lstDiasMes não encontrado no DOM");
            return false;
        }

        // Aguardar instância Syncfusion
        if (!lstDiasMesElement.ej2_instances || !lstDiasMesElement.ej2_instances[0])
        {
            console.warn("⚠️ lstDiasMes ainda não foi renderizado");
            return false;
        }

        const lstDiasMesObj = lstDiasMesElement.ej2_instances[0];

        // Verificar se já está populado
        if (lstDiasMesObj.dataSource && lstDiasMesObj.dataSource.length > 0)
        {
            console.log("ℹ️ lstDiasMes já está populado");
            return true;
        }

        // Criar array com dias de 1 a 31
        const diasDoMes = [];
        for (let i = 1; i <= 31; i++)
        {
            diasDoMes.push({
                Value: i,
                Text: i.toString()
            });
        }

        // Definir dataSource
        lstDiasMesObj.dataSource = diasDoMes;
        lstDiasMesObj.dataBind();

        console.log("✅ lstDiasMes populado com 31 dias");
        return true;

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarLstDiasMes", error);
        return false;
    }
};

/**
 * Inicializa o multiselect de dias da semana
 * ✅ EXPOSTA GLOBALMENTE para poder ser chamada de outros lugares
 */
window.inicializarLstDias = function ()
{
    try
    {
        const lstDiasElement = document.getElementById("lstDias");

        if (!lstDiasElement)
        {
            console.warn("⚠️ lstDias não encontrado no DOM");
            return false;
        }

        // Aguardar instância Syncfusion
        if (!lstDiasElement.ej2_instances || !lstDiasElement.ej2_instances[0])
        {
            console.warn("⚠️ lstDias ainda não foi renderizado");
            return false;
        }

        const lstDiasObj = lstDiasElement.ej2_instances[0];

        // Verificar se já está populado
        if (lstDiasObj.dataSource && lstDiasObj.dataSource.length > 0)
        {
            console.log("ℹ️ lstDias já está populado");
            return true;
        }

        // Dias da semana
        const diasDaSemana = [
            { Value: 0, Text: "Domingo" },
            { Value: 1, Text: "Segunda" },
            { Value: 2, Text: "Terça" },
            { Value: 3, Text: "Quarta" },
            { Value: 4, Text: "Quinta" },
            { Value: 5, Text: "Sexta" },
            { Value: 6, Text: "Sábado" }
        ];

        lstDiasObj.dataSource = diasDaSemana;
        lstDiasObj.dataBind();

        console.log("✅ lstDias populado com dias da semana");
        return true;

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarLstDias", error);
        return false;
    }
};

/**
 * Inicializa o DatePicker de data final de recorrência
 * ✅ EXPOSTA GLOBALMENTE para poder ser chamada de outros lugares
 */
window.inicializarTxtFinalRecorrencia = function ()
{
    try
    {
        const txtFinalRecorrenciaObj = window.getKendoDatePicker("txtFinalRecorrencia");

        if (!txtFinalRecorrenciaObj)
        {
            console.warn("⚠️ txtFinalRecorrencia não encontrado ou não inicializado (Kendo)");
            return false;
        }

        // Definir data mínima como hoje
        const hoje = new Date();
        if (typeof txtFinalRecorrenciaObj.min === "function")
        {
            txtFinalRecorrenciaObj.min(hoje);
        }

        console.log("✅ txtFinalRecorrencia configurado");
        return true;

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarTxtFinalRecorrencia", error);
        return false;
    }
};

/**
* ============================================
* INICIALIZAÇÃO DO DROPDOWN DE PERÍODOS
* ============================================
* 
* Este código deve ser executado quando o modal abre
* para transformar o input em um DropDownList
*/

/**
 * Inicializa ou reconstrói o dropdown de períodos
 */
window.inicializarDropdownPeriodos = function ()
{
    try
    {
        console.log("🔧 Inicializando dropdown de períodos...");

        // Verificar se o Syncfusion está carregado
        if (typeof ej === 'undefined' || !ej.dropdowns || !ej.dropdowns.DropDownList)
        {
            console.warn("⚠️ Syncfusion (ej.dropdowns.DropDownList) ainda não carregado. Aguardando...");
            // Tentar novamente após um delay
            setTimeout(window.inicializarDropdownPeriodos, 200);
            return;
        }

        const lstPeriodosElement = document.getElementById("lstPeriodos");

        if (!lstPeriodosElement)
        {
            console.error("❌ Elemento lstPeriodos não encontrado!");
            return;
        }

        // Destruir instância anterior se existir
        if (lstPeriodosElement.ej2_instances && lstPeriodosElement.ej2_instances[0])
        {
            console.log("🗑️ Destruindo instância anterior...");
            lstPeriodosElement.ej2_instances[0].destroy();
        }

        // Dados dos períodos
        const periodos = [
            { PeriodoId: "D", Periodo: "Diário" },
            { PeriodoId: "S", Periodo: "Semanal" },
            { PeriodoId: "Q", Periodo: "Quinzenal" },
            { PeriodoId: "M", Periodo: "Mensal" },
            { PeriodoId: "V", Periodo: "Dias Variados" }
        ];

        // Criar nova instância do DropDownList
        const dropdownPeriodos = new ej.dropdowns.DropDownList({
            dataSource: periodos,
            fields: {
                text: 'Periodo',
                value: 'PeriodoId'
            },
            placeholder: 'Selecione o período...',
            popupHeight: '200px',
            // change: window.PeriodosValueChange,  // ❌ REMOVIDO - Substituído por recorrencia-logic.js
            floatLabelType: 'Never',
            cssClass: 'e-outline',
            width: '100%'
        });

        // Renderizar o dropdown
        dropdownPeriodos.appendTo(lstPeriodosElement);

        console.log("✅ Dropdown de períodos inicializado com sucesso!");
        console.log("   📊 Total de períodos:", periodos.length);

    } catch (error)
    {
        console.error("❌ Erro ao inicializar dropdown de períodos:", error);
        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("recorrencia-init.js", "inicializarDropdownPeriodos", error);
        }
    }
};

/**
 * Reconstrói o dropdown de períodos (útil para resetar)
 */
window.rebuildLstPeriodos = function ()
{
    try
    {
        console.log("🔄 Reconstruindo dropdown de períodos...");
        window.inicializarDropdownPeriodos();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("recorrencia-init.js", "rebuildLstPeriodos", error);
    }
};

/**
* ============================================
* AUTO-INICIALIZAÇÃO
* ============================================
*/

// Aguardar o Syncfusion carregar
if (typeof ej !== 'undefined' && ej.dropdowns && ej.dropdowns.DropDownList)
{
    console.log("✅ Syncfusion DropDownList disponível");

    // Aguardar um pouco para garantir que o elemento existe
    setTimeout(() =>
    {
        try
        {
            if (document.getElementById("lstPeriodos"))
            {
                window.inicializarDropdownPeriodos();
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia-init.js", "auto-init", error);
        }
    }, 500);
}
else
{
    console.warn("⚠️ Syncfusion ainda não carregado, aguardando...");
}
