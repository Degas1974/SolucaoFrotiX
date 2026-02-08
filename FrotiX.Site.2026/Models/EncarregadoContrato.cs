/* ****************************************************************************************
 * ⚡ ARQUIVO: EncarregadoContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre Encarregado e Contrato via chave composta.
 *
 * 📥 ENTRADAS     : EncarregadoId e ContratoId.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento e ViewModel de apoio.
 *
 * 🔗 CHAMADA POR  : Telas de edição de vínculos e repositórios.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Column).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: EncarregadoContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar vínculo encarregado-contrato nas telas de edição.
     *
     * 📥 ENTRADAS     : EncarregadoId, ContratoId e entidade do vínculo.
     *
     * 📤 SAÍDAS       : ViewModel para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de contratos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class EncarregadoContratoViewModel
    {
        // Identificador do encarregado.
        public Guid EncarregadoId { get; set; }
        // Identificador do contrato.
        public Guid ContratoId { get; set; }
        // Entidade do vínculo.
        public EncarregadoContrato? EncarregadoContrato { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: EncarregadoContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar relacionamento N:N entre Encarregado e Contrato.
     *
     * 📥 ENTRADAS     : EncarregadoId e ContratoId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Repositórios e serviços de contratos.
     *
     * 🔄 CHAMA        : Key, Column.
     *
     * ⚠️ ATENÇÃO      : Chave composta (EncarregadoId + ContratoId).
     ****************************************************************************************/
    public class EncarregadoContrato
    {
        // Chave composta - FK para Encarregado.
        [Key, Column(Order = 0)]
        public Guid EncarregadoId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
