# Program.cs

**Mudanca:** MEDIA | **+14** linhas | **-2** linhas

---

```diff
--- JANEIRO: Program.cs
+++ ATUAL: Program.cs
@@ -1,7 +1,6 @@
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.Extensions.Hosting;
 using Microsoft.Extensions.DependencyInjection;
-using FrotiX.Helpers;
 using FrotiX.Services;
 using System;
 using System.IO;
@@ -15,9 +14,15 @@
             try
             {
 
-                var host = CreateHostBuilder(args).Build();
+                Console.WriteLine("[DIAG-PROG] Antes CreateHostBuilder...");
+                var hostBuilder = CreateHostBuilder(args);
+                Console.WriteLine("[DIAG-PROG] Antes Build...");
+                var host = hostBuilder.Build();
+                Console.WriteLine("[DIAG-PROG] Apos Build...");
 
+                Console.WriteLine("[DIAG-PROG] Antes ConfigureGlobalExceptionHandlers...");
                 ConfigureGlobalExceptionHandlers(host.Services);
+                Console.WriteLine("[DIAG-PROG] Antes Run...");
 
                 host.Run();
             }
@@ -113,9 +118,19 @@
         {
             try
             {
+
+                var isWsl = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
+                    System.Runtime.InteropServices.OSPlatform.Linux) &&
+                    Environment.CurrentDirectory.StartsWith("/mnt/");
+
                 return Host.CreateDefaultBuilder(args)
                     .ConfigureWebHostDefaults(webBuilder =>
                     {
+
+                        if (isWsl)
+                        {
+                            webBuilder.UseSetting(WebHostDefaults.StaticWebAssetsKey, "false");
+                        }
                         webBuilder.UseStartup<Startup>();
                     });
             }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
                var host = CreateHostBuilder(args).Build();
```


### ADICIONAR ao Janeiro

```csharp
                Console.WriteLine("[DIAG-PROG] Antes CreateHostBuilder...");
                var hostBuilder = CreateHostBuilder(args);
                Console.WriteLine("[DIAG-PROG] Antes Build...");
                var host = hostBuilder.Build();
                Console.WriteLine("[DIAG-PROG] Apos Build...");
                Console.WriteLine("[DIAG-PROG] Antes ConfigureGlobalExceptionHandlers...");
                Console.WriteLine("[DIAG-PROG] Antes Run...");
                var isWsl = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                    System.Runtime.InteropServices.OSPlatform.Linux) &&
                    Environment.CurrentDirectory.StartsWith("/mnt/");
                        if (isWsl)
                        {
                            webBuilder.UseSetting(WebHostDefaults.StaticWebAssetsKey, "false");
                        }
```
