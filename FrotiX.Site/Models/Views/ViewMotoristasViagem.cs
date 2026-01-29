/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Views/ViewMotoristasViagem.cs                           ║
 * ║  Descrição: Modelo mapeado da View de motoristas disponíveis para viagem ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;

namespace FrotiX.Models.Views
    {
    public class ViewMotoristasViagem
        {

        public Guid MotoristaId { get; set; }

        public string? Nome { get; set; }

        public bool Status { get; set; }

        public string? MotoristaCondutor { get; set; }

        public string? TipoCondutor { get; set; }

        public byte[]? Foto { get; set; }


        }
    }


