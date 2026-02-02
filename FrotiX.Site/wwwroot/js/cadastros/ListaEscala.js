
/* ****************************************************************************************
 * ⚡ ARQUIVO: ListaEscala.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar a listagem, filtros, visualização, exclusão e observações
 *                   de escalas de motoristas. Integra grid Syncfusion para exibição de
 *                   dados, modal de visualização detalhada e gerenciamento de observações
 *                   diárias com diferentes níveis de prioridade.
 *
 * 📥 ENTRADAS     : Cliques em botões do grid (visualizar, excluir), eventos de mudança
 *                   em filtros (data, tipo serviço, turno, motorista, veículo), submissão
 *                   de formulários de observação. Dados do grid carregados via API.
 *
 * 📤 SAÍDAS       : Modal de visualização preenchido com detalhes da escala, grid filtrado
 *                   com dados atualizados, observações renderizadas no container,
 *                   exportações em Excel/PDF, redirecionamentos para edição/exclusão.
 *
 * 🔗 CHAMADA POR  : Page Razor ListaEscala.cshtml, botões de ação do grid, eventos DOM.
 *
 * 🔄 CHAMA        : API endpoints (/api/Escala/*), funções de UI, Syncfusion grid,
 *                   Alerta.* (SweetAlert), AppToast, FtxSpin (loading).
 *
 * 📦 DEPENDÊNCIAS : jQuery, Syncfusion EJ2 Grid, Bootstrap 5.3, alerta.js, frotix.js
 *
 * 📝 OBSERVAÇÕES  : Grid Syncfusion é inicializado com delay de 300ms. Observações são
 *                   carregadas automaticamente ao filtrar ou mudar data. Modal de
 *                   visualização exibe alertas de cobertura/cobrindo de motoristas.
 **************************************************************************************** */

let gridEscalas = null;

// ===================================================================
// FUNÇÕES NO ESCOPO GLOBAL
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: visualizarEscala
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Abre modal de visualização com detalhes completos da escala, incluindo
 *                   informações do motorista, veículo, horários, e alertas de cobertura.
 *                   Busca dados via AJAX e preenche template do modal.
 *
 * 📥 ENTRADAS     : escalaId [number] - Identificador da escala a visualizar
 *
 * 📤 SAÍDAS       : Modal exibido com dados preenchidos, ou mensagem de erro em toast
 *
 * 🔗 CHAMADA POR  : Evento click em botões .btn-visualizar do grid (event delegation)
 *
 * 🔄 CHAMA        : GET /api/Escala/GetEscalaDetalhes, preencherModalVisualizacao(),
 *                   AppToast.show(), Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Exibe loading enquanto carrega dados. Modal é destruído e recriado
 *                   a cada chamada. Trata erros de ID inválido e modal não encontrado.
 ****************************************************************************************/
