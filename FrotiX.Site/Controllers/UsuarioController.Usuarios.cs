/* ****************************************************************************************
 * ⚡ ARQUIVO: UsuarioController.Usuarios.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Listar usuários com foto Base64 e validar vínculos para exclusão.
 *
 * 📥 ENTRADAS     : IDs de usuário quando aplicável.
 *
 * 📤 SAÍDAS       : JSON com dados de usuários e flags de exclusão.
 *
 * 🔗 CHAMADA POR  : Telas administrativas de usuários.
 *
 * 🔄 CHAMA        : IUnitOfWork.AspNetUsers, ControleAcesso, Viagem, Manutencao,
 *                   MovimentacaoPatrimonio, SetorPatrimonial.
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
     * ⚡ CONTROLLER PARTIAL: UsuarioController.Usuarios
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar listagem avançada de usuários e foto em Base64.
     *
     * 📥 ENTRADAS     : IDs de usuário.
     *
     * 📤 SAÍDAS       : JSON com dados detalhados.
     ****************************************************************************************/
    public partial class UsuarioController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAll
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar usuários com foto Base64 e flag PodeExcluir.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com data (lista de usuários + PodeExcluir).
         *
         * 🔗 CHAMADA POR  : Grid de usuários.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetFoto
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar a foto do usuário em Base64.
         *
         * 📥 ENTRADAS     : usuarioId (string).
         *
         * 📤 SAÍDAS       : JSON com success e dados da foto.
         *
         * 🔗 CHAMADA POR  : Exibição de foto em detalhes/perfil.
         ****************************************************************************************/
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
