/* ****************************************************************************************
 * ⚡ ARQUIVO: alertas_upsert.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Formulário de cadastro e edição de Alertas FrotiX com suporte completo
 *                   a recorrência (TipoExibicao 1-8), validação, dropdowns customizados
 *                   (motorista com foto, agendamento com cards), e integração com API.
 * 📥 ENTRADAS     : Clicks em tipo-alerta-cards, mudanças em dropdowns Syncfusion,
 *                   submit do #formAlerta, dados de edição (backend)
 * 📤 SAÍDAS       : POST /api/AlertasFrotiX/Salvar, validações UI, toasts, SweetAlert,
 *                   redirect para /AlertasFrotiX após sucesso
 * 🔗 CHAMADA POR  : AlertasFrotiX/Upsert.cshtml, DOMContentLoaded auto-init
 * 🔄 CHAMA        : $.ajax, Swal.fire, AppToast.show, Alerta.Confirmar,
 *                   coletarDadosRecorrenciaAlerta (alertas_recorrencia.js),
 *                   initCalendarioAlerta, TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery, Syncfusion EJ2 (DropDownList, TextBox, DatePicker, etc.),
 *                   SweetAlert2, AppToast, Alerta.js, alertas_recorrencia.js (para tipo 8)
 * 📝 OBSERVAÇÕES  : TipoAlerta 1-6 (Agendamento, Manutenção, Motorista, Veículo, Anúncio,
 *                   Diversos). TipoExibicao 1-8 (não recorrente 1-3, recorrente 4-8).
 *                   Todas as funções têm try-catch completo. Previne submit duplo com
 *                   flag window.salvandoAlerta.
 *
 * 📋 ÍNDICE DE FUNÇÕES (18 funções + 3 DOMContentLoaded handlers):
 *
 * ┌─ INICIALIZAÇÃO ──────────────────────────────────────────────────────────┐
 * │ 1. DOMContentLoaded (main)                                              │
 * │    → Chama: inicializarControles, configurarEventHandlers,             │
 * │      aplicarSelecaoInicial, configurarValidacao, configurarAvisoUsuarios│
 * │    → Console.log de inicialização                                       │
 * │                                                                          │
 * │ 2. inicializarControles()                                               │
 * │    → Código comentado: configuração de Syncfusion Tooltip (desativado) │
 * │    → Placeholder para futuras inicializações                            │
 * │                                                                          │
 * │ 3. aplicarSelecaoInicial()                                              │
 * │    → Lê #TipoAlerta.val(), aplica .selected no card correspondente     │
 * │    → Chama configurarCamposRelacionados(tipoAtual)                     │
 * │    → Lê TipoExibicao dropdown, chama configurarCamposExibicao          │
 * │    → Usado em modo de edição para restaurar estado visual              │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EVENT HANDLERS ─────────────────────────────────────────────────────────┐
 * │ 4. configurarEventHandlers()                                            │
 * │    → .tipo-alerta-card click: remove .selected de todos, adiciona ao   │
 * │      clicado, atualiza #TipoAlerta hidden, chama configurarCamposRelacionados│
 * │    → #TipoExibicao dropdown.change: chama configurarCamposExibicao     │
 * │    → #formAlerta submit: preventDefault, validação, desabilita botão,  │
 * │      chama salvarAlerta                                                 │
 * │    → Usa .off().on() para evitar múltiplos handlers                    │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CONFIGURAÇÃO DE CAMPOS DINÂMICOS ───────────────────────────────────────┐
 * │ 5. configurarCamposRelacionados(tipo)                                   │
 * │    → Esconde todos: divViagem, divManutencao, divMotorista, divVeiculo │
 * │    → Limpa valores de todos os dropdowns de vínculo                    │
 * │    → Switch case TipoAlerta (1-6):                                      │
 * │      • 1 (Agendamento): mostra divViagem                               │
 * │      • 2 (Manutenção): mostra divManutencao                            │
 * │      • 3 (Motorista): mostra divMotorista                              │
 * │      • 4 (Veículo): mostra divVeiculo                                  │
 * │      • 5/6 (Anúncio/Diversos): sem vínculos específicos                │
 * │                                                                          │
 * │ 6. configurarCamposExibicao(tipoExibicao)                               │
 * │    → Esconde TODOS os campos primeiro (divDataExibicao, divHorario,    │
 * │      divDataExpiracao, divDias, divDiaMes, calendarContainer)          │
 * │    → Ajusta labels (lblDataExibicao, lblHorarioExibicao) conforme tipo│
 * │    → Switch case TipoExibicao (1-8):                                    │
 * │      • 1: mostra apenas divDataExpiracao                               │
 * │      • 2: mostra divHorario + divDataExpiracao                         │
 * │      • 3: mostra divDataExibicao + divHorario + divDataExpiracao       │
 * │      • 4: Data Inicial, Horário, Data Final (label="Data Inicial")    │
 * │      • 5/6 (Semanal/Quinzenal): + divDiasAlerta                        │
 * │      • 7 (Mensal): + divDiaMesAlerta                                   │
 * │      • 8 (Dias Variados): Horário + Data Final + Calendar, init se needed│
 * │    → Console.log de configuração                                        │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÃO ───────────────────────────────────────────────────────────────┐
 * │ 7. configurarValidacao()                                                │
 * │    → Adiciona blur handlers customizados aos inputs Syncfusion         │
 * │    → #Titulo.blur → validarCampo('Titulo', 'Título é obrigatório')    │
 * │    → #Descricao.blur → validarCampo('Descricao', 'Descrição é obrigatória')│
 * │                                                                          │
 * │ 8. validarCampo(campoId, mensagemErro)                                  │
 * │    → Obtém campo via ej2_instances[0].value                            │
 * │    → Se vazio: mostra mensagem em [data-valmsg-for], retorna false    │
 * │    → Se válido: esconde mensagem, retorna true                         │
 * │                                                                          │
 * │ 9. validarFormulario()                                                  │
 * │    → Valida título e descrição (obrigatórios)                          │
 * │    → Valida TipoAlerta != 0                                            │
 * │    → Usuários: OPCIONAL (vazio = todos os usuários)                    │
 * │    → Valida campos de exibição por TipoExibicao (switch case):        │
 * │      • Tipo 2: requer HorarioExibicao                                  │
 * │      • Tipo 3: requer DataExibicao                                     │
 * │      • Tipo 4-7: requer DataExibicao (inicial) + DataExpiracao (final)│
 * │      • Tipo 5/6: + lstDiasAlerta (array não vazio)                    │
 * │      • Tipo 7: + lstDiasMesAlerta (int)                                │
 * │      • Tipo 8: + datasAlertaSelecionadas.length > 0                   │
 * │    → AppToast.show para cada erro, retorna boolean                     │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ COLETA DE DADOS E SALVAMENTO ──────────────────────────────────────────┐
 * │ 10. obterDadosFormulario()                                              │
 * │     → Monta objeto base: { AlertasFrotiXId, Titulo, Descricao,        │
 * │       TipoAlerta, Prioridade, TipoExibicao, UsuariosIds }              │
 * │     → CAMPOS OPCIONAIS DE VÍNCULOS (baseados em TipoAlerta):          │
 * │       • Tipo 1: ViagemId (limpa não-GUID)                              │
 * │       • Tipo 2: ManutencaoId                                           │
 * │       • Tipo 3: MotoristaId                                            │
 * │       • Tipo 4: VeiculoId                                              │
 * │     → CAMPOS DE EXIBIÇÃO E RECORRÊNCIA (baseados em TipoExibicao):    │
 * │       • Tipo 3-7: DataExibicao                                         │
 * │       • Tipo 2-8: HorarioExibicao                                      │
 * │       • Todos: DataExpiracao (opcional)                                │
 * │       • Tipo 5/6: DiasSemana (array)                                   │
 * │       • Tipo 7: DiaMesRecorrencia (int)                                │
 * │       • Tipo 8: DatasSelecionadas (string "YYYY-MM-DD,...")           │
 * │     → Retorna objeto completo ou null em erro                          │
 * │                                                                          │
 * │ 11. salvarAlerta()                                                      │
 * │     → Previne submit duplo: window.salvandoAlerta flag                │
 * │     → obterDadosFormulario()                                           │
 * │     → Swal.fire loading modal                                          │
 * │     → POST /api/AlertasFrotiX/Salvar (JSON)                            │
 * │     → Sucesso: AppToast.show, redirect /AlertasFrotiX após 1.5s       │
 * │     → Erro: Swal.fire erro, re-habilita botão submit                  │
 * │     → Mensagens específicas: 404, 500, responseJSON.message            │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ AVISO DE USUÁRIOS ──────────────────────────────────────────────────────┐
 * │ 12. configurarAvisoUsuarios()                                           │
 * │     → Obtém dropdown #UsuariosIds (multiselect)                        │
 * │     → Cria div #avisoTodosUsuarios (background azul claro, info)      │
 * │     → multiselect.change: se vazio → slideDown aviso, senão → slideUp │
 * │     → Verifica estado inicial: mostra aviso se sem seleção            │
 * │     → Mensagem: "Nenhum usuário selecionado. O alerta será exibido    │
 * │       para todos os usuários."                                         │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ DROPDOWNS CUSTOMIZADOS ─────────────────────────────────────────────────┐
 * │ 13. configurarDropdownMotoristaComFoto()                                │
 * │     → Dropdown #MotoristaId com templates customizados                │
 * │     → itemTemplate: card com <img> + nome (foto em Group.Name)        │
 * │     → valueTemplate: linha compacta com foto mini + nome              │
 * │     → onerror: fallback /images/placeholder-user.png                   │
 * │     → dataBind() força re-render                                        │
 * │     → DOMContentLoaded: setTimeout 300ms para init                     │
 * │                                                                          │
 * │ 14. configurarDropdownAgendamentoRico()                                 │
 * │     → Dropdown #ViagemId com cards ricos                               │
 * │     → itemTemplate: card com header (data+hora+finalidade), body       │
 * │       (origem→destino, requisitante), badges, ícones Font Awesome     │
 * │     → valueTemplate: linha simples data + origem → destino             │
 * │     → filtering: busca em DataInicial, Origem, Destino, Requisitante, │
 * │       Finalidade (multi-field search)                                  │
 * │     → DOMContentLoaded: setTimeout 300ms                               │
 * │                                                                          │
 * │ 15. configurarDropdownManutencaoRico()                                  │
 * │     → Dropdown #ManutencaoId com cards de OS                           │
 * │     → itemTemplate: card com NumOS, 4 datas (Solicitação,             │
 * │       Disponibilização, Entrega, Devolução), Veículo, Reserva         │
 * │     → valueTemplate: "OS {NumOS} — {Veículo}"                          │
 * │     → Usa helpers linhaData (com legenda) e linha (sem legenda)       │
 * │     → filtering: busca em NumOS, Veiculo, CarroReserva                │
 * │     → DOMContentLoaded: setTimeout 300ms                               │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 📌 TIPOS DE ALERTA (TipoAlerta):
 * 1 = Agendamento (vincula ViagemId)
 * 2 = Manutenção (vincula ManutencaoId)
 * 3 = Motorista (vincula MotoristaId)
 * 4 = Veículo (vincula VeiculoId)
 * 5 = Anúncio (sem vínculos)
 * 6 = Diversos (sem vínculos)
 *
 * 📌 TIPOS DE EXIBIÇÃO (TipoExibicao):
 * 1 = Ao abrir o sistema (não recorrente)
 * 2 = Em Horário Específico (não recorrente)
 * 3 = Em Data/Hora Específica (não recorrente)
 * 4 = Recorrente - Diário (seg-sex automático)
 * 5 = Recorrente - Semanal (requer dias da semana)
 * 6 = Recorrente - Quinzenal (requer dias da semana)
 * 7 = Recorrente - Mensal (requer dia do mês 1-31)
 * 8 = Recorrente - Dias Variados (requer calendário multi-select)
 *
 * 🔄 FLUXO DE CRIAÇÃO DE ALERTA:
 * 1. Usuário seleciona tipo de alerta (click em card)
 * 2. configurarCamposRelacionados: mostra campos de vínculo apropriados
 * 3. Usuário seleciona TipoExibicao
 * 4. configurarCamposExibicao: mostra campos de recorrência apropriados
 * 5. Usuário preenche formulário
 * 6. Submit → validarFormulario
 * 7. obterDadosFormulario: monta objeto JSON
 * 8. salvarAlerta: POST /api, loading modal, redirect
 *
 * 🔄 FLUXO DE EDIÇÃO:
 * 1. Backend preenche #TipoAlerta hidden, TipoExibicao dropdown, campos
 * 2. DOMContentLoaded → aplicarSelecaoInicial
 * 3. configurarCamposRelacionados + configurarCamposExibicao restauram UI
 * 4. Usuário edita, submit segue fluxo normal
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Previne submit duplo com flag global window.salvandoAlerta
 * - Usuários opcional: se vazio, alerta para todos (aviso visual azul)
 * - Dropdowns customizados: 300ms delay na init para Syncfusion carregar
 * - Foto de motorista: hack usando Group.Name para armazenar URL
 * - Agendamento cards: busca em 5 campos diferentes (multi-field filtering)
 * - Manutenção cards: 4 datas com legendas legíveis (Solicitação, etc.)
 * - Validação: mostra AppToast amarelo para cada erro
 * - Redirect após salvar: setTimeout 1500ms para usuário ver toast
 * - Labels dinâmicos: "Data de Exibição" vira "Data Inicial" em recorrentes
 * - Todas as funções têm try-catch com TratamentoErroComLinha
 * - 3 DOMContentLoaded handlers separados: main, motorista, agendamento, manutenção
 * - console.log abundante para debug (inicialização, eventos, configurações)
 *
 * 🔌 VERSÃO: 2.0 (Recorrência Completa)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

$(document).ready(function () 
{
    try
    {
        console.log('===== ALERTAS UPSERT CARREGADO =====');
        console.log('jQuery versão:', $.fn.jquery);
        console.log('Cards encontrados:', $('.tipo-alerta-card').length);

        inicializarControles();
        configurarEventHandlers();
        aplicarSelecaoInicial();
        configurarValidacao();
        configurarAvisoUsuarios();

        console.log('===== INICIALIZAÇÃO COMPLETA =====');
    }
    catch (error)
    {
        console.error('ERRO NA INICIALIZAÇÃO:', error);
        TratamentoErroComLinha("alertas_upsert.js", "document.ready", error);
    }
});

function inicializarControles() 
{
    try
    {
        //// Configurar tooltips Syncfusion
        //if (typeof ej !== 'undefined' && ej.popups && ej.popups.Tooltip) 
        //{
        //    var tooltip = new ej.popups.Tooltip({
        //        cssClass: 'ftx-tooltip-noarrow',
        //        position: 'TopCenter',
        //        isSticky: true,  // ✅ Mantém visível até clicar fora
        //        opensOn: 'Hover',
        //        closeDelay: 500,  // Delay de 500ms antes de fechar
        //        animation: {
        //            open: { effect: 'FadeIn', duration: 150 },
        //            close: { effect: 'FadeOut', duration: 150 }
        //        }
        //    });
        //    tooltip.appendTo('body');
        //}
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "inicializarControles", error);
    }
}

function configurarEventHandlers() 
{
    try
    {
        console.log('>>> Configurando event handlers...');

        // Seleção de tipo de alerta
        $(document).off('click', '.tipo-alerta-card').on('click', '.tipo-alerta-card', function (e) 
        {
            try
            {
                console.log('===== CLICK DETECTADO =====');
                e.preventDefault();
                e.stopPropagation();

                // Remove seleção de todos
                $('.tipo-alerta-card').removeClass('selected');

                // Adiciona seleção ao clicado
                $(this).addClass('selected');

                var tipo = $(this).data('tipo');
                $('#TipoAlerta').val(tipo);

                console.log('Tipo selecionado:', tipo);
                console.log('Possui classe selected:', $(this).hasClass('selected'));
                console.log('Classes do card:', $(this).attr('class'));

                // Mostrar/ocultar campos relacionados
                configurarCamposRelacionados(tipo);
            }
            catch (error)
            {
                console.error('ERRO no click handler:', error);
                TratamentoErroComLinha("alertas_upsert.js", "tipo-alerta-card.click", error);
            }
        });

        // Mudanca no tipo de exibicao (Kendo DropDownList)
        var ddlTipoExibicao = $("#TipoExibicao").data("kendoDropDownList");
        if (ddlTipoExibicao) 
        {
            ddlTipoExibicao.bind("change", function (e) 
            {
                try
                {
                    configurarCamposExibicao(this.value());
                }
                catch (error)
                {
                    TratamentoErroComLinha("alertas_upsert.js", "TipoExibicao.change", error);
                }
            });
        }

        // Submit do formulário
        $('#formAlerta').on('submit', function (e) 
        {
            try
            {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation(); // Previne múltiplas chamadas

                if (!validarFormulario()) 
                {
                    return false;
                }

                // Desabilitar botão de submit para evitar cliques duplos
                var btnSubmit = $(this).find('button[type="submit"]');
                if (btnSubmit.length)
                {
                    btnSubmit.prop('disabled', true);
                }

                salvarAlerta();

                return false;
            }
            catch (error)
            {
                TratamentoErroComLinha("alertas_upsert.js", "formAlerta.submit", error);
                return false;
            }
        });

        console.log('>>> Event handlers configurados!');
    }
    catch (error)
    {
        console.error('ERRO em configurarEventHandlers:', error);
        TratamentoErroComLinha("alertas_upsert.js", "configurarEventHandlers", error);
    }
}

function configurarCamposRelacionados(tipo) 
{
    try
    {
        // Ocultar todos os campos relacionados
        $('#divViagem, #divManutencao, #divMotorista, #divVeiculo').hide();
        $('#secaoVinculos').hide();

        // Limpar valores (Kendo DropDownList)
        var ddlViagem = $("#ViagemId").data("kendoDropDownList");
        if (ddlViagem) { ddlViagem.value(""); }
        var ddlManutencao = $("#ManutencaoId").data("kendoDropDownList");
        if (ddlManutencao) { ddlManutencao.value(""); }
        var ddlMotorista = $("#MotoristaId").data("kendoDropDownList");
        if (ddlMotorista) { ddlMotorista.value(""); }
        var ddlVeiculo = $("#VeiculoId").data("kendoDropDownList");
        if (ddlVeiculo) { ddlVeiculo.value(""); }

        // Mostrar campo específico baseado no tipo
        switch (parseInt(tipo)) 
        {
            case 1: // Agendamento
                $('#divViagem').show();
                $('#secaoVinculos').show();
                break;
            case 2: // Manutenção
                $('#divManutencao').show();
                $('#secaoVinculos').show();
                break;
            case 3: // Motorista
                $('#divMotorista').show();
                $('#secaoVinculos').show();
                break;
            case 4: // Veículo
                $('#divVeiculo').show();
                $('#secaoVinculos').show();
                break;
            case 5: // Anúncio
            case 6: // Diversos
                // Não tem vínculos específicos
                break;
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "configurarCamposRelacionados", error);
    }
}

/**
 * Configura a exibição dos campos baseado no TipoExibicao selecionado
 * 
 * TipoExibicao:
 * 1 = Ao abrir o sistema
 * 2 = Em Horário Específico
 * 3 = Em Data/Hora Específica
 * 4 = Recorrente - Diário
 * 5 = Recorrente - Semanal
 * 6 = Recorrente - Quinzenal
 * 7 = Recorrente - Mensal
 * 8 = Recorrente - Dias Variados
 */
