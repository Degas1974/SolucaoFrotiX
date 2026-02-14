/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrenciasViagemRepository.cs                                                     ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewOcorrenciasViagem.                                              ║
   ║    Fornece consultas filtradas de ocorrências registradas em viagens.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewOcorrenciasViagemRepository(FrotiXDbContext db)                                           ║
   ║    • GetAll(Expression<Func<ViewOcorrenciasViagem, bool>> filter = null, ...)                     ║
   ║    • GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem, bool>> filter, ...)                 ║
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
    // │ 🎯 CLASSE: ViewOcorrenciasViagemRepository                                                    │
    // │ 📦 HERDA DE: (não aplicável)                                                                  │
    // │ 🔌 IMPLEMENTA: IViewOcorrenciasViagemRepository                                               │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de ocorrências em viagens.
    // Disponibiliza consultas filtradas para uso em serviços e controllers.
    
    public class ViewOcorrenciasViagemRepository : IViewOcorrenciasViagemRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewOcorrenciasViagemRepository                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : (atribuição de contexto)                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewOcorrenciasViagemRepository(FrotiXDbContext db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetAll                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewOcorrenciasViagem, Where, ToList                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de ocorrências de viagem, com filtro opcional.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro opcional para a consulta
        // includeProperties - Propriedades de navegação (não utilizado)
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;ViewOcorrenciasViagem&gt; - Lista de ocorrências.
        
        
        // Param filter: Filtro opcional para a consulta.
        // Param includeProperties: Includes (não utilizado).
        // Returns: Lista de ocorrências de viagem.
        public IEnumerable<ViewOcorrenciasViagem> GetAll(Expression<Func<ViewOcorrenciasViagem , bool>>? filter = null , string? includeProperties = null)
        {
            IQueryable<ViewOcorrenciasViagem> query = _db.ViewOcorrenciasViagem;

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetFirstOrDefault                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewOcorrenciasViagem, Where, FirstOrDefault               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar a primeira ocorrência que atende ao filtro informado.
        
        
        
        // 📥 PARÂMETROS:
        // filter - Filtro obrigatório para a consulta
        // includeProperties - Propriedades de navegação (não utilizado)
        
        
        
        // 📤 RETORNO:
        // ViewOcorrenciasViagem? - Registro encontrado ou null.
        
        
        // Param filter: Filtro obrigatório.
        // Param includeProperties: Includes (não utilizado).
        // Returns: Registro encontrado ou null.
        public ViewOcorrenciasViagem? GetFirstOrDefault(Expression<Func<ViewOcorrenciasViagem , bool>> filter , string? includeProperties = null)
        {
            return _db.ViewOcorrenciasViagem.Where(filter).FirstOrDefault();
        }
    }
}
