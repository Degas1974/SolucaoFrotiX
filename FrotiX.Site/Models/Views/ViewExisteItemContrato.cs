// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewExisteItemContrato.cs                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para VIEW que verifica existência de itens em contratos.             ║
// ║ Usado para validar se veículo já está em repactuação.                       ║
// ║                                                                              ║
// ║ CAMPOS:                                                                      ║
// ║ - ItemVeiculoId: ID do item se existir                                      ║
// ║ - ExisteVeiculo: ID do veículo buscado                                      ║
// ║ - RepactuacaoContratoId: Repactuação onde está o item                       ║
// ║ - NumItem, Descricao, Quantidade, ValUnitario                               ║
// ║                                                                              ║
// ║ USO: Validação ao incluir veículos em contratos                             ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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


