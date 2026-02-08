/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ListaEscala.cshtml.cs (Pages/Escalas)                                                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para listagem e gestão de Escalas Diárias com filtros avançados. Permite visualizar, filtrar   ║
 * ║ e excluir escalas de motoristas/veículos por diversos critérios.                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ BIND PROPERTIES                                                                                          ║
 * ║ • Filtro     : FiltroEscalaViewModel          - Filtros selecionados pelo usuário                        ║
 * ║ • EscalasObj : List<ViewEscalasCompletas>     - Resultado da query filtrada                              ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet()         : Carrega escalas do dia atual                                                         ║
 * ║ • OnPost()        : Aplica filtros selecionados                                                          ║
 * ║ • OnGetDelete(id) : Exclui escala pelo EscalaDiaId                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ FILTROS DISPONÍVEIS                                                                                      ║
 * ║ • DataFiltro     : Data específica                                                                       ║
 * ║ • TipoServicoId  : Economildo / Serviços Gerais                                                          ║
 * ║ • TurnoId        : Matutino / Vespertino / Noturno                                                       ║
 * ║ • MotoristaId    : Motorista específico                                                                  ║
 * ║ • VeiculoId      : Veículo específico                                                                    ║
 * ║ • StatusMotorista: Disponível, Em Viagem, Indisponível, etc.                                             ║
 * ║ • Lotacao        : Aeroporto, Rodoviária, PGR, Cefor, etc.                                               ║
 * ║ • TextoPesquisa  : Busca livre em nome, placa, serviço, requisitante                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ORDENAÇÃO PADRÃO                                                                                         ║
 * ║ DataEscala → HoraInicio → NomeMotorista                                                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork (ViewEscalasCompletas, EscalaDiaria, Motorista, Veiculo)                                   ║
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
    public class ListaEscalaModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListaEscalaModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEscala.cshtml.cs", "Construtor", error);
            }
        }

        // ===================================================================
        // PROPRIEDADES
        // ===================================================================

        [BindProperty]
        public FiltroEscalaViewModel Filtro { get; set; } = new FiltroEscalaViewModel();

        [BindProperty]
        public List<Models.ViewEscalasCompletas> EscalasObj { get; set; } = new List<Models.ViewEscalasCompletas>();

        // ===================================================================
        // MÉTODO ONGET - CARREGAMENTO INICIAL
        // ===================================================================

        public void OnGet()
        {
            try
            {
                Filtro.DataFiltro = DateTime.Today;

                Console.WriteLine($"[DEBUG] DataFiltro: {Filtro.DataFiltro}");

                CarregarListasFiltros();
                CarregarEscalas();

                Console.WriteLine($"[DEBUG] EscalasObj Count: {EscalasObj?.Count ?? -1}");
                Console.WriteLine($"[DEBUG] Query sem filtro: {_unitOfWork.ViewEscalasCompletas.GetAll().Count()}");

                var escalasHoje = _unitOfWork.ViewEscalasCompletas.GetAll()
                    .Where(e => e.DataEscala.Date == DateTime.Today)
                    .ToList();
                Console.WriteLine($"[DEBUG] Escalas de hoje: {escalasHoje.Count}");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEscala.cshtml.cs", "OnGet", error);
                TempData["erro"] = "Erro ao carregar dados da página: " + error.Message;
            }
        }

        // ===================================================================
        // MÉTODO ONPOST - APLICAR FILTROS
        // ===================================================================

        public IActionResult OnPost()
        {
            try
            {
                // Recarrega listas para os dropdowns
                CarregarListasFiltros();

                // Aplica filtros e recarrega escalas
                CarregarEscalas();

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEscala.cshtml.cs", "OnPost", error);
                TempData["erro"] = "Erro ao aplicar filtros";
                return Page();
            }
        }

        // ===================================================================
        // HANDLER - EXCLUIR ESCALA
        // ===================================================================

        public IActionResult OnGetDelete(Guid id)
        {
            try
            {
                var escala = _unitOfWork.EscalaDiaria.GetFirstOrDefault(e => e.EscalaDiaId == id);

                if (escala == null)
                {
                    TempData["erro"] = "Escala não encontrada";
                    return RedirectToPage();
                }

                _unitOfWork.EscalaDiaria.Remove(escala);
                _unitOfWork.Save();

                TempData["sucesso"] = "Escala excluída com sucesso!";
                return RedirectToPage();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEscala.cshtml.cs", "OnGetDelete", error);
                TempData["erro"] = "Erro ao excluir escala";
                return RedirectToPage();
            }


        }

        // ===================================================================
        // MÉTODOS PRIVADOS
        // ===================================================================

        /// <summary>
        /// Carrega escalas aplicando os filtros selecionados
        /// </summary>
        private void CarregarEscalas()
        {
            try
            {
                var query = _unitOfWork.ViewEscalasCompletas.GetAll();

                // Aplicar filtros
                if (Filtro.DataFiltro.HasValue)
                {
                    query = query.Where(e => e.DataEscala.Date == Filtro.DataFiltro.Value.Date);
                }

                if (Filtro.TipoServicoId.HasValue && Filtro.TipoServicoId.Value != Guid.Empty)
                {
                    query = query.Where(e => e.TipoServicoId == Filtro.TipoServicoId.Value);
                }

                if (Filtro.TurnoId.HasValue && Filtro.TurnoId.Value != Guid.Empty)
                {
                    query = query.Where(e => e.TurnoId == Filtro.TurnoId.Value);
                }

                if (Filtro.MotoristaId.HasValue && Filtro.MotoristaId.Value != Guid.Empty)
                {
                    query = query.Where(e => e.MotoristaId == Filtro.MotoristaId.Value);
                }

                if (Filtro.VeiculoId.HasValue && Filtro.VeiculoId.Value != Guid.Empty)
                {
                    query = query.Where(e => e.VeiculoId == Filtro.VeiculoId.Value);
                }

                if (!string.IsNullOrWhiteSpace(Filtro.StatusMotorista))
                {
                    query = query.Where(e => e.StatusMotorista == Filtro.StatusMotorista);
                }
                else
                {
                    // Se NÃO está filtrando por status específico, ocultar Indisponível
                    query = query.Where(e => e.StatusMotorista != "Indisponível");
                }

                if (!string.IsNullOrWhiteSpace(Filtro.Lotacao))
                {
                    query = query.Where(e => e.Lotacao == Filtro.Lotacao);
                }

                if (!string.IsNullOrWhiteSpace(Filtro.TextoPesquisa))
                {
                    var texto = Filtro.TextoPesquisa.ToLower();
                    query = query.Where(e =>
                        (e.NomeMotorista != null && e.NomeMotorista.ToLower().Contains(texto)) ||
                        (e.Placa != null && e.Placa.ToLower().Contains(texto)) ||
                        (e.NomeServico != null && e.NomeServico.ToLower().Contains(texto)) ||
                        (e.NomeRequisitante != null && e.NomeRequisitante.ToLower().Contains(texto))
                    );
                }

                // Ordenar e converter para lista
                EscalasObj = query
                    .OrderBy(e => e.DataEscala)
                    .ThenBy(e => e.HoraInicio)
                    .ThenBy(e => e.NomeMotorista)
                    .ToList();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEscala.cshtml.cs", "CarregarEscalas", error);
                EscalasObj = new List<Models.ViewEscalasCompletas>();
            }
        }

        /// <summary>
        /// Carrega listas para os dropdowns usando métodos dos Repositories
        /// ✅ SOLUÇÃO CORRETA: Usa métodos ForDropDown que retornam SelectListItem
        /// </summary>
        private void CarregarListasFiltros()
        {
            try
            {

                Filtro.TipoServicoList = GetTipoServicoList();

                Filtro.TurnoList = GetTurnoList();

                Filtro.MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown();

                // ✅ USA ViewVeiculos.VeiculoCompleto (Placa + Marca/Modelo)
                Filtro.VeiculoList = _unitOfWork.Veiculo.GetVeiculoCompletoListForDropDown();

                Filtro.LotacaoList = GetLotacaoList();


                Filtro.StatusList = GetStatusList();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEscala.cshtml.cs", "CarregarListasFiltros", error);

                // Garantir listas vazias em caso de erro
                Filtro.TipoServicoList = Enumerable.Empty<SelectListItem>();
                Filtro.TurnoList = Enumerable.Empty<SelectListItem>();
                Filtro.MotoristaList = Enumerable.Empty<SelectListItem>();
                Filtro.VeiculoList = Enumerable.Empty<SelectListItem>();
                Filtro.LotacaoList = Enumerable.Empty<SelectListItem>();
                Filtro.StatusList = Enumerable.Empty<SelectListItem>();
            }
        }

        /// <summary>
        /// Retorna lista fixa de lotações
        /// </summary>
        private IEnumerable<SelectListItem> GetLotacaoList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Selecione...", Value = "" },
                new SelectListItem { Text = "Aeroporto", Value = "Aeroporto" },
                new SelectListItem { Text = "Rodoviária", Value = "Rodoviária" },
                new SelectListItem { Text = "PGR", Value = "PGR" },
                new SelectListItem { Text = "Cefor", Value = "Cefor" },
                new SelectListItem { Text = "Setor de Obras", Value = "Setor de Obras" },
                new SelectListItem { Text = "Outros", Value = "Outros" }
            };
        }

        /// <summary>
        /// Retorna lista fixa de status de motorista
        /// </summary>
        private IEnumerable<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Selecione...", Value = "" },
                new SelectListItem { Text = "Disponível", Value = "Disponível" },
                new SelectListItem { Text = "Em Viagem", Value = "Em Viagem" },
                new SelectListItem { Text = "Indisponível", Value = "Indisponível" },
                new SelectListItem { Text = "Em Serviço", Value = "Em Serviço" },
                new SelectListItem { Text = "Economildo", Value = "Economildo" },
                new SelectListItem { Text = "Reservado para Serviço", Value = "Reservado para Serviço" }
            };
        }

        private IEnumerable<SelectListItem> GetTurnoList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Selecione...", Value = "" },
                new SelectListItem { Text = "Matutino", Value = "Matutino" },
                new SelectListItem { Text = "Vespertino", Value = "Vespertino" },
                new SelectListItem { Text = "Noturno", Value = "Noturno" },

            };
        }

        private IEnumerable<SelectListItem> GetTipoServicoList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Selecione...", Value = "" },
                new SelectListItem { Text = "Economildo", Value = "Economildo" },
                new SelectListItem { Text = "Servicos Gerais", Value = "Serviços Gerais" },

            };
        }

    }
}
