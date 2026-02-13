/* ****************************************************************************************
 * ⚡ ARQUIVO: ocorrencias.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gestão de Ocorrências (incidentes/eventos de veículos/motoristas)
 *                   com DataTable, loading overlay padrão FrotiX, upload de imagens,
 *                   abreviação de nomes de motoristas, e CRUD completo.
 * 📥 ENTRADAS     : DataTable #tblOcorrencias, formulário de ocorrências, uploads de imagem,
 *                   eventos de clique (criar/editar/excluir), campos de filtro,
 *                   nomes de motoristas (abreviarNomeMotorista)
 * 📤 SAÍDAS       : DataTable renderizado, loading overlay exibido/oculto
 *                   (#loadingOverlayOcorrencias), nomes abreviados (conectores preservados),
 *                   imagens carregadas (imagemOcorrenciaAlterada, novaImagemOcorrencia),
 *                   AJAX POST/GET para APIs de Ocorrências, AppToast notificações,
 *                   console.warn (debug), Alerta.TratamentoErroComLinha não implícito
 * 🔗 CHAMADA POR  : $(document).ready, event handlers (cliques, filtros), funções auxiliares
 *                   (mostrarLoadingOcorrencias, esconderLoadingOcorrencias,
 *                   abreviarNomeMotorista), Pages/Ocorrencias/Index.cshtml
 * 🔄 CHAMA        : DataTable API, mostrarLoadingOcorrencias(mensagem),
 *                   esconderLoadingOcorrencias(), abreviarNomeMotorista(nome),
 *                   $.ajax, AppToast.show, console.warn, document.getElementById
 * 📦 DEPENDÊNCIAS : jQuery 3.x, DataTables, AppToast (toast notifications),
 *                   Syncfusion (possivelmente para uploads), Loading Overlay FrotiX
 * 📝 OBSERVAÇÕES  : Arquivo grande (1034 linhas). Conectores preservados na abreviação:
 *                   "de", "da", "do", "dos", "das", "e", "d", "d'", "del", etc.
 *                   Variáveis globais: dataTable, imagemOcorrenciaAlterada, novaImagemOcorrencia.
 *                   Try-catch com console.warn (não usa Alerta.TratamentoErroComLinha).
 **************************************************************************************** */

/* =========================================================================
 *  ocorrencias.js
 *  Tela: Gestão de Ocorrências
 *  Padrão FrotiX - Refatorado
 * ========================================================================= */

/* ==========================
   Variáveis Globais
   ========================== */
var dataTable = null;

/* ==========================
   Funções de Loading - Padrão FrotiX
   ========================== */
function mostrarLoadingOcorrencias(mensagem) {
    try {
        var overlay = document.getElementById('loadingOverlayOcorrencias');
        if (overlay) {
            var msgEl = overlay.querySelector('.ftx-loading-text');
            if (msgEl && mensagem) msgEl.textContent = mensagem;
            overlay.style.display = 'flex';
        }
    } catch (error) {
        console.warn("Erro ao mostrar loading:", error);
    }
}

function esconderLoadingOcorrencias() {
    try {
        var overlay = document.getElementById('loadingOverlayOcorrencias');
        if (overlay) {
            overlay.style.display = 'none';
        }
    } catch (error) {
        console.warn("Erro ao esconder loading:", error);
    }
}
var imagemOcorrenciaAlterada = false;
var novaImagemOcorrencia = "";

/* ==========================
   Helpers de Nome
   ========================== */
const CONECTORES = new Set([
    "de", "da", "do", "dos", "das", "e", "d", "d'", "del", "della", "di", "du", "van", "von",
]);

function abreviarNomeMotorista(nome)
{
    try
    {
        if (!nome) return "";
        const palavras = String(nome).trim().split(/\s+/);
        const out = [];

        for (const w of palavras)
        {
            const limp = w.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/[.:()]/g, "");
            if (CONECTORES.has(limp)) continue;
            out.push(w);
            if (out.length === 2) break;
        }

        return out.join(" ");
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "abreviarNomeMotorista", error);
        return nome || "";
    }
}

/* ==========================
   Helpers de Data
   ========================== */
function _keyIsoFromBR(value)
{
    try
    {
        if (!value) return "";
        const [dd, mm, yyyy] = value.split("/");
        return `${yyyy}-${mm}-${dd}`;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "_keyIsoFromBR", error);
        return "";
    }
}

