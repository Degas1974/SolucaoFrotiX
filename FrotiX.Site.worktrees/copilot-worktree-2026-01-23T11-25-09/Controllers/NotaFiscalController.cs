/*
 *  _______________________________________________________
 * |                                                       |
 * |                FROTIX - SOLUÇÃO 2026                  |
 * |          ___________________________________          |
 * |                                                       |
 * |   FrotiX Core - Gestão de Notas Fiscais (Core Stack)  |
 * |_______________________________________________________|
 *
 * (IA) Controlador parcial para gestão de Notas Fiscais, empenhos
 * e liquidação financeira de serviços prestados.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: NotaFiscalController                                              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Notas Fiscais.                                  ║
    /// ║    Controla cadastro, validação e manipulação de NFs associadas a Empenhos.  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/NotaFiscal                                            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class NotaFiscalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: NotaFiscalController (Construtor)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public NotaFiscalController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NotaFiscalController", ex);
            }
        }

        [HttpGet]
        public void Get()
        {
            try
            {
                // [INFO] Endpoint reservado (sem implementação).
            }
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.Get", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Get", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove a NF e restaura o saldo do Empenho associado.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (NotaFiscalViewModel): Dados com ID da NF.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(NotaFiscalViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.NotaFiscalId != Guid.Empty)
                {
                    // [DADOS] Carrega NF.
                    var objFromDb = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                        u.NotaFiscalId == model.NotaFiscalId
                    );
                    if (objFromDb != null)
                    {
                        // [DADOS] Carrega empenho associado.
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                            u.EmpenhoId == objFromDb.EmpenhoId
                        );
                        if (empenho != null)
                        {
                            // [REGRA] Ao excluir NF, devolver valor líquido ao empenho.
                            double valorEstornado = (objFromDb.ValorNF ?? 0) - (objFromDb.ValorGlosa ?? 0);
                            empenho.SaldoFinal = empenho.SaldoFinal + valorEstornado;
                            _unitOfWork.Empenho.Update(empenho);
                            
                            // [LOG] Registro de estorno.
                            _log.Info($"NotaFiscalController.Delete: NF {objFromDb.NumeroNF} removida. Valor estornado ao empenho ID {empenho.EmpenhoId}: {valorEstornado:C}");
                        }

                        // [ACAO] Remove NF e persiste.
                        _unitOfWork.NotaFiscal.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(new
                        {
                            success = true,
                            message = "Nota Fiscal removida com sucesso"
                        });
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar Nota Fiscal"
                });
            }
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.Delete", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Delete", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar Nota Fiscal"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetGlosa (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna os dados atuais de glosa de uma NF específica.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID da Nota Fiscal.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados da glosa.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("GetGlosa")]
        [HttpGet]
        public IActionResult GetGlosa(Guid id)
        {
            try
            {
                // [DADOS] Busca NF por ID.
                var notaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                    u.NotaFiscalId == id
                );

                if (notaFiscal == null)
                {
                    // [RETORNO] NF não encontrada.
                    return Json(new
                    {
                        success = false,
                        message = "Nota Fiscal não encontrada"
                    });
                }

                // [RETORNO] Dados da glosa.
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.GetGlosa", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetGlosa", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao buscar dados da glosa"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Glosa (POST)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Registra glosa em uma NF e recalcula o saldo do empenho.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • glosanota (GlosaNota): Dados da glosa.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Glosa")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Glosa([FromBody] GlosaNota glosanota)
        {
            try
            {
                // [DADOS] Buscar nota fiscal.
                var notaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u =>
                    u.NotaFiscalId == glosanota.NotaFiscalId
                );

                if (notaFiscal == null)
                {
                    // [RETORNO] NF não encontrada.
                    return Json(new
                    {
                        success = false,
                        message = "Nota Fiscal não encontrada"
                    });
                }

                // [CALCULO] Valor da glosa informada (converter de centavos se necessário).
                var valorGlosaInformada = glosanota.ValorGlosa ?? 0;
                
                // [REGRA] Se valor parece estar em centavos, dividir por 100.
                if (valorGlosaInformada > 100 && valorGlosaInformada > (notaFiscal.ValorNF ?? 0) * 1.5)
                {
                    valorGlosaInformada = valorGlosaInformada / 100;
                }

                // [DADOS] Glosa antiga.
                var glosaAntiga = notaFiscal.ValorGlosa ?? 0;

                // [CALCULO] Calcular nova glosa baseado no modo.
                double novaGlosa;
                if (glosanota.ModoGlosa == "somar")
                {
                    novaGlosa = glosaAntiga + valorGlosaInformada;
                }
                else // substituir
                {
                    novaGlosa = valorGlosaInformada;
                }

                // [VALIDACAO] Glosa não pode exceder o valor da NF.
                if (novaGlosa > (notaFiscal.ValorNF ?? 0))
                {
                    return Json(new
                    {
                        success = false,
                        message = $"O valor da glosa (R$ {novaGlosa:N2}) não pode ser maior que o valor da Nota Fiscal (R$ {notaFiscal.ValorNF:N2})"
                    });
                }

                // Calcular diferença para ajustar o saldo do empenho
                // A glosa AUMENTA o saldo (devolve dinheiro ao empenho)
                var diferencaGlosa = novaGlosa - glosaAntiga;

                // Atualizar nota fiscal
                notaFiscal.ValorGlosa = novaGlosa;
                notaFiscal.MotivoGlosa = glosanota.MotivoGlosa;
                _unitOfWork.NotaFiscal.Update(notaFiscal);

                // Atualizar saldo do empenho
                var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                    u.EmpenhoId == notaFiscal.EmpenhoId
                );

                if (empenho != null)
                {
                    // Glosa aumenta o saldo (devolve o valor ao empenho)
                    empenho.SaldoFinal = empenho.SaldoFinal + diferencaGlosa;
                    _unitOfWork.Empenho.Update(empenho);
                    
                    _log.Info($"NotaFiscalController.Glosa: Glosa registrada na NF {notaFiscal.NumeroNF}. Modo: {glosanota.ModoGlosa}. Diferença Saldo Empenho: {diferencaGlosa:C}");
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.Glosa", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "Glosa", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao realizar glosa: " + ex.Message
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Listar Empenhos por Contrato
        /// │ DESCRIÇÃO: Retorna lista de empenhos associados a um contrato específico.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.EmpenhoList", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoList", ex);
                return new JsonResult(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Listar Empenhos por Ata
        /// │ DESCRIÇÃO: Retorna lista de empenhos associados a uma Ata específica.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.EmpenhoListAta", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "EmpenhoListAta", ex);
                return new JsonResult(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Buscar Contrato
        /// │ DESCRIÇÃO: Retorna dados de um contrato específico.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.GetContrato", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "GetContrato", ex);
                return new JsonResult(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Listar NFs por Contrato
        /// │ DESCRIÇÃO: Retorna lista de NFs associadas a um contrato.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.NFContratos", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFContratos", ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Listar NFs por Empenho
        /// │ DESCRIÇÃO: Retorna lista de NFs associadas a um empenho.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
            catch (Exception ex)
            {
                _log.Error("NotaFiscalController.NFEmpenhos", ex);
                Alerta.TratamentoErroComLinha("NotaFiscalController.cs", "NFEmpenhos", ex);
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
