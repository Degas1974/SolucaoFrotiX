/* ****************************************************************************************
 * ⚡ ARQUIVO: agendamento.service.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Serviço centralizado para gerenciamento de agendamentos de viagens.
 *                   Encapsula todas as operações CRUD (Create, Read, Update, Delete) e
 *                   operações especiais (recorrência, cancelamento, datas) via API REST.
 *                   Usa window.ApiClient para HTTP requests, retorna objetos padronizados
 *                   {success, data/error, message}.
 * 📥 ENTRADAS     : IDs (viagemId, recorrenciaViagemId - strings), dados de agendamento
 *                   (Object com propriedades da viagem), fetchInfo (FullCalendar Object
 *                   com startStr/endStr), descrição de cancelamento (string)
 * 📤 SAÍDAS       : Promises resolvidas com {success: boolean, data/error: any, message?}
 *                   - success=true: {success:true, data: response}
 *                   - success=false: {success:false, error: error.message, message?}
 *                   Em caso de erro: Alerta.TratamentoErroComLinha + objeto de erro
 * 🔗 CHAMADA POR  : Componentes de agendamento (main.js, dialogs.js, calendario.js),
 *                   páginas (Index.cshtml), outros serviços
 * 🔄 CHAMA        : window.ApiClient (get, post), fetch nativo, Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : ApiClient (window.ApiClient), Alerta.js (TratamentoErroComLinha),
 *                   fetch API nativa (carregarEventos)
 * 📝 OBSERVAÇÕES  : Exporta window.AgendamentoService (instância global). Todos os métodos
 *                   async retornam Promises com objeto padronizado. Métodos GET usam
 *                   ApiClient.get, POST usam ApiClient.post. carregarEventos usa fetch
 *                   nativo. obterParaEdicao normaliza resposta (Array→Object). Todos têm
 *                   try-catch completo.
 *
 * 📋 ÍNDICE DE MÉTODOS DA CLASSE (11 métodos + constructor):
 *
 * ┌─ CONSTRUCTOR ─────────────────────────────────────────────────────────┐
 * │ 1. constructor()                                                      │
 * │    → Inicializa this.api = window.ApiClient                          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ OPERAÇÕES DE BUSCA (READ) ──────────────────────────────────────────┐
 * │ 2. async buscarViagem(id)                                            │
 * │    → param {string} id - ID da viagem                                │
 * │    → returns {Promise<Object>} {success, data: response.data}        │
 * │    → GET /api/Agenda/RecuperaViagem?id={id}                          │
 * │    → success: {success:true, data: response.data}                    │
 * │    → error: {success:false, error: error.message}                    │
 * │    → try-catch: Alerta.TratamentoErroComLinha("buscarViagem")        │
 * │                                                                        │
 * │ 3. async obterRecorrentes(recorrenciaViagemId)                       │
 * │    → param {string} recorrenciaViagemId - ID da recorrência          │
 * │    → returns {Promise<Object>} {success, data: Array}                │
 * │    → GET /api/Agenda/ObterAgendamentoExclusao?recorrenciaViagemId={} │
 * │    → success: {success:true, data: data}                             │
 * │    → error: {success:false, error: error.message, data: []}          │
 * │    → try-catch: Alerta.TratamentoErroComLinha("obterRecorrentes")    │
 * │                                                                        │
 * │ 4. async obterRecorrenteInicial(viagemId)                            │
 * │    → param {string} viagemId - ID da viagem                          │
 * │    → returns {Promise<Object>} {success, data: Object}               │
 * │    → GET /api/Agenda/ObterAgendamentoEdicaoInicial?viagemId={}       │
 * │    → success: {success:true, data: data}                             │
 * │    → error: {success:false, error: error.message, data: []}          │
 * │    → try-catch: Alerta.TratamentoErroComLinha("obterRecorrenteInicial")│
 * │                                                                        │
 * │ 5. async obterParaEdicao(viagemId)                                   │
 * │    → param {string} viagemId - ID da viagem                          │
 * │    → returns {Promise<Object>} {success, data: Object}               │
 * │    → GET /api/Agenda/ObterAgendamentoEdicao?viagemId={}              │
 * │    → Normaliza: Array.isArray(response) ? response[0] : response     │
 * │    → success: {success:true, data: objViagem}                        │
 * │    → error: {success:false, error: error.message}                    │
 * │    → try-catch: Alerta.TratamentoErroComLinha("obterParaEdicao")     │
 * │                                                                        │
 * │ 6. async obterDatas(viagemId, recorrenciaViagemId)                   │
 * │    → param {string} viagemId - ID da viagem                          │
 * │    → param {string} recorrenciaViagemId - ID da recorrência          │
 * │    → returns {Promise<Object>} {success, data: Array}                │
 * │    → GET /api/Agenda/GetDatasViagem?viagemId={}&recorrenciaViagemId={}│
 * │    → success: {success:true, data: data}                             │
 * │    → error: {success:false, error: error.message, data: []}          │
 * │    → try-catch: Alerta.TratamentoErroComLinha("obterDatas")          │
 * │                                                                        │
 * │ 7. async carregarEventos(fetchInfo)                                  │
 * │    → param {Object} fetchInfo - {startStr, endStr} FullCalendar      │
 * │    → returns {Promise<Object>} {success, data: Array de eventos}     │
 * │    → fetch /api/Agenda/Eventos?start={startStr}&end={endStr}         │
 * │    → Mapeia (payload.data || payload || []) para eventos FullCalendar│
 * │    → Evento: {id, title, description, start, end, backgroundColor,   │
 * │              textColor, allDay}                                      │
 * │    → success: {success:true, data: events}                           │
 * │    → error: {success:false, error: error.message, data: []}          │
 * │    → try-catch: Alerta.TratamentoErroComLinha("carregarEventos")     │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ OPERAÇÕES DE PERSISTÊNCIA (CREATE/UPDATE) ──────────────────────────┐
 * │ 8. async salvar(dados)                                               │
 * │    → param {Object} dados - Dados do agendamento (viagem completa)   │
 * │    → returns {Promise<Object>} {success, data/message}               │
 * │    → POST /api/Agenda/Agendamento com body=dados                     │
 * │    → Se response.success || response.data:                           │
 * │      {success:true, data: response}                                  │
 * │    → Senão: {success:false, message: "Falha ao salvar agendamento"}  │
 * │    → error: {success:false, error: error.message}                    │
 * │    → try-catch: Alerta.TratamentoErroComLinha("salvar")              │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ OPERAÇÕES DE EXCLUSÃO (DELETE) ─────────────────────────────────────┐
 * │ 9. async excluir(viagemId)                                           │
 * │    → param {string} viagemId - ID da viagem                          │
 * │    → returns {Promise<Object>} {success, message}                    │
 * │    → POST /api/Agenda/ApagaAgendamento com {ViagemId: viagemId}      │
 * │    → Se response.success: {success:true, message: response.message}  │
 * │    → Senão: {success:false, message: "Erro ao excluir agendamento"}  │
 * │    → error: {success:false, error: error.message}                    │
 * │    → try-catch: Alerta.TratamentoErroComLinha("excluir")             │
 * │                                                                        │
 * │ 10. async excluirRecorrentes(recorrenciaViagemId)                    │
 * │     → param {string} recorrenciaViagemId - ID da recorrência         │
 * │     → returns {Promise<Object>} {success, message}                   │
 * │     → POST /api/Agenda/ApagaAgendamentosRecorrentes                  │
 * │       com {RecorrenciaViagemId: recorrenciaViagemId}                 │
 * │     → Se response.success: {success:true, message: response.message} │
 * │     → Senão: {success:false, message: "Erro ao excluir recorrentes"} │
 * │     → error: {success:false, error: error.message}                   │
 * │     → try-catch: Alerta.TratamentoErroComLinha("excluirRecorrentes") │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ OPERAÇÕES ESPECIAIS ────────────────────────────────────────────────┐
 * │ 11. async cancelar(viagemId, descricao)                              │
 * │     → param {string} viagemId - ID da viagem                         │
 * │     → param {string} descricao - Descrição do cancelamento           │
 * │     → returns {Promise<Object>} {success, message}                   │
 * │     → POST /api/Agenda/CancelaAgendamento                            │
 * │       com {ViagemId: viagemId, Descricao: descricao}                 │
 * │     → Se response.success: {success:true, message: response.message} │
 * │     → Senão: {success:false, message: "Erro ao cancelar"}            │
 * │     → error: {success:false, error: error.message}                   │
 * │     → try-catch: Alerta.TratamentoErroComLinha("cancelar")           │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE USO TÍPICO (buscarViagem):
 * 1. Chamada: await AgendamentoService.buscarViagem('123')
 * 2. ApiClient.get('/api/Agenda/RecuperaViagem', { id: '123' })
 * 3. Response: { data: { viagemId, dataInicial, ... } }
 * 4. Retorna: { success: true, data: response.data }
 * 5. Em caso de erro: Alerta.TratamentoErroComLinha + { success: false, error }
 *
 * 🔄 FLUXO DE USO TÍPICO (salvar):
 * 1. Chamada: await AgendamentoService.salvar({ viagemId, dataInicial, ... })
 * 2. ApiClient.post('/api/Agenda/Agendamento', dados)
 * 3. Response: { success: true, data: viagemId } ou { success: false, message }
 * 4. Se success || data: retorna { success: true, data: response }
 * 5. Senão: retorna { success: false, message: "Falha ao salvar agendamento" }
 * 6. Em caso de erro: { success: false, error: error.message }
 *
 * 🔄 FLUXO DE USO TÍPICO (excluir):
 * 1. Chamada: await AgendamentoService.excluir('123')
 * 2. ApiClient.post('/api/Agenda/ApagaAgendamento', { ViagemId: '123' })
 * 3. Response: { success: true, message: "Agendamento excluído" }
 * 4. Retorna: { success: true, message: response.message }
 * 5. Se !success: retorna { success: false, message: "Erro ao excluir agendamento" }
 *
 * 🔄 FLUXO DE USO TÍPICO (carregarEventos):
 * 1. Chamada: await AgendamentoService.carregarEventos({ startStr: '2026-01-01', endStr: '2026-01-31' })
 * 2. fetch /api/Agenda/Eventos?start=2026-01-01&end=2026-01-31
 * 3. payload.json() → { data: [...] } ou [...]
 * 4. Mapeia (payload.data || payload || []) para eventos FullCalendar
 * 5. Evento mapeado: { id, title, description, start, end, backgroundColor, textColor, allDay }
 * 6. Retorna: { success: true, data: events }
 *
 * 🔄 FLUXO DE USO TÍPICO (obterParaEdicao):
 * 1. Chamada: await AgendamentoService.obterParaEdicao('123')
 * 2. ApiClient.get('/api/Agenda/ObterAgendamentoEdicao', { viagemId: '123' })
 * 3. Response pode ser Array ou Object
 * 4. Normaliza: Array.isArray(response) ? response[0] : response
 * 5. Retorna: { success: true, data: objViagem }
 *
 * 📌 ESTRUTURA DE RETORNO PADRONIZADA:
 * - Sucesso: { success: true, data: any, message?: string }
 * - Falha: { success: false, error?: string, message?: string, data?: [] }
 * - data: pode ser Object, Array, ou omitido
 * - error: error.message da exception
 * - message: mensagem do backend ou mensagem padrão
 *
 * 📌 DIFERENÇAS ENTRE MÉTODOS GET E POST:
 * - GET (buscar, obter*): usa ApiClient.get(url, params) - query string
 * - POST (salvar, excluir*, cancelar): usa ApiClient.post(url, data) - body JSON
 * - carregarEventos: usa fetch nativo (não ApiClient)
 *
 * 📌 MÉTODOS QUE RETORNAM data: [] EM CASO DE ERRO:
 * - obterRecorrentes: { success: false, error, data: [] }
 * - obterRecorrenteInicial: { success: false, error, data: [] }
 * - obterDatas: { success: false, error, data: [] }
 * - carregarEventos: { success: false, error, data: [] }
 * - Motivo: evitar erros de iteração no código chamador
 *
 * 📌 NORMALIZAÇÃO DE RESPOSTA:
 * - obterParaEdicao: normaliza Array→Object (response[0])
 * - carregarEventos: normaliza payload.data || payload || []
 * - Motivo: backend pode retornar formatos diferentes
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - window.AgendamentoService: instância global criada (line 333)
 * - Todos os métodos são async: retornam Promises
 * - ApiClient wrapper: encapsula jQuery.ajax (GET/POST/PUT/DELETE)
 * - fetch nativo em carregarEventos: mais performático para GET simples
 * - Todos têm try-catch completo com Alerta.TratamentoErroComLinha
 * - Mensagens de erro padrão em pt-BR (ex: "Falha ao salvar agendamento")
 * - Payload FullCalendar: { id, title, description, start, end, backgroundColor, textColor, allDay }
 * - Recorrência: ID separado (recorrenciaViagemId) para agrupar viagens recorrentes
 * - Cancelar vs Excluir: cancelar registra descrição, excluir apaga permanentemente
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

class AgendamentoService
{
    constructor()
    {
        this.api = window.ApiClient;
    }

    /**
     * Busca viagem por ID
     * param {string} id - ID da viagem
     * returns {Promise<Object>} Dados da viagem
     */
    async buscarViagem(id)
    {
        try
        {
            const response = await this.api.get('/api/Agenda/RecuperaViagem', { id });
            return {
                success: true,
                data: response.data
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "buscarViagem", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Cria ou atualiza agendamento
     * param {Object} dados - Dados do agendamento
     * returns {Promise<Object>} Resultado da operação
     */
    async salvar(dados)
    {
        try
        {
            const response = await this.api.post('/api/Agenda/Agendamento', dados);

            if (response.success || response.data)
            {
                return {
                    success: true,
                    data: response
                };
            } else
            {
                return {
                    success: false,
                    message: response.message || "Falha ao salvar agendamento"
                };
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "salvar", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Exclui agendamento
     * param {string} viagemId - ID da viagem
     * returns {Promise<Object>} Resultado da operação
     */
    async excluir(viagemId)
    {
        try
        {
            const response = await this.api.post('/api/Agenda/ApagaAgendamento', { ViagemId: viagemId });

            if (response.success)
            {
                return {
                    success: true,
                    message: response.message
                };
            } else
            {
                return {
                    success: false,
                    message: response.message || "Erro ao excluir agendamento"
                };
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "excluir", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Exclui todos os agendamentos recorrentes de uma vez
     * param {string} recorrenciaViagemId - ID da recorrência
     * returns {Promise<Object>} Resultado da operação
     */
    async excluirRecorrentes(recorrenciaViagemId)
    {
        try
        {
            const response = await this.api.post('/api/Agenda/ApagaAgendamentosRecorrentes', {
                RecorrenciaViagemId: recorrenciaViagemId
            });

            if (response.success)
            {
                return {
                    success: true,
                    message: response.message
                };
            } else
            {
                return {
                    success: false,
                    message: response.message || "Erro ao excluir agendamentos recorrentes"
                };
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "excluirRecorrentes", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Cancela agendamento
     * param {string} viagemId - ID da viagem
     * param {string} descricao - Descrição do cancelamento
     * returns {Promise<Object>} Resultado da operação
     */
    async cancelar(viagemId, descricao)
    {
        try
        {
            const response = await this.api.post('/api/Agenda/CancelaAgendamento', {
                ViagemId: viagemId,
                Descricao: descricao
            });

            if (response.success)
            {
                return {
                    success: true,
                    message: response.message
                };
            } else
            {
                return {
                    success: false,
                    message: response.message || "Erro ao cancelar agendamento"
                };
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "cancelar", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Obtém agendamentos recorrentes
     * param {string} recorrenciaViagemId - ID da recorrência
     * returns {Promise<Array>} Lista de agendamentos
     */
    async obterRecorrentes(recorrenciaViagemId)
    {
        try
        {
            const data = await this.api.get('/api/Agenda/ObterAgendamentoExclusao', {
                recorrenciaViagemId
            });

            return {
                success: true,
                data: data
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "obterRecorrentes", error);
            return {
                success: false,
                error: error.message,
                data: []
            };
        }
    }

    /**
     * Obtém agendamento inicial de recorrência
     * param {string} viagemId - ID da viagem
     * returns {Promise<Object>} Dados do agendamento inicial
     */
    async obterRecorrenteInicial(viagemId)
    {
        try
        {
            const data = await this.api.get('/api/Agenda/ObterAgendamentoEdicaoInicial', {
                viagemId
            });

            return {
                success: true,
                data: data
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "obterRecorrenteInicial", error);
            return {
                success: false,
                error: error.message,
                data: []
            };
        }
    }

    /**
     * Obtém agendamento para edição
     * param {string} viagemId - ID da viagem
     * returns {Promise<Object>} Dados do agendamento
     */
    async obterParaEdicao(viagemId)
    {
        try
        {
            const response = await this.api.get('/api/Agenda/ObterAgendamentoEdicao', {
                viagemId
            });

            const objViagem = Array.isArray(response) ? response[0] : response;

            return {
                success: true,
                data: objViagem
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "obterParaEdicao", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Obtém datas de viagem
     * param {string} viagemId - ID da viagem
     * param {string} recorrenciaViagemId - ID da recorrência
     * returns {Promise<Array>} Lista de datas
     */
    async obterDatas(viagemId, recorrenciaViagemId)
    {
        try
        {
            const data = await this.api.get('/api/Agenda/GetDatasViagem', {
                viagemId,
                recorrenciaViagemId
            });

            return {
                success: true,
                data: data
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "obterDatas", error);
            return {
                success: false,
                error: error.message,
                data: []
            };
        }
    }

    /**
     * Carrega eventos do calendário
     * param {Object} fetchInfo - Informações de fetch do FullCalendar
     * returns {Promise<Array>} Lista de eventos
     */
    async carregarEventos(fetchInfo)
    {
        try
        {
            const resp = await fetch(`/api/Agenda/Eventos?start=${fetchInfo.startStr}&end=${fetchInfo.endStr}`);
            const payload = await resp.json();

            const events = (payload?.data || payload || []).map(item => ({
                id: item.id,
                title: item.title || item.descricao || "",
                description: item.descricao || "",
                start: item.start,
                end: item.end,
                backgroundColor: item.backgroundColor || undefined,
                textColor: item.textColor || undefined,
                allDay: item.allDay === true
            }));

            return {
                success: true,
                data: events
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("agendamento.service.js", "carregarEventos", error);
            return {
                success: false,
                error: error.message,
                data: []
            };
        }
    }
}

// Instância global
window.AgendamentoService = new AgendamentoService();
