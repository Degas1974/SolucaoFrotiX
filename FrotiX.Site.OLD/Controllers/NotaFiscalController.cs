/* ****************************************************************************************
 * ⚡ ARQUIVO: NotaFiscalController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar Notas Fiscais, glosas e vínculos com empenhos/contratos.
 *
 * 📥 ENTRADAS     : IDs e payloads de Nota Fiscal/Glosa.
 *
 * 📤 SAÍDAS       : JSON com dados e mensagens de operação.
 *
 * 🔗 CHAMADA POR  : Telas de Notas Fiscais e controles relacionados.
 *
 * 🔄 CHAMA        : IUnitOfWork (NotaFiscal, Empenho, Contrato).
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: NotaFiscalController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor operações de manutenção, glosa e consulta de NFs.
     *
     * 📥 ENTRADAS     : IDs de nota, empenho, contrato e payloads de glosa.
     *
     * 📤 SAÍDAS       : JSON com listas e confirmações.
     *
     * 🔗 CHAMADA POR  : Telas de gerenciamento de NFs.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class NotaFiscalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: NotaFiscalController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência de acesso ao banco.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public NotaFiscalController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NotaFiscalController", error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Endpoint placeholder (sem implementação).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Sem retorno (void).
         *
         * 🔗 CHAMADA POR  : Rotas de teste/compatibilidade.
         ****************************************************************************************/
        [HttpGet]
        public void Get()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Get", error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Excluir Nota Fiscal e devolver saldo líquido ao empenho.
         *
         * 📥 ENTRADAS     : [NotaFiscalViewModel] model.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Tela de gerenciamento de Notas Fiscais.
         *
         * 🔄 CHAMA        : NotaFiscal.Remove(), Empenho.Update(), Save().
         *
         * 📝 OBSERVAÇÕES  : Devolve ValorLíquido (ValorNF - ValorGlosa) ao saldo do empenho.
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(NotaFiscalViewModel model)
        {
            try
            {
                if (model != null && model.NotaFiscalId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                        u.NotaFiscalId == model.NotaFiscalId
                    );
                    if (objFromDb != null)
                    {
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                            u.EmpenhoId == objFromDb.EmpenhoId
                        );
                        if (empenho != null)
                        {
                            // [DOC] Ao excluir NF, devolver o valor líquido (NF - Glosa) ao saldo do empenho
                            empenho.SaldoFinal = empenho.SaldoFinal + ((objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0));
                            _unitOfWork.Empenho.Update(empenho);
                        }

                        _unitOfWork.NotaFiscal.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(new
                        {
                            success = true,
                            message = "Nota Fiscal removida com sucesso"
                        });
                    }
                }
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar Nota Fiscal"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Delete", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar Nota Fiscal"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetGlosa
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter dados de glosa de uma Nota Fiscal específica.
         *
         * 📥 ENTRADAS     : id (Guid) - NotaFiscalId.
         *
         * 📤 SAÍDAS       : JSON com dados da NF ou erro.
         *
         * 🔗 CHAMADA POR  : Tela de glosa.
         *
         * 🔄 CHAMA        : NotaFiscal.GetFirstOrDefault().
         ****************************************************************************************/
        [Route("GetGlosa")]
        [HttpGet]
        public IActionResult GetGlosa(Guid id)
        {
            try
            {
                var notaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                    u.NotaFiscalId == id
                );

                if (notaFiscal == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nota Fiscal não encontrada"
                    });
                }

                return Json(new
                {
                    success = true,
                    notaFiscalId = notaFiscal.NotaFiscalId,
                    numeroNF = notaFiscal.NumeroNF,
                    valorNF = notaFiscal.ValorNF ?? 0,
                    valorGlosa = notaFiscal.ValorGlosa ?? 0,
                    valorGlosaFormatado = (notaFiscal.ValorGlosa ?? 0).ToString("N2"),
                    motivoGlosa = notaFiscal.MotivoGlosa ?? "",
                    temGlosa = (notaFiscal.ValorGlosa ?? 0) > 0
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetGlosa", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao buscar dados da glosa"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Glosa
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Aplicar ou atualizar glosa em Nota Fiscal e ajustar saldo do empenho
         * 📥 ENTRADAS     : [GlosaNota] glosanota - NotaFiscalId, ValorGlosa, MotivoGlosa, ModoGlosa
         * 📤 SAÍDAS       : [JSON] { success, message, novaGlosa, novaGlosaFormatada }
         * 🔗 CHAMADA POR  : Tela de gerenciamento de Notas Fiscais (modal de glosa)
         * 🔄 CHAMA        : _unitOfWork.NotaFiscal.Update, _unitOfWork.Empenho.Update
         * 📦 DEPENDÊNCIAS : Tabelas NotaFiscal e Empenho
         *
         * [DOC] REGRA DE NEGÓCIO: Glosa DEVOLVE dinheiro ao empenho (aumenta SaldoFinal)
         * [DOC] ModoGlosa: "somar" = soma à glosa existente | "substituir" = substitui valor
         * [DOC] Validação: Glosa não pode exceder ValorNF
         * [DOC] Conversão automática: Se valor parece estar em centavos, divide por 100
         ****************************************************************************************/
        /****************************************************************************************
         * ⚡ FUNÇÃO: Glosa
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Registrar glosa em Nota Fiscal e ajustar saldo do empenho.
         *
         * 📥 ENTRADAS     : [GlosaNota] glosanota (JSON).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Tela de glosa de NF.
         *
         * 🔄 CHAMA        : NotaFiscal.Update(), Empenho.Update(), Save().
         ****************************************************************************************/
        [Route("Glosa")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Glosa([FromBody] GlosaNota glosanota)
        {
            try
            {
                // [DOC] Buscar nota fiscal
                var notaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                    u.NotaFiscalId == glosanota.NotaFiscalId
                );

                if (notaFiscal == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nota Fiscal não encontrada"
                    });
                }

                // [DOC] Valor da glosa informada (converter de centavos se necessário)
                var valorGlosaInformada = glosanota.ValorGlosa ?? 0;

                // [DOC] Se o valor parece estar em centavos (muito maior que o valor da NF), dividir por 100
                if (valorGlosaInformada > 100 && valorGlosaInformada > (notaFiscal.ValorNF ?? 0) * 1.5)
                {
                    valorGlosaInformada = valorGlosaInformada / 100;
                }

                // [DOC] Glosa antiga (para calcular diferença)
                var glosaAntiga = notaFiscal.ValorGlosa ?? 0;

                // [DOC] Calcular nova glosa baseado no modo
                double novaGlosa;
                if (glosanota.ModoGlosa == "somar")
                {
                    novaGlosa = glosaAntiga + valorGlosaInformada;
                }
                else // substituir
                {
                    novaGlosa = valorGlosaInformada;
                }

                // [DOC] Validar se glosa não excede o valor da NF
                if (novaGlosa > (notaFiscal.ValorNF ?? 0))
                {
                    return Json(new
                    {
                        success = false,
                        message = $"O valor da glosa (R$ {novaGlosa:N2}) não pode ser maior que o valor da Nota Fiscal (R$ {notaFiscal.ValorNF:N2})"
                    });
                }

                // [DOC] Calcular diferença para ajustar o saldo do empenho
                // A glosa AUMENTA o saldo (devolve dinheiro ao empenho)
                var diferencaGlosa = novaGlosa - glosaAntiga;

                // [DOC] Atualizar nota fiscal
                notaFiscal.ValorGlosa = novaGlosa;
                notaFiscal.MotivoGlosa = glosanota.MotivoGlosa;
                _unitOfWork.NotaFiscal.Update(notaFiscal);

                // [DOC] Atualizar saldo do empenho
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                    u.EmpenhoId == notaFiscal.EmpenhoId
                );

                if (empenho != null)
                {
                    // [DOC] Glosa aumenta o saldo (devolve o valor ao empenho)
                    empenho.SaldoFinal = empenho.SaldoFinal + diferencaGlosa;
                    _unitOfWork.Empenho.Update(empenho);
                }

                _unitOfWork.Save();

                var mensagem = glosanota.ModoGlosa == "somar"
                    ? $"Glosa somada com sucesso! Valor total: R$ {novaGlosa:N2}"
                    : $"Glosa atualizada com sucesso! Novo valor: R$ {novaGlosa:N2}";

                return Json(new
                {
                    success = true,
                    message = mensagem,
                    novaGlosa = novaGlosa,
                    novaGlosaFormatada = novaGlosa.ToString("N2")
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Glosa", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao realizar glosa: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EmpenhoList
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar empenhos vinculados ao contrato informado.
         *
         * 📥 ENTRADAS     : id (Guid) - ContratoId.
         *
         * 📤 SAÍDAS       : JSON com lista de empenhos.
         *
         * 🔗 CHAMADA POR  : Formulários de NF (seleção de empenho).
         *
         * 🔄 CHAMA        : Empenho.GetAll().
         ****************************************************************************************/
        [Route("EmpenhoList")]
        public JsonResult EmpenhoList(Guid id)
        {
            try
            {
                var EmpenhoList = _unitOfWork.Empenho.GetAll().Where(e => e.ContratoId == id);
                EmpenhoList = EmpenhoList.OrderByDescending(e => e.NotaEmpenho).ToList();
                return new JsonResult(new
                {
                    data = EmpenhoList
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoList", error);
                return new JsonResult(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EmpenhoListAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar empenhos vinculados à ata informada.
         *
         * 📥 ENTRADAS     : id (Guid) - AtaId.
         *
         * 📤 SAÍDAS       : JSON com lista de empenhos.
         *
         * 🔗 CHAMADA POR  : Formulários de NF (seleção de empenho para ata).
         *
         * 🔄 CHAMA        : Empenho.GetAll().
         ****************************************************************************************/
        [Route("EmpenhoListAta")]
        public JsonResult EmpenhoListAta(Guid id)
        {
            try
            {
                var EmpenhoList = _unitOfWork.Empenho.GetAll().Where(e => e.AtaId == id);
                EmpenhoList = EmpenhoList.OrderByDescending(e => e.NotaEmpenho);
                return new JsonResult(new
                {
                    data = EmpenhoList
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoListAta", error);
                return new JsonResult(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter contrato associado a um empenho.
         *
         * 📥 ENTRADAS     : id (Guid) - EmpenhoId.
         *
         * 📤 SAÍDAS       : JSON com contrato encontrado.
         *
         * 🔗 CHAMADA POR  : Tela de NF (carregar contrato).
         *
         * 🔄 CHAMA        : Empenho.GetFirstOrDefault(), Contrato.GetFirstOrDefault().
         ****************************************************************************************/
        [Route("GetContrato")]
        public JsonResult GetContrato(Guid id)
        {
            try
            {
                var objContrato = _unitOfWork.Contrato.GetAll().Where(c => c.ContratoId == id);
                return new JsonResult(new
                {
                    data = objContrato
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetContrato", error);
                return new JsonResult(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: NFContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar notas fiscais vinculadas a um contrato.
         *
         * 📥 ENTRADAS     : id (Guid) - ContratoId.
         *
         * 📤 SAÍDAS       : JSON com notas fiscais.
         *
         * 🔗 CHAMADA POR  : Consultas de NFs por contrato.
         *
         * 🔄 CHAMA        : NotaFiscal.GetAll().
         ****************************************************************************************/
        [Route("NFContratos")]
        public IActionResult NFContratos(Guid id)
        {
            try
            {
                var NFList = (
                    from nf in _unitOfWork.NotaFiscal.GetAll()
                    orderby nf.NumeroNF descending
                    where nf.ContratoId == id
                    select new
                    {
                        nf.NotaFiscalId,
                        nf.NumeroNF,
                        nf.Objeto,
                        nf.TipoNF,
                        DataFormatada = nf.DataEmissao?.ToString("dd/MM/yyyy"),
                        ValorNFFormatado = nf.ValorNF?.ToString("C"),
                        ValorGlosaFormatado = nf.ValorGlosa?.ToString("C"),
                        nf.MotivoGlosa,
                        nf.ContratoId,
                        nf.EmpenhoId,
                    }
                ).ToList();

                return Json(new
                {
                    data = NFList
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFContratos", error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: NFEmpenhos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar notas fiscais vinculadas a um empenho.
         *
         * 📥 ENTRADAS     : id (Guid) - EmpenhoId.
         *
         * 📤 SAÍDAS       : JSON com notas fiscais do empenho.
         *
         * 🔗 CHAMADA POR  : Consultas de NFs por empenho.
         *
         * 🔄 CHAMA        : NotaFiscal.GetAll().
         ****************************************************************************************/
        [Route("NFEmpenhos")]
        public IActionResult NFEmpenhos(Guid id)
        {
            try
            {
                var NFList = (
                    from nf in _unitOfWork.NotaFiscal.GetAll()
                    orderby nf.NumeroNF descending
                    where nf.EmpenhoId == id
                    select new
                    {
                        nf.NotaFiscalId,
                        nf.NumeroNF,
                        nf.Objeto,
                        nf.TipoNF,
                        DataFormatada = nf.DataEmissao?.ToString("dd/MM/yyyy"),
                        ValorNFFormatado = nf.ValorNF?.ToString("C"),
                        ValorGlosaFormatado = nf.ValorGlosa?.ToString("C"),
                        nf.MotivoGlosa,
                        nf.ContratoId,
                        nf.EmpenhoId,
                    }
                ).ToList();

                return Json(new
                {
                    data = NFList
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFEmpenhos", error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
    }

    public class GlosaNota
    {
        [Key]
        public Guid NotaFiscalId { get; set; }

        public double? ValorGlosa { get; set; }

        public string? MotivoGlosa { get; set; }

        public string? ModoGlosa { get; set; } // "somar" ou "substituir"
    }
}
