/* ****************************************************************************************
 * ⚡ ARQUIVO: OcorrenciaViagemController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : CRUD de ocorrências de viagem (listar, criar, baixar, reabrir, excluir).
 *
 * 📥 ENTRADAS     : IDs e DTOs de ocorrência.
 *
 * 📤 SAÍDAS       : JSON com dados e status.
 *
 * 🔗 CHAMADA POR  : Páginas e integrações de ocorrências.
 *
 * 🔄 CHAMA        : IUnitOfWork, ViewOcorrenciasViagem.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrotiX.Repository.IRepository;
using FrotiX.Models;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: OcorrenciaViagemController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para gestão completa de ocorrências de viagem.
     *
     * 📥 ENTRADAS     : DTOs de ocorrência e IDs.
     *
     * 📤 SAÍDAS       : JSON com resultados.
     *
     * 🔗 CHAMADA POR  : Telas de ocorrências e integrações.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public partial class OcorrenciaViagemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: OcorrenciaViagemController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public OcorrenciaViagemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region LISTAR

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListarPorViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências associadas a uma viagem específica.
         *
         * 📥 ENTRADAS     : viagemId (Guid).
         *
         * 📤 SAÍDAS       : JSON com lista de ocorrências.
         *
         * 🔗 CHAMADA POR  : Tela de detalhes da viagem.
         *
         * 🔄 CHAMA        : ViewOcorrenciasViagem.GetAll().
         *
         * 📝 OBSERVAÇÕES  : Ordena por DataCriacao desc (mais recentes primeiro).
         ****************************************************************************************/
        [HttpGet]
        [Route("ListarPorViagem")]
        public IActionResult ListarPorViagem(Guid viagemId)
        {
            try
            {
                var ocorrencias = _unitOfWork.ViewOcorrenciasViagem
                    .GetAll(o => o.ViagemId == viagemId)
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId ,
                        o.ViagemId ,
                        o.VeiculoId ,
                        o.MotoristaId ,
                        o.Resumo ,
                        o.Descricao ,
                        o.ImagemOcorrencia ,
                        o.Status ,
                        DataCriacao = o.DataCriacao.ToString("dd/MM/yyyy HH:mm") ,
                        DataBaixa = o.DataBaixa.HasValue ? o.DataBaixa.Value.ToString("dd/MM/yyyy HH:mm") : "" ,
                        o.UsuarioCriacao ,
                        o.UsuarioBaixa ,
                        o.Placa ,
                        o.VeiculoCompleto ,
                        o.NomeMotorista ,
                        o.DiasEmAberto ,
                        o.Urgencia ,
                        o.CorUrgencia
                    })
                    .ToList();

                return Ok(new { success = true , data = ocorrencias });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao listar ocorrências: " + ex.Message });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListarAbertasPorVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar ocorrências abertas de um veículo específico (popup).
         *
         * 📥 ENTRADAS     : veiculoId (Guid).
         *
         * 📤 SAÍDAS       : JSON com ocorrências abertas.
         *
         * 🔗 CHAMADA POR  : Popups de veículo.
         *
         * 🔄 CHAMA        : ViewOcorrenciasAbertasVeiculo.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("ListarAbertasPorVeiculo")]
        public IActionResult ListarAbertasPorVeiculo(Guid veiculoId)
        {
            try
            {
                var ocorrencias = _unitOfWork.ViewOcorrenciasAbertasVeiculo
                    .GetAll(o => o.VeiculoId == veiculoId)
                    .OrderByDescending(o => o.DataCriacao)
                    .Select(o => new
                    {
                        o.OcorrenciaViagemId ,
                        o.ViagemId ,
                        o.VeiculoId ,
                        o.Resumo ,
                        o.Descricao ,
                        o.ImagemOcorrencia ,
                        DataCriacao = o.DataCriacao.ToString("dd/MM/yyyy HH:mm") ,
                        DataViagem = o.DataViagem.HasValue ? o.DataViagem.Value.ToString("dd/MM/yyyy") : "" ,
                        o.NoFichaVistoria ,
                        o.NomeMotorista ,
                        o.DiasEmAberto ,
                        o.Urgencia ,
                        o.CorUrgencia
                    })
                    .ToList();

                return Ok(new { success = true , data = ocorrencias });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao listar ocorrências abertas: " + ex.Message });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ContarAbertasPorVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Contar ocorrências abertas de um veículo.
         *
         * 📥 ENTRADAS     : veiculoId (Guid).
         *
         * 📤 SAÍDAS       : JSON com quantidade.
         *
         * 🔗 CHAMADA POR  : Indicadores de ocorrência por veículo.
         *
         * 🔄 CHAMA        : ViewOcorrenciasAbertasVeiculo.GetAll().
         ****************************************************************************************/
        [HttpGet]
        [Route("ContarAbertasPorVeiculo")]
        public IActionResult ContarAbertasPorVeiculo(Guid veiculoId)
        {
            try
            {
                var count = _unitOfWork.ViewOcorrenciasAbertasVeiculo
                    .GetAll(o => o.VeiculoId == veiculoId)
                    .Count();

                return Ok(new { success = true , count = count });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao contar ocorrências: " + ex.Message });
            }
        }

        #endregion LISTAR

        #region CRIAR

        /****************************************************************************************
         * ⚡ FUNÇÃO: Criar
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar nova ocorrência de viagem
         * 📥 ENTRADAS     : [OcorrenciaViagemDTO] dto - Dados da ocorrência
         * 📤 SAÍDAS       : [JSON] { success, message, id }
         * 🔗 CHAMADA POR  : Tela de finalização de viagem ou gestão de ocorrências
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.Add
         * 📦 DEPENDÊNCIAS : Tabela OcorrenciaViagem
         *
         * [DOC] Status inicial: "Aberta"
         * [DOC] UsuarioCriacao: User.Identity.Name ou "Sistema" se não autenticado
         ****************************************************************************************/
        [HttpPost]
        [Route("Criar")]
        public IActionResult Criar([FromBody] OcorrenciaViagemDTO dto)
        {
            try
            {
                var ocorrencia = new OcorrenciaViagem
                {
                    OcorrenciaViagemId = Guid.NewGuid() ,
                    ViagemId = dto.ViagemId ,
                    VeiculoId = dto.VeiculoId ,
                    MotoristaId = dto.MotoristaId != Guid.Empty ? dto.MotoristaId : null ,
                    Resumo = dto.Resumo ?? "" ,
                    Descricao = dto.Descricao ?? "" ,
                    ImagemOcorrencia = dto.ImagemOcorrencia ?? "" ,
                    Status = "Aberta" ,
                    DataCriacao = DateTime.Now ,
                    UsuarioCriacao = User.Identity?.Name ?? "Sistema"
                };

                _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                _unitOfWork.Save();

                return Ok(new { success = true , message = "Ocorrência criada com sucesso!" , id = ocorrencia.OcorrenciaViagemId });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao criar ocorrência: " + ex.Message });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CriarMultiplas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar múltiplas ocorrências em lote (finalização de viagem).
         *
         * 📥 ENTRADAS     : [List<OcorrenciaViagemDTO>] dtos.
         *
         * 📤 SAÍDAS       : JSON com quantidade criada ou erro.
         *
         * 🔗 CHAMADA POR  : Finalização de viagem com múltiplas ocorrências.
         *
         * 🔄 CHAMA        : OcorrenciaViagem.Add(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("CriarMultiplas")]
        public IActionResult CriarMultiplas([FromBody] List<OcorrenciaViagemDTO> dtos)
        {
            try
            {
                var criadas = 0;
                foreach (var dto in dtos)
                {
                    var ocorrencia = new OcorrenciaViagem
                    {
                        OcorrenciaViagemId = Guid.NewGuid() ,
                        ViagemId = dto.ViagemId ,
                        VeiculoId = dto.VeiculoId ,
                        MotoristaId = dto.MotoristaId != Guid.Empty ? dto.MotoristaId : null ,
                        Resumo = dto.Resumo ?? "" ,
                        Descricao = dto.Descricao ?? "" ,
                        ImagemOcorrencia = dto.ImagemOcorrencia ?? "" ,
                        Status = "Aberta" ,
                        DataCriacao = DateTime.Now ,
                        UsuarioCriacao = User.Identity?.Name ?? "Sistema"
                    };

                    _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
                    criadas++;
                }

                _unitOfWork.Save();

                return Ok(new { success = true , message = $"{criadas} ocorrência(s) criada(s) com sucesso!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao criar ocorrências: " + ex.Message });
            }
        }

        #endregion CRIAR

        #region ATUALIZAR STATUS

        /****************************************************************************************
         * ⚡ FUNÇÃO: DarBaixa
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Dar baixa em ocorrência (marcar como resolvida)
         * 📥 ENTRADAS     : [Guid] ocorrenciaId - ID da ocorrência
         * 📤 SAÍDAS       : [JSON] { success, message }
         * 🔗 CHAMADA POR  : Tela de gestão de ocorrências
         * 🔄 CHAMA        : _unitOfWork.OcorrenciaViagem.Update
         * 📦 DEPENDÊNCIAS : Tabela OcorrenciaViagem
         *
         * [DOC] Atualiza Status para "Baixada", registra DataBaixa e UsuarioBaixa
         ****************************************************************************************/
        [HttpPost]
        [Route("DarBaixa")]
        public IActionResult DarBaixa(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                ocorrencia.Status = "Baixada";
                ocorrencia.DataBaixa = DateTime.Now;
                ocorrencia.UsuarioBaixa = User.Identity?.Name ?? "Sistema";

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                return Ok(new { success = true , message = "Ocorrência baixada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao dar baixa: " + ex.Message });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Reabrir
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Reabrir ocorrência baixada.
         *
         * 📥 ENTRADAS     : ocorrenciaId (Guid).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ação de reabertura.
         *
         * 🔄 CHAMA        : OcorrenciaViagem.Update(), Save().
         ****************************************************************************************/
        [HttpPost]
        [Route("Reabrir")]
        public IActionResult Reabrir(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                ocorrencia.Status = "Aberta";
                ocorrencia.DataBaixa = null;
                ocorrencia.UsuarioBaixa = "";

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                return Ok(new { success = true , message = "Ocorrência reaberta com sucesso!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao reabrir: " + ex.Message });
            }
        }

        #endregion ATUALIZAR STATUS

        #region EXCLUIR

        /****************************************************************************************
         * ⚡ FUNÇÃO: Excluir
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Excluir ocorrência pelo ID.
         *
         * 📥 ENTRADAS     : ocorrenciaId (Guid).
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de exclusão.
         *
         * 🔄 CHAMA        : OcorrenciaViagem.Remove(), Save().
         ****************************************************************************************/
        [HttpDelete]
        [Route("Excluir")]
        public IActionResult Excluir(Guid ocorrenciaId)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == ocorrenciaId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                _unitOfWork.OcorrenciaViagem.Remove(ocorrencia);
                _unitOfWork.Save();

                return Ok(new { success = true , message = "Ocorrência excluída com sucesso!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao excluir: " + ex.Message });
            }
        }

        #endregion EXCLUIR

        #region ATUALIZAR

        /****************************************************************************************
         * ⚡ FUNÇÃO: Atualizar
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Atualizar dados de ocorrência existente.
         *
         * 📥 ENTRADAS     : [OcorrenciaViagemDTO] dto.
         *
         * 📤 SAÍDAS       : JSON com sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Edição de ocorrência.
         *
         * 🔄 CHAMA        : OcorrenciaViagem.Update(), Save().
         ****************************************************************************************/
        [HttpPut]
        [Route("Atualizar")]
        public IActionResult Atualizar([FromBody] OcorrenciaViagemDTO dto)
        {
            try
            {
                var ocorrencia = _unitOfWork.OcorrenciaViagem.GetFirstOrDefault(o => o.OcorrenciaViagemId == dto.OcorrenciaViagemId);
                if (ocorrencia == null)
                    return Ok(new { success = false , message = "Ocorrência não encontrada." });

                ocorrencia.Resumo = dto.Resumo ?? ocorrencia.Resumo;
                ocorrencia.Descricao = dto.Descricao ?? ocorrencia.Descricao;
                ocorrencia.ImagemOcorrencia = !string.IsNullOrEmpty(dto.ImagemOcorrencia) ? dto.ImagemOcorrencia : ocorrencia.ImagemOcorrencia;
                ocorrencia.Observacoes = dto.Observacoes ?? ocorrencia.Observacoes;

                if (dto.ItemManutencaoId != Guid.Empty)
                    ocorrencia.ItemManutencaoId = dto.ItemManutencaoId;

                _unitOfWork.OcorrenciaViagem.Update(ocorrencia);
                _unitOfWork.Save();

                return Ok(new { success = true , message = "Ocorrência atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro ao atualizar: " + ex.Message });
            }
        }

        #endregion ATUALIZAR

        #region UPLOAD IMAGEM

        /****************************************************************************************
         * ⚡ FUNÇÃO: UploadImagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Upload de imagem/vídeo da ocorrência.
         *
         * 📥 ENTRADAS     : arquivo (IFormFile).
         *
         * 📤 SAÍDAS       : JSON com URL relativa do arquivo.
         *
         * 🔗 CHAMADA POR  : Upload de mídia em ocorrência.
         *
         * 🔄 CHAMA        : FileStream, Directory.CreateDirectory().
         ****************************************************************************************/
        [HttpPost]
        [Route("UploadImagem")]
        public async Task<IActionResult> UploadImagem(IFormFile arquivo)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return Ok(new { success = false , message = "Nenhum arquivo enviado." });

                var extensao = Path.GetExtension(arquivo.FileName).ToLower();
                var extensoesPermitidas = new[] { ".jpg" , ".jpeg" , ".png" , ".gif" , ".webp" , ".mp4" , ".webm" };

                if (!extensoesPermitidas.Contains(extensao))
                    return Ok(new { success = false , message = "Tipo de arquivo não permitido." });

                var pastaUpload = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot" , "uploads" , "ocorrencias");
                if (!Directory.Exists(pastaUpload))
                    Directory.CreateDirectory(pastaUpload);

                var nomeArquivo = Guid.NewGuid().ToString() + extensao;
                var caminhoCompleto = Path.Combine(pastaUpload , nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto , FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                var urlRelativa = "/uploads/ocorrencias/" + nomeArquivo;

                return Ok(new { success = true , url = urlRelativa });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false , message = "Erro no upload: " + ex.Message });
            }
        }

        #endregion UPLOAD IMAGEM
    }

    /****************************************************************************************
     * ⚡ DTO: OcorrenciaViagemDTO
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados de ocorrência de viagem entre camadas.
     *
     * 📥 ENTRADAS     : IDs (Ocorrencia/Viagem/Veículo/Motorista/ItemManutencao) e campos
     *                   descritivos (Resumo, Descricao, ImagemOcorrencia, Observacoes).
     *
     * 📤 SAÍDAS       : Nenhuma (apenas transporte de dados).
     *
     * 🔗 CHAMADA POR  : Criar, CriarMultiplas, Atualizar.
     ****************************************************************************************/
    public class OcorrenciaViagemDTO
    {
        public Guid OcorrenciaViagemId { get; set; }
        public Guid ViagemId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid MotoristaId { get; set; }
        public string? Resumo { get; set; }
        public string? Descricao { get; set; }
        public string? ImagemOcorrencia { get; set; }
        public string? Observacoes { get; set; }
        public Guid ItemManutencaoId { get; set; }
    }
}
