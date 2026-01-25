// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║                                                                                                   ║
// ║   ██╗████████╗███████╗███╗   ███╗    ██╗   ██╗███████╗██╗ ██████╗██╗   ██╗██╗      ██████╗     ║
// ║   ██║╚══██╔══╝██╔════╝████╗ ████║    ██║   ██║██╔════╝██║██╔════╝██║   ██║██║     ██╔═══██╗    ║
// ║   ██║   ██║   █████╗  ██╔████╔██║    ██║   ██║█████╗  ██║██║     ██║   ██║██║     ██║   ██║    ║
// ║   ██║   ██║   ██╔══╝  ██║╚██╔╝██║    ╚██╗ ██╔╝██╔══╝  ██║██║     ██║   ██║██║     ██║   ██║    ║
// ║   ██║   ██║   ███████╗██║ ╚═╝ ██║     ╚████╔╝ ███████╗██║╚██████╗╚██████╔╝███████╗╚██████╔╝    ║
// ║   ╚═╝   ╚═╝   ╚══════╝╚═╝     ╚═╝      ╚═══╝  ╚══════╝╚═╝ ╚═════╝ ╚═════╝ ╚══════╝ ╚═════╝     ║
// ║                          ██████╗ ██████╗ ███╗   ██╗████████╗██████╗  █████╗ ████████╗ ██████╗  ║
// ║                         ██╔════╝██╔═══██╗████╗  ██║╚══██╔══╝██╔══██╗██╔══██╗╚══██╔══╝██╔═══██╗ ║
// ║                         ██║     ██║   ██║██╔██╗ ██║   ██║   ██████╔╝███████║   ██║   ██║   ██║ ║
// ║                         ██║     ██║   ██║██║╚██╗██║   ██║   ██╔══██╗██╔══██║   ██║   ██║   ██║ ║
// ║                         ╚██████╗╚██████╔╝██║ ╚████║   ██║   ██║  ██║██║  ██║   ██║   ╚██████╔╝ ║
// ║                          ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝  ║
// ║                                                                                                   ║
// ║   📋 ARQUIVO: ItemVeiculoContratoRepository.cs                                                    ║
// ║   📂 LOCALIZAÇÃO: Repository/                                                                     ║
// ║   📅 DOCUMENTADO EM: 2026-01-19                                                                   ║
// ║   👤 AUTOR: GitHub Copilot (Documentação INTRA-CODE)                                              ║
// ║   ⚙️ TECNOLOGIAS: C#, .NET 10, EF Core, Repository Pattern                                       ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📖 DESCRIÇÃO GERAL                                                                                ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ Gerencia itens de veículos cadastrados em CONTRATOS (não em Atas).                               ║
// ║ Similar a ItemVeiculoAtaRepository, mas vinculado a Contrato ao invés de Ata de Registro.        ║
// ║ Relaciona tipos/categorias de veículos com contratos ativos.                                     ║
// ║                                                                                                   ║
// ║ CAMPOS PRINCIPAIS:                                                                                ║
// ║ • ItemVeiculoId (PK), Descricao (tipo do veículo)                                                ║
// ║ • Provavelmente: ContratoId (FK), ValorUnitario, Quantidade                                      ║
// ║                                                                                                   ║
// ║ RELACIONAMENTOS:                                                                                  ║
// ║ ItemVeiculoContrato (N) ──────────── (1) Contrato                                                ║
// ║ ItemVeiculoContrato (1) ──────────── (N) Veiculo (categoria/tipo)                                ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ ⚠️ PROBLEMAS IDENTIFICADOS                                                                        ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ❌ MESMO PADRÃO de ItemVeiculoAtaRepository: Update() quebra Unit of Work + código morto         ║
// ║ ⚠️ Sem filtro em GetListForDropDown (retorna todos os registros)                                 ║
// ║                                                                                                   ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

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
    public class ItemVeiculoContratoRepository : Repository<ItemVeiculoContrato>, IItemVeiculoContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public ItemVeiculoContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca itens de veículo do contrato para popular dropdown
        // ⚠️ PROBLEMA: Sem filtro por Status/Ativo (retorna TODOS)
        public IEnumerable<SelectListItem> GetItemVeiculoContratoListForDropDown()
        {
            return _db.ItemVeiculoContrato
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao,
                    Value = i.ItemVeiculoId.ToString()
                });
        }

        // [ETAPA] Atualiza item de veículo do contrato
        // ⚠️ CÓDIGO MORTO: objFromDb buscado mas não utilizado
        // ⚠️ QUEBRA UNIT OF WORK: SaveChanges() direto
        public new void Update(ItemVeiculoContrato itemveiculocontrato)
        {
            var objFromDb = _db.ItemVeiculoContrato.AsTracking().FirstOrDefault(s => s.ItemVeiculoId == itemveiculocontrato.ItemVeiculoId);

            _db.Update(itemveiculocontrato);
            _db.SaveChanges();

        }


    }
}


