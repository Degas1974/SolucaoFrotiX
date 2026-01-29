/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewPatrimonioConferencia.cs                                                          ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de conferência de patrimônio (dados do bem e situação).                     ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: PatrimonioId, NPR, Marca, Modelo, Descricao, Localizacao, Status                         ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System                                                                                     ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
