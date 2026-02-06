/* ****************************************************************************************
 * ⚡ ARQUIVO: ExcelViewModel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar dados de importação/exportação de planilhas Excel.
 *
 * 📥 ENTRADAS     : Nome da planilha e dados serializados.
 *
 * 📤 SAÍDAS       : ViewModel para operações com Excel.
 *
 * 🔗 CHAMADA POR  : Fluxos de importação/exportação.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : System.Data, System.Collections.Generic.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: ExcelViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar informações de planilha para importação/exportação.
     *
     * 📥 ENTRADAS     : SheetName e Data.
     *
     * 📤 SAÍDAS       : Payload para processamento de planilhas.
     *
     * 🔗 CHAMADA POR  : Serviços de planilhas.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ExcelViewModel
    {
        // Nome da planilha.
        public string SheetName { get; set; }

        // Dados da planilha (conteúdo serializado).
        public string Data { get; set; }
    }
}

