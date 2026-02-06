/* ****************************************************************************************
 * ⚡ ARQUIVO: multas-upload-handler.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciador centralizado para múltiplos uploaders de PDF de multas.
 *                   Fornece API modular (MultasUpload) para carregar PDFs em viewers,
 *                   obter instâncias de viewers Syncfusion, e funções auxiliares.
 * 📥 ENTRADAS     : Chamadas de funções (getViewer(viewerId), loadPdfInViewer(fileName, viewerId)),
 *                   fileName (string do arquivo PDF), viewerId (ID do viewer Syncfusion)
 * 📤 SAÍDAS       : PDF carregado em Syncfusion PDF Viewer, instância de viewer retornada
 *                   (ej2_instances[0]), console.warn/error (debug), Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : Módulos externos (páginas de multas), funções internas (uploadSuccess callbacks),
 *                   MultasUpload.loadPdfInViewer(), MultasUpload.getViewer()
 * 🔄 CHAMA        : document.getElementById, ej2_instances[0] (Syncfusion API),
 *                   console.warn/error, window.Alerta?.TratamentoErroComLinha,
 *                   viewer.load() (Syncfusion PDF Viewer)
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (PDF Viewer), Alerta.js (opcional - verifica existência)
 * 📝 OBSERVAÇÕES  : Módulo IIFE (Immediately Invoked Function Expression) com encapsulamento
 *                   'use strict'. Funções privadas (getViewer, loadPdfInViewer) acessíveis
 *                   via objeto retornado MultasUpload. 337 linhas total.
 **************************************************************************************** */

// ====================================================================
// MULTAS UPLOAD HANDLER - MODULE PATTERN
// Gerenciador centralizado para múltiplos uploaders de PDF
// Encapsula funções privadas e expõe apenas API pública
// ====================================================================

