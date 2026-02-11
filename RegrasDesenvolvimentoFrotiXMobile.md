# Regras de Desenvolvimento FrotiX Mobile – Arquivo Consolidado

> **Projeto:** FrotiX 2026 – FrotiX.Mobile
> **Tipo:** Aplicacoes .NET MAUI Blazor Hybrid – Gestao de Frotas (Mobile)
> **Stack:** .NET 10, C#, Blazor Hybrid, MAUI, Syncfusion Blazor, MudBlazor, Radzen, SweetAlert2, Azure Relay
> **Solucao:** FrotiX.Mobile (FrotiX.Economildo, FrotiX.Vistorias, FrotiX.Shared, futuro FrotiX.Patrimonio)
> **Status:** Arquivo UNICO e OFICIAL de regras dos projetos mobile
> **Versao:** 1.0
> **Ultima Atualizacao:** 10/02/2026

---

## 0. COMO ESTE ARQUIVO DEVE SER USADO (LEIA PRIMEIRO)

Este arquivo e a **UNICA FONTE DE VERDADE** para regras tecnicas, padroes, fluxo de trabalho e comportamento esperado de **desenvolvedores e agentes de IA** nos projetos mobile FrotiX.

### Regras fundamentais

- Este arquivo **substitui integralmente** qualquer outro arquivo de regras para projetos mobile
- Em caso de conflito de interpretacao: **este arquivo sempre vence**
- Nenhum codigo mobile deve ser escrito sem respeitar este documento
- O arquivo `RegrasDesenvolvimentoFrotiX.md` e o equivalente para o projeto **WEB** (FrotiX.Site) — NAO se aplica diretamente aos projetos mobile

### Estrutura de Arquivos de Regras

```
FrotiX.Mobile/                           ← Solucao mobile
├── RegrasDesenvolvimentoFrotiXMobile.md  ← ESTE ARQUIVO (fonte unica mobile)
├── FrotiX.Mobile.Shared/                ← Biblioteca compartilhada
├── FrotiX.Mobile.Economildo/            ← App: Economildo (contagem passageiros)
├── FrotiX.Mobile.Vistorias/             ← App: Vistorias (inspecao veicular)
└── FrotiX.Mobile.Patrimonio/            ← App: Patrimonio (futuro)
```

### Relacao com o arquivo Web

| Aspecto | Web (`RegrasDesenvolvimentoFrotiX.md`) | Mobile (ESTE ARQUIVO) |
|---------|---------------------------------------|----------------------|
| **Framework** | ASP.NET Core MVC + Razor Pages | .NET MAUI Blazor Hybrid |
| **UI Components** | Kendo UI + Syncfusion EJ2 + jQuery | Syncfusion Blazor + MudBlazor + Radzen |
| **Renderizacao** | Server-side HTML + JS | Blazor WebView (client-side) |
| **Comunicacao API** | Controllers locais | Azure Relay (RelayApiService) |
| **Armazenamento** | SQL Server direto (EF Core) | SecureStorage local + API remota |
| **Estado** | Session, TempData, ViewData | Component-local, DI services |
| **Loading** | FtxSpin (overlay global) | Blazor conditional rendering |
| **Alertas** | SweetAlert via JS global | SweetAlert via JSRuntime interop |
| **Icones** | FontAwesome Duotone | FontAwesome Duotone (mesmo) |

---

## 1. ARQUITETURA DA SOLUCAO MOBILE

### 1.1 Visao Geral

A solucao `FrotiX.Mobile` segue o padrao **.NET MAUI Blazor Hybrid**, onde:

- **MAUI** fornece o container nativo (Android/iOS)
- **Blazor** renderiza a UI via WebView
- **Razor Components** (.razor) substituem as Razor Pages (.cshtml) do web
- **Servicos C#** substituem Controllers e jQuery/AJAX do web

### 1.2 Projetos da Solucao

| Projeto | Tipo | Proposito | Target |
|---------|------|-----------|--------|
| `FrotiX.Mobile.Shared` | Razor Class Library | Models, Services, Helpers compartilhados | net10.0-android |
| `FrotiX.Mobile.Economildo` | MAUI App | Contagem de passageiros (Economildo) | net10.0-android |
| `FrotiX.Mobile.Vistorias` | MAUI App | Vistoria/inspecao de veiculos | net10.0-android |
| `FrotiX.Mobile.Patrimonio` | MAUI App (futuro) | Gestao de patrimonio | net10.0-android |

### 1.3 Grafo de Dependencias

```
FrotiX.Mobile.Economildo  ──→  FrotiX.Mobile.Shared
FrotiX.Mobile.Vistorias   ──→  FrotiX.Mobile.Shared
FrotiX.Mobile.Patrimonio  ──→  FrotiX.Mobile.Shared  (futuro)
```

**Regra:** Apps individuais NUNCA referenciam outros apps. Toda logica compartilhada vai em `FrotiX.Mobile.Shared`.

### 1.4 Diretorio Padrao de Trabalho

**SEMPRE trabalhe no diretorio:** `FrotiX.Mobile/`

- O projeto **Shared** e a biblioteca comum — priorizar para models, services e helpers
- Os projetos **Economildo** e **Vistorias** contem apenas UI e configuracao de DI
- Ao criar novo app (ex: Patrimonio), seguir a mesma estrutura dos existentes

---

## 2. REGRAS INVIOLAVEIS (ZERO TOLERANCE)

### 2.1 TRY-CATCH (OBRIGATORIO)

