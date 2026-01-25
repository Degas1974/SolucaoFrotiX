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
    /// ║ REPOSITORY: CorridasTaxiLegRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia corridas de táxi do sistema legado (TaxiLeg).
    /// ║    Histórico de corridas realizadas pela Câmara Legislativa.
    /// ║    Sistema legado mantido para consultas históricas.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - CorridasTaxiLeg (corridas de táxi - legado).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - CorridasCanceladasTaxiLeg (1:1 opcional) - Cancelamento da corrida.
    /// ║    - Unidade (N:1 opcional) - Unidade solicitante.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetCorridasTaxiLegListForDropDown() - Lista para dropdown.
    /// ║    2. Update() - Atualiza corrida com SaveChanges imediato.
    /// ║    3. ExisteCorridaNoMesAno() - Verifica existência de corridas em mês/ano.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Sistema LEGADO (TaxiLeg - possivelmente descontinuado).
    /// ║    - Dropdown exibe DescUnidade (descrição da unidade solicitante).
    /// ║    - ExisteCorridaNoMesAno() útil para relatórios mensais.
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - TaxiLeg: Sistema legado de gestão de corridas de táxi.
    /// ║    - Relatórios: Consultas históricas e análises de custo.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class CorridasTaxiLegRepository : Repository<CorridasTaxiLeg>, ICorridasTaxiLegRepository
    {
        private new readonly FrotiXDbContext _db;

        public CorridasTaxiLegRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetCorridasTaxiLegListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de corridas formatada para dropdown.
        /// │    SelectListItem: Text = DescUnidade (unidade solicitante),
        /// │                    Value = CorridaId.
        /// │
        /// │ OBSERVAÇÃO:
        /// │    - Text = DescUnidade (apenas unidade, não mostra data ou destino).
        /// │    - Não há filtro de data ou ordenação.
        /// │    - Pode retornar muitas corridas (sem paginação).
        /// │
        /// │ SUGESTÃO DE MELHORIA:
        /// │    Incluir Data + Destino para melhor identificação:
        /// │    Text = $"{DataAgenda:dd/MM/yyyy} - {DescUnidade} → {Destino}"
        /// │
        /// │ USO:
        /// │    - Relatórios: Seleção de corrida para análise.
        /// │    - Admin: Consulta de histórico de corridas.
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (todas as corridas).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetCorridasTaxiLegListForDropDown()
        {
            try
            {
                // [QUERY] - Lista corridas (sem filtros, ordenação ou paginação)
                return _db.CorridasTaxiLeg.Select(i => new SelectListItem()
                {
                    Text = i.DescUnidade,  // Apenas unidade (não mostra data/destino)
                    Value = i.CorridaId.ToString(),
                });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "GetCorridasTaxiLegListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Corrida)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro CorridasTaxiLeg com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(corridasTaxiLeg) marca entidade como Modified.
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
        /// │    - Admin: Corrigir dados de corrida (valor, data, destino, etc.).
        /// │    - Integração: Atualizar status de sincronização com TaxiLeg.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(CorridasTaxiLeg corridasTaxiLeg)
        {
            try
            {
                // [QUERY TRACKING] - Busca corrida (NÃO USADO - código morto)
                var objFromDb = _db.CorridasTaxiLeg.AsTracking().FirstOrDefault(s =>
                    s.CorridaId == corridasTaxiLeg.CorridaId
                );

                // [UPDATE] - Marca entidade como Modified
                _db.Update(corridasTaxiLeg);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: ExisteCorridaNoMesAno (Verificar Existência de Corridas)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Verifica se existe PELO MENOS uma corrida em determinado mês/ano.
        /// │    Usa DataAgenda (data agendada da corrida).
        /// │
        /// │ PARÂMETROS:
        /// │    - ano: int (ano a verificar, ex: 2026).
        /// │    - mes: int (mês a verificar, 1-12).
        /// │
        /// │ FILTROS:
        /// │    1. DataAgenda.HasValue (ignora corridas sem data agendada).
        /// │    2. DataAgenda.Year == ano.
        /// │    3. DataAgenda.Month == mes.
        /// │
        /// │ USO:
        /// │    - Relatórios: Verificar se há dados para gerar relatório mensal.
        /// │    - Dashboard: Exibir indicador "✅ Corridas registradas em Janeiro/2026".
        /// │    - Validação: Antes de importar dados, verificar se mês já tem corridas.
        /// │
        /// │ PERFORMANCE:
        /// │    - Any() é eficiente (para na primeira ocorrência).
        /// │    - Considera adicionar índice em DataAgenda para otimização.
        /// │
        /// │ RETORNO:
        /// │    bool: true (existe pelo menos uma corrida) ou false (não existe).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public bool ExisteCorridaNoMesAno(int ano, int mes)
        {
            try
            {
                // [QUERY EXISTÊNCIA] - Verifica se há corridas no mês/ano (performance)
                return _db.CorridasTaxiLeg.Any(x =>
                    x.DataAgenda.HasValue
                    && x.DataAgenda.Value.Year == ano
                    && x.DataAgenda.Value.Month == mes
                );
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("CorridasTaxiLegRepository.cs", "ExisteCorridaNoMesAno", ex);
                return false; // Em caso de erro, assume que não existe
            }
        }
    }
}


