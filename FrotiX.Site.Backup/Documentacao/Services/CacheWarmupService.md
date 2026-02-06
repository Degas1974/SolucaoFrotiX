# CacheWarmupService.cs

> **Última Atualização**: 23/01/2026 11:55  
> **Versão**: 1.1  
> **Documentação Intra-Código**: ✅ Completa (Cards adicionados)

---

# PARTE 2: LOG DE MODIFICAÇÕES

## [23/01/2026 11:55] - Documentação Intra-Código Completa

**Descrição**: Adicionados Cards de documentação padrão RegrasDesenvolvimentoFrotiX.md  
**Arquivos Afetados**: CacheWarmupService.cs, CacheWarmupService.md  
**Status**: ✅ Concluído

---

# PARTE 1: VISÃO GERAL

Serviço de **background** (`IHostedService`) que pré-carrega dados frequentes em cache de memória. Executa warm-up bloqueante na inicialização e depois atualiza o cache periodicamente em background.

## Localização

`Services/CacheWarmupService.cs`

## Dependências

- `Microsoft.Extensions.Hosting` (`IHostedService`)
- `Microsoft.Extensions.Caching.Memory` (`IMemoryCache`)
- `Microsoft.Extensions.DependencyInjection` (`IServiceProvider`)
- `Microsoft.EntityFrameworkCore` (`ToListAsync`)
- `FrotiX.Repository.IRepository` (`IUnitOfWork`)
- `FrotiX.Infrastructure` (`CacheKeys`)
- `FrotiX.Models.DTO` (`MotoristaData`, `VeiculoData`)

## Características

### Hosted Service

- Implementa `IHostedService` para execução em background
- Inicia automaticamente com a aplicação
- Para graciosamente quando aplicação encerra

### Cache Strategy

- **TTL**: 30 minutos
- **Refresh Interval**: 10 minutos (atualiza antes de expirar)
- **Priority**: `CacheItemPriority.High` (não é removido sob pressão de memória)

---

## Métodos Principais

### `StartAsync(CancellationToken)`

**Propósito**: Inicia o serviço de warm-up.

**Fluxo**:

1. Cria `CancellationTokenSource` vinculado ao token de cancelamento
2. **Warm-up bloqueante**: Executa `WarmAsync()` e aguarda conclusão
   - Garante que cache está pronto antes de atender requisições
3. **Loop de refresh**: Inicia `RefreshLoopAsync()` em background
   - Atualiza cache a cada 10 minutos

**Complexidade**: Baixa (orquestração)

---

### `StopAsync(CancellationToken)`

**Propósito**: Para o serviço graciosamente.

**Fluxo**:

1. Cancela `CancellationTokenSource`
2. Aguarda conclusão do loop de refresh
3. Ignora exceções durante parada

**Complexidade**: Baixa

---

### `WarmAsync(CancellationToken)` (privado)

**Propósito**: Carrega dados no cache.

**Dados Carregados**:

#### 1. Motoristas

- **Fonte**: `ViewMotoristas`
- **Projeção**: `{ MotoristaId, Nome }`
- **Ordenação**: Por nome (SQL)
- **Chave Cache**: `CacheKeys.Motoristas`
- **DTO**: `List<MotoristaData>`

#### 2. Veículos

- **Fonte**: `ViewVeiculosManutencao`
- **Projeção**: `{ VeiculoId, Descricao }`
- **Ordenação**: Por descrição (SQL)
- **Chave Cache**: `CacheKeys.Veiculos`
- **DTO**: `List<VeiculoData>`

#### 3. Veículos Reserva (opcional)

- **Fonte**: `ViewVeiculosManutencaoReserva`
- **Projeção**: `{ VeiculoId, Descricao }`
- **Ordenação**: Por descrição (SQL)
- **Chave Cache**: `CacheKeys.VeiculosReserva`
- **DTO**: `List<VeiculoData>`
- **Nota**: Pode ser comentado se não usar reserva

**Otimizações**:

- Usa `GetAllReducedIQueryable` com `asNoTracking: true`
- Executa ordenação no SQL (não em memória)
- Usa `Select()` para projetar apenas campos necessários
- Usa `ToListAsync()` para materialização assíncrona

**Logging**: Registra quantidade de motoristas e veículos carregados

**Complexidade**: Média-Alta (consultas otimizadas ao banco)

---

### `RefreshLoopAsync(CancellationToken)` (privado)

**Propósito**: Loop infinito que atualiza cache periodicamente.

**Fluxo**:

1. Cria `PeriodicTimer` com intervalo de 10 minutos
2. Loop: aguarda próximo tick → executa `WarmAsync()` → repete
3. Para quando `CancellationToken` é cancelado

**Complexidade**: Baixa

---

### `Set<T>(string key, List<T> value)` (privado)

**Propósito**: Armazena lista no cache com configurações padrão.

**Configurações**:

- `AbsoluteExpirationRelativeToNow`: 30 minutos
- `Priority`: `High` (não é removido sob pressão)

**Complexidade**: Baixa

---

## Chaves de Cache (`CacheKeys`)

Definidas em `Infrastructure/CacheKeys.cs`:

- `CacheKeys.Motoristas`: Lista de motoristas
- `CacheKeys.Veiculos`: Lista de veículos
- `CacheKeys.VeiculosReserva`: Lista de veículos reserva

---

## DTOs

### `MotoristaData`

```csharp
public record MotoristaData(Guid MotoristaId, string Nome);
```

### `VeiculoData`

```csharp
public record VeiculoData(Guid VeiculoId, string Descricao);
```

---

## Contribuição para o Sistema FrotiX

### ⚡ Performance

- **Reduz latência**: Dados frequentes já estão em memória
- **Reduz carga no banco**: Menos consultas repetidas
- **Melhora UX**: Dropdowns e autocompletes respondem instantaneamente

### 🔄 Atualização Automática

- Cache é atualizado automaticamente a cada 10 minutos
- Garante dados relativamente atualizados sem intervenção manual

### 🚀 Startup Otimizado

- Warm-up bloqueante garante cache pronto antes de atender requisições
- Primeira requisição já tem dados em cache

## Observações Importantes

1. **Warm-up Bloqueante**: O warm-up inicial é bloqueante (`await WarmAsync()`). Se a consulta demorar muito, pode atrasar o startup da aplicação. Considere timeout ou warm-up não bloqueante.

2. **Escopo de Serviços**: Usa `IServiceProvider.CreateScope()` para criar escopo isolado e obter `IUnitOfWork`. Isso é necessário porque `IUnitOfWork` é scoped.

3. **Veículos Reserva**: O código carrega veículos reserva, mas há comentário indicando que pode ser removido se não usar. Verifique se é necessário.

4. **Error Handling**: Não há tratamento de exceções explícito. Se `WarmAsync()` falhar, o serviço pode não iniciar corretamente.

5. **Logging**: Registra informações úteis sobre quantidade de dados carregados. Use logs para monitorar performance.

6. **Memory Pressure**: Cache tem prioridade `High`, então não é removido sob pressão de memória. Se houver problemas de memória, considere reduzir TTL ou prioridade.

## Registro no DI Container

```csharp
// Startup.cs ou Program.cs
services.AddHostedService<CacheWarmupService>();
```

## Arquivos Relacionados

- `Infrastructure/CacheKeys.cs`: Define chaves de cache
- `Models/DTO/MotoristaData.cs`: DTO de motorista
- `Models/DTO/VeiculoData.cs`: DTO de veículo
- `Repository/IRepository/`: Acessa dados via `IUnitOfWork`

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

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
