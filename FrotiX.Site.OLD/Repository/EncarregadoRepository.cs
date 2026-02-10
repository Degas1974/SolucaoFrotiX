/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EncarregadoRepository.cs                                                               ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Encarregado.                                            ║
   ║    Gerencia encarregados responsáveis pela supervisão das operações de frota.                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [Update] : Atualização da entidade Encarregado                                                 ║
   ║    (Encarregado) -> void                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header dos Métodos. ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    /********************************************************************************************
     * ⚡ CLASSE: EncarregadoRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para encarregados supervisores de frota
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Operações CRUD de encarregados
     *
     * 🔗 CHAMADA POR  : Controllers de Encarregado, Services de Gestão
     *
     * 🔄 CHAMA        : DbContext, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<Encarregado>, IEncarregadoRepository
     *
     * 📝 OBSERVAÇÕES  : [NEGOCIO] Supervisores responsáveis pela operação de frota
     *********************************************************************************************/
    public class EncarregadoRepository : Repository<Encarregado>, IEncarregadoRepository
    {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: EncarregadoRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public EncarregadoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de encarregado no banco
         *
         * 📥 ENTRADAS     : encarregado [Encarregado] - Entidade a atualizar
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de edição
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *********************************************************************************************/
        public new void Update(Encarregado encarregado)
        {
            // [DB] Marcar como modificada e persistir
            _db.Encarregado.Update(encarregado);
            _db.SaveChanges();
        }
    }
}