function configurarCamposExibicao(tipoExibicao) 
{
    try
    {
        var tipo = parseInt(tipoExibicao);
        console.log('Configurando campos para TipoExibicao:', tipo);

        // ===================================================================
        // 1. ESCONDER TODOS OS CAMPOS PRIMEIRO
        // ===================================================================
        $('#divDataExibicao').hide();
        $('#divHorarioExibicao').hide();
        $('#divDataExpiracao').hide();
        $('#divDiasAlerta').hide();
        $('#divDiaMesAlerta').hide();
        $('#calendarContainerAlerta').hide();

        // ===================================================================
        // 2. AJUSTAR LABELS CONFORME O TIPO
        // ===================================================================
        var lblDataExibicao = document.getElementById('lblDataExibicao');
        var lblHorarioExibicao = document.getElementById('lblHorarioExibicao');

        // Reset labels para padrão
        if (lblDataExibicao) lblDataExibicao.textContent = 'Data de Exibição';
        if (lblHorarioExibicao) lblHorarioExibicao.textContent = 'Horário de Exibição';

        // ===================================================================
        // 3. MOSTRAR CAMPOS CONFORME O TIPO DE EXIBIÇÃO
        // ===================================================================
        switch (tipo) 
        {
            case 1: // Ao abrir o sistema
                // Apenas Data de Expiração (opcional)
                $('#divDataExpiracao').show();
                break;

            case 2: // Em Horário Específico
                // Horário + Data de Expiração
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                break;

            case 3: // Em Data/Hora Específica
                // Data + Horário + Data de Expiração
                $('#divDataExibicao').show();
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                break;

            case 4: // Recorrente - Diário (seg-sex automático)
                // Data Inicial + Horário (opcional) + Data Final
                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                $('#divDataExibicao').show();
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                break;

            case 5: // Recorrente - Semanal
                // Data Inicial + Horário + Data Final + Dias da Semana
                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                $('#divDataExibicao').show();
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                $('#divDiasAlerta').show();
                break;

            case 6: // Recorrente - Quinzenal
                // Data Inicial + Horário + Data Final + Dias da Semana
                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                $('#divDataExibicao').show();
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                $('#divDiasAlerta').show();
                break;

            case 7: // Recorrente - Mensal
                // Data Inicial + Horário + Data Final + Dia do Mês
                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                $('#divDataExibicao').show();
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                $('#divDiaMesAlerta').show();
                break;

            case 8: // Recorrente - Dias Variados
                // Horário + Data Final + Calendário
                $('#divHorarioExibicao').show();
                $('#divDataExpiracao').show();
                $('#calendarContainerAlerta').show();
                // Inicializar calendário se necessário
                if (typeof initCalendarioAlerta === 'function' && !window.calendarioAlertaInstance) {
                    initCalendarioAlerta();
                }
                break;

            default:
                // Tipo desconhecido - mostrar apenas Data de Expiração
                $('#divDataExpiracao').show();
                break;
        }

        console.log('Campos configurados para tipo:', tipo);
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "configurarCamposExibicao", error);
    }
}

