/* ****************************************************************************************
 * ⚡ ARQUIVO: GridContratoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoints para grids de itens de contrato e manter estruturas
 *                   auxiliares em memória durante a edição de contratos.
 *
 * 📥 ENTRADAS     : Requisições HTTP (GET) e dados obtidos do repositório via UnitOfWork.
 *
 * 📤 SAÍDAS       : JSON com listas de itens/veículos vinculados a contratos.
 *
 * 🔗 CHAMADA POR  : Páginas de Contratos (grids Syncfusion) via chamadas AJAX.
 *
 * 🔄 CHAMA        : IUnitOfWork.ItemVeiculoContrato, ItensVeiculo.GetAllRecords(), Alerta.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork, LINQ, Entity Framework.
 *
 * 📝 OBSERVAÇÕES  : Mantém lista estática para itens temporários enquanto o contrato é editado.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: GridContratoController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer dados para grids de itens de contrato (veículos/serviços)
 *                   e centralizar a montagem das listas exibidas no frontend.
 *
 * 📥 ENTRADAS     : Nenhuma via corpo; utiliza dados do banco via UnitOfWork.
 *
 * 📤 SAÍDAS       : JSON com itens formatados para o grid.
 *
 * 🔗 CHAMADA POR  : JavaScript das páginas de Contratos (Syncfusion Grid).
 *
 * 🔄 CHAMA        : ItensVeiculo.GetAllRecords(), IUnitOfWork.ItemVeiculoContrato.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork.
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
         * 🎯 OBJETIVO     : Representar payload mínimo com RepactuacaoContratoId para operações
         *                   específicas do grid (ex.: seleção/edição).
         *
         * 📥 ENTRADAS     : RepactuacaoContratoId (Guid).
         *
         * 📤 SAÍDAS       : Objeto simples para transporte de dados.
         *
         * 📝 OBSERVAÇÕES  : Classe interna usada como DTO leve.
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
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: DataSource
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar itens de veículos/serviços do contrato para o grid.
         *
         * 📥 ENTRADAS     : Nenhuma (requisição GET).
         *
         * 📤 SAÍDAS       : [IActionResult] JSON com itens formatados.
         *
         * 🔗 CHAMADA POR  : Grid Syncfusion (AJAX).
         *
         * 🔄 CHAMA        : ItensVeiculo.GetAllRecords().
         ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ CLASSE: ItensVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Modelar itens de veículos/serviços vinculados a contratos
     *                   com campos esperados pelo grid.
     *
     * 📥 ENTRADAS     : Dados dos itens obtidos via repositórios.
     *
     * 📤 SAÍDAS       : Lista estática para consumo do frontend.
     *
     * 📝 OBSERVAÇÕES  : Utiliza lista estática para armazenar resultados em memória.
     ****************************************************************************************/
    public class ItensVeiculo
    {
        // [DOC] Lista estática para cachear itens carregados para o grid
        public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();

        /****************************************************************************************
         * ⚡ FUNÇÃO: ItensVeiculo (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar item de contrato com dados e valores calculados.
         *
         * 📥 ENTRADAS     : numitem, descricao, quantidade, valorunitario, valortotal,
         *                   repactuacaoId.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ItensVeiculo.GetAllRecords().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAllRecords
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Carregar itens de veículos do contrato e preparar lista para o grid.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] _unitOfWork.
         *
         * 📤 SAÍDAS       : [List<ItensVeiculo>] lista com itens formatados.
         *
         * 🔗 CHAMADA POR  : GridContratoController.DataSource().
         *
         * 🔄 CHAMA        : _unitOfWork.ItemVeiculoContrato.GetAll(), LINQ.
         *
         * 📝 OBSERVAÇÕES  : Limpa a lista estática antes de preencher novamente.
         ****************************************************************************************/
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
