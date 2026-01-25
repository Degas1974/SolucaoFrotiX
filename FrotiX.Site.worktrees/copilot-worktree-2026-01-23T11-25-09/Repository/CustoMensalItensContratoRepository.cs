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
    /// ║ REPOSITORY: CustoMensalItensContratoRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia custos mensais de itens de contratos (análise financeira).
    /// ║    Consolidação de custos mensais por item de contrato e nota fiscal.
    /// ║    Usado para relatórios de despesas mensais e análise de custos.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - CustoMensalItensContrato (custos mensais consolidados).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - NotaFiscal (N:1) - Nota fiscal relacionada ao custo.
    /// ║    - ItemVeiculoContrato (N:1 opcional) - Item do contrato.
    /// ║    - Contrato (N:1 via ItemVeiculoContrato) - Contrato origem.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetCustoMensalItensContratoListForDropDown() - Lista para dropdown.
    /// ║    2. Update() - Atualiza custo mensal com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Chave composta: NotaFiscalId + Ano + Mes (tabela consolidada).
    /// ║    - Dropdown exibe apenas Ano (não mostra mês ou valor - pouco útil).
    /// ║    - Ordenação por Ano (sem filtro de contrato ou período).
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - NotaFiscal: Vincula custos a notas fiscais.
    /// ║    - Relatórios: Análise de custos mensais por contrato.
    /// ║    - Dashboard: Gráficos de evolução de custos.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class CustoMensalItensContratoRepository : Repository<CustoMensalItensContrato>, ICustoMensalItensContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public CustoMensalItensContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetCustoMensalItensContratoListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de custos mensais formatada para dropdown.
        /// │    SelectListItem: Text = Ano, Value = NotaFiscalId.
        /// │
        /// │ PROBLEMA IDENTIFICADO:
        /// │    ❌ Text = Ano (apenas ano, sem mês ou valor - não identifica registro).
        /// │    ❌ Podem existir múltiplos registros com mesmo Ano (duplicatas no dropdown).
        /// │    ❌ Sem JOIN com NotaFiscal (não mostra número da nota).
        /// │
        /// │ SUGESTÃO DE REFATORAÇÃO:
        /// │    return _db.CustoMensalItensContrato
        /// │        .Include(c => c.NotaFiscal)
        /// │        .OrderByDescending(o => o.Ano).ThenByDescending(o => o.Mes)
        /// │        .Select(i => new SelectListItem {
        /// │            Text = $"{i.Mes:00}/{i.Ano} - NF {i.NotaFiscal.Numero} - R$ {i.ValorTotal:N2}",
        /// │            Value = $"{i.NotaFiscalId}|{i.Ano}|{i.Mes}"  // Chave composta
        /// │        });
        /// │
        /// │ ORDENAÇÃO:
        /// │    Ano ASC (mais antigos primeiro - não ideal para dropdowns).
        /// │
        /// │ USO ATUAL:
        /// │    - Relatórios: Seleção de custo mensal (uso limitado devido ao problema).
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (apenas Ano - não user-friendly).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetCustoMensalItensContratoListForDropDown()
        {
            try
            {
                // [QUERY] - Lista custos mensais (apenas Ano - problema de usabilidade)
                return _db.CustoMensalItensContrato
                    .OrderBy(o => o.Ano)  // Mais antigos primeiro
                    .Select(i => new SelectListItem()
                    {
                        Text = i.Ano.ToString(),  // ❌ Apenas ano (pode haver duplicatas)
                        Value = i.NotaFiscalId.ToString()  // ❌ Value não inclui chave composta
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CustoMensalItensContratoRepository.cs", "GetCustoMensalItensContratoListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Custo Mensal)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro CustoMensalItensContrato com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ CHAVE COMPOSTA:
        /// │    Busca usa 3 campos: NotaFiscalId + Ano + Mes (chave primária composta).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() usando chave composta (não usado depois).
        /// │    2. Update(customensalitenscontrato) marca entidade como Modified.
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
        /// │    - Admin: Corrigir valores de custos mensais.
        /// │    - Fechamento: Atualizar custos após processamento de notas.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(CustoMensalItensContrato customensalitenscontrato)
        {
            try
            {
                // [QUERY TRACKING] - Busca custo mensal por chave composta (NÃO USADO - código morto)
                var objFromDb = _db.CustoMensalItensContrato.AsTracking().FirstOrDefault(s =>
                    (s.NotaFiscalId == customensalitenscontrato.NotaFiscalId) &&
                    (s.Ano == customensalitenscontrato.Ano) &&
                    (s.Mes == customensalitenscontrato.Mes));

                // [UPDATE] - Marca entidade como Modified
                _db.Update(customensalitenscontrato);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CustoMensalItensContratoRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


