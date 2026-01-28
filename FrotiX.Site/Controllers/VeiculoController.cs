/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: VeiculoController.cs                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Veiculo API
     * 🎯 OBJETIVO: Gerenciar veículos do sistema (CRUD + consultas especializadas)
     * 📋 ROTAS: /api/Veiculo/* (Get, Delete, UpdateStatusVeiculo, VeiculoContratos, etc)
     * 🔗 ENTIDADES: Veiculo, VeiculoContrato, ModeloVeiculo, MarcaVeiculo, Unidade, Combustivel, Contrato
     * 📦 DEPENDÊNCIAS: IUnitOfWork, ViewVeiculos (view materializada)
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VeiculoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculoController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * 🎯 OBJETIVO: Listar todos os veículos do sistema (usando view materializada otimizada)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { data: List<{ VeiculoId, Placa, Quilometragem, MarcaModelo, Sigla, ... }> }
         * 🔗 CHAMADA POR: Grid principal de veículos
         * 🔄 CHAMA: ViewVeiculos.GetAllReduced() - view otimizada com joins pré-calculados
         * ⚡ PERFORMANCE: Usa GetAllReduced com selector para minimizar dados trafegados
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DOC] Usa view materializada para performance otimizada em consultas frequentes
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
         * 🎯 OBJETIVO: Excluir veículo (valida dependências antes de remover)
         * 📥 ENTRADAS: model (VeiculoViewModel com VeiculoId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de veículo
         * 🔄 CHAMA: Veiculo.GetFirstOrDefault(), VeiculoContrato.GetFirstOrDefault(), Viagem.GetFirstOrDefault(), Veiculo.Remove()
         * ⚠️ VALIDAÇÕES: Impede exclusão se houver contratos ou viagens associadas
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
                        // [DOC] Validação 1: Verifica se veículo está associado a contratos
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

                        // [DOC] Validação 2: Verifica se veículo possui viagens registradas
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
         * 🎯 OBJETIVO: Alternar status do veículo (Ativo ↔ Inativo)
         * 📥 ENTRADAS: Id (VeiculoId Guid)
         * 📤 SAÍDAS: JSON { success, message, type (0=ativo, 1=inativo) }
         * 🔗 CHAMADA POR: Toggle de status no grid
         * 🔄 CHAMA: Veiculo.GetFirstOrDefault(), Veiculo.Update()
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
                        // [DOC] Toggle status: true → false (type=1) ou false → true (type=0)
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
         * 🎯 OBJETIVO: Listar veículos associados a um contrato específico
         * 📥 ENTRADAS: Id (ContratoId Guid)
         * 📤 SAÍDAS: JSON { data: List<{ VeiculoId, Placa, MarcaModelo, Sigla, CombustivelDescricao, Status }> }
         * 🔗 CHAMADA POR: Grid de veículos dentro do contrato
         * 🔄 CHAMA: Veiculo.GetAll(), VeiculoContrato.GetAll(), ModeloVeiculo.GetAll(), etc.
         * 🔀 JOINS: 6 tabelas + left join opcional em Unidade
         ****************************************************************************************/
        [HttpGet]
        [Route("VeiculoContratos")]
        public IActionResult VeiculoContratos(Guid Id)
        {
            try
            {
                // [DOC] Left join em Unidade: veículo pode não estar alocado a nenhuma unidade
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
         * ⚡ FUNÇÃO: VeiculosDoContrato (VeiculoContratosGlosa)
         * 🎯 OBJETIVO: Listar veículos elegíveis para glosa (com manutenção > 0 dias)
         * 📥 ENTRADAS: id (ContratoId Guid)
         * 📤 SAÍDAS: View com lista de veículos elegíveis
         * 🔗 CHAMADA POR: Página de glosas de contrato
         * 🔄 CHAMA: Manutencao.GetAll(), VeiculoContrato.GetAll(), Veiculo.GetAll(), etc.
         * 🎯 FILTRO: Apenas veículos com manutenções de duração > 0 dias
         ****************************************************************************************/
        [HttpGet]
        [Route("VeiculoContratosGlosa")]
        public IActionResult VeiculosDoContrato(Guid id)
        {
            try
            {
                // [DOC] Filtra veículos com manutenções de duração > 0 dias (elegíveis para glosa)
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

                // [DOC] Filtra apenas veículos no HashSet de elegíveis (performance otimizada)
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
         * 🎯 OBJETIVO: Remover associação veículo-contrato
         * 📥 ENTRADAS: model (VeiculoViewModel com VeiculoId e ContratoId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de remoção de veículo do contrato
         * 🔄 CHAMA: Veiculo.GetFirstOrDefault(), VeiculoContrato.GetFirstOrDefault(), VeiculoContrato.Remove()
         * 💾 LÓGICA: Se ContratoId do veículo for o mesmo, limpa ContratoId também
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
                            // [DOC] Se o contrato principal do veículo é o sendo removido, limpa
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
         * 🎯 OBJETIVO: Obter valor unitário de item de ata de registro de preços
         * 📥 ENTRADAS: itemAta (ItemVeiculoAtaId Guid)
         * 📤 SAÍDAS: JSON { valor }
         * 🔗 CHAMADA POR: Formulário de cadastro de veículo (preenchimento automático de valor)
         * 🔄 CHAMA: ItemVeiculoAta.GetFirstOrDefault()
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
         * 🎯 OBJETIVO: Obter valor unitário de item de contrato
         * 📥 ENTRADAS: itemContrato (ItemVeiculoId Guid)
         * 📤 SAÍDAS: JSON { valor }
         * 🔗 CHAMADA POR: Formulário de cadastro de veículo (preenchimento automático de valor)
         * 🔄 CHAMA: ItemVeiculoContrato.GetFirstOrDefault()
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
