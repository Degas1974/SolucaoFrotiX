/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SetorSolicitanteController.GetAll.cs                                                    ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Partial com listagem/CRUD de setores solicitantes. Estrutura hierárquica pai/filho.    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: GetAll(), GetById(), Upsert(), GetSetoresPai() - MontarHierarquia() recursivo            ║
   ║ 🔗 DEPS: IUnitOfWork (SetorSolicitante) | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0                      ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    public partial class SetorSolicitanteController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAll
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos os setores solicitantes em estrutura hierarquica (arvore)
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON com arvore de setores (pai/filhos)
         * 🔗 CHAMADA POR  : TreeView de setores no frontend
         * 🔄 CHAMA        : SetorSolicitante.GetAll(), MontarHierarquia()
         *
         * 📊 ESTRUTURA DO RETORNO:
         *    - setorSolicitanteId, setorPaiId, nome, sigla, ramal, status
         *    - children: Array recursivo com setores filhos
         ****************************************************************************************/
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                // [DOC] Busca todos setores ordenados por nome
                var todosSetores = _unitOfWork.SetorSolicitante.GetAll()
                    .OrderBy(s => s.Nome)
                    .ToList();

                // [DOC] Monta estrutura hierarquica a partir dos setores raiz (sem pai)
                var raizes = todosSetores
                    .Where(s => !s.SetorPaiId.HasValue || s.SetorPaiId == Guid.Empty)
                    .Select(s => MontarHierarquia(s, todosSetores))
                    .ToList();

                return Json(raizes);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetAll", error);
                return Json(new { success = false, message = "Erro ao listar setores solicitantes" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MontarHierarquia (auxiliar recursiva)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Montar arvore recursiva de setores pai/filho
         * 📥 ENTRADAS     : [SetorSolicitante] setor - Setor atual
         *                   [List<SetorSolicitante>] todosSetores - Lista completa
         * 📤 SAÍDAS       : [object] Objeto anonimo com setor e filhos recursivos
         ****************************************************************************************/
        private object MontarHierarquia(SetorSolicitante setor, List<SetorSolicitante> todosSetores)
        {
            // [DOC] Busca filhos recursivamente (setores cujo SetorPaiId aponta para este)
            var filhos = todosSetores
                .Where(s => s.SetorPaiId == setor.SetorSolicitanteId)
                .Select(s => MontarHierarquia(s, todosSetores))
                .ToList();

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

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetById
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar um setor solicitante especifico pelo ID
         * 📥 ENTRADAS     : [string] id - GUID do setor como string
         * 📤 SAÍDAS       : [IActionResult] JSON { success, data: setor }
         * 🔗 CHAMADA POR  : Modal de edicao de setor no frontend
         ****************************************************************************************/
        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            try
            {
                // [DOC] Valida e converte ID string para GUID
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
                {
                    return Json(new { success = false, message = "ID inválido" });
                }

                var setor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(s => s.SetorSolicitanteId == guidId);
                if (setor == null)
                {
                    return Json(new { success = false, message = "Setor não encontrado" });
                }

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
                return Json(new { success = false, message = "Erro ao buscar setor solicitante" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Upsert
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar ou atualizar setor solicitante (Insert/Update)
         * 📥 ENTRADAS     : [SetorSolicitanteUpsertModel] model - DTO com dados do setor
         * 📤 SAÍDAS       : [IActionResult] JSON { success, message }
         * 🔗 CHAMADA POR  : Formulario de cadastro/edicao de setor
         * 🔄 CHAMA        : SetorSolicitante.Add() ou Update(), Save()
         ****************************************************************************************/
        [Route("Upsert")]
        [HttpPost]
        public IActionResult Upsert([FromBody] SetorSolicitanteUpsertModel model)
        {
            try
            {
                // [DOC] Validacao basica: nome e obrigatorio
                if (model == null || string.IsNullOrEmpty(model.Nome))
                {
                    return Json(new { success = false, message = "Nome é obrigatório" });
                }

                SetorSolicitante setor;
                // [DOC] Determina se e novo registro ou edicao pelo ID
                bool isNew = string.IsNullOrEmpty(model.SetorSolicitanteId) || model.SetorSolicitanteId == Guid.Empty.ToString();

                if (isNew)
                {
                    // [DOC] CRIAR: Novo setor com GUID gerado automaticamente
                    setor = new SetorSolicitante
                    {
                        SetorSolicitanteId = Guid.NewGuid(),
                        Nome = model.Nome,
                        Sigla = model.Sigla,
                        Ramal = model.Ramal,
                        Status = model.Status,
                        // [DOC] Se SetorPaiId for valido, vincula ao pai; senao fica null (setor raiz)
                        SetorPaiId = !string.IsNullOrEmpty(model.SetorPaiId) && Guid.TryParse(model.SetorPaiId, out Guid paiId) && paiId != Guid.Empty
                            ? paiId
                            : (Guid?)null,
                        DataAlteracao = DateTime.Now
                    };
                    _unitOfWork.SetorSolicitante.Add(setor);
                }
                else
                {
                    // [DOC] ATUALIZAR: Busca setor existente pelo ID
                    var id = Guid.Parse(model.SetorSolicitanteId);
                    setor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(s => s.SetorSolicitanteId == id);
                    
                    if (setor == null)
                    {
                        return Json(new { success = false, message = "Setor não encontrado" });
                    }

                    // [DOC] Atualiza todos os campos editaveis
                    setor.Nome = model.Nome;
                    setor.Sigla = model.Sigla;
                    setor.Ramal = model.Ramal;
                    setor.Status = model.Status;
                    setor.SetorPaiId = !string.IsNullOrEmpty(model.SetorPaiId) && Guid.TryParse(model.SetorPaiId, out Guid paiId) && paiId != Guid.Empty
                        ? paiId
                        : (Guid?)null;
                    setor.DataAlteracao = DateTime.Now;

                    _unitOfWork.SetorSolicitante.Update(setor);
                }

                // [DOC] Persiste alteracoes no banco
                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = isNew ? "Setor criado com sucesso" : "Setor atualizado com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "Upsert", error);
                return Json(new { success = false, message = "Erro ao salvar setor solicitante" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetSetoresPai
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar setores possiveis como pai (para dropdown de vinculacao)
         * 📥 ENTRADAS     : [string] excludeId - ID do setor a excluir (evita ciclo pai=self)
         * 📤 SAÍDAS       : [IActionResult] JSON [ { id, nome } ]
         * 🔗 CHAMADA POR  : Select de setor pai no formulario de cadastro
         ****************************************************************************************/
        [Route("GetSetoresPai")]
        [HttpGet]
        public IActionResult GetSetoresPai(string excludeId = null)
        {
            try
            {
                // [DOC] Busca apenas setores ativos ordenados por nome
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

                // [DOC] Remove o proprio setor da lista (previne referencia circular pai=filho)
                if (!string.IsNullOrEmpty(excludeId))
                {
                    setores = setores.Where(s => s.id != excludeId).ToList();
                }

                return Json(setores);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "GetSetoresPai", error);
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
