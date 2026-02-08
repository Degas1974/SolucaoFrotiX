# VeiculoEstatisticaService.cs

## Visão Geral
Serviço para calcular **estatísticas de viagens por veículo** baseadas no histórico. Usado pela **IA evolutiva** para calibrar alertas de validação (detectar anomalias em quilometragem e duração de viagens).

## Localização
`Services/VeiculoEstatisticaService.cs`

## Dependências
- `FrotiX.Data` (`FrotiXDbContext`)
- `Microsoft.Extensions.Caching.Memory` (`IMemoryCache`)
- `Microsoft.EntityFrameworkCore` (`ToListAsync`, `AsNoTracking`)
- `FrotiX.Models.DTO` (`EstatisticaVeiculoDto`)

## Características

### Cache
- **TTL**: 10 minutos
- **Chave**: `"VeiculoEstatistica_{veiculoId}"`
- Reduz recálculos frequentes

### Histórico
- Considera últimas **100 viagens** finalizadas
- Filtra viagens com KM válido (KmFinal > KmInicial > 0)
- Filtra durações válidas (1 minuto a 24 horas)

---

## Métodos Principais

### `ObterEstatisticasAsync(Guid veiculoId)`
**Propósito**: Obtém estatísticas de um veículo (com cache).

**Fluxo**:
1. Verifica cache
2. Se não em cache: calcula estatísticas
3. Armazena no cache por 10 minutos
4. Retorna estatísticas

**Complexidade**: Média-Alta (consultas e cálculos estatísticos)

---

### `CalcularEstatisticasAsync(Guid veiculoId)` (privado)
**Propósito**: Calcula estatísticas baseadas no histórico.

**Estatísticas Calculadas**:

#### Quilometragem:
- `KmMedio`: Média aritmética
- `KmMediano`: Mediana
- `KmDesvioPadrao`: Desvio padrão
- `KmMinimo`, `KmMaximo`: Valores extremos
- `KmPercentil95`, `KmPercentil99`: Percentis

#### Duração:
- `DuracaoMediaMinutos`: Média
- `DuracaoMedianaMinutos`: Mediana
- `DuracaoDesvioPadraoMinutos`: Desvio padrão
- `DuracaoMinimaMinutos`, `DuracaoMaximaMinutos`: Extremos
- `DuracaoPercentil95Minutos`: Percentil 95

#### Metadados:
- `TotalViagens`: Quantidade de viagens analisadas
- `DataViagemMaisAntiga`, `DataViagemMaisRecente`: Período do histórico

**Complexidade**: Alta (cálculos estatísticos complexos)

---

### `InvalidarCache(Guid veiculoId)`
**Propósito**: Invalida cache de um veículo (chamar após finalizar viagem).

**Uso**: Chamar após criar/atualizar viagem para forçar recálculo.

---

## Contribuição para o Sistema FrotiX

### 🤖 IA Evolutiva
- Fornece dados estatísticos para calibração de alertas
- Detecta anomalias (viagens fora do padrão)
- Melhora precisão de validações automáticas

### 📊 Análises
- Permite análise de padrões de uso por veículo
- Identifica veículos com comportamento atípico
- Suporta tomada de decisão baseada em dados

## Observações Importantes

1. **Histórico Limitado**: Considera apenas últimas 100 viagens. Para veículos com muito histórico, pode não refletir padrão completo.

2. **Filtros Rigorosos**: Filtra viagens inválidas (KM negativo, duração extrema). Garante qualidade dos dados estatísticos.

3. **Cache**: Cache de 10 minutos pode não refletir mudanças recentes. Use `InvalidarCache()` após operações críticas.

4. **Performance**: Cálculos estatísticos podem ser lentos com muitos dados. Cache ajuda, mas considere otimizações para grandes volumes.

## Arquivos Relacionados
- `Models/DTO/EstatisticaVeiculoDto.cs`: DTO de estatísticas
- `Controllers/DashboardVeiculosController.cs`: Usa estatísticas para análises
- `Data/FrotiXDbContext.cs`: Acessa dados de viagens


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•S

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

---

### 2026-01-19 - Padronização e Validação
- **Alteração**: Ajustes de formatação e validação de conformidade (Standardization).
- **Responsável**: Agente Gemini/GitHub Copilot