/* ==========================
   Helpers de Combo Syncfusion
   ========================== */
function getComboValue(comboId)
{
    try
    {
        var combo = $("#" + comboId).data("kendoComboBox");
        if (combo)
        {
            var val = combo.value();
            if (val != null && val !== "") return val;
        }
        return "";
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "getComboValue", error);
        return "";
    }
}

/* ==========================
   Construção da Grid
   ========================== */
function BuildGridOcorrencias(params)
{
    try
    {
        // Mostra loading padrão FrotiX
        mostrarLoadingOcorrencias('Carregando Ocorrências...');

        if ($.fn.DataTable.isDataTable("#tblOcorrencia"))
        {
            $("#tblOcorrencia").DataTable().destroy();
            $("#tblOcorrencia tbody").empty();
        }

        dataTable = $("#tblOcorrencia").DataTable({
            autoWidth: false,
            dom: "Bfrtip",
            lengthMenu: [[10, 25, 50, -1], ["10 linhas", "25 linhas", "50 linhas", "Todas"]],
            buttons: ["pageLength", "excel", { extend: "pdfHtml5", orientation: "landscape", pageSize: "LEGAL" }],
            order: [[1, "desc"]],
            columnDefs: [
                { targets: 0, className: "text-center", width: "5%" },
                {
                    targets: 1,
                    className: "text-center",
                    width: "8%",
                    render: function (value, type)
                    {
                        try
                        {
                            if (!value) return "";
                            if (type === "sort" || type === "type")
                            {
                                if (/^\d{2}\/\d{2}\/\d{4}$/.test(value)) return _keyIsoFromBR(value);
                            }
                            return value;
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.data", error);
                            return "";
                        }
                    }
                },
                {
                    targets: 2,
                    className: "text-left",
                    width: "12%",
                    render: function (data, type)
                    {
                        try
                        {
                            return type === "display" ? abreviarNomeMotorista(data) : data;
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.motorista", error);
                            return data;
                        }
                    }
                },
                { targets: 3, className: "text-left", width: "15%" },
                { targets: 4, className: "text-left", width: "15%" },
                { targets: 5, className: "text-left", width: "15%" },
                { targets: 6, className: "text-center", width: "8%" },
                { targets: 7, className: "text-center", width: "8%" },
                { targets: 8, visible: false }
            ],
            responsive: true,
            ajax: {
                url: "/api/OcorrenciaViagem/ListarGestao",
                type: "GET",
                dataType: "json",
                data: params,
                error: function (xhr, error, thrown)
                {
                    try
                    {
                        esconderLoadingOcorrencias();
                        console.error("Erro ao carregar ocorrências:", error, thrown);
                        AppToast.show("Vermelho", "Erro ao carregar ocorrências", 3000);
                    }
                    catch (err)
                    {
                        Alerta.TratamentoErroComLinha("ocorrencias.js", "ajax.error", err);
                    }
                }
            },
            columns: [
                { data: "noFichaVistoria", defaultContent: "-" },
                { data: "data", defaultContent: "-" },
                { data: "nomeMotorista", defaultContent: "-" },
                { data: "descricaoVeiculo", defaultContent: "-" },
                { data: "resumoOcorrencia", defaultContent: "-" },
                { data: "descricaoSolucaoOcorrencia", defaultContent: "-" },
                {
                    data: "statusOcorrencia",
                    render: function (data, type, row)
                    {
                        try
                        {
                            var s = row.statusOcorrencia || "Aberta";
                            var icon = "";
                            var badgeClass = "ftx-badge-aberta";
                            
                            switch (s)
                            {
                                case "Aberta":
                                    icon = '<i class="fa-duotone fa-circle-exclamation me-1"></i>';
                                    badgeClass = "ftx-badge-aberta";
                                    break;
                                case "Baixada":
                                    icon = '<i class="fa-duotone fa-circle-check me-1"></i>';
                                    badgeClass = "ftx-badge-baixada";
                                    break;
                                case "Pendente":
                                    icon = '<i class="fa-duotone fa-clock me-1"></i>';
                                    badgeClass = "ftx-badge-pendente";
                                    break;
                                case "Manutenção":
                                    icon = '<i class="fa-duotone fa-wrench me-1"></i>';
                                    badgeClass = "ftx-badge-manutencao";
                                    break;
                            }
                            
                            return `<span class="ftx-badge-status ${badgeClass}">${icon}${s}</span>`;
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.status", error);
                            return "";
                        }
                    }
                },
                {
                    data: "ocorrenciaViagemId",
                    render: function (data, type, row)
                    {
                        try
                        {
                            var baixada = row.statusOcorrencia === "Baixada";
                            var temImagem = row.imagemOcorrencia && row.imagemOcorrencia.trim() !== "";

                            // Botão Editar - Padrão FrotiX (Azul)
                            var btnEditar = `
                                <a class="btn-azul btn-icon-28 btn-editar-ocorrencia" 
                                    data-id="${data}" 
                                    data-ejtip="Editar Ocorrência"
                                    style="cursor:pointer;">
                                    <i class="fa-duotone fa-pen-to-square"></i>
                                </a>`;

                            // Botão Baixar - Padrão FrotiX (Vinho)
                            var btnBaixa = `
                                <a class="btn-vinho btn-icon-28 btn-baixar ${baixada ? 'disabled' : ''}" 
                                    data-id="${data}" 
                                    data-ejtip="${baixada ? 'Já baixada' : 'Dar Baixa'}"
                                    style="cursor:pointer;"
                                    ${baixada ? 'disabled' : ''}>
                                    <i class="fa-duotone fa-flag-checkered"></i>
                                </a>`;

                            // Botão Ver Imagem - Padrão FrotiX (Terracota)
                            var btnImagem = `
                                <a class="btn-terracota btn-icon-28 btn-ver-imagem ${temImagem ? '' : 'disabled'}" 
                                    data-imagem="${row.imagemOcorrencia || ''}" 
                                    data-ejtip="${temImagem ? 'Ver Imagem/Vídeo' : 'Sem imagem'}"
                                    style="cursor:pointer;"
                                    ${temImagem ? '' : 'disabled'}>
                                    <i class="fa-duotone fa-image"></i>
                                </a>`;

                            // Ordem: Edição, Baixa, Foto
                            return `<div class="text-center" style="display:flex; justify-content:center; gap:4px;">
                                ${btnEditar}
                                ${btnBaixa}
                                ${btnImagem}
                            </div>`;
                        }
                        catch (error)
                        {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.acoes", error);
                            return "";
                        }
                    }
                },
                { data: "descricaoOcorrencia", defaultContent: "" }
            ],
            language: {
                url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json"
            },
            drawCallback: function ()
            {
                try
                {
                    console.log("[ocorrencias.js] Grid carregada com", this.api().rows().count(), "registros");
                    // Esconde loading quando a grid terminar de desenhar
                    esconderLoadingOcorrencias();
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("ocorrencias.js", "drawCallback", error);
                }
            }
        });
    }
    catch (error)
    {
        esconderLoadingOcorrencias();
        Alerta.TratamentoErroComLinha("ocorrencias.js", "BuildGridOcorrencias", error);
    }
}

