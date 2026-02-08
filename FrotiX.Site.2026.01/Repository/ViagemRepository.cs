/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagemRepository.cs                                                                    ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para entidade Viagem com operações CRUD e consultas especializadas.                ║
   ║    Inclui paginação otimizada via ViewViagens e utilitários de correção em lote.                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViagemRepository(FrotiXDbContext db)                                                          ║
   ║    • GetViagemListForDropDown()                                                                    ║
   ║    • Update(Viagem viagem)                                                                         ║
   ║    • GetDistinctOrigensAsync()                                                                     ║
   ║    • GetDistinctDestinosAsync()                                                                    ║
   ║    • CorrigirOrigemAsync(List<string> origensAntigas, string novaOrigem)                           ║
   ║    • CorrigirDestinoAsync(List<string> destinosAntigos, string novoDestino)                        ║
   ║    • BuscarViagensRecorrenciaAsync(Guid id)                                                        ║
   ║    • GetViagensEventoPaginadoAsync(Guid eventoId, int page, int pageSize)                          ║
   ║    • GetQuery(Expression<Func<Viagem, bool>> filter = null)                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A paginação usa ViewViagens para reduzir JOINs complexos e aplica AsNoTracking.                 ║
   ║    Há logs de performance com Stopwatch e tratamento de erro centralizado.                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViagemRepository                                                                   │
    // │ 📦 HERDA DE: Repository                                                               │
    // │ 🔌 IMPLEMENTA: IViagemRepository                                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável por operações e consultas de viagens.
    // Disponibiliza listagens, correções em lote e paginação otimizada.
    
    public class ViagemRepository : Repository<Viagem>, IViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViagemRepository                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViagemRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViagemListForDropDown                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.Viagem, OrderBy, Select                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de viagens para composição de dropdowns.
        // Ordena por data inicial.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para viagens.
        public IEnumerable<SelectListItem> GetViagemListForDropDown()
        {
            return _db.Viagem
                .OrderBy(o => o.DataInicial)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao ,
                    Value = i.ViagemId.ToString()
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma viagem no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // viagem - Entidade contendo os dados atualizados.
        
        
        // Param viagem: Entidade <see cref="Viagem"/> com dados atualizados.
        public new void Update(Viagem viagem)
        {
            _db.Update(viagem);
            _db.SaveChanges();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetDistinctOrigensAsync                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Viagem, Where, Select, Distinct, OrderBy, ToListAsync      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de origens distintas das viagens cadastradas.
        
        
        
        // 📤 RETORNO:
        // Task&lt;List&lt;string&gt;&gt; - Lista de origens únicas.
        
        
        // Returns: Lista de origens distintas.
        public async Task<List<string>> GetDistinctOrigensAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Origem))
                .Select(v => v.Origem)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetDistinctDestinosAsync                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Viagem, Where, Select, Distinct, OrderBy, ToListAsync      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de destinos distintos das viagens cadastradas.
        
        
        
        // 📤 RETORNO:
        // Task&lt;List&lt;string&gt;&gt; - Lista de destinos únicos.
        
        
        // Returns: Lista de destinos distintos.
        public async Task<List<string>> GetDistinctDestinosAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Destino))
                .Select(v => v.Destino)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: CorrigirOrigemAsync                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Viagem, Where, ToListAsync, SaveChangesAsync               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Corrigir origens em lote, substituindo por um novo valor.
        
        
        
        // 📥 PARÂMETROS:
        // origensAntigas - Lista de origens a serem substituídas
        // novaOrigem - Novo valor de origem
        
        
        // Param origensAntigas: Lista de origens a corrigir.
        // Param novaOrigem: Novo valor de origem.
        public async Task CorrigirOrigemAsync(List<string> origensAntigas , string novaOrigem)
        {
            var viagens = await _db.Viagem
                .Where(v => origensAntigas.Contains(v.Origem))
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.Origem = novaOrigem;
            }

            await _db.SaveChangesAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: CorrigirDestinoAsync                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Viagem, Where, ToListAsync, SaveChangesAsync               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Corrigir destinos em lote, substituindo por um novo valor.
        
        
        
        // 📥 PARÂMETROS:
        // destinosAntigos - Lista de destinos a serem substituídos
        // novoDestino - Novo valor de destino
        
        
        // Param destinosAntigos: Lista de destinos a corrigir.
        // Param novoDestino: Novo valor de destino.
        public async Task CorrigirDestinoAsync(List<string> destinosAntigos , string novoDestino)
        {
            var viagens = await _db.Viagem
                .Where(v => destinosAntigos.Contains(v.Destino))
                .ToListAsync();

            foreach (var viagem in viagens)
            {
                viagem.Destino = novoDestino;
            }

            await _db.SaveChangesAsync();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: BuscarViagensRecorrenciaAsync                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Viagem.FindAsync, Where, OrderBy, ToListAsync               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Buscar viagens de recorrência com base no EventoId.
        // Retorna a viagem original quando não há recorrência.
        
        
        
        // 📥 PARÂMETROS:
        // id - Identificador da viagem base.
        
        
        
        // 📤 RETORNO:
        // Task&lt;List&lt;Viagem&gt;&gt; - Lista de viagens relacionadas.
        
        
        // Param id: Identificador da viagem base.
        // Returns: Lista de viagens relacionadas.
        public async Task<List<Viagem>> BuscarViagensRecorrenciaAsync(Guid id)
        {
            var viagemOriginal = await _db.Viagem.FindAsync(id);
            if (viagemOriginal == null)
                return new List<Viagem>();

            if (viagemOriginal.EventoId.HasValue)
            {
                return await _db.Viagem
                    .Where(v => v.EventoId == viagemOriginal.EventoId.Value)
                    .OrderBy(v => v.DataInicial)
                    .ToListAsync();
            }

            return new List<Viagem> { viagemOriginal };
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViagensEventoPaginadoAsync                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Viagem, DbContext.ViewViagens, AsNoTracking, Stopwatch      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter viagens de um evento com paginação otimizada e dados completos da ViewViagens.
        // Separa o COUNT da consulta principal para reduzir custo de JOINs.
        
        
        
        // 📥 PARÂMETROS:
        // eventoId - Identificador do evento
        // page - Página atual (1-based)
        // pageSize - Quantidade de registros por página
        
        
        
        // 📤 RETORNO:
        // Task&lt;(List&lt;ViagemEventoDto&gt; viagens, int totalItems)&gt; - Lista paginada e total.
        
        
        // Param eventoId: Identificador do evento.
        // Param page: Página atual (1-based).
        // Param pageSize: Tamanho da página.
        // Returns: Lista de viagens e total de itens.
        public async Task<(List<ViagemEventoDto> viagens, int totalItems)> GetViagensEventoPaginadoAsync(
            Guid eventoId ,
            int page ,
            int pageSize
        )
        {
            try
            {
                var swTotal = System.Diagnostics.Stopwatch.StartNew();
                var swCount = System.Diagnostics.Stopwatch.StartNew();

                // COUNT otimizado na tabela Viagem
                var totalItems = await _db.Viagem
                    .Where(v => v.EventoId == eventoId && v.Status == "Realizada")
                    .CountAsync();

                swCount.Stop();
                Console.WriteLine($"[SQL COUNT] {totalItems} registros - {swCount.ElapsedMilliseconds}ms");

                if (totalItems == 0)
                {
                    return (new List<ViagemEventoDto>(), 0);
                }

                var swQuery = System.Diagnostics.Stopwatch.StartNew();

                // Buscar IDs das viagens paginadas
                var viagemIds = await _db.Viagem
                    .Where(v => v.EventoId == eventoId && v.Status == "Realizada")
                    .OrderByDescending(v => v.DataInicial)
                    .ThenByDescending(v => v.HoraInicio)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(v => v.ViagemId)
                    .ToListAsync();

                // Buscar dados completos da ViewViagens apenas para os IDs paginados
                var viagens = await _db.ViewViagens
                    .Where(vv => viagemIds.Contains(vv.ViagemId))
                    .Select(vv => new ViagemEventoDto
                    {
                        ViagemId = vv.ViagemId , // ✅ ADICIONADO!
                        EventoId = vv.EventoId ?? Guid.Empty ,
                        NoFichaVistoria = vv.NoFichaVistoria ?? 0 ,
                        NomeRequisitante = vv.NomeRequisitante ?? "" ,
                        NomeSetor = vv.NomeSetor ?? "" ,
                        NomeMotorista = vv.NomeMotorista ?? "" ,
                        DescricaoVeiculo = vv.DescricaoVeiculo ?? "" ,
                        CustoViagem = (decimal)(vv.CustoViagem ?? 0) ,
                        DataInicial = vv.DataInicial ?? DateTime.MinValue ,
                        HoraInicio = vv.HoraInicio ,
                        Placa = vv.Placa ?? ""
                    })
                    .AsNoTracking()
                    .ToListAsync();

                // Reordenar no lado do cliente (já são poucos registros)
                viagens = viagens
                    .OrderByDescending(v => v.DataInicial)
                    .ThenByDescending(v => v.HoraInicio)
                    .ToList();

                swQuery.Stop();
                Console.WriteLine($"[SQL QUERY] {viagens.Count} registros - {swQuery.ElapsedMilliseconds}ms");

                swTotal.Stop();
                Console.WriteLine($"[TOTAL] {swTotal.ElapsedMilliseconds}ms\n");

                return (viagens, totalItems);
            }
            catch (Exception error)
            {
                Console.WriteLine($"[ERRO SQL] {error.Message}");
                Alerta.TratamentoErroComLinha("ViagemRepository.cs" , "GetViagensEventoPaginadoAsync" , error);
                throw;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetQuery                                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbSet.Where                                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar IQueryable para composição de queries sem materialização.
        // Útil para Count(), Min(), Max(), etc.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional para composição da consulta.
        
        
        
        // 📤 RETORNO:
        // IQueryable&lt;Viagem&gt; - Consulta base para composição posterior.
        
        
        // Param filter: Filtro opcional.
        // Returns: Consulta base para composição.
        public IQueryable<Viagem> GetQuery(Expression<Func<Viagem , bool>> filter = null)
        {
            IQueryable<Viagem> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }
}
