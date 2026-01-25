using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    public partial class AtaRegistroPrecosController :ControllerBase
    {
        [Route("VerificarDependencias")]
        [HttpGet]
        public IActionResult VerificarDependencias(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        success = false ,
                        message = "ID inválido"
                    });
                }

                // Verifica dependências
                int itensCount = _unitOfWork.ItemVeiculoAta.GetAll(i => i.RepactuacaoAta.AtaId == id).Count();
                int veiculosCount = _unitOfWork.VeiculoAta.GetAll(v => v.AtaId == id).Count();

                bool possuiDependencias = itensCount > 0 || veiculosCount > 0;

                return Ok(new
                {
                    success = true ,
                    possuiDependencias ,
                    itens = itensCount ,
                    veiculos = veiculosCount
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AtaRegistroPrecosController.Partial.cs" ,
                    "VerificarDependencias" ,
                    error
                );
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao verificar dependências"
                });
            }
        }
    }
}