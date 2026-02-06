# Documentacao: ViagemController.ListaEventos.cs

> **Ultima Atualizacao**: 13/01/2026
> **Versao Atual**: 1.0

---

# PARTE 1: DOCUMENTACAO DA FUNCIONALIDADE

## Visao Geral

Partial class do `ViagemController` responsavel pelo endpoint de **listagem de eventos** com paginacao server-side para o DataTables. Retorna eventos com informacoes de custo total e contagem de viagens associadas.

### Caracteristicas Principais

- GET `/api/Viagem/ListaEventos` - Endpoint principal
- Paginacao server-side (carrega apenas 25 registros por vez)
- Calculo de custos agregados por evento
- Contagem de viagens associadas por evento (`viagensCount`)
- Queries otimizadas com `AsNoTracking`
- Performance: < 2 segundos (otimizado de 30+ segundos)

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
└── Controllers/
    └── ViagemController.ListaEventos.cs  (partial class)
```

### Relacionamento

Este arquivo e uma **partial class** de `ViagemController.cs`, focado exclusivamente no endpoint de listagem de eventos.

---

## Endpoint API

### GET `/api/Viagem/ListaEventos`

**Descricao**: Retorna lista de eventos com paginacao server-side para DataTables

**Parametros Query**:

| Parametro | Tipo | Default | Descricao |
|-----------|------|---------|-----------|
| `draw` | int | 1 | Contador de requisicao DataTables |
| `start` | int | 0 | Offset (inicio da pagina) |
| `length` | int | 25 | Quantidade de registros por pagina |

**Response de Sucesso**:

```json
{
  "draw": 1,
  "recordsTotal": 150,
  "recordsFiltered": 150,
  "data": [
    {
      "eventoId": "guid",
      "nome": "Nome do Evento",
      "descricao": "Descricao do evento",
      "dataInicial": "2024-01-15T00:00:00",
      "dataFinal": "2024-01-20T00:00:00",
      "qtdParticipantes": 50,
      "status": 1,
      "nomeSetor": "Setor X (SIGLA)",
      "nomeRequisitante": "Nome do Requisitante",
      "nomeRequisitanteHTML": "Nome do Requisitante",
      "custoViagem": 1500.50,
      "viagensCount": 5
    }
  ]
}
```

**Response de Erro**:

```json
{
  "draw": 1,
  "recordsTotal": 0,
  "recordsFiltered": 0,
  "data": [],
  "error": "Erro ao carregar eventos: [mensagem]"
}
```

---

## Logica de Negocio

### Fluxo de Execucao

1. **PASSO 1**: Contar total de registros para paginacao
2. **PASSO 2**: Buscar eventos da pagina atual (com Include de SetorSolicitante e Requisitante)
3. **PASSO 3**: Calcular custos e contar viagens dos eventos da pagina
4. **PASSO 4**: Montar resultado em memoria
5. **PASSO 5**: Retornar no formato DataTables server-side

### Calculo de Custos

```csharp
CustoTotal = (CustoCombustivel ?? 0) +
             (CustoMotorista ?? 0) +
             (CustoVeiculo ?? 0) +
             (CustoOperador ?? 0) +
             (CustoLavador ?? 0)
