/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemEventoDto.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar vínculo simplificado entre Viagem e Evento.
 *
 * 📥 ENTRADAS     : Identificadores e dados resumidos da viagem.
 *
 * 📤 SAÍDAS       : DTO para listagens e vínculos.
 *
 * 🔗 CHAMADA POR  : Telas de eventos e viagens.
 *
 * 🔄 CHAMA        : FrotiX.Models.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Models.
 **************************************************************************************** */

using FrotiX.Models;
using System;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ DTO: ViagemEventoDto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados simplificados de viagem vinculada a evento.
     *
     * 📥 ENTRADAS     : Identificadores, datas e custos.
     *
     * 📤 SAÍDAS       : DTO para exibição e vínculo.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de eventos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViagemEventoDto
    {
        // Evento associado.
        public Guid EventoId
        {
            get; set;
        }

        // Viagem associada.
        public Guid ViagemId
        {
            get; set;
        }

        // Número da ficha de vistoria.
        public int NoFichaVistoria
        {
            get; set;
        }
        // Nome do requisitante.
        public string NomeRequisitante
        {
            get; set;
        }
        // Nome do setor solicitante.
        public string NomeSetor
        {
            get; set;
        }
        // Nome do motorista.
        public string NomeMotorista
        {
            get; set;
        }
        // Descrição do veículo.
        public string DescricaoVeiculo
        {
            get; set;
        }
        // Custo da viagem.
        public decimal CustoViagem
        {
            get; set;
        }
        // Data inicial.
        public DateTime DataInicial
        {
            get; set;
        }
        // Hora de início.
        public DateTime? HoraInicio
        {
            get; set;
        } // ← DEVE SER DateTime? (não TimeSpan?)
        // Placa do veículo.
        public string Placa
        {
            get; set;
        }
    }
}
