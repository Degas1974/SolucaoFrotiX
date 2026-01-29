/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/ItensContrato.cs                                        ║
 * ║  Descrição: ViewModels para gerenciamento de itens de contratos e atas   ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
    
    /// <summary>
    /// ViewModel principal para a página ItensContrato
    /// </summary>
    public class ICPageViewModel
    {
        public Guid ContratoId { get; set; }
        public Guid AtaId { get; set; }

        public ICPlaceholder ItensContrato { get; set; }

        // Listas para Dropdowns
        public IEnumerable<SelectListItem> ContratoList { get; set; }
        public IEnumerable<SelectListItem> AtaList { get; set; }
    }

    /// <summary>
    /// Placeholder para PageModel
    /// </summary>
    public class ICPlaceholder
    {
        [NotMapped]
        public Guid ContratoId { get; set; }

        [NotMapped]
        public Guid AtaId { get; set; }
    }

    // ============================================================
    // VIEWMODELS PARA MODAL DE INCLUSÃO
    // ============================================================

    /// <summary>
    /// ViewModel para inclusão de Veículo no Contrato
    /// </summary>
    public class ICIncluirVeiculoContratoVM
    {
        public Guid VeiculoId { get; set; }
        public Guid ContratoId { get; set; }
        public Guid? ItemVeiculoId { get; set; }
    }

    /// <summary>
    /// ViewModel para inclusão de Veículo na Ata
    /// </summary>
    public class ICIncluirVeiculoAtaVM
    {
        public Guid VeiculoId { get; set; }
        public Guid AtaId { get; set; }
        public Guid? ItemVeiculoAtaId { get; set; }
    }

    /// <summary>
    /// ViewModel para inclusão de Encarregado no Contrato
    /// </summary>
    public class ICIncluirEncarregadoContratoVM
    {
        public Guid EncarregadoId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para inclusão de Operador no Contrato
    /// </summary>
    public class ICIncluirOperadorContratoVM
    {
        public Guid OperadorId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para inclusão de Motorista no Contrato
    /// </summary>
    public class ICIncluirMotoristaContratoVM
    {
        public Guid MotoristaId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para inclusão de Lavador no Contrato
    /// </summary>
    public class ICIncluirLavadorContratoVM
    {
        public Guid LavadorId { get; set; }
        public Guid ContratoId { get; set; }
    }

    // ============================================================
    // VIEWMODELS PARA REMOÇÃO
    // ============================================================

    /// <summary>
    /// ViewModel para remoção de Veículo do Contrato
    /// </summary>
    public class ICRemoverVeiculoContratoVM
    {
        public Guid VeiculoId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para remoção de Veículo da Ata
    /// </summary>
    public class ICRemoverVeiculoAtaVM
    {
        public Guid VeiculoId { get; set; }
        public Guid AtaId { get; set; }
    }

    /// <summary>
    /// ViewModel para remoção de Encarregado do Contrato
    /// </summary>
    public class ICRemoverEncarregadoContratoVM
    {
        public Guid EncarregadoId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para remoção de Operador do Contrato
    /// </summary>
    public class ICRemoverOperadorContratoVM
    {
        public Guid OperadorId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para remoção de Motorista do Contrato
    /// </summary>
    public class ICRemoverMotoristaContratoVM
    {
        public Guid MotoristaId { get; set; }
        public Guid ContratoId { get; set; }
    }

    /// <summary>
    /// ViewModel para remoção de Lavador do Contrato
    /// </summary>
    public class ICRemoverLavadorContratoVM
    {
        public Guid LavadorId { get; set; }
        public Guid ContratoId { get; set; }
    }
}
