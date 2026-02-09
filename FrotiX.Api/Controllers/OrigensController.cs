// ====== OrigensController.cs ======
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FrotiXApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrigensController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrigensController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var origens = await _unitOfWork.Viagem.GetDistinctOrigensAsync();

                // Converte as strings para objetos com a propriedade Origens
                var result = origens.Select(o => new { Origens = o }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao carregar origens: {ex.Message}");
            }
        }
    }
}
