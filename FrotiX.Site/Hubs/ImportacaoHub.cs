/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ImportacaoHub.cs                                                                      ║
   ║ 📂 CAMINHO: Hubs/                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Hub SignalR para progresso de importação de planilhas. Inclui DTO de progresso.                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OnConnectedAsync()                                                                           ║
   ║    • OnDisconnectedAsync(Exception exception)                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Microsoft.AspNetCore.SignalR, Alerta                                              ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FrotiX.Hubs
{
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ImportacaoHub                                                                     │
    // │ 📦 HERDA DE: Hub                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Enviar progresso em tempo real para importações de planilhas via SignalR.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Clientes SignalR / Pipeline Hub
    // ➡️ CHAMA       : Clients.Caller.SendAsync(), Alerta.TratamentoErroComLinha()
    //
    public class ImportacaoHub : Hub
    {
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnConnectedAsync                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : base.OnConnectedAsync(), Clients.Caller.SendAsync()                  │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Registrar a conexão e notificar o cliente com o ConnectionId.
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de conexão.
        //
        // Returns: Task de conexão do SignalR.
        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();
                await Clients.Caller.SendAsync("Conectado", Context.ConnectionId);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ImportacaoHub.cs", "OnConnectedAsync", error);
            }
        }

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnDisconnectedAsync                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline SignalR                                                     │
        // │    ➡️ CHAMA       : base.OnDisconnectedAsync()                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        //
        // 🎯 OBJETIVO:
        // Executar rotina padrão ao desconectar o cliente do hub.
        //
        // 📥 PARÂMETROS:
        // exception - Exceção gerada durante a desconexão (se houver).
        //
        // 📤 RETORNO:
        // Task - Operação assíncrona de desconexão.
        //
        // Param exception: Exceção gerada durante a desconexão (se houver).
        // Returns: Task de desconexão do SignalR.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ImportacaoHub.cs", "OnDisconnectedAsync", error);
            }
        }
    }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ProgressoImportacao                                                               │
    // │ 📦 TIPO: DTO                                                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Transportar informações de progresso da importação para o cliente.
    //
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
