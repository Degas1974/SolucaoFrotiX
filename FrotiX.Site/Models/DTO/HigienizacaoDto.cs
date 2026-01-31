/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: HigienizacaoDto.cs                                                                      ║
   ║ 📂 CAMINHO: /Models/DTO                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: DTOs para higienização de dados (correção de origens/destinos).                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: HigienizacaoDto, CorrecaoOrigemDto, CorrecaoDestinoDto                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.Collections.Generic                                                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System.Collections.Generic;


namespace FrotiX.Models.DTO
    {
    // DTO para higienização geral.
    public class HigienizacaoDto
        {
        // Tipo de correção (origem/destino).
        public string Tipo { get; set; }               // origem ou destino
        // Valores antigos.
        public List<string> AntigosValores { get; set; }
        // Novo valor aplicado.
        public string NovosValores { get; set; }
        }

    // DTO para correção de origem.
    public class CorrecaoOrigemDto
        {
        // Lista de origens atuais.
        public List<string> Origens { get; set; }
        // Nova origem.
        public string NovaOrigem { get; set; }
        }

    // DTO para correção de destino.
    public class CorrecaoDestinoDto
        {
        // Lista de destinos atuais.
        public List<string> Destinos { get; set; }
        // Novo destino.
        public string NovoDestino { get; set; }
        }

    }

