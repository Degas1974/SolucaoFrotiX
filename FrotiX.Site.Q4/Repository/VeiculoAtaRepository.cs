/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculoAtaRepository.cs                                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para associação N:N entre veículos e atas de registro de preços.                    ║
   ║    Centraliza listagens para UI e atualização por chave composta.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • VeiculoAtaRepository(FrotiXDbContext db)                                                       ║
   ║    • GetVeiculoAtaListForDropDown()                                                                ║
   ║    • Update(VeiculoAta veiculoAta)                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Entidade com chave composta (VeiculoId + AtaId).                                                 ║
   ║    ⚠️ ATENÇÃO: Listagem de dropdown está com campos comentados (placeholder).                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: VeiculoAtaRepository                                                               │
    // │ 📦 HERDA DE: Repository                                                           │
    // │ 🔌 IMPLEMENTA: IVeiculoAtaRepository                                                          │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela associação entre veículos e atas de registro de preços.
    // Disponibiliza listagem e atualização por chave composta.
    
    public class VeiculoAtaRepository : Repository<VeiculoAta>, IVeiculoAtaRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: VeiculoAtaRepository                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public VeiculoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetVeiculoAtaListForDropDown                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.VeiculoAta, Select                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retornar lista de associações veículo/ata para dropdowns.
        // Atualmente está como placeholder, com campos de texto e valor comentados.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção (placeholder).
        public IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown()
            {
            return _db.VeiculoAta.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.VeiculoAta.FirstOrDefault, _db.Update, _db.SaveChanges       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar a associação veículo/ata usando chave composta.
        
        
        
        // 📥 PARÂMETROS:
        // veiculoAta - Entidade contendo os dados atualizados.
        
        
        // Param veiculoAta: Entidade <see cref="VeiculoAta"/> com dados atualizados.
        public new void Update(VeiculoAta veiculoAta)
            {
            var objFromDb = _db.VeiculoAta.FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));

            _db.Update(veiculoAta);
            _db.SaveChanges();

            }


        }
    }
