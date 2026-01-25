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
    /// ║ REPOSITORY: EmpenhoRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia empenhos orçamentários vinculados a contratos.
    /// ║    Controle de notas de empenho para pagamento de contratos administrativos.
    /// ║    Sistema de gestão orçamentária integrado com contratos.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - Empenho (empenhos orçamentários).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - Contrato (N:1) - Contrato vinculado ao empenho.
    /// ║    - MovimentacaoEmpenho (1:N) - Movimentações do empenho.
    /// ║    - NotaFiscal (1:N) - Notas fiscais pagas com este empenho.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetEmpenhoListForDropDown() - Lista empenhos para dropdown.
    /// ║    2. Update() - Atualiza empenho com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Join com Contrato para exibir AnoContrato/NumeroContrato no dropdown.
    /// ║    - Formato: "NotaEmpenho (Ano/Numero)" ex: "2026NE000123 (2026/001)".
    /// ║    - PROBLEMA: Value = ContratoId (deveria ser EmpenhoId!).
    /// ║    - Ordenação por NotaEmpenho (numérico crescente).
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Contrato: Vincula empenhos a contratos específicos.
    /// ║    - NotaFiscal: Associa notas fiscais a empenhos.
    /// ║    - Relatórios: Controle de execução orçamentária.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class EmpenhoRepository : Repository<Empenho>, IEmpenhoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EmpenhoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetEmpenhoListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de empenhos formatada para dropdown.
        /// │    JOIN com Contrato para exibir AnoContrato e NumeroContrato junto com NotaEmpenho.
        /// │    SelectListItem: Text = "NotaEmpenho (Ano/Numero)",
        /// │                    Value = ContratoId.
        /// │
        /// │ PROBLEMA CRÍTICO IDENTIFICADO:
        /// │    ❌ Value = ContratoId (DEVERIA SER EmpenhoId!).
        /// │    Dropdown retorna ID do contrato, não do empenho.
        /// │    Isso causa erro ao tentar usar o Value para buscar o empenho.
        /// │
        /// │ CORREÇÃO NECESSÁRIA:
        /// │    Value = i.empenho.EmpenhoId.ToString()  // ✅ Correto
        /// │
        /// │ JOIN:
        /// │    Empenho → Contrato (ContratoId).
        /// │    Para exibir "2026NE000123 (2026/001)".
        /// │
        /// │ FORMATO TEXTO:
        /// │    "NotaEmpenho (AnoContrato/NumeroContrato)"
        /// │    Exemplo: "2026NE000123 (2026/001)"
        /// │
        /// │ ORDENAÇÃO:
        /// │    NotaEmpenho ASC (ordem numérica crescente).
        /// │
        /// │ USO:
        /// │    - NotaFiscal: Vincular nota fiscal a empenho específico.
        /// │    - Relatórios: Seleção de empenho para análise orçamentária.
        /// │    - MovimentacaoEmpenho: Registrar movimentações.
        /// │
        /// │ EXEMPLO RESULTADO:
        /// │    Text: "2026NE000123 (2026/001)"
        /// │    Value: "a3b1c2d4-..." ❌ ATENÇÃO: Retorna ContratoId, não EmpenhoId!
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (empenhos com contrato).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetEmpenhoListForDropDown()
        {
            try
            {
                // [QUERY JOIN] - Empenhos com Contrato
                return _db.Empenho
                    .Join(_db.Contrato,
                        empenho => empenho.ContratoId,
                        contrato => contrato.ContratoId,
                        (empenho, contrato) => new { empenho, contrato })
                    .OrderBy(o => o.empenho.NotaEmpenho)  // Ordem numérica
                    .Select(i => new SelectListItem()
                    {
                        // [FORMATO] - "NotaEmpenho (Ano/Numero)"
                        Text = i.empenho.NotaEmpenho + " (" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                        // ❌ PROBLEMA: Retorna ContratoId, não EmpenhoId!
                        Value = i.contrato.ContratoId.ToString()  // DEVERIA SER: i.empenho.EmpenhoId.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoRepository.cs", "GetEmpenhoListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Empenho)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro Empenho com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(empenho) marca entidade como Modified.
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
        /// │    - Admin: Atualizar dados do empenho (valor, data, saldo).
        /// │    - Orçamento: Corrigir informações após auditoria.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(Empenho empenho)
        {
            try
            {
                // [QUERY TRACKING] - Busca empenho (NÃO USADO - código morto)
                var objFromDb = _db.Empenho.AsTracking().FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(empenho);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EmpenhoRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


