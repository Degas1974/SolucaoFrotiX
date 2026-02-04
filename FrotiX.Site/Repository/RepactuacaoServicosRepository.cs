/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoServicosRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de serviços em contratos administrativos.                         ║
   ║    Gerencia reajustes de serviços como manutenção, lavagem e similares.                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoServicosRepository(FrotiXDbContext db)                                              ║
   ║    • GetRepactuacaoServicosListForDropDown()                                                       ║
   ║    • Update(RepactuacaoServicos repactuacaoServicos)                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem usa Valor como texto e RepactuacaoContratoId como identificador.                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Helpers;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: RepactuacaoServicosRepository                                                       │
    // │ 📦 HERDA DE: Repository                                                   │
    // │ 🔌 IMPLEMENTA: IRepactuacaoServicosRepository                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de serviços em contratos.
    // Centraliza listagens para UI e atualização de registros.
    
    public class RepactuacaoServicosRepository : Repository<RepactuacaoServicos>, IRepactuacaoServicosRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoServicosRepository                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork [UnitOfWork.cs:139]                                        │
        // │    ➡️ CHAMA       : base(db) [linha ~62]                                                 │
        // │ 📦 DEPENDÊNCIAS  : FrotiXDbContext                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        

        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db [FrotiXDbContext] - Contexto do banco de dados da aplicação.


        // 📤 SAÍDAS: Instância inicializada do repositório

        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoServicosRepository(FrotiXDbContext db) : base(db)
            {
            try
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "RepactuacaoServicosRepository", erro);
                throw;
            }
            }


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoServicosListForDropDown                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers (dropdown) [linha ~100]                                 │
        // │    ➡️ CHAMA       : _db.RepactuacaoServicos.Select() [linha 102]                         │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Obter lista de repactuações de serviços para composição de dropdowns.
        // Exibe o valor da repactuação e usa o vínculo do contrato como chave.



        // 📥 PARÂMETROS: Nenhum

        // 📤 RETORNO:
        // IEnumerable<SelectListItem> - Itens prontos para seleção em UI.

        // 📝 OBSERVAÇÕES: Exibe valor numérico como texto

        // Returns: Lista de itens de seleção para repactuações de serviços.
        public IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown()
            {
            try
            {
                // [DB] Projeta repactuações de serviços para dropdown com valor e contrato
                return _db.RepactuacaoServicos
                    .Select(i => new SelectListItem()
                        {
                        Text = i.Valor.ToString(),
                        Value = i.RepactuacaoContratoId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "GetRepactuacaoServicosListForDropDown", erro);
                throw;
            }
            }


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers [linha ~100]                                             │
        // │    ➡️ CHAMA       : _db.FirstOrDefault() [linha 138]                                     │
        // │                     _db.Update() [linha 140]                                             │
        // │                     _db.SaveChanges() [linha 141]                                        │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de serviços no banco de dados.



        // 📥 PARÂMETROS:
        // repactuacaoServicos [RepactuacaoServicos] - Entidade contendo os dados atualizados.

        // 📤 SAÍDAS: void

        // 📝 OBSERVAÇÕES: Salva mudanças imediatamente no banco de dados

        // Param repactuacaoServicos: Entidade <see cref="RepactuacaoServicos"/> com dados atualizados.
        public new void Update(RepactuacaoServicos repactuacaoServicos)
            {
            try
            {
                // [VALIDACAO] Verificar se entidade não é nula
                if (repactuacaoServicos == null)
                    throw new ArgumentNullException(nameof(repactuacaoServicos));

                // [DB] Buscar registro existente
                var objFromDb = _db.RepactuacaoServicos.FirstOrDefault(s => s.RepactuacaoServicoId == repactuacaoServicos.RepactuacaoServicoId);

                // [DB] Atualizar e persistir mudanças
                _db.Update(repactuacaoServicos);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoServicosRepository.cs", "Update", erro);
                throw;
            }
            }


        }
    }
