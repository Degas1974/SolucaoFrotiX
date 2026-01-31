/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: UsersEndpoint.cs                                                                       ║
   ║ 📂 CAMINHO: /EndPoints                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    API Endpoint para gerenciamento de Usuários do ASP.NET Identity.                                ║
   ║    CRUD completo via UserManager com criação de senha padrão.                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [Get]    : Lista todos os usuários........... () -> ActionResult<IEnumerable<IdentityUser>>    ║
   ║ 2. [Get]    : Busca usuário por ID.............. (string id) -> ActionResult<IdentityUser>        ║
   ║ 3. [Create] : Cria novo usuário................. (IdentityUser model) -> IActionResult            ║
   ║ 4. [Update] : Atualiza usuário existente........ (IdentityUser model) -> IActionResult            ║
   ║ 5. [Delete] : Remove usuário (bloqueia principal) (IdentityUser model) -> IActionResult           ║
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
using FrotiX.Data;
using FrotiX.Extensions;
using FrotiX.Models;

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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Get (Lista)                                                        │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Lista todos os usuários do sistema para DataTable.                     │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS: { data, recordsTotal, recordsFiltered }                                  │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : GET /api/users (Pages/Users.cshtml)                               │
        // │    ➡️ CHAMA       : UserManager.Users                                                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> Get()
        {
            try
            {
                // [DB] Busca todos os usuários sem tracking
                var users = await _manager.Users.AsNoTracking().ToListAsync();

                // [DADOS] Retorna formato DataTable
                return Ok(new { data = users, recordsTotal = users.Count, recordsFiltered = users.Count });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Get (Por ID)                                                       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Busca um usuário específico pelo ID.                                   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • id [string]: ID do usuário (GUID)                                       │
        // │ 📤 OUTPUTS: IdentityUser                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IdentityUser>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Create                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Cria um novo usuário com senha padrão "Password123!".                  │
        // │               Gera GUID automaticamente e define UserName = Email.                   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • model [IdentityUser]: Dados do novo usuário                             │
        // │ 📤 OUTPUTS: 201 Created ou BadRequest                                                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ ⚠️ ATENÇÃO: Senha padrão deve ser alterada em produção!                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] IdentityUser model)
        {
            try
            {
                // [DADOS] Gerar ID e definir UserName
                model.Id = Guid.NewGuid().ToString();
                model.UserName = model.Email;

                // [DB] Criar usuário via Identity
                var result = await _manager.CreateAsync(model);

                if (result.Succeeded)
                {
                    // [SEGURANCA] Adicionar senha padrão (ALTERAR EM PRODUÇÃO!)
                    result = await _manager.AddPasswordAsync(model, "Password123!");

                    if (result.Succeeded)
                    {
                        return CreatedAtAction("Get", new { id = model.Id }, model);
                    }
                }

                return BadRequest(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Create", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Atualiza dados de um usuário existente.                                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • model [IdentityUser]: Dados atualizados                                 │
        // │ 📤 OUTPUTS: 204 NoContent ou BadRequest                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] IdentityUser model)
        {
            try
            {
                // [DB] Atualizar usuário via extension
                var result = await _context.UpdateAsync(model, model.Id);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Update", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Delete                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Remove um usuário. Bloqueia exclusão do usuário principal.             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • model [IdentityUser]: Usuário a ser removido                            │
        // │ 📤 OUTPUTS: 204 NoContent ou BadRequest (se for usuário principal)                   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] IdentityUser model)
        {
            try
            {
                // [REGRA] Impedir exclusão do usuário principal
                if (model.UserName == _settings.Theme.Email)
                {
                    return BadRequest(SmartError.Failed("Please do not delete the main user! =)"));
                }

                // [DB] Deletar usuário via extension
                var result = await _context.DeleteAsync<IdentityUser>(model.Id);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Delete", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }
    }
}


