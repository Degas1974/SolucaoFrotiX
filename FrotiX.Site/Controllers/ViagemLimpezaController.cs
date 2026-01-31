/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemLimpezaController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Corrigir e padronizar origens/destinos de viagens em lote.
 *
 * 📥 ENTRADAS     : Lista de valores anteriores e novo valor.
 *
 * 📤 SAÍDAS       : JSON/Status das operações.
 *
 * 🔗 CHAMADA POR  : Ferramentas de limpeza de dados.
 *
 * 🔄 CHAMA        : IViagemRepository.
 **************************************************************************************** */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: ViagemLimpezaController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints de limpeza de origem/destino.
     *
     * 📥 ENTRADAS     : DTOs de correção.
     *
     * 📤 SAÍDAS       : JSON/Status HTTP.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemLimpezaController :ControllerBase
    {
        private readonly IViagemRepository _viagemRepo;

        /****************************************************************************************
         * ⚡ FUNÇÃO: ViagemLimpezaController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do repositório de viagens.
         *
         * 📥 ENTRADAS     : viagemRepo.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public ViagemLimpezaController(IViagemRepository viagemRepo)
        {
            try
            {
                _viagemRepo = viagemRepo;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "ViagemLimpezaController.cs" ,
                    "ViagemLimpezaController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetOrigens
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar origens distintas registradas nas viagens.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Lista de origens.
         *
         * 🔗 CHAMADA POR  : Tela de limpeza de dados.
         ****************************************************************************************/
        [HttpGet("origens")]
        public async Task<ActionResult<List<string>>> GetOrigens()
        {
            try
            {
                var origens = await _viagemRepo.GetDistinctOrigensAsync();
                return Ok(origens);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemLimpezaController.cs" , "GetOrigens" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao carregar origens"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetDestinos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar destinos distintos registrados nas viagens.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Lista de destinos.
         *
         * 🔗 CHAMADA POR  : Tela de limpeza de dados.
         ****************************************************************************************/
        [HttpGet("destinos")]
        public async Task<ActionResult<List<string>>> GetDestinos()
        {
            try
            {
                var destinos = await _viagemRepo.GetDistinctDestinosAsync();
                return Ok(destinos);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemLimpezaController.cs" , "GetDestinos" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao carregar destinos"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CorrigirOrigem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Padronizar valores de origem em lote.
         *
         * 📥 ENTRADAS     : request (CorrecaoRequest).
         *
         * 📤 SAÍDAS       : Status HTTP (204/500).
         *
         * 🔗 CHAMADA POR  : Ação de correção de origem.
         ****************************************************************************************/
        [HttpPost("corrigir-origem")]
        public async Task<IActionResult> CorrigirOrigem([FromBody] CorrecaoRequest request)
        {
            try
            {
                await _viagemRepo.CorrigirOrigemAsync(request.Anteriores , request.NovoValor);
                return NoContent();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "ViagemLimpezaController.cs" ,
                    "CorrigirOrigem" ,
                    error
                );
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao corrigir origem"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CorrigirDestino
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Padronizar valores de destino em lote.
         *
         * 📥 ENTRADAS     : request (CorrecaoRequest).
         *
         * 📤 SAÍDAS       : Status HTTP (204/500).
         *
         * 🔗 CHAMADA POR  : Ação de correção de destino.
         ****************************************************************************************/
        [HttpPost("corrigir-destino")]
        public async Task<IActionResult> CorrigirDestino([FromBody] CorrecaoRequest request)
        {
            try
            {
                await _viagemRepo.CorrigirDestinoAsync(request.Anteriores , request.NovoValor);
                return NoContent();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "ViagemLimpezaController.cs" ,
                    "CorrigirDestino" ,
                    error
                );
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao corrigir destino"
                });
            }
        }
    }

    /****************************************************************************************
     * ⚡ DTO: CorrecaoRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar valores para correção em lote.
     *
     * 📥 ENTRADAS     : Anteriores, NovoValor.
     *
     * 📤 SAÍDAS       : Nenhuma (estrutura de dados).
     ****************************************************************************************/
    public class CorrecaoRequest
    {
        public List<string> Anteriores
        {
            get; set;
        }
        public string NovoValor
        {
            get; set;
        }
    }
}
