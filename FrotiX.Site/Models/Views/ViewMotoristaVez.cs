// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewMotoristaVez.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para "Motorista da Vez" - sistema de rodízio/fila de motoristas. ║
// ║ Exibe motoristas disponíveis ordenados por ordem de chamada.                ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Motorista:                                                                    ║
// ║ • MotoristaId - Identificador único                                         ║
// ║ • NomeMotorista - Nome do motorista                                         ║
// ║ • Ponto - Número do ponto funcional                                         ║
// ║ • Foto - Foto para exibição em cards                                        ║
// ║                                                                              ║
// ║ Escala:                                                                       ║
// ║ • DataEscala - Data da escala atual                                         ║
// ║ • NumeroSaidas - Saídas já realizadas no dia                                ║
// ║ • StatusMotorista - Status atual                                            ║
// ║ • Lotacao - Unidade de lotação                                              ║
// ║ • HoraInicio/Fim - Horário de trabalho                                      ║
// ║                                                                              ║
// ║ Veículo:                                                                      ║
// ║ • VeiculoDescricao - Descrição do veículo alocado                           ║
// ║ • Placa - Placa do veículo                                                  ║
// ║                                                                              ║
// ║ MÉTODO HELPER:                                                               ║
// ║ • GetStatusClass() - Retorna classe CSS para status visual                  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
