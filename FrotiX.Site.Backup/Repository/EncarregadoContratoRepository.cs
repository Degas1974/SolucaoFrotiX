/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EncarregadoContratoRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para vínculo entre encarregados e contratos terceirizados.           ║
   ║    Gerencia a associação de encarregados aos contratos de prestação de serviço.                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [Update] : Atualiza vínculo encarregado-contrato                                               ║
   ║    (EncarregadoContrato) -> void                                                                   ║
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
     * ⚡ CLASSE: EncarregadoContratoRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Repositório para vínculo entre encarregados e contratos terceirizados
     *
     * 📥 ENTRADAS     : DbContext injetado no construtor
     *
     * 📤 SAÍDAS       : Operações de atualização de associações encarregado-contrato
     *
     * 🔗 CHAMADA POR  : Controllers de Contrato, Services de Gestão
     *
     * 🔄 CHAMA        : DbContext, Repository<T> (classe base)
     *
     * 📦 DEPENDÊNCIAS : FrotiXDbContext, Repository<EncarregadoContrato>, IEncarregadoContratoRepository
     *
     * 📝 OBSERVAÇÕES  : [NEGOCIO] Mapeia responsabilidades de encarregados sobre contratos
     *********************************************************************************************/
    public class EncarregadoContratoRepository : Repository<EncarregadoContrato>, IEncarregadoContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        /********************************************************************************************
         * ⚡ MÉTODO: EncarregadoContratoRepository (Construtor)
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Inicializar repositório com injeção do DbContext
         *
         * 📥 ENTRADAS     : db [FrotiXDbContext] - Contexto do banco de dados
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, DI container
         *
         * ➡️ CHAMA        : base(db)
         *********************************************************************************************/
        public EncarregadoContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar vínculo entre encarregado e contrato
         *
         * 📥 ENTRADAS     : encarregadoContrato [EncarregadoContrato] - Associação a atualizar
         *
         * 📤 SAÍDAS       : void - Alterações persistidas no DbContext
         *
         * ⬅️ CHAMADO POR  : UnitOfWork.SaveAsync(), Controllers de Contrato
         *
         * ➡️ CHAMA        : DbContext.Update(), DbContext.SaveChanges()
         *
         * 📝 OBSERVAÇÕES  : [NEGOCIO] Atualiza responsabilidade de encarregado sobre contrato
         *********************************************************************************************/
        public new void Update(EncarregadoContrato encarregadoContrato)
        {
            // [DB] Marcar como modificada e persistir
            _db.EncarregadoContrato.Update(encarregadoContrato);
            _db.SaveChanges();
        }
    }
}
