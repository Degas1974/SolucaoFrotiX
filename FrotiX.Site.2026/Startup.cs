using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using FrotiX.Cache;
using FrotiX.Data;
using FrotiX.Filters;
using FrotiX.Hubs;
using FrotiX.Logging;
using FrotiX.Middlewares;
using FrotiX.Models;
using FrotiX.Repository;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using FrotiX.Services.WhatsApp;
using FrotiX.Settings;
using FrotiX.TextNormalization.Extensions;
using FrotiX.Services.DocGenerator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace FrotiX
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            try
            {
                Configuration = configuration;
                _environment = environment; // ⭐ ADICIONE ESTA LINHA

                // Define valores numéricos em Reais
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha(ex, "Startup.cs", ".ctor");
            }
        }

        public IConfiguration Configuration
        {
            get;
        }

        // Serviços / DI
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.Configure<FormOptions>(options =>
                {
                    options.MultipartBodyLengthLimit = 104857600; // 100MB
                    options.ValueLengthLimit = 104857600;
                    options.ValueCountLimit = 10000;
                });

                services.Configure<KestrelServerOptions>(options =>
                {
                    options.Limits.MaxRequestBodySize = 104857600; // 100MB
                });

                // IISServerOptions removido - não disponível em .NET 10 fora do contexto IIS
                // Se hospedando em IIS, configurar no web.config

                // Adicionar compressão
                services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });

                services.Configure<BrotliCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Optimal;
                });

                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Optimal;
                });

                // ⭐ CORS - Configuração melhorada para Telerik Reports e tratamento de erros
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders(
                                "Content-Disposition",    // Importante para downloads de PDF
                                "X-Request-Id",           // ID da requisição para rastreamento de erros
                                "X-Error-Details"         // Detalhes de erro para debug
                            ));
                });

                // Configuração do Telerik Reporting
                services.TryAddSingleton<IReportSourceResolver, CustomReportSourceResolver>();
                services.TryAddSingleton<IReportServiceConfiguration>(sp =>
                    new ReportServiceConfiguration
                    {
                        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
                        HostAppId = "FrotiXApp",
                        Storage = new FileStorage(),
                        ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>()
                    });

                // Habilitar IO síncrono (necessário para o Telerik Reporting)
                // Kestrel: AllowSynchronousIO
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });

                // Adicione temporariamente no Startup.cs para testar:
                var testConn = Configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Connection String: {testConn}");
                Console.WriteLine("[DIAG] Apos Connection String...");

                // Define cultura ANTES do Syncfusion
                services.Configure<RequestLocalizationOptions>(options =>
                {
                    var supportedCultures = new[] { "pt-BR" };
                    options.DefaultRequestCulture = new RequestCulture("pt-BR");
                    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                });

                Console.WriteLine("[DIAG] Antes AddMemoryCache...");
                // Cache em memória e hosted service de warmup
                services.AddMemoryCache();
                Console.WriteLine("[DIAG] Antes AddHostedService CacheWarmupService...");
                // [TEMP-DISABLED] services.AddHostedService<CacheWarmupService>();
                Console.WriteLine("[DIAG] Apos CacheWarmupService...");

                // ========================================================
                // ⭐ SISTEMA DE LOG DE ERROS - SERVIÇOS (v3.0 - Banco de Dados)
                // ========================================================
                // NOTA: ILogRepository é registrado APÓS FrotiXDbContext (linha ~250)
                // para garantir que o DbContext esteja disponível

                // IHttpContextAccessor precisa ser registrado antes do LogService
                services.AddHttpContextAccessor();

                // LogService - gravação primária no banco + fallback TXT
                services.AddSingleton<ILogService, LogService>();

                // Configura LoggerProvider customizado para capturar logs do ILogger (Debug Output)
                // [TEMP-DISABLED - causa travamento no Build()] 
                // services.AddSingleton<ILoggerProvider>(sp =>
                //     new FrotiXLoggerProvider(
                //         sp.GetRequiredService<ILogService>(),
                //         Microsoft.Extensions.Logging.LogLevel.Warning
                //     )
                // );
                services.AddLogging();

                // Filtros de exceção (devem ser Scoped para injeção de dependência)
                services.AddScoped<GlobalExceptionFilter>();
                services.AddScoped<AsyncExceptionFilter>();
                services.AddScoped<PageExceptionFilter>();
                services.AddScoped<AsyncPageExceptionFilter>();

                // Serviço de exportação de logs (PDF/Excel) - registrado após DbContext
                // Serviço de alertas de logs em tempo real (SignalR)
                // [TEMP-DISABLED] services.AddHostedService<LogErrosAlertService>();

                // Serviço de limpeza automática de logs antigos (> 90 dias)
                // [TEMP-DISABLED] services.AddHostedService<LogErrosCleanupService>();
                // ========================================================

                // ⭐ Controllers com Newtonsoft configurado corretamente
                services.AddControllers()
                    .AddNewtonsoftJson();

                // ⭐ SWAGGER - Configuração (apenas APIs)
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "FrotiX API",
                        Version = "v1",
                        Description = "API do Sistema de Gestão de Frotas - Câmara dos Deputados"
                    });

                    // Documentar APENAS endpoints que começam com /api
                    c.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        if (apiDesc.RelativePath == null) return false;
                        return apiDesc.RelativePath.StartsWith("api/", StringComparison.OrdinalIgnoreCase);
                    });

                    // Resolver conflitos de rota
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    // Usar FullName para evitar conflitos de tipo
                    c.CustomSchemaIds(type => type.FullName);
                });

                services.Configure<SmartSettings>(
                    Configuration.GetSection(SmartSettings.SectionName)
                );
                services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);

                services.Configure<FrotiX.Settings.RecorrenciaToggleSettings>(Configuration.GetSection("RecorrenciaToggle"));
                services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<FrotiX.Settings.RecorrenciaToggleSettings>>().Value);

                services.Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

                Console.WriteLine("[DIAG] Antes DbContext...");
                // ========================================================
                // CONFIGURAÇÕES DE BANCO DE DADOS E IDENTITY
                // ========================================================
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options
                        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

                // Registro único do FrotiXDbContext
                services.AddDbContext<FrotiXDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options
                        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

                // ⭐ Repositório de logs - DEVE ser registrado APÓS FrotiXDbContext
                services.AddScoped<FrotiX.Repository.IRepository.ILogRepository, FrotiX.Repository.LogRepository>();

                // Serviço de exportação de logs (PDF/Excel)
                services.AddScoped<ILogErrosExportService, LogErrosExportService>();

                // Serviço de análise de erros com Claude AI
                services.AddHttpClient("ClaudeAI");
                services.AddScoped<IClaudeAnalysisService, ClaudeAnalysisService>();

                Console.WriteLine("[DIAG] Antes AddIdentity...");
                services
                    .AddIdentity<IdentityUser, IdentityRole>(options =>
                        options.SignIn.RequireConfirmedAccount = false
                    )
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                Console.WriteLine("[DIAG] Apos AddIdentity...");

                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<ICorridasTaxiLegRepository, CorridasTaxiLegRepository>();
                Console.WriteLine("[DIAG] Apos UnitOfWork...");

                services.AddScoped<IViagemEstatisticaRepository, ViagemEstatisticaRepository>();
                services.AddScoped<ViagemEstatisticaService>();
                services.AddScoped<VeiculoEstatisticaService>();

                // Repositórios de Alertas
                services.AddScoped<IAlertasFrotiXRepository, AlertasFrotiXRepository>();
                services.AddScoped<IAlertasUsuarioRepository, AlertasUsuarioRepository>();

                services.Configure<IdentityOptions>(opts =>
                {
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                });

                services.AddTransient<IEmailSender, EmailSender>();
                Console.WriteLine("[DIAG] Antes AddControllersWithViews...");
                services.AddControllersWithViews(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));

                    // ⭐ Filtros globais de exceção para log de erros (Controllers)
                    options.Filters.Add<GlobalExceptionFilter>();
                    options.Filters.Add<AsyncExceptionFilter>();
                });
                Console.WriteLine("[DIAG] Apos AddControllersWithViews...");

                services
                    .AddRazorPages()
                    .AddRazorPagesOptions(options =>
                    {
                        // raiz "/" aponta para o dashboard
                        options.Conventions.AddPageRoute("/intel/analyticsdashboard", "");
                    })
                    .AddMvcOptions(options =>
                    {
                        options.MaxModelValidationErrors = 50;
                        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ =>
                            "O campo é obrigatório."
                        );
                        
                        // ⭐ Filtros de exceção também para Razor Pages via MvcOptions
                        options.Filters.Add<PageExceptionFilter>();
                        options.Filters.Add<AsyncPageExceptionFilter>();
                    });

                services.AddRazorPages().AddRazorRuntimeCompilation();
                Console.WriteLine("[DIAG] Apos RazorPages...");

                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = "FrotiX";
                    options.LoginPath = "/Identity/Account/LoginFrotiX";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromHours(10);
                    options.SlidingExpiration = true;
                });

                services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");

                services.AddNotyf(config =>
                {
                    config.DurationInSeconds = 10;
                    config.IsDismissable = true;
                    config.Position = NotyfPosition.TopRight;
                });

                services.AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromHours(10);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                });

                services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
                services.AddTransient<IMailService, MailService>();
                // AddHttpContextAccessor já registrado anteriormente (seção de logs)
                services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
                services.AddScoped<IRazorRenderService, RazorRenderService>();
                Console.WriteLine("[DIAG] Antes AddKendo 1...");
                services.AddKendo();
                Console.WriteLine("[DIAG] Apos AddKendo 1...");
                services.AddScoped<INavigationModel, NavigationModel>();
                services.AddScoped<IViagemRepository, ViagemRepository>();

                services.AddMemoryCache();
                services.AddScoped<MotoristaFotoService>();
                services.AddScoped<MotoristaCache>();
                services.AddScoped<FrotiX.Services.ListaCacheService>(); // Cache centralizado de Motoristas/Veículos
                services.AddScoped<IGlosaService, GlosaService>();

                services.AddScoped<IToastService, ToastService>();

                // >>> Normalizador de texto habilitado (JSON cache + Azure NER auto via env + fallback)
                Console.WriteLine("[DIAG] Antes AddTextNormalization...");
                services.AddTextNormalization();
                Console.WriteLine("[DIAG] Apos AddTextNormalization...");

                // >>> DOCGENERATOR - Registrar serviços do sistema de documentação automática
                Console.WriteLine("[DIAG] Antes AddDocGenerator...");
                services.AddDocGenerator(Configuration);
                Console.WriteLine("[DIAG] Apos AddDocGenerator...");

                services.AddRouting(options =>
                {
                    options.LowercaseUrls = true;  // URLs em lowercase
                    options.LowercaseQueryStrings = false;
                });

                // === Providers de TempData necessários para o Alerta gravar o payload ===
                services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
                services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
                // =======================================================================

                services.Configure<FormOptions>(options =>
                {
                    options.MultipartBodyLengthLimit = 104857600; // 100 MB
                    options.ValueLengthLimit = int.MaxValue;
                    options.MultipartHeadersLengthLimit = int.MaxValue;
                });

                Console.WriteLine("[DIAG] Antes AddSignalR...");
                // *** SignalR para o sistema de Alertas ***
                services.AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true; // Útil para debug
                    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                });
                Console.WriteLine("[DIAG] Apos AddSignalR...");

                services.Configure<EvolutionApiOptions>(Configuration.GetSection("WhatsApp"));

                services.AddHttpClient<IWhatsAppService, EvolutionApiWhatsAppService>((sp, client) =>
                {
                    var opts = sp.GetRequiredService<IOptions<EvolutionApiOptions>>().Value;
                    if (string.IsNullOrWhiteSpace(opts.BaseUrl))
                        throw new InvalidOperationException("WhatsApp.BaseUrl não configurado.");

                    client.BaseAddress = new Uri(opts.BaseUrl.TrimEnd('/'));
                    // Autorização: Evolution API com API Key em header 'apikey' (ou 'Authorization: Bearer')
                    if (!string.IsNullOrWhiteSpace(opts.ApiKey))
                    {
                        client.DefaultRequestHeaders.Remove("apikey");
                        client.DefaultRequestHeaders.Add("apikey", opts.ApiKey);
                    }
                    // Se seu provedor exigir Bearer:
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", opts.ApiKey);
                })
                // timeouts robustos para WhatsApp
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
                Console.WriteLine("[DIAG] FIM ConfigureServices");
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha(ex, "Startup.cs", "ConfigureServices");
            }
        }

        // Pipeline HTTP
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            try
            {
                Console.WriteLine("[DIAG] INICIO Configure...");
                // ========================================================
                // ⭐ SISTEMA DE LOG DE ERROS - MIDDLEWARE (PRIMEIRO!)
                // IMPORTANTE: Deve ser o PRIMEIRO middleware para capturar
                // todos os erros, inclusive os de outros middlewares
                // ========================================================
                app.UseErrorLogging();
                // ========================================================

                // Configurar página de erro
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

                // ⭐ SWAGGER - Middleware (habilitado em todos os ambientes)
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrotiX API v1");
                    c.RoutePrefix = "swagger";
                });

                var supportedCultures = new[]
                {
                    new CultureInfo("pt-BR"),
                };

                var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("pt-BR")
                .AddSupportedCultures(supportedCultures.Select(c => c.Name).ToArray())
                .AddSupportedUICultures(supportedCultures.Select(c => c.Name).ToArray());

                // IMPORTANTE: Configurar o AppToast
                AppToast.Configure(
                    app.ApplicationServices.GetRequiredService<IHttpContextAccessor>(),
                    app.ApplicationServices.GetRequiredService<ITempDataDictionaryFactory>()
                );
                app.UseRequestLocalization(localizationOptions);

                // Para capturar erros de status code (404, 401, etc)
                app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

                // Register Syncfusion license
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                    "Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX1eeHRURmNYWEByXUVWYEs="
                );

                // === Expor dependências para o Alerta (para gravar TempData e acessar HttpContext) ===
                Alerta.HttpCtx = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
                Alerta.TempFactory =
                    app.ApplicationServices.GetRequiredService<ITempDataDictionaryFactory>();
                Alerta.LoggerFactory = loggerFactory;
                // ⭐ Service Provider para Service Locator pattern (resolve ILogService em TratamentoErroComLinha)
                Alerta.ServiceProvider = app.ApplicationServices;
                // ================================================================================

                app.UseResponseCompression();

                // Middleware global de exceções (se já captura e registra, mantemos)
                app.UseMiddleware<UiExceptionMiddleware>();

                app.UseHttpsRedirection();

                app.UseStaticFiles();

                app.UseRouting();

                // ⭐ CORS - IMPORTANTE: deve vir DEPOIS de UseRouting e ANTES de UseAuthentication
                app.UseCors("CorsPolicy");

                app.UseAuthentication();
                app.UseAuthorization();
                app.UseSession();
                app.UseNotyf();

                app.UseEndpoints(endpoints =>
                {
                    // ⭐ Mapear controllers - isso inclui o ReportsController
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();

                    // *** Mapear o Hub do SignalR ***
                    endpoints.MapHub<AlertasHub>("/alertasHub");
                    endpoints.MapHub<FrotiX.Hubs.ImportacaoHub>("/hubs/importacao");
                    endpoints.MapHub<FrotiX.Hubs.DocGenerationHub>("/hubs/docgeneration");
                });

                // ========================================================
                // ⭐ LOG: Sistema de log inicializado
                // ========================================================
                var logService = app.ApplicationServices.GetService<ILogService>();
                logService?.Info("FrotiX Web inicializado com sucesso", "Startup.cs", "Configure");
                // ========================================================
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha(ex, "Startup.cs", "Configure");
            }
        }
    }
}
