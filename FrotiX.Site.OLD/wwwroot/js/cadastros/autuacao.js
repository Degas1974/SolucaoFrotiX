/* ****************************************************************************************
 * ⚡ ARQUIVO: autuacao.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar preenchimento dinâmico de lista de empenhos (Kendo DropDownList)
 *                   baseado na seleção de órgão autuante. Carrega empenhos via AJAX e
 *                   atualiza componente lstEmpenhos (Kendo UI).
 * 📥 ENTRADAS     : lstOrgaoChange() - Seleção de órgão (lstOrgao.value),
 *                   GET /Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos&id,
 *                   res.data (array de {empenhoMultaId, notaEmpenho})
 * 📤 SAÍDAS       : Kendo DropDownList lstEmpenhos atualizado (setDataSource),
 *                   campo hidden #txtEmpenhoMultaId limpo, console.log (debug),
 *                   Alerta.TratamentoErroComLinha em caso de erro
 * 🔗 CHAMADA POR  : Event handler lstOrgaoChange (Kendo change event), formulário de autuação
 * 🔄 CHAMA        : $.ajax, $("#id").data("kendoXxx") (Kendo UI API),
 *                   setDataSource, Alerta.TratamentoErroComLinha, console.log
 * 📦 DEPENDÊNCIAS : jQuery 3.x, Kendo UI (DropDownList, ComboBox), Alerta.js
 * 📝 OBSERVAÇÕES  : Limpa lstEmpenhos antes de carregar novos dados. Constrói array
 *                   EmpenhoList dinamicamente. Usa Kendo UI jQuery API para acessar
 *                   instâncias dos widgets. Try-catch em success handler.
 **************************************************************************************** */

//Escolheu um órgão
//=================
/****************************************************************************************
 * ⚡ FUNÇÃO: lstOrgaoChange
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Buscar lista de empenhos da API conforme órgão selecionado e
 *                   atualizar dropdown Syncfusion lstEmpenhos
 *
 * 📥 ENTRADAS     : lstOrgao.value (ID do órgão selecionado)
 *
 * 📤 SAÍDAS       : Kendo DropDownList lstEmpenhos atualizado com setDataSource,
 *                   campo txtEmpenhoMultaId limpo
 *
 * ⬅️ CHAMADO POR  : Kendo change event lstOrgao
 *
 * ➡️ CHAMA        : GET /Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos [AJAX]
 *                   $("#id").data("kendoDropDownList/kendoComboBox") (Kendo UI API)
 *                   Alerta.TratamentoErroComLinha
 *
 * 📝 OBSERVAÇÕES  : Limpa lstEmpenhos antes de carregar. Constrói EmpenhoList dinamicamente.
 *                   Usa Kendo UI jQuery API para acessar instâncias dos widgets.
 ****************************************************************************************/
