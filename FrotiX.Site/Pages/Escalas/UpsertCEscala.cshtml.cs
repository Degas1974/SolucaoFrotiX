using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Repository.IRepository;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Pages.Escalas
{
    public class UpsertCEscalaModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertCEscalaModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ===================================================================
        // PROPRIEDADES DA ESCALA
        // ===================================================================

        [BindProperty]
        public Guid? MotoristaId { get; set; }

        [BindProperty]
        public Guid? VeiculoId { get; set; }

        [BindProperty]
        public Guid TipoServicoId { get; set; }

        [BindProperty]
        public Guid TurnoId { get; set; }

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

        // Status especiais
        [BindProperty]
        public bool MotoristaIndisponivel { get; set; }

        [BindProperty]
        public bool MotoristaEconomildo { get; set; }

        [BindProperty]
        public bool MotoristaEmServico { get; set; }

        [BindProperty]
        public bool MotoristaReservado { get; set; }

        // Indisponibilidade
        [BindProperty]
        public string? CategoriaIndisponibilidade { get; set; }

        [BindProperty]
        public DateTime? DataInicioIndisponibilidade { get; set; }

        [BindProperty]
        public DateTime? DataFimIndisponibilidade { get; set; }

        [BindProperty]
        public Guid? MotoristaCobertorId { get; set; }

        // ===================================================================
        // CRIAÇÃO EM MASSA - DATAS E DIAS DA SEMANA
        // ===================================================================

        [BindProperty]
        public DateTime DataInicio { get; set; }

        [BindProperty]
        public DateTime DataFim { get; set; }

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
        // LISTAS PARA DROPDOWNS
        // ===================================================================

        public IEnumerable<SelectListItem> MotoristaList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> VeiculoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TipoServicoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TurnoList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> RequisitanteList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> LotacaoList { get; set; } = new List<SelectListItem>();

        // ===================================================================
        // MÉTODOS GET/POST
        // ===================================================================

        public IActionResult OnGet()
        {
            try
            {
                DataInicio = DateTime.Today;
                DataFim = DateTime.Today.AddDays(30);
                StatusMotorista = "Disponível";

                Segunda = true;
                Terca = true;
                Quarta = true;
                Quinta = true;
                Sexta = true;
                Sabado = false;
                Domingo = false;

                CarregarDropdowns();

                Console.WriteLine($"[DEBUG] ===== RESUMO DO CARREGAMENTO =====");
                Console.WriteLine($"[DEBUG] Motoristas: {MotoristaList?.Count()}");
                Console.WriteLine($"[DEBUG] Veículos: {VeiculoList?.Count()}");
                Console.WriteLine($"[DEBUG] TiposServico: {TipoServicoList?.Count()}");
                Console.WriteLine($"[DEBUG] Turnos: {TurnoList?.Count()}");
                Console.WriteLine($"[DEBUG] =====================================");

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO CRÍTICO] OnGet: {ex.Message}");
                Console.WriteLine($"[ERRO CRÍTICO] StackTrace: {ex.StackTrace}");
                TempData["erro"] = $"Erro ao carregar página: {ex.Message}";
                return RedirectToPage("./ListaEscala");
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!MotoristaId.HasValue)
                {
                    ModelState.AddModelError("MotoristaId", "O motorista é obrigatório.");
                    CarregarDropdowns();
                    return Page();
                }

                if (!Segunda && !Terca && !Quarta && !Quinta && !Sexta && !Sabado && !Domingo)
                {
                    ModelState.AddModelError("", "Selecione pelo menos um dia da semana.");
                    CarregarDropdowns();
                    return Page();
                }

                if (DataFim < DataInicio)
                {
                    ModelState.AddModelError("DataFim", "A data fim deve ser maior ou igual à data início.");
                    CarregarDropdowns();
                    return Page();
                }

                if (!TimeSpan.TryParse(HoraInicio, out TimeSpan horaInicioTS))
                {
                    ModelState.AddModelError("HoraInicio", "Formato de hora inválido.");
                    CarregarDropdowns();
                    return Page();
                }

                if (!TimeSpan.TryParse(HoraFim, out TimeSpan horaFimTS))
                {
                    ModelState.AddModelError("HoraFim", "Formato de hora inválido.");
                    CarregarDropdowns();
                    return Page();
                }

                var statusDaEscala = DeterminarStatusDaEscala();
                var datasParaCriar = ObterDatasValidas();
                var escalasCreated = 0;
                var usuarioId = ObterUsuarioId();

                // =====================================================
                // OBTER/CRIAR ASSOCIAÇÃO DO MOTORISTA
                // ✅ CORREÇÃO: Sempre cria associação, mesmo sem veículo
                // =====================================================
                Guid? associacaoId = null;
                
                // Buscar associação existente (com ou sem veículo)
                var associacao = _unitOfWork.VAssociado.GetFirstOrDefault(
                    a => a.MotoristaId == MotoristaId.Value &&
                         a.VeiculoId == VeiculoId &&  // Pode ser NULL
                         a.Ativo);

                if (associacao == null)
                {
                    associacao = new VAssociado
                    {
                        AssociacaoId = Guid.NewGuid(),
                        MotoristaId = MotoristaId.Value,
                        VeiculoId = VeiculoId,  // Pode ser NULL (veículo não definido)
                        DataInicio = DataInicio,
                        Ativo = true,
                        DataCriacao = DateTime.Now,
                        UsuarioIdAlteracao = usuarioId
                    };
                    _unitOfWork.VAssociado.Add(associacao);
                    _unitOfWork.Save();
                }
                associacaoId = associacao.AssociacaoId;

                // =====================================================
                // CRIAR ESCALAS PARA O MOTORISTA
                // =====================================================
                foreach (var data in datasParaCriar)
                {
                    var escalaExistente = _unitOfWork.ViewEscalasCompletas
                        .GetAll()
                        .FirstOrDefault(e => e.MotoristaId == MotoristaId.Value &&
                                           e.DataEscala.Date == data.Date);

                    if (escalaExistente == null)
                    {
                        // Aplicar lógica de Lotação/Requisitante conforme status
                        string lotacaoFinal = null;
                        Guid? requisitanteFinal = null;

                        if (statusDaEscala == "Economildo")
                        {
                            lotacaoFinal = Lotacao;
                        }
                        else if (statusDaEscala == "Em Serviço")
                        {
                            requisitanteFinal = RequisitanteId;
                        }

                        var escala = new EscalaDiaria
                        {
                            EscalaDiaId = Guid.NewGuid(),
                            AssociacaoId = associacaoId,
                            TipoServicoId = TipoServicoId,
                            TurnoId = TurnoId,
                            DataEscala = data,
                            HoraInicio = horaInicioTS,
                            HoraFim = horaFimTS,
                            Lotacao = lotacaoFinal,
                            NumeroSaidas = NumeroSaidas,
                            StatusMotorista = statusDaEscala,
                            RequisitanteId = requisitanteFinal,
                            Observacoes = Observacoes,
                            Ativo = true,
                            DataCriacao = DateTime.Now,
                            UsuarioIdAlteracao = usuarioId
                        };

                        _unitOfWork.EscalaDiaria.Add(escala);
                        escalasCreated++;
                    }
                }

                _unitOfWork.Save();

                // =====================================================
                // LÓGICA DE INDISPONIBILIDADE E COBERTURA
                // =====================================================
                int escalasMarcadasIndisponiveis = 0;
                int escalasCobertorCriadas = 0;

                if (MotoristaIndisponivel && 
                    DataInicioIndisponibilidade.HasValue && 
                    DataFimIndisponibilidade.HasValue)
                {
                    var dataInicioIndisp = DataInicioIndisponibilidade.Value.Date;
                    var dataFimIndisp = DataFimIndisponibilidade.Value.Date;

                    // Obter todas as associações do motorista
                    var todasAssociacoes = _unitOfWork.VAssociado
                        .GetAll(a => a.MotoristaId == MotoristaId.Value)
                        .Select(a => a.AssociacaoId)
                        .ToList();

                    // Dicionário para guardar status original de cada dia
                    var statusPorDia = new Dictionary<DateTime, (string Status, Guid? RequisitanteId, string Lotacao)>();

                    if (todasAssociacoes.Any())
                    {
                        var escalasNoPeriodo = _unitOfWork.EscalaDiaria
                            .GetAll(e => e.AssociacaoId.HasValue &&
                                        todasAssociacoes.Contains(e.AssociacaoId.Value) &&
                                        e.DataEscala >= dataInicioIndisp &&
                                        e.DataEscala <= dataFimIndisp &&
                                        e.Ativo)
                            .ToList();

                        // Capturar status original de cada dia ANTES de marcar como Indisponível
                        foreach (var esc in escalasNoPeriodo)
                        {
                            if (!statusPorDia.ContainsKey(esc.DataEscala.Date))
                            {
                                string statusOriginalDia = esc.StatusMotorista ?? "Disponível";
                                if (statusOriginalDia == "Indisponível")
                                {
                                    statusOriginalDia = "Disponível";
                                }
                                statusPorDia[esc.DataEscala.Date] = (statusOriginalDia, esc.RequisitanteId, esc.Lotacao);
                            }
                        }

                        // Marcar todas as escalas como Indisponível
                        foreach (var esc in escalasNoPeriodo)
                        {
                            if (esc.StatusMotorista != "Indisponível")
                            {
                                esc.StatusMotorista = "Indisponível";
                                esc.DataAlteracao = DateTime.Now;
                                esc.UsuarioIdAlteracao = usuarioId;
                                _unitOfWork.EscalaDiaria.Update(esc);
                                escalasMarcadasIndisponiveis++;
                            }
                        }

                        if (escalasMarcadasIndisponiveis > 0)
                        {
                            _unitOfWork.Save();
                        }
                    }

                    // Determinar status original predominante
                    string statusOriginalPredominante = "Disponível";
                    if (statusPorDia.Any())
                    {
                        var statusMaisComum = statusPorDia.Values
                            .Where(v => v.Status != "Indisponível")
                            .GroupBy(v => v.Status)
                            .OrderByDescending(g => g.Count())
                            .FirstOrDefault()?.Key;

                        if (!string.IsNullOrEmpty(statusMaisComum))
                        {
                            statusOriginalPredominante = statusMaisComum;
                        }
                    }

                    // =====================================================
                    // CRIAR COBERTURA
                    // =====================================================
                    var cobertura = new CoberturaFolga
                    {
                        CoberturaId = Guid.NewGuid(),
                        MotoristaFolgaId = MotoristaId.Value,
                        MotoristaCoberturaId = MotoristaCobertorId ?? Guid.Empty,
                        DataInicio = dataInicioIndisp,
                        DataFim = dataFimIndisp,
                        Motivo = CategoriaIndisponibilidade,
                        StatusOriginal = statusOriginalPredominante,
                        Observacoes = "Criado via criação de escala",
                        Ativo = true,
                        DataCriacao = DateTime.Now,
                        UsuarioIdAlteracao = usuarioId
                    };

                    if (MotoristaCobertorId.HasValue && MotoristaCobertorId.Value != Guid.Empty)
                    {
                        cobertura.MotoristaCoberturaId = MotoristaCobertorId.Value;
                        _unitOfWork.CoberturaFolga.Add(cobertura);
                        _unitOfWork.Save();

                        // =====================================================
                        // CRIAR ESCALAS PARA O COBERTOR
                        // =====================================================
                        var motoristaCobertorId = MotoristaCobertorId.Value;

                        var associacoesCobertor = _unitOfWork.VAssociado
                            .GetAll(a => a.MotoristaId == motoristaCobertorId)
                            .Select(a => a.AssociacaoId)
                            .ToList();

                        Guid? associacaoCobertorId = null;

                        if (VeiculoId.HasValue && VeiculoId.Value != Guid.Empty)
                        {
                            var assocExistente = _unitOfWork.VAssociado.GetFirstOrDefault(
                                a => a.MotoristaId == motoristaCobertorId &&
                                     a.VeiculoId == VeiculoId.Value &&
                                     a.Ativo);

                            if (assocExistente != null)
                            {
                                associacaoCobertorId = assocExistente.AssociacaoId;
                            }
                            else
                            {
                                var novaAssoc = new VAssociado
                                {
                                    AssociacaoId = Guid.NewGuid(),
                                    MotoristaId = motoristaCobertorId,
                                    VeiculoId = VeiculoId.Value,
                                    DataInicio = dataInicioIndisp,
                                    Ativo = true,
                                    DataCriacao = DateTime.Now,
                                    UsuarioIdAlteracao = usuarioId
                                };
                                _unitOfWork.VAssociado.Add(novaAssoc);
                                _unitOfWork.Save();
                                associacaoCobertorId = novaAssoc.AssociacaoId;
                                associacoesCobertor.Add(novaAssoc.AssociacaoId);
                            }
                        }
                        else
                        {
                            var assocQualquer = _unitOfWork.VAssociado
                                .GetFirstOrDefault(a => a.MotoristaId == motoristaCobertorId && a.Ativo);
                            associacaoCobertorId = assocQualquer?.AssociacaoId;
                        }

                        // Criar escalas para o cobertor nos dias válidos (respeitando dias da semana)
                        for (var data = dataInicioIndisp; data <= dataFimIndisp; data = data.AddDays(1))
                        {
                            // Verificar se o dia da semana está marcado
                            if (!DiaDaSemanaEstaMarcado(data.DayOfWeek))
                                continue;

                            bool cobertorJaTemEscala = _unitOfWork.EscalaDiaria
                                .GetAll(e => e.AssociacaoId.HasValue &&
                                            associacoesCobertor.Contains(e.AssociacaoId.Value) &&
                                            e.DataEscala == data &&
                                            e.Ativo)
                                .Any();

                            if (!cobertorJaTemEscala && associacaoCobertorId.HasValue)
                            {
                                // Herdar status original do dia
                                string statusCobertor = "Disponível";
                                Guid? requisitanteCobertor = null;
                                string lotacaoCobertor = null;

                                if (statusPorDia.TryGetValue(data, out var infoOriginal))
                                {
                                    statusCobertor = infoOriginal.Status;

                                    if (infoOriginal.Status == "Em Serviço" && infoOriginal.RequisitanteId.HasValue)
                                    {
                                        requisitanteCobertor = infoOriginal.RequisitanteId;
                                    }

                                    if (infoOriginal.Status == "Economildo" && !string.IsNullOrEmpty(infoOriginal.Lotacao))
                                    {
                                        lotacaoCobertor = infoOriginal.Lotacao;
                                    }
                                }

                                var novaEscala = new EscalaDiaria
                                {
                                    EscalaDiaId = Guid.NewGuid(),
                                    AssociacaoId = associacaoCobertorId,
                                    TipoServicoId = TipoServicoId,
                                    TurnoId = TurnoId,
                                    DataEscala = data,
                                    HoraInicio = horaInicioTS,
                                    HoraFim = horaFimTS,
                                    Lotacao = lotacaoCobertor,
                                    NumeroSaidas = 0,
                                    StatusMotorista = statusCobertor,
                                    RequisitanteId = requisitanteCobertor,
                                    Observacoes = $"Cobertura ({CategoriaIndisponibilidade})",
                                    Ativo = true,
                                    DataCriacao = DateTime.Now,
                                    UsuarioIdAlteracao = usuarioId
                                };

                                _unitOfWork.EscalaDiaria.Add(novaEscala);
                                escalasCobertorCriadas++;
                            }
                        }

                        if (escalasCobertorCriadas > 0)
                        {
                            _unitOfWork.Save();
                        }
                    }
                }

                // =====================================================
                // MENSAGEM DE RETORNO
                // =====================================================
                string mensagem = $"✅ {escalasCreated} escala(s) criada(s) com sucesso!";
                if (escalasMarcadasIndisponiveis > 0)
                    mensagem += $" {escalasMarcadasIndisponiveis} escala(s) marcada(s) como indisponível.";
                if (escalasCobertorCriadas > 0)
                    mensagem += $" {escalasCobertorCriadas} escala(s) criada(s) para o cobertor.";

                TempData["sucesso"] = mensagem;
                return RedirectToPage("./ListaEscala");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] OnPost: {ex.Message}");
                Console.WriteLine($"[ERRO] StackTrace: {ex.StackTrace}");
                TempData["erro"] = $"Erro ao criar escalas: {ex.Message}";
                CarregarDropdowns();
                return Page();
            }
        }

        /// <summary>
        /// Verifica se um dia da semana está marcado nos checkboxes
        /// </summary>
        private bool DiaDaSemanaEstaMarcado(DayOfWeek dia)
        {
            return dia switch
            {
                DayOfWeek.Sunday => Domingo,
                DayOfWeek.Monday => Segunda,
                DayOfWeek.Tuesday => Terca,
                DayOfWeek.Wednesday => Quarta,
                DayOfWeek.Thursday => Quinta,
                DayOfWeek.Friday => Sexta,
                DayOfWeek.Saturday => Sabado,
                _ => false
            };
        }

        public IActionResult OnPostCriarCobertura()
        {
            try
            {
                if (!MotoristaId.HasValue || !MotoristaCobertorId.HasValue)
                {
                    ModelState.AddModelError("", "Motorista titular e cobertor são obrigatórios.");
                    CarregarDropdowns();
                    return Page();
                }

                if (!DataInicioIndisponibilidade.HasValue || !DataFimIndisponibilidade.HasValue)
                {
                    ModelState.AddModelError("", "As datas de início e fim são obrigatórias.");
                    CarregarDropdowns();
                    return Page();
                }

                if (MotoristaId.Value == MotoristaCobertorId.Value)
                {
                    ModelState.AddModelError("MotoristaCobertorId", "O motorista cobertor deve ser diferente do motorista titular.");
                    CarregarDropdowns();
                    return Page();
                }

                var usuarioId = ObterUsuarioId();

                if (CategoriaIndisponibilidade == "Férias")
                {
                    var ferias = new Ferias
                    {
                        FeriasId = Guid.NewGuid(),
                        MotoristaId = MotoristaId.Value,
                        MotoristaSubId = MotoristaCobertorId.Value,
                        DataInicio = DataInicioIndisponibilidade.Value,
                        DataFim = DataFimIndisponibilidade.Value,
                        Observacoes = Observacoes,
                        Ativo = true,
                        DataCriacao = DateTime.Now,
                        UsuarioIdAlteracao = usuarioId
                    };
                    _unitOfWork.Ferias.Add(ferias);
                }
                else
                {
                    var folgaRecesso = new FolgaRecesso
                    {
                        FolgaId = Guid.NewGuid(),
                        MotoristaId = MotoristaId.Value,
                        DataInicio = DataInicioIndisponibilidade.Value,
                        DataFim = DataFimIndisponibilidade.Value,
                        Tipo = CategoriaIndisponibilidade ?? "Folga",
                        Observacoes = Observacoes,
                        Ativo = true,
                        DataCriacao = DateTime.Now,
                        UsuarioIdAlteracao = usuarioId
                    };
                    _unitOfWork.FolgaRecesso.Add(folgaRecesso);
                }

                var cobertura = new CoberturaFolga
                {
                    CoberturaId = Guid.NewGuid(),
                    MotoristaFolgaId = MotoristaId.Value,
                    MotoristaCoberturaId = MotoristaCobertorId.Value,
                    DataInicio = DataInicioIndisponibilidade.Value,
                    DataFim = DataFimIndisponibilidade.Value,
                    Motivo = CategoriaIndisponibilidade,
                    Observacoes = Observacoes,
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    UsuarioIdAlteracao = usuarioId
                };
                _unitOfWork.CoberturaFolga.Add(cobertura);

                _unitOfWork.Save();

                TempData["sucesso"] = "Cobertura criada com sucesso!";
                return RedirectToPage("./ListaEscala");
            }
            catch (Exception ex)
            {
                TempData["erro"] = $"Erro ao criar cobertura: {ex.Message}";
                CarregarDropdowns();
                return Page();
            }
        }

        // ===================================================================
        // MÉTODOS AUXILIARES
        // ===================================================================

        private void CarregarDropdowns()
        {
            try
            {
                Console.WriteLine("[DEBUG] ===== INICIANDO CARREGAMENTO DE DROPDOWNS =====");

                // ✅ MOTORISTAS - USANDO ViewMotoristas IGUAL AOS VEÍCULOS
                try
                {
                    Console.WriteLine("[DEBUG] --- CARREGANDO MOTORISTAS ---");

                    var todosMotoristas = _unitOfWork.ViewMotoristas.GetAll().ToList();
                    var motoristasAtivos = todosMotoristas
                        .Where(m => m.Status == true)
                        .OrderBy(m => m.Nome)
                        .ToList();

                    Console.WriteLine($"[DEBUG] Motoristas ativos: {motoristasAtivos.Count}");

                    MotoristaList = motoristasAtivos.Select(m => new SelectListItem
                    {
                        Value = m.MotoristaId.ToString(),
                        Text = m.Nome ?? "Sem nome"
                    }).ToList();

                    if (!MotoristaList.Any())
                    {
                        MotoristaList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "Nenhum motorista ativo" }
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Carregar Motoristas: {ex.Message}");
                    MotoristaList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Erro ao carregar motoristas" }
                    };
                }

                // ✅ VEÍCULOS
                try
                {
                    Console.WriteLine("[DEBUG] --- CARREGANDO VEÍCULOS ---");

                    var todosVeiculos = _unitOfWork.ViewVeiculos.GetAll().ToList();
                    var veiculosAtivos = todosVeiculos
                        .Where(v => v.Status == true)
                        .OrderBy(v => v.Placa)
                        .ToList();

                    Console.WriteLine($"[DEBUG] Veículos ativos: {veiculosAtivos.Count}");

                    VeiculoList = veiculosAtivos.Select(v => new SelectListItem
                    {
                        Value = v.VeiculoId.ToString(),
                        Text = v.VeiculoCompleto ?? v.Placa ?? "Sem identificação"
                    }).ToList();

                    if (!VeiculoList.Any())
                    {
                        VeiculoList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "Nenhum veículo ativo" }
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Carregar Veículos: {ex.Message}");
                    VeiculoList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Erro ao carregar veículos" }
                    };
                }

                // ✅ TIPOS DE SERVIÇO - FILTRADO
                try
                {
                    Console.WriteLine("[DEBUG] --- CARREGANDO TIPOS DE SERVIÇO ---");

                    var todosTipos = _unitOfWork.TipoServico.GetAll().ToList();
                    var tiposAtivos = todosTipos.Where(t => t.Ativo).ToList();

                    Console.WriteLine($"[DEBUG] Total de tipos ativos: {tiposAtivos.Count}");

                    // Filtrar apenas Economildo e Serviços Gerais
                    var tiposFiltrados = tiposAtivos
                        .Where(t => t.NomeServico == "Economildo" || t.NomeServico == "Serviços Gerais")
                        .OrderBy(t => t.NomeServico)
                        .ToList();

                    Console.WriteLine($"[DEBUG] Tipos após filtro: {tiposFiltrados.Count}");

                    if (tiposFiltrados.Any())
                    {
                        TipoServicoList = tiposFiltrados.Select(t => new SelectListItem
                        {
                            Value = t.TipoServicoId.ToString(),
                            Text = t.NomeServico ?? "Sem nome"
                        }).ToList();

                        Console.WriteLine($"[DEBUG] ✅ Tipos: {string.Join(", ", tiposFiltrados.Select(t => t.NomeServico))}");
                    }
                    else
                    {
                        Console.WriteLine($"[AVISO] ❌ 'Economildo' ou 'Serviços Gerais' não encontrados!");
                        Console.WriteLine($"[AVISO] Execute os scripts SQL de limpeza!");
                        TipoServicoList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "Configure: Economildo e Serviços Gerais" }
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Carregar Tipos: {ex.Message}");
                    TipoServicoList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Erro ao carregar tipos" }
                    };
                }

                // ✅ TURNOS - FILTRADO
                try
                {
                    Console.WriteLine("[DEBUG] --- CARREGANDO TURNOS ---");

                    var todosTurnos = _unitOfWork.Turno.GetAll().ToList();
                    var turnosAtivos = todosTurnos.Where(t => t.Ativo).ToList();

                    Console.WriteLine($"[DEBUG] Total de turnos ativos: {turnosAtivos.Count}");

                    // Filtrar apenas Matutino, Vespertino, Noturno
                    var turnosFiltrados = turnosAtivos
                        .Where(t => t.NomeTurno == "Matutino" ||
                                   t.NomeTurno == "Vespertino" ||
                                   t.NomeTurno == "Noturno")
                        .OrderBy(t => t.NomeTurno)
                        .ToList();

                    Console.WriteLine($"[DEBUG] Turnos após filtro: {turnosFiltrados.Count}");

                    if (turnosFiltrados.Any())
                    {
                        TurnoList = turnosFiltrados.Select(t => new SelectListItem
                        {
                            Value = t.TurnoId.ToString(),
                            Text = t.NomeTurno ?? "Sem nome"
                        }).ToList();

                        Console.WriteLine($"[DEBUG] ✅ Turnos: {string.Join(", ", turnosFiltrados.Select(t => t.NomeTurno))}");
                    }
                    else
                    {
                        Console.WriteLine($"[AVISO] ❌ 'Matutino', 'Vespertino' ou 'Noturno' não encontrados!");
                        Console.WriteLine($"[AVISO] Execute os scripts SQL de limpeza!");
                        TurnoList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "Configure: Matutino, Vespertino, Noturno" }
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Carregar Turnos: {ex.Message}");
                    TurnoList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Erro ao carregar turnos" }
                    };
                }

                // ✅ REQUISITANTES
                try
                {
                    Console.WriteLine("[DEBUG] --- CARREGANDO REQUISITANTES ---");
                    var requisitantes = _unitOfWork.Requisitante.GetAll()
                        .OrderBy(r => r.Nome)
                        .ToList();

                    Console.WriteLine($"[DEBUG] Requisitantes: {requisitantes.Count}");

                    RequisitanteList = requisitantes.Select(r => new SelectListItem
                    {
                        Value = r.RequisitanteId.ToString(),
                        Text = r.Nome ?? "Sem nome"
                    }).ToList();

                    if (!RequisitanteList.Any())
                    {
                        RequisitanteList = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "", Text = "Nenhum requisitante cadastrado" }
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERRO] Carregar Requisitantes: {ex.Message}");
                    RequisitanteList = new List<SelectListItem>();
                }

                // ✅ LOTAÇÕES - Lista fixa
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

                Console.WriteLine("[DEBUG] ===== CARREGAMENTO CONCLUÍDO =====");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO CRÍTICO] CarregarDropdowns: {ex.Message}");
                Console.WriteLine($"[ERRO CRÍTICO] StackTrace: {ex.StackTrace}");
                TempData["erro"] = $"Erro ao carregar dropdowns: {ex.Message}";
            }
        }

        private List<DateTime> ObterDatasValidas()
        {
            var datas = new List<DateTime>();
            var dataAtual = DataInicio.Date;

            var diasSelecionados = new List<DayOfWeek>();
            if (Domingo) diasSelecionados.Add(DayOfWeek.Sunday);
            if (Segunda) diasSelecionados.Add(DayOfWeek.Monday);
            if (Terca) diasSelecionados.Add(DayOfWeek.Tuesday);
            if (Quarta) diasSelecionados.Add(DayOfWeek.Wednesday);
            if (Quinta) diasSelecionados.Add(DayOfWeek.Thursday);
            if (Sexta) diasSelecionados.Add(DayOfWeek.Friday);
            if (Sabado) diasSelecionados.Add(DayOfWeek.Saturday);

            while (dataAtual <= DataFim.Date)
            {
                if (!diasSelecionados.Any() || diasSelecionados.Contains(dataAtual.DayOfWeek))
                {
                    datas.Add(dataAtual);
                }
                dataAtual = dataAtual.AddDays(1);
            }

            return datas;
        }

        private string DeterminarStatusDaEscala()
        {
            if (MotoristaIndisponivel) return "Indisponível";
            if (MotoristaEconomildo) return "Economildo";
            if (MotoristaEmServico) return "Em Serviço";
            if (MotoristaReservado) return "Reservado";
            return "Disponível";
        }

        private string ObterUsuarioId()
        {
            return User?.Identity?.Name ?? "Sistema";
        }
    }
}
