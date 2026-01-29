/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewMotoristaVez.cs                                                                    ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de motorista da vez (fila de atendimento).                                   ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: MotoristaId, NomeMotorista, DataEscala, NumeroSaidas, StatusMotorista, VeiculoDescricao   ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Validations                                                                         ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FrotiX.Models
{
    public class ViewMotoristasVez
    {
        public Guid MotoristaId { get; set; }
        public string NomeMotorista { get; set; }
        public string? Ponto { get; set; }
        public byte[]? Foto { get; set; }
        public DateTime DataEscala { get; set; }
        public int NumeroSaidas { get; set; }
        public string StatusMotorista { get; set; }
        public string? Lotacao { get; set; }
        public string? VeiculoDescricao { get; set; }
        public string? Placa { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }

        public string GetStatusClass()
        {
            return StatusMotorista?.ToLower() switch
            {
                "disponÃ­vel" or "disponivel" => "text-success",
                "em serviÃ§o" or "em servico" => "text-warning",
                _ => "text-secondary"
            };
        }
    }
}
