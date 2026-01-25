/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Endpoint de API para gerenciamento de Perfis de Acesso (Roles).
 * =========================================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrotiX.Models;
using FrotiX.Helpers; // Adicionado para Alerta

namespace FrotiX.EndPoints
    {
    [ApiController]
    [Route("api/roles")]
    public class RolesEndpoint : ControllerBase
        {
        private readonly RoleManager<IdentityRole> _manager;
        private readonly SmartSettings _settings;

        public RolesEndpoint(RoleManager<IdentityRole> manager, SmartSettings settings)
            {
            _manager = manager;
            _settings = settings;
            }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> Get()
            {
            try
            {
                var roles = await _manager.Roles.AsNoTracking().ToListAsync();

                return Ok(new { data = roles, recordsTotal = roles.Count, recordsFiltered = roles.Count });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get", ex);
                return StatusCode(500, new { success = false, message = "Erro ao buscar roles" });
            }
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityRole>> Get([FromRoute] string id)
        {
            try
            {
                return Ok(await _manager.FindByIdAsync(id));
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get(id)", ex);
                return StatusCode(500, new { success = false, message = "Erro ao buscar role" });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] IdentityRole model)
            {
            try
            {
                model.Id = Guid.NewGuid().ToString();
                model.ConcurrencyStamp = Guid.NewGuid().ToString();

                var result = await _manager.CreateAsync(model);

                if (result.Succeeded)
                    {
                    return CreatedAtAction("Get", new { id = model.Id }, model);
                    }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Create", ex);
                return StatusCode(500, new { success = false, message = "Erro ao criar role" });
            }
            }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] IdentityRole model)
            {
            try
            {
                var result = await _manager.UpdateAsync(model);

                if (result.Succeeded)
                    {
                    return NoContent();
                    }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Update", ex);
                return StatusCode(500, new { success = false, message = "Erro ao atualizar role" });
            }
            }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] IdentityRole model)
            {
            try
            {
                // HACK: The code below is just for demonstration purposes!
                // Please use a different method of preventing the default role from being removed
                if (model.Name == _settings.Theme.Role)
                    {
                    return BadRequest(SmartError.Failed("Please do not delete the default role! =)"));
                    }

                var result = await _manager.DeleteAsync(model);

                if (result.Succeeded)
                    {
                    return NoContent();
                    }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Delete", ex);
                return StatusCode(500, new { success = false, message = "Erro ao deletar role" });
            }
            }
        }
    }


