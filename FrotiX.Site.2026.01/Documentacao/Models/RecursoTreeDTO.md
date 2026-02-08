# Documentação: RecursoTreeDTO.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O DTO `RecursoTreeDTO` representa um recurso na estrutura hierárquica de TreeView Syncfusion EJ2, permitindo conversão bidirecional entre entidade `Recurso` do banco de dados e formato esperado pelo componente TreeView.

**Principais objetivos:**

✅ Converter `Recurso` (banco) para formato TreeView Syncfusion  
✅ Converter TreeView Syncfusion para `Recurso` (banco)  
✅ Suportar estrutura hierárquica recursiva com `Items`  
✅ Manter compatibilidade com componente `ejs-treeview`

---

## 📁 Arquivos Envolvidos

- **`Models/RecursoTreeDTO.cs`** - DTO principal
- **`Controllers/NavigationController.cs`** - Usa DTOs para operações de navegação
- **`ViewComponents/NavigationViewComponent.cs`** - Converte Recursos para DTOs
- **`Models/Cadastros/Recurso.cs`** - Entidade do banco

---

## 🏗️ Estrutura do Model

```csharp
public class RecursoTreeDTO
{
    public string? Id { get; set; }                    // ✅ Guid convertido para string
    public string? Text { get; set; }                  // ✅ Nome do recurso
    public string? NomeMenu { get; set; }              // ✅ Identificador único
    public string? Icon { get; set; }                  // ✅ Classe FontAwesome
    public string? IconCss { get; set; }               // ✅ CSS do ícone
    public string? Href { get; set; }                  // ✅ URL da página
    public string? ParentId { get; set; }              // ✅ ID do pai (string)
    public bool HasChild { get; set; }                 // ✅ Tem filhos?
    public bool Expanded { get; set; } = true;         // ✅ Expandido por padrão
    public double Ordem { get; set; }                   // ✅ Ordem de exibição
    public int Nivel { get; set; }                     // ✅ Nível na hierarquia
    public string? Descricao { get; set; }            // ✅ Descrição
    public bool Ativo { get; set; } = true;           // ✅ Ativo no menu
    public List<RecursoTreeDTO>? Items { get; set; }   // ✅ Filhos (recursivo)
    
    // ✅ Método estático de conversão
    public static RecursoTreeDTO FromRecurso(Recurso recurso)
    {
        return new RecursoTreeDTO
        {
            Id = recurso.RecursoId.ToString(),
            Text = recurso.Nome,
            NomeMenu = recurso.NomeMenu,
            Icon = recurso.Icon,
            IconCss = recurso.Icon,
            Href = recurso.Href,
            ParentId = recurso.ParentId?.ToString(),
            Ordem = recurso.Ordem,
            Nivel = recurso.Nivel,
            Descricao = recurso.Descricao,
            Ativo = recurso.Ativo,
            HasChild = recurso.HasChild,
            Expanded = true
        };
    }
    
    // ✅ Método de conversão reversa
    public Recurso ToRecurso()
    {
        return new Recurso
        {
            RecursoId = Guid.TryParse(Id, out var id) ? id : Guid.NewGuid(),
            Nome = Text,
            NomeMenu = NomeMenu,
            Icon = Icon,
            Href = Href,
            ParentId = Guid.TryParse(ParentId, out var parentId) ? parentId : null,
            Ordem = Ordem,
            Nivel = Nivel,
            Descricao = Descricao,
            Ativo = Ativo,
            HasChild = HasChild
        };
    }
}
```

---

## 🔗 Quem Chama e Por Quê

### NavigationController.cs → Montar Árvore Recursiva

```csharp
private List<RecursoTreeDTO> MontarArvoreRecursiva(List<Recurso> recursos, Guid? parentId)
{
    return recursos
        .Where(r => 
            (parentId == null && r.ParentId == null) || 
            (parentId != null && r.ParentId == parentId)
        )
        .OrderBy(r => r.Ordem)
        .Select(r =>
        {
            var dto = RecursoTreeDTO.FromRecurso(r); // ✅ Converte para DTO
            dto.Items = MontarArvoreRecursiva(recursos, r.RecursoId); // ✅ Recursivo
            dto.HasChild = dto.Items != null && dto.Items.Any();
            return dto;
        })
        .ToList();
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: TreeView Precisa de IDs como String

**Problema:** Syncfusion TreeView espera `Id` e `ParentId` como `string`, mas `Recurso` usa `Guid`.

**Solução:** Métodos `FromRecurso()` e `ToRecurso()` fazem conversão automática.

**Código:**

```csharp
// ✅ Conversão Guid → string
public static RecursoTreeDTO FromRecurso(Recurso recurso)
{
    return new RecursoTreeDTO
    {
        Id = recurso.RecursoId.ToString(), // ✅ Guid → string
        ParentId = recurso.ParentId?.ToString() // ✅ Guid? → string?
    };
}

// ✅ Conversão string → Guid
public Recurso ToRecurso()
{
    return new Recurso
    {
        RecursoId = Guid.TryParse(Id, out var id) ? id : Guid.NewGuid(), // ✅ string → Guid
        ParentId = Guid.TryParse(ParentId, out var parentId) ? parentId : null // ✅ string? → Guid?
    };
}
```

---

## 📝 Notas Importantes

1. **Conversão bidirecional** - Métodos `FromRecurso()` e `ToRecurso()` permitem ida e volta.

2. **Estrutura recursiva** - `Items` permite hierarquia ilimitada.

3. **Compatibilidade Syncfusion** - Formato segue exatamente o esperado pelo `ejs-treeview`.

---

**📅 Documentação criada em:** 08/01/2026


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
