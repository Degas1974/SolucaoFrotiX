/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UpsertEEscala.cshtml.cs (Pages/Escalas)                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para visualização/edição de escalas existentes. Carrega dados da escala a partir da View      ║
 * ║ consolidada ViewEscalasCompletas e exibe informações de cobertura quando motorista está indisponível.   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES                                                                                              ║
 * ║ • EscalaDiaId - Identificador da escala                                                                  ║
 * ║ • MotoristaId, VeiculoId, TipoServicoId, TurnoId - Configuração da escala                                ║
 * ║ • DataEscala, HoraInicio, HoraFim - Data e horários                                                      ║
 * ║ • StatusMotorista - Disponível, Economildo, Em Serviço, Reservado, Indisponível                          ║
 * ║ • CoberturaId, MotoristaTitularId, MotoristaCobertorId - Dados de cobertura                              ║
 * ║ • NomeMotoristaCobertor, NomeMotoristaTitular - Nomes para exibição                                      ║
 * ║ • Segunda..Domingo - Checkboxes populados baseado nas escalas futuras do motorista                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega escala da ViewEscalasCompletas pelo EscalaDiaId                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ LÓGICA DE EXIBIÇÃO                                                                                        ║
 * ║ • Mapeia campos da View para propriedades do PageModel                                                   ║
 * ║ • Marca checkboxes de status baseado no StatusMotorista                                                  ║
 * ║ • Se Indisponível: carrega dados de cobertura (cobertor, titular, motivo, período)                       ║
 * ║ • Carrega dias da semana baseado nas escalas futuras do motorista                                        ║
 * ║ • Se não há escalas futuras, define padrão seg-sex                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ MÉTODOS AUXILIARES                                                                                        ║
 * ║ • CarregarDropdowns() : Popula listas filtradas (Economildo/Serviços Gerais, Matutino/Vespertino/Noturno)║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • ViewEscalasCompletas - View consolidada com escalas + coberturas                                       ║
 * ║ • ViewMotoristas, ViewVeiculos - Views para dropdowns                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
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
        // PROPRIEDADES DA ESCALA
        // ===================================================================

       
        public Guid EscalaDiaId { get; set; }

        public Guid? MotoristaId { get; set; }

        public Guid? VeiculoId { get; set; }

        public Guid TipoServicoId { get; set; }

        public Guid TurnoId { get; set; }

        public DateTime DataEscala { get; set; }

        public string HoraInicio { get; set; } = "06:00";

        public string HoraFim { get; set; } = "18:00";

        public string? Lotacao { get; set; }

        public int NumeroSaidas { get; set; } = 0;

        public string StatusMotorista { get; set; } = "Disponível";

        public Guid? RequisitanteId { get; set; }

        public string? Observacoes { get; set; }

        //Status dos Motoristas para checkboxes

        public bool MotoristaIndisponivel { get; set; }

        public bool MotoristaEconomildo { get; set; }

        public bool MotoristaEmServico { get; set; }

        public bool MotoristaReservado { get; set; }


        // Cobertura caso indisponível
        public Guid? CoberturaId { get; set; }
        public Guid? MotoristaTitularId { get; set; }
        public Guid? MotoristaCobertorId { get; set; }
        public string? CategoriaIndisponibilidade { get; set; }
        public string? NomeMotoristaCobertor { get; set; }
        public string? NomeMotoristaTitular { get; set; }
        public DateTime? DataInicioIndisponibilidade { get; set; }

        public DateTime? DataFimIndisponibilidade { get; set; }

        // ===================================================================
        // DIAS DA SEMANA (para escalas futuras)
        // ===================================================================
        
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }

        

        // ===================================================================
        // LISTAS PARA DROPDOWNS
        // ===================================================================

        public IEnumerable<SelectListItem> MotoristaList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> VeiculoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TipoServicoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TurnoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> RequisitanteList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> LotacaoList { get; set; } = new List<SelectListItem>();
       
        // Dados para exibição
        public string? NomeMotorista { get; set; }
        public string? DescricaoVeiculo { get; set; }
        
        // ===================================================================
        // MÉTODOS GET/POST
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

                        //Dados de exibição da indisponibilidade

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

                Console.WriteLine($"[DEBUG] Total de tipos ativos: {tiposAtivos.Count}");

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

                // Status
               
            }
            catch (Exception ex)
            {
                TempData["erro"] = $"Erro ao carregar dropdowns: {ex.Message}";
            }
        }
    }
}
