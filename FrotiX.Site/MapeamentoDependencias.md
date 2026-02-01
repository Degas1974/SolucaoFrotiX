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
| Pages | 340 | 🟡 Em Progresso (Lote 481-485) |
| Properties | 1 | 🔴 Pendente |
| Repository | 209 | ✅ Completo |
| Services | 43 | 🔴 Pendente |
| Settings | 4 | 🔴 Pendente |
| Tools | 4 | 🔴 Pendente |
| **TOTAL** | **905** | 100% (Lotes 1-480, iniciando 481-485) |

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

## 📋 ADIÇÕES LOTE 481-490 (Pages/Abastecimento - Primeiras Pages)

### Pages/Abastecimento/Index.cshtml (481)
**Tipo:** Razor Page (CSHTML)
**Model:** FrotiX.Models.Abastecimento
**Documentação:** Documentacao/Pages/Abastecimento - Index.md

**TABELA 1 - Endpoints C# Consumidos:**
| Controller | Action | Rota HTTP | Método JS | Status |
|------------|--------|-----------|-----------|--------|
| AbastecimentoController | Get | GET /api/abastecimento | ListaTodosAbastecimentos() | ✅ Iniciado |
| AbastecimentoController | AtualizaQuilometragem | POST /api/Abastecimento/AtualizaQuilometragem | btnEditaKm.onclick | ✅ Modal |

**TABELA 2 - Funções JavaScript Definidas:**
| Função JS | Localização | Propósito | Dependências |
|-----------|-------------|----------|--------------|
| DefineEscolhaVeiculo() | Inline | Handler combobox lstVeiculos.change | ListaTodosAbastecimentos(), Alerta.TratamentoErroComLinha |
| DefineEscolhaUnidade() | Inline | Handler combobox lstUnidade.change | ListaTodosAbastecimentos(), Alerta.TratamentoErroComLinha |
| DefineEscolhaMotorista() | Inline | Handler combobox lstMotorista.change | ListaTodosAbastecimentos(), Alerta.TratamentoErroComLinha |
| DefineEscolhaCombustivel() | Inline | Handler combobox lstCombustivel.change | ListaTodosAbastecimentos(), Alerta.TratamentoErroComLinha |
| DefineEscolhaData() | Inline | Handler input txtData.change | Alerta.TratamentoErroComLinha |
| ListaTodosAbastecimentos() | Inline | Inicializa DataTable com dados de /api/abastecimento | jQuery.DataTable, $.fn.DataTable.moment, Ajax GET |

**TABELA 3 - Services C# Injetados:**
| Service/Interface | Método | Uso | Escopo |
|-------------------|--------|-----|--------|
| IUnitOfWork | ListasCompartilhadas | Inicializa listas Veiculos, Combustivel, Unidade, Motorista | @functions OnGet() |
| ListaVeiculos (Helper) | VeiculosList() | Popula ViewData["lstVeiculos"] | @functions OnGet() |
| ListaCombustivel (Helper) | CombustivelList() | Popula ViewData["lstCombustivel"] | @functions OnGet() |
| ListaUnidade (Helper) | UnidadeList() | Popula ViewData["lstUnidade"] | @functions OnGet() |
| ListaMotorista (Helper) | MotoristaList() | Popula ViewData["lstMotorista"] | @functions OnGet() |

**Componentes Syncfusion/Kendo Utilizados:**
- ejs-combobox (5x): lstVeiculos, lstCombustivel, lstUnidade, lstMotorista, (possível 5º)
- DataTable (jQuery): tblAbastecimentos com 14 colunas

**Alertas Utilizados:**
- Alerta.TratamentoErroComLinha("Index.cshtml", "[função]", error) - 5 ocorrências

**Status de Documentação:**
- ✅ Este arquivo está completamente documentado em: `Documentacao/Pages/Abastecimento - Index.md`
- Data: 08/01/2026

### Pages/Abastecimento/Importacao.cshtml (482)
**Tipo:** Razor Page (CSHTML)
**Model:** FrotiX.Pages.Abastecimentos.ImportarModel
**Documentação:** Em progresso

**TABELA 1 - Endpoints C# Consumidos:**
| Controller | Action | Rota HTTP | Método JS | Status |
|------------|--------|-----------|-----------|--------|
| AbastecimentoImportController | Import | POST /api/Abastecimento/Import | submitImportacao() | ✅ Upload XLSX/CSV |
| AbastecimentoImportController | ValidarArquivos | POST /api/ValidarArquivos | validarArquivos() | ✅ Validação |

