# Documentação: TempDataExtensions.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

A classe `TempDataExtensions` fornece métodos de extensão para `ITempDataDictionary` que permitem armazenar e recuperar objetos complexos usando serialização JSON.

**Principais objetivos:**

✅ Armazenar objetos complexos no TempData (não apenas strings)  
✅ Serializar/deserializar automaticamente usando Newtonsoft.Json  
✅ Facilitar passagem de dados entre Actions/Pages após redirects

---

## 🏗️ Estrutura do Model

```csharp
public static class TempDataExtensions
{
    public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
    {
        tempData[key] = JsonConvert.SerializeObject(value);
    }

    public static T Get<T>(this ITempDataDictionary tempData, string key)
    {
        if (tempData.TryGetValue(key, out object o))
        {
            return o == null ? default : JsonConvert.DeserializeObject<T>((string)o);
        }
        return default;
    }
}
```

**Características:**
- ✅ Métodos de extensão - `Put<T>()` e `Get<T>()`
- ✅ Serialização JSON - Usa `Newtonsoft.Json`
- ✅ Type-safe - Genéricos garantem tipo correto

---

## 🔗 Quem Chama e Por Quê

### Controllers → Armazenar Objetos Complexos

```csharp
// ✅ Armazenar ToastMessage
TempData.Put("toast", new ToastMessage("Sucesso!", "Verde"));

// ✅ Armazenar ViewModel
TempData.Put("encarregado", encarregadoViewModel);

// ✅ Recuperar em outra Action/Page
var toast = TempData.Get<ToastMessage>("toast");
var encarregado = TempData.Get<EncarregadoViewModel>("encarregado");
```

---

## 🛠️ Problema → Solução → Código

### Problema: TempData só aceita strings

**Solução:** Serializar objeto para JSON antes de armazenar, deserializar ao recuperar.

```csharp
// ✅ ANTES: Só strings
TempData["mensagem"] = "Texto simples";

// ✅ DEPOIS: Objetos complexos
TempData.Put("toast", new ToastMessage("Sucesso!", "Verde", 3000));
```

---

## 📝 Notas Importantes

1. **Serialização JSON** - Objetos são convertidos para JSON string antes de armazenar.

2. **Type-safe** - Genéricos garantem tipo correto na recuperação.

3. **Default values** - Retorna `default(T)` se chave não existir.

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
