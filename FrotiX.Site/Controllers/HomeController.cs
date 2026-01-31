/* ****************************************************************************************
 * ⚡ ARQUIVO: HomeController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Controlar a página inicial (Dashboard) e prover dados de demonstração
 *                   para grids (OrdersDetails) usados em exemplos de UI.
 *
 * 📥 ENTRADAS     : Requisições GET/POST com parâmetros de paginação e CRUD.
 *
 * 📤 SAÍDAS       : Views (Index) e JSON com dados simulados.
 *
 * 🔗 CHAMADA POR  : Navegação principal do sistema e grids de teste no frontend.
 *
 * 🔄 CHAMA        : OrdersDetails.GetAllRecords(), LINQ (Skip/Take).
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, LINQ, classes auxiliares locais.
 *
 * 📝 OBSERVAÇÕES  : Código de demonstração; OrdersDetails não representa entidade real.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: HomeController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Exibir a Home/Dashboard e simular endpoints de grid para testes.
 *
 * 📥 ENTRADAS     : Data (paginação) e CRUDModel (operações CRUD do grid).
 *
 * 📤 SAÍDAS       : View Index e JSON com registros de OrdersDetails.
 *
 * 🔗 CHAMADA POR  : Rotas padrão (/) e JavaScript de grids de exemplo.
 *
 * 🔄 CHAMA        : OrdersDetails.GetAllRecords().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC.
 *
 * ⚠️ ATENÇÃO      : Endpoints voltados para demo; não refletir regras de negócio reais.
 ****************************************************************************************/
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class HomeController :Controller
    {
        // [DOC] Lista estática para exemplo/demonstração (não utilizada em produção)
        public static List<OrdersDetails> order = new List<OrdersDetails>();

        /****************************************************************************************
         * ⚡ FUNÇÃO: Index
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Renderizar a página inicial (Home/Dashboard).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : [IActionResult] View Index.cshtml.
         *
         * 🔗 CHAMADA POR  : Navegação padrão (/).
         ****************************************************************************************/
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "Index" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DataSource
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar a lista completa de OrdersDetails para o grid de demonstração.
         *
         * 📥 ENTRADAS     : Nenhuma (requisição GET).
         *
         * 📤 SAÍDAS       : [IActionResult] JSON com lista de OrdersDetails.
         *
         * 🔗 CHAMADA POR  : Grids de teste no frontend.
         *
         * 🔄 CHAMA        : OrdersDetails.GetAllRecords().
         ****************************************************************************************/
        [Route("DataSource")]
        [HttpGet]
        public IActionResult DataSource()
        {
            try
            {
                var order = OrdersDetails.GetAllRecords();
                return Json(order);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "DataSource" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UrlDatasource
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista paginada e contagem opcional para grids.
         *
         * 📥 ENTRADAS     : [Data] dm - Parâmetros de paginação (requiresCounts, skip, take).
         *
         * 📤 SAÍDAS       : JSON com result e count (quando solicitado) ou lista simples.
         *
         * 🔗 CHAMADA POR  : Grids com paginação/virtualização.
         *
         * 🔄 CHAMA        : OrdersDetails.GetAllRecords(), LINQ Skip/Take.
         ****************************************************************************************/
        public IActionResult UrlDatasource([FromBody] Data dm)
        {
            try
            {
                var order = OrdersDetails.GetAllRecords();
                var Data = order.ToList();
                int count = order.Count();
                return dm.requiresCounts
                    ? Json(new
                    {
                        result = Data.Skip(dm.skip).Take(dm.take) ,
                        count = count
                    })
                    : Json(Data);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "UrlDatasource" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CrudUpdate
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Simular operações CRUD em memória para o grid de demonstração.
         *
         * 📥 ENTRADAS     : [CRUDModel<OrdersDetails>] value - Ação e dados do registro.
         *
         * 📤 SAÍDAS       : JSON com o registro processado.
         *
         * 🔗 CHAMADA POR  : Grids de teste com edição inline.
         *
         * 🔄 CHAMA        : OrdersDetails.GetAllRecords().
         *
         * 📝 OBSERVAÇÕES  : Não persiste em banco; atua sobre lista estática.
         ****************************************************************************************/
        public ActionResult CrudUpdate([FromBody] CRUDModel<OrdersDetails> value)
        {
            try
            {
                if (value.action == "update")
                {
                    var ord = value.value;
                    OrdersDetails val = OrdersDetails
                        .GetAllRecords()
                        .Where(or => or.orderid == ord.orderid)
                        .FirstOrDefault();
                    val.orderid = ord.orderid;
                    val.employeeid = ord.employeeid;
                    val.customerid = ord.customerid;
                    val.freight = ord.freight;
                    val.orderdate = ord.orderdate;
                    val.shipcity = ord.shipcity;
                    val.shipcountry = ord.shipcountry;
                }
                else if (value.action == "insert")
                {
                    OrdersDetails.GetAllRecords().Insert(0 , value.value);
                }
                return Json(value.value);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "CrudUpdate" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ CLASSE: Data
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Transportar parâmetros de paginação e contagem para grids.
         *
         * 📥 ENTRADAS     : requiresCounts, skip, take.
         *
         * 📤 SAÍDAS       : Objeto de request.
         ****************************************************************************************/
        public class Data
        {
            public bool requiresCounts
            {
                get; set;
            }
            public int skip
            {
                get; set;
            }
            public int take
            {
                get; set;
            }
        }

        /****************************************************************************************
         * ⚡ CLASSE: CRUDModel<T>
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Representar payload de operações CRUD do grid (insert/update/delete).
         *
         * 📥 ENTRADAS     : ação, key, value e coleções added/changed/deleted.
         *
         * 📤 SAÍDAS       : Estrutura para processar alterações no grid.
         ****************************************************************************************/
        public class CRUDModel<T>
            where T : class
        {
            public string action
            {
                get; set;
            }

            public string table
            {
                get; set;
            }

            public string keyColumn
            {
                get; set;
            }

            public object key
            {
                get; set;
            }

            public T value
            {
                get; set;
            }

            public List<T> added
            {
                get; set;
            }

            public List<T> changed
            {
                get; set;
            }

            public List<T> deleted
            {
                get; set;
            }

            public IDictionary<string , object> @params
            {
                get; set;
            }
        }
    }

    /****************************************************************************************
     * ⚡ CLASSE: OrdersDetails
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Modelar registros de pedidos fictícios para demonstração de grids.
     *
     * 📥 ENTRADAS     : Dados simulados de pedido.
     *
     * 📤 SAÍDAS       : Lista estática com registros de demonstração.
     *
     * 📝 OBSERVAÇÕES  : Conteúdo usado apenas para testes/UX, não é dado real do FrotiX.
     ****************************************************************************************/
    public class OrdersDetails
    {
        public static List<OrdersDetails> order = new List<OrdersDetails>();

        /****************************************************************************************
         * ⚡ FUNÇÃO: OrdersDetails (Construtor vazio)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar instância vazia para uso em grids de demonstração.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Instância criada.
         ****************************************************************************************/
        public OrdersDetails()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "OrdersDetails" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OrdersDetails (Construtor completo)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar instância com dados de pedido fictício.
         *
         * 📥 ENTRADAS     : orderid, customerid, employeeid, freight, verified, orderdate,
         *                   shipcity, shipname, shipcountry, shippeddate, shipaddress.
         *
         * 📤 SAÍDAS       : Instância configurada.
         ****************************************************************************************/
        public OrdersDetails(
            int orderid ,
            string customerid ,
            int employeeid ,
            double freight ,
            bool verified ,
            DateTime orderdate ,
            string shipcity ,
            string shipname ,
            string shipcountry ,
            DateTime shippeddate ,
            string shipaddress
        )
        {
            try
            {
                this.orderid = orderid;
                this.customerid = customerid;
                this.employeeid = employeeid;
                this.freight = freight;
                this.shipcity = shipcity;
                this.verified = verified;
                this.orderdate = orderdate;
                this.shipname = shipname;
                this.shipcountry = shipcountry;
                this.shippeddate = shippeddate;
                this.shipaddress = shipaddress;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "OrdersDetails" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAllRecords
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar e retornar lista de pedidos fictícios para o grid.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : [List<OrdersDetails>] lista com dados de demonstração.
         *
         * 📝 OBSERVAÇÕES  : Se a lista estiver vazia, cria um conjunto padrão de registros.
         ****************************************************************************************/
        public static List<OrdersDetails> GetAllRecords()
        {
            try
            {
                if (order.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 10; i++)
                    {
                        order.Add(
                            new OrdersDetails(
                                code + 1 ,
                                "ALFKI" ,
                                i + 0 ,
                                2.3 * i ,
                                false ,
                                new DateTime(1991 , 05 , 15) ,
                                "Berlin" ,
                                "Simons bistro" ,
                                "Denmark" ,
                                new DateTime(1996 , 7 , 16) ,
                                "Kirchgasse 6"
                            )
                        );
                        order.Add(
                            new OrdersDetails(
                                code + 2 ,
                                "ANATR" ,
                                i + 2 ,
                                3.3 * i ,
                                true ,
                                new DateTime(1990 , 04 , 04) ,
                                "Madrid" ,
                                "Queen Cozinha" ,
                                "Brazil" ,
                                new DateTime(1996 , 9 , 11) ,
                                "Avda. Azteca 123"
                            )
                        );
                        order.Add(
                            new OrdersDetails(
                                code + 3 ,
                                "ANTON" ,
                                i + 1 ,
                                4.3 * i ,
                                true ,
                                new DateTime(1957 , 11 , 30) ,
                                "Cholchester" ,
                                "Frankenversand" ,
                                "Germany" ,
                                new DateTime(1996 , 10 , 7) ,
                                "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"
                            )
                        );
                        order.Add(
                            new OrdersDetails(
                                code + 4 ,
                                "BLONP" ,
                                i + 3 ,
                                5.3 * i ,
                                false ,
                                new DateTime(1930 , 10 , 22) ,
                                "Marseille" ,
                                "Ernst Handel" ,
                                "Austria" ,
                                new DateTime(1996 , 12 , 30) ,
                                "Magazinweg 7"
                            )
                        );
                        order.Add(
                            new OrdersDetails(
                                code + 5 ,
                                "BOLID" ,
                                i + 4 ,
                                6.3 * i ,
                                true ,
                                new DateTime(1953 , 02 , 18) ,
                                "Tsawassen" ,
                                "Hanari Carnes" ,
                                "Switzerland" ,
                                new DateTime(1997 , 12 , 3) ,
                                "1029 - 12th Ave. S."
                            )
                        );
                        code += 5;
                    }
                }
                return order;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs" , "GetAllRecords" , error);
                return default(List<OrdersDetails>); // padronizado
            }
        }

        public int? orderid
        {
            get; set;
        }
        public string customerid
        {
            get; set;
        }
        public int? employeeid
        {
            get; set;
        }
        public double? freight
        {
            get; set;
        }
        public string shipcity
        {
            get; set;
        }
        public bool verified
        {
            get; set;
        }
        public DateTime orderdate
        {
            get; set;
        }

        public string shipname
        {
            get; set;
        }

        public string shipcountry
        {
            get; set;
        }

        public DateTime shippeddate
        {
            get; set;
        }
        public string shipaddress
        {
            get; set;
        }
    }
}
