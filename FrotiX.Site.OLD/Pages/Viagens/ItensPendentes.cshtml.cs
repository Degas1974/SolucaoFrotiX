/* ****************************************************************************************
 * ⚡ ARQUIVO: Pages/Viagens/ItensPendentes.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para listar viagens REALIZADAS com itens pendentes de devolução
 *                   (documentos, cartão de abastecimento, cinta, tablet, arla, cabo).
 * 📥 ENTRADAS     : GET request para rota /Viagens/ItensPendentes, método OnGet() chamado
 *                   pelo ASP.NET Core, acesso ao IUnitOfWork via injeção de dependência
 * 📤 SAÍDAS       : Propriedade ViagensComItensPendentes (IList<Viagem>) populada com viagens
 *                   filtradas, renderização de ItensPendentes.cshtml, includeProperties:
 *                   "Motorista,Veiculo" (navigation properties carregadas via EF Core)
 * 🔗 CHAMADA POR  : ASP.NET Core Razor Pages pipeline ao acessar /Viagens/ItensPendentes,
 *                   ItensPendentes.cshtml (view), sistema de alertas/notificações
 * 🔄 CHAMA        : _unitOfWork.Viagem.GetAll (query com filtros complexos), EF Core LINQ
 *                   (Where clauses com 6 condições OR para itens pendentes), ToList()
 * 📦 DEPENDÊNCIAS : IUnitOfWork (Repository pattern), FrotiX.Models.Viagem (entidade),
 *                   Microsoft.AspNetCore.Mvc.RazorPages (PageModel), Entity Framework Core
 *                   (includeProperties, navigation properties), System.Linq
 * 📝 OBSERVAÇÕES  : Filtro complexo: Status = "Realizada" AND ((DocumentoEntregue=true AND
 *                   DocumentoDevolvido=false) OR (CartaoAbastecimentoEntregue=true AND
 *                   CartaoAbastecimentoDevolvido=false) OR ... 7 tipos de itens verificados).
 *                   Sistema crítico para controle de patrimônio e itens emprestados aos motoristas.
 *                   Evita perda de equipamentos. 39 linhas com lógica de filtragem via LINQ/EF Core.
 *                   Navigation properties (Motorista, Veiculo) carregadas via includeProperties
 *                   para exibição na view sem N+1 queries. Query pode ser pesada se muitas viagens.
 **************************************************************************************** */

using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Pages.Viagens
{
    public class ItensPendentesModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItensPendentesModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Viagem> ViagensComItensPendentes { get;set; }

        public void OnGet()
        {
            ViagensComItensPendentes = _unitOfWork.Viagem.GetAll(
                v => v.Status == "Realizada" &&
                ((v.DocumentoEntregue == true && v.DocumentoDevolvido == false) ||
                (v.CartaoAbastecimentoEntregue == true && v.CartaoAbastecimentoDevolvido == false) ||
                (v.CintaEntregue == true && v.CintaDevolvida == false) ||
                (v.TabletEntregue == true && v.TabletDevolvido == false) ||
                (v.ArlaEntregue == true && v.ArlaDevolvido == false) ||
                (v.CaboEntregue == true && v.CaboDevolvido == false) ||
                (v.SuporteIntegro == true && v.SuporteDefeituoso == false)),
                includeProperties: "Motorista,Veiculo"
            ).ToList();
        }
    }
}
