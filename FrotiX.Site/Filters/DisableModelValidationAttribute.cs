/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DisableModelValidationAttribute.cs                                                     ║
   ║ 📂 CAMINHO: Filters/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Atributo IResourceFilter que desabilita a validação automática do ModelState ANTES do           ║
   ║    [ApiController] executar. Indicado para validação manual customizada.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OnResourceExecuting(ResourceExecutingContext context)                                         ║
   ║    • OnResourceExecuted(ResourceExecutedContext context)                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Microsoft.AspNetCore.Mvc, Microsoft.AspNetCore.Mvc.Filters                         ║
   ║ 📅 ATUALIZAÇÃO: 30/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FrotiX.Filters
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: DisableModelValidationAttribute                                                     │
    /// │ 📦 HERDA DE: Attribute                                                                         │
    /// │ 🔌 IMPLEMENTA: IResourceFilter                                                                 │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// <para>
    /// 🎯 <b>OBJETIVO:</b><br/>
    ///    Desabilitar a validação automática do ModelState para endpoints marcados com este atributo.
    /// </para>
    ///
    /// <para>
    /// 🔗 <b>RASTREABILIDADE:</b><br/>
    ///    ⬅️ CHAMADO POR : Pipeline MVC (IResourceFilter) / Controllers e Actions com atributo<br/>
    ///    ➡️ CHAMA       : ModelState.Clear(), ModelState.Remove()
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DisableModelValidationAttribute : Attribute, IResourceFilter
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: OnResourceExecuting                                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Pipeline MVC (IResourceFilter)                                       │
        /// │    ➡️ CHAMA       : context.ModelState.Clear(), context.ModelState.Remove()             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Limpar o ModelState antes da validação automática do [ApiController].
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    context - Contexto de execução do filtro de recurso.
        /// </para>
        /// </summary>
        /// <param name="context">Contexto de execução do filtro de recurso.</param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Desabilita completamente o ModelState para este request
            context.ModelState.Clear();

            // Remove todas as validações pendentes
            foreach (var key in context.ModelState.Keys.ToList())
            {
                context.ModelState.Remove(key);
            }
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: OnResourceExecuted                                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Pipeline MVC (IResourceFilter)                                       │
        /// │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Callback pós-action. Mantido para cumprir o contrato do filtro.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    context - Contexto de execução pós-action.
        /// </para>
        /// </summary>
        /// <param name="context">Contexto de execução pós-action.</param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Nada a fazer aqui
        }
    }
}
