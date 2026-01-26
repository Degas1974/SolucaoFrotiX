using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FrotiXApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequisitantesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RequisitantesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requisitantes = _unitOfWork.Requisitante.GetAll();
            return Ok(requisitantes.OrderBy(r => r.Nome).ToList());
        }
    }
}
