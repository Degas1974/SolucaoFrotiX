// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ExcelViewModel.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para exportação de dados para Excel via Syncfusion.                  ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - SheetName: Nome da aba/planilha                                           ║
// ║ - Data: Dados serializados em JSON para exportação                          ║
// ║                                                                              ║
// ║ USO: Exportação de grids e relatórios para formato .xlsx                    ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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


