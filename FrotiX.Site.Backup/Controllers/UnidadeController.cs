/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Unidades (Core Stack)             |
 * |_____________________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de unidades operacionais,
 * hubs de frota e estrutura geográfica de atendimento.
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: UnidadeController                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de unidades operacionais e lotações de motoristas.                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Unidade                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UnidadeController (Construtor)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork, notificação e serviço de log.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • notyf (INotyfService): Serviço de notificação.                          ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public UnidadeController(IUnitOfWork unitOfWork, INotyfService notyf, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs", "UnidadeController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna todas as unidades cadastradas.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de unidades.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /*
        *  _______________________________________________________
        * |                                                       |
        * |   GET - LISTAGEM GERAL DE UNIDADES                    |
        * |_______________________________________________________|
        */
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta unidades.
                var data = _unitOfWork.Unidade.GetAll();
                // [RETORNO] Lista de unidades.
                return Json(new { data });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "Get");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "Get" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   DELETE - REMOVE UMA UNIDADE                         |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove unidade se não houver veículos vinculados.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (UnidadeViewModel): Dados com ID da unidade.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(UnidadeViewModel model)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (model != null && model.UnidadeId != Guid.Empty)
                {
                    // [DADOS] Busca unidade.
                    var objFromDb = _unitOfWork.Unidade.GetFirstOrDefault(u =>
                        u.UnidadeId == model.UnidadeId
                    );

                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica veículos vinculados.
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.UnidadeId == model.UnidadeId
                        );

                        if (veiculo != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem veículos associados a essa unidade" ,
                                }
                            );
                        }

                        // [ACAO] Remove unidade.
                        _unitOfWork.Unidade.Remove(objFromDb);
                        _unitOfWork.Save();

                        _log.Info($"Unidade removida com sucesso: {objFromDb.Descricao} (ID: {model.UnidadeId})", "UnidadeController", "Delete");

                        // [RETORNO] Sucesso.
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Unidade removida com sucesso"
                            }
                        );
                    }
                }

                // [RETORNO] Falha padrão.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Unidade"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "Delete");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "Delete" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar unidade"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   UPDATE STATUS - ATIVA/DESATIVA UNIDADE              |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatus (POST)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna status (ativo/inativo) da unidade.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID da unidade.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatus")]
        public JsonResult UpdateStatus(Guid Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Busca unidade.
                    var objFromDb = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == Id);
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [REGRA] Alterna status.
                        objFromDb.Status = !objFromDb.Status;
                        type = objFromDb.Status ? 0 : 1;

                        // [ACAO] Persiste alterações.
                        _unitOfWork.Unidade.Update(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro da alteração.
                        string statusMsg = objFromDb.Status ? "Ativo" : "Inativo";
                        _log.Info($"Status da Unidade atualizado para {statusMsg}: {objFromDb.Descricao} (ID: {Id})", "UnidadeController", "UpdateStatus");
                    }

                    // [RETORNO] Resultado da operação.
                    return Json(
                        new
                        {
                            success = true ,
                            type = type ,
                        }
                    );
                }

                // [RETORNO] ID inválido.
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "UpdateStatus");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "UpdateStatus" , error);
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   LISTA LOTACAO - LISTAGEM DE LOTAÇÕES DO MOTORISTA   |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaLotacao (GET)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista lotações do motorista (ou vazia se não informado).                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • motoristaId (string): ID do motorista (opcional).                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lotações.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaLotacao")]
        public IActionResult ListaLotacao(string motoristaId)
        {
            try
            {
                // [DADOS] Consulta base.
                var result = _unitOfWork.ViewLotacaoMotorista.GetAll(lm => lm.MotoristaId == Guid.Empty);

                if (motoristaId != null)
                {
                    // [FILTRO] Filtra por motorista.
                    result = _unitOfWork.ViewLotacaoMotorista.GetAll(lm =>
                        lm.MotoristaId == Guid.Parse(motoristaId)
                    );
                }

                // [RETORNO] Resultado.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "ListaLotacao");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "ListaLotacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao listar lotações"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   LOTA MOTORISTA - CRIA NOVA LOTAÇÃO                  |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LotaMotorista (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cria nova lotação e atualiza unidade do motorista.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • MotoristaId (string): ID do motorista.                                 ║
        /// ║    • UnidadeId (string): ID da unidade.                                     ║
        /// ║    • DataInicio (string): Data início.                                      ║
        /// ║    • DataFim (string): Data fim (opcional).                                 ║
        /// ║    • Lotado (bool): Status de lotação.                                      ║
        /// ║    • Motivo (string): Motivo da lotação.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status e IDs.                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("LotaMotorista")]
        public IActionResult LotaMotorista(
            string MotoristaId ,
            string UnidadeId ,
            string DataInicio ,
            string DataFim ,
            bool Lotado ,
            string Motivo
        )
        {
            try
            {
                // [DADOS] Verifica se já existe lotação.
                var existeLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                    (lm.MotoristaId == Guid.Parse(MotoristaId))
                    && (lm.UnidadeId == Guid.Parse(UnidadeId))
                    && lm.DataInicio.ToString() == DataInicio
                );

                if (existeLotacao != null)
                {
                    // [RETORNO] Já existe lotação.
                    _notyf.Error("Já existe uma lotação com essas informações!" , 3);
                    return new JsonResult(new
                    {
                        data = "00000000-0000-0000-0000-000000000000"
                    });
                }

                // [ACAO] Cria lotação.
                var objLotacaoMotorista = new LotacaoMotorista();
                objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
                objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeId);
                objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicio);
                if (DataFim != null)
                {
                    objLotacaoMotorista.DataFim = DateTime.Parse(DataFim);
                }
                objLotacaoMotorista.Lotado = Lotado;
                objLotacaoMotorista.Motivo = Motivo;

                _unitOfWork.LotacaoMotorista.Add(objLotacaoMotorista);

                // [ACAO] Atualiza unidade do motorista.
                var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == Guid.Parse(MotoristaId)
                );
                obJMotorista.UnidadeId = Guid.Parse(UnidadeId);
                _unitOfWork.Motorista.Update(obJMotorista);

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();

                _log.Info($"Motorista {MotoristaId} lotado na Unidade {UnidadeId} com sucesso.", "UnidadeController", "LotaMotorista");

                // [RETORNO] Sucesso.
                return new JsonResult(
                    new
                    {
                        data = MotoristaId ,
                        message = "Lotação Adicionada com Sucesso" ,
                        lotacaoId = objLotacaoMotorista.LotacaoMotoristaId ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "LotaMotorista");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "LotaMotorista" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao lotar motorista"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   EDITA LOTAÇÃO - ALTERA DADOS DA LOTAÇÃO             |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: EditaLotacao (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Altera dados da lotação e unidade do motorista.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • LotacaoId (string): ID da lotação.                                     ║
        /// ║    • MotoristaId (string): ID do motorista.                                 ║
        /// ║    • UnidadeId (string): ID da unidade.                                     ║
        /// ║    • DataInicio (string): Data início.                                      ║
        /// ║    • DataFim (string): Data fim (opcional).                                 ║
        /// ║    • Lotado (bool): Status de lotação.                                      ║
        /// ║    • Motivo (string): Motivo da lotação.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("EditaLotacao")]
        public IActionResult EditaLotacao(
            string LotacaoId ,
            string MotoristaId ,
            string UnidadeId ,
            string DataInicio ,
            string DataFim ,
            bool Lotado ,
            string Motivo
        )
        {
            try
            {
                // [DADOS] Busca lotação.
                var objLotacaoMotorista = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                    (lm.LotacaoMotoristaId == Guid.Parse(LotacaoId))
                );

                // [ACAO] Atualiza lotação.
                objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
                objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeId);
                objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicio);

                if (DataFim != null)
                {
                    objLotacaoMotorista.DataFim = DateTime.Parse(DataFim);
                }
                else
                {
                    objLotacaoMotorista.DataFim = null;
                }

                objLotacaoMotorista.Lotado = Lotado;
                objLotacaoMotorista.Motivo = Motivo;
                _unitOfWork.LotacaoMotorista.Update(objLotacaoMotorista);

                // [ACAO] Atualiza unidade do motorista.
                var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == Guid.Parse(MotoristaId)
                );
                obJMotorista.UnidadeId = Guid.Parse(UnidadeId);
                _unitOfWork.Motorista.Update(obJMotorista);

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();

                _log.Info($"Lotação {LotacaoId} editada com sucesso para o Motorista {MotoristaId}.", "UnidadeController", "EditaLotacao");

                // [RETORNO] Sucesso.
                return new JsonResult(
                    new
                    {
                        data = MotoristaId ,
                        message = "Lotação Alterada com Sucesso"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "EditaLotacao");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "EditaLotacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar lotação"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   DELETE LOTAÇÃO - REMOVE LOTAÇÃO DO MOTORISTA        |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteLotacao (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove lotação do motorista e limpa a unidade.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): ID da lotação.                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DeleteLotacao")]
        [HttpGet]
        public IActionResult DeleteLotacao(string Id)
        {
            try
            {
                // [DADOS] Busca lotação.
                var objFromDb = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(u =>
                    u.LotacaoMotoristaId == Guid.Parse(Id)
                );

                // [ACAO] Remove lotação.
                var motoristaId = objFromDb.MotoristaId;
                _unitOfWork.LotacaoMotorista.Remove(objFromDb);
                _unitOfWork.Save();

                // [ACAO] Limpa unidade do motorista.
                var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == motoristaId
                );
                obJMotorista.UnidadeId = Guid.Empty;
                _unitOfWork.Motorista.Update(obJMotorista);
                _unitOfWork.Save();

                _log.Info($"Lotação {Id} removida para o Motorista {motoristaId}.", "UnidadeController", "DeleteLotacao");

                // [RETORNO] Sucesso.
                return Json(
                    new
                    {
                        success = true ,
                        message = "Lotação removida com sucesso" ,
                        motoristaId = motoristaId ,
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "DeleteLotacao");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "DeleteLotacao" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar lotação"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ATUALIZA MOTORISTA LOTAÇÃO ATUAL                    |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizaMotoristaLotacaoAtual (GET)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza lotação atual e cria nova lotação quando necessário.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • MotoristaId (string): ID do motorista.                                 ║
        /// ║    • UnidadeAtualId (string): ID da unidade atual.                           ║
        /// ║    • UnidadeNovaId (string): ID da nova unidade (opcional).                 ║
        /// ║    • DataFimLotacaoAnterior (string): Data fim da lotação anterior.         ║
        /// ║    • DataInicioNovoMotivo (string): Data início da nova lotação.            ║
        /// ║    • MotivoLotacaoAtual (string): Motivo da nova lotação.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("AtualizaMotoristaLotacaoAtual")]
        public IActionResult AtualizaMotoristaLotacaoAtual(
            string MotoristaId ,
            string UnidadeAtualId ,
            string UnidadeNovaId ,
            string DataFimLotacaoAnterior ,
            string DataInicioNovoMotivo ,
            string MotivoLotacaoAtual
        )
        {
            try
            {
                if (UnidadeNovaId == null)
                {
                    // [ACAO] Remove unidade do motorista.
                    var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                        m.MotoristaId == Guid.Parse(MotoristaId)
                    );
                    obJMotorista.UnidadeId = Guid.Empty;
                    _unitOfWork.Motorista.Update(obJMotorista);

                    // [DADOS] Busca lotação ativa.
                    var obJLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                        lm.UnidadeId == Guid.Parse(UnidadeAtualId)
                        && lm.MotoristaId == Guid.Parse(MotoristaId)
                        && lm.Lotado == true
                    );

                    if (obJLotacao != null)
                    {
                        // [ACAO] Encerra lotação.
                        obJLotacao.Lotado = false;
                        obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
                        _unitOfWork.LotacaoMotorista.Update(obJLotacao);
                    }
                }
                else if (UnidadeAtualId != UnidadeNovaId)
                {
                    // [ACAO] Atualiza unidade do motorista.
                    var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                        m.MotoristaId == Guid.Parse(MotoristaId)
                    );
                    obJMotorista.UnidadeId = Guid.Parse(UnidadeNovaId);
                    _unitOfWork.Motorista.Update(obJMotorista);

                    // [DADOS] Busca lotação ativa.
                    var obJLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                        lm.UnidadeId == Guid.Parse(UnidadeAtualId)
                        && lm.MotoristaId == Guid.Parse(MotoristaId)
                        && lm.Lotado == true
                    );

                    if (obJLotacao != null)
                    {
                        // [ACAO] Encerra lotação atual.
                        obJLotacao.Lotado = false;
                        obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
                        _unitOfWork.LotacaoMotorista.Update(obJLotacao);
                    }

                    // [ACAO] Cria nova lotação.
                    var objLotacaoMotorista = new LotacaoMotorista();
                    objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
                    objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeNovaId);
                    objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicioNovoMotivo);
                    objLotacaoMotorista.Lotado = true;
                    objLotacaoMotorista.Motivo = MotivoLotacaoAtual;
                    _unitOfWork.LotacaoMotorista.Update(objLotacaoMotorista);
                }

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();

                _log.Info($"Lotação atualizada para o Motorista {MotoristaId}.", "UnidadeController", "AtualizaMotoristaLotacaoAtual");

                // [RETORNO] Sucesso.
                return new JsonResult(
                    new
                    {
                        data = MotoristaId ,
                        message = "Remoção feita com Sucesso"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "AtualizaMotoristaLotacaoAtual");
                Alerta.TratamentoErroComLinha(
                    "UnidadeController.cs" ,
                    "AtualizaMotoristaLotacaoAtual" ,
                    error
                );
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao atualizar lotação"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   ALOCA MOTORISTA COBERTURA - FÉRIAS/COBERTURA        |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AlocaMotoristaCobertura (GET)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Realoca motorista e cria lotações de férias/cobertura.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • MotoristaId (string): ID do motorista titular.                         ║
        /// ║    • MotoristaCoberturaId (string): ID do motorista cobertura.              ║
        /// ║    • DataFimLotacao (string): Data fim da lotação do titular.               ║
        /// ║    • DataInicioLotacao (string): Data início da lotação do titular.         ║
        /// ║    • DataInicioCobertura (string): Data início da cobertura.                ║
        /// ║    • DataFimCobertura (string): Data fim da cobertura.                      ║
        /// ║    • UnidadeId (string): ID da unidade de cobertura.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("AlocaMotoristaCobertura")]
        public IActionResult AlocaMotoristaCobertura(
            string MotoristaId ,
            string MotoristaCoberturaId ,
            string DataFimLotacao ,
            string DataInicioLotacao ,
            string DataInicioCobertura ,
            string DataFimCobertura ,
            string UnidadeId
        )
        {
            try
            {
                // [ACAO] Desabilita motorista atual da sua lotação.
                var objMotoristaAtual = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                    (lm.MotoristaId == Guid.Parse(MotoristaId) && lm.Lotado == true)
                );

                if (objMotoristaAtual != null)
                {
                    objMotoristaAtual.DataFim = DateTime.Parse(DataFimLotacao);
                    objMotoristaAtual.Lotado = false;
                    objMotoristaAtual.Motivo = "Férias";
                    if (MotoristaCoberturaId != null)
                    {
                        objMotoristaAtual.MotoristaCoberturaId = Guid.Parse(MotoristaCoberturaId);
                    }
                    _unitOfWork.LotacaoMotorista.Update(objMotoristaAtual);
                }

                // [ACAO] Insere motorista atual em nova lotação.
                var objMotoristaLotacaoNova = new LotacaoMotorista();
                objMotoristaLotacaoNova.MotoristaId = Guid.Parse(MotoristaId);
                objMotoristaLotacaoNova.DataInicio = DateTime.Parse(DataInicioLotacao);
                objMotoristaLotacaoNova.DataFim = DateTime.Parse(DataFimLotacao);
                objMotoristaLotacaoNova.Lotado = true;
                objMotoristaLotacaoNova.Motivo = "Férias";
                if (MotoristaCoberturaId != null)
                {
                    objMotoristaLotacaoNova.MotoristaCoberturaId = Guid.Parse(MotoristaCoberturaId);
                }
                _unitOfWork.LotacaoMotorista.Add(objMotoristaLotacaoNova);

                // [ACAO] Remove motorista cobertura da lotação atual.
                if (MotoristaCoberturaId != null)
                {
                    var objCobertura = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                        (lm.MotoristaId == Guid.Parse(MotoristaCoberturaId) && lm.Lotado == true)
                    );
                    if (objCobertura != null)
                    {
                        objCobertura.DataFim = DateTime.Parse(DataInicioCobertura);
                        objCobertura.Lotado = false;
                        _unitOfWork.LotacaoMotorista.Update(objCobertura);
                    }
                }

                // [ACAO] Aloca motorista cobertura em nova lotação.
                if (MotoristaCoberturaId != null)
                {
                    var objLotacaoMotorista = new LotacaoMotorista();
                    objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaCoberturaId);
                    objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeId);
                    objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicioCobertura);
                    objLotacaoMotorista.DataFim = DateTime.Parse(DataFimCobertura);
                    objLotacaoMotorista.Lotado = true;
                    objLotacaoMotorista.Motivo = "Cobertura";
                    _unitOfWork.LotacaoMotorista.Add(objLotacaoMotorista);
                }

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();

                _log.Info($"Cobertura alocada para o Motorista {MotoristaId} com o Motorista {MotoristaCoberturaId}.", "UnidadeController", "AlocaMotoristaCobertura");

                // [RETORNO] Sucesso.
                return new JsonResult(
                    new
                    {
                        data = MotoristaId ,
                        message = "Remoção feita com Sucesso"
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "AlocaMotoristaCobertura");
                Alerta.TratamentoErroComLinha(
                    "UnidadeController.cs" ,
                    "AlocaMotoristaCobertura" ,
                    error
                );
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao alocar motorista cobertura"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   LISTA LOTAÇÕES - LISTAGEM GERAL DE LOTAÇÕES         |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaLotacoes (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista geral de lotações com filtro opcional por categoria.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • categoriaId (string): Categoria (opcional).                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lotações.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaLotacoes")]
        public IActionResult ListaLotacoes(string categoriaId)
        {
            try
            {
                // [DADOS] Consulta base.
                var result = _unitOfWork
                    .ViewLotacoes.GetAll()
                    .OrderBy(vl => vl.NomeCategoria)
                    .ThenBy(vl => vl.Unidade)
                    .ToList();

                if (categoriaId != null)
                {
                    // [FILTRO] Por categoria.
                    result = _unitOfWork
                        .ViewLotacoes.GetAll(vl => vl.NomeCategoria == categoriaId)
                        .OrderBy(O => O.NomeCategoria)
                        .ThenBy(vl => vl.Unidade)
                        .ToList();
                }

                // [RETORNO] Lista de lotações.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "ListaLotacoes");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "ListaLotacoes" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao listar lotações"
                });
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   DESATIVAR LOTAÇÕES - MÉDODO AUXILIAR                |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DesativarLotacoes (Helper)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Desativa lotações anteriores do motorista (exceto a atual).             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • motoristaId (string): ID do motorista.                                 ║
        /// ║    • lotacaoAtualId (Guid): ID da lotação atual.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void                                                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private void DesativarLotacoes(string motoristaId , Guid lotacaoAtualId)
        {
            try
            {
                // [DADOS] Lotações anteriores ativas.
                var lotacoesAnteriores = _unitOfWork.LotacaoMotorista.GetAll(lm =>
                    lm.MotoristaId == Guid.Parse(motoristaId)
                    && lm.Lotado == true
                );

                foreach (var lotacao in lotacoesAnteriores)
                {
                    if (lotacao.LotacaoMotoristaId == lotacaoAtualId)
                        continue;

                    // [ACAO] Desativa lotação.
                    lotacao.Lotado = false;
                    _unitOfWork.LotacaoMotorista.Update(lotacao);
                }

                // [ACAO] Persiste alterações.
                _unitOfWork.Save();
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "DesativarLotacoes");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "DesativarLotacoes" , error);
                return;
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   REMOVE LOTAÇÕES - DESATIVA LOTAÇÕES ANTERIORES      |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RemoveLotacoes (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Desativa lotações anteriores do motorista.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • motoristaId (string): ID do motorista.                                 ║
        /// ║    • lotacaoAtualId (Guid): ID da lotação atual.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da remoção.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("RemoveLotacoes")]
        public IActionResult RemoveLotacoes(string motoristaId , Guid lotacaoAtualId)
        {
            try
            {
                // [ACAO] Desativa lotações anteriores.
                DesativarLotacoes(motoristaId , lotacaoAtualId);

                _log.Info($"Lotações anteriores removidas para o Motorista {motoristaId}.", "UnidadeController", "RemoveLotacoes");

                // [RETORNO] Sucesso.
                return new JsonResult(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UnidadeController", "RemoveLotacoes");
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "RemoveLotacoes" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover lotações"
                });
            }
        }
    }
}