/* ==========================
   Coleta de Parâmetros
   ========================== */
function collectParamsFromUI()
{
    try
    {
        const data = ($("#txtData").val() || "").trim();
        const dataInicial = ($("#txtDataInicial").val() || "").trim();
        const dataFinal = ($("#txtDataFinal").val() || "").trim();
        const temPeriodo = dataInicial && dataFinal;

        const veiculoId = getComboValue("lstVeiculos");
        const motoristaId = getComboValue("lstMotorista");

        let statusId = getComboValue("lstStatus");
        if (!statusId)
        {
            statusId = (veiculoId || motoristaId || data || temPeriodo) ? "Todas" : "Aberta";
        }

        return {
            veiculoId: veiculoId,
            motoristaId: motoristaId,
            statusId: statusId,
            data: temPeriodo ? "" : data,
            dataInicial: temPeriodo ? dataInicial : "",
            dataFinal: temPeriodo ? dataFinal : ""
        };
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "collectParamsFromUI", error);
        return { statusId: "Aberta" };
    }
}

/* ==========================
   Validação de Datas
   ========================== */
function validateDatesBeforeSearch()
{
    try
    {
        const dataInicial = ($("#txtDataInicial").val() || "").trim();
        const dataFinal = ($("#txtDataFinal").val() || "").trim();

        if ((dataInicial && !dataFinal) || (!dataInicial && dataFinal))
        {
            Alerta.Erro("Informação Ausente", "Para filtrar por período, preencha Data Inicial e Data Final.", "OK");
            return false;
        }

        return true;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "validateDatesBeforeSearch", error);
        return false;
    }
}

