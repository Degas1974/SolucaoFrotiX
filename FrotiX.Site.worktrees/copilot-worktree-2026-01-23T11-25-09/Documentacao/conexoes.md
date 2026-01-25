# Mapa de Conexões e Dependências do Sistema

Este arquivo mapeia as interações entre arquivos (Controllers, Views, JS, Services), registrando quem chama quem.

## Legenda

- `->`: Chama / Invoca
- `<-`: É chamado por

---

## Controllers

### AbastecimentoController.cs

- **Chama (`_unitOfWork`):**
  - `ViewAbastecimentos` (Leitura otimizada)
  - `Abastecimento` (Gravação)
  - `Veiculo` (Lookup)
  - `Motorista` (Lookup)
  - `Combustivel` (Lookup)
  - `Unidade` (Lookup)
- **Chama (Libs Externas):**
  - `NPOI` (Processamento Excel)
  - `System.Transactions` (TransactionScope)
- **Chama (Outros):**
  - `IHubContext<ImportacaoHub>` (SignalR)
  - `Alerta.TratamentoErroComLinha` (Helper de Erro)

### AdministracaoController.cs

- **Chama (`_context`):**
  - `Veiculo` (Contagem, Status, TipoUso)
  - `Motorista` (Contagem, Validação)
  - `Viagem` (Estatísticas, Ranking, Evolução, Custos)
  - `VeiculoPadraoViagem` (Distribuição por Tipo de Uso)
  - `ViewVeiculos` (Dados de desempenho enrichados)
- **Chama (Outros):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)
- **Servido por:**
  - `api/Administracao/ObterResumoGeralFrota`
  - `api/Administracao/ObterEstatisticasNormalizacao`
  - `api/Administracao/ObterDistribuicaoTipoUso`
  - `api/Administracao/ObterHeatmapViagens`
  - `api/Administracao/ObterTop10VeiculosPorKm`
  - `api/Administracao/ObterTop10MotoristasPorKm`
  - `api/Administracao/ObterCustoPorFinalidade`
  - `api/Administracao/ObterEficienciaFrota`
  - `api/Administracao/ObterEvolucaoMensalCustos`

### AlertasFrotiXController.cs

- **Chama (`_unitOfWork`):**
  - `AlertasFrotiX` (Obtenção e gerenciamento de alertas)
  - `AlertasUsuario` (Controle de leitura e exclusão)
  - `AspNetUsers` (Obtenção de destinatários)
- **Chama (`_hubContext`):**
  - `AlertasHub` (Envio de notificações em tempo real)
- **Chama (Outros):**
  - `Alerta` (Helper de erro e formatação visual)
- **Servido por:**
  - `api/AlertasFrotiX/GetDetalhesAlerta/{id}`
  - `api/AlertasFrotiX/GetAlertasAtivos`
  - `api/AlertasFrotiX/GetQuantidadeNaoLidos`
  - `api/AlertasFrotiX/MarcarComoLido/{alertaId}`
  - `api/AlertasFrotiX/Salvar`

### Api/DocGeneratorController.cs

- **Chama (`Services`):**
  - `IFileDiscoveryService` (Varredura de arquivos)
  - `IDocGeneratorOrchestrator` (Gestão de Jobs de documentação)
  - `IDocCacheService` (Gestão de cache)
- **Servido por:**
  - `api/DocGenerator/discover`
  - `api/DocGenerator/tree`
  - `api/DocGenerator/generate`
  - `api/DocGenerator/job/{jobId}`
  - `api/DocGenerator/providers`

### Api/WhatsAppController.cs

- **Chama (`Services`):**
  - `IWhatsAppService` (Interface de comunicação com API externa de WhatsApp)
- **Servido por:**
  - `api/WhatsApp/start`
  - `api/WhatsApp/status`
  - `api/WhatsApp/qr`
  - `api/WhatsApp/send-text`
  - `api/WhatsApp/send-media`

### AtaRegistroPrecosController.cs

