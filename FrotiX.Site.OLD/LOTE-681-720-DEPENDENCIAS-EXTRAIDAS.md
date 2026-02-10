# Lote 681-720: Extração de Dependências Completa

**Data:** 2026-02-01 12:30:00
**Status:** ✅ SINCRONIZAÇÃO FINAL (720/905 = 79.5%)
**Arquivos:** 40 Models (Estatísticas, DTO, Views)

---

## Sumário Executivo

Este lote conclui a extração de dependências dos **últimos 40 arquivos documentados** (681-720), representando a última etapa para sincronização completa com `DocumentacaoIntracodigo.md`.

**Arquivos Processados:**
1. DateItem.cs (681)
2. Encarregado.cs (682)
3. EncarregadoContrato.cs (683)
4. ErrorViewModel.cs (684)
5. LogErro.cs (685)
6. ApiResponse.cs (686)
7. AnosDisponiveisAbastecimento.cs (687)
8. EstatisticaAbastecimentoCategoria.cs (688)
9. EstatisticaAbastecimentoCombustivel.cs (689)
10. EstatisticaAbastecimentoMensal.cs (690)
11. EstatisticaAbastecimentoTipoVeiculo.cs (691)
12. EstatisticaAbastecimentoVeiculo.cs (692)
13. EstatisticaAbastecimentoVeiculoMensal.cs (693)
14. EstatisticaGeralMensal.cs (694)
15. EstatisticaMotoristasMensal.cs (695)
16. EvolucaoViagensDiaria.cs (696)
17. HeatmapAbastecimentoMensal.cs (697)
18. HeatmapViagensMensal.cs (698)
19. RankingMotoristasMensal.cs (699)
20. EventoListDto.cs (700)
21. FontAwesomeIconsModel.cs (701)
22. ForgotAccount.cs (702)
23. INavigationModel.cs (703)
24. LoginView.cs (704)
25. MailRequest.cs (705)
26. NavigationItemDTO.cs (706)
27. OcorrenciaViagem.cs (707)
28. ExcelViewModel.cs (708)
29. RecursoTreeDTO.cs (709)
30. RepactuacaoVeiculo.cs (710)
31. SmartSettings.cs (711)
32. TempDataExtensions.cs (712)
33. ToastMessage.cs (713)
34. VeiculoPadraoViagem.cs (714)
35. ViagemEventoDto.cs (715)
36. ViewOcorrenciasViagem.cs (716)
37. ViewAbastecimentos.cs (717)
38. ViewAtaFornecedor.cs (718)
39. ViewContratoFornecedor.cs (719)
40. ViewControleAcesso.cs (720)

---

## TABELA 1: Endpoints C# (Controller/Action) x Consumidores JS

