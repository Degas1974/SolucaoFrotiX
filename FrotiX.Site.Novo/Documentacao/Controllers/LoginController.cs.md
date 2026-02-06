# LoginController.cs — Utilidades de login

> **Arquivo:** `Controllers/LoginController.cs`  
> **Papel:** endpoints de login e recuperação do usuário atual.

---

## ✅ Visão Geral

Controller API simples que exibe views de login e retorna informações do usuário autenticado (nome/ponto).

---

## 🔧 Endpoints Principais

- `Index` / `Get`: views de login.
- `RecuperaUsuarioAtual`: retorna nome e ponto do usuário corrente.

---

## 🧩 Snippet Comentado

```csharp
[Route("RecuperaUsuarioAtual")]
public IActionResult RecuperaUsuarioAtual()
{
    var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == currentUserID);

    Settings.GlobalVariables.gPontoUsuario = objUsuario.Ponto;
    return Json(new { nome = objUsuario.NomeCompleto, ponto = objUsuario.Ponto });
}
```

---

## ✅ Observações Técnicas

- Usa `Settings.GlobalVariables.gPontoUsuario` para manter o ponto do usuário.
- Tratamento de erro retorna `View()` padronizado.


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
