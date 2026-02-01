/* ****************************************************************************************
 * ⚡ ARQUIVO: state.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento centralizado de estado global da aplicação de
 *                   agendamento. Implementa padrão Observer para reatividade, getters/setters
 *                   path-based, e aliases para compatibilidade com código legado.
 * 📥 ENTRADAS     : Chamadas a get(path), set(path, value), update(updates),
 *                   subscribe(path, callback)
 * 📤 SAÍDAS       : Notificações a listeners via callbacks, retorno de valores de estado,
 *                   console.error via Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : Módulos de agendamento (main.js, components/*, services/*),
 *                   código legado via aliases window.viagemId, window.modalLock, etc.
 * 🔄 CHAMA        : Callbacks de listeners (subscribe), Alerta.TratamentoErroComLinha,
 *                   Map/Set métodos nativos
 * 📦 DEPENDÊNCIAS : Alerta.js (TratamentoErroComLinha), ES6 class/Map/Set
 * 📝 OBSERVAÇÕES  : Exporta window.AppState (instância global). Define 21 aliases
 *                   Object.defineProperty para compatibilidade com código legado.
 *                   Todas as funções têm try-catch completo. Padrão path: "viagem.id",
 *                   "ui.modalLock", etc. (dot notation).
 *
 * 📋 ÍNDICE DE MÉTODOS DA CLASSE (8 métodos + constructor):
 *
 * ┌─ CONSTRUCTOR E ESTRUTURA DE ESTADO ──────────────────────────────────────┐
 * │ 1. constructor()                                                         │
 * │    → Inicializa this.state com estrutura padrão:                        │
 * │      • viagem: { id, idAJAX, recorrenciaId, recorrenciaIdAJAX,         │
 * │        dataInicial, dataInicialList, horaInicial, status, dados }       │
 * │      • ui: { modalAberto, modalLock, isSubmitting,                     │
 * │        carregandoAgendamento, carregandoViagemBloqueada,               │
 * │        inserindoRequisitante, transformandoEmViagem }                  │
 * │      • recorrencia: { agendamentos, editarTodos, datasSelecionadas }   │
 * │      • calendario: { instancia, selectedDates }                        │
 * │    → Inicializa this.listeners = new Map()                              │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MÉTODOS DE LEITURA E ESCRITA ──────────────────────────────────────────┐
 * │ 2. get(path)                                                             │
 * │    → Navega state usando path.split('.').reduce()                       │
 * │    → Exemplo: get("viagem.id") → this.state.viagem.id                  │
 * │    → Retorna valor ou null em erro                                      │
 * │    → try-catch: Alerta.TratamentoErroComLinha("state.js", "get")       │
 * │                                                                          │
 * │ 3. set(path, value)                                                      │
 * │    → Navega até último nível, atualiza target[lastKey] = value         │
 * │    → Guarda oldValue antes de atribuir                                  │
 * │    → Chama notify(path, value, oldValue) para disparar listeners      │
 * │    → try-catch: Alerta.TratamentoErroComLinha("state.js", "set")       │
 * │                                                                          │
 * │ 4. update(updates)                                                       │
 * │    → Recebe Object { path1: value1, path2: value2, ... }               │
 * │    → Itera com Object.entries, chama set(path, value) para cada um    │
 * │    → Permite atualização batch de múltiplos paths                      │
 * │    → try-catch: Alerta.TratamentoErroComLinha("state.js", "update")    │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ PADRÃO OBSERVER (REATIVIDADE) ──────────────────────────────────────────┐
 * │ 5. subscribe(path, callback)                                            │
 * │    → Registra callback no Map listeners.get(path)                      │
 * │    → Cria array vazio se path ainda não existe no Map                 │
 * │    → Retorna função unsubscribe: () => remove callback do array       │
 * │    → Uso: const unsub = AppState.subscribe("viagem.id", fn);          │
 * │    →       unsub(); // cancela observação                              │
 * │    → try-catch: retorna () => {} em erro                               │
 * │                                                                          │
 * │ 6. notify(path, newValue, oldValue)                                     │
 * │    → Obtém callbacks do listeners.get(path)                            │
 * │    → Itera e executa cada callback(newValue, oldValue)                │
 * │    → try-catch individual para cada callback                           │
 * │    → Erros em callbacks não param execução dos outros                 │
 * │    → try-catch externo: Alerta.TratamentoErroComLinha("notify")       │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ RESET E DEBUG ──────────────────────────────────────────────────────────┐
 * │ 7. reset()                                                              │
 * │    → Reinicializa this.state para estrutura padrão (igual constructor) │
 * │    → Chama notify('*', this.state, null) para avisar reset global     │
 * │    → Limpa todos os valores de viagem, ui, recorrencia, calendario    │
 * │    → try-catch: Alerta.TratamentoErroComLinha("state.js", "reset")     │
 * │                                                                          │
 * │ 8. getState()                                                           │
 * │    → Retorna cópia shallow do estado: { ...this.state }               │
 * │    → Usado para debug (console.log, inspecionar estado)               │
 * │    → Não tem try-catch (método simples)                               │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ ALIASES PARA COMPATIBILIDADE (21 Object.defineProperty) ──────────────┐
 * │ 9. window.viagemId                                                      │
 * │    → get: AppState.get('viagem.id')                                    │
 * │    → set: AppState.set('viagem.id', value)                             │
 * │                                                                          │
 * │ 10. window.viagemId_AJAX → 'viagem.idAJAX'                             │
 * │ 11. window.recorrenciaViagemId → 'viagem.recorrenciaId'                │
 * │ 12. window.recorrenciaViagemId_AJAX → 'viagem.recorrenciaIdAJAX'       │
 * │ 13. window.dataInicial → 'viagem.dataInicial'                          │
 * │ 14. window.dataInicial_List → 'viagem.dataInicialList'                 │
 * │ 15. window.horaInicial → 'viagem.horaInicial'                          │
 * │ 16. window.StatusViagem → 'viagem.status'                              │
 * │ 17. window.modalLock → 'ui.modalLock'                                  │
 * │ 18. window.isSubmitting → 'ui.isSubmitting'                            │
 * │ 19. window.CarregandoAgendamento → 'ui.carregandoAgendamento'          │
 * │ 20. window.CarregandoViagemBloqueada → 'ui.carregandoViagemBloqueada'  │
 * │ 21. window.InserindoRequisitante → 'ui.inserindoRequisitante'          │
 * │ 22. window.transformandoEmViagem → 'ui.transformandoEmViagem'          │
 * │ 23. window.agendamentosRecorrentes → 'recorrencia.agendamentos'        │
 * │ 24. window.editarTodosRecorrentes → 'recorrencia.editarTodos'          │
 * │ 25. window.datasSelecionadas → 'recorrencia.datasSelecionadas'         │
 * │ 26. window.calendar → 'calendario.instancia'                           │
 * │ 27. window.selectedDates → 'calendario.selectedDates'                  │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 📌 ESTRUTURA COMPLETA DO STATE:
 * {
 *   viagem: {
 *     id: "",                    // GUID da viagem
 *     idAJAX: "",                // ID para requisições AJAX
 *     recorrenciaId: "",         // GUID da recorrência
 *     recorrenciaIdAJAX: "",     // ID da recorrência para AJAX
 *     dataInicial: "",           // Data inicial formatada
 *     dataInicialList: "",       // Data inicial para listas
 *     horaInicial: "",           // Hora inicial
 *     status: "Aberta",          // Status da viagem
 *     dados: null                // Dados completos da viagem
 *   },
 *   ui: {
 *     modalAberto: false,                   // Modal de viagem aberto
 *     modalLock: false,                     // Lock para prevenir fechamento
 *     isSubmitting: false,                  // Enviando formulário
 *     carregandoAgendamento: false,         // Loading de agendamento
 *     carregandoViagemBloqueada: false,     // Loading de viagem bloqueada
 *     inserindoRequisitante: false,         // Inserindo novo requisitante
 *     transformandoEmViagem: false          // Transformando agendamento em viagem
 *   },
 *   recorrencia: {
 *     agendamentos: [],           // Lista de agendamentos recorrentes
 *     editarTodos: false,         // Flag editar todos da série
 *     datasSelecionadas: []       // Datas selecionadas no calendário
 *   },
 *   calendario: {
 *     instancia: null,            // Instância do Syncfusion Calendar
 *     selectedDates: []           // Datas selecionadas (array de Date)
 *   }
 * }
 *
 * 🔄 FLUXO DE USO TÍPICO:
 * 1. Código legado: window.viagemId = "abc-123" → AppState.set('viagem.id', "abc-123")
 * 2. AppState.set dispara notify('viagem.id', "abc-123", oldValue)
 * 3. Listeners registrados via subscribe são executados
 * 4. Código moderno: AppState.get('viagem.id') → "abc-123"
 * 5. Código legado: window.viagemId → "abc-123"
 *
 * 🔄 FLUXO DE OBSERVAÇÃO (OBSERVER):
 * 1. Componente: const unsub = AppState.subscribe('viagem.id', (novo, antigo) => {...})
 * 2. Outro módulo: AppState.set('viagem.id', "xyz-789")
 * 3. notify dispara → callback é executado com (novo="xyz-789", antigo="abc-123")
 * 4. Cleanup: unsub() remove callback do listeners
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Path notation: usa '.' para navegação (ex: "viagem.dados.titulo")
 * - Map de listeners: permite múltiplos callbacks por path
 * - Cada callback tem try-catch individual → erros não param outros callbacks
 * - reset() notifica '*' como path especial para reset global
 * - Object.defineProperty: permite código legado usar variáveis globais antigas
 * - getState() retorna shallow copy → modificações não afetam state original
 * - notify sempre passa newValue e oldValue para callbacks
 * - subscribe retorna função unsubscribe (cleanup)
 * - update permite batch: AppState.update({ "viagem.id": "1", "ui.modalLock": true })
 * - Todas as funções têm Alerta.TratamentoErroComLinha para logging de erros
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Gerenciador de estado global da aplicação
 */
