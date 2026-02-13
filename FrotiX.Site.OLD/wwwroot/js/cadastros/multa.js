/* ****************************************************************************************
 * ⚡ ARQUIVO: multa.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento completo do cadastro e edição de multas. Manipula uploads
 *                   de PDFs (Autuação, Penalidade, Comprovante, EDoc, Documentos), integração
 *                   com viewer de PDF, validações de campos obrigatórios, busca de viagens
 *                   associadas, e gerenciamento de contratos de veículos e motoristas.
 * 📥 ENTRADAS     : Dados de formulário HTML (campos de texto, listas Syncfusion, inputs),
 *                   eventos de upload de arquivos, eventos de mudança em dropdowns,
 *                   cliques em botões, dados de AJAX (viagens, fichas, empenhos)
 * 📤 SAÍDAS       : PDFs carregados no viewer, campos preenchidos com dados de API,
 *                   valores monetários formatados (BRL), validações executadas, modais
 *                   abertas/fechadas, toasts de sucesso/erro exibidos
 * 🔗 CHAMADA POR  : Pages/Multa/UpsertAutuacao.cshtml, Pages/Multa/UpsertPenalidade.cshtml
 * 🔄 CHAMA        : $.ajax (jQuery), Syncfusion ej.inputs.Uploader, Syncfusion DropdownList,
 *                   SweetAlert (swal), AppToast.show, Alerta.TratamentoErroComLinha,
 *                   FtxSpin (implícito em validações), moment.js (formatos de data)
 * 📦 DEPENDÊNCIAS : jQuery 3.x, Syncfusion EJ2 (Uploader, DropdownList, RichTextEditor),
 *                   Bootstrap 5.3 (Modal), SweetAlert 2.x, Alerta.js, Font Awesome 6.x
 *                   (fa-duotone), FullCalendar (contexto de viagens), Kendo UI (implícito)
 * 📝 OBSERVAÇÕES  : Todas as funções têm try-catch com Alerta.TratamentoErroComLinha.
 *                   Utiliza validações de dados obrigatórios antes de submit. Remove acentos
 *                   de nomes de PDF (tiraAcento). Carrega primeiro PDF disponível em modo
 *                   edição (carregarPrimeiroPDF com prioridade: Penalidade→Autuação→Comprovante
 *                   →EDoc→Outros). Gerencia duas variáveis globais de controle: EscolhendoVeiculo
 *                   e EscolhendoMotorista para evitar loops de validação. Integração com
 *                   API /api/Multa/* para buscas de viagem, validações, saldo de empenho.
 *
 * 📋 ÍNDICE DE FUNÇÕES (28 funções principais):
 *
 * ┌─ FUNÇÕES UTILITÁRIAS (3 funções) ───────────────────────────────────────────────────┐
 * │ 1. tiraAcento(frase) → string (remove acentos, substitui espaços por underscore)    │
 * │ 2. getMainViewer() → object|null (obtém instância do PDF viewer)                   │
 * │ 3. loadPdfInViewer(fileName) → void (carrega PDF no viewer principal)              │
 * │                                                                                      │
 * ├─ CALLBACKS DE UPLOAD (5 funções) ──────────────────────────────────────────────────┐
 * │ 4. onSuccessAutuacao(e) → void (callback upload PDF Autuação)                      │
 * │ 5. onSuccessPenalidade(e) → void (callback upload PDF Penalidade)                  │
 * │ 6. onSuccessComprovante(e) → void (callback upload PDF Comprovante)                │
 * │ 7. onSuccessEDoc(e) → void (callback upload PDF EDoc)                              │
 * │ 8. onSuccessDocumentos(e) → void (callback upload Outros Documentos)               │
 * │                                                                                      │
 * ├─ INICIALIZAÇÃO (3 funções) ────────────────────────────────────────────────────────┐
 * │ 9. configurarControlesSyncfusion() → void (cria Uploaders Syncfusion para 5 PDFs)  │
 * │ 10. verificarModoEdicao() → void (detecta modo edição vs criação)                   │
 * │ 11. carregarPrimeiroPDF() → void (prioridade: Penalidade→Autuação→...)              │
 * │ 12. inicializarValoresMonetarios() → void (formata valores como "0,00")             │
 * │ 13. inicializarNovoRegistro() → void (limpa listas em modo criação)                │
 * │                                                                                      │
 * ├─ VALIDAÇÕES & FORMATAÇÃO (2 funções) ────────────────────────────────────────────┐
 * │ 14. stopEnterSubmitting(e) → boolean (previne submit com Enter, exceto em div)      │
 * │ 15. moeda(a, e, r, t) → boolean (formata entrada monetária em tempo real)           │
 * │                                                                                      │
 * ├─ EVENT HANDLERS DROPDOWNS (8 funções) ──────────────────────────────────────────────┐
 * │ 16. lstOrgaoChange() → void (ao mudar órgão, limpa empenhos e carrega novos)        │
 * │ 17. lstEmpenhosChange() → void (ao mudar empenho, busca saldo)                      │
 * │ 18. lstVeiculo_Select() → void (marca flag EscolhendoVeiculo=true)                  │
 * │ 19. lstVeiculo_Change() → void (ao mudar veículo, carrega contrato/ata)             │
 * │ 20. lstContratoVeiculo_Change() → void (valida se veículo pertence ao contrato)     │
 * │ 21. lstAtaVeiculo_Change() → void (valida se veículo pertence à ata)                │
 * │ 22. lstMotorista_Select() → void (marca flag EscolhendoMotorista=true)              │
 * │ 23. lstMotorista_Change() → void (ao mudar motorista, carrega contrato)             │
 * │ 24. lstContratoMotorista_Change() → void (valida se motorista pertence ao contrato) │
 * │                                                                                      │
 * ├─ HANDLERS BOTÕES & EVENTOS (9 funções) ───────────────────────────────────────────┐
 * │ 25. #btnSubmit.click (event) → void (validações completas antes de submit)          │
 * │ 26. .btnViagem.click (event) → void (procura viagem associada à multa)              │
 * │ 27. .btnFicha.click (event) → void (procura ficha de vistoria)                      │
 * │ 28. #txtNumInfracao.focusout (event) → void (valida duplicação de número)           │
 * │ 29. .btnComprovante.click (event) → void (abre modal de comprovante)                │
 * │ 30. .btnNotificacao.click (event) → void (abre modal de autuação)                   │
 * │ 31. #btnFecharModalComprovante.click → void (fecha modal comprovante)               │
 * │ 32. #btnFecharModalFichaVistoria.click → void (fecha modal ficha)                   │
 * │ 33. toolbarClick(e) → void (configura XSRF-TOKEN no RTE upload)                     │
 * │                                                                                      │
 * ├─ CONFIGURAÇÕES ────────────────────────────────────────────────────────────────────┐
 * │ 34. document.ready → void (inicializa tudo ao carregar página)                      │
 * │ 35. #modalFicha.on("show.bs.modal") → void (carrega imagem de ficha)                │
 * │ 36. #modalComprovante.on("show.bs.modal") → void (carrega PDF comprovante)          │
 * │ 37. ej.base.L10n.load (pt-BR) → void (localização RichTextEditor)                   │
 * │                                                                                      │
 * ├─ VARIÁVEIS GLOBAIS (5 variáveis) ──────────────────────────────────────────────────┐
 * │ - ViagemId: string (ID da viagem encontrada na busca)                               │
 * │ - ComprovantePDF: string (caminho do PDF de comprovante)                            │
 * │ - ComprovantePDF2: string (segundo PDF de comprovante, se houver)                   │
 * │ - EscolhendoVeiculo: boolean (flag para evitar loop de validação em veículo)        │
 * │ - EscolhendoMotorista: boolean (flag para evitar loop de validação em motorista)    │
 * │                                                                                      │
 * └─────────────────────────────────────────────────────────────────────────────────────┘
 *
 * ⚠️ FLUXO DE EXECUÇÃO CRÍTICO:
 *
 *   1. $(document).ready() dispara:
 *      - configurarControlesSyncfusion() cria os 5 Uploaders
 *      - verificarModoEdicao() detecta modo
 *
 *   2. Se EDIÇÃO (MultaId presente):
 *      - lstInfracao recebe TipoMultaId
 *      - carregarPrimeiroPDF() carrega PDF (prioridade)
 *      - inicializarValoresMonetarios() formata valores
 *      - lstEmpenhosChange() é chamada manualmente
 *
 *   3. Se CRIAÇÃO:
 *      - inicializarNovoRegistro() limpa listas
 *      - inicializarValoresMonetarios() formata com "0,00"
 *
 *   4. Submit (#btnSubmit):
 *      - Valida 12 campos obrigatórios
 *      - Se OK, clica #btnEscondido (submete form real)
 *      - Se erro, exibe SweetAlert
 *
 * 📌 ENDPOINTS API UTILIZADOS:
 *
 *   GET  /api/Upload/save                    → Upload arquivo
 *   GET  /api/Upload/remove                  → Remove arquivo
 *   GET  /api/Multa/PegaInstrumentoVeiculo   → Instrumento (contrato/ata) do veículo
 *   GET  /api/Multa/ValidaContratoVeiculo    → Valida veículo-contrato
 *   GET  /api/Multa/ValidaAtaVeiculo         → Valida veículo-ata
 *   GET  /api/Multa/PegaContratoMotorista    → Contrato do motorista
 *   GET  /api/Multa/ValidaContratoMotorista  → Valida motorista-contrato
 *   POST /api/Multa/ProcuraViagem            → Busca viagem por veículo/data/hora
 *   POST /api/Multa/ProcuraFicha             → Busca ficha de vistoria
 *   GET  /Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos → Empenhos do órgão
 *   GET  /Multa/UpsertAutuacao?handler=PegaSaldoEmpenho → Saldo do empenho
 *   GET  /api/Multa/MultaExistente           → Verifica duplicação de número
 *   GET  /api/Viagem/PegaFichaModal          → Imagem da ficha
 *
 * ✅ DATA DE DOCUMENTAÇÃO: 02/02/2026
 * ✅ VERSÃO PADRÃO: FrotiX 2026 v1.0
 *
 **************************************************************************************** */