function lstOrgaoChange() {
    try
    {
        var ddlEmpenhos = $("#lstEmpenhos").data("kendoDropDownList");
        if (ddlEmpenhos) {
            ddlEmpenhos.setDataSource(new kendo.data.DataSource({ data: [] }));
            ddlEmpenhos.value("");
            ddlEmpenhos.text("");
        }
        $("#txtEmpenhoMultaId").attr("value", "");

        var cmbOrgao = $("#lstOrgao").data("kendoComboBox");
        var orgaoValue = cmbOrgao ? cmbOrgao.value() : null;
        console.log(orgaoValue);

        if (!orgaoValue) {
            return;
        }

        var orgaoid = String(orgaoValue);

        /********************************************************************************
         * [AJAX] Endpoint: GET /Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos
         * ======================================================================
         * 📥 ENVIA        : id (ID do órgão autuante)
         * 📤 RECEBE       : { data: [ { empenhoMultaId, notaEmpenho }, ... ] }
         * 🎯 MOTIVO       : Carregar lista de empenhos de um órgão específico para
         *                   popular dropdown Syncfusion lstEmpenhos
         ********************************************************************************/
        $.ajax({
            url: "/Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos",
            method: "GET",
            datatype: "json",

            data: { id: orgaoid },

            success: function (res) {
                try
                {
                    if (res.data.length != 0) {
                        var empenhomultaid = res.data[0].empenhoMultaId;
                        var notaempenho = res.data[0].notaEmpenho;

                        // [LOGICA] Constrói array de empenhos a partir da resposta
                        let EmpenhoList = [
                            { EmpenhoMultaId: empenhomultaid, NotaEmpenho: notaempenho },
                        ];

                        for (var i = 1; i < res.data.length; ++i) {
                            console.log(
                                res.data[i].empenhoMultaId + " - " + res.data[i].notaEmpenho,
                            );

                            empenhomultaid = res.data[i].empenhoMultaId;
                            notaempenho = res.data[i].notaEmpenho;

                            let empenho = {
                                EmpenhoMultaId: empenhomultaid,
                                NotaEmpenho: notaempenho,
                            };
                            EmpenhoList.push(empenho);
                        }

                        // [UI] Atualiza dropdown Kendo com novos dados
                        var ddlEmp = $("#lstEmpenhos").data("kendoDropDownList");
                        if (ddlEmp) {
                            ddlEmp.setDataSource(new kendo.data.DataSource({ data: EmpenhoList }));
                        }
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("autuacao_<num>.js", "lstOrgaoChange.success", error);
                }
            },
        });

        var ddlEmpRefresh = $("#lstEmpenhos").data("kendoDropDownList");
        if (ddlEmpRefresh) { ddlEmpRefresh.dataSource.read(); }

    //    Alerta.Info(
    //        "Empenho do órgão",
    //        "Já existe o empenho correto cadastrado para o órgão?",
    //        "OK"
    //    );
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("autuacao_<num>.js", "lstOrgaoChange", error);
    }
}

// Por algum motivo o v�nculo do lstEmpenho com o banco de dados n�o est� funcionando. Ent�o estou escondendo o ID do empenho em um text box escondido
function lstEmpenhosChange() {
    try
    {
        var ddlEmpenhos = $("#lstEmpenhos").data("kendoDropDownList");
        var empenhosValue = ddlEmpenhos ? ddlEmpenhos.value() : "";
        $("#txtEmpenhoMultaId").attr("value", empenhosValue);

        var empenhoid = String(empenhosValue);

        $.ajax({
            url: "/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho",
            method: "GET",
            datatype: "json",

            data: { id: empenhoid },

            success: function (res) {
                try
                {
                    //debugger;

                    var saldoempenho = res.data;

                    $("#txtSaldoEmpenho").val(
                        Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(
                            saldoempenho,
                        ),
                    );
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("autuacao_<num>.js", "success", error);
                }
            },
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("autuacao_<num>.js", "lstEmpenhosChange", error);
    }
}

// Edição: popula viewer/valores
$(document).ready(function ()
{
    try
    {
        if (multaId != '00000000-0000-0000-0000-000000000000')
        {
            var ddlInfracao = $("#lstInfracao").data("kendoDropDownList");
            if (ddlInfracao) { ddlInfracao.value('@Model.MultaObj.Multa.TipoMultaId'.toString()); }
            $('#txtNoFichaVistoria').val('@Model.MultaObj.Multa.NoFichaVistoria');

            if ('@Model.MultaObj.Multa.AutuacaoPDF')
            {
                createPdfViewer("/DadosEditaveis/Multas/" + encodeURIComponent('@Model.MultaObj.Multa.AutuacaoPDF'));
            }
            if (!('@Model.MultaObj.Multa.ValorAteVencimento') || '@Model.MultaObj.Multa.ValorAteVencimento' == 0) { $('#txtValorAteVencimento').val("0,00"); }
            if (!('@Model.MultaObj.Multa.ValorPosVencimento') || '@Model.MultaObj.Multa.ValorPosVencimento' == 0) { $('#txtValorPosVencimento').val("0,00"); }

            // Função está no JS externo
            if (typeof lstEmpenhosChange === "function") lstEmpenhosChange();
        } else
        {
            var _cmbContratoVeiculo = $("#lstContratoVeiculo").data("kendoComboBox");
            if (_cmbContratoVeiculo) { _cmbContratoVeiculo.value(""); _cmbContratoVeiculo.text(""); }
            var _cmbContratoMotorista = $("#lstContratoMotorista").data("kendoComboBox");
            if (_cmbContratoMotorista) { _cmbContratoMotorista.value(""); _cmbContratoMotorista.text(""); }
            var _cmbOrgao = $("#lstOrgao").data("kendoComboBox");
            if (_cmbOrgao) { _cmbOrgao.value(""); _cmbOrgao.text(""); }
            var _ddlEmpenhos = $("#lstEmpenhos").data("kendoDropDownList");
            if (_ddlEmpenhos) { _ddlEmpenhos.value(""); _ddlEmpenhos.text(""); }
            var _cmbVeiculo = $("#lstVeiculo").data("kendoComboBox");
            if (_cmbVeiculo) { _cmbVeiculo.value(""); _cmbVeiculo.text(""); }
            var _cmbAtaVeiculo = $("#lstAtaVeiculo").data("kendoComboBox");
            if (_cmbAtaVeiculo) { _cmbAtaVeiculo.value(""); _cmbAtaVeiculo.text(""); }
            var _cmbMotorista = $("#lstMotorista").data("kendoComboBox");
            if (_cmbMotorista) { _cmbMotorista.value(""); _cmbMotorista.text(""); }
            $('#txtValorAteVencimento').val("0,00");
            $('#txtValorPosVencimento').val("0,00");
        }
    } catch (error) { Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "document.ready", error); }
});

// Viagem e Ficha de Vistoria
$(document).on('click', '.btnViagem', function ()
{
    if (!$('#txtDataInfracao').val())
    {
        Alerta.Warning("Informação Ausente", "A Data da Infração deve ser informada")
        return;
    }

    if (!$('#txtHoraInfracao').val())
    {
        Alerta.Warning("Informação Ausente", "A Hora da Infração é obrigatória")
        return;
    }

    var cmbVeiculo = $("#lstVeiculo").data("kendoComboBox");
    var veiculoValue = cmbVeiculo ? cmbVeiculo.value() : null;
    if (!veiculoValue)
    {
        Alerta.Warning("Informação Ausente", "O Veículo deve ser informado")
        return;
    }

    var dataToPost = JSON.stringify({ VeiculoId: veiculoValue, Data: $('#txtDataInfracao').val(), Hora: $('#txtHoraInfracao').val() });
    $.ajax({
        url: '/api/Multa/ProcuraViagem',
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
                    AppToast.show('Verde', data.message);
                    $('#txtNoFichaVistoria').val(data.nofichavistoria);
                    $('#txtNoFichaVistoriaEscondido').val(data.nofichavistoria);
                    EscolhendoMotorista = true;
                    var cmbMotorista = $("#lstMotorista").data("kendoComboBox");
                    if (cmbMotorista) { cmbMotorista.value(data.motoristaid ? data.motoristaid.toString() : ""); }
                } else
                {
                    $('#txtNoFichaVistoria').val('');
                    $('#txtNoFichaVistoriaEscondido').val('');
                    AppToast.show('Vermelho', data.message);
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "ProcuraViagem.success", error);
            }
        },
    });
});

