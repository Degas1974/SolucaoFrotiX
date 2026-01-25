/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Solicitantes (Core Stack)         |
 * |_____________________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de requisitantes de viagens,
 * vínculos com setores solicitantes e parâmetros de atendimento.
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
    /// ║ 📌 NOME: RequisitanteController                                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de requisitantes, vínculos com setores e parâmetros de atendimento.║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Requisitante                                           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class RequisitanteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RequisitanteController (Construtor)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public RequisitanteController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs", "RequisitanteController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista requisitantes formatados para DataTables (legacy).                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados para DataTables.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta requisitantes com setor solicitante.
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

                // [RETORNO] DataTables payload.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Get" , error);
                _log.Error("Erro ao listar requisitantes (DataTables legacy)", error);
                return View();
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAll (GET)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista requisitantes para dropdowns/combos (API JSON).                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista simples.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                // [DADOS] Consulta requisitantes com setor (left join).
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

                // [RETORNO] Lista simples.
                return Json(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetAll" , error);
                _log.Error("Erro ao listar requisitantes (API JSON)", error);
                return Json(new { success = false , message = "Erro ao listar requisitantes" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetById (GET)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém detalhes de requisitante por ID (Guid).                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (string): ID do requisitante.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do requisitante.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id , out Guid guidId))
                {
                    return Json(new { success = false , message = "ID inválido" });
                }

                // [DADOS] Busca requisitante.
                var requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(r => r.RequisitanteId == guidId);
                if (requisitante == null)
                {
                    return Json(new { success = false , message = "Requisitante não encontrado" });
                }

                // [RETORNO] Dados do requisitante.
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
                _log.Error($", errorErro ao buscar requisitante [ID: {id}]");
                return Json(new { success = false , message = "Erro ao buscar requisitante" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Upsert (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cria ou atualiza requisitante com base no modelo enviado.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (RequisitanteUpsertModel): Dados do requisitante.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Upsert")]
        [HttpPost]
        public IActionResult Upsert([FromBody] RequisitanteUpsertModel model)
        {
            try
            {
                // [VALIDACAO] Modelo e nome obrigatório.
                if (model == null || string.IsNullOrEmpty(model.Nome))
                {
                    return Json(new { success = false , message = "Nome é obrigatório" });
                }

                Requisitante requisitante;
                // [REGRA] Determina criação ou atualização.
                bool isNew = string.IsNullOrEmpty(model.RequisitanteId) || model.RequisitanteId == Guid.Empty.ToString();

                // [VALIDACAO] Parse do SetorSolicitanteId.
                Guid setorId = Guid.Empty;
                if (!string.IsNullOrEmpty(model.SetorSolicitanteId))
                {
                    Guid.TryParse(model.SetorSolicitanteId , out setorId);
                }

                // [DADOS] ID do usuário logado.
                var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

                if (isNew)
                {
                    // [ACAO] Cria novo requisitante.
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
                    _log.Info($"Criado novo requisitante: [Nome: {model.Nome}] [Ponto: {model.Ponto}]");
                }
                else
                {
                    // [DADOS] Busca requisitante existente.
                    var id = Guid.Parse(model.RequisitanteId);
                    requisitante = _unitOfWork.Requisitante.GetFirstOrDefault(r => r.RequisitanteId == id);

                    if (requisitante == null)
                    {
                        return Json(new { success = false , message = "Requisitante não encontrado" });
                    }

                    // [ACAO] Atualiza campos.
                    requisitante.Ponto = model.Ponto ?? "";
                    requisitante.Nome = model.Nome;
                    requisitante.Ramal = model.Ramal;
                    requisitante.Status = model.Status;
                    requisitante.SetorSolicitanteId = setorId;
                    requisitante.DataAlteracao = DateTime.Now;
                    requisitante.UsuarioIdAlteracao = usuarioId;

                    _unitOfWork.Requisitante.Update(requisitante);
                    _log.Info($"Atualizado requisitante: [ID: {model.RequisitanteId}] [Nome: {model.Nome}]");
                }

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();

                // [RETORNO] Resultado do upsert.
                return Json(new
                {
                    success = true ,
                    message = isNew ? "Requisitante criado com sucesso" : "Requisitante atualizado com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Upsert" , error);
                _log.Error($", errorErro ao realizar Upsert de requisitante [Nome: {model?.Nome}]");
                var innerMsg = error.InnerException != null ? error.InnerException.Message : "";
                return Json(new { success = false , message = $"Erro: {error.Message} | {innerMsg}" });
            }
        }


        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetSetores (GET)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna setores solicitantes ativos para seleção.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com setores ativos.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetSetores")]
        [HttpGet]
        public IActionResult GetSetores()
        {
            try
            {
                // [DADOS] Consulta setores ativos.
                var setores = _unitOfWork.SetorSolicitante.GetAll()
                    .Where(s => s.Status)
                    .OrderBy(s => s.Nome)
                    .Select(s => new
                    {
                        id = s.SetorSolicitanteId.ToString() ,
                        nome = s.Nome ?? ""
                    })
                    .ToList();

                // [RETORNO] Lista de setores.
                return Json(setores);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetSetores" , error);
                _log.Error("Erro ao buscar setores ativos", error);
                return Json(new List<object>());
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove um requisitante do sistema.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (RequisitanteViewModel): Dados com ID do requisitante.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(RequisitanteViewModel model)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (model != null && model.RequisitanteId != Guid.Empty)
                {
                    // [DADOS] Busca requisitante.
                    var objFromDb = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                        u.RequisitanteId == model.RequisitanteId
                    );
                    if (objFromDb != null)
                    {
                        // [ACAO] Remove e salva.
                        var nome = objFromDb.Nome;
                        _unitOfWork.Requisitante.Remove(objFromDb);
                        _unitOfWork.Save();
                        _log.Info($"Requisitante removido: [ID: {model.RequisitanteId}] [Nome: {nome}]");
                        // [RETORNO] Sucesso.
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Requisitante removido com sucesso"
                            }
                        );
                    }
                }
                // [RETORNO] Falha padrão.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Requisitante"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "Delete" , error);
                _log.Error($", errorErro ao deletar requisitante [ID: {model?.RequisitanteId}]");
                return Json(new { success = false , message = "Erro ao deletar requisitante" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetSetoresHierarquia (GET)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém a estrutura hierárquica (tree) de setores ativos.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com árvore de setores.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetSetoresHierarquia")]
        [HttpGet]
        public IActionResult GetSetoresHierarquia()
        {
            try
            {
                // [DADOS] Carrega setores ativos.
                var todosSetores = _unitOfWork.SetorSolicitante.GetAll()
                    .Where(s => s.Status)
                    .ToList();

                // [REGRA] Busca setores raiz (sem pai).
                var raizes = todosSetores
                    .Where(s => !s.SetorPaiId.HasValue || s.SetorPaiId.Value == Guid.Empty)
                    .OrderBy(s => s.Nome)
                    .Select(s => MontarHierarquiaSetor(s , todosSetores))
                    .ToList();

                // [RETORNO] Árvore de setores.
                return Json(raizes);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "GetSetoresHierarquia" , error);
                _log.Error("Erro ao buscar hierarquia de setores", error);
                return Json(new List<object>());
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MontarHierarquiaSetor (Helper)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Monta recursivamente a árvore de setores solicitantes.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • setor (SetorSolicitante): Nó raiz.                                     ║
        /// ║    • todosSetores (List<SetorSolicitante>): Lista completa.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • object: Estrutura hierárquica com filhos.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private object MontarHierarquiaSetor(SetorSolicitante setor , List<SetorSolicitante> todosSetores)
        {
            // [DADOS] Busca filhos do setor.
            var filhos = todosSetores
                .Where(s => s.SetorPaiId == setor.SetorSolicitanteId)
                .OrderBy(s => s.Nome)
                .Select(s => MontarHierarquiaSetor(s , todosSetores))
                .ToList();

            // [RETORNO] Nó com filhos.
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

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarRequisitanteRamalSetor (POST)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza ramal e/ou setor do requisitante.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto (AtualizarRequisitanteDto): Dados de atualização.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da atualização.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AtualizarRequisitanteRamalSetor")]
        [HttpPost]
        public IActionResult AtualizarRequisitanteRamalSetor([FromBody] AtualizarRequisitanteDto dto)
        {
            try
            {
                // [VALIDACAO] ID do requisitante.
                if (dto.RequisitanteId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "ID do requisitante inválido"
                    });
                }

                // [DADOS] Busca requisitante.
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

                // [REGRA] Detecta mudanças.
                bool houveMudanca = false;

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
                    // [ACAO] Persiste alterações.
                    requisitante.DataAlteracao = DateTime.Now;

                    _unitOfWork.Requisitante.Update(requisitante);
                    _unitOfWork.Save();

                    _log.Info($"Atualizado Ramal/Setor do requisitante: [ID: {dto.RequisitanteId}] [Nome: {requisitante.Nome}]");

                    // [RETORNO] Atualização aplicada.
                    return Json(new
                    {
                        success = true ,
                        message = "Requisitante atualizado com sucesso"
                    });
                }

                // [RETORNO] Nenhuma alteração necessária.
                return Json(new
                {
                    success = true ,
                    message = "Nenhuma alteração necessária"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RequisitanteController.cs" , "AtualizarRequisitanteRamalSetor" , error);
                _log.Error($", errorErro ao atualizar ramal/setor do requisitante [ID: {dto?.RequisitanteId}]");
                return Json(new
                {
                    success = false ,
                    message = "Erro ao atualizar requisitante"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusRequisitante (POST)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status (ativo/inativo) do requisitante.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do requisitante.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusRequisitante")]
        public JsonResult UpdateStatusRequisitante(Guid Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Busca requisitante.
                    var objFromDb = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                        u.RequisitanteId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [REGRA] Alterna status.
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
                        _unitOfWork.Save(); // Adicionado Save que faltava no original para persistir a mudança de status
                        _log.Info(Description);
                    }
                    // [RETORNO] Resultado da operação.
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                // [RETORNO] ID inválido.
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
                _log.Error($", errorErro ao alternar status do requisitante [ID: {Id}]");
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