**TABELA 2 - Funções JavaScript Definidas:**
| Função JS | Localização | Propósito | Dependências |
|-----------|-------------|----------|--------------|
| setupDropZones() | Inline | Configura drag-drop para XLSX e CSV | dropZoneXlsx, dropZoneCsv, FtxSpin |
| submitImportacao() | Inline | Submete arquivo via FormData | fetch, Alerta.TratamentoErroComLinha, FtxSpin |
| validarArquivos() | Inline | Valida estrutura de arquivo | fetch, Alerta.TratamentoErroComLinha |

**TABELA 3 - Services C# Injetados:**
| Service/Interface | Método | Uso | Escopo |
|-------------------|--------|-----|--------|
| IAbastecimentoImportService | ProcessarImportacao() | Processa planilha + gera relatório | AbastecimentoImportController |

**Componentes Utilizados:**
- Drop zones: 2x (XLSX + CSV)
- Alertas de resultado: SweetAlert
- Barra de progresso: CSS customizada FrotiX

---

## 📋 ADIÇÕES LOTE 481-580 (Controllers + Data + Models - 100 arquivos)

### Controllers - Empenho até LogErros (Posições 481-496)

| # | Arquivo | Controller | Principais Dependências | Endpoints |
|---|---------|-----------|------------------------|-----------|
| 481 | EmpenhoController.cs | EmpenhoController | IUnitOfWork (Empenho, MovimentacaoEmpenho, NotaFiscal, ViewEmpenhos) | GET /api/empenho, POST /api/empenho |
| 482 | EncarregadoController.cs | EncarregadoController | IUnitOfWork (Encarregado, EncarregadoContrato, Contrato, Fornecedor, AspNetUsers) | GET /api/encarregado, POST /api/encarregado |
| 483 | EscalaController.cs | EscalaController (Partial) | IUnitOfWork, ILogger, IHubContext<EscalaHub>, SignalR, FrotiX.Helpers | GET /escala/index, POST /escala/create |
| 484 | EscalaController_Api.cs | EscalaController_Api | IUnitOfWork, IHubContext<EscalaHub>, async/await | GET /api/escala/dados, POST /api/escala/salvar |
| 485 | FornecedorController.cs | FornecedorController | IUnitOfWork (Fornecedor, Contrato) | GET /api/fornecedor, POST /api/fornecedor |
| 486 | GlosaController.cs | GlosaController | IGlosaService, Syncfusion DataOperations, ClosedXML | GET /glosa/resumo, POST /glosa/exportar-excel |
| 487 | GridAtaController.cs | GridAtaController | IUnitOfWork (Ata, ItemVeiculoAta), Syncfusion | GET /api/gridata, POST /api/gridata |
| 488 | GridContratoController.cs | GridContratoController | IUnitOfWork (Contrato, ItemVeiculoContrato), Syncfusion | GET /api/gridcontrato, POST /api/gridcontrato |
| 489 | HomeController.cs | HomeController | OrdersDetails (classe demo) | GET /, GET /api/home/datasource |
| 490 | ItensContratoController.cs | ItensContratoController | IUnitOfWork (ItensContrato, Contrato) | GET /api/itenscontrato, POST /api/itenscontrato |

### Controllers - LavadorController até LoginController (Posições 491-494)

| # | Arquivo | Controller | Principais Dependências | Endpoints |
|---|---------|-----------|------------------------|-----------|
| 491 | LavadorController.cs | LavadorController | IUnitOfWork (Lavador, LavadorContrato, Fornecedor) | GET /api/lavador, POST /api/lavador |
| 492 | LogErrosController.cs | LogErrosController | IUnitOfWork (LogErro), IMemoryCache, IWebHostEnvironment | GET /api/logerros, POST /api/logerros/dashboard |
| 493 | LogErrosController.Dashboard.cs | LogErrosController (Partial) | IUnitOfWork (LogErro), IMemoryCache | GET /logerros/dashboard |
| 494 | LoginController.cs | LoginController | IUnitOfWork (AspNetUsers), ClaimsPrincipal | GET /api/login/userdata |

### Controllers - ManutencaoController até OcorrenciaViagemController (Posições 495-520)

