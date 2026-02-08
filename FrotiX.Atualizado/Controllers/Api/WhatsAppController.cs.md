# Controllers/Api/WhatsAppController.cs

**Mudanca:** GRANDE | **+29** linhas | **-43** linhas

---

```diff
--- JANEIRO: Controllers/Api/WhatsAppController.cs
+++ ATUAL: Controllers/Api/WhatsAppController.cs
@@ -1,12 +1,9 @@
 using FrotiX.Services.WhatsApp;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
-using Microsoft.Extensions.Logging;
 using System;
 using System.Threading;
 using System.Threading.Tasks;
-using FrotiX.Helpers;
-using FrotiX.Services;
 
 namespace FrotiX.Controllers.Api
 {
@@ -17,108 +14,91 @@
     public class WhatsAppController : ControllerBase
     {
         private readonly IWhatsAppService _wa;
-        private readonly ILogger<WhatsAppController> _logger;
-        private readonly ILogService _log;
 
-        public WhatsAppController(IWhatsAppService wa, ILogger<WhatsAppController> logger, ILogService log)
+        public WhatsAppController(IWhatsAppService wa)
+        {
+            _wa = wa;
+        }
+
+        [HttpPost("start")]
+        public async Task<IActionResult> Start([FromBody] StartSessionRequest req , CancellationToken ct)
         {
             try
             {
-                _wa = wa;
-                _logger = logger;
-                _log = log;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Constructor", ex);
-            }
-        }
-
-        [HttpPost("start")]
-        public async Task<IActionResult> Start([FromBody] StartSessionRequest req, CancellationToken ct)
-        {
-            try
-            {
-
                 var session = string.IsNullOrWhiteSpace(req?.Session) ? null : req.Session.Trim();
-
-                var r = await _wa.StartSessionAsync(session, ct);
+                var r = await _wa.StartSessionAsync(session , ct);
                 return Ok(r);
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Start", ex);
-                return BadRequest(new { success = false, message = ex.Message });
+                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Start" , ex);
+                return BadRequest(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet("status")]
-        public async Task<IActionResult> Status([FromQuery] string session, CancellationToken ct)
+        public async Task<IActionResult> Status([FromQuery] string session , CancellationToken ct)
         {
             try
             {
-
-                var r = await _wa.GetStatusAsync(session, ct);
+                var r = await _wa.GetStatusAsync(session , ct);
                 return Ok(r);
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Status", ex);
-                return BadRequest(new { success = false, message = ex.Message });
+                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Status" , ex);
+                return BadRequest(new { success = false , message = ex.Message });
             }
         }
 
         [HttpGet("qr")]
-        public async Task<IActionResult> Qr([FromQuery] string session, CancellationToken ct)
+        public async Task<IActionResult> Qr([FromQuery] string session , CancellationToken ct)
         {
             try
             {
+                var b64 = await _wa.GetQrBase64Async(session , ct);
+                if (string.IsNullOrWhiteSpace(b64))
+                    return NotFound(new { success = false , message = "QR não disponível." });
 
-                var b64 = await _wa.GetQrBase64Async(session, ct);
-                if (string.IsNullOrWhiteSpace(b64))
-                    return NotFound(new { success = false, message = "QR não disponível." });
-
-                if (!b64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
+                if (!b64.StartsWith("data:" , StringComparison.OrdinalIgnoreCase))
                     b64 = "data:image/png;base64," + b64;
 
-                return Ok(new { success = true, qrcode = b64 });
+                return Ok(new { success = true , qrcode = b64 });
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Qr", ex);
-                return BadRequest(new { success = false, message = ex.Message });
+                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Qr" , ex);
+                return BadRequest(new { success = false , message = ex.Message });
             }
         }
 
         [HttpPost("send-text")]
-        public async Task<IActionResult> SendText([FromBody] SendTextRequest req, CancellationToken ct)
+        public async Task<IActionResult> SendText([FromBody] SendTextRequest req , CancellationToken ct)
         {
             try
             {
-
-                var r = await _wa.SendTextAsync(req.Session, req.PhoneE164, req.Message, ct);
+                var r = await _wa.SendTextAsync(req.Session , req.PhoneE164 , req.Message , ct);
                 return Ok(r);
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "SendText", ex);
-                return BadRequest(new { success = false, message = ex.Message });
+                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendText" , ex);
+                return BadRequest(new { success = false , message = ex.Message });
             }
         }
 
         [HttpPost("send-media")]
-        public async Task<IActionResult> SendMedia([FromBody] SendMediaRequest req, CancellationToken ct)
+        public async Task<IActionResult> SendMedia([FromBody] SendMediaRequest req , CancellationToken ct)
         {
             try
             {
-
-                var r = await _wa.SendMediaAsync(req.Session, req.PhoneE164, req.FileName, req.Base64Data, req.Caption, ct);
+                var r = await _wa.SendMediaAsync(req.Session , req.PhoneE164 , req.FileName , req.Base64Data , req.Caption , ct);
                 return Ok(r);
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "SendMedia", ex);
-                return BadRequest(new { success = false, message = ex.Message });
+                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendMedia" , ex);
+                return BadRequest(new { success = false , message = ex.Message });
             }
         }
     }
```