var MultasUpload = (function () {
    'use strict';

    // ====================================================================
    // FUNÇÕES PRIVADAS - HELPERS
    // Funções auxiliares usadas internamente pelo módulo
    // ====================================================================

    /**
     * @function getViewer
     * @description Obtém a instância Syncfusion do PDF Viewer pelo ID do elemento.
     * @param {string} viewerId - ID do elemento DOM do viewer
     * @returns {Object|null} Instância do viewer ou null se não encontrado
     * @private
     */
    function getViewer(viewerId) {
        try {
            // Obtém o primeiro instance EJ2 do elemento
            return (
                document.getElementById(viewerId)?.ej2_instances?.[0] || null
            );
        } catch (err) {
            if (window.Alerta?.TratamentoErroComLinha) {
                Alerta.TratamentoErroComLinha(
                    'multas-upload-handler.js',
                    'getViewer',
                    err,
                );
            }
            return null;
        }
    }

    /**
     * @function loadPdfInViewer
     * @description Carrega um arquivo PDF em um viewer Syncfusion específico.
     * @param {string} fileName - Nome/caminho do arquivo PDF
     * @param {string} viewerId - ID do elemento DOM do viewer
     * @returns {void}
     * @private
     */
    function loadPdfInViewer(fileName, viewerId) {
        try {
            // Valida o nome do arquivo
            if (!fileName || fileName === '' || fileName === 'null') {
                console.warn(
                    'Nome de arquivo inválido para carregar no viewer',
                );
                return;
            }

            // Obtém a instância do viewer
            const viewer = getViewer(viewerId);
            if (!viewer) {
                console.error('Viewer não encontrado:', viewerId);
                return;
            }

            // Configura e carrega o documento
            viewer.documentPath = fileName;
            viewer.dataBind();
            viewer.load(fileName, null);
        } catch (error) {
            if (window.Alerta?.TratamentoErroComLinha) {
                Alerta.TratamentoErroComLinha(
                    'multas-upload-handler.js',
                    'loadPdfInViewer',
                    error,
                );
            }
            console.error(error);
        }
    }

    /**
     * @function extractPayload
     * @description Extrai o payload JSON da resposta do servidor após upload.
     *              Trata diferentes formatos de resposta possíveis do Syncfusion Uploader.
     * @param {Object} args - Argumentos do evento de sucesso do uploader
     * @returns {Object|null} Objeto parseado da resposta ou null em caso de erro
     * @private
     */
    function extractPayload(args) {
        try {
            // Formato 1: args.response.response (mais comum)
            if (args?.response?.response) {
                return JSON.parse(args.response.response);
            }
            // Formato 2: args.e.target.response (XHR direto)
            else if (args?.e?.target?.response) {
                return JSON.parse(args.e.target.response);
            }
            // Formato 3: args.response como string
            else if (typeof args?.response === 'string') {
                return JSON.parse(args.response);
            }
            return null;
        } catch (error) {
            if (window.Alerta?.TratamentoErroComLinha) {
                Alerta.TratamentoErroComLinha(
                    'multas-upload-handler.js',
                    'extractPayload',
                    error,
                );
            }
            return null;
        }
    }

    // ====================================================================
    // CALLBACKS GENÉRICOS
    // Handlers reutilizáveis para eventos de upload
    // ====================================================================

    /**
     * @function onUploadSelected
     * @description Callback de validação quando um arquivo é selecionado.
     *              Aceita apenas arquivos com extensão .pdf
     * @param {Object} args - Argumentos do evento selected do Syncfusion Uploader
     * @returns {void}
     */
    function onUploadSelected(args) {
        try {
            // Verifica se há arquivos selecionados
            if (!args || !args.filesData || args.filesData.length === 0) return;

            // Obtém informações do primeiro arquivo
            const file = args.filesData[0];
            const fileName = (file?.name || '').toLowerCase();

            // Valida se é PDF - cancela upload se não for
            if (!fileName.endsWith('.pdf')) {
                // Cancela o upload
                args.cancel = true;

                // Exibe mensagem de erro ao usuário
                if (window.AppToast?.show) {
                    AppToast.show(
                        'Vermelho',
                        'Apenas arquivos PDF são permitidos',
                        3000,
                    );
                } else {
                    alert('Apenas arquivos PDF são permitidos.');
                }
            }
        } catch (error) {
            if (window.Alerta?.TratamentoErroComLinha) {
                Alerta.TratamentoErroComLinha(
                    'multas-upload-handler.js',
                    'onUploadSelected',
                    error,
                );
            }
        }
    }

    /**
     * @function onUploadFailure
     * @description Callback genérico de falha no upload.
     *              Exibe mensagem de erro com detalhes do servidor.
     * @param {Object} args - Argumentos do evento failure do Syncfusion Uploader
     * @returns {void}
     */
    function onUploadFailure(args) {
        try {
            // Monta mensagem de erro
            let msg = 'Falha no upload do PDF';

            // Adiciona detalhes do servidor se disponível
            if (args?.response?.responseText) {
                msg += ': ' + args.response.responseText;
            }

            if (window.AppToast?.show) {
                AppToast.show('Vermelho', msg, 4000);
            } else {
                alert(msg);
            }
        } catch (error) {
            if (window.Alerta?.TratamentoErroComLinha) {
                Alerta.TratamentoErroComLinha(
                    'multas-upload-handler.js',
                    'onUploadFailure',
                    error,
                );
            }
        }
    }

    /**
     * @function createSuccessHandler
     * @description Factory function que cria handlers de sucesso específicos para cada tipo de upload.
     *              Retorna uma closure que atualiza o campo hidden e carrega o PDF no viewer.
     * @param {string} inputId - Seletor jQuery do input hidden que armazena o nome do arquivo
     * @param {string} viewerId - ID do elemento DOM do PDF Viewer
     * @param {string} successMessage - Mensagem a exibir no toast de sucesso
     * @returns {Function} Handler de sucesso configurado
     * @private
     */
    function createSuccessHandler(inputId, viewerId, successMessage) {
        return function (args) {
            try {
                // Extrai o payload da resposta do servidor
                const payload = extractPayload(args);

                // Valida se veio o nome do arquivo
                if (!payload || !payload.fileName) {
                    console.warn(
                        'Upload OK, mas não veio fileName na resposta',
                    );

                    if (window.AppToast?.show) {
                        AppToast.show(
                            'Amarelo',
                            'Arquivo enviado, mas houve problema ao processar',
                            3000,
                        );
                    }
                    return;
                }

                // Atualiza o campo hidden com o nome do arquivo
                // Este valor será enviado no submit do formulário
                $(inputId).val(payload.fileName);

                // Carrega o PDF no viewer para preview
                loadPdfInViewer(payload.fileName, viewerId);

                // Exibe toast de sucesso
                if (window.AppToast?.show) {
                    AppToast.show('Verde', successMessage, 3000);
                }
            } catch (error) {
                if (window.Alerta?.TratamentoErroComLinha) {
                    Alerta.TratamentoErroComLinha(
                        'multas-upload-handler.js',
                        'successHandler',
                        error,
                    );
                }
                console.error(error);
            }
        };
    }

    // ====================================================================
    // CALLBACKS ESPECÍFICOS PARA CADA TIPO DE UPLOAD
    // Cada handler é criado via factory com configurações específicas
    // ====================================================================

    // Handler para upload de PDF de Autuação (notificação inicial da multa)
    var onUploadSuccess_Autuacao = createSuccessHandler(
        '#txtAutuacaoPDF',
        'pdfviewerAutuacao',
        'PDF de Autuação enviado com sucesso!',
    );

    // Handler para upload de PDF de Penalidade (multa efetivada)
    var onUploadSuccess_Penalidade = createSuccessHandler(
        '#txtPenalidadePDF',
        'pdfviewerPenalidade',
        'PDF de Penalidade enviado com sucesso!',
    );

    // Handler para upload de Comprovante de Pagamento
    var onUploadSuccess_Comprovante = createSuccessHandler(
        '#txtComprovantePDF',
        'pdfviewerComprovante',
        'Comprovante enviado com sucesso!',
    );

    // Handler para upload de Processo e-Doc
    var onUploadSuccess_EDoc = createSuccessHandler(
        '#txtEDocPDF',
        'pdfviewerEDoc',
        'Processo e-Doc enviado com sucesso!',
    );

    // Handler para upload de Outros Documentos
    var onUploadSuccess_OutrosDocumentos = createSuccessHandler(
        '#txtOutrosDocumentosPDF',
        'pdfviewerOutrosDocumentos',
        'Documento enviado com sucesso!',
    );

    // ====================================================================
    // API PÚBLICA DO MÓDULO
    // Expõe apenas as funções necessárias para uso externo
    // ====================================================================

    return {
        // Callbacks genéricos (usados em todos os uploaders)
        onUploadSelected: onUploadSelected,
        onUploadFailure: onUploadFailure,

        // Callbacks específicos de sucesso (um para cada tipo de documento)
        onUploadSuccess_Autuacao: onUploadSuccess_Autuacao,
        onUploadSuccess_Penalidade: onUploadSuccess_Penalidade,
        onUploadSuccess_Comprovante: onUploadSuccess_Comprovante,
        onUploadSuccess_EDoc: onUploadSuccess_EDoc,
        onUploadSuccess_OutrosDocumentos: onUploadSuccess_OutrosDocumentos,

        // Utilitários expostos para uso em outras partes do código
        loadPdfInViewer: loadPdfInViewer,
        getViewer: getViewer,
    };
})();

