# Startup.cs

**Mudanca:** GRANDE | **+63** linhas | **-115** linhas

---

```diff
--- JANEIRO: Startup.cs
+++ ATUAL: Startup.cs
@@ -4,6 +4,7 @@
 using FrotiX.Data;
 using FrotiX.Filters;
 using FrotiX.Hubs;
+using FrotiX.Logging;
 using FrotiX.Middlewares;
 using FrotiX.Models;
 using FrotiX.Repository;
@@ -37,7 +38,6 @@
 using System.Globalization;
 using System.IO.Compression;
 using System.Linq;
-using System.Runtime;
 using Telerik.Reporting.Cache.File;
 using Telerik.Reporting.Services;
 using Microsoft.AspNetCore.Server.Kestrel.Core;
@@ -55,8 +55,6 @@
                 Configuration = configuration;
                 _environment = environment;
 
-                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
-
                 CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
             }
             catch (Exception ex)
@@ -86,30 +84,22 @@
                     options.Limits.MaxRequestBodySize = 104857600;
                 });
 
-                services.Configure<IISServerOptions>(options =>
-                {
-                    options.MaxRequestBodySize = 104857600;
-                });
-
-                if (!_environment.IsDevelopment())
-                {
-                    services.AddResponseCompression(options =>
-                    {
-                        options.EnableForHttps = true;
-                        options.Providers.Add<BrotliCompressionProvider>();
-                        options.Providers.Add<GzipCompressionProvider>();
-                    });
-
-                    services.Configure<BrotliCompressionProviderOptions>(options =>
-                    {
-                        options.Level = CompressionLevel.Fastest;
-                    });
-
-                    services.Configure<GzipCompressionProviderOptions>(options =>
-                    {
-                        options.Level = CompressionLevel.Optimal;
-                    });
-                }
+                services.AddResponseCompression(options =>
+                {
+                    options.EnableForHttps = true;
+                    options.Providers.Add<BrotliCompressionProvider>();
+                    options.Providers.Add<GzipCompressionProvider>();
+                });
+
+                services.Configure<BrotliCompressionProviderOptions>(options =>
+                {
+                    options.Level = CompressionLevel.Optimal;
+                });
+
+                services.Configure<GzipCompressionProviderOptions>(options =>
+                {
+                    options.Level = CompressionLevel.Optimal;
+                });
 
                 services.AddCors(options =>
                 {
@@ -118,7 +108,11 @@
                             .AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader()
-                            .WithExposedHeaders("Content-Disposition"));
+                            .WithExposedHeaders(
+                                "Content-Disposition",
+                                "X-Request-Id",
+                                "X-Error-Details"
+                            ));
                 });
 
                 services.TryAddSingleton<IReportSourceResolver, CustomReportSourceResolver>();
@@ -131,13 +125,14 @@
                         ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>()
                     });
 
-                services.Configure<IISServerOptions>(options =>
+                services.Configure<KestrelServerOptions>(options =>
                 {
                     options.AllowSynchronousIO = true;
                 });
 
                 var testConn = Configuration.GetConnectionString("DefaultConnection");
                 Console.WriteLine($"Connection String: {testConn}");
+                Console.WriteLine("[DIAG] Apos Connection String...");
 
                 services.Configure<RequestLocalizationOptions>(options =>
                 {
@@ -147,13 +142,18 @@
                     options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                 });
 
-                services.AddMemoryCache(options =>
-                {
-                    options.SizeLimit = 1024;
-                });
-                services.AddHostedService<CacheWarmupService>();
+                Console.WriteLine("[DIAG] Antes AddMemoryCache...");
+
+                services.AddMemoryCache();
+                Console.WriteLine("[DIAG] Antes AddHostedService CacheWarmupService...");
+
+                Console.WriteLine("[DIAG] Apos CacheWarmupService...");
+
+                services.AddHttpContextAccessor();
 
                 services.AddSingleton<ILogService, LogService>();
+
+                services.AddLogging();
 
                 services.AddScoped<GlobalExceptionFilter>();
                 services.AddScoped<AsyncExceptionFilter>();
@@ -189,18 +189,8 @@
                 );
                 services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);
 
-                var forcarTextoRecorrenciaEnv = Environment.GetEnvironmentVariable("FROTIX_FORCAR_TEXTO_RECORRENCIA");
-                var forcarDatePickerRecorrenciaEnv = Environment.GetEnvironmentVariable("FROTIX_FORCAR_DATEPICKER_RECORRENCIA");
-                var mostrarToggleDevEnv = Environment.GetEnvironmentVariable("FROTIX_MOSTRAR_TOGGLE_RECORRENCIA_DEV");
-
-                var recorrenciaToggleSettings = new RecorrenciaToggleSettings
-                {
-                    ForcarTextoRecorrencia = ParseFlagEnv(forcarTextoRecorrenciaEnv),
-                    ForcarDatePickerRecorrencia = ParseFlagEnv(forcarDatePickerRecorrenciaEnv),
-                    MostrarToggleDev = ParseFlagEnv(mostrarToggleDevEnv)
-                };
-
-                services.AddSingleton(recorrenciaToggleSettings);
+                services.Configure<FrotiX.Settings.RecorrenciaToggleSettings>(Configuration.GetSection("RecorrenciaToggle"));
+                services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<FrotiX.Settings.RecorrenciaToggleSettings>>().Value);
 
                 services.Configure<CookiePolicyOptions>(options =>
                 {
@@ -208,67 +198,42 @@
                     options.MinimumSameSitePolicy = SameSiteMode.None;
                 });
 
-                var enableSensitiveLogging = Configuration.GetValue<bool>(
-                    "EfCore:EnableSensitiveDataLogging",
-                    false
-                );
+                Console.WriteLine("[DIAG] Antes DbContext...");
+
                 services.AddDbContext<ApplicationDbContext>(options =>
                 {
-                    options.UseSqlServer(
-                        Configuration.GetConnectionString("DefaultConnection"),
-                        sql =>
-                        {
-
-                            sql.EnableRetryOnFailure(
-                                maxRetryCount: 5,
-                                maxRetryDelay: TimeSpan.FromSeconds(10),
-                                errorNumbersToAdd: null
-                            );
-                            sql.CommandTimeout(30);
-                        }
-                    );
-
-                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
-
-                    options.EnableSensitiveDataLogging(enableSensitiveLogging);
-                    options.EnableDetailedErrors(_environment.IsDevelopment());
-                },
-                ServiceLifetime.Scoped,
-                ServiceLifetime.Scoped);
+                    options
+                        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
+                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
+                });
 
                 services.AddDbContext<FrotiXDbContext>(options =>
                 {
-                    options.UseSqlServer(
-                        Configuration.GetConnectionString("DefaultConnection"),
-                        sql =>
-                        {
-
-                            sql.EnableRetryOnFailure(
-                                maxRetryCount: 5,
-                                maxRetryDelay: TimeSpan.FromSeconds(10),
-                                errorNumbersToAdd: null
-                            );
-                            sql.CommandTimeout(30);
-                        }
-                    );
-
-                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
-
-                    options.EnableSensitiveDataLogging(enableSensitiveLogging);
-                    options.EnableDetailedErrors(_environment.IsDevelopment());
-                },
-                ServiceLifetime.Scoped,
-                ServiceLifetime.Scoped);
-
+                    options.EnableSensitiveDataLogging();
+                    options
+                        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
+                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
+                });
+
+                services.AddScoped<FrotiX.Repository.IRepository.ILogRepository, FrotiX.Repository.LogRepository>();
+
+                services.AddScoped<ILogErrosExportService, LogErrosExportService>();
+
+                services.AddHttpClient("ClaudeAI");
+                services.AddScoped<IClaudeAnalysisService, ClaudeAnalysisService>();
+
+                Console.WriteLine("[DIAG] Antes AddIdentity...");
                 services
                     .AddIdentity<IdentityUser, IdentityRole>(options =>
                         options.SignIn.RequireConfirmedAccount = false
                     )
                     .AddRoleManager<RoleManager<IdentityRole>>()
                     .AddEntityFrameworkStores<ApplicationDbContext>();
+                Console.WriteLine("[DIAG] Apos AddIdentity...");
 
                 services.AddScoped<IUnitOfWork, UnitOfWork>();
                 services.AddScoped<ICorridasTaxiLegRepository, CorridasTaxiLegRepository>();
+                Console.WriteLine("[DIAG] Apos UnitOfWork...");
 
                 services.AddScoped<IViagemEstatisticaRepository, ViagemEstatisticaRepository>();
                 services.AddScoped<ViagemEstatisticaService>();
@@ -286,7 +251,7 @@
                 });
 
                 services.AddTransient<IEmailSender, EmailSender>();
-
+                Console.WriteLine("[DIAG] Antes AddControllersWithViews...");
                 services.AddControllersWithViews(options =>
                 {
                     var policy = new AuthorizationPolicyBuilder()
@@ -297,6 +262,7 @@
                     options.Filters.Add<GlobalExceptionFilter>();
                     options.Filters.Add<AsyncExceptionFilter>();
                 });
+                Console.WriteLine("[DIAG] Apos AddControllersWithViews...");
 
                 services
                     .AddRazorPages()
@@ -317,6 +283,7 @@
                     });
 
                 services.AddRazorPages().AddRazorRuntimeCompilation();
+                Console.WriteLine("[DIAG] Apos RazorPages...");
 
                 services.ConfigureApplicationCookie(options =>
                 {
@@ -342,28 +309,34 @@
                     options.IdleTimeout = TimeSpan.FromHours(10);
                     options.Cookie.HttpOnly = true;
                     options.Cookie.IsEssential = true;
-                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                 });
 
                 services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
                 services.AddTransient<IMailService, MailService>();
-                services.AddHttpContextAccessor();
+
                 services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
                 services.AddScoped<IRazorRenderService, RazorRenderService>();
+                Console.WriteLine("[DIAG] Antes AddKendo 1...");
                 services.AddKendo();
+                Console.WriteLine("[DIAG] Apos AddKendo 1...");
                 services.AddScoped<INavigationModel, NavigationModel>();
                 services.AddScoped<IViagemRepository, ViagemRepository>();
 
                 services.AddMemoryCache();
                 services.AddScoped<MotoristaFotoService>();
                 services.AddScoped<MotoristaCache>();
+                services.AddScoped<FrotiX.Services.ListaCacheService>();
                 services.AddScoped<IGlosaService, GlosaService>();
 
                 services.AddScoped<IToastService, ToastService>();
 
+                Console.WriteLine("[DIAG] Antes AddTextNormalization...");
                 services.AddTextNormalization();
-
+                Console.WriteLine("[DIAG] Apos AddTextNormalization...");
+
+                Console.WriteLine("[DIAG] Antes AddDocGenerator...");
                 services.AddDocGenerator(Configuration);
+                Console.WriteLine("[DIAG] Apos AddDocGenerator...");
 
                 services.AddRouting(options =>
                 {
@@ -381,14 +354,15 @@
                     options.MultipartHeadersLengthLimit = int.MaxValue;
                 });
 
+                Console.WriteLine("[DIAG] Antes AddSignalR...");
+
                 services.AddSignalR(options =>
                 {
                     options.EnableDetailedErrors = true;
                     options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                     options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                 });
-
-                services.AddKendo();
+                Console.WriteLine("[DIAG] Apos AddSignalR...");
 
                 services.Configure<EvolutionApiOptions>(Configuration.GetSection("WhatsApp"));
 
@@ -409,6 +383,7 @@
                 })
 
                 .SetHandlerLifetime(TimeSpan.FromMinutes(5));
+                Console.WriteLine("[DIAG] FIM ConfigureServices");
             }
             catch (Exception ex)
             {
@@ -423,6 +398,7 @@
         {
             try
             {
+                Console.WriteLine("[DIAG] INICIO Configure...");
 
                 app.UseErrorLogging();
 
@@ -470,20 +446,9 @@
                     app.ApplicationServices.GetRequiredService<ITempDataDictionaryFactory>();
                 Alerta.LoggerFactory = loggerFactory;
 
-                if (!_environment.IsDevelopment())
-                {
-                    app.UseResponseCompression();
-                }
-
-                app.Use(async (context, next) =>
-                {
-                    await next();
-
-                    if (Random.Shared.Next(50) == 0)
-                    {
-                        GC.Collect(2, GCCollectionMode.Optimized, false);
-                    }
-                });
+                Alerta.ServiceProvider = app.ApplicationServices;
+
+                app.UseResponseCompression();
 
                 app.UseMiddleware<UiExceptionMiddleware>();
 
@@ -520,28 +485,5 @@
                 Alerta.TratamentoErroComLinha(ex, "Startup.cs", "Configure");
             }
         }
-
-        private static bool ParseFlagEnv(string value)
-        {
-            try
-            {
-                if (string.IsNullOrWhiteSpace(value))
-                {
-                    return false;
-                }
-
-                if (value.Equals("1", StringComparison.OrdinalIgnoreCase))
-                {
-                    return true;
-                }
-
-                return bool.TryParse(value, out var parsed) && parsed;
-            }
-            catch (Exception ex)
-            {
-                Alerta.TratamentoErroComLinha(ex, "Startup.cs", "ParseFlagEnv");
-                return false;
-            }
-        }
     }
 }
```
