# Documentação: NavigationModel.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 📋 Índice

1. [Objetivos](#objetivos)
2. [Arquivos Envolvidos](#arquivos-envolvidos)
3. [Estrutura do Model](#estrutura-do-model)
4. [Quem Chama e Por Quê](#quem-chama-e-por-quê)
5. [Problema → Solução → Código](#problema--solução--código)
6. [Fluxo de Funcionamento](#fluxo-de-funcionamento)
7. [Troubleshooting](#troubleshooting)

---

## 🎯 Objetivos

A classe `NavigationModel` implementa `INavigationModel` e é responsável por construir a navegação do sistema a partir do arquivo `nav.json`, aplicando controle de acesso baseado em usuário e formatando propriedades dos itens de menu.

**Principais objetivos:**

✅ Ler estrutura de navegação do arquivo `nav.json`  
✅ Aplicar controle de acesso por usuário (filtra itens baseado em `ControleAcesso`)  
✅ Formatar propriedades dos itens (Route, I18n, Type, Tags)  
✅ Suportar navegação básica (`Seed`) e completa (`Full`)  
✅ Integrar com sistema de recursos e controle de acesso

---

## 📁 Arquivos Envolvidos

### Arquivo Principal
- **`Models/NavigationModel.cs`** - Implementação principal

### Arquivos que Utilizam
- **`ViewComponents/NavigationViewComponent.cs`** - Usa `INavigationModel` para obter navegação
- **`nav.json`** - Arquivo JSON com estrutura de navegação
- **`Repository/RecursoRepository.cs`** - Busca recursos do banco
- **`Repository/ControleAcessoRepository.cs`** - Verifica permissões

---

## 🏗️ Estrutura do Model

```csharp
public class NavigationModel : INavigationModel
{
    // ✅ Constantes
    public static readonly string Void = "javascript:void(0);";
    private const string Dash = "-";
    private const string Space = " ";
    private const string Underscore = "_";
    private static readonly string Empty = string.Empty;
    
    // ✅ Dependências estáticas (injetadas no construtor)
    private static IUnitOfWork _currentUnitOfWork;
    private static IHttpContextAccessor _httpContextAccessor;
    
    // ✅ Propriedades da interface
    public SmartNavigation Full => BuildNavigation(seedOnly: false);
    public SmartNavigation Seed => BuildNavigation();
    
    // ✅ Construtor
    public NavigationModel(
        IUnitOfWork currentUnitOfWork,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _currentUnitOfWork = currentUnitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    
    // ✅ Método principal de construção
    private static SmartNavigation BuildNavigation(bool seedOnly = true)
    {
        var jsonText = File.ReadAllText("nav.json");
        var navigation = NavigationBuilder.FromJson(jsonText);
        var menu = FillProperties(navigation.Lists, seedOnly);
        return new SmartNavigation(menu);
    }
    
    // ✅ Método recursivo que preenche propriedades e aplica controle de acesso
    private static List<ListItem> FillProperties(
        IEnumerable<ListItem> items,
        bool seedOnly,
        ListItem parent = null
    )
    {
        // Lógica complexa de filtragem e formatação...
    }
}
```

---

## 🔗 Quem Chama e Por Quê

### 1. **NavigationViewComponent.cs** → Obter Navegação

**Quando:** Componente de navegação é renderizado  
**Por quê:** Exibir menu lateral com itens que o usuário tem acesso

```csharp
public class NavigationViewComponent : ViewComponent
{
    private readonly INavigationModel _navigationModel;
    
    public IViewComponentResult Invoke()
    {
        // ✅ Tenta usar banco de dados primeiro
        var arvoreDb = GetTreeFromDatabase();
        
        if (arvoreDb != null && arvoreDb.Any())
        {
            return View("TreeView", arvoreDb);
        }
        
        // ✅ Fallback: usa nav.json via NavigationModel
        var items = _navigationModel.Full;
        return View(items);
    }
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: Controle de Acesso por Usuário

**Problema:** Menu deve exibir apenas itens que o usuário tem permissão de acesso, baseado na tabela `ControleAcesso`.

**Solução:** Para cada item do `nav.json`, buscar `Recurso` pelo `NomeMenu`, verificar se usuário tem `ControleAcesso` com `Acesso = true`, e só então incluir o item na navegação.

**Código:**

```csharp
private static List<ListItem> FillProperties(...)
{
    var userId = _httpContextAccessor
        .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
        ?.Value;
    
    foreach (var item in items)
    {
        // ✅ Busca recurso pelo NomeMenu
        var objRecurso = _currentUnitOfWork.Recurso.GetFirstOrDefault(ca =>
            ca.NomeMenu == item.NomeMenu
        );
        
        if (objRecurso == null)
            continue; // ✅ Pula se recurso não existe
        
        var recursoId = objRecurso.RecursoId;
        
        // ✅ Verifica controle de acesso do usuário
        var objControleAcesso = _currentUnitOfWork.ControleAcesso.GetFirstOrDefault(
            ca => ca.UsuarioId == userId && ca.RecursoId == recursoId
        );
        
        // ✅ Só inclui se usuário tem acesso
        if (objControleAcesso != null && objControleAcesso.Acesso)
        {
            // Formata propriedades do item...
            item.Items = FillProperties(item.Items, seedOnly, item); // ✅ Recursivo
            result.Add(item);
        }
    }
    
    return result;
}
```

### Problema: Formatação de Rotas e I18n

**Problema:** Itens do `nav.json` têm `Href` simples (ex: "veiculo_index.html"), mas precisam ser convertidos para rotas ASP.NET Core (ex: "/Veiculo/Index") e chaves de internacionalização.

**Solução:** Processar `Href` removendo hífens, convertendo para formato de rota, e gerando chave I18n baseada no título.

**Código:**

```csharp
// ✅ Sanitiza Href removendo hífens
var sanitizedHref = parent == null
    ? item.Href?.Replace(Dash, Empty)
    : item.Href?.Replace(parentRoute, parentRoute.Replace(Underscore, Empty))
                 .Replace(Dash, Empty);

// ✅ Converte para rota ASP.NET Core
var route = Path.GetFileNameWithoutExtension(sanitizedHref ?? Empty)
    ?.Split(Underscore) ?? Array.Empty<string>();

item.Route = route.Length > 1
    ? $"/{route.First()}/{string.Join(Empty, route.Skip(1))}"
    : item.Href;

// ✅ Gera chave I18n
item.I18n = parent == null
    ? $"nav.{item.Title.ToLower().Replace(Space, Underscore)}"
    : $"{parent.I18n}_{item.Title.ToLower().Replace(Space, Underscore)}";
```

---

## 🔄 Fluxo de Funcionamento

### Fluxo: Construção da Navegação

```
1. NavigationViewComponent.Invoke() é chamado
   ↓
2. Tenta ler navegação do banco de dados primeiro
   ↓
3. Se não houver dados no banco, usa fallback:
   ├─ Chama _navigationModel.Full
   ├─ NavigationModel.BuildNavigation(seedOnly: false)
   ├─ Lê arquivo nav.json
   ├─ Deserializa para SmartNavigation
   └─ Chama FillProperties() recursivamente
   ↓
4. FillProperties() para cada item:
   ├─ Busca Recurso pelo NomeMenu
   ├─ Verifica ControleAcesso do usuário atual
   ├─ Se tem acesso:
   │   ├─ Formata Route, I18n, Type, Tags
   │   ├─ Processa filhos recursivamente
   │   └─ Adiciona à lista de resultado
   └─ Se não tem acesso: pula item
   ↓
5. Retorna SmartNavigation com itens filtrados
   ↓
6. ViewComponent renderiza menu lateral
```

---

## 🔍 Troubleshooting

### Erro: Menu vazio mesmo com nav.json existente

**Causa:** Usuário não tem nenhum `ControleAcesso` configurado ou todos estão com `Acesso = false`.

**Solução:**
```csharp
// ✅ Verificar se usuário tem acesso configurado
var controlesAcesso = _unitOfWork.ControleAcesso
    .GetAll(ca => ca.UsuarioId == userId)
    .ToList();
    
if (controlesAcesso.Count == 0)
{
    // Usuário não tem nenhum acesso configurado
    // Pode ser necessário dar acesso padrão ou criar registros
}
```

### Erro: Recurso não encontrado para item do menu

**Causa:** `NomeMenu` no `nav.json` não corresponde a nenhum `Recurso` no banco.

**Solução:**
```csharp
// ✅ Verificar se recurso existe
var objRecurso = _currentUnitOfWork.Recurso.GetFirstOrDefault(ca =>
    ca.NomeMenu == item.NomeMenu
);

if (objRecurso == null)
{
    // Recurso não existe - precisa criar no banco ou corrigir NomeMenu
    continue; // Pula item
}
```

---

## 📝 Notas Importantes

1. **Fallback para nav.json** - Sistema tenta usar banco de dados primeiro, mas cai para `nav.json` se não houver dados.

2. **Recursividade** - `FillProperties()` processa filhos recursivamente, mantendo hierarquia.

3. **Controle de acesso obrigatório** - Itens só aparecem se usuário tem `ControleAcesso` com `Acesso = true`.

4. **Formatação automática** - Rotas e I18n são gerados automaticamente baseados no `Href` e `Title`.

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
