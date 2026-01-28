/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: RequisitanteController.cs                                        ║
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
     * ⚡ CONTROLLER: Requisitante API
     * 🎯 OBJETIVO: Gerenciar requisitantes de viagens (funcionários que solicitam veículos)
     * 📋 ROTAS: /api/Requisitante/* (Get, GetAll, GetById, Upsert, Delete, etc)
     * 🔗 ENTIDADES: Requisitante, SetorSolicitante
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 🌳 HIERARQUIA: Suporta árvore hierárquica de setores (GetSetoresHierarquia)
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class RequisitanteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RequisitanteController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "RequisitanteController.cs" ,
                    "RequisitanteController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * 🎯 OBJETIVO: Listar requisitantes com seus setores para grid (inner join)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { data: List<{ Ponto, Nome, Ramal, NomeSetor, Status, RequisitanteId }> }
         * 🔗 CHAMADA POR: Grid de requisitantes
         * 🔄 CHAMA: Requisitante.GetAll(), SetorSolicitante.GetAll()
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DOC] Inner join: retorna apenas requisitantes com setor associado
                var result = (
                    from r in _unitOfWork.Requisitante.GetAll()
                    join s in _unitOfWork.SetorSolicitante.GetAll()
                        on r.SetorSolicitanteId equals s.SetorSolicitanteId
                    orderby r.Nome
                    select new
                    {
                        r.Ponto ,
                        r.Nome ,
                        r.Ramal ,
                        NomeSetor = s.Nome ,
                        r.Status ,
                        r.RequisitanteId ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Get" , error);
                return View();
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAll
         * 🎯 OBJETIVO: Listar todos os requisitantes (com ou sem setor) para API externa
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON List<{ requisitanteId, ponto, nome, ramal, setorSolicitanteId, setorNome, status }>
         * 🔗 CHAMADA POR: APIs externas, comboboxes
         * 🔄 CHAMA: Requisitante.GetAll(), SetorSolicitante.GetAll()
         ****************************************************************************************/
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                // [DOC] Left join: retorna todos os requisitantes, mesmo sem setor associado
                var result = (
                    from r in _unitOfWork.Requisitante.GetAll()
                    join s in _unitOfWork.SetorSolicitante.GetAll()
                        on r.SetorSolicitanteId equals s.SetorSolicitanteId into setorJoin
                    from s in setorJoin.DefaultIfEmpty()
                    orderby r.Nome
                    select new
                    {
                        requisitanteId = r.RequisitanteId.ToString() ,
                        ponto = r.Ponto ?? "" ,
                        nome = r.Nome ?? "" ,
                        ramal = r.Ramal ?? 0 ,
                        setorSolicitanteId = r.SetorSolicitanteId != Guid.Empty
                            ? r.SetorSolicitanteId.ToString()
                            : "" ,
                        setorNome = s != null ? s.Nome ?? "" : "" ,
                        status = r.Status ? 1 : 0
                    }
                ).ToList();

                return Json(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetAll" , error);
                return Json(new { success = false , message = "Erro ao listar requisitantes" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetById
         * 🎯 OBJETIVO: Buscar requisitante por ID para edição
         * 📥 ENTRADAS: id (string GUID)
         * 📤 SAÍDAS: JSON { success, data: { requisitanteId, ponto, nome, ramal, setorSolicitanteId, status } }
         * 🔗 CHAMADA POR: Modal de edição de requisitante
         * 🔄 CHAMA: Requisitante.GetFirstOrDefault()
         ****************************************************************************************/
        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id , out Guid guidId))
                {
                    return Json(new { success = false , message = "ID inválido" });
                }

                var requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(r => r.RequisitanteId == guidId);
                if (requisitante == null)
                {
                    return Json(new { success = false , message = "Requisitante não encontrado" });
                }

                return Json(new
                {
                    success = true ,
                    data = new
                    {
                        requisitanteId = requisitante.RequisitanteId.ToString() ,
                        ponto = requisitante.Ponto ?? "" ,
                        nome = requisitante.Nome ?? "" ,
                        ramal = requisitante.Ramal ?? 0 ,
                        setorSolicitanteId = requisitante.SetorSolicitanteId != Guid.Empty
                            ? requisitante.SetorSolicitanteId.ToString()
                            : "" ,
                        status = requisitante.Status
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetById" , error);
                return Json(new { success = false , message = "Erro ao buscar requisitante" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Upsert
         * 🎯 OBJETIVO: Criar ou atualizar requisitante (insert ou update)
         * 📥 ENTRADAS: model (RequisitanteUpsertModel: RequisitanteId?, Ponto, Nome, Ramal, SetorSolicitanteId, Status)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de criação/edição de requisitante
         * 🔄 CHAMA: Requisitante.Add() ou Requisitante.Update()
         * 👤 AUDITORIA: Registra DataAlteracao e UsuarioIdAlteracao (via Claims)
         ****************************************************************************************/
        [Route("Upsert")]
        [HttpPost]
        public IActionResult Upsert([FromBody] RequisitanteUpsertModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Nome))
                {
                    return Json(new { success = false , message = "Nome é obrigatório" });
                }

                Requisitante requisitante;
                bool isNew = string.IsNullOrEmpty(model.RequisitanteId) || model.RequisitanteId == Guid.Empty.ToString();

                // [DOC] Parse do SetorSolicitanteId (pode ser null/empty)
                Guid setorId = Guid.Empty;
                if (!string.IsNullOrEmpty(model.SetorSolicitanteId))
                {
                    Guid.TryParse(model.SetorSolicitanteId , out setorId);
                }

                // [DOC] Captura usuário logado via Claims (ASP.NET Identity)
                var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

                if (isNew)
                {
                    requisitante = new Requisitante
                    {
                        RequisitanteId = Guid.NewGuid() ,
                        Ponto = model.Ponto ?? "" ,
                        Nome = model.Nome ,
                        Ramal = model.Ramal ,
                        Status = model.Status ,
                        SetorSolicitanteId = setorId ,
                        DataAlteracao = DateTime.Now ,
                        UsuarioIdAlteracao = usuarioId
                    };
                    _unitOfWork.Requisitante.Add(requisitante);
                }
                else
                {
                    var id = Guid.Parse(model.RequisitanteId);
                    requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(r => r.RequisitanteId == id);

                    if (requisitante == null)
                    {
                        return Json(new { success = false , message = "Requisitante não encontrado" });
                    }

                    requisitante.Ponto = model.Ponto ?? "";
                    requisitante.Nome = model.Nome;
                    requisitante.Ramal = model.Ramal;
                    requisitante.Status = model.Status;
                    requisitante.SetorSolicitanteId = setorId;
                    requisitante.DataAlteracao = DateTime.Now;
                    requisitante.UsuarioIdAlteracao = usuarioId;

                    _unitOfWork.Requisitante.Update(requisitante);
                }

                _unitOfWork.Save();

                return Json(new
                {
                    success = true ,
                    message = isNew ? "Requisitante criado com sucesso" : "Requisitante atualizado com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Upsert" , error);
                var innerMsg = error.InnerException != null ? error.InnerException.Message : "";
                return Json(new { success = false , message = $"Erro: {error.Message} | {innerMsg}" });
            }
        }


        /****************************************************************************************
         * ⚡ FUNÇÃO: GetSetores
         * 🎯 OBJETIVO: Listar setores ativos para dropdown/combobox
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON List<{ id, nome }>
         * 🔗 CHAMADA POR: Combobox de setores em formulários
         * 🔄 CHAMA: SetorSolicitante.GetAll()
         ****************************************************************************************/
        [Route("GetSetores")]
        [HttpGet]
        public IActionResult GetSetores()
        {
            try
            {
                // [DOC] Filtra apenas setores ativos (Status = true)
                var setores = _unitOfWork.SetorSolicitante.GetAll()
                    .Where(s => s.Status)
                    .OrderBy(s => s.Nome)
                    .Select(s => new
                    {
                        id = s.SetorSolicitanteId.ToString() ,
                        nome = s.Nome ?? ""
                    })
                    .ToList();

                return Json(setores);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetSetores" , error);
                return Json(new List<object>());
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * 🎯 OBJETIVO: Excluir requisitante do sistema
         * 📥 ENTRADAS: model (RequisitanteViewModel com RequisitanteId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de requisitante
         * 🔄 CHAMA: Requisitante.GetFirstOrDefault(), Requisitante.Remove()
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(RequisitanteViewModel model)
        {
            try
            {
                if (model != null && model.RequisitanteId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                        u.RequisitanteId == model.RequisitanteId
                    );
                    if (objFromDb != null)
                    {
                        _unitOfWork.Requisitante.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Requisitante removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Requisitante"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Delete" , error);
                return Json(new { success = false , message = "Erro ao deletar requisitante" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetSetoresHierarquia
         * 🎯 OBJETIVO: Retornar setores em estrutura hierárquica (árvore) para TreeView
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON List<{ id, nome, hasChild, children[] }>
         * 🔗 CHAMADA POR: TreeView de setores (SyncFusion ou similar)
         * 🔄 CHAMA: SetorSolicitante.GetAll(), MontarHierarquiaSetor() (recursivo)
         * 🌳 ESTRUTURA: Monta árvore recursiva de setores pai-filho
         ****************************************************************************************/
        [Route("GetSetoresHierarquia")]
        [HttpGet]
        public IActionResult GetSetoresHierarquia()
        {
            try
            {
                var todosSetores = _unitOfWork.SetorSolicitante.GetAll()
                    .Where(s => s.Status)
                    .ToList();

                // [DOC] Busca setores raiz (SetorPaiId = null ou Guid.Empty)
                var raizes = todosSetores
                    .Where(s => !s.SetorPaiId.HasValue || s.SetorPaiId.Value == Guid.Empty)
                    .OrderBy(s => s.Nome)
                    .Select(s => MontarHierarquiaSetor(s , todosSetores))
                    .ToList();

                return Json(raizes);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetSetoresHierarquia" , error);
                return Json(new List<object>());
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MontarHierarquiaSetor (Helper recursivo)
         * 🎯 OBJETIVO: Montar objeto hierárquico de setor com seus filhos (recursivo)
         * 📥 ENTRADAS: setor (SetorSolicitante), todosSetores (List completa)
         * 📤 SAÍDAS: Object { id, nome, hasChild, children[] }
         * 🔗 CHAMADA POR: GetSetoresHierarquia(), MontarHierarquiaSetor() (si mesmo - recursão)
         * 🔄 CHAMA: MontarHierarquiaSetor() recursivamente para cada filho
         * 🌳 RECURSÃO: Profundidade ilimitada de níveis hierárquicos
         ****************************************************************************************/
        private object MontarHierarquiaSetor(SetorSolicitante setor , List<SetorSolicitante> todosSetores)
        {
            // [DOC] Busca todos os filhos deste setor (SetorPaiId == setor.SetorSolicitanteId)
            var filhos = todosSetores
                .Where(s => s.SetorPaiId == setor.SetorSolicitanteId)
                .OrderBy(s => s.Nome)
                .Select(s => MontarHierarquiaSetor(s , todosSetores)) // [DOC] Recursão aqui
                .ToList();

            return new
            {
                id = setor.SetorSolicitanteId.ToString() ,
                nome = setor.Nome ?? "" ,
                hasChild = filhos.Count > 0 ,
                children = filhos.Count > 0 ? filhos : null
            };
        }

        public class AtualizarRequisitanteDto
        {
            public Guid RequisitanteId
            {
                get; set;
            }
            public int? Ramal
            {
                get; set;
            }
            public Guid? SetorSolicitanteId
            {
                get; set;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizarRequisitanteRamalSetor
         * 🎯 OBJETIVO: Atualização parcial de requisitante (apenas Ramal e SetorSolicitanteId)
         * 📥 ENTRADAS: dto (AtualizarRequisitanteDto: RequisitanteId, Ramal?, SetorSolicitanteId?)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: APIs externas ou formulários parciais
         * 🔄 CHAMA: Requisitante.GetFirstOrDefault(), Requisitante.Update()
         * 🔍 OTIMIZAÇÃO: Só atualiza se houver mudança real nos campos
         ****************************************************************************************/
        [Route("AtualizarRequisitanteRamalSetor")]
        [HttpPost]
        public IActionResult AtualizarRequisitanteRamalSetor([FromBody] AtualizarRequisitanteDto dto)
        {
            try
            {
                if (dto.RequisitanteId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID do requisitante inválido"
                    });
                }

                var requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(r =>
                    r.RequisitanteId == dto.RequisitanteId);

                if (requisitante == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Requisitante não encontrado"
                    });
                }

                bool houveMudanca = false;

                // [DOC] Atualiza apenas campos enviados e diferentes dos atuais
                if (dto.Ramal.HasValue && requisitante.Ramal != dto.Ramal.Value)
                {
                    requisitante.Ramal = dto.Ramal.Value;
                    houveMudanca = true;
                }

                if (dto.SetorSolicitanteId.HasValue && requisitante.SetorSolicitanteId != dto.SetorSolicitanteId.Value)
                {
                    requisitante.SetorSolicitanteId = dto.SetorSolicitanteId.Value;
                    houveMudanca = true;
                }

                if (houveMudanca)
                {
                    requisitante.DataAlteracao = DateTime.Now;

                    _unitOfWork.Requisitante.Update(requisitante);
                    _unitOfWork.Save();

                    return Json(new
                    {
                        success = true ,
                        message = "Requisitante atualizado com sucesso"
                    });
                }

                return Json(new
                {
                    success = true ,
                    message = "Nenhuma alteração necessária"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "AtualizarRequisitanteRamalSetor" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao atualizar requisitante"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusRequisitante
         * 🎯 OBJETIVO: Alternar status do requisitante (Ativo ↔ Inativo)
         * 📥 ENTRADAS: Id (Guid do requisitante)
         * 📤 SAÍDAS: JSON { success, message, type (0=ativo, 1=inativo) }
         * 🔗 CHAMADA POR: Toggle de status no grid
         * 🔄 CHAMA: Requisitante.GetFirstOrDefault(), Requisitante.Update()
         ****************************************************************************************/
        [Route("UpdateStatusRequisitante")]
        public JsonResult UpdateStatusRequisitante(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                        u.RequisitanteId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [DOC] Toggle status: true → false (type=1) ou false → true (type=0)
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Requisitante [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Requisitante [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        _unitOfWork.Requisitante.Update(objFromDb);
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
                    "RequisitanteController.cs" ,
                    "UpdateStatusRequisitante" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }
    }

    public class RequisitanteUpsertModel
    {
        public string RequisitanteId { get; set; }
        public string Ponto { get; set; }
        public string Nome { get; set; }
        public int? Ramal { get; set; }
        public string SetorSolicitanteId { get; set; }
        public bool Status { get; set; }
    }
}