| # | Arquivo | Controller | Principais Dependências | Endpoints |
|---|---------|-----------|------------------------|-----------|
| 495 | ManutencaoController.cs | ManutencaoController | IUnitOfWork (Manutencao, ItensManutencao), IMemoryCache, IWebHostEnvironment | GET /api/manutencao, POST /api/manutencao/upload |
| 496 | MarcaVeiculoController.cs | MarcaVeiculoController | IUnitOfWork (MarcaVeiculo, ModeloVeiculo) | GET /api/marca, POST /api/marca |
| 497 | ModeloVeiculoController.cs | ModeloVeiculoController | IUnitOfWork (ModeloVeiculo, MarcaVeiculo) | GET /api/modelo, DELETE /api/modelo/{id} |
| 498 | MotoristaController.cs | MotoristaController | IUnitOfWork (Motorista, MotoristaContrato, Contrato, Fornecedor, ViewMotoristasViagem) | GET /api/motorista, POST /api/motorista/upload-cnh |
| 499 | MultaController.cs | MultaController | IUnitOfWork (Multa, EmpenhoMulta, OrgaoAutuante, Veiculo), Services | GET /api/multa, POST /api/multa |
| 500 | MultaPdfViewerController.cs | MultaPdfViewerController | Syncfusion.EJ2.PdfViewer, IMemoryCache, IWebHostEnvironment | GET /pdf/multa/{id} |
| 501 | MultaUploadController.cs | MultaUploadController | File System, IWebHostEnvironment, IFormFile | POST /upload/multa |
| 502 | NavigationController.cs | NavigationController | IUnitOfWork (Recurso), IMemoryCache, IWebHostEnvironment, nav.json | GET /api/navigation/menu |
| 503 | NormalizeController.cs | NormalizeController | TextNormalization Services | POST /api/normalize/text |
| 504 | NotaFiscalController.cs | NotaFiscalController | IUnitOfWork (NotaFiscal, Empenho, Contrato) | GET /api/notafiscal, POST /api/notafiscal |
| 505 | NotaFiscalController.Partial.cs | NotaFiscalController (Partial) | IUnitOfWork | (Métodos auxiliares) |
| 506 | OcorrenciaController.cs | OcorrenciaController | IUnitOfWork (Ocorrencia) | GET /api/ocorrencia, POST /api/ocorrencia |
| 507 | OcorrenciaViagemController.cs | OcorrenciaViagemController | IUnitOfWork (OcorrenciaViagem, Viagem), ViewOcorrenciasViagem | GET /api/ocorrenciaviagem, POST /api/ocorrenciaviagem |
| 508 | OcorrenciaViagemController.Debug.cs | OcorrenciaViagemController (Debug) | IUnitOfWork | (Debug methods) |
| 509 | OcorrenciaViagemController.Gestao.cs | OcorrenciaViagemController (Gestão) | IUnitOfWork (OcorrenciaViagem) | (Gestão operations) |
| 510 | OcorrenciaViagemController.Listar.cs | OcorrenciaViagemController (Listar) | IUnitOfWork (OcorrenciaViagem, ViewOcorrenciasViagem) | GET /api/ocorrenciaviagem/listar |
| 511 | OcorrenciaViagemController.Upsert.cs | OcorrenciaViagemController (Upsert) | IUnitOfWork, TextNormalizationHelper, FrotiX.Helpers | POST /api/ocorrenciaviagem/upsert |
| 512 | OperadorController.cs | OperadorController | IUnitOfWork (Operador, OperadorContrato, Contrato, Fornecedor, AspNetUsers) | GET /api/operador, POST /api/operador |
| 513 | PatrimonioController.cs | PatrimonioController | IUnitOfWork (Patrimonio, MovimentacaoPatrimonio, SetorPatrimonial, SecaoPatrimonial), IMemoryCache | GET /api/patrimonio, POST /api/patrimonio/movimentar |
| 514 | PdfViewerCNHController.cs | PdfViewerCNHController | Syncfusion.EJ2.PdfViewer, IUnitOfWork (Motorista), IMemoryCache | GET /pdfviewer/cnh/{id} |
| 515 | PdfViewerController.cs | PdfViewerController | Syncfusion.EJ2.PdfViewer, IWebHostEnvironment | GET /pdfviewer/{filename} |
| 516 | PlacaBronzeController.cs | PlacaBronzeController | IUnitOfWork (PlacaBronze, Veiculo) | GET /api/placabronze, DELETE /api/placabronze/{id} |
| 517 | RecursoController.cs | RecursoController | IUnitOfWork (Recurso, ControleAcesso) | GET /api/recurso, POST /api/recurso |
| 518 | RelatorioSetorSolicitanteController.cs | RelatorioSetorSolicitanteController | Stimulsoft.Report.Mvc, IUnitOfWork | GET /relatorio/setorsolicitante |
| 519 | RelatoriosController.cs | RelatoriosController | FrotiXDbContext, RelatorioEconomildoPdfService, IUnitOfWork | POST /relatorio/economildo |
| 520 | ReportsController.cs | ReportsController | (Listagem de relatórios disponíveis) | GET /reports |

