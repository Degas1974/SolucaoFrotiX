/* ****************************************************************************************
 * ⚡ ARQUIVO: AbastecimentoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Controller principal de gestão de abastecimentos, concentrando CRUD,
 *                   views e operações auxiliares do módulo.
 *
 * 📥 ENTRADAS     : Requisições HTTP (GET/POST) para listagem, criação, edição e exclusão.
 *
 * 📤 SAÍDAS       : Views Razor e respostas JSON para o frontend.
 *
 * 🔗 CHAMADA POR  : UI de cadastros/relatórios e rotas do módulo Abastecimento.
 *
 * 🔄 CHAMA        : Partials associados (DashboardAPI, Import, Pendencias) e UnitOfWork.
 *
 * 📦 DEPENDÊNCIAS : IUnitOfWork, SignalR, FrotiXDbContext, NPOI, ILogger.
 *
 * 📝 OBSERVAÇÕES  : Este controller é parcial e é complementado por arquivos .DashboardAPI,
 *                   .Import e .Pendencias.
 **************************************************************************************** */

using FrotiX.Data;
using FrotiX.Hubs;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ PARTIAL CLASS: AbastecimentoController (Principal)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Controller principal de gestão de abastecimentos
     * 📥 ENTRADAS     : Requisições HTTP GET/POST para CRUD de abastecimentos
     * 📤 SAÍDAS       : JSON com dados de abastecimentos e views
     * 🔗 CHAMADA POR  : Frontend de cadastros e relatórios
     * 🔄 CHAMA        : UnitOfWork, ViewAbastecimentos, SignalR
     * 📦 DEPENDÊNCIAS : Entity Framework, Logger, IHubContext, FrotiXDbContext
     * --------------------------------------------------------------------------------------
     * [DOC] Classe parcial principal com construtores e métodos básicos (Index, Get)
     * [DOC] Possui classes parciais: .Import.cs, .Pendencias.cs, .DashboardAPI.cs
     * [DOC] BindProperty REMOVIDO para evitar validação global indesejada em endpoints API
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public partial class AbastecimentoController :ControllerBase
    {
        private readonly ILogger<AbastecimentoController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ImportacaoHub> _hubContext;
        private readonly FrotiXDbContext _context;

        public AbastecimentoController(
            ILogger<AbastecimentoController> logger ,
            IWebHostEnvironment hostingEnvironment ,
            IUnitOfWork unitOfWork ,
            IHubContext<ImportacaoHub> hubContext,
            FrotiXDbContext context
        )
        {
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _hubContext = hubContext;
                _context = context;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoController" ,
                    error
                );
            }
        }

        // [BindProperty] - REMOVIDO: Causava validação global indesejada em endpoints API
        public Models.Abastecimento AbastecimentoObj
        {
            get; set;
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Index
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Endpoint raiz da API de abastecimento
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : StatusCode 200 (OK)
         *
         * ⬅️ CHAMADO POR  : Rotas da API REST
         *
         * ➡️ CHAMA        : Nenhuma dependência
         *
         * 📝 OBSERVAÇÕES  : Endpoint básico para verificar status do controller
         ****************************************************************************************/
        public IActionResult Index()
        {
            try
            {
                return StatusCode(200);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Index" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista completa de abastecimentos ordenados por data/hora
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewAbastecimento> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento [linha 112]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewAbastecimentos.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Ordena por DataHora decrescente (mais recentes primeiro)
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DB] Buscar todos os abastecimentos com ordenação por data/hora decrescente
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Get" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AbastecimentoVeiculos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar abastecimentos filtrados por veículo específico
         *
         * 📥 ENTRADAS     : Id [Guid] - ID do veículo
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewAbastecimento> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/AbastecimentoVeiculos?Id=xxx [linha 134]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewAbastecimentos.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Filtra por VeiculoId e ordena por data/hora
         ****************************************************************************************/
        [Route("AbastecimentoVeiculos")]
        [HttpGet]
        public IActionResult AbastecimentoVeiculos(Guid Id)
        {
            try
            {
                // [DB] Filtrar abastecimentos do veículo específico e ordenar por data/hora decrescente
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.VeiculoId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoVeiculos" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AbastecimentoCombustivel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar abastecimentos filtrados por tipo de combustível
         *
         * 📥 ENTRADAS     : Id [Guid] - ID do combustível
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewAbastecimento> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/AbastecimentoCombustivel?Id=xxx [linha 162]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewAbastecimentos.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Filtra por CombustivelId e ordena por data/hora
         ****************************************************************************************/
        [Route("AbastecimentoCombustivel")]
        [HttpGet]
        public IActionResult AbastecimentoCombustivel(Guid Id)
        {
            try
            {
                // [DB] Filtrar abastecimentos por tipo de combustível e ordenar por data/hora decrescente
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.CombustivelId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoCombustivel" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AbastecimentoUnidade
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar abastecimentos filtrados por unidade de negócio
         *
         * 📥 ENTRADAS     : Id [Guid] - ID da unidade
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewAbastecimento> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/AbastecimentoUnidade?Id=xxx [linha 190]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewAbastecimentos.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Filtra por UnidadeId e ordena por data/hora
         ****************************************************************************************/
        [Route("AbastecimentoUnidade")]
        [HttpGet]
        public IActionResult AbastecimentoUnidade(Guid Id)
        {
            try
            {
                // [DB] Filtrar abastecimentos da unidade e ordenar por data/hora decrescente
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.UnidadeId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoUnidade" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AbastecimentoMotorista
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar abastecimentos filtrados por motorista
         *
         * 📥 ENTRADAS     : Id [Guid] - ID do motorista
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewAbastecimento> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/AbastecimentoMotorista?Id=xxx [linha 218]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewAbastecimentos.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Filtra por MotoristaId e ordena por data/hora
         ****************************************************************************************/
        [Route("AbastecimentoMotorista")]
        [HttpGet]
        public IActionResult AbastecimentoMotorista(Guid Id)
        {
            try
            {
                // [DB] Filtrar abastecimentos do motorista e ordenar por data/hora decrescente
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.MotoristaId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoMotorista" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AbastecimentoData
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar abastecimentos filtrados por data específica
         *
         * 📥 ENTRADAS     : dataAbastecimento [string] - Data no formato esperado pelo sistema
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewAbastecimento> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/AbastecimentoData?dataAbastecimento=xxx [linha 246]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewAbastecimentos.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Filtra por campo Data (string) e ordena por data/hora decrescente
         ****************************************************************************************/
        [Route("AbastecimentoData")]
        [HttpGet]
        public IActionResult AbastecimentoData(string dataAbastecimento)
        {
            try
            {
                // [DB] Filtrar abastecimentos pela data e ordenar por data/hora decrescente
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.Data == dataAbastecimento)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new
                {
                    data = dados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "AbastecimentoData" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Import
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Importar dados de abastecimento de arquivo Excel/XLS
         *
         * 📥 ENTRADAS     : Arquivo Excel (multipart form-data) via Request.Form.Files[0]
         *                   Esperado formato com colunas: Data, Veículo, Motorista, Km Hodômetro,
         *                   Km Rodado, Combustível, Valor Unitário, Litros
         *
         * 📤 SAÍDAS       : JsonResult { success: bool, message: string, response: HTML }
         *                   response: Tabela HTML com preview dos dados importados
         *
         * ⬅️ CHAMADO POR  : Frontend via POST /api/Abastecimento/Import com arquivo [linha 274]
         *
         * ➡️ CHAMA        : _unitOfWork.Abastecimento.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.Veiculo.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.Motorista.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.ViewMediaConsumo.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.Abastecimento.Add() [Repository]
         *                   _unitOfWork.Save() [UnitOfWork]
         *
         * 📝 OBSERVAÇÕES  : Operação com TransactionScope de 30 segundos
         *                   Valida existência de veículo e motorista antes de importar
         *                   Calcula consumo (km/litros) automaticamente
         ****************************************************************************************/
        [Route("Import")]
        [HttpPost]
        public ActionResult Import()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = "DadosEditaveis/UploadExcel";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath , folderName);
                StringBuilder sb = new StringBuilder();

                // [UI] Criar diretório se não existir
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath , file.FileName);

                    using (var stream = new FileStream(fullPath , FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;

                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }

                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        sb.Append(
                            "<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>"
                        );

                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (
                                j == 5
                                || j == 7
                                || j == 10
                                || j == 11
                                || j == 12
                                || j == 13
                                || j == 14
                                || j == 15
                            )
                            {
                                sb.Append("<th>" + cell.ToString() + "</th>");
                            }
                        }

                        sb.Append("<th>" + "Consumo" + "</th>");
                        sb.Append("<th>" + "Média" + "</th>");
                        sb.Append("</tr></thead>");

                        try
                        {
                            using (
                                TransactionScope scope = new TransactionScope(
                                    TransactionScopeOption.RequiresNew ,
                                    new TimeSpan(0 , 30 , 30)
                                )
                            )
                            {
                                sb.AppendLine("<tbody><tr>");

                                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (row == null)
                                        continue;
                                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                                        continue;

                                    AbastecimentoObj = new Abastecimento();

                                    for (int j = row.FirstCellNum; j < cellCount; j++)
                                    {
                                        if (row.GetCell(j) != null)
                                        {
                                            if (i == 1)
                                            {
                                                if (j == 0)
                                                {
                                                    var objFromDb =
                                                        _unitOfWork.Abastecimento.GetFirstOrDefault(
                                                            u =>
                                                                u.DataHora
                                                                == Convert.ToDateTime(
                                                                    row.GetCell(j).ToString()
                                                                )
                                                        );
                                                    if (objFromDb != null)
                                                    {
                                                        return Ok(
                                                            new
                                                            {
                                                                success = false ,
                                                                message = "Os registros para o dia "
                                                                    + Convert.ToDateTime(
                                                                        row.GetCell(j).ToString()
                                                                    )
                                                                    + " já foram importados!" ,
                                                            }
                                                        );
                                                    }
                                                }
                                            }

                                            if (j == 7)
                                            {
                                                AbastecimentoObj.DataHora = Convert.ToDateTime(
                                                    row.GetCell(j).ToString()
                                                );
                                                sb.Append(
                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
                                                );
                                            }

                                            if (j == 5)
                                            {
                                                string placaVeiculo = row.GetCell(j).ToString();

                                                var veiculoObj =
                                                    _unitOfWork.Veiculo.GetFirstOrDefault(m =>
                                                        m.Placa == placaVeiculo
                                                    );
                                                if (veiculoObj != null)
                                                {
                                                    AbastecimentoObj.VeiculoId =
                                                        veiculoObj.VeiculoId;
                                                    sb.Append(
                                                        "<td>" + row.GetCell(j).ToString() + "</td>"
                                                    );
                                                }
                                                else
                                                {
                                                    return Ok(
                                                        new
                                                        {
                                                            success = false ,
                                                            message =
                                                                "Não foi encontrado o veículo de placa: "
                                                                + placaVeiculo ,
                                                        }
                                                    );
                                                }
                                            }

                                            if (j == 10)
                                            {
                                                string motorista = row.GetCell(j).ToString();
                                                motorista = motorista.Replace("." , "");

                                                var motoristaObj =
                                                    _unitOfWork.Motorista.GetFirstOrDefault(m =>
                                                        m.Nome == motorista
                                                    );
                                                if (motoristaObj != null)
                                                {
                                                    AbastecimentoObj.MotoristaId =
                                                        motoristaObj.MotoristaId;
                                                    sb.Append(
                                                        "<td>" + row.GetCell(j).ToString() + "</td>"
                                                    );
                                                }
                                                else
                                                {
                                                    return Ok(
                                                        new
                                                        {
                                                            success = false ,
                                                            message =
                                                                "Não foi encontrado o(a) motorista: "
                                                                + motorista ,
                                                        }
                                                    );
                                                }
                                            }

                                            if (j == 12)
                                            {
                                                AbastecimentoObj.Hodometro = Convert.ToInt32(
                                                    row.GetCell(j).ToString()
                                                );
                                                sb.Append(
                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
                                                );
                                            }

                                            if (j == 11)
                                            {
                                                AbastecimentoObj.KmRodado =
                                                    Convert.ToInt32(row.GetCell(12).ToString())
                                                    - Convert.ToInt32(row.GetCell(11).ToString());
                                                sb.Append(
                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
                                                );
                                            }

                                            if (j == 13)
                                            {
                                                if (row.GetCell(j).ToString() == "GASOLINA")
                                                {
                                                    AbastecimentoObj.CombustivelId = Guid.Parse(
                                                        "F668F660-8380-4DF3-90CD-787DB06FE734"
                                                    );
                                                }
                                                else
                                                {
                                                    AbastecimentoObj.CombustivelId = Guid.Parse(
                                                        "A69AA86A-9162-4242-AB9A-8B184E04C4DA"
                                                    );
                                                }
                                                sb.Append(
                                                    "<td>" + row.GetCell(j).ToString() + "</td>"
                                                );
                                            }

                                            if (j == 14)
                                            {
                                                AbastecimentoObj.ValorUnitario = Convert.ToDouble(
                                                    row.GetCell(j).ToString()
                                                );
                                                sb.Append(
                                                    "<td>"
                                                        + Math.Round(
                                                                (double)
                                                                    AbastecimentoObj.ValorUnitario ,
                                                                2
                                                            )
                                                            .ToString("0.00")
                                                        + "</td>"
                                                );
                                            }

                                            if (j == 15)
                                            {
                                                AbastecimentoObj.Litros = Convert.ToDouble(
                                                    row.GetCell(j).ToString()
                                                );
                                                sb.Append(
                                                    "<td>"
                                                        + Math.Round(
                                                                (double)AbastecimentoObj.Litros ,
                                                                2
                                                            )
                                                            .ToString("0.00")
                                                        + "</td>"
                                                );
                                            }
                                        }
                                    }

                                    sb.Append(
                                        "<td>"
                                            + Math.Round(
                                                    (
                                                        (double)AbastecimentoObj.KmRodado
                                                        / (double)AbastecimentoObj.Litros
                                                    ) ,
                                                    2
                                                )
                                                .ToString("0.00")
                                            + "</td>"
                                    );

                                    var mediaveiculo =
                                        _unitOfWork.ViewMediaConsumo.GetFirstOrDefault(v =>
                                            v.VeiculoId == AbastecimentoObj.VeiculoId
                                        );
                                    if (mediaveiculo != null)
                                    {
                                        sb.Append("<td>" + mediaveiculo.ConsumoGeral + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append(
                                            "<td>"
                                                + Math.Round(
                                                        (
                                                            (double)AbastecimentoObj.KmRodado
                                                            / (double)AbastecimentoObj.Litros
                                                        ) ,
                                                        2
                                                    )
                                                    .ToString("0.00")
                                                + "</td>"
                                        );
                                    }

                                    sb.AppendLine("</tr>");
                                    _unitOfWork.Abastecimento.Add(AbastecimentoObj);
                                    _unitOfWork.Save();
                                }

                                sb.Append("</tbody></table>");
                                scope.Complete();
                            }
                        }
                        catch (Exception error)
                        {
                            Alerta.TratamentoErroComLinha(
                                "AbastecimentoController.cs" ,
                                "Import.TransactionScope" ,
                                error
                            );
                            throw;
                        }
                    }
                }

                return Ok(
                    new
                    {
                        success = true ,
                        message = "Planilha Importada com Sucesso" ,
                        response = sb.ToString() ,
                    }
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "Import" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MotoristaList
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de motoristas ordenada por nome
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : JsonResult { data: List<ViewMotorista> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/MotoristaList [linha 614]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewMotoristas.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Usado para popular dropdowns e comboboxes no formulário
         ****************************************************************************************/
        [Route("MotoristaList")]
        [HttpGet]
        public IActionResult MotoristaList()
        {
            try
            {
                // [DB] Buscar todos os motoristas e ordenar alfabeticamente por nome
                var result = _unitOfWork.ViewMotoristas.GetAll().OrderBy(vm => vm.Nome).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "MotoristaList" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UnidadeList
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de unidades ordenada por descrição
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : JsonResult { data: List<Unidade> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/UnidadeList [linha 634]
         *
         * ➡️ CHAMA        : _unitOfWork.Unidade.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Usado para popular dropdowns de unidades de negócio
         ****************************************************************************************/
        [Route("UnidadeList")]
        [HttpGet]
        public IActionResult UnidadeList()
        {
            try
            {
                // [DB] Buscar todas as unidades e ordenar por descrição
                var result = _unitOfWork.Unidade.GetAll().OrderBy(u => u.Descricao).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "UnidadeList" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CombustivelList
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de combustíveis ordenada por descrição
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : JsonResult { data: List<Combustivel> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/CombustivelList [linha 654]
         *
         * ➡️ CHAMA        : _unitOfWork.Combustivel.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Usado para popular dropdowns de tipos de combustível
         ****************************************************************************************/
        [Route("CombustivelList")]
        [HttpGet]
        public IActionResult CombustivelList()
        {
            try
            {
                // [DB] Buscar todos os combustíveis e ordenar por descrição
                var result = _unitOfWork.Combustivel.GetAll().OrderBy(u => u.Descricao).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "CombustivelList" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VeiculoList
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de veículos com marca e modelo concatenados
         *
         * 📥 ENTRADAS     : Nenhuma
         *
         * 📤 SAÍDAS       : JsonResult { data: List<{ VeiculoId, PlacaMarcaModelo }> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/VeiculoList [linha 678]
         *
         * ➡️ CHAMA        : _unitOfWork.Veiculo.GetAll() [Repository]
         *                   _unitOfWork.ModeloVeiculo.GetAll() [Repository]
         *                   _unitOfWork.MarcaVeiculo.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Faz LINQ com 3 joins (Veiculo, Marca, Modelo) para exibir formato legível
         ****************************************************************************************/
        [Route("VeiculoList")]
        [HttpGet]
        public IActionResult VeiculoList()
        {
            try
            {
                // [LOGICA] Join entre Veículo, Modelo e Marca para retornar lista formatada
                // Exemplo: "ABC-1234 - Volkswagen/Gol"
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    orderby v.Placa
                    select new
                    {
                        v.VeiculoId ,
                        PlacaMarcaModelo = v.Placa
                            + " - "
                            + ma.DescricaoMarca
                            + "/"
                            + m.DescricaoModelo ,
                    }
                )
                    .OrderBy(v => v.PlacaMarcaModelo)
                    .ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs" , "VeiculoList" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizaQuilometragem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Atualizar apenas a quilometragem de um abastecimento existente
         *
         * 📥 ENTRADAS     : payload [Dictionary<string, object>]
         *                   ├─ AbastecimentoId [GUID] - ID do abastecimento (obrigatório)
         *                   └─ KmRodado [int] - Nova quilometragem
         *
         * 📤 SAÍDAS       : JsonResult { success: bool, message: string }
         *
         * ⬅️ CHAMADO POR  : Frontend via POST /api/Abastecimento/AtualizaQuilometragem [linha 718]
         *
         * ➡️ CHAMA        : _unitOfWork.Abastecimento.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.Abastecimento.Update() [Repository]
         *                   _unitOfWork.Save() [UnitOfWork]
         *
         * 📝 OBSERVAÇÕES  : Endpoint flexível que não valida modelo completo, permitindo edição
         *                   isolada da quilometragem sem necessidade de reenviar todos os campos
         ****************************************************************************************/
        /// <summary>
        /// Atualiza apenas a quilometragem de um abastecimento
        /// Endpoint novo que não valida o modelo completo
        /// </summary>
        [Route("AtualizaQuilometragem")]
        [HttpPost]
        public IActionResult AtualizaQuilometragem([FromBody] Dictionary<string, object> payload)
        {
            try
            {
                // [VALIDACAO] Extrair e validar AbastecimentoId obrigatório
                if (!payload.TryGetValue("AbastecimentoId", out var abastecimentoIdObj))
                {
                    return BadRequest(new { success = false, message = "AbastecimentoId é obrigatório" });
                }

                var abastecimentoId = Guid.Parse(abastecimentoIdObj.ToString());

                // [DB] Buscar abastecimento pelo ID
                var objAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a =>
                    a.AbastecimentoId == abastecimentoId
                );

                if (objAbastecimento == null)
                {
                    return NotFound(new { success = false, message = "Abastecimento não encontrado" });
                }

                // [DADOS] Extrair e atualizar KmRodado se presente no payload
                if (payload.TryGetValue("KmRodado", out var kmRodadoObj) && kmRodadoObj != null)
                {
                    if (int.TryParse(kmRodadoObj.ToString(), out var kmRodado))
                    {
                        objAbastecimento.KmRodado = kmRodado;
                    }
                }

                // [DB] Persistir atualização
                _unitOfWork.Abastecimento.Update(objAbastecimento);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Quilometragem atualizada com sucesso" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AtualizaQuilometragem", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EditaKm
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Endpoint legado para atualizar quilometragem (compatibilidade)
         *
         * 📥 ENTRADAS     : payload [Dictionary<string, object>] - Mesmo formato de AtualizaQuilometragem
         *
         * 📤 SAÍDAS       : JsonResult { success: bool, message: string }
         *
         * ⬅️ CHAMADO POR  : Código legado via POST /api/Abastecimento/EditaKm [linha 765]
         *
         * ➡️ CHAMA        : AtualizaQuilometragem() [linha 720]
         *
         * 📝 OBSERVAÇÕES  : Mantido apenas para compatibilidade com código antigo
         *                   Recomenda-se migrar para AtualizaQuilometragem()
         ****************************************************************************************/
        /// <summary>
        /// Endpoint antigo - mantido para compatibilidade
        /// </summary>
        [Route("EditaKm")]
        [HttpPost]
        public IActionResult EditaKm([FromBody] Dictionary<string, object> payload)
        {
            // [HELPER] Redireciona para o novo endpoint (wrapper para compatibilidade)
            return AtualizaQuilometragem(payload);
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaRegistroCupons
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de registros de cupons de abastecimento
         *
         * 📥 ENTRADAS     : IDapi [string] - Parâmetro não utilizado (legado)
         *
         * 📤 SAÍDAS       : JsonResult { data: List<{ DataRegistro, RegistroCupomId }> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/ListaRegistroCupons?IDapi=xxx [linha 773]
         *
         * ➡️ CHAMA        : _unitOfWork.RegistroCupomAbastecimento.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Ordena por data de registro descrescente (mais recentes primeiro)
         *                   Parâmetro IDapi mantido por compatibilidade mas não é utilizado
         ****************************************************************************************/
        [Route("ListaRegistroCupons")]
        [HttpGet]
        public IActionResult ListaRegistroCupons(string IDapi)
        {
            try
            {
                // [DB] Buscar todos os registros de cupom e ordenar por data descrescente
                var result = (
                    from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                    orderby rc.DataRegistro descending
                    select new
                    {
                        DataRegistro = rc.DataRegistro?.ToShortDateString() ,
                        rc.RegistroCupomId ,
                    }
                ).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "ListaRegistroCupons" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaRegistroCupons
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar conteúdo PDF de um cupom de abastecimento específico
         *
         * 📥 ENTRADAS     : IDapi [string] - GUID do registro de cupom (formato string)
         *
         * 📤 SAÍDAS       : JsonResult { RegistroPDF: binary }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/PegaRegistroCupons?IDapi=xxx [linha 805]
         *
         * ➡️ CHAMA        : _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Retorna dados binários do PDF para exibição/download no cliente
         ****************************************************************************************/
        [Route("PegaRegistroCupons")]
        [HttpGet]
        public IActionResult PegaRegistroCupons(string IDapi)
        {
            try
            {
                // [DB] Buscar registro de cupom pelo ID
                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc =>
                    rc.RegistroCupomId == Guid.Parse(IDapi)
                );

                return Ok(new
                {
                    RegistroPDF = objRegistro.RegistroPDF
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "PegaRegistroCupons" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaRegistroCuponsData
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar registros de cupons filtrados por data específica
         *
         * 📥 ENTRADAS     : id [string] - Data no formato "YYYY-MM-DD" ou compatível com DateTime.Parse
         *
         * 📤 SAÍDAS       : JsonResult { data: List<{ DataRegistro, RegistroCupomId }> }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/PegaRegistroCuponsData?id=xxx [linha 831]
         *
         * ➡️ CHAMA        : _unitOfWork.RegistroCupomAbastecimento.GetAll() [Repository]
         *
         * 📝 OBSERVAÇÕES  : Filtra por data exata e ordena por data descrescente
         ****************************************************************************************/
        [Route("PegaRegistroCuponsData")]
        [HttpGet]
        public IActionResult PegaRegistroCuponsData(string id)
        {
            try
            {
                // [DB] Filtrar cupons pela data e ordenar por data descrescente
                var result = (
                    from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                    where rc.DataRegistro == DateTime.Parse(id)
                    orderby rc.DataRegistro descending
                    select new
                    {
                        DataRegistro = rc.DataRegistro?.ToShortDateString() ,
                        rc.RegistroCupomId ,
                    }
                ).ToList();

                return Ok(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "PegaRegistroCuponsData" ,
                    error
                );
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteRegistro
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Excluir um registro de cupom de abastecimento
         *
         * 📥 ENTRADAS     : IDapi [string] - GUID do registro de cupom (formato string)
         *
         * 📤 SAÍDAS       : JsonResult { success: bool, message: string }
         *
         * ⬅️ CHAMADO POR  : Frontend via GET /api/Abastecimento/DeleteRegistro?IDapi=xxx [linha 864]
         *
         * ➡️ CHAMA        : _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.RegistroCupomAbastecimento.Remove() [Repository]
         *                   _unitOfWork.Save() [UnitOfWork]
         *
         * 📝 OBSERVAÇÕES  : Operação de exclusão permanente - sem soft delete
         *                   ⚠️ NOTA: Usar GET para DELETE não é prática RESTful (deveria ser DELETE)
         ****************************************************************************************/
        [Route("DeleteRegistro")]
        [HttpGet]
        public IActionResult DeleteRegistro(string IDapi)
        {
            try
            {
                // [DB] Buscar registro de cupom
                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc =>
                    rc.RegistroCupomId == Guid.Parse(IDapi)
                );

                // [DB] Remover do repositório e persistir
                _unitOfWork.RegistroCupomAbastecimento.Remove(objRegistro);
                _unitOfWork.Save();

                return Ok(new
                {
                    success = true ,
                    message = "Registro excluído com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AbastecimentoController.cs" ,
                    "DeleteRegistro" ,
                    error
                );
                return StatusCode(500);
            }
        }
    }
}
