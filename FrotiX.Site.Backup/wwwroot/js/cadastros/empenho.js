/*
 * ╔══════════════════════════════════════════════════════════════════════════════════╗
 * ║                         SOLUÇÃO FROTIX - GESTÃO DE FROTAS                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: empenho.js                                                           ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                                  ║
 * ║ 📋 VERSÃO: 1.1                                                                   ║
 * ║ 📅 ATUALIZAÇÃO: 22/01/2026                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                               ║
 * ║    Este script gerencia a interface de EMPENHOS orçamentários, permitindo:       ║
 * ║    • Exclusão de empenhos com confirmação via Alerta.Confirmar                   ║
 * ║    • Alteração de status (Ativo/Inativo)                                         ║
 * ║    • Gerenciamento de Notas Fiscais vinculadas aos empenhos                      ║
 * ║    • Formatação de valores monetários e datas                                    ║
 * ║    • Handlers delegados para DataTables dinâmicos                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 🔗 DEPENDÊNCIAS:                                                                 ║
 * ║    • jQuery 3.x (manipulação DOM e AJAX)                                         ║
 * ║    • DataTables.js (tabelas de empenhos e notas fiscais)                         ║
 * ║    • FrotiX Alerta (SweetAlert customizado para alertas e confirmações)          ║
 * ║    • AppToast (notificações toast do FrotiX)                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 📡 ENDPOINTS CONSUMIDOS:                                                         ║
 * ║    POST /api/Empenho/Delete - Exclui um empenho                                  ║
 * ║    GET  /api/Empenho/UpdateStatus - Alterna status do empenho                    ║
 * ║    POST /api/NotaFiscal/Delete - Exclui uma nota fiscal vinculada                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════╣
 * ║ 🎯 PADRÃO FROTIX:                                                                ║
 * ║    • Try-Catch com Alerta.TratamentoErroComLinha em TODAS as funções             ║
 * ║    • Confirmações via Alerta.Confirmar (nunca confirm() nativo)                  ║
 * ║    • Notificações via AppToast (Verde/Vermelho)                                  ║
 * ╚══════════════════════════════════════════════════════════════════════════════════╝
 */

/* ═══════════════════════════════════════════════════════════════════════════════════
   VARIÁVEIS GLOBAIS
   ═══════════════════════════════════════════════════════════════════════════════════ */

/** @type {DataTable} Referência global à instância DataTable de empenhos */
var dataTable;

/* ═══════════════════════════════════════════════════════════════════════════════════
   INICIALIZAÇÃO DO DOCUMENTO
   ═══════════════════════════════════════════════════════════════════════════════════ */

