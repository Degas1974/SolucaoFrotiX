/* ****************************************************************************************
 * ⚡ ARQUIVO: modal-viagem-novo.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento completo do modal Bootstrap de agendamento de viagens.
 *                   28 funções para criar objetos agendamento (novo, edição, alteração
 *                   data), enviar dados via API (/api/Viagem endpoints), editar
 *                   agendamentos únicos/recorrentes, controlar Telerik ReportViewer,
 *                   inicializar/limpar campos Syncfusion (DateTimePicker, DropDownList,
 *                   RichTextEditor) e Kendo (ComboBox), desabilitar controles em modo
 *                   visualização. Integração com StateManager, Bootstrap Modal events,
 *                   RecorrenciaLogic para datas push/pull. Principais fluxos: criar novo
 *                   agendamento (criarAgendamentoNovo → enviarNovoAgendamento → POST),
 *                   editar único (editarAgendamento → aplicarAtualizacao → PUT), editar
 *                   recorrente (editarAgendamentoRecorrente → enviarAgendamentoComOpcao
 *                   → POST/PUT múltiplos), cancelar (cancelarAgendamento → PUT Status),
 *                   carregar relatório (carregarRelatorioNoModal → Telerik instance).
 * 📥 ENTRADAS     : ViagemId (int de URL ou StateManager), agendamento objects (Object
 *                   com 40+ props: ViagemId, DataInicial, DataFinal, MotoristaId,
 *                   VeiculoId, Descricao, Origem, Destino, etc.), editaTodos/editarProximos
 *                   (boolean para recorrência), dataInicial (Date para push), descricao
 *                   (string para cancelamento). Inputs via DOM: 16+ Syncfusion/Kendo
 *                   components (txtDataInicial, lstMotorista, rteDescricao, etc.)
 * 📤 SAÍDAS       : Promises resolvidas (POST/PUT success), objects (agendamento criado),
 *                   void (side effects: DOM updates, modal show/hide, StateManager.set,
 *                   toasts Swal.fire), ReportViewer instance (Telerik). Error handling:
 *                   handleAgendamentoError → Alerta.MostrarMensagemErro + TratamentoErroComLinha
 * 🔗 CHAMADA POR  : main.js (Bootstrap Modal events: shown.bs.modal → aoAbrirModalViagem,
 *                   hidden.bs.modal → aoFecharModalViagem), calendario.js (click event
 *                   → criarAgendamento/editarAgendamento via ViagemId), exibe-viagem.js
 *                   (botões Editar/Cancelar → editarAgendamento/cancelarAgendamento),
 *                   recorrencia.js (btnSalvarRecorrencia click → enviarAgendamento),
 *                   relatorio.js (btnVisualizarRelatorio click → carregarRelatorioNoModal)
 * 🔄 CHAMA        : ApiClient.post/put (6 endpoints: AdicionarAgendamento, AtualizarViagem,
 *                   PegarViagemParaEdicao, CancelarAgendamento, PegarRecorrenciaViagem,
 *                   AlterarRecorrenciaViagem), StateManager.get/set (viagemId, ehEdicao,
 *                   ehRecorrente, modoCancelamento, etc.), RecorrenciaLogic.calcularDatasRecorrencia
 *                   (para push/pull datas), ModalConfig.setModalTitle/resetModal,
 *                   Alerta.TratamentoErroComLinha, Swal.fire (success toasts),
 *                   limparCamposModalViagens/inicializarCamposModal (campo reset),
 *                   detectarAlteracaoDataInicial/calcularPushDatas (data diff), Telerik
 *                   ReportViewer constructor + renderingEnd event, Bootstrap Modal API
 *                   ($.modal('show'/'hide'), shown.bs.modal/hidden.bs.modal events),
 *                   Syncfusion EJ2 instances (refresh/dataBind/destroy/appendTo methods),
 *                   RecorrenciaUI.esconder/mostrar (UI toggle)
 * 📦 DEPENDÊNCIAS : Bootstrap 5 Modal (#modalViagens, #modalRelatorio), Syncfusion EJ2
 *                   Calendars (DateTimePicker: txtDataInicial/txtDataFinal/txtFinalRecorrencia),
 *                   Syncfusion DropDownList (lstMotorista, lstVeiculo, lstFinalidade,
 *                   lstSetorRequisitanteAgendamento, lstRecorrente, lstPeriodos, lstDias,
 *                   lstDiasMes, lstEventos), Syncfusion RichTextEditor (rteDescricao),
 *                   Syncfusion NumericTextBox (ddtCombustivelInicial, ddtCombustivelFinal),
 *                   Syncfusion Calendar (calDatasSelecionadas), Kendo UI ComboBox
 *                   (lstRequisitante via kendoComboBox), Telerik ReportViewer (window.telerikReportViewer,
 *                   instance com reportSource/serviceUrl), jQuery ($.ajax wrapper via
 *                   ApiClient, $(element).data('kendoComboBox'), $.modal), StateManager
 *                   (agendamento module state), RecorrenciaLogic (calcularDatasRecorrencia,
 *                   verificarDatasSaoIguais), ModalConfig (modal title/reset), Alerta
 *                   (error handling), Swal (toasts), RecorrenciaUI (show/hide logic),
 *                   DOM elements (16 form inputs, modal containers, buttons)
 * 📝 OBSERVAÇÕES  : Arquivo principal do módulo agendamento (2874 linhas, 28 funções).
 *                   Estrutura em 7 seções: (1) Criação objetos, (2) Envio API, (3)
 *                   Edição, (4) Alteração data inicial, (5) Relatório, (6) Inicialização,
 *                   (7) Controle estado. Global variables: modalJaFoiLimpo (boolean
 *                   flag para evitar limpeza dupla), telerikReportViewer (Telerik instance),
 *                   isReportViewerLoading (boolean), ultimoViagemIdCarregado (int cache).
 *                   Todas as funções exportadas via window.* (28 exports). Try-catch
 *                   completo em todas as funções async com Alerta.TratamentoErroComLinha.
 *                   Recorrência: suporta 3 tipos (Semanal, Mensal, Custom) com logic
 *                   para editar todos/próximos (POST batch) ou único (PUT). Data push:
 *                   detecta mudança DataInicial e propaga para DataFinal + datas recorrência
 *                   (calcularPushDatas). ReportViewer: lazy loading (carregarRelatorioNoModal
 *                   → new telerikReportViewer só se necessário), renderingEnd event
 *                   para cleanup. Bootstrap Modal: aoAbrirModalViagem configura título
 *                   (Criar/Editar/Visualizar/Cancelar) + carrega dados se ehEdicao,
 *                   aoFecharModalViagem limpa campos + reseta flags. Desabilitar controles:
 *                   desabilitarTodosControles em modo visualização (Status != Aberta),
 *                   protege 5 botões fechar (btnFechar, btnCancelar, modal-footer buttons,
 *                   btn-close). Validações: campos obrigatórios verificados no backend
 *                   (API retorna errors array). Timestamps: DataInicial/DataFinal como
 *                   ISO strings (new Date().toISOString()). Combustível: NumericTextBox
 *                   format="n0" (0 decimais). Setor: carregado via GET AJAXPreencheListaSetores.
 *                   Eventos: DropDownList para eventos pré-cadastrados (via EventoService).
 *
 * 📋 ÍNDICE DE FUNÇÕES (28 funções + 4 global variables + 3 event handlers):
 *
 * ┌─ GLOBAL VARIABLES ───────────────────────────────────────────────────┐
 * │ 1. window.modalJaFoiLimpo = false                                    │
 * │    → Boolean flag para controlar limpeza dupla do modal             │
 * │    → Setado true em aoFecharModalViagem, resetado false em limpar   │
 * │                                                                       │
 * │ 2. window.telerikReportViewer = null                                 │
 * │    → Telerik ReportViewer instance (lazy initialized)               │
 * │    → Criado em carregarRelatorioNoModal se null                     │
 * │                                                                       │
 * │ 3. window.isReportViewerLoading = false                              │
 * │    → Boolean flag para evitar múltiplos carregamentos simultâneos   │
 * │    → True durante carregarRelatorioNoModal, false após renderingEnd │
 * │                                                                       │
 * │ 4. window.ultimoViagemIdCarregado = null                             │
 * │    → Cache do último ViagemId carregado no ReportViewer             │
 * │    → Evita reload desnecessário se mesmo ID                         │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 1: CRIAÇÃO DE OBJETOS ───────────────────────────────────────┐
 * │ 5. window.refreshComponenteSafe(elementId)                           │
 * │    → Refresh seguro de componentes Syncfusion (evita erros)         │
 * │    → param elementId: string, ID do elemento DOM                    │
 * │    → returns boolean: true se refresh ok, false se não encontrado   │
 * │    → Fluxo: getElementById → ej2_instances[0] → refresh() ou        │
 * │      dataBind() → try-catch com console.warn                        │
 * │                                                                       │
 * │ 6. window.criarAgendamentoNovo()                                     │
 * │    → Cria objeto agendamento NOVO lendo todos os campos do form    │
 * │    → returns Object|null: agendamento com 40+ props ou null erro    │
 * │    → Fluxo: (218 linhas)                                            │
 * │      1. Obter 16 instâncias Syncfusion/Kendo (txtDataInicial,      │
 * │         lstMotorista, rteDescricao, lstRequisitante, etc.)          │
 * │      2. Validar requisitante (required)                             │
 * │      3. Construir objeto com props:                                 │
 * │         - Timestamps: DataInicial, DataFinal (ISO strings)          │
 * │         - IDs: MotoristaId, VeiculoId, SetorId, FinalidadeId       │
 * │         - Strings: Descricao (HTML), Origem, Destino               │
 * │         - Numbers: CombustivelInicial, CombustivelFinal            │
 * │         - Recorrência: EhRecorrente, TipoRecorrencia, etc.         │
 * │      4. Recorrência logic:                                          │
 * │         - Se lstRecorrente != "Não": adicionar props recorrência   │
 * │         - TipoRecorrencia: "Semanal", "Mensal", "Custom"           │
 * │         - Semanal: DiasSemana array (0-6)                           │
 * │         - Mensal: DiasMes array (1-31)                              │
 * │         - Custom: DatasSelecionadas array (ISO strings)             │
 * │         - QuantidadePeriodos: número de repetições ou null          │
 * │         - FinalRecorrencia: data limite ou null                     │
 * │      5. Eventos: EventoId (int) e NomeEvento (string)               │
 * │      6. Console.log resultado + return objeto                       │
 * │    → Uso típico: chamado por criarAgendamento/enviarAgendamento     │
 * │                                                                       │
 * │ 7. window.criarAgendamento(viagemId, viagemIdRecorrente, dataInicial)│
 * │    → Wrapper que chama criarAgendamentoNovo e adiciona IDs          │
 * │    → param viagemId: int opcional (para edição)                     │
 * │    → param viagemIdRecorrente: int opcional (ID grupo recorrência)  │
 * │    → param dataInicial: Date opcional (para alteração data)         │
 * │    → returns Object|null: agendamento com IDs adicionados           │
 * │    → Fluxo:                                                          │
 * │      1. Call criarAgendamentoNovo()                                 │
 * │      2. Se agendamento ok:                                          │
 * │         - Adicionar agendamento.ViagemId = viagemId || null         │
 * │         - Adicionar agendamento.RecorrenciaViagemId = viagemIdRecorrente│
 * │      3. Se dataInicial: agendamento.DataInicial = dataInicial.toISOString()│
 * │      4. Return agendamento                                          │
 * │    → Uso típico: calendario.js click handler                        │
 * │                                                                       │
 * │ 8. window.criarAgendamentoEdicao(agendamentoOriginal)               │
 * │    → Cria objeto para EDIÇÃO comparando original vs form atual      │
 * │    → param agendamentoOriginal: Object (dados do backend)           │
 * │    → returns Object: agendamento com alterações detectadas          │
 * │    → Fluxo: (198 linhas)                                            │
 * │      1. Call criarAgendamentoNovo() para ler form atual             │
 * │      2. Detectar alteração DataInicial:                             │
 * │         - detectarAlteracaoDataInicial(agendamentoOriginal)         │
 * │         - Se mudou: calcularPushDatas para propagar mudança         │
 * │      3. Manter props imutáveis do original:                         │
 * │         - ViagemId, RecorrenciaViagemId, Status                     │
 * │      4. Merge agendamentoAtual + agendamentoOriginal (spread)       │
 * │      5. Se houve push datas: aplicar novas datas                    │
 * │      6. Return objeto merged                                        │
 * │    → Uso típico: editarAgendamento após recuperarViagemEdicao       │
 * │                                                                       │
 * │ 9. window.criarAgendamentoViagem(agendamentoUnicoAlterado)          │
 * │    → Cria objeto para PUT de único agendamento em série recorrente  │
 * │    → param agendamentoUnicoAlterado: Object (dados editados)        │
 * │    → returns Object: payload para AlterarRecorrenciaViagem endpoint │
 * │    → Fluxo: (157 linhas)                                            │
 * │      1. Extrair agendamentoOriginal (do StateManager cache)         │
 * │      2. Construir objeto com estrutura específica API:              │
 * │         - AgendamentoUnicoAlterado: { 40+ props }                   │
 * │         - AgendamentoOriginal: { props originais }                  │
 * │         - DataInicial: timestamp novo                               │
 * │         - RecorrenciaViagemId: ID do grupo                          │
 * │      3. Validar campos obrigatórios (Requisitante, DataInicial)     │
 * │      4. Return objeto                                               │
 * │    → Endpoint: POST /api/Viagem/AlterarRecorrenciaViagem            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 2: ENVIO E COMUNICAÇÃO COM API ──────────────────────────────┐
 * │ 10. window.enviarAgendamento(agendamento)                            │
 * │     → Router: decide entre enviarNovoAgendamento ou aplicarAtualizacao│
 * │     → param agendamento: Object (criado por criarAgendamentoNovo)   │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. Verificar StateManager.get("ehEdicao")                     │
 * │       2. Se true: call aplicarAtualizacao(agendamento)              │
 * │       3. Se false: call enviarNovoAgendamento(agendamento)          │
 * │     → Uso típico: recorrencia.js btnSalvarRecorrencia click         │
 * │                                                                       │
 * │ 11. window.enviarNovoAgendamento(agendamento, isUltimoAgendamento)  │
 * │     → POST novo agendamento (único ou recorrente)                   │
 * │     → param agendamento: Object (payload)                           │
 * │     → param isUltimoAgendamento: boolean (default true, para toast) │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. ApiClient.post("/api/Viagem/AdicionarAgendamento", agendamento)│
 * │       2. Se success:                                                 │
 * │          - Se isUltimoAgendamento: exibirMensagemSucesso()          │
 * │          - Se EhRecorrente: success toast "Agendamentos criados"    │
 * │          - Se único: success toast "Agendamento criado"             │
 * │          - Fechar modal: $("#modalViagens").modal("hide")           │
 * │       3. catch: handleAgendamentoError(error)                       │
 * │     → Endpoint: POST /api/Viagem/AdicionarAgendamento               │
 * │                                                                       │
 * │ 12. window.enviarAgendamentoComOpcao(viagemId, editarTodos,         │
 * │                  editarProximos, dataInicial, viagemIdRecorrente)   │
 * │     → Envia batch de agendamentos recorrentes (editar todos/próximos)│
 * │     → param viagemId: int (ID do agendamento clicado)               │
 * │     → param editarTodos: boolean (editar toda a série)              │
 * │     → param editarProximos: boolean (editar este + futuros)         │
 * │     → param dataInicial: Date opcional (para push)                  │
 * │     → param viagemIdRecorrente: int (ID do grupo)                   │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. obterAgendamentosRecorrentes(viagemIdRecorrente)           │
 * │       2. Filtrar agendamentos por critério:                         │
 * │          - editarTodos: todos os agendamentos                       │
 * │          - editarProximos: DataInicial >= dataInicialClicada        │
 * │       3. Para cada agendamento filtrado:                            │
 * │          - criarAgendamentoEdicao(agendamento)                      │
 * │          - enviarNovoAgendamento(agendamentoEditado, isLast)        │
 * │       4. Toast final se isLast                                      │
 * │     → Uso típico: editarAgendamentoRecorrente após Swal.fire choice │
 * │                                                                       │
 * │ 13. window.aplicarAtualizacao(objViagem)                             │
 * │     → PUT atualização de agendamento único                          │
 * │     → param objViagem: Object (payload editado)                     │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. ApiClient.put("/api/Viagem/AtualizarViagem", objViagem)    │
 * │       2. Se success:                                                 │
 * │          - exibirMensagemSucesso()                                  │
 * │          - $("#modalViagens").modal("hide")                         │
 * │       3. catch: handleAgendamentoError(error)                       │
 * │     → Endpoint: PUT /api/Viagem/AtualizarViagem                     │
 * │                                                                       │
 * │ 14. window.recuperarViagemEdicao(viagemId)                           │
 * │     → GET dados de agendamento para edição                          │
 * │     → param viagemId: int (ID do agendamento)                       │
 * │     → returns Promise<Object>: dados do backend                      │
 * │     → Fluxo:                                                         │
 * │       1. ApiClient.get("/api/Viagem/PegarViagemParaEdicao", { viagemId })│
 * │       2. Return response data                                       │
 * │       3. catch: Alerta.TratamentoErroComLinha + throw               │
 * │     → Endpoint: GET /api/Viagem/PegarViagemParaEdicao               │
 * │                                                                       │
 * │ 15. window.obterAgendamentosRecorrentes(recorrenciaViagemId)        │
 * │     → GET todos os agendamentos de um grupo recorrente              │
 * │     → param recorrenciaViagemId: int (ID do grupo)                  │
 * │     → returns Promise<Array>: lista de agendamentos                  │
 * │     → Fluxo:                                                         │
 * │       1. ApiClient.get("/api/Viagem/PegarRecorrenciaViagem",        │
 * │          { recorrenciaViagemId })                                   │
 * │       2. Return response.data (array)                               │
 * │       3. catch: Alerta.TratamentoErroComLinha + throw               │
 * │     → Endpoint: GET /api/Viagem/PegarRecorrenciaViagem              │
 * │                                                                       │
 * │ 16. window.obterAgendamentosRecorrenteInicial(viagemId)             │
 * │     → GET grupo recorrente a partir de um único ViagemId            │
 * │     → param viagemId: int (qualquer ID da série)                    │
 * │     → returns Promise<Object>: { recorrenciaViagemId, agendamentos }│
 * │     → Fluxo:                                                         │
 * │       1. recuperarViagemEdicao(viagemId)                            │
 * │       2. Extrair RecorrenciaViagemId                                │
 * │       3. obterAgendamentosRecorrentes(RecorrenciaViagemId)          │
 * │       4. Return { recorrenciaViagemId, agendamentos }               │
 * │     → Uso típico: editar recorrente → precisa carregar toda série  │
 * │                                                                       │
 * │ 17. window.excluirAgendamento(viagemId)                              │
 * │     → DELETE agendamento (implementação futura)                     │
 * │     → param viagemId: int                                           │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo: console.log "Excluir agendamento" + TODO               │
 * │     → Status: não implementado (placeholder)                        │
 * │                                                                       │
 * │ 18. window.cancelarAgendamento(viagemId, descricao, mostrarToast)   │
 * │     → PUT para cancelar agendamento (Status → Cancelada)            │
 * │     → param viagemId: int                                           │
 * │     → param descricao: string (motivo cancelamento)                 │
 * │     → param mostrarToast: boolean (default true)                    │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. ApiClient.put("/api/Viagem/CancelarAgendamento",           │
 * │          { viagemId, descricao })                                   │
 * │       2. Se success + mostrarToast:                                 │
 * │          - Swal.fire success "Agendamento cancelado"                │
 * │          - $("#modalViagens").modal("hide")                         │
 * │       3. catch: handleAgendamentoError(error)                       │
 * │     → Endpoint: PUT /api/Viagem/CancelarAgendamento                 │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 4: ALTERAÇÃO DE DATA INICIAL (Push/Pull Logic) ─────────────┐
 * │ 19. detectarAlteracaoDataInicial(agendamentoOriginal)                │
 * │     → Detecta se DataInicial mudou comparando original vs atual     │
 * │     → param agendamentoOriginal: Object (dados backend)             │
 * │     → returns Object|null: { dataOriginal, dataNova, houveMudanca } │
 * │     → Fluxo:                                                         │
 * │       1. Obter instância txtDataInicial (Syncfusion DateTimePicker) │
 * │       2. Extrair agendamentoOriginal.DataInicial (ISO string)       │
 * │       3. Converter ambos para Date objects                          │
 * │       4. Comparar timestamps (getTime())                            │
 * │       5. Return { dataOriginal: Date, dataNova: Date,               │
 * │          houveMudanca: boolean }                                    │
 * │     → Uso típico: criarAgendamentoEdicao → detectar push            │
 * │                                                                       │
 * │ 20. calcularPushDatas(dataOriginal, dataNova, intervalo)            │
 * │     → Calcula push de DataFinal + datas recorrência após mudança    │
 * │     → param dataOriginal: Date (DataInicial antiga)                 │
 * │     → param dataNova: Date (DataInicial nova)                       │
 * │     → param intervalo: Object { DataInicial, DataFinal } original   │
 * │     → returns Object|null: { novaDataFinal, novasDatasRecorrencia } │
 * │     → Fluxo: (368 linhas - FUNÇÃO MAIS COMPLEXA)                    │
 * │       1. Calcular diff: dataNova - dataOriginal (ms)                │
 * │       2. Calcular duração original: DataFinal - DataInicial         │
 * │       3. Push DataFinal: new Date(DataFinal.getTime() + diff)       │
 * │       4. Se recorrente:                                             │
 * │          a. Obter config recorrência (lstRecorrente, lstPeriodos, etc.)│
 * │          b. Construir configRecorrencia object para RecorrenciaLogic│
 * │          c. Call RecorrenciaLogic.calcularDatasRecorrencia(dataNova,│
 * │             configRecorrencia)                                      │
 * │          d. Return array de datas pushed                            │
 * │       5. Validações:                                                 │
 * │          - TipoRecorrencia válido ("Semanal"/"Mensal"/"Custom")     │
 * │          - QuantidadePeriodos ou FinalRecorrencia obrigatórios      │
 * │          - DiasSemana/DiasMes/DatasSelecionadas conforme tipo       │
 * │       6. Atualizar UI:                                               │
 * │          - txtDataFinal.value = novaDataFinal                       │
 * │          - calDatasSelecionadas.values = novasDatasRecorrencia      │
 * │       7. Return { novaDataFinal: Date, novasDatasRecorrencia: Array }│
 * │     → Uso típico: criarAgendamentoEdicao após detectar mudança      │
 * │     → Integração: RecorrenciaLogic.calcularDatasRecorrencia         │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 3: EDIÇÃO DE AGENDAMENTOS ───────────────────────────────────┐
 * │ 21. window.editarAgendamento(viagemId)                               │
 * │     → Edita agendamento ÚNICO (não recorrente ou único de série)    │
 * │     → param viagemId: int                                           │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. recuperarViagemEdicao(viagemId)                            │
 * │       2. Verificar se EhRecorrente:                                 │
 * │          - Se false: edição simples (único agendamento)             │
 * │          - Se true: verificar se é único alterado na série          │
 * │       3. Caso único alterado:                                       │
 * │          - Usar criarAgendamentoViagem (estrutura especial)         │
 * │          - Endpoint: AlterarRecorrenciaViagem                       │
 * │       4. Caso normal:                                               │
 * │          - Usar criarAgendamentoEdicao (estrutura padrão)           │
 * │          - Endpoint: AtualizarViagem                                │
 * │       5. aplicarAtualizacao(agendamentoEditado)                     │
 * │       6. catch: handleAgendamentoError(error)                       │
 * │     → Uso típico: calendario.js click em agendamento Status=Aberta  │
 * │                                                                       │
 * │ 22. window.editarAgendamentoRecorrente(viagemId, editaTodos,        │
 * │              dataInicialRecorrencia, recorrenciaViagemId,           │
 * │              editarAgendamentoRecorrente)                           │
 * │     → Edita série recorrente (todos ou próximos)                    │
 * │     → param viagemId: int (ID clicado)                              │
 * │     → param editaTodos: boolean (editar todos da série)             │
 * │     → param dataInicialRecorrencia: Date (data do clicado)          │
 * │     → param recorrenciaViagemId: int (ID do grupo)                  │
 * │     → param editarAgendamentoRecorrente: boolean (true sempre)      │
 * │     → returns Promise<void>                                          │
 * │     → Fluxo:                                                         │
 * │       1. Se editaTodos: editarProximos = false                      │
 * │       2. Senão: editarProximos = true                               │
 * │       3. Call enviarAgendamentoComOpcao(viagemId, editaTodos,       │
 * │          editarProximos, dataInicialRecorrencia, recorrenciaViagemId)│
 * │       4. catch: handleAgendamentoError(error)                       │
 * │     → Uso típico: aoAbrirModalViagem após Swal.fire("Editar todos?")│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 2 (continuação): MENSAGENS E ERRO ───────────────────────────┐
 * │ 23. window.exibirMensagemSucesso()                                   │
 * │     → Toast success genérico (usado raramente)                      │
 * │     → returns void                                                   │
 * │     → Fluxo: Swal.fire({ icon: "success", title: "Sucesso!",       │
 * │       text: "Operação realizada", timer: 2000 })                    │
 * │                                                                       │
 * │ 24. window.exibirErroAgendamento()                                   │
 * │     → Toast error genérico (deprecated, não usado)                  │
 * │     → returns void                                                   │
 * │     → Fluxo: Swal.fire({ icon: "error", title: "Erro!" })          │
 * │                                                                       │
 * │ 25. window.handleAgendamentoError(error)                             │
 * │     → Handler centralizado de erros de agendamento                  │
 * │     → param error: Error object (com responseJSON do backend)       │
 * │     → returns void                                                   │
 * │     → Fluxo:                                                         │
 * │       1. Extrair error.responseJSON.errors (array de strings)       │
 * │       2. Alerta.MostrarMensagemErro(errors.join("<br>"))            │
 * │       3. Alerta.TratamentoErroComLinha("modal-viagem.js",           │
 * │          "handleAgendamentoError", error)                           │
 * │     → Uso típico: catch blocks em enviar/aplicar funções            │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 5: INTEGRAÇÃO COM RELATÓRIO ─────────────────────────────────┐
 * │ 26. window.carregarRelatorioNoModal()                                │
 * │     → Carrega Telerik ReportViewer no modal de relatório            │
 * │     → returns void (side effect: cria/atualiza telerikReportViewer) │
 * │     → Fluxo: (301 linhas)                                           │
 * │       1. Verificar isReportViewerLoading (evitar duplo load)        │
 * │       2. Obter ViagemId do StateManager                             │
 * │       3. Se ViagemId == ultimoViagemIdCarregado: return (cache)     │
 * │       4. Set isReportViewerLoading = true                           │
 * │       5. Show modal: $("#modalRelatorio").modal("show")             │
 * │       6. Verificar se telerikReportViewer já existe:                │
 * │          - Se exists: destroy() para recriar                        │
 * │       7. Criar nova instância Telerik ReportViewer:                 │
 * │          telerikReportViewer = $("#reportViewer1").telerik_ReportViewer({│
 * │            serviceUrl: "/api/reports/",                             │
 * │            reportSource: {                                          │
 * │              report: "ReportAgendamento.trdp",                      │
 * │              parameters: { ViagemId: viagemId }                     │
 * │            },                                                        │
 * │            viewMode: "INTERACTIVE",                                 │
 * │            scaleMode: "FIT_PAGE_WIDTH",                             │
 * │            scale: 1.0,                                              │
 * │            ready: function() { console.log "ReportViewer pronto" }, │
 * │            error: function(e, args) { console.error + Alerta }     │
 * │          }).data("telerik_ReportViewer")                            │
 * │       8. renderingEnd event: isReportViewerLoading = false          │
 * │       9. Atualizar ultimoViagemIdCarregado = viagemId               │
 * │      10. catch: Alerta.TratamentoErroComLinha + isReportViewerLoading = false│
 * │     → Dependências: Telerik Reporting jQuery plugin, DOM #reportViewer1│
 * │     → Uso típico: relatorio.js btnVisualizarRelatorio click         │
 * │                                                                       │
 * │ EVENT HANDLER: aoAbrirModalViagem(event)                             │
 * │     → Bootstrap Modal shown.bs.modal event handler                  │
 * │     → param event: jQuery event object                              │
 * │     → returns void (side effect: configura modal)                   │
 * │     → Fluxo: (79 linhas)                                            │
 * │       1. console.log "Modal aberto"                                 │
 * │       2. Obter StateManager states:                                 │
 * │          - ehEdicao, viagemId, ehRecorrente, modoCancelamento       │
 * │       3. Switch title baseado em modo:                              │
 * │          - modoCancelamento: "Cancelar Agendamento"                 │
 * │          - ehEdicao + ehRecorrente: "Editar Série Recorrente"       │
 * │          - ehEdicao: "Editar Agendamento"                           │
 * │          - default: "Criar Agendamento"                             │
 * │       4. ModalConfig.setModalTitle("modalViagens", title, icon, color)│
 * │       5. Se ehEdicao:                                               │
 * │          a. recuperarViagemEdicao(viagemId)                         │
 * │          b. inicializarCamposModal(dados) → preencher form          │
 * │          c. Se Status != "Aberta": desabilitarTodosControles()      │
 * │       6. Se !ehEdicao:                                              │
 * │          - limparCamposModalViagens() → reset form                  │
 * │       7. Set modalJaFoiLimpo = false                                │
 * │     → Attachment: main.js → $("#modalViagens").on("shown.bs.modal") │
 * │                                                                       │
 * │ EVENT HANDLER: aoFecharModalViagem()                                 │
 * │     → Bootstrap Modal hidden.bs.modal event handler                 │
 * │     → returns void (side effect: limpa modal)                       │
 * │     → Fluxo: (43 linhas)                                            │
 * │       1. console.log "Modal fechado"                                │
 * │       2. Se !modalJaFoiLimpo:                                       │
 * │          a. limparCamposModalViagens()                              │
 * │          b. Set modalJaFoiLimpo = true                              │
 * │       3. StateManager resets:                                       │
 * │          - set("ehEdicao", false)                                   │
 * │          - set("viagemId", null)                                    │
 * │          - set("ehRecorrente", false)                               │
 * │          - set("modoCancelamento", false)                           │
 * │       4. ModalConfig.resetModal("modalViagens")                     │
 * │       5. RecorrenciaUI.esconder() → hide recorrência fields         │
 * │     → Attachment: main.js → $("#modalViagens").on("hidden.bs.modal")│
 * │                                                                       │
 * │ EVENT HANDLER: inicializarEventosRelatorioModal()                    │
 * │     → Inicializa event listeners para modal de relatório            │
 * │     → returns void (side effect: attach events)                     │
 * │     → Fluxo:                                                         │
 * │       1. $("#modalRelatorio").on("shown.bs.modal"): console.log     │
 * │       2. $("#modalRelatorio").on("hidden.bs.modal"):                │
 * │          - console.log "Relatório fechado"                          │
 * │          - telerikReportViewer?.destroy() (cleanup)                 │
 * │          - telerikReportViewer = null                               │
 * │          - isReportViewerLoading = false                            │
 * │     → Chamado por: main.js (inicialização app)                      │
 * │     → Redefinição: linha 2161 redefine window.carregarRelatorioNoModal│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 6: INICIALIZAÇÃO E LIMPEZA DE CAMPOS ────────────────────────┐
 * │ 27. window.inicializarCamposModal(dados)                             │
 * │     → Preenche form com dados de agendamento para edição            │
 * │     → param dados: Object (retorno de recuperarViagemEdicao)        │
 * │     → returns void (side effect: atualiza DOM)                      │
 * │     → Fluxo: (59 linhas)                                            │
 * │       1. console.log "Inicializando campos com dados"               │
 * │       2. Preencher 16 campos Syncfusion/Kendo:                      │
 * │          - DateTimePicker: txtDataInicial.value = new Date(dados.DataInicial)│
 * │          - DropDownList: lstMotorista.value = dados.MotoristaId     │
 * │          - RichTextEditor: rteDescricao.value = dados.Descricao     │
 * │          - ComboBox: lstRequisitante.value(dados.RequisitanteId)    │
 * │          - NumericTextBox: ddtCombustivelInicial.value = dados.CombustivelInicial│
 * │          - etc. (todos os 16 campos)                                │
 * │       3. Se EhRecorrente:                                           │
 * │          - RecorrenciaUI.mostrar()                                  │
 * │          - Preencher campos recorrência (lstRecorrente, lstPeriodos,│
 * │            lstDias, calDatasSelecionadas, etc.)                     │
 * │       4. Se !EhRecorrente:                                          │
 * │          - RecorrenciaUI.esconder()                                 │
 * │       5. refresh() em todos os componentes Syncfusion               │
 * │     → Uso típico: aoAbrirModalViagem após recuperarViagemEdicao     │
 * │                                                                       │
 * │ 28. window.inicializarComponentesEJ2()                               │
 * │     → Cria instâncias Syncfusion EJ2 se não existem                 │
 * │     → returns void (side effect: appendTo em elementos DOM)         │
 * │     → Fluxo: (36 linhas)                                            │
 * │       1. Para cada componentId em lista (txtDataInicial, lstMotorista, etc.):│
 * │          a. Verificar se elemento.ej2_instances existe              │
 * │          b. Se não: console.warn "Componente não encontrado"        │
 * │          c. Não cria automaticamente (apenas verifica)              │
 * │       2. Nota: criação real via Razor/C# (não JavaScript)           │
 * │     → Uso típico: debug/diagnóstico (não usado em produção)         │
 * │                                                                       │
 * │ 29. window.limparCamposRecorrencia()                                 │
 * │     → Limpa apenas campos de recorrência (não todos)                │
 * │     → returns void (side effect: reset 6 campos)                    │
 * │     → Fluxo:                                                         │
 * │       1. lstRecorrente.value = "Não"                                │
 * │       2. lstPeriodos.value = null                                   │
 * │       3. lstDias.value = null                                       │
 * │       4. lstDiasMes.value = null                                    │
 * │       5. txtFinalRecorrencia.value = null                           │
 * │       6. calDatasSelecionadas.values = []                           │
 * │       7. RecorrenciaUI.esconder()                                   │
 * │     → Uso típico: lstRecorrente change → "Não" → limpar fields      │
 * │                                                                       │
 * │ 30. window.limparCamposModalViagens()                                │
 * │     → Limpa TODOS os campos do form (reset completo)                │
 * │     → returns void (side effect: reset 16+ campos)                  │
 * │     → Fluxo: (353 linhas - FUNÇÃO MAIS LONGA)                       │
 * │       1. try-catch completo com Alerta.TratamentoErroComLinha       │
 * │       2. console.log "Limpando campos do modal"                     │
 * │       3. Limpar 16 campos principais:                               │
 * │          - DateTimePicker: txtDataInicial.value = null              │
 * │          - DropDownList: lstMotorista.value = null                  │
 * │          - RichTextEditor: rteDescricao.value = ""                  │
 * │          - ComboBox: lstRequisitante.value(null)                    │
 * │          - NumericTextBox: ddtCombustivelInicial.value = null       │
 * │          - etc.                                                      │
 * │       4. Limpar campos recorrência:                                 │
 * │          - limparCamposRecorrencia()                                │
 * │       5. Limpar campos eventos:                                     │
 * │          - lstEventos.value = null                                  │
 * │       6. RecorrenciaUI.esconder()                                   │
 * │       7. refresh() em todos os componentes                          │
 * │       8. Safe checks: if (componente) antes de cada clear           │
 * │       9. console.log "Campos limpos com sucesso"                    │
 * │     → Uso típico: aoFecharModalViagem, aoAbrirModalViagem (!ehEdicao)│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SEÇÃO 7: CONTROLE DE ESTADO DO MODAL ──────────────────────────────┐
 * │ 31. window.desabilitarTodosControles()                               │
 * │     → Desabilita form inteiro (modo visualização)                   │
 * │     → returns void (side effect: disable 16 campos, protege 5 botões)│
 * │     → Fluxo: (95 linhas)                                            │
 * │       1. try-catch com Alerta.TratamentoErroComLinha                │
 * │       2. console.log "Desabilitando controles do modal"             │
 * │       3. Lista de 5 botões protegidos (NUNCA desabilitar):          │
 * │          - btnFechar, btnCancelar, btnCancelarModal,                │
 * │            btnFecharRelatorio, btn-close                            │
 * │       4. Desabilitar botões genéricos (querySelectorAll button):    │
 * │          - Se !isProtegido: button.disabled = true                  │
 * │       5. Desabilitar 16 componentes EJ2:                            │
 * │          - Para cada: elemento.ej2_instances[0].enabled = false     │
 * │       6. GARANTIR botões protegidos sempre habilitados:             │
 * │          - disabled = false, classList.remove('disabled'),          │
 * │            style.pointerEvents = 'auto'                             │
 * │       7. Garantir botão X do modal (.btn-close) sempre habilitado   │
 * │       8. console.log "Controles desabilitados (exceto fechar)"      │
 * │     → Uso típico: aoAbrirModalViagem quando Status != "Aberta"      │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO 1 - CRIAR NOVO AGENDAMENTO:
 * 1. Usuário clica em data no calendario.js → StateManager.set("ehEdicao", false)
 * 2. Bootstrap Modal show → trigger shown.bs.modal event
 * 3. aoAbrirModalViagem() → ModalConfig.setModalTitle("Criar Agendamento")
 * 4. aoAbrirModalViagem() → !ehEdicao → limparCamposModalViagens()
 * 5. Usuário preenche form (16 campos Syncfusion/Kendo)
 * 6. Usuário clica btnSalvar → recorrencia.js handler
 * 7. recorrencia.js → criarAgendamentoNovo() → objeto com 40+ props
 * 8. enviarAgendamento(agendamento) → !ehEdicao → enviarNovoAgendamento()
 * 9. ApiClient.post("/api/Viagem/AdicionarAgendamento", agendamento)
 * 10. Success → Swal.fire("Agendamento criado") → modal.hide()
 * 11. Modal hide → trigger hidden.bs.modal event
 * 12. aoFecharModalViagem() → limparCamposModalViagens() → StateManager resets
 *
 * 🔄 FLUXO TÍPICO 2 - EDITAR AGENDAMENTO ÚNICO:
 * 1. Usuário clica em agendamento existente no calendario.js
 * 2. calendario.js → StateManager.set("ehEdicao", true, "viagemId", 123)
 * 3. Bootstrap Modal show → trigger shown.bs.modal
 * 4. aoAbrirModalViagem() → ModalConfig.setModalTitle("Editar Agendamento")
 * 5. aoAbrirModalViagem() → ehEdicao → recuperarViagemEdicao(123)
 * 6. ApiClient.get("/api/Viagem/PegarViagemParaEdicao", { viagemId: 123 })
 * 7. inicializarCamposModal(dados) → preencher form com dados backend
 * 8. Se Status != "Aberta" → desabilitarTodosControles() (modo visualização)
 * 9. Se Status == "Aberta" → usuário edita form
 * 10. Usuário clica btnSalvar → criarAgendamentoEdicao(agendamentoOriginal)
 * 11. detectarAlteracaoDataInicial() → se mudou DataInicial: calcularPushDatas()
 * 12. enviarAgendamento() → ehEdicao → aplicarAtualizacao()
 * 13. ApiClient.put("/api/Viagem/AtualizarViagem", objViagem)
 * 14. Success → Swal.fire("Agendamento atualizado") → modal.hide()
 * 15. aoFecharModalViagem() → limparCamposModalViagens() → StateManager resets
 *
 * 🔄 FLUXO TÍPICO 3 - EDITAR SÉRIE RECORRENTE (TODOS):
 * 1. Usuário clica em agendamento de série recorrente
 * 2. calendario.js → detecta EhRecorrente → Swal.fire("Editar apenas este ou todos?")
 * 3. Usuário escolhe "Editar todos"
 * 4. StateManager.set("ehEdicao", true, "ehRecorrente", true, "viagemId", 123)
 * 5. Bootstrap Modal show → aoAbrirModalViagem() → title "Editar Série Recorrente"
 * 6. aoAbrirModalViagem() → recuperarViagemEdicao(123) → inicializarCamposModal()
 * 7. RecorrenciaUI.mostrar() → exibir campos recorrência
 * 8. Usuário edita form (mudanças aplicadas a TODOS da série)
 * 9. Usuário clica btnSalvar → editarAgendamentoRecorrente(123, editaTodos=true, ...)
 * 10. obterAgendamentosRecorrentes(RecorrenciaViagemId) → GET todos da série
 * 11. Para cada agendamento: criarAgendamentoEdicao() → enviarNovoAgendamento()
 * 12. POST batch (N requests, isUltimoAgendamento só no último)
 * 13. Success último → Swal.fire("Agendamentos atualizados") → modal.hide()
 * 14. aoFecharModalViagem() → limparCamposModalViagens() → StateManager resets
 *
 * 🔄 FLUXO TÍPICO 4 - CANCELAR AGENDAMENTO:
 * 1. Usuário clica btnCancelar em exibe-viagem.js
 * 2. Swal.fire com textarea para motivo cancelamento
 * 3. Usuário digita motivo → confirma
 * 4. exibe-viagem.js → cancelarAgendamento(viagemId, descricao)
 * 5. ApiClient.put("/api/Viagem/CancelarAgendamento", { viagemId, descricao })
 * 6. Backend → Status="Cancelada", DescricaoCancelamento=descricao
 * 7. Success → Swal.fire("Agendamento cancelado") → modal.hide()
 * 8. aoFecharModalViagem() → limparCamposModalViagens()
 *
 * 🔄 FLUXO TÍPICO 5 - CARREGAR RELATÓRIO:
 * 1. Usuário clica btnVisualizarRelatorio em relatorio.js
 * 2. relatorio.js → carregarRelatorioNoModal()
 * 3. Verificar isReportViewerLoading (evitar duplo load)
 * 4. Obter ViagemId do StateManager
 * 5. Se ViagemId == ultimoViagemIdCarregado: return (cache)
 * 6. $("#modalRelatorio").modal("show") → exibir modal
 * 7. Criar/atualizar Telerik ReportViewer:
 *    - serviceUrl: "/api/reports/"
 *    - reportSource: { report: "ReportAgendamento.trdp", parameters: { ViagemId } }
 * 8. renderingEnd event → isReportViewerLoading = false
 * 9. Usuário visualiza relatório PDF (Telerik viewer)
 * 10. Usuário fecha modal → hidden.bs.modal event
 * 11. inicializarEventosRelatorioModal handler → telerikReportViewer.destroy()
 *
 * 📌 SINCRONIZAÇÃO ENTRE DATASINICIAL E DATAFINAL (Push Logic):
 * - Quando usuário edita agendamento e muda DataInicial:
 * - detectarAlteracaoDataInicial() compara original vs atual
 * - Se houveMudanca: calcularPushDatas() calcula diff (ms)
 * - Push DataFinal: novaDataFinal = DataFinal + diff (mantém duração)
 * - Push datas recorrência: RecorrenciaLogic.calcularDatasRecorrencia() com nova DataInicial
 * - Atualiza UI: txtDataFinal.value = novaDataFinal, calDatasSelecionadas.values = novasDatas
 * - Resultado: intervalo mantém mesma duração, série recorrente acompanha mudança
 *
 * 📌 ESTRUTURA OBJETO AGENDAMENTO (40+ props):
 * - IDs: ViagemId (int), RecorrenciaViagemId (int nullable), MotoristaId (int),
 *   VeiculoId (int), RequisitanteId (int), SetorId (int), FinalidadeId (int), EventoId (int nullable)
 * - Timestamps: DataInicial (ISO string), DataFinal (ISO string)
 * - Strings: Descricao (HTML), Origem (string), Destino (string), NomeEvento (string nullable),
 *   Status (string: "Aberta", "Cancelada", "Concluída"), DescricaoCancelamento (string nullable)
 * - Numbers: CombustivelInicial (decimal 0-8), CombustivelFinal (decimal 0-8)
 * - Booleans: EhRecorrente (boolean)
 * - Recorrência (se EhRecorrente):
 *   - TipoRecorrencia (string: "Semanal", "Mensal", "Custom")
 *   - DiasSemana (int[] 0-6, para Semanal)
 *   - DiasMes (int[] 1-31, para Mensal)
 *   - DatasSelecionadas (ISO string[], para Custom)
 *   - QuantidadePeriodos (int nullable, número de repetições)
 *   - FinalRecorrencia (ISO string nullable, data limite)
 * - Exemplo:
 *   {
 *     ViagemId: 123,
 *     DataInicial: "2026-02-03T08:00:00.000Z",
 *     DataFinal: "2026-02-03T18:00:00.000Z",
 *     MotoristaId: 45,
 *     VeiculoId: 12,
 *     RequisitanteId: 789,
 *     SetorId: 5,
 *     FinalidadeId: 2,
 *     Descricao: "<p>Reunião importante</p>",
 *     Origem: "Câmara",
 *     Destino: "Prefeitura",
 *     CombustivelInicial: 6,
 *     CombustivelFinal: 4,
 *     EhRecorrente: true,
 *     TipoRecorrencia: "Semanal",
 *     DiasSemana: [1, 3, 5],
 *     QuantidadePeriodos: 10,
 *     EventoId: 3,
 *     NomeEvento: "Reunião Semanal"
 *   }
 *
 * 📌 ENDPOINTS API (6 endpoints):
 * - POST /api/Viagem/AdicionarAgendamento
 *   → Body: agendamento object (novo)
 *   → Returns: { success: boolean, viagemId: int, recorrenciaViagemId: int nullable }
 * - PUT /api/Viagem/AtualizarViagem
 *   → Body: agendamento object (editado)
 *   → Returns: { success: boolean }
 * - POST /api/Viagem/AlterarRecorrenciaViagem
 *   → Body: { AgendamentoUnicoAlterado, AgendamentoOriginal, DataInicial, RecorrenciaViagemId }
 *   → Returns: { success: boolean }
 *   → Uso: editar único agendamento dentro de série recorrente
 * - GET /api/Viagem/PegarViagemParaEdicao?viagemId={id}
 *   → Returns: agendamento object completo
 * - GET /api/Viagem/PegarRecorrenciaViagem?recorrenciaViagemId={id}
 *   → Returns: { data: agendamento[] }
 * - PUT /api/Viagem/CancelarAgendamento
 *   → Body: { viagemId: int, descricao: string }
 *   → Returns: { success: boolean }
 *
 * 📌 COMPONENTES SYNCFUSION/KENDO (16 fields):
 * - Syncfusion DateTimePicker (3): txtDataInicial, txtDataFinal, txtFinalRecorrencia
 * - Syncfusion DropDownList (10): lstMotorista, lstVeiculo, lstFinalidade,
 *   lstSetorRequisitanteAgendamento, lstRecorrente, lstPeriodos, lstDias, lstDiasMes, lstEventos
 * - Syncfusion RichTextEditor (1): rteDescricao
 * - Syncfusion NumericTextBox (2): ddtCombustivelInicial, ddtCombustivelFinal
 * - Syncfusion Calendar (1): calDatasSelecionadas (MultiSelect calendar)
 * - Kendo UI ComboBox (1): lstRequisitante (usa kendoComboBox, não ej2_instances)
 *
 * 📌 BOTÕES PROTEGIDOS (nunca desabilitar, mesmo em modo visualização):
 * 1. btnFechar (id)
 * 2. btnCancelar (id)
 * 3. btnCancelarModal (id)
 * 4. btnFecharRelatorio (id)
 * 5. .btn-close (class, botão X do modal)
 * → Motivo: garantir que usuário sempre pode fechar modal (UX crítico)
 * → Implementação: desabilitarTodosControles() verifica ID antes de disable
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Arquivo mais complexo do módulo agendamento (2874 linhas)
 * - 28 funções exportadas via window.* (escopo global)
 * - 4 global variables para state management (modalJaFoiLimpo, telerikReportViewer, etc.)
 * - 3 event handlers para Bootstrap Modal (shown/hidden) e ReportViewer
 * - Try-catch completo em todas as 28 funções async
 * - Alerta.TratamentoErroComLinha em todos os catch blocks
 * - Console.log extensivo para debug (production-ready)
 * - Safe checks: if (elemento?.ej2_instances?.[0]) em todos os acessos Syncfusion
 * - Kendo special: lstRequisitante usa $(element).data("kendoComboBox") (não ej2_instances)
 * - Recorrência: 3 tipos suportados (Semanal, Mensal, Custom) com validações específicas
 * - Push/Pull logic: 368 linhas em calcularPushDatas (função mais complexa)
 * - ReportViewer: lazy loading + cache (ultimoViagemIdCarregado) + destroy on close
 * - Modal modes: Criar (novo), Editar (único), Editar Série (recorrente), Visualizar (Status fechado), Cancelar
 * - Status agendamento: "Aberta" (editável), "Cancelada" (readonly), "Concluída" (readonly)
 * - Desabilitar controles: mantém 5 botões protegidos sempre habilitados (UX crítico)
 * - Bootstrap Modal API: $.modal('show'/'hide'), on('shown.bs.modal'/'hidden.bs.modal')
 * - Syncfusion API: refresh(), dataBind(), destroy(), appendTo(), value setter/getter
 * - Telerik API: telerik_ReportViewer(), renderingEnd event, destroy()
 * - StateManager integration: 8 estados (ehEdicao, viagemId, ehRecorrente, modoCancelamento, etc.)
 * - RecorrenciaLogic integration: calcularDatasRecorrencia para push/pull datas
 * - ModalConfig integration: setModalTitle (4 icons/colors), resetModal
 * - Alerta integration: TratamentoErroComLinha, MostrarMensagemErro
 * - Swal integration: success toasts (timer 2000ms)
 * - ApiClient integration: 6 endpoints (POST/PUT/GET) com error handling padronizado
 * - RecorrenciaUI integration: mostrar/esconder campos recorrência
 * - Validações: campos obrigatórios verificados no backend (errors array)
 * - Timestamps: sempre ISO strings (new Date().toISOString())
 * - Combustível: NumericTextBox format="n0" (0 decimais, range 0-8)
 * - Descrição: RichTextEditor com HTML output
 * - Requisitante: ComboBox com autocomplete + botão adicionar novo
 * - Setor: carregado via GET AJAXPreencheListaSetores ao selecionar requisitante
 * - Eventos: DropDownList opcional, carregado via EventoService
 * - Recorrência Semanal: DiasSemana array (0=Domingo, 1=Segunda, ..., 6=Sábado)
 * - Recorrência Mensal: DiasMes array (1-31, validação backend para meses com menos dias)
 * - Recorrência Custom: DatasSelecionadas via Calendar multiSelect
 * - Limite recorrência: QuantidadePeriodos XOR FinalRecorrencia (um dos dois obrigatório)
 *
 * 🔌 VERSÃO: 4.0 (refatorado após Lote 192, adiciona comprehensive header)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */

