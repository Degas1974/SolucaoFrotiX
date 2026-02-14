# Documentação: Administração e Gestão de Performance da Frota (AdministracaoController)

O \AdministracaoController\ funciona como o cérebro analítico do FrotiX, consolidando dados de diversas áreas (veículos, motoristas, viagens e normalizações) para fornecer uma visão de alto nível aos gestores do sistema. Ele não lida com o "dia a dia" operacional, mas sim com a inteligência de dados gerada por essas operações.

## 1. Visão Geral da Frota (KPIs Macro)

O endpoint \ObterResumoGeralFrota\ extrai os quatro pilares fundamentais da saúde da frota no período selecionado: quantidade de veículos ativos, motoristas aptos, volume de viagens e quilometragem total percorrida.

\\\csharp
// Exemplo de cálculo do total de KM sem filtros de "outliers" para visão administrativa pura
var totalKm = await _context.Viagem
    .AsNoTracking()
    .Where(v => v.DataInicial >= dataInicio &&
                v.DataInicial <= dataFimAjustada &&
                v.KmRodado != null &&
                v.KmRodado > 0)
    .SumAsync(v => (decimal?)(v.KmRodado) ?? 0);
\\\

Diferente dos dashboards técnicos, aqui a quilometragem é somada de forma bruta para refletir exatamente o que consta no banco de dados, servindo como base para auditorias e conferência de custos.

## 2. Monitoramento de Normalização de Dados

Um dos recursos mais críticos e exclusivos do FrotiX é a sua capacidade de "autocorreção" ou normalização. O endpoint \ObterEstatisticasNormalizacao\ quantifica quantas viagens foram detectadas como inconsistentes e corrigidas pelo sistema.

A lógica de agrupamento revela quais problemas são mais recorrentes na operação:
- **Viagem Aberta/Incoerente:** Quando o odômetro final é menor que o inicial.
- **Odômetro Travado:** Quando o veículo não registra deslocamento.
- **Troca de Veículo no Meio da Rota:** Detectado por saltos geográficos ou temporais.

\\\csharp
var porTipoNormalizacao = viagens
    .Where(v => v.FoiNormalizada == true && !string.IsNullOrEmpty(v.TipoNormalizacao))
    .GroupBy(v => v.TipoNormalizacao)
    .Select(g => new { tipo = g.Key, quantidade = g.Count() })
    .OrderByDescending(x => x.quantidade)
    .ToList();
\\\

Essa visão permite que o administrador identifique, por exemplo, se uma série de falhas de odômetro está ocorrendo em um modelo específico de veículo, sinalizando um problema de hardware ou manutenção.

## 3. Matriz de Intensidade (Heatmap)

Para otimizar a escala de motoristas e a disponibilidade de veículos, o sistema gera uma matriz de calor (7 dias por 24 horas) através do \ObterHeatmapViagens\. 

A inteligência aqui reside na conversão de fuso horário e índices para o padrão brasileiro (Segunda a Domingo):

\\\csharp
// Conversão de Domingo (0) para o final da fila (6) para alinhar com o gráfico PT-BR
var viagensConvertidas = viagens.Select(v => new
{
    DiaSemana = v.DiaSemana == 0 ? 6 : v.DiaSemana - 1, 
    v.Hora
});
\\\

Este mapa permite visualizar os horários de pico onde a frota está 100% comprometida e as janelas de "tempo morto" ideais para manutenções preventivas.

## 4. Distribuição de Uso e Rankings

O controller também analisa a composição da frota entre **Própria** e **Terceirizada**. Caso não existam configurações específicas no \VeiculoPadraoViagem\, o sistema utiliza o campo \VeiculoProprio\ como fallback, garantindo que o gráfico nunca apareça vazio.

Por fim, o \ObterTop10VeiculosPorKm\ identifica os "cavalos de batalha" da frota — os veículos que mais rodam. Esse ranking é essencial para o planejamento de desmobilização (venda de ativos) e revisão de contratos de locação.

---

### Notas de Implementação (Padrão FrotiX)

*   **Try-Catch Robusto:** Todas as Actions retornam um objeto JSON com \sucesso: false\ e um objeto de dados zerado em caso de erro, prevenindo falhas de renderização no frontend (Chart.js/Syncfusion).
*   **AsNoTracking:** Uso extensivo para garantir que consultas analíticas pesadas não sobrecarreguem o contexto do Entity Framework.
*   **Ajuste de Data (Ticks):** As datas de fim são ajustadas com \.AddDays(1).AddTicks(-1)\ para incluir todo o dia selecionado (23:59:59).

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*
