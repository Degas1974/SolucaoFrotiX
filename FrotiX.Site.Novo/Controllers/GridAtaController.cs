/* ****************************************************************************************
 * ⚡ ARQUIVO: GridAtaController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer dados para grids de itens de Atas de Registro de Preços,
 *                   mantendo lista temporária em memória.
 *
 * 📥 ENTRADAS     : Operações de grid (insert/update/delete) e parâmetros de filtro.
 *
 * 📤 SAÍDAS       : JSON com lista de ItensVeiculoAta.
 *
 * 🔗 CHAMADA POR  : Frontend de Atas (Syncfusion Grid).
 *
 * 🔄 CHAMA        : ItensVeiculoAta.GetAllRecords(), IUnitOfWork.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork, Syncfusion Grid.
 *
 * 📝 OBSERVAÇÕES  : Lista estática é usada como armazenamento temporário (sessão).
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: GridAtaController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer dados para grids de itens de Atas de Registro de Preços
 *                   Gerencia lista de veículos/serviços incluídos nas atas
 * 📥 ENTRADAS     : Nenhuma (utiliza dados estáticos/sessão)
 * 📤 SAÍDAS       : JSON com lista de ItensVeiculoAta
 * 🔗 CHAMADA POR  : JavaScript (grids Syncfusion) das páginas de Atas via AJAX
 * 🔄 CHAMA        : ItensVeiculoAta.GetAllRecords(), IUnitOfWork
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork
 *
 * 💡 CONCEITOS:
 *    - Ata de Registro de Preços: Documento que registra preços de itens/serviços
 *    - ItensVeiculoAta: Classe auxiliar que representa itens da ata
 *    - Lista estática: Armazena itens temporariamente em memória (sessão)
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
    public class GridAtaController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // [DOC] Lista estática para armazenar itens temporariamente durante edição de ata
        public static List<ItensVeiculoAta> veiculo = new List<ItensVeiculoAta>();

        /****************************************************************************************
         * ⚡ CLASSE: objItem (Helper)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Wrapper para RepactuacaoAtaId (usado em operações de grid)
         ****************************************************************************************/
        public class objItem
        {
            Guid RepactuacaoAtaId
            {
                get; set;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GridAtaController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do Unit of Work
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork
         * 📤 SAÍDAS       : Instância configurada
         * 🔗 CHAMADA POR  : ASP.NET Core DI
         ****************************************************************************************/
        public GridAtaController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridAtaController.cs" , "GridAtaController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DataSourceAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de itens de Ata de Registro de Preços para o grid
         *                   Busca todos os itens e formata para exibição
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON com lista de ItensVeiculoAta
         * ⬅️ CHAMADO POR  : JavaScript (Syncfusion Grid) da página Atas via AJAX GET
         * ➡️ CHAMA        : ItensVeiculoAta.GetAllRecords(), ItemVeiculoAta.GetAll()
         * 📝 OBSERVAÇÕES  : Converte dados do banco para objetos ItensVeiculoAta
         ****************************************************************************************/
        [Route("DataSourceAta")]
        [HttpGet]
        public IActionResult DataSourceAta()
        {
            try
            {
                var veiculo = ItensVeiculoAta.GetAllRecords(_unitOfWork);

                return Json(veiculo);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridAtaController.cs" , "DataSourceAta" , error);
                return View(); // padronizado
            }
        }
    }

    public class ItensVeiculoAta
    {
        public static List<ItensVeiculoAta> veiculo = new List<ItensVeiculoAta>();

        public ItensVeiculoAta(
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
                Alerta.TratamentoErroComLinha("GridAtaController.cs" , "ItensVeiculoAta" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAllRecords (Estática)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar todos os itens de Ata do banco e converter para lista
         *                   Calcula valor total de cada item (quantidade * valor unitário)
         * 📥 ENTRADAS     : [IUnitOfWork] _unitOfWork - Acesso ao repositório
         * 📤 SAÍDAS       : [List<ItensVeiculoAta>] Lista de itens formatados
         * ⬅️ CHAMADO POR  : DataSourceAta()
         * ➡️ CHAMA        : ItemVeiculoAta.GetAll(), Construtor ItensVeiculoAta()
         * 📝 OBSERVAÇÕES  : Limpa lista estática antes de popular (veiculo.Clear())
         ****************************************************************************************/
        public static List<ItensVeiculoAta> GetAllRecords(IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DB] Buscar todos os itens da ata ordenados por número
                var objItemVeiculos = _unitOfWork.ItemVeiculoAta.GetAll().OrderBy(o => o.NumItem);

                veiculo.Clear();

                // [LOGICA] Converter registros do banco em objetos ItensVeiculoAta com cálculo de total
                foreach (var item in objItemVeiculos)
                {
                    veiculo.Add(
                        new ItensVeiculoAta(
                            (int)item.NumItem ,
                            item.Descricao ,
                            (int)item.Quantidade ,
                            (double)item.ValorUnitario ,
                            (double)(item.Quantidade * item.ValorUnitario) ,
                            item.RepactuacaoAtaId
                        )
                    );
                }

                return veiculo;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridAtaController.cs" , "GetAllRecords" , error);
                return default(List<ItensVeiculoAta>); // padronizado
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