- **Chama (`_unitOfWork`):**
  - `AtaRegistroPrecos` (CRUD principal)
  - `Fornecedor` (Vínculo da ata)
  - `RepactuacaoAta` (Gestão de revisões de preço/vigência)
  - `ItemVeiculoAta` (Itens associados a veículos na ata)
  - `VeiculoAta` (Vínculo direto de veículos com a ata)
- **Servido por:**
  - `api/AtaRegistroPrecos` (GET, DELETE)
  - `api/AtaRegistroPrecos/UpdateStatusAta`
  - `api/AtaRegistroPrecos/InsereAta`
  - `api/AtaRegistroPrecos/EditaAta`
  - `api/AtaRegistroPrecos/InsereItemAta`
  - `api/AtaRegistroPrecos/RepactuacaoList`
  - `api/AtaRegistroPrecos/ListaAtas`

### ContratoController.cs (e Partials)

- **Chama (`_unitOfWork`):**
  - `Contrato` (CRUD e Gestão)
  - `Fornecedor` (Dados do contratado)
  - `RepactuacaoContrato` (Aditivos e revisões)
  - `ItemVeiculoContrato` (Itens do contrato)
  - `VeiculoContrato` (Vínculos com veículos)
  - `Encarregado`, `Operador`, `Lavador`, `Motorista`, `Empenho`, `NotaFiscal` (Dependências e validações)
- **Servido por:**
  - `api/Contrato` (GET, DELETE)
  - `api/Contrato/UpdateStatusContrato`
  - `api/Contrato/ListaContratos` (Select2)
  - `api/Contrato/ListaContratosVeiculosGlosa`
  - `api/Contrato/PegaContrato`
  - `api/Contrato/InsereContrato`
  - `api/Contrato/EditaContrato`
  - `api/Contrato/InsereRepactuacao`, `AtualizaRepactuacao`
  - `api/Contrato/InsereItemContrato`, `AtualizaItemContrato`, `ApagaItemContrato`
  - `api/Contrato/ListaContratosPorStatus` (Partial)
  - `api/Contrato/VerificarDependencias` (Partial)

### CustosViagemController.cs

- **Chama (`_unitOfWork`):**
  - `ViewCustosViagem` (Consulta otimizada de custos)
  - `Viagem` (Recálculo e Atualização)
- **Chama (`Serviços`):**
  - `Servicos.CalculaCustoMotorista`
  - `Servicos.CalculaCustoVeiculo`
  - `Servicos.CalculaCustoCombustivel`
- **Servido por:**
  - `api/CustosViagem` (GET)
  - `api/CustosViagem/CalculaCustoViagens`
  - `api/CustosViagem/ViagemVeiculos`
  - `api/CustosViagem/ViagemMotoristas`
  - `api/CustosViagem/ViagemStatus`
  - `api/CustosViagem/ViagemData`
  - `api/CustosViagem/PegaFicha`

### DashboardEventosController.cs

- **Chama (`_context` e `_userManager`):**
  - `Evento` (Entidade Principal)
  - `SetorSolicitante`, `Requisitante` (Relacionamentos)
  - `IdentityUser` (Autenticação)
- **Servido por:**
  - `api/DashboardEventos/ObterEstatisticasGerais`
  - `api/DashboardEventos/ObterEventosPorStatus`
  - `api/DashboardEventos/ObterEventosPorSetor`
  - `api/DashboardEventos/ObterEvolucaoEventos`
  - `api/DashboardEventos/ObterTop10Requisitantes`
  - `ExportarParaPDF` (Partial)

### DashboardLavagemController.cs

- **Chama (`_context` e `_userManager`):**
  - `Lavagem` (Entidade Principal)
  - `LavadoresLavagem` (Tabela de vínculo N:N com lavadores)
  - `Lavador` (Funcionários)
  - `Veiculo` (Objeto do serviço)
- **Servido por:**
  - `api/DashboardLavagem/EstatisticasGerais`
  - `api/DashboardLavagem/ObterEvolucaoDiaria`
  - `api/DashboardLavagem/ObterRankingLavadores`
  - `api/DashboardLavagem/ObterRankingVeiculos`
  - `api/DashboardLavagem/ObterTiposLavagem`

