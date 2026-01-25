using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using FrotiX.Helpers;

namespace FrotiX.Controllers
{
    /* > ---------------------------------------------------------------------------------------
     > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
     > ---------------------------------------------------------------------------------------
     > 🆔 **Nome:** AtaRegistroPrecosController.Partial.cs
     > 📍 **Local:** Controllers
     > ❓ **Por que existo?** Extensão do controlador principal para métodos auxiliares e
     >                      verificações de integridade.
     > 🔗 **Relevância:** Alta (Suporte e Validação)
     > --------------------------------------------------------------------------------------- */

    public partial class AtaRegistroPrecosController : ControllerBase
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VerificarDependencias                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica se a Ata possui registros dependentes antes de exclusão.         ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Evita exclusões inválidas por integridade referencial.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID da Ata.                                                   ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contadores de dependências.                     ║
        /// ║    • Consumidor: UI de Atas de Registro de Preços.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ItemVeiculoAta.GetAll()                                      ║
        /// ║    • _unitOfWork.VeiculoAta.GetAll()                                          ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/AtaRegistroPrecos/VerificarDependencias                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos/Atas                                           ║
        /// ║    • Arquivos relacionados: Pages/AtaRegistroPrecos/*.cshtml                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("VerificarDependencias")]
        [HttpGet]
        public IActionResult VerificarDependencias(Guid id)
        {
            try
            {
                // [REGRA] Valida ID
                if (id == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "ID inválido"
                    });
                }

                // [DADOS] Contagem de dependências
                int itensCount = _unitOfWork.ItemVeiculoAta.GetAll(i => i.RepactuacaoAta.AtaId == id).Count();
                int veiculosCount = _unitOfWork.VeiculoAta.GetAll(v => v.AtaId == id).Count();

                // [LOGICA] Determina se há dependências
                bool possuiDependencias = itensCount > 0 || veiculosCount > 0;

                // [DADOS] Retorno para UI
                return Ok(new
                {
                    success = true,
                    possuiDependencias,
                    itens = itensCount,
                    veiculos = veiculosCount
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "AtaRegistroPrecosController.Partial.cs", "VerificarDependencias");
                Alerta.TratamentoErroComLinha(
                    "AtaRegistroPrecosController.Partial.cs",
                    "VerificarDependencias",
                    error
                );
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao verificar dependências"
                });
            }
        }
    }
}
