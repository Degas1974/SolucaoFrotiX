// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewMotoristas.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model completo para listagem de motoristas com dados de contrato.      ║
// ║ Usado em grids e cadastros de motoristas com informações denormalizadas.    ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • MotoristaId - Identificador único do motorista                            ║
// ║ • ContratoId - Contrato do motorista                                        ║
// ║                                                                              ║
// ║ Dados Pessoais:                                                              ║
// ║ • Nome - Nome curto para exibição                                           ║
// ║ • NomeCompleto - Nome completo do motorista                                 ║
// ║ • Ponto - Número do ponto funcional                                         ║
// ║ • Celular01 - Telefone de contato                                           ║
// ║ • Foto - Foto do motorista (byte array)                                     ║
// ║                                                                              ║
// ║ Dados de CNH:                                                                ║
// ║ • CNH - Número da CNH                                                       ║
// ║ • CategoriaCNH - Categoria da habilitação (A, B, D, E)                      ║
// ║                                                                              ║
// ║ Dados de Contrato:                                                           ║
// ║ • AnoContrato, NumeroContrato - Identificação do contrato                   ║
// ║ • DescricaoFornecedor - Empresa fornecedora                                 ║
// ║ • Sigla - Sigla da unidade                                                  ║
// ║                                                                              ║
// ║ Classificação:                                                               ║
// ║ • MotoristaCondutor - Nome concatenado para display                         ║
// ║ • TipoCondutor - Tipo (Titular, Reserva, Terceiro)                          ║
// ║ • EfetivoFerista - Classificação funcional                                  ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • DataAlteracao - Última atualização                                        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class ViewMotoristas
    {
        public Guid MotoristaId { get; set; }

        public string? Nome { get; set; }

        public string? MotoristaCondutor { get; set; }

        public string? Ponto { get; set; }

        public string? CNH { get; set; }

        public string? CategoriaCNH { get; set; }

        public string? Celular01 { get; set; }

        public bool Status { get; set; }

        public string? Sigla { get; set; }

        public string? AnoContrato { get; set; }

        public string? NumeroContrato { get; set; }

        public string? DescricaoFornecedor { get; set; }

        public string? NomeCompleto { get; set; }

        public string? TipoCondutor { get; set; }

        public string? EfetivoFerista { get; set; }

        public byte[]? Foto { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public Guid? ContratoId { get; set; }
    }
}
