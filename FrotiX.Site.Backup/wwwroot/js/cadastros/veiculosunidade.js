/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: veiculosunidade.js                                           ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                          ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Vínculo Veículos por Unidade (Centro de Custo).                        ║
 * ║    • DataTable com filtros                                              ║
 * ║    • Transferência de veículos entre unidades                            ║
 * ║    • Relatório de veículos por unidade                                   ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Média (Veículos - Unidades)                                 ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

var dataTable;

$(document).ready(function () {
    try {
        loadList();

        $(document).on('click', '.btn-delete', function () {
            try {
                var id = $(this).data('id');

                Alerta.Confirmar(
                    'Confirmar Remoção',
                    'Você tem certeza que deseja remover este veículo da Unidade Usuária? Você deverá associá-lo novamente se necessário!',
                    'Sim, remover',
                    'Cancelar',
                ).then(function (confirmed) {
                    try {
                        if (confirmed) {
                            var dataToPost = JSON.stringify({ VeiculoId: id });
                            var url = '/api/VeiculosUnidade/Delete';
                            $.ajax({
                                url: url,
                                type: 'POST',
                                data: dataToPost,
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    try {
                                        if (data.success) {
                                            AppToast.show(
                                                'Verde',
                                                data.message,
                                                2000,
                                            );
                                            dataTable.ajax.reload();
                                        } else {
                                            AppToast.show(
                                                'Vermelho',
                                                data.message,
                                                2000,
                                            );
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'veiculosunidade_001.js',
                                            'btn-delete.ajax.success',
                                            error,
                                        );
                                    }
                                },
                                error: function (err) {
                                    try {
                                        console.log(err);
                                        AppToast.show(
                                            'Vermelho',
                                            'Ocorreu um erro ao remover o veículo',
                                            2000,
                                        );
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'veiculosunidade_001.js',
                                            'btn-delete.ajax.error',
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'veiculosunidade_001.js',
                            'btn-delete.confirmar.then',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'veiculosunidade_001.js',
                    'btn-delete.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'veiculosunidade_001.js',
            'document.ready',
            error,
        );
    }
});

function loadList() {
    try {
        dataTable = $('#tblVeiculosUnidade').DataTable({
            columnDefs: [
                {
                    targets: 0,
                    className: 'text-center',
                    width: '7%',
                },
                {
                    targets: 1,
                    className: 'text-left',
                    width: '17%',
                },
                {
                    targets: 2,
                    className: 'text-left',
                    width: '25%',
                },
                {
                    targets: 3,
                    className: 'text-center',
                    width: '8%',
                },
            ],

            responsive: true,
            ajax: {
                url: '/api/veiculosunidade',
                type: 'GET',
                datatype: 'json',
                data: 'unidadeId',
            },
            columns: [
                { data: 'placa' },
                { data: 'marcaModelo' },
                { data: 'contratoVeiculo' },
                {
                    data: 'veiculoId',
                    render: function (data) {
                        try {
                            return `<div class="text-center">
                                <a class="btn-delete btn btn-vinho btn-xs text-white" data-ejtip="Excluir o Veículo da Unidade Usuária" style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;" data-id='${data}'>
                                    <i class="far fa-trash-alt"></i>
                                </a>
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'veiculosunidade_001.js',
                                'veiculoId.render',
                                error,
                            );
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
            'veiculosunidade_001.js',
            'loadList',
            error,
        );
    }
}
