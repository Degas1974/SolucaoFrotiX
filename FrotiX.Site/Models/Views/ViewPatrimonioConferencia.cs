// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewPatrimonioConferencia.cs                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para conferência de patrimônios (inventário físico).             ║
// ║ Usado em processos de auditoria e verificação de bens patrimoniais.         ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • PatrimonioId - Identificador único do patrimônio                          ║
// ║ • SetorConferenciaId - Setor onde foi conferido                             ║
// ║ • SecaoConferenciaId - Seção onde foi conferido                             ║
// ║                                                                              ║
// ║ Dados do Patrimônio:                                                         ║
// ║ • NPR - Número do Patrimônio                                                ║
// ║ • Marca, Modelo - Identificação do bem                                      ║
// ║ • Descricao - Descrição detalhada                                           ║
// ║                                                                              ║
// ║ Localização:                                                                  ║
// ║ • LocalizacaoAtual - Localização registrada no sistema                      ║
// ║ • NomeSetor, NomeSecao - Setor/Seção oficial                                ║
// ║ • LocalizacaoConferencia - Localização encontrada na conferência            ║
// ║                                                                              ║
// ║ Status:                                                                       ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • Situacao - Situação física do bem                                         ║
// ║ • StatusConferencia - 0=Não conferido, 1=OK, 2=Divergência                  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
