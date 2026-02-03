/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewLavagem.cs                                                                         ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de lavagens de veículos (horários, duração, lavadores).                      ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: LavagemId, MotoristaId, VeiculoId, Data, Horario                                         ║
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
     * ⚡ MODEL: ViewLavagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de lavagens de veículos
     *
     * 📥 ENTRADAS     : Veículo, motorista, lavadores, data e horário
     *
     * 📤 SAÍDAS       : Registro somente leitura para controle de lavagens
     *
     * 🔗 CHAMADA POR  : Telas de manutenção e limpeza de frota
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewLavagem
    {
        // [DADOS] Identificador único da lavagem
        public Guid LavagemId { get; set; }

        // [DADOS] Identificador do motorista responsável
        public Guid MotoristaId { get; set; }

        // [DADOS] Identificador do veículo lavado
        public Guid VeiculoId { get; set; }

        // [DADOS] IDs dos lavadores (concatenados/separados)
        public string? LavadoresId { get; set; }

        // [DADOS] Data da lavagem (formatada)
        public string? Data { get; set; }

        // [DADOS] Horário da lavagem
        public string? Horario { get; set; }

        // [DADOS] Nomes dos lavadores
        public string? Lavadores { get; set; }

        // [DADOS] Descrição completa do veículo
        public string? DescricaoVeiculo { get; set; }

        // [DADOS] Nome do motorista/responsável
        public string? Nome { get; set; }
    }
}