// ====================================================================
// SEÇÃO 1: CRIAÇÃO DE OBJETOS DE AGENDAMENTO
// ====================================================================

/**
 * Flag global para controlar limpeza do modal
 * Evita que a limpeza seja executada múltiplas vezes
 */
window.modalJaFoiLimpo = false;

// Variável global para controlar instância do Report Viewer
window.telerikReportViewer = null;
window.isReportViewerLoading = false;

// Variável para rastrear último ID carregado
window.ultimoViagemIdCarregado = null;

/**
 * ðŸ”§ Função auxiliar segura para refresh de componentes Syncfusion
 * Evita erros quando o componente não está inicializado
 * param {string} elementId - ID do elemento
 * returns {boolean} Sucesso da operação
 */
window.refreshComponenteSafe = function (elementId)
{
    try
    {
        const elemento = document.getElementById(elementId);
        if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
        {
            const instancia = elemento.ej2_instances[0];

            // Verificar se o método existe antes de chamar
            if (typeof instancia.refresh === 'function')
            {
                instancia.refresh();
            } else if (typeof instancia.dataBind === 'function')
            {
                instancia.dataBind();
            }

            return true;
        }
        return false;
    } catch (error)
    {
        console.warn(`âš ï¸ Não foi possível atualizar ${elementId}:`, error);
        return false;
    }
};

