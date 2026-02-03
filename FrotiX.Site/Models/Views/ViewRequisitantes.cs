/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewRequisitantes.cs                                                                  ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de requisitantes de viagens (nome e identificação).                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: RequisitanteId, Requisitante                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                       ║
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
     * ⚡ MODEL: ViewRequisitantes
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar requisitantes de viagens (pessoas/departamentos)
     *
     * 📥 ENTRADAS     : Identificação do requisitante
     *
     * 📤 SAÍDAS       : Registro somente leitura para dropdowns
     *
     * 🔗 CHAMADA POR  : Formulários de viagem e filtros
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewRequisitantes
    {
        // [DADOS] Identificador único do requisitante
        public Guid RequisitanteId { get; set; }

        // [DADOS] Nome do requisitante
        public string? Requisitante { get; set; }
    }
}


