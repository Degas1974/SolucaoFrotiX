using FrotiX.Data;
using FrotiX.Hubs;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using FrotiX.Helpers;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  IMPORTAÇÃO DE ABASTECIMENTOS (TRANSACIONAL)                                        #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: AbastecimentoImportController                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Proxy para importação massiva de abastecimentos.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Abastecimento                                         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/Abastecimento")]
    public class AbastecimentoImportController : ControllerBase
    {
        private readonly ILogger<AbastecimentoImportController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ImportacaoHub> _hubContext;
        private readonly FrotiXDbContext _context;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoImportController (Construtor)                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Injeta dependências para importação de abastecimentos.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • logger, hostingEnvironment, unitOfWork                                 ║
        /// ║    • hubContext, context, log                                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public AbastecimentoImportController(
            ILogger<AbastecimentoImportController> logger,
            IWebHostEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork,
            IHubContext<ImportacaoHub> hubContext,
            FrotiXDbContext context,
            ILogService log
        )
        {
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _hubContext = hubContext;
                _context = context;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoImportController.cs", "Constructor", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ImportarDual                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Endpoint de importação dual (CSV + XLSX) usando bypass do controller      ║
        /// ║    principal para evitar validações globais automáticas.                     ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite processamento massivo controlado mantendo compatibilidade         ║
        /// ║    com a lógica interna de importação.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: status e payload da importação dual.                       ║
        /// ║    • Consumidor: UI de Importação de Abastecimentos.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • AbastecimentoController.ImportarDualInternal() → lógica central.        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento padronizado.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Abastecimento/ImportarDual                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Módulo de Abastecimentos                                ║
        /// ║    • Arquivos relacionados: Controllers/AbastecimentoController.cs           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ImportarDual")]
        [HttpPost]
        public async Task<ActionResult> ImportarDual()
        {
            try
            {
                // [REGRA] Hack para usar lÃ³gica do AbastecimentoController sem validaÃ§Ãµes globais
                var mainController = new AbastecimentoController(
                    _logger as ILogger<AbastecimentoController>,
                    _hostingEnvironment,
                    _unitOfWork,
                    _hubContext,
                    _context,
                    _log
                );

                // [LOGICA] Copiar Contexto HTTP
                mainController.ControllerContext = this.ControllerContext;

                // Chamar o mÃ©todo interno de importaÃ§Ã£o (sem validaÃ§Ã£o automÃ¡tica)
                return await mainController.ImportarDualInternal();
            }
            catch (Exception error)
            {
                _log.Error("Erro ao processar importaÃ§Ã£o dual via proxy", error, "AbastecimentoImportController.cs", "ImportarDual");
                Alerta.TratamentoErroComLinha("AbastecimentoImportController.cs", "ImportarDual", error);
                return StatusCode(500, new { message = "Erro ao processar importaÃ§Ã£o dual" });
            }
        }
    }
}

