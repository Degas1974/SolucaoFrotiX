# Startup.cs — Pipeline e Injeção de Dependências

> **Arquivo:** `Startup.cs`  
> **Papel:** configura todos os serviços (DI) e o pipeline HTTP do FrotiX.

---

## ✅ Visão Geral

O `Startup.cs` é o centro de configuração da aplicação web. Ele registra:

- **Serviços de infraestrutura** (DbContext, Identity, Notyf, SignalR, Toast).
- **Repositórios e serviços de domínio**.
- **Filtros globais** com try/catch padronizado.
- **Middlewares críticos** (log, segurança, sessão, CORS, swagger, etc.).

---

## 🔧 Responsabilidades Principais

1. **Configurar DI** (banco, serviços, repositórios e caches).
2. **Definir cultura pt-BR** e localização padrão.
3. **Configurar segurança** (Identity + cookies + antiforgery).
4. **Montar o pipeline HTTP** com logging e handlers globais.
5. **Habilitar integrações** (Syncfusion, Telerik, SignalR, Swagger).

---

## 🧩 Snippets Comentados

### Registro de DbContexts com Pool

```csharp
services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options
        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}, poolSize: 128);
```

### Filtros Globais com Autenticação

```csharp
services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));

    // Filtros globais de exceção
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<AsyncExceptionFilter>();
});
```

### Pipeline HTTP com Logs + Segurança

```csharp
app.UseErrorLogging();
app.UseSwagger();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
```

---

## 🛡️ Tratamento de Erros

Todo o `Startup.cs` está encapsulado em `try/catch`, seguindo o padrão FrotiX:

- `Alerta.TratamentoErroComLinha` é usado no construtor e em `ConfigureServices`.
- `UseErrorLogging` é o primeiro middleware do pipeline.

---

## 🔌 Serviços e Middlewares em Destaque

- **Notyf** para toasts padrão (`AddNotyf`).
- **SignalR** para alertas e hubs.
- **Swagger** para APIs internas.
- **Syncfusion** com licença registrada.
- **AppToast** configurado via DI e TempData.

---

## ✅ Observações Técnicas

- Configura `FormOptions` para uploads grandes (100MB).
- Adiciona `ResponseCompression` (Brotli/Gzip).
- Define rotas Razor Pages (root apontando para dashboard).
- Política CORS aberta para relatórios.

---

## 🔗 Dependências Importantes

- `FrotiX.Data` (DbContexts)
- `FrotiX.Services` (camada de negócio)
- `FrotiX.Middlewares` (log e exceptions)
- `FrotiX.Repository` (UnitOfWork e repositórios)

---

## 📌 Checklist de Leitura

- ✅ **ConfigureServices**: registra tudo que a aplicação precisa.
- ✅ **Configure**: ordena o pipeline e inicializa integrações.
- ✅ **Tratamento de erros**: regras FrotiX aplicadas em todos os níveis.

---

## PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - AtualizaÃ§Ã£o: ReduÃ§Ã£o de Warnings em Debug e Logging EF Core

**DescriÃ§Ã£o**: Ajustes no pipeline de serviÃ§os para reduzir avisos do BrowserLink/BrowserRefresh e tornar o logging sensÃ­vel do EF Core configurÃ¡vel.

**Arquivos Afetados**:

- `Startup.cs`

**MudanÃ§as**:

- CompressÃ£o HTTP desabilitada em Development para evitar warnings de injeÃ§Ã£o de script.
- Logging sensÃ­vel do EF Core controlado por `EfCore:EnableSensitiveDataLogging`.

**Motivo**:

- Reduzir ruÃ­do no output de depuraÃ§Ã£o.
- Evitar avisos recorrentes de injeÃ§Ã£o em respostas comprimidas.

**Impacto**:

- Desenvolvimento mais limpo (menos warnings).
- ProduÃ§Ã£o mantÃ©m compressÃ£o HTTP.

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:

- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:

- âŒ **ANTES**: `_unitOfWork.Entity.AsTracking().Get(id)` ou `_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)`
- âœ… **AGORA**: `_unitOfWork.Entity.GetWithTracking(id)` ou `_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)`

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