/**
 * ðŸ“ Cria objeto de agendamento NOVO a partir dos campos do formulário
 * Esta é a função BASE que lê todos os campos e monta o objeto
 * returns {Object|null} Objeto de agendamento ou null em caso de erro
 */
window.criarAgendamentoNovo = function ()
{
    try
    {
        console.log("ðŸ“ [criarAgendamentoNovo] === INICIANDO ===");

        // Obter instâncias dos componentes Syncfusion
        const rteDescricao = document.getElementById("rteDescricao")?.ej2_instances?.[0];
        const lstMotorista = document.getElementById("lstMotorista")?.ej2_instances?.[0];
        const lstVeiculo = document.getElementById("lstVeiculo")?.ej2_instances?.[0];
        // Telerik Kendo: usa $(element).data("kendoComboBox")
        const lstRequisitanteEl = document.getElementById("lstRequisitante");
        const lstRequisitante = lstRequisitanteEl ? $(lstRequisitanteEl).data("kendoComboBox") : null;
        const lstSetorRequisitanteAgendamento = document.getElementById("lstSetorRequisitanteAgendamento")?.ej2_instances?.[0];
        // ✅ KENDO: Origem e Destino agora usam Kendo ComboBox
        const cmbOrigem = $("#cmbOrigem").data("kendoComboBox");
        const cmbDestino = $("#cmbDestino").data("kendoComboBox");
        const lstFinalidade = document.getElementById("lstFinalidade")?.ej2_instances?.[0];
        const ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial")?.ej2_instances?.[0];
        const ddtCombustivelFinal = document.getElementById("ddtCombustivelFinal")?.ej2_instances?.[0];
        const lstEventos = document.getElementById("lstEventos")?.ej2_instances?.[0];
        const lstRecorrente = document.getElementById("lstRecorrente")?.ej2_instances?.[0];
        const lstPeriodos = document.getElementById("lstPeriodos")?.ej2_instances?.[0];
        const txtFinalRecorrencia = window.getKendoDateValue("txtFinalRecorrencia");
        const lstDias = document.getElementById("lstDias")?.ej2_instances?.[0];
        const calDatasSelecionadas = document.getElementById("calDatasSelecionadas")?.ej2_instances?.[0];
        const lstDiasMes = document.getElementById("lstDiasMes")?.ej2_instances?.[0];

        // Extrair valores
        const dataInicialValue = window.getKendoDateValue("txtDataInicial");
        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
        const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");
        const horaFimTexto = window.getKendoTimeValue("txtHoraFinal");

        // DEPOIS da linha 60, adicione este debug:
        console.log("ðŸ” [DEBUG] Valores capturados:");
        console.log("   - lstMotorista?.value:", lstMotorista?.value);
        console.log("   - lstVeiculo?.value:", lstVeiculo?.value);
        //console.log("   - typeof motoristaId:", typeof motoristaId);
        //console.log("   - typeof veiculoId:", typeof veiculoId);

        const motoristaId = lstMotorista?.value;
        const veiculoId = lstVeiculo?.value;

        // CORREÇÃO: Garantir que os valores sejam strings válidas ou null
        const motoristaIdFinal = (motoristaId && motoristaId !== "null" && motoristaId !== "undefined")
            ? String(motoristaId)
            : null;

        const veiculoIdFinal = (veiculoId && veiculoId !== "null" && veiculoId !== "undefined")
            ? String(veiculoId)
            : null;

        // ✅ KENDO: Precisa chamar value() com parênteses!
        const requisitanteId = lstRequisitante?.value();

        const setorId = lstSetorRequisitanteAgendamento.value[0];
        // ✅ KENDO: Precisa chamar value() com parênteses!
        const origem = cmbOrigem?.value();
        const destino = cmbDestino?.value();
        const finalidade = window.getSfValue0(lstFinalidade);
        const combustivelInicial = window.getSfValue0(ddtCombustivelInicial);
        const combustivelFinal = window.getSfValue0(ddtCombustivelFinal);
        const descricaoHtml = rteDescricao?.getHtml() ?? "";
        const ramal = $("#txtRamalRequisitanteSF").val();
        const kmAtual = window.parseIntSafe($("#txtKmAtual").val());
        const kmInicial = window.parseIntSafe($("#txtKmInicial").val());
        const kmFinal = window.parseIntSafe($("#txtKmFinal").val());
        const noFichaVistoria = $("#txtNoFichaVistoria").val() || 0;

        // Processar evento
        let eventoId = null;

        if (lstEventos?.value)
        {
            const eventosVal = lstEventos.value;

            // ✅ Tratar tanto array (MultiSelect) quanto valor único (ComboBox)
            if (Array.isArray(eventosVal) && eventosVal.length > 0)
            {
                eventoId = eventosVal[0]; // MultiSelect
            } else if (eventosVal)
            {
                eventoId = eventosVal; // ComboBox
            }
        }

        console.log("🎪 EventoId capturado:", eventoId);

        // Processar datas
        let dataInicial = null;
        let horaInicio = null;

        if (dataInicialValue)
        {
            const dataInicialDate = new Date(dataInicialValue);
            dataInicial = window.toDateOnlyString(dataInicialDate);

            if (horaInicioTexto)
            {
                horaInicio = window.toLocalDateTimeString(dataInicialDate, horaInicioTexto);
            }
        }

        let dataFinal = null;
        if (dataFinalValue)
        {
            const dataFinalDate = new Date(dataFinalValue);
            dataFinal = window.toDateOnlyString(dataFinalDate);
        }

        // Processar recorrência
        const recorrente = lstRecorrente?.value ?? "N";
        const intervalo = window.getSfValue0(lstPeriodos) ?? "";

        let dataFinalRecorrencia = null;
        if (txtFinalRecorrencia)
        {
            const dataFinalRecDate = new Date(txtFinalRecorrencia);
            dataFinalRecorrencia = window.toDateOnlyString(dataFinalRecDate);
        }

        // ============================================================================
        // cÓDIGO CORRIGIDO - PRONTO PARA COPIAR E COLAR
        // ============================================================================
        // Substitua as linhas 171-198 do modal-viagem.js por este código
        // ============================================================================

        // Processar dias da semana (para recorrência semanal)
        let monday = false, tuesday = false, wednesday = false;
        let thursday = false, friday = false, saturday = false, sunday = false;

        if (lstDias?.value && Array.isArray(lstDias.value))
        {
            const diasSelecionados = lstDias.value;

            // âœ… CORREÇÃO: lstDias retorna NÚMEROS (0-6), não textos!
            // Mapeamento: 0=Domingo, 1=Segunda, 2=Terça, 3=Quarta, 4=Quinta, 5=Sexta, 6=Sábado
            sunday = diasSelecionados.includes(0);
            monday = diasSelecionados.includes(1);
            tuesday = diasSelecionados.includes(2);
            wednesday = diasSelecionados.includes(3);
            thursday = diasSelecionados.includes(4);
            friday = diasSelecionados.includes(5);
            saturday = diasSelecionados.includes(6);

            // Debug para verificar o mapeamento
            console.log("ðŸ“… Dias selecionados (números):", diasSelecionados);
            console.log("ðŸ“‹ Mapeamento booleano:", {
                domingo: sunday,
                segunda: monday,
                terca: tuesday,
                quarta: wednesday,
                quinta: thursday,
                sexta: friday,
                sabado: saturday
            });
        }

        // Processar datas selecionadas (para recorrência variada)
        let datasSelecionadas = null;
        if (calDatasSelecionadas?.values && Array.isArray(calDatasSelecionadas.values))
        {
            datasSelecionadas = calDatasSelecionadas.values
                .map(d => window.toDateOnlyString(new Date(d)))
                .join(",");
        }

        // ============================================================================
        // FIM DO cÓDIGO CORRIGIDO
        // ============================================================================

        // Processar dia do mês (para recorrência mensal)
        const diaMesRecorrencia = window.getSfValue0(lstDiasMes);

        // Montar objeto de agendamento
        const agendamento = {
            ViagemId: "00000000-0000-0000-0000-000000000000",
            RecorrenciaViagemId: "00000000-0000-0000-0000-000000000000",
            DataInicial: dataInicial,
            HoraInicio: horaInicio,
            DataFinal: dataFinal,
            HoraFim: horaFimTexto,
            Finalidade: finalidade,
            Origem: origem,
            Destino: destino,

            MotoristaId: motoristaIdFinal,
            VeiculoId: veiculoIdFinal,

            //MotoristaId: motoristaId,
            //VeiculoId: veiculoId,
            CombustivelInicial: combustivelInicial,
            CombustivelFinal: combustivelFinal,
            KmAtual: kmAtual,
            KmInicial: kmInicial,
            KmFinal: kmFinal,
            RequisitanteId: requisitanteId,
            RamalRequisitante: ramal,
            SetorSolicitanteId: setorId,
            Descricao: descricaoHtml,
            StatusAgendamento: true,
            FoiAgendamento: false,
            Status: "Agendada",
            EventoId: eventoId,
            Recorrente: recorrente,
            Intervalo: intervalo,
            DataFinalRecorrencia: dataFinalRecorrencia,
            Monday: monday,
            Tuesday: tuesday,
            Wednesday: wednesday,
            Thursday: thursday,
            Friday: friday,
            Saturday: saturday,
            Sunday: sunday,
            //DatasSelecionadas: datasSelecionadas,
            DiaMesRecorrencia: diaMesRecorrencia,
            NoFichaVistoria: noFichaVistoria
        };

        console.log("âœ… [criarAgendamentoNovo] Agendamento criado:", agendamento);
        return agendamento;
    } catch (error)
    {
        console.error("âŒ [criarAgendamentoNovo] ERRO:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamentoNovo", error);
        return null;
    }
};

