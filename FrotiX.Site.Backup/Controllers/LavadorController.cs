/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API)                                                                          |
 * | (IA) IDENTIDADE: LavadorController.cs                                                                   |
 * | (IA) DESCRIÇÃO: API para gerenciamento de Lavadores (Funcionários).                                     |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: LavadorController                                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Lavadores (funcionários).                        ║
    /// ║    Controla cadastro, consulta e vínculos com contratos/fornecedores.         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Lavador                                                ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class LavadorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LavadorController (Construtor)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public LavadorController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs", "LavadorController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna lista de lavadores enriquecida com dados de contrato/fornecedor.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista projetada.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta lavadores com joins para contrato/fornecedor/usuário.
                var result = (
                    from l in _unitOfWork.Lavador.GetAll()

                    join ct in _unitOfWork.Contrato.GetAll()
                        on l.ContratoId equals ct.ContratoId
                        into ctr
                    from ctrResult in ctr.DefaultIfEmpty() // <= Left Join

                    join f in _unitOfWork.Fornecedor.GetAll()
                        on ctrResult == null
                            ? Guid.Empty
                            : ctrResult.FornecedorId equals f.FornecedorId
                        into frd
                    from frdResult in frd.DefaultIfEmpty() // <= Left Join

                    join us in _unitOfWork.AspNetUsers.GetAll()
                        on l.UsuarioIdAlteracao equals us.Id

                    select new
                    {
                        l.LavadorId ,
                        l.Nome ,
                        l.Ponto ,
                        l.Celular01 ,

                        ContratoLavador = ctrResult != null
                            ? (
                                ctrResult.AnoContrato
                                + "/"
                                + ctrResult.NumeroContrato
                                + " - "
                                + frdResult.DescricaoFornecedor
                            )
                            : "<b>(Sem Contrato)</b>" ,

                        l.Status ,
                        l.Foto ,

                        DatadeAlteracao = l.DataAlteracao.HasValue ? l.DataAlteracao.Value.ToString("dd/MM/yy") : string.Empty ,

                        us.NomeCompleto ,
                    }
                ).ToList();

                // [RETORNO] Lista projetada para grid.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "Get");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Deleta base de lavador se não houver vínculos ativos.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model: ViewModel com ID.                                                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(LavadorViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.LavadorId != Guid.Empty)
                {
                    // [DADOS] Carrega entidade para remoção.
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u =>
                        u.LavadorId == model.LavadorId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculo de contrato antes de excluir.
                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefault(u =>
                            u.LavadorId == model.LavadorId
                        );
                        if (lavadorContrato != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo ativo.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o lavador. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        // [ACAO] Remove entidade e persiste.
                        _unitOfWork.Lavador.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Lavador removido com sucesso"
                            }
                        );
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar lavador"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "Delete");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusLavador                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna status Ativo/Inativo.                                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusLavador")]
        public JsonResult UpdateStatusLavador(Guid Id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega lavador com tracking.
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u => u.LavadorId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            // [STATUS] Marca como inativo.
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Lavador [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            // [STATUS] Marca como ativo.
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Lavador  [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        // [ACAO] Atualiza entidade.
                        _unitOfWork.Lavador.Update(objFromDb);
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
                _log.Error(error.Message, error, "LavadorController.cs", "UpdateStatusLavador");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "UpdateStatusLavador" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFoto                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna objeto lavador com a foto convertida (Base64).                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFoto")]
        public JsonResult PegaFoto(Guid id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (id != Guid.Empty)
                {
                    // [DADOS] Busca lavador por ID.
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                        u.LavadorId == id
                    );
                    if (objFromDb.Foto != null)
                    {
                        // [CONVERSAO] Converte foto para Base64.
                        objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                        return Json(objFromDb);
                    }
                    // [RETORNO] Foto inexistente.
                    return Json(false);
                }
                else
                {
                    // [RETORNO] ID inválido.
                    return Json(false);
                }
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "PegaFoto");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "PegaFoto" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFotoModal                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna apenas o binário da foto para exibição em modal.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFotoModal")]
        public JsonResult PegaFotoModal(Guid id)
        {
            try
            {
                // [DADOS] Busca lavador por ID.
                var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == id);
                if (objFromDb.Foto != null)
                {
                    // [CONVERSAO] Converte foto para Base64.
                    objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                    return Json(objFromDb.Foto);
                }
                // [RETORNO] Foto inexistente.
                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "PegaFotoModal");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "PegaFotoModal" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetImage                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Helper: Converte String Base64 para Byte Array.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public byte[] GetImage(string sBase64String)
        {
            try
            {
                byte[] bytes = null;
                if (!string.IsNullOrEmpty(sBase64String))
                {
                    // [CONVERSAO] Decodifica Base64 para bytes.
                    bytes = Convert.FromBase64String(sBase64String);
                }
                // [RETORNO] Bytes decodificados.
                return bytes;
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "GetImage");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "GetImage" , error);
                return default(byte[]); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LavadorContratos                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna contratos vinculados a um lavador específico.                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("LavadorContratos")]
        public IActionResult LavadorContratos(Guid Id)
        {
            try
            {
                // [DADOS] Lista lavadores vinculados ao contrato.
                var result = (
                    from m in _unitOfWork.Lavador.GetAll()

                    join lc in _unitOfWork.LavadorContrato.GetAll()
                        on m.LavadorId equals lc.LavadorId

                    where lc.ContratoId == Id

                    select new
                    {
                        m.LavadorId ,
                        m.Nome ,
                        m.Ponto ,
                        m.Celular01 ,
                        m.Status ,
                    }
                ).ToList();

                // [RETORNO] Lista projetada para grid.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "LavadorContratos");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "LavadorContratos" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteContrato                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove a associação entre lavador e contrato.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(LavadorViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.LavadorId != Guid.Empty)
                {
                    // [DADOS] Carrega lavador com tracking.
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefaultWithTracking(u =>
                        u.LavadorId == model.LavadorId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculo específico antes de excluir.
                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefaultWithTracking(u =>
                            u.LavadorId == model.LavadorId && u.ContratoId == model.ContratoId
                        );
                        if (lavadorContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                // [ACAO] Remove referência de contrato no lavador.
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Lavador.Update(objFromDb);
                            }
                            // [ACAO] Remove vínculo e persiste.
                            _unitOfWork.LavadorContrato.Remove(lavadorContrato);
                            _unitOfWork.Save();
                            return Json(
                                new
                                {
                                    success = true ,
                                    message = "Lavador removido com sucesso"
                                }
                            );
                        }
                        // [RETORNO] Vínculo não encontrado.
                        return Json(new
                        {
                            success = false ,
                            message = "Erro ao remover lavador"
                        });
                    }
                    // [RETORNO] Lavador não encontrado.
                    return Json(new
                    {
                        success = false ,
                        message = "Erro ao remover lavador"
                    });
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover lavador"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LavadorController.cs", "DeleteContrato");
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "DeleteContrato" , error);
                return View(); // padronizado
            }
        }
    }
}
