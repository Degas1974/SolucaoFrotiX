/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ExcelViewModel.cs                                                                      ║
    ║ 📂 CAMINHO: /Models/Planilhas                                                                       ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: ViewModels para importação/exportação de planilhas Excel.                              ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 CLASSE: ExcelViewModel (SheetName, Data)                                                         ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System.Data, System.Collections.Generic                                                    ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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


