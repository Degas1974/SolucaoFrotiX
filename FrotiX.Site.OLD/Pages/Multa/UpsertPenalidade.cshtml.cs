/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: UpsertPenalidade.cshtml.cs (Pages/Multa)                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para criação e edição de multas na fase de Penalidade. Gerencia o ciclo final                  ║
 * ║ da multa incluindo pagamento, envio ao SECLE e arquivamento.                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ PROPRIEDADES BINDPROPERTY                                                                                 ║
 * ║ • MultaObj : MultaViewModel contendo a entidade Multa                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet(id) : Carrega multa existente ou inicializa nova com Status="À Pagar"                            ║
 * ║ • OnPostSubmit() : Cria nova penalidade com Fase="Penalidade"                                            ║
 * ║ • OnPostEdit(Id) : Atualiza penalidade existente                                                         ║
 * ║ • OnGetAJAXPreencheListaEmpenhos(id) : Retorna empenhos por órgão autuante (JSON)                        ║
 * ║ • OnGetPegaSaldoEmpenho(id) : Retorna saldo do empenho selecionado (JSON)                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ MÉTODOS DE PREENCHIMENTO DE LISTAS                                                                        ║
 * ║ • PreencheListaMotoristas() : Lista motoristas ativos com formato "(Ponto) Nome"                         ║
 * ║ • PreencheListaVeiculos() : Lista veículos ativos com Placa + Marca/Modelo                               ║
 * ║ • PreencheListaOrgaos() : Lista órgãos autuantes (DETRAN, PRF, etc.)                                     ║
 * ║ • PreencheListaInfracoes() : Lista tipos de multa com (Artigo)-(Código/Desdobramento) - Descrição        ║
 * ║ • PreencheListaTodosEmpenhos(orgaoId) : Empenhos do órgão para pagamento                                 ║
 * ║ • PreencheListaStatus() : À Pagar, Paga, Paga(Defin/Infrator), Enviada Secle, Arquivada, Cancelada       ║
 * ║ • PreencheListaContratoVeiculos() : Contratos tipo "Locação"                                             ║
 * ║ • PreencheListaAtaVeiculos() : Atas de registro de preço                                                 ║
 * ║ • PreencheListaContratoMotoristas() : Contratos tipo "Terceirização"                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ STATUS DE PENALIDADE                                                                                      ║
 * ║ • À Pagar : Multa confirmada aguardando pagamento                                                        ║
 * ║ • Paga : Multa paga pela instituição                                                                     ║
 * ║ • Paga (Defin) : Multa paga via DEFIN                                                                    ║
 * ║ • Paga (Infrator) : Multa paga pelo motorista infrator                                                   ║
 * ║ • À Enviar Secle : Aguardando envio ao SECLE                                                             ║
 * ║ • Enviada Secle : Multa enviada para cobrança via SECLE                                                  ║
 * ║ • Arquivada (Finalizada) : Processo encerrado                                                            ║
 * ║ • Infração Cancelada : Multa cancelada/prescrita                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • INotyfService - Notificações toast                                                                     ║
 * ║ • AppToast - Toast customizado do sistema                                                                ║
 * ║ • ViewMotoristas - View de motoristas                                                                    ║
 * ║ • ViewContratoFornecedor, ViewAtaFornecedor - Views de contratos                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Pages.Multa
{
    public class UpsertPenalidadeModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public static Guid multaId;

        public UpsertPenalidadeModel(
            IUnitOfWork unitOfWork ,
            INotyfService notyf ,
            IWebHostEnvironment hostingEnvironment
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "UpsertPenalidadeModel" ,
                    error
                );
            }
        }

        [BindProperty]
        public MultaViewModel MultaObj
        {
            get; set;
        }

        private void SetViewModel()
        {
            try
            {
                MultaObj = new MultaViewModel { Multa = new Models.Multa() };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertPenalidade.cshtml.cs" , "SetViewModel" , error);
            }
        }

        public IActionResult OnGet(Guid id)
        {
            try
            {
                SetViewModel();

                multaId = id;

                if (id != Guid.Empty)
                {
                    // MODO EDIÇÃO - Carrega registro existente
                    MultaObj.Multa = _unitOfWork.Multa.GetFirstOrDefault(m => m.MultaId == id);
                    if (MultaObj == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    // ✅ MODO CRIAÇÃO - Define valor padrão para Status
                    if (MultaObj.Multa.Status == null || string.IsNullOrEmpty(MultaObj.Multa.Status))
                    {
                        MultaObj.Multa.Status = "À Pagar";
                    }
                }

                PreencheListaMotoristas();
                PreencheListaVeiculos();
                PreencheListaStatus();
                PreencheListaOrgaos();

                if (MultaObj.Multa.OrgaoAutuanteId != null)
                {
                    PreencheListaTodosEmpenhos((Guid)MultaObj.Multa.OrgaoAutuanteId);
                }

                PreencheListaInfracoes();
                PreencheListaContratoVeiculos();
                PreencheListaAtaVeiculos();
                PreencheListaContratoMotoristas();

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertPenalidade.cshtml.cs" , "OnGet" , error);
                return Page();
            }
        }

        public IActionResult OnPostSubmit()
        {
            try
            {
                Guid multaId = Guid.Empty;

                if (MultaObj.Multa.MultaId != Guid.Empty)
                {
                    multaId = MultaObj.Multa.MultaId;
                }

                if (MultaObj.Multa.EnviadaSecle == null)
                {
                    MultaObj.Multa.EnviadaSecle = false;
                }

                if (MultaObj.Multa.Paga == null)
                {
                    MultaObj.Multa.Paga = false;
                }

                MultaObj.Multa.Fase = "Penalidade";

                AppToast.show("Verde" , "Penalidade adicionada com sucesso!" , 3000);

                _unitOfWork.Multa.Add(MultaObj.Multa);
                _unitOfWork.Save();

                return RedirectToPage("./ListaPenalidade");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertPenalidade.cshtml.cs" , "OnPostSubmit" , error);
                AppToast.show("Vermelho" , "Erro ao adicionar penalidade!" , 3000);
                return RedirectToPage("./ListaPenalidade");
            }
        }

        public IActionResult OnPostEdit(Guid Id)
        {
            try
            {
                MultaObj.Multa.MultaId = Id;

                Guid multaId = Guid.Empty;

                if (MultaObj.Multa.MultaId != Guid.Empty)
                {
                    multaId = MultaObj.Multa.MultaId;
                }

                if (MultaObj.Multa.EnviadaSecle == null)
                {
                    MultaObj.Multa.EnviadaSecle = false;
                }

                if (MultaObj.Multa.Paga == null)
                {
                    MultaObj.Multa.Paga = false;
                }

                AppToast.show("Verde" , "Penalidade atualizada com sucesso!" , 3000);
                _unitOfWork.Multa.Update(MultaObj.Multa);

                _unitOfWork.Save();

                return RedirectToPage("./ListaPenalidade");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UpsertPenalidade.cshtml.cs" , "OnPostEdit" , error);
                AppToast.show("Vermelho" , "Erro ao atualizar penalidade!" , 3000);
                return RedirectToPage("./ListaPenalidade");
            }
        }

        // ========================================
        // MÉTODOS DE PREENCHIMENTO DE LISTAS
        // ========================================

        private class TodosEmpenhosData
        {
            public Guid EmpenhoMultaId
            {
                get; set;
            }

            public string NotaEmpenho
            {
                get; set;
            }
        }

        // ✅ CORREÇÃO 1: Removida a filtragem !string.IsNullOrEmpty e ajustado OrderBy igual ao UpsertAutuacao
        public JsonResult PreencheListaTodosEmpenhos(Guid orgaoAutuanteId)
        {
            try
            {
                var ListaTodosEmpenhos = (
                    from e in _unitOfWork.EmpenhoMulta.GetAll()
                    where e.OrgaoAutuanteId == orgaoAutuanteId
                    orderby e.NotaEmpenho
                    select new
                    {
                        e.EmpenhoMultaId ,
                        e.NotaEmpenho
                    }
                )
                    .OrderByDescending(e => e.NotaEmpenho)
                    .ToList();

                List<TodosEmpenhosData> TodosEmpenhosDataSource = new List<TodosEmpenhosData>();

                foreach (var empenho in ListaTodosEmpenhos)
                {
                    TodosEmpenhosDataSource.Add(
                        new TodosEmpenhosData
                        {
                            EmpenhoMultaId = (Guid)empenho.EmpenhoMultaId ,
                            NotaEmpenho = empenho.NotaEmpenho ,
                        }
                    );
                }

                ViewData["dataTodosEmpenhos"] = TodosEmpenhosDataSource;

                return new JsonResult(new
                {
                    data = TodosEmpenhosDataSource
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaTodosEmpenhos" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false ,
                    erro = error.Message
                });
            }
        }

        private class EmpenhoData
        {
            public Guid EmpenhoMultaId
            {
                get; set;
            }

            public string NotaEmpenho
            {
                get; set;
            }
        }

        // ✅ CORREÇÃO 2: Removida a filtragem !string.IsNullOrEmpty e ajustado OrderBy igual ao UpsertAutuacao
        public JsonResult OnGetAJAXPreencheListaEmpenhos(string id)
        {
            try
            {
                var ListaEmpenhos = (
                    from e in _unitOfWork.EmpenhoMulta.GetAll()
                    where e.OrgaoAutuanteId == Guid.Parse(id)
                    orderby e.NotaEmpenho
                    select new
                    {
                        e.EmpenhoMultaId ,
                        e.NotaEmpenho
                    }
                )
                    .OrderByDescending(e => e.NotaEmpenho)
                    .ToList();

                List<EmpenhoData> EmpenhoDataSource = new List<EmpenhoData>();

                foreach (var empenho in ListaEmpenhos)
                {
                    EmpenhoDataSource.Add(
                        new EmpenhoData
                        {
                            EmpenhoMultaId = (Guid)empenho.EmpenhoMultaId ,
                            NotaEmpenho = empenho.NotaEmpenho ,
                        }
                    );
                }

                ViewData["dataEmpenho"] = EmpenhoDataSource;

                return new JsonResult(new
                {
                    data = EmpenhoDataSource
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "OnGetAJAXPreencheListaEmpenhos" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false ,
                    erro = error.Message
                });
            }
        }

        public JsonResult OnGetPegaSaldoEmpenho(string id)
        {
            try
            {
                if (id != "null" && id != "00000000-0000-0000-0000-000000000000")
                {
                    var Empenhos = _unitOfWork.EmpenhoMulta.GetFirstOrDefault(e =>
                        e.EmpenhoMultaId == Guid.Parse(id)
                    );
                    return new JsonResult(new
                    {
                        data = Empenhos.SaldoAtual
                    });
                }

                return new JsonResult(new
                {
                    data = 0
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "OnGetPegaSaldoEmpenho" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false ,
                    erro = error.Message
                });
            }
        }

        private class MotoristaData
        {
            public Guid MotoristaId
            {
                get; set;
            }

            public string Nome
            {
                get; set;
            }

            public Guid? ContratoId
            {
                get; set;
            }
        }

        public void PreencheListaMotoristas()
        {
            try
            {
                var ListaMotoristas = _unitOfWork
                    .ViewMotoristas.GetAll()
                    .Where(m => m.Status == true)
                    .OrderBy(n => n.Nome);

                List<MotoristaData> MotoristaDataSource = new List<MotoristaData>();

                foreach (var motorista in ListaMotoristas)
                {
                    MotoristaDataSource.Add(
                        new MotoristaData
                        {
                            MotoristaId = (Guid)motorista.MotoristaId ,
                            Nome = "(" + motorista.Ponto + ") " + motorista.MotoristaCondutor ,
                        }
                    );
                }

                ViewData["dataMotorista"] = MotoristaDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertAutuacao.cshtml.cs" ,
                    "PreencheListaMotoristas" ,
                    error
                );
            }
        }

        private class VeiculoData
        {
            public Guid VeiculoId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }

            public Guid? ContratoId
            {
                get; set;
            }

            public Guid? AtaId
            {
                get; set;
            }
        }

        public void PreencheListaVeiculos()
        {
            try
            {
                var ListaVeiculos = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    where v.Status != false
                    orderby v.Placa
                    select new
                    {
                        VeiculoId = v.VeiculoId ,
                        Descricao = v.Placa + " - " + ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                    }
                ).OrderBy(v => v.Descricao);

                List<VeiculoData> VeiculoDataSource = new List<VeiculoData>();

                foreach (var veiculo in ListaVeiculos)
                {
                    VeiculoDataSource.Add(
                        new VeiculoData
                        {
                            VeiculoId = (Guid)veiculo.VeiculoId ,
                            Descricao = veiculo.Descricao ,
                        }
                    );
                }

                ViewData["dataVeiculo"] = VeiculoDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertAutuacao.cshtml.cs" ,
                    "PreencheListaVeiculos" ,
                    error
                );
            }
        }

        private class OrgaoData
        {
            public Guid OrgaoId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }
        }

        // ✅ CORREÇÃO 3: Removida a filtragem !string.IsNullOrEmpty igual ao UpsertAutuacao
        public void PreencheListaOrgaos()
        {
            try
            {
                var ListaOrgaos = (
                    from o in _unitOfWork.OrgaoAutuante.GetAll()
                    orderby o.Sigla
                    select new
                    {
                        OrgaoId = o.OrgaoAutuanteId ,
                        Descricao = o.Sigla + " - " + o.Nome ,
                    }
                ).OrderBy(v => v.Descricao);

                List<OrgaoData> OrgaoDataSource = new List<OrgaoData>();

                foreach (var orgao in ListaOrgaos)
                {
                    OrgaoDataSource.Add(
                        new OrgaoData { OrgaoId = (Guid)orgao.OrgaoId , Descricao = orgao.Descricao }
                    );
                }

                ViewData["dataOrgao"] = OrgaoDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaOrgaos" ,
                    error
                );
            }
        }

        private class InfracaoData
        {
            public Guid TipoMultaId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }
        }

        // ✅ CORREÇÃO 4: Removida a filtragem !string.IsNullOrEmpty e ajustado OrderBy igual ao UpsertAutuacao
        public void PreencheListaInfracoes()
        {
            try
            {
                var ListaInfracoes = (
                    from o in _unitOfWork.TipoMulta.GetAll()
                    orderby o.Artigo
                    select new
                    {
                        TipoMultaId = o.TipoMultaId ,
                        Descricao = (
                            "("
                            + o.Artigo
                            + ")"
                            + "-("
                            + o.CodigoDenatran
                            + "/"
                            + o.Desdobramento
                            + ")"
                            + " - "
                            + Servicos.ConvertHtml(o.Descricao)
                        ) ,
                    }
                ).OrderBy(v => v.Descricao);

                List<InfracaoData> InfracaoDataSource = new List<InfracaoData>();

                foreach (var infracao in ListaInfracoes)
                {
                    InfracaoDataSource.Add(
                        new InfracaoData
                        {
                            TipoMultaId = (Guid)infracao.TipoMultaId ,
                            Descricao = infracao.Descricao ,
                        }
                    );
                }

                ViewData["dataInfracao"] = InfracaoDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaInfracoes" ,
                    error
                );
            }
        }

        private class StatusData
        {
            public string StatusId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }
        }

        public void PreencheListaStatus()
        {
            try
            {
                List<StatusData> StatusDataSource = new List<StatusData>();

                StatusDataSource.Add(
                    new StatusData { StatusId = "À Pagar" , Descricao = "À Pagar" }
                );

                StatusDataSource.Add(
                    new StatusData { StatusId = "Paga" , Descricao = "Paga" }
                );

                StatusDataSource.Add(
                    new StatusData { StatusId = "Paga (Defin)" , Descricao = "Paga (Defin)" }
                );

                StatusDataSource.Add(
                    new StatusData { StatusId = "Paga (Infrator)" , Descricao = "Paga (Infrator)" }
                );

                StatusDataSource.Add(
                    new StatusData { StatusId = "À Enviar Secle" , Descricao = "À Enviar Secle" }
                );

                StatusDataSource.Add(
                    new StatusData { StatusId = "Enviada Secle" , Descricao = "Enviada Secle" }
                );

                StatusDataSource.Add(
                    new StatusData
                    {
                        StatusId = "Arquivada (Finalizada)" ,
                        Descricao = "Arquivada (Finalizada)"
                    }
                );

                StatusDataSource.Add(
                    new StatusData
                    {
                        StatusId = "Infração Cancelada" ,
                        Descricao = "Infração Cancelada"
                    }
                );

                ViewData["dataStatus"] = StatusDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaStatus" ,
                    error
                );
            }
        }

        private class ContratoVeiculosData
        {
            public Guid ContratoId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }
        }

        public void PreencheListaContratoVeiculos()
        {
            try
            {
                var ListaContratoVeiculos = _unitOfWork.ViewContratoFornecedor.GetAll(cv =>
                    cv.TipoContrato == "Locação"
                );

                List<ContratoVeiculosData> ContratoVeiculosDataSource =
                    new List<ContratoVeiculosData>();

                foreach (var contrato in ListaContratoVeiculos)
                {
                    ContratoVeiculosDataSource.Add(
                        new ContratoVeiculosData
                        {
                            ContratoId = (Guid)contrato.ContratoId ,
                            Descricao = contrato.Descricao ,
                        }
                    );
                }

                ViewData["dataContratoVeiculos"] = ContratoVeiculosDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaContratoVeiculos" ,
                    error
                );
            }
        }

        private class AtaVeiculosData
        {
            public Guid AtaId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }
        }

        public void PreencheListaAtaVeiculos()
        {
            try
            {
                var ListaAtaVeiculos = _unitOfWork.ViewAtaFornecedor.GetAll();

                List<AtaVeiculosData> AtaVeiculosDataSource = new List<AtaVeiculosData>();

                foreach (var ata in ListaAtaVeiculos)
                {
                    AtaVeiculosDataSource.Add(
                        new AtaVeiculosData { AtaId = (Guid)ata.AtaId , Descricao = ata.AtaVeiculo }
                    );
                }

                ViewData["dataAtaVeiculos"] = AtaVeiculosDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaAtaVeiculos" ,
                    error
                );
            }
        }

        private class ContratoMotoristasData
        {
            public Guid ContratoId
            {
                get; set;
            }

            public string Descricao
            {
                get; set;
            }
        }

        public void PreencheListaContratoMotoristas()
        {
            try
            {
                var ListaContratoMotoristas = _unitOfWork.ViewContratoFornecedor.GetAll(cv =>
                    cv.TipoContrato == "Terceirização"
                );

                List<ContratoMotoristasData> ContratoMotoristasDataSource =
                    new List<ContratoMotoristasData>();

                foreach (var contrato in ListaContratoMotoristas)
                {
                    ContratoMotoristasDataSource.Add(
                        new ContratoMotoristasData
                        {
                            ContratoId = (Guid)contrato.ContratoId ,
                            Descricao = contrato.Descricao ,
                        }
                    );
                }

                ViewData["dataContratoMotoristas"] = ContratoMotoristasDataSource;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "UpsertPenalidade.cshtml.cs" ,
                    "PreencheListaContratoMotoristas" ,
                    error
                );
            }
        }
    }
}
