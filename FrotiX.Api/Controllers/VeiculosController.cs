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
    /// Controller para gerenciar operações com Veículos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VeiculosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET: api/veiculos/GetVeiculosDropdown
        /// Retorna lista de veículos para dropdown usando ViewVeiculoCompleto
        /// Traz VeiculoId e VeiculoCompleto
        /// ✅ FILTRADO APENAS VEÍCULOS COM ECONOMILDO = TRUE
        /// </summary>
        [HttpGet("GetVeiculosDropdown")]
        public async Task<ActionResult<List<VeiculoViewDto>>> GetVeiculosDropdown()
        {
            try
            {
                Console.WriteLine("📡 API: GetVeiculosDropdown chamado");

                var veiculos = _unitOfWork.ViewVeiculos.GetAll();

                var result = veiculos
                    .Where(v => v.VeiculoId != Guid.Empty
                             && v.Status == true
                             && v.Economildo == true) // ✅ FILTRO ECONOMILDO
                    .Select(v => new
                    {
                        VeiculoId = v.VeiculoId ,
                        Placa = v.Placa ,
                        VeiculoCompleto = v.VeiculoCompleto ,
                        Status = v.Status
                    })
                    .OrderBy(v => v.Placa)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro em GetVeiculosDropdown: {ex.Message}");
                Console.WriteLine($"📚 Stack Trace: {ex.StackTrace}");
                return StatusCode(500 , new { message = $"Erro ao buscar veículos: {ex.Message}" });
            }
        }

        /// <summary>
        /// GET: api/veiculos/economildo
        /// Retorna lista de veículos específicos para o Economildo
        /// ✅ FILTRADO APENAS VEÍCULOS COM ECONOMILDO = TRUE
        /// </summary>
        [HttpGet("economildo")]
        public async Task<IActionResult> GetVeiculosEconomildo()
        {
            try
            {
                var veiculos = _unitOfWork.ViewVeiculos.GetAll();

                var result = veiculos
                    .Where(v => v.VeiculoId != Guid.Empty
                             && v.Status == true
                             && v.Economildo == true) // ✅ FILTRO ECONOMILDO
                    .Select(v => new
                    {
                        VeiculoId = v.VeiculoId ,
                        Placa = v.Placa ,
                        VeiculoCompleto = v.VeiculoCompleto ,
                        Status = v.Status,
                        Quilometragem = v.Quilometragem
                    })
                    .OrderBy(v => v.Placa)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Erro ao carregar veículos Economildo" ,
                    erro = ex.Message
                });
            }
        }

        /// <summary>
        /// GET: api/veiculos/dropdown
        /// Retorna lista simplificada para dropdowns (mantido para compatibilidade)
        /// </summary>
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            try
            {
                var veiculos = _unitOfWork.ViewVeiculos.GetAll();

                var result = veiculos
                    .Where(v => v.Status == true)
                    .Select(v => new
                    {
                        v.VeiculoId ,
                        v.VeiculoCompleto ,
                        v.Placa,
                        v.Quilometragem
                    })
                    .OrderBy(v => v.Placa)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Erro ao carregar veículos" ,
                    erro = ex.Message
                });
            }
        }

        /// <summary>
        /// GET: api/veiculos/{id}
        /// Retorna um veículo específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var veiculo = _unitOfWork.ViewVeiculos.GetAll()
                    .FirstOrDefault(v => v.VeiculoId == id);

                if (veiculo == null)
                {
                    return NotFound(new { mensagem = "Veículo não encontrado" });
                }

                var result = new
                {
                    veiculo.VeiculoId ,
                    veiculo.Placa ,
                    veiculo.VeiculoCompleto ,
                    veiculo.Status,
                    veiculo.Quilometragem
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Erro ao buscar veículo" ,
                    erro = ex.Message
                });
            }
        }

        /// <summary>
        /// GET: api/veiculos
        /// Retorna todos os veículos ativos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var veiculos = _unitOfWork.ViewVeiculos.GetAll();

                var result = veiculos
                    .Where(v => v.VeiculoId != Guid.Empty && v.Status == true)
                    .Select(v => new
                    {
                        v.VeiculoId ,
                        v.Placa ,
                        v.VeiculoCompleto ,
                        v.Status
                    })
                    .OrderBy(v => v.Placa)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Erro ao carregar veículos" ,
                    erro = ex.Message
                });
            }
        }

        /// <summary>
        /// GET: api/veiculos/placa/{placa}
        /// Busca veículo por placa
        /// </summary>
        [HttpGet("placa/{placa}")]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
            try
            {
                var veiculo = _unitOfWork.ViewVeiculos.GetAll()
                    .FirstOrDefault(v => v.Placa.ToUpper() == placa.ToUpper());

                if (veiculo == null)
                {
                    return NotFound(new { mensagem = $"Veículo com placa {placa} não encontrado" });
                }

                var result = new
                {
                    veiculo.VeiculoId ,
                    veiculo.Placa ,
                    veiculo.VeiculoCompleto ,
                    veiculo.Status
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Erro ao buscar veículo por placa" ,
                    erro = ex.Message
                });
            }
        }
    }

    /// <summary>
    /// DTO para retornar dados da ViewVeiculoCompleto
    /// </summary>
    public class VeiculoViewDto
    {
        public Guid VeiculoId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string VeiculoCompleto { get; set; } = string.Empty;
        public bool Economildo { get; set; }
        public bool Status { get; set; }
    }
}