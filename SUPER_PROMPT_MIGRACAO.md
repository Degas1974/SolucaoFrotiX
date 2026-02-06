# SUPER-PROMPT: Migração FrotiX.Site → FrotiX.Site.Backup

## 1. CONTEXTO GERAL

Existe um projeto ASP.NET Core Razor Pages chamado **FrotiX** com duas versões:

- **FrotiX.Site** (SOURCE) — Versão mais recente com todo o código atualizado, MAS com licenciamento Telerik/Kendo/Syncfusion QUEBRADO (passou a usar arquivos locais em vez de CDN, trocou chaves de licença, adicionou kendo-error-suppressor.js como paliativo).
- **FrotiX.Site.Backup** (TARGET) — Versão de janeiro/2026, mais antiga no código, MAS com licenciamento Telerik/Kendo/Syncfusion FUNCIONAL (usa CDN correto, chaves válidas).

**OBJETIVO**: Atualizar o TARGET (Backup) com as alterações de código do SOURCE (Site), **preservando o modelo de licenciamento funcional do Backup**.

**CAMINHOS**:
```
SOURCE: c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site/
TARGET: c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Backup/
```

---

## 2. REGRAS TELERIK/KENDO/SYNCFUSION (CRÍTICO!)

Estas regras são ABSOLUTAS. Ao migrar qualquer arquivo, se encontrar referências Telerik/Kendo/Syncfusion que diferem entre SOURCE e TARGET, **SEMPRE preserve o modelo do TARGET (Backup)**:

| Aspecto | TARGET/Backup (MANTER!) | SOURCE/Site (NÃO USAR!) |
|---------|------------------------|------------------------|
| **Kendo JS** | CDN `kendo.cdn.telerik.com/2025.2.520/js/` | Local `~/lib/kendo/2025.2.520/` |
| **Syncfusion CSS** | CDN `cdn.syncfusion.com/ej2/32.1.19/bootstrap5.css` | unpkg `@syncfusion/ej2@32.1.23` |
| **Syncfusion JS** | CDN `cdn.syncfusion.com/ej2/32.1.19/dist/ej2.min.js` | unpkg `@syncfusion/ej2@32.1.23` |
| **CLDR Loading** | Síncrono (Syncfusion CDN) | Async (jsdelivr) |
| **Telerik Report Viewer** | v18.1.24.514 | v19.1.25.521 |
| **Syncfusion License Key** | Termina em `VkdwXkVWYEs=` | Termina em `RmRcUkFxWEtWYEs=` |
| **kendo-error-suppressor.js** | NÃO EXISTE (não incluir!) | Carregado primeiro |
| **Syncfusion versão no header** | 32.1.19 | 32.1.23 |

**Se o SOURCE adicionar código NOVO que usa Telerik de forma diferente do antigo, REFATORE-o para usar o modelo antigo do TARGET.**

---

## 3. TIPOS DE DIFERENÇAS ENCONTRADAS

As diferenças entre SOURCE e TARGET geralmente se enquadram em:

### 3.1 Headers de Documentação (DocGenerator)
O SOURCE tem headers extensivos gerados por IA no topo dos arquivos (banners ASCII, documentação XML de métodos, etc). O TARGET geralmente tem headers menores ou nenhum. **Pode copiar os headers do SOURCE para o TARGET** — são apenas comentários.

### 3.2 Alterações de Código Real
Funcionalidades novas, correções de bugs, refatorações. **Devem ser migradas.**

### 3.3 Referências Telerik/Kendo/Syncfusion
URLs, versões, chaves de licença. **NÃO migrar — manter o modelo do TARGET.**

### 3.4 Arquivos Novos
20 arquivos existem apenas no SOURCE e precisam ser CRIADOS no TARGET.

---

## 4. COMO PROCESSAR CADA ARQUIVO

Para cada arquivo da lista:

1. **Leia o arquivo no SOURCE** (`c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site/{path}`)
2. **Leia o arquivo no TARGET** (`c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Backup/{path}`)
3. **Compare mentalmente** (NÃO imprima os conteúdos inteiros — economize contexto!)
4. **Se já forem idênticos** → PULE (já foi migrado anteriormente)
5. **Se diferentes** → Aplique as alterações do SOURCE no TARGET usando a ferramenta Edit
6. **Se encontrar referências Telerik/Kendo/Syncfusion diferentes** → MANTENHA a versão do TARGET
7. **NÃO altere** se as diferenças forem apenas whitespace/BOM/formatação

**IMPORTANTE SOBRE EFICIÊNCIA**: Lotes anteriores estouraram contexto. Seja EFICIENTE:
- Não imprima arquivos inteiros
- Faça Edits cirúrgicos
- Se o arquivo já estiver idêntico, pule imediatamente
- Use "replace_all" quando possível para mudanças repetitivas

---

## 5. EXEMPLOS POR TIPO DE ARQUIVO (10 tipos)

### Exemplo A: Controller C# (ex: HomeController.cs)
- **Diferença típica**: SOURCE tem header DocGenerator enorme (50+ linhas com banners ASCII e documentação XML de métodos). TARGET tem header simples ou nenhum.
- **Além do header**: pode ter alterações de código real (novos endpoints, correção de try/catch, imports adicionais)
- **Ação**: Copiar header do SOURCE + aplicar alterações de código. Sem referências Telerik neste tipo.

### Exemplo B: Repository Interface (ex: IViagemRepository.cs)
- **Diferença típica**: SOURCE tem header DocGenerator com "ÍNDICE DE MÉTODOS" detalhado. Pode ter novos métodos na interface.
- **Ação**: Copiar header + adicionar novos métodos. Raramente tem referências Telerik.

### Exemplo C: Repository Implementation (ex: ViagemRepository.cs)
- **Diferença típica**: Header DocGenerator + implementação de novos métodos + possíveis otimizações de queries.
- **Ação**: Migrar tudo. Sem Telerik.

### Exemplo D: Razor Page .cshtml SEM Telerik (ex: Pages/Fornecedor/Index.cshtml)
- **Diferença típica**: Header DocGenerator + melhorias no HTML/Razor (novos botões, ajustes CSS classes, novos modais).
- **Ação**: Migrar tudo normalmente.

### Exemplo E: Razor Page .cshtml COM Telerik (CRÍTICO!) (ex: Pages/Viagens/Index.cshtml)
- **Diferença típica**: Header DocGenerator + alterações de código + possíveis mudanças em referências CDN Syncfusion/Kendo.
- **Ação**: Migrar alterações de código MAS preservar URLs CDN, versões Syncfusion, e referências Kendo do TARGET. Se o SOURCE trocar `cdn.syncfusion.com` por `unpkg.com`, MANTER `cdn.syncfusion.com`.

### Exemplo F: Code-behind .cshtml.cs (ex: Pages/Viagens/Index.cshtml.cs)
- **Diferença típica**: Header DocGenerator + possíveis novos métodos/propriedades no PageModel.
- **Ação**: Migrar tudo. Raramente tem referências Telerik diretas.

### Exemplo G: _Head.cshtml (ARQUIVO CRÍTICO!)
- **Diferença**: SOURCE muda CDN Syncfusion de `cdn.syncfusion.com/ej2/32.1.19/` para `unpkg.com/@syncfusion/ej2@32.1.23/`
- **Ação**: Migrar melhorias de código MAS MANTER as URLs CDN `cdn.syncfusion.com/ej2/32.1.19/` do TARGET.

### Exemplo H: _ScriptsBasePlugins.cshtml (ARQUIVO MAIS CRÍTICO!)
- **Diferença**: SOURCE adiciona `kendo-error-suppressor.js`, muda de CDN para local em Kendo, muda versão Syncfusion.
- **Ação**: Migrar melhorias de lógica MAS:
  - NÃO incluir `kendo-error-suppressor.js`
  - MANTER Kendo via CDN `kendo.cdn.telerik.com`
  - MANTER Syncfusion via CDN `cdn.syncfusion.com/ej2/32.1.19/`
  - MANTER chave de licença que termina em `VkdwXkVWYEs=`

### Exemplo I: JS com controles Kendo (ex: ViagemUpsert.js, kendo-editor-upsert.js)
- **Diferença típica**: Header DocGenerator + alterações de lógica JS + chamadas a controles Kendo (kendoComboBox, kendoEditor, kendoDatePicker, kendoUpload).
- **Ação**: Migrar lógica de código. As chamadas a controles Kendo (`.kendoComboBox()`, `.kendoEditor()`) geralmente são iguais em ambos — manter a sintaxe do TARGET se houver diferença.

