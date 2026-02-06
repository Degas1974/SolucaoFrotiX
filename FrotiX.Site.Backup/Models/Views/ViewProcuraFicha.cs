/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewProcuraFicha.cs                                                                  ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de busca de fichas de vistoria (por motorista/veículo).                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: MotoristaId, VeiculoId, DataInicial/Final, HoraInicio/Fim, NoFichaVistoria               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                       ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewProcuraFicha
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar ficha de vistoria para busca por motorista/veículo
     *
     * 📥 ENTRADAS     : Motorista, veículo, período de data
     *
     * 📤 SAÍDAS       : Registro somente leitura para filtros
     *
     * 🔗 CHAMADA POR  : Tela de busca de fichas
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewProcuraFicha
    {
        // [DADOS] Identificador do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Data inicial do período (filtro)
        public DateTime? DataInicial { get; set; }

        // [DADOS] Data final do período (filtro)
        public DateTime? DataFinal { get; set; }

        // [DADOS] Hora de início da viagem
        public string? HoraInicio { get; set; }

        // [DADOS] Hora de término da viagem
        public string? HoraFim { get; set; }

        // [DADOS] Número da ficha de vistoria
        public int? NoFichaVistoria { get; set; }
    }
}