/**
 * ðŸ“ Cria objeto de agendamento com recorrência
 * Usado quando o agendamento se repete em múltiplas datas
 * param {string} viagemId - ID da viagem
 * param {string} viagemIdRecorrente - ID da recorrência
 * param {string} dataInicial - Data inicial (formato YYYY-MM-DD)
 * returns {Object|null} Objeto de agendamento ou null em caso de erro
 */
window.criarAgendamento = function (viagemId, viagemIdRecorrente, dataInicial)
{
    try
    {
        console.log("ðŸ“ [criarAgendamento] === INICIANDO ===");
        console.log("   ðŸ“‹ Parâmetros recebidos:");
        console.log("      - viagemId:", viagemId);
        console.log("      - viagemIdRecorrente:", viagemIdRecorrente);
        console.log("      - dataInicial:", dataInicial);

        // âœ… CRIAR O AGENDAMENTO BASE usando a função que JÃ FUNCIONA
        console.log("   ðŸ”§ Chamando criarAgendamentoNovo()...");
        const agendamentoBase = window.criarAgendamentoNovo();

        if (!agendamentoBase)
        {
            console.error("   âŒ criarAgendamentoNovo retornou NULL!");
            throw new Error("Não foi possível criar o objeto base do agendamento");
        }

        console.log("   âœ… Agendamento base criado com sucesso");
        console.log("   ðŸ“‹ DataInicial do base:", agendamentoBase.DataInicial);

        // âœ… CLONAR o objeto para não modificar o original
        const agendamento = { ...agendamentoBase };

        // âœ… SOBRESCREVER os campos especí­ficos de recorrência
        agendamento.ViagemId = viagemId || "00000000-0000-0000-0000-000000000000";
        agendamento.RecorrenciaViagemId = viagemIdRecorrente || "00000000-0000-0000-0000-000000000000";

        // âœ… RECALCULAR HoraInicio quando DataInicial for alterada
        if (dataInicial)
        {
            const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");

            if (horaInicioTexto)
            {
                const dataInicialDate = new Date(dataInicial + 'T00:00:00');
                agendamento.DataInicial = dataInicial;
                agendamento.HoraInicio = window.toLocalDateTimeString(dataInicialDate, horaInicioTexto);

                console.log("   ðŸ”„ DataInicial SOBRESCRITA para:", dataInicial);
                console.log("   ðŸ”„ HoraInicio RECALCULADA para:", agendamento.HoraInicio);
            } else
            {
                console.error("   âŒ Hora inicial não encontrada!");
                throw new Error("Hora de Início é obrigatória");
            }
        }

        // âœ… VALIDAÇÕES CríTICAS
        const erros = [];

        if (!agendamento.DataInicial)
        {
            erros.push("Data Inicial é obrigatória");
        }

        if (!agendamento.HoraInicio)
        {
            erros.push("Hora de Início é obrigatória");
        }

        //if (!agendamento.MotoristaId) {
        //    erros.push("Motorista é obrigatório");
        //}

        //if (!agendamento.VeiculoId) {
        //    erros.push("Veí­culo é obrigatório");
        //}

        if (!agendamento.RequisitanteId)
        {
            erros.push("Requisitante é obrigatório");
        }

        if (!agendamento.Finalidade)
        {
            erros.push("Finalidade é obrigatória");
        }

        if (erros.length > 0)
        {
            console.error('âŒ ERRO DE VALIDAÇÃO:');
            console.error('      - ' + erros[0]);
            Alerta.Erro(erros[0]); // Mostra apenas o primeiro erro
            return null; // Para a execução
        }

        console.log("   âœ… === AGENDAMENTO CRIADO COM SUCESSO ===");
        console.log("   ðŸ“‹ Resumo do agendamento:");
        console.log("      - ViagemId:", agendamento.ViagemId);
        console.log("      - RecorrenciaViagemId:", agendamento.RecorrenciaViagemId);
        console.log("      - DataInicial:", agendamento.DataInicial);
        console.log("      - HoraInicio:", agendamento.HoraInicio);
        console.log("      - Recorrente:", agendamento.Recorrente);
        console.log("      - Intervalo:", agendamento.Intervalo);
        console.log("      - MotoristaId:", agendamento.MotoristaId);
        console.log("      - VeiculoId:", agendamento.VeiculoId);
        console.log("      - RequisitanteId:", agendamento.RequisitanteId);
        console.log("      - Finalidade:", agendamento.Finalidade);

        return agendamento;
    } catch (error)
    {
        console.error("âŒ [criarAgendamento] ERRO FATAL:", error);
        console.error("   Stack trace:", error.stack);

        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamento", error);
        AppToast.show("Vermelho", "Erro ao criar agendamento: " + error.message, 5000);

        return null;
    }
};

/**
 * ðŸ“ Cria objeto de agendamento para edição
 * Preserva campos originais e atualiza apenas os modificados
 * param {Object} agendamentoOriginal - Agendamento original do banco
 * returns {Object|null} Objeto de agendamento ou null em caso de erro
 */
window.criarAgendamentoEdicao = function (agendamentoOriginal)
{
    try
    {
        // Obter instâncias dos componentes
        const rteDescricao = document.getElementById("rteDescricao")?.ej2_instances?.[0];
        const lstMotorista = document.getElementById("lstMotorista")?.ej2_instances?.[0];
        const lstVeiculo = document.getElementById("lstVeiculo")?.ej2_instances?.[0];
        const ddtSetor = document.getElementById("lstSetorRequisitanteAgendamento")?.ej2_instances?.[0];
        const ddtFinalidade = document.getElementById("lstFinalidade")?.ej2_instances?.[0];
        const ddtCombIniInst = document.getElementById("ddtCombustivelInicial")?.ej2_instances?.[0];
        const ddtCombFimInst = document.getElementById("ddtCombustivelFinal")?.ej2_instances?.[0];
        const lstEventosInst = document.getElementById("lstEventos")?.ej2_instances?.[0];
        const rteDescricaoHtmlContent = rteDescricao?.getHtml() ?? "";

        // Extrair valores dos componentes
        const motoristaId = lstMotorista?.value ?? null;
        const veiculoId = lstVeiculo?.value ?? null;
        const setorId = window.getSfValue0(ddtSetor);
        // Kendo ComboBox - obter valor
        const lstReqEl = document.getElementById("lstRequisitante");
        const lstReqKendo = lstReqEl ? $(lstReqEl).data("kendoComboBox") : null;
        const requisitanteId = lstReqKendo?.value() ?? null;

        console.log("🔍 DEBUG GRAVAÇÃO Requisitante:");
        console.log("  - lstReqEl encontrado:", lstReqEl ? "SIM" : "NÃO");
        console.log("  - lstReqKendo encontrado:", lstReqKendo ? "SIM" : "NÃO");
        console.log("  - requisitanteId extraído:", requisitanteId);
        // ✅ KENDO: Origem e Destino agora usam Kendo ComboBox
        const destino = $("#cmbDestino").data("kendoComboBox")?.value() ?? null;
        const origem = $("#cmbOrigem").data("kendoComboBox")?.value() ?? null;
        const finalidade = window.getSfValue0(ddtFinalidade);
        const combustivelInicial = window.getSfValue0(ddtCombIniInst);
        const combustivelFinal = window.getSfValue0(ddtCombFimInst);
        const noFichaVistoria = $("#txtNoFichaVistoria").val() || 0;
        const kmAtual = window.parseIntSafe($("#txtKmAtual").val());
        const kmInicial = window.parseIntSafe($("#txtKmInicial").val());
        const kmFinal = window.parseIntSafe($("#txtKmFinal").val());

        // Ler campos de recorrência do formulário
        const dataFinalRecorrenciaValue = window.getKendoDateValue("txtFinalRecorrencia");
        let dataFinalRecorrenciaStr = null;
        if (dataFinalRecorrenciaValue)
        {
            const dataFinalRecorrenciaDate = new Date(dataFinalRecorrenciaValue);
            dataFinalRecorrenciaStr = window.toDateOnlyString(dataFinalRecorrenciaDate);
        }

        // Processar evento
        let eventoId = null;

        if (lstEventosInst?.value)
        {
            const eventosVal = lstEventosInst.value;

            // ✅ Tratar tanto array (MultiSelect) quanto valor único (ComboBox)
            if (Array.isArray(eventosVal) && eventosVal.length > 0)
            {
                eventoId = eventosVal[0]; // MultiSelect
            } else if (eventosVal)
            {
                eventoId = eventosVal; // ComboBox
            }
        }

        console.log("🎪 EventoId capturado:", eventoId);

        // ============================================================
        // LÓGICA DE DATA INICIAL
        // ============================================================
        // IMPORTANTE: Quando editando TODOS os agendamentos recorrentes,
        // cada agendamento deve MANTER sua data original.
        // Só usar a data do formulário quando:
        // 1. Não há data original (agendamentoOriginal.dataInicial é null)
        // 2. Está editando apenas UM agendamento

        let dataInicialStr = null;
        let horaInicioLocal = null;

        // Se agendamentoOriginal tem dataInicial, usar ela (edição recorrente "Todos")
        if (agendamentoOriginal?.dataInicial)
        {
            const dataOriginalDate = new Date(agendamentoOriginal.dataInicial);
            dataInicialStr = window.toDateOnlyString(dataOriginalDate);

            // Para hora, pegar do formulário (alteração aplicada a todos)
            const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");
            if (horaInicioTexto)
            {
                horaInicioLocal = window.toLocalDateTimeString(dataOriginalDate, horaInicioTexto);
            }

            console.log(`📅 Usando data ORIGINAL do agendamento: ${dataInicialStr}`);
        }
        // Senão, usar data do formulário (novo agendamento ou edição de apenas um)
        else
        {
            const txtDataInicialValue = window.getKendoDateValue("txtDataInicial");
            const horaInicioTexto = window.getKendoTimeValue("txtHoraInicial");

            if (txtDataInicialValue)
            {
                const dataInicialDate = new Date(txtDataInicialValue);
                dataInicialStr = window.toDateOnlyString(dataInicialDate);

                if (horaInicioTexto)
                {
                    horaInicioLocal = window.toLocalDateTimeString(dataInicialDate, horaInicioTexto);
                }

                console.log(`📅 Usando data do FORMULÁRIO: ${dataInicialStr}`);
            }
        }

        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
        const dataFinalDate = dataFinalValue ? new Date(dataFinalValue) : null;
        const dataFinalStr = dataFinalDate ? window.toDateOnlyString(dataFinalDate) : null;
        const horaFimTexto = window.getKendoTimeValue("txtHoraFinal") || null;

        // ============================================================
        // LÓGICA DE STATUS E FOIAGENDAMENTO
        // ============================================================
        // Verificar se TODOS os campos de finalização estão preenchidos
        const todosFinalPreenchidos = dataFinalStr && horaFimTexto && combustivelFinal && kmFinal;
        
        // Determinar status original
        const statusOriginal = agendamentoOriginal?.status;
        const statusAgendamentoOriginal = agendamentoOriginal?.statusAgendamento;
        
        // Verificar se é um agendamento (Status = 'Agendada' ou StatusAgendamento = true)
        const eraAgendamento = statusOriginal === "Agendada" || 
                              statusAgendamentoOriginal === true || 
                              statusAgendamentoOriginal === 1 ||
                              statusAgendamentoOriginal === "1" ||
                              statusAgendamentoOriginal === "true";
        
        // Definir novo status
        let novoStatus = statusOriginal;
        let novoStatusAgendamento = statusAgendamentoOriginal;
        let novoFoiAgendamento = agendamentoOriginal?.foiAgendamento ?? false;
        
        // Se todos os campos de finalização preenchidos → Realizada
        if (todosFinalPreenchidos)
        {
            novoStatus = "Realizada";
            novoStatusAgendamento = false;
            
            // Se era agendamento, marcar FoiAgendamento = true
            if (eraAgendamento)
            {
                novoFoiAgendamento = true;
                console.log("✅ Viagem finalizada a partir de Agendamento - FoiAgendamento = true");
            }
            
            console.log("✅ Todos campos de finalização preenchidos - Status = 'Realizada'");
        }

        // Montar payload de edição
        const payload = {
            ViagemId: agendamentoOriginal?.viagemId,
            DataInicial: dataInicialStr,
            HoraInicio: horaInicioLocal,
            DataFinal: dataFinalStr,
            HoraFim: horaFimTexto,
            Finalidade: finalidade,
            Origem: origem,
            Destino: destino,
            MotoristaId: motoristaId,
            VeiculoId: veiculoId,
            CombustivelInicial: combustivelInicial,
            CombustivelFinal: combustivelFinal,
            KmAtual: kmAtual,
            KmInicial: kmInicial,
            KmFinal: kmFinal,
            RequisitanteId: requisitanteId,
            RamalRequisitante: $("#txtRamalRequisitanteSF").val(),
            SetorSolicitanteId: setorId,
            Descricao: rteDescricaoHtmlContent,
            StatusAgendamento: novoStatusAgendamento,
            FoiAgendamento: novoFoiAgendamento,
            Status: novoStatus,
            EventoId: eventoId,
            Recorrente: agendamentoOriginal?.recorrente,
            RecorrenciaViagemId: agendamentoOriginal?.recorrenciaViagemId,
            //DatasSelecionadas: agendamentoOriginal?.datasSelecionadas,
            Intervalo: agendamentoOriginal?.intervalo,
            DataFinalRecorrencia: dataFinalRecorrenciaStr || agendamentoOriginal?.dataFinalRecorrencia,
            Monday: agendamentoOriginal?.monday,
            Tuesday: agendamentoOriginal?.tuesday,
            Wednesday: agendamentoOriginal?.wednesday,
            Thursday: agendamentoOriginal?.thursday,
            Friday: agendamentoOriginal?.friday,
            Saturday: agendamentoOriginal?.saturday,
            Sunday: agendamentoOriginal?.sunday,
            DiaMesRecorrencia: agendamentoOriginal?.diaMesRecorrencia,
            NoFichaVistoria: noFichaVistoria
        };

        return payload;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamentoEdicao", error);
        return null;
    }
};

