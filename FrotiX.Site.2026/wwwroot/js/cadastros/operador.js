/* ****************************************************************************************
 * ⚡ ARQUIVO: operador.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar listagem de operadores com DataTable, exclusão, edição,
 *                   atualização de status (ativo/inativo), visualização de foto.
 *                   Implementa handlers delegados para compatibilidade com grid dinâmico.
 *
 * 📥 ENTRADAS     : Página: /Operador/Index (listagem com filtros)
 *                   Cliques em: .btn-delete (data-id), .updateStatusOperador (data-url),
 *                   .btn-foto (data-id), .btn-editar (href com id)
 *                   Respostas de Alerta.Confirmar (confirmação modal)
 *
 * 📤 SAÍDAS       : - DataTable: grid de operadores com paginação e filtros
 *                   - Alertas: confirmação de exclusão, mensagens de sucesso/erro
 *                   - Toast: notificações AppToast (Verde/Vermelho)
 *                   - Modal: foto do operador
 *                   - Redirecionamentos: após edição
 *
 * 🔗 CHAMADA POR  : - Pages/Operador/Index.cshtml
 *                   - Eventos DOM: click em .btn-delete, .updateStatusOperador
 *                   - $(document).ready (inicialização)
 *
 * 🔄 CHAMA        : - loadList() [inicializa DataTable]
 *                   - POST /api/Operador/Delete [AJAX - exclusão]
 *                   - GET /api/Operador/UpdateStatusOperador [AJAX - toggle status]
 *                   - GET /api/operador [AJAX - lista paginada]
 *                   - Alerta.Confirmar() [SweetAlert]
 *                   - AppToast.show() [toast notifications]
 *                   - Alerta.TratamentoErroComLinha() [error handling]
 *
 * 📦 DEPENDÊNCIAS : jQuery 3.x, DataTables 1.10.25+, Syncfusion (status badges),
 *                   Alerta.js (FrotiX alerts), AppToast (notifications)
 *
 * 📝 OBSERVAÇÕES  : - Try-catch aninhado em todos os níveis (ready, click, AJAX)
 *                   - Handler delegado previne problemas com DataTable dinâmico
 *                   - Status togglável com cores: verde (ativo) / cinza (inativo)
 *                   - Ícones duotone F24: fa-circle-check, fa-circle-xmark, fa-trash-can
 *                   - Botões ações: editar (azul), foto (cinza), excluir (vermelho)
 **************************************************************************************** */

var dataTable;