### Exemplo J: Arquivo NOVO (ex: wwwroot/js/global-error-handler.js)
- **Diferença**: Arquivo existe apenas no SOURCE, não existe no TARGET.
- **Ação**: Criar o diretório no TARGET se necessário (mkdir -p), depois criar o arquivo com Write copiando o conteúdo do SOURCE. Se contiver referências Telerik modelo novo, adaptar para modelo antigo.

---

## 6. ARQUIVOS COM REFERÊNCIAS TELERIK (ATENÇÃO ESPECIAL!)

Estes 41 arquivos contêm referências Telerik/Kendo/Syncfusion e requerem cuidado extra:

```
Controllers/ReportsController.cs
Models/Cadastros/ViagemIndex.js
Pages/_ViewImports.cshtml
Pages/Abastecimento/DashboardAbastecimento.cshtml
Pages/Abastecimento/RegistraCupons.cshtml
Pages/Agenda/Index.cshtml
Pages/AtaRegistroPrecos/Upsert.cshtml
Pages/Manutencao/ControleLavagem.cshtml
Pages/Manutencao/DashboardLavagem.cshtml
Pages/Manutencao/Upsert.cshtml
Pages/Motorista/DashboardMotoristas.cshtml
Pages/Multa/UpsertAutuacao.cshtml
Pages/Relatorio/TesteRelatorio.cshtml
Pages/Shared/_Head.cshtml
Pages/Shared/_Layout.cshtml
Pages/Shared/_ScriptsBasePlugins.cshtml
Pages/Shared/Components/Navigation/TreeView.cshtml
Pages/TaxiLeg/Importacao.cshtml
Pages/Temp/Index.cshtml
Pages/Uploads/UploadPDF.cshtml
Pages/Veiculo/DashboardVeiculos.cshtml
Pages/Viagens/DashboardEventos.cshtml
Pages/Viagens/DashboardViagens.cshtml
Pages/Viagens/Index.cshtml
Pages/Viagens/Upsert.cshtml
Services/CustomReportSourceResolver.cs
Services/TelerikReportWarmupService.cs
wwwroot/js/agendamento/components/event-handlers.js
wwwroot/js/agendamento/components/evento.js
wwwroot/js/agendamento/components/exibe-viagem.js
wwwroot/js/agendamento/components/modal-viagem-novo.js
wwwroot/js/agendamento/components/relatorio.js
wwwroot/js/agendamento/components/reportviewer-close-guard.js
wwwroot/js/agendamento/components/validacao.js
wwwroot/js/agendamento/main.js
wwwroot/js/agendamento/utils/kendo-editor-helper.js
wwwroot/js/cadastros/agendamento_viagem.js
wwwroot/js/cadastros/ViagemIndex.js
wwwroot/js/cadastros/ViagemUpsert.js
wwwroot/js/frotix.js
wwwroot/js/viagens/kendo-editor-upsert.js
```

---

## 7. ARQUIVOS NOVOS (20 - CRIAR NO TARGET)

Estes arquivos existem apenas no SOURCE. Devem ser COPIADOS para o TARGET, adaptando referências Telerik se necessário:

```
Controllers/LogErrosController.Dashboard.cs
Models/Api/ApiResponse.cs
Models/LogErro.cs
Pages/Administracao/LogErrosDashboard.cshtml
Pages/Administracao/LogErrosDashboard.cshtml.cs
Pages/Viagens/ItensPendentes.cshtml
Pages/Viagens/ItensPendentes.cshtml.cs
Repository/IRepository/ILogRepository.cs
Repository/LogRepository.cs
Services/ClaudeAnalysisService.cs
Services/IClaudeAnalysisService.cs
Services/LogErrosAlertService.cs
Services/LogErrosCleanupService.cs
Services/LogErrosExportService.cs
wwwroot/js/agendamento/components/controls-init.js
wwwroot/js/agendamento/components/recorrencia-init.js
wwwroot/js/agendamento/components/recorrencia-logic.js
wwwroot/js/agendamento/components/recorrencia.js
wwwroot/js/frotix-api-client.js
wwwroot/js/global-error-handler.js
```

---

## 8. LISTA COMPLETA DOS 1010 ARQUIVOS DIFERENTES

**IMPORTANTE**: Alguns destes arquivos já foram migrados por uma rodada anterior de agentes. Ao processar, PRIMEIRO verifique se SOURCE e TARGET já são idênticos — se forem, PULE.

Divida em lotes de 15 arquivos e processe com agentes Haiku em paralelo.

### Lista completa (1010 arquivos):

