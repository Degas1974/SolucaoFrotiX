/*
 *  _______________________________________________________
 * |                                                       |
 * |   FrotiX Core - Gestão de Multas (Core Stack)         |
 * |_______________________________________________________|
 *
 * (IA) Controlador responsável pelo ciclo completo de multas:
 * Identificação, Notificação, Recurso e Liquidação Financeira.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MultaController                                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API principal para gestão de Multas de Trânsito.                           ║
    /// ║    Ciclo completo: Importação, Identificação, Pagamento e Recursos.          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Multa                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MultaController : Controller
    {
        [BindProperty]
        public MovimentacaoEmpenhoMultaViewModel MovimentacaoObj { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MultaController (Construtor)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public MultaController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaController.cs", "MultaController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Test (GET)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Endpoint simples para verificar saúde do controlador.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("Test")]
        public IActionResult Test()
        {
            try
            {
                // [RETORNO] Saúde do endpoint.
                return Ok(
                    new
                    {
                        success = true ,
                        message = "MultaPdfViewer está funcionando!"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "Test");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Test" , error);
                return BadRequest(new
                {
                    success = false
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaMultas (GET)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna multas filtradas por critérios (Fase, Veículo, etc.).            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Fase (string): Fase da multa.                                           ║
        /// ║    • Veiculo (string): ID do veículo.                                        ║
        /// ║    • Orgao (string): ID do órgão autuante.                                   ║
        /// ║    • Motorista (string): ID do motorista.                                    ║
        /// ║    • Infracao (string): ID da infração.                                      ║
        /// ║    • Status (string): Status da multa.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ListaMultas")]
        [HttpGet]
        public IActionResult ListaMultas(
            string Fase = null ,
            string Veiculo = null ,
            string Orgao = null ,
            string Motorista = null ,
            string Infracao = null ,
            string Status = null
        )
        {
            try
            {
                // [DADOS] Consulta view de multas por fase.
                var result = (
                    from vm in _unitOfWork.viewMultas.GetAll()
                    where vm.Fase == Fase
                    select new
                    {
                        fase = vm.Fase ,
                        multaId = vm.MultaId ,
                        numInfracao = vm.NumInfracao ,
                        data = vm.Data ,
                        hora = vm.Hora ,
                        nome = vm.Nome ,
                        telefone = vm.Telefone ,
                        motoristaId = vm.MotoristaId ,
                        placa = vm.Placa ,
                        veiculoId = vm.VeiculoId ,
                        sigla = vm.Sigla ,
                        orgaoAutuanteId = vm.OrgaoAutuanteId ,
                        localizacao = vm.Localizacao ,
                        artigo = vm.Artigo ,
                        vencimento = vm.Vencimento ,
                        tipoMultaId = vm.TipoMultaId ,
                        valorAteVencimento = vm.ValorAteVencimento?.ToString("C") ,
                        valorPosVencimento = vm.ValorPosVencimento?.ToString("C") ,
                        processoEDoc = vm.ProcessoEDoc ,
                        status = vm.Status ,
                        descricao = Servicos.ConvertHtml(vm.Descricao) ,
                        observacao = Servicos.ConvertHtml(vm.Observacao) ,
                        paga = vm.Paga ,
                        habilitado = vm.Paga == true
                            ? ""
                            : "data-toggle='modal' data-target='#modalRegistraPagamento'" ,
                        tooltip = vm.Paga == true
                            ? "Pagamento já Registrado"
                            : "Registra Pagamento" ,
                        dataPagamento = vm.DataPagamento != null ? vm.DataPagamento : "" ,
                        valorPago = vm.ValorPago != null ? vm.ValorPago?.ToString("C") : "" ,
                        autuacaoPDF = vm.AutuacaoPDF ?? "" ,
                        penalidadePDF = vm.PenalidadePDF ?? "" ,
                        comprovantePDF = vm.ComprovantePDF ?? ""
                    }
                );

                // [FILTRO] Aplica filtros dinâmicos.
                var filtro = result.AsQueryable();

                if (!string.IsNullOrEmpty(Motorista))
                {
                    filtro = filtro.Where(m => m.motoristaId == Guid.Parse(Motorista));
                }

                if (!string.IsNullOrEmpty(Orgao))
                {
                    filtro = filtro.Where(o => o.orgaoAutuanteId == Guid.Parse(Orgao));
                }

                if (!string.IsNullOrEmpty(Veiculo))
                {
                    filtro = filtro.Where(v => v.veiculoId == Guid.Parse(Veiculo));
                }

                if (!string.IsNullOrEmpty(Infracao))
                {
                    filtro = filtro.Where(t => t.tipoMultaId == Guid.Parse(Infracao));
                }

                if (!string.IsNullOrEmpty(Status))
                {
                    filtro = filtro.Where(t => t.status == Status);
                }

                // [RETORNO] Lista filtrada.
                return Json(new
                {
                    data = filtro.ToList()
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "ListaMultas");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaMultas" , error);
                return Json(new
                {
                    data = new List<object>() ,
                    error = "Erro ao carregar dados"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaTipoMulta (GET)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna lista de naturezas/tipos de infrações cadastradas.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com tipos de multa.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("PegaTipoMulta")]
        [HttpGet]
        public IActionResult PegaTipoMulta()
        {
            try
            {
                // [DADOS] Lista tipos de multa.
                var result = (
                    from tm in _unitOfWork.TipoMulta.GetAll()
                    select new
                    {
                        tm.TipoMultaId ,
                        tm.Artigo ,
                        Denatran = tm.CodigoDenatran + " / " + tm.Desdobramento ,
                        tm.Descricao ,
                        tm.Infracao ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "PegaTipoMulta");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaTipoMulta" , error);
                return Json(new
                {
                    data = new List<object>() ,
                    error = "Erro ao carregar tipos de multa"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: PegaOrgaoAutuante                                                                          |
        /// | Descrição: Retorna a lista de órgãos autuantes (Detran, PRF, etc).                               |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaOrgaoAutuante")]
        [HttpGet]
        public IActionResult PegaOrgaoAutuante()
        {
            try
            {
                var objOrgaoAutuante = _unitOfWork.OrgaoAutuante.GetAll();

                return Json(new
                {
                    data = objOrgaoAutuante
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "PegaOrgaoAutuante");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaOrgaoAutuante" , error);
                return Json(new
                {
                    data = new List<object>() ,
                    error = "Erro ao carregar órgãos autuantes"
                });
            }
        }

        public class TipoMultaAjax
        {
            public Guid TipoMultaId
            {
                get; set;
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: DeleteTipoMulta                                                                            |
        /// | Descrição: Remove uma natureza de infração do sistema.                                           |
        /// | Parâmetros: TipoMultaAjax model                                                                  |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("DeleteTipoMulta")]
        [HttpPost]
        public IActionResult DeleteTipoMulta(TipoMultaAjax model)
        {
            try
            {
                if (model != null && model.TipoMultaId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.TipoMulta.GetFirstOrDefault(u =>
                        u.TipoMultaId == model.TipoMultaId
                    );
                    if (objFromDb != null)
                    {
                        _unitOfWork.TipoMulta.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Infração removida com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Infração"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "DeleteTipoMulta");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "DeleteTipoMulta" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Infração"
                });
            }
        }

        public class OrgaoAutuanteAjax
        {
            public Guid OrgaoAutuanteId
            {
                get; set;
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: DeleteOrgaoAutuante                                                                        |
        /// | Descrição: Remove um órgão autuante do sistema.                                                  |
        /// | Parâmetros: OrgaoAutuanteAjax Orgao                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("DeleteOrgaoAutuante")]
        [HttpPost]
        public IActionResult DeleteOrgaoAutuante(OrgaoAutuanteAjax Orgao)
        {
            try
            {
                var objFromDb = _unitOfWork.OrgaoAutuante.GetFirstOrDefault(u =>
                    u.OrgaoAutuanteId == Orgao.OrgaoAutuanteId
                );
                if (objFromDb != null)
                {
                    _unitOfWork.OrgaoAutuante.Remove(objFromDb);
                    _unitOfWork.Save();
                    return Json(
                        new
                        {
                            success = true ,
                            message = "Órgão Autuante removido com sucesso"
                        }
                    );
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Órgão Autuante"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "DeleteOrgaoAutuante");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "DeleteOrgaoAutuante" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Órgão Autuante"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: PegaEmpenhos                                                                               |
        /// | Descrição: Retorna a lista de empenhos financeiros para um órgão autuante.                       |
        /// | Parâmetros: Guid Id                                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaEmpenhos")]
        [HttpGet]
        public IActionResult PegaEmpenhos(Guid Id)
        {
            try
            {
                var objEmpenhoMulta = (
                    from vem in _unitOfWork.ViewEmpenhoMulta.GetAll()
                    where vem.OrgaoAutuanteId == Id
                    select new
                    {
                        vem.EmpenhoMultaId ,
                        vem.NotaEmpenho ,
                        vem.AnoVigencia ,
                        SaldoInicialFormatado = vem.SaldoInicial?.ToString("C") ,
                        SaldoAtualFormatado = vem.SaldoAtual?.ToString("C") ,
                        SaldoMovimentacaoFormatado = vem.SaldoMovimentacao?.ToString("C") ,
                        SaldoMultaFormatado = vem.SaldoMultas?.ToString("C") ,
                    }
                ).ToList();

                if (objEmpenhoMulta != null)
                {
                    return Json(new
                    {
                        data = objEmpenhoMulta
                    });
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao recuperar Empenhos"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "PegaEmpenhos");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaEmpenhos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao recuperar Empenhos"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: Delete                                                                                     |
        /// | Descrição: Remove uma notificação de autuação, revertendo saldos financeiros se necessário.      |
        /// | Parâmetros: MultaViewModel model                                                                 |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(MultaViewModel model)
        {
            try
            {
                if (model != null && model.MultaId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Multa.GetFirstOrDefault(u =>
                        u.MultaId == model.MultaId
                    );
                    if (objFromDb != null)
                    {
                        if ((bool)(objFromDb.Paga != null))
                        {
                            var objEmpenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(m =>
                                m.EmpenhoMultaId == objFromDb.EmpenhoMultaId
                            );
                            objEmpenhoMulta.SaldoAtual =
                                objEmpenhoMulta.SaldoAtual + objFromDb.ValorPago;
                            _unitOfWork.EmpenhoMulta.Update(objEmpenhoMulta);

                            MovimentacaoObj = new MovimentacaoEmpenhoMultaViewModel
                            {
                                MovimentacaoEmpenhoMulta = new Models.MovimentacaoEmpenhoMulta() ,
                            };
                            MovimentacaoObj.MovimentacaoEmpenhoMulta.Descricao =
                                objFromDb.NumInfracao;
                            MovimentacaoObj.MovimentacaoEmpenhoMulta.TipoMovimentacao = "P";
                            MovimentacaoObj.MovimentacaoEmpenhoMulta.Valor = objFromDb.ValorPago;
                            MovimentacaoObj.MovimentacaoEmpenhoMulta.DataMovimentacao =
                                objFromDb.DataPagamento;
                            MovimentacaoObj.MovimentacaoEmpenhoMulta.MultaId = objFromDb.MultaId;
                            MovimentacaoObj.MovimentacaoEmpenhoMulta.EmpenhoMultaId = (Guid)
                                objFromDb.EmpenhoMultaId;
                            _unitOfWork.MovimentacaoEmpenhoMulta.Add(
                                MovimentacaoObj.MovimentacaoEmpenhoMulta
                            );
                        }

                        _unitOfWork.Save();

                        _unitOfWork.Multa.Remove(objFromDb);

                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Notificação de Autuação removida com sucesso" ,
                            }
                        );
                    }
                }
                return Json(
                    new
                    {
                        success = false ,
                        message = "Erro ao apagar Notificação de Autuação"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "Delete");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Delete" , error);
                return Json(
                    new
                    {
                        success = false ,
                        message = "Erro ao apagar Notificação de Autuação"
                    }
                );
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: TransformaPenalidade                                                                       |
        /// | Descrição: Evolui a fase da multa de Autuação para Penalidade, registrando prazos e valores.     |
        /// | Parâmetros: string MultaId, string DataVencimento, string ValorAteVencimento...                  |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("TransformaPenalidade")]
        [HttpGet]
        public IActionResult TransformaPenalidade(
            string MultaId ,
            string DataVencimento ,
            string ValorAteVencimento ,
            string Observacao ,
            string PenalidadePDF ,
            string ProcessoEDoc
        )
        {
            try
            {
                // Validações de entrada
                if (string.IsNullOrEmpty(MultaId))
                {
                    return Json(new
                    {
                        success = false ,
                        message = "MultaId não pode ser vazio"
                    });
                }

                if (string.IsNullOrEmpty(DataVencimento))
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Data de Vencimento é obrigatória"
                    });
                }

                if (string.IsNullOrEmpty(ValorAteVencimento))
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Valor até Vencimento é obrigatório"
                    });
                }

                var objFromDb = _unitOfWork.Multa.GetFirstOrDefault(u =>
                    u.MultaId == Guid.Parse(MultaId)
                );

                if (objFromDb == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Multa não encontrada no banco de dados"
                    });
                }

                // Converte data
                if (!DateTime.TryParse(DataVencimento , out DateTime dataVencimento))
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Data de Vencimento inválida"
                    });
                }

                // Converte valor - remove pontos e troca vírgula por ponto
                string valorLimpo = ValorAteVencimento.Replace("." , "").Replace("," , ".");
                if (!double.TryParse(valorLimpo , System.Globalization.NumberStyles.Any , System.Globalization.CultureInfo.InvariantCulture , out double valorParsed))
                {
                    return Json(new
                    {
                        success = false ,
                        message = $"Valor inválido: {ValorAteVencimento}"
                    });
                }

                // Atualiza objeto
                objFromDb.Vencimento = dataVencimento;
                objFromDb.ValorAteVencimento = valorParsed;
                objFromDb.Observacao = Observacao ?? "";
                objFromDb.PenalidadePDF = PenalidadePDF ?? "";
                objFromDb.ProcessoEDoc = ProcessoEDoc ?? "";
                objFromDb.Status = "À Pagar";
                objFromDb.Fase = "Penalidade";

                _unitOfWork.Multa.Update(objFromDb);
                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Notificação de Autuação transformada em Penalidade com sucesso" ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "TransformaPenalidade");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "TransformaPenalidade" , error);
                return Json(
                    new
                    {
                        success = false ,
                        message = $"Erro ao transformar: {error.Message}"
                    }
                );
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ProcuraViagem                                                                              |
        /// | Descrição: Localiza a viagem correspondente à data/hora de uma multa para identificar o condutor.|
        /// | Parâmetros: string Data, string Hora, Guid VeiculoId                                             |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ProcuraViagem")]
        [HttpPost]
        public IActionResult ProcuraViagem([FromForm] string Data, [FromForm] string Hora, [FromForm] Guid VeiculoId)
        {
            try
            {
                if (VeiculoId != Guid.Empty)
                {
                    DateTime DataAutuacao = DateTime.Parse(Data);
                    DateTime HoraAutuacao = DateTime.Parse(Hora);

                    var objFromDb = _unitOfWork.ViewProcuraFicha.GetAll(v =>
                        v.VeiculoId == VeiculoId
                        && (
                            (v.DataInicial <= DataAutuacao && v.DataFinal >= DataAutuacao)
                            || (v.DataInicial <= DataAutuacao && v.DataFinal == null)
                        )
                    );

                    if (objFromDb != null)
                    {
                        foreach (var viagem in objFromDb)
                        {
                            Console.WriteLine(viagem.NoFichaVistoria);

                            if (viagem.DataFinal == null)
                            {
                                return Json(
                                    new
                                    {
                                        success = true ,
                                        message = "Viagem encontrada com sucesso!" ,
                                        nofichavistoria = viagem.NoFichaVistoria ,
                                        motoristaid = viagem.MotoristaId ,
                                    }
                                );
                            }

                            if (viagem.DataInicial < viagem.DataFinal)
                            {
                                if (DataAutuacao > viagem.DataInicial)
                                {
                                    if (DataAutuacao < viagem.DataFinal)
                                    {
                                        return Json(
                                            new
                                            {
                                                success = true ,
                                                message = "Viagem encontrada com sucesso!" ,
                                                nofichavistoria = viagem.NoFichaVistoria ,
                                                motoristaid = viagem.MotoristaId ,
                                            }
                                        );
                                    }
                                    else
                                    {
                                        if (HoraAutuacao <= DateTime.Parse(viagem.HoraFim))
                                        {
                                            return Json(
                                                new
                                                {
                                                    success = true ,
                                                    message = "Viagem encontrada com sucesso!" ,
                                                    nofichavistoria = viagem.NoFichaVistoria ,
                                                    motoristaid = viagem.MotoristaId ,
                                                }
                                            );
                                        }
                                    }
                                }
                                else
                                {
                                    if (HoraAutuacao >= DateTime.Parse(viagem.HoraInicio))
                                    {
                                        return Json(
                                            new
                                            {
                                                success = true ,
                                                message = "Viagem encontrada com sucesso!" ,
                                                nofichavistoria = viagem.NoFichaVistoria ,
                                                motoristaid = viagem.MotoristaId ,
                                            }
                                        );
                                    }
                                }
                            }
                            else
                            {
                                if (
                                    HoraAutuacao >= DateTime.Parse(viagem.HoraInicio)
                                    && HoraAutuacao <= DateTime.Parse(viagem.HoraFim)
                                )
                                {
                                    return Json(
                                        new
                                        {
                                            success = true ,
                                            message = "Viagem encontrada com sucesso!" ,
                                            nofichavistoria = viagem.NoFichaVistoria ,
                                            motoristaid = viagem.MotoristaId ,
                                        }
                                    );
                                }
                            }
                        }
                        return Json(
                            new
                            {
                                success = false ,
                                message = "Não foi encontrada viagem para essa multa!" ,
                                nofichavistoria = 0 ,
                            }
                        );
                    }
                    return Json(
                        new
                        {
                            success = false ,
                            message = "Não foi encontrada viagem para essa multa!" ,
                            nofichavistoria = 0 ,
                        }
                    );
                }
                return Json(
                    new
                    {
                        success = false ,
                        message = "Não foi encontrada viagem para essa multa!"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "ProcuraViagem");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ProcuraViagem" , error);
                return Json(
                    new
                    {
                        success = false ,
                        message = "Erro ao procurar viagem"
                    }
                );
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ProcuraFicha                                                                               |
        /// | Descrição: Busca uma viagem específica a partir do número da ficha de vistoria.                  |
        /// | Parâmetros: ProcuraViagemViewModel model                                                         |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ProcuraFicha")]
        [HttpPost]
        public IActionResult ProcuraFicha([FromForm] ProcuraViagemViewModel model)
        {
            try
            {
                if (model != null && model.NoFichaVistoria != 0)
                {
                    var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                        v.NoFichaVistoria == model.NoFichaVistoria && v.Status == "Realizada"
                    );

                    if (objFromDb != null)
                    {
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Ficha encontrada com sucesso!" ,
                                viagemid = objFromDb.ViagemId ,
                            }
                        );
                    }

                    return Json(
                        new
                        {
                            success = false ,
                            message = "Não foi encontrada viagem para essa Ficha" ,
                        }
                    );
                }

                return Json(
                    new
                    {
                        success = false ,
                        message = "Não foi encontrada viagem para essa Ficha"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "ProcuraFicha");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ProcuraFicha" , error);
                return Json(
                    new
                    {
                        success = false ,
                        message = "Erro ao procurar ficha"
                    }
                );
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: PegaImagemFichaVistoria                                                                    |
        /// | Descrição: Recupera a imagem da ficha de vistoria associada a uma viagem e converte para Base64. |
        /// | Parâmetros: int noFicha                                                                          |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaImagemFichaVistoria")]
        [HttpGet]
        public IActionResult PegaImagemFichaVistoria(int noFicha)
        {
            try
            {
                if (noFicha == 0)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Número da ficha não informado"
                    });
                }

                // Busca a viagem pelo número da ficha
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.NoFichaVistoria == noFicha && v.Status == "Realizada"
                );

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Viagem não encontrada para esta ficha"
                    });
                }

                // Verifica se existe imagem da ficha
                if (viagem.FichaVistoria == null || viagem.FichaVistoria.Length == 0)
                {
                    return Json(new
                    {
                        success = false ,
                        message = "Esta viagem não possui imagem da Ficha de Vistoria"
                    });
                }

                // Converte a imagem para base64
                string base64Image = Convert.ToBase64String(viagem.FichaVistoria);

                // Detecta o tipo de imagem (assume PNG por padrão)
                string mimeType = "image/png";
                if (viagem.FichaVistoria.Length > 2)
                {
                    // JPEG começa com FF D8
                    if (viagem.FichaVistoria[0] == 0xFF && viagem.FichaVistoria[1] == 0xD8)
                    {
                        mimeType = "image/jpeg";
                    }
                    // PNG começa com 89 50 4E 47
                    else if (viagem.FichaVistoria[0] == 0x89 && viagem.FichaVistoria[1] == 0x50)
                    {
                        mimeType = "image/png";
                    }
                    // GIF começa com 47 49 46
                    else if (viagem.FichaVistoria[0] == 0x47 && viagem.FichaVistoria[1] == 0x49)
                    {
                        mimeType = "image/gif";
                    }
                }

                return Json(new
                {
                    success = true ,
                    message = "Imagem encontrada" ,
                    noFichaVistoria = viagem.NoFichaVistoria ,
                    viagemId = viagem.ViagemId ,
                    imagemBase64 = $"data:{mimeType};base64,{base64Image}"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "PegaImagemFichaVistoria");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaImagemFichaVistoria" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao buscar imagem da ficha: " + error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnGetMultaExistente                                                                        |
        /// | Descrição: Verifica se um número de infração já está cadastrado no sistema.                      |
        /// | Parâmetros: string NumInfracao                                                                   |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("MultaExistente")]
        [HttpGet]
        public JsonResult OnGetMultaExistente(string NumInfracao)
        {
            try
            {
                var objMulta = _unitOfWork.Multa.GetFirstOrDefault(m =>
                    m.NumInfracao == NumInfracao
                );

                if (objMulta == null)
                {
                    return new JsonResult(new
                    {
                        data = false
                    });
                }

                return new JsonResult(new
                {
                    data = true
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnGetMultaExistente");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnGetMultaExistente" , error);
                return new JsonResult(new
                {
                    data = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostAlteraStatus                                                                         |
        /// | Descrição: Altera manualmente o status de uma multa.                                             |
        /// | Parâmetros: string MultaId, string Status                                                        |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("AlteraStatus")]
        [HttpGet]
        public JsonResult OnPostAlteraStatus(string MultaId , string Status)
        {
            try
            {
                var objMulta = _unitOfWork.Multa.GetFirstOrDefault(m =>
                    m.MultaId == Guid.Parse(MultaId)
                );

                if (objMulta == null)
                {
                    return new JsonResult(
                        new
                        {
                            success = false ,
                            message = "Não foi possível alterar o Status!"
                        }
                    );
                }

                objMulta.Status = Status;
                _unitOfWork.Multa.Update(objMulta);
                _unitOfWork.Save();

                return new JsonResult(
                    new
                    {
                        success = true ,
                        message = "Status Alterado com sucesso!"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostAlteraStatus");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostAlteraStatus" , error);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao alterar status"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostPegaStatus                                                                           |
        /// | Descrição: Recupera o status atual e informações básicas de uma multa.                           |
        /// | Parâmetros: string Id                                                                            |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaStatus")]
        [HttpGet]
        public JsonResult OnPostPegaStatus(string Id)
        {
            try
            {
                // Validação: Verifica se Id não é null ou vazio
                if (string.IsNullOrEmpty(Id))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID não fornecido"
                    });
                }

                // Validação: Verifica se é um GUID válido
                if (!Guid.TryParse(Id , out Guid multaId))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var objMulta = _unitOfWork.viewMultas.GetFirstOrDefault(m =>
                    m.MultaId == multaId
                );

                if (objMulta == null)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Multa não encontrada"
                    });
                }

                return new JsonResult(
                    new
                    {
                        success = true ,
                        numInfracao = objMulta.NumInfracao ,
                        data = objMulta.Data ,
                        hora = objMulta.Hora ,
                        nome = objMulta.Nome ,
                        status = objMulta.Status ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaStatus");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaStatus" , error);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao buscar dados"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostPegaInstrumentoVeiculo                                                               |
        /// | Descrição: Identifica qual instrumento (Contrato ou Ata) está vinculado ao veículo.              |
        /// | Parâmetros: string Id                                                                            |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaInstrumentoVeiculo")]
        [HttpGet]
        public JsonResult OnPostPegaInstrumentoVeiculo(string Id)
        {
            try
            {
                // Validação: Verifica se Id não é null ou vazio
                if (string.IsNullOrEmpty(Id))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID não fornecido"
                    });
                }

                // Validação: Verifica se é um GUID válido
                if (!Guid.TryParse(Id , out Guid veiculoId))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var objDbContrato = _unitOfWork.Veiculo.GetFirstOrDefault(m =>
                    m.VeiculoId == veiculoId
                );

                // CORRIGIDO: Verifica null primeiro antes de acessar propriedades
                if (objDbContrato == null)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "Veículo não encontrado"
                    });
                }

                // Prioriza ContratoId
                if (objDbContrato.ContratoId != null)
                {
                    return new JsonResult(
                        new
                        {
                            success = true ,
                            instrumentoid = objDbContrato.ContratoId ,
                            instrumento = "contrato" ,
                        }
                    );
                }

                // Se não tem contrato, verifica AtaId
                if (objDbContrato.AtaId != null)
                {
                    return new JsonResult(
                        new
                        {
                            success = true ,
                            instrumentoid = objDbContrato.AtaId ,
                            instrumento = "ata" ,
                        }
                    );
                }

                // Não tem nem contrato nem ata
                return new JsonResult(new
                {
                    success = false ,
                    message = "Veículo sem contrato ou ata vinculado"
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaInstrumentoVeiculo");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostPegaInstrumentoVeiculo" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao buscar instrumento do veículo"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostValidaContratoVeiculo                                                                |
        /// | Descrição: Valida se a combinação Veículo/Contrato é válida no banco.                             |
        /// | Parâmetros: string veiculoId, string contratoId                                                  |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ValidaContratoVeiculo")]
        [HttpGet]
        public JsonResult OnPostValidaContratoVeiculo(string veiculoId , string contratoId)
        {
            try
            {
                if (veiculoId == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                var objDb = _unitOfWork.Veiculo.GetFirstOrDefault(m =>
                    m.VeiculoId == Guid.Parse(veiculoId) && m.ContratoId == Guid.Parse(contratoId)
                );

                if (objDb == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                return new JsonResult(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaContratoVeiculo");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostValidaContratoVeiculo" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostValidaAtaVeiculo                                                                     |
        /// | Descrição: Valida se a combinação Veículo/Ata é válida no banco.                                  |
        /// | Parâmetros: string veiculoId, string ataId                                                       |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ValidaAtaVeiculo")]
        [HttpGet]
        public JsonResult OnPostValidaAtaVeiculo(string veiculoId , string ataId)
        {
            try
            {
                if (veiculoId == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                var objDb = _unitOfWork.Veiculo.GetFirstOrDefault(m =>
                    m.VeiculoId == Guid.Parse(veiculoId) && m.AtaId == Guid.Parse(ataId)
                );

                if (objDb == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                return new JsonResult(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaAtaVeiculo");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostValidaAtaVeiculo" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostPegaContratoMotorista                                                                |
        /// | Descrição: Recupera o Contrato ID vinculado a um motorista.                                      |
        /// | Parâmetros: string Id                                                                            |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaContratoMotorista")]
        [HttpGet]
        public JsonResult OnPostPegaContratoMotorista(string Id)
        {
            try
            {
                // Validação: Verifica se Id não é null ou vazio
                if (string.IsNullOrEmpty(Id))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID não fornecido"
                    });
                }

                // Validação: Verifica se é um GUID válido
                if (!Guid.TryParse(Id , out Guid motoristaId))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var objDb = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == motoristaId
                );

                if (objDb == null || objDb.ContratoId == null)
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        contratoid = ""
                    });
                }

                return new JsonResult(new
                {
                    success = true ,
                    contratoid = objDb.ContratoId
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaContratoMotorista");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostPegaContratoMotorista" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostValidaContratoMotorista                                                              |
        /// | Descrição: Valida se a combinação Motorista/Contrato é válida no banco.                          |
        /// | Parâmetros: string motoristaId, string contratoId                                                |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ValidaContratoMotorista")]
        [HttpGet]
        public JsonResult OnPostValidaContratoMotorista(string motoristaId , string contratoId)
        {
            try
            {
                if (motoristaId == null || contratoId == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                var objDb = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == Guid.Parse(motoristaId)
                    && m.ContratoId == Guid.Parse(contratoId)
                );

                if (objDb == null || objDb.ContratoId == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                return new JsonResult(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostValidaContratoMotorista");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostValidaContratoMotorista" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostPegaValor                                                                            |
        /// | Descrição: Recupera o valor atualizado (até o vencimento) de uma multa.                          |
        /// | Parâmetros: string Id                                                                            |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaValor")]
        [HttpGet]
        public JsonResult OnPostPegaValor(string Id)
        {
            try
            {
                // Validação: Verifica se Id não é null ou vazio
                if (string.IsNullOrEmpty(Id))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID não fornecido"
                    });
                }

                // Validação: Verifica se é um GUID válido
                if (!Guid.TryParse(Id , out Guid multaId))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var objMulta = _unitOfWork.viewMultas.GetFirstOrDefault(m =>
                    m.MultaId == multaId
                );

                if (objMulta == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                return new JsonResult(new
                {
                    success = true ,
                    valor = objMulta.ValorAteVencimento
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaValor");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaValor" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostPegaEmpenhoMultaId                                                                   |
        /// | Descrição: Recupera o ID do empenho vinculado a uma multa.                                       |
        /// | Parâmetros: string Id                                                                            |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaEmpenhoMultaId")]
        [HttpGet]
        public JsonResult OnPostPegaEmpenhoMultaId(string Id)
        {
            try
            {
                // Validação: Verifica se Id não é null ou vazio
                if (string.IsNullOrEmpty(Id))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID não fornecido"
                    });
                }

                // Validação: Verifica se é um GUID válido
                if (!Guid.TryParse(Id , out Guid multaId))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var objMulta = _unitOfWork.Multa.GetFirstOrDefault(m =>
                    m.MultaId == multaId
                );

                if (objMulta == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                return new JsonResult(
                    new
                    {
                        success = true ,
                        empenhoMultaId = objMulta.EmpenhoMultaId
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaEmpenhoMultaId");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostPegaEmpenhoMultaId" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostRegistraPagamento                                                                    |
        /// | Descrição: Registra a quitação de uma multa, baixando o saldo do empenho e gerando movimentação. |
        /// | Parâmetros: string MultaId, string DataPagamento, string ValorPago, string Status...             |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("RegistraPagamento")]
        [HttpGet]
        public JsonResult OnPostRegistraPagamento(
            string MultaId ,
            string DataPagamento ,
            string ValorPago ,
            string Status ,
            string FormaPagamento ,
            String ComprovantePDF ,
            string EmpenhoMultaId
        )
        {
            try
            {
                var objMulta = _unitOfWork.Multa.GetFirstOrDefault(m =>
                    m.MultaId == Guid.Parse(MultaId)
                );

                if (objMulta == null)
                {
                    return new JsonResult(
                        new
                        {
                            success = false ,
                            message = "Não foi possível registrar o pagamento!"
                        }
                    );
                }

                ValorPago = ValorPago.Replace("." , ",");

                objMulta.DataPagamento = DateTime.Parse(DataPagamento);
                objMulta.ValorPago = Double.Parse(ValorPago);
                objMulta.Status = Status;
                objMulta.FormaPagamento = FormaPagamento;
                objMulta.ComprovantePDF = ComprovantePDF;
                objMulta.Paga = true;

                _unitOfWork.Multa.Update(objMulta);

                var objEmpenhoMulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(m =>
                    m.EmpenhoMultaId == Guid.Parse(EmpenhoMultaId)
                );
                objEmpenhoMulta.SaldoAtual = objEmpenhoMulta.SaldoAtual - Double.Parse(ValorPago);
                _unitOfWork.EmpenhoMulta.Update(objEmpenhoMulta);

                MovimentacaoObj = new MovimentacaoEmpenhoMultaViewModel
                {
                    MovimentacaoEmpenhoMulta = new Models.MovimentacaoEmpenhoMulta() ,
                };
                MovimentacaoObj.MovimentacaoEmpenhoMulta.Descricao = objMulta.NumInfracao;
                MovimentacaoObj.MovimentacaoEmpenhoMulta.TipoMovimentacao = "M";
                MovimentacaoObj.MovimentacaoEmpenhoMulta.Valor = Double.Parse(ValorPago);
                MovimentacaoObj.MovimentacaoEmpenhoMulta.DataMovimentacao = DateTime.Parse(
                    DataPagamento
                );
                MovimentacaoObj.MovimentacaoEmpenhoMulta.MultaId = objMulta.MultaId;
                MovimentacaoObj.MovimentacaoEmpenhoMulta.EmpenhoMultaId = (Guid)
                    objMulta.EmpenhoMultaId;
                _unitOfWork.MovimentacaoEmpenhoMulta.Add(MovimentacaoObj.MovimentacaoEmpenhoMulta);

                _unitOfWork.Save();

                return new JsonResult(
                    new
                    {
                        success = true ,
                        message = "Pagamento registrado com sucesso!"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostRegistraPagamento");
                Alerta.TratamentoErroComLinha(
                    "MultaController.cs" ,
                    "OnPostRegistraPagamento" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao registrar pagamento"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: OnPostPegaObservacao                                                                       |
        /// | Descrição: Recupera a observação cadastrada em uma multa.                                        |
        /// | Parâmetros: string Id                                                                            |
        /// | Retorno: JsonResult                                                                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("PegaObservacao")]
        [HttpGet]
        public JsonResult OnPostPegaObservacao(string Id)
        {
            try
            {
                // Validação: Verifica se Id não é null ou vazio
                if (string.IsNullOrEmpty(Id))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID não fornecido"
                    });
                }

                // Validação: Verifica se é um GUID válido
                if (!Guid.TryParse(Id , out Guid multaId))
                {
                    return new JsonResult(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                var objMulta = _unitOfWork.viewMultas.GetFirstOrDefault(m =>
                    m.MultaId == multaId
                );

                if (objMulta == null)
                {
                    return new JsonResult(new
                    {
                        success = false
                    });
                }

                return new JsonResult(
                    new
                    {
                        success = true ,
                        numInfracao = objMulta.NumInfracao ,
                        nomeMotorista = objMulta.Nome ,
                        observacao = objMulta.Observacao ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "OnPostPegaObservacao");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaObservacao" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: MultaEmpenho                                                                               |
        /// | Descrição: Lista todas as multas vinculadas a um determinado empenho.                            |
        /// | Parâmetros: Guid id                                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("MultaEmpenho")]
        public IActionResult MultaEmpenho(Guid id)
        {
            try
            {
                var MultaList = (
                    from m in _unitOfWork.Multa.GetAll()
                    orderby m.Data descending
                    where m.EmpenhoMultaId == id
                    select new
                    {
                        DataFormatada = m.Data?.ToString("dd/MM/yyyy") ,
                        m.NumInfracao ,
                        m.Localizacao ,
                        DataPagamentoFormatada = m.DataPagamento?.ToString("dd/MM/yyyy") ,
                        m.ValorPago ,
                        m.MultaId ,
                    }
                ).ToList();

                return Json(new
                {
                    data = MultaList
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "MultaEmpenho");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaEmpenho" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: MultaEmpenhoPagas                                                                          |
        /// | Descrição: Lista apenas as multas pagas vinculadas a um empenho.                                 |
        /// | Parâmetros: Guid id                                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("MultaEmpenhoPagas")]
        public IActionResult MultaEmpenhoPagas(Guid id)
        {
            try
            {
                var MultaList = (
                    from m in _unitOfWork.Multa.GetAll(m => m.ValorPago != null)
                    orderby m.Data descending
                    where m.EmpenhoMultaId == id
                    select new
                    {
                        DataFormatada = m.Data?.ToString("dd/MM/yyyy") ,
                        m.NumInfracao ,
                        m.Localizacao ,
                        DataPagamentoFormatada = m.DataPagamento?.ToString("dd/MM/yyyy") ,
                        m.ValorPago ,
                        m.MultaId ,
                    }
                ).ToList();

                return Json(new
                {
                    data = MultaList
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "MultaEmpenhoPagas");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaEmpenhoPagas" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: SaldoMultas                                                                                |
        /// | Descrição: Calcula o somatório total pago em multas para um empenho específico.                   |
        /// | Parâmetros: Guid Id                                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("SaldoMultas")]
        public IActionResult SaldoMultas(Guid Id)
        {
            try
            {
                var multas = _unitOfWork.Multa.GetAll(m =>
                    m.EmpenhoMultaId == Id && m.ValorPago != null
                );

                double totalmultas = 0;

                foreach (var multa in multas)
                {
                    totalmultas = (double)(totalmultas + (multa.ValorPago));
                }
                return Json(new
                {
                    saldomultas = totalmultas
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "SaldoMultas");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "SaldoMultas" , error);
                return Json(new
                {
                    saldomultas = 0
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ListaAporte                                                                                |
        /// | Descrição: Lista todos os aportes financeiros realizados em um empenho de multas.                |
        /// | Parâmetros: Guid Id                                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ListaAporte")]
        public IActionResult ListaAporte(Guid Id)
        {
            try
            {
                var result = (
                    from p in _unitOfWork.MovimentacaoEmpenhoMulta.GetAll()
                    where p.TipoMovimentacao == "A"
                    orderby p.DataMovimentacao descending
                    where p.EmpenhoMultaId == Id
                    select new
                    {
                        p.MovimentacaoId ,
                        DataFormatada = p.DataMovimentacao?.ToString("dd/MM/yyyy") ,
                        p.Descricao ,
                        ValorFormatado = p.Valor?.ToString("C") ,
                        ValorOriginal = p.Valor ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "ListaAporte");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaAporte" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ListaAnulacao                                                                              |
        /// | Descrição: Lista todas as anulações financeiras realizadas em um empenho de multas.              |
        /// | Parâmetros: Guid Id                                                                              |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("ListaAnulacao")]
        public IActionResult ListaAnulacao(Guid Id)
        {
            try
            {
                var result = (
                    from p in _unitOfWork.MovimentacaoEmpenhoMulta.GetAll()
                    where p.TipoMovimentacao == "G"
                    orderby p.DataMovimentacao descending
                    where p.EmpenhoMultaId == Id
                    select new
                    {
                        p.MovimentacaoId ,
                        DataFormatada = p.DataMovimentacao?.ToString("dd/MM/yyyy") ,
                        p.Descricao ,
                        ValorFormatado = p.Valor?.ToString("C") ,
                        ValorOriginal = p.Valor ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "ListaAnulacao");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaAnulacao" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: Aporte                                                                                     |
        /// | Descrição: Registra um novo aporte de valor ao saldo do empenho de multas.                       |
        /// | Parâmetros: MovimentacaoEmpenhoMulta movimentacao                                                |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("Aporte")]
        [Consumes("application/json")]
        public IActionResult Aporte([FromBody] MovimentacaoEmpenhoMulta movimentacao)
        {
            try
            {
                movimentacao.Valor = movimentacao.Valor / 100;
                _unitOfWork.MovimentacaoEmpenhoMulta.Add(movimentacao);

                var empenhomulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(u =>
                    u.EmpenhoMultaId == movimentacao.EmpenhoMultaId
                );
                empenhomulta.SaldoAtual = empenhomulta.SaldoAtual + movimentacao.Valor;
                _unitOfWork.EmpenhoMulta.Update(empenhomulta);

                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Aporte realizado com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "Aporte");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Aporte" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao realizar aporte"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: Anulacao                                                                                   |
        /// | Descrição: Registra uma anulação (estorno) de valor no saldo do empenho de multas.               |
        /// | Parâmetros: MovimentacaoEmpenhoMulta movimentacao                                                |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("Anulacao")]
        [Consumes("application/json")]
        public IActionResult Anulacao([FromBody] MovimentacaoEmpenhoMulta movimentacao)
        {
            try
            {
                movimentacao.Valor = (movimentacao.Valor / 100) * -1;
                _unitOfWork.MovimentacaoEmpenhoMulta.Update(movimentacao);

                var empenhomulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(e =>
                    e.EmpenhoMultaId == movimentacao.EmpenhoMultaId
                );
                empenhomulta.SaldoAtual = empenhomulta.SaldoAtual + movimentacao.Valor;
                _unitOfWork.EmpenhoMulta.Update(empenhomulta);

                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Anulação realizada com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "Anulacao");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Anulacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao realizar anulação"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: EditarAporte                                                                               |
        /// | Descrição: Edita as informações ou valor de um aporte já registrado.                             |
        /// | Parâmetros: MovimentacaoEmpenhoMulta movimentacao                                                |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("EditarAporte")]
        [Consumes("application/json")]
        public IActionResult EditarAporte([FromBody] MovimentacaoEmpenhoMulta movimentacao)
        {
            try
            {
                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenhoMulta.GetFirstOrDefault(m =>
                    m.MovimentacaoId == movimentacao.MovimentacaoId
                );

                var valorAnterior = movimentacaoDb.Valor;

                movimentacao.Valor = movimentacao.Valor / 100;
                _unitOfWork.MovimentacaoEmpenhoMulta.Update(movimentacao);

                var empenhomulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(u =>
                    u.EmpenhoMultaId == movimentacao.EmpenhoMultaId
                );
                empenhomulta.SaldoAtual =
                    empenhomulta.SaldoAtual - valorAnterior + movimentacao.Valor;
                _unitOfWork.EmpenhoMulta.Update(empenhomulta);

                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Aporte editado com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "EditarAporte");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "EditarAporte" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar aporte"
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: VerificaPDFExiste                                                                          |
        /// | Descrição: Verifica se um arquivo PDF de multa está fisicamente presente no servidor.            |
        /// | Parâmetros: string nomeArquivo                                                                   |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("VerificaPDFExiste")]
        [HttpGet]
        public IActionResult VerificaPDFExiste(string nomeArquivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeArquivo))
                {
                    return Json(new
                    {
                        success = false ,
                        existe = false ,
                        message = "Nome do arquivo não informado"
                    });
                }

                // Caminho da pasta de multas (wwwroot/DadosEditaveis/Multas)
                var webRootPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory() , "wwwroot");
                var pastaMultas = System.IO.Path.Combine(webRootPath , "DadosEditaveis" , "Multas");
                var caminhoCompleto = System.IO.Path.Combine(pastaMultas , System.IO.Path.GetFileName(nomeArquivo));

                var existe = System.IO.File.Exists(caminhoCompleto);

                return Json(new
                {
                    success = true ,
                    existe = existe ,
                    message = existe ? "Arquivo encontrado" : "Arquivo não encontrado no servidor" ,
                    arquivo = nomeArquivo
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "VerificaPDFExiste");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "VerificaPDFExiste" , error);
                return Json(new
                {
                    success = false ,
                    existe = false ,
                    message = "Erro ao verificar arquivo: " + error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: EditarAnulacao                                                                             |
        /// | Descrição: Edita os dados ou valor de uma anulação já registrada.                                |
        /// | Parâmetros: MovimentacaoEmpenhoMulta movimentacao                                                |
        /// | Retorno: IActionResult (JSON)                                                                    |
        /// |__________________________________________________________________________________________________|
        /// </summary>
        [Route("EditarAnulacao")]
        [Consumes("application/json")]
        public IActionResult EditarAnulacao([FromBody] MovimentacaoEmpenhoMulta movimentacao)
        {
            try
            {
                var movimentacaoDb = _unitOfWork.MovimentacaoEmpenhoMulta.GetFirstOrDefault(u =>
                    u.MovimentacaoId == movimentacao.MovimentacaoId
                );

                var valorAnterior = movimentacaoDb.Valor;

                movimentacao.Valor = movimentacao.Valor / 100;
                _unitOfWork.MovimentacaoEmpenhoMulta.Update(movimentacao);

                var empenhomulta = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(u =>
                    u.EmpenhoMultaId == movimentacao.EmpenhoMultaId
                );
                empenhomulta.SaldoAtual =
                    empenhomulta.SaldoAtual + valorAnterior - movimentacao.Valor;
                _unitOfWork.EmpenhoMulta.Update(empenhomulta);

                _unitOfWork.Save();

                return Json(
                    new
                    {
                        success = true ,
                        message = "Anulação editada com sucesso" ,
                        type = 0 ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message , error , "MultaController.cs" , "EditarAnulacao");
                Alerta.TratamentoErroComLinha("MultaController.cs" , "EditarAnulacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar anulação"
                });
            }
        }
    }
}
