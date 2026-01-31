/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemController.MetodosEstatisticas.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerar estatísticas de viagens em background com controle de progresso.
 *
 * 📥 ENTRADAS     : Requisições de iniciar/consultar/limpar processamento.
 *
 * 📤 SAÍDAS       : JSON com progresso e status.
 *
 * 🔗 CHAMADA POR  : Dashboard de estatísticas.
 *
 * 🔄 CHAMA        : IMemoryCache, IServiceScopeFactory, ViagemEstatisticaService.
 **************************************************************************************** */

using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: ViagemController.MetodosEstatisticas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar geração de estatísticas e controle de progresso.
     *
     * 📥 ENTRADAS     : Chamadas do frontend.
     *
     * 📤 SAÍDAS       : JSON de progresso e mensagens.
     ****************************************************************************************/
    public partial class ViagemController
    {
        // ========================================
        // [DOC] MÉTODOS PARA GERAÇÃO DE ESTATÍSTICAS DE VIAGENS
        // Processamento em background com controle de progresso via cache
        // ========================================

        /****************************************************************************************
         * ⚡ CLASSE: ProgressoEstatisticas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : DTO para controlar estado do processamento de estatísticas
         *                   Armazenado em IMemoryCache para consulta do frontend
         * 📦 PROPRIEDADES : Total, Processado, Percentual, Concluido, Erro, Mensagem, IniciadoEm
         ****************************************************************************************/
        public class ProgressoEstatisticas
        {
            public int Total { get; set; }
            public int Processado { get; set; }
            public int Percentual { get; set; }
            public bool Concluido { get; set; }
            public bool Erro { get; set; }
            public string Mensagem { get; set; }
            public DateTime IniciadoEm { get; set; }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GerarEstatisticasViagens
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Iniciar geração de estatísticas em background (fire-and-forget)
         *                   Valida se já existe processamento em andamento antes de iniciar
         * 📥 ENTRADAS     : Nenhuma (usa dados do banco)
         * 📤 SAÍDAS       : [IActionResult] JSON com success e message
         * 🔗 CHAMADA POR  : Botão "Gerar Estatísticas" no dashboard
         * 🔄 CHAMA        : Task.Run(ProcessarGeracaoEstatisticas)
         *
         * ⚠️  VALIDAÇÕES:
         *    - Bloqueia se já existe processamento em andamento (não concluído/erro)
         *    - Usa cache para evitar processamentos duplicados
         ****************************************************************************************/
        [Route("GerarEstatisticasViagens")]
        [HttpPost]
        public IActionResult GerarEstatisticasViagens()
        {
            try
            {
                var cacheKey = "ProgressoEstatisticas";

                // [DOC] Verifica se já existe um processamento em andamento
                if (_cache.TryGetValue(cacheKey , out ProgressoEstatisticas progressoExistente))
                {
                    if (!progressoExistente.Concluido && !progressoExistente.Erro)
                    {
                        return Json(new
                        {
                            success = false ,
                            message = "Já existe um processamento em andamento. Aguarde a conclusão."
                        });
                    }
                }

                // [DOC] Inicia o processamento em background (fire-and-forget)
                // Task.Run garante que não bloqueia a resposta HTTP
                Task.Run(async () => await ProcessarGeracaoEstatisticas());

                return Json(new
                {
                    success = true ,
                    message = "Processamento iniciado com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "GerarEstatisticasViagens" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao iniciar geração de estatísticas"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ProcessarGeracaoEstatisticas (Private Async)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar estatísticas de todas as datas de viagens em background
         *                   Cria novo scope DI para ter DbContext independente
         * 📥 ENTRADAS     : Nenhuma (usa dados do banco)
         * 📤 SAÍDAS       : Atualiza cache com progresso (sem retorno direto)
         * 🔗 CHAMADA POR  : GerarEstatisticasViagens via Task.Run
         * 🔄 CHAMA        : ViagemEstatisticaService.RecalcularEstatisticasAsync
         *
         * ⚡ PERFORMANCE:
         *    - Processa data por data para evitar timeout
         *    - Delay de 50ms a cada 10 iterações para não sobrecarregar
         *    - Continua processando mesmo se uma data falhar
         *
         * 🔐 IMPORTANTE:
         *    - Usa IServiceScopeFactory para criar novo scope (DbContext não é thread-safe)
         *    - Cache expira em 30 minutos
         ****************************************************************************************/
        private async Task ProcessarGeracaoEstatisticas()
        {
            var cacheKey = "ProgressoEstatisticas";
            var progresso = new ProgressoEstatisticas
            {
                Total = 0 ,
                Processado = 0 ,
                Percentual = 0 ,
                Concluido = false ,
                Erro = false ,
                Mensagem = "Inicializando..." ,
                IniciadoEm = DateTime.Now
            };

            try
            {
                // [DOC] STEP 1: Armazena progresso inicial no cache (30 minutos)
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

                // [DOC] STEP 2: CRÍTICO - Criar um novo scope para ter um novo DbContext
                // DbContext não é thread-safe, então precisamos de uma instância separada
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // [DOC] Resolve dependências do scope
                    var context = scope.ServiceProvider.GetRequiredService<FrotiXDbContext>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var viagemEstatisticaRepository = scope.ServiceProvider.GetRequiredService<IViagemEstatisticaRepository>();

                    // [DOC] Cria novo service com as dependências do scope
                    var estatisticaService = new ViagemEstatisticaService(
                        context ,
                        viagemEstatisticaRepository ,
                        unitOfWork
                    );

                    // [DOC] STEP 3: Busca todas as datas únicas de viagens
                    var datasUnicas = await context.Viagem
                        .Where(v => v.DataInicial.HasValue)
                        .Select(v => v.DataInicial.Value.Date)
                        .Distinct()
                        .OrderBy(d => d)
                        .ToListAsync();

                    progresso.Total = datasUnicas.Count;
                    progresso.Mensagem = $"Processando estatísticas de {progresso.Total} datas...";
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

                    int contador = 0;

                    // [DOC] STEP 4: Processa cada data individualmente
                    foreach (var data in datasUnicas)
                    {
                        try
                        {
                            // [DOC] Gera/atualiza estatísticas para a data
                            await estatisticaService.RecalcularEstatisticasAsync(data);

                            // [DOC] Atualiza progresso no cache
                            contador++;
                            progresso.Processado = contador;
                            progresso.Percentual = progresso.Total > 0
                                ? (int)((contador * 100.0) / progresso.Total)
                                : 0;
                            progresso.Mensagem = $"Processando data {contador} de {progresso.Total}... ({data:dd/MM/yyyy})";

                            _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

                            // [DOC] Pequeno delay a cada 10 iterações para não sobrecarregar
                            if (contador % 10 == 0)
                            {
                                await Task.Delay(50);
                            }
                        }
                        catch (Exception ex)
                        {
                            // [DOC] Log do erro mas continua processando as outras datas
                            Console.WriteLine($"Erro ao processar estatísticas da data {data:dd/MM/yyyy}: {ex.Message}");
                        }
                    }

                    // [DOC] STEP 5: Finaliza com sucesso
                    progresso.Concluido = true;
                    progresso.Percentual = 100;
                    progresso.Mensagem = $"Processamento concluído! Estatísticas de {contador} datas geradas.";
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                }
            }
            catch (Exception error)
            {
                // [DOC] Tratamento de erro: marca como concluído com erro
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ProcessarGeracaoEstatisticas" , error);

                progresso.Erro = true;
                progresso.Concluido = true;
                progresso.Mensagem = $"Erro durante o processamento: {error.Message}";
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterProgressoEstatisticas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar estado atual do processamento de estatísticas
         *                   Usado para atualizar barra de progresso no frontend
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON com progresso (total, processado, %, etc)
         * 🔗 CHAMADA POR  : JavaScript (polling a cada X segundos)
         * 🔄 CHAMA        : IMemoryCache.TryGetValue
         ****************************************************************************************/
        [Route("ObterProgressoEstatisticas")]
        [HttpGet]
        public IActionResult ObterProgressoEstatisticas()
        {
            try
            {
                var cacheKey = "ProgressoEstatisticas";

                // [DOC] Busca progresso no cache
                if (_cache.TryGetValue(cacheKey , out ProgressoEstatisticas progresso))
                {
                    return Json(new
                    {
                        success = true ,
                        progresso = new
                        {
                            total = progresso.Total ,
                            processado = progresso.Processado ,
                            percentual = progresso.Percentual ,
                            concluido = progresso.Concluido ,
                            erro = progresso.Erro ,
                            mensagem = progresso.Mensagem
                        }
                    });
                }

                // [DOC] Não há processamento em andamento - retorna valores zerados
                return Json(new
                {
                    success = true ,
                    progresso = new
                    {
                        total = 0 ,
                        processado = 0 ,
                        percentual = 0 ,
                        concluido = false ,
                        erro = false ,
                        mensagem = "Nenhum processamento em andamento"
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ObterProgressoEstatisticas" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao obter progresso"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: LimparProgressoEstatisticas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Limpar cache de progresso (permite reiniciar processamento)
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON com success e message
         * 🔗 CHAMADA POR  : Botão "Limpar" ou após erro no frontend
         * 🔄 CHAMA        : IMemoryCache.Remove
         ****************************************************************************************/
        [Route("LimparProgressoEstatisticas")]
        [HttpPost]
        public IActionResult LimparProgressoEstatisticas()
        {
            try
            {
                var cacheKey = "ProgressoEstatisticas";
                
                // [DOC] Remove entrada do cache para permitir novo processamento
                _cache.Remove(cacheKey);

                return Json(new
                {
                    success = true ,
                    message = "Progresso limpo com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "LimparProgressoEstatisticas" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao limpar progresso"
                });
            }
        }
    }
}