function aplicarSelecaoInicial() 
{
    try
    {
        // Aplicar seleção inicial do tipo de alerta
        var tipoAtual = $('#TipoAlerta').val();
        if (tipoAtual) 
        {
            $(`.tipo-alerta-card[data-tipo="${tipoAtual}"]`).addClass('selected');
            configurarCamposRelacionados(tipoAtual);
        }

        // Aplicar configuracao inicial do tipo de exibicao (Kendo)
        var ddlTipoExibicao = $("#TipoExibicao").data("kendoDropDownList");
        if (ddlTipoExibicao) 
        {
            var tipoExibicaoAtual = ddlTipoExibicao.value();
            if (tipoExibicaoAtual) 
            {
                configurarCamposExibicao(tipoExibicaoAtual);
            }
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "aplicarSelecaoInicial", error);
    }
}

function configurarValidacao() 
{
    try
    {
        // Adicionar validação customizada aos campos Syncfusion
        var tituloInput = document.querySelector("#Titulo");
        if (tituloInput && tituloInput.ej2_instances) 
        {
            tituloInput.ej2_instances[0].blur = function () 
            {
                validarCampo('Titulo', 'Título é obrigatório');
            };
        }

        var descricaoInput = document.querySelector("#Descricao");
        if (descricaoInput && descricaoInput.ej2_instances) 
        {
            descricaoInput.ej2_instances[0].blur = function () 
            {
                validarCampo('Descricao', 'Descrição é obrigatória');
            };
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "configurarValidacao", error);
    }
}