window.visualizarEscala = function(escalaId) {
    try {
        // [VALIDACAO] Verificar se ID foi fornecido
        if (!escalaId) {
            AppToast.show('Vermelho', 'ID da escala não informado', 2000);
            return;
        }

        // [UI] Obter elementos do modal
        var modalElement = document.getElementById('modalVisualizarEscala');
        if (!modalElement) {
            AppToast.show('Vermelho', 'Erro: Modal não encontrado', 2000);
            return;
        }

        // [UI] Mostrar loading, esconder conteúdo
        var loadingElement = document.getElementById('viewLoading');
        var conteudoElement = document.getElementById('viewConteudo');

        if (loadingElement) loadingElement.style.display = 'block';
        if (conteudoElement) conteudoElement.style.display = 'none';

        // [UI] Abrir modal Bootstrap
        var modal = new bootstrap.Modal(modalElement);
        modal.show();

        // [AJAX] Chamada para endpoint /api/Escala/GetEscalaDetalhes
        // 📥 ENVIA: { id: escalaId }
        // 📤 RECEBE: { success: bool, data: EscalaDetalhesDTO, message: string }
        $.ajax({
            url: '/api/Escala/GetEscalaDetalhes',
            type: 'GET',
            data: { id: escalaId },
            success: function (response) {
                try {
                    if (response.success && response.data) {
                        preencherModalVisualizacao(response.data);
                    } else {
                        AppToast.show('Vermelho', response.message || 'Erro ao carregar dados', 3000);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaEscala.js", "visualizarEscala.success", error);
                }
            },
            error: function (xhr, status, error) {
                AppToast.show('Vermelho', 'Erro ao carregar dados da escala', 3000);
            },
            complete: function () {
                // [UI] Esconder loading, mostrar conteúdo
                if (loadingElement) loadingElement.style.display = 'none';
                if (conteudoElement) conteudoElement.style.display = 'block';
            }
        });

    } catch (error) {
        Alerta.TratamentoErroComLinha('ListaEscala.js', 'visualizarEscala', error);
    }
};

/****************************************************************************************
 * ⚡ FUNÇÃO: excluirEscala
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Remove uma escala do sistema após confirmação do usuário. Redireciona
 *                   para handler PageModel no servidor que processa a exclusão lógica.
 *
 * 📥 ENTRADAS     : id [number] - Identificador da escala a excluir
 *
 * 📤 SAÍDAS       : Redirecionamento para página com handler=Delete, ou toast de erro
 *
 * 🔗 CHAMADA POR  : Evento click em botões .btn-excluir do grid (event delegation)
 *
 * 🔄 CHAMA        : window.location.href (redirecionamento), AppToast.show(),
 *                   Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Usa confirm() nativo para confirmação. Exclusão é processada no
 *                   servidor via handler POST. Página recarrega após exclusão.
 ****************************************************************************************/
window.excluirEscala = function(id) {
    try {
        // [VALIDACAO] Verificar se ID foi fornecido
        if (!id) {
            AppToast.show('Vermelho', 'ID da escala não informado', 2000);
            return;
        }

        // [UI] Pedir confirmação do usuário
        if (!confirm('Tem certeza que deseja excluir esta escala?')) {
            return;
        }

        // [LOGICA] Redirecionar para handler Delete no PageModel
        window.location.href = '/Escalas/ListaEscala?handler=Delete&id=' + id;
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "excluirEscala", error);
    }
};

/****************************************************************************************
 * ⚡ FUNÇÃO: getStatusClass
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapeia strings de status para classes CSS de badge, removendo
 *                   acentuação e normalizando comparações. Retorna classe visual
 *                   apropriada para renderização no modal de visualização.
 *
 * 📥 ENTRADAS     : status [string] - Status do motorista (ex: "Indisponível", "Disponível")
 *
 * 📤 SAÍDAS       : Classe CSS [string] - Uma das: 'indisponivel', 'disponivel', 'em-viagem',
 *                   'em-servico', 'economildo', 'reservado', 'secondary'
 *
 * 🔗 CHAMADA POR  : preencherModalVisualizacao() para estilizar badge de status
 *
 * 🔄 CHAMA        : Nenhuma função externa
 *
 * 📝 OBSERVAÇÕES  : Usa normalize('NFD') para remover acentos antes de comparar.
 *                   Retorna 'secondary' para status não reconhecidos.
 ****************************************************************************************/
window.getStatusClass = function(status) {
    try {
        // [VALIDACAO] Se status vazio, retornar classe padrão
        if (!status) return 'secondary';

        // [LOGICA] Normalizar string: lowercase e remover acentos (NFD)
        const statusLower = String(status).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

        // [LOGICA] Mapear strings para classes CSS
        if (statusLower.includes('indisponivel')) return 'indisponivel';
        if (statusLower.includes('disponivel')) return 'disponivel';
        if (statusLower.includes('em viagem')) return 'em-viagem';
        if (statusLower.includes('em servico')) return 'em-servico';
        if (statusLower.includes('economildo')) return 'economildo';
        if (statusLower.includes('reservado')) return 'reservado';

        // [LOGICA] Status não reconhecido: retornar classe padrão
        return 'secondary';
    } catch (error) {
        console.warn('Erro em getStatusClass:', error);
        return 'secondary';
    }
};

// ===================================================================
// INICIALIZAÇÃO
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: document.ready
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicializa toda a página quando DOM está carregado. Configura
 *                   event delegation para botões do grid, aguarda inicialização do
 *                   grid Syncfusion, registra eventos de filtros e carrega observações.
 *
 * 📥 ENTRADAS     : Nenhuma (executa automaticamente ao carregar página)
 *
 * 📤 SAÍDAS       : Eventos configurados, grid inicializado, observações carregadas,
 *                   botões funcionando com event delegation
 *
 * 🔗 CHAMADA POR  : jQuery automaticamente ao document.ready
 *
 * 🔄 CHAMA        : configurarEventos(), configurarEventosGrid(), carregarObservacoesDia()
 *
 * 📝 OBSERVAÇÕES  : Usa setTimeout(300ms) para aguardar inicialização do Syncfusion.
 *                   Event delegation evita problemas com elementos dinâmicos do grid.
 ****************************************************************************************/
