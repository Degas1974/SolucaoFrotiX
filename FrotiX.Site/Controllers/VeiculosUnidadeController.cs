/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: VeiculosUnidadeController.cs                                     ║
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
     * ⚡ CONTROLLER: VeiculosUnidade API
     * 🎯 OBJETIVO: Gerenciar veículos associados a uma unidade específica
     * 📋 ROTAS: /api/VeiculosUnidade/* (Get, Delete)
     * 🔗 ENTIDADES: Veiculo, Unidade, ModeloVeiculo, MarcaVeiculo, Combustivel, Contrato, Fornecedor
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosUnidadeController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VeiculosUnidadeController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "VeiculosUnidadeController.cs" ,
                    "VeiculosUnidadeController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * 🎯 OBJETIVO: Listar todos os veículos de uma unidade específica (com informações completas)
         * 📥 ENTRADAS: id (UnidadeId Guid)
         * 📤 SAÍDAS: JSON { data: List<{ VeiculoId, Placa, MarcaModelo, Sigla, CombustivelDescricao, ContratoVeiculo, Status, DatadeAlteracao, NomeCompleto, UnidadeId }> }
         * 🔗 CHAMADA POR: Grid de veículos da unidade
         * 🔄 CHAMA: Veiculo.GetAll(), ModeloVeiculo.GetAll(), MarcaVeiculo.GetAll(), etc.
         * 🔀 JOINS: 8 tabelas (Veiculo + Modelo + Marca + Unidade + Combustivel + Contrato + Fornecedor + Usuario)
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            try
            {
                // [DOC] Query complexa com 8 joins para consolidar todos os dados do veículo
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    join u in _unitOfWork.Unidade.GetAll() on v.UnidadeId equals u.UnidadeId
                    join c in _unitOfWork.Combustivel.GetAll()
                        on v.CombustivelId equals c.CombustivelId
                    join ct in _unitOfWork.Contrato.GetAll() on v.ContratoId equals ct.ContratoId
                    join f in _unitOfWork.Fornecedor.GetAll()
                        on ct.FornecedorId equals f.FornecedorId
                    join us in _unitOfWork.AspNetUsers.GetAll() on v.UsuarioIdAlteracao equals us.Id
                    where v.UnidadeId == id
                    select new
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
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs" , "Get" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * 🎯 OBJETIVO: Remover veículo de uma unidade (limpa UnidadeId, não exclui o veículo)
         * 📥 ENTRADAS: model (VeiculoViewModel com VeiculoId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de veículo da unidade
         * 🔄 CHAMA: Veiculo.GetFirstOrDefault(), Veiculo.Update()
         * 🗑️ OPERAÇÃO: Define UnidadeId = Guid.Empty (desvincula sem excluir)
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
                        // [DOC] Apenas desvincula o veículo da unidade, não remove permanentemente
                        objFromDb.UnidadeId = Guid.Empty;
                        _unitOfWork.Veiculo.Update(objFromDb);
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
                Alerta.TratamentoErroComLinha("VeiculosUnidadeController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar veículo"
                });
            }
        }
    }
}
