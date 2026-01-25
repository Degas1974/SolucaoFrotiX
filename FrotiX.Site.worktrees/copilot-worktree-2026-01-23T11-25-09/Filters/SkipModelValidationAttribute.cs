/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  FILTERS - SKIP DE VALIDAÇÃO PARA DTOs PARCIAIS                                     #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FrotiX.Filters
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: SkipModelValidationAttribute                                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Atributo mais leve para pular validação de ModelState. Usado quando o    ║
    /// ║    modelo tem propriedades nullable que podem vir null do frontend mas o    ║
    /// ║    [ApiController] tenta validar como required.                             ║
    /// ║                                                                              ║
    /// ║ 🎯 USO TÍPICO:                                                               ║
    /// ║    APIs com DTOs parciais (PATCH), where nem todos os campos são enviados.  ║
    /// ║    Diferente do DisableModelValidation, este é IActionFilter (mais leve).   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 💡 EXEMPLO:                                                                  ║
    /// ║    [HttpPost]                                                                ║
    /// ║    [SkipModelValidation]                                                     ║
    /// ║    public IActionResult SaveData([FromBody] MyDto dto) { ... }               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: Aplicável em Method ou Class                                      ║
    /// ║    • Arquivos relacionados: [ApiController], DTOs parciais                  ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SkipModelValidationAttribute : Attribute, IActionFilter
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnActionExecuting                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Limpa erros de validação do ModelState antes da action executar.          ║
        /// ║    Permite processar requisição mesmo com campos null.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // [REGRA] Limpa todos os erros de validação do ModelState
            // Isso permite que o endpoint processe a requisição mesmo com campos null
            context.ModelState.Clear();
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnActionExecuted                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Executa após a action. Sem ação necessária.                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // [INFO] Nada a fazer após a execução
        }
    }
}
