/* ****************************************************************************************
 * ⚡ ARQUIVO: alertas_recorrencia.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Controle de recorrência para Alertas FrotiX (v2.0). Gerencia campos
 *                   dinâmicos de recorrência baseados no TipoExibicao (1-8), calendário
 *                   Syncfusion de seleção múltipla, badges de contador e validação.
 * 📥 ENTRADAS     : TipoExibicao dropdown (1-8), calendário multi-select, dropdown dias/mes,
 *                   dados de alerta em modo edição
 * 📤 SAÍDAS       : Exibição/ocultação de campos, valores em DatasSelecionadas hidden field,
 *                   objeto dados coletados para submit, atualização de badges
 * 🔗 CHAMADA POR  : AlertasFrotiX Upsert page, dropdown TipoExibicao.change event
 * 🔄 CHAMA        : Syncfusion Calendar/DropDownList APIs, mostrarElemento, esconderElemento,
 *                   Alerta.TratamentoErroComLinha, console.log/warn/error
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 Calendar, DropDownList, Alerta.js (TratamentoErroComLinha)
 * 📝 OBSERVAÇÕES  : TipoExibicao 1-3 = não recorrente, 4-8 = recorrente. Calendário usa
 *                   isMultiSelection=true. Todas as funções têm try-catch completo.
 *                   Exports para window: calendarioAlertaInstance, datasAlertaSelecionadas,
 *                   e 6 funções principais.
 *
 * 📋 ÍNDICE DE FUNÇÕES (13 funções + exports + DOMContentLoaded):
 *
 * ┌─ INICIALIZAÇÃO E CONFIGURAÇÃO ──────────────────────────────────────────┐
 * │ 1. inicializarControlesRecorrenciaAlerta()                             │
 * │    → Entry point: chama configurarEventos e verificarEstado           │
 * │    → Console.log de inicialização                                      │
 * │                                                                         │
 * │ 2. configurarEventosRecorrenciaAlerta()                                │
 * │    → Obtém dropdown #TipoExibicao via ej2_instances                   │
 * │    → Guarda handler original (se existir)                             │
 * │    → Override .change para chamar mostrarCamposPorTipoExibicao       │
 * │    → Fallback: setTimeout 500ms se dropdown ainda não inicializado   │
 * │                                                                         │
 * │ 3. verificarEstadoRecorrenciaAlerta()                                  │
 * │    → Verifica valor inicial do TipoExibicao (modo edição)            │
 * │    → Chama mostrarCamposPorTipoExibicao com valor atual              │
 * │                                                                         │
 * │ 4. onTipoExibicaoChange(e)                                             │
 * │    → Handler legado/alternativo para evento change                    │
 * │    → Extrai valor de e.target.value ou e.value                        │
 * │    → Chama mostrarCamposPorTipoExibicao                               │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ LÓGICA DE EXIBIÇÃO DE CAMPOS ──────────────────────────────────────────┐
 * │ 5. mostrarCamposPorTipoExibicao(tipoExibicao)                          │
 * │    → Esconde TODOS os campos primeiro (divDias, divDiaMes, calendar)  │
 * │    → Se tipo < 4 (não recorrente): retorna early, sem campos extras   │
 * │    → Switch case 4-8:                                                  │
 * │      • Tipo 4 (Diário): sem campos extras (seg-sex automático)        │
 * │      • Tipo 5 (Semanal) / 6 (Quinzenal): mostra divDiasAlerta        │
 * │      • Tipo 7 (Mensal): mostra divDiaMesAlerta                        │
 * │      • Tipo 8 (Dias Variados): mostra calendarContainer, init se needed│
 * │    → Console.log de cada tipo                                          │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CALENDÁRIO SYNCFUSION ──────────────────────────────────────────────────┐
 * │ 6. initCalendarioAlerta()                                              │
 * │    → Obtém container #calDatasSelecionadasAlerta                       │
 * │    → Se já existe instância ej2_instances[0]: reutiliza               │
 * │    → Cria new ej.calendars.Calendar({ isMultiSelection: true })       │
 * │    → change event: atualiza datasAlertaSelecionadas, badge, hidden    │
 * │    → appendTo('#calDatasSelecionadasAlerta')                           │
 * │    → Atribui a window.calendarioAlertaInstance                         │
 * │                                                                         │
 * │ 7. atualizarBadgeContador()                                            │
 * │    → Atualiza #badgeDatasSelecionadas                                  │
 * │    → badge.textContent = count                                         │
 * │    → display: flex se count > 0, none se count = 0                    │
 * │                                                                         │
 * │ 8. atualizarCampoHidden()                                              │
 * │    → Formata datasAlertaSelecionadas para ISO (YYYY-MM-DD)            │
 * │    → Join com ',' → #DatasSelecionadas.value                          │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ COLETA E PREENCHIMENTO DE DADOS ───────────────────────────────────────┐
 * │ 9. coletarDadosRecorrenciaAlerta()                                     │
 * │    → Obtém TipoExibicao do dropdown                                    │
 * │    → Se < 4: retorna { TipoExibicao } apenas                          │
 * │    → Switch case 4-8:                                                  │
 * │      • Tipo 5/6: coleta lstDiasAlerta.value (array dias semana)       │
 * │      • Tipo 7: coleta lstDiasMesAlerta.value (int dia do mês)         │
 * │      • Tipo 8: coleta datasAlertaSelecionadas (array Date)            │
 * │    → Retorna objeto com todos os campos relevantes                     │
 * │                                                                         │
 * │ 10. preencherCamposRecorrenciaAlerta(alerta)                           │
 * │     → Recebe objeto alerta do backend (modo edição)                   │
 * │     → Chama mostrarCamposPorTipoExibicao(alerta.TipoExibicao)         │
 * │     → Switch case 5-8:                                                 │
 * │       • Tipo 5/6: preenche lstDiasAlerta com alerta.DiasSemana        │
 * │       • Tipo 7: preenche lstDiasMesAlerta com alerta.DiaMesRecorrencia│
 * │       • Tipo 8: parse alerta.DatasSelecionadas (string ou array)      │
 * │         → atualiza calendário.values, dataBind, badge                 │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES AUXILIARES ─────────────────────────────────────────────────────┐
 * │ 11. mostrarElemento(elementId)                                         │
 * │     → document.getElementById(elementId).style.display = 'block'      │
 * │     → Console.log sucesso ou warn se não encontrado                    │
 * │                                                                         │
 * │ 12. esconderElemento(elementId)                                        │
 * │     → document.getElementById(elementId).style.display = 'none'       │
 * │                                                                         │
 * │ 13. formatarDataISO(data)                                              │
 * │     → Converte Date para "YYYY-MM-DD"                                 │
 * │     → Usa slice(-2) para pad com zeros à esquerda                     │
 * │     → Retorna string vazia em caso de erro                             │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ INICIALIZAÇÃO E EXPORTS ────────────────────────────────────────────────┐
 * │ 14. DOMContentLoaded handler                                           │
 * │     → setTimeout 300ms (aguarda Syncfusion init)                       │
 * │     → Chama inicializarControlesRecorrenciaAlerta()                    │
 * │     → try-catch duplo (outer e inner timeout)                          │
 * │                                                                         │
 * │ 15. window exports (lines 461-466)                                     │
 * │     → window.inicializarControlesRecorrenciaAlerta                     │
 * │     → window.verificarEstadoRecorrenciaAlerta                          │
 * │     → window.mostrarCamposPorTipoExibicao                              │
 * │     → window.initCalendarioAlerta                                      │
 * │     → window.coletarDadosRecorrenciaAlerta                             │
 * │     → window.preencherCamposRecorrenciaAlerta                          │
 * │     → window.calendarioAlertaInstance                                  │
 * │     → window.datasAlertaSelecionadas                                   │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 📌 TIPOS DE EXIBIÇÃO (TipoExibicao):
 * 1 = Ao abrir (não recorrente)
 * 2 = Horário específico (não recorrente)
 * 3 = Data/Hora específica (não recorrente)
 * 4 = Recorrente Diário (seg-sex automático, sem campos extras)
 * 5 = Recorrente Semanal (requer dias da semana)
 * 6 = Recorrente Quinzenal (requer dias da semana)
 * 7 = Recorrente Mensal (requer dia do mês 1-31)
 * 8 = Recorrente Dias Variados (requer calendário multi-select)
 *
 * 🔄 FLUXO DE INICIALIZAÇÃO:
 * 1. DOMContentLoaded → setTimeout 300ms
 * 2. inicializarControlesRecorrenciaAlerta
 * 3. configurarEventosRecorrenciaAlerta: override TipoExibicao.change
 * 4. verificarEstadoRecorrenciaAlerta: aplica estado inicial (modo edição)
 * 5. Sistema fica aguardando mudanças no TipoExibicao dropdown
 *
 * 🔄 FLUXO DE MUDANÇA DE TIPO:
 * 1. Usuário seleciona TipoExibicao no dropdown
 * 2. .change event dispara
 * 3. mostrarCamposPorTipoExibicao(valor)
 * 4. Esconde todos os campos
 * 5. Mostra campos específicos do tipo (switch case)
 * 6. Se tipo 8: initCalendarioAlerta (se ainda não existe)
 *
 * 🔄 FLUXO DE SUBMIT (coletarDados):
 * 1. Form submit chama coletarDadosRecorrenciaAlerta()
 * 2. Lê TipoExibicao do dropdown
 * 3. Switch case: coleta campos específicos (DiasSemana, DiaMesRecorrencia, DatasSelecionadas)
 * 4. Retorna objeto { TipoExibicao, [campos específicos] }
 * 5. Backend recebe e processa recorrência
 *
 * 🔄 FLUXO DE EDIÇÃO (preencherCampos):
 * 1. Backend envia objeto alerta com dados de recorrência
 * 2. preencherCamposRecorrenciaAlerta(alerta)
 * 3. mostrarCamposPorTipoExibicao: exibe campos corretos
 * 4. Switch case: preenche dropdowns/calendário com valores salvos
 * 5. dataBind() força refresh dos componentes Syncfusion
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Delay de 300ms na init: garante que Syncfusion EJ2 terminou de inicializar
 * - Fallback de 500ms em configurarEventos: caso dropdown ainda não esteja pronto
 * - Calendário: isMultiSelection=true permite múltiplas datas
 * - Badge circular: display flex para centralizar número
 * - DatasSelecionadas hidden field: formato "YYYY-MM-DD,YYYY-MM-DD,..."
 * - Todas as funções têm try-catch com Alerta.TratamentoErroComLinha
 * - Exports para window permitem chamada externa de outras páginas
 * - console.log/warn abundante para debug
 *
 * 🔌 VERSÃO: 2.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

// Variável global para instância do calendário
var calendarioAlertaInstance = null;
var datasAlertaSelecionadas = [];

// Exportar para window para uso global
window.calendarioAlertaInstance = null;
window.datasAlertaSelecionadas = [];

/**
 * Inicializa os controles de recorrência
 */
