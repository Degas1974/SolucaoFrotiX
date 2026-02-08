# Controllers/AbastecimentoController.Import.cs

**Mudanca:** GRANDE | **+230** linhas | **-88** linhas

---

```diff
--- JANEIRO: Controllers/AbastecimentoController.Import.cs
+++ ATUAL: Controllers/AbastecimentoController.Import.cs
@@ -18,14 +18,13 @@
 using System.Text;
 using System.Threading.Tasks;
 using System.Transactions;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers
 {
 
     public partial class AbastecimentoController : ControllerBase
     {
+
         public class ImportacaoRequest
         {
             public DateTime DataAbastecimento { get; set; }
@@ -173,6 +172,7 @@
                 if (Produto < 0) faltantes.Add("Produto");
                 if (Quantidade < 0) faltantes.Add("Qtde");
                 if (ValorUnitario < 0) faltantes.Add("Valor Unitário");
+
                 return faltantes;
             }
         }
@@ -205,8 +205,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao enviar progresso da importação via SignalR", error, "AbastecimentoController.Import.cs", "EnviarProgresso");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "EnviarProgresso", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "EnviarProgresso", error);
             }
         }
 
@@ -234,8 +233,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao enviar resumo da planilha via SignalR", error, "AbastecimentoController.Import.cs", "EnviarResumoPlnailha");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "EnviarResumoPlnailha", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "EnviarResumoPlnailha", error);
             }
         }
 
@@ -248,6 +246,7 @@
 
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -278,6 +277,7 @@
                 }
 
                 await EnviarProgresso(connectionId, 5, "Recebendo arquivo", $"Arquivo recebido: {file.FileName}");
+
                 await EnviarProgresso(connectionId, 10, "Lendo planilha", "Abrindo arquivo Excel...");
 
                 var resultadoLeitura = LerPlanilhaDinamica(file);
@@ -457,33 +457,34 @@
                                 double limiteSuperior = mediaReferencia * 1.4;
                                 bool consumoDentroDoEsperado = consumoAtual >= limiteInferior && consumoAtual <= limiteSuperior;
 
-                                if (!consumoDentroDoEsperado)
+                                if (consumoDentroDoEsperado)
                                 {
-                                    if (consumoAtual > limiteSuperior)
+
+                                }
+                                else if (consumoAtual > limiteSuperior)
+                                {
+                                    linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l)");
+
+                                    int kmAnteriorSugerido = linha.Km - kmRodadoEsperado;
+                                    if (kmAnteriorSugerido > 0)
                                     {
-                                        linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l)");
-
-                                        int kmAnteriorSugerido = linha.Km - kmRodadoEsperado;
-                                        if (kmAnteriorSugerido > 0)
-                                        {
-                                            linha.TemSugestao = true;
-                                            linha.CampoCorrecao = "KmAnterior";
-                                            linha.ValorAtualErrado = linha.KmAnterior;
-                                            linha.ValorSugerido = kmAnteriorSugerido;
-                                            linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l). Provável erro no KM Anterior.";
-                                        }
+                                        linha.TemSugestao = true;
+                                        linha.CampoCorrecao = "KmAnterior";
+                                        linha.ValorAtualErrado = linha.KmAnterior;
+                                        linha.ValorSugerido = kmAnteriorSugerido;
+                                        linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l). Provável erro no KM Anterior.";
                                     }
-                                    else
-                                    {
-                                        linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l)");
-
-                                        int kmSugerido = linha.KmAnterior + kmRodadoEsperado;
-                                        linha.TemSugestao = true;
-                                        linha.CampoCorrecao = "Km";
-                                        linha.ValorAtualErrado = linha.Km;
-                                        linha.ValorSugerido = kmSugerido;
-                                        linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l). Provável erro no KM Atual.";
-                                    }
+                                }
+                                else
+                                {
+                                    linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l)");
+
+                                    int kmSugerido = linha.KmAnterior + kmRodadoEsperado;
+                                    linha.TemSugestao = true;
+                                    linha.CampoCorrecao = "Km";
+                                    linha.ValorAtualErrado = linha.Km;
+                                    linha.ValorSugerido = kmSugerido;
+                                    linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l). Provável erro no KM Atual.";
                                 }
                             }
                         }
@@ -531,6 +532,8 @@
                     var linhasParaGravar = linhasValidas
                         .Where(l => l.Autorizacao <= 0 || !autorizacoesAbastecimento.Contains(l.Autorizacao))
                         .ToList();
+
+                    int linhasIgnoradasDuplicadas = linhasValidas.Count - linhasParaGravar.Count;
 
                     int linhaGravada = 0;
                     int intervaloGravacao = Math.Max(1, linhasParaGravar.Count / 20);
@@ -630,6 +633,7 @@
 
                     foreach (var linha in linhasComErro)
                     {
+
                         if (linha.Autorizacao > 0 && autorizacoesPendentes.Contains(linha.Autorizacao))
                         {
                             continue;
@@ -791,8 +795,7 @@
             catch (Exception error)
             {
                 await EnviarProgresso(connectionId, 0, "Erro", error.Message);
-                _log.Error(error.Message, error, "AbastecimentoController.Import.cs", "ImportarNovo");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "ImportarNovo", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ImportarNovo", error);
                 return StatusCode(500, new ResultadoImportacao
                 {
                     Sucesso = false,
@@ -818,8 +821,7 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "AbastecimentoController.Import.cs", "DeterminarTipoPendencia");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "DeterminarTipoPendencia", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DeterminarTipoPendencia", error);
                 return "erro";
             }
         }
@@ -869,8 +871,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao processar conversão de data/hora (NPOI)", error, "AbastecimentoController.Import.cs", "ParseDataHora");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "ParseDataHora", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ParseDataHora", error);
                 return null;
             }
         }
@@ -896,8 +897,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro ao normalizar descrição do produto comercial", error, "AbastecimentoController.Import.cs", "LimparProduto");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "LimparProduto", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "LimparProduto", error);
                 return null;
             }
         }
@@ -1023,6 +1023,7 @@
                         var dataCellValue = GetCellStringValueWithDateTime(dataRow.GetCell(mapeamento.Data));
                         if (dataCellValue != null && dataCellValue.Contains(" "))
                         {
+
                             var partes = dataCellValue.Split(' ');
                             dataStr = partes[0];
                             horaStr = partes.Length > 1 ? partes[1] : null;
@@ -1038,6 +1039,7 @@
 
                         if (!string.IsNullOrEmpty(produtoRaw))
                         {
+
                             if (System.Text.RegularExpressions.Regex.IsMatch(produtoRaw, @"^\d{1,2}-"))
                             {
                                 produtoLimpo = System.Text.RegularExpressions.Regex.Replace(produtoRaw, @"^\d{1,2}-", "").Trim();
@@ -1069,8 +1071,7 @@
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "AbastecimentoController.Import.cs", "LerPlanilhaDinamica");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "LerPlanilhaDinamica", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "LerPlanilhaDinamica", error);
                 return new ResultadoLeituraPlanilha
                 {
                     Sucesso = false,
@@ -1127,6 +1128,7 @@
                     case CellType.Numeric:
                         if (DateUtil.IsCellDateFormatted(cell))
                         {
+
                             return cell.DateCellValue.ToString("dd/MM/yyyy HH:mm");
                         }
                         return cell.NumericCellValue.ToString();
@@ -1207,26 +1209,6 @@
             }
         }
 
-        private DateTime? GetCellDateTimeValue(ICell cell)
-        {
-            try
-            {
-                if (cell == null) return null;
-
-                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
-                {
-                    return cell.DateCellValue;
-                }
-
-                string strVal = GetCellStringValueWithDateTime(cell);
-                return ParseDataHora(strVal, null);
-            }
-            catch
-            {
-                return null;
-            }
-        }
-
         private async Task<Dictionary<int, LinhaCsv>> LerArquivoCsvAsync(IFormFile file, string connectionId = null)
         {
             try
@@ -1268,8 +1250,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro crítico ao ler arquivo CSV de exportação comercial", error, "AbastecimentoController.Import.cs", "LerArquivoCsvAsync");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "LerArquivoCsvAsync", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "LerArquivoCsv", error);
                 return new Dictionary<int, LinhaCsv>();
             }
         }
@@ -1411,8 +1392,7 @@
             }
             catch (Exception error)
             {
-                _log.Error("Erro crítico ao ler arquivo XLSX de apoio comercial (NPOI)", error, "AbastecimentoController.Import.cs", "LerArquivoXlsxAsync");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "LerArquivoXlsxAsync", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "LerArquivoXlsx", error);
                 return new Dictionary<int, LinhaXlsx>();
             }
         }
@@ -1425,6 +1405,7 @@
 
             try
             {
+
                 ModelState.Remove("Veiculo");
                 ModelState.Remove("Motorista");
                 ModelState.Remove("Combustivel");
@@ -1467,6 +1448,7 @@
                 nomeCsv = arquivoCsv.FileName;
 
                 await EnviarProgresso(connectionId, 5, "Arquivos recebidos", $"XLSX: {nomeXlsx}, CSV: {nomeCsv}");
+
                 await EnviarProgresso(connectionId, 7, "Lendo XLSX", "Extraindo Data/Hora e Autorizações...");
 
                 var dadosXlsx = await LerArquivoXlsxAsync(arquivoXlsx, connectionId);
@@ -1480,6 +1462,7 @@
                 }
 
                 await EnviarProgresso(connectionId, 10, "XLSX lido", $"{dadosXlsx.Count} autorizações encontradas");
+
                 await EnviarProgresso(connectionId, 12, "Lendo CSV", "Extraindo dados de abastecimento...");
 
                 var dadosCsv = await LerArquivoCsvAsync(arquivoCsv, connectionId);
@@ -1493,6 +1476,7 @@
                 }
 
                 await EnviarProgresso(connectionId, 15, "CSV lido", $"{dadosCsv.Count} abastecimentos encontrados");
+
                 await EnviarProgresso(connectionId, 17, "Combinando dados", "Fazendo JOIN por Autorização...");
 
                 var mapaCombustivel = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase)
@@ -1677,33 +1661,34 @@
                             double limiteSuperior = mediaReferencia * 1.4;
                             bool consumoDentroDoEsperado = consumoAtual >= limiteInferior && consumoAtual <= limiteSuperior;
 
-                            if (!consumoDentroDoEsperado)
-                            {
-                                if (consumoAtual > limiteSuperior)
+                            if (consumoDentroDoEsperado)
+                            {
+
+                            }
+                            else if (consumoAtual > limiteSuperior)
+                            {
+                                linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l)");
+
+                                int kmAnteriorSugerido = linha.Km - kmRodadoEsperado;
+                                if (kmAnteriorSugerido > 0)
                                 {
-                                    linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l)");
-
-                                    int kmAnteriorSugerido = linha.Km - kmRodadoEsperado;
-                                    if (kmAnteriorSugerido > 0)
-                                    {
-                                        linha.TemSugestao = true;
-                                        linha.CampoCorrecao = "KmAnterior";
-                                        linha.ValorAtualErrado = linha.KmAnterior;
-                                        linha.ValorSugerido = kmAnteriorSugerido;
-                                        linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l). Provável erro no KM Anterior.";
-                                    }
+                                    linha.TemSugestao = true;
+                                    linha.CampoCorrecao = "KmAnterior";
+                                    linha.ValorAtualErrado = linha.KmAnterior;
+                                    linha.ValorSugerido = kmAnteriorSugerido;
+                                    linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito acima da média ({mediaReferencia:N1} km/l). Provável erro no KM Anterior.";
                                 }
-                                else
-                                {
-                                    linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l)");
-
-                                    int kmSugerido = linha.KmAnterior + kmRodadoEsperado;
-                                    linha.TemSugestao = true;
-                                    linha.CampoCorrecao = "Km";
-                                    linha.ValorAtualErrado = linha.Km;
-                                    linha.ValorSugerido = kmSugerido;
-                                    linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l). Provável erro no KM Atual.";
-                                }
+                            }
+                            else
+                            {
+                                linha.Erros.Add($"Quilometragem de {linha.KmRodado} km excede o limite e consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l)");
+
+                                int kmSugerido = linha.KmAnterior + kmRodadoEsperado;
+                                linha.TemSugestao = true;
+                                linha.CampoCorrecao = "Km";
+                                linha.ValorAtualErrado = linha.Km;
+                                linha.ValorSugerido = kmSugerido;
+                                linha.JustificativaSugestao = $"Consumo de {consumoAtual:N1} km/l está muito abaixo da média ({mediaReferencia:N1} km/l). Provável erro no KM Atual.";
                             }
                         }
                     }
@@ -1751,6 +1736,7 @@
                         .Where(l => l.Autorizacao <= 0 || !autorizacoesAbastecimento.Contains(l.Autorizacao))
                         .ToList();
 
+                    int linhasIgnoradasDuplicadas = linhasValidas.Count - linhasParaGravar.Count;
                     int linhaGravada = 0;
                     int intervaloGravacao = Math.Max(1, linhasParaGravar.Count / 20);
 
@@ -1798,8 +1784,8 @@
                                 Produto = linha.Produto,
                                 ValorUnitario = linha.ValorUnitario.ToString("C2", new CultureInfo("pt-BR")),
                                 Quantidade = linha.Quantidade.ToString("N2"),
-                                ValorTotal = linha.ValorTotal.ToString("C2", new CultureInfo("pt-BR")),
-                                Consumo = linha.Consumo.ToString("N2") + " km/l",
+                                ValorTotal = (linha.ValorUnitario * linha.Quantidade).ToString("C2", new CultureInfo("pt-BR")),
+                                Consumo = (linha.Quantidade > 0 && linha.KmRodado > 0 ? linha.KmRodado / linha.Quantidade : 0).ToString("N2") + " km/l",
                                 DataHora = linha.DataHoraParsed.Value.ToString("dd/MM/yyyy HH:mm")
                             });
                         }
@@ -1887,7 +1873,7 @@
                             MediaConsumoVeiculo = linha.MediaConsumoVeiculo,
                             DataImportacao = DateTime.Now,
                             NumeroLinhaOriginal = linha.NumeroLinhaOriginal,
-                            ArquivoOrigem = $"{nomeXlsx} + {nomeCsv}",
+                            ArquivoOrigem = $"{nomeCsv} + {nomeXlsx}",
                             Status = 0
                         };
 
@@ -2010,13 +1996,220 @@
             catch (Exception error)
             {
                 await EnviarProgresso(connectionId, 0, "Erro", error.Message);
-                _log.Error("Erro crítico no processo de importação Dual (CSV+XLSX)", error, "AbastecimentoController.Import.cs", "ImportarDualInternal");
-                Alerta.TratamentoErroComLinha("AbastecimentoController.Import.cs", "ImportarDualInternal", error);
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ImportarDual", error);
                 return StatusCode(500, new ResultadoImportacao
                 {
                     Sucesso = false,
-                    Mensagem = $"Erro interno ao processar importação dual: {error.Message}"
+                    Mensagem = $"Erro interno ao processar importação: {error.Message}"
                 });
+            }
+        }
+
+        private DateTime? GetCellDateTimeValue(ICell cell)
+        {
+            try
+            {
+                if (cell == null) return null;
+
+                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
+                {
+                    return cell.DateCellValue;
+                }
+
+                string valor = cell.ToString()?.Trim();
+                if (!string.IsNullOrWhiteSpace(valor))
+                {
+                    if (DateTime.TryParse(valor, new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime result))
+                    {
+                        return result;
+                    }
+                }
+
+                return null;
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "GetCellDateTimeValue", error);
+                return null;
+            }
+        }
+
+        [Route("ExcluirPorData")]
+        [HttpPost]
+        public IActionResult ExcluirPorData([FromBody] ImportacaoRequest request)
+        {
+            try
+            {
+                var dataInicio = request.DataAbastecimento.Date;
+                var dataFim = dataInicio.AddDays(1);
+
+                var registros = _unitOfWork.Abastecimento.GetAll()
+                    .Where(a => a.DataHora >= dataInicio && a.DataHora < dataFim)
+                    .ToList();
+
+                if (!registros.Any())
+                {
+                    return Ok(new
+                    {
+                        success = false,
+                        message = $"Nenhum registro encontrado para {dataInicio:dd/MM/yyyy}"
+                    });
+                }
+
+                int quantidade = registros.Count;
+
+                foreach (var registro in registros)
+                {
+                    _unitOfWork.Abastecimento.Remove(registro);
+                }
+
+                _unitOfWork.Save();
+
+                return Ok(new
+                {
+                    success = true,
+                    message = $"{quantidade} abastecimento(s) excluído(s) com sucesso para {dataInicio:dd/MM/yyyy}"
+                });
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ExcluirPorData", error);
+                return StatusCode(500, new { success = false, message = error.Message });
+            }
+        }
+
+        [Route("ExportarPendencias")]
+        [HttpGet]
+        public IActionResult ExportarPendencias()
+        {
+            try
+            {
+
+                var pendencias = _unitOfWork.AbastecimentoPendente
+                    .GetAll()
+                    .Where(p => p.Status == 0)
+                    .OrderBy(p => p.DataImportacao)
+                    .ThenBy(p => p.AutorizacaoQCard)
+                    .ToList();
+
+                if (!pendencias.Any())
+                {
+                    return NotFound(new { success = false, message = "Nenhuma pendência encontrada para exportar." });
+                }
+
+                var workbook = new XSSFWorkbook();
+                var sheet = workbook.CreateSheet("Pendências");
+
+                var headerStyle = workbook.CreateCellStyle();
+                var headerFont = workbook.CreateFont();
+                headerFont.IsBold = true;
+                headerFont.FontHeightInPoints = 11;
+                headerStyle.SetFont(headerFont);
+                headerStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
+                headerStyle.FillPattern = FillPattern.SolidForeground;
+                headerStyle.BorderBottom = BorderStyle.Thin;
+                headerStyle.BorderTop = BorderStyle.Thin;
+                headerStyle.BorderLeft = BorderStyle.Thin;
+                headerStyle.BorderRight = BorderStyle.Thin;
+
+                var dateStyle = workbook.CreateCellStyle();
+                var dataFormat = workbook.CreateDataFormat();
+                dateStyle.DataFormat = dataFormat.GetFormat("dd/mm/yyyy hh:mm");
+
+                var headerRow = sheet.CreateRow(0);
+                string[] headers = {
+                    "Data Importação",
+                    "Autorização QCard",
+                    "Data/Hora Abast.",
+                    "Placa",
+                    "Cód. Motorista",
+                    "Nome Motorista",
+                    "Produto",
+                    "KM Anterior",
+                    "KM",
+                    "KM Rodado",
+                    "Litros",
+                    "Valor Unitário",
+                    "Tipo Pendência",
+                    "Descrição Pendência",
+                    "Arquivo Origem",
+                    "Linha Original"
+                };
+
+                for (int i = 0; i < headers.Length; i++)
+                {
+                    var cell = headerRow.CreateCell(i);
+                    cell.SetCellValue(headers[i]);
+                    cell.CellStyle = headerStyle;
+                }
+
+                int rowIndex = 1;
+                foreach (var pendencia in pendencias)
+                {
+                    var row = sheet.CreateRow(rowIndex++);
+
+                    var cellDataImportacao = row.CreateCell(0);
+                    cellDataImportacao.SetCellValue(pendencia.DataImportacao);
+                    cellDataImportacao.CellStyle = dateStyle;
+
+                    row.CreateCell(1).SetCellValue(pendencia.AutorizacaoQCard ?? 0);
+
+                    if (pendencia.DataHora.HasValue)
+                    {
+                        var cellDataHora = row.CreateCell(2);
+                        cellDataHora.SetCellValue(pendencia.DataHora.Value);
+                        cellDataHora.CellStyle = dateStyle;
+                    }
+                    else
+                    {
+                        row.CreateCell(2).SetCellValue("");
+                    }
+
+                    row.CreateCell(3).SetCellValue(pendencia.Placa ?? "");
+
+                    row.CreateCell(4).SetCellValue(pendencia.CodMotorista ?? 0);
+
+                    row.CreateCell(5).SetCellValue(pendencia.NomeMotorista ?? "");
+
+                    row.CreateCell(6).SetCellValue(pendencia.Produto ?? "");
+
+                    row.CreateCell(7).SetCellValue(pendencia.KmAnterior ?? 0);
+
+                    row.CreateCell(8).SetCellValue(pendencia.Km ?? 0);
+
+                    row.CreateCell(9).SetCellValue(pendencia.KmRodado ?? 0);
+
+                    row.CreateCell(10).SetCellValue(pendencia.Litros ?? 0);
+
+                    row.CreateCell(11).SetCellValue(pendencia.ValorUnitario ?? 0);
+
+                    row.CreateCell(12).SetCellValue(pendencia.TipoPendencia ?? "");
+
+                    row.CreateCell(13).SetCellValue(pendencia.DescricaoPendencia ?? "");
+
+                    row.CreateCell(14).SetCellValue(pendencia.ArquivoOrigem ?? "");
+
+                    row.CreateCell(15).SetCellValue(pendencia.NumeroLinhaOriginal);
+                }
+
+                for (int i = 0; i < headers.Length; i++)
+                {
+                    sheet.AutoSizeColumn(i);
+                }
+
+                using (var memoryStream = new MemoryStream())
+                {
+                    workbook.Write(memoryStream);
+                    var excelBytes = memoryStream.ToArray();
+
+                    string nomeArquivo = $"Pendencias_Abastecimento_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
+                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeArquivo);
+                }
+            }
+            catch (Exception error)
+            {
+                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ExportarPendencias", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
         }
     }
```
