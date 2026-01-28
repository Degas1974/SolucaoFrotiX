// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewRequisitantes.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model simples para listagem de requisitantes de viagens.               ║
// ║ Usado em dropdowns e autocompletes para seleção de requisitantes.           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • RequisitanteId - Identificador único do requisitante                      ║
// ║ • Requisitante - Nome do requisitante para exibição                         ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Dropdowns de seleção em cadastro de viagens                               ║
// ║ • Filtros de relatórios por requisitante                                    ║
// ║ • Autocomplete em campos de busca                                           ║
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
    public class ViewRequisitantes
        {

        public Guid RequisitanteId { get; set; }

        public string? Requisitante { get; set; }

        }
    }


