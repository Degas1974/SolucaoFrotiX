/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ FichaEscalas.cshtml.cs                                                                                ║
 * ║ PageModel para visualização de fichas de escalas - exibe detalhes de escalas por data                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    FichaEscalas.cshtml.cs                                                                    ║
 * ║ @local      Pages/Escalas/                                                                            ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Exibição de fichas detalhadas das escalas de motoristas e veículos                       ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Repository.IRepository;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Escalas
{
    public class FichaEscalasModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public FichaEscalasModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FichaEscalas.cshtml.cs", "Construtor", error);
            }
        }

        // ===================================================================
        // LISTA DE ESCALAS
        // ===================================================================
        
        public List<ViewEscalasCompletas> Escalas { get; set; } = new List<ViewEscalasCompletas>();
        
        // Dicionário para identificar motoristas que estão cobrindo outros
        public Dictionary<Guid, string> MotoristasCobrindo { get; set; } = new Dictionary<Guid, string>();

        // ===================================================================
        // PROPRIEDADES DE FILTRO
        // ===================================================================
        
        [BindProperty(SupportsGet = true)]
        public DateTime? DataEscala { get; set; }

        // ===================================================================
        // ONGET
        // ===================================================================

        public IActionResult OnGet()
        {
            try
            {
                // Define data padrão se não fornecida
                if (!DataEscala.HasValue)
                {
                    DataEscala = DateTime.Today;
                }

                CarregarEscalas();
                CarregarMotoristasCobrindo();

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FichaEscalas.cshtml.cs", "OnGet", error);
                TempData["erro"] = $"Erro ao carregar ficha de escalas: {error.Message}";
                Escalas = new List<ViewEscalasCompletas>();
                return Page();
            }
        }

        // ===================================================================
        // CARREGAR ESCALAS
        // ===================================================================

        private void CarregarEscalas()
        {
            try
            {
                // Buscar escalas da data
                var query = _unitOfWork.ViewEscalasCompletas.GetAll();

                // Filtrar por data
                if (DataEscala.HasValue)
                {
                    query = query.Where(e => e.DataEscala.Date == DataEscala.Value.Date);
                }

                // Ordenar por turno, tipo serviço e hora
                Escalas = query
                    .OrderBy(e => e.NomeTurno == "Matutino" ? 1 : e.NomeTurno == "Vespertino" ? 2 : 3)
                    .ThenBy(e => e.NomeServico == "Serviços Gerais" ? 1 : 2)
                    .ThenBy(e => e.HoraInicio)
                    .ThenBy(e => e.NomeMotorista)
                    .ToList();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FichaEscalas.cshtml.cs", "CarregarEscalas", error);
                Escalas = new List<ViewEscalasCompletas>();
            }
        }

        // ===================================================================
        // CARREGAR MOTORISTAS QUE ESTÃO COBRINDO
        // ===================================================================
        
        private void CarregarMotoristasCobrindo()
        {
            try
            {
                if (!DataEscala.HasValue) return;
                
                var dataFiltro = DataEscala.Value.Date;
                
                // Buscar coberturas ativas para esta data
                var coberturas = _unitOfWork.CoberturaFolga.GetAll(
                    c => c.Ativo &&
                         c.DataInicio.Date <= dataFiltro &&
                         c.DataFim.Date >= dataFiltro
                ).ToList();

                foreach (var cobertura in coberturas)
                {
                    if (cobertura.MotoristaCoberturaId != Guid.Empty)
                    {
                        // Buscar nome do motorista que está de folga/férias
                        var motoristaTitular = _unitOfWork.Motorista.GetFirstOrDefault(
                            m => m.MotoristaId == cobertura.MotoristaFolgaId
                        );

                        if (motoristaTitular != null && !MotoristasCobrindo.ContainsKey(cobertura.MotoristaCoberturaId))
                        {
                            MotoristasCobrindo[cobertura.MotoristaCoberturaId] = motoristaTitular.Nome ?? "";
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FichaEscalas.cshtml.cs", "CarregarMotoristasCobrindo", error);
            }
        }
        
        // ===================================================================
        // MÉTODO AUXILIAR - VERIFICAR SE É COBERTOR
        // ===================================================================
        
        public bool EhCobertor(Guid? motoristaId)
        {
            if (!motoristaId.HasValue) return false;
            return MotoristasCobrindo.ContainsKey(motoristaId.Value);
        }
        
        public string ObterNomeTitular(Guid? motoristaId)
        {
            if (!motoristaId.HasValue) return "";
            return MotoristasCobrindo.TryGetValue(motoristaId.Value, out var nome) ? nome : "";
        }
    }
}
