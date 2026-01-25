using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Services;

/*
 *  ╔═══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 *  ║                                      FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                ║
 *  ╠═══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 *  ║ (IA) CAMADA: CONTROLLERS (API)                                                                        ║
 *  ║ (IA) IDENTIDADE: ContratoController.cs                                                             ║
 *  ║ (IA) DESCRIÇÃO: Gerenciamento de Contratos, Vigências e Aditivos.                                     ║
 *  ║ (IA) PADRÃO: FrotiX 2026 Core                                                                         ║
 *  ╚═══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class ContratoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FrotiXDbContext _db;
        private readonly ILogService _log;

        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ContratoController (Constructor)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de contratos com UnitOfWork, DbContext e Log.   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita gestão de contratos com rastreabilidade e acesso ao banco.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios.                         ║
        /// ║    • db (FrotiXDbContext): contexto EF Core.                                 ║
        /// ║    • logService (ILogService): log centralizado.                              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public ContratoController(IUnitOfWork unitOfWork, FrotiXDbContext db, ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _db = db;
                _log = logService;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ContratoController", ex);
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista contratos com fornecedores e contadores de dependências.            ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Alimenta o grid administrativo com indicadores de vínculo.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contratos e contadores.                          ║
        /// ║    • Consumidor: UI de Contratos.                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.Contrato.GetAll()                                            ║
        /// ║    • _unitOfWork.Fornecedor.GetAll()                                          ║
        /// ║    • _unitOfWork.VeiculoContrato/Encarregado/Operador/Lavador/Motorista       ║
        /// ║    • _unitOfWork.Empenho/NotaFiscal/RepactuacaoContrato/ItemVeiculoContrato   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Contrato                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Contratos                                               ║
        /// ║    • Arquivos relacionados: Pages/Contrato/*.cshtml                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Busca contratos base com fornecedor para grid
                var contratos = (
                    from c in _unitOfWork.Contrato.GetAll()
                    join f in _unitOfWork.Fornecedor.GetAll()
                        on c.FornecedorId equals f.FornecedorId
                    orderby c.AnoContrato descending
                    select new
                    {
                        ContratoCompleto = c.AnoContrato + "/" + c.NumeroContrato,
                        ProcessoCompleto = c.NumeroProcesso
                            + "/"
                            + c.AnoProcesso.ToString().Substring(2, 2),
                        c.Objeto,
                        f.DescricaoFornecedor,
                        Periodo = c.DataInicio?.ToString("dd/MM/yy")
                            + " a "
                            + c.DataFim?.ToString("dd/MM/yy"),
                        ValorFormatado = c.Valor?.ToString("C"),
                        ValorMensal = (c.Valor / 12)?.ToString("C"),
                        VigenciaCompleta = c.Vigencia
                            + "ª vigência + "
                            + c.Prorrogacao
                            + " prorrog.",
                        c.Status,
                        c.ContratoId,
                    }
                ).ToList();

                // [LOGICA] Coleta IDs para processamento em lote
                var contratoIds = contratos.Select(c => c.ContratoId).ToList();

                // [DADOS] Inicializa dicionários de contagem de dependências
                var veiculosContrato = new Dictionary<Guid, int>();
                var encarregados = new Dictionary<Guid, int>();
                var operadores = new Dictionary<Guid, int>();
                var lavadores = new Dictionary<Guid, int>();
                var motoristas = new Dictionary<Guid, int>();
                var empenhosDict = new Dictionary<Guid, int>();
                var notasFiscais = new Dictionary<Guid, int>();
                var repactuacoes = new Dictionary<Guid, int>();
                var itensContrato = new Dictionary<Guid, int>();

                try
                {
                    // [DADOS] Contagem de veículos por contrato
                    veiculosContrato = _unitOfWork.VeiculoContrato
                        .GetAll(x => contratoIds.Contains(x.ContratoId))
                        .GroupBy(x => x.ContratoId)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de encarregados por contrato
                    encarregados = _unitOfWork.Encarregado
                        .GetAll(x => contratoIds.Contains(x.ContratoId))
                        .GroupBy(x => x.ContratoId)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de operadores por contrato
                    operadores = _unitOfWork.Operador
                        .GetAll(x => contratoIds.Contains(x.ContratoId))
                        .GroupBy(x => x.ContratoId)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de lavadores por contrato
                    lavadores = _unitOfWork.Lavador
                        .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                        .GroupBy(x => x.ContratoId.Value)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de motoristas por contrato
                    motoristas = _unitOfWork.Motorista
                        .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                        .GroupBy(x => x.ContratoId.Value)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de empenhos por contrato
                    empenhosDict = _unitOfWork.Empenho
                        .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                        .GroupBy(x => x.ContratoId.Value)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de notas fiscais por contrato
                    notasFiscais = _unitOfWork.NotaFiscal
                        .GetAll(x => x.ContratoId.HasValue && contratoIds.Contains(x.ContratoId.Value))
                        .GroupBy(x => x.ContratoId.Value)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                try
                {
                    // [DADOS] Contagem de repactuações por contrato
                    repactuacoes = _unitOfWork.RepactuacaoContrato
                        .GetAll(x => contratoIds.Contains(x.ContratoId))
                        .GroupBy(x => x.ContratoId)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                catch { }

                // [DADOS] Conta itens de contrato via repactuações
                try
                {
                    var repactuacoesIds = _unitOfWork.RepactuacaoContrato
                        .GetAll(x => contratoIds.Contains(x.ContratoId))
                        .ToList();

                    var repactuacaoIdList = repactuacoesIds.Select(r => r.RepactuacaoContratoId).ToList();

                    var itensCount = _unitOfWork.ItemVeiculoContrato
                        .GetAll(x => repactuacaoIdList.Contains(x.RepactuacaoContratoId))
                        .ToList();

                    // [LOGICA] Agrupa por ContratoId via repactuação
                    foreach (var repact in repactuacoesIds)
                    {
                        var qtdItens = itensCount.Count(i => i.RepactuacaoContratoId == repact.RepactuacaoContratoId);
                        if (qtdItens > 0)
                        {
                            if (itensContrato.ContainsKey(repact.ContratoId))
                                itensContrato[repact.ContratoId] += qtdItens;
                            else
                                itensContrato[repact.ContratoId] = qtdItens;
                        }
                    }
                }
                catch { }

                // [DADOS] Combina dados e contadores no resultado
                var result = contratos.Select(c => new
                {
                    c.ContratoCompleto,
                    c.ProcessoCompleto,
                    c.Objeto,
                    c.DescricaoFornecedor,
                    c.Periodo,
                    c.ValorFormatado,
                    c.ValorMensal,
                    c.VigenciaCompleta,
                    c.Status,
                    c.ContratoId,
                    // [DADOS] Atribuição de dependências
                    DepVeiculos = veiculosContrato.GetValueOrDefault(c.ContratoId, 0),
                    DepEncarregados = encarregados.GetValueOrDefault(c.ContratoId, 0),
                    DepOperadores = operadores.GetValueOrDefault(c.ContratoId, 0),
                    DepLavadores = lavadores.GetValueOrDefault(c.ContratoId, 0),
                    DepMotoristas = motoristas.GetValueOrDefault(c.ContratoId, 0),
                    DepEmpenhos = empenhosDict.GetValueOrDefault(c.ContratoId, 0),
                    DepNotasFiscais = notasFiscais.GetValueOrDefault(c.ContratoId, 0),
                    DepRepactuacoes = repactuacoes.GetValueOrDefault(c.ContratoId, 0),
                    DepItensContrato = itensContrato.GetValueOrDefault(c.ContratoId, 0)
                }).OrderByDescending(c => c.ContratoCompleto);

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em Get: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "Get", ex);
                return StatusCode(500);
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Remove um contrato e suas dependências em cascata (Repactuações e Itens), 
        /// desde que não existam bloqueios (Veículos ou Empenhos vinculados).
        /// </summary>
        /// <param name="model">ViewModel contendo o ID do contrato.</param>
        /// <returns>JSON indicando sucesso ou motivo do impedimento.</returns>
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(ContratoViewModel model)
        {
            try
            {
                if (model != null && model.ContratoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                        u.ContratoId == model.ContratoId
                    );

                    if (objFromDb != null)
                    {
                        // (IA) Verifica se existem veículos vinculados ao contrato para impedir a exclusão
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                            v.ContratoId == model.ContratoId
                        );

                        if (veiculo != null)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Existem veículos associados a esse contrato",
                            });
                        }

                        // (IA) Verifica se existem empenhos vinculados ao contrato
                        var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u =>
                            u.ContratoId == model.ContratoId
                        );

                        if (empenho != null)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Existem empenhos associados a esse contrato",
                            });
                        }

                        // (IA) Realiza a remoção em cascata: primeiro os itens das repactuações, depois as repactuações
                        var objRepactuacao = _unitOfWork.RepactuacaoContrato.GetAll(riv =>
                            riv.ContratoId == model.ContratoId
                        );

                        foreach (var repactuacao in objRepactuacao)
                        {
                            var objItemRepactuacao = _unitOfWork.ItemVeiculoContrato.GetAll(ivc =>
                                ivc.RepactuacaoContratoId == repactuacao.RepactuacaoContratoId
                            );

                            foreach (var itemveiculo in objItemRepactuacao)
                            {
                                _unitOfWork.ItemVeiculoContrato.Remove(itemveiculo);
                            }
                            _unitOfWork.RepactuacaoContrato.Remove(repactuacao);
                        }

                        _unitOfWork.Contrato.Remove(objFromDb);
                        _unitOfWork.Save();

                        return Json(new
                        {
                            success = true,
                            message = "Contrato removido com sucesso"
                        });
                    }
                }

                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar Contrato"
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em Delete: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "Delete", ex);
                return StatusCode(500);
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Altera o status (Ativo/Inativo) de um contrato e gera mensagem de log.
        /// </summary>
        /// <param name="Id">ID do contrato.</param>
        /// <returns>JSON com o novo status e a descrição da alteração.</returns>
        [Route("UpdateStatusContrato")]
        [HttpPost]
        public JsonResult UpdateStatusContrato(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Contrato.GetFirstOrDefault(u => u.ContratoId == Id);
                    string description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // (IA) Alterna o estado booleano do Status e define a mensagem descritiva
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            description = $"Atualizado Status do Contrato [Nome: {objFromDb.AnoContrato}/{objFromDb.NumeroContrato}] (Inativo)";
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            description = $"Atualizado Status do Contrato [Nome: {objFromDb.AnoContrato}/{objFromDb.NumeroContrato}] (Ativo)";
                            type = 0;
                        }

                        _unitOfWork.Contrato.Update(objFromDb);
                        _unitOfWork.Save();
                    }

                    return Json(new
                    {
                        success = true,
                        message = description,
                        type = type,
                    });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em UpdateStatusContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "UpdateStatusContrato", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Retorna uma lista de contratos formatada para DropDown/Select2.
        /// </summary>
        /// <param name="tipoContrato">Filtro opcional por tipo de contrato.</param>
        /// <returns>JSON contendo a lista para DropDown.</returns>
        [Route("ListaContratos")]
        [HttpGet]
        public async Task<JsonResult> OnGetListaContratos(string? tipoContrato = "")
        {
            try
            {
                // (IA) Utiliza o repositório especializado para buscar itens de drop-down de forma assíncrona
                var items = await _unitOfWork
                    .Contrato.GetDropDown(tipoContrato)
                    .ToListAsync();

                return new JsonResult(new { data = items });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em OnGetListaContratos: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "OnGetListaContratos", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Lista contratos do tipo 'Locação' ativos para uso em processos de Glosa.
        /// </summary>
        /// <param name="tipoContrato">Parâmetro ignorado mantido por compatibilidade.</param>
        /// <returns>JSON contendo a lista formatada.</returns>
        [Route("ListaContratosVeiculosGlosa")]
        [HttpGet]
        public async Task<JsonResult> ListaContratosVeiculosGlosa(string? tipoContrato = "")
        {
            try
            {
                // (IA) Busca contratos de locação ativos projetando para SelectListItem
                var contratos = await _db.Set<Contrato>()
                    .AsNoTracking()
                    .Where(c => c.TipoContrato == "Locação" && c.Status)
                    .OrderByDescending(c => c.AnoContrato)
                    .ThenByDescending(c => c.NumeroContrato)
                    .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor)
                    .Select(c => new SelectListItem
                    {
                        Value = c.ContratoId.ToString(),
                        Text = $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}",
                    })
                    .ToListAsync();

                return new JsonResult(new { data = contratos });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em ListaContratosVeiculosGlosa: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaContratosVeiculosGlosa", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Obtém detalhes específicos de configuração de um contrato pelo ID.
        /// </summary>
        /// <param name="id">ID do contrato.</param>
        /// <returns>JSON com os dados do contrato.</returns>
        [Route("PegaContrato")]
        [HttpGet]
        public IActionResult PegaContrato(Guid id)
        {
            try
            {
                // (IA) Projeta campos específicos necessários para o frontend
                var result = (
                    from c in _unitOfWork.Contrato.GetAll()
                    where c.ContratoId == id
                    select new
                    {
                        c.ContratoLavadores,
                        c.ContratoMotoristas,
                        c.ContratoOperadores,
                        c.TipoContrato,
                        c.ContratoId,
                    }
                ).ToList();

                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em PegaContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "PegaContrato", ex);
                return StatusCode(500);
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Insere um novo contrato e gera automaticamente a primeira repactuação com o valor inicial.
        /// </summary>
        /// <param name="contrato">Objeto do contrato a ser inserido.</param>
        /// <returns>JSON indicando sucesso ou erro de duplicidade.</returns>
        [Route("InsereContrato")]
        [HttpPost]
        public JsonResult InsereContrato(Models.Contrato contrato)
        {
            try
            {
                // (IA) Validação de unicidade para evitar contratos duplicados (Ano/Número)
                var existeContrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                    (u.AnoContrato == contrato.AnoContrato)
                    && (u.NumeroContrato == contrato.NumeroContrato)
                );

                if (existeContrato != null && existeContrato.ContratoId != contrato.ContratoId)
                {
                    return new JsonResult(new
                    {
                        data = "00000000-0000-0000-0000-000000000000",
                        message = "Já existe um contrato com esse número",
                    });
                }

                _unitOfWork.Contrato.Add(contrato);

                // (IA) Cria a primeira repactuação automática vinculada ao contrato recém criado
                var objRepactuacao = new RepactuacaoContrato();
                objRepactuacao.DataRepactuacao = contrato.DataInicio;
                objRepactuacao.Descricao = "Valor Inicial";
                objRepactuacao.ContratoId = contrato.ContratoId;
                objRepactuacao.Valor = contrato.Valor;
                _unitOfWork.RepactuacaoContrato.Add(objRepactuacao);

                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = objRepactuacao.RepactuacaoContratoId,
                    message = "Contrato Adicionado com Sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em InsereContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereContrato", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Adiciona uma nova repactuação (aditivo de valor/prazo) ao contrato e atualiza o cabeçalho do contrato.
        /// </summary>
        /// <param name="repactuacaoContrato">Objeto da repactuação.</param>
        /// <returns>JSON com o ID da nova repactuação.</returns>
        [Route("InsereRepactuacao")]
        [HttpPost]
        public JsonResult InsereRepactuacao(RepactuacaoContrato repactuacaoContrato)
        {
            try
            {
                var objRepactuacao = new RepactuacaoContrato();
                objRepactuacao.DataRepactuacao = repactuacaoContrato.DataRepactuacao;
                objRepactuacao.Valor = repactuacaoContrato.Valor;
                objRepactuacao.Descricao = repactuacaoContrato.Descricao;
                objRepactuacao.ContratoId = repactuacaoContrato.ContratoId;
                objRepactuacao.Vigencia = repactuacaoContrato.Vigencia;
                objRepactuacao.Prorrogacao = repactuacaoContrato.Prorrogacao;

                _unitOfWork.RepactuacaoContrato.Add(objRepactuacao);

                // (IA) Atualiza os dados de vigência e valor no contrato principal para refletir a última alteração
                var objContrato = _unitOfWork.Contrato.GetFirstOrDefault(c =>
                    c.ContratoId == repactuacaoContrato.ContratoId
                );

                if (objContrato != null)
                {
                    objContrato.Valor = repactuacaoContrato.Valor;
                    objContrato.DataRepactuacao = repactuacaoContrato.DataRepactuacao;
                    objContrato.Prorrogacao = repactuacaoContrato.Prorrogacao;
                    objContrato.Vigencia = repactuacaoContrato.Vigencia;
                    _unitOfWork.Contrato.Update(objContrato);
                }

                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = objRepactuacao.RepactuacaoContratoId,
                    message = "Repactuação Adicionada com Sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em InsereRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereRepactuacao", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Atualiza dados de uma repactuação existente e opcionalmente sincroniza com o contrato principal.
        /// </summary>
        /// <param name="repactuacaoContrato">Objeto da repactuação a ser atualizada.</param>
        /// <returns>JSON com o resultado da atualização.</returns>
        [Route("AtualizaRepactuacao")]
        [HttpPost]
        public JsonResult AtualizaRepactuacao(RepactuacaoContrato repactuacaoContrato)
        {
            try
            {
                _unitOfWork.RepactuacaoContrato.Update(repactuacaoContrato);

                // (IA) Verifica se a flag 'AtualizaContrato' está ativa para replicar os dados no objeto Contrato
                if (repactuacaoContrato.AtualizaContrato == true)
                {
                    var objContrato = _unitOfWork.Contrato.GetFirstOrDefault(c =>
                        c.ContratoId == repactuacaoContrato.ContratoId
                    );

                    if (objContrato != null)
                    {
                        objContrato.Valor = repactuacaoContrato.Valor;
                        objContrato.DataRepactuacao = repactuacaoContrato.DataRepactuacao;
                        objContrato.Prorrogacao = repactuacaoContrato.Prorrogacao;
                        objContrato.Vigencia = repactuacaoContrato.Vigencia;
                        _unitOfWork.Contrato.Update(objContrato);
                    }
                }

                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = repactuacaoContrato.RepactuacaoContratoId,
                    message = "Repactuação Atualizada com Sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em AtualizaRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaRepactuacao", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Edita as informações básicas de um contrato após validar se o número não está em uso por outro registro.
        /// </summary>
        /// <param name="contrato">Objeto do contrato com os novos dados.</param>
        /// <returns>JSON indicando sucesso ou erro de duplicidade.</returns>
        [Route("EditaContrato")]
        [HttpPost]
        public JsonResult EditaContrato(Models.Contrato contrato)
        {
            try
            {
                // (IA) Validação de unicidade para o par Ano/Número do contrato
                var existeContrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                    (u.AnoContrato == contrato.AnoContrato)
                    && (u.NumeroContrato == contrato.NumeroContrato)
                );

                if (existeContrato != null && existeContrato.ContratoId != contrato.ContratoId)
                {
                    return new JsonResult(new
                    {
                        data = "00000000-0000-0000-0000-000000000000",
                        message = "Já existe um contrato com esse número",
                    });
                }

                _unitOfWork.Contrato.Update(contrato);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = contrato,
                    message = "Contrato Atualizado com Sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em EditaContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "EditaContrato", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Adiciona um novo item (veículo/equipamento) vinculado a uma repactuação específica.
        /// </summary>
        /// <param name="itemveiculo">Objeto do item a ser inserido.</param>
        /// <returns>JSON com o ID do item criado.</returns>
        [Route("InsereItemContrato")]
        [HttpPost]
        public JsonResult InsereItemContrato(ItemVeiculoContrato itemveiculo)
        {
            try
            {
                _unitOfWork.ItemVeiculoContrato.Add(itemveiculo);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = itemveiculo.ItemVeiculoId,
                    message = "Item Veiculo Contrato adicionado com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em InsereItemContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereItemContrato", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Atualiza um item de contrato ou insere um novo caso o ID seja nulo/vazio.
        /// </summary>
        /// <param name="itemveiculo">Objeto do item de veículo contrato.</param>
        /// <returns>JSON com o ID do item processado.</returns>
        [Route("AtualizaItemContrato")]
        [HttpPost]
        public JsonResult AtualizaItemContrato(ItemVeiculoContrato itemveiculo)
        {
            try
            {
                if (itemveiculo.ItemVeiculoId == Guid.Empty)
                {
                    // (IA) Fallback para criação caso o ID não tenha sido gerado pelo frontend
                    var newItemContrato = new ItemVeiculoContrato();
                    newItemContrato.NumItem = itemveiculo.NumItem;
                    newItemContrato.Quantidade = itemveiculo.Quantidade;
                    newItemContrato.Descricao = itemveiculo.Descricao;
                    newItemContrato.ValorUnitario = itemveiculo.ValorUnitario;
                    newItemContrato.RepactuacaoContratoId = itemveiculo.RepactuacaoContratoId;

                    _unitOfWork.ItemVeiculoContrato.Add(newItemContrato);
                }
                else
                {
                    _unitOfWork.ItemVeiculoContrato.Update(itemveiculo);
                }

                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = itemveiculo.ItemVeiculoId,
                    message = "Item Veiculo Contrato atualizado com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em AtualizaItemContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaItemContrato", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Remove um item específico de um contrato.
        /// </summary>
        /// <param name="itemveiculo">Objeto do item a ser removido.</param>
        /// <returns>JSON com o ID do item removido.</returns>
        [Route("ApagaItemContrato")]
        [HttpPost]
        public JsonResult ApagaItemContrato(ItemVeiculoContrato itemveiculo)
        {
            try
            {
                _unitOfWork.ItemVeiculoContrato.Remove(itemveiculo);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = itemveiculo.ItemVeiculoId,
                    message = "Item Veiculo Contrato Eliminado com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em ApagaItemContrato: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ApagaItemContrato", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Adiciona uma repactuação específica para contratos de terceirização de frota.
        /// </summary>
        /// <param name="repactuacaoTerceirizacao">Objeto da repactuação de terceirização.</param>
        /// <returns>JSON com o ID gerado.</returns>
        [Route("InsereRepactuacaoTerceirizacao")]
        [HttpPost]
        public JsonResult InsereRepactuacaoTerceirizacao(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
        {
            try
            {
                _unitOfWork.RepactuacaoTerceirizacao.Add(repactuacaoTerceirizacao);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId,
                    message = "Repactuação do Contrato adicionada com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em InsereRepactuacaoTerceirizacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereRepactuacaoTerceirizacao", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Atualiza uma repactuação de terceirização existente.
        /// </summary>
        /// <param name="repactuacaoTerceirizacao">Objeto da repactuação de terceirização.</param>
        /// <returns>JSON indicando sucesso.</returns>
        [Route("AtualizaRepactuacaoTerceirizacao")]
        [HttpPost]
        public JsonResult AtualizaRepactuacaoTerceirizacao(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
        {
            try
            {
                _unitOfWork.RepactuacaoTerceirizacao.Update(repactuacaoTerceirizacao);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId,
                    message = "Repactuação do Contrato adicionada com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em AtualizaRepactuacaoTerceirizacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaRepactuacaoTerceirizacao", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Adiciona uma repactuação específica para contratos de prestação de serviços.
        /// </summary>
        /// <param name="repactuacaoServicos">Objeto da repactuação de serviços.</param>
        /// <returns>JSON com o ID gerado.</returns>
        [Route("InsereRepactuacaoServicos")]
        [HttpPost]
        public JsonResult InsereRepactuacaoServicos(RepactuacaoServicos repactuacaoServicos)
        {
            try
            {
                _unitOfWork.RepactuacaoServicos.Add(repactuacaoServicos);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = repactuacaoServicos.RepactuacaoServicoId,
                    message = "Repactuação do Contrato adicionada com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em InsereRepactuacaoServicos: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "InsereRepactuacaoServicos", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Atualiza uma repactuação de serviços existente.
        /// </summary>
        /// <param name="repactuacaoServicos">Objeto da repactuação de serviços.</param>
        /// <returns>JSON indicando sucesso.</returns>
        [Route("AtualizaRepactuacaoServicos")]
        [HttpPost]
        public JsonResult AtualizaRepactuacaoServicos(RepactuacaoServicos repactuacaoServicos)
        {
            try
            {
                _unitOfWork.RepactuacaoServicos.Update(repactuacaoServicos);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    data = repactuacaoServicos.RepactuacaoServicoId,
                    message = "Repactuação do Contrato adicionada com sucesso",
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em AtualizaRepactuacaoServicos: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "AtualizaRepactuacaoServicos", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Lista todas as repactuações associadas a um contrato específico.
        /// </summary>
        /// <param name="id">ID do contrato.</param>
        /// <returns>JSON com a lista de repactuações formatada.</returns>
        [Route("RepactuacaoList")]
        [HttpGet]
        public JsonResult RepactuacaoList(Guid id)
        {
            try
            {
                // (IA) Projeta os dados das repactuações formatando valores e datas para exibição
                var repactuacoes = (
                    from r in _unitOfWork.RepactuacaoContrato.GetAll()
                    where r.ContratoId == id
                    orderby r.DataRepactuacao descending, r.Prorrogacao descending
                    select new
                    {
                        DataFormatada = r.DataRepactuacao?.ToString("dd/MM/yy"),
                        r.Descricao,
                        r.RepactuacaoContratoId,
                        Valor = r.Valor?.ToString("C"),
                        ValorMensal = (r.Valor / 12)?.ToString("C"),
                        r.Vigencia,
                        r.Prorrogacao,
                        Repactuacao = "(" + r.DataRepactuacao?.ToString("dd/MM/yy") + ") " + r.Descricao,
                    }
                ).ToList();

                return new JsonResult(new { data = repactuacoes });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em RepactuacaoList: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "RepactuacaoList", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        [Route("RecuperaTipoContrato")]
        [HttpGet]
        public ActionResult RecuperaTipoContrato(Guid Id)
        {
            try
            {
                var contratoObj = _unitOfWork.Contrato.GetFirstOrDefault(c => c.ContratoId == Id);

                return Json(new
                {
                    data = contratoObj
                });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "ContratoController.cs", "RecuperaTipoContrato");
                Alerta.TratamentoErroComLinha(
                    "ContratoController.cs",
                    "RecuperaTipoContrato",
                    error
                );
                return StatusCode(500);
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Recupera os detalhes de uma repactuação de terceirização pelo ID.
        /// </summary>
        /// <param name="RepactuacaoContratoId">ID da repactuação (string).</param>
        /// <returns>JSON com o objeto da repactuação.</returns>
        [Route("RecuperaRepactuacaoTerceirizacao")]
        [HttpGet]
        public ActionResult RecuperaRepactuacaoTerceirizacao(string RepactuacaoContratoId)
        {
            try
            {
                var objRepactuacaoTerceirizacao = _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault(r =>
                    r.RepactuacaoContratoId == Guid.Parse(RepactuacaoContratoId)
                );

                return Json(new { objRepactuacaoTerceirizacao });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em RecuperaRepactuacaoTerceirizacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaRepactuacaoTerceirizacao", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Verifica se existe algum veículo vinculado aos itens de uma determinada repactuação.
        /// </summary>
        /// <param name="RepactuacaoContratoId">ID da repactuação.</param>
        /// <returns>JSON indicando a existência de vínculo.</returns>
        [Route("ExisteItem")]
        [HttpGet]
        public ActionResult ExisteItem(Guid RepactuacaoContratoId)
        {
            try
            {
                // (IA) Realiza join entre Veículo e ItemVeiculoContrato para detectar dependências ativas
                var vinculos = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join ivc in _unitOfWork.ItemVeiculoContrato.GetAll()
                        on v.ItemVeiculoId equals ivc.ItemVeiculoId
                    where ivc.RepactuacaoContratoId == RepactuacaoContratoId
                    select v.VeiculoId
                ).Any();

                return Json(new { existeItem = vinculos });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em ExisteItem: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ExisteItem", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Exclui uma repactuação e todos os itens (Locação, Terceirização ou Serviços) associados a ela.
        /// </summary>
        /// <param name="Id">ID da repactuação.</param>
        /// <returns>JSON com o resultado da exclusão ou mensagem de erro se houver veículos vinculados.</returns>
        [Route("ApagaRepactuacao")]
        [HttpGet]
        public JsonResult ApagaRepactuacao(Guid Id)
        {
            try
            {
                try
                {
                    // (IA) Limpeza em cascata de itens de locação vinculados à repactuação
                    var objRepactuacaoLocacao = _unitOfWork.ItemVeiculoContrato.GetAll(iv =>
                        iv.RepactuacaoContratoId == Id
                    );
                    foreach (var itemLocacao in objRepactuacaoLocacao)
                    {
                        _unitOfWork.ItemVeiculoContrato.Remove(itemLocacao);
                    }

                    // (IA) Limpeza de itens de terceirização
                    var objRepactuacaoTerceirizacao = _unitOfWork.RepactuacaoTerceirizacao.GetAll(
                        rt => rt.RepactuacaoContratoId == Id
                    );
                    foreach (var itemTerceirizacao in objRepactuacaoTerceirizacao)
                    {
                        _unitOfWork.RepactuacaoTerceirizacao.Remove(itemTerceirizacao);
                    }

                    // (IA) Limpeza de itens de serviços
                    var objRepactuacaoServicos = _unitOfWork.RepactuacaoServicos.GetAll(rs =>
                        rs.RepactuacaoContratoId == Id
                    );
                    foreach (var itemServico in objRepactuacaoServicos)
                    {
                        _unitOfWork.RepactuacaoServicos.Remove(itemServico);
                    }

                    // (IA) Por fim, remove o registro principal da repactuação
                    var objRepactuacao = _unitOfWork.RepactuacaoContrato.GetFirstOrDefault(rc =>
                        rc.RepactuacaoContratoId == Id
                    );
                    
                    if (objRepactuacao != null)
                        _unitOfWork.RepactuacaoContrato.Remove(objRepactuacao);

                    _unitOfWork.Save();

                    return new JsonResult(new { success = true, message = "Repactuação Excluída com Sucesso!" });
                }
                catch (Exception)
                {
                    // (IA) Captura erro de chave estrangeira caso existam veículos vinculados que não puderam ser removidos
                    return new JsonResult(new { success = false, message = "Existem Veículos Associados a Essa Repactuação!" });
                }
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em ApagaRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ApagaRepactuacao", ex);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// <summary>
        /// (IA) Recupera o ID da última repactuação realizada para um determinado contrato.
        /// </summary>
        /// <param name="contratoId">ID do contrato.</param>
        /// <returns>GUID da última repactuação ou Guid.Empty.</returns>
        [Route("UltimaRepactuacao")]
        [HttpGet]
        public IActionResult UltimaRepactuacao(Guid contratoId)
        {
            try
            {
                try
                {
                    var objRepactuacao = _unitOfWork
                        .RepactuacaoContrato.GetAll(rc => rc.ContratoId == contratoId)
                        .OrderByDescending(rc => rc.DataRepactuacao)
                        .First();

                    return Json(objRepactuacao.RepactuacaoContratoId);
                }
                catch (Exception)
                {
                    return Json(Guid.Empty);
                }
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em UltimaRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "UltimaRepactuacao", ex);
                return StatusCode(500);
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// (IA) Recupera todos os itens vinculados a uma repactuação específica, indicando se já possuem veículos associados.
        /// </summary>
        /// <param name="repactuacaoContratoId">ID da repactuação.</param>
        /// <returns>JSON com a lista de itens e status de vínculo.</returns>
        [Route("RecuperaItensUltimaRepactuacao")]
        [HttpGet]
        public IActionResult RecuperaItensUltimaRepactuacao(Guid repactuacaoContratoId)
        {
            try
            {
                // (IA) Busca itens da repactuação e lista de IDs de itens já utilizados por veículos
                var itens = _unitOfWork.ItemVeiculoContrato.GetAll()
                    .Where(ivc => ivc.RepactuacaoContratoId == repactuacaoContratoId)
                    .ToList();

                var veiculosComItem = _unitOfWork.Veiculo.GetAll()
                    .Where(v => v.ItemVeiculoId != null)
                    .Select(v => v.ItemVeiculoId)
                    .ToList();

                // (IA) Projeta os dados dos itens formatando moedas e verificando dependências
                var result = itens
                    .OrderBy(ivc => ivc.NumItem)
                    .Select(ivc => new
                    {
                        ivc.ItemVeiculoId,
                        ivc.RepactuacaoContratoId,
                        ivc.NumItem,
                        ivc.Descricao,
                        ivc.Quantidade,
                        valUnitario = ivc.ValorUnitario?.ToString("C"),
                        valTotal = (ivc.ValorUnitario * ivc.Quantidade)?.ToString("C"),
                        ExisteVeiculo = veiculosComItem.Contains(ivc.ItemVeiculoId)
                    })
                    .ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em RecuperaItensUltimaRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaItensUltimaRepactuacao", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Lista os itens de uma repactuação (método de conveniência que chama RecuperaItensUltimaRepactuacao).
        /// </summary>
        /// <param name="repactuacaoContratoId">ID da repactuação.</param>
        /// <returns>IActionResult com os itens.</returns>
        [Route("ListaItensRepactuacao")]
        [HttpGet]
        public IActionResult ListaItensRepactuacao(Guid repactuacaoContratoId)
        {
            try
            {
                return RecuperaItensUltimaRepactuacao(repactuacaoContratoId);
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em ListaItensRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "ListaItensRepactuacao", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Busca todos os dados de uma repactuação, incluindo informações específicas baseadas no tipo de contrato (Terceirização ou Serviços).
        /// </summary>
        /// <param name="repactuacaoContratoId">ID da repactuação.</param>
        /// <returns>JSON com dados da repactuação e dados específicos.</returns>
        [Route("RecuperaRepactuacaoCompleta")]
        [HttpGet]
        public IActionResult RecuperaRepactuacaoCompleta(Guid repactuacaoContratoId)
        {
            try
            {
                var repactuacao = _unitOfWork.RepactuacaoContrato.GetFirstOrDefault(
                    r => r.RepactuacaoContratoId == repactuacaoContratoId
                );

                if (repactuacao == null)
                {
                    return Json(new { success = false, message = "Repactuação não encontrada" });
                }

                var contrato = _unitOfWork.Contrato.GetFirstOrDefault(
                    c => c.ContratoId == repactuacao.ContratoId
                );

                object dadosEspecificos = null;
                var tipoContrato = contrato?.TipoContrato?.ToLower() ?? "";

                // (IA) Lógica condicional para buscar dados complementares conforme o tipo de contrato
                if (tipoContrato.Contains("terceiriz"))
                {
                    var terceirizacao = _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault(
                        t => t.RepactuacaoContratoId == repactuacaoContratoId
                    );
                    if (terceirizacao != null)
                    {
                        dadosEspecificos = new
                        {
                            valorEncarregado = terceirizacao.ValorEncarregado,
                            qtdEncarregados = terceirizacao.QtdEncarregados,
                            valorOperador = terceirizacao.ValorOperador,
                            qtdOperadores = terceirizacao.QtdOperadores,
                            valorMotorista = terceirizacao.ValorMotorista,
                            qtdMotoristas = terceirizacao.QtdMotoristas,
                            valorLavador = terceirizacao.ValorLavador,
                            qtdLavadores = terceirizacao.QtdLavadores
                        };
                    }
                }
                else if (tipoContrato.Contains("servic"))
                {
                    var servicos = _unitOfWork.RepactuacaoServicos.GetFirstOrDefault(
                        s => s.RepactuacaoContratoId == repactuacaoContratoId
                    );
                    if (servicos != null)
                    {
                        dadosEspecificos = new { valor = servicos.Valor };
                    }
                }

                return Json(new
                {
                    success = true,
                    repactuacao = new
                    {
                        repactuacao.RepactuacaoContratoId,
                        repactuacao.ContratoId,
                        repactuacao.DataRepactuacao,
                        repactuacao.Descricao,
                        repactuacao.Valor,
                        repactuacao.Vigencia,
                        repactuacao.Prorrogacao
                    },
                    tipoContrato = contrato?.TipoContrato,
                    dadosEspecificos
                });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em RecuperaRepactuacaoCompleta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "RecuperaRepactuacaoCompleta", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// (IA) Move os veículos vinculados a itens de uma repactuação anterior para os novos itens correspondentes em uma repactuação mais recente.
        /// </summary>
        /// <param name="model">ViewModel com os IDs das repactuações de origem e destino.</param>
        /// <returns>JSON indicando a quantidade de veículos movidos.</returns>
        [Route("MoverVeiculosRepactuacao")]
        [HttpPost]
        public IActionResult MoverVeiculosRepactuacao([FromBody] MoverVeiculosViewModel model)
        {
            try
            {
                // (IA) Busca a nova repactuação para validar o contexto do contrato
                var novaRepactuacao = _unitOfWork.RepactuacaoContrato.GetFirstOrDefault(
                    r => r.RepactuacaoContratoId == model.NovaRepactuacaoId
                );

                if (novaRepactuacao == null)
                {
                    return Json(new { success = false, message = "Nova repactuação não encontrada" });
                }

                // (IA) Recupera os novos itens para servir de destino na migração
                var itensNovos = _unitOfWork.ItemVeiculoContrato.GetAll()
                    .Where(ivc => ivc.RepactuacaoContratoId == model.NovaRepactuacaoId)
                    .ToList();

                if (!itensNovos.Any())
                {
                    return Json(new { success = false, message = "Não há itens na nova repactuação" });
                }

                // (IA) Idenfifica todas as repactuações anteriores do mesmo contrato
                var repactuacoesAnteriores = _unitOfWork.RepactuacaoContrato.GetAll()
                    .Where(r => r.ContratoId == novaRepactuacao.ContratoId &&
                                r.RepactuacaoContratoId != model.NovaRepactuacaoId)
                    .Select(r => r.RepactuacaoContratoId)
                    .ToList();

                // (IA) Coleta os itens dessas repactuações para mapear os veículos vinculados
                var itensAnteriores = _unitOfWork.ItemVeiculoContrato.GetAll()
                    .Where(ivc => repactuacoesAnteriores.Contains(ivc.RepactuacaoContratoId))
                    .ToList();

                if (!itensAnteriores.Any())
                {
                    return Json(new { success = false, message = "Não há itens nas repactuações anteriores" });
                }

                int veiculosMovidos = 0;

                // (IA) Para cada item novo, busca veículos nos itens anteriores equivalentes (mesmo NumItem) e atualiza o vínculo
                foreach (var itemNovo in itensNovos)
                {
                    var itensCorrespondentes = itensAnteriores
                        .Where(i => i.NumItem == itemNovo.NumItem)
                        .ToList();

                    foreach (var itemAnterior in itensCorrespondentes)
                    {
                        var veiculos = _unitOfWork.Veiculo.GetAll()
                            .Where(v => v.ItemVeiculoId == itemAnterior.ItemVeiculoId)
                            .ToList();

                        foreach (var veiculo in veiculos)
                        {
                            veiculo.ItemVeiculoId = itemNovo.ItemVeiculoId;
                            _unitOfWork.Veiculo.Update(veiculo);
                            veiculosMovidos++;
                        }
                    }
                }

                if (veiculosMovidos > 0)
                {
                    _unitOfWork.Save();
                    return Json(new
                    {
                        success = true,
                        message = $"{veiculosMovidos} veículo(s) movido(s) para a nova repactuação"
                    });
                }

                return Json(new { success = false, message = "Nenhum veículo encontrado para mover" });
            }
            catch (Exception ex)
            {
                _log.Error("[ContratoController] Erro em MoverVeiculosRepactuacao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("ContratoController.cs", "MoverVeiculosRepactuacao", ex);
                return StatusCode(500);
            }
        }
    }

    /// <summary>
    /// ViewModel para mover veículos entre repactuações
    /// </summary>
    public class MoverVeiculosViewModel
    {
        public Guid RepactuacaoAnteriorId { get; set; }
        public Guid NovaRepactuacaoId { get; set; }
    }
}

