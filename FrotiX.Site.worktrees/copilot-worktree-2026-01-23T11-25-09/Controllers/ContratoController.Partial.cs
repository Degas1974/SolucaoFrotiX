using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API)                                                                          |
 * | (IA) IDENTIDADE: ContratoController.Partial.cs                                                          |
 * | (IA) DESCRIÇÃO: Fragmento da Controller de Contratos (Métodos Auxiliares).                              |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */


namespace FrotiX.Controllers
{
    public partial class ContratoController : Controller
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaContratosPorStatus                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista contratos filtrados por status para preenchimento de dropdowns.     ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite seleção de contratos ativos/inativos em telas de cadastro.        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • status (int): 1 para Ativo, 0 para Inativo.                              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista {value, text}.                             ║
        /// ║    • Consumidor: UI de contratos (Select2/Dropdown).                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Contrato.GetAll() → consulta contratos.                      ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Contrato/ListaContratosPorStatus                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos                                               ║
        /// ║    • Arquivos relacionados: Pages/Contrato/*.cshtml                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ListaContratosPorStatus")]
        [HttpGet]
        public IActionResult ListaContratosPorStatus(int status)
        {
            try
            {
                // [LOGICA] Converte status para bool
                bool statusBool = status == 1;

                // [DADOS] Filtra e projeta dados para Select2/Dropdown
                var result = (
                    from c in _unitOfWork.Contrato.GetAll()
                    where c.Status == statusBool
                    orderby c.AnoContrato descending, c.NumeroContrato descending
                    select new
                    {
                        value = c.ContratoId,
                        text = c.AnoContrato + "/" + c.NumeroContrato + " - " + c.Objeto
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "ContratoController.Partial.cs", "ListaContratosPorStatus");
                Alerta.TratamentoErroComLinha(
                    "ContratoController.Partial.cs",
                    "ListaContratosPorStatus",
                    error
                );
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao listar contratos"
                });
            }
        }
    }
}

