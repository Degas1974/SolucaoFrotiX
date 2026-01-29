/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculosUnidadeController.cs                                                            ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Controller para gerenciamento de veículos vinculados a unidades. Lista veículos       ║
   ║    de uma unidade específica com dados de marca, modelo, contrato e fornecedor.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENDPOINTS: [GET] /api/VeiculosUnidade?id={guid} → Lista veículos da unidade                     ║
   ║    [POST] /api/VeiculosUnidade/Delete → Remove vínculo veículo-unidade                             ║
   ║    DADOS: VeiculoId, Placa, MarcaModelo, Sigla, CombustivelDescricao, ContratoVeiculo, Status      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: IUnitOfWork (Veiculo, Unidade, ViewVeiculos, VeiculoContrato)                              ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosUnidadeController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: Construtor VeiculosUnidadeController
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar controller com injecao de dependencia do UnitOfWork
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork - Padrao Repository para acesso a dados
         * 🔄 CHAMA        : Nenhum metodo adicional
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
         * 🎯 OBJETIVO     : Listar todos veiculos vinculados a uma unidade especifica
         * 📥 ENTRADAS     : [Guid] id - ID da unidade para filtrar veiculos
         * 📤 SAÍDAS       : [IActionResult] JSON { data: [ veiculos... ] }
         * 🔗 CHAMADA POR  : DataTable de veiculos na pagina de detalhes da unidade
         * 🔄 CHAMA        : Veiculo, ModeloVeiculo, MarcaVeiculo, Unidade, Combustivel,
         *                   Contrato, Fornecedor, AspNetUsers (JOINs)
         *
         * 📦 DADOS RETORNADOS:
         *    - VeiculoId, Placa, MarcaModelo (concatenado)
         *    - Sigla (unidade), CombustivelDescricao
         *    - ContratoVeiculo (ano/numero + fornecedor)
         *    - Status, DataAlteracao, NomeCompleto (usuario)
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
         * 🎯 OBJETIVO     : Remover vinculo de veiculo com unidade (nao exclui o veiculo)
         * 📥 ENTRADAS     : [VeiculoViewModel] model - DTO com VeiculoId
         * 📤 SAÍDAS       : [IActionResult] JSON { success, message }
         * 🔗 CHAMADA POR  : Botao excluir no DataTable de veiculos da unidade
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault(), Update(), Save()
         *
         * 📝 COMPORTAMENTO:
         *    - NAO exclui o veiculo do banco
         *    - Apenas seta UnidadeId = Guid.Empty (desvincula da unidade)
         *    - Veiculo fica disponivel para vincular a outra unidade
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
