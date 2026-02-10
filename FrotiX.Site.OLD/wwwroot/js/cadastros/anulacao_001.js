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
        /* ⚡ FUNÇÃO: Deletar Anulação (Event Delegation Handler)
 *
 * [UI] [AJAX] [LOGICA]
 *
 * 📥 ENTRA: id (MovimentacaoId), context ("empenho" | "empenhoMulta")
 * 📤 SAIRÁ: POST /api/Empenho/DeleteMovimentacao → { success, message }
 * 🎯 MOTIVO: Remover anulações de sistema com confirmação do usuário
 *
 * ⬅️ CHAMADO POR: Click .btn-deleteanulacao (data-id, data-context)
 * ➡️ CHAMA: Alerta.Confirmar (SweetAlert modal), $.ajax, AppToast, location.reload
 *
 * ⚠️ IMPORTANTE: Diferencia entre empenho vs empenhoMulta via context attribute.
 *               Try-catch em TODOS os níveis (ready, click, .then, success, error).
 */
                $(document).on("click", ".btn-deleteanulacao", function () {
                    try
                    {
                        // [UI] Extrai atributos de dados do botão
                        var id = $(this).data("id");
                        var context = $(this).data("context");

                        // [LOGICA] Mapeia context para tipos de empenho corretos
                        var idEmpenho = context === "empenho" ? id : null;
                        var idEmpenhoMulta = context === "empenhoMulta" ? id : null;

                        // [UI] Exibe modal de confirmação com SweetAlert
                        Alerta.Confirmar(
                            "Você tem certeza que deseja apagar esta anulação?",
                            "Não será possível recuperar os dados eliminados!",
                            "Excluir",
                            "Cancelar",
                        ).then((willDelete) => {
                            try
                            {
                                if (willDelete) {
                                    // [AJAX] Prepara payload com um dos tipos de empenho
                                    var dataToPost = JSON.stringify({
                                        mEmpenho: idEmpenho ? { MovimentacaoId: idEmpenho } : null,
                                        mEmpenhoMulta: idEmpenhoMulta ? { MovimentacaoId: idEmpenhoMulta } : null
                                    });
                                    var url = "/api/Empenho/DeleteMovimentacao";

                                    // 📥 ENVIA: Payload JSON com ID da movimentação
                                    // 🎯 MOTIVO: Remover movimentação do banco
                                    $.ajax({
                                        url: url,
                                        type: "POST",
                                        data: dataToPost,
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (data) {
                                            try
                                            {
                                                // 📤 RECEBE: { success: boolean, message: string }
                                                if (data.success) {
                                                    // [UI] Toast de sucesso com mensagem do servidor
                                                    AppToast.show('Verde', data.message);
                                                    // [LOGICA] Recarrega DataTable sem resetar paginação
                                                    $("#tblGlosa").DataTable().ajax.reload(null, false);
                                                    // [UI] Recarrega página após sucesso
                                                    location.reload();
                                                } else {
                                                    // [UI] Toast de erro com mensagem do servidor
                                                    AppToast.show('Vermelho', data.message);
                                                }
                                            }
                                            catch (error)
                                            {
                                                Alerta.TratamentoErroComLinha(
                                                    "anulacao_001.js",
                                                    "$.ajax.success",
                                                    error,
                                                );
                                            }
                                        },
                                        error: function (err) {
                                            try
                                            {
                                                // [DEBUG] Log do erro para inspeção
                                                console.log(err);
                                                // [UI] Aviso simples de erro (poderia melhorar com AppToast)
                                                alert("something went wrong");
                                            }
                                            catch (error)
                                            {
                                                Alerta.TratamentoErroComLinha(
                                                    "anulacao_001.js",
                                                    "$.ajax.error",
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
                                    "anulacao_001.js",
                                    "Alerta.Confirmar.then",
                                    error,
                                );
                            }
                        });
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("anulacao_001.js", ".btn-deleteanulacao.click", error);
                    }
                });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("anulacao_001.js", "DOMContentLoaded", error);
    }
});
