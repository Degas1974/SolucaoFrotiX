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
    /// ║ REPOSITORY: CorridasCanceladasTaxiLegRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia corridas de táxi canceladas (sistema legado/histórico).
    /// ║    Armazena histórico de cancelamentos com motivo e detalhes.
    /// ║    Integração com sistema TaxiLeg (Câmara Legislativa).
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - CorridasCanceladasTaxiLeg (corridas canceladas - legado).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - CorridasTaxiLeg (1:1 opcional) - Corrida original que foi cancelada.
    /// ║    - AspNetUsers (N:1 opcional) - Usuário que cancelou.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetCorridasCanceladasTaxiLegListForDropDown() - Lista para dropdown.
    /// ║    2. Update() - Atualiza corrida cancelada com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Sistema LEGADO (TaxiLeg - possivelmente descontinuado).
    /// ║    - Dropdown exibe MotivoCancelamento (pode ser longo/não ideal).
    /// ║    - Sem filtros de data ou status.
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - TaxiLeg: Sistema legado de gestão de corridas de táxi.
    /// ║    - Relatórios: Análise de cancelamentos e motivos.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class CorridasCanceladasTaxiLegRepository : Repository<CorridasCanceladasTaxiLeg>, ICorridasCanceladasTaxiLegRepository
    {
        private new readonly FrotiXDbContext _db;

        public CorridasCanceladasTaxiLegRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetCorridasCanceladasTaxiLegListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de corridas canceladas formatada para dropdown.
        /// │    SelectListItem: Text = MotivoCancelamento, Value = CorridaCanceladaId.
        /// │
        /// │ OBSERVAÇÃO:
        /// │    - Text = MotivoCancelamento (pode ser texto longo).
        /// │    - Não há filtro de data ou ordenação.
        /// │    - Sem JOIN com CorridasTaxiLeg (não mostra dados da corrida original).
        /// │
        /// │ SUGESTÃO DE MELHORIA:
        /// │    Incluir Data + Resumo curto do motivo:
        /// │    Text = $"{DataCancelamento:dd/MM/yyyy} - {MotivoCancelamento.Substring(0, 30)}..."
        /// │
        /// │ USO:
        /// │    - Relatórios: Seleção de corrida cancelada para análise.
        /// │    - Admin: Consulta de histórico de cancelamentos.
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (todas as corridas canceladas).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetCorridasCanceladasTaxiLegListForDropDown()
        {
            try
            {
                // [QUERY] - Lista corridas canceladas (sem filtros ou ordenação)
                return _db.CorridasCanceladasTaxiLeg
                    .Select(i => new SelectListItem()
                    {
                        Text = i.MotivoCancelamento,  // Pode ser texto longo
                        Value = i.CorridaCanceladaId.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegCanceladasRepository.cs", "GetCorridasCanceladasTaxiLegListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Corrida Cancelada)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro CorridasCanceladasTaxiLeg com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(corridasCanceladasTaxiLeg) marca entidade como Modified.
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
        /// │    - Admin: Corrigir dados de cancelamento (motivo, data, etc.).
        /// │    - Integração: Atualizar status de sincronização com TaxiLeg.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(CorridasCanceladasTaxiLeg corridasCanceladasTaxiLeg)
        {
            try
            {
                // [QUERY TRACKING] - Busca corrida cancelada (NÃO USADO - código morto)
                var objFromDb = _db.CorridasCanceladasTaxiLeg.AsTracking().FirstOrDefault(s => s.CorridaCanceladaId == corridasCanceladasTaxiLeg.CorridaCanceladaId);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(corridasCanceladasTaxiLeg);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegCanceladasRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


