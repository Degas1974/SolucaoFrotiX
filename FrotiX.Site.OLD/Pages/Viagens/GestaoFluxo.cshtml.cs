/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: GestaoFluxo.cshtml.cs (Pages/Viagens)                                                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para gestão de fluxo de veículos Economildo. Inclui classes helper especializadas             ║
 * ║ para listar apenas veículos e motoristas que participam do serviço Economildo (shuttle).                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ CLASSES                                                                                                   ║
 * ║ • GestaoFluxoModel : PageModel principal (static _unitOfWork para persistência)                         ║
 * ║ • ListaVeiculosMOB : Helper para listar veículos Economildo (Coletivos Pequenos/Ônibus)                 ║
 * ║ • ListaMotoristasMOB : Helper para listar motoristas que têm viagens Economildo                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - dados carregados via helpers                                                ║
 * ║ • Initialize(unitOfWork) : Inicializa UnitOfWork estático                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ FILTRO VEÍCULOS ECONOMILDO                                                                                ║
 * ║ ListaVeiculosMOB.VeiculosList() filtra por:                                                             ║
 * ║ • Categoria = "Coletivos Pequenos" OU "Ônibus"                                                          ║
 * ║ • Economildo = true                                                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ FILTRO MOTORISTAS ECONOMILDO                                                                              ║
 * ║ ListaMotoristasMOB.MotoristaList() filtra por:                                                          ║
 * ║ • JOIN com ViagensEconomildo (apenas motoristas com viagens Economildo)                                 ║
 * ║ • Distinct para evitar duplicatas                                                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern (static para compartilhamento)                                       ║
 * ║ • ViewMotoristas - View de motoristas com condutor                                                      ║
 * ║ • ViagensEconomildo - Tabela de viagens Economildo                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Pages.Viagens
{
    public class GestaoFluxoModel :PageModel
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
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "Initialize" , error);
                return;
            }
        }

        public GestaoFluxoModel(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "GestaoFluxoModel" , error);
            }
        }

        public void OnGet()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }

    public class ListaVeiculosMOB
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

        public ListaVeiculosMOB()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "ListaVeiculosMOB" , error);
            }
        }

        public ListaVeiculosMOB(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "ListaVeiculosMOB" , error);
            }
        }

        public List<ListaVeiculosMOB> VeiculosList()
        {
            try
            {
                List<ListaVeiculosMOB> veiculos = new List<ListaVeiculosMOB>();

                var result = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    where
                        (v.Categoria == "Coletivos Pequenos" || v.Categoria == "Ônibus")
                        && (v.Economildo == true)
                    orderby v.Placa
                    select new
                    {
                        Id = v.VeiculoId ,
                        Descricao = v.Placa + " - " + ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                    }
                ).OrderBy(v => v.Descricao);

                foreach (var veiculo in result)
                {
                    veiculos.Add(
                        new ListaVeiculosMOB
                        {
                            Descricao = veiculo.Descricao ,
                            Id = veiculo?.Id ?? Guid.Empty ,
                        }
                    );
                }

                return veiculos;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "VeiculosList" , error);
                return default(List<ListaVeiculosMOB>);
            }
        }
    }

    public class ListaMotoristasMOB
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

        public ListaMotoristasMOB()
        {
            try
            {
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "ListaMotoristasMOB" , error);
            }
        }

        public ListaMotoristasMOB(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "ListaMotoristasMOB" , error);
            }
        }

        public List<ListaMotoristasMOB> MotoristaList()
        {
            try
            {
                List<ListaMotoristasMOB> motoristas = new List<ListaMotoristasMOB>();

                var objMotorista = (
                    from vm in _unitOfWork.ViewMotoristas.GetAll()
                    join ve in _unitOfWork.ViagensEconomildo.GetAll()
                        on vm.MotoristaId equals ve.MotoristaId
                    select new
                    {
                        vm.Nome ,
                        vm.TipoCondutor ,
                        vm.MotoristaId ,
                        vm.MotoristaCondutor ,
                    }
                ).Distinct().OrderBy(vm => vm.Nome);

                foreach (var motorista in objMotorista)
                {
                    motoristas.Add(
                        new ListaMotoristasMOB
                        {
                            Descricao = motorista.MotoristaCondutor ,
                            Id = motorista.MotoristaId ,
                        }
                    );
                }

                return motoristas;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml.cs" , "MotoristaList" , error);
                return default(List<ListaMotoristasMOB>);
            }
        }
    }
}
