/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Veículos (Core Stack)             |
 * |_____________________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de ativos da frota,
 * manutenção de status operacionais, vínculos contratuais e técnicos.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: VeiculoController                                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de veículos, contratos e status operacionais.                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Veiculo                                               ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VeiculoController (Construtor)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public VeiculoController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("VeiculoController.cs", "VeiculoController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista veículos com dados reduzidos (ViewVeiculos).                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com listagem de veículos.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Obtém dados reduzidos de veículos.
                var objVeiculos = _unitOfWork
                    .ViewVeiculos.GetAllReduced(selector: vv => new
                    {
                        vv.VeiculoId ,
                        vv.Placa ,
                        vv.Quilometragem ,
                        vv.MarcaModelo ,
                        vv.Sigla ,
                        vv.Descricao ,
                        vv.Consumo ,
                        vv.OrigemVeiculo ,
                        vv.DataAlteracao ,
                        vv.NomeCompleto ,
                        vv.VeiculoReserva ,
                        vv.Status ,
                        vv.CombustivelId ,
                        vv.VeiculoProprio ,
                    })
                    .ToList(); // [DADOS] Converte o resultado para uma lista

                // [RETORNO] Lista de veículos.
                return Json(new
                {
                    data = objVeiculos
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "Get");
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "Get" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar dados"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove veículo se não houver vínculos com contratos/viagens.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (VeiculoViewModel): Dados com ID do veículo.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(VeiculoViewModel model)
        {
            try
            {
                // [VALIDACAO] Modelo e ID.
                if (model != null && model.VeiculoId != Guid.Empty)
                {
                    // [DADOS] Busca veículo.
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == model.VeiculoId
                    );
                    // [VALIDACAO] Veículo encontrado.
                    if (objFromDb != null)
                    {
                        // ═══════════════════════════════════════════════════════════════
                        // 🔹 BLOCO: Verificação de Vínculos de Exclusão (Contratos e Viagens)
                        // Verifica a existência de registros relacionados que impediriam
                        // a exclusão direta do veículo para manter a integridade dos dados.
                        // ═══════════════════════════════════════════════════════════════

                        // [REGRA] Verifica vínculo com contrato.
                        var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u =>
                            u.VeiculoId == model.VeiculoId
                        );
                        if (veiculoContrato != null)
                        {
                            // [RETORNO] Bloqueia exclusão por contrato.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o veículo. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        // [REGRA] Verifica vínculo com viagem.
                        var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(u =>
                            u.VeiculoId == model.VeiculoId
                        );
                        if (objViagem != null)
                        {
                            // [RETORNO] Bloqueia exclusão por viagem.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o veículo. Ele está associado a uma ou mais viagens!" ,
                                }
                            );
                        }

                        // [ACAO] Remove veículo.
                        _unitOfWork.Veiculo.Remove(objFromDb);
                        _unitOfWork.Save(); // Salva as mudanças no banco

                        // [LOG] Registro de remoção.
                        _log.Info($"Veículo removido com sucesso: {objFromDb.Placa} (ID: {model.VeiculoId})", "VeiculoController", "Delete");

                        // [RETORNO] Sucesso.
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Veículo removido com sucesso"
                            }
                        );
                    }
                }
                // [RETORNO] Falha padrão.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar veículo"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "Delete");
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "Delete" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar veículo"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusVeiculo (POST)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna o status (ativo/inativo) do veículo.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do veículo.                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusVeiculo")]
        public JsonResult UpdateStatusVeiculo(Guid Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Busca veículo.
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.VeiculoId == Id);
                    string Description = ""; // Descrição da alteração para retorno
                    int type = 0; // 0 = Ativo, 1 = Inativo

                    // [REGRA] Se veículo encontrado.
                    if (objFromDb != null)
                    {
                        // [REGRA] Alterna status.
                        objFromDb.Status = !objFromDb.Status;
                        type = objFromDb.Status ? 0 : 1; // Define o tipo para retorno (0=Ativo, 1=Inativo)
                        Description = string.Format(
                            "Atualizado Status do Veículo [Placa: {0}] ({1})" ,
                            objFromDb.Placa,
                            objFromDb.Status ? "Ativo" : "Inativo"
                        );

                        // [ACAO] Persiste alterações.
                        _unitOfWork.Veiculo.Update(objFromDb);
                        _unitOfWork.Save(); // Salva as mudanças

                        // [LOG] Registro de alteração.
                        _log.Info(Description, "VeiculoController", "UpdateStatusVeiculo");
                    }
                    // [RETORNO] Resultado da operação.
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
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
                _log.Error("Erro", error, "VeiculoController", "UpdateStatusVeiculo");
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "UpdateStatusVeiculo" , error);
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VeiculoContratos (GET)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista veículos associados a um contrato.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do contrato.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com veículos do contrato.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("VeiculoContratos")]
        public IActionResult VeiculoContratos(Guid Id)
        {
            try
            {
                // [DADOS] Consulta com joins.
                var result = (
                    from v in _unitOfWork.Veiculo.GetAll() // Busca todos os veículos
                    join vc in _unitOfWork.VeiculoContrato.GetAll() // Junta com os vínculos veículo-contrato
                        on v.VeiculoId equals vc.VeiculoId
                    join m in _unitOfWork.ModeloVeiculo.GetAll() on v.ModeloId equals m.ModeloId // Junta com modelos
                    join ma in _unitOfWork.MarcaVeiculo.GetAll() on v.MarcaId equals ma.MarcaId // Junta com marcas
                    join u in _unitOfWork.Unidade.GetAll() // Junta com unidades (left join)
                        on v.UnidadeId equals u.UnidadeId
                        into ud
                    from udResult in ud.DefaultIfEmpty() // Permite veículos sem unidade
                    join c in _unitOfWork.Combustivel.GetAll() // Junta com combustíveis
                        on v.CombustivelId equals c.CombustivelId
                    where vc.ContratoId == Id // [FILTRO] Contrato
                    select new // [DADOS] Seleciona as propriedades desejadas em um objeto anônimo
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo , // Concatena marca e modelo
                        Sigla = udResult != null ? udResult.Sigla : "" , // Obtém sigla da unidade ou vazio
                        CombustivelDescricao = c.Descricao ,
                        v.Status ,
                    }
                ).ToList(); // Converte o resultado para uma lista

                // [RETORNO] Lista de veículos.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "VeiculoContratos");
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculoContratos" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar veículos do contrato"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VeiculosDoContrato (GET)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista veículos do contrato elegíveis para glosa.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do contrato.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: View com veículos.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("VeiculoContratosGlosa")]
        public IActionResult VeiculosDoContrato(Guid id)
        {
            try
            {
                // [DADOS] Manutenções para filtro.
                var manutencoes = _unitOfWork.Manutencao.GetAll();
                // [REGRA] Veículos elegíveis por duração.
                var veiculosElegiveis = new HashSet<Guid>(
                    manutencoes
                        .Where(m =>
                            m.VeiculoId.HasValue
                            && m.DataSolicitacao.HasValue
                            && m.DataDevolucao.HasValue
                            && (m.DataDevolucao.Value.Date - m.DataSolicitacao.Value.Date).TotalDays
                                > 0
                        )
                        .Select(m => m.VeiculoId.Value)
                        .Distinct()
                );

                // [DADOS] Vínculos veículo-contrato.
                var veiculosContrato = _unitOfWork
                    .VeiculoContrato.GetAll()
                    .Where(vc => vc.ContratoId == id);

                // [DADOS] Dados auxiliares.
                var veiculos = _unitOfWork.Veiculo.GetAll();
                var modelos = _unitOfWork.ModeloVeiculo.GetAll();
                var marcas = _unitOfWork.MarcaVeiculo.GetAll();
                var unidades = _unitOfWork.Unidade.GetAll();
                var combustiveis = _unitOfWork.Combustivel.GetAll();

                // [PROCESSAMENTO] JOINs e filtro de glosa.
                var result = (
                    from vc in veiculosContrato
                    where vc != null && veiculosElegiveis.Contains(vc?.VeiculoId ?? Guid.Empty) // [REGRA] Filtra por veículos do contrato e elegíveis
                    join v in veiculos on vc.VeiculoId equals v.VeiculoId
                    join m in modelos on v.ModeloId equals m.ModeloId
                    join ma in marcas on v.MarcaId equals ma.MarcaId
                    join u in unidades on v.UnidadeId equals u.UnidadeId into ud
                    from udResult in ud.DefaultIfEmpty()
                    join c in combustiveis on v.CombustivelId equals c.CombustivelId
                    select new // [DADOS] Seleciona e formata as propriedades do resultado
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        MarcaModelo = ma.DescricaoMarca + "/" + m.DescricaoModelo ,
                        Sigla = udResult != null ? udResult.Sigla : "" ,
                        CombustivelDescricao = c.Descricao ,
                        v.Status ,
                    }
                ).ToList(); // Converte o resultado para uma lista

                // [RETORNO] View com veículos.
                return View(result);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "VeiculosDoContrato");
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "VeiculosDoContrato" , error);
                // [RETORNO] View fallback.
                return View();
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteContrato (POST)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove vínculo veículo-contrato.                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (VeiculoViewModel): ID do veículo e contrato.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(VeiculoViewModel model)
        {
            try
            {
                // [VALIDACAO] Modelo e ID.
                if (model != null && model.VeiculoId != Guid.Empty)
                {
                    // [DADOS] Busca veículo.
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == model.VeiculoId
                    );
                    // [VALIDACAO] Veículo encontrado.
                    if (objFromDb != null)
                    {
                        // [DADOS] Busca vínculo veículo-contrato.
                        var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u =>
                            u.VeiculoId == model.VeiculoId && u.ContratoId == model.ContratoId
                        );
                        // [VALIDACAO] Vínculo existente.
                        if (veiculoContrato != null)
                        {
                            // [REGRA] Se contrato for principal.
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                // [ACAO] Reseta contrato principal.
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Veiculo.Update(objFromDb); // Atualiza o veículo
                            }
                            // [ACAO] Remove vínculo.
                            _unitOfWork.VeiculoContrato.Remove(veiculoContrato);
                            _unitOfWork.Save(); // Salva as mudanças

                            // [LOG] Registro de remoção.
                            _log.Info($"Vínculo de contrato removido para o veículo: {objFromDb.Placa} (ID Contrato: {model.ContratoId})", "VeiculoController", "DeleteContrato");

                            // [RETORNO] Sucesso.
                            return Json(
                                new
                                {
                                    success = true ,
                                    message = "Veículo removido com sucesso"
                                }
                            );
                        }
                        // [RETORNO] Vínculo não encontrado.
                        return Json(new
                        {
                            success = false ,
                            message = "Erro ao remover veículo"
                        });
                    }
                    // [RETORNO] Veículo não encontrado.
                    return Json(new
                    {
                        success = false ,
                        message = "Erro ao remover veículo"
                    });
                }
                // [RETORNO] Modelo inválido.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover veículo"
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "DeleteContrato");
                Alerta.TratamentoErroComLinha("VeiculoController.cs" , "DeleteContrato" , error);
                // [RETORNO] Erro.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar contrato"
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SelecionaValorMensalAta (GET)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna valor mensal do item de ata.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • itemAta (Guid): ID do item de ata.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Valor unitário.                                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("SelecionaValorMensalAta")]
        [HttpGet]
        public JsonResult SelecionaValorMensalAta(Guid itemAta)
        {
            try
            {
                // [DADOS] Busca item da ata.
                var ItemAta = _unitOfWork.ItemVeiculoAta.GetFirstOrDefault(i =>
                    i.ItemVeiculoAtaId == itemAta
                );

                // [RETORNO] Valor unitário.
                return new JsonResult(new
                {
                    valor = ItemAta.ValorUnitario
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "SelecionaValorMensalAta");
                Alerta.TratamentoErroComLinha(
                    "VeiculoController.cs" ,
                    "SelecionaValorMensalAta" ,
                    error
                );
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SelecionaValorMensalContrato (GET)                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna valor mensal do item de contrato.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • itemContrato (Guid): ID do item do contrato.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Valor unitário.                                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("SelecionaValorMensalContrato")]
        [HttpGet]
        public JsonResult SelecionaValorMensalContrato(Guid itemContrato)
        {
            try
            {
                // [DADOS] Busca item do contrato.
                var ItemContrato = _unitOfWork.ItemVeiculoContrato.GetFirstOrDefault(i =>
                    i.ItemVeiculoId == itemContrato
                );

                // [RETORNO] Valor unitário.
                return new JsonResult(new
                {
                    valor = ItemContrato.ValorUnitario
                });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "VeiculoController", "SelecionaValorMensalContrato");
                Alerta.TratamentoErroComLinha(
                    "VeiculoController.cs" ,
                    "SelecionaValorMensalContrato" ,
                    error
                );
                // [RETORNO] Erro.
                return new JsonResult(new
                {
                    success = false
                });
            }
        }
    }
}
