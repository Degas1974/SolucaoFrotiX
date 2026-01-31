/* ****************************************************************************************
 * ⚡ ARQUIVO: ItensContratoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar itens e vínculos de contratos/atas (veículos, equipes e
 *                   demais recursos) com endpoints de listagem e manutenção.
 *
 * 📥 ENTRADAS     : IDs de contrato/ata, filtros de status e modelos de inclusão/remoção.
 *
 * 📤 SAÍDAS       : JSON com listas, detalhes resumidos e mensagens de sucesso/erro.
 *
 * 🔗 CHAMADA POR  : Páginas de Contratos/Atas e chamadas AJAX do frontend.
 *
 * 🔄 CHAMA        : Repositórios via IUnitOfWork, serviços de alerta.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core, Entity Framework, IUnitOfWork, LINQ.
 *
 * 📝 OBSERVAÇÕES  : Classe parcial, podendo ser complementada por outros arquivos.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: ItensContratoController (Partial Class)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Centralizar operações de listagem e manutenção de itens de contratos
 *                   e atas (veículos, encarregados, operadores, motoristas, lavadores).
 *
 * 📥 ENTRADAS     : IDs, filtros e view models específicos de inclusão/remoção.
 *
 * 📤 SAÍDAS       : JSON estruturado para grids e dropdowns do frontend.
 *
 * 🔗 CHAMADA POR  : JavaScript (AJAX) das páginas de Contratos e Atas.
 *
 * 🔄 CHAMA        : IUnitOfWork (Contrato, Ata, Veiculo, RH), Alerta.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core, Entity Framework, IUnitOfWork.
 ****************************************************************************************/
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ItensContratoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: ItensContratoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork para acesso a repositórios.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public ItensContratoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "Constructor", error);
            }
        }

        // ============================================================
        // CONTRATOS E ATAS - LISTAGEM PARA DROPDOWN
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar contratos para dropdown, filtrando por status.
         *
         * 📥 ENTRADAS     : status (bool) - indica se retorna contratos ativos/inativos.
         *
         * 📤 SAÍDAS       : [IActionResult] Ok com { success, data }.
         *
         * 🔗 CHAMADA POR  : Telas de Contratos/Atas (seleção de contrato).
         *
         * 🔄 CHAMA        : _unitOfWork.Contrato.GetAll(), LINQ (OrderBy/Select).
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaContratos")]
        public IActionResult ListaContratos(bool status = true)
        {
            try
            {
                var contratos = _unitOfWork.Contrato.GetAll(
                    filter: c => c.Status == status,
                    includeProperties: "Fornecedor"
                )
                .OrderBy(c => c.NumeroContrato)
                .ThenBy(c => c.AnoContrato)
                .Select(c => new
                {
                    value = c.ContratoId,
                    text = $"{c.NumeroContrato}/{c.AnoContrato} - {c.TipoContrato} - {c.Fornecedor.DescricaoFornecedor}",
                    tipoContrato = c.TipoContrato
                })
                .ToList();

                return Ok(new { success = true, data = contratos });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "ListaContratos", error);
                return Ok(new { success = false, message = "Erro ao carregar contratos" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaAtas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar atas de registro de preços para dropdown, filtrando por status.
         *
         * 📥 ENTRADAS     : status (bool) - indica se retorna atas ativas/inativas.
         *
         * 📤 SAÍDAS       : [IActionResult] Ok com { success, data }.
         *
         * 🔗 CHAMADA POR  : Telas de Contratos/Atas (seleção de ata).
         *
         * 🔄 CHAMA        : _unitOfWork.AtaRegistroPrecos.GetAll(), LINQ.
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaAtas")]
        public IActionResult ListaAtas(bool status = true)
        {
            try
            {
                var atas = _unitOfWork.AtaRegistroPrecos.GetAll(
                    filter: a => a.Status == status,
                    includeProperties: "Fornecedor"
                )
                .OrderBy(a => a.NumeroAta)
                .ThenBy(a => a.AnoAta)
                .Select(a => new
                {
                    value = a.AtaId,
                    text = $"{a.NumeroAta}/{a.AnoAta} - {a.Fornecedor.DescricaoFornecedor}"
                })
                .ToList();

                return Ok(new { success = true, data = atas });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "ListaAtas", error);
                return Ok(new { success = false, message = "Erro ao carregar atas" });
            }
        }

        // ============================================================
        // DETALHES DO CONTRATO/ATA SELECIONADO
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetContratoDetalhes
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar resumo detalhado do contrato selecionado.
         *
         * 📥 ENTRADAS     : id (Guid) - Identificador do contrato.
         *
         * 📤 SAÍDAS       : JSON com dados resumidos ou mensagem de erro.
         *
         * 🔗 CHAMADA POR  : Frontend ao selecionar contrato.
         *
         * 🔄 CHAMA        : _unitOfWork.Contrato.GetFirstOrDefault().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetContratoDetalhes")]
        public IActionResult GetContratoDetalhes(Guid id)
        {
            try
            {
                var contrato = _unitOfWork.Contrato.GetFirstOrDefault(
                    filter: c => c.ContratoId == id,
                    includeProperties: "Fornecedor"
                );

                if (contrato == null)
                {
                    return Ok(new { success = false, message = "Contrato não encontrado" });
                }

                var resumo = new
                {
                    contratoId = contrato.ContratoId,
                    numeroContrato = contrato.NumeroContrato,
                    anoContrato = contrato.AnoContrato,
                    contratoCompleto = $"{contrato.NumeroContrato}/{contrato.AnoContrato}",
                    tipoContrato = contrato.TipoContrato,
                    objeto = contrato.Objeto,
                    fornecedor = contrato.Fornecedor?.DescricaoFornecedor ?? "",
                    dataInicio = contrato.DataInicio?.ToString("dd/MM/yyyy"),
                    dataFim = contrato.DataFim?.ToString("dd/MM/yyyy"),
                    valor = contrato.Valor,
                    status = contrato.Status,
                    // Flags Terceirização
                    contratoEncarregados = contrato.ContratoEncarregados,
                    contratoOperadores = contrato.ContratoOperadores,
                    contratoMotoristas = contrato.ContratoMotoristas,
                    contratoLavadores = contrato.ContratoLavadores,
                    // Quantidades
                    quantidadeEncarregado = contrato.QuantidadeEncarregado ?? 0,
                    quantidadeOperador = contrato.QuantidadeOperador ?? 0,
                    quantidadeMotorista = contrato.QuantidadeMotorista ?? 0,
                    quantidadeLavador = contrato.QuantidadeLavador ?? 0,
                    // Custos
                    custoMensalEncarregado = contrato.CustoMensalEncarregado ?? 0,
                    custoMensalOperador = contrato.CustoMensalOperador ?? 0,
                    custoMensalMotorista = contrato.CustoMensalMotorista ?? 0,
                    custoMensalLavador = contrato.CustoMensalLavador ?? 0
                };

                return Ok(new { success = true, data = resumo });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetContratoDetalhes", error);
                return Ok(new { success = false, message = "Erro ao carregar detalhes do contrato" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAtaDetalhes
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar resumo detalhado da ata selecionada.
         *
         * 📥 ENTRADAS     : id (Guid) - Identificador da ata.
         *
         * 📤 SAÍDAS       : JSON com dados resumidos ou mensagem de erro.
         *
         * 🔗 CHAMADA POR  : Frontend ao selecionar ata.
         *
         * 🔄 CHAMA        : _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetAtaDetalhes")]
        public IActionResult GetAtaDetalhes(Guid id)
        {
            try
            {
                var ata = _unitOfWork.AtaRegistroPrecos.GetFirstOrDefault(
                    filter: a => a.AtaId == id,
                    includeProperties: "Fornecedor"
                );

                if (ata == null)
                {
                    return Ok(new { success = false, message = "Ata não encontrada" });
                }

                var resumo = new
                {
                    ataId = ata.AtaId,
                    numeroAta = ata.NumeroAta,
                    anoAta = ata.AnoAta,
                    ataCompleta = $"{ata.NumeroAta}/{ata.AnoAta}",
                    objeto = ata.Objeto,
                    fornecedor = ata.Fornecedor?.DescricaoFornecedor ?? "",
                    dataInicio = ata.DataInicio?.ToString("dd/MM/yyyy"),
                    dataFim = ata.DataFim?.ToString("dd/MM/yyyy"),
                    valor = ata.Valor,
                    status = ata.Status
                };

                return Ok(new { success = true, data = resumo });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetAtaDetalhes", error);
                return Ok(new { success = false, message = "Erro ao carregar detalhes da ata" });
            }
        }

        // ============================================================
        // VEÍCULOS DO CONTRATO
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetVeiculosContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos vinculados ao contrato e mapear itens associados.
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato selecionado.
         *
         * 📤 SAÍDAS       : JSON com veículos, itens (num/descrição) e contadores de status.
         *
         * 🔗 CHAMADA POR  : Grid de veículos do contrato.
         *
         * 🔄 CHAMA        : RepactuacaoContrato, ItemVeiculoContrato, VeiculoContrato, ViewVeiculos.
         *
         * 📝 OBSERVAÇÕES  : Busca itens de todas as repactuações para localizar ItemVeiculoId.
         ****************************************************************************************/
        [HttpGet]
        [Route("GetVeiculosContrato")]
        public IActionResult GetVeiculosContrato(Guid contratoId)
        {
            try
            {
                // Busca TODAS as repactuações do contrato
                var repactuacoes = _unitOfWork.RepactuacaoContrato.GetAll(
                    filter: r => r.ContratoId == contratoId
                ).Select(r => r.RepactuacaoContratoId).ToList();

                // Busca TODOS os itens de TODAS as repactuações do contrato
                var itensContrato = new Dictionary<Guid, (int? NumItem, string Descricao)>();
                if (repactuacoes.Any())
                {
                    var itens = _unitOfWork.ItemVeiculoContrato.GetAll(
                        filter: i => repactuacoes.Contains(i.RepactuacaoContratoId)
                    ).ToList();
                    
                    foreach (var item in itens)
                    {
                        itensContrato[item.ItemVeiculoId] = (item.NumItem, item.Descricao);
                    }
                }

                // Busca veículos vinculados ao contrato
                var veiculosContrato = _unitOfWork.VeiculoContrato.GetAll(
                    filter: vc => vc.ContratoId == contratoId
                ).ToList();

                var veiculoIds = veiculosContrato.Select(vc => vc.VeiculoId).ToList();

                var veiculosCompletos = _unitOfWork.Veiculo.GetAll(
                    filter: v => veiculoIds.Contains(v.VeiculoId)
                ).ToDictionary(v => v.VeiculoId);

                var viewVeiculos = _unitOfWork.ViewVeiculos.GetAll(
                    filter: v => veiculoIds.Contains(v.VeiculoId)
                ).ToDictionary(v => v.VeiculoId);

                var veiculos = veiculosContrato
                    .Where(vc => viewVeiculos.ContainsKey(vc.VeiculoId))
                    .Select(vc =>
                    {
                        var viewVeiculo = viewVeiculos[vc.VeiculoId];
                        var veiculoCompleto = veiculosCompletos.ContainsKey(vc.VeiculoId) ? veiculosCompletos[vc.VeiculoId] : null;
                        var itemVeiculoId = veiculoCompleto?.ItemVeiculoId;
                        var temItem = itemVeiculoId.HasValue && itensContrato.ContainsKey(itemVeiculoId.Value);
                        var numItem = temItem ? itensContrato[itemVeiculoId.Value].NumItem : (int?)null;
                        var descricaoItem = temItem ? $"Item {itensContrato[itemVeiculoId.Value].NumItem} - {itensContrato[itemVeiculoId.Value].Descricao}" : "";

                        return new
                        {
                            veiculoId = vc.VeiculoId,
                            contratoId = contratoId,
                            placa = viewVeiculo.Placa,
                            marcaModelo = viewVeiculo.MarcaModelo,
                            itemVeiculoId = itemVeiculoId,
                            numItem = numItem,
                            descricaoItem = descricaoItem,
                            status = viewVeiculo.Status
                        };
                    })
                    .OrderBy(v => v.placa)
                    .ToList();

                var qtdAtivos = veiculos.Count(v => v.status == true);
                var qtdInativos = veiculos.Count(v => v.status == false);

                return Ok(new { success = true, data = veiculos, qtdAtivos = qtdAtivos, qtdInativos = qtdInativos });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosContrato", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetVeiculosDisponiveis
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos ativos disponíveis (não vinculados ao contrato).
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato atual.
         *
         * 📤 SAÍDAS       : JSON com lista para dropdown.
         *
         * 🔗 CHAMADA POR  : Formulários de inclusão de veículos no contrato.
         *
         * 🔄 CHAMA        : VeiculoContrato.GetAll(), ViewVeiculos.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetVeiculosDisponiveis")]
        public IActionResult GetVeiculosDisponiveis(Guid contratoId)
        {
            try
            {
                var veiculosNoContrato = _unitOfWork.VeiculoContrato.GetAll(
                    filter: vc => vc.ContratoId == contratoId
                ).Select(vc => vc.VeiculoId).ToList();

                var veiculos = _unitOfWork.ViewVeiculos.GetAll(
                    filter: v => v.Status == true && !veiculosNoContrato.Contains(v.VeiculoId)
                )
                .Select(v => new
                {
                    value = v.VeiculoId,
                    text = $"{v.Placa} - {v.MarcaModelo}"
                })
                .OrderBy(v => v.text)
                .ToList();

                return Ok(new { success = true, data = veiculos });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosDisponiveis", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetItensContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar itens do contrato com base na última repactuação.
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato selecionado.
         *
         * 📤 SAÍDAS       : JSON com lista de itens formatados para dropdown.
         *
         * 🔗 CHAMADA POR  : Frontend ao associar veículos/itens.
         *
         * 🔄 CHAMA        : RepactuacaoContrato.GetAll(), ItemVeiculoContrato.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetItensContrato")]
        public IActionResult GetItensContrato(Guid contratoId)
        {
            try
            {
                var repactuacao = _unitOfWork.RepactuacaoContrato.GetAll(
                    filter: r => r.ContratoId == contratoId
                ).OrderByDescending(r => r.DataRepactuacao).FirstOrDefault();

                if (repactuacao == null)
                {
                    return Ok(new { success = true, data = new List<object>() });
                }

                var itens = _unitOfWork.ItemVeiculoContrato.GetAll(
                    filter: i => i.RepactuacaoContratoId == repactuacao.RepactuacaoContratoId
                )
                .Select(i => new
                {
                    value = i.ItemVeiculoId,
                    text = $"Item {i.NumItem} - {i.Descricao}",
                    numItem = i.NumItem,
                    descricao = i.Descricao,
                    quantidade = i.Quantidade,
                    valorUnitario = i.ValorUnitario
                })
                .OrderBy(i => i.numItem)
                .ToList();

                return Ok(new { success = true, data = itens });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetItensContrato", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: IncluirVeiculoContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Vincular veículo ao contrato e (opcionalmente) ao item do contrato.
         *
         * 📥 ENTRADAS     : [ICIncluirVeiculoContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de inclusão de veículo no contrato.
         *
         * 🔄 CHAMA        : VeiculoContrato.Add(), Veiculo.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("IncluirVeiculoContrato")]
        public IActionResult IncluirVeiculoContrato([FromBody] ICIncluirVeiculoContratoVM model)
        {
            try
            {
                var existe = _unitOfWork.VeiculoContrato.GetFirstOrDefault(
                    vc => vc.VeiculoId == model.VeiculoId && vc.ContratoId == model.ContratoId
                );

                if (existe != null)
                {
                    return Ok(new { success = false, message = "Este veículo já está associado ao contrato!" });
                }

                var veiculoContrato = new VeiculoContrato
                {
                    VeiculoId = model.VeiculoId,
                    ContratoId = model.ContratoId
                };

                _unitOfWork.VeiculoContrato.Add(veiculoContrato);

                if (model.ItemVeiculoId.HasValue)
                {
                    var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == model.VeiculoId);
                    if (veiculo != null)
                    {
                        veiculo.ItemVeiculoId = model.ItemVeiculoId;
                        _unitOfWork.Veiculo.Update(veiculo);
                    }
                }

                _unitOfWork.Save();

                return Ok(new { success = true, message = "Veículo incluído no contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirVeiculoContrato", error);
                return Ok(new { success = false, message = "Erro ao incluir veículo no contrato" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoverVeiculoContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do veículo com o contrato e limpar associações.
         *
         * 📥 ENTRADAS     : [ICRemoverVeiculoContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção de veículo do contrato.
         *
         * 🔄 CHAMA        : VeiculoContrato.Remove(), Veiculo.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("RemoverVeiculoContrato")]
        public IActionResult RemoverVeiculoContrato([FromBody] ICRemoverVeiculoContratoVM model)
        {
            try
            {
                var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(
                    vc => vc.VeiculoId == model.VeiculoId && vc.ContratoId == model.ContratoId
                );

                if (veiculoContrato == null)
                {
                    return Ok(new { success = false, message = "Associação não encontrada!" });
                }

                _unitOfWork.VeiculoContrato.Remove(veiculoContrato);

                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == model.VeiculoId);
                if (veiculo != null)
                {
                    veiculo.ItemVeiculoId = null;
                    veiculo.ContratoId = null;
                    _unitOfWork.Veiculo.Update(veiculo);
                }

                _unitOfWork.Save();

                return Ok(new { success = true, message = "Veículo removido do contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverVeiculoContrato", error);
                return Ok(new { success = false, message = "Erro ao remover veículo do contrato" });
            }
        }

        // ============================================================
        // ENCARREGADOS DO CONTRATO - Relacionamento 1:N via ContratoId
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetEncarregadosContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar encarregados vinculados ao contrato (relacionamento 1:N).
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato selecionado.
         *
         * 📤 SAÍDAS       : JSON com lista de encarregados.
         *
         * 🔗 CHAMADA POR  : Grid de encarregados do contrato.
         *
         * 🔄 CHAMA        : _unitOfWork.Encarregado.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetEncarregadosContrato")]
        public IActionResult GetEncarregadosContrato(Guid contratoId)
        {
            try
            {
                var encarregados = _unitOfWork.Encarregado.GetAll(
                    filter: e => e.ContratoId == contratoId
                )
                .Select(e => new
                {
                    encarregadoId = e.EncarregadoId,
                    contratoId = e.ContratoId,
                    nome = e.Nome,
                    ponto = e.Ponto,
                    celular01 = e.Celular01,
                    status = e.Status
                })
                .OrderBy(e => e.nome)
                .ToList();

                return Ok(new { success = true, data = encarregados });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetEncarregadosContrato", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetEncarregadosDisponiveis
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar encarregados ativos sem contrato associado.
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato atual (não usado diretamente).
         *
         * 📤 SAÍDAS       : JSON com lista para dropdown.
         *
         * 🔗 CHAMADA POR  : Formulário de inclusão de encarregado.
         *
         * 🔄 CHAMA        : _unitOfWork.Encarregado.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetEncarregadosDisponiveis")]
        public IActionResult GetEncarregadosDisponiveis(Guid contratoId)
        {
            try
            {
                var encarregados = _unitOfWork.Encarregado.GetAll(
                    filter: e => e.Status == true && e.ContratoId == Guid.Empty
                )
                .Select(e => new
                {
                    value = e.EncarregadoId,
                    text = $"{e.Nome} ({e.Ponto})"
                })
                .OrderBy(e => e.text)
                .ToList();

                return Ok(new { success = true, data = encarregados });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetEncarregadosDisponiveis", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: IncluirEncarregadoContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Associar encarregado ao contrato selecionado.
         *
         * 📥 ENTRADAS     : [ICIncluirEncarregadoContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de inclusão de encarregado.
         *
         * 🔄 CHAMA        : Encarregado.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("IncluirEncarregadoContrato")]
        public IActionResult IncluirEncarregadoContrato([FromBody] ICIncluirEncarregadoContratoVM model)
        {
            try
            {
                var encarregado = _unitOfWork.Encarregado.GetFirstOrDefault(
                    e => e.EncarregadoId == model.EncarregadoId
                );

                if (encarregado == null)
                {
                    return Ok(new { success = false, message = "Encarregado não encontrado!" });
                }

                if (encarregado.ContratoId == model.ContratoId)
                {
                    return Ok(new { success = false, message = "Este encarregado já está associado ao contrato!" });
                }

                encarregado.ContratoId = model.ContratoId;
                _unitOfWork.Encarregado.Update(encarregado);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Encarregado incluído no contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirEncarregadoContrato", error);
                return Ok(new { success = false, message = "Erro ao incluir encarregado no contrato" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoverEncarregadoContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do encarregado com o contrato (define Guid.Empty).
         *
         * 📥 ENTRADAS     : [ICRemoverEncarregadoContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção de encarregado.
         *
         * 🔄 CHAMA        : Encarregado.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("RemoverEncarregadoContrato")]
        public IActionResult RemoverEncarregadoContrato([FromBody] ICRemoverEncarregadoContratoVM model)
        {
            try
            {
                var encarregado = _unitOfWork.Encarregado.GetFirstOrDefault(
                    e => e.EncarregadoId == model.EncarregadoId && e.ContratoId == model.ContratoId
                );

                if (encarregado == null)
                {
                    return Ok(new { success = false, message = "Encarregado não encontrado neste contrato!" });
                }

                encarregado.ContratoId = Guid.Empty;
                _unitOfWork.Encarregado.Update(encarregado);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Encarregado removido do contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverEncarregadoContrato", error);
                return Ok(new { success = false, message = "Erro ao remover encarregado do contrato" });
            }
        }

        // ============================================================
        // OPERADORES DO CONTRATO - Relacionamento 1:N via ContratoId
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetOperadoresContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar operadores vinculados ao contrato (relacionamento 1:N).
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato selecionado.
         *
         * 📤 SAÍDAS       : JSON com lista de operadores.
         *
         * 🔗 CHAMADA POR  : Grid de operadores do contrato.
         *
         * 🔄 CHAMA        : _unitOfWork.Operador.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetOperadoresContrato")]
        public IActionResult GetOperadoresContrato(Guid contratoId)
        {
            try
            {
                var operadores = _unitOfWork.Operador.GetAll(
                    filter: o => o.ContratoId == contratoId
                )
                .Select(o => new
                {
                    operadorId = o.OperadorId,
                    contratoId = o.ContratoId,
                    nome = o.Nome,
                    ponto = o.Ponto,
                    celular01 = o.Celular01,
                    status = o.Status
                })
                .OrderBy(o => o.nome)
                .ToList();

                return Ok(new { success = true, data = operadores });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetOperadoresContrato", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetOperadoresDisponiveis
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar operadores ativos sem contrato associado.
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato atual (não usado diretamente).
         *
         * 📤 SAÍDAS       : JSON com lista para dropdown.
         *
         * 🔗 CHAMADA POR  : Formulário de inclusão de operador.
         *
         * 🔄 CHAMA        : _unitOfWork.Operador.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetOperadoresDisponiveis")]
        public IActionResult GetOperadoresDisponiveis(Guid contratoId)
        {
            try
            {
                var operadores = _unitOfWork.Operador.GetAll(
                    filter: o => o.Status == true && o.ContratoId == Guid.Empty
                )
                .Select(o => new
                {
                    value = o.OperadorId,
                    text = $"{o.Nome} ({o.Ponto})"
                })
                .OrderBy(o => o.text)
                .ToList();

                return Ok(new { success = true, data = operadores });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetOperadoresDisponiveis", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: IncluirOperadorContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Associar operador ao contrato selecionado.
         *
         * 📥 ENTRADAS     : [ICIncluirOperadorContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de inclusão de operador.
         *
         * 🔄 CHAMA        : Operador.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("IncluirOperadorContrato")]
        public IActionResult IncluirOperadorContrato([FromBody] ICIncluirOperadorContratoVM model)
        {
            try
            {
                var operador = _unitOfWork.Operador.GetFirstOrDefault(
                    o => o.OperadorId == model.OperadorId
                );

                if (operador == null)
                {
                    return Ok(new { success = false, message = "Operador não encontrado!" });
                }

                if (operador.ContratoId == model.ContratoId)
                {
                    return Ok(new { success = false, message = "Este operador já está associado ao contrato!" });
                }

                operador.ContratoId = model.ContratoId;
                _unitOfWork.Operador.Update(operador);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Operador incluído no contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirOperadorContrato", error);
                return Ok(new { success = false, message = "Erro ao incluir operador no contrato" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoverOperadorContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do operador com o contrato (define Guid.Empty).
         *
         * 📥 ENTRADAS     : [ICRemoverOperadorContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção de operador.
         *
         * 🔄 CHAMA        : Operador.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("RemoverOperadorContrato")]
        public IActionResult RemoverOperadorContrato([FromBody] ICRemoverOperadorContratoVM model)
        {
            try
            {
                var operador = _unitOfWork.Operador.GetFirstOrDefault(
                    o => o.OperadorId == model.OperadorId && o.ContratoId == model.ContratoId
                );

                if (operador == null)
                {
                    return Ok(new { success = false, message = "Operador não encontrado neste contrato!" });
                }

                operador.ContratoId = Guid.Empty;
                _unitOfWork.Operador.Update(operador);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Operador removido do contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverOperadorContrato", error);
                return Ok(new { success = false, message = "Erro ao remover operador do contrato" });
            }
        }

        // ============================================================
        // MOTORISTAS DO CONTRATO - Relacionamento 1:N via ContratoId
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetMotoristasContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar motoristas vinculados ao contrato (relacionamento 1:N).
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato selecionado.
         *
         * 📤 SAÍDAS       : JSON com lista de motoristas.
         *
         * 🔗 CHAMADA POR  : Grid de motoristas do contrato.
         *
         * 🔄 CHAMA        : _unitOfWork.Motorista.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetMotoristasContrato")]
        public IActionResult GetMotoristasContrato(Guid contratoId)
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll(
                    filter: m => m.ContratoId == contratoId
                )
                .Select(m => new
                {
                    motoristaId = m.MotoristaId,
                    contratoId = m.ContratoId,
                    nome = m.Nome,
                    ponto = m.Ponto,
                    cnh = m.CNH,
                    celular01 = m.Celular01,
                    status = m.Status
                })
                .OrderBy(m => m.nome)
                .ToList();

                return Ok(new { success = true, data = motoristas });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetMotoristasContrato", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetMotoristasDisponiveis
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar motoristas ativos sem contrato associado.
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato atual (não usado diretamente).
         *
         * 📤 SAÍDAS       : JSON com lista para dropdown.
         *
         * 🔗 CHAMADA POR  : Formulário de inclusão de motorista.
         *
         * 🔄 CHAMA        : _unitOfWork.Motorista.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetMotoristasDisponiveis")]
        public IActionResult GetMotoristasDisponiveis(Guid contratoId)
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll(
                    filter: m => m.Status == true && m.ContratoId == Guid.Empty
                )
                .Select(m => new
                {
                    value = m.MotoristaId,
                    text = $"{m.Nome} ({m.Ponto})"
                })
                .OrderBy(m => m.text)
                .ToList();

                return Ok(new { success = true, data = motoristas });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetMotoristasDisponiveis", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: IncluirMotoristaContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Associar motorista ao contrato selecionado.
         *
         * 📥 ENTRADAS     : [ICIncluirMotoristaContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de inclusão de motorista.
         *
         * 🔄 CHAMA        : Motorista.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("IncluirMotoristaContrato")]
        public IActionResult IncluirMotoristaContrato([FromBody] ICIncluirMotoristaContratoVM model)
        {
            try
            {
                var motorista = _unitOfWork.Motorista.GetFirstOrDefault(
                    m => m.MotoristaId == model.MotoristaId
                );

                if (motorista == null)
                {
                    return Ok(new { success = false, message = "Motorista não encontrado!" });
                }

                if (motorista.ContratoId == model.ContratoId)
                {
                    return Ok(new { success = false, message = "Este motorista já está associado ao contrato!" });
                }

                motorista.ContratoId = model.ContratoId;
                _unitOfWork.Motorista.Update(motorista);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Motorista incluído no contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirMotoristaContrato", error);
                return Ok(new { success = false, message = "Erro ao incluir motorista no contrato" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoverMotoristaContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do motorista com o contrato (define Guid.Empty).
         *
         * 📥 ENTRADAS     : [ICRemoverMotoristaContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção de motorista.
         *
         * 🔄 CHAMA        : Motorista.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("RemoverMotoristaContrato")]
        public IActionResult RemoverMotoristaContrato([FromBody] ICRemoverMotoristaContratoVM model)
        {
            try
            {
                var motorista = _unitOfWork.Motorista.GetFirstOrDefault(
                    m => m.MotoristaId == model.MotoristaId && m.ContratoId == model.ContratoId
                );

                if (motorista == null)
                {
                    return Ok(new { success = false, message = "Motorista não encontrado neste contrato!" });
                }

                motorista.ContratoId = Guid.Empty;
                _unitOfWork.Motorista.Update(motorista);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Motorista removido do contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverMotoristaContrato", error);
                return Ok(new { success = false, message = "Erro ao remover motorista do contrato" });
            }
        }

        // ============================================================
        // LAVADORES DO CONTRATO - Relacionamento 1:N via ContratoId
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetLavadoresContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar lavadores vinculados ao contrato (relacionamento 1:N).
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato selecionado.
         *
         * 📤 SAÍDAS       : JSON com lista de lavadores.
         *
         * 🔗 CHAMADA POR  : Grid de lavadores do contrato.
         *
         * 🔄 CHAMA        : _unitOfWork.Lavador.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetLavadoresContrato")]
        public IActionResult GetLavadoresContrato(Guid contratoId)
        {
            try
            {
                var lavadores = _unitOfWork.Lavador.GetAll(
                    filter: l => l.ContratoId == contratoId
                )
                .Select(l => new
                {
                    lavadorId = l.LavadorId,
                    contratoId = l.ContratoId,
                    nome = l.Nome,
                    ponto = l.Ponto,
                    celular01 = l.Celular01,
                    status = l.Status
                })
                .OrderBy(l => l.nome)
                .ToList();

                return Ok(new { success = true, data = lavadores });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetLavadoresContrato", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetLavadoresDisponiveis
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar lavadores ativos sem contrato associado.
         *
         * 📥 ENTRADAS     : contratoId (Guid) - Contrato atual (não usado diretamente).
         *
         * 📤 SAÍDAS       : JSON com lista para dropdown.
         *
         * 🔗 CHAMADA POR  : Formulário de inclusão de lavador.
         *
         * 🔄 CHAMA        : _unitOfWork.Lavador.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetLavadoresDisponiveis")]
        public IActionResult GetLavadoresDisponiveis(Guid contratoId)
        {
            try
            {
                var lavadores = _unitOfWork.Lavador.GetAll(
                    filter: l => l.Status == true && l.ContratoId == Guid.Empty
                )
                .Select(l => new
                {
                    value = l.LavadorId,
                    text = $"{l.Nome} ({l.Ponto})"
                })
                .OrderBy(l => l.text)
                .ToList();

                return Ok(new { success = true, data = lavadores });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetLavadoresDisponiveis", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: IncluirLavadorContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Associar lavador ao contrato selecionado.
         *
         * 📥 ENTRADAS     : [ICIncluirLavadorContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de inclusão de lavador.
         *
         * 🔄 CHAMA        : Lavador.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("IncluirLavadorContrato")]
        public IActionResult IncluirLavadorContrato([FromBody] ICIncluirLavadorContratoVM model)
        {
            try
            {
                var lavador = _unitOfWork.Lavador.GetFirstOrDefault(
                    l => l.LavadorId == model.LavadorId
                );

                if (lavador == null)
                {
                    return Ok(new { success = false, message = "Lavador não encontrado!" });
                }

                if (lavador.ContratoId == model.ContratoId)
                {
                    return Ok(new { success = false, message = "Este lavador já está associado ao contrato!" });
                }

                lavador.ContratoId = model.ContratoId;
                _unitOfWork.Lavador.Update(lavador);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Lavador incluído no contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirLavadorContrato", error);
                return Ok(new { success = false, message = "Erro ao incluir lavador no contrato" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoverLavadorContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do lavador com o contrato (define Guid.Empty).
         *
         * 📥 ENTRADAS     : [ICRemoverLavadorContratoVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção de lavador.
         *
         * 🔄 CHAMA        : Lavador.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("RemoverLavadorContrato")]
        public IActionResult RemoverLavadorContrato([FromBody] ICRemoverLavadorContratoVM model)
        {
            try
            {
                var lavador = _unitOfWork.Lavador.GetFirstOrDefault(
                    l => l.LavadorId == model.LavadorId && l.ContratoId == model.ContratoId
                );

                if (lavador == null)
                {
                    return Ok(new { success = false, message = "Lavador não encontrado neste contrato!" });
                }

                lavador.ContratoId = Guid.Empty;
                _unitOfWork.Lavador.Update(lavador);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Lavador removido do contrato com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverLavadorContrato", error);
                return Ok(new { success = false, message = "Erro ao remover lavador do contrato" });
            }
        }

        // ============================================================
        // VEÍCULOS DA ATA
        // ============================================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetVeiculosAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos vinculados à ata e retornar contadores de status.
         *
         * 📥 ENTRADAS     : ataId (Guid) - Ata selecionada.
         *
         * 📤 SAÍDAS       : JSON com veículos e contagem de ativos/inativos.
         *
         * 🔗 CHAMADA POR  : Grid de veículos da ata.
         *
         * 🔄 CHAMA        : VeiculoAta.GetAll(), ViewVeiculos.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetVeiculosAta")]
        public IActionResult GetVeiculosAta(Guid ataId)
        {
            try
            {
                var veiculosAta = _unitOfWork.VeiculoAta.GetAll(
                    filter: va => va.AtaId == ataId
                ).ToList();

                var veiculoIds = veiculosAta.Select(va => va.VeiculoId).ToList();

                var veiculos = _unitOfWork.ViewVeiculos.GetAll(
                    filter: v => veiculoIds.Contains(v.VeiculoId)
                )
                .Select(v => new
                {
                    veiculoId = v.VeiculoId,
                    ataId = ataId,
                    placa = v.Placa,
                    marcaModelo = v.MarcaModelo,
                    descricaoItem = "",
                    status = v.Status
                })
                .OrderBy(v => v.placa)
                .ToList();

                var qtdAtivos = veiculos.Count(v => v.status == true);
                var qtdInativos = veiculos.Count(v => v.status == false);

                return Ok(new { success = true, data = veiculos, qtdAtivos = qtdAtivos, qtdInativos = qtdInativos });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosAta", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetVeiculosDisponiveisAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar veículos ativos disponíveis (não vinculados à ata).
         *
         * 📥 ENTRADAS     : ataId (Guid) - Ata atual.
         *
         * 📤 SAÍDAS       : JSON com lista para dropdown.
         *
         * 🔗 CHAMADA POR  : Formulários de inclusão de veículos na ata.
         *
         * 🔄 CHAMA        : VeiculoAta.GetAll(), ViewVeiculos.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("GetVeiculosDisponiveisAta")]
        public IActionResult GetVeiculosDisponiveisAta(Guid ataId)
        {
            try
            {
                var veiculosNaAta = _unitOfWork.VeiculoAta.GetAll(
                    filter: va => va.AtaId == ataId
                ).Select(va => va.VeiculoId).ToList();

                var veiculos = _unitOfWork.ViewVeiculos.GetAll(
                    filter: v => v.Status == true && !veiculosNaAta.Contains(v.VeiculoId)
                )
                .Select(v => new
                {
                    value = v.VeiculoId,
                    text = $"{v.Placa} - {v.MarcaModelo}"
                })
                .OrderBy(v => v.text)
                .ToList();

                return Ok(new { success = true, data = veiculos });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "GetVeiculosDisponiveisAta", error);
                return Ok(new { success = false, data = new List<object>() });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: IncluirVeiculoAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Vincular veículo à ata selecionada.
         *
         * 📥 ENTRADAS     : [ICIncluirVeiculoAtaVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de inclusão de veículo na ata.
         *
         * 🔄 CHAMA        : VeiculoAta.Add(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("IncluirVeiculoAta")]
        public IActionResult IncluirVeiculoAta([FromBody] ICIncluirVeiculoAtaVM model)
        {
            try
            {
                var existe = _unitOfWork.VeiculoAta.GetFirstOrDefault(
                    va => va.VeiculoId == model.VeiculoId && va.AtaId == model.AtaId
                );

                if (existe != null)
                {
                    return Ok(new { success = false, message = "Este veículo já está associado à ata!" });
                }

                var veiculoAta = new VeiculoAta
                {
                    VeiculoId = model.VeiculoId,
                    AtaId = model.AtaId
                };

                _unitOfWork.VeiculoAta.Add(veiculoAta);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Veículo incluído na ata com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "IncluirVeiculoAta", error);
                return Ok(new { success = false, message = "Erro ao incluir veículo na ata" });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoverVeiculoAta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do veículo com a ata e limpar associações.
         *
         * 📥 ENTRADAS     : [ICRemoverVeiculoAtaVM] model.
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção de veículo da ata.
         *
         * 🔄 CHAMA        : VeiculoAta.Remove(), Veiculo.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("RemoverVeiculoAta")]
        public IActionResult RemoverVeiculoAta([FromBody] ICRemoverVeiculoAtaVM model)
        {
            try
            {
                var veiculoAta = _unitOfWork.VeiculoAta.GetFirstOrDefault(
                    va => va.VeiculoId == model.VeiculoId && va.AtaId == model.AtaId
                );

                if (veiculoAta == null)
                {
                    return Ok(new { success = false, message = "Associação não encontrada!" });
                }

                _unitOfWork.VeiculoAta.Remove(veiculoAta);

                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == model.VeiculoId);
                if (veiculo != null)
                {
                    veiculo.ItemVeiculoId = null;
                    veiculo.AtaId = null;
                    _unitOfWork.Veiculo.Update(veiculo);
                }

                _unitOfWork.Save();

                return Ok(new { success = true, message = "Veículo removido da ata com sucesso!" });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ItensContratoController.cs", "RemoverVeiculoAta", error);
                return Ok(new { success = false, message = "Erro ao remover veículo da ata" });
            }
        }
    }
}
