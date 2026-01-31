/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Veiculo.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Cadastrar veículos da frota com dados de identificação e vínculos.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: VeiculoViewModel, Veiculo                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core, SelectListItem, Validations                              ║
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
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: agregar veículo e listas de seleção para telas de cadastro/edição.
    // ==================================================================================================
    public class VeiculoViewModel
    {
        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Contrato selecionado (quando aplicável).
        public Guid? ContratoId { get; set; }

        // Entidade principal do formulário.
        public Veiculo? Veiculo { get; set; }

        // Nome do usuário responsável pela última alteração.
        public string? NomeUsuarioAlteracao { get; set; }

        // Status do veículo (ativo/inativo).
        public bool? Status { get; set; }

        // Indica se é veículo reserva.
        public bool? Reserva { get; set; }

        // Indica se faz parte do Economildo.
        public bool? Economildo { get; set; }

        // Indica se é veículo próprio.
        public bool? VeiculoProprio { get; set; }

        // Lista de marcas para seleção.
        public IEnumerable<SelectListItem>? MarcaList { get; set; }

        // Lista de modelos para seleção.
        public IEnumerable<SelectListItem>? ModeloList { get; set; }

        // Lista de unidades para seleção.
        public IEnumerable<SelectListItem>? UnidadeList { get; set; }

        // Lista de combustíveis para seleção.
        public IEnumerable<SelectListItem>? CombustivelList { get; set; }

        // Lista de contratos para seleção.
        public IEnumerable<SelectListItem>? ContratoList { get; set; }

        // Lista de atas para seleção.
        public IEnumerable<SelectListItem>? AtaList { get; set; }

        // Lista de usuários para seleção.
        public IEnumerable<SelectListItem>? AspNetUsersList { get; set; }

        // Lista de placas bronze para seleção.
        public IEnumerable<SelectListItem>? PlacaBronzeList { get; set; }

        // Lista de itens veiculares para seleção.
        public IEnumerable<SelectListItem>? ItemVeiculoList { get; set; }

        // ⚠️ ATENÇÃO: conversão ainda não implementada.
        public static implicit operator VeiculoViewModel(PlacaBronzeViewModel v)
        {
            throw new NotImplementedException();
        }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um veículo da frota.
    // ==================================================================================================
    public class Veiculo
    {
        // Identificador único do veículo.
        [Key]
        public Guid VeiculoId { get; set; }

        // Placa do veículo.
        [StringLength(10 , ErrorMessage = "A placa não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "Placa")]
        public string? Placa { get; set; }

        // Quilometragem atual.
        [Display(Name = "Quilometragem")]
        public int? Quilometragem { get; set; }

        // Renavam.
        [StringLength(20 , ErrorMessage = "O Renavam não pode exceder 20 caracteres")]
        [Display(Name = "Renavam")]
        public string? Renavam { get; set; }

        // Placa vinculada.
        [StringLength(20 , ErrorMessage = "A Placa Vinculada não pode exceder 20 caracteres")]
        [Display(Name = "Placa Vinculada")]
        public string? PlacaVinculada { get; set; }

        // Ano de fabricação.
        [ValidaLista(ErrorMessage = "(O ano de fabricação é obrigatório)")]
        [Display(Name = "Ano de Fabricacao")]
        public int? AnoFabricacao { get; set; }

        // Ano do modelo.
        [ValidaLista(ErrorMessage = "(O ano do modelo é obrigatório)")]
        [Display(Name = "Ano do Modelo")]
        public int? AnoModelo { get; set; }

        // Indica se é veículo reserva.
        [Display(Name = "Carro Reserva")]
        public bool Reserva { get; set; }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        // Indica se o veículo é próprio.
        [Display(Name = "Veículo Próprio")]
        public bool VeiculoProprio { get; set; }

        // Número patrimonial.
        [Display(Name = "Nº Patrimônio")]
        public string? Patrimonio { get; set; }

        // Categoria do veículo.
        [Display(Name = "Categoria")]
        public string? Categoria { get; set; }

        // CRLV em bytes.
        public byte[]? CRLV { get; set; }

        // Data da última alteração.
        public DateTime? DataAlteracao { get; set; }

        // Usuário responsável pela alteração.
        public string? UsuarioIdAlteracao { get; set; }

        // Placa de bronze associada.
        [Display(Name = "Placa de Bronze")]
        public Guid? PlacaBronzeId { get; set; }

        // Navegação para placa bronze.
        [ForeignKey("PlacaBronzeId")]
        public virtual PlacaBronze? PlacaBronze { get; set; }

        // Marca do veículo.
        [ValidaLista(ErrorMessage = "(A Marca é obrigatória)")]
        [Display(Name = "Marca do Veículo")]
        public Guid? MarcaId { get; set; }

        // Navegação para marca.
        [ForeignKey("MarcaId")]
        public virtual MarcaVeiculo? MarcaVeiculo { get; set; }

        // Modelo do veículo.
        [ValidaLista(ErrorMessage = "(O Modelo é obrigatório)")]
        [Display(Name = "Modelo do Veículo")]
        public Guid? ModeloId { get; set; }

        // Navegação para modelo.
        [ForeignKey("ModeloId")]
        public virtual ModeloVeiculo? ModeloVeiculo { get; set; }

        // Unidade vinculada.
        [Display(Name = "Unidade Vinculada")]
        public Guid? UnidadeId { get; set; }

        // Navegação para unidade.
        [ForeignKey("UnidadeId")]
        public virtual Unidade? Unidade { get; set; }

        // Tipo de combustível.
        [ValidaLista(ErrorMessage = "(O Tipo de Combustível é obrigatório)")]
        [Display(Name = "Combustível")]
        public Guid? CombustivelId { get; set; }

        // Navegação para combustível.
        [ForeignKey("CombustivelId")]
        public virtual Combustivel? Combustivel { get; set; }

        // Contrato associado.
        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }

        // Ata de registro de preços associada.
        [Display(Name = "Ata de Registro de Preços")]
        public Guid? AtaId { get; set; }

        // Navegação para ata.
        [ForeignKey("AtaId")]
        public virtual AtaRegistroPrecos? AtaRegistroPrecos { get; set; }

        // Item contratual associado.
        [Display(Name = "Item Contratual")]
        public Guid? ItemVeiculoId { get; set; }

        // Navegação para item contratual.
        [ForeignKey("ItemVeiculoId")]
        public virtual ItemVeiculoContrato? ItemVeiculoContrato { get; set; }

        // Item da ata associado.
        [Display(Name = "Item da Ata")]
        public Guid? ItemVeiculoAtaId { get; set; }

        // Navegação para item de ata.
        [ForeignKey("ItemVeiculoAtaId")]
        public virtual ItemVeiculoAta? ItemVeiculoAta { get; set; }

        // Data de ingresso na frota.
        [Display(Name = "Data de Ingresso na Frota")]
        public DateTime? DataIngresso { get; set; }

        // Indica se integra frota Economildo.
        [Display(Name = "Faz parte da Frota do Economildo?")]
        public bool Economildo { get; set; }

        // Valor mensal do veículo.
        public double? ValorMensal { get; set; }

        // Consumo médio.
        [Display(Name = "Consumo Médio")]
        public double? Consumo { get; set; }
    }
}
