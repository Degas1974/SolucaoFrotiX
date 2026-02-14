/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemUpsert.js (4924 lines - CORE MODULE)
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Formulário complexo de cadastro/edição de viagens com 200+ funções. Gerencia CRUD completo,
 *    validações extensivas, integração com múltiplos dropdowns Syncfusion (veículo, motorista,
 *    combustível, kit, solicitante), cálculos automáticos (combustível, custos, distâncias),
 *    modal de finalização de viagem, modal KM ajuste, modal anexos (imagens/docs), sincronização
 *    com agendamentos, exportação Excel, impressão relatório, histórico alterações. Sistema de
 *    autosave rascunho (localStorage). Modal "Gravando viagem..." overlay durante POST.
 * 
 * 🔢 PARÂMETROS ENTRADA: viagemId (GUID URL ou input hidden), modo create/edit, dados form
 * 📤 SAÍDAS: POST /api/Viagens/Salvar, modais interativos, validações, toasts, redirecionamentos
 * 
 * 🔗 DEPENDÊNCIAS: jQuery, Syncfusion EJ2 (DropDownList/DatePicker/NumericTextBox/Grid/RTE),
 *    Kendo UI (DropDownList, Editor), Bootstrap 5, SweetAlert2, AppToast, Alerta.js,
 *    OcorrenciaViagem module
 * 
 * 📝 PRINCIPAIS CATEGORIAS (200+ funções organizadas em seções):
 *    • Inicialização DOMContentLoaded + carregamentos iniciais (20+ funções)
 *    • Dropdowns Syncfusion/Kendo (veículo/motorista/combustível/kit/solicitante/evento) - 30 funções
 *    • Validações campos obrigatórios + regras negócio - 25 funções
 *    • Cálculos automáticos (combustível inicial/final, custos, distância) - 15 funções
 *    • Modal Finalizaração Viagem (ocorrências, KM final, combustível) - 20 funções
 *    • Modal Anexos (upload imagens/documentos, preview, remoção) - 18 funções
 *    • Modal KM Ajuste (correção km_rodado quando 0) - 10 funções
 *    • Sincronização Agendamento (vincula viagem a agendamento) - 12 funções
 *    • Autosave Rascunho (localStorage backup a cada 30s) - 8 funções
 *    • Histórico Alterações (log mudanças, exibição timeline) - 10 funções
 *    • Exportação Excel/PDF, Impressão Relatório - 8 funções
 *    • CRUD Operations (save/update/delete/duplicate) - 15 funções
 *    • Helpers formatação/conversão/validação - 25+ funções
 * 
 * ⚠️ ARQUIVO CRÍTICO: 4924 linhas, núcleo módulo Viagens. Alterações requerem testes extensivos.
 * 
 * **************************************************************************************** */

// IIFE para não vazar variáveis no escopo global
(function ()
{
    try
    {
        var scriptTag = document.currentScript || document.scripts[document.scripts.length - 1];
        var __scriptName = scriptTag.src.split("/").pop();
        window.__scriptName = __scriptName;
    }
    catch (error)
    {
        //TratamentoErroComLinha("ViagemUpsert.js", "IIFE_ObterScriptName", error);
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "IIFE_ObterScriptName", error);
    }
})();

// ===============================================================================
// OVERLAY DE LOADING - GRAVANDO VIAGEM (Padrão FrotiX)
// ===============================================================================
function mostrarModalSalvando()
{
    try
    {
        const el = document.getElementById('loadingOverlaySalvando');
        if (el)
        {
            el.style.display = 'flex';
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "mostrarModalSalvando", error);
    }
}

function esconderModalSalvando()
{
    try
    {
        const el = document.getElementById('loadingOverlaySalvando');
        if (el)
        {
            el.style.display = 'none';
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "esconderModalSalvando", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: enviarFormularioViaAjax
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Submeter formulário de viagem via AJAX com suporte a uploads
 *
 * 📥 ENTRADAS     : handler [string] - "Create" ou "Edit"
 *                   id [GUID] - ID da viagem (opcional para Create)
 *
 * 📤 SAÍDAS       : POST /Viagens/Upsert com FormData (suporta arquivo)
 *                   Redirecionamento para /Viagens após sucesso
 *
 * ⬅️ CHAMADO POR  : Botões de salvar do formulário
 *
 * ➡️ CHAMA        : POST /Viagens/Upsert [AJAX]
 *                   AppToast.show() para notificações
 ****************************************************************************************/
function enviarFormularioViaAjax(handler, id)
{
    try
    {
        // [UI] Mostrar notificação de salvamento em progresso
        AppToast.show("Amarelo", "Salvando dados...", 2000);

        // [DADOS] Criar FormData com todos os campos do formulário
        const form = document.querySelector('form');
        const formData = new FormData(form);

        // [DADOS] Adicionar o Base64 da imagem se existir
        const base64 = $("#hiddenFoto").val();
        if (base64 && base64.length > 0)
        {
            console.log("Incluindo imagem:", base64.length, "caracteres");
            formData.append("FotoBase64", base64);

            // [DADOS] Remover do campo hidden para não duplicar
            $("#hiddenFoto").val("");
        }

        // [DADOS] Adicionar imagem existente se houver
        const fichaExistente = $("#hiddenFichaExistente").val();
        if (fichaExistente)
        {
            formData.append("FichaVistoriaExistente", fichaExistente);
        }

        // [LOGICA] Construir URL com handler e ID (se edit)
        let url = `/Viagens/Upsert?handler=${handler}`;
        if (id)
        {
            url += `&id=${id}`;
        }

        // [SEGURANCA] Obter token anti-CSRF do formulário
        const token = $('input[name="__RequestVerificationToken"]').val();

        /********************************************************************************
         * [AJAX] Endpoint: POST /Viagens/Upsert
         * ------------------------------------------------------------------------------
         * 📥 ENVIA        : FormData com todos os campos + FotoBase64 + FichaVistaExistente
         * 📤 RECEBE       : { success: bool, message: string }
         * 🎯 MOTIVO       : Salvar ou atualizar viagem completa com suporte a imagens
         ********************************************************************************/
        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            headers: {
                'RequestVerificationToken': token
            },
            success: function (response)
            {
                try
                {
                    // [VALIDACAO] Verificar sucesso da operação
                    if (response.success)
                    {
                        // [UI] Exibir mensagem de sucesso diferenciada (Create vs Edit)
                        AppToast.show("Verde", handler === "Edit" ? "Viagem atualizada com sucesso!" : "Viagem criada com sucesso!", 2000);

                        // [UI] Redirecionar após 2 segundos
                        setTimeout(function ()
                        {
                            window.location.href = '/Viagens';
                        }, 2000);
                    } else
                    {
                        // [UI] Exibir mensagem de erro do servidor
                        AppToast.show("Vermelho", response.message || "Erro ao salvar", 3000);
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "enviarFormularioViaAjax.ajax.success", error);
                }
            },
        error: function (xhr, status, error)
        {
            console.error('Erro AJAX:', status, error);
            console.error('Response:', xhr.responseText);

            // Tentar parsear mensagem de erro
            let mensagemErro = "Erro ao salvar. Tente novamente.";
            try
            {
                const resp = JSON.parse(xhr.responseText);
                if (resp.message) mensagemErro = resp.message;
            } catch (e)
            {
                // Se não for JSON, usar a mensagem padrío
            }

            AppToast.show("Vermelho", mensagemErro, 3000);
        }
    });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "enviarFormularioViaAjax", error);
    }
}

$(document).ready(function ()
{
    try
    {
        // Interceptar clique no botão escondido
        // Adicione isto APÓS a função do btnSubmit no seu JavaScript

        $("#btnEscondido").click(function (event)
        {
            try
            {
                event.preventDefault();

                const handler = $(this).data("handler");
                const id = $(this).data("id");

                // MOSTRAR MODAL DE LOADING
                mostrarModalSalvando();

                // Criar FormData com todos os campos do formulário
                const form = document.querySelector('form');
                const formData = new FormData(form);

                // Adicionar a imagem Base64 se existir
                const base64 = $("#hiddenFoto").val();
                if (base64 && base64.length > 0)
                {
                    console.log("Incluindo imagem com", base64.length, "caracteres");
                    formData.append("FotoBase64", base64);

                    // Limpar o campo para não enviar duplicado
                    $("#hiddenFoto").val("");
                }

                // Adicionar imagem existente se houver
                const fichaExistente = $("#hiddenFichaExistente").val();
                if (fichaExistente)
                {
                    formData.append("FichaVistoriaExistente", fichaExistente);
                }

                // Construir URL
                let url = `/Viagens/Upsert?handler=${handler}`;
                if (id)
                {
                    url += `&id=${id}`;
                }

                // Token anti-forgery
                const token = $('input[name="__RequestVerificationToken"]').val();

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    headers: {
                        'RequestVerificationToken': token
                    },
                    success: function (response)
                    {
                        // ESCONDER MODAL DE LOADING
                        esconderModalSalvando();

                        // Verificar se é uma resposta JSON ou HTML
                        if (typeof response === 'object' && response.success !== undefined)
                        {
                            // Resposta JSON
                            if (response.success)
                            {
                                AppToast.show("Verde",
                                    handler === "Edit" ? "Viagem atualizada com sucesso!" : "Viagem criada com sucesso!",
                                    2000);

                                setTimeout(function ()
                                {
                                    window.location.href = response.redirectUrl || '/Viagens';
                                }, 2000);
                            } else
                            {
                                AppToast.show("Vermelho", response.message || "Erro ao salvar", 3000);
                                $("#btnSubmit").prop("disabled", false);
                            }
                        } else
                        {
                            // Se retornou HTML (redirect), provavelmente deu certo
                            AppToast.show("Verde", "Viagem salva com sucesso!", 2000);

                            // Se retornou HTML de redirect, redirecionar
                            if (response.includes('window.location') || response.includes('/Viagens'))
                            {
                                window.location.href = '/Viagens';
                            } else
                            {
                                // Se retornou HTML da página, recarregar
                                document.open();
                                document.write(response);
                                document.close();
                            }
                        }
                    },
                    error: function (xhr, status, error)
                    {
                        // ESCONDER MODAL DE LOADING
                        esconderModalSalvando();

                        console.error('Erro AJAX:', status, error);
                        console.error('Response:', xhr.responseText);

                        AppToast.show("Vermelho", "Erro ao salvar. Tente novamente.", 3000);
                        $("#btnSubmit").prop("disabled", false);
                    }
                });
            } catch (error)
            {
                esconderModalSalvando();
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btnEscondido", error);
                $("#btnSubmit").prop("disabled", false);
            }
        });

        // Configurar o Toast
        toastObj = new ej.notifications.Toast({
            target: document.body,
            position: { X: 'Right', Y: 'Top' },
            animation: {
                show: { effect: 'SlideRightIn', duration: 600, easing: 'ease' },
                hide: { effect: 'SlideRightOut', duration: 600, easing: 'ease' }
            },
            showProgressBar: true,
            progressDirection: 'Ltr',
            timeOut: 2000,  // 2 segundos como solicitado
            extendedTimeout: 0,
            showCloseButton: true,
            newestOnTop: true
        });
        toastObj.appendTo('#toast_container');

        $("#modalEvento")
            .modal({
                keyboard: true,
                backdrop: false,
                show: false,
            })
            .on("hide.bs.modal", function ()
            {
                try
                {
                    let setores = getComboEJ2("ddtSetorRequisitanteEvento");
                    setores.value = "";
                    let requisitantes =
                        getComboEJ2("lstRequisitanteEvento");
                    requisitantes.value = "";
                    $("#txtNome").val("");
                    $("#txtDescricao").val("");
                    $("#txtDataInicial").val("");
                    $("#txtDataFinal").val("");
                    $(".modal-backdrop").remove();
                    $(document.body).removeClass("modal-open");
                }
                catch (error)
                {
                    TratamentoErroComLinha("ViagemUpsert.js", "hide.modalEvento", error);
                }
            });

        $("#modalRequisitante")
            .modal({
                keyboard: true,
                backdrop: "static",
                show: false,
            })
            .on("hide.bs.modal", function ()
            {
                try
                {
                    let setores = getComboEJ2("ddtSetorRequisitante");
                    setores.value = "";
                    $("#txtPonto").val("");
                    $("#txtNome").val("");
                    $("#txtRamal").val("");
                    $("#txtEmail").val("");
                    $(".modal-backdrop").remove();
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        "ViagemUpsert.js",
                        "hide.modalRequisitante",
                        error,
                    );
                }
            });

        $("#modalSetor")
            .modal({
                keyboard: true,
                backdrop: "static",
                show: false,
            })
            .on("hide.bs.modal", function ()
            {
                try
                {
                    let setores = getComboEJ2("ddtSetorPai");
                    setores.value = "";
                    $("#txtSigla").val("");
                    $("#txtNomeSetor").val("");
                    $("#txtRamalSetor").val("");
                }
                catch (error)
                {
                    TratamentoErroComLinha("ViagemUpsert.js", "hide.modalSetor", error);
                }
            });

        $("#txtFile").change(function (event)
        {
            try
            {
                let files = event.target.files;
                if (files.length === 0) return;
                let file = files[0];
                if (!file.type.startsWith("image/"))
                {
                    Alerta.Erro(
                        "Arquivo inválido",
                        "Por favor, selecione um arquivo de imagem válido.",
                    );
                    return;
                }
                $("#imgViewer").attr("src", window.URL.createObjectURL(file));
                $("#painelfundo").css({ "padding-bottom": "200px" });
            }
            catch (error)
            {
                TratamentoErroComLinha("ViagemUpsert.js", "change.txtFile", error);
            }
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "document.ready", error);
    }
});

//Para controlar a exibição de ToolTips
var CarregandoViagemBloqueada = false;
const MAX_KM_VALOR = 1000000;

/****************************************************************************************
 * ⚡ FUNÇÃO: focusout.txtKmInicial
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar quilometragem inicial invalida ou acima do limite.
 *                   [O QUE] Valida KM inicial contra KM atual e limite maximo.
 *                   [COMO] Converte valores, verifica limites e exibe alertas.
 *
 * 📥 ENTRADAS     : #txtKmInicial, #txtKmAtual
 *
 * 📤 SAÍDAS       : Ajustes nos campos e alertas de erro.
 *
 * ⬅️ CHAMADO POR  : Evento focusout do input #txtKmInicial
 *
 * ➡️ CHAMA        : validarKmAtualInicial(), Alerta.Erro()
 ****************************************************************************************/
$("#txtKmInicial").focusout(function ()
{
    try
    {
        // ✅ NOVO: Verificar se há veículo selecionado
        const cmbVeiculo = document.getElementById("cmbVeiculo");
        if (!$("#cmbVeiculo").data("kendoComboBox"))
        {
            $("#txtKmInicial").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Veículo não selecionado",
                'Selecione um <strong>veículo</strong> antes de preencher a quilometragem inicial.',
            );
            return;
        }

        const veiculoId = ($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null);
        if (!veiculoId || veiculoId === null || (Array.isArray(veiculoId) && veiculoId.length === 0))
        {
            $("#txtKmInicial").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Veículo não selecionado",
                'Selecione um <strong>veículo</strong> antes de preencher a quilometragem inicial.',
            );
            return;
        }

        const kmInicialStr = $("#txtKmInicial").val();
        const kmAtualStr = $("#txtKmAtual").val();

        if (!kmInicialStr || !kmAtualStr)
        {
            $("#txtKmPercorrido").val("");
            $("#txtKmFinal").prop("disabled", true); // ✅ NOVO: Desabilitar Km Final se Km Inicial vazio
            if (!kmAtualStr || kmAtualStr === "0" || kmAtualStr === 0)
            {
                $("#txtKmInicial").val("");
                $("#txtKmFinal").val("");
                $("#txtKmPercorrido").val("");
                Alerta.Erro(
                    "Erro na Quilometragem",
                    'A quilometragem <strong class="destaque-erro">Atual</strong> deve estar preenchida e ser maior que <strong class="destaque-erro">Zero</strong>!',
                );
            }
            return;
        }

        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
        const kmAtual = parseFloat(kmAtualStr.replace(",", "."));

        if (isNaN(kmInicial) || isNaN(kmAtual))
        {
            $("#txtKmPercorrido").val("");
            $("#txtKmFinal").prop("disabled", true); // ✅ NOVO: Desabilitar Km Final
            return;
        }

        if (kmInicial > MAX_KM_VALOR)
        {
            $("#txtKmInicial").val("");
            $("#txtKmPercorrido").val("");
            $("#txtKmFinal").prop("disabled", true); // ✅ NOVO: Desabilitar Km Final
            Alerta.Erro(
                "Erro na Quilometragem",
                "A quilometragem <strong>inicial</strong> nao pode ultrapassar <strong>1.000.000</strong>!",
            );
            return;
        }

        if (kmInicial < 0)
        {
            $("#txtKmInicial").val("");
            $("#txtKmPercorrido").val("");
            $("#txtKmFinal").prop("disabled", true); // ✅ NOVO: Desabilitar Km Final
            Alerta.Erro(
                "Erro na Quilometragem",
                "A quilometragem <strong>inicial</strong> deve ser maior que <strong>zero</strong>!",
            );
            return;
        }

        if (kmInicial < kmAtual)
        {
            $("#txtKmInicial").val("");
            $("#txtKmPercorrido").val("");
            $("#txtKmFinal").prop("disabled", true); // ✅ NOVO: Desabilitar Km Final
            Alerta.Erro(
                "Erro na Quilometragem",
                "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>!",
            );
            return;
        }

        // ✅ NOVO: Habilitar Km Final se Km Inicial válido
        $("#txtKmFinal").prop("disabled", false);

        validarKmAtualInicial();

        //calcularKmPercorrido
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtKmInicial", error);
    }
});

/****************************************************************************************
 * ⚡ FUNÇÃO: focusout.txtKmFinal
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar quilometragem final invalida ou acima do limite.
 *                   [O QUE] Valida KM final contra KM inicial e limite maximo.
 *                   [COMO] Converte valores, verifica limites e exibe alertas.
 *
 * 📥 ENTRADAS     : #txtKmInicial, #txtKmFinal
 *
 * 📤 SAÍDAS       : Ajustes nos campos e alertas de erro.
 *
 * ⬅️ CHAMADO POR  : Evento focusout do input #txtKmFinal
 *
 * ➡️ CHAMA        : calcularKmPercorrido(), Alerta.Erro()
 ****************************************************************************************/
// txtKmFinal - VALIDAÇÃO IA
$("#txtKmFinal").focusout(async function ()
{
    try
    {
        // ✅ NOVO: Verificar se há veículo selecionado
        const cmbVeiculo = document.getElementById("cmbVeiculo");
        if (!$("#cmbVeiculo").data("kendoComboBox"))
        {
            $("#txtKmFinal").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Veículo não selecionado",
                'Selecione um <strong>veículo</strong> antes de preencher a quilometragem final.',
            );
            return;
        }

        const veiculoId = ($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null);
        if (!veiculoId || veiculoId === null || (Array.isArray(veiculoId) && veiculoId.length === 0))
        {
            $("#txtKmFinal").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Veículo não selecionado",
                'Selecione um <strong>veículo</strong> antes de preencher a quilometragem final.',
            );
            return;
        }

        const kmInicialStr = $("#txtKmInicial").val();
        const kmFinalStr = $("#txtKmFinal").val();

        // ✅ REFORÇADO: Validar se Km Inicial está preenchido
        if (
            (kmInicialStr === "" || kmInicialStr === null) &&
            kmFinalStr != "" &&
            kmFinalStr != null
        )
        {
            $("#txtKmFinal").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Erro na Quilometragem",
                "A quilometragem <strong>Final</strong> deve ser preenchida somente após a <strong>Inicial</strong>!",
            );
            return;
        }

        if (!kmInicialStr || !kmFinalStr)
        {
            $("#txtKmPercorrido").val("");
            return;
        }

        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
        const kmFinal = parseFloat(kmFinalStr.replace(",", "."));

        if (isNaN(kmInicial) || isNaN(kmFinal))
        {
            $("#txtKmPercorrido").val("");
            return;
        }

        if (kmFinal > MAX_KM_VALOR)
        {
            $("#txtKmFinal").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Erro na Quilometragem",
                "A quilometragem <strong>final</strong> nao pode ultrapassar <strong>1.000.000</strong>!",
            );
            return;
        }

        if (kmFinal < kmInicial)
        {
            $("#txtKmFinal").val("");
            $("#txtKmPercorrido").val("");
            Alerta.Erro(
                "Erro na Quilometragem",
                "A quilometragem final deve ser maior que a inicial!",
            );
            return;
        }

        const kmPercorrido = Math.round(kmFinal - kmInicial);
        $("#txtKmPercorrido").val(kmPercorrido);

        calcularKmPercorrido();

        // VALIDAÇÃO IA: Análise de quilometragem (se disponível)
        if (typeof ValidadorFinalizacaoIA !== 'undefined')
        {
            const veiculoId = ($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null) || '';

            if (veiculoId && kmInicial > 0 && kmFinal > 0)
            {
                const validador = ValidadorFinalizacaoIA.obterInstancia();
                const dadosKm = {
                    kmInicial: kmInicial,
                    kmFinal: kmFinal,
                    veiculoId: veiculoId
                };

                const resultadoKm = await validador.analisarKm(dadosKm);
                if (!resultadoKm.valido)
                {
                    if (resultadoKm.nivel === 'erro')
                    {
                        await Alerta.Erro(resultadoKm.titulo, resultadoKm.mensagem);
                        $("#txtKmFinal").val("");
                        $("#txtKmPercorrido").val("");
                        return;
                    }
                    else if (resultadoKm.nivel === 'aviso')
                    {
                        const confirma = await Alerta.ValidacaoIAConfirmar(
                            resultadoKm.titulo,
                            resultadoKm.mensagem,
                            "Manter KM",
                            "Corrigir"
                        );
                        if (!confirma)
                        {
                            $("#txtKmFinal").val("");
                            $("#txtKmPercorrido").val("");
                            return;
                        }
                    }
                }
            }
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtKmFinal", error);
    }
});

/****************************************************************************************
 * ⚡ EVENTO: input.txtKmInicial
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Habilitar/desabilitar campo Km Final conforme Km Inicial é digitado
 *
 * 📥 ENTRADAS     : Valor de #txtKmInicial
 *
 * 📤 SAÍDAS       : Habilita/desabilita #txtKmFinal, zera #txtKmPercorrido se inválido
 ****************************************************************************************/
$("#txtKmInicial").on("input", function ()
{
    try
    {
        const kmInicialStr = $("#txtKmInicial").val();

        // Se vazio ou inválido, desabilitar Km Final e zerar Km Percorrido
        if (!kmInicialStr || kmInicialStr.trim() === "")
        {
            $("#txtKmFinal").prop("disabled", true);
            $("#txtKmPercorrido").val("");
            return;
        }

        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));

        // Se não é número válido, desabilitar Km Final e zerar Km Percorrido
        if (isNaN(kmInicial) || kmInicial <= 0)
        {
            $("#txtKmFinal").prop("disabled", true);
            $("#txtKmPercorrido").val("");
            return;
        }

        // Se valor válido, habilitar Km Final
        $("#txtKmFinal").prop("disabled", false);

        // Recalcular Km Percorrido se Km Final já estiver preenchido
        calcularKmPercorrido();
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "input.txtKmInicial", error);
    }
});

/****************************************************************************************
 * ⚡ EVENTO: input.txtKmFinal
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Recalcular Km Percorrido conforme Km Final é digitado
 *
 * 📥 ENTRADAS     : Valores de #txtKmInicial e #txtKmFinal
 *
 * 📤 SAÍDAS       : Atualiza ou zera #txtKmPercorrido
 ****************************************************************************************/
$("#txtKmFinal").on("input", function ()
{
    try
    {
        const kmInicialStr = $("#txtKmInicial").val();
        const kmFinalStr = $("#txtKmFinal").val();

        // Se qualquer campo vazio, zerar Km Percorrido
        if (!kmInicialStr || !kmFinalStr || kmInicialStr.trim() === "" || kmFinalStr.trim() === "")
        {
            $("#txtKmPercorrido").val("");
            return;
        }

        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
        const kmFinal = parseFloat(kmFinalStr.replace(",", "."));

        // Se não são números válidos, zerar Km Percorrido
        if (isNaN(kmInicial) || isNaN(kmFinal))
        {
            $("#txtKmPercorrido").val("");
            return;
        }

        // Recalcular Km Percorrido
        calcularKmPercorrido();
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "input.txtKmFinal", error);
    }
});

