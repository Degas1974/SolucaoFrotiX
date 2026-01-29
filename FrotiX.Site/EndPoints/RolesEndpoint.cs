/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : RolesEndpoint.cs                                                ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ API Endpoint para gerenciamento de Roles (perfis/grupos) do ASP.NET Identity.║
║ CRUD completo via RoleManager.                                               ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET    /api/roles      : Lista todas as roles                              ║
║ - GET    /api/roles/{id} : Busca role por ID                                 ║
║ - POST   /api/roles      : Cria nova role                                    ║
║ - PUT    /api/roles      : Atualiza role existente                           ║
║ - DELETE /api/roles      : Remove role (bloqueia role padrao)                ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 21          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrotiX.Models;

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
            var roles = await _manager.Roles.AsNoTracking().ToListAsync();

            return Ok(new { data = roles, recordsTotal = roles.Count, recordsFiltered = roles.Count });
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityRole>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] IdentityRole model)
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] IdentityRole model)
            {
            var result = await _manager.UpdateAsync(model);

            if (result.Succeeded)
                {
                return NoContent();
                }

            return BadRequest(result);
            }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] IdentityRole model)
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
        }
    }


