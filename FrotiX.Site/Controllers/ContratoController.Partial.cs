/*
 ╔══════════════════════════════════════════════════════════════════════════╗
 ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO                                            ║
 ║  Arquivo: ContratoController.Partial.cs                                  ║
 ║  Caminho: /Controllers/ContratoController.Partial.cs                     ║
 ║  Documentado em: 2026-01-26                                              ║
 ║  Partial Class: Métodos auxiliares de Contrato                          ║
 ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ PARTIAL CLASS: ContratoController (Partial)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Fornecer métodos auxiliares para Contratos (lista por status)
     * 📥 ENTRADAS     : [int] status - 1 (Ativo) ou 0 (Inativo)
     * 📤 SAÍDAS       : JSON com lista de contratos para dropdown
     * 🔗 CHAMADA POR  : Frontend de Nota Fiscal (dropdown de Contratos)
     * 🔄 CHAMA        : Contrato.GetAll() via LINQ
     * 📦 DEPENDÊNCIAS : IUnitOfWork, Entity Framework LINQ
     * --------------------------------------------------------------------------------------
     * [DOC] Classe parcial com métodos auxiliares de Contrato
     * [DOC] Lista contratos filtrados por Status para dropdown de Nota Fiscal
     * [DOC] Formata como "Ano/Numero - Objeto" para exibição
     ****************************************************************************************/
    public partial class ContratoController : Controller
    {
        /// <summary>
        /// Lista contratos filtrados por Status (para dropdown de Nota Fiscal)
        /// </summary>
        /// <param name="status">1 = Ativo, 0 = Inativo</param>
        [Route("ListaContratosPorStatus")]
        [HttpGet]
        public IActionResult ListaContratosPorStatus(int status)
        {
            try
            {
                bool statusBool = status == 1;

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
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaContratosPorStatus", error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
    }
}
