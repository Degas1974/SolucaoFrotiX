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
    /// ║ 📌 NOME: GridAtaController                                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Controlador auxiliar para alimentação de Grids de Atas.                   ║
    /// ║    Gerencia DataSource para edição in-line de itens de veículos em Ata.      ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Fornece dados estruturados para componentes de UI (Grid) permitindo       ║
    /// ║    a manipulação de itens de atas de registro de preços.                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/GridAta                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class GridAtaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        public static List<ItensVeiculoAta> veiculo = new List<ItensVeiculoAta>();

        public class objItem
        {
            Guid RepactuacaoAtaId { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GridAtaController (Construtor)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com dependências.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • logService (ILogService): Serviço de log centralizado.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public GridAtaController(IUnitOfWork unitOfWork, ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logService = logService;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridAtaController.cs", "GridAtaController", error);
                throw;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DataSourceAta (GET)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados formatados para o grid de itens da Ata.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de ItensVeiculoAta.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DataSourceAta")]
        [HttpGet]
        public IActionResult DataSourceAta()
        {
            try
            {
                // [DADOS] Chama método estático da classe auxiliar.
                var veiculo = ItensVeiculoAta.GetAllRecords(_unitOfWork);

                // [RETORNO] Serializa lista para o grid.
                return Json(veiculo);
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "GridAtaController.cs", "DataSourceAta");
                Alerta.TratamentoErroComLinha("GridAtaController.cs", "DataSourceAta", error);
                // Retorna Ok com lista vazia para não quebrar o grid, ou erro 500
                return StatusCode(500, new { success = false, message = "Erro ao carregar dados" });
            }
        }
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ItensVeiculoAta (Helper Class)                                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Classe auxiliar (DTO/ViewModel) para estrutura do grid de atas.           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class ItensVeiculoAta
    {
        public static List<ItensVeiculoAta> veiculo = new List<ItensVeiculoAta>();

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ItensVeiculoAta (Construtor)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o DTO do item de veículo da Ata.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • numitem (int): Número do item.                                          ║
        /// ║    • descricao (string): Descrição do item.                                  ║
        /// ║    • quantidade (int): Quantidade do item.                                   ║
        /// ║    • valorunitario (double): Valor unitário.                                 ║
        /// ║    • valortotal (double): Valor total calculado.                             ║
        /// ║    • repactuacaoId (Guid): ID da repactuação.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public ItensVeiculoAta(
            int numitem,
            string descricao,
            int quantidade,
            double valorunitario,
            double valortotal,
            Guid repactuacaoId
        )
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
                Alerta.TratamentoErroComLinha("GridAtaController.cs", "ItensVeiculoAta", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAllRecords                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recupera itens de veículo da Ata e formata para grid.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • _unitOfWork (IUnitOfWork): Acesso a dados.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • List<ItensVeiculoAta>: Lista pronta para o grid.                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static List<ItensVeiculoAta> GetAllRecords(IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DADOS] Consulta itens da ata ordenados por número.
                var objItemVeiculos = _unitOfWork.ItemVeiculoAta.GetAll().OrderBy(o => o.NumItem);

                // [LISTA] Limpa cache atual para repopular.
                veiculo.Clear();

                foreach (var item in objItemVeiculos)
                {
                    // [CALCULO] Total = Quantidade * ValorUnitario.
                    veiculo.Add(
                        new ItensVeiculoAta(
                            (int)item.NumItem,
                            item.Descricao,
                            (int)item.Quantidade,
                            (double)item.ValorUnitario,
                            (double)(item.Quantidade * item.ValorUnitario),
                            item.RepactuacaoAtaId
                        )
                    );
                }

                // [RETORNO] Lista final para o grid.
                return veiculo;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GridAtaController.cs", "GetAllRecords", error);
                return new List<ItensVeiculoAta>();
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
