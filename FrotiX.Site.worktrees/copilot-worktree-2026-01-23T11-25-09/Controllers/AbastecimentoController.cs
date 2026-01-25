using FrotiX.Data;
using FrotiX.Hubs;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using FrotiX.Helpers; 
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  GESTÃO DE ABASTECIMENTOS (CORE)                                                    #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: AbastecimentoController                                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão transacional de abastecimentos.                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Abastecimento                                         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public partial class AbastecimentoController : ControllerBase
    {
        private readonly ILogger<AbastecimentoController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ImportacaoHub> _hubContext;
        private readonly FrotiXDbContext _context;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoController (Construtor)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Injeta dependências do módulo de abastecimentos.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • logger, hostingEnvironment, unitOfWork                                 ║
        /// ║    • hubContext, context, log                                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public AbastecimentoController(
            ILogger<AbastecimentoController> logger,
            IWebHostEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork,
            IHubContext<ImportacaoHub> hubContext,
            FrotiXDbContext context,
            ILogService log
        )
        {
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _hubContext = hubContext;
                _context = context;
                _log = log;

                _log.Info("AbastecimentoController inicializado", "AbastecimentoController.cs", "Constructor");
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Constructor", ex);
            }
        }

        public Models.Abastecimento AbastecimentoObj { get; set; }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: HealthCheck (GET)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica disponibilidade do serviço de abastecimentos.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: status do serviço.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("HealthCheck")]
        public IActionResult Index()
        {
            try
            {
                return Ok(new { status = "Online", service = "Abastecimento" });
            }
            catch (Exception error)
            {
                _log.Error("Erro no HealthCheck de Abastecimento", error, "AbastecimentoController.cs", "Index");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Index", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Retorna uma listagem geral de abastecimentos. A query é        ║
        /// ║    limitada aos 1000 abastecimentos mais recentes, ordenados por data/hora. ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Fornece uma visão geral dos abastecimentos para a interface administrativa.║
        /// ║    É a listagem padrão para exibição inicial em grids ou relatórios.         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo a lista de abastecimentos em `data`.║
        /// ║    • Significado: Coleção de registros de abastecimento para exibição.       ║
        /// ║    • Consumidor: Interfaces de usuário para exibição de dados de           ║
        /// ║      abastecimento.                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewAbastecimentos.GetAll() → Acesso a dados de abastecimentos.║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida primariamente pela UI para listagem.          ║
        /// ║    • Arquivos relacionados: Views/Abastecimento/Index.cshtml (ou similar)    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .OrderByDescending(va => va.DataHora)
                    .Take(1000)
                    .ToList();

                return Ok(new { data = dados });
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar listagem de abastecimentos", error, "AbastecimentoController.cs", "Get");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Get", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoVeiculos                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Filtra os registros de abastecimento com base no ID de um      ║
        /// ║    veículo específico. Retorna todos os abastecimentos associados ao         ║
        /// ║    veículo fornecido, ordenados do mais recente para o mais antigo.          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite visualizar o histórico de abastecimentos de um veículo individual.║
        /// ║    Essencial para análise de consumo e custos por veículo.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): O identificador único do veículo.                            ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo a lista de abastecimentos do       ║
        /// ║      veículo em `data`.                                                      ║
        /// ║    • Significado: Coleção de registros de abastecimento filtrados por veículo.║
        /// ║    • Consumidor: Interfaces de usuário que exibem detalhes de veículos e     ║
        /// ║      seu histórico.                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewAbastecimentos.GetAll() → Acesso a dados de abastecimentos.║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/AbastecimentoVeiculos?Id={Guid}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para filtragem específica.            ║
        /// ║    • Arquivos relacionados: Páginas de detalhes de veículos, componentes de ║
        /// ║      relatório.                                                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("AbastecimentoVeiculos")]
        public IActionResult AbastecimentoVeiculos(Guid Id)
        {
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.VeiculoId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new { data = dados });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao filtrar abastecimentos por veículo {Id}", error, "AbastecimentoController.cs", "AbastecimentoVeiculos");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoVeiculos", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoCombustivel                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Filtra os registros de abastecimento com base no ID de um      ║
        /// ║    tipo de combustível específico. Retorna todos os abastecimentos           ║
        /// ║    associados ao tipo de combustível fornecido, ordenados do mais recente   ║
        /// ║    para o mais antigo.                                                       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite analisar o consumo e a distribuição de diferentes tipos de        ║
        /// ║    combustível na frota. Útil para gestão de custos e planejamento de       ║
        /// ║    recursos.                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): O identificador único do tipo de combustível.                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo a lista de abastecimentos por      ║
        /// ║      combustível em `data`.                                                  ║
        /// ║    • Significado: Coleção de registros de abastecimento filtrados por tipo   ║
        /// ║      de combustível.                                                         ║
        /// ║    • Consumidor: Interfaces de usuário para dashboards, relatórios de consumo║
        /// ║      de combustível.                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewAbastecimentos.GetAll() → Acesso a dados de abastecimentos.║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/AbastecimentoCombustivel?Id={Guid}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para filtragem e análise.             ║
        /// ║    • Arquivos relacionados: Componentes de filtro, dashboards.               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("AbastecimentoCombustivel")]
        public IActionResult AbastecimentoCombustivel(Guid Id)
        {
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.CombustivelId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new { data = dados });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao filtrar abastecimentos por combustível {Id}", error, "AbastecimentoController.cs", "AbastecimentoCombustivel");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoCombustivel", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoUnidade                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Filtra os registros de abastecimento com base no ID de uma     ║
        /// ║    unidade (departamento, filial, etc.) específica. Retorna todos os         ║
        /// ║    abastecimentos associados à unidade fornecida, ordenados do mais recente  ║
        /// ║    para o mais antigo.                                                       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite a análise de abastecimentos por diferentes setores ou localidades.║
        /// ║    Fundamental para a gestão descentralizada e alocação de custos.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): O identificador único da unidade.                            ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo a lista de abastecimentos por      ║
        /// ║      unidade em `data`.                                                      ║
        /// ║    • Significado: Coleção de registros de abastecimento filtrados por unidade.║
        /// ║    • Consumidor: Interfaces de usuário para dashboards gerenciais, relatórios║
        /// ║      de custos por unidade.                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewAbastecimentos.GetAll() → Acesso a dados de abastecimentos.║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/AbastecimentoUnidade?Id={Guid}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para filtragem e análise gerencial.   ║
        /// ║    • Arquivos relacionados: Componentes de filtro, relatórios de gestão.     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("AbastecimentoUnidade")]
        public IActionResult AbastecimentoUnidade(Guid Id)
        {
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.UnidadeId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new { data = dados });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao filtrar abastecimentos por unidade {Id}", error, "AbastecimentoController.cs", "AbastecimentoUnidade");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoUnidade", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoMotorista                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Filtra os registros de abastecimento com base no ID de um      ║
        /// ║    motorista (condutor) específico. Retorna todos os abastecimentos         ║
        /// ║    associados ao motorista fornecido, ordenados do mais recente para o mais ║
        /// ║    antigo.                                                                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite acompanhar o histórico de abastecimentos por condutor, auxiliando║
        /// ║    na gestão de responsabilidades e análise de performance individual.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): O identificador único do motorista.                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo a lista de abastecimentos por      ║
        /// ║      motorista em `data`.                                                    ║
        /// ║    • Significado: Coleção de registros de abastecimento filtrados por        ║
        /// ║      motorista.                                                              ║
        /// ║    • Consumidor: Interfaces de usuário para perfis de motoristas, relatórios║
        /// ║      de performance.                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewAbastecimentos.GetAll() → Acesso a dados de abastecimentos.║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/AbastecimentoMotorista?Id={Guid}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para filtragem e análise de condutor. ║
        /// ║    • Arquivos relacionados: Páginas de detalhes de motoristas, dashboards.   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("AbastecimentoMotorista")]
        public IActionResult AbastecimentoMotorista(Guid Id)
        {
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.MotoristaId == Id)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new { data = dados });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao filtrar abastecimentos por motorista {Id}", error, "AbastecimentoController.cs", "AbastecimentoMotorista");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoMotorista", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AbastecimentoData                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Filtra os registros de abastecimento com base em uma data      ║
        /// ║    específica. Retorna todos os abastecimentos que ocorreram na data         ║
        /// ║    fornecida, ordenados do mais recente para o mais antigo.                  ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite a recuperação de dados de abastecimento para um dia específico,   ║
        /// ║    facilitando análises diárias e a identificação de padrões.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataAbastecimento (string): A data do abastecimento no formato string   ║
        /// ║      (ex: "YYYY-MM-DD").                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo a lista de abastecimentos para a   ║
        /// ║      data especificada em `data`.                                            ║
        /// ║    • Significado: Coleção de registros de abastecimento filtrados por data.  ║
        /// ║    • Consumidor: Interfaces de usuário para consultas pontuais e relatórios  ║
        /// ║      periódicos.                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewAbastecimentos.GetAll() → Acesso a dados de abastecimentos.║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/AbastecimentoData?dataAbastecimento={data}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para filtragem de dados temporais.    ║
        /// ║    • Arquivos relacionados: Componentes de filtro de data, dashboards.       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("AbastecimentoData")]
        public IActionResult AbastecimentoData(string dataAbastecimento)
        {
            try
            {
                var dados = _unitOfWork
                    .ViewAbastecimentos.GetAll()
                    .Where(va => va.Data == dataAbastecimento)
                    .OrderByDescending(va => va.DataHora)
                    .ToList();

                return Ok(new { data = dados });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao filtrar abastecimentos por data {dataAbastecimento}", error, "AbastecimentoController.cs", "AbastecimentoData");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AbastecimentoData", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Import (Legado)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Processa de forma síncrona um arquivo Excel (XLS/XLSX) para    ║
        /// ║    importar dados de abastecimento. Realiza validação básica e persiste os  ║
        /// ║    dados no banco de dados dentro de uma transação.                                                 ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite a importação em massa de dados de abastecimento via planilha.      ║
        /// ║    É uma funcionalidade crítica para a alimentação inicial do sistema e      ║
        /// ║    atualizações pontuais.                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • file (IFormFile): O arquivo Excel a ser importado, recebido via        ║
        /// ║      Request.Form.Files[0].                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: Retorna um objeto JSON indicando sucesso ou falha na     ║
        /// ║      importação, incluindo uma mensagem e, em caso de sucesso, uma tabela   ║
        /// ║      HTML com os dados processados.                                          ║
        /// ║    • Significado: Confirmação do resultado da operação de importação.        ║
        /// ║    • Consumidor: Interface de usuário que dispara a importação de planilha.  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _hostingEnvironment.WebRootPath → Acesso ao caminho raiz da aplicação.  ║
        /// ║    • Path.Combine, Directory.CreateDirectory → Manipulação de arquivos/pastas.║
        /// ║    • NPOI (HSSFWorkbook, XSSFWorkbook, ISheet, IRow, ICell) → Leitura de Excel.║
        /// ║    • TransactionScope → Gestão de transações de banco de dados.              ║
        /// ║    • _unitOfWork.Abastecimento.GetFirstOrDefault(), Add(), Save() → CRUD no DB.║
        /// ║    • _unitOfWork.Veiculo.GetFirstOrDefault() → Busca de veículos.            ║
        /// ║    • _unitOfWork.Motorista.GetFirstOrDefault() → Busca de motoristas.        ║
        /// ║    • _unitOfWork.ViewMediaConsumo.GetFirstOrDefault() → Busca média de consumo.║
        /// ║    • _log.Error(), _log.Info() → Registro de eventos/erros.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Tratamento padronizado de exceções.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP POST para /api/Abastecimento/Import                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA - Apesar de ser uma API, é uma funcionalidade legada      ║
        /// ║    com uso pontual. Integra-se com o Core, Repository, Models.               ║
        /// ║    • Arquivos relacionados: AbastecimentoController.Import.cs (para novos    ║
        /// ║      fluxos), Models/Abastecimento.cs, Repository/IUnitOfWork.cs.            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Import")]
        public ActionResult Import()
        {
            try
            {
                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Inicialização e Preparação do Arquivo
                // Coleta o arquivo enviado, define o caminho de armazenamento e
                // cria o diretório se ele não existir.
                // ═══════════════════════════════════════════════════════════════
                IFormFile file = Request.Form.Files[0];
                string folderName = "DadosEditaveis/UploadExcel";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();

                if (!Directory.Exists(newPath))
                {
                    // [LOGICA] Criação de diretório para upload se não existir
                    Directory.CreateDirectory(newPath);
                }

                // [REGRA] Verifica se o arquivo tem conteúdo antes de processar
                if (file.Length > 0)
                {
                    // ═══════════════════════════════════════════════════════════════
                    // 🔹 BLOCO: Processamento do Arquivo Excel
                    // Determina o tipo de arquivo (.xls ou .xlsx) e carrega a planilha
                    // usando a biblioteca NPOI. Em seguida, lê o cabeçalho para
                    // construir a estrutura da tabela HTML de resposta.
                    // ═══════════════════════════════════════════════════════════════
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        // [DADOS] Salva o arquivo temporariamente no servidor
                        file.CopyTo(stream);
                        stream.Position = 0;

                        // [LOGICA] Identifica e carrega o tipo de planilha (XLS ou XLSX)
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }

                        // [DADOS] Lê a linha de cabeçalho da planilha
                        IRow headerRow = sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;
                        sb.Append("<table id='tblImportacao' class='display' style='width: 100%'><thead><tr>");

                        // [LOGICA] Constrói o cabeçalho da tabela HTML de resposta
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (j == 5 || j == 7 || j == 10 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15)
                            {
                                sb.Append("<th>" + cell.ToString() + "</th>");
                            }
                        }

                        sb.Append("<th>" + "Consumo" + "</th>");
                        sb.Append("<th>" + "Média" + "</th>");
                        sb.Append("</tr></thead>");

                        try
                        {
                            // ═══════════════════════════════════════════════════════════════
                            // 🔹 BLOCO: Transação e Processamento de Linhas
                            // Inicia uma transação para garantir atomicidade na importação.
                            // Itera sobre as linhas da planilha, extrai dados, valida e
                            // persiste cada abastecimento no banco de dados.
                            // ═══════════════════════════════════════════════════════════════
                            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 30, 30)))
                            {
                                sb.AppendLine("<tbody><tr>");

                                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (row == null) continue;
                                    // [REGRA] Ignora linhas completamente em branco
                                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                                    AbastecimentoObj = new Models.Abastecimento();
                                    AbastecimentoObj.AbastecimentoId = Guid.NewGuid();

                                    for (int j = row.FirstCellNum; j < cellCount; j++)
                                    {
                                        if (row.GetCell(j) != null)
                                        {
                                            // [REGRA] Validação para evitar reimportação do mesmo dia
                                            if (i == 1 && j == 0)
                                            {
                                                var dataStr = row.GetCell(j).ToString();
                                                var dataDt = Convert.ToDateTime(dataStr);
                                                var objFromDb = _unitOfWork.Abastecimento.GetFirstOrDefault(u => u.DataHora == dataDt);
                                                if (objFromDb != null)
                                                {
                                                    // [UI] Retorna erro se o dia já foi importado
                                                    return Ok(new { success = false, message = "Os registros para o dia " + dataStr + " já foram importados!" });
                                                }
                                            }

                                            // [DADOS] Popula o objeto Abastecimento com dados da célula
                                            if (j == 7)
                                            {
                                                AbastecimentoObj.DataHora = Convert.ToDateTime(row.GetCell(j).ToString());
                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                            }

                                            // [DADOS] Busca e associa Veículo pelo número da placa
                                            if (j == 5)
                                            {
                                                string placaVeiculo = row.GetCell(j).ToString();
                                                var veiculoObj = _unitOfWork.Veiculo.GetFirstOrDefault(m => m.Placa == placaVeiculo);
                                                if (veiculoObj != null)
                                                {
                                                    AbastecimentoObj.VeiculoId = veiculoObj.VeiculoId;
                                                    sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    // [UI] Retorna erro se o veículo não for encontrado
                                                    return Ok(new { success = false, message = "Não foi encontrado o veículo de placa: " + placaVeiculo });
                                                }
                                            }

                                            // [DADOS] Busca e associa Motorista pelo nome
                                            if (j == 10)
                                            {
                                                string motorista = row.GetCell(j).ToString().Replace(".", "");
                                                var motoristaObj = _unitOfWork.Motorista.GetFirstOrDefault(m => m.Nome == motorista);
                                                if (motoristaObj != null)
                                                {
                                                    AbastecimentoObj.MotoristaId = motoristaObj.MotoristaId;
                                                    sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    // [UI] Retorna erro se o motorista não for encontrado
                                                    return Ok(new { success = false, message = "Não foi encontrado o(a) motorista: " + motorista });
                                                }
                                            }

                                            // [DADOS] Hodometro
                                            if (j == 12)
                                            {
                                                AbastecimentoObj.Hodometro = Convert.ToInt32(row.GetCell(j).ToString());
                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                            }

                                            // [LOGICA] Cálculo de KM Rodado
                                            if (j == 11)
                                            {
                                                AbastecimentoObj.KmRodado = Convert.ToInt32(row.GetCell(12).ToString()) - Convert.ToInt32(row.GetCell(11).ToString());
                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                            }

                                            // [DADOS] Associa tipo de Combustível por GUID pré-definido
                                            if (j == 13)
                                            {
                                                if (row.GetCell(j).ToString() == "GASOLINA")
                                                {
                                                    AbastecimentoObj.CombustivelId = Guid.Parse("F668F660-8380-4DF3-90CD-787DB06FE734");
                                                }
                                                else
                                                {
                                                    AbastecimentoObj.CombustivelId = Guid.Parse("A69AA86A-9162-4242-AB9A-8B184E04C4DA");
                                                }
                                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                            }

                                            // [DADOS] Valor Unitário do Combustível
                                            if (j == 14)
                                            {
                                                AbastecimentoObj.ValorUnitario = Convert.ToDouble(row.GetCell(j).ToString());
                                                sb.Append("<td>" + Math.Round((double)AbastecimentoObj.ValorUnitario, 2).ToString("0.00") + "</td>");
                                            }

                                            // [DADOS] Litros Abastecidos
                                            if (j == 15)
                                            {
                                                AbastecimentoObj.Litros = Convert.ToDouble(row.GetCell(j).ToString());
                                                sb.Append("<td>" + Math.Round((double)AbastecimentoObj.Litros, 2).ToString("0.00") + "</td>");
                                            }
                                        }
                                    }

                                    // [LOGICA] Cálculo e exibição do consumo instantâneo
                                    sb.Append("<td>" + Math.Round(((double)AbastecimentoObj.KmRodado / (double)AbastecimentoObj.Litros), 2).ToString("0.00") + "</td>");

                                    // [DADOS] Busca a média de consumo do veículo (se existir)
                                    var mediaveiculo = _unitOfWork.ViewMediaConsumo.GetFirstOrDefault(v => v.VeiculoId == AbastecimentoObj.VeiculoId);
                                    if (mediaveiculo != null)
                                    {
                                        sb.Append("<td>" + mediaveiculo.ConsumoGeral + "</td>");
                                    }
                                    else
                                    {
                                        // [LOGICA] Se não há média, usa o consumo instantâneo
                                        sb.Append("<td>" + Math.Round(((double)AbastecimentoObj.KmRodado / (double)AbastecimentoObj.Litros), 2).ToString("0.00") + "</td>");
                                    }

                                    sb.AppendLine("</tr>");
                                    // [DADOS] Adiciona e salva o abastecimento no banco de dados
                                    _unitOfWork.Abastecimento.Add(AbastecimentoObj);
                                    _unitOfWork.Save();
                                }

                                sb.Append("</tbody></table>");
                                // [LOGICA] Confirma a transação se todas as operações foram bem-sucedidas
                                scope.Complete();
                            }
                        }
                        catch (Exception error)
                        {
                            // [DEBUG] Log de erro crítico na transação
                            _log.Error("Falha crítica durante transação de importação", error, "AbastecimentoController.cs", "Import.TransactionScope");
                            Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Import.TransactionScope", error);
                            throw; // Re-lança para que a transação seja desfeita
                        }
                    }
                }

                // [DEBUG] Log de sucesso da importação
                _log.Info($"Planilha importada com sucesso: {file.FileName}", "AbastecimentoController.cs", "Import");

                // [UI] Retorna sucesso com a tabela HTML gerada
                return Ok(new
                {
                    success = true,
                    message = "Planilha Importada com Sucesso",
                    response = sb.ToString(),
                });
            }
            catch (Exception error)
            {
                // [DEBUG] Log de erro fatal no processo de importação
                _log.Error("Erro fatal no processo de importação legado", error, "AbastecimentoController.cs", "Import");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "Import", error);
                // [UI] Retorna erro 500 em caso de falha geral
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MotoristaList (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista motoristas para dropdowns.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("MotoristaList")]
        public IActionResult MotoristaList()
        {
            try
            {
                var result = _unitOfWork.ViewMotoristas.GetAll().OrderBy(vm => vm.Nome).ToList();
                return Ok(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar lista de motoristas", error, "AbastecimentoController.cs", "MotoristaList");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "MotoristaList", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UnidadeList (GET)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista unidades para dropdowns.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("UnidadeList")]
        public IActionResult UnidadeList()
        {
            try
            {
                var result = _unitOfWork.Unidade.GetAll().OrderBy(u => u.Descricao).ToList();
                return Ok(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar lista de unidades", error, "AbastecimentoController.cs", "UnidadeList");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "UnidadeList", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CombustivelList (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista combustíveis para dropdowns.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("CombustivelList")]
        public IActionResult CombustivelList()
        {
            try
            {
                var result = _unitOfWork.Combustivel.GetAll().OrderBy(u => u.Descricao).ToList();
                return Ok(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar lista de combustíveis", error, "AbastecimentoController.cs", "CombustivelList");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "CombustivelList", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VeiculoList (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista veículos formatada para dropdowns.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("VeiculoList")]
        public IActionResult VeiculoList()
        {
            try
            {
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll()
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId
                    orderby v.Placa
                    select new
                    {
                        v.VeiculoId,
                        PlacaMarcaModelo = v.Placa + " - " + ma.DescricaoMarca + "/" + m.DescricaoModelo,
                    }
                )
                .OrderBy(v => v.PlacaMarcaModelo)
                .ToList();

                return Ok(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar lista de veículos", error, "AbastecimentoController.cs", "VeiculoList");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "VeiculoList", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizaQuilometragem                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Atualiza o campo 'KmRodado' de um registro de abastecimento    ║
        /// ║    existente no banco de dados. Recebe o ID do abastecimento e o novo valor ║
        /// ║    de KM rodado via payload JSON.                                            ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite a correção ou atualização manual da quilometragem associada a um  ║
        /// ║    abastecimento, o que é crucial para cálculos de consumo e análise de      ║
        /// ║    eficiência.                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • payload (Dictionary<string, object>): Objeto JSON contendo:             ║
        /// ║      - "AbastecimentoId" (Guid): O ID do abastecimento a ser atualizado.     ║
        /// ║      - "KmRodado" (int, opcional): O novo valor da quilometragem rodada.     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult:                                                          ║
        /// ║      - Ok (200): Em caso de sucesso, { success: true, message: "..." }.      ║
        /// ║      - BadRequest (400): Se "AbastecimentoId" for inválido ou ausente.       ║
        /// ║      - NotFound (404): Se o abastecimento não for encontrado.                ║
        /// ║      - StatusCode (500): Em caso de erro interno do servidor.                ║
        /// ║    • Significado: Confirmação da atualização ou indicação de falha com      ║
        /// ║      mensagens descritivas.                                                  ║
        /// ║    • Consumidor: Interfaces de usuário para edição de dados de abastecimento.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Guid.Parse() → Conversão de string para Guid.                           ║
        /// ║    • _unitOfWork.Abastecimento.GetFirstOrDefault() → Busca o abastecimento.  ║
        /// ║    • int.TryParse() → Tentativa de conversão segura para inteiro.            ║
        /// ║    • _unitOfWork.Abastecimento.Update() → Atualiza o registro no banco.      ║
        /// ║    • _unitOfWork.Save() → Persiste as mudanças no banco de dados.            ║
        /// ║    • _log.Info(), _log.Error() → Registro de eventos/erros.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Tratamento padronizado de exceções.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP POST para /api/Abastecimento/AtualizaQuilometragem     ║
        /// ║    • Método EditaKm() (interno)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA - Manipulação direta de dados de abastecimento.            ║
        /// ║    • Arquivos relacionados: Models/Abastecimento.cs, Repository/IUnitOfWork.cs.║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("AtualizaQuilometragem")]
        public IActionResult AtualizaQuilometragem([FromBody] Dictionary<string, object> payload)
        {
            try
            {
                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Validação de Parâmetros de Entrada
                // Verifica se o 'AbastecimentoId' está presente no payload, que
                // é um parâmetro obrigatório para identificar o registro a ser
                // atualizado.
                // ═══════════════════════════════════════════════════════════════
                if (!payload.TryGetValue("AbastecimentoId", out var abastecimentoIdObj))
                {
                    // [REGRA] Retorna BadRequest se AbastecimentoId estiver ausente
                    return BadRequest(new { success = false, message = "AbastecimentoId é obrigatório" });
                }

                // [DADOS] Converte o ID para Guid e busca o objeto de abastecimento no DB
                var abastecimentoId = Guid.Parse(abastecimentoIdObj.ToString());
                var objAbastecimento = _unitOfWork.Abastecimento.GetFirstOrDefault(a => a.AbastecimentoId == abastecimentoId);

                // [REGRA] Verifica se o abastecimento foi encontrado
                if (objAbastecimento == null)
                {
                    // [UI] Retorna NotFound se o abastecimento não existir
                    return NotFound(new { success = false, message = "Abastecimento não encontrado" });
                }

                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Atualização do KM Rodado
                // Verifica se o 'KmRodado' está presente no payload e, se sim,
                // tenta parseá-lo para inteiro e atualiza o objeto de abastecimento.
                // ═══════════════════════════════════════════════════════════════
                if (payload.TryGetValue("KmRodado", out var kmRodadoObj) && kmRodadoObj != null)
                {
                    if (int.TryParse(kmRodadoObj.ToString(), out var kmRodado))
                    {
                        // [DADOS] Atribui o novo valor de KmRodado
                        objAbastecimento.KmRodado = kmRodado;
                    }
                }

                // [DADOS] Persiste as alterações no banco de dados
                _unitOfWork.Abastecimento.Update(objAbastecimento);
                _unitOfWork.Save();

                // [DEBUG] Log de sucesso da atualização
                _log.Info($"KM de abastecimento {abastecimentoId} atualizado para {objAbastecimento.KmRodado}", "AbastecimentoController.cs", "AtualizaQuilometragem");

                // [UI] Retorna sucesso
                return Ok(new { success = true, message = "Quilometragem atualizada com sucesso" });
            }
            catch (Exception error)
            {
                // [DEBUG] Log de erro na atualização da quilometragem
                _log.Error("Erro ao atualizar quilometragem", error, "AbastecimentoController.cs", "AtualizaQuilometragem");
                // [HELPER] Tratamento padronizado de erro
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "AtualizaQuilometragem", error);
                // [UI] Retorna erro 500
                return StatusCode(500, new { success = false, message = error.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: EditaKm                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Alias para o método AtualizaQuilometragem. Permite a           ║
        /// ║    atualização do campo de quilometragem rodada para um abastecimento       ║
        /// ║    específico.                                                               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Oferece flexibilidade na chamada da funcionalidade de atualização de KM   ║
        /// ║    em cenários onde o nome "EditaKm" possa ser mais intuitivo ou já esteja  ║
        /// ║    em uso em outras partes do sistema.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • payload (Dictionary<string, object>): Um dicionário JSON contendo       ║
        /// ║      "AbastecimentoId" (Guid) e opcionalmente "KmRodado" (int).             ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Retorno do método AtualizaQuilometragem. Pode ser Ok    ║
        /// ║      (sucesso), BadRequest (parâmetros inválidos) ou NotFound (abastecimento║
        /// ║      não encontrado).                                                        ║
        /// ║    • Significado: Confirmação da atualização ou indicação de erro.           ║
        /// ║    • Consumidor: Interfaces de usuário para edição de dados de abastecimento.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • AtualizaQuilometragem() → Redireciona a chamada para o método          ║
        /// ║      principal de atualização de quilometragem.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP POST para /api/Abastecimento/EditaKm                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA - Atua como um "proxy" para AtualizaQuilometragem.        ║
        /// ║    • Arquivos relacionados: AbastecimentoController.cs (AtualizaQuilometragem).║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("EditaKm")]
        public IActionResult EditaKm([FromBody] Dictionary<string, object> payload)
        {
            // ═══════════════════════════════════════════════════════════════
            // 🔹 BLOCO: Redirecionamento para AtualizaQuilometragem
            // Este método serve como um alias direto para a funcionalidade de
            // atualização de quilometragem, garantindo que a lógica principal
            // seja centralizada em um único local.
            // ═══════════════════════════════════════════════════════════════
            return AtualizaQuilometragem(payload);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaRegistroCupons                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Retorna uma lista dos registros de cupons de abastecimento     ║
        /// ║    processados, incluindo a data de registro e o ID do cupom.                ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Fornece o histórico de documentos comprobatórios de abastecimentos,       ║
        /// ║    essencial para auditoria e rastreabilidade.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • IDapi (string, opcional/não utilizado): Parâmetro que não é utilizado   ║
        /// ║      atualmente na lógica, mas pode ter sido previsto para filtragem futura.║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo uma lista anônima com `DataRegistro`║
        /// ║      e `RegistroCupomId` dos cupons, ordenados pela data mais recente.       ║
        /// ║    • Significado: Coleção de metadados dos registros de cupons.              ║
        /// ║    • Consumidor: Interfaces de usuário para listagem e consulta de cupons.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.RegistroCupomAbastecimento.GetAll() → Acesso a dados de     ║
        /// ║      registros de cupons.                                                    ║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/ListaRegistroCupons        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para exibição de histórico de cupons. ║
        /// ║    • Arquivos relacionados: Views/Abastecimento/Cupons.cshtml (ou similar).  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("ListaRegistroCupons")]
        public IActionResult ListaRegistroCupons(string IDapi)
        {
            try
            {
                var result = (
                    from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                    orderby rc.DataRegistro descending
                    select new
                    {
                        DataRegistro = rc.DataRegistro != null ? rc.DataRegistro.Value.ToShortDateString() : "",
                        rc.RegistroCupomId,
                    }
                ).ToList();

                return Ok(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro ao carregar lista de registro de cupons", error, "AbastecimentoController.cs", "ListaRegistroCupons");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "ListaRegistroCupons", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaRegistroCupons                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Retorna o conteúdo binário (PDF) de um registro de cupom de    ║
        /// ║    abastecimento específico, identificado pelo seu ID.                       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite a visualização do documento original do cupom de abastecimento,   ║
        /// ║    sendo crucial para conferências e auditorias.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • IDapi (string): O identificador único (GUID) do registro do cupom.      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo o PDF em formato binário           ║
        /// ║      (`RegistroPDF`) associado ao cupom.                                    ║
        /// ║    • Significado: O arquivo PDF do cupom de abastecimento.                   ║
        /// ║    • Consumidor: Componentes de visualização de PDF na interface de usuário. ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Guid.Parse() → Conversão de string para Guid.                           ║
        /// ║    • _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault() → Busca o    ║
        /// ║      registro do cupom.                                                      ║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/PegaRegistroCupons?IDapi={Guid}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para recuperação de documentos.        ║
        /// ║    • Arquivos relacionados: Models/RegistroCupomAbastecimento.cs.            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("PegaRegistroCupons")]
        public IActionResult PegaRegistroCupons(string IDapi)
        {
            try
            {
                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc => rc.RegistroCupomId == Guid.Parse(IDapi));
                return Ok(new { RegistroPDF = objRegistro.RegistroPDF });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao recuperar PDF do cupom {IDapi}", error, "AbastecimentoController.cs", "PegaRegistroCupons");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "PegaRegistroCupons", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaRegistroCuponsData                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Filtra os registros de cupons de abastecimento com base em     ║
        /// ║    uma data específica. Retorna uma lista de cupons registrados na data     ║
        /// ║    fornecida, ordenados da mais recente para a mais antiga.                  ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite aos usuários pesquisar e visualizar os cupons de abastecimento   ║
        /// ║    por período, facilitando a organização e auditoria de documentos.         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (string): A data no formato de string (ex: "YYYY-MM-DD") para        ║
        /// ║      filtragem dos cupons.                                                   ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON contendo uma lista anônima com `DataRegistro`║
        /// ║      e `RegistroCupomId` dos cupons que correspondem à data.                 ║
        /// ║    • Significado: Coleção de metadados dos registros de cupons filtrados por ║
        /// ║      data.                                                                   ║
        /// ║    • Consumidor: Interfaces de usuário para funcionalidade de pesquisa e     ║
        /// ║      filtragem de cupons.                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • DateTime.Parse() → Converte a string de data para um objeto DateTime.   ║
        /// ║    • _unitOfWork.RegistroCupomAbastecimento.GetAll() → Acesso a dados de     ║
        /// ║      registros de cupons.                                                    ║
        /// ║    • _log.Error() → Para registrar erros no sistema de log.                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/PegaRegistroCuponsData?id={data}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para filtragem de histórico de cupons.║
        /// ║    • Arquivos relacionados: Views/Abastecimento/Cupons.cshtml (ou similar).  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("PegaRegistroCuponsData")]
        public IActionResult PegaRegistroCuponsData(string id)
        {
            try
            {
                var result = (
                    from rc in _unitOfWork.RegistroCupomAbastecimento.GetAll()
                    where rc.DataRegistro == DateTime.Parse(id)
                    orderby rc.DataRegistro descending
                    select new
                    {
                        DataRegistro = rc.DataRegistro != null ? rc.DataRegistro.Value.ToShortDateString() : "",
                        rc.RegistroCupomId,
                    }
                ).ToList();

                return Ok(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao filtrar registros de cupons por data {id}", error, "AbastecimentoController.cs", "PegaRegistroCuponsData");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "PegaRegistroCuponsData", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteRegistro                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Exclui permanentemente um registro de cupom de abastecimento   ║
        /// ║    do banco de dados, identificado pelo seu ID.                              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite a remoção de registros incorretos ou obsoletos, mantendo a        ║
        /// ║    integridade e a relevância dos dados do sistema.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • IDapi (string): O identificador único (GUID) do registro do cupom a ser║
        /// ║      excluído.                                                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Objeto JSON indicando sucesso ou falha na exclusão.     ║
        /// ║      Retorna `{ success: true, message: "..." }` em caso de sucesso.        ║
        /// ║    • Significado: Confirmação da operação de exclusão.                       ║
        /// ║    • Consumidor: Interface de usuário para gerenciar registros de cupons.    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Guid.Parse() → Conversão de string para Guid.                           ║
        /// ║    • _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault() → Busca o    ║
        /// ║      registro a ser excluído.                                                ║
        /// ║    • _unitOfWork.RegistroCupomAbastecimento.Remove() → Remove o registro.    ║
        /// ║    • _unitOfWork.Save() → Persiste as mudanças no banco de dados.            ║
        /// ║    • _log.Warning(), _log.Error() → Registro de eventos/erros.               ║
        /// ║    • Alerta.TratamentoErroComLinha() → Para padronizar o tratamento de exceções.║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Requisições HTTP GET para /api/Abastecimento/DeleteRegistro?IDapi={Guid}║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Consumida pela UI para operações de exclusão de dados.  ║
        /// ║    • Arquivos relacionados: Models/RegistroCupomAbastecimento.cs.            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("DeleteRegistro")]
        public IActionResult DeleteRegistro(string IDapi)
        {
            try
            {
                var guid = Guid.Parse(IDapi);
                var objRegistro = _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(rc => rc.RegistroCupomId == guid);

                if (objRegistro != null)
                {
                    _unitOfWork.RegistroCupomAbastecimento.Remove(objRegistro);
                    _unitOfWork.Save();
                    _log.Warning($"Registro de cupom {IDapi} excluído permanentemente", "AbastecimentoController.cs", "DeleteRegistro");
                }

                return Ok(new { success = true, message = "Registro excluído com sucesso!" });
            }
            catch (Exception error)
            {
                _log.Error($"Erro ao excluir registro de cupom {IDapi}", error, "AbastecimentoController.cs", "DeleteRegistro");
                Alerta.TratamentoErroComLinha("AbastecimentoController.cs", "DeleteRegistro", error);
                return StatusCode(500);
            }
        }
    }
}
