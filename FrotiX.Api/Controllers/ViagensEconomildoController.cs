using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FrotiXApi.Models;
using FrotiXApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FrotiXApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViagensEconomildoController : ControllerBase
    {
        private readonly FrotiXDbContext _context;

        public ViagensEconomildoController(FrotiXDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/viagenseconomildo
        /// Retorna todas as viagens
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViagensEconomildo>>> GetViagensEconomildo(
            [FromQuery] DateTime? dataInicio ,
            [FromQuery] DateTime? dataFim)
        {
            try
            {
                var query = _context.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .AsQueryable();

                // Filtros opcionais
                if (dataInicio.HasValue)
                    query = query.Where(v => v.Data >= dataInicio.Value);

                if (dataFim.HasValue)
                    query = query.Where(v => v.Data <= dataFim.Value);

                var viagens = await query
                    .OrderByDescending(v => v.Data)
                    .ThenBy(v => v.HoraInicio)
                    .ToListAsync();

                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao buscar viagens" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/viagenseconomildo/{id}
        /// Retorna uma viagem específica
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ViagensEconomildo>> GetViagensEconomildo(Guid id)
        {
            try
            {
                var viagemEconomildo = await _context.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .FirstOrDefaultAsync(v => v.ViagemEconomildoId == id);

                if (viagemEconomildo == null)
                {
                    return NotFound(new { mensagem = "Viagem não encontrada" });
                }

                return Ok(viagemEconomildo);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao buscar viagem" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/viagenseconomildo/bydata/{data}
        /// Retorna viagens de uma data específica
        /// </summary>
        [HttpGet("bydata/{data}")]
        public async Task<ActionResult<IEnumerable<ViagensEconomildo>>> GetViagensByData(DateTime data)
        {
            try
            {
                var viagens = await _context.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.Data.HasValue && v.Data.Value.Date == data.Date)
                    .OrderBy(v => v.HoraInicio)
                    .ToListAsync();

                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao buscar viagens por data" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/viagenseconomildo/byveiculo/{veiculoId}
        /// Retorna viagens de um veículo específico
        /// </summary>
        [HttpGet("byveiculo/{veiculoId}")]
        public async Task<ActionResult<IEnumerable<ViagensEconomildo>>> GetViagensByVeiculo(Guid veiculoId)
        {
            try
            {
                var viagens = await _context.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.VeiculoId == veiculoId)
                    .OrderByDescending(v => v.Data)
                    .ThenBy(v => v.HoraInicio)
                    .ToListAsync();

                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao buscar viagens por veículo" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/viagenseconomildo/bymotorista/{motoristaId}
        /// Retorna viagens de um motorista específico
        /// </summary>
        [HttpGet("bymotorista/{motoristaId}")]
        public async Task<ActionResult<IEnumerable<ViagensEconomildo>>> GetViagensByMotorista(Guid motoristaId)
        {
            try
            {
                var viagens = await _context.ViagensEconomildo
                    .Include(v => v.Veiculo)
                    .Include(v => v.Motorista)
                    .Where(v => v.MotoristaId == motoristaId)
                    .OrderByDescending(v => v.Data)
                    .ThenBy(v => v.HoraInicio)
                    .ToListAsync();

                return Ok(viagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao buscar viagens por motorista" , erro = ex.Message });
            }
        }

        /// <summary>
        /// POST: api/viagenseconomildo
        /// Cria uma nova viagem
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ViagensEconomildo>> PostViagensEconomildo(ViagensEconomildo viagemEconomildo)
        {
            try
            {
                if (viagemEconomildo.ViagemEconomildoId == Guid.Empty)
                {
                    viagemEconomildo.ViagemEconomildoId = Guid.NewGuid();
                }

                // Validações
                if (viagemEconomildo.VeiculoId == null || viagemEconomildo.VeiculoId == Guid.Empty)
                {
                    return BadRequest(new { mensagem = "VeiculoId é obrigatório" });
                }

                if (viagemEconomildo.MotoristaId == null || viagemEconomildo.MotoristaId == Guid.Empty)
                {
                    return BadRequest(new { mensagem = "MotoristaId é obrigatório" });
                }

                _context.ViagensEconomildo.Add(viagemEconomildo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetViagensEconomildo) ,
                    new { id = viagemEconomildo.ViagemEconomildoId } , viagemEconomildo);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao criar viagem" , erro = ex.Message });
            }
        }

        /// <summary>
        /// POST: api/viagenseconomildo/batch
        /// Recebe múltiplas viagens do app MAUI
        /// ⚠️ Se o app não tiver autenticação, adicione [AllowAnonymous] aqui
        /// </summary>
        // [AllowAnonymous] // ← Descomente se o app não tiver autenticação
        [HttpPost("batch")]
        public async Task<ActionResult<BatchResult>> PostViagensBatch(List<ViagensEconomildo> viagens)
        {
            var result = new BatchResult();

            try
            {
                if (viagens == null || !viagens.Any())
                {
                    return BadRequest(new { mensagem = "Nenhuma viagem fornecida" });
                }


                // 🔍 LOG: Verificar datas recebidas pela API
                System.Diagnostics.Debug.WriteLine($"📥 API RECEBEU {viagens.Count} viagem(ns):");
                foreach (var v in viagens.Take(3)) // Log apenas das 3 primeiras para não poluir
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"   Viagem {v.ViagemEconomildoId.ToString().Substring(0, 8)}... | " +
                        $"Data: {v.Data:dd/MM/yyyy HH:mm:ss} (Kind: {v.Data?.Kind})"
                    );
                }
                foreach (var viagem in viagens)
                {
                    try
                    {
                        // Gera ID se não tiver
                        if (viagem.ViagemEconomildoId == Guid.Empty)
                        {
                            viagem.ViagemEconomildoId = Guid.NewGuid();
                        }

                        // Validações básicas
                        if (viagem.VeiculoId == null || viagem.VeiculoId == Guid.Empty)
                        {
                            result.Erros++;
                            result.MensagensErro.Add($"Viagem sem VeiculoId");
                            continue;
                        }

                        if (viagem.MotoristaId == null || viagem.MotoristaId == Guid.Empty)
                        {
                            result.Erros++;
                            result.MensagensErro.Add($"Viagem sem MotoristaId");
                            continue;
                        }

                        // Verifica se já existe (evita duplicação)
                        var existe = await _context.ViagensEconomildo
                            .AnyAsync(v => v.ViagemEconomildoId == viagem.ViagemEconomildoId);

                        if (!existe)
                        {
                            _context.ViagensEconomildo.Add(viagem);
                            result.Sucesso++;
                        }
                        else
                        {
                            result.Duplicadas++;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Erros++;
                        result.MensagensErro.Add($"Viagem {viagem.ViagemEconomildoId}: {ex.Message}");
                    }
                }

                // Salva apenas se tiver pelo menos uma viagem com sucesso
                if (result.Sucesso > 0)
                {
                    await _context.SaveChangesAsync();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao processar lote de viagens" , erro = ex.Message });
            }
        }

        /// <summary>
        /// PUT: api/viagenseconomildo/{id}
        /// Atualiza uma viagem existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutViagensEconomildo(Guid id , ViagensEconomildo viagemEconomildo)
        {
            if (id != viagemEconomildo.ViagemEconomildoId)
            {
                return BadRequest(new { mensagem = "ID da viagem não corresponde" });
            }

            _context.Entry(viagemEconomildo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViagensEconomildoExists(id))
                {
                    return NotFound(new { mensagem = "Viagem não encontrada" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao atualizar viagem" , erro = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/viagenseconomildo/{id}
        /// Remove uma viagem
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteViagensEconomildo(Guid id)
        {
            try
            {
                var viagemEconomildo = await _context.ViagensEconomildo.FindAsync(id);
                if (viagemEconomildo == null)
                {
                    return NotFound(new { mensagem = "Viagem não encontrada" });
                }

                _context.ViagensEconomildo.Remove(viagemEconomildo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { mensagem = "Erro ao excluir viagem" , erro = ex.Message });
            }
        }

        private bool ViagensEconomildoExists(Guid id)
        {
            return _context.ViagensEconomildo.Any(e => e.ViagemEconomildoId == id);
        }
    }

    /// <summary>
    /// Classe auxiliar para resultado do processamento em lote
    /// </summary>
    public class BatchResult
    {
        public int Sucesso { get; set; } = 0;
        public int Erros { get; set; } = 0;
        public int Duplicadas { get; set; } = 0;
        public List<string> MensagensErro { get; set; } = new List<string>();
        public string Mensagem => $"✅ Sucesso: {Sucesso} | ❌ Erros: {Erros} | ⚠️ Duplicadas: {Duplicadas}";
    }
}