function configurarAvisoUsuarios()
{
    try
    {
        var usuariosSelect = document.querySelector("#UsuariosIds");
        if (usuariosSelect && usuariosSelect.ej2_instances)
        {
            var multiselect = usuariosSelect.ej2_instances[0];

            // Criar div de aviso se não existir
            if (!$('#avisoTodosUsuarios').length)
            {
                var avisoHtml = '<div id="avisoTodosUsuarios" style="display:none; margin-top: 8px; padding: 8px 12px; background-color: #e0f2fe; border-left: 3px solid #0ea5e9; border-radius: 4px; font-size: 0.85rem; color: #0c4a6e;"><i class="fa-duotone fa-info-circle" style="margin-right: 6px;"></i>Nenhum usuário selecionado. O alerta será exibido para <strong>todos os usuários</strong>.</div>';
                $(usuariosSelect).closest('.col-md-12').append(avisoHtml);
            }

            // Evento de mudança no multiselect
            multiselect.change = function (args)
            {
                var usuarios = multiselect.value;
                if (!usuarios || usuarios.length === 0)
                {
                    $('#avisoTodosUsuarios').slideDown(200);
                    $('[data-valmsg-for="UsuariosIds"]').text('').hide();
                }
                else
                {
                    $('#avisoTodosUsuarios').slideUp(200);
                }
            };

            // Verificar estado inicial
            var valoresIniciais = multiselect.value;
            if (!valoresIniciais || valoresIniciais.length === 0)
            {
                $('#avisoTodosUsuarios').show();
            }
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "configurarAvisoUsuarios", error);
    }
}