function inicializarControlesRecorrenciaAlerta() {
    try {
        console.log('Inicializando controles de recorrência v2...');

        // Configurar eventos
        configurarEventosRecorrenciaAlerta();

        // Verificar estado inicial (edição)
        verificarEstadoRecorrenciaAlerta();

        console.log('✅ Controles de recorrência inicializados');
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'inicializarControlesRecorrenciaAlerta');
    }
}

/**
 * Configura os event handlers para o dropdown de TipoExibicao
 */
function configurarEventosRecorrenciaAlerta() {
    try {
        // Event handler para mudança no TipoExibicao (Syncfusion dropdown)
        // CORREÇÃO: O ID correto é "TipoExibicao", não "lstTipoExibicao"
        var tipoExibicaoElement = document.getElementById('TipoExibicao');
        
        if (tipoExibicaoElement && tipoExibicaoElement.ej2_instances && tipoExibicaoElement.ej2_instances[0]) {
            var dropdown = tipoExibicaoElement.ej2_instances[0];
            
            // Guardar referência do handler original se existir
            var originalChangeHandler = dropdown.change;
            
            // Adicionar handler para controles de recorrência
            dropdown.change = function(args) {
                try {
                    // Chamar handler original se existir
                    if (originalChangeHandler && typeof originalChangeHandler === 'function') {
                        originalChangeHandler.call(this, args);
                    }
                    
                    // Processar controles de recorrência
                    var tipoExibicao = parseInt(args.value);
                    mostrarCamposPorTipoExibicao(tipoExibicao);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(error, 'TipoExibicao.change.recorrencia');
                }
            };
            
            console.log('✅ Event handler de recorrência configurado para TipoExibicao');
        } else {
            console.warn('⚠️ Dropdown TipoExibicao não encontrado ou não inicializado');
            
            // Fallback: tentar novamente após um delay
            setTimeout(function() {
                try {
                    var el = document.getElementById('TipoExibicao');
                    if (el && el.ej2_instances && el.ej2_instances[0]) {
                        el.ej2_instances[0].change = function(args) {
                            try {
                                var tipoExibicao = parseInt(args.value);
                                mostrarCamposPorTipoExibicao(tipoExibicao);
                            } catch (error) {
                                Alerta.TratamentoErroComLinha(error, 'TipoExibicao.change.fallback');
                            }
                        };
                        console.log('✅ Event handler de recorrência configurado (fallback)');
                    }
                } catch (err) {
                    console.error('Erro no fallback:', err);
                }
            }, 500);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'configurarEventosRecorrenciaAlerta');
    }
}

