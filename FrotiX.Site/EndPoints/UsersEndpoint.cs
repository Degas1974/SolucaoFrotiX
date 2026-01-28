// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: UsersEndpoint.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ API REST para gerenciamento de Usuários do sistema.                          ║
// ║ Endpoints CRUD completo para administração de usuários Identity.             ║
// ║                                                                              ║
// ║ ROTAS DISPONÍVEIS:                                                           ║
// ║ - GET    /api/users      → Lista todos os usuários (formato DataTables)      ║
// ║ - GET    /api/users/{id} → Obtém usuário por ID                              ║
// ║ - POST   /api/users      → Cria novo usuário (com senha padrão)              ║
// ║ - PUT    /api/users      → Atualiza usuário existente                        ║
// ║ - DELETE /api/users      → Remove usuário (exceto usuário principal)         ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ - Usuário principal (_settings.Theme.Email) não pode ser deletado            ║
// ║ - Novos usuários recebem senha padrão "Password123!" (demo)                  ║
// ║ - UserName é automaticamente definido como o Email                           ║
// ║ - IDs gerados via Guid.NewGuid()                                             ║
// ║                                                                              ║
// ║ DEPENDÊNCIAS:                                                                ║
// ║ - ApplicationDbContext: Contexto Identity                                    ║
// ║ - IdentityExtensions: UpdateAsync, DeleteAsync                               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 11                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrotiX.Data;
using FrotiX.Extensions;
using FrotiX.Models;

namespace FrotiX.EndPoints
    {
    /// <summary>
    /// API REST para CRUD de Usuários.
    /// Rota base: /api/users
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UsersEndpoint : ControllerBase
        {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;
        private readonly SmartSettings _settings;

        public UsersEndpoint(ApplicationDbContext context, UserManager<IdentityUser> manager, SmartSettings settings)
            {
            _context = context;
            _manager = manager;
            _settings = settings;
            }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> Get()
            {
            var users = await _manager.Users.AsNoTracking().ToListAsync();

            return Ok(new { data = users, recordsTotal = users.Count, recordsFiltered = users.Count });
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityUser>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] IdentityUser model)
            {
            model.Id = Guid.NewGuid().ToString();
            model.UserName = model.Email;

            var result = await _manager.CreateAsync(model);

            if (result.Succeeded)
                {
                // HACK: This password is just for demonstration purposes!
                // Please do NOT keep it as-is for your own project!
                result = await _manager.AddPasswordAsync(model, "Password123!");

                if (result.Succeeded)
                    {
                    return CreatedAtAction("Get", new { id = model.Id }, model);
                    }
                }

            return BadRequest(result);
            }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] IdentityUser model)
            {
            var result = await _context.UpdateAsync(model, model.Id);

            if (result.Succeeded)
                {
                return NoContent();
                }

            return BadRequest(result);
            }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] IdentityUser model)
            {
            // HACK: The code below is just for demonstration purposes!
            // Please use a different method of preventing the currently logged in user from being removed
            if (model.UserName == _settings.Theme.Email)
                {
                return BadRequest(SmartError.Failed("Please do not delete the main user! =)"));
                }

            var result = await _context.DeleteAsync<IdentityUser>(model.Id);

            if (result.Succeeded)
                {
                return NoContent();
                }

            return BadRequest(result);
            }
        }
    }