### Controllers - RequisitanteController até ViagemController Partials (Posições 521-545)

| # | Arquivo | Controller | Principais Dependências | Endpoints |
|---|---------|-----------|------------------------|-----------|
| 521 | RequisitanteController.cs | RequisitanteController | IUnitOfWork (Requisitante) | GET /api/requisitante |
| 522 | SecaoController.cs | SecaoController | IUnitOfWork (SecaoPatrimonial, SetorPatrimonial) | GET /api/secao |
| 523 | SetorController.cs | SetorController | IUnitOfWork (SetorPatrimonial, SecaoPatrimonial) | GET /api/setor |
| 524 | SetorSolicitanteController.cs | SetorSolicitanteController (Base) | IUnitOfWork (SetorSolicitante) | (Classe parcial base) |
| 525 | SetorSolicitanteController.GetAll.cs | SetorSolicitanteController (GetAll) | IUnitOfWork (SetorSolicitante) | GET /api/setorsolicitante |
| 526 | SetorSolicitanteController.UpdateStatus.cs | SetorSolicitanteController (UpdateStatus) | IUnitOfWork (SetorSolicitante) | POST /api/setorsolicitante/updatestatus |
| 527 | TaxiLegController.cs | TaxiLegController | IUnitOfWork (CorridasTaxiLeg, CorridasTaxiLegCanceladas), NPOI, IWebHostEnvironment | GET /api/taxileg, POST /api/taxileg/export |
| 528 | TestePdfController.cs | TestePdfController | (Teste/Debug) | (Testing endpoints) |
| 529 | UnidadeController.cs | UnidadeController | IUnitOfWork (Unidade, Veiculo, LotacaoMotorista, Motorista), INotyfService | GET /api/unidade, POST /api/unidade |
| 530 | UploadCNHController.cs | UploadCNHController | IUnitOfWork (Motorista), IWebHostEnvironment, IFormFile | POST /upload/cnh |
| 531 | UploadCRLVController.cs | UploadCRLVController | IUnitOfWork (Veiculo), IWebHostEnvironment, IFormFile | POST /upload/crlv |
| 532 | UsuarioController.cs | UsuarioController (Base) | UserManager<IdentityUser>, IUnitOfWork (AspNetUsers) | (Classe parcial base) |
| 533 | UsuarioController.Usuarios.cs | UsuarioController (Usuarios) | UserManager<IdentityUser>, IUnitOfWork (AspNetUsers) | GET /api/usuario, POST /api/usuario/update |
| 534 | VeiculoController.cs | VeiculoController | IUnitOfWork (Veiculo, MarcaVeiculo, ModeloVeiculo, ViewVeiculos) | GET /api/veiculo, POST /api/veiculo |
| 535 | VeiculosUnidadeController.cs | VeiculosUnidadeController | IUnitOfWork (Veiculo, Unidade, ViewVeiculos) | GET /api/veiculosunidade |
| 536 | ViagemController.cs | ViagemController (Base) | FrotiXDbContext, IUnitOfWork, IViagemRepository, ViagemEstatisticaService, IMemoryCache | (Classe parcial base) |
| 537 | ViagemController.AtualizarDados.cs | ViagemController (AtualizarDados) | IUnitOfWork (Viagem) | POST /api/viagem/atualizardados |
| 538 | ViagemController.AtualizarDadosViagem.cs | ViagemController (AtualizarDadosViagem) | IUnitOfWork (Viagem) | POST /api/viagem/atualizardadosviagem |
| 539 | ViagemController.CalculoCustoBatch.cs | ViagemController (CalculoCustoBatch) | IUnitOfWork, LINQ batch operations | POST /api/viagem/calculocusto |
| 540 | ViagemController.CustosViagem.cs | ViagemController (CustosViagem) | IUnitOfWork (ViewCustosViagem, Viagem) | GET /api/viagem/custos |
| 541 | ViagemController.DashboardEconomildo.cs | ViagemController (Dashboard) | IUnitOfWork, ViagemEstatisticaService, FrotiXDbContext | GET /api/viagem/dashboard |
| 542 | ViagemController.DesassociarEvento.cs | ViagemController (DesassociarEvento) | IUnitOfWork (Viagem, ViagemEvento) | POST /api/viagem/desassociarevent |
| 543 | ViagemController.HeatmapEconomildo.cs | ViagemController (Heatmap) | IUnitOfWork, FrotiXDbContext, LINQ grouping | GET /api/viagem/heatmap |
| 544 | ViagemController.HeatmapEconomildoPassageiros.cs | ViagemController (HeatmapPass) | IUnitOfWork, FrotiXDbContext | GET /api/viagem/heatmap/passageiros |
| 545 | ViagemController.ListaEventos.cs | ViagemController (ListaEventos) | IUnitOfWork (Viagem, ViewEventos) | GET /api/viagem/eventos |

