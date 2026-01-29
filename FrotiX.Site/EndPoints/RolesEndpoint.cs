/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RolesEndpoint.cs                                                                       ║
   ║ 📂 CAMINHO: /EndPoints                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    API Endpoint para gerenciamento de Roles (perfis/grupos) do ASP.NET Identity.                   ║
   ║    CRUD completo via RoleManager.                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [Get]    : Lista todas as roles.............. () -> ActionResult<IEnumerable<IdentityRole>>    ║
   ║ 2. [Get]    : Busca role por ID................. (string id) -> ActionResult<IdentityRole>        ║
   ║ 3. [Create] : Cria nova role.................... (IdentityRole model) -> IActionResult            ║
   ║ 4. [Update] : Atualiza role existente........... (IdentityRole model) -> IActionResult            ║
   ║ 5. [Delete] : Remove role (bloqueia padrão)..... (IdentityRole model) -> IActionResult            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Get (Lista)                                                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Lista todas as roles do sistema para DataTable.                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS: { data, recordsTotal, recordsFiltered }                                  │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : GET /api/roles (Pages/Roles.cshtml)                               │
        /// │    ➡️ CHAMA       : RoleManager.Roles                                                 │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> Get()
        {
            try
            {
                // [DB] Busca todas as roles sem tracking
                var roles = await _manager.Roles.AsNoTracking().ToListAsync();

                // [DADOS] Retorna formato DataTable
                return Ok(new { data = roles, recordsTotal = roles.Count, recordsFiltered = roles.Count });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Get (Por ID)                                                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Busca uma role específica pelo ID.                                     │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • id [string]: ID da role (GUID)                                          │
        /// │ 📤 OUTPUTS: IdentityRole                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityRole>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Create                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Cria uma nova role no sistema. Gera novo GUID automaticamente.         │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • model [IdentityRole]: Dados da nova role                                │
        /// │ 📤 OUTPUTS: 201 Created com location header ou BadRequest                            │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] IdentityRole model)
        {
            try
            {
                // [DADOS] Gerar IDs únicos
                model.Id = Guid.NewGuid().ToString();
                model.ConcurrencyStamp = Guid.NewGuid().ToString();

                // [DB] Criar role via Identity
                var result = await _manager.CreateAsync(model);

                if (result.Succeeded)
                {
                    return CreatedAtAction("Get", new { id = model.Id }, model);
                }

                return BadRequest(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Create", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Update                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Atualiza uma role existente.                                           │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • model [IdentityRole]: Dados atualizados                                 │
        /// │ 📤 OUTPUTS: 204 NoContent ou BadRequest                                              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] IdentityRole model)
        {
            try
            {
                // [DB] Atualizar role via Identity
                var result = await _manager.UpdateAsync(model);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Update", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Delete                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Remove uma role. Bloqueia exclusão da role padrão do sistema.          │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • model [IdentityRole]: Role a ser removida                               │
        /// │ 📤 OUTPUTS: 204 NoContent ou BadRequest (se for role padrão)                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] IdentityRole model)
        {
            try
            {
                // [REGRA] Impedir exclusão da role padrão
                if (model.Name == _settings.Theme.Role)
                {
                    return BadRequest(SmartError.Failed("Please do not delete the default role! =)"));
                }

                // [DB] Deletar role via Identity
                var result = await _manager.DeleteAsync(model);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Delete", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }
    }
}


