/* ****************************************************************************************
 * ⚡ ARQUIVO: AtaRegistroPrecosController.Partial.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Verificar dependências antes de excluir Ata de Registro de Preços,
 *                   retornando contadores de itens e veículos vinculados.
 *
 * 📥 ENTRADAS     : id (Guid) da Ata.
 *
 * 📤 SAÍDAS       : JSON com contagem de itens/veículos e mensagens de validação.
 *
 * 🔗 CHAMADA POR  : Frontend de exclusão de Ata de Registro de Preços.
 *
 * 🔄 CHAMA        : Repositórios ItemVeiculoAta e VeiculoAta via IUnitOfWork.
 *
 * 📦 DEPENDÊNCIAS : IUnitOfWork, LINQ, ControllerBase.
 *
 * 📝 OBSERVAÇÕES  : Classe parcial auxiliar do CRUD principal.
 **************************************************************************************** */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ PARTIAL CLASS: AtaRegistroPrecosController (Partial)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Verificar dependências antes de excluir Ata de Registro de Preços
     * 📥 ENTRADAS     : [Guid] id - ID da Ata a ser verificada
     * 📤 SAÍDAS       : JSON com contagem de itens e veículos vinculados à Ata
     * 🔗 CHAMADA POR  : Frontend antes de exclusão de Ata
     * 🔄 CHAMA        : ItemVeiculoAta.GetAll(), VeiculoAta.GetAll()
     * 📦 DEPENDÊNCIAS : IUnitOfWork, Repository
     * --------------------------------------------------------------------------------------
     * [DOC] Classe parcial para métodos auxiliares de Ata de Registro de Preços
     * [DOC] Verifica se Ata possui itens ou veículos vinculados antes de permitir exclusão
     * [DOC] Retorna contadores para exibir no frontend
     ****************************************************************************************/
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
