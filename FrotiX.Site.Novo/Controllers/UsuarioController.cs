/* ****************************************************************************************
 * ⚡ ARQUIVO: UsuarioController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar usuários (AspNetUsers) e permissões de acesso por recurso.
 *
 * 📥 ENTRADAS     : IDs de usuário, recursos e operações de toggle.
 *
 * 📤 SAÍDAS       : JSON com dados, status e mensagens de retorno.
 *
 * 🔗 CHAMADA POR  : Telas administrativas de usuários e controle de acesso.
 *
 * 🔄 CHAMA        : IUnitOfWork.AspNetUsers, ControleAcesso, Recurso, Viagem, Manutencao,
 *                   SetorPatrimonial, MovimentacaoPatrimonio.
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
     * ⚡ CONTROLLER: UsuarioController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para listar, excluir e gerenciar acessos de usuários.
     *
     * 📥 ENTRADAS     : IDs e parâmetros de controle de acesso.
     *
     * 📤 SAÍDAS       : JSON com dados e mensagens.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public partial class UsuarioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: UsuarioController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public UsuarioController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UsuarioController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar usuários com flag PodeExcluir baseada em vínculos.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com data (lista de usuários).
         *
         * 🔗 CHAMADA POR  : Grid de usuários.
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var usuarios = _unitOfWork.AspNetUsers.GetAll().ToList();
                var result = new List<object>();

                foreach (var u in usuarios)
                {
                    // Verificar se o usuário pode ser excluído (mesma lógica do Delete)
                    bool podeExcluir = true;

                    // 1. ControleAcesso
                    var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                        ca.UsuarioId == u.Id
                    );
                    if (temControleAcesso != null) podeExcluir = false;

                    // 2. Viagens
                    if (podeExcluir)
                    {
                        var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                            v.UsuarioIdCriacao == u.Id || v.UsuarioIdFinalizacao == u.Id
                        );
                        if (temViagens != null) podeExcluir = false;
                    }

                    // 3. Manutenções
                    if (podeExcluir)
                    {
                        var temManutencoes = _unitOfWork.Manutencao.GetFirstOrDefault(m =>
                            m.IdUsuarioAlteracao == u.Id ||
                            m.IdUsuarioCriacao == u.Id ||
                            m.IdUsuarioFinalizacao == u.Id ||
                            m.IdUsuarioCancelamento == u.Id
                        );
                        if (temManutencoes != null) podeExcluir = false;
                    }

                    // 4. MovimentacaoPatrimonio
                    if (podeExcluir)
                    {
                        var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                            mp.ResponsavelMovimentacao == u.Id
                        );
                        if (temMovimentacao != null) podeExcluir = false;
                    }

                    // 5. SetorPatrimonial
                    if (podeExcluir)
                    {
                        var temSetor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(sp =>
                            sp.DetentorId == u.Id
                        );
                        if (temSetor != null) podeExcluir = false;
                    }

                    result.Add(new
                    {
                        UsuarioId = u.Id ,
                        u.NomeCompleto ,
                        u.Ponto ,
                        u.DetentorCargaPatrimonial ,
                        u.Status ,
                        PodeExcluir = podeExcluir
                    });
                }

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "Get" , error);
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
         * 🎯 OBJETIVO     : Excluir usuário quando não houver vínculos impeditivos.
         *
         * 📥 ENTRADAS     : users (AspNetUsers com Id).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de exclusão no grid.
         *
         * 🔄 CHAMA        : AspNetUsers.GetFirstOrDefault(), ControleAcesso/Viagem/Manutencao/
         *                   MovimentacaoPatrimonio/SetorPatrimonial, AspNetUsers.Remove(), Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(AspNetUsers users)
        {
            try
            {
                var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == users.Id);
                if (objFromDb == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Usuário não encontrado"
                    });
                }

                // Verificar vínculos com outras tabelas
                var vinculos = new List<string>();

                // 1. ControleAcesso
                var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                    ca.UsuarioId == users.Id
                );
                if (temControleAcesso != null)
                {
                    vinculos.Add("Controle de Acesso a Recursos");
                }

                // 2. Viagens (Criação e Finalização)
                var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.UsuarioIdCriacao == users.Id || v.UsuarioIdFinalizacao == users.Id
                );
                if (temViagens != null)
                {
                    vinculos.Add("Viagens (como responsável pelo cadastro ou finalização)");
                }

                // 3. Manutenções
                var temManutencoes = _unitOfWork.Manutencao.GetFirstOrDefault(m =>
                    m.IdUsuarioAlteracao == users.Id ||
                    m.IdUsuarioCriacao == users.Id ||
                    m.IdUsuarioFinalizacao == users.Id ||
                    m.IdUsuarioCancelamento == users.Id
                );
                if (temManutencoes != null)
                {
                    vinculos.Add("Manutenções (como responsável pelo cadastro, alteração, finalização ou cancelamento)");
                }

                // 4. MovimentacaoPatrimonio
                var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                    mp.ResponsavelMovimentacao == users.Id
                );
                if (temMovimentacao != null)
                {
                    vinculos.Add("Movimentações de Patrimônio (como responsável)");
                }

                // 5. SetorPatrimonial
                var temSetor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(sp =>
                    sp.DetentorId == users.Id
                );
                if (temSetor != null)
                {
                    vinculos.Add("Setores Patrimoniais (como detentor)");
                }

                // Se houver vínculos, impedir exclusão
                if (vinculos.Any())
                {
                    var mensagemVinculos = string.Join(", ", vinculos);
                    return Json(
                        new
                        {
                            success = false ,
                            message = $"❌ Não é possível excluir o usuário <strong>{objFromDb.NomeCompleto}</strong>.<br><br>" +
                                     $"<strong>Motivo:</strong> Existem registros vinculados a este usuário nas seguintes áreas:<br><br>" +
                                     $"<ul style='text-align: left; margin: 0.5rem 0;'>" +
                                     string.Join("", vinculos.Select(v => $"<li>{v}</li>")) +
                                     $"</ul><br>" +
                                     $"<small style='color: #6c757d;'>Para excluir este usuário, primeiro remova ou transfira os registros vinculados.</small>"
                        }
                    );
                }

                // Se não houver vínculos, pode excluir
                _unitOfWork.AspNetUsers.Remove(objFromDb);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true ,
                    message = $"✅ Usuário <strong>{objFromDb.NomeCompleto}</strong> removido com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "❌ Erro ao deletar usuário: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusUsuario
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status ativo/inativo do usuário.
         *
         * 📥 ENTRADAS     : Id (string do usuário).
         *
         * 📤 SAÍDAS       : JSON com success, message e type.
         *
         * 🔗 CHAMADA POR  : Toggle de status na tela de usuários.
         ****************************************************************************************/
        [Route("UpdateStatusUsuario")]
        public JsonResult UpdateStatusUsuario(String Id)
        {
            try
            {
                if (Id != "")
                {
                    var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Usuário [Nome: {0}] (Inativo)" ,
                                objFromDb.NomeCompleto
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Usuário  [Nome: {0}] (Ativo)" ,
                                objFromDb.NomeCompleto
                            );
                            type = 0;
                        }
                        _unitOfWork.AspNetUsers.Update(objFromDb);
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
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusUsuario" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateCargaPatrimonial
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar flag de detentor de carga patrimonial do usuário.
         *
         * 📥 ENTRADAS     : Id (string do usuário).
         *
         * 📤 SAÍDAS       : JSON com success, message e type.
         *
         * 🔗 CHAMADA POR  : Toggle de detentor patrimonial na tela de usuários.
         ****************************************************************************************/
        [Route("UpdateCargaPatrimonial")]
        public JsonResult UpdateCargaPatrimonial(String Id)
        {
            try
            {
                if (Id != "")
                {
                    var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.DetentorCargaPatrimonial == true)
                        {
                            objFromDb.DetentorCargaPatrimonial = false;
                            Description = string.Format(
                                "Atualizado Carga Patrimonial do Usuário [Nome: {0}] (Não)" ,
                                objFromDb.NomeCompleto
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.DetentorCargaPatrimonial = true;
                            Description = string.Format(
                                "Atualizado Carga Patrimonial do Usuário  [Nome: {0}] (Ativo)" ,
                                objFromDb.NomeCompleto
                            );
                            type = 0;
                        }
                        _unitOfWork.AspNetUsers.Update(objFromDb);
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
                    "UsuarioController.cs" ,
                    "UpdateCargaPatrimonial" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusAcesso
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar acesso do usuário a um recurso específico.
         *
         * 📥 ENTRADAS     : IDS (string com IDs de usuário/recurso).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Gestão de permissões por recurso.
         ****************************************************************************************/
        [Route("UpdateStatusAcesso")]
        public JsonResult UpdateStatusAcesso(String IDS)
        {
            try
            {
                string inputString = IDS;
                char separator = '|';

                string[] parts = inputString.Split(separator);

                string usuarioId = parts[0];
                string recursoId = parts[1];

                var objFromDb = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                    ca.UsuarioId == usuarioId && ca.RecursoId == Guid.Parse(recursoId)
                );
                string Description = "";
                int type = 0;

                if (objFromDb != null)
                {
                    if (objFromDb.Acesso == true)
                    {
                        objFromDb.Acesso = false;
                        Description = string.Format(
                            "Atualizado Acesso do Usuário ao Recurso (Sem Acesso)"
                        );
                        type = 1;
                    }
                    else
                    {
                        objFromDb.Acesso = true;
                        Description = string.Format(
                            "Atualizado Acesso do Usuário ao Recurso (Com Acesso)"
                        );
                        type = 0;
                    }
                    _unitOfWork.Save();
                    _unitOfWork.ControleAcesso.Update(objFromDb);
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusAcesso" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaRecursosUsuario
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar recursos associados a um usuário.
         *
         * 📥 ENTRADAS     : UsuarioId (string).
         *
         * 📤 SAÍDAS       : JSON com recursos do usuário.
         *
         * 🔗 CHAMADA POR  : Tela de permissões.
         ****************************************************************************************/
        [Route("PegaRecursosUsuario")]
        [HttpGet]
        public IActionResult PegaRecursosUsuario(String UsuarioId)
        {
            try
            {
                var objRecursos = _unitOfWork.ViewControleAcesso.GetAll(vca =>
                    vca.UsuarioId == UsuarioId
                );

                return Json(new
                {
                    data = objRecursos
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "PegaRecursosUsuario" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar recursos"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaUsuariosRecurso
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar usuários vinculados a um recurso.
         *
         * 📥 ENTRADAS     : RecursoId (string).
         *
         * 📤 SAÍDAS       : JSON com usuários e flags de vínculo.
         *
         * 🔗 CHAMADA POR  : Tela de permissões por recurso.
         ****************************************************************************************/
        [Route("PegaUsuariosRecurso")]
        [HttpGet]
        public IActionResult PegaUsuariosRecurso(String RecursoId)
        {
            try
            {
                var objRecursos = _unitOfWork
                    .ViewControleAcesso.GetAll(vca => vca.RecursoId == Guid.Parse(RecursoId))
                    .OrderBy(vca => vca.NomeCompleto);

                return Json(new
                {
                    data = objRecursos
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "PegaUsuariosRecurso" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar usuários"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: InsereRecursosUsuario
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar recursos para usuário (permite cadastro em lote).
         *
         * 📥 ENTRADAS     : Request.Form (UsuarioId, ListaRecursos).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de salvar permissões.
         ****************************************************************************************/
        [Route("InsereRecursosUsuario")]
        [HttpPost]
        public IActionResult InsereRecursosUsuario()
        {
            try
            {
                var objUsuarios = (
                    from u in _unitOfWork.AspNetUsers.GetAll()
                    select new
                    {
                        UsuarioId = u.Id ,
                        u.NomeCompleto ,
                        u.Ponto ,
                        u.Ramal ,
                        u.Status ,
                    }
                ).ToList();

                var objRecursos = _unitOfWork.Recurso.GetAll();

                foreach (var usuario in objUsuarios)
                {
                    foreach (var recurso in objRecursos)
                    {
                        var objAcesso = new ControleAcesso();

                        objAcesso.UsuarioId = usuario.UsuarioId;
                        objAcesso.RecursoId = recurso.RecursoId;
                        objAcesso.Acesso = true;

                        _unitOfWork.ControleAcesso.Add(objAcesso);
                        _unitOfWork.Save();
                    }
                }

                return Json(new
                {
                    data = true
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UsuarioController.cs" ,
                    "InsereRecursosUsuario" ,
                    error
                );
                return Json(new
                {
                    success = false ,
                    message = "Erro ao inserir recursos"
                });
            }
        }

        [HttpGet]
        /****************************************************************************************
         * ⚡ FUNÇÃO: listaUsuariosDetentores
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar usuários detentores de carga patrimonial ativos.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com lista de usuários detentores.
         *
         * 🔗 CHAMADA POR  : Combos/seleções de detentor.
         ****************************************************************************************/
        [Route("listaUsuariosDetentores")]
        public IActionResult listaUsuariosDetentores()
        {
            try
            {
                var result = (
                    from u in _unitOfWork.AspNetUsers.GetAll(u =>
                        u.DetentorCargaPatrimonial == true && u.Status == true
                    )
                    select new
                    {
                        UsuarioId = u.Id ,
                        u.NomeCompleto
                    }
                ).ToList();

                return Json(new
                {
                    success = true ,
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UsuarioController.cs" ,
                    "listaUsuariosDetentores" ,
                    error
                );
                return Json(new
                {
                    success = false ,
                    message = "Erro ao listar usuários"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteRecurso
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo de recurso com usuário.
         *
         * 📥 ENTRADAS     : RecursoId (string).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Remoção de permissões.
         ****************************************************************************************/
        [Route("DeleteRecurso")]
        [HttpPost]
        public IActionResult DeleteRecurso([FromBody] string RecursoId)
        {
            try
            {
                var objRecursos = _unitOfWork.Recurso.GetFirstOrDefault(r =>
                    r.RecursoId == Guid.Parse(RecursoId)
                );
                if (objRecursos != null)
                {
                    var objControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                        ca.RecursoId == objRecursos.RecursoId
                    );
                    if (objControleAcesso != null)
                    {
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Não foi possível remover o Recursos. Ele está associado a um ou mais Usuários!" ,
                            }
                        );
                    }

                    _unitOfWork.Recurso.Remove(objRecursos);
                    _unitOfWork.Save();
                    return Json(new
                    {
                        success = true ,
                        message = "Recurso removido com sucesso"
                    });
                }

                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Usuário"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "DeleteRecurso" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar recurso"
                });
            }
        }
    }
}
