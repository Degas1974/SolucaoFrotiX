using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  GESTÃO DE ABASTECIMENTOS (PENDÊNCIAS)                                              #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    public partial class AbastecimentoController : ControllerBase
    {
        public class PendenciaDTO
        {
            [JsonPropertyName("abastecimentoPendenteId")]
            public string AbastecimentoPendenteId { get; set; }
            [JsonPropertyName("autorizacaoQCard")]
            public int? AutorizacaoQCard { get; set; }
            [JsonPropertyName("placa")]
            public string Placa { get; set; }
            [JsonPropertyName("nomeMotorista")]
            public string NomeMotorista { get; set; }
            [JsonPropertyName("produto")]
            public string Produto { get; set; }
            [JsonPropertyName("dataHora")]
            public string DataHora { get; set; }
            [JsonPropertyName("kmAnterior")]
            public int? KmAnterior { get; set; }
            [JsonPropertyName("km")]
            public int? Km { get; set; }
            [JsonPropertyName("kmRodado")]
            public int? KmRodado { get; set; }
            [JsonPropertyName("litros")]
            public string Litros { get; set; }
            [JsonPropertyName("valorUnitario")]
            public string ValorUnitario { get; set; }
            [JsonPropertyName("valorTotal")]
            public string ValorTotal { get; set; }
            [JsonPropertyName("descricaoPendencia")]
            public string DescricaoPendencia { get; set; }
            [JsonPropertyName("tipoPendencia")]
            public string TipoPendencia { get; set; }
            [JsonPropertyName("iconePendencia")]
            public string IconePendencia { get; set; }
            [JsonPropertyName("temSugestao")]
            public bool TemSugestao { get; set; }
            [JsonPropertyName("campoCorrecao")]
            public string CampoCorrecao { get; set; }
            [JsonPropertyName("valorAtualErrado")]
            public int? ValorAtualErrado { get; set; }
            [JsonPropertyName("valorSugerido")]
            public int? ValorSugerido { get; set; }
            [JsonPropertyName("justificativaSugestao")]
            public string JustificativaSugestao { get; set; }
            [JsonPropertyName("dataImportacao")]
            public string DataImportacao { get; set; }
            [JsonPropertyName("arquivoOrigem")]
            public string ArquivoOrigem { get; set; }
            [JsonPropertyName("veiculoId")]
            public string VeiculoId { get; set; }
            [JsonPropertyName("motoristaId")]
            public string MotoristaId { get; set; }
            [JsonPropertyName("combustivelId")]
            public string CombustivelId { get; set; }
            [JsonPropertyName("codMotorista")]
            public int? CodMotorista { get; set; }
        }

        public class EditarPendenciaRequest
        {
            public string AbastecimentoPendenteId { get; set; }
            public int? AutorizacaoQCard { get; set; }
            public string Placa { get; set; }
            public int? CodMotorista { get; set; }
            public string Produto { get; set; }
            public string DataHora { get; set; }
            public int? KmAnterior { get; set; }
            public int? Km { get; set; }
            public double? Litros { get; set; }
            public double? ValorUnitario { get; set; }
            public string VeiculoId { get; set; }
            public string MotoristaId { get; set; }
            public string CombustivelId { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListarPendencias (GET)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista pendências de abastecimento para correção.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com pendências.                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ListarPendencias")]
        [HttpGet]
        public IActionResult ListarPendencias()
        {
            try
            {
                var pendencias = _unitOfWork.AbastecimentoPendente.GetAll()
                    .Where(p => p.Status == 0)
                    .OrderByDescending(p => p.DataImportacao)
                    .ThenBy(p => p.NumeroLinhaOriginal)
                    .ToList();

                var resultado = pendencias.Select(p => new PendenciaDTO
                {
                    AbastecimentoPendenteId = p.AbastecimentoPendenteId.ToString(),
                    AutorizacaoQCard = p.AutorizacaoQCard,
                    Placa = p.Placa,
                    NomeMotorista = p.NomeMotorista,
                    Produto = p.Produto,
                    DataHora = p.DataHora?.ToString("dd/MM/yyyy HH:mm"),
                    KmAnterior = p.KmAnterior,
                    Km = p.Km,
                    KmRodado = p.KmRodado,
                    Litros = p.Litros?.ToString("N2"),
                    ValorUnitario = p.ValorUnitario?.ToString("C2", new CultureInfo("pt-BR")),
                    ValorTotal = (p.Litros * p.ValorUnitario)?.ToString("C2", new CultureInfo("pt-BR")),
                    DescricaoPendencia = p.DescricaoPendencia,
                    TipoPendencia = p.TipoPendencia,
                    IconePendencia = ObterIconePendencia(p.TipoPendencia),
                    TemSugestao = p.TemSugestao,
                    CampoCorrecao = p.CampoCorrecao,
                    ValorAtualErrado = p.ValorAtualErrado,
                    ValorSugerido = p.ValorSugerido,
                    JustificativaSugestao = p.JustificativaSugestao,
                    DataImportacao = p.DataImportacao.ToString("dd/MM/yyyy HH:mm"),
                    ArquivoOrigem = p.ArquivoOrigem,
                    VeiculoId = p.VeiculoId?.ToString(),
                    MotoristaId = p.MotoristaId?.ToString(),
                    CombustivelId = p.CombustivelId?.ToString(),
                    CodMotorista = p.CodMotorista
                }).ToList();

                return Ok(new { data = resultado });
            }
            catch (Exception error)
            {
                _logger.LogError("Erro ao listar pendências de abastecimento", error, "AbastecimentoController.Pendencias.cs", "ListarPendencias");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ListarPendencias", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ContarPendencias (GET)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Estatísticas de pendências por tipo.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com totais.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ContarPendencias")]
        [HttpGet]
        public IActionResult ContarPendencias()
        {
            try
            {
                var pendencias = _unitOfWork.AbastecimentoPendente.GetAll()
                    .Where(p => p.Status == 0)
                    .ToList();

                var totais = new
                {
                    total = pendencias.Count,
                    veiculo = pendencias.Count(p => p.TipoPendencia == "veiculo"),
                    motorista = pendencias.Count(p => p.TipoPendencia == "motorista"),
                    km = pendencias.Count(p => p.TipoPendencia == "km"),
                    autorizacao = pendencias.Count(p => p.TipoPendencia == "autorizacao"),
                    litros = pendencias.Count(p => p.TipoPendencia == "litros"),
                    data = pendencias.Count(p => p.TipoPendencia == "data"),
                    corrigiveis = pendencias.Count(p => p.TemSugestao)
                };

                return Ok(totais);
            }
            catch (Exception error)
            {
                _logger.LogError("Erro ao contabilizar pendências", error, "AbastecimentoController.Pendencias.cs", "ContarPendencias");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ContarPendencias", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterPendencia (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna detalhes de pendência por ID.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com detalhes.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ObterPendencia")]
        [HttpGet]
        public IActionResult ObterPendencia(string id)
        {
            try
            {
                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
                    p.AbastecimentoPendenteId == Guid.Parse(id));

                if (pendencia == null)
                {
                    return Ok(new { success = false, message = "Pendência não encontrada" });
                }

                var resultado = new PendenciaDTO
                {
                    AbastecimentoPendenteId = pendencia.AbastecimentoPendenteId.ToString(),
                    AutorizacaoQCard = pendencia.AutorizacaoQCard,
                    Placa = pendencia.Placa,
                    NomeMotorista = pendencia.NomeMotorista,
                    Produto = pendencia.Produto,
                    DataHora = pendencia.DataHora?.ToString("dd/MM/yyyy HH:mm"),
                    KmAnterior = pendencia.KmAnterior,
                    Km = pendencia.Km,
                    KmRodado = pendencia.KmRodado,
                    Litros = pendencia.Litros?.ToString("N2"),
                    ValorUnitario = pendencia.ValorUnitario?.ToString("N2"),
                    DescricaoPendencia = pendencia.DescricaoPendencia,
                    TipoPendencia = pendencia.TipoPendencia,
                    TemSugestao = pendencia.TemSugestao,
                    CampoCorrecao = pendencia.CampoCorrecao,
                    ValorAtualErrado = pendencia.ValorAtualErrado,
                    ValorSugerido = pendencia.ValorSugerido,
                    JustificativaSugestao = pendencia.JustificativaSugestao,
                    VeiculoId = pendencia.VeiculoId?.ToString(),
                    MotoristaId = pendencia.MotoristaId?.ToString(),
                    CombustivelId = pendencia.CombustivelId?.ToString(),
                    CodMotorista = pendencia.CodMotorista
                };

                return Ok(new { success = true, data = resultado });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ObterPendencia", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ResolverPendencia (POST)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Resolve pendência criando abastecimento e atualizando status.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (EditarPendenciaRequest)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ResolverPendencia")]
        [HttpPost]
        public IActionResult ResolverPendencia([FromBody] EditarPendenciaRequest request)
        {
            try
            {
                ModelState.Remove("Veiculo");
                ModelState.Remove("Motorista");
                ModelState.Remove("Combustivel");

                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
                    p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));

                if (pendencia == null)
                {
                    return Ok(new { success = false, message = "Pendência não encontrada" });
                }

                var erros = ValidarDadosAbastecimento(request);
                if (erros.Any())
                {
                    return Ok(new { success = false, message = string.Join("; ", erros) });
                }

                if (request.AutorizacaoQCard.HasValue)
                {
                    var existenteAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a =>
                        a.AutorizacaoQCard == request.AutorizacaoQCard.Value);

                    if (existenteAbastecimento != null)
                    {
                        pendencia.Status = 1;
                        _unitOfWork.AbastecimentoPendente.Update(pendencia);
                        _unitOfWork.Save();

                        return Ok(new { success = true, message = $"Autorização {request.AutorizacaoQCard} já foi importada anteriormente. Pendência removida." });
                    }
                }

                if (!DateTime.TryParse(request.DataHora, out DateTime dataHora))
                {
                    return Ok(new { success = false, message = "Data/hora inválida" });
                }

                int kmRodado = (request.Km ?? 0) - (request.KmAnterior ?? 0);

                var abastecimento = new Abastecimento
                {
                    AbastecimentoId = Guid.NewGuid(),
                    DataHora = dataHora,
                    VeiculoId = Guid.Parse(request.VeiculoId),
                    MotoristaId = Guid.Parse(request.MotoristaId),
                    CombustivelId = Guid.Parse(request.CombustivelId),
                    AutorizacaoQCard = request.AutorizacaoQCard,
                    Litros = request.Litros,
                    ValorUnitario = request.ValorUnitario,
                    Hodometro = request.Km,
                    KmRodado = kmRodado
                };

                _unitOfWork.Abastecimento.Add(abastecimento);

                pendencia.Status = 1;
                _unitOfWork.AbastecimentoPendente.Update(pendencia);

                _unitOfWork.Save();

                AtualizarMediaConsumoVeiculo(abastecimento.VeiculoId);

                double consumoFinal = request.Litros > 0 ? kmRodado / request.Litros.Value : 0;

                _logger.LogInformation($"Pendência {request.AbastecimentoPendenteId} resolvida com sucesso");

                return Ok(new
                {
                    success = true,
                    message = $"Abastecimento importado com sucesso! KM Rodado: {kmRodado} km, Consumo: {consumoFinal:N2} km/l"
                });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ResolverPendencia", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SalvarPendencia (POST)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Salva alterações na pendência sem resolver.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (EditarPendenciaRequest)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("SalvarPendencia")]
        [HttpPost]
        public IActionResult SalvarPendencia([FromBody] EditarPendenciaRequest request)
        {
            try
            {
                ModelState.Remove("Veiculo");
                ModelState.Remove("Motorista");
                ModelState.Remove("Combustivel");

                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
                    p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));

                if (pendencia == null)
                {
                    return Ok(new { success = false, message = "Pendência não encontrada" });
                }

                pendencia.AutorizacaoQCard = request.AutorizacaoQCard;
                pendencia.Placa = request.Placa;
                pendencia.CodMotorista = request.CodMotorista;
                pendencia.Litros = request.Litros;
                pendencia.ValorUnitario = request.ValorUnitario;
                pendencia.KmAnterior = request.KmAnterior;
                pendencia.Km = request.Km;
                pendencia.KmRodado = (request.Km ?? 0) - (request.KmAnterior ?? 0);

                if (!string.IsNullOrEmpty(request.VeiculoId))
                    pendencia.VeiculoId = Guid.Parse(request.VeiculoId);

                if (!string.IsNullOrEmpty(request.MotoristaId))
                    pendencia.MotoristaId = Guid.Parse(request.MotoristaId);

                if (!string.IsNullOrEmpty(request.CombustivelId))
                    pendencia.CombustivelId = Guid.Parse(request.CombustivelId);

                if (DateTime.TryParse(request.DataHora, out DateTime dataHora))
                {
                    pendencia.DataHora = dataHora;
                }

                if (pendencia.MotoristaId.HasValue)
                {
                    var motorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                        m.MotoristaId == pendencia.MotoristaId.Value);
                    if (motorista != null)
                    {
                        pendencia.NomeMotorista = motorista.Nome;
                    }
                }

                var novosPendencias = RevalidarPendencia(pendencia);
                pendencia.DescricaoPendencia = novosPendencias.descricao;
                pendencia.TipoPendencia = novosPendencias.tipo;
                pendencia.TemSugestao = novosPendencias.temSugestao;

                _unitOfWork.AbastecimentoPendente.Update(pendencia);
                _unitOfWork.Save();

                return Ok(new
                {
                    success = true,
                    message = "Pendência atualizada com sucesso",
                    pendenciasRestantes = !string.IsNullOrEmpty(novosPendencias.descricao)
                });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "SalvarPendencia", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SalvarEImportarPendencia (POST)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Salva alterações e tenta resolver imediatamente.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (EditarPendenciaRequest)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("SalvarEImportarPendencia")]
        [HttpPost]
        public IActionResult SalvarEImportarPendencia([FromBody] EditarPendenciaRequest request)
        {
            try
            {
                ModelState.Remove("Veiculo");
                ModelState.Remove("Motorista");
                ModelState.Remove("Combustivel");

                var resultadoSalvar = SalvarPendenciaInterno(request);
                if (!resultadoSalvar.success)
                {
                    return Ok(resultadoSalvar);
                }

                return ResolverPendencia(request);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "SalvarEImportarPendencia", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExcluirPendencia (POST)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exclui pendência específica.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (EditarPendenciaRequest)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ExcluirPendencia")]
        [HttpPost]
        public IActionResult ExcluirPendencia([FromBody] EditarPendenciaRequest request)
        {
            try
            {
                ModelState.Remove("Veiculo");
                ModelState.Remove("Motorista");
                ModelState.Remove("Combustivel");

                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
                    p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));

                if (pendencia == null)
                {
                    return Ok(new { success = false, message = "Pendência não encontrada" });
                }

                _unitOfWork.AbastecimentoPendente.Remove(pendencia);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Pendência excluída com sucesso" });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ExcluirPendencia", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExcluirTodasPendencias (POST)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exclui todas as pendências não resolvidas.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ExcluirTodasPendencias")]
        [HttpPost]
        public IActionResult ExcluirTodasPendencias()
        {
            try
            {
                var pendencias = _unitOfWork.AbastecimentoPendente.GetAll()
                    .Where(p => p.Status == 0)
                    .ToList();

                if (!pendencias.Any())
                {
                    return Ok(new { success = false, message = "Não há pendências para excluir" });
                }

                int quantidade = pendencias.Count;

                foreach (var pendencia in pendencias)
                {
                    _unitOfWork.AbastecimentoPendente.Remove(pendencia);
                }

                _unitOfWork.Save();

                _logger.LogInformation($"{quantidade} pendências excluídas manualmente");

                return Ok(new { success = true, message = $"{quantidade} pendência(s) excluída(s) com sucesso" });
            }
            catch (Exception error)
            {
                _logger.LogError("Erro ao excluir todas as pendências", error, "AbastecimentoController.Pendencias.cs", "ExcluirTodasPendencias");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "ExcluirTodasPendencias", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AplicarSugestao (POST)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Aplica sugestão automática em uma pendência.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (EditarPendenciaRequest)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AplicarSugestao")]
        [HttpPost]
        public IActionResult AplicarSugestao([FromBody] EditarPendenciaRequest request)
        {
            try
            {
                ModelState.Remove("Veiculo");
                ModelState.Remove("Motorista");
                ModelState.Remove("Combustivel");

                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
                    p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));

                if (pendencia == null)
                {
                    return Ok(new { success = false, message = "Pendência não encontrada" });
                }

                if (!pendencia.TemSugestao || !pendencia.ValorSugerido.HasValue)
                {
                    return Ok(new { success = false, message = "Esta pendência não possui sugestão de correção" });
                }

                if (pendencia.CampoCorrecao == "KmAnterior")
                {
                    pendencia.KmAnterior = (int?)pendencia.ValorSugerido;
                }
                else if (pendencia.CampoCorrecao == "Km")
                {
                    pendencia.Km = (int?)pendencia.ValorSugerido;
                }

                pendencia.KmRodado = (pendencia.Km ?? 0) - (pendencia.KmAnterior ?? 0);

                pendencia.TemSugestao = false;
                pendencia.CampoCorrecao = null;
                pendencia.ValorAtualErrado = null;
                pendencia.ValorSugerido = null;
                pendencia.JustificativaSugestao = null;

                var novosPendencias = RevalidarPendencia(pendencia);
                pendencia.DescricaoPendencia = novosPendencias.descricao;
                pendencia.TipoPendencia = novosPendencias.tipo;

                _unitOfWork.AbastecimentoPendente.Update(pendencia);
                _unitOfWork.Save();

                _logger.LogInformation($"Sugestão aplicada na pendência {request.AbastecimentoPendenteId}");

                return Ok(new
                {
                    success = true,
                    message = "Sugestão aplicada com sucesso",
                    kmRodado = pendencia.KmRodado,
                    pendenciasRestantes = !string.IsNullOrEmpty(novosPendencias.descricao)
                });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "AplicarSugestao", error);
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterIconePendencia (Privado)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Mapeia tipo de erro para ícone FontAwesome Duotone.                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private string ObterIconePendencia(string tipo)
        {
            return tipo switch
            {
                "autorizacao" => "fa-duotone fa-ban",
                "motorista" => "fa-duotone fa-user-xmark",
                "veiculo" => "fa-duotone fa-car-burst",
                "litros" => "fa-duotone fa-gas-pump",
                "km" => "fa-duotone fa-gauge-high",
                "data" => "fa-duotone fa-calendar-xmark",
                _ => "fa-duotone fa-circle-xmark"
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ValidarDadosAbastecimento (Privado)                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Valida dados recebidos para resolução de pendências.                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private List<string> ValidarDadosAbastecimento(EditarPendenciaRequest request)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(request.VeiculoId))
                erros.Add("Veículo não identificado");

            if (string.IsNullOrEmpty(request.MotoristaId))
                erros.Add("Motorista não identificado");

            if (string.IsNullOrEmpty(request.CombustivelId))
                erros.Add("Combustível não identificado");

            if (!request.Litros.HasValue || request.Litros <= 0)
                erros.Add("Quantidade de litros inválida");

            if (!request.ValorUnitario.HasValue || request.ValorUnitario <= 0)
                erros.Add("Valor unitário inválido");

            if (string.IsNullOrEmpty(request.DataHora))
                erros.Add("Data/hora não informada");

            int kmRodado = (request.Km ?? 0) - (request.KmAnterior ?? 0);
            if (kmRodado < 0)
                erros.Add("Quilometragem negativa: KM Anterior maior que KM Atual");

            if (kmRodado > 1000 && request.Litros > 0)
            {
                double consumo = kmRodado / request.Litros.Value;
                if (consumo > 20)
                    erros.Add($"Consumo de {consumo:N1} km/l parece muito alto");
            }

            return erros;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RevalidarPendencia (Privado)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Reanalisa pendência após edição.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private (string descricao, string tipo, bool temSugestao) RevalidarPendencia(AbastecimentoPendente pendencia)
        {
            var erros = new List<string>();
            string tipoPrincipal = "erro";
            bool temSugestao = false;

            if (!pendencia.VeiculoId.HasValue)
            {
                erros.Add($"Veículo de placa '{pendencia.Placa}' não cadastrado");
                tipoPrincipal = "veiculo";
            }

            if (!pendencia.MotoristaId.HasValue)
            {
                erros.Add($"Motorista com código QCard '{pendencia.CodMotorista}' não cadastrado");
                if (tipoPrincipal == "erro") tipoPrincipal = "motorista";
            }

            if (!pendencia.CombustivelId.HasValue)
            {
                erros.Add("Combustível não identificado");
                if (tipoPrincipal == "erro") tipoPrincipal = "combustivel";
            }

            if (pendencia.AutorizacaoQCard.HasValue)
            {
                var existente = _unitOfWork.Abastecimento.GetFirstOrDefault(a =>
                    a.AutorizacaoQCard == pendencia.AutorizacaoQCard.Value);

                if (existente != null)
                {
                    erros.Add($"Autorização '{pendencia.AutorizacaoQCard}' já foi importada anteriormente");
                    tipoPrincipal = "autorizacao";
                }
            }

            int kmRodado = (pendencia.Km ?? 0) - (pendencia.KmAnterior ?? 0);
            if (kmRodado < 0)
            {
                erros.Add($"Quilometragem negativa ({kmRodado} km): KM Anterior maior que KM Atual");
                if (tipoPrincipal == "erro") tipoPrincipal = "km";
            }
            else if (kmRodado > 1000 && pendencia.Litros > 0)
            {
                double consumo = kmRodado / pendencia.Litros.Value;
                if (consumo > 15)
                {
                    erros.Add($"Quilometragem de {kmRodado} km resulta em consumo de {consumo:N1} km/l (acima do esperado)");
                    if (tipoPrincipal == "erro") tipoPrincipal = "km";
                }
            }

            return (string.Join("; ", erros), erros.Any() ? tipoPrincipal : null, temSugestao);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SalvarPendenciaInterno (Privado)                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Persiste alterações de pendência no banco.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private (bool success, string message) SalvarPendenciaInterno(EditarPendenciaRequest request)
        {
            try
            {
                var pendencia = _unitOfWork.AbastecimentoPendente.GetFirstOrDefault(p =>
                    p.AbastecimentoPendenteId == Guid.Parse(request.AbastecimentoPendenteId));

                if (pendencia == null)
                {
                    return (false, "Pendência não encontrada");
                }

                pendencia.AutorizacaoQCard = request.AutorizacaoQCard;
                pendencia.Placa = request.Placa;
                pendencia.CodMotorista = request.CodMotorista;
                pendencia.Litros = request.Litros;
                pendencia.ValorUnitario = request.ValorUnitario;
                pendencia.KmAnterior = request.KmAnterior;
                pendencia.Km = request.Km;
                pendencia.KmRodado = (request.Km ?? 0) - (request.KmAnterior ?? 0);

                if (!string.IsNullOrEmpty(request.VeiculoId))
                    pendencia.VeiculoId = Guid.Parse(request.VeiculoId);

                if (!string.IsNullOrEmpty(request.MotoristaId))
                    pendencia.MotoristaId = Guid.Parse(request.MotoristaId);

                if (!string.IsNullOrEmpty(request.CombustivelId))
                    pendencia.CombustivelId = Guid.Parse(request.CombustivelId);

                if (DateTime.TryParse(request.DataHora, out DateTime dataHora))
                {
                    pendencia.DataHora = dataHora;
                }

                _unitOfWork.AbastecimentoPendente.Update(pendencia);
                _unitOfWork.Save();

                return (true, "Pendência atualizada");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "SalvarPendenciaInterno", error);
                throw;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarMediaConsumoVeiculo (Privado)                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recalcula e atualiza a média de consumo do veículo.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private void AtualizarMediaConsumoVeiculo(Guid veiculoId)
        {
            try
            {
                var mediaConsumo = _unitOfWork.ViewMediaConsumo.GetFirstOrDefault(v =>
                    v.VeiculoId == veiculoId);

                if (mediaConsumo != null)
                {
                    var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                        v.VeiculoId == veiculoId);

                    if (veiculo != null)
                    {
                        veiculo.Consumo = (double?)mediaConsumo.ConsumoGeral;
                        _unitOfWork.Veiculo.Update(veiculo);
                        _unitOfWork.Save();
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "`$1");
                Alerta.TratamentoErroComLinha("AbastecimentoController.Pendencias.cs", "AtualizarMediaConsumoVeiculo", error);
            }
        }
    }
}
