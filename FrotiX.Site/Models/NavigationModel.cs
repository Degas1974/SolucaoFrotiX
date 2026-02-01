/* ****************************************************************************************
 * ⚡ ARQUIVO: NavigationModel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Construir navegação dinâmica baseada em nav.json e permissões do usuário.
 *
 * 📥 ENTRADAS     : IUnitOfWork, IHttpContextAccessor e arquivo nav.json.
 *
 * 📤 SAÍDAS       : SmartNavigation com itens filtrados por permissões.
 *
 * 🔗 CHAMADA POR  : Layouts e componentes de menu.
 *
 * 🔄 CHAMA        : NavigationBuilder, SmartNavigation, IUnitOfWork.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Repository.IRepository, Microsoft.AspNetCore.Http.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using FrotiX.Extensions;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ MODEL: NavigationModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Montar menus dinâmicos a partir de nav.json e permissões do usuário.
     *
     * 📥 ENTRADAS     : UnitOfWork, contexto HTTP e definição de navegação em JSON.
     *
     * 📤 SAÍDAS       : Estruturas de menu filtradas por acesso.
     *
     * 🔗 CHAMADA POR  : Layouts/Views que renderizam o menu.
     *
     * 🔄 CHAMA        : NavigationBuilder, SmartNavigation, IUnitOfWork.
     ****************************************************************************************/
    public class NavigationModel : INavigationModel
        {
        // Valor padrão de href vazio.
        public static readonly string Void = "javascript:void(0);";
        private const string Dash = "-";
        private const string Space = " ";
        private const string Underscore = "_";
        private static readonly string Empty = string.Empty;
        private static IUnitOfWork _currentUnitOfWork;
        private static IHttpContextAccessor _httpContextAccessor;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: NavigationModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar serviços necessários para construir a navegação.
         *
         * 📥 ENTRADAS     : currentUnitOfWork, httpContextAccessor.
         *
         * 📤 SAÍDAS       : Instância pronta para gerar menus.
         *
         * 🔗 CHAMADA POR  : DI/Startup.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
        public NavigationModel(
            IUnitOfWork currentUnitOfWork,
            IHttpContextAccessor httpContextAccessor
        )
            {
            _currentUnitOfWork = currentUnitOfWork;
            _httpContextAccessor = httpContextAccessor;
            }

        // Navegação completa (todos os itens).
        public SmartNavigation Full => BuildNavigation(seedOnly: false);
        // Navegação inicial (itens essenciais).
        public SmartNavigation Seed => BuildNavigation();

        /****************************************************************************************
         * ⚡ MÉTODO: BuildNavigation
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Construir navegação a partir do arquivo nav.json.
         *
         * 📥 ENTRADAS     : seedOnly (true para itens essenciais).
         *
         * 📤 SAÍDAS       : SmartNavigation com menus filtrados.
         *
         * 🔗 CHAMADA POR  : Propriedades Full e Seed.
         *
         * 🔄 CHAMA        : NavigationBuilder.FromJson, FillProperties.
         ****************************************************************************************/
        private static SmartNavigation BuildNavigation(bool seedOnly = true)
            {
            var jsonText = File.ReadAllText("nav.json");
            var navigation = NavigationBuilder.FromJson(jsonText);
            var menu = FillProperties(navigation.Lists, seedOnly);

            return new SmartNavigation(menu);
            }

        /****************************************************************************************
         * ⚡ MÉTODO: FillProperties
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Aplicar permissões e preencher propriedades dos itens de menu.
         *
         * 📥 ENTRADAS     : items, seedOnly e parent (opcional).
         *
         * 📤 SAÍDAS       : Lista de itens filtrados e enriquecidos.
         *
         * 🔗 CHAMADA POR  : BuildNavigation.
         *
         * 🔄 CHAMA        : IUnitOfWork.Recurso, IUnitOfWork.ControleAcesso, IsVoid.
         ****************************************************************************************/
        private static List<ListItem> FillProperties(
            IEnumerable<ListItem> items,
            bool seedOnly,
            ListItem parent = null
        )
            {
            var result = new List<ListItem>();

            //Pega o usuário corrente
            //=======================
            var userId = _httpContextAccessor
                .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            foreach (var item in items)
                {
                try
                    {
                    var ObjRecurso = _currentUnitOfWork.Recurso.GetFirstOrDefault(ca =>
                        ca.NomeMenu == item.NomeMenu
                    );

                    // CORREÇÃO: só segue se encontrou o recurso
                    if (ObjRecurso == null)
                        continue; // ou lance uma exception específica, se preferir

                    var recursoId = ObjRecurso.RecursoId;

                    var objControleAcesso = _currentUnitOfWork.ControleAcesso.GetFirstOrDefault(
                        ca => ca.UsuarioId == userId && ca.RecursoId == recursoId
                    );

                    // CORREÇÃO: Proteja acesso nulo do controle de acesso
                    if (objControleAcesso != null && objControleAcesso.Acesso)
                        {
                        item.Text ??= item.Title;
                        item.Tags = string.Concat(parent?.Tags, Space, item.Title.ToLower()).Trim();

                        var parentRoute = (
                            Path.GetFileNameWithoutExtension(parent?.Text ?? Empty)
                                ?.Replace(Space, Underscore) ?? Empty
                        ).ToLower();
                        var sanitizedHref =
                            parent == null
                                ? item.Href?.Replace(Dash, Empty)
                                : item
                                    .Href?.Replace(
                                        parentRoute,
                                        parentRoute.Replace(Underscore, Empty)
                                    )
                                    .Replace(Dash, Empty);
                        var route =
                            Path.GetFileNameWithoutExtension(sanitizedHref ?? Empty)
                                ?.Split(Underscore) ?? Array.Empty<string>();

                        item.Route =
                            route.Length > 1
                                ? $"/{route.First()}/{string.Join(Empty, route.Skip(1))}"
                                : item.Href;

                        item.I18n =
                            parent == null
                                ? $"nav.{item.Title.ToLower().Replace(Space, Underscore)}"
                                : $"{parent.I18n}_{item.Title.ToLower().Replace(Space, Underscore)}";
                        item.Type =
                            parent == null
                                ? item.Href == null
                                    ? ItemType.Category
                                    : ItemType.Single
                                : item.Items.Any()
                                    ? ItemType.Parent
                                    : ItemType.Child;
                        item.Items = FillProperties(item.Items, seedOnly, item);

                        if (item.Href.IsVoid() && item.Items.Any())
                            item.Type = ItemType.Sibling;

                        if (!seedOnly || item.ShowOnSeed)
                            result.Add(item);
                        }
                    }
                catch (Exception ex)
                    {
                    Console.WriteLine(ex.ToString());
                    throw;
                    }
                }

            return result;
            }
        }
    }
