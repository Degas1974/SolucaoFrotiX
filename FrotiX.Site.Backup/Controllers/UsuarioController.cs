/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Usuários (Core Stack)             |
 * |_____________________________________________________________|
 *
 * (IA) Controlador parcial responsável pela gestão de usuários do
 * sistema (Identity), perfis de acesso e auditoria de logins.
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
    /// ║ 📌 NOME: UsuarioController                                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de usuários (AspNetUsers), perfis e permissões de acesso.          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Usuario                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public partial class UsuarioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UsuarioController (Construtor)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public UsuarioController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs", "UsuarioController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista usuários e informa se podem ser excluídos.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados para listagem.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Recupera usuários.
                var usuarios = _unitOfWork.AspNetUsers.GetAll().ToList();
                // [DADOS] Inicializa resultado.
                var result = new List<object>();

                // 🔹 BLOCO: Processamento de Cada Usuário
                // Itera sobre cada usuário recuperado para determinar se ele possui vínculos
                // que impediriam sua exclusão.
                foreach (var u in usuarios)
                {
                    // [REGRA] Inicializa flag de exclusão.
                    bool podeExcluir = true;

                    // ═══════════════════════════════════════════════════════════════
                    // 🔹 BLOCO: Verificação de Vínculos de Exclusão
                    // Cada bloco a seguir verifica a existência de registros relacionados
                    // em diferentes tabelas. Se um vínculo for encontrado, a exclusão é
                    // impossibilitada (`podeExcluir = false`).
                    // ═══════════════════════════════════════════════════════════════

                    // [REGRA] ControleAcesso.
                    var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                        ca.UsuarioId == u.Id
                    );
                    if (temControleAcesso != null) podeExcluir = false;

                    // [REGRA] Viagens.
                    if (podeExcluir)
                    {
                        var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                            v.UsuarioIdCriacao == u.Id || v.UsuarioIdFinalizacao == u.Id
                        );
                        if (temViagens != null) podeExcluir = false;
                    }

                    // [REGRA] Manutenções.
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

                    // [REGRA] Movimentação de patrimônio.
                    if (podeExcluir)
                    {
                        var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                            mp.ResponsavelMovimentacao == u.Id
                        );
                        if (temMovimentacao != null) podeExcluir = false;
                    }

                    // [REGRA] Setor patrimonial.
                    if (podeExcluir)
                    {
                        var temSetor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(sp =>
                            sp.DetentorId == u.Id
                        );
                        if (temSetor != null) podeExcluir = false;
                    }

                    // [DADOS] Adiciona usuário ao resultado.
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

                // [RETORNO] Sucesso.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UsuarioController", "Get");
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "Get" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove usuário se não houver vínculos com outras entidades.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • users (AspNetUsers): Dados com ID do usuário.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(AspNetUsers users)
        {
            try
            {
                // [DADOS] Busca usuário.
                var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == users.Id);
                // [VALIDACAO] Verifica existência.
                if (objFromDb == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Usuário não encontrado"
                    });
                }
                // [DADOS] Lista de vínculos.
                var vinculos = new List<string>();

                // [REGRA] 1. ControleAcesso: Verifica se o usuário tem controle de acesso associado.
                var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                    ca.UsuarioId == users.Id
                );
                if (temControleAcesso != null)
                {
                    vinculos.Add("Controle de Acesso a Recursos");
                }

                // [REGRA] 2. Viagens: Verifica se o usuário é responsável pela criação ou finalização de viagens.
                var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.UsuarioIdCriacao == users.Id || v.UsuarioIdFinalizacao == users.Id
                );
                if (temViagens != null)
                {
                    vinculos.Add("Viagens (como responsável pelo cadastro ou finalização)");
                }

                // [REGRA] 3. Manutenções: Verifica se o usuário está envolvido em manutenções.
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

                // [REGRA] 4. MovimentacaoPatrimonio: Verifica se o usuário é responsável por movimentações de patrimônio.
                var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                    mp.ResponsavelMovimentacao == users.Id
                );
                if (temMovimentacao != null)
                {
                    vinculos.Add("Movimentações de Patrimônio (como responsável)");
                }

                // [REGRA] 5. SetorPatrimonial: Verifica se o usuário é detentor de setores patrimoniais.
                var temSetor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(sp =>
                    sp.DetentorId == users.Id
                );
                if (temSetor != null)
                {
                    vinculos.Add("Setores Patrimoniais (como detentor)");
                }

                // [REGRA] Impede exclusão se houver vínculos.
                if (vinculos.Any())
                {
                    var mensagemVinculos = string.Join(", ", vinculos);
                    // [RETORNO] Mensagem para o frontend.
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

                // [ACAO] Remove usuário.
                _unitOfWork.AspNetUsers.Remove(objFromDb);
                _unitOfWork.Save();

                // [LOG] Registro de remoção.
                _log.Info($"Usuário removido com sucesso: {objFromDb.NomeCompleto} (ID: {users.Id})", "UsuarioController", "Delete");

                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true ,
                    message = $"✅ Usuário <strong>{objFromDb.NomeCompleto}</strong> removido com sucesso!"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UsuarioController", "Delete");
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "Delete" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "❌ Erro ao deletar usuário: " + error.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusUsuario (GET)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status (ativo/inativo) do usuário.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): ID do usuário.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusUsuario")]
        public JsonResult UpdateStatusUsuario(String Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != "")
                {
                    // [DADOS] Busca usuário.
                    var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
                    int type = 0; // 0 = Ativo, 1 = Inativo

                    // [REGRA] Se usuário encontrado.
                    if (objFromDb != null)
                    {
                        // [REGRA] Alterna status.
                        objFromDb.Status = !(objFromDb.Status ?? false);
                        // [REGRA] Define tipo de status.
                        type = (objFromDb.Status ?? false) ? 0 : 1;

                        // [ACAO] Persiste alterações.
                        _unitOfWork.AspNetUsers.Update(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro de alteração.
                        string statusMsg = (objFromDb.Status ?? false) ? "Ativo" : "Inativo";
                        _log.Info($"Status do Usuário atualizado para {statusMsg}: {objFromDb.NomeCompleto} (ID: {Id})", "UsuarioController", "UpdateStatusUsuario");
                    }

                    // [RETORNO] Resultado da operação.
                    return Json(
                        new
                        {
                            success = true ,
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
                _log.Error("Erro", error, "UsuarioController", "UpdateStatusUsuario");
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusUsuario" , error);
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateCargaPatrimonial (GET)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status de detentor de carga patrimonial.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): ID do usuário.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateCargaPatrimonial")]
        public JsonResult UpdateCargaPatrimonial(String Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != "")
                {
                    // [DADOS] Busca usuário.
                    var objFromDb = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);
                    string Description = ""; // Descrição da alteração para retorno
                    int type = 0; // 0 = Detentor, 1 = Não Detentor

                    // [REGRA] Se usuário encontrado.
                    if (objFromDb != null)
                    {
                        // [REGRA] Alterna status de detentor.
                        if (objFromDb.DetentorCargaPatrimonial == true)
                        {
                            objFromDb.DetentorCargaPatrimonial = false;
                            Description = string.Format(
                                "Atualizado Carga Patrimonial do Usuário [Nome: {0}] (Não)" ,
                                objFromDb.NomeCompleto
                            );
                            type = 1; // Não Detentor
                        }
                        else
                        {
                            objFromDb.DetentorCargaPatrimonial = true;
                            Description = string.Format(
                                "Atualizado Carga Patrimonial do Usuário  [Nome: {0}] (Ativo)" ,
                                objFromDb.NomeCompleto
                            );
                            type = 0; // Detentor
                        }
                        // [ACAO] Persiste alterações.
                        _unitOfWork.AspNetUsers.Update(objFromDb);
                        _unitOfWork.Save(); // Salva as mudanças
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
                    "UsuarioController.cs" ,
                    "UpdateCargaPatrimonial" ,
                    error
                );
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusAcesso (POST)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna acesso do usuário ao recurso informado.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • IDS (string): UsuarioId|RecursoId.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusAcesso")]
        public JsonResult UpdateStatusAcesso(String IDS)
        {
            try
            {
                // [DADOS] Processa IDs.
                string inputString = IDS;
                char separator = '|';
                string[] parts = inputString.Split(separator);

                string usuarioId = parts[0];
                string recursoId = parts[1];

                // [DADOS] Busca controle de acesso.
                var objFromDb = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                    ca.UsuarioId == usuarioId && ca.RecursoId == Guid.Parse(recursoId)
                );
                string Description = ""; // Descrição da alteração para retorno
                int type = 0; // 0 = Com Acesso, 1 = Sem Acesso

                // [REGRA] Se registro encontrado.
                if (objFromDb != null)
                {
                    // [REGRA] Alterna acesso.
                    if (objFromDb.Acesso == true)
                    {
                        objFromDb.Acesso = false;
                        Description = string.Format(
                            "Atualizado Acesso do Usuário ao Recurso (Sem Acesso)"
                        );
                        type = 1; // Sem Acesso
                    }
                    else
                    {
                        objFromDb.Acesso = true;
                        Description = string.Format(
                            "Atualizado Acesso do Usuário ao Recurso (Com Acesso)"
                        );
                        type = 0; // Com Acesso
                    }
                    // [ACAO] Persiste alterações.
                    _unitOfWork.Save();
                    _unitOfWork.ControleAcesso.Update(objFromDb);
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "UpdateStatusAcesso" , error);
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaRecursosUsuario (GET)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna recursos aos quais o usuário tem acesso.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UsuarioId (string): ID do usuário.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com recursos.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("PegaRecursosUsuario")]
        [HttpGet]
        public IActionResult PegaRecursosUsuario(String UsuarioId)
        {
            try
            {
                // [DADOS] Busca recursos do usuário.
                var objRecursos = _unitOfWork.ViewControleAcesso.GetAll(vca =>
                    vca.UsuarioId == UsuarioId
                );

                // [RETORNO] Lista de recursos.
                return Json(new
                {
                    data = objRecursos
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "PegaRecursosUsuario" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar recursos"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaUsuariosRecurso (GET)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna usuários com acesso ao recurso informado.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • RecursoId (string): ID do recurso.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com usuários.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("PegaUsuariosRecurso")]
        [HttpGet]
        public IActionResult PegaUsuariosRecurso(String RecursoId)
        {
            try
            {
                // [DADOS] Busca usuários por recurso.
                var objRecursos = _unitOfWork
                    .ViewControleAcesso.GetAll(vca => vca.RecursoId == Guid.Parse(RecursoId))
                    .OrderBy(vca => vca.NomeCompleto);

                // [RETORNO] Lista de usuários.
                return Json(new
                {
                    data = objRecursos
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "PegaUsuariosRecurso" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar usuários"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: InsereRecursosUsuario (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Insere controle de acesso para todos os usuários e recursos.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("InsereRecursosUsuario")]
        [HttpPost]
        public IActionResult InsereRecursosUsuario()
        {
            try
            {
                // [DADOS] Lista de usuários.
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

                // [DADOS] Lista de recursos.
                var objRecursos = _unitOfWork.Recurso.GetAll();

                // [PROCESSAMENTO] Itera usuários.
                foreach (var usuario in objUsuarios)
                {
                    // [PROCESSAMENTO] Itera recursos.
                    foreach (var recurso in objRecursos)
                    {
                        // [DADOS] Cria controle de acesso.
                        var objAcesso = new ControleAcesso();

                        objAcesso.UsuarioId = usuario.UsuarioId;
                        objAcesso.RecursoId = recurso.RecursoId;
                        objAcesso.Acesso = true; // Define acesso como verdadeiro

                        // [ACAO] Adiciona acesso e persiste.
                        _unitOfWork.ControleAcesso.Add(objAcesso);
                        _unitOfWork.Save();
                    }
                }

                // [RETORNO] Sucesso.
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
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao inserir recursos"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: listaUsuariosDetentores (GET)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna usuários detentores de carga patrimonial ativos.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de usuários.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("listaUsuariosDetentores")]
        public IActionResult listaUsuariosDetentores()
        {
            try
            {
                // [DADOS] Busca usuários detentores ativos.
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

                // [RETORNO] Sucesso.
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
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao listar usuários"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteRecurso (POST)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove recurso se não houver vínculos de controle de acesso.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • RecursoId (string): ID do recurso.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DeleteRecurso")]
        [HttpPost]
        public IActionResult DeleteRecurso([FromBody] string RecursoId)
        {
            try
            {
                // [DADOS] Busca recurso.
                var objRecursos = _unitOfWork.Recurso.GetFirstOrDefault(r =>
                    r.RecursoId == Guid.Parse(RecursoId)
                );
                // [VALIDACAO] Verifica existência.
                if (objRecursos != null)
                {
                    // [REGRA] Verifica vínculo em controle de acesso.
                    var objControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                        ca.RecursoId == objRecursos.RecursoId
                    );
                    if (objControleAcesso != null)
                    {
                        // [RETORNO] Bloqueia exclusão.
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Não foi possível remover o Recursos. Ele está associado a um ou mais Usuários!" ,
                            }
                        );
                    }

                    // [ACAO] Remove recurso.
                    _unitOfWork.Recurso.Remove(objRecursos);
                    _unitOfWork.Save();
                    // [RETORNO] Sucesso.
                    return Json(new
                    {
                        success = true ,
                        message = "Recurso removido com sucesso"
                    });
                }

                // [RETORNO] Recurso não encontrado.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Usuário" // Mensagem original, pode ser melhorada para "Recurso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs" , "DeleteRecurso" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar recurso"
                });
            }
        }    }
}
