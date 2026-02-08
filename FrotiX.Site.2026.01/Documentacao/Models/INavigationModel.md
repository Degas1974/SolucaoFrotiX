# Documentação: INavigationModel.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

A interface `INavigationModel` define o contrato para classes que fornecem navegação do sistema, especificando duas propriedades: `Seed` (navegação básica) e `Full` (navegação completa).

**Principais objetivos:**

✅ Definir contrato para classes de navegação  
✅ Padronizar acesso a navegação básica e completa  
✅ Facilitar injeção de dependência e testes

---

## 🏗️ Estrutura do Model

```csharp
public interface INavigationModel
{
    SmartNavigation Seed { get; }
    SmartNavigation Full { get; }
}
```

**Características:**
- ✅ Interface simples - Apenas duas propriedades
- ✅ Propriedades somente leitura - Apenas getters
- ✅ Retorna `SmartNavigation` - Objeto de navegação estruturado

---

## 🔗 Quem Chama e Por Quê

### NavigationModel.cs → Implementa Interface

```csharp
public class NavigationModel : INavigationModel
{
    public SmartNavigation Full => BuildNavigation(seedOnly: false);
    public SmartNavigation Seed => BuildNavigation();
}
```

### ViewComponents/NavigationViewComponent.cs → Usa Interface

```csharp
public class NavigationViewComponent : ViewComponent
{
    private readonly INavigationModel _navigationModel;
    
    public NavigationViewComponent(INavigationModel navigationModel, IUnitOfWork unitOfWork)
    {
        _navigationModel = navigationModel;
    }
    
    public IViewComponentResult Invoke()
    {
        var items = _navigationModel.Full; // ✅ Usa interface
        return View(items);
    }
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: Acoplamento Direto com NavigationModel

**Problema:** Componentes dependiam diretamente de `NavigationModel`, dificultando testes e substituição de implementação.

**Solução:** Criar interface `INavigationModel` que define o contrato, permitindo injeção de dependência e facilitando testes.

**Código:**

```csharp
// ✅ ANTES: Acoplamento direto
public class NavigationViewComponent : ViewComponent
{
    private readonly NavigationModel _navigationModel; // ❌ Classe concreta
    
    public NavigationViewComponent(NavigationModel navigationModel)
    {
        _navigationModel = navigationModel;
    }
}

// ✅ DEPOIS: Usando interface
public class NavigationViewComponent : ViewComponent
{
    private readonly INavigationModel _navigationModel; // ✅ Interface
    
    public NavigationViewComponent(INavigationModel navigationModel)
    {
        _navigationModel = navigationModel;
    }
}
```

---

## 📝 Notas Importantes

1. **Injeção de Dependência** - Interface permite registrar `NavigationModel` como `INavigationModel` no container DI.

2. **Testabilidade** - Facilita criação de mocks para testes unitários.

3. **Extensibilidade** - Permite criar outras implementações da interface no futuro.

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
