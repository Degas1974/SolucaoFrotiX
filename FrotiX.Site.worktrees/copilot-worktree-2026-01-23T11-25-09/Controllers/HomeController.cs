/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (MIXED)                                                                        |
 * | (IA) IDENTIDADE: HomeController.cs                                                                      |
 * | (IA) DESCRIÇÃO: Controlador raiz e exemplos de integração Syncfusion DataManager.                       |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

using Microsoft.AspNetCore.Mvc;
using FrotiX.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: HomeController                                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Controlador padrão para a rota raiz e exemplos do Syncfusion DataManager. ║
    /// ║    Mantém exemplos de Grids e CRUDs genéricos para referência.               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: MIXED (MVC + API)                                                 ║
    /// ║    • Rota base: /api/Home                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: HomeController (Construtor)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com serviço de logs.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public HomeController(ILogService log)
        {
            try
            {
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs", "HomeController", error);
            }
        }

        public static List<OrdersDetails> order = new List<OrdersDetails>();

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Index (GET)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza a view principal da aplicação (Dashboard ou Landing).          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("/")]
        [HttpGet("/Home/Index")]
        public IActionResult Index()
        {
            try
            {
                // [VIEW] Retorna a view principal.
                return View();
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "HomeController.cs", "Index");
                Alerta.TratamentoErroComLinha("HomeController.cs", "Index", error);
                return View();
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DataSource (GET)                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados de exemplo (OrdersDetails) para grids de teste.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de OrdersDetails.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DataSource")]
        [HttpGet]
        public IActionResult DataSource()
        {
            try
            {
                // [DADOS] Carrega registros de exemplo.
                var order = OrdersDetails.GetAllRecords();
                // [RETORNO] Serializa lista para o grid.
                return Json(order);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "HomeController.cs", "DataSource");
                Alerta.TratamentoErroComLinha("HomeController.cs", "DataSource", error);
                return StatusCode(500, new { success = false, message = "Erro ao carregar dados" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UrlDatasource (POST)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Manipula requisições paginadas para grids Syncfusion.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dm (Data): Modelo de paginação do DataManager.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista e/ou total.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("UrlDatasource")]
        public IActionResult UrlDatasource([FromBody] Data dm)
        {
            try
            {
                // [DADOS] Carrega registros de exemplo.
                var order = OrdersDetails.GetAllRecords();
                var list = order.ToList();
                int count = order.Count();

                if (dm.requiresCounts)
                {
                    // [PAGINACAO] Retorna paginação com contagem total.
                    return Json(new
                    {
                        result = list.Skip(dm.skip).Take(dm.take),
                        count = count
                    });
                }

                // [RETORNO] Retorna lista completa.
                return Json(list);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "HomeController.cs", "UrlDatasource");
                Alerta.TratamentoErroComLinha("HomeController.cs", "UrlDatasource", error);
                return StatusCode(500, new { success = false, message = "Erro ao processar dados" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CrudUpdate (POST)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa atualizações e inserções para OrdersDetails (exemplo).          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • value (CRUDModel<OrdersDetails>): Payload de CRUD.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com o item processado.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("CrudUpdate")]
        public ActionResult CrudUpdate([FromBody] CRUDModel<OrdersDetails> value)
        {
            try
            {
                if (value.action == "update")
                {
                    // [DADOS] Localiza registro existente.
                    var ord = value.value;
                    OrdersDetails val = OrdersDetails.GetAllRecords()
                        .FirstOrDefault(or => or.orderid == ord.orderid);

                    if (val != null)
                    {
                        // [ATUALIZACAO] Atualiza propriedades do registro.
                        val.orderid = ord.orderid;
                        val.employeeid = ord.employeeid;
                        val.customerid = ord.customerid;
                        val.freight = ord.freight;
                        val.orderdate = ord.orderdate;
                        val.shipcity = ord.shipcity;
                        val.shipcountry = ord.shipcountry;
                    }
                }
                else if (value.action == "insert")
                {
                    // [INSERCAO] Adiciona registro no topo da lista.
                    OrdersDetails.GetAllRecords().Insert(0, value.value);
                }

                // [RETORNO] Retorna item processado.
                return Json(value.value);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "HomeController.cs", "CrudUpdate");
                Alerta.TratamentoErroComLinha("HomeController.cs", "CrudUpdate", error);
                return StatusCode(500, new { success = false, message = "Erro na operação CRUD" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Data                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Modelo auxiliar para paginação do DataManager do Syncfusion.             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public class Data
        {
            public bool requiresCounts { get; set; }
            public int skip { get; set; }
            public int take { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CRUDModel                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Modelo genérico para operações CRUD do Syncfusion DataManager.           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public class CRUDModel<T> where T : class
        {
            public string action { get; set; }
            public string table { get; set; }
            public string keyColumn { get; set; }
            public object key { get; set; }
            public T value { get; set; }
            public List<T> added { get; set; }
            public List<T> changed { get; set; }
            public List<T> deleted { get; set; }
            public IDictionary<string, object> @params { get; set; }
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OrdersDetails (Helper Class)                                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Entidade de teste para demonstração de recursos de Grid.                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class OrdersDetails
    {
        public static List<OrdersDetails> order = new List<OrdersDetails>();

        public OrdersDetails() { }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OrdersDetails (Construtor)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa a entidade de teste com dados de pedido.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • orderid (int): ID do pedido.                                            ║
        /// ║    • customerid (string): Cliente.                                           ║
        /// ║    • employeeid (int): Funcionário.                                          ║
        /// ║    • freight (double): Frete.                                                ║
        /// ║    • verified (bool): Status de verificação.                                 ║
        /// ║    • orderdate (DateTime): Data do pedido.                                   ║
        /// ║    • shipcity (string): Cidade de envio.                                     ║
        /// ║    • shipname (string): Destinatário.                                        ║
        /// ║    • shipcountry (string): País.                                             ║
        /// ║    • shippeddate (DateTime): Data de envio.                                  ║
        /// ║    • shipaddress (string): Endereço.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public OrdersDetails(int orderid, string customerid, int employeeid, double freight, bool verified, DateTime orderdate, string shipcity, string shipname, string shipcountry, DateTime shippeddate, string shipaddress)
        {
            try
            {
                // [DADOS] Mapeia propriedades do pedido.
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
                Alerta.TratamentoErroComLinha("HomeController.cs", "OrdersDetails", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAllRecords                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera lista de dados de exemplo para grids de teste.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • List<OrdersDetails>: Lista de registros de exemplo.                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static List<OrdersDetails> GetAllRecords()
        {
            try
            {
                if (order.Count == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 10; i++)
                    {
                        // [DADOS] Adiciona conjunto de registros simulados.
                        order.Add(new OrdersDetails(code + 1, "ALFKI", i + 0, 2.3 * i, false, new DateTime(1991, 05, 15), "Berlin", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                        order.Add(new OrdersDetails(code + 2, "ANATR", i + 2, 3.3 * i, true, new DateTime(1990, 04, 04), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                        order.Add(new OrdersDetails(code + 3, "ANTON", i + 1, 4.3 * i, true, new DateTime(1957, 11, 30), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new OrdersDetails(code + 4, "BLONP", i + 3, 5.3 * i, false, new DateTime(1930, 10, 22), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                        order.Add(new OrdersDetails(code + 5, "BOLID", i + 4, 6.3 * i, true, new DateTime(1953, 02, 18), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                        code += 5;
                    }
                }
                // [RETORNO] Lista carregada.
                return order;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("HomeController.cs", "GetAllRecords", error);
                return new List<OrdersDetails>();
            }
        }

        public int? orderid { get; set; }
        public string customerid { get; set; }
        public int? employeeid { get; set; }
        public double? freight { get; set; }
        public string shipcity { get; set; }
        public bool verified { get; set; }
        public DateTime orderdate { get; set; }
        public string shipname { get; set; }
        public string shipcountry { get; set; }
        public DateTime shippeddate { get; set; }
        public string shipaddress { get; set; }
    }
}
