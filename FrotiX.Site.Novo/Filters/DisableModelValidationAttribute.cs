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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: DisableModelValidationAttribute                                                     │
    // │ 📦 HERDA DE: Attribute                                                                         │
    // │ 🔌 IMPLEMENTA: IResourceFilter                                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Desabilitar a validação automática do ModelState para endpoints marcados com este atributo.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Pipeline MVC (IResourceFilter) / Controllers e Actions com atributo
    // ➡️ CHAMA       : ModelState.Clear(), ModelState.Remove()
    
    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DisableModelValidationAttribute : Attribute, IResourceFilter
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnResourceExecuting                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline MVC (IResourceFilter)                                       │
        // │    ➡️ CHAMA       : context.ModelState.Clear(), context.ModelState.Remove()             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Limpar o ModelState antes da validação automática do [ApiController].
        
        
        
        // 📥 PARÂMETROS:
        // context - Contexto de execução do filtro de recurso.
        
        
        // Param context: Contexto de execução do filtro de recurso.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: OnResourceExecuted                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Pipeline MVC (IResourceFilter)                                       │
        // │    ➡️ CHAMA       : (sem chamadas internas)                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Callback pós-action. Mantido para cumprir o contrato do filtro.
        
        
        
        // 📥 PARÂMETROS:
        // context - Contexto de execução pós-action.
        
        
        // Param context: Contexto de execução pós-action.
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Nada a fazer aqui
        }
    }
}
