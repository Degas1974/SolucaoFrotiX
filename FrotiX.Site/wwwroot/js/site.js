/* ****************************************************************************************
 * ⚡ ARQUIVO: site.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Utilitários gerais do site (modals AJAX via jQuery, DataTable com
 *                   altEditor, shortcut CTRL+Q para menu Apps). Funções auxiliares
 *                   para operações CRUD via modals Bootstrap.
 * 📥 ENTRADAS     : URLs de endpoints, forms HTML, eventos jQuery (click, keydown)
 * 📤 SAÍDAS       : Modals Bootstrap populados, DataTables configuradas, CRUD via AJAX
 * 🔗 CHAMADA POR  : Páginas que usam modals AJAX (ex: #form-modal), DataTables editáveis
 * 🔄 CHAMA        : Endpoints GET/POST via AJAX, Bootstrap modals, DataTables API
 * 📦 DEPENDÊNCIAS : jQuery, Bootstrap 5, DataTables, altEditor plugin
 * 📝 OBSERVAÇÕES  : Funções globais (jQueryModalGet, jQueryModalPost, jQueryModalDelete)
 *                   são expostas no escopo window para uso em atributos onclick
 *
 * 📋 ÍNDICE DE FUNÇÕES (5 funções principais):
 *
 * ┌─ FUNÇÕES DE MODAL AJAX ────────────────────────────────────────────────────────┐
 * │ 1. jQueryModalGet(url, title)                                                   │
 * │    → GET AJAX para carregar conteúdo em modal                                  │
 * │    → Popula #form-modal .modal-body com HTML retornado                         │
 * │    → Define título e mostra modal                                              │
 * │                                                                                 │
 * │ 2. jQueryModalPost(form)                                                        │
 * │    → POST AJAX para submeter formulário de modal                               │
 * │    → Se res.isValid → atualiza #viewAll e fecha modal                          │
 * │    → Usa FormData para suportar upload de arquivos                             │
 * │                                                                                 │
 * │ 3. jQueryModalDelete(form)                                                      │
 * │    → DELETE via POST com confirmação (confirm dialog)                          │
 * │    → Atualiza #viewAll com HTML retornado                                      │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ PLUGIN JQUERY ────────────────────────────────────────────────────────────────┐
 * │ 4. $.fn.DataTableEdit(options)                                                  │
 * │    → Plugin jQuery para DataTable com botões CRUD                              │
 * │    → Configuração: altEditor, serverSide, responsive                           │
 * │    → Botões: Excluir, Editar, Adicionar, Synchronize                           │
 * │    → Suporta filtros via span[data-role=filter] com data-filter                │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ SHORTCUTS ────────────────────────────────────────────────────────────────────┐
 * │ 5. CTRL+Q Handler                                                               │
 * │    → Atalho de teclado CTRL+Q                                                  │
 * │    → Abre menu Apps (trigger click em a[title*=Apps])                          │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔗 AJAX ENDPOINTS CHAMADOS:
 * - GET {url} → retorna { html: string }
 * - POST {form.action} → retorna { isValid: bool, html: string }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Funções jQueryModal* são globais (window.jQueryModalGet, etc.)
 * - DataTableEdit é plugin jQuery ($.fn.DataTableEdit)
 * - Usa Bootstrap 5 modals (#form-modal)
 **************************************************************************************** */

