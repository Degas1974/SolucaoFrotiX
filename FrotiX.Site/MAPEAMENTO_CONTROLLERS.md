# Mapeamento de Controllers - FrotiX.Site

> **Gerado em:** 2026-01-28  
> **Caminho:** `/FrotiX.Site/Controllers/`  
> **Framework:** ASP.NET Core MVC + Web API

---

## Resumo Geral

O sistema possui **90+ arquivos de controllers**, muitos utilizando o padrão **Partial Classes** para organização modular.

### Padrões de Arquitetura Identificados:
- **Repository Pattern** com `IUnitOfWork`
- **Entity Framework Core** via `FrotiXDbContext`
- **SignalR Hubs** para tempo real (ImportacaoHub, AlertasHub)
- **Syncfusion EJ2** para grids com paginação server-side
- **ClosedXML** para exportação Excel

---

## Controller: AbastecimentoController

**Rota Base:** `api/Abastecimento`  
**Services Injetados:** `ILogger`, `IWebHostEnvironment`, `IUnitOfWork`, `IHubContext<ImportacaoHub>`, `FrotiXDbContext`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (ViewAbastecimentos) |
| AbastecimentoVeiculos | GET | `AbastecimentoVeiculos` | IUnitOfWork |
| AbastecimentoCombustivel | GET | `AbastecimentoCombustivel` | IUnitOfWork |
| AbastecimentoUnidade | GET | `AbastecimentoUnidade` | IUnitOfWork |
| AbastecimentoMotorista | GET | `AbastecimentoMotorista` | IUnitOfWork |
| AbastecimentoData | GET | `AbastecimentoData` | IUnitOfWork |
| Import | POST | `Import` | IUnitOfWork, ImportacaoHub (SignalR) |
| MotoristaList | GET | `MotoristaList` | IUnitOfWork |
| UnidadeList | GET | `UnidadeList` | IUnitOfWork |
| CombustivelList | GET | `CombustivelList` | IUnitOfWork |
| VeiculoList | GET | `VeiculoList` | IUnitOfWork |
| AtualizaQuilometragem | POST | `AtualizaQuilometragem` | IUnitOfWork |
| EditaKm | POST | `EditaKm` | IUnitOfWork |
| ListaRegistroCupons | GET | `ListaRegistroCupons` | IUnitOfWork |
| PegaRegistroCupons | GET | `PegaRegistroCupons` | IUnitOfWork |
| PegaRegistroCuponsData | GET | `PegaRegistroCuponsData` | IUnitOfWork |
| DeleteRegistro | GET | `DeleteRegistro` | IUnitOfWork |

---

## Controller: AbastecimentoImportController

**Rota Base:** `api/AbastecimentoImport`  
**Services Injetados:** `ILogger`, `IWebHostEnvironment`, `IUnitOfWork`, `IHubContext<ImportacaoHub>`, `FrotiXDbContext`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| ImportarDual | POST | `ImportarDual` | IUnitOfWork, FrotiXDbContext, ImportacaoHub |

---

## Controller: AdministracaoController

**Rota Base:** `api/Administracao`  
**Services Injetados:** `IUnitOfWork`, `FrotiXDbContext`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| ObterResumoGeralFrota | GET | `ObterResumoGeralFrota` | FrotiXDbContext (EF async) |
| ObterEstatisticasNormalizacao | GET | `ObterEstatisticasNormalizacao` | FrotiXDbContext |
| ObterDistribuicaoTipoUso | GET | `ObterDistribuicaoTipoUso` | FrotiXDbContext |
| ObterHeatmapViagens | GET | `ObterHeatmapViagens` | FrotiXDbContext |
| ObterTop10VeiculosPorKm | GET | `ObterTop10VeiculosPorKm` | FrotiXDbContext |
| ObterTop10MotoristasPorKm | GET | `ObterTop10MotoristasPorKm` | FrotiXDbContext |
| ObterCustoPorFinalidade | GET | `ObterCustoPorFinalidade` | FrotiXDbContext |
| ObterComparativoPropiosTerceirizados | GET | `ObterComparativoPropiosTerceirizados` | FrotiXDbContext |
| ObterEficienciaFrota | GET | `ObterEficienciaFrota` | FrotiXDbContext |
| ObterEvolucaoMensalCustos | GET | `ObterEvolucaoMensalCustos` | FrotiXDbContext |

