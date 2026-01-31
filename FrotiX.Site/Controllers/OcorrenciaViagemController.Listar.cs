/* ****************************************************************************************
 * ⚡ ARQUIVO: OcorrenciaViagemController.Listar.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Listar e verificar ocorrências de viagem (modal, veículo, exclusão).
 *
 * 📥 ENTRADAS     : IDs de viagem, veículo e ocorrência.
 *
 * 📤 SAÍDAS       : JSON com listas e status das operações.
 *
 * 🔗 CHAMADA POR  : Modais de viagem e verificações de disponibilidade.
 *
 * 🔄 CHAMA        : IUnitOfWork.OcorrenciaViagem.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: OcorrenciaViagemController.Listar
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Métodos para listagem e verificação de ocorrências.
     *
     * 📥 ENTRADAS     : IDs de viagem/veículo/ocorrência.
     *
     * 📤 SAÍDAS       : JSON com listas e status.
     *
     * 🔗 CHAMADA POR  : Modais e verificações de veículo.
     ****************************************************************************************/

    public partial class OcorrenciaViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: ListarOcorrenciasModal
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista todas ocorrências de uma viagem para exibição em modal
         * 📥 ENTRADAS     : viagemId (Guid)
         * 📤 SAÍDAS       : JSON com success, data (array de ocorrências), total
         * 🔗 CHAMADA POR  : Modal de detalhes de viagem via GET /ListarOcorrenciasModal?viagemId=X
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.GetAll()
         * 📦 DEPENDÊNCIAS : LINQ, Alerta.TratamentoErroComLinha
         ****************************************************************************************/
        [HttpGet]
        [Route("ListarOcorrenciasModal")]
        public IActionResult ListarOcorrenciasModal(Guid viagemId)
        {
            try
            {
                if (viagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID da viagem não informado"
                    });
                }

                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.ViagemId == viagemId)
                    .OrderBy(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId,
                        o.ViagemId,
                        o.Resumo,
                        o.Descricao,
                        o.ImagemOcorrencia,
                        o.DataCriacao,
                        o.Status,
                        o.StatusOcorrencia
                    })
                    .ToList();

                return new JsonResult(new
                {
                    success = true,
                    data = ocorrencias,
                    total = ocorrencias.Count
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarOcorrenciasModal", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListarOcorrenciasVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista ocorrências abertas/pendentes de um veículo específico
         * 📥 ENTRADAS     : veiculoId (Guid)
         * 📤 SAÍDAS       : JSON com success, data (array de ocorrências), total, temOcorrencias
         * 🔗 CHAMADA POR  : Verificação de disponibilidade de veículo via GET /ListarOcorrenciasVeiculo
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.GetAll()
         * 📦 DEPENDÊNCIAS : LINQ, Alerta.TratamentoErroComLinha
         * 📝 OBSERVAÇÃO   : [DOC] Filtra apenas ocorrências com StatusOcorrencia=true e Status=Aberta/Pendente
         ****************************************************************************************/
        [HttpGet]
        [Route("ListarOcorrenciasVeiculo")]
        public IActionResult ListarOcorrenciasVeiculo(Guid veiculoId)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID do veículo não informado"
                    });
                }

                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.VeiculoId == veiculoId 
                              && o.StatusOcorrencia == true
                              && (o.Status == "Aberta" || o.Status == "Pendente"))
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId,
                        o.ViagemId,
                        o.VeiculoId,
                        o.Resumo,
                        o.Descricao,
                        o.ImagemOcorrencia,
                        o.DataCriacao,
                        o.Status,
                        o.StatusOcorrencia
                    })
                    .ToList();

                return new JsonResult(new
                {
                    success = true,
                    data = ocorrencias,
                    total = ocorrencias.Count,
                    temOcorrencias = ocorrencias.Count > 0
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ListarOcorrenciasVeiculo", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao listar ocorrências: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VerificarOcorrenciasVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Verifica se veículo possui ocorrências abertas/pendentes (sem retornar lista)
         * 📥 ENTRADAS     : veiculoId (Guid)
         * 📤 SAÍDAS       : JSON com success, quantidade, temOcorrencias (boolean)
         * 🔗 CHAMADA POR  : Validação rápida antes de alocar veículo via GET /VerificarOcorrenciasVeiculo
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.GetAll().Count()
         * 📦 DEPENDÊNCIAS : LINQ, Alerta.TratamentoErroComLinha
         ****************************************************************************************/
        [HttpGet]
        [Route("VerificarOcorrenciasVeiculo")]
        public IActionResult VerificarOcorrenciasVeiculo(Guid veiculoId)
        {
            try
            {
                if (veiculoId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID do veículo não informado"
                    });
                }

                var quantidade = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.VeiculoId == veiculoId 
                              && (o.Status == "Aberta" || o.Status == "Pendente"))
                    .Count();

                return new JsonResult(new
                {
                    success = true,
                    quantidade = quantidade,
                    temOcorrencias = quantidade > 0
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "VerificarOcorrenciasVeiculo", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao verificar ocorrências: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExcluirOcorrencia
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remove permanentemente uma ocorrência do banco de dados
         * 📥 ENTRADAS     : ExcluirOcorrenciaDTO (OcorrenciaViagemId)
         * 📤 SAÍDAS       : JSON com success e message
         * 🔗 CHAMADA POR  : Botão "Excluir" em modais/grids via POST /ExcluirOcorrencia
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem (GetFirstOrDefault, Remove)
         * 📦 DEPENDÊNCIAS : Alerta.TratamentoErroComLinha
         * ⚠️  ATENÇÃO     : Exclusão permanente, sem soft delete
         ****************************************************************************************/
        [HttpPost]
        [Route("ExcluirOcorrencia")]
        public IActionResult ExcluirOcorrencia([FromBody] ExcluirOcorrenciaDTO dto)
        {
            try
            {
                if (dto == null || dto.OcorrenciaViagemId == Guid.Empty)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "ID da ocorrência não informado"
                    });
                }

                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);

                if (ocorrencia == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Ocorrência não encontrada"
                    });
                }

                _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                _unitOfWork.Save();

                return new JsonResult(new
                {
                    success = true,
                    message = "Ocorrência excluída com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OcorrenciaViagemController.cs", "ExcluirOcorrencia", error);
                return new JsonResult(new
                {
                    success = false,
                    message = "Erro ao excluir ocorrência: " + error.Message
                });
            }
        }
    }

    /****************************************************************************************
     * ⚡ DTO: ExcluirOcorrenciaDTO
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar o ID da ocorrência de viagem a ser excluída.
     *
     * 📥 ENTRADAS     : OcorrenciaViagemId.
     *
     * 📤 SAÍDAS       : Nenhuma (apenas transporte de dados).
     *
     * 🔗 CHAMADA POR  : ExcluirOcorrencia (POST /ExcluirOcorrencia).
     ****************************************************************************************/
    public class ExcluirOcorrenciaDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
    }
}