$(document).ready(function () {
    /****************************************************************************************
     * ⚡ FUNÇÃO: jQueryModalGet
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Carregar conteúdo via GET AJAX e exibir em modal Bootstrap
     *
     * 📥 ENTRADAS     : url [string] - Endpoint para carregar HTML
     *                   title [string] - Título do modal
     *
     * 📤 SAÍDAS       : false (sempre, para prevenir navegação em onclick)
     *
     * ⬅️ CHAMADO POR  : onclick em links/botões com jQueryModalGet(url, title)
     *
     * ➡️ CHAMA        : GET {url} [AJAX]
     *                   Modal Bootstrap $('#form-modal').modal('show')
     ****************************************************************************************/
    jQueryModalGet = (url, title) => {
        try {
            /********************************************************************************
             * [AJAX] Endpoint: GET {url}
             * ------------------------------------------------------------------------------
             * 📥 ENVIA        : Nenhum parâmetro (GET puro)
             * 📤 RECEBE       : { html: string } - Conteúdo HTML para popular modal
             * 🎯 MOTIVO       : Carregar formulário ou conteúdo editável em modal Bootstrap
             ********************************************************************************/
            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                processData: false,
                success: function (res) {
                    // [UI] Popula modal body com HTML recebido
                    $('#form-modal .modal-body').html(res.html);
                    // [UI] Define título do modal
                    $('#form-modal .modal-title').html(title);
                    // [UI] Exibe modal Bootstrap
                    $('#form-modal').modal('show');
                },
                error: function (err) {
                    console.error('Erro em AJAX GET:', err);
                    Alerta.TratamentoErroComLinha('site.js', 'jQueryModalGet.ajax.error', err);
                }
            })
            return false;
        } catch (erro) {
            console.error('Erro em jQueryModalGet:', erro);
            Alerta.TratamentoErroComLinha('site.js', 'jQueryModalGet', erro);
            return false;
        }
    }
    /****************************************************************************************
     * ⚡ FUNÇÃO: jQueryModalPost
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Submeter formulário via POST AJAX com suporte a file upload
     *
     * 📥 ENTRADAS     : form [HTMLFormElement] - Elemento form do modal
     *
     * 📤 SAÍDAS       : false (sempre, para prevenir submissão tradicional)
     *
     * ⬅️ CHAMADO POR  : onclick em botão de salvar do modal
     *
     * ➡️ CHAMA        : POST {form.action} [AJAX]
     *                   Atualiza DOM (#viewAll, #form-modal)
     ****************************************************************************************/
    jQueryModalPost = form => {
        try {
            /********************************************************************************
             * [AJAX] Endpoint: POST {form.action}
             * ------------------------------------------------------------------------------
             * 📥 ENVIA        : FormData (suporta arquivos e campos normais)
             * 📤 RECEBE       : { isValid: bool, html: string }
             * 🎯 MOTIVO       : Validar e salvar dados do formulário, retornar HTML atualizado
             ********************************************************************************/
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    // [VALIDACAO] Verificar se dados foram válidos no servidor
                    if (res.isValid) {
                        // [UI] Atualiza listagem com HTML retornado
                        $('#viewAll').html(res.html)
                        // [UI] Fecha modal após salvar com sucesso
                        $('#form-modal').hide();
                    }
                },
                error: function (err) {
                    console.error('Erro em AJAX POST:', err);
                    Alerta.TratamentoErroComLinha('site.js', 'jQueryModalPost.ajax.error', err);
                }
            })
            return false;
        } catch (erro) {
            console.error('Erro em jQueryModalPost:', erro);
            Alerta.TratamentoErroComLinha('site.js', 'jQueryModalPost', erro);
            return false;
        }
    }
    /****************************************************************************************
     * ⚡ FUNÇÃO: jQueryModalDelete
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Deletar registro após confirmação via POST AJAX
     *
     * 📥 ENTRADAS     : form [HTMLFormElement] - Elemento form com dados do registro
     *
     * 📤 SAÍDAS       : false (sempre, para prevenir submissão tradicional)
     *
     * ⬅️ CHAMADO POR  : onclick em botão de deletar do modal
     *
     * ➡️ CHAMA        : POST {form.action} [AJAX] (com confirmação)
     *                   Atualiza DOM (#viewAll)
     ****************************************************************************************/
    jQueryModalDelete = form => {
        try {
            // [VALIDACAO] Confirmação do usuário para ação irreversível
            if (confirm('Are you sure to delete this record ?')) {
                /********************************************************************************
                 * [AJAX] Endpoint: POST {form.action} (DELETE semântico)
                 * ------------------------------------------------------------------------------
                 * 📥 ENVIA        : FormData com ID do registro
                 * 📤 RECEBE       : { html: string } - Listagem atualizada
                 * 🎯 MOTIVO       : Deletar registro e retornar listagem sem o item removido
                 ********************************************************************************/
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        // [UI] Atualiza listagem com HTML retornado (sem o registro deletado)
                        $('#viewAll').html(res.html);
                    },
                    error: function (err) {
                        console.error('Erro em AJAX DELETE:', err);
                        Alerta.TratamentoErroComLinha('site.js', 'jQueryModalDelete.ajax.error', err);
                    }
                })
            }
            return false;
        } catch (erro) {
            console.error('Erro em jQueryModalDelete:', erro);
            Alerta.TratamentoErroComLinha('site.js', 'jQueryModalDelete', erro);
            return false;
        }
    }
});