/**
 * ðŸ“ Cria objeto de viagem (transformação de agendamento)
 * Converte um agendamento em viagem real (quando sai do status "Agendada")
 * param {Object} agendamentoUnicoAlterado - Agendamento base
 * returns {Object|null} Objeto de viagem ou null em caso de erro
 */
window.criarAgendamentoViagem = function (agendamentoUnicoAlterado)
{
    try
    {
        const rteDescricao = document.getElementById("rteDescricao").ej2_instances[0];
        const rteDescricaoHtmlContent = rteDescricao.getHtml();

        let motoristaId = document.getElementById("lstMotorista").ej2_instances[0].value;
        let veiculoId = document.getElementById("lstVeiculo").ej2_instances[0].value;

        // Processar evento
        let eventoId = null;
        const lstEventosInst = document.getElementById("lstEventos")?.ej2_instances?.[0];

        if (lstEventosInst?.value)
        {
            const eventosVal = lstEventosInst.value;

            // ✅ Tratar tanto array (MultiSelect) quanto valor único (ComboBox)
            if (Array.isArray(eventosVal) && eventosVal.length > 0)
            {
                eventoId = eventosVal[0]; // MultiSelect
            } else if (eventosVal)
            {
                eventoId = eventosVal; // ComboBox
            }
        }

        console.log("🎪 EventoId capturado:", eventoId);

        let setorId = document.getElementById("lstSetorRequisitanteAgendamento").ej2_instances[0].value[0];
        let ramal = $("#txtRamalRequisitanteSF").val();
        // Kendo ComboBox - obter valor
        const lstReqElement = document.getElementById("lstRequisitante");
        const lstReqKendoCB = lstReqElement ? $(lstReqElement).data("kendoComboBox") : null;
        let requisitanteId = lstReqKendoCB?.value() ?? null;

        console.log("🔍 DEBUG GRAVAÇÃO Requisitante (Registra Viagem):");
        console.log("  - lstReqElement encontrado:", lstReqElement ? "SIM" : "NÃO");
        console.log("  - lstReqKendoCB encontrado:", lstReqKendoCB ? "SIM" : "NÃO");
        console.log("  - requisitanteId extraído:", requisitanteId);
        let kmAtual = parseInt($("#txtKmAtual").val(), 10);
        let kmInicial = parseInt($("#txtKmInicial").val(), 10);
        let kmFinal = parseInt($("#txtKmFinal").val(), 10);
        // ✅ KENDO: Origem e Destino agora usam Kendo ComboBox
        let destino = $("#cmbDestino").data("kendoComboBox").value();
        let origem = $("#cmbOrigem").data("kendoComboBox").value();
        let finalidade = document.getElementById("lstFinalidade").ej2_instances[0].value[0];
        let combustivelInicial = document.getElementById("ddtCombustivelInicial").ej2_instances[0].value[0];

        // Combustí­vel final (opcional)
        let combustivelFinal = "";
        if (document.getElementById("ddtCombustivelFinal").ej2_instances[0].value[0] === null ||
            document.getElementById("ddtCombustivelFinal").ej2_instances[0].value[0] === undefined)
        {
            combustivelFinal = null;
        } else
        {
            combustivelFinal = document.getElementById("ddtCombustivelFinal").ej2_instances[0].value[0];
        }

        // Data final (opcional)
        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
        let dataFinal = dataFinalValue ? moment(dataFinalValue).format("YYYY-MM-DD") : null;

        let horaInicio = window.getKendoTimeValue("txtHoraInicial");

        // Hora fim (opcional)
        let horaFim = "";
        const horaFimValue = window.getKendoTimeValue("txtHoraFinal");
        if (!horaFimValue)
        {
            horaFim = null;
        } else
        {
            horaFim = horaFimValue;
        }

        let statusAgendamento = document.getElementById("txtStatusAgendamento").value;
        let criarViagemFechada = true;
        let noFichaVistoria = document.getElementById("txtNoFichaVistoria").value || 0;
        let status = "Aberta";

        // Ler Data Final Recorrência do formulário
        const dataFinalRecorrenciaValue2 = window.getKendoDateValue("txtFinalRecorrencia");
        let dataFinalRecorrenciaStr2 = null;
        if (dataFinalRecorrenciaValue2)
        {
            dataFinalRecorrenciaStr2 = moment(dataFinalRecorrenciaValue2).format("YYYY-MM-DD");
        }

        // Determinar status baseado nos campos preenchidos
        if (dataFinal && horaFim && combustivelFinal && kmFinal)
        {
            status = "Realizada";
            if (statusAgendamento)
            {
                criarViagemFechada = true;
            } else
            {
                criarViagemFechada = false;
            }
        }

        const agendamento = {
            ViagemId: window.viagemId,
            NoFichaVistoria: noFichaVistoria,
            DataInicial: window.dataInicial,
            HoraInicio: horaInicio,
            DataFinal: dataFinal,
            HoraFim: horaFim,
            Finalidade: finalidade,
            Origem: origem,
            Destino: destino,
            MotoristaId: motoristaId,
            VeiculoId: veiculoId,
            KmAtual: kmAtual,
            KmInicial: kmInicial,
            KmFinal: kmFinal,
            CombustivelInicial: combustivelInicial,
            CombustivelFinal: combustivelFinal,
            RequisitanteId: requisitanteId,
            RamalRequisitante: ramal,
            SetorSolicitanteId: setorId,
            Descricao: rteDescricaoHtmlContent,
            StatusAgendamento: false,
            FoiAgendamento: true,
            Status: status,
            EventoId: eventoId,
            Recorrente: agendamentoUnicoAlterado.recorrente,
            RecorrenciaViagemId: agendamentoUnicoAlterado.recorrenciaViagemId,
            //DatasSelecionadas: agendamentoUnicoAlterado.datasSelecionadas,
            Intervalo: agendamentoUnicoAlterado.intervalo,
            DataFinalRecorrencia: dataFinalRecorrenciaStr2 || agendamentoUnicoAlterado.dataFinalRecorrencia,
            Monday: agendamentoUnicoAlterado.monday,
            Tuesday: agendamentoUnicoAlterado.tuesday,
            Wednesday: agendamentoUnicoAlterado.wednesday,
            Thursday: agendamentoUnicoAlterado.thursday,
            Friday: agendamentoUnicoAlterado.friday,
            Saturday: agendamentoUnicoAlterado.saturday,
            Sunday: agendamentoUnicoAlterado.sunday,
            DiaMesRecorrencia: agendamentoUnicoAlterado.diaMesRecorrencia,
            CriarViagemFechada: criarViagemFechada
        };

        return agendamento;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "criarAgendamentoViagem", error);
        return null;
    }
};

// ====================================================================
// SEÇÃO 2: ENVIO E COMUNICAÇÃO COM API
// ====================================================================

/**
 * ðŸ“¤ Envia agendamento para API
 * Função base para todas as operações de criação/atualização
 * param {Object} agendamento - Objeto de agendamento
 * returns {Promise<Object>} Resultado da operação
 */
