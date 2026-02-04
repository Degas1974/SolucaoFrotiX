# Documentacao: LogErrosExportService.cs

> **Ultima Atualizacao**: 04/02/2026
> **Versao Atual**: 1.0

---

## Visao Geral

Servico responsavel por exportar logs e relatorios gerenciais em Excel e PDF. Usa ClosedXML para planilhas e QuestPDF para documentos PDF.

## Responsabilidades

- Exportar logs filtrados em Excel e CSV.
- Gerar relatorio executivo em PDF e Excel.
- Manter cores e estilos padronizados do FrotiX.

## Principais Metodos

### ExportLogsToExcelAsync(LogQueryFilter filter)

Carrega logs e gera planilha com cabecalhos, dados e formatacao.

### ExportLogsToCsvAsync(LogQueryFilter filter)

Exporta logs em CSV com campos delimitados por ponto e virgula.

### ExportExecutiveReportPdfAsync(DateTime startDate, DateTime endDate)

Gera PDF com seccoes de KPIs, comparativos e distribuicoes.

### ExportExecutiveReportExcelAsync(DateTime startDate, DateTime endDate)

Gera Excel com abas para resumo, rankings e timeline.

## Trecho Critico (Tratamento de Erro)

```csharp
try
{
    // exportacao
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro ao exportar logs para Excel");
    throw;
}
```

## Ajuste Recente

Remocao de campos de cor nao utilizados para eliminar warning CS0414.

---

## Log de Modificacoes

| Versao | Data       | Autor          | Descricao |
|--------|------------|----------------|-----------|
| 1.0    | 04/02/2026 | GitHub Copilot | Remove campos de cor nao usados para eliminar CS0414. |
