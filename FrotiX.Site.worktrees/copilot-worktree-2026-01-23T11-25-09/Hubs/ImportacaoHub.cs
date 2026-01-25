/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HUBS SIGNALR - IMPORTAÇÃO DE PLANILHAS EM TEMPO REAL                               #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FrotiX.Hubs
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ImportacaoHub                                                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Hub SignalR para feedback em tempo real durante importação de planilhas  ║
    /// ║    Excel/CSV de abastecimentos. Envia progresso com 3 barras simultâneas.   ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA:                                                              ║
    /// ║    UX essencial. Importação pode levar minutos (milhares de linhas). Usuario║
    /// ║    precisa ver progresso detalhado: leitura XLSX → conversão CSV → BD.      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📡 EVENTOS ENVIADOS:                                                         ║
    /// ║    • Conectado → Confirmação com ConnectionId                                ║
    /// ║    • ProgressoAtualizado → Objeto ProgressoImportacao com % e detalhes       ║
    /// ║    • ResumoDisponivel → Estatísticas da planilha lida                        ║
    /// ║    • ImportacaoConcluida/Erro → Status final                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: IMPORTAÇÃO - Processo assíncrono de longa duração                 ║
    /// ║    • Arquivos relacionados: ImportacaoController, ImportacaoService         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class ImportacaoHub : Hub
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnConnectedAsync                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Conecta cliente ao hub e envia ConnectionId.                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public override async Task OnConnectedAsync()
        {
            try
            {
                // [AJAX] Estabelece conexão WebSocket
                await base.OnConnectedAsync();
                
                // [DADOS] Retorna ConnectionId para o cliente armazenar
                await Clients.Caller.SendAsync("Conectado", Context.ConnectionId);
            }
            catch (Exception error)
            {
                // [REGRA] Todo erro em Hub deve ser tratado
                Alerta.TratamentoErroComLinha("ImportacaoHub.cs", "OnConnectedAsync", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnDisconnectedAsync                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Desconecta cliente do hub de forma limpa.                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                // [AJAX] Encerra conexão WebSocket
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ImportacaoHub.cs", "OnDisconnectedAsync", error);
            }
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ProgressoImportacao (DTO)                                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Data Transfer Object enviado ao frontend com progresso detalhado da       ║
    /// ║    importação. Suporta 3 barras de progresso simultâneas.                    ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class ProgressoImportacao
    {
        public int Porcentagem { get; set; }
        public string Etapa { get; set; }
        public string Detalhe { get; set; }
        public int LinhaAtual { get; set; }
        public int TotalLinhas { get; set; }

        // Resumo da planilha (enviado após leitura)
        public bool ResumoDisponivel { get; set; }
        public int TotalRegistros { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public int RegistrosGasolina { get; set; }
        public int RegistrosDiesel { get; set; }
        public int RegistrosOutros { get; set; }

        // Progresso detalhado por etapa (3 barras)
        public int XlsxAtual { get; set; }
        public int XlsxTotal { get; set; }
        public int CsvAtual { get; set; }
        public int CsvTotal { get; set; }
        public int ProcessAtual { get; set; }
        public int ProcessTotal { get; set; }
    }
}
