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
    /// ║ REPOSITORY: EmpenhoMultaRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia empenhos orçamentários específicos para pagamento de multas.
    /// ║    Vincula notas de empenho ao órgão autuante (DETRAN, PRF, Guarda, etc.).
    /// ║    Controle orçamentário separado para despesas com multas de trânsito.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - EmpenhoMulta (empenhos para multas).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - OrgaoAutuante (N:1) - Órgão que aplicou a multa.
    /// ║    - MovimentacaoEmpenhoMulta (1:N) - Movimentações do empenho.
    /// ║    - Multa (1:N) - Multas pagas com este empenho.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetEmpenhoMultaListForDropDown() - Lista empenhos para dropdown.
    /// ║    2. Update() - Atualiza empenho com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Join com OrgaoAutuante para exibir Sigla e Nome no dropdown.
    /// ║    - Formato: "NotaEmpenho (Sigla/Nome)" ex: "2026NE000123 (DETRAN/DF)".
    /// ║    - Ordenação por NotaEmpenho (numérico crescente).
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Multa: Vincula multas a empenhos específicos.
    /// ║    - OrgaoAutuante: Identifica órgão responsável pela autuação.
    /// ║    - Relatórios: Controle de gastos com multas por órgão.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class EmpenhoMultaRepository : Repository<EmpenhoMulta>, IEmpenhoMultaRepository
    {
        private new readonly FrotiXDbContext _db;

        public EmpenhoMultaRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetEmpenhoMultaListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de empenhos de multa formatada para dropdown.
        /// │    JOIN com OrgaoAutuante para exibir Sigla e Nome junto com NotaEmpenho.
        /// │    SelectListItem: Text = "NotaEmpenho (Sigla/Nome)",
        /// │                    Value = EmpenhoMultaId.
        /// │
        /// │ JOIN:
        /// │    EmpenhoMulta → OrgaoAutuante (OrgaoAutuanteId).
        /// │    Para exibir "2026NE000123 (DETRAN/DF - Departamento de Trânsito)".
        /// │
        /// │ FORMATO TEXTO:
        /// │    "NotaEmpenho (Sigla/Nome)"
        /// │    Exemplo: "2026NE000123 (DETRAN/Departamento de Trânsito do DF)"
        /// │
        /// │ ORDENAÇÃO:
        /// │    NotaEmpenho ASC (ordem numérica crescente).
        /// │
        /// │ USO:
        /// │    - Multa: Vincular multa a empenho específico.
        /// │    - Relatórios: Seleção de empenho para análise de gastos.
        /// │    - MovimentacaoEmpenhoMulta: Registrar movimentações orçamentárias.
        /// │
        /// │ EXEMPLO RESULTADO:
        /// │    Text: "2026NE000123 (DETRAN/Departamento de Trânsito do DF)"
        /// │    Value: "a3b1c2d4-..."
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (empenhos com órgão autuante).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetEmpenhoMultaListForDropDown()
        {
            try
            {
                // [QUERY JOIN] - Empenhos com OrgaoAutuante
                return _db.EmpenhoMulta
                    .Join(_db.OrgaoAutuante,
                        empenhomulta => empenhomulta.OrgaoAutuanteId,
                        orgaoautuante => orgaoautuante.OrgaoAutuanteId,
                        (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
                    .OrderBy(o => o.empenhomulta.NotaEmpenho)  // Ordem numérica
                    .Select(i => new SelectListItem()
                    {
                        // [FORMATO] - "NotaEmpenho (Sigla/Nome)"
                        Text = i.empenhomulta.NotaEmpenho + " (" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
                        Value = i.empenhomulta.EmpenhoMultaId.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoMultaRepository.cs", "GetEmpenhoMultaListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Empenho Multa)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro EmpenhoMulta com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(empenhomulta) marca entidade como Modified.
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
        /// │    - Admin: Atualizar dados do empenho (valor, data, status).
        /// │    - Orçamento: Corrigir informações de empenho após auditoria.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(EmpenhoMulta empenhomulta)
        {
            try
            {
                // [QUERY TRACKING] - Busca empenho (NÃO USADO - código morto)
                var objFromDb = _db.EmpenhoMulta.AsTracking().FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(empenhomulta);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoMultaRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