/* ==========================
   Upload de Imagem
   ========================== */
async function uploadImagemOcorrencia(file)
{
    try
    {
        const formData = new FormData();
        formData.append("file", file);

        const response = await fetch("/api/OcorrenciaViagem/UploadImagem", {
            method: "POST",
            body: formData
        });

        const data = await response.json();

        if (data.success)
        {
            imagemOcorrenciaAlterada = true;
            novaImagemOcorrencia = data.path || data.url || "";
            exibirPreviewImagem(novaImagemOcorrencia);
            AppToast.show("Verde", "Imagem enviada com sucesso!", 2000);
        }
        else
        {
            AppToast.show("Vermelho", data.message || "Erro ao enviar imagem.", 3000);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "uploadImagemOcorrencia", error);
        AppToast.show("Vermelho", "Erro ao enviar imagem.", 3000);
    }
}

/* ==========================
   Preview de Imagem
   ========================== */
function exibirPreviewImagem(src)
{
    try
    {
        const container = $("#divImagemOcorrencia");
        container.empty();

        if (!src)
        {
            container.html(`
                <div class="p-3 text-center border rounded bg-light" style="cursor:pointer;" onclick="$('#inputImagemOcorrencia').click();">
                    <i class="fa-duotone fa-image fa-3x text-muted mb-2"></i>
                    <p class="text-muted mb-0">Clique para adicionar imagem ou vídeo</p>
                </div>
            `);
            return;
        }

        const isVideo = /\.(mp4|webm)$/i.test(src);

        if (isVideo)
        {
            container.html(`
                <div class="position-relative">
                    <video src="${src}" controls style="max-width:100%; max-height:200px; border-radius:8px;"></video>
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1" onclick="removerImagemOcorrencia()">
                        <i class="fa-duotone fa-trash"></i>
                    </button>
                </div>
            `);
        }
        else
        {
            container.html(`
                <div class="position-relative">
                    <img src="${src}" alt="Preview" style="max-width:100%; max-height:200px; border-radius:8px; cursor:pointer;" onclick="$('#inputImagemOcorrencia').click();" />
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1" onclick="removerImagemOcorrencia()">
                        <i class="fa-duotone fa-trash"></i>
                    </button>
                </div>
            `);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "exibirPreviewImagem", error);
    }
}

/* ==========================
   Remover Imagem
   ========================== */
function removerImagemOcorrencia()
{
    try
    {
        imagemOcorrenciaAlterada = true;
        novaImagemOcorrencia = "";
        exibirPreviewImagem("");
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "removerImagemOcorrencia", error);
    }
}

/* ==========================
   Limpar Modal
   ========================== */
function limparModal()
{
    try
    {
        $("#txtId").val("");
        $("#txtResumo").val("");
        $("#txtImagemOcorrenciaAtual").val("");
        $("#chkStatusOcorrencia").val("");
        imagemOcorrenciaAlterada = false;
        novaImagemOcorrencia = "";

        const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
        const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];
        if (rteDesc) rteDesc.value = "";
        if (rteSol) rteSol.value = "";

        exibirPreviewImagem("");
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "limparModal", error);
    }
}

/* ==========================
   Fechar Modais
   ========================== */
function fecharModalOcorrencia()
{
    try
    {
        const modal = bootstrap.Modal.getInstance(document.getElementById("modalOcorrencia"));
        if (modal) modal.hide();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "fecharModalOcorrencia", error);
    }
}

function fecharModalBaixaRapida()
{
    try
    {
        const modal = bootstrap.Modal.getInstance(document.getElementById("modalBaixaRapida"));
        if (modal) modal.hide();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "fecharModalBaixaRapida", error);
    }
}

/* ==========================
   Carregar Ocorrência
   ========================== */