**REGRA IDENTICA AO WEB:** Toda funcao (C# Blazor) DEVE ter try-catch.

#### Padrao para Componentes Blazor (.razor)

```csharp
protected override async Task OnInitializedAsync()
{
    try
    {
        await CarregarDadosAsync();
    }
    catch (Exception ex)
    {
        Logger?.Error("Erro ao carregar dados", ex);
        await Alerta.TratamentoErroComLinha(ex);
    }
}
```

#### Padrao para Event Handlers

```csharp
private async Task OnSalvarClick()
{
    try
    {
        var resultado = await Service.SalvarAsync(model);
        if (resultado)
        {
            await Alerta.Sucesso("Sucesso", "Dados salvos com sucesso");
            Navigation.NavigateTo("/home");
        }
    }
    catch (Exception ex)
    {
        Logger?.Error("Erro ao salvar", ex);
        await Alerta.Erro("Erro", $"Falha ao salvar: {ex.Message}");
    }
}
```

#### Padrao para Services

```csharp
public async Task<List<VeiculoViewModel>> ObterVeiculosAsync()
{
    try
    {
        var resultado = await _relayApi.GetAsync<List<VeiculoViewModel>>("/api/Veiculo/GetAll");
        return resultado ?? new List<VeiculoViewModel>();
    }
    catch (Exception ex)
    {
        _logger?.Error("Erro ao obter veiculos", ex);
        return new List<VeiculoViewModel>();
    }
}
```

**Diferenca do Web:** No mobile NAO usamos `Alerta.TratamentoErroComLinha("arquivo.cs", "metodo", error)` com strings — usamos o overload com `[CallerMemberName]` e `[CallerLineNumber]` automaticos.

### 2.2 ALERTAS E UX (SweetAlert via JS Interop)

#### PROIBIDO

- `alert()`, `confirm()`, `prompt()` (JavaScript nativo)
- `Application.Current.MainPage.DisplayAlert()` (MAUI nativo)
- `Console.WriteLine()` para feedback ao usuario

#### OBRIGATORIO — AlertaJs (servico injetado)

```csharp
@inject AlertaJs Alerta

// Uso:
await Alerta.Sucesso("Titulo", "Mensagem");
await Alerta.Erro("Titulo", "Mensagem");
await Alerta.Info("Titulo", "Mensagem");
await Alerta.Alerta("Titulo", "Mensagem");

// Confirmacao
bool confirmado = await Alerta.Confirmar("Titulo", "Mensagem", "Sim", "Cancelar");
if (confirmado) { /* acao */ }

// Erro nao tratado (com stack trace automatico)
await Alerta.TratamentoErroComLinha(ex);
```

**Implementacao:** `AlertaJs` → `IJSRuntime.InvokeVoidAsync("SweetAlertInterop.Show*")` → `sweetalert_interop_061.js` → SweetAlert2

### 2.3 ICONES (FontAwesome DUOTONE)

**REGRA IDENTICA AO WEB:**

#### SEMPRE

```html
<i class="fa-duotone fa-clipboard-check"
   style="--fa-primary-color:#667eea; --fa-secondary-color:#764ba2;"></i>
```

#### NUNCA

- `fa-solid`, `fa-regular`, `fa-light`, `fa-thin`, `fa-brands`

**Paleta de Cores Mobile FrotiX:**

| Contexto | Primary | Secondary | Uso |
|----------|---------|-----------|-----|
| Dashboard/Home | `#667eea` | `#764ba2` | Cards principais, headers |
| Sucesso/Positivo | `#10b981` | `#059669` | Botoes confirmar, status OK |
| Erro/Negativo | `#ef4444` | `#dc2626` | Alertas, exclusao |
| Neutro/Info | `#2c5282` | `#1a365d` | Cards de informacao, sync |
| Alerta/Warning | `#f97316` | `#ea580c` | Avisos, pendencias |

### 2.4 LOADING / CARREGAMENTO

**DIFERENTE DO WEB** — No mobile NAO usamos `FtxSpin`. Usamos renderizacao condicional Blazor.

#### Padrao Obrigatorio

```razor
@if (carregando)
{
    <div class="loading-container">
        <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Carregando...</span>
        </div>
        <p class="loading-text">Carregando dados...</p>
    </div>
}
else if (dados.Count == 0)
{
    <div class="empty-state">
        <i class="fa-duotone fa-folder-open"></i>
        <h3>Nenhum registro encontrado</h3>
    </div>
}
else
{
    <!-- Conteudo principal -->
}
```

#### Padrao para operacoes assincronas

```csharp
private bool carregando = true;

protected override async Task OnInitializedAsync()
{
    try
    {
        carregando = true;
        await CarregarDadosAsync();
    }
    catch (Exception ex)
    {
        Logger?.Error("Erro", ex);
    }
    finally
    {
        carregando = false;
        StateHasChanged();
    }
}
```

#### PROIBIDO

- `FtxSpin.show()` / `FtxSpin.hide()` (nao existe no mobile)
- Loading com overlay full-screen (exceto operacoes criticas de longa duracao)

---

## 3. PADROES VISUAIS MOBILE

### 3.1 Estrategia de Bibliotecas UI

**REGRA CRITICA:** Cada app mobile pode usar um MIX de bibliotecas UI, mas com proposito definido:

| Biblioteca | Versao | Uso Principal | Apps |
|------------|--------|---------------|------|
| **Syncfusion Blazor** | 31.2.15 | DropDowns, DatePicker, TimePicker, NumericTextBox, Signature, CheckBox | Ambos |
| **MudBlazor** | 8.15.0 | DatePicker, Autocomplete, Select, Theme | Economildo |
| **Radzen Blazor** | 8.3.5 | Componentes leves de UI | Vistorias |
| **Bootstrap** | 5.x | Grid, spacing, display utilities (apenas classes utilitarias) | Ambos |
| **SweetAlert2** | Local | Dialogs, modals, confirmacoes, toasts | Ambos |
| **FontAwesome Pro** | 6.x | Icones (somente duotone) | Ambos |

**Regra de Escolha para Novos Apps:**

1. **Dropdowns com filtragem:** Syncfusion `SfComboBox` ou `SfDropDownList`
2. **Data/Hora:** Syncfusion `SfDatePicker` / `SfTimePicker` OU MudBlazor `MudDatePicker`
3. **Inputs numericos:** Syncfusion `SfNumericTextBox`
4. **Assinatura digital:** Syncfusion `SfSignature`
5. **Autocomplete com busca:** MudBlazor `MudAutocomplete`
6. **Alertas/Confirmacoes:** SweetAlert2 via `AlertaJs`
7. **Layout/Grid:** CSS Grid + Flexbox (NAO componentes de layout de bibliotecas)

### 3.2 CSS — Abordagem Mobile

#### CSS Global

- **Arquivo:** `wwwroot/css/FrotiXBlazorMaui.css` (presente em cada app)
- **Variaveis CSS (Light/Dark mode):**

```css
:root {
    --bg: #f7f7f9;
    --fg: #1f2328;
    --brand: #374151;
    --brand-contrast: #fff;
}

@media (prefers-color-scheme: dark) {
    :root {
        --bg: #0f172a;
        --fg: #e5e7eb;
    }
}
```

#### CSS Scoped (Componentes)

- **Cada componente .razor** pode ter CSS inline na secao `<style>` no topo do arquivo
- CSS inline e ACEITAVEL no mobile (diferente do web) porque:
  - Componentes Blazor sao auto-contidos
  - Nao ha problema de cache (WebView local)
  - Facilita manutencao do componente como unidade

#### Tecnicas de Layout

```css
/* Grid para formularios */
.form-row {
    display: grid;
    grid-template-columns: 1fr 1fr 2fr;
    gap: 12px;
}

/* Cards com gradiente */
.home-card {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border-radius: 16px;
    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
    transition: all 0.3s ease;
}

/* Footer fixo */
.footer-fixed {
    position: fixed;
    bottom: 50px;
    left: 0; right: 0;
    padding: 16px;
    backdrop-filter: blur(8px);
    z-index: 1000;
}
```

#### Breakpoints Mobile

```css
/* Tablet landscape (padrao) */
/* Tablet portrait */
@media (max-width: 768px) { ... }
/* Phone */
@media (max-width: 600px) { ... }
```

### 3.3 Tooltips

**NAO APLICAVEL AO MOBILE.** Interface touch nao usa tooltips. Substituir por:
- Texto descritivo visivel no card/botao
- Icones auto-explicativos
- Labels nos formularios

---

## 4. PADROES DE CODIGO

### 4.1 Estrutura de Componentes Blazor (.razor)

**Ordem padrao dentro de um componente:**

```razor
@* 1. DIRETIVAS *@
@page "/minharota"
@inject NavigationManager Navigation
@inject ILogService Logger
@inject AlertaJs Alerta
@inject IMeuService Service

@* 2. HTML/MARKUP *@
<div class="page-container">
    @if (carregando)
    {
        <div class="loading">Carregando...</div>
    }
    else
    {
        <!-- Conteudo principal -->
    }
</div>

@* 3. CSS SCOPED (se necessario) *@
<style>
    .page-container { /* estilos */ }
</style>

@* 4. BLOCO DE CODIGO *@
@code {
    // 4a. Campos privados
    private bool carregando = true;
    private List<MeuModel> dados = new();

    // 4b. Lifecycle methods
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await CarregarDadosAsync();
        }
        catch (Exception ex)
        {
            Logger?.Error("Erro ao inicializar", ex);
        }
        finally
        {
            carregando = false;
        }
    }

    // 4c. Event handlers
    private async Task OnSalvar() { ... }
    private void OnCancelar() => Navigation.NavigateTo("/home");

    // 4d. Metodos auxiliares
    private async Task CarregarDadosAsync() { ... }

    // 4e. Classes internas (se necessario)
    private class RegistroLocal { ... }
}
```

### 4.2 Injecao de Dependencia (DI)

**Regra de Registro em MauiProgram.cs:**

| Lifetime | Quando Usar | Exemplo |
|----------|-------------|---------|
| **Singleton** | Services sem estado ou com estado global | `RelayApiService`, `IVistoriadorService` |
| **Scoped** | Services com estado por "escopo" (pagina) | `IVeiculoService`, `IMotoristaService`, `IAlertaService` |
| **Transient** | NAO USAR (exceto casos muito especificos) | — |

```csharp
// MauiProgram.cs
// API & Infraestrutura
builder.Services.AddSingleton<RelayApiService>();

// UI Services
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<AlertaJs>();

// Business Services
builder.Services.AddScoped<IVeiculoService, VeiculoService>();
builder.Services.AddScoped<IMotoristaService, MotoristaService>();
```

### 4.3 Navegacao

**Sistema:** Blazor Router com `NavigationManager`

```csharp
// Navegacao simples
Navigation.NavigateTo("/destino");

// Navegacao com parametro
Navigation.NavigateTo($"/editarvistoria/{viagemId}");

// Navegacao com force reload
Navigation.NavigateTo("/", forceLoad: true);
```

**Rotas:** Definidas com `@page` em cada componente:

```razor
@page "/home"
@page "/fichaviagem"
@page "/syncdados"
@page "/editarvistoria/{Id:guid}"
```

**Multi-Assembly Routing:** Configurado em `Routes.razor` para carregar paginas do Shared:

```razor
<Router AppAssembly="typeof(Routes).Assembly"
        AdditionalAssemblies="new[] { typeof(FrotiX.Mobile.Shared._Imports).Assembly }">
```

### 4.4 Comunicacao com API (RelayApiService)

**REGRA CRITICA:** Toda comunicacao com o backend e feita via **Azure Relay** usando `RelayApiService`.

#### Como funciona

```
App Mobile → RelayApiService → Azure Relay → Backend FrotiX.Site → SQL Server
```

#### Padrao de uso em Services

```csharp
public class VeiculoService : IVeiculoService
{
    private readonly RelayApiService _api;

    public VeiculoService(RelayApiService api)
    {
        _api = api;
    }

    public async Task<List<VeiculoViewModel>?> ObterVeiculoCompletoDropdownAsync()
    {
        try
        {
            return await _api.GetAsync<List<VeiculoViewModel>>("/api/Veiculo/GetVeiculoCompletoDropdown");
        }
        catch (Exception ex)
        {
            // Log e retornar null/default
            return null;
        }
    }
}
```

#### Metodos Disponiveis

```csharp
// GET com deserializacao generica
Task<T?> GetAsync<T>(string endpoint)

// POST com response tipada
Task<T?> PostAsync<T>(string endpoint, object data)

// POST com retorno bool
Task<bool> PostAsync(string endpoint, object data)

// PUT com response tipada
Task<T?> PutAsync<T>(string endpoint, object data)

// PUT com retorno bool
Task<bool> PutAsync(string endpoint, object data)

// DELETE
Task<bool> DeleteAsync(string endpoint)
```

#### PROIBIDO

- `HttpClient` direto (sempre usar `RelayApiService`)
- URLs hardcoded no codigo (usar constantes)
- Chamadas sem try-catch

### 4.5 Camada de Services (Equivalente a Controllers no Web)

**REGRA CRITICA:** No projeto web, a logica de negocio vive nos **Controllers**. No mobile, essa mesma responsabilidade esta nos **Services** do projeto `FrotiX.Mobile.Shared`.

#### Equivalencia Web → Mobile

| Web (FrotiX.Site) | Mobile (FrotiX.Mobile.Shared) |
|-------------------|-------------------------------|
| `Controllers/VeiculoController.cs` | `Services/VeiculoService.cs` |
| `Controllers/Api/WhatsAppController.cs` | `Services/WhatsAppService.cs` (se existisse) |
| `Repository/VeiculoRepository.cs` | `Data/Repository/ViagensEconomildoRepository.cs` |
| `Repository/IRepository/IVeiculoRepository.cs` | `Data/Repository/IRepository/IViagensEconomildoRepository.cs` |
| `IUnitOfWork` (acesso a todos os repos) | `IUnitOfWork` (mesma interface, menos repos) |
| Retorno `Json(new { success, message, data })` | Retorno tipado `Task<T?>` ou `Task<(bool, string)>` |

#### Estrutura Obrigatoria de Services

**Toda feature mobile DEVE ter:**

```
FrotiX.Mobile.Shared/
├── Services/
│   ├── IServices/
│   │   └── IMeuService.cs          ← Interface (contrato)
│   └── MeuService.cs               ← Implementacao
```

#### Interface (Contrato) — Padrao Obrigatorio

```csharp
// Services/IServices/IMeuService.cs
namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IMeuService
    {
        // Leitura (GET)
        Task<List<MeuViewModel>> ObterTodosAsync();
        Task<MeuModel?> ObterPorIdAsync(Guid id);

        // Escrita (POST/PUT)
        Task<bool> SalvarAsync(MeuModel model);
        Task<bool> AtualizarAsync(MeuModel model);

        // Exclusao (DELETE)
        Task<bool> ExcluirAsync(Guid id);

        // Dropdowns
        Task<List<SelectListItemModel>> ObterDropdownAsync();
    }
}
```

#### Implementacao — Padrao Obrigatorio

```csharp
// Services/MeuService.cs
namespace FrotiX.Mobile.Shared.Services
{
    public class MeuService : IMeuService
    {
        private readonly RelayApiService _api;
        private readonly IAlertaService _alerta;
        private readonly ILogService _logger;

        public MeuService(RelayApiService api, IAlertaService alerta, ILogService logger)
        {
            _api = api;
            _alerta = alerta;
            _logger = logger;
        }

        public async Task<List<MeuViewModel>> ObterTodosAsync()
        {
            try
            {
                var resultado = await _api.GetAsync<List<MeuViewModel>>("/api/Meu/GetAll");
                return resultado ?? new List<MeuViewModel>();
            }
            catch (Exception ex)
            {
                _logger?.Error("Erro ao obter dados", ex);
                return new List<MeuViewModel>();
            }
        }

        public async Task<bool> SalvarAsync(MeuModel model)
        {
            try
            {
                return await _api.PostAsync("/api/Meu/Create", model);
            }
            catch (Exception ex)
            {
                _logger?.Error("Erro ao salvar", ex);
                return false;
            }
        }
    }
}
```

#### Padroes de Retorno

| Operacao | Tipo de Retorno | Quando Usar |
|----------|----------------|-------------|
| **Leitura de lista** | `Task<List<T>>` | Retornar `new()` em caso de erro (NUNCA null) |
| **Leitura de item** | `Task<T?>` | Retornar `null` se nao encontrar ou erro |
| **Escrita/Exclusao** | `Task<bool>` | `true` = sucesso, `false` = falha |
| **Operacao com mensagem** | `Task<(bool sucesso, string mensagem)>` | Quando o chamador precisa exibir mensagem |
| **Operacao com dados** | `Task<OperationResult<T>>` | Quando precisa retornar sucesso + dados + mensagem |

#### Tuple Pattern (sucesso, mensagem)

```csharp
public async Task<(bool sucesso, string mensagem)> SincronizarVeiculosAsync()
{
    try
    {
        var veiculos = await _api.GetAsync<List<VeiculoViewDto>>(
            "/api/Veiculo/GetVeiculoCompletoDropdown");

        if (veiculos == null || veiculos.Count == 0)
            return (false, "Nenhum veiculo encontrado no servidor");

        await SecureStorage.SetAsync(KEY_VEICULOS, JsonSerializer.Serialize(veiculos));
        return (true, $"{veiculos.Count} veiculo(s) sincronizado(s)");
    }
    catch (Exception ex)
    {
        _logger?.Error("Erro ao sincronizar veiculos", ex);
        return (false, $"Erro: {ex.Message}");
    }
}
```

#### OperationResult Pattern (alternativa)

```csharp
// Shared/OperationResult.cs (ja existe no projeto)
public class OperationResult<T>
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public T? Dados { get; set; }

    public static OperationResult<T> Success(T dados, string msg = "")
        => new() { Sucesso = true, Dados = dados, Mensagem = msg };

    public static OperationResult<T> Failure(string msg)
        => new() { Sucesso = false, Mensagem = msg };
}
```

#### Regras de Error Handling em Services

1. **NUNCA** deixar excecao propagar sem catch no Service
2. **SEMPRE** logar o erro com `_logger?.Error()`
3. **Listas:** retornar `new List<T>()` em caso de erro (nunca null)
4. **Items:** retornar `null` em caso de erro (o componente trata)
5. **Operacoes:** retornar `false` ou `(false, mensagem)` em caso de erro
6. **NAO** exibir alerta no Service — o componente Blazor decide se/como exibir

#### Services Existentes no Projeto

| Service | Interface | Responsabilidade | Usado Por |
|---------|-----------|-----------------|-----------|
| `VeiculoService` | `IVeiculoService` | Buscar veiculos via API | Economildo, Vistorias |
| `MotoristaService` | `IMotoristaService` | Buscar motoristas via API | Economildo, Vistorias |
| `ViagemService` | `IViagemService` | CRUD de vistorias/viagens | Vistorias |
| `ViagensEconomildoService` | `IViagensEconomildoService` | CRUD viagens Economildo + local storage | Economildo |
| `SyncService` | `ISyncService` | Sincronizacao offline/online | Economildo |
| `OcorrenciaService` | `IOcorrenciaService` | Gestao de ocorrencias de veiculos | Vistorias |
| `VistoriadorService` | `IVistoriadorService` | Lista de vistoriadores | Vistorias |
| `AlertaService` | `IAlertaService` | Alertas SweetAlert via JS Interop | Ambos |
| `ToastService` | `IToastService` | Notificacoes toast | Ambos |
| `LogService` | `ILogService` | Logging em arquivo | Ambos |
| `AlertaJs` | — (classe direta) | Wrapper JS para SweetAlert2 | Ambos |
| `RelayApiService` | — (classe direta) | HTTP client via Azure Relay | Todos os Services |

#### Repositorio (Quando Usar)

O projeto Shared tambem tem o padrao **Repository + UnitOfWork** (em `Data/Repository/`), mas seu uso e limitado:

```csharp
// IUnitOfWork.cs
public interface IUnitOfWork : IDisposable
{
    IViagensEconomildoRepository ViagensEconomildo { get; }
    IRepository<OcorrenciaViagem> OcorrenciasViagem { get; }
    void Save();
    Task SaveAsync();
}
```

**Quando usar Repository vs Service:**

| Cenario | Usar |
|---------|------|
| Dados que vem/vao para a API remota | **Service** (via RelayApiService) |
| Dados locais persistidos em SQLite/EF Core | **Repository** (via IUnitOfWork) |
| Dados temporarios em SecureStorage (JSON) | **Service** (direto no SecureStorage) |

**Regra:** A maioria dos dados no mobile vem da API. Repositorios so sao usados quando ha banco local (EF Core com SQLite) — o que hoje e raro no projeto.

#### Checklist para Criar Novo Service

1. Criar interface em `Shared/Services/IServices/IMeuService.cs`
2. Criar implementacao em `Shared/Services/MeuService.cs`
3. Injetar `RelayApiService` + `ILogService` no construtor
4. Implementar try-catch em TODOS os metodos
5. Retornar defaults seguros (listas vazias, null, false) em caso de erro
6. Registrar no `MauiProgram.cs` do app: `builder.Services.AddScoped<IMeuService, MeuService>();`
7. Injetar no componente Blazor: `@inject IMeuService Service`
8. Documentar com Card de Arquivo e Card de Funcao

### 4.6 Armazenamento Local (SecureStorage)

**Para dados offline, usar MAUI SecureStorage:**

```csharp
// Salvar
await SecureStorage.SetAsync("chave", JsonSerializer.Serialize(dados));

// Ler
var json = await SecureStorage.GetAsync("chave");
var dados = json != null ? JsonSerializer.Deserialize<List<T>>(json) : new();

// Remover
SecureStorage.Remove("chave");
```

**Convencao de chaves:**

```csharp
private const string KEY_VEICULOS = "veiculos_economildo";
private const string KEY_MOTORISTAS = "motoristas_economildo";
private const string KEY_DATA_SYNC = "data_ultima_sync";
private const string KEY_VIAGENS = "viagens_economildo_{date}";
```

**Plataforma condicional:**

```csharp
#if ANDROID || IOS || MACCATALYST
    await Microsoft.Maui.Storage.SecureStorage.SetAsync(key, value);
#else
    await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(key, value);
#endif
```

### 4.7 Gerenciamento de Estado

**Padrao:** Estado e LOCAL ao componente. NAO usar Redux/Flux/StateContainer global.

```csharp
// Estado local em campos privados
private bool carregando = true;
private List<VeiculoViewModel> veiculos = new();
private Guid? veiculoSelecionadoId;

// Propriedades computadas
private bool TemVeiculoSelecionado => veiculoSelecionadoId.HasValue && veiculoSelecionadoId != Guid.Empty;

// Two-way binding
<SfComboBox @bind-Value="veiculoSelecionadoId" ... />

// Notificar mudancas de UI
StateHasChanged();

// Passagem para componentes filhos via CascadingValue/Parameter
<CascadingValue Value="@veiculoAtual">
    <ComponenteFilho />
</CascadingValue>
```

### 4.8 Gerenciamento de Memoria (CRITICO para Mobile)

**Regra:** Antes de navegar para outra pagina, LIMPAR listas grandes e forcar GC:

```csharp
private void LimparMemoria()
{
    listaGrande?.Clear();
    listaGrande = null;
    outroObjeto = null;

    GC.Collect();
    GC.WaitForPendingFinalizers();
}

private void Voltar()
{
    LimparMemoria();
    Navigation.NavigateTo("/home");
}
```

**Quando aplicar:**
- Paginas com listas grandes (>100 itens)
- Paginas com imagens em base64
- Paginas com multiplos componentes Syncfusion
- Antes de qualquer `Navigation.NavigateTo()`

---

## 5. PLATAFORMA E CONFIGURACAO

### 5.1 Configuracao do Projeto (.csproj)

**Template padrao para novos apps:**

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
    <PropertyGroup>
        <TargetFrameworks>net10.0-android</TargetFrameworks>
        <OutputType>Exe</OutputType>
        <ApplicationTitle>FrotiX [NomeApp]</ApplicationTitle>
        <ApplicationId>com.camara.frotix.[nomeapp]</ApplicationId>
        <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
        <ApplicationVersion>1</ApplicationVersion>

        <!-- Compatibilidade com dispositivos antigos -->
        <SupportedOSPlatformVersion>21.0</SupportedOSPlatformVersion>
        <RuntimeIdentifiers>android-arm;android-arm64</RuntimeIdentifiers>
        <EmbedAssembliesIntoApk>true</EmbedAssembliesIntoApk>
        <UseInterpreter>true</UseInterpreter>
        <RunAOTCompilation>false</RunAOTCompilation>
        <PublishTrimmed>false</PublishTrimmed>
    </PropertyGroup>
</Project>
```

**PROIBIDO alterar sem autorizacao:**
- `RunAOTCompilation` → Manter `false` (causa problemas com Azure Relay)
- `PublishTrimmed` → Manter `false` (remove assemblies necessarios)
- `UseInterpreter` → Manter `true` (compatibilidade com tablets SM-P585M)

### 5.2 Android Especifico

#### MainActivity.cs

```csharp
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ...)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
#if DEBUG
        Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
    }
}
```

#### Permissoes

```csharp
// Antes de usar camera/galeria
await Permissions.RequestAsync<Permissions.Camera>();
#if ANDROID
    await Permissions.RequestAsync<Permissions.StorageRead>();
