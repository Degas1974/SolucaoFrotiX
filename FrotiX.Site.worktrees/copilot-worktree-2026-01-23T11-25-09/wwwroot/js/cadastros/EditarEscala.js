$(document).ready(function () {
    try {
        inicializarEventosEditarEscala();
        inicializarSubmitEscala();
    } catch (error) {
        Alerta.TratamentoErroComLinha('EditarEscala.js', 'document.ready', error);
    }
});


function inicializarSubmitEscala() {
    try {
        console.log("inicializarSubmitEscala: Iniciando...");

        var form = $("#formEditarEscala");
        console.log("Formulário encontrado:", form.length > 0);

        if (form.length === 0) {
            console.error("ERRO: Formulário #formEditarEscala não encontrado!");
            return;
        }

        form.on("submit", function (e) {
            try {
                e.preventDefault();
                console.log("=== SUBMIT INTERCEPTADO ===");

                // Obter ID da escala
                var escalaDiaId = $("#hiddenEscalaDiaId").val();
                console.log("EscalaDiaId:", escalaDiaId);

                if (!escalaDiaId || escalaDiaId === '00000000-0000-0000-0000-000000000000') {
                    AppToast.show("Vermelho", "ID da escala inválido.", 3000000);
                    return false;
                }

                // Mostrar loading
                AppToast.show("Amarelo", "Salvando escala...", 2000);

                // Obter valores dos componentes Syncfusion
                var dataEscalaPicker = document.getElementById('dataEscala')?.ej2_instances?.[0];
                var horaInicioPicker = document.getElementById('horaInicio')?.ej2_instances?.[0];
                var horaFimPicker = document.getElementById('horaFim')?.ej2_instances?.[0];
                var turnoDropdown = document.getElementById('turnoId')?.ej2_instances?.[0];
                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
                var tipoServicoDropdown = document.getElementById('tipoServicoId')?.ej2_instances?.[0];
                var lotacaoDropdown = document.getElementById('lotacao')?.ej2_instances?.[0];
                var requisitanteDropdown = document.getElementById('requisitanteId')?.ej2_instances?.[0];
                var observacoesTextbox = document.getElementById('observacoes')?.ej2_instances?.[0];

                // =====================================================
                // COMPONENTES DE INDISPONIBILIDADE
                // =====================================================
                var categoriaDropdown = document.getElementById('categoriaIndisponibilidade')?.ej2_instances?.[0];
                var dataInicioIndispPicker = document.getElementById('dataInicioIndisponibilidade')?.ej2_instances?.[0];
                var dataFimIndispPicker = document.getElementById('dataFimIndisponibilidade')?.ej2_instances?.[0];
                var motoristaCobertorDropdown = document.getElementById('motoristaCobertorId')?.ej2_instances?.[0];

                console.log("Componentes Syncfusion:");
                console.log("- dataEscalaPicker:", dataEscalaPicker ? "OK" : "NULL");
                console.log("- horaInicioPicker:", horaInicioPicker ? "OK" : "NULL");
                console.log("- horaFimPicker:", horaFimPicker ? "OK" : "NULL");
                console.log("- turnoDropdown:", turnoDropdown ? "OK" : "NULL");
                console.log("- tipoServicoDropdown:", tipoServicoDropdown ? "OK" : "NULL");
                console.log("- categoriaDropdown:", categoriaDropdown ? "OK" : "NULL");
                console.log("- dataInicioIndispPicker:", dataInicioIndispPicker ? "OK" : "NULL");
                console.log("- dataFimIndispPicker:", dataFimIndispPicker ? "OK" : "NULL");
                console.log("- motoristaCobertorDropdown:", motoristaCobertorDropdown ? "OK" : "NULL");

                // Verificar se está marcado como indisponível
                var isIndisponivel = $("#MotoristaIndisponivel").is(":checked");
                
                // Verificar se veículo não definido está marcado
                var veiculoNaoDefinido = $("#veiculoNaoDefinido").is(":checked");

                // Montar objeto de dados
                var dados = {
                    EscalaDiaId: escalaDiaId,
                    MotoristaId: $("#hiddenMotoristaId").val(),
                    VeiculoId: veiculoNaoDefinido ? '' : (veiculoDropdown?.value || ''),
                    VeiculoNaoDefinido: veiculoNaoDefinido,
                    NumeroSaidas: $("#hiddenNumeroSaidas").val() || 0,
                    DataEscala: formatarData(dataEscalaPicker),
                    HoraInicio: formatarHora(horaInicioPicker),
                    HoraFim: formatarHora(horaFimPicker),
                    TurnoId: turnoDropdown?.value || '',
                    TipoServicoId: tipoServicoDropdown?.value || '',
                    Lotacao: lotacaoDropdown?.value || '',
                    RequisitanteId: requisitanteDropdown?.value || null,
                    Observacoes: observacoesTextbox?.value || '',
                    MotoristaIndisponivel: isIndisponivel,
                    MotoristaEconomildo: $("#MotoristaEconomildo").is(":checked"),
                    MotoristaEmServico: $("#MotoristaEmServico").is(":checked"),
                    MotoristaReservado: $("#MotoristaReservado").is(":checked"),

                    // =====================================================
                    // CAMPOS DE INDISPONIBILIDADE
                    // =====================================================
                    CategoriaIndisponibilidade: isIndisponivel ? (categoriaDropdown?.value || '') : '',
                    DataInicioIndisponibilidade: isIndisponivel ? formatarData(dataInicioIndispPicker) : '',
                    DataFimIndisponibilidade: isIndisponivel ? formatarData(dataFimIndispPicker) : '',
                    MotoristaCobertorId: isIndisponivel ? (motoristaCobertorDropdown?.value || '') : '',

                    // =====================================================
                    // DIAS DA SEMANA
                    // =====================================================
                    Segunda: $("#segunda").is(":checked"),
                    Terca: $("#terca").is(":checked"),
                    Quarta: $("#quarta").is(":checked"),
                    Quinta: $("#quinta").is(":checked"),
                    Sexta: $("#sexta").is(":checked"),
                    Sabado: $("#sabado").is(":checked"),
                    Domingo: $("#domingo").is(":checked")
                };

                // Log detalhado para debug
                console.log("=== DADOS A ENVIAR ===");
                console.log(JSON.stringify(dados, null, 2));

                // Validação dos campos de indisponibilidade
                if (isIndisponivel) {
                    if (!dados.DataInicioIndisponibilidade) {
                        AppToast.show("Vermelho", "Data de início da indisponibilidade é obrigatória.", 3000000);
                        return false;
                    }
                    if (!dados.DataFimIndisponibilidade) {
                        AppToast.show("Vermelho", "Data de fim da indisponibilidade é obrigatória.", 3000000);
                        return false;
                    }
                    if (!dados.CategoriaIndisponibilidade) {
                        AppToast.show("Vermelho", "Categoria da indisponibilidade é obrigatória.", 3000000);
                        return false;
                    }
                }

                // Token anti-forgery
                var token = $('input[name="__RequestVerificationToken"]').val();
                console.log("Token:", token ? "Presente" : "AUSENTE");

                // Enviar via AJAX para o Controller API
                console.log("Enviando para: /api/Escala/EditEscala");

                $.ajax({
                    url: '/api/Escala/EditEscala',
                    type: 'POST',
                    data: dados,
                    headers: {
                        'RequestVerificationToken': token
                    },
                    success: function (response) {
                        try {
                            console.log("=== RESPOSTA DO SERVIDOR ===");
                            console.log(response);

                            // Exibir debug no console se existir
                            if (response.debugLog) {
                                console.log("=== DEBUG LOG DO SERVIDOR ===");
                                console.log(response.debugLog);
                            }

                            if (response.success) {
                                AppToast.show("Verde", response.message || "Escala atualizada com sucesso!", 3000);
                                setTimeout(function () {
                                    window.location.href = '/Escalas/ListaEscala';
                                }, 2000);
                            } else {
                                AppToast.show("Vermelho", response.message || "Erro ao atualizar escala.", 3000000);
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha('EditarEscala.js', 'ajax.success', error);
                        }
                    },
                    error: function (xhr, status, error) {
                        try {
                            console.error('=== ERRO AJAX ===');
                            console.error('Status:', status);
                            console.error('Error:', error);
                            console.error('Response:', xhr.responseText);
                            console.error('Status Code:', xhr.status);

                            var mensagemErro = "Erro ao salvar escala. Tente novamente.";
                            try {
                                var resp = JSON.parse(xhr.responseText);
                                if (resp.message) mensagemErro = resp.message;
                            } catch (e) {
                                // Se não for JSON, usar mensagem padrão
                            }

                            AppToast.show("Vermelho", mensagemErro, 3000000);
                        } catch (error) {
                            Alerta.TratamentoErroComLinha('EditarEscala.js', 'ajax.error', error);
                        }
                    }
                });

                return false;
            } catch (error) {
                console.error("ERRO no submit:", error);
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'formEditarEscala.submit', error);
                return false;
            }
        });

        console.log("inicializarSubmitEscala: Evento de submit registrado com sucesso!");
    } catch (error) {
        console.error("ERRO em inicializarSubmitEscala:", error);
        Alerta.TratamentoErroComLinha('EditarEscala.js', 'inicializarSubmitEscala', error);
    }
}

