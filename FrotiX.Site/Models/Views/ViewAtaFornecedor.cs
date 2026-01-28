// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewAtaFornecedor.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo simples para dropdown de atas de registro de preços.                 ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - AtaId: Identificador da ata                                               ║
// ║ - AtaVeiculo: Descrição para exibição no dropdown                           ║
// ║                                                                              ║
// ║ USO: Combos de seleção de atas em formulários                               ║
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
    public class ViewAtaFornecedor
        {

        public Guid AtaId { get; set; }

        public string? AtaVeiculo { get; set; }

        }
    }