function validarCampo(campoId, mensagemErro) 
{
    try
    {
        var campo = document.querySelector(`#${campoId}`);
        var spanErro = $(`[data-valmsg-for="${campoId}"]`);

        if (campo && campo.ej2_instances) 
        {
            var valor = campo.ej2_instances[0].value;

            if (!valor || valor.trim() === '') 
            {
                spanErro.text(mensagemErro).show();
                return false;
            }
            else 
            {
                spanErro.text('').hide();
                return true;
            }
        }

        return true;
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "validarCampo", error);
        return false;
    }
}

function validarFormulario() 
{
    try
    {
        var valido = true;

        // Validar título
        if (!validarCampo('Titulo', 'O título é obrigatório')) 
        {
            valido = false;
        }

        // Validar descrição
        if (!validarCampo('Descricao', 'A descrição é obrigatória')) 
        {
            valido = false;
        }

        // Validar tipo de alerta
        var tipoAlerta = $('#TipoAlerta').val();
        if (!tipoAlerta || tipoAlerta == '0') 
        {
            AppToast.show("Amarelo", "Selecione um tipo de alerta", 2000);
            valido = false;
        }

        // Usuários agora são opcionais (se vazio = todos os usuários)
        var usuariosSelect = document.querySelector("#UsuariosIds");
        if (usuariosSelect && usuariosSelect.ej2_instances) 
        {
            $('[data-valmsg-for="UsuariosIds"]').text('').hide();
        }

        // Validar campos de exibição conforme o tipo
        // Obter TipoExibicao via Kendo
        var ddlTipoExibicaoVal = $("#TipoExibicao").data("kendoDropDownList");
        var tipoExibicao = parseInt(ddlTipoExibicaoVal ? ddlTipoExibicaoVal.value() : 1);

        switch (tipoExibicao)
        {
            case 2: // Horario especifico
                var horario = document.querySelector("#HorarioExibicao")?.ej2_instances?.[0]?.value;
                if (!horario) 
                {
                    AppToast.show("Amarelo", "Selecione o horário de exibição", 2000);
                    valido = false;
                }
                break;

            case 3: // Data/Hora especifica
                var dataExib = document.querySelector("#DataExibicao")?.ej2_instances?.[0]?.value;
                if (!dataExib) 
                {
                    AppToast.show("Amarelo", "Selecione a data de exibição", 2000);
                    valido = false;
                }
                break;

            case 4: // Recorrente Diario
            case 5: // Recorrente Semanal
            case 6: // Recorrente Quinzenal
            case 7: // Recorrente Mensal
                var dataInicial = document.querySelector("#DataExibicao")?.ej2_instances?.[0]?.value;
                var dataFinal = document.querySelector("#DataExpiracao")?.ej2_instances?.[0]?.value;
                if (!dataInicial) 
                {
                    AppToast.show("Amarelo", "Selecione a data inicial da recorrência", 2000);
                    valido = false;
                }
                if (!dataFinal) 
                {
                    AppToast.show("Amarelo", "Selecione a data final da recorrência", 2000);
                    valido = false;
                }
                // Validar dias da semana para Semanal/Quinzenal
                if (tipoExibicao === 5 || tipoExibicao === 6)
                {
                    var diasSemana = document.querySelector("#lstDiasAlerta")?.ej2_instances?.[0]?.value;
                    if (!diasSemana || diasSemana.length === 0)
                    {
                        AppToast.show("Amarelo", "Selecione pelo menos um dia da semana", 2000);
                        valido = false;
                    }
                }
                // Validar dia do mes para Mensal (Kendo)
                if (tipoExibicao === 7)
                {
                    var ddlDiaMes = $("#lstDiasMesAlerta").data("kendoDropDownList");
                    var diaMes = ddlDiaMes ? ddlDiaMes.value() : null;
                    if (!diaMes)
                    {
                        AppToast.show("Amarelo", "Selecione o dia do mês", 2000);
                        valido = false;
                    }
                }
                break;

            case 8: // Recorrente Dias Variados
                var datasSelecionadas = window.datasAlertaSelecionadas || [];
                if (datasSelecionadas.length === 0)
                {
                    AppToast.show("Amarelo", "Selecione pelo menos uma data no calendário", 2000);
                    valido = false;
                }
                break;
        }

        return valido;
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "validarFormulario", error);
        return false;
    }
}

