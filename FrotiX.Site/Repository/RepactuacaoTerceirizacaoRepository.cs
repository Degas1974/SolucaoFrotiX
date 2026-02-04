/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoTerceirizacaoRepository.cs                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de valores de terceirização.                                      ║
   ║    Gerencia reajustes de motoristas, operadores e encarregados em contratos.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoTerceirizacaoRepository(FrotiXDbContext db)                                         ║
   ║    • GetRepactuacaoTerceirizacaoListForDropDown()                                                  ║
   ║    • Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem usa ValorEncarregado como texto e RepactuacaoContratoId como identificador.          ║
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
    // │ 🎯 CLASSE: RepactuacaoTerceirizacaoRepository                                                  │
    // │ 📦 HERDA DE: Repository                                              │
    // │ 🔌 IMPLEMENTA: IRepactuacaoTerceirizacaoRepository                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de terceirização.
    // Centraliza listagens para UI e atualização de registros.
    
    public class RepactuacaoTerceirizacaoRepository : Repository<RepactuacaoTerceirizacao>, IRepactuacaoTerceirizacaoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoTerceirizacaoRepository                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork [UnitOfWork.cs:138]                                        │
        // │    ➡️ CHAMA       : base(db) [linha ~62]                                                 │
        // │ 📦 DEPENDÊNCIAS  : FrotiXDbContext                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        

        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db [FrotiXDbContext] - Contexto do banco de dados da aplicação.


        // 📤 SAÍDAS: Instância inicializada do repositório

        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoTerceirizacaoRepository(FrotiXDbContext db) : base(db)
            {
            try
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "RepactuacaoTerceirizacaoRepository", erro);
                throw;
            }
            }


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoTerceirizacaoListForDropDown                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers (dropdown) [linha ~100]                                 │
        // │    ➡️ CHAMA       : _db.RepactuacaoTerceirizacao.Select() [linha 108]                    │
        // │ 📦 DEPENDÊNCIAS  : _db (DbContext)                                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Obter lista de repactuações de terceirização para composição de dropdowns.
        // Exibe o valor do encarregado e usa o vínculo do contrato como chave.



        // 📥 PARÂMETROS: Nenhum

        // 📤 RETORNO:
        // IEnumerable<SelectListItem> - Itens prontos para seleção em UI.

        // 📝 OBSERVAÇÕES: Valor exibido é do encarregado

        // Returns: Lista de itens de seleção para repactuações de terceirização.
        public IEnumerable<SelectListItem> GetRepactuacaoTerceirizacaoListForDropDown()
            {
            try
            {
                // [DB] Projeta terceirizações para dropdown com valor encarregado
                return _db.RepactuacaoTerceirizacao
                    .Select(i => new SelectListItem()
                        {
                        Text = i.ValorEncarregado.ToString(),
                        Value = i.RepactuacaoContratoId.ToString()
                        });
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "GetRepactuacaoTerceirizacaoListForDropDown", erro);
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
        // Atualizar os dados de uma repactuação de terceirização no banco de dados.



        // 📥 PARÂMETROS:
        // repactuacaoTerceirizacao [RepactuacaoTerceirizacao] - Entidade contendo os dados atualizados.

        // 📤 SAÍDAS: void

        // 📝 OBSERVAÇÕES: Salva mudanças imediatamente no banco de dados

        // Param repactuacaoTerceirizacao: Entidade <see cref="RepactuacaoTerceirizacao"/> com dados atualizados.
        public new void Update(RepactuacaoTerceirizacao repactuacaoTerceirizacao)
            {
            try
            {
                // [VALIDACAO] Verificar se entidade não é nula
                if (repactuacaoTerceirizacao == null)
                    throw new ArgumentNullException(nameof(repactuacaoTerceirizacao));

                // [DB] Buscar registro existente
                var objFromDb = _db.RepactuacaoTerceirizacao.FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == repactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);

                // [DB] Atualizar e persistir mudanças
                _db.Update(repactuacaoTerceirizacao);
                _db.SaveChanges();
            }
            catch (Exception erro)
            {
                Alerta.TratamentoErroComLinha("RepactuacaoTerceirizacaoRepository.cs", "Update", erro);
                throw;
            }
            }


        }
    }
