/*
 *  _______________________________________________________
 * |                                                       |
 * |                FROTIX - SOLUÇÃO 2026                  |
 * |          ___________________________________          |
 * |                                                       |
 * |   FrotiX Core - Gestão de Notas Fiscais (Core Stack)  |
 * |_______________________________________________________|
 *
 * (IA) Extensão parcial para rotas de inserção e processamento.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: NotaFiscalController (Partial)                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extensão parcial para rotas de inserção e processamento de NFs.           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class NotaFiscalController : Controller
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Insere (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cadastra nova NF, valida dados e atualiza empenho com auditoria.         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (NotaFiscal): Dados da Nota Fiscal.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status e ID da NF.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Insere")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Insere([FromBody] NotaFiscal model)
        {
            try
            {
                // [VALIDACAO] Verifica payload.
                if (model == null)
                {
                    return Json(new { success = false, message = "Dados inválidos" });
                }

                // [VALIDACAO] Campos obrigatórios.
                if (model.NumeroNF == null || model.NumeroNF == 0)
                {
                    return Json(new { success = false, message = "O número da Nota Fiscal é obrigatório" });
                }

                if (model.EmpenhoId == null || model.EmpenhoId == Guid.Empty)
                {
                    return Json(new { success = false, message = "O Empenho é obrigatório" });
                }

                if (model.ValorNF == null || model.ValorNF == 0)
                {
                    return Json(new { success = false, message = "O valor da Nota Fiscal é obrigatório" });
                }

                // [DADOS] Gerar novo ID.
                model.NotaFiscalId = Guid.NewGuid();

                // [DADOS] Inicializar ValorGlosa se null.
                if (model.ValorGlosa == null)
                {
                    model.ValorGlosa = 0;
                }

                // [DADOS] Atualizar saldo do empenho.
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                if (empenho != null)
                {
                    // [CALCULO] Valor líquido debitado.
                    double valorDebitado = (model.ValorNF ?? 0) - (model.ValorGlosa ?? 0);
                    empenho.SaldoFinal = empenho.SaldoFinal - valorDebitado;
                    _unitOfWork.Empenho.Update(empenho);
                    
                    // [LOG] Auditoria de débito.
                    _log.Info($"NotaFiscalController.Insere: NF {model.NumeroNF} cadastrada. Valor líquido debitado do empenho {empenho.EmpenhoId}: {valorDebitado:C}");
                }

                // [ACAO] Salva NF.
                _unitOfWork.NotaFiscal.Add(model);
                _unitOfWork.Save();

                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true,
                    message = "Nota Fiscal cadastrada with sucesso!",
                    notaFiscalId = model.NotaFiscalId
                });
            }
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.Insere", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.Partial.cs", "Insere", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao cadastrar Nota Fiscal: " + ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Edita (POST)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza dados de NF e reajusta saldo do empenho com auditoria.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (NotaFiscal): Dados atualizados da NF.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da edição.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Edita")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Edita([FromBody] NotaFiscal model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model == null || model.NotaFiscalId == Guid.Empty)
                {
                    return Json(new { success = false, message = "Dados inválidos" });
                }

                // [VALIDACAO] Campos obrigatórios.
                if (model.NumeroNF == null || model.NumeroNF == 0)
                {
                    return Json(new { success = false, message = "O número da Nota Fiscal é obrigatório" });
                }

                if (model.EmpenhoId == null || model.EmpenhoId == Guid.Empty)
                {
                    return Json(new { success = false, message = "O Empenho é obrigatório" });
                }

                if (model.ValorNF == null || model.ValorNF == 0)
                {
                    return Json(new { success = false, message = "O valor da Nota Fiscal é obrigatório" });
                }

                // [DADOS] Buscar nota fiscal existente.
                var objFromDb = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                    u.NotaFiscalId == model.NotaFiscalId
                );

                if (objFromDb == null)
                {
                    // [RETORNO] NF não encontrada.
                    return Json(new { success = false, message = "Nota Fiscal não encontrada" });
                }

                // [CALCULO] Diferença de valor para ajustar saldo do empenho.
                var valorAntigoLiquido = (objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0);
                var valorNovoLiquido = (model.ValorNF ?? 0) - (model.ValorGlosa ?? 0);
                var diferencaValor = valorNovoLiquido - valorAntigoLiquido;

                // [REGRA] Atualizar saldo do empenho (se mudou).
                if (diferencaValor != 0)
                {
                    // [REGRA] Se mudou o empenho, reverter no antigo e aplicar no novo.
                    if (objFromDb.EmpenhoId != model.EmpenhoId)
                    {
                        // [ACAO] Reverter no empenho antigo.
                        var empenhoAntigo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == objFromDb.EmpenhoId);
                        if (empenhoAntigo != null)
                        {
                            empenhoAntigo.SaldoFinal = empenhoAntigo.SaldoFinal + valorAntigoLiquido;
                            _unitOfWork.Empenho.Update(empenhoAntigo);
                            _log.Info($"NotaFiscalController.Edita: Estornando {valorAntigoLiquido:C} para empenho antigo {empenhoAntigo.EmpenhoId} devido a troca de empenho na NF {model.NumeroNF}");
                        }

                        // [ACAO] Aplicar no novo empenho.
                        var empenhoNovo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                        if (empenhoNovo != null)
                        {
                            empenhoNovo.SaldoFinal = empenhoNovo.SaldoFinal - valorNovoLiquido;
                            _unitOfWork.Empenho.Update(empenhoNovo);
                            _log.Info($"NotaFiscalController.Edita: Debitando {valorNovoLiquido:C} do novo empenho {empenhoNovo.EmpenhoId} para a NF {model.NumeroNF}");
                        }
                    }
                    else
                    {
                        // [ACAO] Mesmo empenho, ajusta a diferença.
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                        if (empenho != null)
                        {
                            empenho.SaldoFinal = empenho.SaldoFinal - diferencaValor;
                            _unitOfWork.Empenho.Update(empenho);
                            _log.Info($"NotaFiscalController.Edita: Ajustando saldo do empenho {empenho.EmpenhoId} em {diferencaValor:C} devido a alteração na NF {model.NumeroNF}");
                        }
                    }
                }
                else if (objFromDb.EmpenhoId != model.EmpenhoId)
                {
                    // [REGRA] Valor não mudou mas empenho mudou.
                    var empenhoAntigo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == objFromDb.EmpenhoId);
                    if (empenhoAntigo != null)
                    {
                        empenhoAntigo.SaldoFinal = empenhoAntigo.SaldoFinal + valorAntigoLiquido;
                        _unitOfWork.Empenho.Update(empenhoAntigo);
                    }

                    var empenhoNovo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                    if (empenhoNovo != null)
                    {
                        empenhoNovo.SaldoFinal = empenhoNovo.SaldoFinal - valorNovoLiquido;
                        _unitOfWork.Empenho.Update(empenhoNovo);
                    }
                    _log.Info($"NotaFiscalController.Edita: Troca de empenho da NF {model.NumeroNF} sem alteração de valor. Valor: {valorNovoLiquido:C}");
                }

                // [ATUALIZACAO] Atualizar campos da NF.
                objFromDb.NumeroNF = model.NumeroNF;
                objFromDb.DataEmissao = model.DataEmissao;
                objFromDb.TipoNF = model.TipoNF;
                objFromDb.MesReferencia = model.MesReferencia;
                objFromDb.AnoReferencia = model.AnoReferencia;
                objFromDb.ValorNF = model.ValorNF;
                objFromDb.ValorGlosa = model.ValorGlosa ?? 0;
                objFromDb.Objeto = model.Objeto;
                objFromDb.MotivoGlosa = model.MotivoGlosa;
                objFromDb.EmpenhoId = model.EmpenhoId;
                objFromDb.ContratoId = model.ContratoId;
                objFromDb.AtaId = model.AtaId;

                // [ACAO] Salva alterações.
                _unitOfWork.NotaFiscal.Update(objFromDb);
                _unitOfWork.Save();

                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true,
                    message = "Nota Fiscal atualizada com sucesso!"
                });
            }
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.Edita", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.Partial.cs", "Edita", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao atualizar Nota Fiscal: " + ex.Message
                });
            }
        }
    }
}
