/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RelatorioEconomildoDto.cs                                                               ║
   ║ 📂 CAMINHO: /Services/Pdf                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTOs para Dashboard Economildo. Filtros, HeatmapDto, GraficoBarrasDto, GraficoPizzaDto ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: TipoRelatorioEconomildo (enum), FiltroEconomildoDto, HeatmapDto, GraficoComparativoDto   ║
   ║ 🔗 DEPS: Nenhuma (POCOs) | 📅 14/01/2026 | 👤 Copilot | 📝 v2.0                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Services.Pdf;

/// <summary>
/// Tipos de relatório disponíveis para o Dashboard Economildo
/// </summary>
public enum TipoRelatorioEconomildo
{
    UsuariosMes,
    UsuariosTurno,
    ComparativoMob,
    UsuariosDiaSemana,
    DistribuicaoHorario,
    TopVeiculos,
    HeatmapViagens,
    HeatmapPassageiros
}

/// <summary>
/// Filtros compartilhados por todos os relatórios
/// </summary>
public class FiltroEconomildoDto
{
    public string? Mob { get; set; }
    public int? Mes { get; set; }
    public int? Ano { get; set; }
    
    public DateTime DataInicio => Ano.HasValue && Mes.HasValue
        ? new DateTime(Ano.Value, Mes.Value, 1)
        : Ano.HasValue
            ? new DateTime(Ano.Value, 1, 1)
            : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

    public DateTime DataFim => Ano.HasValue && Mes.HasValue
        ? new DateTime(Ano.Value, Mes.Value, 1).AddMonths(1).AddDays(-1)
        : Ano.HasValue
            // Se é o ano atual e não tem mês, mostra até hoje; senão até 31/12
            ? (Ano.Value == DateTime.Now.Year ? DateTime.Now : new DateTime(Ano.Value, 12, 31))
            : DateTime.Now;

    public string PeriodoFormatado => $"{DataInicio:dd/MM/yyyy} a {DataFim:dd/MM/yyyy}";
    
    public string NomeVeiculo => string.IsNullOrEmpty(Mob) ? "Economildo - Todos" : $"Economildo {Mob}";
}

/// <summary>
/// DTO para Mapa de Calor (Viagens e Passageiros)
/// </summary>
public class HeatmapDto
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
    public string UnidadeLegenda { get; set; } = ""; // "viagens" ou "passageiros"
    public int[,] Valores { get; set; } = new int[7, 24];
    public int ValorMaximo { get; set; }
    public string DiaPico { get; set; } = "";
    public int HoraPico { get; set; }
    public string HorarioPicoManha { get; set; } = "";
    public string DiaMaisMovimentado { get; set; } = "";
    public string PeriodoOperacao { get; set; } = "";
    public FiltroEconomildoDto Filtro { get; set; } = new();
}

/// <summary>
/// DTO para gráficos de barras/linha (dados label + valor)
/// </summary>
public class GraficoBarrasDto
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
    public string EixoX { get; set; } = "";
    public string EixoY { get; set; } = "";
    public List<ItemGraficoDto> Dados { get; set; } = new();
    public FiltroEconomildoDto Filtro { get; set; } = new();
    public int Total => Dados.Sum(d => d.Valor);
}

/// <summary>
/// Item individual de um gráfico
/// </summary>
public class ItemGraficoDto
{
    public string Label { get; set; } = "";
    public int Valor { get; set; }
    public string? Cor { get; set; }
    public double Percentual { get; set; }
}

/// <summary>
/// DTO para gráfico de pizza/rosca (Turno)
/// </summary>
public class GraficoPizzaDto
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
    public List<ItemGraficoDto> Dados { get; set; } = new();
    public FiltroEconomildoDto Filtro { get; set; } = new();
    public int Total => Dados.Sum(d => d.Valor);
}

/// <summary>
/// DTO para comparativo por MOB (múltiplas séries)
/// </summary>
public class GraficoComparativoDto
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
    public List<string> Labels { get; set; } = new(); // Meses
    public List<SerieGraficoDto> Series { get; set; } = new(); // PGR, Rodoviária, Cefor
    public FiltroEconomildoDto Filtro { get; set; } = new();
}

/// <summary>
/// Série de dados para gráfico comparativo
/// </summary>
public class SerieGraficoDto
{
    public string Nome { get; set; } = "";
    public string Cor { get; set; } = "";
    public List<int> Valores { get; set; } = new();
    public int Total => Valores.Sum();
}
