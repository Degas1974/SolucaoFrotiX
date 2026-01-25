/*
 *  _______________________________________________________
 * |                                                       |
 * |   FrotiX Core - Gestão de Modelos (Core Stack)        |
 * |_______________________________________________________|
 *
 * (IA) Controlador responsável pelo cadastro e manutenção dos modelos
 * de veículos, vinculados às suas respectivas marcas.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ModeloVeiculoController                                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Modelos de Veículos.                            ║
    /// ║    Vincula Marcas (ex: Ford) a Modelos (ex: Ka, Fiesta).                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/ModeloVeiculo                                          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloVeiculoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ModeloVeiculoController (Construtor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public ModeloVeiculoController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs", "ModeloVeiculoController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna todos os modelos cadastrados, incluindo a Marca.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de modelos.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Lista modelos com marca associada.
                return Json(
                    new
                    {
                        data = _unitOfWork.ModeloVeiculo.GetAll(null , null , "MarcaVeiculo")
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "Get");
                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove um modelo se não houver veículos associados.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (ModeloVeiculoViewModel): Dados com ID do modelo.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(ModeloVeiculoViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.ModeloId != Guid.Empty)
                {
                    // [DADOS] Carrega modelo para remoção.
                    var objFromDb = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                        u.ModeloId == model.ModeloId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica se existem veículos associados ao modelo.
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.ModeloId == model.ModeloId
                        );
                        if (veiculo != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo existente.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem veículos associados a esse modelo" ,
                                }
                            );
                        }
                        // [ACAO] Remove e persiste.
                        _unitOfWork.ModeloVeiculo.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Modelo de veículo removido com sucesso" ,
                            }
                        );
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar modelo de veículo"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "Delete");
                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusModeloVeiculo                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Ativa ou inativa o status de um modelo de veículo.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do modelo.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusModeloVeiculo")]
        public JsonResult UpdateStatusModeloVeiculo(Guid Id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega modelo.
                    var objFromDb = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                        u.ModeloId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            // [STATUS] Marca como inativo.
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Modelo [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoModelo
                            );
                            type = 1;
                        }
                        else
                        {
                            // [STATUS] Marca como ativo.
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Modelo  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoModelo
                            );
                            type = 0;
                        }
                        // [ACAO] Atualiza entidade.
                        _unitOfWork.ModeloVeiculo.Update(objFromDb);
                    }
                    // [RETORNO] Status atualizado.
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "ModeloVeiculoController.cs" , "UpdateStatusModeloVeiculo");
                Alerta.TratamentoErroComLinha(
                    "ModeloVeiculoController.cs" ,
                    "UpdateStatusModeloVeiculo" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }
    }
}
