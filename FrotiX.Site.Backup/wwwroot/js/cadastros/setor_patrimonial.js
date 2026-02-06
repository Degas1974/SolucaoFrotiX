/**
 * ═══════════════════════════════════════════════════════════════════════════
 * SETOR PATRIMONIAL - MÓDULO DE LISTAGEM E GERENCIAMENTO
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Gerencia setores patrimoniais do sistema FrotiX.
 *              Na página Index: listagem com DataTables, exclusão e toggle de status.
 *              Na página Upsert: cadastro/edição com validações e combo de detentores.
 * @file setor_patrimonial.js
 * @requires jQuery, DataTables, Syncfusion EJ2 (ComboBox), Alerta, AppToast
 * @see /Views/SetorPatrimonial/Index.cshtml
 * @see /Views/SetorPatrimonial/Upsert.cshtml
 * ═══════════════════════════════════════════════════════════════════════════
 */

/** @type {string} Caminho atual da URL para detecção de página */
var path = window.location.pathname.toLowerCase();
console.log(path);

/**
 * ═══════════════════════════════════════════════════════════════════════════
 * PÁGINA DE LISTAGEM DE SETORES (INDEX)
 * ═══════════════════════════════════════════════════════════════════════════
 * @description Lógica específica para página /setorpatrimonial/index
 */