function salvarAlerta() 
{
    // Prevenir múltiplas chamadas
    if (window.salvandoAlerta)
    {
        console.log('Já existe um salvamento em andamento, ignorando...');
        return;
    }

    try
    {
        window.salvandoAlerta = true;

        var dados = obterDadosFormulario();

        if (!dados)
        {
            console.error('Dados do formulário inválidos');
            window.salvandoAlerta = false;
            return;
        }

        Swal.fire({
            title: 'Salvando...',
            text: 'Aguarde enquanto o alerta é salvo',
            allowOutsideClick: false,
            didOpen: () =>
            {
                Swal.showLoading();
            }
        });

        $.ajax({
            url: '/api/AlertasFrotiX/Salvar',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(dados),
            success: function (response) 
            {
                try
                {
                    window.salvandoAlerta = false;
                    Swal.close();

                    if (response.success) 
                    {
                        AppToast.show("Verde", response.message || "Alerta salvo com sucesso!", 2000);

                        // Redirecionar após 1.5 segundos
                        setTimeout(function () 
                        {
                            window.location.href = '/AlertasFrotiX';
                        }, 1500);
                    }
                    else 
                    {
                        Swal.fire('Erro', response.message || 'Erro ao salvar alerta', 'error');
                    }
                }
                catch (error)
                {
                    window.salvandoAlerta = false;
                    TratamentoErroComLinha("alertas_upsert.js", "salvarAlerta.success", error);
                }
            },
            error: function (xhr, status, error) 
            {
                window.salvandoAlerta = false;
                Swal.close();
                TratamentoErroComLinha("alertas_upsert.js", "salvarAlerta.error", error);

                var mensagem = 'Erro ao salvar alerta';
                if (xhr.responseJSON && xhr.responseJSON.message) 
                {
                    mensagem = xhr.responseJSON.message;
                }
                else if (xhr.status === 404) 
                {
                    mensagem = 'Rota não encontrada (404). Verifique se a URL /AlertasFrotiX/Salvar está correta.';
                }
                else if (xhr.status === 500) 
                {
                    mensagem = 'Erro no servidor. Verifique os logs do backend.';
                }

                Swal.fire('Erro', mensagem, 'error');

                // Re-habilitar botão de submit
                $('#formAlerta button[type="submit"]').prop('disabled', false);
            }
        });
    }
    catch (error)
    {
        window.salvandoAlerta = false;
        Swal.close();
        TratamentoErroComLinha("alertas_upsert.js", "salvarAlerta", error);

        // Re-habilitar botão de submit
        $('#formAlerta button[type="submit"]').prop('disabled', false);
    }
}

