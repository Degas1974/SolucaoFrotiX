/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  FILTERS - DESABILITAÇÃO DE VALIDAÇÃO DE MODELO                                     #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FrotiX.Filters
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: DisableModelValidationAttribute                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Atributo de filtro para desabilitar validação automática do ModelState   ║
    /// ║    em endpoints específicos. Executa ANTES da validação do [ApiController]. ║
    /// ║                                                                              ║
    /// ║ 🎯 USO:                                                                      ║
    /// ║    Quando se precisa validação manual personalizada ao invés da automática  ║
    /// ║    do [ApiController]. Útil para APIs com regras complexas de validação.    ║
    /// ║                                                                              ║
    /// ║ ⚠️  IMPORTANTE:                                                              ║
    /// ║    Este filtro é IResourceFilter (executa ANTES de IActionFilter).          ║
    /// ║    Isso garante que limpa ModelState antes da validação automática.         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 💡 EXEMPLO DE USO:                                                           ║
    /// ║    [HttpPost]                                                                ║
    /// ║    [DisableModelValidation]                                                  ║
    /// ║    public IActionResult Save([FromBody] ComplexDto dto) { ... }              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: Aplicável em Method ou Class                                      ║
    /// ║    • Arquivos relacionados: [ApiController], ModelState                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DisableModelValidationAttribute : Attribute, IResourceFilter
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnResourceExecuting                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Executa ANTES da validação automática do [ApiController].                 ║
        /// ║    Limpa completamente o ModelState para desabilitar validação.              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // [REGRA] Desabilita completamente o ModelState para este request
            context.ModelState.Clear();

            // [REGRA] Remove todas as validações pendentes (garantia extra)
            foreach (var key in context.ModelState.Keys.ToList())
            {
                context.ModelState.Remove(key);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnResourceExecuted                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Executa DEPOIS da action. Sem ação necessária.                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // [INFO] Nada a fazer aqui - validação já foi desabilitada no executing
        }
    }
}
