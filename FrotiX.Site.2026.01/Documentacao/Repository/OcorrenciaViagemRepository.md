# Documentação: OcorrenciaViagemRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `OcorrenciaViagemRepository` é um repository específico para a entidade `OcorrenciaViagem` que **NÃO herda de `Repository<T>`**, implementando diretamente a interface `IOcorrenciaViagemRepository`.

**Principais características:**

✅ **Implementação Direta**: Não herda de `Repository<T>`  
✅ **Interface Específica**: Implementa `IOcorrenciaViagemRepository`  
✅ **CRUD Customizado**: Implementa métodos CRUD diretamente  
✅ **Suporte a Includes**: Suporta eager loading via string CSV

---

## Estrutura da Classe

### Implementação

```csharp
public class OcorrenciaViagemRepository : IOcorrenciaViagemRepository
```

**Nota**: ⚠️ **Diferente do padrão** - Não herda de `Repository<OcorrenciaViagem>`

**Motivo**: Implementação customizada para operações específicas de ocorrências

---

## Métodos Implementados

### `GetAll(Expression<Func<OcorrenciaViagem, bool>>? filter = null, string? includeProperties = null)`

**Descrição**: Retorna todas as ocorrências de viagem com filtro e includes opcionais

**Parâmetros**:
- `filter`: Expressão lambda para filtro (opcional)
- `includeProperties`: String CSV de propriedades para eager loading (opcional)

**Retorno**: `IEnumerable<OcorrenciaViagem>` materializado

**Características**:
- Suporta filtros via `Where()`
- Processa includes via `Split(',')` e `Include()`
- Materializa resultado com `ToList()`

---

### `GetFirstOrDefault(Expression<Func<OcorrenciaViagem, bool>> filter, string? includeProperties = null)`

**Descrição**: Retorna primeira ocorrência que satisfaz o filtro

**Parâmetros**:
- `filter`: Expressão lambda para filtro (obrigatório)
- `includeProperties`: String CSV de propriedades para eager loading (opcional)

**Retorno**: `OcorrenciaViagem?` ou `null`

**Nota**: ⚠️ `filter` é obrigatório (diferente do padrão genérico)

---

### `Add(OcorrenciaViagem entity)`

**Descrição**: Adiciona ocorrência ao contexto

**Comportamento**: Não persiste no banco (precisa chamar `Save()` do UnitOfWork)

---

### `Remove(OcorrenciaViagem entity)`

**Descrição**: Remove ocorrência do contexto

**Comportamento**: Não persiste no banco (precisa chamar `Save()` do UnitOfWork)

---

### `Update(OcorrenciaViagem entity)`

**Descrição**: Atualiza ocorrência no contexto

**Nota**: Usa `new` para ocultar método (se houver)

**Comportamento**: Não persiste no banco (precisa chamar `Save()` do UnitOfWork)

---

## Diferenças em Relação ao Padrão

| Aspecto | Repository<T> | OcorrenciaViagemRepository |
|---------|---------------|----------------------------|
| Herança | Herda de `Repository<T>` | Implementa diretamente interface |
| Tracking | Suporta `AsNoTracking` | Sempre tracking |
| Get por ID | `Get(object id)` | Não implementado |
| GetAllAsync | Suporta assíncrono | Apenas síncrono |
| Projeções | `GetAllReduced()` | Não implementado |

---

## Interconexões

### Quem Usa Este Repository

- **OcorrenciaViagemController**: CRUD de ocorrências de viagens
- **ViagemController**: Para exibir ocorrências em viagens

### O Que Este Repository Usa

- **FrotiX.Data**: `FrotiXDbContext`
- **FrotiX.Models**: `OcorrenciaViagem`
- **Microsoft.EntityFrameworkCore**: `Include()`, `Where()`, `FirstOrDefault()`

---

## Exemplo de Uso

```csharp
// Buscar ocorrências com includes
var ocorrencias = unitOfWork.OcorrenciaViagem.GetAll(
    filter: o => o.ViagemId == viagemId,
    includeProperties: "Viagem,Veiculo"
);

// Buscar primeira ocorrência
var ocorrencia = unitOfWork.OcorrenciaViagem.GetFirstOrDefault(
    filter: o => o.OcorrenciaViagemId == id,
    includeProperties: "Viagem"
);

// Adicionar ocorrência
var novaOcorrencia = new OcorrenciaViagem { /* ... */ };
unitOfWork.OcorrenciaViagem.Add(novaOcorrencia);
unitOfWork.Save(); // Persiste no banco
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do OcorrenciaViagemRepository

**Arquivos Afetados**:
- `Repository/OcorrenciaViagemRepository.cs`

**Impacto**: Documentação de referência para repository customizado de ocorrências

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
- Repository/OcorrenciaViagemRepository.cs

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
