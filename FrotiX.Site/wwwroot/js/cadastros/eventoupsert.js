/* ****************************************************************************************
 * ⚡ ARQUIVO: eventoupsert.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Formulário complexo de Criar/Editar Eventos com Syncfusion componentes,
 *                   estatísticas de viagens (total, custo, média), validação de
 *                   participantes, preenchimento DDTs (Requisitante/Setor), e AJAX calls.
 * 📥 ENTRADAS     : Variáveis globais (eventoId, requisitanteId, setorsolicitanteId),
 *                   formulário de evento, input #txtQtdParticipantes,
 *                   GET /api/viagem/ObterTotalCustoViagensEvento
 * 📤 SAÍDAS       : Syncfusion DropDowns preenchidos (lstRequisitanteEvento, ddtSetorRequisitanteEvento),
 *                   data inicial setada (hoje se novo), validação de negativos,
 *                   estatísticas renderizadas (#totalViagens, #custoTotalViagens, #custoMedioViagem,
 *                   #viagensSemCusto), classes CSS (text-danger), formatação de moeda,
 *                   Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : $(document).ready, carregarEstatisticasViagens(), event handlers
 *                   (input #txtQtdParticipantes), Pages/Evento/Upsert.cshtml
 * 🔄 CHAMA        : document.getElementById, ej2_instances[0] (Syncfusion API),
 *                   $.on, $.ajax, carregarEstatisticasViagens(), formatarMoeda(),
 *                   $.addClass, Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery 3.x, Syncfusion EJ2 (DropDown, TextBox), Alerta.js
 * 📝 OBSERVAÇÕES  : Arquivo grande (845 linhas) com múltiplas funções (estatísticas,
 *                   validações, formatação). Try-catch em todos os handlers e funções.
 *                   Preenchimento condicional (editar vs criar) baseado em eventoId.
 **************************************************************************************** */