$(document).ready(function () {
    try {
        // ─────────────────────────────────────────────────────────────────────────────
        // NOTA: A lógica principal de inicialização está no Index.cshtml
        // Este arquivo contém apenas funções auxiliares e handlers delegados
        // O uso de delegação ($(document).on) permite que os handlers funcionem
        // mesmo quando as linhas da DataTable são recriadas dinamicamente
        // ─────────────────────────────────────────────────────────────────────────────

        /* ═══════════════════════════════════════════════════════════════════════════
           HANDLER: EXCLUSÃO DE EMPENHO
           Botão: .btn-delete com data-id={EmpenhoId}
           Fluxo: Confirmar → AJAX POST → Feedback → Reload DataTable
           ═══════════════════════════════════════════════════════════════════════════ */
        $(document).on('click', '.btn-delete', function () {
            try {
                var id = $(this).data('id');

                Alerta.Confirmar(
                    'Você tem certeza que deseja apagar este empenho?',
                    'Não será possível recuperar os dados eliminados!',
                    'Excluir',
                    'Cancelar',
                ).then((willDelete) => {
                    try {
                        if (willDelete) {
                            var dataToPost = JSON.stringify({ EmpenhoId: id });
                            var url = '/api/Empenho/Delete';

                            $.ajax({
                                url: url,
                                type: 'POST',
                                data: dataToPost,
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    try {
                                        if (data.success) {
                                            AppToast.show(
                                                'Verde',
                                                data.message,
                                            );
                                            $('#tblEmpenho')
                                                .DataTable()
                                                .ajax.reload(null, false);
                                        } else {
                                            AppToast.show(
                                                'Vermelho',
                                                data.message,
                                            );
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'empenho.js',
                                            'btn-delete.ajax.success',
                                            error,
                                        );
                                    }
                                },
                                error: function (err) {
                                    try {
                                        console.error(
                                            'Erro ao excluir empenho:',
                                            err,
                                        );
                                        AppToast.show(
                                            'Vermelho',
                                            'Erro ao excluir o empenho. Tente novamente.',
                                        );
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'empenho.js',
                                            'btn-delete.ajax.error',
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'empenho.js',
                            'btn-delete.swal.then',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'empenho.js',
                    'btn-delete.click',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           HANDLER: ALTERAÇÃO DE STATUS DO EMPENHO
           Botão: .updateStatusEmpenho com data-url={urlCompleta}
           Fluxo: GET → Toggle visual → Feedback
           ═══════════════════════════════════════════════════════════════════════════ */
        $(document).on('click', '.updateStatusEmpenho', function () {
            try {
                var url = $(this).data('url');
                var currentElement = $(this);

                $.get(url, function (data) {
                    try {
                        if (data.success) {
                            AppToast.show(
                                'Verde',
                                'Status alterado com sucesso!',
                            );
                            var text = 'Ativo';

                            if (data.type == 1) {
                                text = 'Inativo';
                                currentElement
                                    .removeClass('btn-verde')
                                    .addClass('fundo-cinza');
                            } else {
                                currentElement
                                    .removeClass('fundo-cinza')
                                    .addClass('btn-verde');
                            }

                            currentElement.text(text);
                        } else {
                            AppToast.show(
                                'Vermelho',
                                'Erro ao alterar o status.',
                            );
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'empenho.js',
                            'updateStatusEmpenho.get.success',
                            error,
                        );
                    }
                }).fail(function (err) {
                    try {
                        console.error('Erro ao alterar status:', err);
                        AppToast.show(
                            'Vermelho',
                            'Erro ao alterar o status. Tente novamente.',
                        );
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'empenho.js',
                            'updateStatusEmpenho.get.fail',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'empenho.js',
                    'updateStatusEmpenho.click',
                    error,
                );
            }
        });

        /* ═══════════════════════════════════════════════════════════════════════════
           HANDLER: EXCLUSÃO DE NOTA FISCAL
           Botão: .btn-delete-nf com data-id={NotaFiscalId}
           Fluxo: Confirmar → AJAX POST → Feedback → Reload DataTables (NF + Empenho)
           NOTA: Recarrega também tblEmpenho para atualizar saldos calculados
           ═══════════════════════════════════════════════════════════════════════════ */
        $(document).on('click', '.btn-delete-nf', function () {
            try {
                var id = $(this).data('id');

                Alerta.Confirmar(
                    'Você tem certeza que deseja apagar esta Nota Fiscal?',
                    'Não será possível recuperar os dados eliminados!',
                    'Excluir',
                    'Cancelar',
                ).then((willDelete) => {
                    try {
                        if (willDelete) {
                            $.ajax({
                                url: '/api/NotaFiscal/Delete',
                                type: 'POST',
                                data: JSON.stringify({ NotaFiscalId: id }),
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    try {
                                        if (data.success) {
                                            AppToast.show(
                                                'Verde',
                                                data.message,
                                            );
                                            $('#tblNotaFiscal')
                                                .DataTable()
                                                .ajax.reload(null, false);
                                            // Também recarregar a tabela de empenhos para atualizar saldos
                                            if (
                                                $.fn.DataTable.isDataTable(
                                                    '#tblEmpenho',
                                                )
                                            ) {
                                                $('#tblEmpenho')
                                                    .DataTable()
                                                    .ajax.reload(null, false);
                                            }
                                        } else {
                                            AppToast.show(
                                                'Vermelho',
                                                data.message,
                                            );
                                        }
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'empenho.js',
                                            'btn-delete-nf.ajax.success',
                                            error,
                                        );
                                    }
                                },
                                error: function (err) {
                                    try {
                                        console.error(
                                            'Erro ao excluir nota fiscal:',
                                            err,
                                        );
                                        AppToast.show(
                                            'Vermelho',
                                            'Erro ao excluir a nota fiscal. Tente novamente.',
                                        );
                                    } catch (error) {
                                        Alerta.TratamentoErroComLinha(
                                            'empenho.js',
                                            'btn-delete-nf.ajax.error',
                                            error,
                                        );
                                    }
                                },
                            });
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha(
                            'empenho.js',
                            'btn-delete-nf.swal.then',
                            error,
                        );
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'empenho.js',
                    'btn-delete-nf.click',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('empenho.js', 'document.ready', error);
    }
});

/* ═══════════════════════════════════════════════════════════════════════════════════
   FUNÇÕES UTILITÁRIAS
   ═══════════════════════════════════════════════════════════════════════════════════ */

/**
 * Formata valor numérico para moeda brasileira
 * Utiliza toLocaleString para formatação nativa do navegador
 *
 * @param {number} valor - Valor numérico a ser formatado
 * @returns {string} - Valor formatado no padrão brasileiro (ex: "R$ 1.234,56")
 *
 * @example
 * formatarMoeda(1234.56) // Retorna: "R$ 1.234,56"
 * formatarMoeda(null)    // Retorna: "R$ 0,00"
 */
function formatarMoeda(valor) {
    try {
        if (valor === null || valor === undefined) return 'R$ 0,00';
        return valor.toLocaleString('pt-BR', {
            style: 'currency',
            currency: 'BRL',
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('empenho.js', 'formatarMoeda', error);
        return 'R$ 0,00';
    }
}

/**
 * Converte string de moeda brasileira para número
 * Remove formatação brasileira (pontos de milhar, vírgula decimal, símbolo R$)
 * e retorna valor numérico para cálculos
 *
 * @param {string} valor - String no formato brasileiro (ex: "R$ 1.234,56" ou "1.234,56")
 * @returns {number} - Valor numérico (ex: 1234.56)
 *
 * @example
 * moedaParaNumero("R$ 1.234,56") // Retorna: 1234.56
 * moedaParaNumero("1.234,56")    // Retorna: 1234.56
 * moedaParaNumero("")            // Retorna: 0
 */
function moedaParaNumero(valor) {
    try {
        if (!valor) return 0;
        return parseFloat(
            String(valor)
                .replace(/\s/g, '')
                .replace(/\./g, '')
                .replace(',', '.')
                .replace('R$', '')
                .replace('&nbsp;', ''),
        );
    } catch (error) {
        Alerta.TratamentoErroComLinha('empenho.js', 'moedaParaNumero', error);
        return 0;
    }
}

/**
 * Formata data para o padrão brasileiro (DD/MM/YYYY)
 * Aceita strings ISO ou objetos Date
 *
 * @param {string|Date} data - Data a ser formatada (ISO string ou Date object)
 * @returns {string} - Data formatada no padrão brasileiro ou string vazia se inválida
 *
 * @example
 * formatarData("2026-01-22")          // Retorna: "22/01/2026"
 * formatarData(new Date())            // Retorna: data atual formatada
 * formatarData(null)                  // Retorna: ""
 */
function formatarData(data) {
    try {
        if (!data) return '';
        const d = new Date(data);
        if (isNaN(d.getTime())) return '';
        return d.toLocaleDateString('pt-BR');
    } catch (error) {
        Alerta.TratamentoErroComLinha('empenho.js', 'formatarData', error);
        return '';
    }
}

/* ═══════════════════════════════════════════════════════════════════════════════════
   FUNÇÕES DE RECARGA DE TABELAS
   ═══════════════════════════════════════════════════════════════════════════════════ */

/**
 * Recarrega a tabela de empenhos
 * Verifica se a DataTable existe antes de tentar recarregar
 * Usa ajax.reload com parâmetro false para manter paginação atual
 */
function recarregarTabelaEmpenhos() {
    try {
        if ($.fn.DataTable.isDataTable('#tblEmpenho')) {
            $('#tblEmpenho').DataTable().ajax.reload(null, false);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'empenho.js',
            'recarregarTabelaEmpenhos',
            error,
        );
    }
}

/**
 * Recarrega a tabela de notas fiscais
 * Verifica se a DataTable existe antes de tentar recarregar
 * Usa ajax.reload com parâmetro false para manter paginação atual
 */
function recarregarTabelaNotasFiscais() {
    try {
        if ($.fn.DataTable.isDataTable('#tblNotaFiscal')) {
            $('#tblNotaFiscal').DataTable().ajax.reload(null, false);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'empenho.js',
            'recarregarTabelaNotasFiscais',
            error,
        );
    }
}
