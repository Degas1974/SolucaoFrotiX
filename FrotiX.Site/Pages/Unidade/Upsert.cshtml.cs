/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: Upsert.cshtml.cs (Pages/Unidade)                                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para criação e edição de Unidades Organizacionais (Garagem, Manutenção, TV Câmara, etc.).      ║
 * ║ Gerencia até 3 contatos e normaliza códigos de ponto.                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ BIND PROPERTIES                                                                                          ║
 * ║ • UnidadeObj : Unidade - Entidade da unidade                                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id)      : Carrega unidade para edição ou prepara nova                                           ║
 * ║ • OnPostSubmit() : Cria ou atualiza unidade (diferencia por UnidadeId == Guid.Empty)                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÕES                                                                                               ║
 * ║ • Descrição única (case-insensitive)                                                                     ║
 * ║ • Sigla única (case-insensitive)                                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ REGRAS DE NEGÓCIO                                                                                        ║
 * ║ • Sigla convertida para maiúscula                                                                        ║
 * ║ • PontoPrimeiroContato, PontoSegundoContato, PontoTerceiroContato normalizados com prefixo "p_"          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork (Unidade)                                                                                  ║
 * ║ • INotyfService - Notificações toast                                                                     ║
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

namespace FrotiX.Pages.Unidade
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IndexModel> _logger;
        private readonly INotyfService _notyf;

        public UpsertModel(IUnitOfWork unitOfWork , ILogger<IndexModel> logger , INotyfService notyf)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "UpsertModel" , error);
            }
        }

        [BindProperty]
        public Models.Unidade UnidadeObj
        {
            get; set;
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                UnidadeObj = new Models.Unidade();
                if (id != Guid.Empty)
                {
                    UnidadeObj = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == id);
                    if (UnidadeObj == null)
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
                    return Page();
                }

                UnidadeObj.Sigla = UnidadeObj.Sigla.ToUpper();

                if (UnidadeObj.PontoPrimeiroContato.Substring(0 , 2).ToUpper() != "P_")
                {
                    UnidadeObj.PontoPrimeiroContato = "p_" + UnidadeObj.PontoPrimeiroContato;
                }
                else
                {
                    UnidadeObj.PontoPrimeiroContato = "p_" + UnidadeObj.PontoPrimeiroContato.Substring(2 , UnidadeObj.PontoPrimeiroContato.Length - 2);
                }

                if (UnidadeObj.PontoSegundoContato != null)
                {
                    if (UnidadeObj.PontoSegundoContato.Substring(0 , 2).ToUpper() != "P_")
                    {
                        UnidadeObj.PontoSegundoContato = "p_" + UnidadeObj.PontoSegundoContato;
                    }
                    else
                    {
                        UnidadeObj.PontoSegundoContato = "p_" + UnidadeObj.PontoSegundoContato.Substring(2 , UnidadeObj.PontoSegundoContato.Length - 2);
                    }
                }

                if (UnidadeObj.PontoTerceiroContato != null)
                {
                    if (UnidadeObj.PontoTerceiroContato.Substring(0 , 2).ToUpper() != "P_")
                    {
                        UnidadeObj.PontoTerceiroContato = "p_" + UnidadeObj.PontoTerceiroContato;
                    }
                    else
                    {
                        UnidadeObj.PontoTerceiroContato = "p_" + UnidadeObj.PontoTerceiroContato.Substring(2 , UnidadeObj.PontoTerceiroContato.Length - 2);
                    }
                }

                var existeUnidade = _unitOfWork.Unidade.GetFirstOrDefault(u => u.Descricao.ToUpper() == UnidadeObj.Descricao.ToUpper());
                if (existeUnidade != null && existeUnidade.UnidadeId != UnidadeObj.UnidadeId)
                {
                    AppToast.show("Vermelho" , "Já existe uma unidade com esse nome!" , 3000);
                    return Page();
                }

                var existeSigla = _unitOfWork.Unidade.GetFirstOrDefault(u => u.Sigla.ToUpper() == UnidadeObj.Sigla.ToUpper());
                if (existeSigla != null && existeSigla.UnidadeId != UnidadeObj.UnidadeId)
                {
                    AppToast.show("Vermelho" , "Já existe uma unidade com essa sigla!" , 3000);
                    return Page();
                }

                if (UnidadeObj.UnidadeId == Guid.Empty)
                {
                    AppToast.show("Verde" , "Unidade adicionada com sucesso!" , 3000);
                    _unitOfWork.Unidade.Add(UnidadeObj);
                }
                else
                {
                    AppToast.show("Verde" , "Unidade atualizada com sucesso!" , 3000);
                    _unitOfWork.Unidade.Update(UnidadeObj);
                }

                _unitOfWork.Save();
                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs" , "OnPostSubmit" , error);
                return RedirectToPage("./Index");
            }
        }
    }
}
