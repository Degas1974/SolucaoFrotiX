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
    jQueryModalGet = (url, title) => {
        try {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#form-modal .modal-body').html(res.html);
                    $('#form-modal .modal-title').html(title);
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
    jQueryModalPost = form => {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $('#viewAll').html(res.html)
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
    jQueryModalDelete = form => {
        try {
            if (confirm('Are you sure to delete this record ?')) {
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
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

    /* Trigger app shortcut menu on CTRL+Q press */
    $(document).keydown(function (event) {
        try {
            // CTRL + Q
            if (event.ctrlKey && event.which === 81)
                $("a[title*=Apps]").trigger("click");
        } catch (erro) {
            console.error('Erro em keydown handler:', erro);
            Alerta.TratamentoErroComLinha('site.js', 'keydown.CTRL+Q', erro);
        }
    });

    /* Initialize basic datatable */
    $.fn.DataTableEdit = function ($options) {
        try {
            var options = $.extend({
                dom: "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                responsive: true,
                serverSide: true,
                altEditor: true,
                pageLength: 10,
                select: { style: "single" },
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
                    $("span[data-role=filter]").off().on("click", function () {
                        try {
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