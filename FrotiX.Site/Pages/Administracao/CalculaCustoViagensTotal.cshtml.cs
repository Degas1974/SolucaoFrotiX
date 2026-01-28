// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: CalculaCustoViagensTotal.cshtml.cs                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para cálculo batch de custos de viagens.                          ║
// ║ Permite recalcular custos de todas as viagens do sistema.                   ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • Injeção de IUnitOfWork e IWebHostEnvironment                              ║
// ║ • Processamento em batch via ViagemController                               ║
// ║ • Atualiza CustoCombustivel, CustoMotorista, CustoVeiculo                   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Administracao
{
    public class CalculaCustoViagensTotalModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CalculaCustoViagensTotalModel(IUnitOfWork unitOfWork , IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("CalculaCustoViagensTotal.cshtml.cs" , "CalculaCustoViagensTotalModel" , error);
            }
        }
    }
}
