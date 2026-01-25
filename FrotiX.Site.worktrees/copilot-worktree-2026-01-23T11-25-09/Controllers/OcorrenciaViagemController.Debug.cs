using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  GESTÃO DE VIAGENS (OCORRÊNCIAS)                                                  #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OcorrenciaViagemController (Debug)                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Métodos de DEBUG para gestão de ocorrências.                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class OcorrenciaViagemController
    {
        #region DEBUG - REMOVER DEPOIS

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DebugListar (GET)                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista os primeiros 10 registros sem filtro para verificação de estrutura. ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com amostra e contagens.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("DebugListar")]
        public IActionResult DebugListar()
        {
            try
            {
                // [DADOS] Pega todos os registros sem filtro.
                var todos = _unitOfWork.OcorrenciaViagem.GetAll().ToList();

                // [MONTAGEM] Estrutura de retorno.
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

                // [RETORNO] Resultado de diagnóstico.
                return new JsonResult(resultado);
            }
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.DebugListar", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugListar", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DebugAbertas (GET)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Testa o filtro de ocorrências abertas com combinações de campos.         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contagens por critério.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("DebugAbertas")]
        public IActionResult DebugAbertas()
        {
            try
            {
                // [DADOS] Carrega todas as ocorrências.
                var todos = _unitOfWork.OcorrenciaViagem.GetAll().ToList();

                // [TESTE] Status == "Aberta".
                var porStatusString = todos.Where(x => x.Status == "Aberta").Count();

                // [TESTE] Status nulo ou vazio.
                var porStatusNulo = todos.Where(x => string.IsNullOrEmpty(x.Status)).Count();

                // [TESTE] StatusOcorrencia == true.
                var porStatusBoolTrue = todos.Where(x => x.StatusOcorrencia == true).Count();

                // [TESTE] StatusOcorrencia == null.
                var porStatusBoolNull = todos.Where(x => x.StatusOcorrencia == null).Count();

                // [TESTE] Combinado (como está no código).
                var combinado = todos.Where(x =>
                    x.Status == "Aberta" ||
                    string.IsNullOrEmpty(x.Status) ||
                    x.StatusOcorrencia == true
                ).Count();

                // [RETORNO] Resultado de diagnóstico.
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
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.DebugAbertas", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugAbertas", ex);
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DebugListarTodos (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista sem filtro, ordenado e limitado aos últimos 50 registros.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados de diagnóstico.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("DebugListarTodos")]
        public IActionResult DebugListarTodos()
        {
            try
            {
                // [DADOS] Carrega últimas ocorrências.
                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll()
                    .OrderByDescending(x => x.DataCriacao)
                    .Take(50)
                    .ToList();

                if (!ocorrencias.Any())
                {
                    // [RETORNO] Lista vazia.
                    return new JsonResult(new { data = new List<object>(), mensagem = "Nenhum registro encontrado na tabela OcorrenciaViagem" });
                }

                // [DADOS] Coleta IDs relacionados.
                var viagemIds = ocorrencias.Where(o => o.ViagemId != Guid.Empty).Select(o => o.ViagemId).Distinct().ToList();
                var veiculoIds = ocorrencias.Where(o => o.VeiculoId != Guid.Empty).Select(o => o.VeiculoId).Distinct().ToList();
                var motoristaIds = ocorrencias.Where(o => o.MotoristaId.HasValue && o.MotoristaId != Guid.Empty).Select(o => o.MotoristaId.Value).Distinct().ToList();

                // [DADOS] Carrega dicionários de apoio.
                var viagens = viagemIds.Any()
                    ? _unitOfWork.Viagem.GetAll(v => viagemIds.Contains(v.ViagemId)).ToDictionary(v => v.ViagemId)
                    : new Dictionary<Guid, Viagem>();

                var veiculos = veiculoIds.Any()
                    ? _unitOfWork.ViewVeiculos.GetAll(v => veiculoIds.Contains(v.VeiculoId)).ToDictionary(v => v.VeiculoId)
                    : new Dictionary<Guid, ViewVeiculos>();

                var motoristas = motoristaIds.Any()
                    ? _unitOfWork.ViewMotoristas.GetAll(m => motoristaIds.Contains(m.MotoristaId)).ToDictionary(m => m.MotoristaId)
                    : new Dictionary<Guid, ViewMotoristas>();

                // [MONTAGEM] Projeção com dados consolidados.
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
            catch (Exception ex)
            {
                _log.Error("OcorrenciaViagemController.DebugListarTodos", ex);
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.Debug.cs", "DebugListarTodos", ex);
                return new JsonResult(new
                {
                    data = new List<object>(),
                    erro = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        #endregion
    }
}
