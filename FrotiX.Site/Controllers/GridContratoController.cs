/****************************************************************************************
 * ⚡ CONTROLLER: GridContratoController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer dados para grids de itens de Contratos
 *                   Gerencia lista de veículos/serviços incluídos nos contratos
 * 📥 ENTRADAS     : Nenhuma (utiliza dados estáticos/sessão)
 * 📤 SAÍDAS       : JSON com lista de ItensVeiculo
 * 🔗 CHAMADA POR  : JavaScript (grids Syncfusion) das páginas de Contratos via AJAX
 * 🔄 CHAMA        : ItensVeiculo.GetAllRecords(), IUnitOfWork
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork
 *
 * 💡 CONCEITOS:
 *    - Itens de Contrato: Veículos/serviços contratados com quantidade e valores
 *    - Lista estática: Armazena itens temporariamente durante edição de contrato
 *    - Repactuação: Ajuste de valores contratuais ao longo do tempo
 ****************************************************************************************/
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class GridContratoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // [DOC] Lista estática para armazenar itens temporariamente durante edição de contrato
        public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();

        /****************************************************************************************
         * ⚡ CLASSE: objItem (Helper)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Wrapper para RepactuacaoContratoId (usado em operações de grid)
         ****************************************************************************************/
        public class objItem
        {
            Guid RepactuacaoContratoId
            {
                get; set;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GridContratoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do Unit of Work
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork
         * 📤 SAÍDAS       : Instância configurada
         * 🔗 CHAMADA POR  : ASP.NET Core DI
         ****************************************************************************************/
        public GridContratoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "GridContratoController.cs" ,
                    "GridContratoController" ,
                    error
                );
            }
        }

        [Route("DataSource")]
        [HttpGet]
        public IActionResult DataSource()
        {
            try
            {
                var veiculo = ItensVeiculo.GetAllRecords(_unitOfWork);

                return Json(veiculo);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridContratoController.cs" , "DataSource" , error);
                return View(); // padronizado
            }
        }
    }

    public class ItensVeiculo
    {
        public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();

        public ItensVeiculo(
            int numitem ,
            string descricao ,
            int quantidade ,
            double valorunitario ,
            double valortotal ,
            Guid repactuacaoId
        )
        {
            try
            {
                this.numitem = numitem;
                this.descricao = descricao;
                this.quantidade = quantidade;
                this.valorunitario = valorunitario;
                this.valortotal = valortotal;
                this.repactuacaoId = repactuacaoId;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridContratoController.cs" , "ItensVeiculo" , error);
            }
        }

        public static List<ItensVeiculo> GetAllRecords(IUnitOfWork _unitOfWork)
        {
            try
            {
                var objItemVeiculos = _unitOfWork
                    .ItemVeiculoContrato.GetAll()
                    .OrderBy(o => o.NumItem);

                veiculo.Clear();

                foreach (var item in objItemVeiculos)
                {
                    veiculo.Add(
                        new ItensVeiculo(
                            (int)item.NumItem ,
                            item.Descricao ,
                            (int)item.Quantidade ,
                            (double)item.ValorUnitario ,
                            (double)(item.Quantidade * item.ValorUnitario) ,
                            item.RepactuacaoContratoId
                        )
                    );
                }

                return veiculo;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridContratoController.cs" , "GetAllRecords" , error);
                return default(List<ItensVeiculo>); // padronizado
            }
        }

        public int? numitem
        {
            get; set;
        }
        public string descricao
        {
            get; set;
        }
        public int? quantidade
        {
            get; set;
        }
        public double? valorunitario
        {
            get; set;
        }
        public double? valortotal
        {
            get; set;
        }
        public Guid repactuacaoId
        {
            get; set;
        }
    }
}
