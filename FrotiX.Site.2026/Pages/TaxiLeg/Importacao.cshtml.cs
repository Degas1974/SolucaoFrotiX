/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: Importacao.cshtml.cs (Pages/TaxiLeg)                                                            ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para importação de corridas do TaxiLeg (serviço de táxi corporativo).                          ║
 * ║ Processa arquivos Excel (.xlsx) e converte dados para exibição em HTML.                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnPostSubmit(file) : Upload e processamento de arquivo Excel                                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ FLUXO DE PROCESSAMENTO                                                                                   ║
 * ║ 1. Valida tipo de arquivo (xlsx/xls)                                                                     ║
 * ║ 2. Salva arquivo em wwwroot/Uploads/{guid}/                                                              ║
 * ║ 3. Lê planilhas com EPPlus (ExcelPackage)                                                                ║
 * ║ 4. Converte cada worksheet para HTML (ExcelViewModel)                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ TIPOS DE ARQUIVO ACEITOS                                                                                 ║
 * ║ • application/octet-stream                                                                               ║
 * ║ • application/vnd.openxmlformats-officedocument.spreadsheetml.sheet (.xlsx)                              ║
 * ║ • application/vnd.ms-excel (.xls)                                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork (injetado)                                                                                 ║
 * ║ • IWebHostEnvironment - Para caminho de upload (wwwroot/Uploads)                                         ║
 * ║ • EPPlus (ExcelPackage, ExcelWorksheet) - Leitura de Excel                                               ║
 * ║ • ExcelViewModel - DTO com SheetName + Data (HTML)                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrotiX.Pages.TaxiLeg
{
    public class ImportacaoModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImportacaoModel(IUnitOfWork unitOfWork , IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Importacao.cshtml.cs" , "ImportacaoModel" , error);
            }
        }

        public IActionResult OnPostSubmit(IFormFile file)
        {
            try
            {
                IList<ExcelViewModel> list = new List<ExcelViewModel>();
                var validTypes = new string[]
                {
                    "application/octet-stream",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/vnd.ms-excel",
                };
                if (file == null)
                {
                    ModelState.AddModelError("file" , "Please select excel (.xlsx) file.");
                }

                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        if (!validTypes.Contains(file.ContentType))
                        {
                            ModelState.AddModelError(
                                "file" ,
                                "Only the following file types are allowed: .xlsx"
                            );
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }
                else
                {
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            if (validTypes.Contains(file.ContentType))
                            {
                                string guid = Guid.NewGuid().ToString();
                                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(
                                    file.ContentDisposition
                                );
                                string uploadPath = "Uploads\\" + guid + "";
                                string path = Path.Combine(
                                    _hostingEnvironment.WebRootPath ,
                                    uploadPath
                                );
                                Directory.CreateDirectory(path);
                                var filePath = Path.Combine(
                                    path ,
                                    parsedContentDisposition.FileName.ToString()
                                );
                                FileInfo fileInfo = new FileInfo(filePath);

                                using (var stream = new FileStream(filePath , FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }

                                using (ExcelPackage package = new ExcelPackage(fileInfo))
                                {
                                    var worksheets = package.Workbook.Worksheets;
                                    foreach (ExcelWorksheet item in worksheets)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        int rowCount = item.Dimension.Rows;
                                        int ColCount = item.Dimension.Columns;

                                        ExcelViewModel sheet = new ExcelViewModel();

                                        sheet.SheetName = item.Name;

                                        for (int row = 1; row <= rowCount; row++)
                                        {
                                            sb.Append("<tr>");
                                            for (int col = 1; col <= ColCount; col++)
                                            {
                                                if (row == 1)
                                                {
                                                    sb.Append("<th>");
                                                    sb.Append(
                                                        Convert.ToString(item.Cells[row , col].Value)
                                                    );
                                                    sb.Append("</th>");
                                                }
                                                else
                                                {
                                                    sb.Append("<td>");
                                                    sb.Append(
                                                        Convert.ToString(item.Cells[row , col].Value)
                                                    );
                                                    sb.Append("</td>");
                                                }
                                            }
                                            sb.Append("</tr>");
                                        }
                                        sheet.Data = sb.ToString();
                                        list.Add(sheet);
                                    }
                                }
                            }
                        }
                    }
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Importacao.cshtml.cs" , "OnPostSubmit" , error);
                return Page();
            }
        }
    }
}
