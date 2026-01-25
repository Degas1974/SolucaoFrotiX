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
    /// ║ REPOSITORY: AtaRegistroPrecosRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia Atas de Registro de Preços (licitações/contratos).
    /// ║    Atas vinculam Fornecedores a preços pré-definidos para produtos/serviços.
    /// ║    Sistema de compras públicas (pregões, licitações).
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - AtaRegistroPrecos (Atas de licitação).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - Fornecedor (N:1) - Fornecedor vencedor da licitação.
    /// ║    - AtaRegistroPrecosItem (1:N) - Itens/produtos da ata.
    /// ║    - OrdemCompra (1:N) - Ordens de compra geradas a partir da ata.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetAtaListForDropDown() - Lista atas para dropdown (com fornecedor).
    /// ║    2. Update() - Atualiza ata com SaveChanges imediato.
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Status: true (ata ativa), false (ata vencida/inativa).
    /// ║    - Formato dropdown: "AnoAta/NumeroAta - Fornecedor" (ex: "2026/001 - Posto ABC").
    /// ║    - Update() quebra padrão Unit of Work (SaveChanges direto).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - OrdemCompra: Gera pedidos baseados em atas ativas.
    /// ║    - Fornecedor: Vincula fornecedor vencedor da licitação.
    /// ║    - AtaRegistroPrecosItem: Itens negociados na ata.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class AtaRegistroPrecosRepository : Repository<AtaRegistroPrecos>, IAtaRegistroPrecosRepository
    {
        private new readonly FrotiXDbContext _db;

        public AtaRegistroPrecosRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetAtaListForDropDown (Lista para DropDown)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de Atas de Registro de Preços formatada para dropdown.
        /// │    JOIN com Fornecedor para exibir fornecedor junto com número da ata.
        /// │    SelectListItem: Text = "AnoAta/NumeroAta - Fornecedor", Value = AtaId.
        /// │
        /// │ PARÂMETROS:
        /// │    - status: int (0 = inativas, 1 = ativas).
        /// │      Convertido para bool: true (ativas), false (inativas).
        /// │
        /// │ JOIN:
        /// │    AtaRegistroPrecos → Fornecedor (FornecedorId).
        /// │    Para exibir "2026/001 - Posto ABC Ltda" no dropdown.
        /// │
        /// │ ORDENAÇÃO:
        /// │    Concatenação (AnoAta + "/" + NumeroAta + " - " + Fornecedor) DESC.
        /// │    Atas mais recentes primeiro.
        /// │
        /// │ USO:
        /// │    - OrdemCompra: Selecionar ata para gerar pedido.
        /// │    - Formulários: Dropdown de atas ativas.
        /// │    - ViewBag.Atas: Popular dropdown no Razor.
        /// │
        /// │ EXEMPLO RESULTADO:
        /// │    Text: "2026/001 - Posto Ipiranga"
        /// │    Value: "a3b1c2d4-..."
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<SelectListItem> (MVC SelectListItem).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IEnumerable<SelectListItem> GetAtaListForDropDown(int status)
        {
            try
            {
                // [QUERY JOIN] - Atas com Fornecedor, filtro por Status
                return _db.AtaRegistroPrecos
                    .Where(s => s.Status == Convert.ToBoolean(status)) // 0=false (inativas), 1=true (ativas)
                    .Join(_db.Fornecedor,
                        ataregistroprecos => ataregistroprecos.FornecedorId,
                        fornecedor => fornecedor.FornecedorId,
                        (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
                    .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
                    .Select(i => new SelectListItem()
                    {
                        Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
                        Value = i.ataregistroprecos.AtaId.ToString()
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosRepository.cs", "GetAtaListForDropDown", ex);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Ata)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro AtaRegistroPrecos com SaveChanges IMEDIATO.
        /// │    QUEBRA PADRÃO UNIT OF WORK (não aguarda UnitOfWork.SaveAsync()).
        /// │
        /// │ FLUXO:
        /// │    1. Busca objFromDb com AsTracking() (não usado depois).
        /// │    2. Update(ataRegistroPrecos) marca entidade como Modified.
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
        /// │    - Admin: Alterar Status, DataVigencia, Valores de ata.
        /// │    - Controllers: Atualizar dados da ata após edição.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(AtaRegistroPrecos ataRegistroPrecos)
        {
            try
            {
                // [QUERY TRACKING] - Busca ata (NÃO USADO - código morto)
                var objFromDb = _db.AtaRegistroPrecos.AsTracking().FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);

                // [UPDATE] - Marca entidade como Modified
                _db.Update(ataRegistroPrecos);

                // [SAVE] - QUEBRA UNIT OF WORK - Persiste imediatamente
                _db.SaveChanges(); // PROBLEMA: Transação independente
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AtaRegistroPrecosRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}