$(document).ready(function () {
    try {
        // [UI] Event delegation para botão VISUALIZAR
        $(document).on('click', '.btn-visualizar', function(e) {
            try {
                e.preventDefault();
                e.stopPropagation();
                var escalaId = $(this).data('id');
                if (escalaId) {
                    visualizarEscala(escalaId);
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "btn-visualizar.click", error);
            }
        });

        // [UI] Event delegation para botão EXCLUIR
        $(document).on('click', '.btn-excluir', function(e) {
            try {
                e.preventDefault();
                e.stopPropagation();
                var escalaId = $(this).data('id');
                if (escalaId) {
                    excluirEscala(escalaId);
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "btn-excluir.click", error);
            }
        });

        // [LOGICA] Aguardar grid Syncfusion inicializar (300ms delay necessário)
        setTimeout(function () {
            try {
                const gridElement = document.getElementById('gridEscalas');

                if (gridElement && gridElement.ej2_instances && gridElement.ej2_instances.length > 0) {
                    gridEscalas = gridElement.ej2_instances[0];

                    // [DADOS] Contar registros carregados
                    const totalRegistros = gridEscalas.dataSource ? gridEscalas.dataSource.length : 0;

                    // [UI] Exibir toast com feedback
                    if (totalRegistros > 0) {
                        AppToast.show('Verde', `${totalRegistros} escala(s) carregada(s)`, 3000);
                    } else {
                        AppToast.show('Amarelo', 'Nenhuma escala encontrada para hoje', 3000);
                    }

                    // [LOGICA] Configurar eventos do grid após inicialização
                    configurarEventosGrid();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "setTimeout", error);
            }
        }, 300);

        // [LOGICA] Configurar eventos de filtro e outras ações
        configurarEventos();

        // [UI] Carregar observações do dia após 500ms
        setTimeout(function() {
            carregarObservacoesDia();
        }, 500);

        // [UI] Configurar botão Ficha Escala com parâmetro de data
        $('#btnFichaEscala').off('click').on('click', function(e) {
            try {
                e.preventDefault();
                var dataFiltro = obterValorComponente('DataEscala');
                var url = '/Escalas/FichaEscalas';
                // [DADOS] Adicionar data ao parâmetro de query se selecionada
                if (dataFiltro) {
                    var dataFormatada = new Date(dataFiltro).toISOString().split('T')[0];
                    url += '?DataEscala=' + dataFormatada;
                }
                window.location.href = url;
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "btnFichaEscala.click", error);
                // [LOGICA] Fallback: ir sem data
                window.location.href = '/Escalas/FichaEscalas';
            }
        });

    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "document.ready", error);
    }
});

// ===================================================================
// CONFIGURAR EVENTOS
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: configurarEventos
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registra handlers para botões de filtro/limpeza e eventos de mudança
 *                   de data no componente Syncfusion. Recarrega observações quando data
 *                   é alterada no filtro.
 *
 * 📥 ENTRADAS     : Nenhuma (usa elementos do DOM via jQuery/Syncfusion)
 *
 * 📤 SAÍDAS       : Eventos registrados nos elementos, observações recarregadas ao filtrar
 *
 * 🔗 CHAMADA POR  : document.ready [linha 174]
 *
 * 🔄 CHAMA        : filtrarEscalas(), carregarObservacoesDia(), Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Usa .off('click').on('click') para evitar múltiplos handlers.
 *                   Evento de mudança de data usa Syncfusion change callback.
 ****************************************************************************************/
