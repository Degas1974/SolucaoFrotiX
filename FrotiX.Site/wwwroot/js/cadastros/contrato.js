/* ****************************************************************************************
 * ⚡ ARQUIVO: contrato.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : CRUD de contratos com validação inteligente de dependências antes da
 *                   exclusão e gestão dinâmica de status (Ativo/Inativo) com bloqueio de
 *                   ações (botões Documentos/Itens/Repactuação) quando contrato inativo.
 * 📥 ENTRADAS     : DataTable #tblContrato com botões (.btn-delete, .updateStatusContrato),
 *                   GET /api/Contrato/VerificarDependencias?id (veículos, encarregados,
 *                   operadores, lavadores, motoristas, empenhos, notas fiscais vinculadas)
 * 📤 SAÍDAS       : Alerta.Warning detalhando dependências encontradas (bloqueia exclusão),
 *                   Alerta.Confirmar para exclusão segura, POST /api/Contrato/Delete,
 *                   atualização dinâmica de status com bloqueio/desbloqueio de botões
 * 🔗 CHAMADA POR  : Pages/Contrato/Index.cshtml, loadList() na inicialização
 * 🔄 CHAMA        : loadList() (inicialização DataTable), $.get() status, $.ajax() DELETE,
 *                   Alerta.Warning/Confirmar/TratamentoErroComLinha, AppToast.show,
 *                   window.ejTooltip.refresh() (atualização tooltips Syncfusion)
 * 📦 DEPENDÊNCIAS : jQuery, DataTables, Alerta.js (SweetAlert wrapper), AppToast.js,
 *                   Syncfusion EJ2 tooltips (ejTooltip)
 * 📝 OBSERVAÇÕES  : Sistema avançado de verificação de dependências (7 entidades verificadas:
 *                   veiculosContrato, encarregados, operadores, lavadores, motoristas,
 *                   empenhos, notasFiscais). Fallback se API falhar (confirmação sem validação).
 *                   Bloqueio dinâmico de botões quando inativo: adiciona classe .disabled,
 *                   remove hrefs, atualiza tooltips para "Bloqueado por Contrato Inativo".
 *                   Try-catch aninhado em todos os níveis (ready, click, ajax, success, error).
 *                   462 linhas de lógica de validação e gestão de status complexa.
 **************************************************************************************** */

var dataTable;

