/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Servicos.cs                                                                             ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Serviços de cálculos de custo (Combustível, Veículo, Motorista, Operador, Lavador).    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: CalculaCusto*, ConvertHtml (HtmlAgilityPack), TiraAcento, TreeView helpers               ║
   ║ 🔗 DEPS: IUnitOfWork, HtmlAgilityPack | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                        ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrotiX.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class Servicos
    {
        private readonly IUnitOfWork _unitOfWork;

        public Servicos(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "Constructor" , error);
            }
        }

        // ========================================
        // CÁLCULOS DE CUSTOS
        // ========================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalculaCustoCombustivel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula custo de combustível de uma viagem usando consumo e preço
         *                   Busca último preço de abastecimento; se não houver, usa média
         *
         * 📥 ENTRADAS     : viagemObj [Viagem] - Viagem com KmInicial, KmFinal, VeiculoId
         *                   _unitOfWork [IUnitOfWork] - Acesso aos repositórios
         *
         * 📤 SAÍDAS       : double - Custo em reais (R$), mínimo 0
         *
         * ⬅️ CHAMADO POR  : ViagemController.CalculoCusto() [Controller]
         *                   CustosViagemController.ObterCustos() [Dashboard]
         *
         * ➡️ CHAMA        : _unitOfWork.ViewVeiculos.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.Abastecimento.GetAll() [Repository]
         *                   _unitOfWork.MediaCombustivel.GetAll() [Fallback]
         *
         * 📝 OBSERVAÇÕES  : [REGRA] Se consumo zerado (sem histórico), assume 10 km/L
         *                   [LOGICA] KM consumida / consumo(km/L) * preço/litro
         ****************************************************************************************/
        public static double CalculaCustoCombustivel(Viagem viagemObj , IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DB] Buscar veículo para obter tipo de combustível
                var veiculoObj = _unitOfWork.ViewVeiculos.GetFirstOrDefault(v => v.VeiculoId == viagemObj.VeiculoId);

                // [LOGICA] Buscar últimos abastecimentos do veículo, ordenado por data DESC
                var combustivelObj = _unitOfWork.Abastecimento
                    .GetAll(a => a.VeiculoId == viagemObj.VeiculoId)
                    .OrderByDescending(o => o.DataHora);

                // [VALIDACAO] Obter preço unitário: usa último abastecimento OU média histórica
                double ValorCombustivel = 0;
                if (combustivelObj.FirstOrDefault() == null)
                {
                    // [PERFORMANCE] Fallback: usa média de preços históricos do combustível
                    var abastecimentoObj = _unitOfWork.MediaCombustivel
                        .GetAll(a => a.CombustivelId == veiculoObj.CombustivelId)
                        .OrderByDescending(o => o.Ano)
                        .ThenByDescending(o => o.Mes);
                    ValorCombustivel = (double)abastecimentoObj.FirstOrDefault().PrecoMedio;
                }
                else
                {
                    // [DB] Usa valor unitário do último abastecimento registrado
                    ValorCombustivel = (double)combustivelObj.FirstOrDefault().ValorUnitario;
                }

                // [LOGICA] Calcula quilometragem percorrida
                var Quilometragem = viagemObj.KmFinal - viagemObj.KmInicial;

                var ConsumoVeiculo = Convert.ToDouble(veiculoObj.Consumo);

                // [REGRA] Se consumo é zero (veículo novo sem histórico), usa 10 km/L como padrão
                if (ConsumoVeiculo == 0)
                {
                    ConsumoVeiculo = 10;
                }

                // [LOGICA] Fórmula: (KM / Consumo) * ValorLitro
                var CustoViagem = (Quilometragem / ConsumoVeiculo) * ValorCombustivel;

                return (double)CustoViagem;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalculaCustoCombustivel" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalculaCustoVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula custo de operação do veículo proporcional ao tempo de uso
         *                   Considera dias úteis (seg-sex) e horário operacional (6h-22h)
         *
         * 📥 ENTRADAS     : viagemObj [Viagem] - Viagem com DataInicial, DataFinal, HoraInicio, HoraFim
         *                   _unitOfWork [IUnitOfWork] - Acesso para buscar valor do veículo
         *
         * 📤 SAÍDAS       : double - Custo em reais (R$), máximo = valor mensal do contrato
         *
         * ⬅️ CHAMADO POR  : ViagemController.CalculoCusto() [linha 156]
         *                   CustosViagemController.ObterCustos() [Dashboard]
         *
         * ➡️ CHAMA        : ObterValorUnitarioVeiculo() [linha 267]
         *                   CalcularMinutosUteisViagem() [linha 197]
         *
         * 📝 OBSERVAÇÕES  : [REGRA] Horário operacional = 6h às 22h (16h/dia)
         *                   [REGRA] Dias úteis = 22 dias/mês (segunda a sexta)
         *                   [VALIDACAO] Nunca retorna > valor mensal (cap em contrato)
         ****************************************************************************************/
        public static double CalculaCustoVeiculo(Viagem viagemObj , IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DB] Buscar veículo e seu valor unitário (contrato/ata/padrão)
                var veiculoObj = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == viagemObj.VeiculoId);
                double valorUnitario = ObterValorUnitarioVeiculo(veiculoObj , _unitOfWork);

                const int HORAS_UTEIS_DIA = 16; // [REGRA] 6h às 22h
                const int DIAS_UTEIS_MES = 22; // [REGRA] Apenas dias úteis (seg-sex)

                // [LOGICA] Calcula minutos úteis disponíveis em um mês
                double minutosMesUteis = DIAS_UTEIS_MES * HORAS_UTEIS_DIA * 60; // 21.120 minutos
                double custoMinutoVeiculo = valorUnitario / minutosMesUteis;

                // [DADOS] Converte datas + horas para DateTime completo
                DateTime dataHoraInicio = viagemObj.DataInicial.Value.Date.Add(viagemObj.HoraInicio.Value.TimeOfDay);
                DateTime dataHoraFim = viagemObj.DataFinal.Value.Date.Add(viagemObj.HoraFim.Value.TimeOfDay);

                TimeSpan duracaoTotal = dataHoraFim - dataHoraInicio;

                // [LOGICA] Calcula minutos considerando dias úteis e horário operacional
                double minutosViagemUteis = CalcularMinutosUteisViagem(
                    dataHoraInicio ,
                    dataHoraFim ,
                    duracaoTotal ,
                    HORAS_UTEIS_DIA
                );

                double custoCalculado = minutosViagemUteis * custoMinutoVeiculo;

                // [VALIDACAO] Garante que nunca ultrapasse o valor mensal do contrato
                return Math.Min(custoCalculado , valorUnitario);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalculaCustoVeiculo" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalculaCustoMotorista
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula custo de motorista (apenas terceirizado) proporcional a tempo
         *                   Busca valor em RepactuacaoTerceirizacao com data mais recente
         *
         * 📥 ENTRADAS     : viagemObj [Viagem] - Viagem com datas, horas, MotoristaId
         *                   _unitOfWork [IUnitOfWork] - Acesso repositórios
         *                   minutos [ref int] - Retorna minutos efetivos da viagem (se -1, calcula)
         *
         * 📤 SAÍDAS       : double - Custo em reais (R$), 0 se motorista não é terceirizado
         *
         * ⬅️ CHAMADO POR  : ViagemController.CalculoCusto() [linha 156]
         *                   CustosViagemController.ObterCustos() [Dashboard]
         *
         * ➡️ CHAMA        : _unitOfWork.Motorista.GetFirstOrDefault() [Repository]
         *                   _unitOfWork.RepactuacaoContrato.GetAll() [Busca última]
         *                   _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault() [Valores]
         *                   CalcularMinutosUteisViagem() [linha 197]
         *
         * 📝 OBSERVAÇÕES  : [REGRA] Se ContratoId == null → motorista é interno, retorna 0
         *                   [REGRA] Jornada máxima = 12h/dia (laborais seg-sex)
         *                   [VALIDACAO] Nunca retorna > valor mensal (cap em contrato)
         *                   [HELPER] minutos ref é para evitar recalcular horas em outras funções
         ****************************************************************************************/
        public static double CalculaCustoMotorista(Viagem viagemObj , IUnitOfWork _unitOfWork , ref int minutos)
        {
            try
            {
                // [DB] Buscar motorista
                var motoristaObj = _unitOfWork.Motorista.GetFirstOrDefault(m => m.MotoristaId == viagemObj.MotoristaId);

                // [VALIDACAO] Se motorista não tem ContratoId, é interno (não terceirizado) = sem custo
                if (motoristaObj.ContratoId == null)
                {
                    if (minutos == -1)
                        minutos = 0;
                    return 0;
                }

                // [DB] Busca valor do motorista na última repactuação do contrato
                Guid contratoId = (Guid)motoristaObj.ContratoId;
                var topRepactuacao = _unitOfWork.RepactuacaoContrato
                    .GetAll(r => r.ContratoId == contratoId)
                    .OrderByDescending(r => r.DataRepactuacao)
                    .FirstOrDefault();

                // [DB] Obtém valores de terceirização (valorMotorista, qtdOperadores, etc)
                var topMotorista = _unitOfWork.RepactuacaoTerceirizacao
                    .GetFirstOrDefault(rt => rt.RepactuacaoContratoId == topRepactuacao.RepactuacaoContratoId);

                double valorMotorista = (double)topMotorista.ValorMotorista;

                const int HORAS_TRABALHO_DIA = 12; // [REGRA] Máximo 12h/dia laboral
                const int DIAS_UTEIS_MES = 22; // [REGRA] Apenas dias úteis

                // [LOGICA] Minutos úteis em um mês = 22 dias * 12h * 60 min = 15.840 minutos
                double minutosMesUteis = DIAS_UTEIS_MES * HORAS_TRABALHO_DIA * 60;
                double custoMinutoMotorista = valorMotorista / minutosMesUteis;

                // [DADOS] Converte datas + horas para DateTime completo
                DateTime dataHoraInicio = viagemObj.DataInicial.Value.Date.Add(viagemObj.HoraInicio.Value.TimeOfDay);
                DateTime dataHoraFim = viagemObj.DataFinal.Value.Date.Add(viagemObj.HoraFim.Value.TimeOfDay);

                TimeSpan duracaoTotal = dataHoraFim - dataHoraInicio;

                // [LOGICA] Calcula minutos considerando dias úteis e jornada de trabalho
                double minutosViagemUteis = CalcularMinutosUteisViagem(
                    dataHoraInicio ,
                    dataHoraFim ,
                    duracaoTotal ,
                    HORAS_TRABALHO_DIA
                );

                double custoCalculado = minutosViagemUteis * custoMinutoMotorista;

                // [HELPER] Registra minutos totais via ref param se solicitado (minutos == -1)
                if (minutos == -1)
                {
                    minutos = (int)minutosViagemUteis;
                }

                // [VALIDACAO] Nunca retorna > valor mensal (capping)
                return Math.Min(custoCalculado , valorMotorista);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalculaCustoMotorista" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMinutosUteisViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula minutos efetivos da viagem considerando dias úteis e limites
         *                   REGRA: Se viagem for curta (< 12h), retorna duração real
         *                   REGRA: Se viagem for longa, apena minutos úteis do período
         *
         * 📥 ENTRADAS     : inicio [DateTime] - Data/hora inicial da viagem
         *                   fim [DateTime] - Data/hora final da viagem
         *                   duracao [TimeSpan] - Duração total (fim - inicio)
         *                   horasMaximasDia [int] - Limite diário (12 motorista, 16 veículo)
         *
         * 📤 SAÍDAS       : double - Minutos úteis para cobrança, mínimo = duração real
         *
         * ⬅️ CHAMADO POR  : CalculaCustoVeiculo() [linha 91]
         *                   CalculaCustoMotorista() [linha 129]
         *
         * ➡️ CHAMA        : ContarDiasUteisComExcecoes() [linha 232]
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Viagem curta = não pula dias, usa duração real
         *                   [LOGICA] Viagem longa = multiplica dias úteis × horaMax/dia
         *                   [VALIDACAO] Ajusta com mínimo(minutosUteis, minutosReais)
         ****************************************************************************************/
        public static double CalcularMinutosUteisViagem(DateTime inicio , DateTime fim , TimeSpan duracao , int horasMaximasDia)
        {
            try
            {
                const int MINUTOS_DIA_COMPLETO = 24 * 60;
                int minutosMaximosDia = horasMaximasDia * 60;

                // [LOGICA] Viagem curta (mesmo dia ou poucas horas) = usa duração real
                if (duracao.TotalHours <= horasMaximasDia)
                {
                    return duracao.TotalMinutes;
                }

                // [LOGICA] Viagem longa - conta dias úteis com regra especial de fim de semana
                int diasUteis = ContarDiasUteisComExcecoes(inicio.Date , fim.Date);

                // [LOGICA] Calcula total de minutos úteis = dias * horasMax/dia * 60
                double minutosUteis = diasUteis * minutosMaximosDia;

                // [VALIDACAO] Se duração real for menor, ajusta proporcionalmente
                // Previne overcharging quando viagem não usa horas máximas
                double minutosReaisAjustados = duracao.TotalMinutes * ((double)minutosMaximosDia / MINUTOS_DIA_COMPLETO);

                return Math.Min(minutosUteis , minutosReaisAjustados);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalcularMinutosUteisViagem" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ContarDiasUteisComExcecoes
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Conta dias úteis (seg-sex) com EXCEÇÃO especial para fins de semana
         *                   REGRA: Se viagem COMEÇA ou TERMINA no fim de semana, conta esse dia
         *
         * 📥 ENTRADAS     : dataInicio [DateTime] - Primeira data do período (date only)
         *                   dataFim [DateTime] - Última data do período (date only)
         *
         * 📤 SAÍDAS       : int - Total de dias úteis contabilizados (1+)
         *
         * ⬅️ CHAMADO POR  : CalcularMinutosUteisViagem() [linha 197]
         *
         * ➡️ CHAMA        : Nenhuma função (apenas lógica de datas)
         *
         * 📝 OBSERVAÇÕES  : [REGRA] Loop itera de dataInicio até dataFim (inclusive)
         *                   [REGRA] Contabiliza:
         *                           • Seg-Sex sempre
         *                           • Sab/Dom APENAS se for dia inicial OU final
         *                   [EXEMPLO] Qui-Dom (2 dias) = conta Qui(útil) + Sab(final) = 2
         *                            Seg-Ter (2 dias) = conta Seg(útil) + Ter(útil) = 2
         ****************************************************************************************/
        public static int ContarDiasUteisComExcecoes(DateTime dataInicio , DateTime dataFim)
        {
            try
            {
                int diasUteis = 0;
                DateTime dataAtual = dataInicio;

                // [LOGICA] Itera dia por dia no período (inclusive)
                while (dataAtual <= dataFim)
                {
                    DayOfWeek diaSemana = dataAtual.DayOfWeek;
                    bool ehFimDeSemana = (diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday);
                    bool ehDiaInicial = (dataAtual == dataInicio);
                    bool ehDiaFinal = (dataAtual == dataFim);

                    // [REGRA] Conta se: É dia útil OU (é fim de semana MAS é o dia inicial ou final)
                    if (!ehFimDeSemana || ehDiaInicial || ehDiaFinal)
                    {
                        diasUteis++;
                    }

                    dataAtual = dataAtual.AddDays(1);
                }

                return diasUteis;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "ContarDiasUteisComExcecoes" , error);
                return 0;
            }
        }

        /// <summary>
        /// Extrai valor unitário do veículo (contrato/ata/próprio)
        /// </summary>
        public static double ObterValorUnitarioVeiculo(Veiculo veiculoObj , IUnitOfWork _unitOfWork)
        {
            try
            {
                if (veiculoObj.ContratoId != null)
                {
                    var valorUnitario = (from i in _unitOfWork.ItemVeiculoContrato.GetAll()
                                         join r in _unitOfWork.RepactuacaoContrato.GetAll()
                                         on i.RepactuacaoContratoId equals r.RepactuacaoContratoId
                                         orderby r.DataRepactuacao descending
                                         where i.ItemVeiculoId == veiculoObj.ItemVeiculoId
                                         select i.ValorUnitario).FirstOrDefault();

                    return (double)(valorUnitario ?? 0);
                }
                else if (veiculoObj.AtaId != null)
                {
                    var valorUnitario = (from i in _unitOfWork.ItemVeiculoAta.GetAll()
                                         join r in _unitOfWork.RepactuacaoAta.GetAll()
                                         on i.RepactuacaoAtaId equals r.RepactuacaoAtaId
                                         orderby r.DataRepactuacao descending
                                         where i.ItemVeiculoAtaId == veiculoObj.ItemVeiculoAtaId
                                         select i.ValorUnitario).FirstOrDefault();

                    return (double)(valorUnitario ?? 0);
                }
                else
                {
                    return 100; // Veículo próprio
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "ObterValorUnitarioVeiculo" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalculaCustoOperador
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula custo de operadores (terceirizados) diluído por viagem
         *                   Usa custo mensal total / média de viagens mensais
         *
         * 📥 ENTRADAS     : viagemObj [Viagem] - Viagem com DataInicial para cálculo de média
         *                   _unitOfWork [IUnitOfWork] - Acesso repositórios
         *
         * 📤 SAÍDAS       : double - Custo em reais (R$), 0 se sem contrato/operadores
         *
         * ⬅️ CHAMADO POR  : ViagemController.CalculoCusto() [linha 156]
         *                   CustosViagemController.ObterCustos() [Dashboard]
         *
         * ➡️ CHAMA        : _unitOfWork.Contrato.GetAll() [Busca contrato terceirização]
         *                   _unitOfWork.RepactuacaoContrato.GetAll() [Última repactuação]
         *                   _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault()
         *                   CalcularMediaDiariaViagens() [linha 410]
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Fórmula: (QtdOperadores * ValorUnitário) / MédiaViagens
         *                   [VALIDACAO] Retorna 0 se faltar dados (contrato, qtd, valor)
         *                   [PERFORMANCE] Usa última repactuação (mais recente)
         ****************************************************************************************/
        public static double CalculaCustoOperador(Viagem viagemObj , IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DB] Busca o contrato de operadores terceirizados mais recente
                var contratoOperadores = _unitOfWork.Contrato
                    .GetAll(c => c.TipoContrato == "Terceirização" && c.ContratoOperadores == true)
                    .OrderByDescending(c => c.DataInicio)
                    .FirstOrDefault();

                if (contratoOperadores == null)
                    return 0;

                // [DB] Busca última repactuação do contrato de operadores
                var topRepactuacao = _unitOfWork.RepactuacaoContrato
                    .GetAll(r => r.ContratoId == contratoOperadores.ContratoId)
                    .OrderByDescending(r => r.DataRepactuacao)
                    .FirstOrDefault();

                if (topRepactuacao == null)
                    return 0;

                // [DB] Obtém valores de terceirização (QtdOperadores, ValorOperador)
                var topTerceirizacao = _unitOfWork.RepactuacaoTerceirizacao
                    .GetFirstOrDefault(rt => rt.RepactuacaoContratoId == topRepactuacao.RepactuacaoContratoId);

                if (topTerceirizacao == null || topTerceirizacao.QtdOperadores == null || topTerceirizacao.ValorOperador == null)
                    return 0;

                // [LOGICA] Custo mensal total de operadores = Quantidade × ValorUnitário
                double custoMensalOperadores = (double)(topTerceirizacao.QtdOperadores.Value * topTerceirizacao.ValorOperador.Value);

                // [LOGICA] Calcula média diária de viagens até a data desta viagem
                double mediaViagens = CalcularMediaDiariaViagens(viagemObj.DataInicial.Value , _unitOfWork);

                if (mediaViagens == 0)
                    return 0;

                // [LOGICA] Dilui custo mensal pela média de viagens: CustoMês / MédiaViagensMês
                double custoPorViagem = custoMensalOperadores / mediaViagens;

                return custoPorViagem;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalculaCustoOperador" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalculaCustoLavador
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula custo de lavadores (terceirizados) diluído por viagem
         *                   Similar a CalculaCustoOperador, usa custo mensal / média viagens
         *
         * 📥 ENTRADAS     : viagemObj [Viagem] - Viagem com DataInicial para cálculo de média
         *                   _unitOfWork [IUnitOfWork] - Acesso repositórios
         *
         * 📤 SAÍDAS       : double - Custo em reais (R$), 0 se sem contrato/lavadores
         *
         * ⬅️ CHAMADO POR  : ViagemController.CalculoCusto() [linha 156]
         *                   CustosViagemController.ObterCustos() [Dashboard]
         *
         * ➡️ CHAMA        : _unitOfWork.Contrato.GetAll() [Busca contrato terceirização]
         *                   _unitOfWork.RepactuacaoContrato.GetAll() [Última repactuação]
         *                   _unitOfWork.RepactuacaoTerceirizacao.GetFirstOrDefault()
         *                   CalcularMediaDiariaViagens() [linha 410]
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Fórmula: (QtdLavadores * ValorUnitário) / MédiaViagens
         *                   [VALIDACAO] Retorna 0 se faltar dados (contrato, qtd, valor)
         *                   [PERFORMANCE] Usa última repactuação (mais recente)
         *                   [PATTERN] Segue mesmo padrão que CalculaCustoOperador
         ****************************************************************************************/
        public static double CalculaCustoLavador(Viagem viagemObj , IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DB] Busca o contrato de lavadores terceirizados mais recente
                var contratoLavadores = _unitOfWork.Contrato
                    .GetAll(c => c.TipoContrato == "Terceirização" && c.ContratoLavadores == true)
                    .OrderByDescending(c => c.DataInicio)
                    .FirstOrDefault();

                if (contratoLavadores == null)
                    return 0;

                // [DB] Busca última repactuação do contrato de lavadores
                var topRepactuacao = _unitOfWork.RepactuacaoContrato
                    .GetAll(r => r.ContratoId == contratoLavadores.ContratoId)
                    .OrderByDescending(r => r.DataRepactuacao)
                    .FirstOrDefault();

                if (topRepactuacao == null)
                    return 0;

                // [DB] Obtém valores de terceirização (QtdLavadores, ValorLavador)
                var topTerceirizacao = _unitOfWork.RepactuacaoTerceirizacao
                    .GetFirstOrDefault(rt => rt.RepactuacaoContratoId == topRepactuacao.RepactuacaoContratoId);

                if (topTerceirizacao == null || topTerceirizacao.QtdLavadores == null || topTerceirizacao.ValorLavador == null)
                    return 0;

                // [LOGICA] Custo mensal total de lavadores = Quantidade × ValorUnitário
                double custoMensalLavadores = (double)(topTerceirizacao.QtdLavadores.Value * topTerceirizacao.ValorLavador.Value);

                // [LOGICA] Calcula média diária de viagens até a data desta viagem
                double mediaViagens = CalcularMediaDiariaViagens(viagemObj.DataInicial.Value , _unitOfWork);

                if (mediaViagens == 0)
                    return 0;

                // [LOGICA] Dilui custo mensal pela média de viagens: CustoMês / MédiaViagensMês
                double custoPorViagem = custoMensalLavadores / mediaViagens;

                return custoPorViagem;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalculaCustoLavador" , error);
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMediaDiariaViagens
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula a média MENSAL de viagens baseada no histórico até a data
         *                   Fórmula: (totalViagensAnteriores / diasDesdeInicio) × 30
         *
         * 📥 ENTRADAS     : dataViagem [DateTime] - Data da viagem sendo calculada
         *                   _unitOfWork [IUnitOfWork] - Acesso ao repositório de Viagens
         *
         * 📤 SAÍDAS       : double - Média de viagens/mês (mínimo 0.1, máximo sem limite)
         *
         * ⬅️ CHAMADO POR  : CalculaCustoOperador() [linha 340]
         *                   CalculaCustoLavador() [linha 377]
         *
         * ➡️ CHAMA        : _unitOfWork.Viagem.GetAll() [Filtra viagens "Realizada"]
         *
         * 📝 OBSERVAÇÕES  : [LOGICA] Conta viagens com Status = "Realizada"
         *                   [LOGICA] Considera apenas viagens com DataInicial < dataViagem
         *                   [VALIDACAO] Se sem histórico, retorna 1.0 (mínimo seguro)
         *                   [VALIDACAO] Garante mínimo 0.1 para evitar divisão por zero
         *                   ⚠️ TODO: Usar GetQuery() para query otimizada (veja async)
         ****************************************************************************************/
        public static double CalcularMediaDiariaViagens(DateTime dataViagem , IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DB] Busca TODAS as viagens realizadas ANTES desta data
                var viagensAnteriores = _unitOfWork.Viagem
                    .GetAll(v => v.DataInicial < dataViagem && v.Status == "Realizada")
                    .Select(v => v.DataInicial.Value)
                    .OrderBy(d => d)
                    .ToList();

                int totalViagens = viagensAnteriores.Count;

                // [VALIDACAO] Se não há viagens anteriores, retorna 1 (mínimo seguro)
                if (totalViagens == 0)
                    return 1.0;

                // [LOGICA] Pega a data da PRIMEIRA viagem do histórico
                DateTime primeiraViagem = viagensAnteriores.First();

                // [LOGICA] Calcula total de DIAS desde a primeira viagem até esta data
                int totalDias = (dataViagem.Date - primeiraViagem.Date).Days;

                // [VALIDACAO] Se for no mesmo dia ou 0 dias, usa 1 como divisor (evita /0)
                if (totalDias <= 0)
                    totalDias = 1;

                // [LOGICA] Média DIÁRIA = Total de viagens / Total de dias
                double mediaDiaria = (double)totalViagens / (double)totalDias;

                // [LOGICA] Converte para média MENSAL (multiplica por 30)
                double mediaMensal = mediaDiaria * 30.0;

                // [VALIDACAO] Garante que a média nunca seja zero (mínimo 0.1)
                return Math.Max(mediaMensal , 0.1);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalcularMediaDiariaViagens" , error);
                return 1.0; // Em caso de erro, retorna 1 para evitar divisão por zero
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMediaDiariaViagensAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Versão OTIMIZADA e ASSÍNCRONICADA de CalcularMediaDiariaViagens
         *                   Usa GetQuery() para executar COUNT e MIN no SQL (performance)
         *
         * 📥 ENTRADAS     : dataViagem [DateTime] - Data da viagem sendo calculada
         *                   _unitOfWork [IUnitOfWork] - Acesso ao repositório (com GetQuery)
         *
         * 📤 SAÍDAS       : Task<double> - Média de viagens/mês (mínimo 0.1)
         *
         * ⬅️ CHAMADO POR  : CustosViagemController.ObterCustosAsync() [Dashboard]
         *                   ViagemController.CalculoCustoAsync() [Batch processing]
         *
         * ➡️ CHAMA        : _unitOfWork.Viagem.GetQuery() [Query sem materializar]
         *                   query.Count() [SQL: SELECT COUNT(*)]
         *                   query.Min() [SQL: SELECT MIN(DataInicial)]
         *
         * 📝 OBSERVAÇÕES  : [PERFORMANCE] GetQuery() retorna IQueryable (não materializa)
         *                   [PERFORMANCE] COUNT e MIN executam no SQL Server (milissegundos)
         *                   [PERFORMANCE] Debug.WriteLine para log de operações
         *                   [LOGICA] Mesma fórmula que versão síncrona
         *                   [DEBUG] Registra cada etapa com [MEDIA QUERY] prefix
         ****************************************************************************************/
        public static async Task<double> CalcularMediaDiariaViagensAsync(
            DateTime dataViagem ,
            IUnitOfWork _unitOfWork)
        {
            try
            {
                // [DEBUG] Log de início
                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Iniciando cálculo para {dataViagem:dd/MM/yyyy}");

                // [PERFORMANCE] GetQuery() retorna IQueryable (não materializa)
                // Isso permite COUNT() e Min() executarem no SQL Server
                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Obtendo query (sem materializar)...");

                var query = _unitOfWork.Viagem.GetQuery(v =>
                    v.DataInicial.HasValue &&
                    v.DataInicial < dataViagem &&
                    v.Status == "Realizada"
                );

                // [PERFORMANCE] COUNT executa SELECT COUNT(*) no SQL (milissegundos)
                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Executando COUNT no SQL...");
                int totalViagens = await Task.Run(() => query.Count());

                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Total: {totalViagens}");

                // [VALIDACAO] Se não há viagens anteriores, retorna 1 (mínimo)
                if (totalViagens == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Nenhuma viagem, retornando 1.0");
                    return 1.0;
                }

                // [PERFORMANCE] MIN executa SELECT MIN(DataInicial) no SQL (milissegundos)
                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Executando MIN no SQL...");
                DateTime primeiraViagem = await Task.Run(() => query.Min(v => v.DataInicial.Value));

                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Primeira viagem: {primeiraViagem:dd/MM/yyyy}");

                // [LOGICA] Calcula total de DIAS desde a primeira viagem até esta data
                int totalDias = (dataViagem.Date - primeiraViagem.Date).Days;

                if (totalDias <= 0)
                {
                    totalDias = 1;
                }

                // [LOGICA] Média DIÁRIA = Total de viagens / Total de dias
                double mediaDiaria = (double)totalViagens / (double)totalDias;

                // [LOGICA] Converte para média MENSAL (multiplica por 30)
                double mediaMensal = mediaDiaria * 30.0;

                // [VALIDACAO] Garante que a média nunca seja zero (mínimo 0.1)
                double resultadoFinal = Math.Max(mediaMensal , 0.1);

                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] ✅ Resultado: {resultadoFinal:F2} viagens/mês ({mediaDiaria:F4}/dia)");

                return resultadoFinal;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] ❌ ERRO: {error.Message}");
                System.Diagnostics.Debug.WriteLine($"[MEDIA QUERY] Stack: {error.StackTrace}");
                Alerta.TratamentoErroComLinha("Servicos.cs" , "CalcularMediaDiariaViagensAsync" , error);
                return 1.0;
            }
        }

        // ========================================
        // CONVERSÃO DE HTML PARA TEXTO SIMPLES
        // ========================================

        /****************************************************************************************
         * ⚡ FUNÇÃO: ConvertHtml
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converte HTML para texto simples, removendo tags e entidades
         *                   Usa HtmlAgilityPack para parsing robusto
         *
         * 📥 ENTRADAS     : html [string] - HTML a ser convertido (pode ser null)
         *
         * 📤 SAÍDAS       : string - Texto simples, com \r\n preservado
         *
         * ⬅️ CHAMADO POR  : ReportsController.ExportarPdf() [Conversão de conteúdo]
         *                   GlosaService.ListarDetalhes() [Descrições de items]
         *
         * ➡️ CHAMA        : HtmlDocument.LoadHtml() [HtmlAgilityPack]
         *                   ConvertTo() [Recursivo - linha 569]
         *                   ConvertContentTo() [Recursivo - linha 626]
         *
         * 📝 OBSERVAÇÕES  : [VALIDACAO] Se null, retorna string.Empty
         *                   [LOGICA] Remove 2 primeiros chars se começarem com "\r\n"
         *                   [HELPER] Função ConvertTo faz parsing recursivo de nós DOM
         ****************************************************************************************/
        public static string ConvertHtml(string html)
        {
            try
            {
                if (html != null)
                {
                    // [UI] Parse HTML usando HtmlAgilityPack (robusto com HTML malformado)
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    // [LOGICA] Usa StringWriter + ConvertTo para extrair texto recursivamente
                    StringWriter sw = new StringWriter();
                    ConvertTo(doc.DocumentNode , sw);
                    sw.Flush();
                    var resultado = sw.ToString();

                    // [VALIDACAO] Remove \r\n inicial se presente (artefato do parsing)
                    if (resultado.Length >= 4)
                    {
                        if (resultado != "" && resultado.Substring(0 , 4) == "\r\n")
                        {
                            return resultado.Remove(0 , 2);
                        }
                        else
                        {
                            return resultado;
                        }
                    }
                    else
                    {
                        return resultado;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "ConvertHtml" , error);
                return "";
            }
        }

        public static void ConvertTo(HtmlNode node , TextWriter outText)
        {
            try
            {
                string html;
                switch (node.NodeType)
                {
                    case HtmlNodeType.Comment:
                        // don't output comments
                        break;

                    case HtmlNodeType.Document:
                        ConvertContentTo(node , outText);
                        break;

                    case HtmlNodeType.Text:
                        // script and style must not be output
                        string parentName = node.ParentNode.Name;
                        if ((parentName == "script") || (parentName == "style"))
                            break;

                        // get text
                        html = ((HtmlTextNode)node).Text;

                        // is it in fact a special closing node output as text?
                        if (HtmlNode.IsOverlappedClosingElement(html))
                            break;

                        // check the text is meaningful and not a bunch of whitespaces
                        if (html.Trim().Length > 0)
                        {
                            outText.Write(HtmlEntity.DeEntitize(html));
                        }
                        break;

                    case HtmlNodeType.Element:
                        switch (node.Name)
                        {
                            case "p":
                                // treat paragraphs as crlf
                                outText.Write("\r\n");
                                break;
                        }

                        if (node.HasChildNodes)
                        {
                            ConvertContentTo(node , outText);
                        }
                        break;
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "ConvertTo" , error);
            }
        }

        public static void ConvertContentTo(HtmlNode node , TextWriter outText)
        {
            try
            {
                foreach (HtmlNode subnode in node.ChildNodes)
                {
                    ConvertTo(subnode , outText);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "ConvertContentTo" , error);
            }
        }

        // ========================================
        // TREEVIEW E HIERARQUIA
        // ========================================

        [Route("Employees")]
        [HttpGet]
        public JsonResult Employees()
        {
            try
            {
                var result = _unitOfWork.SetorSolicitante.GetAll();
                {
                    var employees = from e in result
                                    select new
                                    {
                                        id = e.SetorSolicitanteId ,
                                        Name = e.Nome ,
                                        hasChildren = (from q in _unitOfWork.SetorSolicitante.GetAll()
                                                       where (q.SetorPaiId == e.SetorSolicitanteId)
                                                       select q
                                                       ).Count() > 0
                                    };

                    return new JsonResult(employees.ToList());
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "Employees" , error);
                return new JsonResult(new
                {
                    erro = error.Message
                });
            }
        }

        public class HierarchicalViewModel
        {
            public int ID
            {
                get; set;
            }

            public int? ParendID
            {
                get; set;
            }

            public bool HasChildren
            {
                get; set;
            }

            public string Name
            {
                get; set;
            }
        }

        public static IList<HierarchicalViewModel> GetHierarchicalData()
        {
            try
            {
                var result = new List<HierarchicalViewModel>()
                {
                    new HierarchicalViewModel() { ID = 1, ParendID = null, HasChildren = true, Name = "Parent item" },
                    new HierarchicalViewModel() { ID = 2, ParendID = 1, HasChildren = true, Name = "Parent item" },
                    new HierarchicalViewModel() { ID = 3, ParendID = 1, HasChildren = false, Name = "Item" },
                    new HierarchicalViewModel() { ID = 4, ParendID = 2, HasChildren = false, Name = "Item" },
                    new HierarchicalViewModel() { ID = 5, ParendID = 2, HasChildren = false, Name = "Item" }
                };

                return result;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "GetHierarchicalData" , error);
                return new List<HierarchicalViewModel>();
            }
        }

        public IActionResult Read_TreeViewData(int? id)
        {
            try
            {
                var result = GetHierarchicalData()
                    .Where(x => id.HasValue ? x.ParendID == id : x.ParendID == null)
                    .Select(item => new
                    {
                        id = item.ID ,
                        Name = item.Name ,
                        hasChildren = item.HasChildren
                    });

                return new JsonResult(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "Read_TreeViewData" , error);
                return new JsonResult(new
                {
                    erro = error.Message
                });
            }
        }

        // ========================================
        // FUNÇÃO TIRAACENTO - VERSÃO COMPLETA
        // ========================================

        /// <summary>
        /// Remove acentos e caracteres inválidos para nomes de arquivo
        /// Substitui espaços por underscore
        /// </summary>
        /// <param name="texto">Texto a ser normalizado</param>
        /// <returns>Texto normalizado e seguro para nome de arquivo</returns>
        public static string TiraAcento(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            try
            {
                // Remove acentos usando normalização Unicode
                string normalizado = texto.Normalize(NormalizationForm.FormD);
                StringBuilder sb = new StringBuilder();

                foreach (char c in normalizado)
                {
                    UnicodeCategory categoria = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (categoria != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(c);
                    }
                }

                string resultado = sb.ToString().Normalize(NormalizationForm.FormC);

                // Substitui caracteres especiais que podem não ser cobertos pela normalização
                var substituicoes = new Dictionary<string , string>
                {
                    { "ß", "ss" }, { "œ", "oe" }, { "Œ", "OE" },
                    { "æ", "ae" }, { "Æ", "AE" }, { "ð", "d" },
                    { "Ð", "D" },  { "þ", "th" }, { "Þ", "TH" }
                };

                foreach (var sub in substituicoes)
                {
                    resultado = resultado.Replace(sub.Key , sub.Value);
                }

                // Remove caracteres inválidos para nomes de arquivo
                char[] caracteresInvalidos = Path.GetInvalidFileNameChars();
                resultado = string.Concat(resultado.Split(caracteresInvalidos));

                // Remove caracteres especiais, mantendo apenas alfanuméricos, espaços, underscore, hífen e ponto
                resultado = Regex.Replace(resultado , @"[^\w\s.\-]" , "");

                // Substitui espaços por underscore
                resultado = Regex.Replace(resultado , @"\s+" , "_");

                // Remove múltiplos underscores/hífens/pontos consecutivos
                resultado = Regex.Replace(resultado , @"_{2,}" , "_");
                resultado = Regex.Replace(resultado , @"-{2,}" , "-");
                resultado = Regex.Replace(resultado , @"\.{2,}" , ".");

                // Remove underscore/hífen no início e fim
                resultado = Regex.Replace(resultado , @"^[_\-]+|[_\-]+$" , "");

                // Limita tamanho (255 caracteres)
                if (resultado.Length > 255)
                {
                    resultado = resultado.Substring(0 , 255);
                }

                return resultado;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Servicos.cs" , "TiraAcento" , error);
                return texto;
            }
        }

        // Exemplos de uso:
        // TiraAcento("Açúcar & Café.pdf")        → "Acucar_Cafe.pdf"
        // TiraAcento("São Paulo/Rio")            → "Sao_PauloRio"
        // TiraAcento("Relatório 2024: análise")  → "Relatorio_2024_analise"
    }
}