// Função para remover acentos
function tiraAcento(frase)
{
    try
    {
        if (!frase) return '';

        const semAcento = frase
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/\s+/g, '_')
            .toUpperCase();

        return semAcento;
    } catch (error)
    {
        console.error('Erro em tiraAcento:', error);
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "tiraAcento", error);
        }
        return '';
    }
}

// Função para obter o viewer principal
function getMainViewer()
{
    try
    {
        return document.getElementById("pdfviewer")?.ej2_instances?.[0] || null;
    } catch (err)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "getMainViewer", err);
        }
        return null;
    }
}

// Função para carregar PDF no viewer principal
function loadPdfInViewer(fileName)
{
    try
    {
        const viewer = getMainViewer();
        if (!viewer)
        {
            console.error("Viewer não encontrado");
            return;
        }

        viewer.documentPath = fileName;
        viewer.dataBind();
        viewer.load(fileName, null);
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "loadPdfInViewer", error);
        }
        console.error(error);
    }
}

// Funções de callback para os uploads
function onSuccessAutuacao(e)
{
    try
    {
        var files = e.files;
        if (!files || files.length === 0) return;

        var fileName = tiraAcento(files[0].name);
        document.getElementById("txtAutuacaoPDF")?.setAttribute('value', fileName);

        loadPdfInViewer(fileName);

        if (window.AppToast?.show)
        {
            AppToast.show('Verde', 'PDF de Autuação enviado com sucesso!', 3000);
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "onSuccessAutuacao", error);
        }
    }
}

