using System;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrotiXApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VistoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FrotiXDbContext _db;

        public VistoriasController(IUnitOfWork unitOfWork, FrotiXDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        /// <summary>
        /// GET /api/vistorias
        /// Retorna todas as vistorias com dados completos para listagem
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await (
                    from v in _db.Viagem.AsNoTracking()
                    join ve in _db.Veiculo.AsNoTracking() on v.VeiculoId equals ve.VeiculoId into veJoin
                    from ve in veJoin.DefaultIfEmpty()
                    join ma in _db.MarcaVeiculo.AsNoTracking() on ve.MarcaId equals ma.MarcaId into maJoin
                    from ma in maJoin.DefaultIfEmpty()
                    join mo in _db.ModeloVeiculo.AsNoTracking() on ve.ModeloId equals mo.ModeloId into moJoin
                    from mo in moJoin.DefaultIfEmpty()
                    join m in _db.Motorista.AsNoTracking() on v.MotoristaId equals m.MotoristaId into mJoin
                    from m in mJoin.DefaultIfEmpty()
                    orderby v.DataInicial descending, v.HoraInicio descending
                    select new ViewViagemVistoriaDto
                    {
                        ViagemId = v.ViagemId,
                        DataInicial = v.DataInicial,
                        HoraInicio = v.HoraInicio,
                        DataFinal = v.DataFinal,
                        Placa = ve != null ? ve.Placa : null,
                        VeiculoCompleto = ve != null ? $"{ve.Placa} - {(ma != null ? ma.DescricaoMarca : "")} {(mo != null ? mo.DescricaoModelo : "")}" : null,
                        NomeMotorista = m != null ? m.Nome : null,
                        MotoristaCondutor = m != null ? m.Nome : null,
                        Origem = v.Origem,
                        Destino = v.Destino,
                        Rubrica = v.Rubrica,
                        Status = v.Status,
                        StatusAgendamento = v.StatusAgendamento,
                        NoFichaVistoria = v.NoFichaVistoria
                    }
                ).Take(500).ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao carregar vistorias: {ex.Message}");
            }
        }

        /// <summary>
        /// GET /api/vistorias/abertas
        /// Retorna apenas vistorias com status "Aberta" para fechamento
        /// </summary>
        [HttpGet("abertas")]
        public async Task<IActionResult> GetAbertas()
        {
            try
            {
                var items = await (
                    from v in _db.Viagem.AsNoTracking().Where(x => x.Status == "Aberta" && x.StatusAgendamento == false)
                    join ve in _db.Veiculo.AsNoTracking() on v.VeiculoId equals ve.VeiculoId into veJoin
                    from ve in veJoin.DefaultIfEmpty()
                    join ma in _db.MarcaVeiculo.AsNoTracking() on ve.MarcaId equals ma.MarcaId into maJoin
                    from ma in maJoin.DefaultIfEmpty()
                    join mo in _db.ModeloVeiculo.AsNoTracking() on ve.ModeloId equals mo.ModeloId into moJoin
                    from mo in moJoin.DefaultIfEmpty()
                    join m in _db.Motorista.AsNoTracking() on v.MotoristaId equals m.MotoristaId into mJoin
                    from m in mJoin.DefaultIfEmpty()
                    orderby v.DataInicial descending, v.HoraInicio descending
                    select new ViewViagemVistoriaDto
                    {
                        ViagemId = v.ViagemId,
                        DataInicial = v.DataInicial,
                        HoraInicio = v.HoraInicio,
                        DataFinal = v.DataFinal,
                        Placa = ve != null ? ve.Placa : null,
                        VeiculoCompleto = ve != null ? $"{ve.Placa} - {(ma != null ? ma.DescricaoMarca : "")} {(mo != null ? mo.DescricaoModelo : "")}" : null,
                        NomeMotorista = m != null ? m.Nome : null,
                        MotoristaCondutor = m != null ? m.Nome : null,
                        Origem = v.Origem,
                        Destino = v.Destino,
                        Rubrica = v.Rubrica,
                        Status = v.Status,
                        StatusAgendamento = v.StatusAgendamento,
                        NoFichaVistoria = v.NoFichaVistoria,
                        Finalidade = v.Finalidade
                    }
                ).ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao carregar vistorias abertas: {ex.Message}");
            }
        }

        /// <summary>
        /// GET /api/vistorias/{id}
        /// Retorna a entidade Viagem completa (para edição/finalização)
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var viagem = await _db.Viagem.AsNoTracking().FirstOrDefaultAsync(x => x.ViagemId == id);
            if (viagem is null) 
                return NotFound();

            // Buscar nomes de usuário
            string? nomeUsuarioCriacao = null;
            string? nomeUsuarioAgendamento = null;

            if (!string.IsNullOrEmpty(viagem.UsuarioIdCriacao))
            {
                nomeUsuarioCriacao = await _db.AspNetUsers.AsNoTracking()
                    .Where(u => u.Id == viagem.UsuarioIdCriacao)
                    .Select(u => u.NomeCompleto)
                    .FirstOrDefaultAsync();
            }

            if (!string.IsNullOrEmpty(viagem.UsuarioIdAgendamento))
            {
                nomeUsuarioAgendamento = await _db.AspNetUsers.AsNoTracking()
                    .Where(u => u.Id == viagem.UsuarioIdAgendamento)
                    .Select(u => u.NomeCompleto)
                    .FirstOrDefaultAsync();
            }

            // Retorna viagem com campos adicionais de nome
            return Ok(new ViagemComNomesDto(viagem, nomeUsuarioCriacao, nomeUsuarioAgendamento));
        }

        /// <summary>
        /// POST /api/vistorias
        /// Cria uma nova vistoria
        /// </summary>
        [HttpPost]
        public IActionResult Post([FromBody] Viagem viagem)
        {
            try
            {
                if (viagem is null)
                    return BadRequest("Corpo da requisição vazio.");

                // Gera novo ID se não informado
                if (viagem.ViagemId == Guid.Empty)
                    viagem.ViagemId = Guid.NewGuid();

                // Define status baseado na presença da data final
                viagem.Status = viagem.DataFinal == null ? "Aberta" : "Realizada";

                _unitOfWork.Viagem.Add(viagem);
                _unitOfWork.Save();

                return CreatedAtAction(nameof(GetById), new { id = viagem.ViagemId }, viagem.ViagemId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar vistoria: {ex.Message}");
            }
        }

        /// <summary>
        /// PUT /api/vistorias/{id}
        /// Atualiza uma vistoria existente
        /// </summary>
        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] Viagem viagem)
        {
            try
            {
                if (viagem is null || id != viagem.ViagemId)
                    return BadRequest("ID inconsistente ou dados inválidos.");

                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar vistoria: {ex.Message}");
            }
        }

        /// <summary>
        /// DELETE /api/vistorias/{id}
        /// Remove uma vistoria
        /// </summary>
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == id);
                if (viagem is null)
                    return NotFound();

                _unitOfWork.Viagem.Remove(viagem);
                _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir vistoria: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// DTO para listagem de vistorias
    /// </summary>
    public sealed class ViewViagemVistoriaDto
    {
        public Guid ViagemId { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public string? Placa { get; set; }
        public string? VeiculoCompleto { get; set; }
        public string? NomeMotorista { get; set; }
        public string? MotoristaCondutor { get; set; }
        public string? Origem { get; set; }
        public string? Destino { get; set; }
        public string? Rubrica { get; set; }
        public string? Status { get; set; }
        public bool? StatusAgendamento { get; set; }
        public int? NoFichaVistoria {get; set;}
        public string? Finalidade { get; set; }
    }

    /// <summary>
    /// DTO wrapper para retornar Viagem com nomes de usuário
    /// Herda dinamicamente todos os campos da Viagem e adiciona os nomes
    /// </summary>
    public sealed class ViagemComNomesDto
    {
        private readonly Viagem _viagem;

        public ViagemComNomesDto(Viagem viagem, string? nomeUsuarioCriacao, string? nomeUsuarioAgendamento)
        {
            _viagem = viagem;
            NomeUsuarioCriacao = nomeUsuarioCriacao;
            NomeUsuarioAgendamento = nomeUsuarioAgendamento;
        }

        // Campos adicionais de nome
        public string? NomeUsuarioCriacao { get; }
        public string? NomeUsuarioAgendamento { get; }

        // Todos os campos da Viagem expostos
        public Guid ViagemId => _viagem.ViagemId;
        public Guid? VeiculoId => _viagem.VeiculoId;
        public Guid? SetorSolicitanteId => _viagem.SetorSolicitanteId;
        public Guid? RequisitanteId => _viagem.RequisitanteId;
        public Guid? MotoristaId => _viagem.MotoristaId;
        public Guid? EventoId => _viagem.EventoId;
        public Guid? ItemManutencaoId => _viagem.ItemManutencaoId;
        public string? Origem => _viagem.Origem;
        public string? Destino => _viagem.Destino;
        public string? RamalRequisitante => _viagem.RamalRequisitante;
        public string? Finalidade => _viagem.Finalidade;
        public string? Descricao => _viagem.Descricao;
        public string? DescricaoSemFormato => _viagem.DescricaoSemFormato;
        public string? NomeEvento => _viagem.NomeEvento;
        public DateTime? DataInicial => _viagem.DataInicial;
        public DateTime? DataFinal => _viagem.DataFinal;
        public DateTime? HoraInicio => _viagem.HoraInicio;
        public DateTime? HoraFim => _viagem.HoraFim;
        public DateTime? DataAgendamento => _viagem.DataAgendamento;
        public DateTime? DataCancelamento => _viagem.DataCancelamento;
        public DateTime? DataCriacao => _viagem.DataCriacao;
        public DateTime? DataFinalizacao => _viagem.DataFinalizacao;
        public DateTime? DataFinalRecorrencia => _viagem.DataFinalRecorrencia;
        public int? Minutos => _viagem.Minutos;
        public int? KmAtual => _viagem.KmAtual;
        public int? KmInicial => _viagem.KmInicial;
        public int? KmFinal => _viagem.KmFinal;
        public int? NoFichaVistoria => _viagem.NoFichaVistoria;
        public double? CustoCombustivel => _viagem.CustoCombustivel;
        public double? CustoLavador => _viagem.CustoLavador;
        public double? CustoMotorista => _viagem.CustoMotorista;
        public double? CustoVeiculo => _viagem.CustoVeiculo;
        public double? CustoOperador => _viagem.CustoOperador;
        public string? Status => _viagem.Status;
        public bool? StatusAgendamento => _viagem.StatusAgendamento;
        public string? StatusCartaoAbastecimento => _viagem.StatusCartaoAbastecimento;
        public string? StatusCartaoAbastecimentoFinal => _viagem.StatusCartaoAbastecimentoFinal;
        public string? StatusDocumento => _viagem.StatusDocumento;
        public string? StatusDocumentoFinal => _viagem.StatusDocumentoFinal;
        public string? StatusOcorrencia => _viagem.StatusOcorrencia;
        public bool? CintaEntregue => _viagem.CintaEntregue;
        public bool? CintaDevolvida => _viagem.CintaDevolvida;
        public bool? TabletEntregue => _viagem.TabletEntregue;
        public bool? TabletDevolvido => _viagem.TabletDevolvido;
        public string? VistoriadorInicialId => _viagem.VistoriadorInicialId;
        public string? VistoriadorFinalId => _viagem.VistoriadorFinalId;
        public string? Rubrica => _viagem.Rubrica;
        public string? DanoAvaria => _viagem.DanoAvaria;
        public string? NivelCombustivelInicial => _viagem.NivelCombustivelInicial;
        public byte[]? FotosBase64 => _viagem.FotosBase64;
        public byte[]? VideosBase64 => _viagem.VideosBase64;
        public string? RubricaFinal => _viagem.RubricaFinal;
        public string? DanoAvariaFinal => _viagem.DanoAvariaFinal;
        public string? NivelCombustivelFinal => _viagem.NivelCombustivelFinal;
        public byte[]? FotosFinaisBase64 => _viagem.FotosFinaisBase64;
        public byte[]? VideosFinaisBase64 => _viagem.VideosFinaisBase64;
        public string? DescricaoOcorrencia => _viagem.DescricaoOcorrencia;
        public string? DescricaoSolucaoOcorrencia => _viagem.DescricaoSolucaoOcorrencia;
        public string? ResumoOcorrencia => _viagem.ResumoOcorrencia;
        public string? ImagemOcorrencia => _viagem.ImagemOcorrencia;
        public byte[]? DescricaoViagemImagem => _viagem.DescricaoViagemImagem;
        public byte[]? DescricaoViagemWord => _viagem.DescricaoViagemWord;
        public string? Recorrente => _viagem.Recorrente;
        public Guid? RecorrenciaViagemId => _viagem.RecorrenciaViagemId;
        public string? Intervalo => _viagem.Intervalo;
        public int? DiaMesRecorrencia => _viagem.DiaMesRecorrencia;
        public bool? Monday => _viagem.Monday;
        public bool? Tuesday => _viagem.Tuesday;
        public bool? Wednesday => _viagem.Wednesday;
        public bool? Thursday => _viagem.Thursday;
        public bool? Friday => _viagem.Friday;
        public bool? Saturday => _viagem.Saturday;
        public bool? Sunday => _viagem.Sunday;
        public string? UsuarioIdAgendamento => _viagem.UsuarioIdAgendamento;
        public string? UsuarioIdCancelamento => _viagem.UsuarioIdCancelamento;
        public string? UsuarioIdCriacao => _viagem.UsuarioIdCriacao;
        public string? UsuarioIdFinalizacao => _viagem.UsuarioIdFinalizacao;
        public bool? FoiAgendamento => _viagem.FoiAgendamento;
    }
}
