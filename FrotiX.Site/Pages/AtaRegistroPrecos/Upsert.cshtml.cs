using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace FrotiX.Pages.AtaRegistroPrecos
{
    public class UpsertModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly INotyfService _notyf;

        public static Guid ataId;

        public UpsertModel(
            IUnitOfWork unitOfWork ,
            IWebHostEnvironment hostingEnvironment ,
            INotyfService notyf
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnvironment = hostingEnvironment;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "UpsertModel.Constructor" , error);
            }
        }

        [BindProperty]
        public AtaRegistroPrecosViewModel AtaObj
        {
            get; set;
        }

        private void SetViewModel()
        {
            try
            {
                AtaObj = new AtaRegistroPrecosViewModel
                {
                    FornecedorList = _unitOfWork.Fornecedor.GetFornecedorListForDropDown() ,
                    AtaRegistroPrecos = new Models.AtaRegistroPrecos()
                    {
                        Status = true
                    } ,
                };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "SetViewModel" , error);
            }
        }

        private void PopulateViewData()
        {
            var anos = Enumerable.Range(2020, 8).Select(x => new { Value = x, Text = x.ToString() }).ToList();
            ViewData["AnosList"] = new SelectList(anos, "Value", "Text");
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                SetViewModel();
                PopulateViewData();

                if (id != Guid.Empty)
                {
                    AtaObj.AtaRegistroPrecos = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u => u.AtaId == id);
                    if (AtaObj == null)
                    {
                        return NotFound();
                    }
                }
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }

        public IActionResult OnPostSubmit()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    SetViewModel();
                    PopulateViewData();
                    return Page();
                }

                var existeAta = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                    (u.AnoAta == AtaObj.AtaRegistroPrecos.AnoAta) && (u.NumeroAta == AtaObj.AtaRegistroPrecos.NumeroAta)
                );

                if (existeAta != null)
                {
                    _notyf.Error("Já existe uma Ata com esse número!" , 3);
                    SetViewModel();
                    PopulateViewData();
                    return Page();
                }

                _unitOfWork.AtaRegistroPrecos.Add(AtaObj.AtaRegistroPrecos);
                _unitOfWork.Save();

                _notyf.Success("Ata adicionada com sucesso!" , 3);

                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnPostSubmit" , error);
                return RedirectToPage("./Index");
            }
        }

        public IActionResult OnPostEdit(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    SetViewModel();
                    PopulateViewData();
                    AtaObj.AtaRegistroPrecos.AtaId = id;
                    return Page();
                }

                AtaObj.AtaRegistroPrecos.AtaId = id;

                var existeAta = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(u =>
                    (u.AnoAta == AtaObj.AtaRegistroPrecos.AnoAta) && (u.NumeroAta == AtaObj.AtaRegistroPrecos.NumeroAta)
                );

                if (existeAta != null && existeAta.AtaId != AtaObj.AtaRegistroPrecos.AtaId)
                {
                    _notyf.Error("Já existe uma Ata com esse número!" , 3);
                    SetViewModel();
                    PopulateViewData();
                    AtaObj.AtaRegistroPrecos.AtaId = id;
                    return Page();
                }

                _unitOfWork.AtaRegistroPrecos.Update(AtaObj.AtaRegistroPrecos);
                _unitOfWork.Save();

                _notyf.Success("Ata atualizada com sucesso!" , 3);

                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnPostEdit" , error);
                return RedirectToPage("./Index");
            }
        }
    }
}