function onSuccessPenalidade(e)
{
    try
    {
        var files = e.files;
        if (!files || files.length === 0) return;

        var fileName = tiraAcento(files[0].name);
        document.getElementById("txtPenalidadePDF")?.setAttribute('value', fileName);

        loadPdfInViewer(fileName);

        if (window.AppToast?.show)
        {
            AppToast.show('Verde', 'PDF de Penalidade enviado com sucesso!', 3000);
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "onSuccessPenalidade", error);
        }
    }
}

function onSuccessComprovante(e)
{
    try
    {
        var files = e.files;
        if (!files || files.length === 0) return;

        var fileName = tiraAcento(files[0].name);
        document.getElementById("txtComprovantePDF")?.setAttribute('value', fileName);

        loadPdfInViewer(fileName);

        if (window.AppToast?.show)
        {
            AppToast.show('Verde', 'Comprovante enviado com sucesso!', 3000);
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "onSuccessComprovante", error);
        }
    }
}

function onSuccessEDoc(e)
{
    try
    {
        var files = e.files;
        if (!files || files.length === 0) return;

        var fileName = tiraAcento(files[0].name);
        document.getElementById("txtEDocPDF")?.setAttribute('value', fileName);

        loadPdfInViewer(fileName);

        if (window.AppToast?.show)
        {
            AppToast.show('Verde', 'Processo EDoc enviado com sucesso!', 3000);
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "onSuccessEDoc", error);
        }
    }
}

function onSuccessDocumentos(e)
{
    try
    {
        var files = e.files;
        if (!files || files.length === 0) return;

        var fileName = tiraAcento(files[0].name);
        document.getElementById("txtOutrosDocumentosPDF")?.setAttribute('value', fileName);

        loadPdfInViewer(fileName);

        if (window.AppToast?.show)
        {
            AppToast.show('Verde', 'Documento enviado com sucesso!', 3000);
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "onSuccessDocumentos", error);
        }
    }
}

// Variáveis globais
var ViagemId = '';
var ComprovantePDF = '';
var ComprovantePDF2 = '';
var EscolhendoMotorista = false;
var EscolhendoVeiculo = false;

// Inicializar TUDO dentro do document.ready
$(document).ready(function ()
{
    try
    {
        // Configura os controles Syncfusion
        configurarControlesSyncfusion();

        // Carrega dados se for modo edição
        verificarModoEdicao();
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "document.ready", error);
        }
    }
});

function configurarControlesSyncfusion()
{
    try
    {
        // Upload do PDF da Autuação
        if ($("#inputAutuacaoPDF").length)
        {
            var uploadAutuacao = new ej.inputs.Uploader({
                asyncSettings: {
                    saveUrl: "/api/Upload/save",
                    removeUrl: "/api/Upload/remove"
                },
                allowedExtensions: '.pdf',
                multiple: false,
                autoUpload: true,
                success: onSuccessAutuacao,
                failure: function (args)
                {
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', 'Erro no upload da Autuação', 3000);
                    }
                    console.error("Erro no upload da Autuação:", args);
                }
            });
            uploadAutuacao.appendTo('#inputAutuacaoPDF');
        }

        // Upload do PDF da Penalidade
        if ($("#pdf").length)
        {
            var uploadPenalidade = new ej.inputs.Uploader({
                asyncSettings: {
                    saveUrl: "/api/Upload/save",
                    removeUrl: "/api/Upload/remove"
                },
                allowedExtensions: '.pdf',
                multiple: false,
                autoUpload: true,
                success: onSuccessPenalidade,
                failure: function (args)
                {
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', 'Erro no upload da Penalidade', 3000);
                    }
                    console.error("Erro no upload da Penalidade:", args);
                }
            });
            uploadPenalidade.appendTo('#pdf');
        }

        // Upload do PDF do Comprovante
        if ($("#flComprovante").length)
        {
            var uploadComprovante = new ej.inputs.Uploader({
                asyncSettings: {
                    saveUrl: "/api/Upload/save",
                    removeUrl: "/api/Upload/remove"
                },
                allowedExtensions: '.pdf',
                multiple: false,
                autoUpload: true,
                success: onSuccessComprovante,
                failure: function (args)
                {
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', 'Erro no upload do Comprovante', 3000);
                    }
                    console.error("Erro no upload do Comprovante:", args);
                }
            });
            uploadComprovante.appendTo('#flComprovante');
        }

        // Upload do Processo eDoc
        if ($("#inputEDocPDF").length)
        {
            var uploadEDoc = new ej.inputs.Uploader({
                asyncSettings: {
                    saveUrl: "/api/Upload/save",
                    removeUrl: "/api/Upload/remove"
                },
                allowedExtensions: '.pdf',
                multiple: false,
                autoUpload: true,
                success: onSuccessEDoc,
                failure: function (args)
                {
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', 'Erro no upload do eDoc', 3000);
                    }
                    console.error("Erro no upload do eDoc:", args);
                }
            });
            uploadEDoc.appendTo('#inputEDocPDF');
        }

        // Upload de Outros Documentos
        if ($("#inputOutrosDocumentosPDF").length)
        {
            var uploadDocumentos = new ej.inputs.Uploader({
                asyncSettings: {
                    saveUrl: "/api/Upload/save",
                    removeUrl: "/api/Upload/remove"
                },
                allowedExtensions: '.pdf',
                multiple: false,
                autoUpload: true,
                success: onSuccessDocumentos,
                failure: function (args)
                {
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', 'Erro no upload de Outros Documentos', 3000);
                    }
                    console.error("Erro no upload de Outros Documentos:", args);
                }
            });
            uploadDocumentos.appendTo('#inputOutrosDocumentosPDF');
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "configurarControlesSyncfusion", error);
        }
    }
}

