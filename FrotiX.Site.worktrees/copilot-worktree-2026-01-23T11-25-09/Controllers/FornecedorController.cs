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
    public class FornecedorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FornecedorController (Constructor)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de fornecedores com UoW e log centralizado.      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante rastreabilidade e acesso às operações do módulo.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios.                         ║
        /// ║    • logService (ILogService): log centralizado.                              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public FornecedorController(IUnitOfWork unitOfWork, ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logService = logService;
            }
            catch (Exception error)
            {
                // Construtor: Logar erro e relançar ou tratar via Alerta (se possível)
                // Alerta pode falhar se dependências não estiverem prontas, mas tentamos
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "FornecedorController", error);
                throw;
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna lista completa de fornecedores cadastrados.                       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Alimenta grids e seletores de fornecedores.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de fornecedores.                          ║
        /// ║    • Consumidor: UI de Fornecedores.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Fornecedor.GetAll()                                         ║
        /// ║    • _logService.Error() / Alerta.TratamentoErroComLinha() → erros.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Fornecedor                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Cadastros                                               ║
        /// ║    • Arquivos relacionados: Pages/Fornecedor/*.cshtml                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Lista de fornecedores
                return Json(new
                {
                    data = _unitOfWork.Fornecedor.GetAll()
                });
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "FornecedorController.cs", "Get");
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "Get", error);
                return StatusCode(500, new { success = false, message = "Erro interno ao buscar fornecedores" });
            }
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
        /// ║    • _unitOfWork.Contrato.GetFirstOrDefault()                                 ║
        /// ║    • _unitOfWork.Fornecedor.Remove() / Save()                                 ║
        /// ║    • _logService.Error() / Alerta.TratamentoErroComLinha() → erros.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Fornecedor/Delete                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Cadastros                                               ║
        /// ║    • Arquivos relacionados: Pages/Fornecedor/*.cshtml                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(FornecedorViewModel model)
        {
            try
            {
                // [REGRA] Valida modelo e ID
                if (model != null && model.FornecedorId != Guid.Empty)
                {
                    // [DADOS] Carrega fornecedor
                    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                        u.FornecedorId == model.FornecedorId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Impede exclusão se houver contratos
                        var contrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                            u.FornecedorId == model.FornecedorId
                        );
                        if (contrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false,
                                    message = "Existem contratos associados a esse fornecedor"
                                }
                            );
                        }
                        _unitOfWork.Fornecedor.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true,
                                message = "Fornecedor removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar Fornecedor: Fornecedor não encontrado ou ID inválido"
                });
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "FornecedorController.cs", "Delete");
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "Delete", error);
                return StatusCode(500, new { success = false, message = "Erro interno ao excluir fornecedor" });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusFornecedor                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status (Ativo/Inativo) de um fornecedor.                        ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite ativar/desativar fornecedores sem excluir registros.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): identificador do fornecedor.                                 ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status e mensagem da operação.                               ║
        /// ║    • Consumidor: UI de Fornecedores.                                         ║
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
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [LOGICA] Alterna status
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)",
                                objFromDb.DescricaoFornecedor
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Fornecedor  [Nome: {0}] (Ativo)",
                                objFromDb.DescricaoFornecedor
                            );
                            type = 0;
                        }
                        _unitOfWork.Fornecedor.Update(objFromDb);
                        _unitOfWork.Save(); // Adicionado Save(), parece que estava faltando no original se não estivesse implícito no Update (EF Core não salva no Update)
                    }
                    return Json(
                        new
                        {
                            success = true,
                            message = Description,
                            type = type,
                        }
                    );
                }
                return Json(new
                {
                    success = false,
                    message = "ID inválido"
                });
            }
            catch (Exception error)
            {
                _logService.Error(error.Message, error, "FornecedorController.cs", "UpdateStatusFornecedor");
                Alerta.TratamentoErroComLinha("FornecedorController.cs", "UpdateStatusFornecedor", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao atualizar status"
                });
            }
        }
    }
}