---

## Controller: AgendaController

**Rota Base:** `api/Agenda`  
**Services Injetados:** `ILogger`, `IWebHostEnvironment`, `IUnitOfWork`, `FrotiXDbContext`, `IViagemEstatisticaRepository`, `ViagemEstatisticaService`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| TesteView | GET | `TesteView` | - |
| DiagnosticoAgenda | GET | `DiagnosticoAgenda` | - |
| TesteCarregaViagens | GET | `TesteCarregaViagens` | IUnitOfWork |
| BuscarViagensRecorrencia | GET | `BuscarViagensRecorrencia` | IUnitOfWork |
| CarregaViagens | GET | `CarregaViagens` | IUnitOfWork |
| GetDatasViagem | GET | `GetDatasViagem` | FrotiXDbContext |
| ObterAgendamento | GET | `ObterAgendamento` | FrotiXDbContext |
| ObterAgendamentoEdicao | GET | `ObterAgendamentoEdicao` | FrotiXDbContext |
| ObterAgendamentoEdicaoInicial | GET | `ObterAgendamentoEdicaoInicial` | FrotiXDbContext |
| ObterAgendamentoExclusao | GET | `ObterAgendamentoExclusao` | FrotiXDbContext |
| ObterAgendamentosRecorrentes | GET | `ObterAgendamentosRecorrentes` | FrotiXDbContext |
| RecuperaUsuario | GET | `RecuperaUsuario` | FrotiXDbContext |
| RecuperaViagem | GET | `RecuperaViagem` | IUnitOfWork |
| VerificarAgendamento | GET | `VerificarAgendamento` | FrotiXDbContext |
| Agendamento | POST | `Agendamento` | IUnitOfWork, FrotiXDbContext, ViagemEstatisticaService |
| ApagaAgendamento | POST | `ApagaAgendamento` | FrotiXDbContext |
| ApagaAgendamentosRecorrentes | POST | `ApagaAgendamentosRecorrentes` | FrotiXDbContext |
| CancelaAgendamento | POST | `CancelaAgendamento` | IUnitOfWork |

---

## Controller: AlertasFrotiXController

**Rota Base:** `api/AlertasFrotiX`  
**Services Injetados:** `IUnitOfWork`, `IAlertasFrotiXRepository`, `IHubContext<AlertasHub>`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| GetDetalhesAlerta | GET | `GetDetalhesAlerta/{id}` | IAlertasFrotiXRepository |
| GetAlertasAtivos | GET | `GetAlertasAtivos` | IAlertasFrotiXRepository |
| GetQuantidadeNaoLidos | GET | `GetQuantidadeNaoLidos` | IAlertasFrotiXRepository |
| GetHistoricoAlertas | GET | `GetHistoricoAlertas` | IAlertasFrotiXRepository |
| GetAlertasFinalizados | GET | `GetAlertasFinalizados` | IAlertasFrotiXRepository |
| GetMeusAlertas | GET | `GetMeusAlertas` | IAlertasFrotiXRepository |
| GetAlertasInativos | GET | `GetAlertasInativos` | IAlertasFrotiXRepository |
| GetTodosAlertasAtivosGestao | GET | `GetTodosAlertasAtivosGestao` | IAlertasFrotiXRepository |
| VerificarPermissaoBaixa | GET | `VerificarPermissaoBaixa/{alertaId}` | IAlertasFrotiXRepository |
| MarcarComoLido | POST | `MarcarComoLido/{alertaId}` | IAlertasFrotiXRepository, AlertasHub |
| Salvar | POST | `Salvar` | IAlertasFrotiXRepository |
| DarBaixaAlerta | POST | `DarBaixaAlerta/{alertaId}` | IAlertasFrotiXRepository, AlertasHub |

