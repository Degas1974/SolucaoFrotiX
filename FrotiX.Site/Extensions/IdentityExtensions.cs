/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IdentityExtensions.cs                                                                  ║
   ║ 📂 CAMINHO: /Extensions                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Extensões para ASP.NET Identity e ClaimsPrincipal, incluindo verificação de roles,              ║
   ║    autorização de itens de menu, conversão para HtmlString e operações CRUD genéricas.             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [HasRole]       : Verifica roles do usuário...... (ClaimsPrincipal, roles) -> bool              ║
   ║ 2. [AuthorizeFor]  : Filtra itens por permissão..... (IEnumerable<ListItem>) -> IEnumerable        ║
   ║ 3. [AsRaw]         : Converte para HtmlString....... (string) -> HtmlString                        ║
   ║ 4. [ToPage]        : Extrai nome da página.......... (string href) -> string                       ║
   ║ 5. [IsVoid]        : Verifica se href é void........ (string href) -> bool                         ║
   ║ 6. [IsRelatedTo]   : Verifica relação com página.... (ListItem, pageName) -> bool                  ║
   ║ 7. [UpdateAsync]   : Atualiza entidade.............. (T model, string id) -> IdentityResult        ║
   ║ 8. [DeleteAsync]   : Remove entidade................ (string id) -> IdentityResult                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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

namespace FrotiX.Extensions
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ CLASSE: IdentityExtensions                                                         │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Extensions para ASP.NET Identity, autorização e operações CRUD.        │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🔗 RASTREABILIDADE:                                                                   │
    /// │    ⬅️ CHAMADO POR : _Layout.cshtml, NavigationService, UsersEndpoint                  │
    /// │    ➡️ CHAMA       : ApplicationDbContext, ClaimsPrincipal                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: HasRole                                                            │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Verifica se o usuário possui uma das roles especificadas.              │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • principal [ClaimsPrincipal], roles [string[]]                           │
        /// │ 📤 OUTPUTS: • [bool]: true se possui alguma das roles                                │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        private static bool HasRole(this ClaimsPrincipal principal, params string[] roles)
        {
            // [SEGURANCA] Verificar se principal é válido
            if (principal == null)
                return default;

            // [DADOS] Extrair claims de role do usuário
            var claims = principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToSafeList();

            // [LOGICA] Verificar interseção com roles solicitadas
            return claims?.Any() == true && claims.Intersect(roles ?? new string[] { }).Any();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: AuthorizeFor                                                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Filtra itens de menu com base nas roles do usuário logado.             │
        /// │    Se o item não tem roles definidas, é liberado para todos.                         │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • source [IEnumerable<ListItem>], identity [ClaimsPrincipal]              │
        /// │ 📤 OUTPUTS: • [IEnumerable<ListItem>]: Itens autorizados                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static IEnumerable<ListItem> AuthorizeFor(this IEnumerable<ListItem> source, ClaimsPrincipal identity)
            => source.Where(x => x.Roles.IsNullOrEmpty() || (x.Roles.HasItems() && identity.HasRole(x.Roles))).ToSafeList();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: AsRaw                                                              │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Converte string para HtmlString (markup não encodado).                 │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static HtmlString AsRaw(this string value) => new HtmlString(value);

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: ToPage                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Extrai o nome da página (sem extensão) de um href.                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static string ToPage(this string href) => System.IO.Path.GetFileNameWithoutExtension(href)?.ToLower();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: IsVoid                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Verifica se o href é um link vazio (javascript:void).                  │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsVoid(this string href) => href?.ToLower() == NavigationModel.Void;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: IsRelatedTo                                                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Verifica se um item de menu é relacionado à página atual.              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsRelatedTo(this ListItem item, string pageName) => item?.Type == ItemType.Parent && item?.Href?.ToPage() == pageName?.ToLower();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: UpdateAsync                                                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Atualiza uma entidade genérica no banco de dados.                      │
        /// │    Busca a entidade pelo ID, aplica os novos valores e salva.                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • model [T]: Dados atualizados, id [string]: ID da entidade               │
        /// │ 📤 OUTPUTS: • [IdentityResult]: Success ou Failed                                    │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : UsersEndpoint.Update()                                            │
        /// │    ➡️ CHAMA       : ApplicationDbContext.FindAsync, SaveChangesAsync                  │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static async Task<IdentityResult> UpdateAsync<T>(this ApplicationDbContext context, T model, string id) where T : class
        {
            // [DB] Buscar entidade pelo ID
            var entity = await context.FindAsync<T>(id);

            // [LOGICA] Verificar se existe
            if (entity == null)
            {
                return IdentityResult.Failed();
            }

            // [DB] Aplicar valores atualizados
            context.Entry((object)entity).CurrentValues.SetValues(model);

            // [DB] Persistir alterações
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: DeleteAsync                                                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO: Remove uma entidade genérica do banco de dados pelo ID.                │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: • id [string]: ID da entidade a remover                                   │
        /// │ 📤 OUTPUTS: • [IdentityResult]: Success ou Failed                                    │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : UsersEndpoint.Delete()                                            │
        /// │    ➡️ CHAMA       : ApplicationDbContext.FindAsync, Remove, SaveChangesAsync          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        [DebuggerStepThrough]
        public static async Task<IdentityResult> DeleteAsync<T>(this ApplicationDbContext context, string id) where T : class
        {
            // [DB] Buscar entidade pelo ID
            var entity = await context.FindAsync<T>(id);

            // [LOGICA] Verificar se existe
            if (entity == null)
            {
                return IdentityResult.Failed();
            }

            // [DB] Remover entidade
            context.Remove((object)entity);

            // [DB] Persistir alterações
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}


