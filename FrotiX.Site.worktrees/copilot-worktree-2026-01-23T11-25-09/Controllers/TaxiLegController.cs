using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  IMPORTAÇÃO TAXI LEG (CORE)                                                         #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: TaxiLegController                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Importação, auditoria e gestão de corridas da plataforma Taxi Leg.        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/TaxiLeg                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class TaxiLegController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorridasTaxiLegRepository _corridasTaxiLegRepository;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: TaxiLegController (Construtor)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa ambiente, repositórios e serviço de log.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • hostingEnvironment (IWebHostEnvironment): WebRoot/hosting.             ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • corridasTaxiLegRepository (ICorridasTaxiLegRepository): Repositório.    ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public TaxiLegController(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, ICorridasTaxiLegRepository corridasTaxiLegRepository, ILogService log)
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _corridasTaxiLegRepository = corridasTaxiLegRepository;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Constructor", ex);
            }
        }

        #region ==================== PROPERTIES ====================

        [BindProperty]
        public Models.CorridasTaxiLeg TaxiLegObj { get; set; }
        public Models.CorridasCanceladasTaxiLeg TaxiLegCanceladasObj { get; set; }

        #endregion

        #region ==================== VIEW ACTIONS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Index                                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza a view principal de auditoria Taxi Leg.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: View principal.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public IActionResult Index()
        {
            try
            {
                // [LOG] Acesso ao console.
                _log.Info("TaxiLegController.Index: Acessando console de auditoria Taxi Leg.");
                // [RETORNO] View principal.
                return View();
            }
            catch (Exception ex)
            {
                _log.Error("TaxiLegController.Index", ex);
                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Index", ex);
                // [RETORNO] View fallback.
                return View();
            }
        }

        #endregion

        #region ==================== API ACTIONS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Endpoint de verificação do controlador.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [LOG] Checkpoint de conexão.
                _log.Info("TaxiLegController.Get: Checkpoint de conexão bem-sucedido.");
                // [RETORNO] OK.
                return Json(true);
            }
            catch (Exception ex)
            {
                _log.Error("TaxiLegController.Get", ex);
                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Get", ex);
                return Json(new { success = false });
            }
        }

        #endregion

        #region ==================== IMPORTAÇÃO (CORE LOGIC) ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Import (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa upload e parsing de planilhas Excel Taxi Leg.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com status e dados importados.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Import")]
        [HttpPost]
        public ActionResult Import()
        {
            try
            {
                // [ARQUIVO] Obtém arquivo enviado.
                IFormFile file = Request.Form.Files[0];
                // [DADOS] Define pasta de upload.
                string folderName = "DadosEditaveis/UploadExcel";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                // [REGRA] Garante diretório.
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                // [DADOS] Lista de corridas.
                var listaCorridas = new List<CorridasTaxiLeg>();

                if (file.Length > 0)
                {
                    // [ARQUIVO] Extensão e caminho completo.
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        // [ARQUIVO] Salva upload em disco.
                        file.CopyTo(stream);
                        stream.Position = 0;

                        // [REGRA] Leitura de planilha conforme extensão.
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else
                        {
                            XSSFWorkbook xssfwb = new XSSFWorkbook(stream);
                            sheet = xssfwb.GetSheetAt(0);
                        }

                        // [REGRA] Busca a primeira data válida para extrair ano/mês.
                        DateTime? primeiraDataAgenda = null;
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;

                            if (DateTime.TryParse(row.GetCell(7)?.ToString(), out var dataAgenda))
                            {
                                primeiraDataAgenda = dataAgenda;
                                break;
                            }
                        }

                        if (primeiraDataAgenda == null)
                        {
                            // [RETORNO] Sem datas válidas.
                            _log.Warning("TaxiLegController.Import: Planilha sem datas válidas na coluna 7.");
                            return Json(new { success = false, message = "Não foi possível determinar a data das corridas." });
                        }

                        // [DADOS] Ano e mês da importação.
                        var ano = primeiraDataAgenda.Value.Year;
                        var mes = primeiraDataAgenda.Value.Month;
                        // [REGRA] Bloqueia reimportação do mês/ano.
                        if (_corridasTaxiLegRepository.ExisteCorridaNoMesAno(ano, mes))
                        {
                            _log.Warning($"TaxiLegController.Import: O mês {mes:D2}/{ano} já consta como importado.");
                            return Json(new { success = false, message = $"O mês {mes:D2}/{ano} já foi importado!" });
                        }

                        // [LOG] Início do parsing.
                        _log.Info($"TaxiLegController.Import: Iniciando parsing de {sheet.LastRowNum} linhas para {mes:00}/{ano}.");

                        // [PROCESSAMENTO] Linhas da planilha.
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (
                                row == null
                                || row.Cells.TrueForAll(cell =>
                                    cell == null || cell.CellType == CellType.Blank
                                )
                            )
                                continue;

                            // [DADOS] Monta objeto da corrida.
                            var TaxiLegObj = new CorridasTaxiLeg();

                            TaxiLegObj.QRU = row.GetCell(0)?.ToString() ?? "";
                            TaxiLegObj.DescSetor = row.GetCell(1)?.ToString() ?? "";
                            TaxiLegObj.Setor = row.GetCell(2)?.ToString() ?? "";
                            TaxiLegObj.Unidade = row.GetCell(3)?.ToString() ?? "";
                            TaxiLegObj.DescUnidade = row.GetCell(4)?.ToString() ?? "";
                            TaxiLegObj.QtdPassageiros = int.TryParse(
                                row.GetCell(5)?.ToString() ,
                                out var passageiros
                            )
                                ? passageiros
                                : 1;
                            TaxiLegObj.MotivoUso = row.GetCell(6)?.ToString() ?? "";

                            TaxiLegObj.DataAgenda = DateTime.TryParse(
                                row.GetCell(7)?.ToString() ,
                                out var dataAgenda
                            )
                                ? dataAgenda
                                : (DateTime?)null;

                            // [REGRA] Usa função utilitária para hora.
                            TaxiLegObj.HoraAgenda = ExtrairHora(row , 7);
                            TaxiLegObj.HoraAceite = ExtrairHora(row , 8);
                            TaxiLegObj.HoraInicio = ExtrairHora(row , 9);
                            TaxiLegObj.HoraLocal = ExtrairHora(row , 10);
                            TaxiLegObj.HoraFinal = ExtrairHora(row , 11);

                            TaxiLegObj.DataFinal = DateTime.TryParse(
                                row.GetCell(12)?.ToString() ,
                                out var dataFinal
                            )
                                ? dataFinal
                                : (DateTime?)null;

                            TaxiLegObj.OrigemCorrida = row.GetCell(13)?.ToString() ?? "";
                            TaxiLegObj.DestinoCorrida = row.GetCell(14)?.ToString() ?? "";
                            TaxiLegObj.KmReal = double.TryParse(
                                row.GetCell(15)?.ToString() ,
                                out var km
                            )
                                ? km
                                : 0;
                            TaxiLegObj.QtdEstrelas = int.TryParse(
                                row.GetCell(16)?.ToString() ,
                                out var estrelas
                            )
                                ? estrelas
                                : 0;
                            TaxiLegObj.Avaliacao = row.GetCell(17)?.ToString() ?? "";

                            var cultura = new CultureInfo("pt-BR");

                            string dataInicio = TaxiLegObj.DataAgenda?.ToString("dd/MM/yyyy") ?? "";
                            string dataFinalizacao =
                                TaxiLegObj.DataFinal?.ToString("dd/MM/yyyy") ?? "";
                            string horaInicio = TaxiLegObj.HoraInicio?.Trim() ?? "";
                            string horaFinal = TaxiLegObj.HoraFinal?.Trim() ?? "";
                            string horaAceite = TaxiLegObj.HoraAceite?.Trim() ?? "";
                            string horaLocal = TaxiLegObj.HoraLocal?.Trim() ?? "";

                            // [CALCULO] Duração da viagem.
                            TaxiLegObj.Duracao = null;
                            if (
                                DateTime.TryParseExact(
                                    $"{dataInicio} {horaInicio}" ,
                                    "dd/MM/yyyy HH:mm" ,
                                    cultura ,
                                    DateTimeStyles.None ,
                                    out var inicio
                                )
                                && DateTime.TryParseExact(
                                    $"{dataFinalizacao} {horaFinal}" ,
                                    "dd/MM/yyyy HH:mm" ,
                                    cultura ,
                                    DateTimeStyles.None ,
                                    out var fim
                                )
                            )
                            {
                                int duracao = (int)(fim - inicio).TotalMinutes;
                                TaxiLegObj.Duracao = duracao < 0 ? 0 : duracao;
                            }

                            // [CALCULO] Tempo de espera.
                            TaxiLegObj.Espera = null;
                            if (
                                TimeSpan.TryParseExact(
                                    horaAceite ,
                                    "hh\\:mm" ,
                                    cultura ,
                                    out var tsHoraAceite
                                )
                                && TimeSpan.TryParseExact(
                                    horaLocal ,
                                    "hh\\:mm" ,
                                    cultura ,
                                    out var tsHoraLocal
                                )
                                && DateTime.TryParseExact(
                                    $"{dataInicio} {horaAceite}" ,
                                    "dd/MM/yyyy HH:mm" ,
                                    cultura ,
                                    DateTimeStyles.None ,
                                    out var aceite
                                )
                            )
                            {
                                DateTime local;
                                if (tsHoraLocal < tsHoraAceite)
                                    local = aceite.Date.AddDays(1).Add(tsHoraLocal);
                                else
                                    local = aceite.Date.Add(tsHoraLocal);

                                int espera = (int)(local - aceite).TotalMinutes;
                                TaxiLegObj.Espera = espera;
                            }

                            // [REGRA] Cálculo da glosa.
                            TaxiLegObj.Glosa = TaxiLegObj.Espera > 15;
                            TaxiLegObj.ValorGlosa = (bool)TaxiLegObj.Glosa
                                ? Math.Round((double)(TaxiLegObj.KmReal * 2.44), 2)
                                : 0;

                            // [DADOS] Adiciona à lista e persiste.
                            listaCorridas.Add(TaxiLegObj);
                            _unitOfWork.CorridasTaxiLeg.Add(TaxiLegObj);
                        }

                        // [ACAO] Persiste lote.
                        _unitOfWork.Save();
                        _log.Info($"TaxiLegController.Import: Sucesso na importação de {listaCorridas.Count} registros de corridas.");
                    }
                }

                // [REGRA] Ordena saída.
                listaCorridas = listaCorridas.OrderBy(c => c.QRU).ToList();

                // [RETORNO] Resultado da importação.
                return Json(
                    new
                    {
                        success = true,
                        message = "Planilha Importada com Sucesso",
                        data = listaCorridas,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("TaxiLegController.Import", ex);
                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "Import", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao importar planilha"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ImportCanceladas (POST)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Importa corridas canceladas Taxi Leg para auditoria.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com status e conteúdo renderizado.                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ImportCanceladas")]
        [HttpPost]
        public ActionResult ImportCanceladas()
        {
            try
            {
                // [ARQUIVO] Obtém arquivo enviado.
                IFormFile file = Request.Form.Files[0];
                // [DADOS] Define pasta de upload.
                string folderName = "DadosEditaveis/UploadExcel";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();

                // [REGRA] Garante diretório.
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    // [LOG] Início da importação.
                    _log.Info($"TaxiLegController.ImportCanceladas: Iniciando importação de canceladas [{file.FileName}]");
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        // [ARQUIVO] Salva upload em disco.
                        file.CopyTo(stream);
                        stream.Position = 0;
                        // [REGRA] Leitura de planilha conforme extensão.
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else
                        {
                            XSSFWorkbook xssfwb = new XSSFWorkbook(stream);
                            sheet = xssfwb.GetSheetAt(0);
                        }

                        // [DADOS] Monta cabeçalho HTML.
                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        sb.Append("<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            ICell cell = headerRow.GetCell(j);
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("<th>Espera</th></tr></thead><tbody>");

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null || row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            // [DADOS] Monta objeto de cancelamento.
                            TaxiLegCanceladasObj = new CorridasCanceladasTaxiLeg();
                            sb.Append("<tr>");

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                string celulaValor = row.GetCell(j)?.ToString() ?? "N/A";
                                sb.Append("<td>" + celulaValor + "</td>");

                                switch (j)
                                {
                                    case 0: TaxiLegCanceladasObj.Origem = celulaValor; break;
                                    case 1: TaxiLegCanceladasObj.Setor = celulaValor; break;
                                    case 2: TaxiLegCanceladasObj.SetorExtra = celulaValor; break;
                                    case 3: TaxiLegCanceladasObj.Unidade = celulaValor; break;
                                    case 4: TaxiLegCanceladasObj.UnidadeExtra = celulaValor; break;
                                    case 5:
                                        int.TryParse(celulaValor, out int p);
                                        TaxiLegCanceladasObj.QtdPassageiros = p > 0 ? p : 1;
                                        break;
                                    case 6: TaxiLegCanceladasObj.MotivoUso = celulaValor; break;
                                    case 7:
                                        if (DateTime.TryParse(celulaValor, out var dtAlt)) TaxiLegCanceladasObj.DataAgenda = dtAlt;
                                        break;
                                    case 8:
                                        if (DateTime.TryParse(celulaValor, out var hrAlt)) TaxiLegCanceladasObj.HoraAgenda = hrAlt.ToShortTimeString();
                                        break;
                                    case 9:
                                        if (DateTime.TryParse(celulaValor, out var hrCanc))
                                        {
                                            TaxiLegCanceladasObj.DataHoraCancelamento = hrCanc;
                                            TaxiLegCanceladasObj.HoraCancelamento = hrCanc.ToShortTimeString();
                                        }
                                        break;
                                    case 10: TaxiLegCanceladasObj.TipoCancelamento = celulaValor; break;
                                    case 11: TaxiLegCanceladasObj.MotivoCancelamento = celulaValor; break;
                                }
                            }

                            // [CALCULO] Tempo de espera.
                            if (!string.IsNullOrEmpty(TaxiLegCanceladasObj.HoraAgenda) && !string.IsNullOrEmpty(TaxiLegCanceladasObj.HoraCancelamento))
                            {
                                DateTime startTime = DateTime.Parse(TaxiLegCanceladasObj.HoraAgenda);
                                DateTime endTime = DateTime.Parse(TaxiLegCanceladasObj.HoraCancelamento);
                                TimeSpan span = endTime.Subtract(startTime);

                                if (span.TotalMinutes < 0)
                                    TaxiLegCanceladasObj.TempoEspera = (int)span.Add(TimeSpan.FromDays(1)).TotalMinutes;
                                else
                                    TaxiLegCanceladasObj.TempoEspera = (int)span.TotalMinutes;
                            }

                            // [RETORNO] Linha com espera.
                            sb.Append("<td>" + TaxiLegCanceladasObj.TempoEspera + "</td></tr>");
                            _unitOfWork.CorridasCanceladasTaxiLeg.Add(TaxiLegCanceladasObj);
                        }

                        // [ACAO] Persiste lote.
                        _unitOfWork.Save();
                        _log.Info($"TaxiLegController.ImportCanceladas: Sucesso na importação das corridas canceladas.");
                        sb.Append("</tbody></table>");
                    }

                    // [RETORNO] Conteúdo renderizado.
                    return Json(new { success = true, message = "Planilha Importada com Sucesso", response = this.Content(sb.ToString()) });
                }

                // [RETORNO] Arquivo inválido.
                return Json(new { success = false, message = "Arquivo vazio ou inválido." });
            }
            catch (Exception ex)
            {
                _log.Error("TaxiLegController.ImportCanceladas", ex);
                Alerta.TratamentoErroComLinha("TaxiLegController.cs", "ImportCanceladas", ex);
                return Json(new { success = false, message = "Erro ao importar corridas canceladas" });
            }
        }

        #endregion

        #region ==================== HELPERS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExtrairHora (Helper)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Normaliza extração de horário de células Excel.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • row (IRow): Linha da planilha.                                         ║
        /// ║    • cellIndex (int): Índice da célula.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • string: Horário no formato HH:mm ou vazio.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private string ExtrairHora(IRow row, int cellIndex)
        {
            try
            {
                // [DADOS] Célula alvo.
                var cell = row.GetCell(cellIndex);
                if (cell == null) return "";

                // [REGRA] Célula numérica com data.
                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue.ToString("HH:mm");
                }

                // [DADOS] Valor texto.
                string valor = cell.ToString();
                if (string.IsNullOrEmpty(valor)) return "";

                // [REGRA] Parse de horário.
                if (DateTime.TryParse(valor, out var dt))
                {
                    return dt.ToString("HH:mm");
                }

                // [RETORNO] Valor original.
                return valor;
            }
            catch (Exception ex)
            {
                _log.Error("TaxiLegController.ExtrairHora", ex);
                return "";
            }
        }

        #endregion
    }
}
