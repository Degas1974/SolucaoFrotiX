/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewNoFichaVistoria.cs                                                                ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de fichas de vistoria pendentes (número de ficha).                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: NoFichaVistoria                                                                          ║
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
    /****************************************************************************************
     * ⚡ MODEL: ViewNoFichaVistoria
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de números de ficha de vistoria
     *
     * 📥 ENTRADAS     : Números sequenciais de fichas
     *
     * 📤 SAÍDAS       : Registro somente leitura para listagens
     *
     * 🔗 CHAMADA POR  : Dropdowns e filtros de busca
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewNoFichaVistoria
    {
        // [DADOS] Número da ficha de vistoria
        public int? NoFichaVistoria { get; set; }
    }
}