### DashboardMotoristasController.cs

- **Chama (`_context`):**
  - `EstatisticaGeralMensal` (Estatísticas Agregadas)
  - `EstatisticaMotoristasMensal` (Estatísticas Individuais)
  - `RankingMotoristasMensal` (Rankings Pré-calculados)
  - `EvolucaoViagensDiaria` (Gráficos Temporais)
  - `Viagem` (Fallback / Dados reais)
  - `Motorista` (Dados cadastrais / Status)
  - `Abastecimento` (Contagem e Consumo)
  - `Multa` (Infrações e Valores)
- **Chama (Helpers):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)

### DashboardVeiculosController.cs

- **Chama (`_unitOfWork`):**
  - `ViewVeiculos` (Dados principais)
  - `Veiculo` (Detalhes e cadastro)
  - `Viagem` (Quilometragem e uso)
  - `ViewAbastecimentos` (Combustível e custos)
  - `Manutencao` (Custos de manutenção)
- **Servido por:**
  - `api/DashboardVeiculos/DashboardDados`
  - `api/DashboardVeiculos/DashboardUso`
  - `api/DashboardVeiculos/DashboardCustos`

### DashboardViagensController.cs (e Partials)

- **Chama (`_context`):**
  - `Viagem` (Dados principais)
  - `ViagemEstatistica` (Dados agregados)
  - `SetorSolicitante` (Dados de setor)
  - `Veiculo`, `Motorista`, `ModeloVeiculo` (Relacionamentos)
- **Servido por:**
  - `api/DashboardViagens/ObterEstatisticasGerais`
  - `api/DashboardViagens/ObterViagensPorDia`
  - `api/DashboardViagens/ObterViagensPorStatus`
  - `api/DashboardViagens/ObterViagensPorMotorista`
  - `api/DashboardViagens/ObterViagensPorSetor`
  - `api/DashboardViagens/ObterCustosPorMotorista`
  - `api/DashboardViagens/ObterCustosPorVeiculo`
  - `api/DashboardViagens/ObterTop10ViagensMaisCaras`
  - `api/DashboardViagens/ObterCustosPorDia`
  - `api/DashboardViagens/ObterCustosPorTipo`
  - `api/DashboardViagens/ObterViagensPorVeiculo`
  - `api/DashboardViagens/ObterViagensPorFinalidade`
  - `api/DashboardViagens/ObterKmPorVeiculo`
  - `api/DashboardViagens/ExportarParaPDF` (GET/POST)

### EditorController.cs

- **Chama (Helpers):**
  - `SfdtHelper` (Processamento de documentos)
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)
- **Servido por:**
  - `Editor/DownloadImagemDocx`

### EmpenhoController.cs

- **Chama (`_unitOfWork`):**
  - `ViewEmpenhos` (Dados otimizados de empenho)
- **Servido por:**
  - `api/Empenho` (GET)

### EncarregadoController.cs

- **Chama (`_unitOfWork`):**
  - `Encarregado` (Entidade Principal)
  - `Contrato` (Vínculo contratual)
  - `Fornecedor` (Empresa responsável)
  - `AspNetUsers` (Rastreabilidade de usuário)
- **Servido por:**
  - `api/Encarregado` (GET)

### EscalaController.cs (e Partials)

- **Chama (`_unitOfWork`):**
  - `EscalaDiaria` (CRUD Principal)
  - `ViewEscalasCompletas` (Leitura otimizada)
  - `Motorista`, `Veiculo`, `TipoServico`, `Turno`, `Requisitante` (Lookups)
  - `FolgaRecesso`, `CoberturaFolga` (Gestão de disponibilidade)
- **Chama (`_hubContext`):**
  - `EscalaHub` (Atualização em tempo real)
- **Servido por:**
  - `api/Escala/ListaEscalasServerSide`
  - `Escala/Index` (View)
  - `Escala/Create`, `Edit`, `Upsert`

### FornecedorController.cs

