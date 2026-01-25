// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║                                                                                                   ║
// ║   ██╗████████╗███████╗███╗   ███╗    ██╗   ██╗███████╗██╗ ██████╗██╗   ██╗██╗      ██████╗     ║
// ║   ██║╚══██╔══╝██╔════╝████╗ ████║    ██║   ██║██╔════╝██║██╔════╝██║   ██║██║     ██╔═══██╗    ║
// ║   ██║   ██║   █████╗  ██╔████╔██║    ██║   ██║█████╗  ██║██║     ██║   ██║██║     ██║   ██║    ║
// ║   ██║   ██║   ██╔══╝  ██║╚██╔╝██║    ╚██╗ ██╔╝██╔══╝  ██║██║     ██║   ██║██║     ██║   ██║    ║
// ║   ██║   ██║   ███████╗██║ ╚═╝ ██║     ╚████╔╝ ███████╗██║╚██████╗╚██████╔╝███████╗╚██████╔╝    ║
// ║   ╚═╝   ╚═╝   ╚══════╝╚═╝     ╚═╝      ╚═══╝  ╚══════╝╚═╝ ╚═════╝ ╚═════╝ ╚══════╝ ╚═════╝     ║
// ║                                   █████╗ ████████╗ █████╗                                        ║
// ║                                  ██╔══██╗╚══██╔══╝██╔══██╗                                       ║
// ║                                  ███████║   ██║   ███████║                                       ║
// ║                                  ██╔══██║   ██║   ██╔══██║                                       ║
// ║                                  ██║  ██║   ██║   ██║  ██║                                       ║
// ║                                  ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝                                       ║
// ║                                                                                                   ║
// ║   📋 ARQUIVO: ItemVeiculoAtaRepository.cs                                                         ║
// ║   📂 LOCALIZAÇÃO: Repository/                                                                     ║
// ║   📅 DOCUMENTADO EM: 2026-01-19                                                                   ║
// ║   👤 AUTOR: GitHub Copilot (Documentação INTRA-CODE)                                              ║
// ║   ⚙️ TECNOLOGIAS: C#, .NET 10, EF Core, Repository Pattern                                       ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📖 DESCRIÇÃO GERAL                                                                                ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ Gerencia itens de veículos cadastrados em Atas de Registro de Preços.                            ║
// ║ Relaciona tipos/categorias de veículos com atas (ex: Sedan, SUV, Van, Caminhão, etc.).           ║
// ║ Repositório simples com apenas 2 métodos: GetListForDropDown e Update.                           ║
// ║                                                                                                   ║
// ║ CAMPOS PRINCIPAIS:                                                                                ║
// ║ • ItemVeiculoAtaId (PK), Descricao (tipo do veículo)                                             ║
// ║ • Provavelmente: AtaRegistroPrecosId (FK), ValorUnitario, Quantidade                             ║
// ║                                                                                                   ║
// ║ RELACIONAMENTOS:                                                                                  ║
// ║ ItemVeiculoAta (N) ──────────── (1) AtaRegistroPrecos                                            ║
// ║ ItemVeiculoAta (1) ──────────── (N) Veiculo (categoria/tipo do veículo)                          ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 🎯 FUNCIONALIDADES PRINCIPAIS                                                                     ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ✅ GetItemVeiculoAtaListForDropDown()                                                             ║
// ║    • Retorna SelectListItem para dropdowns MVC                                                    ║
// ║    • Ordenação: Descricao ASC                                                                     ║
// ║    • ⚠️ Não filtra por Status/Ativo (retorna TODOS os registros)                                 ║
// ║                                                                                                   ║
// ║ ✅ Update(ItemVeiculoAta)                                                                         ║
// ║    • ⚠️ QUEBRA UNIT OF WORK: Chama _db.Update() + SaveChanges()                                  ║
// ║    • Atualiza registro completo do item                                                           ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ ⚠️ PROBLEMAS IDENTIFICADOS                                                                        ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ❌ PROBLEMA: Update() quebra Unit of Work                                                         ║
// ║    → Chama _db.Update() + SaveChanges() diretamente                                               ║
// ║    → Impede transações coordenadas pelo Controller/Service                                        ║
// ║    → SOLUÇÃO: Remover SaveChanges(), deixar para UnitOfWork                                       ║
// ║                                                                                                   ║
// ║ ⚠️ CÓDIGO MORTO: objFromDb buscado mas não utilizado                                              ║
// ║    → Linha: var objFromDb = _db.ItemVeiculoAta.AsTracking().FirstOrDefault(...)                  ║
// ║    → Não faz nada com objFromDb (dead code)                                                       ║
// ║    → SOLUÇÃO: Remover linha ou usar para update manual de campos                                  ║
// ║                                                                                                   ║
// ║ ⚠️ OBSERVAÇÃO: GetItemVeiculoAtaListForDropDown() sem filtro                                      ║
// ║    → Retorna TODOS os itens (inclusive inativos, se houver campo Status)                         ║
// ║    → SUGESTÃO: Adicionar filtro ".Where(x => x.Ativo)" se campo existir                           ║
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
    public class ItemVeiculoAtaRepository : Repository<ItemVeiculoAta>, IItemVeiculoAtaRepository
    {
        private new readonly FrotiXDbContext _db;

        public ItemVeiculoAtaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca itens de veículo da ata para popular dropdown
        // Retorna SelectListItem com ItemVeiculoAtaId (GUID) e Descricao
        // Ordenação: Descricao ASC
        // ⚠️ PROBLEMA: Não filtra por Status/Ativo (retorna TODOS os registros)
        // TODO: Adicionar filtro ".Where(x => x.Ativo)" se campo existir
        public IEnumerable<SelectListItem> GetItemVeiculoAtaListForDropDown()
        {
            return _db.ItemVeiculoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Descricao,
                    Value = i.ItemVeiculoAtaId.ToString()
                });
        }

        // [ETAPA] Atualiza item de veículo da ata existente
        // ⚠️ CÓDIGO MORTO: objFromDb buscado mas não utilizado
        // ⚠️ QUEBRA UNIT OF WORK: Chama _db.Update() + SaveChanges() diretamente
        // TODO: Remover objFromDb (dead code) OU usá-lo para update manual de campos
        // TODO: Remover SaveChanges() para respeitar Unit of Work pattern
        public new void Update(ItemVeiculoAta itemveiculoata)
        {
            var objFromDb = _db.ItemVeiculoAta.AsTracking().FirstOrDefault(s => s.ItemVeiculoAtaId == itemveiculoata.ItemVeiculoAtaId);

            _db.Update(itemveiculoata);
            _db.SaveChanges();

        }


    }
}