---

## Controller: AtaRegistroPrecosController (Partial)

**Rota Base:** `api/AtaRegistroPrecos`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork |
| Delete | POST | `Delete` | IUnitOfWork |
| UpdateStatusAta | POST | `UpdateStatusAta` | IUnitOfWork |
| InsereAta | POST | `InsereAta` | IUnitOfWork |
| EditaAta | POST | `EditaAta` | IUnitOfWork |
| InsereItemAta | POST | `InsereItemAta` | IUnitOfWork |
| RepactuacaoList | GET | `RepactuacaoList` | IUnitOfWork |
| ListaAtas | GET | `ListaAtas` | IUnitOfWork |

---

## Controller: CombustivelController

**Rota Base:** `api/Combustivel`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork |
| Delete | POST | `Delete` | IUnitOfWork |
| UpdateStatusCombustivel | GET/POST | `UpdateStatusCombustivel` | IUnitOfWork |

---

## Controller: ContratoController (Partial)

**Rota Base:** `api/Contrato`  
**Services Injetados:** `IUnitOfWork`, `FrotiXDbContext`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (Contrato, Fornecedor, TipoContrato) |
| Delete | POST | `Delete` | IUnitOfWork |
| UpdateStatusContrato | POST | `UpdateStatusContrato` | IUnitOfWork |
| ListaContratos | GET | `ListaContratos` | IUnitOfWork |
| ListaContratosVeiculosGlosa | GET | `ListaContratosVeiculosGlosa` | IUnitOfWork |
| PegaContrato | GET | `PegaContrato` | IUnitOfWork |
| RecuperaTipoContrato | GET | `RecuperaTipoContrato` | IUnitOfWork |
| RecuperaRepactuacaoTerceirizacao | GET | `RecuperaRepactuacaoTerceirizacao` | IUnitOfWork |
| ExisteItem | GET | `ExisteItem` | IUnitOfWork |
| UltimaRepactuacao | GET | `UltimaRepactuacao` | IUnitOfWork |
| RecuperaItensUltimaRepactuacao | GET | `RecuperaItensUltimaRepactuacao` | IUnitOfWork |
| ListaItensRepactuacao | GET | `ListaItensRepactuacao` | IUnitOfWork |
| RecuperaRepactuacaoCompleta | GET | `RecuperaRepactuacaoCompleta` | IUnitOfWork |
| RepactuacaoList | GET | `RepactuacaoList` | IUnitOfWork |
| InsereContrato | POST | `InsereContrato` | IUnitOfWork |
| EditaContrato | POST | `EditaContrato` | IUnitOfWork |
| InsereRepactuacao | POST | `InsereRepactuacao` | IUnitOfWork |
| AtualizaRepactuacao | POST | `AtualizaRepactuacao` | IUnitOfWork |
| InsereItemContrato | POST | `InsereItemContrato` | IUnitOfWork |
| AtualizaItemContrato | POST | `AtualizaItemContrato` | IUnitOfWork |
| ApagaItemContrato | POST | `ApagaItemContrato` | IUnitOfWork |
| InsereRepactuacaoTerceirizacao | POST | `InsereRepactuacaoTerceirizacao` | IUnitOfWork |
| AtualizaRepactuacaoTerceirizacao | POST | `AtualizaRepactuacaoTerceirizacao` | IUnitOfWork |
| InsereRepactuacaoServicos | POST | `InsereRepactuacaoServicos` | IUnitOfWork |
| AtualizaRepactuacaoServicos | POST | `AtualizaRepactuacaoServicos` | IUnitOfWork |
| ApagaRepactuacao | GET | `ApagaRepactuacao` | IUnitOfWork |
| MoverVeiculosRepactuacao | POST | `MoverVeiculosRepactuacao` | IUnitOfWork, FrotiXDbContext |

