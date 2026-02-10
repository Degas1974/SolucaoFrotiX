/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: RegistraCupons.cshtml.cs (Pages/Abastecimento - namespace Multa)                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para registro de cupons fiscais de abastecimento. Permite upload de imagens/PDFs              ║
 * ║ de cupons para comprovação de abastecimento.                                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ NOTA: NAMESPACE INCONSISTENTE                                                                             ║
 * ║ • Arquivo está em Pages/Abastecimento mas namespace é FrotiX.Pages.Multa                                 ║
 * ║ • Mantido para compatibilidade com código existente                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                    ║
 * ║ • _unitOfWork : Referência estática ao repositório                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - lógica no JavaScript                                                         ║
 * ║ • OnPostSavePDF(files) : Salva arquivos de cupom em wwwroot/Cupons                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ UPLOAD DE ARQUIVOS                                                                                        ║
 * ║ • Aceita múltiplos arquivos (IEnumerable<IFormFile>)                                                     ║
 * ║ • Salva em wwwroot/Cupons                                                                                ║
 * ║ • Substitui espaços por underscore no nome do arquivo                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • IWebHostEnvironment - Para caminho do wwwroot                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FrotiX.Pages.Multa
{
    public class RegistraCuponsModel :PageModel
    {
        public static IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RegistraCuponsModel(IUnitOfWork unitOfWork , IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RegistraCupons.cshtml.cs" , "RegistraCuponsModel" , error);
            }
        }

        public void OnGet()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RegistraCupons.cshtml.cs" , "OnGet" , error);
                return;
            }
        }

        public ActionResult OnPostSavePDF(IEnumerable<IFormFile> files)
        {
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string folderName = "Cupons";
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        string newPath = Path.Combine(webRootPath , folderName);
                        StringBuilder sb = new StringBuilder();
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        if (file.Length > 0)
                        {
                            string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                            string fullPath = Path.Combine(newPath , file.FileName.Replace(" " , "_"));
                            using (var stream = new FileStream(fullPath , FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                }

                return Content("");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RegistraCupons.cshtml.cs" , "OnPostSavePDF" , error);
                return Content("");
            }
        }
    }
}