#endif
```

#### Captura de Fotos com Downsampling

```csharp
// OBRIGATORIO: Redimensionar imagens antes de converter para base64
private static async Task<string> AndroidDownsampleToBase64Async(
    FileResult file, int maxDim = 1280, int quality = 80)
{
    // Usar Android.Graphics.BitmapFactory com InSampleSize
    // Retornar base64 JPEG reduzido
}
```

**Regra:** NUNCA enviar fotos em resolucao original. Sempre downsampling para max 1280px.

### 5.3 Licencas Syncfusion

**Regra:** Licenca Syncfusion deve ser registrada em `MauiProgram.cs`:

```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8...");
```

**NUNCA** commitar tokens de licenca em arquivos publicos. O token atual esta valido ate 2075.

### 5.4 Azure Relay (Configuracao)

```csharp
// RelayApiService.cs (em Shared)
private const string RelayNamespace = "frotix-relay.servicebus.windows.net";
private const string HybridConnectionName = "frotix-bridge";
private const string SasToken = "SharedAccessSignature sr=..."; // Valido ate 2075
private const int TimeoutSeconds = 60;
```

**Regra:** O SAS Token esta centralizado em `RelayApiService.cs` no projeto Shared. NAO duplicar em outros arquivos.

---

## 6. LOGGING E DIAGNOSTICO

### 6.1 Sistema de Logging

**Dois niveis de logging:**

#### 1. MauiProgram (Global - File-based)

```csharp
// Escrita (nao-bloqueante)
MauiProgram.LogInfo("Mensagem informativa");
MauiProgram.LogError("Mensagem de erro", exception);

