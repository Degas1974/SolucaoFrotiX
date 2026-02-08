/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewMotoristaFluxo.cs                                                                  ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de fluxo de motoristas (Economildo) para dashboards.                         ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: MotoristaId, NomeMotorista                                                               ║
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
     * ⚡ MODEL: ViewMotoristaFluxo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de fluxo de motoristas Economildo
     *
     * 📥 ENTRADAS     : Motorista e identificação
     *
     * 📤 SAÍDAS       : Registro somente leitura para dashboards
     *
     * 🔗 CHAMADA POR  : Consultas de fluxo e distribuição de viagens
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewMotoristaFluxo
    {
        // [DADOS] Identificador do motorista (GUID em string)
        public string? MotoristaId { get; set; }

        // [DADOS] Nome completo do motorista
        public string? NomeMotorista { get; set; }
    }
}


