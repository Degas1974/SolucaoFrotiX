/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoContratoRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações/aditivos de contratos administrativos.                            ║
   ║    Centraliza listagens para UI e atualizações da entidade RepactuacaoContrato.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoContratoRepository(FrotiXDbContext db)                                              ║
   ║    • GetRepactuacaoContratoListForDropDown()                                                        ║
   ║    • Update(RepactuacaoContrato RepactuacaoContrato)                                               ║
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
    // │ 🎯 CLASSE: RepactuacaoContratoRepository                                                       │
    // │ 📦 HERDA DE: Repository                                                   │
    // │ 🔌 IMPLEMENTA: IRepactuacaoContratoRepository                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de contratos administrativos.
    // Provê consultas para dropdowns e atualização de registros.
    
    public class RepactuacaoContratoRepository : Repository<RepactuacaoContrato>, IRepactuacaoContratoRepository
        {
        private new readonly FrotiXDbContext _db;


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoContratoRepository                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork [UnitOfWork.cs:135]                                        │
        // │    ➡️ CHAMA       : base(db) [linha 62]                                                  │
        // │ 📦 DEPENDÊNCIAS  : FrotiXDbContext                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db [FrotiXDbContext] - Contexto do banco de dados da aplicação.


        // 📤 SAÍDAS: Instância inicializada do repositório

        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoContratoRepository(FrotiXDbContext db) : base(db)
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
        // │ ⚡ MÉTODO: GetRepactuacaoContratoListForDropDown                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers (dropdown) [linha ~100]                                 │
        // │    ➡️ CHAMA       : _db.RepactuacaoContrato.OrderBy() [linha 107]                        │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Obter lista de repactuações de contratos para composição de dropdowns.
        // Ordena os registros pela descrição.



        // 📥 PARÂMETROS: Nenhum

        // 📤 RETORNO:
        // IEnumerable<SelectListItem> - Itens prontos para seleção em UI.

        // 📝 OBSERVAÇÕES: Ordenação alfabética por Descricao

        // Returns: Lista de itens de seleção para repactuações de contratos.
        public IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown()
            {
            try
            {
                // [DB] Consulta repactuações, ordena por descrição e projeta para dropdown
                return _db.RepactuacaoContrato
                    .OrderBy(o => o.Descricao)
                    .Select(i => new SelectListItem()
                        {
                        Text = i.Descricao,
                        Value = i.RepactuacaoContratoId.ToString()
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
        // │    ➡️ CHAMA       : _db.FirstOrDefault() [linha 129]                                     │
        // │                     _db.Update() [linha 131]                                             │
        // │                     _db.SaveChanges() [linha 132]                                        │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de contrato no banco de dados.



        // 📥 PARÂMETROS:
        // RepactuacaoContrato [RepactuacaoContrato] - Entidade contendo os dados atualizados.

        // 📤 SAÍDAS: void

        // 📝 OBSERVAÇÕES: Salva mudanças imediatamente no banco de dados

        // Param RepactuacaoContrato: Entidade <see cref="RepactuacaoContrato"/> com dados atualizados.
        public new void Update(RepactuacaoContrato RepactuacaoContrato)
            {
            try
            {
                // [VALIDACAO] Verificar se entidade não é nula
                if (RepactuacaoContrato == null)
                    throw new ArgumentNullException(nameof(RepactuacaoContrato));

                // [DB] Buscar registro existente
                var objFromDb = _db.RepactuacaoContrato.FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);

                // [DB] Atualizar e persistir mudanças
                _db.Update(RepactuacaoContrato);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                throw;
            }
            }


        }
    }
