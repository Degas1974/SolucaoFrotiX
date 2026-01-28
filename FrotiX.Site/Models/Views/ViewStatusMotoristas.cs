// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewStatusMotoristas.cs                                            ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para dashboard de status em tempo real dos motoristas.           ║
// ║ Usado em painéis de controle para visualização rápida da situação.          ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • MotoristaId - Identificador único do motorista                            ║
// ║ • Nome - Nome do motorista                                                  ║
// ║ • Ponto - Número do ponto funcional                                         ║
// ║ • StatusAtual - Status atual (Disponível, Em Viagem, Indisponível)          ║
// ║ • DataEscala - Data da escala atual                                         ║
// ║ • NumeroSaidas - Quantidade de saídas realizadas no dia                     ║
// ║ • Placa - Placa do veículo atual                                            ║
// ║ • Veiculo - Descrição do veículo                                            ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Dashboard de status de motoristas em tempo real                           ║
// ║ • Painel de controle para despachadores                                     ║
// ║ • Indicadores visuais de disponibilidade                                    ║
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
    // ViewModel para Status dos Motoristas
    public class ViewStatusMotoristas
    {
        public Guid MotoristaId { get; set; }
        public string Nome { get; set; }
        public string? Ponto { get; set; }
        public string StatusAtual { get; set; }
        public DateTime? DataEscala { get; set; }
        public int NumeroSaidas { get; set; }
        public string? Placa { get; set; }
        public string? Veiculo { get; set; }
    }
}