async function carregarOcorrencia(id)
{
    try
    {
        if (!id) {
             console.warn("ID inválido para carregar ocorrência");
             return;
        }

        const response = await fetch(`/api/OcorrenciaViagem/ObterOcorrencia?id=${id}`);
        
        if (!response.ok) {
            throw new Error(`Erro HTTP: ${response.status}`);
        }

        const text = await response.text();
        let data;
        try {
            data = JSON.parse(text);
        } catch (e) {
            console.error("Erro ao parsear resposta servida:", text);
            throw new Error("Resposta inválida do servidor (não é JSON).");
        }

        if (data.success && data.ocorrencia)
        {
            const oc = data.ocorrencia;

            $("#txtId").val(oc.ocorrenciaViagemId || "");
            $("#txtResumo").val(oc.resumoOcorrencia || "");
            $("#txtImagemOcorrenciaAtual").val(oc.imagemOcorrencia || "");
            $("#chkStatusOcorrencia").val(oc.statusOcorrencia || "Aberta");

            const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
            const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];

            if (rteDesc) rteDesc.value = oc.descricaoOcorrencia || "";
            if (rteSol) rteSol.value = oc.solucaoOcorrencia || "";

            exibirPreviewImagem(oc.imagemOcorrencia || "");

            // Atualizar título do modal
            const titulo = oc.statusOcorrencia === "Baixada" ? "Visualizar Ocorrência" : "Editar Ocorrência";
            $("#modalOcorrenciaLabel span").text(titulo);

            // Habilitar/desabilitar botões
            const baixada = oc.statusOcorrencia === "Baixada";
            $("#btnBaixarOcorrenciaModal").prop("disabled", baixada);
            $("#btnEditarOcorrencia").prop("disabled", baixada);

            new bootstrap.Modal(document.getElementById("modalOcorrencia")).show();
        }
        else
        {
            AppToast.show("Vermelho", data.message || "Erro ao carregar ocorrência.", 3000);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "carregarOcorrencia", error);
        AppToast.show("Vermelho", "Erro ao carregar ocorrência.", 3000);
    }
}

/* ==========================
   Verificar Solução
   ========================== */
function verificarSolucaoPreenchida(solucao)
{
    try
    {
        if (!solucao) return false;
        const texto = solucao.replace(/<[^>]*>/g, "").trim();
        return texto.length > 0;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "verificarSolucaoPreenchida", error);
        return false;
    }
}

/* ==========================
   Execução da Baixa
   ========================== */
async function executarBaixaOcorrencia(id, solucao, callbackSucesso)
{
    try
    {
        const payload = {
            OcorrenciaViagemId: id,
            SolucaoOcorrencia: solucao,
            StatusOcorrencia: "Baixada"
        };

        const response = await fetch("/api/OcorrenciaViagem/BaixarOcorrencia", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const data = await response.json();

        if (data.success)
        {
            AppToast.show("Verde", data.message || "Ocorrência baixada com sucesso!", 2000);
            if (callbackSucesso) callbackSucesso();
            if (dataTable) dataTable.ajax.reload(null, false);
        }
        else
        {
            AppToast.show("Vermelho", data.message || "Erro ao baixar ocorrência.", 3000);
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "executarBaixaOcorrencia", error);
        AppToast.show("Vermelho", "Erro ao baixar ocorrência.", 3000);
    }
}

/* ==========================
   Baixa com Validação
   ========================== */
async function processarBaixaComValidacao(id, solucaoAtual, callbackSucesso)
{
    try
    {
        if (verificarSolucaoPreenchida(solucaoAtual))
        {
            // Solução preenchida - executa diretamente
            await executarBaixaOcorrencia(id, solucaoAtual, callbackSucesso);
        }
        else
        {
            // Solução vazia - abre modal de baixa rápida
            if (callbackSucesso) callbackSucesso();
            $("#txtBaixaRapidaId").val(id);
            new bootstrap.Modal(document.getElementById("modalBaixaRapida")).show();
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "processarBaixaComValidacao", error);
    }
}

/* ==========================
   Ver Imagem/Vídeo
   ========================== */
function abrirVisualizacaoImagem(src)
{
    try
    {
        const container = $("#divImagemVisualizacao");
        container.empty();

        if (!src)
        {
            container.html('<p class="text-muted">Sem imagem disponível</p>');
            return;
        }

        const isVideo = /\.(mp4|webm)$/i.test(src);

        if (isVideo)
        {
            container.html(`<video src="${src}" controls style="max-width:100%; max-height:500px;"></video>`);
            $("#modalVisualizarImagem .modal-title").html('<i class="fa-duotone fa-video me-2"></i>Vídeo da Ocorrência');
        }
        else
        {
            container.html(`<img src="${src}" alt="Imagem" style="max-width:100%; max-height:500px;" />`);
            $("#modalVisualizarImagem .modal-title").html('<i class="fa-duotone fa-image me-2"></i>Imagem da Ocorrência');
        }

        new bootstrap.Modal(document.getElementById("modalVisualizarImagem")).show();
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "abrirVisualizacaoImagem", error);
    }
}