// Leitura
string logs = MauiProgram.ReadAllLogs();

// Limpeza
MauiProgram.ClearLogs();
```

- **Arquivo:** `FileSystem.AppDataDirectory/app_logs.txt`
- **Tamanho maximo:** 5MB (rotacao automatica para `app_logs_backup.txt`)
- **Formato:** `[INFO/ERROR HH:mm:ss.fff] mensagem`

#### 2. ILogService (Injetavel)

```csharp
@inject ILogService Logger

Logger?.Info("Dados carregados com sucesso");
Logger?.Warning("Dados expirados, necessita sync");
Logger?.Error("Falha ao conectar", ex);
```

### 6.2 Global Exception Handlers

**Configurados automaticamente em MauiProgram.cs:**

```csharp
AppDomain.CurrentDomain.UnhandledException += (s, e) => LogError(...);
TaskScheduler.UnobservedTaskException += (s, e) => LogError(...);
```

### 6.3 Error Boundary (Blazor)

**Componente `SweetAlertErrorBoundary`** envolve toda a aplicacao no `MainLayout.razor`:

```razor
<SweetAlertErrorBoundary>
    <div class="page">
        @Body
    </div>
</SweetAlertErrorBoundary>
```

Se um componente lanca excecao nao tratada, o ErrorBoundary captura e exibe SweetAlert em vez de crashar o app.

### 6.4 Pagina de Logs

**Rota:** `/logs-viewer`

Disponivel em todos os apps para diagnostico local. Mostra logs em tempo real com filtro de erros.

---

## 7. FLUXO DE TRABALHO

### 7.1 Git

**Regras IDENTICAS ao Web:**

- **Branch preferencial:** `main`
- **Push SEMPRE para:** `main`
- **Commit automatico** apos criacao/alteracao de arquivos
- Commit apenas dos arquivos da sessao atual
- **Correcao de erro proprio:** explicar erro + correcao no commit

**Tipos de commit:**

- `feat:` - Nova funcionalidade
- `fix:` - Correcao de bug
- `refactor:` - Refatoracao
- `docs:` - Documentacao
- `style:` - Formatacao/CSS
- `chore:` - Manutencao

### 7.2 Onde Colocar Codigo Novo

| Tipo de Codigo | Destino |
|----------------|---------|
| **Model/DTO** compartilhado | `FrotiX.Mobile.Shared/Models/` |
| **Service** compartilhado | `FrotiX.Mobile.Shared/Services/` |
| **Interface** de service | `FrotiX.Mobile.Shared/Services/IServices/` |
| **Helper** compartilhado | `FrotiX.Mobile.Shared/Helpers/` |
| **Validacao** compartilhada | `FrotiX.Mobile.Shared/Validations/` |
| **Pagina** especifica do app | `[App]/Components/Pages/` |
| **Componente** reutilizavel | `FrotiX.Mobile.Shared/Components/` (criar se necessario) |
| **CSS** global | `[App]/wwwroot/css/FrotiXBlazorMaui.css` |
| **JS** interop | `[App]/wwwroot/js/` |

### 7.3 Checklist para Novo App (ex: FrotiX.Patrimonio)

1. Criar projeto MAUI Blazor: `dotnet new maui-blazor -n FrotiX.Mobile.Patrimonio`
2. Adicionar referencia ao Shared: `<ProjectReference Include="..\FrotiX.Mobile.Shared\..." />`
3. Copiar estrutura de pastas de um app existente (Vistorias e mais limpo)
4. Configurar `MauiProgram.cs` com DI dos services necessarios
5. Configurar `App.razor` + `Routes.razor` com multi-assembly routing
6. Copiar `wwwroot/` (CSS, JS, fonts) de um app existente
7. Registrar licenca Syncfusion em MauiProgram.cs
8. Configurar `MainLayout.razor` com providers e ErrorBoundary
9. Criar `Home.razor` com navegacao principal

---

## 8. DOCUMENTACAO INTRA-CODIGO (PADRAO OBRIGATORIO)

### 8.1 Card de Arquivo (Header Principal)

**REGRA:** Todo arquivo (.cs, .razor) DEVE iniciar com um Card de Identificacao.

#### Modelo para Componentes Blazor (.razor)

```razor
@*
****************************************************************************************
ARQUIVO: NomeDoComponente.razor
--------------------------------------------------------------------------------------
OBJETIVO     : Descricao clara do proposito do componente.

