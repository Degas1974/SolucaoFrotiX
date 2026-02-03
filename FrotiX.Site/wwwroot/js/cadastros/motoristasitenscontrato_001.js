/* ****************************************************************************************
 * ⚡ ARQUIVO: motoristasitenscontrato_001.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : DataTable de motoristas em contrato com configuração de colunas
 *                   (Nome, Ponto, CNH, Categoria, Celular, Unidade, etc.), loadList
 *                   inicial via loadMotoristaList().
 * 📥 ENTRADAS     : $(document).ready, chamada loadMotoristaList()
 * 📤 SAÍDAS       : DataTable renderizado (#tblMotorista) com columnDefs,
 *                   console logs, Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : $(document).ready, Pages/ItensContrato/Motoristas.cshtml
 * 🔄 CHAMA        : loadMotoristaList(), $("#tblMotorista").DataTable(),
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery 3.x, DataTables, Alerta.js
 * 📝 OBSERVAÇÕES  : DataTable com 6+ colunas (Nome text-left, Ponto/CNH/Cat text-center,
 *                   Celular, Unidade). Try-catch em ready e loadMotoristaList. 152 linhas total.
 **************************************************************************************** */

var dataTableMotorista;

$(document).ready(function () {
    try
    {
        loadMotoristaList();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha(
            "motoristasitenscontrato_<num>.js",
            "callback@$.ready#0",
            error,
        );
    }
});

function loadMotoristaList() {
    try
    {
        dataTableMotorista = $("#tblMotorista").DataTable({
            columnDefs: [
                {
                    targets: 0, //Nome
                    className: "text-left",
                    width: "15%",
                },
                {
                    targets: 1, //Ponto
                    className: "text-center",
                    width: "6%",
                },
                {
                    targets: 2, //CNH
                    className: "text-center",
                    width: "6%",
                },
                {
                    targets: 3, //Cat.
                    className: "text-center",
                    width: "5%",
                    defaultContent: "",
                },
                {
                    targets: 4, //Celular
                    className: "text-center",
                    width: "8%",
                },
                {
                    targets: 5, //Unidade
                    className: "text-left",
                    width: "5%",
                },
                {
                    targets: 6, //Contrato
                    className: "text-left",
                    width: "10%",
                },
                {
                    targets: 7, //Status
                    className: "text-center",
                    width: "5%",
                },
                {
                    targets: 8, //Ação
                    className: "text-center",
                    width: "8%",
                },
            ],

            responsive: true,
            ajax: {
                url: "/api/motorista",
                type: "GET",
                datatype: "json",
            },
            columns: [
                { data: "nome" },
                { data: "ponto" },
                { data: "cnh" },
                { data: "categoriaCNH" },
                { data: "celular01" },
                { data: "sigla" },
                { data: "contratoMotorista" },
                {
                    data: "status",
                    render: function (data, type, row, meta) {
                        try
                        {
                            if (data)
                                return (
                                    '<a href="javascript:void" class="updateStatusMotorista btn btn-verde btn-xs text-white" data-url="/api/Motorista/updateStatusMotorista?Id=' +
                                    row.motoristaId +
                                    '">Ativo</a>'
                                );
                            else
                                return (
                                    '<a href="javascript:void" class="updateStatusMotorista btn  btn-xs fundo-cinza text-white text-bold" data-url="/api/Motorista/updateStatusMotorista?Id=' +
                                    row.motoristaId +
                                    '">Inativo</a>'
                                );
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha(
                                "motoristasitenscontrato_<num>.js",
                                "render",
                                error,
                            );
                        }
                    },
                },
                {
                    data: "motoristaId",
                    render: function (data) {
                        try
                        {
                            return `<div class="text-center">
                                <a class="btn-delete btn btn-vinho btn-xs text-white"   aria-label="Excluir o Motorista do Contrato!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                    <i class="far fa-trash-alt"></i>
                                </a>
                    </div>`;
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha(
                                "motoristasitenscontrato_<num>.js",
                                "render",
                                error,
                            );
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
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha(
            "motoristasitenscontrato_<num>.js",
            "loadMotoristaList",
            error,
        );
    }
}
