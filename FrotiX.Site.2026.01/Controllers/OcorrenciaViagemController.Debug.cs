/* ****************************************************************************************
 * ⚡ ARQUIVO: OcorrenciaViagemController.Debug.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Endpoints de debug para diagnosticar filtros e status de ocorrências.
 *
 * 📥 ENTRADAS     : Parâmetros simples via GET.
 *
 * 📤 SAÍDAS       : JSON com dados brutos e estatísticas.
 *
 * 🔗 CHAMADA POR  : Debug manual via URL.
 *
 * 🔄 CHAMA        : IUnitOfWork.OcorrenciaViagem, ViewVeiculos, ViewMotoristas.
 *
 * ⚠️ ATENÇÃO      : Arquivo temporário; remover após resolver problemas.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: OcorrenciaViagemController.Debug
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor métodos de debug para gestão de ocorrências.
     *
     * 📥 ENTRADAS     : Nenhuma (GET).
     *
     * 📤 SAÍDAS       : JSON com métricas e dados de teste.
     *
     * 🔗 CHAMADA POR  : Debug manual via URL.
     *
     * ⚠️ ATENÇÃO      : Remover após resolver problemas.
     ****************************************************************************************/
    public partial class OcorrenciaViagemController
    {
        #region DEBUG - REMOVER DEPOIS

        /****************************************************************************************
         * ⚡ FUNÇÃO: DebugListar
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista primeiros 10 registros SEM filtros para verificar estrutura
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : JSON com totalRegistros, primeiros10, contagemPorStatus, contagemPorStatusBool
         * 🔗 CHAMADA POR  : Debug manual via GET /DebugListar
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.GetAll()
         * 📦 DEPENDÊNCIAS : Repository Pattern
         ****************************************************************************************/
        [HttpGet]
        [Route("DebugListar")]
        public IActionResult DebugListar()
        {
            try
            {
                // Pega TODOS os registros sem filtro
                var todos = _unitOfWork.OcorrenciaViagem.GetAll().ToList();

                var resultado = new
                {
                    totalRegistros = todos.Count,
                    primeiros10 = todos.Take(10).Select(o => new
                    {
                        o.OcorrenciaViagemId,
                        o.ViagemId,
                        o.VeiculoId,
                        o.MotoristaId,
                        o.Resumo,
                        o.Descricao,
                        o.Status,
                        o.StatusOcorrencia,
                        o.DataCriacao,
                        o.DataBaixa,
                        o.UsuarioCriacao,
                        o.UsuarioBaixa,
                        o.ImagemOcorrencia,
                        o.Observacoes
                    }).ToList(),
                    // Contagem por status
                    contagemPorStatus = todos
                        .GroupBy(x => x.Status ?? "NULL")
                        .Select(g => new { status = g.Key, quantidade = g.Count() })
                        .ToList(),
                    // Contagem por StatusOcorrencia (bool)
                    contagemPorStatusBool = todos
                        .GroupBy(x => x.StatusOcorrencia)
                        .Select(g => new { statusBool = g.Key, quantidade = g.Count() })
                        .ToList()
                };

                return new JsonResult(resultado);
            }
            catch (Exception error)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = error.Message,
                    stackTrace = error.StackTrace
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DebugAbertas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Testa múltiplas variações de filtro para ocorrências abertas
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : JSON com contadores de cada tipo de filtro testado
         * 🔗 CHAMADA POR  : Debug manual via GET /DebugAbertas
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.GetAll()
         * 📦 DEPENDÊNCIAS : LINQ
         ****************************************************************************************/
        [HttpGet]
        [Route("DebugAbertas")]
        public IActionResult DebugAbertas()
        {
            try
            {
                var todos = _unitOfWork.OcorrenciaViagem.GetAll().ToList();

                // Teste 1: Status == "Aberta"
                var porStatusString = todos.Where(x => x.Status == "Aberta").Count();

                // Teste 2: Status nulo ou vazio
                var porStatusNulo = todos.Where(x => string.IsNullOrEmpty(x.Status)).Count();

                // Teste 3: StatusOcorrencia == true
                var porStatusBoolTrue = todos.Where(x => x.StatusOcorrencia == true).Count();

                // Teste 4: StatusOcorrencia == null
                var porStatusBoolNull = todos.Where(x => x.StatusOcorrencia == null).Count();

                // Teste 5: Combinado (como está no código)
                var combinado = todos.Where(x =>
                    x.Status == "Aberta" ||
                    string.IsNullOrEmpty(x.Status) ||
                    x.StatusOcorrencia == true
                ).Count();

                return new JsonResult(new
                {
                    totalRegistros = todos.Count,
                    porStatusStringAberta = porStatusString,
                    porStatusNuloOuVazio = porStatusNulo,
                    porStatusBoolTrue = porStatusBoolTrue,
                    porStatusBoolNull = porStatusBoolNull,
                    combinadoFiltroAtual = combinado,
                    // Mostra valores únicos de Status
                    valoresUnicosStatus = todos.Select(x => x.Status).Distinct().ToList(),
                    valoresUnicosStatusBool = todos.Select(x => x.StatusOcorrencia).Distinct().ToList()
                });
            }
            catch (Exception error)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = error.Message,
                    stackTrace = error.StackTrace
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DebugListarTodos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista últimas 50 ocorrências SEM filtros, com dados relacionados
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : JSON com data (array de ocorrências) incluindo campos de debug
         * 🔗 CHAMADA POR  : Debug manual via GET /DebugListarTodos
         * 🔄 CHAMA        : _unitOfWork (OcorrenciaViagem, Viagem, ViewVeiculos, ViewMotoristas)
         * 📦 DEPENDÊNCIAS : LINQ, Repository Pattern
         * 📝 OBSERVAÇÃO   : [DOC] Inclui campos _debug_status_original e _debug_statusBool_original
         ****************************************************************************************/
        [HttpGet]
        [Route("DebugListarTodos")]
        public IActionResult DebugListarTodos()
        {
            try
            {
                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll()
                    .OrderByDescending(x => x.DataCriacao)
                    .Take(50)
                    .ToList();

                if (!ocorrencias.Any())
                {
                    return new JsonResult(new { data = new List<object>(), mensagem = "Nenhum registro encontrado na tabela OcorrenciaViagem" });
                }

                var viagemIds = ocorrencias.Where(o => o.ViagemId != Guid.Empty).Select(o => o.ViagemId).Distinct().ToList();
                var veiculoIds = ocorrencias.Where(o => o.VeiculoId != Guid.Empty).Select(o => o.VeiculoId).Distinct().ToList();
                var motoristaIds = ocorrencias.Where(o => o.MotoristaId.HasValue && o.MotoristaId != Guid.Empty).Select(o => o.MotoristaId.Value).Distinct().ToList();

                var viagens = viagemIds.Any()
                    ? _unitOfWork.Viagem.GetAll(v => viagemIds.Contains(v.ViagemId)).ToDictionary(v => v.ViagemId)
                    : new Dictionary<Guid, Viagem>();

                var veiculos = veiculoIds.Any()
                    ? _unitOfWork.ViewVeiculos.GetAll(v => veiculoIds.Contains(v.VeiculoId)).ToDictionary(v => v.VeiculoId)
                    : new Dictionary<Guid, ViewVeiculos>();

                var motoristas = motoristaIds.Any()
                    ? _unitOfWork.ViewMotoristas.GetAll(m => motoristaIds.Contains(m.MotoristaId)).ToDictionary(m => m.MotoristaId)
                    : new Dictionary<Guid, ViewMotoristas>();

                var result = ocorrencias.Select(oc =>
                {
                    viagens.TryGetValue(oc.ViagemId, out var viagem);
                    veiculos.TryGetValue(oc.VeiculoId, out var veiculo);
                    ViewMotoristas motorista = null;
                    if (oc.MotoristaId.HasValue)
                        motoristas.TryGetValue(oc.MotoristaId.Value, out motorista);

                    var statusFinal = !string.IsNullOrEmpty(oc.Status) ? oc.Status :
                                      (oc.StatusOcorrencia == false ? "Baixada" : "Aberta");

                    return new
                    {
                        ocorrenciaViagemId = oc.OcorrenciaViagemId,
                        viagemId = oc.ViagemId,
                        noFichaVistoria = viagem?.NoFichaVistoria,
                        data = oc.DataCriacao.ToString("dd/MM/yyyy"),
                        nomeMotorista = motorista?.Nome ?? "",
                        descricaoVeiculo = veiculo?.VeiculoCompleto ?? "",
                        resumoOcorrencia = oc.Resumo ?? "",
                        descricaoOcorrencia = oc.Descricao ?? "",
                        descricaoSolucaoOcorrencia = oc.Observacoes ?? "",
                        statusOcorrencia = statusFinal,
                        imagemOcorrencia = oc.ImagemOcorrencia ?? "",
                        // DEBUG: campos originais
                        _debug_status_original = oc.Status,
                        _debug_statusBool_original = oc.StatusOcorrencia
                    };
                }).ToList();

                return new JsonResult(new { data = result });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "DebugListarTodos", error);
                return new JsonResult(new
                {
                    data = new List<object>(),
                    erro = error.Message,
                    stackTrace = error.StackTrace
                });
            }
        }

        #endregion
    }
}
