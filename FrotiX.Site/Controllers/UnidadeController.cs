/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: UnidadeController.cs                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Unidade API
     * 🎯 OBJETIVO: Gerenciar unidades e lotação de motoristas em unidades
     * 📋 ROTAS: /api/Unidade/* (Get, Delete, UpdateStatus, ListaLotacao, LotaMotorista, etc)
     * 🔗 ENTIDADES: Unidade, LotacaoMotorista, Motorista, ViewLotacaoMotorista, ViewLotacoes
     * 📦 DEPENDÊNCIAS: IUnitOfWork, INotyfService (toast notifications)
     * 📝 FUNCIONALIDADES: CRUD unidades + Sistema completo de lotação de motoristas
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;

        public UnidadeController(IUnitOfWork unitOfWork , INotyfService notyf)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _notyf = notyf;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "UnidadeController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * 🎯 OBJETIVO: Listar todas as unidades do sistema
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { data: List<Unidade> }
         * 🔗 CHAMADA POR: Grid de unidades
         * 🔄 CHAMA: Unidade.GetAll()
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Json(new
                {
                    data = _unitOfWork.Unidade.GetAll()
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "Get" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * 🎯 OBJETIVO: Excluir unidade (valida se há veículos associados)
         * 📥 ENTRADAS: model (UnidadeViewModel com UnidadeId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de unidade
         * 🔄 CHAMA: Unidade.GetFirstOrDefault(), Veiculo.GetFirstOrDefault(), Unidade.Remove()
         * ⚠️ VALIDAÇÃO: Impede exclusão se existirem veículos associados
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(UnidadeViewModel model)
        {
            try
            {
                if (model != null && model.UnidadeId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Unidade.GetFirstOrDefault(u =>
                        u.UnidadeId == model.UnidadeId
                    );
                    if (objFromDb != null)
                    {
                        // [DOC] Valida integridade referencial: não permite excluir unidade com veículos
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.UnidadeId == model.UnidadeId
                        );
                        if (veiculo != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem veículos associados a essa unidade" ,
                                }
                            );
                        }
                        _unitOfWork.Unidade.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Unidade removida com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Unidade"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar unidade"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatus
         * 🎯 OBJETIVO: Alternar status da unidade (Ativo ↔ Inativo)
         * 📥 ENTRADAS: Id (UnidadeId Guid)
         * 📤 SAÍDAS: JSON { success, type (0=ativo, 1=inativo) }
         * 🔗 CHAMADA POR: Toggle de status no grid
         * 🔄 CHAMA: Unidade.GetFirstOrDefault(), Unidade.Update()
         * 📝 NOTA: Mensagem de description existe mas está comentada no retorno
         ****************************************************************************************/
        [Route("UpdateStatus")]
        public JsonResult UpdateStatus(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Unidade.GetFirstOrDefault(u => u.UnidadeId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [DOC] Toggle status: true → false (type=1) ou false → true (type=0)
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status da Unidade [Nome: {0}] (Inativo)" ,
                                objFromDb.Descricao
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status da Unidade  [Nome: {0}] (Ativo)" ,
                                objFromDb.Descricao
                            );
                            type = 0;
                        }
                        _unitOfWork.Unidade.Update(objFromDb);
                    }
                    return Json(
                        new
                        {
                            success = true ,
                            //message = Description ,
                            type = type ,
                        }
                    );
                }
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "UpdateStatus" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaLotacao
         * 🎯 OBJETIVO: Listar lotações de um motorista específico (ou vazia se não informado)
         * 📥 ENTRADAS: motoristaId (string Guid - opcional)
         * 📤 SAÍDAS: JSON { data: registros da ViewLotacaoMotorista }
         * 🔗 CHAMADA POR: Grid de histórico de lotações do motorista
         * 🔄 CHAMA: ViewLotacaoMotorista.GetAll()
         * 🔍 FILTRO: Se motoristaId null, retorna lista vazia (Guid.Empty)
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaLotacao")]
        public IActionResult ListaLotacao(string motoristaId)
        {
            try
            {
                // [DOC] Se motoristaId não informado, retorna lista vazia usando Guid.Empty
                var result = _unitOfWork.ViewLotacaoMotorista.GetAll(lm => lm.MotoristaId == Guid.Empty);

                if (motoristaId != null)
                {
                    result = _unitOfWork.ViewLotacaoMotorista.GetAll(lm =>
                        lm.MotoristaId == Guid.Parse(motoristaId)
                    );
                }

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "ListaLotacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao listar lotações"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: LotaMotorista
         * 🎯 OBJETIVO: Criar nova lotação de motorista em unidade
         * 📥 ENTRADAS: MotoristaId, UnidadeId, DataInicio, DataFim?, Lotado, Motivo (strings)
         * 📤 SAÍDAS: JSON { data: MotoristaId, message, lotacaoId } ou "00000000..." se duplicado
         * 🔗 CHAMADA POR: Modal de lotação de motorista
         * 🔄 CHAMA: LotacaoMotorista.GetFirstOrDefault(), LotacaoMotorista.Add(), Motorista.Update()
         * ⚠️ VALIDAÇÃO: Impede criar lotação duplicada (mesmo motorista+unidade+data)
         * 💾 OPERAÇÃO: Cria lotação E atualiza UnidadeId do motorista simultaneamente
         ****************************************************************************************/
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
                // [DOC] Valida duplicidade: mesma lotação não pode existir duas vezes
                var existeLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                    (lm.MotoristaId == Guid.Parse(MotoristaId))
                    && (lm.UnidadeId == Guid.Parse(UnidadeId))
                    && lm.DataInicio.ToString() == DataInicio
                );
                if (existeLotacao != null)
                {
                    _notyf.Error("Já existe uma lotação com essas informações!" , 3);
                    return new JsonResult(new
                    {
                        data = "00000000-0000-0000-0000-000000000000"
                    });
                }

                // [DOC] Cria registro de lotação
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

                // [DOC] Atualiza unidade do motorista (sincronização)
                var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == Guid.Parse(MotoristaId)
                );
                obJMotorista.UnidadeId = Guid.Parse(UnidadeId);
                _unitOfWork.Motorista.Update(obJMotorista);

                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "LotaMotorista" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao lotar motorista"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EditaLotacao
         * 🎯 OBJETIVO: Editar lotação existente de motorista
         * 📥 ENTRADAS: LotacaoId, MotoristaId, UnidadeId, DataInicio, DataFim?, Lotado, Motivo
         * 📤 SAÍDAS: JSON { data: MotoristaId, message }
         * 🔗 CHAMADA POR: Modal de edição de lotação
         * 🔄 CHAMA: LotacaoMotorista.GetFirstOrDefault(), LotacaoMotorista.Update(), Motorista.Update()
         * 💾 OPERAÇÃO: Atualiza lotação E unidade do motorista simultaneamente
         * 🗑️ LÓGICA: DataFim pode ser null (lotação em aberto)
         ****************************************************************************************/
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
                var objLotacaoMotorista = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                    (lm.LotacaoMotoristaId == Guid.Parse(LotacaoId))
                );
                objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
                objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeId);
                objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicio);
                // [DOC] DataFim opcional: null indica lotação em aberto
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

                // [DOC] Sincroniza unidade do motorista
                var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == Guid.Parse(MotoristaId)
                );
                obJMotorista.UnidadeId = Guid.Parse(UnidadeId);
                _unitOfWork.Motorista.Update(obJMotorista);

                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "EditaLotacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao editar lotação"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteLotacao
         * 🎯 OBJETIVO: Remover lotação de motorista
         * 📥 ENTRADAS: Id (LotacaoMotoristaId string)
         * 📤 SAÍDAS: JSON { success, message, motoristaId }
         * 🔗 CHAMADA POR: Modal de exclusão de lotação
         * 🔄 CHAMA: LotacaoMotorista.GetFirstOrDefault(), LotacaoMotorista.Remove(), Motorista.Update()
         * 💾 OPERAÇÃO: Remove lotação E limpa UnidadeId do motorista (Guid.Empty)
         ****************************************************************************************/
        [Route("DeleteLotacao")]
        [HttpGet]
        public IActionResult DeleteLotacao(string Id)
        {
            try
            {
                var objFromDb = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(u =>
                    u.LotacaoMotoristaId == Guid.Parse(Id)
                );
                var motoristaId = objFromDb.MotoristaId;
                _unitOfWork.LotacaoMotorista.Remove(objFromDb);
                _unitOfWork.Save();

                // [DOC] Limpa unidade do motorista após remover lotação
                var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                    m.MotoristaId == motoristaId
                );
                obJMotorista.UnidadeId = Guid.Empty;
                _unitOfWork.Motorista.Update(obJMotorista);

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
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "DeleteLotacao" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar lotação"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizaMotoristaLotacaoAtual
         * 🎯 OBJETIVO: Atualizar lotação atual do motorista (remoção ou transferência)
         * 📥 ENTRADAS: MotoristaId, UnidadeAtualId, UnidadeNovaId?, DataFimLotacaoAnterior, DataInicioNovoMotivo, MotivoLotacaoAtual
         * 📤 SAÍDAS: JSON { data: MotoristaId, message }
         * 🔗 CHAMADA POR: Modal de atualização de lotação
         * 🔄 CHAMA: Motorista.Update(), LotacaoMotorista.GetFirstOrDefault(), LotacaoMotorista.Update()
         * 📝 LÓGICA: Se UnidadeNovaId null → remove (Guid.Empty); Se diferente → transfere
         ****************************************************************************************/
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
                // [DOC] Cenário 1: Remover de unidade (UnidadeNovaId = null)
                if (UnidadeNovaId == null)
                {
                    var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                        m.MotoristaId == Guid.Parse(MotoristaId)
                    );
                    obJMotorista.UnidadeId = Guid.Empty;
                    _unitOfWork.Motorista.Update(obJMotorista);

                    var obJLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                        lm.UnidadeId == Guid.Parse(UnidadeAtualId)
                    );
                    obJLotacao.Lotado = false;
                    obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
                    _unitOfWork.LotacaoMotorista.Update(obJLotacao);
                }
                // [DOC] Cenário 2: Transferir para outra unidade
                else if (UnidadeAtualId != UnidadeNovaId)
                {
                    var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m =>
                        m.MotoristaId == Guid.Parse(MotoristaId)
                    );
                    obJMotorista.UnidadeId = Guid.Parse(UnidadeNovaId);
                    _unitOfWork.Motorista.Update(obJMotorista);

                    // [DOC] Finaliza lotação anterior
                    var obJLotacao = _unitOfWork.LotacaoMotorista.GetFirstOrDefault(lm =>
                        lm.UnidadeId == Guid.Parse(UnidadeAtualId)
                    );
                    obJLotacao.Lotado = false;
                    obJLotacao.DataFim = DateTime.Parse(DataFimLotacaoAnterior);
                    _unitOfWork.LotacaoMotorista.Update(obJLotacao);

                    // [DOC] Cria nova lotação
                    var objLotacaoMotorista = new LotacaoMotorista();
                    objLotacaoMotorista.MotoristaId = Guid.Parse(MotoristaId);
                    objLotacaoMotorista.UnidadeId = Guid.Parse(UnidadeNovaId);
                    objLotacaoMotorista.DataInicio = DateTime.Parse(DataInicioNovoMotivo);
                    objLotacaoMotorista.Lotado = true;
                    objLotacaoMotorista.Motivo = MotivoLotacaoAtual;
                    _unitOfWork.LotacaoMotorista.Update(objLotacaoMotorista);
                }

                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha(
                    "UnidadeController.cs" ,
                    "AtualizaMotoristaLotacaoAtual" ,
                    error
                );
                return Json(new
                {
                    success = false ,
                    message = "Erro ao atualizar lotação"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AlocaMotoristaCobertura
         * 🎯 OBJETIVO: Alocar motorista de cobertura durante férias de outro motorista
         * 📥 ENTRADAS: MotoristaId, MotoristaCoberturaId?, DataFimLotacao, DataInicioLotacao, DataInicioCobertura, DataFimCobertura, UnidadeId
         * 📤 SAÍDAS: JSON { data: MotoristaId, message }
         * 🔗 CHAMADA POR: Modal de gestão de férias/cobertura
         * 🔄 CHAMA: LotacaoMotorista.GetFirstOrDefault(), LotacaoMotorista.Update/Add()
         * 📝 LÓGICA COMPLEXA:
         *    1. Finaliza lotação atual do motorista (Férias)
         *    2. Cria nova lotação para motorista em férias
         *    3. Remove motorista cobertura da lotação atual
         *    4. Aloca motorista cobertura na unidade
         ****************************************************************************************/
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
                // [DOC] Passo 1: Desabilita Motorista Atual da Sua Locacao
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

                // [DOC] Passo 2: Insere Motorista Atual em Nova Locacao (Férias)
                var objMotoristaLotacaoNova = new LotacaoMotorista();
                objMotoristaLotacaoNova.MotoristaId = Guid.Parse(MotoristaId);
                objMotoristaLotacaoNova.DataInicio = DateTime.Parse(DataInicioLotacao);
                objMotoristaLotacaoNova.DataFim = DateTime.Parse(DataFimLotacao);
                objMotoristaLotacaoNova.Lotado = true;
                objMotoristaLotacaoNova.Motivo = "Férias";
                if (MotoristaCoberturaId != null)
                {
                    objMotoristaAtual.MotoristaCoberturaId = Guid.Parse(MotoristaCoberturaId);
                }
                _unitOfWork.LotacaoMotorista.Add(objMotoristaLotacaoNova);

                // [DOC] Passo 3: Remove Motorista Cobertura da Lotação Atual
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

                // [DOC] Passo 4: Aloca Motorista em Nova Lotação (Cobertura)
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

                _unitOfWork.Save();

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
                Alerta.TratamentoErroComLinha(
                    "UnidadeController.cs" ,
                    "AlocaMotoristaCobertura" ,
                    error
                );
                return Json(new
                {
                    success = false ,
                    message = "Erro ao alocar motorista cobertura"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaLotacoes
         * 🎯 OBJETIVO: Listar todas as lotações do sistema (com filtro opcional por categoria)
         * 📥 ENTRADAS: categoriaId (string - opcional)
         * 📤 SAÍDAS: JSON { data: registros da ViewLotacoes ordenados por categoria e unidade }
         * 🔗 CHAMADA POR: Grid de lotações gerenciais
         * 🔄 CHAMA: ViewLotacoes.GetAll()
         * 📊 ORDENAÇÃO: NomeCategoria → Unidade
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaLotacoes")]
        public IActionResult ListaLotacoes(string categoriaId)
        {
            try
            {
                var result = _unitOfWork
                    .ViewLotacoes.GetAll()
                    .OrderBy(vl => vl.NomeCategoria)
                    .ThenBy(vl => vl.Unidade)
                    .ToList();

                // [DOC] Filtro opcional por categoria de motorista
                if (categoriaId != null)
                {
                    result = _unitOfWork
                        .ViewLotacoes.GetAll(vl => vl.NomeCategoria == categoriaId)
                        .OrderBy(O => O.NomeCategoria)
                        .ThenBy(vl => vl.Unidade)
                        .ToList();
                }

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "ListaLotacoes" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao listar lotações"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DesativarLotacoes (Helper privado)
         * 🎯 OBJETIVO: Desativar todas as lotações antigas de um motorista exceto a atual
         * 📥 ENTRADAS: motoristaId (string), lotacaoAtualId (Guid)
         * 📤 SAÍDAS: void
         * 🔗 CHAMADA POR: RemoveLotacoes()
         * 🔄 CHAMA: LotacaoMotorista.GetAll(), LotacaoMotorista.Update()
         * 💾 LÓGICA: Define Lotado = false em todas as lotações ativas exceto a lotacaoAtualId
         ****************************************************************************************/
        private void DesativarLotacoes(string motoristaId , Guid lotacaoAtualId)
        {
            try
            {
                // [DOC] Busca todas lotações ativas do motorista
                var lotacoesAnteriores = _unitOfWork.LotacaoMotorista.GetAll(lm =>
                    lm.MotoristaId == Guid.Parse(motoristaId)
                    && lm.Lotado == true
                );
                foreach (var lotacao in lotacoesAnteriores)
                {
                    // [DOC] Preserva lotação atual, desativa as demais
                    if (lotacao.LotacaoMotoristaId == lotacaoAtualId)
                    {
                        continue;
                    }
                    else
                    {
                        lotacao.Lotado = false;
                        _unitOfWork.LotacaoMotorista.Update(lotacao);
                    }
                }
                _unitOfWork.Save();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "DesativarLotacoes" , error);
                return;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RemoveLotacoes
         * 🎯 OBJETIVO: Remover lotações antigas de motorista mantendo apenas a atual
         * 📥 ENTRADAS: motoristaId (string), lotacaoAtualId (Guid)
         * 📤 SAÍDAS: JSON { success }
         * 🔗 CHAMADA POR: Limpeza de lotações duplicadas/antigas
         * 🔄 CHAMA: DesativarLotacoes()
         ****************************************************************************************/
        [HttpGet]
        [Route("RemoveLotacoes")]
        public IActionResult RemoveLotacoes(string motoristaId , Guid lotacaoAtualId)
        {
            try
            {
                DesativarLotacoes(motoristaId , lotacaoAtualId);
                return new JsonResult(new
                {
                    success = true
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UnidadeController.cs" , "RemoveLotacoes" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover lotações"
                });
            }
        }
    }
}