/**
 * Formata data do DatePicker Syncfusion para yyyy-MM-dd
 */
function formatarData(picker) {
    try {
        if (picker && picker.value) {
            var data = new Date(picker.value);
            return data.getFullYear() + '-' +
                String(data.getMonth() + 1).padStart(2, '0') + '-' +
                String(data.getDate()).padStart(2, '0');
        }
        return '';
    } catch (error) {
        Alerta.TratamentoErroComLinha('EditarEscala.js', 'formatarData', error);
        return '';
    }
}

/**
 * Formata hora do TimePicker Syncfusion para HH:mm
 */
function formatarHora(picker) {
    try {
        if (picker && picker.value) {
            var hora = new Date(picker.value);
            return String(hora.getHours()).padStart(2, '0') + ':' +
                String(hora.getMinutes()).padStart(2, '0');
        }
        return '';
    } catch (error) {
        Alerta.TratamentoErroComLinha('EditarEscala.js', 'formatarHora', error);
        return '';
    }
}

/**
 * Inicializa todos os eventos da página de Editar Escala
 */
function inicializarEventosEditarEscala() {
    try {
        // =====================================================
        // CHECKBOX VEÍCULO NÃO DEFINIDO
        // =====================================================
        
        // Verificar estado inicial da checkbox
        setTimeout(function() {
            try {
                var checkbox = document.getElementById('veiculoNaoDefinido');
                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
                
                if (checkbox && veiculoDropdown) {
                    // Se checkbox está marcada, desabilitar dropdown
                    if (checkbox.checked) {
                        veiculoDropdown.enabled = false;
                        veiculoDropdown.value = null;
                    }
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'veiculoNaoDefinido.init', error);
            }
        }, 600);

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
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'veiculoNaoDefinido.change', error);
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
                            Alerta.TratamentoErroComLinha('EditarEscala.js', 'tipoServicoDropdown.change', error);
                        }
                    };
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'tipoServico.setTimeout', error);
            }
        }, 500);

        // Controlar exibição das seções baseado nos checkboxes
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
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaIndisponivel.change', error);
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
                    $('#statusReservado').removeClass('active-reservado');
                    $('#economildoSection').slideUp();
                    $('#indisponibilidadeSection').slideUp();
                } else {
                    $('#servicoSection').slideUp();
                    $('#statusServico').removeClass('active-servico');
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaEmServico.change', error);
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
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaEconomildo.change', error);
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
                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaReservado.change', error);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('EditarEscala.js', 'inicializarEventosEditarEscala', error);
    }
}

