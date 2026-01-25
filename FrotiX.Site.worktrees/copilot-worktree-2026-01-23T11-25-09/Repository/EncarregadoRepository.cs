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
    /// ║ REPOSITORY: EncarregadoRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia encarregados (responsáveis por contratos).
    /// ║    Encarregados são funcionários responsáveis pela fiscalização e
    /// ║    acompanhamento de contratos administrativos.
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - Encarregado (fiscais/responsáveis de contratos).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - EncarregadoContrato (1:N) - Contratos sob responsabilidade.
    /// ║    - AspNetUsers (1:1 opcional) - Usuário do sistema.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. Update() - Atualiza encarregado com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Repositório SIMPLES (apenas Update).
    /// ║    - Não há métodos de consulta customizados (usa base Repository<T>).
    /// ║    - Não há GetListForDropDown (provavelmente implementado em outro lugar).
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Contrato: Define responsáveis/fiscais.
    /// ║    - EncarregadoContrato: Vinculação N:N com contratos.
    /// ║    - Relatórios: Análise de responsabilidades e carga de trabalho.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class EncarregadoRepository : Repository<Encarregado>, IEncarregadoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EncarregadoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Encarregado)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro Encarregado com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Update(encarregado) marca entidade como Modified.
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
        /// │    - Admin: Atualizar dados do encarregado (nome, matrícula, email).
        /// │    - RH: Alterar status, cargo, setor do encarregado.
        /// │    - Auditoria: Corrigir informações após revisão.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(Encarregado encarregado)
        {
            try
            {
                // [UPDATE] - Marca entidade como Modified (sem busca prévia)
                _db.Encarregado.Update(encarregado);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EncarregadoRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}