Areas/Authorization/Pages/_ViewImports.cshtml
Areas/Authorization/Pages/_ViewStart.cshtml
Areas/Authorization/Pages/Roles.cshtml
Areas/Authorization/Pages/Roles.cshtml.cs
Areas/Authorization/Pages/Users.cshtml
Areas/Authorization/Pages/Users.cshtml.cs
Areas/Authorization/Pages/Usuarios.cshtml
Areas/Authorization/Pages/Usuarios.cshtml.cs
Areas/Identity/Pages/Account/_ViewImports.cshtml
Areas/Identity/Pages/Account/ConfirmEmail.cshtml
Areas/Identity/Pages/Account/ConfirmEmail.cshtml.cs
Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml
Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml.cs
Areas/Identity/Pages/Account/ForgotPassword.cshtml
Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs
Areas/Identity/Pages/Account/ForgotPasswordConfirmation.cshtml
Areas/Identity/Pages/Account/ForgotPasswordConfirmation.cshtml.cs
Areas/Identity/Pages/Account/Lockout.cshtml
Areas/Identity/Pages/Account/Lockout.cshtml.cs
Areas/Identity/Pages/Account/Login.cshtml
Areas/Identity/Pages/Account/Login.cshtml.cs
Areas/Identity/Pages/Account/LoginFrotiX.cshtml
Areas/Identity/Pages/Account/LoginFrotiX.cshtml.cs
Areas/Identity/Pages/Account/Logout.cshtml
Areas/Identity/Pages/Account/Logout.cshtml.cs
Areas/Identity/Pages/Account/Register.cshtml
Areas/Identity/Pages/Account/Register.cshtml.cs
Areas/Identity/Pages/Account/RegisterConfirmation.cshtml
Areas/Identity/Pages/Account/RegisterConfirmation.cshtml.cs
Areas/Identity/Pages/Account/ResetPassword.cshtml
Areas/Identity/Pages/Account/ResetPassword.cshtml.cs
Areas/Identity/Pages/Account/ResetPasswordConfirmation.cshtml
Areas/Identity/Pages/Account/ResetPasswordConfirmation.cshtml.cs
Areas/Identity/Pages/_ConfirmacaoLayout.cshtml
Areas/Identity/Pages/_Layout.cshtml
Areas/Identity/Pages/_LoginLayout.cshtml
Areas/Identity/Pages/_Logo.cshtml
Areas/Identity/Pages/_PageFooter.cshtml
Areas/Identity/Pages/_PageHeader.cshtml
Areas/Identity/Pages/_ViewImports.cshtml
Areas/Identity/Pages/_ViewStart.cshtml
Areas/Identity/Pages/ConfirmarSenha.cshtml
Areas/Identity/Pages/ConfirmarSenha.cshtml.cs
Cache/MotoristaCache.cs
Controllers/Api/DocGeneratorController.cs
Controllers/Api/WhatsAppController.cs
Controllers/AbastecimentoController.cs
Controllers/AbastecimentoController.DashboardAPI.cs
Controllers/AbastecimentoController.Import.cs
Controllers/AbastecimentoController.Pendencias.cs
Controllers/AbastecimentoImportController.cs
Controllers/AdministracaoController.cs
Controllers/AgendaController.cs
Controllers/AlertasFrotiXController.cs
Controllers/AtaRegistroPrecosController.cs
Controllers/AtaRegistroPrecosController.Partial.cs
Controllers/CombustivelController.cs
Controllers/ContratoController.cs
Controllers/ContratoController.Partial.cs
Controllers/ContratoController.VerificarDependencias.cs
Controllers/CustosViagemController.cs
Controllers/DashboardEventosController_ExportacaoPDF.cs
Controllers/DashboardEventosController.cs
Controllers/DashboardLavagemController.cs
Controllers/DashboardMotoristasController.cs
Controllers/DashboardVeiculosController.cs
Controllers/DashboardViagensController_ExportacaoPDF.cs
Controllers/DashboardViagensController.cs
Controllers/EditorController.cs
Controllers/EmpenhoController.cs
Controllers/EncarregadoController.cs
Controllers/EscalaController_Api.cs
Controllers/EscalaController.cs
Controllers/FornecedorController.cs
Controllers/GlosaController.cs
Controllers/GridAtaController.cs
Controllers/GridContratoController.cs
Controllers/HomeController.cs
Controllers/ItensContratoController.cs
Controllers/LavadorController.cs
Controllers/LogErrosController.cs
Controllers/LoginController.cs
Controllers/ManutencaoController.cs
Controllers/MarcaVeiculoController.cs
Controllers/ModeloVeiculoController.cs
Controllers/MotoristaController.cs
Controllers/MultaController.cs
Controllers/MultaPdfViewerController.cs
Controllers/MultaUploadController.cs
Controllers/NavigationController.cs
Controllers/NormalizeController.cs
Controllers/NotaFiscalController.cs
Controllers/NotaFiscalController.Partial.cs
Controllers/OcorrenciaController.cs
Controllers/OcorrenciaViagemController.cs
Controllers/OcorrenciaViagemController.Debug.cs
Controllers/OcorrenciaViagemController.Gestao.cs
Controllers/OcorrenciaViagemController.Listar.cs
Controllers/OcorrenciaViagemController.Upsert.cs
Controllers/OperadorController.cs
Controllers/PatrimonioController.cs
Controllers/PdfViewerCNHController.cs
Controllers/PdfViewerController.cs
Controllers/PlacaBronzeController.cs
Controllers/RecursoController.cs
Controllers/RelatoriosController.cs
Controllers/RelatorioSetorSolicitanteController.cs
Controllers/ReportsController.cs
Controllers/RequisitanteController.cs
Controllers/SecaoController.cs
Controllers/SetorController.cs
Controllers/SetorSolicitanteController.cs
Controllers/SetorSolicitanteController.GetAll.cs
Controllers/SetorSolicitanteController.UpdateStatus.cs
Controllers/TaxiLegController.cs
Controllers/TestePdfController.cs
Controllers/UnidadeController.cs
Controllers/UploadCNHController.cs
Controllers/UploadCRLVController.cs
Controllers/UsuarioController.cs
Controllers/UsuarioController.Usuarios.cs
Controllers/VeiculoController.cs
Controllers/VeiculosUnidadeController.cs
Controllers/ViagemController.AtualizarDados.cs
Controllers/ViagemController.AtualizarDadosViagem.cs
Controllers/ViagemController.CalculoCustoBatch.cs
Controllers/ViagemController.cs
Controllers/ViagemController.CustosViagem.cs
Controllers/ViagemController.DashboardEconomildo.cs
Controllers/ViagemController.DesassociarEvento.cs
Controllers/ViagemController.HeatmapEconomildo.cs
Controllers/ViagemController.HeatmapEconomildoPassageiros.cs
Controllers/ViagemController.ListaEventos.cs
Controllers/ViagemController.MetodosEstatisticas.cs
Controllers/ViagemEventoController.cs
Controllers/ViagemEventoController.UpdateStatus.cs
Controllers/ViagemLimpezaController.cs
Data/ApplicationDbContext.cs
Data/ControleAcessoDbContext.cs
Data/FrotiXDbContext.cs
Data/FrotiXDbContext.OcorrenciaViagem.cs
Data/FrotiXDbContext.RepactuacaoVeiculo.cs
EndPoints/RolesEndpoint.cs
EndPoints/UsersEndpoint.cs
Extensions/EnumerableExtensions.cs
Extensions/IdentityExtensions.cs
Extensions/ToastExtensions.cs
Filters/DisableModelValidationAttribute.cs
Filters/GlobalExceptionFilter.cs
Filters/PageExceptionFilter.cs
Filters/SkipModelValidationAttribute.cs
Helpers/Alerta.cs
Helpers/AlertaBackend.cs
Helpers/ErroHelper.cs
Helpers/ImageHelper.cs
Helpers/ListasCompartilhadas.cs
Helpers/SfdtHelper.cs
Hubs/AlertasHub.cs
Hubs/DocGenerationHub.cs
Hubs/EmailBasedUserIdProvider.cs
Hubs/EscalaHub.cs
Hubs/ImportacaoHub.cs
Infrastructure/CacheKeys.cs
Logging/FrotiXLoggerProvider.cs
Middlewares/ErrorLoggingMiddleware.cs
Middlewares/UiExceptionMiddleware.cs
Models/Cadastros/Abastecimento.cs
Models/Cadastros/Agenda.cs
Models/Cadastros/AspNetUsers.cs
Models/Cadastros/AtaRegistroPrecos.cs
Models/Cadastros/CoberturaFolga.cs
Models/Cadastros/Combustivel.cs
Models/Cadastros/Contrato.cs
Models/Cadastros/ControleAcesso.cs
Models/Cadastros/CorridasTaxiLeg.cs
Models/Cadastros/CorridasTaxiLegCanceladas.cs
Models/Cadastros/DeleteMovimentacaoWrapper.cs
Models/Cadastros/Empenho.cs
Models/Cadastros/EmpenhoMulta.cs
Models/Cadastros/EscalaDiaria.cs
Models/Cadastros/Escalas.cs
Models/Cadastros/Evento.cs
Models/Cadastros/FiltroEscala.cs
Models/Cadastros/Fornecedor.cs
Models/Cadastros/ItensContrato.cs
Models/Cadastros/ItensManutencao.cs
Models/Cadastros/Lavador.cs
Models/Cadastros/LavadorContrato.cs
Models/Cadastros/LavadoresLavagem.cs
Models/Cadastros/Lavagem.cs
Models/Cadastros/LotacaoMotorista.cs
Models/Cadastros/Manutencao.cs
Models/Cadastros/MarcaVeiculo.cs
Models/Cadastros/ModeloVeiculo.cs
Models/Cadastros/Motorista.cs
Models/Cadastros/MotoristaContrato.cs
Models/Cadastros/MovimentacaoEmpenho.cs
Models/Cadastros/MovimentacaoEmpenhoMulta.cs
Models/Cadastros/MovimentacaoPatrimonio.cs
Models/Cadastros/Multa.cs
Models/Cadastros/NotaFiscal.cs
Models/Cadastros/ObservacoesEscala.cs
Models/Cadastros/Operador.cs
Models/Cadastros/OperadorContrato.cs
Models/Cadastros/OrgaoAutuante.cs
Models/Cadastros/Patrimonio.cs
Models/Cadastros/PlacaBronze.cs
Models/Cadastros/Recurso.cs
Models/Cadastros/RegistroCupomAbastecimento.cs
Models/Cadastros/Requisitante.cs
Models/Cadastros/SecaoPatrimonial.cs
Models/Cadastros/SetorPatrimonial.cs
Models/Cadastros/SetorSolicitante.cs
Models/Cadastros/TipoMulta.cs
Models/Cadastros/Unidade.cs
Models/Cadastros/Veiculo.cs
Models/Cadastros/VeiculoAta.cs
Models/Cadastros/VeiculoContrato.cs
Models/Cadastros/Viagem.cs
Models/Cadastros/ViagemIndex.js
Models/Cadastros/ViagensEconomildo.cs
Models/DTO/EstatisticaVeiculoDto.cs
Models/DTO/HigienizacaoDto.cs
Models/DTO/LookupsDto.cs
Models/DTO/ViagemCalendarDTO.cs
Models/Estatisticas/AnosDisponiveisAbastecimento.cs
Models/Estatisticas/EstatisticaAbastecimentoCategoria.cs
Models/Estatisticas/EstatisticaAbastecimentoCombustivel.cs
Models/Estatisticas/EstatisticaAbastecimentoMensal.cs
Models/Estatisticas/EstatisticaAbastecimentoTipoVeiculo.cs
Models/Estatisticas/EstatisticaAbastecimentoVeiculo.cs
Models/Estatisticas/EstatisticaAbastecimentoVeiculoMensal.cs
Models/Estatisticas/EstatisticaGeralMensal.cs
Models/Estatisticas/EstatisticaMotoristasMensal.cs
Models/Estatisticas/EvolucaoViagensDiaria.cs
Models/Estatisticas/HeatmapAbastecimentoMensal.cs
Models/Estatisticas/HeatmapViagensMensal.cs
Models/Estatisticas/RankingMotoristasMensal.cs
Models/FontAwesome/FontAwesomeIconsModel.cs
Models/Planilhas/ExcelViewModel.cs
Models/Views/ViewAbastecimentos.cs
Models/Views/ViewAtaFornecedor.cs
Models/Views/ViewContratoFornecedor.cs
Models/Views/ViewControleAcesso.cs
Models/Views/ViewCustosViagem.cs
Models/Views/ViewEmpenhoMulta.cs
Models/Views/ViewEmpenhos.cs
Models/Views/ViewEscalasCompletas.cs
Models/Views/ViewEventos.cs
Models/Views/ViewExisteItemContrato.cs
Models/Views/ViewFluxoEconomildo.cs
Models/Views/ViewFluxoEconomildoData.cs
Models/Views/ViewGlosa.cs
Models/Views/ViewItensManutencao.cs
Models/Views/ViewLavagem.cs
Models/Views/ViewLotacaoMotorista.cs
Models/Views/ViewLotacoes.cs
Models/Views/ViewManutencao.cs
Models/Views/ViewMediaConsumo.cs
Models/Views/ViewMotoristaFluxo.cs
Models/Views/ViewMotoristas.cs
Models/Views/ViewMotoristasViagem.cs
Models/Views/ViewMotoristaVez.cs
Models/Views/ViewMultas.cs
Models/Views/ViewNoFichaVistoria.cs
Models/Views/ViewOcorrencia.cs
Models/Views/ViewPatrimonioConferencia.cs
Models/Views/ViewPendenciasManutencao.cs
Models/Views/ViewProcuraFicha.cs
Models/Views/ViewRequisitantes.cs
Models/Views/ViewSetores.cs
Models/Views/ViewStatusMotoristas.cs
Models/Views/ViewVeiculos.cs
Models/Views/ViewVeiculosManutencao.cs
Models/Views/ViewVeiculosManutencaoReserva.cs
Models/Views/ViewViagens.cs
Models/Views/ViewViagensAgenda.cs
Models/Views/ViewViagensAgendaTodosMeses.cs
Models/AbastecimentoPendente.cs
Models/AlertasFrotiX.cs
Models/ContractDropDownItem.cs
Models/DateItem.cs
Models/Encarregado.cs
Models/EncarregadoContrato.cs
Models/ErrorViewModel.cs
Models/EventoListDto.cs
Models/ForgotAccount.cs
Models/INavigationModel.cs
Models/ItensContrato.cs
Models/LoginView.cs
Models/MailRequest.cs
Models/NavigationItemDTO.cs
Models/NavigationModel.cs
Models/OcorrenciaViagem.cs
Models/RecursoTreeDTO.cs
Models/RepactuacaoVeiculo.cs
Models/SmartNavigation.cs
Models/SmartSettings.cs
Models/TempDataExtensions.cs
Models/ToastMessage.cs
Models/VeiculoPadraoViagem.cs
Models/ViagemEstatistica.cs
Models/ViagemEventoDto.cs
Models/ViewOcorrenciasAbertasVeiculo.cs
Models/ViewOcorrenciasViagem.cs
Pages/Abastecimento/DashboardAbastecimento.cshtml
Pages/Abastecimento/DashboardAbastecimento.cshtml.cs
Pages/Abastecimento/Importacao.cshtml
Pages/Abastecimento/Importacao.cshtml.cs
Pages/Abastecimento/Index.cshtml
Pages/Abastecimento/Index.cshtml.cs
Pages/Abastecimento/PBI.cshtml
Pages/Abastecimento/PBI.cshtml.cs
Pages/Abastecimento/Pendencias.cshtml
Pages/Abastecimento/Pendencias.cshtml.cs
Pages/Abastecimento/RegistraCupons.cshtml
Pages/Abastecimento/RegistraCupons.cshtml.cs
Pages/Abastecimento/UpsertCupons.cshtml
Pages/Abastecimento/UpsertCupons.cshtml.cs
Pages/Administracao/AjustaCustosViagem.cshtml
Pages/Administracao/AjustaCustosViagem.cshtml.cs
Pages/Administracao/CalculaCustoViagensTotal.cshtml
Pages/Administracao/CalculaCustoViagensTotal.cshtml.cs
Pages/Administracao/DashboardAdministracao.cshtml
Pages/Administracao/DashboardAdministracao.cshtml.cs
Pages/Administracao/DocGenerator.cshtml
Pages/Administracao/GerarEstatisticasViagens.cshtml
Pages/Administracao/GerarEstatisticasViagens.cshtml.cs
Pages/Administracao/GestaoRecursosNavegacao.cshtml
Pages/Administracao/GestaoRecursosNavegacao.cshtml.cs
Pages/Administracao/HigienizarViagens.cshtml
Pages/Administracao/HigienizarViagens.cshtml.cs
Pages/Administracao/LogErros.cshtml
Pages/Administracao/LogErros.cshtml.cs
Pages/Agenda/Index.cshtml
Pages/AlertasFrotiX/AlertasFrotiX.cshtml
Pages/AlertasFrotiX/AlertasFrotiX.cshtml.cs
Pages/AlertasFrotiX/Upsert.cshtml
Pages/AlertasFrotiX/Upsert.cshtml.cs
Pages/AtaRegistroPrecos/Index.cshtml
Pages/AtaRegistroPrecos/Index.cshtml.cs
Pages/AtaRegistroPrecos/Upsert.cshtml
Pages/AtaRegistroPrecos/Upsert.cshtml.cs
Pages/Combustivel/Index.cshtml
Pages/Combustivel/Index.cshtml.cs
Pages/Combustivel/Upsert.cshtml
Pages/Combustivel/Upsert.cshtml.cs
Pages/Contrato/Index.cshtml
Pages/Contrato/ItensContrato.cshtml
Pages/Contrato/ItensContrato.cshtml.cs
Pages/Contrato/RepactuacaoContrato.cshtml
Pages/Contrato/RepactuacaoContrato.cshtml.cs
Pages/Contrato/Upsert.cshtml
Pages/Contrato/Upsert.cshtml.cs
Pages/Empenho/Index.cshtml
Pages/Empenho/Index.cshtml.cs
Pages/Empenho/Upsert.cshtml
Pages/Empenho/Upsert.cshtml.cs
Pages/Encarregado/Index.cshtml
Pages/Encarregado/Index.cshtml.cs
Pages/Encarregado/Upsert.cshtml
Pages/Encarregado/Upsert.cshtml.cs
Pages/Escalas/FichaEscalas.cshtml
Pages/Escalas/FichaEscalas.cshtml.cs
Pages/Escalas/ListaEscala.cshtml
Pages/Escalas/ListaEscala.cshtml.cs
Pages/Escalas/UpsertCEscala.cshtml
Pages/Escalas/UpsertCEscala.cshtml.cs
Pages/Escalas/UpsertEEscala.cshtml
Pages/Escalas/UpsertEEscala.cshtml.cs
Pages/Fornecedor/Index.cshtml
Pages/Fornecedor/Index.cshtml.cs
Pages/Fornecedor/Upsert.cshtml
Pages/Fornecedor/Upsert.cshtml.cs
Pages/Frota/DashboardEconomildo.cshtml
Pages/Frota/DashboardEconomildo.cshtml.cs
Pages/Intel/AnalyticsDashboard.cshtml
Pages/Intel/AnalyticsDashboard.cshtml.cs
Pages/Intel/Introduction.cshtml
Pages/Intel/Introduction.cshtml.cs
Pages/Intel/MarketingDashboard.cshtml
Pages/Intel/MarketingDashboard.cshtml.cs
Pages/Intel/PaginaPrincipal.cshtml
Pages/Intel/PaginaPrincipal.cshtml.cs
Pages/Intel/Privacy.cshtml
Pages/Intel/Privacy.cshtml.cs
Pages/Lavador/Index.cshtml
Pages/Lavador/Index.cshtml.cs
Pages/Lavador/Upsert.cshtml
Pages/Lavador/Upsert.cshtml.cs
Pages/Manutencao/ControleLavagem.cshtml
Pages/Manutencao/ControleLavagem.cshtml.cs
Pages/Manutencao/DashboardLavagem.cshtml
Pages/Manutencao/DashboardLavagem.cshtml.cs
Pages/Manutencao/Glosas.cshtml
Pages/Manutencao/Glosas.cshtml.cs
Pages/Manutencao/ListaManutencao.cshtml
Pages/Manutencao/ListaManutencao.cshtml.cs
Pages/Manutencao/PBILavagem.cshtml
Pages/Manutencao/PBILavagem.cshtml.cs
Pages/Manutencao/Upsert.cshtml
Pages/Manutencao/Upsert.cshtml.cs
Pages/MarcaVeiculo/Index.cshtml
Pages/MarcaVeiculo/Index.cshtml.cs
Pages/MarcaVeiculo/Upsert.cshtml
Pages/MarcaVeiculo/Upsert.cshtml.cs
Pages/ModeloVeiculo/Index.cshtml
Pages/ModeloVeiculo/Index.cshtml.cs
Pages/ModeloVeiculo/Upsert.cshtml
Pages/ModeloVeiculo/Upsert.cshtml.cs
Pages/Motorista/DashboardMotoristas.cshtml
Pages/Motorista/DashboardMotoristas.cshtml.cs
Pages/Motorista/Index.cshtml
Pages/Motorista/PBILotacaoMotorista.cshtml
Pages/Motorista/PBILotacaoMotorista.cshtml.cs
Pages/Motorista/UploadCNH.cshtml
Pages/Motorista/UploadCNH.cshtml.cs
Pages/Motorista/Upsert.cshtml
Pages/Motorista/Upsert.cshtml.cs
Pages/MovimentacaoPatrimonio/Index.cshtml
Pages/MovimentacaoPatrimonio/Index.cshtml.cs
Pages/MovimentacaoPatrimonio/Upsert.cshtml
Pages/MovimentacaoPatrimonio/Upsert.cshtml.cs
Pages/Multa/ExibePDFAutuacao.cshtml
Pages/Multa/ExibePDFAutuacao.cshtml.cs
Pages/Multa/ExibePDFComprovante.cshtml
Pages/Multa/ExibePDFComprovante.cshtml.cs
Pages/Multa/ExibePDFPenalidade.cshtml
Pages/Multa/ExibePDFPenalidade.cshtml.cs
Pages/Multa/ListaAutuacao.cshtml
Pages/Multa/ListaAutuacao.cshtml.cs
Pages/Multa/ListaEmpenhosMulta.cshtml
Pages/Multa/ListaEmpenhosMulta.cshtml.cs
Pages/Multa/ListaOrgaosAutuantes.cshtml
Pages/Multa/ListaOrgaosAutuantes.cshtml.cs
Pages/Multa/ListaPenalidade.cshtml
Pages/Multa/ListaPenalidade.cshtml.cs
Pages/Multa/ListaTiposMulta.cshtml
Pages/Multa/ListaTiposMulta.cshtml.cs
Pages/Multa/PreencheListas.cshtml
Pages/Multa/PreencheListas.cshtml.cs
Pages/Multa/UploadPDF.cshtml
Pages/Multa/UploadPDF.cshtml.cs
Pages/Multa/UpsertAutuacao.cshtml
Pages/Multa/UpsertAutuacao.cshtml.cs
Pages/Multa/UpsertEmpenhosMulta.cshtml
Pages/Multa/UpsertEmpenhosMulta.cshtml.cs
Pages/Multa/UpsertOrgaoAutuante.cshtml
Pages/Multa/UpsertOrgaoAutuante.cshtml.cs
Pages/Multa/UpsertPenalidade.cshtml
Pages/Multa/UpsertPenalidade.cshtml.cs
Pages/Multa/UpsertTipoMulta.cshtml
Pages/Multa/UpsertTipoMulta.cshtml.cs
Pages/NotaFiscal/Index.cshtml
Pages/NotaFiscal/Index.cshtml.cs
Pages/NotaFiscal/Upsert.cshtml
Pages/NotaFiscal/Upsert.cshtml.cs
Pages/Ocorrencia/Ocorrencias.cshtml
Pages/Ocorrencia/Ocorrencias.cshtml.cs
Pages/Operador/Index.cshtml
Pages/Operador/Index.cshtml.cs
Pages/Operador/Upsert.cshtml
Pages/Operador/Upsert.cshtml.cs
Pages/Page/Chat.cshtml
Pages/Page/Chat.cshtml.cs
Pages/Page/Confirmation.cshtml
Pages/Page/Confirmation.cshtml.cs
Pages/Page/Contacts.cshtml
Pages/Page/Contacts.cshtml.cs
Pages/Page/Error.cshtml
Pages/Page/Error.cshtml.cs
Pages/Page/Forget.cshtml
Pages/Page/Forget.cshtml.cs
Pages/Page/ForumDiscussion.cshtml
Pages/Page/ForumDiscussion.cshtml.cs
Pages/Page/ForumList.cshtml
Pages/Page/ForumList.cshtml.cs
Pages/Page/ForumThreads.cshtml
Pages/Page/ForumThreads.cshtml.cs
Pages/Page/InboxGeneral.cshtml
Pages/Page/InboxGeneral.cshtml.cs
Pages/Page/InboxRead.cshtml
Pages/Page/InboxRead.cshtml.cs
Pages/Page/InboxWrite.cshtml
Pages/Page/InboxWrite.cshtml.cs
Pages/Page/Index.cshtml
Pages/Page/Index.cshtml.cs
Pages/Page/Invoice.cshtml
Pages/Page/Invoice.cshtml.cs
Pages/Page/Locked.cshtml
Pages/Page/Locked.cshtml.cs
Pages/Page/Login.cshtml
Pages/Page/Login.cshtml.cs
Pages/Page/LoginAlt.cshtml
Pages/Page/LoginAlt.cshtml.cs
Pages/Page/Profile.cshtml
Pages/Page/Profile.cshtml.cs
Pages/Page/Projects.cshtml
Pages/Page/Projects.cshtml.cs
Pages/Page/Register.cshtml
Pages/Page/Register.cshtml.cs
Pages/Page/Search.cshtml
Pages/Page/Search.cshtml.cs
Pages/Patrimonio/Index.cshtml
Pages/Patrimonio/Index.cshtml.cs
Pages/Patrimonio/Upsert.cshtml
Pages/Patrimonio/Upsert.cshtml.cs
Pages/PlacaBronze/Index.cshtml
Pages/PlacaBronze/Index.cshtml.cs
Pages/PlacaBronze/Upsert.cshtml
Pages/PlacaBronze/Upsert.cshtml.cs
Pages/Relatorio/TesteRelatorio.cshtml
Pages/Relatorio/TesteRelatorio.cshtml.cs
Pages/Requisitante/Index.cshtml
Pages/Requisitante/Index.cshtml.cs
Pages/Requisitante/Upsert.cshtml
Pages/Requisitante/Upsert.cshtml.cs
Pages/SecaoPatrimonial/Index.cshtml
Pages/SecaoPatrimonial/Index.cshtml.cs
Pages/SecaoPatrimonial/Upsert.cshtml
Pages/SecaoPatrimonial/Upsert.cshtml.cs
Pages/SetorPatrimonial/Index.cshtml
Pages/SetorPatrimonial/Index.cshtml.cs
Pages/SetorPatrimonial/Upsert.cshtml
Pages/SetorPatrimonial/Upsert.cshtml.cs
Pages/SetorSolicitante/Index.cshtml
Pages/SetorSolicitante/Index.cshtml.cs
Pages/SetorSolicitante/Upsert.cshtml
Pages/SetorSolicitante/Upsert.cshtml.cs
Pages/Shared/Components/Navigation/Default.cshtml
Pages/Shared/Components/Navigation/TreeView.cshtml
Pages/Shared/_AlertasSino.cshtml
Pages/Shared/_AlertasSino.cshtml.cs
Pages/Shared/_ColorProfileReference.cshtml
Pages/Shared/_Compose.cshtml
Pages/Shared/_ComposeLayout.cshtml
Pages/Shared/_Contact.cshtml
Pages/Shared/_CookieConsentPartial.cshtml
Pages/Shared/_DropdownApp.cshtml
Pages/Shared/_DropdownMenu.cshtml
Pages/Shared/_DropdownNotification.cshtml
Pages/Shared/_Favicon.cshtml
Pages/Shared/_GoogleAnalytics.cshtml
Pages/Shared/_Head.cshtml
Pages/Shared/_ImagemFichaVistoriaAmarela.cshtml
Pages/Shared/_Layout.cshtml
Pages/Shared/_LeftPanel.cshtml
Pages/Shared/_Logo.cshtml
Pages/Shared/_Menu.cshtml
Pages/Shared/_NavFilter.cshtml
Pages/Shared/_NavFilterMsg.cshtml
Pages/Shared/_NavFooter.cshtml
Pages/Shared/_NavInfoCard.cshtml
Pages/Shared/_PageBreadcrumb.cshtml
Pages/Shared/_PageContentOverlay.cshtml
Pages/Shared/_PageFooter.cshtml
Pages/Shared/_PageHeader.cshtml
Pages/Shared/_PageHeading.cshtml
Pages/Shared/_PageSettings.cshtml
Pages/Shared/_ScriptsBasePlugins.cshtml
Pages/Shared/_ScriptsLoadingSaving.cshtml
Pages/Shared/_ShortcutMenu.cshtml
Pages/Shared/_ShortcutMessenger.cshtml
Pages/Shared/_ShortcutModal.cshtml
Pages/Shared/_Signature.cshtml
Pages/Shared/_TabMsgr.cshtml
Pages/Shared/_TabSettings.cshtml
Pages/Shared/_ToastPartial.cshtml
Pages/Shared/_ToastPartial.cshtml.cs
Pages/Shared/_ValidationScriptsPartial.cshtml
Pages/TaxiLeg/Canceladas.cshtml
Pages/TaxiLeg/Canceladas.cshtml.cs
Pages/TaxiLeg/Importacao.cshtml
Pages/TaxiLeg/Importacao.cshtml.cs
Pages/TaxiLeg/PBITaxiLeg.cshtml
Pages/TaxiLeg/PBITaxiLeg.cshtml.cs
Pages/Temp/Index.cshtml
Pages/Temp/Index.cshtml.cs
Pages/Unidade/Index.cshtml
Pages/Unidade/Index.cshtml.cs
Pages/Unidade/LotacaoMotoristas.cshtml
Pages/Unidade/LotacaoMotoristas.cshtml.cs
Pages/Unidade/Upsert.cshtml
Pages/Unidade/Upsert.cshtml.cs
Pages/Unidade/VeiculosUnidade.cshtml
Pages/Unidade/VeiculosUnidade.cshtml.cs
Pages/Unidade/VisualizaLotacoes.cshtml
Pages/Unidade/VisualizaLotacoes.cshtml.cs
Pages/Uploads/UploadPDF.cshtml
Pages/Uploads/UploadPDF.cshtml.cs
Pages/Uploads/UpsertAutuacao.cshtml
Pages/Uploads/UpsertAutuacao.cshtml.cs
Pages/Usuarios/Index.cshtml
Pages/Usuarios/Index.cshtml.cs
Pages/Usuarios/InsereRecursosUsuarios.cshtml
Pages/Usuarios/InsereRecursosUsuarios.cshtml.cs
Pages/Usuarios/Recursos.cshtml
Pages/Usuarios/Recursos.cshtml.cs
Pages/Usuarios/Registrar.cshtml
Pages/Usuarios/Registrar.cshtml.cs
Pages/Usuarios/Report.cshtml
Pages/Usuarios/Report.cshtml.cs
Pages/Usuarios/Upsert.cshtml
Pages/Usuarios/Upsert.cshtml.cs
Pages/Usuarios/UpsertRecurso.cshtml
Pages/Usuarios/UpsertRecurso.cshtml.cs
Pages/Veiculo/DashboardVeiculos.cshtml
Pages/Veiculo/DashboardVeiculos.cshtml.cs
Pages/Veiculo/Index.cshtml
Pages/Veiculo/Index.cshtml.cs
Pages/Veiculo/UploadCRLV.cshtml
Pages/Veiculo/UploadCRLV.cshtml.cs
Pages/Veiculo/Upsert.cshtml
Pages/Veiculo/Upsert.cshtml.cs
Pages/Viagens/_SecaoOcorrenciasFinalizacao.cshtml
Pages/Viagens/DashboardEventos.cshtml
Pages/Viagens/DashboardEventos.cshtml.cs
Pages/Viagens/DashboardViagens.cshtml
Pages/Viagens/DashboardViagens.cshtml.cs
Pages/Viagens/ExportarParaPDF.cshtml
Pages/Viagens/ExportarParaPDF.cshtml.cs
Pages/Viagens/FluxoPassageiros.cshtml
Pages/Viagens/FluxoPassageiros.cshtml.cs
Pages/Viagens/GestaoFluxo.cshtml
Pages/Viagens/GestaoFluxo.cshtml.cs
Pages/Viagens/Index.cshtml
Pages/Viagens/Index.cshtml.cs
Pages/Viagens/ListaEventos.cshtml
Pages/Viagens/ListaEventos.cshtml.cs
Pages/Viagens/TaxiLeg.cshtml
Pages/Viagens/TaxiLeg.cshtml.cs
Pages/Viagens/TestGrid.cshtml
Pages/Viagens/TestGrid.cshtml.cs
Pages/Viagens/Upsert.cshtml
Pages/Viagens/Upsert.cshtml.cs
Pages/Viagens/UpsertEvento.cshtml
Pages/Viagens/UpsertEvento.cshtml.cs
Pages/WhatsApp/Index.cshtml
Pages/WhatsApp/Index.cshtml.cs
Pages/_ViewImports.cshtml
Pages/_ViewStart.cshtml
Repository/IRepository/IAbastecimentoRepository.cs
Repository/IRepository/IAlertasFrotiXRepository.cs
Repository/IRepository/IAlertasUsuarioRepository.cs
Repository/IRepository/IAspNetUsersRepository.cs
Repository/IRepository/IAtaRegistroPrecosRepository.cs
Repository/IRepository/ICombustivelRepository.cs
Repository/IRepository/IContratoRepository.cs
Repository/IRepository/IControleAcessoRepository.cs
Repository/IRepository/ICorridasTaxiLeg.cs
Repository/IRepository/ICorridasTaxiLegCanceladas.cs
Repository/IRepository/ICustoMensalItensContratoRepository.cs
Repository/IRepository/IEmpenhoMultaRepository.cs
Repository/IRepository/IEmpenhoRepository.cs
Repository/IRepository/IEncarregadoContratoRepository.cs
Repository/IRepository/IEncarregadoRepository.cs
Repository/IRepository/IEscalasRepository.cs
Repository/IRepository/IEventoRepository.cs
Repository/IRepository/IFornecedorRepository.cs
Repository/IRepository/IItemVeiculoAtaRepository.cs
Repository/IRepository/IItemVeiculoContratoRepository.cs
Repository/IRepository/IItensManutencaoRepository.cs
Repository/IRepository/ILavadorContratoRepository.cs
Repository/IRepository/ILavadoresLavagemRepository.cs
Repository/IRepository/ILavadorRepository.cs
Repository/IRepository/ILavagemRepository.cs
Repository/IRepository/ILotacaoMotoristaRepository.cs
Repository/IRepository/IManutencaoRepository.cs
Repository/IRepository/IMarcaVeiculoRepository.cs
Repository/IRepository/IMediaCombustivelRepository.cs
Repository/IRepository/IModeloVeiculoRepository.cs
Repository/IRepository/IMotoristaContratoRepository.cs
Repository/IRepository/IMotoristaRepository.cs
Repository/IRepository/IMovimentacaoEmpenhoMultaRepository.cs
Repository/IRepository/IMovimentacaoEmpenhoRepository.cs
Repository/IRepository/IMovimentacaoPatrimonioRepository.cs
Repository/IRepository/IMultaRepository.cs
Repository/IRepository/INotaFiscalRepository.cs
Repository/IRepository/IOcorrenciaViagemRepository.cs
Repository/IRepository/IOperadorContratoRepository.cs
Repository/IRepository/IOperadorRepository.cs
Repository/IRepository/IOrgaoAutuanteRepository.cs
Repository/IRepository/IPatrimonioRepository.cs
Repository/IRepository/IPlacaBronzeRepository.cs
Repository/IRepository/IRecursoRepository.cs
Repository/IRepository/IRegistroCupomAbastecimentoRepository.cs
Repository/IRepository/IRepactuacaoAtaRepository.cs
Repository/IRepository/IRepactuacaoContratoRepository.cs
Repository/IRepository/IRepactuacaoServicosRepository.cs
Repository/IRepository/IRepactuacaoTerceirizacaoRepository.cs
Repository/IRepository/IRepactuacaoVeiculoRepository.cs
Repository/IRepository/IRepository.cs
Repository/IRepository/IRequisitanteRepository.cs
Repository/IRepository/ISecaoPatrimonialRepository.cs
Repository/IRepository/ISetorPatrimonialRepository.cs
Repository/IRepository/ISetorSolicitanteRepository.cs
Repository/IRepository/ITipoMultaRepository.cs
Repository/IRepository/IUnidadeRepository.cs
Repository/IRepository/IUnitOfWork.cs
Repository/IRepository/IUnitOfWork.OcorrenciaViagem.cs
Repository/IRepository/IUnitOfWork.RepactuacaoVeiculo.cs
Repository/IRepository/IVeiculoAtaRepository.cs
Repository/IRepository/IVeiculoContratoRepository.cs
Repository/IRepository/IVeiculoPadraoViagemRepository.cs
Repository/IRepository/IVeiculoRepository.cs
Repository/IRepository/IViagemEstatisticaRepository.cs
Repository/IRepository/IViagemRepository.cs
Repository/IRepository/IViagensEconomildoRepository.cs
Repository/IRepository/IViewAbastecimentosRepository.cs
Repository/IRepository/IViewAtaFornecedor.cs
Repository/IRepository/IViewContratoFornecedor.cs
Repository/IRepository/IViewControleAcessoRepository.cs
Repository/IRepository/IViewCustosViagemRepository.cs
Repository/IRepository/IViewEmpenhoMultaRepository.cs
Repository/IRepository/IViewEmpenhosRepository.cs
Repository/IRepository/IViewEventos.cs
Repository/IRepository/IViewExisteItemContratoRepository.cs
Repository/IRepository/IViewFluxoEconomildo.cs
Repository/IRepository/IViewFluxoEconomildoDataRepository.cs
Repository/IRepository/IViewGlosaRepository.cs
Repository/IRepository/IViewItensManutencaoRepository.cs
Repository/IRepository/IViewLavagemRepository.cs
Repository/IRepository/IViewLotacaoMotorista.cs
Repository/IRepository/IViewLotacoesRepository.cs
Repository/IRepository/IViewManutencaoRepository.cs
Repository/IRepository/IViewMediaConsumoRepository.cs
Repository/IRepository/IViewMotoristaFluxo.cs
Repository/IRepository/IViewMotoristasRepository.cs
Repository/IRepository/IViewMotoristasViagemRepository.cs
Repository/IRepository/IViewMultasRepository.cs
Repository/IRepository/IViewNoFichaVistoriaRepository.cs
Repository/IRepository/IViewOcorrencia.cs
Repository/IRepository/IViewOcorrenciasAbertasVeiculoRepository.cs
Repository/IRepository/IViewOcorrenciasViagemRepository.cs
Repository/IRepository/IViewPatrimonioConferenciaRepository.cs
Repository/IRepository/IViewPendenciasManutencaoRepository.cs
Repository/IRepository/IViewProcuraFichaRepository.cs
Repository/IRepository/IViewRequisitantesRepository.cs
Repository/IRepository/IViewSetoresRepository.cs
Repository/IRepository/IViewVeiculosManutencaoRepository.cs
Repository/IRepository/IViewVeiculosManutencaoReservaRepository.cs
Repository/IRepository/IViewVeiculosRepository.cs
Repository/IRepository/IViewViagensAgendaRepository.cs
Repository/IRepository/IViewViagensAgendaTodosMesesRepository.cs
Repository/IRepository/IViewViagensRepository.cs
Repository/AbastecimentoRepository.cs
Repository/AlertasFrotiXRepository.cs
Repository/AlertasUsuarioRepository.cs
Repository/AspNetUsersRepository.cs
Repository/AtaRegistroPrecosRepository.cs
Repository/CombustivelRepository.cs
Repository/ContratoRepository.cs
Repository/ControleAcessoRepository.cs
Repository/CorridasTaxiLegCanceladasRepository.cs
Repository/CorridasTaxiLegRepository.cs
Repository/CustoMensalItensContratoRepository.cs
Repository/EmpenhoMultaRepository.cs
Repository/EmpenhoRepository.cs
Repository/EncarregadoContratoRepository.cs
Repository/EncarregadoRepository.cs
Repository/EscalasRepository.cs
Repository/EventoRepository.cs
Repository/FornecedorRepository.cs
Repository/ItemVeiculoAtaRepository.cs
Repository/ItemVeiculoContratoRepository.cs
Repository/ItensManutencaoRepository.cs
Repository/LavadorContratoRepository.cs
Repository/LavadoresLavagemRepository.cs
Repository/LavadorRepository.cs
Repository/LavagemRepository.cs
Repository/LotacaoMotoristaRepository.cs
Repository/ManutencaoRepository.cs
Repository/MarcaVeiculoRepository.cs
Repository/MediaCombustivelRepository.cs
Repository/ModeloVeiculoRepository.cs
Repository/MotoristaContratoRepository.cs
Repository/MotoristaRepository.cs
Repository/MovimentacaoEmpenhoMultaRepository.cs
Repository/MovimentacaoEmpenhoRepository.cs
Repository/MovimentacaoPatrimonioRepository.cs
Repository/MultaRepository.cs
Repository/NotaFiscalRepository.cs
Repository/OcorrenciaViagemRepository.cs
Repository/OperadorContratoRepository.cs
Repository/OperadorRepository.cs
Repository/OrgaoAutuanteRepository.cs
Repository/PatrimonioRepository.cs
Repository/PlacaBronzeRepository.cs
Repository/RecursoRepository.cs
Repository/RegistroCupomAbastecimentoRepository.cs
Repository/RepactuacaoAtaRepository.cs
Repository/RepactuacaoContratoRepository.cs
Repository/RepactuacaoServicosRepository.cs
Repository/RepactuacaoTerceirizacaoRepository.cs
Repository/RepactuacaoVeiculoRepository.cs
Repository/Repository.cs
Repository/RequisitanteRepository.cs
Repository/SecaoPatrimonialRepository.cs
Repository/SetorPatrimonialRepository.cs
Repository/SetorSolicitanteRepository.cs
Repository/TipoMultaRepository.cs
Repository/UnidadeRepository.cs
Repository/UnitOfWork.cs
Repository/UnitOfWork.OcorrenciaViagem.cs
Repository/UnitOfWork.RepactuacaoVeiculo.cs
Repository/VeiculoAtaRepository.cs
Repository/VeiculoContratoRepository.cs
Repository/VeiculoPadraoViagemRepository.cs
Repository/VeiculoRepository.cs
Repository/ViagemEstatisticaRepository.cs
Repository/ViagemRepository.cs
Repository/ViagensEconomildoRepository.cs
Repository/ViewAbastecimentosRepository.cs
Repository/ViewAtaFornecedorRepository.cs
Repository/ViewContratoFornecedorRepository.cs
Repository/ViewControleAcessoRepository.cs
Repository/ViewCustosViagemRepository.cs
Repository/ViewEmpenhoMultaRepository.cs
Repository/ViewEmpenhosRepository.cs
Repository/ViewEventosRepository.cs
Repository/ViewExisteItemContratoRepository.cs
Repository/ViewFluxoEconomildo.cs
Repository/ViewFluxoEconomildoData.cs
Repository/ViewGlosaRepository.cs
Repository/ViewItensManutencaoRepository.cs
Repository/ViewLavagemRepository.cs
Repository/ViewLotacaoMotoristaRepository.cs
Repository/ViewLotacoesRepository.cs
Repository/ViewManutencaoRepository.cs
Repository/ViewMediaConsumoRepository.cs
Repository/ViewMotoristaFluxo.cs
Repository/ViewMotoristasRepository.cs
Repository/ViewMotoristasViagemRepository.cs
Repository/ViewMultasRepository.cs
Repository/ViewNoFichaVistoriaRepository.cs
Repository/ViewOcorrencia.cs
Repository/ViewOcorrenciasAbertasVeiculoRepository.cs
Repository/ViewOcorrenciasViagemRepository.cs
Repository/ViewPatrimonioConferenciaRepository.cs
Repository/ViewPendenciasManutencaoRepository.cs
Repository/ViewProcuraFichaRepository.cs
Repository/ViewRequisitantesRepository.cs
Repository/ViewSetoresRepository.cs
Repository/ViewVeiculosManutencaoRepository.cs
Repository/ViewVeiculosManutencaoReservaRepository.cs
Repository/ViewVeiculosRepository.cs
Repository/ViewViagensAgendaRepository.cs
Repository/ViewViagensAgendaTodosMesesRepository.cs
Repository/ViewViagensRepository.cs
Services/DocGenerator/Interfaces/IDocGeneratorServices.cs
Services/DocGenerator/Models/DocGeneratorModels.cs
Services/DocGenerator/Providers/BaseDocProvider.cs
Services/DocGenerator/Providers/ClaudeDocProvider.cs
Services/DocGenerator/Providers/GeminiDocProvider.cs
Services/DocGenerator/Providers/OpenAiDocProvider.cs
Services/DocGenerator/Services/FileTrackingService.cs
Services/DocGenerator/DocCacheService.cs
Services/DocGenerator/DocComposerService.cs
Services/DocGenerator/DocExtractionService.cs
Services/DocGenerator/DocGeneratorOrchestrator.cs
Services/DocGenerator/DocGeneratorServiceCollectionExtensions.cs
Services/DocGenerator/DocRenderService.cs
Services/DocGenerator/FileDiscoveryService.cs
Services/Pdf/RelatorioEconomildoDto.cs
Services/Pdf/RelatorioEconomildoPdfService.cs
Services/Pdf/SvgIcones.cs
Services/WhatsApp/Dtos.cs
Services/WhatsApp/EvolutionApiOptions.cs
Services/WhatsApp/EvolutionApiWhatsAppService.cs
Services/WhatsApp/IWhatsAppService.cs
Services/AlertasBackgroundService.cs
Services/AppToast.cs
Services/CacheWarmupService.cs
Services/CustomReportSourceResolver.cs
Services/GlosaDtos.cs
Services/GlosaService.cs
Services/IGlosaService.cs
Services/ILogService.cs
Services/IMailService.cs
Services/IReCaptchaService.cs
Services/LogService.cs
Services/MailService.cs
Services/MotoristaFotoService.cs
Services/RazorRenderService.cs
Services/ReCaptchaService.cs
Services/Servicos.cs
Services/ServicosAsync.cs
Services/TelerikReportWarmupService.cs
Services/ToastService.cs
Services/Validations.cs
Services/VeiculoEstatisticaService.cs
Services/ViagemEstatisticaService.cs
Settings/GlobalVariables.cs
Settings/MailSettings.cs
Settings/ReCaptchaSettings.cs
Settings/RecorrenciaToggleSettings.cs
Tools/DocGenerator/Program.cs
ViewComponents/CustomViewModel.cs
ViewComponents/NavigationViewComponent.cs
wwwroot/js/agendamento/components/calendario.js
wwwroot/js/agendamento/components/dialogs.js
wwwroot/js/agendamento/components/event-handlers.js
wwwroot/js/agendamento/components/evento.js
wwwroot/js/agendamento/components/exibe-viagem.js
wwwroot/js/agendamento/components/modal-config.js
wwwroot/js/agendamento/components/modal-viagem-novo.js
wwwroot/js/agendamento/components/relatorio.js
wwwroot/js/agendamento/components/reportviewer-close-guard.js
wwwroot/js/agendamento/components/sweetalert_interop.patch.js
wwwroot/js/agendamento/components/validacao.js
wwwroot/js/agendamento/core/ajax-helper.js
wwwroot/js/agendamento/core/api-client.js
wwwroot/js/agendamento/core/state.js
wwwroot/js/agendamento/services/agendamento.service.js
wwwroot/js/agendamento/services/evento.service.js
wwwroot/js/agendamento/services/requisitante.service.js
wwwroot/js/agendamento/services/viagem.service.js
wwwroot/js/agendamento/utils/calendario-config.js
wwwroot/js/agendamento/utils/date.utils.js
wwwroot/js/agendamento/utils/formatters.js
wwwroot/js/agendamento/utils/kendo-editor-helper.js
wwwroot/js/agendamento/utils/syncfusion.utils.js
wwwroot/js/agendamento/main.js
wwwroot/js/alertasfrotix/alertas_gestao.js
wwwroot/js/alertasfrotix/alertas_navbar.js
wwwroot/js/alertasfrotix/alertas_recorrencia.js
wwwroot/js/alertasfrotix/alertas_upsert.js
wwwroot/js/cadastros/agendamento_viagem.js
wwwroot/js/cadastros/anulacao_001.js
wwwroot/js/cadastros/aporte_001.js
wwwroot/js/cadastros/ata.js
wwwroot/js/cadastros/atualizacustosviagem.js
wwwroot/js/cadastros/autuacao.js
wwwroot/js/cadastros/combustivel.js
wwwroot/js/cadastros/condutorapoio_001.js
wwwroot/js/cadastros/contrato.js
wwwroot/js/cadastros/CriarEscala.js
wwwroot/js/cadastros/EditarEscala.js
wwwroot/js/cadastros/empenho.js
wwwroot/js/cadastros/encarregado.js
wwwroot/js/cadastros/eventoupsert.js
wwwroot/js/cadastros/fluxopassageiros.js
wwwroot/js/cadastros/fornecedor.js
wwwroot/js/cadastros/Glosa_001.js
wwwroot/js/cadastros/insereviagem_001.js
wwwroot/js/cadastros/insereviagem.js
wwwroot/js/cadastros/itenscontrato.js
wwwroot/js/cadastros/lavador.js
wwwroot/js/cadastros/listaautuacao.js
wwwroot/js/cadastros/ListaEscala.js
wwwroot/js/cadastros/listaeventos.js
wwwroot/js/cadastros/ListaManutencao.js
wwwroot/js/cadastros/manutencao.js
wwwroot/js/cadastros/marcaveiculo.js
wwwroot/js/cadastros/modal_agenda.js
wwwroot/js/cadastros/modeloveiculo.js
wwwroot/js/cadastros/motorista_upsert.js
wwwroot/js/cadastros/motorista.js
wwwroot/js/cadastros/motoristasitenscontrato_001.js
wwwroot/js/cadastros/movimentacaopatrimonio.js
wwwroot/js/cadastros/multa.js
wwwroot/js/cadastros/multas-upload-handler.js
wwwroot/js/cadastros/notafiscal.js
wwwroot/js/cadastros/ocorrencias.js
wwwroot/js/cadastros/operador.js
wwwroot/js/cadastros/orgaoautuante.js
wwwroot/js/cadastros/patrimonio.js
wwwroot/js/cadastros/placabronze.js
wwwroot/js/cadastros/requisitante.js
wwwroot/js/cadastros/secao_patrimonial.js
wwwroot/js/cadastros/setor_patrimonial.js
wwwroot/js/cadastros/tipomulta_001.js
wwwroot/js/cadastros/unidade.js
wwwroot/js/cadastros/upsert_autuacao.js
wwwroot/js/cadastros/upsert_penalidade.js
wwwroot/js/cadastros/usuario_001.js
wwwroot/js/cadastros/usuario-index.js
wwwroot/js/cadastros/veiculo_index.js
wwwroot/js/cadastros/veiculo_upsert.js
wwwroot/js/cadastros/veiculositenscontrato_001.js
wwwroot/js/cadastros/veiculosunidade.js
wwwroot/js/cadastros/ViagemIndex.js
wwwroot/js/cadastros/ViagemUpsert.js
wwwroot/js/cadastros/viagens_001.js
wwwroot/js/cadastros/viagens_014.js
wwwroot/js/dashboards/dashboard-abastecimento.js
wwwroot/js/dashboards/dashboard-eventos.js
wwwroot/js/dashboards/dashboard-lavagem.js
wwwroot/js/dashboards/dashboard-motoristas.js
wwwroot/js/dashboards/dashboard-veiculos.js
wwwroot/js/dashboards/dashboard-viagens.js
wwwroot/js/validacao/ValidadorFinalizacaoIA.js
wwwroot/js/viagens/kendo-editor-upsert.js
wwwroot/js/viagens/ocorrencia-viagem-popup.js
wwwroot/js/viagens/ocorrencia-viagem.js
wwwroot/js/administracao.js
wwwroot/js/alerta.js
wwwroot/js/botao-loading.js
wwwroot/js/frotix-error-logger.js
wwwroot/js/frotix.js
wwwroot/js/ftx-datatable-style.js
wwwroot/js/global-toast.js
wwwroot/js/higienizarviagens_054.js
wwwroot/js/localization-init.js
wwwroot/js/signalr_manager.js
wwwroot/js/site.js
wwwroot/js/sweetalert_interop.js
wwwroot/js/syncfusion_tooltips.js
wwwroot/js/toastHelper_006.js
wwwroot/js/whatsapp.js

---

## 9. ESTRATÉGIA DE EXECUÇÃO RECOMENDADA

1. Divida os 1010 arquivos em lotes de **15 arquivos**
2. Lance **agentes Haiku em paralelo** (máximo 10 de cada vez para não estourar limites)
3. Cada agente recebe: o prompt base com as regras Telerik + a lista de 15 arquivos
4. O agente lê SOURCE e TARGET de cada arquivo, compara, aplica Edits
5. Se o arquivo já estiver idêntico → PULE (já migrado)
6. Ao final de cada lote, imprima resumo breve
7. Depois dos 1010, processe os 20 arquivos novos (criar no TARGET)

**DICA CRÍTICA**: Instrua os agentes a serem EFICIENTES com contexto — não imprimir arquivos inteiros, fazer Edits cirúrgicos. Agentes que imprimem muito conteúdo estouram o contexto antes de terminar.
