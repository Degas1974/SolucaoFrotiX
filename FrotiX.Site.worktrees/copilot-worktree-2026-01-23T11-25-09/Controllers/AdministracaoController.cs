/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║     ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗     ║
 * ║     ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗    ║
 * ║     █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║    ║
 * ║     ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║    ║
 * ║     ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝    ║
 * ║     ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝     ║
 * ║                                                                          ║
 * ║                      SOLUÇÃO FROTIX CARD IDENTITY                        ║
 * ║           SISTEMA DE GESTÃO DE FROTAS | UNIDADE DE INTELIGÊNCIA          ║
 * ║                                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ATENÇÃO: ESTE CÓDIGO É PARTE INTEGRANTE DO ECOSSISTEMA FROTIX.           ║
 * ║ QUALQUER ALTERAÇÃO NÃO AUTORIZADA PODE CAUSAR INSTABILIDADE NO FLUXO.    ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    [Authorize]
    [ApiController]
    public class AdministracaoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FrotiXDbContext _context;
        private readonly ILogService _log;
        private const decimal KM_MAXIMO_POR_VIAGEM = 2000;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AdministracaoController (Constructor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador administrativo com UnitOfWork, DbContext e Log. ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante acesso a dados e auditoria para métricas administrativas.         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios.                         ║
        /// ║    • context (FrotiXDbContext): contexto EF Core.                             ║
        /// ║    • logService (ILogService): serviço de log.                                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _log.Info() → auditoria de inicialização.                                ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public AdministracaoController(IUnitOfWork unitOfWork, FrotiXDbContext context, ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _context = context;
                _log = logService;

                _log.Info("AdministracaoController inicializado", "AdministracaoController.cs", "Constructor");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Erro no construtor de AdministracaoController: {error.Message}");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "Constructor", error);
            }
        }

        #region Resumo Geral da Frota

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterResumoGeralFrota                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Calcula métricas gerais da frota (ativos, viagens e KM) no período.       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Base do painel administrativo com indicadores operacionais.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do período (opcional).                    ║
        /// ║    • dataFim (DateTime?): fim do período (opcional).                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com métricas e status de sucesso.                    ║
        /// ║    • Consumidor: Dashboard Administrativo.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Veiculo/Motorista/Viagem → consultas EF Core.                   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Administracao/ObterResumoGeralFrota                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                                ║
        /// ║    • Arquivos relacionados: Pages/Administracao/*.cshtml                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterResumoGeralFrota")]
        public async Task<IActionResult> ObterResumoGeralFrota(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                // [LOGICA] Ajuste para incluir o dia inteiro do fim
                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                // [DADOS] Consultas de indicadores base
                var veiculosAtivos = await _context.Veiculo
                    .AsNoTracking()
                    .CountAsync(v => v.Status == true);

                var motoristasAtivos = await _context.Motorista
                    .AsNoTracking()
                    .CountAsync(m => m.Status == true);

                var viagensRealizadas = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada)
                    .CountAsync();

                var totalKm = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.KmRodado != null &&
                                v.KmRodado > 0)
                    .SumAsync(v => (decimal?)(v.KmRodado) ?? 0);

                // [DADOS] Montagem do payload do dashboard
                return Ok(new
                {
                    sucesso = true,
                    dados = new
                    {
                        veiculosAtivos,
                        motoristasAtivos,
                        viagensRealizadas,
                        totalKm = Math.Round(totalKm, 0)
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter resumo geral da frota", ex, "AdministracaoController.cs", "ObterResumoGeralFrota");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterResumoGeralFrota", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, dados = new { veiculosAtivos = 0, motoristasAtivos = 0, viagensRealizadas = 0, totalKm = 0 } });
            }
        }

        #endregion

        #region Estatísticas de Normalização

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEstatisticasNormalizacao                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Calcula métricas de normalização de viagens e distribuição por tipo.      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Acompanha qualidade e consistência da normalização de dados.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?): início do período (opcional).                    ║
        /// ║    • dataFim (DateTime?): fim do período (opcional).                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com métricas e distribuição por tipo.                ║
        /// ║    • Consumidor: Dashboard Administrativo.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.Viagem → consultas EF Core.                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Administracao/ObterEstatisticasNormalizacao                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                                ║
        /// ║    • Arquivos relacionados: Pages/Administracao/*.cshtml                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterEstatisticasNormalizacao")]
        public async Task<IActionResult> ObterEstatisticasNormalizacao(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                // [REGRA] Intervalo padrão quando datas não informadas
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddMonths(-6);
                }

                // [LOGICA] Ajuste para incluir o dia inteiro do fim
                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                // [DADOS] Carrega dados mínimos para cálculo estatístico
                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFimAjustada)
                    .Select(v => new
                    {
                        v.ViagemId,
                        v.FoiNormalizada,
                        v.TipoNormalizacao
                    })
                    .ToListAsync();

                // [LOGICA] Totais e percentuais
                var totalViagens = viagens.Count;
                var viagensNormalizadas = viagens.Count(v => v.FoiNormalizada == true);
                var viagensOriginais = totalViagens - viagensNormalizadas;

                // [LOGICA] Agrupamento por tipo de normalização
                var porTipoNormalizacao = viagens
                    .Where(v => v.FoiNormalizada == true && !string.IsNullOrEmpty(v.TipoNormalizacao))
                    .GroupBy(v => v.TipoNormalizacao)
                    .Select(g => new
                    {
                        tipo = g.Key,
                        quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.quantidade)
                    .ToList();

                return Ok(new
                {
                    sucesso = true,
                    dados = new
                    {
                        resumo = new
                        {
                            totalViagens,
                            viagensNormalizadas,
                            viagensOriginais,
                            percentualNormalizadas = totalViagens > 0
                                ? Math.Round((decimal)viagensNormalizadas / totalViagens * 100, 1)
                                : 0
                        },
                        porTipoNormalizacao
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter estatísticas de normalização", ex, "AdministracaoController.cs", "ObterEstatisticasNormalizacao");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEstatisticasNormalizacao", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message });
            }
        }

        #endregion

        #region Distribuição por Tipo de Uso

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterDistribuicaoTipoUso                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém a distribuição da frota por tipo de uso (Próprio/Terceirizado/Etc). ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Apoia decisões de contratação e gestão de frota.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista agrupada por tipo e quantidade.           ║
        /// ║    • Consumidor: Dashboard Administrativo.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.VeiculoPadraoViagem → consulta EF Core.                         ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Administracao/ObterDistribuicaoTipoUso                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Dashboard                                                ║
        /// ║    • Arquivos relacionados: Pages/Administracao/*.cshtml                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterDistribuicaoTipoUso")]
        public async Task<IActionResult> ObterDistribuicaoTipoUso()
        {
            try
            {
                try
                {
                    // [DADOS] Agrupa veículos por tipo de uso
                    var distribuicao = await _context.VeiculoPadraoViagem
                        .AsNoTracking()
                        .Where(v => !string.IsNullOrEmpty(v.TipoUso))
                        .GroupBy(v => v.TipoUso)
                        .Select(g => new
                        {
                            tipoUso = g.Key,
                            quantidade = g.Count()
                        })
                        .OrderByDescending(x => x.quantidade)
                        .ToListAsync();

                    if (distribuicao.Any())
                    {
                        return Ok(new { sucesso = true, dados = distribuicao });
                    }
                }
                catch
                {
                    // Fallback
                }

                try
                {
                    var distribuicaoVeiculo = await _context.Veiculo
                        .AsNoTracking()
                        .Where(v => v.Status == true)
                        .GroupBy(v => v.VeiculoProprio)
                        .Select(g => new
                        {
                            tipoUso = g.Key == true ? "Próprio" : "Terceirizado",
                            quantidade = g.Count()
                        })
                        .ToListAsync();

                    return Ok(new { sucesso = true, dados = distribuicaoVeiculo });
                }
                catch
                {
                    return Ok(new { sucesso = true, dados = new List<object>() });
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "AdministracaoController.cs", "ObterDistribuicaoTipoUso");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterDistribuicaoTipoUso", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
            }
        }

        #endregion

        #region Heatmap de Viagens

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterHeatmapViagens (GET)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera matriz para heatmap de viagens (dia x hora).                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com matriz do heatmap.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterHeatmapViagens")]
        public async Task<IActionResult> ObterHeatmapViagens(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddDays(-30);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                var viagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.DataInicial != null &&
                                v.HoraInicio != null)
                    .Select(v => new
                    {
                        DiaSemana = (int)v.DataInicial.Value.DayOfWeek,
                        Hora = v.HoraInicio.Value.Hour
                    })
                    .ToListAsync();

                var viagensConvertidas = viagens.Select(v => new
                {
                    DiaSemana = v.DiaSemana == 0 ? 6 : v.DiaSemana - 1,
                    v.Hora
                });

                var matriz = new int[7][];
                for (int dia = 0; dia < 7; dia++)
                {
                    matriz[dia] = new int[24];
                    for (int hora = 0; hora < 24; hora++)
                    {
                        matriz[dia][hora] = viagensConvertidas.Count(v => v.DiaSemana == dia && v.Hora == hora);
                    }
                }

                return Ok(new { sucesso = true, dados = new { matriz } });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter heatmap de viagens", ex, "AdministracaoController.cs", "ObterHeatmapViagens");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterHeatmapViagens", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message });
            }
        }

        #endregion

        #region Top 10 Veículos por KM

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterTop10VeiculosPorKm (GET)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém Top 10 veículos com mais viagens/km no período.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ranking e totais.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterTop10VeiculosPorKm")]
        public async Task<IActionResult> ObterTop10VeiculosPorKm(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddYears(-2);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                var viagensComVeiculo = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.VeiculoId != null)
                    .GroupBy(v => v.VeiculoId)
                    .Select(g => new
                    {
                        veiculoId = g.Key.Value,
                        totalKm = g.Sum(v => (double?)(v.KmRodado) ?? 0),
                        totalViagens = g.Count()
                    })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(10)
                    .ToListAsync();

                if (!viagensComVeiculo.Any())
                {
                    var totalViagensNoPeriodo = await _context.Viagem
                        .AsNoTracking()
                        .CountAsync(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFimAjustada);

                    var viagensComVeiculoCount = await _context.Viagem
                        .AsNoTracking()
                        .CountAsync(v => v.DataInicial >= dataInicio &&
                                        v.DataInicial <= dataFimAjustada &&
                                        v.VeiculoId != null);

                    return Ok(new
                    {
                        sucesso = true,
                        dados = new List<object>(),
                        debug = new
                        {
                            totalViagensNoPeriodo,
                            viagensComVeiculoCount,
                            dataInicio = dataInicio?.ToString("yyyy-MM-dd"),
                            dataFim = dataFim?.ToString("yyyy-MM-dd")
                        }
                    });
                }

                var veiculoIds = viagensComVeiculo.Select(v => v.veiculoId).ToList();
                var veiculos = await _context.ViewVeiculos
                    .AsNoTracking()
                    .Where(v => veiculoIds.Contains(v.VeiculoId))
                    .Select(v => new { v.VeiculoId, v.Placa, v.MarcaModelo })
                    .ToListAsync();

                var resultado = viagensComVeiculo.Select(t =>
                {
                    var veiculo = veiculos.FirstOrDefault(v => v.VeiculoId == t.veiculoId);
                    return new
                    {
                        veiculoId = t.veiculoId,
                        placa = veiculo?.Placa ?? $"ID:{t.veiculoId}",
                        marcaModelo = veiculo?.MarcaModelo ?? "",
                        veiculoDescricao = $"{veiculo?.Placa ?? $"ID:{t.veiculoId}"} - {veiculo?.MarcaModelo ?? "N/A"}",
                        totalKm = Math.Round((decimal)t.totalKm, 0),
                        totalViagens = t.totalViagens
                    };
                }).ToList();

                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter Top 10 veículos por KM", ex, "AdministracaoController.cs", "ObterTop10VeiculosPorKm");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterTop10VeiculosPorKm", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, stack = ex.StackTrace, dados = new List<object>() });
            }
        }

        #endregion

        #region Top 10 Motoristas por KM

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterTop10MotoristasPorKm (GET)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém Top 10 motoristas com mais viagens no período.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com ranking e totais.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterTop10MotoristasPorKm")]
        public async Task<IActionResult> ObterTop10MotoristasPorKm(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddYears(-2);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                var viagensComMotorista = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.MotoristaId != null)
                    .GroupBy(v => v.MotoristaId)
                    .Select(g => new
                    {
                        motoristaId = g.Key.Value,
                        totalKm = g.Sum(v => (double?)(v.KmRodado) ?? 0),
                        totalViagens = g.Count()
                    })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(10)
                    .ToListAsync();

                if (!viagensComMotorista.Any())
                {
                    var totalViagensNoPeriodo = await _context.Viagem
                        .AsNoTracking()
                        .CountAsync(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFimAjustada);

                    var viagensComMotoristaCount = await _context.Viagem
                        .AsNoTracking()
                        .CountAsync(v => v.DataInicial >= dataInicio &&
                                        v.DataInicial <= dataFimAjustada &&
                                        v.MotoristaId != null);

                    return Ok(new
                    {
                        sucesso = true,
                        dados = new List<object>(),
                        debug = new
                        {
                            totalViagensNoPeriodo,
                            viagensComMotoristaCount
                        }
                    });
                }

                var motoristaIds = viagensComMotorista.Select(m => m.motoristaId).ToList();
                var motoristas = await _context.Motorista
                    .AsNoTracking()
                    .Where(m => motoristaIds.Contains(m.MotoristaId))
                    .Select(m => new { m.MotoristaId, m.Nome })
                    .ToListAsync();

                var resultado = viagensComMotorista.Select(t =>
                {
                    var motorista = motoristas.FirstOrDefault(m => m.MotoristaId == t.motoristaId);
                    return new
                    {
                        motoristaId = t.motoristaId,
                        nome = motorista?.Nome ?? $"Motorista #{t.motoristaId}",
                        totalKm = Math.Round((decimal)t.totalKm, 0),
                        totalViagens = t.totalViagens
                    };
                }).ToList();

                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter Top 10 motoristas por KM", ex, "AdministracaoController.cs", "ObterTop10MotoristasPorKm");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterTop10MotoristasPorKm", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
            }
        }

        #endregion

        #region Custo por Finalidade

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterCustoPorFinalidade (GET)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém custos agrupados pela finalidade da viagem.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com custos e médias por finalidade.                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterCustoPorFinalidade")]
        public async Task<IActionResult> ObterCustoPorFinalidade(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddDays(-90);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                var custos = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                !string.IsNullOrEmpty(v.Finalidade))
                    .GroupBy(v => v.Finalidade)
                    .Select(g => new
                    {
                        finalidade = g.Key,
                        totalViagens = g.Count(),
                        custoTotal = g.Sum(v => (v.CustoCombustivel ?? 0) + (v.CustoLavador ?? 0) + (v.CustoMotorista ?? 0))
                    })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(10)
                    .ToListAsync();

                var resultado = custos.Select(c => new
                {
                    c.finalidade,
                    c.totalViagens,
                    custoTotal = Math.Round(c.custoTotal, 2),
                    custoMedio = c.totalViagens > 0 ? Math.Round(c.custoTotal / c.totalViagens, 2) : 0
                }).ToList();

                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter custo por finalidade", ex, "AdministracaoController.cs", "ObterCustoPorFinalidade");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterCustoPorFinalidade", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
            }
        }

        #endregion

        #region Próprios vs Terceirizados

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterComparativoPropiosTerceirizados (GET)                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Compara desempenho entre frota própria e terceirizada.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com comparativos e totais.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterComparativoPropiosTerceirizados")]
        public async Task<IActionResult> ObterComparativoPropiosTerceirizados(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddYears(-2);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                List<Guid> veiculosProprios = new List<Guid>();
                var totalVeiculos = 0;
                var totalVeiculosProprios = 0;

                try
                {
                    veiculosProprios = await _context.ViewVeiculos
                        .AsNoTracking()
                        .Where(v => v.VeiculoProprio == true)
                        .Select(v => v.VeiculoId)
                        .ToListAsync();

                    totalVeiculos = await _context.ViewVeiculos.CountAsync();
                    totalVeiculosProprios = veiculosProprios.Count;
                }
                catch
                {
                    try
                    {
                        veiculosProprios = await _context.Veiculo
                            .AsNoTracking()
                            .Where(v => v.VeiculoProprio == true)
                            .Select(v => v.VeiculoId)
                            .ToListAsync();

                        totalVeiculos = await _context.Veiculo.CountAsync();
                        totalVeiculosProprios = veiculosProprios.Count;
                    }
                    catch { }
                }

                var todasViagens = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.VeiculoId != null)
                    .Select(v => new
                    {
                        veiculoId = v.VeiculoId.Value,
                        km = (double)(v.KmRodado ?? 0),
                        custo = (v.CustoCombustivel ?? 0) + (v.CustoLavador ?? 0) + (v.CustoMotorista ?? 0)
                    })
                    .ToListAsync();

                var viagensProprios = todasViagens.Where(v => veiculosProprios.Contains(v.veiculoId)).ToList();
                var viagensTerceirizados = todasViagens.Where(v => !veiculosProprios.Contains(v.veiculoId)).ToList();

                var resultado = new
                {
                    proprios = new
                    {
                        totalViagens = viagensProprios.Count,
                        totalKm = Math.Round((decimal)viagensProprios.Sum(v => v.km), 0),
                        custoTotal = Math.Round(viagensProprios.Sum(v => v.custo), 2)
                    },
                    terceirizados = new
                    {
                        totalViagens = viagensTerceirizados.Count,
                        totalKm = Math.Round((decimal)viagensTerceirizados.Sum(v => v.km), 0),
                        custoTotal = Math.Round(viagensTerceirizados.Sum(v => v.custo), 2)
                    },
                    debug = new
                    {
                        totalVeiculos,
                        totalVeiculosProprios,
                        totalViagensComVeiculo = todasViagens.Count
                    }
                };

                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter comparativo próprios vs terceirizados", ex, "AdministracaoController.cs", "ObterComparativoPropiosTerceirizados");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterComparativoPropiosTerceirizados", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message });
            }
        }

        #endregion

        #region Eficiência da Frota (Custo por KM)

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEficienciaFrota (GET)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Analisa eficiência da frota (custo por km e por viagem).                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com métricas de eficiência.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterEficienciaFrota")]
        public async Task<IActionResult> ObterEficienciaFrota(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddYears(-2);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                var eficiencia = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.VeiculoId != null)
                    .GroupBy(v => v.VeiculoId)
                    .Select(g => new
                    {
                        veiculoId = g.Key.Value,
                        totalKm = g.Sum(v => (decimal?)(v.KmRodado) ?? 0),
                        custoTotal = g.Sum(v => (v.CustoCombustivel ?? 0) + (v.CustoLavador ?? 0) + (v.CustoMotorista ?? 0)),
                        totalViagens = g.Count()
                    })
                    .OrderByDescending(x => x.totalViagens)
                    .Take(10)
                    .ToListAsync();

                if (!eficiencia.Any())
                {
                    return Ok(new { sucesso = true, dados = new List<object>() });
                }

                var eficienciaComCustoPorKm = eficiencia
                    .Select(e => new
                    {
                        e.veiculoId,
                        e.totalKm,
                        e.custoTotal,
                        e.totalViagens,
                        custoPorKm = e.totalKm > 0 ? (decimal)e.custoTotal / e.totalKm : 0m,
                        custoPorViagem = e.totalViagens > 0 ? (decimal)e.custoTotal / e.totalViagens : 0m
                    })
                    .ToList();

                var veiculoIds = eficienciaComCustoPorKm.Select(e => e.veiculoId).ToList();
                var veiculos = await _context.ViewVeiculos
                    .AsNoTracking()
                    .Where(v => veiculoIds.Contains(v.VeiculoId))
                    .Select(v => new { v.VeiculoId, v.Placa, v.MarcaModelo })
                    .ToListAsync();

                var resultado = eficienciaComCustoPorKm.Select(e =>
                {
                    var veiculo = veiculos.FirstOrDefault(v => v.VeiculoId == e.veiculoId);
                    return new
                    {
                        veiculoId = e.veiculoId,
                        placa = veiculo?.Placa ?? $"ID:{e.veiculoId}",
                        marcaModelo = veiculo?.MarcaModelo ?? "N/A",
                        veiculoDescricao = $"{veiculo?.Placa ?? $"ID:{e.veiculoId}"} - {veiculo?.MarcaModelo ?? "N/A"}",
                        totalKm = Math.Round(e.totalKm, 0),
                        custoTotal = Math.Round(e.custoTotal, 2),
                        totalViagens = e.totalViagens,
                        custoPorKm = Math.Round(e.custoPorKm, 4),
                        custoPorViagem = Math.Round(e.custoPorViagem, 2)
                    };
                }).ToList();

                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter eficiência da frota", ex, "AdministracaoController.cs", "ObterEficienciaFrota");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEficienciaFrota", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
            }
        }

        #endregion

        #region Evolução Mensal de Custos

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterEvolucaoMensalCustos (GET)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Mostra evolução mensal dos custos (combustível, motorista, etc.).         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataInicio (DateTime?), dataFim (DateTime?)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com séries mensais.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("api/Administracao/ObterEvolucaoMensalCustos")]
        public async Task<IActionResult> ObterEvolucaoMensalCustos(DateTime? dataInicio, DateTime? dataFim)
        {
            try
            {
                if (!dataInicio.HasValue || !dataFim.HasValue)
                {
                    dataFim = DateTime.Now.Date;
                    dataInicio = dataFim.Value.AddYears(-2);
                }

                var dataFimAjustada = dataFim.Value.Date.AddDays(1).AddTicks(-1);

                var evolucao = await _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial >= dataInicio &&
                                v.DataInicial <= dataFimAjustada &&
                                v.DataInicial.HasValue)
                    .GroupBy(v => new
                    {
                        Ano = v.DataInicial.Value.Year,
                        Mes = v.DataInicial.Value.Month
                    })
                    .Select(g => new
                    {
                        ano = g.Key.Ano,
                        mes = g.Key.Mes,
                        totalViagens = g.Count(),
                        custoCombustivel = g.Sum(v => v.CustoCombustivel ?? 0),
                        custoLavador = g.Sum(v => v.CustoLavador ?? 0),
                        custoMotorista = g.Sum(v => v.CustoMotorista ?? 0),
                        totalKm = g.Sum(v => (decimal?)(v.KmRodado) ?? 0)
                    })
                    .OrderBy(x => x.ano)
                    .ThenBy(x => x.mes)
                    .ToListAsync();

                var resultado = evolucao.Select(e => new
                {
                    mesAno = $"{e.mes:D2}/{e.ano}",
                    periodo = $"{e.ano}-{e.mes:D2}",
                    e.totalViagens,
                    custoCombustivel = Math.Round(e.custoCombustivel, 2),
                    custoLavador = Math.Round(e.custoLavador, 2),
                    custoMotorista = Math.Round(e.custoMotorista, 2),
                    custoTotal = Math.Round(e.custoCombustivel + e.custoLavador + e.custoMotorista, 2),
                    totalKm = Math.Round(e.totalKm, 0)
                }).ToList();

                return Ok(new { sucesso = true, dados = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter evolução mensal de custos", ex, "AdministracaoController.cs", "ObterEvolucaoMensalCustos");
                Alerta.TratamentoErroComLinha("AdministracaoController.cs", "ObterEvolucaoMensalCustos", ex);
                return Ok(new { sucesso = false, mensagem = ex.Message, dados = new List<object>() });
            }
        }

        #endregion
    }
}
