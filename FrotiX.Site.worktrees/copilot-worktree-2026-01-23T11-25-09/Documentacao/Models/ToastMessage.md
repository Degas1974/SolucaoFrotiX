# Documentação: ToastMessage.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `ToastMessage` representa uma mensagem toast (notificação temporária) com texto, cor e duração configuráveis.

**Principais objetivos:**

✅ Padronizar estrutura de mensagens toast  
✅ Suportar diferentes cores (Verde, Vermelho, Laranja)  
✅ Permitir configuração de duração de exibição

---

## 🏗️ Estrutura do Model

```csharp
public class ToastMessage
{
    public string Texto { get; set; }
    public string Cor { get; set; }
    public int Duracao { get; set; }

    public ToastMessage(string texto, string cor = "Verde", int duracao = 2000)
    {
        Texto = texto;
        Cor = cor;
        Duracao = duracao;
    }
}

public enum ToastColor
{
    Verde,
    Vermelho,
    Laranja
}
```

**Características:**
- ✅ Construtor com valores padrão - Verde, 2000ms
- ✅ Enum `ToastColor` - Cores disponíveis

---

## 🔗 Quem Chama e Por Quê

### Controllers → Mensagens de Sucesso/Erro

```csharp
TempData.Put("toast", new ToastMessage("Operação realizada com sucesso!", "Verde"));
return RedirectToPage("./Index");
```

### Pages/_Layout.cshtml → Exibição

```csharp
@{
    var toast = TempData.Get<ToastMessage>("toast");
}
@if (toast != null)
{
    <script>
        AppToast.show("@toast.Cor", "@toast.Texto", @toast.Duracao);
    </script>
}
```

---

## 📝 Notas Importantes

1. **TempData** - Usado com `TempDataExtensions.Put/Get` para persistir entre redirects.

2. **Duração em ms** - Padrão 2000ms (2 segundos).

3. **Cores** - Verde (sucesso), Vermelho (erro), Laranja (aviso).

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