function obterDadosFormulario() 
{
    try
    {
        // Obter TipoExibicao e Prioridade via Kendo
        var ddlTipoExib = $("#TipoExibicao").data("kendoDropDownList");
        var tipoExibicao = parseInt(ddlTipoExib ? ddlTipoExib.value() : 1);
        var ddlPrioridade = $("#Prioridade").data("kendoDropDownList");

        var dados = {
            AlertasFrotiXId: $('#AlertasFrotiXId').val(),
            Titulo: document.querySelector("#Titulo")?.ej2_instances?.[0]?.value || '',
            Descricao: document.querySelector("#Descricao")?.ej2_instances?.[0]?.value || '',
            TipoAlerta: parseInt($('#TipoAlerta').val()),
            Prioridade: parseInt(ddlPrioridade ? ddlPrioridade.value() : 1),
            TipoExibicao: tipoExibicao,
            UsuariosIds: document.querySelector("#UsuariosIds")?.ej2_instances?.[0]?.value || []
        };

        // ===================================================================
        // CAMPOS OPCIONAIS DE VÍNCULOS (baseados no TipoAlerta)
        // ===================================================================
        var tipoAlerta = dados.TipoAlerta;

        if (tipoAlerta === 1) // Agendamento (Kendo)
        {
            var ddlViagem = $("#ViagemId").data("kendoDropDownList");
            var viagemId = ddlViagem ? ddlViagem.value() : null;
            if (viagemId)
            {
                viagemId = String(viagemId).trim().replace(/[^a-f0-9\-]/gi, '');
                if (viagemId.length > 0) dados.ViagemId = viagemId;
            }
        }
        else if (tipoAlerta === 2) // Manutencao (Kendo)
        {
            var ddlManutencao = $("#ManutencaoId").data("kendoDropDownList");
            var manutencaoId = ddlManutencao ? ddlManutencao.value() : null;
            if (manutencaoId)
            {
                manutencaoId = String(manutencaoId).trim().replace(/[^a-f0-9\-]/gi, '');
                if (manutencaoId.length > 0) dados.ManutencaoId = manutencaoId;
            }
        }
        else if (tipoAlerta === 3) // Motorista (Kendo)
        {
            var ddlMotorista = $("#MotoristaId").data("kendoDropDownList");
            var motoristaId = ddlMotorista ? ddlMotorista.value() : null;
            if (motoristaId)
            {
                motoristaId = String(motoristaId).trim().replace(/[^a-f0-9\-]/gi, '');
                if (motoristaId.length > 0) dados.MotoristaId = motoristaId;
            }
        }
        else if (tipoAlerta === 4) // Veiculo (Kendo)
        {
            var ddlVeiculo = $("#VeiculoId").data("kendoDropDownList");
            var veiculoId = ddlVeiculo ? ddlVeiculo.value() : null;
            if (veiculoId)
            {
                veiculoId = String(veiculoId).trim().replace(/[^a-f0-9\-]/gi, '');
                if (veiculoId.length > 0) dados.VeiculoId = veiculoId;
            }
        }

        // ===================================================================
        // CAMPOS DE EXIBIÇÃO E RECORRÊNCIA (baseados no TipoExibicao)
        // ===================================================================

        // Data de Exibição (tipos 3, 4, 5, 6, 7)
        if (tipoExibicao >= 3 && tipoExibicao <= 7)
        {
            var dataExibicao = document.querySelector("#DataExibicao")?.ej2_instances?.[0]?.value;
            if (dataExibicao) dados.DataExibicao = dataExibicao;
        }

        // Horário de Exibição (tipos 2, 3, 4, 5, 6, 7, 8)
        if (tipoExibicao >= 2)
        {
            var horario = document.querySelector("#HorarioExibicao")?.ej2_instances?.[0]?.value;
            if (horario) dados.HorarioExibicao = horario;
        }

        // Data de Expiração (todos os tipos)
        var dataExpiracao = document.querySelector("#DataExpiracao")?.ej2_instances?.[0]?.value;
        if (dataExpiracao) dados.DataExpiracao = dataExpiracao;

        // ===================================================================
        // CAMPOS ESPECÍFICOS DE RECORRÊNCIA
        // ===================================================================

        // Dias da Semana (tipos 5 e 6)
        if (tipoExibicao === 5 || tipoExibicao === 6)
        {
            var diasSemana = document.querySelector("#lstDiasAlerta")?.ej2_instances?.[0]?.value;
            if (diasSemana && diasSemana.length > 0)
            {
                dados.DiasSemana = diasSemana;
            }
        }

        // Dia do Mes (tipo 7) - Kendo
        if (tipoExibicao === 7)
        {
            var ddlDiaMes = $("#lstDiasMesAlerta").data("kendoDropDownList");
            var diaMes = ddlDiaMes ? ddlDiaMes.value() : null;
            if (diaMes)
            {
                dados.DiaMesRecorrencia = parseInt(diaMes);
            }
        }

        // Datas Selecionadas (tipo 8)
        if (tipoExibicao === 8)
        {
            var datasSelecionadas = window.datasAlertaSelecionadas || [];
            if (datasSelecionadas.length > 0)
            {
                // Converter para string de datas ISO
                var datasFormatadas = datasSelecionadas.map(function(d) {
                    var data = new Date(d);
                    var mes = ('0' + (data.getMonth() + 1)).slice(-2);
                    var dia = ('0' + data.getDate()).slice(-2);
                    return data.getFullYear() + '-' + mes + '-' + dia;
                });
                dados.DatasSelecionadas = datasFormatadas.join(',');
            }
        }

        console.log('Dados do formulário preparados:', dados);
        return dados;
    }
    catch (error)
    {
        TratamentoErroComLinha("alertas_upsert.js", "obterDadosFormulario", error);
        return null;
    }
}

// ============================================================================
// DROPDOWN DE MOTORISTAS COM FOTO
// ============================================================================

function configurarDropdownMotoristaComFoto()
{
    try
    {
        var ddl = $("#MotoristaId").data("kendoDropDownList");
        if (!ddl)
        {
            console.log('Dropdown de motoristas nao encontrado (Kendo)');
            return;
        }

        // Template para itens da lista (Kendo template syntax)
        ddl.setOptions({
            template: function (dataItem) {
                if (!dataItem || !dataItem.text) return '';
                var foto = (dataItem.group && dataItem.group.name) || '/images/placeholder-user.png';
                var texto = dataItem.text || '';
                return '<div class="motorista-item-alerta">' +
                    '<img src="' + foto + '" class="motorista-foto-alerta-item" alt="Foto" onerror="this.src=\'/images/placeholder-user.png\'" />' +
                    '<span class="motorista-nome-alerta">' + texto + '</span>' +
                    '</div>';
            },
            valueTemplate: function (dataItem) {
                if (!dataItem || !dataItem.text) return '';
                var foto = (dataItem.group && dataItem.group.name) || '/images/placeholder-user.png';
                var texto = dataItem.text || '';
                return '<div class="motorista-selected-alerta">' +
                    '<img src="' + foto + '" class="motorista-foto-alerta-selected" alt="Foto" onerror="this.src=\'/images/placeholder-user.png\'" />' +
                    '<span class="motorista-nome-alerta">' + texto + '</span>' +
                    '</div>';
            }
        });

        console.log('Dropdown de motoristas configurada com foto (Kendo)');
    } catch (error)
    {
        console.error('Erro ao configurar dropdown motorista:', error);
        if (typeof Alerta !== 'undefined')
        {
            Alerta.TratamentoErroComLinha("alertas_upsert.js", "configurarDropdownMotoristaComFoto", error);
        }
    }
}

// Inicializar após DOM carregar
document.addEventListener('DOMContentLoaded', function ()
{
    setTimeout(configurarDropdownMotoristaComFoto, 300);
});

// Também inicializar quando o tipo de alerta "Motorista" for selecionado
// (já que o campo fica oculto inicialmente)

// ============================================================================
// DROPDOWN DE AGENDAMENTOS COM CARD RICO
// ============================================================================

