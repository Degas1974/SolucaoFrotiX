// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EventoListDto.cs                                                   ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ DTO para listagem de eventos em grids e DataTables.                         ║
// ║ Contém dados formatados para exibição na tela de eventos.                   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - EventoId: Identificador único do evento                                   ║
// ║ - Nome, Descricao: Dados básicos do evento                                  ║
// ║ - DataInicial, DataFinal: Período do evento                                 ║
// ║ - QtdParticipantes: Quantidade como string formatada                        ║
// ║ - Status: Status do evento (Solicitado, Em Andamento, Finalizado)           ║
// ║ - NomeRequisitante, NomeRequisitanteHTML: Nome com/sem formatação HTML      ║
// ║ - NomeSetor: Setor solicitante                                              ║
// ║ - CustoViagem: Valor formatado como moeda                                   ║
// ║ - CustoViagemNaoFormatado: Valor decimal para cálculos                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
