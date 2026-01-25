using Microsoft.EntityFrameworkCore;
using System;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Helpers;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════
    /// ║ REPOSITORY: EncarregadoContratoRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia vinculação N:N entre Encarregado e Contrato.
    /// ║    Define quais encarregados estão responsáveis por quais contratos.
    /// ║    Tabela intermediária para relacionamento muitos-para-muitos.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - EncarregadoContrato (tabela de junção/pivot table).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - Encarregado (N:1) - Encarregado responsável.
    /// ║    - Contrato (N:1) - Contrato sob responsabilidade.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. Update() - Atualiza vinculação com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Repositório SIMPLES (apenas Update).
    /// ║    - Não há métodos de consulta customizados (usa base Repository<T>).
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║    - Chave composta: EncarregadoId + ContratoId.
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Contrato: Define responsáveis pelo contrato.
    /// ║    - Encarregado: Lista contratos sob responsabilidade.
    /// ║    - Relatórios: Análise de carga de trabalho por encarregado.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class EncarregadoContratoRepository : Repository<EncarregadoContrato>, IEncarregadoContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EncarregadoContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Vinculação Encarregado-Contrato)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro EncarregadoContrato com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Update(encarregadoContrato) marca entidade como Modified.
        /// │    2. SaveChanges() persiste imediatamente.
        /// │
        /// │ DIFERENÇA:
        /// │    Este Update NÃO busca objFromDb antes (diferente dos outros).
        /// │    Mais eficiente (evita query desnecessária).
        /// │
        /// │ PROBLEMA:
        /// │    - Transação independente (não participa da transação do UnitOfWork).
        /// │    - Se houver erro depois, esta alteração NÃO será revertida.
        /// │
        /// │ SUGESTÃO DE REFATORAÇÃO:
        /// │    - Remover SaveChanges() daqui.
        /// │    - Deixar UnitOfWork.SaveAsync() fazer o commit transacional.
        /// │
        /// │ USO:
        /// │    - Admin: Alterar encarregado responsável por contrato.
        /// │    - RH: Redistribuir contratos entre encarregados.
        /// │    - Auditoria: Corrigir responsabilidades após revisão.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(EncarregadoContrato encarregadoContrato)
        {
            try
            {
                // [UPDATE] - Marca entidade como Modified (sem busca prévia)
                _db.EncarregadoContrato.Update(encarregadoContrato);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EncarregadoContratoRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}
