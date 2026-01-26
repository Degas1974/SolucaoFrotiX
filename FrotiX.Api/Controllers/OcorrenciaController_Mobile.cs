using FrotiXApi.Models;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FrotiXApi.Controllers
{
    /// <summary>
    /// Endpoints adicionais para o Mobile - Adicionar ao OcorrenciaController.cs
    /// </summary>
    public partial class OcorrenciaController : Controller
    {
        #region ========== ENDPOINTS MOBILE ==========

        /// <summary>
        /// Lista ocorrências abertas de um veículo (Mobile)
        /// GET /api/ocorrencia/veiculo/{veiculoId}/abertas
        /// </summary>
        [Route("veiculo/{veiculoId}/abertas")]
        [HttpGet]
        public IActionResult GetOcorrenciasAbertasVeiculo(Guid veiculoId)
        {
            try
            {
                // StatusOcorrencia: true ou null = Aberta, false = Baixada
                var ocorrencias = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.VeiculoId == veiculoId && (o.StatusOcorrencia == true || o.StatusOcorrencia == null))
                    .OrderByDescending(o => o.DataCriacao)
                    .ToList();

                return Json(ocorrencias);
            }
            catch (Exception ex)
            {
                return Json(new List<OcorrenciaViagem>());
            }
        }

        /// <summary>
        /// Conta ocorrências abertas de um veículo (Mobile)
        /// GET /api/ocorrencia/veiculo/{veiculoId}/contar
        /// </summary>
        [Route("veiculo/{veiculoId}/contar")]
        [HttpGet]
        public IActionResult ContarOcorrenciasAbertasVeiculo(Guid veiculoId)
        {
            try
            {
                // StatusOcorrencia: true ou null = Aberta, false = Baixada
                var count = _unitOfWork.OcorrenciaViagem
                    .GetAll(o => o.VeiculoId == veiculoId && (o.StatusOcorrencia == true || o.StatusOcorrencia == null))
                    .Count();

                return Json(count);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        /// <summary>
        /// Cria uma nova ocorrência (Mobile)
        /// POST /api/ocorrencia/criar
        /// </summary>
        [Route("criar")]
        [HttpPost]
        public IActionResult CriarOcorrencia([FromBody] OcorrenciaViagem ocorrencia)
        {
            try
            {
                if (ocorrencia == null)
                    return Json(false);

                if (ocorrencia.OcorrenciaViagemId == Guid.Empty)
                    ocorrencia.OcorrenciaViagemId = Guid.NewGuid();

                if (ocorrencia.DataCriacao == default)
                    ocorrencia.DataCriacao = DateTime.Now;

                // StatusOcorrencia: true = Aberta
                ocorrencia.StatusOcorrencia = true;
                ocorrencia.Status = "Aberta";

                _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                _unitOfWork.Save();

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        /// <summary>
        /// Cria múltiplas ocorrências de uma vez (Mobile)
        /// POST /api/ocorrencia/criar-multiplas
        /// </summary>
        [Route("criar-multiplas")]
        [HttpPost]
        public IActionResult CriarOcorrenciasMultiplas([FromBody] CriarOcorrenciasDTO dto)
        {
            try
            {
                if (dto?.Ocorrencias == null || dto.Ocorrencias.Count == 0)
                    return Json(0);

                int count = 0;
                foreach (var item in dto.Ocorrencias)
                {
                    var ocorrencia = new OcorrenciaViagem
                    {
                        OcorrenciaViagemId = Guid.NewGuid(),
                        ViagemId = dto.ViagemId,
                        VeiculoId = dto.VeiculoId,
                        MotoristaId = dto.MotoristaId,
                        Resumo = item.Resumo ?? "",
                        Descricao = item.Descricao ?? "",
                        ImagemOcorrencia = item.ImagemOcorrencia ?? "",
                        DataCriacao = DateTime.Now,
                        StatusOcorrencia = true,
                        Status = "Aberta"
                    };

                    _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                    count++;
                }

                _unitOfWork.Save();
                return Json(count);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        /// <summary>
        /// Dá baixa em uma ocorrência (Mobile)
        /// POST /api/ocorrencia/baixar-mobile
        /// </summary>
        [Route("baixar-mobile")]
        [HttpPost]
        public IActionResult BaixarOcorrenciaMobile([FromBody] BaixarOcorrenciaDTO dto)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaId);

                if (ocorrencia == null)
                    return Json(false);

                // StatusOcorrencia: false = Baixada
                ocorrencia.StatusOcorrencia = false;
                ocorrencia.Status = "Baixada";
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.Solucao = dto.DescricaoBaixa ?? "";

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        /// <summary>
        /// Exclui uma ocorrência (Mobile)
        /// DELETE /api/ocorrencia/{ocorrenciaId}
        /// </summary>
        [Route("{ocorrenciaId}")]
        [HttpDelete]
        public IActionResult ExcluirOcorrencia(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem
                    .GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);

                if (ocorrencia == null)
                    return Json(false);

                _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                _unitOfWork.Save();

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        #endregion
    }

    #region ========== DTOs ==========

    /// <summary>
    /// DTO para criação de múltiplas ocorrências
    /// </summary>
    public class CriarOcorrenciasDTO
    {
        public Guid ViagemId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid? MotoristaId { get; set; }
        public List<OcorrenciaItemDTO> Ocorrencias { get; set; } = new();
    }

    /// <summary>
    /// DTO para item de ocorrência
    /// </summary>
    public class OcorrenciaItemDTO
    {
        public string Resumo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? ImagemOcorrencia { get; set; }
    }

    /// <summary>
    /// DTO para baixar ocorrência
    /// </summary>
    public class BaixarOcorrenciaDTO
    {
        public Guid OcorrenciaId { get; set; }
        public string DescricaoBaixa { get; set; } = string.Empty;
    }

    #endregion
}