/**
 * Verifica estado inicial para modo de edição
 */
function verificarEstadoRecorrenciaAlerta() {
    try {
        var tipoExibicaoElement = document.getElementById('TipoExibicao');
        
        if (tipoExibicaoElement && tipoExibicaoElement.ej2_instances && tipoExibicaoElement.ej2_instances[0]) {
            var valor = tipoExibicaoElement.ej2_instances[0].value;
            if (valor) {
                var tipoExibicao = parseInt(valor);
                console.log('Estado inicial - TipoExibicao:', tipoExibicao);
                mostrarCamposPorTipoExibicao(tipoExibicao);
            }
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'verificarEstadoRecorrenciaAlerta');
    }
}

/**
 * Handler para mudança no TipoExibicao
 * @param {Event} e - Evento de mudança
 */
function onTipoExibicaoChange(e) {
    try {
        var tipoExibicao = parseInt(e.target.value || e.value);
        mostrarCamposPorTipoExibicao(tipoExibicao);
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'onTipoExibicaoChange');
    }
}

/**
 * Mostra/oculta campos baseado no TipoExibicao
 * 
 * TIPOS:
 * 1 = Ao abrir
 * 2 = Horário específico
 * 3 = Data/Hora específica
 * 4 = Recorrente Diário
 * 5 = Recorrente Semanal
 * 6 = Recorrente Quinzenal
 * 7 = Recorrente Mensal
 * 8 = Recorrente Dias Variados
 */
