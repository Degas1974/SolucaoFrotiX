/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: EventoListDto.cs                                                                        ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: DTO para listagem simplificada de eventos em grids e dropdowns.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: EventoListDto                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Nenhuma                                                                            ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models
{
    // ==================================================================================================
    // DTO
    // ==================================================================================================
    // Representa a linha de listagem de eventos com campos pré-formatados.
    // ==================================================================================================
    public class EventoListDto
    {
        // Identificador do evento.
        public Guid EventoId
        {
            get; set;
        }
        // Nome do evento.
        public string Nome
        {
            get; set;
        }
        // Descrição do evento.
        public string Descricao
        {
            get; set;
        }
        // Data inicial.
        public DateTime? DataInicial
        {
            get; set;
        }
        // Data final.
        public DateTime? DataFinal
        {
            get; set;
        }
        // Quantidade de participantes.
        public string QtdParticipantes
        {
            get; set;
        }
        // Status do evento.
        public string Status
        {
            get; set;
        }
        // Nome do requisitante.
        public string NomeRequisitante
        {
            get; set;
        }
        // Nome do requisitante (HTML).
        public string NomeRequisitanteHTML
        {
            get; set;
        }
        // Nome do setor.
        public string NomeSetor
        {
            get; set;
        }
        // Custo da viagem formatado.
        public string CustoViagem
        {
            get; set;
        }
        // Custo da viagem sem formatação.
        public decimal CustoViagemNaoFormatado
        {
            get; set;
        }
    }
}
