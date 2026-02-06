/* ****************************************************************************************
 * ⚡ ARQUIVO: EventoListDto.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer DTO simplificado para listagem de eventos.
 *
 * 📥 ENTRADAS     : Campos pré-formatados de evento, custos e status.
 *
 * 📤 SAÍDAS       : Linha pronta para grids e dropdowns.
 *
 * 🔗 CHAMADA POR  : Listagens e relatórios de eventos.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : Nenhuma.
 **************************************************************************************** */

using System;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ DTO: EventoListDto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar linha de listagem de eventos com campos pré-formatados.
     *
     * 📥 ENTRADAS     : Informações básicas do evento e custos.
     *
     * 📤 SAÍDAS       : DTO pronto para exibição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de eventos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
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