/* ==========================
   Inicialização
   ========================== */
$(document).ready(function ()
{
    try
    {
        // Carrega grid com status "Aberta" por padrão
        BuildGridOcorrencias({ statusId: "Aberta" });

        // Botão Filtrar
        $("#btnFiltrarOcorrencias").on("click", function ()
        {
            try
            {
                if (!validateDatesBeforeSearch()) return;
                const params = collectParamsFromUI();
                BuildGridOcorrencias(params);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnFiltrar.click", error);
            }
        });

        // Delegação de eventos para botões da tabela
        $(document).on("click", ".btn-editar-ocorrencia", function (e)
        {
            try
            {
                e.preventDefault();
                const id = $(this).data("id");
                if (id) carregarOcorrencia(id);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnEditar.click", error);
            }
        });

        $(document).on("click", ".btn-ver-imagem:not(.disabled)", function (e)
        {
            try
            {
                e.preventDefault();
                const imagem = $(this).data("imagem");
                if (imagem) abrirVisualizacaoImagem(imagem);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnVerImagem.click", error);
            }
        });

        $(document).on("click", ".btn-baixar:not(.disabled)", async function (e)
        {
            try
            {
                e.preventDefault();
                const id = $(this).data("id");
                if (id) await processarBaixaComValidacao(id, "", null);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnBaixar.click", error);
            }
        });

        // Botão Confirmar Baixa Rápida
        $("#btnConfirmarBaixaRapida").on("click", async function (e)
        {
            try
            {
                e.preventDefault();

                const $btn = $(this);
                if ($btn.data("busy")) return;

                const id = $("#txtBaixaRapidaId").val();
                const solucao = ($("#txtBaixaRapidaSolucao").val() || "").trim();

                if (!solucao)
                {
                    Alerta.Erro("Informação Ausente", "Preencha a Solução da Ocorrência.", "OK");
                    return;
                }

                $btn.data("busy", true).prop("disabled", true).html('<i class="fa-duotone fa-spinner-third fa-spin me-1"></i> Baixando...');

                await executarBaixaOcorrencia(id, solucao, function() {
                    fecharModalBaixaRapida();
                });

                $btn.data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i> Confirmar Baixa');
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnConfirmarBaixaRapida.click", error);
                $("#btnConfirmarBaixaRapida").data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i> Confirmar Baixa');
            }
        });

        // Limpar modal de baixa rápida ao fechar
        $("#modalBaixaRapida").on("hidden.bs.modal", function ()
        {
            try
            {
                $("#txtBaixaRapidaId").val("");
                $("#txtBaixaRapidaSolucao").val("");
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "modalBaixaRapida.hidden", error);
            }
        });

        // Botão Baixar no Modal de Edição
        $("#btnBaixarOcorrenciaModal").on("click", async function (e)
        {
            try
            {
                e.preventDefault();

                const id = $("#txtId").val();
                if (!id)
                {
                    Alerta.Erro("Erro", "ID da ocorrência não encontrado.", "OK");
                    return;
                }

                // Pega a solução do RTE
                const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];
                const solucaoAtual = rteSol?.value || "";

                await processarBaixaComValidacao(id, solucaoAtual, fecharModalOcorrencia);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnBaixarModal.click", error);
            }
        });

        // Botão Salvar
        $("#btnEditarOcorrencia").on("click", async function (e)
        {
            try
            {
                e.preventDefault();

                const $btn = $(this);
                if ($btn.data("busy")) return;

                const resumo = $("#txtResumo").val();
                if (!resumo)
                {
                    Alerta.Erro("Informação Ausente", "O Resumo da Ocorrência é obrigatório.", "OK");
                    return;
                }

                const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
                const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];

                let imagemFinal = $("#txtImagemOcorrenciaAtual").val() || "";
                if (imagemOcorrenciaAlterada)
                {
                    imagemFinal = novaImagemOcorrencia;
                }

                const payload = {
                    OcorrenciaViagemId: $("#txtId").val(),
                    ResumoOcorrencia: resumo,
                    DescricaoOcorrencia: rteDesc?.value || "",
                    SolucaoOcorrencia: rteSol?.value || "",
                    StatusOcorrencia: $("#chkStatusOcorrencia").val() || "Aberta",
                    ImagemOcorrencia: imagemFinal
                };

                $btn.data("busy", true).prop("disabled", true).html('<i class="fa-duotone fa-spinner-third fa-spin me-2"></i> Salvando...');

                const response = await fetch("/api/OcorrenciaViagem/EditarOcorrencia", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });

                const data = await response.json();

                if (data.success)
                {
                    AppToast.show("Verde", data.message || "Ocorrência atualizada!", 2000);
                    fecharModalOcorrencia();
                    if (dataTable) dataTable.ajax.reload(null, false);
                }
                else
                {
                    AppToast.show("Vermelho", data.message || "Erro ao salvar.", 2000);
                }

                $btn.data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i> Salvar Alterações');
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnSalvar.click", error);
                $("#btnEditarOcorrencia").data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i> Salvar Alterações');
            }
        });

        // Evento de seleção de imagem
        $("#inputImagemOcorrencia").on("change", function (e)
        {
            try
            {
                const file = e.target.files[0];
                if (!file) return;

                const tiposPermitidos = ["image/jpeg", "image/png", "image/gif", "image/webp", "video/mp4", "video/webm"];
                if (!tiposPermitidos.includes(file.type))
                {
                    Alerta.Erro("Tipo Inválido", "Selecione uma imagem (JPG, PNG, GIF, WebP) ou vídeo (MP4, WebM).", "OK");
                    return;
                }

                if (file.size > 50 * 1024 * 1024)
                {
                    Alerta.Erro("Arquivo muito grande", "O arquivo deve ter no máximo 50MB.", "OK");
                    return;
                }

                uploadImagemOcorrencia(file);
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "inputImagem.change", error);
            }
        });

        // Limpar modal ao fechar
        $("#modalOcorrencia").on("hidden.bs.modal", function ()
        {
            try
            {
                limparModal();
            }
            catch (error)
            {
                Alerta.TratamentoErroComLinha("ocorrencias.js", "modal.hidden", error);
            }
        });

        // Refresh UI dos RTEs ao abrir modal
        $("#modalOcorrencia").on("shown.bs.modal", function ()
        {
            try
            {
                document.getElementById("rteOcorrencias")?.ej2_instances?.[0]?.refreshUI();
                document.getElementById("rteSolucao")?.ej2_instances?.[0]?.refreshUI();
            }
            catch (_) { }
        });

        console.log("[ocorrencias.js] Inicialização concluída");
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "document.ready", error);
    }
});

