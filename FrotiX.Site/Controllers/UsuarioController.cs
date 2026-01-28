/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: UsuarioController.cs                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Usuario API (Partial Class)
     * 🎯 OBJETIVO: Gerenciar usuários e controle de acesso a recursos
     * 📋 ROTAS: /api/Usuario/* (Get, Delete, UpdateStatusUsuario, UpdateCargaPatrimonial, etc)
     * 🔗 ENTIDADES: AspNetUsers, ControleAcesso, Recurso, ViewControleAcesso
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 📝 NOTA: Classe parcial - métodos adicionais em UsuarioController.Usuarios.cs
     ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * 🎯 OBJETIVO: Listar todos os usuários com indicador de exclusão permitida
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { data: List<{ UsuarioId, NomeCompleto, Ponto, DetentorCargaPatrimonial, Status, PodeExcluir }> }
         * 🔗 CHAMADA POR: Grid de usuários
         * 🔄 CHAMA: AspNetUsers.GetAll(), valida vínculos em 5 tabelas
         * ⚠️ VALIDAÇÃO: Calcula PodeExcluir verificando ControleAcesso, Viagem, Manutencao, MovimentacaoPatrimonio, SetorPatrimonial
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
                    // [DOC] Verifica todos os vínculos para determinar se usuário pode ser excluído
                    bool podeExcluir = true;

                    // [DOC] Validação 1: ControleAcesso
                    var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                        ca.UsuarioId == u.Id
                    );
                    if (temControleAcesso != null) podeExcluir = false;

                    // [DOC] Validação 2: Viagens (criação ou finalização)
                    if (podeExcluir)
                    {
                        var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                            v.UsuarioIdCriacao == u.Id || v.UsuarioIdFinalizacao == u.Id
                        );
                        if (temViagens != null) podeExcluir = false;
                    }

                    // [DOC] Validação 3: Manutenções (criação, alteração, finalização ou cancelamento)
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

                    // [DOC] Validação 4: MovimentacaoPatrimonio (responsável)
                    if (podeExcluir)
                    {
                        var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                            mp.ResponsavelMovimentacao == u.Id
                        );
                        if (temMovimentacao != null) podeExcluir = false;
                    }

                    // [DOC] Validação 5: SetorPatrimonial (detentor)
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
         * 🎯 OBJETIVO: Excluir usuário (valida vínculos extensivamente antes de remover)
         * 📥 ENTRADAS: users (AspNetUsers com Id)
         * 📤 SAÍDAS: JSON { success, message } - message inclui HTML com lista de vínculos se houver
         * 🔗 CHAMADA POR: Modal de exclusão de usuário
         * 🔄 CHAMA: AspNetUsers.GetFirstOrDefault(), valida 5 tabelas, AspNetUsers.Remove()
         * ⚠️ VALIDAÇÕES: Impede exclusão se houver vínculos em qualquer das 5 áreas
         * 💬 FEEDBACK: Mensagem HTML formatada com lista de vínculos encontrados
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

                // [DOC] Lista acumuladora de vínculos encontrados para feedback detalhado
                var vinculos = new List<string>();

                // [DOC] Validação 1: ControleAcesso
                var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                    ca.UsuarioId == users.Id
                );
                if (temControleAcesso != null)
                {
                    vinculos.Add("Controle de Acesso a Recursos");
                }

                // [DOC] Validação 2: Viagens (Criação e Finalização)
                var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.UsuarioIdCriacao == users.Id || v.UsuarioIdFinalizacao == users.Id
                );
                if (temViagens != null)
                {
                    vinculos.Add("Viagens (como responsável pelo cadastro ou finalização)");
                }

                // [DOC] Validação 3: Manutenções (4 tipos de operação)
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

                // [DOC] Validação 4: MovimentacaoPatrimonio
                var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                    mp.ResponsavelMovimentacao == users.Id
                );
                if (temMovimentacao != null)
                {
                    vinculos.Add("Movimentações de Patrimônio (como responsável)");
                }

                // [DOC] Validação 5: SetorPatrimonial
                var temSetor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(sp =>
                    sp.DetentorId == users.Id
                );
                if (temSetor != null)
                {
                    vinculos.Add("Setores Patrimoniais (como detentor)");
                }

                // [DOC] Se houver vínculos, retorna mensagem HTML formatada com a lista
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

                // [DOC] Se não houver vínculos, pode excluir
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
         * 🎯 OBJETIVO: Alternar status do usuário (Ativo ↔ Inativo)
         * 📥 ENTRADAS: Id (UsuarioId String)
         * 📤 SAÍDAS: JSON { success, message, type (0=ativo, 1=inativo) }
         * 🔗 CHAMADA POR: Toggle de status no grid
         * 🔄 CHAMA: AspNetUsers.GetFirstOrDefault(), AspNetUsers.Update()
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
                        // [DOC] Toggle status: true → false (type=1) ou false → true (type=0)
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
         * 🎯 OBJETIVO: Alternar flag de detentor de carga patrimonial (Sim ↔ Não)
         * 📥 ENTRADAS: Id (UsuarioId String)
         * 📤 SAÍDAS: JSON { success, message, type (0=sim, 1=não) }
         * 🔗 CHAMADA POR: Toggle no grid de usuários
         * 🔄 CHAMA: AspNetUsers.GetFirstOrDefault(), AspNetUsers.Update()
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
                        // [DOC] Toggle DetentorCargaPatrimonial: true → false ou false → true
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
         * 🎯 OBJETIVO: Alternar permissão de acesso de usuário a recurso específico
         * 📥 ENTRADAS: IDS (String concatenada "usuarioId|recursoId" separada por pipe)
         * 📤 SAÍDAS: JSON { success, message, type (0=com acesso, 1=sem acesso) }
         * 🔗 CHAMADA POR: Toggle de acesso na matriz usuário-recurso
         * 🔄 CHAMA: ControleAcesso.GetFirstOrDefault(), ControleAcesso.Update()
         * 🔀 PARSING: Separa string de entrada por pipe para extrair IDs
         ****************************************************************************************/
        [Route("UpdateStatusAcesso")]
        public JsonResult UpdateStatusAcesso(String IDS)
        {
            try
            {
                // [DOC] Parse da string concatenada "usuarioId|recursoId"
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
                    // [DOC] Toggle acesso: true → false (type=1) ou false → true (type=0)
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
         * 🎯 OBJETIVO: Listar recursos e permissões de um usuário específico
         * 📥 ENTRADAS: UsuarioId (String)
         * 📤 SAÍDAS: JSON { data: registros da ViewControleAcesso }
         * 🔗 CHAMADA POR: Modal de gerenciamento de permissões do usuário
         * 🔄 CHAMA: ViewControleAcesso.GetAll()
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
         * 🎯 OBJETIVO: Listar usuários e permissões para um recurso específico
         * 📥 ENTRADAS: RecursoId (String - Guid)
         * 📤 SAÍDAS: JSON { data: registros da ViewControleAcesso ordenados por nome }
         * 🔗 CHAMADA POR: Modal de gerenciamento de permissões do recurso
         * 🔄 CHAMA: ViewControleAcesso.GetAll()
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
         * 🎯 OBJETIVO: Criar registros de controle de acesso para todos usuários x recursos (inicialização)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { data: true }
         * 🔗 CHAMADA POR: Script de inicialização/migração do sistema
         * 🔄 CHAMA: AspNetUsers.GetAll(), Recurso.GetAll(), ControleAcesso.Add() (loop duplo)
         * ⚠️ ATENÇÃO: Operação custosa - cria N x M registros (usuários × recursos)
         ****************************************************************************************/
        [Route("InsereRecursosUsuario")]
        [HttpPost]
        public IActionResult InsereRecursosUsuario()
        {
            try
            {
                // [DOC] Busca todos usuários e recursos do sistema
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

                // [DOC] Loop duplo: cria registro para cada combinação usuário-recurso
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: listaUsuariosDetentores
         * 🎯 OBJETIVO: Listar usuários detentores de carga patrimonial ativos
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, data: List<{ UsuarioId, NomeCompleto }> }
         * 🔗 CHAMADA POR: Combobox de detentores em formulários patrimoniais
         * 🔄 CHAMA: AspNetUsers.GetAll()
         * 🔍 FILTRO: DetentorCargaPatrimonial == true && Status == true
         ****************************************************************************************/
        [HttpGet]
        [Route("listaUsuariosDetentores")]
        public IActionResult listaUsuariosDetentores()
        {
            try
            {
                // [DOC] Filtra apenas usuários ativos que são detentores de carga
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
         * 🎯 OBJETIVO: Excluir recurso (valida se há controle de acesso associado)
         * 📥 ENTRADAS: RecursoId (String - Guid)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de recurso
         * 🔄 CHAMA: Recurso.GetFirstOrDefault(), ControleAcesso.GetFirstOrDefault(), Recurso.Remove()
         * ⚠️ VALIDAÇÃO: Impede exclusão se houver controle de acesso associado
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
                    // [DOC] Valida integridade referencial: não permite excluir recurso com permissões
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
