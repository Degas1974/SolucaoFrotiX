/**
 * ═══════════════════════════════════════════════════════════════════════════
 * MOTORISTAS ITENS CONTRATO - MÓDULO DE LISTAGEM
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Gerencia a listagem de motoristas vinculados a contratos.
 *              Exibe informações como nome, ponto, CNH, categoria, celular,
 *              unidade, contrato e status. Permite alteração de status e exclusão.
 * @file motoristasitenscontrato_001.js
 * @requires jQuery, DataTables, Alerta (biblioteca interna)
 * @see /Views/Motorista/Index.cshtml
 * ═══════════════════════════════════════════════════════════════════════════
 */

/** @type {DataTables.Api} Instância global do DataTable para motoristas */
var dataTableMotorista;

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * INICIALIZAÇÃO DO DOCUMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Carrega a lista de motoristas quando o DOM está pronto
 */
$(document).ready(function () {
    try {
        // Inicializa a grid de motoristas
        loadMotoristaList();
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'motoristasitenscontrato_001.js',
            'document.ready',
            error,
        );
    }
});

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * CARREGAMENTO DA GRID DE MOTORISTAS
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Inicializa DataTable com dados de motoristas via AJAX.
 *              Exibe: Nome, Ponto, CNH, Categoria, Celular, Unidade, Contrato, Status, Ações.
 * @returns {void}
 */
function loadMotoristaList() {
    try {
        // Inicializa DataTable na tabela #tblMotorista
        dataTableMotorista = $('#tblMotorista').DataTable({
            // Configurações de colunas (larguras e alinhamentos)
            columnDefs: [
                {
                    targets: 0, // Coluna Nome do motorista
                    className: 'text-left',
                    width: '15%',
                },
                {
                    targets: 1, // Coluna Ponto (matrícula)
                    className: 'text-center',
                    width: '6%',
                },
                {
                    targets: 2, // Coluna Número da CNH
                    className: 'text-center',
                    width: '6%',
                },
                {
                    targets: 3, // Coluna Categoria CNH (A, B, C, D, E)
                    className: 'text-center',
                    width: '5%',
                    defaultContent: '', // Evita erro se valor for nulo
                },
                {
                    targets: 4, // Coluna Celular
                    className: 'text-center',
                    width: '8%',
                },
                {
                    targets: 5, // Coluna Sigla da Unidade
                    className: 'text-left',
                    width: '5%',
                },
                {
                    targets: 6, // Coluna Nome do Contrato
                    className: 'text-left',
                    width: '10%',
                },
                {
                    targets: 7, // Coluna Status (Ativo/Inativo)
                    className: 'text-center',
                    width: '5%',
                },
                {
                    targets: 8, // Coluna Ações
                    className: 'text-center',
                    width: '8%',
                },
            ],

            responsive: true,

            // Fonte de dados via AJAX
            ajax: {
                url: '/api/motorista',
                type: 'GET',
                datatype: 'json',
            },

            // Mapeamento de colunas
            columns: [
                // Coluna 0: Nome completo do motorista
                { data: 'nome' },

                // Coluna 1: Número do ponto/matrícula
                { data: 'ponto' },

                // Coluna 2: Número da CNH
                { data: 'cnh' },

                // Coluna 3: Categoria da CNH
                { data: 'categoriaCNH' },

                // Coluna 4: Número do celular
                { data: 'celular01' },

                // Coluna 5: Sigla da unidade
                { data: 'sigla' },

                // Coluna 6: Nome do contrato vinculado
                { data: 'contratoMotorista' },

                // Coluna 7: Status - renderizado como botão toggle
                {
                    data: 'status',

                    /**
                     * @description Renderiza botão de status com cores condicionais
                     * @param {boolean} data - Status atual do motorista
                     * @param {string} type - Tipo de renderização
                     * @param {Object} row - Dados completos da linha
                     * @returns {string} HTML do botão de status
                     */
                    render: function (data, type, row, meta) {
                        try {
                            if (data) {
                                // Status ativo = botão verde
                                return (
                                    '<a href="javascript:void" class="updateStatusMotorista btn btn-verde btn-xs text-white" data-url="/api/Motorista/updateStatusMotorista?Id=' +
                                    row.motoristaId +
                                    '">Ativo</a>'
                                );
                            } else {
                                // Status inativo = botão cinza
                                return (
                                    '<a href="javascript:void" class="updateStatusMotorista btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Motorista/updateStatusMotorista?Id=' +
                                    row.motoristaId +
                                    '">Inativo</a>'
                                );
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'motoristasitenscontrato_001.js',
                                'loadList.render.status',
                                error,
                            );
                        }
                    },
                },

                // Coluna 8: Botão de exclusão
                {
                    data: 'motoristaId',

                    /**
                     * @description Renderiza botão de exclusão com tooltip
                     * @param {number} data - ID do motorista
                     * @returns {string} HTML do botão de exclusão
                     */
                    render: function (data) {
                        try {
                            return `<div class="text-center">
                                <a class="btn-delete btn btn-vinho btn-xs text-white" aria-label="Excluir o Motorista do Contrato!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                    <i class="far fa-trash-alt"></i>
                                </a>
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'motoristasitenscontrato_001.js',
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
        Alerta.TratamentoErroComLinha(
            'motoristasitenscontrato_001.js',
            'loadMotoristaList',
            error,
        );
    }
}
