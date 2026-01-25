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
    /// ║ REPOSITORY: ContratoRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia contratos administrativos (prestação de serviços, locação, etc.).
    /// ║    Contratos vinculam Fornecedores a serviços contratados (lavagem, manutenção,
    /// ║    terceirização, locação de veículos, etc.).
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - Contrato (contratos administrativos).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - Fornecedor (N:1) - Fornecedor contratado.
    /// ║    - VeiculoContrato (N:N) - Veículos vinculados ao contrato.
    /// ║    - MotoristaContrato (N:N) - Motoristas terceirizados.
    /// ║    - OperadorContrato (N:N) - Operadores terceirizados.
    /// ║    - ItemVeiculoContrato (1:N) - Itens/serviços do contrato.
    /// ║    - RepactuacaoContrato (1:N) - Repactuações contratuais.
    /// ║    - Empenho (1:N) - Empenhos orçamentários vinculados.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. GetDropDown() - Lista contratos para dropdown (filtrável por tipo).
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - Status: SEMPRE true (contratos ativos). Status false não são listados.
    /// ║    - TipoContrato: "Lavagem", "Manutencao", "Terceirizacao", "Locacao", etc.
    /// ║    - Formato dropdown: "AnoContrato/NumeroContrato - Fornecedor (TipoContrato)".
    /// ║    - Include NÃO é usado: EF Core converte nav props em JOIN automático.
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - Viagem: Vincular veículos contratados.
    /// ║    - Lavagem: Vincular contrato de lavagem.
    /// ║    - Manutencao: Vincular contrato de manutenção.
    /// ║    - Empenho: Vincular empenhos orçamentários.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class ContratoRepository : Repository<Contrato>, IContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public ContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetDropDown (Lista para DropDown com Filtro)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna lista de contratos ATIVOS formatada para dropdown.
        /// │    Permite filtrar por TipoContrato (opcional).
        /// │    SelectListItem: Text = "AnoContrato/NumeroContrato - Fornecedor (Tipo)",
        /// │                    Value = ContratoId.
        /// │
        /// │ PARÂMETROS:
        /// │    - tipoContrato: string? (opcional) - Filtra contratos por tipo.
        /// │      Se null/vazio: Lista TODOS os tipos.
        /// │      Se preenchido: Lista apenas contratos do tipo especificado.
        /// │      Exemplos: "Lavagem", "Manutencao", "Terceirizacao", "Locacao".
        /// │
        /// │ FILTROS:
        /// │    1. Status == true (SEMPRE - contratos ativos).
        /// │    2. TipoContrato == tipoContrato (SE tipoContrato informado).
        /// │
        /// │ ORDENAÇÃO:
        /// │    1. AnoContrato DESC (contratos mais recentes primeiro).
        /// │    2. NumeroContrato DESC (números maiores primeiro).
        /// │    3. Fornecedor.DescricaoFornecedor DESC.
        /// │
        /// │ FORMATO TEXTO:
        /// │    - COM filtro tipo: "2026/001 - Posto ABC Ltda"
        /// │    - SEM filtro tipo: "2026/001 - Posto ABC Ltda (Lavagem)"
        /// │
        /// │ NAVEGAÇÃO FORNECEDOR:
        /// │    EF Core converte 'c.Fornecedor.DescricaoFornecedor' em JOIN automático.
        /// │    Include() NÃO é necessário (otimização).
        /// │
        /// │ USO:
        /// │    - Lavagem: GetDropDown("Lavagem") - Lista contratos de lavagem.
        /// │    - Manutenção: GetDropDown("Manutencao") - Lista contratos de manutenção.
        /// │    - Geral: GetDropDown() - Lista TODOS os contratos (com tipo no texto).
        /// │
        /// │ EXEMPLO RESULTADO:
        /// │    COM filtro:
        /// │      Text: "2026/001 - Posto Ipiranga"
        /// │      Value: "a3b1c2d4-..."
        /// │    SEM filtro:
        /// │      Text: "2026/001 - Posto Ipiranga (Lavagem)"
        /// │      Value: "a3b1c2d4-..."
        /// │
        /// │ RETORNO:
        /// │    IQueryable<SelectListItem> (query não executada - lazy loading).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public IQueryable<SelectListItem> GetDropDown(string? tipoContrato = null)
        {
            try
            {
                // [VALIDAÇÃO] - Verifica se tipo de contrato foi informado
                var temTipo = !string.IsNullOrWhiteSpace(tipoContrato);

                // [QUERY] - Contratos ativos com filtro opcional de tipo
                return _db.Set<Contrato>()
                    .AsNoTracking() // Somente leitura (performance)
                    .Where(c => c.Status && (!temTipo || c.TipoContrato == tipoContrato))
                    // INCLUDE DESNECESSÁRIO: EF Core converte nav prop em JOIN automático
                    .OrderByDescending(c => c.AnoContrato)    // Mais recentes primeiro
                    .ThenByDescending(c => c.NumeroContrato)
                    .ThenByDescending(c => c.Fornecedor.DescricaoFornecedor) // JOIN implícito
                    .Select(c => new SelectListItem
                    {
                        Value = c.ContratoId.ToString(),
                        // [FORMATO TEXTO] - Se filtrado por tipo: Omite tipo. Se não: Exibe tipo.
                        Text = temTipo
                            ? $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor}"
                            : $"{c.AnoContrato}/{c.NumeroContrato} - {c.Fornecedor.DescricaoFornecedor} ({c.TipoContrato})"
                    });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ContratoRepository.cs", "GetDropDown", ex);
                return Enumerable.Empty<SelectListItem>().AsQueryable(); // Retorna query vazia
            }
        }
    }
}


