/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ItensContrato.cs                                                                        ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: ViewModels para gerenciamento de itens de contratos e atas (prefixo "IC").            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ICPageViewModel, ICPlaceholder, VMs de inclusão/remoção                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: SelectListItem                                                                      ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    
    // ViewModel principal para a página ItensContrato.
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

    // Placeholder para PageModel.
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

    // ViewModel para inclusão de Veículo no Contrato.
    public class ICIncluirVeiculoContratoVM
    {
        // Veículo a incluir.
        public Guid VeiculoId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
        // Item veicular associado (opcional).
        public Guid? ItemVeiculoId { get; set; }
    }

    // ViewModel para inclusão de Veículo na Ata.
    public class ICIncluirVeiculoAtaVM
    {
        // Veículo a incluir.
        public Guid VeiculoId { get; set; }
        // Ata de destino.
        public Guid AtaId { get; set; }
        // Item de ata associado (opcional).
        public Guid? ItemVeiculoAtaId { get; set; }
    }

    // ViewModel para inclusão de Encarregado no Contrato.
    public class ICIncluirEncarregadoContratoVM
    {
        // Encarregado a incluir.
        public Guid EncarregadoId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para inclusão de Operador no Contrato.
    public class ICIncluirOperadorContratoVM
    {
        // Operador a incluir.
        public Guid OperadorId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para inclusão de Motorista no Contrato.
    public class ICIncluirMotoristaContratoVM
    {
        // Motorista a incluir.
        public Guid MotoristaId { get; set; }
        // Contrato de destino.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para inclusão de Lavador no Contrato.
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

    // ViewModel para remoção de Veículo do Contrato.
    public class ICRemoverVeiculoContratoVM
    {
        // Veículo a remover.
        public Guid VeiculoId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para remoção de Veículo da Ata.
    public class ICRemoverVeiculoAtaVM
    {
        // Veículo a remover.
        public Guid VeiculoId { get; set; }
        // Ata de origem.
        public Guid AtaId { get; set; }
    }

    // ViewModel para remoção de Encarregado do Contrato.
    public class ICRemoverEncarregadoContratoVM
    {
        // Encarregado a remover.
        public Guid EncarregadoId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para remoção de Operador do Contrato.
    public class ICRemoverOperadorContratoVM
    {
        // Operador a remover.
        public Guid OperadorId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para remoção de Motorista do Contrato.
    public class ICRemoverMotoristaContratoVM
    {
        // Motorista a remover.
        public Guid MotoristaId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }

    // ViewModel para remoção de Lavador do Contrato.
    public class ICRemoverLavadorContratoVM
    {
        // Lavador a remover.
        public Guid LavadorId { get; set; }
        // Contrato de origem.
        public Guid ContratoId { get; set; }
    }
}
