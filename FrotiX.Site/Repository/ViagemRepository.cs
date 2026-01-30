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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViagemRepository                                                                   │
    /// │ 📦 HERDA DE: Repository<Viagem>                                                               │
    /// │ 🔌 IMPLEMENTA: IViagemRepository                                                              │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável por operações e consultas de viagens.
    /// Disponibiliza listagens, correções em lote e paginação otimizada.
    /// </summary>
    public class ViagemRepository : Repository<Viagem>, IViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViagemRepository                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : base(db)                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="FrotiXDbContext"/>.</param>
        public ViagemRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViagemListForDropDown                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.Viagem, OrderBy, Select                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de viagens para composição de dropdowns.
        ///    Ordena por data inicial.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para viagens.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar os dados de uma viagem no banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    viagem - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="viagem">Entidade <see cref="Viagem"/> com dados atualizados.</param>
        public new void Update(Viagem viagem)
        {
            _db.Update(viagem);
            _db.SaveChanges();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetDistinctOrigensAsync                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Viagem, Where, Select, Distinct, OrderBy, ToListAsync      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de origens distintas das viagens cadastradas.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;List&lt;string&gt;&gt; - Lista de origens únicas.
        /// </para>
        /// </summary>
        /// <returns>Lista de origens distintas.</returns>
        public async Task<List<string>> GetDistinctOrigensAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Origem))
                .Select(v => v.Origem)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetDistinctDestinosAsync                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Viagem, Where, Select, Distinct, OrderBy, ToListAsync      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de destinos distintos das viagens cadastradas.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;List&lt;string&gt;&gt; - Lista de destinos únicos.
        /// </para>
        /// </summary>
        /// <returns>Lista de destinos distintos.</returns>
        public async Task<List<string>> GetDistinctDestinosAsync()
        {
            return await _db.Viagem
                .Where(v => !string.IsNullOrEmpty(v.Destino))
                .Select(v => v.Destino)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: CorrigirOrigemAsync                                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Viagem, Where, ToListAsync, SaveChangesAsync               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Corrigir origens em lote, substituindo por um novo valor.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    origensAntigas - Lista de origens a serem substituídas<br/>
        ///    novaOrigem - Novo valor de origem
        /// </para>
        /// </summary>
        /// <param name="origensAntigas">Lista de origens a corrigir.</param>
        /// <param name="novaOrigem">Novo valor de origem.</param>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: CorrigirDestinoAsync                                                         │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Viagem, Where, ToListAsync, SaveChangesAsync               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Corrigir destinos em lote, substituindo por um novo valor.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    destinosAntigos - Lista de destinos a serem substituídos<br/>
        ///    novoDestino - Novo valor de destino
        /// </para>
        /// </summary>
        /// <param name="destinosAntigos">Lista de destinos a corrigir.</param>
        /// <param name="novoDestino">Novo valor de destino.</param>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: BuscarViagensRecorrenciaAsync                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Viagem.FindAsync, Where, OrderBy, ToListAsync               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Buscar viagens de recorrência com base no EventoId.
        ///    Retorna a viagem original quando não há recorrência.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    id - Identificador da viagem base.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;List&lt;Viagem&gt;&gt; - Lista de viagens relacionadas.
        /// </para>
        /// </summary>
        /// <param name="id">Identificador da viagem base.</param>
        /// <returns>Lista de viagens relacionadas.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViagensEventoPaginadoAsync                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Viagem, DbContext.ViewViagens, AsNoTracking, Stopwatch      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter viagens de um evento com paginação otimizada e dados completos da ViewViagens.
        ///    Separa o COUNT da consulta principal para reduzir custo de JOINs.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    eventoId - Identificador do evento<br/>
        ///    page - Página atual (1-based)<br/>
        ///    pageSize - Quantidade de registros por página
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;(List&lt;ViagemEventoDto&gt; viagens, int totalItems)&gt; - Lista paginada e total.
        /// </para>
        /// </summary>
        /// <param name="eventoId">Identificador do evento.</param>
        /// <param name="page">Página atual (1-based).</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <returns>Lista de viagens e total de itens.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetQuery                                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbSet.Where                                                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar IQueryable para composição de queries sem materialização.
        ///    Útil para Count(), Min(), Max(), etc.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    filter - Filtro opcional para composição da consulta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IQueryable&lt;Viagem&gt; - Consulta base para composição posterior.
        /// </para>
        /// </summary>
        /// <param name="filter">Filtro opcional.</param>
        /// <returns>Consulta base para composição.</returns>
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
