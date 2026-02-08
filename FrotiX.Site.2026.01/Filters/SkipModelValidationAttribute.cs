/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Atributo para pular validação de ModelState (útil para APIs com dtos parciais).
 * =========================================================================================
 */

using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FrotiX.Filters
{
    /// <summary>
    /// Atributo para desabilitar a validação automática de ModelState em endpoints específicos.
    /// Usado quando o modelo tem propriedades nullable que podem vir como null do frontend,
    /// mas o [ApiController] tenta validar como required.
    /// </summary>
    /// <example>
    /// [HttpPost]
    /// [SkipModelValidation]
    /// public IActionResult SaveData([FromBody] MyDto dto)
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SkipModelValidationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Limpa todos os erros de validação do ModelState
            // Isso permite que o endpoint processe a requisição mesmo com campos null
            context.ModelState.Clear();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nada a fazer após a execução
        }
    }
}
