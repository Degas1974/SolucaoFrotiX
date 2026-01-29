/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/DTO/HigienizacaoDto.cs                                  ║
 * ║  Descrição: DTO para operações de higienização de dados                  ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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


