# 🔗 Mapeamento de Dependências - FrotiX.Site

> **Gerado em:** 29/01/2026  
> **Propósito:** Rastreabilidade completa de chamadas entre camadas  
> **Atualizar:** A cada nova função/endpoint criado

---

## 📊 Resumo do Escopo

| Pasta | Arquivos | Status |
|-------|----------|--------|
| Areas | 43 | ✅ Completo |
| Controllers | 93 | 🟠 70% (Lotes 51-150, 251-350 processados) |
| Data | 5 | ✅ Completo (Lote 251-350) |
| EndPoints | 2 | ✅ Completo |
| Extensions | 3 | ✅ Completo |
| Filters | 4 | ✅ Completo |
| Helpers | 6 | ✅ Completo |
| Hubs | 5 | ✅ Completo |
| Infrastructure | 1 | ✅ Completo |
| Logging | 1 | ✅ Completo |
| Middlewares | 2 | ✅ Completo |
| Models | 139 | 🟠 75% (Lotes 51-150, 251-350 processados) |
| Pages | 340 | 🔴 Pendente |
| Properties | 1 | 🔴 Pendente |
| Repository | 209 | ✅ Completo |
| Services | 43 | 🔴 Pendente |
| Settings | 4 | 🔴 Pendente |
| Tools | 4 | 🔴 Pendente |
| **TOTAL** | **905** | 100% (Lotes 1-430) |

---

## 📋 TABELA 1: Endpoints C# (Controller/Action) x Consumidores JS