/**
 * Exclui uma escala via API
 * @param {string} escalaDiaId - ID da escala a ser excluída
 */
function excluirEscala(escalaDiaId) {
    try {
        if (!escalaDiaId || escalaDiaId === '00000000-0000-0000-0000-000000000000') {
            AppToast.show("Vermelho", "ID da escala inválido.", 3000);
            return;
        }

        // Confirmação antes de excluir
        if (!confirm('Tem certeza que deseja excluir esta escala? Esta ação não pode ser desfeita.')) {
            return;
        }

        AppToast.show("Amarelo", "Excluindo escala...", 2000);

        $.ajax({
            url: '/api/Escala/DeleteEscala/' + escalaDiaId,
            type: 'POST',
            success: function (response) {
                try {
                    if (response.success) {
                        AppToast.show("Verde", response.message || "Escala excluída com sucesso!", 3000);
                        setTimeout(function () {
                            window.location.href = '/Escalas/ListaEscala';
                        }, 1500);
                    } else {
                        AppToast.show("Vermelho", response.message || "Erro ao excluir escala.", 3000);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha('EditarEscala.js', 'excluirEscala.success', error);
                }
            },
            error: function (xhr, status, error) {
                try {
                    console.error('Erro ao excluir escala:', error);
                    AppToast.show("Vermelho", "Erro ao excluir escala. Tente novamente.", 3000);
                } catch (error) {
                    Alerta.TratamentoErroComLinha('EditarEscala.js', 'excluirEscala.error', error);
                }
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('EditarEscala.js', 'excluirEscala', error);
    }
}