/****************************************************************************************
 * ⚡ INICIALIZAÇÃO: Estado inicial dos campos de Km
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Configurar estado correto dos campos ao carregar a página (create/edit)
 ****************************************************************************************/
$(document).ready(function ()
{
    try
    {
        // Verificar se há veículo selecionado
        const cmbVeiculo = document.getElementById("cmbVeiculo");
        let veiculoSelecionado = false;

        if ($("#cmbVeiculo").data("kendoComboBox"))
        {
            const veiculoId = ($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null);
            veiculoSelecionado = veiculoId && veiculoId !== null && (!Array.isArray(veiculoId) || veiculoId.length > 0);
        }

        // Se não há veículo, desabilitar ambos os campos
        if (!veiculoSelecionado)
        {
            $("#txtKmInicial").prop("disabled", true);
            $("#txtKmFinal").prop("disabled", true);
        }
        else
        {
            // Se há veículo, verificar Km Inicial para habilitar/desabilitar Km Final
            const kmInicialStr = $("#txtKmInicial").val();

            $("#txtKmInicial").prop("disabled", false);

            if (!kmInicialStr || kmInicialStr.trim() === "")
            {
                $("#txtKmFinal").prop("disabled", true);
            }
            else
            {
                const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
                if (isNaN(kmInicial) || kmInicial <= 0)
                {
                    $("#txtKmFinal").prop("disabled", true);
                }
                else
                {
                    $("#txtKmFinal").prop("disabled", false);
                }
            }
        }

        console.log("[ViagemUpsert] Estado inicial dos campos de Km configurado");
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "document.ready.inicializacaoKm", error);
    }
});

$("#txtDataInicial").focusout(function ()
{
    try
    {
        // [KENDO] Obter valores via API Kendo
        const dataInicial = window.getKendoDateValue("txtDataInicial");
        const dataFinal = window.getKendoDateValue("txtDataFinal");

        // [VALIDACAO] Verificar se a data é válida
        if (!dataInicial || isNaN(dataInicial.getTime()))
        {
            Alerta.Erro("Erro na Data", "Data Inicial inválida!");
            window.setKendoDateValue("txtDataInicial", null);
            return;
        }

        // [REGRA] Data Inicial nao pode ser superior a data atual
        const hoje = new Date();
        hoje.setHours(0, 0, 0, 0);
        const dataInicialNormalizada = new Date(dataInicial);
        dataInicialNormalizada.setHours(0, 0, 0, 0);
        if (dataInicialNormalizada > hoje)
        {
            Alerta.Warning("Data Inicial invalida", "A Data Inicial nao pode ser superior a data atual!", "OK");
            window.setKendoDateValue("txtDataInicial", null);
            return;
        }

        // [VALIDACAO] Comparar com Data Final (se preenchida)
        if (dataFinal)
        {
            if (dataFinal < dataInicial)
            {
                window.setKendoDateValue("txtDataInicial", null);
                $("#txtDuracao").val("");
                Alerta.Erro("Erro na Data", "A Data Inicial deve ser menor ou igual à Data Final!");
                return;
            }

            // Formatar para moment (compatibilidade com validarDatasInicialFinal)
            const strDataInicial = moment(dataInicial).format("DD/MM/YYYY");
            const strDataFinal = moment(dataFinal).format("DD/MM/YYYY");
            validarDatasInicialFinal(strDataInicial, strDataFinal);

            // [REGRA] Se mesma data, Hora Fim >= Hora Início
            const sameDay = dataInicial.getFullYear() === dataFinal.getFullYear() &&
                           dataInicial.getMonth() === dataFinal.getMonth() &&
                           dataInicial.getDate() === dataFinal.getDate();
            
            if (sameDay)
            {
                const horaInicial = window.getKendoTimeValue("txtHoraInicial");
                const horaFinal = window.getKendoTimeValue("txtHoraFinal");

                if (horaInicial && horaFinal)
                {
                    const [hI, mI] = horaInicial.split(":").map(Number);
                    const [hF, mF] = horaFinal.split(":").map(Number);
                    const minIni = hI * 60 + mI;
                    const minFin = hF * 60 + mF;

                    if (minFin <= minIni)
                    {
                        window.setKendoTimeValue("txtHoraFinal", null);
                        $("#txtDuracao").val("");
                        Alerta.Erro(
                            "Erro na Hora",
                            "A Hora Início deve ser menor que a Hora Fim quando as datas forem iguais!"
                        );
                        return;
                    }
                }
            }
        }

        calcularDuracaoViagem();
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtDataInicial", error);
    }
});

//================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: focusout.txtHoraInicial
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Garantir que o conjunto Data Inicial + Hora Início não
 *                   represente um momento futuro, e que a Hora Início seja consistente
 *                   com a Hora Fim quando as datas forem iguais.
 *                   [O QUE] Valida hora de início contra momento atual e hora fim.
 *                   [COMO] Compara datetime montado com Date() atual; se mesma data,
 *                   compara minutos totais de início vs fim.
 *
 * 📥 ENTRADAS     : Valores de #txtDataInicial, #txtHoraInicial, #txtDataFinal, #txtHoraFinal
 *
 * 📤 SAÍDAS       : Limpa campo e exibe erro se inválido; recalcula duração se válido.
 *
 * ⬅️ CHAMADO POR  : Evento focusout do input #txtHoraInicial
 *
 * ➡️ CHAMA        : calcularDuracaoViagem() [ViagemUpsert.js]
 *                   Alerta.Erro() [alerta.js]
 ****************************************************************************************/
$("#txtHoraInicial").focusout(function ()
{
    try
    {
        // [KENDO] Obter valores via API Kendo
        const dataInicial = window.getKendoDateValue("txtDataInicial");
        const horaInicial = window.getKendoTimeValue("txtHoraInicial");

        if (!dataInicial || !horaInicial) return;

        // [REGRA] Conjunto Data Inicial + Hora Início não pode ser superior ao momento atual
        const agora = new Date();
        const [horas, minutos] = horaInicial.split(":").map(Number);
        const dataHoraInicio = new Date(dataInicial);
        dataHoraInicio.setHours(horas, minutos, 0, 0);

        if (dataHoraInicio > agora)
        {
            Alerta.Erro(
                "Erro na Hora",
                "O conjunto Data Inicial + Hora Início não pode ser superior ao momento atual!"
            );
            window.setKendoTimeValue("txtHoraInicial", null);
            $("#txtDuracao").val("");
            return;
        }

        // [REGRA] Se mesma data que Data Final, Hora Fim deve ser >= Hora Início
        const dataFinal = window.getKendoDateValue("txtDataFinal");
        const horaFinal = window.getKendoTimeValue("txtHoraFinal");

        if (dataFinal && horaFinal)
        {
            const sameDay = dataInicial.getFullYear() === dataFinal.getFullYear() &&
                           dataInicial.getMonth() === dataFinal.getMonth() &&
                           dataInicial.getDate() === dataFinal.getDate();
            
            if (sameDay)
            {
                const [hI, mI] = horaInicial.split(":").map(Number);
                const [hF, mF] = horaFinal.split(":").map(Number);
                const minIni = hI * 60 + mI;
                const minFin = hF * 60 + mF;

                if (minFin <= minIni)
                {
                    Alerta.Erro(
                        "Erro na Hora",
                        "A Hora Início não pode ser maior ou igual à Hora Fim quando as datas forem iguais!"
                    );
                    window.setKendoTimeValue("txtHoraInicial", null);
                    $("#txtDuracao").val("");
                    return;
                }
            }
        }

        calcularDuracaoViagem();
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtHoraInicial", error);
    }
});

let evitandoLoop = false;
let validandoDataFinal = false;
let atualizandoDataFinal = false;
let validandoHoraFinal = false;

/****************************************************************************************
 * ⚡ FUNÇÃO: validarDataFinal
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUE] Garantir coerencia entre Data Inicial e Data Final, com
 *                   confirmacao quando o intervalo e muito grande.
 *                   [O QUE] Valida formato, ordem, hora final quando mesma data e
 *                   aciona validacao IA quando disponivel.
 *                   [COMO] Obtem valores Kendo, compara datas/horas e dispara alertas.
 *
 * 📥 ENTRADAS     : Valores atuais de #txtDataInicial, #txtDataFinal, #txtHoraInicial,
 *                   #txtHoraFinal
 *
 * 📤 SAÍDAS       : Ajustes nos campos, alertas e recálculo de duração
 *
 * ⬅️ CHAMADO POR  : Eventos change/focusout de #txtDataFinal
 *
 * ➡️ CHAMA        : validarDatasInicialFinal(), calcularDuracaoViagem(),
 *                   Alerta.*(), ValidadorFinalizacaoIA.* (se disponível)
 ****************************************************************************************/
async function validarDataFinal()
{
    try
    {
        if (validandoDataFinal || atualizandoDataFinal) return;
        validandoDataFinal = true;

        if (evitandoLoop) return;

        // [KENDO] Obter valores via API Kendo
        const dataFinal = window.getKendoDateValue("txtDataFinal");
        const dataInicial = window.getKendoDateValue("txtDataInicial");

        if (!dataFinal)
        {
            return;
        }

        // [VALIDACAO] Verificar se a data é válida
        if (isNaN(dataFinal.getTime()))
        {
            Alerta.Erro("Erro na Data", "Data Final inválida!");
            atualizandoDataFinal = true;
            window.setKendoDateValue("txtDataFinal", null);
            atualizandoDataFinal = false;
            return;
        }

        // [REGRA] Data Final nao pode ser superior a data atual
        const hoje = new Date();
        hoje.setHours(0, 0, 0, 0);
        const dataFinalNormalizada = new Date(dataFinal);
        dataFinalNormalizada.setHours(0, 0, 0, 0);
        if (dataFinalNormalizada > hoje)
        {
            Alerta.Warning("Data Final invalida", "A Data Final nao pode ser superior a data atual!", "OK");
            atualizandoDataFinal = true;
            window.setKendoDateValue("txtDataFinal", null);
            atualizandoDataFinal = false;
            return;
        }

        // [VALIDACAO IA] Análise adicional via ValidadorFinalizacaoIA (se disponível)
        if (typeof ValidadorFinalizacaoIA !== 'undefined' && typeof ValidadorFinalizacaoIA.obterInstancia === "function")
        {
            const validador = ValidadorFinalizacaoIA.obterInstancia();
            const strDataFinal = moment(dataFinal).format("YYYY-MM-DD");
            const resultadoDataFutura = await validador.validarDataNaoFutura(strDataFinal);
            if (!resultadoDataFutura.valido)
            {
                await Alerta.Erro(resultadoDataFutura.titulo, resultadoDataFutura.mensagem);
                atualizandoDataFinal = true;
                window.setKendoDateValue("txtDataFinal", null);
                atualizandoDataFinal = false;
                return;
            }
        }
        else if (typeof ValidadorFinalizacaoIA !== 'undefined')
        {
            if (window.console && typeof console.warn === "function")
            {
                console.warn("ValidadorFinalizacaoIA.obterInstancia indisponivel. Validacao IA ignorada.");
            }
        }

        if (!dataInicial) return;

        if (dataFinal < dataInicial)
        {
            atualizandoDataFinal = true;
            window.setKendoDateValue("txtDataFinal", null);
            atualizandoDataFinal = false;
            $("#txtDuracao").val("");
            Alerta.Erro("Erro na Data", "A data final deve ser maior ou igual que a inicial!");
            return;
        }

        // Formatar para moment (compatibilidade com validarDatasInicialFinal)
        const strDataInicial = moment(dataInicial).format("DD/MM/YYYY");
        const strDataFinal = moment(dataFinal).format("DD/MM/YYYY");
        atualizandoDataFinal = true;
        const confirmouIntervalo = await validarDatasInicialFinal(strDataInicial, strDataFinal);
        atualizandoDataFinal = false;
        if (!confirmouIntervalo)
        {
            return;
        }

        const sameDay = dataInicial.getFullYear() === dataFinal.getFullYear() &&
                       dataInicial.getMonth() === dataFinal.getMonth() &&
                       dataInicial.getDate() === dataFinal.getDate();
        
        if (sameDay)
        {
            const horaInicial = window.getKendoTimeValue("txtHoraInicial");
            const horaFinal = window.getKendoTimeValue("txtHoraFinal");

            if (!horaInicial || !horaFinal) return;

            const [hI, mI] = horaInicial.split(":").map(Number);
            const [hF, mF] = horaFinal.split(":").map(Number);
            const minIni = hI * 60 + mI;
            const minFin = hF * 60 + mF;

            if (minFin <= minIni)
            {
                window.setKendoTimeValue("txtHoraFinal", null);
                $("#txtDuracao").val("");
                Alerta.Erro(
                    "Erro na Hora",
                    "A hora final deve ser maior ou igual que a inicial quando as datas forem iguais!",
                );
                return;
            }
        }

        calcularDuracaoViagem();

        // VALIDAÇÃO IA: Análise de duração (se disponível)
        if (typeof ValidadorFinalizacaoIA !== 'undefined' && typeof ValidadorFinalizacaoIA.obterInstancia === "function")
        {
            const horaInicial = window.getKendoTimeValue("txtHoraInicial");
            const horaFinal = window.getKendoTimeValue("txtHoraFinal");

            if (dataInicial && horaInicial && horaFinal)
            {
                const validador = ValidadorFinalizacaoIA.obterInstancia();
                const dadosDatas = {
                    dataInicial: moment(dataInicial).format("YYYY-MM-DD"),
                    horaInicial: horaInicial,
                    dataFinal: moment(dataFinal).format("YYYY-MM-DD"),
                    horaFinal: horaFinal
                };

                const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
                if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
                {
                    const confirma = await Alerta.ValidacaoIAConfirmar(
                        resultadoDatas.titulo,
                        resultadoDatas.mensagem,
                        "Manter Data",
                        "Corrigir"
                    );
                    if (!confirma)
                    {
                        atualizandoDataFinal = true;
                        window.setKendoDateValue("txtDataFinal", null);
                        atualizandoDataFinal = false;
                        return;
                    }
                }
            }
        }
        else if (typeof ValidadorFinalizacaoIA !== 'undefined')
        {
            if (window.console && typeof console.warn === "function")
            {
                console.warn("ValidadorFinalizacaoIA.obterInstancia indisponivel. Validacao IA ignorada.");
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "validarDataFinal", error);
    }
    finally
    {
        validandoDataFinal = false;
    }
}

// txtDataFinal - VALIDACAO IMEDIATA
$("#txtDataFinal").change(function ()
{
    try
    {
        if (atualizandoDataFinal) return;
        validarDataFinal();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "change.txtDataFinal", error);
    }
});

//================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: validarHoraFinal
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUE] Validar Hora Final imediatamente e recalcular duracao.
 *                   [O QUE] Verifica ordem das horas e roda validacao IA quando disponivel.
 *                   [COMO] Usa valores Kendo, compara minutos e recalcula duracao.
 *
 * 📥 ENTRADAS     : Valores atuais de #txtDataInicial, #txtDataFinal, #txtHoraInicial,
 *                   #txtHoraFinal
 *
 * 📤 SAÍDAS       : Ajustes nos campos, alertas e recálculo de duração
 *
 * ⬅️ CHAMADO POR  : Eventos change/focusout de #txtHoraFinal
 *
 * ➡️ CHAMA        : calcularDuracaoViagem(), Alerta.*(), ValidadorFinalizacaoIA.*
 ****************************************************************************************/
async function validarHoraFinal()
{
    try
    {
        if (validandoHoraFinal) return;
        validandoHoraFinal = true;

        // [KENDO] Obter valores via API Kendo
        const dataFinal = window.getKendoDateValue("txtDataFinal");
        const horaFinal = window.getKendoTimeValue("txtHoraFinal");
        
        if (!dataFinal && horaFinal)
        {
            Alerta.Erro(
                "Erro na Hora",
                "A hora final só pode ser preenchida depois de Data Final!",
            );
            window.setKendoTimeValue("txtHoraFinal", null);
            $("#txtDuracao").val("");
            return;
        }

        const dataInicial = window.getKendoDateValue("txtDataInicial");
        const horaInicial = window.getKendoTimeValue("txtHoraInicial");

        if (!dataInicial || !dataFinal || !horaInicial || !horaFinal) return;

        const sameDay = dataInicial.getFullYear() === dataFinal.getFullYear() &&
                       dataInicial.getMonth() === dataFinal.getMonth() &&
                       dataInicial.getDate() === dataFinal.getDate();

        if (sameDay)
        {
            const [hI, mI] = horaInicial.split(":").map(Number);
            const [hF, mF] = horaFinal.split(":").map(Number);
            const minIni = hI * 60 + mI;
            const minFin = hF * 60 + mF;

            if (minFin <= minIni)
            {
                $("#txtHoraFinal").val("");
                $("#txtDuracao").val("");
                Alerta.Erro(
                    "Erro na Hora",
                    "A hora final deve ser maior que a inicial quando as datas forem iguais!",
                );
                return;
            }
        }

        calcularDuracaoViagem();

        // VALIDAÇÃO IA: Análise de duração (se disponível)
        if (typeof ValidadorFinalizacaoIA !== 'undefined' && typeof ValidadorFinalizacaoIA.obterInstancia === "function")
        {
            const validador = ValidadorFinalizacaoIA.obterInstancia();
            const dataInicialStr = moment(dataInicial).format("YYYY-MM-DD");
            const dataFinalStr = moment(dataFinal).format("YYYY-MM-DD");
            const dadosDatas = {
                dataInicial: dataInicialStr,
                horaInicial: horaInicial,
                dataFinal: dataFinalStr,
                horaFinal: horaFinal
            };

            const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
            if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
            {
                const confirma = await Alerta.ValidacaoIAConfirmar(
                    resultadoDatas.titulo,
                    resultadoDatas.mensagem,
                    "Manter Hora",
                    "Corrigir"
                );
                if (!confirma)
                {
                    $("#txtHoraFinal").val("");
                    $("#txtDuracao").val("");
                    return;
                }
            }
        }
        else if (typeof ValidadorFinalizacaoIA !== 'undefined')
        {
            if (window.console && typeof console.warn === "function")
            {
                console.warn("ValidadorFinalizacaoIA.obterInstancia indisponivel. Validacao IA ignorada.");
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "validarHoraFinal", error);
    }
    finally
    {
        validandoHoraFinal = false;
    }
}

// txtHoraFinal - CALCULO IMEDIATO
$("#txtHoraFinal").change(function ()
{
    try
    {
        validarHoraFinal();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "change.txtHoraFinal", error);
    }
});


/****************************************************************************************
 * ⚡ FUNÇÃO: PreencheListaEventos
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Atualizar a lista de eventos após inclusão/alteração,
 *                   mantendo o dropdown sincronizado com o backend.
 *                   [O QUE] Recarrega o dataSource do dropdown Telerik de eventos.
 *                   [COMO] Consome o handler AJAX da página e atualiza o Kendo DropDownList.
 *
 * 📥 ENTRADAS     : eventoSelecionadoId [string] - ID do evento a selecionar (opcional).
 *
 * 📤 SAÍDAS       : Dropdown atualizado e detalhes do evento preenchidos.
 *
 * ⬅️ CHAMADO POR  : click #btnInserirEvento (após criar evento).
 *
 * ➡️ CHAMA        : atualizarDetalhesEventoSelecionado(), limparDetalhesEventoSelecionado().
 *
 * 📝 OBSERVAÇÕES  : Usa Kendo DropDownList e mantém compatibilidade com UI atual.
 ****************************************************************************************/
