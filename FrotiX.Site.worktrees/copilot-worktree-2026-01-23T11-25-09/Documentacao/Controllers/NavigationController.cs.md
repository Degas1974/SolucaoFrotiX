# NavigationController.cs — Navegação dinâmica

> **Arquivo:** `Controllers/NavigationController.cs`  
> **Papel:** leitura e atualização do `nav.json` e sincronização de recursos.

---

## ✅ Visão Geral

Controller API que retorna a árvore de navegação, permite editar itens e sincroniza com a tabela `Recurso` e controles de acesso.

---

## 🔧 Endpoints Principais

- `GetTree`: retorna treeview do `nav.json`.
- `SaveTree`: salva e sincroniza recursos.
- `AddItem` / `UpdateItem` / `DeleteItem`: CRUD de itens.

---

## 🧩 Snippet Comentado

```csharp
[Route("SaveTree")]
[HttpPost]
public IActionResult SaveTree([FromBody] List<NavigationTreeItem> items)
{
    if (System.IO.File.Exists(NavJsonPath))
        System.IO.File.Copy(NavJsonPath, NavJsonBackupPath, true);

    var navigation = new { version = "0.9", lists = TransformFromTreeData(items) };
    System.IO.File.WriteAllText(NavJsonPath, JsonSerializer.Serialize(navigation, options));

    SincronizarRecursos(items);
    return Json(new { success = true, message = "Navegação salva com sucesso!" });
}
```

---

## ✅ Observações Técnicas

- Usa cache para ícones FontAwesome.
- Mantém backups do `nav.json` antes de salvar.


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
