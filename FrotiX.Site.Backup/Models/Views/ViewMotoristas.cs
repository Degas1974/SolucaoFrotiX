/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMotoristas.cs                                                                      ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de motoristas para listagens e filtros.                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: MotoristaId, Nome, CNH, CategoriaCNH, Celular01, Status, Sigla                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                        ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    /****************************************************************************************
     * ⚡ MODEL: ViewMotoristas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de motoristas com dados consolidados
     *
     * 📥 ENTRADAS     : Motorista, contrato, categoria, dados de contato
     *
     * 📤 SAÍDAS       : Registro somente leitura para listagens e filtros
     *
     * 🔗 CHAMADA POR  : Telas de gestão de motoristas e relatórios
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewMotoristas
    {
        // [DADOS] Identificador único do motorista
        public Guid MotoristaId { get; set; }

        // [DADOS] Nome do motorista
        public string? Nome { get; set; }

        // [DADOS] Tipo/classificação de condutor/motorista
        public string? MotoristaCondutor { get; set; }

        // [DADOS] Ponto/matrícula
        public string? Ponto { get; set; }

        // [DADOS] Número da CNH
        public string? CNH { get; set; }

        // [DADOS] Categoria da CNH (A, B, C, D, E)
        public string? CategoriaCNH { get; set; }

        // [DADOS] Telefone celular principal
        public string? Celular01 { get; set; }

        // [DADOS] Flag de status ativo/inativo
        public bool Status { get; set; }

        // [DADOS] Sigla da unidade/setor
        public string? Sigla { get; set; }

        // [DADOS] Ano do contrato vigente
        public string? AnoContrato { get; set; }

        // [DADOS] Número do contrato
        public string? NumeroContrato { get; set; }

        // [DADOS] Descrição do fornecedor (terceirizado)
        public string? DescricaoFornecedor { get; set; }

        // [DADOS] Nome completo do motorista
        public string? NomeCompleto { get; set; }

        // [DADOS] Tipo de condutor (motorista/cobrador/operador)
        public string? TipoCondutor { get; set; }

        // [DADOS] Classificação efetivo/ferista
        public string? EfetivoFerista { get; set; }

        // [DADOS] Foto do motorista (blob)
        public byte[]? Foto { get; set; }

        // [DADOS] Data da última alteração
        public DateTime? DataAlteracao { get; set; }

        // [DADOS] Identificador do contrato (FK nullable)
        public Guid? ContratoId { get; set; }
    }
}