- **Chama (`_unitOfWork`):**
  - `Fornecedor` (CRUD principal)
  - `Contrato` (Validação de exclusão)
- **Servido por:**
  - `api/Fornecedor` (GET)
  - `api/Fornecedor/Delete` (POST)

### GlosaController.cs

- **Chama (`_service`):**
  - `IGlosaService` (Lógica de Faturamento)
- **Servido por:**
  - `glosa/resumo` (Syncfusion DataManager)

### GridAtaController.cs

- **Chama (`_unitOfWork`):**
  - `ItemVeiculoAta` (Itens da Ata)
- **Chama (Helpers):**
  - `ItensVeiculoAta` (Classe estática de população)
- **Servido por:**
  - `api/GridAta/DataSourceAta`

### GridContratoController.cs

- **Chama (`_unitOfWork`):**
  - `ItemVeiculoContrato` (Itens do Contrato)
- **Chama (Helpers):**
  - `ItensVeiculo` (Classe estática de população)
- **Servido por:**
  - `api/GridContrato/DataSource`

### HomeController.cs

- **Chama (Mock):**
  - `OrdersDetails` (Dados fictícios para exemplos)
- **Servido por:**
  - `Index` (View Raiz)
  - `api/Home/DataSource` (Teste JSON)
  - `api/Home/UrlDatasource` (Teste Grid Server-Side)

### ItensContratoController.cs

- **Chama (`_unitOfWork`):**
  - `Contrato` (Lista disponível no Dropdown)
  - `AtaRegistroPrecos` (Lista disponível no Dropdown)
  - `Fornecedor` (Descrição visual)
- **Servido por:**
  - `api/ItensContrato/ListaContratos` (Select2)
  - `api/ItensContrato/ListaAtas` (Select2)

### LavadorController.cs

- **Chama (`_unitOfWork`):**
  - `Lavador` (Entidade Principal)
  - `Contrato` (Vínculo contratual)
  - `Fornecedor` (Empresa responsável)
  - `AspNetUsers` (Rastreabilidade)
- **Servido por:**
  - `api/Lavador` (GET)

### LogErrosController.cs

- **Chama (`_logService`):**
  - `ILogService` (Gravação e Leitura de Logs FS/DB)
- **Servido por:**
  - `api/LogErros/LogJavaScript` (Recepção de erro JS)
  - `api/LogErros/ObterLogs` (Dashboard de Erros)

### LoginController.cs

- **Chama (`_unitOfWork`):**
  - `AspNetUsers` (Dados do usuário logado)
- **Servido por:**
  - `api/Login/RecuperaUsuarioAtual` (Dados de sessão)

### ManutencaoController.cs

- **Chama (`_unitOfWork`):**
  - `Manutencao` (Processo principal)
- **Chama (`_hostingEnvironment`):**
  - `Uploads` (Gestão de arquivos de NFs e Orçamentos)
- **Servido por:**
  - `api/Manutencao` (API geral)

### MarcaVeiculoController.cs

- **Chama (`_unitOfWork`):**
  - `MarcaVeiculo` (Cadastro)
  - `ModeloVeiculo` (Validação de exclusão)
- **Servido por:**
  - `api/MarcaVeiculo` (GET/DELETE)

### ModeloVeiculoController.cs

- **Chama (`_unitOfWork`):**
  - `ModeloVeiculo` (Cadastro)
  - `MarcaVeiculo` (Vinculação)
  - `Veiculo` (Validação de exclusão)
- **Servido por:**
  - `api/ModeloVeiculo` (GET/DELETE)

### MotoristaController.cs

- **Chama (`_unitOfWork`):**
  - `Motorista` (CRUD e gestão de motoristas)
  - `CNH` (Validação de CNH)
  - `Foto` (Gestão de fotos)
  - `Contrato` (Vínculo com contratos)
- **Servido por:**
  - `api/Motorista` (GET, POST, PUT, DELETE)
  - `api/Motorista/UpdateStatusMotorista`
  - `api/Motorista/ListaMotoristas` (Select2)
  - `api/Motorista/ObterDadosMotorista`

