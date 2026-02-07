using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using FrotiX.Helpers;
using FrotiX.Services;

/*
 *  ╔═══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 *  ║                                      FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                ║
 *  ╠═══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 *  ║ (IA) CAMADA: CONTROLLERS (API)                                                                        ║
 *  ║ (IA) IDENTIDADE: CombustivelController.cs                                                             ║
 *  ║ (IA) DESCRIÇÃO: Gerenciamento de Tipos de Combustível (Gasolina, Diesel, Flex, etc).                  ║
 *  ║ (IA) PADRÃO: FrotiX 2026 Core                                                                         ║
 *  ╚═══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombustivelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CombustivelController (Constructor)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de combustíveis com UnitOfWork e serviço de log.║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita gestão de tipos de combustível com rastreabilidade.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios.                         ║
        /// ║    • log (ILogService): log centralizado.                                    ║
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
        public CombustivelController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "CombustivelController", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a lista completa de tipos de combustíveis cadastrados.            ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Fornece dados para grids e seletores do cadastro de veículos.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de combustíveis.                           ║
        /// ║    • Consumidor: UI de Combustíveis.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Combustivel.GetAll()                                        ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Combustivel                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Cadastros                                               ║
        /// ║    • Arquivos relacionados: Pages/Combustivel/*.cshtml                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Retorna lista de combustíveis
                return Json(new
                {
                    data = _unitOfWork.Combustivel.GetAll()
                });
            }
            catch (Exception ex)
            {
                _log.Error("[CombustivelController] Erro em Get: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "Get", ex);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove um tipo de combustível, validando vínculos com veículos.           ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Evita exclusão com integridade referencial violada.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (CombustivelViewModel): contém o ID do combustível.                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado da operação.                           ║
        /// ║    • Consumidor: UI de Combustíveis.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Combustivel.GetFirstOrDefault()                  ║
        /// ║    • _unitOfWork.Veiculo.GetFirstOrDefault()                                  ║
        /// ║    • _unitOfWork.Combustivel.Remove()                                         ║
        /// ║    • _unitOfWork.Save()                                                      ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Combustivel/Delete                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Cadastros                                               ║
        /// ║    • Arquivos relacionados: Pages/Combustivel/*.cshtml                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(CombustivelViewModel model)
        {
            try
            {
                // [REGRA] Valida modelo e ID
                if (model != null && model.CombustivelId != Guid.Empty)
                {
                    // [DADOS] Carrega combustível
                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u =>
                        u.CombustivelId == model.CombustivelId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Impede exclusão se houver veículo vinculado
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.CombustivelId == model.CombustivelId
                        );
                        if (veiculo != null)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Existem veículos associados a esse combustível",
                            });
                        }
                        _unitOfWork.Combustivel.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true,
                                message = "Tipo de Combustível removido com sucesso",
                            }
                        );
                    }
                }
                return Json(
                    new
                    {
                        success = false,
                        message = "Erro ao apagar Tipo de Combustível"
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("[CombustivelController] Erro em Delete: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "Delete", ex);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusCombustivel                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status (Ativo/Inativo) de um combustível.                        ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite ativar/desativar tipos sem excluir registros.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): identificador do combustível.                                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status e mensagem de alteração.                              ║
        /// ║    • Consumidor: UI de Combustíveis.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Combustivel.GetFirstOrDefault()                  ║
        /// ║    • _unitOfWork.Combustivel.Update()                                         ║
        /// ║    • _unitOfWork.Save()                                                      ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Combustivel/UpdateStatusCombustivel                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Cadastros                                               ║
        /// ║    • Arquivos relacionados: Pages/Combustivel/*.cshtml                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusCombustivel")]
        [HttpPost]
        public JsonResult UpdateStatusCombustivel(Guid Id)
        {
            try
            {
                // [REGRA] Valida ID
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega combustível
                    var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u =>
                        u.CombustivelId == Id
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
                                "Atualizado Status do Tipo de Combustível [Nome: {0}] (Inativo)",
                                objFromDb.Descricao
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Tipo de Combustível  [Nome: {0}] (Ativo)",
                                objFromDb.Descricao
                            );
                            type = 0;
                        }
                        _unitOfWork.Combustivel.Update(objFromDb);
                        _unitOfWork.Save(); 
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
                    success = false
                });
            }
            catch (Exception ex)
            {
                _log.Error("[CombustivelController] Erro em UpdateStatusCombustivel: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("CombustivelController.cs", "UpdateStatusCombustivel", ex);
                return new JsonResult(new { sucesso = false });
            }
        }
    }
}