$(document).ready(function ()
{
    try
    {
        // Preenchimento inicial dos DDTs ao editar
        if (eventoId !== '00000000-0000-0000-0000-000000000000' && eventoId !== null)
        {
            const ddtReq = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
            const ddtSet = document.getElementById("ddtSetorRequisitanteEvento")?.ej2_instances?.[0];
            if (ddtReq) ddtReq.value = requisitanteId;
            if (ddtSet) ddtSet.value = setorsolicitanteId;
        } else
        {
            // Seta data inicial padrão hoje se novo
            const hoje = new Date().toISOString().slice(0, 10);
            const di = document.getElementById("txtDataInicialEvento");
            if (di && !di.value) di.value = hoje;
        }

        // Evita números negativos nos participantes
        $("#txtQtdParticipantes").on("input", function ()
        {
            try
            {
                const v = parseInt(this.value || "0", 10);
                if (v < 0) this.value = 0;
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "txtQtdParticipantes.input", error);
            }
        });

        // Função para carregar estatísticas das viagens
        function carregarEstatisticasViagens()
        {
            try
            {
                $.ajax({
                    url: "/api/viagem/ObterTotalCustoViagensEvento",
                    type: "GET",
                    data: { Id: eventoId },
                    success: function (response)
                    {
                        try
                        {
                            if (response.success)
                            {
                                // Preencher campos de estatísticas
                                $("#totalViagens").text(response.totalViagens);
                                $("#custoTotalViagens").text(response.totalCustoFormatado);
                                $("#viagensSemCusto").text(response.viagensSemCusto || "0");

                                // Calcular e preencher média
                                const media = response.custoMedioFormatado ||
                                    formatarMoeda(response.totalViagens > 0 ? response.totalCusto / response.totalViagens : 0);
                                $("#custoMedioViagem").text(media);

                                // Adicionar classes de destaque se necessário
                                if (response.viagensSemCusto > 0)
                                {
                                    $("#viagensSemCusto").addClass('text-danger');
                                }

                                console.log('Estatísticas carregadas:', response);
                            }
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens.success", error);
                        }
                    },
                    error: function (xhr, status, error)
                    {
                        try
                        {
                            console.error('Erro ao carregar estatísticas:', error);
                            // Valores padrão em caso de erro
                            $("#totalViagens").text("0");
                            $("#custoTotalViagens").text("R$ 0,00");
                            $("#custoMedioViagem").text("R$ 0,00");
                            $("#viagensSemCusto").text("0");
                        } catch (err)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens.error", err);
                        }
                    }
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens", error);
            }
        }

        // Função auxiliar para formatar moeda
        function formatarMoeda(valor)
        {
            try
            {
                if (!valor && valor !== 0) return "R$ 0,00";
                return parseFloat(valor).toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'BRL'
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "formatarMoeda", error);
                return "R$ 0,00";
            }
        }

        // Inicializa DataTable apenas se a tabela existir (edição)
        if ($("#tblViagens").length)
        {
            initEventoTable();
        }

        function initEventoTable()
        {
            try
            {
                // Mostra loading padrão FrotiX
                if (typeof mostrarLoading === 'function')
                {
                    mostrarLoading('Carregando Viagens do Evento...');
                }

                // Inicializar o DataTable
                const table = $('#tblViagens').DataTable({
                    lengthChange: false,
                    searching: false,
                    ordering: false,
                    deferRender: true,
                    responsive: true,
                    processing: false, // Desativa o spinner padrão do DataTable
                    ajax: {
                        url: "/api/viagem/listaviagensevento",
                        type: "GET",
                        data: { Id: eventoId },
                        dataSrc: 'data',
                        beforeSend: function ()
                        {
                            console.time('Requisição API');
                        },
                        complete: function (data)
                        {
                            try
                            {
                                // Esconde loading padrão FrotiX
                                if (typeof esconderLoading === 'function')
                                {
                                    esconderLoading();
                                }

                                // Após carregar os dados, buscar o total
                                console.timeEnd('Requisição API');
                                console.log('Quantidade de registros:', data.responseJSON?.data?.length);
                                carregarEstatisticasViagens();
                            } catch (error)
                            {
                                Alerta.TratamentoErroComLinha("eventoupsert.js", "DataTable.ajax.complete", error);
                            }
                        },
                        error: function (xhr, status, error)
                        {
                            // Esconde loading mesmo em caso de erro
                            if (typeof esconderLoading === 'function')
                            {
                                esconderLoading();
                            }
                            console.error('Erro ao carregar viagens:', error);
                        }
                    },
                    columns: [
                        {
                            data: "noFichaVistoria",
                            className: "text-center",
                            width: "5%",
                            render: function (data)
                            {
                                return data || '-';
                            }
                        },
                        {
                            data: "dataInicial",
                            className: "text-center",
                            width: "7%",
                            render: function (data, type, row)
                            {
                                if (!data) return '-';
                                if (type === 'display')
                                {
                                    const date = new Date(data);
                                    const dia = date.getDate().toString().padStart(2, '0');
                                    const mes = (date.getMonth() + 1).toString().padStart(2, '0');
                                    const ano = date.getFullYear();
                                    return `${dia}/${mes}/${ano}`;
                                }
                                return data;
                            }
                        },
                        {
                            data: "horaInicio",
                            className: "text-center",
                            width: "5%",
                            render: function (data, type, row)
                            {
                                if (!data) return '-';
                                if (type === 'display')
                                {
                                    const date = new Date(data);
                                    const horas = date.getHours().toString().padStart(2, '0');
                                    const minutos = date.getMinutes().toString().padStart(2, '0');
                                    return `${horas}:${minutos}`;
                                }
                                return data;
                            }
                        },
                        {
                            data: "nomeRequisitante",
                            className: "text-left",
                            width: "18%"
                        },
                        {
                            data: "nomeSetor",
                            className: "text-left",
                            width: "18%"
                        },
                        {
                            data: "nomeMotorista",
                            className: "text-left",
                            width: "12%",
                            render: function (data)
                            {
                                return data || '<span class="text-muted">-</span>';
                            }
                        },
                        {
                            data: "descricaoVeiculo",
                            className: "text-left",
                            width: "15%"
                        },
                        {
                            data: "custoViagem",
                            className: "text-end",
                            width: "10%",
                            render: function (data, type, row)
                            {
                                if (type === 'display')
                                {
                                    if (data === null || data === undefined)
                                    {
                                        return '<span class="text-muted">-</span>';
                                    }
                                    return data.toLocaleString('pt-BR', {
                                        style: 'currency',
                                        currency: 'BRL'
                                    });
                                }
                                return parseFloat(data) || 0;
                            }
                        },
                        {
                            data: "viagemId",
                            className: "text-center",
                            width: "10%",
                            orderable: false,
                            searchable: false,
                            render: function (data, type, row)
                            {
                                return `
                                    <div class="d-flex justify-content-center gap-1">
                                        <a class="btn fundo-roxo text-white btn-icon-28 btn-custos-viagem"
                                           data-id="${data}"
                                           data-requisitante="${row.nomeRequisitante || ''}"
                                           data-ficha="${row.noFichaVistoria || ''}"
                                           data-ejtip="Detalhamento de Custos">
                                            <i class="fa-duotone fa-file-invoice-dollar"></i>
                                        </a>
                                        <a class="btn btn-vinho text-white btn-icon-28 btn-desassociar-viagem"
                                           data-id="${data}"
                                           data-ficha="${row.noFichaVistoria || ''}"
                                           data-requisitante="${row.nomeRequisitante || ''}"
                                           data-ejtip="Desassociar do Evento">
                                            <i class="fa-duotone fa-link-slash"></i>
                                        </a>
                                    </div>`;
                            }
                        }
                    ],
                    language: {
                        url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                        processing: "Carregando...",
                        emptyTable: "Nenhuma viagem realizada encontrada para este evento"
                    },
                    pageLength: 10,
                    dom: 'rtip'
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "initEventoTable", error);
            }
        }

        // Carregar dados ao iniciar a página
        carregarEstatisticasViagens();

        // Atualizar estatísticas quando a tabela for redesenhada
        $('#tblViagens').on('draw.dt', function ()
        {
            try
            {
                carregarEstatisticasViagens();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "tblViagens.draw", error);
            }
        });

        // Função para atualizar o total via Ajax
        function atualizarTotalViagens()
        {
            try
            {
                $.ajax({
                    url: "/api/viagem/ObterTotalCustoViagensEvento",
                    type: "GET",
                    data: { Id: eventoId },
                    success: function (response)
                    {
                        try
                        {
                            if (response.success)
                            {
                                // Atualizar o título com os totais
                                $("#TituloViagens").html(
                                    `Viagens associadas ao Evento - ` +
                                    `Total de ${response.totalViagens} viagem(ns) - ` +
                                    `Custo Total: ${response.totalCustoFormatado}`
                                );

                                // Se você tiver outros campos para preencher
                                $("#totalViagens").text(response.totalViagens);
                                $("#custoTotal").text(response.totalCustoFormatado);

                                console.log('Total calculado com sucesso:', response);
                            } else
                            {
                                console.error('Erro ao calcular total:', response.error);
                                $("#TituloViagens").html(
                                    `Viagens associadas ao Evento - Erro ao calcular total`
                                );
                            }
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarTotalViagens.success", error);
                        }
                    },
                    error: function (xhr, status, error)
                    {
                        try
                        {
                            console.error('Erro na requisição:', error);
                            $("#TituloViagens").html(
                                `Viagens associadas ao Evento - Erro ao carregar total`
                            );
                        } catch (err)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarTotalViagens.error", err);
                        }
                    }
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarTotalViagens", error);
            }
        }

        // Chamar a função ao carregar a página
        atualizarTotalViagens();

        // Se quiser atualizar quando a tabela for recarregada
        $('#tblViagens').on('draw.dt', function ()
        {
            try
            {
                atualizarTotalViagens();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "tblViagens.draw.atualizarTotal", error);
            }
        });

        // Preenchimento inicial ao editar
        if (eventoId !== '00000000-0000-0000-0000-000000000000' && eventoId !== null)
        {
            const ddtReq = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
            const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');

            if (ddtReq && requisitanteId)
            {
                ddtReq.value = [requisitanteId];
            }

            if (ddlSetor && setorsolicitanteId)
            {
                ddlSetor.value = setorsolicitanteId;
            }
        }

        // ============================================
        // HANDLER: BOTÃO DE CUSTOS DA VIAGEM
        // ============================================
        $(document).on('click', '.btn-custos-viagem', function (e)
        {
            try
            {
                e.preventDefault();
                const viagemId = $(this).data('id');
                const requisitante = $(this).data('requisitante');

                console.log('Clique no botão de custos, Viagem ID:', viagemId);

                if (!viagemId)
                {
                    console.error('ID da viagem não encontrado');
                    AppToast.show('Vermelho', 'Erro ao abrir modal: ID não encontrado', 3000);
                    return;
                }

                // Atualiza o nome do requisitante no modal
                $('#requisitanteCustos').text('Requisitante: ' + (requisitante || 'Não informado'));

                // Carrega detalhamento de custos
                carregarDetalhamentoCustos(viagemId);

                // Abre o modal programaticamente
                const modalElement = document.getElementById('modalCustosViagem');
                if (modalElement)
                {
                    const modal = new bootstrap.Modal(modalElement);
                    modal.show();
                } else
                {
                    console.error('Modal de custos não encontrado no DOM');
                }

            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "btn-custos-viagem.click", error);
            }
        });

        // ============================================
        // HANDLER: BOTÃO DE DESASSOCIAR VIAGEM
        // ============================================
        $(document).on('click', '.btn-desassociar-viagem', function (e)
        {
            try
            {
                e.preventDefault();
                const viagemId = $(this).data('id');
                const ficha = $(this).data('ficha');
                const requisitante = $(this).data('requisitante');

                console.log('Clique no botão de desassociar, Viagem ID:', viagemId);

                if (!viagemId)
                {
                    console.error('ID da viagem não encontrado');
                    AppToast.show('Vermelho', 'Erro ao abrir modal: ID não encontrado', 3000);
                    return;
                }

                // Preenche os dados no modal
                $('#viagemIdDesassociar').val(viagemId);
                $('#infoViagemDesassociar').text(`Ficha ${ficha || '-'} - ${requisitante || 'Não informado'}`);
                $('#lstNovaFinalidade').val(''); // Limpa a seleção

                // Abre o modal programaticamente
                const modalElement = document.getElementById('modalDesassociar');
                if (modalElement)
                {
                    const modal = new bootstrap.Modal(modalElement);
                    modal.show();
                } else
                {
                    console.error('Modal de desassociação não encontrado no DOM');
                }

            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "btn-desassociar-viagem.click", error);
            }
        });

        // ============================================
        // HANDLER: CONFIRMAR DESASSOCIAÇÃO
        // ============================================
        $('#btnConfirmarDesassociar').on('click', function ()
        {
            try
            {
                const viagemId = $('#viagemIdDesassociar').val();
                const novaFinalidade = $('#lstNovaFinalidade').val();

                // Validação: finalidade obrigatória
                if (!novaFinalidade)
                {
                    AppToast.show('Amarelo', 'Selecione uma nova finalidade para a viagem!', 3000);
                    $('#lstNovaFinalidade').focus();
                    return;
                }

                // Desabilita o botão e mostra spinner
                const btn = $(this);
                const textoOriginal = btn.html();
                btn.prop('disabled', true);
                btn.html('<i class="fa-solid fa-spinner fa-spin icon-space"></i> Processando...');

                // Chama a API de desassociação
                $.ajax({
                    url: "/api/viagem/DesassociarViagemEvento",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        ViagemId: viagemId,
                        NovaFinalidade: novaFinalidade
                    }),
                    success: function (response)
                    {
                        try
                        {
                            if (response.success)
                            {
                                AppToast.show('Verde', response.message || 'Viagem desassociada com sucesso!', 3000);

                                // Fecha o modal
                                const modalElement = document.getElementById('modalDesassociar');
                                const modal = bootstrap.Modal.getInstance(modalElement);
                                if (modal) modal.hide();

                                // Recarrega o DataTable
                                if ($.fn.DataTable.isDataTable('#tblViagens'))
                                {
                                    $('#tblViagens').DataTable().ajax.reload(null, false);
                                }

                                // Atualiza estatísticas
                                carregarEstatisticasViagens();
                                atualizarTotalViagens();
                            } else
                            {
                                AppToast.show('Vermelho', response.message || 'Erro ao desassociar viagem', 3000);
                            }
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.success", error);
                        }
                    },
                    error: function (xhr, status, error)
                    {
                        try
                        {
                            console.error('Erro ao desassociar viagem:', error);
                            AppToast.show('Vermelho', 'Erro ao desassociar viagem do evento', 3000);
                        } catch (err)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.error", err);
                        }
                    },
                    complete: function ()
                    {
                        try
                        {
                            // Restaura o botão
                            btn.prop('disabled', false);
                            btn.html(textoOriginal);
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.complete", error);
                        }
                    }
                });

            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.click", error);
            }
        });

        // ============================================
        // FUNÇÃO: CARREGAR DETALHAMENTO DE CUSTOS
        // ============================================
        function carregarDetalhamentoCustos(viagemId)
        {
            try
            {
                // Limpa valores anteriores
                $('#infoViagemCustos').text('--');
                $('#tempoTotalCustos').text('-');
                $('#kmPercorridoCustos').text('-');
                $('#litrosGastosCustos').text('-');
                $('#custoMotoristaCustos').text('R$ 0,00');
                $('#custoVeiculoCustos').text('R$ 0,00');
                $('#custoCombustivelCustos').text('R$ 0,00');
                $('#custoTotalCustos').text('R$ 0,00');

                $.ajax({
                    url: "/api/viagem/ObterCustosViagem",
                    type: "GET",
                    data: { viagemId: viagemId },  // Parâmetro correto: viagemId
                    success: function (response)
                    {
                        try
                        {
                            if (response.success && response.data)
                            {
                                const d = response.data;

                                // Info da Viagem (compatível com ambos os formatos)
                                var infoViagem = d.infoViagem ?? d.InfoViagem;
                                if (infoViagem)
                                {
                                    $('#infoViagemCustos').text(infoViagem);
                                }

                                // Duração
                                var duracaoFormatada = d.duracaoFormatada ?? d.DuracaoFormatada;
                                if (duracaoFormatada)
                                {
                                    $('#tempoTotalCustos').text(duracaoFormatada);
                                }

                                // KM Percorrido
                                var kmPercorrido = d.kmPercorrido ?? d.KmPercorrido;
                                if (kmPercorrido !== undefined)
                                {
                                    $('#kmPercorridoCustos').text(kmPercorrido + ' km');
                                }

                                // Litros Gastos
                                var litrosGastos = d.litrosGastos ?? d.LitrosGastos;
                                if (litrosGastos !== undefined && litrosGastos > 0)
                                {
                                    $('#litrosGastosCustos').text(litrosGastos.toFixed(2).replace('.', ',') + ' L');
                                }

                                // Custos individuais (compatível com ambos os formatos)
                                var custoMotorista = d.custoMotorista ?? d.CustoMotorista ?? 0;
                                var custoVeiculo = d.custoVeiculo ?? d.CustoVeiculo ?? 0;
                                var custoCombustivel = d.custoCombustivel ?? d.CustoCombustivel ?? 0;
                                var custoTotal = d.custoTotal ?? d.CustoTotal ?? 0;

                                $('#custoMotoristaCustos').text(formatarMoeda(custoMotorista));
                                $('#custoVeiculoCustos').text(formatarMoeda(custoVeiculo));
                                $('#custoCombustivelCustos').text(formatarMoeda(custoCombustivel));

                                // Custo Total
                                $('#custoTotalCustos').text(formatarMoeda(custoTotal));

                                console.log('Custos carregados:', d);
                            } else
                            {
                                console.warn('Dados de custos não encontrados:', response.message);
                                AppToast.show('Amarelo', response.message || 'Dados de custos não disponíveis', 2000);
                            }
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos.success", error);
                        }
                    },
                    error: function (xhr, status, error)
                    {
                        try
                        {
                            console.error('Erro ao carregar custos:', error);
                            AppToast.show('Vermelho', 'Erro ao carregar detalhamento de custos', 3000);
                        } catch (err)
                        {
                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos.error", err);
                        }
                    }
                });

            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos", error);
            }
        }

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "document.ready", error);
    }
});

