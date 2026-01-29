/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Views/ViewPatrimonioConferencia.cs                      ║
 * ║  Descrição: Modelo mapeado da View de conferência de patrimônio          ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

#nullable enable
using System;

namespace FrotiX.Models.Views
{
    public class ViewPatrimonioConferencia
    {
        public Guid PatrimonioId
        {
            get; set;
        }

        public string? NPR
        {
            get; set;
        }

        public string? Marca
        {
            get; set;
        }

        public string? Modelo
        {
            get; set;
        }

        public string? Descricao
        {
            get; set;
        }

        public string? LocalizacaoAtual
        {
            get; set;
        }

        public string? NomeSetor
        {
            get; set;
        }

        public string? NomeSecao
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        }

        public string Situacao { get; set; } = null!;

        public int? StatusConferencia
        {
            get; set;
        }

        public string? LocalizacaoConferencia
        {
            get; set;
        }

        public Guid? SetorConferenciaId
        {
            get; set;
        }

        public Guid? SecaoConferenciaId
        {
            get; set;
        }
    }
}
