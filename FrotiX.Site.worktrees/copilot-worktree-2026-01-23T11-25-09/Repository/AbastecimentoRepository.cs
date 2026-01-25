using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  📦 REPOSITORY: AbastecimentoRepository                                      ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Repository especializado para entidade Abastecimento (padrão Repository).  ║
    /// ║  Herda Repository<Abastecimento> e implementa IAbastecimentoRepository.     ║
    /// ║                                                                              ║
    /// ║  ENTIDADE:                                                                   ║
    /// ║  - Abastecimento: Registros de abastecimento de veículos.                   ║
    /// ║                                                                              ║
    /// ║  MÉTODOS CUSTOMIZADOS:                                                       ║
    /// ║  - GetAbastecimentoListForDropDown(): Lista para dropdown (Litros - Value). ║
    /// ║  - Update(): Atualização manual com SaveChanges() imediato.                 ║
    /// ║                                                                              ║
    /// ║  HERANÇA:                                                                    ║
    /// ║  - Repository<Abastecimento>: Métodos base (Add, Remove, Get, GetAll).      ║
    /// ║  - IAbastecimentoRepository: Interface contrato.                            ║
    /// ║                                                                              ║
    /// ║  PADRÃO UNIT OF WORK:                                                        ║
    /// ║  - Usado via UnitOfWork.Abastecimento.                                      ║
    /// ║  - SaveChanges() deve ser chamado no UnitOfWork (exceto Update).            ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public class AbastecimentoRepository : Repository<Abastecimento>, IAbastecimentoRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// Construtor que recebe DbContext e passa para classe base.
        /// </summary>
        public AbastecimentoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetAbastecimentoListForDropDown
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de abastecimentos formatada para dropdown.
        /// │    Text: Litros, Value: AbastecimentoId (Guid).
        /// │
        /// │ USO:
        /// │    Dropdowns de seleção de abastecimento (raro, mais comum filtrar por veículo).
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> com Text=Litros e Value=AbastecimentoId.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetAbastecimentoListForDropDown()
        {
            // [QUERY] - Projeta Abastecimento para SelectListItem (Text=Litros, Value=AbastecimentoId)
            return _db.Abastecimento
            .Select(i => new SelectListItem()
            {
                Text = i.Litros.ToString(),
                Value = i.AbastecimentoId.ToString()
            });
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualização Manual com SaveChanges Imediato)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza abastecimento no banco de dados e PERSISTE IMEDIATAMENTE.
        /// │    Chama SaveChanges() dentro do método (diferente do padrão Repository).
        /// │
        /// │ ATENÇÃO:
        /// │    - SaveChanges() é chamado DENTRO do método (não via UnitOfWork).
        /// │    - Quebra padrão Unit of Work (deve ser refatorado).
        /// │    - AsTracking() força EF Core a rastrear entidade (permite Update).
        /// │
        /// │ REFATORAÇÃO SUGERIDA:
        /// │    Remover SaveChanges() e deixar UnitOfWork.Save() controlar transação.
        /// │
        /// │ PARÂMETROS:
        /// │    - abastecimento: Entidade Abastecimento com dados atualizados.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(Abastecimento abastecimento)
        {
            // [QUERY] - Busca entidade original no banco (AsTracking para rastreamento)
            var objFromDb = _db.Abastecimento.AsTracking().FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);

            // [UPDATE] - Marca entidade como modificada e persiste imediatamente
            _db.Update(abastecimento);
            _db.SaveChanges(); // ⚠️ SaveChanges() IMEDIATO (quebra padrão Unit of Work)
        }
    }
}


