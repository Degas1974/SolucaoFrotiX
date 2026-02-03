/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoAtaRepository.cs                                                            ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de atas de registro de preços.                                     ║
   ║    Centraliza listagens para UI e atualizações da entidade RepactuacaoAta.                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoAtaRepository(FrotiXDbContext db)                                                    ║
   ║    • GetRepactuacaoAtaListForDropDown()                                                             ║
   ║    • Update(RepactuacaoAta repactuacaoitemveiculoata)                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem é ordenada por Descricao para apresentação em dropdowns.                              ║
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
    // │ 🎯 CLASSE: RepactuacaoAtaRepository                                                            │
    // │ 📦 HERDA DE: Repository                                                        │
    // │ 🔌 IMPLEMENTA: IRepactuacaoAtaRepository                                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de atas de registro de preços.
    // Provê consultas para dropdowns e atualização de registros.
    
    public class RepactuacaoAtaRepository : Repository<RepactuacaoAta>, IRepactuacaoAtaRepository
        {
        private new readonly FrotiXDbContext _db;


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoAtaRepository                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork [UnitOfWork.cs:137]                                        │
        // │    ➡️ CHAMA       : base(db) [linha 62]                                                  │
        // │ 📦 DEPENDÊNCIAS  : FrotiXDbContext                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db [FrotiXDbContext] - Contexto do banco de dados da aplicação.


        // 📤 SAÍDAS: Instância inicializada do repositório

        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoAtaRepository(FrotiXDbContext db) : base(db)
            {
            try
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            catch (Exception erro)
            {
                throw;
            }
            }


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoAtaListForDropDown                                               │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers (dropdown) [linha ~100]                                 │
        // │    ➡️ CHAMA       : _db.RepactuacaoAta.OrderBy() [linha 106]                             │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Obter lista de repactuações de atas para composição de dropdowns.
        // Ordena os registros pela descrição.



        // 📥 PARÂMETROS: Nenhum

        // 📤 RETORNO:
        // IEnumerable<SelectListItem> - Itens prontos para seleção em UI.

        // 📝 OBSERVAÇÕES: Ordenação alfabética por Descricao

        // Returns: Lista de itens de seleção para repactuações de atas.
        public IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown()
            {
            try
            {
                // [DB] Consulta repactuações, ordena por descrição e projeta para dropdown
                return _db.RepactuacaoAta
                    .OrderBy(o => o.Descricao)
                    .Select(i => new SelectListItem()
                        {
                        Text = i.Descricao,
                        Value = i.RepactuacaoAtaId.ToString()
                        });
            }
            catch (Exception erro)
            {
                throw;
            }
            }


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers [linha ~100]                                             │
        // │    ➡️ CHAMA       : _db.FirstOrDefault() [linha 128]                                     │
        // │                     _db.Update() [linha 130]                                             │
        // │                     _db.SaveChanges() [linha 131]                                        │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de ata no banco de dados.



        // 📥 PARÂMETROS:
        // repactuacaoitemveiculoata [RepactuacaoAta] - Entidade contendo os dados atualizados.

        // 📤 SAÍDAS: void

        // 📝 OBSERVAÇÕES: Salva mudanças imediatamente no banco de dados

        // Param repactuacaoitemveiculoata: Entidade <see cref="RepactuacaoAta"/> com dados atualizados.
        public new void Update(RepactuacaoAta repactuacaoitemveiculoata)
            {
            try
            {
                // [VALIDACAO] Verificar se entidade não é nula
                if (repactuacaoitemveiculoata == null)
                    throw new ArgumentNullException(nameof(repactuacaoitemveiculoata));

                // [DB] Buscar registro existente
                var objFromDb = _db.RepactuacaoAta.FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);

                // [DB] Atualizar e persistir mudanças
                _db.Update(repactuacaoitemveiculoata);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                throw;
            }
            }


        }
    }