function verificarModoEdicao()
{
    try
    {
        // Pega o valor do MultaId do campo hidden
        var multaId = $('#MultaObj_Multa_MultaId').val() || '';

        if (multaId && multaId != '00000000-0000-0000-0000-000000000000')
        {
            console.log('Modo Edição - MultaId:', multaId);

            // Seleciona a Infração da Lista
            setTimeout(function ()
            {
                try
                {
                    var lstInfracao = $("#lstInfracao").data("kendoDropDownList");
                    if (lstInfracao)
                    {
                        var tipoMultaId = $('#MultaObj_Multa_TipoMultaId').val();
                        if (tipoMultaId)
                        {
                            lstInfracao.value(tipoMultaId.toString());
                        }
                    }
                } catch (error)
                {
                    if (window.Alerta?.TratamentoErroComLinha)
                    {
                        Alerta.TratamentoErroComLinha("multa.js", "lstInfracao.setValue", error);
                    }
                }
            }, 500);

            // Carrega o primeiro PDF encontrado
            setTimeout(function ()
            {
                carregarPrimeiroPDF();
            }, 1000);

            // Define valores monetários
            try
            {
                inicializarValoresMonetarios();
            } catch (error)
            {
                if (window.Alerta?.TratamentoErroComLinha)
                {
                    Alerta.TratamentoErroComLinha("multa.js", "valoresMonetarios", error);
                }
            }

            // Chama função de mudança de empenhos se existir
            try
            {
                if (typeof lstEmpenhosChange === 'function')
                {
                    lstEmpenhosChange();
                }
            } catch (error)
            {
                if (window.Alerta?.TratamentoErroComLinha)
                {
                    Alerta.TratamentoErroComLinha("multa.js", "lstEmpenhosChange", error);
                }
            }
        } else
        {
            console.log('Modo Criação - Novo Registro');
            inicializarNovoRegistro();
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "verificarModoEdicao", error);
        }
    }
}

function carregarPrimeiroPDF()
{
    try
    {
        // Tenta carregar o primeiro PDF encontrado na ordem de prioridade
        var pdfs = [
            { campo: 'txtPenalidadePDF', nome: 'Penalidade' },
            { campo: 'txtAutuacaoPDF', nome: 'Autuação' },
            { campo: 'txtComprovantePDF', nome: 'Comprovante' },
            { campo: 'txtEDocPDF', nome: 'EDoc' },
            { campo: 'txtOutrosDocumentosPDF', nome: 'Outros Documentos' }
        ];

        for (var i = 0; i < pdfs.length; i++)
        {
            var pdfPath = $('#' + pdfs[i].campo).val();
            if (pdfPath && pdfPath != '' && pdfPath != 'null')
            {
                console.log('Carregando PDF de ' + pdfs[i].nome + ':', pdfPath);
                loadPdfInViewer(pdfPath);
                break;
            }
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "carregarPrimeiroPDF", error);
        }
    }
}

function inicializarValoresMonetarios()
{
    var valorAteVencimento = $('#txtValorAteVencimento').val();
    var valorPosVencimento = $('#txtValorPosVencimento').val();
    var valorPago = $('#txtValorPago').val();

    if (!valorAteVencimento || valorAteVencimento == '0' || valorAteVencimento == 'null' || valorAteVencimento == '')
    {
        $('#txtValorAteVencimento').val("0,00");
    }

    if (!valorPosVencimento || valorPosVencimento == '0' || valorPosVencimento == 'null' || valorPosVencimento == '')
    {
        $('#txtValorPosVencimento').val("0,00");
    }

    if (!valorPago || valorPago == '0' || valorPago == 'null' || valorPago == '')
    {
        $('#txtValorPago').val("0,00");
    }
}

function inicializarNovoRegistro()
{
    try
    {
        // Limpa listas
        var lstContratoVeiculo = $("#lstContratoVeiculo").data("kendoComboBox");
        if (lstContratoVeiculo)
        {
            lstContratoVeiculo.value(""); lstContratoVeiculo.text("");
        }

        var lstContratoMotorista = $("#lstContratoMotorista").data("kendoComboBox");
        if (lstContratoMotorista)
        {
            lstContratoMotorista.value(""); lstContratoMotorista.text("");
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "limparListas", error);
        }
    }

    try
    {
        inicializarValoresMonetarios();
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "valoresMonetariosNovo", error);
        }
    }
}

// Restante das funções auxiliares...
function stopEnterSubmitting(e)
{
    try
    {
        if (e.keyCode == 13)
        {
            var src = e.srcElement || e.target;
            if (src.tagName.toLowerCase() != "div")
            {
                if (e.preventDefault)
                {
                    e.preventDefault();
                } else
                {
                    e.returnValue = false;
                }
            }
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "stopEnterSubmitting", error);
        }
    }
}