function mostrarCamposPorTipoExibicao(tipoExibicao) {
    try {
        console.log('mostrarCamposPorTipoExibicao:', tipoExibicao);

        // Esconder todos os campos de recorrência primeiro
        esconderElemento('divDiasAlerta');
        esconderElemento('divDiaMesAlerta');
        esconderElemento('calendarContainerAlerta');

        // Se não é recorrente (1, 2, 3), não mostrar campos de recorrência
        if (tipoExibicao < 4) {
            console.log('Tipo não recorrente, ocultando campos de recorrência');
            return;
        }

        // Se é recorrente (4-8), mostrar campos apropriados
        switch (tipoExibicao) {
            case 4: // Diário - Não precisa de campos extras (seg-sex automático)
                console.log('Tipo 4 - Diário: sem campos extras');
                break;

            case 5: // Semanal
            case 6: // Quinzenal
                console.log('Tipo ' + tipoExibicao + ' - Semanal/Quinzenal: mostrando dias da semana');
                mostrarElemento('divDiasAlerta');
                break;

            case 7: // Mensal
                console.log('Tipo 7 - Mensal: mostrando dia do mês');
                mostrarElemento('divDiaMesAlerta');
                break;

            case 8: // Dias Variados
                console.log('Tipo 8 - Dias Variados: mostrando calendário');
                mostrarElemento('calendarContainerAlerta');
                if (!calendarioAlertaInstance) {
                    initCalendarioAlerta();
                }
                break;
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'mostrarCamposPorTipoExibicao');
    }
}