function configurarDropdownAgendamentoRico()
{
    try
    {
        var ddl = $("#ViagemId").data("kendoDropDownList");
        if (!ddl)
        {
            console.log('Dropdown de viagens nao encontrado (Kendo)');
            return;
        }

        var dropdown = ddl;

        // Templates ricos para Kendo DropDownList (ViagemId)
        // Nota: Kendo DDL templates usam camelCase dos campos JSON
        ddl.setOptions({
            template: function (dataItem) {
                if (!dataItem || !dataItem.dataInicial) return '';
                return '<div class="agendamento-card-item">' +
                    '<div class="agendamento-card-header">' +
                    '<div class="agendamento-card-title">' +
                    '<i class="fa-duotone fa-calendar-check"></i> ' +
                    '<strong>' + (dataItem.dataInicial || 'N/A') + '</strong> ' +
                    '<span class="agendamento-hora"><i class="fa-duotone fa-clock"></i> <strong>' + (dataItem.horaInicio || '') + '</strong></span>' +
                    '</div>' +
                    '<span class="agendamento-badge">' + (dataItem.finalidade || 'Diversos') + '</span>' +
                    '</div>' +
                    '<div class="agendamento-card-body">' +
                    '<div class="agendamento-rota">' +
                    '<span class="agendamento-origem"><i class="fa-duotone fa-location-dot"></i> ' + (dataItem.origem || 'N/A') + '</span>' +
                    ' <i class="fa-duotone fa-arrow-right agendamento-seta"></i> ' +
                    '<span class="agendamento-destino"><i class="fa-duotone fa-flag-checkered"></i> ' + (dataItem.destino || 'N/A') + '</span>' +
                    '</div>' +
                    '<div class="agendamento-requisitante"><i class="fa-duotone fa-user"></i> <span>' + (dataItem.requisitante || 'Nao informado') + '</span></div>' +
                    '</div></div>';
            },
            valueTemplate: function (dataItem) {
                if (!dataItem || !dataItem.dataInicial) return '';
                return '<div class="agendamento-selected">' +
                    '<i class="fa-duotone fa-calendar-check"></i> ' +
                    '<span class="agendamento-selected-text"><strong>' + (dataItem.dataInicial || 'N/A') + '</strong> - ' +
                    (dataItem.origem || 'N/A') + ' -> ' + (dataItem.destino || 'N/A') + '</span></div>';
            }
        });

        console.log('Dropdown de agendamentos configurada com cards ricos (Kendo)');
    } catch (error)
    {
        console.error('Erro ao configurar dropdown agendamento:', error);
        if (typeof Alerta !== 'undefined')
        {
            Alerta.TratamentoErroComLinha("alertas_upsert.js", "configurarDropdownAgendamentoRico", error);
        }
    }
}

// Inicializar após DOM carregar
document.addEventListener('DOMContentLoaded', function ()
{
    setTimeout(configurarDropdownAgendamentoRico, 300);
});

function configurarDropdownManutencaoRico()
{
    try
    {
        var ddl = $("#ManutencaoId").data("kendoDropDownList");
        if (!ddl)
        {
            console.log('Dropdown de manutencoes nao encontrado (Kendo)');
            return;
        }

        // Se havia dataset completo via window.__manutencoesDS, reconfigurar datasource
        if (window.__manutencoesDS)
        {
            ddl.setDataSource(new kendo.data.DataSource({ data: window.__manutencoesDS }));
            ddl.setOptions({
                dataTextField: "numOS",
                dataValueField: "manutencaoId"
            });
        }

        // Helpers para templates
        function linhaSimples(icon, val) {
            return '<span class="manutencao-dado"><i class="fa-duotone ' + icon + '"></i>' + (val || '\u2014') + '</span>';
        }
        function linhaData(icon, rotulo, val) {
            return '<span class="manutencao-dado">' +
                '<i class="fa-duotone ' + icon + '" aria-hidden="true"></i>' +
                '<span class="manutencao-legenda">' + rotulo + ':</span>' +
                '<span class="manutencao-valor">' + (val || '\u2014') + '</span></span>';
        }

        // Templates ricos para Kendo DropDownList (ManutencaoId)
        // Nota: Kendo DDL data items usam camelCase dos campos JSON
        ddl.setOptions({
            template: function (dataItem) {
                if (!dataItem || !dataItem.numOS) return '';
                var reservaTxt = (dataItem.reservaEnviado === 'Sim')
                    ? (dataItem.carroReserva || 'Reserva enviada')
                    : 'Reserva nao enviada';
                return '<div class="manutencao-card-item">' +
                    '<div class="manutencao-card-header">' +
                    '<div class="manutencao-card-title">' +
                    '<i class="fa-duotone fa-screwdriver-wrench"></i> ' +
                    '<strong>OS ' + (dataItem.numOS || '\u2014') + '</strong>' +
                    '</div></div>' +
                    '<div class="manutencao-card-body">' +
                    '<div class="manutencao-linha">' +
                    linhaData('fa-calendar-plus', 'Solicitacao', dataItem.dataSolicitacao) +
                    linhaData('fa-calendar-lines-pen', 'Disponibilizacao', dataItem.dataDisponibilidade) +
                    '</div>' +
                    '<div class="manutencao-linha">' +
                    linhaData('fa-calendar-arrow-up', 'Entrega', dataItem.dataEntrega) +
                    linhaData('fa-calendar-arrow-down', 'Devolucao', dataItem.dataDevolucao) +
                    '</div>' +
                    '<div class="manutencao-linha">' +
                    linhaSimples('fa-car-side', dataItem.veiculo) +
                    linhaSimples('fa-key', reservaTxt) +
                    '</div></div></div>';
            },
            valueTemplate: function (dataItem) {
                if (!dataItem || !dataItem.numOS) return '';
                return '<div class="manutencao-selected">' +
                    '<i class="fa-duotone fa-screwdriver-wrench"></i> ' +
                    '<span class="manutencao-selected-text"><strong>OS ' + (dataItem.numOS || '') + '</strong> \u2014 ' + (dataItem.veiculo || '') + '</span></div>';
            }
        });

        console.log('ManutencaoId com cards ricos (Kendo)');
    } catch (err)
    {
        console.error('Erro ao configurar dropdown manutencao:', err);
        if (typeof Alerta !== 'undefined')
        {
            Alerta.TratamentoErroComLinha("alertas_upsert.js", "configurarDropdownManutencaoRico", err);
        }
    }
}

// chame junto com as outras inicializacoes
document.addEventListener('DOMContentLoaded', function ()
{
    setTimeout(configurarDropdownManutencaoRico, 300);
});
