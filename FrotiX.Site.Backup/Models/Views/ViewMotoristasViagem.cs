/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMotoristasViagem.cs                                                                ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de motoristas disponíveis para viagem (status e tipo de condutor).           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: MotoristaId, Nome, Status, MotoristaCondutor, TipoCondutor, Foto                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System                                                                                     ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models.Views
{
    /****************************************************************************************
     * ⚡ MODEL: ViewMotoristasViagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar motoristas disponíveis para nova viagem
     *
     * 📥 ENTRADAS     : Motorista ativo, tipo de condutor, foto
     *
     * 📤 SAÍDAS       : Registro somente leitura para filtros de seleção
     *
     * 🔗 CHAMADA POR  : Formulário de criação/edição de viagem
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewMotoristasViagem
    {
        // [DADOS] Identificador único do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Nome do motorista
        public string? Nome { get; set; }

        // [DADOS] Flag de status ativo/inativo
        public bool Status { get; set; }

        // [DADOS] Classificação motorista/condutor
        public string? MotoristaCondutor { get; set; }

        // [DADOS] Tipo de condutor (motorista/cobrador/etc)
        public string? TipoCondutor { get; set; }

        // [DADOS] Foto do motorista (blob)
        public byte[]? Foto { get; set; }
    }
}

