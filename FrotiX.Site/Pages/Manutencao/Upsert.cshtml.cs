// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Upsert.cshtml.cs (Manutencao)                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para criação/edição de ordens de serviço de manutenção.           ║
// ║ Utiliza IMemoryCache para dropdowns otimizados.                             ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • [Consumes("application/json")], [IgnoreAntiforgeryToken]                  ║
// ║ • Injeção: IUnitOfWork, ILogger, IWebHostEnvironment, INotyfService, Cache  ║
// ║ • [BindProperty] ManutencaoObj - ManutencaoViewModel                        ║
// ║ • Usa DTOs: MotoristaDataComFoto, VeiculoData, VeiculoReservaData           ║
// ║                                                                              ║
// ║ MÉTODOS AUXILIARES:                                                           ║
// ║ • PreencheListaMotoristasFromCache - Lista de motoristas do cache           ║
// ║ • PreencheListaVeiculosFromCache - Lista de veículos do cache               ║
// ║ • PreencheListaVeiculosReservaFromCache - Veículos reserva do cache         ║
// ║ • SetViewModel - Inicializa ManutencaoViewModel                             ║
// ║                                                                              ║
// ║ HANDLERS:                                                                     ║
// ║ • OnGetAsync(id) - Carrega OS existente ou nova + listas do cache           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-02-04 | LOTE: 19 | v2.0 - Cache Unificado              ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Infrastructure;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoristaDataComFoto = FrotiX.Models.DTO.MotoristaDataComFoto;
using VeiculoData = FrotiX.Models.DTO.VeiculoData;
using VeiculoReservaData = FrotiX.Models.DTO.VeiculoReservaData;

namespace FrotiX.Pages.Manutencao
{
    [Consumes("application/json")]
    [IgnoreAntiforgeryToken]
    public class UpsertModel :PageModel
    {
        public static Guid ManutencaoId;

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<IndexModel> _logger;
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public UpsertModel(
            IUnitOfWork unitOfWork ,
            ILogger<IndexModel> logger ,
            IWebHostEnvironment hostingEnvironment ,
            INotyfService notyf ,
            IMemoryCache cache
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _notyf = notyf;
                _cache = cache;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "UpsertModel" , error);
            }
        }

        [BindProperty]
        public ManutencaoViewModel ManutencaoObj
        {
            get; set;
        }

        private void PreencheListaMotoristasFromCache()
        {
            try
            {
                // [CACHE] Motoristas com foto (MotoristaDataComFoto)
                var ds = _cache.Get<List<MotoristaDataComFoto>>(CacheKeys.Motoristas) ?? new List<MotoristaDataComFoto>();
                ViewData["dataMotorista"] = ds;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaMotoristasFromCache" , error);
                return;
            }
        }

        private void PreencheListaVeiculosFromCache()
        {
            try
            {
                // [CACHE] Veículos Manutenção (ViewVeiculosManutencao)
                var ds = _cache.Get<List<VeiculoData>>(CacheKeys.VeiculosManutencao) ?? new List<VeiculoData>();
                ViewData["dataVeiculo"] = ds;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaVeiculosFromCache" , error);
                return;
            }
        }

        private void PreencheListaVeiculosReservaFromCache()
        {
            try
            {
                // [CACHE] Veículos Reserva (VeiculoReservaData)
                var ds = _cache.Get<List<VeiculoReservaData>>(CacheKeys.VeiculosReserva) ?? new List<VeiculoReservaData>();
                ViewData["dataVeiculoReserva"] = ds;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "PreencheListaVeiculosReservaFromCache" , error);
                ViewData["dataVeiculoReserva"] = new List<VeiculoReservaData>();
            }
        }

        private void SetViewModel()
        {
            try
            {
                ManutencaoObj = new ManutencaoViewModel { Manutencao = new Models.Manutencao() };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "SetViewModel" , error);
                return;
            }
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                SetViewModel();

                if (id != Guid.Empty)
                {
                    ManutencaoObj.Manutencao = _unitOfWork.Manutencao.GetFirstOrDefault(u => u.ManutencaoId == id);
                    if (ManutencaoObj?.Manutencao == null)
                        return NotFound();
                }

                PreencheListaMotoristasFromCache();
                PreencheListaVeiculosFromCache();
                PreencheListaVeiculosReservaFromCache();

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGetAsync" , error);
                return Page();
            }
        }
    }
}
