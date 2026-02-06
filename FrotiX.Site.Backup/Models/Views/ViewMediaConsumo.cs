/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMediaConsumo.cs                                                                   ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de médias de consumo de combustível por veículo.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: VeiculoId, ConsumoGeral                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                        ║
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
     * ⚡ MODEL: ViewMediaConsumo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de média de consumo por veículo
     *
     * 📥 ENTRADAS     : Veículo e valores agregados de consumo
     *
     * 📤 SAÍDAS       : Registro somente leitura para métricas e dashboards
     *
     * 🔗 CHAMADA POR  : Relatórios de consumo e análise de eficiência
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewMediaConsumo
    {
        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Consumo médio geral (litros/km calculado)
        public decimal? ConsumoGeral { get; set; }
    }
}

