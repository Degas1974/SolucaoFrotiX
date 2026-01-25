using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FrotiXApi.Controllers
{
    /// <summary>
    /// Controller para gerenciar operações com Motoristas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MotoristasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MotoristasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retorna lista de motoristas COM fotos para o app MAUI
        /// </summary>
        /// <returns>Lista de motoristas ativos com fotos</returns>
        [HttpGet("GetMotoristas")]
        public IActionResult GetMotoristas()
        {
            try
            {
                Console.WriteLine("📡 API: GetMotoristas chamado");

                // ✅ Usa GetAll() do Repository e filtra em memória
                var todosMotoristas = _unitOfWork.Motorista.GetAll();

                var motoristas = todosMotoristas
                    .Where(m => m.Status == true) // Apenas ativos
                    .OrderBy(m => m.Nome)
                    .Select(m => new MotoristaDto
                    {
                        MotoristaId = m.MotoristaId ,
                        Nome = m.Nome ?? "SEM NOME" ,
                        Foto = m.Foto // ✅ INCLUI A FOTO (byte[])
                    })
                    .ToList();

                Console.WriteLine($"👥 Total de motoristas retornados: {motoristas.Count}");

                // Estatísticas de fotos
                var comFoto = motoristas.Count(m => m.Foto != null && m.Foto.Length > 0);
                var semFoto = motoristas.Count - comFoto;
                Console.WriteLine($"📸 {comFoto} motoristas COM foto, {semFoto} sem foto");

                if (motoristas.Any())
                {
                    var primeiro = motoristas.First();
                    var tamanhoFoto = primeiro.Foto?.Length ?? 0;
                    Console.WriteLine($"🔍 Exemplo: {primeiro.Nome} - Foto: {tamanhoFoto} bytes");
                }

                return Ok(motoristas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro em GetMotoristas: {ex.Message}");
                Console.WriteLine($"📚 Stack Trace: {ex.StackTrace}");
                return StatusCode(500 , new { message = $"Erro ao buscar motoristas: {ex.Message}" });
            }
        }

        /// <summary>
        /// Endpoint auxiliar para verificar fotos de um motorista específico
        /// </summary>
        [HttpGet("{id}/VerificarFoto")]
        public IActionResult VerificarFoto(Guid id)
        {
            try
            {
                // ✅ Usa Get() do Repository para buscar por ID
                var motorista = _unitOfWork.Motorista.Get(id);

                if (motorista == null)
                    return NotFound(new { message = "Motorista não encontrado" });

                var temFoto = motorista.Foto != null && motorista.Foto.Length > 0;
                var tamanhoFoto = motorista.Foto != null ? motorista.Foto.Length : 0;

                return Ok(new
                {
                    motorista.MotoristaId ,
                    motorista.Nome ,
                    TemFoto = temFoto ,
                    TamanhoFoto = tamanhoFoto ,
                    Mensagem = temFoto
                        ? $"Motorista tem foto de {tamanhoFoto} bytes"
                        : "Motorista não tem foto cadastrada"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { message = $"Erro: {ex.Message}" });
            }
        }

        // ============================================
        // ENDPOINTS ORIGINAIS
        // ============================================

        /// <summary>
        /// GET: api/motoristas/GetAll
        /// Retorna todos os motoristas ATIVOS com foto
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll();

                var result = motoristas
                    .Where(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome)
                             && m.Status == true)
                    .Select(m => new
                    {
                        MotoristaId = m.MotoristaId ,
                        Nome = m.Nome ,
                        Foto = m.Foto != null && m.Foto.Length > 0 ? Convert.ToBase64String(m.Foto) : null ,
                        Status = m.Status
                    })
                    .OrderBy(m => m.Nome)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao carregar motoristas" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/motoristas/ativos
        /// Retorna todos os motoristas ativos (alternativa ao GetAll)
        /// </summary>
        [HttpGet("ativos")]
        public async Task<IActionResult> GetAtivos()
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll();

                var result = motoristas
                    .Where(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome)
                             && m.Status == true)
                    .Select(m => new
                    {
                        MotoristaId = m.MotoristaId ,
                        Nome = m.Nome ,
                        Foto = m.Foto != null && m.Foto.Length > 0 ? Convert.ToBase64String(m.Foto) : null ,
                        Status = m.Status
                    })
                    .OrderBy(m => m.Nome)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao carregar motoristas ativos" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/motoristas/{id}
        /// Retorna um motorista específico
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var motorista = _unitOfWork.Motorista.Get(id);

                if (motorista == null)
                    return NotFound(new { mensagem = "Motorista não encontrado" });

                return Ok(new
                {
                    MotoristaId = motorista.MotoristaId ,
                    Nome = motorista.Nome ,
                    Foto = motorista.Foto != null && motorista.Foto.Length > 0 ? Convert.ToBase64String(motorista.Foto) : null ,
                    Status = motorista.Status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao buscar motorista" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/motoristas/dropdown
        /// Retorna lista simplificada para dropdowns (sem foto, apenas ativos)
        /// </summary>
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll();

                var result = motoristas
                    .Where(m => m.Status == true)
                    .Select(m => new
                    {
                        MotoristaId = m.MotoristaId ,
                        Nome = m.Nome
                    })
                    .OrderBy(m => m.Nome)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao carregar motoristas" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/motoristas/todos
        /// Retorna TODOS os motoristas (incluindo inativos)
        /// </summary>
        [HttpGet("todos")]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll();

                var result = motoristas
                    .Where(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome))
                    .Select(m => new
                    {
                        MotoristaId = m.MotoristaId ,
                        Nome = m.Nome ,
                        Foto = m.Foto != null && m.Foto.Length > 0 ? Convert.ToBase64String(m.Foto) : null ,
                        Status = m.Status
                    })
                    .OrderBy(m => m.Nome)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao carregar todos motoristas" , erro = ex.Message });
            }
        }

        // ============================================
        // ENDPOINTS PARA SINCRONIZAÇÃO OTIMIZADA
        // ============================================

        /// <summary>
        /// GET: api/motoristas/GetMotoristasParaSync
        /// Retorna dados básicos dos motoristas SEM FOTOS (rápido para sincronização inicial)
        /// </summary>
        [HttpGet("GetMotoristasParaSync")]
        public async Task<IActionResult> GetMotoristasParaSync()
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll();

                var result = motoristas
                    .Where(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome)
                             && m.Status == true)
                    .Select(m => new
                    {
                        MotoristaId = m.MotoristaId ,
                        Nome = m.Nome ,
                        Status = m.Status ,
                        // NÃO incluir Foto aqui - será carregada separadamente
                    })
                    .OrderBy(m => m.Nome)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao carregar motoristas para sincronização" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/motoristas/GetMotoristasFotosPaginado
        /// Retorna apenas as fotos dos motoristas de forma paginada e comprimida
        /// </summary>
        /// <param name="pagina">Número da página (começa em 1)</param>
        /// <param name="tamanho">Quantidade de fotos por página (padrão: 20)</param>
        [HttpGet("GetMotoristasFotosPaginado")]
        public async Task<IActionResult> GetMotoristasFotosPaginado([FromQuery] int pagina = 1 , [FromQuery] int tamanho = 20)
        {
            try
            {
                if (pagina < 1)
                    pagina = 1;
                if (tamanho < 1 || tamanho > 50)
                    tamanho = 20; // Limita máximo de 50 por página

                var skip = (pagina - 1) * tamanho;

                var motoristas = _unitOfWork.Motorista.GetAll();

                var result = motoristas
                    .Where(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome)
                             && m.Status == true
                             && m.Foto != null && m.Foto.Length > 0) // Apenas motoristas com foto
                    .OrderBy(m => m.MotoristaId)
                    .Skip(skip)
                    .Take(tamanho)
                    .Select(m => new
                    {
                        MotoristaId = m.MotoristaId ,
                        Foto = ComprimirFotoBytes(m.Foto) // Comprime a foto antes de enviar
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao carregar fotos paginadas" , erro = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/motoristas/GetTotalMotoristas
        /// Retorna o total de motoristas ativos (útil para calcular páginas)
        /// </summary>
        [HttpGet("GetTotalMotoristas")]
        public async Task<IActionResult> GetTotalMotoristas()
        {
            try
            {
                var motoristas = _unitOfWork.Motorista.GetAll();

                var totalAtivos = motoristas
                    .Count(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome)
                             && m.Status == true);

                var totalComFoto = motoristas
                    .Count(m => m.MotoristaId != Guid.Empty
                             && !string.IsNullOrEmpty(m.Nome)
                             && m.Status == true
                             && m.Foto != null && m.Foto.Length > 0);

                return Ok(new
                {
                    TotalAtivos = totalAtivos ,
                    TotalComFoto = totalComFoto ,
                    TotalSemFoto = totalAtivos - totalComFoto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao contar motoristas" , erro = ex.Message });
            }
        }

        // ============================================
        // MÉTODOS AUXILIARES
        // ============================================

        /// <summary>
        /// Comprime uma foto em byte[] para Base64 reduzindo o tamanho
        /// Redimensiona para máximo 300x300px e comprime com qualidade 70%
        /// </summary>
        private string ComprimirFotoBytes(byte[] fotoBytes)
        {
            if (fotoBytes == null || fotoBytes.Length == 0)
                return null;

            try
            {
                using (var ms = new MemoryStream(fotoBytes))
                using (var image = Image.Load(ms))
                {
                    // Redimensiona mantendo proporção (máximo 300x300)
                    if (image.Width > 300 || image.Height > 300)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(300 , 300) ,
                            Mode = ResizeMode.Max
                        }));
                    }

                    using (var outputMs = new MemoryStream())
                    {
                        // Comprime com qualidade 70%
                        var encoder = new JpegEncoder { Quality = 70 };
                        image.SaveAsJpeg(outputMs , encoder);

                        var compressedBytes = outputMs.ToArray();
                        var compressedBase64 = Convert.ToBase64String(compressedBytes);

                        // Log do ganho de compressão
                        var taxaCompressao = (1 - (compressedBytes.Length / (double)fotoBytes.Length)) * 100;
                        Console.WriteLine($"Foto comprimida: {fotoBytes.Length} bytes -> {compressedBytes.Length} bytes ({taxaCompressao:F1}% redução)");

                        return compressedBase64;
                    }
                }
            }
            catch (Exception ex)
            {
                // Se falhar a compressão, retorna a foto original em Base64
                Console.WriteLine($"Erro ao comprimir foto: {ex.Message}");
                return Convert.ToBase64String(fotoBytes);
            }
        }
    }

    /// <summary>
    /// DTO para retornar dados do motorista com foto
    /// </summary>
    public class MotoristaDto
    {
        public Guid MotoristaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public byte[]? Foto { get; set; }
    }
}