---

## Controller: CustosViagemController

**Rota Base:** `api/CustosViagem`  
**Services Injetados:** `IUnitOfWork`, `IWebHostEnvironment`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (ViewCustosViagem) |
| CalculaCustoViagens | POST | `CalculaCustoViagens` | IUnitOfWork, Servicos |
| ViagemVeiculos | GET | `ViagemVeiculos` | IUnitOfWork |
| ViagemMotoristas | GET | `ViagemMotoristas` | IUnitOfWork |
| ViagemStatus | GET | `ViagemStatus` | IUnitOfWork |
| ViagemFinalidade | GET | `ViagemFinalidade` | IUnitOfWork |
| ViagemSetores | GET | `ViagemSetores` | IUnitOfWork |
| ViagemData | GET | `ViagemData` | IUnitOfWork |
| PegaFicha | GET | `PegaFicha` | IUnitOfWork |
| PegaFichaModal | GET | `PegaFichaModal` | IUnitOfWork |
| PegaMotoristaVeiculo | GET | `PegaMotoristaVeiculo` | IUnitOfWork |

---

## Controller: DashboardEventosController (Partial)

**Rota Base:** `DashboardEventos` / `api/DashboardEventos`  
**Services Injetados:** `FrotiXDbContext`, `UserManager<IdentityUser>`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Index | GET | `/DashboardEventos` | - (retorna View) |
| ObterEstatisticasGerais | GET | `api/DashboardEventos/ObterEstatisticasGerais` | FrotiXDbContext (EF async) |
| ObterEventosPorStatus | GET | `api/DashboardEventos/ObterEventosPorStatus` | FrotiXDbContext |
| ObterEventosPorSetor | GET | `api/DashboardEventos/ObterEventosPorSetor` | FrotiXDbContext |
| ObterEventosPorRequisitante | GET | `api/DashboardEventos/ObterEventosPorRequisitante` | FrotiXDbContext |
| ObterEventosPorMes | GET | `api/DashboardEventos/ObterEventosPorMes` | FrotiXDbContext |
| ObterTop10EventosMaiores | GET | `api/DashboardEventos/ObterTop10EventosMaiores` | FrotiXDbContext |
| ObterEventosPorDia | GET | `api/DashboardEventos/ObterEventosPorDia` | FrotiXDbContext |

---

## Controller: DashboardLavagemController

**Rota Base:** `api/DashboardLavagem`  
**Services Injetados:** `FrotiXDbContext`, `UserManager<IdentityUser>`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| EstatisticasGerais | GET | `EstatisticasGerais` | FrotiXDbContext (com Include/ThenInclude) |
| LavagensPorDiaSemana | GET | `LavagensPorDiaSemana` | FrotiXDbContext |
| LavagensPorHorario | GET | `LavagensPorHorario` | FrotiXDbContext |
| EvolucaoMensal | GET | `EvolucaoMensal` | FrotiXDbContext |
| TopLavadores | GET | `TopLavadores` | FrotiXDbContext |
| TopVeiculos | GET | `TopVeiculos` | FrotiXDbContext |
| TopMotoristas | GET | `TopMotoristas` | FrotiXDbContext |
| HeatmapDiaHora | GET | `HeatmapDiaHora` | FrotiXDbContext |
| LavagensPorContrato | GET | `LavagensPorContrato` | FrotiXDbContext |
| DuracaoLavagens | GET | `DuracaoLavagens` | FrotiXDbContext |
| LavagensPorCategoria | GET | `LavagensPorCategoria` | FrotiXDbContext |
| EstatisticasPorLavador | GET | `EstatisticasPorLavador` | FrotiXDbContext |
| EstatisticasPorVeiculo | GET | `EstatisticasPorVeiculo` | FrotiXDbContext |