$(document).ready(function () {
    try {
        loadList();

        // Handler delegado para excluir operador
        $(document).on("click", ".btn-delete", function () {
            try {
                var id = $(this).data("id");

                Alerta.Confirmar(
                    "Confirmar Exclusão",
                    "Você tem certeza que deseja apagar este operador?",
                    "Sim, excluir",
                    "Cancelar"
                ).then((willDelete) => {
                    try {
                        if (willDelete) {
                            var dataToPost = JSON.stringify({ OperadorId: id });
                            $.ajax({
                                url: "/api/Operador/Delete",
                                type: "POST",
                                data: dataToPost,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    try {
                                        if (data.success) {
                                            AppToast.show("Verde", data.message, 2000);
                                            dataTable.ajax.reload();
                                        } else {
                                            AppToast.show("Vermelho", data.message, 3000);
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha("operador.js", "Delete.success", error);
                                    }
                                },
                                error: function (err) {
                                    try {
                                        console.error("Erro ao excluir:", err);
                                        AppToast.show("Vermelho", "Erro ao excluir operador", 3000);
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha("operador.js", "Delete.error", error);
                                    }
                                }
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("operador.js", "Delete.confirmar", error);
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("operador.js", "btn-delete.click", error);
            }
        });

        // Handler delegado para alterar status
        $(document).on("click", ".updateStatusOperador", function () {
            try {
                var url = $(this).data("url");
                var currentElement = $(this);

                $.get(url, function (data) {
                    try {
                        if (data.success) {
                            AppToast.show("Verde", "Status alterado com sucesso!", 2000);

                            if (data.type == 1) {
                                // INATIVO
                                currentElement.html('<i class="fa-duotone fa-circle-xmark"></i> Inativo');
                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                                currentElement.attr("data-ejtip", "Operador inativo - clique para ativar");
                            } else {
                                // ATIVO
                                currentElement.html('<i class="fa-duotone fa-circle-check"></i> Ativo');
                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                                currentElement.attr("data-ejtip", "Operador ativo - clique para inativar");
                            }
                        } else {
                            Alerta.Erro("Erro ao Alterar Status", "Ocorreu um erro ao tentar alterar o status.", "OK");
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("operador.js", "updateStatus.callback", error);
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("operador.js", "updateStatusOperador.click", error);
            }
        });

    } catch (error) {
        Alerta.TratamentoErroComLinha("operador.js", "document.ready", error);
    }
});

/****************************************************************************************
 * ⚡ FUNÇÃO: loadList
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicializar DataTable de operadores com configuração completa
 *                   (colunas, ordenação, renderizadores, comportamento responsivo).
 *
 * 📥 ENTRADAS     : Nenhum parâmetro direto
 *
 * 📤 SAÍDAS       : DataTable inicializado em #tblOperador com:
 *                   - Grid paginado com operadores da API
 *                   - Colunas: Nome, Ponto, Celular, Contrato, Status, Ações
 *                   - Status com badges clicáveis (ativo/inativo)
 *                   - Botões: editar (azul), foto (cinza), excluir (vermelho)
 *
 * ⬅️ CHAMADO POR  : $(document).ready [linha 18]
 *
 * ➡️ CHAMA        : $.ajax GET /api/operador [linha 132-135]
 *                   DataTable() [jQuery plugin]
 *                   render callbacks para status e ações
 *
 * 📝 OBSERVAÇÕES  : - Renderizadores de status com feedback imediato
 *                   - Try-catch em todos os renders para segurança
 *                   - Responsivo com classes CSS Bootstrap
 ****************************************************************************************/
function loadList() {
    try {
        dataTable = $("#tblOperador").DataTable({
            autoWidth: false,
            dom: 'Bfrtip',
            lengthMenu: [
                [10, 25, 50, -1],
                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas']
            ],
            buttons: ['pageLength', 'excel', { extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'LEGAL' }],
            order: [[0, 'asc']],
            columnDefs: [
                { targets: 0, className: "text-left", width: "25%" },
                { targets: 1, className: "text-center", width: "8%" },
                { targets: 2, className: "text-center", width: "12%" },
                { targets: 3, className: "text-left", width: "25%" },
                { targets: 4, className: "text-center", width: "10%" },
                { targets: 5, className: "text-center", width: "20%" }
            ],
            responsive: true,
            ajax: {
                url: "/api/operador",
                type: "GET",
                datatype: "json"
            },
            columns: [
                { data: "nome" },
                { data: "ponto" },
                { data: "celular01" },
                { data: "contratoOperador" },
                {
                    data: "status",
                    render: function (data, type, row) {
                        try {
                            if (data) {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusOperador ftx-badge-status btn-verde" 
                                           data-ejtip="Operador ativo - clique para inativar" 
                                           data-url="/api/Operador/UpdateStatusOperador?Id=${row.operadorId}">
                                            <i class="fa-duotone fa-circle-check"></i> Ativo
                                        </a>`;
                            } else {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusOperador ftx-badge-status fundo-cinza" 
                                           data-ejtip="Operador inativo - clique para ativar" 
                                           data-url="/api/Operador/UpdateStatusOperador?Id=${row.operadorId}">
                                            <i class="fa-duotone fa-circle-xmark"></i> Inativo
                                        </a>`;
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("operador.js", "render.status", error);
                            return "";
                        }
                    }
                },
                {
                    data: "operadorId",
                    render: function (data) {
                        try {
                            return `<div class="ftx-btn-acoes">
                                        <a href="/Operador/Upsert?id=${data}" 
                                           class="btn btn-editar btn-icon-28" 
                                           data-ejtip="Editar Operador">
                                            <i class="fa-duotone fa-pen-to-square"></i>
                                        </a>
                                        <a href="javascript:void(0)" 
                                           class="btn btn-foto btn-icon-28" 
                                           data-ejtip="Foto do Operador" 
                                           data-id="${data}">
                                            <i class="fa-duotone fa-camera-retro"></i>
                                        </a>
                                        <a href="javascript:void(0)" 
                                           class="btn fundo-vermelho btn-icon-28 btn-delete" 
                                           data-ejtip="Excluir Operador" 
                                           data-id="${data}">
                                            <i class="fa-duotone fa-trash-can"></i>
                                        </a>
                                    </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("operador.js", "render.acoes", error);
                            return "";
                        }
                    }
                }
            ],
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                emptyTable: "Nenhum operador encontrado"
            },
            width: "100%"
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("operador.js", "loadList", error);
    }
}
