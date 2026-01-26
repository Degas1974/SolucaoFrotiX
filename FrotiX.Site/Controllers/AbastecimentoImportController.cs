/*
 ╔══════════════════════════════════════════════════════════════════════════╗
 ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO                                            ║
 ║  Arquivo: AbastecimentoImportController.cs                               ║
 ║  Caminho: /Controllers/AbastecimentoImportController.cs                  ║
 ║  Documentado em: 2026-01-26                                              ║
 ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Data;
using FrotiX.Hubs;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: AbastecimentoImportController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Controller dedicado exclusivamente para importação de abastecimentos
     * 📥 ENTRADAS     : Arquivos Excel/CSV via FormData
     * 📤 SAÍDAS       : JSON com resultado da importação
     * 🔗 CHAMADA POR  : Frontend de importação de abastecimentos
     * 🔄 CHAMA        : AbastecimentoController (métodos internos de importação)
     * 📦 DEPENDÊNCIAS : IUnitOfWork, IHubContext, FrotiXDbContext, Logger
     * --------------------------------------------------------------------------------------
     * [DOC] NÃO usa [ApiController] para evitar validação automática antes do processamento
     * [DOC] Validação ocorre dentro da lógica de importação, não antes
     * [DOC] Delega processamento para AbastecimentoController via instância interna
     ****************************************************************************************/
    /// <summary>
    /// Controller dedicado para importação de abastecimentos.
    /// NÃO usa [ApiController] para evitar validação automática antes do processamento.
    /// </summary>
    [Route("api/Abastecimento")]
    public class AbastecimentoImportController : ControllerBase
    {
        private readonly ILogger<AbastecimentoImportController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ImportacaoHub> _hubContext;
        private readonly FrotiXDbContext _context;

        public AbastecimentoImportController(
            ILogger<AbastecimentoImportController> logger,
            IWebHostEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork,
            IHubContext<ImportacaoHub> hubContext,
            FrotiXDbContext context)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _context = context;
        }

        /// <summary>
        /// Endpoint de importação dual (CSV + XLSX) SEM validação automática.
        /// Chama o método interno do AbastecimentoController.
        /// </summary>
        [Route("ImportarDual")]
        [HttpPost]
        public async Task<ActionResult> ImportarDual()
        {
            // Criar instância do controller principal com as mesmas dependências
            var mainController = new AbastecimentoController(
                _logger as ILogger<AbastecimentoController>,
                _hostingEnvironment,
                _unitOfWork,
                _hubContext,
                _context
            );

            // Copiar o contexto HTTP para que Request, Response, etc funcionem
            mainController.ControllerContext = this.ControllerContext;

            // Chamar o método interno de importação (sem validação automática)
            return await mainController.ImportarDualInternal();
        }
    }
}
