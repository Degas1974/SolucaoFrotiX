using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: SetorSolicitanteController (GetAll)                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extensão parcial com rotas de consulta e hierarquia de setores.           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class SetorSolicitanteController : Controller
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAll (GET)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna árvore hierárquica de setores solicitantes.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com árvore de setores.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                // [DADOS] Carrega setores.
                var todosSetores = _unitOfWork.SetorSolicitante.GetAll()
                    .OrderBy(s => s.Nome)
                    .ToList();

                // [REGRA] Monta estrutura hierárquica.
                var raizes = todosSetores
                    .Where(s => !s.SetorPaiId.HasValue || s.SetorPaiId == Guid.Empty)
                    .Select(s => MontarHierarquia(s, todosSetores))
                    .ToList();

                // [RETORNO] Árvore de setores.
                return Json(raizes);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetAll", error);
                _log.Error("Erro ao listar setores solicitantes (Hierarquia)", error);
                return Json(new { success = false, message = "Erro ao listar setores solicitantes" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MontarHierarquia (Helper)                                      ║
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
        private object MontarHierarquia(SetorSolicitante setor, List<SetorSolicitante> todosSetores)
        {
            // [DADOS] Busca filhos do setor.
            var filhos = todosSetores
                .Where(s => s.SetorPaiId == setor.SetorSolicitanteId)
                .Select(s => MontarHierarquia(s, todosSetores))
                .ToList();

            // [RETORNO] Nó com filhos.
            return new
            {
                setorSolicitanteId = setor.SetorSolicitanteId.ToString(),
                setorPaiId = setor.SetorPaiId.HasValue && setor.SetorPaiId.Value != Guid.Empty
                    ? setor.SetorPaiId.Value.ToString()
                    : (string)null,
                nome = setor.Nome ?? "",
                sigla = setor.Sigla ?? "",
                ramal = setor.Ramal.HasValue ? setor.Ramal.Value : 0,
                status = setor.Status ? 1 : 0,
                children = filhos.Count > 0 ? filhos : null
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetById (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém detalhes de um setor solicitante por ID.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (string): ID do setor solicitante.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do setor.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
                {
                    return Json(new { success = false, message = "ID inválido" });
                }

                // [DADOS] Busca setor.
                var setor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(s => s.SetorSolicitanteId == guidId);
                if (setor == null)
                {
                    return Json(new { success = false, message = "Setor não encontrado" });
                }

                // [RETORNO] Dados do setor.
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        setorSolicitanteId = setor.SetorSolicitanteId.ToString(),
                        setorPaiId = setor.SetorPaiId.HasValue && setor.SetorPaiId.Value != Guid.Empty
                            ? setor.SetorPaiId.Value.ToString()
                            : "",
                        nome = setor.Nome ?? "",
                        sigla = setor.Sigla ?? "",
                        ramal = setor.Ramal ?? 0,
                        status = setor.Status
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetById", error);
                _log.Error($", errorErro ao buscar setor solicitante [ID: {id}]");
                return Json(new { success = false, message = "Erro ao buscar setor solicitante" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Upsert (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cria ou atualiza setor solicitante.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (SetorSolicitanteUpsertModel): Dados do setor.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Upsert")]
        [HttpPost]
        public IActionResult Upsert([FromBody] SetorSolicitanteUpsertModel model)
        {
            try
            {
                // [VALIDACAO] Modelo e nome obrigatório.
                if (model == null || string.IsNullOrEmpty(model.Nome))
                {
                    return Json(new { success = false, message = "Nome é obrigatório" });
                }

                SetorSolicitante setor;
                // [REGRA] Determina criação ou atualização.
                bool isNew = string.IsNullOrEmpty(model.SetorSolicitanteId) || model.SetorSolicitanteId == Guid.Empty.ToString();

                if (isNew)
                {
                    // [ACAO] Cria novo setor.
                    setor = new SetorSolicitante
                    {
                        SetorSolicitanteId = Guid.NewGuid(),
                        Nome = model.Nome,
                        Sigla = model.Sigla,
                        Ramal = model.Ramal,
                        Status = model.Status,
                        SetorPaiId = !string.IsNullOrEmpty(model.SetorPaiId) && Guid.TryParse(model.SetorPaiId, out Guid paiId) && paiId != Guid.Empty
                            ? paiId
                            : (Guid?)null,
                        DataAlteracao = DateTime.Now
                    };
                    _unitOfWork.SetorSolicitante.Add(setor);
                    _log.Info($"Criado novo setor solicitante: [Nome: {model.Nome}] [Sigla: {model.Sigla}]");
                }
                else
                {
                    // [DADOS] Busca setor existente.
                    var id = Guid.Parse(model.SetorSolicitanteId);
                    setor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(s => s.SetorSolicitanteId == id);
                    
                    if (setor == null)
                    {
                        return Json(new { success = false, message = "Setor não encontrado" });
                    }

                    // [ACAO] Atualiza campos.
                    setor.Nome = model.Nome;
                    setor.Sigla = model.Sigla;
                    setor.Ramal = model.Ramal;
                    setor.Status = model.Status;
                    setor.SetorPaiId = !string.IsNullOrEmpty(model.SetorPaiId) && Guid.TryParse(model.SetorPaiId, out Guid paiId) && paiId != Guid.Empty
                        ? paiId
                        : (Guid?)null;
                    setor.DataAlteracao = DateTime.Now;

                    _unitOfWork.SetorSolicitante.Update(setor);
                    _log.Info($"Atualizado setor solicitante: [ID: {model.SetorSolicitanteId}] [Nome: {model.Nome}]");
                }

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();

                // [RETORNO] Resultado do upsert.
                return Json(new
                {
                    success = true,
                    message = isNew ? "Setor criado com sucesso" : "Setor atualizado com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "Upsert", error);
                _log.Error($", errorErro ao realizar Upsert de setor solicitante [Nome: {model?.Nome}]");
                return Json(new { success = false, message = "Erro ao salvar setor solicitante" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetSetoresPai (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna setores possíveis como pai (exclui o próprio).                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • excludeId (string): ID a excluir da lista.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com setores pai.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetSetoresPai")]
        [HttpGet]
        public IActionResult GetSetoresPai(string excludeId = null)
        {
            try
            {
                // [DADOS] Carrega setores ativos.
                var setores = _unitOfWork.SetorSolicitante.GetAll()
                    .Where(s => s.Status)
                    .OrderBy(s => s.Nome)
                    .ToList()
                    .Select(s => new
                    {
                        id = s.SetorSolicitanteId.ToString(),
                        nome = s.Nome ?? ""
                    })
                    .ToList();

                // [REGRA] Remove o próprio setor da lista (não pode ser pai de si mesmo).
                if (!string.IsNullOrEmpty(excludeId))
                {
                    setores = setores.Where(s => s.id != excludeId).ToList();
                }

                // [RETORNO] Lista de setores pai.
                return Json(setores);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetSetoresPai", error);
                _log.Error("Erro ao buscar lista de setores pai", error);
                return Json(new List<object>());
            }
        }
    }

    public class SetorSolicitanteUpsertModel
    {
        public string SetorSolicitanteId { get; set; }
        public string SetorPaiId { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public int? Ramal { get; set; }
        public bool Status { get; set; }
    }
}
