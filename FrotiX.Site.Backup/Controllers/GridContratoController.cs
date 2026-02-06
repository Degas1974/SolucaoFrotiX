/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API)                                                                          |
 * | (IA) IDENTIDADE: GridContratoController.cs                                                              |
 * | (IA) DESCRIÇÃO: Controlador auxiliar para alimentação de Grids de Contratos.                            |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GridContratoController                                             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Controlador auxiliar para alimentação de Grids de Contratos.              ║
    /// ║    Gerencia DataSource para edição in-line de itens de veículos.             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/GridContrato                                            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class GridContratoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;
        public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();

        public class objItem
        {
            public Guid RepactuacaoContratoId { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GridContratoController (Construtor)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com dependências.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public GridContratoController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridContratoController.cs", "GridContratoController", error);
                throw;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DataSource (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a fonte de dados para o grid de itens de veículos do contrato.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de itens.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DataSource")]
        [HttpGet]
        public IActionResult DataSource()
        {
            try
            {
                // [DADOS] Recupera registros formatados para o grid.
                var veiculo = ItensVeiculo.GetAllRecords(_unitOfWork);
                // [RETORNO] Serializa lista para o grid.
                return Json(veiculo);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "GridContratoController.cs", "DataSource");
                Alerta.TratamentoErroComLinha("GridContratoController.cs", "DataSource", error);
                return StatusCode(500, new { success = false, message = "Erro ao carregar dados" });
            }
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ItensVeiculo (Helper Class)                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Classe auxiliar (DTO/ViewModel) para estrutura do grid de contratos.      ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class ItensVeiculo
    {
        public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ItensVeiculo (Construtor)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o DTO do item de veículo do contrato.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • numitem (int): Número do item.                                          ║
        /// ║    • descricao (string): Descrição do item.                                  ║
        /// ║    • quantidade (int): Quantidade do item.                                   ║
        /// ║    • valorunitario (double): Valor unitário.                                 ║
        /// ║    • valortotal (double): Valor total calculado.                             ║
        /// ║    • repactuacaoId (Guid): ID da repactuação.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public ItensVeiculo(int numitem, string descricao, int quantidade, double valorunitario, double valortotal, Guid repactuacaoId)
        {
            try
            {
                // [DADOS] Mapeia propriedades do DTO.
                this.numitem = numitem;
                this.descricao = descricao;
                this.quantidade = quantidade;
                this.valorunitario = valorunitario;
                this.valortotal = valortotal;
                this.repactuacaoId = repactuacaoId;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridContratoController.cs", "ItensVeiculo", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAllRecords                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recupera itens de veículo do contrato e formata para o grid.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • _unitOfWork (IUnitOfWork): Acesso a dados.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • List<ItensVeiculo>: Lista pronta para o grid.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static List<ItensVeiculo> GetAllRecords(IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DADOS] Consulta itens do contrato ordenados por número.
                var objItemVeiculos = _unitOfWork
                    .ItemVeiculoContrato.GetAll()
                    .OrderBy(o => o.NumItem);

                // [LISTA] Limpa cache atual para repopular.
                veiculo.Clear();

                foreach (var item in objItemVeiculos)
                {
                    // [CALCULO] Total = Quantidade * ValorUnitario.
                    veiculo.Add(new ItensVeiculo(
                        (int)(item.NumItem ?? 0),
                        item.Descricao,
                        (int)(item.Quantidade ?? 0),
                        (double)(item.ValorUnitario ?? 0),
                        (double)((item.Quantidade ?? 0) * (item.ValorUnitario ?? 0)),
                        item.RepactuacaoContratoId
                    ));
                }

                // [RETORNO] Lista final para o grid.
                return veiculo;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridContratoController.cs", "GetAllRecords", error);
                return new List<ItensVeiculo>();
            }
        }

        public int? numitem { get; set; }
        public string descricao { get; set; }
        public int? quantidade { get; set; }
        public double? valorunitario { get; set; }
        public double? valortotal { get; set; }
        public Guid repactuacaoId { get; set; }
    }
}
