/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: veiculo_index.js                                             ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                          ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Lista e Index de Veículos.                                            ║
 * ║    • DataTable com filtros e exportação                                  ║
 * ║    • Ações de editar, excluir                                            ║
 * ║    • Navegação para formulário de veículo                                 ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Alta (Veículos - Lista)                                    ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

(function () {
    'use strict';

    var dataTable;

    $(document).ready(function () {
        try {
            loadList();

            // Evento de exclusão de veículo
            $(document).on('click', '.btn-delete', function () {
                try {
                    var id = $(this).data('id');

                    Alerta.Confirmar(
                        'Confirmar Exclusão',
                        'Você tem certeza que deseja apagar este veículo? Não será possível recuperar os dados eliminados!',
                        'Sim, excluir',
                        'Cancelar',
                    ).then(function (confirmed) {
                        try {
                            if (confirmed) {
                                var dataToPost = JSON.stringify({
                                    VeiculoId: id,
                                });
                                var url = '/api/Veiculo/Delete';

                                $.ajax({
                                    url: url,
                                    type: 'POST',
                                    data: dataToPost,
                                    contentType:
                                        'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {
                                        try {
                                            if (data.success) {
                                                AppToast.show(
                                                    'Verde',
                                                    data.message ||
                                                        'Veículo excluído com sucesso.',
                                                    2000,
                                                );
                                                if (dataTable) {
                                                    dataTable.ajax.reload();
                                                }
                                            } else {
                                                AppToast.show(
                                                    'Vermelho',
                                                    data.message ||
                                                        'Erro ao excluir veículo.',
                                                    2000,
                                                );
                                            }
                                        } catch (error) {
                                            Alerta.TratamentoErroComLinha(
                                                'veiculo_index.js',
                                                'btn-delete.ajax.success',
                                                error,
                                            );
                                        }
                                    },
                                    error: function (err) {
                                        try {
                                            console.error(err);
                                            AppToast.show(
                                                'Vermelho',
                                                'Algo deu errado ao excluir o veículo.',
                                                2000,
                                            );
                                        } catch (error) {
                                            Alerta.TratamentoErroComLinha(
                                                'veiculo_index.js',
                                                'btn-delete.ajax.error',
                                                error,
                                            );
                                        }
                                    },
                                });
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'veiculo_index.js',
                                'btn-delete.confirmar.then',
                                error,
                            );
                        }
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'veiculo_index.js',
                        'btn-delete.click',
                        error,
                    );
                }
            });

            // Evento de atualização de status do veículo
            $(document).on('click', '.updateStatusVeiculo', function () {
                try {
                    var url = $(this).data('url');
                    var currentElement = $(this);

                    $.get(url, function (data) {
                        try {
                            if (data.success) {
                                AppToast.show(
                                    'Verde',
                                    'Status alterado com sucesso!',
                                    2000,
                                );

                                if (data.type == 1) {
                                    // Mudou para Inativo
                                    currentElement
                                        .removeClass('btn-verde')
                                        .addClass('fundo-cinza');
                                    currentElement.html(
                                        '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo',
                                    );
                                } else {
                                    // Mudou para Ativo
                                    currentElement
                                        .removeClass('fundo-cinza')
                                        .addClass('btn-verde');
                                    currentElement.html(
                                        '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo',
                                    );
                                }
                            } else {
                                AppToast.show(
                                    'Vermelho',
                                    'Não foi possível alterar o status.',
                                    2000,
                                );
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'veiculo_index.js',
                                'updateStatusVeiculo.get.success',
                                error,
                            );
                        }
                    }).fail(function () {
                        try {
                            AppToast.show(
                                'Vermelho',
                                'Erro ao alterar o status do veículo.',
                                2000,
                            );
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'veiculo_index.js',
                                'updateStatusVeiculo.get.fail',
                                error,
                            );
                        }
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'veiculo_index.js',
                        'updateStatusVeiculo.click',
                        error,
                    );
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'veiculo_index.js',
                'document.ready',
                error,
            );
        }
    });

    function loadList() {
        try {
            dataTable = $('#tblVeiculo').DataTable({
                columnDefs: [
                    {
                        targets: 0, // Placa
                        className: 'text-center',
                        width: '9%',
                    },
                    {
                        targets: 1, // Marca/Modelo
                        className: 'text-left',
                        width: '17%',
                    },
                    {
                        targets: 2, // Contrato
                        className: 'text-left',
                        width: '35%',
                    },
                    {
                        targets: 3, // Sigla
                        className: 'text-center',
                        width: '5%',
                        defaultContent: '',
                    },
                    {
                        targets: 4, // Combustível
                        className: 'text-center',
                        width: '5%',
                    },
                    {
                        targets: 5, // Consumo
                        className: 'text-right',
                        width: '3%',
                    },
                    {
                        targets: 6, // Quilometragem
                        className: 'text-right',
                        width: '3%',
                    },
                    {
                        targets: 7, // Reserva
                        className: 'text-center',
                        width: '5%',
                    },
                    {
                        targets: 8, // Status
                        className: 'text-center',
                        width: '7%',
                    },
                    {
                        targets: 9, // Ação
                        className: 'text-center',
                        width: '8%',
                    },
                ],
                responsive: true,
                ajax: {
                    url: '/api/veiculo',
                    type: 'GET',
                    datatype: 'json',
                },
                columns: [
                    { data: 'placa' },
                    { data: 'marcaModelo' },
                    { data: 'origemVeiculo' },
                    { data: 'sigla' },
                    { data: 'descricao' },
                    {
                        data: 'consumo',
                        render: function (data) {
                            try {
                                if (data === null || data === undefined)
                                    return '0,00';
                                return parseFloat(data)
                                    .toFixed(2)
                                    .replace('.', ',');
                            } catch (error) {
                                Alerta.TratamentoErroComLinha(
                                    'veiculo_index.js',
                                    'consumo.render',
                                    error,
                                );
                                return '0,00';
                            }
                        },
                    },
                    { data: 'quilometragem' },
                    { data: 'veiculoReserva' },
                    {
                        data: 'status',
                        render: function (data, type, row, meta) {
                            try {
                                if (data) {
                                    return `<a href="javascript:void(0)" 
                                               class="updateStatusVeiculo btn btn-verde text-white" 
                                               data-url="/api/Veiculo/updateStatusVeiculo?Id=${row.veiculoId}" 
                                               data-ejtip="Desativar veículo" 
                                               style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">
                                                <i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo
                                            </a>`;
                                } else {
                                    return `<a href="javascript:void(0)" 
                                               class="updateStatusVeiculo btn fundo-cinza text-white" 
                                               data-url="/api/Veiculo/updateStatusVeiculo?Id=${row.veiculoId}" 
                                               data-ejtip="Ativar veículo" 
                                               style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">
                                                <i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo
                                            </a>`;
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha(
                                    'veiculo_index.js',
                                    'status.render',
                                    error,
                                );
                                return '';
                            }
                        },
                    },
                    {
                        data: 'veiculoId',
                        render: function (data) {
                            try {
                                return `<div class="text-center">
                                    <a href="/Veiculo/Upsert?id=${data}" 
                                       class="btn btn-azul text-white" 
                                       data-ejtip="Editar veículo" 
                                       aria-label="Editar veículo" 
                                       style="cursor:pointer; padding: 4px 8px; font-size: 12px; border-radius: 6px; margin: 1px;">
                                        <i class="fa-duotone fa-pen-to-square" style="--fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>
                                    </a>
                                    <a class="btn-delete btn btn-vinho text-white" 
                                       data-ejtip="Excluir veículo" 
                                       aria-label="Excluir veículo" 
                                       style="cursor:pointer; padding: 4px 8px; font-size: 12px; border-radius: 6px; margin: 1px;" 
                                       data-id='${data}'>
                                        <i class="fa-duotone fa-trash-can" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i>
                                    </a>
                                </div>`;
                            } catch (error) {
                                Alerta.TratamentoErroComLinha(
                                    'veiculo_index.js',
                                    'veiculoId.render',
                                    error,
                                );
                                return '';
                            }
                        },
                    },
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
                    emptyTable: 'Sem Dados para Exibição',
                },
                width: '100%',
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'veiculo_index.js',
                'loadList',
                error,
            );
        }
    }
})();
