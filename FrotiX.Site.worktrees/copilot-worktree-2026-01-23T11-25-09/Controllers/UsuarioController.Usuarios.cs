using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: UsuarioController (Usuarios)                                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extensão parcial para listagem de usuários com foto.                      ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class UsuarioController : Controller
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAll (GET)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista usuários com foto Base64 e indicador de exclusão.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados da listagem.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                // [DADOS] Recupera usuários.
                var usuarios = _unitOfWork.AspNetUsers.GetAll().OrderBy(u => u.NomeCompleto).ToList();
                // [DADOS] Inicializa resultado.
                var result = new List<object>();

                // [LOGICA] Itera sobre cada usuário para processar e verificar vínculos
                foreach (var u in usuarios)
                {
                    // [REGRA] Assume exclusão permitida.
                    bool podeExcluir = true;

                    // ═══════════════════════════════════════════════════════════════
                    // 🔹 BLOCO: Verificação de Vínculos de Exclusão (FKs)
                    // Verifica se o usuário possui registros relacionados em diversas
                    // tabelas que impediriam sua exclusão para manter a integridade
                    // referencial do banco de dados.
                    // ═══════════════════════════════════════════════════════════════

                    // [REGRA] 1. ControleAcesso: Verifica vínculos no controle de acesso
                    var temControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                        ca.UsuarioId == u.Id
                    );
                    if (temControleAcesso != null) podeExcluir = false;

                    // [REGRA] 2. Viagens: Verifica vínculos em viagens (criação/finalização)
                    if (podeExcluir)
                    {
                        var temViagens = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                            v.UsuarioIdCriacao == u.Id || v.UsuarioIdFinalizacao == u.Id
                        );
                        if (temViagens != null) podeExcluir = false;
                    }

                    // [REGRA] 3. Manutenções: Verifica vínculos em manutenções (diversos papéis)
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

                    // [REGRA] 4. MovimentacaoPatrimonio: Verifica vínculos em movimentações de patrimônio
                    if (podeExcluir)
                    {
                        var temMovimentacao = _unitOfWork.MovimentacaoPatrimonio.GetFirstOrDefault(mp =>
                            mp.ResponsavelMovimentacao == u.Id
                        );
                        if (temMovimentacao != null) podeExcluir = false;
                    }

                    // [REGRA] 5. SetorPatrimonial: Verifica vínculos em setores patrimoniais
                    if (podeExcluir)
                    {
                        var temSetor = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(sp =>
                            sp.DetentorId == u.Id
                        );
                        if (temSetor != null) podeExcluir = false;
                    }

                    // [DADOS] Adiciona usuário com foto.
                    result.Add(new
                    {
                        UsuarioId = u.Id,
                        u.NomeCompleto,
                        u.Ponto,
                        u.DetentorCargaPatrimonial,
                        u.Status,
                        FotoBase64 = u.Foto != null ? Convert.ToBase64String(u.Foto) : null, // [DADOS] Converte foto para Base64
                        PodeExcluir = podeExcluir
                    });
                }

                // [RETORNO] Lista de usuários.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UsuarioController", "GetAll");
                Alerta.TratamentoErroComLinha("UsuarioController.Usuarios.cs", "GetAll", error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar dados dos usuários"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetFoto (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a foto do usuário em Base64.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • usuarioId (string): ID do usuário.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com foto e nome.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("GetFoto")]
        public IActionResult GetFoto(string usuarioId)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID do usuário não informado"
                    });
                }

                // [DADOS] Busca usuário.
                var usuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == usuarioId);

                // [VALIDACAO] Usuário encontrado.
                if (usuario == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Usuário não encontrado"
                    });
                }

                // [RETORNO] Foto em Base64.
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        NomeCompleto = usuario.NomeCompleto,
                        FotoBase64 = usuario.Foto != null ? Convert.ToBase64String(usuario.Foto) : null // [DADOS] Converte foto para Base64
                    }
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UsuarioController", "GetFoto");
                Alerta.TratamentoErroComLinha("UsuarioController.Usuarios.cs", "GetFoto", error); // Correção do nome do arquivo
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar foto do usuário"
                });
            }
        }
    }
}
