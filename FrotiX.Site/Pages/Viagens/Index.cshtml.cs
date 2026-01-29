// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : Index.cshtml.cs                                                 ║
// ║ LOCALIZAÇÃO: Pages/Viagens/                                                  ║
// ║ FINALIDADE : PageModel para listagem de viagens no grid de gerenciamento.    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ Ponto de entrada da página principal de viagens:                             ║
// ║ • Injeta IUnitOfWork para acesso a repositórios                              ║
// ║ • FotoMotorista: Cache estático para foto do motorista selecionado           ║
// ║ • FichaVistoria: Upload de ficha de vistoria do veículo                      ║
// ║ • ViagemObj: ViewModel bindada para dados da viagem                          ║
// ║ • ViagemId: ID da viagem selecionada                                         ║
// ║ Grid de viagens e filtros carregados via AJAX no JavaScript.                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 23 — Pages/Viagens                                             ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Viagens
{
    public class IndexModel :PageModel
    {
        public static IUnitOfWork _unitOfWork;

        public static byte[] FotoMotorista;

        public static IFormFile FichaVIstoria;

        public static void Initialize(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "Initialize" , error);
                return;
            }
        }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "IndexModel" , error);
            }
        }

        [BindProperty]
        public Models.ViagemViewModel ViagemObj
        {
            get; set;
        }

        [BindProperty]
        public Guid ViagemId
        {
            get; set;
        }
    }
}
