# Documentação: Hub de Gestão de Capital Humano (DashboardMotoristasController)

O \DashboardMotoristasController\ é o painel analítico mais denso do FrotiX, com foco exclusivo na performance, segurança e conformidade legal da equipe de condutores. Com mais de 1400 linhas de código, ele consolida dados de múltiplas fontes (Viagens, Abastecimentos, Multas e RH) para oferecer uma visão clara da produtividade e dos riscos operacionais.

## 1. Estratégia Híbrida de Performance (Cached Analytics)

Para garantir que o dashboard carregue instantaneamente mesmo em empresas com centenas de motoristas e milhares de viagens, o controller utiliza uma estratégia de **Dados Pré-Calculados**. 

O endpoint \ObterEstatisticasGerais\ tenta primeiro buscar dados na tabela \EstatisticaGeralMensal\. Se as estatísticas do mês solicitado ainda não foram processadas, ele executa um \Fallback\ automático para as tabelas originais (\Viagem\, \Abastecimento\, etc.).

\\\csharp
// Priorização de performance: Tabela estatística > Fallback raw query
var estatGeral = await _context.EstatisticaGeralMensal
    .AsNoTracking()
    .Where(e => e.Ano == ano && e.Mes == mes)
    .FirstOrDefaultAsync();

if (estatGeral != null) {
    // Retorna dados instantâneos já somados
    return Json(new { success = true, kmTotal = estatGeral.KmTotal, ... });
}
\\\

## 2. Monitoramento de Risco e CNH (Zero Vencimento)

Diferente das estatísticas de viagens, o monitoramento de CNH é sempre realizado em **tempo real**, pois a validade de uma licença muda a cada dia. O controller identifica proativamente:
- **CNH Vencidas:** Condutores que não podem mais operar veículos.
- **CNH Vencendo em 30 Dias:** Alertas preventivos para renovação, evitando a indisponibilidade súbita de motoristas na escala.

## 3. Rankings de Performance e Segurança

O controller expõe diversos endpoints de "Top 10" que servem para identificar padrões positivos e negativos:
- **Top Quilometragem e Viagens:** Identifica os motoristas mais produtivos.
- **Top Multas (Quantidade e Valor):** Identifica condutores que precisam de treinamento em direção defensiva ou reavaliação de conduta.
- **Top Abastecimentos:** Ajuda a monitorar o consumo médio e possíveis anomalias em cartões de combustível.

## 4. O Mapa de Disponibilidade (Heatmap)

O endpoint \ObterHeatmapViagensPorMotorista\ gera uma matriz visual de atividade. Isso permite ao gestor de RH visualizar se um motorista está sendo sobrecarregado (muitas horas consecutivas) ou se há ociosidade na escala, garantindo o cumprimento das leis de jornada de trabalho e o descanso obrigatório.

---

### Notas de Implementação (Padrão FrotiX)

*   **Logic Isolation:** As lógicas complexas de cálculo foram movidas para métodos privados (\Fallback\) para manter os endpoints limpos e legíveis.
*   **AsNoTracking:** Uso rigoroso em todas as consultas de leitura para não sobrecarregar o rastreamento do Entity Framework.
*   **Distinct Querying:** Os filtros de "Anos e Meses Disponíveis" são buscados de forma distinta, garantindo que o dropdown da interface mostre apenas períodos que realmente possuem dados registrados.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
