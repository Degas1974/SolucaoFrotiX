/* ****************************************************************************************
 * ⚡ ARQUIVO: recorrencia.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Classe GerenciadorRecorrencia para cálculo de datas recorrentes e
 *                   criação em lote de agendamentos. 9 métodos para gerar arrays de
 *                   datas baseado em tipo recorrência (Diário, Semanal, Quinzenal,
 *                   Mensal, Variado), processar handleRecurrence (loop POST para cada
 *                   data), corrigir RecorrenciaViagemId (link primeiro agendamento com
 *                   série). Principais fluxos: ajustarDataInicialRecorrente → switch
 *                   tipo → gerarRecorrenciaDiaria/Semanal/Mensal/Variada → array YYYY-MM-DD,
 *                   handleRecurrence → POST primeiro agendamento → capturar ViagemId →
 *                   loop for POST subsequentes com RecorrenciaViagemId. Usa moment.js
 *                   para manipulação datas, Syncfusion Calendar.values (multiselect),
 *                   window.criarAgendamento + enviarNovoAgendamento (modal-viagem-novo.js).
 * 📥 ENTRADAS     : tipoRecorrencia (string: "D"/"S"/"Q"/"M"/"V"), txtDataInicial/
 *                   txtFinalRecorrencia (Syncfusion DateTimePicker values), lstDias
 *                   (DropDownList value array: Sunday-Saturday), lstDiasMes (DropDownList
 *                   value array: 1-31), calDatasSelecionadas (Calendar.values Date array),
 *                   periodoRecorrente (string obsoleto, não usado), datasRecorrentes
 *                   (Date array de handleRecurrence)
 * 📤 SAÍDAS       : Array de strings YYYY-MM-DD (datas recorrência), Promise<Object>
 *                   de handleRecurrence ({ sucesso: boolean, totalCriados: int,
 *                   totalFalhas: int, parcial: boolean }), void em atualizarCalendarioExistente
 *                   (side effect: Calendar readonly), boolean de corrigirRecorrenciaViagemId
 * 🔗 CHAMADA POR  : modal-viagem-novo.criarAgendamentoNovo (ajustarDataInicialRecorrente
 *                   para calcular datas), recorrencia-init.js (btnSalvarRecorrencia click
 *                   → handleRecurrence), exibe-viagem.js (atualizarCalendarioExistente
 *                   para edição readonly)
 * 🔄 CHAMA        : moment.js (moment(), .toISOString(), .format(), .add(), .isSameOrBefore(),
 *                   .isoWeekday(), .day(), .month(), .year(), .isValid()), Syncfusion
 *                   API (getElementById().ej2_instances[0], .value getter, Calendar.values,
 *                   Calendar.refresh(), .isMultiSelection setter), window.criarAgendamento
 *                   (modal-viagem-novo.js), window.enviarNovoAgendamento (modal-viagem-novo.js),
 *                   Alerta.TratamentoErroComLinha, AppToast.show (toasts Amarelo/Vermelho),
 *                   console.log/error/warn (debug extensivo)
 * 📦 DEPENDÊNCIAS : moment.js (date manipulation), Syncfusion EJ2 Calendars (Calendar.values
 *                   multiselect, DateTimePicker: txtDataInicial/txtFinalRecorrencia),
 *                   Syncfusion DropDownList (lstDias, lstDiasMes), modal-viagem-novo.js
 *                   (criarAgendamento, enviarNovoAgendamento), Alerta (TratamentoErroComLinha),
 *                   AppToast (toasts), DOM elements (txtDataInicial, txtFinalRecorrencia,
 *                   lstDias, lstDiasMes, calDatasSelecionadas, readOnly-checkbox)
 * 📝 OBSERVAÇÕES  : Arquivo de lógica de recorrência (527 linhas, classe com 9 métodos).
 *                   window.gerenciadorRecorrencia = new GerenciadorRecorrencia() (singleton
 *                   global). 4 window.* exports como wrappers dos métodos da classe.
 *                   Try-catch completo em todos os métodos com Alerta.TratamentoErroComLinha.
 *                   Console.log extensivo para debug (production-ready). Tipos recorrência:
 *                   "D" (Diário seg-sex), "S" (Semanal), "Q" (Quinzenal), "M" (Mensal),
 *                   "V" (Variado/custom). handleRecurrence: primeiro POST retorna ViagemId
 *                   usado como RecorrenciaViagemId nos subsequentes (link série). Problema
 *                   conhecido: backend pode retornar ViagemId em diferentes estruturas
 *                   (agendamentoObj.viagemId, .novaViagem.viagemId, .data.viagemId, etc.),
 *                   código tenta múltiplas propriedades. Fallback GUID vazio: "00000000-0000-0000-0000-000000000000"
 *                   se captura falhar. Diário: apenas dias úteis (isoWeekday 1-5, segunda-sexta).
 *                   Mensal: valida datas (ex: 31 em fevereiro → skip). Variado: Calendar
 *                   multiselect ordenado cronologicamente (sort). Semanal/Quinzenal: mapeia
 *                   Sunday-Saturday → 0-6 indices. Quinzenal: dataAtual.day(8) adjust +
 *                   add 2 weeks loop. corrigirRecorrenciaViagemId: placeholder (API não
 *                   implementada, apenas log). atualizarCalendarioExistente: readonly mode
 *                   (isMultiSelection=false, readOnly-checkbox.checked=true).
 *
 * 📋 ÍNDICE DE MÉTODOS DA CLASSE (9 métodos + 4 exports):
 *
 * ┌─ CLASSE GerenciadorRecorrencia ─────────────────────────────────────┐
 * │ constructor()                                                         │
 * │   → Inicializa this.datasSelecionadas = []                          │
 * │   → Array interno para armazenar datas (não usado atualmente)       │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MÉTODO PRINCIPAL (Router) ─────────────────────────────────────────┐
 * │ 1. ajustarDataInicialRecorrente(tipoRecorrencia)                     │
 * │    → Router que chama geradores específicos baseado em tipo         │
 * │    → param tipoRecorrencia: string ("D"/"S"/"Q"/"M"/"V")            │
 * │    → returns Array<string>|null: datas YYYY-MM-DD ou null erro      │
 * │    → Fluxo: (63 linhas)                                             │
 * │      1. Criar array vazio datas = []                                │
 * │      2. Se tipoRecorrencia == "V" (Variado):                        │
 * │         - gerarRecorrenciaVariada(datas)                            │
 * │         - return datas.length > 0 ? datas : null                    │
 * │      3. Obter txtDataInicial.ej2_instances[0].value                 │
 * │      4. Obter txtFinalRecorrencia.ej2_instances[0].value            │
 * │      5. Validar dataAtual && dataFinal (required)                   │
 * │      6. Converter para moment: moment(dataAtual).toISOString().split("T")[0]│
 * │      7. Obter lstDias.ej2_instances[0].value (array dias semana)    │
 * │      8. Se tipoRecorrencia == "M": obter lstDiasMes.value (dias mês)│
 * │      9. Se não "M": mapear dias semana string → indices 0-6:        │
 * │         - { Sunday: 0, Monday: 1, ..., Saturday: 6 }                │
 * │     10. Switch tipoRecorrencia:                                      │
 * │         - "D": gerarRecorrenciaDiaria(dataAtual, dataFinal, datas)  │
 * │         - "M": gerarRecorrenciaMensal(dataAtual, dataFinal, diasSelecionados, datas)│
 * │         - "S"/"Q": gerarRecorrenciaPorPeriodo(tipo, dataAtual, dataFinal, diasIndex, datas)│
 * │     11. return datas.length > 0 ? datas : null                      │
 * │     12. try-catch: Alerta.TratamentoErroComLinha + return null      │
 * │    → Uso típico: modal-viagem-novo.criarAgendamentoNovo → calcular datas│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ GERADORES DE DATAS (4 métodos) ────────────────────────────────────┐
 * │ 2. gerarRecorrenciaVariada(datas)                                    │
 * │    → Gera datas custom de Calendar multiselect                      │
 * │    → param datas: Array (reference, modificado in-place)            │
 * │    → returns void (side effect: datas.push)                         │
 * │    → Fluxo: (43 linhas)                                             │
 * │      1. Obter calDatasSelecionadas.ej2_instances[0]                 │
 * │      2. Validar calendarObj.values.length > 0                       │
 * │      3. Filtrar nulls: .filter(date => date)                        │
 * │      4. Converter para Date: .map(date => new Date(date))           │
 * │      5. Ordenar cronologicamente: .sort((a, b) => a - b)            │
 * │      6. forEach date: datas.push(moment(date).format("YYYY-MM-DD")) │
 * │      7. console.log "Array de datas gerado"                         │
 * │      8. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: ajustarDataInicialRecorrente("V")                  │
 * │                                                                       │
 * │ 3. gerarRecorrenciaMensal(dataAtual, dataFinal, diasSelecionados, datas)│
 * │    → Gera datas mensais (dias específicos do mês)                   │
 * │    → param dataAtual: string YYYY-MM-DD (início)                    │
 * │    → param dataFinal: string YYYY-MM-DD (fim)                       │
 * │    → param diasSelecionados: int[] (dias mês: 1-31)                 │
 * │    → param datas: Array (reference)                                 │
 * │    → returns void (side effect: datas.push)                         │
 * │    → Fluxo: (37 linhas)                                             │
 * │      1. dataAtual = moment(dataAtual), dataFinal = moment(dataFinal)│
 * │      2. while dataAtual.isSameOrBefore(dataFinal):                  │
 * │         a. mesAtual = dataAtual.month(), anoAtual = dataAtual.year()│
 * │         b. forEach diaDoMes in diasSelecionados:                    │
 * │            - dataEspecifica = moment([anoAtual, mesAtual, diaDoMes])│
 * │            - Validar: isValid() && month() === mesAtual &&          │
 * │              isSameOrAfter(dataAtual) && isSameOrBefore(dataFinal)  │
 * │            - Se válida e não duplicada: datas.push(format("YYYY-MM-DD"))│
 * │         c. dataAtual.add(1, "month").startOf("month") (próx mês)    │
 * │      3. datas.sort((a, b) => moment(a).diff(moment(b))) (ordenar)   │
 * │      4. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Exemplo: dias [5, 15, 25] → gera dia 5, 15 e 25 de cada mês   │
 * │                                                                       │
 * │ 4. gerarRecorrenciaDiaria(dataAtual, dataFinal, datas)              │
 * │    → Gera datas diárias (apenas dias úteis: seg-sex)                │
 * │    → param dataAtual: string YYYY-MM-DD                             │
 * │    → param dataFinal: string YYYY-MM-DD                             │
 * │    → param datas: Array (reference)                                 │
 * │    → returns void (side effect: datas.push)                         │
 * │    → Fluxo: (20 linhas)                                             │
 * │      1. dataAtual = moment(dataAtual), dataFinal = moment(dataFinal)│
 * │      2. while dataAtual.isSameOrBefore(dataFinal):                  │
 * │         a. dayOfWeek = dataAtual.isoWeekday() (1=seg, 7=dom)        │
 * │         b. Se dayOfWeek >= 1 && <= 5 (seg-sex):                     │
 * │            - datas.push(dataAtual.format("YYYY-MM-DD"))             │
 * │         c. dataAtual.add(1, "days") (próximo dia)                   │
 * │      3. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Nota: ignora fins de semana (sábado/domingo)                   │
 * │                                                                       │
 * │ 5. gerarRecorrenciaPorPeriodo(tipoRecorrencia, dataAtual, dataFinal,│
 * │                               diasSelecionadosIndex, datas)         │
 * │    → Gera datas semanais ou quinzenais (dias específicos da semana) │
 * │    → param tipoRecorrencia: string ("S"=Semanal, "Q"=Quinzenal)     │
 * │    → param dataAtual: string YYYY-MM-DD                             │
 * │    → param dataFinal: string YYYY-MM-DD                             │
 * │    → param diasSelecionadosIndex: int[] (0-6: dom-sáb)              │
 * │    → param datas: Array (reference)                                 │
 * │    → returns void (side effect: datas.push)                         │
 * │    → Fluxo: (43 linhas)                                             │
 * │      1. dataAtual = moment(dataAtual), dataFinal = moment(dataFinal)│
 * │      2. Se tipoRecorrencia == "Q": dataAtual.day(8) (ajuste quinzenal)│
 * │      3. while dataAtual.isSameOrBefore(dataFinal):                  │
 * │         a. forEach diaSelecionado in diasSelecionadosIndex:         │
 * │            - proximaData = moment(dataAtual).day(diaSelecionado)    │
 * │            - Se proximaData.isBefore(dataAtual):                    │
 * │              proximaData.add(1, "week")                             │
 * │            - Se proximaData <= dataFinal && não duplicada:          │
 * │              datas.push(proximaData.format("YYYY-MM-DD"))           │
 * │         b. Switch tipoRecorrencia:                                  │
 * │            - "S": dataAtual.add(1, "week")                          │
 * │            - "Q": dataAtual.add(2, "weeks")                         │
 * │            - default: console.error + return                        │
 * │         c. Se dataAtual > dataFinal: break                          │
 * │      4. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Exemplo Semanal: dias [1, 3, 5] (seg, qua, sex) → toda semana │
 * │    → Exemplo Quinzenal: dias [1, 3] → segunda e quarta, a cada 2 semanas│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ PROCESSAMENTO E ENVIO EM LOTE ─────────────────────────────────────┐
 * │ 6. async handleRecurrence(periodoRecorrente, datasRecorrentes)      │
 * │    → Processa recorrência: loop POST para cada data                 │
 * │    → param periodoRecorrente: string (obsoleto, não usado)          │
 * │    → param datasRecorrentes: Array<string> YYYY-MM-DD               │
 * │    → returns Promise<Object>: { sucesso, totalCriados, totalFalhas, │
 * │      parcial }                                                       │
 * │    → Fluxo: (169 linhas - MÉTODO MAIS COMPLEXO)                     │
 * │      1. Validar datasRecorrentes.length > 0                         │
 * │      2. console.log "Processando N agendamento(s)"                  │
 * │      3. PRIMEIRO AGENDAMENTO:                                       │
 * │         a. primeiroAgendamento = criarAgendamento(null, null,       │
 * │            datasRecorrentes[0])                                     │
 * │         b. primeiroAgendamento.RecorrenciaViagemId = "00000000-..." │
 * │            (GUID vazio para primeiro)                               │
 * │         c. agendamentoObj = await enviarNovoAgendamento(primeiro, false)│
 * │         d. Capturar ViagemId da resposta (múltiplas tentativas):    │
 * │            - agendamentoObj.novaViagem?.viagemId                    │
 * │            - agendamentoObj.viagemId                                │
 * │            - agendamentoObj.data?.viagemId                          │
 * │            - agendamentoObj.data (direto)                           │
 * │            - agendamentoObj.id                                      │
 * │            - agendamentoObj.data.value?.viagemId (nested)           │
 * │         e. recorrenciaViagemId = ViagemId capturado ou GUID vazio   │
 * │         f. Se captura falhou: console.error estrutura + warn        │
 * │         g. console.log "Primeiro agendamento criado"                │
 * │      4. AGENDAMENTOS SUBSEQUENTES (se datasRecorrentes.length > 1): │
 * │         a. console.log "Criando N-1 subsequentes"                   │
 * │         b. for (i=1; i < datasRecorrentes.length; i++):             │
 * │            - dataAtual = datasRecorrentes[i]                        │
 * │            - agendamentoSubsequente = criarAgendamento(null,        │
 * │              recorrenciaViagemId, dataAtual)                        │
 * │            - await enviarNovoAgendamento(agendamentoSubsequente, false)│
 * │            - sucessos++ ou falhas++                                 │
 * │            - console.log progresso                                  │
 * │         c. console.log "Resultado: X/N agendamentos criados"        │
 * │      5. Se falhas > 0:                                              │
 * │         - AppToast.show("Amarelo", "X de N criados, Y falharam")    │
 * │         - return { sucesso: true, totalCriados: X, totalFalhas: Y,  │
 * │           parcial: true }                                           │
 * │      6. Senão (sucesso total):                                      │
 * │         - console.log "Todos criados"                               │
 * │         - return { sucesso: true, totalCriados: N, totalFalhas: 0,  │
 * │           parcial: false }                                          │
 * │      7. try-catch: Alerta.TratamentoErroComLinha + throw            │
 * │    → Uso típico: recorrencia-init.js btnSalvarRecorrencia click     │
 * │    → Importante: primeiro POST captura ViagemId, subsequentes usam  │
 * │      esse ID como RecorrenciaViagemId (link série)                 │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ UTILITÁRIOS ────────────────────────────────────────────────────────┐
 * │ 7. atualizarCalendarioExistente(datas)                               │
 * │    → Atualiza Calendar para modo readonly (edição visualização)     │
 * │    → param datas: Array<string> YYYY-MM-DD                          │
 * │    → returns void (side effect: Calendar readonly)                  │
 * │    → Fluxo: (24 linhas)                                             │
 * │      1. selectedDates = datas.map(data => new Date(data))           │
 * │      2. calendarObj = getElementById("calDatasSelecionadas").ej2_instances[0]│
 * │      3. calendarObj.values = selectedDates (preencher)              │
 * │      4. calendarObj.refresh()                                       │
 * │      5. calendarObj.isMultiSelection = false (desabilitar multi)    │
 * │      6. readOnlyElement = getElementById('readOnly-checkbox')       │
 * │      7. Se exists:                                                   │
 * │         - readOnlyElement.checked = true                            │
 * │         - readOnlyElement.disabled = true                           │
 * │      8. console.log "Calendário atualizado para somente leitura"    │
 * │      9. try-catch: Alerta.TratamentoErroComLinha                    │
 * │    → Uso típico: exibe-viagem.js ao carregar agendamento recorrente │
 * │      para visualização                                              │
 * │                                                                       │
 * │ 8. async corrigirRecorrenciaViagemId(viagemId)                      │
 * │    → Corrige RecorrenciaViagemId do primeiro registro (placeholder) │
 * │    → param viagemId: int (ViagemId do primeiro agendamento)         │
 * │    → returns Promise<boolean>                                        │
 * │    → Fluxo: (25 linhas)                                             │
 * │      1. console.log "Corrigindo RecorrenciaViagemId"                │
 * │      2. Construir payload: { ViagemId, RecorrenciaViagemId: ViagemId }│
 * │      3. console.log "Enviando correção"                             │
 * │      4. console.warn "API não implementada"                         │
 * │      5. return true (mock success)                                  │
 * │      6. try-catch: console.error + return false                     │
 * │    → Nota: API backend não implementada, função placeholder         │
 * │    → Objetivo: primeiro registro deve ter RecorrenciaViagemId =     │
 * │      ViagemId (marca como pai da série)                            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EXPORTAÇÕES GLOBAIS (4 wrappers) ──────────────────────────────────┐
 * │ window.gerenciadorRecorrencia = new GerenciadorRecorrencia()        │
 * │   → Singleton global, instância única                               │
 * │                                                                       │
 * │ window.ajustarDataInicialRecorrente(tipoRecorrencia)                 │
 * │   → Wrapper para gerenciadorRecorrencia.ajustarDataInicialRecorrente│
 * │                                                                       │
 * │ window.handleRecurrence(periodoRecorrente, datasRecorrentes)         │
 * │   → Wrapper para gerenciadorRecorrencia.handleRecurrence            │
 * │                                                                       │
 * │ window.atualizarCalendarioExistente(datas)                           │
 * │   → Wrapper para gerenciadorRecorrencia.atualizarCalendarioExistente│
 * │                                                                       │
 * │ window.corrigirRecorrenciaViagemId(viagemId)                         │
 * │   → Wrapper para gerenciadorRecorrencia.corrigirRecorrenciaViagemId │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO 1 - CRIAR RECORRÊNCIA SEMANAL:
 * 1. Usuário preenche form: DataInicial=2026-02-03, DataFinal=2026-03-31
 * 2. Usuário seleciona lstRecorrente="Sim", lstPeriodos="Semanal"
 * 3. Usuário seleciona lstDias=[Monday, Wednesday, Friday]
 * 4. Usuário clica btnSalvar
 * 5. recorrencia-init.js → ajustarDataInicialRecorrente("S")
 * 6. ajustarDataInicialRecorrente → gerarRecorrenciaPorPeriodo("S", ...)
 * 7. gerarRecorrenciaPorPeriodo:
 *    - Mapeia [Monday, Wednesday, Friday] → [1, 3, 5]
 *    - Loop: dataAtual = 2026-02-03, dataFinal = 2026-03-31
 *    - forEach dia [1, 3, 5]: proximaData = dataAtual.day(1/3/5)
 *    - datas = ["2026-02-03", "2026-02-05", "2026-02-07", "2026-02-10", ...]
 *    - dataAtual.add(1, "week") → próxima semana
 *    - Repetir até dataAtual > dataFinal
 * 8. Return datas array (ex: 24 datas de segunda, quarta e sexta)
 * 9. recorrencia-init.js → handleRecurrence(null, datas)
 * 10. handleRecurrence:
 *     - Primeiro: criarAgendamento(null, GUID_VAZIO, datas[0])
 *     - POST /api/Viagem/AdicionarAgendamento
 *     - Capturar ViagemId da resposta (ex: 12345)
 *     - recorrenciaViagemId = 12345
 *     - Loop for i=1 to 23:
 *       - criarAgendamento(null, 12345, datas[i])
 *       - POST /api/Viagem/AdicionarAgendamento (com RecorrenciaViagemId=12345)
 *     - return { sucesso: true, totalCriados: 24, totalFalhas: 0, parcial: false }
 * 11. Toast success "24 agendamentos criados"
 * 12. Modal hide
 *
 * 🔄 FLUXO TÍPICO 2 - CRIAR RECORRÊNCIA MENSAL:
 * 1. Usuário: DataInicial=2026-02-01, DataFinal=2026-12-31
 * 2. lstPeriodos="Mensal", lstDiasMes=[5, 15, 25]
 * 3. ajustarDataInicialRecorrente("M") → gerarRecorrenciaMensal
 * 4. gerarRecorrenciaMensal:
 *    - Loop meses: fevereiro até dezembro (11 meses)
 *    - Para cada mês: forEach dia [5, 15, 25]
 *      - dataEspecifica = moment([2026, 1, 5]) → válida
 *      - dataEspecifica = moment([2026, 1, 15]) → válida
 *      - dataEspecifica = moment([2026, 1, 25]) → válida
 *      - datas.push "2026-02-05", "2026-02-15", "2026-02-25"
 *    - Repetir para março, abril, ..., dezembro
 * 5. Return datas (33 datas: 11 meses × 3 dias)
 * 6. handleRecurrence → POST 33 agendamentos
 *
 * 🔄 FLUXO TÍPICO 3 - CRIAR RECORRÊNCIA VARIADA (Custom Dates):
 * 1. Usuário: lstPeriodos="Dias Variados"
 * 2. Calendar multiselect aparece
 * 3. Usuário seleciona 10 datas específicas no Calendar
 * 4. calDatasSelecionadas.values = [Date1, Date2, ..., Date10]
 * 5. ajustarDataInicialRecorrente("V") → gerarRecorrenciaVariada
 * 6. gerarRecorrenciaVariada:
 *    - calendarObj.values → [Date1, ..., Date10]
 *    - Filter nulls → map to Date → sort cronológico
 *    - forEach: datas.push(moment(date).format("YYYY-MM-DD"))
 * 7. Return datas (10 datas ordenadas)
 * 8. handleRecurrence → POST 10 agendamentos
 *
 * 📌 TIPOS RECORRÊNCIA (5 tipos suportados):
 * - "D" (Diário): todos os dias úteis (seg-sex) entre DataInicial e DataFinal
 *   - Exemplo: 2026-02-03 a 2026-02-28 → ~20 datas (4 semanas × 5 dias)
 * - "S" (Semanal): dias específicos da semana, toda semana
 *   - Exemplo: seg/qua/sex → 3 datas por semana
 * - "Q" (Quinzenal): dias específicos da semana, a cada 2 semanas
 *   - Exemplo: seg/qua → 2 datas a cada 2 semanas
 * - "M" (Mensal): dias específicos do mês, todo mês
 *   - Exemplo: dia 5, 15, 25 → 3 datas por mês
 * - "V" (Variado/Custom): datas específicas selecionadas no Calendar multiselect
 *   - Exemplo: usuário escolhe 10 datas aleatórias
 *
 * 📌 CAPTURA VIAGEMID (handleRecurrence):
 * Problema: backend pode retornar ViagemId em diferentes estruturas
 * Solução: tentar múltiplas propriedades em ordem:
 * 1. agendamentoObj.novaViagem?.viagemId
 * 2. agendamentoObj.viagemId
 * 3. agendamentoObj.data?.viagemId
 * 4. agendamentoObj.data (direto)
 * 5. agendamentoObj.id
 * 6. agendamentoObj.data.value?.viagemId
 * Se todos falharem: usar GUID vazio "00000000-0000-0000-0000-000000000000"
 *
 * 📌 GUID VAZIO (RecorrenciaViagemId):
 * - Primeiro agendamento: RecorrenciaViagemId = "00000000-0000-0000-0000-000000000000"
 * - Subsequentes: RecorrenciaViagemId = ViagemId do primeiro (ex: 12345)
 * - Se captura falhar: todos usam GUID vazio (série não linkada, bug conhecido)
 *
 * 📌 DIAS SEMANA (mapea string → index):
 * - Sunday: 0
 * - Monday: 1
 * - Tuesday: 2
 * - Wednesday: 3
 * - Thursday: 4
 * - Friday: 5
 * - Saturday: 6
 * → lstDias.value retorna strings (ex: ["Monday", "Friday"])
 * → gerarRecorrenciaPorPeriodo mapeia para [1, 5]
 *
 * 📌 MOMENTO.JS METHODS USADOS:
 * - moment(date): criar moment object
 * - .toISOString(): converter para ISO string "2026-02-03T00:00:00.000Z"
 * - .format("YYYY-MM-DD"): formatar como "2026-02-03"
 * - .add(n, "days"/"weeks"/"months"): adicionar período
 * - .isSameOrBefore(date): comparar datas (<=)
 * - .isSameOrAfter(date): comparar datas (>=)
 * - .isBefore(date): comparar datas (<)
 * - .isAfter(date): comparar datas (>)
 * - .isoWeekday(): dia da semana ISO (1=seg, 7=dom)
 * - .day(n): setar/obter dia da semana (0=dom, 6=sáb)
 * - .month(): obter mês (0-11)
 * - .year(): obter ano
 * - .isValid(): validar data
 * - .startOf("day"/"month"): início de período
 * - .diff(date): diferença entre datas
 * - .sort((a, b) => moment(a).diff(moment(b))): ordenar datas
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Classe ES6 (class keyword, não function constructor)
 * - Singleton pattern: window.gerenciadorRecorrencia (única instância)
 * - Todos os métodos da classe são wrappers exportados via window.*
 * - Console.log extensivo facilita debug (pode ser removido em produção)
 * - Try-catch em todos os métodos com Alerta.TratamentoErroComLinha
 * - Arrays datas modificados in-place (pass by reference)
 * - Diário ignora fins de semana (apenas dias úteis)
 * - Mensal valida datas inválidas (ex: 31 em fevereiro → skip)
 * - Variado ordena datas cronologicamente (sort)
 * - Quinzenal usa day(8) adjust (força segunda-feira da segunda semana)
 * - handleRecurrence async: aguarda cada POST sequencialmente (não paralelo)
 * - Sucesso parcial: algumas POSTs falharam mas primeira ok (retorna parcial: true)
 * - Sucesso total: todas POSTs ok (retorna parcial: false)
 * - Falha total: primeira POST falhou → throw error (não continua)
 * - corrigirRecorrenciaViagemId: não implementado (API backend pendente)
 * - atualizarCalendarioExistente: modo readonly para edição/visualização
 * - this.datasSelecionadas: não usado atualmente (placeholder interno)
 *
 * 🔌 VERSÃO: 3.0 (refatorado após Lote 192, adiciona comprehensive header)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */

