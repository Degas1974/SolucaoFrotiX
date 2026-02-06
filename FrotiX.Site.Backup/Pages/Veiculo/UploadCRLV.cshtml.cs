/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UploadCRLV.cshtml.cs (Pages/Veiculo)                                                            ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para upload e visualização de CRLV (Certificado de Registro e Licenciamento de Veículo)        ║
 * ║ em formato PDF. Permite anexar documento digitalizado do veículo para consulta mobile.                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES                                                                                             ║
 * ║ • CRLV           : int             - Flag indicando se veículo possui CRLV anexado (0=não, 1=sim)        ║
 * ║ • VeiculoObj     : VeiculoViewModel - Dados do veículo (Placa, Modelo, CRLV byte[])                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega veículo por ID e verifica se possui CRLV anexado                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ REGRAS                                                                                                   ║
 * ║ • Se veículo não encontrado → retorna NotFound()                                                         ║
 * ║ • Se Veiculo.CRLV != null → seta CRLV=1 (flag para exibir PDF automaticamente no load)                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ OBSERVAÇÃO                                                                                               ║
 * ║ O upload e visualização de PDF são feitos no frontend via Syncfusion Uploader/PDFViewer.                 ║
 * ║ Endpoints de Save/Remove são implementados em UploadCRLVController (API).                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork.Veiculo (GetFirstOrDefault)                                                                ║
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

