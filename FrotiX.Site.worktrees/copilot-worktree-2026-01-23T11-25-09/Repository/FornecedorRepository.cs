// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║                                                                                                   ║
// ║   ███████╗ ██████╗ ██████╗ ███╗   ██╗███████╗ ██████╗███████╗██████╗  ██████╗ ██████╗           ║
// ║   ██╔════╝██╔═══██╗██╔══██╗████╗  ██║██╔════╝██╔════╝██╔════╝██╔══██╗██╔═══██╗██╔══██╗          ║
// ║   █████╗  ██║   ██║██████╔╝██╔██╗ ██║█████╗  ██║     █████╗  ██║  ██║██║   ██║██████╔╝          ║
// ║   ██╔══╝  ██║   ██║██╔══██╗██║╚██╗██║██╔══╝  ██║     ██╔══╝  ██║  ██║██║   ██║██╔══██╗          ║
// ║   ██║     ╚██████╔╝██║  ██║██║ ╚████║███████╗╚██████╗███████╗██████╔╝╚██████╔╝██║  ██║          ║
// ║   ╚═╝      ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝ ╚═════╝╚══════╝╚═════╝  ╚═════╝ ╚═╝  ╚═╝          ║
// ║                                                                                                   ║
// ║   📋 ARQUIVO: FornecedorRepository.cs                                                             ║
// ║   📂 LOCALIZAÇÃO: Repository/                                                                     ║
// ║   📅 DOCUMENTADO EM: 2026-01-14                                                                   ║
// ║   👤 AUTOR: GitHub Copilot (Documentação INTRA-CODE)                                              ║
// ║   ⚙️ TECNOLOGIAS: C#, .NET 10, EF Core, Repository Pattern                                       ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📖 DESCRIÇÃO GERAL                                                                                ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ Gerencia fornecedores de produtos/serviços para a frota (postos de combustível, oficinas, etc.). ║
// ║ Repositório simples com apenas 2 métodos: GetListForDropDown e Update.                           ║
// ║                                                                                                   ║
// ║ CAMPOS PRINCIPAIS:                                                                                ║
// ║ • FornecedorId (PK), DescricaoFornecedor (Nome)                                                  ║
// ║ • Status (bool) - Ativo/Inativo                                                                   ║
// ║ • Provavelmente: CNPJ, Endereco, Telefone, etc. (ver Model)                                      ║
// ║                                                                                                   ║
// ║ RELACIONAMENTOS:                                                                                  ║
// ║ Fornecedor (1) ──────────── (N) NotaFiscal                                                       ║
// ║ Fornecedor (1) ──────────── (N) Abastecimento (postos de combustível)                            ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 🎯 FUNCIONALIDADES PRINCIPAIS                                                                     ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ✅ GetFornecedorListForDropDown()                                                                 ║
// ║    • Retorna SelectListItem para dropdowns MVC                                                    ║
// ║    • Filtro: Status = true (apenas ativos)                                                        ║
// ║    • Ordenação: DescricaoFornecedor ASC                                                           ║
// ║                                                                                                   ║
// ║ ✅ Update(Fornecedor)                                                                             ║
// ║    • ⚠️ QUEBRA UNIT OF WORK: Chama _db.Update() + SaveChanges()                                  ║
// ║    • Atualiza registro completo do fornecedor                                                     ║
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
// ║    → Linha: var objFromDb = _db.Fornecedor.AsTracking().FirstOrDefault(...)                      ║
// ║    → Não faz nada com objFromDb (dead code)                                                       ║
// ║    → SOLUÇÃO: Remover linha ou usar para update manual de campos                                  ║
// ║                                                                                                   ║
// ║ ✅ BOA PRÁTICA: GetFornecedorListForDropDown filtra por Status = true                            ║
// ║    → Evita exibir fornecedores inativos nos dropdowns                                             ║
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
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        private new readonly FrotiXDbContext _db;

        public FornecedorRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca fornecedores ATIVOS para popular dropdown
        // Retorna SelectListItem com FornecedorId (GUID) e DescricaoFornecedor
        // Filtro: Status = true (apenas ativos)
        // Ordenação: DescricaoFornecedor ASC
        public IEnumerable<SelectListItem> GetFornecedorListForDropDown()
        {

            return _db.Fornecedor
            .Where(f => f.Status == true)
            .OrderBy(o => o.DescricaoFornecedor)
            .Select(i => new SelectListItem()
            {
                Text = i.DescricaoFornecedor,
                Value = i.FornecedorId.ToString()
            }); ;
        }

        // [ETAPA] Atualiza fornecedor existente
        // ⚠️ CÓDIGO MORTO: objFromDb buscado mas não utilizado
        // ⚠️ QUEBRA UNIT OF WORK: Chama _db.Update() + SaveChanges() diretamente
        // TODO: Remover objFromDb (dead code) OU usá-lo para update manual de campos
        // TODO: Remover SaveChanges() para respeitar Unit of Work pattern
        public new void Update(Fornecedor fornecedor)
        {
            var objFromDb = _db.Fornecedor.AsTracking().FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);

            _db.Update(fornecedor);
            _db.SaveChanges();

        }


    }
}


