/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: OcorrenciaViagemRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade OcorrenciaViagem.                                       ║
   ║    Gerencia ocorrências e problemas registrados durante viagens (acidentes, panes, etc).          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OcorrenciaViagemRepository(FrotiXDbContext db)                                                ║
   ║    • IEnumerable<OcorrenciaViagem> GetAll(filter, includeProperties)                              ║
   ║    • OcorrenciaViagem GetFirstOrDefault(filter, includeProperties)                                 ║
   ║    • void Add(OcorrenciaViagem entity)                                                             ║
   ║    • void Remove(OcorrenciaViagem entity)                                                          ║
   ║    • void Update(OcorrenciaViagem entity)                                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Implementa interface IOcorrenciaViagemRepository com operações CRUD completas.                 ║
   ║    Essencial para rastreamento de incidentes durante viagens da frota.                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: OcorrenciaViagemRepository                                                         │
    // │ 🔌 IMPLEMENTA: IOcorrenciaViagemRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de ocorrências em viagens.
    // Registra acidentes, panes, multas e outros incidentes durante deslocamentos da frota.
    
    public class OcorrenciaViagemRepository : IOcorrenciaViagemRepository
        {
        private new readonly FrotiXDbContext _db;

        public OcorrenciaViagemRepository(FrotiXDbContext db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAll                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers, Services que listam ocorrências de viagens            │
        // │    ➡️ CHAMA       : DbContext.OcorrenciaViagem, Linq Where/Include                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista completa de ocorrências de viagem com filtros e includes opcionais.
        // Suporta eager loading de entidades relacionadas.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Expressão lambda para filtrar registros (opcional)
        // includeProperties - String com nomes de propriedades de navegação separadas por vírgula (opcional)
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;OcorrenciaViagem&gt; - Lista de ocorrências que atendem ao filtro
        
        
        // Param filter: Expressão lambda para filtrar registros
        // Param includeProperties: Propriedades de navegação a incluir no resultado
        // Returns: Lista de ocorrências de viagem
        public IEnumerable<OcorrenciaViagem> GetAll(Expression<Func<OcorrenciaViagem , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop.Trim());
                }
            }

            return query.ToList();
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFirstOrDefault                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers, Services que buscam ocorrência específica             │
        // │    ➡️ CHAMA       : DbContext.OcorrenciaViagem, Linq Where/Include/FirstOrDefault      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna a primeira ocorrência de viagem que atende ao filtro especificado.
        // Suporta eager loading de entidades relacionadas.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Expressão lambda obrigatória para filtrar registros
        // includeProperties - String com nomes de propriedades de navegação separadas por vírgula (opcional)
        
        
        
        // 📤 RETORNO:
        // OcorrenciaViagem? - Primeira ocorrência que atende ao filtro ou null se não encontrada
        
        
        // Param filter: Expressão lambda obrigatória para filtrar registros
        // Param includeProperties: Propriedades de navegação a incluir no resultado
        // Returns: Primeira ocorrência encontrada ou null
        public OcorrenciaViagem? GetFirstOrDefault(Expression<Func<OcorrenciaViagem , bool>> filter , string? includeProperties = null)
        {
            IQueryable<OcorrenciaViagem> query = _db.OcorrenciaViagem;

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(',' , StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop.Trim());
                }
            }

            return query.FirstOrDefault();
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Add                                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers, Services que registram novas ocorrências             │
        // │    ➡️ CHAMA       : DbContext.OcorrenciaViagem.Add()                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Adiciona nova ocorrência de viagem ao contexto do Entity Framework.
        // Registra incidentes durante deslocamentos da frota.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade OcorrenciaViagem a ser adicionada
        
        
        // Param entity: Entidade OcorrenciaViagem para inserção
        public void Add(OcorrenciaViagem entity)
            {
            _db.OcorrenciaViagem.Add(entity);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Remove                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers, Services que excluem ocorrências                      │
        // │    ➡️ CHAMA       : DbContext.OcorrenciaViagem.Remove()                                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Remove ocorrência de viagem do contexto do Entity Framework.
        // Utilizado para exclusão de registros de incidentes.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade OcorrenciaViagem a ser removida
        
        
        // Param entity: Entidade OcorrenciaViagem para remoção
        public void Remove(OcorrenciaViagem entity)
            {
            _db.OcorrenciaViagem.Remove(entity);
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers, Services que atualizam ocorrências                    │
        // │    ➡️ CHAMA       : DbContext.OcorrenciaViagem.Update()                                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma ocorrência de viagem existente.
        // Permite correções ou complementos em registros de incidentes.
        
        
        
        // 📥 PARÂMETROS:
        // entity - Entidade OcorrenciaViagem com dados atualizados
        
        
        // Param entity: Entidade OcorrenciaViagem com dados a serem persistidos
        public new void Update(OcorrenciaViagem entity)
            {
            _db.OcorrenciaViagem.Update(entity);
            }
        }
    }