### MultaController.cs

- **Chama (`_unitOfWork`):**
  - `viewMultas` (Leitura de multas)
  - `Empenho` (Gestão financeira)
- **Servido por:**
  - `api/Multa/ListaMultas` (Filtros)

### MultaPdfViewerController.cs

- **Chama (`Syncfusion.EJ2.PdfViewer`):**
  - `PdfRenderer` (Renderização de documentos)
- **Servido por:**
  - `api/MultaPdfViewer/Load`
  - `api/MultaPdfViewer/RenderPdfPages`
  - `api/MultaPdfViewer/Unload`

### MultaUploadController.cs

- **Chama (`Servicos`):**
  - `TiraAcento` (Normalização de nomes)
- **Servido por:**
  - `api/MultaUpload/Save` (Syncfusion Uploader)

### NavigationController.cs

- **Chama (`_unitOfWork`):**
  - `Recurso` (Gestão de recursos do sistema)
  - `Grupo` (Gestão de grupos de recursos)
  - `Usuario` (Vínculo de usuários a grupos)
- **Chama (Outros):**
  - `Alerta.TratamentoErroComLinha` (Helper de Erro)
  - `ClaimsPrincipal` (Identidade do Usuário)
- **Servido por:**
  - `api/Navigation/ObterMenu`
  - `api/Navigation/SyncMenu`
  - `api/Navigation/ObterRecursosPorGrupo`
  - `api/Navigation/ObterGruposPorUsuario`
  - `api/Navigation/AtualizarGrupoRecurso`
  - `api/Navigation/ObterIconesFontAwesome`

### NormalizeController.cs

- **Chama (`Services`):**
  - `NormalizationService` (Serviço de Normalização de Texto)

### NotaFiscalController.cs

- **Chama (`_unitOfWork`):**
  - `NotaFiscal` (CRUD e gestão de notas fiscais)
  - `Empenho` (Vínculo com empenhos)
- **Servido por:**
  - `api/NotaFiscal` (GET, POST, PUT, DELETE)
  - `api/NotaFiscal/UpdateStatusNotaFiscal`

### OcorrenciaController.cs

- **Chama (`_unitOfWork`):**
  - `ViewViagens` (Listagem e filtro avançado de ocorrências)
- **Chama (Helpers):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)
- **Servido por:**
  - `api/Ocorrencia/ObterOcorrenciasPorViagem`
  - `api/Ocorrencia/RegistrarOcorrencia`
  - `api/Ocorrencia/AtualizarOcorrencia`
  - `api/Ocorrencia/ExcluirOcorrencia`

### OcorrenciaViagemController.cs

- **Chama (`_unitOfWork`):**
  - `ViewOcorrenciasViagem` (Listagem e filtro avançado de ocorrências em viagens)
- **Chama (Helpers):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)
- **Servido por:**
  - `api/OcorrenciaViagem/ObterOcorrenciasPorViagem`
  - `api/OcorrenciaViagem/RegistrarOcorrencia`
  - `api/OcorrenciaViagem/AtualizarOcorrencia`
  - `api/OcorrenciaViagem/ExcluirOcorrencia`

### OperadorController.cs

- **Chama (`_unitOfWork`):**
  - `Operador` (CRUD e gestão de operadores)
  - `Contrato` (Vínculo com contratos)
- **Servido por:**
  - `api/Operador` (GET, POST, PUT, DELETE)
  - `api/Operador/UpdateStatusOperador`
  - `api/Operador/ListaOperadores` (Select2)
  - `api/Operador/ObterDadosOperador`

### PatrimonioController.cs

- **Chama (`_unitOfWork`):**
  - `ViewPatrimonioConferencia` (Visualização e conferência de patrimônio)
  - `Patrimonio` (Gestão de patrimônio)
- **Chama (Helpers):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)
- **Servido por:**
  - `api/Patrimonio/ObterPatrimonioPorId`
  - `api/Patrimonio/Registrar`
  - `api/Patrimonio/Atualizar`
  - `api/Patrimonio/Excluir`
  - `api/Patrimonio/ObterRelatorioPatrimonio`