// Adicione esta função antes do DataTable
function calcularTotalViagens()
{
    try
    {
        const table = $('#tblViagens').DataTable();
        let total = 0;

        // Pegar todos os dados da coluna de custo
        table.rows({ page: 'current' }).every(function ()
        {
            const data = this.data();
            if (data && data.custoViagem)
            {
                total += parseFloat(data.custoViagem) || 0;
            }
        });

        // Atualizar o título
        $("#TituloViagens").html(
            `Viagens associadas ao Evento - Custo Total: R$ ${total.toFixed(2).replace('.', ',')}`
        );

        return total;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "calcularTotalViagens", error);
        return 0;
    }
}

document.addEventListener('DOMContentLoaded', function ()
{
    try
    {
        // Função para atualizar o campo de texto do setor
        function atualizarCampoSetor()
        {
            try
            {
                // Aguarda um momento para garantir que o DropDownTree foi atualizado
                setTimeout(function ()
                {
                    try
                    {
                        // Obtém a instância do DropDownTree de Setor
                        var setorDropDown = document.getElementById('ddtSetorRequisitanteEvento');
                        if (setorDropDown && setorDropDown.ej2_instances && setorDropDown.ej2_instances[0])
                        {
                            var setorInstance = setorDropDown.ej2_instances[0];

                            // Obtém o texto selecionado
                            var textoSetor = setorInstance.text || '';

                            // Se não houver texto, tenta obter de outras formas
                            if (!textoSetor && setorInstance.value)
                            {
                                // Tenta encontrar o texto baseado no value
                                var selectedData = setorInstance.treeData;
                                if (selectedData && selectedData.length > 0)
                                {
                                    textoSetor = findTextByValue(selectedData, setorInstance.value[0]);
                                }
                            }

                            // Atualiza o campo de texto
                            document.getElementById('txtSetorRequisitante').value = textoSetor;
                        }
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarCampoSetor.setTimeout", error);
                    }
                }, 100);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarCampoSetor", error);
            }
        }

        // Função auxiliar para encontrar texto pelo valor
        function findTextByValue(data, value)
        {
            try
            {
                for (var i = 0; i < data.length; i++)
                {
                    if (data[i].SetorSolicitanteId === value)
                    {
                        return data[i].Nome;
                    }
                    // Se houver subnós, procura recursivamente
                    if (data[i].child)
                    {
                        var found = findTextByValue(data[i].child, value);
                        if (found) return found;
                    }
                }
                return '';
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "findTextByValue", error);
                return '';
            }
        }

        // Monitora mudanças no DropDownTree oculto
        var setorDropDown = document.getElementById('ddtSetorRequisitanteEvento');
        if (setorDropDown)
        {
            setorDropDown.addEventListener('change', atualizarCampoSetor);

            // Se já houver um valor inicial, atualiza
            setTimeout(atualizarCampoSetor, 500);
        }

        // Observer para detectar mudanças no DOM do DropDownTree
        var observer = new MutationObserver(function (mutations)
        {
            try
            {
                mutations.forEach(function (mutation)
                {
                    if (mutation.type === 'attributes' || mutation.type === 'childList')
                    {
                        atualizarCampoSetor();
                    }
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "MutationObserver.callback", error);
            }
        });

        // Observa mudanças no DropDownTree de Setor
        if (setorDropDown)
        {
            observer.observe(setorDropDown, {
                attributes: true,
                childList: true,
                subtree: true,
                attributeFilter: ['value']
            });
        }

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "DOMContentLoaded", error);
    }
});