if (path == '/setorpatrimonial/index' || path == '/setorpatrimonial') {
    // Carrega a grid ao detectar página de listagem
    loadGrid();
    console.log('Entrou na listagem de setores');

    /**
     * ─────────────────────────────────────────────────────────────────
     * INICIALIZAÇÃO DO DOCUMENTO - HANDLERS DE EVENTOS
     * ─────────────────────────────────────────────────────────────────
     * @description Configura handlers de click para exclusão e status
     */
    $(document).ready(function () {
        try {
            /**
             * ─────────────────────────────────────────────────────────
             * HANDLER: EXCLUSÃO DE SETOR
             * ─────────────────────────────────────────────────────────
             * @description Intercepta clique no botão excluir
             * @listens click.btn-delete
             */
            $(document).on('click', '.btn-delete', function () {
                try {
                    /** @type {string} ID (GUID) do setor a ser excluído */
                    var id = $(this).data('id');
                    console.log(id);

                    // Exibe diálogo de confirmação
                    Alerta.Confirmar(
                        'Confirmar Exclusão',
                        'Você tem certeza que deseja apagar este setor? Não será possível recuperar os dados eliminados!',
                        'Sim, excluir',
                        'Cancelar',
                    ).then((willDelete) => {
                        try {
                            // Procede apenas se usuário confirmou
                            if (willDelete) {
                                // Requisição AJAX para excluir setor
                                $.ajax({
                                    url: '/api/Setor/Delete',
                                    type: 'POST',
                                    contentType: 'application/json',
                                    data: JSON.stringify(id),

                                    /**
                                     * @description Callback de sucesso - exibe toast e recarrega
                                     * @param {Object} data - Resposta da API
                                     */
                                    success: function (data) {
                                        try {
                                            if (data.success) {
                                                // Toast verde = sucesso
                                                AppToast.show(
                                                    'Verde',
                                                    data.message,
                                                    2000,
                                                );
                                                // Recarrega tabela
                                                dataTable.ajax.reload();
                                            } else {
                                                // Toast vermelho = erro
                                                AppToast.show(
                                                    'Vermelho',
                                                    data.message,
                                                    2000,
                                                );
                                            }
                                        } catch (error) {
                                            Alerta.TratamentoErroComLinha(
                                                'setor_patrimonial.js',
                                                'btn-delete.ajax.success',
                                                error,
                                            );
                                        }
                                    },

                                    /**
                                     * @description Callback de erro HTTP
                                     * @param {jqXHR} err - Objeto de erro jQuery
                                     */
                                    error: function (err) {
                                        try {
                                            console.error(err);
                                            Alerta.Erro(
                                                'Erro ao Excluir',
                                                'Ocorreu um erro ao tentar excluir o setor patrimonial. Tente novamente.',
                                                'OK',
                                            );
                                        } catch (error) {
                                            Alerta.TratamentoErroComLinha(
                                                'setor_patrimonial.js',
                                                'btn-delete.ajax.error',
                                                error,
                                            );
                                        }
                                    },
                                });
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'setor_patrimonial.js',
                                'btn-delete.confirm.then',
                                error,
                            );
                        }
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'setor_patrimonial.js',
                        'btn-delete.click',
                        error,
                    );
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'document.ready',
                error,
            );
        }
    });

    /**
     * ─────────────────────────────────────────────────────────────────
     * HANDLER: ALTERAÇÃO DE STATUS DO SETOR
     * ─────────────────────────────────────────────────────────────────
     * @description Alterna status Ativo/Inativo via API e atualiza UI
     * @listens click.updateStatusSetor
     */
    $(document).on('click', '.updateStatusSetor', function () {
        try {
            /** @type {string} URL da API com ID do setor */
            var url = $(this).data('url');

            /** @type {jQuery} Referência ao botão clicado */
            var currentElement = $(this);

            // Chamada GET para alternar status
            $.get(url, function (data) {
                try {
                    if (data.success) {
                        /** @type {string} Novo texto do botão */
                        var text;

                        // Atualiza aparência baseado no novo status
                        // data.type == 1 significa que ficou Inativo
                        if (data.type == 1) {
                            text = 'Inativo';
                            // Remove verde, adiciona cinza
                            currentElement
                                .removeClass('btn-verde')
                                .addClass('fundo-cinza');
                        } else {
                            text = 'Ativo';
                            // Remove cinza, adiciona verde
                            currentElement
                                .removeClass('fundo-cinza')
                                .addClass('btn-verde');
                        }

                        // Exibe toast de sucesso
                        AppToast.show('Verde', data.message, 2000);

                        // Atualiza texto do botão
                        currentElement.text(text);
                    } else {
                        Alerta.Erro(
                            'Erro ao Alterar Status',
                            'Ocorreu um erro ao tentar alterar o status. Tente novamente.',
                            'OK',
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'setor_patrimonial.js',
                        'updateStatus.get.callback',
                        error,
                    );
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'updateStatus.click',
                error,
            );
        }
    });

    /**
     * ═══════════════════════════════════════════════════════════════
     * CARREGAMENTO DA GRID DE SETORES
     * ═══════════════════════════════════════════════════════════════
     * @description Inicializa DataTable com dados de setores via AJAX
     * @returns {void}
     */
    function loadGrid() {
        try {
            console.log('Entrou na loadGrid setor');

            // Inicializa DataTable na tabela #tblSetor
            dataTable = $('#tblSetor').DataTable({
                // Configurações de colunas
                columnDefs: [
                    {
                        targets: 0, // Coluna Nome do Setor
                        className: 'text-left',
                        width: '25%',
                    },
                    {
                        targets: 1, // Coluna Nome do Detentor
                        className: 'text-left',
                        width: '35%',
                    },
                    {
                        targets: 2, // Coluna Status (Ativo/Inativo)
                        className: 'text-center',
                        width: '15%',
                    },
                    {
                        targets: 3, // Coluna Ações (Editar/Excluir)
                        className: 'text-center',
                        width: '15%',
                    },
                ],

                responsive: true,

                // Fonte de dados via AJAX
                ajax: {
                    url: '/api/setor/ListaSetores',
                    type: 'GET',
                    datatype: 'json',

                    /**
                     * @description Callback de erro ao carregar dados
                     * @param {jqXHR} xhr - Objeto XMLHttpRequest
                     * @param {string} status - Status do erro
                     * @param {string} error - Mensagem de erro
                     */
                    error: function (xhr, status, error) {
                        try {
                            console.error('Erro ao carregar os dados: ', error);
                            Alerta.Erro(
                                'Erro ao Carregar Dados',
                                'Não foi possível carregar a lista de setores patrimoniais.',
                                'OK',
                            );
                        } catch (error) {
                            Alerta.TratamentoErroComLinha(
                                'setor_patrimonial.js',
                                'loadGrid.ajax.error',
                                error,
                            );
                        }
                    },
                },

                // Mapeamento de colunas
                columns: [
                    // Coluna 0: Nome do setor
                    { data: 'nomeSetor' },

                    // Coluna 1: Nome completo do detentor
                    { data: 'nomeCompleto' },

                    // Coluna 2: Status - renderizado como botão toggle
                    {
                        data: 'status',

                        /**
                         * @description Renderiza botão de status com cores condicionais
                         * @param {boolean} data - Status atual do setor
                         * @param {string} type - Tipo de renderização
                         * @param {Object} row - Dados completos da linha
                         * @returns {string} HTML do botão de status
                         */
                        render: function (data, type, row, meta) {
                            try {
                                if (data) {
                                    // Status ativo = botão verde
                                    return (
                                        '<a href="javascript:void(0)" class="updateStatusSetor btn btn-verde btn-xs text-white" data-url="/api/Setor/updateStatusSetor?Id=' +
                                        row.setorId +
                                        '" data-ejtip="Setor ativo - clique para inativar">Ativo</a>'
                                    );
                                } else {
                                    // Status inativo = botão cinza
                                    return (
                                        '<a href="javascript:void(0)" class="updateStatusSetor btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Setor/updateStatusSetor?Id=' +
                                        row.setorId +
                                        '" data-ejtip="Setor inativo - clique para ativar">Inativo</a>'
                                    );
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha(
                                    'setor_patrimonial.js',
                                    'loadGrid.render.status',
                                    error,
                                );
                                return '';
                            }
                        },
                        width: '6%',
                    },

                    // Coluna 3: Ações (Editar/Excluir)
                    {
                        data: 'setorId',

                        /**
                         * @description Renderiza botões de ação com tooltips
                         * @param {string} data - ID (GUID) do setor
                         * @returns {string} HTML dos botões de ação
                         */
                        render: function (data) {
                            try {
                                return `<div class="text-center">
                                    <a href="/Setorpatrimonial/Upsert?id=${data}"
                                       class="btn btn-azul text-white"
                                       data-ejtip="Editar setor patrimonial"
                                       style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                        <i class="far fa-edit"></i>
                                    </a>
                                    <a class="btn-delete btn btn-vinho text-white"
                                       data-id='${data}'
                                       data-ejtip="Excluir setor patrimonial"
                                       style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                        <i class="far fa-trash-alt"></i>
                                    </a>
                                </div>`;
                            } catch (error) {
                                Alerta.TratamentoErroComLinha(
                                    'setor_patrimonial.js',
                                    'loadGrid.render.actions',
                                    error,
                                );
                                return '';
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
            console.log('Saiu da LoadGrid');
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'loadGrid',
                error,
            );
        }
    }
} else if (path === '/setorpatrimonial/upsert') {
    /**
     * ═══════════════════════════════════════════════════════════════════════
     * PÁGINA DE CADASTRO/EDIÇÃO DE SETOR (UPSERT)
     * ═══════════════════════════════════════════════════════════════════════
     * @description Lógica específica para página /setorpatrimonial/upsert
     *              Gerencia validação do formulário e carregamento de detentores
     */
    console.log('Entrou no setor upsert');

    /**
     * @description Inicializa lista de detentores quando DOM está pronto
     */
    document.addEventListener('DOMContentLoaded', function () {
        try {
            // Carrega lista de usuários detentores para o combo
            loadListaUsuarios();
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'upsert.DOMContentLoaded',
                error,
            );
        }
    });

    /**
     * ─────────────────────────────────────────────────────────────────
     * VALIDAÇÃO DO NOME DO SETOR
     * ─────────────────────────────────────────────────────────────────
     * @description Valida se o campo nome do setor está preenchido antes do submit
     * @returns {void}
     */
    function validaNome() {
        try {
            $(FormsSetor).on('submit', function (event) {
                try {
                    // Obtém valor do campo nome
                    var nomeSetor =
                        document.getElementsByName('SetorObj.NomeSetor')[0]
                            .value;

                    // Bloqueia submit se nome está vazio
                    if (nomeSetor === '') {
                        event.preventDefault(); // Impede reload da página
                        Alerta.Erro(
                            'Erro no Nome do Setor',
                            'O nome do setor não pode estar em branco!',
                            'OK',
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'setor_patrimonial.js',
                        'validaNome.submit',
                        error,
                    );
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'validaNome',
                error,
            );
        }
    }

    /**
     * ─────────────────────────────────────────────────────────────────
     * VALIDAÇÃO DO DETENTOR
     * ─────────────────────────────────────────────────────────────────
     * @description Valida se um detentor foi selecionado antes do submit
     * @returns {void}
     */
    function validaDetentor() {
        try {
            $(FormsSetor).on('submit', function (event) {
                try {
                    // Obtém valor do campo detentor
                    var detentorId = document.getElementsByName(
                        'SetorObj.DetentorId',
                    )[0];

                    // Bloqueia submit se detentor não foi selecionado
                    if (detentorId === '' || detentorId == null) {
                        event.preventDefault(); // Impede reload da página
                        Alerta.Erro(
                            'Erro no Detentor',
                            'O detentor da seção não pode estar em branco!',
                            'OK',
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'setor_patrimonial.js',
                        'validaDetentor.submit',
                        error,
                    );
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'validaDetentor',
                error,
            );
        }
    }

    /**
     * ─────────────────────────────────────────────────────────────────
     * CARREGAMENTO DA LISTA DE USUÁRIOS DETENTORES
     * ─────────────────────────────────────────────────────────────────
     * @description Carrega lista de usuários que podem ser detentores
     *              e popula o ComboBox Syncfusion
     * @returns {void}
     */
    function loadListaUsuarios() {
        try {
            /** @type {ej.dropdowns.ComboBox} Instância do ComboBox Syncfusion */
            var comboBox =
                document.getElementById('cmbDetentores').ej2_instances[0];

            // Requisição AJAX para buscar usuários detentores
            $.ajax({
                type: 'get',
                url: '/api/usuario/listaUsuariosDetentores',
                dataType: 'json',

                /**
                 * @description Callback de sucesso - popula o ComboBox
                 * @param {Object} res - Resposta da API
                 * @param {Array} res.data - Lista de usuários
                 */
                success: function (res) {
                    try {
                        if (res != null && res.data.length) {
                            // Popula o ComboBox com os dados recebidos
                            comboBox.dataSource = res.data;

                            // Define mapeamento de campos
                            comboBox.fields = {
                                text: 'nomeCompleto',
                                value: 'usuarioId',
                            };
                        } else {
                            console.log('Nenhum setor encontrado.');
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'setor_patrimonial.js',
                            'loadListaUsuarios.success',
                            error,
                        );
                    }
                },

                /**
                 * @description Callback de erro ao carregar detentores
                 * @param {jqXHR} error - Objeto de erro jQuery
                 */
                error: function (error) {
                    try {
                        console.error('Erro ao carregar detentores: ', error);
                        Alerta.Erro(
                            'Erro ao Carregar Detentores',
                            'Não foi possível carregar a lista de detentores. Tente novamente.',
                            'OK',
                        );
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'setor_patrimonial.js',
                            'loadListaUsuarios.error',
                            error,
                        );
                    }
                },
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'loadListaUsuarios',
                error,
            );
        }
    }

    /**
     * ─────────────────────────────────────────────────────────────────
     * INICIALIZAÇÃO DO CHECKBOX DE STATUS
     * ─────────────────────────────────────────────────────────────────
     * @description Marca checkbox de status como ativo para novos registros
     *              (detecta pelo GUID vazio)
     */
    document.addEventListener('DOMContentLoaded', function () {
        try {
            // Obtém elemento com o ID do setor
            const infoDiv = document.getElementById('divSetorIdEmpty');

            // Lê o GUID do atributo data
            const setorId = infoDiv.dataset.setorid;
            console.log('Guid do Setor:', setorId);

            /** @type {boolean} Indica se é um novo registro (GUID vazio) */
            const isEmptyGuid =
                setorId === '00000000-0000-0000-0000-000000000000';

            // Obtém referência do checkbox de status
            const checkbox = document.getElementById('chkStatus');

            // Se for novo registro, marca como ativo por padrão
            if (isEmptyGuid && checkbox) {
                checkbox.checked = true;
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha(
                'setor_patrimonial.js',
                'upsert.initCheckbox',
                error,
            );
        }
    });
}