### PdfViewerCNHController.cs

- **Chama (`_unitOfWork`):**
  - `CNH` (Dados da CNH)
  - `Motorista` (Dados do motorista)
- **Chama (Helpers):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)
- **Servido por:**
  - `api/PdfViewerCNH/ObterPdfCNH`
  - `api/PdfViewerCNH/ObterListaPdfCNH`

### PdfViewerController.cs

- **Chama (`_unitOfWork`):**
  - `PdfViewer` (Visualizador de PDFs)
- **Servido por:**
  - `api/PdfViewer/ObterPdf`
  - `api/PdfViewer/ObterListaPdf`

### PlacaBronzeController.cs

- **Chama (`_unitOfWork`):**
  - `PlacaBronze` (CRUD e gestão de placas de bronze)
  - `Veiculo` (Validação de veículos)
- **Servido por:**
  - `api/PlacaBronze` (GET, POST, PUT, DELETE)
  - `api/PlacaBronze/UpdateStatusPlaca`

### RecursoController.cs

- **Chama (`_unitOfWork`):**
  - `Recurso` (Gestão de recursos do sistema)
  - `Grupo` (Gestão de grupos de recursos)
  - `Usuario` (Vínculo de usuários a grupos)
- **Chama (Outros):**
  - `Alerta.TratamentoErroComLinha` (Helper de Erro)
  - `ClaimsPrincipal` (Identidade do Usuário)
- **Servido por:**
  - `api/Navigation/ObterMenu`
  - `api/Navigation/SyncMenu`
  - `api/Navigation/ObterRecursosPorGrupo`
  - `api/Navigation/ObterGruposPorUsuario`
  - `api/Navigation/AtualizarGrupoRecurso`
  - `api/Navigation/ObterIconesFontAwesome`

### RelatoriosController.cs

- **Chama (`_unitOfWork`):**
  - `RelatorioEconomildo` (Geração de relatórios econômicos)
  - `SetorSolicitante` (Dados de setor)
  - `Veiculo`, `Motorista` (Relacionamentos)
- **Servido por:**
  - `api/Relatorios/ObterRelatorioEconomildo`
  - `api/Relatorios/ObterDadosGraficos`

### RelatorioSetorSolicitanteController.cs

- **Chama (`_unitOfWork`):**
  - `RelatorioSetorSolicitante` (Geração de relatórios por setor solicitante)
  - `SetorSolicitante` (Dados de setor)
- **Servido por:**
  - `api/RelatorioSetorSolicitante/ObterRelatorio`
  - `api/RelatorioSetorSolicitante/ObterDadosSetores`

### ReportsController.cs

- **Chama (`Services`):**
  - `IReportService` (Serviço de relatórios)
- **Servido por:**
  - `api/Reports/RunReport`
  - `api/Reports/ExportToPdf`
  - `api/Reports/ExportToExcel`
  - `api/Reports/ExportToWord`
  - `api/Reports/GetReportParameters`
  - `api/Reports/SaveReport`
  - `api/Reports/DeleteReport`
  - `api/Reports/GetReportById`

### RequisitanteController.cs

- **Chama (`_unitOfWork`):**
  - `Requisitante` (CRUD e gestão de requisitantes)
  - `SetorSolicitante` (Dados de setor solicitante)
- **Servido por:**
  - `api/Requisitante` (GET, POST, PUT, DELETE)
  - `api/Requisitante/UpdateStatusRequisitante`
  - `api/Requisitante/ListaRequisitantes` (Select2)
  - `api/Requisitante/ObterDadosRequisitante`

### SecaoController.cs

- **Chama (`_unitOfWork`):**
  - `SecaoPatrimonial` (CRUD e gestão de seções patrimoniais)
- **Servido por:**
  - `api/Secao` (GET, POST, PUT, DELETE)
  - `api/Secao/UpdateStatusSecao`

### SetorController.cs

