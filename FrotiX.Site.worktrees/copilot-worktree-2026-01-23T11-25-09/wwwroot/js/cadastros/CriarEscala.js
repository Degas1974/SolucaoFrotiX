$(document).ready(function () {
    try {
        inicializarEventosEscala();
    } catch (error) {
        Alerta.TratamentoErroComLinha('CriarEscala.js', 'document.ready', error);
    }
});

/**
 * Inicializa todos os eventos da página de Criar Escala
 */
function inicializarEventosEscala() {
    try {
        // =====================================================
        // CHECKBOX VEÍCULO NÃO DEFINIDO
        // =====================================================
        
        $('#veiculoNaoDefinido').change(function() {
            try {
                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
                if (veiculoDropdown) {
                    if (this.checked) {
                        // Desabilitar dropdown e limpar valor
                        veiculoDropdown.enabled = false;
                        veiculoDropdown.value = null;
                        veiculoDropdown.text = '';
                    } else {
                        // Habilitar dropdown
                        veiculoDropdown.enabled = true;
                    }
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'veiculoNaoDefinido.change', error);
            }
        });

        // =====================================================
        // SINCRONIZAÇÃO TIPO SERVIÇO <-> CHECKBOX ECONOMILDO
        // =====================================================
        
        // Aguardar componentes Syncfusion carregarem
        setTimeout(function() {
            try {
                var tipoServicoDropdown = document.getElementById('tipoServicoId')?.ej2_instances?.[0];
                
                if (tipoServicoDropdown) {
                    // Evento quando TipoServico muda
                    tipoServicoDropdown.change = function(args) {
                        try {
                            if (!args.itemData) return;
                            
                            var textoSelecionado = args.itemData.Text || '';
                            
                            // Se selecionou "Economildo", marca a checkbox
                            if (textoSelecionado.toLowerCase() === 'economildo') {
                                if (!$('#MotoristaEconomildo').is(':checked')) {
                                    $('#MotoristaEconomildo').prop('checked', true).trigger('change');
                                }
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha('CriarEscala.js', 'tipoServicoDropdown.change', error);
                        }
                    };
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'tipoServico.setTimeout', error);
            }
        }, 500);

        // =====================================================
        // CHECKBOXES DE STATUS
        // =====================================================

        $('#MotoristaIndisponivel').change(function () {
            try {
                if (this.checked) {
                    $('#indisponibilidadeSection').slideDown();
                    $('#statusIndisponivel').addClass('active-indisponivel');

                    // Desmarcar outros status
                    $('#MotoristaEconomildo').prop('checked', false);
                    $('#MotoristaEmServico').prop('checked', false);
                    $('#MotoristaReservado').prop('checked', false);
                    $('#statusEconomildo').removeClass('active-economildo');
                    $('#economildoSection').slideUp();
                    $('#statusServico').removeClass('active-servico');
                    $('#statusReservado').removeClass('active-reservado');
                    $('#servicoSection').slideUp();
                } else {
                    $('#indisponibilidadeSection').slideUp();
                    $('#statusIndisponivel').removeClass('active-indisponivel');
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaIndisponivel.change', error);
            }
        });

        $('#MotoristaEmServico').change(function () {
            try {
                if (this.checked) {
                    $('#servicoSection').slideDown();
                    $('#statusServico').addClass('active-servico');

                    // Desmarcar outros status
                    $('#MotoristaIndisponivel').prop('checked', false);
                    $('#MotoristaEconomildo').prop('checked', false);
                    $('#MotoristaReservado').prop('checked', false);
                    $('#statusIndisponivel').removeClass('active-indisponivel');
                    $('#statusEconomildo').removeClass('active-economildo');
                    $('#economildoSection').slideUp();
                    $('#statusReservado').removeClass('active-reservado');
                    $('#indisponibilidadeSection').slideUp();
                } else {
                    $('#servicoSection').slideUp();
                    $('#statusServico').removeClass('active-servico');
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaEmServico.change', error);
            }
        });

        $('#MotoristaEconomildo').change(function () {
            try {
                var tipoServicoDropdown = document.getElementById('tipoServicoId')?.ej2_instances?.[0];
                
                if (this.checked) {
                    $('#economildoSection').slideDown();
                    $('#statusEconomildo').addClass('active-economildo');

                    // Desmarcar outros status
                    $('#MotoristaIndisponivel').prop('checked', false);
                    $('#MotoristaEmServico').prop('checked', false);
                    $('#MotoristaReservado').prop('checked', false);
                    $('#statusIndisponivel').removeClass('active-indisponivel');
                    $('#statusServico').removeClass('active-servico');
                    $('#statusReservado').removeClass('active-reservado');
                    $('#indisponibilidadeSection').slideUp();
                    $('#servicoSection').slideUp();
                    
                    // Selecionar "Economildo" no dropdown TipoServico
                    if (tipoServicoDropdown) {
                        var items = tipoServicoDropdown.dataSource;
                        if (items && items.length > 0) {
                            for (var i = 0; i < items.length; i++) {
                                if (items[i].Text && items[i].Text.toLowerCase() === 'economildo') {
                                    tipoServicoDropdown.value = items[i].Value;
                                    break;
                                }
                            }
                        }
                    }
                } else {
                    $('#economildoSection').slideUp();
                    $('#statusEconomildo').removeClass('active-economildo');
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaEconomildo.change', error);
            }
        });

        $('#MotoristaReservado').change(function () {
            try {
                if (this.checked) {
                    $('#statusReservado').addClass('active-reservado');

                    // Desmarcar outros status
                    $('#MotoristaIndisponivel').prop('checked', false);
                    $('#MotoristaEmServico').prop('checked', false);
                    $('#MotoristaEconomildo').prop('checked', false);
                    $('#statusIndisponivel').removeClass('active-indisponivel');
                    $('#statusServico').removeClass('active-servico');
                    $('#statusEconomildo').removeClass('active-economildo');
                    $('#indisponibilidadeSection').slideUp();
                    $('#servicoSection').slideUp();
                    $('#economildoSection').slideUp();
                } else {
                    $('#statusReservado').removeClass('active-reservado');
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaReservado.change', error);
            }
        });

        // Aguardar inicialização completa dos controles Syncfusion
        setTimeout(function () {
            inicializarEventosTurno();
            inicializarEventosMotorista();
        }, 500);

    } catch (error) {
        Alerta.TratamentoErroComLinha('CriarEscala.js', 'inicializarEventosEscala', error);
    }
}

/**
 * Inicializa eventos do campo Turno
 * Ajusta horários automaticamente baseado no turno selecionado
 */
function inicializarEventosTurno() {
    try {
        const turnoElement = document.getElementById('turnoId');

        if (!turnoElement || !turnoElement.ej2_instances || turnoElement.ej2_instances.length === 0) {
            console.warn('[CriarEscala.js] Controle turnoId não encontrado ou não inicializado');
            return;
        }

        const turnoDropdown = turnoElement.ej2_instances[0];

        turnoDropdown.change = function (args) {
            try {
                if (!args.itemData || !args.itemData.Text) {
                    return;
                }

                const turnoText = args.itemData.Text;

                // Verificar se os controles de hora existem
                const horaInicioElement = document.getElementById('horaInicio');
                const horaFimElement = document.getElementById('horaFim');

                if (!horaInicioElement || !horaInicioElement.ej2_instances || horaInicioElement.ej2_instances.length === 0) {
                    console.warn('[CriarEscala.js] Controle horaInicio não encontrado');
                    return;
                }

                if (!horaFimElement || !horaFimElement.ej2_instances || horaFimElement.ej2_instances.length === 0) {
                    console.warn('[CriarEscala.js] Controle horaFim não encontrado');
                    return;
                }

                const horaInicio = horaInicioElement.ej2_instances[0];
                const horaFim = horaFimElement.ej2_instances[0];

                // Ajustar horários baseado no turno
                if (turnoText.includes('Matutino')) {
                    horaInicio.value = new Date(2024, 0, 1, 6, 0);
                    horaFim.value = new Date(2024, 0, 1, 14, 0);
                } else if (turnoText.includes('Vespertino')) {
                    horaInicio.value = new Date(2024, 0, 1, 14, 0);
                    horaFim.value = new Date(2024, 0, 1, 22, 0);
                } else if (turnoText.includes('Noturno')) {
                    horaInicio.value = new Date(2024, 0, 1, 22, 0);
                    horaFim.value = new Date(2024, 0, 2, 6, 0);
                }

                console.log('[CriarEscala.js] Horários ajustados para turno:', turnoText);

            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'turnoDropdown.change', error);
            }
        };

    } catch (error) {
        Alerta.TratamentoErroComLinha('CriarEscala.js', 'inicializarEventosTurno', error);
    }
}

/**
 * Inicializa eventos do campo Motorista
 * Verifica disponibilidade do motorista na data selecionada
 */
function inicializarEventosMotorista() {
    try {
        const motoristaElement = document.getElementById('motoristaId');

        if (!motoristaElement || !motoristaElement.ej2_instances || motoristaElement.ej2_instances.length === 0) {
            console.warn('[CriarEscala.js] Controle motoristaId não encontrado ou não inicializado');
            return;
        }

        const motoristaDropdown = motoristaElement.ej2_instances[0];

        motoristaDropdown.change = function (args) {
            try {
                if (!args.value) {
                    return;
                }

                const motoristaId = args.value;

                const dataInicioElement = document.getElementById('dataInicio');

                if (!dataInicioElement || !dataInicioElement.ej2_instances || dataInicioElement.ej2_instances.length === 0) {
                    console.warn('[CriarEscala.js] Controle dataInicio não encontrado ou não inicializado');
                    return;
                }

                const dataInicioPicker = dataInicioElement.ej2_instances[0];
                const dataInicio = dataInicioPicker.value;

                if (motoristaId && dataInicio) {
                    // Fazer chamada AJAX para verificar disponibilidade
                    $.ajax({
                        url: '/api/Escala/VerificarDisponibilidade',
                        type: 'GET',
                        data: {
                            motoristaId: motoristaId,
                            data: dataInicio
                        },
                        success: function (response) {
                            try {
                                if (response && !response.disponivel) {
                                    AppToast.show('Amarelo',
                                        'Atenção: Este motorista já possui escala nesta data!',
                                        5000);
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha('CriarEscala.js', 'verificarDisponibilidade.success', error);
                            }
                        },
                        error: function (xhr, status, error) {
                            console.warn('[CriarEscala.js] Erro ao verificar disponibilidade:', error);
                        }
                    });
                }

            } catch (error) {
                Alerta.TratamentoErroComLinha('CriarEscala.js', 'motoristaDropdown.change', error);
            }
        };

    } catch (error) {
        Alerta.TratamentoErroComLinha('CriarEscala.js', 'inicializarEventosMotorista', error);
    }
}
