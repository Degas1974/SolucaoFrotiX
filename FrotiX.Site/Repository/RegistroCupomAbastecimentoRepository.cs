/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RegistroCupomAbastecimentoRepository.cs                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para registros de cupons de abastecimento digitalizados.                            ║
   ║    Armazena referências a arquivos PDF para auditoria e consulta.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RegistroCupomAbastecimentoRepository(FrotiXDbContext db)                                       ║
   ║    • GetRegistroCupomAbastecimentoListForDropDown()                                                 ║
   ║    • Update(RegistroCupomAbastecimento registroCupomAbastecimento)                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem é ordenada por DataRegistro e exibe o campo RegistroPDF.                              ║
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
    // │ 🎯 CLASSE: RegistroCupomAbastecimentoRepository                                                │
    // │ 📦 HERDA DE: Repository                                            │
    // │ 🔌 IMPLEMENTA: IRegistroCupomAbastecimentoRepository                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos registros de cupons de abastecimento digitalizados.
    // Mantém consultas para dropdowns e atualização de arquivos associados.
    
    public class RegistroCupomAbastecimentoRepository : Repository<RegistroCupomAbastecimento>, IRegistroCupomAbastecimentoRepository
        {
        private new readonly FrotiXDbContext _db;


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RegistroCupomAbastecimentoRepository                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork [UnitOfWork.cs:141]                                       │
        // │    ➡️ CHAMA       : base(db) [linha 64]                                                  │
        // │ 📦 DEPENDÊNCIAS  : FrotiXDbContext                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db [FrotiXDbContext] - Contexto do banco de dados da aplicação.


        // 📤 SAÍDAS: Instância inicializada do repositório

        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RegistroCupomAbastecimentoRepository(FrotiXDbContext db) : base(db)
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
        // │ ⚡ MÉTODO: GetRegistroCupomAbastecimentoListForDropDown                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers (dropdown) [linha ~100]                                 │
        // │    ➡️ CHAMA       : _db.RegistroCupomAbastecimento.OrderBy() [linha 106]                 │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Obter lista de registros de cupons para composição de dropdowns.
        // Ordena por data do registro e exibe o identificador do arquivo PDF.



        // 📥 PARÂMETROS: Nenhum

        // 📤 RETORNO:
        // IEnumerable<SelectListItem> - Itens prontos para seleção em UI.

        // 📝 OBSERVAÇÕES: Ordenação por DataRegistro para cronologia

        // Returns: Lista de itens de seleção para registros de cupons.
        public IEnumerable<SelectListItem> GetRegistroCupomAbastecimentoListForDropDown()
            {
            try
            {
                // [DB] Consulta registros, ordena por data e projeta para dropdown
                return _db.RegistroCupomAbastecimento
                    .OrderBy(o => o.DataRegistro)
                    .Select(i => new SelectListItem()
                        {
                        Text = i.RegistroPDF,
                        Value = i.RegistroCupomId.ToString()
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
        // │    ➡️ CHAMA       : _db.FirstOrDefault() [linha 130]                                     │
        // │                     _db.Update() [linha 132]                                             │
        // │                     _db.SaveChanges() [linha 133]                                        │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Atualizar os dados de um registro de cupom digitalizado no banco de dados.



        // 📥 PARÂMETROS:
        // registroCupomAbastecimento [RegistroCupomAbastecimento] - Entidade contendo os dados atualizados.

        // 📤 SAÍDAS: void

        // 📝 OBSERVAÇÕES: Salva mudanças imediatamente no banco de dados

        // Param registroCupomAbastecimento: Entidade <see cref="RegistroCupomAbastecimento"/> com dados atualizados.
        public new void Update(RegistroCupomAbastecimento registroCupomAbastecimento)
            {
            try
            {
                // [VALIDACAO] Verificar se entidade não é nula
                if (registroCupomAbastecimento == null)
                    throw new ArgumentNullException(nameof(registroCupomAbastecimento));

                // [DB] Buscar registro existente
                var objFromDb = _db.RegistroCupomAbastecimento.FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);

                // [DB] Atualizar e persistir mudanças
                _db.Update(registroCupomAbastecimento);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                throw;
            }
            }


        }
    }