class GerenciadorRecorrencia
{
    constructor()
    {
        this.datasSelecionadas = [];
    }

    /**
     * Ajusta data inicial para recorrência
     * param {string} tipoRecorrencia - Tipo (D, S, Q, M, V)
     * returns {Array} Array de datas
     */
    ajustarDataInicialRecorrente(tipoRecorrencia)
    {
        try
        {
            const datas = [];

            if (tipoRecorrencia === "V")
            {
                this.gerarRecorrenciaVariada(datas);
                return datas.length > 0 ? datas : null;
            }

            let dataAtual = document.getElementById("txtDataInicial")?.ej2_instances?.[0]?.value;
            const dataFinal = document.getElementById("txtFinalRecorrencia")?.ej2_instances?.[0]?.value;

            if (!dataAtual || !dataFinal)
            {
                console.error("Data Inicial ou Data Final não encontrada.");
                return null;
            }

            dataAtual = moment(dataAtual).toISOString().split("T")[0];
            const dataFinalFormatada = moment(dataFinal).toISOString().split("T")[0];

            let diasSelecionados = document.getElementById("lstDias")?.ej2_instances?.[0]?.value || [];

            if (tipoRecorrencia === "M")
            {
                diasSelecionados = [].concat(document.getElementById("lstDiasMes")?.ej2_instances?.[0]?.value || []);
            }

            let diasSelecionadosIndex = [];
            if (tipoRecorrencia !== "M")
            {
                diasSelecionadosIndex = diasSelecionados.map(dia => ({
                    Sunday: 0,
                    Monday: 1,
                    Tuesday: 2,
                    Wednesday: 3,
                    Thursday: 4,
                    Friday: 5,
                    Saturday: 6
                }[dia]));
            }

            if (tipoRecorrencia === "D")
            {
                this.gerarRecorrenciaDiaria(dataAtual, dataFinalFormatada, datas);
            } else if (tipoRecorrencia === "M")
            {
                this.gerarRecorrenciaMensal(dataAtual, dataFinalFormatada, diasSelecionados, datas);
            } else
            {
                this.gerarRecorrenciaPorPeriodo(tipoRecorrencia, dataAtual, dataFinalFormatada, diasSelecionadosIndex, datas);
            }

            return datas.length > 0 ? datas : null;

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "ajustarDataInicialRecorrente", error);
            return null;
        }
    }

    /**
     * Gera recorrência variada (dias específicos)
     * param {Array} datas - Array para preencher
     */
    gerarRecorrenciaVariada(datas)
    {
        try
        {
            const calendarObj = document.getElementById("calDatasSelecionadas")?.ej2_instances?.[0];

            if (!calendarObj || !calendarObj.values || calendarObj.values.length === 0)
            {
                console.error("Nenhuma data selecionada no calendário para recorrência do tipo 'V'.");
                return;
            }

            console.log("📅 [Variada] Datas selecionadas no calendário:", calendarObj.values);

            // ✅ ORDENAR as datas (da mais antiga para a mais recente)
            const datasOrdenadas = calendarObj.values
                .filter(date => date) // Remover nulls
                .map(date => new Date(date)) // Converter para Date
                .sort((a, b) => a - b); // Ordenar cronologicamente

            console.log("📅 [Variada] Datas ordenadas:", datasOrdenadas);

            // ✅ Adicionar ao array no formato YYYY-MM-DD
            datasOrdenadas.forEach(date =>
            {
                try
                {
                    datas.push(moment(date).format("YYYY-MM-DD"));
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaVariada_forEach", error);
                }
            });

            console.log("✅ [Variada] Array de datas gerado:", datas);

            // ✅ LIMPAR o campo de DatasSelecionadas pois não será mais usado
            // As datas serão enviadas individualmente, uma por requisição
            console.log("🔧 [Variada] Campo DatasSelecionadas será ignorado - enviando datas individualmente");

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaVariada", error);
        }
    }

    /**
     * Gera recorrência mensal
     * param {string} dataAtual - Data inicial
     * param {string} dataFinal - Data final
     * param {Array} diasSelecionados - Dias do mês
     * param {Array} datas - Array para preencher
     */
    gerarRecorrenciaMensal(dataAtual, dataFinal, diasSelecionados, datas)
    {
        try
        {
            dataAtual = moment(dataAtual);
            dataFinal = moment(dataFinal);

            while (dataAtual.isSameOrBefore(dataFinal))
            {
                const mesAtual = dataAtual.month();
                const anoAtual = dataAtual.year();

                diasSelecionados.forEach(diaDoMes =>
                {
                    const dataEspecifica = moment([anoAtual, mesAtual, diaDoMes]);

                    if (dataEspecifica.isValid() &&
                        dataEspecifica.month() === mesAtual &&
                        dataEspecifica.isSameOrAfter(moment(dataAtual).startOf("day")) &&
                        dataEspecifica.isSameOrBefore(dataFinal))
                    {
                        if (!datas.includes(dataEspecifica.format("YYYY-MM-DD")))
                        {
                            datas.push(dataEspecifica.format("YYYY-MM-DD"));
                        }
                    }
                });

                dataAtual.add(1, "month").startOf("month");
            }

            datas.sort((a, b) => moment(a).diff(moment(b)));

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaMensal", error);
        }
    }

    /**
     * Gera recorrência diária
     * param {string} dataAtual - Data inicial
     * param {string} dataFinal - Data final
     * param {Array} datas - Array para preencher
     */
    gerarRecorrenciaDiaria(dataAtual, dataFinal, datas)
    {
        try
        {
            dataAtual = moment(dataAtual);
            dataFinal = moment(dataFinal);

            while (dataAtual.isSameOrBefore(dataFinal))
            {
                const dayOfWeek = dataAtual.isoWeekday();
                if (dayOfWeek >= 1 && dayOfWeek <= 5)
                {
                    datas.push(dataAtual.format("YYYY-MM-DD"));
                }
                dataAtual.add(1, "days");
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaDiaria", error);
        }
    }

    /**
     * Gera recorrência por período (semanal/quinzenal)
     * param {string} tipoRecorrencia - Tipo de recorrência
     * param {string} dataAtual - Data inicial
     * param {string} dataFinal - Data final
     * param {Array} diasSelecionadosIndex - Índices dos dias
     * param {Array} datas - Array para preencher
     */
    gerarRecorrenciaPorPeriodo(tipoRecorrencia, dataAtual, dataFinal, diasSelecionadosIndex, datas)
    {
        try
        {
            dataAtual = moment(dataAtual);
            dataFinal = moment(dataFinal);

            if (tipoRecorrencia === "Q")
            {
                dataAtual = moment(dataAtual).day(8);
            }

            while (dataAtual.isSameOrBefore(dataFinal))
            {
                diasSelecionadosIndex.forEach(diaSelecionado =>
                {
                    let proximaData = moment(dataAtual).day(diaSelecionado);
                    if (proximaData.isBefore(dataAtual)) proximaData.add(1, "week");
                    if (proximaData.isSameOrBefore(dataFinal) && !datas.includes(proximaData.format("YYYY-MM-DD")))
                    {
                        datas.push(proximaData.format("YYYY-MM-DD"));
                    }
                });

                switch (tipoRecorrencia)
                {
                    case "S":
                        dataAtual.add(1, "week");
                        break;
                    case "Q":
                        dataAtual.add(2, "weeks");
                        break;
                    default:
                        console.error("Tipo de recorrência inválido: ", tipoRecorrencia);
                        return;
                }

                if (dataAtual.isAfter(dataFinal)) break;
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "gerarRecorrenciaPorPeriodo", error);
        }
    }

    /**
     * ✅ CORRIGIDO: Processa recorrência com loop para todas as datas
     * Cria um agendamento para cada data no array
     * O primeiro agendamento gera o viagemId que será usado como RecorrenciaViagemId nos demais
     */
    async handleRecurrence(periodoRecorrente, datasRecorrentes)
    {
        try
        {
            if (!datasRecorrentes || datasRecorrentes.length === 0)
            {
                console.error("❌ Nenhuma data inicial válida retornada para o período.");
                AppToast.show("Vermelho", "Erro: Nenhuma data válida para a recorrência", 3000);
                throw new Error("Nenhuma data válida para recorrência");
            }

            console.log(`📅 Processando ${datasRecorrentes.length} agendamento(s) recorrente(s)...`);
            console.log(`📋 Datas a processar:`, datasRecorrentes);

            // ✅ CRIAR PRIMEIRO AGENDAMENTO
            console.log("📤 Criando primeiro agendamento...");
            let primeiroAgendamento = window.criarAgendamento(null, null, datasRecorrentes[0]);

            if (!primeiroAgendamento)
            {
                console.error("❌ criarAgendamento retornou NULL");
                throw new Error("Erro ao criar objeto do primeiro agendamento");
            }

            // ✅ IMPORTANTE: Para o primeiro agendamento, garantir que RecorrenciaViagemId seja null/vazio
            primeiroAgendamento.RecorrenciaViagemId = "00000000-0000-0000-0000-000000000000";

            let agendamentoObj;
            let recorrenciaViagemId = null;

            try
            {
                agendamentoObj = await window.enviarNovoAgendamento(primeiroAgendamento, false);

                // ✅ CORREÇÃO: Verificar diferentes estruturas de resposta possíveis
                if (!agendamentoObj)
                {
                    console.error("❌ Primeiro agendamento falhou - resposta vazia");
                    throw new Error("Erro ao criar o primeiro agendamento.");
                }

                console.log("📦 Resposta completa do primeiro agendamento:", agendamentoObj);

                // ✅ ARMAZENAR O viagemId DO PRIMEIRO AGENDAMENTO
                // A resposta pode vir em diferentes formatos, vamos tentar todos
                recorrenciaViagemId = agendamentoObj.novaViagem?.viagemId ||
                    agendamentoObj.viagemId ||
                    agendamentoObj.data?.viagemId ||
                    agendamentoObj.data ||
                    agendamentoObj.id ||
                    null;

                // Se ainda não encontrou, procurar em propriedades aninhadas
                if (!recorrenciaViagemId || recorrenciaViagemId === "00000000-0000-0000-0000-000000000000")
                {
                    // Tentar buscar em data.value ou result
                    if (agendamentoObj.data && typeof agendamentoObj.data === 'object')
                    {
                        recorrenciaViagemId = agendamentoObj.data.viagemId ||
                            agendamentoObj.data.id ||
                            agendamentoObj.data.value?.viagemId ||
                            null;
                    }
                }

                if (!recorrenciaViagemId || recorrenciaViagemId === "00000000-0000-0000-0000-000000000000")
                {
                    console.error("❌ ViagemId não retornado pela API");
                    console.error("   Estrutura da resposta:", JSON.stringify(agendamentoObj, null, 2));

                    // Mesmo assim, continuar com os demais agendamentos
                    console.warn("⚠️ Continuando sem RecorrenciaViagemId (será gravado como GUID vazio)");
                }

                console.log("✅ Primeiro agendamento criado:");
                console.log("   📅 Data:", datasRecorrentes[0]);
                console.log("   🔑 ViagemId capturado:", recorrenciaViagemId || "NÃO CAPTURADO");
            }
            catch (error)
            {
                console.error("❌ Falha no primeiro agendamento:", error);
                Alerta.TratamentoErroComLinha("recorrencia.js", "handleRecurrence_primeiro", error);
                AppToast.show("Vermelho", "Erro ao criar o primeiro agendamento", 3000);
                throw error;
            }

            // ✅ CRIAR AGENDAMENTOS SUBSEQUENTES
            if (datasRecorrentes.length > 1)
            {
                console.log(`📤 Criando ${datasRecorrentes.length - 1} agendamento(s) subsequente(s)...`);
                console.log(`🔗 Usando RecorrenciaViagemId: ${recorrenciaViagemId || "GUID VAZIO"}`);

                let sucessos = 0;
                let falhas = 0;

                // ✅ LOOP POR TODAS AS DATAS RESTANTES
                for (let i = 1; i < datasRecorrentes.length; i++)
                {
                    const dataAtual = datasRecorrentes[i];

                    console.log(`\n   📤 Criando agendamento ${i}/${datasRecorrentes.length - 1}...`);
                    console.log(`      📅 Data: ${dataAtual}`);
                    console.log(`      🔗 RecorrenciaViagemId: ${recorrenciaViagemId || "00000000-0000-0000-0000-000000000000"}`);

                    // ✅ CRIAR AGENDAMENTO SUBSEQUENTE COM RecorrenciaViagemId
                    const agendamentoSubsequente = window.criarAgendamento(
                        null,                    // viagemId = null (novo)
                        recorrenciaViagemId || "00000000-0000-0000-0000-000000000000",    // RecorrenciaViagemId do primeiro
                        dataAtual               // Data específica
                    );

                    if (!agendamentoSubsequente)
                    {
                        falhas++;
                        console.error(`   ❌ Falha ao criar objeto do agendamento ${i}`);
                        continue;
                    }

                    try
                    {
                        await window.enviarNovoAgendamento(
                            agendamentoSubsequente,
                            false // ❌ NÃO MOSTRAR TOAST INDIVIDUAL
                        );

                        sucessos++;
                        console.log(`   ✅ Agendamento ${i} criado com sucesso`);
                    }
                    catch (error)
                    {
                        falhas++;
                        console.error(`   ❌ Falha no agendamento ${i}:`, error);
                        Alerta.TratamentoErroComLinha("recorrencia.js", "handleRecurrence_subsequente", error);
                    }
                }

                console.log(`\n📊 Resultado: ${sucessos + 1}/${datasRecorrentes.length} agendamentos criados`);

                if (falhas > 0)
                {
                    const mensagem = `${sucessos + 1} de ${datasRecorrentes.length} agendamentos criados. ${falhas} falharam.`;
                    console.warn("⚠️", mensagem);
                    AppToast.show("Amarelo", mensagem, 5000);

                    return {
                        sucesso: true,
                        totalCriados: sucessos + 1,
                        totalFalhas: falhas,
                        parcial: true
                    };
                }
            }

            // ✅ SUCESSO TOTAL
            console.log("✅ Todos os agendamentos foram criados!");
            return {
                sucesso: true,
                totalCriados: datasRecorrentes.length,
                totalFalhas: 0,
                parcial: false
            };

        }
        catch (error)
        {
            console.error("❌ Erro geral em handleRecurrence:", error);
            Alerta.TratamentoErroComLinha("recorrencia.js", "handleRecurrence", error);
            throw error;
        }
    }

    /**
     * Atualiza calendário com datas existentes
     */
    atualizarCalendarioExistente(datas)
    {
        try
        {
            const selectedDates = datas.map(data => new Date(data));
            const calendarObj = document.getElementById("calDatasSelecionadas").ej2_instances[0];

            calendarObj.values = selectedDates;
            calendarObj.refresh();
            calendarObj.isMultiSelection = false;

            const readOnlyElement = document.getElementById('readOnly-checkbox');
            if (readOnlyElement)
            {
                readOnlyElement.checked = true;
                readOnlyElement.disabled = true;
            }

            console.log("Calendário atualizado para modo somente leitura.");
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("recorrencia.js", "atualizarCalendarioExistente", error);
        }
    }

    /**
     * ✅ FUNÇÃO ADICIONAL: Verifica e corrige RecorrenciaViagemId
     * Garante que o primeiro registro tenha RecorrenciaViagemId = ViagemId
     */
    async corrigirRecorrenciaViagemId(viagemId)
    {
        try
        {
            console.log("🔧 Corrigindo RecorrenciaViagemId do primeiro registro...");

            // Fazer uma chamada para atualizar o primeiro registro
            // onde RecorrenciaViagemId = ViagemId (marca como pai da recorrência)
            const payload = {
                ViagemId: viagemId,
                RecorrenciaViagemId: viagemId
            };

            console.log("📤 Enviando correção:", payload);

            // Esta função precisaria de uma API específica no backend
            // Por enquanto, apenas logamos o que deveria ser feito
            console.warn("⚠️ API de correção não implementada - o backend deve atualizar RecorrenciaViagemId = ViagemId para o primeiro registro");

            return true;
        }
        catch (error)
        {
            console.error("❌ Erro ao corrigir RecorrenciaViagemId:", error);
            return false;
        }
    }
}

// ====================================================================
// INICIALIZAÇÃO E EXPORTAÇÃO
// ====================================================================

// Criar instância global
window.gerenciadorRecorrencia = new GerenciadorRecorrencia();

// Exportar funções para uso global
window.ajustarDataInicialRecorrente = function (tipoRecorrencia)
{
    return window.gerenciadorRecorrencia.ajustarDataInicialRecorrente(tipoRecorrencia);
};

window.handleRecurrence = function (periodoRecorrente, datasRecorrentes)
{
    return window.gerenciadorRecorrencia.handleRecurrence(periodoRecorrente, datasRecorrentes);
};

window.atualizarCalendarioExistente = function (datas)
{
    return window.gerenciadorRecorrencia.atualizarCalendarioExistente(datas);
};

window.corrigirRecorrenciaViagemId = function (viagemId)
{
    return window.gerenciadorRecorrencia.corrigirRecorrenciaViagemId(viagemId);
};

console.log("✅ GerenciadorRecorrencia inicializado");
