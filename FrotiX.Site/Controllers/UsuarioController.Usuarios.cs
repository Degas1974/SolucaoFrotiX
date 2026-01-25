/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está completamente documentado em:                         ║
 * ║  📄 Documentacao/Controllers/UsuarioController.md                        ║
 * ║                                                                          ║
 * ║  ⚠️ CLASSE PARCIAL (partial class)                                       ║
 * ║  Este é um dos arquivos da classe UsuarioController.                     ║
 * ║  Documentação completa abrange todos os arquivos parciais.               ║
 * ║                                                                          ║
 * ║  Última atualização: 12/01/2026                                          ║
 * ║                                                                          ║
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
    public partial class UsuarioController : Controller
    {
        /// <summary>
        /// Retorna todos os usuários com foto em Base64 para exibição na grid
        /// Inclui validação de vínculos para desabilitar botão de exclusão preventivamente
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var usuarios = _unitOfWork.AspNetUsers.GetAll().OrderBy(u => u.NomeCompleto).ToList();
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
                        UsuarioId = u.Id,
                        u.NomeCompleto,
                        u.Ponto,
                        u.DetentorCargaPatrimonial,
                        u.Status,
                        FotoBase64 = u.Foto != null ? Convert.ToBase64String(u.Foto) : null,
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
                Alerta.TratamentoErroComLinha("UsuarioController.Usuarios.cs", "GetAll", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar dados dos usuários"
                });
            }
        }

        /// <summary>
        /// Retorna a foto de um usuário específico em Base64
        /// </summary>
        [HttpGet]
        [Route("GetFoto")]
        public IActionResult GetFoto(string usuarioId)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID do usuário não informado"
                    });
                }

                var usuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == usuarioId);

                if (usuario == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Usuário não encontrado"
                    });
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        NomeCompleto = usuario.NomeCompleto,
                        FotoBase64 = usuario.Foto != null ? Convert.ToBase64String(usuario.Foto) : null
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsuarioController.cs", "GetFoto", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar foto do usuário"
                });
            }
        }
    }
}
