/*
 * ╔══════════════════════════════════════════════════════════════════════════════════╗
 * ║                         SOLUÇÃO FROTIX - GESTÃO DE FROTAS                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: insereviagem_001.js                                                  ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                                  ║
 * ║ 📋 VERSÃO: 1.1                                                                   ║
 * ║ 📅 ATUALIZAÇÃO: 22/01/2026                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                               ║
 * ║    Este script gerencia a INSERÇÃO DE VIAGENS com validações inteligentes:       ║
 * ║    • Validação de Ficha de Vistoria (verifica se já existe, tolerância ±100)     ║
 * ║    • Validação de datas (data final > data inicial)                              ║
 * ║    • Validação de horários (hora final > hora inicial)                           ║
 * ║    • Validação de quilometragem (km final > km inicial)                          ║
 * ║    • Auto-preenchimento de Km inicial ao selecionar veículo                      ║
 * ║    • Validação de abastecimento (alerta se litros > 200)                         ║
 * ║    • Validações obrigatórias antes de submeter formulário                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 🔗 DEPENDÊNCIAS:                                                                 ║
 * ║    • jQuery 3.x (manipulação DOM e AJAX)                                         ║
 * ║    • FrotiX Alerta (SweetAlert customizado para alertas e confirmações)          ║
 * ║    • AppToast (notificações toast do FrotiX)                                     ║
 * ║    • SyncfusionTooltips (tooltips visuais opcionais)                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 📡 ENDPOINTS CONSUMIDOS:                                                         ║
 * ║    GET  /Viagens/Upsert?handler=MaxFicha - Última ficha de vistoria cadastrada   ║
 * ║    GET  /Viagens/Upsert?handler=FichaExistente - Verifica se ficha já existe     ║
 * ║    GET  /api/Viagem/UltimaQuilometragem - Última km do veículo selecionado       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 🎯 PADRÃO FROTIX:                                                                ║
 * ║    • Try-Catch com Alerta.TratamentoErroComLinha em TODAS as funções             ║
 * ║    • Alertas via Alerta.Warning (nunca alert() nativo)                           ║
 * ║    • Notificações via AppToast (Verde/Amarelo/Vermelho)                          ║
 * ╚══════════════════════════════════════════════════════════════════════════════════╝
 */

/* ═══════════════════════════════════════════════════════════════════════════════════
   INICIALIZAÇÃO DO DOCUMENTO
   ═══════════════════════════════════════════════════════════════════════════════════ */