| # | Model | Controller | Rota HTTP | Consumidor JS | Função |
|---|-------|-----------|-----------|--------------|--------|
| 681 | DateItem | ViagemController | GET /api/viagem/datas | datatables-config.js | initDataTable() |
| 682 | Encarregado | EncarregadoController | GET /api/encarregado | contrato-upsert.js | salvarEncarregado() |
| 683 | EncarregadoContrato | ContratoController | PUT /api/contrato/encarregado | contrato-upsert.js | addEncarregado() |
| 684 | ErrorViewModel | ErrorController | GET /Erro | error-page.js | (static) |
| 685 | LogErro | LogErrosController | GET /api/logerros | logErros-dashboard.js | carregarLogs() |
| 686 | ApiResponse | All Controllers | /api/* | frotix-api-client.js | handleResponse() |
| 687 | AnosDisponiveisAbastecimento | DashboardAbastecimento | GET /api/dashboard/anos | dashboard-abast.js | loadYears() |
| 688 | EstatisticaAbastecimentoCategoria | DashboardAbastecimento | GET /api/dashboard/categoria | chart-categoria.js | renderChartCategoria() |
| 689 | EstatisticaAbastecimentoCombustivel | DashboardAbastecimento | GET /api/dashboard/combustivel | chart-combustivel.js | renderChartCombustivel() |
| 690 | EstatisticaAbastecimentoMensal | DashboardAbastecimento | GET /api/dashboard/mensal | chart-mensal.js | renderChartMensal() |
| 691 | EstatisticaAbastecimentoTipoVeiculo | DashboardAbastecimento | GET /api/dashboard/tipo-veiculo | chart-tipo-veiculo.js | renderChartTipo() |
| 692 | EstatisticaAbastecimentoVeiculo | DashboardAbastecimento | GET /api/dashboard/veiculo | list-veiculo.js | loadVehicles() |
| 693 | EstatisticaAbastecimentoVeiculoMensal | DashboardAbastecimento | GET /api/dashboard/veiculo-mensal | report-veiculo-mensal.js | generateReport() |
| 694 | EstatisticaGeralMensal | DashboardViagens | GET /api/dashboard/geral | dashboard-geral.js | loadKPIs() |
| 695 | EstatisticaMotoristasMensal | DashboardMotoristas | GET /api/dashboard/motorista | ranking-motoristas.js | loadRanking() |
| 696 | EvolucaoViagensDiaria | DashboardViagens | GET /api/dashboard/evolucao | chart-evolucao.js | renderEvolucao() |
| 697 | HeatmapAbastecimentoMensal | DashboardAbastecimento | GET /api/dashboard/heatmap | heatmap-abast.js | renderHeatmapAbast() |
| 698 | HeatmapViagensMensal | DashboardViagens | GET /api/dashboard/heatmap | heatmap-viagens.js | renderHeatmapViagens() |
| 699 | RankingMotoristasMensal | DashboardMotoristas | GET /api/dashboard/ranking | table-ranking.js | loadTableRanking() |
| 700 | EventoListDto | EventoController | GET /api/evento/lista | eventos-grid.js | loadEventos() |
| 701 | FontAwesomeIconsModel | NavigationController | GET /api/navigation/icons | icon-selector.js | loadIcons() |
| 702 | ForgotAccount | AccountController | POST /account/forgot | forgot-form.js | submitForgot() |
| 703 | INavigationModel | LayoutController | GET /api/menu | navigation-builder.js | buildMenu() |
| 704 | LoginView | AccountController | POST /account/login | login-form.js | validateLogin() |
| 705 | MailRequest | EmailService | (Service) | (Server-side) | N/A |
| 706 | NavigationItemDTO | NavigationController | PUT /api/navigation/save | menu-admin.js | saveMenuTree() |
| 707 | OcorrenciaViagem | OcorrenciaViagemController | POST /api/ocorrencia | ocorrencia-form.js | submitOcorrencia() |
| 708 | ExcelViewModel | ImportController | POST /api/import/excel | excel-upload.js | submitExcel() |
| 709 | RecursoTreeDTO | RecursoController | GET /api/recursos/tree | recurso-tree.js | renderTree() |
| 710 | RepactuacaoVeiculo | ContratoController | PUT /api/contrato/repactuar | repactuar-form.js | submitRepactuar() |
| 711 | SmartSettings | Startup | (Config) | (Config global) | applyTheme() |
| 712 | TempDataExtensions | All Controllers | (Helper) | (Server-side) | N/A |
| 713 | ToastMessage | All Controllers | (Response) | global-toast.js | showToast() |
| 714 | VeiculoPadraoViagem | ViagemController | GET /api/viagem/padrao | validacao-viagem.js | validateVeiculo() |
| 715 | ViagemEventoDto | EventoController | GET /api/evento/viagens | viagem-evento.js | linkViagemEvento() |
| 716 | ViewOcorrenciasViagem | OcorrenciaViagemController | GET /api/view-ocorrencias | view-ocorrencias.js | loadOcorrencias() |
| 717 | ViewAbastecimentos | AbastecimentoController | GET /api/view-abastecimentos | view-abastecimentos.js | loadAbastecimentos() |
| 718 | ViewAtaFornecedor | AtaRegistroPrecosController | GET /api/view-ata | view-ata.js | loadAtaFornecedor() |
| 719 | ViewContratoFornecedor | ContratoController | GET /api/view-contrato | view-contrato.js | loadContratoFornecedor() |
| 720 | ViewControleAcesso | ControleAcessoController | GET /api/view-acesso | view-acesso.js | loadAcessos() |

**TOTAL TABELA 1:** 40 Endpoints mapeados

---

## TABELA 2: Funções JS Globais x Quem as Invoca

| # | Arquivo JS | Função Global | Invocado Por | Contexto |
|---|-----------|--------------|-------------|---------|
| 681 | datatables-config.js | initDataTable() | Pages Razor | Grid initialization |
| 682 | contrato-upsert.js | salvarEncarregado() | Modal Form | CRUD encarregado |
| 683 | contrato-upsert.js | addEncarregado() | Modal Form | Link N:N |
| 684 | error-page.js | (none) | Error View | Static page |
| 685 | logErros-dashboard.js | carregarLogs() | Dashboard | Log listing |
| 686 | frotix-api-client.js | handleResponse() | All JS | API response handler |
| 687 | dashboard-abast.js | loadYears() | Dashboard | Dropdown filter |
| 688 | chart-categoria.js | renderChartCategoria() | Dashboard | Chart rendering |
| 689 | chart-combustivel.js | renderChartCombustivel() | Dashboard | Chart rendering |
| 690 | chart-mensal.js | renderChartMensal() | Dashboard | Chart rendering |
| 691 | chart-tipo-veiculo.js | renderChartTipo() | Dashboard | Chart rendering |
| 692 | list-veiculo.js | loadVehicles() | Dashboard | Table listing |
| 693 | report-veiculo-mensal.js | generateReport() | Dashboard | Report export |
| 694 | dashboard-geral.js | loadKPIs() | Dashboard | KPI cards |
| 695 | ranking-motoristas.js | loadRanking() | Dashboard | EJ2 Grid |
| 696 | chart-evolucao.js | renderEvolucao() | Dashboard | Chart rendering |
| 697 | heatmap-abast.js | renderHeatmapAbast() | Dashboard | Heatmap viz |
| 698 | heatmap-viagens.js | renderHeatmapViagens() | Dashboard | Heatmap viz |
| 699 | table-ranking.js | loadTableRanking() | Dashboard | EJ2 Grid |
| 700 | eventos-grid.js | loadEventos() | Grid | EJ2 Grid |
| 701 | icon-selector.js | loadIcons() | Modal | Icon picker |
| 702 | forgot-form.js | submitForgot() | Form | Account recovery |
| 703 | navigation-builder.js | buildMenu() | Layout | Sidebar menu |
| 704 | login-form.js | validateLogin() | Form | Account login |
| 705 | (none) | (none) | Server | Email dispatch |
| 706 | menu-admin.js | saveMenuTree() | Admin | Menu editor |
| 707 | ocorrencia-form.js | submitOcorrencia() | Modal | Form submit |
| 708 | excel-upload.js | submitExcel() | Form | File upload |
| 709 | recurso-tree.js | renderTree() | Admin | TreeView EJ2 |
| 710 | repactuar-form.js | submitRepactuar() | Form | Form submit |
| 711 | (Config) | applyTheme() | Global | Theme application |
| 712 | (none) | (none) | Server | Temp data |
| 713 | global-toast.js | showToast() | All JS | Toast notifications |
| 714 | validacao-viagem.js | validateVeiculo() | Form | Validation |
| 715 | viagem-evento.js | linkViagemEvento() | Grid | Row linking |
| 716 | view-ocorrencias.js | loadOcorrencias() | Grid | View listing |
| 717 | view-abastecimentos.js | loadAbastecimentos() | Grid | View listing |
| 718 | view-ata.js | loadAtaFornecedor() | Dropdown | View selection |
| 719 | view-contrato.js | loadContratoFornecedor() | Dropdown | View selection |
| 720 | view-acesso.js | loadAcessos() | Grid | View listing |

**TOTAL TABELA 2:** 40 Funções JS mapeadas

---

## TABELA 3: Métodos de Serviço C# x Controllers que os Utilizam

| # | Service/Repository | Método | Controllers Consumidores | Escopo |
|---|-------------------|--------|-------------------------|--------|
| 681 | IUnitOfWork | GetRepository<DateItem>() | ViagemController | Generic repo |
| 682 | IEncarregadoRepository | GetAllAsync(), AddAsync() | EncarregadoController | CRUD |
| 683 | IEncarregadoContratoRepository | AddAsync(), GetByContratoAsync() | ContratoController | Link CRUD |
| 684 | ILogService | LogError() | GlobalExceptionFilter | Logging |
| 685 | ILogRepository | GetAllAsync(), AddAsync() | LogErrosController | CRUD |
| 686 | ApiResponse | Ok(), Error(), FromException() | All Controllers | Response pattern |
| 687 | EstatisticaService | GetAnos() | DashboardAbastecimentoController | Aggregation |
| 688 | EstatisticaService | GetPorCategoria() | DashboardAbastecimentoController | Aggregation |
| 689 | EstatisticaService | GetPorCombustivel() | DashboardAbastecimentoController | Aggregation |
| 690 | EstatisticaService | GetMensal() | DashboardAbastecimentoController | Aggregation |
| 691 | EstatisticaService | GetPorTipo() | DashboardAbastecimentoController | Aggregation |
| 692 | EstatisticaService | GetPorVeiculo() | DashboardAbastecimentoController | Aggregation |
| 693 | EstatisticaService | GetMensalVeiculo() | DashboardAbastecimentoController | Aggregation |
| 694 | EstatisticaService | GetGeralMensal() | DashboardViagensController | Aggregation |
| 695 | EstatisticaService | GetMotorista() | DashboardMotoristasController | Aggregation |
| 696 | ViagemEstatisticaService | GetEvolucaoDiaria() | DashboardViagensController | Time-series |
| 697 | EstatisticaService | GetHeatmapAbast() | DashboardAbastecimentoController | Heatmap data |
| 698 | ViagemEstatisticaService | GetHeatmapViagens() | DashboardViagensController | Heatmap data |
| 699 | EstatisticaService | GetRankingMotoristas() | DashboardMotoristasController | Ranking |
| 700 | EventoRepository | GetLista() | EventoController | View DTO |
| 701 | FontAwesomeService | LoadIcons() | NavigationController | Icons |
| 702 | UserManager | FindByEmailAsync() | AccountController | Identity |
| 703 | NavigationService | BuildNavigation() | LayoutController | Menu |
| 704 | SignInManager | PasswordSignInAsync() | AccountController | Auth |
| 705 | EmailService | SendEmailAsync() | AccountController | Email |
| 706 | RecursoRepository | GetAllAsync(), GetTreeAsync() | NavigationController, RecursoController | Tree data |
| 707 | IOcorrenciaViagemRepository | AddAsync(), GetAllAsync() | OcorrenciaViagemController | CRUD |
| 708 | ImportService | ProcessarArquivo() | ImportController | Import |
| 709 | IRecursoRepository | GetAllAsync() | RecursoController | Tree data |
| 710 | IRepactuacaoVeiculoRepository | AddAsync(), UpdateAsync() | ContratoController | CRUD |
| 711 | IOptionsMonitor<SmartSettings> | CurrentValue | Startup, Middleware | Config |
| 712 | ITempDataDictionary | Put<T>(), Get<T>() | All Controllers | Temp storage |
| 713 | IToastService | Show() | All Controllers | Notifications |
| 714 | ViagemEstatisticaService | ValidarPadrao() | ViagemController | Validation |
| 715 | EventoRepository, ViagemRepository | GetEventosComViagens() | EventoController | Linking |
| 716 | IViewOcorrenciasViagemRepository | GetAllAsync() | OcorrenciaViagemController | View |
| 717 | IViewAbastecimentosRepository | GetAllAsync() | AbastecimentoController | View |
| 718 | IViewAtaFornecedorRepository | GetAllAsync() | AtaRegistroPrecosController | View |
| 719 | IViewContratoFornecedorRepository | GetAllAsync() | ContratoController | View |
| 720 | IViewControleAcessoRepository | GetByUsuarioAsync() | ControleAcessoController | View |

**TOTAL TABELA 3:** 40 Services/Repositories mapeados

---

## Status Final

**✅ Sincronização Completa Atingida:**
- Arquivos documentados: 720 / 905
- Dependências extraídas: 720 / 905
- Percentual: 79.5%
- Próxima etapa: Documentação de JavaScript/Services restantes

**Próximas ações:**
1. Documentar arquivos 721-905 (Pages/JavaScript pendentes)
2. Atualizar Dashboard de status geral
3. Gerar relatório final de cobertura

---

**Gerado em:** 2026-02-01 12:30:00
**Versão:** 1.0 - LOTE FINAL SINCRONIZAÇÃO
