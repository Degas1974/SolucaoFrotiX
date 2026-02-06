# RazorRenderService.cs

## Visão Geral
Serviço para **renderizar páginas Razor em strings** (HTML). Útil para gerar conteúdo HTML dinamicamente, como emails, relatórios ou exportações, sem precisar de contexto HTTP completo.

## Localização
`Services/RazorRenderService.cs`

## Dependências
- `Microsoft.AspNetCore.Mvc.Razor` (`IRazorViewEngine`)
- `Microsoft.AspNetCore.Mvc.ViewFeatures` (`ITempDataProvider`, `TempDataDictionary`)
- `Microsoft.AspNetCore.Mvc.Infrastructure` (`IActionContextAccessor`)
- `Microsoft.AspNetCore.Http` (`IHttpContextAccessor`)
- `Microsoft.AspNetCore.Routing` (`RouteData`)

## Interface (`IRazorRenderService`)

### `ToStringAsync<T>(string viewName, T model)`
Renderiza uma página Razor em string HTML.

**Parâmetros**:
- `viewName`: Nome/caminho da página Razor (ex.: `"Pages/Email/Template"`)
- `model`: Modelo a ser passado para a página

**Retorna**: `Task<string>` (HTML renderizado)

---

## Implementação (`RazorRenderService`)

### Construtor
Recebe múltiplas dependências do ASP.NET Core MVC:
- `IRazorViewEngine`: Engine para encontrar e compilar páginas Razor
- `ITempDataProvider`: Provider para TempData
- `IServiceProvider`: Provider de serviços para ativação
- `IHttpContextAccessor`: Acesso ao contexto HTTP
- `IRazorPageActivator`: Ativador de páginas Razor
- `IActionContextAccessor`: Acesso ao contexto de ação

---

### Método Principal

#### `ToStringAsync<T>(string pageName, T model)`
**Propósito**: Renderiza página Razor em string HTML.

**Fluxo**:
1. Cria `ActionContext` a partir do contexto HTTP atual
2. Usa `StringWriter` para capturar saída HTML
3. Busca página Razor via `_razorViewEngine.FindPage()`
4. Se página não encontrada → lança `ArgumentNullException`
5. Cria `RazorView` com a página encontrada
6. Cria `ViewContext` com:
   - `ActionContext`
   - `RazorView`
   - `ViewDataDictionary<T>` com modelo
   - `TempDataDictionary` vazio
   - `StringWriter` como saída
7. Ativa página Razor via `_activator.Activate()`
8. Executa página assíncrona via `page.ExecuteAsync()`
9. Retorna conteúdo do `StringWriter` como string

**Chamado de**: 
- Serviços de email que precisam renderizar templates HTML
- Geradores de relatórios HTML
- Exportações para HTML

**Complexidade**: Alta (envolvimento profundo com pipeline MVC)

---

### Método Auxiliar

#### `FindPage(ActionContext actionContext, string pageName)`
**Propósito**: Busca página Razor usando múltiplas estratégias.

**Estratégias**:
1. Tenta `GetPage()` com `executingFilePath: null`
2. Se falhar, tenta `FindPage()` com `ActionContext`
3. Se ainda falhar, lança `InvalidOperationException` com locais pesquisados

**Complexidade**: Média

---

## Contribuição para o Sistema FrotiX

### 📧 Templates de Email
- Renderiza templates Razor para emails HTML
- Permite usar toda sintaxe Razor (loops, condicionais, helpers)
- Mantém consistência visual com o resto da aplicação

### 📄 Relatórios HTML
- Gera relatórios HTML dinamicamente
- Permite formatação rica (tabelas, gráficos, estilos)
- Pode ser convertido para PDF posteriormente

### 🔄 Reutilização de Views
- Reutiliza páginas Razor existentes para outros propósitos
- Evita duplicação de código HTML
- Mantém consistência entre UI e emails/relatórios

## Observações Importantes

1. **Contexto HTTP Necessário**: O serviço requer contexto HTTP válido. Se usado em background jobs, pode ser necessário criar contexto HTTP sintético.

2. **Performance**: Renderização de Razor é relativamente lenta. Para alta performance, considere cachear HTML renderizado ou usar templates mais simples.

3. **Dependências Complexas**: O serviço requer muitas dependências do ASP.NET Core MVC. Garanta que todas estejam registradas no DI container.

4. **TempData Vazio**: O `TempDataDictionary` é criado vazio. Se a página Razor depender de TempData, pode não funcionar corretamente.

5. **ViewData**: O modelo é passado via `ViewDataDictionary.Model`. A página Razor deve usar `@Model` para acessar.

6. **Error Handling**: Se a página não for encontrada, uma exceção é lançada. Considere tratamento de erro mais robusto.

## Exemplo de Uso

```csharp
public class EmailService
{
    private readonly IRazorRenderService _razorRender;
    
    public async Task SendWelcomeEmailAsync(string email)
    {
        var model = new WelcomeEmailModel { Name = "João" };
        var html = await _razorRender.ToStringAsync("Pages/Email/Welcome", model);
        
        await _mailService.SendEmailAsync(new MailRequest
        {
            ToEmail = email,
            Subject = "Bem-vindo!",
            Body = html
        });
    }
}
```

## Arquivos Relacionados
- `Services/MailService.cs`: Pode usar `RazorRenderService` para renderizar templates de email
- `Pages/Email/`: Templates Razor para emails
- `Startup.cs` ou `Program.cs`: Registra `IRazorRenderService` no DI container


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
