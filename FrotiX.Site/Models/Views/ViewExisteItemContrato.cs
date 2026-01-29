/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewExisteItemContrato.cs                                                              ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL para verificação de itens de contrato (existência e valores).                ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: ItemVeiculoId, ExisteVeiculo, RepactuacaoContratoId, NumItem, Quantidade, ValUnitario     ║
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
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {
    public class ViewExisteItemContrato
        {

        public Guid ItemVeiculoId { get; set; }

        public Guid ExisteVeiculo { get; set; }

        public Guid RepactuacaoContratoId { get; set; }

        public int? NumItem { get; set; }

        public string? Descricao { get; set; }

        public int? Quantidade { get; set; }

        public double? ValUnitario { get; set; }

        }
    }


