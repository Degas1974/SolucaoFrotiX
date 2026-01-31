/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SkipModelValidationAttribute.cs                                                        ║
   ║ 📂 CAMINHO: Filters/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Atributo IActionFilter que limpa o ModelState durante OnActionExecuting. Usado quando           ║
   ║    propriedades nullable podem vir null do frontend, mas [ApiController] valida como required.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OnActionExecuting(ActionExecutingContext context)                                             ║
   ║    • OnActionExecuted(ActionExecutedContext context)                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Microsoft.AspNetCore.Mvc.Filters                                                  ║
   ║ 📅 ATUALIZAÇÃO: 30/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FrotiX.Filters
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: SkipModelValidationAttribute                                                        │
    /// │ 📦 HERDA DE: Attribute                                                                         │
    /// │ 🔌 IMPLEMENTA: IActionFilter                                                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// <para>
    /// 🎯 <b>OBJETIVO:</b><br/>
    ///    Desabilitar a validação automática de ModelState em endpoints específicos.
    /// </para>
    ///
    /// <para>
    /// 🔗 <b>RASTREABILIDADE:</b><br/>
    ///    ⬅️ CHAMADO POR : Pipeline MVC (IActionFilter) / Controllers e Actions com atributo<br/>
    ///    ➡️ CHAMA       : context.ModelState.Clear()
    /// </para>
    /// </summary>
    /// <example>
    /// [HttpPost]
    /// [SkipModelValidation]
    /// public IActionResult SaveData([FromBody] MyDto dto)
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SkipModelValidationAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: OnActionExecuting                                                            │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Pipeline MVC (IActionFilter)                                         │
        /// │    ➡️ CHAMA       : context.ModelState.Clear()                                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Limpar erros de validação do ModelState antes da execução da action.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    context - Contexto da execução do filtro de action.
        /// </para>
        /// </summary>
        /// <param name="context">Contexto da execução do filtro de action.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Limpa todos os erros de validação do ModelState
            // Isso permite que o endpoint processe a requisição mesmo com campos null
            context.ModelState.Clear();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: OnActionExecuted                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Pipeline MVC (IActionFilter)                                         │
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
        ///    context - Contexto pós-execução da action.
        /// </para>
        /// </summary>
        /// <param name="context">Contexto pós-execução da action.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nada a fazer após a execução
        }
    }
}
