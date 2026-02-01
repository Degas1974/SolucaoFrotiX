/* ****************************************************************************************
 * ⚡ ARQUIVO: ItensContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Reunir ViewModels de itens de contratos e atas (prefixo "IC").
 *
 * 📥 ENTRADAS     : Identificadores de contrato/ata/entidades e listas de seleção.
 *
 * 📤 SAÍDAS       : ViewModels usados em telas e modais de inclusão/remoção.
 *
 * 🔗 CHAMADA POR  : Páginas de Itens de Contratos e Atas.
 *
 * 🔄 CHAMA        : SelectListItem, NotMapped.
 *
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Mvc.Rendering,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ============================================================
    // VIEWMODEL PRINCIPAL - Tela Itens dos Contratos e Atas
    // Prefixo "IC" para evitar conflitos com classes existentes
    // ============================================================
    
    /****************************************************************************************
     * ⚡ VIEWMODEL: ICPageViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Centralizar dados e listas da página de Itens de Contratos/Atas.
     *
     * 📥 ENTRADAS     : ContratoId, AtaId e listas de seleção.
     *
     * 📤 SAÍDAS       : ViewModel completo para renderização da tela.
     *
     * 🔗 CHAMADA POR  : Views/Controllers de Itens de Contrato e Ata.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class ICPageViewModel
    {
        // Contrato selecionado.
        public Guid ContratoId { get; set; }
        // Ata selecionada.
        public Guid AtaId { get; set; }

        // Placeholder para itens.
        public ICPlaceholder ItensContrato { get; set; }

        // Listas para Dropdowns
        // Lista de contratos.
        public IEnumerable<SelectListItem> ContratoList { get; set; }
        // Lista de atas.
        public IEnumerable<SelectListItem> AtaList { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICPlaceholder
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Disponibilizar identificadores não mapeados para o PageModel.
     *
     * 📥 ENTRADAS     : ContratoId e AtaId.
     *
     * 📤 SAÍDAS       : Identificadores para controle de fluxo na UI.
     *
     * 🔗 CHAMADA POR  : PageModel de Itens de Contrato/Ata.
     *
     * 🔄 CHAMA        : NotMapped.
     ****************************************************************************************/
    public class ICPlaceholder
    {
        // Contrato selecionado (não mapeado).
        [NotMapped]
        public Guid ContratoId { get; set; }

        // Ata selecionada (não mapeado).
        [NotMapped]
        public Guid AtaId { get; set; }
    }

    // ============================================================
    // VIEWMODELS PARA MODAL DE INCLUSÃO
    // ============================================================

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICIncluirVeiculoContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar inclusão de veículo em contrato.
     *
     * 📥 ENTRADAS     : VeiculoId, ContratoId e ItemVeiculoId (opcional).
     *
     * 📤 SAÍDAS       : Payload para inclusão no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de inclusão de veículos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICIncluirVeiculoContratoVM
    {
        // Veículo a incluir.
        public Guid VeiculoId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
        // Item veicular associado (opcional).
        public Guid? ItemVeiculoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICIncluirVeiculoAtaVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar inclusão de veículo em ata.
     *
     * 📥 ENTRADAS     : VeiculoId, AtaId e ItemVeiculoAtaId (opcional).
     *
     * 📤 SAÍDAS       : Payload para inclusão na ata.
     *
     * 🔗 CHAMADA POR  : Modais de inclusão de veículos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICIncluirVeiculoAtaVM
    {
        // Veículo a incluir.
        public Guid VeiculoId { get; set; }
        // Ata de destino.
        public Guid AtaId { get; set; }
        // Item de ata associado (opcional).
        public Guid? ItemVeiculoAtaId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICIncluirEncarregadoContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar inclusão de encarregado em contrato.
     *
     * 📥 ENTRADAS     : EncarregadoId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para inclusão no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de inclusão de encarregados.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICIncluirEncarregadoContratoVM
    {
        // Encarregado a incluir.
        public Guid EncarregadoId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICIncluirOperadorContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar inclusão de operador em contrato.
     *
     * 📥 ENTRADAS     : OperadorId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para inclusão no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de inclusão de operadores.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICIncluirOperadorContratoVM
    {
        // Operador a incluir.
        public Guid OperadorId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICIncluirMotoristaContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar inclusão de motorista em contrato.
     *
     * 📥 ENTRADAS     : MotoristaId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para inclusão no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de inclusão de motoristas.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICIncluirMotoristaContratoVM
    {
        // Motorista a incluir.
        public Guid MotoristaId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICIncluirLavadorContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar inclusão de lavador em contrato.
     *
     * 📥 ENTRADAS     : LavadorId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para inclusão no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de inclusão de lavadores.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICIncluirLavadorContratoVM
    {
        // Lavador a incluir.
        public Guid LavadorId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    // ============================================================
    // VIEWMODELS PARA REMOÇÃO
    // ============================================================

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICRemoverVeiculoContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar remoção de veículo do contrato.
     *
     * 📥 ENTRADAS     : VeiculoId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para remoção no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de remoção de veículos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICRemoverVeiculoContratoVM
    {
        // Veículo a remover.
        public Guid VeiculoId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICRemoverVeiculoAtaVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar remoção de veículo da ata.
     *
     * 📥 ENTRADAS     : VeiculoId e AtaId.
     *
     * 📤 SAÍDAS       : Payload para remoção na ata.
     *
     * 🔗 CHAMADA POR  : Modais de remoção de veículos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICRemoverVeiculoAtaVM
    {
        // Veículo a remover.
        public Guid VeiculoId { get; set; }
        // Ata de origem.
        public Guid AtaId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICRemoverEncarregadoContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar remoção de encarregado do contrato.
     *
     * 📥 ENTRADAS     : EncarregadoId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para remoção no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de remoção de encarregados.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICRemoverEncarregadoContratoVM
    {
        // Encarregado a remover.
        public Guid EncarregadoId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICRemoverOperadorContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar remoção de operador do contrato.
     *
     * 📥 ENTRADAS     : OperadorId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para remoção no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de remoção de operadores.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICRemoverOperadorContratoVM
    {
        // Operador a remover.
        public Guid OperadorId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICRemoverMotoristaContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar remoção de motorista do contrato.
     *
     * 📥 ENTRADAS     : MotoristaId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para remoção no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de remoção de motoristas.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICRemoverMotoristaContratoVM
    {
        // Motorista a remover.
        public Guid MotoristaId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    /****************************************************************************************
     * ⚡ VIEWMODEL: ICRemoverLavadorContratoVM
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar remoção de lavador do contrato.
     *
     * 📥 ENTRADAS     : LavadorId e ContratoId.
     *
     * 📤 SAÍDAS       : Payload para remoção no contrato.
     *
     * 🔗 CHAMADA POR  : Modais de remoção de lavadores.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ICRemoverLavadorContratoVM
    {
        // Lavador a remover.
        public Guid LavadorId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }
}