ENTRADAS     : Parametros (@inject services, [Parameter] props, route params).

SAIDAS       : Navegacao, chamadas API, alteracao de estado.

CHAMADA POR  : Navegacao do usuario, route "/rota", componente pai.

CHAMA        : Services (via DI), Navigation, AlertaJs, outros componentes.

DEPENDENCIAS : Syncfusion Blazor, MudBlazor, FontAwesome, SweetAlert2.

OBSERVACOES  : Informacoes adicionais importantes (se aplicavel).
****************************************************************************************
*@
```

#### Modelo para Services (.cs)

```csharp
/* ****************************************************************************************
 * ARQUIVO: NomeDoService.cs
 * --------------------------------------------------------------------------------------
 * OBJETIVO     : Descricao clara da responsabilidade do service.
 *
 * ENTRADAS     : Parametros dos metodos, DI (RelayApiService, ILogService).
 *
 * SAIDAS       : DTOs, ViewModels, resultados de operacao.
 *
 * CHAMADA POR  : Componentes Blazor via injecao de dependencia.
 *
 * CHAMA        : RelayApiService (endpoints API), SecureStorage, ILogService.
 *
 * DEPENDENCIAS : RelayApiService, ILogService, System.Text.Json.
 *
 * OBSERVACOES  : Informacoes adicionais importantes (se aplicavel).
 **************************************************************************************** */
