// ====== DestinosController.cs ======
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FrotiXApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DestinosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var destinos = await _unitOfWork.Viagem.GetDistinctDestinosAsync();

                // Converte as strings para objetos com a propriedade Destinos
                var result = destinos.Select(d => new { Destinos = d }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao carregar destinos: {ex.Message}");
            }
        }
    }
}
