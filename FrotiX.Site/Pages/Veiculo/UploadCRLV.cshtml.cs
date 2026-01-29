/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UploadCRLV.cshtml.cs (Pages/Veiculo)                                                            ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para upload do CRLV (Certificado de Registro e Licenciamento de Veículo) digital.             ║
 * ║ Permite visualizar e fazer upload do documento de licenciamento do veículo.                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES                                                                                              ║
 * ║ • CRLV : int - Indicador se possui CRLV digital (0 = não, 1 = sim)                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • VeiculoObj : VeiculoViewModel - Contém entidade Veiculo                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega veículo e verifica se possui CRLV digital                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÕES                                                                                                ║
 * ║ • ID vazio: retorna página de upload vazia                                                              ║
 * ║ • Veículo não encontrado: retorna NotFound()                                                            ║
 * ║ • CRLV != null: seta indicador CRLV = 1                                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Veiculo
{
    public class UploadCRLVModel :PageModel
    {
        // Propriedade de instância (não estática)
        public int CRLV
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public UploadCRLVModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLV.cshtml.cs" , "UploadCRLVModel" , error);
            }
        }

        [BindProperty]
        public VeiculoViewModel VeiculoObj
        {
            get; set;
        }

        private void SetViewModel()
        {
            try
            {
                VeiculoObj = new VeiculoViewModel { Veiculo = new Models.Veiculo() };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLV.cshtml.cs" , "SetViewModel" , error);
                return;
            }
        }

        public ActionResult OnGet(Guid id)
        {
            try
            {
                SetViewModel();
                CRLV = 0;

                if (id != Guid.Empty)
                {
                    VeiculoObj.Veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.VeiculoId == id);

                    if (VeiculoObj?.Veiculo == null)
                    {
                        return NotFound();
                    }

                    if (VeiculoObj.Veiculo.CRLV != null)
                    {
                        CRLV = 1;
                    }
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLV.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }
    }
}
