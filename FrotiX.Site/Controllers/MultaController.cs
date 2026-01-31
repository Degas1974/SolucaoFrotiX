/* ****************************************************************************************
 * ⚡ ARQUIVO: MultaController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar multas de trânsito (infrações de veículos), incluindo
 *                   filtros, vinculação de viagens, pagamentos e empenhos.
 *
 * 📥 ENTRADAS     : Multa, MovimentacaoEmpenhoMulta e filtros (fase, veículo, órgão, etc.).
 *
 * 📤 SAÍDAS       : JSON com listas, detalhes, validações e saldos de empenhos.
 *
 * 🔗 CHAMADA POR  : Pages/Multas/Index, grids AJAX e modais de upload/pagamento.
 *
 * 🔄 CHAMA        : IUnitOfWork (Multa, EmpenhoMulta, Veiculo, Motorista, Orgao), Servicos.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework, FrotiX.Services.
 *
 * 📝 OBSERVAÇÕES  : Controller concentra regras de fase (Notificação/Penalidade) e
 *                   movimentações de empenho (aporte/anulação).
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: MultaController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoints de multas para listagem, manutenção, pagamentos,
 *                   validações de vínculo e operações de empenho.
 *
 * 📥 ENTRADAS     : IDs, filtros de pesquisa e dados de pagamento/penalidade.
 *
 * 📤 SAÍDAS       : JSON com sucesso/erro e dados de apoio ao frontend.
 *
 * 🔗 CHAMADA POR  : Telas de Multas e integrações com viagens/ocorrências.
 *
 * 🔄 CHAMA        : Repositórios via IUnitOfWork e utilitários Servicos.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework.
 ****************************************************************************************/
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
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class MultaController : Controller
    {
        [BindProperty]
        public MovimentacaoEmpenhoMultaViewModel MovimentacaoObj
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: MultaController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork para acesso a dados de multas.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public MultaController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Test
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Verificar se o endpoint do controller está respondendo.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso.
         *
         * 🔗 CHAMADA POR  : Testes manuais/diagnóstico.
         ****************************************************************************************/
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok(new { success = true , message = "MultaPdfViewer está funcionando!" });
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaMultas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar multas filtrando por fase, veículo, órgão, motorista e status.
         *
         * 📥 ENTRADAS     : Fase, Veiculo, Orgao, Motorista, Infracao, Status.
         *
         * 📤 SAÍDAS       : JSON com lista de multas formatada para o grid.
         *
         * 🔗 CHAMADA POR  : Grid de Multas (AJAX).
         *
         * 🔄 CHAMA        : _unitOfWork.viewMultas.GetAll(), Servicos.ConvertHtml().
         ****************************************************************************************/
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

                return Json(new
                {
                    data = filtro.ToList()
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaMultas" , error);
                return Json(new
                {
                    data = new List<object>() ,
                    error = "Erro ao carregar dados"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaTipoMulta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de tipos de multa (artigo, denatran, descrição).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com tipos de multa.
         *
         * 🔗 CHAMADA POR  : Dropdowns de infração.
         *
         * 🔄 CHAMA        : _unitOfWork.TipoMulta.GetAll().
         ****************************************************************************************/
        [Route("PegaTipoMulta")]
        [HttpGet]
        public IActionResult PegaTipoMulta()
        {
            try
            {
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaTipoMulta" , error);
                return Json(new
                {
                    data = new List<object>() ,
                    error = "Erro ao carregar tipos de multa"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaOrgaoAutuante
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar órgãos autuantes cadastrados.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com órgãos autuantes.
         *
         * 🔗 CHAMADA POR  : Dropdowns de órgão autuante.
         *
         * 🔄 CHAMA        : _unitOfWork.OrgaoAutuante.GetAll().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaOrgaoAutuante" , error);
                return Json(new
                {
                    data = new List<object>() ,
                    error = "Erro ao carregar órgãos autuantes"
                });
            }
        }

        /****************************************************************************************
         * ⚡ CLASSE: TipoMultaAjax
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Payload simples para exclusão de tipo de multa.
         *
         * 📥 ENTRADAS     : TipoMultaId.
         *
         * 📤 SAÍDAS       : Objeto usado em DeleteTipoMulta.
         ****************************************************************************************/
        public class TipoMultaAjax
        {
            public Guid TipoMultaId
            {
                get; set;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteTipoMulta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover um tipo de multa (infração) pelo ID.
         *
         * 📥 ENTRADAS     : [TipoMultaAjax] model.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Tela de cadastro de tipos de multa.
         *
         * 🔄 CHAMA        : TipoMulta.GetFirstOrDefault(), TipoMulta.Remove(), Save().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "DeleteTipoMulta" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Infração"
                });
            }
        }

        /****************************************************************************************
         * ⚡ CLASSE: OrgaoAutuanteAjax
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Payload simples para exclusão de órgão autuante.
         *
         * 📥 ENTRADAS     : OrgaoAutuanteId.
         *
         * 📤 SAÍDAS       : Objeto usado em DeleteOrgaoAutuante.
         ****************************************************************************************/
        public class OrgaoAutuanteAjax
        {
            public Guid OrgaoAutuanteId
            {
                get; set;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteOrgaoAutuante
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover órgão autuante pelo ID.
         *
         * 📥 ENTRADAS     : [OrgaoAutuanteAjax] Orgao.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Tela de cadastro de órgãos autuantes.
         *
         * 🔄 CHAMA        : OrgaoAutuante.GetFirstOrDefault(), Remove(), Save().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "DeleteOrgaoAutuante" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Órgão Autuante"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaEmpenhos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar empenhos de multa por órgão autuante.
         *
         * 📥 ENTRADAS     : Id (Guid) do órgão autuante.
         *
         * 📤 SAÍDAS       : JSON com empenhos e saldos formatados.
         *
         * 🔗 CHAMADA POR  : Filtros/seleção de empenho.
         *
         * 🔄 CHAMA        : ViewEmpenhoMulta.GetAll().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaEmpenhos" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao recuperar Empenhos"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover multa e ajustar empenho/movimentação quando necessário.
         *
         * 📥 ENTRADAS     : [MultaViewModel] model.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de exclusão no grid de multas.
         *
         * 🔄 CHAMA        : Multa.GetFirstOrDefault(), EmpenhoMulta.Update(),
         *                   MovimentacaoEmpenhoMulta.Add(), Save().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: TransformaPenalidade
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Transformar notificação em penalidade e atualizar valores.
         *
         * 📥 ENTRADAS     : MultaId, DataVencimento, ValorAteVencimento, Observacao,
         *                   PenalidadePDF, ProcessoEDoc.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro e mensagens de validação.
         *
         * 🔗 CHAMADA POR  : Fluxo de emissão de penalidade.
         *
         * 🔄 CHAMA        : Multa.GetFirstOrDefault(), Multa.Update(), Save().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: ProcuraViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Localizar viagem e motorista a partir da data/hora da autuação.
         *
         * 📥 ENTRADAS     : Data, Hora, VeiculoId (form-data).
         *
         * 📤 SAÍDAS       : JSON com noFichaVistoria e motoristaId ou mensagem de erro.
         *
         * 🔗 CHAMADA POR  : Inclusão de multa vinculada a viagem.
         *
         * 🔄 CHAMA        : ViewProcuraFicha.GetAll().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: ProcuraFicha
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar viagem por número da ficha de vistoria.
         *
         * 📥 ENTRADAS     : [ProcuraViagemViewModel] model.
         *
         * 📤 SAÍDAS       : JSON com viagemId ou erro.
         *
         * 🔗 CHAMADA POR  : Fluxo de vinculação de multa a ficha.
         *
         * 🔄 CHAMA        : Viagem.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaImagemFichaVistoria
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar imagem da ficha de vistoria e retornar em base64 para exibição.
         *
         * 📥 ENTRADAS     : noFicha (int) - número da ficha.
         *
         * 📤 SAÍDAS       : JSON com imagem em base64 e metadados, ou mensagem de erro.
         *
         * 🔗 CHAMADA POR  : Modal de visualização da ficha.
         *
         * 🔄 CHAMA        : Viagem.GetFirstOrDefault().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "PegaImagemFichaVistoria" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao buscar imagem da ficha: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnGetMultaExistente
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Verificar se já existe multa com o número de infração informado.
         *
         * 📥 ENTRADAS     : NumInfracao (string).
         *
         * 📤 SAÍDAS       : JSON com { data = true/false }.
         *
         * 🔗 CHAMADA POR  : Validação no formulário de multas.
         *
         * 🔄 CHAMA        : Multa.GetFirstOrDefault().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnGetMultaExistente" , error);
                return new JsonResult(new
                {
                    data = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostAlteraStatus
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alterar status da multa.
         *
         * 📥 ENTRADAS     : MultaId, Status.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de atualização de status.
         *
         * 🔄 CHAMA        : Multa.GetFirstOrDefault(), Multa.Update(), Save().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostAlteraStatus" , error);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao alterar status"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostPegaStatus
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter dados básicos da multa e status atual.
         *
         * 📥 ENTRADAS     : Id (string) - MultaId.
         *
         * 📤 SAÍDAS       : JSON com campos da multa e status.
         *
         * 🔗 CHAMADA POR  : Modais de alteração de status.
         *
         * 🔄 CHAMA        : viewMultas.GetFirstOrDefault().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaStatus" , error);
                return new JsonResult(new
                {
                    success = false ,
                    message = "Erro ao buscar dados"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostPegaInstrumentoVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Determinar se o veículo está vinculado a contrato ou ata.
         *
         * 📥 ENTRADAS     : Id (string) - VeiculoId.
         *
         * 📤 SAÍDAS       : JSON com instrumento e ID correspondente.
         *
         * 🔗 CHAMADA POR  : Validações de vínculo no cadastro de multas.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostValidaContratoVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Validar se o veículo está vinculado ao contrato informado.
         *
         * 📥 ENTRADAS     : veiculoId, contratoId.
         *
         * 📤 SAÍDAS       : JSON com success true/false.
         *
         * 🔗 CHAMADA POR  : Validações de vínculo.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostValidaAtaVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Validar se o veículo está vinculado à ata informada.
         *
         * 📥 ENTRADAS     : veiculoId, ataId.
         *
         * 📤 SAÍDAS       : JSON com success true/false.
         *
         * 🔗 CHAMADA POR  : Validações de vínculo.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostPegaContratoMotorista
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Recuperar contrato vinculado ao motorista informado.
         *
         * 📥 ENTRADAS     : Id (string) - MotoristaId.
         *
         * 📤 SAÍDAS       : JSON com contratoid ou erro.
         *
         * 🔗 CHAMADA POR  : Validações em cadastro de multa.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostValidaContratoMotorista
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Validar se o motorista pertence ao contrato informado.
         *
         * 📥 ENTRADAS     : motoristaId, contratoId.
         *
         * 📤 SAÍDAS       : JSON com success true/false.
         *
         * 🔗 CHAMADA POR  : Validações de vínculo motorista-contrato.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostPegaValor
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter valor da multa até o vencimento.
         *
         * 📥 ENTRADAS     : Id (string) - MultaId.
         *
         * 📤 SAÍDAS       : JSON com valor.
         *
         * 🔗 CHAMADA POR  : Tela de pagamento/consulta de multa.
         *
         * 🔄 CHAMA        : viewMultas.GetFirstOrDefault().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaValor" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostPegaEmpenhoMultaId
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter EmpenhoMultaId vinculado à multa.
         *
         * 📥 ENTRADAS     : Id (string) - MultaId.
         *
         * 📤 SAÍDAS       : JSON com empenhoMultaId.
         *
         * 🔗 CHAMADA POR  : Fluxos de pagamento e movimentação.
         *
         * 🔄 CHAMA        : Multa.GetFirstOrDefault().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostRegistraPagamento
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Registrar pagamento da multa e atualizar saldo do empenho.
         *
         * 📥 ENTRADAS     : MultaId, DataPagamento, ValorPago, Status, FormaPagamento,
         *                   ComprovantePDF, EmpenhoMultaId.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Modal de pagamento de multa.
         *
         * 🔄 CHAMA        : Multa.Update(), EmpenhoMulta.Update(),
         *                   MovimentacaoEmpenhoMulta.Add(), Save().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: OnPostPegaObservacao
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar observação e dados da multa para exibição.
         *
         * 📥 ENTRADAS     : Id (string) - MultaId.
         *
         * 📤 SAÍDAS       : JSON com numInfracao, nomeMotorista e observacao.
         *
         * 🔗 CHAMADA POR  : Modais de observação.
         *
         * 🔄 CHAMA        : viewMultas.GetFirstOrDefault().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "OnPostPegaObservacao" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MultaEmpenho
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar multas vinculadas a um empenho específico.
         *
         * 📥 ENTRADAS     : id (Guid) - EmpenhoMultaId.
         *
         * 📤 SAÍDAS       : JSON com multas e valores.
         *
         * 🔗 CHAMADA POR  : Detalhe de empenhos.
         *
         * 🔄 CHAMA        : Multa.GetAll().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaEmpenho" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MultaEmpenhoPagas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar multas pagas vinculadas a um empenho.
         *
         * 📥 ENTRADAS     : id (Guid) - EmpenhoMultaId.
         *
         * 📤 SAÍDAS       : JSON com multas pagas.
         *
         * 🔗 CHAMADA POR  : Detalhe de empenhos (pagas).
         *
         * 🔄 CHAMA        : Multa.GetAll(filter).
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "MultaEmpenhoPagas" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: SaldoMultas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcular soma total de multas pagas de um empenho.
         *
         * 📥 ENTRADAS     : Id (Guid) - EmpenhoMultaId.
         *
         * 📤 SAÍDAS       : JSON com saldomultas.
         *
         * 🔗 CHAMADA POR  : Painel de saldos.
         *
         * 🔄 CHAMA        : Multa.GetAll().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "SaldoMultas" , error);
                return Json(new
                {
                    saldomultas = 0
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaAporte
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar movimentações de aporte do empenho.
         *
         * 📥 ENTRADAS     : Id (Guid) - EmpenhoMultaId.
         *
         * 📤 SAÍDAS       : JSON com aportes formatados.
         *
         * 🔗 CHAMADA POR  : Histórico de movimentações.
         *
         * 🔄 CHAMA        : MovimentacaoEmpenhoMulta.GetAll().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaAporte" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaAnulacao
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar movimentações de anulação do empenho.
         *
         * 📥 ENTRADAS     : Id (Guid) - EmpenhoMultaId.
         *
         * 📤 SAÍDAS       : JSON com anulações formatadas.
         *
         * 🔗 CHAMADA POR  : Histórico de movimentações.
         *
         * 🔄 CHAMA        : MovimentacaoEmpenhoMulta.GetAll().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "ListaAnulacao" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Aporte
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Registrar aporte no empenho de multas.
         *
         * 📥 ENTRADAS     : [MovimentacaoEmpenhoMulta] movimentacao (JSON).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Modal de aporte do empenho.
         *
         * 🔄 CHAMA        : MovimentacaoEmpenhoMulta.Add(), EmpenhoMulta.Update(), Save().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Aporte" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao realizar aporte"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Anulacao
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Registrar anulação (glosa) no empenho de multas.
         *
         * 📥 ENTRADAS     : [MovimentacaoEmpenhoMulta] movimentacao (JSON).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Modal de anulação do empenho.
         *
         * 🔄 CHAMA        : MovimentacaoEmpenhoMulta.Update(), EmpenhoMulta.Update(), Save().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "Anulacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao realizar anulação"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EditarAporte
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Editar aporte e recalcular saldo do empenho.
         *
         * 📥 ENTRADAS     : [MovimentacaoEmpenhoMulta] movimentacao (JSON).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Edição de aporte.
         *
         * 🔄 CHAMA        : MovimentacaoEmpenhoMulta.Update(), EmpenhoMulta.Update(), Save().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "EditarAporte" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar aporte"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VerificaPDFExiste
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Verificar existência de PDF de autuação no servidor.
         *
         * 📥 ENTRADAS     : nomeArquivo (string).
         *
         * 📤 SAÍDAS       : JSON com flag de existência e mensagem.
         *
         * 🔗 CHAMADA POR  : Validação de arquivos de multa.
         *
         * 🔄 CHAMA        : System.IO.File.Exists().
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("MultaController.cs" , "VerificaPDFExiste" , error);
                return Json(new
                {
                    success = false ,
                    existe = false ,
                    message = "Erro ao verificar arquivo: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EditarAnulacao
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Editar anulação e recalcular saldo do empenho.
         *
         * 📥 ENTRADAS     : [MovimentacaoEmpenhoMulta] movimentacao (JSON).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Edição de anulação.
         *
         * 🔄 CHAMA        : MovimentacaoEmpenhoMulta.Update(), EmpenhoMulta.Update(), Save().
         ****************************************************************************************/
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
