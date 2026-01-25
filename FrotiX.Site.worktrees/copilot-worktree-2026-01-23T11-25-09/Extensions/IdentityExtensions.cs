/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Métodos de extensão para manipulação de Identity, Claims e persistência básica.
 * =========================================================================================
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using FrotiX.Data;
using FrotiX.Models;
using System; // Adicionado para Exception
using FrotiX.Helpers;

namespace FrotiX.Extensions
    {
    public static class IdentityExtensions
        {
        [DebuggerStepThrough]
        private static bool HasRole(this ClaimsPrincipal principal, params string[] roles)
            {
            if (principal == null)
                return default;

            var claims = principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToSafeList();

            return claims?.Any() == true && claims.Intersect(roles ?? new string[] { }).Any();
            }

        [DebuggerStepThrough]
        public static IEnumerable<ListItem> AuthorizeFor(this IEnumerable<ListItem> source, ClaimsPrincipal identity)
            => source.Where(x => x.Roles.IsNullOrEmpty() || (x.Roles.HasItems() && identity.HasRole(x.Roles))).ToSafeList();

        [DebuggerStepThrough]
        public static HtmlString AsRaw(this string value) => new HtmlString(value);

        [DebuggerStepThrough]
        public static string ToPage(this string href) => System.IO.Path.GetFileNameWithoutExtension(href)?.ToLower();

        [DebuggerStepThrough]
        public static bool IsVoid(this string href) => href?.ToLower() == NavigationModel.Void;

        [DebuggerStepThrough]
        public static bool IsRelatedTo(this ListItem item, string pageName) => item?.Type == ItemType.Parent && item?.Href?.ToPage() == pageName?.ToLower();

        [DebuggerStepThrough]
        public static async Task<IdentityResult> UpdateAsync<T>(this ApplicationDbContext context, T model, string id) where T : class
            {
            try
            {
                var entity = await context.FindAsync<T>(id);

                if (entity == null)
                    {
                    return IdentityResult.Failed();
                    }

                context.Entry((object)entity).CurrentValues.SetValues(model);

                await context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
               Alerta.TratamentoErroComLinha("IdentityExtensions.cs", "UpdateAsync", ex);
               // Manter comportamento original de retornar Failed ou propagar exception?
               // Como retorna IdentityResult, vamos retornar Failed genérico em caso de erro de DB para não quebrar fluxo
               return IdentityResult.Failed(new IdentityError { Description = "Erro interno ao atualizar registro via Extension." });
            }
            }

        [DebuggerStepThrough]
        public static async Task<IdentityResult> DeleteAsync<T>(this ApplicationDbContext context, string id) where T : class
            {
            try
            {
                var entity = await context.FindAsync<T>(id);

                if (entity == null)
                    {
                    return IdentityResult.Failed();
                    }

                context.Remove((object)entity);

                await context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("IdentityExtensions.cs", "DeleteAsync", ex);
                return IdentityResult.Failed(new IdentityError { Description = "Erro interno ao excluir registro via Extension." });
            }
            }
        }
    }