$(document).ready(function () {
    try {
        loadList();

        $(document).on("click", ".btn-delete", function () {
            try {
                // Ignora clique se o botão está desabilitado (possui dependências)
                if ($(this).hasClass('disabled')) {
                    return;
                }

                var id = $(this).data("id");
                console.log("Verificando dependências do contrato:", id);

                // Primeiro verifica se há dependências
                $.ajax({
                    url: "/api/Contrato/VerificarDependencias?id=" + id,
                    type: "GET",
                    dataType: "json",
                    success: function (result) {
                        try {
                            console.log("Resultado da verificação:", result);
                            
                            if (result.success && result.possuiDependencias) {
                                // Não pode excluir - mostrar mensagem com detalhes
                                var mensagem = "Este contrato não pode ser excluído pois possui:\n\n";
                                
                                if (result.veiculosContrato > 0) {
                                    mensagem += "• " + result.veiculosContrato + " veículo(s) associado(s)\n";
                                }
                                if (result.encarregados > 0) {
                                    mensagem += "• " + result.encarregados + " encarregado(s) vinculado(s)\n";
                                }
                                if (result.operadores > 0) {
                                    mensagem += "• " + result.operadores + " operador(es) vinculado(s)\n";
                                }
                                if (result.lavadores > 0) {
                                    mensagem += "• " + result.lavadores + " lavador(es) vinculado(s)\n";
                                }
                                if (result.motoristas > 0) {
                                    mensagem += "• " + result.motoristas + " motorista(s) vinculado(s)\n";
                                }
                                if (result.empenhos > 0) {
                                    mensagem += "• " + result.empenhos + " empenho(s) vinculado(s)\n";
                                }
                                if (result.notasFiscais > 0) {
                                    mensagem += "• " + result.notasFiscais + " nota(s) fiscal(is) vinculada(s)\n";
                                }
                                
                                mensagem += "\nRemova as associações antes de excluir o contrato.";
                                
                                Alerta.Warning("Exclusão não permitida", mensagem);
                            } else {
                                // Pode excluir - mostrar confirmação
                                Alerta.Confirmar(
                                    "Você tem certeza que deseja apagar este contrato?",
                                    "Não será possível recuperar os dados eliminados!",
                                    "Excluir",
                                    "Cancelar"
                                ).then((willDelete) => {
                                    try {
                                        if (willDelete) {
                                            var dataToPost = JSON.stringify({ ContratoId: id });
                                            var url = "/api/Contrato/Delete";
                                            $.ajax({
                                                url: url,
                                                type: "POST",
                                                data: dataToPost,
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function (data) {
                                                    try {
                                                        if (data.success) {
                                                            AppToast.show('Verde', data.message);
                                                            dataTable.ajax.reload();
                                                        } else {
                                                            AppToast.show('Vermelho', data.message);
                                                        }
                                                    } catch (error) {
                                                        Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.ajax.success", error);
                                                    }
                                                },
                                                error: function (err) {
                                                    try {
                                                        console.log(err);
                                                        AppToast.show('Vermelho', 'Erro ao excluir o contrato!');
                                                    } catch (error) {
                                                        Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.ajax.error", error);
                                                    }
                                                }
                                            });
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.confirm.then", error);
                                    }
                                });
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.verificar.success", error);
                        }
                    },
                    error: function (xhr, status, error) {
                        try {
                            console.log("Erro na API VerificarDependencias:", xhr.status, error);
                            console.log("Resposta:", xhr.responseText);
                            
                            // Se a API falhar, mostra confirmação normal (fallback)
                            Alerta.Confirmar(
                                "Você tem certeza que deseja apagar este contrato?",
                                "Não será possível recuperar os dados eliminados!",
                                "Excluir",
                                "Cancelar"
                            ).then((willDelete) => {
                                try {
                                    if (willDelete) {
                                        var dataToPost = JSON.stringify({ ContratoId: id });
                                        $.ajax({
                                            url: "/api/Contrato/Delete",
                                            type: "POST",
                                            data: dataToPost,
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (data) {
                                                if (data.success) {
                                                    AppToast.show('Verde', data.message);
                                                    dataTable.ajax.reload();
                                                } else {
                                                    AppToast.show('Vermelho', data.message);
                                                }
                                            },
                                            error: function () {
                                                AppToast.show('Vermelho', 'Erro ao excluir o contrato!');
                                            }
                                        });
                                    }
                                } catch (err) {
                                    Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.fallback.then", err);
                                }
                            });
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.verificar.error", error);
                        }
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("contrato.js", "btn-delete.click", error);
            }
        });

        $(document).on("click", ".updateStatusContrato", function () {
            try {
                var url = $(this).data("url");
                var currentElement = $(this);
                var row = currentElement.closest('tr');

                $.get(url, function (data) {
                    try {
                        if (data.success) {
                            AppToast.show('Verde', "Status alterado com sucesso!");

                            // Botões que devem ser bloqueados/desbloqueados (exceto Editar e Excluir)
                            var btnDocumentos = row.find('.btn-documentos');
                            var btnItens = row.find('.btn-itens');
                            var btnRepactuacao = row.find('.btn-repactuacao');
                            var botoesBloqueaveis = row.find('.btn-documentos, .btn-itens, .btn-repactuacao');
                            var tooltipBloqueado = 'Bloqueado por Contrato Inativo';

                            // Inverte o status baseado no estado atual do elemento
                            if (currentElement.hasClass("btn-verde")) {
                                // Era Ativo, agora é Inativo - BLOQUEAR botões
                                currentElement
                                    .removeClass("btn-verde")
                                    .addClass("fundo-cinza")
                                    .html('<i class="fa-duotone fa-circle-xmark"></i> Inativo');

                                // Bloquear botões (classe disabled aplica opacity via CSS)
                                botoesBloqueaveis.addClass('disabled');

                                // Atualizar tooltips para bloqueado
                                btnDocumentos.attr('data-ejtip', tooltipBloqueado);
                                btnItens.attr('data-ejtip', tooltipBloqueado);
                                btnRepactuacao.attr('data-ejtip', tooltipBloqueado);

                                // Remover href dos botões
                                btnItens.attr('href', 'javascript:void(0)');
                                btnRepactuacao.attr('href', 'javascript:void(0)');
                            } else {
                                // Era Inativo, agora é Ativo - DESBLOQUEAR botões
                                currentElement
                                    .removeClass("fundo-cinza")
                                    .addClass("btn-verde")
                                    .html('<i class="fa-duotone fa-circle-check"></i> Ativo');

                                // Desbloquear botões
                                botoesBloqueaveis.removeClass('disabled');

                                // Restaurar tooltips originais
                                btnDocumentos.attr('data-ejtip', 'Documentos do Contrato');
                                btnItens.attr('data-ejtip', 'Itens do Contrato');
                                btnRepactuacao.attr('data-ejtip', 'Adicionar Repactuação');

                                // Restaurar href dos botões
                                var contratoId = btnRepactuacao.data('id');
                                btnItens.attr('href', '/Contrato/ItensContrato?contratoId=' + contratoId);
                                btnRepactuacao.attr('href', '/Contrato/RepactuacaoContrato?id=' + contratoId);
                            }

                            // Atualizar tooltips Syncfusion após alteração dinâmica
                            if (window.ejTooltip) {
                                window.ejTooltip.refresh();
                            }
                        } else {
                            AppToast.show('Vermelho', 'Erro ao alterar status!');
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("contrato.js", "updateStatusContrato.get.success", error);
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("contrato.js", "updateStatusContrato.click", error);
            }
        });

    } catch (error) {
        Alerta.TratamentoErroComLinha("contrato.js", "document.ready", error);
    }
});

function loadList() {
    try {
        dataTable = $("#tblContrato").DataTable({
            ordering: false,
            // order: [[0, "desc"]],

            columnDefs: [
                {
                    targets: 0, // Contrato
                    className: "text-center",
                    width: "6%"
                },
                {
                    targets: 1, // Processo Completo
                    className: "text-center",
                    width: "7%"
                },
                {
                    targets: 2, // Objeto
                    className: "text-left",
                    width: "14%"
                },
                {
                    targets: 3, // Fornecedor
                    className: "text-left",
                    width: "14%"
                },
                {
                    targets: 4, // Vigência
                    className: "text-center",
                    width: "9%"
                },
                {
                    targets: 5, // Valor Anual
                    className: "text-right",
                    width: "8%"
                },
                {
                    targets: 6, // Valor Mensal
                    className: "text-right",
                    width: "8%"
                },
                {
                    targets: 7, // Prorrogação
                    className: "text-center",
                    width: "8%"
                },
                {
                    targets: 8, // Status
                    className: "text-center",
                    width: "6%"
                },
                {
                    targets: 9, // Ação
                    className: "text-center",
                    width: "12%",
                    orderable: false
                }
            ],

            responsive: true,
            ajax: {
                url: "/api/contrato",
                type: "GET",
                datatype: "json"
            },
            columns: [
                { data: "contratoCompleto" },
                { data: "processoCompleto" },
                { data: "objeto" },
                { data: "descricaoFornecedor" },
                { data: "periodo" },
                { data: "valorFormatado" },
                { data: "valorMensal" },
                { data: "vigenciaCompleta" },
                {
                    data: "status",
                    render: function (data, type, row, meta) {
                        try {
                            if (data) {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusContrato ftx-badge-status btn-verde" 
                                           data-url="/api/Contrato/updateStatusContrato?Id=${row.contratoId}">
                                           <i class="fa-duotone fa-circle-check"></i>
                                           Ativo
                                        </a>`;
                            } else {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusContrato ftx-badge-status fundo-cinza" 
                                           data-url="/api/Contrato/updateStatusContrato?Id=${row.contratoId}">
                                           <i class="fa-duotone fa-circle-xmark"></i>
                                           Inativo
                                        </a>`;
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("contrato.js", "render.status", error);
                        }
                    }
                },
                {
                    data: "contratoId",
                    render: function (data, type, row) {
                        try {
                            // Verifica se o contrato está inativo
                            var isInativo = !row.status;
                            var disabledClass = isInativo ? 'disabled' : '';
                            var tooltipBloqueado = 'Bloqueado por Contrato Inativo';

                            // Verifica dependências para o botão de excluir
                            var dependencias = [];
                            if (row.depVeiculos > 0) dependencias.push(' - ' + row.depVeiculos + ' veículo(s)');
                            if (row.depEncarregados > 0) dependencias.push(' - ' + row.depEncarregados + ' encarregado(s)');
                            if (row.depOperadores > 0) dependencias.push(' - ' + row.depOperadores + ' operador(es)');
                            if (row.depLavadores > 0) dependencias.push(' - ' + row.depLavadores + ' lavador(es)');
                            if (row.depMotoristas > 0) dependencias.push(' - ' + row.depMotoristas + ' motorista(s)');
                            if (row.depEmpenhos > 0) dependencias.push(' - ' + row.depEmpenhos + ' empenho(s)');
                            if (row.depNotasFiscais > 0) dependencias.push(' - ' + row.depNotasFiscais + ' nota(s) fiscal(is)');
                            if (row.depRepactuacoes > 0) dependencias.push(' - ' + row.depRepactuacoes + ' repactuação(ões)');
                            if (row.depItensContrato > 0) dependencias.push(' - ' + row.depItensContrato + ' item(ns) de contrato');

                            var possuiDependencias = dependencias.length > 0;
                            var disabledExcluir = possuiDependencias ? 'disabled' : '';

                            // Se tem dependências, usa Bootstrap Tooltip com HTML para quebra de linha
                            // Senão, usa Syncfusion (data-ejtip)
                            var tooltipExcluirHtml = possuiDependencias
                                ? 'Exclusão bloqueada:<br>' + dependencias.join('<br>')
                                : '';
                            var tooltipExcluirAttr = possuiDependencias
                                ? `data-bs-toggle="tooltip" data-bs-html="true" title="${tooltipExcluirHtml}"`
                                : 'data-ejtip="Excluir Contrato"';

                            return `<div class="ftx-actions" data-contrato-id="${data}">
                                        <a href="/Contrato/Upsert?id=${data}"
                                           class="btn btn-azul btn-icon-28"
                                           data-ejtip="Editar Contrato"
                                           style="cursor:pointer;">
                                            <i class="fa-duotone fa-pen-to-square"></i>
                                        </a>
                                        <a href="javascript:void(0)"
                                           class="btn btn-delete btn-vinho btn-icon-28 ${disabledExcluir}"
                                           ${tooltipExcluirAttr}
                                           style="cursor:pointer;"
                                           data-id="${data}">
                                            <i class="fa-duotone fa-trash-can"></i>
                                        </a>
                                        <a href="javascript:void(0)"
                                           class="btn btn-documentos btn-info btn-icon-28 ${disabledClass}"
                                           data-ejtip="${isInativo ? tooltipBloqueado : 'Documentos do Contrato'}"
                                           style="cursor:pointer;"
                                           data-id="${data}">
                                            <i class="fa-duotone fa-file-pdf"></i>
                                        </a>
                                        <a href="${isInativo ? 'javascript:void(0)' : '/Contrato/ItensContrato?contratoId=' + data}"
                                           class="btn btn-itens fundo-cinza btn-icon-28 ${disabledClass}"
                                           data-ejtip="${isInativo ? tooltipBloqueado : 'Itens do Contrato'}"
                                           style="cursor:pointer;"
                                           data-id="${data}">
                                            <i class="fa-duotone fa-sitemap"></i>
                                        </a>
                                        <a href="${isInativo ? 'javascript:void(0)' : '/Contrato/RepactuacaoContrato?id=' + data}"
                                           class="btn btn-repactuacao fundo-chocolate btn-icon-28 ${disabledClass}"
                                           data-ejtip="${isInativo ? tooltipBloqueado : 'Adicionar Repactuação'}"
                                           style="cursor:pointer;"
                                           data-id="${data}">
                                            <i class="fa-duotone fa-handshake"></i>
                                        </a>
                                    </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("contrato.js", "render.actions", error);
                        }
                    }
                }
            ],

            language: {
                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                emptyTable: "Sem Dados para Exibição"
            },
            width: "100%",

            // Reinicializa tooltips após cada renderização do DataTable
            drawCallback: function() {
                try {
                    // Syncfusion tooltips (data-ejtip)
                    if (window.ejTooltip) {
                        window.ejTooltip.refresh();
                    }

                    // Bootstrap tooltips para botões com dependências (estilo Syncfusion)
                    var tooltipTriggerList = document.querySelectorAll('#tblContrato [data-bs-toggle="tooltip"]');
                    tooltipTriggerList.forEach(function(tooltipTriggerEl) {
                        // Destrói tooltip existente se houver
                        var existingTooltip = bootstrap.Tooltip.getInstance(tooltipTriggerEl);
                        if (existingTooltip) {
                            existingTooltip.dispose();
                        }
                        // Cria novo tooltip com estilo Syncfusion (apenas hover, sem focus/click)
                        new bootstrap.Tooltip(tooltipTriggerEl, {
                            customClass: 'tooltip-ftx-syncfusion',
                            trigger: 'hover'
                        });
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha("contrato.js", "drawCallback", error);
                }
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("contrato.js", "loadList", error);
    }
}
