/**
 * ═══════════════════════════════════════════════════════════════════════════
 * VEÍCULOS ITENS CONTRATO - MÓDULO DE LISTAGEM
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Gerencia a listagem de veículos vinculados a contratos.
 *              Exibe informações como placa, marca/modelo, contrato, sigla,
 *              combustível e status. Permite alteração de status e exclusão.
 * @file veiculositenscontrato_001.js
 * @requires jQuery, DataTables, Alerta (biblioteca interna)
 * @see /Views/Veiculo/Index.cshtml
 * ═══════════════════════════════════════════════════════════════════════════
 */

/** @type {DataTables.Api} Instância global do DataTable para veículos */
var dataTableVeiculo;

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * INICIALIZAÇÃO DO DOCUMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Carrega a lista de veículos quando o DOM está pronto
 */
$(document).ready(function () {
    try {
        // Inicializa a grid de veículos
        loadList();
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'veiculositenscontrato_001.js',
            'document.ready',
            error,
        );
    }
});

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * CARREGAMENTO DA GRID DE VEÍCULOS
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Inicializa DataTable com dados de veículos via AJAX.
 *              Exibe: Placa, Marca/Modelo, Contrato, Sigla, Combustível, Status, Ações.
 * @returns {void}
 */
function loadList() {
    try {
        // Inicializa DataTable na tabela #tblVeiculo
        dataTableVeiculo = $('#tblVeiculo').DataTable({
            // Configurações de colunas (larguras e alinhamentos)
            columnDefs: [
                {
                    targets: 0, // Coluna Placa do veículo
                    className: 'text-center',
                    width: '9%',
                },
                {
                    targets: 1, // Coluna Marca/Modelo do veículo
                    className: 'text-left',
                    width: '17%',
                },
                {
                    targets: 2, // Coluna Nome do Contrato
                    className: 'text-left',
                    width: '35%',
                },
                {
                    targets: 3, // Coluna Sigla da Unidade
                    className: 'text-center',
                    width: '5%',
                    defaultContent: '', // Evita erro se valor for nulo
                },
                {
                    targets: 4, // Coluna Tipo de Combustível
                    className: 'text-center',
                    width: '5%',
                },
                {
                    targets: 5, // Coluna Status (Ativo/Inativo)
                    className: 'text-center',
                    width: '7%',
                },
                {
                    targets: 6, // Coluna Ações (Editar/Excluir)
                    className: 'text-center',
                    width: '8%',
                },
            ],

            responsive: true,

            // Fonte de dados via AJAX
            ajax: {
                url: '/api/veiculo',
                type: 'GET',
                datatype: 'json',
            },

            // Mapeamento de colunas
            columns: [
                // Coluna 0: Placa do veículo (formato Mercosul ou antigo)
                { data: 'placa' },

                // Coluna 1: Marca e modelo do veículo
                { data: 'marcaModelo' },

                // Coluna 2: Nome do contrato vinculado
                { data: 'contratoVeiculo' },

                // Coluna 3: Sigla da unidade
                { data: 'sigla' },

                // Coluna 4: Descrição do tipo de combustível
                { data: 'combustivelDescricao' },

                // Coluna 5: Status - renderizado como botão toggle
                {
                    data: 'status',

                    /**
                     * @description Renderiza botão de status com cores condicionais
                     * @param {boolean} data - Status atual do veículo
                     * @param {string} type - Tipo de renderização
                     * @param {Object} row - Dados completos da linha
                     * @returns {string} HTML do botão de status
                     */
                    render: function (data, type, row, meta) {
                        try {
                            if (data) {
                                // Status ativo = botão verde
                                return (
                                    '<a href="javascript:void" class="updateStatusVeiculo btn btn-verde btn-xs text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
                                    row.veiculoId +
                                    '">Ativo</a>'
                                );
                            } else {
                                // Status inativo = botão cinza
                                return (
                                    '<a href="javascript:void" class="updateStatusVeiculo btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
                                    row.veiculoId +
                                    '">Inativo</a>'
                                );
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'veiculositenscontrato_001.js',
                                'loadList.render.status',
                                error,
                            );
                        }
                    },
                },

                // Coluna 6: Ações (Editar/Excluir)
                {
                    data: 'veiculoId',

                    /**
                     * @description Renderiza botões de ação com tooltips
                     * @param {number} data - ID do veículo
                     * @returns {string} HTML dos botões de ação
                     */
                    render: function (data) {
                        try {
                            return `<div class="text-center">
                                <a href="/Veiculo/Upsert?id=${data}" class="btn btn-azul btn-xs text-white" aria-label="Editar o Veículo!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
                                    <i class="far fa-edit"></i> 
                                </a>
                                <a class="btn-delete btn btn-vinho btn-xs text-white" aria-label="Excluir o Veículo!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                    <i class="far fa-trash-alt"></i>
                                </a>
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'veiculositenscontrato_001.js',
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
            'veiculositenscontrato_001.js',
            'loadList',
            error,
        );
    }
}
