/* ****************************************************************************************
 * ⚡ ARQUIVO: aporte_001.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar exclusão de aportes de empenho e empenho-multa
 *                   com confirmação via modal e recarregamento de DataTable
 * 📥 ENTRADAS     : Clique em .btn-deleteaporte (data-id, data-context),
 *                   Resposta Alerta.Confirmar (willDelete boolean)
 * 📤 SAÍDAS       : DELETE via AJAX para /api/Empenho/DeleteMovimentacao,
 *                   AppToast (Verde/Vermelho), reload DataTable e location,
 *                   Alerta.TratamentoErroComLinha em caso de erro
 * 🔗 CHAMADA POR  : Event handler .btn-deleteaporte (páginas de empenho)
 * 🔄 CHAMA        : Alerta.Confirmar, $.ajax, AppToast.show, location.reload,
 *                   $("#tblAporte").DataTable().ajax.reload, Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery 3.x, Alerta.js, AppToast (toast notifications)
 * 📝 OBSERVAÇÕES  : Context diferencia empenho vs empenhoMulta. Try-catch aninhado
 *                   em todos os níveis (ready, click, .then, success, error).
 **************************************************************************************** */

var AporteTable;

/****************************************************************************************
 * ⚡ FUNÇÃO: Inicialização e Handler de Exclusão de Aporte
 * --------------------------------------------------------------------------------------
 *
 * 🎯 OBJETIVO     : Gerenciar eventos de exclusão de aportes (empenho/empenho-multa)
 *                   com confirmação e recarregamento de dados
 *
 * 📥 ENTRADAS     : Evento document.ready, clique em .btn-deleteaporte
 *
 * 📤 SAÍDAS       : Confirmação via Alerta.Confirmar, POST /api/Empenho/DeleteMovimentacao,
 *                   Toast de sucesso/erro, reload DataTable
 *
 * ⬅️ CHAMADO POR  : $(document).ready (inicialização)
 *
 * ➡️ CHAMA        : Alerta.Confirmar, $.ajax, AppToast.show, Alerta.TratamentoErroComLinha
 *
 ****************************************************************************************/
$(document).ready(function () {
    try
    {
        /****** Handler delegado para clique em botão de remover aporte ******/
        $(document).on("click", ".btn-deleteaporte", function () {
            try
            {
                var id = $(this).data("id");
                var context = $(this).data("context");

                var idEmpenho = context === "empenho" ? id : null;
                var idEmpenhoMulta = context === "empenhoMulta" ? id : null;

                Alerta.Confirmar(
                    "Você tem certeza que deseja apagar este aporte?",
                    "Não será possível recuperar os dados eliminados!",
                    "Excluir",
                    "Cancelar",
                ).then((willDelete) => {
                    try
                    {
                        if (willDelete) {
                            var dataToPost = JSON.stringify({
                                mEmpenho: idEmpenho ? { MovimentacaoId: idEmpenho } : null,
                                mEmpenhoMulta: idEmpenhoMulta ? { MovimentacaoId: idEmpenhoMulta } : null
                            });
                            var url = "/api/Empenho/DeleteMovimentacao";
                            $.ajax({
                                url: url,
                                type: "POST",
                                data: dataToPost,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    try
                                    {
                                        if (data.success) {
                                            AppToast.show('Verde', data.message);
                                            $("#tblAporte").DataTable().ajax.reload(null, false);
                                            location.reload();
                                        } else {
                                            AppToast.show('Vermelho', data.message);
                                        }
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "aporte_<num>.js",
                                            "success",
                                            error,
                                        );
                                    }
                                },
                                error: function (err) {
                                    try
                                    {
                                        console.log(err);
                                        alert("something went wrong");
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "aporte_<num>.js",
                                            "error",
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha(
                            "aporte_<num>.js",
                            "callback@swal.then#0",
                            error,
                        );
                    }
                });
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("aporte_<num>.js", "callback@$.on#2", error);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("aporte_<num>.js", "callback@$.ready#0", error);
    }
});
