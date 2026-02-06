/**
 * ═══════════════════════════════════════════════════════════════════════════
 * CONDUTOR DE APOIO - MÓDULO DE LISTAGEM E GERENCIAMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Gerencia a listagem de condutores de apoio com DataTables.
 *              Permite exclusão, alteração de status e edição de registros.
 * @file condutorapoio_001.js
 * @requires jQuery, DataTables, Alerta (biblioteca interna), AppToast
 * @see /Views/CondutorApoio/Index.cshtml
 * ═══════════════════════════════════════════════════════════════════════════
 */

/** @type {DataTables.Api} Instância global do DataTable para condutores */
var dataTable;

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * INICIALIZAÇÃO DO DOCUMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Configura handlers de eventos quando o DOM está pronto
 */
$(document).ready(function () {
    try {
        // Carrega a grid de condutores ao iniciar a página
        loadList();

        /**
         * ─────────────────────────────────────────────────────────────────
         * HANDLER: EXCLUSÃO DE CONDUTOR
         * ─────────────────────────────────────────────────────────────────
         * @description Intercepta clique no botão excluir e exibe confirmação
         * @listens click.btn-delete
         */
        $(document).on('click', '.btn-delete', function () {
            try {
                /** @type {number} ID do condutor a ser excluído */
                var id = $(this).data('id');

                // Exibe diálogo de confirmação usando biblioteca Alerta
                Alerta.Confirmar(
                    'Você tem certeza que deseja apagar este condutor?',
                    'Não será possível recuperar os dados eliminados!',
                    'Excluir',
                    'Cancelar',
                ).then((willDelete) => {
                    try {
                        // Somente procede se usuário confirmou
                        if (willDelete) {
                            /** @type {string} JSON com ID do condutor para API */
                            var dataToPost = JSON.stringify({ CondutorId: id });

                            /** @type {string} Endpoint de exclusão */
                            var url = '/api/CondutorApoio/Delete';

                            // Requisição AJAX para excluir o condutor
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
                                            'condutorapoio_001.js',
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
                                            'Ocorreu um erro ao excluir o condutor. Tente novamente.',
                                        );
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'condutorapoio_001.js',
                                            'btn-delete.ajax.error',
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'condutorapoio_001.js',
                            'btn-delete.confirm.then',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'condutorapoio_001.js',
                    'btn-delete.click',
                    error,
                );
            }
        });

        /**
         * ─────────────────────────────────────────────────────────────────
         * HANDLER: ALTERAÇÃO DE STATUS DO CONDUTOR
         * ─────────────────────────────────────────────────────────────────
         * @description Alterna status Ativo/Inativo via API e atualiza UI
         * @listens click.updateStatusCondutor
         */
        $(document).on('click', '.updateStatusCondutor', function () {
            try {
                /** @type {string} URL da API com ID do condutor */
                var url = $(this).data('url');

                /** @type {jQuery} Referência ao botão clicado para atualizar UI */
                var currentElement = $(this);

                // Chamada GET para alternar status
                $.get(url, function (data) {
                    try {
                        if (data.success) {
                            // Exibe toast de sucesso
                            AppToast.show(
                                'Verde',
                                'Status alterado com sucesso!',
                            );

                            /** @type {string} Texto do novo status */
                            var text = 'Ativo';

                            // Atualiza aparência do botão baseado no novo status
                            // data.type == 1 significa que ficou Inativo
                            if (data.type == 1) {
                                text = 'Inativo';
                                // Remove classe verde, adiciona cinza
                                currentElement
                                    .removeClass('btn-verde')
                                    .addClass('fundo-cinza');
                            } else {
                                // Remove classe cinza, adiciona verde
                                currentElement
                                    .removeClass('fundo-cinza')
                                    .addClass('btn-verde');
                            }

                            // Atualiza texto do botão
                            currentElement.text(text);
                        } else {
                            Alerta.Erro(
                                'Erro',
                                'Não foi possível alterar o status. Tente novamente.',
                            );
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'condutorapoio_001.js',
                            'updateStatus.get.callback',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'condutorapoio_001.js',
                    'updateStatus.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'condutorapoio_001.js',
            'document.ready',
            error,
        );
    }
});

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * CARREGAMENTO DA GRID DE CONDUTORES
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Inicializa DataTable com dados de condutores via AJAX
 * @returns {void}
 */
function loadList() {
    try {
        // Inicializa DataTable na tabela #tblCondutor
        dataTable = $('#tblCondutor').DataTable({
            // Configurações de colunas
            columnDefs: [
                {
                    targets: 1, // Coluna de Status
                    className: 'text-center',
                    width: '20%',
                },
                {
                    targets: 2, // Coluna de Ações
                    className: 'text-center',
                    width: '20%',
                },
            ],

            responsive: true,

            // Fonte de dados via AJAX
            ajax: {
                url: '/api/condutorapoio',
                type: 'GET',
                datatype: 'json',
            },

            // Mapeamento de colunas
            columns: [
                // Coluna 0: Descrição do condutor
                { data: 'descricao', width: '30%' },

                // Coluna 1: Status (Ativo/Inativo) - renderizado como botão
                {
                    data: 'status',

                    /**
                     * @description Renderiza botão de status com cores condicionais
                     * @param {boolean} data - Status atual do condutor
                     * @param {string} type - Tipo de renderização
                     * @param {Object} row - Dados completos da linha
                     * @returns {string} HTML do botão de status
                     */
                    render: function (data, type, row, meta) {
                        try {
                            if (data) {
                                // Status ativo = botão verde
                                return (
                                    '<a href="javascript:void" class="updateStatusCondutor btn btn-verde btn-xs text-white" data-url="/api/CondutorApoio/UpdateStatusCondutor?Id=' +
                                    row.condutorId +
                                    '">Ativo</a>'
                                );
                            } else {
                                // Status inativo = botão cinza
                                return (
                                    '<a href="javascript:void" class="updateStatusCondutor btn btn-xs fundo-cinza text-white text-bold" data-url="/api/CondutorApoio/UpdateStatusCondutor?Id=' +
                                    row.condutorId +
                                    '">Inativo</a>'
                                );
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'condutorapoio_001.js',
                                'loadList.render.status',
                                error,
                            );
                        }
                    },
                    width: '10%',
                },

                // Coluna 2: Ações (Editar/Excluir)
                {
                    data: 'condutorId',

                    /**
                     * @description Renderiza botões de ação (Editar/Excluir)
                     * @param {number} data - ID do condutor
                     * @returns {string} HTML dos botões de ação
                     */
                    render: function (data) {
                        try {
                            return `<div class="text-center">
                                <a href="/CondutorApoio/Upsert?id=${data}" class="btn btn-azul btn-xs text-white" style="cursor:pointer; width:75px;">
                                    <i class="far fa-edit"></i> Editar
                                </a>
                                <a class="btn-delete btn btn-vinho btn-xs text-white" style="cursor:pointer; width:80px;" data-id='${data}'>
                                    <i class="far fa-trash-alt"></i> Excluir
                                </a>
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'condutorapoio_001.js',
                                'loadList.render.actions',
                                error,
                            );
                        }
                    },
                    width: '20%',
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
        Alerta.TratamentoErroComLinha(
            'condutorapoio_001.js',
            'loadList',
            error,
        );
    }
}