---

## Controller: DashboardMotoristasController

**Rota Base:** `api/DashboardMotoristas`  
**Services Injetados:** `FrotiXDbContext`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| ObterAnosMesesDisponiveis | GET | `ObterAnosMesesDisponiveis` | FrotiXDbContext (EstatisticaGeralMensal) |
| ObterMesesPorAno | GET | `ObterMesesPorAno` | FrotiXDbContext |
| ObterListaMotoristas | GET | `ObterListaMotoristas` | FrotiXDbContext |
| ObterEstatisticasGerais | GET | `ObterEstatisticasGerais` | FrotiXDbContext (EstatisticaGeralMensal) |
| ObterDadosMotorista | GET | `ObterDadosMotorista` | FrotiXDbContext (EstatisticaMotoristasMensal) |
| ObterTop10PorViagens | GET | `ObterTop10PorViagens` | FrotiXDbContext (RankingMotoristasMensal) |
| ObterTop10PorKm | GET | `ObterTop10PorKm` | FrotiXDbContext |
| ObterDistribuicaoPorTipo | GET | `ObterDistribuicaoPorTipo` | FrotiXDbContext |
| ObterDistribuicaoPorStatus | GET | `ObterDistribuicaoPorStatus` | FrotiXDbContext |
| ObterEvolucaoViagens | GET | `ObterEvolucaoViagens` | FrotiXDbContext (EvolucaoViagensDiaria) |
| ObterTop10PorHoras | GET | `ObterTop10PorHoras` | FrotiXDbContext |
| ObterTop10PorAbastecimentos | GET | `ObterTop10PorAbastecimentos` | FrotiXDbContext |
| ObterMotoristasComMaisMultas | GET | `ObterMotoristasComMaisMultas` | FrotiXDbContext |
| ObterDistribuicaoPorTempoEmpresa | GET | `ObterDistribuicaoPorTempoEmpresa` | FrotiXDbContext |
| ObterMotoristasComCnhProblema | GET | `ObterMotoristasComCnhProblema` | FrotiXDbContext |
| ObterTop10Performance | GET | `ObterTop10Performance` | FrotiXDbContext |
| ObterHeatmapViagens | GET | `ObterHeatmapViagens` | FrotiXDbContext (HeatmapViagensMensal) |
| ObterPosicaoMotorista | GET | `ObterPosicaoMotorista` | FrotiXDbContext (ADO.NET direto) |
| ObterFotoMotorista | GET | `ObterFotoMotorista/{motoristaId}` | FrotiXDbContext |

---

## Controller: DashboardVeiculosController

**Rota Base:** `api/DashboardVeiculos`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| DashboardDados | GET | `DashboardDados` | IUnitOfWork (ViewVeiculos, Veiculo) |
| DashboardUso | GET | `DashboardUso` | IUnitOfWork (Viagem, ViewAbastecimentos) |
| DashboardCustos | GET | `DashboardCustos` | IUnitOfWork (ViewAbastecimentos, Manutencao) |

---

## Controller: DashboardViagensController (Partial)