```

### 8.2 Card de Funcao/Metodo

```csharp
/****************************************************************************************
 * FUNCAO: NomeDaFuncao
 * --------------------------------------------------------------------------------------
 * OBJETIVO     : Descricao detalhada do que a funcao faz.
 *
 * ENTRADAS     : param1 [tipo] - Descricao
 *                param2 [tipo] - Descricao
 *
 * SAIDAS       : Tipo de retorno e o que representa.
 *
 * CHAMADO POR  : Quem invoca esta funcao (lifecycle, event handler, outro metodo).
 *
 * CHAMA        : O que esta funcao invoca internamente.
 *
 * OBSERVACOES  : Regras especiais, validacoes, side effects.
 ****************************************************************************************/
```

### 8.3 Comentarios Internos (Tags Semanticas)

**MESMAS tags do Web, mais tags especificas mobile:**

| Tag | Significado | Exemplo de Uso |
| :--- | :--- | :--- |
| `// [UI]` | Manipulacao de DOM/Blazor, visibilidade | `StateHasChanged()` |
| `// [LOGICA]` | Regras de fluxo, algoritmos | `Calculo de media ponderada` |
| `// [REGRA]` | Regras de Negocio obrigatorias | `Validar se KM final > KM inicial` |
| `// [DADOS]` | Manipulacao de Models/DTOs | `Mapear ViewModel para DTO` |
| `// [API]` | Chamadas via RelayApiService | `await _api.GetAsync<T>(...)` |
| `// [STORAGE]` | Operacoes SecureStorage | `SecureStorage.SetAsync(...)` |
| `// [SYNC]` | Logica de sincronizacao | `Download/upload de dados offline` |
| `// [PLATAFORMA]` | Codigo especifico Android/iOS | `#if ANDROID` |
| `// [MEMORIA]` | Limpeza de memoria, GC | `GC.Collect()` |
| `// [VALIDACAO]` | Validacoes de entrada | `if (string.IsNullOrEmpty(nome))` |
| `// [NAVEGACAO]` | Navegacao entre paginas | `Navigation.NavigateTo(...)` |

