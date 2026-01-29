/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : OcorrenciaViagemController.Listar.cs                            ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Partial class para listagem e verificacao de ocorrencias (modal, veiculo,    ║
║ exclusao). Operacoes de consulta e remocao.                                  ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET  /api/OcorrenciaViagem/ListarPorViagem  : Lista por viagem (modal)     ║
║ - GET  /api/OcorrenciaViagem/VerificarVeiculo : Verifica por veiculo         ║
║ - POST /api/OcorrenciaViagem/Excluir          : Exclui ocorrencia            ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 21          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

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
     * 🎯 OBJETIVO     : Métodos para listagem e verificação de ocorrências (modal, veículo, exclusão)
     * 📥 ENTRADAS     : IDs de viagem/veículo/ocorrência
     * 📤 SAÍDAS       : JsonResult com lista de ocorrências ou status de operação
     * 🔗 CHAMADA POR  : Modais de viagem, verificações de veículo, exclusões
     * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem
     * 📦 DEPENDÊNCIAS : Repository Pattern, Alerta.TratamentoErroComLinha
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

    /// <summary>
    /// DTO para exclusão de ocorrência
    /// </summary>
    public class ExcluirOcorrenciaDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
    }
}