class StateManager
{
    constructor()
    {
        this.state = {
            // Dados da viagem atual
            viagem: {
                id: "",
                idAJAX: "",
                recorrenciaId: "",
                recorrenciaIdAJAX: "",
                dataInicial: "",
                dataInicialList: "",
                horaInicial: "",
                status: "Aberta",
                dados: null
            },

            // Estado da UI
            ui: {
                modalAberto: false,
                modalLock: false,
                isSubmitting: false,
                carregandoAgendamento: false,
                carregandoViagemBloqueada: false,
                inserindoRequisitante: false,
                transformandoEmViagem: false
            },

            // Recorrência
            recorrencia: {
                agendamentos: [],
                editarTodos: false,
                datasSelecionadas: []
            },

            // Calendário
            calendario: {
                instancia: null,
                selectedDates: []
            }
        };

        this.listeners = new Map();
    }

    /**
     * Obter valor do estado
     * param {string} path - Caminho do estado (ex: "viagem.id")
     * returns {*} Valor do estado
     */
    get(path)
    {
        try
        {
            return path.split('.').reduce((obj, key) => obj?.[key], this.state);
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("state.js", "get", error);
            return null;
        }
    }

    /**
     * Definir valor do estado
     * param {string} path - Caminho do estado
     * param {*} value - Novo valor
     */
    set(path, value)
    {
        try
        {
            const keys = path.split('.');
            const lastKey = keys.pop();
            const target = keys.reduce((obj, key) => obj[key], this.state);

            const oldValue = target[lastKey];
            target[lastKey] = value;

            // Notificar listeners
            this.notify(path, value, oldValue);
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("state.js", "set", error);
        }
    }

