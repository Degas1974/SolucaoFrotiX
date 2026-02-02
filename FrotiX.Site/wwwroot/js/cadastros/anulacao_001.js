/* ****************************************************************************************
 * ⚡ ARQUIVO: anulacao_001.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar exclusão de anulações de empenho e empenho-multa
 *                   com confirmação via modal e recarregamento de DataTable
 * 📥 ENTRADAS     : Clique em .btn-deleteanulacao (data-id, data-context),
 *                   Resposta Alerta.Confirmar (willDelete boolean)
 * 📤 SAÍDAS       : DELETE via AJAX para /api/Empenho/DeleteMovimentacao,
 *                   AppToast (Verde/Vermelho), reload DataTable e location,
 *                   Alerta.TratamentoErroComLinha em caso de erro
 * 🔗 CHAMADA POR  : Event handler .btn-deleteanulacao (páginas de empenho)
 * 🔄 CHAMA        : Alerta.Confirmar, $.ajax, AppToast.show, location.reload,
 *                   $("#tblGlosa").DataTable().ajax.reload, Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery 3.x, Alerta.js, AppToast (toast notifications)
 * 📝 OBSERVAÇÕES  : Context diferencia empenho vs empenhoMulta. Try-catch aninhado
 *                   em todos os níveis (ready, click, .then, success, error).
 **************************************************************************************** */

var GlosaTable;

$(document).ready(function () {
    try
    {
        $(document).on("click", ".btn-deleteanulacao", function () {
            try
            {
                var id = $(this).data("id");
                var context = $(this).data("context");

                var idEmpenho = context === "empenho" ? id : null;
                var idEmpenhoMulta = context === "empenhoMulta" ? id : null;

                Alerta.Confirmar(
                    "Você tem certeza que deseja apagar esta anulação?",
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
                                            $("#tblGlosa").DataTable().ajax.reload(null, false);
                                            location.reload();
                                        } else {
                                            AppToast.show('Vermelho', data.message);
                                        }
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "anulacao_<num>.js",
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
                                            "anulacao_<num>.js",
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
                            "anulacao_<num>.js",
                            "callback@swal.then#0",
                            error,
                        );
                    }
                });
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("anulacao_<num>.js", "callback@$.on#2", error);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("anulacao_<num>.js", "callback@$.ready#0", error);
    }
});
