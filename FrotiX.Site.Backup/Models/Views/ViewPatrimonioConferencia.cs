/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewPatrimonioConferencia.cs                                                          ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
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
    /****************************************************************************************
     * ⚡ MODEL: ViewPatrimonioConferencia
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar patrimônio para conferência/inventário
     *
     * 📥 ENTRADAS     : Bem, localização atual, localização conferência, status
     *
     * 📤 SAÍDAS       : Registro somente leitura para controle de inventário
     *
     * 🔗 CHAMADA POR  : Tela de conferência de patrimônio
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewPatrimonioConferencia
    {
        // [DADOS] Identificador único do bem
        public Guid PatrimonioId
        {
            get; set;
        }

        // [DADOS] Número de Patrimônio/Registro
        public string? NPR
        {
            get; set;
        }

        // [DADOS] Marca do bem
        public string? Marca
        {
            get; set;
        }

        // [DADOS] Modelo do bem
        public string? Modelo
        {
            get; set;
        }

        // [DADOS] Descrição do bem
        public string? Descricao
        {
            get; set;
        }

        // [DADOS] Localização atual registrada
        public string? LocalizacaoAtual
        {
            get; set;
        }

        // [DADOS] Nome do setor atual
        public string? NomeSetor
        {
            get; set;
        }

        // [DADOS] Nome da seção atual
        public string? NomeSecao
        {
            get; set;
        }

        // [DADOS] Flag de status ativo/inativo
        public bool Status
        {
            get; set;
        }

        // [DADOS] Situação atual (em uso/devolução/etc)
        public string Situacao { get; set; } = null!;

        // [DADOS] Status da conferência (0=não conferido, 1=conferido, etc)
        public int? StatusConferencia
        {
            get; set;
        }

        // [DADOS] Localização encontrada na conferência
        public string? LocalizacaoConferencia
        {
            get; set;
        }

        // [DADOS] Identificador do setor conferência (FK)
        public Guid? SetorConferenciaId
        {
            get; set;
        }

        // [DADOS] Identificador da seção conferência (FK)
        public Guid? SecaoConferenciaId
        {
            get; set;
        }
    }
}

