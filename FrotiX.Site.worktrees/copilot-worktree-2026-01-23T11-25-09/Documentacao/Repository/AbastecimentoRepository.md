# Documentação: AbastecimentoRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `AbastecimentoRepository` é um repository específico para a entidade `Abastecimento`, estendendo o repository genérico com métodos customizados.

**Principais características:**

✅ **Herança**: Herda de `Repository<Abastecimento>`  
✅ **Interface Específica**: Implementa `IAbastecimentoRepository`  
✅ **Métodos Customizados**: `GetAbastecimentoListForDropDown()` e `Update()` customizado

---

## Estrutura da Classe

### Herança e Implementação

```csharp
public class AbastecimentoRepository : Repository<Abastecimento>, IAbastecimentoRepository
```

**Herança**: `Repository<Abastecimento>` - Herda operações CRUD genéricas  
**Interface**: `IAbastecimentoRepository` - Define métodos específicos

---

## Construtor

```csharp
public AbastecimentoRepository(FrotiXDbContext db) : base(db)
{
    _db = db;
}
```

---

## Métodos Específicos

### `GetAbastecimentoListForDropDown()`

**Descrição**: Retorna lista de abastecimentos formatada para DropDownList

**Retorno**: `IEnumerable<SelectListItem>`

**Implementação**:
```csharp
public IEnumerable<SelectListItem> GetAbastecimentoListForDropDown()
{
    return _db.Abastecimento
        .Select(i => new SelectListItem()
        {
            Text = i.Litros.ToString(),
            Value = i.AbastecimentoId.ToString()
        });
}
```

**Características**:
- Usa `Litros` como texto (pode não ser ideal)
- Retorna `AbastecimentoId` como valor
- Não ordena resultados

**Uso**: Para dropdowns em views (uso limitado)

---

### `Update(Abastecimento abastecimento)`

**Descrição**: **MÉTODO CUSTOMIZADO** - Atualiza abastecimento com lógica específica

**Implementação**:
```csharp
public new void Update(Abastecimento abastecimento)
{
    var objFromDb = _db.Abastecimento.FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);
    
    _db.Update(abastecimento);
    _db.SaveChanges();
}
```

**Características**:
- Usa `new` para ocultar método da classe base
- Busca entidade do banco antes de atualizar
- Chama `SaveChanges()` diretamente (inconsistente com padrão)

**Nota**: ⚠️ **Inconsistência** - Chama `SaveChanges()` diretamente

---

## Interconexões

### Quem Usa Este Repository

- **AbastecimentoController**: CRUD de abastecimentos
- **Controllers de Relatórios**: Para consultas de abastecimentos

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do AbastecimentoRepository

**Arquivos Afetados**:
- `Repository/AbastecimentoRepository.cs`

**Impacto**: Documentação de referência para repository de abastecimentos

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