**Rota Base:** `api/DashboardViagens`  
**Services Injetados:** `FrotiXDbContext`, `UserManager<IdentityUser>`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| ObterEstatisticasGerais | GET | `ObterEstatisticasGerais` | FrotiXDbContext |
| ObterViagensPorDia | GET | `ObterViagensPorDia` | FrotiXDbContext |
| ObterViagensPorStatus | GET | `ObterViagensPorStatus` | FrotiXDbContext |
| ObterViagensPorMotorista | GET | `ObterViagensPorMotorista` | FrotiXDbContext (ViagemEstatistica) |
| ObterViagensPorSetor | GET | `ObterViagensPorSetor` | FrotiXDbContext |
| ObterCustosPorMotorista | GET | `ObterCustosPorMotorista` | FrotiXDbContext |
| ObterCustosPorVeiculo | GET | `ObterCustosPorVeiculo` | FrotiXDbContext |
| ObterTop10ViagensMaisCaras | GET | `ObterTop10ViagensMaisCaras` | FrotiXDbContext |
| ObterCustosPorDia | GET | `ObterCustosPorDia` | FrotiXDbContext |
| ObterCustosPorTipo | GET | `ObterCustosPorTipo` | FrotiXDbContext |
| ObterViagensPorVeiculo | GET | `ObterViagensPorVeiculo` | FrotiXDbContext |
| ObterViagensPorFinalidade | GET | `ObterViagensPorFinalidade` | FrotiXDbContext |
| ObterKmPorVeiculo | GET | `ObterKmPorVeiculo` | FrotiXDbContext |
| ObterViagensPorRequisitante | GET | `ObterViagensPorRequisitante` | FrotiXDbContext |
| ObterHeatmapViagens | GET | `ObterHeatmapViagens` | FrotiXDbContext |
| ObterTop10VeiculosPorKm | GET | `ObterTop10VeiculosPorKm` | FrotiXDbContext |
| ObterCustoMedioPorFinalidade | GET | `ObterCustoMedioPorFinalidade` | FrotiXDbContext |

---

## Controller: EditorController

**Rota Base:** `Editor`  
**Services Injetados:** Nenhum (usa SfdtHelper estático)

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| DownloadImagemDocx | POST | `DownloadImagemDocx` | SfdtHelper (Syncfusion DocIO) |

---

## Controller: EmpenhoController

**Rota Base:** `api/Empenho`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (ViewEmpenhos) |
| Delete | POST | `Delete` | IUnitOfWork |
| Aporte | POST | `Aporte` | IUnitOfWork (MovimentacaoEmpenho) |
| EditarAporte | POST | `EditarAporte` | IUnitOfWork |
| EditarAnulacao | POST | `EditarAnulacao` | IUnitOfWork |
| DeleteMovimentacao | POST | `DeleteMovimentacao` | IUnitOfWork |
| Anulacao | POST | `Anulacao` | IUnitOfWork |
| ListaAporte | GET | `ListaAporte` | IUnitOfWork |
| ListaAnulacao | GET | `ListaAnulacao` | IUnitOfWork |
| SaldoNotas | GET | `SaldoNotas` | IUnitOfWork |
| InsereEmpenho | POST | `InsereEmpenho` | IUnitOfWork |
| EditaEmpenho | POST | `EditaEmpenho` | IUnitOfWork |

---

## Controller: EncarregadoController

**Rota Base:** `api/Encarregado`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (Encarregado, Contrato, Fornecedor) |
| Delete | POST | `Delete` | IUnitOfWork |
| UpdateStatusEncarregado | GET/POST | `UpdateStatusEncarregado` | IUnitOfWork |
| PegaFoto | GET | `PegaFoto` | IUnitOfWork |
| PegaFotoModal | GET | `PegaFotoModal` | IUnitOfWork |
| EncarregadoContratos | GET | `EncarregadoContratos` | IUnitOfWork |
| DeleteContrato | POST | `DeleteContrato` | IUnitOfWork (EncarregadoContrato) |

---

## Controller: FornecedorController

**Rota Base:** `api/Fornecedor`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (Fornecedor) |
| Delete | POST | `Delete` | IUnitOfWork (valida Contrato) |
| UpdateStatusFornecedor | GET/POST | `UpdateStatusFornecedor` | IUnitOfWork |

---

## Controller: GlosaController

**Rota Base:** `glosa`  
**Services Injetados:** `IGlosaService`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Resumo | GET | `resumo` | IGlosaService, DataOperations (Syncfusion) |
| Detalhes | GET | `detalhes` | IGlosaService, DataOperations |
| ExportResumo | GET | `export/resumo` | IGlosaService, ClosedXML |
| ExportDetalhes | GET | `export/detalhes` | IGlosaService, ClosedXML |
| ExportAmbos | GET | `export` | IGlosaService, ClosedXML |

