using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Helpers;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════
    /// ║ REPOSITORY: CombustivelRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia tipos de combustível (Gasolina, Etanol, Diesel, GNV, etc.).
    /// ║    Cadastro básico usado em Abastecimento e Veículo.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - Combustivel (tipos de combustível disponíveis).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - Abastecimento (1:N) - Abastecimentos realizados com este combustível.
    /// ║    - Veiculo (1:N) - Veículos que usam este combustível.
    /// ║    - MediaCombustivel (1:N) - Médias de consumo por combustível.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetCombustivelListForDropDown() - Lista combustíveis ativos para dropdown.
    /// ║    2. Update() - Atualiza combustível com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Status: true (ativo), false (inativo/desabilitado).
    /// ║    - Descricao: "Gasolina", "Etanol", "Diesel S10", "GNV", etc.
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Abastecimento: Registra tipo de combustível abastecido.
    /// ║    - Veiculo: Define combustível compatível com veículo.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class CombustivelRepository : Repository<Combustivel>, ICombustivelRepository
    {
        private new readonly FrotiXDbContext _db;

        public CombustivelRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetCombustivelListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de combustíveis ATIVOS formatada para dropdown.
        /// │    SelectListItem: Text = Descricao, Value = CombustivelId.
        /// │
        /// │ FILTRO:
        /// │    Status == true (combustíveis ativos/habilitados).
        /// │
        /// │ ORDENAÇÃO:
        /// │    Descricao ASC (ordem alfabética).
        /// │
        /// │ USO:
        /// │    - Abastecimento: Seleção de tipo de combustível.
        /// │    - Veículo: Definir combustível compatível.
        /// │    - ViewBag.Combustiveis: Popular dropdown no Razor.
        /// │
        /// │ EXEMPLO RESULTADO:
        /// │    Text: "Diesel S10"
        /// │    Value: "1"
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (MVC SelectListItem).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetCombustivelListForDropDown()
        {
            try
            {
                // [QUERY] - Lista combustíveis ativos ordenados por Descricao
                return _db.Combustivel
                    .Where(e => e.Status) // Ativos
                    .OrderBy(o => o.Descricao)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.Descricao,
                        Value = i.CombustivelId.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CombustivelRepository.cs", "GetCombustivelListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Combustível)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro Combustivel com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(combustivel) marca entidade como Modified.
        /// │    3. SaveChanges() persiste imediatamente.
        /// │
        /// │ PROBLEMA:
        /// │    - Transação independente (não participa da transação do UnitOfWork).
        /// │    - Se houver erro depois, esta alteração NÃO será revertida.
        /// │    - objFromDb é buscado mas não utilizado (código morto).
        /// │
        /// │ SUGESTÃO DE REFATORAÇÃO:
        /// │    - Remover SaveChanges() daqui.
        /// │    - Deixar UnitOfWork.SaveAsync() fazer o commit transacional.
        /// │    - Remover linha 'objFromDb' (não é usada).
        /// │
        /// │ USO:
        /// │    - Admin: Alterar Descricao, Status de combustível.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(Combustivel combustivel)
        {
            try
            {
                // [QUERY TRACKING] - Busca combustível (NÃO USADO - código morto)
                var objFromDb = _db.Combustivel.AsTracking().FirstOrDefault(s => s.CombustivelId == combustivel.CombustivelId);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(combustivel);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CombustivelRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


