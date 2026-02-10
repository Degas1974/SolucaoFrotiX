/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: ListaEmpenhosMulta.cshtml.cs (Pages/Multa)                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para listagem de Empenhos de Multa (recursos orçamentários alocados para pagamento            ║
 * ║ de multas de trânsito). Inclui classe auxiliar para popular dropdown de órgãos autuantes.               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                    ║
 * ║ • _unitOfWork : Referência estática ao repositório                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • Initialize(unitOfWork) : Método estático para inicialização                                            ║
 * ║ • OnGet() : Handler vazio - dados carregados via Grid                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ CLASSES AUXILIARES                                                                                        ║
 * ║ • ListaOrgaoAutuante : Classe para popular dropdown de órgãos autuantes                                  ║
 * ║   - Id (Guid), Descricao (Nome + Sigla)                                                                  ║
 * ║   - OrgaoAutuanteList() : Retorna lista ordenada por Nome                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • OrgaoAutuante - Entidade (DETRAN, PRF, PM, etc.)                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Pages.Multa
{
    public class ListaEmpenhosMultaModel :PageModel
    {
        public static IUnitOfWork _unitOfWork;

        public ListaEmpenhosMultaModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml.cs" , "ListaEmpenhosMultaModel" , error);
            }
        }

        public static void Initialize(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml.cs" , "Initialize" , error);
                return;
            }
        }

        public void OnGet()
        {
            try
            {
                // Método vazio - lógica está no Initialize
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }

    public class ListaOrgaoAutuante
    {
        public string Descricao
        {
            get; set;
        }
        public Guid Id
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaOrgaoAutuante()
        {
            try
            {
                // Construtor vazio
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml.cs" , "ListaOrgaoAutuante" , error);
            }
        }

        public ListaOrgaoAutuante(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml.cs" , "ListaOrgaoAutuante" , error);
            }
        }

        public List<ListaOrgaoAutuante> OrgaoAutuanteList()
        {
            try
            {
                List<ListaOrgaoAutuante> orgaosautuantes = new List<ListaOrgaoAutuante>();

                var result = _unitOfWork.OrgaoAutuante.GetAll().OrderBy(n => n.Nome);

                foreach (var orgao in result)
                {
                    orgaosautuantes.Add(new ListaOrgaoAutuante
                    {
                        Descricao = orgao.Nome + " (" + orgao.Sigla + ")" ,
                        Id = orgao.OrgaoAutuanteId
                    });
                }

                return orgaosautuantes;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml.cs" , "OrgaoAutuanteList" , error);
                return default(List<ListaOrgaoAutuante>);
            }
        }
    }
}
