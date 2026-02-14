/* ****************************************************************************************
 * ⚡ ARQUIVO: VeiculoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar veículos, vínculos com contratos/atas e status de ativação.
 *
 * 📥 ENTRADAS     : IDs e modelos de veículo/contrato.
 *
 * 📤 SAÍDAS       : JSON com listas e status das operações.
 *
 * 🔗 CHAMADA POR  : Telas de veículos e contratos.
 *
 * 🔄 CHAMA        : IUnitOfWork (Veiculo, ViewVeiculos, VeiculoContrato, Viagem,
 *                   ItemVeiculoAta/Contrato).
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Models.Api;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: VeiculoController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para listar, excluir e gerenciar vínculos de veículos.
     *
     * 📥 ENTRADAS     : IDs e modelos de veículo/contrato.
     *
     * 📤 SAÍDAS       : JSON com dados e mensagens.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        private readonly ILogger<VeiculoController> _logger;

        /****************************************************************************************
         * ⚡ FUNÇÃO: VeiculoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork, LogService e Logger.
         *
         * 📥 ENTRADAS     : unitOfWork, logService, logger.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public VeiculoController(IUnitOfWork unitOfWork, ILogService logService, ILogger<VeiculoController> logger)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logService = logService;
                _logger = logger;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculoController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAll
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos os veículos ativos com resposta padronizada.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : ApiResponse com lista de veículos.
         *
         * 🔗 CHAMADA POR  : Frontend via AJAX/Fetch.
         ****************************************************************************************/
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var requestId = Guid.NewGuid().ToString("N")[..8];

            try
            {
                _logger.LogInformation("[{RequestId}] Iniciando GetAll de veículos", requestId);

                var veiculos = _unitOfWork.ViewVeiculos.GetAll();

                var result = veiculos
                    .Where(v => v.VeiculoId != Guid.Empty && v.Status == true)
                    .Select(v => new
                    {
                        v.VeiculoId,
                        v.Placa,
                        v.VeiculoCompleto,
                        v.MarcaModelo,
                        v.Categoria,
                        v.Sigla,
                        v.Status
                    })
                    .OrderBy(v => v.Placa)
                    .ToList();

                _logger.LogInformation("[{RequestId}] Retornando {Count} veículos", requestId, result.Count);

                // Adiciona header de rastreamento
                Response.Headers["X-Request-Id"] = requestId;

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = result,
                    Message = $"{result.Count} veículo(s) encontrado(s)",
                    RequestId = requestId
                });
            }
            catch (Exception ex)
            {
                _logService.Error(
                    $"[{requestId}] Erro ao carregar veículos: {ex.Message}",
                    ex,
                    "VeiculoController.cs",
                    nameof(GetAll)
                );

                _logger.LogError(ex, "[{RequestId}] Erro ao carregar veículos", requestId);

                Response.Headers["X-Request-Id"] = requestId;

                return StatusCode(500, ApiResponse<object>.FromException(ex,
#if DEBUG
                    includeDetails: true
#else
                    includeDetails: false
#endif
                ));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos a partir da view ViewVeiculos.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com data (lista de veículos).
         *
         * 🔗 CHAMADA POR  : Grid de veículos.
         ****************************************************************************************/
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            try
            {
                var objVeiculos = _unitOfWork
                    .ViewVeiculos.GetAllReduced(selector: vv => new
                    {
                        vv.VeiculoId ,
                        vv.Placa ,
                        vv.Quilometragem ,
                        vv.MarcaModelo ,
                        vv.Sigla ,
                        vv.Descricao ,
                        vv.Consumo ,
                        vv.OrigemVeiculo ,
                        vv.DataAlteracao ,
                        vv.NomeCompleto ,
                        vv.VeiculoReserva ,
                        vv.Status ,
                        vv.CombustivelId ,
                        vv.VeiculoProprio ,
                    })
                    .ToList();

                return Json(new
                {
                    data = objVeiculos
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "Get" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover veículo quando não houver vínculos de contrato ou viagem.
         *
         * 📥 ENTRADAS     : model (VeiculoViewModel).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de exclusão no grid.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault(), VeiculoContrato.GetFirstOrDefault(),
         *                   Viagem.GetFirstOrDefault(), Veiculo.Remove(), UnitOfWork.Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(VeiculoViewModel model)
        {
            try
            {
                if (model != null && model.VeiculoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == model.VeiculoId
                    );
                    if (objFromDb != null)
                    {
                        var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u =>
                            u.VeiculoId == model.VeiculoId
                        );
                        if (veiculoContrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o veículo. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(u =>
                            u.VeiculoId == model.VeiculoId
                        );
                        if (objViagem != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o veículo. Ele está associado a uma ou mais viagens!" ,
                                }
                            );
                        }

                        _unitOfWork.Veiculo.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Veículo removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar veículo"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar veículo"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status ativo/inativo do veículo.
         *
         * 📥 ENTRADAS     : Id (Guid do veículo).
         *
         * 📤 SAÍDAS       : JSON com success, message e type.
         *
         * 🔗 CHAMADA POR  : Toggle de status no grid de veículos.
         ****************************************************************************************/
        [Route("UpdateStatusVeiculo")]
        public JsonResult UpdateStatusVeiculo(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.VeiculoId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Veículo [Nome: {0}] (Inativo)" ,
                                objFromDb.Placa
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Veículo  [Nome: {0}] (Ativo)" ,
                                objFromDb.Placa
                            );
                            type = 0;
                        }
                        _unitOfWork.Veiculo.Update(objFromDb);
                    }
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "UpdateStatusVeiculo" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VeiculoContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos vinculados a um contrato específico.
         *
         * 📥 ENTRADAS     : Id (Guid do contrato).
         *
         * 📤 SAÍDAS       : JSON com data (lista de veículos).
         *
         * 🔗 CHAMADA POR  : Tela de contratos.
         ****************************************************************************************/
        [HttpGet]
        [Route("VeiculoContratos")]
        public IActionResult VeiculoContratos(Guid Id)
        {
            try
            {
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join vc in _unitOfWork.VeiculoContrato.GetAll()
                        on v.VeiculoId equals vc.VeiculoId
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    join u in _unitOfWork.Unidade.GetAll()
                        on v.UnidadeId equals u.UnidadeId
                        into ud
                    from udResult in ud.DefaultIfEmpty()
                    join c in _unitOfWork.Combustivel.GetAll()
                        on v.CombustivelId equals c.CombustivelId
                    where vc.ContratoId == Id
                    select new
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                        Sigla = udResult != null ? udResult.Sigla : "" ,
                        CombustivelDescricao = c.Descricao ,
                        v.Status ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculoContratos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar veículos do contrato"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VeiculosDoContrato (Glosa)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos do contrato elegíveis a glosa por manutenção.
         *
         * 📥 ENTRADAS     : id (Guid do contrato).
         *
         * 📤 SAÍDAS       : JSON com data (lista de veículos elegíveis).
         *
         * 🔗 CHAMADA POR  : Processo de glosa.
         *
         * 🔄 CHAMA        : Manutencao.GetAll(), VeiculoContrato.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("VeiculoContratosGlosa")]
        public IActionResult VeiculosDoContrato(Guid id)
        {
            try
            {
                var manutencoes = _unitOfWork.Manutencao.GetAll();
                var veiculosElegiveis = new HashSet<Guid>(
                    manutencoes
                        .Where(m =>
                            m.VeiculoId.HasValue
                            && m.DataSolicitacao.HasValue
                            && m.DataDevolucao.HasValue
                            && (m.DataDevolucao.Value.Date - m.DataSolicitacao.Value.Date).TotalDays
                                > 0
                        )
                        .Select(m => m.VeiculoId.Value)
                        .Distinct()
                );

                var veiculosContrato = _unitOfWork
                    .VeiculoContrato.GetAll()
                    .Where(vc => vc.ContratoId == id);

                var veiculos = _unitOfWork.Veiculo.GetAll();
                var modelos = _unitOfWork.ModeloVeiculo.GetAll();
                var marcas = _unitOfWork.MarcaVeiculo.GetAll();
                var unidades = _unitOfWork.Unidade.GetAll();
                var combustiveis = _unitOfWork.Combustivel.GetAll();

                var result = (
                    from vc in veiculosContrato
                    where vc != null && veiculosElegiveis.Contains(vc?.VeiculoId ?? Guid.Empty)
                    join v in veiculos on vc.VeiculoId equals v.VeiculoId
                    join m in modelos on v.ModeloId equals m.ModeloId
                    join ma in marcas on v.MarcaId equals ma.MarcaId
                    join u in unidades on v.UnidadeId equals u.UnidadeId into ud
                    from udResult in ud.DefaultIfEmpty()
                    join c in combustiveis on v.CombustivelId equals c.CombustivelId
                    select new
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                        Sigla = udResult != null ? udResult.Sigla : "" ,
                        CombustivelDescricao = c.Descricao ,
                        v.Status ,
                    }
                ).ToList();

                return View(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculosDoContrato" , error);
                return View();
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo veículo-contrato.
         *
         * 📥 ENTRADAS     : model (VeiculoViewModel).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de desvincular veículo do contrato.
         *
         * 🔄 CHAMA        : VeiculoContrato.GetFirstOrDefault(), Remove(), UnitOfWork.Save().
         ****************************************************************************************/
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(VeiculoViewModel model)
        {
            try
            {
                if (model != null && model.VeiculoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == model.VeiculoId
                    );
                    if (objFromDb != null)
                    {
                        var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u =>
                            u.VeiculoId == model.VeiculoId && u.ContratoId == model.ContratoId
                        );
                        if (veiculoContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Veiculo.Update(objFromDb);
                            }
                            _unitOfWork.VeiculoContrato.Remove(veiculoContrato);
                            _unitOfWork.Save();
                            return Json(
                                new
                                {
                                    success = true ,
                                    message = "Veículo removido com sucesso"
                                }
                            );
                        }
                        return Json(new
                        {
                            success = false ,
                            message = "Erro ao remover veículo"
                        });
                    }
                    return Json(new
                    {
                        success = false ,
                        message = "Erro ao remover veículo"
                    });
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover veículo"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "DeleteContrato" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar contrato"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: SelecionaValorMensalAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar valor mensal do item de ata para cálculo de veículo.
         *
         * 📥 ENTRADAS     : itemAta (Guid).
         *
         * 📤 SAÍDAS       : JSON com valor mensal.
         *
         * 🔗 CHAMADA POR  : Seleção de item de ata.
         ****************************************************************************************/
        [Route("SelecionaValorMensalAta")]
        [HttpGet]
        public JsonResult SelecionaValorMensalAta(Guid itemAta)
        {
            try
            {
                var ItemAta = _unitOfWork.ItemVeiculoAta.GetFirstOrDefault(i =>
                    i.ItemVeiculoAtaId == itemAta
                );

                return new JsonResult(new
                {
                    valor = ItemAta.ValorUnitario
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "VeiculoController.cs" ,
                    "SelecionaValorMensalAta" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: SelecionaValorMensalContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar valor mensal do item de contrato para cálculo de veículo.
         *
         * 📥 ENTRADAS     : itemContrato (Guid).
         *
         * 📤 SAÍDAS       : JSON com valor mensal.
         *
         * 🔗 CHAMADA POR  : Seleção de item de contrato.
         ****************************************************************************************/
        [Route("SelecionaValorMensalContrato")]
        [HttpGet]
        public JsonResult SelecionaValorMensalContrato(Guid itemContrato)
        {
            try
            {
                var ItemContrato = _unitOfWork.ItemVeiculoContrato.GetFirstOrDefault(i =>
                    i.ItemVeiculoId == itemContrato
                );

                return new JsonResult(new
                {
                    valor = ItemContrato.ValorUnitario
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "VeiculoController.cs" ,
                    "SelecionaValorMensalContrato" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }
    }
}