---

## 9. CONVENCOES DE NOMENCLATURA

### 9.1 Arquivos

| Tipo | Padrao | Exemplo |
|------|--------|---------|
| Componentes Blazor | PascalCase.razor | `VistoriaInicial.razor`, `Home.razor` |
| Code-behind | Arquivo.razor.cs | (gerado do @code) |
| Models | PascalCase.cs | `Viagem.cs`, `OcorrenciaViagem.cs` |
| ViewModels | PascalCase + ViewModel.cs | `VeiculoViewModel.cs`, `SyncStatusViewModel.cs` |
| DTOs | PascalCase + DTO.cs | `CriarOcorrenciasDTO.cs`, `VeiculoLocalDto.cs` |
| Services | PascalCase + Service.cs | `VeiculoService.cs`, `RelayApiService.cs` |
| Interfaces | I + PascalCase.cs | `IVeiculoService.cs`, `ILogService.cs` |
| CSS | kebab-case.css | `FrotiXBlazorMaui.css` |
| JS | snake_case.js | `sweetalert_interop_061.js`, `alerta.js` |

### 9.2 Classes e Metodos

```csharp
// Classes: PascalCase
public class VistoriaInicial { }
public class RelayApiService { }

// Interfaces: I + PascalCase
public interface IViagemService { }

// Metodos publicos: PascalCase + Async suffix
public async Task<List<T>> ObterTodosAsync() { }
public async Task<bool> SalvarVistoria(Viagem viagem) { }

// Metodos privados: PascalCase
private async Task CarregarDadosAsync() { }
private void LimparMemoria() { }

// Event handlers: On + Evento
private async Task OnSalvarClick() { }
private void OnVeiculoValueChange(ChangeEventArgs<Guid?, VeiculoViewModel> args) { }

// Propriedades: PascalCase
public Guid ViagemId { get; set; }
public string? VeiculoPlaca { get; set; }

// Propriedades computadas: PascalCase (expressao)
public bool HoraFimPendente => string.IsNullOrEmpty(HoraFim);
public string DataStr => Data?.ToString("dd/MM/yyyy") ?? "";

// Campos privados: camelCase (sem underscore para componentes Blazor)
private bool carregando = true;
private List<VeiculoViewModel> veiculos = new();

// Campos privados em Services: _camelCase (com underscore)
private readonly RelayApiService _api;
private readonly ILogService _logger;

// Constantes: UPPER_SNAKE_CASE ou PascalCase
private const string KEY_VEICULOS = "veiculos_economildo";
private const int TimeoutSeconds = 60;
```

### 9.3 Blazor Patterns

