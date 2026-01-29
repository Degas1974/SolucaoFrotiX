/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Planilhas/ExcelViewModel.cs                             ║
 * ║  Descrição: ViewModels para importação/exportação de planilhas Excel     ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    public class ExcelViewModel
        {
        public string SheetName { get; set; }

        public string Data { get; set; }
        }
    }


