/* ****************************************************************************************
 * ⚡ ARQUIVO: Combustivel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar tipos de combustível e médias de preço.
 *
 * 📥 ENTRADAS     : Dados de cadastro e chaves compostas de médias.
 *
 * 📤 SAÍDAS       : Entidades persistidas e ViewModel de apoio.
 *
 * 🔗 CHAMADA POR  : Cadastros, relatórios e estatísticas de abastecimento.
 *
 * 🔄 CHAMA        : DataAnnotations e Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: CombustivelViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Fornecer dados básicos para telas de cadastro de combustível.
     *
     * 📥 ENTRADAS     : Identificador do combustível.
     *
     * 📤 SAÍDAS       : ViewModel simplificado para UI.
     *
     * 🔗 CHAMADA POR  : Views e controllers de cadastro.
     ****************************************************************************************/
    public class CombustivelViewModel
        {
        public Guid CombustivelId { get; set; }
        }

    /****************************************************************************************
     * ⚡ MODEL: Combustivel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um tipo de combustível cadastrado no sistema.
     *
     * 📥 ENTRADAS     : Descrição e status.
     *
     * 📤 SAÍDAS       : Registro persistido.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations.
     ****************************************************************************************/
    public class Combustivel
        {

        [Key]
        public Guid CombustivelId { get; set; }

        [StringLength(50, ErrorMessage = "O combustível não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O tipo de combustível é obrigatório)")]
        [Display(Name = "Tipo de Combustível")]
        public string Descricao { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        }

    /****************************************************************************************
     * ⚡ MODEL: MediaCombustivel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar médias de preço por combustível, nota fiscal e período.
     *
     * 📥 ENTRADAS     : Identificadores e período (ano/mês).
     *
     * 📤 SAÍDAS       : Registro de média para consultas estatísticas.
     *
     * 🔗 CHAMADA POR  : Relatórios/estatísticas.
     *
     * 🔄 CHAMA        : DataAnnotations, Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta: NotaFiscalId + CombustivelId + Ano + Mes.
     ****************************************************************************************/
    public class MediaCombustivel
        {

        [Key, Column(Order = 0)]
        public Guid NotaFiscalId { get; set; }

        [Key, Column(Order = 1)]
        public Guid CombustivelId { get; set; }

        [Key, Column(Order = 2)]
        public int Ano { get; set; }

        [Key, Column(Order = 3)]
        public int Mes { get; set; }

        public double PrecoMedio { get; set; }

        }

    }