var ViagemId = '';
$(document).on('click', '.btnFicha', function ()
{
    if (!$('#txtNoFichaVistoria').val())
    {
        Alerta.Warning("Informação Ausente", "Nenhuma Ficha de Vistoria Localizada")
        return;
    }

    var dataToPost = JSON.stringify({ NoFichaVistoria: $('#txtNoFichaVistoria').val() });
    $.ajax({
        url: '/api/Multa/ProcuraFicha',
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
                    ViagemId = data.viagemid;
                    AppToast.show('Verde', data.message);
                    modalFicha.show();
                }
                else
                {
                    AppToast.show('Vermelho', data.message);
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "ProcuraFicha.success", error);
            }
        },
        error: function (err)
        {
            console.log(err);
            alert('something went wrong');
        }
    });
});

// Inicializar o modal Bootstrap 5
const modalFicha = new bootstrap.Modal(document.getElementById('modalFicha'), {
    keyboard: true,
    backdrop: 'static'
});

// Event listener para quando o modal for mostrado
document.getElementById('modalFicha').addEventListener('show.bs.modal', function ()
{
    try
    {
        $.ajaxSetup({ async: false });
        $.ajax({
            type: "get",
            url: "/api/Viagem/PegaFichaModal",
            data: { id: ViagemId },
            success: function (res)
            {
                const fv = $('#txtNoFichaVistoria').val();
                $('#imgViewer').removeAttr("src");
                if (res === false)
                {
                    $("#DynamicModalLabel").html("Infração sem Autuação digitalizada");
                    $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
                } else
                {
                    $("#DynamicModalLabel").html("Ficha de Vistoria Nº: <b>" + fv + "</b>");
                    $('#imgViewer').attr('src', "data:image/jpg;base64," + res);
                }
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "modalFicha.show", error);
    }
});

// Event listener para quando o modal for escondido
document.getElementById('modalFicha').addEventListener('hide.bs.modal', function ()
{
    try
    {
        $('#imgViewer').removeAttr("src");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "modalFicha.hide", error);
    }
});

