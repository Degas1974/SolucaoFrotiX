# Migração FrotiX.Site -> FrotiX.Site.Backup
## Acompanhamento de Progresso

**Data de Início**: 2026-02-05
**Objetivo**: Trazer todas as alterações de código de FrotiX.Site para FrotiX.Site.Backup preservando o modelo de licenciamento Telerik/Kendo/Syncfusion funcional.

---

## Resumo Geral

| Métrica | Valor |
|---------|-------|
| Total de arquivos listados | 1059 |
| Arquivos idênticos (sem ação) | 25 |
| Arquivos diferentes (a atualizar) | 1010 |
| Arquivos novos (a criar no Backup) | 20 |
| Arquivos ausentes no Site | 4 |
| **Total a processar** | **1030** |

## Progresso Global

```
[░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░] 0/1030 (0%)
```

---

## Regras Telerik/Kendo (Modelo Antigo a Preservar)

| Aspecto | Backup (MANTER) | Site (NÃO USAR) |
|---------|-----------------|-----------------|
| Kendo JS | CDN `kendo.cdn.telerik.com` | Local `~/lib/kendo/` |
| Syncfusion CSS | CDN `cdn.syncfusion.com` | unpkg |
| Syncfusion JS | CDN `cdn.syncfusion.com/ej2/32.1.19` | unpkg `@syncfusion/ej2@32.1.23` |
| CLDR Loading | Síncrono (Syncfusion CDN) | Async (jsdelivr) |
| Telerik Report Viewer | v18.1.24.514 | v19.1.25.521 |
| Syncfusion License Key | `...VkdwXkVWYEs=` | `...RmRcUkFxWEtWYEs=` |
| kendo-error-suppressor.js | NÃO existe | Carregado primeiro |

---

## Lotes de Processamento

### Lote 00 (15 arquivos) - Status: PENDENTE
- Areas/Authorization/Pages/_ViewImports.cshtml
- Areas/Authorization/Pages/_ViewStart.cshtml
- Areas/Authorization/Pages/Roles.cshtml
- Areas/Authorization/Pages/Roles.cshtml.cs
- Areas/Authorization/Pages/Users.cshtml
- Areas/Authorization/Pages/Users.cshtml.cs
- Areas/Authorization/Pages/Usuarios.cshtml
- Areas/Authorization/Pages/Usuarios.cshtml.cs
- Areas/Identity/Pages/Account/_ViewImports.cshtml
- Areas/Identity/Pages/Account/ConfirmEmail.cshtml
- Areas/Identity/Pages/Account/ConfirmEmail.cshtml.cs
- Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml
- Areas/Identity/Pages/Account/ConfirmEmailChange.cshtml.cs
- Areas/Identity/Pages/Account/ForgotPassword.cshtml
- Areas/Identity/Pages/Account/ForgotPassword.cshtml.cs

---

## Arquivos com Referências Telerik/Kendo (Atenção Especial)

Estes 45 arquivos contêm referências a Telerik/Kendo e requerem análise cuidadosa para preservar o modelo antigo:

1. Controllers/ReportsController.cs
2. Models/Cadastros/ViagemIndex.js
3. Pages/_ViewImports.cshtml
4. Pages/Abastecimento/DashboardAbastecimento.cshtml
5. Pages/Abastecimento/RegistraCupons.cshtml
6. Pages/Agenda/Index.cshtml
7. Pages/AtaRegistroPrecos/Upsert.cshtml
8. Pages/Manutencao/ControleLavagem.cshtml
9. Pages/Manutencao/DashboardLavagem.cshtml
10. Pages/Manutencao/Upsert.cshtml
11. Pages/Motorista/DashboardMotoristas.cshtml
12. Pages/Multa/UpsertAutuacao.cshtml
13. Pages/Relatorio/TesteRelatorio.cshtml
14. Pages/Shared/_Head.cshtml
15. Pages/Shared/_Layout.cshtml
16. Pages/Shared/_ScriptsBasePlugins.cshtml
17. Pages/Shared/Components/Navigation/TreeView.cshtml
18. Pages/TaxiLeg/Importacao.cshtml
19. Pages/Temp/Index.cshtml
20. Pages/Uploads/UploadPDF.cshtml
21. Pages/Veiculo/DashboardVeiculos.cshtml
22. Pages/Viagens/DashboardEventos.cshtml
23. Pages/Viagens/DashboardViagens.cshtml
24. Pages/Viagens/Index.cshtml
25. Pages/Viagens/Upsert.cshtml
26. Services/CustomReportSourceResolver.cs
27. Services/TelerikReportWarmupService.cs
28. wwwroot/js/agendamento/components/event-handlers.js
29. wwwroot/js/agendamento/components/evento.js
30. wwwroot/js/agendamento/components/exibe-viagem.js
31. wwwroot/js/agendamento/components/modal-viagem-novo.js
32. wwwroot/js/agendamento/components/relatorio.js
33. wwwroot/js/agendamento/components/reportviewer-close-guard.js
34. wwwroot/js/agendamento/components/validacao.js
35. wwwroot/js/agendamento/main.js
36. wwwroot/js/agendamento/utils/kendo-editor-helper.js
37. wwwroot/js/cadastros/agendamento_viagem.js
38. wwwroot/js/cadastros/ViagemIndex.js
39. wwwroot/js/cadastros/ViagemUpsert.js
40. wwwroot/js/frotix.js
41. wwwroot/js/viagens/kendo-editor-upsert.js

---

## Arquivos Novos (a criar no Backup)

1. Controllers/LogErrosController.Dashboard.cs
2. Models/Api/ApiResponse.cs
3. Models/LogErro.cs
4. Pages/Administracao/LogErrosDashboard.cshtml
5. Pages/Administracao/LogErrosDashboard.cshtml.cs
6. Pages/Viagens/ItensPendentes.cshtml
7. Pages/Viagens/ItensPendentes.cshtml.cs
8. Repository/IRepository/ILogRepository.cs
9. Repository/LogRepository.cs
10. Services/ClaudeAnalysisService.cs
11. Services/IClaudeAnalysisService.cs
12. Services/LogErrosAlertService.cs
13. Services/LogErrosCleanupService.cs
14. Services/LogErrosExportService.cs
15. wwwroot/js/agendamento/components/controls-init.js
16. wwwroot/js/agendamento/components/recorrencia-init.js
17. wwwroot/js/agendamento/components/recorrencia-logic.js
18. wwwroot/js/agendamento/components/recorrencia.js
19. wwwroot/js/frotix-api-client.js
20. wwwroot/js/global-error-handler.js

---

## Referências Telerik Suprimidas

| Arquivo | Linha | Conteúdo Original | Ação |
|---------|-------|--------------------|------|
| *(será preenchido durante o processamento)* | | | |

---

## Log de Processamento por Lote

*(será preenchido à medida que os agentes completarem)*

