# EndPoints/UsersEndpoint.cs

**Mudanca:** GRANDE | **+34** linhas | **-46** linhas

---

```diff
--- JANEIRO: EndPoints/UsersEndpoint.cs
+++ ATUAL: EndPoints/UsersEndpoint.cs
@@ -8,136 +8,127 @@
 using FrotiX.Data;
 using FrotiX.Extensions;
 using FrotiX.Models;
-using FrotiX.Helpers;
 
 namespace FrotiX.EndPoints
-    {
+{
     [ApiController]
     [Route("api/users")]
     public class UsersEndpoint : ControllerBase
-        {
+    {
         private readonly ApplicationDbContext _context;
         private readonly UserManager<IdentityUser> _manager;
         private readonly SmartSettings _settings;
 
         public UsersEndpoint(ApplicationDbContext context, UserManager<IdentityUser> manager, SmartSettings settings)
-            {
+        {
             _context = context;
             _manager = manager;
             _settings = settings;
-            }
+        }
 
         [HttpGet]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<ActionResult<IEnumerable<IdentityUser>>> Get()
-            {
+        {
             try
             {
+
                 var users = await _manager.Users.AsNoTracking().ToListAsync();
 
                 return Ok(new { data = users, recordsTotal = users.Count, recordsFiltered = users.Count });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao buscar usuários" });
+                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
-            }
+        }
 
         [HttpGet("{id}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
-        public async Task<ActionResult<IdentityUser>> Get([FromRoute] string id)
-        {
-            try
-            {
-                return Ok(await _manager.FindByIdAsync(id));
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get(id)", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao buscar usuário" });
-            }
-        }
+        public async Task<ActionResult<IdentityUser>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));
 
         [HttpPost]
         [ProducesResponseType(StatusCodes.Status201Created)]
         public async Task<IActionResult> Create([FromForm] IdentityUser model)
-            {
+        {
             try
             {
+
                 model.Id = Guid.NewGuid().ToString();
                 model.UserName = model.Email;
 
                 var result = await _manager.CreateAsync(model);
 
                 if (result.Succeeded)
-                    {
+                {
 
                     result = await _manager.AddPasswordAsync(model, "Password123!");
 
                     if (result.Succeeded)
-                        {
+                    {
                         return CreatedAtAction("Get", new { id = model.Id }, model);
-                        }
                     }
+                }
 
                 return BadRequest(result);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Create", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao criar usuário" });
+                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Create", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
-            }
+        }
 
         [HttpPut]
         [ProducesResponseType(StatusCodes.Status204NoContent)]
         public async Task<IActionResult> Update([FromForm] IdentityUser model)
-            {
+        {
             try
             {
+
                 var result = await _context.UpdateAsync(model, model.Id);
 
                 if (result.Succeeded)
-                    {
+                {
                     return NoContent();
-                    }
+                }
 
                 return BadRequest(result);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Update", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao atualizar usuário" });
+                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Update", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
-            }
+        }
 
         [HttpDelete]
         [ProducesResponseType(StatusCodes.Status204NoContent)]
         public async Task<IActionResult> Delete([FromForm] IdentityUser model)
-            {
+        {
             try
             {
 
                 if (model.UserName == _settings.Theme.Email)
-                    {
+                {
                     return BadRequest(SmartError.Failed("Please do not delete the main user! =)"));
-                    }
+                }
 
                 var result = await _context.DeleteAsync<IdentityUser>(model.Id);
 
                 if (result.Succeeded)
-                    {
+                {
                     return NoContent();
-                    }
+                }
 
                 return BadRequest(result);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Delete", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao deletar usuário" });
-            }
+                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Delete", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
         }
     }
+}
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
    {
        {
            {
            }
            {
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get", ex);
                return StatusCode(500, new { success = false, message = "Erro ao buscar usuários" });
            }
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
            {
                    {
                        {
                        }
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Create", ex);
                return StatusCode(500, new { success = false, message = "Erro ao criar usuário" });
            }
            {
                    {
                    }
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Update", ex);
                return StatusCode(500, new { success = false, message = "Erro ao atualizar usuário" });
            }
            {
                    {
                    }
                    {
                    }
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Delete", ex);
                return StatusCode(500, new { success = false, message = "Erro ao deletar usuário" });
            }
```


### ADICIONAR ao Janeiro

```csharp
{
    {
        {
        }
        {
            catch (Exception error)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Get", error);
                return StatusCode(500, new { success = false, message = error.Message });
        }
        public async Task<ActionResult<IdentityUser>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));
        {
                {
                    {
                }
            catch (Exception error)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Create", error);
                return StatusCode(500, new { success = false, message = error.Message });
        }
        {
                {
                }
            catch (Exception error)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Update", error);
                return StatusCode(500, new { success = false, message = error.Message });
        }
        {
                {
                }
                {
                }
            catch (Exception error)
                Alerta.TratamentoErroComLinha("UsersEndpoint.cs", "Delete", error);
                return StatusCode(500, new { success = false, message = error.Message });
}
```
