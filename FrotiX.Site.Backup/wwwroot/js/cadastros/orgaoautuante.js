/* ****************************************************************************************
 * ⚡ ARQUIVO: orgaoautuante.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Lista de Órgãos Autuantes com DataTable, exclusão delegada via
 *                   confirmação modal, botões GLOW e ícones duotone (padrão FrotiX).
 * 📥 ENTRADAS     : Clique em .btn-delete (data-id), resposta Alerta.Confirmar (confirmed)
 * 📤 SAÍDAS       : DELETE via AJAX para /api/Multa/DeleteOrgaoAutuante,
 *                   AppToast (Verde/Vermelho), dataTable.ajax.reload,
 *                   Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : $(document).ready (loadList), event handler .btn-delete,
 *                   Pages/OrgaoAutuante/Index.cshtml
 * 🔄 CHAMA        : loadList(), Alerta.Confirmar, $.ajax, AppToast.show,
 *                   dataTable.ajax.reload, Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery 3.x, DataTables, Alerta.js, AppToast (toast notifications)
 * 📝 OBSERVAÇÕES  : Handler delegado para compatibilidade com DataTable dinâmico.
 *                   Try-catch aninhado em todos os níveis (ready, click, .then,
 *                   success, error). 165 linhas total.
 **************************************************************************************** */

/* global $, Alerta, AppToast */

// ============================================================================
// VARIÁVEL GLOBAL - DataTable
// Mantida no escopo global para permitir reload via ajax.reload()
// ============================================================================
var dataTable;

// ============================================================================
// INICIALIZAÇÃO DO MÓDULO
// Aguarda DOM pronto, carrega a lista e configura event handlers
// ============================================================================
$(document).ready(function () {
    try {
        // Carrega a listagem de órgãos autuantes
        loadList();

        // ================================================================
        // EVENT HANDLER: BOTÃO EXCLUIR ÓRGÃO AUTUANTE
        // Usa delegação de eventos para funcionar com elementos dinâmicos
        // Exibe confirmação antes de prosseguir com a exclusão
        // ================================================================
        $(document).on('click', '.btn-delete', function () {
            try {
                // Obtém o ID do órgão do atributo data-id do botão clicado
                var id = $(this).data('id');

                // Exibe modal de confirmação via FrotiX Alerta
                Alerta.Confirmar(
                    'Você tem certeza que deseja apagar este órgão?',
                    'Não será possível recuperar os dados eliminados!',
                    'Excluir',
                    'Cancelar',
                ).then(function (confirmed) {
                    try {
                        // Se usuário cancelou, não faz nada
                        if (!confirmed) return;

                        // Monta o payload JSON com o ID do órgão
                        var dataToPost = JSON.stringify({
                            OrgaoAutuanteId: id,
                        });
                        var url = '/api/Multa/DeleteOrgaoAutuante';

                        // Executa a exclusão via API
                        $.ajax({
                            url: url,
                            type: 'POST',
                            data: dataToPost,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                try {
                                    // Exibe feedback visual via toast
                                    if (data.success) {
                                        AppToast.show(
                                            'Verde',
                                            data.message,
                                            2000,
                                        );
                                        // Recarrega a tabela mantendo a paginação atual
                                        dataTable.ajax.reload(null, false);
                                    } else {
                                        AppToast.show(
                                            'Vermelho',
                                            data.message,
                                            3000,
                                        );
                                    }
                                } catch (error) {
                                    Alerta.TratamentoErroComLinha(
                                        'orgaoautuante.js',
                                        'btn-delete.success',
                                        error,
                                    );
                                }
                            },
                            error: function (err) {
                                try {
                                    console.error('Erro ao excluir:', err);
                                    Alerta.Erro(
                                        'Erro',
                                        'Ocorreu um erro ao excluir o órgão',
                                        'OK',
                                    );
                                } catch (error) {
                                    Alerta.TratamentoErroComLinha(
                                        'orgaoautuante.js',
                                        'btn-delete.error',
                                        error,
                                    );
                                }
                            },
                        });
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'orgaoautuante.js',
                            'btn-delete.confirmar',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'orgaoautuante.js',
                    'btn-delete.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'orgaoautuante.js',
            'document.ready',
            error,
        );
    }
});

// ============================================================================
// FUNÇÃO: CARREGAR LISTA DE ÓRGÃOS AUTUANTES
// Inicializa o DataTable com configuração de colunas e fonte de dados AJAX
// ============================================================================

/**
 * @function loadList
 * @description Inicializa o DataTable de Órgãos Autuantes.
 *              Configura colunas (Sigla, Nome, Ações) e carrega dados via API.
 * @returns {void}
 * @fires DataTable - Inicializa tabela com dados de /api/Multa/PegaOrgaoAutuante
 */
function loadList() {
    try {
        // Inicializa o DataTable na tabela #tblOrgaoAutuante
        dataTable = $('#tblOrgaoAutuante').DataTable({
            // ================================================================
            // CONFIGURAÇÃO DE COLUNAS: Define larguras e alinhamentos
            // ================================================================
            columnDefs: [
                {
                    targets: 0, // Coluna Sigla (ex: DETRAN, PRF)
                    className: 'text-left',
                    width: '15%',
                },
                {
                    targets: 1, // Coluna Nome completo do órgão
                    className: 'text-left',
                    width: '70%',
                },
                {
                    targets: 2, // Coluna Ações (Editar, Excluir)
                    className: 'text-center',
                    width: '15%',
                    render: function (data, type, full) {
                        try {
                            // Renderiza botões de ação com padrão FrotiX GLOW
                            // data = orgaoAutuanteId (usado para edição e exclusão)
                            return `<div class="text-center" style="white-space: nowrap;">
                                <a href="/Multa/UpsertOrgaoAutuante?id=${data}" 
                                   class="ftx-btn-icon ftx-btn-editar"
                                   data-ejtip="Editar Órgão Autuante">
                                    <i class="fa-duotone fa-pen-to-square"></i>
                                </a>
                                <a class="ftx-btn-icon ftx-btn-apagar btn-delete"
                                   data-ejtip="Excluir Órgão Autuante"
                                   data-id="${data}">
                                    <i class="fa-duotone fa-trash-can"></i>
                                </a>
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'orgaoautuante.js',
                                'render.acoes',
                                error,
                            );
                            return '';
                        }
                    },
                },
            ],

            responsive: true, // Adapta a tabela para telas menores

            // ================================================================
            // FONTE DE DADOS: API REST que retorna lista de órgãos
            // ================================================================
            ajax: {
                url: '/api/Multa/PegaOrgaoAutuante',
                type: 'GET',
                datatype: 'json',
            },

            // ================================================================
            // MAPEAMENTO DE COLUNAS: Define quais campos do JSON exibir
            // ================================================================
            columns: [
                { data: 'sigla' }, // Sigla do órgão (ex: DETRAN)
                { data: 'nome' }, // Nome completo do órgão
                { data: 'orgaoAutuanteId' }, // ID para ações (render customizado)
            ],

            // Tradução para Português Brasil
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
                emptyTable: 'Nenhum órgão autuante cadastrado',
            },
            width: '100%',
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('orgaoautuante.js', 'loadList', error);
    }
}
