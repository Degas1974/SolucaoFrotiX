// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : UpsertAutuacao.cshtml.cs                                        ║
// ║ LOCALIZAÇÃO: Pages/Uploads/                                                  ║
// ║ FINALIDADE : PageModel para upload e criação de multas/autuações.            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO: Processa upload de PDF de autuação e cria registro de multa.       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE: 23 — Pages/Uploads | DATA: 29/01/2026                                  ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FrotiX.Pages.Uploads
{
    public class UpsertMultaModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public static Guid multaId;

        public UpsertMultaModel(IUnitOfWork unitOfWork , INotyfService notyf , IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml.cs" , "UpsertMultaModel" , error);
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
                        string folderName = "DadosEditaveis/Multas";
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
                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml.cs" , "OnPostSavePDF" , error);
                return Content("");
            }
        }

        public ActionResult OnPostSaveImagemOcorrencia(IEnumerable<IFormFile> files)
        {
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string folderName = "DadosEditaveis/ImagensOcorrencias";
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
                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml.cs" , "OnPostSaveImagemOcorrencia" , error);
                return Content("");
            }
        }
    }
}
