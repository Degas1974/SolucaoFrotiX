// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: HigienizacaoDto.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ DTOs para operações de higienização/limpeza de dados de viagens.            ║
// ║ Usado pelo ViagemLimpezaController para correção em massa.                  ║
// ║                                                                              ║
// ║ CLASSES:                                                                     ║
// ║ - HigienizacaoDto: Request genérico (Tipo: origem/destino)                  ║
// ║ - CorrecaoOrigemDto: Corrigir múltiplas origens para NovaOrigem             ║
// ║ - CorrecaoDestinoDto: Corrigir múltiplos destinos para NovoDestino          ║
// ║                                                                              ║
// ║ EXEMPLO: Unificar "SEDE", "Sede", "sede" → "Sede"                           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System.Collections.Generic;


namespace FrotiX.Models.DTO
    {
    public class HigienizacaoDto
        {
        public string Tipo { get; set; }               // origem ou destino
        public List<string> AntigosValores { get; set; }
        public string NovosValores { get; set; }
        }

    public class CorrecaoOrigemDto
        {
        public List<string> Origens { get; set; }
        public string NovaOrigem { get; set; }
        }

    public class CorrecaoDestinoDto
        {
        public List<string> Destinos { get; set; }
        public string NovoDestino { get; set; }
        }

    }


