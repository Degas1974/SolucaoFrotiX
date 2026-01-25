using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Helpers;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Pages.Escalas
{
    public class UpsertEEscalaModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertEEscalaModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ===================================================================
        // PROPRIEDADES DA ESCALA (com BindProperty para evitar ambiguidade)
        // ===================================================================

        [BindProperty]
        public Guid EscalaDiaId { get; set; }

        [BindProperty]
        public Guid? MotoristaId { get; set; }

        [BindProperty]
        public Guid? VeiculoId { get; set; }

        [BindProperty]
        public Guid TipoServicoId { get; set; }

        [BindProperty]
        public Guid TurnoId { get; set; }

        [BindProperty]
        public DateTime DataEscala { get; set; }

        [BindProperty]
        public string HoraInicio { get; set; } = "06:00";

        [BindProperty]
        public string HoraFim { get; set; } = "18:00";

        [BindProperty]
        public string? Lotacao { get; set; }

        [BindProperty]
        public int NumeroSaidas { get; set; } = 0;

        [BindProperty]
        public string StatusMotorista { get; set; } = "Disponível";

        [BindProperty]
        public Guid? RequisitanteId { get; set; }

        [BindProperty]
        public string? Observacoes { get; set; }

        // ===================================================================
        // STATUS DOS MOTORISTAS (checkboxes)
        // ===================================================================

        [BindProperty]
        public bool MotoristaIndisponivel { get; set; }

        [BindProperty]
        public bool MotoristaEconomildo { get; set; }

        [BindProperty]
        public bool MotoristaEmServico { get; set; }

        [BindProperty]
        public bool MotoristaReservado { get; set; }

        // ===================================================================
        // COBERTURA (caso indisponível)
        // ===================================================================

        [BindProperty]
        public Guid? CoberturaId { get; set; }

        [BindProperty]
        public Guid? MotoristaTitularId { get; set; }

        [BindProperty]
        public Guid? MotoristaCobertorId { get; set; }

        [BindProperty]
        public string? CategoriaIndisponibilidade { get; set; }

        [BindProperty]
        public string? NomeMotoristaCobertor { get; set; }

        [BindProperty]
        public string? NomeMotoristaTitular { get; set; }

        [BindProperty]
        public DateTime? DataInicioIndisponibilidade { get; set; }

        [BindProperty]
        public DateTime? DataFimIndisponibilidade { get; set; }

        // ===================================================================
        // DIAS DA SEMANA (para escalas futuras)
        // ===================================================================

        [BindProperty]
        public bool Segunda { get; set; }

        [BindProperty]
        public bool Terca { get; set; }

        [BindProperty]
        public bool Quarta { get; set; }

        [BindProperty]
        public bool Quinta { get; set; }

        [BindProperty]
        public bool Sexta { get; set; }

        [BindProperty]
        public bool Sabado { get; set; }

        [BindProperty]
        public bool Domingo { get; set; }

        // ===================================================================
        // LISTAS PARA DROPDOWNS (não precisam de BindProperty - são apenas leitura)
        // ===================================================================

        public IEnumerable<SelectListItem> MotoristaList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> VeiculoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TipoServicoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TurnoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> RequisitanteList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> LotacaoList { get; set; } = new List<SelectListItem>();

        // ===================================================================
        // DADOS PARA EXIBIÇÃO (não precisam de BindProperty - são apenas leitura)
        // ===================================================================

        public string? NomeMotorista { get; set; }
        public string? DescricaoVeiculo { get; set; }

        // ===================================================================
        // MÉTODO GET
        // ===================================================================

        public IActionResult OnGet(Guid? id)
        {
            try
            {
                DataEscala = DateTime.Today;
                StatusMotorista = "Disponível";

                if (id.HasValue && id.Value != Guid.Empty)
                {
                    // Modo Edição - carregar usando ViewEscalasCompletas
                    var escalaView = _unitOfWork.ViewEscalasCompletas.GetFirstOrDefault(
                        e => e.EscalaDiaId == id.Value
                    );

                    if (escalaView == null)
                    {
                        TempData["erro"] = "Escala não encontrada.";
                        return RedirectToPage("./ListaEscala");
                    }

                    // Mapear da View para o PageModel
                    EscalaDiaId = escalaView.EscalaDiaId ?? Guid.Empty;
                    MotoristaId = escalaView.MotoristaId;
                    VeiculoId = escalaView.VeiculoId ?? Guid.Empty;
                    TipoServicoId = escalaView.TipoServicoId ?? Guid.Empty;
                    TurnoId = escalaView.TurnoId ?? Guid.Empty;
                    DataEscala = escalaView.DataEscala;
                    HoraInicio = escalaView.HoraInicio ?? "06:00";
                    HoraFim = escalaView.HoraFim ?? "18:00";
                    Lotacao = escalaView.Lotacao;
                    NumeroSaidas = escalaView.NumeroSaidas;
                    StatusMotorista = escalaView.StatusMotorista ?? "Disponível";
                    RequisitanteId = escalaView.RequisitanteId;
                    Observacoes = escalaView.Observacoes;

                    // Dados para exibição
                    NomeMotorista = escalaView.NomeMotorista;
                    DescricaoVeiculo = escalaView.VeiculoDescricao;

                    // Marcar checkboxes baseado no status
                    MotoristaIndisponivel = StatusMotorista == "Indisponível";
                    MotoristaEconomildo = StatusMotorista == "Economildo";
                    MotoristaEmServico = StatusMotorista == "Em Serviço";
                    MotoristaReservado = StatusMotorista == "Reservado para Serviço";

                    if (StatusMotorista == "Indisponível")
                    {
                        CoberturaId = escalaView.CoberturaId;
                        MotoristaCobertorId = escalaView.MotoristaCoberturaId;
                        MotoristaTitularId = escalaView.MotoristaFolgaId;

                        // Dados de exibição da indisponibilidade
                        NomeMotoristaCobertor = escalaView.NomeMotoristaCobertor;
                        NomeMotoristaTitular = escalaView.NomeMotoristaTitular;
                        CategoriaIndisponibilidade = escalaView.MotivoCobertura;
                        DataInicioIndisponibilidade = escalaView.DataInicio;
                        DataFimIndisponibilidade = escalaView.DataFim;
                    }

                    // =====================================================
                    // CARREGAR DIAS DA SEMANA BASEADO NAS ESCALAS FUTURAS
                    // =====================================================
                    if (MotoristaId.HasValue)
                    {
                        var escalasFuturas = _unitOfWork.ViewEscalasCompletas
                            .GetAll(e => e.MotoristaId == MotoristaId.Value &&
                                        e.DataEscala >= DateTime.Today &&
                                        e.StatusMotorista != "Indisponível")
                            .Select(e => e.DataEscala.DayOfWeek)
                            .Distinct()
                            .ToList();

                        Segunda = escalasFuturas.Contains(DayOfWeek.Monday);
                        Terca = escalasFuturas.Contains(DayOfWeek.Tuesday);
                        Quarta = escalasFuturas.Contains(DayOfWeek.Wednesday);
                        Quinta = escalasFuturas.Contains(DayOfWeek.Thursday);
                        Sexta = escalasFuturas.Contains(DayOfWeek.Friday);
                        Sabado = escalasFuturas.Contains(DayOfWeek.Saturday);
                        Domingo = escalasFuturas.Contains(DayOfWeek.Sunday);

                        // Se não tem escalas futuras, usar padrão seg-sex
                        if (!escalasFuturas.Any())
                        {
                            Segunda = true;
                            Terca = true;
                            Quarta = true;
                            Quinta = true;
                            Sexta = true;
                            Sabado = false;
                            Domingo = false;
                        }
                    }
                }

                CarregarDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UpsertEEscalaModel.cs", "OnGet", ex);
                TempData["erro"] = $"Erro ao carregar escala: {ex.Message}";
                return RedirectToPage("./ListaEscala");
            }
        }

        // ===================================================================
        // MÉTODOS AUXILIARES
        // ===================================================================

        private void CarregarDropdowns()
        {
            try
            {
                // Motoristas
                var todosMotoristas = _unitOfWork.ViewMotoristas.GetAll().ToList();
                var motoristasAtivos = todosMotoristas
                    .Where(m => m.Status == true)
                    .OrderBy(m => m.Nome)
                    .ToList();

                MotoristaList = motoristasAtivos.Select(m => new SelectListItem
                {
                    Value = m.MotoristaId.ToString(),
                    Text = m.Nome ?? "Sem nome"
                }).ToList();

                // Veículos - USA ViewVeiculos
                var todosVeiculos = _unitOfWork.ViewVeiculos.GetAll().ToList();
                var veiculosAtivos = todosVeiculos
                    .Where(v => v.Status == true)
                    .OrderBy(v => v.Placa)
                    .ToList();

                VeiculoList = veiculosAtivos.Select(v => new SelectListItem
                {
                    Value = v.VeiculoId.ToString(),
                    Text = v.VeiculoCompleto ?? v.Placa ?? "Sem identificação"
                }).ToList();

                // Tipos de Serviço
                var todosTipos = _unitOfWork.TipoServico.GetAll().ToList();
                var tiposAtivos = todosTipos.Where(t => t.Ativo).ToList();

                // Filtrar apenas Economildo e Serviços Gerais
                var tiposFiltrados = tiposAtivos
                    .Where(t => t.NomeServico == "Economildo" || t.NomeServico == "Serviços Gerais")
                    .OrderBy(t => t.NomeServico)
                    .ToList();

                TipoServicoList = tiposFiltrados.Select(t => new SelectListItem
                {
                    Value = t.TipoServicoId.ToString(),
                    Text = t.NomeServico ?? "Sem nome"
                }).ToList();

                // Turnos
                var todosTurnos = _unitOfWork.Turno.GetAll().ToList();
                var turnosAtivos = todosTurnos.Where(t => t.Ativo).ToList();

                // Filtrar apenas Matutino, Vespertino, Noturno
                var turnosFiltrados = turnosAtivos
                    .Where(t => t.NomeTurno == "Matutino" ||
                               t.NomeTurno == "Vespertino" ||
                               t.NomeTurno == "Noturno")
                    .OrderBy(t => t.NomeTurno)
                    .ToList();

                TurnoList = turnosFiltrados.Select(t => new SelectListItem
                {
                    Value = t.TurnoId.ToString(),
                    Text = t.NomeTurno ?? "Sem nome"
                }).ToList();

                // Requisitantes
                var requisitantes = _unitOfWork.Requisitante.GetAll()
                        .OrderBy(r => r.Nome)
                        .ToList();

                RequisitanteList = requisitantes.Select(r => new SelectListItem
                {
                    Value = r.RequisitanteId.ToString(),
                    Text = r.Nome ?? "Sem nome"
                }).ToList();

                // Lotações
                LotacaoList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Selecione..." },
                    new SelectListItem { Value = "Aeroporto", Text = "Aeroporto" },
                    new SelectListItem { Value = "Rodoviária", Text = "Rodoviária" },
                    new SelectListItem { Value = "PGR", Text = "PGR" },
                    new SelectListItem { Value = "Cefor", Text = "Cefor" },
                    new SelectListItem { Value = "Setor de Obras", Text = "Setor de Obras" },
                    new SelectListItem { Value = "Outros", Text = "Outros" }
                };
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UpsertEEscalaModel.cs", "CarregarDropdowns", ex);
                TempData["erro"] = $"Erro ao carregar dropdowns: {ex.Message}";
            }
        }
    }
}
