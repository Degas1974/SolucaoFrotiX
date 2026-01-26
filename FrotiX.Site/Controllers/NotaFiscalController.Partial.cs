using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    public partial class NotaFiscalController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: Insere
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inserir nova Nota Fiscal e atualizar saldo do empenho
         * 📥 ENTRADAS     : [NotaFiscal] model - Dados da NF a inserir
         * 📤 SAÍDAS       : [JSON] { success, message, notaFiscalId }
         * 🔗 CHAMADA POR  : Tela de cadastro de Notas Fiscais
         * 🔄 CHAMA        : _unitOfWork.NotaFiscal.Add, _unitOfWork.Empenho.Update
         * 📦 DEPENDÊNCIAS : Tabelas NotaFiscal e Empenho
         *
         * [DOC] REGRA DE NEGÓCIO: Ao inserir NF, DEBITA ValorLíquido (ValorNF - ValorGlosa) do SaldoFinal do Empenho
         * [DOC] Validações: NumeroNF, EmpenhoId e ValorNF são obrigatórios
         ****************************************************************************************/
        [Route("Insere")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Insere([FromBody] NotaFiscal model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Dados inválidos" });
                }

                // [DOC] Validações básicas: campos obrigatórios
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

                // [DOC] Gerar novo ID único
                model.NotaFiscalId = Guid.NewGuid();

                // [DOC] Inicializar ValorGlosa como 0 se nulo
                if (model.ValorGlosa == null)
                {
                    model.ValorGlosa = 0;
                }

                // [DOC] Atualizar saldo do empenho: DEBITAR ValorLíquido (NF - Glosa)
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                if (empenho != null)
                {
                    empenho.SaldoFinal = empenho.SaldoFinal - (model.ValorNF - model.ValorGlosa);
                    _unitOfWork.Empenho.Update(empenho);
                }

                _unitOfWork.NotaFiscal.Add(model);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = "Nota Fiscal cadastrada com sucesso!",
                    notaFiscalId = model.NotaFiscalId
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Insere", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao cadastrar Nota Fiscal: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Edita
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Editar Nota Fiscal e ajustar saldo do(s) empenho(s)
         * 📥 ENTRADAS     : [NotaFiscal] model - Dados atualizados da NF
         * 📤 SAÍDAS       : [JSON] { success, message }
         * 🔗 CHAMADA POR  : Tela de edição de Notas Fiscais
         * 🔄 CHAMA        : _unitOfWork.NotaFiscal.Update, _unitOfWork.Empenho.Update
         * 📦 DEPENDÊNCIAS : Tabelas NotaFiscal e Empenho
         *
         * [DOC] REGRA DE NEGÓCIO COMPLEXA:
         * - Se mudou VALOR: Ajusta diferença no empenho
         * - Se mudou EMPENHO: Reverte no antigo, debita no novo
         * - Se mudou AMBOS: Combina as duas operações
         ****************************************************************************************/
        [Route("Edita")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Edita([FromBody] NotaFiscal model)
        {
            try
            {
                if (model == null || model.NotaFiscalId == Guid.Empty)
                {
                    return Json(new { success = false, message = "Dados inválidos" });
                }

                // [DOC] Validações básicas
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

                // [DOC] Buscar nota fiscal existente no banco
                var objFromDb = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                    u.NotaFiscalId == model.NotaFiscalId
                );

                if (objFromDb == null)
                {
                    return Json(new { success = false, message = "Nota Fiscal não encontrada" });
                }

                // [DOC] Calcular diferença de valor líquido para ajustar saldo do empenho
                var valorAntigoLiquido = (objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0);
                var valorNovoLiquido = (model.ValorNF ?? 0) - (model.ValorGlosa ?? 0);
                var diferencaValor = valorNovoLiquido - valorAntigoLiquido;

                // [DOC] Atualizar saldo do empenho (se mudou valor ou empenho)
                if (diferencaValor != 0)
                {
                    // [DOC] CASO 1: Mudou o empenho E o valor
                    if (objFromDb.EmpenhoId != model.EmpenhoId)
                    {
                        // Reverter valor antigo no empenho antigo (CREDITAR)
                        var empenhoAntigo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == objFromDb.EmpenhoId);
                        if (empenhoAntigo != null)
                        {
                            empenhoAntigo.SaldoFinal = empenhoAntigo.SaldoFinal + valorAntigoLiquido;
                            _unitOfWork.Empenho.Update(empenhoAntigo);
                        }

                        // Aplicar valor novo no novo empenho (DEBITAR)
                        var empenhoNovo = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                        if (empenhoNovo != null)
                        {
                            empenhoNovo.SaldoFinal = empenhoNovo.SaldoFinal - valorNovoLiquido;
                            _unitOfWork.Empenho.Update(empenhoNovo);
                        }
                    }
                    else
                    {
                        // [DOC] CASO 2: Mesmo empenho, só ajustar a diferença
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
                        if (empenho != null)
                        {
                            empenho.SaldoFinal = empenho.SaldoFinal - diferencaValor;
                            _unitOfWork.Empenho.Update(empenho);
                        }
                    }
                }
                else if (objFromDb.EmpenhoId != model.EmpenhoId)
                {
                    // [DOC] CASO 3: Valor não mudou mas empenho mudou
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
                }

                // Atualizar campos
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

                _unitOfWork.NotaFiscal.Update(objFromDb);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = "Nota Fiscal atualizada com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Edita", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao atualizar Nota Fiscal: " + error.Message
                });
            }
        }
    }
}
