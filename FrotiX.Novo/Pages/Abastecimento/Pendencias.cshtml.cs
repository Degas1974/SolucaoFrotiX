/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: Pendencias.cshtml.cs (Pages/Abastecimento)                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para listagem de pendências de abastecimento. Exibe abastecimentos que precisam               ║
 * ║ de validação, correção de dados ou aprovação.                                                            ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                    ║
 * ║ • _unitOfWork : Referência estática ao repositório                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • Initialize(unitOfWork) : Método estático para inicialização                                            ║
 * ║ • OnGet() : Carrega listas de veículos, combustíveis e motoristas via ViewData                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ LISTAS CARREGADAS (ViewData)                                                                              ║
 * ║ • lstVeiculos : Lista de veículos para filtro                                                            ║
 * ║ • lstCombustivel : Lista de tipos de combustível                                                         ║
 * ║ • lstMotorista : Lista de motoristas para filtro                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • ListaVeiculos, ListaCombustivel, ListaMotorista - Classes helper do módulo                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Abastecimento
{
    public class PendenciasModel : PageModel
    {
        public static IUnitOfWork _unitOfWork;

        public static void Initialize(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Pendencias.cshtml.cs", "Initialize", error);
                return;
            }
        }

        public PendenciasModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Pendencias.cshtml.cs", "PendenciasModel", error);
            }
        }

        public void OnGet()
        {
            try
            {
                Initialize(_unitOfWork);
                ViewData["lstVeiculos"] = new FrotiX.Pages.Abastecimento.ListaVeiculos(_unitOfWork).VeiculosList();
                ViewData["lstCombustivel"] = new FrotiX.Pages.Abastecimento.ListaCombustivel(_unitOfWork).CombustivelList();
                ViewData["lstMotorista"] = new FrotiX.Pages.Abastecimento.ListaMotorista(_unitOfWork).MotoristaList();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Pendencias.cshtml.cs", "OnGet", error);
                return;
            }
        }
    }
}