/* ==========================
   Localização RTE Syncfusion
   ========================== */
try
{
    if (typeof ej !== "undefined" && ej.base && ej.base.L10n)
    {
        ej.base.L10n.load({
            "pt-BR": {
                richtexteditor: {
                    alignments: "Alinhamentos", justifyLeft: "Alinhar à Esquerda", justifyCenter: "Centralizar",
                    justifyRight: "Alinhar à Direita", justifyFull: "Justificar", fontName: "Fonte",
                    fontSize: "Tamanho", fontColor: "Cor da Fonte", backgroundColor: "Cor de Fundo",
                    bold: "Negrito", italic: "Itálico", underline: "Sublinhado", strikethrough: "Tachado",
                    clearFormat: "Limpar Formatação", cut: "Cortar", copy: "Copiar", paste: "Colar",
                    unorderedList: "Lista", orderedList: "Lista Numerada", indent: "Aumentar Recuo",
                    outdent: "Diminuir Recuo", undo: "Desfazer", redo: "Refazer",
                    createLink: "Inserir Link", image: "Inserir Imagem", fullscreen: "Maximizar",
                    formats: "Formatos", sourcecode: "Código Fonte"
                }
            }
        });
    }
}
catch (error)
{
    console.warn("Erro ao carregar localização RTE:", error);
}