    /**
     * Atualizar múltiplos valores
     * param {Object} updates - Objeto com paths e valores
     */
    update(updates)
    {
        try
        {
            Object.entries(updates).forEach(([path, value]) =>
            {
                this.set(path, value);
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("state.js", "update", error);
        }
    }

    /**
     * Observar mudanças em um path
     * param {string} path - Caminho a observar
     * param {Function} callback - Função callback
     * returns {Function} Função para cancelar a observação
     */
    subscribe(path, callback)
    {
        try
        {
            if (!this.listeners.has(path))
            {
                this.listeners.set(path, []);
            }
            this.listeners.get(path).push(callback);

            return () =>
            {
                const callbacks = this.listeners.get(path);
                const index = callbacks.indexOf(callback);
                if (index > -1)
                {
                    callbacks.splice(index, 1);
                }
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("state.js", "subscribe", error);
            return () => { };
        }
    }

    /**
     * Notificar listeners
     */
    notify(path, newValue, oldValue)
    {
        try
        {
            const callbacks = this.listeners.get(path) || [];
            callbacks.forEach(callback =>
            {
                try
                {
                    callback(newValue, oldValue);
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("state.js", "notify_callback", error);
                }
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("state.js", "notify", error);
        }
    }

    /**
     * Resetar estado
     */
    reset()
    {
        try
        {
            this.state = {
                viagem: {
                    id: "",
                    idAJAX: "",
                    recorrenciaId: "",
                    recorrenciaIdAJAX: "",
                    dataInicial: "",
                    dataInicialList: "",
                    horaInicial: "",
                    status: "Aberta",
                    dados: null
                },
                ui: {
                    modalAberto: false,
                    modalLock: false,
                    isSubmitting: false,
                    carregandoAgendamento: false,
                    carregandoViagemBloqueada: false,
                    inserindoRequisitante: false,
                    transformandoEmViagem: false
                },
                recorrencia: {
                    agendamentos: [],
                    editarTodos: false,
                    datasSelecionadas: []
                },
                calendario: {
                    instancia: null,
                    selectedDates: []
                }
            };

            this.notify('*', this.state, null);
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("state.js", "reset", error);
        }
    }

    /**
     * Obter estado completo (para debug)
     */
    getState()
    {
        return { ...this.state };
    }
}

// Instância global
window.AppState = new StateManager();

// Aliases para compatibilidade com código legado
Object.defineProperty(window, 'viagemId', {
    get: () => window.AppState.get('viagem.id'),
    set: (value) => window.AppState.set('viagem.id', value)
});

Object.defineProperty(window, 'viagemId_AJAX', {
    get: () => window.AppState.get('viagem.idAJAX'),
    set: (value) => window.AppState.set('viagem.idAJAX', value)
});

Object.defineProperty(window, 'recorrenciaViagemId', {
    get: () => window.AppState.get('viagem.recorrenciaId'),
    set: (value) => window.AppState.set('viagem.recorrenciaId', value)
});

Object.defineProperty(window, 'recorrenciaViagemId_AJAX', {
    get: () => window.AppState.get('viagem.recorrenciaIdAJAX'),
    set: (value) => window.AppState.set('viagem.recorrenciaIdAJAX', value)
});

Object.defineProperty(window, 'dataInicial', {
    get: () => window.AppState.get('viagem.dataInicial'),
    set: (value) => window.AppState.set('viagem.dataInicial', value)
});

Object.defineProperty(window, 'dataInicial_List', {
    get: () => window.AppState.get('viagem.dataInicialList'),
    set: (value) => window.AppState.set('viagem.dataInicialList', value)
});

Object.defineProperty(window, 'horaInicial', {
    get: () => window.AppState.get('viagem.horaInicial'),
    set: (value) => window.AppState.set('viagem.horaInicial', value)
});

Object.defineProperty(window, 'StatusViagem', {
    get: () => window.AppState.get('viagem.status'),
    set: (value) => window.AppState.set('viagem.status', value)
});

Object.defineProperty(window, 'modalLock', {
    get: () => window.AppState.get('ui.modalLock'),
    set: (value) => window.AppState.set('ui.modalLock', value)
});

Object.defineProperty(window, 'isSubmitting', {
    get: () => window.AppState.get('ui.isSubmitting'),
    set: (value) => window.AppState.set('ui.isSubmitting', value)
});

Object.defineProperty(window, 'CarregandoAgendamento', {
    get: () => window.AppState.get('ui.carregandoAgendamento'),
    set: (value) => window.AppState.set('ui.carregandoAgendamento', value)
});

Object.defineProperty(window, 'CarregandoViagemBloqueada', {
    get: () => window.AppState.get('ui.carregandoViagemBloqueada'),
    set: (value) => window.AppState.set('ui.carregandoViagemBloqueada', value)
});

Object.defineProperty(window, 'InserindoRequisitante', {
    get: () => window.AppState.get('ui.inserindoRequisitante'),
    set: (value) => window.AppState.set('ui.inserindoRequisitante', value)
});

Object.defineProperty(window, 'transformandoEmViagem', {
    get: () => window.AppState.get('ui.transformandoEmViagem'),
    set: (value) => window.AppState.set('ui.transformandoEmViagem', value)
});

Object.defineProperty(window, 'agendamentosRecorrentes', {
    get: () => window.AppState.get('recorrencia.agendamentos'),
    set: (value) => window.AppState.set('recorrencia.agendamentos', value)
});

Object.defineProperty(window, 'editarTodosRecorrentes', {
    get: () => window.AppState.get('recorrencia.editarTodos'),
    set: (value) => window.AppState.set('recorrencia.editarTodos', value)
});

Object.defineProperty(window, 'datasSelecionadas', {
    get: () => window.AppState.get('recorrencia.datasSelecionadas'),
    set: (value) => window.AppState.set('recorrencia.datasSelecionadas', value)
});

Object.defineProperty(window, 'calendar', {
    get: () => window.AppState.get('calendario.instancia'),
    set: (value) => window.AppState.set('calendario.instancia', value)
});

Object.defineProperty(window, 'selectedDates', {
    get: () => window.AppState.get('calendario.selectedDates'),
    set: (value) => window.AppState.set('calendario.selectedDates', value)
});