```csharp
// Nullable para bind
private Guid? veiculoSelecionadoId;
private DateTime? dataViagem;

// Listas inicializadas
private List<VeiculoViewModel> veiculos = new();

// Tuples para resultados de operacao
public async Task<(bool sucesso, string mensagem)> SincronizarAsync()
{
    return (true, "Dados sincronizados com sucesso");
}

// OperationResult (alternativa)
public async Task<OperationResult<T>> OperacaoAsync()
{
    return OperationResult<T>.Success(dados);
}
```

---

## 10. SYNCFUSION BLAZOR — GUIA RAPIDO

### 10.1 Imports Obrigatorios

```razor
@* _Imports.razor *@
@using Syncfusion.Blazor
@using Syncfusion.Blazor.DropDowns
@using Syncfusion.Blazor.Inputs
@using Syncfusion.Blazor.Calendars
@using Syncfusion.Blazor.Buttons
@using Syncfusion.Blazor.Navigations
@using Syncfusion.Blazor.Popups
@using Syncfusion.Blazor.Notifications
```

### 10.2 Controles Mais Usados

#### SfComboBox (Dropdown com filtro)

```razor
<SfComboBox TValue="Guid?" TItem="VeiculoViewModel"
    @bind-Value="veiculoSelecionadoId"
    DataSource="@veiculos"
    AllowFiltering="true"
    FilterType="Syncfusion.Blazor.DropDowns.FilterType.Contains"
    Placeholder="Selecione um veiculo...">
    <ComboBoxFieldSettings Value="VeiculoId" Text="VeiculoCompleto" />
    <ComboBoxEvents TValue="Guid?" TItem="VeiculoViewModel"
        ValueChange="OnVeiculoValueChange"
        Filtering="OnVeiculoFiltering" />
</SfComboBox>
```

#### SfDropDownList (Dropdown sem filtro)

```razor
<SfDropDownList TValue="string" TItem="SelectListItemModel"
    @bind-Value="valorSelecionado"
    DataSource="@opcoes"
    Placeholder="Selecione...">
    <DropDownListFieldSettings Value="Value" Text="Text" />
</SfDropDownList>
```

#### SfDatePicker

```razor
<SfDatePicker TValue="DateTime?"
    @bind-Value="dataViagem"
    Format="dd/MM/yyyy"
    Placeholder="Selecione a data" />
```

#### SfTimePicker

```razor
<SfTimePicker TValue="DateTime?"
    @bind-Value="horaInicio"
    Format="HH:mm"
    Step="5"
    Placeholder="Hora" />
```

#### SfNumericTextBox

```razor
<SfNumericTextBox TValue="int"
    @bind-Value="kmAtual"
    Format="N0"
    Min="0"
    Placeholder="KM atual" />
```

#### SfCheckBox

```razor
<SfCheckBox TChecked="bool"
    @bind-Checked="documentoEntregue"
    Label="CRLV entregue" />
```

#### SfSignature

```razor
<SfSignature @ref="signaturePad"
    MaxStrokeWidth="3"
    MinStrokeWidth="1" />
```

---

## 11. ERROS COMUNS A EVITAR

### 11.1 Blazor

| Erro | Consequencia | Solucao |
|------|-------------|---------|
| Esquecer `StateHasChanged()` | UI nao atualiza | Chamar apos alteracao de estado assincrono |
| Modificar estado em `OnInitialized` (sincrono) | Race condition | Usar `OnInitializedAsync` |
| `@bind` sem `@bind-Value` em Syncfusion | Binding nao funciona | Sempre usar `@bind-Value` |
| Nao limpar listas antes de navegar | Memory leak | Chamar `LimparMemoria()` |
| `await` em handler sem `Task` return | Warning + possivel deadlock | Retornar `Task` no handler |
| JSRuntime antes de OnAfterRender | JS nao disponivel | Usar `OnAfterRenderAsync(firstRender)` |

### 11.2 MAUI

| Erro | Consequencia | Solucao |
|------|-------------|---------|
| `PublishTrimmed=true` | Azure Relay quebra | Manter `false` |
| `RunAOTCompilation=true` | Crash em devices antigos | Manter `false` |
| Foto sem downsampling | OutOfMemory | Usar `AndroidDownsampleToBase64Async()` |
| SecureStorage sem try-catch | Crash silencioso | Sempre envolver em try-catch |
| Nao verificar `Connectivity.Current` | Chamada API sem rede | Verificar antes de chamadas API |

### 11.3 Syncfusion Blazor

| Erro | Consequencia | Solucao |
|------|-------------|---------|
| Esquecer `TValue` e `TItem` | Compilacao falha | Sempre tipar generics |
| `FilterType` sem namespace | Ambiguidade | `Syncfusion.Blazor.DropDowns.FilterType.Contains` |
| `DataSource` null | Componente crashou | Inicializar com `new()` |
| Muitos componentes na mesma pagina | Lentidao | Lazy loading ou tabs |
| Falta de licenca | Banner de trial | `RegisterLicense()` em MauiProgram |

---

## 12. VERSIONAMENTO DESTE ARQUIVO

**Formato:** `X.Y`

- **X** = mudanca estrutural
- **Y** = ajustes incrementais

### Historico de Versoes

| Versao | Data       | Descricao |
| ------ | ---------- | --------- |
| 1.0    | 10/02/2026 | Versao inicial — Criado a partir de analise comparativa com RegrasDesenvolvimentoFrotiX.md (Web) e exploracao completa dos projetos FrotiX.Economildo, FrotiX.Shared e FrotiX.Vistorias |

---

## MEMORIA PERMANENTE

Este arquivo, `RegrasDesenvolvimentoFrotiXMobile.md`, atua como a **MEMORIA PERMANENTE** dos projetos mobile.
Qualquer regra, padrao ou instrucao que deva ser "memorizada" pelo agente deve ser adicionada aqui.

**AGENTES (Claude/Gemini/Copilot):**

1. **LEITURA OBRIGATORIA:** Voce DEVE ler e seguir estritamente as regras deste arquivo quando trabalhar em projetos mobile.
2. **ESCRITA:** Se o usuario pedir para "memorizar" algo relacionado a mobile, adicione neste arquivo.
3. **NAO CONFUNDIR:** Este arquivo e para MOBILE. Para regras WEB, consultar `RegrasDesenvolvimentoFrotiX.md`.
