/* ****************************************************************************************
 * ⚡ ARQUIVO: RelatoriosController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Exportar PDFs do Dashboard Economildo (heatmaps, gráficos e rankings).
 *
 * 📥 ENTRADAS     : Tipo de relatório e filtros (mob, mês, ano).
 *
 * 📤 SAÍDAS       : Arquivo PDF gerado com os dados solicitados.
 *
 * 🔗 CHAMADA POR  : Dashboard Economildo (exportação de relatórios).
 *
 * 🔄 CHAMA        : RelatorioEconomildoPdfService, FrotiXDbContext, IUnitOfWork.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.Services.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace FrotiX.Controllers;

/****************************************************************************************
 * ⚡ CONTROLLER: RelatoriosController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoint de exportação de PDFs do Dashboard Economildo.
 *
 * 📥 ENTRADAS     : Tipo de relatório e filtros opcionais.
 *
 * 📤 SAÍDAS       : PDF como arquivo para download.
 *
 * 🔗 CHAMADA POR  : Dashboard Economildo.
 ****************************************************************************************/
[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : Controller
{
    private readonly FrotiXDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RelatorioEconomildoPdfService _pdfService;

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: RelatoriosController (Construtor)                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Inicializa contexto, UnitOfWork e serviço de PDF.                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • context (FrotiXDbContext): Contexto EF Core.                            ║
    /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
    /// ║    • log (ILogService): Serviço de log centralizado.                         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public RelatoriosController(FrotiXDbContext context, IUnitOfWork unitOfWork, ILogService log)
    {
        try
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _log = log;
            _pdfService = new RelatorioEconomildoPdfService();
        }
        catch (Exception ex)
        {
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "RelatoriosController", ex);
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ExportarEconomildo (GET)                                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta relatório Economildo conforme tipo e filtros informados.         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • tipo (TipoRelatorioEconomildo): Tipo do relatório.                      ║
    /// ║    • mob (string?): Unidade MOB (opcional).                                 ║
    /// ║    • mes (int?): Mês de referência (opcional).                               ║
    /// ║    • ano (int?): Ano de referência (opcional).                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • IActionResult: Arquivo PDF ou erro.                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [HttpGet]
    [Route("ExportarEconomildo")]
    public IActionResult ExportarEconomildo(
        TipoRelatorioEconomildo tipo,
        string? mob = null,
        int? mes = null,
        int? ano = null)
    {
        try
        {
            // [FILTRO] Monta DTO de filtro.
            var filtro = new FiltroEconomildoDto { Mob = mob, Mes = mes, Ano = ano };

            // [LOG] Registro de geração.
            _log.Info($"RelatoriosController.ExportarEconomildo: Gerando relatório {tipo} (MOB: {mob ?? "Todos"}, Mês: {mes ?? 0}, Ano: {ano ?? 0})");

            // [ACAO] Resolve gerador conforme tipo.
            byte[] pdfBytes = tipo switch
            {
                TipoRelatorioEconomildo.HeatmapViagens => GerarHeatmapViagens(filtro),
                TipoRelatorioEconomildo.HeatmapPassageiros => GerarHeatmapPassageiros(filtro),
                TipoRelatorioEconomildo.UsuariosMes => GerarUsuariosMes(filtro),
                TipoRelatorioEconomildo.UsuariosTurno => GerarUsuariosTurno(filtro),
                TipoRelatorioEconomildo.ComparativoMob => GerarComparativoMob(filtro),
                TipoRelatorioEconomildo.UsuariosDiaSemana => GerarUsuariosDiaSemana(filtro),
                TipoRelatorioEconomildo.DistribuicaoHorario => GerarDistribuicaoHorario(filtro),
                TipoRelatorioEconomildo.TopVeiculos => GerarTopVeiculos(filtro),
                _ => throw new ArgumentException($"Tipo de relatório não suportado: {tipo}")
            };

            // [RETORNO] Retorna arquivo PDF.
            var nomeArquivo = $"Economildo_{tipo}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            return File(pdfBytes, "application/pdf", nomeArquivo);
        }
        catch (Exception ex)
        {
            // [LOG] Registro de erro.
            _log.Error("RelatoriosController.ExportarEconomildo", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "ExportarEconomildo", ex);
            // [RETORNO] Retorno de erro para o cliente.
            return BadRequest($"Erro ao gerar PDF: {ex.Message}");
        }
    }

    #region ==================== FILTRAGEM BASE ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: BuscarViagensEconomildo (Helper)                                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Filtra viagens por MOB, mês e ano conforme o filtro informado.           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ║    • ignorarMob (bool): Ignorar MOB no filtro.                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • List<ViagensEconomildo>: Lista filtrada.                                ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private List<ViagensEconomildo> BuscarViagensEconomildo(FiltroEconomildoDto filtro, bool ignorarMob = false)
    {
        try
        {
            // [DADOS] Base de consulta.
            var query = _context.ViagensEconomildo.AsQueryable();

            if (!ignorarMob && !string.IsNullOrEmpty(filtro.Mob))
            {
                // [FILTRO] MOB.
                query = query.Where(v => v.MOB == filtro.Mob);
            }

            if (filtro.Mes.HasValue && filtro.Mes.Value > 0)
            {
                // [FILTRO] Mês.
                query = query.Where(v => v.Data.HasValue && v.Data.Value.Month == filtro.Mes.Value);
            }

            if (filtro.Ano.HasValue && filtro.Ano.Value > 0)
            {
                // [FILTRO] Ano.
                query = query.Where(v => v.Data.HasValue && v.Data.Value.Year == filtro.Ano.Value);
            }

            // [RETORNO] Lista filtrada.
            return query.ToList();
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.BuscarViagensEconomildo", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "BuscarViagensEconomildo", ex);
            return new List<ViagensEconomildo>();
        }
    }

    #endregion

    #region ==================== HEATMAP VIAGENS ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarHeatmapViagens (Helper)                                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Processa dados e gera PDF do mapa de calor de viagens.                    ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarHeatmapViagens(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Monta dataset do heatmap.
            var dados = MontarDadosHeatmap(filtro, usarPassageiros: false);
            // [REGRA] Ajusta títulos e unidade.
            dados.Titulo = "Mapa de Calor - Distribuição de Viagens";
            dados.Subtitulo = $"{filtro.NomeVeiculo} | Análise por Dia da Semana e Horário";
            dados.UnidadeLegenda = "viagens";
            // [RETORNO] Gera PDF.
            return _pdfService.GerarHeatmapViagens(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarHeatmapViagens", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarHeatmapViagens", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== HEATMAP PASSAGEIROS ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarHeatmapPassageiros (Helper)                                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Processa dados e gera PDF do mapa de calor de passageiros.                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarHeatmapPassageiros(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Monta dataset do heatmap.
            var dados = MontarDadosHeatmap(filtro, usarPassageiros: true);
            // [REGRA] Ajusta títulos e unidade.
            dados.Titulo = "Mapa de Calor - Distribuição de Passageiros";
            dados.Subtitulo = $"{filtro.NomeVeiculo} | Análise por Dia da Semana e Horário";
            dados.UnidadeLegenda = "passageiros";
            // [RETORNO] Gera PDF.
            return _pdfService.GerarHeatmapPassageiros(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarHeatmapPassageiros", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarHeatmapPassageiros", ex);
            return Array.Empty<byte>();
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MontarDadosHeatmap (Helper)                                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Monta DTO de heatmap com matriz, picos e indicadores.                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ║    • usarPassageiros (bool): Usa passageiros ao invés de viagens.            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • HeatmapDto: Dados prontos para renderização.                            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private HeatmapDto MontarDadosHeatmap(FiltroEconomildoDto filtro, bool usarPassageiros)
    {
        try
        {
            // [DADOS] Carrega viagens conforme filtro.
            var viagens = BuscarViagensEconomildo(filtro);

            // [DADOS] Inicializa matrizes e indicadores.
            var valores = new int[7, 24];
            int valorMaximo = 0;
            string diaPico = "";
            int horaPico = 0;
            var totaisPorDia = new int[7];

            // [PROCESSAMENTO] Percorre viagens e computa métricas.
            foreach (var viagem in viagens)
            {
                if (!viagem.Data.HasValue) continue;

                var diaSemana = (int)viagem.Data.Value.DayOfWeek;
                diaSemana = diaSemana == 0 ? 6 : diaSemana - 1; // Seg=0, Dom=6

                var hora = ExtrairHora(viagem.HoraInicio);
                if (hora < 0) continue;

                var quantidade = usarPassageiros ? (viagem.QtdPassageiros ?? 1) : 1;

                valores[diaSemana, hora] += quantidade;
                totaisPorDia[diaSemana] += quantidade;

                if (valores[diaSemana, hora] > valorMaximo)
                {
                    valorMaximo = valores[diaSemana, hora];
                    diaPico = ObterNomeDiaAbreviado(diaSemana);
                    horaPico = hora;
                }
            }

            var diasNomes = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };
            var indiceDiaMaisMovimentado = Array.IndexOf(totaisPorDia, totaisPorDia.Max());

            // Horário pico manhã
            int maxManha = 0, horaInicioManha = 10;
            for (int h = 6; h <= 12; h++)
            {
                int totalHora = 0;
                for (int d = 0; d < 7; d++) totalHora += valores[d, h];
                if (totalHora > maxManha) { maxManha = totalHora; horaInicioManha = h; }
            }

            // Período operação
            int primeiraHora = 23, ultimaHora = 0;
            for (int h = 0; h < 24; h++)
                for (int d = 0; d < 7; d++)
                    if (valores[d, h] > 0)
                    {
                        if (h < primeiraHora) primeiraHora = h;
                        if (h > ultimaHora) ultimaHora = h;
                    }

            // [RETORNO] DTO completo do heatmap.
            return new HeatmapDto
            {
                Valores = valores,
                ValorMaximo = valorMaximo,
                DiaPico = diaPico,
                HoraPico = horaPico,
                HorarioPicoManha = $"{horaInicioManha}h - {Math.Min(horaInicioManha + 2, 12)}h",
                DiaMaisMovimentado = diasNomes[indiceDiaMaisMovimentado],
                PeriodoOperacao = primeiraHora <= ultimaHora ? $"{primeiraHora:00}h - {ultimaHora:00}h" : "—",
                Filtro = filtro
            };
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.MontarDadosHeatmap", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "MontarDadosHeatmap", ex);
            return new HeatmapDto { Valores = new int[7, 24], Filtro = filtro };
        }
    }

    #endregion

    #region ==================== USUÁRIOS POR MÊS ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarUsuariosMes (Helper)                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta PDF com gráfico de barras de usuários por mês.                    ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarUsuariosMes(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Carrega viagens.
            var viagens = BuscarViagensEconomildo(filtro);

            // [DADOS] Agrupa por mês.
            var usuariosPorMes = viagens
                .Where(v => v.Data.HasValue)
                .GroupBy(v => v.Data!.Value.Month)
                .Select(g => new ItemGraficoDto
                {
                    Label = ObterNomeMes(g.Key),
                    Valor = g.Sum(v => v.QtdPassageiros ?? 0)
                })
                .OrderBy(x => ObterNumeroMes(x.Label))
                .ToList();

            // [REGRA] Calcula percentuais.
            var total = usuariosPorMes.Sum(d => d.Valor);
            foreach (var item in usuariosPorMes)
                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;

            var dados = new GraficoBarrasDto
            {
                Titulo = "Usuários por Mês",
                Subtitulo = filtro.NomeVeiculo,
                EixoX = "Mês",
                EixoY = "Usuários",
                Dados = usuariosPorMes,
                Filtro = filtro
            };

            // [RETORNO] Gera PDF.
            return _pdfService.GerarUsuariosMes(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarUsuariosMes", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarUsuariosMes", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== USUÁRIOS POR TURNO ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarUsuariosTurno (Helper)                                       ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta PDF com gráfico de pizza por turno (Manhã/Tarde/Noite).           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarUsuariosTurno(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Carrega viagens.
            var viagens = BuscarViagensEconomildo(filtro);

            // [DADOS] Soma passageiros por turno.
            var manha = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Manhã").Sum(v => v.QtdPassageiros ?? 0);
            var tarde = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Tarde").Sum(v => v.QtdPassageiros ?? 0);
            var noite = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Noite").Sum(v => v.QtdPassageiros ?? 0);

            // [CALCULO] Total geral.
            var total = manha + tarde + noite;

            // [DADOS] Monta DTO do gráfico.
            var dados = new GraficoPizzaDto
            {
                Titulo = "Usuários por Turno",
                Subtitulo = filtro.NomeVeiculo,
                Dados = new List<ItemGraficoDto>
                {
                    new() { Label = "Manhã", Valor = manha, Percentual = total > 0 ? (double)manha / total * 100 : 0 },
                    new() { Label = "Tarde", Valor = tarde, Percentual = total > 0 ? (double)tarde / total * 100 : 0 },
                    new() { Label = "Noite", Valor = noite, Percentual = total > 0 ? (double)noite / total * 100 : 0 }
                },
                Filtro = filtro
            };

            // [RETORNO] Gera PDF.
            return _pdfService.GerarUsuariosTurno(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarUsuariosTurno", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarUsuariosTurno", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== COMPARATIVO MOB ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarComparativoMob (Helper)                                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta PDF com gráfico comparativo entre unidades MOB.                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarComparativoMob(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Carrega viagens sem filtro de MOB.
            var viagensTodos = BuscarViagensEconomildo(filtro, ignorarMob: true);

            // [DADOS] Agrupa por mês e MOB.
            var comparativoMob = viagensTodos
                .Where(v => v.Data.HasValue)
                .GroupBy(v => v.Data!.Value.Month)
                .Select(g => new
                {
                    mesNum = g.Key,
                    mes = ObterNomeMes(g.Key),
                    rodoviaria = g.Where(v => v.MOB == "Rodoviaria").Sum(v => v.QtdPassageiros ?? 0),
                    pgr = g.Where(v => v.MOB == "PGR").Sum(v => v.QtdPassageiros ?? 0),
                    cefor = g.Where(v => v.MOB == "Cefor").Sum(v => v.QtdPassageiros ?? 0)
                })
                .OrderBy(x => x.mesNum)
                .ToList();

            // [DADOS] Labels do gráfico.
            var labels = comparativoMob.Select(x => x.mes).ToList();

            // [DADOS] Séries do gráfico.
            var series = new List<SerieGraficoDto>
            {
                new() { Nome = "PGR", Cor = "#3b82f6", Valores = comparativoMob.Select(x => x.pgr).ToList() },
                new() { Nome = "Rodoviária", Cor = "#f97316", Valores = comparativoMob.Select(x => x.rodoviaria).ToList() },
                new() { Nome = "Cefor", Cor = "#8b5cf6", Valores = comparativoMob.Select(x => x.cefor).ToList() }
            };

            var dados = new GraficoComparativoDto
            {
                Titulo = "Comparativo Mensal por MOB",
                Subtitulo = $"Ano: {filtro.Ano ?? DateTime.Now.Year}",
                Labels = labels,
                Series = series,
                Filtro = filtro
            };

            // [RETORNO] Gera PDF.
            return _pdfService.GerarComparativoMob(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarComparativoMob", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarComparativoMob", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== USUÁRIOS DIA SEMANA ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarUsuariosDiaSemana (Helper)                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta PDF com gráfico de barras por dia da semana.                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarUsuariosDiaSemana(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Carrega viagens.
            var viagens = BuscarViagensEconomildo(filtro);

            // [DADOS] Agrupa por dia útil.
            var usuariosPorDiaSemana = viagens
                .Where(v => v.Data.HasValue)
                .GroupBy(v => v.Data!.Value.DayOfWeek)
                .Where(g => g.Key != DayOfWeek.Saturday && g.Key != DayOfWeek.Sunday)
                .Select(g => new ItemGraficoDto
                {
                    Label = ObterNomeDiaSemana(g.Key),
                    Valor = g.Sum(v => v.QtdPassageiros ?? 0)
                })
                .OrderBy(x => OrdemDiaSemana(x.Label))
                .ToList();

            // [REGRA] Calcula percentuais.
            var total = usuariosPorDiaSemana.Sum(d => d.Valor);
            foreach (var item in usuariosPorDiaSemana)
                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;

            var dados = new GraficoBarrasDto
            {
                Titulo = "Usuários por Dia da Semana",
                Subtitulo = filtro.NomeVeiculo,
                EixoX = "Dia",
                EixoY = "Usuários",
                Dados = usuariosPorDiaSemana,
                Filtro = filtro
            };

            // [RETORNO] Gera PDF.
            return _pdfService.GerarUsuariosDiaSemana(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarUsuariosDiaSemana", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarUsuariosDiaSemana", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== DISTRIBUIÇÃO HORÁRIO ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarDistribuicaoHorario (Helper)                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta PDF com distribuição de usuários por hora do dia.                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarDistribuicaoHorario(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Carrega viagens.
            var viagens = BuscarViagensEconomildo(filtro);

            // [DADOS] Agrupa por hora.
            var usuariosPorHora = viagens
                .Where(v => !string.IsNullOrEmpty(v.HoraInicio))
                .GroupBy(v => ExtrairHora(v.HoraInicio))
                .Where(g => g.Key >= 0)
                .Select(g => new ItemGraficoDto
                {
                    Label = g.Key.ToString("00") + ":00",
                    Valor = g.Sum(v => v.QtdPassageiros ?? 0)
                })
                .OrderBy(x => int.Parse(x.Label.Substring(0, 2)))
                .ToList();

            // [REGRA] Calcula percentuais.
            var total = usuariosPorHora.Sum(d => d.Valor);
            foreach (var item in usuariosPorHora)
                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;

            var dados = new GraficoBarrasDto
            {
                Titulo = "Distribuição por Horário",
                Subtitulo = filtro.NomeVeiculo,
                EixoX = "Horário",
                EixoY = "Usuários",
                Dados = usuariosPorHora,
                Filtro = filtro
            };

            // [RETORNO] Gera PDF.
            return _pdfService.GerarDistribuicaoHorario(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarDistribuicaoHorario", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarDistribuicaoHorario", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== TOP VEÍCULOS ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GerarTopVeiculos (Helper)                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Exporta PDF com os 10 veículos com mais viagens no período.               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • filtro (FiltroEconomildoDto): Filtro aplicado.                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • byte[]: PDF gerado.                                                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private byte[] GerarTopVeiculos(FiltroEconomildoDto filtro)
    {
        try
        {
            // [DADOS] Carrega viagens.
            var viagens = BuscarViagensEconomildo(filtro);

            // [DADOS] Calcula top 10 por quantidade.
            var topVeiculos = viagens
                .Where(v => v.VeiculoId != Guid.Empty)
                .GroupBy(v => v.VeiculoId)
                .Select(g => new
                {
                    veiculoId = g.Key,
                    total = g.Count()
                })
                .OrderByDescending(x => x.total)
                .Take(10)
                .ToList();

            // [DADOS] Carrega placas por ID.
            var veiculoIds = topVeiculos.Select(v => v.veiculoId).ToList();
            var veiculos = _unitOfWork.ViewVeiculos
                .GetAll(v => veiculoIds.Contains(v.VeiculoId))
                .ToDictionary(v => v.VeiculoId, v => v.Placa ?? "S/N");

            var dadosVeiculos = topVeiculos
                .Select(v => new ItemGraficoDto
                {
                    Label = veiculos.ContainsKey(v.veiculoId) ? veiculos[v.veiculoId] : "S/N",
                    Valor = v.total
                })
                .ToList();

            // [REGRA] Calcula percentuais.
            var total = dadosVeiculos.Sum(d => d.Valor);
            foreach (var item in dadosVeiculos)
                item.Percentual = total > 0 ? (double)item.Valor / total * 100 : 0;

            var dados = new GraficoBarrasDto
            {
                Titulo = "Top 10 Veículos",
                Subtitulo = filtro.NomeVeiculo,
                EixoX = "Veículo",
                EixoY = "Viagens",
                Dados = dadosVeiculos,
                Filtro = filtro
            };

            // [RETORNO] Gera PDF.
            return _pdfService.GerarTopVeiculos(dados);
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.GerarTopVeiculos", ex);
            Alerta.TratamentoErroComLinha("RelatoriosController.cs", "GerarTopVeiculos", ex);
            return Array.Empty<byte>();
        }
    }

    #endregion

    #region ==================== MÉTODOS AUXILIARES ====================

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ClassificarTurno (Helper)                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Classifica uma hora em turnos (Manhã/Tarde/Noite).                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • horaInicio (string?): Hora em formato texto.                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • string: Turno identificado.                                             ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private string ClassificarTurno(string? horaInicio)
    {
        try
        {
            // [VALIDACAO] Hora vazia retorna padrão.
            if (string.IsNullOrEmpty(horaInicio)) return "Manhã";

            if (TimeSpan.TryParse(horaInicio, out var hora))
            {
                // [REGRA] Classificação por faixa horária.
                if (hora.Hours >= 6 && hora.Hours < 12) return "Manhã";
                if (hora.Hours >= 12 && hora.Hours < 18) return "Tarde";
                return "Noite";
            }

            // [RETORNO] Padrão.
            return "Manhã";
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.ClassificarTurno", ex);
            return "Manhã";
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ExtrairHora (Helper)                                              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extrai a hora (inteiro) de uma string de horário.                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📥 PARÂMETROS:                                                               ║
    /// ║    • horaStr (string?): Hora em formato texto.                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📤 RETORNO:                                                                  ║
    /// ║    • int: Hora extraída ou -1.                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    private int ExtrairHora(string? horaStr)
    {
        try
        {
            // [VALIDACAO] Hora vazia retorna inválido.
            if (string.IsNullOrEmpty(horaStr)) return -1;

            if (TimeSpan.TryParse(horaStr, out var hora))
            {
                // [RETORNO] Hora extraída.
                return hora.Hours;
            }

            // [RETORNO] Fallback inválido.
            return -1;
        }
        catch (Exception ex)
        {
            _log.Error("RelatoriosController.ExtrairHora", ex);
            return -1;
        }
    }

    private string ObterNomeDiaAbreviado(int diaSemana) => diaSemana switch
    {
        0 => "Seg", 1 => "Ter", 2 => "Qua", 3 => "Qui", 4 => "Sex", 5 => "Sáb", 6 => "Dom", _ => ""
    };

    private string ObterNomeMes(int mes)
    {
        var nomes = new[] { "", "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
        return mes >= 1 && mes <= 12 ? nomes[mes] : "";
    }

    private int ObterNumeroMes(string nomeMes) => nomeMes switch
    {
        "Jan" => 1, "Fev" => 2, "Mar" => 3, "Abr" => 4, "Mai" => 5, "Jun" => 6,
        "Jul" => 7, "Ago" => 8, "Set" => 9, "Out" => 10, "Nov" => 11, "Dez" => 12, _ => 0
    };

    private string ObterNomeDiaSemana(DayOfWeek dia) => dia switch
    {
        DayOfWeek.Monday => "Seg",
        DayOfWeek.Tuesday => "Ter",
        DayOfWeek.Wednesday => "Qua",
        DayOfWeek.Thursday => "Qui",
        DayOfWeek.Friday => "Sex",
        DayOfWeek.Saturday => "Sáb",
        DayOfWeek.Sunday => "Dom",
        _ => ""
    };

    private int OrdemDiaSemana(string dia) => dia switch
    {
        "Seg" => 1, "Ter" => 2, "Qua" => 3, "Qui" => 4, "Sex" => 5, "Sáb" => 6, "Dom" => 7, _ => 0
    };

    #endregion
}
