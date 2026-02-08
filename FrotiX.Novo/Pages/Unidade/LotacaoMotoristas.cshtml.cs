/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: LotacaoMotoristas.cshtml.cs (Pages/Unidade)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para gestão de lotação de motoristas nas unidades. Permite transferir motoristas entre         ║
 * ║ unidades com registro do motivo da mudança.                                                              ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES ESTÁTICAS                                                                                   ║
 * ║ • _unitOfWork : IUnitOfWork - Acesso ao repositório (inicializado via Initialize ou construtor)          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Método vazio (dados carregados via AJAX)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ CLASSES AUXILIARES                                                                                       ║
 * ║ • ListaUnidades : UnidadeId (Guid), Descricao - Lista de unidades para dropdown                          ║
 * ║ • ListaMudancas : MudancaId (string), Descricao - Motivos de transferência:                              ║
 * ║   - Férias, Cobertura, Retorno, Devolução, À Pedido, Lotação Inicial, Desligamento                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                             ║
 * ║ • IUnitOfWork (Unidade)                                                                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Pages.Unidade
{
    public class LotacaoMotoristas :PageModel
    {
        public static IUnitOfWork _unitOfWork;

        public static void Initialize(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "Initialize" , error);
                return;
            }
        }

        public LotacaoMotoristas(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "LotacaoMotoristas" , error);
            }
        }

        public void OnGet()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }

    public class ListaUnidades
    {
        public Guid UnidadeId
        {
            get; set;
        }
        public string Descricao
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaUnidades()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "ListaUnidades" , error);
            }
        }

        public ListaUnidades(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "ListaUnidades" , error);
            }
        }

        public List<ListaUnidades> UnidadesList()
        {
            try
            {
                List<ListaUnidades> unidades = new List<ListaUnidades>();

                var result = _unitOfWork.Unidade.GetAll().OrderBy(u => u.Descricao);

                foreach (var unidade in result)
                {
                    unidades.Add(
                        new ListaUnidades
                        {
                            Descricao = unidade.Descricao ,
                            UnidadeId = unidade.UnidadeId ,
                        }
                    );
                }

                return unidades;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "UnidadesList" , error);
                return default(List<ListaUnidades>);
            }
        }
    }

    public class ListaMudancas
    {
        public string MudancaId
        {
            get; set;
        }
        public string Descricao
        {
            get; set;
        }

        private readonly IUnitOfWork _unitOfWork;

        public ListaMudancas()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "ListaMudancas" , error);
            }
        }

        public ListaMudancas(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "ListaMudancas" , error);
            }
        }

        public List<ListaMudancas> MudançasList()
        {
            try
            {
                List<ListaMudancas> mudancas = new List<ListaMudancas>();

                mudancas.Add(new ListaMudancas { Descricao = "Férias" , MudancaId = "Férias" });
                mudancas.Add(new ListaMudancas { Descricao = "Cobertura" , MudancaId = "Cobertura" });
                mudancas.Add(new ListaMudancas { Descricao = "Retorno" , MudancaId = "Retorno" });
                mudancas.Add(new ListaMudancas { Descricao = "Devolução" , MudancaId = "Devolução" });
                mudancas.Add(new ListaMudancas { Descricao = "À Pedido" , MudancaId = "À Pedido" });
                mudancas.Add(new ListaMudancas { Descricao = "Lotação Inicial" , MudancaId = "Lotação Inicial" });
                mudancas.Add(new ListaMudancas { Descricao = "Desligamento" , MudancaId = "Desligamento" });

                return mudancas;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml.cs" , "MudançasList" , error);
                return default(List<ListaMudancas>);
            }
        }
    }
}
