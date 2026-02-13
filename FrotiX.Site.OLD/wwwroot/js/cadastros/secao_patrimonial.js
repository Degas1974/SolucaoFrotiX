/* ****************************************************************************************
 * ⚡ ARQUIVO: secao_patrimonial.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento de Seções Patrimoniais com DataTable, exclusão delegada,
 *                   verificação de pathname para garantir execução apenas na página correta.
 * 📥 ENTRADAS     : window.location.pathname (verificação de rota),
 *                   clique em .btn-delete (data-id), resposta Alerta.Confirmar (willDelete)
 * 📤 SAÍDAS       : DELETE via AJAX para /api/Secao/Delete,
 *                   AppToast (Verde/Vermelho), dataTable.ajax.reload,
 *                   console.log (debug), Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : Verificação de pathname (/secaopatrimonial/index ou /secaopatrimonial),
 *                   $(document).ready (loadGrid), event handler .btn-delete,
 *                   Pages/SecaoPatrimonial/Index.cshtml
 * 🔄 CHAMA        : window.location.pathname.toLowerCase(), loadGrid(),
 *                   Alerta.Confirmar, $.ajax, AppToast.show, dataTable.ajax.reload,
 *                   console.log, Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery 3.x, DataTables, Alerta.js, AppToast (toast notifications)
 * 📝 OBSERVAÇÕES  : Verifica path ANTES de executar (evita conflitos em outras páginas).
 *                   Try-catch aninhado em todos os níveis (ready, click, .then,
 *                   success, error). 448 linhas total.
 **************************************************************************************** */

var path = window.location.pathname.toLowerCase();
console.log(path);