/**
 * Inicializa o calendário Syncfusion para seleção de datas
 */
function initCalendarioAlerta() {
    try {
        var container = document.getElementById('calDatasSelecionadasAlerta');
        if (!container) {
            console.warn('Container do calendário não encontrado');
            return;
        }

        // Verificar se já existe uma instância
        if (container.ej2_instances && container.ej2_instances[0]) {
            calendarioAlertaInstance = container.ej2_instances[0];
            window.calendarioAlertaInstance = calendarioAlertaInstance;
            console.log('Calendário já inicializado, usando instância existente');
            return;
        }

        calendarioAlertaInstance = new ej.calendars.Calendar({
            isMultiSelection: true,
            change: function (args) {
                try {
                    datasAlertaSelecionadas = args.values || [];
                    window.datasAlertaSelecionadas = datasAlertaSelecionadas;
                    atualizarBadgeContador();
                    atualizarCampoHidden();
                    console.log('Datas selecionadas:', datasAlertaSelecionadas.length);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(error, 'calendarioAlerta.change');
                }
            }
        });
        calendarioAlertaInstance.appendTo('#calDatasSelecionadasAlerta');
        window.calendarioAlertaInstance = calendarioAlertaInstance;

        console.log('✅ Calendário inicializado');
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'initCalendarioAlerta');
    }
}

/**
 * Atualiza o badge com contador de datas selecionadas
 */
function atualizarBadgeContador() {
    try {
        var badge = document.getElementById('badgeDatasSelecionadas');
        if (badge) {
            var count = datasAlertaSelecionadas.length;
            badge.textContent = count;
            // Usar display:flex para centralizar o número no badge circular
            badge.style.display = count > 0 ? 'flex' : 'none';
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'atualizarBadgeContador');
    }
}

/**
 * Atualiza o campo hidden com as datas selecionadas
 */
function atualizarCampoHidden() {
    try {
        var hiddenField = document.getElementById('DatasSelecionadas');
        if (hiddenField) {
            var datasFormatadas = datasAlertaSelecionadas.map(function (d) {
                return formatarDataISO(d);
            });
            hiddenField.value = datasFormatadas.join(',');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'atualizarCampoHidden');
    }
}

/**
 * Coleta os dados de recorrência do formulário
 * @returns {Object} Dados de recorrência
 */
function coletarDadosRecorrenciaAlerta() {
    try {
        var tipoExibicaoElement = document.getElementById('TipoExibicao');
        var tipoExibicao = 1;
        
        if (tipoExibicaoElement && tipoExibicaoElement.ej2_instances && tipoExibicaoElement.ej2_instances[0]) {
            tipoExibicao = parseInt(tipoExibicaoElement.ej2_instances[0].value) || 1;
        }

        var dados = {
            TipoExibicao: tipoExibicao
        };

        // Se não é recorrente, retornar apenas o tipo
        if (tipoExibicao < 4) {
            return dados;
        }

        // Coletar dados específicos por tipo recorrente
        switch (tipoExibicao) {
            case 4: // Diário - sem dados extras
                break;

            case 5: // Semanal
            case 6: // Quinzenal
                var lstDias = document.getElementById('lstDiasAlerta');
                if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0]) {
                    dados.DiasSemana = lstDias.ej2_instances[0].value || [];
                }
                break;

            case 7: // Mensal
                var lstDiasMes = document.getElementById('lstDiasMesAlerta');
                if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0]) {
                    dados.DiaMesRecorrencia = parseInt(lstDiasMes.ej2_instances[0].value);
                }
                break;

            case 8: // Dias Variados
                dados.DatasSelecionadas = datasAlertaSelecionadas;
                break;
        }

        return dados;
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'coletarDadosRecorrenciaAlerta');
        return null;
    }
}

/**
 * Preenche os campos de recorrência no modo de edição
 * @param {Object} alerta - Dados do alerta
 */
