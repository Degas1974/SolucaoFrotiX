/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewStatusMotoristas.cs                                                               ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de status de motoristas ativos (escala, veículo, posição).                  ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: MotoristaId, Nome, StatusAtual, DataEscala, NumeroSaidas, Placa, Veiculo                 ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Validations                                                                        ║
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
