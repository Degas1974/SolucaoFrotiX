// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EncarregadoContrato.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Tabela de relacionamento N:N entre Encarregado e Contrato.                  ║
// ║ Permite que um encarregado esteja vinculado a múltiplos contratos.          ║
// ║                                                                              ║
// ║ CLASSES:                                                                     ║
// ║ - EncarregadoContratoViewModel: ViewModel para operações                    ║
// ║ - EncarregadoContrato: Entidade com chave composta                          ║
// ║                                                                              ║
// ║ CHAVE COMPOSTA:                                                              ║
// ║ - EncarregadoId + ContratoId (configurado em FrotiXDbContext)               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    public class EncarregadoContratoViewModel
    {
        public Guid EncarregadoId { get; set; }
        public Guid ContratoId { get; set; }
        public EncarregadoContrato? EncarregadoContrato { get; set; }
    }

    public class EncarregadoContrato
    {
        // 2 Foreign Keys as Primary Key (Chave Composta)
        // ===============================================
        [Key, Column(Order = 0)]
        public Guid EncarregadoId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
