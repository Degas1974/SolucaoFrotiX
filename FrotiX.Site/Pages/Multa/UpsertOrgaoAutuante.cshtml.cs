/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UpsertOrgaoAutuante.cshtml.cs (Pages/Multa)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para cadastro e edição de Órgãos Autuantes (entidades que aplicam multas de trânsito).        ║
 * ║ Exemplos: DETRAN, PRF, PM, DER, Prefeituras, etc.                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • OrgaoAutuanteObj : OrgaoAutuante - Entidade direta (Nome, Sigla)                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega órgão existente ou prepara novo                                                   ║
 * ║ • OnPostSubmit() : Insere ou atualiza órgão                                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÕES                                                                                                ║
 * ║ • ModelState.IsValid                                                                                    ║
 * ║ • Verificação de duplicidade por Nome (case insensitive)                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • ILogger<UpsertOrgaoAutuanteModel> - Logging                                                           ║
 * ║ • INotyfService - Notificações toast                                                                    ║
 * ║ • AppToast - Notificações visuais                                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace FrotiX.Pages.Multa
{
    public class UpsertOrgaoAutuanteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpsertOrgaoAutuanteModel> _logger;
        private readonly INotyfService _notyf;

        public UpsertOrgaoAutuanteModel(
            IUnitOfWork unitOfWork ,
            ILogger<UpsertOrgaoAutuanteModel> logger ,
            INotyfService notyf
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertOrgaoAutuante.cshtml.cs" ,
                    "UpsertOrgaoAutuanteModel" ,
                    error
                );
            }
        }

        [BindProperty]
        public Models.OrgaoAutuante OrgaoAutuanteObj
        {
            get; set;
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                OrgaoAutuanteObj = new Models.OrgaoAutuante();

                if (id != Guid.Empty)
                {
                    OrgaoAutuanteObj = _unitOfWork.OrgaoAutuante.GetFirstOrDefault(u =>
                        u.OrgaoAutuanteId == id
                    );
                    if (OrgaoAutuanteObj == null)
                    {
                        return NotFound();
                    }
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertOrgaoAutuante.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }

        public IActionResult OnPostSubmit()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Verifica Duplicado
                var existeOrgaoAutuante = _unitOfWork.OrgaoAutuante.GetFirstOrDefault(u =>
                    u.Nome.ToUpper() == OrgaoAutuanteObj.Nome.ToUpper()
                );

                if (OrgaoAutuanteObj.OrgaoAutuanteId != Guid.Empty && existeOrgaoAutuante != null)
                {
                    if (OrgaoAutuanteObj.OrgaoAutuanteId != existeOrgaoAutuante.OrgaoAutuanteId)
                    {
                        AppToast.show("Vermelho" , "Já existe este Órgão cadastrado!" , 3000);
                        return Page();
                    }
                }
                else if (existeOrgaoAutuante != null)
                {
                    AppToast.show("Vermelho" , "Já existe este Órgão cadastrado!" , 3000);
                    return Page();
                }

                if (OrgaoAutuanteObj.OrgaoAutuanteId == Guid.Empty)
                {
                    _unitOfWork.OrgaoAutuante.Add(OrgaoAutuanteObj);
                    AppToast.show("Verde" , "Órgão Autuante cadastrado com sucesso!" , 3000);
                }
                else
                {
                    _unitOfWork.OrgaoAutuante.Update(OrgaoAutuanteObj);
                    AppToast.show("Verde" , "Órgão Autuante atualizado com sucesso!" , 3000);
                }
                _unitOfWork.Save();

                return RedirectToPage("./ListaOrgaosAutuantes");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertOrgaoAutuante.cshtml.cs" ,
                    "OnPostSubmit" ,
                    error
                );
                return RedirectToPage("./ListaOrgaosAutuantes");
            }
        }
    }
}
