using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  VEÍCULOS POR UNIDADE                                                               #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: VeiculosUnidadeController                                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de veículos vinculados a unidades.                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/VeiculosUnidade                                       ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosUnidadeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VeiculosUnidadeController (Construtor)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public VeiculosUnidadeController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs", "VeiculosUnidadeController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista veículos vinculados à unidade informada.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID da unidade.                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com veículos da unidade.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            try
            {
                // [DADOS] Consulta com joins.
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll() // Busca todos os veículos
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId // Junta com modelos
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId // Junta com marcas
                    join u in _unitOfWork.Unidade.GetAll() on v.UnidadeId equals u.UnidadeId // Junta com unidades
                    join c in _unitOfWork.Combustivel.GetAll() // Junta com combustíveis
                        on v.CombustivelId equals c.CombustivelId
                    join ct in _unitOfWork.Contrato.GetAll() on v.ContratoId equals ct.ContratoId // Junta com contratos
                    join f in _unitOfWork.Fornecedor.GetAll() // Junta com fornecedores
                        on ct.FornecedorId equals f.FornecedorId
                    join us in _unitOfWork.AspNetUsers.GetAll() on v.UsuarioIdAlteracao equals us.Id // Junta com usuários
                    where v.UnidadeId == id // [FILTRO] Unidade
                    select new // [DADOS] Seleciona as propriedades desejadas em um objeto anônimo
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                        u.Sigla ,
                        CombustivelDescricao = c.Descricao ,
                        ContratoVeiculo = ct.AnoContrato
                            + "/"
                            + ct.NumeroContrato
                            + " - "
                            + f.DescricaoFornecedor ,
                        v.Status ,
                        DatadeAlteracao = v.DataAlteracao?.ToString("dd/MM/yy") ,
                        us.NomeCompleto ,
                        u.UnidadeId ,
                    }
                ).ToList(); // Converte o resultado para uma lista

                // [RETORNO] Lista de veículos.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs" , "Get" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Desvincula veículo da unidade (UnidadeId = Guid.Empty).                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (VeiculoViewModel): Dados com ID do veículo.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(VeiculoViewModel model)
        {
            try
            {
                // [VALIDACAO] Modelo e ID.
                if (model != null && model.VeiculoId != Guid.Empty)
                {
                    // [DADOS] Busca veículo.
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == model.VeiculoId
                    );
                    // [VALIDACAO] Veículo encontrado.
                    if (objFromDb != null)
                    {
                        // [ACAO] Remove vínculo com unidade.
                        objFromDb.UnidadeId = Guid.Empty;
                        _unitOfWork.Veiculo.Update(objFromDb); // Atualiza o veículo
                        _unitOfWork.Save(); // Salva as mudanças

                        // [RETORNO] Sucesso.
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Veículo removido com sucesso"
                            }
                        );
                    }
                }
                // [RETORNO] Falha padrão.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar veículo"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs" , "Delete" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar veículo"
                });
            }
        }
    }
}