$(document).ready(function () {
    try {
        // ─────────────────────────────────────────────────────────────────────────────
        // Inicializa tooltips Syncfusion se disponível no escopo global
        // ─────────────────────────────────────────────────────────────────────────────
        if (
            window.SyncfusionTooltips &&
            typeof window.SyncfusionTooltips.init === 'function'
        ) {
            window.SyncfusionTooltips.init();
        }

        /* ═══════════════════════════════════════════════════════════════════════════
           VALIDAÇÃO DE FICHA DE VISTORIA
           Campo: #txtNoFichaVistoria
           Evento: focusout
           Regras:
             1. Verifica se o número difere ±100 da última ficha cadastrada
             2. Verifica se o número já foi utilizado em outra viagem
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#txtNoFichaVistoria').focusout(function () {
            try {
                const noFichaVistoria = parseInt($(this).val());

                if (!noFichaVistoria || noFichaVistoria === 0) {
                    return;
                }

                // Busca o número máximo da ficha
                $.ajax({
                    url: '/Viagens/Upsert?handler=MaxFicha',
                    method: 'GET',
                    datatype: 'json',
                    success: function (res) {
                        try {
                            const maxFichaVistoria = parseInt(res.data);

                            // Valida se a ficha é maior que o máximo + 100
                            if (noFichaVistoria > maxFichaVistoria + 100) {
                                Alerta.Warning(
                                    'Alerta na Ficha de Vistoria',
                                    'O número inserido difere em +100 da última Ficha inserida!',
                                );
                                return;
                            }

                            // Valida se a ficha é menor que o máximo - 100
                            if (noFichaVistoria < maxFichaVistoria - 100) {
                                Alerta.Warning(
                                    'Alerta na Ficha de Vistoria',
                                    'O número inserido difere em -100 da última Ficha inserida!',
                                );
                                return;
                            }
                        } catch (innerError) {
                            Alerta.TratamentoErroComLinha(
                                'insereviagem.js',
                                'txtNoFichaVistoria.MaxFicha.success',
                                innerError,
                            );
                        }
                    },
                    error: function (xhr, status, error) {
                        Alerta.TratamentoErroComLinha(
                            'insereviagem.js',
                            'txtNoFichaVistoria.MaxFicha.error',
                            new Error(error),
                        );
                    },
                });

                // Verifica se o número da ficha já foi cadastrado
                $.ajax({
                    url: '/Viagens/Upsert?handler=FichaExistente',
                    method: 'GET',
                    datatype: 'json',
                    data: { id: noFichaVistoria },
                    success: function (res) {
                        try {
                            const existeFicha = res.data;

                            if (existeFicha === true) {
                                Alerta.Warning(
                                    'Alerta na Ficha de Vistoria',
                                    'Já existe uma Ficha inserida com esta numeração!',
                                );
                                $('#txtNoFichaVistoria').val('');
                                $('#txtNoFichaVistoria').focus();
                            }
                        } catch (innerError) {
                            Alerta.TratamentoErroComLinha(
                                'insereviagem.js',
                                'txtNoFichaVistoria.FichaExistente.success',
                                innerError,
                            );
                        }
                    },
                    error: function (xhr, status, error) {
                        Alerta.TratamentoErroComLinha(
                            'insereviagem.js',
                            'txtNoFichaVistoria.FichaExistente.error',
                            new Error(error),
                        );
                    },
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'txtNoFichaVistoria.focusout',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           VALIDAÇÃO DE DATA FINAL
           Campo: #txtDataFinal
           Evento: focusout
           Regra: Data final deve ser maior ou igual à data inicial
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#txtDataFinal').focusout(function () {
            try {
                const dataInicial = $('#txtDataInicial').val();
                const dataFinal = $('#txtDataFinal').val();

                if (dataFinal === '') {
                    return;
                }

                if (dataFinal < dataInicial) {
                    $('#txtDataFinal').val('');
                    Alerta.Warning(
                        'Erro na Data',
                        'A data final deve ser maior que a inicial!',
                    );
                    $('#txtDataFinal').focus();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'txtDataFinal.focusout',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           VALIDAÇÃO DE HORÁRIO FINAL
           Campo: #txtHoraFinal
           Evento: focusout
           Regra: Hora final deve ser estritamente maior que hora inicial
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#txtHoraFinal').focusout(function () {
            try {
                const horaInicial = $('#txtHoraInicial').val();
                const horaFinal = $('#txtHoraFinal').val();

                if (horaFinal === '') {
                    return;
                }

                if (horaFinal <= horaInicial) {
                    $('#txtHoraFinal').val('');
                    Alerta.Warning(
                        'Erro no Horário',
                        'O horário final deve ser maior que o inicial!',
                    );
                    $('#txtHoraFinal').focus();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'txtHoraFinal.focusout',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           VALIDAÇÃO DE QUILOMETRAGEM FINAL
           Campo: #txtKmFinal
           Evento: focusout
           Regra: Km final deve ser estritamente maior que Km inicial
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#txtKmFinal').focusout(function () {
            try {
                const kmInicial = parseFloat($('#txtKmInicial').val());
                const kmFinal = parseFloat($('#txtKmFinal').val());

                if (!kmFinal || kmFinal === 0) {
                    return;
                }

                if (kmFinal <= kmInicial) {
                    $('#txtKmFinal').val('');
                    Alerta.Warning(
                        'Erro na Quilometragem',
                        'A quilometragem final deve ser maior que a inicial!',
                    );
                    $('#txtKmFinal').focus();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'txtKmFinal.focusout',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           AUTO-PREENCHIMENTO DE QUILOMETRAGEM INICIAL
           Campo: #lstVeiculo (dropdown de veículos)
           Evento: change
           Ação: Busca a última quilometragem registrada para o veículo selecionado
                 e preenche automaticamente o campo #txtKmInicial
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#lstVeiculo').change(function () {
            try {
                const veiculoId = $(this).val();

                if (!veiculoId) {
                    $('#txtKmInicial').val('');
                    return;
                }

                // Busca a última quilometragem do veículo
                $.ajax({
                    url: '/api/Viagem/UltimaQuilometragem',
                    method: 'GET',
                    data: { veiculoId: veiculoId },
                    success: function (res) {
                        try {
                            if (res.success && res.data) {
                                $('#txtKmInicial').val(res.data.quilometragem);
                                AppToast.show(
                                    'Verde',
                                    `Última quilometragem: ${res.data.quilometragem} km`,
                                    3000,
                                );
                            }
                        } catch (innerError) {
                            Alerta.TratamentoErroComLinha(
                                'insereviagem.js',
                                'lstVeiculo.UltimaQuilometragem.success',
                                innerError,
                            );
                        }
                    },
                    error: function (xhr, status, error) {
                        Alerta.TratamentoErroComLinha(
                            'insereviagem.js',
                            'lstVeiculo.UltimaQuilometragem.error',
                            new Error(error),
                        );
                    },
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'lstVeiculo.change',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           VALIDAÇÃO DE LITROS ABASTECIDOS
           Campo: #txtLitrosAbastecidos
           Evento: focusout
           Regra: Alerta se quantidade > 200 litros (pode indicar erro de digitação)
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#txtLitrosAbastecidos').focusout(function () {
            try {
                const litros = parseFloat($(this).val());

                if (!litros || litros === 0) {
                    return;
                }

                if (litros > 200) {
                    Alerta.Warning(
                        'Atenção no Abastecimento',
                        'Quantidade de litros abastecidos parece estar muito alta. Verifique!',
                    );
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'txtLitrosAbastecidos.focusout',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           SUBMIT DO FORMULÁRIO DE VIAGEM
           Botão: #btnSalvar
           Evento: click
           Validações obrigatórias antes de submeter:
             1. Data inicial preenchida
             2. Data final preenchida
             3. Veículo selecionado
             4. Motorista selecionado
             5. Km inicial > 0
             6. Km final > 0
           ═══════════════════════════════════════════════════════════════════════════ */

        $('#btnSalvar').click(function (e) {
            try {
                e.preventDefault();

                // Validações antes de submeter
                if (!$('#txtDataInicial').val()) {
                    Alerta.Warning(
                        'Informação Ausente',
                        'A data inicial da viagem é obrigatória',
                    );
                    $('#txtDataInicial').focus();
                    return false;
                }

                if (!$('#txtDataFinal').val()) {
                    Alerta.Warning(
                        'Informação Ausente',
                        'A data final da viagem é obrigatória',
                    );
                    $('#txtDataFinal').focus();
                    return false;
                }

                if (!$('#lstVeiculo').val()) {
                    Alerta.Warning(
                        'Informação Ausente',
                        'O veículo da viagem é obrigatório',
                    );
                    $('#lstVeiculo').focus();
                    return false;
                }

                if (!$('#lstMotorista').val()) {
                    Alerta.Warning(
                        'Informação Ausente',
                        'O motorista da viagem é obrigatório',
                    );
                    $('#lstMotorista').focus();
                    return false;
                }

                if (
                    !$('#txtKmInicial').val() ||
                    parseFloat($('#txtKmInicial').val()) === 0
                ) {
                    Alerta.Warning(
                        'Informação Ausente',
                        'A quilometragem inicial é obrigatória',
                    );
                    $('#txtKmInicial').focus();
                    return false;
                }

                if (
                    !$('#txtKmFinal').val() ||
                    parseFloat($('#txtKmFinal').val()) === 0
                ) {
                    Alerta.Warning(
                        'Informação Ausente',
                        'A quilometragem final é obrigatória',
                    );
                    $('#txtKmFinal').focus();
                    return false;
                }

                // Se passou todas as validações, submete o formulário
                AppToast.show('Amarelo', 'Salvando viagem...', 2000);
                $('#formViagem').submit();
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'insereviagem.js',
                    'btnSalvar.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'insereviagem.js',
            'document.ready',
            error,
        );
    }
});
