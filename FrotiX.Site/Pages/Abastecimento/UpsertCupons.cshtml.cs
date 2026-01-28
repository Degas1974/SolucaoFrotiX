/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UpsertCupons.cshtml.cs (Pages/Abastecimento)                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para criação e edição de registros de cupons de abastecimento. Gerencia metadados              ║
 * ║ dos cupons fiscais associados a abastecimentos da frota.                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • RegistroCupomAbastecimentoObj : ViewModel contendo RegistroCupomAbastecimento                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega cupom existente ou inicializa novo                                                 ║
 * ║ • OnPostSubmit() : Cria novo registro de cupom                                                           ║
 * ║ • OnPostEdit(Id) : Atualiza registro existente                                                           ║
 * ║ • OnPostSavePDF(files) : Salva arquivos em DadosEditaveis/Cupons com tratamento de acentos               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ MÉTODOS AUXILIARES                                                                                        ║
 * ║ • TiraAcento(frase) : Remove acentos e substitui espaços por underscore                                  ║
 * ║ • RemoveAcento(c) : Remove acento de caractere individual (á→a, ç→c, etc.)                               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ UPLOAD DE ARQUIVOS                                                                                        ║
 * ║ • Diretório: wwwroot/DadosEditaveis/Cupons                                                               ║
 * ║ • Tratamento: remove acentos e converte para maiúsculo                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • INotyfService - Notificações toast                                                                     ║
 * ║ • IWebHostEnvironment - Para caminho do wwwroot                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FrotiX.Pages.Abastecimento
{
    public class UpsertCuponsModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public static Guid RegistroCupomId;

        public UpsertCuponsModel(
            IUnitOfWork unitOfWork ,
            INotyfService notyf ,
            IWebHostEnvironment hostingEnvironment
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "UpsertCuponsModel" , error);
            }
        }

        [BindProperty]
        public RegistroCupomAbastecimentoViewModel RegistroCupomAbastecimentoObj
        {
            get; set;
        }

        private void SetViewModel()
        {
            try
            {
                RegistroCupomAbastecimentoObj = new RegistroCupomAbastecimentoViewModel
                {
                    RegistroCupomAbastecimento = new Models.RegistroCupomAbastecimento() ,
                };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "SetViewModel" , error);
                return;
            }
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                SetViewModel();

                if (id != Guid.Empty)
                {
                    RegistroCupomId = id;
                }

                if (id != Guid.Empty)
                {
                    RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento =
                        _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(m =>
                            m.RegistroCupomId == id
                        );
                    if (RegistroCupomAbastecimentoObj == null)
                    {
                        return NotFound();
                    }
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }

        public IActionResult OnPostSubmit()
        {
            try
            {
                _notyf.Success("Registro adicionado com sucesso!" , 3);
                _unitOfWork.RegistroCupomAbastecimento.Add(
                    RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento
                );
                _unitOfWork.Save();

                return RedirectToPage("./RegistraCupons");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "OnPostSubmit" , error);
                return RedirectToPage("./RegistraCupons");
            }
        }

        public IActionResult OnPostEdit(Guid Id)
        {
            try
            {
                RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroCupomId = Id;

                Guid RegistroCupomId = Guid.Empty;

                if (
                    RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroCupomId
                    != Guid.Empty
                )
                {
                    RegistroCupomId = RegistroCupomAbastecimentoObj
                        .RegistroCupomAbastecimento
                        .RegistroCupomId;
                }

                _notyf.Success("Registro atualizado com sucesso!" , 3);
                _unitOfWork.RegistroCupomAbastecimento.Update(
                    RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento
                );

                _unitOfWork.Save();

                return RedirectToPage("./RegistraCupons");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "OnPostEdit" , error);
                return RedirectToPage("./RegistraCupons");
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
                        string folderName = "DadosEditaveis/Cupons";
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
                            string fullPath = Path.Combine(newPath , TiraAcento(file.FileName));
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
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "OnPostSavePDF" , error);
                return Content("");
            }
        }

        static string TiraAcento(string frase)
        {
            try
            {
                StringBuilder resultado = new StringBuilder();

                foreach (char c in frase)
                {
                    char caractereSemAcento = RemoveAcento(c);
                    resultado.Append(caractereSemAcento == ' ' ? '_' : caractereSemAcento);
                }

                return resultado.ToString().ToUpper();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "TiraAcento" , error);
                return string.Empty;
            }
        }

        static char RemoveAcento(char c)
        {
            try
            {
                switch (c)
                {
                    case 'Á':
                    case 'á':
                        return 'a';
                    case 'É':
                    case 'é':
                        return 'e';
                    case 'Í':
                    case 'í':
                        return 'i';
                    case 'Ó':
                    case 'ó':
                        return 'o';
                    case 'Ú':
                    case 'ú':
                        return 'u';
                    case 'À':
                    case 'à':
                        return 'a';
                    case 'È':
                    case 'è':
                        return 'e';
                    case 'Ì':
                    case 'ì':
                        return 'i';
                    case 'Ò':
                    case 'ò':
                        return 'o';
                    case 'Ù':
                    case 'ù':
                        return 'u';
                    case 'Â':
                    case 'â':
                        return 'a';
                    case 'Ê':
                    case 'ê':
                        return 'e';
                    case 'Î':
                    case 'î':
                        return 'i';
                    case 'Ô':
                    case 'ô':
                        return 'o';
                    case 'Û':
                    case 'û':
                        return 'u';
                    case 'Ã':
                    case 'ã':
                        return 'a';
                    case 'Õ':
                    case 'õ':
                        return 'o';
                    case 'Ç':
                    case 'ç':
                        return 'c';
                    default:
                        return c;
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs" , "RemoveAcento" , error);
                return default(char);
            }
        }
    }
}
