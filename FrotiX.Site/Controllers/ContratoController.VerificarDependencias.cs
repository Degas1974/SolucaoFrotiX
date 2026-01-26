/*
 ╔══════════════════════════════════════════════════════════════════════════╗
 ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO                                            ║
 ║  Arquivo: ContratoController.VerificarDependencias.cs                    ║
 ║  Caminho: /Controllers/ContratoController.VerificarDependencias.cs       ║
 ║  Documentado em: 2026-01-26                                              ║
 ║  Partial Class: Verificação de dependências de Contrato                 ║
 ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ PARTIAL CLASS: ContratoController (VerificarDependencias)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Verificar dependências antes de excluir Contrato
     * 📥 ENTRADAS     : [Guid] id - ID do Contrato a ser verificado
     * 📤 SAÍDAS       : JSON com contagens de dependências (veículos, pessoas, empenhos, NFs)
     * 🔗 CHAMADA POR  : Frontend antes de exclusão de Contrato
     * 🔄 CHAMA        : VeiculoContrato, Encarregado, Operador, Lavador, Motorista, etc.
     * 📦 DEPENDÊNCIAS : IUnitOfWork, Multiple Repositories
     * --------------------------------------------------------------------------------------
     * [DOC] Classe parcial dedicada a verificação de dependências de Contrato
     * [DOC] Verifica 7 tipos de dependências: VeiculosContrato, Encarregados, Operadores,
     *       Lavadores, Motoristas, Empenhos, NotasFiscais
     * [DOC] Cada verificação em try/catch separado para não falhar se tabela não existir
     * [DOC] Retorna contadores individuais e flag possuiDependencias
     ****************************************************************************************/
    public partial class ContratoController
    {
        /// <summary>
        /// Verifica se o contrato possui dependências que impedem sua exclusão
        /// </summary>
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
                // Cada verificação em try/catch separado para não falhar se uma tabela não existir

                try
                {
                    veiculosContrato = _unitOfWork.VeiculoContrato
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                try
                {
                    encarregados = _unitOfWork.Encarregado
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                try
                {
                    operadores = _unitOfWork.Operador
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                try
                {
                    lavadores = _unitOfWork.Lavador
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                try
                {
                    motoristas = _unitOfWork.Motorista
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                try
                {
                    empenhos = _unitOfWork.Empenho
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                try
                {
                    notasFiscais = _unitOfWork.NotaFiscal
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }

                var possuiDependencias = veiculosContrato > 0 || encarregados > 0 || 
                                         operadores > 0 || lavadores > 0 || motoristas > 0 ||
                                         empenhos > 0 || notasFiscais > 0;

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
            catch (System.Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Erro ao verificar dependências: " + ex.Message
                });
            }
        }
    }
}
