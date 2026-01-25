using Microsoft.AspNetCore.Mvc;
using FrotiXApi.Data;
using FrotiXApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Repository.IRepository;

namespace FrotiXBlazorMaui.Controllers
{
    /// <summary>
    /// Controller para gerenciar operações com Vistoriadores
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VistoriadoresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VistoriadoresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET: api/vistoriadores
        /// Retorna lista de usuários marcados como Vistoriador para dropdown
        /// Traz VistoriadorId (Id) e NomeCompleto
        /// ✅ FILTRADO: Vistoriador = true E Status = true
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVistoriadores()
        {
            try
            {
                Console.WriteLine("📡 API: GetVistoriadores chamado");

                var usuarios = _unitOfWork.AspNetUsers.GetAll();

                var result = usuarios
                    .Where(u => u.Vistoriador == true && u.Status == true)
                    .Select(u => new
                    {
                        VistoriadorId = u.Id,
                        NomeCompleto = u.NomeCompleto
                    })
                    .OrderBy(u => u.NomeCompleto)
                    .ToList();

                Console.WriteLine($"✅ Vistoriadores encontrados: {result.Count}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro em GetVistoriadores: {ex.Message}");
                Console.WriteLine($"📚 Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Erro ao buscar vistoriadores: {ex.Message}" });
            }
        }

        /// <summary>
        /// GET: api/vistoriadores/{id}
        /// Retorna um vistoriador específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var usuario = _unitOfWork.AspNetUsers.GetAll()
                    .FirstOrDefault(u => u.Id == id && u.Vistoriador == true);

                if (usuario == null)
                {
                    return NotFound(new { mensagem = "Vistoriador não encontrado" });
                }

                var result = new
                {
                    VistoriadorId = usuario.Id,
                    NomeCompleto = usuario.NomeCompleto,
                    Status = usuario.Status
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Erro ao buscar vistoriador",
                    erro = ex.Message
                });
            }
        }
    }
}
