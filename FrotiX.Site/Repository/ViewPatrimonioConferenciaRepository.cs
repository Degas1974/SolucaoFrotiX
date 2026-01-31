/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewPatrimonioConferenciaRepository.cs                                                 ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewPatrimonioConferencia.                                          ║
   ║    Centraliza acesso a dados consolidados de conferência patrimonial.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewPatrimonioConferenciaRepository(FrotiXDbContext db)                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; métodos de consulta são herdados de Repository<T>.                  ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models.Views;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewPatrimonioConferenciaRepository                                                │
    // │ 📦 HERDA DE: Repository                                            │
    // │ 🔌 IMPLEMENTA: IViewPatrimonioConferenciaRepository                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de conferência patrimonial.
    // Utiliza os métodos genéricos herdados para consultas.
    
    public class ViewPatrimonioConferenciaRepository :Repository<ViewPatrimonioConferencia>, IViewPatrimonioConferenciaRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewPatrimonioConferenciaRepository                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewPatrimonioConferenciaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // GetAll e GetFirstOrDefault já estão implementados na classe base Repository<T>
        // Métodos adicionais específicos podem ser implementados aqui se necessário
    }
}
