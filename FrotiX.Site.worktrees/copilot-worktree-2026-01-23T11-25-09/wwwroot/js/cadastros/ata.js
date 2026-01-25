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

                // Verifica dependências antes de confirmar exclusão
                $.ajax({
                    url: "/api/AtaRegistroPrecos/VerificarDependencias?id=" + id,
                    type: "GET",
                    dataType: "json",
                    success: function (result) {
                        try {
                            if (result.success && result.possuiDependencias) {
                                var mensagem = "Esta ata não pode ser excluída pois possui:\n\n";
                                if (result.itens > 0) mensagem += "• " + result.itens + " item(ns) vinculado(s)\n";
                                if (result.veiculos > 0) mensagem += "• " + result.veiculos + " veículo(s) associado(s)\n";
                                
                                mensagem += "\nRemova as associações antes de excluir a ata.";
                                Alerta.Warning("Exclusão não permitida", mensagem);
                            } else {
                                // Pode excluir
                                Alerta.Confirmar(
                                    "Você tem certeza que deseja apagar?",
                                    "Você não será capaz de restaurar os dados!",
                                    "Sim, apague!",
                                    "Não, cancele!"
                                ).then((willDelete) => {
                                    if (willDelete) {
                                        var dataToPost = JSON.stringify({ AtaId: id });
                                        $.ajax({
                                            url: "api/AtaRegistroPrecos/Delete",
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
                                            }
                                        });
                                    }
                                });
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ata.js", "btn-delete.ajax.success", error);
                        }
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("ata.js", "btn-delete.click", error);
            }
        });

        // Handler para mudança de status (Ativo/Inativo)
        $(document).on("click", ".updateStatusAta", function () {
            try {
                var url = $(this).data("url");
                var currentElement = $(this);
                
                $.post(url, function (data) {
                    try {
                        if (data.success) {
                            AppToast.show('Verde', data.message);
                            dataTable.ajax.reload(); // Recarrega para atualizar estado dos botões
                        } else {
                            AppToast.show('Vermelho', 'Erro ao alterar status!');
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("ata.js", "updateStatusAta.post.success", error);
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("ata.js", "updateStatusAta.click", error);
            }
        });

    } catch (error) {
        Alerta.TratamentoErroComLinha("ata.js", "document.ready", error);
    }
});

function loadList() {
    try {
        dataTable = $("#tblAta").DataTable({
            "order": [[0, "desc"]],
            "responsive": true,
            "ajax": {
                "url": "api/AtaRegistroPrecos",
                "type": "GET",
                "datatype": "json"
            },
            "columns": [
                { "data": "ataCompleta", "width": "10%" },
                { "data": "processoCompleto", "width": "10%" },
                { "data": "objeto", "width": "20%" },
                { "data": "descricaoFornecedor", "width": "20%" },
                { "data": "periodo", "width": "10%" },
                { "data": "valorFormatado", "width": "10%" },
                {
                    "data": "status",
                    "render": function (data, type, row) {
                        try {
                            if (data) {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusAta ftx-badge-status btn-verde" 
                                           data-url="api/AtaRegistroPrecos/UpdateStatusAta?Id=${row.ataId}">
                                           <i class="fa-duotone fa-circle-check"></i> Ativo
                                        </a>`;
                            } else {
                                return `<a href="javascript:void(0)" 
                                           class="updateStatusAta ftx-badge-status fundo-cinza" 
                                           data-url="api/AtaRegistroPrecos/UpdateStatusAta?Id=${row.ataId}">
                                           <i class="fa-duotone fa-circle-xmark"></i> Inativo
                                        </a>`;
                            }
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ata.js", "render.status", error);
                        }
                    }, "width": "10%"
                },
                {
                    "data": "ataId",
                    "render": function (data, type, row) {
                        try {
                            // Verifica dependências para o botão de excluir
                            var dependencias = [];
                            if (row.depItens > 0) dependencias.push(' - ' + row.depItens + ' item(ns) vinculado(s)');
                            if (row.depVeiculos > 0) dependencias.push(' - ' + row.depVeiculos + ' veículo(s) associado(s)');

                            var possuiDependencias = dependencias.length > 0;
                            var disabledExcluir = possuiDependencias ? 'disabled' : '';

                            // Tooltip HTML para dependências
                            var tooltipExcluirHtml = possuiDependencias
                                ? 'Exclusão bloqueada:<br>' + dependencias.join('<br>')
                                : '';
                            
                            var tooltipExcluirAttr = possuiDependencias
                                ? `data-bs-toggle="tooltip" data-bs-html="true" title="${tooltipExcluirHtml}"`
                                : 'data-ejtip="Excluir Ata"';

                            return `<div class="ftx-actions">
                                        <a href="/AtaRegistroPrecos/Upsert?id=${data}" 
                                           class="btn btn-azul btn-icon-28" 
                                           data-ejtip="Editar Ata">
                                            <i class="fa-duotone fa-pen-to-square"></i>
                                        </a>
                                        <a href="javascript:void(0)" 
                                           class="btn btn-delete btn-vinho btn-icon-28 ${disabledExcluir}" 
                                           ${tooltipExcluirAttr}
                                           data-id="${data}">
                                            <i class="fa-duotone fa-trash-can"></i>
                                        </a>
                                    </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ata.js", "render.actions", error);
                        }
                    }, "width": "10%"
                }
            ],
            "language": {
                "url": "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
                "emptyTable": "Nenhum registro encontrado"
            },
            "width": "100%",

            // Inicializa tooltips após desenhar a tabela
            drawCallback: function() {
                try {
                    // Syncfusion tooltips (data-ejtip)
                    if (window.ejTooltip) {
                        window.ejTooltip.refresh();
                    }

                    // Bootstrap tooltips para botões com dependências (estilo Syncfusion)
                    var tooltipTriggerList = document.querySelectorAll('#tblAta [data-bs-toggle="tooltip"]');
                    tooltipTriggerList.forEach(function(tooltipTriggerEl) {
                        // Destrói tooltip existente se houver para evitar duplicatas
                        var existingTooltip = bootstrap.Tooltip.getInstance(tooltipTriggerEl);
                        if (existingTooltip) {
                            existingTooltip.dispose();
                        }
                        // Cria novo tooltip
                        new bootstrap.Tooltip(tooltipTriggerEl, {
                            customClass: 'tooltip-ftx-syncfusion',
                            trigger: 'hover'
                        });
                    });
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ata.js", "drawCallback", error);
                }
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ata.js", "loadList", error);
    }
}