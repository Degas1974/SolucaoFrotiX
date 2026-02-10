/* ****************************************************************************************
 * ⚡ ARQUIVO: evento.service.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Service class para gerenciamento completo de eventos (Evento) do
 *                   sistema FrotiX. Fornece 3 métodos para adicionar novos eventos via
 *                   ApiClient POST, listar eventos existentes via jQuery AJAX GET com
 *                   mapeamento de dados (eventoId→EventoId, nome→Evento), e atualizar
 *                   dropdown Syncfusion com lista de eventos e seleção opcional.
 * 📥 ENTRADAS     : adicionar(dados: Object com propriedades do evento),
 *                   listar() sem parâmetros, atualizarListaDropdown(eventoId?: number)
 * 📤 SAÍDAS       : adicionar retorna {success, message, eventoId, eventoText} ou
 *                   {success: false, message/error}, listar retorna Promise<{success,
 *                   data: Array<{EventoId, Evento}>}> ou reject(erro), atualizarListaDropdown
 *                   retorna Promise<void> (atualiza DOM diretamente via Syncfusion)
 * 🔗 CHAMADA POR  : Event handlers (onSelectFinalidade, event-handlers.js), controls-init.js
 *                   (inicializarEventHandlersControles), exibe-viagem.js (ExibeViagem,
 *                   preencherCamposParaEdicao), modal para adicionar novo evento, qualquer
 *                   código que precise gerenciar dropdown lstEventos
 * 🔄 CHAMA        : ApiClient.post('/api/Viagem/AdicionarEvento'), $.ajax GET (/Viagens/
 *                   Upsert?handler=AJAXPreencheListaEventos), criarErroAjax (error callbacks),
 *                   Alerta.TratamentoErroComLinha (3 try-catch blocks), Syncfusion DropDownList
 *                   methods (dataSource setter, dataBind(), value setter), document.getElementById,
 *                   Array.map
 * 📦 DEPENDÊNCIAS : ApiClient (window.ApiClient, instância global), jQuery ($.ajax),
 *                   criarErroAjax (ajax-helper.js), Alerta.TratamentoErroComLinha
 *                   (frotix-core.js), Syncfusion EJ2 DropDownList (ej2_instances[0]),
 *                   DOM element #lstEventos, Razor Pages endpoint /Viagens/Upsert handler
 *                   AJAXPreencheListaEventos, API endpoint /api/Viagem/AdicionarEvento
 * 📝 OBSERVAÇÕES  : Exporta window.EventoService (instância singleton global new EventoService()).
 *                   Mistura async/await (adicionar) com Promise manual (listar) e async/await
 *                   (atualizarListaDropdown). listar() usa pattern Promise wrapper around $.ajax.
 *                   Mapeia response.data de {eventoId, nome} para {EventoId, Evento} (PascalCase
 *                   para compatibilidade Syncfusion fields). adicionar retorna both success e
 *                   error objects (não throw). atualizarListaDropdown silenciosamente retorna
 *                   se lstEventos não existe (sem erro). Try-catch em todos os 3 métodos com
 *                   logging via TratamentoErroComLinha. Error callback em $.ajax usa criarErroAjax
 *                   para erro enriquecido. eventoId opcional em atualizarListaDropdown para
 *                   selecionar item após refresh (usado após adicionar).
 *
 * 📋 ÍNDICE DE MÉTODOS (3 métodos públicos, 1 instância global):
 *
 * ┌─ CLASS EventoService ──────────────────────────────────────────────┐
 * │ constructor()                                                       │
 * │ → Inicializa this.api = window.ApiClient (instância global)        │
 * │ → Chamado internamente ao criar window.EventoService               │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ async adicionar(dados: Object) ───────────────────────────────────┐
 * │ → Adiciona novo evento via API POST                                │
 * │ → param dados: Object com propriedades do evento (nome, etc.)      │
 * │ → returns Promise<{success, message, eventoId, eventoText}> ou     │
 * │           {success: false, message/error}                          │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. await this.api.post('/api/Viagem/AdicionarEvento', dados)     │
 * │   3. if response.success: return {success: true, message,           │
 * │      eventoId, eventoText}                                          │
 * │   4. else: return {success: false, message || "Erro ao adicionar"}  │
 * │   5. catch: Alerta.TratamentoErroComLinha + return {success: false, │
 * │      error: error.message}                                          │
 * │ → Não lança exceção, sempre retorna objeto com success flag        │
 * │ → Usado após modal de adicionar evento para salvar e atualizar     │
 * │   dropdown                                                          │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ async listar() ────────────────────────────────────────────────────┐
 * │ → Lista todos os eventos do sistema                                │
 * │ → returns Promise<{success: true, data: Array<{EventoId, Evento}>}>│
 * │           ou reject(erro enriquecido)                               │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. return new Promise((resolve, reject) => {...})                 │
 * │   3. $.ajax GET /Viagens/Upsert?handler=AJAXPreencheListaEventos   │
 * │   4. success callback:                                              │
 * │      a. const eventos = res.data.map(item => ({                     │
 * │           EventoId: item.eventoId, Evento: item.nome }))            │
 * │      b. resolve({success: true, data: eventos})                     │
 * │   5. error callback:                                                │
 * │      a. const erro = criarErroAjax(jqXHR, textStatus, errorThrown,  │
 * │           this)                                                     │
 * │      b. Alerta.TratamentoErroComLinha("evento.service.js", "listar",│
 * │           erro)                                                     │
 * │      c. reject(erro)                                                │
 * │   6. catch outer: Alerta.TratamentoErroComLinha + return {success:  │
 * │      false, error, data: []}                                        │
 * │ → Mapeia camelCase backend (eventoId, nome) para PascalCase        │
 * │   Syncfusion (EventoId, Evento)                                     │
 * │ → Pattern Promise wrapper around jQuery.ajax (não usa ApiClient)   │
 * │ → Usado por atualizarListaDropdown e event handlers                │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ async atualizarListaDropdown(eventoId?: number) ──────────────────┐
 * │ → Atualiza dropdown lstEventos com lista completa e seleciona item │
 * │ → param eventoId: number opcional, ID do evento para selecionar    │
 * │                   após refresh (usado após adicionar)               │
 * │ → returns Promise<void> (atualiza DOM diretamente)                  │
 * │ → Fluxo:                                                            │
 * │   1. try-catch wrapper                                              │
 * │   2. const result = await this.listar()                             │
 * │   3. if !result.success: throw new Error(result.error)              │
 * │   4. const lstEventosElement = document.getElementById("lstEventos")│
 * │   5. if !lstEventosElement || !ej2_instances[0]: return (silent)    │
 * │   6. const lstEventos = lstEventosElement.ej2_instances[0]          │
 * │   7. lstEventos.dataSource = result.data                            │
 * │   8. lstEventos.dataBind() (refresh Syncfusion DropDownList)        │
 * │   9. if eventoId: lstEventos.value = eventoId (seleciona)           │
 * │   10. catch: Alerta.TratamentoErroComLinha                          │
 * │ → Acessa instância Syncfusion via ej2_instances[0]                 │
 * │ → Silenciosamente retorna se dropdown não existe (não é erro)      │
 * │ → Usado após adicionar evento para refresh + select                │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ INSTÂNCIA GLOBAL ──────────────────────────────────────────────────┐
 * │ window.EventoService = new EventoService()                          │
 * │ → Singleton pattern, instância única global                         │
 * │ → Acesso: window.EventoService.adicionar(dados)                     │
 * │ → Usado em todos os event handlers e components                     │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO DE ADICIONAR EVENTO:
 * 1. Usuário seleciona finalidade que tem eventos
 * 2. onSelectFinalidade mostra section #divEvento (event-handlers.js)
 * 3. Dropdown lstEventos é exibido com eventos existentes
 * 4. Usuário clica em botão "Adicionar Novo Evento" (modal)
 * 5. Modal captura dados do novo evento (nome, descrição, etc.)
 * 6. Chama EventoService.adicionar(dados)
 * 7. Se sucesso: chama EventoService.atualizarListaDropdown(eventoId) para
 *    refresh dropdown e selecionar novo item
 * 8. Modal fecha, novo evento aparece selecionado em lstEventos
 *
 * 🔄 FLUXO TÍPICO DE LISTAR EVENTOS:
 * 1. inicializarEventHandlersControles() inicializa lstEventos (controls-init.js)
 * 2. EventoService.listar() é chamado para popular dataSource inicial
 * 3. Syncfusion DropDownList renderiza lista com {EventoId, Evento}
 * 4. Usuário vê dropdown com todos os eventos disponíveis
 * 5. Select de evento dispara onSelectEvento handler (event-handlers.js)
 *
 * 🔄 FLUXO TÍPICO DE ATUALIZAR DROPDOWN:
 * 1. Após adicionar novo evento via EventoService.adicionar(dados)
 * 2. Chama EventoService.atualizarListaDropdown(response.eventoId)
 * 3. listar() faz GET para buscar lista atualizada (incluindo novo item)
 * 4. dataSource = result.data (novo array com novo evento)
 * 5. dataBind() força refresh do Syncfusion DropDownList
 * 6. lstEventos.value = eventoId (seleciona novo item automaticamente)
 * 7. Dropdown mostra nova lista com item recém-adicionado selecionado
 *
 * 📌 MAPEAMENTO DE DADOS (listar method):
 * Backend response:
 * {
 *   data: [
 *     { eventoId: 1, nome: "Conferência" },
 *     { eventoId: 2, nome: "Reunião Externa" }
 *   ]
 * }
 *
 * Após map:
 * [
 *   { EventoId: 1, Evento: "Conferência" },
 *   { EventoId: 2, Evento: "Reunião Externa" }
 * ]
 *
 * → PascalCase necessário para Syncfusion fields configuration
 *
 * 📌 ESTRUTURA DE RESPOSTA (adicionar method):
 * Success:
 * {
 *   success: true,
 *   message: "Evento adicionado com sucesso",
 *   eventoId: 123,
 *   eventoText: "Conferência Anual"
 * }
 *
 * Failure:
 * {
 *   success: false,
 *   message: "Evento já existe" // ou
 *   error: "Network error"       // em caso de exception
 * }
 *
 * 📌 DOM ELEMENT DEPENDENCY:
 * - lstEventos (id="lstEventos"): Syncfusion DropDownList para eventos
 * - Configurado com fields: { value: 'EventoId', text: 'Evento' }
 * - Inicializado em controls-init.js
 * - Visibilidade controlada por divEvento section (show/hide)
 * - Parent section mostrado quando finalidade tem eventos
 *
 * 📌 TRATAMENTO DE ERROS:
 * 1. adicionar: try-catch com TratamentoErroComLinha, sempre retorna objeto
 *    (nunca lança exceção)
 * 2. listar: try-catch outer + error callback com criarErroAjax, pode reject
 *    Promise ou retornar {success: false, data: []}
 * 3. atualizarListaDropdown: try-catch com TratamentoErroComLinha, silent
 *    return se elemento não existe
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - ApiClient.post usado em adicionar (centralizado em api-client.js)
 * - $.ajax usado em listar (direct jQuery call, não via ApiClient)
 * - Inconsistência de pattern: adicionar usa ApiClient, listar usa $.ajax
 * - listar retorna Promise wrapper (resolve/reject) para async/await chain
 * - atualizarListaDropdown depende de listar() success (throw se falha)
 * - eventoId opcional em atualizarListaDropdown (null/undefined = não seleciona)
 * - Syncfusion ej2_instances[0] acessa instância JavaScript do componente
 * - dataSource setter + dataBind() é pattern Syncfusion para refresh
 * - value setter em Syncfusion DropDownList seleciona item por ID
 * - Silent return em atualizarListaDropdown se lstEventos não existe (normal
 *   em páginas sem esse dropdown)
 * - TratamentoErroComLinha loga erro em console + envia para backend log
 * - criarErroAjax enriquece erro com status, url, method, mensagemUsuario
 * - Singleton pattern window.EventoService facilita acesso global
 * - Instância criada imediatamente ao carregar script (não lazy)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

class EventoService
{
    constructor()
    {
        this.api = window.ApiClient;
    }

    /**
     * Adiciona novo evento
     * param {Object} dados - Dados do evento
     * returns {Promise<Object>} Resultado da operação
     */
    async adicionar(dados)
    {
        try
        {
            const response = await this.api.post('/api/Viagem/AdicionarEvento', dados);

            if (response.success)
            {
                return {
                    success: true,
                    message: response.message,
                    eventoId: response.eventoId,
                    eventoText: response.eventoText
                };
            } else
            {
                return {
                    success: false,
                    message: response.message || "Erro ao adicionar evento"
                };
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("evento.service.js", "adicionar", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Lista eventos
     * returns {Promise<Array>} Lista de eventos
     */
    async listar()
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: "/Viagens/Upsert?handler=AJAXPreencheListaEventos",
                    method: "GET",
                    datatype: "json",
                    success: function (res)
                    {
                        const eventos = res.data.map(item => ({
                            EventoId: item.eventoId,
                            Evento: item.nome
                        }));

                        resolve({
                            success: true,
                            data: eventos
                        });
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        Alerta.TratamentoErroComLinha("evento.service.js", "listar", erro);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("evento.service.js", "listar", error);
            return {
                success: false,
                error: error.message,
                data: []
            };
        }
    }

    /**
     * Atualiza lista de eventos em dropdown
     * param {number} eventoId - ID do evento para selecionar
     * returns {Promise<void>}
     */
    async atualizarListaDropdown(eventoId = null)
    {
        try
        {
            const result = await this.listar();

            if (!result.success)
            {
                throw new Error(result.error);
            }

            const lstEventosElement = document.getElementById("lstEventos");
            if (!lstEventosElement || !lstEventosElement.ej2_instances || !lstEventosElement.ej2_instances[0])
            {
                return;
            }

            const lstEventos = lstEventosElement.ej2_instances[0];
            lstEventos.dataSource = result.data;
            lstEventos.dataBind();

            if (eventoId)
            {
                lstEventos.value = eventoId;
            }
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("evento.service.js", "atualizarListaDropdown", error);
        }
    }
}

// Instância global
window.EventoService = new EventoService();