function configurarEventos() {
    try {
        // [UI] Registrar clique em botão FILTRAR
        $('#btnFiltrar').off('click').on('click', function (e) {
            try {
                e.preventDefault();
                // [LOGICA] Aplicar filtros e recarregar observações
                filtrarEscalas();
                carregarObservacoesDia();
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "btnFiltrar.click", error);
            }
        });

        // [UI] Registrar clique em botão LIMPAR (retorna para página limpa)
        $('#btnLimpar, a[href*="ListaEscala"]').off('click').on('click', function (e) {
            try {
                if ($(this).hasClass('btn-secondary') || $(this).text().includes('Limpar')) {
                    e.preventDefault();
                    window.location.href = '/Escalas/ListaEscala';
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "btnLimpar.click", error);
            }
        });

        // [UI] Evento de mudança na data do filtro (Syncfusion DatePicker)
        var dataEscalaElement = document.getElementById('DataEscala');
        if (dataEscalaElement && dataEscalaElement.ej2_instances && dataEscalaElement.ej2_instances[0]) {
            dataEscalaElement.ej2_instances[0].change = function(args) {
                try {
                    // [LOGICA] Recarregar observações quando data muda
                    carregarObservacoesDia();
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaEscala.js", "DataEscala.change", error);
                }
            };
        }

    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "configurarEventos", error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: configurarEventosGrid
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registra handlers de toolbar do grid Syncfusion para exportação
 *                   em Excel e PDF. Detecta cliques nos botões de export e dispara
 *                   métodos correspondentes do grid.
 *
 * 📥 ENTRADAS     : Nenhuma (usa variável global gridEscalas)
 *
 * 📤 SAÍDAS       : Exports gerados em Excel/PDF quando botões são clicados
 *
 * 🔗 CHAMADA POR  : document.ready [linha 167]
 *
 * 🔄 CHAMA        : gridEscalas.excelExport(), gridEscalas.pdfExport()
 *
 * 📝 OBSERVAÇÕES  : Verifica se gridEscalas existe antes de configurar. Toolbar ids
 *                   são definidos automaticamente pelo Syncfusion como #gridEscalas_excelexport.
 ****************************************************************************************/
function configurarEventosGrid() {
    try {
        // [VALIDACAO] Verificar se grid foi inicializado
        if (!gridEscalas) return;

        // [UI] Registrar handler de cliques na toolbar (botões de export)
        gridEscalas.toolbarClick = function (args) {
            try {
                // [LOGICA] Detectar botão clicado e disparar export correspondente
                if (args.item.id === 'gridEscalas_excelexport') {
                    gridEscalas.excelExport();
                }
                if (args.item.id === 'gridEscalas_pdfexport') {
                    gridEscalas.pdfExport();
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha("ListaEscala.js", "toolbarClick", error);
            }
        };

    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "configurarEventosGrid", error);
    }
}

// ===================================================================
// FILTRAR ESCALAS VIA AJAX
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: filtrarEscalas
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Busca escalas do servidor com base em filtros selecionados
 *                   (data, tipo serviço, turno, motorista, veículo, status, lotação).
 *                   Atualiza dataSource do grid com resultados formatados e exibe toast.
 *
 * 📥 ENTRADAS     : Valores de componentes Syncfusion no filtro (via obterValorComponente)
 *
 * 📤 SAÍDAS       : Grid atualizado com dados filtrados, toast com contagem de resultados
 *
 * 🔗 CHAMADA POR  : Clique em #btnFiltrar [linha 209], mudança de data [linha 238]
 *
 * 🔄 CHAMA        : POST /api/Escala/GetEscalasFiltradas, obterValorComponente(),
 *                   AppToast.show(), Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Formata horas para HH:mm removendo segundos. Mapeia nomes de campos
 *                   da API (camelCase) para propriedades do ViewModel (PascalCase).
 *                   Se nenhum resultado, exibe toast amarelo e limpa grid.
 ****************************************************************************************/
function filtrarEscalas() {
    try {
        // [VALIDACAO] Verificar se grid está inicializado
        if (!gridEscalas) {
            AppToast.show('Vermelho', 'Erro: Grid não inicializado', 2000);
            return;
        }

        // [DADOS] Obter valores dos componentes de filtro (Syncfusion)
        var dataFiltro = obterValorComponente('DataEscala');

        // [DADOS] Converter data para ISO 8601 se fornecida
        if (dataFiltro) {
            dataFiltro = new Date(dataFiltro).toISOString();
        }

        // [DADOS] Montar objeto com todos os filtros
        var dados = {
            dataFiltro: dataFiltro || '',
            tipoServicoId: obterValorComponente('tipoServicoId') || '',
            turnoId: obterValorComponente('turnoId') || '',
            motoristaId: obterValorComponente('motoristaId') || '',
            veiculoId: obterValorComponente('veiculoId') || '',
            statusMotorista: obterValorComponente('statusMotorista') || '',
            lotacao: obterValorComponente('lotacao') || '',
            textoPesquisa: obterValorComponente('textoPesquisa') || ''
        };

        // [AJAX] Chamada para endpoint /api/Escala/GetEscalasFiltradas
        // 📥 ENVIA: { dataFiltro, tipoServicoId, turnoId, motoristaId, ... }
        // 📤 RECEBE: { success: bool, data: [EscalaDetalhesDTO], message: string }
        $.ajax({
            url: '/api/Escala/GetEscalasFiltradas',
            type: 'POST',
            data: dados,
            success: function (response) {
                try {
                    if (response.success && response.data) {
                        // [DADOS] Mapear resposta da API para propriedades do grid
                        // Converte camelCase (API) para PascalCase (grid) e formata horários
                        const resultados = response.data.map(item => {
                            return {
                                EscalaDiaId: item.escalaDiaId,
                                MotoristaId: item.motoristaId,
                                VeiculoId: item.veiculoId,
                                TipoServicoId: item.tipoServicoId,
                                TurnoId: item.turnoId,
                                DataEscala: item.dataEscala,
                                HoraInicio: item.horaInicio ? item.horaInicio.substring(0, 5) : '',
                                HoraFim: item.horaFim ? item.horaFim.substring(0, 5) : '',
                                NomeMotorista: item.nomeMotorista,
                                NomeServico: item.nomeServico,
                                NomeTurno: item.nomeTurno,
                                Placa: item.placa,
                                Lotacao: item.lotacao,
                                NumeroSaidas: item.numeroSaidas,
                                StatusMotorista: item.statusMotorista,
                                NomeRequisitante: item.nomeRequisitante,
                                Observacoes: item.observacoes
                            };
                        });

                        // [UI] Atualizar grid com novos dados e exibir feedback
                        gridEscalas.dataSource = resultados;
                        AppToast.show('Verde', resultados.length + ' escala(s) encontrada(s)', 2000);
                    } else {
                        // [UI] Sem resultados: exibir mensagem e limpar grid
                        AppToast.show('Amarelo', response.message || 'Nenhuma escala encontrada', 3000);
                        gridEscalas.dataSource = [];
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaEscala.js", "filtrarEscalas.success", error);
                }
            },
            error: function (xhr, status, error) {
                AppToast.show('Vermelho', 'Erro ao aplicar filtros', 3000);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "filtrarEscalas", error);
    }
}

// ===================================================================
// SALVAR OBSERVAÇÃO
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: salvarObservacao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Valida e salva observação diária (título, descrição, prioridade,
 *                   período de exibição) no servidor. Fecha modal, limpa formulário e
 *                   recarrega observações na página após sucesso.
 *
 * 📥 ENTRADAS     : Valores dos campos do modal #modalObservacao:
 *                   - obsTitle, obsDescription, obsPriority, obsDateFrom, obsDateTo
 *
 * 📤 SAÍDAS       : Observação salva no BD, modal fechado, formulário limpo, container
 *                   atualizado com novas observações
 *
 * 🔗 CHAMADA POR  : Botão "Salvar" do modal (handler JS no .cshtml)
 *
 * 🔄 CHAMA        : POST /api/Escala/SalvarObservacaoDia, obterValorComponente(),
 *                   limparCampoComponente(), carregarObservacoesDia(),
 *                   AppToast.show(), Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Valida descrição e datas antes de enviar. Convertef datas para
 *                   ISO 8601 no envio. Prioridade padrão: 'Normal'. Modal é recuperado
 *                   e fechado via bootstrap.Modal.getInstance().
 ****************************************************************************************/
function salvarObservacao() {
    try {
        // [DADOS] Obter valores do modal de observação
        const titulo = obterValorComponente('obsTitle');
        const descricao = obterValorComponente('obsDescription');
        const prioridade = obterValorComponente('obsPriority');
        const exibirDe = obterValorComponente('obsDateFrom');
        const exibirAte = obterValorComponente('obsDateTo');

        // [VALIDACAO] Verificar campos obrigatórios
        if (!descricao) {
            AppToast.show('Amarelo', 'Por favor, preencha a descrição', 3000);
            return;
        }

        if (!exibirDe || !exibirAte) {
            AppToast.show('Amarelo', 'Por favor, preencha as datas de exibição', 3000);
            return;
        }

        // [AJAX] Chamada para endpoint /api/Escala/SalvarObservacaoDia
        // 📥 ENVIA: { titulo, descricao, prioridade, exibirDe, exibirAte }
        // 📤 RECEBE: { success: bool, message: string }
        $.ajax({
            url: '/api/Escala/SalvarObservacaoDia',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                titulo: titulo || '',
                descricao: descricao,
                prioridade: prioridade || 'Normal',
                exibirDe: new Date(exibirDe).toISOString(),
                exibirAte: new Date(exibirAte).toISOString()
            }),
            success: function (response) {
                try {
                    if (response.success) {
                        AppToast.show('Verde', 'Observação salva com sucesso!', 3000);

                        // [UI] Fechar modal e limpar campos
                        const modal = bootstrap.Modal.getInstance(document.getElementById('modalObservacao'));
                        if (modal) modal.hide();

                        // [UI] Limpar campos do formulário (valores Syncfusion)
                        limparCampoComponente('obsTitle');
                        limparCampoComponente('obsDescription');
                        limparCampoComponente('obsDateFrom');
                        limparCampoComponente('obsDateTo');

                        // [LOGICA] Recarregar observações para refletir nova entrada
                        carregarObservacoesDia();
                    } else {
                        AppToast.show('Vermelho', response.message || 'Erro ao salvar', 3000);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaEscala.js", "salvarObservacao.success", error);
                }
            },
            error: function (xhr, status, error) {
                AppToast.show('Vermelho', 'Erro ao salvar observação', 3000);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "salvarObservacao", error);
    }
}

// ===================================================================
// CARREGAR OBSERVAÇÕES DO DIA
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: carregarObservacoesDia
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Busca observações válidas para a data selecionada (ou hoje se não
 *                   selecionada) e renderiza como cards de alerta no container, com
 *                   cores baseadas em prioridade e botão de exclusão.
 *
 * 📥 ENTRADAS     : Data selecionada no filtro DataEscala (ou data atual como fallback)
 *
 * 📤 SAÍDAS       : HTML renderizado em #observacoesContainer com cards de observação
 *
 * 🔗 CHAMADA POR  : document.ready [linha 178], filtrarEscalas [linha 214],
 *                   DataEscala.change [linha 238], salvarObservacao [linha 400]
 *
 * 🔄 CHAMA        : GET /api/Escala/GetObservacoesDia, excluirObservacaoDia(),
 *                   Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Observações são renderizadas como alert Bootstrap (danger/warning/secondary
 *                   conforme prioridade). Container é esvaziado antes de popular.
 *                   Datas já vêm formatadas do servidor (dd/MM/yyyy).
 ****************************************************************************************/
function carregarObservacoesDia() {
    try {
        var dataFiltro = obterValorComponente('DataEscala');
        var dataParam = dataFiltro ? new Date(dataFiltro).toISOString() : new Date().toISOString();

        $.ajax({
            url: '/api/Escala/GetObservacoesDia',
            type: 'GET',
            data: { data: dataParam },
            success: function (response) {
                try {
                    var container = document.getElementById('observacoesContainer');
                    if (!container) return;

                    if (response.success && response.data && response.data.length > 0) {
                        var html = '';
                        response.data.forEach(function(obs) {
                            var prioridadeClass = obs.prioridade === 'Alta' ? 'danger' : 
                                                  obs.prioridade === 'Normal' ? 'warning' : 'secondary';
                            
                            // Usar diretamente as datas já formatadas pela API (dd/MM/yyyy)
                            var dataDeFormatada = obs.exibirDe;
                            var dataAteFormatada = obs.exibirAte;
                            
                            html += '<div class="alert alert-' + prioridadeClass + ' mb-2">';
                            html += '<div class="d-flex justify-content-between align-items-start">';
                            html += '<div>';
                            if (obs.titulo) {
                                html += '<strong>' + obs.titulo + '</strong><br>';
                            }
                            html += obs.descricao;
                            html += '<br><small class="text-muted">Período: ' + dataDeFormatada + ' a ' + dataAteFormatada + ' | Prioridade: ' + obs.prioridade + '</small>';
                            html += '</div>';
                            html += '<button type="button" class="btn btn-sm btn-outline-danger ms-2" onclick="excluirObservacaoDia(\'' + obs.observacaoId + '\')">';
                            html += '<i class="fas fa-trash"></i>';
                            html += '</button>';
                            html += '</div>';
                            html += '</div>';
                        });
                        container.innerHTML = html;
                    } else {
                        container.innerHTML = '<p class="text-muted">Nenhuma observação para este dia</p>';
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaEscala.js", "carregarObservacoesDia.success", error);
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar observações:', error);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "carregarObservacoesDia", error);
    }
}

// ===================================================================
// EXCLUIR OBSERVAÇÃO DO DIA
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: excluirObservacaoDia
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Remove observação após confirmação do usuário. Envia requisição
 *                   POST ao servidor e recarrega observações do dia após sucesso.
 *
 * 📥 ENTRADAS     : observacaoId [number] - ID da observação a excluir
 *
 * 📤 SAÍDAS       : Observação removida do BD, container atualizado com novo HTML
 *
 * 🔗 CHAMADA POR  : Clique em botões de exclusão nos cards de observação (inline onclick)
 *
 * 🔄 CHAMA        : POST /api/Escala/ExcluirObservacaoDia, carregarObservacoesDia(),
 *                   AppToast.show(), Alerta.TratamentoErroComLinha()
 *
 * 📝 OBSERVAÇÕES  : Usa confirm() nativo para confirmação. Recarrega observações
 *                   automaticamente após exclusão bem-sucedida.
 ****************************************************************************************/
window.excluirObservacaoDia = function(observacaoId) {
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
                        AppToast.show('Vermelho', response.message || 'Erro ao excluir', 3000);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaEscala.js", "excluirObservacaoDia.success", error);
                }
            },
            error: function () {
                AppToast.show('Vermelho', 'Erro ao excluir observação', 3000);
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaEscala.js", "excluirObservacaoDia", error);
    }
};

// ===================================================================
// LIMPAR CAMPO COMPONENTE SYNCFUSION
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: limparCampoComponente
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Reseta o valor de um componente Syncfusion EJ2 para null,
 *                   limpando o campo visualmente.
 *
 * 📥 ENTRADAS     : elementId [string] - ID do elemento HTML contendo instância EJ2
 *
 * 📤 SAÍDAS       : Campo limpo no componente Syncfusion
 *
 * 🔗 CHAMADA POR  : salvarObservacao [linhas 395-398] para limpar modal após salvar
 *
 * 🔄 CHAMA        : Nenhuma função externa (acessa elemento.ej2_instances)
 *
 * 📝 OBSERVAÇÕES  : Função auxiliar defensiva. Não dispara erros se elemento não
 *                   existir ou não ter instância EJ2. Usada após sucesso de AJAX.
 ****************************************************************************************/
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

/****************************************************************************************
 * ⚡ FUNÇÃO: obterValorComponente
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Extrai valor de um componente Syncfusion EJ2. Função auxiliar
 *                   defensiva que retorna null se elemento não existir ou não tiver
 *                   instância EJ2.
 *
 * 📥 ENTRADAS     : elementId [string] - ID do elemento contendo instância Syncfusion
 *
 * 📤 SAÍDAS       : Valor do componente [any] ou null se não encontrado
 *
 * 🔗 CHAMADA POR  : filtrarEscalas [linhas 285-300], carregarObservacoesDia [linha 423],
 *                   document.ready [linhas 185-189]
 *
 * 🔄 CHAMA        : Nenhuma função externa
 *
 * 📝 OBSERVAÇÕES  : Usada extensivamente para ler valores de dropdowns e datepickers
 *                   Syncfusion nos filtros. Log de aviso se componente não encontrado.
 ****************************************************************************************/
function obterValorComponente(elementId) {
    try {
        const element = document.getElementById(elementId);
        if (element && element.ej2_instances && element.ej2_instances[0]) {
            return element.ej2_instances[0].value;
        }
        return null;
    } catch (error) {
        console.warn(`⚠️ Erro ao obter valor do componente ${elementId}:`, error);
        return null;
    }
}

// ===================================================================
// PREENCHER MODAL DE VISUALIZAÇÃO
// ===================================================================

/****************************************************************************************
 * ⚡ FUNÇÃO: preencherModalVisualizacao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Popula template do modal de visualização com dados completos da
 *                   escala: motorista, veículo, horários, turno, requisitante, observações.
 *                   Exibe alertas especiais para cobertura/cobrindo de motoristas.
 *
 * 📥 ENTRADAS     : dados [object] - DTO da escala com propriedades detalhadas:
 *                   - nomeMotorista, fotoBase64, statusMotorista, dataEscala
 *                   - horaInicio, horaFim, nomeTurno, placa, lotacao
 *                   - nomeServico, nomeRequisitante, observacoes
 *                   - nomeMotoristaCobertor, motivoCobertura (se indisponível)
 *                   - nomeMotoristaCobrindo, motivoCoberturaCobrindo (se cobrindo)
 *
 * 📤 SAÍDAS       : Modal com campos HTML preenchidos, badges de status estilizadas
 *
 * 🔗 CHAMADA POR  : visualizarEscala.success [linha 45]
 *
 * 🔄 CHAMA        : getStatusClass(), Alerta.TratamentoErroComLinha(),
 *                   manipulação direta de getElementById().textContent
 *
 * 📝 OBSERVAÇÕES  : Exibe divisões especiais (divCobertura, divCobrindo) conforme status.
 *                   Formata datas e horários para localidade pt-BR. Imagem padrão
 *                   (/images/default-driver.png) se sem foto. Link de edição usa ID da escala.
 ****************************************************************************************/
function preencherModalVisualizacao(dados) {
    try {
        console.log('📝 Preenchendo modal com:', dados);

        document.getElementById('viewNomeMotorista').textContent = dados.nomeMotorista || '-';

        var fotoElement = document.getElementById('viewFotoMotorista');
        if (dados.fotoBase64 && dados.fotoBase64 !== '') {
            fotoElement.src = 'data:image/png;base64,' + dados.fotoBase64;
        } else {
            fotoElement.src = '/images/default-driver.png';
        }

        var statusBadge = document.getElementById('viewStatusBadge');
        var statusTexto = dados.statusMotorista || 'Indefinido';
        statusBadge.textContent = statusTexto;
        statusBadge.className = 'badge mt-2 status-' + getStatusClass(statusTexto);

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

        document.getElementById('viewTurno').textContent = dados.nomeTurno || '-';
        document.getElementById('viewPlaca').textContent = dados.placa || 'Sem veículo';
        document.getElementById('viewLotacao').textContent = dados.lotacao || '-';
        document.getElementById('viewNumeroSaidas').textContent = dados.numeroSaidas || '0';
        document.getElementById('viewTipoServico').textContent = dados.nomeServico || '-';
        document.getElementById('viewRequisitante').textContent = dados.nomeRequisitante || '-';
        document.getElementById('viewObservacoes').textContent = dados.observacoes || 'Nenhuma observação registrada';

        var divCobertura = document.getElementById('divCoberturaInfo');
        var divCobrindo = document.getElementById('divCobrindoInfo');
        var statusLower = (statusTexto || '').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

        // Alerta quando motorista está INDISPONÍVEL (sendo coberto)
        if (statusLower.includes('indisponivel')) {
            divCobertura.style.display = 'block';
            document.getElementById('viewNomeCobertor').textContent = dados.nomeMotoristaCobertor || dados.nomeCobertor || 'Não definido';
            document.getElementById('viewMotivoCobertura').textContent = dados.motivoCobertura || 'Não informado';
        } else {
            divCobertura.style.display = 'none';
        }

        // Alerta quando motorista está COBRINDO outro
        if (dados.nomeMotoristaCobrindo && dados.nomeMotoristaCobrindo !== '') {
            divCobrindo.style.display = 'block';
            document.getElementById('viewNomeCoberto').textContent = dados.nomeMotoristaCobrindo;
            document.getElementById('viewMotivoCoberto').textContent = dados.motivoCoberturaCobrindo || 'Não informado';
        } else {
            divCobrindo.style.display = 'none';
        }

        document.getElementById('btnEditarEscala').href = '/Escalas/UpsertEEscala?id=' + dados.escalaDiaId;

        console.log('✅ Modal preenchido com sucesso');

    } catch (error) {
        Alerta.TratamentoErroComLinha('ListaEscala.js', 'preencherModalVisualizacao', error);
    }
}

console.log('📄 ListaEscala.js carregado');