function preencherCamposRecorrenciaAlerta(alerta) {
    try {
        if (!alerta) return;

        var tipoExibicao = alerta.TipoExibicao || 1;

        // Primeiro, mostrar os campos apropriados
        mostrarCamposPorTipoExibicao(tipoExibicao);

        // Se não é recorrente, não preencher nada
        if (tipoExibicao < 4) {
            return;
        }

        // Preencher campos específicos por tipo
        switch (tipoExibicao) {
            case 5: // Semanal
            case 6: // Quinzenal
                if (alerta.DiasSemana && alerta.DiasSemana.length > 0) {
                    var lstDias = document.getElementById('lstDiasAlerta');
                    if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0]) {
                        lstDias.ej2_instances[0].value = alerta.DiasSemana;
                        lstDias.ej2_instances[0].dataBind();
                    }
                }
                break;

            case 7: // Mensal
                if (alerta.DiaMesRecorrencia) {
                    var lstDiasMes = document.getElementById('lstDiasMesAlerta');
                    if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0]) {
                        lstDiasMes.ej2_instances[0].value = alerta.DiaMesRecorrencia;
                        lstDiasMes.ej2_instances[0].dataBind();
                    }
                }
                break;

            case 8: // Dias Variados
                if (alerta.DatasSelecionadas) {
                    var datasStr = typeof alerta.DatasSelecionadas === 'string' 
                        ? alerta.DatasSelecionadas.split(',') 
                        : alerta.DatasSelecionadas;
                    
                    datasAlertaSelecionadas = datasStr.map(function (d) {
                        return new Date(d.trim ? d.trim() : d);
                    });
                    window.datasAlertaSelecionadas = datasAlertaSelecionadas;

                    if (calendarioAlertaInstance) {
                        calendarioAlertaInstance.values = datasAlertaSelecionadas;
                        calendarioAlertaInstance.dataBind();
                    }

                    atualizarBadgeContador();
                }
                break;
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'preencherCamposRecorrenciaAlerta');
    }
}

// ========================================================================
// FUNÇÕES AUXILIARES
// ========================================================================

/**
 * Mostra um elemento pelo ID
 * @param {string} elementId - ID do elemento
 */
function mostrarElemento(elementId) {
    try {
        var el = document.getElementById(elementId);
        if (el) {
            el.style.display = 'block';
            console.log('Elemento mostrado:', elementId);
        } else {
            console.warn('Elemento não encontrado:', elementId);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'mostrarElemento');
    }
}

/**
 * Esconde um elemento pelo ID
 * @param {string} elementId - ID do elemento
 */
function esconderElemento(elementId) {
    try {
        var el = document.getElementById(elementId);
        if (el) {
            el.style.display = 'none';
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'esconderElemento');
    }
}

/**
 * Formata uma data para o formato ISO (YYYY-MM-DD)
 * @param {Date} data - Data a ser formatada
 * @returns {string} Data formatada
 */
function formatarDataISO(data) {
    try {
        var d = new Date(data);
        var mes = ('0' + (d.getMonth() + 1)).slice(-2);
        var dia = ('0' + d.getDate()).slice(-2);
        return d.getFullYear() + '-' + mes + '-' + dia;
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'formatarDataISO');
        return '';
    }
}

// ========================================================================
// INICIALIZAÇÃO
// ========================================================================

// Inicializar quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', function () {
    try {
        // Aguardar um pequeno delay para garantir que os componentes Syncfusion estejam inicializados
        setTimeout(function() {
            try {
                inicializarControlesRecorrenciaAlerta();
            } catch (error) {
                Alerta.TratamentoErroComLinha(error, 'DOMContentLoaded.setTimeout.alertas_recorrencia');
            }
        }, 300);
    } catch (error) {
        Alerta.TratamentoErroComLinha(error, 'DOMContentLoaded.alertas_recorrencia');
    }
});

// Exportar funções para uso global
window.inicializarControlesRecorrenciaAlerta = inicializarControlesRecorrenciaAlerta;
window.verificarEstadoRecorrenciaAlerta = verificarEstadoRecorrenciaAlerta;
window.mostrarCamposPorTipoExibicao = mostrarCamposPorTipoExibicao;
window.initCalendarioAlerta = initCalendarioAlerta;
window.coletarDadosRecorrenciaAlerta = coletarDadosRecorrenciaAlerta;
window.preencherCamposRecorrenciaAlerta = preencherCamposRecorrenciaAlerta;
