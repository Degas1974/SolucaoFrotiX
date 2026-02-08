# Documentação: LoginView.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `LoginView` é usado na página de login do sistema, capturando credenciais do usuário (username e senha).

**Principais objetivos:**

✅ Capturar username e senha do usuário  
✅ Validar campos obrigatórios  
✅ Integrar com sistema de autenticação ASP.NET Identity

---

## 🏗️ Estrutura do Model

```csharp
public class LoginView
{
    [Required]
    [UIHint("username")]
    public string UserName { get; set; }
    
    [Required]
    [UIHint("password")]
    public string Password { get; set; }
}
```

**Características:**
- ✅ Validação `[Required]` - Ambos campos obrigatórios
- ✅ `[UIHint]` - Indica tipo de input para renderização

---

## 🔗 Quem Chama e Por Quê

### Pages/Account/Login.cshtml.cs → Autenticação

```csharp
[BindProperty]
public LoginView LoginView { get; set; }

public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
        return Page();
    
    var result = await _signInManager.PasswordSignInAsync(
        LoginView.UserName,
        LoginView.Password,
        isPersistent: false,
        lockoutOnFailure: true
    );
    
    if (result.Succeeded)
        return RedirectToPage("/Index");
    
    ModelState.AddModelError("", "Credenciais inválidas");
    return Page();
}
```

---

## 📝 Notas Importantes

1. **UIHint** - Ajuda renderização de inputs com tipos específicos.

2. **Validação** - `[Required]` garante que campos não sejam vazios.

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