if (path == "/secaopatrimonial/index" || path == "/secaopatrimonial")
{
    console.log("Entrou na seção index");

    $(document).ready(function ()
    {
        try
        {
            loadGrid();
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "document.ready", error);
        }
    });

    $(document).on("click", ".btn-delete", function ()
    {
        try
        {
            var id = $(this).data("id");
            console.log(id);

            Alerta.Confirmar(
                "Confirmar Exclusão",
                "Você tem certeza que deseja apagar esta Seção Patrimonial? Não será possível recuperar os dados eliminados!",
                "Sim, excluir",
                "Cancelar"
            ).then((willDelete) =>
            {
                try
                {
                    if (willDelete)
                    {
                        $.ajax({
                            url: "/api/Secao/Delete",
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify(id),
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
                                }
                                catch (error)
                                {
                                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.Delete.success", error);
                                }
                            },
                            error: function (err)
                            {
                                try
                                {
                                    console.error(err);
                                    Alerta.Erro("Erro ao Excluir", "Ocorreu um erro ao tentar excluir a seção patrimonial. Tente novamente.", "OK");
                                }
                                catch (error)
                                {
                                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.Delete.error", error);
                                }
                            },
                        });
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "btn-delete.Confirmar.then", error);
                }
            });
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "btn-delete.click", error);
        }
    });

    $(document).on("click", ".updateStatusSecao", function ()
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
                        var text;
                        if (data.type == 1)
                        {
                            text = "Inativa";
                            currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                        } else
                        {
                            text = "Ativa";
                            currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                        }

                        AppToast.show("Verde", data.message, 2000);

                        currentElement.text(text);
                    } else
                    {
                        Alerta.Erro("Erro ao Alterar Status", "Ocorreu um erro ao tentar alterar o status. Tente novamente.", "OK");
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "updateStatusSecao.get.callback", error);
                }
            });
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "updateStatusSecao.click", error);
        }
    });

    function loadGrid()
    {
        try
        {
            console.log("Entrou na loadGrid secao");
            dataTable = $("#tblSecao").DataTable({
                columnDefs: [
                    {
                        targets: 0, // NOME SECAO
                        className: "text-left",
                        width: "15%",
                    },
                    {
                        targets: 1, // NOME SETOR
                        className: "text-left",
                        width: "15%",
                    },
                    {
                        targets: 2, // ATIVO / INATIVO
                        className: "text-center",
                        width: "10%",
                    },
                    {
                        targets: 3, // AÇÃO
                        className: "text-center",
                        width: "10%",
                    },
                ],

                responsive: true,
                ajax: {
                    url: "/api/secao/ListaSecoes",
                    type: "GET",
                    datatype: "json",
                    error: function (xhr, status, error)
                    {
                        try
                        {
                            console.error("Erro ao carregar os dados: ", error);
                            Alerta.Erro("Erro ao Carregar Dados", "Não foi possível carregar a lista de seções patrimoniais.", "OK");
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.GetGrid.error", error);
                        }
                    },
                },
                columns: [
                    { data: "nomeSecao" },
                    { data: "nomeSetor" },
                    {
                        data: "status",
                        render: function (data, type, row, meta)
                        {
                            try
                            {
                                if (data)
                                {
                                    return (
                                        '<a href="javascript:void(0)" class="updateStatusSecao btn btn-verde btn-xs text-white" data-url="/api/Secao/updateStatusSecao?Id=' +
                                        row.secaoId +
                                        '" data-ejtip="Seção ativa - clique para inativar">Ativa</a>'
                                    );
                                } else
                                {
                                    return (
                                        '<a href="javascript:void(0)" class="updateStatusSecao btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Secao/updateStatusSecao?Id=' +
                                        row.secaoId +
                                        '" data-ejtip="Seção inativa - clique para ativar">Inativa</a>'
                                    );
                                }
                            }
                            catch (error)
                            {
                                Alerta.TratamentoErroComLinha("secao_patrimonial.js", "DataTable.render.status", error);
                                return "";
                            }
                        },
                        width: "6%",
                    },
                    {
                        data: "secaoId",
                        render: function (data)
                        {
                            try
                            {
                                return `<div class="text-center">
                                    <a href="/SecaoPatrimonial/Upsert?id=${data}"
                                       class="btn btn-azul text-white"
                                       data-ejtip="Editar seção patrimonial"
                                       style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                        <i class="far fa-edit"></i>
                                    </a>
                                    <a class="btn-delete btn btn-vinho text-white"
                                       data-id='${data}'
                                       data-ejtip="Excluir seção patrimonial"
                                       style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                        <i class="far fa-trash-alt"></i>
                                    </a>
                                </div>`;
                            }
                            catch (error)
                            {
                                Alerta.TratamentoErroComLinha("secao_patrimonial.js", "DataTable.render.acoes", error);
                                return "";
                            }
                        },
                    },
                ],

                language: {
                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                    emptyTable: "Sem Dados para Exibição",
                },
                width: "100%",
            });
            console.log("Saiu da LoadGrid");
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "loadGrid", error);
        }
    }
} else if (path === "/secaopatrimonial/upsert")
{
    console.log("Upsert seção");
    $(document).ready(function ()
    {
        try
        {
            loadListaSetores();
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "document.ready.upsert", error);
        }
    });

    function validaNome()
    {
        try
        {
            $(FormsSecao).on("submit", function (event)
            {
                try
                {
                    // Verifica se o nome está preenchido
                    var nomeSecao = document.getElementsByName("SecaoObj.NomeSecao")[0].value;

                    if (nomeSecao === "")
                    {
                        event.preventDefault(); // Isso aqui impede a página de ser recarregada
                        Alerta.Erro(
                            "Erro no Nome da Seção",
                            "O nome da seção não pode estar em branco!",
                            "OK"
                        );
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaNome.submit", error);
                }
            });
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaNome", error);
        }
    }

    function validaSetor()
    {
        try
        {
            $(FormsSecao).on("submit", function (event)
            {
                try
                {
                    // Verifica se o nome está preenchido
                    var setorId = $("#cmbSetor").data("kendoComboBox")?.value();

                    if (setorId === "" || setorId == null)
                    {
                        event.preventDefault(); // Isso aqui impede a página de ser recarregada
                        Alerta.Erro(
                            "Erro no Setor",
                            "O Setor da seção não pode estar em branco!",
                            "OK"
                        );
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaSetor.submit", error);
                }
            });
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaSetor", error);
        }
    }

    function loadListaSetores()
    {
        try
        {
            var comboBox = $("#cmbSetor").data("kendoComboBox");
            var secaoId = document.getElementsByName("SecaoObj.SecaoId");
            var setorId;
            if (secaoId.length <= 0)
            {
                if (comboBox) comboBox.value("");
                console.log(secaoId > 0);
            } else
            {
                setorId = comboBox?.value();
            }

            $.ajax({
                type: "get",
                url: "/api/Setor/ListaSetoresCombo",
                datatype: "json",
                success: function (res)
                {
                    try
                    {
                        if (res != null && res.data.length)
                        {
                            // Atualizar dataSource do Kendo ComboBox
                            if (comboBox)
                            {
                                comboBox.setDataSource(new kendo.data.DataSource({ data: res.data }));

                                if (setorId)
                                {
                                    // Isso aqui vai trocar o id que aparece por causa do razor para o nome correto do id representado
                                    var item = res.data.find((item) =>
                                    {
                                        try
                                        {
                                            return item.value.toLowerCase() == setorId.toString();
                                        }
                                        catch (error)
                                        {
                                            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "loadListaSetores.find", error);
                                            return false;
                                        }
                                    });
                                    console.log("item: ", item);
                                    if (item)
                                    {
                                        comboBox.value(item.value);
                                    }
                                }
                            }
                        } else
                        {
                            console.log("Nenhum setor encontrado.");
                        }
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.ListaSetores.success", error);
                    }
                },
                error: function (error)
                {
                    try
                    {
                        console.error("Erro ao carregar setores: ", error);
                        Alerta.Erro("Erro ao Carregar Setores", "Não foi possível carregar a lista de setores. Tente novamente.", "OK");
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.ListaSetores.error", error);
                    }
                },
            });
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "loadListaSetores", error);
        }
    }

    document.addEventListener("DOMContentLoaded", function ()
    {
        try
        {
            // Pega o elemento que contém o Guid
            const infoDiv = document.getElementById("divSecaoIdEmpty");

            // Lê o valor do atributo data-patrimonioid
            const secaoId = infoDiv.dataset.secaoid;
            console.log("Guid da Seção:", secaoId);

            // Verifica se é um Guid vazio
            const isEmptyGuid = secaoId === "00000000-0000-0000-0000-000000000000";

            // Seleciona o checkbox pelo ID
            const checkbox = document.getElementById("chkStatus");

            // Se for Guid vazio, marca o checkbox
            if (isEmptyGuid && checkbox)
            {
                checkbox.checked = true;
            }
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "DOMContentLoaded", error);
        }
    });
}
