/**
 * ═══════════════════════════════════════════════════════════════════════════
 * TIPO DE MULTA - MÓDULO DE LISTAGEM E GERENCIAMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Gerencia a listagem de tipos de multa (infrações de trânsito)
 *              com DataTables. Exibe artigo, código Denatran, descrição e
 *              gravidade da infração. Permite edição e exclusão de registros.
 * @file tipomulta_001.js
 * @requires jQuery, DataTables, Alerta (biblioteca interna), AppToast
 * @see /Views/Multa/TipoMulta.cshtml
 * ═══════════════════════════════════════════════════════════════════════════
 */

/** @type {DataTables.Api} Instância global do DataTable para tipos de multa */
var dataTable;

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * INICIALIZAÇÃO DO DOCUMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Configura handlers de eventos quando o DOM está pronto
 */
$(document).ready(function () {
    try {
        // Carrega a grid de tipos de multa ao iniciar
        loadList();

        /**
         * ─────────────────────────────────────────────────────────────────
         * HANDLER: EXCLUSÃO DE TIPO DE MULTA
         * ─────────────────────────────────────────────────────────────────
         * @description Intercepta clique no botão excluir e exibe confirmação
         * @listens click.btn-delete
         */
        $(document).on('click', '.btn-delete', function () {
            try {
                /** @type {number} ID do tipo de multa a ser excluído */
                var id = $(this).data('id');

                // Exibe diálogo de confirmação usando biblioteca Alerta
                Alerta.Confirmar(
                    'Você tem certeza que deseja apagar este tipo de multa?',
                    'Não será possível recuperar os dados eliminados!',
                    'Excluir',
                    'Cancelar',
                ).then((willDelete) => {
                    try {
                        // Somente procede se usuário confirmou
                        if (willDelete) {
                            /** @type {string} JSON com ID do tipo de multa para API */
                            var dataToPost = JSON.stringify({
                                TipoMultaId: id,
                            });

                            /** @type {string} Endpoint de exclusão de tipo de multa */
                            var url = '/api/Multa/DeleteTipoMulta';

                            // Requisição AJAX para excluir o tipo de multa
                            $.ajax({
                                url: url,
                                type: 'POST',
                                data: dataToPost,
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',

                                /**
                                 * @description Callback de sucesso - exibe toast e recarrega grid
                                 * @param {Object} data - Resposta da API
                                 * @param {boolean} data.success - Indica se operação foi bem-sucedida
                                 * @param {string} data.message - Mensagem para exibir ao usuário
                                 */
                                success: function (data) {
                                    try {
                                        if (data.success) {
                                            // Toast verde = sucesso
                                            AppToast.show(
                                                'Verde',
                                                data.message,
                                            );
                                            // Recarrega tabela para refletir exclusão
                                            dataTable.ajax.reload();
                                        } else {
                                            // Toast vermelho = erro
                                            AppToast.show(
                                                'Vermelho',
                                                data.message,
                                            );
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'tipomulta_001.js',
                                            'btn-delete.ajax.success',
                                            error,
                                        );
                                    }
                                },

                                /**
                                 * @description Callback de erro - exibe alerta de falha
                                 * @param {jqXHR} err - Objeto de erro jQuery AJAX
                                 */
                                error: function (err) {
                                    try {
                                        console.log(err);
                                        Alerta.Erro(
                                            'Erro',
                                            'Ocorreu um erro ao excluir o tipo de multa. Tente novamente.',
                                        );
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'tipomulta_001.js',
                                            'btn-delete.ajax.error',
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'tipomulta_001.js',
                            'btn-delete.confirm.then',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'tipomulta_001.js',
                    'btn-delete.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'tipomulta_001.js',
            'document.ready',
            error,
        );
    }
});

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * CARREGAMENTO DA GRID DE TIPOS DE MULTA
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Inicializa DataTable com dados de tipos de multa via AJAX.
 *              Exibe: Artigo, Código Denatran, Descrição, Gravidade e Ações.
 * @returns {void}
 */
function loadList() {
    try {
        // Inicializa DataTable na tabela #tblTipoMulta
        dataTable = $('#tblTipoMulta').DataTable({
            // Configurações de colunas
            columnDefs: [
                {
                    targets: 0, // Coluna Artigo (código do CTB)
                    className: 'text-left',
                    width: '20%',
                },
                {
                    targets: 1, // Coluna Código Denatran
                    className: 'text-left',
                    width: '20%',
                },
                {
                    targets: 2, // Coluna Descrição da infração
                    className: 'text-left',
                    width: '64%',
                },
                {
                    targets: 3, // Coluna Gravidade (Leve/Média/Grave/Gravíssima)
                    className: 'text-center',
                    width: '8%',
                },
                {
                    targets: 4, // Coluna Ações (Editar/Excluir)
                    className: 'text-center',
                    width: '8%',
                },
            ],

            responsive: true,

            // Fonte de dados via AJAX
            ajax: {
                url: '/api/Multa/PegaTipoMulta',
                type: 'GET',
                datatype: 'json',
            },

            // Mapeamento de colunas
            columns: [
                // Coluna 0: Artigo do CTB
                { data: 'artigo' },

                // Coluna 1: Código Denatran
                { data: 'denatran' },

                // Coluna 2: Descrição da infração
                { data: 'descricao' },

                // Coluna 3: Gravidade da infração
                { data: 'infracao' },

                // Coluna 4: Ações (Editar/Excluir)
                {
                    data: 'tipoMultaId',

                    /**
                     * @description Renderiza botões de ação (Editar/Excluir)
                     * @param {number} data - ID do tipo de multa
                     * @returns {string} HTML dos botões de ação
                     */
                    render: function (data) {
                        try {
                            return `<div class="text-center">
                                <a href="/Multa/UpsertTipoMulta?id=${data}" class="btn btn-azul btn-xs text-white" style="cursor:pointer;">
                                    <i class="far fa-edit"></i>
                                </a>
                                <a class="btn-delete btn btn-vinho btn-xs text-white" style="cursor:pointer;" data-id='${data}'>
                                    <i class="far fa-trash-alt"></i>
                                </a>
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'tipomulta_001.js',
                                'loadList.render.actions',
                                error,
                            );
                        }
                    },
                },
            ],

            // Configuração de idioma português
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
                emptyTable: 'Sem Dados para Exibição',
            },
            width: '100%',
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('tipomulta_001.js', 'loadList', error);
    }
}
