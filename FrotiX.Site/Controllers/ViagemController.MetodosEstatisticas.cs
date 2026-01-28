/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.MetodosEstatisticas.cs                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
     * ⚡ CONTROLLER: Viagem API (Partial - MetodosEstatisticas)
     * 🎯 OBJETIVO: Geração assíncrona de estatísticas de viagens com rastreamento de progresso
     * 📋 ROTAS:
     *    - /api/Viagem/GerarEstatisticasViagens [POST]
     *    - /api/Viagem/ObterProgressoEstatisticas [GET]
     *    - /api/Viagem/LimparProgressoEstatisticas [POST]
     * 🔗 ENTIDADES: Viagem, ViagemEstatistica
     * 📦 DEPENDÊNCIAS: IUnitOfWork, IMemoryCache, IServiceScopeFactory, ViagemEstatisticaService
     * ⚡ PROCESSAMENTO: Task.Run (background) com progresso em cache (30 min)
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController
    {
        // [DOC] ========================================
        // [DOC] MÉTODOS PARA GERAÇÃO DE ESTATÍSTICAS DE VIAGENS
        // [DOC] ========================================

        /****************************************************************************************
         * 📦 DTO: ProgressoEstatisticas
         * 🎯 OBJETIVO: Rastrear progresso da geração assíncrona de estatísticas
         * 📋 PROPRIEDADES:
         *    - Total: Quantidade total de datas a processar
         *    - Processado: Quantidade já processada
         *    - Percentual: Progresso em % (0-100)
         *    - Concluido: Se processamento terminou (sucesso ou erro)
         *    - Erro: Se ocorreu erro durante processamento
         *    - Mensagem: Mensagem descritiva do status atual
         *    - IniciadoEm: Timestamp de início
         * 🗑️ CACHE: Armazenado em IMemoryCache por 30 minutos
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
         * 🎯 OBJETIVO: Iniciar geração assíncrona de estatísticas de viagens (background)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Interface de gerenciamento de estatísticas
         * 🔄 CHAMA: Task.Run() → ProcessarGeracaoEstatisticas()
         * ⚠️ VALIDAÇÃO: Impede múltiplos processamentos simultâneos (verifica cache)
         * 🗑️ CACHE: Verifica/registra "ProgressoEstatisticas" (30 min)
         ****************************************************************************************/
        [Route("GerarEstatisticasViagens")]
        [HttpPost]
        public IActionResult GerarEstatisticasViagens()
        {
            try
            {
                var cacheKey = "ProgressoEstatisticas";

                // [DOC] Verifica se já existe um processamento em andamento (evita duplicação)
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

                // [DOC] Inicia o processamento em background (Task.Run não bloqueia requisição)
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
         * ⚡ FUNÇÃO: ProcessarGeracaoEstatisticas (PRIVATE)
         * 🎯 OBJETIVO: Processar geração de estatísticas em background (chamado via Task.Run)
         * 📥 ENTRADAS: Nenhuma (acessa cache e banco)
         * 📤 SAÍDAS: Atualiza cache "ProgressoEstatisticas" a cada iteração
         * 🔄 CHAMA: ViagemEstatisticaService.RecalcularEstatisticasAsync() para cada data única
         * 📊 ALGORITMO:
         *    1. Busca todas as datas únicas de viagens
         *    2. Para cada data, recalcula estatísticas
         *    3. Atualiza progresso no cache a cada iteração
         *    4. Delay de 50ms a cada 10 iterações (não sobrecarregar)
         * 🔧 SCOPED: Cria novo scope/DbContext (background task precisa de nova instância)
         * ⚠️ ERRO: Loga erro individual mas continua processando outras datas
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
                // [DOC] Armazena progresso inicial no cache (30 minutos)
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

                // [DOC] CRÍTICO: Criar novo scope para ter novo DbContext (Task.Run roda em thread separada)
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // Resolve dependências do scope
                    var context = scope.ServiceProvider.GetRequiredService<FrotiXDbContext>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var viagemEstatisticaRepository = scope.ServiceProvider.GetRequiredService<IViagemEstatisticaRepository>();

                    // Cria novo service com as dependências do scope
                    var estatisticaService = new ViagemEstatisticaService(
                        context ,
                        viagemEstatisticaRepository ,
                        unitOfWork
                    );

                    // [DOC] Busca todas as datas únicas de viagens (base para estatísticas)
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

                    // [DOC] Loop principal: processa cada data única
                    foreach (var data in datasUnicas)
                    {
                        try
                        {
                            // [DOC] Gera/atualiza estatísticas para a data usando ViagemEstatisticaService
                            await estatisticaService.RecalcularEstatisticasAsync(data);

                            // [DOC] Atualiza progresso no cache (percentual calculado em tempo real)
                            contador++;
                            progresso.Processado = contador;
                            progresso.Percentual = progresso.Total > 0
                                ? (int)((contador * 100.0) / progresso.Total)
                                : 0;
                            progresso.Mensagem = $"Processando data {contador} de {progresso.Total}... ({data:dd/MM/yyyy})";

                            _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));

                            // [DOC] Delay a cada 10 iterações para não sobrecarregar banco/CPU
                            if (contador % 10 == 0)
                            {
                                await Task.Delay(50);
                            }
                        }
                        catch (Exception ex)
                        {
                            // [DOC] Loga erro individual mas continua processando (não para tudo por uma falha)
                            Console.WriteLine($"Erro ao processar estatísticas da data {data:dd/MM/yyyy}: {ex.Message}");
                        }
                    }

                    // Finaliza com sucesso
                    progresso.Concluido = true;
                    progresso.Percentual = 100;
                    progresso.Mensagem = $"Processamento concluído! Estatísticas de {contador} datas geradas.";
                    _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "ProcessarGeracaoEstatisticas" , error);

                progresso.Erro = true;
                progresso.Concluido = true;
                progresso.Mensagem = $"Erro durante o processamento: {error.Message}";
                _cache.Set(cacheKey , progresso , TimeSpan.FromMinutes(30));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterProgressoEstatisticas
         * 🎯 OBJETIVO: Consultar progresso atual da geração de estatísticas
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, progresso: ProgressoEstatisticas }
         * 🔗 CHAMADA POR: Polling do frontend (a cada X segundos)
         * 🔄 CHAMA: IMemoryCache.TryGetValue()
         * 🗑️ CACHE: Lê "ProgressoEstatisticas" (se não existir, retorna "Nenhum processamento")
         ****************************************************************************************/
        [Route("ObterProgressoEstatisticas")]
        [HttpGet]
        public IActionResult ObterProgressoEstatisticas()
        {
            try
            {
                var cacheKey = "ProgressoEstatisticas";

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

                // Não há processamento em andamento
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
         * 🎯 OBJETIVO: Limpar progresso do cache (resetar estado)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Interface de gerenciamento (reset manual)
         * 🔄 CHAMA: IMemoryCache.Remove()
         * 🗑️ CACHE: Remove "ProgressoEstatisticas"
         ****************************************************************************************/
        [Route("LimparProgressoEstatisticas")]
        [HttpPost]
        public IActionResult LimparProgressoEstatisticas()
        {
            try
            {
                var cacheKey = "ProgressoEstatisticas";
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
