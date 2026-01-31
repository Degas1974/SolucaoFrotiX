# 🔗 Mapeamento de Dependências - FrotiX.Site

> **Gerado em:** 29/01/2026  
> **Propósito:** Rastreabilidade completa de chamadas entre camadas  
> **Atualizar:** A cada nova função/endpoint criado

---

## 📊 Resumo do Escopo

| Pasta | Arquivos | Status |
|-------|----------|--------|
| Areas | 43 | 🔴 Pendente |
| Controllers | 93 | 🔴 Pendente |
| Data | 5 | 🔴 Pendente |
| EndPoints | 2 | 🔴 Pendente |
| Extensions | 3 | 🔴 Pendente |
| Filters | 4 | 🔴 Pendente |
| Helpers | 6 | 🔴 Pendente |
| Hubs | 5 | 🔴 Pendente |
| Infrastructure | 1 | 🔴 Pendente |
| Logging | 1 | 🔴 Pendente |
| Middlewares | 2 | 🔴 Pendente |
| Models | 139 | 🔴 Pendente |
| Pages | 340 | 🔴 Pendente |
| Properties | 1 | 🔴 Pendente |
| Repository | 209 | 🔴 Pendente |
| Services | 43 | 🔴 Pendente |
| Settings | 4 | 🔴 Pendente |
| Tools | 4 | 🔴 Pendente |
| **TOTAL** | **905** | 0% |

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

> ⚠️ **Nota:** Tabela em construção. Processados: 6/375 arquivos documentados.

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

> ⚠️ **Nota:** Tabela em construção. Processados: 6/375 arquivos documentados.

---

## 📋 TABELA 3: Métodos de Serviço C# x Controllers que os Utilizam

| Service | Método | Controllers Consumidores |
|---------|--------|-------------------------|
| UserManager<IdentityUser> | FindByIdAsync() | ConfirmEmailModel, ConfirmEmailChangeModel |
| UserManager<IdentityUser> | ConfirmEmailAsync() | ConfirmEmailModel |
| UserManager<IdentityUser> | ChangeEmailAsync() | ConfirmEmailChangeModel |
| UserManager<IdentityUser> | SetUserNameAsync() | ConfirmEmailChangeModel |
| SignInManager<IdentityUser> | RefreshSignInAsync() | ConfirmEmailChangeModel |
| IUnitOfWork | GetRepository<T>() | Todos (~80% dos controllers) |
| IUnitOfWork | SaveChangesAsync() | Todos com operações de escrita |
| IGlosaService | ObterResumoAsync() | GlosaController |
| IGlosaService | ObterDetalhesAsync() | GlosaController |
| IAlertasFrotiXRepository | GetAlertasAtivosAsync() | AlertasFrotiXController |
| IAlertasFrotiXRepository | MarcarComoLidoAsync() | AlertasFrotiXController |
| ViagemEstatisticaService | GerarEstatisticasAsync() | AgendaController, ViagemController |
| IHubContext<ImportacaoHub> | SendAsync() | AbastecimentoController, AbastecimentoImportController |
| IHubContext<AlertasHub> | SendAsync() | AlertasFrotiXController |

> ⚠️ **Nota:** Tabela em construção. Processados: 10/375 arquivos documentados.

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

## 📝 Log de Atualizações

| Data | Alteração | Autor |
|------|-----------|-------|
| 29/01/2026 | Criação inicial do mapeamento | Arquiteto IA |

---

**FIM DO MAPEAMENTO**