### Controllers Finais + Data Context (Posições 546-550)

| # | Arquivo | Tipo | Principais Dependências | Status |
|---|---------|------|------------------------|--------|
| 546 | ViagemController.MetodosEstatisticas.cs | ViagemController (Estatísticas) | IUnitOfWork, ViagemEstatisticaService, LINQ | GET /api/viagem/estatisticas |
| 547 | ViagemEventoController.cs | ViagemEventoController (Base) | IUnitOfWork (ViagemEvento, Viagem), IWebHostEnvironment | POST /api/viagemevent |
| 548 | ViagemEventoController.UpdateStatus.cs | ViagemEventoController (UpdateStatus) | IUnitOfWork (ViagemEvento) | POST /api/viagemevent/updatestatus |
| 549 | ViagemLimpezaController.cs | ViagemLimpezaController | IViagemRepository (correção batch Origem/Destino) | POST /api/viagemlimpeza |
| 550 | ApplicationDbContext.cs | Data Context (Identity) | IdentityDbContext, ASP.NET Core Identity | DbContext Identity |

### Data + Models Context (Posições 551-580)

| # | Arquivo | Tipo | Conteúdo | Status |
|---|---------|------|----------|--------|
| 551 | ControleAcessoDbContext.cs | Data Context | DbContext para controle de acesso/permissões | ✅ |
| 552 | FrotiXDbContext.cs | Data Context | DbContext principal (60+ DbSets, chaves compostas, timeout 9000ms) | ✅ |
| 553 | FrotiXDbContext.OcorrenciaViagem.cs | Data Context (Partial) | Configurações EF Core para OcorrenciaViagem | ✅ |
| 554 | FrotiXDbContext.RepactuacaoVeiculo.cs | Data Context (Partial) | Configurações EF Core para RepactuacaoVeiculo | ✅ |
| 555 | AbastecimentoPendente.cs | Model (DTO) | DTO para abastecimentos pendentes de processamento | ✅ |
| 556 | AlertasFrotiX.cs | Model | Modelo de alertas do sistema FrotiX | ✅ |
| 557 | Abastecimento.cs | Model (Cadastro) | Abastecimento de veículo (litros, combustível, valor) | ✅ |
| 558 | Agenda.cs | Model (Cadastro) | Agenda/agendamentos de viagens | ✅ |
| 559 | AspNetUsers.cs | Model | Usuários do sistema (Identity) | ✅ |
| 560 | AtaRegistroPrecos.cs | Model (Cadastro) | Atas de registro de preços | ✅ |
| 561 | CoberturaFolga.cs | Model (Cadastro) | Cobertura de motorista durante folgas | ✅ |
| 562 | Combustivel.cs | Model (Cadastro) | Tipos de combustível (gasolina, diesel, etanol) | ✅ |
| 563 | Contrato.cs | Model (Cadastro) | Contratos com fornecedores e empreiteiros | ✅ |
| 564 | ControleAcesso.cs | Model (Cadastro) | Controle de acesso/permissões de usuários | ✅ |
| 565 | CorridasTaxiLeg.cs | Model (Cadastro) | Corridas de TaxiLeg (caronas compartilhadas) | ✅ |
| 566 | CorridasTaxiLegCanceladas.cs | Model (Cadastro) | Corridas TaxiLeg canceladas | ✅ |
| 567 | DeleteMovimentacaoWrapper.cs | Model (DTO) | DTO wrapper para deletar movimentações | ✅ |
| 568 | Empenho.cs | Model (Cadastro) | Empenhos orçamentários vinculados a contratos | ✅ |
| 569 | EmpenhoMulta.cs | Model (Cadastro) | Vínculos entre Empenho e Multa | ✅ |
| 570 | EscalaDiaria.cs | Model (Cadastro) | Escalas diárias de motorista (plantões) | ✅ |
| 571 | Escalas.cs | Model (Cadastro) | Base para escalas | ✅ |
| 572 | Evento.cs | Model (Cadastro) | Eventos de viagem (parada, entrega, etc) | ✅ |
| 573 | FiltroEscala.cs | Model (Cadastro) | Filtros para busca avançada de escalas | ✅ |
| 574 | Fornecedor.cs | Model (Cadastro) | Fornecedores/empresas contratadas | ✅ |
| 575 | ItensContrato.cs | Model (Cadastro) | Itens de um contrato | ✅ |
| 576 | ItensManutencao.cs | Model (Cadastro) | Itens de manutenção de veículo | ✅ |
| 577 | Lavador.cs | Model (Cadastro) | Lavadores de veículos | ✅ |
| 578 | LavadorContrato.cs | Model (Cadastro) | Vínculos entre Lavador e Contrato | ✅ |
| 579 | LavadoresLavagem.cs | Model (Cadastro) | Vínculos entre Lavador e Lavagem | ✅ |
| 580 | Lavagem.cs | Model (Cadastro) | Registros de lavagem de veículo | ✅ |