(function ($) {

    /****************************************************************************************
     * ⚡ EVENTO GLOBAL: keydown (CTRL+Q shortcut)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Registrar handler global para atalho de teclado CTRL+Q
     *
     * 📥 ENTRADAS     : Evento de teclado do navegador (event.ctrlKey, event.which)
     *
     * 📤 SAÍDAS       : Trigger click em a[title*=Apps] (abre menu de aplicativos)
     *
     * ⬅️ CHAMADO POR  : Automaticamente ao pressionar CTRL+Q em qualquer página
     ****************************************************************************************/
    /* Trigger app shortcut menu on CTRL+Q press */
    $(document).keydown(function (event) {
        try {
            // [LOGICA] Verifica se CTRL + Q foram pressionados (qual = 81 é Q)
            if (event.ctrlKey && event.which === 81)
                // [UI] Simula clique no menu Apps via atributo title
                $("a[title*=Apps]").trigger("click");
        } catch (erro) {
            console.error('Erro em keydown handler:', erro);
            Alerta.TratamentoErroComLinha('site.js', 'keydown.CTRL+Q', erro);
        }
    });

    /****************************************************************************************
     * ⚡ FUNÇÃO: $.fn.DataTableEdit (Plugin jQuery)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Inicializar DataTable com suporte a edição inline (altEditor)
     *                   e botões CRUD (Excluir, Editar, Adicionar, Sincronizar)
     *
     * 📥 ENTRADAS     : $options [object] - Configurações customizadas para DataTable
     *
     * 📤 SAÍDAS       : DataTable instance com .on('init.dt') handler
     *
     * ⬅️ CHAMADO POR  : $('table').DataTableEdit({...}) em páginas com listagens
     *
     * ➡️ CHAMA        : DataTable API (filtering, drawing, responsive)
     *                   Event handlers (click em span[data-role=filter])
     ****************************************************************************************/
    /* Initialize basic datatable */
    $.fn.DataTableEdit = function ($options) {
        try {
            // [LOGICA] Mescla configurações padrão com opções customizadas
            var options = $.extend({
                // [UI] DOM layout: filtro (esquerda) + botões (direita)
                dom: "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                responsive: true,
                serverSide: true,
                altEditor: true,  // [UI] Habilita edição inline
                pageLength: 10,
                select: { style: "single" },
                // [UI] Botões de ação: Excluir, Editar, Adicionar, Sincronizar
                buttons: [
                    {
                        extend: 'selected',
                        text: '<i class="fal fa-times mr-1"></i> Excluir',
                        name: 'delete',
                        className: 'btn-vinho btn-sm mr-1'
                    },
                    {
                        extend: 'selected',
                        text: '<i class="fal fa-edit mr-1"></i> Editar',
                        name: 'edit',
                        className: 'btn-warning btn-sm mr-1'
                    },
                    {
                        text: '<i class="fal fa-plus mr-1"></i> Adicionar',
                        name: 'add',
                        className: 'btn-info btn-sm mr-1'
                    },
                    {
                        text: '<i class="fal fa-sync mr-1"></i> Synchronize',
                        name: 'refresh',
                        className: 'btn-azul btn-sm'
                    }
                ]
            }, $options);

            return $(this).DataTable(options).on('init.dt', function () {
                try {
                    // [EVENT] Handler para cliques em filtros rápidos (span[data-role=filter])
                    $("span[data-role=filter]").off().on("click", function () {
                        try {
                            // [LOGICA] Extrai termo de filtro e aplica busca na tabela
                            const search = $(this).data("filter");
                            if (table)
                                table.search(search).draw();
                        } catch (erro) {
                            console.error('Erro em filter click:', erro);
                            Alerta.TratamentoErroComLinha('site.js', 'DataTableEdit.filter.click', erro);
                        }
                    });
                } catch (erro) {
                    console.error('Erro em init.dt:', erro);
                    Alerta.TratamentoErroComLinha('site.js', 'DataTableEdit.init.dt', erro);
                }
            });
        } catch (erro) {
            console.error('Erro em DataTableEdit:', erro);
            Alerta.TratamentoErroComLinha('site.js', 'DataTableEdit', erro);
            return null;
        }
    };
}(jQuery));