/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Veiculo.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para cadastro de veículos da frota (placa, modelo, combustível).║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: Veiculo (VeiculoId, Placa, Marca, Modelo), VeiculoViewModel (MarcaList, UnidadeList)    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations, SelectListItem                                        ║
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class VeiculoViewModel
    {
        public Guid VeiculoId { get; set; }
        public Guid? ContratoId { get; set; }
        public Veiculo? Veiculo { get; set; }
        public string? NomeUsuarioAlteracao { get; set; }
        public bool? Status { get; set; }
        public bool? Reserva { get; set; }
        public bool? Economildo { get; set; }
        public bool? VeiculoProprio { get; set; }
        public IEnumerable<SelectListItem>? MarcaList { get; set; }
        public IEnumerable<SelectListItem>? ModeloList { get; set; }
        public IEnumerable<SelectListItem>? UnidadeList { get; set; }
        public IEnumerable<SelectListItem>? CombustivelList { get; set; }
        public IEnumerable<SelectListItem>? ContratoList { get; set; }
        public IEnumerable<SelectListItem>? AtaList { get; set; }
        public IEnumerable<SelectListItem>? AspNetUsersList { get; set; }
        public IEnumerable<SelectListItem>? PlacaBronzeList { get; set; }
        public IEnumerable<SelectListItem>? ItemVeiculoList { get; set; }

        public static implicit operator VeiculoViewModel(PlacaBronzeViewModel v)
        {
            throw new NotImplementedException();
        }
    }

    public class Veiculo
    {
        [Key]
        public Guid VeiculoId { get; set; }

        [StringLength(10 , ErrorMessage = "A placa não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "Placa")]
        public string? Placa { get; set; }

        [Display(Name = "Quilometragem")]
        public int? Quilometragem { get; set; }

        [StringLength(20 , ErrorMessage = "O Renavam não pode exceder 20 caracteres")]
        [Display(Name = "Renavam")]
        public string? Renavam { get; set; }

        [StringLength(20 , ErrorMessage = "A Placa Vinculada não pode exceder 20 caracteres")]
        [Display(Name = "Placa Vinculada")]
        public string? PlacaVinculada { get; set; }

        [ValidaLista(ErrorMessage = "(O ano de fabricação é obrigatório)")]
        [Display(Name = "Ano de Fabricacao")]
        public int? AnoFabricacao { get; set; }

        [ValidaLista(ErrorMessage = "(O ano do modelo é obrigatório)")]
        [Display(Name = "Ano do Modelo")]
        public int? AnoModelo { get; set; }

        [Display(Name = "Carro Reserva")]
        public bool Reserva { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        [Display(Name = "Veículo Próprio")]
        public bool VeiculoProprio { get; set; }

        [Display(Name = "Nº Patrimônio")]
        public string? Patrimonio { get; set; }

        [Display(Name = "Categoria")]
        public string? Categoria { get; set; }

        public byte[]? CRLV { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public string? UsuarioIdAlteracao { get; set; }

        [Display(Name = "Placa de Bronze")]
        public Guid? PlacaBronzeId { get; set; }

        [ForeignKey("PlacaBronzeId")]
        public virtual PlacaBronze? PlacaBronze { get; set; }

        [ValidaLista(ErrorMessage = "(A Marca é obrigatória)")]
        [Display(Name = "Marca do Veículo")]
        public Guid? MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public virtual MarcaVeiculo? MarcaVeiculo { get; set; }

        [ValidaLista(ErrorMessage = "(O Modelo é obrigatório)")]
        [Display(Name = "Modelo do Veículo")]
        public Guid? ModeloId { get; set; }

        [ForeignKey("ModeloId")]
        public virtual ModeloVeiculo? ModeloVeiculo { get; set; }

        [Display(Name = "Unidade Vinculada")]
        public Guid? UnidadeId { get; set; }

        [ForeignKey("UnidadeId")]
        public virtual Unidade? Unidade { get; set; }

        [ValidaLista(ErrorMessage = "(O Tipo de Combustível é obrigatório)")]
        [Display(Name = "Combustível")]
        public Guid? CombustivelId { get; set; }

        [ForeignKey("CombustivelId")]
        public virtual Combustivel? Combustivel { get; set; }

        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }

        [Display(Name = "Ata de Registro de Preços")]
        public Guid? AtaId { get; set; }

        [ForeignKey("AtaId")]
        public virtual AtaRegistroPrecos? AtaRegistroPrecos { get; set; }

        [Display(Name = "Item Contratual")]
        public Guid? ItemVeiculoId { get; set; }

        [ForeignKey("ItemVeiculoId")]
        public virtual ItemVeiculoContrato? ItemVeiculoContrato { get; set; }

        [Display(Name = "Item da Ata")]
        public Guid? ItemVeiculoAtaId { get; set; }

        [ForeignKey("ItemVeiculoAtaId")]
        public virtual ItemVeiculoAta? ItemVeiculoAta { get; set; }

        [Display(Name = "Data de Ingresso na Frota")]
        public DateTime? DataIngresso { get; set; }

        [Display(Name = "Faz parte da Frota do Economildo?")]
        public bool Economildo { get; set; }

        public double? ValorMensal { get; set; }

        [Display(Name = "Consumo Médio")]
        public double? Consumo { get; set; }
    }
}