- **Chama (`_unitOfWork`):**
  - `SetorPatrimonial` (CRUD e gestão de setores patrimoniais)
  - `AspNetUsers` (Vínculo de detentores)
- **Servido por:**
  - `api/Setor` (GET, POST, PUT, DELETE)
  - `api/Setor/UpdateStatusSetor`

### SetorSolicitanteController.cs

- **Chama (`_unitOfWork`):**
  - `SetorSolicitante` (CRUD e gestão de setores solicitantes)
- **Servido por:**
  - `api/SetorSolicitante` (GET, POST, PUT, DELETE)
  - `api/SetorSolicitante/UpdateStatusSetorSolicitante`

### TaxiLegController.cs

- **Chama (`_unitOfWork`):**
  - `CustoViagem` (Gestão de custos de viagem)
  - `TaxiLeg` (Cadastro e gestão de taxi legs)
- **Servido por:**
  - `api/TaxiLeg` (GET, POST, PUT, DELETE)
  - `api/TaxiLeg/UpdateStatusTaxiLeg`

### UnidadeController.cs

- **Chama (`_unitOfWork`):**
  - `Unidade` (CRUD e gestão de unidades)
  - `Veiculo` (Vínculo de veículos às unidades)
- **Servido por:**
  - `api/Unidade` (GET, POST, PUT, DELETE)
  - `api/Unidade/UpdateStatusUnidade`

### UploadCNHController.cs

- **Chama (`_unitOfWork`):**
  - `Motorista` (Upload de CNH digital)
- **Servido por:**
  - `api/UploadCNH/Registrar`

### UploadCRLVController.cs

- **Chama (`_unitOfWork`):**
  - `Veiculo` (Upload de CRLV)
- **Servido por:**
  - `api/UploadCRLV/Registrar`

### UsuarioController.cs

- **Chama (`_unitOfWork`):**
  - `AspNetUsers` (CRUD e gestão de usuários)
  - `Role` (Gestão de papéis)
  - `Claim` (Gestão de permissões)
- **Servido por:**
  - `api/Usuario` (GET, POST, PUT, DELETE)
  - `api/Usuario/UpdateStatusUsuario`
  - `api/Usuario/ListaUsuarios` (Select2)
  - `api/Usuario/ObterDadosUsuario`

### VeiculoController.cs

- **Chama (`_unitOfWork`):**
  - `ViewVeiculos` (Dados principais)
  - `Veiculo` (Detalhes e cadastro)
  - `Viagem` (Quilometragem e uso)
  - `ViewAbastecimentos` (Combustível e custos)
  - `Manutencao` (Custos de manutenção)
- **Servido por:**
  - `api/DashboardVeiculos/DashboardDados`
  - `api/DashboardVeiculos/DashboardUso`
  - `api/DashboardVeiculos/DashboardCustos`

### VeiculosUnidadeController.cs

- **Chama (`_unitOfWork`):**
  - `Veiculo` (Gestão de veículos)
  - `Unidade` (Gestão de unidades)
- **Servido por:**
  - `api/VeiculosUnidade/ObterVeiculosPorUnidade`
  - `api/VeiculosUnidade/RegistrarVeiculoUnidade`
  - `api/VeiculosUnidade/AtualizarVeiculoUnidade`
  - `api/VeiculosUnidade/ExcluirVeiculoUnidade`

### ViagemLimpezaController.cs

- **Chama (`_unitOfWork`):**
  - `ViagemRepository` (Repositório para acesso e manipulação de dados de viagem)
  - `Viagem` (Entidade de viagem)
- **Servido por:**
  - `api/ViagemLimpeza/LimparDadosViagem`
  - `api/ViagemLimpeza/ObterViagensParaLimpeza`
  - `api/ViagemLimpeza/RegistrarLimpezaViagem`

### ViagemEventoController.cs

- **Chama (`_unitOfWork`):**
  - `ViewViagens` (Gestão de Viagens para Eventos)
  - `Evento` (Dados dos Eventos)
  - `ViewFluxoEconomildo` (Fluxo de Dados para Economia)
