# Documentação: ViagemRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ViagemRepository` é um repository específico para a entidade `Viagem`, estendendo o repository genérico com métodos customizados para operações complexas de viagens, incluindo normalização de dados, paginação otimizada e busca de recorrências.

**Principais características:**

✅ **Herança**: Herda de `Repository<Viagem>`  
✅ **Interface Específica**: Implementa `IViagemRepository`  
✅ **Normalização**: Métodos para correção de origens/destinos  
✅ **Paginação Otimizada**: Query otimizada com ViewViagens  
✅ **Recorrências**: Busca viagens relacionadas por evento

---

## Estrutura da Classe

### Herança e Implementação

```csharp
public class ViagemRepository : Repository<Viagem>, IViagemRepository
```

---

## Métodos Específicos

### `GetViagemListForDropDown()`

**Descrição**: Retorna lista de viagens formatada para DropDownList

**Retorno**: `IEnumerable<SelectListItem>`

**Ordenação**: Por `DataInicial`

**Formato**: `Descricao` como texto, `ViagemId` como valor

---

### `Update(Viagem viagem)`

**Descrição**: Atualiza viagem com lógica específica

**Nota**: ⚠️ Chama `SaveChanges()` diretamente (inconsistente com padrão)

---

### `GetDistinctOrigensAsync()`

**Descrição**: **MÉTODO CRÍTICO** - Retorna lista distinta de origens

**Retorno**: `Task<List<string>>`

**Filtro**: Apenas origens não vazias

**Ordenação**: Alfabética

**Uso**: Para normalização e autocomplete

---

### `GetDistinctDestinosAsync()`

**Descrição**: Retorna lista distinta de destinos

**Retorno**: `Task<List<string>>`

**Filtro**: Apenas destinos não vazios

**Ordenação**: Alfabética

---

### `CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem)`

**Descrição**: **MÉTODO CRÍTICO** - Corrige múltiplas origens para um valor único

**Parâmetros**:
- `origensAntigas`: Lista de valores antigos a serem corrigidos
- `novaOrigem`: Valor único de substituição

**Lógica**:
1. Busca todas viagens com origens na lista antiga
2. Atualiza `Origem` para `novaOrigem`
3. Salva mudanças

**Uso**: Para normalização em massa de dados

**Exemplo**:
```csharp
await repository.CorrigirOrigemAsync(
    new List<string> { "Local A", "Local B", "Local C" },
    "Local Unificado"
);
```

---

### `CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino)`

**Descrição**: Corrige múltiplos destinos para um valor único

**Lógica**: Similar a `CorrigirOrigemAsync`

---

### `BuscarViagensRecorrenciaAsync(Guid id)`

**Descrição**: Busca viagens de recorrência relacionadas

**Lógica**:
1. Busca viagem original pelo ID
2. Se tem `EventoId`, retorna todas viagens do mesmo evento
3. Caso contrário, retorna apenas a viagem original

**Uso**: Para gerenciar viagens recorrentes (diárias, semanais, etc.)

---

### `GetViagensEventoPaginadoAsync(...)`

**Descrição**: **MÉTODO OTIMIZADO** - Query otimizada para lista paginada de viagens de evento

**Assinatura**:
```csharp
Task<(List<ViagemEventoDto> viagens, int totalItems)> GetViagensEventoPaginadoAsync(
    Guid eventoId,
    int page,
    int pageSize
)
```

**Otimizações**:
1. **COUNT Separado**: Conta total antes de buscar dados
2. **Pagination First**: Busca apenas IDs paginados primeiro
3. **View Otimizada**: Usa `ViewViagens` em vez de JOINs complexos
4. **Reordenação Cliente**: Reordena no cliente (poucos registros)

**Performance**: Inclui logging de tempo de execução

**Retorno**: Tupla com lista de viagens e total de itens

---

### `GetQuery(Expression<Func<Viagem, bool>> filter = null)`

**Descrição**: Retorna `IQueryable` para composição de queries

**Uso**: Para operações que só precisam de `Count()`, `Min()`, `Max()`, etc.

**Vantagem**: Não materializa até execução explícita

---

## Interconexões

### Quem Usa Este Repository

- **ViagemController**: CRUD e operações complexas de viagens
- **ViagemLimpezaController**: Para normalização de dados
- **DashboardViagensController**: Para estatísticas

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ViagemRepository

**Arquivos Afetados**:
- `Repository/ViagemRepository.cs`

**Impacto**: Documentação de referência para repository de viagens

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - Manutencao: Remocao de using duplicado do EF Core

**Descricao**: Removida duplicidade de `using Microsoft.EntityFrameworkCore` no cabecalho para eliminar warnings CS0105.

**Arquivos Afetados**:
- Repository/ViagemRepository.cs

**Mudancas**:
- Remocao do `using Microsoft.EntityFrameworkCore` duplicado.

**Impacto**: Nenhuma mudanca funcional; apenas limpeza de compilacao.

**Status**: Concluido

**Responsavel**: Codex

**Versao**: Incremento de patch

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
