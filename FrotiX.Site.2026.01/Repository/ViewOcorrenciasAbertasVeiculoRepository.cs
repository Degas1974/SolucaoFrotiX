/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrenciasAbertasVeiculoRepository.cs                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewOcorrenciasAbertasVeiculo.                                      ║
   ║    Fornece consultas filtradas de ocorrências abertas por veículo.                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewOcorrenciasAbertasVeiculoRepository(FrotiXDbContext db)                                   ║
   ║    • GetAll(Expression<Func<ViewOcorrenciasAbertasVeiculo, bool>> filter = null, ...)             ║
   ║    • GetFirstOrDefault(Expression<Func<ViewOcorrenciasAbertasVeiculo, bool>> filter, ...)         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    O parâmetro includeProperties é aceito por compatibilidade, mas não é utilizado.               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewOcorrenciasAbertasVeiculoRepository                                            │
    // │ 📦 HERDA DE: (não aplicável)                                                                  │
    // │ 🔌 IMPLEMENTA: IViewOcorrenciasAbertasVeiculoRepository                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de ocorrências abertas por veículo.
    // Disponibiliza consultas filtradas para uso em serviços e controllers.
    
    public class ViewOcorrenciasAbertasVeiculoRepository : IViewOcorrenciasAbertasVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewOcorrenciasAbertasVeiculoRepository                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : (atribuição de contexto)                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewOcorrenciasAbertasVeiculoRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAll                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewOcorrenciasAbertasVeiculo, Where, ToList               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de ocorrências abertas por veículo, com filtro opcional.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional para a consulta
        // includeProperties - Propriedades de navegação (não utilizado)
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;ViewOcorrenciasAbertasVeiculo&gt; - Lista de ocorrências abertas.
        
        
        // Param filter: Filtro opcional para a consulta.
        // Param includeProperties: Includes (não utilizado).
        // Returns: Lista de ocorrências abertas.
        public IEnumerable<ViewOcorrenciasAbertasVeiculo> GetAll(Expression<Func<ViewOcorrenciasAbertasVeiculo , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<ViewOcorrenciasAbertasVeiculo> query = _db.ViewOcorrenciasAbertasVeiculo;

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFirstOrDefault                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewOcorrenciasAbertasVeiculo, Where, FirstOrDefault       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar a primeira ocorrência aberta que atende ao filtro informado.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro obrigatório para a consulta
        // includeProperties - Propriedades de navegação (não utilizado)
        
        
        
        // 📤 RETORNO:
        // ViewOcorrenciasAbertasVeiculo? - Registro encontrado ou null.
        
        
        // Param filter: Filtro obrigatório.
        // Param includeProperties: Includes (não utilizado).
        // Returns: Registro encontrado ou null.
        public ViewOcorrenciasAbertasVeiculo? GetFirstOrDefault(Expression<Func<ViewOcorrenciasAbertasVeiculo , bool>> filter , string? includeProperties = null)
        {
            return _db.ViewOcorrenciasAbertasVeiculo.Where(filter).FirstOrDefault();
        }
    }
}
