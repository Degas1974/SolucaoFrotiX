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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: SkipModelValidationAttribute                                                        │
    // │ 📦 HERDA DE: Attribute                                                                         │
    // │ 🔌 IMPLEMENTA: IActionFilter                                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Desabilitar a validação automática de ModelState em endpoints específicos.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Pipeline MVC (IActionFilter) / Controllers e Actions com atributo
    // ➡️ CHAMA       : context.ModelState.Clear()
    
    
    
    // [HttpPost]
    // [SkipModelValidation]
    // public IActionResult SaveData([FromBody] MyDto dto)
    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SkipModelValidationAttribute : Attribute, IActionFilter
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnActionExecuting                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline MVC (IActionFilter)                                         │
        // │    ➡️ CHAMA       : context.ModelState.Clear()                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Limpar erros de validação do ModelState antes da execução da action.
        
        
        
        // 📥 PARÂMETROS:
        // context - Contexto da execução do filtro de action.
        
        
        // Param context: Contexto da execução do filtro de action.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Limpa todos os erros de validação do ModelState
            // Isso permite que o endpoint processe a requisição mesmo com campos null
            context.ModelState.Clear();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnActionExecuted                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline MVC (IActionFilter)                                         │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Callback pós-action. Mantido para cumprir o contrato do filtro.
        
        
        
        // 📥 PARÂMETROS:
        // context - Contexto pós-execução da action.
        
        
        // Param context: Contexto pós-execução da action.
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nada a fazer após a execução
        }
    }
}