- **Servido por:**
  - `api/ViagemEvento/ObterViagensPorEvento`
  - `api/ViagemEvento/RegistrarViagemEvento`
  - `api/ViagemEvento/AtualizarViagemEvento`
  - `api/ViagemEvento/ExcluirViagemEvento`

### ViagemController.cs

- **Chama (`_unitOfWork`):**
  - `Viagem` (CRUD e gestão de viagens)
  - `ViewViagens` (Visualização e filtros avançados)
  - `Veiculo` (Validação e dados do veículo)
  - `Motorista` (Validação e dados do motorista)
  - `Evento` (Relação com eventos especiais)
  - `Contrato` (Vínculo com contratos)
  - `Fornecedor` (Dados do fornecedor)
  - `CustoViagem` (Cálculo e gestão de custos)
- **Chama (Helpers):**
  - `Alerta.TratamentoErroComLinha` (Gestão de Erros)

### NavigationController | `Recurso`, `nav.json` | `Recurso`, `FontAwesome` | Gerenciamento de Menu Dinâmico

### NormalizeController | `NormalizationService` | `IA Local (BERT/ONNX)` | Normalização de Texto

### NotaFiscalController | `NotaFiscal`, `Empenho` | `Empenho (Saldo)` | Gestão de Notas Fiscais e Glosas

### OcorrenciaController | `ViewViagens` | `ViewViagens` | Relatórios de Ocorrências

### OcorrenciaViagemController | `OcorrenciaViagem`, `ViewOcorrenciasViagem` | `OcorrenciaViagem` | Gestão Detalhada de Ocorrências (Partial)

### OperadorController | `Operador`, `Contrato` | `Operador`, `OperadorContrato` | Gestão de Operadores

### PatrimonioController | `Patrimonio`, `ViewPatrimonioConferencia` | `MovimentacaoPatrimonio` | Gestão de Ativos e Movimentações

### PdfViewerCNHController | `Syncfusion.PdfViewer` | `File System (CNH)` | Visualizador de CNHs

### PdfViewerController | `Syncfusion.PdfViewer` | `File System (wwwroot)` | Visualizador de PDFs Genérico

### PlacaBronzeController | `PlacaBronze`, `Veiculo` | `PlacaBronze` | Gestão de Placas Oficiais

### RecursoController | `Recurso`, `ControleAcesso` | `Recurso` | Gestão de Permissões e Menu

### RelatoriosController | `FrotiXDbContext`, `RelatorioEconomildoPdfService` | `ViagensEconomildo` (Heatmap/Gráficos) | Geração de Relatórios PDF (QuestPDF)

### RelatorioSetorSolicitanteController | `Stimulsoft.Report` | `Reports/SetoresSolicitantes.mrt` | Relatórios de Setores Solicitantes

### ReportsController | `Telerik.Reporting` | `Telerik API` | API Base Telerik Reporting

### RequisitanteController | `Requisitante`, `SetorSolicitante` | `Requisitante` | Gestão de Requisitantes

### TaxiLegController | `ICorridasTaxiLegRepository`, `NPOI` | `CorridasTaxiLeg` | Importação e Gestão de Corridas Taxi Leg (Excel)

| Controller | Chama | É chamado por | Descrição |
|------------|-------|---------------|-----------|
| VeiculoController | `ViewVeiculos`, `Veiculo` | `Viagem`, `VeiculoContrato`, `Manutencao` | Gestão de Veículos da Frota |
| VeiculosUnidadeController | `Veiculo`, `Unidade` | `Veiculo` | Gestão de Veículos por Unidade |
| ViagemController | `ViagemRepository`, `FrotiXDbContext` | `ViewViagens`, `ViagensEconomildo` | Gestão Central de Viagens e Dashboard |
| ViagemEventoController | `ViewViagens`, `ViewFluxoEconomildo` | `ViagensEconomildo` | Gestão de Viagens de Eventos (Economildo) |
| ViagemLimpezaController | `ViagemRepository` | `Viagem` | Ferramentas de Limpeza de Dados (Origem/Destino) |

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

- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
