# LoginFrotiX.cshtml — Login customizado FrotiX

> **Arquivo:** `Areas/Identity/Pages/Account/LoginFrotiX.cshtml`  
> **Papel:** tela principal de login com visual customizado FrotiX/Neon.

---

## ✅ Visão Geral

Formulário com layout Neon, ícones SVG e botão estilizado. Usa antiforgery e inputs do modelo `LoginFrotiX`.

---

## 🧩 Snippets Comentados

```cshtml
@page
@model FrotiX.Areas.Identity.Pages.Account.LoginFrotiX
@{
    ViewData["Title"] = "FrotiX | Login";
    Layout = Layout = "_LoginLayout";
}
```

```cshtml
<form method="post" id="form_login" autocomplete="off">
    @Html.AntiForgeryToken()
    <input name="Ponto" asp-for="Input.Ponto" class="form-control" placeholder="Insira seu ponto" />
    <input name="Password" asp-for="Input.Password" type="password" class="form-control" placeholder="Senha" />
</form>
```

---

## ✅ Observações Técnicas

- Botão de login utiliza SVG e efeito “brushed”.
- Link de recuperação aponta para `extra-forgot-password.html` (pendente de ajuste para página real).


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