// ====================================================================
// INICIALIZAÇÃO NA CARGA DA PÁGINA
// Carrega PDFs existentes quando a página é aberta em modo edição
// ====================================================================

$(document).ready(function () {
    try {
        console.log('MultasUpload Handler inicializado com sucesso!');

        // Delay para garantir que os viewers Syncfusion estejam prontos
        // Em modo edição, carrega os PDFs já associados ao registro
        setTimeout(function () {
            try {
                // Carrega PDF de Autuação se existir
                const autuacaoPDF = $('#txtAutuacaoPDF').val();
                if (
                    autuacaoPDF &&
                    autuacaoPDF !== '' &&
                    autuacaoPDF !== 'null'
                ) {
                    MultasUpload.loadPdfInViewer(
                        autuacaoPDF,
                        'pdfviewerAutuacao',
                    );
                }

                // Carrega PDF de Penalidade se existir
                const penalidadePDF = $('#txtPenalidadePDF').val();
                if (
                    penalidadePDF &&
                    penalidadePDF !== '' &&
                    penalidadePDF !== 'null'
                ) {
                    MultasUpload.loadPdfInViewer(
                        penalidadePDF,
                        'pdfviewerPenalidade',
                    );
                }

                // Carrega PDF de Comprovante se existir
                const comprovantePDF = $('#txtComprovantePDF').val();
                if (
                    comprovantePDF &&
                    comprovantePDF !== '' &&
                    comprovantePDF !== 'null'
                ) {
                    MultasUpload.loadPdfInViewer(
                        comprovantePDF,
                        'pdfviewerComprovante',
                    );
                }

                // Carrega PDF de E-Doc se existir
                const edocPDF = $('#txtEDocPDF').val();
                if (edocPDF && edocPDF !== '' && edocPDF !== 'null') {
                    MultasUpload.loadPdfInViewer(edocPDF, 'pdfviewerEDoc');
                }

                // Carrega PDF de Outros Documentos se existir
                const outrosPDF = $('#txtOutrosDocumentosPDF').val();
                if (outrosPDF && outrosPDF !== '' && outrosPDF !== 'null') {
                    MultasUpload.loadPdfInViewer(
                        outrosPDF,
                        'pdfviewerOutrosDocumentos',
                    );
                }
            } catch (error) {
                if (window.Alerta?.TratamentoErroComLinha) {
                    Alerta.TratamentoErroComLinha(
                        'multas-upload-handler.js',
                        'carregarPDFsExistentes',
                        error,
                    );
                }
            }
        }, 1500); // 1.5s de delay para componentes Syncfusion inicializarem
    } catch (error) {
        if (window.Alerta?.TratamentoErroComLinha) {
            Alerta.TratamentoErroComLinha(
                'multas-upload-handler.js',
                'document.ready',
                error,
            );
        }
    }
});
