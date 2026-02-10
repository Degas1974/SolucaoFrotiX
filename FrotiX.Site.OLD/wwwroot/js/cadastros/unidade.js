/* ****************************************************************************************
 * ⚡ ARQUIVO: unidade.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : CRUD de unidades com DataTable, exclusão com confirmação e gestão
 *                   de status Ativo/Inativo com feedback visual (classes btn-verde/fundo-cinza).
 * 📥 ENTRADAS     : DataTable #tblUnidade, botões delegados (.btn-delete com data-id,
 *                   .updateStatus com data-url), AJAX GET para toggle status
 * 📤 SAÍDAS       : POST /api/Unidade/Delete (exclusão), GET data-url (alteração status),
 *                   AppToast.show notificações (Verde/Vermelho), dataTable.ajax.reload(),
 *                   troca dinâmica de classes CSS (btn-verde ↔ fundo-cinza) e texto (Ativo/Inativo)
 * 🔗 CHAMADA POR  : Pages/Unidade/Index.cshtml, $(document).ready() inicialização
 * 🔄 CHAMA        : loadList() (inicialização DataTable com columnDefs), $.ajax() DELETE,
 *                   $.get().fail() para status, Alerta.Confirmar (confirmação SweetAlert),
 *                   Alerta.TratamentoErroComLinha (erro global), AppToast.show (notificações)
 * 📦 DEPENDÊNCIAS : jQuery, DataTables (columnDefs para alinhamento/largura colunas),
 *                   Alerta.js (SweetAlert wrapper), AppToast.js
 * 📝 OBSERVAÇÕES  : DataTable configurado com columnDefs específicos (5 colunas: text-left,
 *                   text-left, text-left, text-center, text-center com widths 6%/25%/15%/7%/8%).
 *                   Status binário (0=Ativo, 1=Inativo). Confirmação de exclusão: "Não será
 *                   possível recuperar os dados eliminados!". Try-catch aninhado em todos os
 *                   handlers (ready, click, ajax success/error/fail). 289 linhas incluindo
 *                   configuração detalhada de DataTable e handlers de erro com jqXHR logging.
 **************************************************************************************** */

var dataTable;

