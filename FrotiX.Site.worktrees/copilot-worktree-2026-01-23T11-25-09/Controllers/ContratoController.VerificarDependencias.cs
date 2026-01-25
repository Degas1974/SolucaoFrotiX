using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API)                                                                          |
 * | (IA) IDENTIDADE: ContratoController.VerificarDependencias.cs                                            |
 * | (IA) DESCRIÇÃO: Fragmento da Controller de Contratos (Integridade de Dados).                            |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */


namespace FrotiX.Controllers
{
    public partial class ContratoController
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VerificarDependencias                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica dependências do contrato em várias tabelas para evitar exclusão.║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Protege integridade referencial e previne exclusões indevidas.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do contrato.                                              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contadores e flag `possuiDependencias`.         ║
        /// ║    • Consumidor: UI de Contratos.                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.*.GetAll() → consultas de dependências.                      ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Contrato/VerificarDependencias                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos                                               ║
        /// ║    • Arquivos relacionados: Pages/Contrato/*.cshtml                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("VerificarDependencias")]
        public IActionResult VerificarDependencias(Guid id)
        {
            int veiculosContrato = 0;
            int encarregados = 0;
            int operadores = 0;
            int lavadores = 0;
            int motoristas = 0;
            int empenhos = 0;
            int notasFiscais = 0;

            try
            {
                // [LOGICA] Verificações isoladas por tabela para evitar interrupções por erros pontuais
                try { veiculosContrato = _unitOfWork.VeiculoContrato.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { encarregados = _unitOfWork.Encarregado.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { operadores = _unitOfWork.Operador.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { lavadores = _unitOfWork.Lavador.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { motoristas = _unitOfWork.Motorista.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { empenhos = _unitOfWork.Empenho.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { notasFiscais = _unitOfWork.NotaFiscal.GetAll(x => x.ContratoId == id).Count(); } catch { }

                // [LOGICA] Determina se há dependências
                var possuiDependencias = veiculosContrato > 0 || encarregados > 0 ||
                                         operadores > 0 || lavadores > 0 || motoristas > 0 ||
                                         empenhos > 0 || notasFiscais > 0;

                // [DADOS] Retorno para UI
                return Json(new
                {
                    success = true,
                    possuiDependencias = possuiDependencias,
                    veiculosContrato = veiculosContrato,
                    encarregados = encarregados,
                    operadores = operadores,
                    lavadores = lavadores,
                    motoristas = motoristas,
                    empenhos = empenhos,
                    notasFiscais = notasFiscais
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex, "ContratoController.VerificarDependencias.cs", "VerificarDependencias");
                Alerta.TratamentoErroComLinha("ContratoController.VerificarDependencias.cs", "VerificarDependencias", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao verificar dependências: " + ex.Message
                });
            }
        }
    }
}

