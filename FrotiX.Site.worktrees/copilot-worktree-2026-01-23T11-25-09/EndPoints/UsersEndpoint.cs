/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Endpoint de API para gerenciamento de Usuários.
 * =========================================================================================
 */

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
using FrotiX.Helpers; // Adicionado para Alerta

namespace FrotiX.EndPoints
    {
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
            try
            {
                var users = await _manager.Users.AsNoTracking().ToListAsync();

                return Ok(new { data = users, recordsTotal = users.Count, recordsFiltered = users.Count });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get", ex);
                return StatusCode(500, new { success = false, message = "Erro ao buscar usuários" });
            }
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityUser>> Get([FromRoute] string id)
        {
            try
            {
                return Ok(await _manager.FindByIdAsync(id));
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get(id)", ex);
                return StatusCode(500, new { success = false, message = "Erro ao buscar usuário" });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] IdentityUser model)
            {
            try
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
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Create", ex);
                return StatusCode(500, new { success = false, message = "Erro ao criar usuário" });
            }
            }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] IdentityUser model)
            {
            try
            {
                var result = await _context.UpdateAsync(model, model.Id);

                if (result.Succeeded)
                    {
                    return NoContent();
                    }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Update", ex);
                return StatusCode(500, new { success = false, message = "Erro ao atualizar usuário" });
            }
            }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] IdentityUser model)
            {
            try
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
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Delete", ex);
                return StatusCode(500, new { success = false, message = "Erro ao deletar usuário" });
            }
            }
        }
    }


