# Documentação: Dashboard de Higienização e Conservação (DashboardLavagemController)

O \DashboardLavagemController\ é a ferramenta de gestão para o controle de limpeza da frota. Ele monitora a produtividade da equipe de lavadores e garante que os ativos (veículos) estejam recebendo a manutenção estética necessária. Este painel é vital para o controle de contratos de prestação de serviços de limpeza e para a preservação do patrimônio da instituição.

## 1. KPIs Operacionais e Destaques

O endpoint \EstatisticasGerais\ consolida a produtividade do pátio. Um recurso inteligente aqui é a identificação do **Veículo Mais Lavado**, que inclui o selo **"(PM)"** caso o veículo pertença ao sistema de Placa Bronze. Isso permite identificar se veículos prioritários estão sendo higienizados com a frequência correta.

\\\csharp
// Identificação semântica de veículos prioritários no dashboard
var veiculoMaisLavado = lavagens
    .GroupBy(l => new { l.VeiculoId, Placa = l.Veiculo?.Placa, IsPM = l.Veiculo?.PlacaBronzeId != null })
    .Select(g => new { 
        Placa = g.Key.IsPM ? $"{{g.Key.Placa}} (PM)" : g.Key.Placa ?? "N/A", 
        Quantidade = g.Count() 
    })
    .OrderByDescending(x => x.Quantidade).FirstOrDefault();
\\\

## 2. Padrões de Demanda (Dia e Hora)

Através dos endpoints \LavagensPorDiaSemana\ e \LavagensPorHorario\, o gestor consegue visualizar o **Horário de Pico**. Essa informação é essencial para o escalonamento de lavadores: se o pico ocorre entre 07:00 e 09:00 (saída das equipes), o gestor pode reforçar o turno da madrugada/manhã para garantir que a frota saia limpa para a operação.

## 3. Monitoramento de Produtividade (Top Lavadores)

O endpoint \TopLavadores\ mergulha na tabela de relacionamento \LavadoresLavagem\. Como o FrotiX permite que múltiplos lavadores atuem em um mesmo veículo (lavagem em equipe), o sistema contabiliza individualmente a participação de cada profissional, permitindo premiações por desempenho ou identificação de gargalos na equipe.

## 4. Evolução Mensal e Comparação

O sistema mantém a lógica de **Comparação de Período**, informando se o volume de lavagens atual está acima ou abaixo da média histórica. Isso ajuda a identificar sazonalidades (ex: períodos de chuva que triplicam a demanda) e a auditar o consumo de insumos de limpeza.

---

### Notas de Implementação (Padrão FrotiX)

*   **Deep Including:** O controller utiliza de forma extensiva o \.Include().ThenInclude()\ para navegar desde a Lavagem até o Contrato do Lavador, garantindo que os dados de vinculação contratual estejam sempre corretos.
*   **Segurança de Dados:** O acesso é restrito via \[Authorize]\, garantindo que apenas usuários autenticados (tipicamente gestores de logística e supervisores de pátio) acessem as estatísticas de desempenho.
*   **Encarregamento de Gráficos:** Os dados são entregues em formatos otimizados para o ApexCharts, com arrays de dias da semana (\Dom\, \Seg\, etc.) e janelas de 24 horas pré-calculadas.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*
