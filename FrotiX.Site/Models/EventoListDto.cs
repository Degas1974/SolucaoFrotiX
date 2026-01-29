/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EventoListDto.cs                                                                        ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTO para listagem simplificada de eventos em grids e dropdowns.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: EventoId, Nome, Descricao, DataInicial, DataFinal, QtdParticipantes, Status              ║
   ║    NomeRequisitante, NomeSetor                                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: Nenhuma | 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models
{
    public class EventoListDto
    {
        public Guid EventoId
        {
            get; set;
        }
        public string Nome
        {
            get; set;
        }
        public string Descricao
        {
            get; set;
        }
        public DateTime? DataInicial
        {
            get; set;
        }
        public DateTime? DataFinal
        {
            get; set;
        }
        public string QtdParticipantes
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public string NomeRequisitante
        {
            get; set;
        }
        public string NomeRequisitanteHTML
        {
            get; set;
        }
        public string NomeSetor
        {
            get; set;
        }
        public string CustoViagem
        {
            get; set;
        }
        public decimal CustoViagemNaoFormatado
        {
            get; set;
        }
    }
}