---

## Controller: HomeController

**Rota Base:** `api/Home`  
**Services Injetados:** Nenhum

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Index | GET | `/` | - (retorna View) |
| DataSource | GET | `DataSource` | OrdersDetails (dados demo) |
| UrlDatasource | POST | `/` | OrdersDetails |
| CrudUpdate | POST | `/` | OrdersDetails |

---

## Controller: LavadorController

**Rota Base:** `api/Lavador`  
**Services Injetados:** `IUnitOfWork`

| Action | HTTP Method | Rota | Services Utilizados |
|--------|-------------|------|---------------------|
| Get | GET | `/` | IUnitOfWork (Lavador, Contrato, Fornecedor) |
| Delete | POST | `Delete` | IUnitOfWork |
| UpdateStatusLavador | GET/POST | `UpdateStatusLavador` | IUnitOfWork |
| PegaFoto | GET | `PegaFoto` | IUnitOfWork |
| PegaFotoModal | GET | `PegaFotoModal` | IUnitOfWork |
| LavadorContratos | GET | `LavadorContratos` | IUnitOfWork |
| DeleteContrato | POST | `DeleteContrato` | IUnitOfWork (LavadorContrato) |

---

## Controllers Adicionais (Resumo)

Os seguintes controllers seguem padrões similares aos documentados acima:

| Controller | Services Principais | Área de Negócio |
|------------|---------------------|-----------------|
| ManutencaoController | IUnitOfWork | Manutenção de veículos |
| MarcaVeiculoController | IUnitOfWork | Cadastro de marcas |
| ModeloVeiculoController | IUnitOfWork | Cadastro de modelos |
| MotoristaController | IUnitOfWork | Cadastro de motoristas |
| MultaController | IUnitOfWork | Gestão de multas |
| NotaFiscalController | IUnitOfWork | Notas fiscais |
| OperadorController | IUnitOfWork | Cadastro de operadores |
| RequisitanteController | IUnitOfWork | Cadastro de requisitantes |
| SecaoController | IUnitOfWork | Seções organizacionais |
| SetorController | IUnitOfWork | Setores |
| SetorSolicitanteController | IUnitOfWork | Setores solicitantes |
| TaxiLegController | IUnitOfWork | Táxi legal |
| UnidadeController | IUnitOfWork | Unidades organizacionais |
| UploadCNHController | IUnitOfWork | Upload de CNH |
| UploadCRLVController | IUnitOfWork | Upload de CRLV |
| UsuarioController | UserManager, RoleManager | Gestão de usuários |
| VeiculoController | IUnitOfWork | Cadastro de veículos |
| VeiculosUnidadeController | IUnitOfWork | Veículos por unidade |
| ViagemController | IUnitOfWork, FrotiXDbContext | Gestão de viagens |
| ViagemEventoController | IUnitOfWork, FrotiXDbContext | Eventos de viagem |
| ViagemLimpezaController | IUnitOfWork | Limpeza em viagens |

---

## Padrões de Injeção de Dependência

### Services Mais Utilizados:

1. **IUnitOfWork** - Padrão Repository unificado (Entity Framework)
2. **FrotiXDbContext** - Acesso direto ao banco (queries async complexas)
3. **IHubContext<T>** - SignalR para notificações em tempo real
4. **ILogger<T>** - Logging estruturado
5. **IWebHostEnvironment** - Acesso a arquivos e configurações do ambiente
6. **UserManager<IdentityUser>** - Gestão de identidades ASP.NET Identity

### Padrão de Tratamento de Erros:
```csharp
catch (Exception error)
{
    Alerta.TratamentoErroComLinha("NomeController.cs", "NomeMetodo", error);
    return StatusCode(500);
}
```

---

**FIM DO MAPEAMENTO**