window.enviarAgendamento = async function (agendamento)
{
    try
    {
        // Evitar múltiplos envios simultâneos
        if (window.isSubmitting)
        {
            console.warn("âš ï¸ Tentativa de enviar enquanto outra requisição está em andamento.");
            return;
        }

        // VALIDAÇÃO: Data Final não pode ser superior à data atual
        if (agendamento.DataFinal)
        {
            const dataFinalDate = new Date(agendamento.DataFinal + "T00:00:00");
            const hoje = new Date();
            hoje.setHours(0, 0, 0, 0);
            if (dataFinalDate > hoje)
            {
                // Limpar campo Data Final no modal
                window.setKendoDateValue("txtDataFinal", null);
                AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
                return { success: false, message: "Data Final inválida" };
            }
        }

        window.isSubmitting = true;
        $("#btnConfirma").prop("disabled", true);

        try
        {
            const response = await $.ajax({
                type: "POST",
                url: "/api/Agenda/Agendamento",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(agendamento)
            });

            if (response?.success === true)
            {
                console.log("Agendamento enviado com sucesso.");
            } else
            {
                console.error("Erro ao enviar agendamento: operação mal sucedida.", response);
                throw new Error("Erro ao criar agendamento. Operação mal sucedida.");
            }

            response.operacaoBemSucedida = true;
            return response;
        } catch (error)
        {
            if (error.statusText)
            {
                // Ã‰ um erro AJAX
                const erroAjax = window.criarErroAjax(error, error.statusText, error.responseText, { url: "/api/Agenda/Agendamento", type: "POST" });
                Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamento", erroAjax);
            } else
            {
                Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamento", error);
            }
            throw error;
        } finally
        {
            window.isSubmitting = false;
            $("#btnConfirma").prop("disabled", false);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamento", error);
        throw error;
    }
};

/**
 * ðŸ“¤ Envia novo agendamento
 * Wrapper para envio com feedback de sucesso
 * param {Object} agendamento - Objeto de agendamento
 * param {boolean} isUltimoAgendamento - Se é o último da série
 * returns {Promise<Object>} Resultado da operação
 */
window.enviarNovoAgendamento = async function (agendamento, isUltimoAgendamento = true)
{
    try
    {
        try
        {
            const objViagem = await window.enviarAgendamento(agendamento);

            if (!objViagem.operacaoBemSucedida)
            {
                console.error("âŒ Erro ao criar novo agendamento: operação não bem-sucedida", objViagem);
                throw new Error("Erro ao criar novo agendamento");
            }

            // Mostrar feedback apenas no último agendamento de uma série
            if (isUltimoAgendamento)
            {
                window.exibirMensagemSucesso();
            }

            return objViagem;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarNovoAgendamento_inner", error);
            throw error;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarNovoAgendamento", error);
        throw error;
    }
};

/**
 * ðŸ“¤ Envia agendamento com opções de edição
 * Usado para editar agendamentos recorrentes (editar todos ou apenas próximos)
 * param {string} viagemId - ID da viagem
 * param {boolean} editarTodos - Editar todos os recorrentes
 * param {boolean} editarProximos - Editar próximos
 * param {string} dataInicial - Data inicial
 * param {string} viagemIdRecorrente - ID da recorrência
 */
window.enviarAgendamentoComOpcao = async function (viagemId, editarTodos, editarProximos, dataInicial = null, viagemIdRecorrente = null)
{
    try
    {
        try
        {
            if (!dataInicial)
            {
                dataInicial = moment().format("YYYY-MM-DD");
            }

            const agendamento = window.criarAgendamento(viagemId, viagemIdRecorrente, dataInicial);

            agendamento.EditarTodos = editarTodos;
            agendamento.EditarProximos = editarProximos;

            const objViagem = await window.enviarAgendamento(agendamento);

            if (objViagem)
            {
                AppToast.show("Verde", "Agendamento atualizado com sucesso", 3000);
                $("#modalViagens").modal("hide");
                $(document.body).removeClass("modal-open");
                $(".modal-backdrop").remove();
                $(document.body).css("overflow", "");
                window.calendar.refetchEvents();
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamentoComOpcao_inner", error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "enviarAgendamentoComOpcao", error);
    }
};

/**
 * ðŸ”„ Aplica atualização em agendamento
 * Envia alterações para o servidor usando Fetch API
 * param {Object} objViagem - Objeto de viagem
 * returns {Promise<boolean>} Sucesso da operação
 */
window.aplicarAtualizacao = async function (objViagem)
{
    try
    {
        const response = await fetch("/api/Agenda/Agendamento", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(objViagem)
        });

        const data = await response.json();

        if (data?.success || data?.data)
        {
            AppToast.show("Verde", data.message || "Agendamento Atualizado", 2000);
            return true;
        } else
        {
            AppToast.show("Vermelho", data?.message || "Falha ao atualizar agendamento", 2000);
            return false;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "aplicarAtualizacao", error);
        return false;
    }
};

// ====================================================================
// SEÇÃO 3: RECUPERAÇÃO E CONSULTA DE DADOS
// ====================================================================

/**
 * ðŸ” Recupera viagem para edição
 * Busca dados completos da viagem do servidor
 * param {string} viagemId - ID da viagem
 * returns {Promise<Object|null>} Dados da viagem ou null
 */
window.recuperarViagemEdicao = async function (viagemId)
{
    try
    {
        const result = await window.AgendamentoService.obterParaEdicao(viagemId);

        if (result.success)
        {
            console.log("DEBUG - Dados carregados do banco:", result.data);
            console.log("DEBUG - dataFinalRecorrencia:", result.data.dataFinalRecorrencia);
            return result.data;
        } else
        {
            throw new Error(result.error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "recuperarViagemEdicao", error);
        return null;
    }
};

/**
 * ðŸ” Obtém agendamentos recorrentes para exclusão
 * Busca todos os agendamentos de uma série recorrente
 * param {string} recorrenciaViagemId - ID da recorrência
 * returns {Promise<Array>} Lista de agendamentos
 */
window.obterAgendamentosRecorrentes = async function (recorrenciaViagemId)
{
    try
    {
        const result = await window.AgendamentoService.obterRecorrentes(recorrenciaViagemId);

        if (result.success)
        {
            return result.data;
        } else
        {
            throw new Error(result.error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "obterAgendamentosRecorrentes", error);
        return [];
    }
};

/**
 * ðŸ” Obtém agendamento inicial de recorrência
 * Busca o primeiro agendamento de uma série recorrente
 * param {string} viagemId - ID da viagem
 * returns {Promise<Array>} Lista com agendamento inicial
 */
window.obterAgendamentosRecorrenteInicial = async function (viagemId)
{
    try
    {
        const result = await window.AgendamentoService.obterRecorrenteInicial(viagemId);

        if (result.success)
        {
            return result.data;
        } else
        {
            throw new Error(result.error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "obterAgendamentosRecorrenteInicial", error);
        return [];
    }
};

// ====================================================================
// SEÇÃO 4: EXCLUSÃO E CANCELAMENTO
// ====================================================================

/**
 * ðŸ—‘ï¸ Exclui agendamento
 * Remove completamente o agendamento do sistema
 * param {string} viagemId - ID da viagem
 */
window.excluirAgendamento = async function (viagemId)
{
    try
    {
        const result = await window.AgendamentoService.excluir(viagemId);

        if (result.success)
        {
            // Sucesso já tratado no service
        } else
        {
            AppToast.show("Vermelho", result.message, 2000);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "excluirAgendamento", error);
    }
};

/**
 * âŒ Cancela agendamento
 * Muda status para cancelado (mantém no banco para histórico)
 * param {string} viagemId - ID da viagem
 * param {string} descricao - Descrição do cancelamento
 * param {boolean} mostrarToast - Se deve mostrar toast
 * returns {Promise<Object>} Resultado da operação
 */
window.cancelarAgendamento = async function (viagemId, descricao, mostrarToast = true)
{
    try
    {
        const result = await window.AgendamentoService.cancelar(viagemId, descricao);

        if (result.success)
        {
            if (mostrarToast)
            {
                AppToast.show("Verde", "O agendamento foi cancelado com sucesso!", 2000);
            }
            return result;
        } else
        {
            AppToast.show("Vermelho", result.message, 2000);
            return result;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "cancelarAgendamento", error);
        return { success: false, error: error.message };
    }
};

// ====================================================================
// SEÇÃO 4.5: ALTERAÇÃO DE DATA INICIAL (NOVA FUNCIONALIDADE)
// ====================================================================

/**
 * ðŸ—“ï¸ Detecta se houve alteração na Data Inicial
 * param {Object} agendamentoOriginal - Dados originais do banco
 * returns {Object} { alterou: boolean, dataOriginal: Date, dataNova: Date }
 */
function detectarAlteracaoDataInicial(agendamentoOriginal)
{
    try
    {
        // Obter data original do banco
        const dataOriginalStr = agendamentoOriginal?.dataInicial;
        if (!dataOriginalStr)
        {
            return { alterou: false, dataOriginal: null, dataNova: null };
        }

        const dataOriginal = new Date(dataOriginalStr);
        dataOriginal.setHours(0, 0, 0, 0);

        // Obter data atual do formulário
        const dataNovaValue = window.getKendoDateValue("txtDataInicial");
        if (!dataNovaValue)
        {
            return { alterou: false, dataOriginal: null, dataNova: null };
        }

        const dataNova = new Date(dataNovaValue);
        dataNova.setHours(0, 0, 0, 0);

        // Comparar timestamps
        const alterou = dataOriginal.getTime() !== dataNova.getTime();

        console.log("ðŸ“… [DataInicial] Detecção de alteração:", {
            dataOriginal: dataOriginal.toLocaleDateString('pt-BR'),
            dataNova: dataNova.toLocaleDateString('pt-BR'),
            alterou: alterou
        });

        return {
            alterou: alterou,
            dataOriginal: dataOriginal,
            dataNova: dataNova,
            dataOriginalStr: dataOriginalStr,
            dataNovaStr: window.toDateOnlyString(dataNova)
        };
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "detectarAlteracaoDataInicial", error);
        return { alterou: false, dataOriginal: null, dataNova: null };
    }
}

/**
 * ðŸ”€ Calcula "push" de datas para agendamentos subsequentes
 * param {Date} dataOriginal - Data original
 * param {Date} dataNova - Data nova escolhida
 * param {string} intervalo - Tipo de recorrência (D, S, Q, M)
 * returns {number} Quantidade de dias/semanas/meses a avançar
 */
function calcularPushDatas(dataOriginal, dataNova, intervalo)
{
    try
    {
        const diffDias = Math.floor((dataNova - dataOriginal) / (1000 * 60 * 60 * 24));

        console.log("ðŸ“Š [Push] Diferença em dias:", diffDias);

        switch (intervalo)
        {
            case "D": // Diário
                return diffDias;

            case "S": // Semanal
                return Math.floor(diffDias / 7);

            case "Q": // Quinzenal
                return Math.floor(diffDias / 14);

            case "M": // Mensal
                const mOriginal = moment(dataOriginal);
                const mNova = moment(dataNova);
                return mNova.diff(mOriginal, 'months');

            default:
                console.warn("âš ï¸ Intervalo não reconhecido:", intervalo);
                return 0;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "calcularPushDatas", error);
        return 0;
    }
}

/**
 * ðŸ”„ Aplica "push" nas datas de agendamentos subsequentes
 * param {string} recorrenciaViagemId - ID da recorrência
 * param {Date} dataOriginal - Data original
 * param {Date} dataNova - Nova data
 * param {string} intervalo - Tipo de intervalo (D, S, Q, M)
 * param {Date} dataReferencia - Data a partir da qual aplicar o push
 * returns {Promise<boolean>} Sucesso da operação
 */
async function aplicarPushDatasSubsequentes(recorrenciaViagemId, dataOriginal, dataNova, intervalo, dataReferencia)
{
    try
    {
        console.log("ðŸ”„ [Push] Iniciando aplicação de push nas datas subsequentes...");

        // Buscar todos os agendamentos da recorrência
        const agendamentos = await window.obterAgendamentosRecorrentes(recorrenciaViagemId);

        if (!agendamentos || agendamentos.length === 0)
        {
            console.warn("âš ï¸ Nenhum agendamento recorrente encontrado");
            return false;
        }

        // Calcular unidades de push
        const pushUnidades = calcularPushDatas(dataOriginal, dataNova, intervalo);

        console.log("ðŸ“Š [Push] Unidades a avançar:", pushUnidades, "no intervalo:", intervalo);

        let contadorSucesso = 0;
        let contadorErro = 0;

        // Filtrar apenas agendamentos com data >= dataReferencia
        const agendamentosFiltrados = agendamentos.filter(ag =>
        {
            const dataAg = new Date(ag.dataInicial);
            dataAg.setHours(0, 0, 0, 0);
            return dataAg.getTime() >= dataReferencia.getTime();
        });

        console.log(`ðŸ“‹ [Push] Total de agendamentos a atualizar: ${agendamentosFiltrados.length}`);

        // Aplicar push em cada agendamento
        for (const agendamento of agendamentosFiltrados)
        {
            try
            {
                const dataAtual = moment(agendamento.dataInicial);
                let novaData;

                // Aplicar push conforme o intervalo
                switch (intervalo)
                {
                    case "D": // Diário
                        novaData = dataAtual.add(pushUnidades, 'days');
                        break;

                    case "S": // Semanal
                        novaData = dataAtual.add(pushUnidades, 'weeks');
                        break;

                    case "Q": // Quinzenal
                        novaData = dataAtual.add(pushUnidades * 2, 'weeks');
                        break;

                    case "M": // Mensal
                        novaData = dataAtual.add(pushUnidades, 'months');
                        break;

                    default:
                        console.warn("âš ï¸ Intervalo inválido:", intervalo);
                        continue;
                }

                // Criar payload de atualização MANUALMENTE (sem spread operator)
                const payload = {
                    ViagemId: agendamento.viagemId,
                    DataInicial: novaData.format("YYYY-MM-DD"),
                    HoraInicio: agendamento.horaInicio,
                    DataFinal: agendamento.dataFinal,
                    HoraFim: agendamento.horaFim,
                    Finalidade: agendamento.finalidade,
                    Origem: agendamento.origem,
                    Destino: agendamento.destino,
                    MotoristaId: agendamento.motoristaId,
                    VeiculoId: agendamento.veiculoId,
                    CombustivelInicial: agendamento.combustivelInicial,
                    CombustivelFinal: agendamento.combustivelFinal,
                    KmAtual: agendamento.kmAtual,
                    KmInicial: agendamento.kmInicial,
                    KmFinal: agendamento.kmFinal,
                    RequisitanteId: agendamento.requisitanteId,
                    RamalRequisitante: agendamento.ramalRequisitante,
                    SetorSolicitanteId: agendamento.setorSolicitanteId,
                    Descricao: agendamento.descricao,
                    StatusAgendamento: agendamento.statusAgendamento,
                    FoiAgendamento: agendamento.foiAgendamento,
                    Status: agendamento.status,
                    EventoId: agendamento.eventoId,
                    Recorrente: agendamento.recorrente,
                    RecorrenciaViagemId: agendamento.recorrenciaViagemId,
                    //DatasSelecionadas: agendamento.datasSelecionadas,
                    Intervalo: agendamento.intervalo,
                    DataFinalRecorrencia: agendamento.dataFinalRecorrencia,
                    Monday: agendamento.monday,
                    Tuesday: agendamento.tuesday,
                    Wednesday: agendamento.wednesday,
                    Thursday: agendamento.thursday,
                    Friday: agendamento.friday,
                    Saturday: agendamento.saturday,
                    Sunday: agendamento.sunday,
                    DiaMesRecorrencia: agendamento.diaMesRecorrencia,
                    NoFichaVistoria: agendamento.noFichaVistoria
                };

                // Enviar atualização
                const sucesso = await window.aplicarAtualizacao(payload);

                if (sucesso)
                {
                    contadorSucesso++;
                    console.log(`âœ… [Push] Agendamento ${agendamento.viagemId} atualizado para ${novaData.format("DD/MM/YYYY")}`);
                } else
                {
                    contadorErro++;
                    console.error(`âŒ [Push] Falha ao atualizar ${agendamento.viagemId}`);
                }
            } catch (error)
            {
                contadorErro++;
                console.error(`âŒ [Push] Erro ao processar agendamento:`, error);
                Alerta.TratamentoErroComLinha("modal-viagem.js", "aplicarPushDatasSubsequentes_loop", error);
            }
        }

        console.log(`ðŸ“Š [Push] Resultado: ${contadorSucesso} sucessos, ${contadorErro} erros`);

        return contadorErro === 0;
    } catch (error)
    {
        console.error("âŒ [Push] Erro geral:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "aplicarPushDatasSubsequentes", error);
        return false;
    }
}

/**
 * â“ Pergunta ao usuário sobre alteração de datas recorrentes
 * Usa Alerta.Confirmar3 para 3 opções
 * param {string} dataOriginalStr - Data original formatada
 * param {string} dataNovaStr - Nova data formatada
 * returns {Promise<string>} "apenas_este" | "todos_subsequentes" | "cancelar"
 */
async function perguntarAlteracaoRecorrente(dataOriginalStr, dataNovaStr)
{
    try
    {
        const mensagem = `
            <div class="text-start">
                <p><strong>Você está alterando a Data Inicial de um agendamento recorrente:</strong></p>
                <ul class="mb-3">
                    <li>Data Original: <strong>${dataOriginalStr}</strong></li>
                    <li>Nova Data: <strong class="text-primary">${dataNovaStr}</strong></li>
                </ul>
                <p class="mb-2">Como deseja proceder?</p>
            </div>
        `;

        const resultado = await Alerta.Confirmar3(
            "Alteração de Data Inicial",
            mensagem,
            "Alterar apenas este",          // Botão 1 (Azul)
            "Alterar este e subsequentes",   // Botão 2 (Verde)
            "Cancelar operação"              // Botão 3 (Vermelho)
        );

        console.log("ðŸ¤” [Pergunta] Resposta do usuário:", resultado);

        // Mapear resultado do Alerta.Confirmar3
        switch (resultado)
        {
            case 1:
                return "apenas_este";
            case 2:
                return "todos_subsequentes";
            case 3:
            default:
                return "cancelar";
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "perguntarAlteracaoRecorrente", error);
        return "cancelar";
    }
}

/**
 * ðŸ”§ Processa alteração de Data Inicial em edição
 * Função principal que coordena toda a lógica
 * param {Object} agendamentoOriginal - Dados originais do banco
 * param {Object} agendamentoEditado - Dados editados do formulário
 * returns {Promise<Object>} { sucesso: boolean, agendamentoFinal: Object }
 */
async function processarAlteracaoDataInicial(agendamentoOriginal, agendamentoEditado)
{
    try
    {
        console.log("ðŸ”§ [ProcessarData] Iniciando processamento...");

        // 1. Detectar se houve alteração
        const deteccao = detectarAlteracaoDataInicial(agendamentoOriginal);

        if (!deteccao.alterou)
        {
            console.log("â„¹ï¸ [ProcessarData] Data não foi alterada, seguindo fluxo normal");
            return {
                sucesso: true,
                agendamentoFinal: agendamentoEditado,
                precisaRecarregar: false
            };
        }

        // 2. Verificar se o status permite alteração
        const status = agendamentoOriginal?.status || "";
        if (status !== "Aberta" && status !== "Agendada")
        {
            console.warn("âš ï¸ [ProcessarData] Status não permite alteração de data:", status);
            AppToast.show("Amarelo", "Não é possível alterar a data de viagens com status '" + status + "'", 3000);
            return {
                sucesso: false,
                agendamentoFinal: null,
                precisaRecarregar: false
            };
        }

        // 3. Verificar se é recorrente
        const isRecorrente = agendamentoOriginal?.recorrente === "S" || agendamentoOriginal?.recorrente === "M" ||
            agendamentoOriginal?.recorrente === "Q" || agendamentoOriginal?.recorrente === "D";
        const intervalo = agendamentoOriginal?.intervalo || "";
        const recorrenciaViagemId = agendamentoOriginal?.recorrenciaViagemId || "";

        // 4. Se não é recorrente OU é recorrência variada (V), permite alteração direta
        if (!isRecorrente || intervalo === "V")
        {
            console.log("â„¹ï¸ [ProcessarData] Não é recorrente ou é variada, permitindo alteração direta");
            return {
                sucesso: true,
                agendamentoFinal: agendamentoEditado,
                precisaRecarregar: false
            };
        }

        // 5. Ã‰ recorrente e NÃO é variada - perguntar ao usuário
        console.log("â“ [ProcessarData] Ã‰ recorrente, perguntando ao usuário...");

        const dataOriginalFormatada = deteccao.dataOriginal.toLocaleDateString('pt-BR');
        const dataNovaFormatada = deteccao.dataNova.toLocaleDateString('pt-BR');

        const escolha = await perguntarAlteracaoRecorrente(dataOriginalFormatada, dataNovaFormatada);

        console.log("âœ… [ProcessarData] Escolha do usuário:", escolha);

        if (escolha === "cancelar")
        {
            // Usuário cancelou - não fazer nada
            console.log("ðŸš« [ProcessarData] Operação cancelada pelo usuário");
            return {
                sucesso: false,
                agendamentoFinal: null,
                precisaRecarregar: false
            };
        }

        if (escolha === "apenas_este")
        {
            // Alterar apenas este agendamento
            console.log("âœï¸ [ProcessarData] Alterando apenas este agendamento");
            return {
                sucesso: true,
                agendamentoFinal: agendamentoEditado,
                precisaRecarregar: false
            };
        }

        if (escolha === "todos_subsequentes")
        {
            // Alterar este e aplicar push nos subsequentes
            console.log("ðŸ”„ [ProcessarData] Alterando este e aplicando push nos subsequentes");

            // Aplicar push
            const pushSucesso = await aplicarPushDatasSubsequentes(
                recorrenciaViagemId,
                deteccao.dataOriginal,
                deteccao.dataNova,
                intervalo,
                deteccao.dataOriginal
            );

            if (pushSucesso)
            {
                console.log("âœ… [ProcessarData] Push aplicado com sucesso");
                AppToast.show("Verde", "Data inicial atualizada em todos os agendamentos subsequentes", 3000);
            } else
            {
                console.warn("âš ï¸ [ProcessarData] Push teve erros, mas prosseguindo");
                AppToast.show("Amarelo", "Alguns agendamentos não puderam ser atualizados", 3000);
            }

            return {
                sucesso: true,
                agendamentoFinal: agendamentoEditado,
                precisaRecarregar: true
            };
        }

        // Caso não reconhecido (não deveria chegar aqui)
        console.warn("âš ï¸ [ProcessarData] Escolha não reconhecida:", escolha);
        return {
            sucesso: false,
            agendamentoFinal: null,
            precisaRecarregar: false
        };
    } catch (error)
    {
        console.error("âŒ [ProcessarData] Erro:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "processarAlteracaoDataInicial", error);
        return {
            sucesso: false,
            agendamentoFinal: null,
            precisaRecarregar: false
        };
    }
}

// ====================================================================
// SEÇÃO 5: EDIÇÃO DE AGENDAMENTOS
// ====================================================================

/**
 * âœï¸ Edita agendamento único
 * Atualiza agendamento que não faz parte de série recorrente
 * param {string} viagemId - ID da viagem
 */
window.editarAgendamento = async function (viagemId)
{
    try
    {
        if (!viagemId)
        {
            throw new Error("ViagemId é obrigatório.");
        }

        try
        {
            // Buscar dados originais
            const agendamentoBase = await window.recuperarViagemEdicao(viagemId);

            if (!agendamentoBase)
            {
                throw new Error("Agendamento inexistente.");
            }

            // Criar objeto com alterações
            const agendamentoEditado = window.criarAgendamentoEdicao(agendamentoBase);

            // NOVA LÃ“GICA: Processar alteração de data inicial
            const resultadoProcessamento = await processarAlteracaoDataInicial(agendamentoBase, agendamentoEditado);

            if (!resultadoProcessamento.sucesso)
            {
                console.log("ðŸš« [EditarAgendamento] Operação não prosseguiu");
                return;
            }

            const agendamentoFinal = resultadoProcessamento.agendamentoFinal;

            // Validar e enviar
            if (await window.ValidaCampos(agendamentoFinal.ViagemId))
            {
                const response = await fetch("/api/Agenda/Agendamento", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(agendamentoFinal)
                });

                // Determinar tipo para feedback
                let tipoAgendamento = "Viagem";
                if (agendamentoFinal.Status === "Aberta")
                {
                    tipoAgendamento = "Viagem";
                } else
                {
                    tipoAgendamento = "Agendamento";
                }

                const resultado = await response.json();

                if (resultado.success)
                {
                    AppToast.show("Verde", tipoAgendamento + " atualizado com sucesso!", 2000);

                    // Fechar modal
                    $("#modalViagens").modal("hide");
                    $(document.body).removeClass("modal-open");
                    $(".modal-backdrop").remove();
                    $(document.body).css("overflow", "");
                } else
                {
                    AppToast.show("Vermelho", "Erro ao atualizar " + tipoAgendamento, 2000);
                }

                // Atualizar calendário
                if (window.calendar?.refetchEvents)
                {
                    window.calendar.refetchEvents();
                }
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamento_inner", error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamento", error);
    }
};

/**
 * âœï¸ Edita agendamento recorrente
 * Atualiza agendamentos de uma série recorrente (todos ou a partir de data)
 * param {string} viagemId - ID da viagem
 * param {boolean} editaTodos - Se edita todos
 * param {string} dataInicialRecorrencia - Data inicial da recorrência
 * param {string} recorrenciaViagemId - ID da recorrência
 * param {boolean} editarAgendamentoRecorrente - Flag de edição
 */
window.editarAgendamentoRecorrente = async function (viagemId, editaTodos, dataInicialRecorrencia, recorrenciaViagemId, editarAgendamentoRecorrente)
{
    try
    {
        /**
         * Compara se uma data é igual ou posterior a outra (ignora hora)
         */
        const isSameOrAfterDay = (left, right) =>
        {
            try
            {
                const L = window.toLocalDateOnly(left);
                const R = window.toLocalDateOnly(right);
                if (!L || !R) return false;
                return L.getTime() >= R.getTime();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("modal-viagem.js", "isSameOrAfterDay", error);
                return false;
            }
        };

        /**
         * Fecha modal com sucesso e atualiza calendário
         */
        const fecharModalComSucesso = () =>
        {
            try
            {
                try
                {
                    $("#modalViagens").modal("hide");
                } catch { }
                $(".modal-backdrop").remove();
                $("body").removeClass("modal-open").css("overflow", "");
                if (window.calendar?.refetchEvents) window.calendar.refetchEvents();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("modal-viagem.js", "fecharModalComSucesso", error);
            }
        };

        try
        {
            if (!viagemId) throw new Error("ViagemId não fornecido.");

            let houveSucesso = false;

            if (editaTodos)
            {
                // Editar todos os agendamentos da série
                if (recorrenciaViagemId === "00000000-0000-0000-0000-000000000000" || !recorrenciaViagemId)
                {
                    recorrenciaViagemId = viagemId;
                    const [primeiroDaSerie = {}] = await window.obterAgendamentosRecorrenteInicial(viagemId);
                    let objViagem = window.criarAgendamentoEdicao(primeiroDaSerie);

                    objViagem.editarTodosRecorrentes = true;
                    objViagem.editarAPartirData = dataInicialRecorrencia;
                    const ok = await window.aplicarAtualizacao(objViagem);
                    houveSucesso = houveSucesso || ok;
                }

                // Buscar e atualizar todos os agendamentos da série
                const agendamentos = await window.obterAgendamentosRecorrentes(recorrenciaViagemId);
                for (const agendamentoRecorrente of agendamentos)
                {
                    if (isSameOrAfterDay(agendamentoRecorrente.dataInicial, dataInicialRecorrencia))
                    {
                        let objViagem = window.criarAgendamentoEdicao(agendamentoRecorrente);
                        const ok = await window.aplicarAtualizacao(objViagem);
                        houveSucesso = houveSucesso || ok;
                    }
                }
            } else
            {
                // Editar apenas este agendamento
                const agendamentoUnicoAlterado = await window.recuperarViagemEdicao(viagemId);
                let objViagem = window.criarAgendamentoEdicao(agendamentoUnicoAlterado);
                const ok = await window.aplicarAtualizacao(objViagem);
                houveSucesso = houveSucesso || ok;
            }

            if (houveSucesso) fecharModalComSucesso();
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamentoRecorrente_inner", error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "editarAgendamentoRecorrente", error);
    }
};

// ====================================================================
// SEÇÃO 6: FEEDBACK E MENSAGENS
// ====================================================================

/**
 * âœ… Exibe mensagem de sucesso e fecha modal
 * Usado após criação bem-sucedida de agendamentos
 */
window.exibirMensagemSucesso = function ()
{
    try
    {
        AppToast.show("Verde", "Todos os agendamentos foram criados com sucesso", 3000);
        Alerta.Sucesso("Agendamento criado com sucesso", "Todos os agendamentos foram criados com sucesso");
        $("#modalViagens").modal("hide");
        $(document.body).removeClass("modal-open");
        $(".modal-backdrop").remove();
        $(document.body).css("overflow", "");
        window.calendar.refetchEvents();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "exibirMensagemSucesso", error);
    }
};

/**
 * âŒ Exibe erro ao criar agendamento
 * Feedback visual quando falha a criação
 */
window.exibirErroAgendamento = function ()
{
    try
    {
        AppToast.show("Vermelho", "Não foi possível criar o agendamento com os dados informados", 3000);
        Alerta.Erro("Erro ao criar agendamento", "Não foi possível criar o agendamento com os dados informados");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "exibirErroAgendamento", error);
    }
};

/**
 * âš ï¸ Handler de erro de agendamento
 * Ponto central para tratamento de erros de agendamento
 * param {Error} error - Erro
 */
window.handleAgendamentoError = function (error)
{
    try
    {
        window.exibirErroAgendamento();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "handleAgendamentoError", error);
    }
};

// ====================================================================
// SEÇÃO 7: INTEGRAÇÃO COM RELAtÓRIO (VERSÃO MELHORADA)
// ====================================================================

/**
 * ðŸ“Š Carrega o relatório no modal
 * Integração com o módulo de relatório (relatorio.js)
 * Busca o ViagemId e exibe o relatório da ficha de vistoria
 */
window.carregarRelatorioNoModal = function ()
{
    try
    {
        console.log("ðŸ“Š [ModalViagem] ===== INICIANDO CARREGAMENTO DE RELAtÓRIO =====");

        // Buscar ViagemId de diferentes fontes
        const viagemId = window.State?.get('viagemAtual')?.viagemId ||
            $('#txtViagemIdRelatorio').val() ||
            $('#txtViagemId').val() ||
            window.currentViagemId ||
            window.viagemId;

        console.log("ðŸ” [ModalViagem] Fontes de ViagemId:", {
            state: window.State?.get('viagemAtual')?.viagemId,
            txtViagemIdRelatorio: $('#txtViagemIdRelatorio').val(),
            txtViagemId: $('#txtViagemId').val(),
            currentViagemId: window.currentViagemId,
            viagemId: window.viagemId,
            final: viagemId
        });

        if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000')
        {
            console.error("âŒ [ModalViagem] ViagemId não encontrado ou inválido:", viagemId);

            if (typeof AppToast !== 'undefined')
            {
                AppToast.show('Amarelo', 'ID da viagem não identificado', 3000);
            }

            return;
        }

        console.log("âœ… [ModalViagem] ViagemId válido encontrado:", viagemId);

        // Verificar se o módulo de relatório existe
        if (typeof window.carregarRelatorioViagem !== 'function')
        {
            console.error("âŒ [ModalViagem] Função carregarRelatorioViagem não encontrada!");
            console.error("    Verifique se relatorio.js está carregado");

            if (typeof AppToast !== 'undefined')
            {
                AppToast.show('Vermelho', 'Módulo de relatório não carregado', 3000);
            }

            return;
        }

        console.log("âœ… [ModalViagem] Módulo de relatório encontrado");

        // Verificar se o container do relatório existe
        const reportContainer = document.getElementById('reportViewerAgenda');
        if (!reportContainer)
        {
            console.error("âŒ [ModalViagem] Container #reportViewerAgenda não encontrado no DOM");

            if (typeof AppToast !== 'undefined')
            {
                AppToast.show('Vermelho', 'Container do relatório não encontrado', 3000);
            }

            return;
        }

        console.log("âœ… [ModalViagem] Container do relatório encontrado");

        // Mostrar o card do relatório
        const cardRelatorio = $('#cardRelatorio');
        const reportContainerDiv = $('#ReportContainerAgenda');

        if (cardRelatorio.length > 0)
        {
            console.log("ðŸ“º [ModalViagem] Exibindo card do relatório");
            cardRelatorio.slideDown(300);
        }

        if (reportContainerDiv.length > 0)
        {
            console.log("ðŸ“º [ModalViagem] Exibindo container do relatório");
            reportContainerDiv.slideDown(300);
        }

        // Aguardar um pouco para garantir que o DOM está pronto
        setTimeout(() =>
        {
            console.log("ðŸš€ [ModalViagem] Chamando carregarRelatorioViagem com ViagemId:", viagemId);

            // Scroll suave até o relatório
            const card = document.getElementById('cardRelatorio');
            if (card)
            {
                card.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }

            // Chamar a função de carregamento
            window.carregarRelatorioViagem(viagemId)
                .then(() =>
                {
                    console.log("âœ… [ModalViagem] Relatório carregado com sucesso");

                    if (typeof AppToast !== 'undefined')
                    {
                        AppToast.show('Verde', 'Relatório carregado com sucesso', 2000);
                    }
                })
                .catch((error) =>
                {
                    console.error("âŒ [ModalViagem] Erro ao carregar relatório:", error);

                    if (typeof AppToast !== 'undefined')
                    {
                        AppToast.show('Vermelho', 'Erro ao carregar relatório: ' + error.message, 3000);
                    }
                });
        }, 500); // Aguardar 500ms para garantir que o DOM está pronto
    } catch (error)
    {
        console.error("âŒ [ModalViagem] Erro crí­tico em carregarRelatorioNoModal:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "carregarRelatorioNoModal", error);

        if (typeof AppToast !== 'undefined')
        {
            AppToast.show('Vermelho', 'Erro ao inicializar relatório', 3000);
        }
    }
};

// Variável para rastrear último ID carregado
window.ultimoViagemIdCarregado = null;

/**
 * ðŸ“‚ Event handler para quando o modal é aberto
 */
function aoAbrirModalViagem(event)
{
    try
    {
        console.log("ðŸ“‚ [ModalViagem] ===== MODAL ABERTO =====");

        // Resetar flags
        window.modalJaFoiLimpo = false;
        window.ignorarEventosRecorrencia = false;

        // Inicializar Kendo ComboBox para Origem e Destino (se ainda não foram inicializados)
        if (!$("#cmbOrigem").data("kendoComboBox"))
        {
            $("#cmbOrigem").kendoComboBox({
                dataSource: window.dataOrigem || [],
                filter: "contains",
                placeholder: "Selecione ou digite a origem",
                height: 220,
                suggest: true
            });
            console.log("✅ [ModalViagem] Kendo ComboBox cmbOrigem inicializado");
        }

        if (!$("#cmbDestino").data("kendoComboBox"))
        {
            $("#cmbDestino").kendoComboBox({
                dataSource: window.dataDestino || [],
                filter: "contains",
                placeholder: "Selecione ou digite o destino",
                height: 220,
                suggest: true
            });
            console.log("✅ [ModalViagem] Kendo ComboBox cmbDestino inicializado");
        }

        // Inicializar Fuzzy Validator para Origem/Destino (com delay para garantir que controles estejam prontos)
        setTimeout(() =>
        {
            if (typeof KendoFuzzyValidator !== 'undefined')
            {
                try
                {
                    KendoFuzzyValidator.init({
                        origemId: 'cmbOrigem',
                        destinoId: 'cmbDestino',
                        timeout: 200
                    });
                    console.log("✅ [ModalViagem] Fuzzy Validator inicializado");
                } catch (error)
                {
                    console.warn("⚠️ [ModalViagem] Erro ao inicializar Fuzzy Validator:", error);
                }
            }
        }, 300);

        // Buscar ViagemId
        const viagemId = $('#txtViagemId').val() ||
            $('#txtViagemIdRelatorio').val() ||
            window.currentViagemId;

        console.log("ðŸ“‹ [ModalViagem] ViagemId encontrado:", viagemId);
        console.log("ðŸ“‹ [ModalViagem] Último ViagemId carregado:", window.ultimoViagemIdCarregado);

        // Se houver ViagemId válido e for diferente do último carregado
        if (viagemId && viagemId !== "" && viagemId !== "00000000-0000-0000-0000-000000000000")
        {
            // Verificar se é um ID diferente do último carregado
            if (viagemId !== window.ultimoViagemIdCarregado)
            {
                console.log("ðŸ“Š [ModalViagem] ViagemId diferente, recarregando relatório...");

                // Destruir viewer anterior primeiro
                if (typeof destruirViewerAnterior === 'function')
                {
                    destruirViewerAnterior().then(() =>
                    {
                        // Aguardar e carregar novo relatório
                        setTimeout(() =>
                        {
                            if (typeof window.carregarRelatorioViagem === 'function')
                            {
                                window.carregarRelatorioViagem(viagemId);
                                $("#cardRelatorio").show();
                                window.ultimoViagemIdCarregado = viagemId;
                            }
                        }, 300);
                    });
                } else
                {
                    // Fallback se a função não existir
                    setTimeout(() =>
                    {
                        if (typeof window.carregarRelatorioViagem === 'function')
                        {
                            window.carregarRelatorioViagem(viagemId);
                            $("#cardRelatorio").show();
                            window.ultimoViagemIdCarregado = viagemId;
                        }
                    }, 500);
                }
            } else
            {
                console.log("ðŸ“Š [ModalViagem] Mesmo ViagemId, mantendo relatório atual");
            }
        } else
        {
            console.log("â„¹ï¸ [ModalViagem] Novo agendamento - não carregar relatório");
            $('#cardRelatorio').hide();
            window.ultimoViagemIdCarregado = null;
        }

        // Inicializar sistema de requisitante (accordion)
        setTimeout(() =>
        {
            if (typeof inicializarSistemaRequisitante === 'function')
            {
                inicializarSistemaRequisitante();
            }
        }, 500);
    } catch (error)
    {
        console.error("âŒ [ModalViagem] Erro ao abrir modal:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "aoAbrirModalViagem", error);
    }
}

/**
 * ðŸšª Event handler para quando o modal é fechado
 */
function aoFecharModalViagem()
{
    try
    {
        console.log("ðŸšª [ModalViagem] ===== MODAL FECHANDO =====");

        // Limpar o relatório
        if (typeof window.limparRelatorio === 'function')
        {
            window.limparRelatorio();
        }

        // Resetar variáveis EXCETO modalJaFoiLimpo
        window.ignorarEventosRecorrencia = false;
        window.carregandoViagemExistente = false;

        // Cancelar timeout pendente
        if (window.timeoutAbrirModal)
        {
            clearTimeout(window.timeoutAbrirModal);
            window.timeoutAbrirModal = null;
        }

        // Limpar campos do modal
        if (typeof window.limparCamposModalViagens === 'function')
        {
            window.limparCamposModalViagens();
            console.log("Campos limpos ao fechar modal");
        }

        // Resetar modalJaFoiLimpo DEPOIS da limpeza
        window.modalJaFoiLimpo = false;

        window.currentViagemId = null;
        window.ultimoViagemIdCarregado = null;

        console.log("Modal fechado e limpo");
        console.log("âœ… [ModalViagem] Modal fechado e limpo");
    } catch (error)
    {
        console.error("âŒ [ModalViagem] Erro ao fechar modal:", error);
    }
}

/**
 * ðŸŽ¬ Inicializa eventos de relatório no modal
 * Registra os event handlers do Bootstrap no modal
 */
function inicializarEventosRelatorioModal()
{
    try
    {
        console.log("ðŸŽ¬ [ModalViagem] ===== INICIALIZANDO EVENTOS DE RELAtÓRIO =====");

        const $modal = $('#modalViagens');

        if ($modal.length === 0)
        {
            console.warn("âš ï¸ [ModalViagem] Modal #modalViagens não encontrado no DOM");
            return;
        }

        console.log("âœ… [ModalViagem] Modal #modalViagens encontrado");

        // Remove eventos anteriores para evitar duplicação
        $modal.off('shown.bs.modal', aoAbrirModalViagem);
        $modal.off('hidden.bs.modal', aoFecharModalViagem);

        // Registra eventos
        $modal.on('shown.bs.modal', aoAbrirModalViagem);
        $modal.on('hidden.bs.modal', aoFecharModalViagem);

        console.log("âœ… [ModalViagem] Eventos de relatório inicializados com sucesso");
        console.log("   - shown.bs.modal â†’ aoAbrirModalViagem");
        console.log("   - hidden.bs.modal â†’ aoFecharModalViagem");
    } catch (error)
    {
        console.error("âŒ [ModalViagem] Erro ao inicializar eventos:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "inicializarEventosRelatorioModal", error);
    }
}

// Expor função globalmente
window.carregarRelatorioNoModal = carregarRelatorioNoModal;

$(function ()
{
    console.log("ðŸŽ¬ [ModalViagem] ===== DOCUMENTO PRONTO =====");
    console.log("ðŸŽ¬ [ModalViagem] Inicializando eventos de relatório...");
    inicializarEventosRelatorioModal();

    // VALIDAÇÃO: Data Final não pode ser superior à data atual
    // Configura evento blur para o DatePicker txtDataFinal
    const configurarValidacaoDataFinal = function ()
    {
        try
        {
            const datePicker = window.getKendoDatePicker("txtDataFinal");
            if (datePicker)
            {
                if (!datePicker._dataFinalValidacaoConfigurada)
                {
                    datePicker.bind("change", function ()
                    {
                        try
                        {
                            const dataFinalValue = datePicker.value();
                            if (dataFinalValue)
                            {
                                const dataFinal = new Date(dataFinalValue);
                                dataFinal.setHours(0, 0, 0, 0);
                                const hoje = new Date();
                                hoje.setHours(0, 0, 0, 0);

                                if (dataFinal > hoje)
                                {
                                    datePicker.value(null);
                                    AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
                                }
                            }
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("modal-viagem.js", "txtDataFinal.change", error);
                        }
                    });
                    datePicker._dataFinalValidacaoConfigurada = true;
                    console.log("✅ [ModalViagem] Validação de Data Final configurada (Kendo)");
                }
                return;
            }

            const txtDataFinal = document.getElementById("txtDataFinal");
            if (txtDataFinal && !txtDataFinal._dataFinalValidacaoConfigurada)
            {
                txtDataFinal.addEventListener("blur", function ()
                {
                    try
                    {
                        const dataFinalValue = window.getKendoDateValue("txtDataFinal");
                        if (dataFinalValue)
                        {
                            const dataFinal = new Date(dataFinalValue);
                            dataFinal.setHours(0, 0, 0, 0);
                            const hoje = new Date();
                            hoje.setHours(0, 0, 0, 0);

                            if (dataFinal > hoje)
                            {
                                window.setKendoDateValue("txtDataFinal", null);
                                AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
                            }
                        }
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("modal-viagem.js", "txtDataFinal.blur", error);
                    }
                });
                txtDataFinal._dataFinalValidacaoConfigurada = true;
            }
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("modal-viagem.js", "configurarValidacaoDataFinal", error);
        }
    };

    // Configura quando o modal da viagem abrir (componente pode não existir ainda)
    $(document).on("shown.bs.modal", "#ModalViagem", function ()
    {
        setTimeout(configurarValidacaoDataFinal, 100);
    });

    // Tenta configurar imediatamente também (caso o componente já exista)
    setTimeout(configurarValidacaoDataFinal, 500);
});

// ====================================================================
// SEÇÃO 8: INICIALIZAÇÃO E LIMPEZA DE CAMPOS
// ====================================================================

/**
 * ðŸŽ¬ Inicializa campos do modal
 * Prepara o modal para criar um novo agendamento
 */
window.inicializarCamposModal = function ()
{
    try
    {
        // Habilita todos os campos exceto o container de botões
        const divModal = document.getElementById("divModal");
        if (divModal)
        {
            const childNodes = divModal.getElementsByTagName("*");
            for (const node of childNodes)
            {
                if (node.id !== "divBotoes")
                {
                    node.disabled = false;
                    node.value = "";
                }
            }
        }

        // Configura campos de hora (Kendo TimePicker)
        window.setKendoTimeValue("txtHoraInicial", "");
        window.setKendoTimeValue("txtHoraFinal", "");

        // Oculta campos especí­ficos de viagem (só aparecem quando transformar em viagem)
        const camposViagem = [
            "divNoFichaVistoria", "divDataFinal", "divHoraFinal", "divDuracao",
            "divKmAtual", "divKmInicial", "divKmFinal", "divQuilometragem",
            "divCombustivelInicial", "divCombustivelFinal"
        ];

        camposViagem.forEach(id =>
        {
            const elemento = document.getElementById(id);
            if (elemento) elemento.style.display = "none";
        });

        // Inicializa componentes EJ2
        window.inicializarComponentesEJ2();

        // Configura visibilidade de botões
        $("#btnImprime, #btnConfirma, #btnApaga, #btnCancela").show();

        // ✅ Botão Novo Evento é controlado por evento.js (controlarVisibilidadeSecaoEvento)

        // ✅ lstEventos está SEMPRE HABILITADO
        // Apenas o valor é limpo quando necessário (em lstFinalidade_Change)

        // Configura botão requisitante
        const btnRequisitante = document.getElementById("btnRequisitante");
        if (btnRequisitante)
        {
            btnRequisitante.classList.remove("disabled");
        }

        console.log("âœ… [ModalViagem] Campos inicializados");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "inicializarCamposModal", error);
    }
};

/**
 * âš™ï¸ Inicializa componentes Syncfusion EJ2
 * Configura estado inicial dos componentes visuais
 */
window.inicializarComponentesEJ2 = function ()
{
    try
    {
        const componentes = [
            { id: "rteDescricao", propriedades: { enabled: true, value: "" } },
            { id: "lstMotorista", propriedades: { enabled: true, value: "" } },
            { id: "lstVeiculo", propriedades: { enabled: true, value: "" } },
            { id: "lstRequisitante", propriedades: { enabled: true, value: "" } },
            // REMOVIDO: lstSetorRequisitanteAgendamento - não limpar pois será preenchido depois
            { id: "ddtCombustivelInicial", propriedades: { value: "" } },
            { id: "ddtCombustivelFinal", propriedades: { value: "" } }
        ];

        componentes.forEach(({ id, propriedades }) =>
        {
            try
            {
                const elemento = document.getElementById(id);
                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
                {
                    const componente = elemento.ej2_instances[0];
                    Object.assign(componente, propriedades);
                }
            } catch (error)
            {
                console.warn(`âš ï¸ Não foi possível inicializar o componente: ${id}`);
            }
        });

        console.log("âœ… [ModalViagem] Componentes EJ2 inicializados");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "inicializarComponentesEJ2", error);
    }
};

/**
 * ðŸ§¹ Limpa campos de recorrência
 * Reseta todos os campos relacionados Ã  recorrência
 */
window.limparCamposRecorrencia = function ()
{
    try
    {
        const componentesRecorrencia = [
            { id: "lstRecorrente", valor: "N" },
            { id: "lstPeriodos", valor: "" },
            { id: "lstDias", valor: [] },
            { id: "txtFinalRecorrencia", valor: null },
            { id: "calDatasSelecionadas", valor: null }
        ];

        componentesRecorrencia.forEach(({ id, valor }) =>
        {
            if (id === "txtFinalRecorrencia")
            {
                window.setKendoDateValue(id, null);
                return;
            }
            const elemento = document.getElementById(id);
            if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
            {
                elemento.ej2_instances[0].value = valor;
            } else if (elemento)
            {
                elemento.value = valor;
            }
        });

        // Limpar lista de dias selecionados
        const listBox = document.getElementById("lstDiasCalendario");
        if (listBox && listBox.ej2_instances && listBox.ej2_instances[0])
        {
            listBox.ej2_instances[0].dataSource = [];
        }

        // Resetar badge de contagem
        const badge = document.getElementById("itensBadge");
        if (badge) badge.textContent = 0;

        // Limpar listbox de datas variadas
        const lstDatasVariadas = document.getElementById("lstDatasVariadas");
        if (lstDatasVariadas)
        {
            lstDatasVariadas.innerHTML = '';
            lstDatasVariadas.size = 3;
        }

        // Resetar badge de datas variadas
        const badgeDatasVariadas = document.getElementById("badgeContadorDatasVariadas");
        if (badgeDatasVariadas)
        {
            badgeDatasVariadas.textContent = 0;
            badgeDatasVariadas.style.display = 'none';
        }

        // Esconder container da listbox de datas variadas
        const listboxContainer = document.getElementById("listboxDatasVariadasContainer");
        if (listboxContainer)
        {
            listboxContainer.style.display = 'none';
        }

        console.log("âœ… [ModalViagem] Campos de recorrência limpos");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposRecorrencia", error);
    }
};

window.limparCamposModalViagens = function ()
{
    try
    {
        // âœ… VERIFICAR FLAG ANTES DE LIMPAR
        if (window.modalJaFoiLimpo)
        {
            console.log("â­•ï¸ [ModalViagem] Modal já foi limpo, pulando limpeza...");
            return;
        }

        // âœ… VERIFICAR SE EStí CARREGANDO VIAGEM EXISTENTE
        if (window.carregandoViagemExistente)
        {
            console.log("ðŸ“Œ [ModalViagem] Carregando viagem existente, pulando limpeza");
            return;
        }

        console.log("ðŸ§¹ [ModalViagem] Limpando todos os campos...");

        // Remover classes de modo de edição variada
        document.body.classList.remove('modo-edicao-variada');
        document.body.classList.remove('modo-criacao-variada');

        // âœ… MARCAR QUE O MODAL FOI LIMPO
        window.modalJaFoiLimpo = true;

        // MOSTRAR CARD DE RECORRÊNCIA (para novo agendamento)
        $("#cardRecorrencia").show();
        // Limpar campos HTML nativos
        $("#txtReport, #txtViagemId, #txtRecorrenciaViagemId, #txtStatusAgendamento, #txtUsuarioIdCriacao, #txtDataCriacao, #txtNoFichaVistoria, #txtDataFinal, #txtHoraFinal, #txtKmAtual, #txtKmInicial, #txtKmFinal, #txtRamalRequisitante, #txtNomeDoEvento, #txtDescricaoEvento, #txtDataInicialEvento, #txtDataFinalEvento, #txtQtdPessoas, #txtPonto, #txtNome, #txtRamal, #txtEmail").val("");

        // âœ… Ramal já é limpo na linha acima (txtRamalRequisitante é campo HTML nativo, não Syncfusion)

        // Limpar setor
        const lstSetor = document.getElementById("lstSetorRequisitanteAgendamento");
        if (lstSetor && lstSetor.ej2_instances && lstSetor.ej2_instances[0])
        {
            lstSetor.ej2_instances[0].value = null;
            window.refreshComponenteSafe("lstSetorRequisitanteAgendamento");
        }

        // Limpar campos de duração e quilometragem
        ["txtDuracao", "txtQuilometragem"].forEach(id =>
        {
            try
            {
                const elemento = document.getElementById(id);
                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
                {
                    const instance = elemento.ej2_instances[0];
                    instance.value = null;
                    window.refreshComponenteSafe(id);
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens_forEach1", error);
            }
        });

        // ✅ KENDO: Limpar cmbOrigem e cmbDestino (agora são Kendo ComboBox)
        try
        {
            const origemKendo = $("#cmbOrigem").data("kendoComboBox");
            if (origemKendo)
            {
                origemKendo.value(null);
                origemKendo.text("");
            }
        } catch (error)
        {
            console.warn("⚠️ [ModalViagem] Erro ao limpar cmbOrigem:", error);
        }

        try
        {
            const destinoKendo = $("#cmbDestino").data("kendoComboBox");
            if (destinoKendo)
            {
                destinoKendo.value(null);
                destinoKendo.text("");
            }
        } catch (error)
        {
            console.warn("⚠️ [ModalViagem] Erro ao limpar cmbDestino:", error);
        }

        // Limpar comboboxes e dropdowns Syncfusion - VERSÃO CORRIGIDA (removido cmbOrigem e cmbDestino)
        const syncIds = ["lstFinalidade", "ddtSetor", "lstMotorista", "lstVeiculo", "lstRequisitante", "lstSetorRequisitanteAgendamento", "lstEventos", "ddtCombustivelInicial", "ddtCombustivelFinal", "lstDiasMes", "lstDias"];
        syncIds.forEach(id =>
        {
            try
            {
                const elemento = document.getElementById(id);
                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
                {
                    const instance = elemento.ej2_instances[0];

                    // âœ… LIMPEZA COMPLETA
                    instance.value = null;
                    instance.text = '';

                    // ✅ SEMPRE HABILITAR todos os componentes (incluindo lstEventos)
                    if (typeof instance.enabled !== "undefined")
                    {
                        instance.enabled = true;
                    }

                    // Forçar atualização visual
                    if (typeof instance.dataBind === 'function')
                    {
                        instance.dataBind();
                    }

                    // Refresh adicional para garantir
                    if (typeof instance.refresh === 'function')
                    {
                        instance.refresh();
                    }

                    console.log(`âœ… ${id} limpo com sucesso`);
                } else
                {
                    console.warn(`âš ï¸ ${id} não encontrado ou não inicializado`);
                }
            } catch (error)
            {
                console.error(`âŒ Erro ao limpar ${id}:`, error);
                Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens_forEach2", error);
            }
        });

        // âœ… LIMPEZA ESPEcíFICA EXTRA PARA MOTORISTA E VeíCULO
        console.log("ðŸ§¹ [Limpeza Extra] Garantindo limpeza de Motorista e Veí­culo...");

        // Motorista
        const lstMotorista = document.getElementById("lstMotorista");
        if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0])
        {
            const motoristaInst = lstMotorista.ej2_instances[0];
            motoristaInst.value = null;
            motoristaInst.text = '';
            motoristaInst.index = null;

            if (typeof motoristaInst.dataBind === 'function')
            {
                motoristaInst.dataBind();
            }

            if (typeof motoristaInst.clear === 'function')
            {
                motoristaInst.clear();
            }

            console.log("âœ… Motorista limpo completamente");
        }

        // Veí­culo
        const lstVeiculo = document.getElementById("lstVeiculo");
        if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0])
        {
            const veiculoInst = lstVeiculo.ej2_instances[0];
            veiculoInst.value = null;
            veiculoInst.text = '';
            veiculoInst.index = null;

            if (typeof veiculoInst.dataBind === 'function')
            {
                veiculoInst.dataBind();
            }

            if (typeof veiculoInst.clear === 'function')
            {
                veiculoInst.clear();
            }

            console.log("âœ… Veí­culo limpo completamente");
        }

        // Limpar datas (Kendo DatePicker)
        ["txtDataInicial", "txtDataFinal", "txtFinalRecorrencia"].forEach(id =>
        {
            try
            {
                window.setKendoDateValue(id, null);
                window.enableKendoDatePicker(id, true);
            } catch (error)
            {
                console.error(`âŒ Erro ao limpar ${id}:`, error);
            }
        });

        // Limpar finalidade
        const lstFinalidade = document.getElementById("lstFinalidade");
        if (lstFinalidade && lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0])
        {
            lstFinalidade.ej2_instances[0].value = null;
            lstFinalidade.ej2_instances[0].enabled = true;
            window.refreshComponenteSafe("lstFinalidade");
        }

        // Limpar recorrência - CORRIGIDO COM INICIALIZAÇÃO DE DATASOURCE
        console.log("🔄 [limparCampos] Inicializando lstRecorrente...");

        // CRÍTICO: Garantir que dataSource está inicializado
        if (typeof window.inicializarLstRecorrente === 'function')
        {
            window.inicializarLstRecorrente();
        }

        // USAR TIMEOUT PARA GARANTIR QUE O VALOR SEJA DEFINIDO APÓS A INICIALIZAÇÃO
        setTimeout(() =>
        {
            const elRecorrente = document.getElementById("lstRecorrente");
            if (elRecorrente && elRecorrente.ej2_instances && elRecorrente.ej2_instances[0])
            {
                window.ignorarEventosRecorrencia = true;

                // Garantir que tem dataSource antes de definir valor
                const instance = elRecorrente.ej2_instances[0];
                if (!instance.dataSource || instance.dataSource.length === 0)
                {
                    instance.dataSource = [
                        { RecorrenteId: "N", Descricao: "Não" },
                        { RecorrenteId: "S", Descricao: "Sim" }
                    ];
                    instance.fields = { text: 'Descricao', value: 'RecorrenteId' };
                }

                instance.value = "N";
                instance.enabled = true;

                // Usar dataBind para aplicar valor
                if (typeof instance.dataBind === 'function')
                {
                    instance.dataBind();
                }

                console.log("✅ [limparCampos] lstRecorrente definido como 'Não' (com timeout)");
                window.ignorarEventosRecorrencia = false;
            }
        }, 100);

        // Limpar perí­odo - VERSÃO CORRIGIDA
        const elPeriodos = document.getElementById("lstPeriodos");
        if (elPeriodos && elPeriodos.ej2_instances && elPeriodos.ej2_instances[0])
        {
            elPeriodos.ej2_instances[0].value = null;
            elPeriodos.ej2_instances[0].enabled = true;
            window.refreshComponenteSafe("lstPeriodos");
        } else if (typeof window.rebuildLstPeriodos === "function")
        {
            window.rebuildLstPeriodos();
        }

        // Limpar editor de texto rico
        const rteDescricao = document.getElementById("rteDescricao");
        if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0])
        {
            rteDescricao.ej2_instances[0].value = "";
            window.refreshComponenteSafe("rteDescricao");
        }

        // Limpar campos de evento/requisitante
        const idsToReset = ["lstRequisitanteEvento", "lstSetorRequisitanteEvento", "ddtSetorRequisitante"];
        idsToReset.forEach(id =>
        {
            try
            {
                const elemento = document.getElementById(id);
                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
                {
                    const instance = elemento.ej2_instances[0];
                    instance.value = null;
                    window.refreshComponenteSafe(id);
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens_forEach3", error);
            }
        });

        // Esconder divs de campos avançados
        $("#divPeriodo, #divTxtPeriodo, #divDias, #divDiaMes, #divFinalRecorrencia, #divFinalFalsoRecorrencia, #calendarContainer, #listboxContainer, #listboxContainerHTML").hide();

        // Limpar labels de usuário
        $("#lblUsuarioAgendamento, #lblUsuarioCriacao, #lblUsuarioFinalizacao, #lblUsuarioCancelamento").text("");

        // Resetar botão confirmar
        $("#btnConfirma").html("<i class='fa-regular fa-thumbs-up'></i> Confirmar").prop("disabled", false);

        // Limpar calendário de datas selecionadas
        const calInstance = document.getElementById("calDatasSelecionadas");
        if (calInstance && calInstance.ej2_instances && calInstance.ej2_instances[0])
        {
            const calendario = calInstance.ej2_instances[0];
            if ("values" in calendario) calendario.values = [];
            if ("value" in calendario) calendario.value = null;
            window.refreshComponenteSafe("calDatasSelecionadas");
        }

        // Limpar lista HTML de dias
        const lstDiasHTML = document.getElementById("lstDiasCalendarioHTML");
        if (lstDiasHTML) lstDiasHTML.innerHTML = "";

        // Limpar lista de dias selecionados
        const listBox = document.getElementById("lstDiasCalendario");
        if (listBox && listBox.ej2_instances && listBox.ej2_instances[0])
        {
            listBox.ej2_instances[0].dataSource = [];
        }

        // Resetar badge de contagem
        const badge = document.getElementById("itensBadge");
        if (badge) badge.textContent = 0;

        // âœ… LIMPAR E ESCONDER RELAtÓRIO
        console.log("ðŸ§¹ [ModalViagem] Limpando relatório...");

        if (typeof window.limparRelatorio === 'function')
        {
            window.limparRelatorio();
        } else
        {
            // Fallback manual se função não existir
            $("#ReportContainerAgenda").hide();
            $("#reportViewerAgenda").html("");
            $("#cardRelatorio").hide();
        }

        // Limpar campos hidden de viagem
        $('#txtViagemIdRelatorio').val('');
        window.currentViagemId = null;

        // Abortar requisições de relatório pendentes (se houver)
        if (window.xhrRelatorio && window.xhrRelatorio.abort)
        {
            window.xhrRelatorio.abort();
        }

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
            window.showKendoDatePicker("txtFinalRecorrencia", true);
            window.setKendoDateValue("txtFinalRecorrencia", null);
            window.enableKendoDatePicker("txtFinalRecorrencia", true);
        }

        console.log("âœ… [ModalViagem] Todos os campos limpos");
    } catch (error)
    {
        console.error("âŒ [ModalViagem] Erro ao limpar campos:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "limparCamposModalViagens", error);
    }
};

// ====================================================================
// SEÇÃO 9: CONTROLE DE ESTADO DO MODAL
// ====================================================================

/**
 * ðŸ”’ Desabilita todos os controles do formulário (EXCETO botões de fechar/footer)
 * Usado para modo de visualização (quando o agendamento já foi realizado ou cancelado)
 */
window.desabilitarTodosControles = function ()
{
    try
    {
        console.log("ðŸ”’ [ModalViagem] Desabilitando controles...");

        // IMPORTANTE: IDs de botões que NUNCA devem ser desabilitados
        const botoesProtegidos = [
            'btnFecha',           // Botão X do modal
            'btnFechar',          // Botão Fechar
            'btnCancelar',        // Botão Cancelar
            'btnClose',           // Variação de nome
            'btnCancel'           // Variação de nome
        ];

        // Desabilita campos HTML nativos (EXCETO botões protegidos)
        const divModal = document.getElementById("divModal");
        if (divModal)
        {
            const childNodes = divModal.getElementsByTagName("*");
            for (const node of childNodes)
            {
                // Verificar se é botão protegido
                const isProtegido = botoesProtegidos.includes(node.id) ||
                    node.hasAttribute('data-bs-dismiss') ||
                    node.classList.contains('btn-close') ||
                    node.closest('.modal-header') !== null ||
                    node.closest('[data-bs-dismiss]') !== null;

                if (!isProtegido)
                {
                    node.disabled = true;
                }
            }
        }

        // Desabilita componentes EJ2 (EXCETO os do modal-footer)
        // ✅ KENDO: Desabilitar cmbOrigem e cmbDestino (agora são Kendo ComboBox)
        try
        {
            const origemKendo = $("#cmbOrigem").data("kendoComboBox");
            if (origemKendo) origemKendo.enable(false);
        } catch (error)
        {
            console.warn("⚠️ Erro ao desabilitar cmbOrigem:", error);
        }

        try
        {
            const destinoKendo = $("#cmbDestino").data("kendoComboBox");
            if (destinoKendo) destinoKendo.enable(false);
        } catch (error)
        {
            console.warn("⚠️ Erro ao desabilitar cmbDestino:", error);
        }

        const componentesEJ2 = [
            "txtDataInicial", "txtDataFinal", "lstFinalidade",
            "lstMotorista", "lstVeiculo", "lstRequisitante",
            "lstSetorRequisitanteAgendamento",
            "ddtCombustivelInicial", "ddtCombustivelFinal", "rteDescricao",
            "lstRecorrente", "lstPeriodos", "lstDias", "lstEventos"
        ];

        componentesEJ2.forEach(id =>
        {
            try
            {
                const elemento = document.getElementById(id);
                if (elemento && elemento.ej2_instances && elemento.ej2_instances[0])
                {
                    elemento.ej2_instances[0].enabled = false;
                }
            } catch (error)
            {
                console.warn(`âš ï¸ Erro ao desabilitar componente ${id}:`, error);
            }
        });

        //         // Desabilita botão requisitante (mas não botões de fechar)
        //         const btnRequisitante = document.getElementById("btnRequisitante");
        //         if (btnRequisitante)
        //         {
        //             btnRequisitante.classList.add("disabled");
        //             btnRequisitante.addEventListener("click", function (event)
        //             {
        //                 event.preventDefault();
        //             });
        //         }

        // GARANTIR que botões de fechar NUNCA são desabilitados
        botoesProtegidos.forEach(id =>
        {
            const btn = document.getElementById(id);
            if (btn)
            {
                btn.disabled = false;
                btn.classList.remove('disabled');
                btn.style.pointerEvents = 'auto';
            }
        });

        // Garantir botão X do modal sempre habilitado
        const btnClose = document.querySelector('#modalViagens .btn-close, #modalViagens [data-bs-dismiss="modal"]');
        if (btnClose)
        {
            btnClose.disabled = false;
            btnClose.style.pointerEvents = 'auto';
        }

        console.log("ðŸ”’ [ModalViagem] Controles desabilitados (exceto botões de fechar)");
    } catch (error)
    {
        console.error("âŒ [ModalViagem] Erro ao desabilitar controles:", error);
        Alerta.TratamentoErroComLinha("modal-viagem.js", "desabilitarTodosControles", error);
    }
};

// ====================================================================
// FIM DO ARQUIVO modal-viagem.js
// ====================================================================
console.log("âœ… [ModalViagem] Arquivo carregado completamente");