function moeda(a, e, r, t)
{
    try
    {
        let n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
        if (13 == o || 8 == o) return true;
        if (n = String.fromCharCode(o), -1 == "0123456789".indexOf(n)) return false;

        for (u = a.value.length, h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++);
        for (l = ""; h < u; h++)
        {
            if (-1 != "0123456789".indexOf(a.value.charAt(h)))
            {
                l += a.value.charAt(h);
            }
        }

        if (l += n, 0 == (u = l.length) && (a.value = ""), 1 == u && (a.value = "0" + r + "0" + l), 2 == u && (a.value = "0" + r + l), u > 2)
        {
            for (ajd2 = "", j = 0, h = u - 3; h >= 0; h--)
            {
                if (3 == j)
                {
                    ajd2 += e;
                    j = 0;
                }
                ajd2 += l.charAt(h);
                j++;
            }
            for (a.value = "", tamanho2 = ajd2.length, h = tamanho2 - 1; h >= 0; h--)
            {
                a.value += ajd2.charAt(h);
            }
            a.value += r + l.substr(u - 2, u);
        }
        return false;
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "moeda", error);
        }
        return false;
    }
}

// Event handlers para listas
function lstOrgaoChange()
{
    try
    {
        var lstEmpenhos = $("#lstEmpenhos").data("kendoDropDownList");
        if (lstEmpenhos)
        {
            lstEmpenhos.setDataSource(new kendo.data.DataSource({ data: [] }));
            lstEmpenhos.value(""); lstEmpenhos.text("");
        }
        $('#txtEmpenhoMultaId').attr('value', "");

        var lstOrgao = $("#lstOrgao").data("kendoComboBox");
        if (!lstOrgao || !lstOrgao.value())
        {
            return;
        }

        var orgaoid = String(lstOrgao.value());

        $.ajax({
            url: "/Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos",
            method: "GET",
            datatype: "json",
            data: { id: orgaoid },
            success: function (res)
            {
                if (res.data.length != 0)
                {
                    let EmpenhoList = [];
                    for (var i = 0; i < res.data.length; ++i)
                    {
                        let empenho = {
                            EmpenhoMultaId: res.data[i].empenhoMultaId,
                            NotaEmpenho: res.data[i].notaEmpenho
                        };
                        EmpenhoList.push(empenho);
                    }
                    if (lstEmpenhos)
                    {
                        lstEmpenhos.setDataSource(new kendo.data.DataSource({ data: EmpenhoList }));
                    }
                }
            }
        });

        if (lstEmpenhos)
        {
            lstEmpenhos.dataSource.read();
        }

        swal({
            title: "Empenho do Órgão",
            text: "Já existe o empenho correto cadastrado para o órgão?",
            icon: "info",
            buttons: { ok: "Ok" }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstOrgaoChange", error);
        }
    }
}

function lstEmpenhosChange()
{
    try
    {
        var lstEmpenhos = $("#lstEmpenhos").data("kendoDropDownList");
        if (!lstEmpenhos) return;

        $('#txtEmpenhoMultaId').attr('value', lstEmpenhos.value());

        var empenhoid = String(lstEmpenhos.value());

        $.ajax({
            url: "/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho",
            method: "GET",
            datatype: "json",
            data: { id: empenhoid },
            success: function (res)
            {
                var saldoempenho = res.data;
                $("#txtSaldoEmpenho").val(
                    Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(saldoempenho)
                );
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstEmpenhosChange", error);
        }
    }
}

// Funções de veículo
function lstVeiculo_Select()
{
    try
    {
        EscolhendoVeiculo = true;
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstVeiculo_Select", error);
        }
    }
}

function lstVeiculo_Change()
{
    try
    {
        var lstVeiculo = $("#lstVeiculo").data("kendoComboBox");
        if (!lstVeiculo || lstVeiculo.value() === '')
        {
            return;
        }

        var veiculoId = lstVeiculo.value();

        $.ajax({
            url: "/api/Multa/PegaInstrumentoVeiculo",
            method: "GET",
            datatype: "json",
            data: { Id: veiculoId },
            success: function (data)
            {
                var lstContratoVeiculo = $("#lstContratoVeiculo").data("kendoComboBox");
                var lstAtaVeiculo = $("#lstAtaVeiculo").data("kendoComboBox");

                if (data.instrumentoid != null)
                {
                    if (data.instrumento == "contrato")
                    {
                        if (lstContratoVeiculo)
                        {
                            lstContratoVeiculo.value(data.instrumentoid.toString());
                        }
                        if (lstAtaVeiculo)
                        {
                            lstAtaVeiculo.value(""); lstAtaVeiculo.text("");
                        }
                    } else
                    {
                        if (lstContratoVeiculo)
                        {
                            lstContratoVeiculo.value(""); lstContratoVeiculo.text("");
                        }
                        if (lstAtaVeiculo)
                        {
                            lstAtaVeiculo.value(data.instrumentoid.toString());
                        }
                    }
                } else
                {
                    if (lstContratoVeiculo)
                    {
                        lstContratoVeiculo.value(""); lstContratoVeiculo.text("");
                    }
                    swal({
                        title: "Atenção ao Contrato do Veículo",
                        text: "O veículo escolhido não possui contrato/ata!",
                        icon: "info",
                        buttons: { ok: "Ok" }
                    });
                }
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstVeiculo_Change", error);
        }
    }
}

function lstContratoVeiculo_Change()
{
    try
    {
        if (EscolhendoVeiculo)
        {
            EscolhendoVeiculo = false;
            return;
        }

        var lstAtaVeiculo = $("#lstAtaVeiculo").data("kendoComboBox");
        if (lstAtaVeiculo)
        {
            lstAtaVeiculo.value(""); lstAtaVeiculo.text("");
        }

        var lstContratoVeiculo = $("#lstContratoVeiculo").data("kendoComboBox");
        var lstVeiculo = $("#lstVeiculo").data("kendoComboBox");

        if (!lstContratoVeiculo || !lstVeiculo) return;
        if (lstContratoVeiculo.value() === '' || lstVeiculo.value() === '') return;

        var veiculoId = lstVeiculo.value();
        var contratoId = lstContratoVeiculo.value();

        $.ajax({
            url: "/api/Multa/ValidaContratoVeiculo",
            method: "GET",
            datatype: "json",
            data: { veiculoId: veiculoId, contratoId: contratoId },
            success: function (data)
            {
                if (data.success === false)
                {
                    swal({
                        title: "Alerta no Contrato do Veículo",
                        text: "O veículo escolhido não pertence a esse contrato!",
                        icon: "warning",
                        buttons: { ok: "Ok" }
                    });
                    if (lstVeiculo)
                    {
                        lstVeiculo.value(""); lstVeiculo.text("");
                    }
                }
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstContratoVeiculo_Change", error);
        }
    }
}

function lstAtaVeiculo_Change()
{
    try
    {
        if (EscolhendoVeiculo)
        {
            EscolhendoVeiculo = false;
            return;
        }

        var lstContratoVeiculo = $("#lstContratoVeiculo").data("kendoComboBox");
        if (lstContratoVeiculo)
        {
            lstContratoVeiculo.value(""); lstContratoVeiculo.text("");
        }

        var lstAtaVeiculo = $("#lstAtaVeiculo").data("kendoComboBox");
        var lstVeiculo = $("#lstVeiculo").data("kendoComboBox");

        if (!lstAtaVeiculo || !lstVeiculo) return;
        if (lstAtaVeiculo.value() === '' || lstVeiculo.value() === '') return;

        var veiculoId = lstVeiculo.value();
        var ataId = lstAtaVeiculo.value();

        $.ajax({
            url: "/api/Multa/ValidaAtaVeiculo",
            method: "GET",
            datatype: "json",
            data: { veiculoId: veiculoId, ataId: ataId },
            success: function (data)
            {
                if (data.success === false)
                {
                    swal({
                        title: "Alerta na Ata do Veículo",
                        text: "O veículo escolhido não pertence a essa ata!",
                        icon: "warning",
                        buttons: { ok: "Ok" }
                    });
                    if (lstVeiculo)
                    {
                        lstVeiculo.value(""); lstVeiculo.text("");
                    }
                }
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstAtaVeiculo_Change", error);
        }
    }
}

// Funções de motorista
function lstMotorista_Select()
{
    try
    {
        EscolhendoMotorista = true;
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstMotorista_Select", error);
        }
    }
}

function lstMotorista_Change()
{
    try
    {
        var lstMotorista = $("#lstMotorista").data("kendoComboBox");
        if (!lstMotorista || lstMotorista.value() === '')
        {
            return;
        }

        var motoristaId = lstMotorista.value();

        $.ajax({
            url: "/api/Multa/PegaContratoMotorista",
            method: "GET",
            datatype: "json",
            data: { Id: motoristaId },
            success: function (data)
            {
                var lstContratoMotorista = $("#lstContratoMotorista").data("kendoComboBox");

                if (data.contratoid != '')
                {
                    if (lstContratoMotorista)
                    {
                        lstContratoMotorista.value(data.contratoid.toString());
                    }
                } else
                {
                    if (lstContratoMotorista)
                    {
                        lstContratoMotorista.value(""); lstContratoMotorista.text("");
                    }
                    swal({
                        title: "Atenção ao Contrato do Motorista",
                        text: "O motorista escolhido não possui contrato!",
                        icon: "info",
                        buttons: { ok: "Ok" }
                    });
                }
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstMotorista_Change", error);
        }
    }
}

function lstContratoMotorista_Change()
{
    try
    {
        if (EscolhendoMotorista)
        {
            EscolhendoMotorista = false;
            return;
        }

        var lstContratoMotorista = $("#lstContratoMotorista").data("kendoComboBox");
        var lstMotorista = $("#lstMotorista").data("kendoComboBox");

        if (!lstContratoMotorista || !lstMotorista) return;
        if (lstContratoMotorista.value() === '' || lstMotorista.value() === '') return;

        var motoristaId = lstMotorista.value();
        var contratoId = lstContratoMotorista.value();

        $.ajax({
            url: "/api/Multa/ValidaContratoMotorista",
            method: "GET",
            datatype: "json",
            data: { veiculoId: motoristaId, contratoId: contratoId },
            success: function (data)
            {
                if (data.success === false)
                {
                    swal({
                        title: "Alerta no Contrato do Motorista",
                        text: "O motorista escolhido não pertence a esse contrato!",
                        icon: "warning",
                        buttons: { ok: "Ok" }
                    });
                    if (lstMotorista)
                    {
                        lstMotorista.value(""); lstMotorista.text("");
                    }
                }
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "lstContratoMotorista_Change", error);
        }
    }
}

// Event handlers de botões
$("#btnFecharModalComprovante").click(function ()
{
    try
    {
        $('.modal-backdrop').hide();
        $('body').removeClass('modal-open');
        $("#modalComprovante").hide();
        $('body').css({ 'overflow': 'visible' });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "#btnFecharModalComprovante.click", error);
        }
    }
});

$("#btnFecharModalFichaVistoria").click(function ()
{
    try
    {
        $('.modal-backdrop').hide();
        $('body').removeClass('modal-open');
        $("#modalFicha").hide();
        $('body').css({ 'overflow': 'visible' });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "#btnFecharModalFichaVistoria.click", error);
        }
    }
});

$("#btnSubmit").click(function (event)
{
    try
    {
        event.preventDefault();

        // Validações
        if (document.getElementById("txtNumInfracao").value === "")
        {
            swal({ title: "Informação Ausente", text: "O número da Infração é obrigatório", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtDataInfracao").value === "")
        {
            swal({ title: "Informação Ausente", text: "A Data da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtHoraInfracao").value === "")
        {
            swal({ title: "Informação Ausente", text: "A Hora da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtDataNotificacao").value === "")
        {
            swal({ title: "Informação Ausente", text: "A Data da Notificação é obrigatória", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtDataLimite").value === "")
        {
            swal({ title: "Informação Ausente", text: "A Data Limite para Interposição de Defesa é obrigatória", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        var lstStatus = $("#lstStatus").data("kendoComboBox");
        if (lstStatus && !lstStatus.value())
        {
            swal({ title: "Informação Ausente", text: "O Status é obrigatório", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtLocalizacao").value === "")
        {
            swal({ title: "Informação Ausente", text: "A Localização da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        var lstInfracao = $("#lstInfracao").data("kendoDropDownList");
        if (lstInfracao && !lstInfracao.value())
        {
            swal({ title: "Informação Ausente", text: "A Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        var lstOrgao = $("#lstOrgao").data("kendoComboBox");
        if (lstOrgao && !lstOrgao.value())
        {
            swal({ title: "Informação Ausente", text: "O Órgão Autuante é obrigatório", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        var lstVeiculo = $("#lstVeiculo").data("kendoComboBox");
        if (lstVeiculo && !lstVeiculo.value())
        {
            swal({ title: "Informação Ausente", text: "O Veículo é obrigatório", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtValorAteVencimento").value === "" || document.getElementById("txtValorAteVencimento").value === "0")
        {
            swal({ title: "Informação Ausente", text: "O Valor Até o Vencimento é obrigatório", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        if (document.getElementById("txtValorPosVencimento").value === "" || document.getElementById("txtValorPosVencimento").value === "0")
        {
            swal({ title: "Informação Ausente", text: "O Valor Após o Vencimento é obrigatório", icon: "error", buttons: { ok: "Ok" } });
            return;
        }

        $("#btnEscondido").click();
        $("#btnSubmit").prop("disabled", true);
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "#btnSubmit.click", error);
        }
    }
});

// Procura Viagem Associada à Multa
$(document).on('click', '.btnViagem', function ()
{
    if (document.getElementById("txtDataInfracao").value === "")
    {
        swal({ title: "Informação Ausente", text: "A Data da Infração deve ser informada!", icon: "error", buttons: { ok: "Ok" } });
        return;
    }

    if (document.getElementById("txtHoraInfracao").value === "")
    {
        swal({ title: "Informação Ausente", text: "A Hora da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
        return;
    }

    var lstVeiculo = $("#lstVeiculo").data("kendoComboBox");
    if (!lstVeiculo || !lstVeiculo.value())
    {
        swal({ title: "Informação Ausente", text: "O Veículo deve ser informado!", icon: "error", buttons: { ok: "Ok" } });
        return;
    }

    var dataToPost = JSON.stringify({
        'VeiculoId': lstVeiculo.value(),
        'Data': document.getElementById("txtDataInfracao").value,
        'Hora': document.getElementById("txtHoraInfracao").value
    });

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
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Verde', data.message);
                    }
                    $('#txtNoFichaVistoria').attr('value', data.nofichavistoria);
                    $('#txtNoFichaVistoriaEscondido').attr('value', data.nofichavistoria);

                    // Recupera o motorista
                    EscolhendoMotorista = true;
                    var lstMotorista = $("#lstMotorista").data("kendoComboBox");
                    if (lstMotorista)
                    {
                        lstMotorista.value(data.motoristaid.toString());
                    }
                } else
                {
                    $('#txtNoFichaVistoria').attr('value', '');
                    $('#txtNoFichaVistoriaEscondido').attr('value', '');
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', data.message);
                    }
                }
            } catch (error)
            {
                if (window.Alerta?.TratamentoErroComLinha)
                {
                    Alerta.TratamentoErroComLinha("multa.js", "ajax.success", error);
                }
            }
        },
        error: function (err)
        {
            console.log(err);
            alert('Algo deu errado');
        }
    });
});

// Procura Ficha de Vistoria da Viagem
$(document).on('click', '.btnFicha', function ()
{
    var dataToPost = JSON.stringify({ 'NoFichaVistoria': document.getElementById("txtNoFichaVistoria").value });

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
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Verde', data.message);
                    }
                    $('#modalFicha').modal('show');
                } else
                {
                    if (window.AppToast?.show)
                    {
                        AppToast.show('Vermelho', data.message);
                    }
                }
            } catch (error)
            {
                if (window.Alerta?.TratamentoErroComLinha)
                {
                    Alerta.TratamentoErroComLinha("multa.js", "ajax.success", error);
                }
            }
        },
        error: function (err)
        {
            console.log(err);
            alert('Algo deu errado');
        }
    });
});

// Modal de Ficha de Vistoria
$("#modalFicha").modal({
    keyboard: true,
    backdrop: "static",
    show: false
}).on("show.bs.modal", function ()
{
    try
    {
        var id = ViagemId;
        var label = document.getElementById("DynamicModalLabelFicha");
        label.innerHTML = "";

        $.ajax({
            type: "get",
            url: "/api/Viagem/PegaFichaModal",
            data: { id: id },
            async: false,
            success: function (res)
            {
                var fichavistoria = document.getElementById("txtNoFichaVistoria").value;
                $('#imgViewer').removeAttr("src");

                if (res === false)
                {
                    label.innerHTML = "Infração sem Autuação digitalizada";
                    $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
                } else
                {
                    label.innerHTML = "Ficha de Vistoria Nº: <b>" + fichavistoria + "</b>";
                    $('#imgViewer').attr('src', "data:image/jpg;base64," + res);
                }
            }
        });
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "on show.bs.modal", error);
        }
    }
}).on("hide.bs.modal", function ()
{
    try
    {
        $('#imgViewer').removeAttr("src");
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "on hide.bs.modal", error);
        }
    }
});

// Validação de número de infração
$("#txtNumInfracao").focusout(function ()
{
    if (document.getElementById("txtNumInfracao").value === '') return;

    var NumInfracao = document.getElementById("txtNumInfracao").value;

    $.ajax({
        url: "/api/Multa/MultaExistente",
        method: "GET",
        datatype: "json",
        data: { numinfracao: NumInfracao },
        success: function (res)
        {
            try
            {
                var ExisteFicha = [res.data];
                if (ExisteFicha[0] === true)
                {
                    swal({
                        title: "Alerta no Número da Infração",
                        text: "Já existe uma Multa inserida com esta numeração!",
                        icon: "warning",
                        buttons: { ok: "Ok" }
                    });
                }
            } catch (error)
            {
                if (window.Alerta?.TratamentoErroComLinha)
                {
                    Alerta.TratamentoErroComLinha("multa.js", "ajax.success", error);
                }
            }
        }
    });
});

// Modal de Comprovante
$(document).on('click', '.btnComprovante', function ()
{
    $('#modalComprovante').modal('show');
});

$("#modalComprovante").modal({
    keyboard: true,
    show: false
}).on("show.bs.modal", function ()
{
    try
    {
        var comprovantePath = document.getElementById("txtComprovantePDF").value;
        if (comprovantePath && comprovantePath != '')
        {
            loadPdfInViewer(comprovantePath);
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "on show.bs.modal", error);
        }
    }
}).on("hide.bs.modal", function ()
{
    try
    {
        var uploaderElement = document.getElementById("flComprovante");
        if (uploaderElement?.ej2_instances?.[0])
        {
            uploaderElement.ej2_instances[0].clearAll();
        }
        $("div").removeClass("modal-backdrop");
        $('body').removeClass('modal-open');
        $("body").css("overflow", "auto");
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "on hide.bs.modal", error);
        }
    }
});

// Modal de Notificação de Autuação
$(document).on('click', '.btnNotificacao', function ()
{
    $('#modalAutuacao').modal('show');
});

// Toolbar do Rich Text Editor
function toolbarClick(e)
{
    try
    {
        if (e.item.id == "rte_toolbar_Image")
        {
            var element = document.getElementById('rte_upload');
            if (element?.ej2_instances?.[0])
            {
                element.ej2_instances[0].uploading = function upload(args)
                {
                    args.currentRequest.setRequestHeader('XSRF-TOKEN',
                        document.getElementsByName('__RequestVerificationToken')[0].value);
                };
            }
        }
    } catch (error)
    {
        if (window.Alerta?.TratamentoErroComLinha)
        {
            Alerta.TratamentoErroComLinha("multa.js", "toolbarClick", error);
        }
    }
}

// Localização do Rich Text Editor para PT-BR
ej.base.L10n.load({
    "pt-BR": {
        "richtexteditor": {
            "alignments": "Alinhamentos",
            "justifyLeft": "Alinhar à Esquerda",
            "justifyCenter": "Centralizar",
            "justifyRight": "Alinhar à Direita",
            "justifyFull": "Justificar",
            "fontName": "Nome da Fonte",
            "fontSize": "Tamanho da Fonte",
            "fontColor": "Cor da Fonte",
            "backgroundColor": "Cor de Fundo",
            "bold": "Negrito",
            "italic": "Itálico",
            "underline": "Sublinhado",
            "strikethrough": "Tachado",
            "clearFormat": "Limpar Formatação",
            "clearAll": "Limpar Tudo",
            "cut": "Cortar",
            "copy": "Copiar",
            "paste": "Colar",
            "unorderedList": "Lista com Marcadores",
            "orderedList": "Lista Numerada",
            "indent": "Aumentar Indentação",
            "outdent": "Diminuir Indentação",
            "undo": "Desfazer",
            "redo": "Refazer",
            "superscript": "Sobrescrito",
            "subscript": "Subscrito",
            "createLink": "Inserir Link",
            "openLink": "Abrir Link",
            "editLink": "Editar Link",
            "removeLink": "Remover Link",
            "image": "Inserir Imagem",
            "replace": "Substituir",
            "align": "Alinhar",
            "caption": "Título da Imagem",
            "remove": "Remover",
            "insertLink": "Inserir Link",
            "display": "Exibir",
            "altText": "Texto Alternativo",
            "dimension": "Mudar Tamanho",
            "fullscreen": "Maximizar",
            "maximize": "Maximizar",
            "minimize": "Minimizar",
            "print": "Imprimir",
            "formats": "Formatos",
            "sourcecode": "Visualizar Código",
            "preview": "Exibir"
        }
    }
});
