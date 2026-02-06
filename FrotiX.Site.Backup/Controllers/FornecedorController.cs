using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: FornecedorController                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Fornecedores.                                   ║
    /// ║    CRUD básico para cadastro de empresas parceiras.                          ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Permite o cadastro e manutenção da base de fornecedores que prestam       ║
    /// ║    serviços à frota (manutenção, peças, etc.).                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Fornecedor                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: FornecedorController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do Unit of Work
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork
         * 📤 SAÍDAS       : Instância configurada
         * 🔗 CHAMADA POR  : ASP.NET Core DI
         ****************************************************************************************/
        public FornecedorController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "FornecedorController.cs" ,
                    "FornecedorController" ,
                    error
                );
            }
        }

        /// <summary>
        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos os fornecedores cadastrados no sistema
         *                   Retorna dados completos para popular grids e dropdowns
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON { data: List<Fornecedor> }
         * ⬅️ CHAMADO POR  : JavaScript (DataTables) de Fornecedores/Index, Contratos
         * ➡️ CHAMA        : Fornecedor.GetAll()
         * 📝 OBSERVAÇÕES  : Retorna fornecedores ativos e inativos
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Json(new
                {
                    data = _unitOfWork.Fornecedor.GetAll()
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Get" , error);
                return StatusCode(500
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove um fornecedor após validar vínculos com contratos.                 ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Evita exclusões que quebrem integridade referencial.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (FornecedorViewModel): contém o ID do fornecedor.                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                              ║
        /// ║    • Consumidor: UI de Fornecedores.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Fornecedor.GetFirstOrDefault()                               ║
        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Excluir fornecedor do banco validando integridade referencial
         * 📥 ENTRADAS     : [FornecedorViewModel] model - contém FornecedorId
         * 📤 SAÍDAS       : [IActionResult] JSON success/message
         * 🔄 CHAMA        : Fornecedor.GetFirstOrDefault(), Contrato, Remove(), Save()
         * ⚠️  VALIDAÇÃO   : Bloqueia exclusão se houver contratos associados ao fornecedor
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(FornecedorViewModel model)
        {
            try
            {
                if (model != null && model.FornecedorId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                        u.FornecedorId == model.FornecedorId
                    );
                    if (objFromDb != null)
                    {
                        // [DOC] Verifica integridade referencial - bloqueia se houver contratos vinculados
                        var contrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                            u.FornecedorId == model.FornecedorId
                        );
                        if (contrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem contratos associados a esse fornecedor" ,
                                }
                            );
                        }
                        _unitOfWork.Fornecedor.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Fornecedor removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Fornecedor"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Delete" , error);
                return StatusCode(500
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Fornecedor.GetFirstOrDefault()                               ║
        /// ║    • _unitOfWork.Fornecedor.Update() / Save()                                 ║
        /// ║    • _logService.Error() / Alerta.TratamentoErroComLinha() → erros.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Fornecedor/UpdateStatusFornecedor                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Cadastros                                               ║
        /// ║    • Arquivos relacionados: Pages/Fornecedor/*.cshtml                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [Route("UpdateStatusFornecedor")]
        [HttpPost] // Adicionado HttpPost para segurança, embora a rota seja customizada
        public JsonResult UpdateStatusFornecedor(Guid Id)
        {
            try
            {
                // [REGRA] Valida ID
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega fornecedor
                    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                        u.FornecedorId == Id
                    );
                    string Description = "";
         ****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusFornecedor
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status do fornecedor entre ativo e inativo
         *                   Retorna mensagem descritiva com o novo status
         * 📥 ENTRADAS     : [Guid] Id - FornecedorId
         * 📤 SAÍDAS       : [JsonResult] { success: bool, message: string, type: int }
         * ⬅️ CHAMADO POR  : JavaScript (AJAX) ao clicar botão de ativar/desativar
         * ➡️ CHAMA        : Fornecedor.GetFirstOrDefault(), Update(), Save()
         * 📝 OBSERVAÇÕES  : type=0 (ativo), type=1 (inativo) para feedback visual
         ****************************************************************************************/
        [Route("UpdateStatusFornecedor")]
        public JsonResult UpdateStatusFornecedor(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                        u.FornecedorId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoFornecedor
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Fornecedor  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoFornecedor
                            );
                            type = 0;
                        }
                        _unitOfWork.Fornecedor.Update(objFromDb);
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
                Alerta.TratamentoErroComLinha(
                    "FornecedorController.cs" ,
                    "UpdateStatusFornecedor" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false