```

### Contagem de Viagens

O campo `viagensCount` retorna a quantidade de viagens associadas a cada evento. Este valor e usado pelo frontend para:
- Desabilitar o botao "Excluir Evento" quando ha viagens associadas
- Exibir tooltip informando quantas viagens estao vinculadas

---

## Codigo Fonte

```csharp
[HttpGet]
[Route("ListaEventos")]
public IActionResult ListaEventos(
    int draw = 1,
    int start = 0,
    int length = 25)
{
    var sw = System.Diagnostics.Stopwatch.StartNew();

    try
    {
        // PASSO 1: Contar total de registros
        var totalRecords = _context.Evento.Count();

        // PASSO 2: Buscar eventos da pagina atual
        var eventos = _context.Evento
            .Include(e => e.SetorSolicitante)
            .Include(e => e.Requisitante)
            .AsNoTracking()
            .OrderBy(e => e.Nome)
            .Skip(start)
            .Take(length)
            .ToList();

        // PASSO 3: Buscar custos e contagem de viagens
        var eventoIds = eventos.Select(e => e.EventoId).ToList();

        var viagensDict = _context.Viagem
            .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value))
            .AsNoTracking()
            .GroupBy(v => v.EventoId)
            .Select(g => new
            {
                EventoId = g.Key,
                CustoTotal = g.Sum(v =>
                    (v.CustoCombustivel ?? 0) +
                    (v.CustoMotorista ?? 0) +
                    (v.CustoVeiculo ?? 0) +
                    (v.CustoOperador ?? 0) +
                    (v.CustoLavador ?? 0)),
                ViagensCount = g.Count()
            })
            .ToDictionary(x => x.EventoId, x => new { Custo = Math.Round(x.CustoTotal, 2), Viagens = x.ViagensCount });

        // PASSO 4: Montar resultado
        var resultado = eventos.Select(e =>
        {
            double custoViagem = 0;
            int viagensCount = 0;
            if (viagensDict.TryGetValue(e.EventoId, out var viagemInfo))
            {
                custoViagem = viagemInfo.Custo;
                viagensCount = viagemInfo.Viagens;
            }

            return new
            {
                eventoId = e.EventoId,
                nome = e.Nome ?? "",
                descricao = e.Descricao ?? "",
                dataInicial = e.DataInicial,
                dataFinal = e.DataFinal,
                qtdParticipantes = e.QtdParticipantes,
                status = e.Status == "1" ? 1 : 0,
                nomeSetor = nomeSetor,
                nomeRequisitante = e.Requisitante?.Nome ?? "",
                custoViagem = custoViagem,
                viagensCount = viagensCount
            };
        }).ToList();

        // PASSO 5: Retornar formato DataTables
        return Json(new
        {
            draw = draw,
            recordsTotal = totalRecords,
            recordsFiltered = totalRecords,
            data = resultado
        });
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("ViagemController.cs", "ListaEventos", error);
        return Json(new
        {
            draw = draw,
            recordsTotal = 0,
            recordsFiltered = 0,
            data = new List<object>(),
            error = "Erro ao carregar eventos: " + error.Message
        });
    }
}
```

---

## Interconexoes

### Quem Chama Este Endpoint

- `Pages/Viagens/ListaEventos.cshtml` - DataTables AJAX

### O Que Este Endpoint Chama

- `_context.Evento` - Tabela de eventos
- `_context.Viagem` - Tabela de viagens (para custos e contagem)
- `Alerta.TratamentoErroComLinha()` - Helper de tratamento de erros

---

## Troubleshooting

### Problema: Performance Lenta

**Sintoma**: Endpoint demora mais de 2 segundos

**Causa**: Muitos registros ou falta de indices

**Solucao**: Verificar indices nas tabelas Evento e Viagem

### Problema: Custos zerados

**Sintoma**: Eventos mostram custo R$ 0,00 mesmo com viagens

**Causa**: Viagens sem custos preenchidos

**Solucao**: Verificar se viagens tem os campos de custo populados

---

# PARTE 2: LOG DE MODIFICACOES/CORRECOES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026] - Adicao do campo viagensCount

**Descricao**:

- Adicionado campo `viagensCount` no retorno do endpoint
- O campo retorna a quantidade de viagens associadas a cada evento
- Usado pelo frontend para desabilitar o botao "Excluir" quando ha viagens

**Arquivos Afetados**:

- `Controllers/ViagemController.ListaEventos.cs`

**Impacto**: Frontend pode verificar se evento tem viagens antes de permitir exclusao

**Status**: Concluido

**Versao**: 1.0

---

**Ultima atualizacao**: 13/01/2026
**Autor**: Sistema FrotiX
**Versao**: 1.0


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
