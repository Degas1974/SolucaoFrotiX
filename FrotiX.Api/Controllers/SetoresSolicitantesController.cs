using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FrotiXApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetoresSolicitantesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetoresSolicitantesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var setores = _unitOfWork.SetorSolicitante.GetAll();
            return Ok(setores.OrderBy(s => s.Nome).ToList());
        }
    }
}
