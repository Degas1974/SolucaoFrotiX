/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: ListaEscala.js                                               ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                          ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Lista e Grid de Escalas de Motoristas.                                ║
 * ║    • Grid Syncfusion com filtros e ordenação                            ║
 * ║    • Ações de visualizar, editar, excluir                               ║
 * ║    • Modal de visualização rápida                                        ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Alta (Escalas - Lista)                                    ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

let gridEscalas = null;

// ===================================================================
// FUNÇÕES NO ESCOPO GLOBAL
// ===================================================================
window.visualizarEscala = function (escalaId) {
    try {
        console.log('👁️ visualizarEscala chamada com ID:', escalaId);

        if (!escalaId) {
            console.error('❌ ID da escala não informado');
            AppToast.show('Vermelho', 'ID da escala não informado', 2000);
            return;
        }

        var modalElement = document.getElementById('modalVisualizarEscala');
        if (!modalElement) {
            console.error('❌ Modal não encontrado no DOM');
            AppToast.show('Vermelho', 'Erro: Modal não encontrado', 2000);
            return;
        }

        // Mostrar loading
        var loadingElement = document.getElementById('viewLoading');
        var conteudoElement = document.getElementById('viewConteudo');

        if (loadingElement) loadingElement.style.display = 'block';
        if (conteudoElement) conteudoElement.style.display = 'none';

        // Abrir modal
        var modal = new bootstrap.Modal(modalElement);
        modal.show();

        // Buscar dados via AJAX
        $.ajax({
            url: '/api/Escala/GetEscalaDetalhes',
            type: 'GET',
            data: { id: escalaId },
            success: function (response) {
                try {
                    console.log('✅ Resposta AJAX:', response);

                    if (response.success && response.data) {
                        preencherModalVisualizacao(response.data);
                    } else {
                        console.error('❌ Erro na resposta:', response.message);
                        AppToast.show(
                            'Vermelho',
                            response.message || 'Erro ao carregar dados',
                            3000,
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'visualizarEscala.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('❌ Erro AJAX:', error);
                console.error('❌ Status:', xhr.status);
                console.error('❌ Response:', xhr.responseText);
                AppToast.show(
                    'Vermelho',
                    'Erro ao carregar dados da escala',
                    3000,
                );
            },
            complete: function () {
                if (loadingElement) loadingElement.style.display = 'none';
                if (conteudoElement) conteudoElement.style.display = 'block';
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'visualizarEscala',
            error,
        );
    }
};

window.excluirEscala = function (id) {
    try {
        console.log('🗑️ excluirEscala chamada com ID:', id);

        if (!id) {
            AppToast.show('Vermelho', 'ID da escala não informado', 2000);
            return;
        }

        if (!confirm('Tem certeza que deseja excluir esta escala?')) {
            return;
        }

        window.location.href = '/Escalas/ListaEscala?handler=Delete&id=' + id;
    } catch (error) {
        Alerta.TratamentoErroComLinha('ListaEscala.js', 'excluirEscala', error);
    }
};

window.getStatusClass = function (status) {
    try {
        if (!status) return 'secondary';

        const statusLower = String(status)
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '');

        if (statusLower.includes('indisponivel')) return 'indisponivel';
        if (statusLower.includes('disponivel')) return 'disponivel';
        if (statusLower.includes('em viagem')) return 'em-viagem';
        if (statusLower.includes('em servico')) return 'em-servico';
        if (statusLower.includes('economildo')) return 'economildo';
        if (statusLower.includes('reservado')) return 'reservado';

        return 'secondary';
    } catch (error) {
        console.warn('Erro em getStatusClass:', error);
        return 'secondary';
    }
};

// ===================================================================
// INICIALIZAÇÃO
// ===================================================================
$(document).ready(function () {
    try {
        console.log('✅ ListaEscala.js inicializando...');

        $(document).on('click', '.btn-visualizar', function (e) {
            try {
                e.preventDefault();
                e.stopPropagation();
                var escalaId = $(this).data('id');
                console.log('👁️ Clique em visualizar, ID:', escalaId);
                if (escalaId) {
                    visualizarEscala(escalaId);
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'ListaEscala.js',
                    'btn-visualizar.click',
                    error,
                );
            }
        });

        // ✅ EVENT DELEGATION - Botão EXCLUIR
        $(document).on('click', '.btn-excluir', function (e) {
            try {
                e.preventDefault();
                e.stopPropagation();
                var escalaId = $(this).data('id');
                console.log('🗑️ Clique em excluir, ID:', escalaId);
                if (escalaId) {
                    excluirEscala(escalaId);
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'ListaEscala.js',
                    'btn-excluir.click',
                    error,
                );
            }
        });

        console.log('✅ Event delegation configurado');

        // Aguardar grid Syncfusion inicializar
        setTimeout(function () {
            try {
                const gridElement = document.getElementById('gridEscalas');

                if (
                    gridElement &&
                    gridElement.ej2_instances &&
                    gridElement.ej2_instances.length > 0
                ) {
                    gridEscalas = gridElement.ej2_instances[0];
                    console.log('✅ Grid Syncfusion inicializado');

                    const totalRegistros = gridEscalas.dataSource
                        ? gridEscalas.dataSource.length
                        : 0;
                    console.log('📊 Registros no grid:', totalRegistros);

                    if (totalRegistros > 0) {
                        AppToast.show(
                            'Verde',
                            `${totalRegistros} escala(s) carregada(s)`,
                            3000,
                        );
                    } else {
                        AppToast.show(
                            'Amarelo',
                            'Nenhuma escala encontrada para hoje',
                            3000,
                        );
                    }

                    configurarEventosGrid();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'ListaEscala.js',
                    'setTimeout',
                    error,
                );
            }
        }, 300);

        configurarEventos();

        // Carregar observações do dia
        setTimeout(function () {
            carregarObservacoesDia();
        }, 500);

        // Configurar botão Ficha Escala
        $('#btnFichaEscala')
            .off('click')
            .on('click', function (e) {
                try {
                    e.preventDefault();
                    var dataFiltro = obterValorComponente('DataEscala');
                    var url = '/Escalas/FichaEscalas';
                    if (dataFiltro) {
                        var dataFormatada = new Date(dataFiltro)
                            .toISOString()
                            .split('T')[0];
                        url += '?DataEscala=' + dataFormatada;
                    }
                    window.location.href = url;
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'btnFichaEscala.click',
                        error,
                    );
                    // Fallback - ir sem data
                    window.location.href = '/Escalas/FichaEscalas';
                }
            });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'document.ready',
            error,
        );
    }
});

// ===================================================================
// CONFIGURAR EVENTOS
// ===================================================================
function configurarEventos() {
    try {
        $('#btnFiltrar')
            .off('click')
            .on('click', function (e) {
                try {
                    e.preventDefault();
                    console.log('🔍 Aplicando filtros...');
                    filtrarEscalas();
                    carregarObservacoesDia(); // Recarregar observações ao filtrar
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'btnFiltrar.click',
                        error,
                    );
                }
            });

        $('#btnLimpar, a[href*="ListaEscala"]')
            .off('click')
            .on('click', function (e) {
                try {
                    if (
                        $(this).hasClass('btn-secondary') ||
                        $(this).text().includes('Limpar')
                    ) {
                        console.log('🧹 Limpando filtros...');
                        e.preventDefault();
                        window.location.href = '/Escalas/ListaEscala';
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'btnLimpar.click',
                        error,
                    );
                }
            });

        // Evento para recarregar observações quando mudar a data no filtro
        var dataEscalaElement = document.getElementById('DataEscala');
        if (
            dataEscalaElement &&
            dataEscalaElement.ej2_instances &&
            dataEscalaElement.ej2_instances[0]
        ) {
            dataEscalaElement.ej2_instances[0].change = function (args) {
                try {
                    console.log('📅 Data alterada:', args.value);
                    carregarObservacoesDia();
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'DataEscala.change',
                        error,
                    );
                }
            };
        }

        console.log('✅ Eventos configurados');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'configurarEventos',
            error,
        );
    }
}

