// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: RazorRenderService.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Serviço para renderização de Razor Views para string HTML.                   ║
// ║ Útil para geração de e-mails HTML, PDFs, ou retorno de HTML via API.         ║
// ║                                                                              ║
// ║ INTERFACE IRazorRenderService:                                               ║
// ║ - ToStringAsync<T>(): Renderiza view com modelo para string                  ║
// ║                                                                              ║
// ║ DEPENDÊNCIAS:                                                                ║
// ║ - IRazorViewEngine: Engine de views Razor                                    ║
// ║ - ITempDataProvider: Provider de TempData                                    ║
// ║ - IHttpContextAccessor: Acesso ao contexto HTTP                              ║
// ║ - IRazorPageActivator: Ativador de páginas Razor                             ║
// ║                                                                              ║
// ║ USO TÍPICO:                                                                  ║
// ║ - Geração de corpo de e-mail a partir de template Razor                      ║
// ║ - Geração de HTML para conversão em PDF                                      ║
// ║ - Retorno de partial views via AJAX                                          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 15                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FrotiX.Services
    {
    /// <summary>
    /// Interface para renderização de Razor Views para string.
    /// </summary>
    public interface IRazorRenderService
        {
        Task<string> ToStringAsync<T>(string viewName, T model);
        }

    /// <summary>
    /// Serviço para renderização de Razor Views para string HTML.
    /// </summary>
    public class RazorRenderService : IRazorRenderService
        {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;
        private readonly IRazorPageActivator _activator;
        public RazorRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContext,
            IRazorPageActivator activator,
            IActionContextAccessor actionContext)
            {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
            _actionContext = actionContext;
            _activator = activator;
            }
        public async Task<string> ToStringAsync<T>(string pageName, T model)
            {
            var actionContext =
                new ActionContext(
                    _httpContext.HttpContext,
                    _httpContext.HttpContext.GetRouteData(),
                    _actionContext.ActionContext.ActionDescriptor
                );
            using (var sw = new StringWriter())
                {
                var result = _razorViewEngine.FindPage(actionContext, pageName);
                if (result.Page == null)
                    {
                    throw new ArgumentNullException($"The page {pageName} cannot be found.");
                    }
                var view = new RazorView(_razorViewEngine,
                    _activator,
                    new List<IRazorPage>(),
                    result.Page,
                    HtmlEncoder.Default,
                    new DiagnosticListener("RazorRenderService"));
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                        {
                        Model = model
                        },
                    new TempDataDictionary(
                        _httpContext.HttpContext,
                        _tempDataProvider
                    ),
                    sw,
                    new HtmlHelperOptions()
                );
                var page = (result.Page);
                page.ViewContext = viewContext;
                _activator.Activate(page, viewContext);
                await page.ExecuteAsync();
                return sw.ToString();
                }
            }
        private IRazorPage FindPage(ActionContext actionContext, string pageName)
            {
            var getPageResult = _razorViewEngine.GetPage(executingFilePath: null, pagePath: pageName);
            if (getPageResult.Page != null)
                {
                return getPageResult.Page;
                }
            var findPageResult = _razorViewEngine.FindPage(actionContext, pageName);
            if (findPageResult.Page != null)
                {
                return findPageResult.Page;
                }
            var searchedLocations = getPageResult.SearchedLocations.Concat(findPageResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find page '{pageName}'. The following locations were searched:" }.Concat(searchedLocations));
            throw new InvalidOperationException(errorMessage);
            }
        }
    }


