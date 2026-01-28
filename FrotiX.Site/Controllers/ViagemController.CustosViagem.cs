/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.CustosViagem.cs                                 ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - CustosViagem)
     * 🎯 OBJETIVO: Obter custos detalhados de viagem com cálculo inteligente de combustível
     * 📋 ROTAS: /api/Viagem/ObterCustosViagem [GET]
     * 🔗 ENTIDADES: Viagem, Veiculo, Combustivel, Abastecimento, MediaCombustivel
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 📊 CÁLCULOS:
     *    1. Duração (data/hora início → data/hora fim)
     *    2. Km percorrido (KmFinal - KmInicial)
     *    3. Litros gastos (km / consumo médio veículo OU histórico abastecimentos)
     *    4. Preço combustível (abastecimento mais próximo da data)
     *    5. Custo combustível (litros × preço)
     *    6. Custos totais (Motorista + Veículo + Combustível + Operador + Lavador)
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterCustosViagem
         * 🎯 OBJETIVO: Calcular e retornar custos detalhados de uma viagem (combustível inteligente)
         * 📥 ENTRADAS: viagemId (Guid da viagem)
         * 📤 SAÍDAS: JSON { success, data: CustosDTO com 11 campos }
         * 🔗 CHAMADA POR: Modal de visualização de custos de viagem
         * 🔄 CHAMA: Viagem.GetFirstOrDefaultAsync(), Abastecimento.GetAll(), MediaCombustivel.GetAll()
         * 📊 ALGORITMO (6 etapas):
         *    1. Calcula duração (data/hora fim - data/hora início)
         *    2. Calcula km percorrido (KmFinal - KmInicial)
         *    3. Determina consumo veículo (Veiculo.Consumo OU média histórica abastecimentos)
         *    4. Calcula litros gastos (km / consumo)
         *    5. Busca preço combustível (abastecimento mais próximo OU média mensal)
         *    6. Calcula custos: combustível (litros × preço) + outros custos
         * 💡 LÓGICA INTELIGENTE:
         *    - Prioriza dados reais sobre estimativas
         *    - Fallback: consumo médio histórico se veículo não tem cadastro
         *    - Preço: abastecimento mais próximo da data (Math.Abs diferença dias)
         ****************************************************************************************/
        [Route("ObterCustosViagem")]
        [HttpGet]
        public async Task<IActionResult> ObterCustosViagem(Guid viagemId)
        {
            try
            {
                if (viagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID da viagem inválido"
                    });
                }

                // Busca a viagem com relacionamentos
                var viagem = await _unitOfWork.Viagem.GetFirstOrDefaultAsync(
                    filter: v => v.ViagemId == viagemId,
                    includeProperties: "Veiculo,Veiculo.Combustivel,Motorista,Requisitante,SetorSolicitante"
                );

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // [DOC] ========== ETAPA 1: CÁLCULO DE DURAÇÃO (data/hora fim - data/hora início) ==========
                double duracaoMinutos = 0;
                string duracaoFormatada = "-";
                if (viagem.DataInicial.HasValue && viagem.HoraInicio.HasValue &&
                    viagem.DataFinal.HasValue && viagem.HoraFim.HasValue)
                {
                    var dataHoraInicio = viagem.DataInicial.Value.Date + viagem.HoraInicio.Value.TimeOfDay;
                    var dataHoraFim = viagem.DataFinal.Value.Date + viagem.HoraFim.Value.TimeOfDay;
                    var diferenca = dataHoraFim - dataHoraInicio;
                    duracaoMinutos = diferenca.TotalMinutes;

                    if (duracaoMinutos > 0)
                    {
                        int horas = (int)(duracaoMinutos / 60);
                        int mins = (int)(duracaoMinutos % 60);
                        duracaoFormatada = horas > 0 ? $"{horas}h {mins}min" : $"{mins}min";
                    }
                }

                // [DOC] ========== ETAPA 2: CÁLCULO DE KM PERCORRIDO (KmFinal - KmInicial) ==========
                int kmPercorrido = 0;
                if (viagem.KmFinal.HasValue && viagem.KmInicial.HasValue)
                {
                    kmPercorrido = viagem.KmFinal.Value - viagem.KmInicial.Value;
                    if (kmPercorrido < 0) kmPercorrido = 0;
                }

                // [DOC] ========== TIPO DE COMBUSTÍVEL (para buscar preço depois) ==========
                string tipoCombustivel = "-";
                Guid? combustivelId = null;
                if (viagem.Veiculo != null && viagem.Veiculo.Combustivel != null)
                {
                    tipoCombustivel = viagem.Veiculo.Combustivel.Descricao ?? "-";
                    combustivelId = viagem.Veiculo.CombustivelId;
                }

                // [DOC] ========== ETAPA 3: LÓGICA INTELIGENTE PARA DETERMINAR CONSUMO VEÍCULO ==========
                double litrosGastos = 0;
                double consumoVeiculo = 0;

                // [DOC] Estratégia 1: Usa consumo cadastrado no veículo (prioridade)
                if (viagem.Veiculo != null && viagem.Veiculo.Consumo.HasValue && viagem.Veiculo.Consumo.Value > 0)
                {
                    consumoVeiculo = viagem.Veiculo.Consumo.Value;
                }
                else
                {
                    // [DOC] Estratégia 2: Fallback - calcula média histórica de abastecimentos do veículo
                    if (viagem.VeiculoId.HasValue)
                    {
                        var abastecimentosHistorico = _unitOfWork.Abastecimento
                            .GetAll()
                            .Where(a => a.VeiculoId == viagem.VeiculoId.Value
                                        && a.Litros.HasValue && a.Litros.Value > 0
                                        && a.KmRodado.HasValue && a.KmRodado.Value > 0)
                            .ToList();

                        if (abastecimentosHistorico.Any())
                        {
                            // [DOC] Média de consumo: soma(km) / soma(litros) = km/L médio do veículo
                            double totalKm = abastecimentosHistorico.Sum(a => a.KmRodado.Value);
                            double totalLitros = abastecimentosHistorico.Sum(a => a.Litros.Value);
                            if (totalLitros > 0)
                            {
                                consumoVeiculo = totalKm / totalLitros;
                            }
                        }
                    }
                }

                // [DOC] ========== ETAPA 4: CALCULA LITROS GASTOS NA VIAGEM ==========
                // REGRA DE NEGÓCIO: km percorrido / consumo médio (km/L)
                // IMPORTANTE: NÃO usa abastecimentos do período, pois um abastecimento serve múltiplas viagens
                if (kmPercorrido > 0 && consumoVeiculo > 0)
                {
                    litrosGastos = kmPercorrido / consumoVeiculo;
                }

                // [DOC] ========== ETAPA 5: BUSCA PREÇO DO COMBUSTÍVEL (2 estratégias) ==========
                double precoCombustivel = 0;

                // [DOC] Estratégia 1: Busca abastecimento mais PRÓXIMO da data da viagem (prioridade)
                if (combustivelId.HasValue && viagem.DataInicial.HasValue)
                {
                    var dataViagem = viagem.DataInicial.Value;

                    // [DOC] OrderBy com Math.Abs garante o abastecimento com menor diferença de dias
                    var abastecimentoProximo = _unitOfWork.Abastecimento
                        .GetAll()
                        .Where(a => a.CombustivelId == combustivelId.Value
                                    && a.ValorUnitario.HasValue
                                    && a.ValorUnitario.Value > 0
                                    && a.DataHora.HasValue)
                        .OrderBy(a => Math.Abs((a.DataHora.Value - dataViagem).TotalDays))
                        .FirstOrDefault();

                    if (abastecimentoProximo != null && abastecimentoProximo.ValorUnitario.HasValue)
                    {
                        precoCombustivel = abastecimentoProximo.ValorUnitario.Value;
                    }
                }

                // [DOC] Estratégia 2: Fallback - usa média mensal de combustível (mais recente)
                if (precoCombustivel == 0 && combustivelId.HasValue)
                {
                    var mediaCombustivel = _unitOfWork.MediaCombustivel
                        .GetAll()
                        .Where(m => m.CombustivelId == combustivelId.Value)
                        .OrderByDescending(m => m.Ano)
                        .ThenByDescending(m => m.Mes)
                        .FirstOrDefault();

                    if (mediaCombustivel != null)
                    {
                        precoCombustivel = mediaCombustivel.PrecoMedio;
                    }
                }

                // [DOC] ========== ETAPA 6: CÁLCULO FINAL DE CUSTOS ==========
                // Custo combustível: litros gastos × preço por litro
                double custoCombustivelCalculado = 0;
                if (litrosGastos > 0 && precoCombustivel > 0)
                {
                    custoCombustivelCalculado = litrosGastos * precoCombustivel;
                }

                // [DOC] Prioriza valores reais (da viagem) sobre valores calculados
                double custoMotorista = viagem.CustoMotorista ?? 0;
                double custoVeiculo = viagem.CustoVeiculo ?? 0;
                double custoCombustivel = viagem.CustoCombustivel ?? custoCombustivelCalculado;
                double custoOperador = viagem.CustoOperador ?? 0;
                double custoLavador = viagem.CustoLavador ?? 0;

                // [DOC] Se custo real é 0 mas temos estimativa, usa a estimativa
                if ((viagem.CustoCombustivel ?? 0) == 0 && custoCombustivelCalculado > 0)
                {
                    custoCombustivel = custoCombustivelCalculado;
                }

                // [DOC] Ajuste reverso: se tem custo real mas litros não foi calculado, calcula retroativamente
                if ((viagem.CustoCombustivel ?? 0) > 0 && precoCombustivel > 0 && litrosGastos == 0)
                {
                    litrosGastos = viagem.CustoCombustivel.Value / precoCombustivel;
                }

                // [DOC] Custo total: soma de todos os componentes
                double custoTotal = custoMotorista + custoVeiculo + custoCombustivel + custoOperador + custoLavador;

                // ========== CÁLCULO DE CONSUMO (KM/L) ==========
                double consumo = 0;
                string consumoFormatado = "-";
                if (kmPercorrido > 0 && litrosGastos > 0)
                {
                    consumo = kmPercorrido / litrosGastos;
                    consumoFormatado = $"{consumo:F2} km/l";
                }
                else if (consumoVeiculo > 0)
                {
                    // Se não conseguiu calcular, usa o consumo médio do veículo
                    consumo = consumoVeiculo;
                    consumoFormatado = $"{consumo:F2} km/l (média)";
                }

                // ========== INFORMAÇÕES DA VIAGEM ==========
                string infoViagem = "";
                if (viagem.DataInicial.HasValue)
                {
                    infoViagem = viagem.DataInicial.Value.ToString("dd/MM/yyyy");
                    if (viagem.HoraInicio.HasValue)
                    {
                        infoViagem += $" às {viagem.HoraInicio.Value:HH:mm}";
                    }
                }
                if (!string.IsNullOrEmpty(viagem.Origem) || !string.IsNullOrEmpty(viagem.Destino))
                {
                    infoViagem += $" • {viagem.Origem ?? ""} → {viagem.Destino ?? ""}";
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        ViagemId = viagem.ViagemId,
                        NoFichaVistoria = viagem.NoFichaVistoria ?? 0,
                        InfoViagem = infoViagem,

                        // Estatísticas
                        DuracaoMinutos = duracaoMinutos,
                        DuracaoFormatada = duracaoFormatada,
                        KmPercorrido = kmPercorrido,
                        LitrosGastos = Math.Round(litrosGastos, 2),
                        Consumo = Math.Round(consumo, 2),
                        ConsumoFormatado = consumoFormatado,
                        TipoCombustivel = tipoCombustivel,
                        PrecoCombustivel = Math.Round(precoCombustivel, 2),

                        // Custos
                        CustoMotorista = Math.Round(custoMotorista, 2),
                        CustoVeiculo = Math.Round(custoVeiculo, 2),
                        CustoCombustivel = Math.Round(custoCombustivel, 2),
                        CustoOperador = Math.Round(custoOperador, 2),
                        CustoLavador = Math.Round(custoLavador, 2),
                        CustoTotal = Math.Round(custoTotal, 2)
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "ObterCustosViagem", error);
                return Json(new
                {
                    success = false,
                    message = $"Erro ao obter custos da viagem: {error.Message}"
                });
            }
        }
    }
}
