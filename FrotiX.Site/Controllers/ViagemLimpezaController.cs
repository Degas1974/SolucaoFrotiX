/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemLimpezaController.cs                                       ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: ViagemLimpeza API (Data Cleaning)
     * 🎯 OBJETIVO: Limpeza e correção em massa de dados de origem/destino de viagens
     * 📋 ROTAS: /api/ViagemLimpeza/* (origens, destinos, corrigir-origem, corrigir-destino)
     * 🔗 ENTIDADES: Viagem
     * 📦 DEPENDÊNCIAS: IViagemRepository
     * 🧹 FUNCIONALIDADE: Listar valores distintos e corrigir múltiplos registros de uma vez
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class ViagemLimpezaController :ControllerBase
    {
        private readonly IViagemRepository _viagemRepo;

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
         * 🎯 OBJETIVO: Listar todos os valores distintos de origem de viagens
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: List<string> com origens distintas
         * 🔗 CHAMADA POR: Interface de limpeza de dados (dropdown de origens)
         * 🔄 CHAMA: IViagemRepository.GetDistinctOrigensAsync()
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
         * 🎯 OBJETIVO: Listar todos os valores distintos de destino de viagens
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: List<string> com destinos distintos
         * 🔗 CHAMADA POR: Interface de limpeza de dados (dropdown de destinos)
         * 🔄 CHAMA: IViagemRepository.GetDistinctDestinosAsync()
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
         * 🎯 OBJETIVO: Corrigir em massa múltiplas origens diferentes para um valor único padrão
         * 📥 ENTRADAS: CorrecaoRequest { Anteriores: List<string>, NovoValor: string }
         * 📤 SAÍDAS: 204 NoContent (sucesso) ou 500 (erro)
         * 🔗 CHAMADA POR: Interface de limpeza de dados (correção de origem)
         * 🔄 CHAMA: IViagemRepository.CorrigirOrigemAsync()
         * 📝 EXEMPLO: ["Origem1", "OriGem1", "origem 1"] → "Origem 1"
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
         * 🎯 OBJETIVO: Corrigir em massa múltiplos destinos diferentes para um valor único padrão
         * 📥 ENTRADAS: CorrecaoRequest { Anteriores: List<string>, NovoValor: string }
         * 📤 SAÍDAS: 204 NoContent (sucesso) ou 500 (erro)
         * 🔗 CHAMADA POR: Interface de limpeza de dados (correção de destino)
         * 🔄 CHAMA: IViagemRepository.CorrigirDestinoAsync()
         * 📝 EXEMPLO: ["Destino1", "DestiNo1", "destino 1"] → "Destino 1"
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
     * 📦 DTO: CorrecaoRequest
     * 🎯 OBJETIVO: Request para correção em massa de valores de origem/destino
     * 📋 PROPRIEDADES:
     *    - Anteriores: Lista de valores antigos/incorretos a serem substituídos
     *    - NovoValor: Valor novo/correto que substituirá todos os anteriores
     * 📝 EXEMPLO: Corrigir variações ["SP", "sp", "São Paulo"] → "São Paulo"
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
