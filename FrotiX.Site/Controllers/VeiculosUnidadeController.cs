/* ****************************************************************************************
 * ⚡ ARQUIVO: VeiculosUnidadeController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Listar veículos vinculados a uma unidade e permitir desvinculação.
 *
 * 📥 ENTRADAS     : ID da unidade e modelos de veículo.
 *
 * 📤 SAÍDAS       : JSON com lista de veículos e mensagens.
 *
 * 🔗 CHAMADA POR  : Tela de detalhes da unidade.
 *
 * 🔄 CHAMA        : IUnitOfWork (Veiculo, Unidade, ModeloVeiculo, MarcaVeiculo, Contrato,
 *                   Fornecedor, AspNetUsers, Combustivel).
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: VeiculosUnidadeController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints de consulta e desvinculação de veículos por unidade.
     *
     * 📥 ENTRADAS     : IDs e modelos de veículo.
     *
     * 📤 SAÍDAS       : JSON com dados e mensagens.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosUnidadeController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: VeiculosUnidadeController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
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
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos vinculados a uma unidade específica.
         *
         * 📥 ENTRADAS     : id (Guid da unidade).
         *
         * 📤 SAÍDAS       : JSON com data (lista de veículos).
         *
         * 🔗 CHAMADA POR  : DataTable de veículos da unidade.
         *
         * 🔄 CHAMA        : Veiculo, ModeloVeiculo, MarcaVeiculo, Unidade, Combustivel,
         *                   Contrato, Fornecedor, AspNetUsers (JOINs).
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            try
            {
                // [DOC] Consulta com multiplos JOINs para montar dados completos do veiculo
                // [DOC] Relaciona: Veiculo -> Marca/Modelo, Unidade, Combustivel, Contrato/Fornecedor, Usuario
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
                    where v.UnidadeId == id  // [DOC] Filtro pela unidade solicitada
                    select new
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        // [DOC] Concatena marca/modelo para exibicao unica
                        MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                        u.Sigla ,
                        CombustivelDescricao = c.Descricao ,
                        // [DOC] Formato contrato: "2024/1234 - Nome Fornecedor"
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
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Desvincular veículo da unidade (sem excluir o veículo).
         *
         * 📥 ENTRADAS     : model (VeiculoViewModel).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Botão excluir no DataTable de veículos da unidade.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault(), Veiculo.Update(), UnitOfWork.Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(VeiculoViewModel model)
        {
            try
            {
                // [DOC] Valida se recebeu ID valido do veiculo
                if (model != null && model.VeiculoId != Guid.Empty)
                {
                    // [DOC] Busca veiculo no banco pelo ID
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == model.VeiculoId
                    );
                    if (objFromDb != null)
                    {
                        // [DOC] DESVINCULA: seta UnidadeId vazio (nao exclui veiculo)
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