function configurarEventosGrid() {
    try {
        if (!gridEscalas) return;

        gridEscalas.toolbarClick = function (args) {
            try {
                if (args.item.id === 'gridEscalas_excelexport') {
                    gridEscalas.excelExport();
                }
                if (args.item.id === 'gridEscalas_pdfexport') {
                    gridEscalas.pdfExport();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'ListaEscala.js',
                    'toolbarClick',
                    error,
                );
            }
        };

        console.log('✅ Eventos do grid configurados');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'configurarEventosGrid',
            error,
        );
    }
}

// ===================================================================
// FILTRAR ESCALAS VIA AJAX
// ===================================================================
function filtrarEscalas() {
    try {
        if (!gridEscalas) {
            console.error('❌ Grid não inicializado');
            AppToast.show('Vermelho', 'Erro: Grid não inicializado', 2000);
            return;
        }

        var dataFiltro = obterValorComponente('DataEscala');

        if (dataFiltro) {
            dataFiltro = new Date(dataFiltro).toISOString();
        }

        var dados = {
            dataFiltro: dataFiltro || '',
            tipoServicoId: obterValorComponente('tipoServicoId') || '',
            turnoId: obterValorComponente('turnoId') || '',
            motoristaId: obterValorComponente('motoristaId') || '',
            veiculoId: obterValorComponente('veiculoId') || '',
            statusMotorista: obterValorComponente('statusMotorista') || '',
            lotacao: obterValorComponente('lotacao') || '',
            textoPesquisa: obterValorComponente('textoPesquisa') || '',
        };

        console.log('🔍 Filtros:', dados);

        $.ajax({
            url: '/api/Escala/GetEscalasFiltradas',
            type: 'POST',
            data: dados,
            success: function (response) {
                try {
                    console.log('✅ Resposta recebida:', response);

                    if (response.success && response.data) {
                        const dados = response.data.map((item) => {
                            return {
                                EscalaDiaId: item.escalaDiaId,
                                MotoristaId: item.motoristaId,
                                VeiculoId: item.veiculoId,
                                TipoServicoId: item.tipoServicoId,
                                TurnoId: item.turnoId,
                                DataEscala: item.dataEscala,
                                HoraInicio: item.horaInicio
                                    ? item.horaInicio.substring(0, 5)
                                    : '',
                                HoraFim: item.horaFim
                                    ? item.horaFim.substring(0, 5)
                                    : '',
                                NomeMotorista: item.nomeMotorista,
                                NomeServico: item.nomeServico,
                                NomeTurno: item.nomeTurno,
                                Placa: item.placa,
                                Lotacao: item.lotacao,
                                NumeroSaidas: item.numeroSaidas,
                                StatusMotorista: item.statusMotorista,
                                NomeRequisitante: item.nomeRequisitante,
                                Observacoes: item.observacoes,
                            };
                        });

                        gridEscalas.dataSource = dados;
                        AppToast.show(
                            'Verde',
                            dados.length + ' escala(s) encontrada(s)',
                            2000,
                        );
                    } else {
                        AppToast.show(
                            'Amarelo',
                            response.message || 'Nenhuma escala encontrada',
                            3000,
                        );
                        gridEscalas.dataSource = [];
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'filtrarEscalas.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('❌ Erro AJAX:', error);
                AppToast.show('Vermelho', 'Erro ao aplicar filtros', 3000);
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'filtrarEscalas',
            error,
        );
    }
}

// ===================================================================
// SALVAR OBSERVAÇÃO
// ===================================================================
function salvarObservacao() {
    try {
        const titulo = obterValorComponente('obsTitle');
        const descricao = obterValorComponente('obsDescription');
        const prioridade = obterValorComponente('obsPriority');
        const exibirDe = obterValorComponente('obsDateFrom');
        const exibirAte = obterValorComponente('obsDateTo');

        if (!descricao) {
            AppToast.show('Amarelo', 'Por favor, preencha a descrição', 3000);
            return;
        }

        if (!exibirDe || !exibirAte) {
            AppToast.show(
                'Amarelo',
                'Por favor, preencha as datas de exibição',
                3000,
            );
            return;
        }

        $.ajax({
            url: '/api/Escala/SalvarObservacaoDia',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                titulo: titulo || '',
                descricao: descricao,
                prioridade: prioridade || 'Normal',
                exibirDe: new Date(exibirDe).toISOString(),
                exibirAte: new Date(exibirAte).toISOString(),
            }),
            success: function (response) {
                try {
                    if (response.success) {
                        AppToast.show(
                            'Verde',
                            'Observação salva com sucesso!',
                            3000,
                        );

                        const modal = bootstrap.Modal.getInstance(
                            document.getElementById('modalObservacao'),
                        );
                        if (modal) modal.hide();

                        limparCampoComponente('obsTitle');
                        limparCampoComponente('obsDescription');
                        limparCampoComponente('obsDateFrom');
                        limparCampoComponente('obsDateTo');

                        carregarObservacoesDia();
                    } else {
                        AppToast.show(
                            'Vermelho',
                            response.message || 'Erro ao salvar',
                            3000,
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'salvarObservacao.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro AJAX:', error);
                AppToast.show('Vermelho', 'Erro ao salvar observação', 3000);
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'salvarObservacao',
            error,
        );
    }
}

// ===================================================================
// CARREGAR OBSERVAÇÕES DO DIA
// ===================================================================
function carregarObservacoesDia() {
    try {
        var dataFiltro = obterValorComponente('DataEscala');
        var dataParam = dataFiltro
            ? new Date(dataFiltro).toISOString()
            : new Date().toISOString();

        $.ajax({
            url: '/api/Escala/GetObservacoesDia',
            type: 'GET',
            data: { data: dataParam },
            success: function (response) {
                try {
                    var container = document.getElementById(
                        'observacoesContainer',
                    );
                    if (!container) return;

                    if (
                        response.success &&
                        response.data &&
                        response.data.length > 0
                    ) {
                        var html = '';
                        response.data.forEach(function (obs) {
                            var prioridadeClass =
                                obs.prioridade === 'Alta'
                                    ? 'danger'
                                    : obs.prioridade === 'Normal'
                                      ? 'warning'
                                      : 'secondary';

                            // Usar diretamente as datas já formatadas pela API (dd/MM/yyyy)
                            var dataDeFormatada = obs.exibirDe;
                            var dataAteFormatada = obs.exibirAte;

                            html +=
                                '<div class="alert alert-' +
                                prioridadeClass +
                                ' mb-2">';
                            html +=
                                '<div class="d-flex justify-content-between align-items-start">';
                            html += '<div>';
                            if (obs.titulo) {
                                html +=
                                    '<strong>' + obs.titulo + '</strong><br>';
                            }
                            html += obs.descricao;
                            html +=
                                '<br><small class="text-muted">Período: ' +
                                dataDeFormatada +
                                ' a ' +
                                dataAteFormatada +
                                ' | Prioridade: ' +
                                obs.prioridade +
                                '</small>';
                            html += '</div>';
                            html +=
                                '<button type="button" class="btn btn-sm btn-outline-danger ms-2" onclick="excluirObservacaoDia(\'' +
                                obs.observacaoId +
                                '\')">';
                            html += '<i class="fas fa-trash"></i>';
                            html += '</button>';
                            html += '</div>';
                            html += '</div>';
                        });
                        container.innerHTML = html;
                    } else {
                        container.innerHTML =
                            '<p class="text-muted">Nenhuma observação para este dia</p>';
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'carregarObservacoesDia.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar observações:', error);
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'carregarObservacoesDia',
            error,
        );
    }
}

// ===================================================================
// EXCLUIR OBSERVAÇÃO DO DIA
// ===================================================================
window.excluirObservacaoDia = function (observacaoId) {
    try {
        if (!confirm('Deseja excluir esta observação?')) return;

        $.ajax({
            url: '/api/Escala/ExcluirObservacaoDia',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ observacaoId: observacaoId }),
            success: function (response) {
                try {
                    if (response.success) {
                        AppToast.show('Verde', 'Observação excluída!', 2000);
                        carregarObservacoesDia();
                    } else {
                        AppToast.show(
                            'Vermelho',
                            response.message || 'Erro ao excluir',
                            3000,
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEscala.js',
                        'excluirObservacaoDia.success',
                        error,
                    );
                }
            },
            error: function () {
                AppToast.show('Vermelho', 'Erro ao excluir observação', 3000);
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'excluirObservacaoDia',
            error,
        );
    }
};

// ===================================================================
// LIMPAR CAMPO COMPONENTE SYNCFUSION
// ===================================================================
function limparCampoComponente(elementId) {
    try {
        const element = document.getElementById(elementId);
        if (element && element.ej2_instances && element.ej2_instances[0]) {
            element.ej2_instances[0].value = null;
        }
    } catch (error) {
        console.warn('Erro ao limpar componente:', error);
    }
}

// ===================================================================
// FUNÇÕES AUXILIARES
// ===================================================================
function obterValorComponente(elementId) {
    try {
        const element = document.getElementById(elementId);
        if (element && element.ej2_instances && element.ej2_instances[0]) {
            return element.ej2_instances[0].value;
        }
        return null;
    } catch (error) {
        console.warn(
            `⚠️ Erro ao obter valor do componente ${elementId}:`,
            error,
        );
        return null;
    }
}

// ===================================================================
// PREENCHER MODAL DE VISUALIZAÇÃO
// ===================================================================
function preencherModalVisualizacao(dados) {
    try {
        console.log('📝 Preenchendo modal com:', dados);

        document.getElementById('viewNomeMotorista').textContent =
            dados.nomeMotorista || '-';

        var fotoElement = document.getElementById('viewFotoMotorista');
        if (dados.fotoBase64 && dados.fotoBase64 !== '') {
            fotoElement.src = 'data:image/png;base64,' + dados.fotoBase64;
        } else {
            fotoElement.src = '/images/default-driver.png';
        }

        var statusBadge = document.getElementById('viewStatusBadge');
        var statusTexto = dados.statusMotorista || 'Indefinido';
        statusBadge.textContent = statusTexto;
        statusBadge.className =
            'badge mt-2 status-' + getStatusClass(statusTexto);

        var dataFormatada = '-';
        if (dados.dataEscala) {
            var data = new Date(dados.dataEscala);
            if (!isNaN(data.getTime())) {
                dataFormatada = data.toLocaleDateString('pt-BR');
            }
        }
        document.getElementById('viewDataEscala').textContent = dataFormatada;

        var horarioTexto = '-';
        if (dados.horaInicio) {
            horarioTexto = dados.horaInicio.substring(0, 5);
            if (dados.horaFim) {
                horarioTexto += ' às ' + dados.horaFim.substring(0, 5);
            }
        }
        document.getElementById('viewHorario').textContent = horarioTexto;

        document.getElementById('viewTurno').textContent =
            dados.nomeTurno || '-';
        document.getElementById('viewPlaca').textContent =
            dados.placa || 'Sem veículo';
        document.getElementById('viewLotacao').textContent =
            dados.lotacao || '-';
        document.getElementById('viewNumeroSaidas').textContent =
            dados.numeroSaidas || '0';
        document.getElementById('viewTipoServico').textContent =
            dados.nomeServico || '-';
        document.getElementById('viewRequisitante').textContent =
            dados.nomeRequisitante || '-';
        document.getElementById('viewObservacoes').textContent =
            dados.observacoes || 'Nenhuma observação registrada';

        var divCobertura = document.getElementById('divCoberturaInfo');
        var divCobrindo = document.getElementById('divCobrindoInfo');
        var statusLower = (statusTexto || '')
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '');

        // Alerta quando motorista está INDISPONÍVEL (sendo coberto)
        if (statusLower.includes('indisponivel')) {
            divCobertura.style.display = 'block';
            document.getElementById('viewNomeCobertor').textContent =
                dados.nomeMotoristaCobertor ||
                dados.nomeCobertor ||
                'Não definido';
            document.getElementById('viewMotivoCobertura').textContent =
                dados.motivoCobertura || 'Não informado';
        } else {
            divCobertura.style.display = 'none';
        }

        // Alerta quando motorista está COBRINDO outro
        if (dados.nomeMotoristaCobrindo && dados.nomeMotoristaCobrindo !== '') {
            divCobrindo.style.display = 'block';
            document.getElementById('viewNomeCoberto').textContent =
                dados.nomeMotoristaCobrindo;
            document.getElementById('viewMotivoCoberto').textContent =
                dados.motivoCoberturaCobrindo || 'Não informado';
        } else {
            divCobrindo.style.display = 'none';
        }

        document.getElementById('btnEditarEscala').href =
            '/Escalas/UpsertEEscala?id=' + dados.escalaDiaId;

        console.log('✅ Modal preenchido com sucesso');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEscala.js',
            'preencherModalVisualizacao',
            error,
        );
    }
}

console.log('📄 ListaEscala.js carregado');
