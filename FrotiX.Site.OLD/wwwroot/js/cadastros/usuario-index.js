/* ****************************************************************************************
 * ⚡ ARQUIVO: usuario-index.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar a listagem de usuários com operações de CRUD, controle de
 *                   acesso a recursos, visualização de fotos, e gerenciamento de status.
 *                   Implementa DataTable responsivo, modais para fotos e recursos, e
 *                   atualização de status/carga patrimonial via AJAX.
 *
 * 📥 ENTRADAS     : Eventos DOM (cliques em botões, aberturas de modais), dados da API
 *                   através de endpoints REST (/api/Usuario/GetAll, /api/Usuario/PegaRecursosUsuario).
 *
 * 📤 SAÍDAS       : Manipulação de DOM (tabelas DataTable, modais), chamadas AJAX,
 *                   toasts de notificação, redirecionamentos para páginas de edição.
 *
 * 🔗 CHAMADA POR  : Página /Usuarios/Index (view que carrega este script).
 *
 * 🔄 CHAMA        : APIs REST (/api/Usuario/*), Alerta (SweetAlert), AppToast,
 *                   FtxSpin (loading overlay), DataTable jQuery.
 *
 * 📦 DEPENDÊNCIAS : jQuery 3.6+, DataTables 1.10.25+, Bootstrap 5.3, SweetAlert2,
 *                   Syncfusion EJ2 (tooltips), Font Awesome 6.5 (ícones),
 *                   alerta.js, frotix.js (AppToast/FtxSpin).
 *
 * 📝 OBSERVAÇÕES  :
 *   • Uso de IIFE para evitar poluição de escopo global
 *   • Delegação de eventos para compatibilidade com elementos dinâmicos
 *   • Destruição/recriação de DataTables para evitar memory leaks
 *   • Try-catch em TODAS as funções conforme padrão FrotiX
 *   • Ícones fa-duotone (nunca fa-solid)
 *   • Tooltips Syncfusion (data-ejtip)
 **************************************************************************************** */

