/*
 *  _______________________________________________________
 * |                                                       |
 * |   FrotiX Core - Gestão de Marcas (Core Stack)         |
 * |_______________________________________________________|
 *
 * (IA) Controlador responsável pelo cadastro e manutenção das marcas
 * de veículos da frota. Base para a hierarquia Marca -> Modelo.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MarcaVeiculoController                                             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Marcas de Veículos (Cadastros Básicos).         ║
    /// ║    Mantém lista de marcas usadas nos modelos.                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/MarcaVeiculo                                           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaVeiculoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MarcaVeiculoController (Construtor)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public MarcaVeiculoController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs", "MarcaVeiculoController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna todas as marcas cadastradas no sistema.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de marcas.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Lista marcas de veículo.
                return Json(new
                {
                    data = _unitOfWork.MarcaVeiculo.GetAll()
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "Get");
                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove uma marca se não houver modelos vinculados.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (MarcaVeiculoViewModel): Dados com ID da marca.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(MarcaVeiculoViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.MarcaId != Guid.Empty)
                {
                    // [DADOS] Carrega marca com tracking.
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u =>
                        u.MarcaId == model.MarcaId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculo de modelos antes de excluir.
                        var modelo = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                            u.MarcaId == model.MarcaId
                        );
                        if (modelo != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo existente.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem modelos associados a essa marca" ,
                                }
                            );
                        }
                        // [ACAO] Remove e persiste.
                        _unitOfWork.MarcaVeiculo.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Marca de veículo removida com sucesso" ,
                            }
                        );
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar marca de veículo"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "Delete");
                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusMarcaVeiculo                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Ativa ou desativa o status de uma marca de veículo.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID da marca.                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusMarcaVeiculo")]
        public JsonResult UpdateStatusMarcaVeiculo(Guid Id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega marca com tracking.
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefaultWithTracking(u =>
                        u.MarcaId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            // [STATUS] Marca como inativa.
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status da Marca [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoMarca
                            );
                            type = 1;
                        }
                        else
                        {
                            // [STATUS] Marca como ativa.
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status da Marca  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoMarca
                            );
                            type = 0;
                        }
                        // [ACAO] Atualiza entidade.
                        _unitOfWork.MarcaVeiculo.Update(objFromDb);
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
                _log.Error(error.Message , error , "MarcaVeiculoController.cs" , "UpdateStatusMarcaVeiculo");
                Alerta.TratamentoErroComLinha(
                    "MarcaVeiculoController.cs" ,
                    "UpdateStatusMarcaVeiculo" ,
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
