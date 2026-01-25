/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - RENDERIZAÇÃO RAZOR PARA STRINGS                                         #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

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
    public interface IRazorRenderService
    {
        Task<string> ToStringAsync<T>(string viewName, T model);
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: RazorRenderService                                                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Serviço para renderizar Views/Pages Razor como strings HTML fora do      ║
    /// ║    contexto de requisição HTTP normal. Útil para gerar e-mails, PDFs, etc.   ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Permite gerar HTML dinâmico a partir de templates Razor para envio de    ║
    /// ║    e-mails formatados, geração de relatórios e outros cenários.             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • ToStringAsync() → Renderiza Razor Page como string HTML                 ║
    /// ║    • FindPage() → Localiza Razor Page no sistema de arquivos                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Serviço de infraestrutura                               ║
    /// ║    • Arquivos relacionados: MailService, Pages/Email/*.cshtml               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
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

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ToStringAsync                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza uma Razor Page completa como string HTML com modelo tipado.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • pageName: Nome da página (ex: "/Pages/Email/Welcome")                   ║
        /// ║    • model: Modelo tipado para a página                                      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task<string>: HTML renderizado                                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
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