function PreencheListaEventos(eventoSelecionadoId)
{
    try
    {
        const ddlEvento = $("#ddlEvento").data("kendoDropDownList");
        if (!ddlEvento) return;

        /********************************************************************************
         * [AJAX] Endpoint: GET /Viagens/Upsert?handler=AJAXPreencheListaEventos
         * ------------------------------------------------------------------------------
         * 📥 ENVIA        : Nenhum parâmetro
         * 📤 RECEBE       : { data: [{ eventoId, nome }] }
         * 🎯 MOTIVO       : Recarregar lista de eventos após inclusão no modal
         ********************************************************************************/
        if (typeof FrotiXApi !== "undefined" && FrotiXApi?.get)
        {
            FrotiXApi.get("/Viagens/Upsert?handler=AJAXPreencheListaEventos")
                .then(function (response)
                {
                    try
                    {
                        const listaEventos = response?.data || [];
                        ddlEvento.setDataSource(new kendo.data.DataSource({ data: listaEventos }));
                        ddlEvento.dataSource.read();
                        ddlEvento.refresh();

                        if (eventoSelecionadoId)
                        {
                            ddlEvento.value(eventoSelecionadoId);
                            atualizarDetalhesEventoSelecionado(eventoSelecionadoId);
                        }
                        else
                        {
                            limparDetalhesEventoSelecionado();
                        }
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaEventos.then", error);
                    }
                })
                .catch(function (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaEventos.catch", error);
                });
        }
        else
        {
            $.ajax({
                type: "GET",
                url: "/Viagens/Upsert?handler=AJAXPreencheListaEventos",
                success: function (response)
                {
                    try
                    {
                        const listaEventos = response?.data || [];
                        ddlEvento.setDataSource(new kendo.data.DataSource({ data: listaEventos }));
                        ddlEvento.dataSource.read();
                        ddlEvento.refresh();

                        if (eventoSelecionadoId)
                        {
                            ddlEvento.value(eventoSelecionadoId);
                            atualizarDetalhesEventoSelecionado(eventoSelecionadoId);
                        }
                        else
                        {
                            limparDetalhesEventoSelecionado();
                        }
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaEventos.ajax.success", error);
                    }
                },
                error: function (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaEventos.ajax.error", error);
                }
            });
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaEventos", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: onEventoSelecionado
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Atualizar os dados basicos do evento ao selecionar na lista.
 *                   [O QUE] Dispara carregamento de detalhes ou limpa campos quando vazio.
 *                   [COMO] Le valor do Kendo DropDownList e chama helper de detalhe.
 *
 * 📥 ENTRADAS     : e [Object] - Evento do Kendo DropDownList.
 *
 * 📤 SAÍDAS       : Campos de detalhes preenchidos/limpos.
 *
 * ⬅️ CHAMADO POR  : Change do dropdown `ddlEvento`.
 *
 * ➡️ CHAMA        : atualizarDetalhesEventoSelecionado(), limparDetalhesEventoSelecionado().
 ****************************************************************************************/
function onEventoSelecionado(e)
{
    try
    {
        const ddlEvento = $("#ddlEvento").data("kendoDropDownList");
        const eventoId = e?.sender?.value() || ddlEvento?.value();

        if (eventoId)
        {
            atualizarDetalhesEventoSelecionado(eventoId);
        }
        else
        {
            limparDetalhesEventoSelecionado();
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "onEventoSelecionado", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: atualizarDetalhesEventoSelecionado
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Exibir dados basicos do evento escolhido para padronizar UX.
 *                   [O QUE] Busca detalhes do evento e preenche Data Inicio/Fim/Qtd.
 *                   [COMO] Consome o endpoint `api/ViagemEvento/ObterPorId`.
 *
 * 📥 ENTRADAS     : eventoId [string] - ID do evento selecionado.
 *
 * 📤 SAÍDAS       : Inputs de detalhe preenchidos.
 *
 * ⬅️ CHAMADO POR  : onEventoSelecionado(), ExibeViagem().
 *
 * ➡️ CHAMA        : limparDetalhesEventoSelecionado() quando vazio.
 ****************************************************************************************/
function atualizarDetalhesEventoSelecionado(eventoId)
{
    try
    {
        if (!eventoId)
        {
            limparDetalhesEventoSelecionado();
            return;
        }

        /********************************************************************************
         * [AJAX] Endpoint: GET /api/ViagemEvento/ObterPorId?id={eventoId}
         * ------------------------------------------------------------------------------
         * 📥 ENVIA        : eventoId (query param)
         * 📤 RECEBE       : { success, data: { DataInicial, DataFinal, QtdParticipantes } }
         * 🎯 MOTIVO       : Preencher dados basicos do evento na tela de Viagem
         ********************************************************************************/
        if (typeof FrotiXApi !== "undefined" && FrotiXApi?.get)
        {
            FrotiXApi.get("/api/ViagemEvento/ObterPorId?id=" + eventoId)
                .then(function (response)
                {
                    try
                    {
                        if (!response?.success)
                        {
                            limparDetalhesEventoSelecionado();
                            return;
                        }

                        const data = response.data || {};
                        const dataInicialRaw = data.dataInicial ?? data.DataInicial ?? null;
                        const dataFinalRaw = data.dataFinal ?? data.DataFinal ?? null;
                        const qtdParticipantes = data.qtdParticipantes ?? data.QtdParticipantes ?? "";

                        const dataInicio = dataInicialRaw ? moment(dataInicialRaw).format("DD/MM/YYYY") : "";
                        const dataFim = dataFinalRaw ? moment(dataFinalRaw).format("DD/MM/YYYY") : "";

                        $("#txtEventoDataInicio").val(dataInicio);
                        $("#txtEventoDataFim").val(dataFim);
                        $("#txtEventoQtdParticipantes").val(qtdParticipantes);
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarDetalhesEventoSelecionado.then", error);
                    }
                })
                .catch(function (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarDetalhesEventoSelecionado.catch", error);
                });
        }
        else
        {
            $.ajax({
                type: "GET",
                url: "/api/ViagemEvento/ObterPorId",
                data: { id: eventoId },
                success: function (response)
                {
                    try
                    {
                        if (!response?.success)
                        {
                            limparDetalhesEventoSelecionado();
                            return;
                        }

                        const data = response.data || {};
                        const dataInicialRaw = data.dataInicial ?? data.DataInicial ?? null;
                        const dataFinalRaw = data.dataFinal ?? data.DataFinal ?? null;
                        const qtdParticipantes = data.qtdParticipantes ?? data.QtdParticipantes ?? "";

                        const dataInicio = dataInicialRaw ? moment(dataInicialRaw).format("DD/MM/YYYY") : "";
                        const dataFim = dataFinalRaw ? moment(dataFinalRaw).format("DD/MM/YYYY") : "";

                        $("#txtEventoDataInicio").val(dataInicio);
                        $("#txtEventoDataFim").val(dataFim);
                        $("#txtEventoQtdParticipantes").val(qtdParticipantes);
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarDetalhesEventoSelecionado.ajax.success", error);
                    }
                },
                error: function (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarDetalhesEventoSelecionado.ajax.error", error);
                }
            });
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarDetalhesEventoSelecionado", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: limparDetalhesEventoSelecionado
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar dados antigos quando nenhum evento esta selecionado.
 *                   [O QUE] Limpa os campos basicos de evento.
 *                   [COMO] Reseta valores dos inputs de detalhes.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : Inputs limpos.
 *
 * ⬅️ CHAMADO POR  : onEventoSelecionado(), lstFinalidade_Change(), atualizarDetalhesEventoSelecionado().
 ****************************************************************************************/
function limparDetalhesEventoSelecionado()
{
    try
    {
        $("#txtEventoDataInicio").val("");
        $("#txtEventoDataFim").val("");
        $("#txtEventoQtdParticipantes").val("");
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "limparDetalhesEventoSelecionado", error);
    }
}

function PreencheListaRequisitantes()
{
    try
    {
        const requisitantes = document.getElementById("cmbRequisitante");
        if (
            $("#cmbRequisitante").data("kendoComboBox")
        )
        {
            $("#cmbRequisitante").data("kendoComboBox").dataSource.data([]);
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaRequisitantes", error);
    }
}

function PreencheListaSetores(SetorSolicitanteId)
{
    try
    {
        const setor = document.getElementById("cmbSetor");
        if (getComboEJ2("cmbSetor"))
        {
            getComboEJ2("cmbSetor").dataSource = [];
            getComboEJ2("cmbSetor").enabled = true;
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaSetores", error);
    }
}

function upload(args)
{
    try
    {
        console.log("Arquivo enviado:", args);
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "upload", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: toolbarClick
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Garantir que uploads do editor levem AntiForgery.
 *                   [O QUE] Injeta o header XSRF ao clicar em Image no toolbar.
 *                   [COMO] Configura o callback de upload do EJ2 quando necessário.
 *
 * 📥 ENTRADAS     : e [Object] - Evento do toolbar com item.id.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : Toolbar do RichTextEditor.
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function toolbarClick(e)
{
    try
    {
        if (e && e.item && e.item.id && e.item.id.indexOf("Image") >= 0)
        {
            var up = document.getElementById("rte_upload");
            if (up && up.ej2_instances && up.ej2_instances[0])
            {
                up.ej2_instances[0].uploading = function (args)
                {
                    const token =
                        document.getElementsByName("__RequestVerificationToken")[0]?.value ||
                        document.querySelector('input[name="__RequestVerificationToken"]')?.value;
                    if (token)
                    {
                        args.currentRequest.setRequestHeader("XSRF-TOKEN", token);
                    }
                };
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "toolbarClick", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: stopEnterSubmitting
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar submissão acidental do formulário por Enter.
 *                   [O QUE] Bloqueia Enter fora de divs contenteditable.
 *                   [COMO] Cancela o evento de teclado quando apropriado.
 *
 * 📥 ENTRADAS     : e [Event] - Evento de teclado.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : onkeypress do formulário de viagem.
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function stopEnterSubmitting(e)
{
    try
    {
        const evt = e || window.event;
        if (!evt) return;
        if ((evt.key && evt.key === "Enter") || evt.keyCode === 13)
        {
            const src = evt.target || evt.srcElement;
            if (!src || src.tagName.toLowerCase() !== "div")
            {
                if (evt.preventDefault) evt.preventDefault();
                else evt.returnValue = false;
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "stopEnterSubmitting", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: atualizarImagemModal
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Garantir que o modal de zoom mostre a imagem atual.
 *                   [O QUE] Copia o src da imagem de preview para o modal.
 *                   [COMO] Busca os elementos e atualiza o atributo src.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : Clique no botão de zoom da ficha.
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function atualizarImagemModal()
{
    try
    {
        const imgSrc = document.getElementById("imgViewerItem")?.src || "";
        const modalImg = document.getElementById("imgZoomed");
        if (modalImg)
        {
            modalImg.src = imgSrc;
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarImagemModal", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: lstFinalidade_Change
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Exibir campos de Evento apenas quando finalidade for Evento.
 *                   [O QUE] Alterna visibilidade e habilita/desabilita o dropdown.
 *                   [COMO] Usa o Kendo DropDownList e atualiza o card de evento.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : UI atualizada.
 *
 * ⬅️ CHAMADO POR  : change do Kendo `ddlFinalidade`.
 *
 * ➡️ CHAMA        : limparDetalhesEventoSelecionado().
 ****************************************************************************************/
function lstFinalidade_Change()
{
    try
    {
        const ddl = $("#ddlFinalidade").data("kendoDropDownList");
        const finalidade = ddl ? ddl.value() : null;
        const ddlEvento = $("#ddlEvento").data("kendoDropDownList");
        const btnEvento = document.getElementById("btnEvento");

        if (finalidade === "Evento")
        {
            if (ddlEvento) ddlEvento.enable(true);
            if (btnEvento)
            {
                btnEvento.style.display = "block";
                btnEvento.setAttribute("data-bs-toggle", "modal");
                btnEvento.setAttribute("data-bs-target", "#modalEvento");
            }
            $(".esconde-diveventos").show();
        }
        else
        {
            if (ddlEvento)
            {
                ddlEvento.value("");
                ddlEvento.enable(false);
            }
            if (btnEvento) btnEvento.style.display = "none";
            $(".esconde-diveventos").hide();
            if (typeof limparDetalhesEventoSelecionado === "function")
            {
                limparDetalhesEventoSelecionado();
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "lstFinalidade_Change", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: controlarSecaoOcorrencias
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Impedir ocorrencias sem veiculo valido.
 *                   [O QUE] Habilita/desabilita a secao conforme selecao.
 *                   [COMO] Aplica classes e ajusta botao/aviso.
 *
 * 📥 ENTRADAS     : veiculoId [string] - GUID do veiculo.
 *
 * 📤 SAÍDAS       : UI da secao de ocorrencias.
 *
 * ⬅️ CHAMADO POR  : VeiculoValueChange(), inicializacao da pagina.
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function controlarSecaoOcorrencias(veiculoId)
{
    try
    {
        const secao = document.getElementById("secaoOcorrenciasUpsert");
        const btnAdicionar = document.getElementById("btnAdicionarOcorrenciaUpsert");
        const aviso = document.getElementById("avisoSelecionarVeiculo");

        if (!secao) return;

        if (window.viagemFinalizada === true)
        {
            if (btnAdicionar) btnAdicionar.disabled = true;
            return;
        }

        const temVeiculo = veiculoId &&
            veiculoId !== "" &&
            veiculoId !== "00000000-0000-0000-0000-000000000000";

        if (temVeiculo)
        {
            secao.classList.remove("ftx-section-disabled");
            if (btnAdicionar) btnAdicionar.disabled = false;
            if (aviso) aviso.style.display = "none";
        }
        else
        {
            secao.classList.add("ftx-section-disabled");
            if (btnAdicionar) btnAdicionar.disabled = true;
            if (aviso) aviso.style.display = "";
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "controlarSecaoOcorrencias", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: initModalZoomHandler
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Garantir sincronismo da imagem no modal de zoom.
 *                   [O QUE] Copia o src do preview antes de abrir o modal.
 *                   [COMO] Escuta o evento show.bs.modal.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initViagemUpsertPage().
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function initModalZoomHandler()
{
    try
    {
        var mz = document.getElementById("modalZoom");
        if (!mz) return;
        mz.addEventListener("show.bs.modal", function ()
        {
            try
            {
                var src = document.getElementById("imgViewerItem")?.getAttribute("src") ||
                    document.getElementById("imgViewer")?.getAttribute("src") || "";
                var target = document.getElementById("imgZoomed");
                if (target) target.setAttribute("src", src);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initModalZoomHandler.show", error);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initModalZoomHandler", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: normalizeTextoFuzzy
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Padronizar strings para comparacao fuzzy.
 *                   [O QUE] Remove acentos, espacos extras e normaliza caixa.
 *                   [COMO] Aplica normalizacao Unicode e regex.
 *
 * 📥 ENTRADAS     : texto [string] - Texto bruto.
 *
 * 📤 SAÍDAS       : string normalizada.
 *
 * ⬅️ CHAMADO POR  : validarDuplicadoNaLista(), validarCruzado().
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function normalizeTextoFuzzy(texto)
{
    try
    {
        if (!texto) return "";
        return String(texto)
            .normalize("NFKC")
            .replace(/[\u200B-\u200D\uFEFF]/g, "")
            .replace(/\u00A0/g, " ")
            .toLowerCase()
            .normalize("NFD").replace(/[\u0300-\u036f]/g, "")
            .replace(/[\s\u00A0]+/g, " ")
            .trim();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "normalizeTextoFuzzy", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: levenshteinRaw
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Calcular distancia de edicao para comparacao fuzzy.
 *                   [O QUE] Retorna numero de edicoes entre duas strings.
 *                   [COMO] Usa DP classico com matriz n+1 x m+1.
 *
 * 📥 ENTRADAS     : a [string], b [string].
 *
 * 📤 SAÍDAS       : number.
 *
 * ⬅️ CHAMADO POR  : similarityFuzzy().
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function levenshteinRaw(a, b)
{
    try
    {
        const n = a.length, m = b.length;
        if (n === 0) return m;
        if (m === 0) return n;
        const dp = Array.from({ length: n + 1 }, () => new Array(m + 1).fill(0));
        for (let i = 0; i <= n; i++) dp[i][0] = i;
        for (let j = 0; j <= m; j++) dp[0][j] = j;
        for (let i = 1; i <= n; i++)
        {
            for (let j = 1; j <= m; j++)
            {
                const cost = a[i - 1] === b[j - 1] ? 0 : 1;
                dp[i][j] = Math.min(
                    dp[i - 1][j] + 1,
                    dp[i][j - 1] + 1,
                    dp[i - 1][j - 1] + cost,
                );
            }
        }
        return dp[n][m];
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "levenshteinRaw", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: similarityFuzzy
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Medir similaridade para evitar destinos duplicados.
 *                   [O QUE] Retorna score 0..1.
 *                   [COMO] Normaliza e usa distancia de Levenshtein.
 *
 * 📥 ENTRADAS     : a [string], b [string].
 *
 * 📤 SAÍDAS       : number.
 *
 * ⬅️ CHAMADO POR  : validarDuplicadoNaLista(), validarCruzado().
 *
 * ➡️ CHAMA        : normalizeTextoFuzzy(), levenshteinRaw().
 ****************************************************************************************/
function similarityFuzzy(a, b)
{
    try
    {
        const na = normalizeTextoFuzzy(a);
        const nb = normalizeTextoFuzzy(b);
        if (!na && !nb) return 1;
        const dist = levenshteinRaw(na, nb);
        const maxLen = Math.max(na.length, nb.length) || 1;
        return 1 - dist / maxLen;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "similarityFuzzy", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: getComboEJ2
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Centralizar acesso a combos EJ2 por ID.
 *                   [O QUE] Retorna instancia ou null.
 *                   [COMO] Usa ej2_instances do elemento.
 *
 * 📥 ENTRADAS     : id [string] - ID do elemento.
 *
 * 📤 SAÍDAS       : Object|null.
 *
 * ⬅️ CHAMADO POR  : initFuzzyDestinoValidation().
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function getComboEJ2(id)
{
    try
    {
        const host = document.getElementById(id);
        return host?.ej2_instances?.[0] ?? null;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "getComboEJ2", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: extractTextsFromItems
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Obter textos para validacao fuzzy.
 *                   [O QUE] Extrai textos de arrays heterogeneos.
 *                   [COMO] Usa fields.text quando disponivel.
 *
 * 📥 ENTRADAS     : items [Array], fields [Object].
 *
 * 📤 SAÍDAS       : Array<string>.
 *
 * ⬅️ CHAMADO POR  : getMasterTexts().
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function extractTextsFromItems(items, fields)
{
    try
    {
        const textField = fields?.text;
        if (!Array.isArray(items)) return [];
        if (items.length && (typeof items[0] === "string" || typeof items[0] === "number"))
        {
            return items.map(x => String(x));
        }
        return items.map(obj =>
        {
            if (!obj) return "";
            if (textField && obj[textField] != null) return String(obj[textField]);
            if (obj.text != null) return String(obj.text);
            if (obj.value != null) return String(obj.value);
            return "";
        }).filter(x => x !== "");
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "extractTextsFromItems", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: getMasterTexts
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Cachear textos de lista para validacao fuzzy.
 *                   [O QUE] Retorna lista de textos do datasource.
 *                   [COMO] Usa listData ou dataSource do combo EJ2.
 *
 * 📥 ENTRADAS     : combo [Object] - Instancia EJ2.
 *
 * 📤 SAÍDAS       : Array<string>.
 *
 * ⬅️ CHAMADO POR  : validarDuplicadoNaLista().
 *
 * ➡️ CHAMA        : extractTextsFromItems().
 ****************************************************************************************/
function getMasterTexts(combo)
{
    try
    {
        if (Array.isArray(combo.__masterTexts) && combo.__masterTexts.length) return combo.__masterTexts;
        let texts = extractTextsFromItems(combo.listData, combo.fields);
        if (!texts.length)
        {
            const ds = combo.dataSource;
            texts = extractTextsFromItems(ds?.dataSource?.json ?? ds, combo.fields);
        }
        combo.__masterTexts = texts;
        return texts;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "getMasterTexts", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: wireMasterCache
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Manter cache atualizado apos dataBound.
 *                   [O QUE] Conecta o cache de textos ao combo EJ2.
 *                   [COMO] Sobrescreve dataBound preservando o original.
 *
 * 📥 ENTRADAS     : combo [Object] - Instancia EJ2.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initFuzzyDestinoValidation().
 ****************************************************************************************/
function wireMasterCache(combo)
{
    try
    {
        if (!combo.__masterTexts || !combo.__masterTexts.length)
        {
            combo.__masterTexts = getMasterTexts(combo);
        }
        const prev = combo.dataBound;
        combo.dataBound = function ()
        {
            if (typeof prev === "function") prev.apply(combo, arguments);
            if (!combo.__masterTexts || !combo.__masterTexts.length)
            {
                combo.__masterTexts = extractTextsFromItems(combo.listData, combo.fields);
            }
        };
        combo.dataBind();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "wireMasterCache", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: alertInfo
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Padronizar informativos do fuzzy validator.
 *                   [O QUE] Dispara Alerta.Info com fallback console.
 *                   [COMO] Verifica disponibilidade do Alerta.
 *
 * 📥 ENTRADAS     : titulo [string], texto [string], confirm [string].
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : validarDuplicadoNaLista(), validarCruzado().
 ****************************************************************************************/
function alertInfo(titulo, texto, confirm)
{
    try
    {
        const confirmText = confirm || "OK";
        if (typeof Alerta !== "undefined" && Alerta?.Info) Alerta.Info(titulo, texto, confirmText);
        else console.warn(`${titulo}\n\n${texto}`);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "alertInfo", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: alertWarn
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Padronizar advertencias do fuzzy validator.
 *                   [O QUE] Dispara Alerta.Warning com fallback console.
 *                   [COMO] Verifica disponibilidade do Alerta.
 *
 * 📥 ENTRADAS     : titulo [string], texto [string], confirm [string].
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : validarDuplicadoNaLista(), validarCruzado().
 ****************************************************************************************/
function alertWarn(titulo, texto, confirm)
{
    try
    {
        const confirmText = confirm || "OK";
        if (typeof Alerta !== "undefined" && Alerta?.Warning) Alerta.Warning(titulo, texto, confirmText);
        else console.warn(`${titulo}\n\n${texto}`);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "alertWarn", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: validarDuplicadoNaLista
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar origem/destino duplicado por digitacao livre.
 *                   [O QUE] Compara com lista via similaridade fuzzy.
 *                   [COMO] Usa thresholds e sugere canonizacao.
 *
 * 📥 ENTRADAS     : combo [Object], opts [Object].
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initFuzzyDestinoValidation().
 *
 * ➡️ CHAMA        : getMasterTexts(), similarityFuzzy(), alertInfo(), alertWarn().
 ****************************************************************************************/
function validarDuplicadoNaLista(combo, opts)
{
    try
    {
        const {
            infoThreshold = 0.85,
            warnThreshold = 0.92,
            confirmarTexto = "OK",
            incluirSugestao = true,
            autoCanonizar = true,
        } = opts || {};

        if (Number.isInteger(combo.index) && combo.index >= 0) return;

        const digitado = combo.inputElement ? combo.inputElement.value : combo.value;
        const norm = normalizeTextoFuzzy(digitado);
        if (!norm) return;

        const opcoes = getMasterTexts(combo);
        if (!opcoes.length) return;

        const existeExato = opcoes.some(o => String(o) === digitado);
        if (existeExato) return;

        const mapaNormParaOriginal = new Map();
        for (const o of opcoes)
        {
            const n = normalizeTextoFuzzy(o);
            if (!mapaNormParaOriginal.has(n)) mapaNormParaOriginal.set(n, o);
        }
        if (mapaNormParaOriginal.has(norm))
        {
            const canonico = mapaNormParaOriginal.get(norm);
            if (autoCanonizar && canonico && digitado !== canonico)
            {
                combo.inputElement.value = canonico;
                if ("text" in combo) combo.text = canonico;
                if ("value" in combo && (typeof combo.value === "string" || combo.value == null)) combo.value = canonico;
                combo.dataBind?.();
            }
            return;
        }

        let best = { item: null, score: 0 };
        for (const opt of opcoes)
        {
            const s = similarityFuzzy(digitado, opt);
            if (s > best.score) best = { item: opt, score: s };
        }
        if (!best.item) return;

        const pct = (best.score * 100).toFixed(0);
        const id = combo.element?.id || "";
        const tituloBase = id === "cmbOrigem" ? "Origem" : id === "cmbDestino" ? "Destino" : "Item";
        const sugestao = incluirSugestao ? `\nSugestao: “${best.item}” (similaridade ${pct}%).` : "";

        if (best.score >= warnThreshold)
        {
            alertWarn(`Provavel duplicado • ${tituloBase}`, `E muito provavel que ja exista na lista.${sugestao}`, confirmarTexto);
        }
        else if (best.score >= infoThreshold)
        {
            alertInfo(`Semelhanca alta • ${tituloBase}`, `Pode ja existir algo parecido na lista.${sugestao}`, confirmarTexto);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "validarDuplicadoNaLista", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: validarCruzado
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar origem e destino praticamente iguais.
 *                   [O QUE] Alerta quando campos sao muito similares.
 *                   [COMO] Usa similarityFuzzy com thresholds.
 *
 * 📥 ENTRADAS     : origemCombo [Object], destinoCombo [Object], opts [Object].
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initFuzzyDestinoValidation().
 *
 * ➡️ CHAMA        : similarityFuzzy(), alertInfo(), alertWarn().
 ****************************************************************************************/
function validarCruzado(origemCombo, destinoCombo, opts)
{
    try
    {
        const { infoThreshold = 0.85, warnThreshold = 0.92, confirmarTexto = "OK" } = opts || {};

        const origem = origemCombo?.inputElement ? origemCombo.inputElement.value : origemCombo?.value;
        const destino = destinoCombo?.inputElement ? destinoCombo.inputElement.value : destinoCombo?.value;

        const norigem = normalizeTextoFuzzy(origem);
        const ndestino = normalizeTextoFuzzy(destino);
        if (!norigem || !ndestino) return;

        const score = norigem === ndestino ? 1 : similarityFuzzy(norigem, ndestino);
        const pct = (score * 100).toFixed(0);

        if (score >= warnThreshold)
        {
            alertWarn("Origem e destino muito parecidos", `Os campos parecem referir-se ao mesmo lugar (similaridade ${pct}%).`, confirmarTexto);
        }
        else if (score >= infoThreshold)
        {
            alertInfo("Origem e destino semelhantes", `Verifique se sao realmente distintos (similaridade ${pct}%).`, confirmarTexto);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "validarCruzado", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: initFuzzyDestinoValidation
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Ativar validacao fuzzy para Origem/Destino.
 *                   [O QUE] Conecta blur handlers aos combos EJ2.
 *                   [COMO] Usa getComboEJ2 e wireMasterCache.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initViagemUpsertPage().
 *
 * ➡️ CHAMA        : validarDuplicadoNaLista(), validarCruzado().
 ****************************************************************************************/
function initFuzzyDestinoValidation()
{
    try
    {
        function connect(id, peerId, opts)
        {
            const c = getComboEJ2(id);
            const p = getComboEJ2(peerId);
            if (!c || !c.inputElement) return false;
            wireMasterCache(c);
            c.inputElement.addEventListener("blur", function ()
            {
                validarDuplicadoNaLista(c, opts);
                if (p && p.inputElement) validarCruzado(id === "cmbOrigem" ? c : p, id === "cmbOrigem" ? p : c, opts);
            });
            return true;
        }

        function tryWire()
        {
            const opts = { infoThreshold: 0.85, warnThreshold: 0.92, autoCanonizar: true };
            const okO = connect("cmbOrigem", "cmbDestino", opts);
            const okD = connect("cmbDestino", "cmbOrigem", opts);
            return okO && okD;
        }

        if (!tryWire())
        {
            document.addEventListener("DOMContentLoaded", tryWire, { once: true });
            window.addEventListener("load", () => setTimeout(tryWire, 0), { once: true });
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initFuzzyDestinoValidation", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: initKendoDropDowns
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Inicializar dropdowns Kendo do Upsert.
 *                   [O QUE] Configura Finalidade, Evento, Motorista e Combustiveis.
 *                   [COMO] Usa dados do contexto e templates locais.
 *
 * 📥 ENTRADAS     : context [Object] - Dados serializados da pagina.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initViagemUpsertPage().
 *
 * ➡️ CHAMA        : lstFinalidade_Change(), MotoristaValueChange(), onEventoSelecionado().
 ****************************************************************************************/
function initKendoDropDowns(context)
{
    try
    {
        if (!window.kendo || !context) return;

        var dataFinalidade = context.dataFinalidade || [];
        var dataCombustivel = context.dataCombustivel || [];
        var dataEvento = context.dataEvento || [];
        var dataMotorista = context.dataMotorista || [];

        function combustivelTemplate(data)
        {
            try
            {
                return '<span class="combustivel-item">' +
                    '<img src="' + kendo.htmlEncode(data.imagem) + '" alt="' + kendo.htmlEncode(data.descricao) + '" />' +
                    '<span>' + kendo.htmlEncode(data.descricao) + '</span>' +
                    '</span>';
            }
            catch (error)
            {
                return '<span>' + (data.descricao || '') + '</span>';
            }
        }

        function combustivelValueTemplate(data)
        {
            try
            {
                if (data && data.imagem)
                {
                    return '<span class="combustivel-value">' +
                        '<img src="' + kendo.htmlEncode(data.imagem) + '" alt="' + kendo.htmlEncode(data.descricao) + '" />' +
                        '<span>' + kendo.htmlEncode(data.descricao) + '</span>' +
                        '</span>';
                }
                return '<span>' + kendo.htmlEncode(data.descricao || '') + '</span>';
            }
            catch (error)
            {
                return '<span>' + (data.descricao || '') + '</span>';
            }
        }

        function motoristaTemplate(data)
        {
            try
            {
                var imgSrc = (data && data.foto && data.foto.indexOf('data:image') === 0)
                    ? data.foto
                    : '/images/barbudo.jpg';
                return '<span class="motorista-item">' +
                    '<img src="' + kendo.htmlEncode(imgSrc) + '" alt="Foto" />' +
                    '<span>' + kendo.htmlEncode(data.nome || '') + '</span>' +
                    '</span>';
            }
            catch (error)
            {
                return '<span>' + (data && data.nome ? kendo.htmlEncode(data.nome) : '') + '</span>';
            }
        }

        function motoristaValueTemplate(data)
        {
            try
            {
                if (!data) return '';
                var imgSrc = (data.foto && data.foto.indexOf('data:image') === 0)
                    ? data.foto
                    : '/images/barbudo.jpg';
                return '<span class="motorista-value">' +
                    '<img src="' + kendo.htmlEncode(imgSrc) + '" alt="Foto" />' +
                    '<span>' + kendo.htmlEncode(data.nome || '') + '</span>' +
                    '</span>';
            }
            catch (error)
            {
                return '<span>' + (data && data.nome ? kendo.htmlEncode(data.nome) : '') + '</span>';
            }
        }

        $("#ddlFinalidade").kendoDropDownList({
            dataTextField: "descricao",
            dataValueField: "finalidadeId",
            optionLabel: "Escolha a Finalidade...",
            dataSource: dataFinalidade,
            height: 200,
            value: context.valores?.finalidade || "",
            change: function ()
            {
                try { lstFinalidade_Change(); } catch (err) { console.error("lstFinalidade_Change:", err); }
            },
        });

        $("#ddlEvento").kendoDropDownList({
            dataTextField: "nome",
            dataValueField: "eventoId",
            optionLabel: "Selecione um Evento...",
            dataSource: dataEvento,
            filter: "contains",
            height: 200,
            value: context.valores?.eventoId || "",
            change: function (e)
            {
                try { if (typeof onEventoSelecionado === "function") onEventoSelecionado(e); }
                catch (err) { console.error("onEventoSelecionado:", err); }
            },
        });

        $("#cmbMotorista").kendoDropDownList({
            dataTextField: "nome",
            dataValueField: "motoristaId",
            optionLabel: "Selecione um Motorista",
            dataSource: dataMotorista,
            filter: "contains",
            height: 200,
            value: context.valores?.motoristaId || "",
            template: motoristaTemplate,
            valueTemplate: motoristaValueTemplate,
            change: function ()
            {
                try { if (typeof MotoristaValueChange === "function") MotoristaValueChange(); }
                catch (err) { console.error("MotoristaValueChange:", err); }
            },
        });

        $("#ddlCombustivelInicial").kendoDropDownList({
            dataTextField: "descricao",
            dataValueField: "nivel",
            optionLabel: "Selecione...",
            dataSource: dataCombustivel,
            height: 200,
            value: context.valores?.combustivelInicial || "",
            template: combustivelTemplate,
            valueTemplate: combustivelValueTemplate,
        });

        $("#ddlCombustivelFinal").kendoDropDownList({
            dataTextField: "descricao",
            dataValueField: "nivel",
            optionLabel: "Selecione...",
            dataSource: dataCombustivel,
            height: 200,
            value: context.valores?.combustivelFinal || "",
            template: combustivelTemplate,
            valueTemplate: combustivelValueTemplate,
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initKendoDropDowns", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: initKendoDateTimePickers
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Inicializar DatePicker/TimePicker com regras pt-BR.
 *                   [O QUE] Configura limites, placeholders e validacoes de periodo.
 *                   [COMO] Usa contexto para valores iniciais e helpers locais.
 *
 * 📥 ENTRADAS     : context [Object] - Datas e horas iniciais.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initViagemUpsertPage().
 *
 * ➡️ CHAMA        : Alerta.TratamentoErroComLinha().
 ****************************************************************************************/
function initKendoDateTimePickers(context)
{
    try
    {
        var agora = new Date();
        var dataMinima = new Date();
        dataMinima.setDate(dataMinima.getDate() - 15);
        var dataMaxima = new Date();
        var horaAgora = new Date(0, 0, 0, agora.getHours(), agora.getMinutes(), 0, 0);

        var dataInicialContext = context?.datas?.dataInicial || null;
        var dataFinalContext = context?.datas?.dataFinal || null;
        var horaInicialContext = context?.datas?.horaInicio || null;
        var horaFinalContext = context?.datas?.horaFim || null;

        if (window.kendo && kendo.ui && kendo.ui.DateInput)
        {
            kendo.culture("pt-BR");
            kendo.ui.DateInput.prototype.options.messages = {
                year: "yyyy",
                month: "MM",
                day: "dd",
                hour: "HH",
                minute: "mm",
            };
        }

        var dpDataInicial = $("#txtDataInicial").kendoDatePicker({
            format: "dd/MM/yyyy",
            culture: "pt-BR",
            min: dataMinima,
            max: dataMaxima,
            dateInput: {
                format: "dd/MM/yyyy",
                messages: { year: "yyyy", month: "MM", day: "dd" },
            },
            placeholder: "dd/MM/yyyy",
            value: dataInicialContext || new Date(agora.getFullYear(), agora.getMonth(), agora.getDate()),
            change: function ()
            {
                try
                {
                    var dataInicial = this.value();
                    if (!dataInicial)
                    {
                        setTimeout(function ()
                        {
                            Alerta.Erro("Data Inicial invalida", "Por favor, selecione uma data valida.");
                        }, 100);
                        dpDataInicial.value(null);
                    }
                    if (dataInicial && dataInicial > dataMaxima)
                    {
                        Alerta.Warning("Data Inicial invalida", "A Data Inicial nao pode ser superior a data atual.", "OK");
                        dpDataInicial.value(null);
                        aplicarMinimoDataFinal(null);
                        return;
                    }
                    aplicarMinimoDataFinal(dataInicial);
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "dpDataInicial.change", error);
                }
            },
        }).data("kendoDatePicker");

        var tpHoraInicial = $("#txtHoraInicial").kendoTimePicker({
            format: "HH:mm",
            culture: "pt-BR",
            interval: 15,
            dateInput: {
                format: "HH:mm",
                messages: { hour: "HH", minute: "mm" },
            },
            placeholder: "HH:mm",
            value: horaInicialContext || horaAgora,
        }).data("kendoTimePicker");

        function aplicarMinimoDataFinal(dataInicial)
        {
            try
            {
                if (!dpDataFinal) return;

                var baseMin = (dataInicial && dataInicial instanceof Date && !isNaN(dataInicial))
                    ? new Date(dataInicial.getFullYear(), dataInicial.getMonth(), dataInicial.getDate())
                    : dataMinima;

                dpDataFinal.min(baseMin);
                dpDataFinal.max(dataMaxima);

                var dataFinalAtual = dpDataFinal.value();
                if (dataFinalAtual && dataFinalAtual instanceof Date && !isNaN(dataFinalAtual))
                {
                    if (dataFinalAtual < baseMin)
                    {
                        dpDataFinal.value(null);
                    }
                }

                validarDataFinalEControlarHoraFim();
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "aplicarMinimoDataFinal", error);
            }
        }

        function validarDataFinalEControlarHoraFim()
        {
            try
            {
                if (!dpDataFinal || !tpHoraFinal) return;

                var dataFinal = dpDataFinal.value();
                var dataInicial = dpDataInicial ? dpDataInicial.value() : null;

                if (dataFinal && dataFinal instanceof Date && !isNaN(dataFinal))
                {
                    if (dataFinal > dataMaxima)
                    {
                        Alerta.Warning(
                            "Data Final invalida",
                            "A <strong>Data Final</strong> nao pode ser superior a data atual.",
                            "OK",
                        );
                        dpDataFinal.value(null);
                        tpHoraFinal.enable(false);
                        tpHoraFinal.value(null);
                        return;
                    }
                    if (dataInicial && dataInicial instanceof Date && !isNaN(dataInicial))
                    {
                        if (dataFinal < dataInicial)
                        {
                            Alerta.Warning(
                                "Data Final invalida",
                                "A <strong>Data Final</strong> deve ser maior ou igual a <strong>Data Inicial</strong>.",
                                "OK",
                            );
                            dpDataFinal.value(null);
                            tpHoraFinal.enable(false);
                            tpHoraFinal.value(null);
                            return;
                        }
                    }

                    tpHoraFinal.enable(true);
                }
                else
                {
                    tpHoraFinal.enable(false);
                    tpHoraFinal.value(null);
                }
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "validarDataFinalEControlarHoraFim", error);
            }
        }

        var dpDataFinal = $("#txtDataFinal").kendoDatePicker({
            format: "dd/MM/yyyy",
            culture: "pt-BR",
            min: dataMinima,
            max: dataMaxima,
            placeholder: "dd/MM/yyyy",
            dateInput: {
                format: "dd/MM/yyyy",
                placeholder: "dd/MM/yyyy",
                messages: { year: "yyyy", month: "MM", day: "dd" },
            },
            value: dataFinalContext,
            change: function ()
            {
                validarDataFinalEControlarHoraFim();
            },
        }).data("kendoDatePicker");

        $("#txtDataInicialEvento").kendoDatePicker({
            format: "dd/MM/yyyy",
            culture: "pt-BR",
            placeholder: "dd/MM/yyyy",
            dateInput: {
                format: "dd/MM/yyyy",
                messages: { year: "yyyy", month: "MM", day: "dd" },
            },
            value: null,
        });

        $("#txtDataFinalEvento").kendoDatePicker({
            format: "dd/MM/yyyy",
            culture: "pt-BR",
            placeholder: "dd/MM/yyyy",
            dateInput: {
                format: "dd/MM/yyyy",
                messages: { year: "yyyy", month: "MM", day: "dd" },
            },
            value: null,
        });

        var tpHoraFinal = $("#txtHoraFinal").kendoTimePicker({
            format: "HH:mm",
            culture: "pt-BR",
            interval: 15,
            placeholder: "HH:mm",
            dateInput: {
                format: "HH:mm",
                placeholder: "HH:mm",
                messages: { hour: "HH", minute: "mm" },
            },
            value: horaFinalContext,
            open: function (e)
            {
                try
                {
                    var dataFinal = dpDataFinal.value();
                    if (!dataFinal || !(dataFinal instanceof Date) || isNaN(dataFinal))
                    {
                        e.preventDefault();
                        Alerta.Warning(
                            "Data Final Obrigatoria",
                            "Preencha a <strong>Data Final</strong> antes de selecionar a Hora Fim.",
                            "OK",
                        );
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "tpHoraFinal.open", error);
                }
            },
        }).data("kendoTimePicker");

        if (dpDataFinal && dpDataFinal.dateInput && dpDataFinal.dateInput.element)
        {
            if (typeof dpDataFinal.dateInput.setOptions === "function")
            {
                dpDataFinal.dateInput.setOptions({
                    format: "dd/MM/yyyy",
                    messages: { year: "yyyy", month: "MM", day: "dd" },
                });
            }
            dpDataFinal.dateInput.element.attr("placeholder", "dd/MM/yyyy");
        }
        if (tpHoraFinal && tpHoraFinal.dateInput && tpHoraFinal.dateInput.element)
        {
            if (typeof tpHoraFinal.dateInput.setOptions === "function")
            {
                tpHoraFinal.dateInput.setOptions({
                    format: "HH:mm",
                    messages: { hour: "HH", minute: "mm" },
                });
            }
            tpHoraFinal.dateInput.element.attr("placeholder", "HH:mm");
        }

        setTimeout(function ()
        {
            try
            {
                $("#txtDataFinal").attr("placeholder", "dd/MM/yyyy");
                $("#txtHoraFinal").attr("placeholder", "HH:mm");

                if (dpDataFinal && dpDataFinal.dateInput && dpDataFinal.dateInput.element)
                {
                    dpDataFinal.dateInput.element.attr("placeholder", "dd/MM/yyyy");
                }
                if (tpHoraFinal && tpHoraFinal.dateInput && tpHoraFinal.dateInput.element)
                {
                    tpHoraFinal.dateInput.element.attr("placeholder", "HH:mm");
                }
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "placeholder.reinforce", error);
            }
        }, 800);

        aplicarMinimoDataFinal(dpDataInicial ? dpDataInicial.value() : null);
        var dataFinalInicial = dpDataFinal.value();
        if (!dataFinalInicial || !(dataFinalInicial instanceof Date) || isNaN(dataFinalInicial))
        {
            tpHoraFinal.enable(false);
        }
        else
        {
            tpHoraFinal.enable(true);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initKendoDateTimePickers", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: initUnsavedChangesGuard
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Evitar perda de alteracoes nao salvas.
 *                   [O QUE] Detecta mudancas e confirma ao sair.
 *                   [COMO] Compara serialize() do form e intercepta botoes.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : initViagemUpsertPage().
 *
 * ➡️ CHAMA        : Alerta.Confirmar().
 ****************************************************************************************/
function initUnsavedChangesGuard()
{
    try
    {
        var estadoInicial = $("form").serialize();
        var formularioAlterado = false;

        $("form").on("change input", "input, select, textarea", function ()
        {
            try
            {
                formularioAlterado = ($("form").serialize() !== estadoInicial);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "form.change", error);
            }
        });

        function verificarEVoltar()
        {
            try
            {
                if (formularioAlterado)
                {
                    Alerta.Confirmar(
                        "Descartar Alteracoes?",
                        "Voce fez alteracoes no formulario que ainda nao foram salvas. Deseja realmente descartar essas mudancas?",
                        "Sim, descartar",
                        "Cancelar",
                    ).then(function (confirmado)
                    {
                        try
                        {
                            if (confirmado)
                            {
                                window.location.href = "/Viagens";
                            }
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarEVoltar.confirmar.then", error);
                        }
                    });
                }
                else
                {
                    window.location.href = "/Viagens";
                }
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarEVoltar", error);
            }
        }

        $("#btnVoltarLista").on("click", function (e)
        {
            try
            {
                e.preventDefault();
                verificarEVoltar();
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "btnVoltarLista.click", error);
            }
        });

        $(".btn-voltar-lista").on("click", function (e)
        {
            try
            {
                e.preventDefault();
                verificarEVoltar();
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "btn-voltar-lista.click", error);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initUnsavedChangesGuard", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: initViagemUpsertPage
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Centralizar bootstrap do Upsert de Viagens.
 *                   [O QUE] Inicializa Kendo, validacoes e handlers da pagina.
 *                   [COMO] Executa inicializadores no ready.
 *
 * 📥 ENTRADAS     : context [Object] - Dados da pagina.
 *
 * 📤 SAÍDAS       : void.
 *
 * ⬅️ CHAMADO POR  : Upsert.cshtml (ScriptsBlock).
 *
 * ➡️ CHAMA        : initKendoDropDowns(), initKendoDateTimePickers(), initFuzzyDestinoValidation(), initModalZoomHandler(), initUnsavedChangesGuard().
 ****************************************************************************************/
function initViagemUpsertPage(context)
{
    try
    {
        $(document).ready(function ()
        {
            try
            {
                initKendoDropDowns(context);
                initKendoDateTimePickers(context);

                // ⚠️ SISTEMA FUZZY MIGRADO: Agora usa kendo-fuzzy-validator.js (v2.0)
                // A validação fuzzy para Origem/Destino está no novo módulo carregado no Upsert.cshtml
                // initFuzzyDestinoValidation(); // ❌ DESATIVADO - código Syncfusion obsoleto

                initModalZoomHandler();
                initUnsavedChangesGuard();
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initViagemUpsertPage.ready", error);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "initViagemUpsertPage", error);
    }
}

async function validarKmAtualInicial()
{
    try
    {
        if (CarregandoViagemBloqueada)
        {
            return;
        }

        const kmInicial = $("#txtKmInicial").val();
        const kmAtual = $("#txtKmAtual").val();

        if (!kmInicial || !kmAtual) return true;

        const ini = parseFloat(kmAtual.replace(",", "."));
        const fim = parseFloat(kmInicial.replace(",", "."));

        if (fim < ini)
        {
            Alerta.Erro(
                "Erro",
                "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>.",
            );
            return false;
        }

        const diff = fim - ini;
        if (diff > 100)
        {
            const confirmado = await Alerta.Confirmar(
                "Quilometragem Alta",
                "A quilometragem <strong>inicial</strong> excede em 100km a <strong>atual</strong>. Tem certeza?",
                "Tenho certeza! 💪🏼",
                "Me enganei! 😟'",
            );

            if (!confirmado)
            {
                const txtKmInicialElement = document.getElementById("txtKmInicial");
                txtKmInicialElement.value = null;
                txtKmInicialElement.focus();
                return false;
            } else
            {
                calcularKmPercorrido();
            }
        }

        return true;
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "validarKmAtualInicial", error);
        return false;
    }
}

async function validarKmInicialFinal()
{
    try
    {
        if ($("#btnSubmit").is(":hidden"))
        {
            return;
        }

        const kmInicial = $("#txtKmInicial").val();
        const kmFinal = $("#txtKmFinal").val();

        if (!kmInicial || !kmFinal) return true;

        const ini = parseFloat(kmInicial.replace(",", "."));
        const fim = parseFloat(kmFinal.replace(",", "."));

        if (fim < ini)
        {
            Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial.");
            return false;
        }

        const diff = fim - ini;
        if (diff > 100)
        {
            const confirmado = await Alerta.Confirmar(
                "Quilometragem Alta",
                "A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?",
                "Tenho certeza! 💪🏼",
                "Me enganei! 😟'",
            );

            if (!confirmado)
            {
                const txtKmFinalElement = document.getElementById("txtKmFinal");
                txtKmFinalElement.value = null;
                txtKmFinalElement.focus();
                return false;
            } else
            {
                calcularKmPercorrido();
            }
        }

        return true;
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "validarKmInicialFinal", error);
        return false;
    }
}

async function validarDatasInicialFinal(DataInicial, DataFinal)
{
    try
    {
        if (CarregandoViagemBloqueada)
        {
            return;
        }

        function parseData(data)
        {
            try
            {
                if (!data) return null;
                if (data instanceof Date) return new Date(data.getTime());

                if (typeof data === "string")
                {
                    if (data.match(/^\d{4}\/\d{2}\/\d{2}$/))
                    {
                        const [ano, mes, dia] = data.split("/");
                        return new Date(ano, mes - 1, dia);
                    }
                    if (data.match(/^\d{4}-\d{2}-\d{2}$/))
                    {
                        const [ano, mes, dia] = data.split("-");
                        return new Date(ano, mes - 1, dia);
                    }
                    if (data.match(/^\d{2}\/\d{2}\/\d{4}$/))
                    {
                        const [dia, mes, ano] = data.split("/");
                        return new Date(ano, mes - 1, dia);
                    }
                }

                return null;
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "parseData", error);
            }
        }

        const dtIni = parseData(DataInicial);
        const dtFim = parseData(DataFinal);

        if (!dtIni || !dtFim || isNaN(dtIni) || isNaN(dtFim)) return true;

        const diff = (dtFim - dtIni) / (1000 * 60 * 60 * 24);

        if (diff >= 5)
        {
            const mensagem = "A Data Final está 5 dias ou mais após a Data Inicial. Tem certeza?";
            const confirmado = await window.SweetAlertInterop.ShowPreventionAlert(mensagem);

            if (confirmado)
            {
                AppToast.show("Verde", "Confirmacao feita pelo usuario!", 2000);
                document.getElementById("txtHoraFinal").focus();
            } else
            {
                AppToast.show("Amarelo", "Acao cancelada pelo usuario", 2000);

                const campo = document.getElementById("txtDataFinal");
                if (campo)
                {
                    campo.value = "";
                    campo.focus();
                    return false;
                }
            }
        }

        return true;
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "validarDatasInicialFinal", error);
        return false;
    }
}

// ========================= Duração: calcularDuracaoViagem ===================
function calcularDuracaoViagem()
{
    try
    {
        // [KENDO] Obter valores via API Kendo
        var dataInicial = window.getKendoDateValue("txtDataInicial");
        var horaInicial = window.getKendoTimeValue("txtHoraInicial");
        var dataFinal = window.getKendoDateValue("txtDataFinal");
        var horaFinal = window.getKendoTimeValue("txtHoraFinal");
        var elDuracao = document.getElementById("txtDuracao");
        
        console.log("calcularDuracaoViagem - DataInicial:", dataInicial, "HoraInicial:", horaInicial, "DataFinal:", dataFinal, "HoraFinal:", horaFinal);
        
        if (!elDuracao) return;

        var LIMIAR_MINUTOS = 120; // > 120 => inválido/tooltip

        // Faltando campos → limpar e resetar estados
        if (!dataInicial || !horaInicial || !dataFinal || !horaFinal)
        {
            elDuracao.value = "";
            if (typeof FieldUX !== 'undefined') {
                FieldUX.setInvalid(elDuracao, false);
                FieldUX.tooltipOnTransition(elDuracao, false, 1000, 'tooltipDuracao');
            }
            return;
        }

        // Montar strings para moment
        const strDataInicial = moment(dataInicial).format("YYYY-MM-DD");
        const strDataFinal = moment(dataFinal).format("YYYY-MM-DD");
        
        var inicio = moment(strDataInicial + "T" + horaInicial, "YYYY-MM-DDTHH:mm");
        var fim = moment(strDataFinal + "T" + horaFinal, "YYYY-MM-DDTHH:mm");
        if (!inicio.isValid() || !fim.isValid())
        {
            elDuracao.value = "";
            if (typeof FieldUX !== 'undefined') {
                FieldUX.setInvalid(elDuracao, false);
                FieldUX.tooltipOnTransition(elDuracao, false, 1000, 'tooltipDuracao');
            }
            return;
        }

        var duracaoMinutos = fim.diff(inicio, "minutes");
        var dias = Math.floor(duracaoMinutos / 1440);
        var horas = Math.floor((duracaoMinutos % 1440) / 60);
        var textoDuracao = dias + " dia" + (dias !== 1 ? "s" : "") +
            " e " + horas + " hora" + (horas !== 1 ? "s" : "");
        elDuracao.value = textoDuracao;

        // Regra Duração
        var invalid = (duracaoMinutos > LIMIAR_MINUTOS);
        if (typeof FieldUX !== 'undefined') {
            FieldUX.setInvalid(elDuracao, invalid);
            // Tooltip de 1s quando ultrapassar 120 minutos
            FieldUX.tooltipOnTransition(elDuracao, duracaoMinutos > LIMIAR_MINUTOS, 1000, 'tooltipDuracao');
        }
    } catch (error)
    {
        if (typeof TratamentoErroComLinha === 'function')
        {
            TratamentoErroComLinha("ViagemUpsert.js", "calcularDuracaoViagem", error);
        } else
        {
            console.error(error);
        }
    }
}

$(document).ready(function ()
{
    try
    {
        $(".esconde-diveventos").hide();

        if (ViagemId !== "00000000-0000-0000-0000-000000000000" && ViagemId != null)
        {
            $.ajax({
                type: "GET",
                url: "/api/Viagem/PegaFicha?id=" + ViagemId,  // PASSA O ID AQUI
                success: function (data)
                {
                    try
                    {
                        if (data.fichaVistoria !== null && data.fichaVistoria !== undefined)
                        {
                            $("#imgViewer").attr(
                                "src",
                                "data:image/jpg;base64," + data.fichaVistoria,
                            );
                        } else
                        {
                            $("#imgViewer").attr("src", "/Images/FichaAmarelaNova.jpg");
                        }
                    }
                    catch (error)
                    {
                        TratamentoErroComLinha(__scriptName, "ajax.PegaFicha.success", error);
                    }
                },
                error: function (data)
                {
                    try
                    {
                        console.log("Error:", data);
                    }
                    catch (error)
                    {
                        TratamentoErroComLinha(__scriptName, "ajax.PegaFicha.error", error);
                    }
                },
            });
        } else
        {
            const origin = window.location.origin;
            $("#imgViewer").attr("src", "/Images/FichaAmarelaNova.jpg");

            let list = new DataTransfer();
            let file = new File(["content"], origin + "/Images/FichaAmarelaNova.jpg");
            list.items.add(file);
        }

        const viagemId = document.getElementById("txtViagemId").value;
        if (viagemId && viagemId !== "00000000-0000-0000-0000-000000000000")
        {
            $.ajax({
                type: "GET",
                url: "/api/Agenda/RecuperaViagem",
                data: { id: viagemId },
                contentType: "application/json",
                dataType: "json",
                success: function (response)
                {
                    try
                    {
                        ExibeViagem(response.data);
                    }
                    catch (error)
                    {
                        TratamentoErroComLinha(__scriptName, "ajax.RecuperaViagem.success", error);
                    }
                },
            });
        } else
        {
            const agora = new Date();
            const dataAtual = moment().format("YYYY-MM-DD");
            const horaAtual = agora.toTimeString().split(":").slice(0, 2).join(":");

            $("#txtDataInicial").val(dataAtual);
            $("#txtHoraInicial").val(horaAtual);
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "document.ready", error);
    }
});

//=================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: ExibeViagem
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : [PORQUÊ] Popular a tela com os dados da viagem recuperada.
 *                   [O QUE] Preenche campos, aplica regras de status e habilita/desabilita UI.
 *                   [COMO] Usa dados da API para setar combos, inputs e estados dos controles.
 *
 * 📥 ENTRADAS     : viagem [Object] - DTO retornado por /api/Agenda/RecuperaViagem.
 *
 * 📤 SAÍDAS       : UI preenchida e controles ajustados.
 *
 * ⬅️ CHAMADO POR  : ajax.RecuperaViagem.success.
 *
 * ➡️ CHAMA        : atualizarDetalhesEventoSelecionado(), limparDetalhesEventoSelecionado().
 ****************************************************************************************/
function ExibeViagem(viagem)
{
    try
    {
        console.log("ExibeViagem - status:", viagem.status, "viagem:", viagem);
        
        $("#btnSubmit").hide();

        var ddlFinalidade = $("#ddlFinalidade").data("kendoDropDownList");
        if (ddlFinalidade) ddlFinalidade.value(viagem.finalidade);

        if (viagem.eventoId != null)
        {
            const ddlEvento = $("#ddlEvento").data("kendoDropDownList");
            if (ddlEvento)
            {
                ddlEvento.enable(true);
                ddlEvento.value(viagem.eventoId);
            }
            document.getElementById("btnEvento").style.display = "block";
            $(".esconde-diveventos").show();
            atualizarDetalhesEventoSelecionado(viagem.eventoId);
        } else
        {
            const ddlEvento = $("#ddlEvento").data("kendoDropDownList");
            if (ddlEvento)
            {
                ddlEvento.value("");
                ddlEvento.enable(false);
            }
            document.getElementById("btnEvento").style.display = "none";
            $(".esconde-diveventos").hide();
            limparDetalhesEventoSelecionado();
        }

        if (viagem.setorSolicitanteId) {
            var ddtSetorWidget = $("#ddtSetor").data("kendoDropDownTree");
            if (ddtSetorWidget) ddtSetorWidget.value([viagem.setorSolicitanteId.toString()]);
        }

        if (viagem.combustivelInicial) {
            var ddlCombInicial = $("#ddlCombustivelInicial").data("kendoDropDownList");
            if (ddlCombInicial) ddlCombInicial.value(viagem.combustivelInicial);
        }

        if (viagem.combustivelFinal) {
            var ddlCombFinal = $("#ddlCombustivelFinal").data("kendoDropDownList");
            if (ddlCombFinal) ddlCombFinal.value(viagem.combustivelFinal);
        }

        $("#txtKmInicial").val(viagem.kmInicial);

        if (viagem.status === "Realizada" || viagem.status === "Cancelada")
        {
            CarregandoViagemBloqueada = true;

            $("#divPainel :input").each(function ()
            {
                try
                {
                    $(this).prop("disabled", true);
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha(
                        "ViagemUpsert.js",
                        "callback@$.each#0",
                        error,
                    );
                }
            });

            // RTE (Kendo Editor) - desabilitar com delay para garantir que já foi inicializado
            setTimeout(function() {
                try {
                    const rteElement = document.getElementById("rte");
                    // ej2_instances RTE block removed — disableEditorUpsert() below handles Kendo Editor
                    if (typeof disableEditorUpsert === 'function') {
                        disableEditorUpsert();
                    }
                    // Garantir que o editor Kendo esteja desabilitado
                    if (typeof _kendoEditorUpsert !== 'undefined' && _kendoEditorUpsert) {
                        _kendoEditorUpsert.body.contentEditable = false;
                        $('#rte').closest('.k-editor').addClass('k-disabled');
                    }
                } catch (e) {
                    console.log("Erro ao desabilitar editor:", e);
                }
            }, 500);

            // Syncfusion: cmbVeiculo, cmbRequisitante
            ["cmbVeiculo", "cmbRequisitante"].forEach(
                (id) =>
                {
                    try
                    {
                        const control = $("#" + id).data("kendoComboBox");
                        if (control) control.enable(false);
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha(
                            "ViagemUpsert.js",
                            'callback@["cmbVeiculo", "cmbRequisitante"].forEach#0',
                            error,
                        );
                    }
                },
            );

            // Kendo: cmbOrigem, cmbDestino
            try {
                const cmbOrigem = $("#cmbOrigem").data("kendoComboBox");
                if (cmbOrigem) cmbOrigem.enable(false);
            } catch (error) {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "cmbOrigem.disable", error);
            }

            try {
                const cmbDestino = $("#cmbDestino").data("kendoComboBox");
                if (cmbDestino) cmbDestino.enable(false);
            } catch (error) {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "cmbDestino.disable", error);
            }

            // Kendo: Motorista
            try {
                const ddlMotorista = $("#cmbMotorista").data("kendoDropDownList");
                if (ddlMotorista) ddlMotorista.enable(false);
            } catch (error) {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "cmbMotorista.disable", error);
            }

            // Kendo: ddtSetor
            try {
                var ddtSetorWidget = $("#ddtSetor").data("kendoDropDownTree");
                if (ddtSetorWidget) ddtSetorWidget.enable(false);
            } catch (error) {
                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "ddtSetor.disable", error);
            }

            // Kendo: Combustível Inicial/Final
            ["ddlCombustivelInicial", "ddlCombustivelFinal"].forEach((id) =>
            {
                try
                {
                    const ddl = $("#" + id).data("kendoDropDownList");
                    if (ddl) ddl.enable(false);
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", id + ".disable", error);
                }
            });

            // Kendo: Finalidade
            var ddlFin = $("#ddlFinalidade").data("kendoDropDownList");
            if (ddlFin) ddlFin.enable(false);
            var ddlEvento = $("#ddlEvento").data("kendoDropDownList");
            if (ddlEvento) ddlEvento.enable(false);

            ["btnRequisitante", "btnSetor", "btnEvento"].forEach((id) =>
            {
                try
                {
                    const button = document.getElementById(id);
                    if (button) button.disabled = true;
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha(
                        "ViagemUpsert.js",
                        'callback@["btnRequisitante", "btnSetor", "btnEven.forEach#0',
                        error,
                    );
                }
            });

            document.getElementById("divSubmit").style.display = "none";
        } else
        {
            $("#btnSubmit").show();
        }
        
        // Calcular duração e km percorrido SEMPRE ao carregar viagem
        // Usar setTimeout maior para garantir que os campos já estejam preenchidos pelo Model Binding
        setTimeout(function() {
            calcularDuracaoViagem();
            calcularKmPercorrido();
        }, 600);

        // Função auxiliar para verificar se GUID é válido
        const isGuidValido = (guid) => {
            return guid && guid !== "00000000-0000-0000-0000-000000000000" && guid !== "";
        };
        
        // - Definir o texto da label de Agendamento;
        const lblAgendamento = document.getElementById("lblUsuarioAgendamento");
            if (lblAgendamento)
            {
                const temUsuarioAgendamento = isGuidValido(viagem.usuarioIdAgendamento);
                const dataAgendamentoValida = viagem.dataAgendamento && moment(viagem.dataAgendamento).isValid();
                
                if ((viagem.statusAgendamento || viagem.foiAgendamento) && temUsuarioAgendamento && dataAgendamentoValida)
                {
                    const DataAgendamento = moment(viagem.dataAgendamento).format("DD/MM/YYYY");
                    const HoraAgendamento = moment(viagem.dataAgendamento).format("HH:mm");
                    $.ajax({
                        url: "/api/Agenda/RecuperaUsuario",
                        type: "Get",
                        data: { id: viagem.usuarioIdAgendamento },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data)
                        {
                            try
                            {
                                let usuarioAgendamento;
                                $.each(data, function (key, val)
                                {
                                    try
                                    {
                                        usuarioAgendamento = val;
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "ViagemUpsert.js",
                                            "callback@$.each#1",
                                            error,
                                        );
                                    }
                                });
                                const lbl = document.getElementById("lblUsuarioAgendamento");
                                if (lbl && usuarioAgendamento)
                                {
                                    lbl.innerHTML =
                                        '<i class="fa-duotone fa-solid fa-user-clock"></i> ' +
                                        "<span>Agendado por:</span> " +
                                        usuarioAgendamento +
                                        " em " +
                                        DataAgendamento +
                                        " às " +
                                        HoraAgendamento;
                                    lbl.style.display = "";
                                    }
                                }
                                catch (error)
                                {
                                    //TratamentoErroComLinha("agendamento_viagem.js", "success", error);
                                    Alerta.TratamentoErroComLinha(
                                        "agendamento_viagem.js",
                                        "success",
                                        error,
                                    );
                                }
                            },
                            error: function (err)
                            {
                                try
                                {
                                    console.log(err);
                                    AppToast.show("Vermelho", "Erro ao buscar dados de agendamento", 3000);
                                }
                                catch (error)
                                {
                                    Alerta.TratamentoErroComLinha(
                                        "ViagemUpsert.js",
                                        "error",
                                        error,
                                    );
                                }
                            },
                        });
                    } else
                    {
                        lblAgendamento.innerHTML = "";
                    }
            }

            // - Definir o texto da label de Criação;
            const lblCriacao = document.getElementById("lblUsuarioCriacao");
            if (lblCriacao)
            {
                const temUsuarioCriacao = isGuidValido(viagem.usuarioIdCriacao);
                const dataCriacaoValida = viagem.dataCriacao && moment(viagem.dataCriacao).isValid();
                
                if (viagem.statusAgendamento === false && temUsuarioCriacao && dataCriacaoValida)
                {
                    const DataCriacao = moment(viagem.dataCriacao).format("DD/MM/YYYY");
                    const HoraCriacao = moment(viagem.dataCriacao).format("HH:mm");
                    $.ajax({
                        url: "/api/Agenda/RecuperaUsuario",
                        type: "Get",
                        data: { id: viagem.usuarioIdCriacao },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data)
                        {
                            try
                            {
                                let usuarioCriacao;
                                $.each(data, function (key, val)
                                {
                                    try
                                    {
                                        usuarioCriacao = val;
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "ViagemUpsert.js",
                                            "callback@$.each#1",
                                            error,
                                        );
                                    }
                                });
                                const lbl = document.getElementById("lblUsuarioCriacao");
                                if (lbl && usuarioCriacao)
                                {
                                    lbl.innerHTML =
                                        '<i class="fa-sharp-duotone fa-solid fa-user-plus"></i> ' +
                                        "<span>Criado/Alterado por:</span> " +
                                        usuarioCriacao +
                                        " em " +
                                        DataCriacao +
                                        " às " +
                                        HoraCriacao;
                                    lbl.style.display = "";
                                }
                            }
                            catch (error)
                            {
                                //TratamentoErroComLinha("agendamento_viagem.js", "success", error);
                                Alerta.TratamentoErroComLinha(
                                    "agendamento_viagem.js",
                                    "success",
                                    error,
                                );
                            }
                        },
                        error: function (err)
                        {
                            try
                            {
                                console.log(err);
                                AppToast.show("Vermelho", "Erro ao buscar dados de criação", 3000);
                            }
                            catch (error)
                            {
                                Alerta.TratamentoErroComLinha(
                                    "ViagemUpsert.js",
                                    "error",
                                    error,
                                );
                            }
                        },
                    });
                } else
                {
                    lblCriacao.innerHTML = "";
                }
            }

            // - Definir o texto da label de Finalização;
            const lblFinalizacao = document.getElementById("lblUsuarioFinalizacao");
            if (lblFinalizacao)
            {
                const temUsuarioFinalizacao = isGuidValido(viagem.usuarioIdFinalizacao);
                const dataFinalizacaoValida = viagem.dataFinalizacao && moment(viagem.dataFinalizacao).isValid();
                
                if (viagem.horaFim != null && temUsuarioFinalizacao && dataFinalizacaoValida)
                {
                    const DataFinalizacao = moment(viagem.dataFinalizacao).format("DD/MM/YYYY");
                    const HoraFinalizacao = moment(viagem.dataFinalizacao).format("HH:mm");
                    $.ajax({
                        url: "/api/Agenda/RecuperaUsuario",
                        type: "Get",
                        data: { id: viagem.usuarioIdFinalizacao },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data)
                        {
                            try
                            {
                                let usuarioFinalizacao;
                                $.each(data, function (key, val)
                                {
                                    try
                                    {
                                        usuarioFinalizacao = val;
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "ViagemUpsert.js",
                                            "callback@$.each#1",
                                            error,
                                        );
                                    }
                                });
                                const lbl = document.getElementById("lblUsuarioFinalizacao");
                                if (lbl && usuarioFinalizacao)
                                {
                                    lbl.innerHTML =
                                        '<i class="fa-duotone fa-solid fa-user-check"></i> ' +
                                        "<span>Finalizado por:</span> " +
                                        usuarioFinalizacao +
                                        " em " +
                                        DataFinalizacao +
                                        " às " +
                                        HoraFinalizacao;
                                    lbl.style.display = "";
                                }
                            }
                            catch (error)
                            {
                                //TratamentoErroComLinha("agendamento_viagem.js", "success", error);
                                Alerta.TratamentoErroComLinha(
                                    "agendamento_viagem.js",
                                    "success",
                                    error,
                                );
                            }
                        },
                        error: function (err)
                        {
                            try
                            {
                                console.log(err);
                                AppToast.show("Vermelho", "Erro ao buscar dados de finalização", 3000);
                            }
                            catch (error)
                            {
                                Alerta.TratamentoErroComLinha(
                                    "ViagemUpsert.js",
                                    "error",
                                    error,
                                );
                            }
                        },
                    });
                } else
                {
                    lblFinalizacao.innerHTML = "";
                }
            }

            // - Definir o texto da label de Cancelamento;
            const lblCancelamento = document.getElementById("lblUsuarioCancelamento");
            if (lblCancelamento)
            {
                const temUsuarioCancelamento = isGuidValido(viagem.usuarioIdCancelamento);
                const dataCancelamentoValida = viagem.dataCancelamento && moment(viagem.dataCancelamento).isValid();
                
                if (temUsuarioCancelamento && dataCancelamentoValida)
                {
                    const DataCancelamento = moment(viagem.dataCancelamento).format("DD/MM/YYYY");
                    $.ajax({
                        url: "/api/Agenda/RecuperaUsuario",
                        type: "Get",
                        data: { id: viagem.usuarioIdCancelamento },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data)
                        {
                            try
                            {
                                let usuarioCancelamento;
                                $.each(data, function (key, val)
                                {
                                    try
                                    {
                                        usuarioCancelamento = val;
                                    }
                                    catch (error)
                                    {
                                        Alerta.TratamentoErroComLinha(
                                            "ViagemUpsert.js",
                                            "callback@$.each#1",
                                            error,
                                        );
                                    }
                                });
                                const lbl = document.getElementById("lblUsuarioCancelamento");
                                if (lbl && usuarioCancelamento)
                                {
                                    lbl.innerHTML =
                                        '<i class="fa-duotone fa-regular fa-trash-can-xmark"></i> ' +
                                        "<span>Cancelado por:</span> " +
                                        usuarioCancelamento +
                                        " em " +
                                        DataCancelamento;
                                    lbl.style.display = "";
                                }
                            }
                            catch (error)
                            {
                                //TratamentoErroComLinha("agendamento_viagem.js", "success", error);
                                Alerta.TratamentoErroComLinha(
                                    "agendamento_viagem.js",
                                    "success",
                                    error,
                                );
                            }
                        },
                        error: function (err)
                        {
                            try
                            {
                                console.log(err);
                                AppToast.show("Vermelho", "Erro ao buscar dados de cancelamento", 3000);
                            }
                            catch (error)
                            {
                                Alerta.TratamentoErroComLinha(
                                    "ViagemUpsert.js",
                                    "error",
                                    error,
                                );
                            }
                        },
                    });
                } else
                {
                    lblCancelamento.innerHTML = "";
                }
            }
    }
    catch (error)
    {
        //TratamentoErroComLinha("agendamento_viagem.js", "ExibeViagem", error);
        Alerta.TratamentoErroComLinha("agendamento_viagem.js", "ExibeViagem", error);
    }
}

//========================================================================

function BuscarSetoresPorMotorista(motoristaId)
{
    try
    {
        if (!motoristaId) return;

        $.ajax({
            url: "/Setores/BuscarSetoresPorMotorista",
            data: { motoristaId: motoristaId },
            success: function (data)
            {
                try
                {
                    // [LOGICA] Recarregar DDT com novos setores (Kendo)
                    KendoDDTHelper.setFlatDataSource("#ddtSetor", data, "SetorSolicitanteId", "SetorPaiId", "Nome");
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.BuscarSetoresPorMotorista.success",
                        error,
                    );
                }
            },
            error: function (xhr)
            {
                try
                {
                    TratamentoErroComLinha(
                        "ViagemUpsert.js",
                        "BuscarSetoresPorMotorista",
                        xhr,
                    );
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.BuscarSetoresPorMotorista.error",
                        error,
                    );
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "BuscarSetoresPorMotorista", error);
    }
}

function InserirNovoRequisitante()
{
    try
    {
        const nome = $("#txtNomeRequisitante").val();
        if (!nome)
        {
            Alerta.Info("Atenção", "Informe o nome do novo requisitante.");
            return;
        }

        $.ajax({
            url: "/Requisitantes/CriarNovoRequisitante",
            type: "POST",
            data: { nome: nome },
            success: function (requisitante)
            {
                try
                {
                    const cmb = $("#cmbRequisitante").data("kendoComboBox");
                    cmb.dataSource.add(requisitante);
                    cmb.value(requisitante.id);
                    // cmb.dataBind(); // Removed: Kendo ComboBox auto-updates
                    $("#modalNovoRequisitante").modal("hide");
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.CriarNovoRequisitante.success",
                        error,
                    );
                }
            },
            error: function (xhr)
            {
                try
                {
                    Alerta.Erro("Erro", "Erro ao criar novo requisitante: " + xhr.statusText);
                }
                catch (error)
                {
                    TratamentoErroComLinha(__scriptName, "ajax.CriarNovoRequisitante.error", error);
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "InserirNovoRequisitante", error);
    }
}

// Atualizar também quando a imagem for trocada
function VisualizaImagem(input)
{
    try
    {
        if (input.files && input.files[0])
        {
            const file = input.files[0];

            // Validações
            const maxSize = 5 * 1024 * 1024; // 5MB
            if (file.size > maxSize)
            {
                AppToast.show("Amarelo", "Arquivo muito grande! Máximo: 5MB", 3000);
                input.value = "";
                return;
            }

            // Validar tipo
            const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
            if (!allowedTypes.includes(file.type))
            {
                AppToast.show("Amarelo", "Tipo de arquivo não permitido! Use JPG, PNG ou GIF.", 3000);
                input.value = "";
                return;
            }

            const reader = new FileReader();

            reader.onload = function (e)
            {
                try
                {
                    const base64String = e.target.result;

                    // Exibe a imagem no preview
                    $("#imgViewerItem").attr("src", base64String);

                    // IMPORTANTE: Armazena o Base64 no campo hidden correto
                    $("#hiddenFoto").val(base64String);

                    // Limpa o campo da imagem existente pois há uma nova
                    $("#hiddenFichaExistente").val("");

                    // Debug
                    console.log("Imagem carregada com sucesso!");
                    console.log("Base64 length:", base64String.length);
                    console.log("Campo hidden preenchido?", $("#hiddenFoto").val().length > 0);

                    AppToast.show("Verde", "Imagem carregada com sucesso", 2000);
                }
                catch (error)
                {
                    console.error("Erro no onload:", error);
                    const fallbackImg = "/images/FichaAmarelaNova.jpg";
                    $("#imgViewerItem").attr("src", fallbackImg);
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "reader.onload", error);
                }
            };

            reader.onerror = function ()
            {
                const fallbackImg = "/images/FichaAmarelaNova.jpg";
                $("#imgViewerItem").attr("src", fallbackImg);
                AppToast.show("Vermelho", "Erro ao ler arquivo!", 3000);
            };

            reader.readAsDataURL(file);
        }
        else
        {
            // Se o input foi limpo, limpa o hidden também
            $("#hiddenFoto").val("");
            console.log("Input file limpo");
        }
    }
    catch (error)
    {
        const fallbackImg = "/images/FichaAmarelaNova.jpg";
        $("#imgViewerItem").attr("src", fallbackImg);
        TratamentoErroComLinha("ViagemUpsert.js", "VisualizaImagem", error);
    }
}

//=======================================================================

function PreencheListaSetores(SetorSolicitanteId)
{
    try
    {
        $.ajax({
            url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
            method: "GET",
            datatype: "json",
            success: function (res)
            {
                try
                {
                    let SetorList = [];

                    res.data.forEach((item) =>
                    {
                        try
                        {
                            SetorList.push({
                                SetorSolicitanteId: item.setorSolicitanteId,
                                SetorPaiId: item.setorPaiId,
                                Nome: item.nome,
                                HasChild: item.hasChild,
                            });
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha(
                                "ViagemUpsert.js",
                                "callback@res.data.forEach#0",
                                error,
                            );
                        }
                    });

                    // [LOGICA] Recarregar DDT com SetorList (Kendo)
                    KendoDDTHelper.setFlatDataSource("#ddtSetor", SetorList, "SetorSolicitanteId", "SetorPaiId", "Nome");
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerAJAXPreencheListaSetores.success",
                        error,
                    );
                }
            },
        });

        // [LOGICA] Selecionar setor no DDT (Kendo) — refresh já feito em setFlatDataSource acima
        var strSetor = String(SetorSolicitanteId);
        KendoDDTHelper.setValue("#ddtSetor", [strSetor]);
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaSetores", error);
    }
}

function RequisitanteValueChange()
{
    try
    {
        var ddTreeObj = $("#cmbRequisitante").data("kendoComboBox");
        if ((ddTreeObj.value() === null || ddTreeObj.value() === "" || ddTreeObj.value() === undefined)) return;
        var requisitanteid = String(ddTreeObj.value());

        $.ajax({
            url: "/Viagens/Upsert?handler=PegaSetor",
            method: "GET",
            datatype: "json",
            data: { id: requisitanteid },
            success: function (res)
            {
                try
                {
                    KendoDDTHelper.setValue("#ddtSetor", [res.data.toString()]);
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerPegaSetor.success",
                        error,
                    );
                }
            },
        });

        $.ajax({
            url: "/Viagens/Upsert?handler=PegaRamal",
            method: "GET",
            datatype: "json",
            data: { id: requisitanteid },
            success: function (res)
            {
                try
                {
                    document.getElementById("txtRamalRequisitante").value = res.data;
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerPegaRamal.success",
                        error,
                    );
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "RequisitanteValueChange", error);
    }
}

function RequisitanteEventoValueChange()
{
    try
    {
        var ddTreeObj = getComboEJ2("lstRequisitanteEvento");
        if (ddTreeObj.value === null) return;
        var requisitanteid = String(ddTreeObj.value);

        $.ajax({
            url: "/Viagens/Upsert?handler=PegaSetor",
            method: "GET",
            datatype: "json",
            data: { id: requisitanteid },
            success: function (res)
            {
                try
                {
                    getComboEJ2("ddtSetorRequisitanteEvento").value = [
                        res.data,
                    ];
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerPegaSetor.success",
                        error,
                    );
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha(
            "ViagemUpsert.js",
            "RequisitanteEventoValueChange",
            error,
        );
    }
}

function MotoristaValueChange()
{
    try
    {
        var ddlMotorista = $("#cmbMotorista").data("kendoDropDownList");
        if (!ddlMotorista || !ddlMotorista.value()) return;

        var motoristaid = String(ddlMotorista.value());

        $.ajax({
            url: "/Viagens/Upsert?handler=VerificaMotoristaViagem",
            method: "GET",
            datatype: "json",
            data: { id: motoristaid },
            success: function (res)
            {
                try
                {
                    var viajando = res.data;
                    console.log("Motorista Viajando:", viajando);

                    if (viajando)
                    {
                        AppToast.show(
                            "amarelo",
                            "Este motorista encontra-se em uma viagem não terminada!",
                            5000
                        );
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerVerificaMotoristaViagem.success",
                        error,
                    );
                }
            },
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "MotoristaValueChange", error);
    }
}

function VeiculoValueChange()
{
    try
    {
        var ddTreeObj = $("#cmbVeiculo").data("kendoComboBox");
        console.log("Objeto Veículo:", ddTreeObj);

        if ((ddTreeObj.value() === null || ddTreeObj.value() === "" || ddTreeObj.value() === undefined))
        {
            // Desabilita o botão de ocorrências quando não há veículo selecionado
            desabilitarBotaoOcorrenciasVeiculo();

            // Desabilitar seção de ocorrências da viagem
            controlarSecaoOcorrencias(null);

            // ✅ NOVO: Desabilitar campos de Km quando não há veículo selecionado
            $("#txtKmInicial").prop("disabled", true).val("");
            $("#txtKmFinal").prop("disabled", true).val("");
            $("#txtKmPercorrido").val("");
            $("#txtKmAtual").val("");

            return;
        }

        var veiculoid = String(ddTreeObj.value());

        // Habilitar seção de ocorrências da viagem
        controlarSecaoOcorrencias(veiculoid);

        // ✅ NOVO: Habilitar campo Km Inicial quando veículo é selecionado
        $("#txtKmInicial").prop("disabled", false);
        // Km Final continua desabilitado até que Km Inicial seja preenchido

        $.ajax({
            url: "/Viagens/Upsert?handler=VerificaVeiculoViagem",
            method: "GET",
            datatype: "json",
            data: { id: veiculoid },
            success: function (res)
            {
                try
                {
                    var viajando = res.data;
                    console.log("Veículo Viajando:", viajando);

                    if (viajando)
                    {
                        AppToast.show(
                            "amarelo",
                            "Este veículo encontra-se em uma viagem não terminada!",
                            5000
                        );
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerVerificaVeiculoViagem.success",
                        error,
                    );
                }
            },
        });

        $.ajax({
            url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
            method: "GET",
            datatype: "json",
            data: { id: veiculoid },
            success: function (res)
            {
                try
                {
                    var km = res.data;
                    document.getElementById("txtKmAtual").value = km;
                    document.getElementById("txtKmInicial").value = km;
                    if (km === 0 || km === "0" || km === null)
                    {
                        AppToast.show(
                            "amarelo",
                            "Este veículo está sem Quilometragem Atual!",
                            5000
                        );
                        document.getElementById("txtKmAtual").value = "";
                        document.getElementById("txtKmInicial").value = "";
                        document.getElementById("txtKmFinal").value = "";
                        var combo = $("#cmbVeiculo").data("kendoComboBox");

                        // 1️⃣ Método oficial Syncfusion
                        combo.input.focus(); // leva o foco para o input do ComboBox
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerPegaKmAtualVeiculo.success",
                        error,
                    );
                }
            },
        });

        // ✅ NOVO: Verificar ocorrências em aberto do veículo
        verificarOcorrenciasVeiculo(veiculoid);
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "VeiculoValueChange", error);
    }
}

$("#btnInserirEvento").click(function (e)
{
    try
    {
        e.preventDefault();

        if (
            $("#txtNomeDoEvento").val() === "" ||
            $("#txtDescricao").val() === "" ||
            $("#txtDataInicialEvento").val() === "" ||
            $("#txtDataFinalEvento").val() === "" ||
            $("#txtQtdPessoas").val() === ""
        )
        {
            AppToast.show("amarelo", "Todos os campos são obrigatórios!", 5000);
            return;
        }

        let setores = getComboEJ2("ddtSetorRequisitanteEvento");
        let requisitantes = getComboEJ2("lstRequisitanteEvento");

        if (!setores.value || !requisitantes.value)
        {
            AppToast.show("amarelo", "Setor e Requisitante são obrigatórios!", 5000);
            return;
        }

        let objEvento = JSON.stringify({
            Nome: $("#txtNomeDoEvento").val(),
            Descricao: $("#txtDescricaoEvento").val(),
            SetorSolicitanteId: setores.value.toString(),
            RequisitanteId: requisitantes.value.toString(),
            QtdParticipantes: $("#txtQtdPessoas").val(),
            DataInicial: moment($("#txtDataInicialEvento").val()).format("MM-DD-YYYY"),
            DataFinal: moment($("#txtDataFinalEvento").val()).format("MM-DD-YYYY"),
            Status: "1",
        });

        $.ajax({
            type: "POST",
            url: "/api/Viagem/AdicionarEvento",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: objEvento,
            success: function (data)
            {
                try
                {
                    AppToast.show('Verde', data.message);
                    PreencheListaEventos(data.eventoId);
                    $("#modalEvento").hide();
                }
                catch (error)
                {
                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarEvento.success", error);
                }
            },
            error: function (data)
            {
                try
                {
                    AppToast.show("Vermelho", "Erro ao adicionar evento", 3000);
                    console.log(data);
                }
                catch (error)
                {
                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarEvento.error", error);
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "click.btnInserirEvento", error);
    }
});

//============================================================================================================

$("#btnInserirRequisitante").click(function (e)
{
    try
    {
        e.preventDefault();

        if (
            $("#txtPonto").val() === "" ||
            $("#txtNome").val() === "" ||
            $("#txtRamal").val() === ""
        )
        {
            AppToast.show("amarelo", "Ponto, Nome e Ramal são obrigatórios!", 5000);
            return;
        }

        let setores = getComboEJ2("ddtSetorRequisitante");
        if (!setores.value)
        {
            AppToast.show("amarelo", "O Setor do Requisitante é obrigatório!", 5000);
            return;
        }

        let objRequisitante = JSON.stringify({
            Nome: $("#txtNome").val(),
            Ponto: $("#txtPonto").val(),
            Ramal: $("#txtRamal").val(),
            Email: $("#txtEmail").val(),
            SetorSolicitanteId: setores.value.toString(),
        });

        $.ajax({
            type: "POST",
            url: "/api/Viagem/AdicionarRequisitante",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: objRequisitante,
            success: function (data)
            {
                try
                {
                    if (data.success)
                    {
                        AppToast.show('Verde', data.message);
                        $("#cmbRequisitante").data("kendoComboBox").dataSource.add(
                            {
                                RequisitanteId: data.requisitanteid,
                                Requisitante: $("#txtNome").val() + " - " + $("#txtPonto").val(),
                            },
                            0,
                        );
                        $("#modalRequisitante").hide();
                        $(".modal-backdrop").remove();
                        $("body").removeClass("modal-open").css("overflow", "auto");
                        $("#btnFecharRequisitante").click();
                    } else
                    {
                        AppToast.show('Vermelho', data.message);
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.AdicionarRequisitante.success",
                        error,
                    );
                }
            },
            error: function (data)
            {
                try
                {
                    Alerta.Erro("Atenção", "Já existe um requisitante com este ponto/nome!");
                    console.log(data);
                }
                catch (error)
                {
                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarRequisitante.error", error);
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "click.btnInserirRequisitante", error);
    }
});

$("#btnInserirSetor").click(function (e)
{
    try
    {
        e.preventDefault();

        if ($("#txtNomeSetor").val() === "" || $("#txtRamalSetor").val() === "")
        {
            AppToast.show("amarelo", "Nome e Ramal do Setor são obrigatórios!", 5000);
            return;
        }

        let setorPaiId = null;
        let setorPai = getComboEJ2("ddtSetorPai").value;
        if (setorPai !== "" && setorPai !== null)
        {
            setorPaiId = setorPai.toString();
        }

        let objSetorData = {
            Nome: $("#txtNomeSetor").val(),
            Ramal: $("#txtRamalSetor").val(),
            Sigla: $("#txtSigla").val(),
        };

        if (setorPaiId)
        {
            objSetorData["SetorPaiId"] = setorPaiId;
        }

        let objSetor = JSON.stringify(objSetorData);

        $.ajax({
            type: "POST",
            url: "/api/Viagem/AdicionarSetor",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: objSetor,
            success: function (data)
            {
                try
                {
                    AppToast.show('Verde', data.message);
                    PreencheListaSetores(data.setorId);
                    $("#modalSetor").hide();
                    $(".modal-backdrop").remove();
                    $("body").removeClass("modal-open");
                    $("body").css("overflow", "auto");
                }
                catch (error)
                {
                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarSetor.success", error);
                }
            },
            error: function (data)
            {
                try
                {
                    AppToast.show("Vermelho", "Erro ao adicionar setor", 3000);
                    console.log(data);
                }
                catch (error)
                {
                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarSetor.error", error);
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "click.btnInserirSetor", error);
    }
});

$("#btnSubmit").click(async function (event)
{
    try
    {
        event.preventDefault();

        // NoFichaVistoria não é mais obrigatório - se vazio, será gravado como 0 (viagem mobile)

        if ($("#txtDataInicial").val() === "")
        {
            Alerta.Erro("Informação Ausente", "A Data Inicial é obrigatória");
            return;
        }

        if ($("#txtHoraInicial").val() === "")
        {
            Alerta.Erro("Informação Ausente", "A Hora Inicial é obrigatória");
            return;
        }

        const ddlFinalidadeVal = $("#ddlFinalidade").data("kendoDropDownList");
        if (!ddlFinalidadeVal || !ddlFinalidadeVal.value())
        {
            Alerta.Erro("Informação Ausente", "A Finalidade é obrigatória");
            return;
        }

        const cmbOrigem = $("#cmbOrigem").data("kendoComboBox");
        if (!cmbOrigem || !cmbOrigem.value())
        {
            Alerta.Erro("Informação Ausente", "A Origem é obrigatória");
            return;
        }

        const ddlMotorista = $("#cmbMotorista").data("kendoDropDownList");
        if (!ddlMotorista || !ddlMotorista.value())
        {
            Alerta.Erro("Informação Ausente", "O Motorista é obrigatório");
            return;
        }

        const veiculo = $("#cmbVeiculo").data("kendoComboBox");
        if ((veiculo.value() === null || veiculo.value() === "" || veiculo.value() === undefined))
        {
            Alerta.Erro("Informação Ausente", "O Veículo é obrigatório");
            return;
        }

        if ($("#txtKmInicial").val() === "")
        {
            Alerta.Erro("Informação Ausente", "A Quilometragem Inicial é obrigatória");
            return;
        }

        const ddlCombInicialVal = $("#ddlCombustivelInicial").data("kendoDropDownList");
        if (!ddlCombInicialVal || !ddlCombInicialVal.value())
        {
            Alerta.Erro("Informação Ausente", "O Nível de Combustível Inicial é obrigatório");
            return;
        }

        const requisitante = $("#cmbRequisitante").data("kendoComboBox");
        if ((!requisitante.value() || requisitante.value() === "") || requisitante.value() === null)
        {
            Alerta.Erro("Informação Ausente", "O Requisitante é obrigatório");
            return;
        }

        if ($("#txtRamalRequisitante").val() === "")
        {
            Alerta.Erro("Informação Ausente", "O Ramal do Requisitante é obrigatório");
            return;
        }

        var setorVal = KendoDDTHelper.getValue("#ddtSetor");
        if (!setorVal)
        {
            Alerta.Erro("Informação Ausente", "O Setor Solicitante é obrigatório");
            return;
        }

        const dataFinal = $("#txtDataFinal").val();
        const horaFinal = $("#txtHoraFinal").val();
        const ddlCombFinalVal = $("#ddlCombustivelFinal").data("kendoDropDownList");
        const combustivelFinal = ddlCombFinalVal ? ddlCombFinalVal.value() : null;
        const kmFinal = $("#txtKmFinal").val();

        // VALIDAÇÃO: Data Final não pode ser superior à data atual
        if (dataFinal)
        {
            const dataFinalDate = new Date(dataFinal + "T00:00:00");
            const hoje = new Date();
            hoje.setHours(0, 0, 0, 0);
            if (dataFinalDate > hoje)
            {
                $("#txtDataFinal").val("");
                $("#txtDataFinal").focus();
                AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
                return;
            }
        }

        const algumFinalPreenchido = dataFinal || horaFinal || combustivelFinal || kmFinal;
        const todosFinalPreenchidos = dataFinal && horaFinal && combustivelFinal && kmFinal;

        if (kmFinal && parseFloat(kmFinal) <= 0)
        {
            Alerta.Erro("Informação Incorreta", "A Quilometragem Final deve ser maior que zero");
            return;
        }

        if (algumFinalPreenchido && !todosFinalPreenchidos)
        {
            Alerta.Erro(
                "Informação Incompleta",
                "Todos os campos de Finalização devem ser preenchidos para encerrar a viagem",
            );
            return;
        }

        if (todosFinalPreenchidos)
        {
            const confirmacao = await Alerta.Confirmar(
                "Confirmar Fechamento",
                'Você está criando a viagem como "Realizada". Deseja continuar?',
                "Sim, criar!",
                "Cancelar",
            );

            if (!confirmacao)
            {
                return;
            }
        }

        const datasOk = await validarDatasInicialFinal(
            $("#txtDataInicial").val(),
            $("#txtDataFinal").val(),
        );
        if (!datasOk)
        {
            return;
        }

        const kmOk = await validarKmInicialFinal();
        if (!kmOk)
        {
            return;
        }

        // VALIDAÇÃO IA CONSOLIDADA - Verifica se há alertas pendentes ao finalizar viagem
        if (todosFinalPreenchidos && typeof window.validarFinalizacaoConsolidadaIA === 'function')
        {
            const veiculoId = ($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null) || '';

            const iaValida = await window.validarFinalizacaoConsolidadaIA({
                dataInicial: $("#txtDataInicial").val(),
                horaInicial: $("#txtHoraInicial").val(),
                dataFinal: dataFinal,
                horaFinal: horaFinal,
                kmInicial: parseInt($("#txtKmInicial").val()) || 0,
                kmFinal: parseInt(kmFinal) || 0,
                veiculoId: veiculoId
            });

            if (!iaValida)
            {
                return;
            }
        }

        $("#btnSubmit").prop("disabled", true);
        $("#btnEscondido").click();
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "click.btnSubmit", error);
    }
});

//========================================================================================================

$("#txtNoFichaVistoria").focusout(async function ()
{
    try
    {
        let noFicha = $("#txtNoFichaVistoria").val();
        if (noFicha === "") return;

        $.ajax({
            url: "/Viagens/Upsert?handler=VerificaFicha",
            method: "GET",
            datatype: "json",
            data: { id: noFicha },
            success: async function (res)
            {
                try
                {
                    let maxFicha = parseInt(res.data);
                    if (noFicha > maxFicha + 100 || noFicha < maxFicha - 100)
                    {
                        const confirmado = await Alerta.Confirmar(
                            "Ficha Divergente",
                            "O número inserido difere em ±100 da última Ficha inserida! Tem certeza?",
                            "Tenho certeza! 💪🏼",
                            "Me enganei! 😟'",
                        );

                        if (!confirmado)
                        {
                            document.getElementById("txtNoFichaVistoria").value = "";
                            document.getElementById("txtNoFichaVistoria").focus();
                            return;
                        }
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerVerificaFicha.success",
                        error,
                    );
                }
            },
        });

        $.ajax({
            url: "/Viagens/Upsert?handler=FichaExistente",
            method: "GET",
            datatype: "json",
            data: { id: noFicha },
            success: async function (res)
            {
                try
                {
                    if (res.data === true)
                    {
                        await window.SweetAlertInterop.ShowPreventionAlert(
                            "Já existe uma Ficha inserida com esta numeração!",
                        );
                    }
                }
                catch (error)
                {
                    TratamentoErroComLinha(
                        __scriptName,
                        "ajax.UpserthandlerFichaExistente.success",
                        error,
                    );
                }
            },
        });
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtNoFichaVistoria", error);
    }
});

// ========================= KM: calcularKmPercorrido =========================
function calcularKmPercorrido()
{
    try
    {
        var elKmInicial = document.getElementById("txtKmInicial");
        var elKmFinal = document.getElementById("txtKmFinal");
        var elKmPercorrido = document.getElementById("txtKmPercorrido");
        
        console.log("calcularKmPercorrido - KmInicial:", elKmInicial?.value, "KmFinal:", elKmFinal?.value, "Status:", window.viagemStatus);
        
        if (!elKmInicial || !elKmFinal || !elKmPercorrido) return;

        // Só calcular Km Percorrido para viagens com status "Realizada"
        // Para viagens Abertas/Canceladas, não faz sentido calcular
        if (window.viagemStatus !== "Realizada")
        {
            elKmPercorrido.value = "";
            if (typeof FieldUX !== 'undefined') {
                FieldUX.setInvalid(elKmPercorrido, false);
                FieldUX.setHigh(elKmPercorrido, false);
                FieldUX.tooltipOnTransition(elKmPercorrido, false, 1000, 'tooltipKm');
            }
            return;
        }

        var kmInicial = parseFloat((elKmInicial.value || '').replace(",", "."));
        var kmFinal = parseFloat((elKmFinal.value || '').replace(",", "."));
        if (isNaN(kmInicial) || isNaN(kmFinal))
        {
            elKmPercorrido.value = "";
            if (typeof FieldUX !== 'undefined') {
                FieldUX.setInvalid(elKmPercorrido, false);
                FieldUX.setHigh(elKmPercorrido, false);
                FieldUX.tooltipOnTransition(elKmPercorrido, false, 1000, 'tooltipKm');
            }
            return;
        }

        var diff = kmFinal - kmInicial;
        elKmPercorrido.value = diff;

        // Regras KM
        var invalid = (diff < 0 || diff > 100);
        var high = (diff >= 50 && diff < 100);
        if (typeof FieldUX !== 'undefined') {
            FieldUX.setInvalid(elKmPercorrido, invalid);
            FieldUX.setHigh(elKmPercorrido, high);
            // Tooltip de 1s quando ultrapassar 100
            FieldUX.tooltipOnTransition(elKmPercorrido, diff > 100, 1000, 'tooltipKm');
        }
    } catch (error)
    {
        if (typeof TratamentoErroComLinha === 'function')
        {
            TratamentoErroComLinha("ViagemUpsert.js", "calcularKmPercorrido", error);
        } else
        {
            console.error(error);
        }
    }
}

["input", "focusout", "change"].forEach((evt) =>
{
    try
    {
        return document.getElementById("txtKmFinal")?.addEventListener(evt, calcularKmPercorrido);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha(
            "ViagemUpsert.js",
            'callback@["input", "focusout", "change"].forEach#0',
            error,
        );
    }
});

["input", "focusout", "change"].forEach((evt) =>
{
    try
    {
        return document
            .getElementById("txtHoraFinal")
            ?.addEventListener(evt, calcularDuracaoViagem);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha(
            "ViagemUpsert.js",
            'callback@["input", "focusout", "change"].forEach#0',
            error,
        );
    }
});

["input", "focusout", "change"].forEach((evt) =>
{
    try
    {
        return document
            .getElementById("txtKmPercorrido")
            ?.addEventListener(evt, calcularKmPercorrido);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha(
            "ViagemUpsert.js",
            'callback@["input", "focusout", "change"].forEach#0',
            error,
        );
    }
});

window.addEventListener("load", () =>
{
    try
    {
        const duracaoInput = document.getElementById("txtDuracao");
        if (duracaoInput)
        {
            duracaoInput.addEventListener(
                "focus",
                () =>
                {
                    try
                    {
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha(
                            "ViagemUpsert.js",
                            "callback@duracaoInput.addEventListener#1",
                            error,
                        );
                    }
                }, //    tooltipDuracao.open(duracaoInput);
                //    setTimeout(() => tooltipDuracao.close(), 2000);
            );
        }
        const percorridoInput = document.getElementById("txtKmPercorrido");
        if (percorridoInput)
        {
            percorridoInput.addEventListener(
                "focus",
                () =>
                {
                    try
                    {
                    }
                    catch (error)
                    {
                        Alerta.TratamentoErroComLinha(
                            "ViagemUpsert.js",
                            "callback@percorridoInput.addEventListener#1",
                            error,
                        );
                    }
                }, //    tooltipKm.open(percorridoInput);
                //    setTimeout(() => tooltipKm.close(), 2000);
            );
        }
    }
    catch (error)
    {
        TratamentoErroComLinha("ViagemUpsert.js", "load.window", error);
    }
});

var textBoxNoFichaVistoria = new ej.inputs.TextBox({
    input: function (args)
    {
        try
        {
            const value = args.event.target.value;

            // Remove qualquer caractere não numérico (exceto "-")
            args.event.target.value = value.replace(/[^\d-]/g, "");

            // Impede múltiplos sinais de "-"
            if ((value.match(/-/g) || []).length > 1 || value.indexOf("-") > 0)
            {
                args.event.target.value = value.replace(/-/g, "");
            }

            // Limite para inteiro de 32 bits
            const num = parseInt(args.event.target.value, 10);
            if (!isNaN(num))
            {
                if (num > 2147483647)
                {
                    args.event.target.value = "2147483647";
                } else if (num < -2147483648)
                {
                    args.event.target.value = "-2147483648";
                }
            }
        }
        catch (error)
        {
            TratamentoErroComLinha(
                "ViagemUpsert.js",
                "textBoxNoFichaVistoria.input",
                error,
            );
        }
    },
});
textBoxNoFichaVistoria.appendTo("#txtNoFichaVistoria");

var textBoxKmInicial = new ej.inputs.TextBox({
    input: function (args)
    {
        try
        {
            const value = args.event.target.value;

            // Remove qualquer caractere não numérico (exceto "-")
            args.event.target.value = value.replace(/[^\d-]/g, "");

            // Impede múltiplos sinais de "-"
            if ((value.match(/-/g) || []).length > 1 || value.indexOf("-") > 0)
            {
                args.event.target.value = value.replace(/-/g, "");
            }

            // Limite para inteiro de 32 bits
            const num = parseInt(args.event.target.value, 10);
            if (!isNaN(num))
            {
                if (num > 2147483647)
                {
                    args.event.target.value = "2147483647";
                } else if (num < -2147483648)
                {
                    args.event.target.value = "-2147483648";
                }
            }
        }
        catch (error)
        {
            TratamentoErroComLinha("ViagemUpsert.js", "textBoxKmInicial.input", error);
        }
    },
});
textBoxKmInicial.appendTo("#txtKmInicial");

var textBoxKmFinal = new ej.inputs.TextBox({
    input: function (args)
    {
        try
        {
            const value = args.event.target.value;

            // Remove qualquer caractere não numérico (exceto "-")
            args.event.target.value = value.replace(/[^\d-]/g, "");

            // Impede múltiplos sinais de "-"
            if ((value.match(/-/g) || []).length > 1 || value.indexOf("-") > 0)
            {
                args.event.target.value = value.replace(/-/g, "");
            }

            // Limite para inteiro de 32 bits
            const num = parseInt(args.event.target.value, 10);
            if (!isNaN(num))
            {
                if (num > 2147483647)
                {
                    args.event.target.value = "2147483647";
                } else if (num < -2147483648)
                {
                    args.event.target.value = "-2147483648";
                }
            }
        }
        catch (error)
        {
            TratamentoErroComLinha("ViagemUpsert.js", "textBoxKmFinal.input", error);
        }
    },
});
textBoxKmFinal.appendTo("#txtKmFinal");

// =============== Helper Único p/ UI dos campos (KM e Duração) ===============
(function ()
{
    // Garante instância da Syncfusion Tooltip no elemento (reaproveita se já existir)
    function ensureTooltip(el, globalName)
    {
        // 1) Já tem global?
        if (globalName && window[globalName] && typeof window[globalName].open === 'function')
        {
            return window[globalName];
        }
        // 2) Já está anexada ao elemento?
        if (el && el.ej2_instances && el.ej2_instances.length)
        {
            for (var i = 0; i < el.ej2_instances.length; i++)
            {
                var inst = el.ej2_instances[i];
                if (inst && typeof inst.open === 'function' && typeof inst.close === 'function')
                {
                    if (globalName) window[globalName] = inst;
                    return inst;
                }
            }
        }
        // 3) Cria (se EJ2 estiver disponível)
        if (window.ej && ej.popups && typeof ej.popups.Tooltip === 'function')
        {
            var Tooltip = ej.popups.Tooltip;
            var content = el.getAttribute('data-ejtip') || 'Valor acima do limite.';
            var inst = new Tooltip({
                content: content,
                opensOn: 'Custom',
                position: 'TopCenter'
            });
            inst.appendTo(el);
            if (globalName) window[globalName] = inst;
            return inst;
        }
        return null; // biblioteca não carregada
    }

    // Aplica/Remove estado inválido + pinta o texto (vermelho p/ inválido, preto p/ válido)
    function setInvalid(el, invalid)
    {
        if (!el) return;

        // 1) Marca o próprio campo
        if (el.classList)
        {
            el.classList.toggle('is-invalid', !!invalid);
        } else
        {
            var cls = el.className || '';
            var has = /\bis-invalid\b/.test(cls);
            if (invalid && !has) el.className = (cls + ' is-invalid').trim();
            if (!invalid && has) el.className = cls.replace(/\bis-invalid\b/, '').replace(/\s{2,}/g, ' ').trim();
        }
        try { el.setAttribute('aria-invalid', String(!!invalid)); } catch (e) { }

        // 2) Pinta o texto (vermelho inválido / preto válido)
        try { el.style.color = invalid ? 'var(--ftx-invalid, #dc3545)' : 'black'; } catch (e) { }

        // 3) Tenta marcar o WRAPPER do EJ2 (se existir) p/ o CSS aplicar o glow no container
        try
        {
            var wrapper = el.closest('.e-input-group, .e-float-input, .e-control-wrapper');
            if (wrapper && wrapper.classList)
            {
                wrapper.classList.toggle('is-invalid', !!invalid);
            }
        } catch (e) { /* silencioso */ }
    }

    // Aplica/Remove estado alto (apenas para KM)
    function setHigh(el, high)
    {
        if (!el) return;
        if (el.classList)
        {
            el.classList.toggle('is-high', !!high);
        } else
        {
            var cls = el.className || '';
            var has = /\bis-high\b/.test(cls);
            if (high && !has) el.className = (cls + ' is-high').trim();
            if (!high && has) el.className = cls.replace(/\bis-high\b/, '').replace(/\s{2,}/g, ' ').trim();
        }
    }

    // Abre tooltip por N ms somente na transição false -> true da condição
    function tooltipOnTransition(el, condition, ms, globalName)
    {
        if (!el) return;
        var key = '_prevCond_' + (globalName || 'tt');
        var prev = !!el[key];
        var now = !!condition;

        if (now && !prev)
        {
            var tip = ensureTooltip(el, globalName);
            if (tip && typeof tip.open === 'function')
            {
                tip.open(el);
                clearTimeout(el._tipTimer);
                el._tipTimer = setTimeout(function ()
                {
                    if (tip && typeof tip.close === 'function') tip.close();
                }, ms || 1000); // default 1s
            }
        }
        el[key] = now;
    }

    // Expõe globalmente
    window.FieldUX = {
        ensureTooltip: ensureTooltip,
        setInvalid: setInvalid,
        setHigh: setHigh,
        tooltipOnTransition: tooltipOnTransition
    };
})();

// Copia o src da miniatura ao abrir (BS5)
(function ()
{
    const modalEl = document.getElementById('modalZoom');
    const viewer = document.getElementById('imgViewer');
    const zoomed = document.getElementById('imgZoomed');

    if (!modalEl) return;

    modalEl.addEventListener('show.bs.modal', function ()
    {
        if (viewer && zoomed) zoomed.src = viewer.getAttribute('src') || '';
    });
})();

function salvarFormulario()
{
    const fileInput = document.getElementById('txtFile');

    if (fileInput.files && fileInput.files[0])
    {
        const reader = new FileReader();

        reader.onload = function (e)
        {
            // Adiciona a imagem como base64 em um campo hidden
            $('#hiddenFoto').val(e.target.result);

            // Submete o formulário
            $('form').submit();
        };

        reader.readAsDataURL(fileInput.files[0]);
    } else
    {
        // Se não tem arquivo novo, apenas submete
        $('form').submit();
    }

    return false; // Previne submit duplo
}

/* =========================================================================================
   OCORRÊNCIAS DO VEÍCULO - Funções para exibir ocorrências em aberto
   ========================================================================================= */

// Variável para armazenar o ID do veículo selecionado
var _veiculoIdSelecionado = null;
var _qtdOcorrenciasVeiculo = 0;

// Verifica se o veículo possui ocorrências em aberto
function verificarOcorrenciasVeiculo(veiculoId)
{
    try
    {
        _veiculoIdSelecionado = veiculoId;

        $.ajax({
            url: '/api/OcorrenciaViagem/VerificarOcorrenciasVeiculo',
            type: 'GET',
            data: { veiculoId: veiculoId },
            success: function (response)
            {
                try
                {
                    if (response.success)
                    {
                        _qtdOcorrenciasVeiculo = response.quantidade || 0;

                        if (response.temOcorrencias)
                        {
                            habilitarBotaoOcorrenciasVeiculo(response.quantidade);
                        }
                        else
                        {
                            desabilitarBotaoOcorrenciasVeiculo();
                        }
                    }
                    else
                    {
                        desabilitarBotaoOcorrenciasVeiculo();
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarOcorrenciasVeiculo.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                console.error('Erro ao verificar ocorrências:', error);
                desabilitarBotaoOcorrenciasVeiculo();
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarOcorrenciasVeiculo", error);
    }
}

// Habilita o botão de ocorrências e mostra quantidade
function habilitarBotaoOcorrenciasVeiculo(quantidade)
{
    try
    {
        const $btn = $('#btnOcorrenciasVeiculo');
        if ($btn.length)
        {
            $btn.removeClass('disabled').prop('disabled', false);
            $btn.attr('title', `${quantidade} ocorrência(s) em aberto`);
            
            // Atualiza o badge de quantidade
            const $badge = $('#badgeOcorrenciasVeiculo');
            if ($badge.length)
            {
                $badge.text(quantidade).show();
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "habilitarBotaoOcorrenciasVeiculo", error);
    }
}

// Desabilita o botão de ocorrências
function desabilitarBotaoOcorrenciasVeiculo()
{
    try
    {
        _qtdOcorrenciasVeiculo = 0;
        
        const $btn = $('#btnOcorrenciasVeiculo');
        if ($btn.length)
        {
            $btn.addClass('disabled').prop('disabled', true);
            $btn.attr('title', 'Nenhuma ocorrência em aberto');
            
            // Esconde o badge
            const $badge = $('#badgeOcorrenciasVeiculo');
            if ($badge.length)
            {
                $badge.hide();
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "desabilitarBotaoOcorrenciasVeiculo", error);
    }
}

// Clique no botão de ocorrências do veículo
$(document).on('click', '#btnOcorrenciasVeiculo:not(.disabled)', function (e)
{
    try
    {
        e.preventDefault();

        if (!_veiculoIdSelecionado)
        {
            AppToast.show('Amarelo', 'Selecione um veículo primeiro', 3000);
            return;
        }

        const modalEl = document.getElementById('modalOcorrenciasVeiculoUpsert');
        if (!modalEl) return;

        // Guarda o ID no modal
        modalEl.setAttribute('data-veiculo-id', String(_veiculoIdSelecionado));

        // Pega o texto do veículo selecionado
        const ddTreeObj = $("#cmbVeiculo").data("kendoComboBox");
        const textoVeiculo = ddTreeObj.text() || 'Veículo';

        // Atualiza o título do modal
        const tituloSpan = modalEl.querySelector('#modalOcorrenciasVeiculoUpsertLabel span');
        if (tituloSpan)
        {
            tituloSpan.textContent = `Ocorrências em Aberto - ${textoVeiculo}`;
        }

        // Abre o modal
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btnOcorrenciasVeiculo", error);
    }
});

// Quando o modal de ocorrências do veículo é aberto
$('#modalOcorrenciasVeiculoUpsert').on('shown.bs.modal', function (e)
{
    try
    {
        const modalEl = this;
        const veiculoId = modalEl.getAttribute('data-veiculo-id');

        if (!veiculoId)
        {
            console.error('VeiculoId não encontrado');
            return;
        }

        // Reseta a tabela
        $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center"><i class="fa fa-spinner fa-spin"></i> Carregando...</td></tr>');

        // Busca as ocorrências do veículo
        carregarOcorrenciasVeiculoUpsert(veiculoId);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "modalOcorrenciasVeiculoUpsert.shown", error);
    }
});

// Carrega as ocorrências do veículo via API
function carregarOcorrenciasVeiculoUpsert(veiculoId)
{
    try
    {
        $.ajax({
            url: '/api/OcorrenciaViagem/ListarOcorrenciasVeiculo',
            type: 'GET',
            data: { veiculoId: veiculoId },
            success: function (response)
            {
                try
                {
                    if (response.success && response.data && response.data.length > 0)
                    {
                        renderizarTabelaOcorrenciasVeiculoUpsert(response.data);
                    }
                    else
                    {
                        $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasVeiculoUpsert.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                console.error('Erro ao carregar ocorrências:', error);
                $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-danger">Erro ao carregar ocorrências</td></tr>');
                AppToast.show('Vermelho', 'Erro ao carregar ocorrências do veículo', 4000);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasVeiculoUpsert", error);
    }
}

// Renderiza a tabela de ocorrências do veículo
function renderizarTabelaOcorrenciasVeiculoUpsert(ocorrencias)
{
    try
    {
        if (!ocorrencias || ocorrencias.length === 0)
        {
            $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
            return;
        }

        let html = '';
        ocorrencias.forEach(function (oc, index)
        {
            const dataFormatada = oc.dataCriacao ? new Date(oc.dataCriacao).toLocaleDateString('pt-BR') : '-';
            const temImagem = oc.imagemOcorrencia && oc.imagemOcorrencia.trim() !== '';
            const statusOc = oc.statusOcorrencia;
            const statusStr = oc.status || '';
            const itemManutId = oc.itemManutencaoId;
            
            // Determina status final para exibição
            let statusFinal = 'Aberta';
            let badgeClass = 'ftx-ocorrencia-badge-aberta';
            
            if (statusStr === 'Pendente')
            {
                statusFinal = 'Pendente';
                badgeClass = 'ftx-ocorrencia-badge-pendente';
            }
            else if (statusStr === 'Baixada' || statusOc === false)
            {
                statusFinal = 'Baixada';
                badgeClass = 'ftx-ocorrencia-badge-baixada';
            }
            else if (itemManutId && itemManutId !== '00000000-0000-0000-0000-000000000000')
            {
                statusFinal = 'Manutenção';
                badgeClass = 'ftx-ocorrencia-badge-manutencao';
            }
            
            const jaBaixada = statusFinal === 'Baixada';

            html += `
                <tr>
                    <td class="text-center">${index + 1}</td>
                    <td>${oc.resumo || '-'}</td>
                    <td>${oc.descricao || '-'}</td>
                    <td class="text-center">${dataFormatada}</td>
                    <td class="text-center">
                        <span class="ftx-ocorrencia-badge ${badgeClass}">${statusFinal}</span>
                    </td>
                    <td class="text-center">
                        <button type="button" class="btn btn-foto text-white btn-icon-28 btn-ver-imagem-ocorrencia-upsert ${temImagem ? '' : 'disabled'}"
                                data-imagem="${oc.imagemOcorrencia || ''}"
                                ${temImagem ? '' : 'disabled tabindex="-1" aria-disabled="true"'}
                                title="${temImagem ? 'Ver Imagem' : 'Sem imagem'}">
                            <i class="fab fa-wpforms"></i>
                        </button>
                        <button type="button" class="btn btn-verde text-white btn-icon-28 btn-baixar-ocorrencia-upsert ${jaBaixada ? 'disabled' : ''}"
                                data-id="${oc.ocorrenciaViagemId}"
                                data-resumo="${(oc.resumo || '').replace(/"/g, '&quot;')}"
                                ${jaBaixada ? 'disabled tabindex="-1" aria-disabled="true"' : ''}
                                title="${jaBaixada ? 'Já baixada' : 'Dar Baixa na Ocorrência'}">
                            <i class="fa-duotone fa-circle-check"></i>
                        </button>
                        <button type="button" class="btn btn-vinho text-white btn-icon-28 btn-excluir-ocorrencia-upsert"
                                data-id="${oc.ocorrenciaViagemId}"
                                title="Excluir Ocorrência">
                            <i class="fa-duotone fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
        });

        $('#tblOcorrenciasVeiculoUpsert tbody').html(html);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "renderizarTabelaOcorrenciasVeiculoUpsert", error);
    }
}

// Clique no botão de excluir ocorrência (Upsert)
$(document).on('click', '.btn-excluir-ocorrencia-upsert', function (e)
{
    try
    {
        e.preventDefault();
        const ocorrenciaId = $(this).data('id');
        const $btn = $(this);
        const $row = $btn.closest('tr');

        Alerta.Confirmar(
            "Deseja realmente excluir esta ocorrência?",
            "Esta ação não poderá ser desfeita!",
            "Sim, excluir",
            "Cancelar"
        ).then((confirmado) =>
        {
            if (confirmado)
            {
                excluirOcorrenciaVeiculoUpsert(ocorrenciaId, $row);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btn-excluir-ocorrencia-upsert", error);
    }
});

// Exclui a ocorrência via API (Upsert)
function excluirOcorrenciaVeiculoUpsert(ocorrenciaId, $row)
{
    try
    {
        $.ajax({
            url: '/api/OcorrenciaViagem/ExcluirOcorrencia',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ ocorrenciaViagemId: ocorrenciaId }),
            success: function (response)
            {
                try
                {
                    if (response.success)
                    {
                        AppToast.show('Verde', 'Ocorrência excluída com sucesso', 3000);
                        $row.fadeOut(300, function ()
                        {
                            $(this).remove();
                            
                            // Atualiza contador
                            _qtdOcorrenciasVeiculo--;
                            
                            // Verifica se ainda há linhas
                            if ($('#tblOcorrenciasVeiculoUpsert tbody tr').length === 0)
                            {
                                $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
                                desabilitarBotaoOcorrenciasVeiculo();
                            }
                            else
                            {
                                // Atualiza o badge
                                habilitarBotaoOcorrenciasVeiculo(_qtdOcorrenciasVeiculo);
                            }
                        });
                    }
                    else
                    {
                        AppToast.show('Vermelho', response.message || 'Erro ao excluir ocorrência', 4000);
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "excluirOcorrenciaVeiculoUpsert.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                console.error('Erro ao excluir ocorrência:', error);
                AppToast.show('Vermelho', 'Erro ao excluir ocorrência', 4000);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "excluirOcorrenciaVeiculoUpsert", error);
    }
}

// ===============================================================================
// BAIXAR OCORRÊNCIA (Upsert)
// ===============================================================================

// Clique no botão de baixar ocorrência (Upsert)
$(document).on('click', '.btn-baixar-ocorrencia-upsert', async function (e)
{
    try
    {
        e.preventDefault();
        const ocorrenciaId = $(this).data('id');
        const resumo = $(this).data('resumo') || 'esta ocorrência';
        const $btn = $(this);
        const $row = $btn.closest('tr');

        // Primeiro confirma a baixa
        const confirmaBaixa = await Alerta.Confirmar(
            "Dar Baixa na Ocorrência?",
            `Deseja dar baixa em: "${resumo}"?`,
            "Sim, dar baixa",
            "Cancelar"
        );

        if (!confirmaBaixa)
        {
            return;
        }

        // Pergunta se quer adicionar solução
        const querSolucao = await Alerta.Confirmar(
            "Adicionar Solução?",
            "Deseja informar a solução aplicada para esta ocorrência?",
            "Sim, informar",
            "Não, baixar sem solução"
        );

        if (querSolucao)
        {
            // Abre o modal para inserir a solução
            $('#hiddenOcorrenciaIdSolucaoUpsert').val(ocorrenciaId);
            $('#txtSolucaoOcorrenciaUpsert').val('');
            
            const modalSolucao = new bootstrap.Modal(document.getElementById('modalSolucaoOcorrenciaUpsert'));
            modalSolucao.show();
            
            // Foca no campo de texto após o modal abrir
            $('#modalSolucaoOcorrenciaUpsert').one('shown.bs.modal', function () {
                $('#txtSolucaoOcorrenciaUpsert').focus();
            });
        }
        else
        {
            // Baixa sem solução
            baixarOcorrenciaVeiculoUpsert(ocorrenciaId, null, $row);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btn-baixar-ocorrencia-upsert", error);
    }
});

// Clique no botão de confirmar solução no modal
$(document).on('click', '#btnConfirmarSolucaoUpsert', function (e)
{
    try
    {
        e.preventDefault();
        
        const ocorrenciaId = $('#hiddenOcorrenciaIdSolucaoUpsert').val();
        const solucao = $('#txtSolucaoOcorrenciaUpsert').val().trim();
        
        if (!solucao)
        {
            AppToast.show('Amarelo', 'Por favor, informe a solução aplicada', 3000);
            $('#txtSolucaoOcorrenciaUpsert').focus();
            return;
        }
        
        // Fecha o modal
        const modalEl = document.getElementById('modalSolucaoOcorrenciaUpsert');
        const modalInstance = bootstrap.Modal.getInstance(modalEl);
        if (modalInstance)
        {
            modalInstance.hide();
        }
        
        // Encontra a linha da tabela
        const $row = $(`.btn-baixar-ocorrencia-upsert[data-id="${ocorrenciaId}"]`).closest('tr');
        
        // Executa a baixa com solução
        baixarOcorrenciaVeiculoUpsert(ocorrenciaId, solucao, $row);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btnConfirmarSolucaoUpsert", error);
    }
});

// Executa a baixa da ocorrência via API (Upsert)
function baixarOcorrenciaVeiculoUpsert(ocorrenciaId, solucao, $row)
{
    try
    {
        const payload = {
            OcorrenciaViagemId: ocorrenciaId
        };
        
        // Se tiver solução, inclui no payload
        if (solucao)
        {
            payload.SolucaoOcorrencia = solucao;
        }

        $.ajax({
            url: '/api/OcorrenciaViagem/BaixarOcorrenciaUpsert',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload),
            success: function (response)
            {
                try
                {
                    if (response.success)
                    {
                        AppToast.show('Verde', 'Ocorrência baixada com sucesso', 3000);
                        $row.fadeOut(300, function ()
                        {
                            $(this).remove();
                            
                            // Atualiza contador
                            _qtdOcorrenciasVeiculo--;
                            
                            // Verifica se ainda há linhas
                            if ($('#tblOcorrenciasVeiculoUpsert tbody tr').length === 0)
                            {
                                $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
                                desabilitarBotaoOcorrenciasVeiculo();
                            }
                            else
                            {
                                // Atualiza o badge
                                habilitarBotaoOcorrenciasVeiculo(_qtdOcorrenciasVeiculo);
                            }
                        });
                    }
                    else
                    {
                        AppToast.show('Vermelho', response.message || 'Erro ao baixar ocorrência', 4000);
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "baixarOcorrenciaVeiculoUpsert.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                console.error('Erro ao baixar ocorrência:', error);
                AppToast.show('Vermelho', 'Erro ao baixar ocorrência', 4000);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "baixarOcorrenciaVeiculoUpsert", error);
    }
}

// Limpa o modal de solução quando fechado
$('#modalSolucaoOcorrenciaUpsert').on('hidden.bs.modal', function ()
{
    try
    {
        $('#hiddenOcorrenciaIdSolucaoUpsert').val('');
        $('#txtSolucaoOcorrenciaUpsert').val('');
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "modalSolucaoOcorrenciaUpsert.hidden", error);
    }
});

// Quando o modal é fechado, limpa o data attribute
$('#modalOcorrenciasVeiculoUpsert').on('hidden.bs.modal', function ()
{
    try
    {
        this.removeAttribute('data-veiculo-id');
        $('#tblOcorrenciasVeiculoUpsert tbody').html('');

        // Reseta o título
        const tituloSpan = this.querySelector('#modalOcorrenciasVeiculoUpsertLabel span');
        if (tituloSpan)
        {
            tituloSpan.textContent = 'Ocorrências em Aberto do Veículo';
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "modalOcorrenciasVeiculoUpsert.hidden", error);
    }
});

// ===============================================================================
// INTEGRAÇÃO FROTIX MOBILE - RUBRICAS E OCORRÊNCIAS
// ===============================================================================

// Variáveis globais para dados mobile
let _dadosMobile = null;
let _ocorrenciasViagem = [];

// ===============================================================================
// FORMATAÇÃO DE HORA HH:mm
// ===============================================================================

/**
 * Formata um valor de hora para o formato HH:mm
 * @param {string} valor - Valor da hora (pode ser HH:mm, HH:mm:ss, etc)
 * @returns {string} - Hora formatada como HH:mm
 */
function formatarHora(valor)
{
    try
    {
        if (!valor) return '';
        
        // Se já está no formato correto, retorna
        if (/^\d{2}:\d{2}$/.test(valor)) return valor;
        
        // Se tem segundos (HH:mm:ss), remove
        if (/^\d{2}:\d{2}:\d{2}$/.test(valor))
        {
            return valor.substring(0, 5);
        }
        
        // Tenta parsear como hora
        const partes = valor.split(':');
        if (partes.length >= 2)
        {
            const horas = partes[0].padStart(2, '0');
            const minutos = partes[1].padStart(2, '0');
            return `${horas}:${minutos}`;
        }
        
        return valor;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "formatarHora", error);
        return valor;
    }
}

/**
 * Aplica formatação HH:mm nos campos de hora ao carregar a página
 */
function aplicarFormatacaoHoras()
{
    try
    {
        const txtHoraInicial = document.getElementById('txtHoraInicial');
        const txtHoraFinal = document.getElementById('txtHoraFinal');
        
        if (txtHoraInicial && txtHoraInicial.value)
        {
            txtHoraInicial.value = formatarHora(txtHoraInicial.value);
        }
        
        if (txtHoraFinal && txtHoraFinal.value)
        {
            txtHoraFinal.value = formatarHora(txtHoraFinal.value);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "aplicarFormatacaoHoras", error);
    }
}

// ===============================================================================
// LÓGICA NO FICHA VISTORIA (MOBILE vs WEB)
// ===============================================================================

/**
 * Configura exibição do campo NoFichaVistoria
 * - Se valor = 0 ou null/vazio: mostra campo texto "(mobile)"
 * - Se valor > 0: mostra campo numérico normal
 */
function configurarCampoNoFichaVistoria()
{
    // A visibilidade dos campos é decidida pelo Razor no servidor
    // Esta função só ajusta o wrapper do Syncfusion se necessário
    try
    {
        const txtNumerico = document.getElementById('txtNoFichaVistoria');
        const txtMobile = document.getElementById('txtNoFichaVistoriaMobile');
        
        if (!txtNumerico || !txtMobile) return;
        
        // Syncfusion cria wrapper, precisamos garantir que o wrapper tenha a mesma visibilidade
        const wrapperNumerico = txtNumerico.closest('.e-input-group');
        
        if (wrapperNumerico)
        {
            // Sincronizar visibilidade do wrapper com o input
            if (txtNumerico.style.display === 'none')
            {
                wrapperNumerico.style.display = 'none';
            }
            else
            {
                wrapperNumerico.style.display = '';
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "configurarCampoNoFichaVistoria", error);
    }
}

// ===============================================================================
// CARREGAR DADOS MOBILE (RUBRICAS + OCORRÊNCIAS)
// ===============================================================================

/**
 * Carrega dados do mobile via API (apenas se a seção mobile estiver visível)
 * A visibilidade da seção é decidida pelo Razor no servidor
 */
async function carregarDadosMobile()
{
    try
    {
        // Verificar se a seção mobile está visível (decidido pelo servidor/Razor)
        const secaoMobile = document.getElementById('secaoMobile');
        if (!secaoMobile || secaoMobile.style.display === 'none')
        {
            // Seção mobile não está visível, não precisa carregar dados
            return;
        }
        
        if (!window.viagemId || window.viagemId === '' || window.viagemId === '00000000-0000-0000-0000-000000000000')
        {
            // Nova viagem - não há dados mobile para carregar
            return;
        }
        
        const response = await fetch(`/api/Viagem/ObterDadosMobile?viagemId=${window.viagemId}`);
        const data = await response.json();
        
        if (data.success && data.isMobile)
        {
            _dadosMobile = data;
            
            // Carregar rubricas
            carregarRubricas(data);
            
            // Carregar documentos/itens
            carregarDocumentosItensMobile(data);
            
            // Carregar ocorrências
            carregarOcorrenciasViagem(data.ocorrencias);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarDadosMobile", error);
    }
}

/**
 * Carrega as rubricas (inicial e final) na interface
 */
function carregarRubricas(data)
{
    try
    {
        // Rubrica Inicial
        const imgRubricaInicial = document.getElementById('imgRubricaInicial');
        const semRubricaInicial = document.getElementById('semRubricaInicial');
        
        if (data.temRubricaInicial && data.rubricaInicial)
        {
            if (imgRubricaInicial)
            {
                imgRubricaInicial.src = data.rubricaInicial;
                imgRubricaInicial.style.display = 'block';
            }
            if (semRubricaInicial)
            {
                semRubricaInicial.style.display = 'none';
            }
        }
        else
        {
            if (imgRubricaInicial)
            {
                imgRubricaInicial.style.display = 'none';
            }
            if (semRubricaInicial)
            {
                semRubricaInicial.style.display = 'block';
            }
        }
        
        // Rubrica Final
        const imgRubricaFinal = document.getElementById('imgRubricaFinal');
        const semRubricaFinal = document.getElementById('semRubricaFinal');
        
        if (data.temRubricaFinal && data.rubricaFinal)
        {
            if (imgRubricaFinal)
            {
                imgRubricaFinal.src = data.rubricaFinal;
                imgRubricaFinal.style.display = 'block';
            }
            if (semRubricaFinal)
            {
                semRubricaFinal.style.display = 'none';
            }
        }
        else
        {
            if (imgRubricaFinal)
            {
                imgRubricaFinal.style.display = 'none';
            }
            if (semRubricaFinal)
            {
                semRubricaFinal.style.display = 'block';
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarRubricas", error);
    }
}

/**
 * Carrega os dados de documentos/itens entregues e devolvidos
 */
function carregarDocumentosItensMobile(data)
{
    try
    {
        // Documentos/Itens Entregues (Vistoria Inicial) - todos checkboxes
        const chkStatusDocumento = document.getElementById('chkStatusDocumentoMobile');
        const chkStatusCartao = document.getElementById('chkStatusCartaoMobile');
        const chkCintaEntregue = document.getElementById('chkCintaEntregueMobile');
        const chkTabletEntregue = document.getElementById('chkTabletEntregueMobile');
        
        if (chkStatusDocumento) chkStatusDocumento.checked = data.documentoEntregue === true;
        if (chkStatusCartao) chkStatusCartao.checked = data.cartaoAbastecimentoEntregue === true;
        if (chkCintaEntregue) chkCintaEntregue.checked = data.cintaEntregue === true;
        if (chkTabletEntregue) chkTabletEntregue.checked = data.tabletEntregue === true;
        
        // Documentos/Itens Devolvidos (Vistoria Final) - todos checkboxes
        const chkStatusDocumentoFinal = document.getElementById('chkStatusDocumentoFinalMobile');
        const chkStatusCartaoFinal = document.getElementById('chkStatusCartaoFinalMobile');
        const chkCintaDevolvida = document.getElementById('chkCintaDevolvidaMobile');
        const chkTabletDevolvido = document.getElementById('chkTabletDevolvidoMobile');
        
        if (chkStatusDocumentoFinal) chkStatusDocumentoFinal.checked = data.documentoDevolvido === true;
        if (chkStatusCartaoFinal) chkStatusCartaoFinal.checked = data.cartaoAbastecimentoDevolvido === true;
        if (chkCintaDevolvida) chkCintaDevolvida.checked = data.cintaDevolvida === true;
        if (chkTabletDevolvido) chkTabletDevolvido.checked = data.tabletDevolvido === true;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarDocumentosItensMobile", error);
    }
}

/**
 * Carrega as ocorrências da viagem na tabela
 */
function carregarOcorrenciasViagem(ocorrencias)
{
    try
    {
        _ocorrenciasViagem = ocorrencias || [];
        
        const tbody = document.querySelector('#tblOcorrenciasViagem tbody');
        const badge = document.getElementById('badgeOcorrenciasViagem');
        
        if (!tbody) return;
        
        // Limpar tabela
        tbody.innerHTML = '';
        
        if (_ocorrenciasViagem.length === 0)
        {
            // Sem ocorrências
            tbody.innerHTML = `
                <tr id="rowSemOcorrenciasViagem">
                    <td colspan="6" class="text-center text-muted py-4">
                        <i class="fa fa-check-circle fa-2x mb-2 text-success"></i>
                        <br />Nenhuma ocorrência registrada nesta viagem
                    </td>
                </tr>`;
            
            if (badge) badge.style.display = 'none';
        }
        else
        {
            // Tem ocorrências
            let html = '';
            let idx = 1;
            
            for (const oc of _ocorrenciasViagem)
            {
                const statusClass = obterClasseStatusOcorrencia(oc.statusOcorrencia);
                const statusTexto = oc.statusOcorrencia || 'Aberta';
                
                html += `
                    <tr>
                        <td class="text-center">${idx}</td>
                        <td title="${escapeHtmlMobile(oc.resumo || '')}">${escapeHtmlMobile(oc.resumo || '-')}</td>
                        <td title="${escapeHtmlMobile(oc.descricao || '')}">${escapeHtmlMobile(truncarTextoMobile(oc.descricao, 50) || '-')}</td>
                        <td class="text-center">${oc.dataOcorrencia || '-'}</td>
                        <td class="text-center">
                            <span class="ftx-status-badge ${statusClass}">${statusTexto}</span>
                        </td>
                        <td class="text-center">
                            <button type="button" 
                                    class="btn-ver-ocorrencia" 
                                    onclick="verOcorrenciaViagem('${oc.ocorrenciaViagemId}')"
                                    title="Ver detalhes">
                                <i class="fa fa-eye"></i>
                            </button>
                        </td>
                    </tr>`;
                idx++;
            }
            
            tbody.innerHTML = html;
            
            // Atualizar badge
            if (badge)
            {
                badge.textContent = _ocorrenciasViagem.length;
                badge.style.display = 'inline';
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasViagem", error);
    }
}

/**
 * Retorna a classe CSS baseada no status da ocorrência
 */
function obterClasseStatusOcorrencia(status)
{
    try
    {
        if (!status) return 'ftx-status-aberta';
        
        const statusLower = status.toLowerCase();
        
        if (statusLower === 'baixada' || statusLower === 'resolvida' || statusLower === 'fechada')
        {
            return 'ftx-status-baixada';
        }
        else if (statusLower === 'pendente' || statusLower === 'em análise')
        {
            return 'ftx-status-pendente';
        }
        else
        {
            return 'ftx-status-aberta';
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "obterClasseStatusOcorrencia", error);
        return 'ftx-status-aberta';
    }
}

/**
 * Trunca texto para exibição na tabela
 */
function truncarTextoMobile(texto, maxLength)
{
    try
    {
        if (!texto) return '';
        if (texto.length <= maxLength) return texto;
        return texto.substring(0, maxLength) + '...';
    }
    catch (error)
    {
        return texto || '';
    }
}

/**
 * Escapa HTML para evitar XSS
 */
function escapeHtmlMobile(text)
{
    try
    {
        if (!text) return '';
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
    catch (error)
    {
        return text || '';
    }
}

// ===============================================================================
// MODAL VISUALIZAR OCORRÊNCIA DA VIAGEM
// ===============================================================================

/**
 * Abre o modal para visualizar detalhes de uma ocorrência
 */
async function verOcorrenciaViagem(ocorrenciaId)
{
    try
    {
        console.log("verOcorrenciaViagem - ocorrenciaId:", ocorrenciaId);
        console.log("verOcorrenciaViagem - _ocorrenciasViagem:", _ocorrenciasViagem);
        console.log("verOcorrenciaViagem - ocorrenciasUpsert:", ocorrenciasUpsert);
        
        if (!ocorrenciaId) return;
        
        // Buscar ocorrência nas listas disponíveis (Mobile ou Upsert)
        let oc = null;
        
        // Primeiro, tentar na lista do Mobile
        if (_ocorrenciasViagem && _ocorrenciasViagem.length > 0)
        {
            oc = _ocorrenciasViagem.find(o => o.ocorrenciaViagemId === ocorrenciaId);
        }
        
        // Se não encontrou, tentar na lista do Upsert
        if (!oc && ocorrenciasUpsert && ocorrenciasUpsert.length > 0)
        {
            oc = ocorrenciasUpsert.find(o => o.id === ocorrenciaId);
            // Normalizar campos se encontrou em ocorrenciasUpsert
            if (oc)
            {
                oc = {
                    ocorrenciaViagemId: oc.id,
                    resumo: oc.resumo,
                    descricao: oc.descricao,
                    dataOcorrencia: oc.dataCriacao,
                    statusOcorrencia: oc.status || 'Aberta',
                    imagemBase64: oc.imagemBase64,
                    temImagem: oc.imagemBase64 && oc.imagemBase64.length > 0
                };
            }
        }
        
        console.log("verOcorrenciaViagem - oc encontrada:", oc);
        
        if (!oc)
        {
            AppToast.show('Amarelo', 'Ocorrência não encontrada', 3000);
            return;
        }
        
        // Preencher modal
        document.getElementById('txtOcorrenciaResumo').textContent = oc.resumo || '-';
        document.getElementById('txtOcorrenciaDescricao').textContent = oc.descricao || 'Sem descrição';
        document.getElementById('txtOcorrenciaData').textContent = oc.dataOcorrencia || '-';
        
        // Status
        const divStatus = document.getElementById('divOcorrenciaStatus');
        const statusClass = obterClasseStatusOcorrencia(oc.statusOcorrencia);
        divStatus.innerHTML = `<span class="ftx-status-badge ${statusClass}">${oc.statusOcorrencia || 'Aberta'}</span>`;
        
        // Solução (se houver)
        const divSolucao = document.getElementById('divSolucaoOcorrencia');
        const txtSolucao = document.getElementById('txtOcorrenciaSolucao');
        
        if (oc.solucao)
        {
            txtSolucao.textContent = oc.solucao;
            divSolucao.style.display = 'block';
        }
        else
        {
            divSolucao.style.display = 'none';
        }
        
        // Imagem
        const imgOcorrencia = document.getElementById('imgOcorrenciaViagem');
        const semImagem = document.getElementById('semImagemOcorrenciaViagem');
        
        if (oc.temImagem && oc.imagemBase64)
        {
            imgOcorrencia.src = oc.imagemBase64;
            imgOcorrencia.style.display = 'block';
            semImagem.style.display = 'none';
        }
        else if (oc.temImagem)
        {
            // Precisa buscar imagem via API
            try
            {
                const resp = await fetch(`/api/Viagem/ObterImagemOcorrencia?ocorrenciaId=${ocorrenciaId}`);
                const imgData = await resp.json();
                
                if (imgData.success && imgData.temImagem)
                {
                    imgOcorrencia.src = imgData.imagemBase64;
                    imgOcorrencia.style.display = 'block';
                    semImagem.style.display = 'none';
                }
                else
                {
                    imgOcorrencia.style.display = 'none';
                    semImagem.style.display = 'block';
                }
            }
            catch (e)
            {
                imgOcorrencia.style.display = 'none';
                semImagem.style.display = 'block';
            }
        }
        else
        {
            imgOcorrencia.style.display = 'none';
            semImagem.style.display = 'block';
        }
        
        // Abrir modal
        const modal = new bootstrap.Modal(document.getElementById('modalVerOcorrenciaViagem'));
        modal.show();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verOcorrenciaViagem", error);
    }
}

// ===============================================================================
// INICIALIZAÇÃO MOBILE
// ===============================================================================

/**
 * Função de inicialização dos componentes mobile
 */
function inicializarIntegracaoMobile()
{
    try
    {
        // Definir viagemId global
        const txtViagemId = document.getElementById('txtViagemId') || document.getElementById('txtId');
        window.viagemId = txtViagemId ? txtViagemId.value : '';
        
        // Aplicar formatação de horas
        aplicarFormatacaoHoras();
        
        // Configurar campo NoFichaVistoria
        configurarCampoNoFichaVistoria();
        
        // Carregar dados mobile (rubricas + ocorrências) - também controla visibilidade da ficha
        carregarDadosMobile();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "inicializarIntegracaoMobile", error);
    }
}

// Chamar inicialização quando DOM estiver pronto
$(document).ready(function ()
{
    try
    {
        // Aguardar para garantir que Syncfusion TextBox já renderizou
        setTimeout(function ()
        {
            inicializarIntegracaoMobile();
        }, 500);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "document.ready.mobile", error);
    }
});

// ===============================================================================
// SISTEMA DE OCORRÊNCIAS NA CRIAÇÃO/EDIÇÃO DE VIAGEM
// ===============================================================================

// Array para armazenar ocorrências em memória
var ocorrenciasUpsert = [];

/**
 * Inicializa o sistema de ocorrências
 */
function inicializarSistemaOcorrencias()
{
    try
    {
        // Verificar se veículo está selecionado (para garantir estado correto na inicialização)
        // Aguardar um pouco mais para garantir que os combos Syncfusion estejam prontos
        setTimeout(function() {
            verificarVeiculoParaOcorrencias();
        }, 100);
        
        // Botão adicionar ocorrência
        $('#btnAdicionarOcorrenciaUpsert').on('click', function()
        {
            abrirModalInserirOcorrencia();
        });

        // Preview de imagem
        $('#fileImagemOcorrenciaUpsert').on('change', function()
        {
            previewImagemOcorrencia(this);
        });

        // Limpar imagem
        $('#btnLimparImagemOcorrenciaUpsert').on('click', function()
        {
            limparImagemOcorrencia();
        });

        // Confirmar ocorrência
        $('#btnConfirmarOcorrenciaUpsert').on('click', function()
        {
            confirmarOcorrencia();
        });

        // Delegação de eventos para botões dinâmicos
        $(document).on('click', '.btn-remover-ocorrencia-upsert', function()
        {
            const index = $(this).data('index');
            removerOcorrencia(index);
        });

        $(document).on('click', '.btn-ver-imagem-ocorrencia-upsert', function()
        {
            const index = $(this).data('index');
            verImagemOcorrencia(index);
        });

        $(document).on('click', '.btn-ver-detalhes-ocorrencia-upsert', function()
        {
            const id = $(this).data('id');
            if (id) {
                verOcorrenciaViagem(id);
            }
        });

        // Carregar ocorrências existentes se for edição
        carregarOcorrenciasExistentes();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "inicializarSistemaOcorrencias", error);
    }
}

/**
 * Verifica se há veículo selecionado e habilita/desabilita seção de ocorrências
 */
function verificarVeiculoParaOcorrencias()
{
    try
    {
        const cmbVeiculo = document.getElementById('cmbVeiculo');
        if (!$("#cmbVeiculo").data("kendoComboBox")) return;
        
        // Obter valor - pode ser string ou array
        let veiculoId = ($("#cmbVeiculo").data("kendoComboBox") ? $("#cmbVeiculo").data("kendoComboBox").value() : null);
        if (Array.isArray(veiculoId))
        {
            veiculoId = veiculoId[0];
        }
        
        // Chamar função global de controle (definida no Upsert.cshtml)
        if (typeof controlarSecaoOcorrencias === 'function')
        {
            controlarSecaoOcorrencias(veiculoId);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarVeiculoParaOcorrencias", error);
    }
}

/**
 * Abre o modal para inserir nova ocorrência
 */
function abrirModalInserirOcorrencia()
{
    try
    {
        // Limpar campos
        $('#txtResumoOcorrenciaUpsert').val('');
        $('#txtDescricaoOcorrenciaUpsert').val('');
        $('#fileImagemOcorrenciaUpsert').val('');
        $('#previewImagemOcorrenciaUpsert').hide();
        $('#imgPreviewOcorrenciaUpsert').attr('src', '');

        // Abrir modal
        const modal = new bootstrap.Modal(document.getElementById('modalInserirOcorrenciaUpsert'));
        modal.show();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "abrirModalInserirOcorrencia", error);
    }
}

/**
 * Preview da imagem selecionada
 */
function previewImagemOcorrencia(input)
{
    try
    {
        if (input.files && input.files[0])
        {
            const reader = new FileReader();
            reader.onload = function(e)
            {
                $('#imgPreviewOcorrenciaUpsert').attr('src', e.target.result);
                $('#previewImagemOcorrenciaUpsert').show();
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "previewImagemOcorrencia", error);
    }
}

/**
 * Limpa a imagem selecionada
 */
function limparImagemOcorrencia()
{
    try
    {
        $('#fileImagemOcorrenciaUpsert').val('');
        $('#previewImagemOcorrenciaUpsert').hide();
        $('#imgPreviewOcorrenciaUpsert').attr('src', '');
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "limparImagemOcorrencia", error);
    }
}

/**
 * Confirma e adiciona a ocorrência à lista
 */
function confirmarOcorrencia()
{
    try
    {
        const resumo = $('#txtResumoOcorrenciaUpsert').val().trim();
        const descricao = $('#txtDescricaoOcorrenciaUpsert').val().trim();
        const imgPreview = $('#imgPreviewOcorrenciaUpsert').attr('src') || '';

        // Validação
        if (!resumo)
        {
            AppToast.show('Amarelo', 'Informe o resumo da ocorrência', 3000);
            $('#txtResumoOcorrenciaUpsert').focus();
            return;
        }

        // Criar objeto da ocorrência
        const ocorrencia = {
            id: 'temp_' + Date.now(),
            resumo: resumo,
            descricao: descricao,
            imagemBase64: imgPreview,
            dataCriacao: new Date().toLocaleString('pt-BR')
        };

        // Adicionar ao array
        ocorrenciasUpsert.push(ocorrencia);

        // Atualizar UI
        renderizarListaOcorrencias();
        atualizarBadgeOcorrencias();
        atualizarHiddenOcorrencias();

        // Fechar modal
        bootstrap.Modal.getInstance(document.getElementById('modalInserirOcorrenciaUpsert')).hide();

        AppToast.show('Verde', 'Ocorrência adicionada com sucesso', 2000);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "confirmarOcorrencia", error);
    }
}

/**
 * Remove uma ocorrência da lista
 */
function removerOcorrencia(index)
{
    try
    {
        if (index >= 0 && index < ocorrenciasUpsert.length)
        {
            ocorrenciasUpsert.splice(index, 1);
            renderizarListaOcorrencias();
            atualizarBadgeOcorrencias();
            atualizarHiddenOcorrencias();
            AppToast.show('Vermelho', 'Ocorrência removida', 2000);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "removerOcorrencia", error);
    }
}

/**
 * Abre modal para ver imagem da ocorrência
 */
function verImagemOcorrencia(index)
{
    try
    {
        if (index >= 0 && index < ocorrenciasUpsert.length)
        {
            const ocorrencia = ocorrenciasUpsert[index];
            if (ocorrencia.imagemBase64)
            {
                $('#imgViewerOcorrenciaUpsert').attr('src', ocorrencia.imagemBase64);
                const modal = new bootstrap.Modal(document.getElementById('modalVerImagemOcorrenciaUpsert'));
                modal.show();
            }
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verImagemOcorrencia", error);
    }
}

/**
 * Renderiza a lista de ocorrências na tela
 */
function renderizarListaOcorrencias()
{
    try
    {
        const container = $('#listaOcorrenciasUpsert');
        const semOcorrencias = $('#semOcorrenciasUpsert');

        if (ocorrenciasUpsert.length === 0)
        {
            semOcorrencias.show();
            container.find('.ocorrencia-item').remove();
            return;
        }

        semOcorrencias.hide();
        
        // Limpar itens existentes
        container.find('.ocorrencia-item').remove();

        // Adicionar cada ocorrência
        ocorrenciasUpsert.forEach((oc, index) =>
        {
            const temImagem = oc.imagemBase64 && oc.imagemBase64.length > 0;
            const temId = oc.id && oc.id !== "" && oc.id !== "00000000-0000-0000-0000-000000000000";
            const podeExcluir = !window.viagemFinalizada;
            
            const html = `
                <div class="ocorrencia-item" data-index="${index}">
                    <div class="ocorrencia-info">
                        <div class="ocorrencia-resumo">
                            <i class="fa-duotone fa-triangle-exclamation text-warning me-1"></i>
                            ${escapeHtml(oc.resumo)}
                        </div>
                        ${oc.descricao ? `<div class="ocorrencia-descricao">${escapeHtml(oc.descricao)}</div>` : ''}
                    </div>
                    <div class="ocorrencia-acoes">
                        ${temId ? `
                            <button type="button" class="btn btn-sm btn-outline-primary btn-ver-detalhes-ocorrencia-upsert" 
                                    data-id="${oc.id}" data-index="${index}" title="Ver detalhes">
                                <i class="fa fa-eye"></i>
                            </button>
                        ` : ''}
                        ${temImagem ? `
                            <button type="button" class="btn btn-sm btn-outline-info btn-ver-imagem-ocorrencia-upsert" 
                                    data-index="${index}" title="Ver imagem">
                                <i class="fa fa-image"></i>
                            </button>
                        ` : ''}
                        ${podeExcluir ? `
                            <button type="button" class="btn btn-sm btn-outline-danger btn-remover-ocorrencia-upsert" 
                                    data-index="${index}" title="Remover">
                                <i class="fa fa-trash"></i>
                            </button>
                        ` : ''}
                    </div>
                </div>
            `;
            
            container.append(html);
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "renderizarListaOcorrencias", error);
    }
}

/**
 * Atualiza o badge de quantidade de ocorrências
 */
function atualizarBadgeOcorrencias()
{
    try
    {
        const badge = $('#badgeOcorrenciasUpsert');
        const qtd = ocorrenciasUpsert.length;

        if (qtd > 0)
        {
            badge.text(qtd);
            badge.show();
        }
        else
        {
            badge.hide();
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarBadgeOcorrencias", error);
    }
}

/**
 * Atualiza o campo hidden com o JSON das ocorrências
 */
function atualizarHiddenOcorrencias()
{
    try
    {
        const json = JSON.stringify(ocorrenciasUpsert.map(oc => ({
            Resumo: oc.resumo,
            Descricao: oc.descricao,
            ImagemOcorrencia: oc.imagemBase64 || ''
        })));
        
        $('#hiddenOcorrenciasJson').val(json);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarHiddenOcorrencias", error);
    }
}

/**
 * Carrega ocorrências existentes (modo edição)
 */
function carregarOcorrenciasExistentes()
{
    try
    {
        // Tentar obter viagemId de múltiplas fontes
        let viagemId = window.viagemId || $('#txtViagemId').val() || $('input[name="ViagemObj.Viagem.ViagemId"]').val();
        
        console.log("carregarOcorrenciasExistentes - viagemId:", viagemId);
        
        if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000')
        {
            console.log("Nova viagem - não carregando ocorrências");
            return; // Nova viagem, não há ocorrências para carregar
        }

        // Usar a mesma API que carrega dados mobile
        $.ajax({
            url: '/api/Viagem/ObterDadosMobile',
            type: 'GET',
            data: { viagemId: viagemId },
            success: function(response)
            {
                try
                {
                    console.log("ObterDadosMobile response:", response);
                    if (response.success && response.ocorrencias && response.ocorrencias.length > 0)
                    {
                        ocorrenciasUpsert = response.ocorrencias.map(oc => ({
                            id: oc.ocorrenciaViagemId,
                            resumo: oc.resumo || '',
                            descricao: oc.descricao || '',
                            imagemBase64: oc.imagemBase64 || oc.imagemOcorrencia || oc.imagem || '',
                            dataCriacao: oc.dataOcorrencia || '',
                            status: oc.statusOcorrencia || 'Aberta',
                            temImagem: oc.temImagem || (oc.imagemBase64 && oc.imagemBase64.length > 0) || (oc.imagemOcorrencia && oc.imagemOcorrencia.length > 0)
                        }));
                        
                        console.log("ocorrenciasUpsert mapeadas:", ocorrenciasUpsert);

                        renderizarListaOcorrencias();
                        atualizarBadgeOcorrencias();
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasExistentes.success", error);
                }
            },
            error: function(xhr, status, error)
            {
                console.log("Erro ao carregar ocorrências:", error, xhr.responseText);
            }
        });
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasExistentes", error);
    }
}

/**
 * Função auxiliar para escapar HTML
 */
function escapeHtml(text)
{
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Inicializar sistema de ocorrências quando DOM estiver pronto
$(document).ready(function()
{
    try
    {
        setTimeout(function()
        {
            inicializarSistemaOcorrencias();
            
            // Garantir que duração e km percorrido sejam calculados após carregamento
            calcularDuracaoViagem();
            calcularKmPercorrido();
        }, 600);
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "document.ready.ocorrencias", error);
    }
});
