# Documentação: ForgotAccount.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `ForgotAccount` é usado na funcionalidade de recuperação de conta, permitindo que usuários recuperem acesso usando username ou email.

**Principais objetivos:**

✅ Capturar username ou email do usuário  
✅ Validar se conta existe no sistema  
✅ Iniciar processo de recuperação de senha

---

## 🏗️ Estrutura do Model

```csharp
public class ForgotAccount
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
```

**Características:**
- ✅ Campos opcionais - Usuário pode informar username OU email
- ✅ Sem validações no Model - Validações feitas no Controller/PageModel

---

## 🔗 Quem Chama e Por Quê

### Pages/Account/ForgotPassword.cshtml.cs → Recuperação

```csharp
[BindProperty]
public ForgotAccount ForgotAccount { get; set; }

public async Task<IActionResult> OnPostAsync()
{
    var user = await _userManager.FindByNameAsync(ForgotAccount.UserName) 
            ?? await _userManager.FindByEmailAsync(ForgotAccount.Email);
    
    if (user == null)
    {
        // Usuário não encontrado
        return Page();
    }
    
    // Gera token de reset e envia email
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    // ... envia email
}
```

---

## 📝 Notas Importantes

1. **Campos opcionais** - Ambos podem ser nulos, validação verifica se pelo menos um foi preenchido.

2. **Busca flexível** - Sistema busca por username OU email.

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