### REMOVER do Janeiro

```csharp
using Microsoft.Extensions.Logging;
using FrotiX.Helpers;
using FrotiX.Services;
        private readonly ILogger<WhatsAppController> _logger;
        private readonly ILogService _log;
        public WhatsAppController(IWhatsAppService wa, ILogger<WhatsAppController> logger, ILogService log)
                _wa = wa;
                _logger = logger;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Constructor", ex);
            }
        }
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartSessionRequest req, CancellationToken ct)
        {
            try
            {
                var r = await _wa.StartSessionAsync(session, ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Start", ex);
                return BadRequest(new { success = false, message = ex.Message });
        public async Task<IActionResult> Status([FromQuery] string session, CancellationToken ct)
                var r = await _wa.GetStatusAsync(session, ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Status", ex);
                return BadRequest(new { success = false, message = ex.Message });
        public async Task<IActionResult> Qr([FromQuery] string session, CancellationToken ct)
                var b64 = await _wa.GetQrBase64Async(session, ct);
                if (string.IsNullOrWhiteSpace(b64))
                    return NotFound(new { success = false, message = "QR não disponível." });
                if (!b64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                return Ok(new { success = true, qrcode = b64 });
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Qr", ex);
                return BadRequest(new { success = false, message = ex.Message });
        public async Task<IActionResult> SendText([FromBody] SendTextRequest req, CancellationToken ct)
                var r = await _wa.SendTextAsync(req.Session, req.PhoneE164, req.Message, ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "SendText", ex);
                return BadRequest(new { success = false, message = ex.Message });
        public async Task<IActionResult> SendMedia([FromBody] SendMediaRequest req, CancellationToken ct)
                var r = await _wa.SendMediaAsync(req.Session, req.PhoneE164, req.FileName, req.Base64Data, req.Caption, ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "SendMedia", ex);
                return BadRequest(new { success = false, message = ex.Message });
```


### ADICIONAR ao Janeiro

```csharp
        public WhatsAppController(IWhatsAppService wa)
        {
            _wa = wa;
        }
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartSessionRequest req , CancellationToken ct)
                var r = await _wa.StartSessionAsync(session , ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Start" , ex);
                return BadRequest(new { success = false , message = ex.Message });
        public async Task<IActionResult> Status([FromQuery] string session , CancellationToken ct)
                var r = await _wa.GetStatusAsync(session , ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Status" , ex);
                return BadRequest(new { success = false , message = ex.Message });
        public async Task<IActionResult> Qr([FromQuery] string session , CancellationToken ct)
                var b64 = await _wa.GetQrBase64Async(session , ct);
                if (string.IsNullOrWhiteSpace(b64))
                    return NotFound(new { success = false , message = "QR não disponível." });
                if (!b64.StartsWith("data:" , StringComparison.OrdinalIgnoreCase))
                return Ok(new { success = true , qrcode = b64 });
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Qr" , ex);
                return BadRequest(new { success = false , message = ex.Message });
        public async Task<IActionResult> SendText([FromBody] SendTextRequest req , CancellationToken ct)
                var r = await _wa.SendTextAsync(req.Session , req.PhoneE164 , req.Message , ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendText" , ex);
                return BadRequest(new { success = false , message = ex.Message });
        public async Task<IActionResult> SendMedia([FromBody] SendMediaRequest req , CancellationToken ct)
                var r = await _wa.SendMediaAsync(req.Session , req.PhoneE164 , req.FileName , req.Base64Data , req.Caption , ct);
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendMedia" , ex);
                return BadRequest(new { success = false , message = ex.Message });
```