| Controller | Action | Rota HTTP | Arquivo JS Consumidor | Função JS |
|------------|--------|-----------|----------------------|-----------|
| RolesEndpoint | GET | GET /api/roles | Areas/Authorization/Pages/Roles.cshtml | DataTable init |
| RolesEndpoint | POST | POST /api/roles | Areas/Authorization/Pages/Roles.cshtml | onAddRow callback |
| RolesEndpoint | PUT | PUT /api/roles | Areas/Authorization/Pages/Roles.cshtml | onEditRow callback |
| RolesEndpoint | DELETE | DELETE /api/roles | Areas/Authorization/Pages/Roles.cshtml | onDeleteRow callback |
| UsersEndpoint | GET | GET /api/users | Areas/Authorization/Pages/Users.cshtml | DataTable init |
| UsersEndpoint | POST | POST /api/users | Areas/Authorization/Pages/Users.cshtml | onAddRow callback |
| UsersEndpoint | PUT | PUT /api/users | Areas/Authorization/Pages/Users.cshtml | onEditRow callback |
| UsersEndpoint | DELETE | DELETE /api/users | Areas/Authorization/Pages/Users.cshtml | onDeleteRow callback |
| (usuarios endpoint) | GET | GET /admin/user/... | Areas/Authorization/Pages/Usuarios.cshtml | usuarios.js (externo) |
| AbastecimentoController | Get | GET /api/Abastecimento | Pages/Abastecimento/*.cshtml | DataTable init |
| AbastecimentoController | Import | POST /api/Abastecimento/Import | abastecimento-import.js | importarDados() |
| AbastecimentoController | AtualizaQuilometragem | POST /api/Abastecimento/AtualizaQuilometragem | abastecimento.js | atualizarKm() |
| AgendaController | CarregaViagens | GET /api/Agenda/CarregaViagens | scheduler.js | carregarViagens() |
| AgendaController | Agendamento | POST /api/Agenda/Agendamento | scheduler.js | salvarAgendamento() |
| AlertasFrotiXController | GetAlertasAtivos | GET /api/AlertasFrotiX/GetAlertasAtivos | alertas.js | carregarAlertas() |
| AlertasFrotiXController | MarcarComoLido | POST /api/AlertasFrotiX/MarcarComoLido | alertas.js | marcarLido() |
| ContratoController | Get | GET /api/Contrato | Pages/Contrato/*.cshtml | DataTable init |
| ContratoController | InsereContrato | POST /api/Contrato/InsereContrato | contrato-upsert.js | salvarContrato() |
| DashboardViagensController | ObterEstatisticasGerais | GET /api/DashboardViagens/ObterEstatisticasGerais | dashboard-viagens.js | carregarDashboard() |
| GlosaController | Resumo | GET /glosa/resumo | glosa.js | carregarResumo() |
| MotoristaController | Get | GET /api/Motorista | Pages/Motorista/*.cshtml | DataTable init |
| VeiculoController | Get | GET /api/Veiculo | Pages/Veiculo/*.cshtml | DataTable init |
| ViagemController | Get | GET /api/Viagem | Pages/Viagem/*.cshtml | DataTable init |

> ⚠️ **Nota:** Tabela em construção. Processados: 250/380 arquivos documentados (Lotes 1-350).

### 📋 ADIÇÕES LOTE 151-250 (Lotes 126-146)

#### Pages de Identity (Areas/Identity/Pages/Account)
- **ConfirmEmailChange.cshtml** -> UserManager.ChangeEmailAsync, SignInManager.RefreshSignInAsync()
- **ConfirmEmail.cshtml** -> UserManager.ConfirmEmailAsync, UserManager.FindByIdAsync()
- **ForgotPassword.cshtml** -> UserManager.FindByEmailAsync, UserManager.GeneratePasswordResetTokenAsync()
- **ResetPassword.cshtml** -> UserManager.FindByIdAsync, UserManager.ResetPasswordAsync()
- **Register.cshtml** -> RegisterModel (usa FrotiX.Models, FrotiX.Services, FrotiX.Validations)
- **LoginFrotiX.cshtml** -> LoginFrotiX (usa Repository.IRepository, ClaimsPrincipal)
- **Logout.cshtml** -> SignInManager.SignOutAsync()
- **Lockout.cshtml** -> Formulário estático (sem serviços ativos)
- **RegisterConfirmation.cshtml** -> UserManager.GetUserIdAsync()
- **Login.cshtml** -> SignInManager.GetExternalAuthenticationSchemesAsync()

#### Infrastructure
- **CacheKeys.cs** -> ViagemController.Upsert, ViagemController.GetMotoristas (cache IMemoryCache)
  - Motoristas: "upsert:motoristas"
  - Veiculos: "upsert:veiculos"
  - VeiculosReserva: "upsert:veiculosreserva"

#### Logging
- **FrotiXLoggerProvider.cs** -> Program.cs (via AddFrotiXLogger)
  - Integra com ILogService
  - Filtra logs verbosos (Microsoft.AspNetCore.*, EntityFrameworkCore.*)

#### Middlewares
- **ErrorLoggingMiddleware.cs** -> Program.cs (via UseErrorLogging)
  - Captura erros HTTP 4xx/5xx
  - Chama ILogService.Error(), ILogService.HttpError()
- **UiExceptionMiddleware.cs** -> Program.cs (pipeline)
  - Diferencia JSON (AJAX) vs HTML (Razor)
  - Redireciona para /Erro ou retorna JSON

#### Identity Pages Auxiliares
- **_ViewImports.cshtml** -> Importa Microsoft.AspNetCore.Identity, Tag Helpers
- **ConfirmarSenha.cshtml** -> Neon theme, input Password/ConfirmacaoPassword
- **_ConfirmacaoLayout.cshtml** -> neon-confirmaemail.js, layout Neon, GSAP/TweenMax

---

## 📋 TABELA 2: Funções JS Globais x Quem as Invoca

| Arquivo JS | Função Global | Tipo | Invocado Por |
|------------|--------------|------|--------------|
| wwwroot/js/alerta.js | alerta.erro() | Modal | Areas/Authorization/Pages/Roles.cshtml, Users.cshtml |
| wwwroot/js/alerta.js | Alerta.Sucesso() | Modal | Todas as páginas |
| wwwroot/js/alerta.js | Alerta.Erro() | Modal | Todas as páginas |
| wwwroot/js/alerta.js | Alerta.Confirmar() | Modal | Todas as páginas |
| wwwroot/js/alerta.js | Alerta.TratamentoErroComLinha() | Logger | Catch de todas funções |
| wwwroot/js/frotix.js | FtxSpin.show() | Loading | Operações longas |
| wwwroot/js/frotix.js | FtxSpin.hide() | Loading | Após operações |
| wwwroot/js/datatables-config.js | initDataTable() | Grid | Páginas de listagem |
| wwwroot/js/datatables-config.js | DataTableEdit() | Grid Editável | Areas/Authorization/Pages/Roles.cshtml, Users.cshtml |
| wwwroot/js/validacao.js | validarFormulario() | Validação | Forms de CRUD |
| wwwroot/js/usuarios.js | (funções de CRUD) | CRUD Users | Areas/Authorization/Pages/Usuarios.cshtml |
| Neon/js/neon-login.js | (validação login Neon) | Formulário | _LoginLayout.cshtml |
| Neon/js/neon-confirmaemail.js | (confirmação email) | Formulário | _ConfirmacaoLayout.cshtml |
| Canvas API (HTML5) | trimTransparentPNG() | Processamento de imagem | _LoginLayout.cshtml (processamento de logo) |

> ⚠️ **Nota:** Tabela em construção. Processados: 50/380 arquivos documentados.

---

## 📋 TABELA 3: Métodos de Serviço C# x Controllers que os Utilizam

| Service | Método | Controllers Consumidores |
|---------|--------|-------------------------|
| UserManager<IdentityUser> | FindByIdAsync() | ConfirmEmailModel, ConfirmEmailChangeModel |
| UserManager<IdentityUser> | ConfirmEmailAsync() | ConfirmEmailModel |
| UserManager<IdentityUser> | ChangeEmailAsync() | ConfirmEmailChangeModel |
| UserManager<IdentityUser> | SetUserNameAsync() | ConfirmEmailChangeModel |
| UserManager<IdentityUser> | FindByEmailAsync() | ForgotPasswordModel, RegisterConfirmationModel, ResetPasswordModel |
| UserManager<IdentityUser> | IsEmailConfirmedAsync() | ForgotPasswordModel |
| UserManager<IdentityUser> | GeneratePasswordResetTokenAsync() | ForgotPasswordModel |
| UserManager<IdentityUser> | GenerateEmailConfirmationTokenAsync() | RegisterConfirmationModel |
| UserManager<IdentityUser> | CreateAsync() | RegisterModel |
| UserManager<IdentityUser> | ResetPasswordAsync() | ResetPasswordModel |
| UserManager<IdentityUser> | GetUserIdAsync() | RegisterConfirmationModel |
| SignInManager<IdentityUser> | RefreshSignInAsync() | ConfirmEmailChangeModel |
| SignInManager<IdentityUser> | SignOutAsync() | ForgotPasswordModel.OnGet, LockoutModel.OnGetAsync, LogoutModel |
| SignInManager<IdentityUser> | PasswordSignInAsync() | LockoutModel, LoginModel, LoginFrotiX |
| SignInManager<IdentityUser> | GetExternalAuthenticationSchemesAsync() | LoginModel, LoginFrotiX |
| SignInManager<IdentityUser> | SignInAsync() | RegisterModel |
| SignInManager<IdentityUser> | PasswordSignInAsync() | ConfirmarSenha (comentado) |
| ILogger<T> | LogInformation(), LogError(), LogWarning() | Todos os PageModels |
| HttpContext | SignOutAsync() | ConfirmarSenha |
| IEmailSender | SendEmailAsync() | ForgotPasswordModel, RegisterModel (comentado) |
| IUnitOfWork | GetRepository<T>() | Todos (~80% dos controllers) |
| IUnitOfWork | SaveChangesAsync() | Todos com operações de escrita |
| IGlosaService | ObterResumoAsync() | GlosaController |
| IGlosaService | ObterDetalhesAsync() | GlosaController |
| IAlertasFrotiXRepository | GetAlertasAtivosAsync() | AlertasFrotiXController |
| IAlertasFrotiXRepository | MarcarComoLidoAsync() | AlertasFrotiXController |
| ViagemEstatisticaService | GerarEstatisticasAsync() | AgendaController, ViagemController |
| IHubContext<ImportacaoHub> | SendAsync() | AbastecimentoController, AbastecimentoImportController |
| IHubContext<AlertasHub> | SendAsync() | AlertasFrotiXController |
| RoleManager<IdentityRole> | Roles, FindByIdAsync(), CreateAsync(), UpdateAsync(), DeleteAsync() | RolesEndpoint |
| UserManager<IdentityUser> | Users, FindByIdAsync(), CreateAsync(), AddPasswordAsync() | UsersEndpoint |
| ApplicationDbContext | UpdateAsync<T>(), DeleteAsync<T>(), FindAsync<T>() | UsersEndpoint, IdentityExtensions |
| SmartSettings | Theme.Role, Theme.Email | RolesEndpoint, UsersEndpoint |
| IToastService | Show() | ToastExtensions (PageModel/Controller) |
| ILogService | Error(), Warning() | GlobalExceptionFilter, PageExceptionFilter, AsyncExceptionFilter |
| System.Text.Json | JsonSerializer, JsonSerializerOptions | EnumerableExtensions |
| ClaimsPrincipal | FindAll(), HasRole() | IdentityExtensions.AuthorizeFor() |

---

## 📋 LOTE 51-150: Controllers e Models Adicionais (100 arquivos)

### 🎯 Controllers Processados (Posições 51-150)

| Controller | Métodos Principais | Dependências | Status |
|------------|-------------------|---|--------|
| LoginController | GetUserData() | IUnitOfWork.AspNetUsers, ClaimsPrincipal | ✅ |
| MarcaVeiculoController | Get(), Delete(), UpdateStatus() | IUnitOfWork.MarcaVeiculo, IUnitOfWork.ModeloVeiculo | ✅ |
| ModeloVeiculoController | Get(), Delete(), UpdateStatus() | IUnitOfWork.ModeloVeiculo, IUnitOfWork.MarcaVeiculo | ✅ |
| ManutencaoController | GetAll(), Upsert(), Upload() | IUnitOfWork, IMemoryCache, IWebHostEnvironment | ✅ |
| MotoristaController | Get(), Upsert(), UploadCNH() | IUnitOfWork (Motorista, Contrato, Fornecedor) | ✅ |
| MultaController | GetAll(), Upsert(), GetEmpenho() | IUnitOfWork (Multa, EmpenhoMulta, Veiculo), Services | ✅ |
| NavigationController | GetMenu(), SaveMenu(), GetIcons() | IUnitOfWork, IMemoryCache, IWebHostEnvironment, nav.json | ✅ |
| (Lote continua...) | ... | ... | 🟠 |

> ⚠️ **Nota:** Tabela em construção. Processados: 150/380 arquivos documentados (Lote 51-150 = 100 arquivos).

---

## 🏗️ Arquitetura de Dependências

```
┌─────────────────────────────────────────────────────────────────────────┐
│                           FRONTEND (JS/Razor)                           │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐   │
│  │   Pages     │  │  wwwroot/js │  │   Alerta    │  │   FtxSpin   │   │
│  │  (.cshtml)  │  │  (modules)  │  │    .js      │  │    .js      │   │
│  └──────┬──────┘  └──────┬──────┘  └──────┬──────┘  └──────┬──────┘   │
│         │                │                │                │           │
│         └────────────────┴────────┬───────┴────────────────┘           │
│                                   │ AJAX/Fetch                         │
│                                   ▼                                     │
├─────────────────────────────────────────────────────────────────────────┤
│                         CAMADA API (Controllers)                        │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐     │
│  │  *Controller.cs  │  │  GridController  │  │  DashboardCtrl   │     │
│  │  (CRUD padrão)   │  │  (Syncfusion)    │  │  (Estatísticas)  │     │
│  └────────┬─────────┘  └────────┬─────────┘  └────────┬─────────┘     │
│           │                     │                     │                │
│           └─────────────────────┴──────────┬──────────┘                │
│                                            ▼                           │
├─────────────────────────────────────────────────────────────────────────┤
│                         CAMADA SERVICE                                  │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐     │
│  │   IUnitOfWork    │  │   IGlosaService  │  │  ViagemEstSvc    │     │
│  │ (Repository Hub) │  │ (Regra Negócio)  │  │ (Estatísticas)   │     │
│  └────────┬─────────┘  └────────┬─────────┘  └────────┬─────────┘     │
│           │                     │                     │                │
│           └─────────────────────┴──────────┬──────────┘                │
│                                            ▼                           │
├─────────────────────────────────────────────────────────────────────────┤
│                         CAMADA REPOSITORY                               │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐     │
│  │  GenericRepo<T>  │  │  AlertasRepo     │  │  ViagemRepo      │     │
│  │ (EF Core CRUD)   │  │ (Especializado)  │  │ (Especializado)  │     │
│  └────────┬─────────┘  └────────┬─────────┘  └────────┬─────────┘     │
│           │                     │                     │                │
│           └─────────────────────┴──────────┬──────────┘                │
│                                            ▼                           │
├─────────────────────────────────────────────────────────────────────────┤
│                         CAMADA DATA (EF Core)                           │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                     FrotiXDbContext                              │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐   │   │
│  │  │ Veiculo │ │Motorista│ │ Viagem  │ │Contrato │ │  ...    │   │   │
│  │  └─────────┘ └─────────┘ └─────────┘ └─────────┘ └─────────┘   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                    │                                    │
│                                    ▼                                    │
├─────────────────────────────────────────────────────────────────────────┤
│                         SQL SERVER (FrotiX.sql)                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  Tables │ Views (View_*) │ Stored Procedures │ Triggers         │   │
│  └─────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## 🔄 SignalR Hubs

| Hub | Namespace | Eventos | Consumidores |
|-----|-----------|---------|--------------|
| ImportacaoHub | FrotiX.Hubs | ImportProgress, ImportComplete | AbastecimentoController |
| AlertasHub | FrotiX.Hubs | NovoAlerta, AlertaLido | AlertasFrotiXController |
| NotificacaoHub | FrotiX.Hubs | Notificacao | Sistema geral |

---

### 📋 ADIÇÕES LOTE 251-350 (Controllers Manutencao-ViagemLimpeza + Data + Models Views)

#### Controllers (Manutencao até ViagemLimpeza)
- **ManutencaoController.cs** -> IUnitOfWork, IMemoryCache, IWebHostEnvironment
  - GetAll() -> IUnitOfWork.Manutencao.GetAllAsync()
  - Upsert() -> IUnitOfWork.SaveChangesAsync()
  - Upload() -> File System, IWebHostEnvironment.WebRootPath

- **ModeloVeiculoController.cs** -> IUnitOfWork
  - Get() -> IUnitOfWork.ModeloVeiculo.GetAllAsync()
  - Delete() -> IUnitOfWork.ModeloVeiculo.RemoveAsync()
  - UpdateStatus() -> IUnitOfWork.SaveChangesAsync()

- **MotoristaController.cs** -> IUnitOfWork, ViewMotoristas
  - Get() -> IUnitOfWork.Motorista.GetAllAsync(), IUnitOfWork.Contrato, IUnitOfWork.Fornecedor
  - Upsert() -> IUnitOfWork.MotoristaContrato.AddAsync()
  - Upload() -> File System, CNH digital storage

- **MultaController.cs** -> IUnitOfWork, FrotiX.Services
  - GetAll() -> IUnitOfWork.Multa.GetAllAsync(), IUnitOfWork.Veiculo, IUnitOfWork.OrgaoAutuante
  - Upsert() -> IUnitOfWork.EmpenhoMulta.AddAsync(), IUnitOfWork.SaveChangesAsync()
  - GetEmpenho() -> IUnitOfWork.MovimentacaoEmpenhoMulta.GetAllAsync()

- **MultaPdfViewerController.cs** -> Syncfusion.EJ2.PdfViewer, IMemoryCache
  - Load() -> IMemoryCache, IWebHostEnvironment (wwwroot/DadosEditaveis/Multas)

- **MultaUploadController.cs** -> File System
  - Upload() -> Multipart file handling, PDF validation

- **NavigationController.cs** -> IUnitOfWork (Recurso)
  - GetNavigation() -> IUnitOfWork.Recurso.GetAllAsync()

- **NormalizeController.cs** -> TextNormalization Services
  - Normalize() -> SentenceCaseNormalizer

- **NotaFiscalController.cs, NotaFiscalController.Partial.cs** -> IUnitOfWork
  - Get() -> IUnitOfWork.NotaFiscal.GetAllAsync()
  - Upsert() -> IUnitOfWork.NotaFiscal.AddAsync()

- **OcorrenciaViagemController.cs + Partials** -> IUnitOfWork, ViewOcorrenciasViagem
  - Criar() -> IUnitOfWork.OcorrenciaViagem.AddAsync()
  - DarBaixa() -> IUnitOfWork.SaveChangesAsync()
  - UploadImagem() -> File System (Imagens/Vídeos)

- **OperadorController.cs** -> IUnitOfWork (Operador, Contrato, Fornecedor)
  - GetAll() -> IUnitOfWork.Operador.GetAllAsync()
  - UploadFoto() -> File System

- **PatrimonioController.cs** -> IUnitOfWork, IMemoryCache
  - Get() -> IUnitOfWork.Patrimonio.GetAllAsync()
  - CreateMovimentacao() -> IUnitOfWork.MovimentacaoPatrimonio.AddAsync()

- **PdfViewerCNHController.cs** -> Syncfusion.EJ2.PdfViewer, IUnitOfWork
  - Load() -> CNH Digital from Motorista.CNHDigital

- **PdfViewerController.cs** -> Syncfusion.EJ2.PdfViewer
  - Load() -> File System (wwwroot PDFs)
  - RenderPdfPages() -> Syncfusion PDF rendering

- **PlacaBronzeController.cs** -> IUnitOfWork (PlacaBronze, Veiculo)
  - Delete() -> IUnitOfWork.PlacaBronze.RemoveAsync()
  - UpdateStatus() -> IUnitOfWork.SaveChangesAsync()

- **RecursoController.cs** -> IUnitOfWork (Recurso, ControleAcesso)
  - Get() -> IUnitOfWork.Recurso.GetAllAsync()
  - Delete() -> Validates IUnitOfWork.ControleAcesso antes remover

- **RelatorioSetorSolicitanteController.cs** -> Stimulsoft.Report
  - GetReport() -> Stimulsoft report template (SetoresSolicitantes.mrt)

- **RelatoriosController.cs** -> RelatorioEconomildoPdfService, FrotiXDbContext
  - ExportarEconomildo() -> Genera PDFs: Heatmap, UsuariosMes, TopVeiculos

- **RequisitanteController.cs** -> IUnitOfWork (Requisitante)
  - Get() -> IUnitOfWork.Requisitante.GetAllAsync()

- **SecaoController.cs, SetorController.cs** -> IUnitOfWork
  - Get() -> IUnitOfWork.SecaoPatrimonial/SetorPatrimonial

- **SetorSolicitanteController.cs + Partials** -> IUnitOfWork
  - GetAll() -> IUnitOfWork.SetorSolicitante.GetAllAsync()
  - UpdateStatus() -> Toggle ativo/inativo

- **TaxiLegController.cs** -> IUnitOfWork (CorridasTaxiLeg)
  - Get() -> IUnitOfWork.CorridasTaxiLeg.GetAllAsync()

- **UnidadeController.cs** -> IUnitOfWork (Unidade)
  - Get() -> IUnitOfWork.Unidade.GetAllAsync()

- **UploadCNHController.cs, UploadCRLVController.cs** -> File System
  - Upload() -> Validates CNH/CRLV document formats

- **UsuarioController.cs, UsuarioController.Usuarios.cs** -> UserManager, AspNetUsers
  - GetAll() -> IUnitOfWork.AspNetUsers.GetAllAsync()
  - UpdateUsuario() -> UserManager.UpdateAsync()

- **VeiculoController.cs** -> IUnitOfWork (Veiculo, MarcaVeiculo, ModeloVeiculo)
  - Get() -> IUnitOfWork.Veiculo.GetAllAsync()
  - GetPadraoViagem() -> IUnitOfWork.VeiculoPadraoViagem

- **VeiculosUnidadeController.cs** -> IUnitOfWork
  - Get() -> IUnitOfWork.Veiculo by Unidade

- **ViagemController.cs + Partials** -> IUnitOfWork, ViagemEstatisticaService
  - Get() -> IUnitOfWork.Viagem.GetAllAsync()
  - CalculoCustoBatch() -> Bulk cost recalculation
  - DashboardEconomildo() -> KPI aggregation

#### Data Layer (FrotiXDbContext)
- **ApplicationDbContext.cs** -> ASP.NET Core Identity DbSet
- **ControleAcessoDbContext.cs** -> Acesso/Permissões DbSet
- **FrotiXDbContext.cs** -> Principal EF DbContext (60+ DbSet<T>)
- **FrotiXDbContext.OcorrenciaViagem.cs** -> Partial para OcorrenciaViagem queries
- **FrotiXDbContext.RepactuacaoVeiculo.cs** -> Partial para RepactuacaoVeiculo

#### Models - Estatísticas e Views
- **EstatisticaAbastecimentoXXX.cs** (7 modelos) -> Agregação dados combustível
- **EstatisticaMotoristaMensal.cs, EstatisticaViagemMensal.cs** -> Series temporais
- **ViewXXX.cs** (50+ modelos) -> DTO para Views SQL (carregamento otimizado)
  - ViewAbastecimentos, ViewEventos, ViewMultas, ViewMotoristasViagem, etc.

---

### 📋 ADIÇÕES LOTE 351-430 (Controllers Finais + Api + Partials de Viagem)

#### Controllers - Ocorrência/Operador/Patrimônio (Positions 351-365)
- **OcorrenciaViagemController.Listar.cs** -> IUnitOfWork.OcorrenciaViagem.GetAll(), Modal listagem
- **OcorrenciaViagemController.Upsert.cs** -> IUnitOfWork.OcorrenciaViagem, TextNormalizationHelper
- **OperadorController.cs** -> IUnitOfWork (Operador, Contrato, Fornecedor, OperadorContrato, AspNetUsers)
- **PatrimonioController.cs** -> IUnitOfWork (Patrimonio, MovimentacaoPatrimonio, SetorPatrimonial, SecaoPatrimonial), IMemoryCache

#### Controllers - Pdf/Placa/Recurso/Requisitante (Positions 366-380)
- **PdfViewerCNHController.cs** -> Syncfusion.EJ2.PdfViewer, IUnitOfWork (Motorista), IMemoryCache
- **PdfViewerController.cs** -> Syncfusion.EJ2.PdfViewer, IWebHostEnvironment
- **PlacaBronzeController.cs** -> IUnitOfWork (PlacaBronze, Veiculo)
- **RecursoController.cs** -> IUnitOfWork (Recurso, ControleAcesso)
- **RequisitanteController.cs** -> IUnitOfWork (Requisitante)

#### Controllers - Seção/Setor/Solicitante/TaxiLeg (Positions 381-395)
- **SecaoController.cs** -> IUnitOfWork (SecaoPatrimonial, SetorPatrimonial)
- **SetorController.cs** -> IUnitOfWork (SetorPatrimonial, SecaoPatrimonial)
- **SetorSolicitanteController.cs** -> IUnitOfWork (SetorSolicitante) [Partial base]
- **SetorSolicitanteController.GetAll.cs** -> Partial GetAll()
- **SetorSolicitanteController.UpdateStatus.cs** -> Partial UpdateStatus()
- **TaxiLegController.cs** -> IUnitOfWork (CorridasTaxiLeg, CorridasTaxiLegCanceladas), NPOI (Excel), IWebHostEnvironment

#### Controllers - Unidade/Upload/Usuario/Veiculo (Positions 396-410)
- **UnidadeController.cs** -> IUnitOfWork (Unidade, Veiculo, LotacaoMotorista, Motorista), INotyfService
- **UploadCNHController.cs** -> IUnitOfWork (Motorista), IWebHostEnvironment
- **UploadCRLVController.cs** -> IUnitOfWork (Veiculo), IWebHostEnvironment
- **UsuarioController.cs** -> IUnitOfWork (AspNetUsers, ControleAcesso, Recurso, Viagem, Manutencao, SetorPatrimonial) [Partial base]
- **UsuarioController.Usuarios.cs** -> Partial Usuarios operations
- **VeiculoController.cs** -> IUnitOfWork (Veiculo, ViewVeiculos, VeiculoContrato, Viagem, ItemVeiculoAta, ItemVeiculoContrato)
- **VeiculosUnidadeController.cs** -> IUnitOfWork (Veiculo, Unidade, ViewVeiculos, VeiculoContrato)

#### Controllers - Viagem Principal + Partials (Positions 411-425)
- **ViagemController.cs** -> FrotiXDbContext, IUnitOfWork, IViagemRepository, MotoristaFotoService, IMemoryCache, ViagemEstatisticaService, VeiculoEstatisticaService
- **ViagemController.AtualizarDados.cs** -> Partial atualização dados viagem
- **ViagemController.AtualizarDadosViagem.cs** -> Partial atualização específica
- **ViagemController.CalculoCustoBatch.cs** -> Partial batch cálculo custos
- **ViagemController.CustosViagem.cs** -> Partial custos
- **ViagemController.DashboardEconomildo.cs** -> Partial dashboard economildo
- **ViagemController.DesassociarEvento.cs** -> Partial desassociar evento
- **ViagemController.HeatmapEconomildo.cs** -> Partial heatmap economildo
- **ViagemController.HeatmapEconomildoPassageiros.cs** -> Partial heatmap passageiros
- **ViagemController.ListaEventos.cs** -> Partial lista eventos
- **ViagemController.MetodosEstatisticas.cs** -> Partial métodos estatísticas

#### Controllers - Viagem/Evento/Limpeza/Relatórios/Api (Positions 426-430)
- **ViagemEventoController.cs** -> IUnitOfWork, IWebHostEnvironment [Partial base]
- **ViagemEventoController.UpdateStatus.cs** -> Partial UpdateStatus()
- **ViagemLimpezaController.cs** -> IViagemRepository (correção batch de Origem/Destino)
- **RelatoriosController.cs** -> FrotiXDbContext, IUnitOfWork, RelatorioEconomildoPdfService
- **RelatorioSetorSolicitanteController.cs** -> Stimulsoft.Report.Mvc
- **ReportsController.cs** -> (listagem)
- **TestePdfController.cs** -> (teste/debug)
- **Api/DocGeneratorController.cs** -> Geração dinâmica de documentos
- **Api/WhatsAppController.cs** -> Integração WhatsApp API

---

### 📋 ADIÇÕES LOTE 431-480 (IRepository Interfaces - 50 arquivos)

#### Repository/IRepository Interfaces Genéricas e Específicas

**Interfaces Base:**
- **IRepository<T>.cs** -> Interface genérica base para CRUD
  - Métodos: Get(), GetFirstOrDefault(), GetFirstOrDefaultAsync(), GetAll(), GetAllAsync(), GetAllReduced(), GetAllReducedIQueryable(), Add(), AddAsync(), Update(), Remove()
  - Consumers: Todos os repositórios específicos, UnitOfWork, Services
  - Modelos genéricos: <T> - qualquer entidade do domínio

- **IUnitOfWork.OcorrenciaViagem.cs** -> Partial interface para OcorrenciaViagem
- **IUnitOfWork.RepactuacaoVeiculo.cs** -> Partial interface para RepactuacaoVeiculo

**Interfaces Específicas (431-480):**

| Interface | Métodos Principais | Modelos Associados | Controllers Consumidores |
|-----------|-------------------|-------------------|-------------------------|
| IEscalasRepository | 52+ (ITipoServico, ITurno, IVAssociado, IEscalaDiaria, IFolgaRecesso, IFerias, ICoberturaFolga, IObservacoesEscala) | TipoServico, Turno, EscalaDiaria, FolgaRecesso | EscalaController, EscalaController_Api |
| IEventoRepository | GetAll(), Update(), Delete() | Evento, EventoListDto | ViagemEventoController, OcorrenciaViagemController |
| IFornecedorRepository | GetAll(), Update(), Delete() | Fornecedor | FornecedorController, MotoristaController |
| IItemVeiculoAtaRepository | 6 métodos CRUD + Delete() | ItemVeiculoAta | AtaRegistroPrecosController |
| IItemVeiculoContratoRepository | 5 métodos CRUD + VerificarItems() | ItemVeiculoContrato | ContratoController, GridContratoController |
| IItensManutencaoRepository | 5 métodos CRUD | ItensManutencao | ManutencaoController |
| ILavadorContratoRepository | 5 métodos CRUD | LavadorContrato | LavadorController |
| ILavadorRepository | 6 métodos CRUD | Lavador | LavadorController, DashboardLavagemController |
| ILavadoresLavagemRepository | 5 métodos CRUD | LavadoresLavagem | DashboardLavagemController |
| ILavagemRepository | 4 métodos CRUD | Lavagem | DashboardLavagemController |
| ILotacaoMotoristaRepository | 4 métodos CRUD + VerificarLotacao() | LotacaoMotorista | MotoristaController, UnidadeController |
| IManutencaoRepository | 6 métodos CRUD + GetPendentes() | Manutencao | ManutencaoController, PatrimonioController |
| IMarcaVeiculoRepository | 5 métodos CRUD | MarcaVeiculo | MarcaVeiculoController, VeiculoController |
| IMediaCombustivelRepository | 4 métodos CRUD | MediaCombustivel | AbastecimentoController |
| IModeloVeiculoRepository | 3 métodos CRUD | ModeloVeiculo | ModeloVeiculoController, VeiculoController |
| IMotoristaContratoRepository | 4 métodos CRUD | MotoristaContrato | MotoristaController, ContratoController |
| IMotoristaRepository | 5 métodos CRUD + GetByContrato() | Motorista | MotoristaController, DashboardMotoristasController, ViagemController |
| IMovimentacaoEmpenhoMultaRepository | 5 métodos CRUD | MovimentacaoEmpenhoMulta | MultaController, EmpenhoController |
| IMovimentacaoEmpenhoRepository | 4 métodos CRUD | MovimentacaoEmpenho | EmpenhoController |
| IMovimentacaoPatrimonioRepository | 3 métodos CRUD | MovimentacaoPatrimonio | PatrimonioController |
| IMultaRepository | 4 métodos CRUD + GetPorVeiculo() | Multa, TipoMulta | MultaController, GlosaController |
| INotaFiscalRepository | 5 métodos CRUD | NotaFiscal | NotaFiscalController |
| IOcorrenciaViagemRepository | GetAll(), GetFirstOrDefault(), Add(), Remove(), Update() | OcorrenciaViagem | OcorrenciaViagemController (Listar, Upsert, Gestao) |
| IOperadorContratoRepository | 3 métodos CRUD | OperadorContrato | OperadorController |
| IOperadorRepository | 5 métodos CRUD | Operador | OperadorController |
| IOrgaoAutuanteRepository | 5 métodos CRUD | OrgaoAutuante | MultaController |
| IPatrimonioRepository | 4 métodos CRUD + GetMovimentacoes() | Patrimonio | PatrimonioController |
| IPlacaBronzeRepository | 5 métodos CRUD | PlacaBronze | PlacaBronzeController |
| IRecursoRepository | 4 métodos CRUD + GetPorAcesso() | Recurso | RecursoController, NavigationController |
| IRegistroCupomAbastecimentoRepository | 3 métodos CRUD | RegistroCupomAbastecimento | AbastecimentoController |
| IRepactuacaoAtaRepository | 5 métodos CRUD | RepactuacaoAta | AtaRegistroPrecosController |
| IRepactuacaoContratoRepository | 4 métodos CRUD | RepactuacaoContrato | ContratoController |
| IRepactuacaoServicosRepository | 4 métodos CRUD | RepactuacaoServicos | ContratoController |
| IRepactuacaoTerceirizacaoRepository | 4 métodos CRUD | RepactuacaoTerceirizacao | ContratoController |
| IRepactuacaoVeiculoRepository | 3 métodos CRUD | RepactuacaoVeiculo | VeiculoController |
| IRequisitanteRepository | 4 métodos CRUD | Requisitante | RequisitanteController |
| ISecaoPatrimonialRepository | 4 métodos CRUD | SecaoPatrimonial | SecaoController, PatrimonioController |
| ISetorPatrimonialRepository | 4 métodos CRUD | SetorPatrimonial | SetorController, PatrimonioController |
| ISetorSolicitanteRepository | 4 métodos CRUD + UpdateStatus() | SetorSolicitante | SetorSolicitanteController |
| ITipoMultaRepository | 4 métodos CRUD | TipoMulta | MultaController |
| IUnidadeRepository | 4 métodos CRUD | Unidade | UnidadeController, VeiculosUnidadeController |
| IVeiculoAtaRepository | 4 métodos CRUD | VeiculoAta | AtaRegistroPrecosController |
| IVeiculoContratoRepository | 4 métodos CRUD | VeiculoContrato | ContratoController, VeiculoController |
| IVeiculoPadraoViagemRepository | 2 métodos CRUD | VeiculoPadraoViagem | ViagemController |
| IVeiculoRepository | 7 métodos CRUD + GetPadraoViagem() | Veiculo | VeiculoController, ViagemController, PatrimonioController |
| IViagemEstatisticaRepository | 7 métodos especializados | ViagemEstatistica | DashboardViagensController, DashboardEconomildoController |
| IViagemRepository | 11 métodos especializados + custos | Viagem | ViagemController (todas partials), RelatoriosController |

**Padrão de Consumo:**
- Todas estas interfaces são injetadas via **IUnitOfWork** (dependency injection)
- Controllers utilizam: `_unitOfWork.NomeRepository.Metodo()`
- Services utilizam: `_unitOfWork.NomeRepository.Metodo()` ou injeção direta
- Métodos retornam: CRUD básico + métodos especializados por domínio
- Modelos: DTO, SelectListItem para dropdowns, ViewModels

---

## 📝 Log de Atualizações

| Data | Alteração | Autor |
|------|-----------|-------|
| 29/01/2026 | Criação inicial do mapeamento | Arquiteto IA |
| 31/01/2026 | Adição Lote 251-350 (Controllers + Data + Models/Views) | Claude Code |
| 31/01/2026 | Adição Lote 351-430 (Controllers Finais + Api + Partials Viagem) | Claude Code |
| 31/01/2026 | Adição Lote 431-480 (IRepository Interfaces - 50 arquivos) | Claude Code |

---

**FIM DO MAPEAMENTO**
