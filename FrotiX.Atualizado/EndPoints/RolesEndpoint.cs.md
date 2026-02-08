# EndPoints/RolesEndpoint.cs

**Mudanca:** GRANDE | **+33** linhas | **-45** linhas

---

```diff
--- JANEIRO: EndPoints/RolesEndpoint.cs
+++ ATUAL: EndPoints/RolesEndpoint.cs
@@ -6,128 +6,119 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.EntityFrameworkCore;
 using FrotiX.Models;
-using FrotiX.Helpers;
 
 namespace FrotiX.EndPoints
-    {
+{
     [ApiController]
     [Route("api/roles")]
     public class RolesEndpoint : ControllerBase
-        {
+    {
         private readonly RoleManager<IdentityRole> _manager;
         private readonly SmartSettings _settings;
 
         public RolesEndpoint(RoleManager<IdentityRole> manager, SmartSettings settings)
-            {
+        {
             _manager = manager;
             _settings = settings;
-            }
+        }
 
         [HttpGet]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<ActionResult<IEnumerable<IdentityRole>>> Get()
-            {
+        {
             try
             {
+
                 var roles = await _manager.Roles.AsNoTracking().ToListAsync();
 
                 return Ok(new { data = roles, recordsTotal = roles.Count, recordsFiltered = roles.Count });
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao buscar roles" });
+                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
-            }
+        }
 
         [HttpGet("{id}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
-        public async Task<ActionResult<IdentityRole>> Get([FromRoute] string id)
-        {
-            try
-            {
-                return Ok(await _manager.FindByIdAsync(id));
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get(id)", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao buscar role" });
-            }
-        }
+        public async Task<ActionResult<IdentityRole>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));
 
         [HttpPost]
         [ProducesResponseType(StatusCodes.Status201Created)]
         public async Task<IActionResult> Create([FromForm] IdentityRole model)
-            {
+        {
             try
             {
+
                 model.Id = Guid.NewGuid().ToString();
                 model.ConcurrencyStamp = Guid.NewGuid().ToString();
 
                 var result = await _manager.CreateAsync(model);
 
                 if (result.Succeeded)
-                    {
+                {
                     return CreatedAtAction("Get", new { id = model.Id }, model);
-                    }
+                }
 
                 return BadRequest(result);
             }
-            catch (Exception ex)
+            catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Create", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao criar role" });
+                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Create", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
-            }
+        }
 
         [HttpPut]
         [ProducesResponseType(StatusCodes.Status204NoContent)]
         public async Task<IActionResult> Update([FromForm] IdentityRole model)
-            {
+        {
             try
             {
+
                 var result = await _manager.UpdateAsync(model);
 
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
-                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Update", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao atualizar role" });
+                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Update", error);
+                return StatusCode(500, new { success = false, message = error.Message });
             }
-            }
+        }
 
         [HttpDelete]
         [ProducesResponseType(StatusCodes.Status204NoContent)]
         public async Task<IActionResult> Delete([FromForm] IdentityRole model)
-            {
+        {
             try
             {
 
                 if (model.Name == _settings.Theme.Role)
-                    {
+                {
                     return BadRequest(SmartError.Failed("Please do not delete the default role! =)"));
-                    }
+                }
 
                 var result = await _manager.DeleteAsync(model);
 
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
-                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Delete", ex);
-                return StatusCode(500, new { success = false, message = "Erro ao deletar role" });
-            }
+                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Delete", error);
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
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get", ex);
                return StatusCode(500, new { success = false, message = "Erro ao buscar roles" });
            }
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
            {
                    {
                    }
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Create", ex);
                return StatusCode(500, new { success = false, message = "Erro ao criar role" });
            }
            {
                    {
                    }
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Update", ex);
                return StatusCode(500, new { success = false, message = "Erro ao atualizar role" });
            }
            {
                    {
                    }
                    {
                    }
            catch (Exception ex)
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Delete", ex);
                return StatusCode(500, new { success = false, message = "Erro ao deletar role" });
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
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Get", error);
                return StatusCode(500, new { success = false, message = error.Message });
        }
        public async Task<ActionResult<IdentityRole>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));
        {
                {
                }
            catch (Exception error)
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Create", error);
                return StatusCode(500, new { success = false, message = error.Message });
        }
        {
                {
                }
            catch (Exception error)
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Update", error);
                return StatusCode(500, new { success = false, message = error.Message });
        }
        {
                {
                }
                {
                }
            catch (Exception error)
                Alerta.TratamentoErroComLinha("RolesEndpoint.cs", "Delete", error);
                return StatusCode(500, new { success = false, message = error.Message });
}
```
