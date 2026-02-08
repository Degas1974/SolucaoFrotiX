# Documentação: EventoRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `EventoRepository` é um repository específico para a entidade `Evento`, com método altamente otimizado para paginação que inclui JOINs, cálculo de custos em batch e formatação de dados.

**Principais características:**

✅ **Herança**: Herda de `Repository<Evento>`  
✅ **Interface Específica**: Implementa `IEventoRepository`  
✅ **Paginação Otimizada**: Query complexa com JOINs e cálculos em batch  
✅ **Performance Monitoring**: Inclui logging de tempo de execução  
✅ **Cálculo de Custos**: Agrega custos de viagens relacionadas

---

## Métodos Específicos

### `GetEventoListForDropDown()`

**Descrição**: Retorna lista de eventos formatada para DropDownList

**Ordenação**: Por `Nome`

**Formato**: `Nome` como texto, `EventoId` como valor

---

### `Update(Evento evento)`

**Descrição**: Atualiza evento com lógica específica

**Nota**: ⚠️ Chama `SaveChanges()` diretamente (inconsistente com padrão)

---

### `GetEventosPaginadoAsync(...)`

**Descrição**: **MÉTODO CRÍTICO E OTIMIZADO** - Query complexa para listagem paginada de eventos com custos calculados

**Assinatura**:
```csharp
Task<(List<EventoListDto> eventos, int totalItems)> GetEventosPaginadoAsync(
    int page,
    int pageSize,
    string filtroStatus = null
)
```

**Retorno**: Tupla com lista de eventos formatados e total de itens

---

## Estrutura da Query Otimizada

### Etapa 1: Buscar Dados dos Eventos

**JOINs Realizados**:
- `Evento` LEFT JOIN `Requisitante` (via `RequisitanteId`)
- `Evento` LEFT JOIN `SetorSolicitante` (via `SetorSolicitanteId`)

**Campos Selecionados**:
- Dados do evento: `EventoId`, `Nome`, `Descricao`, `DataInicial`, `DataFinal`, `QtdParticipantes`, `Status`
- Dados relacionados: `NomeRequisitante`, `NomeSetor`

**Filtros**:
- Filtro opcional por `Status` se fornecido

**Paginação**:
- Ordenação: Por `DataInicial` descendente
- `Skip` e `Take` para paginação
- `AsNoTracking()` para performance

---

### Etapa 2: Calcular Custos (Batch)

**Estratégia**: Busca custos de todas as viagens relacionadas em uma única query

**Query de Agregação**:
```csharp
var custosPorEvento = await _db.Viagem
    .Where(v => eventoIds.Contains(v.EventoId.Value))
    .GroupBy(v => v.EventoId.Value)
    .Select(g => new {
        EventoId = g.Key,
        CustoTotal = g.Sum(v => v.CustoCombustivel ?? 0) +
                     g.Sum(v => v.CustoMotorista ?? 0) +
                     g.Sum(v => v.CustoVeiculo ?? 0) +
                     g.Sum(v => v.CustoOperador ?? 0) +
                     g.Sum(v => v.CustoLavador ?? 0)
    })
    .ToListAsync();
```

**Custos Agregados**:
- `CustoCombustivel`
- `CustoMotorista`
- `CustoVeiculo`
- `CustoOperador`
- `CustoLavador`

**Otimização**: Usa `Dictionary` para lookup rápido de custos por evento

---

### Etapa 3: Processar Formatações

**Formatações Aplicadas**:
- `QtdParticipantes`: Padding com zeros à esquerda (3 dígitos)
- `NomeRequisitanteHTML`: Conversão HTML via `Servicos.ConvertHtml()`
- `CustoViagem`: Formatação monetária `"R$ {0:N2}"`
- `CustoViagemNaoFormatado`: Valor numérico para ordenação

---

## Performance Monitoring

**Logging Incluído**:
- Tempo de query de eventos
- Tempo de cálculo de custos
- Tempo de formatação
- Tempo total do método

**Uso**: Para identificar gargalos de performance

**Exemplo de Log**:
```
[QUERY EVENTOS] 10/50 registros - 45ms
[CUSTOS] 10 eventos com custos - 12ms
[FORMATO] 10 registros - 2ms
[TOTAL REPOSITORY] 59ms
```

---

## Tratamento de Erros

**Try-Catch**: Captura exceções e registra:
- Mensagem de erro
- Stack trace completo
- Log via `Alerta.TratamentoErroComLinha()`

**Re-throw**: Re-lança exceção após logging

---

## Interconexões

### Quem Usa Este Repository

- **EventoController**: CRUD e listagem paginada de eventos
- **DashboardEventosController**: Para estatísticas de eventos

### O Que Este Repository Usa

- **FrotiX.Services**: `Servicos.ConvertHtml()` para conversão HTML
- **FrotiX.Helpers**: `Alerta.TratamentoErroComLinha()` para logging

---

## Exemplo de Uso

```csharp
var (eventos, totalItems) = await unitOfWork.Evento.GetEventosPaginadoAsync(
    page: 1,
    pageSize: 10,
    filtroStatus: "Ativo"
);

// eventos: List<EventoListDto> com dados formatados
// totalItems: Total de eventos (para paginação)
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do EventoRepository

**Arquivos Afetados**:
- `Repository/EventoRepository.cs`

**Impacto**: Documentação de referência para repository de eventos com método otimizado

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
- Repository/EventoRepository.cs

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
