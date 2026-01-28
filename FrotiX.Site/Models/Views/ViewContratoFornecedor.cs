// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewContratoFornecedor.cs                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo simples para dropdown de contratos com fornecedores.                 ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - ContratoId: Identificador do contrato                                     ║
// ║ - Descricao: Descrição do contrato                                          ║
// ║ - TipoContrato: Tipo (Locação, Terceirização, etc)                          ║
// ║                                                                              ║
// ║ USO: Combos de seleção de contratos em formulários                          ║
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
    public class ViewContratoFornecedor
        {

        public Guid ContratoId { get; set; }

        public string? Descricao { get; set; }

        public string? TipoContrato { get; set; }

        }
    }