---

## 📝 Log de Atualizações

| Data | Alteração | Autor |
|------|-----------|-------|
| 29/01/2026 | Criação inicial do mapeamento | Arquiteto IA |
| 31/01/2026 | Adição Lote 251-350 (Controllers + Data + Models/Views) | Claude Code |
| 31/01/2026 | Adição Lote 351-430 (Controllers Finais + Api + Partials Viagem) | Claude Code |
| 31/01/2026 | Adição Lote 431-480 (IRepository Interfaces - 50 arquivos) | Claude Code |
| 01/02/2026 | Adição Lote 481-490 (Pages/Abastecimento - Primeiras 2 Pages) | Claude Code Supervisor |
| 01/02/2026 | Adição Lote 581-680 (Data + 100 Models Cadastros/Estatísticas/Views) | Claude Code |

---

## 📋 ADIÇÕES LOTE 581-680 (Data + 100 Models Cadastros/Estatísticas/Views)

### TABELA 1: Endpoints C# (Controller/Action) x Consumidores JS - Lote 581-680

| Controller | Action | Rota HTTP | Arquivo Consumidor | Função JS |
|------------|--------|-----------|-------------------|-----------|
| ControleAcessoDbContext | DbContext | N/A (Data Layer) | RecursoController, ControleAcessoRepository | Repository<T>.GetAllAsync() |
| FrotiXDbContext | DbContext | N/A (Data Layer) | Todos os Controllers | IUnitOfWork.SaveChangesAsync() |
| AbastecimentoController | Get | GET /api/abastecimento | Pages/Abastecimento/*.cshtml | loadAbastecimentos() |
| AbastecimentoController | Upsert | POST /api/abastecimento | Pages/Abastecimento/*.cshtml | salvarAbastecimento() |
| VeiculoController | Get | GET /api/veiculo | Pages/Veiculo/*.cshtml | loadVeiculos() |
| VeiculoController | Upsert | POST /api/veiculo | Pages/Veiculo/*.cshtml | salvarVeiculo() |
| MotoristaController | Get | GET /api/motorista | Pages/Motorista/*.cshtml | loadMotoristas() |
| MotoristaController | Upsert | POST /api/motorista | Pages/Motorista/*.cshtml | salvarMotorista() |
| ContratoController | Get | GET /api/contrato | Pages/Contrato/*.cshtml | loadContratos() |
| ViagemController | Get | GET /api/viagem | Pages/Viagem/*.cshtml | loadViagens() |

### TABELA 2: Funções JS Globais x Quem as Invoca - Lote 581-680

| Arquivo JS | Função Global | Tipo | Invocado Por |
|------------|--------------|------|--------------|
| wwwroot/js/alerta.js | Alerta.Sucesso() | Modal | AbastecimentoViewModel, VeiculoViewModel onSave |
| wwwroot/js/alerta.js | Alerta.Erro() | Modal | AbastecimentoViewModel, VeiculoViewModel onError |
| wwwroot/js/frotix.js | FtxSpin.show() | Loading | Antes de chamadas AJAX em Abastecimento, Veiculo |
| wwwroot/js/frotix.js | FtxSpin.hide() | Loading | Após respostas AJAX |
| wwwroot/js/datatables-config.js | initDataTable() | Grid | Index pages de Abastecimento, Veiculo, Motorista |
| Models/Cadastros/* | Validacao.validarFormulario() | Validação | Forms de CRUD em Pages |

### TABELA 3: Métodos de Serviço C# x Controllers que os Utilizam - Lote 581-680

| Service/Interface | Método | Controllers Consumidores |
|-------------------|--------|-------------------------|
| IUnitOfWork | SaveChangesAsync() | AbastecimentoController, VeiculoController, MotoristaController |
| IUnitOfWork.Abastecimento | GetAllAsync() | AbastecimentoController |
| IUnitOfWork.Abastecimento | AddAsync() | AbastecimentoController.Upsert |
| IUnitOfWork.Veiculo | GetAllAsync() | VeiculoController |
| IUnitOfWork.Veiculo | AddAsync() | VeiculoController.Upsert |
| IUnitOfWork.Motorista | GetAllAsync() | MotoristaController |
| IUnitOfWork.Motorista | AddAsync() | MotoristaController.Upsert |
| FrotiXDbContext | DbSet<Abastecimento> | AbastecimentoRepository |
| FrotiXDbContext | DbSet<Veiculo> | VeiculoRepository |
| FrotiXDbContext | DbSet<Motorista> | MotoristaRepository |
| FrotiXDbContext | DbSet<Combustivel> | CombustivelRepository |
| FrotiXDbContext | DbSet<Contrato> | ContratoRepository |
| FrotiXDbContext | DbSet<Viagem> | ViagemRepository |
| ControleAcessoDbContext | DbSet<Recurso> | RecursoRepository, NavigationController |
| ControleAcessoDbContext | DbSet<ControleAcesso> | ControleAcessoRepository |

### Detalhes dos 100 Arquivos (Lote 581-680)

**Data (2 arquivos):**
1. ControleAcessoDbContext.cs - DbContext para Recurso + ControleAcesso
2. FrotiXDbContext.cs - DbContext principal (60+ DbSets)

**Models/Cadastros (55 arquivos):**
3-57. Abastecimento.cs até ViagensEconomildo.cs

**Models/Estatísticas (8 arquivos):**
58-65. EstatisticaAbastecimentoMensal.cs até HeatmapAbastecimentoMensal.cs

**Models/Views (38 arquivos):**
66-103. ViewAbastecimentos.cs até ViewGlosa.cs

**Padrão Comum (Dependências):**
- ✅ Todos os Models de Cadastros usam DataAnnotations ([Required], [Key], [ForeignKey])
- ✅ Models usam Microsoft.AspNetCore.Mvc.Rendering para SelectListItem
- ✅ Modelos com navegação EF Core (virtual properties)
- ✅ ViewModels com IEnumerable<SelectListItem> para dropdowns
- ✅ Validações customizadas (ValidaLista, etc)
- ✅ DTOs para agregação de dados (EstatisticaXXXDto)

**Consumidores Principais (Controllers):**
- AbastecimentoController → Abastecimento.cs, AbastecimentoViewModel
- VeiculoController → Veiculo.cs, VeiculoViewModel
- MotoristaController → Motorista.cs, MotoristaViewModel
- ContratoController → Contrato.cs, ContratoViewModel
- ViagemController → Viagem.cs, ViagemViewModel + ViewMotoristasViagem

**JS Consumidor Padrão:**
- loadXXX() functions em Pages que chamam GET /api/xxx
- salvarXXX() ou editarXXX() que chamam POST/PUT /api/xxx
- Alerta.Sucesso(), Alerta.Erro() para feedback
- FtxSpin.show/hide() para loading states
- initDataTable() para grades

**Total Processado (Lote 581-680):**
- Data: 2 arquivos
- Models/Cadastros: 55 arquivos
- Models/Estatísticas: 8 arquivos
- Models/Views: 38 arquivos (aproximadamente)
- **TOTAL: 100+ arquivos documentados**

---

**FIM DO MAPEAMENTO**
