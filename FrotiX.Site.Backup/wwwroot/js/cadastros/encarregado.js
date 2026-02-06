/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: encarregado.js                                               ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                          ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Gestão de Encarregados - DataTable com CRUD completo.                 ║
 * ║    • Listagem paginada com server-side processing                        ║
 * ║    • Exclusão com confirmação SweetAlert                                 ║
 * ║    • Ativação/Inativação de registros                                    ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Média (Cadastros Básicos)                                 ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

var dataTable;

$(document).ready(function () {
    try {
        loadList();

        // Handler delegado para excluir encarregado
        $(document).on('click', '.btn-delete', function () {
            try {
                var id = $(this).data('id');

                Alerta.Confirmar(
                    'Confirmar Exclusão',
                    'Você tem certeza que deseja apagar este encarregado?',
                    'Sim, excluir',
                    'Cancelar',
                ).then((willDelete) => {
                    try {
                        if (willDelete) {
                            var dataToPost = JSON.stringify({
                                EncarregadoId: id,
                            });
                            $.ajax({
                                url: '/api/Encarregado/Delete',
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
                                                3000,
                                            );
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'encarregado.js',
                                            'Delete.success',
                                            error,
                                        );
                                    }
                                },
                                error: function (err) {
                                    try {
                                        console.error('Erro ao excluir:', err);
                                        AppToast.show(
                                            'Vermelho',
                                            'Erro ao excluir encarregado',
                                            3000,
                                        );
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'encarregado.js',
                                            'Delete.error',
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'encarregado.js',
                            'Delete.confirmar',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'encarregado.js',
                    'btn-delete.click',
                    error,
                );
            }
        });

        // Handler delegado para alterar status
        $(document).on('click', '.updateStatusEncarregado', function () {
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
                                // INATIVO
                                currentElement.html(
                                    '<i class="fa-duotone fa-circle-xmark me-1"></i> Inativo',
                                );
                                currentElement
                                    .removeClass('btn-verde')
                                    .addClass('fundo-cinza');
                            } else {
                                // ATIVO
                                currentElement.html(
                                    '<i class="fa-duotone fa-circle-check me-1"></i> Ativo',
                                );
                                currentElement
                                    .removeClass('fundo-cinza')
                                    .addClass('btn-verde');
                            }
                        } else {
                            Alerta.Erro(
                                'Erro ao Alterar Status',
                                'Ocorreu um erro ao tentar alterar o status.',
                                'OK',
                            );
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'encarregado.js',
                            'updateStatus.callback',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'encarregado.js',
                    'updateStatusEncarregado.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'encarregado.js',
            'document.ready',
            error,
        );
    }
});

function loadList() {
    try {
        dataTable = $('#tblEncarregado').DataTable({
            autoWidth: false,
            dom: 'Bfrtip',
            lengthMenu: [
                [10, 25, 50, -1],
                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas'],
            ],
            buttons: [
                'pageLength',
                'excel',
                {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                },
            ],
            order: [[0, 'asc']],
            columnDefs: [
                { targets: 0, className: 'text-left', width: '25%' },
                { targets: 1, className: 'text-center', width: '8%' },
                { targets: 2, className: 'text-center', width: '12%' },
                { targets: 3, className: 'text-left', width: '29%' },
                { targets: 4, className: 'text-center', width: '10%' },
                { targets: 5, className: 'text-center', width: '16%' },
            ],
            responsive: true,
            ajax: {
                url: '/api/encarregado',
                type: 'GET',
                datatype: 'json',
            },
            columns: [
                { data: 'nome' },
                { data: 'ponto' },
                { data: 'celular01' },
                { data: 'contratoEncarregado' },
                {
                    data: 'status',
                    render: function (data, type, row) {
                        try {
                            if (data) {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusEncarregado btn btn-verde text-white" 
                                           data-url="/api/Encarregado/UpdateStatusEncarregado?Id=${row.encarregadoId}" 
                                           style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">
                                            <i class="fa-duotone fa-circle-check me-1"></i> Ativo
                                        </a>`;
                            } else {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusEncarregado btn fundo-cinza text-white" 
                                           data-url="/api/Encarregado/UpdateStatusEncarregado?Id=${row.encarregadoId}" 
                                           style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">
                                            <i class="fa-duotone fa-circle-xmark me-1"></i> Inativo
                                        </a>`;
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'encarregado.js',
                                'render.status',
                                error,
                            );
                            return '';
                        }
                    },
                },
                {
                    data: 'encarregadoId',
                    render: function (data) {
                        try {
                            return `<div class="ftx-btn-acoes">
                                        <a href="/Encarregado/Upsert?id=${data}" 
                                           class="btn btn-editar btn-icon-28">
                                            <i class="fa-duotone fa-pen-to-square"></i>
                                        </a>
                                        <a href="javascript:void(0)" 
                                           class="btn btn-foto btn-icon-28" 
                                           data-id="${data}">
                                            <i class="fa-duotone fa-camera-retro"></i>
                                        </a>
                                        <a href="javascript:void(0)" 
                                           class="btn fundo-vermelho btn-icon-28 btn-delete" 
                                           data-id="${data}">
                                            <i class="fa-duotone fa-trash-can"></i>
                                        </a>
                                    </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'encarregado.js',
                                'render.acoes',
                                error,
                            );
                            return '';
                        }
                    },
                },
            ],
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
                emptyTable: 'Nenhum encarregado encontrado',
            },
            width: '100%',
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('encarregado.js', 'loadList', error);
    }
}