$(document).ready(function ()
{
    try
    {
        loadList();

        $(document).on("click", ".btn-delete", function ()
        {
            try
            {
                var id = $(this).data("id");

                Alerta.Confirmar(
                    "Confirmar Exclusão",
                    "Você tem certeza que deseja apagar esta unidade? Não será possível recuperar os dados eliminados!",
                    "Sim, excluir",
                    "Cancelar"
                ).then(function (confirmed)
                {
                    try
                    {
                        if (confirmed)
                        {
                            var dataToPost = JSON.stringify({ UnidadeId: id });
                            var url = "/api/Unidade/Delete";
                            $.ajax({
                                url: url,
                                type: "POST",
                                data: dataToPost,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data)
                                {
                                    try
                                    {
                                        if (data.success)
                                        {
                                            AppToast.show("Verde", data.message, 2000);
                                            dataTable.ajax.reload();
                                        } else
                                        {
                                            AppToast.show("Vermelho", data.message, 2000);
                                        }
                                    } catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha("unidade.js", "btn-delete.ajax.success", error);
                                    }
                                },
                                error: function (err)
                                {
                                    try
                                    {
                                        console.error(err);
                                        AppToast.show("Vermelho", "Ocorreu um erro ao excluir a unidade", 2000);
                                    } catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha("unidade.js", "btn-delete.ajax.error", error);
                                    }
                                },
                            });
                        }
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("unidade.js", "btn-delete.confirmar.then", error);
                    }
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("unidade.js", "btn-delete.click", error);
            }
        });

        $(document).on("click", ".updateStatus", function ()
        {
            try
            {
                var url = $(this).data("url");
                var currentElement = $(this);

                $.get(url, function (data)
                {
                    try
                    {
                        if (data.success)
                        {
                            AppToast.show("Verde", "Status alterado com sucesso!", 2000);
                            var text = "Ativo";

                            if (data.type == 1)
                            {
                                text = "Inativo";
                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                            } else
                            {
                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                            }

                            currentElement.text(text);
                        } else
                        {
                            AppToast.show("Vermelho", "Erro ao alterar o status. Tente novamente.", 2000);
                        }
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("unidade.js", "updateStatus.get.success", error);
                    }
                })
                    .fail(function (jqXHR)
                    {
                        try
                        {
                            console.error(jqXHR);
                            AppToast.show("Vermelho", "Ocorreu um erro ao alterar o status", 2000);
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("unidade.js", "updateStatus.get.fail", error);
                        }
                    });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("unidade.js", "updateStatus.click", error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("unidade.js", "document.ready", error);
    }
});

function loadList()
{
    try
    {
        dataTable = $("#tblUnidade").DataTable({
            columnDefs: [
                {
                    targets: 0,
                    className: "text-left",
                    width: "6%",
                },
                {
                    targets: 1,
                    className: "text-left",
                    width: "25%",
                },
                {
                    targets: 2,
                    className: "text-left",
                    width: "15%",
                },
                {
                    targets: 3,
                    className: "text-center",
                    width: "7%",
                },
                {
                    targets: 4,
                    className: "text-center",
                    width: "8%",
                },
                {
                    targets: 5,
                    className: "text-center",
                    width: "7%",
                },
                {
                    targets: 6,
                    className: "text-center",
                    width: "10%",
                },
            ],

            responsive: true,
            ajax: {
                url: "/api/unidade",
                type: "GET",
                datatype: "json",
            },
            columns: [
                { data: "sigla" },
                { data: "descricao" },
                { data: "primeiroContato" },
                { data: "pontoPrimeiroContato" },
                { data: "primeiroRamal" },
                {
                    data: "status",
                    render: function (data, type, row, meta)
                    {
                        try
                        {
                            if (data)
                            {
                                return '<a href="javascript:void(0)" ' +
                                    'class="updateStatus btn btn-verde text-white" ' +
                                    'data-url="/api/Unidade/UpdateStatus?Id=' + row.unidadeId + '" ' +
                                    'data-ejtip="Unidade ativa - clique para inativar" ' +
                                    'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">' +
                                    'Ativo</a>';
                            } else
                            {
                                return '<a href="javascript:void(0)" ' +
                                    'class="updateStatus btn fundo-cinza text-white text-bold" ' +
                                    'data-url="/api/Unidade/UpdateStatus?Id=' + row.unidadeId + '" ' +
                                    'data-ejtip="Unidade inativa - clique para ativar" ' +
                                    'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">' +
                                    'Inativo</a>';
                            }
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("unidade.js", "status.render", error);
                            return "";
                        }
                    },
                    width: "6%",
                    className: "text-center"
                },
                {
                    data: "unidadeId",
                    render: function (data)
                    {
                        try
                        {
                            return `<div class="text-center">
                                <a href="/Unidade/Upsert?id=${data}" 
                                   class="btn btn-azul text-white" 
                                   data-ejtip="Editar unidade" 
                                   aria-label="Editar unidade"
                                   style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                    <i class="far fa-edit"></i>
                                </a>
                                <a class="btn-delete btn btn-vinho text-white" 
                                   data-id="${data}"
                                   data-ejtip="Excluir unidade" 
                                   aria-label="Excluir unidade"
                                   style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                    <i class="far fa-trash-alt"></i>
                                </a>
                                <a href="/Unidade/VeiculosUnidade?id=${data}" 
                                   class="btn fundo-chocolate text-white" 
                                   data-ejtip="Veículos da unidade" 
                                   aria-label="Ver veículos da unidade"
                                   style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                    <i class="far fa-cars"></i>
                                </a>
                            </div>`;
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("unidade.js", "unidadeId.render", error);
                            return "";
                        }
                    },
                    className: "text-center"
                },
            ],

            language: {
                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                emptyTable: "Sem Dados para Exibição",
            },
            width: "100%",
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("unidade.js", "loadList", error);
    }
}
