/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: listaeventos.js                                              ║
 * ║ 📍 LOCAL: wwwroot/js/cadastros/                                          ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Lista de Eventos (Viagens) e Detalhamento de Custos.                  ║
 * ║    • Carregamento de custos por viagem                                  ║
 * ║    • Modal de detalhamento de custos                                    ║
 * ║    • Integração com API ViagemEvento                                     ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Média (Eventos - Lista)                                    ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// ============================================
// FUNÇÃO: Carregar Detalhamento de Custos
// ============================================
function carregarDetalhamentoCustos(viagemId) {
    try {
        $.ajax({
            url: '/api/ViagemEvento/ObterDetalhamentoCustosViagem',
            type: 'GET',
            data: { viagemId: viagemId },
            success: function (response) {
                try {
                    if (response.success) {
                        const dados = response.data;

                        $('#nomeEventoDetalhes').text(
                            'Requisitante: ' + dados.nomeRequisitante ||
                                'Viagem',
                        );

                        let dataHoraInicial = '--';
                        if (dados.dataInicial) {
                            const dataInicial = formatarData(dados.dataInicial);
                            const horaInicial = dados.horaInicial
                                ? dados.horaInicial.substring(0, 5)
                                : '--';
                            dataHoraInicial = `${dataInicial} às ${horaInicial}`;
                        }
                        $('#dataHoraInicialDetalhes').text(dataHoraInicial);

                        let dataHoraFinal = '--';
                        if (dados.dataFinal) // ✅ CORRIGIDO! Minúsculo
                        {
                            const dataFinal = formatarData(dados.dataFinal);
                            const horaFinal = dados.horaFinal
                                ? dados.horaFinal.substring(0, 5)
                                : '--';
                            dataHoraFinal = `${dataFinal} às ${horaFinal}`;
                        }
                        $('#dataHoraFinalDetalhes').text(dataHoraFinal);

                        const tempoTotal =
                            dados.tempoTotalHoras !== null &&
                            dados.tempoTotalHoras !== undefined
                                ? dados.tempoTotalHoras.toFixed(2)
                                : '0.00';
                        $('#tempoTotalDetalhes').text(`${tempoTotal} horas`);

                        $('#custoMotoristaDetalhes').text(
                            formatarMoeda(dados.custoMotorista || 0),
                        );
                        $('#custoVeiculoDetalhes').text(
                            formatarMoeda(dados.custoVeiculo || 0),
                        );
                        $('#custoCombustivelDetalhes').text(
                            formatarMoeda(dados.custoCombustivel || 0),
                        );
                        $('#custoTotalDetalhes').text(
                            formatarMoeda(dados.custoTotal || 0),
                        );
                    } else {
                        AppToast.show(
                            'Vermelho',
                            response.message ||
                                'Erro ao carregar detalhamento de custos',
                            3000,
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEventos.cshtml',
                        'carregarDetalhamentoCustos.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                try {
                    console.error(
                        'Erro ao carregar detalhamento de custos:',
                        error,
                    );
                    AppToast.show(
                        'Vermelho',
                        'Erro ao carregar detalhamento de custos',
                        3000,
                    );
                } catch (err) {
                    console.error('Erro no tratamento de erro:', err);
                }
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEventos.cshtml',
            'carregarDetalhamentoCustos',
            error,
        );
    }
}

// Função para carregar o valor total do evento
function carregarValorTotalEventoModal() {
    try {
        $.ajax({
            url: '/api/evento/ObterValorTotal',
            type: 'GET',
            data: { Id: eventoIdAtual },
            success: function (response) {
                try {
                    if (response.success) {
                        $('#valorTotalEventoModal').val(
                            formatarMoeda(response.valorTotal),
                        );
                    } else {
                        $('#valorTotalEventoModal').val('R$ 0,00');
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'ListaEventos.cshtml',
                        'carregarValorTotalEventoModal.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                try {
                    console.error('Erro ao carregar valor total:', error);
                    $('#valorTotalEventoModal').val('R$ 0,00');
                } catch (err) {
                    console.error('Erro no tratamento de erro:', err);
                }
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'ListaEventos.cshtml',
            'carregarValorTotalEventoModal',
            error,
        );
    }
}
