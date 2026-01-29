/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UsuarioController.cs (Controllers)                                                              ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ API Controller para operações de Usuários (AspNetUsers) e Controle de Acesso. Gerencia CRUD de usuários,║
 * ║ toggle de status, gestão de permissões por recurso e carga patrimonial.                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ROTA BASE: api/Usuario                                                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ENDPOINTS                                                                                                 ║
 * ║ • [GET]  /                       : Lista usuários com flag PodeExcluir                                  ║
 * ║ • [POST] /Delete                 : Remove usuário (verifica vínculos extensivos)                        ║
 * ║ • [GET]  /UpdateStatusUsuario    : Toggle status Ativo/Inativo                                          ║
 * ║ • [GET]  /UpdateCargaPatrimonial : Toggle DetentorCargaPatrimonial                                      ║
 * ║ • [GET]  /UpdateStatusAcesso     : Toggle acesso a recurso específico (IDS = "usuarioId|recursoId")     ║
 * ║ • [GET]  /PegaRecursosUsuario    : Lista recursos de um usuário                                         ║
 * ║ • [GET]  /PegaUsuariosRecurso    : Lista usuários de um recurso                                         ║
 * ║ • [POST] /InsereRecursosUsuario  : Cria ControleAcesso para todos usuários x recursos (inicial)         ║
 * ║ • [GET]  /listaUsuariosDetentores: Lista usuários ativos com DetentorCargaPatrimonial = true            ║
 * ║ • [POST] /DeleteRecurso          : Remove recurso (verifica vínculos em ControleAcesso)                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÕES DE EXCLUSÃO USUÁRIO                                                                            ║
 * ║ Não pode excluir se vinculado a:                                                                        ║
 * ║ • ControleAcesso (como UsuarioId)                                                                       ║
 * ║ • Viagens (como UsuarioIdCriacao ou UsuarioIdFinalizacao)                                               ║
 * ║ • Manutenções (Alteração, Criação, Finalização, Cancelamento)                                           ║
 * ║ • MovimentacaoPatrimonio (como ResponsavelMovimentacao)                                                 ║
 * ║ • SetorPatrimonial (como DetentorId)                                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork (AspNetUsers, ControleAcesso, ViewControleAcesso, Recurso, Viagem, Manutencao,            ║
 * ║   MovimentacaoPatrimonio, SetorPatrimonial)                                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

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
    public partial class UsuarioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

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