(function () {
    "use strict";

    // =============================================
    // VARIÁVEIS GLOBAIS (Escopo IIFE)
    // =============================================
    let dataTableUsuarios = null;       // Instância da DataTable de usuários
    let dataTableRecursos = null;       // Instância da DataTable de recursos do modal
    let modalControleAcessoInstance = null;  // Instância Bootstrap Modal (recursos)
    let modalFotoInstance = null;       // Instância Bootstrap Modal (foto)"

    // =============================================
    // INICIALIZAÇÃO - PONTO DE ENTRADA
    // =============================================
    /****************************************************************************************
     * ⚡ EVENTO: document.addEventListener('DOMContentLoaded')
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Disparado quando o DOM está completamente carregado. Inicializa
     *                   as tabelas, modais e delegação de eventos para toda a página.
     *
     * 📤 SAÍDAS       : DataTables instanciadas, modais configuradas, listeners configurados.
     ****************************************************************************************/
    document.addEventListener('DOMContentLoaded', function () {
        try {
            // [UI] Inicializar componentes da página
            inicializarDataTableUsuarios();   // DataTable principal de usuários
            inicializarModais();              // Modais Bootstrap
            configurarEventosDelegados();     // Listeners de eventos delegados
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "DOMContentLoaded", error);
        }
    });

    // =============================================
    // INICIALIZAR MODAIS BOOTSTRAP
    // =============================================
    /****************************************************************************************
     * ⚡ FUNÇÃO: inicializarModais
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Instanciar modais Bootstrap e configurar eventos de fechamento
     *                   para limpeza de dados. Inicializa dois modais:
     *                   1. modalControleAcesso: gerenciamento de recursos/permissões
     *                   2. modalFoto: visualização ampliada de foto do usuário
     *
     * 📤 SAÍDAS       : Modais globais instanciados (modalControleAcessoInstance,
     *                   modalFotoInstance), listeners de "hidden.bs.modal" configurados.
     *
     * 🔗 CHAMADA POR  : DOMContentLoaded (inicializarModais)
     *
     * 🔄 CHAMA        : bootstrap.Modal, addEventListener
     *
     * 📝 OBSERVAÇÕES  : Garante que dados residuais sejam limpos quando modais fecham
     *                   para evitar conflitos em aberturas subsequentes.
     ****************************************************************************************/
    function inicializarModais() {
        try {
            // [UI] Modal Controle de Acesso (Recursos/Permissões)
            const modalControleAcessoEl = document.getElementById('modalControleAcesso');
            if (modalControleAcessoEl) {
                modalControleAcessoInstance = new bootstrap.Modal(modalControleAcessoEl);

                // [UI] Limpar dados ao fechar modal
                modalControleAcessoEl.addEventListener('hidden.bs.modal', function () {
                    try {
                        document.getElementById('txtUsuarioIdRecurso').value = '';
                        document.getElementById('txtNomeUsuarioRecurso').textContent = '';
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("usuario-index.js", "modalControleAcesso.hidden", error);
                    }
                });
            }

            // [UI] Modal Foto (Visualização Ampliada)
            const modalFotoEl = document.getElementById('modalFoto');
            if (modalFotoEl) {
                modalFotoInstance = new bootstrap.Modal(modalFotoEl);

                // [UI] Limpar dados ao fechar modal
                modalFotoEl.addEventListener('hidden.bs.modal', function () {
                    try {
                        document.getElementById('txtNomeUsuarioFoto').textContent = '';
                        document.getElementById('divFotoContainer').innerHTML = '';
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("usuario-index.js", "modalFoto.hidden", error);
                    }
                });
            }

        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "inicializarModais", error);
        }
    }

    // =============================================
    // CARREGAR RECURSOS DO USUÁRIO
    // =============================================
    /****************************************************************************************
     * ⚡ FUNÇÃO: carregarRecursosUsuario
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Buscar lista de recursos (permissões) de um usuário específico via API
     *                   e popular uma DataTable dentro do modal de controle de acesso.
     *                   Permite gerenciar quais recursos o usuário tem acesso.
     *
     * 📥 ENTRADAS     : usuarioId [int] - ID do usuário para buscar recursos
     *
     * 📤 SAÍDAS       : DataTable populada com recursos, cada linha contendo:
     *                   - Nome do recurso
     *                   - Botão toggle de status (Com Acesso / Sem Acesso)
     *
     * 🔗 CHAMADA POR  : click.btnRecursos (botão gerenciar recursos na tabela principal)
     *
     * 🔄 CHAMA        : GET /api/Usuario/PegaRecursosUsuario, DataTable jQuery
     *
     * 📝 OBSERVAÇÕES  :
     *   • Destrói DataTable anterior antes de criar nova (evita duplicação)
     *   • Usa render() para renderizar botões de toggle dinamicamente
     *   • Cores: verde (acesso) e cinza (sem acesso)
     *   • Ícones: fa-duotone fa-unlock (com acesso) ou fa-lock (sem acesso)
     ****************************************************************************************/
    function carregarRecursosUsuario(usuarioId) {
        try {
            // [LOGICA] Destroi tabela anterior se existir (evita memory leak)
            if ($.fn.DataTable.isDataTable('#tblRecursos')) {
                $('#tblRecursos').DataTable().clear().destroy();
            }

            // [AJAX] Inicializar nova DataTable com dados da API
            dataTableRecursos = $('#tblRecursos').DataTable({
                // [LOGICA] Configuração de ordenação padrão por nome (coluna 0)
                order: [[0, 'asc']],
                columnDefs: [
                    { targets: 0, className: "text-left" },       // Nome do recurso: alinhado à esquerda
                    { targets: 1, className: "text-center", width: "130px" }  // Status: centralizaado, largura fixa
                ],
                responsive: true,
                // [AJAX] Endpoint: GET /api/Usuario/PegaRecursosUsuario
                // 📥 ENVIA    : usuarioId (parâmetro query)
                // 📤 RECEBE   : { data: [{ids, nome, acesso}], ... }
                ajax: {
                    url: "/api/Usuario/PegaRecursosUsuario",
                    type: "GET",
                    datatype: "json",
                    data: { usuarioId: usuarioId },
                    error: function (xhr, error, code) {
                        console.error("Erro ao carregar recursos:", error);
                        AppToast.show("Vermelho", "Erro ao carregar recursos do usuário", 5000);
                    }
                },
                // [UI] Definição de colunas
                columns: [
                    // Coluna 0: Nome do Recurso
                    { data: "nome" },
                    // Coluna 1: Status de Acesso (Com Acesso / Sem Acesso)
                    {
                        data: "acesso",
                        render: function (data, type, row) {
                            try {
                                const url = `/api/Usuario/UpdateStatusAcesso?IDS=${row.ids}`;
                                if (data === true) {
                                    // [UI] Botão verde com ícone de desbloqueio
                                    return `<a href="javascript:void(0)"
                                               class="btn btn-xs ftx-badge-status btn-verde updateStatusAcesso"
                                               data-url="${url}"
                                               data-ejtip="Clique para remover acesso">
                                                <i class="fa-duotone fa-unlock me-1"></i>Com Acesso
                                            </a>`;
                                } else {
                                    // [UI] Botão cinza com ícone de bloqueio
                                    return `<a href="javascript:void(0)"
                                               class="btn btn-xs ftx-badge-status fundo-cinza updateStatusAcesso"
                                               data-url="${url}"
                                               data-ejtip="Clique para conceder acesso">
                                                <i class="fa-duotone fa-lock me-1"></i>Sem Acesso
                                            </a>`;
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.acesso", error);
                                return '';
                            }
                        }
                    }
                ],
                // [UI] Idioma português Brasil
                language: {
                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                    emptyTable: "Nenhum recurso disponível"
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "carregarRecursosUsuario", error);
        }
    }

    // =============================================
    // DATATABLE DE USUÁRIOS (TABELA PRINCIPAL)
    // =============================================
    /****************************************************************************************
     * ⚡ FUNÇÃO: inicializarDataTableUsuarios
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Criar e configurar a DataTable principal que exibe lista de usuários
     *                   com 5 colunas: Nome+Avatar, Ponto, Detentor Carga, Status, Ações.
     *                   Implementa renderizadores customizados para avatares, badges de
     *                   status, e botões de ação (Editar, Recursos, Foto, Excluir).
     *
     * 📤 SAÍDAS       : DataTable populada com usuários da API, com:
     *                   - Avatar com foto ou ícone de usuário
     *                   - Badges de status (Ativo/Inativo, Detentor/Não Detentor)
     *                   - Botões de ação com ativação/desativação condicional
     *
     * 🔗 CHAMADA POR  : DOMContentLoaded (através de inicializarDataTableUsuarios)
     *
     * 🔄 CHAMA        : GET /api/Usuario/GetAll (carrega dados), DataTable jQuery
     *
     * 📝 OBSERVAÇÕES  :
     *   • Colunas possuem larguras fixas em percentuais para melhor responsividade
     *   • Coluna de ações não é ordenável (orderable: false)
     *   • Renderizadores customizados para avatares, status e botões
     *   • Botões de Foto e Excluir são desabilitados condicionalmente com tooltips
     ****************************************************************************************/
    function inicializarDataTableUsuarios() {
        try {
            // [UI] Inicializar DataTable com configurações de layout
            dataTableUsuarios = $('#tblUsuario').DataTable({
                // [LOGICA] Ordenação padrão por nome (coluna 0)
                order: [[0, 'asc']],
                autoWidth: false,
                columnDefs: [
                    { targets: 0, className: "text-left", width: "40%" },     // Nome+Avatar
                    { targets: 1, className: "text-center", width: "15%" },   // Ponto
                    { targets: 2, className: "text-center", width: "15%" },   // Detentor Carga
                    { targets: 3, className: "text-center", width: "15%" },   // Status
                    { targets: 4, className: "text-center ftx-actions", width: "15%", orderable: false }  // Ações
                ],
                responsive: true,
                // [AJAX] Endpoint: GET /api/Usuario/GetAll
                // 📥 ENVIA    : nenhum parâmetro
                // 📤 RECEBE   : { data: [{usuarioId, nomeCompleto, ponto, detentorCargaPatrimonial, status, fotoBase64, podeExcluir}], ... }
                ajax: {
                    url: "/api/Usuario/GetAll",
                    type: "GET",
                    datatype: "json",
                    error: function (xhr, error, code) {
                        console.error("Erro ao carregar usuários:", error);
                        AppToast.show("Vermelho", "Erro ao carregar lista de usuários", 5000);
                    }
                },
                // [UI] Definição de colunas
                columns: [
                    // Coluna 0: Nome Completo com Avatar
                    {
                        data: "nomeCompleto",
                        render: function (data, type, row) {
                            try {
                                const nome = data || 'Sem Nome';

                                // [LOGICA] Para ordenação e filtro, retornar apenas texto puro
                                if (type === 'sort' || type === 'filter') {
                                    return nome;
                                }

                                // [UI] Para exibição visual, renderizar com avatar + nome
                                const foto = row.fotoBase64;

                                let avatarHtml = '';
                                if (foto && foto.trim() !== '') {
                                    // [UI] Avatar com foto em base64 (clicável)
                                    avatarHtml = `<div class="ftx-avatar btnAbrirFoto"
                                                       data-id="${row.usuarioId}"
                                                       data-nome="${nome}"
                                                       data-foto="${foto}"
                                                       data-ejtip="Clique para ampliar foto"
                                                       style="cursor:pointer;">
                                                    <img src="data:image/jpeg;base64,${foto}"
                                                         class="ftx-avatar-img is-visible"
                                                         alt="${nome}" />
                                                  </div>`;
                                } else {
                                    // [UI] Avatar sem foto - ícone genérico de usuário
                                    avatarHtml = `<div class="ftx-avatar" data-ejtip="Usuário sem foto">
                                                    <span class="ftx-avatar-ico"><i class="fa-duotone fa-user"></i></span>
                                                  </div>`;
                                }

                                // [UI] Container com avatar + nome em flexbox
                                return `<div class="d-flex align-items-center ftx-row-gap">
                                            ${avatarHtml}
                                            <span>${nome}</span>
                                        </div>`;
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.nomeCompleto", error);
                                return '';
                            }
                        }
                    },
                    // Coluna 1: Ponto
                    {
                        data: "ponto",
                        render: function (data) {
                            // [UI] Exibir ponto ou "-" se vazio
                            return data || '-';
                        }
                    },
                    // Coluna 2: Detentor de Carga Patrimonial (Toggle Sim/Não)
                    {
                        data: "detentorCargaPatrimonial",
                        render: function (data, type, row) {
                            try {
                                // [AJAX] Endpoint: GET /api/Usuario/UpdateCargaPatrimonial
                                // 📥 ENVIA    : Id (query parameter)
                                // 📤 RECEBE   : { success, message }
                                const url = `/api/Usuario/UpdateCargaPatrimonial?Id=${row.usuarioId}`;
                                if (data === true) {
                                    // [UI] Badge verde - usuário é detentor
                                    return `<a href="javascript:void(0)"
                                               class="btn btn-xs ftx-badge-status btn-verde updateCargaPatrimonial"
                                               data-url="${url}"
                                               data-ejtip="Clique para remover como Detentor">
                                                <i class="fa-duotone fa-badge-check me-1"></i>Sim
                                            </a>`;
                                } else {
                                    // [UI] Badge cinza - usuário não é detentor
                                    return `<a href="javascript:void(0)"
                                               class="btn btn-xs ftx-badge-status fundo-cinza updateCargaPatrimonial"
                                               data-url="${url}"
                                               data-ejtip="Clique para definir como Detentor">
                                                <i class="fa-duotone fa-circle-xmark me-1"></i>Não
                                            </a>`;
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.detentorCarga", error);
                                return '';
                            }
                        }
                    },
                    // Coluna 3: Status (Toggle Ativo/Inativo)
                    {
                        data: "status",
                        render: function (data, type, row) {
                            try {
                                // [AJAX] Endpoint: GET /api/Usuario/UpdateStatusUsuario
                                // 📥 ENVIA    : Id (query parameter)
                                // 📤 RECEBE   : { success, message }
                                const url = `/api/Usuario/UpdateStatusUsuario?Id=${row.usuarioId}`;
                                if (data === true) {
                                    // [UI] Badge verde - usuário ativo
                                    return `<a href="javascript:void(0)"
                                               class="btn btn-xs ftx-badge-status btn-verde updateStatusUsuario"
                                               data-url="${url}"
                                               data-ejtip="Clique para inativar">
                                                <i class="fa-duotone fa-circle-check me-1"></i>Ativo
                                            </a>`;
                                } else {
                                    // [UI] Badge cinza - usuário inativo
                                    return `<a href="javascript:void(0)"
                                               class="btn btn-xs ftx-badge-status fundo-cinza updateStatusUsuario"
                                               data-url="${url}"
                                               data-ejtip="Clique para ativar">
                                                <i class="fa-duotone fa-circle-xmark me-1"></i>Inativo
                                            </a>`;
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.status", error);
                                return '';
                            }
                        }
                    },
                    // Coluna 4: Ações (Editar, Recursos, Foto, Excluir)
                    {
                        data: null,
                        render: function (data, type, row) {
                            try {
                                const temFoto = row.fotoBase64;
                                const podeExcluir = row.podeExcluir !== false;

                                // [UI] Botão Editar - SEMPRE habilitado (azul)
                                let btnEditar = `<a href="/Usuarios/Upsert?id=${row.usuarioId}"
                                                    class="btn btn-azul btn-icon-28"
                                                    data-ejtip="Editar usuário">
                                                    <i class="fa-duotone fa-pen-to-square"></i>
                                                </a>`;

                                // [UI] Botão Gerenciar Recursos - SEMPRE habilitado (laranja)
                                let btnRecursos = `<button type="button"
                                                          class="btn btn-fundo-laranja btn-icon-28 btnRecursos"
                                                          data-id="${row.usuarioId}"
                                                          data-nome="${row.nomeCompleto || 'Usuário'}"
                                                          data-ejtip="Gerenciar recursos">
                                                        <i class="fa-duotone fa-shield-keyhole"></i>
                                                    </button>`;

                                // [UI] Botão Visualizar Foto - Condicional (habilitado se tem foto)
                                let btnFoto = '';
                                if (temFoto) {
                                    // Botão habilitado (azul claro)
                                    btnFoto = `<button type="button"
                                                      class="btn btn-foto btn-icon-28 btnFoto"
                                                      data-id="${row.usuarioId}"
                                                      data-nome="${row.nomeCompleto || 'Usuário'}"
                                                      data-foto="${row.fotoBase64}"
                                                      data-ejtip="Visualizar foto">
                                                    <i class="fa-duotone fa-id-badge"></i>
                                                </button>`;
                                } else {
                                    // Botão desabilitado com span wrapper para exibir tooltip
                                    btnFoto = `<span class="d-inline-block" tabindex="0" data-ejtip="Usuário sem foto">
                                                    <button type="button"
                                                            class="btn btn-foto btn-icon-28"
                                                            disabled
                                                            style="pointer-events: none;">
                                                        <i class="fa-duotone fa-id-badge" style="pointer-events: none;"></i>
                                                    </button>
                                                </span>`;
                                }

                                // [UI] Botão Excluir - Condicional (habilitado se não está em uso)
                                let btnExcluir = '';
                                if (podeExcluir) {
                                    // Botão habilitado (vinho/vermelho)
                                    btnExcluir = `<button type="button"
                                                         class="btn btn-vinho btn-icon-28 btnExcluir"
                                                         data-id="${row.usuarioId}"
                                                         data-nome="${row.nomeCompleto || 'Usuário'}"
                                                         data-ejtip="Excluir usuário">
                                                        <i class="fa-duotone fa-trash-can"></i>
                                                    </button>`;
                                } else {
                                    // Botão desabilitado com span wrapper para exibir tooltip
                                    btnExcluir = `<span class="d-inline-block" tabindex="0" data-ejtip="Usuário não pode ser excluído pois está em uso">
                                                       <button type="button"
                                                               class="btn btn-vinho btn-icon-28"
                                                               disabled
                                                               style="pointer-events: none;">
                                                           <i class="fa-duotone fa-trash-can" style="pointer-events: none;"></i>
                                                       </button>
                                                   </span>`;
                                }

                                // [UI] Concatenar todos os botões
                                return btnEditar + btnRecursos + btnFoto + btnExcluir;
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("usuario-index.js", "render.acoes", error);
                                return '';
                            }
                        }
                    }
                ],
                // [UI] Idioma português Brasil
                language: {
                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                    emptyTable: "Nenhum usuário cadastrado"
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "inicializarDataTableUsuarios", error);
        }
    }

    // =============================================
    // EVENTOS DELEGADOS (CLIQUES NA TABELA E MODAIS)
    // =============================================
    /****************************************************************************************
     * ⚡ FUNÇÃO: configurarEventosDelegados
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Configurar delegação de eventos para elementos dinâmicos renderizados
     *                   pela DataTable. Implementa 7 tipos de eventos:
     *                   1. Clique em avatar para abrir foto
     *                   2. Clique em botão Foto
     *                   3. Clique em botão Recursos
     *                   4. Toggle Status Usuário
     *                   5. Toggle Carga Patrimonial
     *                   6. Toggle Status Acesso (modal recursos)
     *                   7. Excluir usuário
     *
     * 📤 SAÍDAS       : Listeners jQuery configurados com $(document).on()
     *
     * 🔗 CHAMADA POR  : DOMContentLoaded (através de configurarEventosDelegados)
     *
     * 🔄 CHAMA        : abrirModalFoto(), carregarRecursosUsuario(), executarAcaoAjax(),
     *                   confirmarExclusao(), modalControleAcessoInstance.show()
     *
     * 📝 OBSERVAÇÕES  :
     *   • Usa $(document).on() para capturar eventos de elementos dinâmicos
     *   • Cada listener tem seu próprio try-catch
     *   • Chamadas AJAX retornam Promises que disparao reload de DataTables
     ****************************************************************************************/
    function configurarEventosDelegados() {
        try {
            // [EVENTO] Clique no avatar (imagem de foto) na tabela
            $(document).on('click', '.btnAbrirFoto', function (e) {
                try {
                    e.preventDefault();
                    e.stopPropagation();

                    const nome = $(this).data('nome');
                    const foto = $(this).data('foto');

                    abrirModalFoto(nome, foto);
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnAbrirFoto", error);
                }
            });

            // [EVENTO] Clique em botão Visualizar Foto (ações)
            $(document).on('click', '.btnFoto:not(.disabled)', function (e) {
                try {
                    e.preventDefault();
                    e.stopPropagation();

                    const nome = $(this).data('nome');
                    const foto = $(this).data('foto');

                    if (foto) {
                        abrirModalFoto(nome, foto);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnFoto", error);
                }
            });

            // [EVENTO] Clique em botão Gerenciar Recursos
            $(document).on('click', '.btnRecursos', function (e) {
                try {
                    e.preventDefault();
                    e.stopPropagation();

                    const usuarioId = $(this).data('id');
                    const nomeUsuario = $(this).data('nome');

                    // [UI] Preencher campos do modal
                    document.getElementById('txtUsuarioIdRecurso').value = usuarioId;
                    document.getElementById('txtNomeUsuarioRecurso').textContent = nomeUsuario;

                    // [DADOS] Carregar recursos via AJAX
                    carregarRecursosUsuario(usuarioId);

                    // [UI] Exibir modal
                    if (modalControleAcessoInstance) {
                        modalControleAcessoInstance.show();
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnRecursos", error);
                }
            });

            // [EVENTO] Clique no botão toggle Status Usuário (Ativo/Inativo)
            $(document).on('click', '.updateStatusUsuario', function (e) {
                try {
                    e.preventDefault();
                    const url = $(this).data('url');
                    // [AJAX] Executar update e recarregar DataTable
                    executarAcaoAjax(url, "Status atualizado!", function () {
                        dataTableUsuarios.ajax.reload(null, false);
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.updateStatusUsuario", error);
                }
            });

            // [EVENTO] Clique no botão toggle Carga Patrimonial (Sim/Não)
            $(document).on('click', '.updateCargaPatrimonial', function (e) {
                try {
                    e.preventDefault();
                    const url = $(this).data('url');
                    // [AJAX] Executar update e recarregar DataTable
                    executarAcaoAjax(url, "Carga patrimonial atualizada!", function () {
                        dataTableUsuarios.ajax.reload(null, false);
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.updateCargaPatrimonial", error);
                }
            });

            // [EVENTO] Clique no botão toggle Status Acesso (Com Acesso / Sem Acesso) - MODAL
            $(document).on('click', '.updateStatusAcesso', function (e) {
                try {
                    e.preventDefault();
                    const url = $(this).data('url');
                    // [AJAX] Executar update e recarregar DataTable de recursos
                    executarAcaoAjax(url, "Acesso atualizado!", function () {
                        if (dataTableRecursos) {
                            dataTableRecursos.ajax.reload(null, false);
                        }
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.updateStatusAcesso", error);
                }
            });

            // [EVENTO] Clique em botão Excluir Usuário
            $(document).on('click', '.btnExcluir', function (e) {
                try {
                    e.preventDefault();
                    const usuarioId = $(this).data('id');
                    const nome = $(this).data('nome');
                    // [REGRA] Exibir confirmação antes de excluir
                    confirmarExclusao(usuarioId, nome);
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "click.btnExcluir", error);
                }
            });

        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "configurarEventosDelegados", error);
        }
    }

    // =============================================
    // ABRIR MODAL DE FOTO
    // =============================================
    /****************************************************************************************
     * ⚡ FUNÇÃO: abrirModalFoto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Abrir modal com visualização ampliada de foto do usuário. Se não
     *                   houver foto, exibir placeholder com mensagem.
     *
     * 📥 ENTRADAS     : nome [string] - Nome completo do usuário
     *                   foto [string] - Foto em base64 ou vazio
     *
     * 📤 SAÍDAS       : Modal exibido com foto ou placeholder
     *
     * 🔗 CHAMADA POR  : click.btnAbrirFoto, click.btnFoto
     *
     * 🔄 CHAMA        : modalFotoInstance.show()
     *
     * 📝 OBSERVAÇÕES  : Renderiza foto como data URL (data:image/jpeg;base64,...)
     ****************************************************************************************/
    function abrirModalFoto(nome, foto) {
        try {
            // [UI] Preencher título do modal
            document.getElementById('txtNomeUsuarioFoto').textContent = nome;

            const container = document.getElementById('divFotoContainer');

            if (foto && foto.trim() !== '') {
                // [UI] Renderizar imagem em base64
                container.innerHTML = `<img src="data:image/jpeg;base64,${foto}"
                                            class="img-foto-usuario"
                                            alt="Foto de ${nome}" />`;
            } else {
                // [UI] Renderizar placeholder quando não há foto
                container.innerHTML = `<div class="no-foto-placeholder">
                                           <i class="fa-duotone fa-user-slash"></i>
                                           <p>Usuário sem foto cadastrada</p>
                                       </div>`;
            }

            // [UI] Exibir modal
            if (modalFotoInstance) {
                modalFotoInstance.show();
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "abrirModalFoto", error);
        }
    }

    // =============================================
    // FUNÇÕES AUXILIARES
    // =============================================
    /****************************************************************************************
     * ⚡ FUNÇÃO: executarAcaoAjax
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar requisição AJAX GET genérica e exibir toast de sucesso/erro.
     *                   Reutilizada para update de status (usuário, carga, acesso).
     *
     * 📥 ENTRADAS     : url [string] - URL completa do endpoint (GET)
     *                   mensagemSucesso [string] - Mensagem toast padrão
     *                   callback [function] - Função a executar após sucesso (ex: reload)
     *
     * 📤 SAÍDAS       : Toast exibido, callback executado (se sucesso)
     *
     * 🔗 CHAMADA POR  : click.updateStatusUsuario, click.updateCargaPatrimonial,
     *                   click.updateStatusAcesso
     *
     * 🔄 CHAMA        : $.ajax, AppToast.show(), callback()
     *
     * 📝 OBSERVAÇÕES  :
     *   • Verifica response.success antes de considerar sucesso
     *   • Toast verde para sucesso (3s), vermelho para erro (5s)
     *   • Callback é opcional
     ****************************************************************************************/
    function executarAcaoAjax(url, mensagemSucesso, callback) {
        try {
            // [AJAX] Executar GET request
            $.ajax({
                url: url,
                type: "GET",
                success: function (response) {
                    try {
                        // [LOGICA] Verificar se resposta foi bem-sucedida
                        if (response.success) {
                            // [UI] Toast verde com mensagem
                            AppToast.show("Verde", response.message || mensagemSucesso, 3000);
                            // [LOGICA] Executar callback se fornecido (geralmente reload de tabela)
                            if (typeof callback === 'function') {
                                callback();
                            }
                        } else {
                            // [UI] Toast vermelho com mensagem de erro
                            AppToast.show("Vermelho", response.message || "Erro ao executar ação", 5000);
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("usuario-index.js", "executarAcaoAjax.success", error);
                    }
                },
                error: function (xhr, status, error) {
                    // [DEBUG] Log erro
                    console.error("Erro AJAX:", error);
                    // [UI] Toast vermelho genérico
                    AppToast.show("Vermelho", "Erro ao executar ação", 5000);
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "executarAcaoAjax", error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: confirmarExclusao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Exibir modal de confirmação SweetAlert antes de excluir usuário.
     *                   Alerta o usuário que a ação não pode ser desfeita e explica que
     *                   o sistema validará integridade referencial.
     *
     * 📥 ENTRADAS     : usuarioId [int] - ID do usuário a excluir
     *                   nome [string] - Nome do usuário (para exibir no modal)
     *
     * 📤 SAÍDAS       : Promise que resolve true (confirmar) ou false (cancelar)
     *
     * 🔗 CHAMADA POR  : click.btnExcluir
     *
     * 🔄 CHAMA        : Alerta.Confirmar(), excluirUsuario()
     *
     * 📝 OBSERVAÇÕES  : O sistema valida constraints FK na API antes de excluir
     ****************************************************************************************/
    function confirmarExclusao(usuarioId, nome) {
        try {
            // [UI] Exibir modal de confirmação SweetAlert com detalhes
            Alerta.Confirmar(
                "Confirmar Exclusão",
                `Deseja realmente excluir o usuário <strong>${nome}</strong>?<br><br>` +
                `<small style="color: #dc3545; font-size: 0.875rem;">⚠️ Esta ação não pode ser desfeita!</small><br><br>` +
                `<small style="color: #6c757d; font-size: 0.875rem;">O sistema verificará automaticamente se o usuário possui registros vinculados (Viagens, Manutenções, etc.).</small>`,
                "Sim, Excluir",
                "Cancelar"
            ).then((willDelete) => {
                try {
                    // [LOGICA] Se usuário confirmou, executar exclusão
                    if (willDelete) {
                        excluirUsuario(usuarioId);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("usuario-index.js", "confirmarExclusao.then", error);
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "confirmarExclusao", error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: excluirUsuario
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Executar requisição POST para deletar usuário. Após sucesso,
     *                   recarregar DataTable. Se falhar, exibir mensagem de erro.
     *
     * 📥 ENTRADAS     : usuarioId [int] - ID do usuário a deletar
     *
     * 📤 SAÍDAS       : Toast de sucesso/erro, DataTable recarregada (se sucesso)
     *
     * 🔗 CHAMADA POR  : confirmarExclusao() após confirmação do usuário
     *
     * 🔄 CHAMA        : POST /api/Usuario/Delete, AppToast.show(), dataTableUsuarios.ajax.reload()
     *
     * 📝 OBSERVAÇÕES  :
     *   • Envia { Id: usuarioId } em JSON
     *   • Espera response.success antes de recarregar tabela
     *   • Recarrega tabela sem resetar paginação (null, false)
     ****************************************************************************************/
    function excluirUsuario(usuarioId) {
        try {
            // [AJAX] Endpoint: POST /api/Usuario/Delete
            // 📥 ENVIA    : { Id: usuarioId }
            // 📤 RECEBE   : { success, message }
            $.ajax({
                url: "/api/Usuario/Delete",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ Id: usuarioId }),
                success: function (response) {
                    try {
                        // [LOGICA] Verificar sucesso
                        if (response.success) {
                            // [UI] Toast verde
                            AppToast.show("Verde", response.message || "Usuário excluído!", 3000);
                            // [UI] Recarregar DataTable (sem resetar página)
                            dataTableUsuarios.ajax.reload(null, false);
                        } else {
                            // [UI] Toast vermelho com mensagem do servidor
                            AppToast.show("Vermelho", response.message || "Erro ao excluir", 5000);
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("usuario-index.js", "excluirUsuario.success", error);
                    }
                },
                error: function (xhr, status, error) {
                    // [DEBUG] Log erro
                    console.error("Erro ao excluir:", error);
                    // [UI] Toast vermelho genérico
                    AppToast.show("Vermelho", "Erro ao excluir usuário", 5000);
                }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("usuario-index.js", "excluirUsuario", error);
        }
    }

})();
